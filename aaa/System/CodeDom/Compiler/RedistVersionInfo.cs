using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001FA RID: 506
	internal static class RedistVersionInfo
	{
		// Token: 0x0600113D RID: 4413 RVA: 0x000382F8 File Offset: 0x000372F8
		public static string GetCompilerPath(IDictionary<string, string> provOptions, string compilerExecutable)
		{
			string text = Executor.GetRuntimeInstallDirectory();
			string text2;
			if (provOptions != null && provOptions.TryGetValue("CompilerVersion", out text2))
			{
				string text3;
				if ((text3 = text2) != null)
				{
					if (text3 == "v3.5")
					{
						text = RedistVersionInfo.GetOrcasPath();
						goto IL_0043;
					}
					if (text3 == "v2.0")
					{
						goto IL_0043;
					}
				}
				text = null;
			}
			IL_0043:
			if (text == null)
			{
				throw new InvalidOperationException(SR.GetString("CompilerNotFound", new object[] { compilerExecutable }));
			}
			return text;
		}

		// Token: 0x0600113E RID: 4414 RVA: 0x00038368 File Offset: 0x00037368
		private static string GetOrcasPath()
		{
			string environmentVariable = Environment.GetEnvironmentVariable("COMPLUS_InstallRoot");
			string environmentVariable2 = Environment.GetEnvironmentVariable("COMPLUS_Version");
			string text;
			if (!string.IsNullOrEmpty(environmentVariable) && !string.IsNullOrEmpty(environmentVariable2))
			{
				text = Path.Combine(environmentVariable, environmentVariable2);
				if (Directory.Exists(text))
				{
					return text;
				}
			}
			text = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions\\3.5", "MSBuildToolsPath", null) as string;
			if (text != null && Directory.Exists(text))
			{
				return text;
			}
			return null;
		}

		// Token: 0x04000FAB RID: 4011
		internal const string NameTag = "CompilerVersion";

		// Token: 0x04000FAC RID: 4012
		internal const string DefaultVersion = "v2.0";

		// Token: 0x04000FAD RID: 4013
		internal const string InPlaceVersion = "v2.0";

		// Token: 0x04000FAE RID: 4014
		internal const string RedistVersion = "v3.5";

		// Token: 0x04000FAF RID: 4015
		private const string dotNetFrameworkSdkInstallKeyValueV35 = "MSBuildToolsPath";

		// Token: 0x04000FB0 RID: 4016
		private const string dotNetFrameworkRegistryPath = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\MSBuild\\ToolsVersions\\3.5";
	}
}
