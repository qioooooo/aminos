using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x02000820 RID: 2080
	internal sealed class SymbolMethod : MethodInfo
	{
		// Token: 0x06004AE4 RID: 19172 RVA: 0x0010476C File Offset: 0x0010376C
		internal SymbolMethod(ModuleBuilder mod, MethodToken token, Type arrayClass, string methodName, CallingConventions callingConvention, Type returnType, Type[] parameterTypes)
		{
			this.m_mdMethod = token;
			this.m_tkMethod = token.Token;
			this.m_returnType = returnType;
			if (parameterTypes != null)
			{
				this.m_parameterTypes = new Type[parameterTypes.Length];
				Array.Copy(parameterTypes, this.m_parameterTypes, parameterTypes.Length);
			}
			else
			{
				this.m_parameterTypes = new Type[0];
			}
			this.m_module = mod;
			this.m_containingType = arrayClass;
			this.m_name = methodName;
			this.m_callingConvention = callingConvention;
			this.m_signature = SignatureHelper.GetMethodSigHelper(mod, callingConvention, returnType, null, null, parameterTypes, null, null);
		}

		// Token: 0x06004AE5 RID: 19173 RVA: 0x00104801 File Offset: 0x00103801
		internal override Type[] GetParameterTypes()
		{
			return this.m_parameterTypes;
		}

		// Token: 0x06004AE6 RID: 19174 RVA: 0x00104809 File Offset: 0x00103809
		internal MethodToken GetToken(ModuleBuilder mod)
		{
			return mod.GetArrayMethodToken(this.m_containingType, this.m_name, this.m_callingConvention, this.m_returnType, this.m_parameterTypes);
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x06004AE7 RID: 19175 RVA: 0x0010482F File Offset: 0x0010382F
		public override Module Module
		{
			get
			{
				return this.m_module;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06004AE8 RID: 19176 RVA: 0x00104837 File Offset: 0x00103837
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_tkMethod;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06004AE9 RID: 19177 RVA: 0x0010483F File Offset: 0x0010383F
		public override Type ReflectedType
		{
			get
			{
				return this.m_containingType;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06004AEA RID: 19178 RVA: 0x00104847 File Offset: 0x00103847
		public override string Name
		{
			get
			{
				return this.m_name;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06004AEB RID: 19179 RVA: 0x0010484F File Offset: 0x0010384F
		public override Type DeclaringType
		{
			get
			{
				return this.m_containingType;
			}
		}

		// Token: 0x06004AEC RID: 19180 RVA: 0x00104857 File Offset: 0x00103857
		public override ParameterInfo[] GetParameters()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004AED RID: 19181 RVA: 0x00104868 File Offset: 0x00103868
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06004AEE RID: 19182 RVA: 0x00104879 File Offset: 0x00103879
		public override MethodAttributes Attributes
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
			}
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x06004AEF RID: 19183 RVA: 0x0010488A File Offset: 0x0010388A
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_callingConvention;
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x00104892 File Offset: 0x00103892
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
			}
		}

		// Token: 0x06004AF1 RID: 19185 RVA: 0x001048A3 File Offset: 0x001038A3
		internal override Type GetReturnType()
		{
			return this.m_returnType;
		}

		// Token: 0x17000CFE RID: 3326
		// (get) Token: 0x06004AF2 RID: 19186 RVA: 0x001048AB File Offset: 0x001038AB
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x001048AE File Offset: 0x001038AE
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x001048BF File Offset: 0x001038BF
		public override MethodInfo GetBaseDefinition()
		{
			return this;
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x001048C2 File Offset: 0x001038C2
		public override object[] GetCustomAttributes(bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x001048D3 File Offset: 0x001038D3
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x001048E4 File Offset: 0x001038E4
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			throw new NotSupportedException(Environment.GetResourceString("NotSupported_SymbolMethod"));
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x001048F5 File Offset: 0x001038F5
		public Module GetModule()
		{
			return this.m_module;
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x001048FD File Offset: 0x001038FD
		public MethodToken GetToken()
		{
			return this.m_mdMethod;
		}

		// Token: 0x04002613 RID: 9747
		private ModuleBuilder m_module;

		// Token: 0x04002614 RID: 9748
		private Type m_containingType;

		// Token: 0x04002615 RID: 9749
		private string m_name;

		// Token: 0x04002616 RID: 9750
		private CallingConventions m_callingConvention;

		// Token: 0x04002617 RID: 9751
		private Type m_returnType;

		// Token: 0x04002618 RID: 9752
		private MethodToken m_mdMethod;

		// Token: 0x04002619 RID: 9753
		private int m_tkMethod;

		// Token: 0x0400261A RID: 9754
		private Type[] m_parameterTypes;

		// Token: 0x0400261B RID: 9755
		private SignatureHelper m_signature;
	}
}
