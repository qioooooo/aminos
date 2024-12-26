using System;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000095 RID: 149
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	internal class JSColorizer : IColorizeText
	{
		// Token: 0x060006A8 RID: 1704 RVA: 0x0002ECCD File Offset: 0x0002DCCD
		internal JSColorizer()
		{
			this._scanner = new JSScanner();
			this._scanner.SetAuthoringMode(true);
			this._state = SourceState.STATE_COLOR_NORMAL;
		}

		// Token: 0x060006A9 RID: 1705 RVA: 0x0002ECF4 File Offset: 0x0002DCF4
		public virtual ITokenEnumerator Colorize(string sourceCode, SourceState state)
		{
			TokenColorInfoList tokenColorInfoList = new TokenColorInfoList();
			this._state = SourceState.STATE_COLOR_NORMAL;
			if (sourceCode.Length > 0)
			{
				Context context = new Context(null, sourceCode);
				this._scanner.SetSource(context);
				try
				{
					if (SourceState.STATE_COLOR_COMMENT == state)
					{
						int num = this._scanner.SkipMultiLineComment();
						if (num > sourceCode.Length)
						{
							this._state = SourceState.STATE_COLOR_COMMENT;
							num = sourceCode.Length;
						}
						tokenColorInfoList.Add(context);
						if (num == sourceCode.Length)
						{
							return tokenColorInfoList;
						}
					}
					this._scanner.GetNextToken();
					JSToken jstoken = JSToken.None;
					while (context.GetToken() != JSToken.EndOfFile)
					{
						tokenColorInfoList.Add(context);
						jstoken = context.GetToken();
						this._scanner.GetNextToken();
					}
					if (JSToken.UnterminatedComment == jstoken)
					{
						this._state = SourceState.STATE_COLOR_COMMENT;
					}
				}
				catch (ScannerException)
				{
				}
				return tokenColorInfoList;
			}
			return tokenColorInfoList;
		}

		// Token: 0x060006AA RID: 1706 RVA: 0x0002EDC0 File Offset: 0x0002DDC0
		public virtual SourceState GetStateForText(string sourceCode, SourceState state)
		{
			if (sourceCode != null)
			{
				this._state = SourceState.STATE_COLOR_NORMAL;
				Context context = new Context(null, sourceCode);
				this._scanner.SetSource(context);
				if (SourceState.STATE_COLOR_COMMENT == state)
				{
					int num = this._scanner.SkipMultiLineComment();
					if (num > sourceCode.Length)
					{
						this._state = SourceState.STATE_COLOR_COMMENT;
						return this._state;
					}
				}
				this._scanner.GetNextToken();
				JSToken jstoken = JSToken.None;
				while (context.GetToken() != JSToken.EndOfFile)
				{
					jstoken = context.GetToken();
					this._scanner.GetNextToken();
				}
				if (JSToken.UnterminatedComment == jstoken)
				{
					this._state = SourceState.STATE_COLOR_COMMENT;
				}
			}
			return this._state;
		}

		// Token: 0x04000305 RID: 773
		private JSScanner _scanner;

		// Token: 0x04000306 RID: 774
		private SourceState _state;
	}
}
