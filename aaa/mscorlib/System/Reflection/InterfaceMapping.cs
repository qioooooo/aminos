using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002FA RID: 762
	[ComVisible(true)]
	public struct InterfaceMapping
	{
		// Token: 0x04000B1A RID: 2842
		[ComVisible(true)]
		public Type TargetType;

		// Token: 0x04000B1B RID: 2843
		[ComVisible(true)]
		public Type InterfaceType;

		// Token: 0x04000B1C RID: 2844
		[ComVisible(true)]
		public MethodInfo[] TargetMethods;

		// Token: 0x04000B1D RID: 2845
		[ComVisible(true)]
		public MethodInfo[] InterfaceMethods;
	}
}
