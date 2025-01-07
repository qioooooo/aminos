using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Data.Design
{
	internal abstract class DataSourceComponent : Component, ICustomTypeDescriptor, IObjectWithParent, IDataSourceCollectionMember, IDataSourceRenamableObject
	{
		protected internal virtual DataSourceCollectionBase CollectionParent
		{
			get
			{
				return this.collectionParent;
			}
			set
			{
				this.collectionParent = value;
			}
		}

		protected virtual object ExternalPropertyHost
		{
			get
			{
				return null;
			}
		}

		[Browsable(false)]
		public virtual object Parent
		{
			get
			{
				return this.collectionParent;
			}
		}

		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return TypeDescriptor.GetAttributes(base.GetType());
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			if (this is IDataSourceNamedObject)
			{
				return ((IDataSourceNamedObject)this).PublicTypeName;
			}
			return null;
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			INamedObject namedObject = this as INamedObject;
			if (namedObject == null)
			{
				return null;
			}
			return namedObject.Name;
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return TypeDescriptor.GetConverter(base.GetType());
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(base.GetType());
		}

		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(base.GetType());
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(base.GetType(), editorBaseType);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(base.GetType());
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(base.GetType(), attributes);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return this.GetProperties(null);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return this.GetProperties(attributes);
		}

		private PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(base.GetType(), attributes);
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		protected override object GetService(Type service)
		{
			DataSourceComponent dataSourceComponent = this;
			while (dataSourceComponent != null && dataSourceComponent.Site == null)
			{
				if (dataSourceComponent.CollectionParent != null)
				{
					dataSourceComponent = dataSourceComponent.CollectionParent.CollectionHost;
				}
				else if (dataSourceComponent.Parent != null && dataSourceComponent.Parent is DataSourceComponent)
				{
					dataSourceComponent = dataSourceComponent.Parent as DataSourceComponent;
				}
				else
				{
					dataSourceComponent = null;
				}
			}
			if (dataSourceComponent != null && dataSourceComponent.Site != null)
			{
				return dataSourceComponent.Site.GetService(service);
			}
			return null;
		}

		public virtual void SetCollection(DataSourceCollectionBase collection)
		{
			this.CollectionParent = collection;
		}

		internal void SetPropertyValue(string propertyName, object value)
		{
		}

		internal virtual StringCollection NamingPropertyNames
		{
			get
			{
				return null;
			}
		}

		[Browsable(false)]
		public virtual string GeneratorName
		{
			get
			{
				return null;
			}
		}

		private DataSourceCollectionBase collectionParent;
	}
}
