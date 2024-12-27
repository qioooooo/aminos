using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Web.Security
{
	// Token: 0x0200031D RID: 797
	[SuppressUnmanagedCodeSecurity]
	[ComVisible(false)]
	internal static class NativeComInterfaces
	{
		// Token: 0x04001E37 RID: 7735
		internal const int ADS_SETTYPE_FULL = 1;

		// Token: 0x04001E38 RID: 7736
		internal const int ADS_SETTYPE_DN = 4;

		// Token: 0x04001E39 RID: 7737
		internal const int ADS_FORMAT_PROVIDER = 10;

		// Token: 0x04001E3A RID: 7738
		internal const int ADS_FORMAT_SERVER = 9;

		// Token: 0x04001E3B RID: 7739
		internal const int ADS_FORMAT_X500_DN = 7;

		// Token: 0x04001E3C RID: 7740
		internal const int ADS_ESCAPEDMODE_ON = 2;

		// Token: 0x04001E3D RID: 7741
		internal const int ADS_ESCAPEDMODE_OFF = 3;

		// Token: 0x0200031E RID: 798
		[Guid("080d0d78-f421-11d0-a36e-00c04fb950dc")]
		[ComImport]
		internal class Pathname
		{
			// Token: 0x06002750 RID: 10064
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern Pathname();
		}

		// Token: 0x0200031F RID: 799
		[Guid("D592AED4-F420-11D0-A36E-00C04FB950DC")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		internal interface IAdsPathname
		{
			// Token: 0x06002751 RID: 10065
			[SuppressUnmanagedCodeSecurity]
			int Set([MarshalAs(UnmanagedType.BStr)] [In] string bstrADsPath, [MarshalAs(UnmanagedType.U4)] [In] int lnSetType);

			// Token: 0x06002752 RID: 10066
			int SetDisplayType([MarshalAs(UnmanagedType.U4)] [In] int lnDisplayType);

			// Token: 0x06002753 RID: 10067
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.BStr)]
			string Retrieve([MarshalAs(UnmanagedType.U4)] [In] int lnFormatType);

			// Token: 0x06002754 RID: 10068
			[return: MarshalAs(UnmanagedType.U4)]
			int GetNumElements();

			// Token: 0x06002755 RID: 10069
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetElement([MarshalAs(UnmanagedType.U4)] [In] int lnElementIndex);

			// Token: 0x06002756 RID: 10070
			void AddLeafElement([MarshalAs(UnmanagedType.BStr)] [In] string bstrLeafElement);

			// Token: 0x06002757 RID: 10071
			void RemoveLeafElement();

			// Token: 0x06002758 RID: 10072
			[return: MarshalAs(UnmanagedType.Interface)]
			object CopyPath();

			// Token: 0x06002759 RID: 10073
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetEscapedElement([MarshalAs(UnmanagedType.U4)] [In] int lnReserved, [MarshalAs(UnmanagedType.BStr)] [In] string bstrInStr);

			// Token: 0x1700083B RID: 2107
			// (get) Token: 0x0600275A RID: 10074
			// (set) Token: 0x0600275B RID: 10075
			int EscapedMode
			{
				get; [SuppressUnmanagedCodeSecurity]
				set;
			}
		}

		// Token: 0x02000320 RID: 800
		[Guid("927971f5-0939-11d1-8be1-00c04fd8d503")]
		[ComImport]
		internal class LargeInteger
		{
			// Token: 0x0600275C RID: 10076
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern LargeInteger();
		}

		// Token: 0x02000321 RID: 801
		[Guid("9068270b-0939-11d1-8be1-00c04fd8d503")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		internal interface IAdsLargeInteger
		{
			// Token: 0x1700083C RID: 2108
			// (get) Token: 0x0600275D RID: 10077
			// (set) Token: 0x0600275E RID: 10078
			long HighPart
			{
				[SuppressUnmanagedCodeSecurity]
				get;
				[SuppressUnmanagedCodeSecurity]
				set;
			}

			// Token: 0x1700083D RID: 2109
			// (get) Token: 0x0600275F RID: 10079
			// (set) Token: 0x06002760 RID: 10080
			long LowPart
			{
				[SuppressUnmanagedCodeSecurity]
				get;
				[SuppressUnmanagedCodeSecurity]
				set;
			}
		}
	}
}
