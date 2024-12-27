using System;

namespace System.Data.Odbc
{
	// Token: 0x020001BB RID: 443
	internal sealed class DbCache
	{
		// Token: 0x06001953 RID: 6483 RVA: 0x0023E7C0 File Offset: 0x0023DBC0
		internal DbCache(OdbcDataReader record, int count)
		{
			this._count = count;
			this._record = record;
			this._randomaccess = !record.IsBehavior(CommandBehavior.SequentialAccess);
			this._values = new object[count];
			this._isBadValue = new bool[count];
		}

		// Token: 0x17000333 RID: 819
		internal object this[int i]
		{
			get
			{
				if (this._isBadValue[i])
				{
					OverflowException ex = (OverflowException)this.Values[i];
					throw new OverflowException(ex.Message, ex);
				}
				return this.Values[i];
			}
			set
			{
				this.Values[i] = value;
				this._isBadValue[i] = false;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001956 RID: 6486 RVA: 0x0023E870 File Offset: 0x0023DC70
		internal int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x0023E884 File Offset: 0x0023DC84
		internal void InvalidateValue(int i)
		{
			this._isBadValue[i] = true;
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001958 RID: 6488 RVA: 0x0023E89C File Offset: 0x0023DC9C
		internal object[] Values
		{
			get
			{
				return this._values;
			}
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x0023E8B0 File Offset: 0x0023DCB0
		internal object AccessIndex(int i)
		{
			object[] values = this.Values;
			if (this._randomaccess)
			{
				for (int j = 0; j < i; j++)
				{
					if (values[j] == null)
					{
						values[j] = this._record.GetValue(j);
					}
				}
			}
			return values[i];
		}

		// Token: 0x0600195A RID: 6490 RVA: 0x0023E8F0 File Offset: 0x0023DCF0
		internal DbSchemaInfo GetSchema(int i)
		{
			if (this._schema == null)
			{
				this._schema = new DbSchemaInfo[this.Count];
			}
			if (this._schema[i] == null)
			{
				this._schema[i] = new DbSchemaInfo();
			}
			return this._schema[i];
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x0023E938 File Offset: 0x0023DD38
		internal void FlushValues()
		{
			int num = this._values.Length;
			for (int i = 0; i < num; i++)
			{
				this._values[i] = null;
			}
		}

		// Token: 0x04000E34 RID: 3636
		private bool[] _isBadValue;

		// Token: 0x04000E35 RID: 3637
		private DbSchemaInfo[] _schema;

		// Token: 0x04000E36 RID: 3638
		private object[] _values;

		// Token: 0x04000E37 RID: 3639
		private OdbcDataReader _record;

		// Token: 0x04000E38 RID: 3640
		internal int _count;

		// Token: 0x04000E39 RID: 3641
		internal bool _randomaccess = true;
	}
}
