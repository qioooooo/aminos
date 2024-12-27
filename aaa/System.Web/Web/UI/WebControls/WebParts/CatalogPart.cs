using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006A7 RID: 1703
	[Bindable(false)]
	[Designer("System.Web.UI.Design.WebControls.WebParts.CatalogPartDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class CatalogPart : Part
	{
		// Token: 0x17001528 RID: 5416
		// (get) Token: 0x06005330 RID: 21296 RVA: 0x00151B48 File Offset: 0x00150B48
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		public string DisplayTitle
		{
			get
			{
				string text = this.Title;
				if (string.IsNullOrEmpty(text))
				{
					text = SR.GetString("Part_Untitled");
				}
				return text;
			}
		}

		// Token: 0x17001529 RID: 5417
		// (get) Token: 0x06005331 RID: 21297 RVA: 0x00151B70 File Offset: 0x00150B70
		protected WebPartManager WebPartManager
		{
			get
			{
				return this._webPartManager;
			}
		}

		// Token: 0x1700152A RID: 5418
		// (get) Token: 0x06005332 RID: 21298 RVA: 0x00151B78 File Offset: 0x00150B78
		protected CatalogZoneBase Zone
		{
			get
			{
				return this._zone;
			}
		}

		// Token: 0x06005333 RID: 21299
		public abstract WebPartDescriptionCollection GetAvailableWebPartDescriptions();

		// Token: 0x06005334 RID: 21300 RVA: 0x00151B80 File Offset: 0x00150B80
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override IDictionary GetDesignModeState()
		{
			IDictionary dictionary = new HybridDictionary(1);
			dictionary["Zone"] = this.Zone;
			return dictionary;
		}

		// Token: 0x06005335 RID: 21301
		public abstract WebPart GetWebPart(WebPartDescription description);

		// Token: 0x06005336 RID: 21302 RVA: 0x00151BA8 File Offset: 0x00150BA8
		protected internal override void OnPreRender(EventArgs e)
		{
			base.OnPreRender(e);
			if (this.Zone == null)
			{
				throw new InvalidOperationException(SR.GetString("CatalogPart_MustBeInZone", new object[] { this.ID }));
			}
		}

		// Token: 0x06005337 RID: 21303 RVA: 0x00151BE8 File Offset: 0x00150BE8
		[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
		protected override void SetDesignModeState(IDictionary data)
		{
			if (data != null)
			{
				object obj = data["Zone"];
				if (obj != null)
				{
					this.SetZone((CatalogZoneBase)obj);
				}
			}
		}

		// Token: 0x06005338 RID: 21304 RVA: 0x00151C13 File Offset: 0x00150C13
		internal void SetWebPartManager(WebPartManager webPartManager)
		{
			this._webPartManager = webPartManager;
		}

		// Token: 0x06005339 RID: 21305 RVA: 0x00151C1C File Offset: 0x00150C1C
		internal void SetZone(CatalogZoneBase zone)
		{
			this._zone = zone;
		}

		// Token: 0x04002E55 RID: 11861
		private WebPartManager _webPartManager;

		// Token: 0x04002E56 RID: 11862
		private CatalogZoneBase _zone;
	}
}
