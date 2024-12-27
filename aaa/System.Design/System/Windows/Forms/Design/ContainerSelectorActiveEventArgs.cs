using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001CA RID: 458
	internal class ContainerSelectorActiveEventArgs : EventArgs
	{
		// Token: 0x060011E4 RID: 4580 RVA: 0x000571C9 File Offset: 0x000561C9
		public ContainerSelectorActiveEventArgs(object component)
			: this(component, ContainerSelectorActiveEventArgsType.Mouse)
		{
		}

		// Token: 0x060011E5 RID: 4581 RVA: 0x000571D3 File Offset: 0x000561D3
		public ContainerSelectorActiveEventArgs(object component, ContainerSelectorActiveEventArgsType eventType)
		{
			this.component = component;
			this.eventType = eventType;
		}

		// Token: 0x040010F0 RID: 4336
		private readonly object component;

		// Token: 0x040010F1 RID: 4337
		private readonly ContainerSelectorActiveEventArgsType eventType;
	}
}
