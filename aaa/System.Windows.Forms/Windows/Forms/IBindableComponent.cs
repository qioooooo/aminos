using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020001E9 RID: 489
	public interface IBindableComponent : IComponent, IDisposable
	{
		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x0600134C RID: 4940
		ControlBindingsCollection DataBindings { get; }

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600134D RID: 4941
		// (set) Token: 0x0600134E RID: 4942
		BindingContext BindingContext { get; set; }
	}
}
