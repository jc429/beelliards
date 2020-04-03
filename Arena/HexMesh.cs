using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour
{
	Mesh hexMesh;
	[System.NonSerialized] List<Vector3> vertices = new List<Vector3>();
	[System.NonSerialized] List<int> triangles = new List<int>();
	[System.NonSerialized] List<Color> colors = new List<Color>();

	MeshCollider meshCollider;
	public bool useCollider;

	
	// Start is called before the first frame update
	void Awake()
	{
	}

	// Update is called once per frame
	void Update()
	{
			
	}

	public void InitHexMesh(){
		hexMesh = GetComponent<MeshFilter>().mesh = new Mesh();
		if (useCollider) {
			meshCollider = gameObject.AddComponent<MeshCollider>();
		}
		hexMesh.name = "HexMesh";
	}

	public void ClearAll() {
		if(hexMesh == null){
			InitHexMesh();
		}
		hexMesh.Clear();
		vertices.Clear();
		triangles.Clear();
		colors.Clear();	
	}

	public void Apply () {
		//Debug.Log(vertices.Count + ", " + triangles.Count + ", " + colors.Count);
		hexMesh.SetVertices(vertices);
		hexMesh.SetColors(colors);
		hexMesh.SetTriangles(triangles, 0);
		hexMesh.RecalculateNormals();
		if (useCollider) {
			meshCollider.sharedMesh = hexMesh;
		}
	}

	/* Creates a triangle from three points and adds it to the lists */
	public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3){
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
	}

	/* Creates a quad from four points and adds it to the lists */
	public void AddQuad (Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
		int vertexIndex = vertices.Count;
		vertices.Add(v1);
		vertices.Add(v2);
		vertices.Add(v3);
		vertices.Add(v4);
		triangles.Add(vertexIndex);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 1);
		triangles.Add(vertexIndex + 2);
		triangles.Add(vertexIndex + 3);
	}

	public void AddQuad(EdgeVertices e1, EdgeVertices e2){
		AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
	}

}
