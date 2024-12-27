using System;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Activation;
using System.Security.Permissions;

namespace System.Runtime.Remoting
{
	// Token: 0x02000746 RID: 1862
	[ComVisible(true)]
	public static class RemotingConfiguration
	{
		// Token: 0x060042B0 RID: 17072 RVA: 0x000E5134 File Offset: 0x000E4134
		[Obsolete("Use System.Runtime.Remoting.RemotingConfiguration.Configure(string fileName, bool ensureSecurity) instead.", false)]
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void Configure(string filename)
		{
			RemotingConfiguration.Configure(filename, false);
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x000E513D File Offset: 0x000E413D
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void Configure(string filename, bool ensureSecurity)
		{
			RemotingConfigHandler.DoConfiguration(filename, ensureSecurity);
			RemotingServices.InternalSetRemoteActivationConfigured();
		}

		// Token: 0x17000BCF RID: 3023
		// (get) Token: 0x060042B2 RID: 17074 RVA: 0x000E514B File Offset: 0x000E414B
		// (set) Token: 0x060042B3 RID: 17075 RVA: 0x000E515B File Offset: 0x000E415B
		public static string ApplicationName
		{
			get
			{
				if (!RemotingConfigHandler.HasApplicationNameBeenSet())
				{
					return null;
				}
				return RemotingConfigHandler.ApplicationName;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				RemotingConfigHandler.ApplicationName = value;
			}
		}

		// Token: 0x17000BD0 RID: 3024
		// (get) Token: 0x060042B4 RID: 17076 RVA: 0x000E5163 File Offset: 0x000E4163
		public static string ApplicationId
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return Identity.AppDomainUniqueId;
			}
		}

		// Token: 0x17000BD1 RID: 3025
		// (get) Token: 0x060042B5 RID: 17077 RVA: 0x000E516A File Offset: 0x000E416A
		public static string ProcessId
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get
			{
				return Identity.ProcessGuid;
			}
		}

		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x060042B6 RID: 17078 RVA: 0x000E5171 File Offset: 0x000E4171
		// (set) Token: 0x060042B7 RID: 17079 RVA: 0x000E5178 File Offset: 0x000E4178
		public static CustomErrorsModes CustomErrorsMode
		{
			get
			{
				return RemotingConfigHandler.CustomErrorsMode;
			}
			[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
			set
			{
				RemotingConfigHandler.CustomErrorsMode = value;
			}
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x000E5180 File Offset: 0x000E4180
		public static bool CustomErrorsEnabled(bool isLocalRequest)
		{
			switch (RemotingConfiguration.CustomErrorsMode)
			{
			case CustomErrorsModes.On:
				return true;
			case CustomErrorsModes.Off:
				return false;
			case CustomErrorsModes.RemoteOnly:
				return !isLocalRequest;
			default:
				return true;
			}
		}

		// Token: 0x060042B9 RID: 17081 RVA: 0x000E51B4 File Offset: 0x000E41B4
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void RegisterActivatedServiceType(Type type)
		{
			ActivatedServiceTypeEntry activatedServiceTypeEntry = new ActivatedServiceTypeEntry(type);
			RemotingConfiguration.RegisterActivatedServiceType(activatedServiceTypeEntry);
		}

		// Token: 0x060042BA RID: 17082 RVA: 0x000E51CE File Offset: 0x000E41CE
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void RegisterActivatedServiceType(ActivatedServiceTypeEntry entry)
		{
			RemotingConfigHandler.RegisterActivatedServiceType(entry);
			if (!RemotingConfiguration.s_ListeningForActivationRequests)
			{
				RemotingConfiguration.s_ListeningForActivationRequests = true;
				ActivationServices.StartListeningForRemoteRequests();
			}
		}

		// Token: 0x060042BB RID: 17083 RVA: 0x000E51E8 File Offset: 0x000E41E8
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void RegisterWellKnownServiceType(Type type, string objectUri, WellKnownObjectMode mode)
		{
			WellKnownServiceTypeEntry wellKnownServiceTypeEntry = new WellKnownServiceTypeEntry(type, objectUri, mode);
			RemotingConfiguration.RegisterWellKnownServiceType(wellKnownServiceTypeEntry);
		}

		// Token: 0x060042BC RID: 17084 RVA: 0x000E5204 File Offset: 0x000E4204
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void RegisterWellKnownServiceType(WellKnownServiceTypeEntry entry)
		{
			RemotingConfigHandler.RegisterWellKnownServiceType(entry);
		}

		// Token: 0x060042BD RID: 17085 RVA: 0x000E520C File Offset: 0x000E420C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void RegisterActivatedClientType(Type type, string appUrl)
		{
			ActivatedClientTypeEntry activatedClientTypeEntry = new ActivatedClientTypeEntry(type, appUrl);
			RemotingConfiguration.RegisterActivatedClientType(activatedClientTypeEntry);
		}

		// Token: 0x060042BE RID: 17086 RVA: 0x000E5227 File Offset: 0x000E4227
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void RegisterActivatedClientType(ActivatedClientTypeEntry entry)
		{
			RemotingConfigHandler.RegisterActivatedClientType(entry);
			RemotingServices.InternalSetRemoteActivationConfigured();
		}

		// Token: 0x060042BF RID: 17087 RVA: 0x000E5234 File Offset: 0x000E4234
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void RegisterWellKnownClientType(Type type, string objectUrl)
		{
			WellKnownClientTypeEntry wellKnownClientTypeEntry = new WellKnownClientTypeEntry(type, objectUrl);
			RemotingConfiguration.RegisterWellKnownClientType(wellKnownClientTypeEntry);
		}

		// Token: 0x060042C0 RID: 17088 RVA: 0x000E524F File Offset: 0x000E424F
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static void RegisterWellKnownClientType(WellKnownClientTypeEntry entry)
		{
			RemotingConfigHandler.RegisterWellKnownClientType(entry);
			RemotingServices.InternalSetRemoteActivationConfigured();
		}

		// Token: 0x060042C1 RID: 17089 RVA: 0x000E525C File Offset: 0x000E425C
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static ActivatedServiceTypeEntry[] GetRegisteredActivatedServiceTypes()
		{
			return RemotingConfigHandler.GetRegisteredActivatedServiceTypes();
		}

		// Token: 0x060042C2 RID: 17090 RVA: 0x000E5263 File Offset: 0x000E4263
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static WellKnownServiceTypeEntry[] GetRegisteredWellKnownServiceTypes()
		{
			return RemotingConfigHandler.GetRegisteredWellKnownServiceTypes();
		}

		// Token: 0x060042C3 RID: 17091 RVA: 0x000E526A File Offset: 0x000E426A
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static ActivatedClientTypeEntry[] GetRegisteredActivatedClientTypes()
		{
			return RemotingConfigHandler.GetRegisteredActivatedClientTypes();
		}

		// Token: 0x060042C4 RID: 17092 RVA: 0x000E5271 File Offset: 0x000E4271
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static WellKnownClientTypeEntry[] GetRegisteredWellKnownClientTypes()
		{
			return RemotingConfigHandler.GetRegisteredWellKnownClientTypes();
		}

		// Token: 0x060042C5 RID: 17093 RVA: 0x000E5278 File Offset: 0x000E4278
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static ActivatedClientTypeEntry IsRemotelyActivatedClientType(Type svrType)
		{
			return RemotingConfigHandler.IsRemotelyActivatedClientType(svrType);
		}

		// Token: 0x060042C6 RID: 17094 RVA: 0x000E5280 File Offset: 0x000E4280
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static ActivatedClientTypeEntry IsRemotelyActivatedClientType(string typeName, string assemblyName)
		{
			return RemotingConfigHandler.IsRemotelyActivatedClientType(typeName, assemblyName);
		}

		// Token: 0x060042C7 RID: 17095 RVA: 0x000E5289 File Offset: 0x000E4289
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static WellKnownClientTypeEntry IsWellKnownClientType(Type svrType)
		{
			return RemotingConfigHandler.IsWellKnownClientType(svrType);
		}

		// Token: 0x060042C8 RID: 17096 RVA: 0x000E5291 File Offset: 0x000E4291
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static WellKnownClientTypeEntry IsWellKnownClientType(string typeName, string assemblyName)
		{
			return RemotingConfigHandler.IsWellKnownClientType(typeName, assemblyName);
		}

		// Token: 0x060042C9 RID: 17097 RVA: 0x000E529A File Offset: 0x000E429A
		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.RemotingConfiguration)]
		public static bool IsActivationAllowed(Type svrType)
		{
			return RemotingConfigHandler.IsActivationAllowed(svrType);
		}

		// Token: 0x04002172 RID: 8562
		private static bool s_ListeningForActivationRequests;
	}
}
