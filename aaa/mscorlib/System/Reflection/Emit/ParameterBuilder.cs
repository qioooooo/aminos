using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x0200082D RID: 2093
	[ClassInterface(ClassInterfaceType.None)]
	[ComDefaultInterface(typeof(_ParameterBuilder))]
	[ComVisible(true)]
	public class ParameterBuilder : _ParameterBuilder
	{
		// Token: 0x06004B94 RID: 19348 RVA: 0x00109EEC File Offset: 0x00108EEC
		[Obsolete("An alternate API is available: Emit the MarshalAs custom attribute instead. http://go.microsoft.com/fwlink/?linkid=14202")]
		public virtual void SetMarshal(UnmanagedMarshal unmanagedMarshal)
		{
			if (unmanagedMarshal == null)
			{
				throw new ArgumentNullException("unmanagedMarshal");
			}
			byte[] array = unmanagedMarshal.InternalGetBytes();
			TypeBuilder.InternalSetMarshalInfo(this.m_methodBuilder.GetModule(), this.m_pdToken.Token, array, array.Length);
		}

		// Token: 0x06004B95 RID: 19349 RVA: 0x00109F30 File Offset: 0x00108F30
		public virtual void SetConstant(object defaultValue)
		{
			TypeBuilder.SetConstantValue(this.m_methodBuilder.GetModule(), this.m_pdToken.Token, (this.m_iPosition == 0) ? this.m_methodBuilder.m_returnType : this.m_methodBuilder.m_parameterTypes[this.m_iPosition - 1], defaultValue);
		}

		// Token: 0x06004B96 RID: 19350 RVA: 0x00109F84 File Offset: 0x00108F84
		[ComVisible(true)]
		public void SetCustomAttribute(ConstructorInfo con, byte[] binaryAttribute)
		{
			if (con == null)
			{
				throw new ArgumentNullException("con");
			}
			if (binaryAttribute == null)
			{
				throw new ArgumentNullException("binaryAttribute");
			}
			TypeBuilder.InternalCreateCustomAttribute(this.m_pdToken.Token, ((ModuleBuilder)this.m_methodBuilder.GetModule()).GetConstructorToken(con).Token, binaryAttribute, this.m_methodBuilder.GetModule(), false);
		}

		// Token: 0x06004B97 RID: 19351 RVA: 0x00109FE8 File Offset: 0x00108FE8
		public void SetCustomAttribute(CustomAttributeBuilder customBuilder)
		{
			if (customBuilder == null)
			{
				throw new ArgumentNullException("customBuilder");
			}
			customBuilder.CreateCustomAttribute((ModuleBuilder)this.m_methodBuilder.GetModule(), this.m_pdToken.Token);
		}

		// Token: 0x06004B98 RID: 19352 RVA: 0x0010A019 File Offset: 0x00109019
		private ParameterBuilder()
		{
		}

		// Token: 0x06004B99 RID: 19353 RVA: 0x0010A024 File Offset: 0x00109024
		internal ParameterBuilder(MethodBuilder methodBuilder, int sequence, ParameterAttributes attributes, string strParamName)
		{
			this.m_iPosition = sequence;
			this.m_strParamName = strParamName;
			this.m_methodBuilder = methodBuilder;
			this.m_strParamName = strParamName;
			this.m_attributes = attributes;
			this.m_pdToken = new ParameterToken(TypeBuilder.InternalSetParamInfo(this.m_methodBuilder.GetModule(), this.m_methodBuilder.GetToken().Token, sequence, attributes, strParamName));
		}

		// Token: 0x06004B9A RID: 19354 RVA: 0x0010A08E File Offset: 0x0010908E
		public virtual ParameterToken GetToken()
		{
			return this.m_pdToken;
		}

		// Token: 0x06004B9B RID: 19355 RVA: 0x0010A096 File Offset: 0x00109096
		void _ParameterBuilder.GetTypeInfoCount(out uint pcTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B9C RID: 19356 RVA: 0x0010A09D File Offset: 0x0010909D
		void _ParameterBuilder.GetTypeInfo(uint iTInfo, uint lcid, IntPtr ppTInfo)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B9D RID: 19357 RVA: 0x0010A0A4 File Offset: 0x001090A4
		void _ParameterBuilder.GetIDsOfNames([In] ref Guid riid, IntPtr rgszNames, uint cNames, uint lcid, IntPtr rgDispId)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06004B9E RID: 19358 RVA: 0x0010A0AB File Offset: 0x001090AB
		void _ParameterBuilder.Invoke(uint dispIdMember, [In] ref Guid riid, uint lcid, short wFlags, IntPtr pDispParams, IntPtr pVarResult, IntPtr pExcepInfo, IntPtr puArgErr)
		{
			throw new NotImplementedException();
		}

		// Token: 0x17000D0B RID: 3339
		// (get) Token: 0x06004B9F RID: 19359 RVA: 0x0010A0B2 File Offset: 0x001090B2
		internal virtual int MetadataTokenInternal
		{
			get
			{
				return this.m_pdToken.Token;
			}
		}

		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x06004BA0 RID: 19360 RVA: 0x0010A0BF File Offset: 0x001090BF
		public virtual string Name
		{
			get
			{
				return this.m_strParamName;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x06004BA1 RID: 19361 RVA: 0x0010A0C7 File Offset: 0x001090C7
		public virtual int Position
		{
			get
			{
				return this.m_iPosition;
			}
		}

		// Token: 0x17000D0E RID: 3342
		// (get) Token: 0x06004BA2 RID: 19362 RVA: 0x0010A0CF File Offset: 0x001090CF
		public virtual int Attributes
		{
			get
			{
				return (int)this.m_attributes;
			}
		}

		// Token: 0x17000D0F RID: 3343
		// (get) Token: 0x06004BA3 RID: 19363 RVA: 0x0010A0D7 File Offset: 0x001090D7
		public bool IsIn
		{
			get
			{
				return (this.m_attributes & ParameterAttributes.In) != ParameterAttributes.None;
			}
		}

		// Token: 0x17000D10 RID: 3344
		// (get) Token: 0x06004BA4 RID: 19364 RVA: 0x0010A0E7 File Offset: 0x001090E7
		public bool IsOut
		{
			get
			{
				return (this.m_attributes & ParameterAttributes.Out) != ParameterAttributes.None;
			}
		}

		// Token: 0x17000D11 RID: 3345
		// (get) Token: 0x06004BA5 RID: 19365 RVA: 0x0010A0F7 File Offset: 0x001090F7
		public bool IsOptional
		{
			get
			{
				return (this.m_attributes & ParameterAttributes.Optional) != ParameterAttributes.None;
			}
		}

		// Token: 0x04002779 RID: 10105
		private string m_strParamName;

		// Token: 0x0400277A RID: 10106
		private int m_iPosition;

		// Token: 0x0400277B RID: 10107
		private ParameterAttributes m_attributes;

		// Token: 0x0400277C RID: 10108
		private MethodBuilder m_methodBuilder;

		// Token: 0x0400277D RID: 10109
		private ParameterToken m_pdToken;
	}
}
