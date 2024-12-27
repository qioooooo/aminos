using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000077 RID: 119
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ActiveDirectorySchemaProperty : IDisposable
	{
		// Token: 0x060002DF RID: 735 RVA: 0x0000C2C0 File Offset: 0x0000B2C0
		public ActiveDirectorySchemaProperty(DirectoryContext context, string ldapDisplayName)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.Name == null && !context.isRootDomain())
			{
				throw new ArgumentException(Res.GetString("ContextNotAssociatedWithDomain"), "context");
			}
			if (context.Name != null && !context.isRootDomain() && !context.isADAMConfigSet() && !context.isServer())
			{
				throw new ArgumentException(Res.GetString("NotADOrADAM"), "context");
			}
			if (ldapDisplayName == null)
			{
				throw new ArgumentNullException("ldapDisplayName");
			}
			if (ldapDisplayName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "ldapDisplayName");
			}
			this.context = new DirectoryContext(context);
			this.schemaEntry = DirectoryEntryManager.GetDirectoryEntry(context, WellKnownDN.SchemaNamingContext);
			this.schemaEntry.Bind(true);
			this.ldapDisplayName = ldapDisplayName;
			this.commonName = ldapDisplayName;
			this.isBound = false;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x0000C3CC File Offset: 0x0000B3CC
		internal ActiveDirectorySchemaProperty(DirectoryContext context, string ldapDisplayName, DirectoryEntry propertyEntry, DirectoryEntry schemaEntry)
		{
			this.context = context;
			this.ldapDisplayName = ldapDisplayName;
			this.propertyEntry = propertyEntry;
			this.isDefunctOnServer = false;
			this.isDefunct = this.isDefunctOnServer;
			try
			{
				this.abstractPropertyEntry = DirectoryEntryManager.GetDirectoryEntryInternal(context, "LDAP://" + context.GetServerName() + "/schema/" + ldapDisplayName);
				this.iadsProperty = (NativeComInterfaces.IAdsProperty)this.abstractPropertyEntry.NativeObject;
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147463168)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySchemaProperty), ldapDisplayName);
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			catch (InvalidCastException)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySchemaProperty), ldapDisplayName);
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
				throw new ActiveDirectoryOperationException(Res.GetString("ADAMInstanceNotFoundInConfigSet", new object[] { context.Name }));
			}
			this.isBound = true;
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x0000C50C File Offset: 0x0000B50C
		internal ActiveDirectorySchemaProperty(DirectoryContext context, string commonName, SearchResult propertyValuesFromServer, DirectoryEntry schemaEntry)
		{
			this.context = context;
			this.schemaEntry = schemaEntry;
			this.propertyValuesFromServer = propertyValuesFromServer;
			this.propertiesFromSchemaContainerInitialized = true;
			this.propertyEntry = this.GetSchemaPropertyDirectoryEntry();
			this.commonName = commonName;
			this.ldapDisplayName = (string)this.GetValueFromCache(PropertyManager.LdapDisplayName, true);
			this.isDefunctOnServer = true;
			this.isDefunct = this.isDefunctOnServer;
			this.isBound = true;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x0000C5AC File Offset: 0x0000B5AC
		internal ActiveDirectorySchemaProperty(DirectoryContext context, string commonName, string ldapDisplayName, DirectoryEntry propertyEntry, DirectoryEntry schemaEntry)
		{
			this.context = context;
			this.schemaEntry = schemaEntry;
			this.propertyEntry = propertyEntry;
			this.commonName = commonName;
			this.ldapDisplayName = ldapDisplayName;
			this.isDefunctOnServer = true;
			this.isDefunct = this.isDefunctOnServer;
			this.isBound = true;
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x0000C629 File Offset: 0x0000B629
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0000C634 File Offset: 0x0000B634
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					if (this.schemaEntry != null)
					{
						this.schemaEntry.Dispose();
						this.schemaEntry = null;
					}
					if (this.propertyEntry != null)
					{
						this.propertyEntry.Dispose();
						this.propertyEntry = null;
					}
					if (this.abstractPropertyEntry != null)
					{
						this.abstractPropertyEntry.Dispose();
						this.abstractPropertyEntry = null;
					}
					if (this.schema != null)
					{
						this.schema.Dispose();
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x0000C6B4 File Offset: 0x0000B6B4
		public static ActiveDirectorySchemaProperty FindByName(DirectoryContext context, string ldapDisplayName)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.Name == null && !context.isRootDomain())
			{
				throw new ArgumentException(Res.GetString("ContextNotAssociatedWithDomain"), "context");
			}
			if (context.Name != null && !context.isRootDomain() && !context.isADAMConfigSet() && !context.isServer())
			{
				throw new ArgumentException(Res.GetString("NotADOrADAM"), "context");
			}
			if (ldapDisplayName == null)
			{
				throw new ArgumentNullException("ldapDisplayName");
			}
			if (ldapDisplayName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "ldapDisplayName");
			}
			context = new DirectoryContext(context);
			return new ActiveDirectorySchemaProperty(context, ldapDisplayName, null, null);
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x0000C76C File Offset: 0x0000B76C
		public void Save()
		{
			this.CheckIfDisposed();
			if (!this.isBound)
			{
				try
				{
					if (this.schemaEntry == null)
					{
						this.schemaEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.SchemaNamingContext);
					}
					string text = "CN=" + this.commonName;
					text = Utils.GetEscapedPath(text);
					this.propertyEntry = this.schemaEntry.Children.Add(text, "attributeSchema");
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				catch (ActiveDirectoryObjectNotFoundException)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("ADAMInstanceNotFoundInConfigSet", new object[] { this.context.Name }));
				}
				this.SetProperty(PropertyManager.LdapDisplayName, this.ldapDisplayName);
				this.SetProperty(PropertyManager.AttributeID, this.oid);
				if (this.syntax != (ActiveDirectorySyntax)(-1))
				{
					this.SetSyntax(this.syntax);
				}
				this.SetProperty(PropertyManager.Description, this.description);
				this.propertyEntry.Properties[PropertyManager.IsSingleValued].Value = this.isSingleValued;
				this.propertyEntry.Properties[PropertyManager.IsMemberOfPartialAttributeSet].Value = this.isInGlobalCatalog;
				this.propertyEntry.Properties[PropertyManager.IsDefunct].Value = this.isDefunct;
				if (this.rangeLower != null)
				{
					this.propertyEntry.Properties[PropertyManager.RangeLower].Value = this.rangeLower.Value;
				}
				if (this.rangeUpper != null)
				{
					this.propertyEntry.Properties[PropertyManager.RangeUpper].Value = this.rangeUpper.Value;
				}
				if (this.searchFlags != SearchFlags.None)
				{
					this.propertyEntry.Properties[PropertyManager.SearchFlags].Value = (int)this.searchFlags;
				}
				if (this.linkId != null)
				{
					this.propertyEntry.Properties[PropertyManager.LinkID].Value = this.linkId.Value;
				}
				if (this.schemaGuidBinaryForm != null)
				{
					this.SetProperty(PropertyManager.SchemaIDGuid, this.schemaGuidBinaryForm);
				}
			}
			try
			{
				this.propertyEntry.CommitChanges();
				if (this.schema == null)
				{
					ActiveDirectorySchema activeDirectorySchema = ActiveDirectorySchema.GetSchema(this.context);
					bool flag = false;
					DirectoryServer directoryServer = null;
					try
					{
						directoryServer = activeDirectorySchema.SchemaRoleOwner;
						if (Utils.Compare(directoryServer.Name, this.context.GetServerName()) != 0)
						{
							DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(directoryServer.Name, DirectoryContextType.DirectoryServer, this.context);
							this.schema = ActiveDirectorySchema.GetSchema(newDirectoryContext);
						}
						else
						{
							flag = true;
							this.schema = activeDirectorySchema;
						}
					}
					finally
					{
						if (directoryServer != null)
						{
							directoryServer.Dispose();
						}
						if (!flag)
						{
							activeDirectorySchema.Dispose();
						}
					}
				}
				this.schema.RefreshSchema();
			}
			catch (COMException ex2)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex2);
			}
			this.isDefunctOnServer = this.isDefunct;
			this.commonName = null;
			this.oid = null;
			this.syntaxInitialized = false;
			this.descriptionInitialized = false;
			this.isSingleValuedInitialized = false;
			this.isInGlobalCatalogInitialized = false;
			this.rangeLowerInitialized = false;
			this.rangeUpperInitialized = false;
			this.searchFlagsInitialized = false;
			this.linkedPropertyInitialized = false;
			this.linkIdInitialized = false;
			this.schemaGuidBinaryForm = null;
			this.propertiesFromSchemaContainerInitialized = false;
			this.isBound = true;
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0000CAFC File Offset: 0x0000BAFC
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x0000CB04 File Offset: 0x0000BB04
		public DirectoryEntry GetDirectoryEntry()
		{
			this.CheckIfDisposed();
			if (!this.isBound)
			{
				throw new InvalidOperationException(Res.GetString("CannotGetObject"));
			}
			this.GetSchemaPropertyDirectoryEntry();
			return DirectoryEntryManager.GetDirectoryEntryInternal(this.context, this.propertyEntry.Path);
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0000CB41 File Offset: 0x0000BB41
		public string Name
		{
			get
			{
				this.CheckIfDisposed();
				return this.ldapDisplayName;
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x060002EA RID: 746 RVA: 0x0000CB4F File Offset: 0x0000BB4F
		// (set) Token: 0x060002EB RID: 747 RVA: 0x0000CB84 File Offset: 0x0000BB84
		public string CommonName
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && this.commonName == null)
				{
					this.commonName = (string)this.GetValueFromCache(PropertyManager.Cn, true);
				}
				return this.commonName;
			}
			set
			{
				this.CheckIfDisposed();
				if (value != null && value.Length == 0)
				{
					throw new ArgumentException(Res.GetString("EmptyStringParameter"), "value");
				}
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.Cn, value);
				}
				this.commonName = value;
			}
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x060002EC RID: 748 RVA: 0x0000CBD4 File Offset: 0x0000BBD4
		// (set) Token: 0x060002ED RID: 749 RVA: 0x0000CC50 File Offset: 0x0000BC50
		public string Oid
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && this.oid == null)
				{
					if (!this.isDefunctOnServer)
					{
						try
						{
							this.oid = this.iadsProperty.OID;
							goto IL_0056;
						}
						catch (COMException ex)
						{
							throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
						}
					}
					this.oid = (string)this.GetValueFromCache(PropertyManager.AttributeID, true);
				}
				IL_0056:
				return this.oid;
			}
			set
			{
				this.CheckIfDisposed();
				if (value != null && value.Length == 0)
				{
					throw new ArgumentException(Res.GetString("EmptyStringParameter"), "value");
				}
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.AttributeID, value);
				}
				this.oid = value;
			}
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000CCA0 File Offset: 0x0000BCA0
		// (set) Token: 0x060002EF RID: 751 RVA: 0x0000CD1E File Offset: 0x0000BD1E
		public ActiveDirectorySyntax Syntax
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && !this.syntaxInitialized)
				{
					byte[] array = (byte[])this.GetValueFromCache(PropertyManager.OMObjectClass, false);
					OMObjectClass omobjectClass = ((array != null) ? new OMObjectClass(array) : null);
					this.syntax = this.MapSyntax((string)this.GetValueFromCache(PropertyManager.AttributeSyntax, true), (int)this.GetValueFromCache(PropertyManager.OMSyntax, true), omobjectClass);
					this.syntaxInitialized = true;
				}
				return this.syntax;
			}
			set
			{
				this.CheckIfDisposed();
				if (value < ActiveDirectorySyntax.CaseExactString || value > ActiveDirectorySyntax.ReplicaLink)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ActiveDirectorySyntax));
				}
				if (this.isBound)
				{
					this.SetSyntax(value);
				}
				this.syntax = value;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x060002F0 RID: 752 RVA: 0x0000CD5B File Offset: 0x0000BD5B
		// (set) Token: 0x060002F1 RID: 753 RVA: 0x0000CD98 File Offset: 0x0000BD98
		public string Description
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && !this.descriptionInitialized)
				{
					this.description = (string)this.GetValueFromCache(PropertyManager.Description, false);
					this.descriptionInitialized = true;
				}
				return this.description;
			}
			set
			{
				this.CheckIfDisposed();
				if (value != null && value.Length == 0)
				{
					throw new ArgumentException(Res.GetString("EmptyStringParameter"), "value");
				}
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.Description, value);
				}
				this.description = value;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000CDE8 File Offset: 0x0000BDE8
		// (set) Token: 0x060002F3 RID: 755 RVA: 0x0000CE6C File Offset: 0x0000BE6C
		public bool IsSingleValued
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && !this.isSingleValuedInitialized)
				{
					if (!this.isDefunctOnServer)
					{
						try
						{
							this.isSingleValued = !this.iadsProperty.MultiValued;
							goto IL_0059;
						}
						catch (COMException ex)
						{
							throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
						}
					}
					this.isSingleValued = (bool)this.GetValueFromCache(PropertyManager.IsSingleValued, true);
					IL_0059:
					this.isSingleValuedInitialized = true;
				}
				return this.isSingleValued;
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.GetSchemaPropertyDirectoryEntry();
					this.propertyEntry.Properties[PropertyManager.IsSingleValued].Value = value;
				}
				this.isSingleValued = value;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000CEAA File Offset: 0x0000BEAA
		// (set) Token: 0x060002F5 RID: 757 RVA: 0x0000CEB9 File Offset: 0x0000BEB9
		public bool IsIndexed
		{
			get
			{
				this.CheckIfDisposed();
				return this.IsSetInSearchFlags(SearchFlags.IsIndexed);
			}
			set
			{
				this.CheckIfDisposed();
				if (value)
				{
					this.SetBitInSearchFlags(SearchFlags.IsIndexed);
					return;
				}
				this.ResetBitInSearchFlags(SearchFlags.IsIndexed);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x0000CED3 File Offset: 0x0000BED3
		// (set) Token: 0x060002F7 RID: 759 RVA: 0x0000CEE2 File Offset: 0x0000BEE2
		public bool IsIndexedOverContainer
		{
			get
			{
				this.CheckIfDisposed();
				return this.IsSetInSearchFlags(SearchFlags.IsIndexedOverContainer);
			}
			set
			{
				this.CheckIfDisposed();
				if (value)
				{
					this.SetBitInSearchFlags(SearchFlags.IsIndexedOverContainer);
					return;
				}
				this.ResetBitInSearchFlags(SearchFlags.IsIndexedOverContainer);
			}
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x060002F8 RID: 760 RVA: 0x0000CEFC File Offset: 0x0000BEFC
		// (set) Token: 0x060002F9 RID: 761 RVA: 0x0000CF0B File Offset: 0x0000BF0B
		public bool IsInAnr
		{
			get
			{
				this.CheckIfDisposed();
				return this.IsSetInSearchFlags(SearchFlags.IsInAnr);
			}
			set
			{
				this.CheckIfDisposed();
				if (value)
				{
					this.SetBitInSearchFlags(SearchFlags.IsInAnr);
					return;
				}
				this.ResetBitInSearchFlags(SearchFlags.IsInAnr);
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x060002FA RID: 762 RVA: 0x0000CF25 File Offset: 0x0000BF25
		// (set) Token: 0x060002FB RID: 763 RVA: 0x0000CF34 File Offset: 0x0000BF34
		public bool IsOnTombstonedObject
		{
			get
			{
				this.CheckIfDisposed();
				return this.IsSetInSearchFlags(SearchFlags.IsOnTombstonedObject);
			}
			set
			{
				this.CheckIfDisposed();
				if (value)
				{
					this.SetBitInSearchFlags(SearchFlags.IsOnTombstonedObject);
					return;
				}
				this.ResetBitInSearchFlags(SearchFlags.IsOnTombstonedObject);
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000CF4E File Offset: 0x0000BF4E
		// (set) Token: 0x060002FD RID: 765 RVA: 0x0000CF5E File Offset: 0x0000BF5E
		public bool IsTupleIndexed
		{
			get
			{
				this.CheckIfDisposed();
				return this.IsSetInSearchFlags(SearchFlags.IsTupleIndexed);
			}
			set
			{
				this.CheckIfDisposed();
				if (value)
				{
					this.SetBitInSearchFlags(SearchFlags.IsTupleIndexed);
					return;
				}
				this.ResetBitInSearchFlags(SearchFlags.IsTupleIndexed);
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x060002FE RID: 766 RVA: 0x0000CF7C File Offset: 0x0000BF7C
		// (set) Token: 0x060002FF RID: 767 RVA: 0x0000CFCB File Offset: 0x0000BFCB
		public bool IsInGlobalCatalog
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && !this.isInGlobalCatalogInitialized)
				{
					object valueFromCache = this.GetValueFromCache(PropertyManager.IsMemberOfPartialAttributeSet, false);
					this.isInGlobalCatalog = valueFromCache != null && (bool)valueFromCache;
					this.isInGlobalCatalogInitialized = true;
				}
				return this.isInGlobalCatalog;
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.GetSchemaPropertyDirectoryEntry();
					this.propertyEntry.Properties[PropertyManager.IsMemberOfPartialAttributeSet].Value = value;
				}
				this.isInGlobalCatalog = value;
			}
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000300 RID: 768 RVA: 0x0000D00C File Offset: 0x0000C00C
		// (set) Token: 0x06000301 RID: 769 RVA: 0x0000D06C File Offset: 0x0000C06C
		public int? RangeLower
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && !this.rangeLowerInitialized)
				{
					object valueFromCache = this.GetValueFromCache(PropertyManager.RangeLower, false);
					if (valueFromCache == null)
					{
						this.rangeLower = null;
					}
					else
					{
						this.rangeLower = new int?((int)valueFromCache);
					}
					this.rangeLowerInitialized = true;
				}
				return this.rangeLower;
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.GetSchemaPropertyDirectoryEntry();
					if (value == null)
					{
						if (this.propertyEntry.Properties.Contains(PropertyManager.RangeLower))
						{
							this.propertyEntry.Properties[PropertyManager.RangeLower].Clear();
						}
					}
					else
					{
						this.propertyEntry.Properties[PropertyManager.RangeLower].Value = value.Value;
					}
				}
				this.rangeLower = value;
			}
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000302 RID: 770 RVA: 0x0000D0F8 File Offset: 0x0000C0F8
		// (set) Token: 0x06000303 RID: 771 RVA: 0x0000D158 File Offset: 0x0000C158
		public int? RangeUpper
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && !this.rangeUpperInitialized)
				{
					object valueFromCache = this.GetValueFromCache(PropertyManager.RangeUpper, false);
					if (valueFromCache == null)
					{
						this.rangeUpper = null;
					}
					else
					{
						this.rangeUpper = new int?((int)valueFromCache);
					}
					this.rangeUpperInitialized = true;
				}
				return this.rangeUpper;
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.GetSchemaPropertyDirectoryEntry();
					if (value == null)
					{
						if (this.propertyEntry.Properties.Contains(PropertyManager.RangeUpper))
						{
							this.propertyEntry.Properties[PropertyManager.RangeUpper].Clear();
						}
					}
					else
					{
						this.propertyEntry.Properties[PropertyManager.RangeUpper].Value = value.Value;
					}
				}
				this.rangeUpper = value;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000304 RID: 772 RVA: 0x0000D1E3 File Offset: 0x0000C1E3
		// (set) Token: 0x06000305 RID: 773 RVA: 0x0000D1F1 File Offset: 0x0000C1F1
		public bool IsDefunct
		{
			get
			{
				this.CheckIfDisposed();
				return this.isDefunct;
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.IsDefunct, value);
				}
				this.isDefunct = value;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x06000306 RID: 774 RVA: 0x0000D21C File Offset: 0x0000C21C
		public ActiveDirectorySchemaProperty Link
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && !this.linkedPropertyInitialized)
				{
					object valueFromCache = this.GetValueFromCache(PropertyManager.LinkID, false);
					int num = ((valueFromCache != null) ? ((int)valueFromCache) : (-1));
					if (num != -1)
					{
						int num2 = num - 2 * (num % 2) + 1;
						try
						{
							if (this.schemaEntry == null)
							{
								this.schemaEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.SchemaNamingContext);
							}
							string text = string.Concat(new object[]
							{
								"(&(",
								PropertyManager.ObjectCategory,
								"=attributeSchema)(",
								PropertyManager.LinkID,
								"=",
								num2,
								"))"
							});
							ReadOnlyActiveDirectorySchemaPropertyCollection allProperties = ActiveDirectorySchema.GetAllProperties(this.context, this.schemaEntry, text);
							if (allProperties.Count != 1)
							{
								throw new ActiveDirectoryObjectNotFoundException(Res.GetString("LinkedPropertyNotFound", new object[] { num2 }), typeof(ActiveDirectorySchemaProperty), null);
							}
							this.linkedProperty = allProperties[0];
						}
						catch (COMException ex)
						{
							throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
						}
					}
					this.linkedPropertyInitialized = true;
				}
				return this.linkedProperty;
			}
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000D364 File Offset: 0x0000C364
		// (set) Token: 0x06000308 RID: 776 RVA: 0x0000D3C4 File Offset: 0x0000C3C4
		public int? LinkId
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && !this.linkIdInitialized)
				{
					object valueFromCache = this.GetValueFromCache(PropertyManager.LinkID, false);
					if (valueFromCache == null)
					{
						this.linkId = null;
					}
					else
					{
						this.linkId = new int?((int)valueFromCache);
					}
					this.linkIdInitialized = true;
				}
				return this.linkId;
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.GetSchemaPropertyDirectoryEntry();
					if (value == null)
					{
						if (this.propertyEntry.Properties.Contains(PropertyManager.LinkID))
						{
							this.propertyEntry.Properties[PropertyManager.LinkID].Clear();
						}
					}
					else
					{
						this.propertyEntry.Properties[PropertyManager.LinkID].Value = value.Value;
					}
				}
				this.linkId = value;
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000309 RID: 777 RVA: 0x0000D44F File Offset: 0x0000C44F
		// (set) Token: 0x0600030A RID: 778 RVA: 0x0000D490 File Offset: 0x0000C490
		public Guid SchemaGuid
		{
			get
			{
				this.CheckIfDisposed();
				Guid empty = Guid.Empty;
				if (this.isBound && this.schemaGuidBinaryForm == null)
				{
					this.schemaGuidBinaryForm = (byte[])this.GetValueFromCache(PropertyManager.SchemaIDGuid, true);
				}
				return new Guid(this.schemaGuidBinaryForm);
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.SchemaIDGuid, value.Equals(Guid.Empty) ? null : value.ToByteArray());
				}
				this.schemaGuidBinaryForm = (value.Equals(Guid.Empty) ? null : value.ToByteArray());
			}
		}

		// Token: 0x0600030B RID: 779 RVA: 0x0000D4EC File Offset: 0x0000C4EC
		private void CheckIfDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x0000D508 File Offset: 0x0000C508
		private object GetValueFromCache(string propertyName, bool mustExist)
		{
			object obj = null;
			this.InitializePropertiesFromSchemaContainer();
			try
			{
				ResultPropertyValueCollection resultPropertyValueCollection = this.propertyValuesFromServer.Properties[propertyName];
				if (resultPropertyValueCollection == null || resultPropertyValueCollection.Count < 1)
				{
					if (mustExist)
					{
						throw new ActiveDirectoryOperationException(Res.GetString("PropertyNotFound", new object[] { propertyName }));
					}
				}
				else
				{
					obj = resultPropertyValueCollection[0];
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			return obj;
		}

		// Token: 0x0600030D RID: 781 RVA: 0x0000D588 File Offset: 0x0000C588
		private void InitializePropertiesFromSchemaContainer()
		{
			if (!this.propertiesFromSchemaContainerInitialized)
			{
				if (this.schemaEntry == null)
				{
					this.schemaEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.SchemaNamingContext);
				}
				this.propertyValuesFromServer = ActiveDirectorySchemaProperty.GetPropertiesFromSchemaContainer(this.context, this.schemaEntry, this.isDefunctOnServer ? this.commonName : this.ldapDisplayName, this.isDefunctOnServer);
				this.propertiesFromSchemaContainerInitialized = true;
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x0000D5F4 File Offset: 0x0000C5F4
		internal static SearchResult GetPropertiesFromSchemaContainer(DirectoryContext context, DirectoryEntry schemaEntry, string name, bool isDefunctOnServer)
		{
			SearchResult searchResult = null;
			StringBuilder stringBuilder = new StringBuilder(15);
			stringBuilder.Append("(&(");
			stringBuilder.Append(PropertyManager.ObjectCategory);
			stringBuilder.Append("=attributeSchema)");
			stringBuilder.Append("(");
			if (!isDefunctOnServer)
			{
				stringBuilder.Append(PropertyManager.LdapDisplayName);
			}
			else
			{
				stringBuilder.Append(PropertyManager.Cn);
			}
			stringBuilder.Append("=");
			stringBuilder.Append(Utils.GetEscapedFilterValue(name));
			stringBuilder.Append(")");
			if (!isDefunctOnServer)
			{
				stringBuilder.Append("(!(");
			}
			else
			{
				stringBuilder.Append("(");
			}
			stringBuilder.Append(PropertyManager.IsDefunct);
			if (!isDefunctOnServer)
			{
				stringBuilder.Append("=TRUE)))");
			}
			else
			{
				stringBuilder.Append("=TRUE))");
			}
			string[] array;
			if (!isDefunctOnServer)
			{
				array = new string[]
				{
					PropertyManager.DistinguishedName,
					PropertyManager.Cn,
					PropertyManager.AttributeSyntax,
					PropertyManager.OMSyntax,
					PropertyManager.OMObjectClass,
					PropertyManager.Description,
					PropertyManager.SearchFlags,
					PropertyManager.IsMemberOfPartialAttributeSet,
					PropertyManager.LinkID,
					PropertyManager.SchemaIDGuid,
					PropertyManager.RangeLower,
					PropertyManager.RangeUpper
				};
			}
			else
			{
				array = new string[]
				{
					PropertyManager.DistinguishedName,
					PropertyManager.Cn,
					PropertyManager.AttributeSyntax,
					PropertyManager.OMSyntax,
					PropertyManager.OMObjectClass,
					PropertyManager.Description,
					PropertyManager.SearchFlags,
					PropertyManager.IsMemberOfPartialAttributeSet,
					PropertyManager.LinkID,
					PropertyManager.SchemaIDGuid,
					PropertyManager.AttributeID,
					PropertyManager.IsSingleValued,
					PropertyManager.RangeLower,
					PropertyManager.RangeUpper,
					PropertyManager.LdapDisplayName
				};
			}
			ADSearcher adsearcher = new ADSearcher(schemaEntry, stringBuilder.ToString(), array, SearchScope.OneLevel, false, false);
			try
			{
				searchResult = adsearcher.FindOne();
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147016656)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySchemaProperty), name);
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			if (searchResult == null)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySchemaProperty), name);
			}
			return searchResult;
		}

		// Token: 0x0600030F RID: 783 RVA: 0x0000D83C File Offset: 0x0000C83C
		internal DirectoryEntry GetSchemaPropertyDirectoryEntry()
		{
			if (this.propertyEntry == null)
			{
				this.InitializePropertiesFromSchemaContainer();
				this.propertyEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, (string)this.GetValueFromCache(PropertyManager.DistinguishedName, true));
			}
			return this.propertyEntry;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x0000D874 File Offset: 0x0000C874
		private bool IsSetInSearchFlags(SearchFlags searchFlagBit)
		{
			if (this.isBound && !this.searchFlagsInitialized)
			{
				object valueFromCache = this.GetValueFromCache(PropertyManager.SearchFlags, false);
				if (valueFromCache != null)
				{
					this.searchFlags = (SearchFlags)((int)valueFromCache);
				}
				this.searchFlagsInitialized = true;
			}
			return (this.searchFlags & searchFlagBit) != SearchFlags.None;
		}

		// Token: 0x06000311 RID: 785 RVA: 0x0000D8C4 File Offset: 0x0000C8C4
		private void SetBitInSearchFlags(SearchFlags searchFlagBit)
		{
			this.searchFlags |= searchFlagBit;
			if (this.isBound)
			{
				this.GetSchemaPropertyDirectoryEntry();
				this.propertyEntry.Properties[PropertyManager.SearchFlags].Value = (int)this.searchFlags;
			}
		}

		// Token: 0x06000312 RID: 786 RVA: 0x0000D914 File Offset: 0x0000C914
		private void ResetBitInSearchFlags(SearchFlags searchFlagBit)
		{
			this.searchFlags &= ~searchFlagBit;
			if (this.isBound)
			{
				this.GetSchemaPropertyDirectoryEntry();
				this.propertyEntry.Properties[PropertyManager.SearchFlags].Value = (int)this.searchFlags;
			}
		}

		// Token: 0x06000313 RID: 787 RVA: 0x0000D964 File Offset: 0x0000C964
		private void SetProperty(string propertyName, object value)
		{
			this.GetSchemaPropertyDirectoryEntry();
			if (value == null)
			{
				if (this.propertyEntry.Properties.Contains(propertyName))
				{
					this.propertyEntry.Properties[propertyName].Clear();
					return;
				}
			}
			else
			{
				this.propertyEntry.Properties[propertyName].Value = value;
			}
		}

		// Token: 0x06000314 RID: 788 RVA: 0x0000D9BC File Offset: 0x0000C9BC
		private ActiveDirectorySyntax MapSyntax(string syntaxId, int oMID, OMObjectClass oMObjectClass)
		{
			for (int i = 0; i < ActiveDirectorySchemaProperty.SyntaxesCount; i++)
			{
				if (ActiveDirectorySchemaProperty.syntaxes[i].Equals(new Syntax(syntaxId, oMID, oMObjectClass)))
				{
					return (ActiveDirectorySyntax)i;
				}
			}
			throw new ActiveDirectoryOperationException(Res.GetString("UnknownSyntax", new object[] { this.ldapDisplayName }));
		}

		// Token: 0x06000315 RID: 789 RVA: 0x0000DA14 File Offset: 0x0000CA14
		private void SetSyntax(ActiveDirectorySyntax syntax)
		{
			if (syntax < ActiveDirectorySyntax.CaseExactString || syntax > (ActiveDirectorySyntax)(ActiveDirectorySchemaProperty.SyntaxesCount - 1))
			{
				throw new InvalidEnumArgumentException("syntax", (int)syntax, typeof(ActiveDirectorySyntax));
			}
			this.GetSchemaPropertyDirectoryEntry();
			this.propertyEntry.Properties[PropertyManager.AttributeSyntax].Value = ActiveDirectorySchemaProperty.syntaxes[(int)syntax].attributeSyntax;
			this.propertyEntry.Properties[PropertyManager.OMSyntax].Value = ActiveDirectorySchemaProperty.syntaxes[(int)syntax].oMSyntax;
			OMObjectClass oMObjectClass = ActiveDirectorySchemaProperty.syntaxes[(int)syntax].oMObjectClass;
			if (oMObjectClass != null)
			{
				this.propertyEntry.Properties[PropertyManager.OMObjectClass].Value = oMObjectClass.Data;
			}
		}

		// Token: 0x040002F9 RID: 761
		private DirectoryEntry schemaEntry;

		// Token: 0x040002FA RID: 762
		private DirectoryEntry propertyEntry;

		// Token: 0x040002FB RID: 763
		private DirectoryEntry abstractPropertyEntry;

		// Token: 0x040002FC RID: 764
		private NativeComInterfaces.IAdsProperty iadsProperty;

		// Token: 0x040002FD RID: 765
		private DirectoryContext context;

		// Token: 0x040002FE RID: 766
		internal bool isBound;

		// Token: 0x040002FF RID: 767
		private bool disposed;

		// Token: 0x04000300 RID: 768
		private ActiveDirectorySchema schema;

		// Token: 0x04000301 RID: 769
		private bool propertiesFromSchemaContainerInitialized;

		// Token: 0x04000302 RID: 770
		private bool isDefunctOnServer;

		// Token: 0x04000303 RID: 771
		private SearchResult propertyValuesFromServer;

		// Token: 0x04000304 RID: 772
		private string ldapDisplayName;

		// Token: 0x04000305 RID: 773
		private string commonName;

		// Token: 0x04000306 RID: 774
		private string oid;

		// Token: 0x04000307 RID: 775
		private ActiveDirectorySyntax syntax = (ActiveDirectorySyntax)(-1);

		// Token: 0x04000308 RID: 776
		private bool syntaxInitialized;

		// Token: 0x04000309 RID: 777
		private string description;

		// Token: 0x0400030A RID: 778
		private bool descriptionInitialized;

		// Token: 0x0400030B RID: 779
		private bool isSingleValued;

		// Token: 0x0400030C RID: 780
		private bool isSingleValuedInitialized;

		// Token: 0x0400030D RID: 781
		private bool isInGlobalCatalog;

		// Token: 0x0400030E RID: 782
		private bool isInGlobalCatalogInitialized;

		// Token: 0x0400030F RID: 783
		private int? rangeLower = null;

		// Token: 0x04000310 RID: 784
		private bool rangeLowerInitialized;

		// Token: 0x04000311 RID: 785
		private int? rangeUpper = null;

		// Token: 0x04000312 RID: 786
		private bool rangeUpperInitialized;

		// Token: 0x04000313 RID: 787
		private bool isDefunct;

		// Token: 0x04000314 RID: 788
		private SearchFlags searchFlags;

		// Token: 0x04000315 RID: 789
		private bool searchFlagsInitialized;

		// Token: 0x04000316 RID: 790
		private ActiveDirectorySchemaProperty linkedProperty;

		// Token: 0x04000317 RID: 791
		private bool linkedPropertyInitialized;

		// Token: 0x04000318 RID: 792
		private int? linkId = null;

		// Token: 0x04000319 RID: 793
		private bool linkIdInitialized;

		// Token: 0x0400031A RID: 794
		private byte[] schemaGuidBinaryForm;

		// Token: 0x0400031B RID: 795
		private static OMObjectClass dnOMObjectClass = new OMObjectClass(new byte[] { 43, 12, 2, 135, 115, 28, 0, 133, 74 });

		// Token: 0x0400031C RID: 796
		private static OMObjectClass dNWithStringOMObjectClass = new OMObjectClass(new byte[] { 42, 134, 72, 134, 247, 20, 1, 1, 1, 12 });

		// Token: 0x0400031D RID: 797
		private static OMObjectClass dNWithBinaryOMObjectClass = new OMObjectClass(new byte[] { 42, 134, 72, 134, 247, 20, 1, 1, 1, 11 });

		// Token: 0x0400031E RID: 798
		private static OMObjectClass replicaLinkOMObjectClass = new OMObjectClass(new byte[] { 42, 134, 72, 134, 247, 20, 1, 1, 1, 6 });

		// Token: 0x0400031F RID: 799
		private static OMObjectClass presentationAddressOMObjectClass = new OMObjectClass(new byte[] { 43, 12, 2, 135, 115, 28, 0, 133, 92 });

		// Token: 0x04000320 RID: 800
		private static OMObjectClass accessPointDnOMObjectClass = new OMObjectClass(new byte[] { 43, 12, 2, 135, 115, 28, 0, 133, 62 });

		// Token: 0x04000321 RID: 801
		private static OMObjectClass oRNameOMObjectClass = new OMObjectClass(new byte[] { 86, 6, 1, 2, 5, 11, 29 });

		// Token: 0x04000322 RID: 802
		private static int SyntaxesCount = 23;

		// Token: 0x04000323 RID: 803
		private static Syntax[] syntaxes = new Syntax[]
		{
			new Syntax("2.5.5.3", 27, null),
			new Syntax("2.5.5.4", 20, null),
			new Syntax("2.5.5.6", 18, null),
			new Syntax("2.5.5.12", 64, null),
			new Syntax("2.5.5.10", 4, null),
			new Syntax("2.5.5.15", 66, null),
			new Syntax("2.5.5.9", 2, null),
			new Syntax("2.5.5.16", 65, null),
			new Syntax("2.5.5.8", 1, null),
			new Syntax("2.5.5.2", 6, null),
			new Syntax("2.5.5.11", 24, null),
			new Syntax("2.5.5.11", 23, null),
			new Syntax("2.5.5.1", 127, ActiveDirectorySchemaProperty.dnOMObjectClass),
			new Syntax("2.5.5.7", 127, ActiveDirectorySchemaProperty.dNWithBinaryOMObjectClass),
			new Syntax("2.5.5.14", 127, ActiveDirectorySchemaProperty.dNWithStringOMObjectClass),
			new Syntax("2.5.5.9", 10, null),
			new Syntax("2.5.5.5", 22, null),
			new Syntax("2.5.5.5", 19, null),
			new Syntax("2.5.5.17", 4, null),
			new Syntax("2.5.5.14", 127, ActiveDirectorySchemaProperty.accessPointDnOMObjectClass),
			new Syntax("2.5.5.7", 127, ActiveDirectorySchemaProperty.oRNameOMObjectClass),
			new Syntax("2.5.5.13", 127, ActiveDirectorySchemaProperty.presentationAddressOMObjectClass),
			new Syntax("2.5.5.10", 127, ActiveDirectorySchemaProperty.replicaLinkOMObjectClass)
		};
	}
}
