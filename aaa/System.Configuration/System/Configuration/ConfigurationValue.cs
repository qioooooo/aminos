using System;

namespace System.Configuration
{
	// Token: 0x02000041 RID: 65
	internal class ConfigurationValue
	{
		// Token: 0x06000309 RID: 777 RVA: 0x00011C63 File Offset: 0x00010C63
		internal ConfigurationValue(object value, ConfigurationValueFlags valueFlags, PropertySourceInfo sourceInfo)
		{
			this.Value = value;
			this.ValueFlags = valueFlags;
			this.SourceInfo = sourceInfo;
		}

		// Token: 0x040002A8 RID: 680
		internal ConfigurationValueFlags ValueFlags;

		// Token: 0x040002A9 RID: 681
		internal object Value;

		// Token: 0x040002AA RID: 682
		internal PropertySourceInfo SourceInfo;
	}
}
