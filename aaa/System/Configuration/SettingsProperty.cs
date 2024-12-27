using System;

namespace System.Configuration
{
	// Token: 0x02000713 RID: 1811
	public class SettingsProperty
	{
		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06003755 RID: 14165 RVA: 0x000EB031 File Offset: 0x000EA031
		// (set) Token: 0x06003756 RID: 14166 RVA: 0x000EB039 File Offset: 0x000EA039
		public virtual string Name
		{
			get
			{
				return this._Name;
			}
			set
			{
				this._Name = value;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x06003757 RID: 14167 RVA: 0x000EB042 File Offset: 0x000EA042
		// (set) Token: 0x06003758 RID: 14168 RVA: 0x000EB04A File Offset: 0x000EA04A
		public virtual bool IsReadOnly
		{
			get
			{
				return this._IsReadOnly;
			}
			set
			{
				this._IsReadOnly = value;
			}
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x06003759 RID: 14169 RVA: 0x000EB053 File Offset: 0x000EA053
		// (set) Token: 0x0600375A RID: 14170 RVA: 0x000EB05B File Offset: 0x000EA05B
		public virtual object DefaultValue
		{
			get
			{
				return this._DefaultValue;
			}
			set
			{
				this._DefaultValue = value;
			}
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x0600375B RID: 14171 RVA: 0x000EB064 File Offset: 0x000EA064
		// (set) Token: 0x0600375C RID: 14172 RVA: 0x000EB06C File Offset: 0x000EA06C
		public virtual Type PropertyType
		{
			get
			{
				return this._PropertyType;
			}
			set
			{
				this._PropertyType = value;
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x0600375D RID: 14173 RVA: 0x000EB075 File Offset: 0x000EA075
		// (set) Token: 0x0600375E RID: 14174 RVA: 0x000EB07D File Offset: 0x000EA07D
		public virtual SettingsSerializeAs SerializeAs
		{
			get
			{
				return this._SerializeAs;
			}
			set
			{
				this._SerializeAs = value;
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x0600375F RID: 14175 RVA: 0x000EB086 File Offset: 0x000EA086
		// (set) Token: 0x06003760 RID: 14176 RVA: 0x000EB08E File Offset: 0x000EA08E
		public virtual SettingsProvider Provider
		{
			get
			{
				return this._Provider;
			}
			set
			{
				this._Provider = value;
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06003761 RID: 14177 RVA: 0x000EB097 File Offset: 0x000EA097
		public virtual SettingsAttributeDictionary Attributes
		{
			get
			{
				return this._Attributes;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x06003762 RID: 14178 RVA: 0x000EB09F File Offset: 0x000EA09F
		// (set) Token: 0x06003763 RID: 14179 RVA: 0x000EB0A7 File Offset: 0x000EA0A7
		public bool ThrowOnErrorDeserializing
		{
			get
			{
				return this._ThrowOnErrorDeserializing;
			}
			set
			{
				this._ThrowOnErrorDeserializing = value;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06003764 RID: 14180 RVA: 0x000EB0B0 File Offset: 0x000EA0B0
		// (set) Token: 0x06003765 RID: 14181 RVA: 0x000EB0B8 File Offset: 0x000EA0B8
		public bool ThrowOnErrorSerializing
		{
			get
			{
				return this._ThrowOnErrorSerializing;
			}
			set
			{
				this._ThrowOnErrorSerializing = value;
			}
		}

		// Token: 0x06003766 RID: 14182 RVA: 0x000EB0C1 File Offset: 0x000EA0C1
		public SettingsProperty(string name)
		{
			this._Name = name;
			this._Attributes = new SettingsAttributeDictionary();
		}

		// Token: 0x06003767 RID: 14183 RVA: 0x000EB0DC File Offset: 0x000EA0DC
		public SettingsProperty(string name, Type propertyType, SettingsProvider provider, bool isReadOnly, object defaultValue, SettingsSerializeAs serializeAs, SettingsAttributeDictionary attributes, bool throwOnErrorDeserializing, bool throwOnErrorSerializing)
		{
			this._Name = name;
			this._PropertyType = propertyType;
			this._Provider = provider;
			this._IsReadOnly = isReadOnly;
			this._DefaultValue = defaultValue;
			this._SerializeAs = serializeAs;
			this._Attributes = attributes;
			this._ThrowOnErrorDeserializing = throwOnErrorDeserializing;
			this._ThrowOnErrorSerializing = throwOnErrorSerializing;
		}

		// Token: 0x06003768 RID: 14184 RVA: 0x000EB134 File Offset: 0x000EA134
		public SettingsProperty(SettingsProperty propertyToCopy)
		{
			this._Name = propertyToCopy.Name;
			this._IsReadOnly = propertyToCopy.IsReadOnly;
			this._DefaultValue = propertyToCopy.DefaultValue;
			this._SerializeAs = propertyToCopy.SerializeAs;
			this._Provider = propertyToCopy.Provider;
			this._PropertyType = propertyToCopy.PropertyType;
			this._ThrowOnErrorDeserializing = propertyToCopy.ThrowOnErrorDeserializing;
			this._ThrowOnErrorSerializing = propertyToCopy.ThrowOnErrorSerializing;
			this._Attributes = new SettingsAttributeDictionary(propertyToCopy.Attributes);
		}

		// Token: 0x040031BF RID: 12735
		private string _Name;

		// Token: 0x040031C0 RID: 12736
		private bool _IsReadOnly;

		// Token: 0x040031C1 RID: 12737
		private object _DefaultValue;

		// Token: 0x040031C2 RID: 12738
		private SettingsSerializeAs _SerializeAs;

		// Token: 0x040031C3 RID: 12739
		private SettingsProvider _Provider;

		// Token: 0x040031C4 RID: 12740
		private SettingsAttributeDictionary _Attributes;

		// Token: 0x040031C5 RID: 12741
		private Type _PropertyType;

		// Token: 0x040031C6 RID: 12742
		private bool _ThrowOnErrorDeserializing;

		// Token: 0x040031C7 RID: 12743
		private bool _ThrowOnErrorSerializing;
	}
}
