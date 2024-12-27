using System;
using System.Reflection;

namespace System.Runtime.InteropServices.TCEAdapterGen
{
	// Token: 0x020008DF RID: 2271
	internal class EventItfInfo
	{
		// Token: 0x060052D3 RID: 21203 RVA: 0x0012C775 File Offset: 0x0012B775
		public EventItfInfo(string strEventItfName, string strSrcItfName, string strEventProviderName, Assembly asmImport, Assembly asmSrcItf)
		{
			this.m_strEventItfName = strEventItfName;
			this.m_strSrcItfName = strSrcItfName;
			this.m_strEventProviderName = strEventProviderName;
			this.m_asmImport = asmImport;
			this.m_asmSrcItf = asmSrcItf;
		}

		// Token: 0x060052D4 RID: 21204 RVA: 0x0012C7A4 File Offset: 0x0012B7A4
		public Type GetEventItfType()
		{
			Type type = this.m_asmImport.GetType(this.m_strEventItfName, true, false);
			if (type != null && !type.IsVisible)
			{
				type = null;
			}
			return type;
		}

		// Token: 0x060052D5 RID: 21205 RVA: 0x0012C7D4 File Offset: 0x0012B7D4
		public Type GetSrcItfType()
		{
			Type type = this.m_asmSrcItf.GetType(this.m_strSrcItfName, true, false);
			if (type != null && !type.IsVisible)
			{
				type = null;
			}
			return type;
		}

		// Token: 0x060052D6 RID: 21206 RVA: 0x0012C803 File Offset: 0x0012B803
		public string GetEventProviderName()
		{
			return this.m_strEventProviderName;
		}

		// Token: 0x04002AB8 RID: 10936
		private string m_strEventItfName;

		// Token: 0x04002AB9 RID: 10937
		private string m_strSrcItfName;

		// Token: 0x04002ABA RID: 10938
		private string m_strEventProviderName;

		// Token: 0x04002ABB RID: 10939
		private Assembly m_asmImport;

		// Token: 0x04002ABC RID: 10940
		private Assembly m_asmSrcItf;
	}
}
