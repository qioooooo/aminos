using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000AA RID: 170
	internal sealed class JSFieldMethod : JSMethod
	{
		// Token: 0x060007D7 RID: 2007 RVA: 0x00035B40 File Offset: 0x00034B40
		internal JSFieldMethod(FieldInfo field, object obj)
			: base(obj)
		{
			this.field = field;
			this.func = null;
			if (!field.IsLiteral)
			{
				return;
			}
			object obj2 = ((field is JSVariableField) ? ((JSVariableField)field).value : field.GetValue(null));
			if (obj2 is FunctionObject)
			{
				this.func = (FunctionObject)obj2;
			}
		}

		// Token: 0x060007D8 RID: 2008 RVA: 0x00035B9C File Offset: 0x00034B9C
		internal override object Construct(object[] args)
		{
			return LateBinding.CallValue(this.field.GetValue(this.obj), args, true, false, ((ScriptObject)this.obj).engine, null, JSBinder.ob, null, null);
		}

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00035BDC File Offset: 0x00034BDC
		public override MethodAttributes Attributes
		{
			get
			{
				if (this.func != null)
				{
					return this.func.attributes;
				}
				if (this.field.IsPublic)
				{
					return MethodAttributes.Public;
				}
				if (this.field.IsFamily)
				{
					return MethodAttributes.Family;
				}
				if (this.field.IsAssembly)
				{
					return MethodAttributes.Assembly;
				}
				return MethodAttributes.Private;
			}
		}

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x060007DA RID: 2010 RVA: 0x00035C2B File Offset: 0x00034C2B
		public override Type DeclaringType
		{
			get
			{
				if (this.func != null)
				{
					return Convert.ToType(this.func.enclosing_scope);
				}
				return Typeob.Object;
			}
		}

		// Token: 0x060007DB RID: 2011 RVA: 0x00035C4B File Offset: 0x00034C4B
		internal ScriptObject EnclosingScope()
		{
			if (this.func != null)
			{
				return this.func.enclosing_scope;
			}
			return null;
		}

		// Token: 0x060007DC RID: 2012 RVA: 0x00035C64 File Offset: 0x00034C64
		public override object[] GetCustomAttributes(bool inherit)
		{
			if (this.func != null)
			{
				CustomAttributeList customAttributes = this.func.customAttributes;
				if (customAttributes != null)
				{
					return (object[])customAttributes.Evaluate(inherit);
				}
			}
			return new object[0];
		}

		// Token: 0x060007DD RID: 2013 RVA: 0x00035C9B File Offset: 0x00034C9B
		public override ParameterInfo[] GetParameters()
		{
			if (this.func != null)
			{
				return this.func.parameter_declarations;
			}
			return JSFieldMethod.EmptyParams;
		}

		// Token: 0x060007DE RID: 2014 RVA: 0x00035CB6 File Offset: 0x00034CB6
		internal override MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
		{
			return this.func.GetMethodInfo(compilerGlobals);
		}

		// Token: 0x060007DF RID: 2015 RVA: 0x00035CC4 File Offset: 0x00034CC4
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override object Invoke(object obj, object thisob, BindingFlags options, Binder binder, object[] parameters, CultureInfo culture)
		{
			bool flag = (options & BindingFlags.CreateInstance) != BindingFlags.Default;
			bool flag2 = (options & BindingFlags.GetProperty) != BindingFlags.Default && (options & BindingFlags.InvokeMethod) == BindingFlags.Default;
			object value = this.func;
			if (value == null)
			{
				value = this.field.GetValue(this.obj);
			}
			FunctionObject functionObject = value as FunctionObject;
			JSObject jsobject = obj as JSObject;
			if (jsobject != null && functionObject != null && functionObject.isMethod && (functionObject.attributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope && jsobject.GetParent() != functionObject.enclosing_scope && ((ClassScope)functionObject.enclosing_scope).HasInstance(jsobject))
			{
				return new LateBinding(functionObject.name)
				{
					obj = jsobject
				}.Call(parameters, flag, flag2, ((ScriptObject)this.obj).engine);
			}
			return LateBinding.CallValue(value, parameters, flag, flag2, ((ScriptObject)this.obj).engine, thisob, binder, culture, null);
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x00035DB0 File Offset: 0x00034DB0
		internal bool IsAccessibleFrom(ScriptObject scope)
		{
			return ((JSMemberField)this.field).IsAccessibleFrom(scope);
		}

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x060007E1 RID: 2017 RVA: 0x00035DC3 File Offset: 0x00034DC3
		public override string Name
		{
			get
			{
				return this.field.Name;
			}
		}

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x060007E2 RID: 2018 RVA: 0x00035DD0 File Offset: 0x00034DD0
		public override Type ReturnType
		{
			get
			{
				if (this.func != null)
				{
					return Convert.ToType(this.func.ReturnType(null));
				}
				return Typeob.Object;
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00035DF1 File Offset: 0x00034DF1
		internal IReflect ReturnIR()
		{
			if (this.func != null)
			{
				return this.func.ReturnType(null);
			}
			return Typeob.Object;
		}

		// Token: 0x04000431 RID: 1073
		internal FieldInfo field;

		// Token: 0x04000432 RID: 1074
		internal FunctionObject func;

		// Token: 0x04000433 RID: 1075
		private static readonly ParameterInfo[] EmptyParams = new ParameterInfo[0];
	}
}
