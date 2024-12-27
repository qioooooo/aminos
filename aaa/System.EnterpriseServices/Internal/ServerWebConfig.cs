using System;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000D3 RID: 211
	public class ServerWebConfig : IServerWebConfig
	{
		// Token: 0x060004CB RID: 1227 RVA: 0x0000EC28 File Offset: 0x0000DC28
		public void Create(string FilePath, string FilePrefix, out string Error)
		{
			Error = "";
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
				securityPermission.Demand();
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
				throw;
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "ServerWebConfig.Create"));
				throw;
			}
			if (!FilePath.EndsWith("/", StringComparison.Ordinal) && !FilePath.EndsWith("\\", StringComparison.Ordinal))
			{
				FilePath += "\\";
			}
			if (File.Exists(FilePath + FilePrefix + ".config"))
			{
				return;
			}
			this.webconfig = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\r\n";
			this.webconfig += "<configuration>\r\n";
			this.webconfig += "  <system.runtime.remoting>\r\n";
			this.webconfig += "    <application>\r\n";
			this.webconfig += "      <service>\r\n";
			this.webconfig += "      </service>\r\n";
			this.webconfig += "    </application>\r\n";
			this.webconfig += "  </system.runtime.remoting>\r\n";
			this.webconfig += "</configuration>\r\n";
			if (!this.WriteFile(FilePath, FilePrefix, ".config"))
			{
				Error = Resource.FormatString("Soap_WebConfigFailed");
				ComSoapPublishError.Report(Error);
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0000EDAC File Offset: 0x0000DDAC
		public void AddElement(string FilePath, string AssemblyName, string TypeName, string ProgId, string WkoMode, out string Error)
		{
			Error = "";
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
				securityPermission.Demand();
				string text = TypeName + ", " + AssemblyName;
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(FilePath);
				XmlNode xmlNode = xmlDocument.DocumentElement;
				while (xmlNode.Name != "configuration")
				{
					xmlNode = xmlNode.NextSibling;
				}
				xmlNode = xmlNode.FirstChild;
				while (xmlNode.Name != "system.runtime.remoting")
				{
					xmlNode = xmlNode.NextSibling;
				}
				xmlNode = xmlNode.FirstChild;
				while (xmlNode.Name != "application")
				{
					xmlNode = xmlNode.NextSibling;
				}
				xmlNode = xmlNode.FirstChild;
				while (xmlNode.Name != "service")
				{
					xmlNode = xmlNode.NextSibling;
				}
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text + "']");
				while (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					XmlNode xmlNode2 = xmlNodeList.Item(0);
					if (xmlNode2.ParentNode != null)
					{
						xmlNode2.ParentNode.RemoveChild(xmlNode2);
						xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::type='" + text + "']");
					}
				}
				XmlElement xmlElement = xmlDocument.CreateElement("", "wellknown", "");
				xmlElement.SetAttribute("mode", WkoMode);
				xmlElement.SetAttribute("type", text);
				xmlElement.SetAttribute("objectUri", ProgId + ".soap");
				xmlNode.AppendChild(xmlElement);
				XmlElement xmlElement2 = xmlDocument.CreateElement("", "activated", "");
				xmlElement2.SetAttribute("type", text);
				xmlNode.AppendChild(xmlElement2);
				xmlDocument.Save(FilePath);
			}
			catch (Exception ex)
			{
				Error = ex.ToString();
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "ServerWebConfig.AddElement"));
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0000EFC4 File Offset: 0x0000DFC4
		internal void AddGacElement(string FilePath, string AssemblyName, string TypeName, string ProgId, string WkoMode, string AssemblyFile)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
				securityPermission.Demand();
				AssemblyManager assemblyManager = new AssemblyManager();
				string text = TypeName + ", " + assemblyManager.GetFullName(AssemblyFile, AssemblyName);
				string text2 = TypeName + ", " + AssemblyName;
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(FilePath);
				XmlNode xmlNode = xmlDocument.DocumentElement;
				while (xmlNode.Name != "configuration")
				{
					xmlNode = xmlNode.NextSibling;
				}
				xmlNode = xmlNode.FirstChild;
				while (xmlNode.Name != "system.runtime.remoting")
				{
					xmlNode = xmlNode.NextSibling;
				}
				xmlNode = xmlNode.FirstChild;
				while (xmlNode.Name != "application")
				{
					xmlNode = xmlNode.NextSibling;
				}
				xmlNode = xmlNode.FirstChild;
				while (xmlNode.Name != "service")
				{
					xmlNode = xmlNode.NextSibling;
				}
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
				XmlElement xmlElement = xmlDocument.CreateElement("", "wellknown", "");
				xmlElement.SetAttribute("mode", WkoMode);
				xmlElement.SetAttribute("type", text);
				xmlElement.SetAttribute("objectUri", ProgId + ".soap");
				xmlNode.AppendChild(xmlElement);
				XmlElement xmlElement2 = xmlDocument.CreateElement("", "activated", "");
				xmlElement2.SetAttribute("type", text2);
				xmlNode.AppendChild(xmlElement2);
				xmlDocument.Save(FilePath);
			}
			catch (RegistrationException)
			{
				throw;
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "ServerWebConfig.AddGacElement"));
			}
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0000F284 File Offset: 0x0000E284
		internal void DeleteElement(string FilePath, string AssemblyName, string TypeName, string ProgId, string WkoMode, string AssemblyFile)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
				securityPermission.Demand();
				AssemblyManager assemblyManager = new AssemblyManager();
				string text = TypeName + ", " + assemblyManager.GetFullName(AssemblyFile, AssemblyName);
				string text2 = TypeName + ", " + AssemblyName;
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(FilePath);
				XmlNode xmlNode = xmlDocument.DocumentElement;
				while (xmlNode.Name != "configuration")
				{
					xmlNode = xmlNode.NextSibling;
				}
				xmlNode = xmlNode.FirstChild;
				while (xmlNode.Name != "system.runtime.remoting")
				{
					xmlNode = xmlNode.NextSibling;
				}
				xmlNode = xmlNode.FirstChild;
				while (xmlNode.Name != "application")
				{
					xmlNode = xmlNode.NextSibling;
				}
				xmlNode = xmlNode.FirstChild;
				while (xmlNode.Name != "service")
				{
					xmlNode = xmlNode.NextSibling;
				}
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
				xmlDocument.Save(FilePath);
			}
			catch (DirectoryNotFoundException)
			{
			}
			catch (FileNotFoundException)
			{
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "ServerWebConfig.DeleteElement"));
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0000F4D8 File Offset: 0x0000E4D8
		private bool WriteFile(string PhysicalDirectory, string FilePrefix, string FileSuffix)
		{
			bool flag;
			try
			{
				string text = PhysicalDirectory + FilePrefix + FileSuffix;
				if (File.Exists(text))
				{
					File.Delete(text);
				}
				FileStream fileStream = new FileStream(text, FileMode.Create);
				StreamWriter streamWriter = new StreamWriter(fileStream);
				streamWriter.Write(this.webconfig);
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
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "ServerWebConfig.WriteFile"));
				flag = false;
			}
			return flag;
		}

		// Token: 0x040001FC RID: 508
		private const string indent = "  ";

		// Token: 0x040001FD RID: 509
		private string webconfig = "";
	}
}
