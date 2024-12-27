using System;

namespace System.Reflection.Emit
{
	// Token: 0x020007ED RID: 2029
	internal class NativeVersionInfo
	{
		// Token: 0x06004867 RID: 18535 RVA: 0x000FD1B4 File Offset: 0x000FC1B4
		internal NativeVersionInfo()
		{
			this.m_strDescription = null;
			this.m_strCompany = null;
			this.m_strTitle = null;
			this.m_strCopyright = null;
			this.m_strTrademark = null;
			this.m_strProduct = null;
			this.m_strProductVersion = null;
			this.m_strFileVersion = null;
			this.m_lcid = -1;
		}

		// Token: 0x0400252C RID: 9516
		internal string m_strDescription;

		// Token: 0x0400252D RID: 9517
		internal string m_strCompany;

		// Token: 0x0400252E RID: 9518
		internal string m_strTitle;

		// Token: 0x0400252F RID: 9519
		internal string m_strCopyright;

		// Token: 0x04002530 RID: 9520
		internal string m_strTrademark;

		// Token: 0x04002531 RID: 9521
		internal string m_strProduct;

		// Token: 0x04002532 RID: 9522
		internal string m_strProductVersion;

		// Token: 0x04002533 RID: 9523
		internal string m_strFileVersion;

		// Token: 0x04002534 RID: 9524
		internal int m_lcid;
	}
}
