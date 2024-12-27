using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000074 RID: 116
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class ActiveDirectorySchemaClass : IDisposable
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x00009CF0 File Offset: 0x00008CF0
		public ActiveDirectorySchemaClass(DirectoryContext context, string ldapDisplayName)
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

		// Token: 0x060002A2 RID: 674 RVA: 0x00009DD8 File Offset: 0x00008DD8
		internal ActiveDirectorySchemaClass(DirectoryContext context, string ldapDisplayName, DirectoryEntry classEntry, DirectoryEntry schemaEntry)
		{
			this.context = context;
			this.ldapDisplayName = ldapDisplayName;
			this.classEntry = classEntry;
			this.schemaEntry = schemaEntry;
			this.isDefunctOnServer = false;
			this.isDefunct = this.isDefunctOnServer;
			try
			{
				this.abstractClassEntry = DirectoryEntryManager.GetDirectoryEntryInternal(context, "LDAP://" + context.GetServerName() + "/schema/" + ldapDisplayName);
				this.iadsClass = (NativeComInterfaces.IAdsClass)this.abstractClassEntry.NativeObject;
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147463168)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySchemaClass), ldapDisplayName);
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			catch (InvalidCastException)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySchemaClass), ldapDisplayName);
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
				throw new ActiveDirectoryOperationException(Res.GetString("ADAMInstanceNotFoundInConfigSet", new object[] { context.Name }));
			}
			this.isBound = true;
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00009EFC File Offset: 0x00008EFC
		internal ActiveDirectorySchemaClass(DirectoryContext context, string commonName, Hashtable propertyValuesFromServer, DirectoryEntry schemaEntry)
		{
			this.context = context;
			this.schemaEntry = schemaEntry;
			this.propertyValuesFromServer = propertyValuesFromServer;
			this.propertiesFromSchemaContainerInitialized = true;
			this.classEntry = this.GetSchemaClassDirectoryEntry();
			this.commonName = commonName;
			this.ldapDisplayName = (string)this.GetValueFromCache(PropertyManager.LdapDisplayName, true);
			this.isDefunctOnServer = true;
			this.isDefunct = this.isDefunctOnServer;
			this.isBound = true;
		}

		// Token: 0x060002A4 RID: 676 RVA: 0x00009F78 File Offset: 0x00008F78
		internal ActiveDirectorySchemaClass(DirectoryContext context, string commonName, string ldapDisplayName, DirectoryEntry classEntry, DirectoryEntry schemaEntry)
		{
			this.context = context;
			this.schemaEntry = schemaEntry;
			this.classEntry = classEntry;
			this.commonName = commonName;
			this.ldapDisplayName = ldapDisplayName;
			this.isDefunctOnServer = true;
			this.isDefunct = this.isDefunctOnServer;
			this.isBound = true;
		}

		// Token: 0x060002A5 RID: 677 RVA: 0x00009FD1 File Offset: 0x00008FD1
		public void Dispose()
		{
			this.Dispose(true);
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x00009FDC File Offset: 0x00008FDC
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
					if (this.classEntry != null)
					{
						this.classEntry.Dispose();
						this.classEntry = null;
					}
					if (this.abstractClassEntry != null)
					{
						this.abstractClassEntry.Dispose();
						this.abstractClassEntry = null;
					}
					if (this.schema != null)
					{
						this.schema.Dispose();
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x0000A05C File Offset: 0x0000905C
		public static ActiveDirectorySchemaClass FindByName(DirectoryContext context, string ldapDisplayName)
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
			if (ldapDisplayName == null)
			{
				throw new ArgumentNullException("ldapDisplayName");
			}
			if (ldapDisplayName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "ldapDisplayName");
			}
			context = new DirectoryContext(context);
			return new ActiveDirectorySchemaClass(context, ldapDisplayName, null, null);
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000A114 File Offset: 0x00009114
		public ReadOnlyActiveDirectorySchemaPropertyCollection GetAllProperties()
		{
			this.CheckIfDisposed();
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(this.MandatoryProperties);
			arrayList.AddRange(this.OptionalProperties);
			return new ReadOnlyActiveDirectorySchemaPropertyCollection(arrayList);
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000A14C File Offset: 0x0000914C
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
					this.classEntry = this.schemaEntry.Children.Add(text, "classSchema");
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
				this.SetProperty(PropertyManager.GovernsID, this.oid);
				this.SetProperty(PropertyManager.Description, this.description);
				if (this.possibleSuperiors != null)
				{
					this.classEntry.Properties[PropertyManager.PossibleSuperiors].AddRange(this.possibleSuperiors.GetMultiValuedProperty());
				}
				if (this.mandatoryProperties != null)
				{
					this.classEntry.Properties[PropertyManager.MustContain].AddRange(this.mandatoryProperties.GetMultiValuedProperty());
				}
				if (this.optionalProperties != null)
				{
					this.classEntry.Properties[PropertyManager.MayContain].AddRange(this.optionalProperties.GetMultiValuedProperty());
				}
				if (this.subClassOf != null)
				{
					this.SetProperty(PropertyManager.SubClassOf, this.subClassOf.Name);
				}
				else
				{
					this.SetProperty(PropertyManager.SubClassOf, "top");
				}
				this.SetProperty(PropertyManager.ObjectClassCategory, this.type);
				if (this.schemaGuidBinaryForm != null)
				{
					this.SetProperty(PropertyManager.SchemaIDGuid, this.schemaGuidBinaryForm);
				}
				if (this.defaultSDSddlForm != null)
				{
					this.SetProperty(PropertyManager.DefaultSecurityDescriptor, this.defaultSDSddlForm);
				}
			}
			try
			{
				this.classEntry.CommitChanges();
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
			this.description = null;
			this.descriptionInitialized = false;
			this.possibleSuperiors = null;
			this.auxiliaryClasses = null;
			this.possibleInferiors = null;
			this.mandatoryProperties = null;
			this.optionalProperties = null;
			this.subClassOf = null;
			this.typeInitialized = false;
			this.schemaGuidBinaryForm = null;
			this.defaultSDSddlForm = null;
			this.defaultSDSddlFormInitialized = false;
			this.propertiesFromSchemaContainerInitialized = false;
			this.isBound = true;
		}

		// Token: 0x060002AA RID: 682 RVA: 0x0000A47C File Offset: 0x0000947C
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000A484 File Offset: 0x00009484
		public DirectoryEntry GetDirectoryEntry()
		{
			this.CheckIfDisposed();
			if (!this.isBound)
			{
				throw new InvalidOperationException(Res.GetString("CannotGetObject"));
			}
			this.GetSchemaClassDirectoryEntry();
			return DirectoryEntryManager.GetDirectoryEntryInternal(this.context, this.classEntry.Path);
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x060002AC RID: 684 RVA: 0x0000A4C1 File Offset: 0x000094C1
		public string Name
		{
			get
			{
				this.CheckIfDisposed();
				return this.ldapDisplayName;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000A4CF File Offset: 0x000094CF
		// (set) Token: 0x060002AE RID: 686 RVA: 0x0000A504 File Offset: 0x00009504
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
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.Cn, value);
				}
				this.commonName = value;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000A528 File Offset: 0x00009528
		// (set) Token: 0x060002B0 RID: 688 RVA: 0x0000A5A4 File Offset: 0x000095A4
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
							this.oid = this.iadsClass.OID;
							goto IL_0056;
						}
						catch (COMException ex)
						{
							throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
						}
					}
					this.oid = (string)this.GetValueFromCache(PropertyManager.GovernsID, true);
				}
				IL_0056:
				return this.oid;
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.GovernsID, value);
				}
				this.oid = value;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x060002B1 RID: 689 RVA: 0x0000A5C7 File Offset: 0x000095C7
		// (set) Token: 0x060002B2 RID: 690 RVA: 0x0000A603 File Offset: 0x00009603
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
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.Description, value);
				}
				this.description = value;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000A626 File Offset: 0x00009626
		// (set) Token: 0x060002B4 RID: 692 RVA: 0x0000A634 File Offset: 0x00009634
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

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x060002B5 RID: 693 RVA: 0x0000A65C File Offset: 0x0000965C
		public ActiveDirectorySchemaClassCollection PossibleSuperiors
		{
			get
			{
				this.CheckIfDisposed();
				if (this.possibleSuperiors == null)
				{
					if (this.isBound)
					{
						if (!this.isDefunctOnServer)
						{
							ArrayList arrayList = new ArrayList();
							bool flag = false;
							object obj = null;
							try
							{
								obj = this.iadsClass.PossibleSuperiors;
							}
							catch (COMException ex)
							{
								if (ex.ErrorCode != -2147463155)
								{
									throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
								}
								flag = true;
							}
							if (!flag)
							{
								if (obj is ICollection)
								{
									arrayList.AddRange((ICollection)obj);
								}
								else
								{
									arrayList.Add((string)obj);
								}
								this.possibleSuperiors = new ActiveDirectorySchemaClassCollection(this.context, this, true, PropertyManager.PossibleSuperiors, arrayList, true);
							}
							else
							{
								this.possibleSuperiors = new ActiveDirectorySchemaClassCollection(this.context, this, true, PropertyManager.PossibleSuperiors, new ArrayList());
							}
						}
						else
						{
							ArrayList arrayList2 = new ArrayList();
							arrayList2.AddRange(this.GetValuesFromCache(PropertyManager.PossibleSuperiors));
							arrayList2.AddRange(this.GetValuesFromCache(PropertyManager.SystemPossibleSuperiors));
							this.possibleSuperiors = new ActiveDirectorySchemaClassCollection(this.context, this, true, PropertyManager.PossibleSuperiors, this.GetClasses(arrayList2));
						}
					}
					else
					{
						this.possibleSuperiors = new ActiveDirectorySchemaClassCollection(this.context, this, false, PropertyManager.PossibleSuperiors, new ArrayList());
					}
				}
				return this.possibleSuperiors;
			}
		}

		// Token: 0x170000AE RID: 174
		// (get) Token: 0x060002B6 RID: 694 RVA: 0x0000A7B0 File Offset: 0x000097B0
		public ReadOnlyActiveDirectorySchemaClassCollection PossibleInferiors
		{
			get
			{
				this.CheckIfDisposed();
				if (this.possibleInferiors == null)
				{
					if (this.isBound)
					{
						this.possibleInferiors = new ReadOnlyActiveDirectorySchemaClassCollection(this.GetClasses(this.GetValuesFromCache(PropertyManager.PossibleInferiors)));
					}
					else
					{
						this.possibleInferiors = new ReadOnlyActiveDirectorySchemaClassCollection(new ArrayList());
					}
				}
				return this.possibleInferiors;
			}
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x060002B7 RID: 695 RVA: 0x0000A808 File Offset: 0x00009808
		public ActiveDirectorySchemaPropertyCollection MandatoryProperties
		{
			get
			{
				this.CheckIfDisposed();
				if (this.mandatoryProperties == null)
				{
					if (this.isBound)
					{
						if (!this.isDefunctOnServer)
						{
							ArrayList arrayList = new ArrayList();
							bool flag = false;
							object obj = null;
							try
							{
								obj = this.iadsClass.MandatoryProperties;
							}
							catch (COMException ex)
							{
								if (ex.ErrorCode != -2147463155)
								{
									throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
								}
								flag = true;
							}
							if (!flag)
							{
								if (obj is ICollection)
								{
									arrayList.AddRange((ICollection)obj);
								}
								else
								{
									arrayList.Add((string)obj);
								}
								this.mandatoryProperties = new ActiveDirectorySchemaPropertyCollection(this.context, this, true, PropertyManager.MustContain, arrayList, true);
							}
							else
							{
								this.mandatoryProperties = new ActiveDirectorySchemaPropertyCollection(this.context, this, true, PropertyManager.MustContain, new ArrayList());
							}
						}
						else
						{
							string[] array = new string[]
							{
								PropertyManager.SystemMustContain,
								PropertyManager.MustContain
							};
							this.mandatoryProperties = new ActiveDirectorySchemaPropertyCollection(this.context, this, true, PropertyManager.MustContain, this.GetProperties(this.GetPropertyValuesRecursively(array)));
						}
					}
					else
					{
						this.mandatoryProperties = new ActiveDirectorySchemaPropertyCollection(this.context, this, false, PropertyManager.MustContain, new ArrayList());
					}
				}
				return this.mandatoryProperties;
			}
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x060002B8 RID: 696 RVA: 0x0000A94C File Offset: 0x0000994C
		public ActiveDirectorySchemaPropertyCollection OptionalProperties
		{
			get
			{
				this.CheckIfDisposed();
				if (this.optionalProperties == null)
				{
					if (this.isBound)
					{
						if (!this.isDefunctOnServer)
						{
							ArrayList arrayList = new ArrayList();
							bool flag = false;
							object obj = null;
							try
							{
								obj = this.iadsClass.OptionalProperties;
							}
							catch (COMException ex)
							{
								if (ex.ErrorCode != -2147463155)
								{
									throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
								}
								flag = true;
							}
							if (!flag)
							{
								if (obj is ICollection)
								{
									arrayList.AddRange((ICollection)obj);
								}
								else
								{
									arrayList.Add((string)obj);
								}
								this.optionalProperties = new ActiveDirectorySchemaPropertyCollection(this.context, this, true, PropertyManager.MayContain, arrayList, true);
							}
							else
							{
								this.optionalProperties = new ActiveDirectorySchemaPropertyCollection(this.context, this, true, PropertyManager.MayContain, new ArrayList());
							}
						}
						else
						{
							string[] array = new string[]
							{
								PropertyManager.SystemMayContain,
								PropertyManager.MayContain
							};
							ArrayList arrayList2 = new ArrayList();
							foreach (object obj2 in this.GetPropertyValuesRecursively(array))
							{
								string text = (string)obj2;
								if (!this.MandatoryProperties.Contains(text))
								{
									arrayList2.Add(text);
								}
							}
							this.optionalProperties = new ActiveDirectorySchemaPropertyCollection(this.context, this, true, PropertyManager.MayContain, this.GetProperties(arrayList2));
						}
					}
					else
					{
						this.optionalProperties = new ActiveDirectorySchemaPropertyCollection(this.context, this, false, PropertyManager.MayContain, new ArrayList());
					}
				}
				return this.optionalProperties;
			}
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x060002B9 RID: 697 RVA: 0x0000AAFC File Offset: 0x00009AFC
		public ActiveDirectorySchemaClassCollection AuxiliaryClasses
		{
			get
			{
				this.CheckIfDisposed();
				if (this.auxiliaryClasses == null)
				{
					if (this.isBound)
					{
						if (!this.isDefunctOnServer)
						{
							ArrayList arrayList = new ArrayList();
							bool flag = false;
							object obj = null;
							try
							{
								obj = this.iadsClass.AuxDerivedFrom;
							}
							catch (COMException ex)
							{
								if (ex.ErrorCode != -2147463155)
								{
									throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
								}
								flag = true;
							}
							if (!flag)
							{
								if (obj is ICollection)
								{
									arrayList.AddRange((ICollection)obj);
								}
								else
								{
									arrayList.Add((string)obj);
								}
								this.auxiliaryClasses = new ActiveDirectorySchemaClassCollection(this.context, this, true, PropertyManager.AuxiliaryClass, arrayList, true);
							}
							else
							{
								this.auxiliaryClasses = new ActiveDirectorySchemaClassCollection(this.context, this, true, PropertyManager.AuxiliaryClass, new ArrayList());
							}
						}
						else
						{
							string[] array = new string[]
							{
								PropertyManager.AuxiliaryClass,
								PropertyManager.SystemAuxiliaryClass
							};
							this.auxiliaryClasses = new ActiveDirectorySchemaClassCollection(this.context, this, true, PropertyManager.AuxiliaryClass, this.GetClasses(this.GetPropertyValuesRecursively(array)));
						}
					}
					else
					{
						this.auxiliaryClasses = new ActiveDirectorySchemaClassCollection(this.context, this, false, PropertyManager.AuxiliaryClass, new ArrayList());
					}
				}
				return this.auxiliaryClasses;
			}
		}

		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x060002BA RID: 698 RVA: 0x0000AC40 File Offset: 0x00009C40
		// (set) Token: 0x060002BB RID: 699 RVA: 0x0000AC92 File Offset: 0x00009C92
		public ActiveDirectorySchemaClass SubClassOf
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && this.subClassOf == null)
				{
					this.subClassOf = new ActiveDirectorySchemaClass(this.context, (string)this.GetValueFromCache(PropertyManager.SubClassOf, true), null, this.schemaEntry);
				}
				return this.subClassOf;
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.SubClassOf, value);
				}
				this.subClassOf = value;
			}
		}

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060002BC RID: 700 RVA: 0x0000ACB5 File Offset: 0x00009CB5
		// (set) Token: 0x060002BD RID: 701 RVA: 0x0000ACF4 File Offset: 0x00009CF4
		public SchemaClassType Type
		{
			get
			{
				this.CheckIfDisposed();
				if (this.isBound && !this.typeInitialized)
				{
					this.type = (SchemaClassType)((int)this.GetValueFromCache(PropertyManager.ObjectClassCategory, true));
					this.typeInitialized = true;
				}
				return this.type;
			}
			set
			{
				this.CheckIfDisposed();
				if (value < SchemaClassType.Type88 || value > SchemaClassType.Auxiliary)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SchemaClassType));
				}
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.ObjectClassCategory, value);
				}
				this.type = value;
			}
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060002BE RID: 702 RVA: 0x0000AD45 File Offset: 0x00009D45
		// (set) Token: 0x060002BF RID: 703 RVA: 0x0000AD88 File Offset: 0x00009D88
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

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060002C0 RID: 704 RVA: 0x0000ADE4 File Offset: 0x00009DE4
		// (set) Token: 0x060002C1 RID: 705 RVA: 0x0000AE42 File Offset: 0x00009E42
		public ActiveDirectorySecurity DefaultObjectSecurityDescriptor
		{
			get
			{
				this.CheckIfDisposed();
				ActiveDirectorySecurity activeDirectorySecurity = null;
				if (this.isBound && !this.defaultSDSddlFormInitialized)
				{
					this.defaultSDSddlForm = (string)this.GetValueFromCache(PropertyManager.DefaultSecurityDescriptor, false);
					this.defaultSDSddlFormInitialized = true;
				}
				if (this.defaultSDSddlForm != null)
				{
					activeDirectorySecurity = new ActiveDirectorySecurity();
					activeDirectorySecurity.SetSecurityDescriptorSddlForm(this.defaultSDSddlForm);
				}
				return activeDirectorySecurity;
			}
			set
			{
				this.CheckIfDisposed();
				if (this.isBound)
				{
					this.SetProperty(PropertyManager.DefaultSecurityDescriptor, (value == null) ? null : value.GetSecurityDescriptorSddlForm(AccessControlSections.All));
				}
				this.defaultSDSddlForm = ((value == null) ? null : value.GetSecurityDescriptorSddlForm(AccessControlSections.All));
			}
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x0000AE7F File Offset: 0x00009E7F
		private void CheckIfDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		// Token: 0x060002C3 RID: 707 RVA: 0x0000AE9C File Offset: 0x00009E9C
		private object GetValueFromCache(string propertyName, bool mustExist)
		{
			object obj = null;
			this.InitializePropertiesFromSchemaContainer();
			ArrayList arrayList = (ArrayList)this.propertyValuesFromServer[propertyName.ToLower(CultureInfo.InvariantCulture)];
			if (arrayList.Count < 1 && mustExist)
			{
				throw new ActiveDirectoryOperationException(Res.GetString("PropertyNotFound", new object[] { propertyName }));
			}
			if (arrayList.Count > 0)
			{
				obj = arrayList[0];
			}
			return obj;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x0000AF08 File Offset: 0x00009F08
		private ICollection GetValuesFromCache(string propertyName)
		{
			this.InitializePropertiesFromSchemaContainer();
			return (ArrayList)this.propertyValuesFromServer[propertyName.ToLower(CultureInfo.InvariantCulture)];
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000AF38 File Offset: 0x00009F38
		private void InitializePropertiesFromSchemaContainer()
		{
			if (!this.propertiesFromSchemaContainerInitialized)
			{
				if (this.schemaEntry == null)
				{
					this.schemaEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.SchemaNamingContext);
				}
				this.propertyValuesFromServer = ActiveDirectorySchemaClass.GetPropertiesFromSchemaContainer(this.context, this.schemaEntry, this.isDefunctOnServer ? this.commonName : this.ldapDisplayName, this.isDefunctOnServer);
				this.propertiesFromSchemaContainerInitialized = true;
			}
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000AFA4 File Offset: 0x00009FA4
		internal static Hashtable GetPropertiesFromSchemaContainer(DirectoryContext context, DirectoryEntry schemaEntry, string name, bool isDefunctOnServer)
		{
			Hashtable hashtable = null;
			StringBuilder stringBuilder = new StringBuilder(15);
			stringBuilder.Append("(&(");
			stringBuilder.Append(PropertyManager.ObjectCategory);
			stringBuilder.Append("=classSchema)");
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
			ArrayList arrayList = new ArrayList();
			ArrayList arrayList2 = new ArrayList();
			arrayList2.Add(PropertyManager.DistinguishedName);
			arrayList2.Add(PropertyManager.Cn);
			arrayList2.Add(PropertyManager.Description);
			arrayList2.Add(PropertyManager.PossibleInferiors);
			arrayList2.Add(PropertyManager.SubClassOf);
			arrayList2.Add(PropertyManager.ObjectClassCategory);
			arrayList2.Add(PropertyManager.SchemaIDGuid);
			arrayList2.Add(PropertyManager.DefaultSecurityDescriptor);
			arrayList.Add(PropertyManager.AuxiliaryClass);
			arrayList.Add(PropertyManager.SystemAuxiliaryClass);
			arrayList.Add(PropertyManager.MustContain);
			arrayList.Add(PropertyManager.SystemMustContain);
			arrayList.Add(PropertyManager.MayContain);
			arrayList.Add(PropertyManager.SystemMayContain);
			if (isDefunctOnServer)
			{
				arrayList2.Add(PropertyManager.LdapDisplayName);
				arrayList2.Add(PropertyManager.GovernsID);
				arrayList.Add(PropertyManager.SystemPossibleSuperiors);
				arrayList.Add(PropertyManager.PossibleSuperiors);
			}
			try
			{
				hashtable = Utils.GetValuesWithRangeRetrieval(schemaEntry, stringBuilder.ToString(), arrayList, arrayList2, SearchScope.OneLevel);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147016656)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DSNotFound"), typeof(ActiveDirectorySchemaClass), name);
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			return hashtable;
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x0000B1B4 File Offset: 0x0000A1B4
		internal DirectoryEntry GetSchemaClassDirectoryEntry()
		{
			if (this.classEntry == null)
			{
				this.InitializePropertiesFromSchemaContainer();
				this.classEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, (string)this.GetValueFromCache(PropertyManager.DistinguishedName, true));
			}
			return this.classEntry;
		}

		// Token: 0x060002C8 RID: 712 RVA: 0x0000B1EC File Offset: 0x0000A1EC
		private void SetProperty(string propertyName, object value)
		{
			this.GetSchemaClassDirectoryEntry();
			try
			{
				if (value == null)
				{
					if (this.classEntry.Properties.Contains(propertyName))
					{
						this.classEntry.Properties[propertyName].Clear();
					}
				}
				else
				{
					this.classEntry.Properties[propertyName].Value = value;
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x0000B268 File Offset: 0x0000A268
		private ArrayList GetClasses(ICollection ldapDisplayNames)
		{
			ArrayList arrayList = new ArrayList();
			SearchResultCollection searchResultCollection = null;
			try
			{
				if (ldapDisplayNames.Count < 1)
				{
					return arrayList;
				}
				if (this.schemaEntry == null)
				{
					this.schemaEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.SchemaNamingContext);
				}
				StringBuilder stringBuilder = new StringBuilder(100);
				if (ldapDisplayNames.Count > 1)
				{
					stringBuilder.Append("(|");
				}
				foreach (object obj in ldapDisplayNames)
				{
					string text = (string)obj;
					stringBuilder.Append("(");
					stringBuilder.Append(PropertyManager.LdapDisplayName);
					stringBuilder.Append("=");
					stringBuilder.Append(Utils.GetEscapedFilterValue(text));
					stringBuilder.Append(")");
				}
				if (ldapDisplayNames.Count > 1)
				{
					stringBuilder.Append(")");
				}
				string text2 = string.Concat(new string[]
				{
					"(&(",
					PropertyManager.ObjectCategory,
					"=classSchema)",
					stringBuilder.ToString(),
					"(!(",
					PropertyManager.IsDefunct,
					"=TRUE)))"
				});
				string[] array = new string[] { PropertyManager.LdapDisplayName };
				ADSearcher adsearcher = new ADSearcher(this.schemaEntry, text2, array, SearchScope.OneLevel);
				searchResultCollection = adsearcher.FindAll();
				foreach (object obj2 in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj2;
					string text3 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.LdapDisplayName);
					DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
					directoryEntry.AuthenticationType = Utils.DefaultAuthType;
					directoryEntry.Username = this.context.UserName;
					directoryEntry.Password = this.context.Password;
					ActiveDirectorySchemaClass activeDirectorySchemaClass = new ActiveDirectorySchemaClass(this.context, text3, directoryEntry, this.schemaEntry);
					arrayList.Add(activeDirectorySchemaClass);
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

		// Token: 0x060002CA RID: 714 RVA: 0x0000B4F8 File Offset: 0x0000A4F8
		private ArrayList GetProperties(ICollection ldapDisplayNames)
		{
			ArrayList arrayList = new ArrayList();
			SearchResultCollection searchResultCollection = null;
			try
			{
				if (ldapDisplayNames.Count < 1)
				{
					return arrayList;
				}
				if (this.schemaEntry == null)
				{
					this.schemaEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.SchemaNamingContext);
				}
				StringBuilder stringBuilder = new StringBuilder(100);
				if (ldapDisplayNames.Count > 1)
				{
					stringBuilder.Append("(|");
				}
				foreach (object obj in ldapDisplayNames)
				{
					string text = (string)obj;
					stringBuilder.Append("(");
					stringBuilder.Append(PropertyManager.LdapDisplayName);
					stringBuilder.Append("=");
					stringBuilder.Append(Utils.GetEscapedFilterValue(text));
					stringBuilder.Append(")");
				}
				if (ldapDisplayNames.Count > 1)
				{
					stringBuilder.Append(")");
				}
				string text2 = string.Concat(new string[]
				{
					"(&(",
					PropertyManager.ObjectCategory,
					"=attributeSchema)",
					stringBuilder.ToString(),
					"(!(",
					PropertyManager.IsDefunct,
					"=TRUE)))"
				});
				string[] array = new string[] { PropertyManager.LdapDisplayName };
				ADSearcher adsearcher = new ADSearcher(this.schemaEntry, text2, array, SearchScope.OneLevel);
				searchResultCollection = adsearcher.FindAll();
				foreach (object obj2 in searchResultCollection)
				{
					SearchResult searchResult = (SearchResult)obj2;
					string text3 = (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.LdapDisplayName);
					DirectoryEntry directoryEntry = searchResult.GetDirectoryEntry();
					directoryEntry.AuthenticationType = Utils.DefaultAuthType;
					directoryEntry.Username = this.context.UserName;
					directoryEntry.Password = this.context.Password;
					ActiveDirectorySchemaProperty activeDirectorySchemaProperty = new ActiveDirectorySchemaProperty(this.context, text3, directoryEntry, this.schemaEntry);
					arrayList.Add(activeDirectorySchemaProperty);
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

		// Token: 0x060002CB RID: 715 RVA: 0x0000B788 File Offset: 0x0000A788
		private ArrayList GetPropertyValuesRecursively(string[] propertyNames)
		{
			ArrayList arrayList = new ArrayList();
			try
			{
				if (Utils.Compare(this.SubClassOf.Name, this.Name) != 0)
				{
					foreach (object obj in this.SubClassOf.GetPropertyValuesRecursively(propertyNames))
					{
						string text = (string)obj;
						if (!arrayList.Contains(text))
						{
							arrayList.Add(text);
						}
					}
				}
				foreach (object obj2 in this.GetValuesFromCache(PropertyManager.AuxiliaryClass))
				{
					string text2 = (string)obj2;
					ActiveDirectorySchemaClass activeDirectorySchemaClass = new ActiveDirectorySchemaClass(this.context, text2, null, null);
					foreach (object obj3 in activeDirectorySchemaClass.GetPropertyValuesRecursively(propertyNames))
					{
						string text3 = (string)obj3;
						if (!arrayList.Contains(text3))
						{
							arrayList.Add(text3);
						}
					}
				}
				foreach (object obj4 in this.GetValuesFromCache(PropertyManager.SystemAuxiliaryClass))
				{
					string text4 = (string)obj4;
					ActiveDirectorySchemaClass activeDirectorySchemaClass2 = new ActiveDirectorySchemaClass(this.context, text4, null, null);
					foreach (object obj5 in activeDirectorySchemaClass2.GetPropertyValuesRecursively(propertyNames))
					{
						string text5 = (string)obj5;
						if (!arrayList.Contains(text5))
						{
							arrayList.Add(text5);
						}
					}
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			foreach (string text6 in propertyNames)
			{
				foreach (object obj6 in this.GetValuesFromCache(text6))
				{
					string text7 = (string)obj6;
					if (!arrayList.Contains(text7))
					{
						arrayList.Add(text7);
					}
				}
			}
			return arrayList;
		}

		// Token: 0x040002D1 RID: 721
		private DirectoryEntry classEntry;

		// Token: 0x040002D2 RID: 722
		private DirectoryEntry schemaEntry;

		// Token: 0x040002D3 RID: 723
		private DirectoryEntry abstractClassEntry;

		// Token: 0x040002D4 RID: 724
		private NativeComInterfaces.IAdsClass iadsClass;

		// Token: 0x040002D5 RID: 725
		private DirectoryContext context;

		// Token: 0x040002D6 RID: 726
		internal bool isBound;

		// Token: 0x040002D7 RID: 727
		private bool disposed;

		// Token: 0x040002D8 RID: 728
		private ActiveDirectorySchema schema;

		// Token: 0x040002D9 RID: 729
		private bool propertiesFromSchemaContainerInitialized;

		// Token: 0x040002DA RID: 730
		private bool isDefunctOnServer;

		// Token: 0x040002DB RID: 731
		private Hashtable propertyValuesFromServer;

		// Token: 0x040002DC RID: 732
		private string ldapDisplayName;

		// Token: 0x040002DD RID: 733
		private string commonName;

		// Token: 0x040002DE RID: 734
		private string oid;

		// Token: 0x040002DF RID: 735
		private string description;

		// Token: 0x040002E0 RID: 736
		private bool descriptionInitialized;

		// Token: 0x040002E1 RID: 737
		private bool isDefunct;

		// Token: 0x040002E2 RID: 738
		private ActiveDirectorySchemaClassCollection possibleSuperiors;

		// Token: 0x040002E3 RID: 739
		private ActiveDirectorySchemaClassCollection auxiliaryClasses;

		// Token: 0x040002E4 RID: 740
		private ReadOnlyActiveDirectorySchemaClassCollection possibleInferiors;

		// Token: 0x040002E5 RID: 741
		private ActiveDirectorySchemaPropertyCollection mandatoryProperties;

		// Token: 0x040002E6 RID: 742
		private ActiveDirectorySchemaPropertyCollection optionalProperties;

		// Token: 0x040002E7 RID: 743
		private ActiveDirectorySchemaClass subClassOf;

		// Token: 0x040002E8 RID: 744
		private SchemaClassType type = SchemaClassType.Structural;

		// Token: 0x040002E9 RID: 745
		private bool typeInitialized;

		// Token: 0x040002EA RID: 746
		private byte[] schemaGuidBinaryForm;

		// Token: 0x040002EB RID: 747
		private string defaultSDSddlForm;

		// Token: 0x040002EC RID: 748
		private bool defaultSDSddlFormInitialized;
	}
}
