﻿using System;
using System.IO;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	internal class DesignTimeHtmlTextWriter : HtmlTextWriter
	{
		public DesignTimeHtmlTextWriter(TextWriter writer)
			: base(writer)
		{
		}

		public DesignTimeHtmlTextWriter(TextWriter writer, string tabString)
			: base(writer, tabString)
		{
		}

		public override void AddAttribute(HtmlTextWriterAttribute key, string value)
		{
			if (key == HtmlTextWriterAttribute.Src || key == HtmlTextWriterAttribute.Href || key == HtmlTextWriterAttribute.Background)
			{
				base.AddAttribute(key.ToString(), value, key);
				return;
			}
			base.AddAttribute(key, value);
		}
	}
}
