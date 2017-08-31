using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Unity3D C# Coding Guidelines: http://wiki.unity3d.com/index.php/Csharp_Coding_Guidelines
namespace PSFlocking
{

	/*! The manager that holds a list of all units, and all attributes about the flock. */
	public class PSUnitManager : MonoBehaviour 
	{
		//! Array holding the boids
		public List<GameObject> units;
		//public GameObject[] units;

		//! Will not place any boids automatically in the scene.
		public bool manualStart = false;

		//! Prefab Object from which the Boids will be created.
		public GameObject unitPrefab;

		//! Number of Boids that will be created.
		public int numUnits = 100;

		//! The Spawnrange in which the Boids will be instantiated.
		public Vector3 spawnRange = new Vector3(10.0f, 5f, 10.0f);

		//! A Goal the Boids will follow, if seekGoal is set to true.
		public GameObject goal;

		//! If set to true, the Boids will follow the "goal" GameObject.
		public bool seekGoal = true;

		//! Will apply random forces to boids from time to time, if set to true.
		public bool obedient = true;



		// - - - - - - - - - - 
		// Force & Velocity
		// - - - - - - - - - - 

		[Range(0,10)]
		//! The maximum Force that will  be applied to the Boids.
		public float maxForce = 4.0f;

		[Range(0,5)]
		//! The maximum Velocity the Boids can reach.
		public float maxvelocity = 2.0f;



		// - - - - - - - - - - 
		// Alignemnt
		// - - - - - - - - - - 

		[Range(0, 1)]
		//! Sets how much the Boids will align to each other.
		public float alignmentStrength = 0.5f;

		[Range(0, 25)]
		//! The maximum distance that Boids can be away from each other to make alignmet work.
		public float alignmentDistance = 6;



		// - - - - - - - - - - 
		// Cohesion
		// - - - - - - - - - - 

		[Range(0, 1)]
		//! Sets how much the Boids will stick to each other.
		public float cohesionStrength = 0.5f;

		[Range(0, 25)]
		//! The maximum distance that Boids can be away from each other to make cohesion work.
		public float cohesionDistance = 6;



		// - - - - - - - - - - 
		// Separation
		// - - - - - - - - - - 
		[Range(0, 1)]
		//! Sets how much the Boids will try to move away from each other.
		public float separationStrength = 0.5f;

		[Range(0, 25)]
		//! The maximum distance that Boids can be away from each other to make separation work.
		public float separationDistance = 5;



		// - - - - - - - - - - 
		// Others
		// - - - - - - - - - - 

		[Range(0, 1)]
		//! Sets how much randomized Force should be applied to the Boids.
		public float randomizerStrength = 0.2f;

		[Range(0, 360)]
		//! The angle in which a Boid can "see" others. Cohesion, Alignment, Separation will only be applied if Boids are within the viewing Angle.
		public float viewingAngle = 170.0f;



		/**
		 * @brief Called once from Unity. Do not call manually.
		 * Sets up the units and places them at a random position within the SpawnRange.
		 */
		protected void Start() 
		{
			//units = new GameObject[numUnits];
			units = new List<GameObject>();

			if (manualStart) 
			{
				// manually find all gameobjects
				units.AddRange(GameObject.FindGameObjectsWithTag("Boid"));
				foreach (GameObject unit in units) 
				{

					// Add the PSFlockingUnit Component to the boid, if not already attached
					PSFlockingUnit flockingUnit = unit.GetComponent<PSFlockingUnit>();
					if (flockingUnit == null) 
					{
						flockingUnit = unit.AddComponent<PSFlockingUnit>();
						flockingUnit.Manager = this.gameObject;
					}
					unit.GetComponent<PSFlockingUnit>().Manager = this.gameObject;
				}

			} else 
			{
				for (int i = 0; i < numUnits; i++) 
				{
					// create a random position where the gameobject will be spawned
					Vector3 unitPos = new Vector3(Random.Range(-spawnRange.x, spawnRange.x), 0.5f, Random.Range(-spawnRange.z, spawnRange.z));

					// create a new unit, and add a PSFlockingUnit Component to it, if none is attached
					GameObject unit = Instantiate (unitPrefab, unitPos, Quaternion.LookRotation(new Vector3 (Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)))) as GameObject;
					if (unit.GetComponent<PSFlockingUnit>() == null) 
					{
						unit.AddComponent<PSFlockingUnit>();
					}

					// add a rigidbody if none is attached
					if (unit.GetComponent<Rigidbody>() == null) 
					{
						Rigidbody rb = unit.AddComponent<Rigidbody>();	
						rb.useGravity = false;
					}

					// configuer the PSFlockingUnit and add the unit to the list and 
					unit.GetComponent<PSFlockingUnit>().Manager = this.gameObject;
					units.Add(unit);
				}
			}
		}


		#region Manage FlockUnits

		/**
		 * @brief Adds a unit to the unit list
		 * If the passed gameobject does not have a PSFlockingUnit or a Rigibdoy attached to it, it adds both. The added rigidbody will not use gravity.
		 * @param flocker GameObject that should be added to the units list.
		 */
		public void AddFlockingUnit(GameObject flocker)
		{
			// early out if parameter is null
			if (flocker == null) 
			{
				return;
			}


			// add a PSFlockingUnit Component to the GameObject if not already attached to it
			PSFlockingUnit flockingUnit = flocker.GetComponent<PSFlockingUnit>();
			if (flockingUnit == null) 
			{
				flockingUnit = flocker.AddComponent<PSFlockingUnit>();
			}

			// configure the PSFlockingUnit component
			flockingUnit.Manager = this.gameObject;

			// Add a rigidbody Component to the GameObject if not already attached to it
			Rigidbody rigidbody = flocker.GetComponent<Rigidbody>();
			if (rigidbody == null) 
			{
				Rigidbody rb = flocker.AddComponent<Rigidbody>();
				rb.useGravity = false;
			}

			// Add the flocker to the units
			this.units.Add(flocker);
		}


		/**
		 * @brief Removes a unit from the units-list.
		 * If the passed unit is not within the unit list, this method does nothing.
		 * @param flocker GameObject that shold be removed from the units list.
		 */
		public void RemoveFlockingUnit(GameObject flocker) 
		{
			// early out if parameter is null
			if (flocker == null) 
			{
				return;
			}


			if (this.units.Contains(flocker))
			{
				this.units.Remove(flocker);
			}
		}

		#endregion
	}
}



/*! \mainpage PSFlockingLibrary
 *
 * \section intro_sec Introduction
 *
 * This is the introduction.
 *
 * \section install_sec Installation
 *
 * \subsection step1 Step 1: Opening the box
 *  
 */