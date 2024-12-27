using System;

namespace System.Data.SqlTypes
{
	// Token: 0x0200035B RID: 859
	public sealed class TypeVarCharSchemaImporterExtension : SqlTypesSchemaImporterExtensionHelper
	{
		// Token: 0x06002F24 RID: 12068 RVA: 0x002AFAF0 File Offset: 0x002AEEF0
		public TypeVarCharSchemaImporterExtension()
			: base("varchar", "System.Data.SqlTypes.SqlString", false)
		{
		}
	}
}
