using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x0200071F RID: 1823
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class WebPartDisplayMode
	{
		// Token: 0x06005876 RID: 22646 RVA: 0x00163BE7 File Offset: 0x00162BE7
		protected WebPartDisplayMode(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("name");
			}
			this._name = name;
		}

		// Token: 0x170016EA RID: 5866
		// (get) Token: 0x06005877 RID: 22647 RVA: 0x00163C09 File Offset: 0x00162C09
		public virtual bool AllowPageDesign
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016EB RID: 5867
		// (get) Token: 0x06005878 RID: 22648 RVA: 0x00163C0C File Offset: 0x00162C0C
		public virtual bool AssociatedWithToolZone
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016EC RID: 5868
		// (get) Token: 0x06005879 RID: 22649 RVA: 0x00163C0F File Offset: 0x00162C0F
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x170016ED RID: 5869
		// (get) Token: 0x0600587A RID: 22650 RVA: 0x00163C17 File Offset: 0x00162C17
		public virtual bool RequiresPersonalization
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170016EE RID: 5870
		// (get) Token: 0x0600587B RID: 22651 RVA: 0x00163C1A File Offset: 0x00162C1A
		public virtual bool ShowHiddenWebParts
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x00163C1D File Offset: 0x00162C1D
		public virtual bool IsEnabled(WebPartManager webPartManager)
		{
			return !this.RequiresPersonalization || webPartManager.Personalization.IsModifiable;
		}

		// Token: 0x04002FE7 RID: 12263
		private string _name;
	}
}
