using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class MailFileEditor : UrlEditor
	{
		protected override string Caption
		{
			get
			{
				return SR.GetString("MailFilePicker_Caption");
			}
		}

		protected override string Filter
		{
			get
			{
				return SR.GetString("MailFilePicker_Filter");
			}
		}
	}
}
