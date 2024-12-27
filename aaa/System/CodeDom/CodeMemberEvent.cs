using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000060 RID: 96
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeMemberEvent : CodeTypeMember
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000387 RID: 903 RVA: 0x0001398D File Offset: 0x0001298D
		// (set) Token: 0x06000388 RID: 904 RVA: 0x000139AD File Offset: 0x000129AD
		public CodeTypeReference Type
		{
			get
			{
				if (this.type == null)
				{
					this.type = new CodeTypeReference("");
				}
				return this.type;
			}
			set
			{
				this.type = value;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000389 RID: 905 RVA: 0x000139B6 File Offset: 0x000129B6
		// (set) Token: 0x0600038A RID: 906 RVA: 0x000139BE File Offset: 0x000129BE
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600038B RID: 907 RVA: 0x000139C7 File Offset: 0x000129C7
		public CodeTypeReferenceCollection ImplementationTypes
		{
			get
			{
				if (this.implementationTypes == null)
				{
					this.implementationTypes = new CodeTypeReferenceCollection();
				}
				return this.implementationTypes;
			}
		}

		// Token: 0x0400083F RID: 2111
		private CodeTypeReference type;

		// Token: 0x04000840 RID: 2112
		private CodeTypeReference privateImplements;

		// Token: 0x04000841 RID: 2113
		private CodeTypeReferenceCollection implementationTypes;
	}
}
