using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowFieldScript : MonoBehaviour {

	public GameObject terrain;
	public Vector3[,] flowField;

	public int xTer = 100;
	public int zTer = 100;

	public Material debugMat;

	public bool DebugOn = false;

	// Use this for initialization
	void Start () {
		//int x = (int)terrain.GetComponent<Terrain> ().terrainData.size.x / 5;
		//int z = (int)terrain.GetComponent<Terrain> ().terrainData.size.z / 5;
		GenerateFF ();
	}
	
	// Update is called once per frame
	void Update () {

		//toggles debug lines **NOTE: debug mode has considerable lag
		if (Input.GetKeyDown(KeyCode.F1)){
			DebugOn = !DebugOn;
		}
	}

	/// <summary>
	/// Generates the Flow Field
	/// </summary>
	void GenerateFF(){

		flowField = new Vector3[xTer, zTer];

		float perlinX = 0f;
		float perlinY = 0f;
		for (int i = 0; i < xTer; i++){
			for (int j = 0; j < zTer; j++){
				float rRot = Mathf.PerlinNoise (perlinX, perlinY) * 360f;
				Vector3 newVec = Quaternion.Euler (0f, rRot, 0f) * Vector3.right;
				newVec.Normalize ();
				newVec.y = 0f;
				flowField [i, j] = newVec;
				Debug.Log ("Added: " + newVec.x + ", " + newVec.y + ", " + newVec.z);
				perlinY += 0.1f;
			}
			perlinX = 0.1f;
		}
	}

	/// <summary>
	/// Raises the render object event.
	/// </summary>
	void OnRenderObject(){
		if (DebugOn){
			debugMat.SetPass (0);
			Debug.Log ("X: " + xTer + " Z: " + zTer);
			for (int i = 0; i < xTer; i++)
			{
				Debug.Log ("in loop!");
				for (int j = 0; j < zTer; j++)
				{
					GL.Begin (GL.LINES);
					Vector3 start = new Vector3 (i * 5, 205f, j * 5);
					Vector3 end = start + flowField [i, j];
					GL.Vertex (start);
					GL.Vertex (end);
					GL.End ();
				}
			}
		}

	}
}
