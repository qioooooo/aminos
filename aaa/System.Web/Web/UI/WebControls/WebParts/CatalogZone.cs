using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006AD RID: 1709
	[SupportsEventValidation]
	[Designer("System.Web.UI.Design.WebControls.WebParts.CatalogZoneDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class CatalogZone : CatalogZoneBase
	{
		// Token: 0x060053C9 RID: 21449 RVA: 0x0015447C File Offset: 0x0015347C
		protected override CatalogPartCollection CreateCatalogParts()
		{
			CatalogPartCollection catalogPartCollection = new CatalogPartCollection();
			if (this._zoneTemplate != null)
			{
				Control control = new NonParentingControl();
				this._zoneTemplate.InstantiateIn(control);
				if (control.HasControls())
				{
					foreach (object obj in control.Controls)
					{
						Control control2 = (Control)obj;
						CatalogPart catalogPart = control2 as CatalogPart;
						if (catalogPart != null)
						{
							catalogPartCollection.Add(catalogPart);
						}
						else
						{
							LiteralControl literalControl = control2 as LiteralControl;
							if ((literalControl == null || literalControl.Text.Trim().Length != 0) && !base.DesignMode)
							{
								throw new InvalidOperationException(SR.GetString("CatalogZone_OnlyCatalogParts", new object[] { this.ID }));
							}
						}
					}
				}
			}
			return catalogPartCollection;
		}

		// Token: 0x1700155A RID: 5466
		// (get) Token: 0x060053CA RID: 21450 RVA: 0x00154564 File Offset: 0x00153564
		// (set) Token: 0x060053CB RID: 21451 RVA: 0x0015456C File Offset: 0x0015356C
		[Browsable(false)]
		[DefaultValue(null)]
		[TemplateContainer(typeof(CatalogZone))]
		[TemplateInstance(TemplateInstance.Single)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		public virtual ITemplate ZoneTemplate
		{
			get
			{
				return this._zoneTemplate;
			}
			set
			{
				base.InvalidateCatalogParts();
				this._zoneTemplate = value;
			}
		}

		// Token: 0x04002E91 RID: 11921
		private ITemplate _zoneTemplate;
	}
}
