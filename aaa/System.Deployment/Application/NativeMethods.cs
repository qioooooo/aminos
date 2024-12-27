using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace System.Deployment.Application
{
	// Token: 0x02000085 RID: 133
	internal static class NativeMethods
	{
		// Token: 0x0600040B RID: 1035
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref NativeMethods.SYSTEM_INFO sysInfo);

		// Token: 0x0600040C RID: 1036
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern void GetNativeSystemInfo([MarshalAs(UnmanagedType.Struct)] ref NativeMethods.SYSTEM_INFO sysInfo);

		// Token: 0x0600040D RID: 1037
		[DllImport("kernel32.dll", BestFitMapping = false, SetLastError = true)]
		public static extern bool VerifyVersionInfo([In] [Out] NativeMethods.OSVersionInfoEx osvi, [In] uint dwTypeMask, [In] ulong dwConditionMask);

		// Token: 0x0600040E RID: 1038
		[DllImport("kernel32.dll")]
		public static extern ulong VerSetConditionMask([In] ulong ConditionMask, [In] uint TypeMask, [In] byte Condition);

		// Token: 0x0600040F RID: 1039
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr LoadLibraryEx(string lpModuleName, IntPtr hFile, uint dwFlags);

		// Token: 0x06000410 RID: 1040
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr LoadLibrary(string lpModuleName);

		// Token: 0x06000411 RID: 1041
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

		// Token: 0x06000412 RID: 1042
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool FreeLibrary(IntPtr hModule);

		// Token: 0x06000413 RID: 1043
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr FindResource(IntPtr hModule, string lpName, string lpType);

		// Token: 0x06000414 RID: 1044
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr LoadResource(IntPtr hModule, IntPtr handle);

		// Token: 0x06000415 RID: 1045
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr LockResource(IntPtr hglobal);

		// Token: 0x06000416 RID: 1046
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint SizeofResource(IntPtr hModule, IntPtr handle);

		// Token: 0x06000417 RID: 1047
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
		internal static extern bool CloseHandle(HandleRef handle);

		// Token: 0x06000418 RID: 1048
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern int GetShortPathName(string LongPath, [Out] StringBuilder ShortPath, int BufferSize);

		// Token: 0x06000419 RID: 1049
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeFileHandle CreateFile(string lpFileName, uint dwDesiredAccess, uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition, uint dwFlagsAndAttributes, IntPtr hTemplateFile);

		// Token: 0x0600041A RID: 1050
		[DllImport("mscorwks.dll", BestFitMapping = false, CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
		internal static extern void CorLaunchApplication(uint hostType, string applicationFullName, int manifestPathsCount, string[] manifestPaths, int activationDataCount, string[] activationData, NativeMethods.PROCESS_INFORMATION processInformation);

		// Token: 0x0600041B RID: 1051
		[DllImport("mscorwks.dll", PreserveSig = false)]
		internal static extern void CreateAssemblyCache(out NativeMethods.IAssemblyCache ppAsmCache, int reserved);

		// Token: 0x0600041C RID: 1052
		[DllImport("mscorwks.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
		[return: MarshalAs(UnmanagedType.IUnknown)]
		internal static extern object GetAssemblyIdentityFromFile([MarshalAs(UnmanagedType.LPWStr)] [In] string filePath, [In] ref Guid riid);

		// Token: 0x0600041D RID: 1053
		[DllImport("mscorwks.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
		internal static extern void CreateAssemblyNameObject(out NativeMethods.IAssemblyName ppEnum, string szAssemblyName, uint dwFlags, IntPtr pvReserved);

		// Token: 0x0600041E RID: 1054
		[DllImport("mscorwks.dll", CharSet = CharSet.Auto, PreserveSig = false)]
		internal static extern void CreateAssemblyEnum(out NativeMethods.IAssemblyEnum ppEnum, NativeMethods.IApplicationContext pAppCtx, NativeMethods.IAssemblyName pName, uint dwFlags, IntPtr pvReserved);

		// Token: 0x0600041F RID: 1055
		[DllImport("mscoree.dll")]
		internal static extern byte StrongNameSignatureVerificationEx([MarshalAs(UnmanagedType.LPWStr)] string filePath, byte forceVerification, out byte wasVerified);

		// Token: 0x06000420 RID: 1056
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		internal static extern IntPtr CreateActCtxW([In] NativeMethods.ACTCTXW actCtx);

		// Token: 0x06000421 RID: 1057
		[DllImport("kernel32.dll", ExactSpelling = true)]
		internal static extern void ReleaseActCtx([In] IntPtr hActCtx);

		// Token: 0x06000422 RID: 1058 RVA: 0x00015D24 File Offset: 0x00014D24
		internal static string GetLoadedModulePath(string moduleName)
		{
			string text = null;
			IntPtr moduleHandle = NativeMethods.GetModuleHandle(moduleName);
			if (moduleHandle != IntPtr.Zero)
			{
				StringBuilder stringBuilder = new StringBuilder(260);
				int moduleFileName = NativeMethods.GetModuleFileName(moduleHandle, stringBuilder, stringBuilder.Capacity);
				if (moduleFileName > 0)
				{
					text = stringBuilder.ToString();
				}
			}
			return text;
		}

		// Token: 0x06000423 RID: 1059
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr GetModuleHandle(string moduleName);

		// Token: 0x06000424 RID: 1060
		[DllImport("kernel32.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetModuleFileName(IntPtr module, [Out] StringBuilder fileName, int size);

		// Token: 0x06000425 RID: 1061
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint GetCurrentThreadId();

		// Token: 0x06000426 RID: 1062
		[DllImport("wininet.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CreateUrlCacheEntry([In] string urlName, [In] int expectedFileSize, [In] string fileExtension, [Out] StringBuilder fileName, [In] int dwReserved);

		// Token: 0x06000427 RID: 1063
		[DllImport("wininet.dll", BestFitMapping = false, CharSet = CharSet.Auto, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool CommitUrlCacheEntry([In] string lpszUrlName, [In] string lpszLocalFileName, [In] long ExpireTime, [In] long LastModifiedTime, [In] uint CacheEntryType, [In] string lpHeaderInfo, [In] int dwHeaderSize, [In] string lpszFileExtension, [In] string lpszOriginalUrl);

		// Token: 0x06000428 RID: 1064
		[DllImport("mscoree.dll", PreserveSig = false)]
		private static extern IntPtr LoadLibraryShim([MarshalAs(UnmanagedType.LPWStr)] string dllName, [MarshalAs(UnmanagedType.LPWStr)] string szVersion, IntPtr reserved);

		// Token: 0x06000429 RID: 1065
		[DllImport("mscoree.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
		public static extern void GetFileVersion(string szFileName, StringBuilder szBuffer, uint cchBuffer, out uint dwLength);

		// Token: 0x0600042A RID: 1066
		[DllImport("mscoree.dll", CharSet = CharSet.Unicode, ExactSpelling = true, PreserveSig = false)]
		public static extern void GetRequestedRuntimeInfo(string pExe, string pwszVersion, string pConfigurationFile, uint startupFlags, uint runtimeInfoFlags, StringBuilder pDirectory, uint dwDirectory, out uint dwDirectoryLength, StringBuilder pVersion, uint cchBuffer, out uint dwLength);

		// Token: 0x0600042B RID: 1067
		[DllImport("mscoree.dll", ExactSpelling = true, PreserveSig = false)]
		internal static extern void StrongNameTokenFromPublicKey(byte[] publicKeyBlob, uint publicKeyBlobCount, ref IntPtr strongNameTokenArray, ref uint strongNameTokenCount);

		// Token: 0x0600042C RID: 1068
		[DllImport("mscoree.dll", ExactSpelling = true, PreserveSig = false)]
		internal static extern void StrongNameFreeBuffer(IntPtr buffer);

		// Token: 0x0600042D RID: 1069
		[DllImport("wininet.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern bool InternetGetCookieW([In] string url, [In] string cookieName, [Out] StringBuilder cookieData, [In] [Out] ref uint bytes);

		// Token: 0x0600042E RID: 1070
		[DllImport("shell32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern void SHChangeNotify(int eventID, uint flags, IntPtr item1, IntPtr item2);

		// Token: 0x0600042F RID: 1071
		[DllImport("shell32.dll", CharSet = CharSet.Unicode)]
		public static extern uint SHCreateItemFromParsingName([MarshalAs(UnmanagedType.LPWStr)] [In] string pszPath, [In] IntPtr pbc, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid riid, [MarshalAs(UnmanagedType.Interface)] out object ppv);

		// Token: 0x06000430 RID: 1072
		[DllImport("Ole32.dll")]
		public static extern uint CoCreateInstance([In] ref Guid clsid, [MarshalAs(UnmanagedType.IUnknown)] object punkOuter, int context, [In] ref Guid iid, [MarshalAs(UnmanagedType.IUnknown)] out object o);

		// Token: 0x040002C0 RID: 704
		public const ushort PROCESSOR_ARCHITECTURE_INTEL = 0;

		// Token: 0x040002C1 RID: 705
		public const ushort PROCESSOR_ARCHITECTURE_IA64 = 6;

		// Token: 0x040002C2 RID: 706
		public const ushort PROCESSOR_ARCHITECTURE_AMD64 = 9;

		// Token: 0x040002C3 RID: 707
		internal static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

		// Token: 0x02000086 RID: 134
		public struct SYSTEM_INFO
		{
			// Token: 0x040002C4 RID: 708
			internal NativeMethods._PROCESSOR_INFO_UNION uProcessorInfo;

			// Token: 0x040002C5 RID: 709
			public uint dwPageSize;

			// Token: 0x040002C6 RID: 710
			public IntPtr lpMinimumApplicationAddress;

			// Token: 0x040002C7 RID: 711
			public IntPtr lpMaximumApplicationAddress;

			// Token: 0x040002C8 RID: 712
			public IntPtr dwActiveProcessorMask;

			// Token: 0x040002C9 RID: 713
			public uint dwNumberOfProcessors;

			// Token: 0x040002CA RID: 714
			public uint dwProcessorType;

			// Token: 0x040002CB RID: 715
			public uint dwAllocationGranularity;

			// Token: 0x040002CC RID: 716
			public uint dwProcessorLevel;

			// Token: 0x040002CD RID: 717
			public uint dwProcessorRevision;
		}

		// Token: 0x02000087 RID: 135
		[StructLayout(LayoutKind.Explicit)]
		public struct _PROCESSOR_INFO_UNION
		{
			// Token: 0x040002CE RID: 718
			[FieldOffset(0)]
			internal uint dwOemId;

			// Token: 0x040002CF RID: 719
			[FieldOffset(0)]
			internal ushort wProcessorArchitecture;

			// Token: 0x040002D0 RID: 720
			[FieldOffset(2)]
			internal ushort wReserved;
		}

		// Token: 0x02000088 RID: 136
		[StructLayout(LayoutKind.Sequential)]
		public class OSVersionInfoEx
		{
			// Token: 0x040002D1 RID: 721
			public uint dwOSVersionInfoSize;

			// Token: 0x040002D2 RID: 722
			public uint dwMajorVersion;

			// Token: 0x040002D3 RID: 723
			public uint dwMinorVersion;

			// Token: 0x040002D4 RID: 724
			public uint dwBuildNumber;

			// Token: 0x040002D5 RID: 725
			public uint dwPlatformId;

			// Token: 0x040002D6 RID: 726
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
			public string szCSDVersion;

			// Token: 0x040002D7 RID: 727
			public ushort wServicePackMajor;

			// Token: 0x040002D8 RID: 728
			public ushort wServicePackMinor;

			// Token: 0x040002D9 RID: 729
			public ushort wSuiteMask;

			// Token: 0x040002DA RID: 730
			public byte bProductType;

			// Token: 0x040002DB RID: 731
			public byte bReserved;
		}

		// Token: 0x02000089 RID: 137
		[Flags]
		internal enum GenericAccess : uint
		{
			// Token: 0x040002DD RID: 733
			GENERIC_READ = 2147483648U,
			// Token: 0x040002DE RID: 734
			GENERIC_WRITE = 1073741824U,
			// Token: 0x040002DF RID: 735
			GENERIC_EXECUTE = 536870912U,
			// Token: 0x040002E0 RID: 736
			GENERIC_ALL = 268435456U
		}

		// Token: 0x0200008A RID: 138
		internal enum CreationDisposition : uint
		{
			// Token: 0x040002E2 RID: 738
			CREATE_NEW = 1U,
			// Token: 0x040002E3 RID: 739
			CREATE_ALWAYS,
			// Token: 0x040002E4 RID: 740
			OPEN_EXISTING,
			// Token: 0x040002E5 RID: 741
			OPEN_ALWAYS,
			// Token: 0x040002E6 RID: 742
			TRUNCATE_EXISTING
		}

		// Token: 0x0200008B RID: 139
		[Flags]
		internal enum ShareMode : uint
		{
			// Token: 0x040002E8 RID: 744
			FILE_SHARE_NONE = 0U,
			// Token: 0x040002E9 RID: 745
			FILE_SHARE_READ = 1U,
			// Token: 0x040002EA RID: 746
			FILE_SHARE_WRITE = 2U,
			// Token: 0x040002EB RID: 747
			FILE_SHARE_DELETE = 4U
		}

		// Token: 0x0200008C RID: 140
		[Flags]
		internal enum FlagsAndAttributes : uint
		{
			// Token: 0x040002ED RID: 749
			FILE_FLAG_WRITE_THROUGH = 2147483648U,
			// Token: 0x040002EE RID: 750
			FILE_FLAG_OVERLAPPED = 1073741824U,
			// Token: 0x040002EF RID: 751
			FILE_FLAG_NO_BUFFERING = 536870912U,
			// Token: 0x040002F0 RID: 752
			FILE_FLAG_RANDOM_ACCESS = 268435456U,
			// Token: 0x040002F1 RID: 753
			FILE_FLAG_SEQUENTIAL_SCAN = 134217728U,
			// Token: 0x040002F2 RID: 754
			FILE_FLAG_DELETE_ON_CLOSE = 67108864U,
			// Token: 0x040002F3 RID: 755
			FILE_FLAG_BACKUP_SEMANTICS = 33554432U,
			// Token: 0x040002F4 RID: 756
			FILE_FLAG_POSIX_SEMANTICS = 16777216U,
			// Token: 0x040002F5 RID: 757
			FILE_FLAG_OPEN_REPARSE_POINT = 2097152U,
			// Token: 0x040002F6 RID: 758
			FILE_FLAG_OPEN_NO_RECALL = 1048576U,
			// Token: 0x040002F7 RID: 759
			FILE_FLAG_FIRST_PIPE_INSTANCE = 524288U,
			// Token: 0x040002F8 RID: 760
			FILE_ATTRIBUTE_READONLY = 1U,
			// Token: 0x040002F9 RID: 761
			FILE_ATTRIBUTE_HIDDEN = 2U,
			// Token: 0x040002FA RID: 762
			FILE_ATTRIBUTE_SYSTEM = 4U,
			// Token: 0x040002FB RID: 763
			FILE_ATTRIBUTE_DIRECTORY = 16U,
			// Token: 0x040002FC RID: 764
			FILE_ATTRIBUTE_ARCHIVE = 32U,
			// Token: 0x040002FD RID: 765
			FILE_ATTRIBUTE_DEVICE = 64U,
			// Token: 0x040002FE RID: 766
			FILE_ATTRIBUTE_NORMAL = 128U,
			// Token: 0x040002FF RID: 767
			FILE_ATTRIBUTE_TEMPORARY = 256U,
			// Token: 0x04000300 RID: 768
			FILE_ATTRIBUTE_SPARSE_FILE = 512U,
			// Token: 0x04000301 RID: 769
			FILE_ATTRIBUTE_REPARSE_POINT = 1024U,
			// Token: 0x04000302 RID: 770
			FILE_ATTRIBUTE_COMPRESSED = 2048U,
			// Token: 0x04000303 RID: 771
			FILE_ATTRIBUTE_OFFLINE = 4096U,
			// Token: 0x04000304 RID: 772
			FILE_ATTRIBUTE_NOT_CONTENT_INDEXED = 8192U,
			// Token: 0x04000305 RID: 773
			FILE_ATTRIBUTE_ENCRYPTED = 16384U
		}

		// Token: 0x0200008D RID: 141
		internal enum Win32Error
		{
			// Token: 0x04000307 RID: 775
			ERROR_SUCCESS,
			// Token: 0x04000308 RID: 776
			ERROR_INVALID_FUNCTION,
			// Token: 0x04000309 RID: 777
			ERROR_FILE_NOT_FOUND,
			// Token: 0x0400030A RID: 778
			ERROR_PATH_NOT_FOUND,
			// Token: 0x0400030B RID: 779
			ERROR_TOO_MANY_OPEN_FILES,
			// Token: 0x0400030C RID: 780
			ERROR_ACCESS_DENIED,
			// Token: 0x0400030D RID: 781
			ERROR_INVALID_HANDLE,
			// Token: 0x0400030E RID: 782
			ERROR_NO_MORE_FILES = 18,
			// Token: 0x0400030F RID: 783
			ERROR_NOT_READY = 21,
			// Token: 0x04000310 RID: 784
			ERROR_SHARING_VIOLATION = 32,
			// Token: 0x04000311 RID: 785
			ERROR_FILE_EXISTS = 80,
			// Token: 0x04000312 RID: 786
			ERROR_INVALID_PARAMETER = 87,
			// Token: 0x04000313 RID: 787
			ERROR_CALL_NOT_IMPLEMENTED = 120,
			// Token: 0x04000314 RID: 788
			ERROR_ALREADY_EXISTS = 183,
			// Token: 0x04000315 RID: 789
			ERROR_FILENAME_EXCED_RANGE = 206
		}

		// Token: 0x0200008E RID: 142
		internal enum HResults
		{
			// Token: 0x04000317 RID: 791
			HRESULT_ERROR_REVISION_MISMATCH = -2147023590
		}

		// Token: 0x0200008F RID: 143
		[SuppressUnmanagedCodeSecurity]
		[StructLayout(LayoutKind.Sequential)]
		internal class PROCESS_INFORMATION
		{
			// Token: 0x06000433 RID: 1075 RVA: 0x00015D84 File Offset: 0x00014D84
			~PROCESS_INFORMATION()
			{
				this.Close();
			}

			// Token: 0x06000434 RID: 1076 RVA: 0x00015DB0 File Offset: 0x00014DB0
			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			internal void Close()
			{
				if (this.hProcess != IntPtr.Zero && this.hProcess != NativeMethods.INVALID_HANDLE_VALUE)
				{
					NativeMethods.CloseHandle(new HandleRef(this, this.hProcess));
					this.hProcess = NativeMethods.INVALID_HANDLE_VALUE;
				}
				if (this.hThread != IntPtr.Zero && this.hThread != NativeMethods.INVALID_HANDLE_VALUE)
				{
					NativeMethods.CloseHandle(new HandleRef(this, this.hThread));
					this.hThread = NativeMethods.INVALID_HANDLE_VALUE;
				}
			}

			// Token: 0x04000318 RID: 792
			public IntPtr hProcess = IntPtr.Zero;

			// Token: 0x04000319 RID: 793
			public IntPtr hThread = IntPtr.Zero;

			// Token: 0x0400031A RID: 794
			public int dwProcessId;

			// Token: 0x0400031B RID: 795
			public int dwThreadId;
		}

		// Token: 0x02000090 RID: 144
		internal struct AssemblyInfoInternal
		{
			// Token: 0x0400031C RID: 796
			internal const int MaxPath = 1024;

			// Token: 0x0400031D RID: 797
			internal int cbAssemblyInfo;

			// Token: 0x0400031E RID: 798
			internal int assemblyFlags;

			// Token: 0x0400031F RID: 799
			internal long assemblySizeInKB;

			// Token: 0x04000320 RID: 800
			internal IntPtr currentAssemblyPathBuf;

			// Token: 0x04000321 RID: 801
			internal int cchBuf;
		}

		// Token: 0x02000091 RID: 145
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("e707dcde-d1cd-11d2-bab9-00c04f8eceae")]
		[ComImport]
		internal interface IAssemblyCache
		{
			// Token: 0x06000436 RID: 1078
			void UninstallAssembly();

			// Token: 0x06000437 RID: 1079
			void QueryAssemblyInfo(int flags, [MarshalAs(UnmanagedType.LPWStr)] string assemblyName, ref NativeMethods.AssemblyInfoInternal assemblyInfo);

			// Token: 0x06000438 RID: 1080
			void CreateAssemblyCacheItem();

			// Token: 0x06000439 RID: 1081
			void CreateAssemblyScavenger();

			// Token: 0x0600043A RID: 1082
			void InstallAssembly();
		}

		// Token: 0x02000092 RID: 146
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("21b8916c-f28e-11d2-a473-00c04f8ef448")]
		[ComImport]
		internal interface IAssemblyEnum
		{
			// Token: 0x0600043B RID: 1083
			[PreserveSig]
			int GetNextAssembly(NativeMethods.IApplicationContext ppAppCtx, out NativeMethods.IAssemblyName ppName, uint dwFlags);

			// Token: 0x0600043C RID: 1084
			[PreserveSig]
			int Reset();

			// Token: 0x0600043D RID: 1085
			[PreserveSig]
			int Clone(out NativeMethods.IAssemblyEnum ppEnum);
		}

		// Token: 0x02000093 RID: 147
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("7c23ff90-33af-11d3-95da-00a024a85b51")]
		[ComImport]
		internal interface IApplicationContext
		{
			// Token: 0x0600043E RID: 1086
			void SetContextNameObject(NativeMethods.IAssemblyName pName);

			// Token: 0x0600043F RID: 1087
			void GetContextNameObject(out NativeMethods.IAssemblyName ppName);

			// Token: 0x06000440 RID: 1088
			void Set([MarshalAs(UnmanagedType.LPWStr)] string szName, int pvValue, uint cbValue, uint dwFlags);

			// Token: 0x06000441 RID: 1089
			void Get([MarshalAs(UnmanagedType.LPWStr)] string szName, out int pvValue, ref uint pcbValue, uint dwFlags);

			// Token: 0x06000442 RID: 1090
			void GetDynamicDirectory(out int wzDynamicDir, ref uint pdwSize);
		}

		// Token: 0x02000094 RID: 148
		[Guid("CD193BC0-B4BC-11d2-9833-00C04FC31D2E")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		internal interface IAssemblyName
		{
			// Token: 0x06000443 RID: 1091
			[PreserveSig]
			int SetProperty(uint PropertyId, IntPtr pvProperty, uint cbProperty);

			// Token: 0x06000444 RID: 1092
			[PreserveSig]
			int GetProperty(uint PropertyId, IntPtr pvProperty, ref uint pcbProperty);

			// Token: 0x06000445 RID: 1093
			[PreserveSig]
			int Finalize();

			// Token: 0x06000446 RID: 1094
			[PreserveSig]
			int GetDisplayName(IntPtr szDisplayName, ref uint pccDisplayName, uint dwDisplayFlags);

			// Token: 0x06000447 RID: 1095
			[PreserveSig]
			int BindToObject(object refIID, object pAsmBindSink, NativeMethods.IApplicationContext pApplicationContext, [MarshalAs(UnmanagedType.LPWStr)] string szCodeBase, long llFlags, int pvReserved, uint cbReserved, out int ppv);

			// Token: 0x06000448 RID: 1096
			[PreserveSig]
			int GetName(out uint lpcwBuffer, out int pwzName);

			// Token: 0x06000449 RID: 1097
			[PreserveSig]
			int GetVersion(out uint pdwVersionHi, out uint pdwVersionLow);

			// Token: 0x0600044A RID: 1098
			[PreserveSig]
			int IsEqual(NativeMethods.IAssemblyName pName, uint dwCmpFlags);

			// Token: 0x0600044B RID: 1099
			[PreserveSig]
			int Clone(out NativeMethods.IAssemblyName pName);
		}

		// Token: 0x02000095 RID: 149
		internal enum ASM_CACHE : uint
		{
			// Token: 0x04000323 RID: 803
			ZAP = 1U,
			// Token: 0x04000324 RID: 804
			GAC,
			// Token: 0x04000325 RID: 805
			DOWNLOAD = 4U
		}

		// Token: 0x02000096 RID: 150
		internal enum CreateAssemblyNameObjectFlags : uint
		{
			// Token: 0x04000327 RID: 807
			CANOF_DEFAULT,
			// Token: 0x04000328 RID: 808
			CANOF_PARSE_DISPLAY_NAME
		}

		// Token: 0x02000097 RID: 151
		[StructLayout(LayoutKind.Sequential)]
		internal class ACTCTXW
		{
			// Token: 0x0600044C RID: 1100 RVA: 0x00015E5D File Offset: 0x00014E5D
			public ACTCTXW(string manifestPath)
			{
				this.cbSize = (uint)Marshal.SizeOf(typeof(NativeMethods.ACTCTXW));
				this.dwFlags = 0U;
				this.lpSource = manifestPath;
			}

			// Token: 0x04000329 RID: 809
			public uint cbSize;

			// Token: 0x0400032A RID: 810
			public uint dwFlags;

			// Token: 0x0400032B RID: 811
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpSource;

			// Token: 0x0400032C RID: 812
			public ushort wProcessorArchitecture;

			// Token: 0x0400032D RID: 813
			public ushort wLangId;

			// Token: 0x0400032E RID: 814
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpAssemblyDirectory;

			// Token: 0x0400032F RID: 815
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpResourceName;

			// Token: 0x04000330 RID: 816
			[MarshalAs(UnmanagedType.LPWStr)]
			public string lpApplicationName;

			// Token: 0x04000331 RID: 817
			public IntPtr hModule;
		}

		// Token: 0x02000098 RID: 152
		public enum CacheEntryFlags : uint
		{
			// Token: 0x04000333 RID: 819
			Normal = 1U,
			// Token: 0x04000334 RID: 820
			Sticky = 4U,
			// Token: 0x04000335 RID: 821
			Edited = 8U,
			// Token: 0x04000336 RID: 822
			TrackOffline = 16U,
			// Token: 0x04000337 RID: 823
			TrackOnline = 32U,
			// Token: 0x04000338 RID: 824
			Sparse = 65536U,
			// Token: 0x04000339 RID: 825
			Cookie = 1048576U,
			// Token: 0x0400033A RID: 826
			UrlHistory = 2097152U
		}

		// Token: 0x02000099 RID: 153
		public enum SHChangeNotifyEventID
		{
			// Token: 0x0400033C RID: 828
			SHCNE_ASSOCCHANGED = 134217728
		}

		// Token: 0x0200009A RID: 154
		public enum SHChangeNotifyFlags : uint
		{
			// Token: 0x0400033E RID: 830
			SHCNF_IDLIST
		}

		// Token: 0x0200009B RID: 155
		internal enum SIGDN : uint
		{
			// Token: 0x04000340 RID: 832
			NORMALDISPLAY,
			// Token: 0x04000341 RID: 833
			PARENTRELATIVEPARSING = 2147581953U,
			// Token: 0x04000342 RID: 834
			DESKTOPABSOLUTEPARSING = 2147647488U,
			// Token: 0x04000343 RID: 835
			PARENTRELATIVEEDITING = 2147684353U,
			// Token: 0x04000344 RID: 836
			DESKTOPABSOLUTEEDITING = 2147794944U,
			// Token: 0x04000345 RID: 837
			FILESYSPATH = 2147844096U,
			// Token: 0x04000346 RID: 838
			URL = 2147909632U,
			// Token: 0x04000347 RID: 839
			PARENTRELATIVEFORADDRESSBAR = 2147991553U,
			// Token: 0x04000348 RID: 840
			PARENTRELATIVE = 2148007937U
		}

		// Token: 0x0200009C RID: 156
		[Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IShellItem
		{
			// Token: 0x0600044D RID: 1101
			void BindToHandler(IntPtr pbc, [MarshalAs(UnmanagedType.LPStruct)] Guid bhid, [MarshalAs(UnmanagedType.LPStruct)] Guid riid, out IntPtr ppv);

			// Token: 0x0600044E RID: 1102
			void GetParent(out NativeMethods.IShellItem ppsi);

			// Token: 0x0600044F RID: 1103
			void GetDisplayName(NativeMethods.SIGDN sigdnName, out IntPtr ppszName);

			// Token: 0x06000450 RID: 1104
			void GetAttributes(uint sfgaoMask, out uint psfgaoAttribs);

			// Token: 0x06000451 RID: 1105
			void Compare(NativeMethods.IShellItem psi, uint hint, out int piOrder);
		}

		// Token: 0x0200009D RID: 157
		[Guid("4CD19ADA-25A5-4A32-B3B7-347BEE5BE36B")]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[ComImport]
		public interface IStartMenuPinnedList
		{
			// Token: 0x06000452 RID: 1106
			void RemoveFromList(NativeMethods.IShellItem psi);
		}
	}
}
