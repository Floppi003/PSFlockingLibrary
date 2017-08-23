﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FFlockingUnit : MonoBehaviour {

	public GameObject manager;
	public Vector3 location = Vector3.zero;
	public Vector3 velocity;
	Vector3 goalPos = Vector3.zero;
	Vector3 currentForce;
	private Vector3 previousPosition = Vector3.zero;

	void Start() {
		velocity = new Vector3(Random.Range(0.01f, 0.01f),0f, Random.Range(0.01f, 0.01f));
		location = new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
	}

	Vector3 seek(Vector3 target) {
		return(target - location);
	}

	void applyForce(Vector3 force) {
		Vector3 appylingForce = new Vector3 (force.x, 0f, force.z);

		if (force.magnitude > manager.GetComponent<FUnitManager> ().maxForce) {
			force = force.normalized;
			force *= manager.GetComponent<FUnitManager> ().maxForce;
		}

		this.GetComponent<Rigidbody> ().AddForce (appylingForce);


		if (this.GetComponent<Rigidbody> ().velocity.magnitude > manager.GetComponent<FUnitManager> ().maxvelocity) {
			this.GetComponent<Rigidbody> ().velocity = this.GetComponent<Rigidbody> ().velocity.normalized;
			this.GetComponent<Rigidbody> ().velocity *= manager.GetComponent<FUnitManager> ().maxvelocity;
		}

		Debug.DrawRay (this.transform.position, appylingForce, Color.red);
	}

	Vector3 align() {
		float neighbordist = manager.GetComponent<FUnitManager> ().neighbourDistance;
		Vector3 sum = Vector3.zero;
		int count = 0;
		foreach (GameObject other in manager.GetComponent<FUnitManager>().units) {
			if (other == this.gameObject) {
				continue;
			}

			float d = Vector3.Distance (location, other.GetComponent<FFlockingUnit> ().location);

			if (d < neighbordist) {
				sum += other.GetComponent<FFlockingUnit> ().velocity;
				count++;
			}
		}
			
		if (count > 0) {
			sum /= count;
			Vector3 steer = sum * this.manager.GetComponent<FUnitManager>().alignment;
			Debug.Log ("sum: " + sum + ", steer: " + steer);
			return steer;
		}

		return Vector3.zero;
	}

	Vector3 cohesion() {
		float cohesionDistance = manager.GetComponent<FUnitManager> ().cohesionDistance;
		Vector3 sum = Vector3.zero;
		int count = 0;
		foreach (GameObject other in manager.GetComponent<FUnitManager>().units) {
			if (other == this.gameObject) {
				continue;
			}

			float d = Vector3.Distance (location, other.GetComponent<FFlockingUnit> ().location);

			if (d < cohesionDistance) {
				sum += other.GetComponent<FFlockingUnit> ().location;
				count++;
			}
		}

		if (count > 0) {
			sum /= count;
			Vector3 vectorToMiddle = seek (sum);
			vectorToMiddle = vectorToMiddle * manager.GetComponent<FUnitManager>().cohesionStrength * 2.0f;
			return vectorToMiddle;
		}

		return Vector3.zero;
	}

	Vector3 separation() {
		Vector3 force = Vector3.zero;
		foreach (GameObject other in manager.GetComponent<FUnitManager>().units) {
			if (other == this.gameObject) {
				continue;
			}

			float distance = Vector3.Distance (location, other.GetComponent<FFlockingUnit>().location);
			float separationDistance = this.manager.GetComponent<FUnitManager> ().separationDistance;

			// check if the other unit is within the separation-visibility-distance
			if (distance < separationDistance) {
				float separationForce = distance / separationDistance;
				Vector3 direction = other.GetComponent<FFlockingUnit> ().location - location;
				direction = direction * (-1);
				direction = direction.normalized;
				direction = direction * separationForce;
				force += direction;
			}
		}

		float separation = this.manager.GetComponent<FUnitManager> ().separationStrength;
		force = (force.normalized * separation * 2.0f);
		force = force + force.normalized;
		return force;
	}

	void flock() {
		location = this.transform.position;
		velocity = this.GetComponent<Rigidbody> ().velocity;

		if (manager.GetComponent<FUnitManager> ().obedient/* && Random.Range (0, 50) <= 1*/) {

			//Vector3 coh = Vector3.zero;
			Vector3 ali = Vector3.zero;
			//Vector3 separation = Vector3.zero;

			//Vector3 ali = align ();
			Vector3 coh = this.cohesion ();
			Vector3 separation = this.separation();

			Debug.Log ("cohesion: " + coh + ", separation: " + separation);

			Vector3 gl;

			if (manager.GetComponent<FUnitManager> ().seekGoal) {
				gl = seek (goalPos);
				currentForce = gl + ali + coh + separation;
			} else {
				currentForce = ali + coh + separation;
			}

			currentForce = currentForce.normalized;
		}

		if (manager.GetComponent<FUnitManager> ().willful && Random.Range (0, 50) <= 1) {
			if (Random.Range (0, 50) < 1) {
				currentForce = new Vector3 (Random.Range (0.01f, 0.1f), Random.Range (0.01f, 0.1f));
			}
		}

		applyForce (currentForce);
	}

	void Update() {
		flock ();
		goalPos = manager.transform.position;

		// look to the front
		this.transform.LookAt(this.transform.position + (this.transform.position - this.previousPosition));
		this.previousPosition = this.transform.position;
	}
}