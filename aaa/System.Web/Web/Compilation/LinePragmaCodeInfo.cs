using System;
using System.Security.Permissions;

namespace System.Web.Compilation
{
	// Token: 0x02000160 RID: 352
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class LinePragmaCodeInfo
	{
		// Token: 0x06000FF7 RID: 4087 RVA: 0x00046C60 File Offset: 0x00045C60
		public LinePragmaCodeInfo()
		{
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00046C68 File Offset: 0x00045C68
		public LinePragmaCodeInfo(int startLine, int startColumn, int startGeneratedColumn, int codeLength, bool isCodeNugget)
		{
			this._startLine = startLine;
			this._startColumn = startColumn;
			this._startGeneratedColumn = startGeneratedColumn;
			this._codeLength = codeLength;
			this._isCodeNugget = isCodeNugget;
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x06000FF9 RID: 4089 RVA: 0x00046C95 File Offset: 0x00045C95
		public int StartLine
		{
			get
			{
				return this._startLine;
			}
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x06000FFA RID: 4090 RVA: 0x00046C9D File Offset: 0x00045C9D
		public int StartColumn
		{
			get
			{
				return this._startColumn;
			}
		}

		// Token: 0x17000404 RID: 1028
		// (get) Token: 0x06000FFB RID: 4091 RVA: 0x00046CA5 File Offset: 0x00045CA5
		public int StartGeneratedColumn
		{
			get
			{
				return this._startGeneratedColumn;
			}
		}

		// Token: 0x17000405 RID: 1029
		// (get) Token: 0x06000FFC RID: 4092 RVA: 0x00046CAD File Offset: 0x00045CAD
		public int CodeLength
		{
			get
			{
				return this._codeLength;
			}
		}

		// Token: 0x17000406 RID: 1030
		// (get) Token: 0x06000FFD RID: 4093 RVA: 0x00046CB5 File Offset: 0x00045CB5
		public bool IsCodeNugget
		{
			get
			{
				return this._isCodeNugget;
			}
		}

		// Token: 0x04001626 RID: 5670
		internal int _startLine;

		// Token: 0x04001627 RID: 5671
		internal int _startColumn;

		// Token: 0x04001628 RID: 5672
		internal int _startGeneratedColumn;

		// Token: 0x04001629 RID: 5673
		internal int _codeLength;

		// Token: 0x0400162A RID: 5674
		internal bool _isCodeNugget;
	}
}
