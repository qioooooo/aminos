using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200007D RID: 125
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ActiveDirectorySiteLinkBridge : IDisposable
	{
		// Token: 0x0600037F RID: 895 RVA: 0x00012357 File Offset: 0x00011357
		public ActiveDirectorySiteLinkBridge(DirectoryContext context, string bridgeName)
			: this(context, bridgeName, ActiveDirectoryTransportType.Rpc)
		{
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00012364 File Offset: 0x00011364
		public ActiveDirectorySiteLinkBridge(DirectoryContext context, string bridgeName, ActiveDirectoryTransportType transport)
		{
			this.links = new ActiveDirectorySiteLinkCollection();
			base..ctor();
			ActiveDirectorySiteLinkBridge.ValidateArgument(context, bridgeName, transport);
			context = new DirectoryContext(context);
			this.context = context;
			this.name = bridgeName;
			this.transport = transport;
			DirectoryEntry directoryEntry;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
				string text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ConfigurationNamingContext);
				string text2;
				if (transport == ActiveDirectoryTransportType.Rpc)
				{
					text2 = "CN=IP,CN=Inter-Site Transports,CN=Sites," + text;
				}
				else
				{
					text2 = "CN=SMTP,CN=Inter-Site Transports,CN=Sites," + text;
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
				string text3 = "cn=" + this.name;
				text3 = Utils.GetEscapedPath(text3);
				this.cachedEntry = directoryEntry.Children.Add(text3, "siteLinkBridge");
			}
			catch (COMException ex2)
			{
				if (ex2.ErrorCode == -2147016656)
				{
					DirectoryEntry directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
					if (Utils.CheckCapability(directoryEntry2, Capability.ActiveDirectoryApplicationMode) && transport == ActiveDirectoryTransportType.Smtp)
					{
						throw new NotSupportedException(Res.GetString("NotSupportTransportSMTP"));
					}
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex2);
			}
			finally
			{
				directoryEntry.Dispose();
			}
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000124CC File Offset: 0x000114CC
		internal ActiveDirectorySiteLinkBridge(DirectoryContext context, string bridgeName, ActiveDirectoryTransportType transport, bool existing)
		{
			this.links = new ActiveDirectorySiteLinkCollection();
			base..ctor();
			this.context = context;
			this.name = bridgeName;
			this.transport = transport;
			this.existing = existing;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000124FC File Offset: 0x000114FC
		public static ActiveDirectorySiteLinkBridge FindByName(DirectoryContext context, string bridgeName)
		{
			return ActiveDirectorySiteLinkBridge.FindByName(context, bridgeName, ActiveDirectoryTransportType.Rpc);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00012508 File Offset: 0x00011508
		public static ActiveDirectorySiteLinkBridge FindByName(DirectoryContext context, string bridgeName, ActiveDirectoryTransportType transport)
		{
			ActiveDirectorySiteLinkBridge.ValidateArgument(context, bridgeName, transport);
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
			ActiveDirectorySiteLinkBridge activeDirectorySiteLinkBridge;
			try
			{
				ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=siteLinkBridge)(objectCategory=SiteLinkBridge)(name=" + Utils.GetEscapedFilterValue(bridgeName) + "))", new string[] { "distinguishedName" }, SearchScope.OneLevel, false, false);
				SearchResult searchResult = adsearcher.FindOne();
				if (searchResult == null)
				{
					Exception ex2 = new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySiteLinkBridge), bridgeName);
					throw ex2;
				}
				DirectoryEntry directoryEntry2 = searchResult.GetDirectoryEntry();
				activeDirectorySiteLinkBridge = new ActiveDirectorySiteLinkBridge(context, bridgeName, transport, true)
				{
					cachedEntry = directoryEntry2
				};
			}
			catch (COMException ex3)
			{
				if (ex3.ErrorCode != -2147016656)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(context, ex3);
				}
				DirectoryEntry directoryEntry3 = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
				if (Utils.CheckCapability(directoryEntry3, Capability.ActiveDirectoryApplicationMode) && transport == ActiveDirectoryTransportType.Smtp)
				{
					throw new NotSupportedException(Res.GetString("NotSupportTransportSMTP"));
				}
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySiteLinkBridge), bridgeName);
			}
			finally
			{
				directoryEntry.Dispose();
			}
			return activeDirectorySiteLinkBridge;
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x06000384 RID: 900 RVA: 0x000126BC File Offset: 0x000116BC
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

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000385 RID: 901 RVA: 0x000126E0 File Offset: 0x000116E0
		public ActiveDirectorySiteLinkCollection SiteLinks
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && !this.linksRetrieved)
				{
					this.links.initialized = false;
					this.links.Clear();
					this.GetLinks();
					this.linksRetrieved = true;
				}
				this.links.initialized = true;
				this.links.de = this.cachedEntry;
				this.links.context = this.context;
				return this.links;
			}
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000386 RID: 902 RVA: 0x0001276E File Offset: 0x0001176E
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

		// Token: 0x06000387 RID: 903 RVA: 0x00012790 File Offset: 0x00011790
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
			if (this.existing)
			{
				this.linksRetrieved = false;
				return;
			}
			this.existing = true;
		}

		// Token: 0x06000388 RID: 904 RVA: 0x000127F8 File Offset: 0x000117F8
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
				this.cachedEntry.Parent.Children.Remove(this.cachedEntry);
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00012874 File Offset: 0x00011874
		public override string ToString()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			return this.name;
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00012898 File Offset: 0x00011898
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

		// Token: 0x0600038B RID: 907 RVA: 0x000128EC File Offset: 0x000118EC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000128FB File Offset: 0x000118FB
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.cachedEntry != null)
			{
				this.cachedEntry.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x0001291C File Offset: 0x0001191C
		private static void ValidateArgument(DirectoryContext context, string bridgeName, ActiveDirectoryTransportType transport)
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
			if (bridgeName == null)
			{
				throw new ArgumentNullException("bridgeName");
			}
			if (bridgeName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "bridgeName");
			}
			if (transport < ActiveDirectoryTransportType.Rpc || transport > ActiveDirectoryTransportType.Smtp)
			{
				throw new InvalidEnumArgumentException("value", (int)transport, typeof(ActiveDirectoryTransportType));
			}
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000129DC File Offset: 0x000119DC
		private void GetLinks()
		{
			ArrayList arrayList = new ArrayList();
			NativeComInterfaces.IAdsPathname adsPathname = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
			adsPathname.EscapedMode = 4;
			string text = "siteLinkList";
			arrayList.Add(text);
			Hashtable valuesWithRangeRetrieval = Utils.GetValuesWithRangeRetrieval(this.cachedEntry, "(objectClass=*)", arrayList, SearchScope.Base);
			ArrayList arrayList2 = (ArrayList)valuesWithRangeRetrieval[text.ToLower(CultureInfo.InvariantCulture)];
			if (arrayList2 == null)
			{
				return;
			}
			for (int i = 0; i < arrayList2.Count; i++)
			{
				string text2 = (string)arrayList2[i];
				adsPathname.Set(text2, 4);
				string text3 = adsPathname.Retrieve(11);
				text3 = text3.Substring(3);
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text2);
				ActiveDirectorySiteLink activeDirectorySiteLink = new ActiveDirectorySiteLink(this.context, text3, this.transport, true, directoryEntry);
				this.links.Add(activeDirectorySiteLink);
			}
		}

		// Token: 0x04000363 RID: 867
		internal DirectoryContext context;

		// Token: 0x04000364 RID: 868
		private string name;

		// Token: 0x04000365 RID: 869
		private ActiveDirectoryTransportType transport;

		// Token: 0x04000366 RID: 870
		private bool disposed;

		// Token: 0x04000367 RID: 871
		private bool existing;

		// Token: 0x04000368 RID: 872
		internal DirectoryEntry cachedEntry;

		// Token: 0x04000369 RID: 873
		private ActiveDirectorySiteLinkCollection links;

		// Token: 0x0400036A RID: 874
		private bool linksRetrieved;
	}
}
