using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class MdbDataFileEditor : UrlEditor
	{
		protected override string Caption
		{
			get
			{
				return SR.GetString("MdbDataFileEditor_Caption");
			}
		}

		protected override string Filter
		{
			get
			{
				return SR.GetString("MdbDataFileEditor_Filter");
			}
		}
	}
}
