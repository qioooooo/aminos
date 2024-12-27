using System;
using System.Collections;
using System.Data.Common;

namespace System.Data.Design
{
	// Token: 0x02000072 RID: 114
	internal sealed class DataDesignUtil
	{
		// Token: 0x060004E3 RID: 1251 RVA: 0x00005981 File Offset: 0x00004981
		private DataDesignUtil()
		{
		}

		// Token: 0x060004E4 RID: 1252 RVA: 0x0000598C File Offset: 0x0000498C
		internal static string[] MapColumnNames(DataColumnMappingCollection mappingCollection, string[] names, DataDesignUtil.MappingDirection direction)
		{
			if (mappingCollection == null || names == null)
			{
				return new string[0];
			}
			ArrayList arrayList = new ArrayList();
			foreach (string text in names)
			{
				string text2;
				try
				{
					if (direction == DataDesignUtil.MappingDirection.DataSetToSource)
					{
						DataColumnMapping dataColumnMapping = mappingCollection.GetByDataSetColumn(text);
						text2 = dataColumnMapping.SourceColumn;
					}
					else
					{
						DataColumnMapping dataColumnMapping = mappingCollection[text];
						text2 = dataColumnMapping.DataSetColumn;
					}
				}
				catch (IndexOutOfRangeException)
				{
					text2 = text;
				}
				arrayList.Add(text2);
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x060004E5 RID: 1253 RVA: 0x00005A20 File Offset: 0x00004A20
		public static void CopyColumn(DataColumn srcColumn, DataColumn destColumn)
		{
			destColumn.AllowDBNull = srcColumn.AllowDBNull;
			destColumn.AutoIncrement = srcColumn.AutoIncrement;
			destColumn.AutoIncrementSeed = srcColumn.AutoIncrementSeed;
			destColumn.AutoIncrementStep = srcColumn.AutoIncrementStep;
			destColumn.Caption = srcColumn.Caption;
			destColumn.ColumnMapping = srcColumn.ColumnMapping;
			destColumn.ColumnName = srcColumn.ColumnName;
			destColumn.DataType = srcColumn.DataType;
			destColumn.DefaultValue = srcColumn.DefaultValue;
			destColumn.Expression = srcColumn.Expression;
			destColumn.MaxLength = srcColumn.MaxLength;
			destColumn.Prefix = srcColumn.Prefix;
			destColumn.ReadOnly = srcColumn.ReadOnly;
		}

		// Token: 0x060004E6 RID: 1254 RVA: 0x00005ACC File Offset: 0x00004ACC
		public static DataColumn CloneColumn(DataColumn column)
		{
			DataColumn dataColumn = new DataColumn();
			DataDesignUtil.CopyColumn(column, dataColumn);
			return dataColumn;
		}

		// Token: 0x04000AB4 RID: 2740
		internal static string DataSetClassName = typeof(DataSet).ToString();

		// Token: 0x02000073 RID: 115
		internal enum MappingDirection
		{
			// Token: 0x04000AB6 RID: 2742
			SourceToDataSet,
			// Token: 0x04000AB7 RID: 2743
			DataSetToSource
		}
	}
}
