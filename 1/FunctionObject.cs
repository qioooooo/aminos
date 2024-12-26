using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000084 RID: 132
	public sealed class FunctionObject : ScriptFunction
	{
		// Token: 0x060005D8 RID: 1496 RVA: 0x00029804 File Offset: 0x00028804
		internal FunctionObject(string name, ParameterDeclaration[] parameter_declarations, TypeExpression return_type_expr, Block body, FunctionScope own_scope, ScriptObject enclosing_scope, Context funcContext, MethodAttributes attributes)
			: this(name, parameter_declarations, return_type_expr, body, own_scope, enclosing_scope, funcContext, attributes, null, false)
		{
		}

		// Token: 0x060005D9 RID: 1497 RVA: 0x00029828 File Offset: 0x00028828
		internal FunctionObject(string name, ParameterDeclaration[] parameter_declarations, TypeExpression return_type_expr, Block body, FunctionScope own_scope, ScriptObject enclosing_scope, Context funcContext, MethodAttributes attributes, CustomAttributeList customAttributes, bool isMethod)
			: base(body.Globals.globalObject.originalFunction.originalPrototype, name, parameter_declarations.Length)
		{
			this.parameter_declarations = parameter_declarations;
			int num = parameter_declarations.Length;
			this.formal_parameters = new string[num];
			for (int i = 0; i < num; i++)
			{
				this.formal_parameters[i] = parameter_declarations[i].identifier;
			}
			this.argumentsSlotNumber = 0;
			this.return_type_expr = return_type_expr;
			if (this.return_type_expr != null)
			{
				own_scope.AddReturnValueField();
			}
			this.body = body;
			this.method = null;
			this.parameterInfos = null;
			this.funcContext = funcContext;
			this.own_scope = own_scope;
			this.own_scope.owner = this;
			if ((!(enclosing_scope is ActivationObject) || !((ActivationObject)enclosing_scope).fast) && !isMethod)
			{
				this.argumentsSlotNumber = this.own_scope.GetNextSlotNumber();
				JSLocalField jslocalField = (JSLocalField)this.own_scope.AddNewField("arguments", null, FieldAttributes.PrivateScope);
				jslocalField.type = new TypeExpression(new ConstantWrapper(Typeob.Object, funcContext));
				jslocalField.isDefined = true;
				this.hasArgumentsObject = true;
			}
			else
			{
				this.hasArgumentsObject = false;
			}
			this.implementedIface = null;
			this.implementedIfaceMethod = null;
			this.isMethod = isMethod;
			this.isExpandoMethod = customAttributes != null && customAttributes.ContainsExpandoAttribute();
			this.isStatic = (this.own_scope.isStatic = (attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope);
			this.suppressIL = false;
			this.noVersionSafeAttributeSpecified = true;
			this.fields = this.own_scope.GetLocalFields();
			this.enclosing_scope = enclosing_scope;
			this.must_save_stack_locals = false;
			this.text = null;
			this.mb = null;
			this.cb = null;
			this.attributes = attributes;
			if (!this.isStatic)
			{
				this.attributes |= MethodAttributes.HideBySig;
			}
			this.globals = body.Globals;
			this.superConstructor = null;
			this.superConstructorCall = null;
			this.customAttributes = customAttributes;
			this.noExpando = false;
			this.clsCompliance = CLSComplianceSpec.NotAttributed;
			this.engineLocal = null;
			this.partiallyEvaluated = false;
		}

		// Token: 0x060005DA RID: 1498 RVA: 0x00029A38 File Offset: 0x00028A38
		internal FunctionObject(Type t, string name, string method_name, string[] formal_parameters, JSLocalField[] fields, bool must_save_stack_locals, bool hasArgumentsObject, string text, VsaEngine engine)
			: base(engine.Globals.globalObject.originalFunction.originalPrototype, name, formal_parameters.Length)
		{
			this.engine = engine;
			this.formal_parameters = formal_parameters;
			this.argumentsSlotNumber = 0;
			this.body = null;
			TypeReflector typeReflectorFor = TypeReflector.GetTypeReflectorFor(Globals.TypeRefs.ToReferenceContext(t));
			this.method = typeReflectorFor.GetMethod(method_name, BindingFlags.Static | BindingFlags.Public);
			this.parameterInfos = this.method.GetParameters();
			if (!CustomAttribute.IsDefined(this.method, typeof(JSFunctionAttribute), false))
			{
				this.isMethod = true;
			}
			else
			{
				object[] array = CustomAttribute.GetCustomAttributes(this.method, typeof(JSFunctionAttribute), false);
				JSFunctionAttributeEnum attributeValue = ((JSFunctionAttribute)array[0]).attributeValue;
				this.isExpandoMethod = (attributeValue & JSFunctionAttributeEnum.IsExpandoMethod) != JSFunctionAttributeEnum.None;
			}
			this.funcContext = null;
			this.own_scope = null;
			this.fields = fields;
			this.must_save_stack_locals = must_save_stack_locals;
			this.hasArgumentsObject = hasArgumentsObject;
			this.text = text;
			this.attributes = MethodAttributes.Public;
			this.globals = engine.Globals;
			this.superConstructor = null;
			this.superConstructorCall = null;
			this.enclosing_scope = this.globals.ScopeStack.Peek();
			this.noExpando = false;
			this.clsCompliance = CLSComplianceSpec.NotAttributed;
		}

		// Token: 0x060005DB RID: 1499 RVA: 0x00029B7F File Offset: 0x00028B7F
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object Call(object[] args, object thisob)
		{
			return this.Call(args, thisob, null, null);
		}

		// Token: 0x060005DC RID: 1500 RVA: 0x00029B8C File Offset: 0x00028B8C
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object Call(object[] args, object thisob, Binder binder, CultureInfo culture)
		{
			if (this.body == null)
			{
				return this.Call(args, thisob, this.enclosing_scope, new Closure(this), binder, culture);
			}
			StackFrame stackFrame = new StackFrame((thisob is JSObject) ? ((JSObject)thisob) : this.enclosing_scope, this.fields, new object[this.fields.Length], thisob);
			if (this.isConstructor)
			{
				stackFrame.closureInstance = thisob;
				if (this.superConstructor != null)
				{
					if (this.superConstructorCall == null)
					{
						if (this.superConstructor is JSConstructor)
						{
							this.superConstructor.Invoke(thisob, new object[0]);
						}
					}
					else
					{
						ASTList arguments = this.superConstructorCall.arguments;
						int count = arguments.count;
						object[] array = new object[count];
						for (int i = 0; i < count; i++)
						{
							array[i] = arguments[i].Evaluate();
						}
						this.superConstructor.Invoke(thisob, BindingFlags.Default, binder, array, culture);
					}
				}
				this.globals.ScopeStack.GuardedPush((thisob is JSObject) ? ((JSObject)thisob) : this.enclosing_scope);
				try
				{
					((ClassScope)this.enclosing_scope).owner.body.EvaluateInstanceVariableInitializers();
					goto IL_0169;
				}
				finally
				{
					this.globals.ScopeStack.Pop();
				}
			}
			if (this.isMethod && !this.isStatic)
			{
				if (!((ClassScope)this.enclosing_scope).HasInstance(thisob))
				{
					throw new JScriptException(JSError.TypeMismatch);
				}
				stackFrame.closureInstance = thisob;
			}
			IL_0169:
			this.globals.ScopeStack.GuardedPush(stackFrame);
			object obj;
			try
			{
				this.own_scope.CloseNestedFunctions(stackFrame);
				this.ConvertArguments(args, stackFrame.localVars, 0, args.Length, this.formal_parameters.Length, binder, culture);
				Completion completion = (Completion)this.body.Evaluate();
				if (completion.Return)
				{
					obj = completion.value;
				}
				else
				{
					obj = null;
				}
			}
			finally
			{
				this.globals.ScopeStack.Pop();
			}
			return obj;
		}

		// Token: 0x060005DD RID: 1501 RVA: 0x00029D98 File Offset: 0x00028D98
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object Call(object[] args, object thisob, ScriptObject enclosing_scope, Closure calleeClosure, Binder binder, CultureInfo culture)
		{
			if (this.body != null)
			{
				return this.CallASTFunc(args, thisob, enclosing_scope, calleeClosure, binder, culture);
			}
			object caller = calleeClosure.caller;
			calleeClosure.caller = this.globals.caller;
			this.globals.caller = calleeClosure;
			object arguments = calleeClosure.arguments;
			ScriptObject scriptObject = this.globals.ScopeStack.Peek();
			ArgumentsObject argumentsObject = ((scriptObject is StackFrame) ? ((StackFrame)scriptObject).caller_arguments : null);
			StackFrame stackFrame = new StackFrame(enclosing_scope, this.fields, this.must_save_stack_locals ? new object[this.fields.Length] : null, thisob);
			this.globals.ScopeStack.GuardedPush(stackFrame);
			ArgumentsObject argumentsObject2 = new ArgumentsObject(this.globals.globalObject.originalObjectPrototype, args, this, calleeClosure, stackFrame, argumentsObject);
			stackFrame.caller_arguments = argumentsObject2;
			calleeClosure.arguments = argumentsObject2;
			object obj;
			try
			{
				int num = this.formal_parameters.Length;
				int num2 = args.Length;
				if (this.hasArgumentsObject)
				{
					object[] array = new object[num + 3];
					array[0] = thisob;
					array[1] = this.engine;
					array[2] = argumentsObject2;
					this.ConvertArguments(args, array, 3, num2, num, binder, culture);
					obj = this.method.Invoke(thisob, BindingFlags.SuppressChangeType, null, array, null);
				}
				else if (!this.isMethod)
				{
					object[] array2 = new object[num + 2];
					array2[0] = thisob;
					array2[1] = this.engine;
					this.ConvertArguments(args, array2, 2, num2, num, binder, culture);
					obj = this.method.Invoke(thisob, BindingFlags.SuppressChangeType, null, array2, null);
				}
				else if (num == num2)
				{
					this.ConvertArguments(args, args, 0, num2, num, binder, culture);
					obj = this.method.Invoke(thisob, BindingFlags.SuppressChangeType, null, args, null);
				}
				else
				{
					object[] array3 = new object[num];
					this.ConvertArguments(args, array3, 0, num2, num, binder, culture);
					obj = this.method.Invoke(thisob, BindingFlags.SuppressChangeType, null, array3, null);
				}
			}
			catch (TargetInvocationException ex)
			{
				throw ex.InnerException;
			}
			finally
			{
				this.globals.ScopeStack.Pop();
				calleeClosure.arguments = arguments;
				this.globals.caller = calleeClosure.caller;
				calleeClosure.caller = caller;
			}
			return obj;
		}

		// Token: 0x060005DE RID: 1502 RVA: 0x0002A00C File Offset: 0x0002900C
		private object CallASTFunc(object[] args, object thisob, ScriptObject enclosing_scope, Closure calleeClosure, Binder binder, CultureInfo culture)
		{
			object caller = calleeClosure.caller;
			calleeClosure.caller = this.globals.caller;
			this.globals.caller = calleeClosure;
			object arguments = calleeClosure.arguments;
			ScriptObject scriptObject = this.globals.ScopeStack.Peek();
			ArgumentsObject argumentsObject = ((scriptObject is StackFrame) ? ((StackFrame)scriptObject).caller_arguments : null);
			StackFrame stackFrame = new StackFrame(enclosing_scope, this.fields, new object[this.fields.Length], thisob);
			if (this.isMethod && !this.isStatic)
			{
				stackFrame.closureInstance = thisob;
			}
			this.globals.ScopeStack.GuardedPush(stackFrame);
			object obj;
			try
			{
				this.own_scope.CloseNestedFunctions(stackFrame);
				ArgumentsObject argumentsObject2 = null;
				if (this.hasArgumentsObject)
				{
					argumentsObject2 = new ArgumentsObject(this.globals.globalObject.originalObjectPrototype, args, this, calleeClosure, stackFrame, argumentsObject);
					stackFrame.localVars[this.argumentsSlotNumber] = argumentsObject2;
				}
				stackFrame.caller_arguments = argumentsObject2;
				calleeClosure.arguments = argumentsObject2;
				this.ConvertArguments(args, stackFrame.localVars, 0, args.Length, this.formal_parameters.Length, binder, culture);
				Completion completion = (Completion)this.body.Evaluate();
				if (completion.Return)
				{
					obj = completion.value;
				}
				else
				{
					obj = null;
				}
			}
			finally
			{
				this.globals.ScopeStack.Pop();
				calleeClosure.arguments = arguments;
				this.globals.caller = calleeClosure.caller;
				calleeClosure.caller = caller;
			}
			return obj;
		}

		// Token: 0x060005DF RID: 1503 RVA: 0x0002A1A0 File Offset: 0x000291A0
		internal void CheckCLSCompliance(bool classIsCLSCompliant)
		{
			if (classIsCLSCompliant)
			{
				if (this.clsCompliance != CLSComplianceSpec.NonCLSCompliant)
				{
					int i = 0;
					int num = this.parameter_declarations.Length;
					while (i < num)
					{
						IReflect parameterIReflect = this.parameter_declarations[i].ParameterIReflect;
						if (parameterIReflect != null && !TypeExpression.TypeIsCLSCompliant(parameterIReflect))
						{
							this.clsCompliance = CLSComplianceSpec.NonCLSCompliant;
							this.funcContext.HandleError(JSError.NonCLSCompliantMember);
							return;
						}
						i++;
					}
					if (this.return_type_expr != null && !this.return_type_expr.IsCLSCompliant())
					{
						this.clsCompliance = CLSComplianceSpec.NonCLSCompliant;
						this.funcContext.HandleError(JSError.NonCLSCompliantMember);
						return;
					}
				}
			}
			else if (this.clsCompliance == CLSComplianceSpec.CLSCompliant)
			{
				this.funcContext.HandleError(JSError.MemberTypeCLSCompliantMismatch);
			}
		}

		// Token: 0x060005E0 RID: 1504 RVA: 0x0002A248 File Offset: 0x00029248
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal object Construct(JSObject thisob, object[] args)
		{
			JSObject jsobject = new JSObject(null, false);
			jsobject.SetParent(base.GetPrototypeForConstructedObject());
			jsobject.outer_class_instance = thisob;
			object obj = this.Call(args, jsobject);
			if (obj is ScriptObject)
			{
				return obj;
			}
			return jsobject;
		}

		// Token: 0x060005E1 RID: 1505 RVA: 0x0002A284 File Offset: 0x00029284
		private void ConvertArguments(object[] args, object[] newargs, int offset, int length, int n, Binder binder, CultureInfo culture)
		{
			ParameterInfo[] array = this.parameterInfos;
			if (array != null)
			{
				int i = 0;
				int num = offset;
				while (i < n)
				{
					Type parameterType = array[num].ParameterType;
					if (i == n - 1 && CustomAttribute.IsDefined(array[num], typeof(ParamArrayAttribute), false))
					{
						int num2 = length - i;
						if (num2 < 0)
						{
							num2 = 0;
						}
						newargs[num] = FunctionObject.CopyToNewParamArray(parameterType.GetElementType(), num2, args, i, binder, culture);
						return;
					}
					object obj = ((i < length) ? args[i] : null);
					if (parameterType == Typeob.Object)
					{
						newargs[num] = obj;
					}
					else if (binder != null)
					{
						newargs[num] = binder.ChangeType(obj, parameterType, culture);
					}
					else
					{
						newargs[num] = Convert.CoerceT(obj, parameterType);
					}
					i++;
					num++;
				}
				return;
			}
			ParameterDeclaration[] array2 = this.parameter_declarations;
			int j = 0;
			int num3 = offset;
			while (j < n)
			{
				IReflect parameterIReflect = array2[j].ParameterIReflect;
				if (j == n - 1 && CustomAttribute.IsDefined(array2[num3], typeof(ParamArrayAttribute), false))
				{
					int num4 = length - j;
					if (num4 < 0)
					{
						num4 = 0;
					}
					newargs[num3] = FunctionObject.CopyToNewParamArray(((TypedArray)parameterIReflect).elementType, num4, args, j);
					return;
				}
				object obj2 = ((j < length) ? args[j] : null);
				if (parameterIReflect == Typeob.Object)
				{
					newargs[num3] = obj2;
				}
				else if (parameterIReflect is ClassScope)
				{
					newargs[num3] = Convert.Coerce(obj2, parameterIReflect);
				}
				else if (binder != null)
				{
					newargs[num3] = binder.ChangeType(obj2, Convert.ToType(parameterIReflect), culture);
				}
				else
				{
					newargs[num3] = Convert.CoerceT(obj2, Convert.ToType(parameterIReflect));
				}
				j++;
				num3++;
			}
		}

		// Token: 0x060005E2 RID: 1506 RVA: 0x0002A424 File Offset: 0x00029424
		private static object[] CopyToNewParamArray(IReflect ir, int n, object[] args, int offset)
		{
			object[] array = new object[n];
			for (int i = 0; i < n; i++)
			{
				array[i] = Convert.Coerce(args[i + offset], ir);
			}
			return array;
		}

		// Token: 0x060005E3 RID: 1507 RVA: 0x0002A454 File Offset: 0x00029454
		private static Array CopyToNewParamArray(Type t, int n, object[] args, int offset, Binder binder, CultureInfo culture)
		{
			Array array = Array.CreateInstance(t, n);
			for (int i = 0; i < n; i++)
			{
				array.SetValue(binder.ChangeType(args[i + offset], t, culture), i);
			}
			return array;
		}

		// Token: 0x060005E4 RID: 1508 RVA: 0x0002A48C File Offset: 0x0002948C
		internal void EmitLastLineInfo(ILGenerator il)
		{
			if (!this.isImplicitCtor)
			{
				int endLine = this.body.context.EndLine;
				int endColumn = this.body.context.EndColumn;
				this.body.context.document.EmitLineInfo(il, endLine, endColumn, endLine, endColumn + 1);
			}
		}

		// Token: 0x060005E5 RID: 1509 RVA: 0x0002A4DF File Offset: 0x000294DF
		internal string GetName()
		{
			return this.name;
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0002A4E7 File Offset: 0x000294E7
		internal override int GetNumberOfFormalParameters()
		{
			return this.formal_parameters.Length;
		}

		// Token: 0x060005E7 RID: 1511 RVA: 0x0002A4F1 File Offset: 0x000294F1
		internal ConstructorInfo GetConstructorInfo(CompilerGlobals compilerGlobals)
		{
			return (ConstructorInfo)this.GetMethodBase(compilerGlobals);
		}

		// Token: 0x060005E8 RID: 1512 RVA: 0x0002A4FF File Offset: 0x000294FF
		internal MethodInfo GetMethodInfo(CompilerGlobals compilerGlobals)
		{
			return (MethodInfo)this.GetMethodBase(compilerGlobals);
		}

		// Token: 0x060005E9 RID: 1513 RVA: 0x0002A510 File Offset: 0x00029510
		internal MethodBase GetMethodBase(CompilerGlobals compilerGlobals)
		{
			if (this.mb != null)
			{
				return this.mb;
			}
			if (this.cb != null)
			{
				return this.cb;
			}
			JSFunctionAttributeEnum jsfunctionAttributeEnum = JSFunctionAttributeEnum.None;
			int num = 3;
			if (this.isMethod)
			{
				if (this.isConstructor && ((ClassScope)this.enclosing_scope).outerClassField != null)
				{
					num = 1;
					jsfunctionAttributeEnum |= JSFunctionAttributeEnum.IsInstanceNestedClassConstructor;
				}
				else
				{
					num = 0;
				}
			}
			else if (!this.hasArgumentsObject)
			{
				num = 2;
			}
			int num2 = this.formal_parameters.Length + num;
			Type[] array = new Type[num2];
			Type type = Convert.ToType(this.ReturnType(null));
			if (num > 0)
			{
				if (this.isConstructor)
				{
					array[num2 - 1] = ((ClassScope)this.enclosing_scope).outerClassField.FieldType;
				}
				else
				{
					array[0] = Typeob.Object;
				}
				jsfunctionAttributeEnum |= JSFunctionAttributeEnum.HasThisObject;
			}
			if (num > 1)
			{
				array[1] = Typeob.VsaEngine;
				jsfunctionAttributeEnum |= JSFunctionAttributeEnum.HasEngine;
			}
			if (num > 2)
			{
				array[2] = Typeob.Object;
				jsfunctionAttributeEnum |= JSFunctionAttributeEnum.HasArguments;
			}
			if (this.must_save_stack_locals)
			{
				jsfunctionAttributeEnum |= JSFunctionAttributeEnum.HasStackFrame;
			}
			if (this.isExpandoMethod)
			{
				jsfunctionAttributeEnum |= JSFunctionAttributeEnum.IsExpandoMethod;
			}
			if (this.isConstructor)
			{
				for (int i = 0; i < num2 - num; i++)
				{
					array[i] = this.parameter_declarations[i].ParameterType;
				}
			}
			else
			{
				for (int j = num; j < num2; j++)
				{
					array[j] = this.parameter_declarations[j - num].ParameterType;
				}
			}
			if (this.enclosing_scope is ClassScope)
			{
				if (this.isConstructor)
				{
					this.cb = ((ClassScope)this.enclosing_scope).GetTypeBuilder().DefineConstructor(this.attributes & MethodAttributes.MemberAccessMask, CallingConventions.Standard, array);
				}
				else
				{
					string text = this.name;
					if (this.implementedIfaceMethod != null)
					{
						JSMethod jsmethod = this.implementedIfaceMethod as JSMethod;
						if (jsmethod != null)
						{
							this.implementedIfaceMethod = jsmethod.GetMethodInfo(compilerGlobals);
						}
						text = this.implementedIfaceMethod.DeclaringType.FullName + "." + text;
					}
					TypeBuilder typeBuilder = ((ClassScope)this.enclosing_scope).GetTypeBuilder();
					if (this.mb != null)
					{
						return this.mb;
					}
					this.mb = typeBuilder.DefineMethod(text, this.attributes, type, array);
					if (this.implementedIfaceMethod != null)
					{
						((ClassScope)this.enclosing_scope).GetTypeBuilder().DefineMethodOverride(this.mb, this.implementedIfaceMethod);
					}
				}
			}
			else
			{
				if (this.enclosing_scope is FunctionScope)
				{
					if (((FunctionScope)this.enclosing_scope).owner != null)
					{
						this.name = ((FunctionScope)this.enclosing_scope).owner.name + "." + this.name;
						jsfunctionAttributeEnum |= JSFunctionAttributeEnum.IsNested;
					}
					else
					{
						for (ScriptObject parent = this.enclosing_scope; parent != null; parent = parent.GetParent())
						{
							if (parent is FunctionScope && ((FunctionScope)parent).owner != null)
							{
								this.name = ((FunctionScope)parent).owner.name + "." + this.name;
								jsfunctionAttributeEnum |= JSFunctionAttributeEnum.IsNested;
								break;
							}
						}
					}
				}
				if (compilerGlobals.usedNames[this.name] != null)
				{
					this.name = this.name + ":" + compilerGlobals.usedNames.count.ToString(CultureInfo.InvariantCulture);
				}
				compilerGlobals.usedNames[this.name] = this;
				ScriptObject parent2 = this.enclosing_scope;
				while (parent2 != null && !(parent2 is ClassScope))
				{
					parent2 = parent2.GetParent();
				}
				this.classwriter = ((parent2 == null) ? compilerGlobals.globalScopeClassWriter : compilerGlobals.classwriter);
				this.mb = this.classwriter.DefineMethod(this.name, this.attributes, type, array);
			}
			if (num > 0)
			{
				if (this.mb != null)
				{
					this.mb.DefineParameter(1, ParameterAttributes.None, "this");
				}
				else
				{
					ParameterBuilder parameterBuilder = this.cb.DefineParameter(num2, ParameterAttributes.None, "this");
					parameterBuilder.SetConstant(null);
					num = 0;
					num2--;
				}
			}
			if (num > 1)
			{
				this.mb.DefineParameter(2, ParameterAttributes.None, "vsa Engine");
			}
			if (num > 2)
			{
				this.mb.DefineParameter(3, ParameterAttributes.None, "arguments");
			}
			for (int k = num; k < num2; k++)
			{
				ParameterBuilder parameterBuilder2 = ((this.mb != null) ? this.mb.DefineParameter(k + 1, ParameterAttributes.None, this.parameter_declarations[k - num].identifier) : this.cb.DefineParameter(k + 1, ParameterAttributes.None, this.parameter_declarations[k - num].identifier));
				CustomAttributeList customAttributeList = this.parameter_declarations[k - num].customAttributes;
				if (customAttributeList != null)
				{
					CustomAttributeBuilder[] customAttributeBuilders = customAttributeList.GetCustomAttributeBuilders(false);
					for (int l = 0; l < customAttributeBuilders.Length; l++)
					{
						parameterBuilder2.SetCustomAttribute(customAttributeBuilders[l]);
					}
				}
			}
			if (jsfunctionAttributeEnum > JSFunctionAttributeEnum.None)
			{
				CustomAttributeBuilder customAttributeBuilder = new CustomAttributeBuilder(CompilerGlobals.jsFunctionAttributeConstructor, new object[] { jsfunctionAttributeEnum });
				if (this.mb != null)
				{
					this.mb.SetCustomAttribute(customAttributeBuilder);
				}
				else
				{
					this.cb.SetCustomAttribute(customAttributeBuilder);
				}
			}
			if (this.customAttributes != null)
			{
				CustomAttributeBuilder[] customAttributeBuilders2 = this.customAttributes.GetCustomAttributeBuilders(false);
				for (int m = 0; m < customAttributeBuilders2.Length; m++)
				{
					if (this.mb != null)
					{
						this.mb.SetCustomAttribute(customAttributeBuilders2[m]);
					}
					else
					{
						this.cb.SetCustomAttribute(customAttributeBuilders2[m]);
					}
				}
			}
			if (this.clsCompliance == CLSComplianceSpec.CLSCompliant)
			{
				if (this.mb != null)
				{
					this.mb.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[] { true }));
				}
				else
				{
					this.cb.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[] { true }));
				}
			}
			else if (this.clsCompliance == CLSComplianceSpec.NonCLSCompliant)
			{
				if (this.mb != null)
				{
					this.mb.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[] { false }));
				}
				else
				{
					this.cb.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[] { false }));
				}
			}
			if (this.mb != null)
			{
				this.mb.InitLocals = true;
				return this.mb;
			}
			this.cb.InitLocals = true;
			return this.cb;
		}

		// Token: 0x060005EA RID: 1514 RVA: 0x0002AB54 File Offset: 0x00029B54
		private static bool IsPresentIn(FieldInfo field, FieldInfo[] fields)
		{
			int i = 0;
			int num = fields.Length;
			while (i < num)
			{
				if (field == fields[i])
				{
					return true;
				}
				i++;
			}
			return false;
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060005EB RID: 1515 RVA: 0x0002AB7A File Offset: 0x00029B7A
		internal bool Must_save_stack_locals
		{
			get
			{
				if (!this.partiallyEvaluated)
				{
					this.PartiallyEvaluate();
				}
				return this.must_save_stack_locals;
			}
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0002AB90 File Offset: 0x00029B90
		internal void PartiallyEvaluate()
		{
			if (this.partiallyEvaluated)
			{
				return;
			}
			ClassScope classScope = this.enclosing_scope as ClassScope;
			if (classScope != null)
			{
				classScope.owner.PartiallyEvaluate();
			}
			if (this.partiallyEvaluated)
			{
				return;
			}
			this.partiallyEvaluated = true;
			this.clsCompliance = CLSComplianceSpec.NotAttributed;
			if (this.customAttributes != null)
			{
				this.customAttributes.PartiallyEvaluate();
				CustomAttribute customAttribute = this.customAttributes.GetAttribute(Typeob.CLSCompliantAttribute);
				if (customAttribute != null)
				{
					this.clsCompliance = customAttribute.GetCLSComplianceValue();
					this.customAttributes.Remove(customAttribute);
				}
				customAttribute = this.customAttributes.GetAttribute(Typeob.Override);
				if (customAttribute != null)
				{
					if (this.isStatic)
					{
						customAttribute.context.HandleError(JSError.StaticMethodsCannotOverride);
					}
					else
					{
						this.attributes &= ~MethodAttributes.VtableLayoutMask;
					}
					this.noVersionSafeAttributeSpecified = false;
					this.customAttributes.Remove(customAttribute);
				}
				customAttribute = this.customAttributes.GetAttribute(Typeob.Hide);
				if (customAttribute != null)
				{
					if (!this.noVersionSafeAttributeSpecified)
					{
						customAttribute.context.HandleError(JSError.OverrideAndHideUsedTogether);
						this.attributes |= MethodAttributes.VtableLayoutMask;
						this.noVersionSafeAttributeSpecified = true;
					}
					else
					{
						if (this.isStatic)
						{
							customAttribute.context.HandleError(JSError.StaticMethodsCannotHide);
						}
						this.noVersionSafeAttributeSpecified = false;
					}
					this.customAttributes.Remove(customAttribute);
				}
				CustomAttribute attribute = this.customAttributes.GetAttribute(Typeob.Expando);
				if (attribute != null)
				{
					if (!this.noVersionSafeAttributeSpecified && (this.attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope)
					{
						attribute.context.HandleError(JSError.ExpandoPrecludesOverride);
						this.attributes |= MethodAttributes.VtableLayoutMask;
						this.noVersionSafeAttributeSpecified = true;
					}
					if (this.isConstructor)
					{
						attribute.context.HandleError(JSError.NotValidForConstructor);
					}
					else if ((this.attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
					{
						attribute.context.HandleError(JSError.ExpandoPrecludesAbstract);
					}
					else if ((this.attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
					{
						attribute.context.HandleError(JSError.ExpandoPrecludesStatic);
					}
					else if ((this.attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.Public)
					{
						attribute.context.HandleError(JSError.ExpandoMustBePublic);
					}
					else
					{
						this.own_scope.isMethod = false;
						this.isMethod = false;
						this.isExpandoMethod = true;
						this.isStatic = true;
						this.attributes &= ~MethodAttributes.Virtual;
						this.attributes &= ~MethodAttributes.VtableLayoutMask;
						this.attributes |= MethodAttributes.Static;
					}
				}
			}
			int i = 0;
			int num = this.parameter_declarations.Length;
			while (i < num)
			{
				this.parameter_declarations[i].PartiallyEvaluate();
				JSLocalField jslocalField = (JSLocalField)this.own_scope.name_table[this.formal_parameters[i]];
				jslocalField.type = this.parameter_declarations[i].type;
				if (jslocalField.type == null)
				{
					jslocalField.type = new TypeExpression(new ConstantWrapper(Typeob.Object, this.parameter_declarations[i].context));
				}
				jslocalField.isDefined = true;
				i++;
			}
			if (this.return_type_expr != null)
			{
				this.return_type_expr.PartiallyEvaluate();
				this.own_scope.returnVar.type = this.return_type_expr;
				if (this.own_scope.returnVar.type.ToIReflect() == Typeob.Void)
				{
					this.own_scope.returnVar.type = null;
					this.own_scope.returnVar = null;
				}
			}
			this.globals.ScopeStack.Push(this.own_scope);
			if (!this.own_scope.isKnownAtCompileTime)
			{
				int j = 0;
				int num2 = this.fields.Length;
				while (j < num2)
				{
					this.fields[j].SetInferredType(Typeob.Object, null);
					j++;
				}
			}
			if (!this.isConstructor)
			{
				this.body.PartiallyEvaluate();
			}
			else
			{
				this.body.MarkSuperOKIfIsFirstStatement();
				this.body.PartiallyEvaluate();
				ClassScope classScope2 = (ClassScope)this.enclosing_scope;
				int num3 = ((this.superConstructorCall == null) ? 0 : this.superConstructorCall.arguments.count);
				if (num3 == 0)
				{
					Type[] emptyTypes = Type.EmptyTypes;
				}
				IReflect[] array = new IReflect[num3];
				for (int k = 0; k < num3; k++)
				{
					array[k] = this.superConstructorCall.arguments[k].InferType(null);
				}
				Context context = ((this.superConstructorCall == null) ? this.funcContext : this.superConstructorCall.context);
				try
				{
					if (this.superConstructorCall != null && !this.superConstructorCall.isSuperConstructorCall)
					{
						this.superConstructor = JSBinder.SelectConstructor(classScope2.constructors, array);
					}
					else
					{
						this.superConstructor = classScope2.owner.GetSuperConstructor(array);
					}
					if (this.superConstructor == null)
					{
						context.HandleError(JSError.SuperClassConstructorNotAccessible);
					}
					else
					{
						ConstructorInfo constructorInfo = this.superConstructor;
						if (!constructorInfo.IsPublic && !constructorInfo.IsFamily && !constructorInfo.IsFamilyOrAssembly && (!(this.superConstructor is JSConstructor) || !((JSConstructor)this.superConstructor).IsAccessibleFrom(this.enclosing_scope)))
						{
							context.HandleError(JSError.SuperClassConstructorNotAccessible);
							this.superConstructor = null;
						}
						else if (num3 > 0 && !Binding.CheckParameters(constructorInfo.GetParameters(), array, this.superConstructorCall.arguments, this.superConstructorCall.context))
						{
							this.superConstructor = null;
						}
					}
				}
				catch (AmbiguousMatchException)
				{
					context.HandleError(JSError.AmbiguousConstructorCall);
				}
			}
			this.own_scope.HandleUnitializedVariables();
			this.globals.ScopeStack.Pop();
			this.must_save_stack_locals = this.own_scope.mustSaveStackLocals;
			this.fields = this.own_scope.GetLocalFields();
		}

		// Token: 0x060005ED RID: 1517 RVA: 0x0002B140 File Offset: 0x0002A140
		internal IReflect ReturnType(JSField inference_target)
		{
			if (!this.partiallyEvaluated)
			{
				this.PartiallyEvaluate();
			}
			if (this.own_scope.returnVar == null)
			{
				return Typeob.Void;
			}
			if (this.return_type_expr != null)
			{
				return this.return_type_expr.ToIReflect();
			}
			return this.own_scope.returnVar.GetInferredType(inference_target);
		}

		// Token: 0x060005EE RID: 1518 RVA: 0x0002B193 File Offset: 0x0002A193
		public override string ToString()
		{
			if (this.text != null)
			{
				return this.text;
			}
			return this.funcContext.GetCode();
		}

		// Token: 0x060005EF RID: 1519 RVA: 0x0002B1B0 File Offset: 0x0002A1B0
		internal void TranslateBodyToIL(ILGenerator il, CompilerGlobals compilerGlobals)
		{
			this.returnLabel = il.DefineLabel();
			if (this.body.Engine.GenerateDebugInfo)
			{
				for (ScriptObject scriptObject = this.enclosing_scope.GetParent(); scriptObject != null; scriptObject = scriptObject.GetParent())
				{
					if (scriptObject is PackageScope)
					{
						il.UsingNamespace(((PackageScope)scriptObject).name);
					}
					else if (scriptObject is WrappedNamespace && !((WrappedNamespace)scriptObject).name.Equals(""))
					{
						il.UsingNamespace(((WrappedNamespace)scriptObject).name);
					}
				}
			}
			int startLine = this.body.context.StartLine;
			int startColumn = this.body.context.StartColumn;
			this.body.context.document.EmitLineInfo(il, startLine, startColumn, startLine, startColumn + 1);
			if (this.body.context.document.debugOn)
			{
				il.Emit(OpCodes.Nop);
			}
			int num = this.fields.Length;
			for (int i = 0; i < num; i++)
			{
				if (!this.fields[i].IsLiteral || this.fields[i].value is FunctionObject)
				{
					Type fieldType = this.fields[i].FieldType;
					LocalBuilder localBuilder = il.DeclareLocal(fieldType);
					if (this.fields[i].debugOn)
					{
						localBuilder.SetLocalSymInfo(this.fields[i].debuggerName);
					}
					this.fields[i].metaData = localBuilder;
				}
			}
			this.globals.ScopeStack.Push(this.own_scope);
			try
			{
				if (this.must_save_stack_locals)
				{
					this.TranslateToMethodWithStackFrame(il, compilerGlobals, true);
				}
				else
				{
					this.body.TranslateToILInitializer(il);
					this.body.TranslateToIL(il, Typeob.Void);
					il.MarkLabel(this.returnLabel);
				}
			}
			finally
			{
				this.globals.ScopeStack.Pop();
			}
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x0002B3A4 File Offset: 0x0002A3A4
		internal void TranslateToIL(CompilerGlobals compilerGlobals)
		{
			if (this.suppressIL)
			{
				return;
			}
			this.globals.ScopeStack.Push(this.own_scope);
			try
			{
				if (this.mb == null && this.cb == null)
				{
					this.GetMethodBase(compilerGlobals);
				}
				int num = (((this.attributes & MethodAttributes.Static) == MethodAttributes.Static) ? 0 : 1);
				int num2 = 3;
				if (this.isMethod)
				{
					num2 = 0;
				}
				else if (!this.hasArgumentsObject)
				{
					num2 = 2;
				}
				ILGenerator ilgenerator = ((this.mb != null) ? this.mb.GetILGenerator() : this.cb.GetILGenerator());
				this.returnLabel = ilgenerator.DefineLabel();
				if (this.body.Engine.GenerateDebugInfo)
				{
					for (ScriptObject scriptObject = this.enclosing_scope.GetParent(); scriptObject != null; scriptObject = scriptObject.GetParent())
					{
						if (scriptObject is PackageScope)
						{
							ilgenerator.UsingNamespace(((PackageScope)scriptObject).name);
						}
						else if (scriptObject is WrappedNamespace && !((WrappedNamespace)scriptObject).name.Equals(""))
						{
							ilgenerator.UsingNamespace(((WrappedNamespace)scriptObject).name);
						}
					}
				}
				if (!this.isImplicitCtor && this.body != null)
				{
					int startLine = this.body.context.StartLine;
					int startColumn = this.body.context.StartColumn;
					this.body.context.document.EmitLineInfo(ilgenerator, startLine, startColumn, startLine, startColumn + 1);
					if (this.body.context.document.debugOn)
					{
						ilgenerator.Emit(OpCodes.Nop);
					}
				}
				int num3 = this.fields.Length;
				for (int i = 0; i < num3; i++)
				{
					int num4 = (this.IsNestedFunctionField(this.fields[i]) ? (-1) : Array.IndexOf<string>(this.formal_parameters, this.fields[i].Name));
					if (num4 >= 0)
					{
						this.fields[i].metaData = (short)(num4 + num2 + num);
					}
					else if (this.hasArgumentsObject && this.fields[i].Name.Equals("arguments"))
					{
						this.fields[i].metaData = (short)(2 + num);
					}
					else if (!this.fields[i].IsLiteral || this.fields[i].value is FunctionObject)
					{
						Type fieldType = this.fields[i].FieldType;
						LocalBuilder localBuilder = ilgenerator.DeclareLocal(fieldType);
						if (this.fields[i].debugOn)
						{
							localBuilder.SetLocalSymInfo(this.fields[i].debuggerName);
						}
						this.fields[i].metaData = localBuilder;
					}
					else if (this.own_scope.mustSaveStackLocals)
					{
						LocalBuilder localBuilder2 = ilgenerator.DeclareLocal(this.fields[i].FieldType);
						this.fields[i].metaData = localBuilder2;
					}
				}
				if (this.isConstructor)
				{
					int num5 = this.formal_parameters.Length + 1;
					ClassScope classScope = (ClassScope)this.enclosing_scope;
					if (this.superConstructor == null)
					{
						classScope.owner.EmitInitialCalls(ilgenerator, null, null, null, 0);
					}
					else
					{
						ParameterInfo[] parameters = this.superConstructor.GetParameters();
						if (this.superConstructorCall != null)
						{
							classScope.owner.EmitInitialCalls(ilgenerator, this.superConstructor, parameters, this.superConstructorCall.arguments, num5);
						}
						else
						{
							classScope.owner.EmitInitialCalls(ilgenerator, this.superConstructor, parameters, null, num5);
						}
					}
				}
				if ((this.isMethod || this.isConstructor) && this.must_save_stack_locals)
				{
					this.TranslateToMethodWithStackFrame(ilgenerator, compilerGlobals, false);
				}
				else
				{
					this.TranslateToILToCopyOuterScopeLocals(ilgenerator, true, null);
					bool insideProtectedRegion = compilerGlobals.InsideProtectedRegion;
					compilerGlobals.InsideProtectedRegion = false;
					bool insideFinally = compilerGlobals.InsideFinally;
					int finallyStackTop = compilerGlobals.FinallyStackTop;
					compilerGlobals.InsideFinally = false;
					this.body.TranslateToILInitializer(ilgenerator);
					this.body.TranslateToIL(ilgenerator, Typeob.Void);
					compilerGlobals.InsideProtectedRegion = insideProtectedRegion;
					compilerGlobals.InsideFinally = insideFinally;
					compilerGlobals.FinallyStackTop = finallyStackTop;
					ilgenerator.MarkLabel(this.returnLabel);
					if (this.body.context.document.debugOn)
					{
						this.EmitLastLineInfo(ilgenerator);
						ilgenerator.Emit(OpCodes.Nop);
					}
					this.TranslateToILToSaveLocals(ilgenerator);
					if (this.own_scope.returnVar != null)
					{
						ilgenerator.Emit(OpCodes.Ldloc, (LocalBuilder)this.own_scope.returnVar.GetMetaData());
					}
					ilgenerator.Emit(OpCodes.Ret);
				}
			}
			finally
			{
				this.globals.ScopeStack.Pop();
			}
		}

		// Token: 0x060005F1 RID: 1521 RVA: 0x0002B844 File Offset: 0x0002A844
		private bool IsNestedFunctionField(JSLocalField field)
		{
			return field.value != null && field.value is FunctionObject;
		}

		// Token: 0x060005F2 RID: 1522 RVA: 0x0002B85E File Offset: 0x0002A85E
		internal void TranslateToILToLoadEngine(ILGenerator il)
		{
			this.TranslateToILToLoadEngine(il, false);
		}

		// Token: 0x060005F3 RID: 1523 RVA: 0x0002B868 File Offset: 0x0002A868
		private void TranslateToILToLoadEngine(ILGenerator il, bool allocateLocal)
		{
			if (!this.isMethod)
			{
				il.Emit(OpCodes.Ldarg_1);
				return;
			}
			if (!this.isStatic)
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Callvirt, CompilerGlobals.getEngineMethod);
				return;
			}
			if (this.body.Engine.doCRS)
			{
				il.Emit(OpCodes.Ldsfld, CompilerGlobals.contextEngineField);
				return;
			}
			if (this.engineLocal == null)
			{
				if (allocateLocal)
				{
					this.engineLocal = il.DeclareLocal(Typeob.VsaEngine);
				}
				if (this.body.Engine.PEFileKind == PEFileKinds.Dll)
				{
					il.Emit(OpCodes.Ldtoken, ((ClassScope)this.own_scope.GetParent()).GetTypeBuilder());
					il.Emit(OpCodes.Call, CompilerGlobals.createVsaEngineWithType);
				}
				else
				{
					il.Emit(OpCodes.Call, CompilerGlobals.createVsaEngine);
				}
				if (!allocateLocal)
				{
					return;
				}
				il.Emit(OpCodes.Stloc, this.engineLocal);
			}
			il.Emit(OpCodes.Ldloc, this.engineLocal);
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x0002B974 File Offset: 0x0002A974
		private void TranslateToMethodWithStackFrame(ILGenerator il, CompilerGlobals compilerGlobals, bool staticInitializer)
		{
			if (this.isStatic)
			{
				il.Emit(OpCodes.Ldtoken, ((ClassScope)this.own_scope.GetParent()).GetTypeBuilder());
			}
			else
			{
				il.Emit(OpCodes.Ldarg_0);
			}
			int num = this.fields.Length;
			ConstantWrapper.TranslateToILInt(il, num);
			il.Emit(OpCodes.Newarr, Typeob.JSLocalField);
			for (int i = 0; i < num; i++)
			{
				JSLocalField jslocalField = this.fields[i];
				il.Emit(OpCodes.Dup);
				ConstantWrapper.TranslateToILInt(il, i);
				il.Emit(OpCodes.Ldstr, jslocalField.Name);
				il.Emit(OpCodes.Ldtoken, jslocalField.FieldType);
				ConstantWrapper.TranslateToILInt(il, jslocalField.slotNumber);
				il.Emit(OpCodes.Newobj, CompilerGlobals.jsLocalFieldConstructor);
				il.Emit(OpCodes.Stelem_Ref);
			}
			this.TranslateToILToLoadEngine(il, true);
			if (this.isStatic)
			{
				il.Emit(OpCodes.Call, CompilerGlobals.pushStackFrameForStaticMethod);
			}
			else
			{
				il.Emit(OpCodes.Call, CompilerGlobals.pushStackFrameForMethod);
			}
			bool insideProtectedRegion = compilerGlobals.InsideProtectedRegion;
			compilerGlobals.InsideProtectedRegion = true;
			il.BeginExceptionBlock();
			this.body.TranslateToILInitializer(il);
			this.body.TranslateToIL(il, Typeob.Void);
			il.MarkLabel(this.returnLabel);
			this.TranslateToILToSaveLocals(il);
			Label label = il.DefineLabel();
			il.Emit(OpCodes.Leave, label);
			il.BeginFinallyBlock();
			this.TranslateToILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
			il.Emit(OpCodes.Pop);
			il.EndExceptionBlock();
			il.MarkLabel(label);
			if (!staticInitializer)
			{
				if (this.body.context.document.debugOn)
				{
					this.EmitLastLineInfo(il);
					il.Emit(OpCodes.Nop);
				}
				if (this.own_scope.returnVar != null)
				{
					il.Emit(OpCodes.Ldloc, (LocalBuilder)this.own_scope.returnVar.GetMetaData());
				}
				il.Emit(OpCodes.Ret);
			}
			compilerGlobals.InsideProtectedRegion = insideProtectedRegion;
		}

		// Token: 0x060005F5 RID: 1525 RVA: 0x0002BB73 File Offset: 0x0002AB73
		internal void TranslateToILToRestoreLocals(ILGenerator il)
		{
			this.TranslateToILToRestoreLocals(il, null);
		}

		// Token: 0x060005F6 RID: 1526 RVA: 0x0002BB80 File Offset: 0x0002AB80
		internal void TranslateToILToRestoreLocals(ILGenerator il, JSLocalField[] notToBeRestored)
		{
			this.TranslateToILToCopyOuterScopeLocals(il, true, notToBeRestored);
			if (!this.must_save_stack_locals)
			{
				return;
			}
			int num = (((this.attributes & MethodAttributes.Static) == MethodAttributes.Static) ? 0 : 1);
			int num2 = 3;
			if (this.isMethod)
			{
				num2 = 0;
			}
			else if (!this.hasArgumentsObject)
			{
				num2 = 2;
			}
			int num3 = this.fields.Length;
			this.TranslateToILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			ScriptObject scriptObject = this.globals.ScopeStack.Peek();
			while (scriptObject is WithObject || scriptObject is BlockScope)
			{
				il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
				scriptObject = scriptObject.GetParent();
			}
			il.Emit(OpCodes.Castclass, Typeob.StackFrame);
			il.Emit(OpCodes.Ldfld, CompilerGlobals.localVarsField);
			for (int i = 0; i < num3; i++)
			{
				if ((notToBeRestored == null || !FunctionObject.IsPresentIn(this.fields[i], notToBeRestored)) && !this.fields[i].IsLiteral)
				{
					il.Emit(OpCodes.Dup);
					int num4 = Array.IndexOf<string>(this.formal_parameters, this.fields[i].Name);
					ConstantWrapper.TranslateToILInt(il, this.fields[i].slotNumber);
					il.Emit(OpCodes.Ldelem_Ref);
					Convert.Emit(this.body, il, Typeob.Object, this.fields[i].FieldType);
					if (num4 >= 0 || (this.fields[i].Name.Equals("arguments") && this.hasArgumentsObject))
					{
						il.Emit(OpCodes.Starg, (short)(num4 + num2 + num));
					}
					else
					{
						il.Emit(OpCodes.Stloc, (LocalBuilder)this.fields[i].metaData);
					}
				}
			}
			il.Emit(OpCodes.Pop);
		}

		// Token: 0x060005F7 RID: 1527 RVA: 0x0002BD4C File Offset: 0x0002AD4C
		internal void TranslateToILToSaveLocals(ILGenerator il)
		{
			this.TranslateToILToCopyOuterScopeLocals(il, false, null);
			if (!this.must_save_stack_locals)
			{
				return;
			}
			int num = (((this.attributes & MethodAttributes.Static) == MethodAttributes.Static) ? 0 : 1);
			int num2 = 3;
			if (this.isMethod)
			{
				num2 = 0;
			}
			else if (!this.hasArgumentsObject)
			{
				num2 = 2;
			}
			int num3 = this.fields.Length;
			this.TranslateToILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			ScriptObject scriptObject = this.globals.ScopeStack.Peek();
			while (scriptObject is WithObject || scriptObject is BlockScope)
			{
				il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
				scriptObject = scriptObject.GetParent();
			}
			il.Emit(OpCodes.Castclass, Typeob.StackFrame);
			il.Emit(OpCodes.Ldfld, CompilerGlobals.localVarsField);
			for (int i = 0; i < num3; i++)
			{
				JSLocalField jslocalField = this.fields[i];
				if (!jslocalField.IsLiteral || jslocalField.value is FunctionObject)
				{
					il.Emit(OpCodes.Dup);
					ConstantWrapper.TranslateToILInt(il, jslocalField.slotNumber);
					int num4 = Array.IndexOf<string>(this.formal_parameters, jslocalField.Name);
					if (num4 >= 0 || (jslocalField.Name.Equals("arguments") && this.hasArgumentsObject))
					{
						Convert.EmitLdarg(il, (short)(num4 + num2 + num));
					}
					else
					{
						il.Emit(OpCodes.Ldloc, (LocalBuilder)jslocalField.metaData);
					}
					Convert.Emit(this.body, il, jslocalField.FieldType, Typeob.Object);
					il.Emit(OpCodes.Stelem_Ref);
				}
			}
			il.Emit(OpCodes.Pop);
		}

		// Token: 0x060005F8 RID: 1528 RVA: 0x0002BEEC File Offset: 0x0002AEEC
		private void TranslateToILToCopyOuterScopeLocals(ILGenerator il, bool copyToNested, JSLocalField[] notToBeRestored)
		{
			if (this.own_scope.ProvidesOuterScopeLocals == null || this.own_scope.ProvidesOuterScopeLocals.count == 0)
			{
				return;
			}
			this.TranslateToILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.scriptObjectStackTopMethod);
			ScriptObject scriptObject = this.globals.ScopeStack.Peek();
			while (scriptObject is WithObject || scriptObject is BlockScope)
			{
				il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
				scriptObject = scriptObject.GetParent();
			}
			for (scriptObject = this.enclosing_scope; scriptObject != null; scriptObject = scriptObject.GetParent())
			{
				il.Emit(OpCodes.Call, CompilerGlobals.getParentMethod);
				if (scriptObject is FunctionScope && ((FunctionScope)scriptObject).owner != null && this.own_scope.ProvidesOuterScopeLocals[scriptObject] != null)
				{
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Castclass, Typeob.StackFrame);
					il.Emit(OpCodes.Ldfld, CompilerGlobals.localVarsField);
					if (copyToNested)
					{
						((FunctionScope)scriptObject).owner.TranslateToILToCopyLocalsToNestedScope(il, this.own_scope, notToBeRestored);
					}
					else
					{
						((FunctionScope)scriptObject).owner.TranslateToILToCopyLocalsFromNestedScope(il, this.own_scope);
					}
				}
				else if (scriptObject is GlobalScope || scriptObject is ClassScope)
				{
					break;
				}
			}
			il.Emit(OpCodes.Pop);
		}

		// Token: 0x060005F9 RID: 1529 RVA: 0x0002C03C File Offset: 0x0002B03C
		private void TranslateToILToCopyLocalsToNestedScope(ILGenerator il, FunctionScope nestedScope, JSLocalField[] notToBeRestored)
		{
			int num = this.fields.Length;
			for (int i = 0; i < num; i++)
			{
				JSLocalField outerLocalField = nestedScope.GetOuterLocalField(this.fields[i].Name);
				if (outerLocalField != null && outerLocalField.outerField == this.fields[i] && (notToBeRestored == null || !FunctionObject.IsPresentIn(outerLocalField, notToBeRestored)))
				{
					il.Emit(OpCodes.Dup);
					ConstantWrapper.TranslateToILInt(il, this.fields[i].slotNumber);
					il.Emit(OpCodes.Ldelem_Ref);
					Convert.Emit(this.body, il, Typeob.Object, this.fields[i].FieldType);
					il.Emit(OpCodes.Stloc, (LocalBuilder)outerLocalField.metaData);
				}
			}
			il.Emit(OpCodes.Pop);
		}

		// Token: 0x060005FA RID: 1530 RVA: 0x0002C100 File Offset: 0x0002B100
		private void TranslateToILToCopyLocalsFromNestedScope(ILGenerator il, FunctionScope nestedScope)
		{
			int num = this.fields.Length;
			for (int i = 0; i < num; i++)
			{
				JSLocalField outerLocalField = nestedScope.GetOuterLocalField(this.fields[i].Name);
				if (outerLocalField != null && outerLocalField.outerField == this.fields[i])
				{
					il.Emit(OpCodes.Dup);
					ConstantWrapper.TranslateToILInt(il, this.fields[i].slotNumber);
					il.Emit(OpCodes.Ldloc, (LocalBuilder)outerLocalField.metaData);
					Convert.Emit(this.body, il, this.fields[i].FieldType, Typeob.Object);
					il.Emit(OpCodes.Stelem_Ref);
				}
			}
			il.Emit(OpCodes.Pop);
		}

		// Token: 0x04000290 RID: 656
		internal ParameterDeclaration[] parameter_declarations;

		// Token: 0x04000291 RID: 657
		internal string[] formal_parameters;

		// Token: 0x04000292 RID: 658
		internal TypeExpression return_type_expr;

		// Token: 0x04000293 RID: 659
		private Block body;

		// Token: 0x04000294 RID: 660
		private MethodInfo method;

		// Token: 0x04000295 RID: 661
		private ParameterInfo[] parameterInfos;

		// Token: 0x04000296 RID: 662
		internal Context funcContext;

		// Token: 0x04000297 RID: 663
		private int argumentsSlotNumber;

		// Token: 0x04000298 RID: 664
		internal JSLocalField[] fields;

		// Token: 0x04000299 RID: 665
		internal FunctionScope own_scope;

		// Token: 0x0400029A RID: 666
		internal ScriptObject enclosing_scope;

		// Token: 0x0400029B RID: 667
		internal bool must_save_stack_locals;

		// Token: 0x0400029C RID: 668
		internal bool hasArgumentsObject;

		// Token: 0x0400029D RID: 669
		internal IReflect implementedIface;

		// Token: 0x0400029E RID: 670
		internal MethodInfo implementedIfaceMethod;

		// Token: 0x0400029F RID: 671
		internal bool isMethod;

		// Token: 0x040002A0 RID: 672
		internal bool isExpandoMethod;

		// Token: 0x040002A1 RID: 673
		internal bool isConstructor;

		// Token: 0x040002A2 RID: 674
		internal bool isImplicitCtor;

		// Token: 0x040002A3 RID: 675
		internal bool isStatic;

		// Token: 0x040002A4 RID: 676
		internal bool noVersionSafeAttributeSpecified;

		// Token: 0x040002A5 RID: 677
		internal bool suppressIL;

		// Token: 0x040002A6 RID: 678
		internal string text;

		// Token: 0x040002A7 RID: 679
		private MethodBuilder mb;

		// Token: 0x040002A8 RID: 680
		private ConstructorBuilder cb;

		// Token: 0x040002A9 RID: 681
		internal TypeBuilder classwriter;

		// Token: 0x040002AA RID: 682
		internal MethodAttributes attributes;

		// Token: 0x040002AB RID: 683
		internal Globals globals;

		// Token: 0x040002AC RID: 684
		private ConstructorInfo superConstructor;

		// Token: 0x040002AD RID: 685
		internal ConstructorCall superConstructorCall;

		// Token: 0x040002AE RID: 686
		internal CustomAttributeList customAttributes;

		// Token: 0x040002AF RID: 687
		private CLSComplianceSpec clsCompliance;

		// Token: 0x040002B0 RID: 688
		private LocalBuilder engineLocal;

		// Token: 0x040002B1 RID: 689
		private bool partiallyEvaluated;

		// Token: 0x040002B2 RID: 690
		internal Label returnLabel;
	}
}
