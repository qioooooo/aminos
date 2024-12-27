using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Deployment.Application
{
	// Token: 0x0200007F RID: 127
	[ClassInterface(ClassInterfaceType.None)]
	[Guid("E5CB7A31-7512-11d2-89CE-0080C792E5D8")]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	[ComImport]
	internal class CorMetaDataDispenser
	{
		// Token: 0x060003EF RID: 1007
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public extern CorMetaDataDispenser();
	}
}
