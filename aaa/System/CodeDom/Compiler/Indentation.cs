using System;
using System.Text;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001F8 RID: 504
	internal class Indentation
	{
		// Token: 0x0600113B RID: 4411 RVA: 0x0003827B File Offset: 0x0003727B
		internal Indentation(IndentedTextWriter writer, int indent)
		{
			this.writer = writer;
			this.indent = indent;
			this.s = null;
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600113C RID: 4412 RVA: 0x00038298 File Offset: 0x00037298
		internal string IndentationString
		{
			get
			{
				if (this.s == null)
				{
					string tabString = this.writer.TabString;
					StringBuilder stringBuilder = new StringBuilder(this.indent * tabString.Length);
					for (int i = 0; i < this.indent; i++)
					{
						stringBuilder.Append(tabString);
					}
					this.s = stringBuilder.ToString();
				}
				return this.s;
			}
		}

		// Token: 0x04000FA5 RID: 4005
		private IndentedTextWriter writer;

		// Token: 0x04000FA6 RID: 4006
		private int indent;

		// Token: 0x04000FA7 RID: 4007
		private string s;
	}
}
