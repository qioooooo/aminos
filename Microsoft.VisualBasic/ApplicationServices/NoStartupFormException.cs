using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Serializable]
	public class NoStartupFormException : Exception
	{
		public NoStartupFormException()
			: base(Utils.GetResourceString("AppModel_NoStartupForm"))
		{
		}

		public NoStartupFormException(string message)
			: base(message)
		{
		}

		public NoStartupFormException(string message, Exception inner)
			: base(message, inner)
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected NoStartupFormException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
