using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDebugScript : MonoBehaviour {

	public GameObject follower;

	public Material debugMat;
	public Material debugMatResist;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnRenderObject(){
		if (GetComponent<FlowFieldScript>().DebugOn){
			debugMat.SetPass (0);

			for (int i = 0; i < follower.GetComponent<VehicleScript>().path.Length; i++){
				GL.Begin (GL.LINES);
				GL.Vertex (follower.GetComponent<VehicleScript> ().path [i].transform.position);
				if ((i + 1) == follower.GetComponent<VehicleScript>().path.Length){
					GL.Vertex (follower.GetComponent<VehicleScript> ().path [0].transform.position);
				}
				else {
					GL.Vertex (follower.GetComponent<VehicleScript> ().path [i + 1].transform.position);
				}
				GL.End ();
			}
				

			/*debugMatResist.SetPass (0);
			GL.Begin (GL.LINES);
			//GL.Vertex (follower.GetComponent<VehicleScript> ().resistanceCenter);
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x + 10f, follower.GetComponent<VehicleScript> ().resistanceCenter.y, follower.GetComponent<VehicleScript> ().resistanceCenter.z));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x + 4.47f, follower.GetComponent<VehicleScript> ().resistanceCenter.y, follower.GetComponent<VehicleScript> ().resistanceCenter.z + 4.47f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x, follower.GetComponent<VehicleScript> ().resistanceCenter.y, follower.GetComponent<VehicleScript> ().resistanceCenter.z + 10f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x - 4.47f, follower.GetComponent<VehicleScript> ().resistanceCenter.y, follower.GetComponent<VehicleScript> ().resistanceCenter.z + 4.47f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x - 10f, follower.GetComponent<VehicleScript> ().resistanceCenter.y, follower.GetComponent<VehicleScript> ().resistanceCenter.z));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x - 4.47f, follower.GetComponent<VehicleScript> ().resistanceCenter.y, follower.GetComponent<VehicleScript> ().resistanceCenter.z - 4.47f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x, follower.GetComponent<VehicleScript> ().resistanceCenter.y, follower.GetComponent<VehicleScript> ().resistanceCenter.z - 10f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x + 4.47f, follower.GetComponent<VehicleScript> ().resistanceCenter.y, follower.GetComponent<VehicleScript> ().resistanceCenter.z - 4.47f));
			GL.End ();*/
			
		}

	}
}
