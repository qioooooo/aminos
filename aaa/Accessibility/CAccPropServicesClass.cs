using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Accessibility
{
	// Token: 0x0200000A RID: 10
	[TypeLibType(2)]
	[ClassInterface(0)]
	[Guid("B5F8350B-0548-48B1-A6EE-88BD00B4A5E7")]
	[ComConversionLoss]
	[ComImport]
	public class CAccPropServicesClass : IAccPropServices, CAccPropServices
	{
		// Token: 0x06000028 RID: 40
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public extern CAccPropServicesClass();

		// Token: 0x06000029 RID: 41
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void SetPropValue([In] ref byte pIDString, [In] uint dwIDStringLen, [In] Guid idProp, [MarshalAs(UnmanagedType.Struct)] [In] object var);

		// Token: 0x0600002A RID: 42
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void SetPropServer([In] ref byte pIDString, [In] uint dwIDStringLen, [In] ref Guid paProps, [In] int cProps, [MarshalAs(UnmanagedType.Interface)] [In] IAccPropServer pServer, [In] AnnoScope AnnoScope);

		// Token: 0x0600002B RID: 43
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void ClearProps([In] ref byte pIDString, [In] uint dwIDStringLen, [In] ref Guid paProps, [In] int cProps);

		// Token: 0x0600002C RID: 44
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void SetHwndProp([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [In] Guid idProp, [MarshalAs(UnmanagedType.Struct)] [In] object var);

		// Token: 0x0600002D RID: 45
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void SetHwndPropStr([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [In] Guid idProp, [MarshalAs(UnmanagedType.LPWStr)] [In] string str);

		// Token: 0x0600002E RID: 46
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void SetHwndPropServer([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [In] ref Guid paProps, [In] int cProps, [MarshalAs(UnmanagedType.Interface)] [In] IAccPropServer pServer, [In] AnnoScope AnnoScope);

		// Token: 0x0600002F RID: 47
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void ClearHwndProps([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [In] ref Guid paProps, [In] int cProps);

		// Token: 0x06000030 RID: 48
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void ComposeHwndIdentityString([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [Out] IntPtr ppIDString, out uint pdwIDStringLen);

		// Token: 0x06000031 RID: 49
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void DecomposeHwndIdentityString([In] ref byte pIDString, [In] uint dwIDStringLen, [ComAliasName("Accessibility.wireHWND")] [Out] IntPtr phwnd, out uint pidObject, out uint pidChild);

		// Token: 0x06000032 RID: 50
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void SetHmenuProp([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [In] Guid idProp, [MarshalAs(UnmanagedType.Struct)] [In] object var);

		// Token: 0x06000033 RID: 51
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void SetHmenuPropStr([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [In] Guid idProp, [MarshalAs(UnmanagedType.LPWStr)] [In] string str);

		// Token: 0x06000034 RID: 52
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void SetHmenuPropServer([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [In] ref Guid paProps, [In] int cProps, [MarshalAs(UnmanagedType.Interface)] [In] IAccPropServer pServer, [In] AnnoScope AnnoScope);

		// Token: 0x06000035 RID: 53
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void ClearHmenuProps([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [In] ref Guid paProps, [In] int cProps);

		// Token: 0x06000036 RID: 54
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void ComposeHmenuIdentityString([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [Out] IntPtr ppIDString, out uint pdwIDStringLen);

		// Token: 0x06000037 RID: 55
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		public virtual extern void DecomposeHmenuIdentityString([In] ref byte pIDString, [In] uint dwIDStringLen, [ComAliasName("Accessibility.wireHMENU")] [Out] IntPtr phmenu, out uint pidChild);
	}
}
