using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSelectScript : MonoBehaviour {

	public Camera[] cameras = new Camera[3];
	//Dropdown selector;
	int ctrl = 0;

	// Use this for initialization
	void Start () {
		//selector = GetComponent<Dropdown> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetKeyDown(KeyCode.C)){
			ctrl++;
			if (ctrl == 3){ctrl = 0;}
		}

		if (ctrl == 0){
			cameras [0].gameObject.SetActive(true);
			cameras [1].gameObject.SetActive(false);
			cameras [2].gameObject.SetActive(false);
		}
		else if (ctrl == 1){
			cameras [0].gameObject.SetActive(false);
			cameras [1].gameObject.SetActive(true);
			cameras [2].gameObject.SetActive(false);
		}
		else if (ctrl == 2){
			cameras [0].gameObject.SetActive(false);
			cameras [1].gameObject.SetActive(false);
			cameras [2].gameObject.SetActive(true);
		}
	}

	void OnGUI(){
		GUI.Box (new Rect (new Vector2 (10, 10), new Vector2 (300, 40)), "Press 'C' to switch cameras!\nPress 'F1' to enable debug lines!");
	}
}
