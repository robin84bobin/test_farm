//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2015 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Popup list can be used to display pop-up menus and drop-down lists.
/// </summary>

[ExecuteInEditMode]
[AddComponentMenu("NGUI/Interaction/Popup List")]
public class UIPopupList : UIWidgetContainer
{
	/// <summary>
	/// Current popup list. Only available during the OnSelectionChange event callback.
	/// </summary>

	static public UIPopupList current;
	static GameObject mChild;
	static float mFadeOutComplete = 0f;

	const float animSpeed = 0.15f;

	// MS_ADD
	public bool InheritDepth = false;
	public bool AlignDropdownCenter = false;
	public bool AutocChooseFirstItem = true;

	public enum Position
	{
		Auto,
		Above,
		Below,
	}

	/// <summary>
	/// Atlas used by the sprites.
	/// </summary>

	public UIAtlas atlas;

	/// <summary>
	/// Font used by the labels.
	/// </summary>

	public UIFont bitmapFont;

	/// <summary>
	/// True type font used by the labels. Alternative to specifying a bitmap font ('font').
	/// </summary>

	public Font trueTypeFont;

	/// <summary>
	/// Font used by the popup list. Conveniently wraps both dynamic and bitmap fonts into one property.
	/// </summary>

	public Object ambigiousFont
	{
		get
		{
			if (trueTypeFont != null) return trueTypeFont;
			if (bitmapFont != null) return bitmapFont;
			return font;
		}
		set
		{
			if (value is Font) {
				trueTypeFont = value as Font;
				bitmapFont = null;
				font = null;
			} else if (value is UIFont) {
				bitmapFont = value as UIFont;
				trueTypeFont = null;
				font = null;
			}
		}
	}

	/// <summary>
	/// Size of the font to use for the popup list's labels.
	/// </summary>

	public int fontSize = 16;

	/// <summary>
	/// Font style used by the dynamic font.
	/// </summary>

	public FontStyle fontStyle = FontStyle.Normal;

	/// <summary>
	/// Name of the sprite used to create the popup's background.
	/// </summary>

	public string backgroundSprite;

	/// <summary>
	/// Name of the sprite used to highlight items.
	/// </summary>

	public string highlightSprite;

	public string itemNormalSprite;
	public string highlightSideSprite;
	public string itemNormalSideSprite;
	public string highlightSideDownSprite;
	public string itemNormalSideDownSprite;

	/// <summary>
	/// Popup list's display style.
	/// </summary>

	public Position position = Position.Auto;

	/// <summary>
	/// Label alignment to use.
	/// </summary>

	public NGUIText.Alignment alignment = NGUIText.Alignment.Left;

	/// <summary>
	/// New line-delimited list of items.
	/// </summary>

	public List<string> items = new List<string>();

	/// <summary>
	/// You can associate arbitrary _messageViewData to be associated with your entries if you like.
	/// The only downside is that this must be done via code.
	/// </summary>

	public List<object> itemData = new List<object>();

	/// <summary>
	/// Amount of padding added to labels.
	/// </summary>

	public Vector2 padding = new Vector3(4f, 4f);
	
	public float itemHorizontalPadding = 0f;

	/// <summary>
	/// Color tint applied to labels inside the list.
	/// </summary>

	public Color textColor = Color.white;

	public Color highlightTextColor = Color.white;

	/// <summary>
	/// Color tint applied to the background.
	/// </summary>

	public Color backgroundColor = Color.white;

	/// <summary>
	/// Color tint applied to the highlighter.
	/// </summary>

	public Color highlightColor = new Color(225f / 255f, 200f / 255f, 150f / 255f, 1f);

	/// <summary>
	/// Whether the popup list is animated or not. Disable for better performance.
	/// </summary>

	public bool isAnimated = true;

	/// <summary>
	/// Whether the popup list's values will be localized.
	/// </summary>

	public bool isLocalized = false;

	/// <summary>
	/// Whether a separate panel will be used to ensure that the popup will appear on top of everything else.
	/// </summary>

	public bool separatePanel = true;

	public enum OpenOn
	{
		ClickOrTap,
		RightClick,
		DoubleClick,
		Manual,
	}

	/// <summary>
	/// What kind of click is needed in order to open the popup list.
	/// </summary>

	public OpenOn openOn = OpenOn.ClickOrTap;

	/// <summary>
	/// Callbacks triggered when the popup list gets a new item selection.
	/// </summary>

	public List<EventDelegate> onChange = new List<EventDelegate>();
	public List<EventDelegate> onClose = new List<EventDelegate>();
	public List<EventDelegate> onOpen = new List<EventDelegate>();

	// Currently selected item
	[HideInInspector]
	[SerializeField]
	string mSelectedItem;
	[HideInInspector]
	[SerializeField]
	UIPanel mPanel;
	[HideInInspector]
	[SerializeField]
	UISprite mBackground;
	[HideInInspector]
	[SerializeField]
	UISprite mHighlight;
	[SerializeField]
	UISprite mHighlightSide;
	[SerializeField]
	UISprite mHighlightSideDown;
	[HideInInspector]
	[SerializeField]
	UILabel mHighlightedLabel = null;
	[HideInInspector]
	[SerializeField]
	List<UILabel> mLabelList = new List<UILabel>();
	[HideInInspector]
	[SerializeField]
	float mBgBorder = 0f;

	[System.NonSerialized]
	GameObject mSelection;
	[System.NonSerialized]
	int mOpenFrame = 0;

	// Deprecated functionality
	[HideInInspector]
	[SerializeField]
	GameObject eventReceiver;
	[HideInInspector]
	[SerializeField]
	string functionName = "OnSelectionChange";
	[HideInInspector]
	[SerializeField]
	float textScale = 0f;
	[HideInInspector]
	[SerializeField]
	UIFont font; // Use 'bitmapFont' instead

	// This functionality is no longer needed as the same can be achieved by choosing a
	// OnValueChange notification targeting a label's SetCurrentSelection function.
	// If your code was list.textLabel = myLabel, change it to:
	// EventDelegate.Add(list.onChange, lbl.SetCurrentSelection);
	[HideInInspector]
	[SerializeField]
	UILabel textLabel;

	public delegate void LegacyEvent(string val);
	LegacyEvent mLegacyEvent;

	[System.Obsolete("Use EventDelegate.Add(popup.onChange, YourCallback) instead, and UIPopupList.current.value to determine the state")]
	public LegacyEvent onSelectionChange { get { return mLegacyEvent; } set { mLegacyEvent = value; } }

	/// <summary>
	/// Whether the popup list is currently open.
	/// </summary>

	static public bool isOpen { get { return current != null && (mChild != null || mFadeOutComplete > Time.unscaledTime); } }

	/// <summary>
	/// Current selection.
	/// </summary>

	public string value
	{
		get
		{
			return mSelectedItem;
		}
		set
		{
			mSelectedItem = value;
			if (mSelectedItem == null) return;
#if UNITY_EDITOR
			if (!Application.isPlaying) return;
#endif
			if (mSelectedItem != null)
				TriggerCallbacks();
		}
	}

	/// <summary>
	/// DataItem _messageViewData associated with the current selection.
	/// </summary>

	public object data
	{
		get
		{
			int index = items.IndexOf(mSelectedItem);
			return (index > -1) && index < itemData.Count ? itemData[index] : null;
		}
	}

	/// <summary>
	/// Whether the collider is enabled and the widget can be interacted with.
	/// </summary>

	public bool isColliderEnabled
	{
		get
		{
			Collider c = GetComponent<Collider>();
			if (c != null) return c.enabled;
			Collider2D b = GetComponent<Collider2D>();
			return (b != null && b.enabled);
		}
	}

	[System.Obsolete("Use 'value' instead")]
	public string selection { get { return value; } set { this.value = value; } }

	/// <summary>
	/// Whether the popup list is actually usable.
	/// </summary>

	bool isValid { get { return bitmapFont != null || trueTypeFont != null; } }

	/// <summary>
	/// Active font size.
	/// </summary>

	int activeFontSize { get { return (trueTypeFont != null || bitmapFont == null) ? fontSize : bitmapFont.defaultSize; } }

	/// <summary>
	/// Font scale applied to the popup list's text.
	/// </summary>

	float activeFontScale { get { return (trueTypeFont != null || bitmapFont == null) ? 1f : (float)fontSize / bitmapFont.defaultSize; } }


	private List<UILabel> labels;
	/// <summary>
	/// Clear the popup list's contents.
	/// </summary>

	public void Clear()
	{
		items.Clear();
		itemData.Clear();
	}

	/// <summary>
	/// Add a new item to the popup list.
	/// </summary>

	public void AddItem(string text)
	{
		items.Add(text);
		itemData.Add(null);
	}

	/// <summary>
	/// Add a new item to the popup list.
	/// </summary>

	public void AddItem(string text, object data)
	{
		items.Add(text);
		itemData.Add(data);
	}

	/// <summary>
	/// Remove the specified item.
	/// </summary>

	public void RemoveItem(string text)
	{
		int index = items.IndexOf(text);

		if (index != -1) {
			items.RemoveAt(index);
			itemData.RemoveAt(index);
		}
	}

	/// <summary>
	/// Remove the specified item.
	/// </summary>

	public void RemoveItemByData(object data)
	{
		int index = itemData.IndexOf(data);

		if (index != -1) {
			items.RemoveAt(index);
			itemData.RemoveAt(index);
		}
	}

	[System.NonSerialized]
	bool mExecuting = false;

	/// <summary>
	/// Trigger all event notification callbacks.
	/// </summary>

	protected void TriggerCallbacks()
	{
		if (!mExecuting) {
			mExecuting = true;
			UIPopupList old = current;
			current = this;

			// Legacy functionality
			if (mLegacyEvent != null) mLegacyEvent(mSelectedItem);

			if (EventDelegate.IsValid(onChange)) {
				EventDelegate.Execute(onChange);
			} else if (eventReceiver != null && !string.IsNullOrEmpty(functionName)) {
				// Legacy functionality support (for backwards compatibility)
				eventReceiver.SendMessage(functionName, mSelectedItem, SendMessageOptions.DontRequireReceiver);
			}
			current = old;
			mExecuting = false;
		}
	}

	/// <summary>
	/// Remove legacy functionality.
	/// </summary>

	void OnEnable()
	{
		if (EventDelegate.IsValid(onChange)) {
			eventReceiver = null;
			functionName = null;
		}

		// 'font' is no longer used
		if (font != null) {
			if (font.isDynamic) {
				trueTypeFont = font.dynamicFont;
				fontStyle = font.dynamicFontStyle;
				mUseDynamicFont = true;
			} else if (bitmapFont == null) {
				bitmapFont = font;
				mUseDynamicFont = false;
			}
			font = null;
		}

		// 'textScale' is no longer used
		if (textScale != 0f) {
			fontSize = (bitmapFont != null) ? Mathf.RoundToInt(bitmapFont.defaultSize * textScale) : 16;
			textScale = 0f;
		}

		// Auto-upgrade to the true type font
		if (trueTypeFont == null && bitmapFont != null && bitmapFont.isDynamic) {
			trueTypeFont = bitmapFont.dynamicFont;
			bitmapFont = null;
		}
	}

	bool mUseDynamicFont = false;

	void OnValidate()
	{
		Font ttf = trueTypeFont;
		UIFont fnt = bitmapFont;

		bitmapFont = null;
		trueTypeFont = null;

		if (ttf != null && (fnt == null || !mUseDynamicFont)) {
			bitmapFont = null;
			trueTypeFont = ttf;
			mUseDynamicFont = true;
		} else if (fnt != null) {
			// Auto-upgrade from 3.0.2 and earlier
			if (fnt.isDynamic) {
				trueTypeFont = fnt.dynamicFont;
				fontStyle = fnt.dynamicFontStyle;
				fontSize = fnt.defaultSize;
				mUseDynamicFont = true;
			} else {
				bitmapFont = fnt;
				mUseDynamicFont = false;
			}
		} else {
			trueTypeFont = ttf;
			mUseDynamicFont = true;
		}
	}

	/// <summary>
	/// Send out the selection message on start.
	/// </summary>

	protected void Start()
	{
		// Auto-upgrade legacy functionality
		if (textLabel != null) {
			EventDelegate.Add(onChange, textLabel.SetCurrentSelection);
			textLabel = null;
#if UNITY_EDITOR
			NGUITools.SetDirty(this);
#endif
		}

		// Automatically choose the first item
		if (Application.isPlaying && AutocChooseFirstItem) {
			if (string.IsNullOrEmpty(mSelectedItem) && items.Count > 0)
				mSelectedItem = items[0];
			if (!string.IsNullOrEmpty(mSelectedItem))
				TriggerCallbacks();
		}
	}

	/// <summary>
	/// Localize the text label.
	/// </summary>

	void OnLocalize() { if (isLocalized) TriggerCallbacks(); }

	/// <summary>
	/// Visibly highlight the specified transform by moving the highlight sprite to be over it.
	/// </summary>

	UILabel oldLbl;
	void Highlight(UILabel lbl, bool instant)
	{
		if (mHighlight != null) {
			mHighlightedLabel = lbl;
			int index = labels.IndexOf(lbl);

			UISpriteData sp = mHighlight.GetAtlasSprite();
			if (sp == null) return;

			Vector3 pos = GetHighlightPosition();

			if (oldLbl != null) {
				oldLbl.color = textColor;
			}
			oldLbl = lbl;
			lbl.color = highlightTextColor;

			if (index == 0 ) {
				mHighlight.enabled = false;
				mHighlightSide.enabled = true;
				mHighlightSideDown.enabled = false;
			} else if (index == labels.Count - 1) {
				mHighlight.enabled = false;
				mHighlightSide.enabled = false;
				mHighlightSideDown.enabled = true;
			} else {
				mHighlight.enabled = true;
				mHighlightSide.enabled = false;
				mHighlightSideDown.enabled = false;
			}

			if (instant || !isAnimated || !mHighlight.enabled) {
				mHighlight.cachedTransform.localPosition = pos;
				mHighlightSide.cachedTransform.localPosition = pos;
				mHighlightSideDown.cachedTransform.localPosition = pos;
			} else {
				TweenPosition.Begin(mHighlight.gameObject, 0.1f, pos).method = UITweener.Method.EaseOut;

				if (!mTweening) {
					mTweening = true;
					StartCoroutine("UpdateTweenPosition");
				}
			}
			//else mHighlight.cachedTransform.localPosition = pos;
		}
	}

	/// <summary>
	/// Helper function that calculates where the tweened position should be.
	/// </summary>

	Vector3 GetHighlightPosition()
	{
		if (mHighlightedLabel == null || mHighlight == null) return Vector3.zero;
		UISpriteData sp = mHighlight.GetAtlasSprite();
		if (sp == null) return Vector3.zero;

		float scaleFactor = atlas.pixelSize;
		float offsetX = sp.borderLeft * scaleFactor;
		float offsetY = sp.borderTop * scaleFactor;

		var localPosition = mHighlightedLabel.cachedTransform.localPosition;
		localPosition.y += padding.y;

		// MS_ADD
		if (AlignDropdownCenter) {
			Vector4 bgPadding = mBackground.border;
			localPosition.x = bgPadding.x;
		}

		return localPosition + new Vector3(-offsetX - itemHorizontalPadding, offsetY, 1f);
	}

	bool mTweening = false;

	/// <summary>
	/// Periodically update the tweened target position.
	/// It's needed because the popup list animates into view, and the target position changes.
	/// </summary>

	IEnumerator UpdateTweenPosition()
	{
		if (mHighlight != null && mHighlightedLabel != null) {
			TweenPosition tp = mHighlight.GetComponent<TweenPosition>();

			while (tp != null && tp.enabled) {
				tp.to = GetHighlightPosition();
				yield return null;
			}
		}
		mTweening = false;
	}

	/// <summary>
	/// Event function triggered when the mouse hovers over an item.
	/// </summary>

	void OnItemHover(GameObject go, bool isOver)
	{
		if (isOver) {
			UILabel lbl = go.GetComponent<UILabel>();
			Highlight(lbl, false);
		}
	}

	/// <summary>
	/// Event function triggered when the drop-down list item gets clicked on.
	/// </summary>

	void OnItemPress(GameObject go, bool isPressed)
	{
		if (isPressed) {
			Select(go.GetComponent<UILabel>(), true);

			UIEventListener listener = go.GetComponent<UIEventListener>();
			value = listener.parameter as string;
			UIPlaySound[] sounds = GetComponents<UIPlaySound>();

			for (int i = 0, imax = sounds.Length; i < imax; ++i) {
				UIPlaySound snd = sounds[i];
				if (snd.trigger == UIPlaySound.Trigger.OnClick)
					NGUITools.PlaySound(snd.audioClip, snd.volume, 1f);
			}
			CloseSelf();
		}
	}

	/// <summary>
	/// Select the specified label.
	/// </summary>

	void Select(UILabel lbl, bool instant) { Highlight(lbl, instant); }

	/// <summary>
	/// React to key-based input.
	/// </summary>

	void OnNavigate(KeyCode key)
	{
		if (enabled && current == this) {
			int index = mLabelList.IndexOf(mHighlightedLabel);
			if (index == -1) index = 0;

			if (key == KeyCode.UpArrow) {
				if (index > 0) {
					Select(mLabelList[--index], false);
				}
			} else if (key == KeyCode.DownArrow) {
				if (index + 1 < mLabelList.Count) {
					Select(mLabelList[++index], false);
				}
			}
		}
	}

	/// <summary>
	/// React to key-based input.
	/// </summary>

	void OnKey(KeyCode key)
	{
		if (enabled && current == this) {
			if (key == UICamera.current.cancelKey0 || key == UICamera.current.cancelKey1)
				OnSelect(false);
		}
	}

	/// <summary>
	/// Close the popup list when disabled.
	/// </summary>

	void OnDisable() { CloseSelf(); }

	/// <summary>
	/// Get rid of the popup dialog when the selection gets lost.
	/// </summary>

	void OnSelect(bool isSelected) { if (!isSelected) CloseSelf(); }

	/// <summary>
	/// Manually close the popup list.
	/// </summary>

	static public void Close()
	{
		if (current != null) {
			current.CloseSelf();
			current = null;
		}
	}

	/// <summary>
	/// Manually close the popup list.
	/// </summary>

	public void CloseSelf()
	{
		if (mChild != null && current == this) {
			StopCoroutine("CloseIfUnselected");
			mSelection = null;

			mLabelList.Clear();

			if (isAnimated) {
				UIWidget[] widgets = mChild.GetComponentsInChildren<UIWidget>();

				for (int i = 0, imax = widgets.Length; i < imax; ++i) {
					UIWidget w = widgets[i];
					Color c = w.color;
					c.a = 0f;
					TweenColor.Begin(w.gameObject, animSpeed, c).method = UITweener.Method.EaseOut;
				}

				Collider[] cols = mChild.GetComponentsInChildren<Collider>();
				for (int i = 0, imax = cols.Length; i < imax; ++i) cols[i].enabled = false;
				Destroy(mChild, animSpeed);

				mFadeOutComplete = Time.unscaledTime + Mathf.Max(0.1f, animSpeed);
			} else {
				Destroy(mChild);
				mFadeOutComplete = Time.unscaledTime + 0.1f;
			}

			mBackground = null;
			mHighlight = null;
			mHighlightSide = null;
			mHighlightSideDown = null;
			mChild = null;
			current = null;
		}

		EventDelegate.Execute(onClose);
	}

	/// <summary>
	/// Helper function that causes the widget to smoothly fade in.
	/// </summary>

	void AnimateColor(UIWidget widget)
	{
		Color c = widget.color;
		widget.color = new Color(c.r, c.g, c.b, 0f);
		TweenColor.Begin(widget.gameObject, animSpeed, c).method = UITweener.Method.EaseOut;
	}

	/// <summary>
	/// Helper function that causes the widget to smoothly move into position.
	/// </summary>

	void AnimatePosition(UIWidget widget, bool placeAbove, float bottom)
	{
		Vector3 target = widget.cachedTransform.localPosition;
		Vector3 start = placeAbove ? new Vector3(target.x, bottom, target.z) : new Vector3(target.x, 0f, target.z);

		widget.cachedTransform.localPosition = start;

		GameObject go = widget.gameObject;
		TweenPosition.Begin(go, animSpeed, target).method = UITweener.Method.EaseOut;
	}

	/// <summary>
	/// Helper function that causes the widget to smoothly grow until it reaches its original size.
	/// </summary>

	void AnimateScale(UIWidget widget, bool placeAbove, float bottom)
	{
		GameObject go = widget.gameObject;
		Transform t = widget.cachedTransform;

		float minHeight = activeFontSize * activeFontScale + mBgBorder * 2f;
		t.localScale = new Vector3(1f, minHeight / widget.height, 1f);
		TweenScale.Begin(go, animSpeed, Vector3.one).method = UITweener.Method.EaseOut;

		if (placeAbove) {
			Vector3 pos = t.localPosition;
			t.localPosition = new Vector3(pos.x, pos.y - widget.height + minHeight, pos.z);
			TweenPosition.Begin(go, animSpeed, pos).method = UITweener.Method.EaseOut;
		}
	}

	/// <summary>
	/// Helper function used to animate widgets.
	/// </summary>

	void Animate(UIWidget widget, bool placeAbove, float bottom)
	{
		AnimateColor(widget);
		AnimatePosition(widget, placeAbove, bottom);
	}

	/// <summary>
	/// Display the drop-down list when the game object gets clicked on.
	/// </summary>

	void OnClick()
	{
		if (mOpenFrame == Time.frameCount) return;

		if (mChild == null) {
			if (openOn == OpenOn.DoubleClick || openOn == OpenOn.Manual) return;
			if (openOn == OpenOn.RightClick && UICamera.currentTouchID != -2) return;
			Show();
		} else if (mHighlightedLabel != null) {
			OnItemPress(mHighlightedLabel.gameObject, true);
		}
	}

	/// <summary>
	/// Show the popup list on double-click.
	/// </summary>

	void OnDoubleClick() { if (openOn == OpenOn.DoubleClick) Show(); }

	/// <summary>
	/// Used to keep an eye on the selected object, closing the popup if it changes.
	/// </summary>

	IEnumerator CloseIfUnselected()
	{
		for (; ; ) {
			yield return null;

			if (UICamera.selectedObject != mSelection) {
				CloseSelf();
				break;
			}
		}
	}

	public GameObject source;

	/// <summary>
	/// Show the popup list dialog.
	/// </summary>

	public void Show()
	{
		if (enabled && NGUITools.GetActive(gameObject) && mChild == null && atlas != null && isValid && items.Count > 0) {
			mLabelList.Clear();
			StopCoroutine("CloseIfUnselected");

			// Ensure the popup's source has the selection
			UICamera.selectedObject = (UICamera.hoveredObject ?? gameObject);
			mSelection = UICamera.selectedObject;
			source = UICamera.selectedObject;

			if (source == null) {
				Debug.LogError("Popup list needs a source object...");
				return;
			}

			mOpenFrame = Time.frameCount;

			// Automatically locate the panel responsible for this object
			if (mPanel == null) {
				mPanel = UIPanel.Find(transform);
				if (mPanel == null) return;
			}

			// Calculate the dimensions of the object triggering the popup list so we can position it below it
			Vector3 min;
			Vector3 max;

			UIWidget thisWidget = GetComponent<UIWidget>();

			// Create the root object for the list
			mChild = new GameObject("Drop-down List");
			mChild.layer = gameObject.layer;

			if (separatePanel) {
				if (GetComponent<Collider>() != null) {
					Rigidbody rb = mChild.AddComponent<Rigidbody>();
					rb.isKinematic = true;
				} else if (GetComponent<Collider2D>() != null) {
					Rigidbody2D rb = mChild.AddComponent<Rigidbody2D>();
					rb.isKinematic = true;
				}
				var childPanel = mChild.AddComponent<UIPanel>();
				childPanel.depth = 1000000;
				childPanel.sortingOrder = 1;
			}
			current = this;

			Transform t = mChild.transform;
			t.parent = mPanel.cachedTransform;
			Vector3 pos;

			// Manually triggered popup list on some other game object
			if (openOn == OpenOn.Manual && mSelection != gameObject) {
				pos = UICamera.lastEventPosition;
				min = mPanel.cachedTransform.InverseTransformPoint(mPanel.anchorCamera.ScreenToWorldPoint(pos));
				max = min;
				t.localPosition = min;
				pos = t.position;
			} else {
				Bounds bounds = NGUIMath.CalculateRelativeWidgetBounds(mPanel.cachedTransform, transform, false, false);
				min = bounds.min;
				max = bounds.max;
				t.localPosition = min;
				pos = t.position;
			}

			StartCoroutine("CloseIfUnselected");

			t.localRotation = Quaternion.identity;
			t.localScale = Vector3.one;

			// Add a sprite for the background
			mBackground = NGUITools.AddSprite(mChild, atlas, backgroundSprite, separatePanel ? 0 : NGUITools.CalculateNextDepth(mPanel.gameObject));
			mBackground.pivot = UIWidget.Pivot.TopLeft;
			mBackground.color = backgroundColor;
			if (InheritDepth) {
				mBackground.depth = thisWidget.depth;
			} else {
				mBackground.depth = NGUITools.CalculateNextDepth(mPanel.gameObject);
			}

			// We need to know the size of the background sprite for padding purposes
			Vector4 bgPadding = mBackground.border;
			mBgBorder = bgPadding.y;
			mBackground.cachedTransform.localPosition = new Vector3(0f, bgPadding.y, 0f);

			// Add a sprite used for the selection
			mHighlight = NGUITools.AddSprite(mChild, atlas, highlightSprite, mBackground.depth + 2);
			mHighlight.pivot = UIWidget.Pivot.TopLeft;
			mHighlight.color = highlightColor;
			mHighlight.enabled = false; // MS_ADD
			if (highlightSideSprite != null) {
				mHighlightSide = NGUITools.AddSprite(mChild, atlas, highlightSideSprite, mBackground.depth + 2);
				mHighlightSide.pivot = UIWidget.Pivot.TopLeft;
				mHighlightSide.color = highlightColor;
				mHighlightSide.enabled = false;
			}
			if (highlightSideDownSprite != null) {
				mHighlightSideDown = NGUITools.AddSprite(mChild, atlas, highlightSideDownSprite, mBackground.depth + 2);
				mHighlightSideDown.pivot = UIWidget.Pivot.TopLeft;
				mHighlightSideDown.color = highlightColor;
				mHighlightSideDown.enabled = false;
			}

			UISpriteData hlsp = mHighlight.GetAtlasSprite();
			if (hlsp == null) return;

			float hlspHeight = hlsp.borderTop;
			float fontHeight = activeFontSize;
			float dynScale = activeFontScale;
			float labelHeight = fontHeight * dynScale;
			float x = 0f, y = -padding.y;
			int labelFontSize = (bitmapFont != null) ? bitmapFont.defaultSize : fontSize;
			labels = new List<UILabel>();
			List<UISprite> itemsBack  = new List<UISprite>();

			// Clear the selection if it's no longer present
			if (!items.Contains(mSelectedItem))
				mSelectedItem = null;

			// Run through all items and create labels for each one
			for (int i = 0, imax = items.Count; i < imax; ++i) {
				string s = items[i];

				UILabel lbl = NGUITools.AddWidget<UILabel>(mChild, mBackground.depth + 3);
				lbl.name = i.ToString();
				lbl.pivot = UIWidget.Pivot.TopLeft;
				lbl.bitmapFont = bitmapFont;
				lbl.trueTypeFont = trueTypeFont;
				lbl.fontSize = labelFontSize;
				lbl.fontStyle = fontStyle;
				lbl.text = isLocalized ? Localization.Get(s) : s;
				lbl.color = textColor;
				if (AlignDropdownCenter) { // MS_ADD
					lbl.cachedTransform.localPosition = new Vector3(0f, y, -1f);
				} else {
					lbl.cachedTransform.localPosition = new Vector3(bgPadding.x + padding.x, y, -1f);
				}
				lbl.cachedTransform.localPosition = new Vector3(bgPadding.x + padding.x + itemHorizontalPadding - lbl.pivotOffset.x, y - padding.y, -1f);
				lbl.overflowMethod = UILabel.Overflow.ResizeFreely;
				lbl.depth = NGUITools.CalculateNextDepth(mChild) + 10; // MS_ADD leave enough depths for dropdown button etc
				lbl.MakePixelPerfect();
				if (dynScale != 1f) lbl.cachedTransform.localScale = Vector3.one * dynScale;
				lbl.alignment = alignment;
				labels.Add(lbl);

				if (itemNormalSprite != null) {
					UISprite itemBack;
					if (i == 0) {
						itemBack = NGUITools.AddSprite(mChild, atlas, itemNormalSideSprite, mBackground.depth + 1);
					} else if (i == imax - 1) {
						itemBack = NGUITools.AddSprite(mChild, atlas, itemNormalSideDownSprite, mBackground.depth + 1);
					} else {
						itemBack = NGUITools.AddSprite(mChild, atlas, itemNormalSprite, mBackground.depth + 1);
					}
					itemBack.pivot = UIWidget.Pivot.TopLeft;
					itemBack.cachedTransform.localPosition = lbl.cachedTransform.localPosition + new Vector3(-itemHorizontalPadding , padding.y, 0f);
					itemsBack.Add(itemBack);
				}

				y -= labelHeight;
				y -= padding.y * 3;
				x = Mathf.Max(x, lbl.printedSize.x);

				// MS_ADD
				if (AlignDropdownCenter) {
					var localPos = lbl.transform.localPosition;
					localPos.x = (thisWidget.width - lbl.printedSize.x) * 0.5f;
					lbl.transform.localPosition = localPos;
				}

				// Add an event listener
				UIEventListener listener = UIEventListener.Get(lbl.gameObject);
				listener.onHover = OnItemHover;
				listener.onPress = OnItemPress;
				listener.parameter = s;

				// Move the selection here if this is the right label
				if (mSelectedItem == s || (i == 0 && string.IsNullOrEmpty(mSelectedItem)))
					Highlight(lbl, true);

				// Add this label to the list
				mLabelList.Add(lbl);
			}

			// The triggering widget's width should be the minimum allowed width
			x = Mathf.Max(x, (max.x - min.x) - (bgPadding.x + padding.x) * 2f);

			float cx = x;
			Vector3 bcCenter = new Vector3(cx * 0.5f, -labelHeight * 0.5f, 0f);
			Vector3 bcSize = new Vector3(cx, (labelHeight + padding.y), 1f);

			// Run through all labels and add colliders
			for (int i = 0, imax = labels.Count; i < imax; ++i) {
				UILabel lbl = labels[i];
				NGUITools.AddWidgetCollider(lbl.gameObject);
				lbl.autoResizeBoxCollider = false;
				BoxCollider bc = lbl.GetComponent<BoxCollider>();

				if (bc != null) {
					bcCenter.z = bc.center.z;

					// MS_ADD
					if (AlignDropdownCenter) {
						bcCenter.x = lbl.printedSize.x * 0.5f;
					}

					bc.center = bcCenter;
					bc.size = bcSize;
				} else {
					BoxCollider2D b2d = lbl.GetComponent<BoxCollider2D>();
#if UNITY_4_3 || UNITY_4_5 || UNITY_4_6
					b2d.center = bcCenter;
#else
					b2d.offset = bcCenter;
#endif
					b2d.size = bcSize;
				}
			}

			int lblWidth = Mathf.RoundToInt(x);
			x += (bgPadding.x + padding.x + itemHorizontalPadding) * 2f;
			y -= bgPadding.y;

			// Scale the background sprite to envelop the entire set of items
			mBackground.width = Mathf.RoundToInt(x);
			mBackground.height = Mathf.RoundToInt(-y + bgPadding.y);

			// Set the label width to make alignment work
			for (int i = 0, imax = labels.Count; i < imax; ++i) {
				UILabel lbl = labels[i];
				lbl.overflowMethod = UILabel.Overflow.ShrinkContent;
				lbl.width = lblWidth;
			}

			// Scale the highlight sprite to envelop a single item
			float scaleFactor = 2f * atlas.pixelSize;
			float w = x - (bgPadding.x + padding.x) * 2f + hlsp.borderLeft * scaleFactor;
			float h = labelHeight + hlspHeight * scaleFactor + padding.y * 2;
			mHighlight.width = Mathf.RoundToInt(w);
			mHighlight.height = Mathf.RoundToInt(h);
			mHighlightSide.width = Mathf.RoundToInt(w);
			mHighlightSide.height = Mathf.RoundToInt(h);
			mHighlightSideDown.width = Mathf.RoundToInt(w);
			mHighlightSideDown.height = Mathf.RoundToInt(h);

			foreach (var item in itemsBack) {
				item.width = Mathf.RoundToInt(w);
				item.height = Mathf.RoundToInt(h);
			}

			bool placeAbove = (position == Position.Above);

			if (position == Position.Auto) {
				UICamera cam = UICamera.FindCameraForLayer(mSelection.layer);

				if (cam != null) {
					Vector3 viewPos = cam.cachedCamera.WorldToViewportPoint(pos);
					placeAbove = (viewPos.y < 0.5f);
				}
			}

			// If the list should be animated, let's animate it by expanding it
			if (isAnimated) {
				AnimateColor(mBackground);

				if (Time.timeScale == 0f || Time.timeScale >= 0.1f) {
					float bottom = y + labelHeight;
					Animate(mHighlight, placeAbove, bottom);
					for (int i = 0, imax = labels.Count; i < imax; ++i)
						Animate(labels[i], placeAbove, bottom);
					AnimateScale(mBackground, placeAbove, bottom);
				}
			}

			// If we need to place the popup list above the item, we need to reposition everything by the size of the list
			if (placeAbove) {
				min.y = max.y - bgPadding.y;
				max.y = min.y + mBackground.height;
				max.x = min.x + mBackground.width;
				t.localPosition = new Vector3(min.x, max.y - bgPadding.y, min.z);
			} else {
				max.y = min.y + bgPadding.y;
				min.y = max.y - mBackground.height;
				max.x = min.x + mBackground.width;
			}

			Transform pt = mPanel.cachedTransform.parent;

			if (pt != null) {
				min = mPanel.cachedTransform.TransformPoint(min);
				max = mPanel.cachedTransform.TransformPoint(max);
				min = pt.InverseTransformPoint(min);
				max = pt.InverseTransformPoint(max);
			}

			// Ensure that everything fits into the panel's visible range
//			Vector3 offset = mPanel.hasClipping ? Vector3.zero : mPanel.CalculateConstrainOffset(min, max);
//			pos = t.localPosition + offset;
//			pos.x = Mathf.Round(pos.x);
//			pos.y = Mathf.Round(pos.y);
//			t.localPosition = pos;

			EventDelegate.Execute(onOpen);
		} else OnSelect(false);
	}
}
