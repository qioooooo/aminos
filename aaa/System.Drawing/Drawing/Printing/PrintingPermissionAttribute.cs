using System;
using System.Security;
using System.Security.Permissions;

namespace System.Drawing.Printing
{
	// Token: 0x02000124 RID: 292
	[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
	public sealed class PrintingPermissionAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x06000F49 RID: 3913 RVA: 0x0002DBDC File Offset: 0x0002CBDC
		public PrintingPermissionAttribute(SecurityAction action)
			: base(action)
		{
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x06000F4A RID: 3914 RVA: 0x0002DBE5 File Offset: 0x0002CBE5
		// (set) Token: 0x06000F4B RID: 3915 RVA: 0x0002DBED File Offset: 0x0002CBED
		public PrintingPermissionLevel Level
		{
			get
			{
				return this.level;
			}
			set
			{
				if (value < PrintingPermissionLevel.NoPrinting || value > PrintingPermissionLevel.AllPrinting)
				{
					throw new ArgumentException(SR.GetString("PrintingPermissionAttributeInvalidPermissionLevel"), "value");
				}
				this.level = value;
			}
		}

		// Token: 0x06000F4C RID: 3916 RVA: 0x0002DC13 File Offset: 0x0002CC13
		public override IPermission CreatePermission()
		{
			if (base.Unrestricted)
			{
				return new PrintingPermission(PermissionState.Unrestricted);
			}
			return new PrintingPermission(this.level);
		}

		// Token: 0x04000C68 RID: 3176
		private PrintingPermissionLevel level;
	}
}
