using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002C0 RID: 704
	internal class ChoiceIdentifierAccessor : Accessor
	{
		// Token: 0x1700080C RID: 2060
		// (get) Token: 0x06002194 RID: 8596 RVA: 0x0009F05C File Offset: 0x0009E05C
		// (set) Token: 0x06002195 RID: 8597 RVA: 0x0009F064 File Offset: 0x0009E064
		internal string MemberName
		{
			get
			{
				return this.memberName;
			}
			set
			{
				this.memberName = value;
			}
		}

		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002196 RID: 8598 RVA: 0x0009F06D File Offset: 0x0009E06D
		// (set) Token: 0x06002197 RID: 8599 RVA: 0x0009F075 File Offset: 0x0009E075
		internal string[] MemberIds
		{
			get
			{
				return this.memberIds;
			}
			set
			{
				this.memberIds = value;
			}
		}

		// Token: 0x04001469 RID: 5225
		private string memberName;

		// Token: 0x0400146A RID: 5226
		private string[] memberIds;
	}
}
