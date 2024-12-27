using System;

namespace System.Net
{
	// Token: 0x0200038F RID: 911
	internal class CookieParser
	{
		// Token: 0x06001C76 RID: 7286 RVA: 0x0006B97E File Offset: 0x0006A97E
		internal CookieParser(string cookieString)
		{
			this.m_tokenizer = new CookieTokenizer(cookieString);
		}

		// Token: 0x06001C77 RID: 7287 RVA: 0x0006B994 File Offset: 0x0006A994
		internal Cookie Get()
		{
			Cookie cookie = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			bool flag9 = false;
			do
			{
				CookieToken cookieToken = this.m_tokenizer.Next(cookie == null, true);
				if (cookie == null && (cookieToken == CookieToken.NameValuePair || cookieToken == CookieToken.Attribute))
				{
					cookie = new Cookie();
					if (!cookie.InternalSetName(this.m_tokenizer.Name))
					{
						cookie.InternalSetName(string.Empty);
					}
					cookie.Value = this.m_tokenizer.Value;
				}
				else
				{
					switch (cookieToken)
					{
					case CookieToken.NameValuePair:
						switch (this.m_tokenizer.Token)
						{
						case CookieToken.Comment:
							if (!flag)
							{
								flag = true;
								cookie.Comment = this.m_tokenizer.Value;
								goto IL_02F6;
							}
							goto IL_02F6;
						case CookieToken.CommentUrl:
						{
							if (flag2)
							{
								goto IL_02F6;
							}
							flag2 = true;
							Uri uri;
							if (Uri.TryCreate(CookieParser.CheckQuoted(this.m_tokenizer.Value), UriKind.Absolute, out uri))
							{
								cookie.CommentUri = uri;
								goto IL_02F6;
							}
							goto IL_02F6;
						}
						case CookieToken.CookieName:
						case CookieToken.Discard:
						case CookieToken.Secure:
						case CookieToken.HttpOnly:
						case CookieToken.Unknown:
							goto IL_02F6;
						case CookieToken.Domain:
							if (!flag3)
							{
								flag3 = true;
								cookie.Domain = CookieParser.CheckQuoted(this.m_tokenizer.Value);
								cookie.IsQuotedDomain = this.m_tokenizer.Quoted;
								goto IL_02F6;
							}
							goto IL_02F6;
						case CookieToken.Expires:
						{
							if (flag4)
							{
								goto IL_02F6;
							}
							flag4 = true;
							DateTime dateTime;
							if (HttpDateParse.ParseCookieDate(CookieParser.CheckQuoted(this.m_tokenizer.Value), out dateTime))
							{
								cookie.Expires = dateTime;
								goto IL_02F6;
							}
							cookie.InternalSetName(string.Empty);
							goto IL_02F6;
						}
						case CookieToken.MaxAge:
						{
							if (flag4)
							{
								goto IL_02F6;
							}
							flag4 = true;
							int num;
							if (int.TryParse(CookieParser.CheckQuoted(this.m_tokenizer.Value), out num))
							{
								cookie.Expires = DateTime.Now.AddSeconds((double)num);
								goto IL_02F6;
							}
							cookie.InternalSetName(string.Empty);
							goto IL_02F6;
						}
						case CookieToken.Path:
							if (!flag5)
							{
								flag5 = true;
								cookie.Path = this.m_tokenizer.Value;
								goto IL_02F6;
							}
							goto IL_02F6;
						case CookieToken.Port:
							if (flag6)
							{
								goto IL_02F6;
							}
							flag6 = true;
							try
							{
								cookie.Port = this.m_tokenizer.Value;
								goto IL_02F6;
							}
							catch
							{
								cookie.InternalSetName(string.Empty);
								goto IL_02F6;
							}
							break;
						case CookieToken.Version:
							break;
						default:
							goto IL_02F6;
						}
						if (!flag7)
						{
							flag7 = true;
							int num2;
							if (int.TryParse(CookieParser.CheckQuoted(this.m_tokenizer.Value), out num2))
							{
								cookie.Version = num2;
								cookie.IsQuotedVersion = this.m_tokenizer.Quoted;
							}
							else
							{
								cookie.InternalSetName(string.Empty);
							}
						}
						break;
					case CookieToken.Attribute:
					{
						CookieToken token = this.m_tokenizer.Token;
						if (token != CookieToken.Discard)
						{
							switch (token)
							{
							case CookieToken.Port:
								if (!flag6)
								{
									flag6 = true;
									cookie.Port = string.Empty;
								}
								break;
							case CookieToken.Secure:
								if (!flag8)
								{
									flag8 = true;
									cookie.Secure = true;
								}
								break;
							case CookieToken.HttpOnly:
								cookie.HttpOnly = true;
								break;
							}
						}
						else if (!flag9)
						{
							flag9 = true;
							cookie.Discard = true;
						}
						break;
					}
					}
				}
				IL_02F6:;
			}
			while (!this.m_tokenizer.Eof && !this.m_tokenizer.EndOfCookie);
			return cookie;
		}

		// Token: 0x06001C78 RID: 7288 RVA: 0x0006BCC8 File Offset: 0x0006ACC8
		internal Cookie GetServer()
		{
			Cookie cookie = this.m_savedCookie;
			this.m_savedCookie = null;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			for (;;)
			{
				bool flag4 = cookie == null || cookie.Name == null || cookie.Name.Length == 0;
				CookieToken cookieToken = this.m_tokenizer.Next(flag4, false);
				if (flag4 && (cookieToken == CookieToken.NameValuePair || cookieToken == CookieToken.Attribute))
				{
					if (cookie == null)
					{
						cookie = new Cookie();
					}
					if (!cookie.InternalSetName(this.m_tokenizer.Name))
					{
						cookie.InternalSetName(string.Empty);
					}
					cookie.Value = this.m_tokenizer.Value;
				}
				else
				{
					switch (cookieToken)
					{
					case CookieToken.NameValuePair:
						switch (this.m_tokenizer.Token)
						{
						case CookieToken.Domain:
							if (!flag)
							{
								flag = true;
								cookie.Domain = CookieParser.CheckQuoted(this.m_tokenizer.Value);
								cookie.IsQuotedDomain = this.m_tokenizer.Quoted;
							}
							break;
						case CookieToken.Path:
							if (!flag2)
							{
								flag2 = true;
								cookie.Path = this.m_tokenizer.Value;
							}
							break;
						case CookieToken.Port:
							if (!flag3)
							{
								flag3 = true;
								try
								{
									cookie.Port = this.m_tokenizer.Value;
									break;
								}
								catch (CookieException)
								{
									cookie.InternalSetName(string.Empty);
									break;
								}
								goto IL_016A;
							}
							break;
						case CookieToken.Unknown:
							goto IL_0198;
						case CookieToken.Version:
							goto IL_016A;
						}
						break;
					case CookieToken.Attribute:
					{
						CookieToken token = this.m_tokenizer.Token;
						if (token == CookieToken.Port && !flag3)
						{
							flag3 = true;
							cookie.Port = string.Empty;
						}
						break;
					}
					}
				}
				if (this.m_tokenizer.Eof || this.m_tokenizer.EndOfCookie)
				{
					return cookie;
				}
			}
			IL_016A:
			this.m_savedCookie = new Cookie();
			int num;
			if (int.TryParse(this.m_tokenizer.Value, out num))
			{
				this.m_savedCookie.Version = num;
			}
			return cookie;
			IL_0198:
			this.m_savedCookie = new Cookie();
			if (!this.m_savedCookie.InternalSetName(this.m_tokenizer.Name))
			{
				this.m_savedCookie.InternalSetName(string.Empty);
			}
			this.m_savedCookie.Value = this.m_tokenizer.Value;
			return cookie;
		}

		// Token: 0x06001C79 RID: 7289 RVA: 0x0006BF0C File Offset: 0x0006AF0C
		internal static string CheckQuoted(string value)
		{
			if (value.Length < 2 || value[0] != '"' || value[value.Length - 1] != '"')
			{
				return value;
			}
			if (value.Length != 2)
			{
				return value.Substring(1, value.Length - 2);
			}
			return string.Empty;
		}

		// Token: 0x04001D1A RID: 7450
		private CookieTokenizer m_tokenizer;

		// Token: 0x04001D1B RID: 7451
		private Cookie m_savedCookie;
	}
}
