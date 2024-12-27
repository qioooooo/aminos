using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F6 RID: 758
	[ComVisible(true)]
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	[Serializable]
	public sealed class DefaultMemberAttribute : Attribute
	{
		// Token: 0x06001E4C RID: 7756 RVA: 0x0004D18B File Offset: 0x0004C18B
		public DefaultMemberAttribute(string memberName)
		{
			this.m_memberName = memberName;
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06001E4D RID: 7757 RVA: 0x0004D19A File Offset: 0x0004C19A
		public string MemberName
		{
			get
			{
				return this.m_memberName;
			}
		}

		// Token: 0x04000AF7 RID: 2807
		private string m_memberName;
	}
}
