using System;

namespace Microsoft.JScript
{
	// Token: 0x020000BD RID: 189
	internal class NoSkipTokenSet
	{
		// Token: 0x06000888 RID: 2184 RVA: 0x00040E4B File Offset: 0x0003FE4B
		internal NoSkipTokenSet()
		{
			this._tokenSet = null;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00040E5A File Offset: 0x0003FE5A
		internal void Add(JSToken[] tokens)
		{
			this._tokenSet = new NoSkipTokenSet.TokenSetListItem(tokens, this._tokenSet);
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00040E70 File Offset: 0x0003FE70
		internal void Remove(JSToken[] tokens)
		{
			NoSkipTokenSet.TokenSetListItem tokenSetListItem = this._tokenSet;
			NoSkipTokenSet.TokenSetListItem tokenSetListItem2 = null;
			while (tokenSetListItem != null)
			{
				if (tokenSetListItem._tokens == tokens)
				{
					if (tokenSetListItem2 == null)
					{
						this._tokenSet = this._tokenSet._next;
						return;
					}
					tokenSetListItem2._next = tokenSetListItem._next;
					return;
				}
				else
				{
					tokenSetListItem2 = tokenSetListItem;
					tokenSetListItem = tokenSetListItem._next;
				}
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x00040EC0 File Offset: 0x0003FEC0
		internal bool HasToken(JSToken token)
		{
			for (NoSkipTokenSet.TokenSetListItem tokenSetListItem = this._tokenSet; tokenSetListItem != null; tokenSetListItem = tokenSetListItem._next)
			{
				int i = 0;
				int num = tokenSetListItem._tokens.Length;
				while (i < num)
				{
					if (tokenSetListItem._tokens[i] == token)
					{
						return true;
					}
					i++;
				}
			}
			return false;
		}

		// Token: 0x04000483 RID: 1155
		private NoSkipTokenSet.TokenSetListItem _tokenSet;

		// Token: 0x04000484 RID: 1156
		internal static readonly JSToken[] s_ArrayInitNoSkipTokenSet = new JSToken[]
		{
			JSToken.RightBracket,
			JSToken.Comma
		};

		// Token: 0x04000485 RID: 1157
		internal static readonly JSToken[] s_BlockConditionNoSkipTokenSet = new JSToken[]
		{
			JSToken.RightParen,
			JSToken.LeftCurly,
			JSToken.EndOfLine
		};

		// Token: 0x04000486 RID: 1158
		internal static readonly JSToken[] s_BlockNoSkipTokenSet = new JSToken[] { JSToken.RightCurly };

		// Token: 0x04000487 RID: 1159
		internal static readonly JSToken[] s_BracketToken = new JSToken[] { JSToken.RightBracket };

		// Token: 0x04000488 RID: 1160
		internal static readonly JSToken[] s_CaseNoSkipTokenSet = new JSToken[]
		{
			JSToken.Case,
			JSToken.Default,
			JSToken.Colon,
			JSToken.EndOfLine
		};

		// Token: 0x04000489 RID: 1161
		internal static readonly JSToken[] s_ClassBodyNoSkipTokenSet = new JSToken[]
		{
			JSToken.Class,
			JSToken.Interface,
			JSToken.Enum,
			JSToken.Function,
			JSToken.Var,
			JSToken.Const,
			JSToken.Static,
			JSToken.Public,
			JSToken.Private,
			JSToken.Protected
		};

		// Token: 0x0400048A RID: 1162
		internal static readonly JSToken[] s_InterfaceBodyNoSkipTokenSet = new JSToken[]
		{
			JSToken.Enum,
			JSToken.Function,
			JSToken.Public,
			JSToken.EndOfLine,
			JSToken.Semicolon
		};

		// Token: 0x0400048B RID: 1163
		internal static readonly JSToken[] s_ClassExtendsNoSkipTokenSet = new JSToken[]
		{
			JSToken.LeftCurly,
			JSToken.Implements
		};

		// Token: 0x0400048C RID: 1164
		internal static readonly JSToken[] s_ClassImplementsNoSkipTokenSet = new JSToken[]
		{
			JSToken.LeftCurly,
			JSToken.Comma
		};

		// Token: 0x0400048D RID: 1165
		internal static readonly JSToken[] s_DoWhileBodyNoSkipTokenSet = new JSToken[] { JSToken.While };

		// Token: 0x0400048E RID: 1166
		internal static readonly JSToken[] s_EndOfLineToken = new JSToken[] { JSToken.EndOfLine };

		// Token: 0x0400048F RID: 1167
		internal static readonly JSToken[] s_EndOfStatementNoSkipTokenSet = new JSToken[]
		{
			JSToken.Semicolon,
			JSToken.EndOfLine
		};

		// Token: 0x04000490 RID: 1168
		internal static readonly JSToken[] s_EnumBaseTypeNoSkipTokenSet = new JSToken[] { JSToken.LeftCurly };

		// Token: 0x04000491 RID: 1169
		internal static readonly JSToken[] s_EnumBodyNoSkipTokenSet = new JSToken[] { JSToken.Identifier };

		// Token: 0x04000492 RID: 1170
		internal static readonly JSToken[] s_ExpressionListNoSkipTokenSet = new JSToken[] { JSToken.Comma };

		// Token: 0x04000493 RID: 1171
		internal static readonly JSToken[] s_FunctionDeclNoSkipTokenSet = new JSToken[]
		{
			JSToken.RightParen,
			JSToken.LeftCurly,
			JSToken.Comma
		};

		// Token: 0x04000494 RID: 1172
		internal static readonly JSToken[] s_IfBodyNoSkipTokenSet = new JSToken[] { JSToken.Else };

		// Token: 0x04000495 RID: 1173
		internal static readonly JSToken[] s_MemberExprNoSkipTokenSet = new JSToken[]
		{
			JSToken.LeftBracket,
			JSToken.LeftParen,
			JSToken.AccessField
		};

		// Token: 0x04000496 RID: 1174
		internal static readonly JSToken[] s_NoTrySkipTokenSet = new JSToken[]
		{
			JSToken.Catch,
			JSToken.Finally
		};

		// Token: 0x04000497 RID: 1175
		internal static readonly JSToken[] s_ObjectInitNoSkipTokenSet = new JSToken[]
		{
			JSToken.RightCurly,
			JSToken.Comma
		};

		// Token: 0x04000498 RID: 1176
		internal static readonly JSToken[] s_PackageBodyNoSkipTokenSet = new JSToken[]
		{
			JSToken.Class,
			JSToken.Interface,
			JSToken.Enum
		};

		// Token: 0x04000499 RID: 1177
		internal static readonly JSToken[] s_ParenExpressionNoSkipToken = new JSToken[] { JSToken.RightParen };

		// Token: 0x0400049A RID: 1178
		internal static readonly JSToken[] s_ParenToken = new JSToken[] { JSToken.RightParen };

		// Token: 0x0400049B RID: 1179
		internal static readonly JSToken[] s_PostfixExpressionNoSkipTokenSet = new JSToken[]
		{
			JSToken.Increment,
			JSToken.Decrement
		};

		// Token: 0x0400049C RID: 1180
		internal static readonly JSToken[] s_StartBlockNoSkipTokenSet = new JSToken[] { JSToken.LeftCurly };

		// Token: 0x0400049D RID: 1181
		internal static readonly JSToken[] s_StartStatementNoSkipTokenSet = new JSToken[]
		{
			JSToken.LeftCurly,
			JSToken.Var,
			JSToken.Const,
			JSToken.If,
			JSToken.For,
			JSToken.Do,
			JSToken.While,
			JSToken.With,
			JSToken.Switch,
			JSToken.Try
		};

		// Token: 0x0400049E RID: 1182
		internal static readonly JSToken[] s_SwitchNoSkipTokenSet = new JSToken[]
		{
			JSToken.Case,
			JSToken.Default
		};

		// Token: 0x0400049F RID: 1183
		internal static readonly JSToken[] s_TopLevelNoSkipTokenSet = new JSToken[]
		{
			JSToken.Package,
			JSToken.Class,
			JSToken.Interface,
			JSToken.Enum,
			JSToken.Function,
			JSToken.Import
		};

		// Token: 0x040004A0 RID: 1184
		internal static readonly JSToken[] s_VariableDeclNoSkipTokenSet = new JSToken[]
		{
			JSToken.Comma,
			JSToken.Semicolon
		};

		// Token: 0x020000BE RID: 190
		private class TokenSetListItem
		{
			// Token: 0x0600088D RID: 2189 RVA: 0x0004128A File Offset: 0x0004028A
			internal TokenSetListItem(JSToken[] tokens, NoSkipTokenSet.TokenSetListItem next)
			{
				this._next = next;
				this._tokens = tokens;
			}

			// Token: 0x040004A1 RID: 1185
			internal NoSkipTokenSet.TokenSetListItem _next;

			// Token: 0x040004A2 RID: 1186
			internal JSToken[] _tokens;
		}
	}
}
