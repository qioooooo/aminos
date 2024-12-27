using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

namespace Microsoft.CSharp
{
	// Token: 0x020002B8 RID: 696
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class CSharpCodeProvider : CodeDomProvider
	{
		// Token: 0x060016C6 RID: 5830 RVA: 0x000483C2 File Offset: 0x000473C2
		public CSharpCodeProvider()
		{
			this.generator = new CSharpCodeGenerator();
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x000483D5 File Offset: 0x000473D5
		public CSharpCodeProvider(IDictionary<string, string> providerOptions)
		{
			if (providerOptions == null)
			{
				throw new ArgumentNullException("providerOptions");
			}
			this.generator = new CSharpCodeGenerator(providerOptions);
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x060016C8 RID: 5832 RVA: 0x000483F7 File Offset: 0x000473F7
		public override string FileExtension
		{
			get
			{
				return "cs";
			}
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x000483FE File Offset: 0x000473FE
		[Obsolete("Callers should not use the ICodeGenerator interface and should instead use the methods directly on the CodeDomProvider class.")]
		public override ICodeGenerator CreateGenerator()
		{
			return this.generator;
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00048406 File Offset: 0x00047406
		[Obsolete("Callers should not use the ICodeCompiler interface and should instead use the methods directly on the CodeDomProvider class.")]
		public override ICodeCompiler CreateCompiler()
		{
			return this.generator;
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x0004840E File Offset: 0x0004740E
		public override TypeConverter GetConverter(Type type)
		{
			if (type == typeof(MemberAttributes))
			{
				return CSharpMemberAttributeConverter.Default;
			}
			if (type == typeof(TypeAttributes))
			{
				return CSharpTypeAttributeConverter.Default;
			}
			return base.GetConverter(type);
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x0004843D File Offset: 0x0004743D
		public override void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
		{
			this.generator.GenerateCodeFromMember(member, writer, options);
		}

		// Token: 0x04001601 RID: 5633
		private CSharpCodeGenerator generator;
	}
}
