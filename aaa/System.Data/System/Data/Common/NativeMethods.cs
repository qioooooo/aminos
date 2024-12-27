using System;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace System.Data.Common
{
	// Token: 0x02000156 RID: 342
	internal static class NativeMethods
	{
		// Token: 0x060015A0 RID: 5536
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		internal static extern IntPtr MapViewOfFile(IntPtr hFileMappingObject, int dwDesiredAccess, int dwFileOffsetHigh, int dwFileOffsetLow, IntPtr dwNumberOfBytesToMap);

		// Token: 0x060015A1 RID: 5537
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr OpenFileMappingA(int dwDesiredAccess, bool bInheritHandle, [MarshalAs(UnmanagedType.LPStr)] string lpName);

		// Token: 0x060015A2 RID: 5538
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true)]
		internal static extern IntPtr CreateFileMappingA(IntPtr hFile, IntPtr pAttr, int flProtect, int dwMaximumSizeHigh, int dwMaximumSizeLow, [MarshalAs(UnmanagedType.LPStr)] string lpName);

		// Token: 0x060015A3 RID: 5539
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
		internal static extern bool UnmapViewOfFile(IntPtr lpBaseAddress);

		// Token: 0x060015A4 RID: 5540
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060015A5 RID: 5541
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern bool AllocateAndInitializeSid(IntPtr pIdentifierAuthority, byte nSubAuthorityCount, int dwSubAuthority0, int dwSubAuthority1, int dwSubAuthority2, int dwSubAuthority3, int dwSubAuthority4, int dwSubAuthority5, int dwSubAuthority6, int dwSubAuthority7, ref IntPtr pSid);

		// Token: 0x060015A6 RID: 5542
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern int GetLengthSid(IntPtr pSid);

		// Token: 0x060015A7 RID: 5543
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern bool InitializeAcl(IntPtr pAcl, int nAclLength, int dwAclRevision);

		// Token: 0x060015A8 RID: 5544
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern bool AddAccessDeniedAce(IntPtr pAcl, int dwAceRevision, int AccessMask, IntPtr pSid);

		// Token: 0x060015A9 RID: 5545
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern bool AddAccessAllowedAce(IntPtr pAcl, int dwAceRevision, uint AccessMask, IntPtr pSid);

		// Token: 0x060015AA RID: 5546
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern bool InitializeSecurityDescriptor(IntPtr pSecurityDescriptor, int dwRevision);

		// Token: 0x060015AB RID: 5547
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern bool SetSecurityDescriptorDacl(IntPtr pSecurityDescriptor, bool bDaclPresent, IntPtr pDacl, bool bDaclDefaulted);

		// Token: 0x060015AC RID: 5548
		[DllImport("advapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern IntPtr FreeSid(IntPtr pSid);

		// Token: 0x02000157 RID: 343
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("0c733a1e-2a1c-11ce-ade5-00aa0044773d")]
		[ComImport]
		internal interface ISourcesRowset
		{
			// Token: 0x060015AD RID: 5549
			[PreserveSig]
			OleDbHResult GetSourcesRowset([In] IntPtr pUnkOuter, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid riid, [In] int cPropertySets, [In] IntPtr rgProperties, [MarshalAs(UnmanagedType.Interface)] out object ppRowset);
		}

		// Token: 0x02000158 RID: 344
		[Guid("0C733A5E-2A1C-11CE-ADE5-00AA0044773D")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface ITransactionJoin
		{
			// Token: 0x060015AE RID: 5550
			[Obsolete("not used", true)]
			[PreserveSig]
			int GetOptionsObject();

			// Token: 0x060015AF RID: 5551
			void JoinTransaction([MarshalAs(UnmanagedType.Interface)] [In] object punkTransactionCoord, [In] int isoLevel, [In] int isoFlags, [In] IntPtr pOtherOptions);
		}
	}
}
