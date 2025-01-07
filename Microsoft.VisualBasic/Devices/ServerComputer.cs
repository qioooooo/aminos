using System;
using System.Security.Permissions;
using Microsoft.VisualBasic.MyServices;

namespace Microsoft.VisualBasic.Devices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class ServerComputer
	{
		public Clock Clock
		{
			get
			{
				if (ServerComputer.m_Clock != null)
				{
					return ServerComputer.m_Clock;
				}
				ServerComputer.m_Clock = new Clock();
				return ServerComputer.m_Clock;
			}
		}

		public FileSystemProxy FileSystem
		{
			get
			{
				if (this.m_FileIO == null)
				{
					this.m_FileIO = new FileSystemProxy();
				}
				return this.m_FileIO;
			}
		}

		public ComputerInfo Info
		{
			get
			{
				if (this.m_ComputerInfo == null)
				{
					this.m_ComputerInfo = new ComputerInfo();
				}
				return this.m_ComputerInfo;
			}
		}

		public Network Network
		{
			get
			{
				if (this.m_Network != null)
				{
					return this.m_Network;
				}
				this.m_Network = new Network();
				return this.m_Network;
			}
		}

		public string Name
		{
			get
			{
				return Environment.MachineName;
			}
		}

		public RegistryProxy Registry
		{
			get
			{
				if (this.m_RegistryInstance != null)
				{
					return this.m_RegistryInstance;
				}
				this.m_RegistryInstance = new RegistryProxy();
				return this.m_RegistryInstance;
			}
		}

		private ComputerInfo m_ComputerInfo;

		private FileSystemProxy m_FileIO;

		private Network m_Network;

		private RegistryProxy m_RegistryInstance;

		private static Clock m_Clock;
	}
}
