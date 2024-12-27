using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200012A RID: 298
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class PropertyChangingEventArgs : EventArgs
	{
		// Token: 0x06000970 RID: 2416 RVA: 0x0001F729 File Offset: 0x0001E729
		public PropertyChangingEventArgs(string propertyName)
		{
			this.propertyName = propertyName;
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x06000971 RID: 2417 RVA: 0x0001F738 File Offset: 0x0001E738
		public virtual string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x04000A0F RID: 2575
		private readonly string propertyName;
	}
}
