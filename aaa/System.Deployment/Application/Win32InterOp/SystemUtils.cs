using System;
using System.ComponentModel;
using System.Deployment.Internal.Isolation;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Deployment.Application.Win32InterOp
{
	// Token: 0x020000EB RID: 235
	internal class SystemUtils
	{
		// Token: 0x060005F4 RID: 1524 RVA: 0x0001EBB4 File Offset: 0x0001DBB4
		public static byte[] GetManifestFromPEResources(string filePath)
		{
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			IntPtr intPtr3 = IntPtr.Zero;
			IntPtr intPtr4 = IntPtr.Zero;
			IntPtr intPtr5 = new IntPtr(0);
			uint num = 2U;
			byte[] array = null;
			try
			{
				intPtr2 = NativeMethods.LoadLibraryEx(filePath, intPtr5, num);
				int num2 = Marshal.GetLastWin32Error();
				if (intPtr2 == IntPtr.Zero)
				{
					SystemUtils.Win32LoadExceptionHelper(num2, "Ex_Win32LoadException", filePath);
				}
				intPtr = NativeMethods.FindResource(intPtr2, "#1", "#24");
				if (intPtr != IntPtr.Zero)
				{
					uint num3 = NativeMethods.SizeofResource(intPtr2, intPtr);
					num2 = Marshal.GetLastWin32Error();
					if (num3 == 0U)
					{
						SystemUtils.Win32LoadExceptionHelper(num2, "Ex_Win32ResourceLoadException", filePath);
					}
					intPtr3 = NativeMethods.LoadResource(intPtr2, intPtr);
					num2 = Marshal.GetLastWin32Error();
					if (intPtr3 == IntPtr.Zero)
					{
						SystemUtils.Win32LoadExceptionHelper(num2, "Ex_Win32ResourceLoadException", filePath);
					}
					intPtr4 = NativeMethods.LockResource(intPtr3);
					if (intPtr4 == IntPtr.Zero)
					{
						throw new Win32Exception(33);
					}
					array = new byte[num3];
					Marshal.Copy(intPtr4, array, 0, (int)num3);
				}
			}
			finally
			{
				if (intPtr2 != IntPtr.Zero)
				{
					bool flag = NativeMethods.FreeLibrary(intPtr2);
					int num2 = Marshal.GetLastWin32Error();
					if (!flag)
					{
						throw new Win32Exception(num2);
					}
				}
			}
			return array;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0001ECEC File Offset: 0x0001DCEC
		private static void Win32LoadExceptionHelper(int win32ErrorCode, string resourceId, string filePath)
		{
			string fileName = Path.GetFileName(filePath);
			string text = string.Format(CultureInfo.CurrentUICulture, Resources.GetString(resourceId), new object[]
			{
				fileName,
				Convert.ToString(win32ErrorCode, 16)
			});
			throw new Win32Exception(win32ErrorCode, text);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0001ED30 File Offset: 0x0001DD30
		internal static SystemUtils.AssemblyInfo QueryAssemblyInfo(SystemUtils.QueryAssemblyInfoFlags flags, string assemblyName)
		{
			SystemUtils.AssemblyInfo assemblyInfo = new SystemUtils.AssemblyInfo();
			NativeMethods.AssemblyInfoInternal assemblyInfoInternal = default(NativeMethods.AssemblyInfoInternal);
			if ((flags & SystemUtils.QueryAssemblyInfoFlags.GetCurrentPath) != (SystemUtils.QueryAssemblyInfoFlags)0)
			{
				assemblyInfoInternal.cchBuf = 1024;
				assemblyInfoInternal.currentAssemblyPathBuf = Marshal.AllocHGlobal(assemblyInfoInternal.cchBuf * 2);
			}
			else
			{
				assemblyInfoInternal.cchBuf = 0;
				assemblyInfoInternal.currentAssemblyPathBuf = (IntPtr)0;
			}
			NativeMethods.IAssemblyCache assemblyCache = null;
			NativeMethods.CreateAssemblyCache(out assemblyCache, 0);
			try
			{
				assemblyCache.QueryAssemblyInfo((int)flags, assemblyName, ref assemblyInfoInternal);
			}
			catch (FileNotFoundException)
			{
				assemblyInfo = null;
			}
			if (assemblyInfo != null)
			{
				assemblyInfo.AssemblyInfoSizeInByte = assemblyInfoInternal.cbAssemblyInfo;
				assemblyInfo.AssemblyFlags = (SystemUtils.AssemblyInfoFlags)assemblyInfoInternal.assemblyFlags;
				assemblyInfo.AssemblySizeInKB = assemblyInfoInternal.assemblySizeInKB;
				if ((flags & SystemUtils.QueryAssemblyInfoFlags.GetCurrentPath) != (SystemUtils.QueryAssemblyInfoFlags)0)
				{
					assemblyInfo.CurrentAssemblyPath = Marshal.PtrToStringUni(assemblyInfoInternal.currentAssemblyPathBuf);
					Marshal.FreeHGlobal(assemblyInfoInternal.currentAssemblyPathBuf);
				}
			}
			return assemblyInfo;
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0001EE04 File Offset: 0x0001DE04
		internal static DefinitionIdentity GetDefinitionIdentityFromManagedAssembly(string filePath)
		{
			Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IReferenceIdentity));
			IReferenceIdentity referenceIdentity = (IReferenceIdentity)NativeMethods.GetAssemblyIdentityFromFile(filePath, ref guidOfType);
			ReferenceIdentity referenceIdentity2 = new ReferenceIdentity(referenceIdentity);
			string processorArchitecture = referenceIdentity2.ProcessorArchitecture;
			if (processorArchitecture != null)
			{
				referenceIdentity2.ProcessorArchitecture = processorArchitecture.ToLower(CultureInfo.InvariantCulture);
			}
			return new DefinitionIdentity(referenceIdentity2);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0001EE5C File Offset: 0x0001DE5C
		internal static void CheckSupportedImageAndCLRVersions(string path)
		{
			StringBuilder stringBuilder = new StringBuilder(24);
			uint num;
			try
			{
				NativeMethods.GetFileVersion(path, stringBuilder, (uint)stringBuilder.Capacity, out num);
			}
			catch (BadImageFormatException)
			{
				throw;
			}
			if (stringBuilder[0] != 'v')
			{
				throw new InvalidDeploymentException(ExceptionTypes.ClrValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidCLRVersionInFile"), new object[]
				{
					stringBuilder,
					Path.GetFileName(path)
				}));
			}
			Version version = new Version(stringBuilder.ToString(1, stringBuilder.Length - 1));
			if ((long)version.Major < 2L)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ClrValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_ImageVersionCLRNotSupported"), new object[]
				{
					version,
					Path.GetFileName(path)
				}));
			}
			uint num2 = 81U;
			uint num3;
			NativeMethods.GetRequestedRuntimeInfo(path, null, null, 0U, num2, null, 0U, out num3, stringBuilder, (uint)stringBuilder.Capacity, out num);
			if (stringBuilder[0] != 'v')
			{
				throw new FormatException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidCLRVersionInFile"), new object[]
				{
					stringBuilder,
					Path.GetFileName(path)
				}));
			}
			string text = stringBuilder.ToString(1, stringBuilder.Length - 1);
			int num4 = text.IndexOf(".", StringComparison.Ordinal);
			uint num5 = uint.Parse((num4 >= 0) ? text.Substring(0, num4) : text, CultureInfo.InvariantCulture);
			if (num5 < 2U)
			{
				throw new InvalidDeploymentException(ExceptionTypes.ClrValidation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_RuntimeVersionCLRNotSupported"), new object[]
				{
					text,
					Path.GetFileName(path)
				}));
			}
		}

		// Token: 0x040004BC RID: 1212
		private const int MAX_CLR_VERSION_LENGTH = 24;

		// Token: 0x020000EC RID: 236
		private enum RUNTIME_INFO_FLAGS : uint
		{
			// Token: 0x040004BE RID: 1214
			RUNTIME_INFO_UPGRADE_VERSION = 1U,
			// Token: 0x040004BF RID: 1215
			RUNTIME_INFO_REQUEST_IA64,
			// Token: 0x040004C0 RID: 1216
			RUNTIME_INFO_REQUEST_AMD64 = 4U,
			// Token: 0x040004C1 RID: 1217
			RUNTIME_INFO_REQUEST_X86 = 8U,
			// Token: 0x040004C2 RID: 1218
			RUNTIME_INFO_DONT_RETURN_DIRECTORY = 16U,
			// Token: 0x040004C3 RID: 1219
			RUNTIME_INFO_DONT_RETURN_VERSION = 32U,
			// Token: 0x040004C4 RID: 1220
			RUNTIME_INFO_DONT_SHOW_ERROR_DIALOG = 64U
		}

		// Token: 0x020000ED RID: 237
		internal enum AssemblyInfoFlags
		{
			// Token: 0x040004C6 RID: 1222
			Installed = 1,
			// Token: 0x040004C7 RID: 1223
			PayLoadResident
		}

		// Token: 0x020000EE RID: 238
		[Flags]
		internal enum QueryAssemblyInfoFlags
		{
			// Token: 0x040004C9 RID: 1225
			Validate = 1,
			// Token: 0x040004CA RID: 1226
			GetSize = 2,
			// Token: 0x040004CB RID: 1227
			GetCurrentPath = 4,
			// Token: 0x040004CC RID: 1228
			All = 7
		}

		// Token: 0x020000EF RID: 239
		internal class AssemblyInfo
		{
			// Token: 0x17000148 RID: 328
			// (set) Token: 0x060005FA RID: 1530 RVA: 0x0001F008 File Offset: 0x0001E008
			internal int AssemblyInfoSizeInByte
			{
				set
				{
					this.assemblyInfoSizeInByte = value;
				}
			}

			// Token: 0x17000149 RID: 329
			// (set) Token: 0x060005FB RID: 1531 RVA: 0x0001F011 File Offset: 0x0001E011
			internal SystemUtils.AssemblyInfoFlags AssemblyFlags
			{
				set
				{
					this.assemblyFlags = value;
				}
			}

			// Token: 0x1700014A RID: 330
			// (set) Token: 0x060005FC RID: 1532 RVA: 0x0001F01A File Offset: 0x0001E01A
			internal long AssemblySizeInKB
			{
				set
				{
					this.assemblySizeInKB = value;
				}
			}

			// Token: 0x1700014B RID: 331
			// (set) Token: 0x060005FD RID: 1533 RVA: 0x0001F023 File Offset: 0x0001E023
			internal string CurrentAssemblyPath
			{
				set
				{
					this.currentAssemblyPath = value;
				}
			}

			// Token: 0x040004CD RID: 1229
			private int assemblyInfoSizeInByte;

			// Token: 0x040004CE RID: 1230
			private SystemUtils.AssemblyInfoFlags assemblyFlags;

			// Token: 0x040004CF RID: 1231
			private long assemblySizeInKB;

			// Token: 0x040004D0 RID: 1232
			private string currentAssemblyPath;
		}
	}
}
