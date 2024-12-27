using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000045 RID: 69
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeCatchClause
	{
		// Token: 0x060002BF RID: 703 RVA: 0x00012B56 File Offset: 0x00011B56
		public CodeCatchClause()
		{
		}

		// Token: 0x060002C0 RID: 704 RVA: 0x00012B5E File Offset: 0x00011B5E
		public CodeCatchClause(string localName)
		{
			this.localName = localName;
		}

		// Token: 0x060002C1 RID: 705 RVA: 0x00012B6D File Offset: 0x00011B6D
		public CodeCatchClause(string localName, CodeTypeReference catchExceptionType)
		{
			this.localName = localName;
			this.catchExceptionType = catchExceptionType;
		}

		// Token: 0x060002C2 RID: 706 RVA: 0x00012B83 File Offset: 0x00011B83
		public CodeCatchClause(string localName, CodeTypeReference catchExceptionType, params CodeStatement[] statements)
		{
			this.localName = localName;
			this.catchExceptionType = catchExceptionType;
			this.Statements.AddRange(statements);
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x00012BA5 File Offset: 0x00011BA5
		// (set) Token: 0x060002C4 RID: 708 RVA: 0x00012BBB File Offset: 0x00011BBB
		public string LocalName
		{
			get
			{
				if (this.localName != null)
				{
					return this.localName;
				}
				return string.Empty;
			}
			set
			{
				this.localName = value;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060002C5 RID: 709 RVA: 0x00012BC4 File Offset: 0x00011BC4
		// (set) Token: 0x060002C6 RID: 710 RVA: 0x00012BE9 File Offset: 0x00011BE9
		public CodeTypeReference CatchExceptionType
		{
			get
			{
				if (this.catchExceptionType == null)
				{
					this.catchExceptionType = new CodeTypeReference(typeof(Exception));
				}
				return this.catchExceptionType;
			}
			set
			{
				this.catchExceptionType = value;
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060002C7 RID: 711 RVA: 0x00012BF2 File Offset: 0x00011BF2
		public CodeStatementCollection Statements
		{
			get
			{
				if (this.statements == null)
				{
					this.statements = new CodeStatementCollection();
				}
				return this.statements;
			}
		}

		// Token: 0x040007FF RID: 2047
		private CodeStatementCollection statements;

		// Token: 0x04000800 RID: 2048
		private CodeTypeReference catchExceptionType;

		// Token: 0x04000801 RID: 2049
		private string localName;
	}
}
