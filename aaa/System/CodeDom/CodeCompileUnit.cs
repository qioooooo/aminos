using System;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200004C RID: 76
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCompileUnit : CodeObject
	{
		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002F9 RID: 761 RVA: 0x00012F5B File Offset: 0x00011F5B
		public CodeNamespaceCollection Namespaces
		{
			get
			{
				return this.namespaces;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060002FA RID: 762 RVA: 0x00012F63 File Offset: 0x00011F63
		public StringCollection ReferencedAssemblies
		{
			get
			{
				if (this.assemblies == null)
				{
					this.assemblies = new StringCollection();
				}
				return this.assemblies;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00012F7E File Offset: 0x00011F7E
		public CodeAttributeDeclarationCollection AssemblyCustomAttributes
		{
			get
			{
				if (this.attributes == null)
				{
					this.attributes = new CodeAttributeDeclarationCollection();
				}
				return this.attributes;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002FC RID: 764 RVA: 0x00012F99 File Offset: 0x00011F99
		public CodeDirectiveCollection StartDirectives
		{
			get
			{
				if (this.startDirectives == null)
				{
					this.startDirectives = new CodeDirectiveCollection();
				}
				return this.startDirectives;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002FD RID: 765 RVA: 0x00012FB4 File Offset: 0x00011FB4
		public CodeDirectiveCollection EndDirectives
		{
			get
			{
				if (this.endDirectives == null)
				{
					this.endDirectives = new CodeDirectiveCollection();
				}
				return this.endDirectives;
			}
		}

		// Token: 0x04000808 RID: 2056
		private CodeNamespaceCollection namespaces = new CodeNamespaceCollection();

		// Token: 0x04000809 RID: 2057
		private StringCollection assemblies;

		// Token: 0x0400080A RID: 2058
		private CodeAttributeDeclarationCollection attributes;

		// Token: 0x0400080B RID: 2059
		[OptionalField]
		private CodeDirectiveCollection startDirectives;

		// Token: 0x0400080C RID: 2060
		[OptionalField]
		private CodeDirectiveCollection endDirectives;
	}
}
