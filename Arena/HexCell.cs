using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexCell : MonoBehaviour
{
	/* cell label */
	public TextMeshPro label;

	//set true to disable this cell entirely
	public bool invalid;	

	/* determines whether or not this cell is a goal hole */
	public bool isHole;

	public HexCoordinates coordinates;
    
	// adjacent cells
	[SerializeField]
	[NamedArrayAttribute (new string[] {"NE", "E", "SE", "SW", "W", "NW"})]
	HexCell[] neighbors = new HexCell[6];

	/* Position of cell */
	public Vector3 Position {
		get {
			return transform.localPosition;
		}
	}


	// Start is called before the first frame update
	void Start()
	{
			
	}

	// Update is called once per frame
	void Update()
	{
			
	}

	/* sets the cell label to whatever */
	public void SetLabel (string text) {
		//Debug.Log("Setting Label " + text);
		label.text = text;
	}

	public void SetLabelActive(bool active){
		label.gameObject.SetActive(active);
	}

	public HexCell GetNeighbor (HexDirection direction) {
		if(neighbors[(int)direction] != null && neighbors[(int)direction].invalid){
			return null;
		}
		return neighbors[(int)direction];
	}

	public void SetNeighbor (HexDirection direction, HexCell cell) {
		neighbors[(int)direction] = cell;
		cell.neighbors[(int)direction.Opposite()] = this;
	}
}
