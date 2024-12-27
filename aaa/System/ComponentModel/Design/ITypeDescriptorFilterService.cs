using System;
using System.Collections;

namespace System.ComponentModel.Design
{
	// Token: 0x02000190 RID: 400
	public interface ITypeDescriptorFilterService
	{
		// Token: 0x06000CA3 RID: 3235
		bool FilterAttributes(IComponent component, IDictionary attributes);

		// Token: 0x06000CA4 RID: 3236
		bool FilterEvents(IComponent component, IDictionary events);

		// Token: 0x06000CA5 RID: 3237
		bool FilterProperties(IComponent component, IDictionary properties);
	}
}
