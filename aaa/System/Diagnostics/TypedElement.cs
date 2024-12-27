using System;
using System.Configuration;

namespace System.Diagnostics
{
	// Token: 0x020001C3 RID: 451
	internal class TypedElement : ConfigurationElement
	{
		// Token: 0x06000E1A RID: 3610 RVA: 0x0002CF58 File Offset: 0x0002BF58
		public TypedElement(Type baseType)
		{
			this._properties = new ConfigurationPropertyCollection();
			this._properties.Add(TypedElement._propTypeName);
			this._properties.Add(TypedElement._propInitData);
			this._baseType = baseType;
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000E1B RID: 3611 RVA: 0x0002CF92 File Offset: 0x0002BF92
		// (set) Token: 0x06000E1C RID: 3612 RVA: 0x0002CFA4 File Offset: 0x0002BFA4
		[ConfigurationProperty("initializeData", DefaultValue = "")]
		public string InitData
		{
			get
			{
				return (string)base[TypedElement._propInitData];
			}
			set
			{
				base[TypedElement._propInitData] = value;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000E1D RID: 3613 RVA: 0x0002CFB2 File Offset: 0x0002BFB2
		protected override ConfigurationPropertyCollection Properties
		{
			get
			{
				return this._properties;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000E1E RID: 3614 RVA: 0x0002CFBA File Offset: 0x0002BFBA
		// (set) Token: 0x06000E1F RID: 3615 RVA: 0x0002CFCC File Offset: 0x0002BFCC
		[ConfigurationProperty("type", IsRequired = true, DefaultValue = "")]
		public virtual string TypeName
		{
			get
			{
				return (string)base[TypedElement._propTypeName];
			}
			set
			{
				base[TypedElement._propTypeName] = value;
			}
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x0002CFDA File Offset: 0x0002BFDA
		protected object BaseGetRuntimeObject()
		{
			if (this._runtimeObject == null)
			{
				this._runtimeObject = TraceUtils.GetRuntimeObject(this.TypeName, this._baseType, this.InitData);
			}
			return this._runtimeObject;
		}

		// Token: 0x04000EE3 RID: 3811
		protected static readonly ConfigurationProperty _propTypeName = new ConfigurationProperty("type", typeof(string), string.Empty, ConfigurationPropertyOptions.IsRequired);

		// Token: 0x04000EE4 RID: 3812
		protected static readonly ConfigurationProperty _propInitData = new ConfigurationProperty("initializeData", typeof(string), string.Empty, ConfigurationPropertyOptions.None);

		// Token: 0x04000EE5 RID: 3813
		protected ConfigurationPropertyCollection _properties;

		// Token: 0x04000EE6 RID: 3814
		protected object _runtimeObject;

		// Token: 0x04000EE7 RID: 3815
		private Type _baseType;
	}
}
