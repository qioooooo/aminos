using System;

namespace System.Windows.Forms.Design
{
	internal class ContainerSelectorActiveEventArgs : EventArgs
	{
		public ContainerSelectorActiveEventArgs(object component)
			: this(component, ContainerSelectorActiveEventArgsType.Mouse)
		{
		}

		public ContainerSelectorActiveEventArgs(object component, ContainerSelectorActiveEventArgsType eventType)
		{
			this.component = component;
			this.eventType = eventType;
		}

		private readonly object component;

		private readonly ContainerSelectorActiveEventArgsType eventType;
	}
}
