using System;
using System.Diagnostics;

namespace System.Xml.Xsl
{
	// Token: 0x02000008 RID: 8
	[DebuggerDisplay("{uriString} [{startLine},{startPos} -- {endLine},{endPos}]")]
	internal class SourceLineInfo : ISourceLineInfo
	{
		// Token: 0x0600001B RID: 27 RVA: 0x00002337 File Offset: 0x00001337
		public SourceLineInfo(string uriString, int startLine, int startPos, int endLine, int endPos)
		{
			this.uriString = uriString;
			this.startLine = startLine;
			this.startPos = startPos;
			this.endLine = endLine;
			this.endPos = endPos;
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001C RID: 28 RVA: 0x00002364 File Offset: 0x00001364
		public string Uri
		{
			get
			{
				return this.uriString;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600001D RID: 29 RVA: 0x0000236C File Offset: 0x0000136C
		public int StartLine
		{
			get
			{
				return this.startLine;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600001E RID: 30 RVA: 0x00002374 File Offset: 0x00001374
		public int StartPos
		{
			get
			{
				return this.startPos;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x0600001F RID: 31 RVA: 0x0000237C File Offset: 0x0000137C
		public int EndLine
		{
			get
			{
				return this.endLine;
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000020 RID: 32 RVA: 0x00002384 File Offset: 0x00001384
		public int EndPos
		{
			get
			{
				return this.endPos;
			}
		}

		// Token: 0x06000021 RID: 33 RVA: 0x0000238C File Offset: 0x0000138C
		internal void SetEndLinePos(int endLine, int endPos)
		{
			this.endLine = endLine;
			this.endPos = endPos;
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000022 RID: 34 RVA: 0x0000239C File Offset: 0x0000139C
		public bool IsNoSource
		{
			get
			{
				return this.startLine == 16707566;
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023AB File Offset: 0x000013AB
		[Conditional("DEBUG")]
		public static void Validate(ISourceLineInfo lineInfo)
		{
			if (lineInfo.StartLine != 0)
			{
				if (lineInfo.StartLine == 16707566)
				{
					return;
				}
				if (lineInfo.StartLine == lineInfo.EndLine)
				{
				}
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000023D4 File Offset: 0x000013D4
		public static string GetFileName(string uriString)
		{
			Uri uri;
			if (uriString.Length != 0 && global::System.Uri.TryCreate(uriString, UriKind.Absolute, out uri) && uri.IsFile)
			{
				return uri.LocalPath;
			}
			return uriString;
		}

		// Token: 0x040000AE RID: 174
		private const int NoSourceMagicNumber = 16707566;

		// Token: 0x040000AF RID: 175
		private string uriString;

		// Token: 0x040000B0 RID: 176
		private int startLine;

		// Token: 0x040000B1 RID: 177
		private int startPos;

		// Token: 0x040000B2 RID: 178
		private int endLine;

		// Token: 0x040000B3 RID: 179
		private int endPos;

		// Token: 0x040000B4 RID: 180
		public static SourceLineInfo NoSource = new SourceLineInfo(string.Empty, 16707566, 0, 16707566, 0);
	}
}
