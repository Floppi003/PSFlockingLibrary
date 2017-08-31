using System;
using UnityEngine;
using PSFlocking;

public class PSCustomFlocker : PSFlockingUnit
{
	public PSCustomFlocker()
	{
	}

	protected override Vector3 Align() 
	{
		Debug.Log ("Align subclass");
		Debug.DrawRay (this.transform.position, new Vector3 (10.0f, 0.0f, 0.0f), Color.green, 1.0f);
		return new Vector3(1.0f, 0.0f, 0.0f);
	}

	protected override Vector3 Cohesion() 
	{
		Debug.Log ("Cohesion subclass");
		return new Vector3(1.0f, 0.0f, 0.0f);
	}

	protected override Vector3 Separation() 
	{
		Debug.Log ("Separation subclass");
		return new Vector3(1.0f, 0.0f, 0.0f);
	}

	protected override Vector3 SeekGoal() 
	{
		return new Vector3(1.0f, 0.0f, 0.0f);
	}
}


