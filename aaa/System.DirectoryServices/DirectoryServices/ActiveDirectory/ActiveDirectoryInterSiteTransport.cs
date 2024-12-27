using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000E7 RID: 231
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ActiveDirectoryInterSiteTransport : IDisposable
	{
		// Token: 0x0600071C RID: 1820 RVA: 0x00025428 File Offset: 0x00024428
		internal ActiveDirectoryInterSiteTransport(DirectoryContext context, ActiveDirectoryTransportType transport, DirectoryEntry entry)
		{
			this.context = context;
			this.transport = transport;
			this.cachedEntry = entry;
		}

		// Token: 0x0600071D RID: 1821 RVA: 0x0002545C File Offset: 0x0002445C
		public static ActiveDirectoryInterSiteTransport FindByTransportType(DirectoryContext context, ActiveDirectoryTransportType transport)
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
			if (transport < ActiveDirectoryTransportType.Rpc || transport > ActiveDirectoryTransportType.Smtp)
			{
				throw new InvalidEnumArgumentException("value", (int)transport, typeof(ActiveDirectoryTransportType));
			}
			context = new DirectoryContext(context);
			DirectoryEntry directoryEntry;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
				string text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ConfigurationNamingContext);
				string text2 = "CN=Inter-Site Transports,CN=Sites," + text;
				if (transport == ActiveDirectoryTransportType.Rpc)
				{
					text2 = "CN=IP," + text2;
				}
				else
				{
					text2 = "CN=SMTP," + text2;
				}
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, text2);
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
				throw new ActiveDirectoryOperationException(Res.GetString("ADAMInstanceNotFoundInConfigSet", new object[] { context.Name }));
			}
			try
			{
				directoryEntry.RefreshCache(new string[] { "options" });
			}
			catch (COMException ex2)
			{
				if (ex2.ErrorCode != -2147016656)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(context, ex2);
				}
				DirectoryEntry directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
				if (Utils.CheckCapability(directoryEntry2, Capability.ActiveDirectoryApplicationMode) && transport == ActiveDirectoryTransportType.Smtp)
				{
					throw new NotSupportedException(Res.GetString("NotSupportTransportSMTP"));
				}
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("TransportNotFound", new object[] { transport.ToString() }), typeof(ActiveDirectoryInterSiteTransport), transport.ToString());
			}
			return new ActiveDirectoryInterSiteTransport(context, transport, directoryEntry);
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x0600071E RID: 1822 RVA: 0x0002563C File Offset: 0x0002463C
		public ActiveDirectoryTransportType TransportType
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.transport;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x0600071F RID: 1823 RVA: 0x00025660 File Offset: 0x00024660
		// (set) Token: 0x06000720 RID: 1824 RVA: 0x000256E8 File Offset: 0x000246E8
		public bool IgnoreReplicationSchedule
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				int num = 0;
				try
				{
					if (this.cachedEntry.Properties.Contains("options"))
					{
						num = (int)this.cachedEntry.Properties["options"][0];
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				return (num & 1) != 0;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				int num = 0;
				try
				{
					if (this.cachedEntry.Properties.Contains("options"))
					{
						num = (int)this.cachedEntry.Properties["options"][0];
					}
					if (value)
					{
						num |= 1;
					}
					else
					{
						num &= -2;
					}
					this.cachedEntry.Properties["options"].Value = num;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000721 RID: 1825 RVA: 0x00025798 File Offset: 0x00024798
		// (set) Token: 0x06000722 RID: 1826 RVA: 0x00025820 File Offset: 0x00024820
		public bool BridgeAllSiteLinks
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				int num = 0;
				try
				{
					if (this.cachedEntry.Properties.Contains("options"))
					{
						num = (int)this.cachedEntry.Properties["options"][0];
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				return (num & 2) == 0;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				int num = 0;
				try
				{
					if (this.cachedEntry.Properties.Contains("options"))
					{
						num = (int)this.cachedEntry.Properties["options"][0];
					}
					if (value)
					{
						num &= -3;
					}
					else
					{
						num |= 2;
					}
					this.cachedEntry.Properties["options"].Value = num;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x000258D0 File Offset: 0x000248D0
		public ReadOnlySiteLinkCollection SiteLinks
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (!this.linkRetrieved)
				{
					this.siteLinkCollection.Clear();
					ADSearcher adsearcher = new ADSearcher(this.cachedEntry, "(&(objectClass=siteLink)(objectCategory=SiteLink))", new string[] { "cn" }, SearchScope.OneLevel);
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
							DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
							string text = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.Cn);
							ActiveDirectorySiteLink activeDirectorySiteLink = new ActiveDirectorySiteLink(this.context, text, this.transport, true, directoryEntry);
							this.siteLinkCollection.Add(activeDirectorySiteLink);
						}
					}
					finally
					{
						searchResultCollection.Dispose();
					}
					this.linkRetrieved = true;
				}
				return this.siteLinkCollection;
			}
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000724 RID: 1828 RVA: 0x00025A00 File Offset: 0x00024A00
		public ReadOnlySiteLinkBridgeCollection SiteLinkBridges
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (!this.bridgeRetrieved)
				{
					this.bridgeCollection.Clear();
					ADSearcher adsearcher = new ADSearcher(this.cachedEntry, "(&(objectClass=siteLinkBridge)(objectCategory=SiteLinkBridge))", new string[] { "cn" }, SearchScope.OneLevel);
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
							DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
							string text = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.Cn);
							ActiveDirectorySiteLinkBridge activeDirectorySiteLinkBridge = new ActiveDirectorySiteLinkBridge(this.context, text, this.transport, true);
							activeDirectorySiteLinkBridge.cachedEntry = directoryEntry;
							this.bridgeCollection.Add(activeDirectorySiteLinkBridge);
						}
					}
					finally
					{
						searchResultCollection.Dispose();
					}
					this.bridgeRetrieved = true;
				}
				return this.bridgeCollection;
			}
		}

		// Token: 0x06000725 RID: 1829 RVA: 0x00025B38 File Offset: 0x00024B38
		public void Save()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			try
			{
				this.cachedEntry.CommitChanges();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x06000726 RID: 1830 RVA: 0x00025B8C File Offset: 0x00024B8C
		public DirectoryEntry GetDirectoryEntry()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			return DirectoryEntryManager.GetDirectoryEntryInternal(this.context, this.cachedEntry.Path);
		}

		// Token: 0x06000727 RID: 1831 RVA: 0x00025BBD File Offset: 0x00024BBD
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x00025BCC File Offset: 0x00024BCC
		public override string ToString()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			return this.transport.ToString();
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x00025BF7 File Offset: 0x00024BF7
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.cachedEntry != null)
			{
				this.cachedEntry.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x040005B3 RID: 1459
		private DirectoryContext context;

		// Token: 0x040005B4 RID: 1460
		private DirectoryEntry cachedEntry;

		// Token: 0x040005B5 RID: 1461
		private ActiveDirectoryTransportType transport;

		// Token: 0x040005B6 RID: 1462
		private bool disposed;

		// Token: 0x040005B7 RID: 1463
		private bool linkRetrieved;

		// Token: 0x040005B8 RID: 1464
		private bool bridgeRetrieved;

		// Token: 0x040005B9 RID: 1465
		private ReadOnlySiteLinkCollection siteLinkCollection = new ReadOnlySiteLinkCollection();

		// Token: 0x040005BA RID: 1466
		private ReadOnlySiteLinkBridgeCollection bridgeCollection = new ReadOnlySiteLinkBridgeCollection();
	}
}
