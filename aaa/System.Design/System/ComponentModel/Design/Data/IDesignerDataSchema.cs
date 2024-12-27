using System;
using System.Collections;

namespace System.ComponentModel.Design.Data
{
	// Token: 0x0200014D RID: 333
	public interface IDesignerDataSchema
	{
		// Token: 0x06000CB1 RID: 3249
		ICollection GetSchemaItems(DesignerDataSchemaClass schemaClass);

		// Token: 0x06000CB2 RID: 3250
		bool SupportsSchemaClass(DesignerDataSchemaClass schemaClass);
	}
}
