using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200005E RID: 94
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeLabeledStatement : CodeStatement
	{
		// Token: 0x06000379 RID: 889 RVA: 0x000138DA File Offset: 0x000128DA
		public CodeLabeledStatement()
		{
		}

		// Token: 0x0600037A RID: 890 RVA: 0x000138E2 File Offset: 0x000128E2
		public CodeLabeledStatement(string label)
		{
			this.label = label;
		}

		// Token: 0x0600037B RID: 891 RVA: 0x000138F1 File Offset: 0x000128F1
		public CodeLabeledStatement(string label, CodeStatement statement)
		{
			this.label = label;
			this.statement = statement;
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x0600037C RID: 892 RVA: 0x00013907 File Offset: 0x00012907
		// (set) Token: 0x0600037D RID: 893 RVA: 0x0001391D File Offset: 0x0001291D
		public string Label
		{
			get
			{
				if (this.label != null)
				{
					return this.label;
				}
				return string.Empty;
			}
			set
			{
				this.label = value;
			}
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x0600037E RID: 894 RVA: 0x00013926 File Offset: 0x00012926
		// (set) Token: 0x0600037F RID: 895 RVA: 0x0001392E File Offset: 0x0001292E
		public CodeStatement Statement
		{
			get
			{
				return this.statement;
			}
			set
			{
				this.statement = value;
			}
		}

		// Token: 0x0400083B RID: 2107
		private string label;

		// Token: 0x0400083C RID: 2108
		private CodeStatement statement;
	}
}
