using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000073 RID: 115
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ActiveDirectorySchema : ActiveDirectoryPartition
	{
		// Token: 0x0600028C RID: 652 RVA: 0x0000901D File Offset: 0x0000801D
		internal ActiveDirectorySchema(DirectoryContext context, string distinguishedName)
			: base(context, distinguishedName)
		{
			this.directoryEntryMgr = new DirectoryEntryManager(context);
			this.schemaEntry = DirectoryEntryManager.GetDirectoryEntry(context, distinguishedName);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00009040 File Offset: 0x00008040
		internal ActiveDirectorySchema(DirectoryContext context, string distinguishedName, DirectoryEntryManager directoryEntryMgr)
			: base(context, distinguishedName)
		{
			this.directoryEntryMgr = directoryEntryMgr;
			this.schemaEntry = DirectoryEntryManager.GetDirectoryEntry(context, distinguishedName);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00009060 File Offset: 0x00008060
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					if (disposing)
					{
						if (this.schemaEntry != null)
						{
							this.schemaEntry.Dispose();
							this.schemaEntry = null;
						}
						if (this.abstractSchemaEntry != null)
						{
							this.abstractSchemaEntry.Dispose();
							this.abstractSchemaEntry = null;
						}
					}
					this.disposed = true;
				}
				finally
				{
					base.Dispose();
				}
			}
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000090CC File Offset: 0x000080CC
		public static ActiveDirectorySchema GetSchema(DirectoryContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Forest && context.ContextType != DirectoryContextType.ConfigurationSet && context.ContextType != DirectoryContextType.DirectoryServer)
			{
				throw new ArgumentException(Res.GetString("NotADOrADAM"), "context");
			}
			if (context.Name == null && !context.isRootDomain())
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ContextNotAssociatedWithDomain"), typeof(ActiveDirectorySchema), null);
			}
			if (context.Name == null || context.isRootDomain() || context.isADAMConfigSet() || context.isServer())
			{
				context = new DirectoryContext(context);
				DirectoryEntryManager directoryEntryManager = new DirectoryEntryManager(context);
				string text = null;
				try
				{
					DirectoryEntry cachedDirectoryEntry = directoryEntryManager.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
					if (context.isServer() && !Utils.CheckCapability(cachedDirectoryEntry, Capability.ActiveDirectoryOrADAM))
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ServerNotFound", new object[] { context.Name }), typeof(ActiveDirectorySchema), null);
					}
					text = (string)PropertyManager.GetPropertyValue(context, cachedDirectoryEntry, PropertyManager.SchemaNamingContext);
				}
				catch (COMException ex)
				{
					int errorCode = ex.ErrorCode;
					if (errorCode != -2147016646)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
					}
					if (context.ContextType == DirectoryContextType.Forest)
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ForestNotFound"), typeof(ActiveDirectorySchema), context.Name);
					}
					if (context.ContextType == DirectoryContextType.ConfigurationSet)
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ConfigSetNotFound"), typeof(ActiveDirectorySchema), context.Name);
					}
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ServerNotFound", new object[] { context.Name }), typeof(ActiveDirectorySchema), null);
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					if (context.ContextType == DirectoryContextType.ConfigurationSet)
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ConfigSetNotFound"), typeof(ActiveDirectorySchema), context.Name);
					}
					throw;
				}
				return new ActiveDirectorySchema(context, text, directoryEntryManager);
			}
			if (context.ContextType == DirectoryContextType.Forest)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ForestNotFound"), typeof(ActiveDirectorySchema), context.Name);
			}
			if (context.ContextType == DirectoryContextType.ConfigurationSet)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ConfigSetNotFound"), typeof(ActiveDirectorySchema), context.Name);
			}
			throw new ActiveDirectoryObjectNotFoundException(Res.GetString("ServerNotFound", new object[] { context.Name }), typeof(ActiveDirectorySchema), null);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x00009350 File Offset: 0x00008350
		public void RefreshSchema()
		{
			base.CheckIfDisposed();
			DirectoryEntry directoryEntry = null;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
				directoryEntry.Properties[PropertyManager.SchemaUpdateNow].Value = 1;
				directoryEntry.CommitChanges();
				if (this.abstractSchemaEntry == null)
				{
					this.abstractSchemaEntry = this.directoryEntryMgr.GetCachedDirectoryEntry("Schema");
				}
				this.abstractSchemaEntry.RefreshCache();
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
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000093F4 File Offset: 0x000083F4
		public ActiveDirectorySchemaClass FindClass(string ldapDisplayName)
		{
			base.CheckIfDisposed();
			return ActiveDirectorySchemaClass.FindByName(this.context, ldapDisplayName);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x00009408 File Offset: 0x00008408
		public ActiveDirectorySchemaClass FindDefunctClass(string commonName)
		{
			base.CheckIfDisposed();
			if (commonName == null)
			{
				throw new ArgumentNullException("commonName");
			}
			if (commonName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "commonName");
			}
			Hashtable propertiesFromSchemaContainer = ActiveDirectorySchemaClass.GetPropertiesFromSchemaContainer(this.context, this.schemaEntry, commonName, true);
			return new ActiveDirectorySchemaClass(this.context, commonName, propertiesFromSchemaContainer, this.schemaEntry);
		}

		// Token: 0x06000293 RID: 659 RVA: 0x00009470 File Offset: 0x00008470
		public ReadOnlyActiveDirectorySchemaClassCollection FindAllClasses()
		{
			base.CheckIfDisposed();
			string text = string.Concat(new string[]
			{
				"(&(",
				PropertyManager.ObjectCategory,
				"=classSchema)(!(",
				PropertyManager.IsDefunct,
				"=TRUE)))"
			});
			return ActiveDirectorySchema.GetAllClasses(this.context, this.schemaEntry, text);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000094CC File Offset: 0x000084CC
		public ReadOnlyActiveDirectorySchemaClassCollection FindAllClasses(SchemaClassType type)
		{
			base.CheckIfDisposed();
			if (type < SchemaClassType.Type88 || type > SchemaClassType.Auxiliary)
			{
				throw new InvalidEnumArgumentException("type", (int)type, typeof(SchemaClassType));
			}
			string text = string.Concat(new object[]
			{
				"(&(",
				PropertyManager.ObjectCategory,
				"=classSchema)(",
				PropertyManager.ObjectClassCategory,
				"=",
				(int)type,
				")(!(",
				PropertyManager.IsDefunct,
				"=TRUE)))"
			});
			return ActiveDirectorySchema.GetAllClasses(this.context, this.schemaEntry, text);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00009568 File Offset: 0x00008568
		public ReadOnlyActiveDirectorySchemaClassCollection FindAllDefunctClasses()
		{
			base.CheckIfDisposed();
			string text = string.Concat(new string[]
			{
				"(&(",
				PropertyManager.ObjectCategory,
				"=classSchema)(",
				PropertyManager.IsDefunct,
				"=TRUE))"
			});
			return ActiveDirectorySchema.GetAllClasses(this.context, this.schemaEntry, text);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x000095C3 File Offset: 0x000085C3
		public ActiveDirectorySchemaProperty FindProperty(string ldapDisplayName)
		{
			base.CheckIfDisposed();
			return ActiveDirectorySchemaProperty.FindByName(this.context, ldapDisplayName);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x000095D8 File Offset: 0x000085D8
		public ActiveDirectorySchemaProperty FindDefunctProperty(string commonName)
		{
			base.CheckIfDisposed();
			if (commonName == null)
			{
				throw new ArgumentNullException("commonName");
			}
			if (commonName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "commonName");
			}
			SearchResult propertiesFromSchemaContainer = ActiveDirectorySchemaProperty.GetPropertiesFromSchemaContainer(this.context, this.schemaEntry, commonName, true);
			return new ActiveDirectorySchemaProperty(this.context, commonName, propertiesFromSchemaContainer, this.schemaEntry);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00009640 File Offset: 0x00008640
		public ReadOnlyActiveDirectorySchemaPropertyCollection FindAllProperties()
		{
			base.CheckIfDisposed();
			string text = string.Concat(new string[]
			{
				"(&(",
				PropertyManager.ObjectCategory,
				"=attributeSchema)(!(",
				PropertyManager.IsDefunct,
				"=TRUE)))"
			});
			return ActiveDirectorySchema.GetAllProperties(this.context, this.schemaEntry, text);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000969C File Offset: 0x0000869C
		public ReadOnlyActiveDirectorySchemaPropertyCollection FindAllProperties(PropertyTypes type)
		{
			base.CheckIfDisposed();
			if ((type & ~(PropertyTypes.Indexed | PropertyTypes.InGlobalCatalog)) != (PropertyTypes)0)
			{
				throw new ArgumentException(Res.GetString("InvalidFlags"), "type");
			}
			StringBuilder stringBuilder = new StringBuilder(25);
			stringBuilder.Append("(&(");
			stringBuilder.Append(PropertyManager.ObjectCategory);
			stringBuilder.Append("=attributeSchema)");
			stringBuilder.Append("(!(");
			stringBuilder.Append(PropertyManager.IsDefunct);
			stringBuilder.Append("=TRUE))");
			if ((type & PropertyTypes.Indexed) != (PropertyTypes)0)
			{
				stringBuilder.Append("(");
				stringBuilder.Append(PropertyManager.SearchFlags);
				stringBuilder.Append(":1.2.840.113556.1.4.804:=");
				stringBuilder.Append(1);
				stringBuilder.Append(")");
			}
			if ((type & PropertyTypes.InGlobalCatalog) != (PropertyTypes)0)
			{
				stringBuilder.Append("(");
				stringBuilder.Append(PropertyManager.IsMemberOfPartialAttributeSet);
				stringBuilder.Append("=TRUE)");
			}
			stringBuilder.Append(")");
			return ActiveDirectorySchema.GetAllProperties(this.context, this.schemaEntry, stringBuilder.ToString());
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000097A4 File Offset: 0x000087A4
		public ReadOnlyActiveDirectorySchemaPropertyCollection FindAllDefunctProperties()
		{
			base.CheckIfDisposed();
			string text = string.Concat(new string[]
			{
				"(&(",
				PropertyManager.ObjectCategory,
				"=attributeSchema)(",
				PropertyManager.IsDefunct,
				"=TRUE))"
			});
			return ActiveDirectorySchema.GetAllProperties(this.context, this.schemaEntry, text);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000097FF File Offset: 0x000087FF
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override DirectoryEntry GetDirectoryEntry()
		{
			base.CheckIfDisposed();
			return DirectoryEntryManager.GetDirectoryEntry(this.context, base.Name);
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00009818 File Offset: 0x00008818
		public static ActiveDirectorySchema GetCurrentSchema()
		{
			return ActiveDirectorySchema.GetSchema(new DirectoryContext(DirectoryContextType.Forest));
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00009825 File Offset: 0x00008825
		public DirectoryServer SchemaRoleOwner
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedSchemaRoleOwner == null)
				{
					this.cachedSchemaRoleOwner = this.GetSchemaRoleOwner();
				}
				return this.cachedSchemaRoleOwner;
			}
		}

		// Token: 0x0600029E RID: 670 RVA: 0x00009848 File Offset: 0x00008848
		internal static ReadOnlyActiveDirectorySchemaPropertyCollection GetAllProperties(DirectoryContext context, DirectoryEntry schemaEntry, string filter)
		{
			ArrayList arrayList = new ArrayList();
			ADSearcher adsearcher = new ADSearcher(schemaEntry, filter, new string[]
			{
				PropertyManager.LdapDisplayName,
				PropertyManager.Cn,
				PropertyManager.IsDefunct
			}, SearchScope.OneLevel);
			SearchResultCollection searchResultCollection = null;
			try
			{
				searchResultCollection = adsearcher.FindAll();
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.LdapDisplayName);
					DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
					directoryEntry.AuthenticationType = Utils.DefaultAuthType;
					directoryEntry.Username = context.UserName;
					directoryEntry.Password = context.Password;
					bool flag = false;
					if (searchResult.Properties[PropertyManager.IsDefunct] != null && searchResult.Properties[PropertyManager.IsDefunct].Count > 0)
					{
						flag = (bool)searchResult.Properties[PropertyManager.IsDefunct][0];
					}
					if (flag)
					{
						string text2 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.Cn);
						arrayList.Add(new ActiveDirectorySchemaProperty(context, text2, text, directoryEntry, schemaEntry));
					}
					else
					{
						arrayList.Add(new ActiveDirectorySchemaProperty(context, text, directoryEntry, schemaEntry));
					}
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			finally
			{
				if (searchResultCollection != null)
				{
					searchResultCollection.Dispose();
				}
			}
			return new ReadOnlyActiveDirectorySchemaPropertyCollection(arrayList);
		}

		// Token: 0x0600029F RID: 671 RVA: 0x00009A04 File Offset: 0x00008A04
		internal static ReadOnlyActiveDirectorySchemaClassCollection GetAllClasses(DirectoryContext context, DirectoryEntry schemaEntry, string filter)
		{
			ArrayList arrayList = new ArrayList();
			ADSearcher adsearcher = new ADSearcher(schemaEntry, filter, new string[]
			{
				PropertyManager.LdapDisplayName,
				PropertyManager.Cn,
				PropertyManager.IsDefunct
			}, SearchScope.OneLevel);
			SearchResultCollection searchResultCollection = null;
			try
			{
				searchResultCollection = adsearcher.FindAll();
				foreach (object obj in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj;
					string text = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.LdapDisplayName);
					DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
					directoryEntry.AuthenticationType = Utils.DefaultAuthType;
					directoryEntry.Username = context.UserName;
					directoryEntry.Password = context.Password;
					bool flag = false;
					if (searchResult.Properties[PropertyManager.IsDefunct] != null && searchResult.Properties[PropertyManager.IsDefunct].Count > 0)
					{
						flag = (bool)searchResult.Properties[PropertyManager.IsDefunct][0];
					}
					if (flag)
					{
						string text2 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.Cn);
						arrayList.Add(new ActiveDirectorySchemaClass(context, text2, text, directoryEntry, schemaEntry));
					}
					else
					{
						arrayList.Add(new ActiveDirectorySchemaClass(context, text, directoryEntry, schemaEntry));
					}
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			finally
			{
				if (searchResultCollection != null)
				{
					searchResultCollection.Dispose();
				}
			}
			return new ReadOnlyActiveDirectorySchemaClassCollection(arrayList);
		}

		// Token: 0x060002A0 RID: 672 RVA: 0x00009BC0 File Offset: 0x00008BC0
		private DirectoryServer GetSchemaRoleOwner()
		{
			DirectoryServer directoryServer;
			try
			{
				this.schemaEntry.RefreshCache();
				if (this.context.isADAMConfigSet())
				{
					string adamDnsHostNameFromNTDSA = Utils.GetAdamDnsHostNameFromNTDSA(this.context, (string)PropertyManager.GetPropertyValue(this.context, this.schemaEntry, PropertyManager.FsmoRoleOwner));
					DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(adamDnsHostNameFromNTDSA, DirectoryContextType.DirectoryServer, this.context);
					directoryServer = new AdamInstance(newDirectoryContext, adamDnsHostNameFromNTDSA);
				}
				else
				{
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
					DirectoryServer directoryServer2;
					if (Utils.CheckCapability(cachedDirectoryEntry, Capability.ActiveDirectory))
					{
						string dnsHostNameFromNTDSA = Utils.GetDnsHostNameFromNTDSA(this.context, (string)PropertyManager.GetPropertyValue(this.context, this.schemaEntry, PropertyManager.FsmoRoleOwner));
						DirectoryContext newDirectoryContext2 = Utils.GetNewDirectoryContext(dnsHostNameFromNTDSA, DirectoryContextType.DirectoryServer, this.context);
						directoryServer2 = new DomainController(newDirectoryContext2, dnsHostNameFromNTDSA);
					}
					else
					{
						string adamDnsHostNameFromNTDSA2 = Utils.GetAdamDnsHostNameFromNTDSA(this.context, (string)PropertyManager.GetPropertyValue(this.context, this.schemaEntry, PropertyManager.FsmoRoleOwner));
						DirectoryContext newDirectoryContext3 = Utils.GetNewDirectoryContext(adamDnsHostNameFromNTDSA2, DirectoryContextType.DirectoryServer, this.context);
						directoryServer2 = new AdamInstance(newDirectoryContext3, adamDnsHostNameFromNTDSA2);
					}
					directoryServer = directoryServer2;
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			return directoryServer;
		}

		// Token: 0x040002CD RID: 717
		private bool disposed;

		// Token: 0x040002CE RID: 718
		private DirectoryEntry schemaEntry;

		// Token: 0x040002CF RID: 719
		private DirectoryEntry abstractSchemaEntry;

		// Token: 0x040002D0 RID: 720
		private DirectoryServer cachedSchemaRoleOwner;
	}
}
