using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Security.Permissions;

namespace System.ServiceProcess.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ServiceControllerDesigner : ComponentDesigner
	{
		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			RuntimeComponentFilter.FilterProperties(properties, new string[] { "ServiceName", "DisplayName" }, new string[] { "CanPauseAndContinue", "CanShutdown", "CanStop", "DisplayName", "DependentServices", "ServicesDependedOn", "Status", "ServiceType", "MachineName" }, new bool[]
			{
				default(bool),
				default(bool),
				default(bool),
				default(bool),
				default(bool),
				default(bool),
				default(bool),
				default(bool),
				true
			});
		}
	}
}
