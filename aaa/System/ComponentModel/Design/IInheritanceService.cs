using System;

namespace System.ComponentModel.Design
{
	// Token: 0x02000187 RID: 391
	public interface IInheritanceService
	{
		// Token: 0x06000C7C RID: 3196
		void AddInheritedComponents(IComponent component, IContainer container);

		// Token: 0x06000C7D RID: 3197
		InheritanceAttribute GetInheritanceAttribute(IComponent component);
	}
}
