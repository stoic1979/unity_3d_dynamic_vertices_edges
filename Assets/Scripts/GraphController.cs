using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GraphController : MonoBehaviour {
    public Graph map;
    public GameObject MapInstance;
	public GameObject sphere;
	public GameObject cylinder;


	// variables to maintain the fieldOfView of camera
	private float minFov = 20.0f;
	private float maxFov = 80.0f;
	private float sensitivity = 10.0f;


	//------------------------------------------
	// materials for vertex, edge and selection
	//------------------------------------------
	public Material vertexMaterial;
	public Material edgeMaterial;
	public Material selectionMaterial;

	private float scale = 0.3f;

	//------------------------------------------
	// panel to show selected vertex details
	//------------------------------------------
	public GameObject vertexPanel;

	//------------------------------------------
	// panel to show selected edge details
	//------------------------------------------
	public GameObject edgePanel;

	//------------------------------------------
	// text fields for vertex/edge details
	//------------------------------------------
	public InputField vertexName;
	public InputField vertexDescription;
	public InputField edgeDirection;

	//------------------------------------------
	// GameObject to refer to selected
	// vertex or edge
	//------------------------------------------
	private GameObject selectedGameObject = null;

	//------------------------------------------
	// Camera object to handle the changes
	// related to maincamera
	//------------------------------------------
	public Camera mainCamera;
	private Vector3 camRot;

	//------------------------------------------
	// indices for selected vertex/edge
	//------------------------------------------
	private int selectedVertexIndex = 0;
	private int selectedEdgeIndex = 0;

	public float distance = 4.5f;


	// Use this for initialization
	void Start () {
        map = new Graph ();
		map.sphere = sphere;
		map.cylinder = cylinder;

        map.AddVertex (0, 0, 0, "center", "Center of the world");
        map.AddVertex (1, 0, 0, "left", "Left of center");
        map.AddVertex (-1, 0, 0, "right", "Right of center");
        map.AddVertex (0, 1, 0, "top", "Above center");
        map.AddVertex (0, -1, 0, "bottom", "Below center");
        map.AddVertex (0, 0, 1, "elsewhere", "Somewhere else");

        map.AddEdge (0, 1, "left");
        map.AddEdge (0, 3, "up");
		map.AddEdge (0, 2, "right");
        map.AddEdge (0, 4, "down");

		InstantiateMap ();

		ResetMaterials ();

		// selecting the first vertex by default
		GameObject.FindGameObjectsWithTag ("Vertex")[0].GetComponent<Renderer> ().material = selectionMaterial;
		selectedGameObject = GameObject.FindGameObjectsWithTag ("Vertex") [0];

		if (selectedGameObject.tag == "Vertex") {
			vertexPanel.SetActive (true);
			vertexName.text = map.vertices [0].name;
			vertexDescription.text = map.vertices [0].description;

			// set the camera position so that seleted object will
			// set into the center of the view
			MoveCameraToSelectedObjectPosition(selectedGameObject);
		} else {
			edgePanel.SetActive (true);
		}
	}

	public void InstantiateMap() {
		MapInstance = map.Instantiate (scale);
	}
		
	//--------------------------------------------------------
	// function resets the material of all vertices and edges
	// to default material
	//--------------------------------------------------------
	public void ResetMaterials(){
		
		GameObject[] vertices, edges;

		vertices = GameObject.FindGameObjectsWithTag ("Vertex");
		edges = GameObject.FindGameObjectsWithTag ("Edge");

		foreach (GameObject vertex in vertices) {
			vertex.gameObject.GetComponent<Renderer> ().material = vertexMaterial;
		}

		foreach ( GameObject edge in edges) {
			edge.gameObject.GetComponent<Renderer> ().material = edgeMaterial;
		}
	}

	//--------------------------------------------
	// function add new vertex near to the last
	// added vertex
	//-------------------------------------------- 
	public void AddVertex(){

		int x, y, z;

		if (map.vertices.Count > 0) {
			x = map.vertices [map.vertices.Count - 1].x + Random.Range(0, 2);
			y = map.vertices [map.vertices.Count - 1].y + Random.Range(0, 2);
			z = map.vertices [map.vertices.Count - 1].z + Random.Range(0, 2);
		} else {
			x = 0;
			y = 0;
			z = 0;
		}
		map.AddVertex (x, y, z, "New Vertex", "A new vertex");

		GameObject parent = GameObject.Find("Graph");
		Vertex vertex = map.vertices [map.vertices.Count - 1];

		Vector3 position = new Vector3 (vertex.x, vertex.y, vertex.z);

		GameObject obj = (GameObject) GameObject.Instantiate (sphere, position, Quaternion.identity);
		obj.transform.localScale = new Vector3 (scale, scale, scale);
		obj.transform.parent = parent.transform;
		obj.name = vertex.description;
		obj.tag = "Vertex";
	}

	//--------------------------------------------
	// function removes a selected edge or vertex
	//--------------------------------------------
	public void RemoveEdgeVertex() {

		if (!selectedGameObject) {
			return;
		}

		if(selectedGameObject.tag == "Vertex") {
			Destroy(selectedGameObject);

			int i = getSelectedGameObjectIndex (selectedGameObject);
			if(i != -1){
				int id = map.vertices [i].id;
				GameObject[] edges = GameObject.FindGameObjectsWithTag ("Edge");
				for (int j = map.edges.Count - 1; j >= 0 ; j--) {
					if (map.edges [j].start == id || map.edges [j].end == id) {
						map.RemoveEdge (map.edges [j].id);
						Destroy (edges [j]);
					}
				}
				map.vertices.RemoveAt (i);
			}
		}
		if (selectedGameObject.tag == "Edge") {
			Destroy (selectedGameObject);
			int i = getSelectedGameObjectIndex (selectedGameObject);
			if (i != -1) {
				map.edges.RemoveAt (i);
			}
		}
	}

	//--------------------------------------------
	// function uses vertex name and description
	// from UI/Canvas and saves to selected 
	// vertex in the graph
	//--------------------------------------------
	public void SaveVertexDetail(){
		map.vertices [selectedVertexIndex].name = vertexName.text;
		map.vertices [selectedVertexIndex].description = vertexDescription.text;
	}

	//--------------------------------------------
	// function uses edge description
	// from UI/Canvas and saves to selected 
	// edge in the graph
	//--------------------------------------------
	public void SaveEdgeDetail(){
		map.edges [selectedEdgeIndex].direction = edgeDirection.text;
	}

	//--------------------------------------------
	// function move the main camera to the
	// selected object position so that object
	// should look in the center on the view
	//--------------------------------------------
	public void MoveCameraToSelectedObjectPosition(GameObject obj){
	//	mainCamera.transform.position = new Vector3 (obj.transform.position.x,
	//												obj.transform.position.y,
	//												obj.transform.position.z - 5);
		//mainCamera.GetComponent<Camera> ().transform.rotation = Quaternion.Euler (10,10,10);

//		Debug.Log ("initial position of camera = " + mainCamera.transform.position);
//		mainCamera.transform.position = new Vector3 (( mainCamera.transform.position.x - obj.transform.position.x),
//			( mainCamera.transform.position.y - obj.transform.position.y),
//			( mainCamera.transform.position.z - obj.transform.position.z));

//		mainCamera.transform.position = new Vector3 (obj.transform.position.x - 2,
//			obj.transform.position.y,
//			obj.transform.position.z - 2);
	}
		
	//--------------------------------------------------------
	// function to rotate the camera around the selected 
	// vertex/edge on specific axis
	//--------------------------------------------------------
	public void RotateAroundAxis(string axis){
		if (selectedGameObject == null) {
			return;
		}

		mainCamera.transform.LookAt (selectedGameObject.transform.position);

		if (axis.Equals ("x")) {
			
			mainCamera.transform.RotateAround (
				selectedGameObject.transform.position, // target
				new Vector3 (1.0f, 0.0f, 0.0f),        // transform
				45);                                   // angle
		}

		if (axis.Equals ("y")) {
			mainCamera.transform.RotateAround (
				selectedGameObject.transform.position, // target
				new Vector3 (0.0f, 1.0f, 0.0f),        // transform
				45);                                   // angle
		}

		if (axis.Equals ("z")) {
			mainCamera.transform.RotateAround (          // target
				selectedGameObject.transform.position,   // transform
				new Vector3 (0.0f, 0.0f, 1.0f),          // angle
				45);
		}
	}

	void Update()
	{
		CastRayToWorld ();
		// set the object for camera to rotate around the specific one

			//mainCamera.transform.LookAt (selectedGameObject.transform.position);


		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = new RaycastHit();
			bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);

			// selecting edge/vertex
			if (hit) {
				ResetMaterials ();
				hitInfo.transform.gameObject.GetComponent<Renderer> ().material = selectionMaterial;
				selectedGameObject = hitInfo.transform.gameObject;
				getSelectedGameObjectIndex (selectedGameObject);

				// reset the camera position on changing the selected object
				// so that new selected object will set into the center of the view
				MoveCameraToSelectedObjectPosition(selectedGameObject);
			}
		}

		// handle camera fieldOfView with mouse scroll wheel
		HandleCameraWithMouseScroll ();

		// handle camera position with keyboard keys
		HandleCameraWithKeyboardKeys();
	}

	//--------------------------------------------------------
	// function to handle the camera field of view with
	// mouse wheel scrolling
	//--------------------------------------------------------
	private void HandleCameraWithMouseScroll(){

		float fov = Camera.main.fieldOfView;
		fov -= Input.GetAxis ("Mouse ScrollWheel") * sensitivity;
		fov = Mathf.Clamp (fov, minFov, maxFov);
		Camera.main.fieldOfView = fov;
	}

	//--------------------------------------------------------
	// function to handle the camera position
	// with keyboard keys
	//--------------------------------------------------------
	private void HandleCameraWithKeyboardKeys(){

		mainCamera.transform.LookAt (null);

		if(Input.GetKey(KeyCode.RightArrow)){
			Camera.main.transform.Translate (new Vector3 (5.0f * Time.deltaTime, 0, 0));
		}
		if (Input.GetKey (KeyCode.LeftArrow)) {
			Camera.main.transform.Translate (new Vector3 (-5.0f * Time.deltaTime, 0, 0));
		}
		if (Input.GetKey (KeyCode.UpArrow)) {
			Camera.main.transform.Translate (new Vector3 (0, 5.0f * Time.deltaTime, 0));
		}
		if (Input.GetKey (KeyCode.DownArrow)) {
			Camera.main.transform.Translate (new Vector3 (0, -5.0f * Time.deltaTime, 0));
		}
	}

	// testing for dragging the vertex from one point to another starts here


	public void CastRayToWorld() {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);    
			Vector3 point = ray.origin + (ray.direction * distance);
		}
	}

	// testing for dragging the vertex from one point to another ends here

	public int getSelectedGameObjectIndex(GameObject obj) {
		mainCamera.transform.LookAt (selectedGameObject.transform.position);
		if (obj.tag == "Vertex") {
			vertexPanel.SetActive (true);
			edgePanel.SetActive (false);
			GameObject[] vertices = GameObject.FindGameObjectsWithTag ("Vertex");
			for (int i = 0; i < vertices.Length; i++) {
				GameObject vertex = vertices [i];
				if (vertex == obj) {
					vertexName.text = map.vertices [i].name;
					vertexDescription.text = map.vertices [i].description;
					selectedVertexIndex = i;
					return i;
				}
			}
		}

		if (obj.tag == "Edge") {
			edgePanel.SetActive (true);
			vertexPanel.SetActive (false);
			GameObject[] edges = GameObject.FindGameObjectsWithTag ("Edge");
			for (int i = 0; i < edges.Length; i++) {
				GameObject edge = edges [i];
				if (edge == obj) {
					edgeDirection.text = map.edges [i].direction;
					selectedEdgeIndex = i;
					return i;
				}
			}
		}
		return -1;
	}

}// GraphController