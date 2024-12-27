using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Management.Instrumentation
{
	// Token: 0x020000B2 RID: 178
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	[Guid("E5CB7A31-7512-11d2-89CE-0080C792E5D8")]
	[ClassInterface(ClassInterfaceType.None)]
	[ComImport]
	internal class CorMetaDataDispenser
	{
		// Token: 0x06000555 RID: 1365
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public extern CorMetaDataDispenser();
	}
}
