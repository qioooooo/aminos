using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002F3 RID: 755
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct CustomAttributeType
	{
		// Token: 0x06001E11 RID: 7697 RVA: 0x0004B943 File Offset: 0x0004A943
		public CustomAttributeType(CustomAttributeEncoding encodedType, CustomAttributeEncoding encodedArrayType, CustomAttributeEncoding encodedEnumType, string enumName)
		{
			this.m_encodedType = encodedType;
			this.m_encodedArrayType = encodedArrayType;
			this.m_encodedEnumType = encodedEnumType;
			this.m_enumName = enumName;
			this.m_padding = this.m_encodedType;
		}

		// Token: 0x170004FD RID: 1277
		// (get) Token: 0x06001E12 RID: 7698 RVA: 0x0004B96E File Offset: 0x0004A96E
		public CustomAttributeEncoding EncodedType
		{
			get
			{
				return this.m_encodedType;
			}
		}

		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06001E13 RID: 7699 RVA: 0x0004B976 File Offset: 0x0004A976
		public CustomAttributeEncoding EncodedEnumType
		{
			get
			{
				return this.m_encodedEnumType;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06001E14 RID: 7700 RVA: 0x0004B97E File Offset: 0x0004A97E
		public CustomAttributeEncoding EncodedArrayType
		{
			get
			{
				return this.m_encodedArrayType;
			}
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06001E15 RID: 7701 RVA: 0x0004B986 File Offset: 0x0004A986
		[ComVisible(true)]
		public string EnumName
		{
			get
			{
				return this.m_enumName;
			}
		}

		// Token: 0x04000AF0 RID: 2800
		private string m_enumName;

		// Token: 0x04000AF1 RID: 2801
		private CustomAttributeEncoding m_encodedType;

		// Token: 0x04000AF2 RID: 2802
		private CustomAttributeEncoding m_encodedEnumType;

		// Token: 0x04000AF3 RID: 2803
		private CustomAttributeEncoding m_encodedArrayType;

		// Token: 0x04000AF4 RID: 2804
		private CustomAttributeEncoding m_padding;
	}
}
