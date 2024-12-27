using System;
using System.Xml.Serialization;

namespace System.Xml.Schema
{
	// Token: 0x0200025F RID: 607
	public class XmlSchemaIdentityConstraint : XmlSchemaAnnotated
	{
		// Token: 0x1700074A RID: 1866
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x000831DD File Offset: 0x000821DD
		// (set) Token: 0x06001C6C RID: 7276 RVA: 0x000831E5 File Offset: 0x000821E5
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

		// Token: 0x1700074B RID: 1867
		// (get) Token: 0x06001C6D RID: 7277 RVA: 0x000831EE File Offset: 0x000821EE
		// (set) Token: 0x06001C6E RID: 7278 RVA: 0x000831F6 File Offset: 0x000821F6
		[XmlElement("selector", typeof(XmlSchemaXPath))]
		public XmlSchemaXPath Selector
		{
			get
			{
				return this.selector;
			}
			set
			{
				this.selector = value;
			}
		}

		// Token: 0x1700074C RID: 1868
		// (get) Token: 0x06001C6F RID: 7279 RVA: 0x000831FF File Offset: 0x000821FF
		[XmlElement("field", typeof(XmlSchemaXPath))]
		public XmlSchemaObjectCollection Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x1700074D RID: 1869
		// (get) Token: 0x06001C70 RID: 7280 RVA: 0x00083207 File Offset: 0x00082207
		[XmlIgnore]
		public XmlQualifiedName QualifiedName
		{
			get
			{
				return this.qualifiedName;
			}
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x0008320F File Offset: 0x0008220F
		internal void SetQualifiedName(XmlQualifiedName value)
		{
			this.qualifiedName = value;
		}

		// Token: 0x1700074E RID: 1870
		// (get) Token: 0x06001C72 RID: 7282 RVA: 0x00083218 File Offset: 0x00082218
		// (set) Token: 0x06001C73 RID: 7283 RVA: 0x00083220 File Offset: 0x00082220
		[XmlIgnore]
		internal CompiledIdentityConstraint CompiledConstraint
		{
			get
			{
				return this.compiledConstraint;
			}
			set
			{
				this.compiledConstraint = value;
			}
		}

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06001C74 RID: 7284 RVA: 0x00083229 File Offset: 0x00082229
		// (set) Token: 0x06001C75 RID: 7285 RVA: 0x00083231 File Offset: 0x00082231
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

		// Token: 0x0400118B RID: 4491
		private string name;

		// Token: 0x0400118C RID: 4492
		private XmlSchemaXPath selector;

		// Token: 0x0400118D RID: 4493
		private XmlSchemaObjectCollection fields = new XmlSchemaObjectCollection();

		// Token: 0x0400118E RID: 4494
		private XmlQualifiedName qualifiedName = XmlQualifiedName.Empty;

		// Token: 0x0400118F RID: 4495
		private CompiledIdentityConstraint compiledConstraint;
	}
}
