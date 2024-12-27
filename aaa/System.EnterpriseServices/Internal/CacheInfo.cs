using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000D5 RID: 213
	internal static class CacheInfo
	{
		// Token: 0x060004D5 RID: 1237 RVA: 0x0000F850 File Offset: 0x0000E850
		internal static string GetCacheName(string AssemblyPath, string srcTypeLib)
		{
			string text = string.Empty;
			try
			{
				FileInfo fileInfo = new FileInfo(srcTypeLib);
				string cachePath = CacheInfo.GetCachePath(true);
				string text2 = fileInfo.Length.ToString(CultureInfo.InvariantCulture);
				string text3 = fileInfo.Name.ToString();
				string text4 = fileInfo.LastWriteTime.Year.ToString(CultureInfo.InvariantCulture);
				text4 = text4 + "_" + fileInfo.LastWriteTime.Month.ToString(CultureInfo.InvariantCulture);
				text4 = text4 + "_" + fileInfo.LastWriteTime.Day.ToString(CultureInfo.InvariantCulture);
				text4 = text4 + "_" + fileInfo.LastWriteTime.Hour.ToString(CultureInfo.InvariantCulture);
				text4 = text4 + "_" + fileInfo.LastWriteTime.Minute.ToString(CultureInfo.InvariantCulture);
				text4 = text4 + "_" + fileInfo.LastWriteTime.Second.ToString(CultureInfo.InvariantCulture);
				string text5 = string.Concat(new string[] { text3, "_", text2, "_", text4 });
				text5 = cachePath + text5 + "\\";
				if (!Directory.Exists(text5))
				{
					Directory.CreateDirectory(text5);
				}
				char[] array = new char[] { '/', '\\' };
				int num = AssemblyPath.LastIndexOfAny(array) + 1;
				if (num <= 0)
				{
					num = 0;
				}
				string text6 = AssemblyPath.Substring(num, AssemblyPath.Length - num);
				text5 += text6;
				text = text5;
			}
			catch (Exception ex)
			{
				text = string.Empty;
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				text = string.Empty;
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "CacheInfo.GetCacheName"));
			}
			return text;
		}

		// Token: 0x060004D6 RID: 1238 RVA: 0x0000FA94 File Offset: 0x0000EA94
		internal static string GetCachePath(bool CreateDir)
		{
			StringBuilder stringBuilder = new StringBuilder(1024, 1024);
			uint num = 1024U;
			Publish.GetSystemDirectory(stringBuilder, num);
			string text = stringBuilder.ToString();
			text += "\\com\\SOAPCache\\";
			if (CreateDir)
			{
				try
				{
					if (!Directory.Exists(text))
					{
						Directory.CreateDirectory(text);
					}
				}
				catch (Exception ex)
				{
					ComSoapPublishError.Report(ex.ToString());
				}
				catch
				{
					ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "CacheInfo.GetCachePath"));
				}
			}
			return text;
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0000FB28 File Offset: 0x0000EB28
		internal static string GetMetadataName(string strSrcTypeLib, ITypeLib TypeLib, out string strMetaFileRoot)
		{
			string text = "";
			strMetaFileRoot = "";
			if (TypeLib == null)
			{
				TypeLib = CacheInfo.GetTypeLib(strSrcTypeLib);
				if (TypeLib == null)
				{
					return text;
				}
			}
			text = Marshal.GetTypeLibName(TypeLib);
			strMetaFileRoot = text + ".dll";
			char[] array = new char[] { '/', '\\' };
			int num = strSrcTypeLib.LastIndexOfAny(array) + 1;
			if (num <= 0)
			{
				num = 0;
			}
			string text2 = strSrcTypeLib.Substring(num, strSrcTypeLib.Length - num);
			if (text2.ToLower(CultureInfo.InvariantCulture) == strMetaFileRoot.ToLower(CultureInfo.InvariantCulture))
			{
				text += "SoapLib";
				strMetaFileRoot = text + ".dll";
			}
			return text;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0000FBD8 File Offset: 0x0000EBD8
		internal static ITypeLib GetTypeLib(string strTypeLibPath)
		{
			ITypeLib typeLib = null;
			try
			{
				CacheInfo.LoadTypeLibEx(strTypeLibPath, REGKIND.REGKIND_NONE, out typeLib);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147312566)
				{
					string text = Resource.FormatString("Soap_InputFileNotValidTypeLib");
					ComSoapPublishError.Report(text + " " + strTypeLibPath);
				}
				else
				{
					ComSoapPublishError.Report(ex.ToString());
				}
				return null;
			}
			return typeLib;
		}

		// Token: 0x060004D9 RID: 1241
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
		private static extern void LoadTypeLibEx(string strTypeLibName, REGKIND regKind, out ITypeLib TypeLib);
	}
}
