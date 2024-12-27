using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Security.Permissions;

namespace Microsoft.VisualBasic
{
	// Token: 0x020002BD RID: 701
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class VBCodeProvider : CodeDomProvider
	{
		// Token: 0x06001785 RID: 6021 RVA: 0x0004E643 File Offset: 0x0004D643
		public VBCodeProvider()
		{
			this.generator = new VBCodeGenerator();
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x0004E656 File Offset: 0x0004D656
		public VBCodeProvider(IDictionary<string, string> providerOptions)
		{
			if (providerOptions == null)
			{
				throw new ArgumentNullException("providerOptions");
			}
			this.generator = new VBCodeGenerator(providerOptions);
		}

		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x06001787 RID: 6023 RVA: 0x0004E678 File Offset: 0x0004D678
		public override string FileExtension
		{
			get
			{
				return "vb";
			}
		}

		// Token: 0x1700049A RID: 1178
		// (get) Token: 0x06001788 RID: 6024 RVA: 0x0004E67F File Offset: 0x0004D67F
		public override LanguageOptions LanguageOptions
		{
			get
			{
				return LanguageOptions.CaseInsensitive;
			}
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x0004E682 File Offset: 0x0004D682
		[Obsolete("Callers should not use the ICodeGenerator interface and should instead use the methods directly on the CodeDomProvider class.")]
		public override ICodeGenerator CreateGenerator()
		{
			return this.generator;
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x0004E68A File Offset: 0x0004D68A
		[Obsolete("Callers should not use the ICodeCompiler interface and should instead use the methods directly on the CodeDomProvider class.")]
		public override ICodeCompiler CreateCompiler()
		{
			return this.generator;
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x0004E692 File Offset: 0x0004D692
		public override TypeConverter GetConverter(Type type)
		{
			if (type == typeof(MemberAttributes))
			{
				return VBMemberAttributeConverter.Default;
			}
			if (type == typeof(TypeAttributes))
			{
				return VBTypeAttributeConverter.Default;
			}
			return base.GetConverter(type);
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x0004E6C1 File Offset: 0x0004D6C1
		public override void GenerateCodeFromMember(CodeTypeMember member, TextWriter writer, CodeGeneratorOptions options)
		{
			this.generator.GenerateCodeFromMember(member, writer, options);
		}

		// Token: 0x04001614 RID: 5652
		private VBCodeGenerator generator;
	}
}
