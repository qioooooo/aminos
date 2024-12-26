using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Accessibility
{
	// Token: 0x02000006 RID: 6
	[ComConversionLoss]
	[Guid("6E26E776-04F0-495D-80E4-3330352E3169")]
	[InterfaceType(1)]
	[ComImport]
	public interface IAccPropServices
	{
		// Token: 0x06000019 RID: 25
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetPropValue([In] ref byte pIDString, [In] uint dwIDStringLen, [In] Guid idProp, [MarshalAs(UnmanagedType.Struct)] [In] object var);

		// Token: 0x0600001A RID: 26
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetPropServer([In] ref byte pIDString, [In] uint dwIDStringLen, [In] ref Guid paProps, [In] int cProps, [MarshalAs(UnmanagedType.Interface)] [In] IAccPropServer pServer, [In] AnnoScope AnnoScope);

		// Token: 0x0600001B RID: 27
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ClearProps([In] ref byte pIDString, [In] uint dwIDStringLen, [In] ref Guid paProps, [In] int cProps);

		// Token: 0x0600001C RID: 28
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetHwndProp([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [In] Guid idProp, [MarshalAs(UnmanagedType.Struct)] [In] object var);

		// Token: 0x0600001D RID: 29
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetHwndPropStr([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [In] Guid idProp, [MarshalAs(UnmanagedType.LPWStr)] [In] string str);

		// Token: 0x0600001E RID: 30
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetHwndPropServer([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [In] ref Guid paProps, [In] int cProps, [MarshalAs(UnmanagedType.Interface)] [In] IAccPropServer pServer, [In] AnnoScope AnnoScope);

		// Token: 0x0600001F RID: 31
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ClearHwndProps([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [In] ref Guid paProps, [In] int cProps);

		// Token: 0x06000020 RID: 32
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ComposeHwndIdentityString([ComAliasName("Accessibility.wireHWND")] [In] ref _RemotableHandle hwnd, [In] uint idObject, [In] uint idChild, [Out] IntPtr ppIDString, out uint pdwIDStringLen);

		// Token: 0x06000021 RID: 33
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void DecomposeHwndIdentityString([In] ref byte pIDString, [In] uint dwIDStringLen, [ComAliasName("Accessibility.wireHWND")] [Out] IntPtr phwnd, out uint pidObject, out uint pidChild);

		// Token: 0x06000022 RID: 34
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetHmenuProp([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [In] Guid idProp, [MarshalAs(UnmanagedType.Struct)] [In] object var);

		// Token: 0x06000023 RID: 35
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetHmenuPropStr([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [In] Guid idProp, [MarshalAs(UnmanagedType.LPWStr)] [In] string str);

		// Token: 0x06000024 RID: 36
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void SetHmenuPropServer([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [In] ref Guid paProps, [In] int cProps, [MarshalAs(UnmanagedType.Interface)] [In] IAccPropServer pServer, [In] AnnoScope AnnoScope);

		// Token: 0x06000025 RID: 37
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ClearHmenuProps([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [In] ref Guid paProps, [In] int cProps);

		// Token: 0x06000026 RID: 38
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ComposeHmenuIdentityString([ComAliasName("Accessibility.wireHMENU")] [In] ref _RemotableHandle hmenu, [In] uint idChild, [Out] IntPtr ppIDString, out uint pdwIDStringLen);

		// Token: 0x06000027 RID: 39
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void DecomposeHmenuIdentityString([In] ref byte pIDString, [In] uint dwIDStringLen, [ComAliasName("Accessibility.wireHMENU")] [Out] IntPtr phmenu, out uint pidChild);
	}
}
