using System;
using System.Security.Permissions;

namespace System.ComponentModel
{
	// Token: 0x0200008B RID: 139
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class AddingNewEventArgs : EventArgs
	{
		// Token: 0x060004DC RID: 1244 RVA: 0x00015914 File Offset: 0x00014914
		public AddingNewEventArgs()
		{
		}

		// Token: 0x060004DD RID: 1245 RVA: 0x0001591C File Offset: 0x0001491C
		public AddingNewEventArgs(object newObject)
		{
			this.newObject = newObject;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x0001592B File Offset: 0x0001492B
		// (set) Token: 0x060004DF RID: 1247 RVA: 0x00015933 File Offset: 0x00014933
		public object NewObject
		{
			get
			{
				return this.newObject;
			}
			set
			{
				this.newObject = value;
			}
		}

		// Token: 0x040008B4 RID: 2228
		private object newObject;
	}
}
