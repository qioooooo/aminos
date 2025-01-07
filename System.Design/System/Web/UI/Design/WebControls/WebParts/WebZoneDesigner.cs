using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public abstract class WebZoneDesigner : ControlDesigner
	{
		internal WebZoneDesigner()
		{
		}

		internal TemplateDefinition TemplateDefinition
		{
			get
			{
				return new TemplateDefinition(this, "ZoneTemplate", base.Component, "ZoneTemplate", ((WebControl)base.ViewControl).ControlStyle, true);
			}
		}

		internal TemplateGroup CreateZoneTemplateGroup()
		{
			TemplateGroup templateGroup = new TemplateGroup("ZoneTemplate", ((WebControl)base.ViewControl).ControlStyle);
			templateGroup.AddTemplateDefinition(new TemplateDefinition(this, "ZoneTemplate", base.Component, "ZoneTemplate", ((WebControl)base.ViewControl).ControlStyle));
			return templateGroup;
		}

		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(WebZone));
			base.Initialize(component);
		}

		internal const string _templateName = "ZoneTemplate";
	}
}
