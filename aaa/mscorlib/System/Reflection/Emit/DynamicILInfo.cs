using System;
using System.Runtime.InteropServices;

namespace System.Reflection.Emit
{
	// Token: 0x02000805 RID: 2053
	[ComVisible(true)]
	public class DynamicILInfo
	{
		// Token: 0x0600495D RID: 18781 RVA: 0x001006F4 File Offset: 0x000FF6F4
		internal DynamicILInfo(DynamicScope scope, DynamicMethod method, byte[] methodSignature)
		{
			this.m_method = method;
			this.m_scope = scope;
			this.m_methodSignature = this.m_scope.GetTokenFor(methodSignature);
			this.m_exceptions = new byte[0];
			this.m_code = new byte[0];
			this.m_localSignature = new byte[0];
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x0010074B File Offset: 0x000FF74B
		internal unsafe RuntimeMethodHandle GetCallableMethod(void* module)
		{
			return new RuntimeMethodHandle(ModuleHandle.GetDynamicMethod(module, this.m_method.Name, (byte[])this.m_scope[this.m_methodSignature], new DynamicResolver(this)));
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x0600495F RID: 18783 RVA: 0x0010077F File Offset: 0x000FF77F
		internal byte[] LocalSignature
		{
			get
			{
				if (this.m_localSignature == null)
				{
					this.m_localSignature = SignatureHelper.GetLocalVarSigHelper().InternalGetSignatureArray();
				}
				return this.m_localSignature;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x06004960 RID: 18784 RVA: 0x0010079F File Offset: 0x000FF79F
		internal byte[] Exceptions
		{
			get
			{
				return this.m_exceptions;
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x06004961 RID: 18785 RVA: 0x001007A7 File Offset: 0x000FF7A7
		internal byte[] Code
		{
			get
			{
				return this.m_code;
			}
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06004962 RID: 18786 RVA: 0x001007AF File Offset: 0x000FF7AF
		internal int MaxStackSize
		{
			get
			{
				return this.m_maxStackSize;
			}
		}

		// Token: 0x17000CA5 RID: 3237
		// (get) Token: 0x06004963 RID: 18787 RVA: 0x001007B7 File Offset: 0x000FF7B7
		public DynamicMethod DynamicMethod
		{
			get
			{
				return this.m_method;
			}
		}

		// Token: 0x17000CA6 RID: 3238
		// (get) Token: 0x06004964 RID: 18788 RVA: 0x001007BF File Offset: 0x000FF7BF
		internal DynamicScope DynamicScope
		{
			get
			{
				return this.m_scope;
			}
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x001007C7 File Offset: 0x000FF7C7
		public void SetCode(byte[] code, int maxStackSize)
		{
			if (code == null)
			{
				code = new byte[0];
			}
			this.m_code = (byte[])code.Clone();
			this.m_maxStackSize = maxStackSize;
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x001007EC File Offset: 0x000FF7EC
		[CLSCompliant(false)]
		public unsafe void SetCode(byte* code, int codeSize, int maxStackSize)
		{
			if (codeSize < 0)
			{
				throw new ArgumentOutOfRangeException("codeSize", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			this.m_code = new byte[codeSize];
			for (int i = 0; i < codeSize; i++)
			{
				this.m_code[i] = *code;
				code++;
			}
			this.m_maxStackSize = maxStackSize;
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x00100841 File Offset: 0x000FF841
		public void SetExceptions(byte[] exceptions)
		{
			if (exceptions == null)
			{
				exceptions = new byte[0];
			}
			this.m_exceptions = (byte[])exceptions.Clone();
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x00100860 File Offset: 0x000FF860
		[CLSCompliant(false)]
		public unsafe void SetExceptions(byte* exceptions, int exceptionsSize)
		{
			if (exceptionsSize < 0)
			{
				throw new ArgumentOutOfRangeException("exceptionsSize", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			this.m_exceptions = new byte[exceptionsSize];
			for (int i = 0; i < exceptionsSize; i++)
			{
				this.m_exceptions[i] = *exceptions;
				exceptions++;
			}
		}

		// Token: 0x06004969 RID: 18793 RVA: 0x001008AE File Offset: 0x000FF8AE
		public void SetLocalSignature(byte[] localSignature)
		{
			if (localSignature == null)
			{
				localSignature = new byte[0];
			}
			this.m_localSignature = (byte[])localSignature.Clone();
		}

		// Token: 0x0600496A RID: 18794 RVA: 0x001008CC File Offset: 0x000FF8CC
		[CLSCompliant(false)]
		public unsafe void SetLocalSignature(byte* localSignature, int signatureSize)
		{
			if (signatureSize < 0)
			{
				throw new ArgumentOutOfRangeException("signatureSize", Environment.GetResourceString("ArgumentOutOfRange_GenericPositive"));
			}
			this.m_localSignature = new byte[signatureSize];
			for (int i = 0; i < signatureSize; i++)
			{
				this.m_localSignature[i] = *localSignature;
				localSignature++;
			}
		}

		// Token: 0x0600496B RID: 18795 RVA: 0x0010091A File Offset: 0x000FF91A
		public int GetTokenFor(RuntimeMethodHandle method)
		{
			return this.DynamicScope.GetTokenFor(method);
		}

		// Token: 0x0600496C RID: 18796 RVA: 0x00100928 File Offset: 0x000FF928
		public int GetTokenFor(DynamicMethod method)
		{
			return this.DynamicScope.GetTokenFor(method);
		}

		// Token: 0x0600496D RID: 18797 RVA: 0x00100936 File Offset: 0x000FF936
		public int GetTokenFor(RuntimeMethodHandle method, RuntimeTypeHandle contextType)
		{
			return this.DynamicScope.GetTokenFor(method, contextType);
		}

		// Token: 0x0600496E RID: 18798 RVA: 0x00100945 File Offset: 0x000FF945
		public int GetTokenFor(RuntimeFieldHandle field)
		{
			return this.DynamicScope.GetTokenFor(field);
		}

		// Token: 0x0600496F RID: 18799 RVA: 0x00100953 File Offset: 0x000FF953
		public int GetTokenFor(RuntimeTypeHandle type)
		{
			return this.DynamicScope.GetTokenFor(type);
		}

		// Token: 0x06004970 RID: 18800 RVA: 0x00100961 File Offset: 0x000FF961
		public int GetTokenFor(string literal)
		{
			return this.DynamicScope.GetTokenFor(literal);
		}

		// Token: 0x06004971 RID: 18801 RVA: 0x0010096F File Offset: 0x000FF96F
		public int GetTokenFor(byte[] signature)
		{
			return this.DynamicScope.GetTokenFor(signature);
		}

		// Token: 0x0400256D RID: 9581
		private DynamicMethod m_method;

		// Token: 0x0400256E RID: 9582
		private DynamicScope m_scope;

		// Token: 0x0400256F RID: 9583
		private byte[] m_exceptions;

		// Token: 0x04002570 RID: 9584
		private byte[] m_code;

		// Token: 0x04002571 RID: 9585
		private byte[] m_localSignature;

		// Token: 0x04002572 RID: 9586
		private int m_maxStackSize;

		// Token: 0x04002573 RID: 9587
		private int m_methodSignature;
	}
}
