using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x02000267 RID: 615
	public class XmlSchemaNotation : XmlSchemaAnnotated
	{
		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06001C9F RID: 7327 RVA: 0x00083491 File Offset: 0x00082491
		// (set) Token: 0x06001CA0 RID: 7328 RVA: 0x00083499 File Offset: 0x00082499
		[XmlAttribute("name")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06001CA1 RID: 7329 RVA: 0x000834A2 File Offset: 0x000824A2
		// (set) Token: 0x06001CA2 RID: 7330 RVA: 0x000834AA File Offset: 0x000824AA
		[XmlAttribute("public")]
		public string Public
		{
			get
			{
				return this.publicId;
			}
			set
			{
				this.publicId = value;
			}
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06001CA3 RID: 7331 RVA: 0x000834B3 File Offset: 0x000824B3
		// (set) Token: 0x06001CA4 RID: 7332 RVA: 0x000834BB File Offset: 0x000824BB
		[XmlAttribute("system")]
		public string System
		{
			get
			{
				return this.systemId;
			}
			set
			{
				this.systemId = value;
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06001CA5 RID: 7333 RVA: 0x000834C4 File Offset: 0x000824C4
		// (set) Token: 0x06001CA6 RID: 7334 RVA: 0x000834CC File Offset: 0x000824CC
		[XmlIgnore]
		internal XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qname;
			}
			set
			{
				this.qname = value;
			}
		}

		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06001CA7 RID: 7335 RVA: 0x000834D5 File Offset: 0x000824D5
		// (set) Token: 0x06001CA8 RID: 7336 RVA: 0x000834DD File Offset: 0x000824DD
		[XmlIgnore]
		internal override string NameAttribute
		{
			get
			{
				return this.Name;
			}
			set
			{
				this.Name = value;
			}
		}

		// Token: 0x0400119D RID: 4509
		private string name;

		// Token: 0x0400119E RID: 4510
		private string publicId;

		// Token: 0x0400119F RID: 4511
		private string systemId;

		// Token: 0x040011A0 RID: 4512
		private XmlQualifiedName qname = XmlQualifiedName.Empty;
	}
}
