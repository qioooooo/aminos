using System;
using System.Text;

namespace System.Globalization
{
	// Token: 0x0200000F RID: 15
	internal class PosixFormatConverter
	{
		// Token: 0x060000DB RID: 219 RVA: 0x0000A36F File Offset: 0x0000936F
		internal PosixFormatConverter()
		{
		}

		// Token: 0x060000DC RID: 220 RVA: 0x0000A378 File Offset: 0x00009378
		internal string ConvertFromPosix(string format)
		{
			this._originalFormat = format;
			if (this._originalFormat.Length == 0)
			{
				return this._originalFormat;
			}
			this._index = 0;
			this._lastQuotePos = -2;
			this._builder = new StringBuilder(this._originalFormat.Length);
			while (this._index < this._originalFormat.Length)
			{
				char c = this._originalFormat[this._index];
				int num;
				if (c <= '\\')
				{
					if (c <= 'H')
					{
						if (c == '\'')
						{
							this.HandleSingleQuote();
							continue;
						}
						switch (c)
						{
						case 'E':
							break;
						case 'F':
							goto IL_03A4;
						case 'G':
							this._builder.Append('g');
							this._index++;
							continue;
						case 'H':
							this._builder.Append('H');
							this._index++;
							continue;
						default:
							goto IL_03A4;
						}
					}
					else
					{
						if (c == 'M')
						{
							this._builder.Append('M');
							this._index++;
							continue;
						}
						if (c == 'S')
						{
							this._builder.Append('F');
							this._index++;
							continue;
						}
						switch (c)
						{
						case 'Z':
							num = this.GetTokenLength('Z');
							if (num == 1)
							{
								this.InsertEscapedString("GMT");
								this._builder.Append("zzz");
							}
							else
							{
								this.StoreEscapedRepeatedcChar('Z', num);
							}
							this._index += num;
							continue;
						case '[':
							goto IL_03A4;
						case '\\':
							this._index++;
							if (this._index < this._originalFormat.Length)
							{
								this._builder.Append('\\');
								this._builder.Append(this._originalFormat[this._index]);
								this._index++;
								continue;
							}
							continue;
						default:
							goto IL_03A4;
						}
					}
				}
				else if (c <= 'h')
				{
					switch (c)
					{
					case 'a':
						this._builder.Append("tt");
						this._index++;
						continue;
					case 'b':
					case 'c':
						goto IL_03A4;
					case 'd':
						this._builder.Append('d');
						this._index++;
						continue;
					case 'e':
						break;
					default:
						if (c != 'h')
						{
							goto IL_03A4;
						}
						this._builder.Append('h');
						this._index++;
						continue;
					}
				}
				else
				{
					if (c == 'm')
					{
						this._builder.Append('m');
						this._index++;
						continue;
					}
					if (c == 's')
					{
						this._builder.Append('s');
						this._index++;
						continue;
					}
					switch (c)
					{
					case 'y':
						this._builder.Append('y');
						this._index++;
						continue;
					case 'z':
						num = this.GetTokenLength('z');
						if (num == 3 || num == 4)
						{
							this._builder.Append("zzz");
						}
						else
						{
							this.StoreEscapedRepeatedcChar('z', num);
						}
						this._index += num;
						continue;
					default:
						goto IL_03A4;
					}
				}
				char c2 = this._originalFormat[this._index];
				num = this.GetTokenLength(c2);
				if (num == 3)
				{
					this._builder.Append("ddd");
				}
				else if (num == 4)
				{
					this._builder.Append("dddd");
				}
				else
				{
					this.StoreEscapedRepeatedcChar(c2, num);
				}
				this._index += num;
				continue;
				IL_03A4:
				this.InsertEscapedChar(this._originalFormat[this._index]);
				this._index++;
			}
			return this._builder.ToString();
		}

		// Token: 0x060000DD RID: 221 RVA: 0x0000A770 File Offset: 0x00009770
		private int GetTokenLength(char ch)
		{
			int num = 1;
			while (this._index + num < this._originalFormat.Length && this._originalFormat[this._index + num] == ch)
			{
				num++;
			}
			return num;
		}

		// Token: 0x060000DE RID: 222 RVA: 0x0000A7B4 File Offset: 0x000097B4
		internal void InsertEscapedChar(char ch)
		{
			if (this._lastQuotePos == this._builder.Length - 1)
			{
				this._builder.Insert(this._builder.Length - 1, ch);
			}
			else
			{
				this._builder.Append('\'');
				this._builder.Append(ch);
				this._builder.Append('\'');
			}
			this._lastQuotePos = this._builder.Length - 1;
		}

		// Token: 0x060000DF RID: 223 RVA: 0x0000A830 File Offset: 0x00009830
		internal void InsertEscapedString(string text)
		{
			if (this._lastQuotePos == this._builder.Length - 1)
			{
				this._builder.Insert(this._builder.Length - 1, text);
			}
			else
			{
				this._builder.Append('\'');
				this._builder.Append(text);
				this._builder.Append('\'');
			}
			this._lastQuotePos = this._builder.Length - 1;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000A8AC File Offset: 0x000098AC
		private void StoreEscapedRepeatedcChar(char ch, int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.InsertEscapedChar(ch);
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000A8CC File Offset: 0x000098CC
		private void HandleSingleQuote()
		{
			if (this._index < this._originalFormat.Length - 1 && this._originalFormat[this._index + 1] == '\'')
			{
				this._builder.Append('\\');
				this._builder.Append('\'');
				this._index += 2;
				return;
			}
			this._index++;
			while (this._index < this._originalFormat.Length && this._originalFormat[this._index] != '\'')
			{
				this.InsertEscapedChar(this._originalFormat[this._index]);
				this._index++;
			}
			if (this._index < this._originalFormat.Length)
			{
				this._index++;
			}
		}

		// Token: 0x040001A9 RID: 425
		private const char SingleQuote = '\'';

		// Token: 0x040001AA RID: 426
		private const char Backslash = '\\';

		// Token: 0x040001AB RID: 427
		private string _originalFormat;

		// Token: 0x040001AC RID: 428
		private int _index;

		// Token: 0x040001AD RID: 429
		private StringBuilder _builder;

		// Token: 0x040001AE RID: 430
		private int _lastQuotePos;
	}
}
