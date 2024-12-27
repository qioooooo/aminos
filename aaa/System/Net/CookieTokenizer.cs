using System;

namespace System.Net
{
	// Token: 0x0200038D RID: 909
	internal class CookieTokenizer
	{
		// Token: 0x06001C61 RID: 7265 RVA: 0x0006B31A File Offset: 0x0006A31A
		internal CookieTokenizer(string tokenStream)
		{
			this.m_length = tokenStream.Length;
			this.m_tokenStream = tokenStream;
		}

		// Token: 0x17000582 RID: 1410
		// (get) Token: 0x06001C62 RID: 7266 RVA: 0x0006B335 File Offset: 0x0006A335
		// (set) Token: 0x06001C63 RID: 7267 RVA: 0x0006B33D File Offset: 0x0006A33D
		internal bool EndOfCookie
		{
			get
			{
				return this.m_eofCookie;
			}
			set
			{
				this.m_eofCookie = value;
			}
		}

		// Token: 0x17000583 RID: 1411
		// (get) Token: 0x06001C64 RID: 7268 RVA: 0x0006B346 File Offset: 0x0006A346
		internal bool Eof
		{
			get
			{
				return this.m_index >= this.m_length;
			}
		}

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06001C65 RID: 7269 RVA: 0x0006B359 File Offset: 0x0006A359
		// (set) Token: 0x06001C66 RID: 7270 RVA: 0x0006B361 File Offset: 0x0006A361
		internal string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06001C67 RID: 7271 RVA: 0x0006B36A File Offset: 0x0006A36A
		// (set) Token: 0x06001C68 RID: 7272 RVA: 0x0006B372 File Offset: 0x0006A372
		internal bool Quoted
		{
			get
			{
				return this.m_quoted;
			}
			set
			{
				this.m_quoted = value;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06001C69 RID: 7273 RVA: 0x0006B37B File Offset: 0x0006A37B
		// (set) Token: 0x06001C6A RID: 7274 RVA: 0x0006B383 File Offset: 0x0006A383
		internal CookieToken Token
		{
			get
			{
				return this.m_token;
			}
			set
			{
				this.m_token = value;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06001C6B RID: 7275 RVA: 0x0006B38C File Offset: 0x0006A38C
		// (set) Token: 0x06001C6C RID: 7276 RVA: 0x0006B394 File Offset: 0x0006A394
		internal string Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = value;
			}
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x0006B3A0 File Offset: 0x0006A3A0
		internal string Extract()
		{
			string text = string.Empty;
			if (this.m_tokenLength != 0)
			{
				text = this.m_tokenStream.Substring(this.m_start, this.m_tokenLength);
				if (!this.Quoted)
				{
					text = text.Trim();
				}
			}
			return text;
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x0006B3E4 File Offset: 0x0006A3E4
		internal CookieToken FindNext(bool ignoreComma, bool ignoreEquals)
		{
			this.m_tokenLength = 0;
			this.m_start = this.m_index;
			while (this.m_index < this.m_length && char.IsWhiteSpace(this.m_tokenStream[this.m_index]))
			{
				this.m_index++;
				this.m_start++;
			}
			CookieToken cookieToken = CookieToken.End;
			int num = 1;
			if (!this.Eof)
			{
				if (this.m_tokenStream[this.m_index] == '"')
				{
					this.Quoted = true;
					this.m_index++;
					bool flag = false;
					while (this.m_index < this.m_length)
					{
						char c = this.m_tokenStream[this.m_index];
						if (!flag && c == '"')
						{
							break;
						}
						if (flag)
						{
							flag = false;
						}
						else if (c == '\\')
						{
							flag = true;
						}
						this.m_index++;
					}
					if (this.m_index < this.m_length)
					{
						this.m_index++;
					}
					this.m_tokenLength = this.m_index - this.m_start;
					num = 0;
					ignoreComma = false;
				}
				while (this.m_index < this.m_length && this.m_tokenStream[this.m_index] != ';' && (ignoreEquals || this.m_tokenStream[this.m_index] != '=') && (ignoreComma || this.m_tokenStream[this.m_index] != ','))
				{
					if (this.m_tokenStream[this.m_index] == ',')
					{
						this.m_start = this.m_index + 1;
						this.m_tokenLength = -1;
						ignoreComma = false;
					}
					this.m_index++;
					this.m_tokenLength += num;
				}
				if (!this.Eof)
				{
					switch (this.m_tokenStream[this.m_index])
					{
					case ';':
						cookieToken = CookieToken.EndToken;
						goto IL_01F0;
					case '=':
						cookieToken = CookieToken.Equals;
						goto IL_01F0;
					}
					cookieToken = CookieToken.EndCookie;
					IL_01F0:
					this.m_index++;
				}
			}
			return cookieToken;
		}

		// Token: 0x06001C6F RID: 7279 RVA: 0x0006B5F0 File Offset: 0x0006A5F0
		internal CookieToken Next(bool first, bool parseResponseCookies)
		{
			this.Reset();
			CookieToken cookieToken = this.FindNext(false, false);
			if (cookieToken == CookieToken.EndCookie)
			{
				this.EndOfCookie = true;
			}
			if (cookieToken == CookieToken.End || cookieToken == CookieToken.EndCookie)
			{
				if ((this.Name = this.Extract()).Length != 0)
				{
					this.Token = this.TokenFromName(parseResponseCookies);
					return CookieToken.Attribute;
				}
				return cookieToken;
			}
			else
			{
				this.Name = this.Extract();
				if (first)
				{
					this.Token = CookieToken.CookieName;
				}
				else
				{
					this.Token = this.TokenFromName(parseResponseCookies);
				}
				if (cookieToken == CookieToken.Equals)
				{
					cookieToken = this.FindNext(!first && this.Token == CookieToken.Expires, true);
					if (cookieToken == CookieToken.EndCookie)
					{
						this.EndOfCookie = true;
					}
					this.Value = this.Extract();
					return CookieToken.NameValuePair;
				}
				return CookieToken.Attribute;
			}
		}

		// Token: 0x06001C70 RID: 7280 RVA: 0x0006B6A2 File Offset: 0x0006A6A2
		internal void Reset()
		{
			this.m_eofCookie = false;
			this.m_name = string.Empty;
			this.m_quoted = false;
			this.m_start = this.m_index;
			this.m_token = CookieToken.Nothing;
			this.m_tokenLength = 0;
			this.m_value = string.Empty;
		}

		// Token: 0x06001C71 RID: 7281 RVA: 0x0006B6E4 File Offset: 0x0006A6E4
		internal CookieToken TokenFromName(bool parseResponseCookies)
		{
			if (!parseResponseCookies)
			{
				for (int i = 0; i < CookieTokenizer.RecognizedServerAttributes.Length; i++)
				{
					if (CookieTokenizer.RecognizedServerAttributes[i].IsEqualTo(this.Name))
					{
						return CookieTokenizer.RecognizedServerAttributes[i].Token;
					}
				}
			}
			else
			{
				for (int j = 0; j < CookieTokenizer.RecognizedAttributes.Length; j++)
				{
					if (CookieTokenizer.RecognizedAttributes[j].IsEqualTo(this.Name))
					{
						return CookieTokenizer.RecognizedAttributes[j].Token;
					}
				}
			}
			return CookieToken.Unknown;
		}

		// Token: 0x04001D0C RID: 7436
		private bool m_eofCookie;

		// Token: 0x04001D0D RID: 7437
		private int m_index;

		// Token: 0x04001D0E RID: 7438
		private int m_length;

		// Token: 0x04001D0F RID: 7439
		private string m_name;

		// Token: 0x04001D10 RID: 7440
		private bool m_quoted;

		// Token: 0x04001D11 RID: 7441
		private int m_start;

		// Token: 0x04001D12 RID: 7442
		private CookieToken m_token;

		// Token: 0x04001D13 RID: 7443
		private int m_tokenLength;

		// Token: 0x04001D14 RID: 7444
		private string m_tokenStream;

		// Token: 0x04001D15 RID: 7445
		private string m_value;

		// Token: 0x04001D16 RID: 7446
		private static CookieTokenizer.RecognizedAttribute[] RecognizedAttributes = new CookieTokenizer.RecognizedAttribute[]
		{
			new CookieTokenizer.RecognizedAttribute("Path", CookieToken.Path),
			new CookieTokenizer.RecognizedAttribute("Max-Age", CookieToken.MaxAge),
			new CookieTokenizer.RecognizedAttribute("Expires", CookieToken.Expires),
			new CookieTokenizer.RecognizedAttribute("Version", CookieToken.Version),
			new CookieTokenizer.RecognizedAttribute("Domain", CookieToken.Domain),
			new CookieTokenizer.RecognizedAttribute("Secure", CookieToken.Secure),
			new CookieTokenizer.RecognizedAttribute("Discard", CookieToken.Discard),
			new CookieTokenizer.RecognizedAttribute("Port", CookieToken.Port),
			new CookieTokenizer.RecognizedAttribute("Comment", CookieToken.Comment),
			new CookieTokenizer.RecognizedAttribute("CommentURL", CookieToken.CommentUrl),
			new CookieTokenizer.RecognizedAttribute("HttpOnly", CookieToken.HttpOnly)
		};

		// Token: 0x04001D17 RID: 7447
		private static CookieTokenizer.RecognizedAttribute[] RecognizedServerAttributes = new CookieTokenizer.RecognizedAttribute[]
		{
			new CookieTokenizer.RecognizedAttribute('$' + "Path", CookieToken.Path),
			new CookieTokenizer.RecognizedAttribute('$' + "Version", CookieToken.Version),
			new CookieTokenizer.RecognizedAttribute('$' + "Domain", CookieToken.Domain),
			new CookieTokenizer.RecognizedAttribute('$' + "Port", CookieToken.Port),
			new CookieTokenizer.RecognizedAttribute('$' + "HttpOnly", CookieToken.HttpOnly)
		};

		// Token: 0x0200038E RID: 910
		private struct RecognizedAttribute
		{
			// Token: 0x06001C73 RID: 7283 RVA: 0x0006B954 File Offset: 0x0006A954
			internal RecognizedAttribute(string name, CookieToken token)
			{
				this.m_name = name;
				this.m_token = token;
			}

			// Token: 0x17000588 RID: 1416
			// (get) Token: 0x06001C74 RID: 7284 RVA: 0x0006B964 File Offset: 0x0006A964
			internal CookieToken Token
			{
				get
				{
					return this.m_token;
				}
			}

			// Token: 0x06001C75 RID: 7285 RVA: 0x0006B96C File Offset: 0x0006A96C
			internal bool IsEqualTo(string value)
			{
				return string.Compare(this.m_name, value, StringComparison.OrdinalIgnoreCase) == 0;
			}

			// Token: 0x04001D18 RID: 7448
			private string m_name;

			// Token: 0x04001D19 RID: 7449
			private CookieToken m_token;
		}
	}
}
