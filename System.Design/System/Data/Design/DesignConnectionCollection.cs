using System;
using System.Collections;

namespace System.Data.Design
{
	internal class DesignConnectionCollection : DataSourceCollectionBase, IDesignConnectionCollection, INamedObjectCollection, ICollection, IEnumerable
	{
		internal DesignConnectionCollection(DataSourceComponent collectionHost)
			: base(collectionHost)
		{
		}

		protected override Type ItemType
		{
			get
			{
				return typeof(IDesignConnection);
			}
		}

		protected override INameService NameService
		{
			get
			{
				return SimpleNameService.DefaultInstance;
			}
		}

		public IDesignConnection Get(string name)
		{
			return (IDesignConnection)NamedObjectUtil.Find(this, name);
		}

		protected override void OnSet(int index, object oldValue, object newValue)
		{
			base.OnSet(index, oldValue, newValue);
			base.ValidateType(newValue);
			IDesignConnection designConnection = (IDesignConnection)oldValue;
			IDesignConnection designConnection2 = (IDesignConnection)newValue;
			if (!StringUtil.EqualValue(designConnection.Name, designConnection2.Name))
			{
				this.ValidateUniqueName(designConnection2, designConnection2.Name);
			}
		}

		public void Set(IDesignConnection connection)
		{
			INamedObject namedObject = NamedObjectUtil.Find(this, connection.Name);
			if (namedObject != null)
			{
				base.List.Remove(namedObject);
			}
			base.List.Add(connection);
		}

		public bool Contains(IDesignConnection connection)
		{
			return base.List.Contains(connection);
		}

		public int Add(IDesignConnection connection)
		{
			return base.List.Add(connection);
		}

		public void Remove(IDesignConnection connection)
		{
			base.List.Remove(connection);
		}
	}
}
