using System;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000035 RID: 53
	internal class Call : AST
	{
		// Token: 0x0600020A RID: 522 RVA: 0x00010044 File Offset: 0x0000F044
		internal Call(Context context, AST func, ASTList args, bool inBrackets)
			: base(context)
		{
			this.func = func;
			this.args = ((args == null) ? new ASTList(context) : args);
			this.argValues = null;
			this.outParameterCount = 0;
			int i = 0;
			int count = this.args.count;
			while (i < count)
			{
				if (this.args[i] is AddressOf)
				{
					this.outParameterCount++;
				}
				i++;
			}
			this.isConstructor = false;
			this.inBrackets = inBrackets;
			this.enclosingFunctionScope = null;
			this.alreadyPartiallyEvaluated = false;
			this.isAssignmentToDefaultIndexedProperty = false;
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (!(scriptObject is FunctionScope))
			{
				scriptObject = scriptObject.GetParent();
				if (scriptObject == null)
				{
					return;
				}
			}
			this.enclosingFunctionScope = (FunctionScope)scriptObject;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x00010110 File Offset: 0x0000F110
		private bool AllParamsAreMissing()
		{
			int i = 0;
			int count = this.args.count;
			while (i < count)
			{
				AST ast = this.args[i];
				if (!(ast is ConstantWrapper) || ((ConstantWrapper)ast).value != Missing.Value)
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x0600020C RID: 524 RVA: 0x00010160 File Offset: 0x0000F160
		private IReflect[] ArgIRs()
		{
			int count = this.args.count;
			IReflect[] array = new IReflect[count];
			for (int i = 0; i < count; i++)
			{
				AST ast = this.args[i];
				IReflect reflect = (array[i] = ast.InferType(null));
				if (ast is AddressOf)
				{
					if (reflect is ClassScope)
					{
						reflect = ((ClassScope)reflect).GetBakedSuperType();
					}
					array[i] = Convert.ToType("&", Convert.ToType(reflect));
				}
			}
			return array;
		}

		// Token: 0x0600020D RID: 525 RVA: 0x000101E0 File Offset: 0x0000F1E0
		internal bool CanBeFunctionDeclaration()
		{
			bool flag = this.func is Lookup && this.outParameterCount == 0;
			if (flag)
			{
				int i = 0;
				int count = this.args.count;
				while (i < count)
				{
					AST ast = this.args[i];
					flag = ast is Lookup;
					if (!flag)
					{
						break;
					}
					i++;
				}
			}
			return flag;
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0001023D File Offset: 0x0000F23D
		internal override void CheckIfOKToUseInSuperConstructorCall()
		{
			this.func.CheckIfOKToUseInSuperConstructorCall();
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0001024C File Offset: 0x0000F24C
		internal override bool Delete()
		{
			object[] array = ((this.args == null) ? null : this.args.EvaluateAsArray());
			int num = array.Length;
			object obj = this.func.Evaluate();
			if (obj == null)
			{
				return true;
			}
			if (num == 0)
			{
				return true;
			}
			Type type = obj.GetType();
			MethodInfo methodInfo = type.GetMethod("op_Delete", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[]
			{
				type,
				Typeob.ArrayOfObject
			}, null);
			if (methodInfo == null || (methodInfo.Attributes & MethodAttributes.SpecialName) == MethodAttributes.PrivateScope || methodInfo.ReturnType != Typeob.Boolean)
			{
				return LateBinding.DeleteMember(obj, Convert.ToString(array[num - 1]));
			}
			methodInfo = new JSMethodInfo(methodInfo);
			return (bool)methodInfo.Invoke(null, new object[] { obj, array });
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0001031C File Offset: 0x0000F31C
		internal override object Evaluate()
		{
			if (this.outParameterCount > 0 && VsaEngine.executeForJSEE)
			{
				throw new JScriptException(JSError.RefParamsNonSupportedInDebugger);
			}
			LateBinding lateBinding = this.func.EvaluateAsLateBinding();
			object[] array = ((this.args == null) ? null : this.args.EvaluateAsArray());
			base.Globals.CallContextStack.Push(new CallContext(this.context, lateBinding, array));
			object obj2;
			try
			{
				CallableExpression callableExpression = this.func as CallableExpression;
				object obj;
				if (callableExpression == null || !(callableExpression.expression is Call))
				{
					obj = lateBinding.Call(array, this.isConstructor, this.inBrackets, base.Engine);
				}
				else
				{
					obj = LateBinding.CallValue(lateBinding.obj, array, this.isConstructor, this.inBrackets, base.Engine, callableExpression.GetObject2(), JSBinder.ob, null, null);
				}
				if (this.outParameterCount > 0)
				{
					int i = 0;
					int count = this.args.count;
					while (i < count)
					{
						if (this.args[i] is AddressOf)
						{
							this.args[i].SetValue(array[i]);
						}
						i++;
					}
				}
				obj2 = obj;
			}
			catch (TargetInvocationException ex)
			{
				JScriptException ex2;
				if (ex.InnerException is JScriptException)
				{
					ex2 = (JScriptException)ex.InnerException;
					if (ex2.context == null)
					{
						if (ex2.Number == -2146823281)
						{
							ex2.context = this.func.context;
						}
						else
						{
							ex2.context = this.context;
						}
					}
				}
				else
				{
					ex2 = new JScriptException(ex.InnerException, this.context);
				}
				throw ex2;
			}
			catch (JScriptException ex3)
			{
				if (ex3.context == null)
				{
					if (ex3.Number == -2146823281)
					{
						ex3.context = this.func.context;
					}
					else
					{
						ex3.context = this.context;
					}
				}
				throw ex3;
			}
			catch (Exception ex4)
			{
				throw new JScriptException(ex4, this.context);
			}
			catch
			{
				throw new JScriptException(JSError.NonClsException, this.context);
			}
			finally
			{
				base.Globals.CallContextStack.Pop();
			}
			return obj2;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x000105A4 File Offset: 0x0000F5A4
		internal void EvaluateIndices()
		{
			this.argValues = this.args.EvaluateAsArray();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x000105B7 File Offset: 0x0000F5B7
		internal IdentifierLiteral GetName()
		{
			return new IdentifierLiteral(this.func.ToString(), this.func.context);
		}

		// Token: 0x06000213 RID: 531 RVA: 0x000105D4 File Offset: 0x0000F5D4
		internal void GetParameters(ArrayList parameters)
		{
			int i = 0;
			int count = this.args.count;
			while (i < count)
			{
				AST ast = this.args[i];
				parameters.Add(new ParameterDeclaration(ast.context, ast.ToString(), null, null));
				i++;
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x00010620 File Offset: 0x0000F620
		internal override IReflect InferType(JSField inference_target)
		{
			if (this.func is Binding)
			{
				return ((Binding)this.func).InferTypeOfCall(inference_target, this.isConstructor);
			}
			if (this.func is ConstantWrapper)
			{
				object value = ((ConstantWrapper)this.func).value;
				if (value is Type || value is ClassScope || value is TypedArray)
				{
					return (IReflect)value;
				}
			}
			return Typeob.Object;
		}

		// Token: 0x06000215 RID: 533 RVA: 0x00010694 File Offset: 0x0000F694
		private JSLocalField[] LocalsThatWereOutParameters()
		{
			int num = this.outParameterCount;
			if (num == 0)
			{
				return null;
			}
			JSLocalField[] array = new JSLocalField[num];
			int num2 = 0;
			for (int i = 0; i < num; i++)
			{
				AST ast = this.args[i];
				if (ast is AddressOf)
				{
					FieldInfo field = ((AddressOf)ast).GetField();
					if (field is JSLocalField)
					{
						array[num2++] = (JSLocalField)field;
					}
				}
			}
			return array;
		}

		// Token: 0x06000216 RID: 534 RVA: 0x00010700 File Offset: 0x0000F700
		internal void MakeDeletable()
		{
			if (this.func is Binding)
			{
				Binding binding = (Binding)this.func;
				binding.InvalidateBinding();
				binding.PartiallyEvaluateAsCallable();
				binding.ResolveLHValue();
			}
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0001073C File Offset: 0x0000F73C
		internal override AST PartiallyEvaluate()
		{
			if (this.alreadyPartiallyEvaluated)
			{
				return this;
			}
			this.alreadyPartiallyEvaluated = true;
			if (this.inBrackets && this.AllParamsAreMissing())
			{
				if (this.isConstructor)
				{
					this.args.context.HandleError(JSError.TypeMismatch);
				}
				IReflect reflect = ((TypeExpression)new TypeExpression(this.func).PartiallyEvaluate()).ToIReflect();
				return new ConstantWrapper(new TypedArray(reflect, this.args.count + 1), this.context);
			}
			this.func = this.func.PartiallyEvaluateAsCallable();
			this.args = (ASTList)this.args.PartiallyEvaluate();
			IReflect[] array = this.ArgIRs();
			this.func.ResolveCall(this.args, array, this.isConstructor, this.inBrackets);
			if (!this.isConstructor && !this.inBrackets && this.func is Binding && this.args.count == 1)
			{
				Binding binding = (Binding)this.func;
				if (binding.member is Type)
				{
					Type type = (Type)binding.member;
					ConstantWrapper constantWrapper = this.args[0] as ConstantWrapper;
					if (constantWrapper != null)
					{
						try
						{
							if (constantWrapper.value == null || constantWrapper.value is DBNull)
							{
								return this;
							}
							if (constantWrapper.isNumericLiteral && (type == Typeob.Decimal || type == Typeob.Int64 || type == Typeob.UInt64 || type == Typeob.Single))
							{
								return new ConstantWrapper(Convert.CoerceT(constantWrapper.context.GetCode(), type, true), this.context);
							}
							return new ConstantWrapper(Convert.CoerceT(constantWrapper.Evaluate(), type, true), this.context);
						}
						catch
						{
							constantWrapper.context.HandleError(JSError.TypeMismatch);
							return this;
						}
					}
					if (!Binding.AssignmentCompatible(type, this.args[0], array[0], false))
					{
						this.args[0].context.HandleError(JSError.ImpossibleConversion);
					}
				}
				else if (binding.member is JSVariableField)
				{
					JSVariableField jsvariableField = (JSVariableField)binding.member;
					if (jsvariableField.IsLiteral)
					{
						if (jsvariableField.value is ClassScope)
						{
							ClassScope classScope = (ClassScope)jsvariableField.value;
							IReflect underlyingTypeIfEnum = classScope.GetUnderlyingTypeIfEnum();
							if (underlyingTypeIfEnum != null)
							{
								if (!Convert.IsPromotableTo(array[0], underlyingTypeIfEnum) && !Convert.IsPromotableTo(underlyingTypeIfEnum, array[0]) && (array[0] != Typeob.String || underlyingTypeIfEnum == classScope))
								{
									this.args[0].context.HandleError(JSError.ImpossibleConversion);
								}
							}
							else if (!Convert.IsPromotableTo(array[0], classScope) && !Convert.IsPromotableTo(classScope, array[0]))
							{
								this.args[0].context.HandleError(JSError.ImpossibleConversion);
							}
						}
						else if (jsvariableField.value is TypedArray)
						{
							TypedArray typedArray = (TypedArray)jsvariableField.value;
							if (!Convert.IsPromotableTo(array[0], typedArray) && !Convert.IsPromotableTo(typedArray, array[0]))
							{
								this.args[0].context.HandleError(JSError.ImpossibleConversion);
							}
						}
					}
				}
			}
			return this;
		}

		// Token: 0x06000218 RID: 536 RVA: 0x00010AA0 File Offset: 0x0000FAA0
		internal override AST PartiallyEvaluateAsReference()
		{
			this.func = this.func.PartiallyEvaluateAsCallable();
			this.args = (ASTList)this.args.PartiallyEvaluate();
			return this;
		}

		// Token: 0x06000219 RID: 537 RVA: 0x00010ACC File Offset: 0x0000FACC
		internal override void SetPartialValue(AST partial_value)
		{
			if (this.isConstructor)
			{
				this.context.HandleError(JSError.IllegalAssignment);
				return;
			}
			if (this.func is Binding)
			{
				((Binding)this.func).SetPartialValue(this.args, this.ArgIRs(), partial_value, this.inBrackets);
				return;
			}
			if (this.func is ThisLiteral)
			{
				((ThisLiteral)this.func).ResolveAssignmentToDefaultIndexedProperty(this.args, this.ArgIRs(), partial_value);
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x00010B50 File Offset: 0x0000FB50
		internal override void SetValue(object value)
		{
			LateBinding lateBinding = this.func.EvaluateAsLateBinding();
			try
			{
				lateBinding.SetIndexedPropertyValue((this.argValues != null) ? this.argValues : this.args.EvaluateAsArray(), value);
			}
			catch (JScriptException ex)
			{
				if (ex.context == null)
				{
					ex.context = this.func.context;
				}
				throw ex;
			}
			catch (Exception ex2)
			{
				throw new JScriptException(ex2, this.func.context);
			}
			catch
			{
				throw new JScriptException(JSError.NonClsException, this.context);
			}
		}

		// Token: 0x0600021B RID: 539 RVA: 0x00010BF8 File Offset: 0x0000FBF8
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			if (this.context.document.debugOn)
			{
				il.Emit(OpCodes.Nop);
			}
			bool flag = true;
			if (this.enclosingFunctionScope != null && this.enclosingFunctionScope.owner != null)
			{
				Binding binding = this.func as Binding;
				if (binding != null && !this.enclosingFunctionScope.closuresMightEscape)
				{
					if (binding.member is JSLocalField)
					{
						this.enclosingFunctionScope.owner.TranslateToILToSaveLocals(il);
					}
					else
					{
						flag = false;
					}
				}
				else
				{
					this.enclosingFunctionScope.owner.TranslateToILToSaveLocals(il);
				}
			}
			this.func.TranslateToILCall(il, rtype, this.args, this.isConstructor, this.inBrackets);
			if (flag && this.enclosingFunctionScope != null && this.enclosingFunctionScope.owner != null)
			{
				if (this.outParameterCount == 0)
				{
					this.enclosingFunctionScope.owner.TranslateToILToRestoreLocals(il);
					return;
				}
				this.enclosingFunctionScope.owner.TranslateToILToRestoreLocals(il, this.LocalsThatWereOutParameters());
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x00010CF2 File Offset: 0x0000FCF2
		internal CustomAttribute ToCustomAttribute()
		{
			return new CustomAttribute(this.context, this.func, this.args);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x00010D0C File Offset: 0x0000FD0C
		internal override void TranslateToILDelete(ILGenerator il, Type rtype)
		{
			IReflect reflect = this.func.InferType(null);
			Type type = Convert.ToType(reflect);
			this.func.TranslateToIL(il, type);
			this.args.TranslateToIL(il, Typeob.ArrayOfObject);
			if (this.func is Binding)
			{
				MethodInfo methodInfo;
				if (reflect is ClassScope)
				{
					methodInfo = ((ClassScope)reflect).owner.deleteOpMethod;
				}
				else
				{
					methodInfo = reflect.GetMethod("op_Delete", BindingFlags.Static | BindingFlags.Public | BindingFlags.ExactBinding, null, new Type[]
					{
						type,
						Typeob.ArrayOfObject
					}, null);
				}
				if (methodInfo != null && (methodInfo.Attributes & MethodAttributes.SpecialName) != MethodAttributes.PrivateScope && methodInfo.ReturnType == Typeob.Boolean)
				{
					il.Emit(OpCodes.Call, methodInfo);
					Convert.Emit(this, il, Typeob.Boolean, rtype);
					return;
				}
			}
			ConstantWrapper.TranslateToILInt(il, this.args.count - 1);
			il.Emit(OpCodes.Ldelem_Ref);
			Convert.Emit(this, il, Typeob.Object, Typeob.String);
			il.Emit(OpCodes.Call, CompilerGlobals.deleteMemberMethod);
			Convert.Emit(this, il, Typeob.Boolean, rtype);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x00010E1E File Offset: 0x0000FE1E
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.func.TranslateToILInitializer(il);
			this.args.TranslateToILInitializer(il);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x00010E38 File Offset: 0x0000FE38
		internal override void TranslateToILPreSet(ILGenerator il)
		{
			this.func.TranslateToILPreSet(il, this.args);
		}

		// Token: 0x06000220 RID: 544 RVA: 0x00010E4C File Offset: 0x0000FE4C
		internal override void TranslateToILPreSet(ILGenerator il, ASTList args)
		{
			this.isAssignmentToDefaultIndexedProperty = true;
			base.TranslateToILPreSet(il, args);
		}

		// Token: 0x06000221 RID: 545 RVA: 0x00010E5D File Offset: 0x0000FE5D
		internal override void TranslateToILPreSetPlusGet(ILGenerator il)
		{
			this.func.TranslateToILPreSetPlusGet(il, this.args, this.inBrackets);
		}

		// Token: 0x06000222 RID: 546 RVA: 0x00010E77 File Offset: 0x0000FE77
		internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
		{
			if (this.isAssignmentToDefaultIndexedProperty)
			{
				base.TranslateToILSet(il, rhvalue);
				return;
			}
			this.func.TranslateToILSet(il, rhvalue);
		}

		// Token: 0x04000141 RID: 321
		internal AST func;

		// Token: 0x04000142 RID: 322
		private ASTList args;

		// Token: 0x04000143 RID: 323
		private object[] argValues;

		// Token: 0x04000144 RID: 324
		private int outParameterCount;

		// Token: 0x04000145 RID: 325
		internal bool isConstructor;

		// Token: 0x04000146 RID: 326
		internal bool inBrackets;

		// Token: 0x04000147 RID: 327
		private FunctionScope enclosingFunctionScope;

		// Token: 0x04000148 RID: 328
		private bool alreadyPartiallyEvaluated;

		// Token: 0x04000149 RID: 329
		private bool isAssignmentToDefaultIndexedProperty;
	}
}
