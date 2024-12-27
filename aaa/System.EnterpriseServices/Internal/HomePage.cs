using System;
using System.IO;
using System.Security.Permissions;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000D1 RID: 209
	internal class HomePage
	{
		// Token: 0x060004C5 RID: 1221 RVA: 0x0000E204 File Offset: 0x0000D204
		public void Create(string FilePath, string VirtualRoot, string PageName, string DiscoRef)
		{
			try
			{
				if (!FilePath.EndsWith("/", StringComparison.Ordinal) && !FilePath.EndsWith("\\", StringComparison.Ordinal))
				{
					FilePath += "\\";
				}
				if (!File.Exists(FilePath + PageName))
				{
					SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
					securityPermission.Demand();
					string text = FilePath + "web.config";
					string text2 = "<%@ Import Namespace=\"System.Collections\" %>\r\n";
					text2 += "<%@ Import Namespace=\"System.IO\" %>\r\n";
					text2 += "<%@ Import Namespace=\"System.Xml.Serialization\" %>\r\n";
					text2 += "<%@ Import Namespace=\"System.Xml\" %>\r\n";
					text2 += "<%@ Import Namespace=\"System.Xml.Schema\" %>\r\n";
					text2 += "<%@ Import Namespace=\"System.Web.Services.Description\" %>\r\n";
					text2 += "<%@ Import Namespace=\"System\" %>\r\n";
					text2 += "<%@ Import Namespace=\"System.Globalization\" %>\r\n";
					text2 += "<%@ Import Namespace=\"System.Resources\" %>\r\n";
					text2 += "<%@ Import Namespace=\"System.Diagnostics\" %>\r\n";
					text2 += "<html>\r\n";
					text2 += "<script language=\"C#\" runat=\"server\">\r\n";
					text2 += "    string soapNs = \"http://schemas.xmlsoap.org/soap/envelope/\";\r\n";
					text2 += "    string soapEncNs = \"http://schemas.xmlsoap.org/soap/encoding/\";\r\n";
					text2 += "    string urtNs = \"urn:schemas-microsoft-com:urt-types\";\r\n";
					text2 += "    string wsdlNs = \"http://schemas.xmlsoap.org/wsdl/\";\r\n";
					text2 = text2 + "    string VRoot = \"" + VirtualRoot + "\";\r\n";
					text2 += "    string ServiceName() { return VRoot; }\r\n";
					text2 += "\r\n";
					text2 += "   XmlNode GetNextNamedSiblingNode(XmlNode inNode, string name)\r\n";
					text2 += "    {\r\n";
					text2 += "       if (inNode == null ) return inNode;\r\n";
					text2 += "      if (inNode.Name == name) return inNode;\r\n";
					text2 += "       XmlNode newNode = inNode.NextSibling;\r\n";
					text2 += "       if (newNode == null) return newNode;\r\n";
					text2 += "       if (newNode.Name == name ) return newNode;\r\n";
					text2 += "       bool found = false;\r\n";
					text2 += "       while (!found)\r\n";
					text2 += "       {\r\n";
					text2 += "           XmlNode oldNode = newNode;\r\n";
					text2 += "           newNode = oldNode.NextSibling;\r\n";
					text2 += "           if (null == newNode || newNode == oldNode)\r\n";
					text2 += "           {\r\n";
					text2 += "               newNode = null;\r\n";
					text2 += "               break;\r\n";
					text2 += "           }\r\n";
					text2 += "           if (newNode.Name == name) found = true;\r\n";
					text2 += "       }\r\n";
					text2 += "       return newNode;\r\n";
					text2 += "   }\r\n";
					text2 += "\r\n";
					text2 += "   string GetNodes()\r\n";
					text2 += "   {\r\n";
					text2 += "       string retval = \"\";\r\n";
					text2 += "       XmlDocument configXml = new XmlDocument();\r\n";
					text2 = text2 + "      configXml.Load(@\"" + text + "\");\r\n";
					text2 += "       XmlNode node= configXml.DocumentElement;\r\n";
					text2 += "        node = GetNextNamedSiblingNode(node,\"configuration\");\r\n";
					text2 += "        node = GetNextNamedSiblingNode(node.FirstChild, \"system.runtime.remoting\");\r\n";
					text2 += "        node = GetNextNamedSiblingNode(node.FirstChild, \"application\");\r\n";
					text2 += "        node = GetNextNamedSiblingNode(node.FirstChild, \"service\");\r\n";
					text2 += "        node = GetNextNamedSiblingNode(node.FirstChild, \"wellknown\");\r\n";
					text2 += "       while (node != null)\r\n";
					text2 += "       {\r\n";
					text2 += "           XmlNode attribType = node.Attributes.GetNamedItem(\"objectUri\");\r\n";
					text2 += "           retval += \"<a href=\" + attribType.Value + \"?WSDL>\" + attribType.Value +\"?WSDL</a><br><br>\";\r\n";
					text2 += "           node = GetNextNamedSiblingNode(node.NextSibling, \"wellknown\");\r\n";
					text2 += "       }\r\n";
					text2 += "        return retval;\r\n";
					text2 += "    }\r\n";
					text2 += "\r\n";
					text2 += "</script>\r\n";
					text2 += "<title><% = ServiceName() %></title>\r\n";
					text2 += "<head>\r\n";
					text2 = text2 + "<link type='text/xml' rel='alternate' href='" + DiscoRef + "' />\r\n";
					text2 += "\r\n";
					text2 += "   <style type=\"text/css\">\r\n";
					text2 += " \r\n";
					text2 += "       BODY { color: #000000; background-color: white; font-family: \"Verdana\"; margin-left: 0px; margin-top: 0px; }\r\n";
					text2 += "       #content { margin-left: 30px; font-size: .70em; padding-bottom: 2em; }\r\n";
					text2 += "       A:link { color: #336699; font-weight: bold; text-decoration: underline; }\r\n";
					text2 += "       A:visited { color: #6699cc; font-weight: bold; text-decoration: underline; }\r\n";
					text2 += "       A:active { color: #336699; font-weight: bold; text-decoration: underline; }\r\n";
					text2 += "       A:hover { color: cc3300; font-weight: bold; text-decoration: underline; }\r\n";
					text2 += "       P { color: #000000; margin-top: 0px; margin-bottom: 12px; font-family: \"Verdana\"; }\r\n";
					text2 += "       pre { background-color: #e5e5cc; padding: 5px; font-family: \"Courier New\"; font-size: x-small; margin-top: -5px; border: 1px #f0f0e0 solid; }\r\n";
					text2 += "       td { color: #000000; font-family: verdana; font-size: .7em; }\r\n";
					text2 += "       h2 { font-size: 1.5em; font-weight: bold; margin-top: 25px; margin-bottom: 10px; border-top: 1px solid #003366; margin-left: -15px; color: #003366; }\r\n";
					text2 += "       h3 { font-size: 1.1em; color: #000000; margin-left: -15px; margin-top: 10px; margin-bottom: 10px; }\r\n";
					text2 += "       ul, ol { margin-top: 10px; margin-left: 20px; }\r\n";
					text2 += "       li { margin-top: 10px; color: #000000; }\r\n";
					text2 += "       font.value { color: darkblue; font: bold; }\r\n";
					text2 += "       font.key { color: darkgreen; font: bold; }\r\n";
					text2 += "       .heading1 { color: #ffffff; font-family: \"Tahoma\"; font-size: 26px; font-weight: normal; background-color: #003366; margin-top: 0px; margin-bottom: 0px; margin-left: 0px; padding-top: 10px; padding-bottom: 3px; padding-left: 15px; width: 105%; }\r\n";
					text2 += "       .button { background-color: #dcdcdc; font-family: \"Verdana\"; font-size: 1em; border-top: #cccccc 1px solid; border-bottom: #666666 1px solid; border-left: #cccccc 1px solid; border-right: #666666 1px solid; }\r\n";
					text2 += "       .frmheader { color: #000000; background: #dcdcdc; font-family: \"Verdana\"; font-size: .7em; font-weight: normal; border-bottom: 1px solid #dcdcdc; padding-top: 2px; padding-bottom: 2px; }\r\n";
					text2 += "       .frmtext { font-family: \"Verdana\"; font-size: .7em; margin-top: 8px; margin-bottom: 0px; margin-left: 32px; }\r\n";
					text2 += "       .frmInput { font-family: \"Verdana\"; font-size: 1em; }\r\n";
					text2 += "       .intro { margin-left: -15px; }\r\n";
					text2 += " \r\n";
					text2 += "    </style>\r\n";
					text2 += "\r\n";
					text2 += "</head>\r\n";
					text2 += "<body>\r\n";
					text2 += "<p class=\"heading1\"><% = ServiceName() %></p><br>\r\n";
					text2 += "<% = GetNodes() %>\r\n";
					text2 += "</body>\r\n";
					text2 += "</html>\r\n";
					FileStream fileStream = new FileStream(FilePath + PageName, FileMode.Create);
					StreamWriter streamWriter = new StreamWriter(fileStream);
					streamWriter.Write(text2);
					streamWriter.Close();
					fileStream.Close();
				}
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "HomePage.Create"));
			}
		}
	}
}
