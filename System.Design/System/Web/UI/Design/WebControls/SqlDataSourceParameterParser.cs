using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	internal static class SqlDataSourceParameterParser
	{
		public static Parameter[] ParseCommandText(string providerName, string commandText)
		{
			if (string.IsNullOrEmpty(providerName))
			{
				providerName = "System.Data.SqlClient";
			}
			if (string.IsNullOrEmpty(commandText))
			{
				commandText = string.Empty;
			}
			SqlDataSourceParameterParser.ParameterParser parameterParser = null;
			string text;
			if ((text = providerName.ToLowerInvariant()) != null)
			{
				if (!(text == "system.data.sqlclient"))
				{
					if (!(text == "system.data.odbc") && !(text == "system.data.oledb"))
					{
						if (text == "system.data.oracleclient")
						{
							parameterParser = new SqlDataSourceParameterParser.OracleClientParameterParser();
						}
					}
					else
					{
						parameterParser = new SqlDataSourceParameterParser.MiscParameterParser();
					}
				}
				else
				{
					parameterParser = new SqlDataSourceParameterParser.SqlClientParameterParser();
				}
			}
			if (parameterParser == null)
			{
				return new Parameter[0];
			}
			return parameterParser.ParseCommandText(commandText);
		}

		private abstract class ParameterParser
		{
			public abstract Parameter[] ParseCommandText(string commandText);
		}

		private sealed class SqlClientParameterParser : SqlDataSourceParameterParser.ParameterParser
		{
			private static bool IsValidParamNameChar(char c)
			{
				return char.IsLetterOrDigit(c) || c == '@' || c == '$' || c == '#' || c == '_';
			}

			public override Parameter[] ParseCommandText(string commandText)
			{
				int i = 0;
				int length = commandText.Length;
				SqlDataSourceParameterParser.SqlClientParameterParser.State state = SqlDataSourceParameterParser.SqlClientParameterParser.State.InText;
				List<Parameter> list = new List<Parameter>();
				StringCollection stringCollection = new StringCollection();
				while (i < length)
				{
					switch (state)
					{
					case SqlDataSourceParameterParser.SqlClientParameterParser.State.InText:
						if (commandText[i] == '\'')
						{
							state = SqlDataSourceParameterParser.SqlClientParameterParser.State.InQuote;
						}
						else if (commandText[i] == '"')
						{
							state = SqlDataSourceParameterParser.SqlClientParameterParser.State.InDoubleQuote;
						}
						else if (commandText[i] == '[')
						{
							state = SqlDataSourceParameterParser.SqlClientParameterParser.State.InBracket;
						}
						else if (commandText[i] == '@')
						{
							state = SqlDataSourceParameterParser.SqlClientParameterParser.State.InParameter;
						}
						else
						{
							i++;
						}
						break;
					case SqlDataSourceParameterParser.SqlClientParameterParser.State.InQuote:
						i++;
						while (i < length && commandText[i] != '\'')
						{
							i++;
						}
						i++;
						state = SqlDataSourceParameterParser.SqlClientParameterParser.State.InText;
						break;
					case SqlDataSourceParameterParser.SqlClientParameterParser.State.InDoubleQuote:
						i++;
						while (i < length && commandText[i] != '"')
						{
							i++;
						}
						i++;
						state = SqlDataSourceParameterParser.SqlClientParameterParser.State.InText;
						break;
					case SqlDataSourceParameterParser.SqlClientParameterParser.State.InBracket:
						i++;
						while (i < length && commandText[i] != ']')
						{
							i++;
						}
						i++;
						state = SqlDataSourceParameterParser.SqlClientParameterParser.State.InText;
						break;
					case SqlDataSourceParameterParser.SqlClientParameterParser.State.InParameter:
					{
						i++;
						string text = string.Empty;
						while (i < length && SqlDataSourceParameterParser.SqlClientParameterParser.IsValidParamNameChar(commandText[i]))
						{
							text += commandText[i];
							i++;
						}
						if (!text.StartsWith("@", StringComparison.Ordinal))
						{
							Parameter parameter = new Parameter(text);
							if (!stringCollection.Contains(text))
							{
								list.Add(parameter);
								stringCollection.Add(text);
							}
						}
						state = SqlDataSourceParameterParser.SqlClientParameterParser.State.InText;
						break;
					}
					}
				}
				return list.ToArray();
			}

			private enum State
			{
				InText,
				InQuote,
				InDoubleQuote,
				InBracket,
				InParameter
			}
		}

		private sealed class MiscParameterParser : SqlDataSourceParameterParser.ParameterParser
		{
			public override Parameter[] ParseCommandText(string commandText)
			{
				int i = 0;
				int length = commandText.Length;
				SqlDataSourceParameterParser.MiscParameterParser.State state = SqlDataSourceParameterParser.MiscParameterParser.State.InText;
				List<Parameter> list = new List<Parameter>();
				while (i < length)
				{
					switch (state)
					{
					case SqlDataSourceParameterParser.MiscParameterParser.State.InText:
						if (commandText[i] == '\'')
						{
							state = SqlDataSourceParameterParser.MiscParameterParser.State.InQuote;
						}
						else if (commandText[i] == '"')
						{
							state = SqlDataSourceParameterParser.MiscParameterParser.State.InDoubleQuote;
						}
						else if (commandText[i] == '[')
						{
							state = SqlDataSourceParameterParser.MiscParameterParser.State.InBracket;
						}
						else if (commandText[i] == '?')
						{
							state = SqlDataSourceParameterParser.MiscParameterParser.State.InQuestion;
						}
						else
						{
							i++;
						}
						break;
					case SqlDataSourceParameterParser.MiscParameterParser.State.InQuote:
						i++;
						while (i < length && commandText[i] != '\'')
						{
							i++;
						}
						i++;
						state = SqlDataSourceParameterParser.MiscParameterParser.State.InText;
						break;
					case SqlDataSourceParameterParser.MiscParameterParser.State.InDoubleQuote:
						i++;
						while (i < length && commandText[i] != '"')
						{
							i++;
						}
						i++;
						state = SqlDataSourceParameterParser.MiscParameterParser.State.InText;
						break;
					case SqlDataSourceParameterParser.MiscParameterParser.State.InBracket:
						i++;
						while (i < length && commandText[i] != ']')
						{
							i++;
						}
						i++;
						state = SqlDataSourceParameterParser.MiscParameterParser.State.InText;
						break;
					case SqlDataSourceParameterParser.MiscParameterParser.State.InQuestion:
						i++;
						list.Add(new Parameter("?"));
						state = SqlDataSourceParameterParser.MiscParameterParser.State.InText;
						break;
					}
				}
				return list.ToArray();
			}

			private enum State
			{
				InText,
				InQuote,
				InDoubleQuote,
				InBracket,
				InQuestion
			}
		}

		private sealed class OracleClientParameterParser : SqlDataSourceParameterParser.ParameterParser
		{
			private static bool IsValidParamNameChar(char c)
			{
				return char.IsLetterOrDigit(c) || c == '_';
			}

			public override Parameter[] ParseCommandText(string commandText)
			{
				int i = 0;
				int length = commandText.Length;
				SqlDataSourceParameterParser.OracleClientParameterParser.State state = SqlDataSourceParameterParser.OracleClientParameterParser.State.InText;
				List<Parameter> list = new List<Parameter>();
				StringCollection stringCollection = new StringCollection();
				while (i < length)
				{
					switch (state)
					{
					case SqlDataSourceParameterParser.OracleClientParameterParser.State.InText:
						if (commandText[i] == '\'')
						{
							state = SqlDataSourceParameterParser.OracleClientParameterParser.State.InQuote;
						}
						else if (commandText[i] == '"')
						{
							state = SqlDataSourceParameterParser.OracleClientParameterParser.State.InDoubleQuote;
						}
						else if (commandText[i] == '[')
						{
							state = SqlDataSourceParameterParser.OracleClientParameterParser.State.InBracket;
						}
						else if (commandText[i] == ':')
						{
							state = SqlDataSourceParameterParser.OracleClientParameterParser.State.InParameter;
						}
						else
						{
							i++;
						}
						break;
					case SqlDataSourceParameterParser.OracleClientParameterParser.State.InQuote:
						i++;
						while (i < length && commandText[i] != '\'')
						{
							i++;
						}
						i++;
						state = SqlDataSourceParameterParser.OracleClientParameterParser.State.InText;
						break;
					case SqlDataSourceParameterParser.OracleClientParameterParser.State.InDoubleQuote:
						i++;
						while (i < length && commandText[i] != '"')
						{
							i++;
						}
						i++;
						state = SqlDataSourceParameterParser.OracleClientParameterParser.State.InText;
						break;
					case SqlDataSourceParameterParser.OracleClientParameterParser.State.InBracket:
						i++;
						while (i < length && commandText[i] != ']')
						{
							i++;
						}
						i++;
						state = SqlDataSourceParameterParser.OracleClientParameterParser.State.InText;
						break;
					case SqlDataSourceParameterParser.OracleClientParameterParser.State.InParameter:
					{
						i++;
						string text = string.Empty;
						while (i < length && SqlDataSourceParameterParser.OracleClientParameterParser.IsValidParamNameChar(commandText[i]))
						{
							text += commandText[i];
							i++;
						}
						Parameter parameter = new Parameter(text);
						if (!stringCollection.Contains(text))
						{
							list.Add(parameter);
							stringCollection.Add(text);
						}
						state = SqlDataSourceParameterParser.OracleClientParameterParser.State.InText;
						break;
					}
					}
				}
				return list.ToArray();
			}

			private enum State
			{
				InText,
				InQuote,
				InDoubleQuote,
				InBracket,
				InParameter
			}
		}
	}
}
