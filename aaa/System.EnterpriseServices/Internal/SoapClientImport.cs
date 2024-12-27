using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000F2 RID: 242
	[Guid("346D5B9F-45E1-45c0-AADF-1B7D221E9063")]
	public sealed class SoapClientImport : ISoapClientImport
	{
		// Token: 0x0600056E RID: 1390 RVA: 0x00013108 File Offset: 0x00012108
		public void ProcessClientTlbEx(string progId, string virtualRoot, string baseUrl, string authentication, string assemblyName, string typeName)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode);
				securityPermission.Demand();
			}
			catch (SecurityException)
			{
				ComSoapPublishError.Report(Resource.FormatString("Soap_SecurityFailure"));
				throw;
			}
			try
			{
				Platform.Assert(Platform.Whistler, "SoapClientImport.ProcessClientTlbEx");
				string clientPhysicalPath = SoapClientImport.GetClientPhysicalPath(true);
				if (progId.Length > 0)
				{
					Uri uri = new Uri(baseUrl);
					Uri uri2 = new Uri(uri, virtualRoot);
					string text = authentication;
					if (text.Length <= 0 && uri2.Scheme.ToLower(CultureInfo.InvariantCulture) == "https")
					{
						text = "Windows";
					}
					SoapClientConfig.Write(clientPhysicalPath, uri2.AbsoluteUri, assemblyName, typeName, progId, text);
				}
			}
			catch
			{
				string text2 = Resource.FormatString("Soap_ClientConfigAddFailure");
				ComSoapPublishError.Report(text2);
				throw;
			}
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000131E0 File Offset: 0x000121E0
		internal static string GetClientPhysicalPath(bool createDir)
		{
			uint num = 1024U;
			StringBuilder stringBuilder = new StringBuilder((int)num, (int)num);
			if (SoapClientImport.GetSystemDirectory(stringBuilder, num) == 0U)
			{
				throw new ServicedComponentException(Resource.FormatString("Soap_GetSystemDirectoryFailure"));
			}
			string text = stringBuilder.ToString() + "\\com\\SOAPAssembly\\";
			if (createDir && !Directory.Exists(text))
			{
				Directory.CreateDirectory(text);
			}
			return text;
		}

		// Token: 0x06000570 RID: 1392
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern uint GetSystemDirectory(StringBuilder lpBuf, uint uSize);
	}
}
