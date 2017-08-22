using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingUnit : MonoBehaviour {

	public GameObject manager;
	public Vector3 location = Vector3.zero;
	public Vector3 velocity;
	Vector3 goalPos = Vector3.zero;
	Vector3 currentForce;

	void Start() {
		velocity = new Vector3(Random.Range(0.01f, 0.01f),0f, Random.Range(0.01f, 0.01f));
		location = new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
	}

	Vector3 seek(Vector3 target) {
		return(target - location);
	}

	void applyForce(Vector3 force) {
		Vector3 appylingForce = new Vector3 (force.x, 0f, force.z);

		if (force.magnitude > manager.GetComponent<UnitManager> ().maxForce) {
			force = force.normalized;
			force *= manager.GetComponent<UnitManager> ().maxForce;
		}

		this.GetComponent<Rigidbody> ().AddForce (appylingForce);

		if (this.GetComponent<Rigidbody> ().velocity.magnitude > manager.GetComponent<UnitManager> ().maxvelocity) {
			this.GetComponent<Rigidbody> ().velocity = this.GetComponent<Rigidbody> ().velocity.normalized;
			this.GetComponent<Rigidbody> ().velocity *= manager.GetComponent<UnitManager> ().maxvelocity;
		}

		Debug.DrawRay (this.transform.position, appylingForce, Color.red);
	}

	Vector3 align() {
		float neighbordist = manager.GetComponent<UnitManager> ().neighbourDistance;
		Vector3 sum = Vector3.zero;
		int count = 0;
		foreach (GameObject other in manager.GetComponent<UnitManager>().units) {
			if (other == this.gameObject) {
				continue;
			}

			float d = Vector3.Distance (location, other.GetComponent<FlockingUnit> ().location);

			if (d < neighbordist) {
				sum += other.GetComponent<FlockingUnit> ().velocity;
				count++;
			}
		}

		if (count > 0) {
			sum /= count;
			Vector3 steer = sum - velocity;
			return steer;
		}

		return Vector3.zero;
	}

	Vector3 cohesion() {
		float neighbordist = manager.GetComponent<UnitManager> ().neighbourDistance;
		Vector3 sum = Vector3.zero;
		int count = 0;
		foreach (GameObject other in manager.GetComponent<UnitManager>().units) {
			if (other == this.gameObject) {
				continue;
			}

			float d = Vector3.Distance (location, other.GetComponent<FlockingUnit> ().location);

			if (d < neighbordist) {
				sum += other.GetComponent<FlockingUnit> ().location;
				count++;
			}
		}

		if (count > 0) {
			sum /= count;
			return seek (sum);
		}

		return Vector3.zero;
	}

	void flock() {
		location = this.transform.position;
		velocity = this.GetComponent<Rigidbody> ().velocity;

		if (manager.GetComponent<UnitManager> ().obedient && Random.Range (0, 50) <= 1) {
			Vector3 ali = align ();
			Vector3 coh = cohesion ();
			Vector3 gl;

			if (manager.GetComponent<UnitManager> ().seekGoal) {
				gl = seek (goalPos);
				currentForce = gl + ali + coh;
			} else {
				currentForce = ali + coh;
			}

			currentForce = currentForce.normalized;
		}

		if (manager.GetComponent<UnitManager> ().willful && Random.Range (0, 50) <= 1) {
			if (Random.Range (0, 50) < 1) {
				currentForce = new Vector3 (Random.Range (0.01f, 0.1f), Random.Range (0.01f, 0.1f));
			}
		}

		applyForce (currentForce);
	}

	void Update() {
		flock ();
		goalPos = manager.transform.position;	
	}
}
