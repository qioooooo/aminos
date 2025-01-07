using System;
using System.Collections;
using System.Globalization;

namespace System.Data.Design
{
	internal class DbSourceParameterCollection : DataSourceCollectionBase, IDataParameterCollection, IList, ICollection, IEnumerable, ICloneable
	{
		internal DbSourceParameterCollection(DataSourceComponent collectionHost)
			: base(collectionHost)
		{
		}

		protected override INameService NameService
		{
			get
			{
				return SimpleNameService.DefaultInstance;
			}
		}

		object IDataParameterCollection.this[string parameterName]
		{
			get
			{
				int num = this.RangeCheck(parameterName);
				return base.List[num];
			}
			set
			{
				int num = this.RangeCheck(parameterName);
				base.List[num] = value;
			}
		}

		public DesignParameter this[int index]
		{
			get
			{
				return (DesignParameter)base.List[index];
			}
		}

		public bool Contains(string value)
		{
			return this.IndexOf(value) != -1;
		}

		public int IndexOf(string parameterName)
		{
			int count = base.InnerList.Count;
			for (int i = 0; i < count; i++)
			{
				if (StringUtil.EqualValue(parameterName, ((IDbDataParameter)base.InnerList[i]).ParameterName))
				{
					return i;
				}
			}
			return -1;
		}

		private int RangeCheck(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw new InternalException(string.Format(CultureInfo.CurrentCulture, "No parameter named '{0}' found", new object[] { parameterName }), 20004);
			}
			return num;
		}

		public void RemoveAt(string parameterName)
		{
			int num = this.RangeCheck(parameterName);
			base.List.RemoveAt(num);
		}

		protected override Type ItemType
		{
			get
			{
				return typeof(DesignParameter);
			}
		}

		public object Clone()
		{
			DbSourceParameterCollection dbSourceParameterCollection = new DbSourceParameterCollection(null);
			foreach (object obj in this)
			{
				DesignParameter designParameter = (DesignParameter)obj;
				DesignParameter designParameter2 = (DesignParameter)designParameter.Clone();
				((IList)dbSourceParameterCollection).Add(designParameter2);
			}
			return dbSourceParameterCollection;
		}
	}
}
