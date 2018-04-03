using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour {

	public GameObject[] flock;
	public Vector3 flockCenter = Vector3.zero;
	public Vector3 avgDir = Vector3.zero;

	private Vector3 direction;

	public Vector3 currOOBTarget = Vector3.zero;

	//public GameObject centerObj;
	//public GameObject frontObj;
	public Material dirColor;

	// Use this for initialization
	void Start () {
		flock = GameObject.FindGameObjectsWithTag ("flocker");
	}
	
	// Update is called once per frame
	void Update () {
		FindCenter ();
		FindDirection ();
		NewOOBTarget ();
	}

	/// <summary>
	/// Finds the center of the flock
	/// </summary>
	void FindCenter (){
		float avgX = 0;
		float avgY = 0;
		float avgZ = 0;

		for (int i = 0; i < flock.Length; i++){
			avgX += flock [i].transform.position.x;
			avgY += flock [i].transform.position.y;
			avgZ += flock [i].transform.position.z;
		}

		avgX /= flock.Length;
		avgY /= flock.Length;
		avgZ /= flock.Length;

		flockCenter = new Vector3 (avgX, avgY, avgZ);

		transform.position = flockCenter;
		//centerObj.transform.position = flockCenter;
		//frontObj.transform.position = transform.position + transform.forward * 15f;
	}

	/// <summary>
	/// Finds the direction of the flock
	/// </summary>
	void FindDirection (){
		avgDir = Vector3.zero;

		for (int i = 0; i < flock.Length; i++){
			avgDir += flock [i].GetComponent<FlockerScript> ().direction;
		}

		avgDir.Normalize ();

		gameObject.transform.forward = avgDir;
		//frontObj.transform.forward = -avgDir;
	}

	/// <summary>
	/// Determines location that the flock will seek when out of bounds
	/// </summary>
	void NewOOBTarget(){
		float rY = 0;
		float rX = 0;
		float rZ = 0;


		rX = Random.Range (-250f, 250f);
		rZ = Random.Range (-250f, 250f);

		currOOBTarget = new Vector3 (rX, 500, rZ);
	}


}
