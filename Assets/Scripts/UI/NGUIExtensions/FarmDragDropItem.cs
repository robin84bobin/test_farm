using Data;
using UnityEngine;

namespace UI.NGUIExtensions
{
    public class FarmDragDropItem : UIDragDropItem, IDroppableData
    {
        public bool dropClone = true;
        public bool removeClone = true;

        public DataItem Data { get; set; }

        protected override void OnClone(GameObject original)
        {
            base.OnClone(original);
            var item = original.GetComponent<FarmDragDropItem>();
            Data = item.Data;
        }

        protected override void OnDragDropRelease(GameObject surface)
        {
            if (!cloneOnDrag || (cloneOnDrag && dropClone))
            {
                // Re-enable the collider
                if (mButton != null) mButton.isEnabled = true;
                else if (mCollider != null) mCollider.enabled = true;
                else if (mCollider2D != null) mCollider2D.enabled = true;

                // Is there a droppable container?
                FarmCellDropContainer container =
                    surface ? NGUITools.FindInParents<FarmCellDropContainer>(surface) : null;

                if (container != null)
                {
                    Vector3 pos = mTrans.localPosition;
                    pos.z = 0f;
                    mTrans.localPosition = pos;
                    container.OnDroppedObject(this);
                }
                else if (cloneOnDrag && removeClone)
                {
                    NGUITools.Destroy(gameObject);
                    OnDragDropEnd();
                    return;
                }
                else
                {
                    // No valid container under the mouse -- revert the item's parent
                    mTrans.parent = mParent;
                }

                // Update the grid and table references
                mParent = mTrans.parent;
                mGrid = NGUITools.FindInParents<UIGrid>(mParent);
                mTable = NGUITools.FindInParents<UITable>(mParent);

                // Re-enable the drag scroll view script
                if (mDragScrollView != null)
                    StartCoroutine(EnableDragScrollView());

                // Notify the widgets that the parent has changed
                NGUITools.MarkParentAsChanged(gameObject);

                if (mTable != null) mTable.repositionNow = true;
                if (mGrid != null) mGrid.repositionNow = true;

            }
            
            NGUITools.Destroy(gameObject);

            // We're now done
            OnDragDropEnd();
        }

    }

    public interface IDroppableData
    {
        DataItem Data { get; }
    }
}