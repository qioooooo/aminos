using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000118 RID: 280
	public sealed class StackFrame : ScriptObject, IActivationObject
	{
		// Token: 0x06000B8B RID: 2955 RVA: 0x00057B04 File Offset: 0x00056B04
		internal StackFrame(ScriptObject parent, JSLocalField[] fields, object[] local_vars, object thisObject)
			: base(parent)
		{
			this.caller_arguments = null;
			this.fields = fields;
			this.localVars = local_vars;
			this.nestedFunctionScope = null;
			this.thisObject = thisObject;
			if (parent is StackFrame)
			{
				this.closureInstance = ((StackFrame)parent).closureInstance;
				return;
			}
			if (parent is JSObject)
			{
				this.closureInstance = parent;
				return;
			}
			this.closureInstance = null;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00057B6D File Offset: 0x00056B6D
		internal JSVariableField AddNewField(string name, object value, FieldAttributes attributeFlags)
		{
			this.AllocateFunctionScope();
			return this.nestedFunctionScope.AddNewField(name, value, attributeFlags);
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00057B84 File Offset: 0x00056B84
		private void AllocateFunctionScope()
		{
			if (this.nestedFunctionScope != null)
			{
				return;
			}
			this.nestedFunctionScope = new FunctionScope(this.parent);
			if (this.fields != null)
			{
				int i = 0;
				int num = this.fields.Length;
				while (i < num)
				{
					this.nestedFunctionScope.AddOuterScopeField(this.fields[i].Name, this.fields[i]);
					i++;
				}
			}
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x00057BE8 File Offset: 0x00056BE8
		public object GetDefaultThisObject()
		{
			ScriptObject parent = base.GetParent();
			IActivationObject activationObject = parent as IActivationObject;
			if (activationObject != null)
			{
				return activationObject.GetDefaultThisObject();
			}
			return parent;
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x00057C0E File Offset: 0x00056C0E
		public FieldInfo GetField(string name, int lexLevel)
		{
			return null;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x00057C11 File Offset: 0x00056C11
		public GlobalScope GetGlobalScope()
		{
			return ((IActivationObject)base.GetParent()).GetGlobalScope();
		}

		// Token: 0x06000B91 RID: 2961 RVA: 0x00057C23 File Offset: 0x00056C23
		FieldInfo IActivationObject.GetLocalField(string name)
		{
			this.AllocateFunctionScope();
			return this.nestedFunctionScope.GetLocalField(name);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00057C37 File Offset: 0x00056C37
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			this.AllocateFunctionScope();
			return this.nestedFunctionScope.GetMember(name, bindingAttr);
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00057C4C File Offset: 0x00056C4C
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			this.AllocateFunctionScope();
			return this.nestedFunctionScope.GetMembers(bindingAttr);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00057C60 File Offset: 0x00056C60
		internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00057C69 File Offset: 0x00056C69
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object GetMemberValue(string name)
		{
			this.AllocateFunctionScope();
			return this.nestedFunctionScope.GetMemberValue(name);
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00057C7D File Offset: 0x00056C7D
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object GetMemberValue(string name, int lexlevel)
		{
			if (lexlevel <= 0)
			{
				return Missing.Value;
			}
			if (this.nestedFunctionScope != null)
			{
				return this.nestedFunctionScope.GetMemberValue(name, lexlevel);
			}
			return ((IActivationObject)this.parent).GetMemberValue(name, lexlevel - 1);
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00057CB3 File Offset: 0x00056CB3
		public static void PushStackFrameForStaticMethod(RuntimeTypeHandle thisclass, JSLocalField[] fields, VsaEngine engine)
		{
			StackFrame.PushStackFrameForMethod(Type.GetTypeFromHandle(thisclass), fields, engine);
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00057CC4 File Offset: 0x00056CC4
		public static void PushStackFrameForMethod(object thisob, JSLocalField[] fields, VsaEngine engine)
		{
			Globals globals = engine.Globals;
			IActivationObject activationObject = (IActivationObject)globals.ScopeStack.Peek();
			string @namespace = thisob.GetType().Namespace;
			WithObject withObject;
			if (@namespace != null && @namespace.Length > 0)
			{
				withObject = new WithObject(new WithObject(activationObject.GetGlobalScope(), new WrappedNamespace(@namespace, engine))
				{
					isKnownAtCompileTime = true
				}, thisob);
			}
			else
			{
				withObject = new WithObject(activationObject.GetGlobalScope(), thisob);
			}
			withObject.isKnownAtCompileTime = true;
			StackFrame stackFrame = new StackFrame(withObject, fields, new object[fields.Length], thisob);
			stackFrame.closureInstance = thisob;
			globals.ScopeStack.GuardedPush(stackFrame);
		}

		// Token: 0x06000B99 RID: 2969 RVA: 0x00057D62 File Offset: 0x00056D62
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override void SetMemberValue(string name, object value)
		{
			this.AllocateFunctionScope();
			this.nestedFunctionScope.SetMemberValue(name, value, this);
		}

		// Token: 0x040006F2 RID: 1778
		internal ArgumentsObject caller_arguments;

		// Token: 0x040006F3 RID: 1779
		private JSLocalField[] fields;

		// Token: 0x040006F4 RID: 1780
		public object[] localVars;

		// Token: 0x040006F5 RID: 1781
		private FunctionScope nestedFunctionScope;

		// Token: 0x040006F6 RID: 1782
		internal object thisObject;

		// Token: 0x040006F7 RID: 1783
		public object closureInstance;
	}
}
