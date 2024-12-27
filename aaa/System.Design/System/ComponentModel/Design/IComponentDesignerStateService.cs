using System;

namespace System.ComponentModel.Design
{
	// Token: 0x0200012E RID: 302
	public interface IComponentDesignerStateService
	{
		// Token: 0x06000BCD RID: 3021
		object GetState(IComponent component, string key);

		// Token: 0x06000BCE RID: 3022
		void SetState(IComponent component, string key, object value);
	}
}
