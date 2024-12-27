using System;

namespace System.Xml.Serialization
{
	// Token: 0x02000346 RID: 838
	public class UnreferencedObjectEventArgs : EventArgs
	{
		// Token: 0x060028CB RID: 10443 RVA: 0x000D1EAA File Offset: 0x000D0EAA
		public UnreferencedObjectEventArgs(object o, string id)
		{
			this.o = o;
			this.id = id;
		}

		// Token: 0x170009B1 RID: 2481
		// (get) Token: 0x060028CC RID: 10444 RVA: 0x000D1EC0 File Offset: 0x000D0EC0
		public object UnreferencedObject
		{
			get
			{
				return this.o;
			}
		}

		// Token: 0x170009B2 RID: 2482
		// (get) Token: 0x060028CD RID: 10445 RVA: 0x000D1EC8 File Offset: 0x000D0EC8
		public string UnreferencedId
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x0400169A RID: 5786
		private object o;

		// Token: 0x0400169B RID: 5787
		private string id;
	}
}
