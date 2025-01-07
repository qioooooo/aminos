using System;
using System.ComponentModel;
using System.Security.Permissions;
using Microsoft.Win32;

namespace Microsoft.VisualBasic.MyServices
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class RegistryProxy
	{
		public RegistryKey CurrentUser
		{
			get
			{
				return Registry.CurrentUser;
			}
		}

		public RegistryKey LocalMachine
		{
			get
			{
				return Registry.LocalMachine;
			}
		}

		public RegistryKey ClassesRoot
		{
			get
			{
				return Registry.ClassesRoot;
			}
		}

		public RegistryKey Users
		{
			get
			{
				return Registry.Users;
			}
		}

		public RegistryKey PerformanceData
		{
			get
			{
				return Registry.PerformanceData;
			}
		}

		public RegistryKey CurrentConfig
		{
			get
			{
				return Registry.CurrentConfig;
			}
		}

		public RegistryKey DynData
		{
			get
			{
				return Registry.DynData;
			}
		}

		public object GetValue(string keyName, string valueName, object defaultValue)
		{
			return Registry.GetValue(keyName, valueName, defaultValue);
		}

		public void SetValue(string keyName, string valueName, object value)
		{
			Registry.SetValue(keyName, valueName, value);
		}

		public void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind)
		{
			Registry.SetValue(keyName, valueName, value, valueKind);
		}

		internal RegistryProxy()
		{
		}
	}
}
