using System;
using System.Collections;
using System.Globalization;
using System.Reflection;

namespace System.Runtime.Remoting.Activation
{
	// Token: 0x02000738 RID: 1848
	internal class RemotingXmlConfigFileData
	{
		// Token: 0x06004267 RID: 16999 RVA: 0x000E26EC File Offset: 0x000E16EC
		internal void AddInteropXmlElementEntry(string xmlElementName, string xmlElementNamespace, string urtTypeName, string urtAssemblyName)
		{
			this.TryToLoadTypeIfApplicable(urtTypeName, urtAssemblyName);
			RemotingXmlConfigFileData.InteropXmlElementEntry interopXmlElementEntry = new RemotingXmlConfigFileData.InteropXmlElementEntry(xmlElementName, xmlElementNamespace, urtTypeName, urtAssemblyName);
			this.InteropXmlElementEntries.Add(interopXmlElementEntry);
		}

		// Token: 0x06004268 RID: 17000 RVA: 0x000E271C File Offset: 0x000E171C
		internal void AddInteropXmlTypeEntry(string xmlTypeName, string xmlTypeNamespace, string urtTypeName, string urtAssemblyName)
		{
			this.TryToLoadTypeIfApplicable(urtTypeName, urtAssemblyName);
			RemotingXmlConfigFileData.InteropXmlTypeEntry interopXmlTypeEntry = new RemotingXmlConfigFileData.InteropXmlTypeEntry(xmlTypeName, xmlTypeNamespace, urtTypeName, urtAssemblyName);
			this.InteropXmlTypeEntries.Add(interopXmlTypeEntry);
		}

		// Token: 0x06004269 RID: 17001 RVA: 0x000E274C File Offset: 0x000E174C
		internal void AddPreLoadEntry(string typeName, string assemblyName)
		{
			this.TryToLoadTypeIfApplicable(typeName, assemblyName);
			RemotingXmlConfigFileData.PreLoadEntry preLoadEntry = new RemotingXmlConfigFileData.PreLoadEntry(typeName, assemblyName);
			this.PreLoadEntries.Add(preLoadEntry);
		}

		// Token: 0x0600426A RID: 17002 RVA: 0x000E2778 File Offset: 0x000E1778
		internal RemotingXmlConfigFileData.RemoteAppEntry AddRemoteAppEntry(string appUri)
		{
			RemotingXmlConfigFileData.RemoteAppEntry remoteAppEntry = new RemotingXmlConfigFileData.RemoteAppEntry(appUri);
			this.RemoteAppEntries.Add(remoteAppEntry);
			return remoteAppEntry;
		}

		// Token: 0x0600426B RID: 17003 RVA: 0x000E279C File Offset: 0x000E179C
		internal void AddServerActivatedEntry(string typeName, string assemName, ArrayList contextAttributes)
		{
			this.TryToLoadTypeIfApplicable(typeName, assemName);
			RemotingXmlConfigFileData.TypeEntry typeEntry = new RemotingXmlConfigFileData.TypeEntry(typeName, assemName, contextAttributes);
			this.ServerActivatedEntries.Add(typeEntry);
		}

		// Token: 0x0600426C RID: 17004 RVA: 0x000E27C8 File Offset: 0x000E17C8
		internal RemotingXmlConfigFileData.ServerWellKnownEntry AddServerWellKnownEntry(string typeName, string assemName, ArrayList contextAttributes, string objURI, WellKnownObjectMode objMode)
		{
			this.TryToLoadTypeIfApplicable(typeName, assemName);
			RemotingXmlConfigFileData.ServerWellKnownEntry serverWellKnownEntry = new RemotingXmlConfigFileData.ServerWellKnownEntry(typeName, assemName, contextAttributes, objURI, objMode);
			this.ServerWellKnownEntries.Add(serverWellKnownEntry);
			return serverWellKnownEntry;
		}

		// Token: 0x0600426D RID: 17005 RVA: 0x000E27F8 File Offset: 0x000E17F8
		private void TryToLoadTypeIfApplicable(string typeName, string assemblyName)
		{
			if (!RemotingXmlConfigFileData.LoadTypes)
			{
				return;
			}
			Assembly assembly = Assembly.Load(assemblyName);
			if (assembly == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_AssemblyLoadFailed"), new object[] { assemblyName }));
			}
			if (assembly.GetType(typeName, false, false) == null)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Remoting_BadType"), new object[] { typeName }));
			}
		}

		// Token: 0x04002137 RID: 8503
		internal static bool LoadTypes;

		// Token: 0x04002138 RID: 8504
		internal string ApplicationName;

		// Token: 0x04002139 RID: 8505
		internal RemotingXmlConfigFileData.LifetimeEntry Lifetime;

		// Token: 0x0400213A RID: 8506
		internal bool UrlObjRefMode = RemotingConfigHandler.UrlObjRefMode;

		// Token: 0x0400213B RID: 8507
		internal RemotingXmlConfigFileData.CustomErrorsEntry CustomErrors;

		// Token: 0x0400213C RID: 8508
		internal ArrayList ChannelEntries = new ArrayList();

		// Token: 0x0400213D RID: 8509
		internal ArrayList InteropXmlElementEntries = new ArrayList();

		// Token: 0x0400213E RID: 8510
		internal ArrayList InteropXmlTypeEntries = new ArrayList();

		// Token: 0x0400213F RID: 8511
		internal ArrayList PreLoadEntries = new ArrayList();

		// Token: 0x04002140 RID: 8512
		internal ArrayList RemoteAppEntries = new ArrayList();

		// Token: 0x04002141 RID: 8513
		internal ArrayList ServerActivatedEntries = new ArrayList();

		// Token: 0x04002142 RID: 8514
		internal ArrayList ServerWellKnownEntries = new ArrayList();

		// Token: 0x02000739 RID: 1849
		internal class ChannelEntry
		{
			// Token: 0x0600426F RID: 17007 RVA: 0x000E28DB File Offset: 0x000E18DB
			internal ChannelEntry(string typeName, string assemblyName, Hashtable properties)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemblyName;
				this.Properties = properties;
			}

			// Token: 0x04002143 RID: 8515
			internal string TypeName;

			// Token: 0x04002144 RID: 8516
			internal string AssemblyName;

			// Token: 0x04002145 RID: 8517
			internal Hashtable Properties;

			// Token: 0x04002146 RID: 8518
			internal bool DelayLoad;

			// Token: 0x04002147 RID: 8519
			internal ArrayList ClientSinkProviders = new ArrayList();

			// Token: 0x04002148 RID: 8520
			internal ArrayList ServerSinkProviders = new ArrayList();
		}

		// Token: 0x0200073A RID: 1850
		internal class ClientWellKnownEntry
		{
			// Token: 0x06004270 RID: 17008 RVA: 0x000E290E File Offset: 0x000E190E
			internal ClientWellKnownEntry(string typeName, string assemName, string url)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemName;
				this.Url = url;
			}

			// Token: 0x04002149 RID: 8521
			internal string TypeName;

			// Token: 0x0400214A RID: 8522
			internal string AssemblyName;

			// Token: 0x0400214B RID: 8523
			internal string Url;
		}

		// Token: 0x0200073B RID: 1851
		internal class ContextAttributeEntry
		{
			// Token: 0x06004271 RID: 17009 RVA: 0x000E292B File Offset: 0x000E192B
			internal ContextAttributeEntry(string typeName, string assemName, Hashtable properties)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemName;
				this.Properties = properties;
			}

			// Token: 0x0400214C RID: 8524
			internal string TypeName;

			// Token: 0x0400214D RID: 8525
			internal string AssemblyName;

			// Token: 0x0400214E RID: 8526
			internal Hashtable Properties;
		}

		// Token: 0x0200073C RID: 1852
		internal class InteropXmlElementEntry
		{
			// Token: 0x06004272 RID: 17010 RVA: 0x000E2948 File Offset: 0x000E1948
			internal InteropXmlElementEntry(string xmlElementName, string xmlElementNamespace, string urtTypeName, string urtAssemblyName)
			{
				this.XmlElementName = xmlElementName;
				this.XmlElementNamespace = xmlElementNamespace;
				this.UrtTypeName = urtTypeName;
				this.UrtAssemblyName = urtAssemblyName;
			}

			// Token: 0x0400214F RID: 8527
			internal string XmlElementName;

			// Token: 0x04002150 RID: 8528
			internal string XmlElementNamespace;

			// Token: 0x04002151 RID: 8529
			internal string UrtTypeName;

			// Token: 0x04002152 RID: 8530
			internal string UrtAssemblyName;
		}

		// Token: 0x0200073D RID: 1853
		internal class CustomErrorsEntry
		{
			// Token: 0x06004273 RID: 17011 RVA: 0x000E296D File Offset: 0x000E196D
			internal CustomErrorsEntry(CustomErrorsModes mode)
			{
				this.Mode = mode;
			}

			// Token: 0x04002153 RID: 8531
			internal CustomErrorsModes Mode;
		}

		// Token: 0x0200073E RID: 1854
		internal class InteropXmlTypeEntry
		{
			// Token: 0x06004274 RID: 17012 RVA: 0x000E297C File Offset: 0x000E197C
			internal InteropXmlTypeEntry(string xmlTypeName, string xmlTypeNamespace, string urtTypeName, string urtAssemblyName)
			{
				this.XmlTypeName = xmlTypeName;
				this.XmlTypeNamespace = xmlTypeNamespace;
				this.UrtTypeName = urtTypeName;
				this.UrtAssemblyName = urtAssemblyName;
			}

			// Token: 0x04002154 RID: 8532
			internal string XmlTypeName;

			// Token: 0x04002155 RID: 8533
			internal string XmlTypeNamespace;

			// Token: 0x04002156 RID: 8534
			internal string UrtTypeName;

			// Token: 0x04002157 RID: 8535
			internal string UrtAssemblyName;
		}

		// Token: 0x0200073F RID: 1855
		internal class LifetimeEntry
		{
			// Token: 0x17000BCB RID: 3019
			// (get) Token: 0x06004275 RID: 17013 RVA: 0x000E29A1 File Offset: 0x000E19A1
			// (set) Token: 0x06004276 RID: 17014 RVA: 0x000E29A9 File Offset: 0x000E19A9
			internal TimeSpan LeaseTime
			{
				get
				{
					return this._leaseTime;
				}
				set
				{
					this._leaseTime = value;
					this.IsLeaseTimeSet = true;
				}
			}

			// Token: 0x17000BCC RID: 3020
			// (get) Token: 0x06004277 RID: 17015 RVA: 0x000E29B9 File Offset: 0x000E19B9
			// (set) Token: 0x06004278 RID: 17016 RVA: 0x000E29C1 File Offset: 0x000E19C1
			internal TimeSpan RenewOnCallTime
			{
				get
				{
					return this._renewOnCallTime;
				}
				set
				{
					this._renewOnCallTime = value;
					this.IsRenewOnCallTimeSet = true;
				}
			}

			// Token: 0x17000BCD RID: 3021
			// (get) Token: 0x06004279 RID: 17017 RVA: 0x000E29D1 File Offset: 0x000E19D1
			// (set) Token: 0x0600427A RID: 17018 RVA: 0x000E29D9 File Offset: 0x000E19D9
			internal TimeSpan SponsorshipTimeout
			{
				get
				{
					return this._sponsorshipTimeout;
				}
				set
				{
					this._sponsorshipTimeout = value;
					this.IsSponsorshipTimeoutSet = true;
				}
			}

			// Token: 0x17000BCE RID: 3022
			// (get) Token: 0x0600427B RID: 17019 RVA: 0x000E29E9 File Offset: 0x000E19E9
			// (set) Token: 0x0600427C RID: 17020 RVA: 0x000E29F1 File Offset: 0x000E19F1
			internal TimeSpan LeaseManagerPollTime
			{
				get
				{
					return this._leaseManagerPollTime;
				}
				set
				{
					this._leaseManagerPollTime = value;
					this.IsLeaseManagerPollTimeSet = true;
				}
			}

			// Token: 0x04002158 RID: 8536
			internal bool IsLeaseTimeSet;

			// Token: 0x04002159 RID: 8537
			internal bool IsRenewOnCallTimeSet;

			// Token: 0x0400215A RID: 8538
			internal bool IsSponsorshipTimeoutSet;

			// Token: 0x0400215B RID: 8539
			internal bool IsLeaseManagerPollTimeSet;

			// Token: 0x0400215C RID: 8540
			private TimeSpan _leaseTime;

			// Token: 0x0400215D RID: 8541
			private TimeSpan _renewOnCallTime;

			// Token: 0x0400215E RID: 8542
			private TimeSpan _sponsorshipTimeout;

			// Token: 0x0400215F RID: 8543
			private TimeSpan _leaseManagerPollTime;
		}

		// Token: 0x02000740 RID: 1856
		internal class PreLoadEntry
		{
			// Token: 0x0600427E RID: 17022 RVA: 0x000E2A09 File Offset: 0x000E1A09
			public PreLoadEntry(string typeName, string assemblyName)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemblyName;
			}

			// Token: 0x04002160 RID: 8544
			internal string TypeName;

			// Token: 0x04002161 RID: 8545
			internal string AssemblyName;
		}

		// Token: 0x02000741 RID: 1857
		internal class RemoteAppEntry
		{
			// Token: 0x0600427F RID: 17023 RVA: 0x000E2A1F File Offset: 0x000E1A1F
			internal RemoteAppEntry(string appUri)
			{
				this.AppUri = appUri;
			}

			// Token: 0x06004280 RID: 17024 RVA: 0x000E2A44 File Offset: 0x000E1A44
			internal void AddWellKnownEntry(string typeName, string assemName, string url)
			{
				RemotingXmlConfigFileData.ClientWellKnownEntry clientWellKnownEntry = new RemotingXmlConfigFileData.ClientWellKnownEntry(typeName, assemName, url);
				this.WellKnownObjects.Add(clientWellKnownEntry);
			}

			// Token: 0x06004281 RID: 17025 RVA: 0x000E2A68 File Offset: 0x000E1A68
			internal void AddActivatedEntry(string typeName, string assemName, ArrayList contextAttributes)
			{
				RemotingXmlConfigFileData.TypeEntry typeEntry = new RemotingXmlConfigFileData.TypeEntry(typeName, assemName, contextAttributes);
				this.ActivatedObjects.Add(typeEntry);
			}

			// Token: 0x04002162 RID: 8546
			internal string AppUri;

			// Token: 0x04002163 RID: 8547
			internal ArrayList WellKnownObjects = new ArrayList();

			// Token: 0x04002164 RID: 8548
			internal ArrayList ActivatedObjects = new ArrayList();
		}

		// Token: 0x02000742 RID: 1858
		internal class TypeEntry
		{
			// Token: 0x06004282 RID: 17026 RVA: 0x000E2A8B File Offset: 0x000E1A8B
			internal TypeEntry(string typeName, string assemName, ArrayList contextAttributes)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemName;
				this.ContextAttributes = contextAttributes;
			}

			// Token: 0x04002165 RID: 8549
			internal string TypeName;

			// Token: 0x04002166 RID: 8550
			internal string AssemblyName;

			// Token: 0x04002167 RID: 8551
			internal ArrayList ContextAttributes;
		}

		// Token: 0x02000743 RID: 1859
		internal class ServerWellKnownEntry : RemotingXmlConfigFileData.TypeEntry
		{
			// Token: 0x06004283 RID: 17027 RVA: 0x000E2AA8 File Offset: 0x000E1AA8
			internal ServerWellKnownEntry(string typeName, string assemName, ArrayList contextAttributes, string objURI, WellKnownObjectMode objMode)
				: base(typeName, assemName, contextAttributes)
			{
				this.ObjectURI = objURI;
				this.ObjectMode = objMode;
			}

			// Token: 0x04002168 RID: 8552
			internal string ObjectURI;

			// Token: 0x04002169 RID: 8553
			internal WellKnownObjectMode ObjectMode;
		}

		// Token: 0x02000744 RID: 1860
		internal class SinkProviderEntry
		{
			// Token: 0x06004284 RID: 17028 RVA: 0x000E2AC3 File Offset: 0x000E1AC3
			internal SinkProviderEntry(string typeName, string assemName, Hashtable properties, bool isFormatter)
			{
				this.TypeName = typeName;
				this.AssemblyName = assemName;
				this.Properties = properties;
				this.IsFormatter = isFormatter;
			}

			// Token: 0x0400216A RID: 8554
			internal string TypeName;

			// Token: 0x0400216B RID: 8555
			internal string AssemblyName;

			// Token: 0x0400216C RID: 8556
			internal Hashtable Properties;

			// Token: 0x0400216D RID: 8557
			internal ArrayList ProviderData = new ArrayList();

			// Token: 0x0400216E RID: 8558
			internal bool IsFormatter;
		}
	}
}
