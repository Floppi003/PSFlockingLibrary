using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*! An example class that makes the GameObject holding it look at the center of the flock. It is used for the example scenes on the camera. */
public class PSCameraManager : MonoBehaviour {
	
	/**
	 * @brief Called periodically from Unity. Do not call manually.
	 */
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
