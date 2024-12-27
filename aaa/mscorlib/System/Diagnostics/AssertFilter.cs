using System;

namespace System.Diagnostics
{
	// Token: 0x0200029A RID: 666
	[Serializable]
	internal abstract class AssertFilter
	{
		// Token: 0x06001AB7 RID: 6839
		public abstract AssertFilters AssertFailure(string condition, string message, StackTrace location);
	}
}
