using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000A2 RID: 162
	public sealed class JSConstructor : ConstructorInfo
	{
		// Token: 0x06000782 RID: 1922 RVA: 0x000344FE File Offset: 0x000334FE
		internal JSConstructor(FunctionObject cons)
		{
			this.cons = cons;
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06000783 RID: 1923 RVA: 0x0003450D File Offset: 0x0003350D
		public override MethodAttributes Attributes
		{
			get
			{
				return this.cons.attributes;
			}
		}

		// Token: 0x06000784 RID: 1924 RVA: 0x0003451C File Offset: 0x0003351C
		internal object Construct(object thisob, object[] args)
		{
			return LateBinding.CallValue(this.cons, args, true, false, this.cons.engine, thisob, JSBinder.ob, null, null);
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06000785 RID: 1925 RVA: 0x0003454A File Offset: 0x0003354A
		public override string Name
		{
			get
			{
				return this.cons.name;
			}
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x00034557 File Offset: 0x00033557
		public override Type DeclaringType
		{
			get
			{
				return Convert.ToType(this.cons.enclosing_scope);
			}
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x00034569 File Offset: 0x00033569
		internal string GetClassFullName()
		{
			return ((ClassScope)this.cons.enclosing_scope).GetFullName();
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00034580 File Offset: 0x00033580
		internal ClassScope GetClassScope()
		{
			return (ClassScope)this.cons.enclosing_scope;
		}

		// Token: 0x06000789 RID: 1929 RVA: 0x00034592 File Offset: 0x00033592
		internal ConstructorInfo GetConstructorInfo(CompilerGlobals compilerGlobals)
		{
			return this.cons.GetConstructorInfo(compilerGlobals);
		}

		// Token: 0x0600078A RID: 1930 RVA: 0x000345A0 File Offset: 0x000335A0
		public override object[] GetCustomAttributes(Type t, bool inherit)
		{
			return new object[0];
		}

		// Token: 0x0600078B RID: 1931 RVA: 0x000345A8 File Offset: 0x000335A8
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.cons != null)
			{
				CustomAttributeList customAttributes = this.cons.customAttributes;
				if (customAttributes != null)
				{
					return (object[])customAttributes.Evaluate(false);
				}
			}
			return new object[0];
		}

		// Token: 0x0600078C RID: 1932 RVA: 0x000345DF File Offset: 0x000335DF
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return MethodImplAttributes.IL;
		}

		// Token: 0x0600078D RID: 1933 RVA: 0x000345E2 File Offset: 0x000335E2
		internal PackageScope GetPackage()
		{
			return ((ClassScope)this.cons.enclosing_scope).GetPackage();
		}

		// Token: 0x0600078E RID: 1934 RVA: 0x000345F9 File Offset: 0x000335F9
		public override ParameterInfo[] GetParameters()
		{
			return this.cons.parameter_declarations;
		}

		// Token: 0x0600078F RID: 1935 RVA: 0x00034608 File Offset: 0x00033608
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object Invoke(BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			return LateBinding.CallValue(this.cons, parameters, true, false, this.cons.engine, null, binder, culture, null);
		}

		// Token: 0x06000790 RID: 1936 RVA: 0x00034633 File Offset: 0x00033633
		[DebuggerStepThrough]
		[DebuggerHidden]
		public override object Invoke(object obj, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.cons.Call(parameters, obj, binder, culture);
		}

		// Token: 0x06000791 RID: 1937 RVA: 0x00034648 File Offset: 0x00033648
		internal bool IsAccessibleFrom(ScriptObject scope)
		{
			while (scope != null && !(scope is ClassScope))
			{
				scope = scope.GetParent();
			}
			ClassScope classScope = (ClassScope)this.cons.enclosing_scope;
			if (base.IsPrivate)
			{
				return scope != null && (scope == classScope || ((ClassScope)scope).IsNestedIn(classScope, false));
			}
			if (base.IsFamily)
			{
				return scope != null && (((ClassScope)scope).IsSameOrDerivedFrom(classScope) || ((ClassScope)scope).IsNestedIn(classScope, false));
			}
			if (base.IsFamilyOrAssembly && scope != null && (((ClassScope)scope).IsSameOrDerivedFrom(classScope) || ((ClassScope)scope).IsNestedIn(classScope, false)))
			{
				return true;
			}
			if (scope == null)
			{
				return classScope.GetPackage() == null;
			}
			return classScope.GetPackage() == ((ClassScope)scope).GetPackage();
		}

		// Token: 0x06000792 RID: 1938 RVA: 0x00034711 File Offset: 0x00033711
		public override bool IsDefined(Type type, bool inherit)
		{
			return false;
		}

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x06000793 RID: 1939 RVA: 0x00034714 File Offset: 0x00033714
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Constructor;
			}
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x00034717 File Offset: 0x00033717
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.GetConstructorInfo(null).MethodHandle;
			}
		}

		// Token: 0x06000795 RID: 1941 RVA: 0x00034728 File Offset: 0x00033728
		internal Type OuterClassType()
		{
			FieldInfo outerClassField = ((ClassScope)this.cons.enclosing_scope).outerClassField;
			if (outerClassField != null)
			{
				return outerClassField.FieldType;
			}
			return null;
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x00034756 File Offset: 0x00033756
		public override Type ReflectedType
		{
			get
			{
				return this.DeclaringType;
			}
		}

		// Token: 0x0400032C RID: 812
		internal FunctionObject cons;
	}
}
