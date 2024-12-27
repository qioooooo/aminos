using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003A1 RID: 929
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class TypeFieldSchema : IDataSourceFieldSchema
	{
		// Token: 0x0600224D RID: 8781 RVA: 0x000BBC22 File Offset: 0x000BAC22
		public TypeFieldSchema(PropertyDescriptor fieldDescriptor)
		{
			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException("fieldDescriptor");
			}
			this._fieldDescriptor = fieldDescriptor;
		}

		// Token: 0x1700064F RID: 1615
		// (get) Token: 0x0600224E RID: 8782 RVA: 0x000BBC48 File Offset: 0x000BAC48
		public Type DataType
		{
			get
			{
				Type propertyType = this._fieldDescriptor.PropertyType;
				if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					return propertyType.GetGenericArguments()[0];
				}
				return propertyType;
			}
		}

		// Token: 0x17000650 RID: 1616
		// (get) Token: 0x0600224F RID: 8783 RVA: 0x000BBC85 File Offset: 0x000BAC85
		public bool Identity
		{
			get
			{
				this.EnsureMetaData();
				return this._isIdentity;
			}
		}

		// Token: 0x17000651 RID: 1617
		// (get) Token: 0x06002250 RID: 8784 RVA: 0x000BBC93 File Offset: 0x000BAC93
		public bool IsReadOnly
		{
			get
			{
				return this._fieldDescriptor.IsReadOnly;
			}
		}

		// Token: 0x17000652 RID: 1618
		// (get) Token: 0x06002251 RID: 8785 RVA: 0x000BBCA0 File Offset: 0x000BACA0
		public bool IsUnique
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000653 RID: 1619
		// (get) Token: 0x06002252 RID: 8786 RVA: 0x000BBCA3 File Offset: 0x000BACA3
		public int Length
		{
			get
			{
				this.EnsureMetaData();
				return this._length;
			}
		}

		// Token: 0x17000654 RID: 1620
		// (get) Token: 0x06002253 RID: 8787 RVA: 0x000BBCB1 File Offset: 0x000BACB1
		public string Name
		{
			get
			{
				return this._fieldDescriptor.Name;
			}
		}

		// Token: 0x17000655 RID: 1621
		// (get) Token: 0x06002254 RID: 8788 RVA: 0x000BBCC0 File Offset: 0x000BACC0
		public bool Nullable
		{
			get
			{
				this.EnsureMetaData();
				Type propertyType = this._fieldDescriptor.PropertyType;
				return !propertyType.IsValueType || this._isNullable || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>));
			}
		}

		// Token: 0x17000656 RID: 1622
		// (get) Token: 0x06002255 RID: 8789 RVA: 0x000BBD0D File Offset: 0x000BAD0D
		public int Precision
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x17000657 RID: 1623
		// (get) Token: 0x06002256 RID: 8790 RVA: 0x000BBD10 File Offset: 0x000BAD10
		public bool PrimaryKey
		{
			get
			{
				this.EnsureMetaData();
				return this._primaryKey;
			}
		}

		// Token: 0x17000658 RID: 1624
		// (get) Token: 0x06002257 RID: 8791 RVA: 0x000BBD1E File Offset: 0x000BAD1E
		public int Scale
		{
			get
			{
				return -1;
			}
		}

		// Token: 0x06002258 RID: 8792 RVA: 0x000BBD24 File Offset: 0x000BAD24
		private void EnsureMetaData()
		{
			if (this._retrievedMetaData)
			{
				return;
			}
			DataObjectFieldAttribute dataObjectFieldAttribute = (DataObjectFieldAttribute)this._fieldDescriptor.Attributes[typeof(DataObjectFieldAttribute)];
			if (dataObjectFieldAttribute != null)
			{
				this._primaryKey = dataObjectFieldAttribute.PrimaryKey;
				this._isIdentity = dataObjectFieldAttribute.IsIdentity;
				this._isNullable = dataObjectFieldAttribute.IsNullable;
				this._length = dataObjectFieldAttribute.Length;
			}
			this._retrievedMetaData = true;
		}

		// Token: 0x04001849 RID: 6217
		private PropertyDescriptor _fieldDescriptor;

		// Token: 0x0400184A RID: 6218
		private bool _retrievedMetaData;

		// Token: 0x0400184B RID: 6219
		private bool _primaryKey;

		// Token: 0x0400184C RID: 6220
		private bool _isIdentity;

		// Token: 0x0400184D RID: 6221
		private bool _isNullable;

		// Token: 0x0400184E RID: 6222
		private int _length = -1;
	}
}
