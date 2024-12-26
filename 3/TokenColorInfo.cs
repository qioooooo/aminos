using System;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000097 RID: 151
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	internal class TokenColorInfo : ITokenColorInfo
	{
		// Token: 0x060006AF RID: 1711 RVA: 0x0002EF35 File Offset: 0x0002DF35
		internal TokenColorInfo(Context token)
		{
			this._token = token.Clone();
			this._color = TokenColorInfo.ColorFromToken(this._token);
			this._next = this;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002EF64 File Offset: 0x0002DF64
		internal static TokenColor ColorFromToken(Context context)
		{
			JSToken token = context.GetToken();
			if (JSScanner.IsKeyword(token))
			{
				return TokenColor.COLOR_KEYWORD;
			}
			if (JSToken.Identifier == token)
			{
				if (context.Equals("eval"))
				{
					return TokenColor.COLOR_KEYWORD;
				}
				return TokenColor.COLOR_IDENTIFIER;
			}
			else
			{
				if (JSToken.StringLiteral == token)
				{
					return TokenColor.COLOR_STRING;
				}
				if (JSToken.NumericLiteral == token || JSToken.IntegerLiteral == token)
				{
					return TokenColor.COLOR_NUMBER;
				}
				if (JSToken.Comment == token || JSToken.UnterminatedComment == token)
				{
					return TokenColor.COLOR_COMMENT;
				}
				if (JSScanner.IsOperator(token))
				{
					return TokenColor.COLOR_OPERATOR;
				}
				return TokenColor.COLOR_TEXT;
			}
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x0002EFC2 File Offset: 0x0002DFC2
		internal TokenColorInfo Clone()
		{
			return (TokenColorInfo)base.MemberwiseClone();
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x060006B2 RID: 1714 RVA: 0x0002EFCF File Offset: 0x0002DFCF
		public TokenColor Color
		{
			get
			{
				return this._color;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x060006B3 RID: 1715 RVA: 0x0002EFD7 File Offset: 0x0002DFD7
		public int EndPosition
		{
			get
			{
				return this._token.EndPosition;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x060006B4 RID: 1716 RVA: 0x0002EFE4 File Offset: 0x0002DFE4
		public int StartPosition
		{
			get
			{
				return this._token.StartPosition;
			}
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002EFF1 File Offset: 0x0002DFF1
		internal void UpdateToken(Context token)
		{
			this._token = token.Clone();
			this._color = TokenColorInfo.ColorFromToken(this._token);
		}

		// Token: 0x0400030A RID: 778
		private Context _token;

		// Token: 0x0400030B RID: 779
		private TokenColor _color;

		// Token: 0x0400030C RID: 780
		internal TokenColorInfo _next;
	}
}
