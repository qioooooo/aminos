using System;
using System.Security;
using System.Security.Permissions;

namespace System.Transactions
{
	// Token: 0x02000070 RID: 112
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public sealed class DistributedTransactionPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00032988 File Offset: 0x00031D88
		// (set) Token: 0x06000320 RID: 800 RVA: 0x0003299C File Offset: 0x00031D9C
		public new bool Unrestricted
		{
			get
			{
				return this.unrestricted;
			}
			set
			{
				this.unrestricted = value;
			}
		}

		// Token: 0x06000321 RID: 801 RVA: 0x000329B0 File Offset: 0x00031DB0
		public DistributedTransactionPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x06000322 RID: 802 RVA: 0x000329C4 File Offset: 0x00031DC4
		public override IPermission CreatePermission()
		{
			if (this.Unrestricted)
			{
				return new DistributedTransactionPermission(PermissionState.Unrestricted);
			}
			return new DistributedTransactionPermission(PermissionState.None);
		}

		// Token: 0x04000146 RID: 326
		private bool unrestricted;
	}
}
