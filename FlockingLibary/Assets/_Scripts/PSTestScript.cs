using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSFlocking;

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
		}
	}
}
