using System;
using System.Collections;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	// Token: 0x0200053C RID: 1340
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class EditorPartDesigner : PartDesigner
	{
		// Token: 0x06002F56 RID: 12118 RVA: 0x0010E2A4 File Offset: 0x0010D2A4
		protected override Control CreateViewControl()
		{
			Control control = base.CreateViewControl();
			IDictionary designModeState = ((IControlDesignerAccessor)this._editorPart).GetDesignModeState();
			((IControlDesignerAccessor)control).SetDesignModeState(designModeState);
			return control;
		}

		// Token: 0x06002F57 RID: 12119 RVA: 0x0010E2CC File Offset: 0x0010D2CC
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(EditorPart));
			this._editorPart = (EditorPart)component;
			base.Initialize(component);
		}

		// Token: 0x06002F58 RID: 12120 RVA: 0x0010E2F1 File Offset: 0x0010D2F1
		public override string GetDesignTimeHtml()
		{
			if (!(this._editorPart.Parent is EditorZoneBase))
			{
				return base.CreateInvalidParentDesignTimeHtml(typeof(EditorPart), typeof(EditorZoneBase));
			}
			return base.GetDesignTimeHtml();
		}

		// Token: 0x0400203F RID: 8255
		private EditorPart _editorPart;
	}
}
