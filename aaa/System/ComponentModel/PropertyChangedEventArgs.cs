using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x02000128 RID: 296
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class PropertyChangedEventArgs : EventArgs
	{
		// Token: 0x0600096A RID: 2410 RVA: 0x0001F712 File Offset: 0x0001E712
		public PropertyChangedEventArgs(string propertyName)
		{
			this.propertyName = propertyName;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0001F721 File Offset: 0x0001E721
		public virtual string PropertyName
		{
			get
			{
				return this.propertyName;
			}
		}

		// Token: 0x04000A0E RID: 2574
		private readonly string propertyName;
	}
}
