using System;
using System.ComponentModel.Design.Data;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class SqlDataSourceOrderClause
	{
		public SqlDataSourceOrderClause(DesignerDataConnection designerDataConnection, DesignerDataTableBase designerDataTable, DesignerDataColumn designerDataColumn, bool isDescending)
		{
			this._designerDataConnection = designerDataConnection;
			this._designerDataTable = designerDataTable;
			this._designerDataColumn = designerDataColumn;
			this._isDescending = isDescending;
		}

		public DesignerDataColumn DesignerDataColumn
		{
			get
			{
				return this._designerDataColumn;
			}
		}

		public bool IsDescending
		{
			get
			{
				return this._isDescending;
			}
		}

		public override string ToString()
		{
			SqlDataSourceColumnData sqlDataSourceColumnData = new SqlDataSourceColumnData(this._designerDataConnection, this._designerDataColumn);
			if (this._isDescending)
			{
				return sqlDataSourceColumnData.EscapedName + " DESC";
			}
			return sqlDataSourceColumnData.EscapedName;
		}

		private DesignerDataColumn _designerDataColumn;

		private DesignerDataTableBase _designerDataTable;

		private DesignerDataConnection _designerDataConnection;

		private bool _isDescending;
	}
}
