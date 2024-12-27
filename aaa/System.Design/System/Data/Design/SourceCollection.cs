using System;
using System.Collections;
using System.Design;

namespace System.Data.Design
{
	// Token: 0x020000BB RID: 187
	internal class SourceCollection : DataSourceCollectionBase, ICloneable
	{
		// Token: 0x06000848 RID: 2120 RVA: 0x0001528C File Offset: 0x0001428C
		internal SourceCollection(DataSourceComponent collectionHost)
			: base(collectionHost)
		{
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x00015295 File Offset: 0x00014295
		protected override Type ItemType
		{
			get
			{
				return typeof(Source);
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x000152A4 File Offset: 0x000142A4
		private DbSource MainSource
		{
			get
			{
				DesignTable designTable = this.CollectionHost as DesignTable;
				return designTable.MainSource as DbSource;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x0600084B RID: 2123 RVA: 0x000152C8 File Offset: 0x000142C8
		protected override INameService NameService
		{
			get
			{
				return SourceNameService.DefaultInstance;
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000152CF File Offset: 0x000142CF
		public int Add(Source s)
		{
			return base.List.Add(s);
		}

		// Token: 0x0600084D RID: 2125 RVA: 0x000152E0 File Offset: 0x000142E0
		public object Clone()
		{
			SourceCollection sourceCollection = new SourceCollection(null);
			foreach (object obj in this)
			{
				Source source = (Source)obj;
				sourceCollection.Add((Source)source.Clone());
			}
			return sourceCollection;
		}

		// Token: 0x0600084E RID: 2126 RVA: 0x00015348 File Offset: 0x00014348
		public bool Contains(Source s)
		{
			return base.List.Contains(s);
		}

		// Token: 0x0600084F RID: 2127 RVA: 0x00015358 File Offset: 0x00014358
		private bool DbSourceNameExist(DbSource dbSource, bool isFillName, string nameToBeChecked)
		{
			if (isFillName && StringUtil.EqualValue(nameToBeChecked, dbSource.GetMethodName, true))
			{
				return true;
			}
			if (!isFillName && StringUtil.EqualValue(nameToBeChecked, dbSource.FillMethodName, true))
			{
				return true;
			}
			foreach (object obj in this)
			{
				DbSource dbSource2 = (DbSource)obj;
				if (dbSource2 != dbSource && dbSource2.NameExist(nameToBeChecked))
				{
					return true;
				}
			}
			DbSource mainSource = this.MainSource;
			return dbSource != mainSource && mainSource != null && mainSource.NameExist(nameToBeChecked);
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00015400 File Offset: 0x00014400
		protected internal override IDataSourceNamedObject FindObject(string name)
		{
			DbSource mainSource = this.MainSource;
			if (mainSource != null && mainSource.NameExist(name))
			{
				return mainSource;
			}
			foreach (object obj in base.InnerList)
			{
				DbSource dbSource = obj as DbSource;
				if (dbSource != null)
				{
					if (dbSource.NameExist(name))
					{
						return dbSource;
					}
				}
				else
				{
					IEnumerator enumerator;
					IDataSourceNamedObject dataSourceNamedObject = (IDataSourceNamedObject)enumerator.Current;
					if (StringUtil.EqualValue(dataSourceNamedObject.Name, name, false))
					{
						return dataSourceNamedObject;
					}
				}
			}
			return null;
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00015470 File Offset: 0x00014470
		public int IndexOf(Source s)
		{
			return base.List.IndexOf(s);
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x0001547E File Offset: 0x0001447E
		public void Remove(Source s)
		{
			base.List.Remove(s);
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x0001548C File Offset: 0x0001448C
		private void ValidateNameWithMainSource(object dbSourceToCheck, string nameToCheck)
		{
			DbSource mainSource = this.MainSource;
			if (dbSourceToCheck != mainSource && mainSource != null && mainSource.NameExist(nameToCheck))
			{
				throw new NameValidationException(SR.GetString("CM_NameExist", new object[] { nameToCheck }));
			}
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x000154CC File Offset: 0x000144CC
		protected internal override void ValidateName(IDataSourceNamedObject obj)
		{
			DbSource dbSource = obj as DbSource;
			if (dbSource != null)
			{
				if ((dbSource.GenerateMethods & GenerateMethodTypes.Get) == GenerateMethodTypes.Get)
				{
					this.NameService.ValidateName(dbSource.GetMethodName);
				}
				if ((dbSource.GenerateMethods & GenerateMethodTypes.Fill) == GenerateMethodTypes.Fill)
				{
					this.NameService.ValidateName(dbSource.FillMethodName);
					return;
				}
			}
			else
			{
				base.ValidateName(obj);
			}
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00015523 File Offset: 0x00014523
		protected internal override void ValidateUniqueName(IDataSourceNamedObject obj, string proposedName)
		{
			this.ValidateNameWithMainSource(obj, proposedName);
			base.ValidateUniqueName(obj, proposedName);
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00015538 File Offset: 0x00014538
		internal void ValidateUniqueDbSourceName(DbSource dbSource, string proposedName, bool isFillName)
		{
			if (this.DbSourceNameExist(dbSource, isFillName, proposedName))
			{
				throw new NameValidationException(SR.GetString("CM_NameExist", new object[] { proposedName }));
			}
			this.NameService.ValidateName(proposedName);
		}
	}
}
