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
		return new Vector3(1.0f, 0.0f, 0.0f);
	}

	protected override Vector3 Cohesion() 
	{
		return new Vector3(1.0f, 0.0f, 0.0f);
	}

	protected override Vector3 Separation() 
	{
		return new Vector3(1.0f, 0.0f, 0.0f);
	}

	protected override Vector3 SeekGoal() 
	{
		return new Vector3(1.0f, 0.0f, 0.0f);
	}
}


