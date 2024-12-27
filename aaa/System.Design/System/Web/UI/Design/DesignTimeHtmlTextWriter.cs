using System;
using System.IO;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000363 RID: 867
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	internal class DesignTimeHtmlTextWriter : HtmlTextWriter
	{
		// Token: 0x060020A7 RID: 8359 RVA: 0x000B7660 File Offset: 0x000B6660
		public DesignTimeHtmlTextWriter(TextWriter writer)
			: base(writer)
		{
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000B7669 File Offset: 0x000B6669
		public DesignTimeHtmlTextWriter(TextWriter writer, string tabString)
			: base(writer, tabString)
		{
		}

		// Token: 0x060020A9 RID: 8361 RVA: 0x000B7673 File Offset: 0x000B6673
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
