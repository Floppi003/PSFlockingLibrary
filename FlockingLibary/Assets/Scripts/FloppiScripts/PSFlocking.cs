using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSFlocking : MonoBehaviour {

	// movement
	private float speed;
	public float acceleration;
	public float maximumSpeed;

	// angular movement
	private float angularSpeed = 40;
	public float angularAcceleration;
	public float maximumAngularSpeed;

	// sight
	public float sightDistance;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// get the angle from forwardVector to centerOfFlock
		Vector3 centerOfFlock = this.findCenterOfFlock();
		Vector3 lookAtFlock = centerOfFlock - this.transform.position;
		lookAtFlock.Normalize ();
		float angleInDegrees = Vector3.Angle (this.transform.forward, lookAtFlock);
		int rotationDirection = 1;
		if (angleInDegrees < 0) {
			rotationDirection = -1;
		}

		// get a rotation axis
		Vector3 rotationAxis = Vector3.Cross(this.transform.forward, lookAtFlock);
		this.transform.RotateAround (this.transform.position, rotationAxis, this.angularSpeed * Time.deltaTime * rotationDirection);


		// move gameobject forward
		this.transform.position += this.transform.forward * speed * Time.deltaTime;

		// accelerate gameobject
		this.speed += this.acceleration * Time.deltaTime;

		// clamp to maximum speed
		this.speed = Mathf.Min(this.speed, this.maximumSpeed);

		// turn boid towards center of visible flock
		//this.transform.LookAt(this.findCenterOfFlock());

	}



	private Vector3 findCenterOfFlock() {

		// get all boids
		GameObject[] boids = GameObject.FindGameObjectsWithTag ("Boid");

		// find center
		Vector3 center = new Vector3(0, 0, 0);
		int boidsInSightDistance = 0;
		foreach (GameObject boid in boids) {

			// don't consider the boid itself 
			if (boid == this.gameObject) {
				continue;
			}

			// check if the boid is within the sightDistance
			float distance = Vector3.Distance(this.transform.position, boid.transform.position);
			if (distance < sightDistance) {
				center += boid.transform.position;
				boidsInSightDistance++;
			}
		}

		// get the center
		center = center / boidsInSightDistance;
		return center;
	}
}
