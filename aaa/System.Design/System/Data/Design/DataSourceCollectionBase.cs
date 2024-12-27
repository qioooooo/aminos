using System;
using System.Collections;
using System.ComponentModel;

namespace System.Data.Design
{
	// Token: 0x02000076 RID: 118
	internal abstract class DataSourceCollectionBase : CollectionBase, INamedObjectCollection, ICollection, IEnumerable, IObjectWithParent
	{
		// Token: 0x060004FF RID: 1279 RVA: 0x00008683 File Offset: 0x00007683
		internal DataSourceCollectionBase(DataSourceComponent collectionHost)
		{
			this.collectionHost = collectionHost;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x06000500 RID: 1280 RVA: 0x00008692 File Offset: 0x00007692
		// (set) Token: 0x06000501 RID: 1281 RVA: 0x0000869A File Offset: 0x0000769A
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

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x06000502 RID: 1282 RVA: 0x000086A3 File Offset: 0x000076A3
		protected virtual Type ItemType
		{
			get
			{
				return typeof(IDataSourceNamedObject);
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x06000503 RID: 1283
		protected abstract INameService NameService { get; }

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000504 RID: 1284 RVA: 0x000086AF File Offset: 0x000076AF
		[Browsable(false)]
		object IObjectWithParent.Parent
		{
			get
			{
				return this.collectionHost;
			}
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x000086B8 File Offset: 0x000076B8
		protected virtual string CreateUniqueName(IDataSourceNamedObject value)
		{
			string text = (StringUtil.NotEmpty(value.Name) ? value.Name : value.PublicTypeName);
			return this.NameService.CreateUniqueName(this, text, 1);
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x000086EF File Offset: 0x000076EF
		protected internal virtual void EnsureUniqueName(IDataSourceNamedObject namedObject)
		{
			if (namedObject.Name == null || namedObject.Name.Length == 0 || this.FindObject(namedObject.Name) != null)
			{
				namedObject.Name = this.CreateUniqueName(namedObject);
			}
		}

		// Token: 0x06000507 RID: 1287 RVA: 0x00008724 File Offset: 0x00007724
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

		// Token: 0x06000508 RID: 1288 RVA: 0x00008764 File Offset: 0x00007764
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

		// Token: 0x06000509 RID: 1289 RVA: 0x0000879D File Offset: 0x0000779D
		protected override void OnValidate(object value)
		{
			base.OnValidate(value);
			this.ValidateType(value);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x000087B0 File Offset: 0x000077B0
		public void Remove(string name)
		{
			INamedObject namedObject = NamedObjectUtil.Find(this, name);
			if (namedObject != null)
			{
				base.List.Remove(namedObject);
			}
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x000087D4 File Offset: 0x000077D4
		protected internal virtual void ValidateName(IDataSourceNamedObject obj)
		{
			this.NameService.ValidateName(obj.Name);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x000087E7 File Offset: 0x000077E7
		protected internal virtual void ValidateUniqueName(IDataSourceNamedObject obj, string proposedName)
		{
			this.NameService.ValidateUniqueName(this, obj, proposedName);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x000087F7 File Offset: 0x000077F7
		protected void ValidateType(object value)
		{
			if (!this.ItemType.IsInstanceOfType(value))
			{
				throw new InternalException("{0} can hold only {1} objects", 20016, true);
			}
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x00008818 File Offset: 0x00007818
		public INameService GetNameService()
		{
			return this.NameService;
		}

		// Token: 0x04000ABF RID: 2751
		private DataSourceComponent collectionHost;
	}
}
