using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200024C RID: 588
	public abstract class XmlSchemaExternal : XmlSchemaObject
	{
		// Token: 0x17000736 RID: 1846
		// (get) Token: 0x06001C30 RID: 7216 RVA: 0x00082F21 File Offset: 0x00081F21
		// (set) Token: 0x06001C31 RID: 7217 RVA: 0x00082F29 File Offset: 0x00081F29
		[XmlAttribute("schemaLocation", DataType = "anyURI")]
		public string SchemaLocation
		{
			get
			{
				return this.location;
			}
			set
			{
				this.location = value;
			}
		}

		// Token: 0x17000737 RID: 1847
		// (get) Token: 0x06001C32 RID: 7218 RVA: 0x00082F32 File Offset: 0x00081F32
		// (set) Token: 0x06001C33 RID: 7219 RVA: 0x00082F3A File Offset: 0x00081F3A
		[XmlIgnore]
		public XmlSchema Schema
		{
			get
			{
				return this.schema;
			}
			set
			{
				this.schema = value;
			}
		}

		// Token: 0x17000738 RID: 1848
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x00082F43 File Offset: 0x00081F43
		// (set) Token: 0x06001C35 RID: 7221 RVA: 0x00082F4B File Offset: 0x00081F4B
		[XmlAttribute("id", DataType = "ID")]
		public string Id
		{
			get
			{
				return this.id;
			}
			set
			{
				this.id = value;
			}
		}

		// Token: 0x17000739 RID: 1849
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x00082F54 File Offset: 0x00081F54
		// (set) Token: 0x06001C37 RID: 7223 RVA: 0x00082F5C File Offset: 0x00081F5C
		[XmlAnyAttribute]
		public XmlAttribute[] UnhandledAttributes
		{
			get
			{
				return this.moreAttributes;
			}
			set
			{
				this.moreAttributes = value;
			}
		}

		// Token: 0x1700073A RID: 1850
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x00082F65 File Offset: 0x00081F65
		// (set) Token: 0x06001C39 RID: 7225 RVA: 0x00082F6D File Offset: 0x00081F6D
		[XmlIgnore]
		internal Uri BaseUri
		{
			get
			{
				return this.baseUri;
			}
			set
			{
				this.baseUri = value;
			}
		}

		// Token: 0x1700073B RID: 1851
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x00082F76 File Offset: 0x00081F76
		// (set) Token: 0x06001C3B RID: 7227 RVA: 0x00082F7E File Offset: 0x00081F7E
		[XmlIgnore]
		internal override string IdAttribute
		{
			get
			{
				return this.Id;
			}
			set
			{
				this.Id = value;
			}
		}

		// Token: 0x06001C3C RID: 7228 RVA: 0x00082F87 File Offset: 0x00081F87
		internal override void SetUnhandledAttributes(XmlAttribute[] moreAttributes)
		{
			this.moreAttributes = moreAttributes;
		}

		// Token: 0x1700073C RID: 1852
		// (get) Token: 0x06001C3D RID: 7229 RVA: 0x00082F90 File Offset: 0x00081F90
		// (set) Token: 0x06001C3E RID: 7230 RVA: 0x00082F98 File Offset: 0x00081F98
		internal Compositor Compositor
		{
			get
			{
				return this.compositor;
			}
			set
			{
				this.compositor = value;
			}
		}

		// Token: 0x04001167 RID: 4455
		private string location;

		// Token: 0x04001168 RID: 4456
		private Uri baseUri;

		// Token: 0x04001169 RID: 4457
		private XmlSchema schema;

		// Token: 0x0400116A RID: 4458
		private string id;

		// Token: 0x0400116B RID: 4459
		private XmlAttribute[] moreAttributes;

		// Token: 0x0400116C RID: 4460
		private Compositor compositor;
	}
}
