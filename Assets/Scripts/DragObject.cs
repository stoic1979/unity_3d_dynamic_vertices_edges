using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragObject : MonoBehaviour {

	public Graph map;
	private Vector3 screenPoint;
	private Vector3 offset;
	private Vector3 cursorPosition;
	private Vector3 cursorPoint;
	private float scale = 0.3f;
	private int counter = 0;
	private int vertexIndex = 0;

	private List<int> edgesStart = new List<int> ();
	private List<int> edgesEnd = new List<int> ();

	private List<int> endVertex = new List<int> ();
	private List<int> startVertex = new List<int> ();

	private GameObject[] edges;

	public Vector3[] verticesInitialPositions;
	// Use this for initialization
	void Start () {
		map = GameObject.Find ("Controller").GetComponent<GraphController> ().map;

		vertexIndex = GetVertexIndex (gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseDown(){

		// save records of edges starts/ends from this vertex
	//	index = getIndex (gameObject);
		if (vertexIndex != -1) {
			int id = map.vertices [vertexIndex].id;
			edges = GameObject.FindGameObjectsWithTag ("Edge");
			for (int j = map.edges.Count - 1; j >= 0 ; j--) {
				if (map.edges [j].start == id) {
					Debug.Log ("index for edges starts from this vertex in edges list = " + j);
					edgesStart.Add (j);
					endVertex.Add(map.edges [j].end);
					counter++;
				}
				if (map.edges [j].end == id) {
					Debug.Log ("index for edges ends to this vertex in edges list = " + j);
					edgesEnd.Add (j);
					startVertex.Add (map.edges [j].start);
					counter++;
				}
			}
			Debug.Log ("Edges with  this vertex = " + counter);
			counter = 0;
		}

		// save the initial positions of all the vertices to restore on if we drop on another vertex
		verticesInitialPositions = new Vector3[map.vertices.Count];
		GameObject[] vertices = GameObject.FindGameObjectsWithTag ("Vertex");
		for (int i = 0; i < map.vertices.Count; i++) {
			verticesInitialPositions[i] = vertices [i].transform.position;
		}

		//maintain the screenPoints to update the values of vertex on dragging
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}

	void OnMouseDrag(){
		float dragFactor = 0.5f;
		cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		transform.position = cursorPosition;
	}

	void OnMouseUp(){

		int x = (int) (Camera.main.ScreenToWorldPoint (cursorPoint).x + offset.x);
		int y = (int) (Camera.main.ScreenToWorldPoint (cursorPoint).y + offset.y);
		int z = (int) (Camera.main.ScreenToWorldPoint (cursorPoint).z + offset.z);

		cursorPosition = new Vector3(x, y, z);
		transform.position = cursorPosition;

		for(int i = 0; i<map.vertices.Count; i++){
			// restrict to match with it self value
			if (i == vertexIndex) {
				continue;
			}
			if (transform.position == verticesInitialPositions [i]) {
				GameObject[] vertices = GameObject.FindGameObjectsWithTag ("Vertex");
				for(int j = 0 ; j < vertices.Length; j++){
					vertices [j].transform.position = verticesInitialPositions [j];

					// set new value to vertices list
					map.vertices [j].x = (int) verticesInitialPositions [j].x;
					map.vertices [j].y = (int) verticesInitialPositions [j].y;
					map.vertices [j].z = (int) verticesInitialPositions [j].z;
				}

				map.AddEdge (map.vertices [vertexIndex].id, map.vertices [i].id, "New Direction");

				// draw the new edge graphically
				GameObject parent = GameObject.Find("Graph");
				Edge edge = map.edges [map.edges.Count - 1];

				Vertex start = map.GetVertex (edge.start);
				Vertex end = map.GetVertex (edge.end);

				Vector3 V1 = new Vector3 (start.x, start.y, start.z);
				Vector3 V2 = new Vector3 (end.x, end.y, end.z);
				Vector3 edgeOffset = V2 - V1;
				Vector3 position = V1 + (edgeOffset / 2.0f);

				// instatiate
				GameObject obj = (GameObject) GameObject.Instantiate (map.cylinder, position, Quaternion.identity);

				// transform
				obj.transform.localScale = new Vector3(scale * 0.5f, edgeOffset.magnitude * .5f, scale * 0.5f);

				// rotate
				obj.transform.rotation = Quaternion.FromToRotation(Vector3.up, V2 - V1);

				obj.name = "" + edge;
				obj.tag = "Edge";
				obj.transform.parent = parent.transform;
			}
		}


//		int id = map.vertices [vertexIndex].id;
//		GameObject[] edges = GameObject.FindGameObjectsWithTag ("Edge");
//		for (int j = map.edges.Count - 1; j >= 0 ; j--) {
//			if (map.edges [j].start == id || map.edges [j].end == id) {
//				Destroy (edges [j]);
//				Debug.Log ("No of edges remaining = " + map.edges.Count);
//				//map.DrawEdge (map.edges [j]);
//
//			}
//		}
//
//		Debug.Log ("Map: " + map);
			
	}



	public int GetVertexIndex(GameObject obj) {

		if (obj.tag == "Vertex") {
			GameObject[] vertices = GameObject.FindGameObjectsWithTag ("Vertex");
			for (int i = 0; i < vertices.Length; i++) {
				GameObject vertex = vertices [i];
				if (vertex == obj) {
					return i;
				}
			}
		}
		return -1;
	}

}// DragObject