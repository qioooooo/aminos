using System;
using System.ComponentModel;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class DeclarativeCatalogPartDesigner : CatalogPartDesigner
	{
		public override TemplateGroupCollection TemplateGroups
		{
			get
			{
				TemplateGroupCollection templateGroups = base.TemplateGroups;
				if (this._templateGroup == null)
				{
					this._templateGroup = new TemplateGroup("WebPartsTemplate", this._catalogPart.ControlStyle);
					this._templateGroup.AddTemplateDefinition(new TemplateDefinition(this, "WebPartsTemplate", this._catalogPart, "WebPartsTemplate", this._catalogPart.ControlStyle));
				}
				templateGroups.Add(this._templateGroup);
				return templateGroups;
			}
		}

		public override string GetDesignTimeHtml()
		{
			if (!(this._catalogPart.Parent is CatalogZoneBase))
			{
				return base.CreateInvalidParentDesignTimeHtml(typeof(CatalogPart), typeof(CatalogZoneBase));
			}
			string text = string.Empty;
			try
			{
				if (((DeclarativeCatalogPart)base.ViewControl).WebPartsTemplate == null)
				{
					text = this.GetEmptyDesignTimeHtml();
				}
				else
				{
					text = string.Empty;
				}
			}
			catch (Exception ex)
			{
				text = this.GetErrorDesignTimeHtml(ex);
			}
			return text;
		}

		protected override string GetEmptyDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("DeclarativeCatalogPartDesigner_Empty"));
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(DeclarativeCatalogPart));
			base.Initialize(component);
			this._catalogPart = (DeclarativeCatalogPart)component;
			if (base.View != null)
			{
				base.View.SetFlags(ViewFlags.TemplateEditing, true);
			}
		}

		private const string templateName = "WebPartsTemplate";

		private DeclarativeCatalogPart _catalogPart;

		private TemplateGroup _templateGroup;
	}
}
