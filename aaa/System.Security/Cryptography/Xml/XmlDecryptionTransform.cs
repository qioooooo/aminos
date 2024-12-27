using System;
using System.Collections;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000B6 RID: 182
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class XmlDecryptionTransform : Transform
	{
		// Token: 0x06000421 RID: 1057 RVA: 0x00015734 File Offset: 0x00014734
		public XmlDecryptionTransform()
		{
			base.Algorithm = "http://www.w3.org/2002/07/decrypt#XML";
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000422 RID: 1058 RVA: 0x00015795 File Offset: 0x00014795
		private ArrayList ExceptUris
		{
			get
			{
				if (this.m_arrayListUri == null)
				{
					this.m_arrayListUri = new ArrayList();
				}
				return this.m_arrayListUri;
			}
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x000157B0 File Offset: 0x000147B0
		protected virtual bool IsTargetElement(XmlElement inputElement, string idValue)
		{
			return inputElement != null && (inputElement.GetAttribute("Id") == idValue || inputElement.GetAttribute("id") == idValue || inputElement.GetAttribute("ID") == idValue);
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000424 RID: 1060 RVA: 0x00015800 File Offset: 0x00014800
		// (set) Token: 0x06000425 RID: 1061 RVA: 0x00015865 File Offset: 0x00014865
		public EncryptedXml EncryptedXml
		{
			get
			{
				if (this.m_exml != null)
				{
					return this.m_exml;
				}
				Reference reference = base.Reference;
				SignedXml signedXml = ((reference == null) ? base.SignedXml : reference.SignedXml);
				if (signedXml == null || signedXml.EncryptedXml == null)
				{
					this.m_exml = new EncryptedXml(this.m_containingDocument);
				}
				else
				{
					this.m_exml = signedXml.EncryptedXml;
				}
				return this.m_exml;
			}
			set
			{
				this.m_exml = value;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000426 RID: 1062 RVA: 0x0001586E File Offset: 0x0001486E
		public override Type[] InputTypes
		{
			get
			{
				return this.m_inputTypes;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000427 RID: 1063 RVA: 0x00015876 File Offset: 0x00014876
		public override Type[] OutputTypes
		{
			get
			{
				return this.m_outputTypes;
			}
		}

		// Token: 0x06000428 RID: 1064 RVA: 0x0001587E File Offset: 0x0001487E
		public void AddExceptUri(string uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			this.ExceptUris.Add(uri);
		}

		// Token: 0x06000429 RID: 1065 RVA: 0x0001589C File Offset: 0x0001489C
		public override void LoadInnerXml(XmlNodeList nodeList)
		{
			if (nodeList == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
			}
			this.ExceptUris.Clear();
			foreach (object obj in nodeList)
			{
				XmlNode xmlNode = (XmlNode)obj;
				XmlElement xmlElement = xmlNode as XmlElement;
				if (xmlElement != null)
				{
					if (xmlElement.LocalName == "Except" && xmlElement.NamespaceURI == "http://www.w3.org/2002/07/decrypt#")
					{
						string attribute = Utils.GetAttribute(xmlElement, "URI", "http://www.w3.org/2002/07/decrypt#");
						if (attribute == null || attribute.Length == 0 || attribute[0] != '#')
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UriRequired"));
						}
						if (!Utils.VerifyAttributes(xmlElement, "URI"))
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
						}
						string text = Utils.ExtractIdFromLocalUri(attribute);
						this.ExceptUris.Add(text);
					}
					else if (!Utils.GetAllowAdditionalSignatureNodes())
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
					}
				}
			}
		}

		// Token: 0x0600042A RID: 1066 RVA: 0x000159C8 File Offset: 0x000149C8
		protected override XmlNodeList GetInnerXml()
		{
			if (this.ExceptUris.Count == 0)
			{
				return null;
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("Transform", "http://www.w3.org/2000/09/xmldsig#");
			if (!string.IsNullOrEmpty(base.Algorithm))
			{
				xmlElement.SetAttribute("Algorithm", base.Algorithm);
			}
			foreach (object obj in this.ExceptUris)
			{
				string text = (string)obj;
				XmlElement xmlElement2 = xmlDocument.CreateElement("Except", "http://www.w3.org/2002/07/decrypt#");
				xmlElement2.SetAttribute("URI", text);
				xmlElement.AppendChild(xmlElement2);
			}
			return xmlElement.ChildNodes;
		}

		// Token: 0x0600042B RID: 1067 RVA: 0x00015A90 File Offset: 0x00014A90
		public override void LoadInput(object obj)
		{
			if (obj is Stream)
			{
				this.LoadStreamInput((Stream)obj);
				return;
			}
			if (obj is XmlDocument)
			{
				this.LoadXmlDocumentInput((XmlDocument)obj);
			}
		}

		// Token: 0x0600042C RID: 1068 RVA: 0x00015ABC File Offset: 0x00014ABC
		private void LoadStreamInput(Stream stream)
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.PreserveWhitespace = true;
			XmlResolver xmlResolver = (base.ResolverSet ? this.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), base.BaseURI));
			XmlReader xmlReader = Utils.PreProcessStreamInput(stream, xmlResolver, base.BaseURI);
			xmlDocument.Load(xmlReader);
			this.m_containingDocument = xmlDocument;
			this.m_nsm = new XmlNamespaceManager(this.m_containingDocument.NameTable);
			this.m_nsm.AddNamespace("enc", "http://www.w3.org/2001/04/xmlenc#");
			this.m_encryptedDataList = xmlDocument.SelectNodes("//enc:EncryptedData", this.m_nsm);
		}

		// Token: 0x0600042D RID: 1069 RVA: 0x00015B58 File Offset: 0x00014B58
		private void LoadXmlDocumentInput(XmlDocument document)
		{
			if (document == null)
			{
				throw new ArgumentNullException("document");
			}
			this.m_containingDocument = document;
			this.m_nsm = new XmlNamespaceManager(document.NameTable);
			this.m_nsm.AddNamespace("enc", "http://www.w3.org/2001/04/xmlenc#");
			this.m_encryptedDataList = document.SelectNodes("//enc:EncryptedData", this.m_nsm);
		}

		// Token: 0x0600042E RID: 1070 RVA: 0x00015BB8 File Offset: 0x00014BB8
		private bool ProcessEncryptedDataItem(XmlElement encryptedDataElement)
		{
			if (this.ExceptUris.Count > 0)
			{
				for (int i = 0; i < this.ExceptUris.Count; i++)
				{
					if (this.IsTargetElement(encryptedDataElement, (string)this.ExceptUris[i]))
					{
						return false;
					}
				}
			}
			EncryptedData encryptedData = new EncryptedData();
			encryptedData.LoadXml(encryptedDataElement);
			SymmetricAlgorithm decryptionKey = this.EncryptedXml.GetDecryptionKey(encryptedData, null);
			if (decryptionKey == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_MissingDecryptionKey"));
			}
			byte[] array = this.EncryptedXml.DecryptData(encryptedData, decryptionKey);
			this.EncryptedXml.ReplaceData(encryptedDataElement, array);
			return true;
		}

		// Token: 0x0600042F RID: 1071 RVA: 0x00015C50 File Offset: 0x00014C50
		private void ProcessElementRecursively(XmlNodeList encryptedDatas)
		{
			if (encryptedDatas == null || encryptedDatas.Count == 0)
			{
				return;
			}
			Queue queue = new Queue();
			foreach (object obj in encryptedDatas)
			{
				XmlNode xmlNode = (XmlNode)obj;
				queue.Enqueue(xmlNode);
			}
			for (XmlNode xmlNode2 = queue.Dequeue() as XmlNode; xmlNode2 != null; xmlNode2 = queue.Dequeue() as XmlNode)
			{
				XmlElement xmlElement = xmlNode2 as XmlElement;
				if (xmlElement != null && xmlElement.LocalName == "EncryptedData" && xmlElement.NamespaceURI == "http://www.w3.org/2001/04/xmlenc#")
				{
					XmlNode nextSibling = xmlElement.NextSibling;
					XmlNode parentNode = xmlElement.ParentNode;
					if (this.ProcessEncryptedDataItem(xmlElement))
					{
						XmlNode xmlNode3 = parentNode.FirstChild;
						while (xmlNode3 != null && xmlNode3.NextSibling != nextSibling)
						{
							xmlNode3 = xmlNode3.NextSibling;
						}
						if (xmlNode3 != null)
						{
							XmlNodeList xmlNodeList = xmlNode3.SelectNodes("//enc:EncryptedData", this.m_nsm);
							if (xmlNodeList.Count > 0)
							{
								foreach (object obj2 in xmlNodeList)
								{
									XmlNode xmlNode4 = (XmlNode)obj2;
									queue.Enqueue(xmlNode4);
								}
							}
						}
					}
				}
				if (queue.Count == 0)
				{
					return;
				}
			}
		}

		// Token: 0x06000430 RID: 1072 RVA: 0x00015DD0 File Offset: 0x00014DD0
		public override object GetOutput()
		{
			if (this.m_encryptedDataList != null)
			{
				this.ProcessElementRecursively(this.m_encryptedDataList);
			}
			Utils.AddNamespaces(this.m_containingDocument.DocumentElement, base.PropagatedNamespaces);
			return this.m_containingDocument;
		}

		// Token: 0x06000431 RID: 1073 RVA: 0x00015E02 File Offset: 0x00014E02
		public override object GetOutput(Type type)
		{
			if (type == typeof(XmlDocument))
			{
				return (XmlDocument)this.GetOutput();
			}
			throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"), "type");
		}

		// Token: 0x04000580 RID: 1408
		private const string XmlDecryptionTransformNamespaceUrl = "http://www.w3.org/2002/07/decrypt#";

		// Token: 0x04000581 RID: 1409
		private Type[] m_inputTypes = new Type[]
		{
			typeof(Stream),
			typeof(XmlDocument)
		};

		// Token: 0x04000582 RID: 1410
		private Type[] m_outputTypes = new Type[] { typeof(XmlDocument) };

		// Token: 0x04000583 RID: 1411
		private XmlNodeList m_encryptedDataList;

		// Token: 0x04000584 RID: 1412
		private ArrayList m_arrayListUri;

		// Token: 0x04000585 RID: 1413
		private EncryptedXml m_exml;

		// Token: 0x04000586 RID: 1414
		private XmlDocument m_containingDocument;

		// Token: 0x04000587 RID: 1415
		private XmlNamespaceManager m_nsm;
	}
}
