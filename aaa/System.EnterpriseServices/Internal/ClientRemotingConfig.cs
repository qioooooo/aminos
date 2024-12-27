using System;
using System.Globalization;
using System.IO;
using System.Security.Permissions;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000D0 RID: 208
	public class ClientRemotingConfig
	{
		// Token: 0x060004C3 RID: 1219 RVA: 0x0000DF98 File Offset: 0x0000CF98
		public static bool Write(string DestinationDirectory, string VRoot, string BaseUrl, string AssemblyName, string TypeName, string ProgId, string Mode, string Transport)
		{
			bool flag;
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
				securityPermission.Demand();
				string text = "<configuration>\r\n";
				text += "  <system.runtime.remoting>\r\n";
				text += "    <application>\r\n";
				string text2 = BaseUrl;
				if (text2.Length > 0 && !text2.EndsWith("/", StringComparison.Ordinal))
				{
					text2 += "/";
				}
				text2 += VRoot;
				text = text + "      <client url=\"" + text2 + "\">\r\n";
				if (Mode.Length <= 0 || "WELLKNOWNOBJECT" == Mode.ToUpper(CultureInfo.InvariantCulture))
				{
					text += "        ";
					string text3 = text;
					text = string.Concat(new string[] { text3, "<wellknown type=\"", TypeName, ", ", AssemblyName, "\" url=\"", text2 });
					if (!text2.EndsWith("/", StringComparison.Ordinal))
					{
						text += "/";
					}
					text = text + ProgId + ".soap\" />\r\n";
				}
				else
				{
					text += "        ";
					string text4 = text;
					text = string.Concat(new string[] { text4, "<activated type=\"", TypeName, ", ", AssemblyName, "\"/>\r\n" });
				}
				text += "      </client>\r\n";
				text += "    </application>\r\n";
				text += "  </system.runtime.remoting>\r\n";
				text += "</configuration>\r\n";
				string text5 = DestinationDirectory;
				if (text5.Length > 0 && !text5.EndsWith("\\", StringComparison.Ordinal))
				{
					text5 += "\\";
				}
				text5 = text5 + TypeName + ".config";
				if (File.Exists(text5))
				{
					File.Delete(text5);
				}
				FileStream fileStream = new FileStream(text5, FileMode.Create);
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.Write(text);
				streamWriter.Close();
				fileStream.Close();
				flag = true;
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				flag = false;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "ClientRemotingConfig.Write"));
				flag = false;
			}
			return flag;
		}

		// Token: 0x040001FB RID: 507
		private const string indent = "  ";
	}
}
