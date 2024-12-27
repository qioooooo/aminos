using System;
using System.Xml.Schema;

namespace System.Xml
{
	// Token: 0x020000D6 RID: 214
	public class XmlDocumentType : XmlLinkedNode
	{
		// Token: 0x06000D12 RID: 3346 RVA: 0x0003A1DC File Offset: 0x000391DC
		protected internal XmlDocumentType(string name, string publicId, string systemId, string internalSubset, XmlDocument doc)
			: base(doc)
		{
			this.name = name;
			this.publicId = publicId;
			this.systemId = systemId;
			this.namespaces = true;
			this.internalSubset = internalSubset;
			if (!doc.IsLoading)
			{
				doc.IsLoading = true;
				XmlLoader xmlLoader = new XmlLoader();
				xmlLoader.ParseDocumentType(this);
				doc.IsLoading = false;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000D13 RID: 3347 RVA: 0x0003A23B File Offset: 0x0003923B
		public override string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000D14 RID: 3348 RVA: 0x0003A243 File Offset: 0x00039243
		public override string LocalName
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000D15 RID: 3349 RVA: 0x0003A24B File Offset: 0x0003924B
		public override XmlNodeType NodeType
		{
			get
			{
				return XmlNodeType.DocumentType;
			}
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x0003A24F File Offset: 0x0003924F
		public override XmlNode CloneNode(bool deep)
		{
			return this.OwnerDocument.CreateDocumentType(this.name, this.publicId, this.systemId, this.internalSubset);
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000D17 RID: 3351 RVA: 0x0003A274 File Offset: 0x00039274
		public override bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000D18 RID: 3352 RVA: 0x0003A277 File Offset: 0x00039277
		public XmlNamedNodeMap Entities
		{
			get
			{
				if (this.entities == null)
				{
					this.entities = new XmlNamedNodeMap(this);
				}
				return this.entities;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000D19 RID: 3353 RVA: 0x0003A293 File Offset: 0x00039293
		public XmlNamedNodeMap Notations
		{
			get
			{
				if (this.notations == null)
				{
					this.notations = new XmlNamedNodeMap(this);
				}
				return this.notations;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000D1A RID: 3354 RVA: 0x0003A2AF File Offset: 0x000392AF
		public string PublicId
		{
			get
			{
				return this.publicId;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000D1B RID: 3355 RVA: 0x0003A2B7 File Offset: 0x000392B7
		public string SystemId
		{
			get
			{
				return this.systemId;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000D1C RID: 3356 RVA: 0x0003A2BF File Offset: 0x000392BF
		public string InternalSubset
		{
			get
			{
				return this.internalSubset;
			}
		}

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000D1D RID: 3357 RVA: 0x0003A2C7 File Offset: 0x000392C7
		// (set) Token: 0x06000D1E RID: 3358 RVA: 0x0003A2CF File Offset: 0x000392CF
		internal bool ParseWithNamespaces
		{
			get
			{
				return this.namespaces;
			}
			set
			{
				this.namespaces = value;
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x0003A2D8 File Offset: 0x000392D8
		public override void WriteTo(XmlWriter w)
		{
			w.WriteDocType(this.name, this.publicId, this.systemId, this.internalSubset);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x0003A2F8 File Offset: 0x000392F8
		public override void WriteContentTo(XmlWriter w)
		{
		}

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000D21 RID: 3361 RVA: 0x0003A2FA File Offset: 0x000392FA
		// (set) Token: 0x06000D22 RID: 3362 RVA: 0x0003A302 File Offset: 0x00039302
		internal SchemaInfo DtdSchemaInfo
		{
			get
			{
				return this.schemaInfo;
			}
			set
			{
				this.schemaInfo = value;
			}
		}

		// Token: 0x04000929 RID: 2345
		private string name;

		// Token: 0x0400092A RID: 2346
		private string publicId;

		// Token: 0x0400092B RID: 2347
		private string systemId;

		// Token: 0x0400092C RID: 2348
		private string internalSubset;

		// Token: 0x0400092D RID: 2349
		private bool namespaces;

		// Token: 0x0400092E RID: 2350
		private XmlNamedNodeMap entities;

		// Token: 0x0400092F RID: 2351
		private XmlNamedNodeMap notations;

		// Token: 0x04000930 RID: 2352
		private SchemaInfo schemaInfo;
	}
}
