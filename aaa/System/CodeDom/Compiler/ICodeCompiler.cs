using System;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001E3 RID: 483
	public interface ICodeCompiler
	{
		// Token: 0x06000FE5 RID: 4069
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromDom(CompilerParameters options, CodeCompileUnit compilationUnit);

		// Token: 0x06000FE6 RID: 4070
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromFile(CompilerParameters options, string fileName);

		// Token: 0x06000FE7 RID: 4071
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromSource(CompilerParameters options, string source);

		// Token: 0x06000FE8 RID: 4072
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromDomBatch(CompilerParameters options, CodeCompileUnit[] compilationUnits);

		// Token: 0x06000FE9 RID: 4073
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromFileBatch(CompilerParameters options, string[] fileNames);

		// Token: 0x06000FEA RID: 4074
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		CompilerResults CompileAssemblyFromSourceBatch(CompilerParameters options, string[] sources);
	}
}
