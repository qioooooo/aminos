using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002EF RID: 751
	[Serializable]
	[StructLayout(LayoutKind.Auto)]
	internal struct CustomAttributeEncodedArgument
	{
		// Token: 0x06001E05 RID: 7685
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void ParseAttributeArguments(IntPtr pCa, int cCa, ref CustomAttributeCtorParameter[] CustomAttributeCtorParameters, ref CustomAttributeNamedParameter[] CustomAttributeTypedArgument, IntPtr assembly);

		// Token: 0x06001E06 RID: 7686 RVA: 0x0004B864 File Offset: 0x0004A864
		internal static void ParseAttributeArguments(ConstArray attributeBlob, ref CustomAttributeCtorParameter[] customAttributeCtorParameters, ref CustomAttributeNamedParameter[] customAttributeNamedParameters, Module customAttributeModule)
		{
			if (customAttributeModule == null)
			{
				throw new ArgumentNullException("customAttributeModule");
			}
			if (customAttributeNamedParameters == null)
			{
				customAttributeNamedParameters = new CustomAttributeNamedParameter[0];
			}
			CustomAttributeCtorParameter[] array = customAttributeCtorParameters;
			CustomAttributeNamedParameter[] array2 = customAttributeNamedParameters;
			CustomAttributeEncodedArgument.ParseAttributeArguments(attributeBlob.Signature, attributeBlob.Length, ref array, ref array2, (IntPtr)customAttributeModule.Assembly.AssemblyHandle.Value);
			customAttributeCtorParameters = array;
			customAttributeNamedParameters = array2;
		}

		// Token: 0x170004F7 RID: 1271
		// (get) Token: 0x06001E07 RID: 7687 RVA: 0x0004B8C6 File Offset: 0x0004A8C6
		public CustomAttributeType CustomAttributeType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x0004B8CE File Offset: 0x0004A8CE
		public long PrimitiveValue
		{
			get
			{
				return this.m_primitiveValue;
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06001E09 RID: 7689 RVA: 0x0004B8D6 File Offset: 0x0004A8D6
		public CustomAttributeEncodedArgument[] ArrayValue
		{
			get
			{
				return this.m_arrayValue;
			}
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x0004B8DE File Offset: 0x0004A8DE
		public string StringValue
		{
			get
			{
				return this.m_stringValue;
			}
		}

		// Token: 0x04000AE1 RID: 2785
		private long m_primitiveValue;

		// Token: 0x04000AE2 RID: 2786
		private CustomAttributeEncodedArgument[] m_arrayValue;

		// Token: 0x04000AE3 RID: 2787
		private string m_stringValue;

		// Token: 0x04000AE4 RID: 2788
		private CustomAttributeType m_type;
	}
}
