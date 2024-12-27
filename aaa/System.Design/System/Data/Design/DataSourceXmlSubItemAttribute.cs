using System;

namespace System.Data.Design
{
	// Token: 0x02000082 RID: 130
	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class DataSourceXmlSubItemAttribute : DataSourceXmlSerializationAttribute
	{
		// Token: 0x06000548 RID: 1352 RVA: 0x00009D49 File Offset: 0x00008D49
		internal DataSourceXmlSubItemAttribute()
		{
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x00009D51 File Offset: 0x00008D51
		internal DataSourceXmlSubItemAttribute(Type itemType)
		{
			base.ItemType = itemType;
		}
	}
}
