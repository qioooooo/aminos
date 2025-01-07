using System;
using System.Runtime.Serialization;

namespace Aladdin.HASP
{
	[Serializable]
	public class DllBrokenException : Exception
	{
		public DllBrokenException()
		{
		}

		public DllBrokenException(string message)
			: base(message)
		{
		}

		public DllBrokenException(string message, Exception ex)
			: base(message, ex)
		{
		}

		protected DllBrokenException(SerializationInfo serInfo, StreamingContext streamContext)
			: base(serInfo, streamContext)
		{
		}
	}
}
