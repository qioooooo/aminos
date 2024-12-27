using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000F5 RID: 245
	[Guid("5F9A955F-AA55-4127-A32B-33496AA8A44E")]
	public sealed class SoapUtility : ISoapUtility
	{
		// Token: 0x06000576 RID: 1398 RVA: 0x000133A0 File Offset: 0x000123A0
		public void GetServerPhysicalPath(string rootWebServer, string inBaseUrl, string inVirtualRoot, out string physicalPath)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				Platform.Assert(Platform.Whistler, "SoapUtility.GetServerPhysicalPath");
				physicalPath = SoapServerInfo.ServerPhysicalPath(rootWebServer, inBaseUrl, inVirtualRoot, false);
			}
			catch (SecurityException)
			{
				ComSoapPublishError.Report(Resource.FormatString("Soap_SecurityFailure"));
				throw;
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x000133FC File Offset: 0x000123FC
		public void GetServerBinPath(string rootWebServer, string inBaseUrl, string inVirtualRoot, out string binPath)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				Platform.Assert(Platform.Whistler, "SoapUtility.GetServerBinPath");
				binPath = SoapServerInfo.ServerPhysicalPath(rootWebServer, inBaseUrl, inVirtualRoot, false) + "\\bin\\";
			}
			catch (SecurityException)
			{
				ComSoapPublishError.Report(Resource.FormatString("Soap_SecurityFailure"));
				throw;
			}
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00013460 File Offset: 0x00012460
		public void Present()
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
				Platform.Assert(Platform.Whistler, "SoapUtility.Present");
			}
			catch (SecurityException)
			{
				ComSoapPublishError.Report(Resource.FormatString("Soap_SecurityFailure"));
				throw;
			}
		}
	}
}
