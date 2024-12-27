using System;
using System.IO;
using System.Text;

namespace System.Security.Util
{
	// Token: 0x02000601 RID: 1537
	internal sealed class Parser
	{
		// Token: 0x06003818 RID: 14360 RVA: 0x000BD431 File Offset: 0x000BC431
		internal SecurityElement GetTopElement()
		{
			if (!this.ParsedSuccessfully())
			{
				throw new XmlSyntaxException(this._t.LineNo);
			}
			return this._doc.GetRootElement();
		}

		// Token: 0x06003819 RID: 14361 RVA: 0x000BD457 File Offset: 0x000BC457
		internal bool ParsedSuccessfully()
		{
			return true;
		}

		// Token: 0x0600381A RID: 14362 RVA: 0x000BD45C File Offset: 0x000BC45C
		private void GetRequiredSizes(TokenizerStream stream, ref int index)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			int num = 1;
			SecurityElementType securityElementType = SecurityElementType.Regular;
			string text = null;
			bool flag5 = false;
			bool flag6 = false;
			int num2 = 0;
			for (;;)
			{
				short num3 = stream.GetNextToken();
				while (num3 != -1)
				{
					switch (num3 & 255)
					{
					case 0:
						flag4 = true;
						flag6 = false;
						num3 = stream.GetNextToken();
						if (num3 == 2)
						{
							stream.TagLastToken(17408);
							for (;;)
							{
								num3 = stream.GetNextToken();
								if (num3 != 3)
								{
									break;
								}
								stream.ThrowAwayNextString();
								stream.TagLastToken(20480);
							}
							if (num3 == -1)
							{
								goto Block_9;
							}
							if (num3 != 1)
							{
								goto Block_10;
							}
							flag4 = false;
							index++;
							flag6 = false;
							num--;
							flag = true;
							goto IL_03BD;
						}
						else if (num3 == 3)
						{
							flag3 = true;
							stream.TagLastToken(16640);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							if (securityElementType != SecurityElementType.Regular)
							{
								goto Block_12;
							}
							flag = true;
							num++;
							goto IL_03BD;
						}
						else
						{
							if (num3 == 6)
							{
								num2 = 1;
								do
								{
									num3 = stream.GetNextToken();
									switch (num3)
									{
									case 0:
										num2++;
										break;
									case 1:
										num2--;
										break;
									case 3:
										stream.ThrowAwayNextString();
										stream.TagLastToken(20480);
										break;
									}
								}
								while (num2 > 0);
								flag4 = false;
								flag6 = false;
								flag = true;
								goto IL_03BD;
							}
							if (num3 != 5)
							{
								goto IL_02B7;
							}
							num3 = stream.GetNextToken();
							if (num3 != 3)
							{
								goto Block_17;
							}
							flag3 = true;
							securityElementType = SecurityElementType.Format;
							stream.TagLastToken(16640);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							num2 = 1;
							num++;
							flag = true;
							goto IL_03BD;
						}
						break;
					case 1:
						if (flag4)
						{
							flag4 = false;
							goto IL_03C8;
						}
						goto IL_02E4;
					case 2:
						num3 = stream.GetNextToken();
						if (num3 == 1)
						{
							stream.TagLastToken(17408);
							index++;
							num--;
							flag6 = false;
							flag = true;
							goto IL_03BD;
						}
						goto IL_032D;
					case 3:
						if (flag4)
						{
							if (securityElementType == SecurityElementType.Comment)
							{
								stream.ThrowAwayNextString();
								stream.TagLastToken(20480);
								goto IL_03BD;
							}
							if (text == null)
							{
								text = stream.GetNextString();
								goto IL_03BD;
							}
							if (!flag5)
							{
								goto Block_5;
							}
							stream.TagLastToken(16896);
							index += SecurityDocument.EncodedStringSize(text) + SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							text = null;
							flag5 = false;
							goto IL_03BD;
						}
						else
						{
							if (flag6)
							{
								stream.TagLastToken(25344);
								index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + SecurityDocument.EncodedStringSize(" ");
								goto IL_03BD;
							}
							stream.TagLastToken(17152);
							index += SecurityDocument.EncodedStringSize(stream.GetNextString()) + 1;
							flag6 = true;
							goto IL_03BD;
						}
						break;
					case 4:
						flag5 = true;
						goto IL_03BD;
					case 5:
						if (!flag4 || securityElementType != SecurityElementType.Format || num2 != 1)
						{
							goto IL_039B;
						}
						num3 = stream.GetNextToken();
						if (num3 == 1)
						{
							stream.TagLastToken(17408);
							index++;
							num--;
							flag6 = false;
							flag = true;
							goto IL_03BD;
						}
						goto IL_0380;
					}
					goto Block_1;
					IL_03C8:
					num3 = stream.GetNextToken();
					continue;
					IL_03BD:
					if (flag)
					{
						flag = false;
						flag2 = false;
						break;
					}
					flag2 = true;
					goto IL_03C8;
				}
				if (flag2)
				{
					index++;
					num--;
					flag6 = false;
				}
				else if (num3 == -1 && (num != 1 || !flag3))
				{
					goto IL_03F9;
				}
				if (num <= 1)
				{
					return;
				}
			}
			Block_1:
			goto IL_03AC;
			Block_5:
			throw new XmlSyntaxException(this._t.LineNo);
			Block_9:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
			Block_10:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_ExpectedCloseBracket"));
			Block_12:
			throw new XmlSyntaxException(this._t.LineNo);
			Block_17:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_02B7:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_ExpectedSlashOrString"));
			IL_02E4:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedCloseBracket"));
			IL_032D:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_ExpectedCloseBracket"));
			IL_0380:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_ExpectedCloseBracket"));
			IL_039B:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_03AC:
			throw new XmlSyntaxException(this._t.LineNo);
			IL_03F9:
			throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
		}

		// Token: 0x0600381B RID: 14363 RVA: 0x000BD888 File Offset: 0x000BC888
		private int DetermineFormat(TokenizerStream stream)
		{
			if (stream.GetNextToken() == 0 && stream.GetNextToken() == 5)
			{
				this._t.GetTokens(stream, -1, true);
				stream.GoToPosition(2);
				bool flag = false;
				bool flag2 = false;
				short num = stream.GetNextToken();
				while (num != -1 && num != 1)
				{
					switch (num)
					{
					case 3:
						if (flag && flag2)
						{
							this._t.ChangeFormat(Encoding.GetEncoding(stream.GetNextString()));
							return 0;
						}
						if (!flag)
						{
							if (string.Compare(stream.GetNextString(), "encoding", StringComparison.Ordinal) == 0)
							{
								flag2 = true;
							}
						}
						else
						{
							flag = false;
							flag2 = false;
							stream.ThrowAwayNextString();
						}
						break;
					case 4:
						flag = true;
						break;
					default:
						throw new XmlSyntaxException(this._t.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
					}
					num = stream.GetNextToken();
				}
				return 0;
			}
			return 2;
		}

		// Token: 0x0600381C RID: 14364 RVA: 0x000BD95C File Offset: 0x000BC95C
		private void ParseContents()
		{
			TokenizerStream tokenizerStream = new TokenizerStream();
			this._t.GetTokens(tokenizerStream, 2, false);
			tokenizerStream.Reset();
			int num = this.DetermineFormat(tokenizerStream);
			tokenizerStream.GoToPosition(num);
			this._t.GetTokens(tokenizerStream, -1, false);
			tokenizerStream.Reset();
			int num2 = 0;
			this.GetRequiredSizes(tokenizerStream, ref num2);
			this._doc = new SecurityDocument(num2);
			int num3 = 0;
			tokenizerStream.Reset();
			for (short num4 = tokenizerStream.GetNextFullToken(); num4 != -1; num4 = tokenizerStream.GetNextFullToken())
			{
				if ((num4 & 16384) == 16384)
				{
					short num5 = (short)((int)num4 & 65280);
					if (num5 <= 17152)
					{
						if (num5 == 16640)
						{
							this._doc.AddToken(1, ref num3);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num3);
							goto IL_019D;
						}
						if (num5 == 16896)
						{
							this._doc.AddToken(2, ref num3);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num3);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num3);
							goto IL_019D;
						}
						if (num5 == 17152)
						{
							this._doc.AddToken(3, ref num3);
							this._doc.AddString(tokenizerStream.GetNextString(), ref num3);
							goto IL_019D;
						}
					}
					else
					{
						if (num5 == 17408)
						{
							this._doc.AddToken(4, ref num3);
							goto IL_019D;
						}
						if (num5 == 20480)
						{
							tokenizerStream.ThrowAwayNextString();
							goto IL_019D;
						}
						if (num5 == 25344)
						{
							this._doc.AppendString(" ", ref num3);
							this._doc.AppendString(tokenizerStream.GetNextString(), ref num3);
							goto IL_019D;
						}
					}
					throw new XmlSyntaxException();
				}
				IL_019D:;
			}
		}

		// Token: 0x0600381D RID: 14365 RVA: 0x000BDB14 File Offset: 0x000BCB14
		private Parser(Tokenizer t)
		{
			this._t = t;
			this._doc = null;
			try
			{
				this.ParseContents();
			}
			finally
			{
				this._t.Recycle();
			}
		}

		// Token: 0x0600381E RID: 14366 RVA: 0x000BDB5C File Offset: 0x000BCB5C
		internal Parser(string input)
			: this(new Tokenizer(input))
		{
		}

		// Token: 0x0600381F RID: 14367 RVA: 0x000BDB6A File Offset: 0x000BCB6A
		internal Parser(string input, string[] searchStrings, string[] replaceStrings)
			: this(new Tokenizer(input, searchStrings, replaceStrings))
		{
		}

		// Token: 0x06003820 RID: 14368 RVA: 0x000BDB7A File Offset: 0x000BCB7A
		internal Parser(byte[] array, Tokenizer.ByteTokenEncoding encoding)
			: this(new Tokenizer(array, encoding, 0))
		{
		}

		// Token: 0x06003821 RID: 14369 RVA: 0x000BDB8A File Offset: 0x000BCB8A
		internal Parser(byte[] array, Tokenizer.ByteTokenEncoding encoding, int startIndex)
			: this(new Tokenizer(array, encoding, startIndex))
		{
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x000BDB9A File Offset: 0x000BCB9A
		internal Parser(StreamReader input)
			: this(new Tokenizer(input))
		{
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x000BDBA8 File Offset: 0x000BCBA8
		internal Parser(char[] array)
			: this(new Tokenizer(array))
		{
		}

		// Token: 0x04001CDF RID: 7391
		private const short c_flag = 16384;

		// Token: 0x04001CE0 RID: 7392
		private const short c_elementtag = 16640;

		// Token: 0x04001CE1 RID: 7393
		private const short c_attributetag = 16896;

		// Token: 0x04001CE2 RID: 7394
		private const short c_texttag = 17152;

		// Token: 0x04001CE3 RID: 7395
		private const short c_additionaltexttag = 25344;

		// Token: 0x04001CE4 RID: 7396
		private const short c_childrentag = 17408;

		// Token: 0x04001CE5 RID: 7397
		private const short c_wastedstringtag = 20480;

		// Token: 0x04001CE6 RID: 7398
		private SecurityDocument _doc;

		// Token: 0x04001CE7 RID: 7399
		private Tokenizer _t;
	}
}
