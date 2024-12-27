using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Provider;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using System.Web.Compilation;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Security;

namespace System.Web.Profile
{
	// Token: 0x02000302 RID: 770
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class ProfileBase : SettingsBase
	{
		// Token: 0x170007F1 RID: 2033
		public override object this[string propertyName]
		{
			get
			{
				if (HttpRuntime.NamedPermissionSet != null && HttpRuntime.ProcessRequestInApplicationTrust)
				{
					HttpRuntime.NamedPermissionSet.PermitOnly();
				}
				return this.GetInternal(propertyName);
			}
			set
			{
				if (HttpRuntime.NamedPermissionSet != null && HttpRuntime.ProcessRequestInApplicationTrust)
				{
					HttpRuntime.NamedPermissionSet.PermitOnly();
				}
				this.SetInternal(propertyName, value);
			}
		}

		// Token: 0x06002626 RID: 9766 RVA: 0x000A352D File Offset: 0x000A252D
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private object GetInternal(string propertyName)
		{
			return base[propertyName];
		}

		// Token: 0x06002627 RID: 9767 RVA: 0x000A3538 File Offset: 0x000A2538
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private void SetInternal(string propertyName, object value)
		{
			if (!this._IsAuthenticated)
			{
				SettingsProperty settingsProperty = ProfileBase.s_Properties[propertyName];
				if (settingsProperty != null && !(bool)settingsProperty.Attributes["AllowAnonymous"])
				{
					throw new ProviderException(SR.GetString("Profile_anonoymous_not_allowed_to_set_property"));
				}
			}
			base[propertyName] = value;
		}

		// Token: 0x06002628 RID: 9768 RVA: 0x000A358D File Offset: 0x000A258D
		public object GetPropertyValue(string propertyName)
		{
			return this[propertyName];
		}

		// Token: 0x06002629 RID: 9769 RVA: 0x000A3596 File Offset: 0x000A2596
		public void SetPropertyValue(string propertyName, object propertyValue)
		{
			this[propertyName] = propertyValue;
		}

		// Token: 0x0600262A RID: 9770 RVA: 0x000A35A0 File Offset: 0x000A25A0
		public ProfileGroupBase GetProfileGroup(string groupName)
		{
			ProfileGroupBase profileGroupBase = (ProfileGroupBase)this._Groups[groupName];
			if (profileGroupBase == null)
			{
				Type type = BuildManager.GetProfileType();
				if (type == null)
				{
					throw new ProviderException(SR.GetString("Profile_group_not_found", new object[] { groupName }));
				}
				type = type.Assembly.GetType("ProfileGroup" + groupName, false);
				if (type == null)
				{
					throw new ProviderException(SR.GetString("Profile_group_not_found", new object[] { groupName }));
				}
				profileGroupBase = (ProfileGroupBase)Activator.CreateInstance(type);
				profileGroupBase.Init(this, groupName);
			}
			return profileGroupBase;
		}

		// Token: 0x0600262B RID: 9771 RVA: 0x000A3633 File Offset: 0x000A2633
		public ProfileBase()
		{
			if (!ProfileManager.Enabled)
			{
				throw new ProviderException(SR.GetString("Profile_not_enabled"));
			}
			if (!ProfileBase.s_Initialized)
			{
				ProfileBase.InitializeStatic();
			}
		}

		// Token: 0x0600262C RID: 9772 RVA: 0x000A366C File Offset: 0x000A266C
		public void Initialize(string username, bool isAuthenticated)
		{
			if (username != null)
			{
				this._UserName = username.Trim();
			}
			else
			{
				this._UserName = username;
			}
			SettingsContext settingsContext = new SettingsContext();
			settingsContext.Add("UserName", this._UserName);
			settingsContext.Add("IsAuthenticated", isAuthenticated);
			this._IsAuthenticated = isAuthenticated;
			base.Initialize(settingsContext, ProfileBase.s_Properties, ProfileManager.Providers);
		}

		// Token: 0x0600262D RID: 9773 RVA: 0x000A36D1 File Offset: 0x000A26D1
		public override void Save()
		{
			if (HttpRuntime.NamedPermissionSet != null && HttpRuntime.ProcessRequestInApplicationTrust)
			{
				HttpRuntime.NamedPermissionSet.PermitOnly();
			}
			this.SaveWithAssert();
		}

		// Token: 0x0600262E RID: 9774 RVA: 0x000A36F1 File Offset: 0x000A26F1
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.SerializationFormatter)]
		private void SaveWithAssert()
		{
			base.Save();
			this._IsDirty = false;
			this._DatesRetrieved = false;
		}

		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x0600262F RID: 9775 RVA: 0x000A3707 File Offset: 0x000A2707
		public string UserName
		{
			get
			{
				return this._UserName;
			}
		}

		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06002630 RID: 9776 RVA: 0x000A370F File Offset: 0x000A270F
		public bool IsAnonymous
		{
			get
			{
				return !this._IsAuthenticated;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06002631 RID: 9777 RVA: 0x000A371C File Offset: 0x000A271C
		public bool IsDirty
		{
			get
			{
				if (this._IsDirty)
				{
					return true;
				}
				foreach (object obj in this.PropertyValues)
				{
					SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj;
					if (settingsPropertyValue.IsDirty)
					{
						this._IsDirty = true;
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06002632 RID: 9778 RVA: 0x000A3790 File Offset: 0x000A2790
		public DateTime LastActivityDate
		{
			get
			{
				if (!this._DatesRetrieved)
				{
					this.RetrieveDates();
				}
				return this._LastActivityDate.ToLocalTime();
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06002633 RID: 9779 RVA: 0x000A37AB File Offset: 0x000A27AB
		public DateTime LastUpdatedDate
		{
			get
			{
				if (!this._DatesRetrieved)
				{
					this.RetrieveDates();
				}
				return this._LastUpdatedDate.ToLocalTime();
			}
		}

		// Token: 0x06002634 RID: 9780 RVA: 0x000A37C6 File Offset: 0x000A27C6
		public static ProfileBase Create(string username)
		{
			return ProfileBase.Create(username, true);
		}

		// Token: 0x06002635 RID: 9781 RVA: 0x000A37D0 File Offset: 0x000A27D0
		public static ProfileBase Create(string username, bool isAuthenticated)
		{
			if (!ProfileManager.Enabled)
			{
				throw new ProviderException(SR.GetString("Profile_not_enabled"));
			}
			ProfileBase.InitializeStatic();
			if (ProfileBase.s_SingletonInstance != null)
			{
				return ProfileBase.s_SingletonInstance;
			}
			if (ProfileBase.s_Properties.Count == 0)
			{
				lock (ProfileBase.s_InitializeLock)
				{
					if (ProfileBase.s_SingletonInstance == null)
					{
						ProfileBase.s_SingletonInstance = new DefaultProfile();
					}
					return ProfileBase.s_SingletonInstance;
				}
			}
			HttpRuntime.CheckAspNetHostingPermission(AspNetHostingPermissionLevel.Low, "Feature_not_supported_at_this_level");
			return ProfileBase.CreateMyInstance(username, isAuthenticated);
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06002636 RID: 9782 RVA: 0x000A3868 File Offset: 0x000A2868
		public new static SettingsPropertyCollection Properties
		{
			get
			{
				ProfileBase.InitializeStatic();
				return ProfileBase.s_Properties;
			}
		}

		// Token: 0x170007F8 RID: 2040
		// (get) Token: 0x06002637 RID: 9783 RVA: 0x000A3874 File Offset: 0x000A2874
		internal static Type InheritsFromType
		{
			get
			{
				if (!ProfileManager.Enabled)
				{
					return typeof(DefaultProfile);
				}
				Type type;
				if (HostingEnvironment.IsHosted)
				{
					type = BuildManager.GetType(ProfileBase.InheritsFromTypeString, true, true);
				}
				else
				{
					type = ProfileBase.GetPropType(ProfileBase.InheritsFromTypeString);
				}
				if (!typeof(ProfileBase).IsAssignableFrom(type))
				{
					ProfileSection profile = RuntimeConfig.GetAppConfig().Profile;
					throw new ConfigurationErrorsException(SR.GetString("Wrong_profile_base_type"), null, profile.ElementInformation.Properties["inherits"].Source, profile.ElementInformation.Properties["inherit"].LineNumber);
				}
				return type;
			}
		}

		// Token: 0x170007F9 RID: 2041
		// (get) Token: 0x06002638 RID: 9784 RVA: 0x000A3918 File Offset: 0x000A2918
		internal static string InheritsFromTypeString
		{
			get
			{
				string text = typeof(ProfileBase).ToString();
				if (!ProfileManager.Enabled)
				{
					return text;
				}
				ProfileSection profile = RuntimeConfig.GetAppConfig().Profile;
				if (profile.Inherits == null)
				{
					return text;
				}
				string text2 = profile.Inherits.Trim();
				if (text2.Length < 1)
				{
					return text;
				}
				Type type = Type.GetType(text2, false, true);
				if (type == null)
				{
					return text2;
				}
				if (!typeof(ProfileBase).IsAssignableFrom(type))
				{
					throw new ConfigurationErrorsException(SR.GetString("Wrong_profile_base_type"), null, profile.ElementInformation.Properties["inherits"].Source, profile.ElementInformation.Properties["inherit"].LineNumber);
				}
				return type.AssemblyQualifiedName;
			}
		}

		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06002639 RID: 9785 RVA: 0x000A39D8 File Offset: 0x000A29D8
		internal static bool InheritsFromCustomType
		{
			get
			{
				if (!ProfileManager.Enabled)
				{
					return false;
				}
				ProfileSection profile = RuntimeConfig.GetAppConfig().Profile;
				if (profile.Inherits == null)
				{
					return false;
				}
				string text = profile.Inherits.Trim();
				if (text == null || text.Length < 1)
				{
					return false;
				}
				Type type = Type.GetType(text, false, true);
				return type == null || type != typeof(ProfileBase);
			}
		}

		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x0600263A RID: 9786 RVA: 0x000A3A39 File Offset: 0x000A2A39
		internal static ProfileBase SingletonInstance
		{
			get
			{
				return ProfileBase.s_SingletonInstance;
			}
		}

		// Token: 0x0600263B RID: 9787 RVA: 0x000A3A40 File Offset: 0x000A2A40
		internal static Hashtable GetPropertiesForCompilation()
		{
			if (!ProfileManager.Enabled)
			{
				return null;
			}
			if (ProfileBase.s_PropertiesForCompilation != null)
			{
				return ProfileBase.s_PropertiesForCompilation;
			}
			lock (ProfileBase.s_InitializeLock)
			{
				if (ProfileBase.s_PropertiesForCompilation != null)
				{
					return ProfileBase.s_PropertiesForCompilation;
				}
				Hashtable hashtable = new Hashtable();
				ProfileSection profile = RuntimeConfig.GetAppConfig().Profile;
				if (profile.PropertySettings == null)
				{
					ProfileBase.s_PropertiesForCompilation = hashtable;
					return ProfileBase.s_PropertiesForCompilation;
				}
				ProfileBase.AddProfilePropertySettingsForCompilation(profile.PropertySettings, hashtable, null);
				foreach (object obj2 in profile.PropertySettings.GroupSettings)
				{
					ProfileGroupSettings profileGroupSettings = (ProfileGroupSettings)obj2;
					ProfileBase.AddProfilePropertySettingsForCompilation(profileGroupSettings.PropertySettings, hashtable, profileGroupSettings.Name);
				}
				ProfileBase.s_PropertiesForCompilation = hashtable;
			}
			return ProfileBase.s_PropertiesForCompilation;
		}

		// Token: 0x0600263C RID: 9788 RVA: 0x000A3B3C File Offset: 0x000A2B3C
		internal static string GetProfileClassName()
		{
			Hashtable propertiesForCompilation = ProfileBase.GetPropertiesForCompilation();
			if (propertiesForCompilation == null)
			{
				return "System.Web.Profile.DefaultProfile";
			}
			if (propertiesForCompilation.Count > 0 || ProfileBase.InheritsFromCustomType)
			{
				return "ProfileCommon";
			}
			return "System.Web.Profile.DefaultProfile";
		}

		// Token: 0x0600263D RID: 9789 RVA: 0x000A3B74 File Offset: 0x000A2B74
		private static void AddProfilePropertySettingsForCompilation(ProfilePropertySettingsCollection propertyCollection, Hashtable ht, string groupName)
		{
			foreach (object obj in propertyCollection)
			{
				ProfilePropertySettings profilePropertySettings = (ProfilePropertySettings)obj;
				ProfileNameTypeStruct profileNameTypeStruct = new ProfileNameTypeStruct();
				if (groupName != null)
				{
					profileNameTypeStruct.Name = groupName + "." + profilePropertySettings.Name;
				}
				else
				{
					profileNameTypeStruct.Name = profilePropertySettings.Name;
				}
				Type type = profilePropertySettings.TypeInternal;
				if (type == null)
				{
					type = ProfileBase.ResolvePropertyTypeForCommonTypes(profilePropertySettings.Type.ToLower(CultureInfo.InvariantCulture));
				}
				if (type == null)
				{
					type = BuildManager.GetType(profilePropertySettings.Type, false);
				}
				if (type == null)
				{
					profileNameTypeStruct.PropertyCodeRefType = new CodeTypeReference(profilePropertySettings.Type);
				}
				else
				{
					profileNameTypeStruct.PropertyCodeRefType = new CodeTypeReference(type);
				}
				profileNameTypeStruct.PropertyType = type;
				profilePropertySettings.TypeInternal = type;
				profileNameTypeStruct.IsReadOnly = profilePropertySettings.ReadOnly;
				profileNameTypeStruct.LineNumber = profilePropertySettings.ElementInformation.Properties["name"].LineNumber;
				profileNameTypeStruct.FileName = profilePropertySettings.ElementInformation.Properties["name"].Source;
				ht.Add(profileNameTypeStruct.Name, profileNameTypeStruct);
			}
		}

		// Token: 0x0600263E RID: 9790 RVA: 0x000A3CC0 File Offset: 0x000A2CC0
		private static ProfileBase CreateMyInstance(string username, bool isAuthenticated)
		{
			Type type;
			if (HostingEnvironment.IsHosted)
			{
				type = BuildManager.GetProfileType();
			}
			else
			{
				type = ProfileBase.InheritsFromType;
			}
			ProfileBase profileBase = (ProfileBase)Activator.CreateInstance(type);
			profileBase.Initialize(username, isAuthenticated);
			return profileBase;
		}

		// Token: 0x0600263F RID: 9791 RVA: 0x000A3CF8 File Offset: 0x000A2CF8
		private static void InitializeStatic()
		{
			if (ProfileManager.Enabled && !ProfileBase.s_Initialized)
			{
				lock (ProfileBase.s_InitializeLock)
				{
					if (ProfileBase.s_Initialized)
					{
						if (ProfileBase.s_InitializeException != null)
						{
							throw ProfileBase.s_InitializeException;
						}
						return;
					}
					else
					{
						try
						{
							ProfileSection profile = RuntimeConfig.GetAppConfig().Profile;
							bool flag = !HostingEnvironment.IsHosted || AnonymousIdentificationModule.Enabled;
							Type inheritsFromType = ProfileBase.InheritsFromType;
							bool flag2 = HttpRuntime.HasAspNetHostingPermission(AspNetHostingPermissionLevel.Low);
							ProfileBase.s_Properties = new SettingsPropertyCollection();
							if (inheritsFromType != typeof(ProfileBase))
							{
								PropertyInfo[] properties = typeof(ProfileBase).GetProperties();
								NameValueCollection nameValueCollection = new NameValueCollection(properties.Length);
								foreach (PropertyInfo propertyInfo in properties)
								{
									nameValueCollection.Add(propertyInfo.Name, string.Empty);
								}
								PropertyInfo[] properties2 = inheritsFromType.GetProperties();
								foreach (PropertyInfo propertyInfo2 in properties2)
								{
									if (nameValueCollection[propertyInfo2.Name] == null)
									{
										ProfileProvider profileProvider = (flag2 ? ProfileManager.Provider : null);
										bool flag3 = false;
										SettingsSerializeAs settingsSerializeAs = SettingsSerializeAs.ProviderSpecific;
										string text = string.Empty;
										bool flag4 = false;
										string text2 = null;
										Attribute[] customAttributes = Attribute.GetCustomAttributes(propertyInfo2, true);
										foreach (Attribute attribute in customAttributes)
										{
											if (attribute is SettingsSerializeAsAttribute)
											{
												settingsSerializeAs = ((SettingsSerializeAsAttribute)attribute).SerializeAs;
											}
											else if (attribute is SettingsAllowAnonymousAttribute)
											{
												flag4 = ((SettingsAllowAnonymousAttribute)attribute).Allow;
												if (!flag && flag4)
												{
													throw new ConfigurationErrorsException(SR.GetString("Annoymous_id_module_not_enabled", new object[] { propertyInfo2.Name }), profile.ElementInformation.Properties["inherits"].Source, profile.ElementInformation.Properties["inherits"].LineNumber);
												}
											}
											else if (attribute is ReadOnlyAttribute)
											{
												flag3 = ((ReadOnlyAttribute)attribute).IsReadOnly;
											}
											else if (attribute is DefaultSettingValueAttribute)
											{
												text = ((DefaultSettingValueAttribute)attribute).Value;
											}
											else if (attribute is CustomProviderDataAttribute)
											{
												text2 = ((CustomProviderDataAttribute)attribute).CustomProviderData;
											}
											else if (flag2 && attribute is ProfileProviderAttribute)
											{
												profileProvider = ProfileManager.Providers[((ProfileProviderAttribute)attribute).ProviderName];
												if (profileProvider == null)
												{
													throw new ConfigurationErrorsException(SR.GetString("Profile_provider_not_found", new object[] { ((ProfileProviderAttribute)attribute).ProviderName }), profile.ElementInformation.Properties["inherits"].Source, profile.ElementInformation.Properties["inherits"].LineNumber);
												}
											}
										}
										SettingsAttributeDictionary settingsAttributeDictionary = new SettingsAttributeDictionary();
										settingsAttributeDictionary.Add("AllowAnonymous", flag4);
										if (!string.IsNullOrEmpty(text2))
										{
											settingsAttributeDictionary.Add("CustomProviderData", text2);
										}
										SettingsProperty settingsProperty = new SettingsProperty(propertyInfo2.Name, propertyInfo2.PropertyType, profileProvider, flag3, text, settingsSerializeAs, settingsAttributeDictionary, false, true);
										ProfileBase.s_Properties.Add(settingsProperty);
									}
								}
							}
							if (profile.PropertySettings != null)
							{
								ProfileBase.AddPropertySettingsFromConfig(inheritsFromType, flag, flag2, profile.PropertySettings, null);
								foreach (object obj2 in profile.PropertySettings.GroupSettings)
								{
									ProfileGroupSettings profileGroupSettings = (ProfileGroupSettings)obj2;
									ProfileBase.AddPropertySettingsFromConfig(inheritsFromType, flag, flag2, profileGroupSettings.PropertySettings, profileGroupSettings.Name);
								}
							}
						}
						catch (Exception ex)
						{
							if (ProfileBase.s_InitializeException == null)
							{
								ProfileBase.s_InitializeException = ex;
							}
						}
						if (ProfileBase.s_Properties == null)
						{
							ProfileBase.s_Properties = new SettingsPropertyCollection();
						}
						ProfileBase.s_Properties.SetReadOnly();
						ProfileBase.s_Initialized = true;
					}
				}
				if (ProfileBase.s_InitializeException != null)
				{
					throw ProfileBase.s_InitializeException;
				}
				return;
			}
			if (ProfileBase.s_InitializeException != null)
			{
				throw ProfileBase.s_InitializeException;
			}
		}

		// Token: 0x06002640 RID: 9792 RVA: 0x000A4144 File Offset: 0x000A3144
		private static void AddPropertySettingsFromConfig(Type baseType, bool fAnonEnabled, bool hasLowTrust, ProfilePropertySettingsCollection settingsCollection, string groupName)
		{
			foreach (object obj in settingsCollection)
			{
				ProfilePropertySettings profilePropertySettings = (ProfilePropertySettings)obj;
				string text = ((groupName != null) ? (groupName + "." + profilePropertySettings.Name) : profilePropertySettings.Name);
				if (baseType != typeof(ProfileBase) && ProfileBase.s_Properties[text] != null)
				{
					throw new ConfigurationErrorsException(SR.GetString("Profile_property_already_added"), null, profilePropertySettings.ElementInformation.Properties["name"].Source, profilePropertySettings.ElementInformation.Properties["name"].LineNumber);
				}
				try
				{
					if (profilePropertySettings.TypeInternal == null)
					{
						profilePropertySettings.TypeInternal = ProfileBase.ResolvePropertyType(profilePropertySettings.Type);
					}
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("Profile_could_not_create_type", new object[] { ex.Message }), ex, profilePropertySettings.ElementInformation.Properties["type"].Source, profilePropertySettings.ElementInformation.Properties["type"].LineNumber);
				}
				if (!fAnonEnabled)
				{
					bool allowAnonymous = profilePropertySettings.AllowAnonymous;
					if (allowAnonymous)
					{
						throw new ConfigurationErrorsException(SR.GetString("Annoymous_id_module_not_enabled", new object[] { profilePropertySettings.Name }), profilePropertySettings.ElementInformation.Properties["allowAnonymous"].Source, profilePropertySettings.ElementInformation.Properties["allowAnonymous"].LineNumber);
					}
				}
				if (profilePropertySettings.SerializeAs == SerializationMode.Binary && !profilePropertySettings.TypeInternal.IsSerializable)
				{
					throw new ConfigurationErrorsException(SR.GetString("Property_not_serializable", new object[] { profilePropertySettings.Name }), profilePropertySettings.ElementInformation.Properties["serializeAs"].Source, profilePropertySettings.ElementInformation.Properties["serializeAs"].LineNumber);
				}
				if (hasLowTrust)
				{
					ProfileBase.SetProviderForProperty(profilePropertySettings);
				}
				else
				{
					profilePropertySettings.ProviderInternal = null;
				}
				SettingsAttributeDictionary settingsAttributeDictionary = new SettingsAttributeDictionary();
				settingsAttributeDictionary.Add("AllowAnonymous", profilePropertySettings.AllowAnonymous);
				if (!string.IsNullOrEmpty(profilePropertySettings.CustomProviderData))
				{
					settingsAttributeDictionary.Add("CustomProviderData", profilePropertySettings.CustomProviderData);
				}
				SettingsProperty settingsProperty = new SettingsProperty(text, profilePropertySettings.TypeInternal, profilePropertySettings.ProviderInternal, profilePropertySettings.ReadOnly, profilePropertySettings.DefaultValue, (SettingsSerializeAs)profilePropertySettings.SerializeAs, settingsAttributeDictionary, false, true);
				ProfileBase.s_Properties.Add(settingsProperty);
			}
		}

		// Token: 0x06002641 RID: 9793 RVA: 0x000A4410 File Offset: 0x000A3410
		private static void SetProviderForProperty(ProfilePropertySettings pps)
		{
			if (pps.Provider == null || pps.Provider.Length < 1)
			{
				pps.ProviderInternal = ProfileManager.Provider;
			}
			else
			{
				pps.ProviderInternal = ProfileManager.Providers[pps.Provider];
			}
			if (pps.ProviderInternal == null)
			{
				throw new ConfigurationErrorsException(SR.GetString("Profile_provider_not_found", new object[] { pps.Provider }), pps.ElementInformation.Properties["provider"].Source, pps.ElementInformation.Properties["provider"].LineNumber);
			}
		}

		// Token: 0x06002642 RID: 9794 RVA: 0x000A44B4 File Offset: 0x000A34B4
		private static Type ResolvePropertyTypeForCommonTypes(string typeName)
		{
			switch (typeName)
			{
			case "string":
				return typeof(string);
			case "byte":
			case "int8":
				return typeof(byte);
			case "boolean":
			case "bool":
				return typeof(bool);
			case "char":
				return typeof(char);
			case "int":
			case "integer":
			case "int32":
				return typeof(int);
			case "date":
			case "datetime":
				return typeof(DateTime);
			case "decimal":
				return typeof(decimal);
			case "double":
			case "float64":
				return typeof(double);
			case "float":
			case "float32":
				return typeof(float);
			case "long":
			case "int64":
				return typeof(long);
			case "short":
			case "int16":
				return typeof(short);
			case "single":
				return typeof(float);
			case "uint16":
			case "ushort":
				return typeof(ushort);
			case "uint32":
			case "uint":
				return typeof(uint);
			case "ulong":
			case "uint64":
				return typeof(ulong);
			case "object":
				return typeof(object);
			}
			return null;
		}

		// Token: 0x06002643 RID: 9795 RVA: 0x000A4788 File Offset: 0x000A3788
		private static Type ResolvePropertyType(string typeName)
		{
			Type type = ProfileBase.ResolvePropertyTypeForCommonTypes(typeName.ToLower(CultureInfo.InvariantCulture));
			if (type != null)
			{
				return type;
			}
			if (HostingEnvironment.IsHosted)
			{
				return BuildManager.GetType(typeName, true, true);
			}
			return ProfileBase.GetPropType(typeName);
		}

		// Token: 0x06002644 RID: 9796 RVA: 0x000A47C4 File Offset: 0x000A37C4
		private static Type GetPropType(string typeName)
		{
			Exception ex = null;
			try
			{
				return Type.GetType(typeName, true, true);
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			try
			{
				CompilationSection compilation = RuntimeConfig.GetAppConfig().Compilation;
				if (compilation != null)
				{
					AssemblyCollection assemblies = compilation.Assemblies;
					if (assemblies != null)
					{
						foreach (object obj in assemblies)
						{
							Assembly assembly = (Assembly)obj;
							Type type = assembly.GetType(typeName, false, true);
							if (type != null)
							{
								return type;
							}
						}
					}
				}
			}
			catch
			{
			}
			throw ex;
		}

		// Token: 0x06002645 RID: 9797 RVA: 0x000A487C File Offset: 0x000A387C
		private void RetrieveDates()
		{
			if (this._DatesRetrieved || ProfileManager.Provider == null)
			{
				return;
			}
			int num;
			ProfileInfoCollection profileInfoCollection = ProfileManager.Provider.FindProfilesByUserName(ProfileAuthenticationOption.All, this._UserName, 0, 1, out num);
			using (IEnumerator enumerator = profileInfoCollection.GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					ProfileInfo profileInfo = (ProfileInfo)enumerator.Current;
					this._LastActivityDate = profileInfo.LastActivityDate.ToUniversalTime();
					this._LastUpdatedDate = profileInfo.LastUpdatedDate.ToUniversalTime();
					this._DatesRetrieved = true;
				}
			}
		}

		// Token: 0x04001D9D RID: 7581
		private Hashtable _Groups = new Hashtable();

		// Token: 0x04001D9E RID: 7582
		private bool _IsAuthenticated;

		// Token: 0x04001D9F RID: 7583
		private string _UserName;

		// Token: 0x04001DA0 RID: 7584
		private bool _IsDirty;

		// Token: 0x04001DA1 RID: 7585
		private DateTime _LastActivityDate;

		// Token: 0x04001DA2 RID: 7586
		private DateTime _LastUpdatedDate;

		// Token: 0x04001DA3 RID: 7587
		private bool _DatesRetrieved;

		// Token: 0x04001DA4 RID: 7588
		private static SettingsPropertyCollection s_Properties = null;

		// Token: 0x04001DA5 RID: 7589
		private static object s_InitializeLock = new object();

		// Token: 0x04001DA6 RID: 7590
		private static Exception s_InitializeException = null;

		// Token: 0x04001DA7 RID: 7591
		private static bool s_Initialized = false;

		// Token: 0x04001DA8 RID: 7592
		private static ProfileBase s_SingletonInstance = null;

		// Token: 0x04001DA9 RID: 7593
		private static Hashtable s_PropertiesForCompilation = null;
	}
}
