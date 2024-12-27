using System;

namespace System.Data.SqlTypes
{
	// Token: 0x0200035C RID: 860
	public sealed class TypeNVarCharSchemaImporterExtension : SqlTypesSchemaImporterExtensionHelper
	{
		// Token: 0x06002F25 RID: 12069 RVA: 0x002AFB10 File Offset: 0x002AEF10
		public TypeNVarCharSchemaImporterExtension()
			: base("nvarchar", "System.Data.SqlTypes.SqlString", false)
		{
		}
	}
}
