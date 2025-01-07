using System;
using System.ComponentModel.Design.Data;
using System.Globalization;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class SqlDataSourceFilterClause
	{
		public SqlDataSourceFilterClause(DesignerDataConnection designerDataConnection, DesignerDataTableBase designerDataTable, DesignerDataColumn designerDataColumn, string operatorFormat, bool isBinary, string value, Parameter parameter)
		{
			this._designerDataConnection = designerDataConnection;
			this._designerDataTable = designerDataTable;
			this._designerDataColumn = designerDataColumn;
			this._isBinary = isBinary;
			this._operatorFormat = operatorFormat;
			this._value = value;
			this._parameter = parameter;
		}

		public DesignerDataColumn DesignerDataColumn
		{
			get
			{
				return this._designerDataColumn;
			}
		}

		public bool IsBinary
		{
			get
			{
				return this._isBinary;
			}
		}

		public string OperatorFormat
		{
			get
			{
				return this._operatorFormat;
			}
		}

		public Parameter Parameter
		{
			get
			{
				return this._parameter;
			}
		}

		public string Value
		{
			get
			{
				return this._value;
			}
		}

		public override string ToString()
		{
			SqlDataSourceColumnData sqlDataSourceColumnData = new SqlDataSourceColumnData(this._designerDataConnection, this._designerDataColumn);
			if (this._isBinary)
			{
				return string.Format(CultureInfo.InvariantCulture, this._operatorFormat, new object[] { sqlDataSourceColumnData.EscapedName, this._value });
			}
			return string.Format(CultureInfo.InvariantCulture, this._operatorFormat, new object[] { sqlDataSourceColumnData.EscapedName });
		}

		private DesignerDataColumn _designerDataColumn;

		private DesignerDataTableBase _designerDataTable;

		private DesignerDataConnection _designerDataConnection;

		private bool _isBinary;

		private string _operatorFormat;

		private string _value;

		private Parameter _parameter;
	}
}
