using System;
using System.ComponentModel;

namespace Microsoft.VisualBasic.CompilerServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Serializable]
	public sealed class StaticLocalInitFlag
	{
		public short State;
	}
}
