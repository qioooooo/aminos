using System;
using System.Reflection;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004DF RID: 1247
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue, Inherited = false)]
	[ComVisible(true)]
	public sealed class MarshalAsAttribute : Attribute
	{
		// Token: 0x06003117 RID: 12567 RVA: 0x000A8E85 File Offset: 0x000A7E85
		internal static Attribute GetCustomAttribute(ParameterInfo parameter)
		{
			return MarshalAsAttribute.GetCustomAttribute(parameter.MetadataToken, parameter.Member.Module);
		}

		// Token: 0x06003118 RID: 12568 RVA: 0x000A8E9D File Offset: 0x000A7E9D
		internal static bool IsDefined(ParameterInfo parameter)
		{
			return MarshalAsAttribute.GetCustomAttribute(parameter) != null;
		}

		// Token: 0x06003119 RID: 12569 RVA: 0x000A8EAB File Offset: 0x000A7EAB
		internal static Attribute GetCustomAttribute(RuntimeFieldInfo field)
		{
			return MarshalAsAttribute.GetCustomAttribute(field.MetadataToken, field.Module);
		}

		// Token: 0x0600311A RID: 12570 RVA: 0x000A8EBE File Offset: 0x000A7EBE
		internal static bool IsDefined(RuntimeFieldInfo field)
		{
			return MarshalAsAttribute.GetCustomAttribute(field) != null;
		}

		// Token: 0x0600311B RID: 12571 RVA: 0x000A8ECC File Offset: 0x000A7ECC
		internal static Attribute GetCustomAttribute(int token, Module scope)
		{
			int num = 0;
			int num2 = 0;
			string text = null;
			string text2 = null;
			string text3 = null;
			int num3 = 0;
			ConstArray fieldMarshal = scope.ModuleHandle.GetMetadataImport().GetFieldMarshal(token);
			if (fieldMarshal.Length == 0)
			{
				return null;
			}
			UnmanagedType unmanagedType;
			VarEnum varEnum;
			UnmanagedType unmanagedType2;
			MetadataImport.GetMarshalAs(fieldMarshal, out unmanagedType, out varEnum, out text3, out unmanagedType2, out num, out num2, out text, out text2, out num3);
			Type type = ((text3 == null || text3.Length == 0) ? null : RuntimeTypeHandle.GetTypeByNameUsingCARules(text3, scope));
			Type type2 = null;
			try
			{
				type2 = ((text == null) ? null : RuntimeTypeHandle.GetTypeByNameUsingCARules(text, scope));
			}
			catch (TypeLoadException)
			{
			}
			return new MarshalAsAttribute(unmanagedType, varEnum, type, unmanagedType2, (short)num, num2, text, type2, text2, num3);
		}

		// Token: 0x0600311C RID: 12572 RVA: 0x000A8F84 File Offset: 0x000A7F84
		internal MarshalAsAttribute(UnmanagedType val, VarEnum safeArraySubType, Type safeArrayUserDefinedSubType, UnmanagedType arraySubType, short sizeParamIndex, int sizeConst, string marshalType, Type marshalTypeRef, string marshalCookie, int iidParamIndex)
		{
			this._val = val;
			this.SafeArraySubType = safeArraySubType;
			this.SafeArrayUserDefinedSubType = safeArrayUserDefinedSubType;
			this.IidParameterIndex = iidParamIndex;
			this.ArraySubType = arraySubType;
			this.SizeParamIndex = sizeParamIndex;
			this.SizeConst = sizeConst;
			this.MarshalType = marshalType;
			this.MarshalTypeRef = marshalTypeRef;
			this.MarshalCookie = marshalCookie;
		}

		// Token: 0x0600311D RID: 12573 RVA: 0x000A8FE4 File Offset: 0x000A7FE4
		public MarshalAsAttribute(UnmanagedType unmanagedType)
		{
			this._val = unmanagedType;
		}

		// Token: 0x0600311E RID: 12574 RVA: 0x000A8FF3 File Offset: 0x000A7FF3
		public MarshalAsAttribute(short unmanagedType)
		{
			this._val = (UnmanagedType)unmanagedType;
		}

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x0600311F RID: 12575 RVA: 0x000A9002 File Offset: 0x000A8002
		public UnmanagedType Value
		{
			get
			{
				return this._val;
			}
		}

		// Token: 0x04001936 RID: 6454
		internal UnmanagedType _val;

		// Token: 0x04001937 RID: 6455
		public VarEnum SafeArraySubType;

		// Token: 0x04001938 RID: 6456
		public Type SafeArrayUserDefinedSubType;

		// Token: 0x04001939 RID: 6457
		public int IidParameterIndex;

		// Token: 0x0400193A RID: 6458
		public UnmanagedType ArraySubType;

		// Token: 0x0400193B RID: 6459
		public short SizeParamIndex;

		// Token: 0x0400193C RID: 6460
		public int SizeConst;

		// Token: 0x0400193D RID: 6461
		[ComVisible(true)]
		public string MarshalType;

		// Token: 0x0400193E RID: 6462
		[ComVisible(true)]
		public Type MarshalTypeRef;

		// Token: 0x0400193F RID: 6463
		public string MarshalCookie;
	}
}
