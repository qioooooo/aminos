using System;
using System.Globalization;

namespace System.Reflection.Emit
{
	// Token: 0x0200083C RID: 2108
	internal sealed class ConstructorOnTypeBuilderInstantiation : ConstructorInfo
	{
		// Token: 0x06004DC2 RID: 19906 RVA: 0x0010F2CB File Offset: 0x0010E2CB
		internal static ConstructorInfo GetConstructor(ConstructorInfo Constructor, TypeBuilderInstantiation type)
		{
			return new ConstructorOnTypeBuilderInstantiation(Constructor, type);
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x0010F2D4 File Offset: 0x0010E2D4
		internal ConstructorOnTypeBuilderInstantiation(ConstructorInfo constructor, TypeBuilderInstantiation type)
		{
			this.m_ctor = constructor;
			this.m_type = type;
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x0010F2EA File Offset: 0x0010E2EA
		internal override Type[] GetParameterTypes()
		{
			return this.m_ctor.GetParameterTypes();
		}

		// Token: 0x17000D7B RID: 3451
		// (get) Token: 0x06004DC5 RID: 19909 RVA: 0x0010F2F7 File Offset: 0x0010E2F7
		public override MemberTypes MemberType
		{
			get
			{
				return this.m_ctor.MemberType;
			}
		}

		// Token: 0x17000D7C RID: 3452
		// (get) Token: 0x06004DC6 RID: 19910 RVA: 0x0010F304 File Offset: 0x0010E304
		public override string Name
		{
			get
			{
				return this.m_ctor.Name;
			}
		}

		// Token: 0x17000D7D RID: 3453
		// (get) Token: 0x06004DC7 RID: 19911 RVA: 0x0010F311 File Offset: 0x0010E311
		public override Type DeclaringType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x17000D7E RID: 3454
		// (get) Token: 0x06004DC8 RID: 19912 RVA: 0x0010F319 File Offset: 0x0010E319
		public override Type ReflectedType
		{
			get
			{
				return this.m_type;
			}
		}

		// Token: 0x06004DC9 RID: 19913 RVA: 0x0010F321 File Offset: 0x0010E321
		public override object[] GetCustomAttributes(bool inherit)
		{
			return this.m_ctor.GetCustomAttributes(inherit);
		}

		// Token: 0x06004DCA RID: 19914 RVA: 0x0010F32F File Offset: 0x0010E32F
		public override object[] GetCustomAttributes(Type attributeType, bool inherit)
		{
			return this.m_ctor.GetCustomAttributes(attributeType, inherit);
		}

		// Token: 0x06004DCB RID: 19915 RVA: 0x0010F33E File Offset: 0x0010E33E
		public override bool IsDefined(Type attributeType, bool inherit)
		{
			return this.m_ctor.IsDefined(attributeType, inherit);
		}

		// Token: 0x17000D7F RID: 3455
		// (get) Token: 0x06004DCC RID: 19916 RVA: 0x0010F34D File Offset: 0x0010E34D
		internal override int MetadataTokenInternal
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x17000D80 RID: 3456
		// (get) Token: 0x06004DCD RID: 19917 RVA: 0x0010F354 File Offset: 0x0010E354
		public override Module Module
		{
			get
			{
				return this.m_ctor.Module;
			}
		}

		// Token: 0x06004DCE RID: 19918 RVA: 0x0010F361 File Offset: 0x0010E361
		public new Type GetType()
		{
			return base.GetType();
		}

		// Token: 0x06004DCF RID: 19919 RVA: 0x0010F369 File Offset: 0x0010E369
		public override ParameterInfo[] GetParameters()
		{
			return this.m_ctor.GetParameters();
		}

		// Token: 0x06004DD0 RID: 19920 RVA: 0x0010F376 File Offset: 0x0010E376
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return this.m_ctor.GetMethodImplementationFlags();
		}

		// Token: 0x17000D81 RID: 3457
		// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x0010F383 File Offset: 0x0010E383
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.m_ctor.MethodHandle;
			}
		}

		// Token: 0x17000D82 RID: 3458
		// (get) Token: 0x06004DD2 RID: 19922 RVA: 0x0010F390 File Offset: 0x0010E390
		public override MethodAttributes Attributes
		{
			get
			{
				return this.m_ctor.Attributes;
			}
		}

		// Token: 0x06004DD3 RID: 19923 RVA: 0x0010F39D File Offset: 0x0010E39D
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		// Token: 0x17000D83 RID: 3459
		// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x0010F3A4 File Offset: 0x0010E3A4
		public override CallingConventions CallingConvention
		{
			get
			{
				return this.m_ctor.CallingConvention;
			}
		}

		// Token: 0x06004DD5 RID: 19925 RVA: 0x0010F3B1 File Offset: 0x0010E3B1
		public override Type[] GetGenericArguments()
		{
			return this.m_ctor.GetGenericArguments();
		}

		// Token: 0x17000D84 RID: 3460
		// (get) Token: 0x06004DD6 RID: 19926 RVA: 0x0010F3BE File Offset: 0x0010E3BE
		public override bool IsGenericMethodDefinition
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D85 RID: 3461
		// (get) Token: 0x06004DD7 RID: 19927 RVA: 0x0010F3C1 File Offset: 0x0010E3C1
		public override bool ContainsGenericParameters
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000D86 RID: 3462
		// (get) Token: 0x06004DD8 RID: 19928 RVA: 0x0010F3C4 File Offset: 0x0010E3C4
		public override bool IsGenericMethod
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06004DD9 RID: 19929 RVA: 0x0010F3C7 File Offset: 0x0010E3C7
		public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			throw new InvalidOperationException();
		}

		// Token: 0x04002802 RID: 10242
		internal ConstructorInfo m_ctor;

		// Token: 0x04002803 RID: 10243
		private TypeBuilderInstantiation m_type;
	}
}
