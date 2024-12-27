using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F1 RID: 753
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct CustomAttributeCtorParameter
	{
		// Token: 0x06001E0D RID: 7693 RVA: 0x0004B926 File Offset: 0x0004A926
		public CustomAttributeCtorParameter(CustomAttributeType type)
		{
			this.m_type = type;
			this.m_encodedArgument = default(CustomAttributeEncodedArgument);
		}

		// Token: 0x170004FC RID: 1276
		// (get) Token: 0x06001E0E RID: 7694 RVA: 0x0004B93B File Offset: 0x0004A93B
		public CustomAttributeEncodedArgument CustomAttributeEncodedArgument
		{
			get
			{
				return this.m_encodedArgument;
			}
		}

		// Token: 0x04000AEA RID: 2794
		private CustomAttributeType m_type;

		// Token: 0x04000AEB RID: 2795
		private CustomAttributeEncodedArgument m_encodedArgument;
	}
}
