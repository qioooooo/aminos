using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000EE RID: 238
	[Guid("CAA817CC-0C04-4d22-A05C-2B7E162F4E8F")]
	public sealed class SoapServerVRoot : ISoapServerVRoot
	{
		// Token: 0x0600055F RID: 1375 RVA: 0x000123F0 File Offset: 0x000113F0
		public void CreateVirtualRootEx(string rootWebServer, string inBaseUrl, string inVirtualRoot, string homePage, string discoFile, string secureSockets, string authentication, string operation, out string baseUrl, out string virtualRoot, out string physicalPath)
		{
			baseUrl = "";
			virtualRoot = "";
			physicalPath = "";
			bool flag = true;
			bool flag2 = true;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = true;
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				Platform.Assert(Platform.Whistler, "SoapServerVRoot.CreateVirtualRootEx");
				if (inBaseUrl.Length > 0 || inVirtualRoot.Length > 0)
				{
					string text = "IIS://localhost/W3SVC/1/ROOT";
					if (rootWebServer.Length > 0)
					{
						text = rootWebServer;
					}
					if (authentication.ToLower(CultureInfo.InvariantCulture) == "anonymous")
					{
						flag3 = true;
						flag2 = false;
						flag6 = false;
					}
					flag4 = SoapServerInfo.BoolFromString(discoFile, flag4);
					flag5 = SoapServerInfo.BoolFromString(homePage, flag5);
					flag = SoapServerInfo.BoolFromString(secureSockets, flag);
					string text2 = "https";
					if (!flag)
					{
						text2 = "http";
					}
					SoapServerInfo.CheckUrl(inBaseUrl, inVirtualRoot, text2);
					SoapServerInfo.ParseUrl(inBaseUrl, inVirtualRoot, text2, out baseUrl, out virtualRoot);
					physicalPath = SoapServerInfo.ServerPhysicalPath(text, inBaseUrl, inVirtualRoot, true);
					SoapServerConfig.Create(physicalPath, flag6, flag2);
					if (flag4)
					{
						DiscoFile discoFile2 = new DiscoFile();
						discoFile2.Create(physicalPath, "Default.disco");
					}
					else if (File.Exists(physicalPath + "\\Default.disco"))
					{
						File.Delete(physicalPath + "\\Default.disco");
					}
					if (flag5)
					{
						HomePage homePage2 = new HomePage();
						string text3 = "";
						if (flag4)
						{
							text3 = "Default.disco";
						}
						homePage2.Create(physicalPath, virtualRoot, "Default.aspx", text3);
					}
					else if (File.Exists(physicalPath + "\\Default.aspx"))
					{
						File.Delete(physicalPath + "\\Default.aspx");
					}
					IISVirtualRootEx.CreateOrModify(text, physicalPath, virtualRoot, flag, flag2, flag3, flag5);
				}
			}
			catch
			{
				string text4 = Resource.FormatString("Soap_VRootCreationFailed");
				ComSoapPublishError.Report(text4 + " " + virtualRoot);
				throw;
			}
		}

		// Token: 0x06000560 RID: 1376 RVA: 0x000125D4 File Offset: 0x000115D4
		public void DeleteVirtualRootEx(string rootWebServer, string inBaseUrl, string inVirtualRoot)
		{
			try
			{
				try
				{
					SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
					securityPermission.Demand();
				}
				catch (SecurityException)
				{
					string text = Resource.FormatString("Soap_SecurityFailure");
					ComSoapPublishError.Report(text);
					throw;
				}
				Platform.Assert(Platform.Whistler, "SoapServerVRoot.DeleteVirtualRootEx");
				if (inBaseUrl.Length > 0 || inVirtualRoot.Length > 0)
				{
					int length = rootWebServer.Length;
					string text2 = "";
					string text3 = "";
					string text4 = "";
					SoapServerInfo.ParseUrl(inBaseUrl, inVirtualRoot, text2, out text3, out text4);
				}
			}
			catch
			{
				string text5 = Resource.FormatString("Soap_VRootDirectoryDeletionFailed");
				ComSoapPublishError.Report(text5);
				throw;
			}
		}

		// Token: 0x06000561 RID: 1377 RVA: 0x00012684 File Offset: 0x00011684
		public void GetVirtualRootStatus(string RootWebServer, string inBaseUrl, string inVirtualRoot, out string Exists, out string SSL, out string WindowsAuth, out string Anonymous, out string HomePage, out string DiscoFile, out string PhysicalPath, out string BaseUrl, out string VirtualRoot)
		{
			string text = "IIS://localhost/W3SVC/1/ROOT";
			if (RootWebServer.Length > 0)
			{
				text = RootWebServer;
			}
			Exists = "false";
			SSL = "false";
			WindowsAuth = "false";
			Anonymous = "false";
			HomePage = "false";
			DiscoFile = "false";
			SoapServerInfo.ParseUrl(inBaseUrl, inVirtualRoot, "http", out BaseUrl, out VirtualRoot);
			PhysicalPath = SoapServerInfo.ServerPhysicalPath(text, BaseUrl, VirtualRoot, false);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			IISVirtualRootEx.GetStatus(text, PhysicalPath, VirtualRoot, out flag, out flag2, out flag3, out flag4, out flag5, out flag6);
			if (flag)
			{
				Exists = "true";
			}
			if (flag2)
			{
				SSL = "true";
				SoapServerInfo.ParseUrl(inBaseUrl, inVirtualRoot, "https", out BaseUrl, out VirtualRoot);
			}
			if (flag3)
			{
				WindowsAuth = "true";
			}
			if (flag4)
			{
				Anonymous = "true";
			}
			if (flag5)
			{
				HomePage = "true";
			}
			if (flag6)
			{
				DiscoFile = "true";
			}
		}
	}
}
