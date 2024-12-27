using System;
using System.Collections;
using System.Data.OracleClient;
using System.Globalization;
using System.Security;
using System.Text;

namespace System.Data.Common
{
	// Token: 0x0200005B RID: 91
	internal class DbConnectionOptions
	{
		// Token: 0x0600039D RID: 925 RVA: 0x0006277C File Offset: 0x00061B7C
		public DbConnectionOptions(string connectionString)
			: this(connectionString, null, false)
		{
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00062794 File Offset: 0x00061B94
		public DbConnectionOptions(string connectionString, Hashtable synonyms, bool useOdbcRules)
		{
			this.UseOdbcRules = useOdbcRules;
			this._parsetable = new Hashtable();
			this._usersConnectionString = ((connectionString != null) ? connectionString : "");
			if (0 < this._usersConnectionString.Length)
			{
				this.KeyChain = DbConnectionOptions.ParseInternal(this._parsetable, this._usersConnectionString, true, synonyms, this.UseOdbcRules);
				this.HasPasswordKeyword = this._parsetable.ContainsKey("password") || this._parsetable.ContainsKey("pwd");
			}
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00062824 File Offset: 0x00061C24
		public string UsersConnectionString(bool hidePassword)
		{
			return this.UsersConnectionString(hidePassword, false);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x0006283C File Offset: 0x00061C3C
		private string UsersConnectionString(bool hidePassword, bool forceHidePassword)
		{
			string usersConnectionString = this._usersConnectionString;
			if (this.HasPasswordKeyword && (forceHidePassword || (hidePassword && !this.HasPersistablePassword)))
			{
				this.ReplacePasswordPwd(out usersConnectionString, false);
			}
			if (usersConnectionString == null)
			{
				return "";
			}
			return usersConnectionString;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x0006287C File Offset: 0x00061C7C
		internal string UsersConnectionStringForTrace()
		{
			return this.UsersConnectionString(true, true);
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x00062894 File Offset: 0x00061C94
		internal bool HasBlankPassword
		{
			get
			{
				if (this.ConvertValueToIntegratedSecurity())
				{
					return false;
				}
				if (this._parsetable.ContainsKey("password"))
				{
					return ADP.IsEmpty((string)this._parsetable["password"]);
				}
				if (this._parsetable.ContainsKey("pwd"))
				{
					return ADP.IsEmpty((string)this._parsetable["pwd"]);
				}
				return (this._parsetable.ContainsKey("user id") && !ADP.IsEmpty((string)this._parsetable["user id"])) || (this._parsetable.ContainsKey("uid") && !ADP.IsEmpty((string)this._parsetable["uid"]));
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x060003A3 RID: 931 RVA: 0x0006296C File Offset: 0x00061D6C
		internal bool HasPersistablePassword
		{
			get
			{
				return !this.HasPasswordKeyword || this.ConvertValueToBoolean("persist security info", false);
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00062990 File Offset: 0x00061D90
		public bool IsEmpty
		{
			get
			{
				return null == this.KeyChain;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x000629A8 File Offset: 0x00061DA8
		internal Hashtable Parsetable
		{
			get
			{
				return this._parsetable;
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x000629BC File Offset: 0x00061DBC
		public bool ConvertValueToBoolean(string keyName, bool defaultValue)
		{
			object obj = this._parsetable[keyName];
			if (obj == null)
			{
				return defaultValue;
			}
			return DbConnectionOptions.ConvertValueToBooleanInternal(keyName, (string)obj);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x000629E8 File Offset: 0x00061DE8
		internal static bool ConvertValueToBooleanInternal(string keyName, string stringValue)
		{
			if (DbConnectionOptions.CompareInsensitiveInvariant(stringValue, "true") || DbConnectionOptions.CompareInsensitiveInvariant(stringValue, "yes"))
			{
				return true;
			}
			if (DbConnectionOptions.CompareInsensitiveInvariant(stringValue, "false") || DbConnectionOptions.CompareInsensitiveInvariant(stringValue, "no"))
			{
				return false;
			}
			string text = stringValue.Trim();
			if (DbConnectionOptions.CompareInsensitiveInvariant(text, "true") || DbConnectionOptions.CompareInsensitiveInvariant(text, "yes"))
			{
				return true;
			}
			if (DbConnectionOptions.CompareInsensitiveInvariant(text, "false") || DbConnectionOptions.CompareInsensitiveInvariant(text, "no"))
			{
				return false;
			}
			throw ADP.InvalidConnectionOptionValue(keyName);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x00062A74 File Offset: 0x00061E74
		public bool ConvertValueToIntegratedSecurity()
		{
			object obj = this._parsetable["integrated security"];
			return obj != null && this.ConvertValueToIntegratedSecurityInternal((string)obj);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00062AA4 File Offset: 0x00061EA4
		internal bool ConvertValueToIntegratedSecurityInternal(string stringValue)
		{
			if (DbConnectionOptions.CompareInsensitiveInvariant(stringValue, "sspi") || DbConnectionOptions.CompareInsensitiveInvariant(stringValue, "true") || DbConnectionOptions.CompareInsensitiveInvariant(stringValue, "yes"))
			{
				return true;
			}
			if (DbConnectionOptions.CompareInsensitiveInvariant(stringValue, "false") || DbConnectionOptions.CompareInsensitiveInvariant(stringValue, "no"))
			{
				return false;
			}
			string text = stringValue.Trim();
			if (DbConnectionOptions.CompareInsensitiveInvariant(text, "sspi") || DbConnectionOptions.CompareInsensitiveInvariant(text, "true") || DbConnectionOptions.CompareInsensitiveInvariant(text, "yes"))
			{
				return true;
			}
			if (DbConnectionOptions.CompareInsensitiveInvariant(text, "false") || DbConnectionOptions.CompareInsensitiveInvariant(text, "no"))
			{
				return false;
			}
			throw ADP.InvalidConnectionOptionValue("integrated security");
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00062B4C File Offset: 0x00061F4C
		public int ConvertValueToInt32(string keyName, int defaultValue)
		{
			object obj = this._parsetable[keyName];
			if (obj == null)
			{
				return defaultValue;
			}
			return DbConnectionOptions.ConvertToInt32Internal(keyName, (string)obj);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00062B78 File Offset: 0x00061F78
		internal static int ConvertToInt32Internal(string keyname, string stringValue)
		{
			int num;
			try
			{
				num = int.Parse(stringValue, NumberStyles.Integer, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw ADP.InvalidConnectionOptionValue(keyname, ex);
			}
			catch (OverflowException ex2)
			{
				throw ADP.InvalidConnectionOptionValue(keyname, ex2);
			}
			return num;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00062BE0 File Offset: 0x00061FE0
		public string ConvertValueToString(string keyName, string defaultValue)
		{
			string text = (string)this._parsetable[keyName];
			if (text == null)
			{
				return defaultValue;
			}
			return text;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00062C08 File Offset: 0x00062008
		private static bool CompareInsensitiveInvariant(string strvalue, string strconst)
		{
			return 0 == StringComparer.OrdinalIgnoreCase.Compare(strvalue, strconst);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00062C24 File Offset: 0x00062024
		protected internal virtual PermissionSet CreatePermissionSet()
		{
			return null;
		}

		// Token: 0x060003AF RID: 943 RVA: 0x00062C34 File Offset: 0x00062034
		internal void DemandPermission()
		{
			if (this._permissionset == null)
			{
				this._permissionset = this.CreatePermissionSet();
			}
			this._permissionset.Demand();
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00062C60 File Offset: 0x00062060
		protected internal virtual string Expand()
		{
			return this._usersConnectionString;
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x00062C74 File Offset: 0x00062074
		private static string GetKeyName(StringBuilder buffer)
		{
			int num = buffer.Length;
			while (0 < num && char.IsWhiteSpace(buffer[num - 1]))
			{
				num--;
			}
			return buffer.ToString(0, num).ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00062CB4 File Offset: 0x000620B4
		private static string GetKeyValue(StringBuilder buffer, bool trimWhitespace)
		{
			int num = buffer.Length;
			int i = 0;
			if (trimWhitespace)
			{
				while (i < num)
				{
					if (!char.IsWhiteSpace(buffer[i]))
					{
						break;
					}
					i++;
				}
				while (0 < num && char.IsWhiteSpace(buffer[num - 1]))
				{
					num--;
				}
			}
			return buffer.ToString(i, num - i);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00062D0C File Offset: 0x0006210C
		internal static int GetKeyValuePair(string connectionString, int currentPosition, StringBuilder buffer, bool useOdbcRules, out string keyname, out string keyvalue)
		{
			int num = currentPosition;
			buffer.Length = 0;
			keyname = null;
			keyvalue = null;
			char c = '\0';
			DbConnectionOptions.ParserState parserState = DbConnectionOptions.ParserState.NothingYet;
			int length = connectionString.Length;
			while (currentPosition < length)
			{
				c = connectionString[currentPosition];
				switch (parserState)
				{
				case DbConnectionOptions.ParserState.NothingYet:
					if (';' != c && !char.IsWhiteSpace(c))
					{
						if (c == '\0')
						{
							parserState = DbConnectionOptions.ParserState.NullTermination;
						}
						else
						{
							if (char.IsControl(c))
							{
								throw ADP.ConnectionStringSyntax(num);
							}
							num = currentPosition;
							if ('=' != c)
							{
								parserState = DbConnectionOptions.ParserState.Key;
								goto IL_024D;
							}
							parserState = DbConnectionOptions.ParserState.KeyEqual;
						}
					}
					break;
				case DbConnectionOptions.ParserState.Key:
					if ('=' == c)
					{
						parserState = DbConnectionOptions.ParserState.KeyEqual;
					}
					else
					{
						if (!char.IsWhiteSpace(c) && char.IsControl(c))
						{
							throw ADP.ConnectionStringSyntax(num);
						}
						goto IL_024D;
					}
					break;
				case DbConnectionOptions.ParserState.KeyEqual:
					if (!useOdbcRules && '=' == c)
					{
						parserState = DbConnectionOptions.ParserState.Key;
						goto IL_024D;
					}
					keyname = DbConnectionOptions.GetKeyName(buffer);
					if (ADP.IsEmpty(keyname))
					{
						throw ADP.ConnectionStringSyntax(num);
					}
					buffer.Length = 0;
					parserState = DbConnectionOptions.ParserState.KeyEnd;
					goto IL_010C;
				case DbConnectionOptions.ParserState.KeyEnd:
					goto IL_010C;
				case DbConnectionOptions.ParserState.UnquotedValue:
					if (char.IsWhiteSpace(c))
					{
						goto IL_024D;
					}
					if (char.IsControl(c))
					{
						goto IL_0262;
					}
					if (';' == c)
					{
						goto IL_0262;
					}
					goto IL_024D;
				case DbConnectionOptions.ParserState.DoubleQuoteValue:
					if ('"' == c)
					{
						parserState = DbConnectionOptions.ParserState.DoubleQuoteValueQuote;
					}
					else
					{
						if (c == '\0')
						{
							throw ADP.ConnectionStringSyntax(num);
						}
						goto IL_024D;
					}
					break;
				case DbConnectionOptions.ParserState.DoubleQuoteValueQuote:
					if ('"' == c)
					{
						parserState = DbConnectionOptions.ParserState.DoubleQuoteValue;
						goto IL_024D;
					}
					keyvalue = DbConnectionOptions.GetKeyValue(buffer, false);
					parserState = DbConnectionOptions.ParserState.QuotedValueEnd;
					goto IL_0217;
				case DbConnectionOptions.ParserState.SingleQuoteValue:
					if ('\'' == c)
					{
						parserState = DbConnectionOptions.ParserState.SingleQuoteValueQuote;
					}
					else
					{
						if (c == '\0')
						{
							throw ADP.ConnectionStringSyntax(num);
						}
						goto IL_024D;
					}
					break;
				case DbConnectionOptions.ParserState.SingleQuoteValueQuote:
					if ('\'' == c)
					{
						parserState = DbConnectionOptions.ParserState.SingleQuoteValue;
						goto IL_024D;
					}
					keyvalue = DbConnectionOptions.GetKeyValue(buffer, false);
					parserState = DbConnectionOptions.ParserState.QuotedValueEnd;
					goto IL_0217;
				case DbConnectionOptions.ParserState.BraceQuoteValue:
					if ('}' == c)
					{
						parserState = DbConnectionOptions.ParserState.BraceQuoteValueQuote;
						goto IL_024D;
					}
					if (c == '\0')
					{
						throw ADP.ConnectionStringSyntax(num);
					}
					goto IL_024D;
				case DbConnectionOptions.ParserState.BraceQuoteValueQuote:
					if ('}' == c)
					{
						parserState = DbConnectionOptions.ParserState.BraceQuoteValue;
						goto IL_024D;
					}
					keyvalue = DbConnectionOptions.GetKeyValue(buffer, false);
					parserState = DbConnectionOptions.ParserState.QuotedValueEnd;
					goto IL_0217;
				case DbConnectionOptions.ParserState.QuotedValueEnd:
					goto IL_0217;
				case DbConnectionOptions.ParserState.NullTermination:
					if (c != '\0' && !char.IsWhiteSpace(c))
					{
						throw ADP.ConnectionStringSyntax(currentPosition);
					}
					break;
				default:
					throw ADP.InternalError(ADP.InternalErrorCode.InvalidParserState1);
				}
				IL_0255:
				currentPosition++;
				continue;
				IL_010C:
				if (char.IsWhiteSpace(c))
				{
					goto IL_0255;
				}
				if (useOdbcRules)
				{
					if ('{' == c)
					{
						parserState = DbConnectionOptions.ParserState.BraceQuoteValue;
						goto IL_024D;
					}
				}
				else
				{
					if ('\'' == c)
					{
						parserState = DbConnectionOptions.ParserState.SingleQuoteValue;
						goto IL_0255;
					}
					if ('"' == c)
					{
						parserState = DbConnectionOptions.ParserState.DoubleQuoteValue;
						goto IL_0255;
					}
				}
				if (';' == c || c == '\0')
				{
					break;
				}
				if (char.IsControl(c))
				{
					throw ADP.ConnectionStringSyntax(num);
				}
				parserState = DbConnectionOptions.ParserState.UnquotedValue;
				goto IL_024D;
				IL_0217:
				if (char.IsWhiteSpace(c))
				{
					goto IL_0255;
				}
				if (';' == c)
				{
					break;
				}
				if (c == '\0')
				{
					parserState = DbConnectionOptions.ParserState.NullTermination;
					goto IL_0255;
				}
				throw ADP.ConnectionStringSyntax(num);
				IL_024D:
				buffer.Append(c);
				goto IL_0255;
			}
			IL_0262:
			switch (parserState)
			{
			case DbConnectionOptions.ParserState.NothingYet:
			case DbConnectionOptions.ParserState.KeyEnd:
			case DbConnectionOptions.ParserState.NullTermination:
				break;
			case DbConnectionOptions.ParserState.Key:
			case DbConnectionOptions.ParserState.DoubleQuoteValue:
			case DbConnectionOptions.ParserState.SingleQuoteValue:
			case DbConnectionOptions.ParserState.BraceQuoteValue:
				throw ADP.ConnectionStringSyntax(num);
			case DbConnectionOptions.ParserState.KeyEqual:
				keyname = DbConnectionOptions.GetKeyName(buffer);
				if (ADP.IsEmpty(keyname))
				{
					throw ADP.ConnectionStringSyntax(num);
				}
				break;
			case DbConnectionOptions.ParserState.UnquotedValue:
			{
				keyvalue = DbConnectionOptions.GetKeyValue(buffer, true);
				char c2 = keyvalue[keyvalue.Length - 1];
				if (!useOdbcRules && ('\'' == c2 || '"' == c2))
				{
					throw ADP.ConnectionStringSyntax(num);
				}
				break;
			}
			case DbConnectionOptions.ParserState.DoubleQuoteValueQuote:
			case DbConnectionOptions.ParserState.SingleQuoteValueQuote:
			case DbConnectionOptions.ParserState.BraceQuoteValueQuote:
			case DbConnectionOptions.ParserState.QuotedValueEnd:
				keyvalue = DbConnectionOptions.GetKeyValue(buffer, false);
				break;
			default:
				throw ADP.InternalError(ADP.InternalErrorCode.InvalidParserState2);
			}
			if (';' == c && currentPosition < connectionString.Length)
			{
				currentPosition++;
			}
			return currentPosition;
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00063038 File Offset: 0x00062438
		private static bool IsValueValidInternal(string keyvalue)
		{
			return keyvalue == null || -1 == keyvalue.IndexOf('\0');
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00063054 File Offset: 0x00062454
		private static bool IsKeyNameValid(string keyname)
		{
			return keyname != null && (0 < keyname.Length && ';' != keyname[0] && !char.IsWhiteSpace(keyname[0])) && -1 == keyname.IndexOf('\0');
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00063094 File Offset: 0x00062494
		private static NameValuePair ParseInternal(Hashtable parsetable, string connectionString, bool buildChain, Hashtable synonyms, bool firstKey)
		{
			StringBuilder stringBuilder = new StringBuilder();
			NameValuePair nameValuePair = null;
			NameValuePair nameValuePair2 = null;
			int i = 0;
			int length = connectionString.Length;
			while (i < length)
			{
				int num = i;
				string text;
				string text2;
				i = DbConnectionOptions.GetKeyValuePair(connectionString, num, stringBuilder, firstKey, out text, out text2);
				if (ADP.IsEmpty(text))
				{
					break;
				}
				string text3 = ((synonyms != null) ? ((string)synonyms[text]) : text);
				if (!DbConnectionOptions.IsKeyNameValid(text3))
				{
					throw ADP.KeywordNotSupported(text);
				}
				if (!firstKey || !parsetable.Contains(text3))
				{
					parsetable[text3] = text2;
				}
				if (nameValuePair != null)
				{
					nameValuePair = (nameValuePair.Next = new NameValuePair(text3, text2, i - num));
				}
				else if (buildChain)
				{
					nameValuePair = (nameValuePair2 = new NameValuePair(text3, text2, i - num));
				}
			}
			return nameValuePair2;
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x0006314C File Offset: 0x0006254C
		internal NameValuePair ReplacePasswordPwd(out string constr, bool fakePassword)
		{
			int num = 0;
			NameValuePair nameValuePair = null;
			NameValuePair nameValuePair2 = null;
			NameValuePair nameValuePair3 = null;
			StringBuilder stringBuilder = new StringBuilder(this._usersConnectionString.Length);
			for (NameValuePair nameValuePair4 = this.KeyChain; nameValuePair4 != null; nameValuePair4 = nameValuePair4.Next)
			{
				if ("password" != nameValuePair4.Name && "pwd" != nameValuePair4.Name)
				{
					stringBuilder.Append(this._usersConnectionString, num, nameValuePair4.Length);
					if (fakePassword)
					{
						nameValuePair3 = new NameValuePair(nameValuePair4.Name, nameValuePair4.Value, nameValuePair4.Length);
					}
				}
				else if (fakePassword)
				{
					stringBuilder.Append(nameValuePair4.Name).Append("=*;");
					nameValuePair3 = new NameValuePair(nameValuePair4.Name, "*", nameValuePair4.Name.Length + "=*;".Length);
				}
				if (fakePassword)
				{
					if (nameValuePair2 != null)
					{
						nameValuePair2 = (nameValuePair2.Next = nameValuePair3);
					}
					else
					{
						nameValuePair = (nameValuePair2 = nameValuePair3);
					}
				}
				num += nameValuePair4.Length;
			}
			constr = stringBuilder.ToString();
			return nameValuePair;
		}

		// Token: 0x040003BE RID: 958
		private const string ConnectionStringValidKeyPattern = "^(?![;\\s])[^\\p{Cc}]+(?<!\\s)$";

		// Token: 0x040003BF RID: 959
		private const string ConnectionStringValidValuePattern = "^[^\0]*$";

		// Token: 0x040003C0 RID: 960
		private const string ConnectionStringQuoteValuePattern = "^[^\"'=;\\s\\p{Cc}]*$";

		// Token: 0x040003C1 RID: 961
		private const string ConnectionStringQuoteOdbcValuePattern = "^\\{([^\\}\0]|\\}\\})*\\}$";

		// Token: 0x040003C2 RID: 962
		internal const string DataDirectory = "|datadirectory|";

		// Token: 0x040003C3 RID: 963
		private readonly string _usersConnectionString;

		// Token: 0x040003C4 RID: 964
		private readonly Hashtable _parsetable;

		// Token: 0x040003C5 RID: 965
		internal readonly NameValuePair KeyChain;

		// Token: 0x040003C6 RID: 966
		internal readonly bool HasPasswordKeyword;

		// Token: 0x040003C7 RID: 967
		internal readonly bool UseOdbcRules;

		// Token: 0x040003C8 RID: 968
		private PermissionSet _permissionset;

		// Token: 0x0200005C RID: 92
		private enum ParserState
		{
			// Token: 0x040003CA RID: 970
			NothingYet = 1,
			// Token: 0x040003CB RID: 971
			Key,
			// Token: 0x040003CC RID: 972
			KeyEqual,
			// Token: 0x040003CD RID: 973
			KeyEnd,
			// Token: 0x040003CE RID: 974
			UnquotedValue,
			// Token: 0x040003CF RID: 975
			DoubleQuoteValue,
			// Token: 0x040003D0 RID: 976
			DoubleQuoteValueQuote,
			// Token: 0x040003D1 RID: 977
			SingleQuoteValue,
			// Token: 0x040003D2 RID: 978
			SingleQuoteValueQuote,
			// Token: 0x040003D3 RID: 979
			BraceQuoteValue,
			// Token: 0x040003D4 RID: 980
			BraceQuoteValueQuote,
			// Token: 0x040003D5 RID: 981
			QuotedValueEnd,
			// Token: 0x040003D6 RID: 982
			NullTermination
		}
	}
}
