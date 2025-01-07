using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class CatalogPartDesigner : PartDesigner
	{
		protected override Control CreateViewControl()
		{
			Control control = base.CreateViewControl();
			IDictionary designModeState = ((IControlDesignerAccessor)this._catalogPart).GetDesignModeState();
			((IControlDesignerAccessor)control).SetDesignModeState(designModeState);
			return control;
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(CatalogPart));
			this._catalogPart = (CatalogPart)component;
			base.Initialize(component);
		}

		public override string GetDesignTimeHtml()
		{
			if (!(this._catalogPart.Parent is CatalogZoneBase))
			{
				return base.CreateInvalidParentDesignTimeHtml(typeof(CatalogPart), typeof(CatalogZoneBase));
			}
			return base.GetDesignTimeHtml();
		}

		private CatalogPart _catalogPart;
	}
}
