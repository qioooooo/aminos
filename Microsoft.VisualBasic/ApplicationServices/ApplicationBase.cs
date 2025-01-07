using System;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.Logging;

namespace Microsoft.VisualBasic.ApplicationServices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class ApplicationBase
	{
		public string GetEnvironmentVariable(string name)
		{
			string environmentVariable = Environment.GetEnvironmentVariable(name);
			if (environmentVariable == null)
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("name", "EnvVarNotFound_Name", new string[] { name });
			}
			return environmentVariable;
		}

		public Log Log
		{
			get
			{
				if (this.m_Log == null)
				{
					this.m_Log = new Log();
				}
				return this.m_Log;
			}
		}

		public AssemblyInfo Info
		{
			[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
			get
			{
				if (this.m_Info == null)
				{
					Assembly assembly = Assembly.GetEntryAssembly();
					if (assembly == null)
					{
						assembly = Assembly.GetCallingAssembly();
					}
					this.m_Info = new AssemblyInfo(assembly);
				}
				return this.m_Info;
			}
		}

		public CultureInfo Culture
		{
			get
			{
				return Thread.CurrentThread.CurrentCulture;
			}
		}

		public CultureInfo UICulture
		{
			get
			{
				return Thread.CurrentThread.CurrentUICulture;
			}
		}

		[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
		public void ChangeCulture(string cultureName)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo(cultureName);
		}

		public void ChangeUICulture(string cultureName)
		{
			Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureName);
		}

		private Log m_Log;

		private AssemblyInfo m_Info;
	}
}
