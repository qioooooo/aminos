using System;

namespace System.Data.SqlTypes
{
	// Token: 0x02000363 RID: 867
	public sealed class TypeNumericSchemaImporterExtension : SqlTypesSchemaImporterExtensionHelper
	{
		// Token: 0x06002F2C RID: 12076 RVA: 0x002AFBF0 File Offset: 0x002AEFF0
		public TypeNumericSchemaImporterExtension()
			: base("numeric", "System.Data.SqlTypes.SqlDecimal", false)
		{
		}
	}
}
