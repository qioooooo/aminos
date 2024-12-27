using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000310 RID: 784
	internal struct MetadataImport
	{
		// Token: 0x06001E6E RID: 7790 RVA: 0x0004D3C3 File Offset: 0x0004C3C3
		public override int GetHashCode()
		{
			return this.m_metadataImport2.GetHashCode();
		}

		// Token: 0x06001E6F RID: 7791 RVA: 0x0004D3D6 File Offset: 0x0004C3D6
		public override bool Equals(object obj)
		{
			return obj is MetadataImport && this.Equals((MetadataImport)obj);
		}

		// Token: 0x06001E70 RID: 7792 RVA: 0x0004D3EE File Offset: 0x0004C3EE
		internal bool Equals(MetadataImport import)
		{
			return import.m_metadataImport2 == this.m_metadataImport2;
		}

		// Token: 0x06001E71 RID: 7793
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetMarshalAs(IntPtr pNativeType, int cNativeType, out int unmanagedType, out int safeArraySubType, out string safeArrayUserDefinedSubType, out int arraySubType, out int sizeParamIndex, out int sizeConst, out string marshalType, out string marshalCookie, out int iidParamIndex);

		// Token: 0x06001E72 RID: 7794 RVA: 0x0004D404 File Offset: 0x0004C404
		internal static void GetMarshalAs(ConstArray nativeType, out UnmanagedType unmanagedType, out VarEnum safeArraySubType, out string safeArrayUserDefinedSubType, out UnmanagedType arraySubType, out int sizeParamIndex, out int sizeConst, out string marshalType, out string marshalCookie, out int iidParamIndex)
		{
			int num;
			int num2;
			int num3;
			MetadataImport._GetMarshalAs(nativeType.Signature, nativeType.Length, out num, out num2, out safeArrayUserDefinedSubType, out num3, out sizeParamIndex, out sizeConst, out marshalType, out marshalCookie, out iidParamIndex);
			unmanagedType = (UnmanagedType)num;
			safeArraySubType = (VarEnum)num2;
			arraySubType = (UnmanagedType)num3;
		}

		// Token: 0x06001E73 RID: 7795 RVA: 0x0004D43F File Offset: 0x0004C43F
		internal static void ThrowError(int hResult)
		{
			throw new MetadataException(hResult);
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0004D447 File Offset: 0x0004C447
		internal MetadataImport(IntPtr metadataImport2)
		{
			this.m_metadataImport2 = metadataImport2;
		}

		// Token: 0x06001E75 RID: 7797
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _Enum(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int type, int parent, int* result, int count);

		// Token: 0x06001E76 RID: 7798
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int _EnumCount(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int type, int parent, out int count);

		// Token: 0x06001E77 RID: 7799 RVA: 0x0004D450 File Offset: 0x0004C450
		public unsafe void Enum(int type, int parent, int* result, int count)
		{
			MetadataImport._Enum(this.m_metadataImport2, out MetadataArgs.Skip, type, parent, result, count);
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x0004D468 File Offset: 0x0004C468
		public int EnumCount(int type, int parent)
		{
			int num = 0;
			MetadataImport._EnumCount(this.m_metadataImport2, out MetadataArgs.Skip, type, parent, out num);
			return num;
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x0004D48D File Offset: 0x0004C48D
		public unsafe void EnumNestedTypes(int mdTypeDef, int* result, int count)
		{
			this.Enum(33554432, mdTypeDef, result, count);
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x0004D49D File Offset: 0x0004C49D
		public int EnumNestedTypesCount(int mdTypeDef)
		{
			return this.EnumCount(33554432, mdTypeDef);
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x0004D4AB File Offset: 0x0004C4AB
		public unsafe void EnumCustomAttributes(int mdToken, int* result, int count)
		{
			this.Enum(201326592, mdToken, result, count);
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x0004D4BB File Offset: 0x0004C4BB
		public int EnumCustomAttributesCount(int mdToken)
		{
			return this.EnumCount(201326592, mdToken);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x0004D4C9 File Offset: 0x0004C4C9
		public unsafe void EnumParams(int mdMethodDef, int* result, int count)
		{
			this.Enum(134217728, mdMethodDef, result, count);
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x0004D4D9 File Offset: 0x0004C4D9
		public int EnumParamsCount(int mdMethodDef)
		{
			return this.EnumCount(134217728, mdMethodDef);
		}

		// Token: 0x06001E7F RID: 7807 RVA: 0x0004D4E8 File Offset: 0x0004C4E8
		public unsafe void GetAssociates(int mdPropEvent, AssociateRecord* result, int count)
		{
			int* ptr = stackalloc int[4 * (count * 2)];
			this.Enum(100663296, mdPropEvent, ptr, count);
			for (int i = 0; i < count; i++)
			{
				result[i].MethodDefToken = ptr[i * 2];
				result[i].Semantics = (MethodSemanticsAttributes)ptr[i * 2 + 1];
			}
		}

		// Token: 0x06001E80 RID: 7808 RVA: 0x0004D54B File Offset: 0x0004C54B
		public int GetAssociatesCount(int mdPropEvent)
		{
			return this.EnumCount(100663296, mdPropEvent);
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x0004D559 File Offset: 0x0004C559
		public unsafe void EnumFields(int mdTypeDef, int* result, int count)
		{
			this.Enum(67108864, mdTypeDef, result, count);
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x0004D569 File Offset: 0x0004C569
		public int EnumFieldsCount(int mdTypeDef)
		{
			return this.EnumCount(67108864, mdTypeDef);
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x0004D577 File Offset: 0x0004C577
		public unsafe void EnumProperties(int mdTypeDef, int* result, int count)
		{
			this.Enum(385875968, mdTypeDef, result, count);
		}

		// Token: 0x06001E84 RID: 7812 RVA: 0x0004D587 File Offset: 0x0004C587
		public int EnumPropertiesCount(int mdTypeDef)
		{
			return this.EnumCount(385875968, mdTypeDef);
		}

		// Token: 0x06001E85 RID: 7813 RVA: 0x0004D595 File Offset: 0x0004C595
		public unsafe void EnumEvents(int mdTypeDef, int* result, int count)
		{
			this.Enum(335544320, mdTypeDef, result, count);
		}

		// Token: 0x06001E86 RID: 7814 RVA: 0x0004D5A5 File Offset: 0x0004C5A5
		public int EnumEventsCount(int mdTypeDef)
		{
			return this.EnumCount(335544320, mdTypeDef);
		}

		// Token: 0x06001E87 RID: 7815
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetDefaultValue(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, out long value, out int length, out int corElementType);

		// Token: 0x06001E88 RID: 7816 RVA: 0x0004D5B4 File Offset: 0x0004C5B4
		public void GetDefaultValue(int mdToken, out long value, out int length, out CorElementType corElementType)
		{
			int num;
			MetadataImport._GetDefaultValue(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, out value, out length, out num);
			corElementType = (CorElementType)num;
		}

		// Token: 0x06001E89 RID: 7817
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetUserString(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** name, out int length);

		// Token: 0x06001E8A RID: 7818 RVA: 0x0004D5DC File Offset: 0x0004C5DC
		public unsafe string GetUserString(int mdToken)
		{
			void* ptr;
			int num;
			MetadataImport._GetUserString(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &ptr, out num);
			if (ptr == null)
			{
				return null;
			}
			char[] array = new char[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (char)(*(ushort*)((byte*)ptr + (IntPtr)i * 2));
			}
			return new string(array);
		}

		// Token: 0x06001E8B RID: 7819
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetName(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** name);

		// Token: 0x06001E8C RID: 7820 RVA: 0x0004D62C File Offset: 0x0004C62C
		public unsafe Utf8String GetName(int mdToken)
		{
			void* ptr;
			MetadataImport._GetName(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &ptr);
			return new Utf8String(ptr);
		}

		// Token: 0x06001E8D RID: 7821
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetNamespace(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** namesp);

		// Token: 0x06001E8E RID: 7822 RVA: 0x0004D654 File Offset: 0x0004C654
		public unsafe Utf8String GetNamespace(int mdToken)
		{
			void* ptr;
			MetadataImport._GetNamespace(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &ptr);
			return new Utf8String(ptr);
		}

		// Token: 0x06001E8F RID: 7823
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetEventProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** name, out int eventAttributes);

		// Token: 0x06001E90 RID: 7824 RVA: 0x0004D67C File Offset: 0x0004C67C
		public unsafe void GetEventProps(int mdToken, out void* name, out EventAttributes eventAttributes)
		{
			void* ptr;
			int num;
			MetadataImport._GetEventProps(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &ptr, out num);
			name = ptr;
			eventAttributes = (EventAttributes)num;
		}

		// Token: 0x06001E91 RID: 7825
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetFieldDefProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, out int fieldAttributes);

		// Token: 0x06001E92 RID: 7826 RVA: 0x0004D6A8 File Offset: 0x0004C6A8
		public void GetFieldDefProps(int mdToken, out FieldAttributes fieldAttributes)
		{
			int num;
			MetadataImport._GetFieldDefProps(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, out num);
			fieldAttributes = (FieldAttributes)num;
		}

		// Token: 0x06001E93 RID: 7827
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetPropertyProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, void** name, out int propertyAttributes, out ConstArray signature);

		// Token: 0x06001E94 RID: 7828 RVA: 0x0004D6CC File Offset: 0x0004C6CC
		public unsafe void GetPropertyProps(int mdToken, out void* name, out PropertyAttributes propertyAttributes, out ConstArray signature)
		{
			void* ptr;
			int num;
			MetadataImport._GetPropertyProps(this.m_metadataImport2, out MetadataArgs.Skip, mdToken, &ptr, out num, out signature);
			name = ptr;
			propertyAttributes = (PropertyAttributes)num;
		}

		// Token: 0x06001E95 RID: 7829
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetParentToken(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int mdToken, out int tkParent);

		// Token: 0x06001E96 RID: 7830 RVA: 0x0004D6F8 File Offset: 0x0004C6F8
		public int GetParentToken(int tkToken)
		{
			int num;
			MetadataImport._GetParentToken(this.m_metadataImport2, out MetadataArgs.Skip, tkToken, out num);
			return num;
		}

		// Token: 0x06001E97 RID: 7831
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetParamDefProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int parameterToken, out int sequence, out int attributes);

		// Token: 0x06001E98 RID: 7832 RVA: 0x0004D71C File Offset: 0x0004C71C
		public void GetParamDefProps(int parameterToken, out int sequence, out ParameterAttributes attributes)
		{
			int num;
			MetadataImport._GetParamDefProps(this.m_metadataImport2, out MetadataArgs.Skip, parameterToken, out sequence, out num);
			attributes = (ParameterAttributes)num;
		}

		// Token: 0x06001E99 RID: 7833
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetGenericParamProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int genericParameter, out int flags);

		// Token: 0x06001E9A RID: 7834 RVA: 0x0004D740 File Offset: 0x0004C740
		public void GetGenericParamProps(int genericParameter, out GenericParameterAttributes attributes)
		{
			int num;
			MetadataImport._GetGenericParamProps(this.m_metadataImport2, out MetadataArgs.Skip, genericParameter, out num);
			attributes = (GenericParameterAttributes)num;
		}

		// Token: 0x06001E9B RID: 7835
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetScopeProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, out Guid mvid);

		// Token: 0x06001E9C RID: 7836 RVA: 0x0004D763 File Offset: 0x0004C763
		public void GetScopeProps(out Guid mvid)
		{
			MetadataImport._GetScopeProps(this.m_metadataImport2, out MetadataArgs.Skip, out mvid);
		}

		// Token: 0x06001E9D RID: 7837 RVA: 0x0004D776 File Offset: 0x0004C776
		public ConstArray GetMethodSignature(MetadataToken token)
		{
			if (token.IsMemberRef)
			{
				return this.GetMemberRefProps(token);
			}
			return this.GetSigOfMethodDef(token);
		}

		// Token: 0x06001E9E RID: 7838
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetSigOfMethodDef(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int methodToken, ref ConstArray signature);

		// Token: 0x06001E9F RID: 7839 RVA: 0x0004D79C File Offset: 0x0004C79C
		public ConstArray GetSigOfMethodDef(int methodToken)
		{
			ConstArray constArray = default(ConstArray);
			MetadataImport._GetSigOfMethodDef(this.m_metadataImport2, out MetadataArgs.Skip, methodToken, ref constArray);
			return constArray;
		}

		// Token: 0x06001EA0 RID: 7840
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetSignatureFromToken(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int methodToken, ref ConstArray signature);

		// Token: 0x06001EA1 RID: 7841 RVA: 0x0004D7C8 File Offset: 0x0004C7C8
		public ConstArray GetSignatureFromToken(int token)
		{
			ConstArray constArray = default(ConstArray);
			MetadataImport._GetSignatureFromToken(this.m_metadataImport2, out MetadataArgs.Skip, token, ref constArray);
			return constArray;
		}

		// Token: 0x06001EA2 RID: 7842
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetMemberRefProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int memberTokenRef, out ConstArray signature);

		// Token: 0x06001EA3 RID: 7843 RVA: 0x0004D7F4 File Offset: 0x0004C7F4
		public ConstArray GetMemberRefProps(int memberTokenRef)
		{
			ConstArray constArray = default(ConstArray);
			MetadataImport._GetMemberRefProps(this.m_metadataImport2, out MetadataArgs.Skip, memberTokenRef, out constArray);
			return constArray;
		}

		// Token: 0x06001EA4 RID: 7844
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetCustomAttributeProps(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int customAttributeToken, out int constructorToken, out ConstArray signature);

		// Token: 0x06001EA5 RID: 7845 RVA: 0x0004D81D File Offset: 0x0004C81D
		public void GetCustomAttributeProps(int customAttributeToken, out int constructorToken, out ConstArray signature)
		{
			MetadataImport._GetCustomAttributeProps(this.m_metadataImport2, out MetadataArgs.Skip, customAttributeToken, out constructorToken, out signature);
		}

		// Token: 0x06001EA6 RID: 7846
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetClassLayout(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int typeTokenDef, out int packSize, out int classSize);

		// Token: 0x06001EA7 RID: 7847 RVA: 0x0004D832 File Offset: 0x0004C832
		public void GetClassLayout(int typeTokenDef, out int packSize, out int classSize)
		{
			MetadataImport._GetClassLayout(this.m_metadataImport2, out MetadataArgs.Skip, typeTokenDef, out packSize, out classSize);
		}

		// Token: 0x06001EA8 RID: 7848
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool _GetFieldOffset(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int typeTokenDef, int fieldTokenDef, out int offset);

		// Token: 0x06001EA9 RID: 7849 RVA: 0x0004D847 File Offset: 0x0004C847
		public bool GetFieldOffset(int typeTokenDef, int fieldTokenDef, out int offset)
		{
			return MetadataImport._GetFieldOffset(this.m_metadataImport2, out MetadataArgs.Skip, typeTokenDef, fieldTokenDef, out offset);
		}

		// Token: 0x06001EAA RID: 7850
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetSigOfFieldDef(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int fieldToken, ref ConstArray fieldMarshal);

		// Token: 0x06001EAB RID: 7851 RVA: 0x0004D85C File Offset: 0x0004C85C
		public ConstArray GetSigOfFieldDef(int fieldToken)
		{
			ConstArray constArray = default(ConstArray);
			MetadataImport._GetSigOfFieldDef(this.m_metadataImport2, out MetadataArgs.Skip, fieldToken, ref constArray);
			return constArray;
		}

		// Token: 0x06001EAC RID: 7852
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern void _GetFieldMarshal(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int fieldToken, ref ConstArray fieldMarshal);

		// Token: 0x06001EAD RID: 7853 RVA: 0x0004D888 File Offset: 0x0004C888
		public ConstArray GetFieldMarshal(int fieldToken)
		{
			ConstArray constArray = default(ConstArray);
			MetadataImport._GetFieldMarshal(this.m_metadataImport2, out MetadataArgs.Skip, fieldToken, ref constArray);
			return constArray;
		}

		// Token: 0x06001EAE RID: 7854
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern void _GetPInvokeMap(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int token, out int attributes, void** importName, void** importDll);

		// Token: 0x06001EAF RID: 7855 RVA: 0x0004D8B4 File Offset: 0x0004C8B4
		public unsafe void GetPInvokeMap(int token, out PInvokeAttributes attributes, out string importName, out string importDll)
		{
			int num;
			void* ptr;
			void* ptr2;
			MetadataImport._GetPInvokeMap(this.m_metadataImport2, out MetadataArgs.Skip, token, out num, &ptr, &ptr2);
			importName = new Utf8String(ptr).ToString();
			importDll = new Utf8String(ptr2).ToString();
			attributes = (PInvokeAttributes)num;
		}

		// Token: 0x06001EB0 RID: 7856
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool _IsValidToken(IntPtr scope, out MetadataArgs.SkipAddresses skipAddresses, int token);

		// Token: 0x06001EB1 RID: 7857 RVA: 0x0004D90B File Offset: 0x0004C90B
		public bool IsValidToken(int token)
		{
			return MetadataImport._IsValidToken(this.m_metadataImport2, out MetadataArgs.Skip, token);
		}

		// Token: 0x04000CD2 RID: 3282
		private IntPtr m_metadataImport2;

		// Token: 0x04000CD3 RID: 3283
		internal static readonly MetadataImport EmptyImport = new MetadataImport((IntPtr)0);

		// Token: 0x04000CD4 RID: 3284
		internal static Guid IID_IMetaDataImport = new Guid(3530420970U, 32600, 16771, 134, 190, 48, 174, 41, 167, 93, 141);

		// Token: 0x04000CD5 RID: 3285
		internal static Guid IID_IMetaDataAssemblyImport = new Guid(3999418123U, 59723, 16974, 155, 124, 47, 0, 201, 36, 159, 147);

		// Token: 0x04000CD6 RID: 3286
		internal static Guid IID_IMetaDataTables = new Guid(3639966123U, 16429, 19342, 130, 217, 93, 99, 177, 6, 92, 104);
	}
}
