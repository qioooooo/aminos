using System;
using System.ComponentModel;
using System.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XmlDesigner : ControlDesigner
	{
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.xml = null;
			}
			base.Dispose(disposing);
		}

		public override string GetDesignTimeHtml()
		{
			return this.GetEmptyDesignTimeHtml();
		}

		protected override string GetEmptyDesignTimeHtml()
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Xml_Inst"));
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(Xml));
			this.xml = (Xml)component;
			base.Initialize(component);
		}

		internal override string GetPersistInnerHtmlInternal()
		{
			Xml xml = (Xml)base.Component;
			string text = (string)((IControlDesignerAccessor)xml).GetDesignModeState()["OriginalContent"];
			if (text != null)
			{
				return text;
			}
			return xml.DocumentContent;
		}

		private Xml xml;
	}
}
