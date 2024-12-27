using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.ComponentModel.Design
{
	// Token: 0x02000183 RID: 387
	[ComVisible(true)]
	public interface IEventBindingService
	{
		// Token: 0x06000C6A RID: 3178
		string CreateUniqueMethodName(IComponent component, EventDescriptor e);

		// Token: 0x06000C6B RID: 3179
		ICollection GetCompatibleMethods(EventDescriptor e);

		// Token: 0x06000C6C RID: 3180
		EventDescriptor GetEvent(PropertyDescriptor property);

		// Token: 0x06000C6D RID: 3181
		PropertyDescriptorCollection GetEventProperties(EventDescriptorCollection events);

		// Token: 0x06000C6E RID: 3182
		PropertyDescriptor GetEventProperty(EventDescriptor e);

		// Token: 0x06000C6F RID: 3183
		bool ShowCode();

		// Token: 0x06000C70 RID: 3184
		bool ShowCode(int lineNumber);

		// Token: 0x06000C71 RID: 3185
		bool ShowCode(IComponent component, EventDescriptor e);
	}
}
