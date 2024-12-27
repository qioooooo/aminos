using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x0200017B RID: 379
	public interface IComponentInitializer
	{
		// Token: 0x06000C2E RID: 3118
		void InitializeExistingComponent(IDictionary defaultValues);

		// Token: 0x06000C2F RID: 3119
		void InitializeNewComponent(IDictionary defaultValues);
	}
}
