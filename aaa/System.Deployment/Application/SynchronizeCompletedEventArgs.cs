using System;
using System.ComponentModel;

namespace System.Deployment.Application
{
	// Token: 0x02000040 RID: 64
	internal class SynchronizeCompletedEventArgs : AsyncCompletedEventArgs
	{
		// Token: 0x06000215 RID: 533 RVA: 0x0000DC6F File Offset: 0x0000CC6F
		internal SynchronizeCompletedEventArgs(Exception error, bool cancelled, object userState, string groupName)
			: base(error, cancelled, userState)
		{
			this._groupName = groupName;
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000216 RID: 534 RVA: 0x0000DC82 File Offset: 0x0000CC82
		public string Group
		{
			get
			{
				return this._groupName;
			}
		}

		// Token: 0x040001C3 RID: 451
		private readonly string _groupName;
	}
}
