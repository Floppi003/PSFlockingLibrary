using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSCameraManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// focus the camera on the units
		GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");

		// find the center of all units
		Vector3 center = new Vector3(0, 0, 0);
		foreach (GameObject unit in units) {
			center += unit.transform.position;
		}
		center = center / units.Length;

		// make the camera look at the center
		this.transform.LookAt(center);
		this.transform.position = center + new Vector3 (0.0f, 24.0f, -16.0f);
	}
}
