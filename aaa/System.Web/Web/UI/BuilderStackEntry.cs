using System;

namespace System.Web.UI
{
	// Token: 0x02000474 RID: 1140
	internal class BuilderStackEntry : SourceLineInfo
	{
		// Token: 0x060035B6 RID: 13750 RVA: 0x000E7F0A File Offset: 0x000E6F0A
		internal BuilderStackEntry(ControlBuilder builder, string tagName, string virtualPath, int line, string inputText, int textPos)
		{
			this._builder = builder;
			this._tagName = tagName;
			base.VirtualPath = virtualPath;
			base.Line = line;
			this._inputText = inputText;
			this._textPos = textPos;
		}

		// Token: 0x0400254C RID: 9548
		internal ControlBuilder _builder;

		// Token: 0x0400254D RID: 9549
		internal string _tagName;

		// Token: 0x0400254E RID: 9550
		internal string _inputText;

		// Token: 0x0400254F RID: 9551
		internal int _textPos;

		// Token: 0x04002550 RID: 9552
		internal int _repeatCount;
	}
}
