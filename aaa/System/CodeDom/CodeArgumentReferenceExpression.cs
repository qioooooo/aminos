using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000037 RID: 55
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeArgumentReferenceExpression : CodeExpression
	{
		// Token: 0x06000258 RID: 600 RVA: 0x000123D2 File Offset: 0x000113D2
		public CodeArgumentReferenceExpression()
		{
		}

		// Token: 0x06000259 RID: 601 RVA: 0x000123DA File Offset: 0x000113DA
		public CodeArgumentReferenceExpression(string parameterName)
		{
			this.parameterName = parameterName;
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x0600025A RID: 602 RVA: 0x000123E9 File Offset: 0x000113E9
		// (set) Token: 0x0600025B RID: 603 RVA: 0x000123FF File Offset: 0x000113FF
		public string ParameterName
		{
			get
			{
				if (this.parameterName != null)
				{
					return this.parameterName;
				}
				return string.Empty;
			}
			set
			{
				this.parameterName = value;
			}
		}

		// Token: 0x040007D5 RID: 2005
		private string parameterName;
	}
}
