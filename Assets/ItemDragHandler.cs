using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler {
	private void OnMouseDrag()
	{
		throw new System.NotImplementedException();
	}

	public void OnDrag(Vector2 pos)
	{
	}

	public void OnDrag(PointerEventData eventData)
	{
		transform.position = Input.mousePosition;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		transform.localPosition = Vector3.zero;
	}
}
