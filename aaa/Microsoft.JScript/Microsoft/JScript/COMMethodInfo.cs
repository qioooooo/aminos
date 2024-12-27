using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000044 RID: 68
	[ComVisible(true)]
	[Guid("C7B9C313-2FD4-4384-8571-7ABC08BD17E5")]
	public class COMMethodInfo : JSMethod, MemberInfoInitializer
	{
		// Token: 0x060002D1 RID: 721 RVA: 0x00015936 File Offset: 0x00014936
		public COMMethodInfo()
			: base(null)
		{
			this._comObject = null;
			this._name = null;
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060002D2 RID: 722 RVA: 0x0001594D File Offset: 0x0001494D
		public override MethodAttributes Attributes
		{
			get
			{
				return MethodAttributes.Public;
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00015950 File Offset: 0x00014950
		public virtual void Initialize(string name, COMMemberInfo dispatch)
		{
			this._name = name;
			this._comObject = dispatch;
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00015960 File Offset: 0x00014960
		public COMMemberInfo GetCOMMemberInfo()
		{
			return this._comObject;
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x00015968 File Offset: 0x00014968
		public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this._comObject.Call(invokeAttr, binder, (parameters != null) ? parameters : new object[0], culture);
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060002D6 RID: 726 RVA: 0x00015987 File Offset: 0x00014987
		public override Type DeclaringType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x0001598A File Offset: 0x0001498A
		public override MethodInfo GetBaseDefinition()
		{
			return this;
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0001598D File Offset: 0x0001498D
		public override MethodImplAttributes GetMethodImplementationFlags()
		{
			return MethodImplAttributes.IL;
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00015990 File Offset: 0x00014990
		public override ParameterInfo[] GetParameters()
		{
			return COMMethodInfo.EmptyParams;
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060002DA RID: 730 RVA: 0x00015997 File Offset: 0x00014997
		public override MemberTypes MemberType
		{
			get
			{
				return MemberTypes.Method;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0001599A File Offset: 0x0001499A
		public override RuntimeMethodHandle MethodHandle
		{
			get
			{
				throw new JScriptException(JSError.InternalError);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060002DC RID: 732 RVA: 0x000159A3 File Offset: 0x000149A3
		public override string Name
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060002DD RID: 733 RVA: 0x000159AB File Offset: 0x000149AB
		public override Type ReflectedType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060002DE RID: 734 RVA: 0x000159AE File Offset: 0x000149AE
		public override Type ReturnType
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x060002DF RID: 735 RVA: 0x000159B1 File Offset: 0x000149B1
		public override ICustomAttributeProvider ReturnTypeCustomAttributes
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000159B4 File Offset: 0x000149B4
		internal override object Construct(object[] args)
		{
			return this._comObject.Call(BindingFlags.CreateInstance, null, (args != null) ? args : new object[0], null);
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x000159D4 File Offset: 0x000149D4
		internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
		{
			return null;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x000159D7 File Offset: 0x000149D7
		internal override object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			return this.Invoke(thisob, options, binder, parameters, culture);
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x000159E7 File Offset: 0x000149E7
		public override string ToString()
		{
			return "";
		}

		// Token: 0x040001BA RID: 442
		protected static readonly ParameterInfo[] EmptyParams = new ParameterInfo[0];

		// Token: 0x040001BB RID: 443
		protected COMMemberInfo _comObject;

		// Token: 0x040001BC RID: 444
		protected string _name;
	}
}
