using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200007F RID: 127
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ActiveDirectorySubnet : IDisposable
	{
		// Token: 0x0600039F RID: 927 RVA: 0x00013130 File Offset: 0x00012130
		public static ActiveDirectorySubnet FindByName(DirectoryContext context, string subnetName)
		{
			ActiveDirectorySubnet.ValidateArgument(context, subnetName);
			context = new DirectoryContext(context);
			DirectoryEntry directoryEntry;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
				string text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ConfigurationNamingContext);
				string text2 = "CN=Subnets,CN=Sites," + text;
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
			ActiveDirectorySubnet activeDirectorySubnet2;
			try
			{
				ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=subnet)(objectCategory=subnet)(name=" + Utils.GetEscapedFilterValue(subnetName) + "))", new string[] { "distinguishedName" }, SearchScope.OneLevel, false, false);
				SearchResult searchResult = adsearcher.FindOne();
				if (searchResult == null)
				{
					Exception ex2 = new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySubnet), subnetName);
					throw ex2;
				}
				string text3 = null;
				DirectoryEntry directoryEntry2 = searchResult.GetDirectoryEntry();
				if (directoryEntry2.Properties.Contains("siteObject"))
				{
					NativeComInterfaces.IAdsPathname adsPathname = (NativeComInterfaces.IAdsPathname)new NativeComInterfaces.Pathname();
					adsPathname.EscapedMode = 4;
					string text4 = (string)directoryEntry2.Properties["siteObject"][0];
					adsPathname.Set(text4, 4);
					string text5 = adsPathname.Retrieve(11);
					text3 = text5.Substring(3);
				}
				ActiveDirectorySubnet activeDirectorySubnet;
				if (text3 == null)
				{
					activeDirectorySubnet = new ActiveDirectorySubnet(context, subnetName, null, true);
				}
				else
				{
					activeDirectorySubnet = new ActiveDirectorySubnet(context, subnetName, text3, true);
				}
				activeDirectorySubnet.cachedEntry = directoryEntry2;
				activeDirectorySubnet2 = activeDirectorySubnet;
			}
			catch (COMException ex3)
			{
				if (ex3.ErrorCode == -2147016656)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySubnet), subnetName);
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex3);
			}
			finally
			{
				if (directoryEntry != null)
				{
					directoryEntry.Dispose();
				}
			}
			return activeDirectorySubnet2;
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00013350 File Offset: 0x00012350
		public ActiveDirectorySubnet(DirectoryContext context, string subnetName)
		{
			ActiveDirectorySubnet.ValidateArgument(context, subnetName);
			context = new DirectoryContext(context);
			this.context = context;
			this.name = subnetName;
			DirectoryEntry directoryEntry = null;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.RootDSE);
				string text = (string)PropertyManager.GetPropertyValue(context, directoryEntry, PropertyManager.ConfigurationNamingContext);
				string text2 = "CN=Subnets,CN=Sites," + text;
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(context, text2);
				string text3 = "cn=" + this.name;
				text3 = Utils.GetEscapedPath(text3);
				this.cachedEntry = directoryEntry.Children.Add(text3, "subnet");
			}
			catch (COMException ex)
			{
				ExceptionHelper.GetExceptionFromCOMException(context, ex);
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
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00013448 File Offset: 0x00012448
		public ActiveDirectorySubnet(DirectoryContext context, string subnetName, string siteName)
			: this(context, subnetName)
		{
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			if (siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
			try
			{
				this.site = ActiveDirectorySite.FindByName(this.context, siteName);
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
				throw new ArgumentException(Res.GetString("SiteNotExist", new object[] { siteName }), "siteName");
			}
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x000134D0 File Offset: 0x000124D0
		internal ActiveDirectorySubnet(DirectoryContext context, string subnetName, string siteName, bool existing)
		{
			this.context = context;
			this.name = subnetName;
			if (siteName != null)
			{
				try
				{
					this.site = ActiveDirectorySite.FindByName(context, siteName);
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					throw new ArgumentException(Res.GetString("SiteNotExist", new object[] { siteName }), "siteName");
				}
			}
			this.existing = true;
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0001353C File Offset: 0x0001253C
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

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x0001355D File Offset: 0x0001255D
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x00013580 File Offset: 0x00012580
		public ActiveDirectorySite Site
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.site;
			}
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				if (value != null && !value.existing)
				{
					throw new InvalidOperationException(Res.GetString("SiteNotCommitted", new object[] { value }));
				}
				this.site = value;
			}
		}

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x060003A6 RID: 934 RVA: 0x000135D4 File Offset: 0x000125D4
		// (set) Token: 0x060003A7 RID: 935 RVA: 0x00013658 File Offset: 0x00012658
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

		// Token: 0x060003A8 RID: 936 RVA: 0x000136F0 File Offset: 0x000126F0
		public void Save()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			try
			{
				if (this.existing)
				{
					if (this.site == null)
					{
						if (this.cachedEntry.Properties.Contains("siteObject"))
						{
							this.cachedEntry.Properties["siteObject"].Clear();
						}
					}
					else
					{
						this.cachedEntry.Properties["siteObject"].Value = this.site.cachedEntry.Properties["distinguishedName"][0];
					}
					this.cachedEntry.CommitChanges();
				}
				else
				{
					if (this.Site != null)
					{
						this.cachedEntry.Properties["siteObject"].Add(this.site.cachedEntry.Properties["distinguishedName"][0]);
					}
					this.cachedEntry.CommitChanges();
					this.existing = true;
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00013818 File Offset: 0x00012818
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

		// Token: 0x060003AA RID: 938 RVA: 0x00013894 File Offset: 0x00012894
		public override string ToString()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			return this.Name;
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000138B8 File Offset: 0x000128B8
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

		// Token: 0x060003AC RID: 940 RVA: 0x0001390C File Offset: 0x0001290C
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x0001391B File Offset: 0x0001291B
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.cachedEntry != null)
			{
				this.cachedEntry.Dispose();
			}
			this.disposed = true;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x0001393C File Offset: 0x0001293C
		private static void ValidateArgument(DirectoryContext context, string subnetName)
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
			if (subnetName == null)
			{
				throw new ArgumentNullException("subnetName");
			}
			if (subnetName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "subnetName");
			}
		}

		// Token: 0x0400036E RID: 878
		private ActiveDirectorySite site;

		// Token: 0x0400036F RID: 879
		private string name;

		// Token: 0x04000370 RID: 880
		internal DirectoryContext context;

		// Token: 0x04000371 RID: 881
		private bool disposed;

		// Token: 0x04000372 RID: 882
		internal bool existing;

		// Token: 0x04000373 RID: 883
		internal DirectoryEntry cachedEntry;
	}
}
