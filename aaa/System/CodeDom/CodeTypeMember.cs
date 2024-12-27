using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace System.CodeDom
{
	// Token: 0x0200004E RID: 78
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeTypeMember : CodeObject
	{
		// Token: 0x1700006F RID: 111
		// (get) Token: 0x06000305 RID: 773 RVA: 0x0001307C File Offset: 0x0001207C
		// (set) Token: 0x06000306 RID: 774 RVA: 0x00013092 File Offset: 0x00012092
		public string Name
		{
			get
			{
				if (this.name != null)
				{
					return this.name;
				}
				return string.Empty;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x17000070 RID: 112
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0001309B File Offset: 0x0001209B
		// (set) Token: 0x06000308 RID: 776 RVA: 0x000130A3 File Offset: 0x000120A3
		public MemberAttributes Attributes
		{
			get
			{
				return this.attributes;
			}
			set
			{
				this.attributes = value;
			}
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000309 RID: 777 RVA: 0x000130AC File Offset: 0x000120AC
		// (set) Token: 0x0600030A RID: 778 RVA: 0x000130C7 File Offset: 0x000120C7
		public CodeAttributeDeclarationCollection CustomAttributes
		{
			get
			{
				if (this.customAttributes == null)
				{
					this.customAttributes = new CodeAttributeDeclarationCollection();
				}
				return this.customAttributes;
			}
			set
			{
				this.customAttributes = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x0600030B RID: 779 RVA: 0x000130D0 File Offset: 0x000120D0
		// (set) Token: 0x0600030C RID: 780 RVA: 0x000130D8 File Offset: 0x000120D8
		public CodeLinePragma LinePragma
		{
			get
			{
				return this.linePragma;
			}
			set
			{
				this.linePragma = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x0600030D RID: 781 RVA: 0x000130E1 File Offset: 0x000120E1
		public CodeCommentStatementCollection Comments
		{
			get
			{
				return this.comments;
			}
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600030E RID: 782 RVA: 0x000130E9 File Offset: 0x000120E9
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

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x0600030F RID: 783 RVA: 0x00013104 File Offset: 0x00012104
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

		// Token: 0x04000810 RID: 2064
		private MemberAttributes attributes = (MemberAttributes)20482;

		// Token: 0x04000811 RID: 2065
		private string name;

		// Token: 0x04000812 RID: 2066
		private CodeCommentStatementCollection comments = new CodeCommentStatementCollection();

		// Token: 0x04000813 RID: 2067
		private CodeAttributeDeclarationCollection customAttributes;

		// Token: 0x04000814 RID: 2068
		private CodeLinePragma linePragma;

		// Token: 0x04000815 RID: 2069
		[OptionalField]
		private CodeDirectiveCollection startDirectives;

		// Token: 0x04000816 RID: 2070
		[OptionalField]
		private CodeDirectiveCollection endDirectives;
	}
}
