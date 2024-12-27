using System;
using System.ComponentModel;
using System.Deployment.Application.Manifest;
using System.Deployment.Application.Win32InterOp;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace System.Deployment.Application
{
	// Token: 0x020000BD RID: 189
	internal static class PlatformDetector
	{
		// Token: 0x060004B1 RID: 1201 RVA: 0x00017840 File Offset: 0x00016840
		public static bool VerifyCLRVersionInfo(Version v, string procArch)
		{
			bool flag = true;
			PlatformDetector.NameMap[] array = new PlatformDetector.NameMap[]
			{
				new PlatformDetector.NameMap("x86", 8U),
				new PlatformDetector.Product("ia64", 2U),
				new PlatformDetector.Product("amd64", 4U)
			};
			uint num = PlatformDetector.NameMap.MapNameToMask(procArch, array);
			num |= 65U;
			StringBuilder stringBuilder = new StringBuilder(260);
			StringBuilder stringBuilder2 = new StringBuilder("v65535.65535.65535".Length);
			uint num2 = 0U;
			uint num3 = 0U;
			string text = v.ToString(3);
			text = "v" + text;
			try
			{
				NativeMethods.GetRequestedRuntimeInfo(null, text, null, 0U, num, stringBuilder, (uint)stringBuilder.Capacity, out num2, stringBuilder2, (uint)stringBuilder2.Capacity, out num3);
			}
			catch (COMException ex)
			{
				flag = false;
				if (ex.ErrorCode != -2146232576)
				{
					throw;
				}
			}
			return flag;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00017918 File Offset: 0x00016918
		public static bool IsCLRDependencyText(string clrTextName)
		{
			return string.Compare(clrTextName, "Microsoft-Windows-CLRCoreComp", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(clrTextName, "Microsoft.Windows.CommonLanguageRuntime", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x0001793C File Offset: 0x0001693C
		public static bool IsSupportedProcessorArchitecture(string arch)
		{
			if (string.Compare(arch, "msil", StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(arch, "x86", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			NativeMethods.SYSTEM_INFO system_INFO = default(NativeMethods.SYSTEM_INFO);
			bool flag = false;
			try
			{
				NativeMethods.GetNativeSystemInfo(ref system_INFO);
				flag = true;
			}
			catch (EntryPointNotFoundException)
			{
				flag = false;
			}
			if (!flag)
			{
				NativeMethods.GetSystemInfo(ref system_INFO);
			}
			ushort wProcessorArchitecture = system_INFO.uProcessorInfo.wProcessorArchitecture;
			if (wProcessorArchitecture != 6)
			{
				return wProcessorArchitecture == 9 && string.Compare(arch, "amd64", StringComparison.OrdinalIgnoreCase) == 0;
			}
			return string.Compare(arch, "ia64", StringComparison.OrdinalIgnoreCase) == 0;
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x000179D4 File Offset: 0x000169D4
		public static bool VerifyOSDependency(ref PlatformDetector.OSDependency osd)
		{
			OperatingSystem osversion = Environment.OSVersion;
			if ((long)osversion.Version.Major == 4L)
			{
				return (long)osversion.Version.Major >= (long)((ulong)osd.dwMajorVersion);
			}
			NativeMethods.OSVersionInfoEx osversionInfoEx = new NativeMethods.OSVersionInfoEx();
			osversionInfoEx.dwOSVersionInfoSize = (uint)Marshal.SizeOf(osversionInfoEx);
			osversionInfoEx.dwMajorVersion = osd.dwMajorVersion;
			osversionInfoEx.dwMinorVersion = osd.dwMinorVersion;
			osversionInfoEx.dwBuildNumber = osd.dwBuildNumber;
			osversionInfoEx.dwPlatformId = 0U;
			osversionInfoEx.szCSDVersion = null;
			osversionInfoEx.wServicePackMajor = osd.wServicePackMajor;
			osversionInfoEx.wServicePackMinor = osd.wServicePackMinor;
			osversionInfoEx.wSuiteMask = (ushort)((osd.suiteName != null) ? PlatformDetector.NameMap.MapNameToMask(osd.suiteName, PlatformDetector.Suites) : 0U);
			osversionInfoEx.bProductType = (byte)((osd.productName != null) ? PlatformDetector.NameMap.MapNameToMask(osd.productName, PlatformDetector.Products) : 0U);
			osversionInfoEx.bReserved = 0;
			ulong num = 0UL;
			uint num2 = 2U | ((osd.dwMinorVersion != 0U) ? 1U : 0U) | ((osd.dwBuildNumber != 0U) ? 4U : 0U) | ((osd.suiteName != null) ? 64U : 0U) | ((osd.productName != null) ? 128U : 0U) | ((osd.wServicePackMajor != 0) ? 32U : 0U) | ((osd.wServicePackMinor != 0) ? 16U : 0U);
			num = NativeMethods.VerSetConditionMask(num, 2U, 3);
			if (osd.dwMinorVersion != 0U)
			{
				num = NativeMethods.VerSetConditionMask(num, 1U, 3);
			}
			if (osd.dwBuildNumber != 0U)
			{
				num = NativeMethods.VerSetConditionMask(num, 4U, 3);
			}
			if (osd.suiteName != null)
			{
				num = NativeMethods.VerSetConditionMask(num, 64U, 6);
			}
			if (osd.productName != null)
			{
				num = NativeMethods.VerSetConditionMask(num, 128U, 1);
			}
			if (osd.wServicePackMajor != 0)
			{
				num = NativeMethods.VerSetConditionMask(num, 32U, 3);
			}
			if (osd.wServicePackMinor != 0)
			{
				num = NativeMethods.VerSetConditionMask(num, 16U, 3);
			}
			bool flag = NativeMethods.VerifyVersionInfo(osversionInfoEx, num2, num);
			if (!flag)
			{
				int lastWin32Error = Marshal.GetLastWin32Error();
				if (lastWin32Error != 1150)
				{
					throw new Win32Exception(lastWin32Error);
				}
			}
			return flag;
		}

		// Token: 0x060004B5 RID: 1205 RVA: 0x00017BC5 File Offset: 0x00016BC5
		public static bool VerifyGACDependency(ReferenceIdentity refId, string tempDir)
		{
			if (string.Compare(refId.ProcessorArchitecture, "msil", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return PlatformDetector.VerifyGACDependencyWhidbey(refId);
			}
			return PlatformDetector.VerifyGACDependencyXP(refId, tempDir) || PlatformDetector.VerifyGACDependencyWhidbey(refId);
		}

		// Token: 0x060004B6 RID: 1206 RVA: 0x00017BF4 File Offset: 0x00016BF4
		public static bool VerifyGACDependencyWhidbey(ReferenceIdentity refId)
		{
			string text = refId.ToString();
			string text2 = null;
			try
			{
				text2 = AppDomain.CurrentDomain.ApplyPolicy(text);
			}
			catch (ArgumentException)
			{
				return false;
			}
			catch (COMException)
			{
				return false;
			}
			ReferenceIdentity referenceIdentity = new ReferenceIdentity(text2);
			referenceIdentity.ProcessorArchitecture = refId.ProcessorArchitecture;
			string text3 = referenceIdentity.ToString();
			SystemUtils.AssemblyInfo assemblyInfo = SystemUtils.QueryAssemblyInfo(SystemUtils.QueryAssemblyInfoFlags.All, text3);
			if (assemblyInfo == null && referenceIdentity.ProcessorArchitecture == null)
			{
				NativeMethods.IAssemblyName assemblyName;
				NativeMethods.CreateAssemblyNameObject(out assemblyName, referenceIdentity.ToString(), 1U, IntPtr.Zero);
				NativeMethods.IAssemblyEnum assemblyEnum;
				NativeMethods.CreateAssemblyEnum(out assemblyEnum, null, assemblyName, 2U, IntPtr.Zero);
				return assemblyEnum.GetNextAssembly(null, out assemblyName, 0U) == 0;
			}
			return assemblyInfo != null;
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00017CB4 File Offset: 0x00016CB4
		public static bool VerifyGACDependencyXP(ReferenceIdentity refId, string tempDir)
		{
			if (!PlatformSpecific.OnXPOrAbove)
			{
				return false;
			}
			bool flag;
			using (TempFile tempFile = new TempFile(tempDir, ".manifest"))
			{
				ManifestGenerator.GenerateGACDetectionManifest(refId, tempFile.Path);
				NativeMethods.ACTCTXW actctxw = new NativeMethods.ACTCTXW(tempFile.Path);
				IntPtr intPtr = NativeMethods.CreateActCtxW(actctxw);
				if (intPtr != NativeMethods.INVALID_HANDLE_VALUE)
				{
					NativeMethods.ReleaseActCtx(intPtr);
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			return flag;
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00017D2C File Offset: 0x00016D2C
		public static void VerifyPlatformDependencies(AssemblyManifest appManifest, Uri deploySupportUri, string tempDir)
		{
			Uri uri = deploySupportUri;
			DependentOS dependentOS = appManifest.DependentOS;
			if (dependentOS != null)
			{
				PlatformDetector.OSDependency osdependency = new PlatformDetector.OSDependency((uint)dependentOS.MajorVersion, (uint)dependentOS.MinorVersion, (uint)dependentOS.BuildNumber, (ushort)dependentOS.ServicePackMajor, (ushort)dependentOS.ServicePackMinor, null, null);
				if (!PlatformDetector.VerifyOSDependency(ref osdependency))
				{
					StringBuilder stringBuilder = new StringBuilder();
					string text = string.Concat(new object[] { dependentOS.MajorVersion, ".", dependentOS.MinorVersion, ".", dependentOS.BuildNumber, ".", dependentOS.ServicePackMajor, dependentOS.ServicePackMinor });
					stringBuilder.AppendFormat(Resources.GetString("PlatformMicrosoftWindowsOperatingSystem"), text);
					string text2 = stringBuilder.ToString();
					if (dependentOS.SupportUrl != null)
					{
						uri = dependentOS.SupportUrl;
					}
					throw new DependentPlatformMissingException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("ErrorMessage_PlatformDetectionFailed"), new object[] { text2 }), uri);
				}
			}
			bool flag = false;
			bool flag2 = false;
			foreach (DependentAssembly dependentAssembly in appManifest.DependentAssemblies)
			{
				if (dependentAssembly.IsPreRequisite && PlatformDetector.IsCLRDependencyText(dependentAssembly.Identity.Name))
				{
					Version version = dependentAssembly.Identity.Version;
					string processorArchitecture = dependentAssembly.Identity.ProcessorArchitecture;
					if (!PlatformDetector.VerifyCLRVersionInfo(version, processorArchitecture))
					{
						StringBuilder stringBuilder2 = new StringBuilder();
						stringBuilder2.AppendFormat(Resources.GetString("PlatformMicrosoftCommonLanguageRuntime"), version.ToString());
						string text2 = stringBuilder2.ToString();
						if (dependentAssembly.SupportUrl != null)
						{
							uri = dependentAssembly.SupportUrl;
						}
						throw new DependentPlatformMissingException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("ErrorMessage_PlatformDetectionFailed"), new object[] { text2 }), uri);
					}
				}
				if (dependentAssembly.IsPreRequisite && PlatformDetector.IsNetFX35SP1ClientSignatureAsm(dependentAssembly.Identity))
				{
					flag = true;
				}
				if (dependentAssembly.IsPreRequisite && PlatformDetector.IsNetFX35SP1FullSignatureAsm(dependentAssembly.Identity))
				{
					flag2 = true;
				}
			}
			if (!PolicyKeys.SkipSKUDetection())
			{
				PlatformDetector.NetFX35SP1SKU platformNetFx35SKU = PlatformDetector.GetPlatformNetFx35SKU(tempDir);
				if (platformNetFx35SKU == PlatformDetector.NetFX35SP1SKU.Client35SP1 && !flag && !flag2)
				{
					string text2 = ".NET Framework 3.5 SP1";
					throw new DependentPlatformMissingException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("ErrorMessage_PlatformDetectionFailed"), new object[] { text2 }));
				}
			}
			foreach (DependentAssembly dependentAssembly2 in appManifest.DependentAssemblies)
			{
				if (dependentAssembly2.IsPreRequisite && !PlatformDetector.IsCLRDependencyText(dependentAssembly2.Identity.Name) && !PlatformDetector.VerifyGACDependency(dependentAssembly2.Identity, tempDir))
				{
					string text2;
					if (dependentAssembly2.Description != null)
					{
						text2 = dependentAssembly2.Description;
					}
					else
					{
						ReferenceIdentity identity = dependentAssembly2.Identity;
						StringBuilder stringBuilder3 = new StringBuilder();
						stringBuilder3.AppendFormat(Resources.GetString("PlatformDependentAssemblyVersion"), identity.Name, identity.Version);
						text2 = stringBuilder3.ToString();
					}
					if (dependentAssembly2.SupportUrl != null)
					{
						uri = dependentAssembly2.SupportUrl;
					}
					throw new DependentPlatformMissingException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("ErrorMessage_PlatformGACDetectionFailed"), new object[] { text2 }), uri);
				}
			}
		}

		// Token: 0x060004B9 RID: 1209 RVA: 0x000180A4 File Offset: 0x000170A4
		private static bool IsNetFX35SP1ClientSignatureAsm(ReferenceIdentity ra)
		{
			DefinitionIdentity definitionIdentity = new DefinitionIdentity("Sentinel.v3.5Client, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a,processorArchitecture=msil");
			return definitionIdentity.Matches(ra, true);
		}

		// Token: 0x060004BA RID: 1210 RVA: 0x000180CC File Offset: 0x000170CC
		private static bool IsNetFX35SP1FullSignatureAsm(ReferenceIdentity ra)
		{
			DefinitionIdentity definitionIdentity = new DefinitionIdentity("System.Data.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089,processorArchitecture=msil");
			return definitionIdentity.Matches(ra, true);
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x000180F4 File Offset: 0x000170F4
		private static PlatformDetector.NetFX35SP1SKU GetPlatformNetFx35SKU(string tempDir)
		{
			ReferenceIdentity referenceIdentity = new ReferenceIdentity("Sentinel.v3.5Client, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a,processorArchitecture=msil");
			ReferenceIdentity referenceIdentity2 = new ReferenceIdentity("System.Data.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089,processorArchitecture=msil");
			bool flag = false;
			bool flag2 = false;
			if (PlatformDetector.VerifyGACDependency(referenceIdentity, tempDir))
			{
				flag = true;
			}
			if (PlatformDetector.VerifyGACDependency(referenceIdentity2, tempDir))
			{
				flag2 = true;
			}
			if (flag && !flag2)
			{
				return PlatformDetector.NetFX35SP1SKU.Client35SP1;
			}
			if (flag && flag2)
			{
				return PlatformDetector.NetFX35SP1SKU.Full35SP1;
			}
			return PlatformDetector.NetFX35SP1SKU.No35SP1;
		}

		// Token: 0x040003FD RID: 1021
		private const int MAX_PATH = 260;

		// Token: 0x040003FE RID: 1022
		private const byte VER_EQUAL = 1;

		// Token: 0x040003FF RID: 1023
		private const byte VER_GREATER = 2;

		// Token: 0x04000400 RID: 1024
		private const byte VER_GREATER_EQUAL = 3;

		// Token: 0x04000401 RID: 1025
		private const byte VER_LESS = 4;

		// Token: 0x04000402 RID: 1026
		private const byte VER_LESS_EQUAL = 5;

		// Token: 0x04000403 RID: 1027
		private const byte VER_AND = 6;

		// Token: 0x04000404 RID: 1028
		private const byte VER_OR = 7;

		// Token: 0x04000405 RID: 1029
		private const uint VER_MINORVERSION = 1U;

		// Token: 0x04000406 RID: 1030
		private const uint VER_MAJORVERSION = 2U;

		// Token: 0x04000407 RID: 1031
		private const uint VER_BUILDNUMBER = 4U;

		// Token: 0x04000408 RID: 1032
		private const uint VER_PLATFORMID = 8U;

		// Token: 0x04000409 RID: 1033
		private const uint VER_SERVICEPACKMINOR = 16U;

		// Token: 0x0400040A RID: 1034
		private const uint VER_SERVICEPACKMAJOR = 32U;

		// Token: 0x0400040B RID: 1035
		private const uint VER_SUITENAME = 64U;

		// Token: 0x0400040C RID: 1036
		private const uint VER_PRODUCT_TYPE = 128U;

		// Token: 0x0400040D RID: 1037
		private const uint VER_SERVER_NT = 2147483648U;

		// Token: 0x0400040E RID: 1038
		private const uint VER_WORKSTATION_NT = 1073741824U;

		// Token: 0x0400040F RID: 1039
		private const uint VER_SUITE_SMALLBUSINESS = 1U;

		// Token: 0x04000410 RID: 1040
		private const uint VER_SUITE_ENTERPRISE = 2U;

		// Token: 0x04000411 RID: 1041
		private const uint VER_SUITE_BACKOFFICE = 4U;

		// Token: 0x04000412 RID: 1042
		private const uint VER_SUITE_COMMUNICATIONS = 8U;

		// Token: 0x04000413 RID: 1043
		private const uint VER_SUITE_TERMINAL = 16U;

		// Token: 0x04000414 RID: 1044
		private const uint VER_SUITE_SMALLBUSINESS_RESTRICTED = 32U;

		// Token: 0x04000415 RID: 1045
		private const uint VER_SUITE_EMBEDDEDNT = 64U;

		// Token: 0x04000416 RID: 1046
		private const uint VER_SUITE_DATACENTER = 128U;

		// Token: 0x04000417 RID: 1047
		private const uint VER_SUITE_SINGLEUSERTS = 256U;

		// Token: 0x04000418 RID: 1048
		private const uint VER_SUITE_PERSONAL = 512U;

		// Token: 0x04000419 RID: 1049
		private const uint VER_SUITE_BLADE = 1024U;

		// Token: 0x0400041A RID: 1050
		private const uint VER_SUITE_EMBEDDED_RESTRICTED = 2048U;

		// Token: 0x0400041B RID: 1051
		private const uint VER_NT_WORKSTATION = 1U;

		// Token: 0x0400041C RID: 1052
		private const uint VER_NT_DOMAIN_CONTROLLER = 2U;

		// Token: 0x0400041D RID: 1053
		private const uint VER_NT_SERVER = 3U;

		// Token: 0x0400041E RID: 1054
		private const uint Windows9XMajorVersion = 4U;

		// Token: 0x0400041F RID: 1055
		private const uint RUNTIME_INFO_UPGRADE_VERSION = 1U;

		// Token: 0x04000420 RID: 1056
		private const uint RUNTIME_INFO_REQUEST_IA64 = 2U;

		// Token: 0x04000421 RID: 1057
		private const uint RUNTIME_INFO_REQUEST_AMD64 = 4U;

		// Token: 0x04000422 RID: 1058
		private const uint RUNTIME_INFO_REQUEST_X86 = 8U;

		// Token: 0x04000423 RID: 1059
		private const uint RUNTIME_INFO_DONT_RETURN_DIRECTORY = 16U;

		// Token: 0x04000424 RID: 1060
		private const uint RUNTIME_INFO_DONT_RETURN_VERSION = 32U;

		// Token: 0x04000425 RID: 1061
		private const uint RUNTIME_INFO_DONT_SHOW_ERROR_DIALOG = 64U;

		// Token: 0x04000426 RID: 1062
		private static PlatformDetector.Suite[] Suites = new PlatformDetector.Suite[]
		{
			new PlatformDetector.Suite("server", 2147483648U),
			new PlatformDetector.Suite("workstation", 1073741824U),
			new PlatformDetector.Suite("smallbusiness", 1U),
			new PlatformDetector.Suite("enterprise", 2U),
			new PlatformDetector.Suite("backoffice", 4U),
			new PlatformDetector.Suite("communications", 8U),
			new PlatformDetector.Suite("terminal", 16U),
			new PlatformDetector.Suite("smallbusinessRestricted", 32U),
			new PlatformDetector.Suite("embeddednt", 64U),
			new PlatformDetector.Suite("datacenter", 128U),
			new PlatformDetector.Suite("singleuserts", 256U),
			new PlatformDetector.Suite("personal", 512U),
			new PlatformDetector.Suite("blade", 1024U),
			new PlatformDetector.Suite("embeddedrestricted", 2048U)
		};

		// Token: 0x04000427 RID: 1063
		private static PlatformDetector.Product[] Products = new PlatformDetector.Product[]
		{
			new PlatformDetector.Product("workstation", 1U),
			new PlatformDetector.Product("domainController", 2U),
			new PlatformDetector.Product("server", 3U)
		};

		// Token: 0x020000BE RID: 190
		private enum NetFX35SP1SKU
		{
			// Token: 0x04000429 RID: 1065
			No35SP1,
			// Token: 0x0400042A RID: 1066
			Client35SP1,
			// Token: 0x0400042B RID: 1067
			Full35SP1
		}

		// Token: 0x020000BF RID: 191
		public class OSDependency
		{
			// Token: 0x060004BD RID: 1213 RVA: 0x0001827E File Offset: 0x0001727E
			public OSDependency()
			{
			}

			// Token: 0x060004BE RID: 1214 RVA: 0x00018286 File Offset: 0x00017286
			public OSDependency(uint dwMajorVersion, uint dwMinorVersion, uint dwBuildNumber, ushort wServicePackMajor, ushort wServicePackMinor, string suiteName, string productName)
			{
				this.dwMajorVersion = dwMajorVersion;
				this.dwMinorVersion = dwMinorVersion;
				this.dwBuildNumber = dwBuildNumber;
				this.wServicePackMajor = wServicePackMajor;
				this.wServicePackMinor = wServicePackMinor;
				this.suiteName = suiteName;
				this.productName = productName;
			}

			// Token: 0x060004BF RID: 1215 RVA: 0x000182C4 File Offset: 0x000172C4
			public OSDependency(NativeMethods.OSVersionInfoEx osvi)
			{
				this.dwMajorVersion = osvi.dwMajorVersion;
				this.dwMinorVersion = osvi.dwMinorVersion;
				this.dwMajorVersion = osvi.dwBuildNumber;
				this.dwMajorVersion = (uint)osvi.wServicePackMajor;
				this.dwMajorVersion = (uint)osvi.wServicePackMinor;
				this.suiteName = PlatformDetector.NameMap.MapMaskToName((uint)osvi.wSuiteMask, PlatformDetector.Suites);
				this.productName = PlatformDetector.NameMap.MapMaskToName((uint)osvi.bProductType, PlatformDetector.Products);
			}

			// Token: 0x0400042C RID: 1068
			public uint dwMajorVersion;

			// Token: 0x0400042D RID: 1069
			public uint dwMinorVersion;

			// Token: 0x0400042E RID: 1070
			public uint dwBuildNumber;

			// Token: 0x0400042F RID: 1071
			public ushort wServicePackMajor;

			// Token: 0x04000430 RID: 1072
			public ushort wServicePackMinor;

			// Token: 0x04000431 RID: 1073
			public string suiteName;

			// Token: 0x04000432 RID: 1074
			public string productName;
		}

		// Token: 0x020000C0 RID: 192
		public class NameMap
		{
			// Token: 0x060004C0 RID: 1216 RVA: 0x0001833F File Offset: 0x0001733F
			public NameMap(string Name, uint Mask)
			{
				this.name = Name;
				this.mask = Mask;
			}

			// Token: 0x060004C1 RID: 1217 RVA: 0x00018358 File Offset: 0x00017358
			public static uint MapNameToMask(string name, PlatformDetector.NameMap[] nmArray)
			{
				foreach (PlatformDetector.NameMap nameMap in nmArray)
				{
					if (nameMap.name == name)
					{
						return nameMap.mask;
					}
				}
				return 0U;
			}

			// Token: 0x060004C2 RID: 1218 RVA: 0x00018394 File Offset: 0x00017394
			public static string MapMaskToName(uint mask, PlatformDetector.NameMap[] nmArray)
			{
				foreach (PlatformDetector.NameMap nameMap in nmArray)
				{
					if (nameMap.mask == mask)
					{
						return nameMap.name;
					}
				}
				return null;
			}

			// Token: 0x04000433 RID: 1075
			public string name;

			// Token: 0x04000434 RID: 1076
			public uint mask;
		}

		// Token: 0x020000C1 RID: 193
		public class Suite : PlatformDetector.NameMap
		{
			// Token: 0x060004C3 RID: 1219 RVA: 0x000183CA File Offset: 0x000173CA
			public Suite(string Name, uint Mask)
				: base(Name, Mask)
			{
			}
		}

		// Token: 0x020000C2 RID: 194
		public class Product : PlatformDetector.NameMap
		{
			// Token: 0x060004C4 RID: 1220 RVA: 0x000183D4 File Offset: 0x000173D4
			public Product(string Name, uint Mask)
				: base(Name, Mask)
			{
			}
		}
	}
}
