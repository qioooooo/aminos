﻿using System;
using System.Data;

namespace System.Web.UI.Design
{
	public sealed class DataSetFieldSchema : IDataSourceFieldSchema
	{
		public DataSetFieldSchema(DataColumn column)
		{
			if (column == null)
			{
				throw new ArgumentNullException("column");
			}
			this._column = column;
		}

		public Type DataType
		{
			get
			{
				return this._column.DataType;
			}
		}

		public bool Identity
		{
			get
			{
				return this._column.AutoIncrement;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return this._column.ReadOnly;
			}
		}

		public bool IsUnique
		{
			get
			{
				return this._column.Unique;
			}
		}

		public int Length
		{
			get
			{
				return this._column.MaxLength;
			}
		}

		public string Name
		{
			get
			{
				return this._column.ColumnName;
			}
		}

		public bool Nullable
		{
			get
			{
				return this._column.AllowDBNull;
			}
		}

		public int Precision
		{
			get
			{
				return -1;
			}
		}

		public bool PrimaryKey
		{
			get
			{
				if (this._column.Table == null || this._column.Table.PrimaryKey == null)
				{
					return false;
				}
				foreach (DataColumn dataColumn in this._column.Table.PrimaryKey)
				{
					if (dataColumn == this._column)
					{
						return true;
					}
				}
				return false;
			}
		}

		public int Scale
		{
			get
			{
				return -1;
			}
		}

		private DataColumn _column;
	}
}
