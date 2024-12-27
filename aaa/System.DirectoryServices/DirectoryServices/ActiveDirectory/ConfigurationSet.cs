using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200008E RID: 142
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ConfigurationSet
	{
		// Token: 0x06000459 RID: 1113 RVA: 0x0001817E File Offset: 0x0001717E
		internal ConfigurationSet(DirectoryContext context, string configSetName, DirectoryEntryManager directoryEntryMgr)
		{
			this.context = context;
			this.configSetName = configSetName;
			this.directoryEntryMgr = directoryEntryMgr;
		}

		// Token: 0x0600045A RID: 1114 RVA: 0x000181A2 File Offset: 0x000171A2
		internal ConfigurationSet(DirectoryContext context, string configSetName)
			: this(context, configSetName, new DirectoryEntryManager(context))
		{
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x000181B2 File Offset: 0x000171B2
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000181BC File Offset: 0x000171BC
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					foreach (object obj in this.directoryEntryMgr.GetCachedDirectoryEntries())
					{
						DirectoryEntry directoryEntry = (DirectoryEntry)obj;
						directoryEntry.Dispose();
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x0001822C File Offset: 0x0001722C
		public static ConfigurationSet GetConfigurationSet(DirectoryContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.ConfigurationSet && context.ContextType != DirectoryContextType.DirectoryServer)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeServerORConfigSet"), "context");
			}
			if (context.isServer() || context.isADAMConfigSet())
			{
				context = new DirectoryContext(context);
				DirectoryEntryManager directoryEntryManager = new DirectoryEntryManager(context);
				string text = null;
				try
				{
					DirectoryEntry cachedDirectoryEntry = directoryEntryManager.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
					if (context.isServer() && !Utils.CheckCapability(cachedDirectoryEntry, Capability.ActiveDirectoryApplicationMode))
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("AINotFound", new object[] { context.Name }), typeof(ConfigurationSet), null);
					}
					text = (string)PropertyManager.GetPropertyValue(context, cachedDirectoryEntry, PropertyManager.ConfigurationNamingContext);
				}
				catch (COMException ex)
				{
					int errorCode = ex.ErrorCode;
					if (errorCode != -2147016646)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
					}
					if (context.ContextType == DirectoryContextType.ConfigurationSet)
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ConfigSetNotFound"), typeof(ConfigurationSet), context.Name);
					}
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("AINotFound", new object[] { context.Name }), typeof(ConfigurationSet), null);
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					if (context.ContextType == DirectoryContextType.ConfigurationSet)
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ConfigSetNotFound"), typeof(ConfigurationSet), context.Name);
					}
					throw;
				}
				return new ConfigurationSet(context, text, directoryEntryManager);
			}
			if (context.ContextType == DirectoryContextType.ConfigurationSet)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ConfigSetNotFound"), typeof(ConfigurationSet), context.Name);
			}
			throw new ActiveDirectoryObjectNotFoundException(Res.GetString("AINotFound", new object[] { context.Name }), typeof(ConfigurationSet), null);
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x0001840C File Offset: 0x0001740C
		public AdamInstance FindAdamInstance()
		{
			this.CheckIfDisposed();
			return ConfigurationSet.FindOneAdamInstance(this.Name, this.context, null, null);
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00018427 File Offset: 0x00017427
		public AdamInstance FindAdamInstance(string partitionName)
		{
			this.CheckIfDisposed();
			if (partitionName == null)
			{
				throw new ArgumentNullException("partitionName");
			}
			return ConfigurationSet.FindOneAdamInstance(this.Name, this.context, partitionName, null);
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00018450 File Offset: 0x00017450
		public AdamInstance FindAdamInstance(string partitionName, string siteName)
		{
			this.CheckIfDisposed();
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			return ConfigurationSet.FindOneAdamInstance(this.Name, this.context, partitionName, siteName);
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x00018479 File Offset: 0x00017479
		public AdamInstanceCollection FindAllAdamInstances()
		{
			this.CheckIfDisposed();
			return ConfigurationSet.FindAdamInstances(this.context, null, null);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x0001848E File Offset: 0x0001748E
		public AdamInstanceCollection FindAllAdamInstances(string partitionName)
		{
			this.CheckIfDisposed();
			if (partitionName == null)
			{
				throw new ArgumentNullException("partitionName");
			}
			return ConfigurationSet.FindAdamInstances(this.context, partitionName, null);
		}

		// Token: 0x06000463 RID: 1123 RVA: 0x000184B1 File Offset: 0x000174B1
		public AdamInstanceCollection FindAllAdamInstances(string partitionName, string siteName)
		{
			this.CheckIfDisposed();
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			return ConfigurationSet.FindAdamInstances(this.context, partitionName, siteName);
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x000184D4 File Offset: 0x000174D4
		public DirectoryEntry GetDirectoryEntry()
		{
			this.CheckIfDisposed();
			return DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.ConfigurationNamingContext);
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x000184E8 File Offset: 0x000174E8
		public ReplicationSecurityLevel GetSecurityLevel()
		{
			this.CheckIfDisposed();
			if (this.cachedSecurityLevel == (ReplicationSecurityLevel)(-1))
			{
				DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.ConfigurationNamingContext);
				this.cachedSecurityLevel = (ReplicationSecurityLevel)((int)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.MsDSReplAuthenticationMode));
			}
			return this.cachedSecurityLevel;
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00018534 File Offset: 0x00017534
		public void SetSecurityLevel(ReplicationSecurityLevel securityLevel)
		{
			this.CheckIfDisposed();
			if (securityLevel < ReplicationSecurityLevel.NegotiatePassThrough || securityLevel > ReplicationSecurityLevel.MutualAuthentication)
			{
				throw new InvalidEnumArgumentException("securityLevel", (int)securityLevel, typeof(ReplicationSecurityLevel));
			}
			try
			{
				DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.ConfigurationNamingContext);
				cachedDirectoryEntry.Properties[PropertyManager.MsDSReplAuthenticationMode].Value = (int)securityLevel;
				cachedDirectoryEntry.CommitChanges();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			this.cachedSecurityLevel = (ReplicationSecurityLevel)(-1);
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x000185BC File Offset: 0x000175BC
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x000185C4 File Offset: 0x000175C4
		public string Name
		{
			get
			{
				this.CheckIfDisposed();
				return this.configSetName;
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000469 RID: 1129 RVA: 0x000185D2 File Offset: 0x000175D2
		public ReadOnlySiteCollection Sites
		{
			get
			{
				this.CheckIfDisposed();
				if (this.cachedSites == null)
				{
					this.cachedSites = new ReadOnlySiteCollection(this.GetSites());
				}
				return this.cachedSites;
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x000185F9 File Offset: 0x000175F9
		public AdamInstanceCollection AdamInstances
		{
			get
			{
				this.CheckIfDisposed();
				if (this.cachedADAMInstances == null)
				{
					this.cachedADAMInstances = this.FindAllAdamInstances();
				}
				return this.cachedADAMInstances;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x0600046B RID: 1131 RVA: 0x0001861B File Offset: 0x0001761B
		public ApplicationPartitionCollection ApplicationPartitions
		{
			get
			{
				this.CheckIfDisposed();
				if (this.cachedApplicationPartitions == null)
				{
					this.cachedApplicationPartitions = new ApplicationPartitionCollection(this.GetApplicationPartitions());
				}
				return this.cachedApplicationPartitions;
			}
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00018644 File Offset: 0x00017644
		public ActiveDirectorySchema Schema
		{
			get
			{
				this.CheckIfDisposed();
				if (this.cachedSchema == null)
				{
					try
					{
						this.cachedSchema = new ActiveDirectorySchema(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.SchemaNamingContext));
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
				}
				return this.cachedSchema;
			}
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x0600046D RID: 1133 RVA: 0x000186A4 File Offset: 0x000176A4
		public AdamInstance SchemaRoleOwner
		{
			get
			{
				this.CheckIfDisposed();
				if (this.cachedSchemaRoleOwner == null)
				{
					this.cachedSchemaRoleOwner = this.GetRoleOwner(AdamRole.SchemaRole);
				}
				return this.cachedSchemaRoleOwner;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x000186C7 File Offset: 0x000176C7
		public AdamInstance NamingRoleOwner
		{
			get
			{
				this.CheckIfDisposed();
				if (this.cachedNamingRoleOwner == null)
				{
					this.cachedNamingRoleOwner = this.GetRoleOwner(AdamRole.NamingRole);
				}
				return this.cachedNamingRoleOwner;
			}
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000186EC File Offset: 0x000176EC
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		private static DirectoryEntry GetSearchRootEntry(Forest forest)
		{
			DirectoryContext directoryContext = forest.GetDirectoryContext();
			bool flag = false;
			bool flag2 = false;
			AuthenticationTypes authenticationTypes = Utils.DefaultAuthType;
			if (directoryContext.ContextType == DirectoryContextType.DirectoryServer)
			{
				flag = true;
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(directoryContext, WellKnownDN.RootDSE);
				string text = (string)PropertyManager.GetPropertyValue(directoryContext, directoryEntry, PropertyManager.IsGlobalCatalogReady);
				flag2 = Utils.Compare(text, "TRUE") == 0;
			}
			DirectoryEntry directoryEntry2;
			if (flag)
			{
				if (DirectoryContext.ServerBindSupported)
				{
					authenticationTypes |= AuthenticationTypes.ServerBind;
				}
				if (flag2)
				{
					directoryEntry2 = new DirectoryEntry("GC://" + directoryContext.GetServerName(), directoryContext.UserName, directoryContext.Password, authenticationTypes);
				}
				else
				{
					directoryEntry2 = new DirectoryEntry("LDAP://" + directoryContext.GetServerName(), directoryContext.UserName, directoryContext.Password, authenticationTypes);
				}
			}
			else
			{
				directoryEntry2 = new DirectoryEntry("GC://" + forest.Name, directoryContext.UserName, directoryContext.Password, authenticationTypes);
			}
			return directoryEntry2;
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x000187CC File Offset: 0x000177CC
		internal static AdamInstance FindAnyAdamInstance(DirectoryContext context)
		{
			if (context.ContextType == DirectoryContextType.ConfigurationSet)
			{
				DirectoryEntry searchRootEntry = ConfigurationSet.GetSearchRootEntry(Forest.GetCurrentForest());
				ArrayList arrayList = new ArrayList();
				try
				{
					string text = (string)searchRootEntry.Properties["distinguishedName"].Value;
					StringBuilder stringBuilder = new StringBuilder(15);
					stringBuilder.Append("(&(");
					stringBuilder.Append(PropertyManager.ObjectCategory);
					stringBuilder.Append("=serviceConnectionPoint)");
					stringBuilder.Append("(");
					stringBuilder.Append(PropertyManager.Keywords);
					stringBuilder.Append("=1.2.840.113556.1.4.1851)(");
					stringBuilder.Append(PropertyManager.Keywords);
					stringBuilder.Append("=");
					stringBuilder.Append(Utils.GetEscapedFilterValue(context.Name));
					stringBuilder.Append("))");
					string text2 = stringBuilder.ToString();
					ADSearcher adsearcher = new ADSearcher(searchRootEntry, text2, new string[] { PropertyManager.ServiceBindingInformation }, SearchScope.Subtree, false, false);
					SearchResultCollection searchResultCollection = adsearcher.FindAll();
					try
					{
						foreach (object obj in searchResultCollection)
						{
							SearchResult searchResult = (SearchResult)obj;
							string text3 = "ldap://";
							foreach (object obj2 in searchResult.Properties[PropertyManager.ServiceBindingInformation])
							{
								string text4 = (string)obj2;
								if (text4.Length > text3.Length && string.Compare(text4.Substring(0, text3.Length), text3, StringComparison.OrdinalIgnoreCase) == 0)
								{
									arrayList.Add(text4.Substring(text3.Length));
								}
							}
						}
					}
					finally
					{
						searchResultCollection.Dispose();
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
				}
				finally
				{
					searchRootEntry.Dispose();
				}
				return ConfigurationSet.FindAliveAdamInstance(null, context, arrayList);
			}
			DirectoryEntryManager directoryEntryManager = new DirectoryEntryManager(context);
			DirectoryEntry cachedDirectoryEntry = directoryEntryManager.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
			if (!Utils.CheckCapability(cachedDirectoryEntry, Capability.ActiveDirectoryApplicationMode))
			{
				directoryEntryManager.RemoveIfExists(directoryEntryManager.ExpandWellKnownDN(WellKnownDN.RootDSE));
				throw new ArgumentException(Res.GetString("TargetShouldBeServerORConfigSet"), "context");
			}
			string text5 = (string)PropertyManager.GetPropertyValue(context, cachedDirectoryEntry, PropertyManager.DnsHostName);
			return new AdamInstance(context, text5, directoryEntryManager);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00018A9C File Offset: 0x00017A9C
		internal static AdamInstance FindOneAdamInstance(DirectoryContext context, string partitionName, string siteName)
		{
			return ConfigurationSet.FindOneAdamInstance(null, context, partitionName, siteName);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00018AA8 File Offset: 0x00017AA8
		internal static AdamInstance FindOneAdamInstance(string configSetName, DirectoryContext context, string partitionName, string siteName)
		{
			if (partitionName != null && partitionName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "partitionName");
			}
			if (siteName != null && siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
			ArrayList replicaList = Utils.GetReplicaList(context, partitionName, siteName, false, true, false);
			if (replicaList.Count < 1)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ADAMInstanceNotFound"), typeof(AdamInstance), null);
			}
			return ConfigurationSet.FindAliveAdamInstance(configSetName, context, replicaList);
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00018B30 File Offset: 0x00017B30
		internal static AdamInstanceCollection FindAdamInstances(DirectoryContext context, string partitionName, string siteName)
		{
			if (partitionName != null && partitionName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "partitionName");
			}
			if (siteName != null && siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
			ArrayList arrayList = new ArrayList();
			foreach (object obj in Utils.GetReplicaList(context, partitionName, siteName, false, true, false))
			{
				string text = (string)obj;
				DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text, DirectoryContextType.DirectoryServer, context);
				arrayList.Add(new AdamInstance(newDirectoryContext, text));
			}
			return new AdamInstanceCollection(arrayList);
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x00018BF0 File Offset: 0x00017BF0
		internal static AdamInstance FindAliveAdamInstance(string configSetName, DirectoryContext context, ArrayList adamInstanceNames)
		{
			bool flag = false;
			AdamInstance adamInstance = null;
			DateTime utcNow = DateTime.UtcNow;
			foreach (object obj in adamInstanceNames)
			{
				string text = (string)obj;
				DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text, DirectoryContextType.DirectoryServer, context);
				DirectoryEntryManager directoryEntryManager = new DirectoryEntryManager(newDirectoryContext);
				DirectoryEntry cachedDirectoryEntry = directoryEntryManager.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
				try
				{
					cachedDirectoryEntry.Bind(true);
					adamInstance = new AdamInstance(newDirectoryContext, text, directoryEntryManager, true);
					flag = true;
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode != -2147016646 && ex.ErrorCode != -2147016690 && ex.ErrorCode != -2147016689 && ex.ErrorCode != -2147023436)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
					}
					if (DateTime.UtcNow.Subtract(utcNow) > ConfigurationSet.locationTimeout)
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ADAMInstanceNotFoundInConfigSet", new object[] { (configSetName != null) ? configSetName : context.Name }), typeof(AdamInstance), null);
					}
				}
				if (flag)
				{
					return adamInstance;
				}
			}
			throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ADAMInstanceNotFoundInConfigSet", new object[] { (configSetName != null) ? configSetName : context.Name }), typeof(AdamInstance), null);
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00018D6C File Offset: 0x00017D6C
		private AdamInstance GetRoleOwner(AdamRole role)
		{
			DirectoryEntry directoryEntry = null;
			string text = null;
			try
			{
				switch (role)
				{
				case AdamRole.SchemaRole:
					directoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.SchemaNamingContext);
					break;
				case AdamRole.NamingRole:
					directoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.PartitionsContainer);
					break;
				}
				directoryEntry.RefreshCache();
				text = Utils.GetAdamDnsHostNameFromNTDSA(this.context, (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.FsmoRoleOwner));
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			finally
			{
				if (directoryEntry != null)
				{
					directoryEntry.Dispose();
				}
			}
			DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text, DirectoryContextType.DirectoryServer, this.context);
			return new AdamInstance(newDirectoryContext, text);
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00018E24 File Offset: 0x00017E24
		private ArrayList GetSites()
		{
			ArrayList arrayList = new ArrayList();
			DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.SitesContainer);
			string text = "(" + PropertyManager.ObjectCategory + "=site)";
			ADSearcher adsearcher = new ADSearcher(cachedDirectoryEntry, text, new string[] { PropertyManager.Cn }, SearchScope.OneLevel);
			SearchResultCollection searchResultCollection = null;
			try
			{
				searchResultCollection = adsearcher.FindAll();
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					arrayList.Add(new ActiveDirectorySite(this.context, (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.Cn), true));
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			finally
			{
				if (searchResultCollection != null)
				{
					searchResultCollection.Dispose();
				}
			}
			return arrayList;
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00018F28 File Offset: 0x00017F28
		private ArrayList GetApplicationPartitions()
		{
			ArrayList arrayList = new ArrayList();
			DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
			DirectoryEntry cachedDirectoryEntry2 = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.PartitionsContainer);
			StringBuilder stringBuilder = new StringBuilder(100);
			stringBuilder.Append("(&(");
			stringBuilder.Append(PropertyManager.ObjectCategory);
			stringBuilder.Append("=crossRef)(");
			stringBuilder.Append(PropertyManager.SystemFlags);
			stringBuilder.Append(":1.2.840.113556.1.4.804:=");
			stringBuilder.Append(1);
			stringBuilder.Append(")(!(");
			stringBuilder.Append(PropertyManager.SystemFlags);
			stringBuilder.Append(":1.2.840.113556.1.4.803:=");
			stringBuilder.Append(2);
			stringBuilder.Append(")))");
			string text = stringBuilder.ToString();
			ADSearcher adsearcher = new ADSearcher(cachedDirectoryEntry2, text, new string[]
			{
				PropertyManager.NCName,
				PropertyManager.MsDSNCReplicaLocations
			}, SearchScope.OneLevel);
			SearchResultCollection searchResultCollection = null;
			try
			{
				searchResultCollection = adsearcher.FindAll();
				string text2 = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.SchemaNamingContext);
				string text3 = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.ConfigurationNamingContext);
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text4 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.NCName);
					if (!text4.Equals(text2) && !text4.Equals(text3))
					{
						ResultPropertyValueCollection resultPropertyValueCollection = searchResult.Properties[PropertyManager.MsDSNCReplicaLocations];
						if (resultPropertyValueCollection.Count > 0)
						{
							string adamDnsHostNameFromNTDSA = Utils.GetAdamDnsHostNameFromNTDSA(this.context, (string)resultPropertyValueCollection[Utils.GetRandomIndex(resultPropertyValueCollection.Count)]);
							DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(adamDnsHostNameFromNTDSA, DirectoryContextType.DirectoryServer, this.context);
							arrayList.Add(new ApplicationPartition(newDirectoryContext, text4, null, ApplicationPartitionType.ADAMApplicationPartition, new DirectoryEntryManager(newDirectoryContext)));
						}
					}
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			finally
			{
				if (searchResultCollection != null)
				{
					searchResultCollection.Dispose();
				}
			}
			return arrayList;
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00019184 File Offset: 0x00018184
		private void CheckIfDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		// Token: 0x040003D3 RID: 979
		private DirectoryContext context;

		// Token: 0x040003D4 RID: 980
		private DirectoryEntryManager directoryEntryMgr;

		// Token: 0x040003D5 RID: 981
		private bool disposed;

		// Token: 0x040003D6 RID: 982
		private string configSetName;

		// Token: 0x040003D7 RID: 983
		private ReadOnlySiteCollection cachedSites;

		// Token: 0x040003D8 RID: 984
		private AdamInstanceCollection cachedADAMInstances;

		// Token: 0x040003D9 RID: 985
		private ApplicationPartitionCollection cachedApplicationPartitions;

		// Token: 0x040003DA RID: 986
		private ActiveDirectorySchema cachedSchema;

		// Token: 0x040003DB RID: 987
		private AdamInstance cachedSchemaRoleOwner;

		// Token: 0x040003DC RID: 988
		private AdamInstance cachedNamingRoleOwner;

		// Token: 0x040003DD RID: 989
		private ReplicationSecurityLevel cachedSecurityLevel = (ReplicationSecurityLevel)(-1);

		// Token: 0x040003DE RID: 990
		private static TimeSpan locationTimeout = new TimeSpan(0, 4, 0);
	}
}
