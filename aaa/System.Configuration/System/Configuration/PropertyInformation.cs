using System;
using System.ComponentModel;

namespace System.Configuration
{
	// Token: 0x02000083 RID: 131
	public sealed class PropertyInformation
	{
		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060004DE RID: 1246 RVA: 0x00018F5F File Offset: 0x00017F5F
		private ConfigurationProperty Prop
		{
			get
			{
				if (this._Prop == null)
				{
					this._Prop = this.ThisElement.Properties[this.PropertyName];
				}
				return this._Prop;
			}
		}

		// Token: 0x060004DF RID: 1247 RVA: 0x00018F8B File Offset: 0x00017F8B
		internal PropertyInformation(ConfigurationElement thisElement, string propertyName)
		{
			this.PropertyName = propertyName;
			this.ThisElement = thisElement;
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060004E0 RID: 1248 RVA: 0x00018FA1 File Offset: 0x00017FA1
		public string Name
		{
			get
			{
				return this.PropertyName;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x00018FA9 File Offset: 0x00017FA9
		internal string ProvidedName
		{
			get
			{
				return this.Prop.ProvidedName;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060004E2 RID: 1250 RVA: 0x00018FB6 File Offset: 0x00017FB6
		// (set) Token: 0x060004E3 RID: 1251 RVA: 0x00018FC9 File Offset: 0x00017FC9
		public object Value
		{
			get
			{
				return this.ThisElement[this.PropertyName];
			}
			set
			{
				this.ThisElement[this.PropertyName] = value;
			}
		}

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060004E4 RID: 1252 RVA: 0x00018FDD File Offset: 0x00017FDD
		public object DefaultValue
		{
			get
			{
				return this.Prop.DefaultValue;
			}
		}

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060004E5 RID: 1253 RVA: 0x00018FEA File Offset: 0x00017FEA
		public PropertyValueOrigin ValueOrigin
		{
			get
			{
				if (this.ThisElement.Values[this.PropertyName] == null)
				{
					return PropertyValueOrigin.Default;
				}
				if (this.ThisElement.Values.IsInherited(this.PropertyName))
				{
					return PropertyValueOrigin.Inherited;
				}
				return PropertyValueOrigin.SetHere;
			}
		}

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060004E6 RID: 1254 RVA: 0x00019021 File Offset: 0x00018021
		public bool IsModified
		{
			get
			{
				return this.ThisElement.Values[this.PropertyName] != null && this.ThisElement.Values.IsModified(this.PropertyName);
			}
		}

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00019058 File Offset: 0x00018058
		public bool IsKey
		{
			get
			{
				return this.Prop.IsKey;
			}
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00019065 File Offset: 0x00018065
		public bool IsRequired
		{
			get
			{
				return this.Prop.IsRequired;
			}
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00019074 File Offset: 0x00018074
		public bool IsLocked
		{
			get
			{
				return (this.ThisElement.LockedAllExceptAttributesList != null && !this.ThisElement.LockedAllExceptAttributesList.DefinedInParent(this.PropertyName)) || (this.ThisElement.LockedAttributesList != null && (this.ThisElement.LockedAttributesList.DefinedInParent(this.PropertyName) || this.ThisElement.LockedAttributesList.DefinedInParent("*"))) || ((this.ThisElement.ItemLocked & ConfigurationValueFlags.Locked) != ConfigurationValueFlags.Default && (this.ThisElement.ItemLocked & ConfigurationValueFlags.Inherited) != ConfigurationValueFlags.Default);
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x060004EA RID: 1258 RVA: 0x00019108 File Offset: 0x00018108
		public string Source
		{
			get
			{
				PropertySourceInfo propertySourceInfo = this.ThisElement.Values.GetSourceInfo(this.PropertyName);
				if (propertySourceInfo == null)
				{
					propertySourceInfo = this.ThisElement.Values.GetSourceInfo(string.Empty);
				}
				if (propertySourceInfo == null)
				{
					return string.Empty;
				}
				return propertySourceInfo.FileName;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x060004EB RID: 1259 RVA: 0x00019154 File Offset: 0x00018154
		public int LineNumber
		{
			get
			{
				PropertySourceInfo propertySourceInfo = this.ThisElement.Values.GetSourceInfo(this.PropertyName);
				if (propertySourceInfo == null)
				{
					propertySourceInfo = this.ThisElement.Values.GetSourceInfo(string.Empty);
				}
				if (propertySourceInfo == null)
				{
					return 0;
				}
				return propertySourceInfo.LineNumber;
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x060004EC RID: 1260 RVA: 0x0001919C File Offset: 0x0001819C
		public Type Type
		{
			get
			{
				return this.Prop.Type;
			}
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x060004ED RID: 1261 RVA: 0x000191A9 File Offset: 0x000181A9
		public ConfigurationValidatorBase Validator
		{
			get
			{
				return this.Prop.Validator;
			}
		}

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x060004EE RID: 1262 RVA: 0x000191B6 File Offset: 0x000181B6
		public TypeConverter Converter
		{
			get
			{
				return this.Prop.Converter;
			}
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x060004EF RID: 1263 RVA: 0x000191C3 File Offset: 0x000181C3
		public string Description
		{
			get
			{
				return this.Prop.Description;
			}
		}

		// Token: 0x0400035E RID: 862
		private const string LockAll = "*";

		// Token: 0x0400035F RID: 863
		private ConfigurationElement ThisElement;

		// Token: 0x04000360 RID: 864
		private string PropertyName;

		// Token: 0x04000361 RID: 865
		private ConfigurationProperty _Prop;
	}
}
