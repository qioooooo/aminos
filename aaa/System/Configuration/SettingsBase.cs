using System;
using System.ComponentModel;

namespace System.Configuration
{
	// Token: 0x020006E0 RID: 1760
	public abstract class SettingsBase
	{
		// Token: 0x06003640 RID: 13888 RVA: 0x000E7998 File Offset: 0x000E6998
		protected SettingsBase()
		{
			this._PropertyValues = new SettingsPropertyValueCollection();
		}

		// Token: 0x17000C8B RID: 3211
		public virtual object this[string propertyName]
		{
			get
			{
				if (this.IsSynchronized)
				{
					lock (this)
					{
						return this.GetPropertyValueByName(propertyName);
					}
				}
				return this.GetPropertyValueByName(propertyName);
			}
			set
			{
				if (this.IsSynchronized)
				{
					lock (this)
					{
						this.SetPropertyValueByName(propertyName, value);
						return;
					}
				}
				this.SetPropertyValueByName(propertyName, value);
			}
		}

		// Token: 0x06003643 RID: 13891 RVA: 0x000E7A3C File Offset: 0x000E6A3C
		private object GetPropertyValueByName(string propertyName)
		{
			if (this.Properties == null || this._PropertyValues == null || this.Properties.Count == 0)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
			}
			SettingsProperty settingsProperty = this.Properties[propertyName];
			if (settingsProperty == null)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
			}
			SettingsPropertyValue settingsPropertyValue = this._PropertyValues[propertyName];
			if (settingsPropertyValue == null)
			{
				this.GetPropertiesFromProvider(settingsProperty.Provider);
				settingsPropertyValue = this._PropertyValues[propertyName];
				if (settingsPropertyValue == null)
				{
					throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
				}
			}
			return settingsPropertyValue.PropertyValue;
		}

		// Token: 0x06003644 RID: 13892 RVA: 0x000E7B00 File Offset: 0x000E6B00
		private void SetPropertyValueByName(string propertyName, object propertyValue)
		{
			if (this.Properties == null || this._PropertyValues == null || this.Properties.Count == 0)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
			}
			SettingsProperty settingsProperty = this.Properties[propertyName];
			if (settingsProperty == null)
			{
				throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
			}
			if (settingsProperty.IsReadOnly)
			{
				throw new SettingsPropertyIsReadOnlyException(SR.GetString("SettingsPropertyReadOnly", new object[] { propertyName }));
			}
			if (propertyValue != null && !settingsProperty.PropertyType.IsInstanceOfType(propertyValue))
			{
				throw new SettingsPropertyWrongTypeException(SR.GetString("SettingsPropertyWrongType", new object[] { propertyName }));
			}
			SettingsPropertyValue settingsPropertyValue = this._PropertyValues[propertyName];
			if (settingsPropertyValue == null)
			{
				this.GetPropertiesFromProvider(settingsProperty.Provider);
				settingsPropertyValue = this._PropertyValues[propertyName];
				if (settingsPropertyValue == null)
				{
					throw new SettingsPropertyNotFoundException(SR.GetString("SettingsPropertyNotFound", new object[] { propertyName }));
				}
			}
			settingsPropertyValue.PropertyValue = propertyValue;
		}

		// Token: 0x06003645 RID: 13893 RVA: 0x000E7C1B File Offset: 0x000E6C1B
		public void Initialize(SettingsContext context, SettingsPropertyCollection properties, SettingsProviderCollection providers)
		{
			this._Context = context;
			this._Properties = properties;
			this._Providers = providers;
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x000E7C34 File Offset: 0x000E6C34
		public virtual void Save()
		{
			if (this.IsSynchronized)
			{
				lock (this)
				{
					this.SaveCore();
					return;
				}
			}
			this.SaveCore();
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x000E7C78 File Offset: 0x000E6C78
		private void SaveCore()
		{
			if (this.Properties == null || this._PropertyValues == null || this.Properties.Count == 0)
			{
				return;
			}
			foreach (object obj in this.Providers)
			{
				SettingsProvider settingsProvider = (SettingsProvider)obj;
				SettingsPropertyValueCollection settingsPropertyValueCollection = new SettingsPropertyValueCollection();
				foreach (object obj2 in this.PropertyValues)
				{
					SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj2;
					if (settingsPropertyValue.Property.Provider == settingsProvider)
					{
						settingsPropertyValueCollection.Add(settingsPropertyValue);
					}
				}
				if (settingsPropertyValueCollection.Count > 0)
				{
					settingsProvider.SetPropertyValues(this.Context, settingsPropertyValueCollection);
				}
			}
			foreach (object obj3 in this.PropertyValues)
			{
				SettingsPropertyValue settingsPropertyValue2 = (SettingsPropertyValue)obj3;
				settingsPropertyValue2.IsDirty = false;
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x06003648 RID: 13896 RVA: 0x000E7DBC File Offset: 0x000E6DBC
		public virtual SettingsPropertyCollection Properties
		{
			get
			{
				return this._Properties;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06003649 RID: 13897 RVA: 0x000E7DC4 File Offset: 0x000E6DC4
		public virtual SettingsProviderCollection Providers
		{
			get
			{
				return this._Providers;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x0600364A RID: 13898 RVA: 0x000E7DCC File Offset: 0x000E6DCC
		public virtual SettingsPropertyValueCollection PropertyValues
		{
			get
			{
				return this._PropertyValues;
			}
		}

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x0600364B RID: 13899 RVA: 0x000E7DD4 File Offset: 0x000E6DD4
		public virtual SettingsContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x000E7DDC File Offset: 0x000E6DDC
		private void GetPropertiesFromProvider(SettingsProvider provider)
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
			if (settingsPropertyCollection.Count > 0)
			{
				SettingsPropertyValueCollection propertyValues = provider.GetPropertyValues(this.Context, settingsPropertyCollection);
				foreach (object obj2 in propertyValues)
				{
					SettingsPropertyValue settingsPropertyValue = (SettingsPropertyValue)obj2;
					if (this._PropertyValues[settingsPropertyValue.Name] == null)
					{
						this._PropertyValues.Add(settingsPropertyValue);
					}
				}
			}
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x000E7EC4 File Offset: 0x000E6EC4
		public static SettingsBase Synchronized(SettingsBase settingsBase)
		{
			settingsBase._IsSynchronized = true;
			return settingsBase;
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x0600364E RID: 13902 RVA: 0x000E7ECE File Offset: 0x000E6ECE
		[Browsable(false)]
		public bool IsSynchronized
		{
			get
			{
				return this._IsSynchronized;
			}
		}

		// Token: 0x04003170 RID: 12656
		private SettingsPropertyCollection _Properties;

		// Token: 0x04003171 RID: 12657
		private SettingsProviderCollection _Providers;

		// Token: 0x04003172 RID: 12658
		private SettingsPropertyValueCollection _PropertyValues;

		// Token: 0x04003173 RID: 12659
		private SettingsContext _Context;

		// Token: 0x04003174 RID: 12660
		private bool _IsSynchronized;
	}
}
