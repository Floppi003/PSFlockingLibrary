using System;
using UnityEngine;
using PSFlocking;

public class PSCustomFlocker : PSFlockingUnit
{

	/*! An example class that implements custom movement. It is very primitive - it moves the unit along the x-axis. */
	public PSCustomFlocker()
	{
	}

	/**
	 * @brief Align function, overridden from PSFlockingUnit
	 * @return Vector3 Alignment for the Unit, will be applied as a force on its rigidbody.
	 */
	protected override Vector3 Align() 
	{
		return new Vector3(1.0f, 0.0f, 0.0f);
	}

	/**
	 * @brief Cohesion function, overridden from PSFlockingUnit
	 * @return Vector3 Cohesion for the Unit, will be applied as a force on its rigidbody.
	 */
	protected override Vector3 Cohesion() 
	{
		return new Vector3(1.0f, 0.0f, 0.0f);
	}

	/**
	 * @brief Separation function, overridden from PSFlockingUnit
	 * @return Vector3 Separation for the Unit, will be applied as a force on its rigidbody.
	 */
	protected override Vector3 Separation() 
	{
		return new Vector3(1.0f, 0.0f, 0.0f);
	}

	/**
	 * @brief SeekGoal function, overridden from PSFlockingUnit
	 * @return Vector3 Vector towards the goal GameObject of PSUnitManager, or a zero-vector if that variable is not set.
	 */
	protected override Vector3 SeekGoal() 
	{
		return new Vector3(1.0f, 0.0f, 0.0f);
	}
}


