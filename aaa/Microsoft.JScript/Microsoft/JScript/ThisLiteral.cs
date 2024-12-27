using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x0200011F RID: 287
	internal sealed class ThisLiteral : AST
	{
		// Token: 0x06000BBC RID: 3004 RVA: 0x000593A0 File Offset: 0x000583A0
		internal ThisLiteral(Context context, bool isSuper)
			: base(context)
		{
			this.isSuper = isSuper;
			this.method = null;
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x000593B7 File Offset: 0x000583B7
		internal override void CheckIfOKToUseInSuperConstructorCall()
		{
			this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x000593CC File Offset: 0x000583CC
		internal override object Evaluate()
		{
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject || scriptObject is BlockScope)
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is StackFrame)
			{
				return ((StackFrame)scriptObject).thisObject;
			}
			return ((GlobalScope)scriptObject).thisObject;
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x00059424 File Offset: 0x00058424
		internal override IReflect InferType(JSField inference_target)
		{
			if (this.method != null)
			{
				ParameterInfo[] parameters = this.method.GetParameters();
				if (parameters == null || parameters.Length == 0)
				{
					return this.method.ReturnType;
				}
				return parameters[0].ParameterType;
			}
			else
			{
				ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
				while (scriptObject is WithObject)
				{
					scriptObject = scriptObject.GetParent();
				}
				if (scriptObject is GlobalScope)
				{
					return scriptObject;
				}
				if (!(scriptObject is FunctionScope) || !((FunctionScope)scriptObject).isMethod)
				{
					return Typeob.Object;
				}
				ClassScope classScope = (ClassScope)((FunctionScope)scriptObject).owner.enclosing_scope;
				if (this.isSuper)
				{
					return classScope.GetSuperType();
				}
				return classScope;
			}
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x000594D0 File Offset: 0x000584D0
		internal override AST PartiallyEvaluate()
		{
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject)
			{
				scriptObject = scriptObject.GetParent();
			}
			bool flag = false;
			if (scriptObject is FunctionScope)
			{
				flag = ((FunctionScope)scriptObject).isStatic && ((FunctionScope)scriptObject).isMethod;
			}
			else if (scriptObject is StackFrame)
			{
				flag = ((StackFrame)scriptObject).thisObject is Type;
			}
			if (flag)
			{
				this.context.HandleError(JSError.NotAccessible);
				return new Lookup("this", this.context).PartiallyEvaluate();
			}
			return this;
		}

		// Token: 0x06000BC1 RID: 3009 RVA: 0x0005956D File Offset: 0x0005856D
		internal override AST PartiallyEvaluateAsReference()
		{
			this.context.HandleError(JSError.CantAssignThis);
			return new Lookup("this", this.context).PartiallyEvaluateAsReference();
		}

		// Token: 0x06000BC2 RID: 3010 RVA: 0x00059594 File Offset: 0x00058594
		internal void ResolveAssignmentToDefaultIndexedProperty(ASTList args, IReflect[] argIRs, AST rhvalue)
		{
			IReflect reflect = this.InferType(null);
			Type type = ((reflect is Type) ? ((Type)reflect) : null);
			if (reflect is ClassScope)
			{
				type = ((ClassScope)reflect).GetBakedSuperType();
			}
			MemberInfo[] defaultMembers = JSBinder.GetDefaultMembers(type);
			if (defaultMembers != null && defaultMembers.Length > 0)
			{
				try
				{
					PropertyInfo propertyInfo = JSBinder.SelectProperty(defaultMembers, argIRs);
					if (propertyInfo != null)
					{
						this.method = JSProperty.GetSetMethod(propertyInfo, true);
						if (this.method == null)
						{
							this.context.HandleError(JSError.AssignmentToReadOnly, true);
						}
						if (!Binding.CheckParameters(propertyInfo.GetIndexParameters(), argIRs, args, this.context, 0, false, true))
						{
							this.method = null;
						}
						return;
					}
				}
				catch (AmbiguousMatchException)
				{
					this.context.HandleError(JSError.AmbiguousMatch);
					return;
				}
			}
			string text = ((reflect is ClassScope) ? ((ClassScope)reflect).GetName() : ((Type)reflect).Name);
			this.context.HandleError(JSError.NotIndexable, text);
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0005968C File Offset: 0x0005868C
		internal override void ResolveCall(ASTList args, IReflect[] argIRs, bool constructor, bool brackets)
		{
			if (!constructor && brackets)
			{
				IReflect reflect = this.InferType(null);
				Type type = ((reflect is Type) ? ((Type)reflect) : null);
				if (reflect is ClassScope)
				{
					type = ((ClassScope)reflect).GetBakedSuperType();
				}
				MemberInfo[] defaultMembers = JSBinder.GetDefaultMembers(type);
				if (defaultMembers != null && defaultMembers.Length > 0)
				{
					try
					{
						this.method = JSBinder.SelectMethod(defaultMembers, argIRs);
						if (this.method != null)
						{
							if (!Binding.CheckParameters(this.method.GetParameters(), argIRs, args, this.context, 0, false, true))
							{
								this.method = null;
							}
							return;
						}
					}
					catch (AmbiguousMatchException)
					{
						this.context.HandleError(JSError.AmbiguousMatch);
						return;
					}
				}
				string text = ((reflect is ClassScope) ? ((ClassScope)reflect).GetName() : ((Type)reflect).Name);
				this.context.HandleError(JSError.NotIndexable, text);
				return;
			}
			if (this.isSuper)
			{
				this.context.HandleError(JSError.IllegalUseOfSuper);
				return;
			}
			this.context.HandleError(JSError.IllegalUseOfThis);
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0005979C File Offset: 0x0005879C
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (rtype == Typeob.Void)
			{
				return;
			}
			IReflect reflect = this.InferType(null);
			if (!(reflect is GlobalScope))
			{
				il.Emit(OpCodes.Ldarg_0);
				Convert.Emit(this, il, Convert.ToType(this.InferType(null)), rtype);
				return;
			}
			base.EmitILToLoadEngine(il);
			if (rtype == Typeob.LenientGlobalObject)
			{
				il.Emit(OpCodes.Call, CompilerGlobals.getLenientGlobalObjectMethod);
				return;
			}
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.getDefaultThisObjectMethod);
		}

		// Token: 0x06000BC5 RID: 3013 RVA: 0x00059834 File Offset: 0x00058834
		internal override void TranslateToILCall(ILGenerator il, Type rtype, ASTList argList, bool construct, bool brackets)
		{
			MethodInfo methodInfo = this.method;
			if (methodInfo != null)
			{
				Type reflectedType = methodInfo.ReflectedType;
				if (!methodInfo.IsStatic)
				{
					this.method = null;
					this.TranslateToIL(il, reflectedType);
					this.method = methodInfo;
				}
				ParameterInfo[] parameters = methodInfo.GetParameters();
				Binding.PlaceArgumentsOnStack(il, parameters, argList, 0, 0, Binding.ReflectionMissingCW);
				if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!reflectedType.IsSealed || !reflectedType.IsValueType))
				{
					il.Emit(OpCodes.Callvirt, methodInfo);
				}
				else
				{
					il.Emit(OpCodes.Call, methodInfo);
				}
				Convert.Emit(this, il, methodInfo.ReturnType, rtype);
				return;
			}
			base.TranslateToILCall(il, rtype, argList, construct, brackets);
		}

		// Token: 0x06000BC6 RID: 3014 RVA: 0x000598E0 File Offset: 0x000588E0
		internal override void TranslateToILPreSet(ILGenerator il, ASTList argList)
		{
			MethodInfo methodInfo = this.method;
			if (methodInfo != null)
			{
				Type reflectedType = methodInfo.ReflectedType;
				if (!methodInfo.IsStatic)
				{
					this.TranslateToIL(il, reflectedType);
				}
				Binding.PlaceArgumentsOnStack(il, methodInfo.GetParameters(), argList, 0, 1, Binding.ReflectionMissingCW);
				return;
			}
			base.TranslateToILPreSet(il, argList);
		}

		// Token: 0x06000BC7 RID: 3015 RVA: 0x0005992C File Offset: 0x0005892C
		internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
		{
			MethodInfo methodInfo = this.method;
			if (methodInfo != null)
			{
				if (rhvalue != null)
				{
					rhvalue.TranslateToIL(il, methodInfo.GetParameters()[0].ParameterType);
				}
				Type reflectedType = methodInfo.ReflectedType;
				if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!reflectedType.IsSealed || !reflectedType.IsValueType))
				{
					il.Emit(OpCodes.Callvirt, methodInfo);
				}
				else
				{
					il.Emit(OpCodes.Call, methodInfo);
				}
				if (methodInfo.ReturnType != Typeob.Void)
				{
					il.Emit(OpCodes.Pop);
					return;
				}
			}
			else
			{
				base.TranslateToILSet(il, rhvalue);
			}
		}

		// Token: 0x06000BC8 RID: 3016 RVA: 0x000599BC File Offset: 0x000589BC
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}

		// Token: 0x04000704 RID: 1796
		internal bool isSuper;

		// Token: 0x04000705 RID: 1797
		private MethodInfo method;
	}
}
