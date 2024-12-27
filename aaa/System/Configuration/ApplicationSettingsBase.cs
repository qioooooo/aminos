using System;
using System.Collections;
using System.ComponentModel;
using System.Deployment.Internal;
using System.Reflection;
using System.Security.Permissions;

namespace System.Configuration
{
	// Token: 0x020006E1 RID: 1761
	public abstract class ApplicationSettingsBase : SettingsBase, INotifyPropertyChanged
	{
		// Token: 0x0600364F RID: 13903 RVA: 0x000E7ED6 File Offset: 0x000E6ED6
		protected ApplicationSettingsBase()
		{
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x000E7EF0 File Offset: 0x000E6EF0
		protected ApplicationSettingsBase(IComponent owner)
			: this(owner, string.Empty)
		{
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x000E7EFE File Offset: 0x000E6EFE
		protected ApplicationSettingsBase(string settingsKey)
		{
			this._settingsKey = settingsKey;
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x000E7F20 File Offset: 0x000E6F20
		protected ApplicationSettingsBase(IComponent owner, string settingsKey)
			: this(settingsKey)
		{
			if (owner == null)
			{
				throw new ArgumentNullException("owner");
			}
			this._owner = owner;
			if (owner.Site != null)
			{
				ISettingsProviderService settingsProviderService = owner.Site.GetService(typeof(ISettingsProviderService)) as ISettingsProviderService;
				if (settingsProviderService != null)
				{
					foreach (object obj in this.Properties)
					{
						SettingsProperty settingsProperty = (SettingsProperty)obj;
						SettingsProvider settingsProvider = settingsProviderService.GetSettingsProvider(settingsProperty);
						if (settingsProvider != null)
						{
							settingsProperty.Provider = settingsProvider;
						}
					}
					this.ResetProviders();
				}
			}
		}

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06003653 RID: 13907 RVA: 0x000E7FD0 File Offset: 0x000E6FD0
		[Browsable(false)]
		public override SettingsContext Context
		{
			get
			{
				if (this._context == null)
				{
					if (base.IsSynchronized)
					{
						lock (this)
						{
							if (this._context == null)
							{
								this._context = new SettingsContext();
								this.EnsureInitialized();
							}
							goto IL_004B;
						}
					}
					this._context = new SettingsContext();
					this.EnsureInitialized();
				}
				IL_004B:
				return this._context;
			}
		}

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06003654 RID: 13908 RVA: 0x000E8040 File Offset: 0x000E7040
		[Browsable(false)]
		public override SettingsPropertyCollection Properties
		{
			get
			{
				if (this._settings == null)
				{
					if (base.IsSynchronized)
					{
						lock (this)
						{
							if (this._settings == null)
							{
								this._settings = new SettingsPropertyCollection();
								this.EnsureInitialized();
							}
							goto IL_004B;
						}
					}
					this._settings = new SettingsPropertyCollection();
					this.EnsureInitialized();
				}
				IL_004B:
				return this._settings;
			}
		}

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06003655 RID: 13909 RVA: 0x000E80B0 File Offset: 0x000E70B0
		[Browsable(false)]
		public override SettingsPropertyValueCollection PropertyValues
		{
			get
			{
				return base.PropertyValues;
			}
		}

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06003656 RID: 13910 RVA: 0x000E80B8 File Offset: 0x000E70B8
		[Browsable(false)]
		public override SettingsProviderCollection Providers
		{
			get
			{
				if (this._providers == null)
				{
					if (base.IsSynchronized)
					{
						lock (this)
						{
							if (this._providers == null)
							{
								this._providers = new SettingsProviderCollection();
								this.EnsureInitialized();
							}
							goto IL_004B;
						}
					}
					this._providers = new SettingsProviderCollection();
					this.EnsureInitialized();
				}
				IL_004B:
				return this._providers;
			}
		}

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x06003657 RID: 13911 RVA: 0x000E8128 File Offset: 0x000E7128
		// (set) Token: 0x06003658 RID: 13912 RVA: 0x000E8130 File Offset: 0x000E7130
		[Browsable(false)]
		public string SettingsKey
		{
			get
			{
				return this._settingsKey;
			}
			set
			{
				this._settingsKey = value;
				this.Context["SettingsKey"] = this._settingsKey;
			}
		}

		// Token: 0x1400004F RID: 79
		// (add) Token: 0x06003659 RID: 13913 RVA: 0x000E814F File Offset: 0x000E714F
		// (remove) Token: 0x0600365A RID: 13914 RVA: 0x000E8168 File Offset: 0x000E7168
		public event PropertyChangedEventHandler PropertyChanged
		{
			add
			{
				this._onPropertyChanged = (PropertyChangedEventHandler)Delegate.Combine(this._onPropertyChanged, value);
			}
			remove
			{
				this._onPropertyChanged = (PropertyChangedEventHandler)Delegate.Remove(this._onPropertyChanged, value);
			}
		}

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x0600365B RID: 13915 RVA: 0x000E8181 File Offset: 0x000E7181
		// (remove) Token: 0x0600365C RID: 13916 RVA: 0x000E819A File Offset: 0x000E719A
		public event SettingChangingEventHandler SettingChanging
		{
			add
			{
				this._onSettingChanging = (SettingChangingEventHandler)Delegate.Combine(this._onSettingChanging, value);
			}
			remove
			{
				this._onSettingChanging = (SettingChangingEventHandler)Delegate.Remove(this._onSettingChanging, value);
			}
		}

		// Token: 0x14000051 RID: 81
		// (add) Token: 0x0600365D RID: 13917 RVA: 0x000E81B3 File Offset: 0x000E71B3
		// (remove) Token: 0x0600365E RID: 13918 RVA: 0x000E81CC File Offset: 0x000E71CC
		public event SettingsLoadedEventHandler SettingsLoaded
		{
			add
			{
				this._onSettingsLoaded = (SettingsLoadedEventHandler)Delegate.Combine(this._onSettingsLoaded, value);
			}
			remove
			{
				this._onSettingsLoaded = (SettingsLoadedEventHandler)Delegate.Remove(this._onSettingsLoaded, value);
			}
		}

		// Token: 0x14000052 RID: 82
		// (add) Token: 0x0600365F RID: 13919 RVA: 0x000E81E5 File Offset: 0x000E71E5
		// (remove) Token: 0x06003660 RID: 13920 RVA: 0x000E81FE File Offset: 0x000E71FE
		public event SettingsSavingEventHandler SettingsSaving
		{
			add
			{
				this._onSettingsSaving = (SettingsSavingEventHandler)Delegate.Combine(this._onSettingsSaving, value);
			}
			remove
			{
				this._onSettingsSaving = (SettingsSavingEventHandler)Delegate.Remove(this._onSettingsSaving, value);
			}
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x000E8218 File Offset: 0x000E7218
		public object GetPreviousVersion(string propertyName)
		{
			if (this.Properties.Count == 0)
			{
				throw new SettingsPropertyNotFoundException();
			}
			SettingsProperty settingsProperty = this.Properties[propertyName];
			SettingsPropertyValue settingsPropertyValue = null;
			if (settingsProperty == null)
			{
				throw new SettingsPropertyNotFoundException();
			}
			IApplicationSettingsProvider applicationSettingsProvider = settingsProperty.Provider as IApplicationSettingsProvider;
			if (applicationSettingsProvider != null)
			{
				settingsPropertyValue = applicationSettingsProvider.GetPreviousVersion(this.Context, settingsProperty);
			}
			if (settingsPropertyValue != null)
			{
				return settingsPropertyValue.PropertyValue;
			}
			return null;
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x000E8278 File Offset: 0x000E7278
		protected virtual void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (this._onPropertyChanged != null)
			{
				this._onPropertyChanged(this, e);
			}
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x000E828F File Offset: 0x000E728F
		protected virtual void OnSettingChanging(object sender, SettingChangingEventArgs e)
		{
			if (this._onSettingChanging != null)
			{
				this._onSettingChanging(this, e);
			}
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x000E82A6 File Offset: 0x000E72A6
		protected virtual void OnSettingsLoaded(object sender, SettingsLoadedEventArgs e)
		{
			if (this._onSettingsLoaded != null)
			{
				this._onSettingsLoaded(this, e);
			}
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x000E82BD File Offset: 0x000E72BD
		protected virtual void OnSettingsSaving(object sender, CancelEventArgs e)
		{
			if (this._onSettingsSaving != null)
			{
				this._onSettingsSaving(this, e);
			}
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x000E82D4 File Offset: 0x000E72D4
		public void Reload()
		{
			if (this.PropertyValues != null)
			{
				this.PropertyValues.Clear();
			}
			foreach (object obj in this.Properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(settingsProperty.Name);
				this.OnPropertyChanged(this, propertyChangedEventArgs);
			}
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x000E8350 File Offset: 0x000E7350
		public void Reset()
		{
			if (this.Properties != null)
			{
				foreach (object obj in this.Providers)
				{
					SettingsProvider settingsProvider = (SettingsProvider)obj;
					IApplicationSettingsProvider applicationSettingsProvider = settingsProvider as IApplicationSettingsProvider;
					if (applicationSettingsProvider != null)
					{
						applicationSettingsProvider.Reset(this.Context);
					}
				}
			}
			this.Reload();
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x000E83C8 File Offset: 0x000E73C8
		public override void Save()
		{
			CancelEventArgs cancelEventArgs = new CancelEventArgs(false);
			this.OnSettingsSaving(this, cancelEventArgs);
			if (!cancelEventArgs.Cancel)
			{
				base.Save();
			}
		}

		// Token: 0x17000C96 RID: 3222
		public override object this[string propertyName]
		{
			get
			{
				if (base.IsSynchronized)
				{
					lock (this)
					{
						return this.GetPropertyValue(propertyName);
					}
				}
				return this.GetPropertyValue(propertyName);
			}
			set
			{
				SettingChangingEventArgs settingChangingEventArgs = new SettingChangingEventArgs(propertyName, base.GetType().FullName, this.SettingsKey, value, false);
				this.OnSettingChanging(this, settingChangingEventArgs);
				if (!settingChangingEventArgs.Cancel)
				{
					base[propertyName] = value;
					PropertyChangedEventArgs propertyChangedEventArgs = new PropertyChangedEventArgs(propertyName);
					this.OnPropertyChanged(this, propertyChangedEventArgs);
				}
			}
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x000E848C File Offset: 0x000E748C
		public virtual void Upgrade()
		{
			if (this.Properties != null)
			{
				foreach (object obj in this.Providers)
				{
					SettingsProvider settingsProvider = (SettingsProvider)obj;
					IApplicationSettingsProvider applicationSettingsProvider = settingsProvider as IApplicationSettingsProvider;
					if (applicationSettingsProvider != null)
					{
						applicationSettingsProvider.Upgrade(this.Context, this.GetPropertiesForProvider(settingsProvider));
					}
				}
			}
			this.Reload();
		}

		// Token: 0x0600366C RID: 13932 RVA: 0x000E850C File Offset: 0x000E750C
		private SettingsProperty CreateSetting(PropertyInfo propInfo)
		{
			object[] customAttributes = propInfo.GetCustomAttributes(false);
			SettingsProperty settingsProperty = new SettingsProperty(this.Initializer);
			bool flag = this._explicitSerializeOnClass;
			settingsProperty.Name = propInfo.Name;
			settingsProperty.PropertyType = propInfo.PropertyType;
			for (int i = 0; i < customAttributes.Length; i++)
			{
				Attribute attribute = customAttributes[i] as Attribute;
				if (attribute != null)
				{
					if (attribute is DefaultSettingValueAttribute)
					{
						settingsProperty.DefaultValue = ((DefaultSettingValueAttribute)attribute).Value;
					}
					else if (attribute is ReadOnlyAttribute)
					{
						settingsProperty.IsReadOnly = true;
					}
					else if (attribute is SettingsProviderAttribute)
					{
						string providerTypeName = ((SettingsProviderAttribute)attribute).ProviderTypeName;
						Type type = Type.GetType(providerTypeName);
						if (type == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("ProviderTypeLoadFailed", new object[] { providerTypeName }));
						}
						SettingsProvider settingsProvider = SecurityUtils.SecureCreateInstance(type) as SettingsProvider;
						if (settingsProvider == null)
						{
							throw new ConfigurationErrorsException(SR.GetString("ProviderInstantiationFailed", new object[] { providerTypeName }));
						}
						settingsProvider.Initialize(null, null);
						settingsProvider.ApplicationName = ConfigurationManagerInternalFactory.Instance.ExeProductName;
						SettingsProvider settingsProvider2 = this._providers[settingsProvider.Name];
						if (settingsProvider2 != null)
						{
							settingsProvider = settingsProvider2;
						}
						settingsProperty.Provider = settingsProvider;
					}
					else if (attribute is SettingsSerializeAsAttribute)
					{
						settingsProperty.SerializeAs = ((SettingsSerializeAsAttribute)attribute).SerializeAs;
						flag = true;
					}
					else
					{
						settingsProperty.Attributes.Add(attribute.GetType(), attribute);
					}
				}
			}
			if (!flag)
			{
				settingsProperty.SerializeAs = this.GetSerializeAs(propInfo.PropertyType);
			}
			return settingsProperty;
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x000E86A8 File Offset: 0x000E76A8
		private void EnsureInitialized()
		{
			if (!this._initialized)
			{
				this._initialized = true;
				Type type = base.GetType();
				if (this._context == null)
				{
					this._context = new SettingsContext();
				}
				this._context["GroupName"] = type.FullName;
				this._context["SettingsKey"] = this.SettingsKey;
				this._context["SettingsClassType"] = type;
				PropertyInfo[] array = this.SettingsFilter(type.GetProperties(BindingFlags.Instance | BindingFlags.Public));
				this._classAttributes = type.GetCustomAttributes(false);
				if (this._settings == null)
				{
					this._settings = new SettingsPropertyCollection();
				}
				if (this._providers == null)
				{
					this._providers = new SettingsProviderCollection();
				}
				for (int i = 0; i < array.Length; i++)
				{
					SettingsProperty settingsProperty = this.CreateSetting(array[i]);
					if (settingsProperty != null)
					{
						this._settings.Add(settingsProperty);
						if (settingsProperty.Provider != null && this._providers[settingsProperty.Provider.Name] == null)
						{
							this._providers.Add(settingsProperty.Provider);
						}
					}
				}
			}
		}

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x0600366E RID: 13934 RVA: 0x000E87B8 File Offset: 0x000E77B8
		private SettingsProperty Initializer
		{
			get
			{
				if (this._init == null)
				{
					this._init = new SettingsProperty("");
					this._init.DefaultValue = null;
					this._init.IsReadOnly = false;
					this._init.PropertyType = null;
					SettingsProvider settingsProvider = new LocalFileSettingsProvider();
					if (this._classAttributes != null)
					{
						for (int i = 0; i < this._classAttributes.Length; i++)
						{
							Attribute attribute = this._classAttributes[i] as Attribute;
							if (attribute != null)
							{
								if (attribute is ReadOnlyAttribute)
								{
									this._init.IsReadOnly = true;
								}
								else if (attribute is SettingsGroupNameAttribute)
								{
									if (this._context == null)
									{
										this._context = new SettingsContext();
									}
									this._context["GroupName"] = ((SettingsGroupNameAttribute)attribute).GroupName;
								}
								else if (attribute is SettingsProviderAttribute)
								{
									string providerTypeName = ((SettingsProviderAttribute)attribute).ProviderTypeName;
									Type type = Type.GetType(providerTypeName);
									if (type == null)
									{
										throw new ConfigurationErrorsException(SR.GetString("ProviderTypeLoadFailed", new object[] { providerTypeName }));
									}
									SettingsProvider settingsProvider2 = SecurityUtils.SecureCreateInstance(type) as SettingsProvider;
									if (settingsProvider2 == null)
									{
										throw new ConfigurationErrorsException(SR.GetString("ProviderInstantiationFailed", new object[] { providerTypeName }));
									}
									settingsProvider = settingsProvider2;
								}
								else if (attribute is SettingsSerializeAsAttribute)
								{
									this._init.SerializeAs = ((SettingsSerializeAsAttribute)attribute).SerializeAs;
									this._explicitSerializeOnClass = true;
								}
								else
								{
									this._init.Attributes.Add(attribute.GetType(), attribute);
								}
							}
						}
					}
					settingsProvider.Initialize(null, null);
					settingsProvider.ApplicationName = ConfigurationManagerInternalFactory.Instance.ExeProductName;
					this._init.Provider = settingsProvider;
				}
				return this._init;
			}
		}

		// Token: 0x0600366F RID: 13935 RVA: 0x000E8974 File Offset: 0x000E7974
		private SettingsPropertyCollection GetPropertiesForProvider(SettingsProvider provider)
		{
			SettingsPropertyCollection settingsPropertyCollection = new SettingsPropertyCollection();
			foreach (object obj in this.Properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				if (settingsProperty.Provider == provider)
				{
					settingsPropertyCollection.Add(settingsProperty);
				}
			}
			return settingsPropertyCollection;
		}

		// Token: 0x06003670 RID: 13936 RVA: 0x000E89E0 File Offset: 0x000E79E0
		private object GetPropertyValue(string propertyName)
		{
			if (this.PropertyValues[propertyName] == null)
			{
				object obj = base[propertyName];
				SettingsProperty settingsProperty = this.Properties[propertyName];
				SettingsProvider settingsProvider = ((settingsProperty != null) ? settingsProperty.Provider : null);
				if (this._firstLoad)
				{
					this._firstLoad = false;
					if (this.IsFirstRunOfClickOnceApp())
					{
						this.Upgrade();
					}
				}
				SettingsLoadedEventArgs settingsLoadedEventArgs = new SettingsLoadedEventArgs(settingsProvider);
				this.OnSettingsLoaded(this, settingsLoadedEventArgs);
				return base[propertyName];
			}
			return base[propertyName];
		}

		// Token: 0x06003671 RID: 13937 RVA: 0x000E8A58 File Offset: 0x000E7A58
		private SettingsSerializeAs GetSerializeAs(Type type)
		{
			TypeConverter converter = TypeDescriptor.GetConverter(type);
			bool flag = converter.CanConvertTo(typeof(string));
			bool flag2 = converter.CanConvertFrom(typeof(string));
			if (flag && flag2)
			{
				return SettingsSerializeAs.String;
			}
			return SettingsSerializeAs.Xml;
		}

		// Token: 0x06003672 RID: 13938 RVA: 0x000E8A98 File Offset: 0x000E7A98
		private bool IsFirstRunOfClickOnceApp()
		{
			ActivationContext activationContext = AppDomain.CurrentDomain.ActivationContext;
			return ApplicationSettingsBase.IsClickOnceDeployed(AppDomain.CurrentDomain) && InternalActivationContextHelper.IsFirstRun(activationContext);
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x000E8AC4 File Offset: 0x000E7AC4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		internal static bool IsClickOnceDeployed(AppDomain appDomain)
		{
			ActivationContext activationContext = appDomain.ActivationContext;
			if (activationContext != null && activationContext.Form == ActivationContext.ContextForm.StoreBounded)
			{
				string fullName = activationContext.Identity.FullName;
				if (!string.IsNullOrEmpty(fullName))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x000E8AFC File Offset: 0x000E7AFC
		private PropertyInfo[] SettingsFilter(PropertyInfo[] allProps)
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < allProps.Length; i++)
			{
				object[] customAttributes = allProps[i].GetCustomAttributes(false);
				for (int j = 0; j < customAttributes.Length; j++)
				{
					Attribute attribute = customAttributes[j] as Attribute;
					if (attribute is SettingAttribute)
					{
						arrayList.Add(allProps[i]);
						break;
					}
				}
			}
			return (PropertyInfo[])arrayList.ToArray(typeof(PropertyInfo));
		}

		// Token: 0x06003675 RID: 13941 RVA: 0x000E8B6C File Offset: 0x000E7B6C
		private void ResetProviders()
		{
			this.Providers.Clear();
			foreach (object obj in this.Properties)
			{
				SettingsProperty settingsProperty = (SettingsProperty)obj;
				if (this.Providers[settingsProperty.Provider.Name] == null)
				{
					this.Providers.Add(settingsProperty.Provider);
				}
			}
		}

		// Token: 0x04003175 RID: 12661
		private bool _explicitSerializeOnClass;

		// Token: 0x04003176 RID: 12662
		private object[] _classAttributes;

		// Token: 0x04003177 RID: 12663
		private IComponent _owner;

		// Token: 0x04003178 RID: 12664
		private PropertyChangedEventHandler _onPropertyChanged;

		// Token: 0x04003179 RID: 12665
		private SettingsContext _context;

		// Token: 0x0400317A RID: 12666
		private SettingsProperty _init;

		// Token: 0x0400317B RID: 12667
		private SettingsPropertyCollection _settings;

		// Token: 0x0400317C RID: 12668
		private SettingsProviderCollection _providers;

		// Token: 0x0400317D RID: 12669
		private SettingChangingEventHandler _onSettingChanging;

		// Token: 0x0400317E RID: 12670
		private SettingsLoadedEventHandler _onSettingsLoaded;

		// Token: 0x0400317F RID: 12671
		private SettingsSavingEventHandler _onSettingsSaving;

		// Token: 0x04003180 RID: 12672
		private string _settingsKey = string.Empty;

		// Token: 0x04003181 RID: 12673
		private bool _firstLoad = true;

		// Token: 0x04003182 RID: 12674
		private bool _initialized;
	}
}
