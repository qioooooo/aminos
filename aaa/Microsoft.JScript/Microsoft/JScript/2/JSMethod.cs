using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000043 RID: 67
	[Guid("561AC104-8869-4368-902F-4E0D7DDEDDDD")]
	[ComVisible(true)]
	public abstract class JSMethod : MethodInfo
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x00015894 File Offset: 0x00014894
		internal JSMethod(object obj)
		{
			this.obj = obj;
		}

		// Token: 0x060002C2 RID: 706
		internal abstract object Construct(object[] args);

		// Token: 0x060002C3 RID: 707 RVA: 0x000158A3 File Offset: 0x000148A3
		public override MethodInfo GetBaseDefinition()
		{
			return this;
		}

		// Token: 0x060002C4 RID: 708 RVA: 0x000158A6 File Offset: 0x000148A6
		internal virtual string GetClassFullName()
		{
			if (this.obj is ClassScope)
			{
				return ((ClassScope)this.obj).GetFullName();
			}
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x000158CD File Offset: 0x000148CD
		public override object[] GetCustomAttributes(Type t, bool inherit)
		{
			return new object[0];
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x000158D5 File Offset: 0x000148D5
		public override object[] GetCustomAttributes(bool inherit)
		{
			return new object[0];
		}

		// Token: 0x060002C7 RID: 711 RVA: 0x000158DD File Offset: 0x000148DD
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return MethodImplAttributes.IL;
		}

		// Token: 0x060002C8 RID: 712
		internal abstract MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals);

		// Token: 0x060002C9 RID: 713 RVA: 0x000158E0 File Offset: 0x000148E0
		internal virtual PackageScope GetPackage()
		{
			if (this.obj is ClassScope)
			{
				return ((ClassScope)this.obj).GetPackage();
			}
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00015907 File Offset: 0x00014907
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object Invoke(object obj, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(obj, obj, options, binder, parameters, culture);
		}

		// Token: 0x060002CB RID: 715
		internal abstract object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture);

		// Token: 0x060002CC RID: 716 RVA: 0x00015917 File Offset: 0x00014917
		public sealed override bool IsDefined(Type type, bool inherit)
		{
			return false;
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060002CD RID: 717 RVA: 0x0001591A File Offset: 0x0001491A
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Method;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060002CE RID: 718 RVA: 0x0001591D File Offset: 0x0001491D
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				return this.GetMethodInfo(null).MethodHandle;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060002CF RID: 719 RVA: 0x0001592B File Offset: 0x0001492B
		public override Type ReflectedType
		{
			get
			{
				return this.DeclaringType;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060002D0 RID: 720 RVA: 0x00015933 File Offset: 0x00014933
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return null;
			}
		}

		// Token: 0x040001B9 RID: 441
		internal object obj;
	}
}
