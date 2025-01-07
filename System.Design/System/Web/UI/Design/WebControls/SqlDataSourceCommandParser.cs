using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Web.UI.Design.WebControls
{
	internal static class SqlDataSourceCommandParser
	{
		private static bool ConsumeField(string s, int startIndex, List<string> parts)
		{
			while (startIndex < s.Length && char.IsWhiteSpace(s, startIndex))
			{
				startIndex++;
			}
			string text;
			startIndex = SqlDataSourceCommandParser.ConsumeIdentifier(s, startIndex, out text);
			parts.Add(text);
			return SqlDataSourceCommandParser.ExpectField(s, startIndex, parts);
		}

		private static bool ConsumeFrom(string s, int startIndex, List<string> parts)
		{
			while (startIndex < s.Length && char.IsWhiteSpace(s, startIndex))
			{
				startIndex++;
			}
			return startIndex + 5 < s.Length && (string.Compare(s, startIndex, "from", 0, 4, StringComparison.OrdinalIgnoreCase) == 0 && char.IsWhiteSpace(s, startIndex + 4)) && SqlDataSourceCommandParser.ConsumeTable(s, startIndex + 5, parts);
		}

		private static int ConsumeIdentifier(string s, int startIndex, out string identifier)
		{
			bool flag = false;
			identifier = string.Empty;
			while (startIndex < s.Length)
			{
				if (!flag && s[startIndex] == '[')
				{
					flag = true;
					identifier += s[startIndex];
					startIndex++;
				}
				else if (flag && s[startIndex] == ']')
				{
					flag = false;
					identifier += s[startIndex];
					startIndex++;
				}
				else if (flag)
				{
					identifier += s[startIndex];
					startIndex++;
				}
				else
				{
					if (char.IsWhiteSpace(s, startIndex) || s[startIndex] == ',')
					{
						break;
					}
					identifier += s[startIndex];
					startIndex++;
				}
			}
			return startIndex;
		}

		private static bool ConsumeSelect(string s, int startIndex, List<string> parts)
		{
			return s.Length >= 7 && s.ToLowerInvariant().StartsWith("select", StringComparison.Ordinal) && char.IsWhiteSpace(s, 6) && SqlDataSourceCommandParser.ConsumeField(s, startIndex + 7, parts);
		}

		private static bool ConsumeTable(string s, int startIndex, List<string> parts)
		{
			while (startIndex < s.Length && char.IsWhiteSpace(s, startIndex))
			{
				startIndex++;
			}
			string text;
			startIndex = SqlDataSourceCommandParser.ConsumeIdentifier(s, startIndex, out text);
			parts.Add(text);
			return startIndex == s.Length;
		}

		private static bool ExpectField(string s, int startIndex, List<string> parts)
		{
			while (startIndex < s.Length && char.IsWhiteSpace(s, startIndex))
			{
				startIndex++;
			}
			if (startIndex >= s.Length - 1)
			{
				return false;
			}
			if (s[startIndex] == ',')
			{
				return SqlDataSourceCommandParser.ConsumeField(s, startIndex + 1, parts);
			}
			return SqlDataSourceCommandParser.ConsumeFrom(s, startIndex, parts);
		}

		private static string[] GetIdentifierParts(string identifier)
		{
			bool flag = false;
			StringBuilder stringBuilder = new StringBuilder();
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < identifier.Length; i++)
			{
				char c = identifier[i];
				char c2 = c;
				if (c2 != '.')
				{
					switch (c2)
					{
					case '[':
						if (flag)
						{
							return null;
						}
						flag = true;
						goto IL_00E4;
					case ']':
						if (!flag || (identifier.Length > i + 2 && identifier[i + 1] != '.'))
						{
							return null;
						}
						flag = false;
						goto IL_00E4;
					}
					if (!flag)
					{
						char c3 = c;
						if (c3 <= '*')
						{
							if (c3 == '#' || c3 == '*')
							{
								goto IL_00DB;
							}
						}
						else if (c3 == '@' || c3 == '_')
						{
							goto IL_00DB;
						}
						if (!char.IsLetter(c) && (stringBuilder.Length <= 0 || (c != '$' && !char.IsDigit(c))))
						{
							return null;
						}
					}
					IL_00DB:
					stringBuilder.Append(c);
				}
				else if (flag)
				{
					stringBuilder.Append('.');
				}
				else
				{
					arrayList.Add(stringBuilder.ToString());
					stringBuilder.Length = 0;
				}
				IL_00E4:;
			}
			arrayList.Add(stringBuilder.ToString());
			return (string[])arrayList.ToArray(typeof(string));
		}

		public static string GetLastIdentifierPart(string identifier)
		{
			string[] identifierParts = SqlDataSourceCommandParser.GetIdentifierParts(identifier);
			if (identifierParts == null || identifierParts.Length == 0)
			{
				return null;
			}
			return identifierParts[identifierParts.Length - 1];
		}

		public static string[] ParseSqlString(string sqlString)
		{
			if (string.IsNullOrEmpty(sqlString))
			{
				return null;
			}
			string[] array;
			try
			{
				sqlString = sqlString.Trim();
				List<string> list = new List<string>();
				array = (SqlDataSourceCommandParser.ConsumeSelect(sqlString, 0, list) ? list.ToArray() : null);
			}
			catch (Exception)
			{
				array = null;
			}
			return array;
		}
	}
}
