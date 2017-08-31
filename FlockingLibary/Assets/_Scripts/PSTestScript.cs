using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSTestScript : MonoBehaviour 
{

	public GameObject boid;
	public GameObject manager;
	private float timeUntilNextFlockerSpawn = 2.0f;

	// Use this for initialization
	void Start() 
	{
		
	}
	
	// Update is called once per frame
	void Update() 
	{
		this.timeUntilNextFlockerSpawn = this.timeUntilNextFlockerSpawn - Time.deltaTime;

		if (this.timeUntilNextFlockerSpawn < 0.0f) {
			this.timeUntilNextFlockerSpawn = 4.0f;

			GameObject newObject = GameObject.Instantiate(boid);
			manager.GetComponent<PSUnitManager>().AddFlockingUnit (newObject);

			/*
			try 
			{
			manager.GetComponent<PSUnitManager>().AddFlockingUnit (newObject);
			} catch (PSNotAFlockingUnitException e) {
				Debug.Log ("catched PSNotAFlockingUnitException: " + e);
				newObject.AddComponent<PSFlockingUnit> ();
				newObject.GetComponent<PSFlockingUnit> ().Manager = this.manager;
			}

			try 
			{
				manager.GetComponent<PSUnitManager>().AddFlockingUnit (newObject);
			} catch (PSNotAFlockingUnitException e) {
				Debug.Log ("catched again! PSNotAFlockingUnitException: " + e);
			}
			*/
		}
	}
}
