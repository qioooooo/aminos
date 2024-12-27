using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006A2 RID: 1698
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class PersonalizationState
	{
		// Token: 0x06005301 RID: 21249 RVA: 0x0015048D File Offset: 0x0014F48D
		protected PersonalizationState(WebPartManager webPartManager)
		{
			if (webPartManager == null)
			{
				throw new ArgumentNullException("webPartManager");
			}
			this._webPartManager = webPartManager;
		}

		// Token: 0x17001520 RID: 5408
		// (get) Token: 0x06005302 RID: 21250 RVA: 0x001504AA File Offset: 0x0014F4AA
		public bool IsDirty
		{
			get
			{
				return this._dirty;
			}
		}

		// Token: 0x17001521 RID: 5409
		// (get) Token: 0x06005303 RID: 21251
		public abstract bool IsEmpty { get; }

		// Token: 0x17001522 RID: 5410
		// (get) Token: 0x06005304 RID: 21252 RVA: 0x001504B2 File Offset: 0x0014F4B2
		public WebPartManager WebPartManager
		{
			get
			{
				return this._webPartManager;
			}
		}

		// Token: 0x06005305 RID: 21253
		public abstract void ApplyWebPartPersonalization(WebPart webPart);

		// Token: 0x06005306 RID: 21254
		public abstract void ApplyWebPartManagerPersonalization();

		// Token: 0x06005307 RID: 21255
		public abstract void ExtractWebPartPersonalization(WebPart webPart);

		// Token: 0x06005308 RID: 21256
		public abstract void ExtractWebPartManagerPersonalization();

		// Token: 0x06005309 RID: 21257
		public abstract string GetAuthorizationFilter(string webPartID);

		// Token: 0x0600530A RID: 21258 RVA: 0x001504BA File Offset: 0x0014F4BA
		protected void SetDirty()
		{
			this._dirty = true;
		}

		// Token: 0x0600530B RID: 21259
		public abstract void SetWebPartDirty(WebPart webPart);

		// Token: 0x0600530C RID: 21260
		public abstract void SetWebPartManagerDirty();

		// Token: 0x0600530D RID: 21261 RVA: 0x001504C3 File Offset: 0x0014F4C3
		protected void ValidateWebPart(WebPart webPart)
		{
			if (webPart == null)
			{
				throw new ArgumentNullException("webPart");
			}
			if (!this._webPartManager.WebParts.Contains(webPart))
			{
				throw new ArgumentException(SR.GetString("UnknownWebPart"), "webPart");
			}
		}

		// Token: 0x04002E3B RID: 11835
		private WebPartManager _webPartManager;

		// Token: 0x04002E3C RID: 11836
		private bool _dirty;
	}
}
