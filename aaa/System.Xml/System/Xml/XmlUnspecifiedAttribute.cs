using System;

namespace System.Xml
{
	// Token: 0x020000EF RID: 239
	internal class XmlUnspecifiedAttribute : XmlAttribute
	{
		// Token: 0x06000EAC RID: 3756 RVA: 0x00040969 File Offset: 0x0003F969
		protected internal XmlUnspecifiedAttribute(string prefix, string localName, string namespaceURI, XmlDocument doc)
			: base(prefix, localName, namespaceURI, doc)
		{
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000EAD RID: 3757 RVA: 0x00040976 File Offset: 0x0003F976
		public override bool Specified
		{
			get
			{
				return this.fSpecified;
			}
		}

		// Token: 0x06000EAE RID: 3758 RVA: 0x00040980 File Offset: 0x0003F980
		public override XmlNode CloneNode(bool deep)
		{
			XmlDocument ownerDocument = this.OwnerDocument;
			XmlUnspecifiedAttribute xmlUnspecifiedAttribute = (XmlUnspecifiedAttribute)ownerDocument.CreateDefaultAttribute(this.Prefix, this.LocalName, this.NamespaceURI);
			xmlUnspecifiedAttribute.CopyChildren(ownerDocument, this, true);
			xmlUnspecifiedAttribute.fSpecified = true;
			return xmlUnspecifiedAttribute;
		}

		// Token: 0x170003A3 RID: 931
		// (set) Token: 0x06000EAF RID: 3759 RVA: 0x000409C3 File Offset: 0x0003F9C3
		public override string InnerText
		{
			set
			{
				base.InnerText = value;
				this.fSpecified = true;
			}
		}

		// Token: 0x06000EB0 RID: 3760 RVA: 0x000409D4 File Offset: 0x0003F9D4
		public override XmlNode InsertBefore(XmlNode newChild, XmlNode refChild)
		{
			XmlNode xmlNode = base.InsertBefore(newChild, refChild);
			this.fSpecified = true;
			return xmlNode;
		}

		// Token: 0x06000EB1 RID: 3761 RVA: 0x000409F4 File Offset: 0x0003F9F4
		public override XmlNode InsertAfter(XmlNode newChild, XmlNode refChild)
		{
			XmlNode xmlNode = base.InsertAfter(newChild, refChild);
			this.fSpecified = true;
			return xmlNode;
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x00040A14 File Offset: 0x0003FA14
		public override XmlNode ReplaceChild(XmlNode newChild, XmlNode oldChild)
		{
			XmlNode xmlNode = base.ReplaceChild(newChild, oldChild);
			this.fSpecified = true;
			return xmlNode;
		}

		// Token: 0x06000EB3 RID: 3763 RVA: 0x00040A34 File Offset: 0x0003FA34
		public override XmlNode RemoveChild(XmlNode oldChild)
		{
			XmlNode xmlNode = base.RemoveChild(oldChild);
			this.fSpecified = true;
			return xmlNode;
		}

		// Token: 0x06000EB4 RID: 3764 RVA: 0x00040A54 File Offset: 0x0003FA54
		public override XmlNode AppendChild(XmlNode newChild)
		{
			XmlNode xmlNode = base.AppendChild(newChild);
			this.fSpecified = true;
			return xmlNode;
		}

		// Token: 0x06000EB5 RID: 3765 RVA: 0x00040A71 File Offset: 0x0003FA71
		public override void WriteTo(XmlWriter w)
		{
			if (this.fSpecified)
			{
				base.WriteTo(w);
			}
		}

		// Token: 0x06000EB6 RID: 3766 RVA: 0x00040A82 File Offset: 0x0003FA82
		internal void SetSpecified(bool f)
		{
			this.fSpecified = f;
		}

		// Token: 0x040009A7 RID: 2471
		private bool fSpecified;
	}
}
