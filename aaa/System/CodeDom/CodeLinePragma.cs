using System;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200005F RID: 95
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeLinePragma
	{
		// Token: 0x06000380 RID: 896 RVA: 0x00013937 File Offset: 0x00012937
		public CodeLinePragma()
		{
		}

		// Token: 0x06000381 RID: 897 RVA: 0x0001393F File Offset: 0x0001293F
		public CodeLinePragma(string fileName, int lineNumber)
		{
			this.FileName = fileName;
			this.LineNumber = lineNumber;
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x06000382 RID: 898 RVA: 0x00013955 File Offset: 0x00012955
		// (set) Token: 0x06000383 RID: 899 RVA: 0x0001396B File Offset: 0x0001296B
		public string FileName
		{
			get
			{
				if (this.fileName != null)
				{
					return this.fileName;
				}
				return string.Empty;
			}
			set
			{
				this.fileName = value;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000384 RID: 900 RVA: 0x00013974 File Offset: 0x00012974
		// (set) Token: 0x06000385 RID: 901 RVA: 0x0001397C File Offset: 0x0001297C
		public int LineNumber
		{
			get
			{
				return this.lineNumber;
			}
			set
			{
				this.lineNumber = value;
			}
		}

		// Token: 0x0400083D RID: 2109
		private string fileName;

		// Token: 0x0400083E RID: 2110
		private int lineNumber;
	}
}
