using System;
using System.IO;
using System.Text;
using System.Xml;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000F0 RID: 240
	internal static class SoapServerConfig
	{
		// Token: 0x06000566 RID: 1382 RVA: 0x00012ABC File Offset: 0x00011ABC
		internal static bool Create(string inFilePath, bool impersonate, bool windowsAuth)
		{
			string text = inFilePath;
			if (text.Length <= 0)
			{
				return false;
			}
			if (!text.EndsWith("/", StringComparison.Ordinal) && !text.EndsWith("\\", StringComparison.Ordinal))
			{
				text += "\\";
			}
			string text2 = text + "web.config";
			if (!File.Exists(text2))
			{
				XmlTextWriter xmlTextWriter = new XmlTextWriter(text2, new UTF8Encoding());
				xmlTextWriter.Formatting = Formatting.Indented;
				xmlTextWriter.WriteStartDocument();
				xmlTextWriter.WriteStartElement("configuration");
				xmlTextWriter.Flush();
				xmlTextWriter.Close();
			}
			return SoapServerConfig.ChangeSecuritySettings(text2, impersonate, windowsAuth);
		}

		// Token: 0x06000567 RID: 1383 RVA: 0x00012B4C File Offset: 0x00011B4C
		internal static XmlElement FindOrCreateElement(XmlDocument configXml, XmlNode node, string elemName)
		{
			XmlNodeList xmlNodeList = node.SelectNodes(elemName);
			XmlElement xmlElement2;
			if (xmlNodeList.Count == 0)
			{
				XmlElement xmlElement = configXml.CreateElement(elemName);
				node.AppendChild(xmlElement);
				xmlElement2 = xmlElement;
			}
			else
			{
				xmlElement2 = (XmlElement)xmlNodeList[0];
			}
			return xmlElement2;
		}

		// Token: 0x06000568 RID: 1384 RVA: 0x00012B90 File Offset: 0x00011B90
		internal static bool UpdateChannels(XmlDocument configXml)
		{
			XmlNode documentElement = configXml.DocumentElement;
			XmlElement xmlElement = SoapServerConfig.FindOrCreateElement(configXml, documentElement, "system.runtime.remoting");
			xmlElement = SoapServerConfig.FindOrCreateElement(configXml, xmlElement, "application");
			xmlElement = SoapServerConfig.FindOrCreateElement(configXml, xmlElement, "channels");
			xmlElement = SoapServerConfig.FindOrCreateElement(configXml, xmlElement, "channel");
			xmlElement.SetAttribute("ref", "http server");
			return true;
		}

		// Token: 0x06000569 RID: 1385 RVA: 0x00012BEC File Offset: 0x00011BEC
		internal static bool UpdateSystemWeb(XmlDocument configXml, bool impersonate, bool authentication)
		{
			XmlNode documentElement = configXml.DocumentElement;
			XmlElement xmlElement = SoapServerConfig.FindOrCreateElement(configXml, documentElement, "system.web");
			if (impersonate)
			{
				XmlElement xmlElement2 = SoapServerConfig.FindOrCreateElement(configXml, xmlElement, "identity");
				xmlElement2.SetAttribute("impersonate", "true");
			}
			if (authentication)
			{
				XmlElement xmlElement3 = SoapServerConfig.FindOrCreateElement(configXml, xmlElement, "authentication");
				xmlElement3.SetAttribute("mode", "Windows");
			}
			return true;
		}

		// Token: 0x0600056A RID: 1386 RVA: 0x00012C50 File Offset: 0x00011C50
		internal static bool ChangeSecuritySettings(string fileName, bool impersonate, bool authentication)
		{
			if (!File.Exists(fileName))
			{
				return false;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.Load(fileName);
			bool flag = SoapServerConfig.UpdateChannels(xmlDocument);
			if (flag)
			{
				flag = SoapServerConfig.UpdateSystemWeb(xmlDocument, impersonate, authentication);
				try
				{
					if (flag)
					{
						xmlDocument.Save(fileName);
					}
				}
				catch
				{
					string text = Resource.FormatString("Soap_WebConfigFailed");
					ComSoapPublishError.Report(text);
					throw;
				}
			}
			if (!flag)
			{
				string text2 = Resource.FormatString("Soap_WebConfigFailed");
				ComSoapPublishError.Report(text2);
			}
			return flag;
		}

		// Token: 0x0600056B RID: 1387 RVA: 0x00012CCC File Offset: 0x00011CCC
		internal static void AddComponent(string filePath, string assemblyName, string typeName, string progId, string assemblyFile, string wkoMode, bool wellKnown, bool clientActivated)
		{
			try
			{
				AssemblyManager assemblyManager = new AssemblyManager();
				string text = typeName + ", " + assemblyManager.GetFullName(assemblyFile, assemblyName);
				string text2 = typeName + ", " + assemblyName;
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filePath);
				XmlNode xmlNode = xmlDocument.DocumentElement;
				xmlNode = SoapServerConfig.FindOrCreateElement(xmlDocument, xmlNode, "system.runtime.remoting");
				xmlNode = SoapServerConfig.FindOrCreateElement(xmlDocument, xmlNode, "application");
				xmlNode = SoapServerConfig.FindOrCreateElement(xmlDocument, xmlNode, "service");
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text2 + "']");
				while (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					XmlNode xmlNode2 = xmlNodeList.Item(0);
					if (xmlNode2.ParentNode != null)
					{
						xmlNode2.ParentNode.RemoveChild(xmlNode2);
						xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text2 + "']");
					}
				}
				xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text + "']");
				while (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					XmlNode xmlNode3 = xmlNodeList.Item(0);
					if (xmlNode3.ParentNode != null)
					{
						xmlNode3.ParentNode.RemoveChild(xmlNode3);
						xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text + "']");
					}
				}
				if (wellKnown)
				{
					XmlElement xmlElement = xmlDocument.CreateElement("wellknown");
					xmlElement.SetAttribute("mode", wkoMode);
					xmlElement.SetAttribute("type", text);
					xmlElement.SetAttribute("objectUri", progId + ".soap");
					xmlNode.AppendChild(xmlElement);
				}
				if (clientActivated)
				{
					XmlElement xmlElement2 = xmlDocument.CreateElement("activated");
					xmlElement2.SetAttribute("type", text2);
					xmlNode.AppendChild(xmlElement2);
				}
				xmlDocument.Save(filePath);
			}
			catch (Exception ex)
			{
				string text3 = Resource.FormatString("Soap_ConfigAdditionFailure");
				ComSoapPublishError.Report(text3 + " " + ex.Message);
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "SoapServerConfig.AddComponent"));
				throw;
			}
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00012EFC File Offset: 0x00011EFC
		internal static void DeleteComponent(string filePath, string assemblyName, string typeName, string progId, string assemblyFile)
		{
			try
			{
				AssemblyManager assemblyManager = new AssemblyManager();
				string text = typeName + ", " + assemblyManager.GetFullName(assemblyFile, assemblyName);
				string text2 = typeName + ", " + assemblyName;
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(filePath);
				XmlNode xmlNode = xmlDocument.DocumentElement;
				xmlNode = SoapServerConfig.FindOrCreateElement(xmlDocument, xmlNode, "system.runtime.remoting");
				xmlNode = SoapServerConfig.FindOrCreateElement(xmlDocument, xmlNode, "application");
				xmlNode = SoapServerConfig.FindOrCreateElement(xmlDocument, xmlNode, "service");
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text2 + "']");
				while (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					XmlNode xmlNode2 = xmlNodeList.Item(0);
					if (xmlNode2.ParentNode != null)
					{
						xmlNode2.ParentNode.RemoveChild(xmlNode2);
						xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text2 + "']");
					}
				}
				xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text + "']");
				while (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					XmlNode xmlNode3 = xmlNodeList.Item(0);
					if (xmlNode3.ParentNode != null)
					{
						xmlNode3.ParentNode.RemoveChild(xmlNode3);
						xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text + "']");
					}
				}
				xmlDocument.Save(filePath);
			}
			catch (DirectoryNotFoundException)
			{
			}
			catch (FileNotFoundException)
			{
			}
			catch (RegistrationException)
			{
			}
			catch (Exception ex)
			{
				string text3 = Resource.FormatString("Soap_ConfigDeletionFailure");
				ComSoapPublishError.Report(text3 + " " + ex.Message);
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "SoapServerConfig.DeleteComponent"));
				throw;
			}
		}
	}
}
