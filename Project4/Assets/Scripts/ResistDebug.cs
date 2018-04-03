using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResistDebug : MonoBehaviour {

	public GameObject follower;
	public Material debugMatResist;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnRenderObject(){
		if (GetComponent<FlowFieldScript>().DebugOn){
			


			debugMatResist.SetPass (0);
			GL.Begin (GL.LINES);
			//GL.Vertex (follower.GetComponent<VehicleScript> ().resistanceCenter);
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x + 10f, follower.GetComponent<VehicleScript> ().resistanceCenter.y + .2f, follower.GetComponent<VehicleScript> ().resistanceCenter.z));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x + 4.47f, follower.GetComponent<VehicleScript> ().resistanceCenter.y + .2f, follower.GetComponent<VehicleScript> ().resistanceCenter.z + 4.47f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x, follower.GetComponent<VehicleScript> ().resistanceCenter.y + .2f, follower.GetComponent<VehicleScript> ().resistanceCenter.z + 10f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x - 4.47f, follower.GetComponent<VehicleScript> ().resistanceCenter.y + .2f, follower.GetComponent<VehicleScript> ().resistanceCenter.z + 4.47f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x - 10f, follower.GetComponent<VehicleScript> ().resistanceCenter.y + .2f, follower.GetComponent<VehicleScript> ().resistanceCenter.z));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x - 4.47f, follower.GetComponent<VehicleScript> ().resistanceCenter.y + .2f, follower.GetComponent<VehicleScript> ().resistanceCenter.z - 4.47f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x, follower.GetComponent<VehicleScript> ().resistanceCenter.y + .2f, follower.GetComponent<VehicleScript> ().resistanceCenter.z - 10f));
			GL.Vertex (new Vector3 (follower.GetComponent<VehicleScript> ().resistanceCenter.x + 4.47f, follower.GetComponent<VehicleScript> ().resistanceCenter.y + .2f, follower.GetComponent<VehicleScript> ().resistanceCenter.z - 4.47f));
			GL.End ();

		}

	}
}
