using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PSFlocking;

/*! An example class that adds custom units, with a custom flocking behaviour to the PSUnitManager. */
public class PSCustomFlockingExample : MonoBehaviour {

	public GameObject manager;

	/**
 	 * @brief Called once from Unity. Do not call manually.
	 */
	void Start () 
	{
		// Get the Custom Units, and add them to the units-list in the PSUnitManager
		GameObject[] customUnits = GameObject.FindGameObjectsWithTag("CustomUnit");
		foreach (GameObject customUnit in customUnits) 
		{
			manager.GetComponent<PSUnitManager>().AddFlockingUnit (customUnit);
		}
	}
}
