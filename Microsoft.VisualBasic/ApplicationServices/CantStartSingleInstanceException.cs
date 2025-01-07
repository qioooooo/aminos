using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using Microsoft.VisualBasic.CompilerServices;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Serializable]
	public class CantStartSingleInstanceException : Exception
	{
		public CantStartSingleInstanceException()
			: base(Utils.GetResourceString("AppModel_SingleInstanceCantConnect"))
		{
		}

		public CantStartSingleInstanceException(string message)
			: base(message)
		{
		}

		public CantStartSingleInstanceException(string message, Exception inner)
			: base(message, inner)
		{
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected CantStartSingleInstanceException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
