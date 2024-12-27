using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Xml;

namespace System.Security.Cryptography.Xml
{
	// Token: 0x020000AC RID: 172
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	public abstract class Transform
	{
		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060003C1 RID: 961 RVA: 0x00013CB9 File Offset: 0x00012CB9
		// (set) Token: 0x060003C2 RID: 962 RVA: 0x00013CC1 File Offset: 0x00012CC1
		internal string BaseURI
		{
			get
			{
				return this.m_baseUri;
			}
			set
			{
				this.m_baseUri = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060003C3 RID: 963 RVA: 0x00013CCA File Offset: 0x00012CCA
		// (set) Token: 0x060003C4 RID: 964 RVA: 0x00013CD2 File Offset: 0x00012CD2
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

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060003C5 RID: 965 RVA: 0x00013CDB File Offset: 0x00012CDB
		// (set) Token: 0x060003C6 RID: 966 RVA: 0x00013CE3 File Offset: 0x00012CE3
		internal Reference Reference
		{
			get
			{
				return this.m_reference;
			}
			set
			{
				this.m_reference = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060003C8 RID: 968 RVA: 0x00013CF4 File Offset: 0x00012CF4
		// (set) Token: 0x060003C9 RID: 969 RVA: 0x00013CFC File Offset: 0x00012CFC
		public string Algorithm
		{
			get
			{
				return this.m_algorithm;
			}
			set
			{
				this.m_algorithm = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (set) Token: 0x060003CA RID: 970 RVA: 0x00013D05 File Offset: 0x00012D05
		[ComVisible(false)]
		public XmlResolver Resolver
		{
			set
			{
				this.m_xmlResolver = value;
				this.m_bResolverSet = true;
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060003CB RID: 971 RVA: 0x00013D15 File Offset: 0x00012D15
		internal bool ResolverSet
		{
			get
			{
				return this.m_bResolverSet;
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060003CC RID: 972
		public abstract Type[] InputTypes { get; }

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060003CD RID: 973
		public abstract Type[] OutputTypes { get; }

		// Token: 0x060003CE RID: 974 RVA: 0x00013D20 File Offset: 0x00012D20
		internal bool AcceptsType(Type inputType)
		{
			if (this.InputTypes != null)
			{
				for (int i = 0; i < this.InputTypes.Length; i++)
				{
					if (inputType == this.InputTypes[i] || inputType.IsSubclassOf(this.InputTypes[i]))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00013D68 File Offset: 0x00012D68
		public XmlElement GetXml()
		{
			return this.GetXml(new XmlDocument
			{
				PreserveWhitespace = true
			});
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00013D89 File Offset: 0x00012D89
		internal XmlElement GetXml(XmlDocument document)
		{
			return this.GetXml(document, "Transform");
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x00013D98 File Offset: 0x00012D98
		internal XmlElement GetXml(XmlDocument document, string name)
		{
			XmlElement xmlElement = document.CreateElement(name, "http://www.w3.org/2000/09/xmldsig#");
			if (!string.IsNullOrEmpty(this.Algorithm))
			{
				xmlElement.SetAttribute("Algorithm", this.Algorithm);
			}
			XmlNodeList innerXml = this.GetInnerXml();
			if (innerXml != null)
			{
				foreach (object obj in innerXml)
				{
					XmlNode xmlNode = (XmlNode)obj;
					xmlElement.AppendChild(document.ImportNode(xmlNode, true));
				}
			}
			return xmlElement;
		}

		// Token: 0x060003D2 RID: 978
		public abstract void LoadInnerXml(XmlNodeList nodeList);

		// Token: 0x060003D3 RID: 979
		protected abstract XmlNodeList GetInnerXml();

		// Token: 0x060003D4 RID: 980
		public abstract void LoadInput(object obj);

		// Token: 0x060003D5 RID: 981
		public abstract object GetOutput();

		// Token: 0x060003D6 RID: 982
		public abstract object GetOutput(Type type);

		// Token: 0x060003D7 RID: 983 RVA: 0x00013E30 File Offset: 0x00012E30
		[ComVisible(false)]
		public virtual byte[] GetDigestedOutput(HashAlgorithm hash)
		{
			return hash.ComputeHash((Stream)this.GetOutput(typeof(Stream)));
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00013E50 File Offset: 0x00012E50
		// (set) Token: 0x060003D9 RID: 985 RVA: 0x00013E90 File Offset: 0x00012E90
		[ComVisible(false)]
		public XmlElement Context
		{
			get
			{
				if (this.m_context != null)
				{
					return this.m_context;
				}
				Reference reference = this.Reference;
				SignedXml signedXml = ((reference == null) ? this.SignedXml : reference.SignedXml);
				if (signedXml == null)
				{
					return null;
				}
				return signedXml.m_context;
			}
			set
			{
				this.m_context = value;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060003DA RID: 986 RVA: 0x00013E9C File Offset: 0x00012E9C
		[ComVisible(false)]
		public Hashtable PropagatedNamespaces
		{
			get
			{
				if (this.m_propagatedNamespaces != null)
				{
					return this.m_propagatedNamespaces;
				}
				Reference reference = this.Reference;
				SignedXml signedXml = ((reference == null) ? this.SignedXml : reference.SignedXml);
				if (reference != null && (reference.ReferenceTargetType != ReferenceTargetType.UriReference || reference.Uri == null || reference.Uri.Length == 0 || reference.Uri[0] != '#'))
				{
					this.m_propagatedNamespaces = new Hashtable(0);
					return this.m_propagatedNamespaces;
				}
				CanonicalXmlNodeList canonicalXmlNodeList = null;
				if (reference != null)
				{
					canonicalXmlNodeList = reference.m_namespaces;
				}
				else if (signedXml.m_context != null)
				{
					canonicalXmlNodeList = Utils.GetPropagatedAttributes(signedXml.m_context);
				}
				if (canonicalXmlNodeList == null)
				{
					this.m_propagatedNamespaces = new Hashtable(0);
					return this.m_propagatedNamespaces;
				}
				this.m_propagatedNamespaces = new Hashtable(canonicalXmlNodeList.Count);
				foreach (object obj in canonicalXmlNodeList)
				{
					XmlNode xmlNode = (XmlNode)obj;
					string text = ((xmlNode.Prefix.Length > 0) ? (xmlNode.Prefix + ":" + xmlNode.LocalName) : xmlNode.LocalName);
					if (!this.m_propagatedNamespaces.Contains(text))
					{
						this.m_propagatedNamespaces.Add(text, xmlNode.Value);
					}
				}
				return this.m_propagatedNamespaces;
			}
		}

		// Token: 0x04000556 RID: 1366
		private string m_algorithm;

		// Token: 0x04000557 RID: 1367
		private string m_baseUri;

		// Token: 0x04000558 RID: 1368
		internal XmlResolver m_xmlResolver;

		// Token: 0x04000559 RID: 1369
		private bool m_bResolverSet;

		// Token: 0x0400055A RID: 1370
		private SignedXml m_signedXml;

		// Token: 0x0400055B RID: 1371
		private Reference m_reference;

		// Token: 0x0400055C RID: 1372
		private Hashtable m_propagatedNamespaces;

		// Token: 0x0400055D RID: 1373
		private XmlElement m_context;
	}
}
