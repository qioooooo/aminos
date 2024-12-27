using System;
using System.Collections;
using System.Data;
using System.Globalization;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000579 RID: 1401
	internal static class FilteredDataSetHelper
	{
		// Token: 0x060044BE RID: 17598 RVA: 0x0011A5D8 File Offset: 0x001195D8
		public static DataView CreateFilteredDataView(DataTable table, string sortExpression, string filterExpression, IDictionary filterParameters)
		{
			DataView dataView = new DataView(table);
			if (!string.IsNullOrEmpty(sortExpression))
			{
				dataView.Sort = sortExpression;
			}
			if (!string.IsNullOrEmpty(filterExpression))
			{
				bool flag = false;
				object[] array = new object[filterParameters.Count];
				int num = 0;
				foreach (object obj in filterParameters)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					if (dictionaryEntry.Value == null)
					{
						flag = true;
						break;
					}
					array[num] = dictionaryEntry.Value;
					num++;
				}
				filterExpression = string.Format(CultureInfo.InvariantCulture, filterExpression, array);
				if (!flag)
				{
					dataView.RowFilter = filterExpression;
				}
			}
			return dataView;
		}

		// Token: 0x060044BF RID: 17599 RVA: 0x0011A690 File Offset: 0x00119690
		public static DataTable GetDataTable(Control owner, object dataObject)
		{
			DataSet dataSet = dataObject as DataSet;
			if (dataSet == null)
			{
				return dataObject as DataTable;
			}
			if (dataSet.Tables.Count == 0)
			{
				throw new InvalidOperationException(SR.GetString("FilteredDataSetHelper_DataSetHasNoTables", new object[] { owner.ID }));
			}
			return dataSet.Tables[0];
		}
	}
}
