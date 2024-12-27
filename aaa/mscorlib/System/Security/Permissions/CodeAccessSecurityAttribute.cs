using System;
using System.Runtime.InteropServices;

namespace System.Security.Permissions
{
	// Token: 0x0200061B RID: 1563
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public abstract class CodeAccessSecurityAttribute : SecurityAttribute
	{
		// Token: 0x060038DD RID: 14557 RVA: 0x000C0FA1 File Offset: 0x000BFFA1
		protected CodeAccessSecurityAttribute(SecurityAction action)
			: base(action)
		{
		}
	}
}
