using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Management.Instrumentation
{
	// Token: 0x020000A2 RID: 162
	public class Instrumentation
	{
		// Token: 0x060004BD RID: 1213
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int GetCurrentProcessId();

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00022BEC File Offset: 0x00021BEC
		internal static string ProcessIdentity
		{
			get
			{
				lock (typeof(Instrumentation))
				{
					if (Instrumentation.processIdentity == null)
					{
						Instrumentation.processIdentity = Guid.NewGuid().ToString().ToLower(CultureInfo.InvariantCulture);
					}
				}
				return Instrumentation.processIdentity;
			}
		}

		// Token: 0x060004BF RID: 1215 RVA: 0x00022C54 File Offset: 0x00021C54
		public static void RegisterAssembly(Assembly assemblyToRegister)
		{
			if (assemblyToRegister == null)
			{
				throw new ArgumentNullException("assemblyToRegister");
			}
			Instrumentation.GetInstrumentedAssembly(assemblyToRegister);
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x00022C6C File Offset: 0x00021C6C
		public static bool IsAssemblyRegistered(Assembly assemblyToRegister)
		{
			if (assemblyToRegister == null)
			{
				throw new ArgumentNullException("assemblyToRegister");
			}
			lock (Instrumentation.instrumentedAssemblies)
			{
				if (Instrumentation.instrumentedAssemblies.ContainsKey(assemblyToRegister))
				{
					return true;
				}
			}
			SchemaNaming schemaNaming = SchemaNaming.GetSchemaNaming(assemblyToRegister);
			return schemaNaming != null && schemaNaming.IsAssemblyRegistered();
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x00022CD4 File Offset: 0x00021CD4
		public static void Fire(object eventData)
		{
			IEvent @event = eventData as IEvent;
			if (@event != null)
			{
				@event.Fire();
				return;
			}
			Instrumentation.GetFireFunction(eventData.GetType())(eventData);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x00022D04 File Offset: 0x00021D04
		public static void Publish(object instanceData)
		{
			Type type = instanceData as Type;
			Assembly assembly = instanceData as Assembly;
			IInstance instance = instanceData as IInstance;
			if (type != null)
			{
				Instrumentation.GetInstrumentedAssembly(type.Assembly);
				return;
			}
			if (assembly != null)
			{
				Instrumentation.GetInstrumentedAssembly(assembly);
				return;
			}
			if (instance != null)
			{
				instance.Published = true;
				return;
			}
			Instrumentation.GetPublishFunction(instanceData.GetType())(instanceData);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x00022D60 File Offset: 0x00021D60
		public static void Revoke(object instanceData)
		{
			IInstance instance = instanceData as IInstance;
			if (instance != null)
			{
				instance.Published = false;
				return;
			}
			Instrumentation.GetRevokeFunction(instanceData.GetType())(instanceData);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x00022D90 File Offset: 0x00021D90
		public static void SetBatchSize(Type instrumentationClass, int batchSize)
		{
			Instrumentation.GetInstrumentedAssembly(instrumentationClass.Assembly).SetBatchSize(instrumentationClass, batchSize);
		}

		// Token: 0x060004C5 RID: 1221 RVA: 0x00022DA4 File Offset: 0x00021DA4
		internal static ProvisionFunction GetFireFunction(Type type)
		{
			return new ProvisionFunction(Instrumentation.GetInstrumentedAssembly(type.Assembly).Fire);
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x00022DBC File Offset: 0x00021DBC
		internal static ProvisionFunction GetPublishFunction(Type type)
		{
			return new ProvisionFunction(Instrumentation.GetInstrumentedAssembly(type.Assembly).Publish);
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x00022DD4 File Offset: 0x00021DD4
		internal static ProvisionFunction GetRevokeFunction(Type type)
		{
			return new ProvisionFunction(Instrumentation.GetInstrumentedAssembly(type.Assembly).Revoke);
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x00022DEC File Offset: 0x00021DEC
		private static void Initialize(Assembly assembly)
		{
			lock (Instrumentation.instrumentedAssemblies)
			{
				if (!Instrumentation.instrumentedAssemblies.ContainsKey(assembly))
				{
					SchemaNaming schemaNaming = SchemaNaming.GetSchemaNaming(assembly);
					if (schemaNaming != null)
					{
						if (!schemaNaming.IsAssemblyRegistered())
						{
							if (!WMICapabilities.IsUserAdmin())
							{
								throw new Exception(RC.GetString("ASSEMBLY_NOT_REGISTERED"));
							}
							schemaNaming.DecoupledProviderInstanceName = AssemblyNameUtility.UniqueToAssemblyFullVersion(assembly);
							schemaNaming.RegisterNonAssemblySpecificSchema(null);
							schemaNaming.RegisterAssemblySpecificSchema();
						}
						InstrumentedAssembly instrumentedAssembly = new InstrumentedAssembly(assembly, schemaNaming);
						Instrumentation.instrumentedAssemblies.Add(assembly, instrumentedAssembly);
					}
				}
			}
		}

		// Token: 0x060004C9 RID: 1225 RVA: 0x00022E88 File Offset: 0x00021E88
		private static InstrumentedAssembly GetInstrumentedAssembly(Assembly assembly)
		{
			InstrumentedAssembly instrumentedAssembly;
			lock (Instrumentation.instrumentedAssemblies)
			{
				if (!Instrumentation.instrumentedAssemblies.ContainsKey(assembly))
				{
					Instrumentation.Initialize(assembly);
				}
				instrumentedAssembly = (InstrumentedAssembly)Instrumentation.instrumentedAssemblies[assembly];
			}
			return instrumentedAssembly;
		}

		// Token: 0x04000287 RID: 647
		private static string processIdentity = null;

		// Token: 0x04000288 RID: 648
		private static Hashtable instrumentedAssemblies = new Hashtable();
	}
}
