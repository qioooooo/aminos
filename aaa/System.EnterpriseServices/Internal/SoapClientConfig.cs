using System;
using System.Globalization;
using System.IO;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000F3 RID: 243
	internal static class SoapClientConfig
	{
		// Token: 0x06000572 RID: 1394 RVA: 0x00013240 File Offset: 0x00012240
		internal static bool Write(string destinationDirectory, string fullUrl, string assemblyName, string typeName, string progId, string authentication)
		{
			string text = "<configuration>\r\n";
			text += "  <system.runtime.remoting>\r\n";
			text += "    <application>\r\n";
			text = text + "      <client url=\"" + fullUrl + "\">\r\n";
			text += "        ";
			string text2 = text;
			text = string.Concat(new string[] { text2, "<activated type=\"", typeName, ", ", assemblyName, "\"/>\r\n" });
			text += "      </client>\r\n";
			if (authentication.ToLower(CultureInfo.InvariantCulture) == "windows")
			{
				text += "      <channels>\r\n";
				text += "        <channel ref=\"http\" useDefaultCredentials=\"true\" />\r\n";
				text += "      </channels>\r\n";
			}
			text += "    </application>\r\n";
			text += "  </system.runtime.remoting>\r\n";
			text += "</configuration>\r\n";
			string text3 = destinationDirectory;
			if (text3.Length > 0 && !text3.EndsWith("\\", StringComparison.Ordinal))
			{
				text3 += "\\";
			}
			text3 = text3 + typeName + ".config";
			if (File.Exists(text3))
			{
				File.Delete(text3);
			}
			FileStream fileStream = new FileStream(text3, FileMode.Create);
			StreamWriter streamWriter = new StreamWriter(fileStream);
			streamWriter.Write(text);
			streamWriter.Close();
			fileStream.Close();
			return true;
		}
	}
}
