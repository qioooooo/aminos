using System;
using System.Collections.Specialized;
using System.ComponentModel;

namespace System.Data.Design
{
	// Token: 0x0200006D RID: 109
	internal abstract class DataSourceComponent : Component, ICustomTypeDescriptor, IObjectWithParent, IDataSourceCollectionMember, IDataSourceRenamableObject
	{
		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x00003EC2 File Offset: 0x00002EC2
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x00003ECA File Offset: 0x00002ECA
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

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x00003ED3 File Offset: 0x00002ED3
		protected virtual object ExternalPropertyHost
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x060004A2 RID: 1186 RVA: 0x00003ED6 File Offset: 0x00002ED6
		[Browsable(false)]
		public virtual object Parent
		{
			get
			{
				return this.collectionParent;
			}
		}

		// Token: 0x060004A3 RID: 1187 RVA: 0x00003EDE File Offset: 0x00002EDE
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return TypeDescriptor.GetAttributes(base.GetType());
		}

		// Token: 0x060004A4 RID: 1188 RVA: 0x00003EEB File Offset: 0x00002EEB
		string ICustomTypeDescriptor.GetClassName()
		{
			if (this is IDataSourceNamedObject)
			{
				return ((IDataSourceNamedObject)this).PublicTypeName;
			}
			return null;
		}

		// Token: 0x060004A5 RID: 1189 RVA: 0x00003F04 File Offset: 0x00002F04
		string ICustomTypeDescriptor.GetComponentName()
		{
			INamedObject namedObject = this as INamedObject;
			if (namedObject == null)
			{
				return null;
			}
			return namedObject.Name;
		}

		// Token: 0x060004A6 RID: 1190 RVA: 0x00003F23 File Offset: 0x00002F23
		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return TypeDescriptor.GetConverter(base.GetType());
		}

		// Token: 0x060004A7 RID: 1191 RVA: 0x00003F30 File Offset: 0x00002F30
		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return TypeDescriptor.GetDefaultEvent(base.GetType());
		}

		// Token: 0x060004A8 RID: 1192 RVA: 0x00003F3D File Offset: 0x00002F3D
		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return TypeDescriptor.GetDefaultProperty(base.GetType());
		}

		// Token: 0x060004A9 RID: 1193 RVA: 0x00003F4A File Offset: 0x00002F4A
		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return TypeDescriptor.GetEditor(base.GetType(), editorBaseType);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x00003F58 File Offset: 0x00002F58
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return TypeDescriptor.GetEvents(base.GetType());
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x00003F65 File Offset: 0x00002F65
		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return TypeDescriptor.GetEvents(base.GetType(), attributes);
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x00003F73 File Offset: 0x00002F73
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return this.GetProperties(null);
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x00003F7C File Offset: 0x00002F7C
		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return this.GetProperties(attributes);
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00003F85 File Offset: 0x00002F85
		private PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			return TypeDescriptor.GetProperties(base.GetType(), attributes);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00003F93 File Offset: 0x00002F93
		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}

		// Token: 0x060004B0 RID: 1200 RVA: 0x00003F98 File Offset: 0x00002F98
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

		// Token: 0x060004B1 RID: 1201 RVA: 0x00004008 File Offset: 0x00003008
		public virtual void SetCollection(DataSourceCollectionBase collection)
		{
			this.CollectionParent = collection;
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00004011 File Offset: 0x00003011
		internal void SetPropertyValue(string propertyName, object value)
		{
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x00004013 File Offset: 0x00003013
		internal virtual StringCollection NamingPropertyNames
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00004016 File Offset: 0x00003016
		[Browsable(false)]
		public virtual string GeneratorName
		{
			get
			{
				return null;
			}
		}

		// Token: 0x04000A95 RID: 2709
		private DataSourceCollectionBase collectionParent;
	}
}
