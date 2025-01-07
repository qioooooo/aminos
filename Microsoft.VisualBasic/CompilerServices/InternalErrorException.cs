using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Serializable]
	public sealed class InternalErrorException : Exception
	{
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		private InternalErrorException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public InternalErrorException(string message)
			: base(message)
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public InternalErrorException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public InternalErrorException()
			: base(Utils.GetResourceString("InternalError"))
		{
		}
	}
}
