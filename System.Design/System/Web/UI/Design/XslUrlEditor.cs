using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XslUrlEditor : UrlEditor
	{
		protected override string Caption
		{
			get
			{
				return SR.GetString("UrlPicker_XslCaption");
			}
		}

		protected override string Filter
		{
			get
			{
				return SR.GetString("UrlPicker_XslFilter");
			}
		}

		protected override UrlBuilderOptions Options
		{
			get
			{
				return UrlBuilderOptions.NoAbsolute;
			}
		}
	}
}
