using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200007C RID: 124
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ActiveDirectorySiteLink : IDisposable
	{
		// Token: 0x06000362 RID: 866 RVA: 0x00011475 File Offset: 0x00010475
		public ActiveDirectorySiteLink(DirectoryContext context, string siteLinkName)
			: this(context, siteLinkName, ActiveDirectoryTransportType.Rpc, null)
		{
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00011481 File Offset: 0x00010481
		public ActiveDirectorySiteLink(DirectoryContext context, string siteLinkName, ActiveDirectoryTransportType transport)
			: this(context, siteLinkName, transport, null)
		{
		}

		// Token: 0x06000364 RID: 868 RVA: 0x00011490 File Offset: 0x00010490
		public ActiveDirectorySiteLink(DirectoryContext context, string siteLinkName, ActiveDirectoryTransportType transport, ActiveDirectorySchedule schedule)
		{
			this.systemDefaultInterval = new TimeSpan(0, 15, 0);
			this.sites = new ActiveDirectorySiteCollection();
			base..ctor();
			ActiveDirectorySiteLink.ValidateArgument(context, siteLinkName, transport);
			context = new DirectoryContext(context);
			this.context = context;
			this.name = siteLinkName;
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
				this.cachedEntry = directoryEntry.Children.Add(text3, "siteLink");
				this.cachedEntry.Properties["cost"].Value = 100;
				this.cachedEntry.Properties["replInterval"].Value = 180;
				if (schedule != null)
				{
					this.cachedEntry.Properties["schedule"].Value = schedule.GetUnmanagedSchedule();
				}
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

		// Token: 0x06000365 RID: 869 RVA: 0x00011670 File Offset: 0x00010670
		internal ActiveDirectorySiteLink(DirectoryContext context, string siteLinkName, ActiveDirectoryTransportType transport, bool existing, DirectoryEntry entry)
		{
			this.systemDefaultInterval = new TimeSpan(0, 15, 0);
			this.sites = new ActiveDirectorySiteCollection();
			base..ctor();
			this.context = context;
			this.name = siteLinkName;
			this.transport = transport;
			this.existing = existing;
			this.cachedEntry = entry;
		}

		// Token: 0x06000366 RID: 870 RVA: 0x000116C2 File Offset: 0x000106C2
		public static ActiveDirectorySiteLink FindByName(DirectoryContext context, string siteLinkName)
		{
			return ActiveDirectorySiteLink.FindByName(context, siteLinkName, ActiveDirectoryTransportType.Rpc);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x000116CC File Offset: 0x000106CC
		public static ActiveDirectorySiteLink FindByName(DirectoryContext context, string siteLinkName, ActiveDirectoryTransportType transport)
		{
			ActiveDirectorySiteLink.ValidateArgument(context, siteLinkName, transport);
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
			ActiveDirectorySiteLink activeDirectorySiteLink2;
			try
			{
				ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=siteLink)(objectCategory=SiteLink)(name=" + Utils.GetEscapedFilterValue(siteLinkName) + "))", new string[] { "distinguishedName" }, SearchScope.OneLevel, false, false);
				SearchResult searchResult = adsearcher.FindOne();
				if (searchResult == null)
				{
					Exception ex2 = new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySiteLink), siteLinkName);
					throw ex2;
				}
				DirectoryEntry directoryEntry2 = searchResult.GetDirectoryEntry();
				ActiveDirectorySiteLink activeDirectorySiteLink = new ActiveDirectorySiteLink(context, siteLinkName, transport, true, directoryEntry2);
				activeDirectorySiteLink2 = activeDirectorySiteLink;
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
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySiteLink), siteLinkName);
			}
			finally
			{
				directoryEntry.Dispose();
			}
			return activeDirectorySiteLink2;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x06000368 RID: 872 RVA: 0x00011878 File Offset: 0x00010878
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

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x06000369 RID: 873 RVA: 0x00011899 File Offset: 0x00010899
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

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x0600036A RID: 874 RVA: 0x000118BC File Offset: 0x000108BC
		public ActiveDirectorySiteCollection Sites
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.existing && !this.siteRetrieved)
				{
					this.sites.initialized = false;
					this.sites.Clear();
					this.GetSites();
					this.siteRetrieved = true;
				}
				this.sites.initialized = true;
				this.sites.de = this.cachedEntry;
				this.sites.context = this.context;
				return this.sites;
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0001194C File Offset: 0x0001094C
		// (set) Token: 0x0600036C RID: 876 RVA: 0x000119D0 File Offset: 0x000109D0
		public int Cost
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				try
				{
					if (this.cachedEntry.Properties.Contains("cost"))
					{
						return (int)this.cachedEntry.Properties["cost"][0];
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				return 0;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (value < 0)
				{
					throw new ArgumentException("value");
				}
				try
				{
					this.cachedEntry.Properties["cost"].Value = value;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600036D RID: 877 RVA: 0x00011A48 File Offset: 0x00010A48
		// (set) Token: 0x0600036E RID: 878 RVA: 0x00011ADC File Offset: 0x00010ADC
		public TimeSpan ReplicationInterval
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				try
				{
					if (this.cachedEntry.Properties.Contains("replInterval"))
					{
						int num = (int)this.cachedEntry.Properties["replInterval"][0];
						return new TimeSpan(0, num, 0);
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				return this.systemDefaultInterval;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentException(Res.GetString("NoNegativeTime"), "value");
				}
				double totalMinutes = value.TotalMinutes;
				if (totalMinutes > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("ReplicationIntervalExceedMax"), "value");
				}
				int num = (int)totalMinutes;
				if ((double)num < totalMinutes)
				{
					throw new ArgumentException(Res.GetString("ReplicationIntervalInMinutes"), "value");
				}
				try
				{
					this.cachedEntry.Properties["replInterval"].Value = num;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600036F RID: 879 RVA: 0x00011BAC File Offset: 0x00010BAC
		// (set) Token: 0x06000370 RID: 880 RVA: 0x00011C2C File Offset: 0x00010C2C
		public bool ReciprocalReplicationEnabled
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				int num = 0;
				PropertyValueCollection propertyValueCollection = null;
				try
				{
					propertyValueCollection = this.cachedEntry.Properties["options"];
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (propertyValueCollection.Count != 0)
				{
					num = (int)propertyValueCollection[0];
				}
				return (num & 2) != 0;
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
					PropertyValueCollection propertyValueCollection = this.cachedEntry.Properties["options"];
					if (propertyValueCollection.Count != 0)
					{
						num = (int)propertyValueCollection[0];
					}
					if (value)
					{
						num |= 2;
					}
					else
					{
						num &= -3;
					}
					this.cachedEntry.Properties["options"].Value = num;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x06000371 RID: 881 RVA: 0x00011CD0 File Offset: 0x00010CD0
		// (set) Token: 0x06000372 RID: 882 RVA: 0x00011D50 File Offset: 0x00010D50
		public bool NotificationEnabled
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				int num = 0;
				PropertyValueCollection propertyValueCollection = null;
				try
				{
					propertyValueCollection = this.cachedEntry.Properties["options"];
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (propertyValueCollection.Count != 0)
				{
					num = (int)propertyValueCollection[0];
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
					PropertyValueCollection propertyValueCollection = this.cachedEntry.Properties["options"];
					if (propertyValueCollection.Count != 0)
					{
						num = (int)propertyValueCollection[0];
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

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x06000373 RID: 883 RVA: 0x00011DF4 File Offset: 0x00010DF4
		// (set) Token: 0x06000374 RID: 884 RVA: 0x00011E74 File Offset: 0x00010E74
		public bool DataCompressionEnabled
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				int num = 0;
				PropertyValueCollection propertyValueCollection = null;
				try
				{
					propertyValueCollection = this.cachedEntry.Properties["options"];
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (propertyValueCollection.Count != 0)
				{
					num = (int)propertyValueCollection[0];
				}
				return (num & 4) == 0;
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
					PropertyValueCollection propertyValueCollection = this.cachedEntry.Properties["options"];
					if (propertyValueCollection.Count != 0)
					{
						num = (int)propertyValueCollection[0];
					}
					if (!value)
					{
						num |= 4;
					}
					else
					{
						num &= -5;
					}
					this.cachedEntry.Properties["options"].Value = num;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x06000375 RID: 885 RVA: 0x00011F18 File Offset: 0x00010F18
		// (set) Token: 0x06000376 RID: 886 RVA: 0x00011FA8 File Offset: 0x00010FA8
		public ActiveDirectorySchedule InterSiteReplicationSchedule
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				ActiveDirectorySchedule activeDirectorySchedule = null;
				try
				{
					if (this.cachedEntry.Properties.Contains("schedule"))
					{
						byte[] array = (byte[])this.cachedEntry.Properties["schedule"][0];
						activeDirectorySchedule = new ActiveDirectorySchedule();
						activeDirectorySchedule.SetUnmanagedSchedule(array);
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				return activeDirectorySchedule;
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
						if (this.cachedEntry.Properties.Contains("schedule"))
						{
							this.cachedEntry.Properties["schedule"].Clear();
						}
					}
					else
					{
						this.cachedEntry.Properties["schedule"].Value = value.GetUnmanagedSchedule();
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x00012044 File Offset: 0x00011044
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
				this.siteRetrieved = false;
				return;
			}
			this.existing = true;
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000120AC File Offset: 0x000110AC
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

		// Token: 0x06000379 RID: 889 RVA: 0x00012128 File Offset: 0x00011128
		public override string ToString()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			return this.name;
		}

		// Token: 0x0600037A RID: 890 RVA: 0x0001214C File Offset: 0x0001114C
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

		// Token: 0x0600037B RID: 891 RVA: 0x000121A0 File Offset: 0x000111A0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000121AF File Offset: 0x000111AF
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.cachedEntry != null)
			{
				this.cachedEntry.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000121D0 File Offset: 0x000111D0
		private static void ValidateArgument(DirectoryContext context, string siteLinkName, ActiveDirectoryTransportType transport)
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
			if (siteLinkName == null)
			{
				throw new ArgumentNullException("siteLinkName");
			}
			if (siteLinkName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteLinkName");
			}
			if (transport < ActiveDirectoryTransportType.Rpc || transport > ActiveDirectoryTransportType.Smtp)
			{
				throw new InvalidEnumArgumentException("value", (int)transport, typeof(ActiveDirectoryTransportType));
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00012290 File Offset: 0x00011290
		private void GetSites()
		{
			NativeComInterfaces.IAdsPathname adsPathname = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
			ArrayList arrayList = new ArrayList();
			adsPathname.EscapedMode = 4;
			string text = "siteList";
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
				ActiveDirectorySite activeDirectorySite = new ActiveDirectorySite(this.context, text3, true);
				this.sites.Add(activeDirectorySite);
			}
		}

		// Token: 0x04000357 RID: 855
		private const int systemDefaultCost = 0;

		// Token: 0x04000358 RID: 856
		private const int appDefaultCost = 100;

		// Token: 0x04000359 RID: 857
		private const int appDefaultInterval = 180;

		// Token: 0x0400035A RID: 858
		internal DirectoryContext context;

		// Token: 0x0400035B RID: 859
		private string name;

		// Token: 0x0400035C RID: 860
		private ActiveDirectoryTransportType transport;

		// Token: 0x0400035D RID: 861
		private bool disposed;

		// Token: 0x0400035E RID: 862
		internal bool existing;

		// Token: 0x0400035F RID: 863
		internal DirectoryEntry cachedEntry;

		// Token: 0x04000360 RID: 864
		private TimeSpan systemDefaultInterval;

		// Token: 0x04000361 RID: 865
		private ActiveDirectorySiteCollection sites;

		// Token: 0x04000362 RID: 866
		private bool siteRetrieved;
	}
}
