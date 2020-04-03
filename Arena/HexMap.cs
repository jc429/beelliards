using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HexMap : MonoBehaviour
{
	public static bool _ShowLabels;
	
	//where the center of the map is located
	HexCoordinates centerCoords;	//x = half of radius, z = radius - 1 

	/* size of map in cells */
	public int cellCountX = 6, cellCountZ = 6;
	public int cellCountTotal = 0;
	
	HexCell[] cells;

	public HexCell cellPrefab;

	public HexMesh cellMesh;
	public HexMesh edgeMesh;

	public Transform cellContainer;

	void Awake()
	{
		cellMesh.InitHexMesh();
		edgeMesh.InitHexMesh();
		CreateMap(4);
	}

	// Update is called once per frame
	void Update()
	{
			
	}


	public bool CreateMap(int radius){
		centerCoords = new HexCoordinates(radius / 2, radius - 1);
		
		//just make a larger map and then disable certain cells
		cellCountX = (radius * 2) + 1;
		cellCountZ = (radius * 2) + 1;
		cellCountTotal = cellCountX * cellCountZ;

		CreateCells(radius);

		//carve out the excess tiles to make this a hexagon
		foreach(HexCell cell in cells){
			int x = cell.coordinates.X;
			int z = cell.coordinates.Z;
			if((z >= (2 * radius) - 1) 											// top
			|| (x >= Mathf.Floor(1.5f * (float)radius))			//lower right
			|| ((x + z) >= (2 * radius) + (radius - 2)/2)		//upper right
			|| ((x + z) < (radius / 2))											//lower left
			|| (x <= -((float)radius * 0.5f))){							//upper left 

				cell.invalid = true;
				cell.SetLabel("");
			}
		}

		TriangulateAll();
		
		return true;
	}

	void CreateCells(int radius){
		cells = new HexCell[cellCountX * cellCountZ];
		for(int z = 0, i = 0; z < cellCountZ; z++){
			for(int x = 0; x < cellCountX; x++){
				CreateCell(x,z,i++);
			}
		}
	}

	/* Creates and labels a Hex Cell */
	void CreateCell(int x, int z, int i){
		Vector3 pos;
		pos.x = (x + (z * 0.5f - (z / 2))) * (HexMetrics.innerRadius * 2f);
		pos.y = 0;
		pos.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(cellContainer,false);
		cell.transform.localPosition = pos;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x,z);
		//cell.coordinates = new HexCoordinates(x,z);	
		cell.SetLabel(cell.coordinates.ToString());
		cell.SetLabelActive(_ShowLabels);

		cell.isHole = CheckIfHole(cell.coordinates);

		//connect to neighbors
		if(x > 0){
			cell.SetNeighbor(HexDirection.W, cells[i - 1]);
		}
		if(z > 0){
			if((z & 1) == 0){
				cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
				if(x > 0){
					cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
				}
			}
			else {
				cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
				if (x < cellCountX - 1) {
					cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
				}
			}
		}
	

	//label cell
	//TextMeshPro label = Instantiate<TextMeshPro>(cellLabelPrefab);
	//label.rectTransform.anchoredPosition = new Vector2(pos.x, pos.z);
	//label.text = cell.coordinates.ToString();

	//cell.uiRect = label.rectTransform;


	//AddCellToChunk(x, z, cell);
	}

	public void TriangulateAll(){
		cellMesh.ClearAll();
		edgeMesh.ClearAll();

		foreach(HexCell c in cells){
			if(!c.invalid){
				TriangulateCell(c);
			}
		}

		cellMesh.Apply();
		edgeMesh.Apply();
	}


	/* Creates the 6 triangles that comprise a hex cell */
	void TriangulateCell(HexCell cell){
		for (HexDirection d = HexDirection.NE; d <= HexDirection.NW; d++) {
			if(cell.isHole){
				TriangulateHoleWall(d, cell);
			}
			else{
				TriangulateWedge(d, cell);
			}

			HexCell n = cell.GetNeighbor(d);
			if(n != null && !n.invalid){
				TriangulateEdge(d, cell);
			}
			else{
				TriangulateOuterWallPanel(d, cell);
				HexCell n2 = cell.GetNeighbor(d.Next());
				if(n2 != null && !n2.invalid){
					TriangulateOuterWallPanelCorner(d, cell, n2);
				}
			}
		}
	}

	/* Creates a single triangle from cell center to cell edge */
	void TriangulateWedge(HexDirection direction, HexCell cell){
		Vector3 center = cell.transform.localPosition;
		EdgeVertices e = new EdgeVertices(
			center + HexMetrics.GetFirstSolidCorner(direction),
			center + HexMetrics.GetSecondSolidCorner(direction)
		);

		cellMesh.AddTriangle(center, e.v1, e.v2);
	}
	
	/* Creates a quad stretching from a cell's solid boundary to its true edge*/
	void TriangulateEdge(HexDirection direction, HexCell cell){
		Vector3 center = cell.transform.localPosition;
		EdgeVertices e1 = new EdgeVertices(
			center + HexMetrics.GetFirstSolidCorner(direction),
			center + HexMetrics.GetSecondSolidCorner(direction)
		);
		EdgeVertices e2 = new EdgeVertices(
			center + HexMetrics.GetFirstCorner(direction),
			center + HexMetrics.GetSecondCorner(direction)
		);

		edgeMesh.AddQuad(e1, e2);
	}

	/* Creates a quad wall for hole cells */
	void TriangulateHoleWall(HexDirection direction, HexCell cell){
		Vector3 center = cell.transform.localPosition;
		Vector3 offset = new Vector3(0, -2, 0);
		EdgeVertices e2 = new EdgeVertices(
			center + HexMetrics.GetFirstSolidCorner(direction),
			center + HexMetrics.GetSecondSolidCorner(direction)
		);
		EdgeVertices e1 = new EdgeVertices(
			e2.v1 + offset,
			e2.v2 + offset
		);

		edgeMesh.AddQuad(e1, e2);
	}





	/*** Unused? ***/ 

	/* creates the stage boundaries */ 
	void TriangulateOuterWallPanel(HexDirection direction, HexCell cell){
		Vector3 center = cell.transform.localPosition;
		Vector3 offset = new Vector3(0, 4, 0);
		EdgeVertices e1 = new EdgeVertices(
			center + HexMetrics.GetFirstSolidCorner(direction),
			center + HexMetrics.GetSecondSolidCorner(direction)
		);
		EdgeVertices e2 = new EdgeVertices(
			e1.v1 + offset,
			e1.v2 + offset
		);
		EdgeVertices e3 = new EdgeVertices(
			center + offset + HexMetrics.GetFirstCorner(direction),
			center + offset + HexMetrics.GetSecondCorner(direction)
		);

		edgeMesh.AddQuad(e1, e2);
		edgeMesh.AddQuad(e2, e3);
	}

	/* creates the stage boundaries */ 
	void TriangulateOuterWallPanelCorner(HexDirection direction, HexCell cell, HexCell neighbor){
		Vector3 offset = new Vector3(0, 4, 0);
		Vector3 endpoint = offset + cell.transform.localPosition + HexMetrics.GetSecondCorner(direction);

		EdgeVertices e1 = new EdgeVertices(
			cell.transform.localPosition + HexMetrics.GetSecondSolidCorner(direction),
			neighbor.transform.localPosition + HexMetrics.GetFirstSolidCorner(direction.Previous())
		);
		EdgeVertices e2 = new EdgeVertices(
			e1.v1 + offset,
			e1.v2 + offset
		);
		
		edgeMesh.AddQuad(e1, e2);
		edgeMesh.AddTriangle(e2.v1, endpoint, e2.v2);
	}



	bool CheckIfHole(HexCoordinates c){
		if(c.X == -1){
			if(c.Z == 3 || c.Z == 6){
				return true;
			}
		}
		if(c.X == 2){
			if(c.Z == 0 || c.Z == 6){
				return true;
			}
		}
		if(c.X == 5){
			if(c.Z == 0 || c.Z == 3){
				return true;
			}
		}
		return false;
	}
}
