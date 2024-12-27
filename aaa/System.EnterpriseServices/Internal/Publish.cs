using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000D7 RID: 215
	[Guid("d8013eef-730b-45e2-ba24-874b7242c425")]
	public class Publish : IComSoapPublisher
	{
		// Token: 0x060004E5 RID: 1253 RVA: 0x000100F4 File Offset: 0x0000F0F4
		public void RegisterAssembly(string AssemblyPath)
		{
			try
			{
				RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
				registryPermission.Demand();
				registryPermission.Assert();
				Assembly assembly = Assembly.LoadFrom(AssemblyPath);
				RegistrationServices registrationServices = new RegistrationServices();
				registrationServices.RegisterAssembly(assembly, AssemblyRegistrationFlags.SetCodeBase);
				Version version = Assembly.GetExecutingAssembly().GetName().Version;
				foreach (AssemblyName assemblyName in assembly.GetReferencedAssemblies())
				{
					if (assemblyName.Name == "System.EnterpriseServices" && version < assemblyName.Version)
					{
						Uri uri = new Uri(assembly.Location);
						if (uri.IsFile && uri.LocalPath != "")
						{
							string text = uri.LocalPath.Remove(uri.LocalPath.Length - Path.GetFileName(uri.LocalPath).Length, Path.GetFileName(uri.LocalPath).Length);
							string[] files = Directory.GetFiles(text, "*.tlb");
							foreach (string text2 in files)
							{
								Guid guid = new Guid("90883F05-3D28-11D2-8F17-00A0C9A6186D");
								ITypeLib typeLib;
								Marshal.ThrowExceptionForHR(Publish.LoadTypeLib(text2, out typeLib));
								object obj;
								if (((ITypeLib2)typeLib).GetCustData(ref guid, out obj) == 0 && (string)obj == assembly.FullName)
								{
									Marshal.ReleaseComObject(typeLib);
									RegistrationDriver.GenerateTypeLibrary(assembly, text2, null);
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "Publish.RegisterAssembly"));
				throw;
			}
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x000102D8 File Offset: 0x0000F2D8
		public void UnRegisterAssembly(string AssemblyPath)
		{
			try
			{
				RegistryPermission registryPermission = new RegistryPermission(PermissionState.Unrestricted);
				registryPermission.Demand();
				registryPermission.Assert();
				Assembly assembly = Assembly.LoadFrom(AssemblyPath);
				new RegistrationServices().UnregisterAssembly(assembly);
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "Publish.UnregisterAssembly"));
				throw;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00010350 File Offset: 0x0000F350
		private static string MsCorLibDirectory
		{
			get
			{
				string text = Assembly.GetAssembly(typeof(object)).Location.Replace('/', '\\');
				return Path.GetDirectoryName(text);
			}
		}

		// Token: 0x060004E8 RID: 1256 RVA: 0x00010384 File Offset: 0x0000F384
		public void GacInstall(string AssemblyPath)
		{
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			string text = Path.Combine(Publish.MsCorLibDirectory, "fusion.dll");
			IntPtr intPtr = Publish.LoadLibrary(text);
			if (intPtr == IntPtr.Zero)
			{
				throw new DllNotFoundException(text);
			}
			this.PrivateGacInstall(AssemblyPath);
		}

		// Token: 0x060004E9 RID: 1257 RVA: 0x000103D0 File Offset: 0x0000F3D0
		private void PrivateGacInstall(string AssemblyPath)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				IAssemblyCache assemblyCache = null;
				int num = Publish.CreateAssemblyCache(out assemblyCache, 0U);
				if (num == 0)
				{
					num = assemblyCache.InstallAssembly(0U, AssemblyPath, (IntPtr)0);
				}
				if (num != 0)
				{
					string text = Resource.FormatString("Soap_GacInstallFailed");
					ComSoapPublishError.Report(text + " " + AssemblyPath);
				}
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "Publish.PrivateGacInstall"));
				throw;
			}
		}

		// Token: 0x060004EA RID: 1258 RVA: 0x0001046C File Offset: 0x0000F46C
		public void GacRemove(string AssemblyPath)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				AssemblyManager assemblyManager = new AssemblyManager();
				string gacName = assemblyManager.GetGacName(AssemblyPath);
				IAssemblyCache assemblyCache = null;
				int num = Publish.CreateAssemblyCache(out assemblyCache, 0U);
				uint num2 = 0U;
				if (num == 0)
				{
					num = assemblyCache.UninstallAssembly(0U, gacName, (IntPtr)0, out num2);
				}
				if (num != 0)
				{
					string text = Resource.FormatString("Soap_GacRemoveFailed");
					ComSoapPublishError.Report(string.Concat(new string[] { text, " ", AssemblyPath, " ", gacName }));
				}
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "Publish.GacRemove"));
				throw;
			}
		}

		// Token: 0x060004EB RID: 1259 RVA: 0x00010544 File Offset: 0x0000F544
		public void GetAssemblyNameForCache(string TypeLibPath, out string CachePath)
		{
			CacheInfo.GetMetadataName(TypeLibPath, null, out CachePath);
			CachePath = CacheInfo.GetCacheName(CachePath, TypeLibPath);
		}

		// Token: 0x060004EC RID: 1260 RVA: 0x0001055C File Offset: 0x0000F55C
		public static string GetClientPhysicalPath(bool CreateDir)
		{
			StringBuilder stringBuilder = new StringBuilder(1024, 1024);
			uint num = 1024U;
			Publish.GetSystemDirectory(stringBuilder, num);
			string text = stringBuilder.ToString() + "\\com\\SOAPAssembly\\";
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
					text = string.Empty;
					ComSoapPublishError.Report(ex.ToString());
				}
				catch
				{
					text = string.Empty;
					ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "Publish.GetClientPhysicalPath"));
				}
			}
			return text;
		}

		// Token: 0x060004ED RID: 1261 RVA: 0x000105FC File Offset: 0x0000F5FC
		private bool GetVRootPhysicalPath(string VirtualRoot, out string PhysicalPath, out string BinDirectory, bool CreateDir)
		{
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder(1024, 1024);
			uint num = 1024U;
			Publish.GetSystemDirectory(stringBuilder, num);
			string text = stringBuilder.ToString();
			text += "\\com\\SOAPVRoots\\";
			PhysicalPath = text + VirtualRoot + "\\";
			BinDirectory = PhysicalPath + "bin\\";
			if (CreateDir)
			{
				try
				{
					try
					{
						if (!Directory.Exists(text))
						{
							Directory.CreateDirectory(text);
						}
					}
					catch
					{
					}
					try
					{
						if (!Directory.Exists(PhysicalPath))
						{
							Directory.CreateDirectory(PhysicalPath);
						}
					}
					catch
					{
					}
					try
					{
						if (!Directory.Exists(BinDirectory))
						{
							Directory.CreateDirectory(BinDirectory);
							flag = false;
						}
					}
					catch
					{
					}
					return flag;
				}
				catch (Exception ex)
				{
					PhysicalPath = string.Empty;
					BinDirectory = string.Empty;
					ComSoapPublishError.Report(ex.ToString());
					return flag;
				}
				catch
				{
					PhysicalPath = string.Empty;
					BinDirectory = string.Empty;
					ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "Publish.GetVRootPhysicalPath"));
					return flag;
				}
			}
			flag = Directory.Exists(BinDirectory);
			return flag;
		}

		// Token: 0x060004EE RID: 1262 RVA: 0x00010730 File Offset: 0x0000F730
		public static void ParseUrl(string FullUrl, out string BaseUrl, out string VirtualRoot)
		{
			try
			{
				Uri uri = new Uri(FullUrl);
				string[] segments = uri.Segments;
				VirtualRoot = segments[segments.GetUpperBound(0)];
				BaseUrl = FullUrl.Substring(0, FullUrl.Length - VirtualRoot.Length);
				char[] array = new char[] { '/' };
				VirtualRoot = VirtualRoot.TrimEnd(array);
			}
			catch
			{
				BaseUrl = string.Empty;
				VirtualRoot = FullUrl;
			}
			if (BaseUrl.Length <= 0)
			{
				try
				{
					BaseUrl = "http://";
					BaseUrl += Dns.GetHostName();
					BaseUrl += "/";
				}
				catch (Exception ex)
				{
					ComSoapPublishError.Report(ex.ToString());
				}
				catch
				{
					ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "Publish.ParseUrl"));
				}
			}
		}

		// Token: 0x060004EF RID: 1263 RVA: 0x00010814 File Offset: 0x0000F814
		public void CreateVirtualRoot(string Operation, string FullUrl, out string BaseUrl, out string VirtualRoot, out string PhysicalPath, out string Error)
		{
			BaseUrl = "";
			VirtualRoot = "";
			PhysicalPath = "";
			Error = "";
			if (FullUrl.Length <= 0)
			{
				return;
			}
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				Publish.ParseUrl(FullUrl, out BaseUrl, out VirtualRoot);
				if (VirtualRoot.Length > 0)
				{
					string text = "IIS://localhost/W3SVC/1/ROOT";
					bool flag = true;
					if (Operation.ToLower(CultureInfo.InvariantCulture) == "delete" || Operation.ToLower(CultureInfo.InvariantCulture) == "addcomponent")
					{
						flag = false;
					}
					string text2 = "";
					this.GetVRootPhysicalPath(VirtualRoot, out PhysicalPath, out text2, flag);
					if (PhysicalPath.Length <= 0)
					{
						Error = Resource.FormatString("Soap_VRootDirectoryCreationFailed");
					}
					else if (flag)
					{
						ServerWebConfig serverWebConfig = new ServerWebConfig();
						string text3 = "";
						serverWebConfig.Create(PhysicalPath, "Web", out text3);
						DiscoFile discoFile = new DiscoFile();
						discoFile.Create(PhysicalPath, "Default.disco");
						HomePage homePage = new HomePage();
						homePage.Create(PhysicalPath, VirtualRoot, "Default.aspx", "Default.disco");
						string text4 = "";
						try
						{
							IISVirtualRoot iisvirtualRoot = new IISVirtualRoot();
							iisvirtualRoot.Create(text, PhysicalPath, VirtualRoot, out text4);
						}
						catch (Exception ex)
						{
							if (text4.Length <= 0)
							{
								string text5 = Resource.FormatString("Soap_VRootCreationFailed");
								text4 = string.Format(CultureInfo.CurrentCulture, string.Concat(new string[]
								{
									text5,
									" ",
									VirtualRoot,
									" ",
									ex.ToString()
								}), new object[0]);
							}
						}
						catch
						{
							if (text4.Length <= 0)
							{
								text4 = Resource.FormatString("Soap_VRootCreationFailed") + VirtualRoot + " " + Resource.FormatString("Err_NonClsException", "Publish.CreateVirtualRoot");
							}
						}
						if (text4.Length > 0)
						{
							Error = text4;
						}
					}
				}
			}
			catch (Exception ex2)
			{
				Error = ex2.ToString();
				ComSoapPublishError.Report(Error);
			}
			catch
			{
				Error = Resource.FormatString("Err_NonClsException", "Publish.CreateVirtualRoot");
				ComSoapPublishError.Report(Error);
			}
		}

		// Token: 0x060004F0 RID: 1264 RVA: 0x00010A9C File Offset: 0x0000FA9C
		public void DeleteVirtualRoot(string RootWebServer, string FullUrl, out string Error)
		{
			Error = "";
			try
			{
				if (FullUrl.Length > 0)
				{
					SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
					securityPermission.Demand();
					int length = RootWebServer.Length;
					string text = "";
					string text2 = "";
					Publish.ParseUrl(FullUrl, out text, out text2);
					if (text2.Length > 0)
					{
						string text3 = "";
						string text4 = "";
						this.GetVRootPhysicalPath(text2, out text3, out text4, false);
						if (text3.Length <= 0)
						{
						}
					}
				}
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				Error = Resource.FormatString("Err_NonClsException", "Publish.DeleteVirtualRoot");
				ComSoapPublishError.Report(Error);
			}
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x00010B6C File Offset: 0x0000FB6C
		public void CreateMailBox(string RootMailServer, string MailBox, out string SmtpName, out string Domain, out string PhysicalPath, out string Error)
		{
			SmtpName = "";
			Domain = "";
			PhysicalPath = "";
			Error = "";
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			string text = Resource.FormatString("Soap_SmtpNotImplemented");
			ComSoapPublishError.Report(text);
			if (MailBox.Length <= 0)
			{
			}
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00010BC0 File Offset: 0x0000FBC0
		public void DeleteMailBox(string RootMailServer, string MailBox, out string Error)
		{
			Error = "";
			SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
			securityPermission.Demand();
			string text = Resource.FormatString("Soap_SmtpNotImplemented");
			ComSoapPublishError.Report(text);
			if (MailBox.Length <= 0)
			{
			}
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00010BFC File Offset: 0x0000FBFC
		public void ProcessServerTlb(string ProgId, string SrcTlbPath, string PhysicalPath, string Operation, out string strAssemblyName, out string TypeName, out string Error)
		{
			strAssemblyName = "";
			TypeName = "";
			Error = "";
			bool flag = false;
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				if (Operation != null && Operation.ToLower(CultureInfo.InvariantCulture) == "delete")
				{
					flag = true;
				}
				if (SrcTlbPath.Length > 0)
				{
					if (!PhysicalPath.EndsWith("/", StringComparison.Ordinal) && !PhysicalPath.EndsWith("\\", StringComparison.Ordinal))
					{
						PhysicalPath += "\\";
					}
					string text = SrcTlbPath.ToLower(CultureInfo.InvariantCulture);
					if (text.EndsWith("mscoree.dll", StringComparison.Ordinal))
					{
						Type typeFromProgID = Type.GetTypeFromProgID(ProgId);
						if (typeFromProgID.FullName == "System.__ComObject")
						{
							throw new ServicedComponentException(Resource.FormatString("ServicedComponentException_DependencyNotInGAC"));
						}
						TypeName = typeFromProgID.FullName;
						Assembly assembly = typeFromProgID.Assembly;
						strAssemblyName = assembly.GetName().Name;
					}
					else if (text.EndsWith("scrobj.dll", StringComparison.Ordinal))
					{
						if (!flag)
						{
							throw new ServicedComponentException(Resource.FormatString("ServicedComponentException_WSCNotSupported"));
						}
					}
					else
					{
						GenerateMetadata generateMetadata = new GenerateMetadata();
						if (flag)
						{
							strAssemblyName = generateMetadata.GetAssemblyName(SrcTlbPath, PhysicalPath + "bin\\");
						}
						else
						{
							strAssemblyName = generateMetadata.GenerateSigned(SrcTlbPath, PhysicalPath + "bin\\", false, out Error);
						}
						if (strAssemblyName.Length > 0)
						{
							try
							{
								TypeName = this.GetTypeNameFromProgId(PhysicalPath + "bin\\" + strAssemblyName + ".dll", ProgId);
							}
							catch (DirectoryNotFoundException)
							{
								if (!flag)
								{
									throw;
								}
							}
							catch (FileNotFoundException)
							{
								if (!flag)
								{
									throw;
								}
							}
						}
					}
					if (ProgId.Length > 0 && strAssemblyName.Length > 0 && TypeName.Length > 0)
					{
						ServerWebConfig serverWebConfig = new ServerWebConfig();
						DiscoFile discoFile = new DiscoFile();
						string text2 = PhysicalPath + "bin\\" + strAssemblyName + ".dll";
						if (flag)
						{
							serverWebConfig.DeleteElement(PhysicalPath + "Web.Config", strAssemblyName, TypeName, ProgId, "SingleCall", text2);
							discoFile.DeleteElement(PhysicalPath + "Default.disco", ProgId + ".soap?WSDL");
						}
						else
						{
							serverWebConfig.AddGacElement(PhysicalPath + "Web.Config", strAssemblyName, TypeName, ProgId, "SingleCall", text2);
							discoFile.AddElement(PhysicalPath + "Default.disco", ProgId + ".soap?WSDL");
						}
					}
				}
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				ComSoapPublishError.Report(Error);
				if (typeof(ServicedComponentException) == ex.GetType() || typeof(RegistrationException) == ex.GetType())
				{
					throw;
				}
			}
			catch
			{
				Error = Resource.FormatString("Err_NonClsException", "Publish.ProcessServerTlb");
				ComSoapPublishError.Report(Error);
			}
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x00010F14 File Offset: 0x0000FF14
		public string GetTypeNameFromProgId(string AssemblyPath, string ProgId)
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
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "Publish.GetTypeNameFromProgId"));
				throw;
			}
			string text = "";
			AppDomainSetup appDomainSetup = new AppDomainSetup();
			AppDomain appDomain = AppDomain.CreateDomain("SoapDomain", null, appDomainSetup);
			if (appDomain != null)
			{
				try
				{
					ObjectHandle objectHandle = appDomain.CreateInstance(typeof(AssemblyManager).Assembly.FullName, typeof(AssemblyManager).FullName);
					if (objectHandle != null)
					{
						AssemblyManager assemblyManager = (AssemblyManager)objectHandle.Unwrap();
						text = assemblyManager.InternalGetTypeNameFromProgId(AssemblyPath, ProgId);
					}
				}
				finally
				{
					AppDomain.Unload(appDomain);
				}
			}
			return text;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00010FF0 File Offset: 0x0000FFF0
		public void ProcessClientTlb(string ProgId, string SrcTlbPath, string PhysicalPath, string VRoot, string BaseUrl, string Mode, string Transport, out string AssemblyName, out string TypeName, out string Error)
		{
			AssemblyName = "";
			TypeName = "";
			Error = "";
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				string clientPhysicalPath = Publish.GetClientPhysicalPath(true);
				string text = SrcTlbPath.ToLower(CultureInfo.InvariantCulture);
				if (!text.EndsWith("mscoree.dll", StringComparison.Ordinal) && SrcTlbPath.Length > 0)
				{
					GenerateMetadata generateMetadata = new GenerateMetadata();
					AssemblyName = generateMetadata.Generate(SrcTlbPath, clientPhysicalPath);
					if (ProgId.Length > 0)
					{
						TypeName = this.GetTypeNameFromProgId(clientPhysicalPath + AssemblyName + ".dll", ProgId);
					}
				}
				else if (ProgId.Length > 0)
				{
					RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey(ProgId + "\\CLSID");
					string text2 = (string)registryKey.GetValue("");
					Guid guid = new Guid(text2);
					RegistryKey registryKey2 = Registry.ClassesRoot.OpenSubKey("CLSID\\{" + guid + "}\\InprocServer32");
					AssemblyName = (string)registryKey2.GetValue("Assembly");
					int num = AssemblyName.IndexOf(",");
					if (num > 0)
					{
						AssemblyName = AssemblyName.Substring(0, num);
					}
					TypeName = (string)registryKey2.GetValue("Class");
				}
				if (ProgId.Length > 0)
				{
					Uri uri = new Uri(BaseUrl);
					Uri uri2 = new Uri(uri, VRoot);
					if (uri2.Scheme.ToLower(CultureInfo.InvariantCulture) == "https")
					{
						string text3 = "Windows";
						SoapClientConfig.Write(clientPhysicalPath, uri2.AbsoluteUri, AssemblyName, TypeName, ProgId, text3);
					}
					else
					{
						ClientRemotingConfig.Write(clientPhysicalPath, VRoot, BaseUrl, AssemblyName, TypeName, ProgId, Mode, Transport);
					}
				}
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				ComSoapPublishError.Report(Error);
			}
			catch
			{
				Error = Resource.FormatString("Err_NonClsException", "Publish.ProcessClientTlb");
				ComSoapPublishError.Report(Error);
			}
		}

		// Token: 0x060004F6 RID: 1270
		[DllImport("Fusion.dll", CharSet = CharSet.Auto)]
		internal static extern int CreateAssemblyCache(out IAssemblyCache ppAsmCache, uint dwReserved);

		// Token: 0x060004F7 RID: 1271
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern uint GetSystemDirectory(StringBuilder lpBuf, uint uSize);

		// Token: 0x060004F8 RID: 1272
		[DllImport("oleaut32.dll", CharSet = CharSet.Unicode)]
		internal static extern int LoadTypeLib([MarshalAs(UnmanagedType.LPWStr)] string file, out ITypeLib tlib);

		// Token: 0x060004F9 RID: 1273
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPWStr)] string filename);
	}
}
