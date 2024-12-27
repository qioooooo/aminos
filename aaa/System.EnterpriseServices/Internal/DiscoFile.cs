using System;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000D2 RID: 210
	internal class DiscoFile
	{
		// Token: 0x060004C7 RID: 1223 RVA: 0x0000E7A0 File Offset: 0x0000D7A0
		public void Create(string FilePath, string DiscoRef)
		{
			try
			{
				if (!FilePath.EndsWith("/", StringComparison.Ordinal) && !FilePath.EndsWith("\\", StringComparison.Ordinal))
				{
					FilePath += "\\";
				}
				if (!File.Exists(FilePath + DiscoRef))
				{
					SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
					securityPermission.Demand();
					string text = "<?xml version=\"1.0\" ?>\n";
					text += "<discovery xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://schemas.xmlsoap.org/disco/\">\n";
					text += "</discovery>\n";
					FileStream fileStream = new FileStream(FilePath + DiscoRef, FileMode.Create);
					StreamWriter streamWriter = new StreamWriter(fileStream);
					streamWriter.Write(text);
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
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "DiscoFile.Create"));
			}
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0000E888 File Offset: 0x0000D888
		internal void DeleteElement(string FilePath, string SoapPageRef)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
				securityPermission.Demand();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(FilePath);
				XmlNode xmlNode = xmlDocument.DocumentElement;
				while (xmlNode.Name != "discovery")
				{
					xmlNode = xmlNode.NextSibling;
				}
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::ref='" + SoapPageRef + "']");
				while (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					XmlNode xmlNode2 = xmlNodeList.Item(0);
					if (xmlNode2.ParentNode != null)
					{
						xmlNode2.ParentNode.RemoveChild(xmlNode2);
						xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::ref='" + SoapPageRef + "']");
					}
				}
				xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::address='" + SoapPageRef + "']");
				while (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					XmlNode xmlNode3 = xmlNodeList.Item(0);
					if (xmlNode3.ParentNode != null)
					{
						xmlNode3.ParentNode.RemoveChild(xmlNode3);
						xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::address='" + SoapPageRef + "']");
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
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "DiscoFile.DeleteElement"));
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x0000EA2C File Offset: 0x0000DA2C
		public void AddElement(string FilePath, string SoapPageRef)
		{
			try
			{
				SecurityPermission securityPermission = new SecurityPermission(SecurityPermissionFlag.RemotingConfiguration);
				securityPermission.Demand();
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.Load(FilePath);
				XmlNode xmlNode = xmlDocument.DocumentElement;
				while (xmlNode.Name != "discovery")
				{
					xmlNode = xmlNode.NextSibling;
				}
				XmlNodeList xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::ref='" + SoapPageRef + "']");
				while (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					XmlNode xmlNode2 = xmlNodeList.Item(0);
					if (xmlNode2.ParentNode != null)
					{
						xmlNode2.ParentNode.RemoveChild(xmlNode2);
						xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::ref='" + SoapPageRef + "']");
					}
				}
				xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::address='" + SoapPageRef + "']");
				while (xmlNodeList != null && xmlNodeList.Count > 0)
				{
					XmlNode xmlNode3 = xmlNodeList.Item(0);
					if (xmlNode3.ParentNode != null)
					{
						xmlNode3.ParentNode.RemoveChild(xmlNode3);
						xmlNodeList = xmlNode.SelectNodes("descendant::*[attribute::address='" + SoapPageRef + "']");
					}
				}
				XmlElement xmlElement = xmlDocument.CreateElement("", "contractRef", "");
				xmlElement.SetAttribute("ref", SoapPageRef);
				xmlElement.SetAttribute("docRef", SoapPageRef);
				xmlElement.SetAttribute("xmlns", "http://schemas.xmlsoap.org/disco/scl/");
				xmlNode.AppendChild(xmlElement);
				XmlElement xmlElement2 = xmlDocument.CreateElement("", "soap", "");
				xmlElement2.SetAttribute("address", SoapPageRef);
				xmlElement2.SetAttribute("xmlns", "http://schemas.xmlsoap.org/disco/soap/");
				xmlNode.AppendChild(xmlElement2);
				xmlDocument.Save(FilePath);
			}
			catch (Exception ex)
			{
				ComSoapPublishError.Report(ex.ToString());
			}
			catch
			{
				ComSoapPublishError.Report(Resource.FormatString("Err_NonClsException", "DiscoFile.AddElement"));
			}
		}
	}
}
