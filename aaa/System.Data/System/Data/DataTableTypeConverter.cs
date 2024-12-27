using System;
using System.ComponentModel;

namespace System.Data
{
	// Token: 0x020000A5 RID: 165
	internal sealed class DataTableTypeConverter : ReferenceConverter
	{
		// Token: 0x06000B1C RID: 2844 RVA: 0x001F6164 File Offset: 0x001F5564
		public DataTableTypeConverter()
			: base(typeof(DataTable))
		{
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x001F6184 File Offset: 0x001F5584
		public override bool GetPropertiesSupported(ITypeDescriptorContext context)
		{
			return false;
		}
	}
}
