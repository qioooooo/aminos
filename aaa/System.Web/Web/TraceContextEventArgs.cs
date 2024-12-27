using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x0200047B RID: 1147
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class TraceContextEventArgs : EventArgs
	{
		// Token: 0x060035F4 RID: 13812 RVA: 0x000E988A File Offset: 0x000E888A
		public TraceContextEventArgs(ICollection records)
		{
			this._records = records;
		}

		// Token: 0x17000C0F RID: 3087
		// (get) Token: 0x060035F5 RID: 13813 RVA: 0x000E9899 File Offset: 0x000E8899
		public ICollection TraceRecords
		{
			get
			{
				return this._records;
			}
		}

		// Token: 0x0400256D RID: 9581
		private ICollection _records;
	}
}
