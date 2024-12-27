using System;
using System.Collections;

namespace System.Data.Design
{
	// Token: 0x02000094 RID: 148
	internal class DesignConnectionCollection : DataSourceCollectionBase, IDesignConnectionCollection, INamedObjectCollection, ICollection, IEnumerable
	{
		// Token: 0x06000633 RID: 1587 RVA: 0x0000BFAE File Offset: 0x0000AFAE
		internal DesignConnectionCollection(DataSourceComponent collectionHost)
			: base(collectionHost)
		{
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0000BFB7 File Offset: 0x0000AFB7
		protected override Type ItemType
		{
			get
			{
				return typeof(IDesignConnection);
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x0000BFC3 File Offset: 0x0000AFC3
		protected override INameService NameService
		{
			get
			{
				return SimpleNameService.DefaultInstance;
			}
		}

		// Token: 0x06000636 RID: 1590 RVA: 0x0000BFCA File Offset: 0x0000AFCA
		public IDesignConnection Get(string name)
		{
			return (IDesignConnection)NamedObjectUtil.Find(this, name);
		}

		// Token: 0x06000637 RID: 1591 RVA: 0x0000BFD8 File Offset: 0x0000AFD8
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

		// Token: 0x06000638 RID: 1592 RVA: 0x0000C024 File Offset: 0x0000B024
		public void Set(IDesignConnection connection)
		{
			INamedObject namedObject = NamedObjectUtil.Find(this, connection.Name);
			if (namedObject != null)
			{
				base.List.Remove(namedObject);
			}
			base.List.Add(connection);
		}

		// Token: 0x06000639 RID: 1593 RVA: 0x0000C05A File Offset: 0x0000B05A
		public bool Contains(IDesignConnection connection)
		{
			return base.List.Contains(connection);
		}

		// Token: 0x0600063A RID: 1594 RVA: 0x0000C068 File Offset: 0x0000B068
		public int Add(IDesignConnection connection)
		{
			return base.List.Add(connection);
		}

		// Token: 0x0600063B RID: 1595 RVA: 0x0000C076 File Offset: 0x0000B076
		public void Remove(IDesignConnection connection)
		{
			base.List.Remove(connection);
		}
	}
}
