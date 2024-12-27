using System;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	// Token: 0x0200029D RID: 669
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
	[ComVisible(true)]
	[Serializable]
	public sealed class ConditionalAttribute : Attribute
	{
		// Token: 0x06001ABB RID: 6843 RVA: 0x000469D0 File Offset: 0x000459D0
		public ConditionalAttribute(string conditionString)
		{
			this.m_conditionString = conditionString;
		}

		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x06001ABC RID: 6844 RVA: 0x000469DF File Offset: 0x000459DF
		public string ConditionString
		{
			get
			{
				return this.m_conditionString;
			}
		}

		// Token: 0x04000A00 RID: 2560
		private string m_conditionString;
	}
}
