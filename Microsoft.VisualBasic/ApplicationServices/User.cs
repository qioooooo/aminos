using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class User
	{
		public string Name
		{
			get
			{
				return this.InternalPrincipal.Identity.Name;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public IPrincipal CurrentPrincipal
		{
			get
			{
				return this.InternalPrincipal;
			}
			set
			{
				this.InternalPrincipal = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public void InitializeWithWindowsUser()
		{
			Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
		}

		public bool IsAuthenticated
		{
			get
			{
				return this.InternalPrincipal.Identity.IsAuthenticated;
			}
		}

		public bool IsInRole(string role)
		{
			return this.InternalPrincipal.IsInRole(role);
		}

		public bool IsInRole(BuiltInRole role)
		{
			User.ValidateBuiltInRoleEnumValue(role, "role");
			TypeConverter converter = TypeDescriptor.GetConverter(typeof(BuiltInRole));
			if (this.IsWindowsPrincipal())
			{
				WindowsBuiltInRole windowsBuiltInRole = (WindowsBuiltInRole)converter.ConvertTo(role, typeof(WindowsBuiltInRole));
				return ((WindowsPrincipal)this.InternalPrincipal).IsInRole(windowsBuiltInRole);
			}
			return this.InternalPrincipal.IsInRole(converter.ConvertToString(role));
		}

		protected virtual IPrincipal InternalPrincipal
		{
			get
			{
				return Thread.CurrentPrincipal;
			}
			set
			{
				Thread.CurrentPrincipal = value;
			}
		}

		private bool IsWindowsPrincipal()
		{
			return this.InternalPrincipal is WindowsPrincipal;
		}

		internal static void ValidateBuiltInRoleEnumValue(BuiltInRole testMe, string parameterName)
		{
			if (testMe == BuiltInRole.AccountOperator)
			{
				return;
			}
			if (testMe == BuiltInRole.Administrator)
			{
				return;
			}
			if (testMe == BuiltInRole.BackupOperator)
			{
				return;
			}
			if (testMe == BuiltInRole.Guest)
			{
				return;
			}
			if (testMe == BuiltInRole.PowerUser)
			{
				return;
			}
			if (testMe == BuiltInRole.PrintOperator)
			{
				return;
			}
			if (testMe == BuiltInRole.Replicator)
			{
				return;
			}
			if (testMe == BuiltInRole.SystemOperator)
			{
				return;
			}
			if (testMe == BuiltInRole.User)
			{
				return;
			}
			throw new InvalidEnumArgumentException(parameterName, (int)testMe, typeof(BuiltInRole));
		}
	}
}
