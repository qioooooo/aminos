using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using Microsoft.Win32;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000D8 RID: 216
	[Guid("d8013ff1-730b-45e2-ba24-874b7242c425")]
	public class GenerateMetadata : IComSoapMetadata
	{
		// Token: 0x060004FB RID: 1275 RVA: 0x00011214 File Offset: 0x00010214
		internal string GetAssemblyName(string strSrcTypeLib, string outPath)
		{
			this._nameonly = true;
			return this.Generate(strSrcTypeLib, outPath);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x00011225 File Offset: 0x00010225
		public string Generate(string strSrcTypeLib, string outPath)
		{
			return this.GenerateMetaData(strSrcTypeLib, outPath, null, null);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00011234 File Offset: 0x00010234
		public string GenerateSigned(string strSrcTypeLib, string outPath, bool InstallGac, out string Error)
		{
			string text = "";
			this._signed = true;
			try
			{
				Error = "";
				uint num = 0U;
				IntPtr zero = IntPtr.Zero;
				uint num2 = 0U;
				GenerateMetadata.StrongNameKeyGen(strSrcTypeLib, num, out zero, out num2);
				byte[] array = new byte[num2];
				Marshal.Copy(zero, array, 0, (int)num2);
				GenerateMetadata.StrongNameFreeBuffer(zero);
				StrongNameKeyPair strongNameKeyPair = new StrongNameKeyPair(array);
				text = this.GenerateMetaData(strSrcTypeLib, outPath, null, strongNameKeyPair);
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				ComSoapPublishError.Report(Error);
			}
			catch
			{
				Error = Resource.FormatString("Err_NonClsException", "GenerateMetadata.GenerateSigned");
				ComSoapPublishError.Report(Error);
			}
			return text;
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x000112F4 File Offset: 0x000102F4
		public string GenerateMetaData(string strSrcTypeLib, string outPath, byte[] PublicKey, StrongNameKeyPair KeyPair)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "GenerateMetadata.GenerateMetaData"));
				throw;
			}
			string text = "";
			if (0 >= strSrcTypeLib.Length || 0 >= outPath.Length)
			{
				return text;
			}
			if (!outPath.EndsWith("/", StringComparison.Ordinal) && !outPath.EndsWith("\\", StringComparison.Ordinal))
			{
				outPath += "\\";
			}
			ITypeLib typeLib = null;
			typeLib = CacheInfo.GetTypeLib(strSrcTypeLib);
			if (typeLib == null)
			{
				return text;
			}
			string text2;
			text = CacheInfo.GetMetadataName(strSrcTypeLib, typeLib, out text2);
			if (text.Length == 0)
			{
				return text;
			}
			if (this._nameonly)
			{
				return text;
			}
			string text3 = outPath + text2;
			if (this._signed)
			{
				try
				{
					AssemblyManager assemblyManager = new AssemblyManager();
					if (assemblyManager.CompareToCache(text3, strSrcTypeLib))
					{
						Publish publish = new Publish();
						publish.GacInstall(text3);
						return text;
					}
					if (assemblyManager.GetFromCache(text3, strSrcTypeLib))
					{
						Publish publish2 = new Publish();
						publish2.GacInstall(text3);
						return text;
					}
					goto IL_013B;
				}
				catch (Exception ex2)
				{
					ComSoapPublishError.Report(ex2.ToString());
					goto IL_013B;
				}
				catch
				{
					ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "GenerateMetadata.GenerateMetaData"));
					goto IL_013B;
				}
			}
			if (File.Exists(text3))
			{
				return text;
			}
			try
			{
				IL_013B:
				ITypeLibConverter typeLibConverter = new TypeLibConverter();
				AssemblyBuilder assemblyBuilder = typeLibConverter.ConvertTypeLibToAssembly(typeLib, text3, TypeLibImporterFlags.UnsafeInterfaces, new GenerateMetadata.ImporterCallback
				{
					OutputDir = outPath
				}, PublicKey, KeyPair, null, null);
				FileInfo fileInfo = new FileInfo(text3);
				assemblyBuilder.Save(fileInfo.Name);
				if (this._signed)
				{
					AssemblyManager assemblyManager2 = new AssemblyManager();
					assemblyManager2.CopyToCache(text3, strSrcTypeLib);
					Publish publish3 = new Publish();
					publish3.GacInstall(text3);
				}
			}
			catch (ReflectionTypeLoadException ex3)
			{
				Exception[] loaderExceptions = ex3.LoaderExceptions;
				for (int i = 0; i < loaderExceptions.Length; i++)
				{
					try
					{
						ComSoapPublishError.Report(loaderExceptions[i].ToString());
					}
					catch (Exception ex4)
					{
						ComSoapPublishError.Report(ex4.ToString());
					}
					catch
					{
						ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "GenerateMetaData.GenerateMetaData"));
					}
				}
				return string.Empty;
			}
			catch (Exception ex5)
			{
				ComSoapPublishError.Report(ex5.ToString());
				return string.Empty;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "GenerateMetaData.GenerateMetaData"));
				return string.Empty;
			}
			return text;
		}

		// Token: 0x060004FF RID: 1279
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int SearchPath(string path, string fileName, string extension, int numBufferChars, string buffer, int[] filePart);

		// Token: 0x06000500 RID: 1280
		[DllImport("mscoree.dll")]
		private static extern int StrongNameKeyGen(string wszKeyContainer, uint dwFlags, out IntPtr ppbKeyBlob, out uint pcbKeyBlob);

		// Token: 0x06000501 RID: 1281
		[DllImport("mscoree.dll")]
		private static extern void StrongNameFreeBuffer(IntPtr ppbKeyBlob);

		// Token: 0x040001FE RID: 510
		internal bool _signed;

		// Token: 0x040001FF RID: 511
		internal bool _nameonly;

		// Token: 0x020000D9 RID: 217
		internal class ImporterCallback : ITypeLibImporterNotifySink
		{
			// Token: 0x06000503 RID: 1283 RVA: 0x000115C0 File Offset: 0x000105C0
			public void ReportEvent(ImporterEventKind EventKind, int EventCode, string EventMsg)
			{
			}

			// Token: 0x06000504 RID: 1284 RVA: 0x000115C4 File Offset: 0x000105C4
			internal string GetTlbPath(string guidAttr, string strMajorVer, string strMinorVer)
			{
				string text = string.Concat(new string[] { "TypeLib\\{", guidAttr, "}\\", strMajorVer, ".", strMinorVer, "\\0\\win32" });
				RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(text);
				if (registryKey == null)
				{
					throw new COMException(Resource.FormatString("Soap_ResolutionForTypeLibFailed") + " " + guidAttr, Util.REGDB_E_CLASSNOTREG);
				}
				return (string)registryKey.GetValue("");
			}

			// Token: 0x06000505 RID: 1285 RVA: 0x00011654 File Offset: 0x00010654
			public Assembly ResolveRef(object TypeLib)
			{
				Assembly assembly = null;
				IntPtr intPtr = (IntPtr)0;
				try
				{
					((ITypeLib)TypeLib).GetLibAttr(out intPtr);
					global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR typelibattr = (global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR)Marshal.PtrToStructure(intPtr, typeof(global::System.Runtime.InteropServices.ComTypes.TYPELIBATTR));
					string tlbPath = this.GetTlbPath(typelibattr.guid.ToString(string.Empty, CultureInfo.InvariantCulture), typelibattr.wMajorVerNum.ToString(CultureInfo.InvariantCulture), typelibattr.wMinorVerNum.ToString(CultureInfo.InvariantCulture));
					if (tlbPath.Length > 0)
					{
						GenerateMetadata generateMetadata = new GenerateMetadata();
						string text = "";
						string text2 = generateMetadata.GenerateSigned(tlbPath, this.m_strOutputDir, true, out text);
						if (text2.Length > 0)
						{
							assembly = Assembly.Load(text2, null);
						}
					}
				}
				finally
				{
					if (intPtr != (IntPtr)0)
					{
						((ITypeLib)TypeLib).ReleaseTLibAttr(intPtr);
					}
				}
				if (assembly == null)
				{
					string typeLibName = Marshal.GetTypeLibName((ITypeLib)TypeLib);
					string text3 = Resource.FormatString("Soap_ResolutionForTypeLibFailed");
					ComSoapPublishError.Report(text3 + " " + typeLibName);
				}
				return assembly;
			}

			// Token: 0x170000A8 RID: 168
			// (set) Token: 0x06000506 RID: 1286 RVA: 0x00011768 File Offset: 0x00010768
			internal string OutputDir
			{
				set
				{
					this.m_strOutputDir = value;
				}
			}

			// Token: 0x04000200 RID: 512
			private string m_strOutputDir = "";
		}
	}
}
