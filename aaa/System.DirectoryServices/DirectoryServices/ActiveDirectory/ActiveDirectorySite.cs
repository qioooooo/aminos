using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200007A RID: 122
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ActiveDirectorySite : IDisposable
	{
		// Token: 0x0600032B RID: 811 RVA: 0x0000E640 File Offset: 0x0000D640
		public static ActiveDirectorySite FindByName(DirectoryContext context, string siteName)
		{
			ActiveDirectorySite.ValidateArgument(context, siteName);
			context = new DirectoryContext(context);
			DirectoryEntry directoryEntry;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
				string text = "CN=Sites," + (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ConfigurationNamingContext);
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, text);
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
				throw new ActiveDirectoryOperationException(Res.GetString("ADAMInstanceNotFoundInConfigSet", new object[] { context.Name }));
			}
			ActiveDirectorySite activeDirectorySite2;
			try
			{
				ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=site)(objectCategory=site)(name=" + Utils.GetEscapedFilterValue(siteName) + "))", new string[] { "distinguishedName" }, SearchScope.OneLevel, false, false);
				if (adsearcher.FindOne() == null)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySite), siteName);
				}
				ActiveDirectorySite activeDirectorySite = new ActiveDirectorySite(context, siteName, true);
				activeDirectorySite2 = activeDirectorySite;
			}
			catch (COMException ex2)
			{
				if (ex2.ErrorCode == -2147016656)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySite), siteName);
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex2);
			}
			finally
			{
				directoryEntry.Dispose();
			}
			return activeDirectorySite2;
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000E794 File Offset: 0x0000D794
		public ActiveDirectorySite(DirectoryContext context, string siteName)
		{
			ActiveDirectorySite.ValidateArgument(context, siteName);
			context = new DirectoryContext(context);
			this.context = context;
			this.name = siteName;
			DirectoryEntry directoryEntry = null;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
				string text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ConfigurationNamingContext);
				this.siteDN = "CN=Sites," + text;
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, this.siteDN);
				string text2 = "cn=" + this.name;
				text2 = Utils.GetEscapedPath(text2);
				this.cachedEntry = directoryEntry.Children.Add(text2, "site");
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
				throw new ActiveDirectoryOperationException(Res.GetString("ADAMInstanceNotFoundInConfigSet", new object[] { context.Name }));
			}
			finally
			{
				if (directoryEntry != null)
				{
					directoryEntry.Dispose();
				}
			}
			this.subnets = new ActiveDirectorySubnetCollection(context, "CN=" + siteName + "," + this.siteDN);
			string text3 = "CN=IP,CN=Inter-Site Transports," + this.siteDN;
			this.RPCBridgeheadServers = new DirectoryServerCollection(context, "CN=" + siteName + "," + this.siteDN, text3);
			text3 = "CN=SMTP,CN=Inter-Site Transports," + this.siteDN;
			this.SMTPBridgeheadServers = new DirectoryServerCollection(context, "CN=" + siteName + "," + this.siteDN, text3);
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000E958 File Offset: 0x0000D958
		internal ActiveDirectorySite(DirectoryContext context, string siteName, bool existing)
		{
			this.context = context;
			this.name = siteName;
			this.existing = existing;
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
			this.siteDN = "CN=Sites," + (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ConfigurationNamingContext);
			this.cachedEntry = DirectoryEntryManager.GetDirectoryEntry(context, "CN=" + siteName + "," + this.siteDN);
			this.subnets = new ActiveDirectorySubnetCollection(context, "CN=" + siteName + "," + this.siteDN);
			string text = "CN=IP,CN=Inter-Site Transports," + this.siteDN;
			this.RPCBridgeheadServers = new DirectoryServerCollection(context, (string)PropertyManager.GetPropertyValue(context, this.cachedEntry, PropertyManager.DistinguishedName), text);
			text = "CN=SMTP,CN=Inter-Site Transports," + this.siteDN;
			this.SMTPBridgeheadServers = new DirectoryServerCollection(context, (string)PropertyManager.GetPropertyValue(context, this.cachedEntry, PropertyManager.DistinguishedName), text);
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000EA90 File Offset: 0x0000DA90
		public static ActiveDirectorySite GetComputerSite()
		{
			new DirectoryContext(DirectoryContextType.Forest);
			IntPtr intPtr = (IntPtr)0;
			int num = UnsafeNativeMethods.DsGetSiteName(null, ref intPtr);
			if (num == 0)
			{
				ActiveDirectorySite activeDirectorySite2;
				try
				{
					string text = Marshal.PtrToStringUni(intPtr);
					string dnsForestName = Locator.GetDomainControllerInfo(null, null, null, 16L).DnsForestName;
					DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(dnsForestName, DirectoryContextType.Forest, null);
					ActiveDirectorySite activeDirectorySite = ActiveDirectorySite.FindByName(newDirectoryContext, text);
					activeDirectorySite2 = activeDirectorySite;
				}
				finally
				{
					if (intPtr != (IntPtr)0)
					{
						Marshal.FreeHGlobal(intPtr);
					}
				}
				return activeDirectorySite2;
			}
			if (num == ActiveDirectorySite.ERROR_NO_SITENAME)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("NoCurrentSite"), typeof(ActiveDirectorySite), null);
			}
			throw ExceptionHelper.GetExceptionFromErrorCode(num);
		}

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x0600032F RID: 815 RVA: 0x0000EB3C File Offset: 0x0000DB3C
		public string Name
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.name;
			}
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000330 RID: 816 RVA: 0x0000EB60 File Offset: 0x0000DB60
		public DomainCollection Domains
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && !this.domainsRetrieved)
				{
					this.domains.Clear();
					this.GetDomains();
					this.domainsRetrieved = true;
				}
				return this.domains;
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x06000331 RID: 817 RVA: 0x0000EBB4 File Offset: 0x0000DBB4
		public ActiveDirectorySubnetCollection Subnets
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && !this.subnetRetrieved)
				{
					this.subnets.initialized = false;
					this.subnets.Clear();
					this.GetSubnets();
					this.subnetRetrieved = true;
				}
				this.subnets.initialized = true;
				return this.subnets;
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x06000332 RID: 818 RVA: 0x0000EC20 File Offset: 0x0000DC20
		public ReadOnlyDirectoryServerCollection Servers
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && !this.serversRetrieved)
				{
					this.servers.Clear();
					this.GetServers();
					this.serversRetrieved = true;
				}
				return this.servers;
			}
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000EC74 File Offset: 0x0000DC74
		public ReadOnlySiteCollection AdjacentSites
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && !this.adjacentSitesRetrieved)
				{
					this.adjacentSites.Clear();
					this.GetAdjacentSites();
					this.adjacentSitesRetrieved = true;
				}
				return this.adjacentSites;
			}
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000334 RID: 820 RVA: 0x0000ECC8 File Offset: 0x0000DCC8
		public ReadOnlySiteLinkCollection SiteLinks
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && !this.belongLinksRetrieved)
				{
					this.links.Clear();
					this.GetLinks();
					this.belongLinksRetrieved = true;
				}
				return this.links;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000ED1C File Offset: 0x0000DD1C
		// (set) Token: 0x06000336 RID: 822 RVA: 0x0000EE90 File Offset: 0x0000DE90
		public DirectoryServer InterSiteTopologyGenerator
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && this.topologyGenerator == null && !this.topologyTouched)
				{
					bool flag;
					try
					{
						flag = this.NTDSSiteEntry.Properties.Contains("interSiteTopologyGenerator");
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
					if (flag)
					{
						string text = (string)PropertyManager.GetPropertyValue(this.context, this.NTDSSiteEntry, PropertyManager.InterSiteTopologyGenerator);
						string text2 = null;
						DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text);
						try
						{
							text2 = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry.Parent, PropertyManager.DnsHostName);
						}
						catch (COMException ex2)
						{
							if (ex2.ErrorCode == -2147016656)
							{
								return null;
							}
						}
						if (this.IsADAM)
						{
							int num = (int)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.MsDSPortLDAP);
							string text3 = text2;
							if (num != 389)
							{
								text3 = text2 + ":" + num;
							}
							this.topologyGenerator = new AdamInstance(Utils.GetNewDirectoryContext(text3, DirectoryContextType.DirectoryServer, this.context), text3);
							goto IL_0142;
						}
						this.topologyGenerator = new DomainController(Utils.GetNewDirectoryContext(text2, DirectoryContextType.DirectoryServer, this.context), text2);
					}
				}
				IL_0142:
				return this.topologyGenerator;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (this.existing)
				{
					DirectoryEntry ntdssiteEntry = this.NTDSSiteEntry;
				}
				this.topologyTouched = true;
				this.topologyGenerator = value;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000337 RID: 823 RVA: 0x0000EEE4 File Offset: 0x0000DEE4
		// (set) Token: 0x06000338 RID: 824 RVA: 0x0000EF78 File Offset: 0x0000DF78
		public ActiveDirectorySiteOptions Options
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing)
				{
					try
					{
						if (this.NTDSSiteEntry.Properties.Contains("options"))
						{
							return (ActiveDirectorySiteOptions)this.NTDSSiteEntry.Properties["options"][0];
						}
						return ActiveDirectorySiteOptions.None;
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
				}
				return this.siteOptions;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing)
				{
					try
					{
						this.NTDSSiteEntry.Properties["options"].Value = value;
						return;
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
				}
				this.siteOptions = value;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000339 RID: 825 RVA: 0x0000EFF0 File Offset: 0x0000DFF0
		// (set) Token: 0x0600033A RID: 826 RVA: 0x0000F074 File Offset: 0x0000E074
		public string Location
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				string text;
				try
				{
					if (this.cachedEntry.Properties.Contains("location"))
					{
						text = (string)this.cachedEntry.Properties["location"][0];
					}
					else
					{
						text = null;
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				return text;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				try
				{
					if (value == null)
					{
						if (this.cachedEntry.Properties.Contains("location"))
						{
							this.cachedEntry.Properties["location"].Clear();
						}
					}
					else
					{
						this.cachedEntry.Properties["location"].Value = value;
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x0600033B RID: 827 RVA: 0x0000F10C File Offset: 0x0000E10C
		public ReadOnlyDirectoryServerCollection BridgeheadServers
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (!this.bridgeheadServerRetrieved)
				{
					this.bridgeheadServers = this.GetBridgeheadServers();
					this.bridgeheadServerRetrieved = true;
				}
				return this.bridgeheadServers;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x0600033C RID: 828 RVA: 0x0000F148 File Offset: 0x0000E148
		public DirectoryServerCollection PreferredSmtpBridgeheadServers
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && !this.SMTPBridgeRetrieved)
				{
					this.SMTPBridgeheadServers.initialized = false;
					this.SMTPBridgeheadServers.Clear();
					this.GetPreferredBridgeheadServers(ActiveDirectoryTransportType.Smtp);
					this.SMTPBridgeRetrieved = true;
				}
				this.SMTPBridgeheadServers.initialized = true;
				return this.SMTPBridgeheadServers;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x0600033D RID: 829 RVA: 0x0000F1B8 File Offset: 0x0000E1B8
		public DirectoryServerCollection PreferredRpcBridgeheadServers
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && !this.RPCBridgeRetrieved)
				{
					this.RPCBridgeheadServers.initialized = false;
					this.RPCBridgeheadServers.Clear();
					this.GetPreferredBridgeheadServers(ActiveDirectoryTransportType.Rpc);
					this.RPCBridgeRetrieved = true;
				}
				this.RPCBridgeheadServers.initialized = true;
				return this.RPCBridgeheadServers;
			}
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x0600033E RID: 830 RVA: 0x0000F228 File Offset: 0x0000E228
		// (set) Token: 0x0600033F RID: 831 RVA: 0x0000F2D8 File Offset: 0x0000E2D8
		public ActiveDirectorySchedule IntraSiteReplicationSchedule
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				ActiveDirectorySchedule activeDirectorySchedule = null;
				if (this.existing)
				{
					try
					{
						if (this.NTDSSiteEntry.Properties.Contains("schedule"))
						{
							byte[] array = (byte[])this.NTDSSiteEntry.Properties["schedule"][0];
							activeDirectorySchedule = new ActiveDirectorySchedule();
							activeDirectorySchedule.SetUnmanagedSchedule(array);
						}
						return activeDirectorySchedule;
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
				}
				if (this.replicationSchedule != null)
				{
					activeDirectorySchedule = new ActiveDirectorySchedule();
					activeDirectorySchedule.SetUnmanagedSchedule(this.replicationSchedule);
				}
				return activeDirectorySchedule;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing)
				{
					try
					{
						if (value == null)
						{
							if (this.NTDSSiteEntry.Properties.Contains("schedule"))
							{
								this.NTDSSiteEntry.Properties["schedule"].Clear();
							}
						}
						else
						{
							this.NTDSSiteEntry.Properties["schedule"].Value = value.GetUnmanagedSchedule();
						}
						return;
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
				}
				if (value == null)
				{
					this.replicationSchedule = null;
					return;
				}
				this.replicationSchedule = value.GetUnmanagedSchedule();
			}
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x06000340 RID: 832 RVA: 0x0000F394 File Offset: 0x0000E394
		private bool IsADAM
		{
			get
			{
				if (!this.checkADAM)
				{
					DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
					PropertyValueCollection propertyValueCollection = null;
					try
					{
						propertyValueCollection = directoryEntry.Properties["supportedCapabilities"];
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
					if (propertyValueCollection.Contains(SupportedCapability.ADAMOid))
					{
						this.isADAMServer = true;
					}
				}
				return this.isADAMServer;
			}
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x06000341 RID: 833 RVA: 0x0000F404 File Offset: 0x0000E404
		private DirectoryEntry NTDSSiteEntry
		{
			get
			{
				if (this.ntdsEntry == null)
				{
					DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, "CN=NTDS Site Settings," + (string)PropertyManager.GetPropertyValue(this.context, this.cachedEntry, PropertyManager.DistinguishedName));
					try
					{
						directoryEntry.RefreshCache();
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode == -2147016656)
						{
							string @string = Res.GetString("NTDSSiteSetting", new object[] { this.name });
							throw new ActiveDirectoryOperationException(@string, ex, 8240);
						}
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
					this.ntdsEntry = directoryEntry;
				}
				return this.ntdsEntry;
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x0000F4B8 File Offset: 0x0000E4B8
		public void Save()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			try
			{
				this.cachedEntry.CommitChanges();
				foreach (object obj in this.subnets.changeList)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					try
					{
						((DirectoryEntry)dictionaryEntry.Value).CommitChanges();
					}
					catch (COMException ex)
					{
						if (ex.ErrorCode != -2147016694)
						{
							throw ExceptionHelper.GetExceptionFromCOMException(ex);
						}
					}
				}
				this.subnets.changeList.Clear();
				this.subnetRetrieved = false;
				foreach (object obj2 in this.SMTPBridgeheadServers.changeList)
				{
					DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj2;
					try
					{
						((DirectoryEntry)dictionaryEntry2.Value).CommitChanges();
					}
					catch (COMException ex2)
					{
						if (this.IsADAM && ex2.ErrorCode == -2147016657)
						{
							throw new NotSupportedException(Res.GetString("NotSupportTransportSMTP"));
						}
						if (ex2.ErrorCode != -2147016694)
						{
							throw ExceptionHelper.GetExceptionFromCOMException(ex2);
						}
					}
				}
				this.SMTPBridgeheadServers.changeList.Clear();
				this.SMTPBridgeRetrieved = false;
				foreach (object obj3 in this.RPCBridgeheadServers.changeList)
				{
					DictionaryEntry dictionaryEntry3 = (DictionaryEntry)obj3;
					try
					{
						((DirectoryEntry)dictionaryEntry3.Value).CommitChanges();
					}
					catch (COMException ex3)
					{
						if (ex3.ErrorCode != -2147016694)
						{
							throw ExceptionHelper.GetExceptionFromCOMException(ex3);
						}
					}
				}
				this.RPCBridgeheadServers.changeList.Clear();
				this.RPCBridgeRetrieved = false;
				if (this.existing)
				{
					if (this.topologyTouched)
					{
						try
						{
							DirectoryServer interSiteTopologyGenerator = this.InterSiteTopologyGenerator;
							string text = ((interSiteTopologyGenerator is DomainController) ? ((DomainController)interSiteTopologyGenerator).NtdsaObjectName : ((AdamInstance)interSiteTopologyGenerator).NtdsaObjectName);
							this.NTDSSiteEntry.Properties["interSiteTopologyGenerator"].Value = text;
						}
						catch (COMException ex4)
						{
							throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex4);
						}
					}
					this.NTDSSiteEntry.CommitChanges();
					this.topologyTouched = false;
				}
				else
				{
					try
					{
						DirectoryEntry directoryEntry = this.cachedEntry.Children.Add("CN=NTDS Site Settings", "nTDSSiteSettings");
						DirectoryServer interSiteTopologyGenerator2 = this.InterSiteTopologyGenerator;
						if (interSiteTopologyGenerator2 != null)
						{
							string text2 = ((interSiteTopologyGenerator2 is DomainController) ? ((DomainController)interSiteTopologyGenerator2).NtdsaObjectName : ((AdamInstance)interSiteTopologyGenerator2).NtdsaObjectName);
							directoryEntry.Properties["interSiteTopologyGenerator"].Value = text2;
						}
						directoryEntry.Properties["options"].Value = this.siteOptions;
						if (this.replicationSchedule != null)
						{
							directoryEntry.Properties["schedule"].Value = this.replicationSchedule;
						}
						directoryEntry.CommitChanges();
						this.ntdsEntry = directoryEntry;
						directoryEntry = this.cachedEntry.Children.Add("CN=Servers", "serversContainer");
						directoryEntry.CommitChanges();
						if (!this.IsADAM)
						{
							directoryEntry = this.cachedEntry.Children.Add("CN=Licensing Site Settings", "licensingSiteSettings");
							directoryEntry.CommitChanges();
						}
					}
					finally
					{
						this.existing = true;
					}
				}
			}
			catch (COMException ex5)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex5);
			}
		}

		// Token: 0x06000343 RID: 835 RVA: 0x0000F91C File Offset: 0x0000E91C
		public void Delete()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (!this.existing)
			{
				throw new InvalidOperationException(Res.GetString("CannotDelete"));
			}
			try
			{
				this.cachedEntry.DeleteTree();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x06000344 RID: 836 RVA: 0x0000F988 File Offset: 0x0000E988
		public override string ToString()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			return this.name;
		}

		// Token: 0x06000345 RID: 837 RVA: 0x0000F9AC File Offset: 0x0000E9AC
		private ReadOnlyDirectoryServerCollection GetBridgeheadServers()
		{
			NativeComInterfaces.IAdsPathname adsPathname = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
			adsPathname.EscapedMode = 4;
			ReadOnlyDirectoryServerCollection readOnlyDirectoryServerCollection = new ReadOnlyDirectoryServerCollection();
			if (this.existing)
			{
				Hashtable hashtable = new Hashtable();
				Hashtable hashtable2 = new Hashtable();
				Hashtable hashtable3 = new Hashtable();
				string text = "CN=Servers," + (string)PropertyManager.GetPropertyValue(this.context, this.cachedEntry, PropertyManager.DistinguishedName);
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text);
				try
				{
					ADSearcher adsearcher = new ADSearcher(directoryEntry, "(|(objectCategory=server)(objectCategory=NTDSConnection))", new string[] { "fromServer", "distinguishedName", "dNSHostName", "objectCategory" }, SearchScope.Subtree, true, true);
					SearchResultCollection searchResultCollection = null;
					try
					{
						searchResultCollection = adsearcher.FindAll();
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
					try
					{
						foreach (object obj in searchResultCollection)
						{
							SearchResult searchResult = (SearchResult)obj;
							string text2 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.ObjectCategory);
							if (Utils.Compare(text2, 0, "CN=Server".Length, "CN=Server", 0, "CN=Server".Length) == 0)
							{
								hashtable3.Add((string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DistinguishedName), (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DnsHostName));
							}
						}
						foreach (object obj2 in searchResultCollection)
						{
							SearchResult searchResult2 = (SearchResult)obj2;
							string text3 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult2, PropertyManager.ObjectCategory);
							if (Utils.Compare(text3, 0, "CN=Server".Length, "CN=Server", 0, "CN=Server".Length) != 0)
							{
								string text4 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult2, PropertyManager.FromServer);
								string text5 = Utils.GetPartialDN(text4, 3);
								adsPathname.Set(text5, 4);
								text5 = adsPathname.Retrieve(11);
								text5 = text5.Substring(3);
								string partialDN = Utils.GetPartialDN((string)PropertyManager.GetSearchResultPropertyValue(searchResult2, PropertyManager.DistinguishedName), 2);
								if (!hashtable.Contains(partialDN))
								{
									string text6 = (string)hashtable3[partialDN];
									if (!hashtable2.Contains(partialDN))
									{
										hashtable2.Add(partialDN, text6);
									}
									if (Utils.Compare((string)PropertyManager.GetPropertyValue(this.context, this.cachedEntry, PropertyManager.Cn), text5) != 0)
									{
										hashtable.Add(partialDN, text6);
										hashtable2.Remove(partialDN);
									}
								}
							}
						}
					}
					finally
					{
						searchResultCollection.Dispose();
					}
				}
				finally
				{
					directoryEntry.Dispose();
				}
				if (hashtable2.Count != 0)
				{
					DirectoryEntry directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(this.context, this.siteDN);
					StringBuilder stringBuilder = new StringBuilder(100);
					if (hashtable2.Count > 1)
					{
						stringBuilder.Append("(|");
					}
					foreach (object obj3 in hashtable2)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj3;
						stringBuilder.Append("(fromServer=");
						stringBuilder.Append("CN=NTDS Settings,");
						stringBuilder.Append(Utils.GetEscapedFilterValue((string)dictionaryEntry.Key));
						stringBuilder.Append(")");
					}
					if (hashtable2.Count > 1)
					{
						stringBuilder.Append(")");
					}
					ADSearcher adsearcher2 = new ADSearcher(directoryEntry2, "(&(objectClass=nTDSConnection)(objectCategory=NTDSConnection)" + stringBuilder.ToString() + ")", new string[] { "fromServer", "distinguishedName" }, SearchScope.Subtree);
					SearchResultCollection searchResultCollection2 = null;
					try
					{
						searchResultCollection2 = adsearcher2.FindAll();
					}
					catch (COMException ex2)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex2);
					}
					try
					{
						foreach (object obj4 in searchResultCollection2)
						{
							SearchResult searchResult3 = (SearchResult)obj4;
							string text7 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult3, PropertyManager.FromServer);
							string text8 = text7.Substring(17);
							if (hashtable2.Contains(text8))
							{
								string text9 = Utils.GetPartialDN((string)PropertyManager.GetSearchResultPropertyValue(searchResult3, PropertyManager.DistinguishedName), 4);
								adsPathname.Set(text9, 4);
								text9 = adsPathname.Retrieve(11);
								text9 = text9.Substring(3);
								if (Utils.Compare(text9, (string)PropertyManager.GetPropertyValue(this.context, this.cachedEntry, PropertyManager.Cn)) != 0)
								{
									string text10 = (string)hashtable2[text8];
									hashtable2.Remove(text8);
									hashtable.Add(text8, text10);
								}
							}
						}
					}
					finally
					{
						searchResultCollection2.Dispose();
						directoryEntry2.Dispose();
					}
				}
				foreach (object obj5 in hashtable)
				{
					DictionaryEntry dictionaryEntry2 = (DictionaryEntry)obj5;
					string text11 = (string)dictionaryEntry2.Value;
					DirectoryServer directoryServer;
					if (this.IsADAM)
					{
						DirectoryEntry directoryEntry3 = DirectoryEntryManager.GetDirectoryEntry(this.context, "CN=NTDS Settings," + dictionaryEntry2.Key);
						int num = (int)PropertyManager.GetPropertyValue(this.context, directoryEntry3, PropertyManager.MsDSPortLDAP);
						string text12 = text11;
						if (num != 389)
						{
							text12 = text11 + ":" + num;
						}
						directoryServer = new AdamInstance(Utils.GetNewDirectoryContext(text12, DirectoryContextType.DirectoryServer, this.context), text12);
					}
					else
					{
						directoryServer = new DomainController(Utils.GetNewDirectoryContext(text11, DirectoryContextType.DirectoryServer, this.context), text11);
					}
					readOnlyDirectoryServerCollection.Add(directoryServer);
				}
			}
			return readOnlyDirectoryServerCollection;
		}

		// Token: 0x06000346 RID: 838 RVA: 0x0001005C File Offset: 0x0000F05C
		public DirectoryEntry GetDirectoryEntry()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (!this.existing)
			{
				throw new InvalidOperationException(Res.GetString("CannotGetObject"));
			}
			return DirectoryEntryManager.GetDirectoryEntryInternal(this.context, this.cachedEntry.Path);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000100B0 File Offset: 0x0000F0B0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000100BF File Offset: 0x0000F0BF
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.cachedEntry != null)
				{
					this.cachedEntry.Dispose();
				}
				if (this.ntdsEntry != null)
				{
					this.ntdsEntry.Dispose();
				}
			}
			this.disposed = true;
		}

		// Token: 0x06000349 RID: 841 RVA: 0x000100F4 File Offset: 0x0000F0F4
		private static void ValidateArgument(DirectoryContext context, string siteName)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.Name == null && !context.isRootDomain())
			{
				throw new ArgumentException(Res.GetString("ContextNotAssociatedWithDomain"), "context");
			}
			if (context.Name != null && !context.isRootDomain() && !context.isServer() && !context.isADAMConfigSet())
			{
				throw new ArgumentException(Res.GetString("NotADOrADAM"), "context");
			}
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			if (siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
		}

		// Token: 0x0600034A RID: 842 RVA: 0x00010194 File Offset: 0x0000F194
		private void GetSubnets()
		{
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
			string text = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.ConfigurationNamingContext);
			string text2 = "CN=Subnets,CN=Sites," + text;
			directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text2);
			ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=subnet)(objectCategory=subnet)(siteObject=" + Utils.GetEscapedFilterValue((string)PropertyManager.GetPropertyValue(this.context, this.cachedEntry, PropertyManager.DistinguishedName)) + "))", new string[] { "cn", "location" }, SearchScope.OneLevel);
			SearchResultCollection searchResultCollection = null;
			try
			{
				searchResultCollection = adsearcher.FindAll();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			try
			{
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text3 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.Cn);
					ActiveDirectorySubnet activeDirectorySubnet = new ActiveDirectorySubnet(this.context, text3, null, true);
					activeDirectorySubnet.cachedEntry = searchResult.GetDirectoryEntry();
					activeDirectorySubnet.Site = this;
					this.subnets.Add(activeDirectorySubnet);
				}
			}
			finally
			{
				searchResultCollection.Dispose();
				directoryEntry.Dispose();
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x0001030C File Offset: 0x0000F30C
		private void GetAdjacentSites()
		{
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
			string text = (string)directoryEntry.Properties["configurationNamingContext"][0];
			string text2 = "CN=Inter-Site Transports,CN=Sites," + text;
			directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text2);
			ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=siteLink)(objectCategory=SiteLink)(siteList=" + Utils.GetEscapedFilterValue((string)PropertyManager.GetPropertyValue(this.context, this.cachedEntry, PropertyManager.DistinguishedName)) + "))", new string[] { "cn", "distinguishedName" }, SearchScope.Subtree);
			SearchResultCollection searchResultCollection = null;
			try
			{
				searchResultCollection = adsearcher.FindAll();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			try
			{
				ActiveDirectorySiteLink activeDirectorySiteLink = null;
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text3 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DistinguishedName);
					string text4 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.Cn);
					string value = Utils.GetDNComponents(text3)[1].Value;
					ActiveDirectoryTransportType activeDirectoryTransportType;
					if (string.Compare(value, "IP", StringComparison.OrdinalIgnoreCase) == 0)
					{
						activeDirectoryTransportType = ActiveDirectoryTransportType.Rpc;
					}
					else
					{
						if (string.Compare(value, "SMTP", StringComparison.OrdinalIgnoreCase) != 0)
						{
							string @string = Res.GetString("UnknownTransport", new object[] { value });
							throw new ActiveDirectoryOperationException(@string);
						}
						activeDirectoryTransportType = ActiveDirectoryTransportType.Smtp;
					}
					try
					{
						activeDirectorySiteLink = new ActiveDirectorySiteLink(this.context, text4, activeDirectoryTransportType, true, searchResult.GetDirectoryEntry());
						foreach (object obj2 in activeDirectorySiteLink.Sites)
						{
							ActiveDirectorySite activeDirectorySite = (ActiveDirectorySite)obj2;
							if (Utils.Compare(activeDirectorySite.Name, this.Name) != 0 && !this.adjacentSites.Contains(activeDirectorySite))
							{
								this.adjacentSites.Add(activeDirectorySite);
							}
						}
					}
					finally
					{
						activeDirectorySiteLink.Dispose();
					}
				}
			}
			finally
			{
				searchResultCollection.Dispose();
				directoryEntry.Dispose();
			}
		}

		// Token: 0x0600034C RID: 844 RVA: 0x000105B4 File Offset: 0x0000F5B4
		private void GetLinks()
		{
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
			string text = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.ConfigurationNamingContext);
			string text2 = "CN=Inter-Site Transports,CN=Sites," + text;
			directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text2);
			ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=siteLink)(objectCategory=SiteLink)(siteList=" + Utils.GetEscapedFilterValue((string)PropertyManager.GetPropertyValue(this.context, this.cachedEntry, PropertyManager.DistinguishedName)) + "))", new string[] { "cn", "distinguishedName" }, SearchScope.Subtree);
			SearchResultCollection searchResultCollection = null;
			try
			{
				searchResultCollection = adsearcher.FindAll();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			try
			{
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					DirectoryEntry directoryEntry2 = searchResult.GetDirectoryEntry();
					string text3 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.Cn);
					string value = Utils.GetDNComponents((string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DistinguishedName))[1].Value;
					ActiveDirectorySiteLink activeDirectorySiteLink;
					if (string.Compare(value, "IP", StringComparison.OrdinalIgnoreCase) == 0)
					{
						activeDirectorySiteLink = new ActiveDirectorySiteLink(this.context, text3, ActiveDirectoryTransportType.Rpc, true, directoryEntry2);
					}
					else
					{
						if (string.Compare(value, "SMTP", StringComparison.OrdinalIgnoreCase) != 0)
						{
							string @string = Res.GetString("UnknownTransport", new object[] { value });
							throw new ActiveDirectoryOperationException(@string);
						}
						activeDirectorySiteLink = new ActiveDirectorySiteLink(this.context, text3, ActiveDirectoryTransportType.Smtp, true, directoryEntry2);
					}
					this.links.Add(activeDirectorySiteLink);
				}
			}
			finally
			{
				searchResultCollection.Dispose();
				directoryEntry.Dispose();
			}
		}

		// Token: 0x0600034D RID: 845 RVA: 0x000107A4 File Offset: 0x0000F7A4
		private void GetDomains()
		{
			if (!this.IsADAM)
			{
				string currentServerName = this.cachedEntry.Options.GetCurrentServerName();
				DomainController domainController = DomainController.GetDomainController(Utils.GetNewDirectoryContext(currentServerName, DirectoryContextType.DirectoryServer, this.context));
				IntPtr handle = domainController.Handle;
				IntPtr intPtr = (IntPtr)0;
				IntPtr intPtr2 = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsListDomainsInSiteW");
				if (intPtr2 == (IntPtr)0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
				}
				UnsafeNativeMethods.DsListDomainsInSiteW dsListDomainsInSiteW = (UnsafeNativeMethods.DsListDomainsInSiteW)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.DsListDomainsInSiteW));
				int num = dsListDomainsInSiteW(handle, (string)PropertyManager.GetPropertyValue(this.context, this.cachedEntry, PropertyManager.DistinguishedName), ref intPtr);
				if (num != 0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(num, currentServerName);
				}
				try
				{
					DS_NAME_RESULT ds_NAME_RESULT = new DS_NAME_RESULT();
					Marshal.PtrToStructure(intPtr, ds_NAME_RESULT);
					int cItems = ds_NAME_RESULT.cItems;
					IntPtr rItems = ds_NAME_RESULT.rItems;
					if (cItems > 0)
					{
						Marshal.ReadInt32(rItems);
						IntPtr intPtr3 = (IntPtr)0;
						for (int i = 0; i < cItems; i++)
						{
							intPtr3 = Utils.AddToIntPtr(rItems, Marshal.SizeOf(typeof(DS_NAME_RESULT_ITEM)) * i);
							DS_NAME_RESULT_ITEM ds_NAME_RESULT_ITEM = new DS_NAME_RESULT_ITEM();
							Marshal.PtrToStructure(intPtr3, ds_NAME_RESULT_ITEM);
							if (ds_NAME_RESULT_ITEM.status == DS_NAME_ERROR.DS_NAME_NO_ERROR || ds_NAME_RESULT_ITEM.status == DS_NAME_ERROR.DS_NAME_ERROR_DOMAIN_ONLY)
							{
								string text = Marshal.PtrToStringUni(ds_NAME_RESULT_ITEM.pName);
								if (text != null && text.Length > 0)
								{
									string dnsNameFromDN = Utils.GetDnsNameFromDN(text);
									Domain domain = new Domain(Utils.GetNewDirectoryContext(dnsNameFromDN, DirectoryContextType.Domain, this.context), dnsNameFromDN);
									this.domains.Add(domain);
								}
							}
						}
					}
				}
				finally
				{
					intPtr2 = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsFreeNameResultW");
					if (intPtr2 == (IntPtr)0)
					{
						throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
					}
					UnsafeNativeMethods.DsFreeNameResultW dsFreeNameResultW = (UnsafeNativeMethods.DsFreeNameResultW)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.DsFreeNameResultW));
					dsFreeNameResultW(intPtr);
				}
			}
		}

		// Token: 0x0600034E RID: 846 RVA: 0x0001099C File Offset: 0x0000F99C
		private void GetServers()
		{
			ADSearcher adsearcher = new ADSearcher(this.cachedEntry, "(&(objectClass=server)(objectCategory=server))", new string[] { "dNSHostName" }, SearchScope.Subtree);
			SearchResultCollection searchResultCollection = null;
			try
			{
				searchResultCollection = adsearcher.FindAll();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			try
			{
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DnsHostName);
					DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
					DirectoryEntry directoryEntry2 = null;
					try
					{
						directoryEntry2 = directoryEntry.Children.Find("CN=NTDS Settings", "nTDSDSA");
					}
					catch (COMException ex2)
					{
						if (ex2.ErrorCode == -2147016656)
						{
							continue;
						}
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex2);
					}
					DirectoryServer directoryServer;
					if (this.IsADAM)
					{
						int num = (int)PropertyManager.GetPropertyValue(this.context, directoryEntry2, PropertyManager.MsDSPortLDAP);
						string text2 = text;
						if (num != 389)
						{
							text2 = text + ":" + num;
						}
						directoryServer = new AdamInstance(Utils.GetNewDirectoryContext(text2, DirectoryContextType.DirectoryServer, this.context), text2);
					}
					else
					{
						directoryServer = new DomainController(Utils.GetNewDirectoryContext(text, DirectoryContextType.DirectoryServer, this.context), text);
					}
					this.servers.Add(directoryServer);
				}
			}
			finally
			{
				searchResultCollection.Dispose();
			}
		}

		// Token: 0x0600034F RID: 847 RVA: 0x00010B70 File Offset: 0x0000FB70
		private void GetPreferredBridgeheadServers(ActiveDirectoryTransportType transport)
		{
			string text = "CN=Servers," + PropertyManager.GetPropertyValue(this.context, this.cachedEntry, PropertyManager.DistinguishedName);
			string text2;
			if (transport == ActiveDirectoryTransportType.Smtp)
			{
				text2 = "CN=SMTP,CN=Inter-Site Transports," + this.siteDN;
			}
			else
			{
				text2 = "CN=IP,CN=Inter-Site Transports," + this.siteDN;
			}
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text);
			ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=server)(objectCategory=Server)(bridgeheadTransportList=" + Utils.GetEscapedFilterValue(text2) + "))", new string[] { "dNSHostName", "distinguishedName" }, SearchScope.OneLevel);
			SearchResultCollection searchResultCollection = null;
			try
			{
				searchResultCollection = adsearcher.FindAll();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			try
			{
				DirectoryEntry directoryEntry2 = null;
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text3 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.DnsHostName);
					DirectoryEntry directoryEntry3 = searchResult.GetDirectoryEntry();
					try
					{
						directoryEntry2 = directoryEntry3.Children.Find("CN=NTDS Settings", "nTDSDSA");
					}
					catch (COMException ex2)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex2);
					}
					DirectoryServer directoryServer;
					if (this.IsADAM)
					{
						int num = (int)PropertyManager.GetPropertyValue(this.context, directoryEntry2, PropertyManager.MsDSPortLDAP);
						string text4 = text3;
						if (num != 389)
						{
							text4 = text3 + ":" + num;
						}
						directoryServer = new AdamInstance(Utils.GetNewDirectoryContext(text4, DirectoryContextType.DirectoryServer, this.context), text4);
					}
					else
					{
						directoryServer = new DomainController(Utils.GetNewDirectoryContext(text3, DirectoryContextType.DirectoryServer, this.context), text3);
					}
					if (transport == ActiveDirectoryTransportType.Smtp)
					{
						this.SMTPBridgeheadServers.Add(directoryServer);
					}
					else
					{
						this.RPCBridgeheadServers.Add(directoryServer);
					}
				}
			}
			finally
			{
				directoryEntry.Dispose();
				searchResultCollection.Dispose();
			}
		}

		// Token: 0x04000336 RID: 822
		internal DirectoryContext context;

		// Token: 0x04000337 RID: 823
		private string name;

		// Token: 0x04000338 RID: 824
		internal DirectoryEntry cachedEntry;

		// Token: 0x04000339 RID: 825
		private DirectoryEntry ntdsEntry;

		// Token: 0x0400033A RID: 826
		private ActiveDirectorySubnetCollection subnets;

		// Token: 0x0400033B RID: 827
		private DirectoryServer topologyGenerator;

		// Token: 0x0400033C RID: 828
		private ReadOnlySiteCollection adjacentSites = new ReadOnlySiteCollection();

		// Token: 0x0400033D RID: 829
		private bool disposed;

		// Token: 0x0400033E RID: 830
		private DomainCollection domains = new DomainCollection(null);

		// Token: 0x0400033F RID: 831
		private ReadOnlyDirectoryServerCollection servers = new ReadOnlyDirectoryServerCollection();

		// Token: 0x04000340 RID: 832
		private ReadOnlySiteLinkCollection links = new ReadOnlySiteLinkCollection();

		// Token: 0x04000341 RID: 833
		private ActiveDirectorySiteOptions siteOptions;

		// Token: 0x04000342 RID: 834
		private ReadOnlyDirectoryServerCollection bridgeheadServers = new ReadOnlyDirectoryServerCollection();

		// Token: 0x04000343 RID: 835
		private DirectoryServerCollection SMTPBridgeheadServers;

		// Token: 0x04000344 RID: 836
		private DirectoryServerCollection RPCBridgeheadServers;

		// Token: 0x04000345 RID: 837
		private byte[] replicationSchedule;

		// Token: 0x04000346 RID: 838
		internal bool existing;

		// Token: 0x04000347 RID: 839
		private bool subnetRetrieved;

		// Token: 0x04000348 RID: 840
		private bool isADAMServer;

		// Token: 0x04000349 RID: 841
		private bool checkADAM;

		// Token: 0x0400034A RID: 842
		private bool topologyTouched;

		// Token: 0x0400034B RID: 843
		private bool adjacentSitesRetrieved;

		// Token: 0x0400034C RID: 844
		private string siteDN;

		// Token: 0x0400034D RID: 845
		private bool domainsRetrieved;

		// Token: 0x0400034E RID: 846
		private bool serversRetrieved;

		// Token: 0x0400034F RID: 847
		private bool belongLinksRetrieved;

		// Token: 0x04000350 RID: 848
		private bool bridgeheadServerRetrieved;

		// Token: 0x04000351 RID: 849
		private bool SMTPBridgeRetrieved;

		// Token: 0x04000352 RID: 850
		private bool RPCBridgeRetrieved;

		// Token: 0x04000353 RID: 851
		private static int ERROR_NO_SITENAME = 1919;
	}
}
