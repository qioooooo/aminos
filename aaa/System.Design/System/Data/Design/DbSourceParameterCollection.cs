using System;
using System.Collections;
using System.Globalization;

namespace System.Data.Design
{
	// Token: 0x0200008D RID: 141
	internal class DbSourceParameterCollection : DataSourceCollectionBase, IDataParameterCollection, IList, ICollection, IEnumerable, ICloneable
	{
		// Token: 0x060005C6 RID: 1478 RVA: 0x0000B23B File Offset: 0x0000A23B
		internal DbSourceParameterCollection(DataSourceComponent collectionHost)
			: base(collectionHost)
		{
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060005C7 RID: 1479 RVA: 0x0000B244 File Offset: 0x0000A244
		protected override INameService NameService
		{
			get
			{
				return SimpleNameService.DefaultInstance;
			}
		}

		// Token: 0x1700005B RID: 91
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

		// Token: 0x1700005C RID: 92
		public DesignParameter this[int index]
		{
			get
			{
				return (DesignParameter)base.List[index];
			}
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x0000B2A5 File Offset: 0x0000A2A5
		public bool Contains(string value)
		{
			return this.IndexOf(value) != -1;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x0000B2B4 File Offset: 0x0000A2B4
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

		// Token: 0x060005CD RID: 1485 RVA: 0x0000B2FC File Offset: 0x0000A2FC
		private int RangeCheck(string parameterName)
		{
			int num = this.IndexOf(parameterName);
			if (num < 0)
			{
				throw new InternalException(string.Format(CultureInfo.CurrentCulture, "No parameter named '{0}' found", new object[] { parameterName }), 20004);
			}
			return num;
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0000B33C File Offset: 0x0000A33C
		public void RemoveAt(string parameterName)
		{
			int num = this.RangeCheck(parameterName);
			base.List.RemoveAt(num);
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060005CF RID: 1487 RVA: 0x0000B35D File Offset: 0x0000A35D
		protected override Type ItemType
		{
			get
			{
				return typeof(DesignParameter);
			}
		}

		// Token: 0x060005D0 RID: 1488 RVA: 0x0000B36C File Offset: 0x0000A36C
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
