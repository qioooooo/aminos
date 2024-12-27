using System;

namespace System.Xml
{
	// Token: 0x020000DF RID: 223
	public class XmlEntityReference : XmlLinkedNode
	{
		// Token: 0x06000DA9 RID: 3497 RVA: 0x0003C3DC File Offset: 0x0003B3DC
		protected internal XmlEntityReference(string name, XmlDocument doc)
			: base(doc)
		{
			if (!doc.IsLoading && name.Length > 0 && name[0] == '#')
			{
				throw new ArgumentException(Res.GetString("Xdom_InvalidCharacter_EntityReference"));
			}
			this.name = doc.NameTable.Add(name);
			doc.fEntRefNodesPresent = true;
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x0003C435 File Offset: 0x0003B435
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0003C43D File Offset: 0x0003B43D
		public override string LocalName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x0003C445 File Offset: 0x0003B445
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x0003C448 File Offset: 0x0003B448
		public override string Value
		{
			get
			{
				return null;
			}
			set
			{
				throw new InvalidOperationException(Res.GetString("Xdom_EntRef_SetVal"));
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x0003C459 File Offset: 0x0003B459
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.EntityReference;
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0003C45C File Offset: 0x0003B45C
		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateEntityReference(this.name);
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000DB0 RID: 3504 RVA: 0x0003C47C File Offset: 0x0003B47C
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000DB1 RID: 3505 RVA: 0x0003C47F File Offset: 0x0003B47F
		internal override bool IsContainer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0003C484 File Offset: 0x0003B484
		internal override void SetParent(XmlNode node)
		{
			base.SetParent(node);
			if (this.LastNode == null && node != null && node != this.OwnerDocument)
			{
				XmlLoader xmlLoader = new XmlLoader();
				xmlLoader.ExpandEntityReference(this);
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0003C4B9 File Offset: 0x0003B4B9
		internal override void SetParentForLoad(XmlNode node)
		{
			this.SetParent(node);
		}

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000DB4 RID: 3508 RVA: 0x0003C4C2 File Offset: 0x0003B4C2
		// (set) Token: 0x06000DB5 RID: 3509 RVA: 0x0003C4CA File Offset: 0x0003B4CA
		internal override XmlLinkedNode LastNode
		{
			get
			{
				return this.lastChild;
			}
			set
			{
				this.lastChild = value;
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0003C4D4 File Offset: 0x0003B4D4
		internal override bool IsValidChildType(XmlNodeType type)
		{
			switch (type)
			{
			case XmlNodeType.Element:
			case XmlNodeType.Text:
			case XmlNodeType.CDATA:
			case XmlNodeType.EntityReference:
			case XmlNodeType.ProcessingInstruction:
			case XmlNodeType.Comment:
			case XmlNodeType.Whitespace:
			case XmlNodeType.SignificantWhitespace:
				return true;
			}
			return false;
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0003C528 File Offset: 0x0003B528
		public override void WriteTo(XmlWriter w)
		{
			w.WriteEntityRef(this.name);
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x0003C538 File Offset: 0x0003B538
		public override void WriteContentTo(XmlWriter w)
		{
			foreach (object obj in this)
			{
				XmlNode xmlNode = (XmlNode)obj;
				xmlNode.WriteTo(w);
			}
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000DB9 RID: 3513 RVA: 0x0003C58C File Offset: 0x0003B58C
		public override string BaseURI
		{
			get
			{
				return this.OwnerDocument.BaseURI;
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0003C59C File Offset: 0x0003B59C
		private string ConstructBaseURI(string baseURI, string systemId)
		{
			if (baseURI == null)
			{
				return systemId;
			}
			int num = baseURI.LastIndexOf('/') + 1;
			string text = baseURI;
			if (num > 0 && num < baseURI.Length)
			{
				text = baseURI.Substring(0, num);
			}
			else if (num == 0)
			{
				text += "\\";
			}
			return text + systemId.Replace('\\', '/');
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000DBB RID: 3515 RVA: 0x0003C5F4 File Offset: 0x0003B5F4
		internal string ChildBaseURI
		{
			get
			{
				XmlEntity entityNode = this.OwnerDocument.GetEntityNode(this.name);
				if (entityNode == null)
				{
					return string.Empty;
				}
				if (entityNode.SystemId != null && entityNode.SystemId.Length > 0)
				{
					return this.ConstructBaseURI(entityNode.BaseURI, entityNode.SystemId);
				}
				return entityNode.BaseURI;
			}
		}

		// Token: 0x04000962 RID: 2402
		private string name;

		// Token: 0x04000963 RID: 2403
		private XmlLinkedNode lastChild;
	}
}
