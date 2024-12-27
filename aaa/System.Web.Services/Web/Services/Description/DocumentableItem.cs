using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Xml;
using System.Xml.Serialization;

namespace System.Web.Services.Description
{
	// Token: 0x020000DD RID: 221
	public abstract class DocumentableItem
	{
		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060005D5 RID: 1493 RVA: 0x0001C951 File Offset: 0x0001B951
		// (set) Token: 0x060005D6 RID: 1494 RVA: 0x0001C97C File Offset: 0x0001B97C
		[XmlIgnore]
		public string Documentation
		{
			get
			{
				if (this.documentation != null)
				{
					return this.documentation;
				}
				if (this.documentationElement == null)
				{
					return string.Empty;
				}
				return this.documentationElement.InnerXml;
			}
			set
			{
				this.documentation = value;
				StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
				XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
				xmlTextWriter.WriteElementString("wsdl", "documentation", "http://schemas.xmlsoap.org/wsdl/", value);
				this.Parent.LoadXml(stringWriter.ToString());
				this.documentationElement = this.parent.DocumentElement;
				xmlTextWriter.Close();
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060005D7 RID: 1495 RVA: 0x0001C9E0 File Offset: 0x0001B9E0
		// (set) Token: 0x060005D8 RID: 1496 RVA: 0x0001C9E8 File Offset: 0x0001B9E8
		[ComVisible(false)]
		[XmlAnyElement("documentation", Namespace = "http://schemas.xmlsoap.org/wsdl/")]
		public XmlElement DocumentationElement
		{
			get
			{
				return this.documentationElement;
			}
			set
			{
				this.documentationElement = value;
				this.documentation = null;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060005D9 RID: 1497 RVA: 0x0001C9F8 File Offset: 0x0001B9F8
		// (set) Token: 0x060005DA RID: 1498 RVA: 0x0001CA00 File Offset: 0x0001BA00
		[XmlAnyAttribute]
		public XmlAttribute[] ExtensibleAttributes
		{
			get
			{
				return this.anyAttribute;
			}
			set
			{
				this.anyAttribute = value;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x0001CA09 File Offset: 0x0001BA09
		// (set) Token: 0x060005DC RID: 1500 RVA: 0x0001CA24 File Offset: 0x0001BA24
		[XmlNamespaceDeclarations]
		public XmlSerializerNamespaces Namespaces
		{
			get
			{
				if (this.namespaces == null)
				{
					this.namespaces = new XmlSerializerNamespaces();
				}
				return this.namespaces;
			}
			set
			{
				this.namespaces = value;
			}
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060005DD RID: 1501
		[XmlIgnore]
		public abstract ServiceDescriptionFormatExtensionCollection Extensions { get; }

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001CA2D File Offset: 0x0001BA2D
		internal XmlDocument Parent
		{
			get
			{
				if (this.parent == null)
				{
					this.parent = new XmlDocument();
				}
				return this.parent;
			}
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0001CA48 File Offset: 0x0001BA48
		internal XmlElement GetDocumentationElement()
		{
			if (this.documentationElement == null)
			{
				this.documentationElement = this.Parent.CreateElement("wsdl", "documentation", "http://schemas.xmlsoap.org/wsdl/");
				this.Parent.InsertBefore(this.documentationElement, null);
			}
			return this.documentationElement;
		}

		// Token: 0x04000435 RID: 1077
		private XmlDocument parent;

		// Token: 0x04000436 RID: 1078
		private string documentation;

		// Token: 0x04000437 RID: 1079
		private XmlElement documentationElement;

		// Token: 0x04000438 RID: 1080
		private XmlAttribute[] anyAttribute;

		// Token: 0x04000439 RID: 1081
		private XmlSerializerNamespaces namespaces;
	}
}
