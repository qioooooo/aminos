using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000E5 RID: 229
	internal sealed class Lookup : Binding
	{
		// Token: 0x06000A1A RID: 2586 RVA: 0x0004C603 File Offset: 0x0004B603
		internal Lookup(Context context)
			: base(context, context.GetCode())
		{
			this.lexLevel = 0;
			this.evalLexLevel = 0;
			this.fieldLoc = null;
			this.refLoc = null;
			this.lateBinding = null;
			this.thereIsAnObjectOnTheStack = false;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x0004C63C File Offset: 0x0004B63C
		internal Lookup(string name, Context context)
			: this(context)
		{
			this.name = name;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06000A1C RID: 2588 RVA: 0x0004C64C File Offset: 0x0004B64C
		internal string Name
		{
			get
			{
				return this.name;
			}
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x0004C654 File Offset: 0x0004B654
		private void BindName()
		{
			int num = 0;
			int num2 = 0;
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			BindingFlags bindingFlags = BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
			bool flag = false;
			bool flag2 = false;
			while (scriptObject != null)
			{
				WithObject withObject = scriptObject as WithObject;
				MemberInfo[] array;
				if (withObject != null && flag2)
				{
					array = withObject.GetMember(this.name, bindingFlags, false);
				}
				else
				{
					array = scriptObject.GetMember(this.name, bindingFlags);
				}
				this.members = array;
				if (array.Length > 0)
				{
					break;
				}
				if (scriptObject is WithObject)
				{
					this.isFullyResolved = this.isFullyResolved && ((WithObject)scriptObject).isKnownAtCompileTime;
					num++;
				}
				else if (scriptObject is ActivationObject)
				{
					this.isFullyResolved = this.isFullyResolved && ((ActivationObject)scriptObject).isKnownAtCompileTime;
					if (scriptObject is BlockScope || (scriptObject is FunctionScope && ((FunctionScope)scriptObject).mustSaveStackLocals))
					{
						num++;
					}
					if (scriptObject is ClassScope)
					{
						if (flag)
						{
							flag2 = true;
						}
						if (((ClassScope)scriptObject).owner.isStatic)
						{
							bindingFlags &= ~BindingFlags.Instance;
							flag = true;
						}
					}
				}
				else if (scriptObject is StackFrame)
				{
					num++;
				}
				num2++;
				scriptObject = scriptObject.GetParent();
			}
			if (this.members.Length > 0)
			{
				this.lexLevel = num;
				this.evalLexLevel = num2;
			}
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0004C79C File Offset: 0x0004B79C
		internal bool CanPlaceAppropriateObjectOnStack(object ob)
		{
			if (ob is LenientGlobalObject)
			{
				return true;
			}
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			int num = this.lexLevel;
			while (num > 0 && (scriptObject is WithObject || scriptObject is BlockScope))
			{
				if (scriptObject is WithObject)
				{
					num--;
				}
				scriptObject = scriptObject.GetParent();
			}
			return scriptObject is WithObject || scriptObject is GlobalScope;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x0004C808 File Offset: 0x0004B808
		internal override void CheckIfOKToUseInSuperConstructorCall()
		{
			FieldInfo fieldInfo = this.member as FieldInfo;
			if (fieldInfo != null)
			{
				if (!fieldInfo.IsStatic)
				{
					this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
				}
				return;
			}
			MethodInfo methodInfo = this.member as MethodInfo;
			if (methodInfo != null)
			{
				if (!methodInfo.IsStatic)
				{
					this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
				}
				return;
			}
			PropertyInfo propertyInfo = this.member as PropertyInfo;
			if (propertyInfo != null)
			{
				methodInfo = JSProperty.GetGetMethod(propertyInfo, true);
				if (methodInfo != null && !methodInfo.IsStatic)
				{
					this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
					return;
				}
				methodInfo = JSProperty.GetSetMethod(propertyInfo, true);
				if (methodInfo != null && !methodInfo.IsStatic)
				{
					this.context.HandleError(JSError.NotAllowedInSuperConstructorCall);
				}
			}
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x0004C8BC File Offset: 0x0004B8BC
		internal override object Evaluate()
		{
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			object obj;
			if (!this.isFullyResolved)
			{
				obj = ((IActivationObject)scriptObject).GetMemberValue(this.name, this.evalLexLevel);
				if (!(obj is Missing))
				{
					return obj;
				}
			}
			if (this.members == null && !VsaEngine.executeForJSEE)
			{
				this.BindName();
				base.ResolveRHValue();
			}
			obj = base.Evaluate();
			if (obj is Missing)
			{
				throw new JScriptException(JSError.UndefinedIdentifier, this.context);
			}
			return obj;
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0004C944 File Offset: 0x0004B944
		internal override LateBinding EvaluateAsLateBinding()
		{
			if (!this.isFullyResolved)
			{
				this.BindName();
				this.isFullyResolved = false;
			}
			if (this.defaultMember == this.member)
			{
				this.defaultMember = null;
			}
			object @object = this.GetObject();
			LateBinding lateBinding = this.lateBinding;
			if (lateBinding == null)
			{
				lateBinding = (this.lateBinding = new LateBinding(this.name, @object, VsaEngine.executeForJSEE));
			}
			lateBinding.obj = @object;
			lateBinding.last_object = @object;
			lateBinding.last_members = this.members;
			lateBinding.last_member = this.member;
			if (!this.isFullyResolved)
			{
				this.members = null;
			}
			return lateBinding;
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x0004C9E0 File Offset: 0x0004B9E0
		internal override WrappedNamespace EvaluateAsWrappedNamespace(bool giveErrorIfNameInUse)
		{
			Namespace @namespace = Namespace.GetNamespace(this.name, base.Engine);
			GlobalScope globalScope = ((IActivationObject)base.Globals.ScopeStack.Peek()).GetGlobalScope();
			FieldInfo fieldInfo = (giveErrorIfNameInUse ? globalScope.GetLocalField(this.name) : globalScope.GetField(this.name, BindingFlags.Static | BindingFlags.Public));
			if (fieldInfo != null)
			{
				if (giveErrorIfNameInUse && (!fieldInfo.IsLiteral || !(fieldInfo.GetValue(null) is Namespace)))
				{
					this.context.HandleError(JSError.DuplicateName, true);
				}
			}
			else
			{
				fieldInfo = globalScope.AddNewField(this.name, @namespace, FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Literal);
				((JSVariableField)fieldInfo).type = new TypeExpression(new ConstantWrapper(Typeob.Namespace, this.context));
				((JSVariableField)fieldInfo).originalContext = this.context;
			}
			return new WrappedNamespace(this.name, base.Engine);
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x0004CABC File Offset: 0x0004BABC
		protected override object GetObject()
		{
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			object obj;
			if (this.member is JSMemberField)
			{
				while (scriptObject != null)
				{
					StackFrame stackFrame = scriptObject as StackFrame;
					if (stackFrame != null)
					{
						obj = stackFrame.closureInstance;
						goto IL_0059;
					}
					scriptObject = scriptObject.GetParent();
				}
				return null;
			}
			for (int i = this.evalLexLevel; i > 0; i--)
			{
				scriptObject = scriptObject.GetParent();
			}
			obj = scriptObject;
			IL_0059:
			if (this.defaultMember != null)
			{
				MemberTypes memberType = this.defaultMember.MemberType;
				if (memberType <= MemberTypes.Method)
				{
					switch (memberType)
					{
					case MemberTypes.Event:
						return null;
					case MemberTypes.Constructor | MemberTypes.Event:
						break;
					case MemberTypes.Field:
						return ((FieldInfo)this.defaultMember).GetValue(obj);
					default:
						if (memberType == MemberTypes.Method)
						{
							return ((MethodInfo)this.defaultMember).Invoke(obj, new object[0]);
						}
						break;
					}
				}
				else
				{
					if (memberType == MemberTypes.Property)
					{
						return ((PropertyInfo)this.defaultMember).GetValue(obj, null);
					}
					if (memberType == MemberTypes.NestedType)
					{
						return this.member;
					}
				}
			}
			return obj;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x0004CBB3 File Offset: 0x0004BBB3
		protected override void HandleNoSuchMemberError()
		{
			if (!this.isFullyResolved)
			{
				return;
			}
			this.context.HandleError(JSError.UndeclaredVariable, base.Engine.doFast);
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x0004CBD9 File Offset: 0x0004BBD9
		internal override IReflect InferType(JSField inference_target)
		{
			if (!this.isFullyResolved)
			{
				return Typeob.Object;
			}
			return base.InferType(inference_target);
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x0004CBF0 File Offset: 0x0004BBF0
		internal bool InFunctionNestedInsideInstanceMethod()
		{
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject || scriptObject is BlockScope)
			{
				scriptObject = scriptObject.GetParent();
			}
			for (FunctionScope functionScope = scriptObject as FunctionScope; functionScope != null; functionScope = scriptObject as FunctionScope)
			{
				if (functionScope.owner.isMethod)
				{
					return !functionScope.owner.isStatic;
				}
				scriptObject = functionScope.owner.enclosing_scope;
				while (scriptObject is WithObject || scriptObject is BlockScope)
				{
					scriptObject = scriptObject.GetParent();
				}
			}
			return false;
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x0004CC7C File Offset: 0x0004BC7C
		internal bool InStaticCode()
		{
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject || scriptObject is BlockScope)
			{
				scriptObject = scriptObject.GetParent();
			}
			FunctionScope functionScope = scriptObject as FunctionScope;
			if (functionScope != null)
			{
				return functionScope.isStatic;
			}
			StackFrame stackFrame = scriptObject as StackFrame;
			if (stackFrame != null)
			{
				return stackFrame.thisObject is Type;
			}
			ClassScope classScope = scriptObject as ClassScope;
			return classScope == null || classScope.inStaticInitializerCode;
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x0004CCF0 File Offset: 0x0004BCF0
		internal override AST PartiallyEvaluate()
		{
			this.BindName();
			if (this.members == null || this.members.Length == 0)
			{
				ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
				while (scriptObject is FunctionScope)
				{
					scriptObject = scriptObject.GetParent();
				}
				if (!(scriptObject is WithObject) || this.isFullyResolved)
				{
					this.context.HandleError(JSError.UndeclaredVariable, this.isFullyResolved && base.Engine.doFast);
				}
			}
			else
			{
				base.ResolveRHValue();
				MemberInfo member = this.member;
				if (member is FieldInfo)
				{
					FieldInfo fieldInfo = (FieldInfo)member;
					if (fieldInfo is JSLocalField && !((JSLocalField)fieldInfo).isDefined)
					{
						((JSLocalField)fieldInfo).isUsedBeforeDefinition = true;
						this.context.HandleError(JSError.VariableMightBeUnitialized);
					}
					if (fieldInfo.IsLiteral)
					{
						object obj = ((fieldInfo is JSVariableField) ? ((JSVariableField)fieldInfo).value : TypeReferences.GetConstantValue(fieldInfo));
						if (obj is AST)
						{
							AST ast = ((AST)obj).PartiallyEvaluate();
							if (ast is ConstantWrapper && this.isFullyResolved)
							{
								return ast;
							}
							obj = null;
						}
						if (!(obj is FunctionObject) && this.isFullyResolved)
						{
							return new ConstantWrapper(obj, this.context);
						}
					}
					else if (fieldInfo.IsInitOnly && fieldInfo.IsStatic && fieldInfo.DeclaringType == Typeob.GlobalObject && this.isFullyResolved)
					{
						return new ConstantWrapper(fieldInfo.GetValue(null), this.context);
					}
				}
				else if (member is PropertyInfo)
				{
					PropertyInfo propertyInfo = (PropertyInfo)member;
					if (!propertyInfo.CanWrite && !(propertyInfo is JSProperty) && propertyInfo.DeclaringType == Typeob.GlobalObject && this.isFullyResolved)
					{
						return new ConstantWrapper(propertyInfo.GetValue(null, null), this.context);
					}
				}
				if (member is Type && this.isFullyResolved)
				{
					return new ConstantWrapper(member, this.context);
				}
			}
			return this;
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0004CEDA File Offset: 0x0004BEDA
		internal override AST PartiallyEvaluateAsCallable()
		{
			this.BindName();
			return this;
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x0004CEE4 File Offset: 0x0004BEE4
		internal override AST PartiallyEvaluateAsReference()
		{
			this.BindName();
			if (this.members == null || this.members.Length == 0)
			{
				ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
				if (!(scriptObject is WithObject) || this.isFullyResolved)
				{
					this.context.HandleError(JSError.UndeclaredVariable, this.isFullyResolved && base.Engine.doFast);
				}
			}
			else
			{
				base.ResolveLHValue();
			}
			return this;
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x0004CF5C File Offset: 0x0004BF5C
		internal override object ResolveCustomAttribute(ASTList args, IReflect[] argIRs, AST target)
		{
			if (this.name == "expando")
			{
				this.members = Typeob.Expando.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			}
			else if (this.name == "override")
			{
				this.members = Typeob.Override.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			}
			else if (this.name == "hide")
			{
				this.members = Typeob.Hide.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			}
			else if (this.name == "...")
			{
				this.members = Typeob.ParamArrayAttribute.GetConstructors(BindingFlags.Instance | BindingFlags.Public);
			}
			else
			{
				this.name += "Attribute";
				this.BindName();
				if (this.members == null || this.members.Length == 0)
				{
					this.name = this.name.Substring(0, this.name.Length - 9);
					this.BindName();
				}
			}
			return base.ResolveCustomAttribute(args, argIRs, target);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0004D064 File Offset: 0x0004C064
		internal override void SetPartialValue(AST partial_value)
		{
			if (this.members == null || this.members.Length == 0)
			{
				return;
			}
			if (this.member is JSLocalField)
			{
				JSLocalField jslocalField = (JSLocalField)this.member;
				if (jslocalField.type == null)
				{
					IReflect reflect = partial_value.InferType(jslocalField);
					if (reflect == Typeob.String && partial_value is Plus)
					{
						jslocalField.SetInferredType(Typeob.Object, partial_value);
						return;
					}
					jslocalField.SetInferredType(reflect, partial_value);
					return;
				}
				else
				{
					jslocalField.isDefined = true;
				}
			}
			Binding.AssignmentCompatible(this.InferType(null), partial_value, partial_value.InferType(null), this.isFullyResolved);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0004D0F5 File Offset: 0x0004C0F5
		internal override void SetValue(object value)
		{
			if (!this.isFullyResolved)
			{
				this.EvaluateAsLateBinding().SetValue(value);
				return;
			}
			base.SetValue(value);
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x0004D114 File Offset: 0x0004C114
		internal void SetWithValue(WithObject scope, object value)
		{
			FieldInfo field = scope.GetField(this.name, this.lexLevel);
			if (field != null)
			{
				field.SetValue(scope, value);
			}
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x0004D13F File Offset: 0x0004C13F
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x0004D148 File Offset: 0x0004C148
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.isFullyResolved)
			{
				base.TranslateToIL(il, rtype);
				return;
			}
			Label label = il.DefineLabel();
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
			il.Emit(OpCodes.Ldstr, this.name);
			ConstantWrapper.TranslateToILInt(il, this.lexLevel);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.getMemberValueMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Call, CompilerGlobals.isMissingMethod);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Pop);
			base.TranslateToIL(il, Typeob.Object);
			il.MarkLabel(label);
			Convert.Emit(this, il, Typeob.Object, rtype);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x0004D214 File Offset: 0x0004C214
		internal override void TranslateToILCall(ILGenerator il, Type rtype, ASTList argList, bool construct, bool brackets)
		{
			if (this.isFullyResolved)
			{
				base.TranslateToILCall(il, rtype, argList, construct, brackets);
				return;
			}
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
			il.Emit(OpCodes.Ldstr, this.name);
			ConstantWrapper.TranslateToILInt(il, this.lexLevel);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.getMemberValueMethod);
			il.Emit(OpCodes.Dup);
			il.Emit(OpCodes.Call, CompilerGlobals.isMissingMethod);
			il.Emit(OpCodes.Brfalse, label);
			il.Emit(OpCodes.Pop);
			base.TranslateToILCall(il, Typeob.Object, argList, construct, brackets);
			il.Emit(OpCodes.Br, label2);
			il.MarkLabel(label);
			this.TranslateToILDefaultThisObject(il);
			argList.TranslateToIL(il, Typeob.ArrayOfObject);
			if (construct)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			if (brackets)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.callValue2Method);
			il.MarkLabel(label2);
			Convert.Emit(this, il, Typeob.Object, rtype);
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x0004D365 File Offset: 0x0004C365
		internal void TranslateToILDefaultThisObject(ILGenerator il)
		{
			this.TranslateToILDefaultThisObject(il, 0);
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0004D370 File Offset: 0x0004C370
		private void TranslateToILDefaultThisObject(ILGenerator il, int lexLevel)
		{
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			while (lexLevel-- > 0)
			{
				il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
			}
			il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.getDefaultThisObjectMethod);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x0004D3D0 File Offset: 0x0004C3D0
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			if (this.defaultMember != null)
			{
				return;
			}
			if (this.member != null)
			{
				MemberTypes memberType = this.member.MemberType;
				if (memberType <= MemberTypes.Method)
				{
					if (memberType != MemberTypes.Constructor)
					{
						if (memberType != MemberTypes.Field)
						{
							if (memberType != MemberTypes.Method)
							{
								goto IL_0059;
							}
						}
						else
						{
							if (this.member is JSExpandoField)
							{
								this.member = null;
								goto IL_0059;
							}
							return;
						}
					}
				}
				else if (memberType != MemberTypes.Property && memberType != MemberTypes.TypeInfo && memberType != MemberTypes.NestedType)
				{
					goto IL_0059;
				}
				return;
			}
			IL_0059:
			this.refLoc = il.DeclareLocal(Typeob.LateBinding);
			il.Emit(OpCodes.Ldstr, this.name);
			if (this.isFullyResolved && this.member == null && this.IsBoundToMethodInfos())
			{
				MethodInfo methodInfo = this.members[0] as MethodInfo;
				if (methodInfo.IsStatic)
				{
					il.Emit(OpCodes.Ldtoken, methodInfo.DeclaringType);
					il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
				}
				else
				{
					this.TranslateToILObjectForMember(il, methodInfo.DeclaringType, false, methodInfo);
				}
			}
			else
			{
				base.EmitILToLoadEngine(il);
				il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			}
			il.Emit(OpCodes.Newobj, CompilerGlobals.lateBindingConstructor2);
			il.Emit(OpCodes.Stloc, this.refLoc);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x0004D4F4 File Offset: 0x0004C4F4
		private bool IsBoundToMethodInfos()
		{
			if (this.members == null || this.members.Length == 0)
			{
				return false;
			}
			for (int i = 0; i < this.members.Length; i++)
			{
				if (!(this.members[i] is MethodInfo))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x0004D53A File Offset: 0x0004C53A
		protected override void TranslateToILObject(ILGenerator il, Type obType, bool noValue)
		{
			this.TranslateToILObjectForMember(il, obType, noValue, this.member);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x0004D54C File Offset: 0x0004C54C
		private void TranslateToILObjectForMember(ILGenerator il, Type obType, bool noValue, MemberInfo mem)
		{
			this.thereIsAnObjectOnTheStack = true;
			if (!(mem is IWrappedMember))
			{
				ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
				while (scriptObject is WithObject || scriptObject is BlockScope)
				{
					scriptObject = scriptObject.GetParent();
				}
				if (scriptObject is FunctionScope)
				{
					FunctionObject owner = ((FunctionScope)scriptObject).owner;
					if (!owner.isMethod)
					{
						base.EmitILToLoadEngine(il);
						il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
						scriptObject = base.Globals.ScopeStack.Peek();
						while (scriptObject is WithObject || scriptObject is BlockScope)
						{
							il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
							scriptObject = scriptObject.GetParent();
						}
						il.Emit(OpCodes.Castclass, Typeob.StackFrame);
						il.Emit(OpCodes.Ldfld, CompilerGlobals.closureInstanceField);
						while (scriptObject != null)
						{
							if (scriptObject is ClassScope)
							{
								ClassScope classScope = (ClassScope)scriptObject;
								if (classScope.IsSameOrDerivedFrom(obType))
								{
									break;
								}
								il.Emit(OpCodes.Castclass, classScope.GetTypeBuilder());
								il.Emit(OpCodes.Ldfld, classScope.outerClassField);
							}
							scriptObject = scriptObject.GetParent();
						}
						il.Emit(OpCodes.Castclass, obType);
						return;
					}
				}
				il.Emit(OpCodes.Ldarg_0);
				while (scriptObject != null)
				{
					if (scriptObject is ClassScope)
					{
						ClassScope classScope2 = (ClassScope)scriptObject;
						if (classScope2.IsSameOrDerivedFrom(obType))
						{
							return;
						}
						il.Emit(OpCodes.Ldfld, classScope2.outerClassField);
					}
					scriptObject = scriptObject.GetParent();
				}
				return;
			}
			object wrappedObject = ((IWrappedMember)mem).GetWrappedObject();
			if (wrappedObject is LenientGlobalObject)
			{
				base.EmitILToLoadEngine(il);
				il.Emit(OpCodes.Call, CompilerGlobals.getLenientGlobalObjectMethod);
				return;
			}
			if (!(wrappedObject is Type) && !(wrappedObject is ClassScope))
			{
				this.TranslateToILDefaultThisObject(il, this.lexLevel);
				Convert.Emit(this, il, Typeob.Object, obType);
				return;
			}
			if (obType.IsAssignableFrom(Typeob.Type))
			{
				new ConstantWrapper(wrappedObject, null).TranslateToIL(il, Typeob.Type);
				return;
			}
			ScriptObject scriptObject2 = base.Globals.ScopeStack.Peek();
			while (scriptObject2 is WithObject || scriptObject2 is BlockScope)
			{
				scriptObject2 = scriptObject2.GetParent();
			}
			if (scriptObject2 is FunctionScope)
			{
				FunctionObject owner2 = ((FunctionScope)scriptObject2).owner;
				if (owner2.isMethod)
				{
					il.Emit(OpCodes.Ldarg_0);
				}
				else
				{
					base.EmitILToLoadEngine(il);
					il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
					scriptObject2 = base.Globals.ScopeStack.Peek();
					while (scriptObject2 is WithObject || scriptObject2 is BlockScope)
					{
						il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
						scriptObject2 = scriptObject2.GetParent();
					}
					il.Emit(OpCodes.Castclass, Typeob.StackFrame);
					il.Emit(OpCodes.Ldfld, CompilerGlobals.closureInstanceField);
				}
			}
			else if (scriptObject2 is ClassScope)
			{
				il.Emit(OpCodes.Ldarg_0);
			}
			for (scriptObject2 = base.Globals.ScopeStack.Peek(); scriptObject2 != null; scriptObject2 = scriptObject2.GetParent())
			{
				ClassScope classScope3 = scriptObject2 as ClassScope;
				if (classScope3 != null)
				{
					if (classScope3.IsSameOrDerivedFrom(obType))
					{
						return;
					}
					il.Emit(OpCodes.Ldfld, classScope3.outerClassField);
				}
			}
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x0004D879 File Offset: 0x0004C879
		internal override void TranslateToILPreSet(ILGenerator il)
		{
			this.TranslateToILPreSet(il, false);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x0004D884 File Offset: 0x0004C884
		internal void TranslateToILPreSet(ILGenerator il, bool doBoth)
		{
			if (this.isFullyResolved)
			{
				base.TranslateToILPreSet(il);
				return;
			}
			Label label = il.DefineLabel();
			LocalBuilder localBuilder = (this.fieldLoc = il.DeclareLocal(Typeob.FieldInfo));
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
			il.Emit(OpCodes.Ldstr, this.name);
			ConstantWrapper.TranslateToILInt(il, this.lexLevel);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.getFieldMethod);
			il.Emit(OpCodes.Stloc, localBuilder);
			if (!doBoth)
			{
				il.Emit(OpCodes.Ldloc, localBuilder);
				il.Emit(OpCodes.Ldnull);
				il.Emit(OpCodes.Bne_Un_S, label);
			}
			base.TranslateToILPreSet(il);
			if (this.thereIsAnObjectOnTheStack)
			{
				Label label2 = il.DefineLabel();
				il.Emit(OpCodes.Br_S, label2);
				il.MarkLabel(label);
				il.Emit(OpCodes.Ldnull);
				il.MarkLabel(label2);
				return;
			}
			il.MarkLabel(label);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x0004D988 File Offset: 0x0004C988
		internal override void TranslateToILPreSetPlusGet(ILGenerator il)
		{
			if (this.isFullyResolved)
			{
				base.TranslateToILPreSetPlusGet(il);
				return;
			}
			Label label = il.DefineLabel();
			Label label2 = il.DefineLabel();
			LocalBuilder localBuilder = (this.fieldLoc = il.DeclareLocal(Typeob.FieldInfo));
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			il.Emit(OpCodes.Castclass, Typeob.IActivationObject);
			il.Emit(OpCodes.Ldstr, this.name);
			ConstantWrapper.TranslateToILInt(il, this.lexLevel);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.getFieldMethod);
			il.Emit(OpCodes.Stloc, localBuilder);
			il.Emit(OpCodes.Ldloc, localBuilder);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Bne_Un_S, label2);
			base.TranslateToILPreSetPlusGet(il);
			il.Emit(OpCodes.Br_S, label);
			il.MarkLabel(label2);
			if (this.thereIsAnObjectOnTheStack)
			{
				il.Emit(OpCodes.Ldnull);
			}
			il.Emit(OpCodes.Ldloc, this.fieldLoc);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.getFieldValueMethod);
			il.MarkLabel(label);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x0004DAAB File Offset: 0x0004CAAB
		internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
		{
			this.TranslateToILSet(il, false, rhvalue);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x0004DAB8 File Offset: 0x0004CAB8
		internal void TranslateToILSet(ILGenerator il, bool doBoth, AST rhvalue)
		{
			if (this.isFullyResolved)
			{
				base.TranslateToILSet(il, rhvalue);
				return;
			}
			if (rhvalue != null)
			{
				rhvalue.TranslateToIL(il, Typeob.Object);
			}
			if (this.fieldLoc == null)
			{
				il.Emit(OpCodes.Call, CompilerGlobals.setIndexedPropertyValueStaticMethod);
				return;
			}
			LocalBuilder localBuilder = il.DeclareLocal(Typeob.Object);
			if (doBoth)
			{
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Stloc, localBuilder);
				this.isFullyResolved = true;
				Convert.Emit(this, il, Typeob.Object, Convert.ToType(this.InferType(null)));
				base.TranslateToILSet(il, null);
			}
			Label label = il.DefineLabel();
			il.Emit(OpCodes.Ldloc, this.fieldLoc);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Beq_S, label);
			Label label2 = il.DefineLabel();
			if (!doBoth)
			{
				il.Emit(OpCodes.Stloc, localBuilder);
				if (this.thereIsAnObjectOnTheStack)
				{
					il.Emit(OpCodes.Pop);
				}
			}
			il.Emit(OpCodes.Ldloc, this.fieldLoc);
			il.Emit(OpCodes.Ldnull);
			il.Emit(OpCodes.Ldloc, localBuilder);
			il.Emit(OpCodes.Callvirt, CompilerGlobals.setFieldValueMethod);
			il.Emit(OpCodes.Br_S, label2);
			il.MarkLabel(label);
			if (!doBoth)
			{
				this.isFullyResolved = true;
				Convert.Emit(this, il, Typeob.Object, Convert.ToType(this.InferType(null)));
				base.TranslateToILSet(il, null);
			}
			il.MarkLabel(label2);
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x0004DC1F File Offset: 0x0004CC1F
		protected override void TranslateToILWithDupOfThisOb(ILGenerator il)
		{
			this.TranslateToILDefaultThisObject(il);
			this.TranslateToIL(il, Typeob.Object);
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x0004DC34 File Offset: 0x0004CC34
		internal void TranslateToLateBinding(ILGenerator il)
		{
			this.thereIsAnObjectOnTheStack = true;
			il.Emit(OpCodes.Ldloc, this.refLoc);
		}

		// Token: 0x0400065B RID: 1627
		private int lexLevel;

		// Token: 0x0400065C RID: 1628
		private int evalLexLevel;

		// Token: 0x0400065D RID: 1629
		private LocalBuilder fieldLoc;

		// Token: 0x0400065E RID: 1630
		private LocalBuilder refLoc;

		// Token: 0x0400065F RID: 1631
		private LateBinding lateBinding;

		// Token: 0x04000660 RID: 1632
		private bool thereIsAnObjectOnTheStack;
	}
}
