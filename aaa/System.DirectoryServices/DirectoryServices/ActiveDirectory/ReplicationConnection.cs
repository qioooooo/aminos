using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000DA RID: 218
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ReplicationConnection : IDisposable
	{
		// Token: 0x060006AE RID: 1710 RVA: 0x000233AC File Offset: 0x000223AC
		public static ReplicationConnection FindByName(DirectoryContext context, string name)
		{
			ReplicationConnection.ValidateArgument(context, name);
			context = new DirectoryContext(context);
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
			ReplicationConnection replicationConnection;
			try
			{
				string text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ServerName);
				string text2 = "CN=NTDS Settings," + text;
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, text2);
				ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=nTDSConnection)(objectCategory=NTDSConnection)(name=" + Utils.GetEscapedFilterValue(name) + "))", new string[] { "distinguishedName" }, SearchScope.OneLevel, false, false);
				SearchResult searchResult = null;
				try
				{
					searchResult = adsearcher.FindOne();
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2147016656)
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ReplicationConnection), name);
					}
					throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
				}
				if (searchResult == null)
				{
					Exception ex2 = new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ReplicationConnection), name);
					throw ex2;
				}
				DirectoryEntry directoryEntry2 = searchResult.GetDirectoryEntry();
				replicationConnection = new ReplicationConnection(context, directoryEntry2, name);
			}
			finally
			{
				directoryEntry.Dispose();
			}
			return replicationConnection;
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x000234C8 File Offset: 0x000224C8
		internal ReplicationConnection(DirectoryContext context, DirectoryEntry connectionEntry, string name)
		{
			this.context = context;
			this.cachedDirectoryEntry = connectionEntry;
			this.connectionName = name;
			this.existingConnection = true;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x000234EC File Offset: 0x000224EC
		public ReplicationConnection(DirectoryContext context, string name, DirectoryServer sourceServer)
			: this(context, name, sourceServer, null, ActiveDirectoryTransportType.Rpc)
		{
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000234F9 File Offset: 0x000224F9
		public ReplicationConnection(DirectoryContext context, string name, DirectoryServer sourceServer, ActiveDirectorySchedule schedule)
			: this(context, name, sourceServer, schedule, ActiveDirectoryTransportType.Rpc)
		{
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x00023507 File Offset: 0x00022507
		public ReplicationConnection(DirectoryContext context, string name, DirectoryServer sourceServer, ActiveDirectoryTransportType transport)
			: this(context, name, sourceServer, null, transport)
		{
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x00023518 File Offset: 0x00022518
		public ReplicationConnection(DirectoryContext context, string name, DirectoryServer sourceServer, ActiveDirectorySchedule schedule, ActiveDirectoryTransportType transport)
		{
			ReplicationConnection.ValidateArgument(context, name);
			if (sourceServer == null)
			{
				throw new ArgumentNullException("sourceServer");
			}
			if (transport < ActiveDirectoryTransportType.Rpc || transport > ActiveDirectoryTransportType.Smtp)
			{
				throw new InvalidEnumArgumentException("value", (int)transport, typeof(ActiveDirectoryTransportType));
			}
			context = new DirectoryContext(context);
			this.ValidateTargetAndSourceServer(context, sourceServer);
			this.context = context;
			this.connectionName = name;
			this.transport = transport;
			DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
			try
			{
				string text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ServerName);
				string text2 = "CN=NTDS Settings," + text;
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, text2);
				string text3 = "cn=" + this.connectionName;
				text3 = Utils.GetEscapedPath(text3);
				this.cachedDirectoryEntry = directoryEntry.Children.Add(text3, "nTDSConnection");
				DirectoryContext directoryContext = sourceServer.Context;
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(directoryContext, WellKnownDN.RootDSE);
				string text4 = (string)PropertyManager.GetPropertyValue(directoryContext, directoryEntry, PropertyManager.ServerName);
				text4 = "CN=NTDS Settings," + text4;
				this.cachedDirectoryEntry.Properties["fromServer"].Add(text4);
				if (schedule != null)
				{
					this.cachedDirectoryEntry.Properties["schedule"].Value = schedule.GetUnmanagedSchedule();
				}
				string dnfromTransportType = Utils.GetDNFromTransportType(this.TransportType, context);
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, dnfromTransportType);
				try
				{
					directoryEntry.Bind(true);
				}
				catch (COMException ex)
				{
					if (ex.ErrorCode == -2147016656)
					{
						DirectoryEntry directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
						if (Utils.CheckCapability(directoryEntry2, Capability.ActiveDirectoryApplicationMode) && transport == ActiveDirectoryTransportType.Smtp)
						{
							throw new NotSupportedException(Res.GetString("NotSupportTransportSMTP"));
						}
					}
					throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
				}
				this.cachedDirectoryEntry.Properties["transportType"].Add(dnfromTransportType);
				this.cachedDirectoryEntry.Properties["enabledConnection"].Value = false;
				this.cachedDirectoryEntry.Properties["options"].Value = 0;
			}
			catch (COMException ex2)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex2);
			}
			finally
			{
				directoryEntry.Close();
			}
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002377C File Offset: 0x0002277C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002378B File Offset: 0x0002278B
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing && this.cachedDirectoryEntry != null)
				{
					this.cachedDirectoryEntry.Dispose();
				}
				this.disposed = true;
			}
		}

		// Token: 0x060006B6 RID: 1718 RVA: 0x000237B4 File Offset: 0x000227B4
		~ReplicationConnection()
		{
			this.Dispose(false);
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x000237E4 File Offset: 0x000227E4
		public string Name
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.connectionName;
			}
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x060006B8 RID: 1720 RVA: 0x00023808 File Offset: 0x00022808
		public string SourceServer
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.sourceServerName == null)
				{
					string text = (string)PropertyManager.GetPropertyValue(this.context, this.cachedDirectoryEntry, PropertyManager.FromServer);
					DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text);
					if (this.IsADAM)
					{
						int num = (int)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.MsDSPortLDAP);
						string text2 = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry.Parent, PropertyManager.DnsHostName);
						if (num != 389)
						{
							this.sourceServerName = text2 + ":" + num;
						}
					}
					else
					{
						this.sourceServerName = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry.Parent, PropertyManager.DnsHostName);
					}
				}
				return this.sourceServerName;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x060006B9 RID: 1721 RVA: 0x000238E8 File Offset: 0x000228E8
		public string DestinationServer
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (this.destinationServerName == null)
				{
					DirectoryEntry directoryEntry = null;
					DirectoryEntry directoryEntry2 = null;
					try
					{
						directoryEntry = this.cachedDirectoryEntry.Parent;
						directoryEntry2 = directoryEntry.Parent;
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
					string text = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry2, PropertyManager.DnsHostName);
					if (this.IsADAM)
					{
						int num = (int)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.MsDSPortLDAP);
						if (num != 389)
						{
							this.destinationServerName = text + ":" + num;
						}
						else
						{
							this.destinationServerName = text;
						}
					}
					else
					{
						this.destinationServerName = text;
					}
				}
				return this.destinationServerName;
			}
		}

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x060006BA RID: 1722 RVA: 0x000239C0 File Offset: 0x000229C0
		// (set) Token: 0x060006BB RID: 1723 RVA: 0x00023A44 File Offset: 0x00022A44
		public bool Enabled
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				bool flag;
				try
				{
					if (this.cachedDirectoryEntry.Properties.Contains("enabledConnection"))
					{
						flag = (bool)this.cachedDirectoryEntry.Properties["enabledConnection"][0];
					}
					else
					{
						flag = false;
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				return flag;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				try
				{
					this.cachedDirectoryEntry.Properties["enabledConnection"].Value = value;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00023AAC File Offset: 0x00022AAC
		public ActiveDirectoryTransportType TransportType
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (!this.existingConnection)
				{
					return this.transport;
				}
				PropertyValueCollection propertyValueCollection = null;
				try
				{
					propertyValueCollection = this.cachedDirectoryEntry.Properties["transportType"];
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (propertyValueCollection.Count == 0)
				{
					return ActiveDirectoryTransportType.Rpc;
				}
				return Utils.GetTransportTypeFromDN((string)propertyValueCollection[0]);
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x060006BD RID: 1725 RVA: 0x00023B34 File Offset: 0x00022B34
		// (set) Token: 0x060006BE RID: 1726 RVA: 0x00023BC4 File Offset: 0x00022BC4
		public bool GeneratedByKcc
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				PropertyValueCollection propertyValueCollection = null;
				try
				{
					propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (propertyValueCollection.Count == 0)
				{
					this.options = 0;
				}
				else
				{
					this.options = (int)propertyValueCollection[0];
				}
				return (this.options & 1) != 0;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				try
				{
					PropertyValueCollection propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
					if (propertyValueCollection.Count == 0)
					{
						this.options = 0;
					}
					else
					{
						this.options = (int)propertyValueCollection[0];
					}
					if (value)
					{
						this.options |= 1;
					}
					else
					{
						this.options &= -2;
					}
					this.cachedDirectoryEntry.Properties["options"].Value = this.options;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x060006BF RID: 1727 RVA: 0x00023C8C File Offset: 0x00022C8C
		// (set) Token: 0x060006C0 RID: 1728 RVA: 0x00023D1C File Offset: 0x00022D1C
		public bool ReciprocalReplicationEnabled
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				PropertyValueCollection propertyValueCollection = null;
				try
				{
					propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (propertyValueCollection.Count == 0)
				{
					this.options = 0;
				}
				else
				{
					this.options = (int)propertyValueCollection[0];
				}
				return (this.options & 2) != 0;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				try
				{
					PropertyValueCollection propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
					if (propertyValueCollection.Count == 0)
					{
						this.options = 0;
					}
					else
					{
						this.options = (int)propertyValueCollection[0];
					}
					if (value)
					{
						this.options |= 2;
					}
					else
					{
						this.options &= -3;
					}
					this.cachedDirectoryEntry.Properties["options"].Value = this.options;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x060006C1 RID: 1729 RVA: 0x00023DE4 File Offset: 0x00022DE4
		// (set) Token: 0x060006C2 RID: 1730 RVA: 0x00023E8C File Offset: 0x00022E8C
		public NotificationStatus ChangeNotificationStatus
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				PropertyValueCollection propertyValueCollection = null;
				try
				{
					propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (propertyValueCollection.Count == 0)
				{
					this.options = 0;
				}
				else
				{
					this.options = (int)propertyValueCollection[0];
				}
				int num = this.options & 4;
				int num2 = this.options & 8;
				if (num == 4 && num2 == 0)
				{
					return NotificationStatus.NoNotification;
				}
				if (num == 4 && num2 == 8)
				{
					return NotificationStatus.NotificationAlways;
				}
				return NotificationStatus.IntraSiteOnly;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (value < NotificationStatus.NoNotification || value > NotificationStatus.NotificationAlways)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(NotificationStatus));
				}
				try
				{
					PropertyValueCollection propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
					if (propertyValueCollection.Count == 0)
					{
						this.options = 0;
					}
					else
					{
						this.options = (int)propertyValueCollection[0];
					}
					if (value == NotificationStatus.IntraSiteOnly)
					{
						this.options &= -5;
						this.options &= -9;
					}
					else if (value == NotificationStatus.NoNotification)
					{
						this.options |= 4;
						this.options &= -9;
					}
					else
					{
						this.options |= 4;
						this.options |= 8;
					}
					this.cachedDirectoryEntry.Properties["options"].Value = this.options;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x060006C3 RID: 1731 RVA: 0x00023FB0 File Offset: 0x00022FB0
		// (set) Token: 0x060006C4 RID: 1732 RVA: 0x00024040 File Offset: 0x00023040
		public bool DataCompressionEnabled
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				PropertyValueCollection propertyValueCollection = null;
				try
				{
					propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (propertyValueCollection.Count == 0)
				{
					this.options = 0;
				}
				else
				{
					this.options = (int)propertyValueCollection[0];
				}
				return (this.options & 16) == 0;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				try
				{
					PropertyValueCollection propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
					if (propertyValueCollection.Count == 0)
					{
						this.options = 0;
					}
					else
					{
						this.options = (int)propertyValueCollection[0];
					}
					if (!value)
					{
						this.options |= 16;
					}
					else
					{
						this.options &= -17;
					}
					this.cachedDirectoryEntry.Properties["options"].Value = this.options;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x060006C5 RID: 1733 RVA: 0x00024108 File Offset: 0x00023108
		// (set) Token: 0x060006C6 RID: 1734 RVA: 0x00024198 File Offset: 0x00023198
		public bool ReplicationScheduleOwnedByUser
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				PropertyValueCollection propertyValueCollection = null;
				try
				{
					propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (propertyValueCollection.Count == 0)
				{
					this.options = 0;
				}
				else
				{
					this.options = (int)propertyValueCollection[0];
				}
				return (this.options & 32) != 0;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				try
				{
					PropertyValueCollection propertyValueCollection = this.cachedDirectoryEntry.Properties["options"];
					if (propertyValueCollection.Count == 0)
					{
						this.options = 0;
					}
					else
					{
						this.options = (int)propertyValueCollection[0];
					}
					if (value)
					{
						this.options |= 32;
					}
					else
					{
						this.options &= -33;
					}
					this.cachedDirectoryEntry.Properties["options"].Value = this.options;
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x060006C7 RID: 1735 RVA: 0x00024260 File Offset: 0x00023260
		public ReplicationSpan ReplicationSpan
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				string text = (string)PropertyManager.GetPropertyValue(this.context, this.cachedDirectoryEntry, PropertyManager.FromServer);
				string value = Utils.GetDNComponents(text)[3].Value;
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
				string text2 = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.ServerName);
				string value2 = Utils.GetDNComponents(text2)[2].Value;
				if (Utils.Compare(value, value2) == 0)
				{
					return ReplicationSpan.IntraSite;
				}
				return ReplicationSpan.InterSite;
			}
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x060006C8 RID: 1736 RVA: 0x000242F8 File Offset: 0x000232F8
		// (set) Token: 0x060006C9 RID: 1737 RVA: 0x0002438C File Offset: 0x0002338C
		public ActiveDirectorySchedule ReplicationSchedule
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				ActiveDirectorySchedule activeDirectorySchedule = null;
				bool flag = false;
				try
				{
					flag = this.cachedDirectoryEntry.Properties.Contains("schedule");
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				if (flag)
				{
					byte[] array = (byte[])this.cachedDirectoryEntry.Properties["schedule"][0];
					activeDirectorySchedule = new ActiveDirectorySchedule();
					activeDirectorySchedule.SetUnmanagedSchedule(array);
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
						if (this.cachedDirectoryEntry.Properties.Contains("schedule"))
						{
							this.cachedDirectoryEntry.Properties["schedule"].Clear();
						}
					}
					else
					{
						this.cachedDirectoryEntry.Properties["schedule"].Value = value.GetUnmanagedSchedule();
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x060006CA RID: 1738 RVA: 0x00024428 File Offset: 0x00023428
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
					if (propertyValueCollection.Contains("1.2.840.113556.1.4.1851"))
					{
						this.isADAMServer = true;
					}
				}
				return this.isADAMServer;
			}
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00024498 File Offset: 0x00023498
		public void Delete()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (!this.existingConnection)
			{
				throw new InvalidOperationException(Res.GetString("CannotDelete"));
			}
			try
			{
				this.cachedDirectoryEntry.Parent.Children.Remove(this.cachedDirectoryEntry);
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x060006CC RID: 1740 RVA: 0x00024514 File Offset: 0x00023514
		public void Save()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			try
			{
				this.cachedDirectoryEntry.CommitChanges();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			if (!this.existingConnection)
			{
				this.existingConnection = true;
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x00024574 File Offset: 0x00023574
		public override string ToString()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			return this.Name;
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x00024598 File Offset: 0x00023598
		public DirectoryEntry GetDirectoryEntry()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (!this.existingConnection)
			{
				throw new InvalidOperationException(Res.GetString("CannotGetObject"));
			}
			return DirectoryEntryManager.GetDirectoryEntryInternal(this.context, this.cachedDirectoryEntry.Path);
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x000245EC File Offset: 0x000235EC
		private static void ValidateArgument(DirectoryContext context, string name)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.Name == null || !context.isServer())
			{
				throw new ArgumentException(Res.GetString("DirectoryContextNeedHost"));
			}
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "name");
			}
		}

		// Token: 0x060006D0 RID: 1744 RVA: 0x00024654 File Offset: 0x00023654
		private void ValidateTargetAndSourceServer(DirectoryContext context, DirectoryServer sourceServer)
		{
			bool flag = false;
			DirectoryEntry directoryEntry = null;
			DirectoryEntry directoryEntry2 = null;
			directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
			try
			{
				if (Utils.CheckCapability(directoryEntry, Capability.ActiveDirectory))
				{
					flag = true;
				}
				else if (!Utils.CheckCapability(directoryEntry, Capability.ActiveDirectoryApplicationMode))
				{
					throw new ArgumentException(Res.GetString("DirectoryContextNeedHost"), "context");
				}
				if (flag && !(sourceServer is DomainController))
				{
					throw new ArgumentException(Res.GetString("ConnectionSourcServerShouldBeDC"), "sourceServer");
				}
				if (!flag && sourceServer is DomainController)
				{
					throw new ArgumentException(Res.GetString("ConnectionSourcServerShouldBeADAM"), "sourceServer");
				}
				directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(sourceServer.Context, WellKnownDN.RootDSE);
				if (flag)
				{
					string text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.RootDomainNamingContext);
					string text2 = (string)PropertyManager.GetPropertyValue(sourceServer.Context, directoryEntry2, PropertyManager.RootDomainNamingContext);
					if (Utils.Compare(text, text2) != 0)
					{
						throw new ArgumentException(Res.GetString("ConnectionSourcServerSameForest"), "sourceServer");
					}
				}
				else
				{
					string text3 = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ConfigurationNamingContext);
					string text4 = (string)PropertyManager.GetPropertyValue(sourceServer.Context, directoryEntry2, PropertyManager.ConfigurationNamingContext);
					if (Utils.Compare(text3, text4) != 0)
					{
						throw new ArgumentException(Res.GetString("ConnectionSourcServerSameConfigSet"), "sourceServer");
					}
				}
			}
			catch (COMException ex)
			{
				ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			finally
			{
				if (directoryEntry != null)
				{
					directoryEntry.Close();
				}
				if (directoryEntry2 != null)
				{
					directoryEntry2.Close();
				}
			}
		}

		// Token: 0x04000562 RID: 1378
		private const string ADAMGuid = "1.2.840.113556.1.4.1851";

		// Token: 0x04000563 RID: 1379
		internal DirectoryContext context;

		// Token: 0x04000564 RID: 1380
		internal DirectoryEntry cachedDirectoryEntry;

		// Token: 0x04000565 RID: 1381
		internal bool existingConnection;

		// Token: 0x04000566 RID: 1382
		private bool disposed;

		// Token: 0x04000567 RID: 1383
		private bool checkADAM;

		// Token: 0x04000568 RID: 1384
		private bool isADAMServer;

		// Token: 0x04000569 RID: 1385
		private int options;

		// Token: 0x0400056A RID: 1386
		private string connectionName;

		// Token: 0x0400056B RID: 1387
		private string sourceServerName;

		// Token: 0x0400056C RID: 1388
		private string destinationServerName;

		// Token: 0x0400056D RID: 1389
		private ActiveDirectoryTransportType transport;
	}
}
