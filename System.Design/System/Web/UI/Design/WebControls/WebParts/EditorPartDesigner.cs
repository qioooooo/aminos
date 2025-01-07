using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class EditorPartDesigner : PartDesigner
	{
		protected override Control CreateViewControl()
		{
			Control control = base.CreateViewControl();
			IDictionary designModeState = ((IControlDesignerAccessor)this._editorPart).GetDesignModeState();
			((IControlDesignerAccessor)control).SetDesignModeState(designModeState);
			return control;
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(EditorPart));
			this._editorPart = (EditorPart)component;
			base.Initialize(component);
		}

		public override string GetDesignTimeHtml()
		{
			if (!(this._editorPart.Parent is EditorZoneBase))
			{
				return base.CreateInvalidParentDesignTimeHtml(typeof(EditorPart), typeof(EditorZoneBase));
			}
			return base.GetDesignTimeHtml();
		}

		private EditorPart _editorPart;
	}
}
