using System;

namespace System.Xml
{
	// Token: 0x020000DE RID: 222
	public class XmlEntity : XmlNode
	{
		// Token: 0x06000D93 RID: 3475 RVA: 0x0003C294 File Offset: 0x0003B294
		internal XmlEntity(string name, string strdata, string publicId, string systemId, string notationName, XmlDocument doc)
			: base(doc)
		{
			this.name = doc.NameTable.Add(name);
			this.publicId = publicId;
			this.systemId = systemId;
			this.notationName = notationName;
			this.unparsedReplacementStr = strdata;
			this.childrenFoliating = false;
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x0003C2E1 File Offset: 0x0003B2E1
		public override XmlNode CloneNode(bool deep)
		{
			throw new InvalidOperationException(Res.GetString("Xdom_Node_Cloning"));
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000D95 RID: 3477 RVA: 0x0003C2F2 File Offset: 0x0003B2F2
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000D96 RID: 3478 RVA: 0x0003C2F5 File Offset: 0x0003B2F5
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0003C2FD File Offset: 0x0003B2FD
		public override string LocalName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000D98 RID: 3480 RVA: 0x0003C305 File Offset: 0x0003B305
		// (set) Token: 0x06000D99 RID: 3481 RVA: 0x0003C30D File Offset: 0x0003B30D
		public override string InnerText
		{
			get
			{
				return base.InnerText;
			}
			set
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Ent_Innertext"));
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000D9A RID: 3482 RVA: 0x0003C31E File Offset: 0x0003B31E
		internal override bool IsContainer
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000D9B RID: 3483 RVA: 0x0003C324 File Offset: 0x0003B324
		// (set) Token: 0x06000D9C RID: 3484 RVA: 0x0003C35B File Offset: 0x0003B35B
		internal override XmlLinkedNode LastNode
		{
			get
			{
				if (this.lastChild == null && !this.childrenFoliating)
				{
					this.childrenFoliating = true;
					XmlLoader xmlLoader = new XmlLoader();
					xmlLoader.ExpandEntity(this);
				}
				return this.lastChild;
			}
			set
			{
				this.lastChild = value;
			}
		}

		// Token: 0x06000D9D RID: 3485 RVA: 0x0003C364 File Offset: 0x0003B364
		internal override bool IsValidChildType(XmlNodeType type)
		{
			return type == XmlNodeType.Text || type == XmlNodeType.Element || type == XmlNodeType.ProcessingInstruction || type == XmlNodeType.Comment || type == XmlNodeType.CDATA || type == XmlNodeType.Whitespace || type == XmlNodeType.SignificantWhitespace || type == XmlNodeType.EntityReference;
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000D9E RID: 3486 RVA: 0x0003C38A File Offset: 0x0003B38A
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.Entity;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000D9F RID: 3487 RVA: 0x0003C38D File Offset: 0x0003B38D
		public string PublicId
		{
			get
			{
				return this.publicId;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000DA0 RID: 3488 RVA: 0x0003C395 File Offset: 0x0003B395
		public string SystemId
		{
			get
			{
				return this.systemId;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000DA1 RID: 3489 RVA: 0x0003C39D File Offset: 0x0003B39D
		public string NotationName
		{
			get
			{
				return this.notationName;
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x0003C3A5 File Offset: 0x0003B3A5
		public override string OuterXml
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000DA3 RID: 3491 RVA: 0x0003C3AC File Offset: 0x0003B3AC
		// (set) Token: 0x06000DA4 RID: 3492 RVA: 0x0003C3B3 File Offset: 0x0003B3B3
		public override string InnerXml
		{
			get
			{
				return string.Empty;
			}
			set
			{
				throw new InvalidOperationException(Res.GetString("Xdom_Set_InnerXml"));
			}
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0003C3C4 File Offset: 0x0003B3C4
		public override void WriteTo(XmlWriter w)
		{
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0003C3C6 File Offset: 0x0003B3C6
		public override void WriteContentTo(XmlWriter w)
		{
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000DA7 RID: 3495 RVA: 0x0003C3C8 File Offset: 0x0003B3C8
		public override string BaseURI
		{
			get
			{
				return this.baseURI;
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0003C3D0 File Offset: 0x0003B3D0
		internal void SetBaseURI(string inBaseURI)
		{
			this.baseURI = inBaseURI;
		}

		// Token: 0x0400095A RID: 2394
		private string publicId;

		// Token: 0x0400095B RID: 2395
		private string systemId;

		// Token: 0x0400095C RID: 2396
		private string notationName;

		// Token: 0x0400095D RID: 2397
		private string name;

		// Token: 0x0400095E RID: 2398
		private string unparsedReplacementStr;

		// Token: 0x0400095F RID: 2399
		private string baseURI;

		// Token: 0x04000960 RID: 2400
		private XmlLinkedNode lastChild;

		// Token: 0x04000961 RID: 2401
		private bool childrenFoliating;
	}
}
