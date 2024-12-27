using System;
using System.ComponentModel;
using System.Data.OracleClient;

namespace System.Data.Common
{
	// Token: 0x02000088 RID: 136
	internal class DbConnectionStringBuilderDescriptor : PropertyDescriptor
	{
		// Token: 0x060007C2 RID: 1986 RVA: 0x00072450 File Offset: 0x00071850
		internal DbConnectionStringBuilderDescriptor(string propertyName, Type componentType, Type propertyType, bool isReadOnly, Attribute[] attributes)
			: base(propertyName, attributes)
		{
			this._componentType = componentType;
			this._propertyType = propertyType;
			this._isReadOnly = isReadOnly;
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x0007247C File Offset: 0x0007187C
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x00072490 File Offset: 0x00071890
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

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060007C5 RID: 1989 RVA: 0x000724A4 File Offset: 0x000718A4
		public override Type ComponentType
		{
			get
			{
				return this._componentType;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x000724B8 File Offset: 0x000718B8
		public override bool IsReadOnly
		{
			get
			{
				return this._isReadOnly;
			}
		}

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060007C7 RID: 1991 RVA: 0x000724CC File Offset: 0x000718CC
		public override Type PropertyType
		{
			get
			{
				return this._propertyType;
			}
		}

		// Token: 0x060007C8 RID: 1992 RVA: 0x000724E0 File Offset: 0x000718E0
		public override bool CanResetValue(object component)
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = component as DbConnectionStringBuilder;
			return dbConnectionStringBuilder != null && dbConnectionStringBuilder.ShouldSerialize(this.DisplayName);
		}

		// Token: 0x060007C9 RID: 1993 RVA: 0x00072508 File Offset: 0x00071908
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

		// Token: 0x060007CA RID: 1994 RVA: 0x00072534 File Offset: 0x00071934
		public override void ResetValue(object component)
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = component as DbConnectionStringBuilder;
			if (dbConnectionStringBuilder != null)
			{
				dbConnectionStringBuilder.Remove(this.DisplayName);
				if (this.RefreshOnChange)
				{
					((OracleConnectionStringBuilder)dbConnectionStringBuilder).ClearPropertyDescriptors();
				}
			}
		}

		// Token: 0x060007CB RID: 1995 RVA: 0x0007256C File Offset: 0x0007196C
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
					((OracleConnectionStringBuilder)dbConnectionStringBuilder).ClearPropertyDescriptors();
				}
			}
		}

		// Token: 0x060007CC RID: 1996 RVA: 0x000725C8 File Offset: 0x000719C8
		public override bool ShouldSerializeValue(object component)
		{
			DbConnectionStringBuilder dbConnectionStringBuilder = component as DbConnectionStringBuilder;
			return dbConnectionStringBuilder != null && dbConnectionStringBuilder.ShouldSerialize(this.DisplayName);
		}

		// Token: 0x04000505 RID: 1285
		private Type _componentType;

		// Token: 0x04000506 RID: 1286
		private Type _propertyType;

		// Token: 0x04000507 RID: 1287
		private bool _isReadOnly;

		// Token: 0x04000508 RID: 1288
		private bool _refreshOnChange;
	}
}
