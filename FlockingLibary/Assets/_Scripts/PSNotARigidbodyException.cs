using System;


public class PSNotARigidbodyException : Exception
{

	private static int errorCode = 1001;

	private String errorMessage;


	public PSNotARigidbodyException(String errorMessage)
	{
		this.errorMessage = errorMessage;
	}

	public override String ToString() {
		// create a string describing this Exception
		String message = "PSNotARigidbodyException (errorCode: " + errorCode + ")";

		// add custom errorMessage if available
		if (this.errorMessage != null) 
		{
			message = message + ", errorMessage: " + this.errorMessage;
		} 

		return message;
	}
}


