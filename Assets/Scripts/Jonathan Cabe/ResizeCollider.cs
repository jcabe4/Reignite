using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class ResizeCollider : MonoBehaviour 
{
	public enum resizeType {Horizontal, Vertical, Both};
	public resizeType type;

	public bool doubleBar = false;
	public UIWidget target;
	public Vector2 relativeSize = Vector2.one;
	private Vector2 size;
	private Vector2 center;
	private UIWidget widget;
	private BoxCollider bCollider;

	void Start()
	{
		widget = gameObject.GetComponent<UIWidget>();
		bCollider = gameObject.GetComponent<BoxCollider>();
	
		if (widget)
		{
			if (target && (type == resizeType.Both || type == resizeType.Horizontal))
			{
				widget.width = target.width;

				if (doubleBar)
				{
					widget.height = target.height * 2;
				}
				else
				{
					widget.height = target.height;
				}
			}
			if (target && (type == resizeType.Both || type == resizeType.Vertical))
			{
				widget.height = target.height;
			}
		}

		Align();
	}

	void Update()
	{
		Align();
	}

	void Align()
	{
		if (widget)
		{
			size.x = widget.width * relativeSize.x;

			size.y = widget.height * relativeSize.y;
			
			center.x = -widget.width / 2;
			center.y = -widget.height / 2;
			
			if (widget.pivot == UIWidget.Pivot.BottomLeft ||
			    widget.pivot == UIWidget.Pivot.Left ||
			    widget.pivot == UIWidget.Pivot.TopLeft)
			{
				center.x = -center.x;
			}
			else if (widget.pivot == UIWidget.Pivot.Bottom ||
			         widget.pivot == UIWidget.Pivot.Center ||
			         widget.pivot == UIWidget.Pivot.Top)
			{
				center.x = 0;
			}
			
			if (widget.pivot == UIWidget.Pivot.Bottom ||
			    widget.pivot == UIWidget.Pivot.BottomLeft ||
			    widget.pivot == UIWidget.Pivot.BottomRight)
			{
				center.y = -center.y;
			}
			else if (widget.pivot == UIWidget.Pivot.Center ||
			         widget.pivot == UIWidget.Pivot.Left ||
			         widget.pivot == UIWidget.Pivot.Right)
			{
				center.y = 0;
			}

			bCollider.size = size;
			bCollider.center = center;
		}
		else
		{
			size.x = Screen.width * relativeSize.x;
			size.y = Screen.height * relativeSize.y;
			
			center = Vector2.zero;
			
			bCollider.size = size;
			bCollider.center = center;
		}
	}
}
