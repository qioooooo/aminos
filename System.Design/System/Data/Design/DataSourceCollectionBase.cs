using System;
using System.Collections;
using System.ComponentModel;

namespace System.Data.Design
{
	internal abstract class DataSourceCollectionBase : CollectionBase, INamedObjectCollection, ICollection, IEnumerable, IObjectWithParent
	{
		internal DataSourceCollectionBase(DataSourceComponent collectionHost)
		{
			this.collectionHost = collectionHost;
		}

		internal virtual DataSourceComponent CollectionHost
		{
			get
			{
				return this.collectionHost;
			}
			set
			{
				this.collectionHost = value;
			}
		}

		protected virtual Type ItemType
		{
			get
			{
				return typeof(IDataSourceNamedObject);
			}
		}

		protected abstract INameService NameService { get; }

		[Browsable(false)]
		object IObjectWithParent.Parent
		{
			get
			{
				return this.collectionHost;
			}
		}

		protected virtual string CreateUniqueName(IDataSourceNamedObject value)
		{
			string text = (StringUtil.NotEmpty(value.Name) ? value.Name : value.PublicTypeName);
			return this.NameService.CreateUniqueName(this, text, 1);
		}

		protected internal virtual void EnsureUniqueName(IDataSourceNamedObject namedObject)
		{
			if (namedObject.Name == null || namedObject.Name.Length == 0 || this.FindObject(namedObject.Name) != null)
			{
				namedObject.Name = this.CreateUniqueName(namedObject);
			}
		}

		protected internal virtual IDataSourceNamedObject FindObject(string name)
		{
			foreach (object obj in base.InnerList)
			{
				IDataSourceNamedObject dataSourceNamedObject = (IDataSourceNamedObject)obj;
				if (StringUtil.EqualValue(dataSourceNamedObject.Name, name))
				{
					return dataSourceNamedObject;
				}
			}
			return null;
		}

		public void InsertBefore(object value, object refObject)
		{
			int num = base.List.IndexOf(refObject);
			if (num >= 0)
			{
				base.List.Insert(num, value);
				return;
			}
			base.List.Add(value);
		}

		protected override void OnValidate(object value)
		{
			base.OnValidate(value);
			this.ValidateType(value);
		}

		public void Remove(string name)
		{
			INamedObject namedObject = NamedObjectUtil.Find(this, name);
			if (namedObject != null)
			{
				base.List.Remove(namedObject);
			}
		}

		protected internal virtual void ValidateName(IDataSourceNamedObject obj)
		{
			this.NameService.ValidateName(obj.Name);
		}

		protected internal virtual void ValidateUniqueName(IDataSourceNamedObject obj, string proposedName)
		{
			this.NameService.ValidateUniqueName(this, obj, proposedName);
		}

		protected void ValidateType(object value)
		{
			if (!this.ItemType.IsInstanceOfType(value))
			{
				throw new InternalException("{0} can hold only {1} objects", 20016, true);
			}
		}

		public INameService GetNameService()
		{
			return this.NameService;
		}

		private DataSourceComponent collectionHost;
	}
}
