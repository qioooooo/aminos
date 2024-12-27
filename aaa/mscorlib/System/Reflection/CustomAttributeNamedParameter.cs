using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F0 RID: 752
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct CustomAttributeNamedParameter
	{
		// Token: 0x06001E0B RID: 7691 RVA: 0x0004B8E6 File Offset: 0x0004A8E6
		public CustomAttributeNamedParameter(string argumentName, CustomAttributeEncoding fieldOrProperty, CustomAttributeType type)
		{
			if (argumentName == null)
			{
				throw new ArgumentNullException("argumentName");
			}
			this.m_argumentName = argumentName;
			this.m_fieldOrProperty = fieldOrProperty;
			this.m_padding = fieldOrProperty;
			this.m_type = type;
			this.m_encodedArgument = default(CustomAttributeEncodedArgument);
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x0004B91E File Offset: 0x0004A91E
		public CustomAttributeEncodedArgument EncodedArgument
		{
			get
			{
				return this.m_encodedArgument;
			}
		}

		// Token: 0x04000AE5 RID: 2789
		private string m_argumentName;

		// Token: 0x04000AE6 RID: 2790
		private CustomAttributeEncoding m_fieldOrProperty;

		// Token: 0x04000AE7 RID: 2791
		private CustomAttributeEncoding m_padding;

		// Token: 0x04000AE8 RID: 2792
		private CustomAttributeType m_type;

		// Token: 0x04000AE9 RID: 2793
		private CustomAttributeEncodedArgument m_encodedArgument;
	}
}
