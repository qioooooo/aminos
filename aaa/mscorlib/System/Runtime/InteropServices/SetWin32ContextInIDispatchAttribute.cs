using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004F2 RID: 1266
	[Obsolete("This attribute has been deprecated.  Application Domains no longer respect Activation Context boundaries in IDispatch calls.", false)]
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	public sealed class SetWin32ContextInIDispatchAttribute : Attribute
	{
	}
}
