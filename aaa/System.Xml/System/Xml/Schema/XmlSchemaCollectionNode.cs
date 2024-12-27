using System;

namespace System.Xml.Schema
{
	// Token: 0x0200023D RID: 573
	internal sealed class XmlSchemaCollectionNode
	{
		// Token: 0x170006DC RID: 1756
		// (get) Token: 0x06001B64 RID: 7012 RVA: 0x0008195B File Offset: 0x0008095B
		// (set) Token: 0x06001B65 RID: 7013 RVA: 0x00081963 File Offset: 0x00080963
		internal string NamespaceURI
		{
			get
			{
				return this.namespaceUri;
			}
			set
			{
				this.namespaceUri = value;
			}
		}

		// Token: 0x170006DD RID: 1757
		// (get) Token: 0x06001B66 RID: 7014 RVA: 0x0008196C File Offset: 0x0008096C
		// (set) Token: 0x06001B67 RID: 7015 RVA: 0x00081974 File Offset: 0x00080974
		internal SchemaInfo SchemaInfo
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

		// Token: 0x170006DE RID: 1758
		// (get) Token: 0x06001B68 RID: 7016 RVA: 0x0008197D File Offset: 0x0008097D
		// (set) Token: 0x06001B69 RID: 7017 RVA: 0x00081985 File Offset: 0x00080985
		internal XmlSchema Schema
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

		// Token: 0x04001109 RID: 4361
		private string namespaceUri;

		// Token: 0x0400110A RID: 4362
		private SchemaInfo schemaInfo;

		// Token: 0x0400110B RID: 4363
		private XmlSchema schema;
	}
}
