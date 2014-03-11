/*****************************************************
 * Program: Reignite
 * Script: ResizeGrid.cs
 * Author: Jonathan Cabe
 * Description: This script adjusts NGUI grids' scaling
 * spacing, and orientation with respect to either a
 * specified widget container or screen dimentions.
 * ***************************************************/

using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
[RequireComponent(typeof(UIGrid))]
public class ResizeGrid : MonoBehaviour 
{
	public enum Orientation {Horizontal, Vertical, Both, None}
	public enum Boundaries {World, Widget}
	
	public Orientation type;
	public Boundaries target;
	public UIWidget container;
	
	public bool additiveSpacing = false;
	
	public Vector2 spacing;
	public Vector2 relativeSize;
	public Vector2 relativePosition;
	
	private UIGrid grid;
	private Vector2 cellSize;
	
	private float targetWidth = 0f;
	private float targetHeight = 0f;
	
	
	void Start()
	{
		grid = gameObject.GetComponent<UIGrid>();
		
		Vector3 newPos = new Vector3(Screen.width * relativePosition.x, Screen.height * relativePosition.y, 0f);
		
		transform.localPosition = newPos;
	}
	
	void Update()
	{
		if (additiveSpacing)
		{
			if (target == Boundaries.World || !container)
			{
				targetWidth = Screen.width;
				targetHeight = Screen.height;
			}
			else if (target == Boundaries.Widget && container)
			{
				targetWidth = container.localSize.x;
				targetHeight = container.localSize.y;
			}
			else
			{
				targetWidth = 1f;
				targetHeight = 1f;
			}
			
			cellSize = new Vector2(relativeSize.x * targetWidth, relativeSize.y * targetHeight);
			
			cellSize += spacing;
		}
		else
		{
			if (target == Boundaries.World || !container)
			{
				targetWidth = Screen.width;
				targetHeight = Screen.height;
			}
			else if (target == Boundaries.Widget && container)
			{
				targetWidth = container.localSize.x;
				targetHeight = container.localSize.y;
			}
			else
			{
				targetWidth = 1f;
				targetHeight = 1f;
			}
			
			cellSize = new Vector2(relativeSize.x * targetWidth, relativeSize.y * targetHeight);
			
			cellSize -= spacing;
		}
		
		if (grid)
		{
			if (type == Orientation.Both)
			{
				grid.cellWidth = cellSize.x;
				grid.cellHeight = cellSize.y;
			}
			else if (type == Orientation.Horizontal)
			{
				grid.cellWidth = cellSize.x;
			}
			else if (type == Orientation.Vertical)
			{
				grid.cellHeight = cellSize.y;
			}
		}
		
		grid.Reposition();
	}
}
