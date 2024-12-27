using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020004B2 RID: 1202
	internal static class SqlDataSourceCommandParser
	{
		// Token: 0x06002B88 RID: 11144 RVA: 0x000F0374 File Offset: 0x000EF374
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

		// Token: 0x06002B89 RID: 11145 RVA: 0x000F03B4 File Offset: 0x000EF3B4
		private static bool ConsumeFrom(string s, int startIndex, List<string> parts)
		{
			while (startIndex < s.Length && char.IsWhiteSpace(s, startIndex))
			{
				startIndex++;
			}
			return startIndex + 5 < s.Length && (string.Compare(s, startIndex, "from", 0, 4, StringComparison.OrdinalIgnoreCase) == 0 && char.IsWhiteSpace(s, startIndex + 4)) && SqlDataSourceCommandParser.ConsumeTable(s, startIndex + 5, parts);
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x000F0410 File Offset: 0x000EF410
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

		// Token: 0x06002B8B RID: 11147 RVA: 0x000F04DD File Offset: 0x000EF4DD
		private static bool ConsumeSelect(string s, int startIndex, List<string> parts)
		{
			return s.Length >= 7 && s.ToLowerInvariant().StartsWith("select", StringComparison.Ordinal) && char.IsWhiteSpace(s, 6) && SqlDataSourceCommandParser.ConsumeField(s, startIndex + 7, parts);
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000F0514 File Offset: 0x000EF514
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

		// Token: 0x06002B8D RID: 11149 RVA: 0x000F0558 File Offset: 0x000EF558
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

		// Token: 0x06002B8E RID: 11150 RVA: 0x000F05AC File Offset: 0x000EF5AC
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

		// Token: 0x06002B8F RID: 11151 RVA: 0x000F06D0 File Offset: 0x000EF6D0
		public static string GetLastIdentifierPart(string identifier)
		{
			string[] identifierParts = SqlDataSourceCommandParser.GetIdentifierParts(identifier);
			if (identifierParts == null || identifierParts.Length == 0)
			{
				return null;
			}
			return identifierParts[identifierParts.Length - 1];
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000F06F8 File Offset: 0x000EF6F8
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
