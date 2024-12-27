using System;

namespace System.Data
{
	// Token: 0x020000D5 RID: 213
	internal sealed class RelatedView : DataView, IFilter
	{
		// Token: 0x06000CFE RID: 3326 RVA: 0x001FC9AC File Offset: 0x001FBDAC
		public RelatedView(DataColumn[] columns, object[] values)
			: base(columns[0].Table, false)
		{
			if (values == null)
			{
				throw ExceptionBuilder.ArgumentNull("values");
			}
			this.key = new DataKey(columns, true);
			this.values = values;
			base.ResetRowViewCache();
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x001FC9F0 File Offset: 0x001FBDF0
		public bool Invoke(DataRow row, DataRowVersion version)
		{
			object[] keyValues = row.GetKeyValues(this.key, version);
			bool flag = true;
			if (keyValues.Length != this.values.Length)
			{
				flag = false;
			}
			else
			{
				for (int i = 0; i < keyValues.Length; i++)
				{
					if (!keyValues[i].Equals(this.values[i]))
					{
						flag = false;
						break;
					}
				}
			}
			IFilter filter = base.GetFilter();
			if (filter != null)
			{
				flag &= filter.Invoke(row, version);
			}
			return flag;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x001FCA58 File Offset: 0x001FBE58
		internal override IFilter GetFilter()
		{
			return this;
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x001FCA68 File Offset: 0x001FBE68
		public override DataRowView AddNew()
		{
			DataRowView dataRowView = base.AddNew();
			dataRowView.Row.SetKeyValues(this.key, this.values);
			return dataRowView;
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x001FCA94 File Offset: 0x001FBE94
		internal override void SetIndex(string newSort, DataViewRowState newRowStates, IFilter newRowFilter)
		{
			base.SetIndex2(newSort, newRowStates, newRowFilter, false);
			base.Reset();
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x001FCAB4 File Offset: 0x001FBEB4
		public override bool Equals(DataView dv)
		{
			return dv is RelatedView && base.Equals(dv) && (this.CompareArray(this.key.ColumnsReference, ((RelatedView)dv).key.ColumnsReference) || this.CompareArray(this.values, ((RelatedView)dv).values));
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x001FCB18 File Offset: 0x001FBF18
		private bool CompareArray(object[] value1, object[] value2)
		{
			if (value1.Length != value2.Length)
			{
				return false;
			}
			for (int i = 0; i < value1.Length; i++)
			{
				if (value1[i] != value2[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x040008E1 RID: 2273
		private readonly DataKey key;

		// Token: 0x040008E2 RID: 2274
		private object[] values;
	}
}
