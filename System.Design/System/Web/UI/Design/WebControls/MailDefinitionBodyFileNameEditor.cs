using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class MailDefinitionBodyFileNameEditor : UrlEditor
	{
		protected override string Caption
		{
			get
			{
				return SR.GetString("MailDefinitionBodyFileNameEditor_DefaultCaption");
			}
		}

		protected override string Filter
		{
			get
			{
				return SR.GetString("MailDefinitionBodyFileNameEditor_DefaultFilter");
			}
		}
	}
}
