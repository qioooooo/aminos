using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace System.Security.Util
{
	// Token: 0x02000602 RID: 1538
	internal sealed class Tokenizer
	{
		// Token: 0x06003824 RID: 14372 RVA: 0x000BDBB8 File Offset: 0x000BCBB8
		internal void BasicInitialization()
		{
			this.LineNo = 1;
			this._inProcessingTag = 0;
			this._inSavedCharacter = -1;
			this._inIndex = 0;
			this._inSize = 0;
			this._inNestedSize = 0;
			this._inNestedIndex = 0;
			this._inTokenSource = Tokenizer.TokenSource.Other;
			this._maker = SharedStatics.GetSharedStringMaker();
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x000BDC08 File Offset: 0x000BCC08
		public void Recycle()
		{
			SharedStatics.ReleaseSharedStringMaker(ref this._maker);
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x000BDC15 File Offset: 0x000BCC15
		internal Tokenizer(string input)
		{
			this.BasicInitialization();
			this._inString = input;
			this._inSize = input.Length;
			this._inTokenSource = Tokenizer.TokenSource.String;
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x000BDC3D File Offset: 0x000BCC3D
		internal Tokenizer(string input, string[] searchStrings, string[] replaceStrings)
		{
			this.BasicInitialization();
			this._inString = input;
			this._inSize = this._inString.Length;
			this._inTokenSource = Tokenizer.TokenSource.NestedStrings;
			this._searchStrings = searchStrings;
			this._replaceStrings = replaceStrings;
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x000BDC78 File Offset: 0x000BCC78
		internal Tokenizer(byte[] array, Tokenizer.ByteTokenEncoding encoding, int startIndex)
		{
			this.BasicInitialization();
			this._inBytes = array;
			this._inSize = array.Length;
			this._inIndex = startIndex;
			switch (encoding)
			{
			case Tokenizer.ByteTokenEncoding.UnicodeTokens:
				this._inTokenSource = Tokenizer.TokenSource.UnicodeByteArray;
				return;
			case Tokenizer.ByteTokenEncoding.UTF8Tokens:
				this._inTokenSource = Tokenizer.TokenSource.UTF8ByteArray;
				return;
			case Tokenizer.ByteTokenEncoding.ByteTokens:
				this._inTokenSource = Tokenizer.TokenSource.ASCIIByteArray;
				return;
			default:
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Arg_EnumIllegalVal"), new object[] { (int)encoding }));
			}
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x000BDD00 File Offset: 0x000BCD00
		internal Tokenizer(char[] array)
		{
			this.BasicInitialization();
			this._inChars = array;
			this._inSize = array.Length;
			this._inTokenSource = Tokenizer.TokenSource.CharArray;
		}

		// Token: 0x0600382A RID: 14378 RVA: 0x000BDD25 File Offset: 0x000BCD25
		internal Tokenizer(StreamReader input)
		{
			this.BasicInitialization();
			this._inTokenReader = new Tokenizer.StreamTokenReader(input);
		}

		// Token: 0x0600382B RID: 14379 RVA: 0x000BDD40 File Offset: 0x000BCD40
		internal void ChangeFormat(Encoding encoding)
		{
			if (encoding == null)
			{
				return;
			}
			switch (this._inTokenSource)
			{
			case Tokenizer.TokenSource.UnicodeByteArray:
			case Tokenizer.TokenSource.UTF8ByteArray:
			case Tokenizer.TokenSource.ASCIIByteArray:
				if (encoding == Encoding.Unicode)
				{
					this._inTokenSource = Tokenizer.TokenSource.UnicodeByteArray;
					return;
				}
				if (encoding == Encoding.UTF8)
				{
					this._inTokenSource = Tokenizer.TokenSource.UTF8ByteArray;
					return;
				}
				if (encoding != Encoding.ASCII)
				{
					goto IL_005B;
				}
				this._inTokenSource = Tokenizer.TokenSource.ASCIIByteArray;
				break;
			case Tokenizer.TokenSource.CharArray:
			case Tokenizer.TokenSource.String:
			case Tokenizer.TokenSource.NestedStrings:
				break;
			default:
				goto IL_005B;
			}
			return;
			IL_005B:
			Stream stream;
			switch (this._inTokenSource)
			{
			case Tokenizer.TokenSource.UnicodeByteArray:
			case Tokenizer.TokenSource.UTF8ByteArray:
			case Tokenizer.TokenSource.ASCIIByteArray:
				stream = new MemoryStream(this._inBytes, this._inIndex, this._inSize - this._inIndex);
				break;
			case Tokenizer.TokenSource.CharArray:
			case Tokenizer.TokenSource.String:
			case Tokenizer.TokenSource.NestedStrings:
				return;
			default:
			{
				Tokenizer.StreamTokenReader streamTokenReader = this._inTokenReader as Tokenizer.StreamTokenReader;
				if (streamTokenReader == null)
				{
					return;
				}
				stream = streamTokenReader._in.BaseStream;
				string text = new string(' ', streamTokenReader.NumCharEncountered);
				stream.Position = (long)streamTokenReader._in.CurrentEncoding.GetByteCount(text);
				break;
			}
			}
			this._inTokenReader = new Tokenizer.StreamTokenReader(new StreamReader(stream, encoding));
			this._inTokenSource = Tokenizer.TokenSource.Other;
		}

		// Token: 0x0600382C RID: 14380 RVA: 0x000BDE50 File Offset: 0x000BCE50
		internal void GetTokens(TokenizerStream stream, int maxNum, bool endAfterKet)
		{
			while (maxNum == -1 || stream.GetTokenCount() < maxNum)
			{
				int num = 0;
				bool flag = false;
				bool flag2 = false;
				Tokenizer.StringMaker maker = this._maker;
				maker._outStringBuilder = null;
				maker._outIndex = 0;
				int num2;
				for (;;)
				{
					if (this._inSavedCharacter != -1)
					{
						num2 = this._inSavedCharacter;
						this._inSavedCharacter = -1;
					}
					else
					{
						switch (this._inTokenSource)
						{
						case Tokenizer.TokenSource.UnicodeByteArray:
							if (this._inIndex + 1 >= this._inSize)
							{
								goto Block_3;
							}
							num2 = ((int)this._inBytes[this._inIndex + 1] << 8) + (int)this._inBytes[this._inIndex];
							this._inIndex += 2;
							break;
						case Tokenizer.TokenSource.UTF8ByteArray:
							if (this._inIndex >= this._inSize)
							{
								goto Block_4;
							}
							num2 = (int)this._inBytes[this._inIndex++];
							if ((num2 & 128) != 0)
							{
								switch ((num2 & 240) >> 4)
								{
								case 8:
								case 9:
								case 10:
								case 11:
									goto IL_012C;
								case 12:
								case 13:
									num2 &= 31;
									num = 2;
									break;
								case 14:
									num2 &= 15;
									num = 3;
									break;
								case 15:
									goto IL_014A;
								}
								if (this._inIndex >= this._inSize)
								{
									goto Block_7;
								}
								byte b = this._inBytes[this._inIndex++];
								if ((b & 192) != 128)
								{
									goto Block_8;
								}
								num2 = (num2 << 6) | (int)(b & 63);
								if (num != 2)
								{
									if (this._inIndex >= this._inSize)
									{
										goto Block_10;
									}
									b = this._inBytes[this._inIndex++];
									if ((b & 192) != 128)
									{
										goto Block_11;
									}
									num2 = (num2 << 6) | (int)(b & 63);
								}
							}
							break;
						case Tokenizer.TokenSource.ASCIIByteArray:
							if (this._inIndex >= this._inSize)
							{
								goto Block_12;
							}
							num2 = (int)this._inBytes[this._inIndex++];
							break;
						case Tokenizer.TokenSource.CharArray:
							if (this._inIndex >= this._inSize)
							{
								goto Block_13;
							}
							num2 = (int)this._inChars[this._inIndex++];
							break;
						case Tokenizer.TokenSource.String:
							if (this._inIndex >= this._inSize)
							{
								goto Block_14;
							}
							num2 = (int)this._inString[this._inIndex++];
							break;
						case Tokenizer.TokenSource.NestedStrings:
							if (this._inNestedSize != 0)
							{
								if (this._inNestedIndex < this._inNestedSize)
								{
									num2 = (int)this._inNestedString[this._inNestedIndex++];
									break;
								}
								this._inNestedSize = 0;
							}
							if (this._inIndex >= this._inSize)
							{
								goto Block_17;
							}
							num2 = (int)this._inString[this._inIndex++];
							if (num2 == 123)
							{
								for (int i = 0; i < this._searchStrings.Length; i++)
								{
									if (string.Compare(this._searchStrings[i], 0, this._inString, this._inIndex - 1, this._searchStrings[i].Length, StringComparison.Ordinal) == 0)
									{
										this._inNestedString = this._replaceStrings[i];
										this._inNestedSize = this._inNestedString.Length;
										this._inNestedIndex = 1;
										num2 = (int)this._inNestedString[0];
										this._inIndex += this._searchStrings[i].Length - 1;
										break;
									}
								}
							}
							break;
						default:
							num2 = this._inTokenReader.Read();
							if (num2 == -1)
							{
								goto Block_21;
							}
							break;
						}
					}
					if (!flag)
					{
						int num3 = num2;
						if (num3 <= 34)
						{
							switch (num3)
							{
							case 9:
							case 13:
								continue;
							case 10:
								this.LineNo++;
								continue;
							case 11:
							case 12:
								break;
							default:
								switch (num3)
								{
								case 32:
									continue;
								case 33:
									if (this._inProcessingTag != 0)
									{
										goto Block_31;
									}
									break;
								case 34:
									flag = true;
									flag2 = true;
									continue;
								}
								break;
							}
						}
						else
						{
							switch (num3)
							{
							case 45:
								if (this._inProcessingTag != 0)
								{
									goto Block_32;
								}
								break;
							case 46:
								break;
							case 47:
								if (this._inProcessingTag != 0)
								{
									goto Block_29;
								}
								break;
							default:
								switch (num3)
								{
								case 60:
									goto IL_0492;
								case 61:
									goto IL_04C8;
								case 62:
									goto IL_04AC;
								case 63:
									if (this._inProcessingTag != 0)
									{
										goto Block_30;
									}
									break;
								}
								break;
							}
						}
					}
					else
					{
						int num4 = num2;
						if (num4 <= 34)
						{
							switch (num4)
							{
							case 9:
							case 13:
								break;
							case 10:
								this.LineNo++;
								if (!flag2)
								{
									goto Block_43;
								}
								goto IL_0650;
							case 11:
							case 12:
								goto IL_0650;
							default:
								switch (num4)
								{
								case 32:
									break;
								case 33:
									goto IL_0650;
								case 34:
									if (flag2)
									{
										goto Block_41;
									}
									goto IL_0650;
								default:
									goto IL_0650;
								}
								break;
							}
							if (!flag2)
							{
								goto Block_42;
							}
						}
						else
						{
							if (num4 != 47)
							{
								switch (num4)
								{
								case 60:
									if (!flag2)
									{
										goto Block_38;
									}
									goto IL_0650;
								case 61:
								case 62:
									break;
								default:
									goto IL_0650;
								}
							}
							if (!flag2 && this._inProcessingTag != 0)
							{
								goto Block_40;
							}
						}
					}
					IL_0650:
					flag = true;
					if (maker._outIndex < 512)
					{
						maker._outChars[maker._outIndex++] = (char)num2;
					}
					else
					{
						if (maker._outStringBuilder == null)
						{
							maker._outStringBuilder = new StringBuilder();
						}
						maker._outStringBuilder.Append(maker._outChars, 0, 512);
						maker._outChars[0] = (char)num2;
						maker._outIndex = 1;
					}
				}
				IL_0492:
				this._inProcessingTag++;
				stream.AddToken(0);
				continue;
				Block_3:
				stream.AddToken(-1);
				return;
				IL_04AC:
				this._inProcessingTag--;
				stream.AddToken(1);
				if (endAfterKet)
				{
					return;
				}
				continue;
				IL_04C8:
				stream.AddToken(4);
				continue;
				Block_29:
				stream.AddToken(2);
				continue;
				Block_30:
				stream.AddToken(5);
				continue;
				Block_31:
				stream.AddToken(6);
				continue;
				Block_32:
				stream.AddToken(7);
				continue;
				Block_38:
				this._inSavedCharacter = num2;
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_40:
				this._inSavedCharacter = num2;
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_41:
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_42:
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_43:
				stream.AddToken(3);
				stream.AddString(this.GetStringToken());
				continue;
				Block_4:
				stream.AddToken(-1);
				return;
				IL_012C:
				throw new XmlSyntaxException(this.LineNo);
				IL_014A:
				throw new XmlSyntaxException(this.LineNo);
				Block_7:
				throw new XmlSyntaxException(this.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
				Block_8:
				throw new XmlSyntaxException(this.LineNo);
				Block_10:
				throw new XmlSyntaxException(this.LineNo, Environment.GetResourceString("XMLSyntax_UnexpectedEndOfFile"));
				Block_11:
				throw new XmlSyntaxException(this.LineNo);
				Block_12:
				stream.AddToken(-1);
				return;
				Block_13:
				stream.AddToken(-1);
				return;
				Block_14:
				stream.AddToken(-1);
				return;
				Block_17:
				stream.AddToken(-1);
				return;
				Block_21:
				stream.AddToken(-1);
				return;
			}
		}

		// Token: 0x0600382D RID: 14381 RVA: 0x000BE53A File Offset: 0x000BD53A
		private string GetStringToken()
		{
			return this._maker.MakeString();
		}

		// Token: 0x04001CE8 RID: 7400
		internal const byte bra = 0;

		// Token: 0x04001CE9 RID: 7401
		internal const byte ket = 1;

		// Token: 0x04001CEA RID: 7402
		internal const byte slash = 2;

		// Token: 0x04001CEB RID: 7403
		internal const byte cstr = 3;

		// Token: 0x04001CEC RID: 7404
		internal const byte equals = 4;

		// Token: 0x04001CED RID: 7405
		internal const byte quest = 5;

		// Token: 0x04001CEE RID: 7406
		internal const byte bang = 6;

		// Token: 0x04001CEF RID: 7407
		internal const byte dash = 7;

		// Token: 0x04001CF0 RID: 7408
		internal const int intOpenBracket = 60;

		// Token: 0x04001CF1 RID: 7409
		internal const int intCloseBracket = 62;

		// Token: 0x04001CF2 RID: 7410
		internal const int intSlash = 47;

		// Token: 0x04001CF3 RID: 7411
		internal const int intEquals = 61;

		// Token: 0x04001CF4 RID: 7412
		internal const int intQuote = 34;

		// Token: 0x04001CF5 RID: 7413
		internal const int intQuest = 63;

		// Token: 0x04001CF6 RID: 7414
		internal const int intBang = 33;

		// Token: 0x04001CF7 RID: 7415
		internal const int intDash = 45;

		// Token: 0x04001CF8 RID: 7416
		internal const int intTab = 9;

		// Token: 0x04001CF9 RID: 7417
		internal const int intCR = 13;

		// Token: 0x04001CFA RID: 7418
		internal const int intLF = 10;

		// Token: 0x04001CFB RID: 7419
		internal const int intSpace = 32;

		// Token: 0x04001CFC RID: 7420
		public int LineNo;

		// Token: 0x04001CFD RID: 7421
		private int _inProcessingTag;

		// Token: 0x04001CFE RID: 7422
		private byte[] _inBytes;

		// Token: 0x04001CFF RID: 7423
		private char[] _inChars;

		// Token: 0x04001D00 RID: 7424
		private string _inString;

		// Token: 0x04001D01 RID: 7425
		private int _inIndex;

		// Token: 0x04001D02 RID: 7426
		private int _inSize;

		// Token: 0x04001D03 RID: 7427
		private int _inSavedCharacter;

		// Token: 0x04001D04 RID: 7428
		private Tokenizer.TokenSource _inTokenSource;

		// Token: 0x04001D05 RID: 7429
		private Tokenizer.ITokenReader _inTokenReader;

		// Token: 0x04001D06 RID: 7430
		private Tokenizer.StringMaker _maker;

		// Token: 0x04001D07 RID: 7431
		private string[] _searchStrings;

		// Token: 0x04001D08 RID: 7432
		private string[] _replaceStrings;

		// Token: 0x04001D09 RID: 7433
		private int _inNestedIndex;

		// Token: 0x04001D0A RID: 7434
		private int _inNestedSize;

		// Token: 0x04001D0B RID: 7435
		private string _inNestedString;

		// Token: 0x02000603 RID: 1539
		private enum TokenSource
		{
			// Token: 0x04001D0D RID: 7437
			UnicodeByteArray,
			// Token: 0x04001D0E RID: 7438
			UTF8ByteArray,
			// Token: 0x04001D0F RID: 7439
			ASCIIByteArray,
			// Token: 0x04001D10 RID: 7440
			CharArray,
			// Token: 0x04001D11 RID: 7441
			String,
			// Token: 0x04001D12 RID: 7442
			NestedStrings,
			// Token: 0x04001D13 RID: 7443
			Other
		}

		// Token: 0x02000604 RID: 1540
		internal enum ByteTokenEncoding
		{
			// Token: 0x04001D15 RID: 7445
			UnicodeTokens,
			// Token: 0x04001D16 RID: 7446
			UTF8Tokens,
			// Token: 0x04001D17 RID: 7447
			ByteTokens
		}

		// Token: 0x02000605 RID: 1541
		[Serializable]
		internal sealed class StringMaker
		{
			// Token: 0x0600382E RID: 14382 RVA: 0x000BE548 File Offset: 0x000BD548
			private static uint HashString(string str)
			{
				uint num = 0U;
				int length = str.Length;
				for (int i = 0; i < length; i++)
				{
					num = (num << 3) ^ (uint)str[i] ^ (num >> 29);
				}
				return num;
			}

			// Token: 0x0600382F RID: 14383 RVA: 0x000BE57C File Offset: 0x000BD57C
			private static uint HashCharArray(char[] a, int l)
			{
				uint num = 0U;
				for (int i = 0; i < l; i++)
				{
					num = (num << 3) ^ (uint)a[i] ^ (num >> 29);
				}
				return num;
			}

			// Token: 0x06003830 RID: 14384 RVA: 0x000BE5A5 File Offset: 0x000BD5A5
			public StringMaker()
			{
				this.cStringsMax = 2048U;
				this.cStringsUsed = 0U;
				this.aStrings = new string[this.cStringsMax];
				this._outChars = new char[512];
			}

			// Token: 0x06003831 RID: 14385 RVA: 0x000BE5E4 File Offset: 0x000BD5E4
			private bool CompareStringAndChars(string str, char[] a, int l)
			{
				if (str.Length != l)
				{
					return false;
				}
				for (int i = 0; i < l; i++)
				{
					if (a[i] != str[i])
					{
						return false;
					}
				}
				return true;
			}

			// Token: 0x06003832 RID: 14386 RVA: 0x000BE618 File Offset: 0x000BD618
			public string MakeString()
			{
				char[] outChars = this._outChars;
				int outIndex = this._outIndex;
				if (this._outStringBuilder != null)
				{
					this._outStringBuilder.Append(this._outChars, 0, this._outIndex);
					return this._outStringBuilder.ToString();
				}
				uint num3;
				if (this.cStringsUsed > this.cStringsMax / 4U * 3U)
				{
					uint num = this.cStringsMax * 2U;
					string[] array = new string[num];
					int num2 = 0;
					while ((long)num2 < (long)((ulong)this.cStringsMax))
					{
						if (this.aStrings[num2] != null)
						{
							num3 = Tokenizer.StringMaker.HashString(this.aStrings[num2]) % num;
							while (array[(int)((UIntPtr)num3)] != null)
							{
								if ((num3 += 1U) >= num)
								{
									num3 = 0U;
								}
							}
							array[(int)((UIntPtr)num3)] = this.aStrings[num2];
						}
						num2++;
					}
					this.cStringsMax = num;
					this.aStrings = array;
				}
				num3 = Tokenizer.StringMaker.HashCharArray(outChars, outIndex) % this.cStringsMax;
				string text;
				while ((text = this.aStrings[(int)((UIntPtr)num3)]) != null)
				{
					if (this.CompareStringAndChars(text, outChars, outIndex))
					{
						return text;
					}
					if ((num3 += 1U) >= this.cStringsMax)
					{
						num3 = 0U;
					}
				}
				text = new string(outChars, 0, outIndex);
				this.aStrings[(int)((UIntPtr)num3)] = text;
				this.cStringsUsed += 1U;
				return text;
			}

			// Token: 0x04001D18 RID: 7448
			public const int outMaxSize = 512;

			// Token: 0x04001D19 RID: 7449
			private string[] aStrings;

			// Token: 0x04001D1A RID: 7450
			private uint cStringsMax;

			// Token: 0x04001D1B RID: 7451
			private uint cStringsUsed;

			// Token: 0x04001D1C RID: 7452
			public StringBuilder _outStringBuilder;

			// Token: 0x04001D1D RID: 7453
			public char[] _outChars;

			// Token: 0x04001D1E RID: 7454
			public int _outIndex;
		}

		// Token: 0x02000606 RID: 1542
		internal interface ITokenReader
		{
			// Token: 0x06003833 RID: 14387
			int Read();
		}

		// Token: 0x02000607 RID: 1543
		internal class StreamTokenReader : Tokenizer.ITokenReader
		{
			// Token: 0x06003834 RID: 14388 RVA: 0x000BE749 File Offset: 0x000BD749
			internal StreamTokenReader(StreamReader input)
			{
				this._in = input;
				this._numCharRead = 0;
			}

			// Token: 0x06003835 RID: 14389 RVA: 0x000BE760 File Offset: 0x000BD760
			public virtual int Read()
			{
				int num = this._in.Read();
				if (num != -1)
				{
					this._numCharRead++;
				}
				return num;
			}

			// Token: 0x1700097D RID: 2429
			// (get) Token: 0x06003836 RID: 14390 RVA: 0x000BE78C File Offset: 0x000BD78C
			internal int NumCharEncountered
			{
				get
				{
					return this._numCharRead;
				}
			}

			// Token: 0x04001D1F RID: 7455
			internal StreamReader _in;

			// Token: 0x04001D20 RID: 7456
			internal int _numCharRead;
		}
	}
}
