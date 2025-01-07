using System;
using System.Collections;
using System.Design;

namespace System.Data.Design
{
	internal class SourceCollection : DataSourceCollectionBase, ICloneable
	{
		internal SourceCollection(DataSourceComponent collectionHost)
			: base(collectionHost)
		{
		}

		protected override Type ItemType
		{
			get
			{
				return typeof(Source);
			}
		}

		private DbSource MainSource
		{
			get
			{
				DesignTable designTable = this.CollectionHost as DesignTable;
				return designTable.MainSource as DbSource;
			}
		}

		protected override INameService NameService
		{
			get
			{
				return SourceNameService.DefaultInstance;
			}
		}

		public int Add(Source s)
		{
			return base.List.Add(s);
		}

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

		public bool Contains(Source s)
		{
			return base.List.Contains(s);
		}

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

		public int IndexOf(Source s)
		{
			return base.List.IndexOf(s);
		}

		public void Remove(Source s)
		{
			base.List.Remove(s);
		}

		private void ValidateNameWithMainSource(object dbSourceToCheck, string nameToCheck)
		{
			DbSource mainSource = this.MainSource;
			if (dbSourceToCheck != mainSource && mainSource != null && mainSource.NameExist(nameToCheck))
			{
				throw new NameValidationException(SR.GetString("CM_NameExist", new object[] { nameToCheck }));
			}
		}

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

		protected internal override void ValidateUniqueName(IDataSourceNamedObject obj, string proposedName)
		{
			this.ValidateNameWithMainSource(obj, proposedName);
			base.ValidateUniqueName(obj, proposedName);
		}

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
