using System;
using System.Collections;
using System.Data.Common;

namespace System.Data.Design
{
	internal sealed class DataDesignUtil
	{
		private DataDesignUtil()
		{
		}

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

		public static DataColumn CloneColumn(DataColumn column)
		{
			DataColumn dataColumn = new DataColumn();
			DataDesignUtil.CopyColumn(column, dataColumn);
			return dataColumn;
		}

		internal static string DataSetClassName = typeof(DataSet).ToString();

		internal enum MappingDirection
		{
			SourceToDataSet,
			DataSetToSource
		}
	}
}
