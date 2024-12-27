using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Accessibility
{
	// Token: 0x02000004 RID: 4
	[Guid("7852B78D-1CFD-41C1-A615-9C0C85960B5F")]
	[InterfaceType(1)]
	[ComConversionLoss]
	[ComImport]
	public interface IAccIdentity
	{
		// Token: 0x06000017 RID: 23
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetIdentityString([In] uint dwIDChild, [Out] IntPtr ppIDString, out uint pdwIDStringLen);
	}
}
