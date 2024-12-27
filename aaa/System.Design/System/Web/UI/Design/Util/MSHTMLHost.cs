using System;
using System.Design;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	// Token: 0x020003C9 RID: 969
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class MSHTMLHost : Control
	{
		// Token: 0x06002372 RID: 9074 RVA: 0x000BF0A0 File Offset: 0x000BE0A0
		public NativeMethods.IHTMLDocument2 GetDocument()
		{
			return this.tridentSite.GetDocument();
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x06002373 RID: 9075 RVA: 0x000BF0B0 File Offset: 0x000BE0B0
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 131072;
				return createParams;
			}
		}

		// Token: 0x06002374 RID: 9076 RVA: 0x000BF0D8 File Offset: 0x000BE0D8
		public bool CreateTrident()
		{
			try
			{
				this.tridentSite = new TridentSite(this);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		// Token: 0x06002375 RID: 9077 RVA: 0x000BF10C File Offset: 0x000BE10C
		public void ActivateTrident()
		{
			this.tridentSite.Activate();
		}

		// Token: 0x040018A1 RID: 6305
		private TridentSite tridentSite;
	}
}
