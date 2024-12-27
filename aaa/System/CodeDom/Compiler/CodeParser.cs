using System;
using System.IO;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001ED RID: 493
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public abstract class CodeParser : ICodeParser
	{
		// Token: 0x0600104D RID: 4173
		public abstract CodeCompileUnit Parse(TextReader codeStream);
	}
}
