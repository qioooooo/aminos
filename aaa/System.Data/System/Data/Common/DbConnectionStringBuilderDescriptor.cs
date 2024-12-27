using System;
using System.ComponentModel;

namespace System.Data.Common
{
	// Token: 0x0200012E RID: 302
	internal class DbConnectionStringBuilderDescriptor : PropertyDescriptor
	{
		// Token: 0x060013F5 RID: 5109 RVA: 0x00224D14 File Offset: 0x00224114
		internal DbConnectionStringBuilderDescriptor(string propertyName, Type componentType, Type propertyType, bool isReadOnly, Attribute[] attributes)
			: base(propertyName, attributes)
		{
			this._componentType = componentType;
			this._propertyType = propertyType;
			this._isReadOnly = isReadOnly;
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x060013F6 RID: 5110 RVA: 0x00224D40 File Offset: 0x00224140
		// (set) Token: 0x060013F7 RID: 5111 RVA: 0x00224D54 File Offset: 0x00224154
		internal bool RefreshOnChange
		{
			get
			{
				return this._refreshOnChange;
			}
			set
			{
				this._refreshOnChange = value;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x060013F8 RID: 5112 RVA: 0x00224D68 File Offset: 0x00224168
		public override Type ComponentType
		{
			get
			{
				return this._componentType;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x060013F9 RID: 5113 RVA: 0x00224D7C File Offset: 0x0022417C
		public override bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x060013FA RID: 5114 RVA: 0x00224D90 File Offset: 0x00224190
		public override Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		// Token: 0x060013FB RID: 5115 RVA: 0x00224DA4 File Offset: 0x002241A4
		public override bool CanResetValue(object component)
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = component as DbConnectionStringBuilder;
			return dbConnectionStringBuilder != null && dbConnectionStringBuilder.ShouldSerialize(this.DisplayName);
		}

		// Token: 0x060013FC RID: 5116 RVA: 0x00224DCC File Offset: 0x002241CC
		public override object GetValue(object component)
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = component as DbConnectionStringBuilder;
			object obj;
			if (dbConnectionStringBuilder != null && dbConnectionStringBuilder.TryGetValue(this.DisplayName, out obj))
			{
				return obj;
			}
			return null;
		}

		// Token: 0x060013FD RID: 5117 RVA: 0x00224DF8 File Offset: 0x002241F8
		public override void ResetValue(object component)
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = component as DbConnectionStringBuilder;
			if (dbConnectionStringBuilder != null)
			{
				dbConnectionStringBuilder.Remove(this.DisplayName);
				if (this.RefreshOnChange)
				{
					dbConnectionStringBuilder.ClearPropertyDescriptors();
				}
			}
		}

		// Token: 0x060013FE RID: 5118 RVA: 0x00224E2C File Offset: 0x0022422C
		public override void SetValue(object component, object value)
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = component as DbConnectionStringBuilder;
			if (dbConnectionStringBuilder != null)
			{
				if (typeof(string) == this.PropertyType && string.Empty.Equals(value))
				{
					value = null;
				}
				dbConnectionStringBuilder[this.DisplayName] = value;
				if (this.RefreshOnChange)
				{
					dbConnectionStringBuilder.ClearPropertyDescriptors();
				}
			}
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x00224E80 File Offset: 0x00224280
		public override bool ShouldSerializeValue(object component)
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = component as DbConnectionStringBuilder;
			return dbConnectionStringBuilder != null && dbConnectionStringBuilder.ShouldSerialize(this.DisplayName);
		}

		// Token: 0x04000C24 RID: 3108
		private Type _componentType;

		// Token: 0x04000C25 RID: 3109
		private Type _propertyType;

		// Token: 0x04000C26 RID: 3110
		private bool _isReadOnly;

		// Token: 0x04000C27 RID: 3111
		private bool _refreshOnChange;
	}
}
