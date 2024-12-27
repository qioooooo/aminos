using System;
using System.Collections;
using System.Globalization;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Data.Common
{
	// Token: 0x0200012A RID: 298
	internal class DbConnectionOptions
	{
		// Token: 0x06001385 RID: 4997 RVA: 0x002227D4 File Offset: 0x00221BD4
		public DbConnectionOptions(string connectionString)
			: this(connectionString, null, false)
		{
		}

		// Token: 0x06001386 RID: 4998 RVA: 0x002227EC File Offset: 0x00221BEC
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

		// Token: 0x06001387 RID: 4999 RVA: 0x0022287C File Offset: 0x00221C7C
		protected DbConnectionOptions(DbConnectionOptions connectionOptions)
		{
			this._usersConnectionString = connectionOptions._usersConnectionString;
			this.HasPasswordKeyword = connectionOptions.HasPasswordKeyword;
			this.UseOdbcRules = connectionOptions.UseOdbcRules;
			this._parsetable = connectionOptions._parsetable;
			this.KeyChain = connectionOptions.KeyChain;
		}

		// Token: 0x06001388 RID: 5000 RVA: 0x002228CC File Offset: 0x00221CCC
		public string UsersConnectionString(bool hidePassword)
		{
			return this.UsersConnectionString(hidePassword, false);
		}

		// Token: 0x06001389 RID: 5001 RVA: 0x002228E4 File Offset: 0x00221CE4
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

		// Token: 0x0600138A RID: 5002 RVA: 0x00222924 File Offset: 0x00221D24
		internal string UsersConnectionStringForTrace()
		{
			return this.UsersConnectionString(true, true);
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600138B RID: 5003 RVA: 0x0022293C File Offset: 0x00221D3C
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

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600138C RID: 5004 RVA: 0x00222A14 File Offset: 0x00221E14
		internal bool HasPersistablePassword
		{
			get
			{
				return !this.HasPasswordKeyword || this.ConvertValueToBoolean("persist security info", false);
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600138D RID: 5005 RVA: 0x00222A38 File Offset: 0x00221E38
		public bool IsEmpty
		{
			get
			{
				return null == this.KeyChain;
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x0600138E RID: 5006 RVA: 0x00222A50 File Offset: 0x00221E50
		internal Hashtable Parsetable
		{
			get
			{
				return this._parsetable;
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x0600138F RID: 5007 RVA: 0x00222A64 File Offset: 0x00221E64
		public ICollection Keys
		{
			get
			{
				return this._parsetable.Keys;
			}
		}

		// Token: 0x170002A1 RID: 673
		public string this[string keyword]
		{
			get
			{
				return (string)this._parsetable[keyword];
			}
		}

		// Token: 0x06001391 RID: 5009 RVA: 0x00222A9C File Offset: 0x00221E9C
		internal static void AppendKeyValuePairBuilder(StringBuilder builder, string keyName, string keyValue, bool useOdbcRules)
		{
			ADP.CheckArgumentNull(builder, "builder");
			ADP.CheckArgumentLength(keyName, "keyName");
			if (keyName == null || !DbConnectionOptions.ConnectionStringValidKeyRegex.IsMatch(keyName))
			{
				throw ADP.InvalidKeyname(keyName);
			}
			if (keyValue != null && !DbConnectionOptions.IsValueValidInternal(keyValue))
			{
				throw ADP.InvalidValue(keyName);
			}
			if (0 < builder.Length && ';' != builder[builder.Length - 1])
			{
				builder.Append(";");
			}
			if (useOdbcRules)
			{
				builder.Append(keyName);
			}
			else
			{
				builder.Append(keyName.Replace("=", "=="));
			}
			builder.Append("=");
			if (keyValue != null)
			{
				if (useOdbcRules)
				{
					if (0 < keyValue.Length && ('{' == keyValue[0] || 0 <= keyValue.IndexOf(';') || string.Compare("Driver", keyName, StringComparison.OrdinalIgnoreCase) == 0) && !DbConnectionOptions.ConnectionStringQuoteOdbcValueRegex.IsMatch(keyValue))
					{
						builder.Append('{').Append(keyValue.Replace("}", "}}")).Append('}');
						return;
					}
					builder.Append(keyValue);
					return;
				}
				else
				{
					if (DbConnectionOptions.ConnectionStringQuoteValueRegex.IsMatch(keyValue))
					{
						builder.Append(keyValue);
						return;
					}
					if (-1 != keyValue.IndexOf('"') && -1 == keyValue.IndexOf('\''))
					{
						builder.Append('\'');
						builder.Append(keyValue);
						builder.Append('\'');
						return;
					}
					builder.Append('"');
					builder.Append(keyValue.Replace("\"", "\"\""));
					builder.Append('"');
				}
			}
		}

		// Token: 0x06001392 RID: 5010 RVA: 0x00222C24 File Offset: 0x00222024
		public bool ConvertValueToBoolean(string keyName, bool defaultValue)
		{
			object obj = this._parsetable[keyName];
			if (obj == null)
			{
				return defaultValue;
			}
			return DbConnectionOptions.ConvertValueToBooleanInternal(keyName, (string)obj);
		}

		// Token: 0x06001393 RID: 5011 RVA: 0x00222C50 File Offset: 0x00222050
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

		// Token: 0x06001394 RID: 5012 RVA: 0x00222CDC File Offset: 0x002220DC
		public bool ConvertValueToIntegratedSecurity()
		{
			object obj = this._parsetable["integrated security"];
			return obj != null && this.ConvertValueToIntegratedSecurityInternal((string)obj);
		}

		// Token: 0x06001395 RID: 5013 RVA: 0x00222D0C File Offset: 0x0022210C
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

		// Token: 0x06001396 RID: 5014 RVA: 0x00222DB4 File Offset: 0x002221B4
		public int ConvertValueToInt32(string keyName, int defaultValue)
		{
			object obj = this._parsetable[keyName];
			if (obj == null)
			{
				return defaultValue;
			}
			return DbConnectionOptions.ConvertToInt32Internal(keyName, (string)obj);
		}

		// Token: 0x06001397 RID: 5015 RVA: 0x00222DE0 File Offset: 0x002221E0
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

		// Token: 0x06001398 RID: 5016 RVA: 0x00222E48 File Offset: 0x00222248
		public string ConvertValueToString(string keyName, string defaultValue)
		{
			string text = (string)this._parsetable[keyName];
			if (text == null)
			{
				return defaultValue;
			}
			return text;
		}

		// Token: 0x06001399 RID: 5017 RVA: 0x00222E70 File Offset: 0x00222270
		private static bool CompareInsensitiveInvariant(string strvalue, string strconst)
		{
			return 0 == StringComparer.OrdinalIgnoreCase.Compare(strvalue, strconst);
		}

		// Token: 0x0600139A RID: 5018 RVA: 0x00222E8C File Offset: 0x0022228C
		public bool ContainsKey(string keyword)
		{
			return this._parsetable.ContainsKey(keyword);
		}

		// Token: 0x0600139B RID: 5019 RVA: 0x00222EA8 File Offset: 0x002222A8
		protected internal virtual PermissionSet CreatePermissionSet()
		{
			return null;
		}

		// Token: 0x0600139C RID: 5020 RVA: 0x00222EB8 File Offset: 0x002222B8
		internal void DemandPermission()
		{
			if (this._permissionset == null)
			{
				this._permissionset = this.CreatePermissionSet();
			}
			this._permissionset.Demand();
		}

		// Token: 0x0600139D RID: 5021 RVA: 0x00222EE4 File Offset: 0x002222E4
		protected internal virtual string Expand()
		{
			return this._usersConnectionString;
		}

		// Token: 0x0600139E RID: 5022 RVA: 0x00222EF8 File Offset: 0x002222F8
		internal static string ExpandDataDirectory(string keyword, string value, ref string datadir)
		{
			string text = null;
			if (value != null && value.StartsWith("|datadirectory|", StringComparison.OrdinalIgnoreCase))
			{
				string text2 = datadir;
				if (text2 == null)
				{
					object data = AppDomain.CurrentDomain.GetData("DataDirectory");
					text2 = data as string;
					if (data != null && text2 == null)
					{
						throw ADP.InvalidDataDirectory();
					}
					if (ADP.IsEmpty(text2))
					{
						text2 = AppDomain.CurrentDomain.BaseDirectory;
					}
					if (text2 == null)
					{
						text2 = "";
					}
					datadir = text2;
				}
				int length = "|datadirectory|".Length;
				bool flag = 0 < text2.Length && text2[text2.Length - 1] == '\\';
				bool flag2 = length < value.Length && value[length] == '\\';
				if (!flag && !flag2)
				{
					text = text2 + '\\' + value.Substring(length);
				}
				else if (flag && flag2)
				{
					text = text2 + value.Substring(length + 1);
				}
				else
				{
					text = text2 + value.Substring(length);
				}
				if (!ADP.GetFullPath(text).StartsWith(text2, StringComparison.Ordinal))
				{
					throw ADP.InvalidConnectionOptionValue(keyword);
				}
			}
			return text;
		}

		// Token: 0x0600139F RID: 5023 RVA: 0x00223008 File Offset: 0x00222408
		internal string ExpandDataDirectories(ref string filename, ref int position)
		{
			StringBuilder stringBuilder = new StringBuilder(this._usersConnectionString.Length);
			string text = null;
			int num = 0;
			bool flag = false;
			string text2;
			for (NameValuePair nameValuePair = this.KeyChain; nameValuePair != null; nameValuePair = nameValuePair.Next)
			{
				text2 = nameValuePair.Value;
				if (this.UseOdbcRules)
				{
					string name;
					if ((name = nameValuePair.Name) == null || (!(name == "driver") && !(name == "pwd") && !(name == "uid")))
					{
						text2 = DbConnectionOptions.ExpandDataDirectory(nameValuePair.Name, text2, ref text);
					}
				}
				else
				{
					string name2;
					switch (name2 = nameValuePair.Name)
					{
					case "provider":
					case "data provider":
					case "remote provider":
					case "extended properties":
					case "user id":
					case "password":
					case "uid":
					case "pwd":
						goto IL_0151;
					}
					text2 = DbConnectionOptions.ExpandDataDirectory(nameValuePair.Name, text2, ref text);
				}
				IL_0151:
				if (text2 == null)
				{
					text2 = nameValuePair.Value;
				}
				if (this.UseOdbcRules || "file name" != nameValuePair.Name)
				{
					if (text2 != nameValuePair.Value)
					{
						flag = true;
						DbConnectionOptions.AppendKeyValuePairBuilder(stringBuilder, nameValuePair.Name, text2, this.UseOdbcRules);
						stringBuilder.Append(';');
					}
					else
					{
						stringBuilder.Append(this._usersConnectionString, num, nameValuePair.Length);
					}
				}
				else
				{
					flag = true;
					filename = text2;
					position = stringBuilder.Length;
				}
				num += nameValuePair.Length;
			}
			if (flag)
			{
				text2 = stringBuilder.ToString();
			}
			else
			{
				text2 = null;
			}
			return text2;
		}

		// Token: 0x060013A0 RID: 5024 RVA: 0x00223208 File Offset: 0x00222608
		internal string ExpandKeyword(string keyword, string replacementValue)
		{
			bool flag = false;
			int num = 0;
			StringBuilder stringBuilder = new StringBuilder(this._usersConnectionString.Length);
			for (NameValuePair nameValuePair = this.KeyChain; nameValuePair != null; nameValuePair = nameValuePair.Next)
			{
				if (nameValuePair.Name == keyword && nameValuePair.Value == this[keyword])
				{
					DbConnectionOptions.AppendKeyValuePairBuilder(stringBuilder, nameValuePair.Name, replacementValue, this.UseOdbcRules);
					stringBuilder.Append(';');
					flag = true;
				}
				else
				{
					stringBuilder.Append(this._usersConnectionString, num, nameValuePair.Length);
				}
				num += nameValuePair.Length;
			}
			if (!flag)
			{
				DbConnectionOptions.AppendKeyValuePairBuilder(stringBuilder, keyword, replacementValue, this.UseOdbcRules);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060013A1 RID: 5025 RVA: 0x002232B4 File Offset: 0x002226B4
		private static string GetKeyName(StringBuilder buffer)
		{
			int num = buffer.Length;
			while (0 < num && char.IsWhiteSpace(buffer[num - 1]))
			{
				num--;
			}
			return buffer.ToString(0, num).ToLower(CultureInfo.InvariantCulture);
		}

		// Token: 0x060013A2 RID: 5026 RVA: 0x002232F4 File Offset: 0x002226F4
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

		// Token: 0x060013A3 RID: 5027 RVA: 0x0022334C File Offset: 0x0022274C
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

		// Token: 0x060013A4 RID: 5028 RVA: 0x00223678 File Offset: 0x00222A78
		private static bool IsValueValidInternal(string keyvalue)
		{
			return keyvalue == null || -1 == keyvalue.IndexOf('\0');
		}

		// Token: 0x060013A5 RID: 5029 RVA: 0x00223694 File Offset: 0x00222A94
		private static bool IsKeyNameValid(string keyname)
		{
			return keyname != null && (0 < keyname.Length && ';' != keyname[0] && !char.IsWhiteSpace(keyname[0])) && -1 == keyname.IndexOf('\0');
		}

		// Token: 0x060013A6 RID: 5030 RVA: 0x002236D4 File Offset: 0x00222AD4
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

		// Token: 0x060013A7 RID: 5031 RVA: 0x0022378C File Offset: 0x00222B8C
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

		// Token: 0x060013A8 RID: 5032 RVA: 0x00223894 File Offset: 0x00222C94
		internal static void ValidateKeyValuePair(string keyword, string value)
		{
			if (keyword == null || !DbConnectionOptions.ConnectionStringValidKeyRegex.IsMatch(keyword))
			{
				throw ADP.InvalidKeyname(keyword);
			}
			if (value != null && !DbConnectionOptions.ConnectionStringValidValueRegex.IsMatch(value))
			{
				throw ADP.InvalidValue(keyword);
			}
		}

		// Token: 0x04000BF8 RID: 3064
		private const string ConnectionStringValidKeyPattern = "^(?![;\\s])[^\\p{Cc}]+(?<!\\s)$";

		// Token: 0x04000BF9 RID: 3065
		private const string ConnectionStringValidValuePattern = "^[^\0]*$";

		// Token: 0x04000BFA RID: 3066
		private const string ConnectionStringQuoteValuePattern = "^[^\"'=;\\s\\p{Cc}]*$";

		// Token: 0x04000BFB RID: 3067
		private const string ConnectionStringQuoteOdbcValuePattern = "^\\{([^\\}\0]|\\}\\})*\\}$";

		// Token: 0x04000BFC RID: 3068
		internal const string DataDirectory = "|datadirectory|";

		// Token: 0x04000BFD RID: 3069
		private static readonly Regex ConnectionStringValidKeyRegex = new Regex("^(?![;\\s])[^\\p{Cc}]+(?<!\\s)$", RegexOptions.Compiled);

		// Token: 0x04000BFE RID: 3070
		private static readonly Regex ConnectionStringValidValueRegex = new Regex("^[^\0]*$", RegexOptions.Compiled);

		// Token: 0x04000BFF RID: 3071
		private static readonly Regex ConnectionStringQuoteValueRegex = new Regex("^[^\"'=;\\s\\p{Cc}]*$", RegexOptions.Compiled);

		// Token: 0x04000C00 RID: 3072
		private static readonly Regex ConnectionStringQuoteOdbcValueRegex = new Regex("^\\{([^\\}\0]|\\}\\})*\\}$", RegexOptions.ExplicitCapture | RegexOptions.Compiled);

		// Token: 0x04000C01 RID: 3073
		private readonly string _usersConnectionString;

		// Token: 0x04000C02 RID: 3074
		private readonly Hashtable _parsetable;

		// Token: 0x04000C03 RID: 3075
		internal readonly NameValuePair KeyChain;

		// Token: 0x04000C04 RID: 3076
		internal readonly bool HasPasswordKeyword;

		// Token: 0x04000C05 RID: 3077
		internal readonly bool UseOdbcRules;

		// Token: 0x04000C06 RID: 3078
		private PermissionSet _permissionset;

		// Token: 0x0200012B RID: 299
		private enum ParserState
		{
			// Token: 0x04000C08 RID: 3080
			NothingYet = 1,
			// Token: 0x04000C09 RID: 3081
			Key,
			// Token: 0x04000C0A RID: 3082
			KeyEqual,
			// Token: 0x04000C0B RID: 3083
			KeyEnd,
			// Token: 0x04000C0C RID: 3084
			UnquotedValue,
			// Token: 0x04000C0D RID: 3085
			DoubleQuoteValue,
			// Token: 0x04000C0E RID: 3086
			DoubleQuoteValueQuote,
			// Token: 0x04000C0F RID: 3087
			SingleQuoteValue,
			// Token: 0x04000C10 RID: 3088
			SingleQuoteValueQuote,
			// Token: 0x04000C11 RID: 3089
			BraceQuoteValue,
			// Token: 0x04000C12 RID: 3090
			BraceQuoteValueQuote,
			// Token: 0x04000C13 RID: 3091
			QuotedValueEnd,
			// Token: 0x04000C14 RID: 3092
			NullTermination
		}
	}
}
