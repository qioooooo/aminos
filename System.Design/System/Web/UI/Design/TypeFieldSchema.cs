using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal sealed class TypeFieldSchema : IDataSourceFieldSchema
	{
		public TypeFieldSchema(PropertyDescriptor fieldDescriptor)
		{
			if (fieldDescriptor == null)
			{
				throw new ArgumentNullException("fieldDescriptor");
			}
			this._fieldDescriptor = fieldDescriptor;
		}

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

		public bool Identity
		{
			get
			{
				this.EnsureMetaData();
				return this._isIdentity;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this._fieldDescriptor.IsReadOnly;
			}
		}

		public bool IsUnique
		{
			get
			{
				return false;
			}
		}

		public int Length
		{
			get
			{
				this.EnsureMetaData();
				return this._length;
			}
		}

		public string Name
		{
			get
			{
				return this._fieldDescriptor.Name;
			}
		}

		public bool Nullable
		{
			get
			{
				this.EnsureMetaData();
				Type propertyType = this._fieldDescriptor.PropertyType;
				return !propertyType.IsValueType || this._isNullable || (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>));
			}
		}

		public int Precision
		{
			get
			{
				return -1;
			}
		}

		public bool PrimaryKey
		{
			get
			{
				this.EnsureMetaData();
				return this._primaryKey;
			}
		}

		public int Scale
		{
			get
			{
				return -1;
			}
		}

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

		private PropertyDescriptor _fieldDescriptor;

		private bool _retrievedMetaData;

		private bool _primaryKey;

		private bool _isIdentity;

		private bool _isNullable;

		private int _length = -1;
	}
}
