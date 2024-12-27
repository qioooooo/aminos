using System;

namespace System.Text
{
	// Token: 0x020003F7 RID: 1015
	[Serializable]
	public sealed class EncodingInfo
	{
		// Token: 0x06002A31 RID: 10801 RVA: 0x00084600 File Offset: 0x00083600
		internal EncodingInfo(int codePage, string name, string displayName)
		{
			this.iCodePage = codePage;
			this.strEncodingName = name;
			this.strDisplayName = displayName;
		}

		// Token: 0x17000805 RID: 2053
		// (get) Token: 0x06002A32 RID: 10802 RVA: 0x0008461D File Offset: 0x0008361D
		public int CodePage
		{
			get
			{
				return this.iCodePage;
			}
		}

		// Token: 0x17000806 RID: 2054
		// (get) Token: 0x06002A33 RID: 10803 RVA: 0x00084625 File Offset: 0x00083625
		public string Name
		{
			get
			{
				return this.strEncodingName;
			}
		}

		// Token: 0x17000807 RID: 2055
		// (get) Token: 0x06002A34 RID: 10804 RVA: 0x0008462D File Offset: 0x0008362D
		public string DisplayName
		{
			get
			{
				return this.strDisplayName;
			}
		}

		// Token: 0x06002A35 RID: 10805 RVA: 0x00084635 File Offset: 0x00083635
		public Encoding GetEncoding()
		{
			return Encoding.GetEncoding(this.iCodePage);
		}

		// Token: 0x06002A36 RID: 10806 RVA: 0x00084644 File Offset: 0x00083644
		public override bool Equals(object value)
		{
			EncodingInfo encodingInfo = value as EncodingInfo;
			return encodingInfo != null && this.CodePage == encodingInfo.CodePage;
		}

		// Token: 0x06002A37 RID: 10807 RVA: 0x0008466B File Offset: 0x0008366B
		public override int GetHashCode()
		{
			return this.CodePage;
		}

		// Token: 0x0400145E RID: 5214
		private int iCodePage;

		// Token: 0x0400145F RID: 5215
		private string strEncodingName;

		// Token: 0x04001460 RID: 5216
		private string strDisplayName;
	}
}
