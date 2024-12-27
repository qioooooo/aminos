using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Accessibility
{
	// Token: 0x02000002 RID: 2
	[TypeLibType(4176)]
	[Guid("618736E0-3C3D-11CF-810C-00AA00389B71")]
	[ComImport]
	public interface IAccessible
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1
		[DispId(-5000)]
		object accParent
		{
			[TypeLibFunc(64)]
			[DispId(-5000)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			get;
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000002 RID: 2
		[DispId(-5001)]
		int accChildCount
		{
			[TypeLibFunc(64)]
			[DispId(-5001)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			get;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000003 RID: 3
		[DispId(-5002)]
		object accChild
		{
			[DispId(-5002)]
			[TypeLibFunc(64)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.IDispatch)]
			get;
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000004 RID: 4
		// (set) Token: 0x06000014 RID: 20
		[DispId(-5003)]
		string accName
		{
			[TypeLibFunc(64)]
			[DispId(-5003)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[TypeLibFunc(64)]
			[DispId(-5003)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[param: MarshalAs(UnmanagedType.BStr)]
			[param: In]
			set;
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000005 RID: 5
		// (set) Token: 0x06000015 RID: 21
		[DispId(-5004)]
		string accValue
		{
			[DispId(-5004)]
			[TypeLibFunc(64)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[TypeLibFunc(64)]
			[DispId(-5004)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[param: MarshalAs(UnmanagedType.BStr)]
			[param: In]
			set;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000006 RID: 6
		[DispId(-5005)]
		string accDescription
		{
			[DispId(-5005)]
			[TypeLibFunc(64)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000007 RID: 7
		[DispId(-5006)]
		object accRole
		{
			[DispId(-5006)]
			[TypeLibFunc(64)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Struct)]
			get;
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000008 RID: 8
		[DispId(-5007)]
		object accState
		{
			[DispId(-5007)]
			[TypeLibFunc(64)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Struct)]
			get;
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000009 RID: 9
		[DispId(-5008)]
		string accHelp
		{
			[DispId(-5008)]
			[TypeLibFunc(64)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600000A RID: 10
		[DispId(-5009)]
		int accHelpTopic
		{
			[DispId(-5009)]
			[TypeLibFunc(64)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			get;
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600000B RID: 11
		[DispId(-5010)]
		string accKeyboardShortcut
		{
			[TypeLibFunc(64)]
			[DispId(-5010)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600000C RID: 12
		[DispId(-5011)]
		object accFocus
		{
			[TypeLibFunc(64)]
			[DispId(-5011)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Struct)]
			get;
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600000D RID: 13
		[DispId(-5012)]
		object accSelection
		{
			[TypeLibFunc(64)]
			[DispId(-5012)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.Struct)]
			get;
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600000E RID: 14
		[DispId(-5013)]
		string accDefaultAction
		{
			[TypeLibFunc(64)]
			[DispId(-5013)]
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
		}

		// Token: 0x0600000F RID: 15
		[TypeLibFunc(64)]
		[DispId(-5014)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void accSelect([In] int flagsSelect, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

		// Token: 0x06000010 RID: 16
		[TypeLibFunc(64)]
		[DispId(-5015)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void accLocation(out int pxLeft, out int pyTop, out int pcxWidth, out int pcyHeight, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);

		// Token: 0x06000011 RID: 17
		[TypeLibFunc(64)]
		[DispId(-5016)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Struct)]
		object accNavigate([In] int navDir, [MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varStart);

		// Token: 0x06000012 RID: 18
		[TypeLibFunc(64)]
		[DispId(-5017)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		[return: MarshalAs(UnmanagedType.Struct)]
		object accHitTest([In] int xLeft, [In] int yTop);

		// Token: 0x06000013 RID: 19
		[DispId(-5018)]
		[TypeLibFunc(64)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void accDoDefaultAction([MarshalAs(UnmanagedType.Struct)] [In] [Optional] object varChild);
	}
}
