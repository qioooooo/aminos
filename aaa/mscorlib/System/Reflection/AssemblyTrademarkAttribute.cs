using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002CA RID: 714
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false)]
	[ComVisible(true)]
	public sealed class AssemblyTrademarkAttribute : Attribute
	{
		// Token: 0x06001CAA RID: 7338 RVA: 0x0004991B File Offset: 0x0004891B
		public AssemblyTrademarkAttribute(string trademark)
		{
			this.m_trademark = trademark;
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06001CAB RID: 7339 RVA: 0x0004992A File Offset: 0x0004892A
		public string Trademark
		{
			get
			{
				return this.m_trademark;
			}
		}

		// Token: 0x04000A7B RID: 2683
		private string m_trademark;
	}
}
