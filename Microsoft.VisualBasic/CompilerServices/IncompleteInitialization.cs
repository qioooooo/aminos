using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Serializable]
	public sealed class IncompleteInitialization : Exception
	{
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		private IncompleteInitialization(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IncompleteInitialization(string message)
			: base(message)
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IncompleteInitialization(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		public IncompleteInitialization()
		{
		}
	}
}
