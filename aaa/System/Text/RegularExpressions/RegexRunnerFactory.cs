using System;
using System.ComponentModel;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200002D RID: 45
	[EditorBrowsable(EditorBrowsableState.Never)]
	public abstract class RegexRunnerFactory
	{
		// Token: 0x06000231 RID: 561
		protected internal abstract RegexRunner CreateInstance();
	}
}
