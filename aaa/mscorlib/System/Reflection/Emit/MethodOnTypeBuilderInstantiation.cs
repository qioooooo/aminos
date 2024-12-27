using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x0200083B RID: 2107
	internal sealed class MethodOnTypeBuilderInstantiation : MethodInfo
	{
		// Token: 0x06004DA5 RID: 19877 RVA: 0x0010F160 File Offset: 0x0010E160
		internal static MethodInfo GetMethod(MethodInfo method, TypeBuilderInstantiation type)
		{
			return new MethodOnTypeBuilderInstantiation(method, type);
		}

		// Token: 0x06004DA6 RID: 19878 RVA: 0x0010F169 File Offset: 0x0010E169
		internal MethodOnTypeBuilderInstantiation(MethodInfo method, TypeBuilderInstantiation type)
		{
			this.m_method = method;
			this.m_type = type;
		}

		// Token: 0x06004DA7 RID: 19879 RVA: 0x0010F17F File Offset: 0x0010E17F
		internal override Type[] GetParameterTypes()
		{
			return this.m_method.GetParameterTypes();
		}

		// Token: 0x17000D6D RID: 3437
		// (get) Token: 0x06004DA8 RID: 19880 RVA: 0x0010F18C File Offset: 0x0010E18C
		public override MemberTypes MemberType
		{
			get
			{
				return this.m_method.MemberType;
			}
		}

		// Token: 0x17000D6E RID: 3438
		// (get) Token: 0x06004DA9 RID: 19881 RVA: 0x0010F199 File Offset: 0x0010E199
		public override string Name
		{
			get
			{
				return this.m_method.Name;
			}
		}

		// Token: 0x17000D6F RID: 3439
		// (get) Token: 0x06004DAA RID: 19882 RVA: 0x0010F1A6 File Offset: 0x0010E1A6
		public override Type DeclaringType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000D70 RID: 3440
		// (get) Token: 0x06004DAB RID: 19883 RVA: 0x0010F1AE File Offset: 0x0010E1AE
		public override Type ReflectedType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x06004DAC RID: 19884 RVA: 0x0010F1B6 File Offset: 0x0010E1B6
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_method.GetCustomAttributes(inherit);
		}

		// Token: 0x06004DAD RID: 19885 RVA: 0x0010F1C4 File Offset: 0x0010E1C4
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_method.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004DAE RID: 19886 RVA: 0x0010F1D3 File Offset: 0x0010E1D3
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_method.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000D71 RID: 3441
		// (get) Token: 0x06004DAF RID: 19887 RVA: 0x0010F1E2 File Offset: 0x0010E1E2
		internal override int MetadataTokenInternal
		{
			get
			{
				return this.m_method.MetadataTokenInternal;
			}
		}

		// Token: 0x17000D72 RID: 3442
		// (get) Token: 0x06004DB0 RID: 19888 RVA: 0x0010F1EF File Offset: 0x0010E1EF
		public override Module Module
		{
			get
			{
				return this.m_method.Module;
			}
		}

		// Token: 0x06004DB1 RID: 19889 RVA: 0x0010F1FC File Offset: 0x0010E1FC
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06004DB2 RID: 19890 RVA: 0x0010F204 File Offset: 0x0010E204
		public override ParameterInfo[] GetParameters()
		{
			return this.m_method.GetParameters();
		}

		// Token: 0x06004DB3 RID: 19891 RVA: 0x0010F211 File Offset: 0x0010E211
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_method.GetMethodImplementationFlags();
		}

		// Token: 0x17000D73 RID: 3443
		// (get) Token: 0x06004DB4 RID: 19892 RVA: 0x0010F21E File Offset: 0x0010E21E
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.m_method.MethodHandle;
			}
		}

		// Token: 0x17000D74 RID: 3444
		// (get) Token: 0x06004DB5 RID: 19893 RVA: 0x0010F22B File Offset: 0x0010E22B
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_method.Attributes;
			}
		}

		// Token: 0x06004DB6 RID: 19894 RVA: 0x0010F238 File Offset: 0x0010E238
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D75 RID: 3445
		// (get) Token: 0x06004DB7 RID: 19895 RVA: 0x0010F23F File Offset: 0x0010E23F
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_method.CallingConvention;
			}
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x0010F24C File Offset: 0x0010E24C
		public override Type[] GetGenericArguments()
		{
			return this.m_method.GetGenericArguments();
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x0010F259 File Offset: 0x0010E259
		public override MethodInfo GetGenericMethodDefinition()
		{
			return this.m_method;
		}

		// Token: 0x17000D76 RID: 3446
		// (get) Token: 0x06004DBA RID: 19898 RVA: 0x0010F261 File Offset: 0x0010E261
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return this.m_method.IsGenericMethodDefinition;
			}
		}

		// Token: 0x17000D77 RID: 3447
		// (get) Token: 0x06004DBB RID: 19899 RVA: 0x0010F26E File Offset: 0x0010E26E
		public override bool ContainsGenericParameters
		{
			get
			{
				return this.m_method.ContainsGenericParameters;
			}
		}

		// Token: 0x06004DBC RID: 19900 RVA: 0x0010F27B File Offset: 0x0010E27B
		public override MethodInfo MakeGenericMethod(params Type[] typeArgs)
		{
			if (!this.IsGenericMethodDefinition)
			{
				throw new InvalidOperationException(Environment.GetResourceString("Arg_NotGenericMethodDefinition"));
			}
			return MethodBuilderInstantiation.MakeGenericMethod(this, typeArgs);
		}

		// Token: 0x17000D78 RID: 3448
		// (get) Token: 0x06004DBD RID: 19901 RVA: 0x0010F29C File Offset: 0x0010E29C
		public override bool IsGenericMethod
		{
			get
			{
				return this.m_method.IsGenericMethod;
			}
		}

		// Token: 0x06004DBE RID: 19902 RVA: 0x0010F2A9 File Offset: 0x0010E2A9
		internal override Type GetReturnType()
		{
			return this.m_method.GetReturnType();
		}

		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06004DBF RID: 19903 RVA: 0x0010F2B6 File Offset: 0x0010E2B6
		public override ParameterInfo ReturnParameter
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D7A RID: 3450
		// (get) Token: 0x06004DC0 RID: 19904 RVA: 0x0010F2BD File Offset: 0x0010E2BD
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06004DC1 RID: 19905 RVA: 0x0010F2C4 File Offset: 0x0010E2C4
		public override MethodInfo GetBaseDefinition()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04002800 RID: 10240
		internal MethodInfo m_method;

		// Token: 0x04002801 RID: 10241
		private TypeBuilderInstantiation m_type;
	}
}
