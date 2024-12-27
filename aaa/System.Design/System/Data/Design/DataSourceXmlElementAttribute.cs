using System;

namespace System.Data.Design
{
	// Token: 0x0200007E RID: 126
	[AttributeUsage(AttributeTargets.Property)]
	internal sealed class DataSourceXmlElementAttribute : DataSourceXmlSerializationAttribute
	{
		// Token: 0x0600053A RID: 1338 RVA: 0x00009363 File Offset: 0x00008363
		internal DataSourceXmlElementAttribute()
			: this(null)
		{
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0000936C File Offset: 0x0000836C
		internal DataSourceXmlElementAttribute(string elementName)
		{
			base.Name = elementName;
		}
	}
}
