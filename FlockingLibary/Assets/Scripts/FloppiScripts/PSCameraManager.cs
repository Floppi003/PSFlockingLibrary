using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSCameraManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// focus the camera on the boids
		GameObject[] boids = GameObject.FindGameObjectsWithTag("Boid");

		// find the center of all boids
		Vector3 center = new Vector3(0, 0, 0);
		foreach (GameObject boid in boids) {
			center += boid.transform.position;
		}
		center = center / boids.Length;

		// make the camera look at the center
		this.transform.LookAt(center);
	}
}
