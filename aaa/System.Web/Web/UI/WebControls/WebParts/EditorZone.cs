using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006BE RID: 1726
	[Designer("System.Web.UI.Design.WebControls.WebParts.EditorZoneDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[SupportsEventValidation]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class EditorZone : EditorZoneBase
	{
		// Token: 0x060054DD RID: 21725 RVA: 0x001593D8 File Offset: 0x001583D8
		protected override EditorPartCollection CreateEditorParts()
		{
			EditorPartCollection editorPartCollection = new EditorPartCollection();
			if (this._zoneTemplate != null)
			{
				Control control = new NonParentingControl();
				this._zoneTemplate.InstantiateIn(control);
				if (control.HasControls())
				{
					foreach (object obj in control.Controls)
					{
						Control control2 = (Control)obj;
						EditorPart editorPart = control2 as EditorPart;
						if (editorPart != null)
						{
							editorPartCollection.Add(editorPart);
						}
						else
						{
							LiteralControl literalControl = control2 as LiteralControl;
							if ((literalControl == null || literalControl.Text.Trim().Length != 0) && !base.DesignMode)
							{
								throw new InvalidOperationException(SR.GetString("EditorZone_OnlyEditorParts", new object[] { this.ID }));
							}
						}
					}
				}
			}
			return editorPartCollection;
		}

		// Token: 0x170015BA RID: 5562
		// (get) Token: 0x060054DE RID: 21726 RVA: 0x001594C0 File Offset: 0x001584C0
		// (set) Token: 0x060054DF RID: 21727 RVA: 0x001594C8 File Offset: 0x001584C8
		[DefaultValue(null)]
		[Browsable(false)]
		[TemplateInstance(TemplateInstance.Single)]
		[PersistenceMode(PersistenceMode.InnerProperty)]
		[TemplateContainer(typeof(EditorZone))]
		public virtual ITemplate ZoneTemplate
		{
			get
			{
				return this._zoneTemplate;
			}
			set
			{
				base.InvalidateEditorParts();
				this._zoneTemplate = value;
			}
		}

		// Token: 0x04002EF7 RID: 12023
		private ITemplate _zoneTemplate;
	}
}
