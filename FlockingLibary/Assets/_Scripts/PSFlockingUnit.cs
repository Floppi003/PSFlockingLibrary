using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unity3D C# Coding Guidelines: http://wiki.unity3d.com/index.php/Csharp_Coding_Guidelines

public class PSFlockingUnit : MonoBehaviour 
{

	// Reference to the Manager Object which has the PSUnitManager Script attached to it
	public GameObject manager;

	private Vector3 velocity;
	private Vector3 previousPosition = Vector3.zero;

	// For Developers only: change position of goal randomly
	private float timeUntilNextRandom = 2.0f;
	private float maximumTimeUntilNextRandom = 3.0f;
	private float strengthRandomizer;



	#region MonoBehaviour Subclassing

	void Start() 
	{
		// set initial values for velocity
		velocity = new Vector3(Random.Range(0.01f, 0.01f),0f, Random.Range(0.01f, 0.01f));
	}

	void Update() 
	{
		flock();

		// look to the front
		this.transform.LookAt(this.transform.position + (this.transform.position - this.previousPosition));
		this.previousPosition = this.transform.position;

		// update randomizer strength timer
		this.timeUntilNextRandom -= Time.deltaTime;
		if (this.timeUntilNextRandom < 0.0f) 
		{
			this.makeNewRandom();
			this.timeUntilNextRandom = Random.Range(1.0f, this.maximumTimeUntilNextRandom);
		}
	}

	#endregion




	#region Flocking Behaviour

	private Vector3 align() 
	{
		float alignmentDistance = manager.GetComponent<PSUnitManager>().alignmentDistance;

		Vector3 sum = Vector3.zero;
		int count = 0;
		foreach (GameObject other in manager.GetComponent<PSUnitManager>().units) 
		{
			if (other == this.gameObject) 
			{
				continue;
			}

			// check if other boid is within distance 
			float distance = Vector3.Distance (this.transform.position, other.transform.position);
			if (distance < alignmentDistance) 
			{

				// check if other boid is within viewing angle
				if (this.isWithinViewingAngle(other)) 
				{
					sum += other.GetComponent<PSFlockingUnit>().velocity;
					count++;
				}
			}
		}

		if (count > 0) 
		{
			sum /= count;
			float strengthMultiplier = this.manager.GetComponent<PSUnitManager>().alignmentStrength + strengthRandomizer;
			strengthMultiplier = Mathf.Max(strengthMultiplier, 0.0f);
			strengthMultiplier = Mathf.Min(strengthMultiplier, 1.0f);
			Vector3 steer = sum * strengthMultiplier;
			return steer;
		}

		return Vector3.zero;
	}

	private Vector3 cohesion() 
	{
		// get the maximum distance other boids can be away to be still taken into account for cohesion
		float cohesionDistance = manager.GetComponent<PSUnitManager>().cohesionDistance;

		// prepare variables
		Vector3 sum = Vector3.zero;
		int count = 0;

		// get all boids
		foreach (GameObject other in manager.GetComponent<PSUnitManager>().units) 
		{

			// do not compare this boid to itself
			if (other == this.gameObject) 
			{
				continue;
			}

			// get distance from this boid to other boid
			float distance = Vector3.Distance(this.transform.position, other.transform.position);

			// compare if boid is within distance
			if (distance < cohesionDistance) 
			{
				// check if other boid is within viewing angle
				if (this.isWithinViewingAngle(other)) 
				{
					sum += other.transform.position;
					count++;
				}
			}
		}

		if (count > 0) 
		{
			sum /= count;
			Vector3 vectorToMiddle = this.vectorToTarget(sum);
			float strengthMultiplier = manager.GetComponent<PSUnitManager>().cohesionStrength + strengthRandomizer;
			strengthMultiplier = Mathf.Max(strengthMultiplier, 0.0f);
			strengthMultiplier = Mathf.Min(strengthMultiplier, 1.0f);
			vectorToMiddle = vectorToMiddle * strengthMultiplier * 2.0f;
			return vectorToMiddle;
		}

		return Vector3.zero;
	}

	private Vector3 separation() 
	{
		Vector3 force = Vector3.zero;
		foreach (GameObject other in manager.GetComponent<PSUnitManager>().units) 
		{
			if (other == this.gameObject) 
			{
				continue;
			}

			// get the distance to the other object, and the total allowed distance for seperation to work
			float distance = Vector3.Distance(this.transform.position, other.transform.position);
			float separationDistance = this.manager.GetComponent<PSUnitManager>().separationDistance;

			// check if the other unit is within the separation-visibility-distance
			if (distance < separationDistance) 
			{

				// check if the boid is within viewing angle
				if (this.isWithinViewingAngle(other)) 
				{

					// bring the force in a range of 0..1, depending on distance
					float separationForce = Mathf.Pow(1 - (distance / separationDistance), 3) * 10.0f; // makes it exponentially strong when they get really close together
					Vector3 direction = other.transform.position - this.transform.position;
					direction = direction * (-1);
					direction = direction.normalized;
					direction = direction * separationForce;
					force += direction;
				}
			}
		}

		float strengthMultiplier = this.manager.GetComponent<PSUnitManager>().separationStrength + this.strengthRandomizer;
		strengthMultiplier = Mathf.Max(strengthMultiplier, 0.0f);
		strengthMultiplier = Mathf.Min(strengthMultiplier, 1.0f);
		force = force * strengthMultiplier * 2.0f;
		force = force + force.normalized;

		//Debug.DrawRay (this.transform.position, force, Color.magenta);

		return force;
	}

	private void flock() 
	{
		velocity = this.GetComponent<Rigidbody>().velocity;
		Vector3 currentForce = Vector3.zero;

		if (manager.GetComponent<PSUnitManager>().obedient/* && Random.Range (0, 50) <= 1*/) 
		{

			//Vector3 coh = Vector3.zero;
			//Vector3 ali = Vector3.zero;
			//Vector3 separation = Vector3.zero;

			Vector3 ali = align();
			Vector3 coh = this.cohesion();
			Vector3 separation = this.separation();
			Vector3 goal = Vector3.zero;

			// check if there is a goal 
			if (manager.GetComponent<PSUnitManager>().seekGoal) 
			{
				// get the goal
				GameObject goalGO = manager.GetComponent<PSUnitManager>().goal;
				if (goalGO != null) 
				{
					goal = this.vectorToTarget(goalGO.transform.position);
				}
			}

			// add the differenct forces up and normalize
			currentForce = goal + ali + coh + separation;
			currentForce = currentForce.normalized;
		}

		if (manager.GetComponent<PSUnitManager>().willful && Random.Range(0, 50) <= 1) 
		{
			if (Random.Range (0, 50) < 1) 
			{
				currentForce = new Vector3(Random.Range (0.01f, 0.1f), Random.Range(0.01f, 0.1f));
			}
		}

		applyForce(currentForce);
	}

	#endregion




	#region Other Functions

	private Vector3 vectorToTarget(Vector3 target) 
	{
		return(target - this.transform.position);
	}

	private void applyForce(Vector3 force) 
	{
		Vector3 appylingForce = new Vector3(force.x, 0f, force.z);

		if (force.magnitude > manager.GetComponent<PSUnitManager>().maxForce) 
		{
			force = force.normalized;
			force *= manager.GetComponent<PSUnitManager>().maxForce;
		}

		this.GetComponent<Rigidbody>().AddForce(appylingForce);


		if (this.GetComponent<Rigidbody>().velocity.magnitude > manager.GetComponent<PSUnitManager>().maxvelocity) 
		{
			this.GetComponent<Rigidbody>().velocity = this.GetComponent<Rigidbody>().velocity.normalized;
			this.GetComponent<Rigidbody>().velocity *= manager.GetComponent<PSUnitManager>().maxvelocity;
		}

		//Debug.DrawRay (this.transform.position, appylingForce, Color.red);
	}

	private void makeNewRandom() 
	{
		this.strengthRandomizer = Random.Range(0.0f, manager.GetComponent<PSUnitManager>().randomizerStrength) - (manager.GetComponent<PSUnitManager>().randomizerStrength / 2.0f);
	}

	private bool isWithinViewingAngle(GameObject other) 
	{
		// check whether that boid is within the viewing angle
		float viewingAngle = manager.GetComponent<PSUnitManager>().viewingAngle;
		float angle = Vector3.Angle (this.transform.forward, this.vectorToTarget(other.transform.position).normalized);
		if (angle < (viewingAngle / 2.0f)) 
		{
			return true;
		} else 
		{
			return false;
		}
	}

	#endregion
}
