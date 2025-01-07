using System;
using System.Collections.Specialized;
using System.ComponentModel.Design.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Globalization;
using System.Text;

namespace System.Web.UI.Design.WebControls
{
	internal sealed class SqlDataSourceColumnData
	{
		public SqlDataSourceColumnData(DesignerDataConnection connection, DesignerDataColumn column)
			: this(connection, column, null)
		{
		}

		public SqlDataSourceColumnData(DesignerDataConnection connection, DesignerDataColumn column, StringCollection usedNames)
		{
			this._connection = connection;
			this._column = column;
			this._usedNames = usedNames;
		}

		public string AliasedName
		{
			get
			{
				if (this._cachedAliasedName == null)
				{
					this._cachedAliasedName = this.CreateAliasedName();
				}
				return this._cachedAliasedName;
			}
		}

		public DesignerDataColumn Column
		{
			get
			{
				return this._column;
			}
		}

		public string SelectName
		{
			get
			{
				if (this._column == null)
				{
					return this.EscapedName;
				}
				string aliasedName = this.AliasedName;
				if (aliasedName != this._column.Name)
				{
					return this.EscapedName + " AS " + this.AliasedName;
				}
				return this.EscapedName;
			}
		}

		public string EscapedName
		{
			get
			{
				if (this._cachedEscapedName == null)
				{
					this._cachedEscapedName = this.CreateEscapedName();
				}
				return this._cachedEscapedName;
			}
		}

		public string ParameterPlaceholder
		{
			get
			{
				if (this._cachedParameterPlaceholder == null)
				{
					this._cachedParameterPlaceholder = this.CreateParameterPlaceholder(null);
				}
				return this._cachedParameterPlaceholder;
			}
		}

		public string WebParameterName
		{
			get
			{
				if (this._cachedWebParameterName == null)
				{
					this._cachedWebParameterName = this.CreateWebParameterName(null);
				}
				return this._cachedWebParameterName;
			}
		}

		internal static string EscapeObjectName(DesignerDataConnection connection, string objectName)
		{
			string text = "[";
			string text2 = "]";
			string text3;
			try
			{
				DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(connection.ProviderName);
				DbCommandBuilder dbCommandBuilder = dbProviderFactory.CreateCommandBuilder();
				if (dbProviderFactory == OracleClientFactory.Instance)
				{
					text2 = (text = "\"");
				}
				dbCommandBuilder.QuotePrefix = text;
				dbCommandBuilder.QuoteSuffix = text2;
				text3 = dbCommandBuilder.QuoteIdentifier(objectName);
			}
			catch (Exception)
			{
				text3 = text + objectName + text2;
			}
			return text3;
		}

		private string CreateAliasedName()
		{
			string name = this._column.Name;
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			bool flag2 = false;
			foreach (char c in name)
			{
				if (char.IsWhiteSpace(c) || c == '_')
				{
					if (!flag2)
					{
						stringBuilder.Append('_');
						flag2 = true;
					}
				}
				else
				{
					if (!char.IsLetterOrDigit(c))
					{
						flag = true;
						break;
					}
					stringBuilder.Append(c);
					flag2 = false;
				}
			}
			if (stringBuilder.Length == 0 || !char.IsLetter(stringBuilder[0]))
			{
				flag = true;
			}
			string text2;
			int num;
			string text3;
			if (flag)
			{
				text2 = "column";
				num = 1;
				text3 = text2 + '1';
			}
			else
			{
				num = 2;
				text2 = stringBuilder.ToString();
				text3 = text2;
			}
			if (this._usedNames != null)
			{
				if (this._usedNames.Contains(text3))
				{
					do
					{
						text3 = text2 + num.ToString(CultureInfo.InvariantCulture);
						num++;
					}
					while (this._usedNames.Contains(text3));
				}
				this._usedNames.Add(text3);
			}
			return text3;
		}

		private string CreateEscapedName()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this._column == null)
			{
				stringBuilder.Append("*");
			}
			else
			{
				stringBuilder.Append(SqlDataSourceColumnData.EscapeObjectName(this._connection, this._column.Name));
			}
			return stringBuilder.ToString();
		}

		private string CreateParameterPlaceholder(string oldValueFormatString)
		{
			DbProviderFactory dbProviderFactory = SqlDataSourceDesigner.GetDbProviderFactory(this._connection.ProviderName);
			string parameterPlaceholderPrefix = SqlDataSourceDesigner.GetParameterPlaceholderPrefix(dbProviderFactory);
			string text = parameterPlaceholderPrefix;
			if (SqlDataSourceDesigner.SupportsNamedParameters(dbProviderFactory))
			{
				if (oldValueFormatString == null)
				{
					text += this.AliasedName;
				}
				else
				{
					text += string.Format(CultureInfo.InvariantCulture, oldValueFormatString, new object[] { this.AliasedName });
				}
			}
			return text;
		}

		private string CreateWebParameterName(string oldValueFormatString)
		{
			if (oldValueFormatString == null)
			{
				return this.AliasedName;
			}
			return string.Format(CultureInfo.InvariantCulture, oldValueFormatString, new object[] { this.AliasedName });
		}

		public string GetOldValueParameterPlaceHolder(string oldValueFormatString)
		{
			return this.CreateParameterPlaceholder(oldValueFormatString);
		}

		public string GetOldValueWebParameterName(string oldValueFormatString)
		{
			return this.CreateWebParameterName(oldValueFormatString);
		}

		private DesignerDataConnection _connection;

		private DesignerDataColumn _column;

		private StringCollection _usedNames;

		private string _cachedAliasedName;

		private string _cachedEscapedName;

		private string _cachedParameterPlaceholder;

		private string _cachedWebParameterName;
	}
}
