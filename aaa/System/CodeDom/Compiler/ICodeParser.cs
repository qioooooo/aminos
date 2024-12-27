using System;
using System.IO;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001EC RID: 492
	public interface ICodeParser
	{
		// Token: 0x0600104C RID: 4172
		CodeCompileUnit Parse(TextReader codeStream);
	}
}
