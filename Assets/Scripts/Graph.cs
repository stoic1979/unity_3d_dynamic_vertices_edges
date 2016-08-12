using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Graph {

	public GameObject sphere;
	public GameObject cylinder;

	GameObject parent;
	float scale;


    public List<Vertex> vertices = new List<Vertex> ();
    public List<Edge> edges = new List<Edge> ();

    public void AddVertex(int x, int y, int z, string name, string description){

		// id is 0 for first vertex, 
		// then 1+ last vertex in the list
		int id = 0;
		if (vertices.Count > 0) {
			id = vertices [vertices.Count - 1].id + 1;
		}

        // add vertex
		Vertex vertex = new Vertex(id, x, y, z, name, description); 

		// dont add same vertex again in the list
		for (int i = 0; i < vertices.Count; i++) {
			if (vertices [i].Equals (vertex)) {
				return;
			}
		}

		// adding vertex in list
		vertices.Add(vertex);
    }

	private int GetVertexIndex(int id) {
		// vertex id and list index cant be same
		// after removing an elemnt inside list
		for (int i = 0; i < vertices.Count; i++) {
			if (vertices [i].id == id) {
				return i;
			}
		}

		// invalid id
		return -1;
	}

    public void RemoveVertex(int id){
		int index = GetVertexIndex (id);

		// ensure that id is valid
		if (index == - 1) {
			return;
		}

		vertices.RemoveAt (index);

		// removing edges incident on this vertex
		// (in descendeing order of index to avoid array out of exception)
		for (int i = edges.Count - 1; i >= 0; i--) {
			if (edges [i].start == id || edges [i].end == id) {
				RemoveEdge (edges [i].id);
			}
		}

    }

	private int GetEdgeIndex(int id) {
		// edge id and list index cant be same
		// after removing an elemnt inside list
		for (int i = 0; i < edges.Count; i++) {
			if (edges [i].id == id) {
				return i;
			}
		}

		// invalid id
		return -1;
	}

    public void AddEdge(int start, int end, string direction){

		// ensure that edge start vertex is valid
		if (start > vertices.Count - 1 || start < 0) {
			return;
		}

		// ensure that edge end vertex is valid
		if (end > vertices.Count - 1 || end < 0) {
			return;
		}

		// ensure that start != end
		if (start == end) {
			return;
		}


		// id is 0 for first edge, 
		// then 1+ last edge in the list
		int id = 0;
		if (edges.Count > 0) {
			id = edges [edges.Count - 1].id + 1;
		}
			
        // add edge starting at start, ending at end and pointing in direction
		Edge edge = new Edge(id, start, end, direction);

		// don't add same edge again in the list
		for(int i = 0; i < edges.Count; i++){
			if (edges [i].Equals (edge)) {
				return;
			}
		}

		// adding edge in the list
		edges.Add(edge);
    }

    public void RemoveEdge(int id){
		int index = GetEdgeIndex (id);

		// ensure that id is valid
		if (index == - 1) {
			return;
		}

		edges.RemoveAt (index);

    }

	public Vertex GetVertex(int id) {

		for (int i = 0; i < vertices.Count; i++) {
			Vertex vertex = vertices [i];
			if (vertex.id == id)
				return vertex;
		}
		return null;
	}

    public GameObject Instantiate(float scale) {
		parent = new GameObject ();
		this.scale = scale;

		// will be fetching this parent globally with name,
		// so assigning this a name
		parent.name = "Graph";

		// creating vertices using the sphere primitives scaled by scale factor
		for (int i = 0; i < vertices.Count; i++) {
			Vertex vertex = vertices [i];

			Vector3 position = new Vector3 (vertex.x, vertex.y, vertex.z);

			GameObject obj = (GameObject) GameObject.Instantiate (sphere, position, Quaternion.identity);
			obj.transform.localScale = new Vector3 (scale, scale, scale);
			obj.transform.parent = parent.transform;
			obj.name = vertex.description;
			obj.tag = "Vertex";
		}

		// creating edges using cylinder primitives
		for (int i = 0; i < edges.Count; i++) {
			DrawEdge (edges [i]);
		}

        return parent;
    }


	public void DrawEdge(Edge edge) {
		
		Vertex start = GetVertex (edge.start);
		Vertex end = GetVertex (edge.end);

		Vector3 V1 = new Vector3 (start.x, start.y, start.z);
		Vector3 V2 = new Vector3 (end.x, end.y, end.z);
		Vector3 offset = V2 - V1;
		Vector3 position = V1 + (offset / 2.0f);

		// instatiate
		GameObject obj = (GameObject) GameObject.Instantiate (cylinder, position, Quaternion.identity);

		// transform
		obj.transform.localScale = new Vector3(scale * 0.5f, offset.magnitude * .5f, scale * 0.5f);

		// rotate
		obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, V2 - V1);

		obj.name = "" + edge;
		obj.tag = "Edge";
		obj.transform.parent = parent.transform;
	}

	public override string ToString()
	{
		string ret = "------ Vertices -------";
		for (int i = 0; i < vertices.Count; i++) {
			ret += "\n" + vertices[i];
		}

		ret += "\n\n" + "------ Edges -------";
		for (int i = 0; i < edges.Count; i++) {
			ret += "\n" + edges[i];
		}

		return ret;
	}

}// Graph