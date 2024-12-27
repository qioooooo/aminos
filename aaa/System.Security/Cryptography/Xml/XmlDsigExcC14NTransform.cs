using System;
using System.IO;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000AF RID: 175
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public class XmlDsigExcC14NTransform : Transform
	{
		// Token: 0x060003E6 RID: 998 RVA: 0x00014257 File Offset: 0x00013257
		public XmlDsigExcC14NTransform()
			: this(false, null)
		{
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00014261 File Offset: 0x00013261
		public XmlDsigExcC14NTransform(bool includeComments)
			: this(includeComments, null)
		{
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x0001426B File Offset: 0x0001326B
		public XmlDsigExcC14NTransform(string inclusiveNamespacesPrefixList)
			: this(false, inclusiveNamespacesPrefixList)
		{
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00014278 File Offset: 0x00013278
		public XmlDsigExcC14NTransform(bool includeComments, string inclusiveNamespacesPrefixList)
		{
			this._includeComments = includeComments;
			this._inclusiveNamespacesPrefixList = inclusiveNamespacesPrefixList;
			base.Algorithm = (includeComments ? "http://www.w3.org/2001/10/xml-exc-c14n#WithComments" : "http://www.w3.org/2001/10/xml-exc-c14n#");
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x000142FE File Offset: 0x000132FE
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x00014306 File Offset: 0x00013306
		public string InclusiveNamespacesPrefixList
		{
			get
			{
				return this._inclusiveNamespacesPrefixList;
			}
			set
			{
				this._inclusiveNamespacesPrefixList = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x060003EC RID: 1004 RVA: 0x0001430F File Offset: 0x0001330F
		public override Type[] InputTypes
		{
			get
			{
				return this._inputTypes;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x060003ED RID: 1005 RVA: 0x00014317 File Offset: 0x00013317
		public override Type[] OutputTypes
		{
			get
			{
				return this._outputTypes;
			}
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00014320 File Offset: 0x00013320
		public override void LoadInnerXml(XmlNodeList nodeList)
		{
			if (nodeList != null)
			{
				foreach (object obj in nodeList)
				{
					XmlNode xmlNode = (XmlNode)obj;
					XmlElement xmlElement = xmlNode as XmlElement;
					if (xmlElement != null)
					{
						if (xmlElement.LocalName.Equals("InclusiveNamespaces") && xmlElement.NamespaceURI.Equals("http://www.w3.org/2001/10/xml-exc-c14n#") && Utils.HasAttribute(xmlElement, "PrefixList", "http://www.w3.org/2000/09/xmldsig#"))
						{
							if (!Utils.VerifyAttributes(xmlElement, "PrefixList"))
							{
								throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
							}
							this.InclusiveNamespacesPrefixList = Utils.GetAttribute(xmlElement, "PrefixList", "http://www.w3.org/2000/09/xmldsig#");
							break;
						}
						else if (!Utils.GetAllowAdditionalSignatureNodes())
						{
							throw new CryptographicException(SecurityResources.GetResourceString("Cryptography_Xml_UnknownTransform"));
						}
					}
				}
			}
		}

		// Token: 0x060003EF RID: 1007 RVA: 0x00014408 File Offset: 0x00013408
		public override void LoadInput(object obj)
		{
			XmlResolver xmlResolver = (base.ResolverSet ? this.m_xmlResolver : new XmlSecureResolver(new XmlUrlResolver(), base.BaseURI));
			if (obj is Stream)
			{
				this._excCanonicalXml = new ExcCanonicalXml((Stream)obj, this._includeComments, this._inclusiveNamespacesPrefixList, xmlResolver, base.BaseURI);
				return;
			}
			if (obj is XmlDocument)
			{
				this._excCanonicalXml = new ExcCanonicalXml((XmlDocument)obj, this._includeComments, this._inclusiveNamespacesPrefixList, xmlResolver);
				return;
			}
			if (obj is XmlNodeList)
			{
				this._excCanonicalXml = new ExcCanonicalXml((XmlNodeList)obj, this._includeComments, this._inclusiveNamespacesPrefixList, xmlResolver);
				return;
			}
			throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_IncorrectObjectType"), "obj");
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x000144C8 File Offset: 0x000134C8
		protected override XmlNodeList GetInnerXml()
		{
			if (this.InclusiveNamespacesPrefixList == null)
			{
				return null;
			}
			XmlDocument xmlDocument = new XmlDocument();
			XmlElement xmlElement = xmlDocument.CreateElement("Transform", "http://www.w3.org/2000/09/xmldsig#");
			if (!string.IsNullOrEmpty(base.Algorithm))
			{
				xmlElement.SetAttribute("Algorithm", base.Algorithm);
			}
			XmlElement xmlElement2 = xmlDocument.CreateElement("InclusiveNamespaces", "http://www.w3.org/2001/10/xml-exc-c14n#");
			xmlElement2.SetAttribute("PrefixList", this.InclusiveNamespacesPrefixList);
			xmlElement.AppendChild(xmlElement2);
			return xmlElement.ChildNodes;
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x00014544 File Offset: 0x00013544
		public override object GetOutput()
		{
			return new MemoryStream(this._excCanonicalXml.GetBytes());
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00014558 File Offset: 0x00013558
		public override object GetOutput(Type type)
		{
			if (type != typeof(Stream) && !type.IsSubclassOf(typeof(Stream)))
			{
				throw new ArgumentException(SecurityResources.GetResourceString("Cryptography_Xml_TransformIncorrectInputType"), "type");
			}
			return new MemoryStream(this._excCanonicalXml.GetBytes());
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x000145A9 File Offset: 0x000135A9
		public override byte[] GetDigestedOutput(HashAlgorithm hash)
		{
			return this._excCanonicalXml.GetDigestedBytes(hash);
		}

		// Token: 0x04000562 RID: 1378
		private Type[] _inputTypes = new Type[]
		{
			typeof(Stream),
			typeof(XmlDocument),
			typeof(XmlNodeList)
		};

		// Token: 0x04000563 RID: 1379
		private Type[] _outputTypes = new Type[] { typeof(Stream) };

		// Token: 0x04000564 RID: 1380
		private bool _includeComments;

		// Token: 0x04000565 RID: 1381
		private string _inclusiveNamespacesPrefixList;

		// Token: 0x04000566 RID: 1382
		private ExcCanonicalXml _excCanonicalXml;
	}
}
