﻿using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.Diagnostics.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ProcessModuleDesigner : ComponentDesigner
	{
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			RuntimeComponentFilter.FilterProperties(properties, null, new string[] { "FileVersionInfo" });
		}
	}
}
