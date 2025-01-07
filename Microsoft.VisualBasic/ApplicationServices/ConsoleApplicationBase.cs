using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Deployment.Application;
using System.Security.Permissions;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class ConsoleApplicationBase : ApplicationBase
	{
		public ReadOnlyCollection<string> CommandLineArgs
		{
			get
			{
				checked
				{
					if (this.m_CommandLineArgs == null)
					{
						string[] commandLineArgs = Environment.GetCommandLineArgs();
						if (commandLineArgs.GetLength(0) >= 2)
						{
							string[] array = new string[commandLineArgs.GetLength(0) - 2 + 1];
							Array.Copy(commandLineArgs, 1, array, 0, commandLineArgs.GetLength(0) - 1);
							this.m_CommandLineArgs = new ReadOnlyCollection<string>(array);
						}
						else
						{
							this.m_CommandLineArgs = new ReadOnlyCollection<string>(new string[0]);
						}
					}
					return this.m_CommandLineArgs;
				}
			}
		}

		public ApplicationDeployment Deployment
		{
			get
			{
				return ApplicationDeployment.CurrentDeployment;
			}
		}

		public bool IsNetworkDeployed
		{
			get
			{
				return ApplicationDeployment.IsNetworkDeployed;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		protected ReadOnlyCollection<string> InternalCommandLine
		{
			set
			{
				this.m_CommandLineArgs = value;
			}
		}

		private ReadOnlyCollection<string> m_CommandLineArgs;
	}
}
