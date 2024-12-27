using System;

namespace System.Xml.Schema
{
	// Token: 0x02000206 RID: 518
	internal class RedefineEntry
	{
		// Token: 0x06001879 RID: 6265 RVA: 0x0006E0A8 File Offset: 0x0006D0A8
		public RedefineEntry(XmlSchemaRedefine external, XmlSchema schema)
		{
			this.redefine = external;
			this.schemaToUpdate = schema;
		}

		// Token: 0x04000E70 RID: 3696
		internal XmlSchemaRedefine redefine;

		// Token: 0x04000E71 RID: 3697
		internal XmlSchema schemaToUpdate;
	}
}
