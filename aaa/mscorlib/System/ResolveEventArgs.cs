using System;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x0200004C RID: 76
	[ComVisible(true)]
	public class ResolveEventArgs : EventArgs
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06000423 RID: 1059 RVA: 0x000109F3 File Offset: 0x0000F9F3
		public string Name
		{
			get
			{
				return this._Name;
			}
		}

		// Token: 0x06000424 RID: 1060 RVA: 0x000109FB File Offset: 0x0000F9FB
		public ResolveEventArgs(string name)
		{
			this._Name = name;
		}

		// Token: 0x0400018E RID: 398
		private string _Name;
	}
}
