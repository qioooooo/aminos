using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x02000009 RID: 9
	public abstract class ScriptFunction : JSObject
	{
		// Token: 0x0600005E RID: 94 RVA: 0x00004155 File Offset: 0x00003155
		internal ScriptFunction(ScriptObject parent)
			: base(parent)
		{
			this.ilength = 0;
			this.name = "Function.prototype";
			this.proto = Missing.Value;
		}

		// Token: 0x0600005F RID: 95 RVA: 0x0000417B File Offset: 0x0000317B
		protected ScriptFunction(ScriptObject parent, string name)
			: base(parent, typeof(ScriptFunction))
		{
			this.ilength = 0;
			this.name = name;
			this.proto = new JSPrototypeObject(parent.GetParent(), this);
		}

		// Token: 0x06000060 RID: 96 RVA: 0x000041AE File Offset: 0x000031AE
		internal ScriptFunction(ScriptObject parent, string name, int length)
			: base(parent)
		{
			this.ilength = length;
			this.name = name;
			this.proto = new JSPrototypeObject(parent.GetParent(), this);
		}

		// Token: 0x06000061 RID: 97
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal abstract object Call(object[] args, object thisob);

		// Token: 0x06000062 RID: 98 RVA: 0x000041D7 File Offset: 0x000031D7
		internal virtual object Call(object[] args, object thisob, Binder binder, CultureInfo culture)
		{
			return this.Call(args, thisob);
		}

		// Token: 0x06000063 RID: 99 RVA: 0x000041E1 File Offset: 0x000031E1
		internal virtual object Call(object[] args, object thisob, ScriptObject enclosing_scope, Closure calleeClosure, Binder binder, CultureInfo culture)
		{
			return this.Call(args, thisob);
		}

		// Token: 0x06000064 RID: 100 RVA: 0x000041EC File Offset: 0x000031EC
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal virtual object Construct(object[] args)
		{
			JSObject jsobject = new JSObject(null, false);
			jsobject.SetParent(this.GetPrototypeForConstructedObject());
			object obj = this.Call(args, jsobject);
			if (obj is ScriptObject || (this is BuiltinFunction && ((BuiltinFunction)this).method.Name.Equals("CreateInstance")))
			{
				return obj;
			}
			return jsobject;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00004245 File Offset: 0x00003245
		[DebuggerStepThrough]
		[JSFunction(JSFunctionAttributeEnum.HasVarArgs)]
		[DebuggerHidden]
		public object CreateInstance(params object[] args)
		{
			return this.Construct(args);
		}

		// Token: 0x06000066 RID: 102 RVA: 0x0000424E File Offset: 0x0000324E
		internal override string GetClassName()
		{
			return "Function";
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00004255 File Offset: 0x00003255
		internal virtual int GetNumberOfFormalParameters()
		{
			return this.ilength;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00004260 File Offset: 0x00003260
		protected ScriptObject GetPrototypeForConstructedObject()
		{
			object obj = this.proto;
			if (obj is JSObject)
			{
				return (JSObject)obj;
			}
			if (obj is ClassScope)
			{
				return (ClassScope)obj;
			}
			return (ObjectPrototype)base.GetParent().GetParent();
		}

		// Token: 0x06000069 RID: 105 RVA: 0x000042A4 File Offset: 0x000032A4
		internal virtual bool HasInstance(object ob)
		{
			if (!(ob is JSObject))
			{
				return false;
			}
			object obj = this.proto;
			if (!(obj is ScriptObject))
			{
				throw new JScriptException(JSError.InvalidPrototype);
			}
			ScriptObject scriptObject = ((JSObject)ob).GetParent();
			ScriptObject scriptObject2 = (ScriptObject)obj;
			while (scriptObject != null)
			{
				if (scriptObject == scriptObject2)
				{
					return true;
				}
				if (scriptObject is WithObject)
				{
					object contained_object = ((WithObject)scriptObject).contained_object;
					if (contained_object == scriptObject2 && contained_object is ClassScope)
					{
						return true;
					}
				}
				scriptObject = scriptObject.GetParent();
			}
			return false;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x0000431D File Offset: 0x0000331D
		[DebuggerStepThrough]
		[JSFunction(JSFunctionAttributeEnum.HasThisObject | JSFunctionAttributeEnum.HasVarArgs)]
		[DebuggerHidden]
		public object Invoke(object thisob, params object[] args)
		{
			return this.Call(args, thisob);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00004328 File Offset: 0x00003328
		[DebuggerHidden]
		[DebuggerStepThrough]
		public override object InvokeMember(string name, BindingFlags invokeAttr, Binder binder, object target, object[] args, ParameterModifier[] modifiers, CultureInfo culture, string[] namedParameters)
		{
			if (target != this)
			{
				throw new TargetException();
			}
			string text = "this";
			if (name.Equals("[DISPID=0]"))
			{
				name = string.Empty;
				if (namedParameters != null)
				{
					text = "[DISPID=-613]";
				}
			}
			if (name == null || name == string.Empty)
			{
				if ((invokeAttr & BindingFlags.CreateInstance) != BindingFlags.Default)
				{
					if ((invokeAttr & (BindingFlags.InvokeMethod | BindingFlags.GetField | BindingFlags.SetField | BindingFlags.GetProperty | BindingFlags.SetProperty | BindingFlags.PutDispProperty)) != BindingFlags.Default)
					{
						throw new ArgumentException();
					}
					return this.Construct(args);
				}
				else if ((invokeAttr & BindingFlags.InvokeMethod) != BindingFlags.Default)
				{
					object obj = null;
					if (namedParameters != null)
					{
						int num = Array.IndexOf<string>(namedParameters, text);
						if (num == 0)
						{
							obj = args[0];
							int num2 = args.Length - 1;
							object[] array = new object[num2];
							ArrayObject.Copy(args, 1, array, 0, num2);
							args = array;
						}
						if (num != 0 || namedParameters.Length != 1)
						{
							throw new ArgumentException();
						}
					}
					if (args.Length > 0 || (invokeAttr & (BindingFlags.GetField | BindingFlags.GetProperty)) == BindingFlags.Default)
					{
						return this.Call(args, obj, binder, culture);
					}
				}
			}
			return base.InvokeMember(name, invokeAttr, binder, target, args, modifiers, culture, namedParameters);
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600006C RID: 108 RVA: 0x00004417 File Offset: 0x00003417
		// (set) Token: 0x0600006D RID: 109 RVA: 0x0000441F File Offset: 0x0000341F
		public virtual int length
		{
			get
			{
				return this.ilength;
			}
			set
			{
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00004421 File Offset: 0x00003421
		public override string ToString()
		{
			return "function " + this.name + "() {\n    [native code]\n}";
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600006F RID: 111 RVA: 0x00004438 File Offset: 0x00003438
		// (set) Token: 0x06000070 RID: 112 RVA: 0x00004440 File Offset: 0x00003440
		public object prototype
		{
			get
			{
				return this.proto;
			}
			set
			{
				if (!this.noExpando)
				{
					this.proto = value;
				}
			}
		}

		// Token: 0x0400001E RID: 30
		protected int ilength;

		// Token: 0x0400001F RID: 31
		internal string name;

		// Token: 0x04000020 RID: 32
		internal object proto;
	}
}
