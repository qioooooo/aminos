using System;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Data.OracleClient
{
	// Token: 0x0200000F RID: 15
	internal abstract class DbSqlParser
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00055A08 File Offset: 0x00054E08
		public DbSqlParser(string quotePrefixCharacter, string quoteSuffixCharacter, string regexPattern)
		{
			this._quotePrefixCharacter = quotePrefixCharacter;
			this._quoteSuffixCharacter = quoteSuffixCharacter;
			DbSqlParser._sqlTokenPattern = regexPattern;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00055A30 File Offset: 0x00054E30
		internal static string CreateRegexPattern(string validIdentifierFirstCharacters, string validIdendifierCharacters, string quotePrefixCharacter, string quotedIdentifierCharacters, string quoteSuffixCharacter, string stringPattern)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("[\\s;]*((?<keyword>all|as|compute|cross|distinct|for|from|full|group|having|intersect|inner|join|left|minus|natural|order|outer|on|right|select|top|union|using|where)\\b|(?<identifier>");
			stringBuilder.Append(validIdentifierFirstCharacters);
			stringBuilder.Append(validIdendifierCharacters);
			stringBuilder.Append("*)|");
			stringBuilder.Append(quotePrefixCharacter);
			stringBuilder.Append("(?<quotedidentifier>");
			stringBuilder.Append(quotedIdentifierCharacters);
			stringBuilder.Append(")");
			stringBuilder.Append(quoteSuffixCharacter);
			stringBuilder.Append("|(?<string>");
			stringBuilder.Append(stringPattern);
			stringBuilder.Append(")|(?<other>.))[\\s;]*");
			return stringBuilder.ToString();
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x060000A1 RID: 161 RVA: 0x00055AC8 File Offset: 0x00054EC8
		internal DbSqlParserColumnCollection Columns
		{
			get
			{
				if (this._columns == null)
				{
					this._columns = new DbSqlParserColumnCollection();
				}
				return this._columns;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x060000A2 RID: 162 RVA: 0x00055AF0 File Offset: 0x00054EF0
		protected virtual string QuotePrefixCharacter
		{
			get
			{
				return this._quotePrefixCharacter;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x060000A3 RID: 163 RVA: 0x00055B04 File Offset: 0x00054F04
		protected virtual string QuoteSuffixCharacter
		{
			get
			{
				return this._quoteSuffixCharacter;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x00055B18 File Offset: 0x00054F18
		private static Regex SqlTokenParser
		{
			get
			{
				Regex regex = DbSqlParser._sqlTokenParser;
				if (regex == null)
				{
					regex = DbSqlParser.GetSqlTokenParser();
				}
				return regex;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x00055B38 File Offset: 0x00054F38
		internal DbSqlParserTableCollection Tables
		{
			get
			{
				if (this._tables == null)
				{
					this._tables = new DbSqlParserTableCollection();
				}
				return this._tables;
			}
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00055B60 File Offset: 0x00054F60
		private void AddColumn(int maxPart, DbSqlParser.Token[] namePart, DbSqlParser.Token aliasName)
		{
			this.Columns.Add(this.GetPart(0, namePart, maxPart), this.GetPart(1, namePart, maxPart), this.GetPart(2, namePart, maxPart), this.GetPart(3, namePart, maxPart), this.GetTokenAsString(aliasName));
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00055BA4 File Offset: 0x00054FA4
		private void AddTable(int maxPart, DbSqlParser.Token[] namePart, DbSqlParser.Token correlationName)
		{
			this.Tables.Add(this.GetPart(1, namePart, maxPart), this.GetPart(2, namePart, maxPart), this.GetPart(3, namePart, maxPart), this.GetTokenAsString(correlationName));
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00055BE0 File Offset: 0x00054FE0
		private void CompleteSchemaInformation()
		{
			DbSqlParserColumnCollection columns = this.Columns;
			DbSqlParserTableCollection tables = this.Tables;
			int num = columns.Count;
			int count = tables.Count;
			for (int i = 0; i < count; i++)
			{
				DbSqlParserTable dbSqlParserTable = tables[i];
				DbSqlParserColumnCollection dbSqlParserColumnCollection = this.GatherTableColumns(dbSqlParserTable);
				dbSqlParserTable.Columns = dbSqlParserColumnCollection;
			}
			for (int j = 0; j < num; j++)
			{
				DbSqlParserColumn dbSqlParserColumn = columns[j];
				DbSqlParserTable dbSqlParserTable2 = this.FindTableForColumn(dbSqlParserColumn);
				if (!dbSqlParserColumn.IsExpression)
				{
					if ("*" == dbSqlParserColumn.ColumnName)
					{
						columns.RemoveAt(j);
						if (dbSqlParserColumn.TableName.Length != 0)
						{
							DbSqlParserColumnCollection columns2 = dbSqlParserTable2.Columns;
							int count2 = columns2.Count;
							for (int k = 0; k < count2; k++)
							{
								columns.Insert(j + k, columns2[k]);
							}
							num += count2 - 1;
							j += count2 - 1;
						}
						else
						{
							for (int l = 0; l < count; l++)
							{
								dbSqlParserTable2 = tables[l];
								DbSqlParserColumnCollection columns3 = dbSqlParserTable2.Columns;
								int count3 = columns3.Count;
								for (int m = 0; m < count3; m++)
								{
									columns.Insert(j + m, columns3[m]);
								}
								num += count3 - 1;
								j += count3;
							}
						}
					}
					else
					{
						DbSqlParserColumn dbSqlParserColumn2 = this.FindCompletedColumn(dbSqlParserTable2, dbSqlParserColumn);
						if (dbSqlParserColumn2 != null)
						{
							dbSqlParserColumn.CopySchemaInfoFrom(dbSqlParserColumn2);
						}
						else
						{
							dbSqlParserColumn.CopySchemaInfoFrom(dbSqlParserTable2);
						}
					}
				}
			}
			for (int n = 0; n < count; n++)
			{
				DbSqlParserTable dbSqlParserTable3 = tables[n];
				this.GatherKeyColumns(dbSqlParserTable3);
			}
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00055D80 File Offset: 0x00055180
		protected DbSqlParserColumn FindCompletedColumn(DbSqlParserTable table, DbSqlParserColumn searchColumn)
		{
			DbSqlParserColumnCollection columns = table.Columns;
			int count = columns.Count;
			for (int i = 0; i < count; i++)
			{
				DbSqlParserColumn dbSqlParserColumn = columns[i];
				if (this.CatalogMatch(dbSqlParserColumn.ColumnName, searchColumn.ColumnName))
				{
					return dbSqlParserColumn;
				}
			}
			return null;
		}

		// Token: 0x060000AA RID: 170 RVA: 0x00055DC8 File Offset: 0x000551C8
		internal DbSqlParserTable FindTableForColumn(DbSqlParserColumn column)
		{
			DbSqlParserTableCollection tables = this.Tables;
			int count = tables.Count;
			for (int i = 0; i < count; i++)
			{
				DbSqlParserTable dbSqlParserTable = tables[i];
				if (ADP.IsEmpty(column.DatabaseName) && ADP.IsEmpty(column.SchemaName) && this.CatalogMatch(column.TableName, dbSqlParserTable.CorrelationName))
				{
					return dbSqlParserTable;
				}
				if ((ADP.IsEmpty(column.DatabaseName) || this.CatalogMatch(column.DatabaseName, dbSqlParserTable.DatabaseName)) && (ADP.IsEmpty(column.SchemaName) || this.CatalogMatch(column.SchemaName, dbSqlParserTable.SchemaName)) && (ADP.IsEmpty(column.TableName) || this.CatalogMatch(column.TableName, dbSqlParserTable.TableName)))
				{
					return dbSqlParserTable;
				}
			}
			return null;
		}

		// Token: 0x060000AB RID: 171 RVA: 0x00055E94 File Offset: 0x00055294
		private string GetPart(int part, DbSqlParser.Token[] namePart, int maxPart)
		{
			int num = maxPart - namePart.Length + part + 1;
			if (0 > num)
			{
				return null;
			}
			return this.GetTokenAsString(namePart[num]);
		}

		// Token: 0x060000AC RID: 172 RVA: 0x00055EC4 File Offset: 0x000552C4
		private static Regex GetSqlTokenParser()
		{
			Regex regex = DbSqlParser._sqlTokenParser;
			if (regex == null)
			{
				regex = new Regex(DbSqlParser._sqlTokenPattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
				DbSqlParser._identifierGroup = regex.GroupNumberFromName("identifier");
				DbSqlParser._quotedidentifierGroup = regex.GroupNumberFromName("quotedidentifier");
				DbSqlParser._keywordGroup = regex.GroupNumberFromName("keyword");
				DbSqlParser._stringGroup = regex.GroupNumberFromName("string");
				DbSqlParser._otherGroup = regex.GroupNumberFromName("other");
				DbSqlParser._sqlTokenParser = regex;
			}
			return regex;
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00055F40 File Offset: 0x00055340
		private string GetTokenAsString(DbSqlParser.Token token)
		{
			if (DbSqlParser.TokenType.QuotedIdentifier == token.Type)
			{
				return this._quotePrefixCharacter + token.Value + this._quoteSuffixCharacter;
			}
			return token.Value;
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00055F78 File Offset: 0x00055378
		public void Parse(string statementText)
		{
			this.Parse2(statementText);
			this.CompleteSchemaInformation();
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00055F94 File Offset: 0x00055394
		private void Parse2(string statementText)
		{
			DbSqlParser.PARSERSTATE parserstate = DbSqlParser.PARSERSTATE.NOTHINGYET;
			DbSqlParser.Token[] array = new DbSqlParser.Token[4];
			int num = 0;
			DbSqlParser.Token token = DbSqlParser.Token.Null;
			DbSqlParser.TokenType tokenType = DbSqlParser.TokenType.Null;
			int num2 = 0;
			this._columns = null;
			this._tables = null;
			Match match = DbSqlParser.SqlTokenParser.Match(statementText);
			DbSqlParser.Token token2 = DbSqlParser.TokenFromMatch(match);
			for (;;)
			{
				bool flag = false;
				switch (parserstate)
				{
				case DbSqlParser.PARSERSTATE.NOTHINGYET:
				{
					DbSqlParser.TokenType type = token2.Type;
					if (type == DbSqlParser.TokenType.Keyword_SELECT)
					{
						parserstate = DbSqlParser.PARSERSTATE.SELECT;
						goto IL_061A;
					}
					goto IL_0095;
				}
				case DbSqlParser.PARSERSTATE.SELECT:
				{
					DbSqlParser.TokenType type2 = token2.Type;
					if (type2 <= DbSqlParser.TokenType.Other_Star)
					{
						switch (type2)
						{
						case DbSqlParser.TokenType.Identifier:
						case DbSqlParser.TokenType.QuotedIdentifier:
							parserstate = DbSqlParser.PARSERSTATE.COLUMN;
							num = 0;
							array[0] = token2;
							goto IL_061A;
						default:
							switch (type2)
							{
							case DbSqlParser.TokenType.Other_LeftParen:
								parserstate = DbSqlParser.PARSERSTATE.EXPRESSION;
								num2++;
								goto IL_061A;
							case DbSqlParser.TokenType.Other_RightParen:
								goto IL_0146;
							case DbSqlParser.TokenType.Other_Star:
								parserstate = DbSqlParser.PARSERSTATE.COLUMNALIAS;
								num = 0;
								array[0] = token2;
								goto IL_061A;
							}
							break;
						}
					}
					else
					{
						if (type2 == DbSqlParser.TokenType.Keyword_ALL)
						{
							goto IL_061A;
						}
						switch (type2)
						{
						case DbSqlParser.TokenType.Keyword_DISTINCT:
							goto IL_061A;
						case DbSqlParser.TokenType.Keyword_FROM:
							parserstate = DbSqlParser.PARSERSTATE.FROM;
							goto IL_061A;
						}
					}
					parserstate = DbSqlParser.PARSERSTATE.EXPRESSION;
					goto IL_061A;
				}
				case DbSqlParser.PARSERSTATE.COLUMN:
				{
					DbSqlParser.TokenType type3 = token2.Type;
					if (type3 <= DbSqlParser.TokenType.Other_Star)
					{
						switch (type3)
						{
						case DbSqlParser.TokenType.Identifier:
						case DbSqlParser.TokenType.QuotedIdentifier:
							if (DbSqlParser.TokenType.Other_Period != tokenType)
							{
								parserstate = DbSqlParser.PARSERSTATE.COLUMNALIAS;
								token = token2;
								goto IL_061A;
							}
							array[++num] = token2;
							goto IL_061A;
						default:
							switch (type3)
							{
							case DbSqlParser.TokenType.Other_Comma:
								break;
							case DbSqlParser.TokenType.Other_Period:
								if (num > 3)
								{
									goto Block_14;
								}
								goto IL_061A;
							case DbSqlParser.TokenType.Other_LeftParen:
								parserstate = DbSqlParser.PARSERSTATE.EXPRESSION;
								num2++;
								num = -1;
								goto IL_061A;
							case DbSqlParser.TokenType.Other_RightParen:
								goto IL_0236;
							case DbSqlParser.TokenType.Other_Star:
								parserstate = DbSqlParser.PARSERSTATE.COLUMNALIAS;
								array[++num] = token2;
								goto IL_061A;
							default:
								goto IL_023C;
							}
							break;
						}
					}
					else
					{
						if (type3 == DbSqlParser.TokenType.Keyword_AS)
						{
							goto IL_061A;
						}
						if (type3 != DbSqlParser.TokenType.Keyword_FROM)
						{
							goto IL_023C;
						}
					}
					parserstate = ((token2.Type == DbSqlParser.TokenType.Keyword_FROM) ? DbSqlParser.PARSERSTATE.FROM : DbSqlParser.PARSERSTATE.SELECT);
					this.AddColumn(num, array, token);
					num = -1;
					token = DbSqlParser.Token.Null;
					goto IL_061A;
					IL_023C:
					parserstate = DbSqlParser.PARSERSTATE.EXPRESSION;
					num = -1;
					goto IL_061A;
				}
				case DbSqlParser.PARSERSTATE.COLUMNALIAS:
				{
					DbSqlParser.TokenType type4 = token2.Type;
					if (type4 == DbSqlParser.TokenType.Other_Comma || type4 == DbSqlParser.TokenType.Keyword_FROM)
					{
						parserstate = ((token2.Type == DbSqlParser.TokenType.Keyword_FROM) ? DbSqlParser.PARSERSTATE.FROM : DbSqlParser.PARSERSTATE.SELECT);
						this.AddColumn(num, array, token);
						num = -1;
						token = DbSqlParser.Token.Null;
						goto IL_061A;
					}
					goto IL_0287;
				}
				case DbSqlParser.PARSERSTATE.TABLE:
				{
					DbSqlParser.TokenType type5 = token2.Type;
					switch (type5)
					{
					case DbSqlParser.TokenType.Null:
						break;
					case DbSqlParser.TokenType.Identifier:
					case DbSqlParser.TokenType.QuotedIdentifier:
						if (DbSqlParser.TokenType.Other_Period != tokenType)
						{
							parserstate = DbSqlParser.PARSERSTATE.TABLEALIAS;
							token = token2;
							goto IL_061A;
						}
						array[++num] = token2;
						goto IL_061A;
					default:
						switch (type5)
						{
						case DbSqlParser.TokenType.Other_Comma:
							break;
						case DbSqlParser.TokenType.Other_Period:
							if (num > 2)
							{
								goto Block_36;
							}
							goto IL_061A;
						default:
							switch (type5)
							{
							case DbSqlParser.TokenType.Keyword_AS:
								goto IL_061A;
							case DbSqlParser.TokenType.Keyword_COMPUTE:
							case DbSqlParser.TokenType.Keyword_FOR:
							case DbSqlParser.TokenType.Keyword_GROUP:
							case DbSqlParser.TokenType.Keyword_HAVING:
							case DbSqlParser.TokenType.Keyword_INTERSECT:
							case DbSqlParser.TokenType.Keyword_MINUS:
							case DbSqlParser.TokenType.Keyword_ORDER:
							case DbSqlParser.TokenType.Keyword_UNION:
							case DbSqlParser.TokenType.Keyword_WHERE:
								goto IL_0534;
							case DbSqlParser.TokenType.Keyword_CROSS:
							case DbSqlParser.TokenType.Keyword_LEFT:
							case DbSqlParser.TokenType.Keyword_NATURAL:
							case DbSqlParser.TokenType.Keyword_RIGHT:
								parserstate = DbSqlParser.PARSERSTATE.JOIN;
								flag = true;
								goto IL_061A;
							case DbSqlParser.TokenType.Keyword_JOIN:
								goto IL_053F;
							case DbSqlParser.TokenType.Keyword_ON:
							case DbSqlParser.TokenType.Keyword_USING:
								parserstate = DbSqlParser.PARSERSTATE.JOINCONDITION;
								flag = true;
								goto IL_061A;
							}
							goto Block_34;
						}
						IL_053F:
						parserstate = DbSqlParser.PARSERSTATE.FROM;
						flag = true;
						goto IL_061A;
					}
					IL_0534:
					parserstate = DbSqlParser.PARSERSTATE.DONE;
					flag = true;
					goto IL_061A;
				}
				case DbSqlParser.PARSERSTATE.TABLEALIAS:
				{
					flag = true;
					DbSqlParser.TokenType type6 = token2.Type;
					if (type6 != DbSqlParser.TokenType.Null)
					{
						if (type6 != DbSqlParser.TokenType.Other_Comma)
						{
							switch (type6)
							{
							case DbSqlParser.TokenType.Keyword_COMPUTE:
							case DbSqlParser.TokenType.Keyword_FOR:
							case DbSqlParser.TokenType.Keyword_GROUP:
							case DbSqlParser.TokenType.Keyword_HAVING:
							case DbSqlParser.TokenType.Keyword_INTERSECT:
							case DbSqlParser.TokenType.Keyword_MINUS:
							case DbSqlParser.TokenType.Keyword_ORDER:
							case DbSqlParser.TokenType.Keyword_UNION:
							case DbSqlParser.TokenType.Keyword_WHERE:
								goto IL_05F1;
							case DbSqlParser.TokenType.Keyword_CROSS:
							case DbSqlParser.TokenType.Keyword_LEFT:
							case DbSqlParser.TokenType.Keyword_NATURAL:
							case DbSqlParser.TokenType.Keyword_RIGHT:
								parserstate = DbSqlParser.PARSERSTATE.JOIN;
								goto IL_061A;
							case DbSqlParser.TokenType.Keyword_JOIN:
								goto IL_05F6;
							case DbSqlParser.TokenType.Keyword_ON:
							case DbSqlParser.TokenType.Keyword_USING:
								parserstate = DbSqlParser.PARSERSTATE.JOINCONDITION;
								goto IL_061A;
							}
							goto Block_39;
						}
						IL_05F6:
						parserstate = DbSqlParser.PARSERSTATE.FROM;
						goto IL_061A;
					}
					IL_05F1:
					parserstate = DbSqlParser.PARSERSTATE.DONE;
					goto IL_061A;
				}
				case DbSqlParser.PARSERSTATE.FROM:
					switch (token2.Type)
					{
					case DbSqlParser.TokenType.Identifier:
					case DbSqlParser.TokenType.QuotedIdentifier:
						parserstate = DbSqlParser.PARSERSTATE.TABLE;
						num = 0;
						array[0] = token2;
						goto IL_061A;
					}
					goto Block_24;
				case DbSqlParser.PARSERSTATE.EXPRESSION:
				{
					DbSqlParser.TokenType type7 = token2.Type;
					switch (type7)
					{
					case DbSqlParser.TokenType.Identifier:
					case DbSqlParser.TokenType.QuotedIdentifier:
						if (num2 == 0)
						{
							token = token2;
							goto IL_061A;
						}
						goto IL_061A;
					default:
						switch (type7)
						{
						case DbSqlParser.TokenType.Other_Comma:
							break;
						case DbSqlParser.TokenType.Other_Period:
							goto IL_061A;
						case DbSqlParser.TokenType.Other_LeftParen:
							num2++;
							goto IL_061A;
						case DbSqlParser.TokenType.Other_RightParen:
							num2--;
							goto IL_061A;
						default:
							if (type7 != DbSqlParser.TokenType.Keyword_FROM)
							{
								goto IL_061A;
							}
							break;
						}
						if (num2 == 0)
						{
							parserstate = ((token2.Type == DbSqlParser.TokenType.Keyword_FROM) ? DbSqlParser.PARSERSTATE.FROM : DbSqlParser.PARSERSTATE.SELECT);
							this.AddColumn(num, array, token);
							num = -1;
							token = DbSqlParser.Token.Null;
							goto IL_061A;
						}
						goto IL_061A;
					}
					break;
				}
				case DbSqlParser.PARSERSTATE.JOIN:
				{
					DbSqlParser.TokenType type8 = token2.Type;
					if (type8 == DbSqlParser.TokenType.Keyword_INNER)
					{
						goto IL_061A;
					}
					if (type8 == DbSqlParser.TokenType.Keyword_JOIN)
					{
						parserstate = DbSqlParser.PARSERSTATE.FROM;
						goto IL_061A;
					}
					if (type8 != DbSqlParser.TokenType.Keyword_OUTER)
					{
						goto Block_27;
					}
					goto IL_061A;
				}
				case DbSqlParser.PARSERSTATE.JOINCONDITION:
					switch (token2.Type)
					{
					case DbSqlParser.TokenType.Other_LeftParen:
						num2++;
						goto IL_061A;
					case DbSqlParser.TokenType.Other_RightParen:
						num2--;
						goto IL_061A;
					default:
						if (num2 == 0)
						{
							DbSqlParser.TokenType type9 = token2.Type;
							if (type9 != DbSqlParser.TokenType.Null)
							{
								switch (type9)
								{
								case DbSqlParser.TokenType.Keyword_COMPUTE:
								case DbSqlParser.TokenType.Keyword_FOR:
								case DbSqlParser.TokenType.Keyword_GROUP:
								case DbSqlParser.TokenType.Keyword_HAVING:
								case DbSqlParser.TokenType.Keyword_INTERSECT:
								case DbSqlParser.TokenType.Keyword_MINUS:
								case DbSqlParser.TokenType.Keyword_ORDER:
								case DbSqlParser.TokenType.Keyword_UNION:
								case DbSqlParser.TokenType.Keyword_WHERE:
									break;
								case DbSqlParser.TokenType.Keyword_CROSS:
								case DbSqlParser.TokenType.Keyword_LEFT:
								case DbSqlParser.TokenType.Keyword_NATURAL:
								case DbSqlParser.TokenType.Keyword_RIGHT:
									parserstate = DbSqlParser.PARSERSTATE.JOIN;
									goto IL_061A;
								case DbSqlParser.TokenType.Keyword_DISTINCT:
								case DbSqlParser.TokenType.Keyword_FROM:
								case DbSqlParser.TokenType.Keyword_FULL:
								case DbSqlParser.TokenType.Keyword_INNER:
								case DbSqlParser.TokenType.Keyword_INTO:
								case DbSqlParser.TokenType.Keyword_ON:
								case DbSqlParser.TokenType.Keyword_OUTER:
								case DbSqlParser.TokenType.Keyword_SELECT:
								case DbSqlParser.TokenType.Keyword_TOP:
								case DbSqlParser.TokenType.Keyword_USING:
									goto IL_061A;
								case DbSqlParser.TokenType.Keyword_JOIN:
									parserstate = DbSqlParser.PARSERSTATE.FROM;
									goto IL_061A;
								default:
									goto IL_061A;
								}
							}
							parserstate = DbSqlParser.PARSERSTATE.DONE;
							goto IL_061A;
						}
						goto IL_061A;
					}
					break;
				case DbSqlParser.PARSERSTATE.DONE:
					return;
				}
				break;
				IL_061A:
				if (flag)
				{
					this.AddTable(num, array, token);
					num = -1;
					token = DbSqlParser.Token.Null;
				}
				tokenType = token2.Type;
				match = match.NextMatch();
				token2 = DbSqlParser.TokenFromMatch(match);
			}
			throw ADP.InvalidOperation(Res.GetString("ADP_SQLParserInternalError"));
			IL_0095:
			throw ADP.InvalidOperation(Res.GetString("ADP_SQLParserInternalError"));
			IL_0146:
			throw ADP.SyntaxErrorMissingParenthesis();
			Block_14:
			throw ADP.SyntaxErrorTooManyNameParts();
			IL_0236:
			throw ADP.SyntaxErrorMissingParenthesis();
			IL_0287:
			throw ADP.SyntaxErrorExpectedCommaAfterColumn();
			Block_24:
			throw ADP.SyntaxErrorExpectedIdentifier();
			Block_27:
			throw ADP.SyntaxErrorExpectedNextPart();
			Block_34:
			goto IL_055F;
			Block_36:
			throw ADP.SyntaxErrorTooManyNameParts();
			IL_055F:
			throw ADP.SyntaxErrorExpectedNextPart();
			Block_39:
			throw ADP.SyntaxErrorExpectedCommaAfterTable();
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000565F4 File Offset: 0x000559F4
		internal static DbSqlParser.Token TokenFromMatch(Match match)
		{
			if (match == null || Match.Empty == match || !match.Success)
			{
				return DbSqlParser.Token.Null;
			}
			if (match.Groups[DbSqlParser._identifierGroup].Success)
			{
				return new DbSqlParser.Token(DbSqlParser.TokenType.Identifier, match.Groups[DbSqlParser._identifierGroup].Value);
			}
			if (match.Groups[DbSqlParser._quotedidentifierGroup].Success)
			{
				return new DbSqlParser.Token(DbSqlParser.TokenType.QuotedIdentifier, match.Groups[DbSqlParser._quotedidentifierGroup].Value);
			}
			if (match.Groups[DbSqlParser._stringGroup].Success)
			{
				return new DbSqlParser.Token(DbSqlParser.TokenType.String, match.Groups[DbSqlParser._stringGroup].Value);
			}
			if (match.Groups[DbSqlParser._otherGroup].Success)
			{
				string text = match.Groups[DbSqlParser._otherGroup].Value.ToLower(CultureInfo.InvariantCulture);
				DbSqlParser.TokenType tokenType = DbSqlParser.TokenType.Other;
				switch (text[0])
				{
				case '(':
					tokenType = DbSqlParser.TokenType.Other_LeftParen;
					break;
				case ')':
					tokenType = DbSqlParser.TokenType.Other_RightParen;
					break;
				case '*':
					tokenType = DbSqlParser.TokenType.Other_Star;
					break;
				case ',':
					tokenType = DbSqlParser.TokenType.Other_Comma;
					break;
				case '.':
					tokenType = DbSqlParser.TokenType.Other_Period;
					break;
				}
				return new DbSqlParser.Token(tokenType, match.Groups[DbSqlParser._otherGroup].Value);
			}
			if (match.Groups[DbSqlParser._keywordGroup].Success)
			{
				string text2 = match.Groups[DbSqlParser._keywordGroup].Value.ToLower(CultureInfo.InvariantCulture);
				int length = text2.Length;
				DbSqlParser.TokenType tokenType2 = DbSqlParser.TokenType.Keyword;
				switch (length)
				{
				case 2:
					if ("as" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_AS;
					}
					else if ("on" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_ON;
					}
					break;
				case 3:
					if ("for" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_FOR;
					}
					else if ("all" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_ALL;
					}
					else if ("top" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_TOP;
					}
					break;
				case 4:
					if ("from" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_FROM;
					}
					else if ("into" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_INTO;
					}
					else if ("join" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_JOIN;
					}
					else if ("left" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_LEFT;
					}
					break;
				case 5:
					if ("where" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_WHERE;
					}
					else if ("group" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_GROUP;
					}
					else if ("order" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_ORDER;
					}
					else if ("right" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_RIGHT;
					}
					else if ("outer" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_OUTER;
					}
					else if ("using" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_USING;
					}
					else if ("cross" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_CROSS;
					}
					else if ("union" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_UNION;
					}
					else if ("minus" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_MINUS;
					}
					else if ("inner" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_INNER;
					}
					break;
				case 6:
					if ("select" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_SELECT;
					}
					else if ("having" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_HAVING;
					}
					break;
				case 7:
					if ("compute" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_COMPUTE;
					}
					else if ("natural" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_NATURAL;
					}
					break;
				case 8:
					if ("distinct" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_DISTINCT;
					}
					break;
				case 9:
					if ("intersect" == text2)
					{
						tokenType2 = DbSqlParser.TokenType.Keyword_INTERSECT;
					}
					break;
				}
				if (DbSqlParser.TokenType.Keyword != tokenType2)
				{
					return new DbSqlParser.Token(tokenType2, text2);
				}
			}
			return DbSqlParser.Token.Null;
		}

		// Token: 0x060000B1 RID: 177
		protected abstract bool CatalogMatch(string valueA, string valueB);

		// Token: 0x060000B2 RID: 178
		protected abstract void GatherKeyColumns(DbSqlParserTable table);

		// Token: 0x060000B3 RID: 179
		protected abstract DbSqlParserColumnCollection GatherTableColumns(DbSqlParserTable table);

		// Token: 0x040000FE RID: 254
		private const string SqlTokenPattern_Part1 = "[\\s;]*((?<keyword>all|as|compute|cross|distinct|for|from|full|group|having|intersect|inner|join|left|minus|natural|order|outer|on|right|select|top|union|using|where)\\b|(?<identifier>";

		// Token: 0x040000FF RID: 255
		private const string SqlTokenPattern_Part2 = "*)|";

		// Token: 0x04000100 RID: 256
		private const string SqlTokenPattern_Part3 = "(?<quotedidentifier>";

		// Token: 0x04000101 RID: 257
		private const string SqlTokenPattern_Part4 = ")";

		// Token: 0x04000102 RID: 258
		private const string SqlTokenPattern_Part5 = "|(?<string>";

		// Token: 0x04000103 RID: 259
		private const string SqlTokenPattern_Part6 = ")|(?<other>.))[\\s;]*";

		// Token: 0x04000104 RID: 260
		private static Regex _sqlTokenParser;

		// Token: 0x04000105 RID: 261
		private static string _sqlTokenPattern;

		// Token: 0x04000106 RID: 262
		private static int _identifierGroup;

		// Token: 0x04000107 RID: 263
		private static int _quotedidentifierGroup;

		// Token: 0x04000108 RID: 264
		private static int _keywordGroup;

		// Token: 0x04000109 RID: 265
		private static int _stringGroup;

		// Token: 0x0400010A RID: 266
		private static int _otherGroup;

		// Token: 0x0400010B RID: 267
		private string _quotePrefixCharacter;

		// Token: 0x0400010C RID: 268
		private string _quoteSuffixCharacter;

		// Token: 0x0400010D RID: 269
		private DbSqlParserColumnCollection _columns;

		// Token: 0x0400010E RID: 270
		private DbSqlParserTableCollection _tables;

		// Token: 0x02000010 RID: 16
		public enum TokenType
		{
			// Token: 0x04000110 RID: 272
			Null,
			// Token: 0x04000111 RID: 273
			Identifier,
			// Token: 0x04000112 RID: 274
			QuotedIdentifier,
			// Token: 0x04000113 RID: 275
			String,
			// Token: 0x04000114 RID: 276
			Other = 100,
			// Token: 0x04000115 RID: 277
			Other_Comma,
			// Token: 0x04000116 RID: 278
			Other_Period,
			// Token: 0x04000117 RID: 279
			Other_LeftParen,
			// Token: 0x04000118 RID: 280
			Other_RightParen,
			// Token: 0x04000119 RID: 281
			Other_Star,
			// Token: 0x0400011A RID: 282
			Keyword = 200,
			// Token: 0x0400011B RID: 283
			Keyword_ALL,
			// Token: 0x0400011C RID: 284
			Keyword_AS,
			// Token: 0x0400011D RID: 285
			Keyword_COMPUTE,
			// Token: 0x0400011E RID: 286
			Keyword_CROSS,
			// Token: 0x0400011F RID: 287
			Keyword_DISTINCT,
			// Token: 0x04000120 RID: 288
			Keyword_FOR,
			// Token: 0x04000121 RID: 289
			Keyword_FROM,
			// Token: 0x04000122 RID: 290
			Keyword_FULL,
			// Token: 0x04000123 RID: 291
			Keyword_GROUP,
			// Token: 0x04000124 RID: 292
			Keyword_HAVING,
			// Token: 0x04000125 RID: 293
			Keyword_INNER,
			// Token: 0x04000126 RID: 294
			Keyword_INTERSECT,
			// Token: 0x04000127 RID: 295
			Keyword_INTO,
			// Token: 0x04000128 RID: 296
			Keyword_JOIN,
			// Token: 0x04000129 RID: 297
			Keyword_LEFT,
			// Token: 0x0400012A RID: 298
			Keyword_MINUS,
			// Token: 0x0400012B RID: 299
			Keyword_NATURAL,
			// Token: 0x0400012C RID: 300
			Keyword_ON,
			// Token: 0x0400012D RID: 301
			Keyword_ORDER,
			// Token: 0x0400012E RID: 302
			Keyword_OUTER,
			// Token: 0x0400012F RID: 303
			Keyword_RIGHT,
			// Token: 0x04000130 RID: 304
			Keyword_SELECT,
			// Token: 0x04000131 RID: 305
			Keyword_TOP,
			// Token: 0x04000132 RID: 306
			Keyword_UNION,
			// Token: 0x04000133 RID: 307
			Keyword_USING,
			// Token: 0x04000134 RID: 308
			Keyword_WHERE
		}

		// Token: 0x02000011 RID: 17
		internal struct Token
		{
			// Token: 0x17000011 RID: 17
			// (get) Token: 0x060000B4 RID: 180 RVA: 0x00056A34 File Offset: 0x00055E34
			internal string Value
			{
				get
				{
					return this._value;
				}
			}

			// Token: 0x17000012 RID: 18
			// (get) Token: 0x060000B5 RID: 181 RVA: 0x00056A48 File Offset: 0x00055E48
			internal DbSqlParser.TokenType Type
			{
				get
				{
					return this._type;
				}
			}

			// Token: 0x060000B6 RID: 182 RVA: 0x00056A5C File Offset: 0x00055E5C
			internal Token(DbSqlParser.TokenType type, string value)
			{
				this._type = type;
				this._value = value;
			}

			// Token: 0x04000135 RID: 309
			private DbSqlParser.TokenType _type;

			// Token: 0x04000136 RID: 310
			private string _value;

			// Token: 0x04000137 RID: 311
			internal static readonly DbSqlParser.Token Null = new DbSqlParser.Token(DbSqlParser.TokenType.Null, null);
		}

		// Token: 0x02000012 RID: 18
		private enum PARSERSTATE
		{
			// Token: 0x04000139 RID: 313
			NOTHINGYET = 1,
			// Token: 0x0400013A RID: 314
			SELECT,
			// Token: 0x0400013B RID: 315
			COLUMN,
			// Token: 0x0400013C RID: 316
			COLUMNALIAS,
			// Token: 0x0400013D RID: 317
			TABLE,
			// Token: 0x0400013E RID: 318
			TABLEALIAS,
			// Token: 0x0400013F RID: 319
			FROM,
			// Token: 0x04000140 RID: 320
			EXPRESSION,
			// Token: 0x04000141 RID: 321
			JOIN,
			// Token: 0x04000142 RID: 322
			JOINCONDITION,
			// Token: 0x04000143 RID: 323
			DONE
		}
	}
}
