using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000030 RID: 48
	internal sealed class CompiledRegexRunner : RegexRunner
	{
		// Token: 0x06000242 RID: 578 RVA: 0x000122CD File Offset: 0x000112CD
		internal CompiledRegexRunner()
		{
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000122D5 File Offset: 0x000112D5
		internal void SetDelegates(NoParamDelegate go, FindFirstCharDelegate firstChar, NoParamDelegate trackCount)
		{
			this.goMethod = go;
			this.findFirstCharMethod = firstChar;
			this.initTrackCountMethod = trackCount;
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000122EC File Offset: 0x000112EC
		protected override void Go()
		{
			this.goMethod(this);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000122FA File Offset: 0x000112FA
		protected override bool FindFirstChar()
		{
			return this.findFirstCharMethod(this);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00012308 File Offset: 0x00011308
		protected override void InitTrackCount()
		{
			this.initTrackCountMethod(this);
		}

		// Token: 0x040007CE RID: 1998
		private NoParamDelegate goMethod;

		// Token: 0x040007CF RID: 1999
		private FindFirstCharDelegate findFirstCharMethod;

		// Token: 0x040007D0 RID: 2000
		private NoParamDelegate initTrackCountMethod;
	}
}
