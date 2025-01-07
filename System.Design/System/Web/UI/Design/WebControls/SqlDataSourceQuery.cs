using System;
using System.Collections;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class SqlDataSourceQuery
	{
		public SqlDataSourceQuery(string command, SqlDataSourceCommandType commandType, ICollection parameters)
		{
			this._command = command;
			this._commandType = commandType;
			this._parameters = parameters;
		}

		public string Command
		{
			get
			{
				return this._command;
			}
		}

		public SqlDataSourceCommandType CommandType
		{
			get
			{
				return this._commandType;
			}
		}

		public ICollection Parameters
		{
			get
			{
				return this._parameters;
			}
		}

		private string _command;

		private SqlDataSourceCommandType _commandType;

		private ICollection _parameters;
	}
}
