using System;

namespace System.Data.SqlTypes
{
	// Token: 0x02000361 RID: 865
	public sealed class TypeVarImageSchemaImporterExtension : SqlTypesSchemaImporterExtensionHelper
	{
		// Token: 0x06002F2A RID: 12074 RVA: 0x002AFBB0 File Offset: 0x002AEFB0
		public TypeVarImageSchemaImporterExtension()
			: base("image", "System.Data.SqlTypes.SqlBinary", false)
		{
		}
	}
}
