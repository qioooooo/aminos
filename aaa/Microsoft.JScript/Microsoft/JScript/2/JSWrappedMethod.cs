using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000C7 RID: 199
	internal sealed class JSWrappedMethod : JSMethod, IWrappedMember
	{
		// Token: 0x0600090A RID: 2314 RVA: 0x00045AA4 File Offset: 0x00044AA4
		internal JSWrappedMethod(MethodInfo method, object obj)
			: base(obj)
		{
			this.obj = obj;
			if (method is JSMethodInfo)
			{
				method = ((JSMethodInfo)method).method;
			}
			this.method = method.GetBaseDefinition();
			this.pars = this.method.GetParameters();
			if (obj is JSObject && !Typeob.JSObject.IsAssignableFrom(method.DeclaringType))
			{
				if (obj is BooleanObject)
				{
					this.obj = ((BooleanObject)obj).value;
					return;
				}
				if (obj is NumberObject)
				{
					this.obj = ((NumberObject)obj).value;
					return;
				}
				if (obj is StringObject)
				{
					this.obj = ((StringObject)obj).value;
					return;
				}
				if (obj is ArrayWrapper)
				{
					this.obj = ((ArrayWrapper)obj).value;
				}
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x0600090B RID: 2315 RVA: 0x00045B77 File Offset: 0x00044B77
		public override MethodAttributes Attributes
		{
			get
			{
				return this.method.Attributes;
			}
		}

		// Token: 0x0600090C RID: 2316 RVA: 0x00045B84 File Offset: 0x00044B84
		private object[] CheckArguments(object[] args)
		{
			object[] array = args;
			if (args != null && args.Length < this.pars.Length)
			{
				array = new object[this.pars.Length];
				ArrayObject.Copy(args, array, args.Length);
				int i = args.Length;
				int num = this.pars.Length;
				while (i < num)
				{
					array[i] = Type.Missing;
					i++;
				}
			}
			return array;
		}

		// Token: 0x0600090D RID: 2317 RVA: 0x00045BDC File Offset: 0x00044BDC
		internal override object Construct(object[] args)
		{
			if (this.method is JSMethod)
			{
				return ((JSMethod)this.method).Construct(args);
			}
			if (this.method.GetParameters().Length == 0 && this.method.ReturnType == Typeob.Object)
			{
				object obj = this.method.Invoke(this.obj, BindingFlags.SuppressChangeType, null, null, null);
				if (obj is ScriptFunction)
				{
					return ((ScriptFunction)obj).Construct(args);
				}
			}
			throw new JScriptException(JSError.NoConstructor);
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x00045C62 File Offset: 0x00044C62
		public override Type DeclaringType
		{
			get
			{
				return this.method.DeclaringType;
			}
		}

		// Token: 0x0600090F RID: 2319 RVA: 0x00045C6F File Offset: 0x00044C6F
		internal override string GetClassFullName()
		{
			if (this.method is JSMethod)
			{
				return ((JSMethod)this.method).GetClassFullName();
			}
			return this.method.DeclaringType.FullName;
		}

		// Token: 0x06000910 RID: 2320 RVA: 0x00045C9F File Offset: 0x00044C9F
		internal override PackageScope GetPackage()
		{
			if (this.method is JSMethod)
			{
				return ((JSMethod)this.method).GetPackage();
			}
			return null;
		}

		// Token: 0x06000911 RID: 2321 RVA: 0x00045CC0 File Offset: 0x00044CC0
		public override ParameterInfo[] GetParameters()
		{
			return this.pars;
		}

		// Token: 0x06000912 RID: 2322 RVA: 0x00045CC8 File Offset: 0x00044CC8
		internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
		{
			if (this.method is JSMethod)
			{
				return ((JSMethod)this.method).GetMethodInfo(compilerGlobals);
			}
			return this.method;
		}

		// Token: 0x06000913 RID: 2323 RVA: 0x00045CEF File Offset: 0x00044CEF
		public object GetWrappedObject()
		{
			return this.obj;
		}

		// Token: 0x06000914 RID: 2324 RVA: 0x00045CF7 File Offset: 0x00044CF7
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object Invoke(object obj, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			parameters = this.CheckArguments(parameters);
			return this.Invoke(this.obj, this.obj, options, binder, parameters, culture);
		}

		// Token: 0x06000915 RID: 2325 RVA: 0x00045D1C File Offset: 0x00044D1C
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			parameters = this.CheckArguments(parameters);
			if (this.obj != null && !(this.obj is Type))
			{
				obj = this.obj;
			}
			if (this.method is JSMethod)
			{
				return ((JSMethod)this.method).Invoke(obj, thisob, options, binder, parameters, culture);
			}
			return this.method.Invoke(obj, options, binder, parameters, culture);
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x00045D8A File Offset: 0x00044D8A
		public override string Name
		{
			get
			{
				return this.method.Name;
			}
		}

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x00045D97 File Offset: 0x00044D97
		public override Type ReturnType
		{
			get
			{
				return this.method.ReturnType;
			}
		}

		// Token: 0x06000918 RID: 2328 RVA: 0x00045DA4 File Offset: 0x00044DA4
		public override string ToString()
		{
			return this.method.ToString();
		}

		// Token: 0x0400055A RID: 1370
		internal MethodInfo method;

		// Token: 0x0400055B RID: 1371
		private ParameterInfo[] pars;
	}
}
