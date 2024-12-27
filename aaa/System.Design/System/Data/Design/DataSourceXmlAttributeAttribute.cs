using System;

namespace System.Data.Design
{
	// Token: 0x0200007C RID: 124
	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class DataSourceXmlAttributeAttribute : DataSourceXmlSerializationAttribute
	{
		// Token: 0x06000535 RID: 1333 RVA: 0x0000932B File Offset: 0x0000832B
		internal DataSourceXmlAttributeAttribute()
			: this(null)
		{
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x00009334 File Offset: 0x00008334
		internal DataSourceXmlAttributeAttribute(string attributeName)
		{
			base.Name = attributeName;
		}
	}
}
