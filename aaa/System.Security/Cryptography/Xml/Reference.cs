using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000A6 RID: 166
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class Reference
	{
		// Token: 0x06000337 RID: 823 RVA: 0x00010B56 File Offset: 0x0000FB56
		public Reference()
		{
			this.m_transformChain = new TransformChain();
			this.m_refTarget = null;
			this.m_refTargetType = ReferenceTargetType.UriReference;
			this.m_cachedXml = null;
			this.m_digestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00010B89 File Offset: 0x0000FB89
		public Reference(Stream stream)
		{
			this.m_transformChain = new TransformChain();
			this.m_refTarget = stream;
			this.m_refTargetType = ReferenceTargetType.Stream;
			this.m_cachedXml = null;
			this.m_digestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00010BBC File Offset: 0x0000FBBC
		public Reference(string uri)
		{
			this.m_transformChain = new TransformChain();
			this.m_refTarget = uri;
			this.m_uri = uri;
			this.m_refTargetType = ReferenceTargetType.UriReference;
			this.m_cachedXml = null;
			this.m_digestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00010BF6 File Offset: 0x0000FBF6
		internal Reference(XmlElement element)
		{
			this.m_transformChain = new TransformChain();
			this.m_refTarget = element;
			this.m_refTargetType = ReferenceTargetType.XmlElement;
			this.m_cachedXml = null;
			this.m_digestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x0600033B RID: 827 RVA: 0x00010C29 File Offset: 0x0000FC29
		// (set) Token: 0x0600033C RID: 828 RVA: 0x00010C31 File Offset: 0x0000FC31
		public string Id
		{
			get
			{
				return this.m_id;
			}
			set
			{
				this.m_id = value;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x0600033D RID: 829 RVA: 0x00010C3A File Offset: 0x0000FC3A
		// (set) Token: 0x0600033E RID: 830 RVA: 0x00010C42 File Offset: 0x0000FC42
		public string Uri
		{
			get
			{
				return this.m_uri;
			}
			set
			{
				this.m_uri = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x0600033F RID: 831 RVA: 0x00010C52 File Offset: 0x0000FC52
		// (set) Token: 0x06000340 RID: 832 RVA: 0x00010C5A File Offset: 0x0000FC5A
		public string Type
		{
			get
			{
				return this.m_type;
			}
			set
			{
				this.m_type = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x06000341 RID: 833 RVA: 0x00010C6A File Offset: 0x0000FC6A
		// (set) Token: 0x06000342 RID: 834 RVA: 0x00010C72 File Offset: 0x0000FC72
		public string DigestMethod
		{
			get
			{
				return this.m_digestMethod;
			}
			set
			{
				this.m_digestMethod = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x06000343 RID: 835 RVA: 0x00010C82 File Offset: 0x0000FC82
		// (set) Token: 0x06000344 RID: 836 RVA: 0x00010C8A File Offset: 0x0000FC8A
		public byte[] DigestValue
		{
			get
			{
				return this.m_digestValue;
			}
			set
			{
				this.m_digestValue = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x06000345 RID: 837 RVA: 0x00010C9A File Offset: 0x0000FC9A
		// (set) Token: 0x06000346 RID: 838 RVA: 0x00010CB5 File Offset: 0x0000FCB5
		public TransformChain TransformChain
		{
			get
			{
				if (this.m_transformChain == null)
				{
					this.m_transformChain = new TransformChain();
				}
				return this.m_transformChain;
			}
			[ComVisible(false)]
			set
			{
				this.m_transformChain = value;
				this.m_cachedXml = null;
			}
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x06000347 RID: 839 RVA: 0x00010CC5 File Offset: 0x0000FCC5
		internal bool CacheValid
		{
			get
			{
				return this.m_cachedXml != null;
			}
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x06000348 RID: 840 RVA: 0x00010CD3 File Offset: 0x0000FCD3
		// (set) Token: 0x06000349 RID: 841 RVA: 0x00010CDB File Offset: 0x0000FCDB
		internal SignedXml SignedXml
		{
			get
			{
				return this.m_signedXml;
			}
			set
			{
				this.m_signedXml = value;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x0600034A RID: 842 RVA: 0x00010CE4 File Offset: 0x0000FCE4
		internal ReferenceTargetType ReferenceTargetType
		{
			get
			{
				return this.m_refTargetType;
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00010CEC File Offset: 0x0000FCEC
		public XmlElement GetXml()
		{
			if (this.CacheValid)
			{
				return this.m_cachedXml;
			}
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00010D1C File Offset: 0x0000FD1C
		internal XmlElement GetXml(XmlDocument document)
		{
			XmlElement xmlElement = document.CreateElement("Reference", "http://www.w3.org/2000/09/xmldsig#");
			if (!string.IsNullOrEmpty(this.m_id))
			{
				xmlElement.SetAttribute("Id", this.m_id);
			}
			if (this.m_uri != null)
			{
				xmlElement.SetAttribute("URI", this.m_uri);
			}
			if (!string.IsNullOrEmpty(this.m_type))
			{
				xmlElement.SetAttribute("Type", this.m_type);
			}
			if (this.TransformChain.Count != 0)
			{
				xmlElement.AppendChild(this.TransformChain.GetXml(document, "http://www.w3.org/2000/09/xmldsig#"));
			}
			if (string.IsNullOrEmpty(this.m_digestMethod))
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_DigestMethodRequired"));
			}
			XmlElement xmlElement2 = document.CreateElement("DigestMethod", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement2.SetAttribute("Algorithm", this.m_digestMethod);
			xmlElement.AppendChild(xmlElement2);
			if (this.DigestValue == null)
			{
				if (this.m_hashAlgorithm.Hash == null)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_DigestValueRequired"));
				}
				this.DigestValue = this.m_hashAlgorithm.Hash;
			}
			XmlElement xmlElement3 = document.CreateElement("DigestValue", "http://www.w3.org/2000/09/xmldsig#");
			xmlElement3.AppendChild(document.CreateTextNode(Convert.ToBase64String(this.m_digestValue)));
			xmlElement.AppendChild(xmlElement3);
			return xmlElement;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00010E64 File Offset: 0x0000FE64
		public void LoadXml(XmlElement value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			this.m_id = Utils.GetAttribute(value, "Id", "http://www.w3.org/2000/09/xmldsig#");
			this.m_uri = Utils.GetAttribute(value, "URI", "http://www.w3.org/2000/09/xmldsig#");
			this.m_type = Utils.GetAttribute(value, "Type", "http://www.w3.org/2000/09/xmldsig#");
			if (!Utils.VerifyAttributes(value, new string[] { "Id", "URI", "Type" }))
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference");
			}
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(value.OwnerDocument.NameTable);
			xmlNamespaceManager.AddNamespace("ds", "http://www.w3.org/2000/09/xmldsig#");
			bool flag = false;
			this.TransformChain = new TransformChain();
			XmlNodeList xmlNodeList = value.SelectNodes("ds:Transforms", xmlNamespaceManager);
			if (xmlNodeList != null && xmlNodeList.Count != 0)
			{
				if (!Utils.GetAllowAdditionalSignatureNodes() && xmlNodeList.Count > 1)
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference/Transforms");
				}
				flag = true;
				XmlElement xmlElement = xmlNodeList[0] as XmlElement;
				if (!Utils.VerifyAttributes(xmlElement, null))
				{
					throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference/Transforms");
				}
				XmlNodeList xmlNodeList2 = xmlElement.SelectNodes("ds:Transform", xmlNamespaceManager);
				if (xmlNodeList2 != null)
				{
					if (!Utils.GetAllowAdditionalSignatureNodes() && xmlNodeList2.Count != xmlElement.SelectNodes("*").Count)
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference/Transforms");
					}
					if ((long)xmlNodeList2.Count > Utils.GetMaxTransformsPerReference())
					{
						throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference/Transforms");
					}
					foreach (object obj in xmlNodeList2)
					{
						XmlNode xmlNode = (XmlNode)obj;
						XmlElement xmlElement2 = xmlNode as XmlElement;
						string attribute = Utils.GetAttribute(xmlElement2, "Algorithm", "http://www.w3.org/2000/09/xmldsig#");
						if ((attribute == null && !Utils.GetSkipSignatureAttributeEnforcement()) || !Utils.VerifyAttributes(xmlElement2, "Algorithm"))
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
						}
						Transform transform = Utils.CreateFromName<Transform>(attribute);
						if (transform == null)
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
						}
						this.AddTransform(transform);
						transform.LoadInnerXml(xmlElement2.ChildNodes);
						if (transform is XmlDsigEnvelopedSignatureTransform)
						{
							XmlNode xmlNode2 = xmlElement2.SelectSingleNode("ancestor::ds:Signature[1]", xmlNamespaceManager);
							XmlNodeList xmlNodeList3 = xmlElement2.SelectNodes("//ds:Signature", xmlNamespaceManager);
							if (xmlNodeList3 != null)
							{
								int num = 0;
								foreach (object obj2 in xmlNodeList3)
								{
									XmlNode xmlNode3 = (XmlNode)obj2;
									num++;
									if (xmlNode3 == xmlNode2)
									{
										((XmlDsigEnvelopedSignatureTransform)transform).SignaturePosition = num;
										break;
									}
								}
							}
						}
					}
				}
			}
			XmlNodeList xmlNodeList4 = value.SelectNodes("ds:DigestMethod", xmlNamespaceManager);
			if (xmlNodeList4 == null || xmlNodeList4.Count == 0 || (!Utils.GetAllowAdditionalSignatureNodes() && xmlNodeList4.Count > 1))
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference/DigestMethod");
			}
			XmlElement xmlElement3 = xmlNodeList4[0] as XmlElement;
			this.m_digestMethod = Utils.GetAttribute(xmlElement3, "Algorithm", "http://www.w3.org/2000/09/xmldsig#");
			if ((this.m_digestMethod == null && !Utils.GetSkipSignatureAttributeEnforcement()) || !Utils.VerifyAttributes(xmlElement3, "Algorithm"))
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference/DigestMethod");
			}
			XmlNodeList xmlNodeList5 = value.SelectNodes("ds:DigestValue", xmlNamespaceManager);
			if (xmlNodeList5 == null || xmlNodeList5.Count == 0 || (!Utils.GetAllowAdditionalSignatureNodes() && xmlNodeList5.Count > 1))
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference/DigestValue");
			}
			XmlElement xmlElement4 = xmlNodeList5[0] as XmlElement;
			this.m_digestValue = Convert.FromBase64String(Utils.DiscardWhiteSpaces(xmlElement4.InnerText));
			if (!Utils.VerifyAttributes(xmlElement4, null))
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference/DigestValue");
			}
			int num2 = (flag ? 3 : 2);
			if (!Utils.GetAllowAdditionalSignatureNodes() && value.SelectNodes("*").Count != num2)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidElement"), "Reference");
			}
			this.m_cachedXml = value;
		}

		// Token: 0x0600034E RID: 846 RVA: 0x000112DC File Offset: 0x000102DC
		public void AddTransform(Transform transform)
		{
			if (transform == null)
			{
				throw new ArgumentNullException("transform");
			}
			transform.Reference = this;
			this.TransformChain.Add(transform);
		}

		// Token: 0x0600034F RID: 847 RVA: 0x000112FF File Offset: 0x000102FF
		internal void UpdateHashValue(XmlDocument document, CanonicalXmlNodeList refList)
		{
			this.DigestValue = this.CalculateHashValue(document, refList);
		}

		// Token: 0x06000350 RID: 848 RVA: 0x00011310 File Offset: 0x00010310
		internal byte[] CalculateHashValue(XmlDocument document, CanonicalXmlNodeList refList)
		{
			this.m_hashAlgorithm = Utils.CreateFromName<HashAlgorithm>(this.m_digestMethod);
			if (this.m_hashAlgorithm == null)
			{
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_CreateHashAlgorithmFailed"));
			}
			string text = ((document == null) ? (Environment.CurrentDirectory + "\\") : document.BaseURI);
			Stream stream = null;
			WebResponse webResponse = null;
			Stream stream2 = null;
			XmlResolver xmlResolver = null;
			byte[] array = null;
			try
			{
				switch (this.m_refTargetType)
				{
				case ReferenceTargetType.Stream:
					xmlResolver = (this.SignedXml.ResolverSet ? this.SignedXml.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), text));
					stream = this.TransformChain.TransformToOctetStream((Stream)this.m_refTarget, xmlResolver, text);
					goto IL_04A5;
				case ReferenceTargetType.XmlElement:
					xmlResolver = (this.SignedXml.ResolverSet ? this.SignedXml.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), text));
					stream = this.TransformChain.TransformToOctetStream(Utils.PreProcessElementInput((XmlElement)this.m_refTarget, xmlResolver, text), xmlResolver, text);
					goto IL_04A5;
				case ReferenceTargetType.UriReference:
					if (this.m_uri == null)
					{
						xmlResolver = (this.SignedXml.ResolverSet ? this.SignedXml.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), text));
						stream = this.TransformChain.TransformToOctetStream(null, xmlResolver, text);
						goto IL_04A5;
					}
					if (this.m_uri.Length == 0)
					{
						if (document == null)
						{
							throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Cryptography_Xml_SelfReferenceRequiresContext"), new object[] { this.m_uri }));
						}
						xmlResolver = (this.SignedXml.ResolverSet ? this.SignedXml.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), text));
						XmlDocument xmlDocument = Utils.DiscardComments(Utils.PreProcessDocumentInput(document, xmlResolver, text));
						stream = this.TransformChain.TransformToOctetStream(xmlDocument, xmlResolver, text);
						goto IL_04A5;
					}
					else if (this.m_uri[0] == '#')
					{
						bool flag = true;
						string idFromLocalUri = Utils.GetIdFromLocalUri(this.m_uri, out flag);
						if (idFromLocalUri == "xpointer(/)")
						{
							if (document == null)
							{
								throw new CryptographicException(string.Format(CultureInfo.CurrentCulture, SecurityResources.GetResourceString("Cryptography_Xml_SelfReferenceRequiresContext"), new object[] { this.m_uri }));
							}
							xmlResolver = (this.SignedXml.ResolverSet ? this.SignedXml.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), text));
							stream = this.TransformChain.TransformToOctetStream(Utils.PreProcessDocumentInput(document, xmlResolver, text), xmlResolver, text);
							goto IL_04A5;
						}
						else
						{
							XmlElement xmlElement = this.SignedXml.GetIdElement(document, idFromLocalUri);
							if (xmlElement != null)
							{
								this.m_namespaces = Utils.GetPropagatedAttributes(xmlElement.ParentNode as XmlElement);
							}
							if (xmlElement == null && refList != null)
							{
								foreach (object obj in refList)
								{
									XmlNode xmlNode = (XmlNode)obj;
									XmlElement xmlElement2 = xmlNode as XmlElement;
									if (xmlElement2 != null && Utils.HasAttribute(xmlElement2, "Id", "http://www.w3.org/2000/09/xmldsig#") && Utils.GetAttribute(xmlElement2, "Id", "http://www.w3.org/2000/09/xmldsig#").Equals(idFromLocalUri))
									{
										xmlElement = xmlElement2;
										if (this.m_signedXml.m_context != null)
										{
											this.m_namespaces = Utils.GetPropagatedAttributes(this.m_signedXml.m_context);
											break;
										}
										break;
									}
								}
							}
							if (xmlElement == null)
							{
								throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_InvalidReference"));
							}
							XmlDocument xmlDocument2 = Utils.PreProcessElementInput(xmlElement, xmlResolver, text);
							Utils.AddNamespaces(xmlDocument2.DocumentElement, this.m_namespaces);
							xmlResolver = (this.SignedXml.ResolverSet ? this.SignedXml.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), text));
							if (flag)
							{
								XmlDocument xmlDocument3 = Utils.DiscardComments(xmlDocument2);
								stream = this.TransformChain.TransformToOctetStream(xmlDocument3, xmlResolver, text);
								goto IL_04A5;
							}
							stream = this.TransformChain.TransformToOctetStream(xmlDocument2, xmlResolver, text);
							goto IL_04A5;
						}
					}
					else
					{
						if (!Utils.AllowDetachedSignature())
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UriNotResolved"), this.m_uri);
						}
						Uri uri = new Uri(this.m_uri, UriKind.RelativeOrAbsolute);
						if (!uri.IsAbsoluteUri)
						{
							uri = new Uri(new Uri(text), uri);
						}
						WebRequest webRequest = WebRequest.Create(uri);
						if (webRequest != null)
						{
							webResponse = webRequest.GetResponse();
							if (webResponse != null)
							{
								stream2 = webResponse.GetResponseStream();
								if (stream2 != null)
								{
									xmlResolver = (this.SignedXml.ResolverSet ? this.SignedXml.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), text));
									stream = this.TransformChain.TransformToOctetStream(stream2, xmlResolver, this.m_uri);
									goto IL_04A5;
								}
							}
						}
					}
					break;
				}
				throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UriNotResolved"), this.m_uri);
				IL_04A5:
				array = this.m_hashAlgorithm.ComputeHash(stream);
			}
			finally
			{
				if (stream != null)
				{
					stream.Close();
				}
				if (webResponse != null)
				{
					webResponse.Close();
				}
				if (stream2 != null)
				{
					stream2.Close();
				}
			}
			return array;
		}

		// Token: 0x04000508 RID: 1288
		private string m_id;

		// Token: 0x04000509 RID: 1289
		private string m_uri;

		// Token: 0x0400050A RID: 1290
		private string m_type;

		// Token: 0x0400050B RID: 1291
		private TransformChain m_transformChain;

		// Token: 0x0400050C RID: 1292
		private string m_digestMethod;

		// Token: 0x0400050D RID: 1293
		private byte[] m_digestValue;

		// Token: 0x0400050E RID: 1294
		private HashAlgorithm m_hashAlgorithm;

		// Token: 0x0400050F RID: 1295
		private object m_refTarget;

		// Token: 0x04000510 RID: 1296
		private ReferenceTargetType m_refTargetType;

		// Token: 0x04000511 RID: 1297
		private XmlElement m_cachedXml;

		// Token: 0x04000512 RID: 1298
		private SignedXml m_signedXml;

		// Token: 0x04000513 RID: 1299
		internal CanonicalXmlNodeList m_namespaces;
	}
}
