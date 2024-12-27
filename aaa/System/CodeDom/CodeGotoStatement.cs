using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200005B RID: 91
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeGotoStatement : CodeStatement
	{
		// Token: 0x06000367 RID: 871 RVA: 0x000137C2 File Offset: 0x000127C2
		public CodeGotoStatement()
		{
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000137CA File Offset: 0x000127CA
		public CodeGotoStatement(string label)
		{
			this.Label = label;
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x06000369 RID: 873 RVA: 0x000137D9 File Offset: 0x000127D9
		// (set) Token: 0x0600036A RID: 874 RVA: 0x000137E1 File Offset: 0x000127E1
		public string Label
		{
			get
			{
				return this.label;
			}
			set
			{
				if (string.IsNullOrEmpty(value))
				{
					throw new ArgumentNullException("value");
				}
				this.label = value;
			}
		}

		// Token: 0x04000834 RID: 2100
		private string label;
	}
}
