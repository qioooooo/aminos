using System;
using System.IO;
using System.Security.Permissions;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001E1 RID: 481
	public interface ICodeGenerator
	{
		// Token: 0x06000F51 RID: 3921
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		bool IsValidIdentifier(string value);

		// Token: 0x06000F52 RID: 3922
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		void ValidateIdentifier(string value);

		// Token: 0x06000F53 RID: 3923
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		string CreateEscapedIdentifier(string value);

		// Token: 0x06000F54 RID: 3924
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		string CreateValidIdentifier(string value);

		// Token: 0x06000F55 RID: 3925
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		string GetTypeOutput(CodeTypeReference type);

		// Token: 0x06000F56 RID: 3926
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		bool Supports(GeneratorSupport supports);

		// Token: 0x06000F57 RID: 3927
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void GenerateCodeFromExpression(CodeExpression e, TextWriter w, CodeGeneratorOptions o);

		// Token: 0x06000F58 RID: 3928
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void GenerateCodeFromStatement(CodeStatement e, TextWriter w, CodeGeneratorOptions o);

		// Token: 0x06000F59 RID: 3929
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		void GenerateCodeFromNamespace(CodeNamespace e, TextWriter w, CodeGeneratorOptions o);

		// Token: 0x06000F5A RID: 3930
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		void GenerateCodeFromCompileUnit(CodeCompileUnit e, TextWriter w, CodeGeneratorOptions o);

		// Token: 0x06000F5B RID: 3931
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
		void GenerateCodeFromType(CodeTypeDeclaration e, TextWriter w, CodeGeneratorOptions o);
	}
}
