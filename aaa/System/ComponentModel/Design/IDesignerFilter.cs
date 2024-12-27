using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x0200017E RID: 382
	public interface IDesignerFilter
	{
		// Token: 0x06000C3E RID: 3134
		void PostFilterAttributes(IDictionary attributes);

		// Token: 0x06000C3F RID: 3135
		void PostFilterEvents(IDictionary events);

		// Token: 0x06000C40 RID: 3136
		void PostFilterProperties(IDictionary properties);

		// Token: 0x06000C41 RID: 3137
		void PreFilterAttributes(IDictionary attributes);

		// Token: 0x06000C42 RID: 3138
		void PreFilterEvents(IDictionary events);

		// Token: 0x06000C43 RID: 3139
		void PreFilterProperties(IDictionary properties);
	}
}
