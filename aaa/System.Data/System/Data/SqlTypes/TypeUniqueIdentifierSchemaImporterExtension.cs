using System;

namespace System.Data.SqlTypes
{
	// Token: 0x0200036F RID: 879
	public sealed class TypeUniqueIdentifierSchemaImporterExtension : SqlTypesSchemaImporterExtensionHelper
	{
		// Token: 0x06002F38 RID: 12088 RVA: 0x002AFD70 File Offset: 0x002AF170
		public TypeUniqueIdentifierSchemaImporterExtension()
			: base("uniqueidentifier", "System.Data.SqlTypes.SqlGuid")
		{
		}
	}
}
