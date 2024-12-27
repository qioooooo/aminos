using System;
using System.Globalization;
using System.Reflection;

namespace System.Management.Instrumentation
{
	// Token: 0x0200008D RID: 141
	internal sealed class AssemblyNameUtility
	{
		// Token: 0x0600043C RID: 1084 RVA: 0x000213C8 File Offset: 0x000203C8
		private static string BinToString(byte[] rg)
		{
			if (rg == null)
			{
				return "";
			}
			string text = "";
			for (int i = 0; i < rg.GetLength(0); i++)
			{
				text += string.Format("{0:x2}", rg[i]);
			}
			return text;
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00021410 File Offset: 0x00020410
		public static string UniqueToAssemblyMinorVersion(Assembly assembly)
		{
			AssemblyName name = assembly.GetName(true);
			return string.Concat(new object[]
			{
				name.Name,
				"_SN_",
				AssemblyNameUtility.BinToString(name.GetPublicKeyToken()),
				"_Version_",
				name.Version.Major,
				".",
				name.Version.Minor
			});
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00021488 File Offset: 0x00020488
		public static string UniqueToAssemblyFullVersion(Assembly assembly)
		{
			AssemblyName name = assembly.GetName(true);
			return string.Concat(new object[]
			{
				name.Name,
				"_SN_",
				AssemblyNameUtility.BinToString(name.GetPublicKeyToken()),
				"_Version_",
				name.Version.Major,
				".",
				name.Version.Minor,
				".",
				name.Version.Build,
				".",
				name.Version.Revision
			});
		}

		// Token: 0x0600043F RID: 1087 RVA: 0x00021538 File Offset: 0x00020538
		private static string UniqueToAssemblyVersion(Assembly assembly)
		{
			AssemblyName name = assembly.GetName(true);
			return string.Concat(new object[]
			{
				name.Name,
				"_SN_",
				AssemblyNameUtility.BinToString(name.GetPublicKeyToken()),
				"_Version_",
				name.Version.Major,
				".",
				name.Version.Minor,
				".",
				name.Version.Build,
				".",
				name.Version.Revision
			});
		}

		// Token: 0x06000440 RID: 1088 RVA: 0x000215E8 File Offset: 0x000205E8
		public static string UniqueToAssemblyBuild(Assembly assembly)
		{
			return AssemblyNameUtility.UniqueToAssemblyVersion(assembly) + "_Mvid_" + MetaDataInfo.GetMvid(assembly).ToString().ToLower(CultureInfo.InvariantCulture);
		}
	}
}
