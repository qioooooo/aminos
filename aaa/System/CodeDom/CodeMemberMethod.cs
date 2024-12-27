using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200004F RID: 79
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeMemberMethod : CodeTypeMember
	{
		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000311 RID: 785 RVA: 0x0001313D File Offset: 0x0001213D
		// (remove) Token: 0x06000312 RID: 786 RVA: 0x00013156 File Offset: 0x00012156
		public event EventHandler PopulateParameters;

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000313 RID: 787 RVA: 0x0001316F File Offset: 0x0001216F
		// (remove) Token: 0x06000314 RID: 788 RVA: 0x00013188 File Offset: 0x00012188
		public event EventHandler PopulateStatements;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000315 RID: 789 RVA: 0x000131A1 File Offset: 0x000121A1
		// (remove) Token: 0x06000316 RID: 790 RVA: 0x000131BA File Offset: 0x000121BA
		public event EventHandler PopulateImplementationTypes;

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x06000317 RID: 791 RVA: 0x000131D3 File Offset: 0x000121D3
		// (set) Token: 0x06000318 RID: 792 RVA: 0x000131FD File Offset: 0x000121FD
		public CodeTypeReference ReturnType
		{
			get
			{
				if (this.returnType == null)
				{
					this.returnType = new CodeTypeReference(typeof(void).FullName);
				}
				return this.returnType;
			}
			set
			{
				this.returnType = value;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00013206 File Offset: 0x00012206
		public CodeStatementCollection Statements
		{
			get
			{
				if ((this.populated & 2) == 0)
				{
					this.populated |= 2;
					if (this.PopulateStatements != null)
					{
						this.PopulateStatements(this, EventArgs.Empty);
					}
				}
				return this.statements;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0001323F File Offset: 0x0001223F
		public CodeParameterDeclarationExpressionCollection Parameters
		{
			get
			{
				if ((this.populated & 1) == 0)
				{
					this.populated |= 1;
					if (this.PopulateParameters != null)
					{
						this.PopulateParameters(this, EventArgs.Empty);
					}
				}
				return this.parameters;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00013278 File Offset: 0x00012278
		// (set) Token: 0x0600031C RID: 796 RVA: 0x00013280 File Offset: 0x00012280
		public CodeTypeReference PrivateImplementationType
		{
			get
			{
				return this.privateImplements;
			}
			set
			{
				this.privateImplements = value;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x0600031D RID: 797 RVA: 0x0001328C File Offset: 0x0001228C
		public CodeTypeReferenceCollection ImplementationTypes
		{
			get
			{
				if (this.implementationTypes == null)
				{
					this.implementationTypes = new CodeTypeReferenceCollection();
				}
				if ((this.populated & 4) == 0)
				{
					this.populated |= 4;
					if (this.PopulateImplementationTypes != null)
					{
						this.PopulateImplementationTypes(this, EventArgs.Empty);
					}
				}
				return this.implementationTypes;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x0600031E RID: 798 RVA: 0x000132E3 File Offset: 0x000122E3
		public CodeAttributeDeclarationCollection ReturnTypeCustomAttributes
		{
			get
			{
				if (this.returnAttributes == null)
				{
					this.returnAttributes = new CodeAttributeDeclarationCollection();
				}
				return this.returnAttributes;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x0600031F RID: 799 RVA: 0x000132FE File Offset: 0x000122FE
		[ComVisible(false)]
		public CodeTypeParameterCollection TypeParameters
		{
			get
			{
				if (this.typeParameters == null)
				{
					this.typeParameters = new CodeTypeParameterCollection();
				}
				return this.typeParameters;
			}
		}

		// Token: 0x04000817 RID: 2071
		private const int ParametersCollection = 1;

		// Token: 0x04000818 RID: 2072
		private const int StatementsCollection = 2;

		// Token: 0x04000819 RID: 2073
		private const int ImplTypesCollection = 4;

		// Token: 0x0400081A RID: 2074
		private CodeParameterDeclarationExpressionCollection parameters = new CodeParameterDeclarationExpressionCollection();

		// Token: 0x0400081B RID: 2075
		private CodeStatementCollection statements = new CodeStatementCollection();

		// Token: 0x0400081C RID: 2076
		private CodeTypeReference returnType;

		// Token: 0x0400081D RID: 2077
		private CodeTypeReference privateImplements;

		// Token: 0x0400081E RID: 2078
		private CodeTypeReferenceCollection implementationTypes;

		// Token: 0x0400081F RID: 2079
		private CodeAttributeDeclarationCollection returnAttributes;

		// Token: 0x04000820 RID: 2080
		[OptionalField]
		private CodeTypeParameterCollection typeParameters;

		// Token: 0x04000821 RID: 2081
		private int populated;
	}
}
