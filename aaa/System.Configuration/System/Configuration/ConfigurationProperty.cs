using System;
using System.ComponentModel;
using System.Reflection;

namespace System.Configuration
{
	// Token: 0x02000037 RID: 55
	public sealed class ConfigurationProperty
	{
		// Token: 0x06000293 RID: 659 RVA: 0x000106A0 File Offset: 0x0000F6A0
		public ConfigurationProperty(string name, Type type)
		{
			object obj = null;
			this.ConstructorInit(name, type, ConfigurationPropertyOptions.None, null, null);
			if (type == typeof(string))
			{
				obj = string.Empty;
			}
			else if (type.IsValueType)
			{
				obj = TypeUtil.CreateInstanceWithReflectionPermission(type);
			}
			this.SetDefaultValue(obj);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x000106EB File Offset: 0x0000F6EB
		public ConfigurationProperty(string name, Type type, object defaultValue)
			: this(name, type, defaultValue, ConfigurationPropertyOptions.None)
		{
		}

		// Token: 0x06000295 RID: 661 RVA: 0x000106F7 File Offset: 0x0000F6F7
		public ConfigurationProperty(string name, Type type, object defaultValue, ConfigurationPropertyOptions options)
			: this(name, type, defaultValue, null, null, options)
		{
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00010706 File Offset: 0x0000F706
		public ConfigurationProperty(string name, Type type, object defaultValue, TypeConverter typeConverter, ConfigurationValidatorBase validator, ConfigurationPropertyOptions options)
			: this(name, type, defaultValue, typeConverter, validator, options, null)
		{
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00010718 File Offset: 0x0000F718
		public ConfigurationProperty(string name, Type type, object defaultValue, TypeConverter typeConverter, ConfigurationValidatorBase validator, ConfigurationPropertyOptions options, string description)
		{
			this.ConstructorInit(name, type, options, validator, typeConverter);
			this.SetDefaultValue(defaultValue);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00010738 File Offset: 0x0000F738
		internal ConfigurationProperty(PropertyInfo info)
		{
			ConfigurationPropertyAttribute configurationPropertyAttribute = null;
			DescriptionAttribute descriptionAttribute = null;
			DefaultValueAttribute defaultValueAttribute = null;
			TypeConverter typeConverter = null;
			ConfigurationValidatorBase configurationValidatorBase = null;
			foreach (Attribute attribute in Attribute.GetCustomAttributes(info))
			{
				if (attribute is TypeConverterAttribute)
				{
					TypeConverterAttribute typeConverterAttribute = (TypeConverterAttribute)attribute;
					typeConverter = TypeUtil.CreateInstanceRestricted<TypeConverter>(info.DeclaringType, typeConverterAttribute.ConverterTypeName);
				}
				else if (attribute is ConfigurationPropertyAttribute)
				{
					configurationPropertyAttribute = (ConfigurationPropertyAttribute)attribute;
				}
				else if (attribute is ConfigurationValidatorAttribute)
				{
					if (configurationValidatorBase != null)
					{
						throw new ConfigurationErrorsException(SR.GetString("Validator_multiple_validator_attributes", new object[] { info.Name }));
					}
					ConfigurationValidatorAttribute configurationValidatorAttribute = (ConfigurationValidatorAttribute)attribute;
					configurationValidatorAttribute.SetDeclaringType(info.DeclaringType);
					configurationValidatorBase = configurationValidatorAttribute.ValidatorInstance;
				}
				else if (attribute is DescriptionAttribute)
				{
					descriptionAttribute = (DescriptionAttribute)attribute;
				}
				else if (attribute is DefaultValueAttribute)
				{
					defaultValueAttribute = (DefaultValueAttribute)attribute;
				}
			}
			Type propertyType = info.PropertyType;
			if (typeof(ConfigurationElementCollection).IsAssignableFrom(propertyType))
			{
				ConfigurationCollectionAttribute configurationCollectionAttribute = Attribute.GetCustomAttribute(info, typeof(ConfigurationCollectionAttribute)) as ConfigurationCollectionAttribute;
				if (configurationCollectionAttribute == null)
				{
					configurationCollectionAttribute = Attribute.GetCustomAttribute(propertyType, typeof(ConfigurationCollectionAttribute)) as ConfigurationCollectionAttribute;
				}
				if (configurationCollectionAttribute != null)
				{
					if (configurationCollectionAttribute.AddItemName.IndexOf(',') == -1)
					{
						this._addElementName = configurationCollectionAttribute.AddItemName;
					}
					this._removeElementName = configurationCollectionAttribute.RemoveItemName;
					this._clearElementName = configurationCollectionAttribute.ClearItemsName;
				}
			}
			this.ConstructorInit(configurationPropertyAttribute.Name, info.PropertyType, configurationPropertyAttribute.Options, configurationValidatorBase, typeConverter);
			this.InitDefaultValueFromTypeInfo(configurationPropertyAttribute, defaultValueAttribute);
			if (descriptionAttribute != null && !string.IsNullOrEmpty(descriptionAttribute.Description))
			{
				this._description = descriptionAttribute.Description;
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00010900 File Offset: 0x0000F900
		private void ConstructorInit(string name, Type type, ConfigurationPropertyOptions options, ConfigurationValidatorBase validator, TypeConverter converter)
		{
			if (typeof(ConfigurationSection).IsAssignableFrom(type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Config_properties_may_not_be_derived_from_configuration_section", new object[] { name }));
			}
			this._providedName = name;
			if ((options & ConfigurationPropertyOptions.IsDefaultCollection) != ConfigurationPropertyOptions.None && string.IsNullOrEmpty(name))
			{
				name = ConfigurationProperty.DefaultCollectionPropertyName;
			}
			else
			{
				this.ValidatePropertyName(name);
			}
			this._name = name;
			this._type = type;
			this._options = options;
			this._validator = validator;
			this._converter = converter;
			if (this._validator == null)
			{
				this._validator = ConfigurationProperty.DefaultValidatorInstance;
				return;
			}
			if (!this._validator.CanValidate(this._type))
			{
				throw new ConfigurationErrorsException(SR.GetString("Validator_does_not_support_prop_type", new object[] { this._name }));
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x000109CC File Offset: 0x0000F9CC
		private void ValidatePropertyName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(SR.GetString("String_null_or_empty"), "name");
			}
			if (BaseConfigurationRecord.IsReservedAttributeName(name))
			{
				throw new ArgumentException(SR.GetString("Property_name_reserved", new object[] { name }));
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x00010A1C File Offset: 0x0000FA1C
		private void SetDefaultValue(object value)
		{
			if (value != null && value != ConfigurationElement.s_nullPropertyValue)
			{
				bool flag = this._type.IsAssignableFrom(value.GetType());
				if (!flag && this.Converter.CanConvertFrom(value.GetType()))
				{
					value = this.Converter.ConvertFrom(value);
				}
				else if (!flag)
				{
					throw new ConfigurationErrorsException(SR.GetString("Default_value_wrong_type", new object[] { this._name }));
				}
				this.Validate(value);
				this._defaultValue = value;
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x00010AA0 File Offset: 0x0000FAA0
		private void InitDefaultValueFromTypeInfo(ConfigurationPropertyAttribute attribProperty, DefaultValueAttribute attribStdDefault)
		{
			object obj = attribProperty.DefaultValue;
			if ((obj == null || obj == ConfigurationElement.s_nullPropertyValue) && attribStdDefault != null)
			{
				obj = attribStdDefault.Value;
			}
			if (obj != null && obj is string && this._type != typeof(string))
			{
				try
				{
					obj = this.Converter.ConvertFromInvariantString((string)obj);
				}
				catch (Exception ex)
				{
					throw new ConfigurationErrorsException(SR.GetString("Default_value_conversion_error_from_string", new object[] { this._name, ex.Message }));
				}
				catch
				{
					throw new ConfigurationErrorsException(SR.GetString("Default_value_conversion_error_from_string", new object[]
					{
						this._name,
						ExceptionUtil.NoExceptionInformation
					}));
				}
			}
			if (obj == null || obj == ConfigurationElement.s_nullPropertyValue)
			{
				if (this._type == typeof(string))
				{
					obj = string.Empty;
				}
				else if (this._type.IsValueType)
				{
					obj = TypeUtil.CreateInstanceWithReflectionPermission(this._type);
				}
			}
			this.SetDefaultValue(obj);
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600029D RID: 669 RVA: 0x00010BB4 File Offset: 0x0000FBB4
		public string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600029E RID: 670 RVA: 0x00010BBC File Offset: 0x0000FBBC
		public string Description
		{
			get
			{
				return this._description;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600029F RID: 671 RVA: 0x00010BC4 File Offset: 0x0000FBC4
		internal string ProvidedName
		{
			get
			{
				return this._providedName;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x00010BCC File Offset: 0x0000FBCC
		public Type Type
		{
			get
			{
				return this._type;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x00010BD4 File Offset: 0x0000FBD4
		public object DefaultValue
		{
			get
			{
				return this._defaultValue;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060002A2 RID: 674 RVA: 0x00010BDC File Offset: 0x0000FBDC
		public bool IsRequired
		{
			get
			{
				return (this._options & ConfigurationPropertyOptions.IsRequired) != ConfigurationPropertyOptions.None;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060002A3 RID: 675 RVA: 0x00010BEC File Offset: 0x0000FBEC
		public bool IsKey
		{
			get
			{
				return (this._options & ConfigurationPropertyOptions.IsKey) != ConfigurationPropertyOptions.None;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060002A4 RID: 676 RVA: 0x00010BFC File Offset: 0x0000FBFC
		public bool IsDefaultCollection
		{
			get
			{
				return (this._options & ConfigurationPropertyOptions.IsDefaultCollection) != ConfigurationPropertyOptions.None;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x00010C0C File Offset: 0x0000FC0C
		public TypeConverter Converter
		{
			get
			{
				this.CreateConverter();
				return this._converter;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x00010C1A File Offset: 0x0000FC1A
		public ConfigurationValidatorBase Validator
		{
			get
			{
				return this._validator;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x00010C22 File Offset: 0x0000FC22
		internal string AddElementName
		{
			get
			{
				return this._addElementName;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x060002A8 RID: 680 RVA: 0x00010C2A File Offset: 0x0000FC2A
		internal string RemoveElementName
		{
			get
			{
				return this._removeElementName;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x060002A9 RID: 681 RVA: 0x00010C32 File Offset: 0x0000FC32
		internal string ClearElementName
		{
			get
			{
				return this._clearElementName;
			}
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00010C3C File Offset: 0x0000FC3C
		internal object ConvertFromString(string value)
		{
			object obj = null;
			try
			{
				obj = this.Converter.ConvertFromInvariantString(value);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Top_level_conversion_error_from_string", new object[] { this._name, ex.Message }));
			}
			catch
			{
				throw new ConfigurationErrorsException(SR.GetString("Top_level_conversion_error_from_string", new object[]
				{
					this._name,
					ExceptionUtil.NoExceptionInformation
				}));
			}
			return obj;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00010CCC File Offset: 0x0000FCCC
		internal string ConvertToString(object value)
		{
			string text = null;
			try
			{
				if (this._type == typeof(bool))
				{
					text = (((bool)value) ? "true" : "false");
				}
				else
				{
					text = this.Converter.ConvertToInvariantString(value);
				}
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Top_level_conversion_error_to_string", new object[] { this._name, ex.Message }));
			}
			catch
			{
				throw new ConfigurationErrorsException(SR.GetString("Top_level_conversion_error_to_string", new object[]
				{
					this._name,
					ExceptionUtil.NoExceptionInformation
				}));
			}
			return text;
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00010D88 File Offset: 0x0000FD88
		internal void Validate(object value)
		{
			try
			{
				this._validator.Validate(value);
			}
			catch (Exception ex)
			{
				throw new ConfigurationErrorsException(SR.GetString("Top_level_validation_error", new object[] { this._name, ex.Message }), ex);
			}
			catch
			{
				throw new ConfigurationErrorsException(SR.GetString("Top_level_validation_error", new object[]
				{
					this._name,
					ExceptionUtil.NoExceptionInformation
				}));
			}
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00010E18 File Offset: 0x0000FE18
		private void CreateConverter()
		{
			if (this._converter == null)
			{
				if (this._type.IsEnum)
				{
					this._converter = new GenericEnumConverter(this._type);
					return;
				}
				if (!this._type.IsSubclassOf(typeof(ConfigurationElement)))
				{
					this._converter = TypeDescriptor.GetConverter(this._type);
					if (this._converter == null || !this._converter.CanConvertFrom(typeof(string)) || !this._converter.CanConvertTo(typeof(string)))
					{
						throw new ConfigurationErrorsException(SR.GetString("No_converter", new object[]
						{
							this._name,
							this._type.Name
						}));
					}
				}
			}
		}

		// Token: 0x04000277 RID: 631
		internal static readonly ConfigurationValidatorBase NonEmptyStringValidator = new StringValidator(1);

		// Token: 0x04000278 RID: 632
		private static readonly ConfigurationValidatorBase DefaultValidatorInstance = new DefaultValidator();

		// Token: 0x04000279 RID: 633
		internal static readonly string DefaultCollectionPropertyName = "";

		// Token: 0x0400027A RID: 634
		private string _name;

		// Token: 0x0400027B RID: 635
		private string _providedName;

		// Token: 0x0400027C RID: 636
		private string _description;

		// Token: 0x0400027D RID: 637
		private Type _type;

		// Token: 0x0400027E RID: 638
		private object _defaultValue;

		// Token: 0x0400027F RID: 639
		private TypeConverter _converter;

		// Token: 0x04000280 RID: 640
		private ConfigurationPropertyOptions _options;

		// Token: 0x04000281 RID: 641
		private ConfigurationValidatorBase _validator;

		// Token: 0x04000282 RID: 642
		private string _addElementName;

		// Token: 0x04000283 RID: 643
		private string _removeElementName;

		// Token: 0x04000284 RID: 644
		private string _clearElementName;
	}
}
