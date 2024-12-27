using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200016B RID: 363
	[ComVisible(true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	public class DesignerTransactionCloseEventArgs : EventArgs
	{
		// Token: 0x06000BC6 RID: 3014 RVA: 0x000289DF File Offset: 0x000279DF
		[Obsolete("This constructor is obsolete. Use DesignerTransactionCloseEventArgs(bool, bool) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
		public DesignerTransactionCloseEventArgs(bool commit)
			: this(commit, true)
		{
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x000289E9 File Offset: 0x000279E9
		public DesignerTransactionCloseEventArgs(bool commit, bool lastTransaction)
		{
			this.commit = commit;
			this.lastTransaction = lastTransaction;
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x000289FF File Offset: 0x000279FF
		public bool TransactionCommitted
		{
			get
			{
				return this.commit;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000BC9 RID: 3017 RVA: 0x00028A07 File Offset: 0x00027A07
		public bool LastTransaction
		{
			get
			{
				return this.lastTransaction;
			}
		}

		// Token: 0x04000ABC RID: 2748
		private bool commit;

		// Token: 0x04000ABD RID: 2749
		private bool lastTransaction;
	}
}
