using System;

namespace System.Data.Design
{
	[Serializable]
	internal sealed class NameValidationException : ApplicationException
	{
		public NameValidationException(string message)
			: base(message)
		{
		}
	}
}
