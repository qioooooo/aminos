using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x0200081D RID: 2077
	internal sealed class MethodBuilderInstantiation : MethodInfo
	{
		// Token: 0x06004A91 RID: 19089 RVA: 0x00103E97 File Offset: 0x00102E97
		internal static MethodInfo MakeGenericMethod(MethodInfo method, Type[] inst)
		{
			if (!method.IsGenericMethodDefinition)
			{
				throw new InvalidOperationException();
			}
			return new MethodBuilderInstantiation(method, inst);
		}

		// Token: 0x06004A92 RID: 19090 RVA: 0x00103EAE File Offset: 0x00102EAE
		internal MethodBuilderInstantiation(MethodInfo method, Type[] inst)
		{
			this.m_method = method;
			this.m_inst = inst;
		}

		// Token: 0x06004A93 RID: 19091 RVA: 0x00103EC4 File Offset: 0x00102EC4
		internal override Type[] GetParameterTypes()
		{
			return this.m_method.GetParameterTypes();
		}

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x06004A94 RID: 19092 RVA: 0x00103ED1 File Offset: 0x00102ED1
		public override MemberTypes MemberType
		{
			get
			{
				return this.m_method.MemberType;
			}
		}

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x06004A95 RID: 19093 RVA: 0x00103EDE File Offset: 0x00102EDE
		public override string Name
		{
			get
			{
				return this.m_method.Name;
			}
		}

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x06004A96 RID: 19094 RVA: 0x00103EEB File Offset: 0x00102EEB
		public override Type DeclaringType
		{
			get
			{
				return this.m_method.DeclaringType;
			}
		}

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06004A97 RID: 19095 RVA: 0x00103EF8 File Offset: 0x00102EF8
		public override Type ReflectedType
		{
			get
			{
				return this.m_method.ReflectedType;
			}
		}

		// Token: 0x06004A98 RID: 19096 RVA: 0x00103F05 File Offset: 0x00102F05
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_method.GetCustomAttributes(inherit);
		}

		// Token: 0x06004A99 RID: 19097 RVA: 0x00103F13 File Offset: 0x00102F13
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_method.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004A9A RID: 19098 RVA: 0x00103F22 File Offset: 0x00102F22
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_method.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06004A9B RID: 19099 RVA: 0x00103F31 File Offset: 0x00102F31
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06004A9C RID: 19100 RVA: 0x00103F38 File Offset: 0x00102F38
		public override Module Module
		{
			get
			{
				return this.m_method.Module;
			}
		}

		// Token: 0x06004A9D RID: 19101 RVA: 0x00103F45 File Offset: 0x00102F45
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06004A9E RID: 19102 RVA: 0x00103F4D File Offset: 0x00102F4D
		public override ParameterInfo[] GetParameters()
		{
			throw new NotSupportedException();
		}

		// Token: 0x06004A9F RID: 19103 RVA: 0x00103F54 File Offset: 0x00102F54
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_method.GetMethodImplementationFlags();
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06004AA0 RID: 19104 RVA: 0x00103F61 File Offset: 0x00102F61
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_DynamicModule"));
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06004AA1 RID: 19105 RVA: 0x00103F72 File Offset: 0x00102F72
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_method.Attributes;
			}
		}

		// Token: 0x06004AA2 RID: 19106 RVA: 0x00103F7F File Offset: 0x00102F7F
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x06004AA3 RID: 19107 RVA: 0x00103F86 File Offset: 0x00102F86
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_method.CallingConvention;
			}
		}

		// Token: 0x06004AA4 RID: 19108 RVA: 0x00103F93 File Offset: 0x00102F93
		public override Type[] GetGenericArguments()
		{
			return this.m_inst;
		}

		// Token: 0x06004AA5 RID: 19109 RVA: 0x00103F9B File Offset: 0x00102F9B
		public override MethodInfo GetGenericMethodDefinition()
		{
			return this.m_method;
		}

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x06004AA6 RID: 19110 RVA: 0x00103FA3 File Offset: 0x00102FA3
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x06004AA7 RID: 19111 RVA: 0x00103FA8 File Offset: 0x00102FA8
		public override bool ContainsGenericParameters
		{
			get
			{
				for (int i = 0; i < this.m_inst.Length; i++)
				{
					if (this.m_inst[i].ContainsGenericParameters)
					{
						return true;
					}
				}
				return this.DeclaringType != null && this.DeclaringType.ContainsGenericParameters;
			}
		}

		// Token: 0x06004AA8 RID: 19112 RVA: 0x00103FF1 File Offset: 0x00102FF1
		public override MethodInfo MakeGenericMethod(params Type[] arguments)
		{
			throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericMethodDefinition"));
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06004AA9 RID: 19113 RVA: 0x00104002 File Offset: 0x00103002
		public override bool IsGenericMethod
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06004AAA RID: 19114 RVA: 0x00104005 File Offset: 0x00103005
		internal override Type GetReturnType()
		{
			return this.m_method.GetReturnType();
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06004AAB RID: 19115 RVA: 0x00104012 File Offset: 0x00103012
		public override ParameterInfo ReturnParameter
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06004AAC RID: 19116 RVA: 0x00104019 File Offset: 0x00103019
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004AAD RID: 19117 RVA: 0x00104020 File Offset: 0x00103020
		public override MethodInfo GetBaseDefinition()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04002606 RID: 9734
		internal MethodInfo m_method;

		// Token: 0x04002607 RID: 9735
		private Type[] m_inst;
	}
}
