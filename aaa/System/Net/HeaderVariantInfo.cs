using System;

namespace System.Net
{
	// Token: 0x02000394 RID: 916
	internal struct HeaderVariantInfo
	{
		// Token: 0x06001C92 RID: 7314 RVA: 0x0006C455 File Offset: 0x0006B455
		internal HeaderVariantInfo(string name, CookieVariant variant)
		{
			this.m_name = name;
			this.m_variant = variant;
		}

		// Token: 0x17000591 RID: 1425
		// (get) Token: 0x06001C93 RID: 7315 RVA: 0x0006C465 File Offset: 0x0006B465
		internal string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000592 RID: 1426
		// (get) Token: 0x06001C94 RID: 7316 RVA: 0x0006C46D File Offset: 0x0006B46D
		internal CookieVariant Variant
		{
			get
			{
				return this.m_variant;
			}
		}

		// Token: 0x04001D2A RID: 7466
		private string m_name;

		// Token: 0x04001D2B RID: 7467
		private CookieVariant m_variant;
	}
}
