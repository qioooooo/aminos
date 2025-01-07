using System;
using System.Design;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design.Util
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class MSHTMLHost : Control
	{
		public NativeMethods.IHTMLDocument2 GetDocument()
		{
			return this.tridentSite.GetDocument();
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 131072;
				return createParams;
			}
		}

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

		public void ActivateTrident()
		{
			this.tridentSite.Activate();
		}

		private TridentSite tridentSite;
	}
}
