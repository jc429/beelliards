﻿using System.Collections;
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
	int radius;

	/* height of the map walls*/ 
	public static float wallHeight = 2;

	HexCell[] cells;

	public HexCell cellPrefab;

	public HexMesh cellMesh;
	public HexMesh edgeMesh;
	public HexMesh wallMesh;

	public Transform cellContainer;
	public GameObject holePrefab;

	public Transform transOffset;

	void Awake()
	{
		cellMesh.InitHexMesh();
		edgeMesh.InitHexMesh();
		wallMesh.InitHexMesh();
		CreateMap(4);
	}

	// Update is called once per frame
	void Update()
	{
			
	}


	public bool CreateMap(int mapRadius){
		radius = mapRadius;
		Vector3 offset = new Vector3(-4f * (radius - 1), -0.5f, -3f * (radius - 1));
		transOffset.localPosition	= offset;


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

		if(cell.isHole){
			GameObject hole = Instantiate(holePrefab);
			hole.transform.SetParent(cellContainer,false);
			hole.transform.localPosition = pos;
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
		wallMesh.ClearAll();

		foreach(HexCell c in cells){
			if(!c.invalid){
				TriangulateCell(c);
			}
		}

		cellMesh.Apply();
		edgeMesh.Apply();
		wallMesh.Apply();
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

			if(CheckCellValid(cell.GetNeighbor(d))){
				TriangulateEdge(d, cell);
			}

			/*** if corner cell, create the long walls of arena ***/
			// this check only passes for corner cells 
			if(!CheckCellValid(cell.GetNeighbor(d)) 
			&& !CheckCellValid(cell.GetNeighbor(d.Next())) 
			&& !CheckCellValid(cell.GetNeighbor(d.Next2()))){
				TriangulateOuterWallPanel(d, cell);
				TriangulateOuterWallPanel(d.Next(), cell);
				TriangulateOuterWallPanel(d.Next2(), cell);

				TriangulateWallStrip(d, cell);
			}

			/**** Alt Method - creates honeycomb walls - pretty but makes for bad gameplay
			HexCell n = cell.GetNeighbor(d);
			if(CheckCellValid(n)){
				TriangulateEdge(d, cell);
			}
			else{
				TriangulateOuterWallPanel(d, cell);
				HexCell n2 = cell.GetNeighbor(d.Next());
				if(CheckCellValid(n2)){
					TriangulateOuterWallPanelCorner(d, cell, n2);
				}
			}
			*/
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

	void TriangulateWallStrip(HexDirection d, HexCell cell){
		HexDirection castDir = d.Opposite();
		HexCell n = cell.GetNeighbor(castDir);
		if(CheckCellValid(n)){
			//limit to prevent infinite loops if something goes wrong
			for(int i = 0; i < 10; i++){
				HexCell n2 = n.GetNeighbor(castDir);
				if(CheckCellValid(n2)){
					n = n2;
				}
				else{
					break;
				}
			}
			//n is final cell 
			Vector3 offset = new Vector3(0, wallHeight, 0);

			EdgeVertices e1 = new EdgeVertices(
				cell.transform.localPosition + HexMetrics.GetFirstSolidCorner(castDir),
				n.transform.localPosition + HexMetrics.GetSecondSolidCorner(d)
			);

			EdgeVertices e2 = new EdgeVertices(
				e1.v1 + offset,
				e1.v2 + offset
			);
			wallMesh.AddQuad(e1, e2);
			
			EdgeVertices e3 = new EdgeVertices(
				offset + cell.transform.localPosition + HexMetrics.GetFirstCorner(castDir),
				offset + n.transform.localPosition + HexMetrics.GetSecondCorner(d)
			);
			wallMesh.AddQuad(e2, e3);

			EdgeVertices e4 = new EdgeVertices(
				offset + cell.transform.localPosition + HexMetrics.GetFirstCorner(castDir.Previous()),
				offset + n.transform.localPosition + HexMetrics.GetSecondCorner(d.Next())
			);
			wallMesh.AddQuad(e3,e4);	


		}
	}



	/*** Unused? ***/ 

	/* creates the stage boundaries */ 
	void TriangulateOuterWallPanel(HexDirection direction, HexCell cell){
		Vector3 center = cell.transform.localPosition;
		Vector3 offset = new Vector3(0, wallHeight, 0);
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

		wallMesh.AddQuad(e1, e2);
		wallMesh.AddQuad(e2, e3);
	}

	/* creates the stage boundaries */ 
	void TriangulateOuterWallPanelCorner(HexDirection direction, HexCell cell, HexCell neighbor){
		Vector3 offset = new Vector3(0, wallHeight, 0);
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

	bool CheckCellValid(HexCell cell){
		return (cell != null && !cell.invalid);
	}

	bool CheckIfHole(HexCoordinates c){
		int r1 = radius - 1;
		if(c.X == -1){
			if(c.Z == r1 || c.Z == 2*r1){
				return true;
			}
		}
		if(c.X == r1 - 1){
			if(c.Z == 0 || c.Z == 2*r1){
				return true;
			}
		}
		if(c.X == (2 * r1) - 1){
			if(c.Z == 0 || c.Z == r1){
				return true;
			}
		}
		return false;
	}
}
