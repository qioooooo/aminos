using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x0200003A RID: 58
	public sealed class Closure : ScriptFunction
	{
		// Token: 0x06000284 RID: 644 RVA: 0x000150A7 File Offset: 0x000140A7
		public Closure(FunctionObject func)
			: this(func, null)
		{
			if (func.enclosing_scope is StackFrame)
			{
				this.enclosing_scope = func.enclosing_scope;
			}
		}

		// Token: 0x06000285 RID: 645 RVA: 0x000150CC File Offset: 0x000140CC
		internal Closure(FunctionObject func, object declaringObject)
			: base(func.GetParent(), func.name, func.GetNumberOfFormalParameters())
		{
			this.func = func;
			this.engine = func.engine;
			this.proto = new JSPrototypeObject(((ScriptObject)func.proto).GetParent(), this);
			this.enclosing_scope = this.engine.ScriptObjectStackTop();
			this.arguments = DBNull.Value;
			this.caller = DBNull.Value;
			this.declaringObject = declaringObject;
			this.noExpando = func.noExpando;
			if (func.isExpandoMethod)
			{
				StackFrame stackFrame = new StackFrame(new WithObject(this.enclosing_scope, declaringObject), new JSLocalField[0], new object[0], null);
				this.enclosing_scope = stackFrame;
				stackFrame.closureInstance = declaringObject;
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0001518F File Offset: 0x0001418F
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object Call(object[] args, object thisob)
		{
			return this.Call(args, thisob, JSBinder.ob, null);
		}

		// Token: 0x06000287 RID: 647 RVA: 0x000151A0 File Offset: 0x000141A0
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override object Call(object[] args, object thisob, Binder binder, CultureInfo culture)
		{
			if (this.func.isExpandoMethod)
			{
				((StackFrame)this.enclosing_scope).thisObject = thisob;
			}
			else if (this.declaringObject != null && !(this.declaringObject is ClassScope))
			{
				thisob = this.declaringObject;
			}
			if (thisob == null)
			{
				thisob = ((IActivationObject)this.engine.ScriptObjectStackTop()).GetDefaultThisObject();
			}
			if (this.enclosing_scope is ClassScope && this.declaringObject == null)
			{
				if (thisob is StackFrame)
				{
					thisob = ((StackFrame)thisob).closureInstance;
				}
				if (!this.func.isStatic && !((ClassScope)this.enclosing_scope).HasInstance(thisob))
				{
					throw new JScriptException(JSError.InvalidCall);
				}
			}
			return this.func.Call(args, thisob, this.enclosing_scope, this, binder, culture);
		}

		// Token: 0x06000288 RID: 648 RVA: 0x0001526C File Offset: 0x0001426C
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal Delegate ConvertToDelegate(Type delegateType)
		{
			return Delegate.CreateDelegate(delegateType, this.declaringObject, this.func.name);
		}

		// Token: 0x06000289 RID: 649 RVA: 0x00015285 File Offset: 0x00014285
		public override string ToString()
		{
			return this.func.ToString();
		}

		// Token: 0x0400017D RID: 381
		internal FunctionObject func;

		// Token: 0x0400017E RID: 382
		private ScriptObject enclosing_scope;

		// Token: 0x0400017F RID: 383
		private object declaringObject;

		// Token: 0x04000180 RID: 384
		public object arguments;

		// Token: 0x04000181 RID: 385
		public object caller;
	}
}
