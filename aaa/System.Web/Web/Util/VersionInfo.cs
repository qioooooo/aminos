using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace System.Web.Util
{
	// Token: 0x02000799 RID: 1945
	internal class VersionInfo
	{
		// Token: 0x06005D5C RID: 23900 RVA: 0x00175FE4 File Offset: 0x00174FE4
		private VersionInfo()
		{
		}

		// Token: 0x06005D5D RID: 23901 RVA: 0x00175FEC File Offset: 0x00174FEC
		internal static string GetFileVersion(string filename)
		{
			string text;
			try
			{
				FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filename);
				text = string.Format(CultureInfo.InvariantCulture, "{0}.{1}.{2}.{3}", new object[] { versionInfo.FileMajorPart, versionInfo.FileMinorPart, versionInfo.FileBuildPart, versionInfo.FilePrivatePart });
			}
			catch
			{
				text = string.Empty;
			}
			return text;
		}

		// Token: 0x06005D5E RID: 23902 RVA: 0x0017606C File Offset: 0x0017506C
		internal static string GetLoadedModuleFileName(string module)
		{
			IntPtr moduleHandle = UnsafeNativeMethods.GetModuleHandle(module);
			if (moduleHandle == IntPtr.Zero)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder(256);
			if (UnsafeNativeMethods.GetModuleFileName(moduleHandle, stringBuilder, 256) == 0)
			{
				return null;
			}
			string text = stringBuilder.ToString();
			if (StringUtil.StringStartsWith(text, "\\\\?\\"))
			{
				text = text.Substring(4);
			}
			return text;
		}

		// Token: 0x06005D5F RID: 23903 RVA: 0x001760C8 File Offset: 0x001750C8
		internal static string GetLoadedModuleVersion(string module)
		{
			string loadedModuleFileName = VersionInfo.GetLoadedModuleFileName(module);
			if (loadedModuleFileName == null)
			{
				return null;
			}
			return VersionInfo.GetFileVersion(loadedModuleFileName);
		}

		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x06005D60 RID: 23904 RVA: 0x001760E8 File Offset: 0x001750E8
		internal static string SystemWebVersion
		{
			get
			{
				if (VersionInfo._systemWebVersion == null)
				{
					lock (VersionInfo._lock)
					{
						if (VersionInfo._systemWebVersion == null)
						{
							VersionInfo._systemWebVersion = VersionInfo.GetFileVersion(typeof(HttpRuntime).Module.FullyQualifiedName);
						}
					}
				}
				return VersionInfo._systemWebVersion;
			}
		}

		// Token: 0x170017E9 RID: 6121
		// (get) Token: 0x06005D61 RID: 23905 RVA: 0x0017614C File Offset: 0x0017514C
		internal static string EngineVersion
		{
			get
			{
				if (VersionInfo._engineVersion == null)
				{
					lock (VersionInfo._lock)
					{
						if (VersionInfo._engineVersion == null)
						{
							VersionInfo._engineVersion = VersionInfo.GetLoadedModuleVersion("webengine.dll");
						}
					}
				}
				return VersionInfo._engineVersion;
			}
		}

		// Token: 0x170017EA RID: 6122
		// (get) Token: 0x06005D62 RID: 23906 RVA: 0x001761A0 File Offset: 0x001751A0
		internal static string ClrVersion
		{
			get
			{
				if (VersionInfo._mscoreeVersion == null)
				{
					lock (VersionInfo._lock)
					{
						if (VersionInfo._mscoreeVersion == null)
						{
							VersionInfo._mscoreeVersion = VersionInfo.GetLoadedModuleVersion("MSCORWKS.DLL");
						}
					}
				}
				return VersionInfo._mscoreeVersion;
			}
		}

		// Token: 0x170017EB RID: 6123
		// (get) Token: 0x06005D63 RID: 23907 RVA: 0x001761F4 File Offset: 0x001751F4
		internal static string ExeName
		{
			get
			{
				if (VersionInfo._exeName == null)
				{
					lock (VersionInfo._lock)
					{
						if (VersionInfo._exeName == null)
						{
							string text = VersionInfo.GetLoadedModuleFileName(null);
							if (text == null)
							{
								text = string.Empty;
							}
							int num = text.LastIndexOf('\\');
							if (num >= 0)
							{
								text = text.Substring(num + 1);
							}
							num = text.LastIndexOf('.');
							if (num >= 0)
							{
								text = text.Substring(0, num);
							}
							VersionInfo._exeName = text.ToLower(CultureInfo.InvariantCulture);
						}
					}
				}
				return VersionInfo._exeName;
			}
		}

		// Token: 0x040031C6 RID: 12742
		private static string _systemWebVersion;

		// Token: 0x040031C7 RID: 12743
		private static string _engineVersion;

		// Token: 0x040031C8 RID: 12744
		private static string _mscoreeVersion;

		// Token: 0x040031C9 RID: 12745
		private static string _exeName;

		// Token: 0x040031CA RID: 12746
		private static object _lock = new object();
	}
}
