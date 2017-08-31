using System;


public class PSNotAFlockingUnitException : Exception
{

	private static int errorCode = 1000;

	private String errorMessage;

	public PSNotAFlockingUnitException(String errorMessage)
	{
		this.errorMessage = errorMessage;	
	}

	public override String ToString() {
		// create a string describing this Exception
		String message = "PSNotAFlockingUnitException (errorCode: " + errorCode + ")";

		// add custom errorMessage if available
		if (this.errorMessage != null) 
		{
			message = message + ", errorMessage: " + this.errorMessage;
		} 

		return message;
	}
}


