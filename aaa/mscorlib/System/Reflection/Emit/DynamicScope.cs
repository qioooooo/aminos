using System;
using System.Collections;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x02000806 RID: 2054
	internal class DynamicScope
	{
		// Token: 0x06004972 RID: 18802 RVA: 0x0010097D File Offset: 0x000FF97D
		internal DynamicScope()
		{
			this.m_tokens = new ArrayList();
			this.m_tokens.Add(null);
		}

		// Token: 0x17000CA7 RID: 3239
		internal object this[int token]
		{
			get
			{
				token &= 16777215;
				if (token < 0 || token > this.m_tokens.Count)
				{
					return null;
				}
				return this.m_tokens[token];
			}
		}

		// Token: 0x06004974 RID: 18804 RVA: 0x001009C8 File Offset: 0x000FF9C8
		internal int GetTokenFor(VarArgMethod varArgMethod)
		{
			return this.m_tokens.Add(varArgMethod) | 167772160;
		}

		// Token: 0x06004975 RID: 18805 RVA: 0x001009DC File Offset: 0x000FF9DC
		internal string GetString(int token)
		{
			return this[token] as string;
		}

		// Token: 0x06004976 RID: 18806 RVA: 0x001009EC File Offset: 0x000FF9EC
		internal byte[] ResolveSignature(int token, int fromMethod)
		{
			if (fromMethod == 0)
			{
				return (byte[])this[token];
			}
			VarArgMethod varArgMethod = this[token] as VarArgMethod;
			if (varArgMethod == null)
			{
				return null;
			}
			return varArgMethod.m_signature.GetSignature(true);
		}

		// Token: 0x06004977 RID: 18807 RVA: 0x00100A28 File Offset: 0x000FFA28
		public int GetTokenFor(RuntimeMethodHandle method)
		{
			MethodBase methodBase = RuntimeType.GetMethodBase(method);
			if (methodBase.DeclaringType != null && methodBase.DeclaringType.IsGenericType)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_MethodDeclaringTypeGenericLcg"), new object[]
				{
					methodBase,
					methodBase.DeclaringType.GetGenericTypeDefinition()
				}));
			}
			return this.m_tokens.Add(method) | 100663296;
		}

		// Token: 0x06004978 RID: 18808 RVA: 0x00100A9C File Offset: 0x000FFA9C
		public int GetTokenFor(RuntimeMethodHandle method, RuntimeTypeHandle typeContext)
		{
			return this.m_tokens.Add(new GenericMethodInfo(method, typeContext)) | 100663296;
		}

		// Token: 0x06004979 RID: 18809 RVA: 0x00100AB6 File Offset: 0x000FFAB6
		public int GetTokenFor(DynamicMethod method)
		{
			return this.m_tokens.Add(method) | 100663296;
		}

		// Token: 0x0600497A RID: 18810 RVA: 0x00100ACA File Offset: 0x000FFACA
		public int GetTokenFor(RuntimeFieldHandle field)
		{
			return this.m_tokens.Add(field) | 67108864;
		}

		// Token: 0x0600497B RID: 18811 RVA: 0x00100AE3 File Offset: 0x000FFAE3
		public int GetTokenFor(RuntimeFieldHandle field, RuntimeTypeHandle typeContext)
		{
			return this.m_tokens.Add(new GenericFieldInfo(field, typeContext)) | 67108864;
		}

		// Token: 0x0600497C RID: 18812 RVA: 0x00100AFD File Offset: 0x000FFAFD
		public int GetTokenFor(RuntimeTypeHandle type)
		{
			return this.m_tokens.Add(type) | 33554432;
		}

		// Token: 0x0600497D RID: 18813 RVA: 0x00100B16 File Offset: 0x000FFB16
		public int GetTokenFor(string literal)
		{
			return this.m_tokens.Add(literal) | 1879048192;
		}

		// Token: 0x0600497E RID: 18814 RVA: 0x00100B2A File Offset: 0x000FFB2A
		public int GetTokenFor(byte[] signature)
		{
			return this.m_tokens.Add(signature) | 285212672;
		}

		// Token: 0x04002574 RID: 9588
		internal ArrayList m_tokens;
	}
}
