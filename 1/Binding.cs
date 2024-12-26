using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;

namespace Microsoft.JScript
{
	// Token: 0x02000029 RID: 41
	public abstract class Binding : AST
	{
		// Token: 0x0600017D RID: 381 RVA: 0x0000853C File Offset: 0x0000753C
		internal Binding(Context context, string name)
			: base(context)
		{
			this.argIRs = null;
			this.defaultMember = null;
			this.defaultMemberReturnIR = null;
			this.isArrayElementAccess = false;
			this.isArrayConstructor = false;
			this.isAssignmentToDefaultIndexedProperty = false;
			this.isFullyResolved = true;
			this.isNonVirtual = false;
			this.members = null;
			this.member = null;
			this.name = name;
			this.giveErrors = true;
		}

		// Token: 0x0600017E RID: 382 RVA: 0x000085A4 File Offset: 0x000075A4
		private bool Accessible(bool checkSetter)
		{
			if (this.member == null)
			{
				return false;
			}
			MemberTypes memberType = this.member.MemberType;
			if (memberType <= MemberTypes.Method)
			{
				switch (memberType)
				{
				case MemberTypes.Constructor:
					return this.AccessibleConstructor();
				case MemberTypes.Event:
					return false;
				case MemberTypes.Constructor | MemberTypes.Event:
					break;
				case MemberTypes.Field:
					return this.AccessibleField(checkSetter);
				default:
					if (memberType == MemberTypes.Method)
					{
						return this.AccessibleMethod();
					}
					break;
				}
			}
			else
			{
				if (memberType == MemberTypes.Property)
				{
					return this.AccessibleProperty(checkSetter);
				}
				if (memberType != MemberTypes.TypeInfo)
				{
					if (memberType == MemberTypes.NestedType)
					{
						if (!((Type)this.member).IsNestedPublic)
						{
							if (this.giveErrors)
							{
								this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
							}
							return false;
						}
						return !checkSetter;
					}
				}
				else
				{
					if (!((Type)this.member).IsPublic)
					{
						if (this.giveErrors)
						{
							this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
						}
						return false;
					}
					return !checkSetter;
				}
			}
			return false;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000869C File Offset: 0x0000769C
		private bool AccessibleConstructor()
		{
			ConstructorInfo constructorInfo = (ConstructorInfo)this.member;
			if ((constructorInfo is JSConstructor && ((JSConstructor)this.member).GetClassScope().owner.isAbstract) || (!(constructorInfo is JSConstructor) && constructorInfo.DeclaringType.IsAbstract))
			{
				this.context.HandleError(JSError.CannotInstantiateAbstractClass);
				return false;
			}
			if (constructorInfo.IsPublic)
			{
				return true;
			}
			if (constructorInfo is JSConstructor && ((JSConstructor)constructorInfo).IsAccessibleFrom(base.Globals.ScopeStack.Peek()))
			{
				return true;
			}
			if (this.giveErrors)
			{
				this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
			}
			return false;
		}

		// Token: 0x06000180 RID: 384 RVA: 0x00008750 File Offset: 0x00007750
		private bool AccessibleField(bool checkWritable)
		{
			FieldInfo fieldInfo = (FieldInfo)this.member;
			if (checkWritable && (fieldInfo.IsInitOnly || fieldInfo.IsLiteral))
			{
				return false;
			}
			if (!fieldInfo.IsPublic)
			{
				JSWrappedField jswrappedField = fieldInfo as JSWrappedField;
				if (jswrappedField != null)
				{
					fieldInfo = (this.member = jswrappedField.wrappedField);
				}
				JSClosureField jsclosureField = fieldInfo as JSClosureField;
				JSMemberField jsmemberField = ((jsclosureField != null) ? jsclosureField.field : fieldInfo) as JSMemberField;
				if (jsmemberField == null)
				{
					if ((!fieldInfo.IsFamily && !fieldInfo.IsFamilyOrAssembly) || !Binding.InsideClassThatExtends(base.Globals.ScopeStack.Peek(), fieldInfo.ReflectedType))
					{
						if (this.giveErrors)
						{
							this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
						}
						return false;
					}
				}
				else if (!jsmemberField.IsAccessibleFrom(base.Globals.ScopeStack.Peek()))
				{
					if (this.giveErrors)
					{
						this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
					}
					return false;
				}
			}
			if (fieldInfo.IsLiteral && fieldInfo is JSVariableField)
			{
				ClassScope classScope = ((JSVariableField)fieldInfo).value as ClassScope;
				if (classScope != null && !classScope.owner.IsStatic)
				{
					Lookup lookup = this as Lookup;
					if (lookup == null || !lookup.InStaticCode() || lookup.InFunctionNestedInsideInstanceMethod())
					{
						return true;
					}
					if (this.giveErrors)
					{
						this.context.HandleError(JSError.InstanceNotAccessibleFromStatic, this.isFullyResolved);
					}
					return true;
				}
			}
			if (fieldInfo.IsStatic || fieldInfo.IsLiteral || this.defaultMember != null || !(this is Lookup) || !((Lookup)this).InStaticCode())
			{
				return true;
			}
			if (fieldInfo is JSWrappedField && fieldInfo.DeclaringType == Typeob.LenientGlobalObject)
			{
				return true;
			}
			if (this.giveErrors)
			{
				if (!fieldInfo.IsStatic && this is Lookup && ((Lookup)this).InStaticCode())
				{
					this.context.HandleError(JSError.InstanceNotAccessibleFromStatic, this.isFullyResolved);
				}
				else
				{
					this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
				}
			}
			return false;
		}

		// Token: 0x06000181 RID: 385 RVA: 0x00008950 File Offset: 0x00007950
		private bool AccessibleMethod()
		{
			MethodInfo methodInfo = (MethodInfo)this.member;
			return this.AccessibleMethod(methodInfo);
		}

		// Token: 0x06000182 RID: 386 RVA: 0x00008970 File Offset: 0x00007970
		private bool AccessibleMethod(MethodInfo meth)
		{
			if (meth == null)
			{
				return false;
			}
			if (this.isNonVirtual && meth.IsAbstract)
			{
				this.context.HandleError(JSError.InvalidCall);
				return false;
			}
			if (!meth.IsPublic)
			{
				JSWrappedMethod jswrappedMethod = meth as JSWrappedMethod;
				if (jswrappedMethod != null)
				{
					meth = jswrappedMethod.method;
				}
				JSClosureMethod jsclosureMethod = meth as JSClosureMethod;
				JSFieldMethod jsfieldMethod = ((jsclosureMethod != null) ? jsclosureMethod.method : meth) as JSFieldMethod;
				if (jsfieldMethod == null)
				{
					if ((meth.IsFamily || meth.IsFamilyOrAssembly) && Binding.InsideClassThatExtends(base.Globals.ScopeStack.Peek(), meth.ReflectedType))
					{
						return true;
					}
					if (this.giveErrors)
					{
						this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
					}
					return false;
				}
				else if (!jsfieldMethod.IsAccessibleFrom(base.Globals.ScopeStack.Peek()))
				{
					if (this.giveErrors)
					{
						this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
					}
					return false;
				}
			}
			if (meth.IsStatic || this.defaultMember != null || !(this is Lookup) || !((Lookup)this).InStaticCode())
			{
				return true;
			}
			if (meth is JSWrappedMethod && ((Lookup)this).CanPlaceAppropriateObjectOnStack(((JSWrappedMethod)meth).obj))
			{
				return true;
			}
			if (this.giveErrors)
			{
				if (!meth.IsStatic && this is Lookup && ((Lookup)this).InStaticCode())
				{
					this.context.HandleError(JSError.InstanceNotAccessibleFromStatic, this.isFullyResolved);
				}
				else
				{
					this.context.HandleError(JSError.NotAccessible, this.isFullyResolved);
				}
			}
			return false;
		}

		// Token: 0x06000183 RID: 387 RVA: 0x00008B00 File Offset: 0x00007B00
		private bool AccessibleProperty(bool checkSetter)
		{
			PropertyInfo propertyInfo = (PropertyInfo)this.member;
			if (this.AccessibleMethod(checkSetter ? JSProperty.GetSetMethod(propertyInfo, true) : JSProperty.GetGetMethod(propertyInfo, true)))
			{
				return true;
			}
			if (this.giveErrors && !checkSetter)
			{
				this.context.HandleError(JSError.WriteOnlyProperty);
			}
			return false;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x00008B54 File Offset: 0x00007B54
		internal static bool AssignmentCompatible(IReflect lhir, AST rhexpr, IReflect rhir, bool reportError)
		{
			if (rhexpr is ConstantWrapper)
			{
				object obj = rhexpr.Evaluate();
				if (obj is ClassScope)
				{
					if (lhir == Typeob.Type || lhir == Typeob.Object || lhir == Typeob.String)
					{
						return true;
					}
					if (reportError)
					{
						rhexpr.context.HandleError(JSError.TypeMismatch);
					}
					return false;
				}
				else
				{
					ClassScope classScope = lhir as ClassScope;
					if (classScope != null)
					{
						EnumDeclaration enumDeclaration = classScope.owner as EnumDeclaration;
						if (enumDeclaration != null)
						{
							ConstantWrapper constantWrapper = rhexpr as ConstantWrapper;
							if (constantWrapper != null && constantWrapper.value is string)
							{
								FieldInfo field = classScope.GetField((string)constantWrapper.value, BindingFlags.Static | BindingFlags.Public);
								if (field == null)
								{
									return false;
								}
								enumDeclaration.PartiallyEvaluate();
								constantWrapper.value = new DeclaredEnumValue(((JSMemberField)field).value, field.Name, classScope);
							}
							if (rhir == Typeob.String)
							{
								return true;
							}
							lhir = enumDeclaration.baseType.ToType();
						}
					}
					else if (lhir is Type)
					{
						Type type = lhir as Type;
						if (type.IsEnum)
						{
							ConstantWrapper constantWrapper2 = rhexpr as ConstantWrapper;
							if (constantWrapper2 != null && constantWrapper2.value is string)
							{
								FieldInfo field2 = type.GetField((string)constantWrapper2.value, BindingFlags.Static | BindingFlags.Public);
								if (field2 == null)
								{
									return false;
								}
								constantWrapper2.value = MetadataEnumValue.GetEnumValue(field2.FieldType, field2.GetRawConstantValue());
							}
							if (rhir == Typeob.String)
							{
								return true;
							}
							lhir = Enum.GetUnderlyingType(type);
						}
					}
					if (lhir is Type)
					{
						try
						{
							Convert.CoerceT(obj, (Type)lhir);
							return true;
						}
						catch
						{
							if (lhir == Typeob.Single && obj is double)
							{
								if (((ConstantWrapper)rhexpr).isNumericLiteral)
								{
									return true;
								}
								double num = (double)obj;
								float num2 = (float)num;
								if (num.ToString(CultureInfo.InvariantCulture).Equals(num2.ToString(CultureInfo.InvariantCulture)))
								{
									((ConstantWrapper)rhexpr).value = num2;
									return true;
								}
							}
							if (lhir == Typeob.Decimal)
							{
								ConstantWrapper constantWrapper3 = rhexpr as ConstantWrapper;
								if (constantWrapper3 != null && constantWrapper3.isNumericLiteral)
								{
									try
									{
										Convert.CoerceT(constantWrapper3.context.GetCode(), Typeob.Decimal);
										return true;
									}
									catch
									{
									}
								}
							}
							if (reportError)
							{
								rhexpr.context.HandleError(JSError.TypeMismatch);
							}
						}
						return false;
					}
				}
			}
			else if (rhexpr is ArrayLiteral)
			{
				return ((ArrayLiteral)rhexpr).AssignmentCompatible(lhir, reportError);
			}
			if (rhir == Typeob.Object)
			{
				return true;
			}
			if (rhir == Typeob.Double && Convert.IsPrimitiveNumericType(lhir))
			{
				return true;
			}
			if (lhir is Type && Typeob.Delegate.IsAssignableFrom((Type)lhir) && rhir == Typeob.ScriptFunction && rhexpr is Binding && ((Binding)rhexpr).IsCompatibleWithDelegate((Type)lhir))
			{
				return true;
			}
			if (Convert.IsPromotableTo(rhir, lhir))
			{
				return true;
			}
			if (Convert.IsJScriptArray(rhir) && Binding.ArrayAssignmentCompatible(rhexpr, lhir))
			{
				return true;
			}
			if (lhir == Typeob.String)
			{
				return true;
			}
			if (rhir == Typeob.String && (lhir == Typeob.Boolean || Convert.IsPrimitiveNumericType(lhir)))
			{
				if (reportError)
				{
					rhexpr.context.HandleError(JSError.PossibleBadConversionFromString);
				}
				return true;
			}
			if ((lhir == Typeob.Char && rhir == Typeob.String) || Convert.IsPromotableTo(lhir, rhir) || (Convert.IsPrimitiveNumericType(lhir) && Convert.IsPrimitiveNumericType(rhir)))
			{
				if (reportError)
				{
					rhexpr.context.HandleError(JSError.PossibleBadConversion);
				}
				return true;
			}
			if (reportError)
			{
				rhexpr.context.HandleError(JSError.TypeMismatch);
			}
			return false;
		}

		// Token: 0x06000185 RID: 389 RVA: 0x00008EC8 File Offset: 0x00007EC8
		private static bool ArrayAssignmentCompatible(AST ast, IReflect lhir)
		{
			if (!Convert.IsArray(lhir))
			{
				return false;
			}
			if (lhir == Typeob.Array)
			{
				ast.context.HandleError(JSError.ArrayMayBeCopied);
				return true;
			}
			if (Convert.GetArrayRank(lhir) == 1)
			{
				ast.context.HandleError(JSError.ArrayMayBeCopied);
				return true;
			}
			return false;
		}

		// Token: 0x06000186 RID: 390 RVA: 0x00008F15 File Offset: 0x00007F15
		internal void CheckIfDeletable()
		{
			if (this.member != null || this.defaultMember != null)
			{
				this.context.HandleError(JSError.NotDeletable);
			}
			this.member = null;
			this.defaultMember = null;
		}

		// Token: 0x06000187 RID: 391 RVA: 0x00008F45 File Offset: 0x00007F45
		internal void CheckIfUseless()
		{
			if (this.members == null || this.members.Length == 0)
			{
				return;
			}
			this.context.HandleError(JSError.UselessExpression);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x00008F6A File Offset: 0x00007F6A
		internal static bool CheckParameters(ParameterInfo[] pars, IReflect[] argIRs, ASTList argAST, Context ctx)
		{
			return Binding.CheckParameters(pars, argIRs, argAST, ctx, 0, false, true);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x00008F78 File Offset: 0x00007F78
		internal static bool CheckParameters(ParameterInfo[] pars, IReflect[] argIRs, ASTList argAST, Context ctx, int offset, bool defaultIsUndefined, bool reportError)
		{
			int num = argIRs.Length;
			int num2 = pars.Length;
			bool flag = false;
			if (num > num2 - offset)
			{
				num = num2 - offset;
				flag = true;
			}
			int i = 0;
			while (i < num)
			{
				IReflect reflect = ((pars[i + offset] is ParameterDeclaration) ? ((ParameterDeclaration)pars[i + offset]).ParameterIReflect : pars[i + offset].ParameterType);
				IReflect reflect2 = argIRs[i];
				if (i == num - 1 && ((reflect is Type && Typeob.Array.IsAssignableFrom((Type)reflect)) || reflect is TypedArray) && CustomAttribute.IsDefined(pars[i + offset], typeof(ParamArrayAttribute), false))
				{
					int num3 = argIRs.Length;
					if (i == num3 - 1 && Binding.AssignmentCompatible(reflect, argAST[i], argIRs[i], false))
					{
						return true;
					}
					IReflect reflect3 = ((reflect is Type) ? ((Type)reflect).GetElementType() : ((TypedArray)reflect).elementType);
					for (int j = i; j < num3; j++)
					{
						if (!Binding.AssignmentCompatible(reflect3, argAST[j], argIRs[j], reportError))
						{
							return false;
						}
					}
					return true;
				}
				else
				{
					if (!Binding.AssignmentCompatible(reflect, argAST[i], reflect2, reportError))
					{
						return false;
					}
					i++;
				}
			}
			if (flag && reportError)
			{
				ctx.HandleError(JSError.TooManyParameters);
			}
			if (offset == 0 && num < num2 && !defaultIsUndefined)
			{
				for (int k = num; k < num2; k++)
				{
					if (TypeReferences.GetDefaultParameterValue(pars[k]) == Convert.DBNull)
					{
						ParameterDeclaration parameterDeclaration = pars[k] as ParameterDeclaration;
						if (parameterDeclaration != null)
						{
							parameterDeclaration.PartiallyEvaluate();
						}
						if (k < num2 - 1 || !CustomAttribute.IsDefined(pars[k], typeof(ParamArrayAttribute), false))
						{
							if (reportError)
							{
								ctx.HandleError(JSError.TooFewParameters);
							}
							IReflect reflect4 = ((pars[k + offset] is ParameterDeclaration) ? ((ParameterDeclaration)pars[k + offset]).ParameterIReflect : pars[k + offset].ParameterType);
							Type type = reflect4 as Type;
							if (type != null && type.IsValueType && !type.IsPrimitive && !type.IsEnum)
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600018A RID: 394 RVA: 0x000091A0 File Offset: 0x000081A0
		internal override bool Delete()
		{
			return this.EvaluateAsLateBinding().Delete();
		}

		// Token: 0x0600018B RID: 395 RVA: 0x000091B0 File Offset: 0x000081B0
		internal override object Evaluate()
		{
			object @object = this.GetObject();
			MemberInfo memberInfo = this.member;
			if (memberInfo != null)
			{
				MemberTypes memberType = memberInfo.MemberType;
				switch (memberType)
				{
				case MemberTypes.Event:
					return null;
				case MemberTypes.Constructor | MemberTypes.Event:
					break;
				case MemberTypes.Field:
					return ((FieldInfo)memberInfo).GetValue(@object);
				default:
					if (memberType == MemberTypes.Property)
					{
						MemberInfo[] array = new MemberInfo[] { JSProperty.GetGetMethod((PropertyInfo)memberInfo, false) };
						return LateBinding.CallOneOfTheMembers(array, new object[0], false, @object, null, null, null, base.Engine);
					}
					if (memberType == MemberTypes.NestedType)
					{
						return memberInfo;
					}
					break;
				}
			}
			if (this.members != null && this.members.Length > 0)
			{
				if (this.members.Length == 1 && this.members[0].MemberType == MemberTypes.Method)
				{
					MethodInfo methodInfo = (MethodInfo)this.members[0];
					Type type = ((methodInfo is JSMethod) ? null : methodInfo.DeclaringType);
					if (type == Typeob.GlobalObject || (type != null && type != Typeob.StringObject && type != Typeob.NumberObject && type != Typeob.BooleanObject && type.IsSubclassOf(Typeob.JSObject)))
					{
						return Globals.BuiltinFunctionFor(@object, TypeReferences.ToExecutionContext(methodInfo));
					}
				}
				return new FunctionWrapper(this.name, @object, this.members);
			}
			return this.EvaluateAsLateBinding().GetValue();
		}

		// Token: 0x0600018C RID: 396 RVA: 0x000092F8 File Offset: 0x000082F8
		private MemberInfoList GetAllKnownInstanceBindingsForThisName()
		{
			IReflect[] allEligibleClasses = this.GetAllEligibleClasses();
			MemberInfoList memberInfoList = new MemberInfoList();
			foreach (IReflect reflect in allEligibleClasses)
			{
				if (reflect is ClassScope)
				{
					if (((ClassScope)reflect).ParentIsInSamePackage())
					{
						memberInfoList.AddRange(reflect.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
					}
					else
					{
						memberInfoList.AddRange(reflect.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
					}
				}
				else
				{
					memberInfoList.AddRange(reflect.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public));
				}
			}
			return memberInfoList;
		}

		// Token: 0x0600018D RID: 397 RVA: 0x00009384 File Offset: 0x00008384
		private IReflect[] GetAllEligibleClasses()
		{
			ArrayList arrayList = new ArrayList(16);
			ClassScope classScope = null;
			PackageScope packageScope = null;
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			while (scriptObject is WithObject || scriptObject is BlockScope)
			{
				scriptObject = scriptObject.GetParent();
			}
			if (scriptObject is FunctionScope)
			{
				scriptObject = ((FunctionScope)scriptObject).owner.enclosing_scope;
			}
			if (scriptObject is ClassScope)
			{
				classScope = (ClassScope)scriptObject;
				packageScope = classScope.package;
			}
			if (classScope != null)
			{
				classScope.AddClassesFromInheritanceChain(this.name, arrayList);
			}
			if (packageScope != null)
			{
				packageScope.AddClassesExcluding(classScope, this.name, arrayList);
			}
			else
			{
				((IActivationObject)scriptObject).GetGlobalScope().AddClassesExcluding(classScope, this.name, arrayList);
			}
			IReflect[] array = new IReflect[arrayList.Count];
			arrayList.CopyTo(array);
			return array;
		}

		// Token: 0x0600018E RID: 398
		protected abstract object GetObject();

		// Token: 0x0600018F RID: 399
		protected abstract void HandleNoSuchMemberError();

		// Token: 0x06000190 RID: 400 RVA: 0x00009448 File Offset: 0x00008448
		internal override IReflect InferType(JSField inference_target)
		{
			if (this.isArrayElementAccess)
			{
				IReflect reflect = this.defaultMemberReturnIR;
				if (!(reflect is TypedArray))
				{
					return ((Type)reflect).GetElementType();
				}
				return ((TypedArray)reflect).elementType;
			}
			else if (this.isAssignmentToDefaultIndexedProperty)
			{
				if (this.member is PropertyInfo)
				{
					return ((PropertyInfo)this.member).PropertyType;
				}
				return Typeob.Object;
			}
			else
			{
				MemberInfo memberInfo = this.member;
				if (memberInfo is FieldInfo)
				{
					JSWrappedField jswrappedField = memberInfo as JSWrappedField;
					if (jswrappedField != null)
					{
						memberInfo = jswrappedField.wrappedField;
					}
					if (memberInfo is JSVariableField)
					{
						return ((JSVariableField)memberInfo).GetInferredType(inference_target);
					}
					return ((FieldInfo)memberInfo).FieldType;
				}
				else if (memberInfo is PropertyInfo)
				{
					JSWrappedProperty jswrappedProperty = memberInfo as JSWrappedProperty;
					if (jswrappedProperty != null)
					{
						memberInfo = jswrappedProperty.property;
					}
					if (memberInfo is JSProperty)
					{
						return ((JSProperty)memberInfo).PropertyIR();
					}
					PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
					if (propertyInfo.DeclaringType == Typeob.GlobalObject)
					{
						return (IReflect)propertyInfo.GetValue(base.Globals.globalObject, null);
					}
					return propertyInfo.PropertyType;
				}
				else
				{
					if (memberInfo is Type)
					{
						return Typeob.Type;
					}
					if (memberInfo is EventInfo)
					{
						return Typeob.EventInfo;
					}
					if (this.members.Length > 0 && base.Engine.doFast)
					{
						return Typeob.ScriptFunction;
					}
					return Typeob.Object;
				}
			}
		}

		// Token: 0x06000191 RID: 401 RVA: 0x00009594 File Offset: 0x00008594
		internal virtual IReflect InferTypeOfCall(JSField inference_target, bool isConstructor)
		{
			if (!this.isFullyResolved)
			{
				return Typeob.Object;
			}
			if (this.isArrayConstructor)
			{
				return this.defaultMemberReturnIR;
			}
			if (this.isArrayElementAccess)
			{
				IReflect reflect = this.defaultMemberReturnIR;
				if (!(reflect is TypedArray))
				{
					return ((Type)reflect).GetElementType();
				}
				return ((TypedArray)reflect).elementType;
			}
			else
			{
				MemberInfo memberInfo = this.member;
				if (memberInfo is JSFieldMethod)
				{
					if (!isConstructor)
					{
						return ((JSFieldMethod)memberInfo).ReturnIR();
					}
					return Typeob.Object;
				}
				else
				{
					if (memberInfo is MethodInfo)
					{
						return ((MethodInfo)memberInfo).ReturnType;
					}
					if (memberInfo is JSConstructor)
					{
						return ((JSConstructor)memberInfo).GetClassScope();
					}
					if (memberInfo is ConstructorInfo)
					{
						return ((ConstructorInfo)memberInfo).DeclaringType;
					}
					if (memberInfo is Type)
					{
						return (Type)memberInfo;
					}
					if (memberInfo is FieldInfo && ((FieldInfo)memberInfo).IsLiteral)
					{
						object obj = ((memberInfo is JSVariableField) ? ((JSVariableField)memberInfo).value : TypeReferences.GetConstantValue((FieldInfo)memberInfo));
						if (obj is ClassScope || obj is TypedArray)
						{
							return (IReflect)obj;
						}
					}
					return Typeob.Object;
				}
			}
		}

		// Token: 0x06000192 RID: 402 RVA: 0x000096B0 File Offset: 0x000086B0
		private static bool InsideClassThatExtends(ScriptObject scope, Type type)
		{
			while (scope is WithObject || scope is BlockScope)
			{
				scope = scope.GetParent();
			}
			if (scope is ClassScope)
			{
				return type.IsAssignableFrom(((ClassScope)scope).GetBakedSuperType());
			}
			return scope is FunctionScope && Binding.InsideClassThatExtends(((FunctionScope)scope).owner.enclosing_scope, type);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x00009711 File Offset: 0x00008711
		internal void InvalidateBinding()
		{
			this.isAssignmentToDefaultIndexedProperty = false;
			this.isArrayConstructor = false;
			this.isArrayElementAccess = false;
			this.defaultMember = null;
			this.member = null;
			this.members = new MemberInfo[0];
		}

		// Token: 0x06000194 RID: 404 RVA: 0x00009744 File Offset: 0x00008744
		internal bool IsCompatibleWithDelegate(Type delegateType)
		{
			MethodInfo method = delegateType.GetMethod("Invoke");
			ParameterInfo[] parameters = method.GetParameters();
			Type returnType = method.ReturnType;
			foreach (MemberInfo memberInfo in this.members)
			{
				if (memberInfo is MethodInfo)
				{
					MethodInfo methodInfo = (MethodInfo)memberInfo;
					Type type;
					if (methodInfo is JSFieldMethod)
					{
						IReflect reflect = ((JSFieldMethod)methodInfo).ReturnIR();
						if (reflect is ClassScope)
						{
							type = ((ClassScope)reflect).GetBakedSuperType();
						}
						else if (reflect is Type)
						{
							type = (Type)reflect;
						}
						else
						{
							type = Convert.ToType(reflect);
						}
						if (((JSFieldMethod)methodInfo).func.isExpandoMethod)
						{
							return false;
						}
					}
					else
					{
						type = methodInfo.ReturnType;
					}
					if (type != returnType || !Class.ParametersMatch(parameters, methodInfo.GetParameters()))
					{
						goto IL_00DC;
					}
					this.member = methodInfo;
					this.isFullyResolved = true;
					return true;
				}
				IL_00DC:;
			}
			return false;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x00009842 File Offset: 0x00008842
		public static bool IsMissing(object value)
		{
			return value is Missing;
		}

		// Token: 0x06000196 RID: 406 RVA: 0x00009850 File Offset: 0x00008850
		private MethodInfo LookForParameterlessPropertyGetter()
		{
			int i = 0;
			int num = this.members.Length;
			while (i < num)
			{
				PropertyInfo propertyInfo = this.members[i] as PropertyInfo;
				if (propertyInfo != null)
				{
					MethodInfo getMethod = propertyInfo.GetGetMethod(true);
					if (getMethod != null)
					{
						ParameterInfo[] parameters = getMethod.GetParameters();
						if (parameters != null && parameters.Length != 0)
						{
							goto IL_003B;
						}
					}
					i++;
					continue;
				}
				IL_003B:
				return null;
			}
			try
			{
				MethodInfo methodInfo = JSBinder.SelectMethod(this.members, new IReflect[0]);
				if (methodInfo != null && methodInfo.IsSpecialName)
				{
					return methodInfo;
				}
			}
			catch (AmbiguousMatchException)
			{
			}
			return null;
		}

		// Token: 0x06000197 RID: 407 RVA: 0x000098E4 File Offset: 0x000088E4
		internal override bool OkToUseAsType()
		{
			MemberInfo memberInfo = this.member;
			if (memberInfo is Type)
			{
				return this.isFullyResolved = true;
			}
			if (memberInfo is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				if (fieldInfo.IsLiteral)
				{
					return (!(fieldInfo is JSMemberField) || !(((JSMemberField)fieldInfo).value is ClassScope) || fieldInfo.IsStatic) && (this.isFullyResolved = true);
				}
				if (!(memberInfo is JSField) && fieldInfo.IsStatic && fieldInfo.GetValue(null) is Type)
				{
					return this.isFullyResolved = true;
				}
			}
			return false;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x00009980 File Offset: 0x00008980
		private int PlaceValuesForHiddenParametersOnStack(ILGenerator il, MethodInfo meth, ParameterInfo[] pars)
		{
			int num = 0;
			if (meth is JSFieldMethod)
			{
				FunctionObject func = ((JSFieldMethod)meth).func;
				if (func != null && func.isMethod)
				{
					return 0;
				}
				if (this is Lookup)
				{
					((Lookup)this).TranslateToILDefaultThisObject(il);
				}
				else
				{
					this.TranslateToILObject(il, Typeob.Object, false);
				}
				base.EmitILToLoadEngine(il);
				return 0;
			}
			else
			{
				object[] customAttributes = CustomAttribute.GetCustomAttributes(meth, typeof(JSFunctionAttribute), false);
				if (customAttributes == null || customAttributes.Length == 0)
				{
					return 0;
				}
				JSFunctionAttributeEnum attributeValue = ((JSFunctionAttribute)customAttributes[0]).attributeValue;
				if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
				{
					num = 1;
					Type parameterType = pars[0].ParameterType;
					if (this is Lookup && parameterType == Typeob.Object)
					{
						((Lookup)this).TranslateToILDefaultThisObject(il);
					}
					else if (Typeob.ArrayObject.IsAssignableFrom(this.member.DeclaringType))
					{
						this.TranslateToILObject(il, Typeob.ArrayObject, false);
					}
					else
					{
						this.TranslateToILObject(il, parameterType, false);
					}
				}
				if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
				{
					num++;
					base.EmitILToLoadEngine(il);
				}
				return num;
			}
		}

		// Token: 0x06000199 RID: 409 RVA: 0x00009A78 File Offset: 0x00008A78
		private bool ParameterlessPropertyValueIsCallable(MethodInfo meth, ASTList args, IReflect[] argIRs, bool constructor, bool brackets)
		{
			ParameterInfo[] parameters = meth.GetParameters();
			if (parameters == null || parameters.Length == 0)
			{
				if ((meth is JSWrappedMethod && ((JSWrappedMethod)meth).GetWrappedObject() is GlobalObject) || argIRs.Length > 0 || (!(meth is JSMethod) && Typeob.ScriptFunction.IsAssignableFrom(meth.ReturnType)))
				{
					this.member = this.ResolveOtherKindOfCall(args, argIRs, constructor, brackets);
					return true;
				}
				IReflect reflect = ((meth is JSFieldMethod) ? ((JSFieldMethod)meth).ReturnIR() : meth.ReturnType);
				if (reflect == Typeob.Object || reflect == Typeob.ScriptFunction)
				{
					this.member = this.ResolveOtherKindOfCall(args, argIRs, constructor, brackets);
					return true;
				}
				this.context.HandleError(JSError.InvalidCall);
			}
			return false;
		}

		// Token: 0x0600019A RID: 410 RVA: 0x00009B34 File Offset: 0x00008B34
		internal static void PlaceArgumentsOnStack(ILGenerator il, ParameterInfo[] pars, ASTList args, int offset, int rhoffset, AST missing)
		{
			int count = args.count;
			int num = count + offset;
			int num2 = pars.Length - rhoffset;
			bool flag = num2 > 0 && CustomAttribute.IsDefined(pars[num2 - 1], typeof(ParamArrayAttribute), false) && (count != num2 || !Convert.IsArrayType(args[count - 1].InferType(null)));
			Type type = (flag ? pars[--num2].ParameterType.GetElementType() : null);
			if (num > num2)
			{
				num = num2;
			}
			for (int i = offset; i < num; i++)
			{
				Type parameterType = pars[i].ParameterType;
				AST ast = args[i - offset];
				if (ast is ConstantWrapper && ((ConstantWrapper)ast).value == Missing.Value)
				{
					object defaultParameterValue = TypeReferences.GetDefaultParameterValue(pars[i]);
					((ConstantWrapper)ast).value = ((defaultParameterValue != Convert.DBNull) ? defaultParameterValue : null);
				}
				if (parameterType.IsByRef)
				{
					ast.TranslateToILReference(il, parameterType.GetElementType());
				}
				else
				{
					ast.TranslateToIL(il, parameterType);
				}
			}
			if (num < num2)
			{
				for (int j = num; j < num2; j++)
				{
					Type parameterType2 = pars[j].ParameterType;
					if (TypeReferences.GetDefaultParameterValue(pars[j]) == Convert.DBNull)
					{
						if (parameterType2.IsByRef)
						{
							missing.TranslateToILReference(il, parameterType2.GetElementType());
						}
						else
						{
							missing.TranslateToIL(il, parameterType2);
						}
					}
					else if (parameterType2.IsByRef)
					{
						new ConstantWrapper(TypeReferences.GetDefaultParameterValue(pars[j]), null).TranslateToILReference(il, parameterType2.GetElementType());
					}
					else
					{
						new ConstantWrapper(TypeReferences.GetDefaultParameterValue(pars[j]), null).TranslateToIL(il, parameterType2);
					}
				}
			}
			if (flag)
			{
				num -= offset;
				num2 = ((count > num) ? (count - num) : 0);
				ConstantWrapper.TranslateToILInt(il, num2);
				il.Emit(OpCodes.Newarr, type);
				bool flag2 = type.IsValueType && !type.IsPrimitive;
				for (int k = 0; k < num2; k++)
				{
					il.Emit(OpCodes.Dup);
					ConstantWrapper.TranslateToILInt(il, k);
					if (flag2)
					{
						il.Emit(OpCodes.Ldelema, type);
					}
					args[k + num].TranslateToIL(il, type);
					Binding.TranslateToStelem(il, type);
				}
			}
		}

		// Token: 0x0600019B RID: 411 RVA: 0x00009D6F File Offset: 0x00008D6F
		internal bool RefersToMemoryLocation()
		{
			return this.isFullyResolved && (this.isArrayElementAccess || this.member is FieldInfo);
		}

		// Token: 0x0600019C RID: 412 RVA: 0x00009D94 File Offset: 0x00008D94
		internal override void ResolveCall(ASTList args, IReflect[] argIRs, bool constructor, bool brackets)
		{
			this.argIRs = argIRs;
			if (this.members != null && this.members.Length != 0)
			{
				MemberInfo memberInfo = null;
				if (!(this is CallableExpression))
				{
					if (constructor)
					{
						if (brackets)
						{
							goto IL_01B8;
						}
					}
					try
					{
						if (constructor)
						{
							memberInfo = (this.member = JSBinder.SelectConstructor(this.members, argIRs));
						}
						else
						{
							MethodInfo methodInfo;
							memberInfo = (this.member = (methodInfo = JSBinder.SelectMethod(this.members, argIRs)));
							if (methodInfo != null && methodInfo.IsSpecialName)
							{
								if (this.name == methodInfo.Name)
								{
									if (this.name.StartsWith("get_", StringComparison.Ordinal) || this.name.StartsWith("set_", StringComparison.Ordinal))
									{
										this.context.HandleError(JSError.NotMeantToBeCalledDirectly);
										this.member = null;
										return;
									}
								}
								else if (this.ParameterlessPropertyValueIsCallable(methodInfo, args, argIRs, constructor, brackets))
								{
									return;
								}
							}
						}
					}
					catch (AmbiguousMatchException)
					{
						if (constructor)
						{
							this.context.HandleError(JSError.AmbiguousConstructorCall, this.isFullyResolved);
						}
						else
						{
							MethodInfo methodInfo2 = this.LookForParameterlessPropertyGetter();
							if (methodInfo2 == null || !this.ParameterlessPropertyValueIsCallable(methodInfo2, args, argIRs, constructor, brackets))
							{
								this.context.HandleError(JSError.AmbiguousMatch, this.isFullyResolved);
								this.member = null;
							}
						}
						return;
					}
					catch (JScriptException ex)
					{
						this.context.HandleError((JSError)(ex.ErrorNumber & 65535), ex.Message, true);
						return;
					}
				}
				IL_01B8:
				if (memberInfo == null)
				{
					memberInfo = this.ResolveOtherKindOfCall(args, argIRs, constructor, brackets);
				}
				if (memberInfo != null)
				{
					if (!this.Accessible(false))
					{
						this.member = null;
						return;
					}
					this.WarnIfObsolete();
					if (memberInfo is MethodBase)
					{
						if (CustomAttribute.IsDefined(memberInfo, typeof(JSFunctionAttribute), false) && !(this.defaultMember is PropertyInfo))
						{
							int num = 0;
							object[] customAttributes = CustomAttribute.GetCustomAttributes(memberInfo, typeof(JSFunctionAttribute), false);
							JSFunctionAttributeEnum attributeValue = ((JSFunctionAttribute)customAttributes[0]).attributeValue;
							if ((constructor && !(memberInfo is ConstructorInfo)) || (attributeValue & JSFunctionAttributeEnum.HasArguments) != JSFunctionAttributeEnum.None)
							{
								this.member = LateBinding.SelectMember(this.members);
								this.defaultMember = null;
								return;
							}
							if ((attributeValue & JSFunctionAttributeEnum.HasThisObject) != JSFunctionAttributeEnum.None)
							{
								num = 1;
							}
							if ((attributeValue & JSFunctionAttributeEnum.HasEngine) != JSFunctionAttributeEnum.None)
							{
								num++;
							}
							if (!Binding.CheckParameters(((MethodBase)memberInfo).GetParameters(), argIRs, args, this.context, num, true, this.isFullyResolved))
							{
								this.member = null;
								return;
							}
						}
						else
						{
							if (constructor && memberInfo is JSFieldMethod)
							{
								this.member = LateBinding.SelectMember(this.members);
								return;
							}
							if (constructor && memberInfo is ConstructorInfo && !(memberInfo is JSConstructor) && Typeob.Delegate.IsAssignableFrom(memberInfo.DeclaringType))
							{
								this.context.HandleError(JSError.DelegatesShouldNotBeExplicitlyConstructed);
								this.member = null;
								return;
							}
							if (!Binding.CheckParameters(((MethodBase)memberInfo).GetParameters(), argIRs, args, this.context, 0, false, this.isFullyResolved))
							{
								this.member = null;
							}
						}
					}
					return;
				}
				return;
			}
			if (!constructor || !this.isFullyResolved || !base.Engine.doFast)
			{
				this.HandleNoSuchMemberError();
				return;
			}
			if (this.member != null && (this.member is Type || (this.member is FieldInfo && ((FieldInfo)this.member).IsLiteral)))
			{
				this.context.HandleError(JSError.NoConstructor);
				return;
			}
			this.HandleNoSuchMemberError();
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000A0E8 File Offset: 0x000090E8
		internal override object ResolveCustomAttribute(ASTList args, IReflect[] argIRs, AST target)
		{
			try
			{
				this.ResolveCall(args, argIRs, true, false);
			}
			catch (AmbiguousMatchException)
			{
				this.context.HandleError(JSError.AmbiguousConstructorCall);
				return null;
			}
			JSConstructor jsconstructor = this.member as JSConstructor;
			if (jsconstructor != null)
			{
				ClassScope classScope = jsconstructor.GetClassScope();
				if (classScope.owner.IsCustomAttribute())
				{
					return classScope;
				}
			}
			else
			{
				ConstructorInfo constructorInfo = this.member as ConstructorInfo;
				if (constructorInfo != null)
				{
					Type declaringType = constructorInfo.DeclaringType;
					if (Typeob.Attribute.IsAssignableFrom(declaringType))
					{
						object[] customAttributes = CustomAttribute.GetCustomAttributes(declaringType, typeof(AttributeUsageAttribute), false);
						if (customAttributes.Length > 0)
						{
							return declaringType;
						}
					}
				}
			}
			this.context.HandleError(JSError.InvalidCustomAttributeClassOrCtor);
			return null;
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000A1A0 File Offset: 0x000091A0
		internal void ResolveLHValue()
		{
			MemberInfo memberInfo = (this.member = LateBinding.SelectMember(this.members));
			if ((memberInfo != null && !this.Accessible(true)) || (this.member == null && this.members.Length > 0))
			{
				this.context.HandleError(JSError.AssignmentToReadOnly, this.isFullyResolved);
				this.member = null;
				this.members = new MemberInfo[0];
				return;
			}
			if (memberInfo is JSPrototypeField)
			{
				this.member = null;
				this.members = new MemberInfo[0];
				return;
			}
			this.WarnIfNotFullyResolved();
			this.WarnIfObsolete();
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000A234 File Offset: 0x00009234
		private MemberInfo ResolveOtherKindOfCall(ASTList argList, IReflect[] argIRs, bool constructor, bool brackets)
		{
			MemberInfo memberInfo = (this.member = LateBinding.SelectMember(this.members));
			if (memberInfo is PropertyInfo && !(memberInfo is JSProperty) && memberInfo.DeclaringType == Typeob.GlobalObject)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				Type propertyType = propertyInfo.PropertyType;
				if (propertyType == Typeob.Type)
				{
					memberInfo = (Type)propertyInfo.GetValue(null, null);
				}
				else if (constructor && brackets)
				{
					MethodInfo method = propertyType.GetMethod("CreateInstance", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
					if (method != null)
					{
						Type returnType = method.ReturnType;
						if (returnType == Typeob.BooleanObject)
						{
							memberInfo = Typeob.Boolean;
						}
						else if (returnType == Typeob.StringObject)
						{
							memberInfo = Typeob.String;
						}
						else
						{
							memberInfo = returnType;
						}
					}
				}
			}
			CallableExpression callableExpression = this as CallableExpression;
			if (callableExpression != null)
			{
				ConstantWrapper constantWrapper = callableExpression.expression as ConstantWrapper;
				if (constantWrapper != null && constantWrapper.InferType(null) is Type)
				{
					memberInfo = new JSGlobalField(null, null, constantWrapper.value, FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.Literal);
				}
			}
			if (memberInfo is Type)
			{
				if (constructor)
				{
					if (brackets)
					{
						this.isArrayConstructor = true;
						this.defaultMember = memberInfo;
						this.defaultMemberReturnIR = new TypedArray((Type)memberInfo, argIRs.Length);
						int i = 0;
						int num = argIRs.Length;
						while (i < num)
						{
							if (argIRs[i] != Typeob.Object && !Convert.IsPrimitiveNumericType(argIRs[i]))
							{
								argList[i].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
								break;
							}
							i++;
						}
						return this.member = memberInfo;
					}
					ConstructorInfo[] constructors = ((Type)memberInfo).GetConstructors(BindingFlags.Instance | BindingFlags.Public);
					if (constructors == null || constructors.Length == 0)
					{
						this.context.HandleError(JSError.NoConstructor);
						this.member = null;
						return null;
					}
					this.members = constructors;
					this.ResolveCall(argList, argIRs, true, brackets);
					return this.member;
				}
				else
				{
					if (!brackets && argIRs.Length == 1)
					{
						return memberInfo;
					}
					this.context.HandleError(JSError.InvalidCall);
					return this.member = null;
				}
			}
			else
			{
				if (memberInfo is JSPrototypeField)
				{
					return this.member = null;
				}
				if (memberInfo is FieldInfo && ((FieldInfo)memberInfo).IsLiteral)
				{
					if (!this.AccessibleField(false))
					{
						return this.member = null;
					}
					object obj = ((memberInfo is JSVariableField) ? ((JSVariableField)memberInfo).value : TypeReferences.GetConstantValue((FieldInfo)memberInfo));
					if (obj is ClassScope || obj is Type)
					{
						if (constructor)
						{
							if (brackets)
							{
								this.isArrayConstructor = true;
								this.defaultMember = memberInfo;
								this.defaultMemberReturnIR = new TypedArray((obj is ClassScope) ? ((IReflect)obj) : ((IReflect)obj), argIRs.Length);
								int j = 0;
								int num2 = argIRs.Length;
								while (j < num2)
								{
									if (argIRs[j] != Typeob.Object && !Convert.IsPrimitiveNumericType(argIRs[j]))
									{
										argList[j].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
										break;
									}
									j++;
								}
								return this.member = memberInfo;
							}
							ConstantWrapper constantWrapper2;
							if (obj is ClassScope && !((ClassScope)obj).owner.isStatic && this is Member && (constantWrapper2 = ((Member)this).rootObject as ConstantWrapper) != null && !(constantWrapper2.Evaluate() is Namespace))
							{
								((Member)this).rootObject.context.HandleError(JSError.NeedInstance);
								return null;
							}
							this.members = ((obj is ClassScope) ? ((ClassScope)obj).constructors : ((Type)obj).GetConstructors(BindingFlags.Instance | BindingFlags.Public));
							if (this.members == null || this.members.Length == 0)
							{
								this.context.HandleError(JSError.NoConstructor);
								this.member = null;
								return null;
							}
							this.ResolveCall(argList, argIRs, true, brackets);
							return this.member;
						}
						else
						{
							if (!brackets && argIRs.Length == 1)
							{
								Type type = obj as Type;
								return this.member = ((type != null) ? type : memberInfo);
							}
							this.context.HandleError(JSError.InvalidCall);
							return this.member = null;
						}
					}
					else if (obj is TypedArray)
					{
						if (!constructor)
						{
							if (argIRs.Length == 1 && !brackets)
							{
								return this.member = memberInfo;
							}
							goto IL_08E1;
						}
						else
						{
							if (brackets && argIRs.Length != 0)
							{
								this.isArrayConstructor = true;
								this.defaultMember = memberInfo;
								this.defaultMemberReturnIR = new TypedArray((IReflect)obj, argIRs.Length);
								int k = 0;
								int num3 = argIRs.Length;
								while (k < num3)
								{
									if (argIRs[k] != Typeob.Object && !Convert.IsPrimitiveNumericType(argIRs[k]))
									{
										argList[k].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
										break;
									}
									k++;
								}
								return this.member = memberInfo;
							}
							goto IL_08E1;
						}
					}
					else if (obj is FunctionObject)
					{
						FunctionObject functionObject = (FunctionObject)obj;
						if (!functionObject.isExpandoMethod && !functionObject.Must_save_stack_locals && (functionObject.own_scope.ProvidesOuterScopeLocals == null || functionObject.own_scope.ProvidesOuterScopeLocals.count == 0))
						{
							return this.member = ((JSVariableField)this.member).GetAsMethod(functionObject.own_scope);
						}
						return this.member;
					}
				}
				IReflect reflect = this.InferType(null);
				Type type2 = reflect as Type;
				if (!brackets && ((type2 != null && Typeob.ScriptFunction.IsAssignableFrom(type2)) || reflect is ScriptFunction))
				{
					this.defaultMember = memberInfo;
					if (type2 == null)
					{
						this.defaultMemberReturnIR = Globals.TypeRefs.ToReferenceContext(reflect.GetType());
						this.member = this.defaultMemberReturnIR.GetMethod(constructor ? "CreateInstance" : "Invoke", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
						if (this.member == null)
						{
							this.defaultMemberReturnIR = Typeob.ScriptFunction;
							this.member = this.defaultMemberReturnIR.GetMethod(constructor ? "CreateInstance" : "Invoke", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
						}
						return this.member;
					}
					if (constructor && this.members.Length != 0 && this.members[0] is JSFieldMethod)
					{
						JSFieldMethod jsfieldMethod = (JSFieldMethod)this.members[0];
						jsfieldMethod.func.PartiallyEvaluate();
						if (!jsfieldMethod.func.isExpandoMethod)
						{
							this.context.HandleError(JSError.NotAnExpandoFunction, this.isFullyResolved);
						}
					}
					this.defaultMemberReturnIR = type2;
					return this.member = type2.GetMethod(constructor ? "CreateInstance" : "Invoke", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				}
				else
				{
					if (reflect == Typeob.Type)
					{
						this.member = null;
						return null;
					}
					if (reflect == Typeob.Object || (reflect is ScriptObject && brackets && !(reflect is ClassScope)))
					{
						return memberInfo;
					}
					if (reflect is TypedArray || (reflect is Type && ((Type)reflect).IsArray))
					{
						int num4 = argIRs.Length;
						int num5 = ((reflect is TypedArray) ? ((TypedArray)reflect).rank : ((Type)reflect).GetArrayRank());
						if (num4 != num5)
						{
							this.context.HandleError(JSError.IncorrectNumberOfIndices, this.isFullyResolved);
						}
						else
						{
							for (int l = 0; l < num5; l++)
							{
								if (argIRs[l] != Typeob.Object && (!Convert.IsPrimitiveNumericType(argIRs[l]) || Convert.IsBadIndex(argList[l])))
								{
									argList[l].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
									break;
								}
							}
						}
						if (constructor)
						{
							if (!brackets)
							{
								goto IL_08E1;
							}
							if (!(reflect is TypedArray))
							{
								((Type)reflect).GetElementType();
							}
							else
							{
								IReflect elementType = ((TypedArray)reflect).elementType;
							}
							if (reflect != Typeob.Object && !(reflect is ClassScope) && (!(reflect is Type) || Typeob.Type.IsAssignableFrom((Type)reflect) || Typeob.ScriptFunction.IsAssignableFrom((Type)reflect)))
							{
								goto IL_08E1;
							}
						}
						this.isArrayElementAccess = true;
						this.defaultMember = memberInfo;
						this.defaultMemberReturnIR = reflect;
						return null;
					}
					if (!constructor)
					{
						if (brackets && reflect == Typeob.String && (this.argIRs.Length != 1 || !Convert.IsPrimitiveNumericType(argIRs[0])))
						{
							reflect = Typeob.StringObject;
						}
						MemberInfo[] array = ((brackets || !(reflect is ScriptObject)) ? JSBinder.GetDefaultMembers(reflect) : null);
						if (array != null && array.Length > 0)
						{
							try
							{
								this.defaultMember = memberInfo;
								this.defaultMemberReturnIR = reflect;
								return this.member = JSBinder.SelectMethod(this.members = array, argIRs);
							}
							catch (AmbiguousMatchException)
							{
								this.context.HandleError(JSError.AmbiguousMatch, this.isFullyResolved);
								return this.member = null;
							}
						}
						if (!brackets && reflect is Type && Typeob.Delegate.IsAssignableFrom((Type)reflect))
						{
							this.defaultMember = memberInfo;
							this.defaultMemberReturnIR = reflect;
							return this.member = ((Type)reflect).GetMethod("Invoke");
						}
					}
				}
				IL_08E1:
				if (constructor)
				{
					this.context.HandleError(JSError.NeedType, this.isFullyResolved);
				}
				else if (brackets)
				{
					this.context.HandleError(JSError.NotIndexable, this.isFullyResolved);
				}
				else
				{
					this.context.HandleError(JSError.FunctionExpected, this.isFullyResolved);
				}
				return this.member = null;
			}
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000AB90 File Offset: 0x00009B90
		protected void ResolveRHValue()
		{
			MemberInfo memberInfo = (this.member = LateBinding.SelectMember(this.members));
			JSLocalField jslocalField = this.member as JSLocalField;
			if (jslocalField != null)
			{
				FunctionObject functionObject = jslocalField.value as FunctionObject;
				if (functionObject != null)
				{
					FunctionScope functionScope = functionObject.enclosing_scope as FunctionScope;
					if (functionScope != null)
					{
						functionScope.closuresMightEscape = true;
					}
				}
			}
			if (memberInfo is JSPrototypeField)
			{
				this.member = null;
				return;
			}
			if (!this.Accessible(false))
			{
				this.member = null;
				return;
			}
			this.WarnIfObsolete();
			this.WarnIfNotFullyResolved();
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000AC15 File Offset: 0x00009C15
		internal override void SetPartialValue(AST partial_value)
		{
			Binding.AssignmentCompatible(this.InferType(null), partial_value, partial_value.InferType(null), this.isFullyResolved);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000AC34 File Offset: 0x00009C34
		internal void SetPartialValue(ASTList argList, IReflect[] argIRs, AST partial_value, bool inBrackets)
		{
			if (this.members == null || this.members.Length == 0)
			{
				this.HandleNoSuchMemberError();
				this.isAssignmentToDefaultIndexedProperty = true;
				return;
			}
			this.PartiallyEvaluate();
			IReflect reflect = this.InferType(null);
			this.isAssignmentToDefaultIndexedProperty = true;
			if (reflect == Typeob.Object)
			{
				JSVariableField jsvariableField = this.member as JSVariableField;
				if (jsvariableField == null || !jsvariableField.IsLiteral || !(jsvariableField.value is ClassScope))
				{
					return;
				}
				reflect = Typeob.Type;
			}
			else
			{
				if (reflect is TypedArray || (reflect is Type && ((Type)reflect).IsArray))
				{
					bool flag = false;
					int num = argIRs.Length;
					int num2 = ((reflect is TypedArray) ? ((TypedArray)reflect).rank : ((Type)reflect).GetArrayRank());
					if (num != num2)
					{
						this.context.HandleError(JSError.IncorrectNumberOfIndices, this.isFullyResolved);
						flag = true;
					}
					for (int i = 0; i < num2; i++)
					{
						if (!flag && i < num && argIRs[i] != Typeob.Object && (!Convert.IsPrimitiveNumericType(argIRs[i]) || Convert.IsBadIndex(argList[i])))
						{
							argList[i].context.HandleError(JSError.TypeMismatch, this.isFullyResolved);
							flag = true;
						}
					}
					this.isArrayElementAccess = true;
					this.isAssignmentToDefaultIndexedProperty = false;
					this.defaultMember = this.member;
					this.defaultMemberReturnIR = reflect;
					IReflect reflect2 = ((reflect is TypedArray) ? ((TypedArray)reflect).elementType : ((Type)reflect).GetElementType());
					Binding.AssignmentCompatible(reflect2, partial_value, partial_value.InferType(null), this.isFullyResolved);
					return;
				}
				MemberInfo[] defaultMembers = JSBinder.GetDefaultMembers(reflect);
				if (defaultMembers != null && defaultMembers.Length > 0 && this.member != null)
				{
					try
					{
						PropertyInfo propertyInfo = JSBinder.SelectProperty(defaultMembers, argIRs);
						if (propertyInfo == null)
						{
							this.context.HandleError(JSError.NotIndexable, Convert.ToTypeName(reflect));
						}
						else if (JSProperty.GetSetMethod(propertyInfo, true) == null)
						{
							if (reflect == Typeob.String)
							{
								this.context.HandleError(JSError.UselessAssignment);
							}
							else
							{
								this.context.HandleError(JSError.AssignmentToReadOnly, this.isFullyResolved && base.Engine.doFast);
							}
						}
						else if (Binding.CheckParameters(propertyInfo.GetIndexParameters(), argIRs, argList, this.context, 0, false, true))
						{
							this.defaultMember = this.member;
							this.defaultMemberReturnIR = reflect;
							this.members = defaultMembers;
							this.member = propertyInfo;
						}
					}
					catch (AmbiguousMatchException)
					{
						this.context.HandleError(JSError.AmbiguousMatch, this.isFullyResolved);
						this.member = null;
					}
					return;
				}
			}
			this.member = null;
			if (!inBrackets)
			{
				this.context.HandleError(JSError.IllegalAssignment);
				return;
			}
			this.context.HandleError(JSError.NotIndexable, Convert.ToTypeName(reflect));
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000AF08 File Offset: 0x00009F08
		internal override void SetValue(object value)
		{
			MemberInfo memberInfo = this.member;
			object @object = this.GetObject();
			if (memberInfo is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				if (fieldInfo.IsLiteral || fieldInfo.IsInitOnly)
				{
					return;
				}
				if (!(fieldInfo is JSField) || fieldInfo is JSWrappedField)
				{
					value = Convert.CoerceT(value, fieldInfo.FieldType, false);
				}
				fieldInfo.SetValue(@object, value, BindingFlags.SuppressChangeType, null, null);
				return;
			}
			else if (memberInfo is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				if (@object is ClassScope && !(propertyInfo is JSProperty))
				{
					JSProperty.SetValue(propertyInfo, ((WithObject)((ClassScope)@object).GetParent()).contained_object, value, null);
					return;
				}
				if (!(propertyInfo is JSProperty))
				{
					value = Convert.CoerceT(value, propertyInfo.PropertyType, false);
				}
				JSProperty.SetValue(propertyInfo, @object, value, null);
				return;
			}
			else
			{
				if (this.members == null || this.members.Length == 0)
				{
					this.EvaluateAsLateBinding().SetValue(value);
					return;
				}
				throw new JScriptException(JSError.IllegalAssignment);
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0000AFF9 File Offset: 0x00009FF9
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.TranslateToIL(il, rtype, false, false);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000B005 File Offset: 0x0000A005
		internal void TranslateToIL(ILGenerator il, Type rtype, bool calledFromDelete)
		{
			this.TranslateToIL(il, rtype, false, false, calledFromDelete);
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000B012 File Offset: 0x0000A012
		private void TranslateToIL(ILGenerator il, Type rtype, bool preSet, bool preSetPlusGet)
		{
			this.TranslateToIL(il, rtype, preSet, preSetPlusGet, false);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000B020 File Offset: 0x0000A020
		private void TranslateToIL(ILGenerator il, Type rtype, bool preSet, bool preSetPlusGet, bool calledFromDelete)
		{
			if (this.member is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)this.member;
				bool flag = fieldInfo.IsStatic || fieldInfo.IsLiteral;
				if (fieldInfo.IsLiteral && fieldInfo is JSMemberField)
				{
					object value = ((JSMemberField)fieldInfo).value;
					FunctionObject functionObject = value as FunctionObject;
					flag = functionObject == null || !functionObject.isExpandoMethod;
				}
				if (!flag || fieldInfo is JSClosureField)
				{
					this.TranslateToILObject(il, fieldInfo.DeclaringType, true);
					if (preSetPlusGet)
					{
						il.Emit(OpCodes.Dup);
					}
					flag = false;
				}
				if (!preSet)
				{
					object obj = ((fieldInfo is JSField) ? ((JSField)fieldInfo).GetMetaData() : ((fieldInfo is JSFieldInfo) ? ((JSFieldInfo)fieldInfo).field : fieldInfo));
					if (obj is FieldInfo && !((FieldInfo)obj).IsLiteral)
					{
						il.Emit(flag ? OpCodes.Ldsfld : OpCodes.Ldfld, (FieldInfo)obj);
					}
					else if (obj is LocalBuilder)
					{
						il.Emit(OpCodes.Ldloc, (LocalBuilder)obj);
					}
					else
					{
						if (fieldInfo.IsLiteral)
						{
							new ConstantWrapper(TypeReferences.GetConstantValue(fieldInfo), this.context).TranslateToIL(il, rtype);
							return;
						}
						Convert.EmitLdarg(il, (short)obj);
					}
					Convert.Emit(this, il, fieldInfo.FieldType, rtype);
				}
				return;
			}
			if (this.member is PropertyInfo)
			{
				PropertyInfo propertyInfo = (PropertyInfo)this.member;
				MethodInfo methodInfo = (preSet ? JSProperty.GetSetMethod(propertyInfo, true) : JSProperty.GetGetMethod(propertyInfo, true));
				if (methodInfo != null)
				{
					bool flag2 = methodInfo.IsStatic && !(methodInfo is JSClosureMethod);
					if (!flag2)
					{
						Type declaringType = methodInfo.DeclaringType;
						if (declaringType == Typeob.StringObject && methodInfo.Name.Equals("get_length"))
						{
							this.TranslateToILObject(il, Typeob.String, false);
							methodInfo = CompilerGlobals.stringLengthMethod;
						}
						else
						{
							this.TranslateToILObject(il, declaringType, true);
						}
					}
					if (!preSet)
					{
						methodInfo = this.GetMethodInfoMetadata(methodInfo);
						if (flag2)
						{
							il.Emit(OpCodes.Call, methodInfo);
						}
						else
						{
							if (preSetPlusGet)
							{
								il.Emit(OpCodes.Dup);
							}
							if (!this.isNonVirtual && methodInfo.IsVirtual && !methodInfo.IsFinal && (!methodInfo.ReflectedType.IsSealed || !methodInfo.ReflectedType.IsValueType))
							{
								il.Emit(OpCodes.Callvirt, methodInfo);
							}
							else
							{
								il.Emit(OpCodes.Call, methodInfo);
							}
						}
						Convert.Emit(this, il, methodInfo.ReturnType, rtype);
					}
					return;
				}
				if (preSet)
				{
					return;
				}
				if (this is Lookup)
				{
					il.Emit(OpCodes.Ldc_I4, 5041);
					il.Emit(OpCodes.Newobj, CompilerGlobals.scriptExceptionConstructor);
					il.Emit(OpCodes.Throw);
					return;
				}
				il.Emit(OpCodes.Ldnull);
				return;
			}
			else
			{
				if (!(this.member is MethodInfo))
				{
					object obj2 = null;
					if (this is Lookup)
					{
						((Lookup)this).TranslateToLateBinding(il);
					}
					else
					{
						if (!this.isFullyResolved && !preSet && !preSetPlusGet)
						{
							obj2 = this.TranslateToSpeculativeEarlyBindings(il, rtype, false);
						}
						((Member)this).TranslateToLateBinding(il, obj2 != null);
						if (!this.isFullyResolved && preSetPlusGet)
						{
							obj2 = this.TranslateToSpeculativeEarlyBindings(il, rtype, true);
						}
					}
					if (preSetPlusGet)
					{
						il.Emit(OpCodes.Dup);
					}
					if (!preSet)
					{
						if (this is Lookup && !calledFromDelete)
						{
							il.Emit(OpCodes.Call, CompilerGlobals.getValue2Method);
						}
						else
						{
							il.Emit(OpCodes.Call, CompilerGlobals.getNonMissingValueMethod);
						}
						Convert.Emit(this, il, Typeob.Object, rtype);
						if (obj2 != null)
						{
							il.MarkLabel((Label)obj2);
						}
					}
					return;
				}
				MethodInfo methodInfoMetadata = this.GetMethodInfoMetadata((MethodInfo)this.member);
				if (Typeob.Delegate.IsAssignableFrom(rtype))
				{
					if (!methodInfoMetadata.IsStatic)
					{
						Type declaringType2 = methodInfoMetadata.DeclaringType;
						this.TranslateToILObject(il, declaringType2, false);
						if (declaringType2.IsValueType)
						{
							il.Emit(OpCodes.Box, declaringType2);
						}
					}
					else
					{
						il.Emit(OpCodes.Ldnull);
					}
					if (methodInfoMetadata.IsVirtual && !methodInfoMetadata.IsFinal && (!methodInfoMetadata.ReflectedType.IsSealed || !methodInfoMetadata.ReflectedType.IsValueType))
					{
						il.Emit(OpCodes.Dup);
						il.Emit(OpCodes.Ldvirtftn, methodInfoMetadata);
					}
					else
					{
						il.Emit(OpCodes.Ldftn, methodInfoMetadata);
					}
					ConstructorInfo constructorInfo = rtype.GetConstructor(new Type[]
					{
						Typeob.Object,
						Typeob.UIntPtr
					});
					if (constructorInfo == null)
					{
						constructorInfo = rtype.GetConstructor(new Type[]
						{
							Typeob.Object,
							Typeob.IntPtr
						});
					}
					il.Emit(OpCodes.Newobj, constructorInfo);
					return;
				}
				if (this.member is JSExpandoIndexerMethod)
				{
					MemberInfo memberInfo = this.member;
					this.member = this.defaultMember;
					this.TranslateToIL(il, Typeob.Object);
					this.member = memberInfo;
					return;
				}
				il.Emit(OpCodes.Ldnull);
				Convert.Emit(this, il, Typeob.Object, rtype);
				return;
			}
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000B51C File Offset: 0x0000A51C
		internal override void TranslateToILCall(ILGenerator il, Type rtype, ASTList argList, bool construct, bool brackets)
		{
			MemberInfo memberInfo = this.member;
			if (this.defaultMember != null)
			{
				if (this.isArrayConstructor)
				{
					TypedArray typedArray = (TypedArray)this.defaultMemberReturnIR;
					Type type = Convert.ToType(typedArray.elementType);
					int rank = typedArray.rank;
					if (rank == 1)
					{
						argList[0].TranslateToIL(il, Typeob.Int32);
						il.Emit(OpCodes.Newarr, type);
					}
					else
					{
						Type type2 = typedArray.ToType();
						Type[] array = new Type[rank];
						for (int i = 0; i < rank; i++)
						{
							array[i] = Typeob.Int32;
						}
						int j = 0;
						int count = argList.count;
						while (j < count)
						{
							argList[j].TranslateToIL(il, Typeob.Int32);
							j++;
						}
						TypeBuilder typeBuilder = type as TypeBuilder;
						if (typeBuilder != null)
						{
							MethodInfo arrayMethod = ((ModuleBuilder)type2.Module).GetArrayMethod(type2, ".ctor", CallingConventions.HasThis, Typeob.Void, array);
							il.Emit(OpCodes.Newobj, arrayMethod);
						}
						else
						{
							ConstructorInfo constructor = type2.GetConstructor(array);
							il.Emit(OpCodes.Newobj, constructor);
						}
					}
					Convert.Emit(this, il, typedArray.ToType(), rtype);
					return;
				}
				this.member = this.defaultMember;
				IReflect reflect = this.defaultMemberReturnIR;
				Type type3 = ((reflect is Type) ? ((Type)reflect) : Convert.ToType(reflect));
				this.TranslateToIL(il, type3);
				if (this.isArrayElementAccess)
				{
					int k = 0;
					int count2 = argList.count;
					while (k < count2)
					{
						argList[k].TranslateToIL(il, Typeob.Int32);
						k++;
					}
					Type elementType = type3.GetElementType();
					int arrayRank = type3.GetArrayRank();
					if (arrayRank == 1)
					{
						Binding.TranslateToLdelem(il, elementType);
					}
					else
					{
						Type[] array2 = new Type[arrayRank];
						for (int l = 0; l < arrayRank; l++)
						{
							array2[l] = Typeob.Int32;
						}
						MethodInfo arrayMethod2 = base.compilerGlobals.module.GetArrayMethod(type3, "Get", CallingConventions.HasThis, elementType, array2);
						il.Emit(OpCodes.Call, arrayMethod2);
					}
					Convert.Emit(this, il, elementType, rtype);
					return;
				}
				this.member = memberInfo;
			}
			if (memberInfo is MethodInfo)
			{
				MethodInfo methodInfo = (MethodInfo)memberInfo;
				Type declaringType = methodInfo.DeclaringType;
				Type reflectedType = methodInfo.ReflectedType;
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (!methodInfo.IsStatic && this.defaultMember == null)
				{
					this.TranslateToILObject(il, declaringType, true);
				}
				if (methodInfo is JSClosureMethod)
				{
					this.TranslateToILObject(il, declaringType, false);
				}
				int num = 0;
				ConstantWrapper constantWrapper;
				if (methodInfo is JSFieldMethod || CustomAttribute.IsDefined(methodInfo, typeof(JSFunctionAttribute), false))
				{
					num = this.PlaceValuesForHiddenParametersOnStack(il, methodInfo, parameters);
					constantWrapper = Binding.JScriptMissingCW;
				}
				else
				{
					constantWrapper = Binding.ReflectionMissingCW;
				}
				if (argList.count == 1 && constantWrapper == Binding.JScriptMissingCW && this.defaultMember is PropertyInfo)
				{
					il.Emit(OpCodes.Ldc_I4_1);
					il.Emit(OpCodes.Newarr, Typeob.Object);
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Ldc_I4_0);
					argList[0].TranslateToIL(il, Typeob.Object);
					il.Emit(OpCodes.Stelem_Ref);
				}
				else
				{
					Binding.PlaceArgumentsOnStack(il, parameters, argList, num, 0, constantWrapper);
				}
				methodInfo = this.GetMethodInfoMetadata(methodInfo);
				if (!this.isNonVirtual && methodInfo.IsVirtual && !methodInfo.IsFinal && (!reflectedType.IsSealed || !reflectedType.IsValueType))
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
			if (memberInfo is ConstructorInfo)
			{
				ConstructorInfo constructorInfo = (ConstructorInfo)memberInfo;
				ParameterInfo[] parameters2 = constructorInfo.GetParameters();
				bool flag = false;
				if (CustomAttribute.IsDefined(constructorInfo, typeof(JSFunctionAttribute), false))
				{
					object[] customAttributes = CustomAttribute.GetCustomAttributes(constructorInfo, typeof(JSFunctionAttribute), false);
					flag = (((JSFunctionAttribute)customAttributes[0]).attributeValue & JSFunctionAttributeEnum.IsInstanceNestedClassConstructor) != JSFunctionAttributeEnum.None;
				}
				if (flag)
				{
					Binding.PlaceArgumentsOnStack(il, parameters2, argList, 0, 1, Binding.ReflectionMissingCW);
					this.TranslateToILObject(il, parameters2[parameters2.Length - 1].ParameterType, false);
				}
				else
				{
					Binding.PlaceArgumentsOnStack(il, parameters2, argList, 0, 0, Binding.ReflectionMissingCW);
				}
				Type type4;
				if (memberInfo is JSConstructor && (type4 = ((JSConstructor)memberInfo).OuterClassType()) != null)
				{
					this.TranslateToILObject(il, type4, false);
				}
				Type declaringType2 = constructorInfo.DeclaringType;
				bool flag2;
				if (constructorInfo is JSConstructor)
				{
					constructorInfo = ((JSConstructor)constructorInfo).GetConstructorInfo(base.compilerGlobals);
					flag2 = true;
				}
				else
				{
					flag2 = Typeob.INeedEngine.IsAssignableFrom(declaringType2);
				}
				il.Emit(OpCodes.Newobj, constructorInfo);
				if (flag2)
				{
					il.Emit(OpCodes.Dup);
					base.EmitILToLoadEngine(il);
					il.Emit(OpCodes.Callvirt, CompilerGlobals.setEngineMethod);
				}
				Convert.Emit(this, il, declaringType2, rtype);
				return;
			}
			Type type5 = memberInfo as Type;
			if (type5 != null)
			{
				AST ast = argList[0];
				if (ast is NullLiteral)
				{
					ast.TranslateToIL(il, type5);
					Convert.Emit(this, il, type5, rtype);
					return;
				}
				IReflect reflect2 = ast.InferType(null);
				if (reflect2 == Typeob.ScriptFunction && Typeob.Delegate.IsAssignableFrom(type5))
				{
					ast.TranslateToIL(il, type5);
				}
				else
				{
					Type type6 = Convert.ToType(reflect2);
					ast.TranslateToIL(il, type6);
					Convert.Emit(this, il, type6, type5, true);
				}
				Convert.Emit(this, il, type5, rtype);
				return;
			}
			else
			{
				if (memberInfo is FieldInfo && ((FieldInfo)memberInfo).IsLiteral)
				{
					object obj = ((memberInfo is JSVariableField) ? ((JSVariableField)memberInfo).value : TypeReferences.GetConstantValue((FieldInfo)memberInfo));
					if (obj is Type || obj is ClassScope || obj is TypedArray)
					{
						AST ast2 = argList[0];
						if (ast2 is NullLiteral)
						{
							il.Emit(OpCodes.Ldnull);
							return;
						}
						ClassScope classScope = obj as ClassScope;
						if (classScope != null)
						{
							EnumDeclaration enumDeclaration = classScope.owner as EnumDeclaration;
							if (enumDeclaration != null)
							{
								obj = enumDeclaration.baseType.ToType();
							}
						}
						Type type7 = Convert.ToType(ast2.InferType(null));
						ast2.TranslateToIL(il, type7);
						Type type8 = ((obj is Type) ? ((Type)obj) : ((obj is ClassScope) ? Convert.ToType((ClassScope)obj) : ((TypedArray)obj).ToType()));
						Convert.Emit(this, il, type7, type8, true);
						if (!rtype.IsEnum)
						{
							Convert.Emit(this, il, type8, rtype);
						}
						return;
					}
				}
				LocalBuilder localBuilder = null;
				int m = 0;
				int count3 = argList.count;
				while (m < count3)
				{
					if (argList[m] is AddressOf)
					{
						localBuilder = il.DeclareLocal(Typeob.ArrayOfObject);
						break;
					}
					m++;
				}
				object obj2 = null;
				if (memberInfo == null && (this.members == null || this.members.Length == 0))
				{
					if (this is Lookup)
					{
						((Lookup)this).TranslateToLateBinding(il);
					}
					else
					{
						obj2 = this.TranslateToSpeculativeEarlyBoundCalls(il, rtype, argList, construct, brackets);
						((Member)this).TranslateToLateBinding(il, obj2 != null);
					}
					argList.TranslateToIL(il, Typeob.ArrayOfObject);
					if (localBuilder != null)
					{
						il.Emit(OpCodes.Dup);
						il.Emit(OpCodes.Stloc, localBuilder);
					}
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
					il.Emit(OpCodes.Call, CompilerGlobals.callMethod);
					Convert.Emit(this, il, Typeob.Object, rtype);
					if (localBuilder != null)
					{
						int n = 0;
						int count4 = argList.count;
						while (n < count4)
						{
							AddressOf addressOf = argList[n] as AddressOf;
							if (addressOf != null)
							{
								addressOf.TranslateToILPreSet(il);
								il.Emit(OpCodes.Ldloc, localBuilder);
								ConstantWrapper.TranslateToILInt(il, n);
								il.Emit(OpCodes.Ldelem_Ref);
								Convert.Emit(this, il, Typeob.Object, Convert.ToType(addressOf.InferType(null)));
								addressOf.TranslateToILSet(il, null);
							}
							n++;
						}
					}
					if (obj2 != null)
					{
						il.MarkLabel((Label)obj2);
					}
					return;
				}
				this.TranslateToILWithDupOfThisOb(il);
				argList.TranslateToIL(il, Typeob.ArrayOfObject);
				if (localBuilder != null)
				{
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Stloc, localBuilder);
				}
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
				il.Emit(OpCodes.Call, CompilerGlobals.callValueMethod);
				Convert.Emit(this, il, Typeob.Object, rtype);
				if (localBuilder != null)
				{
					int num2 = 0;
					int count5 = argList.count;
					while (num2 < count5)
					{
						AddressOf addressOf2 = argList[num2] as AddressOf;
						if (addressOf2 != null)
						{
							addressOf2.TranslateToILPreSet(il);
							il.Emit(OpCodes.Ldloc, localBuilder);
							ConstantWrapper.TranslateToILInt(il, num2);
							il.Emit(OpCodes.Ldelem_Ref);
							Convert.Emit(this, il, Typeob.Object, Convert.ToType(addressOf2.InferType(null)));
							addressOf2.TranslateToILSet(il, null);
						}
						num2++;
					}
				}
				return;
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000BE3C File Offset: 0x0000AE3C
		internal override void TranslateToILDelete(ILGenerator il, Type rtype)
		{
			if (this is Lookup)
			{
				((Lookup)this).TranslateToLateBinding(il);
			}
			else
			{
				((Member)this).TranslateToLateBinding(il, false);
			}
			il.Emit(OpCodes.Call, CompilerGlobals.deleteMethod);
			Convert.Emit(this, il, Typeob.Boolean, rtype);
		}

		// Token: 0x060001AA RID: 426
		protected abstract void TranslateToILObject(ILGenerator il, Type obtype, bool noValue);

		// Token: 0x060001AB RID: 427 RVA: 0x0000BE89 File Offset: 0x0000AE89
		internal override void TranslateToILPreSet(ILGenerator il)
		{
			this.TranslateToIL(il, null, true, false);
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000BE98 File Offset: 0x0000AE98
		internal override void TranslateToILPreSet(ILGenerator il, ASTList argList)
		{
			if (this.isArrayElementAccess)
			{
				this.member = this.defaultMember;
				IReflect reflect = this.defaultMemberReturnIR;
				Type type = ((reflect is Type) ? ((Type)reflect) : Convert.ToType(reflect));
				this.TranslateToIL(il, type);
				int i = 0;
				int count = argList.count;
				while (i < count)
				{
					argList[i].TranslateToIL(il, Typeob.Int32);
					i++;
				}
				if (type.GetArrayRank() == 1)
				{
					Type elementType = type.GetElementType();
					if (elementType.IsValueType && !elementType.IsPrimitive && !elementType.IsEnum)
					{
						il.Emit(OpCodes.Ldelema, elementType);
					}
				}
				return;
			}
			if (this.member is PropertyInfo && this.defaultMember != null)
			{
				PropertyInfo propertyInfo = (PropertyInfo)this.member;
				this.member = this.defaultMember;
				this.TranslateToIL(il, Convert.ToType(this.defaultMemberReturnIR));
				this.member = propertyInfo;
				Binding.PlaceArgumentsOnStack(il, propertyInfo.GetIndexParameters(), argList, 0, 0, Binding.ReflectionMissingCW);
				return;
			}
			base.TranslateToILPreSet(il, argList);
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000BFA7 File Offset: 0x0000AFA7
		internal override void TranslateToILPreSetPlusGet(ILGenerator il)
		{
			this.TranslateToIL(il, Convert.ToType(this.InferType(null)), false, true);
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000BFC0 File Offset: 0x0000AFC0
		internal override void TranslateToILPreSetPlusGet(ILGenerator il, ASTList argList, bool inBrackets)
		{
			if (this.isArrayElementAccess)
			{
				this.member = this.defaultMember;
				IReflect reflect = this.defaultMemberReturnIR;
				Type type = ((reflect is Type) ? ((Type)reflect) : Convert.ToType(reflect));
				this.TranslateToIL(il, type);
				il.Emit(OpCodes.Dup);
				int arrayRank = type.GetArrayRank();
				LocalBuilder[] array = new LocalBuilder[arrayRank];
				int i = 0;
				int count = argList.count;
				while (i < count)
				{
					argList[i].TranslateToIL(il, Typeob.Int32);
					array[i] = il.DeclareLocal(Typeob.Int32);
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Stloc, array[i]);
					i++;
				}
				Type elementType = type.GetElementType();
				if (arrayRank == 1)
				{
					Binding.TranslateToLdelem(il, elementType);
				}
				else
				{
					Type[] array2 = new Type[arrayRank];
					for (int j = 0; j < arrayRank; j++)
					{
						array2[j] = Typeob.Int32;
					}
					MethodInfo method = type.GetMethod("Get", array2);
					il.Emit(OpCodes.Call, method);
				}
				LocalBuilder localBuilder = il.DeclareLocal(elementType);
				il.Emit(OpCodes.Stloc, localBuilder);
				for (int k = 0; k < arrayRank; k++)
				{
					il.Emit(OpCodes.Ldloc, array[k]);
				}
				if (arrayRank == 1 && elementType.IsValueType && !elementType.IsPrimitive)
				{
					il.Emit(OpCodes.Ldelema, elementType);
				}
				il.Emit(OpCodes.Ldloc, localBuilder);
				return;
			}
			if (this.member != null && this.defaultMember != null)
			{
				this.member = this.defaultMember;
				this.defaultMember = null;
			}
			base.TranslateToILPreSetPlusGet(il, argList, inBrackets);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000C160 File Offset: 0x0000B160
		internal override object TranslateToILReference(ILGenerator il, Type rtype)
		{
			if (this.member is FieldInfo)
			{
				FieldInfo fieldInfo = (FieldInfo)this.member;
				Type fieldType = fieldInfo.FieldType;
				if (rtype == fieldType)
				{
					bool isStatic = fieldInfo.IsStatic;
					if (!isStatic)
					{
						this.TranslateToILObject(il, fieldInfo.DeclaringType, true);
					}
					object obj = ((fieldInfo is JSField) ? ((JSField)fieldInfo).GetMetaData() : ((fieldInfo is JSFieldInfo) ? ((JSFieldInfo)fieldInfo).field : fieldInfo));
					if (obj is FieldInfo)
					{
						if (fieldInfo.IsInitOnly)
						{
							LocalBuilder localBuilder = il.DeclareLocal(fieldType);
							il.Emit(isStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, (FieldInfo)obj);
							il.Emit(OpCodes.Stloc, localBuilder);
							il.Emit(OpCodes.Ldloca, localBuilder);
						}
						else
						{
							il.Emit(isStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, (FieldInfo)obj);
						}
					}
					else if (obj is LocalBuilder)
					{
						il.Emit(OpCodes.Ldloca, (LocalBuilder)obj);
					}
					else
					{
						il.Emit(OpCodes.Ldarga, (short)obj);
					}
					return null;
				}
			}
			return base.TranslateToILReference(il, rtype);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000C280 File Offset: 0x0000B280
		internal override void TranslateToILSet(ILGenerator il, AST rhvalue)
		{
			if (this.isArrayElementAccess)
			{
				IReflect reflect = this.defaultMemberReturnIR;
				Type type = ((reflect is Type) ? ((Type)reflect) : Convert.ToType(reflect));
				int arrayRank = type.GetArrayRank();
				Type elementType = type.GetElementType();
				if (rhvalue != null)
				{
					rhvalue.TranslateToIL(il, elementType);
				}
				if (arrayRank == 1)
				{
					Binding.TranslateToStelem(il, elementType);
					return;
				}
				Type[] array = new Type[arrayRank + 1];
				for (int i = 0; i < arrayRank; i++)
				{
					array[i] = Typeob.Int32;
				}
				array[arrayRank] = elementType;
				MethodInfo arrayMethod = base.compilerGlobals.module.GetArrayMethod(type, "Set", CallingConventions.HasThis, Typeob.Void, array);
				il.Emit(OpCodes.Call, arrayMethod);
				return;
			}
			else
			{
				if (this.isAssignmentToDefaultIndexedProperty)
				{
					if (this.member is PropertyInfo && this.defaultMember != null)
					{
						PropertyInfo propertyInfo = (PropertyInfo)this.member;
						MethodInfo methodInfo = JSProperty.GetSetMethod(propertyInfo, false);
						JSWrappedMethod jswrappedMethod = methodInfo as JSWrappedMethod;
						if (jswrappedMethod == null || !(jswrappedMethod.GetWrappedObject() is GlobalObject))
						{
							methodInfo = this.GetMethodInfoMetadata(methodInfo);
							if (rhvalue != null)
							{
								rhvalue.TranslateToIL(il, propertyInfo.PropertyType);
							}
							if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!methodInfo.ReflectedType.IsSealed || !methodInfo.ReflectedType.IsValueType))
							{
								il.Emit(OpCodes.Callvirt, methodInfo);
								return;
							}
							il.Emit(OpCodes.Call, methodInfo);
							return;
						}
					}
					base.TranslateToILSet(il, rhvalue);
					return;
				}
				if (this.member is FieldInfo)
				{
					FieldInfo fieldInfo = (FieldInfo)this.member;
					if (rhvalue != null)
					{
						rhvalue.TranslateToIL(il, fieldInfo.FieldType);
					}
					if (fieldInfo.IsLiteral || fieldInfo.IsInitOnly)
					{
						il.Emit(OpCodes.Pop);
						return;
					}
					object obj = ((fieldInfo is JSField) ? ((JSField)fieldInfo).GetMetaData() : ((fieldInfo is JSFieldInfo) ? ((JSFieldInfo)fieldInfo).field : fieldInfo));
					FieldInfo fieldInfo2 = obj as FieldInfo;
					if (fieldInfo2 != null)
					{
						il.Emit(fieldInfo2.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, fieldInfo2);
						return;
					}
					if (obj is LocalBuilder)
					{
						il.Emit(OpCodes.Stloc, (LocalBuilder)obj);
						return;
					}
					il.Emit(OpCodes.Starg, (short)obj);
					return;
				}
				else
				{
					if (!(this.member is PropertyInfo))
					{
						object obj2 = this.TranslateToSpeculativeEarlyBoundSet(il, rhvalue);
						if (rhvalue != null)
						{
							rhvalue.TranslateToIL(il, Typeob.Object);
						}
						il.Emit(OpCodes.Call, CompilerGlobals.setValueMethod);
						if (obj2 != null)
						{
							il.MarkLabel((Label)obj2);
						}
						return;
					}
					PropertyInfo propertyInfo2 = (PropertyInfo)this.member;
					if (rhvalue != null)
					{
						rhvalue.TranslateToIL(il, propertyInfo2.PropertyType);
					}
					MethodInfo methodInfo2 = JSProperty.GetSetMethod(propertyInfo2, true);
					if (methodInfo2 == null)
					{
						il.Emit(OpCodes.Pop);
						return;
					}
					methodInfo2 = this.GetMethodInfoMetadata(methodInfo2);
					if (methodInfo2.IsStatic && !(methodInfo2 is JSClosureMethod))
					{
						il.Emit(OpCodes.Call, methodInfo2);
						return;
					}
					if (!this.isNonVirtual && methodInfo2.IsVirtual && !methodInfo2.IsFinal && (!methodInfo2.ReflectedType.IsSealed || !methodInfo2.ReflectedType.IsValueType))
					{
						il.Emit(OpCodes.Callvirt, methodInfo2);
						return;
					}
					il.Emit(OpCodes.Call, methodInfo2);
					return;
				}
			}
		}

		// Token: 0x060001B1 RID: 433
		protected abstract void TranslateToILWithDupOfThisOb(ILGenerator il);

		// Token: 0x060001B2 RID: 434 RVA: 0x0000C5D0 File Offset: 0x0000B5D0
		private static void TranslateToLdelem(ILGenerator il, Type etype)
		{
			switch (Type.GetTypeCode(etype))
			{
			case TypeCode.Object:
			case TypeCode.Decimal:
			case TypeCode.DateTime:
			case TypeCode.String:
				if (etype.IsValueType)
				{
					il.Emit(OpCodes.Ldelema, etype);
					il.Emit(OpCodes.Ldobj, etype);
					return;
				}
				il.Emit(OpCodes.Ldelem_Ref);
				break;
			case TypeCode.DBNull:
			case (TypeCode)17:
				break;
			case TypeCode.Boolean:
			case TypeCode.Byte:
				il.Emit(OpCodes.Ldelem_U1);
				return;
			case TypeCode.Char:
			case TypeCode.UInt16:
				il.Emit(OpCodes.Ldelem_U2);
				return;
			case TypeCode.SByte:
				il.Emit(OpCodes.Ldelem_I1);
				return;
			case TypeCode.Int16:
				il.Emit(OpCodes.Ldelem_I2);
				return;
			case TypeCode.Int32:
				il.Emit(OpCodes.Ldelem_I4);
				return;
			case TypeCode.UInt32:
				il.Emit(OpCodes.Ldelem_U4);
				return;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				il.Emit(OpCodes.Ldelem_I8);
				return;
			case TypeCode.Single:
				il.Emit(OpCodes.Ldelem_R4);
				return;
			case TypeCode.Double:
				il.Emit(OpCodes.Ldelem_R8);
				return;
			default:
				return;
			}
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000C6D0 File Offset: 0x0000B6D0
		private object TranslateToSpeculativeEarlyBoundSet(ILGenerator il, AST rhvalue)
		{
			this.giveErrors = false;
			object obj = null;
			bool flag = true;
			LocalBuilder localBuilder = null;
			LocalBuilder localBuilder2 = null;
			Label label = il.DefineLabel();
			MemberInfoList allKnownInstanceBindingsForThisName = this.GetAllKnownInstanceBindingsForThisName();
			int i = 0;
			int count = allKnownInstanceBindingsForThisName.count;
			while (i < count)
			{
				MemberInfo memberInfo = allKnownInstanceBindingsForThisName[i];
				FieldInfo fieldInfo = null;
				MethodInfo methodInfo = null;
				PropertyInfo propertyInfo = null;
				if (memberInfo is FieldInfo)
				{
					fieldInfo = (FieldInfo)memberInfo;
					if (!fieldInfo.IsLiteral)
					{
						if (!fieldInfo.IsInitOnly)
						{
							goto IL_00A4;
						}
					}
				}
				else if (memberInfo is PropertyInfo)
				{
					propertyInfo = (PropertyInfo)memberInfo;
					if (propertyInfo.GetIndexParameters().Length <= 0 && (methodInfo = JSProperty.GetSetMethod(propertyInfo, true)) != null)
					{
						goto IL_00A4;
					}
				}
				IL_02A8:
				i++;
				continue;
				IL_00A4:
				this.member = memberInfo;
				if (this.Accessible(true))
				{
					if (flag)
					{
						flag = false;
						if (rhvalue == null)
						{
							localBuilder2 = il.DeclareLocal(Typeob.Object);
							il.Emit(OpCodes.Stloc, localBuilder2);
						}
						il.Emit(OpCodes.Dup);
						il.Emit(OpCodes.Ldfld, CompilerGlobals.objectField);
						localBuilder = il.DeclareLocal(Typeob.Object);
						il.Emit(OpCodes.Stloc, localBuilder);
						obj = il.DefineLabel();
					}
					Type declaringType = memberInfo.DeclaringType;
					il.Emit(OpCodes.Ldloc, localBuilder);
					il.Emit(OpCodes.Isinst, declaringType);
					LocalBuilder localBuilder3 = il.DeclareLocal(declaringType);
					il.Emit(OpCodes.Dup);
					il.Emit(OpCodes.Stloc, localBuilder3);
					il.Emit(OpCodes.Brfalse, label);
					il.Emit(OpCodes.Ldloc, localBuilder3);
					if (rhvalue == null)
					{
						il.Emit(OpCodes.Ldloc, localBuilder2);
					}
					if (fieldInfo != null)
					{
						if (rhvalue == null)
						{
							Convert.Emit(this, il, Typeob.Object, fieldInfo.FieldType);
						}
						else
						{
							rhvalue.TranslateToIL(il, fieldInfo.FieldType);
						}
						if (fieldInfo is JSField)
						{
							il.Emit(OpCodes.Stfld, (FieldInfo)((JSField)fieldInfo).GetMetaData());
						}
						else if (fieldInfo is JSFieldInfo)
						{
							il.Emit(OpCodes.Stfld, ((JSFieldInfo)fieldInfo).field);
						}
						else
						{
							il.Emit(OpCodes.Stfld, fieldInfo);
						}
					}
					else
					{
						if (rhvalue == null)
						{
							Convert.Emit(this, il, Typeob.Object, propertyInfo.PropertyType);
						}
						else
						{
							rhvalue.TranslateToIL(il, propertyInfo.PropertyType);
						}
						methodInfo = this.GetMethodInfoMetadata(methodInfo);
						if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!declaringType.IsSealed || !declaringType.IsValueType))
						{
							il.Emit(OpCodes.Callvirt, methodInfo);
						}
						else
						{
							il.Emit(OpCodes.Call, methodInfo);
						}
					}
					il.Emit(OpCodes.Pop);
					il.Emit(OpCodes.Br, (Label)obj);
					il.MarkLabel(label);
					label = il.DefineLabel();
					goto IL_02A8;
				}
				goto IL_02A8;
			}
			if (localBuilder2 != null)
			{
				il.Emit(OpCodes.Ldloc, localBuilder2);
			}
			this.member = null;
			return obj;
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000C9AC File Offset: 0x0000B9AC
		private object TranslateToSpeculativeEarlyBindings(ILGenerator il, Type rtype, bool getObjectFromLateBindingInstance)
		{
			this.giveErrors = false;
			object obj = null;
			bool flag = true;
			LocalBuilder localBuilder = null;
			Label label = il.DefineLabel();
			MemberInfoList allKnownInstanceBindingsForThisName = this.GetAllKnownInstanceBindingsForThisName();
			int i = 0;
			int count = allKnownInstanceBindingsForThisName.count;
			while (i < count)
			{
				MemberInfo memberInfo = allKnownInstanceBindingsForThisName[i];
				if (memberInfo is FieldInfo || (memberInfo is PropertyInfo && ((PropertyInfo)memberInfo).GetIndexParameters().Length <= 0 && JSProperty.GetGetMethod((PropertyInfo)memberInfo, true) != null))
				{
					this.member = memberInfo;
					if (this.Accessible(false))
					{
						if (flag)
						{
							flag = false;
							if (getObjectFromLateBindingInstance)
							{
								il.Emit(OpCodes.Dup);
								il.Emit(OpCodes.Ldfld, CompilerGlobals.objectField);
							}
							else
							{
								this.TranslateToILObject(il, Typeob.Object, false);
							}
							localBuilder = il.DeclareLocal(Typeob.Object);
							il.Emit(OpCodes.Stloc, localBuilder);
							obj = il.DefineLabel();
						}
						Type declaringType = memberInfo.DeclaringType;
						il.Emit(OpCodes.Ldloc, localBuilder);
						il.Emit(OpCodes.Isinst, declaringType);
						LocalBuilder localBuilder2 = il.DeclareLocal(declaringType);
						il.Emit(OpCodes.Dup);
						il.Emit(OpCodes.Stloc, localBuilder2);
						il.Emit(OpCodes.Brfalse_S, label);
						il.Emit(OpCodes.Ldloc, localBuilder2);
						if (memberInfo is FieldInfo)
						{
							FieldInfo fieldInfo = (FieldInfo)memberInfo;
							if (fieldInfo.IsLiteral)
							{
								il.Emit(OpCodes.Pop);
								goto IL_025F;
							}
							if (fieldInfo is JSField)
							{
								il.Emit(OpCodes.Ldfld, (FieldInfo)((JSField)fieldInfo).GetMetaData());
							}
							else if (fieldInfo is JSFieldInfo)
							{
								il.Emit(OpCodes.Ldfld, ((JSFieldInfo)fieldInfo).field);
							}
							else
							{
								il.Emit(OpCodes.Ldfld, fieldInfo);
							}
							Convert.Emit(this, il, fieldInfo.FieldType, rtype);
						}
						else if (memberInfo is PropertyInfo)
						{
							MethodInfo methodInfo = JSProperty.GetGetMethod((PropertyInfo)memberInfo, true);
							methodInfo = this.GetMethodInfoMetadata(methodInfo);
							if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!declaringType.IsSealed || declaringType.IsValueType))
							{
								il.Emit(OpCodes.Callvirt, methodInfo);
							}
							else
							{
								il.Emit(OpCodes.Call, methodInfo);
							}
							Convert.Emit(this, il, methodInfo.ReturnType, rtype);
						}
						il.Emit(OpCodes.Br, (Label)obj);
						il.MarkLabel(label);
						label = il.DefineLabel();
					}
				}
				IL_025F:
				i++;
			}
			il.MarkLabel(label);
			if (!flag && !getObjectFromLateBindingInstance)
			{
				il.Emit(OpCodes.Ldloc, localBuilder);
			}
			this.member = null;
			return obj;
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000CC48 File Offset: 0x0000BC48
		private object TranslateToSpeculativeEarlyBoundCalls(ILGenerator il, Type rtype, ASTList argList, bool construct, bool brackets)
		{
			this.giveErrors = false;
			object obj = null;
			bool flag = true;
			LocalBuilder localBuilder = null;
			Label label = il.DefineLabel();
			IReflect[] allEligibleClasses = this.GetAllEligibleClasses();
			if (construct)
			{
				return obj;
			}
			foreach (IReflect reflect in allEligibleClasses)
			{
				MemberInfo[] array2 = reflect.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
				try
				{
					MemberInfo memberInfo = JSBinder.SelectCallableMember(array2, this.argIRs);
					MethodInfo methodInfo;
					if (memberInfo != null && memberInfo.MemberType == MemberTypes.Property)
					{
						methodInfo = ((PropertyInfo)memberInfo).GetGetMethod(true);
						ParameterInfo[] parameters;
						if (methodInfo == null || (parameters = methodInfo.GetParameters()) == null || parameters.Length == 0)
						{
							goto IL_0274;
						}
					}
					else
					{
						methodInfo = memberInfo as MethodInfo;
					}
					if (methodInfo != null)
					{
						if (Binding.CheckParameters(methodInfo.GetParameters(), this.argIRs, argList, this.context, 0, true, false))
						{
							if (methodInfo is JSFieldMethod)
							{
								FunctionObject func = ((JSFieldMethod)methodInfo).func;
								if (func != null && (func.attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope && ((ClassScope)reflect).ParentIsInSamePackage())
								{
									goto IL_0274;
								}
							}
							else if (methodInfo is JSWrappedMethod && ((JSWrappedMethod)methodInfo).obj is ClassScope && ((JSWrappedMethod)methodInfo).GetPackage() == ((ClassScope)reflect).package)
							{
								goto IL_0274;
							}
							this.member = methodInfo;
							if (this.Accessible(false))
							{
								if (flag)
								{
									flag = false;
									this.TranslateToILObject(il, Typeob.Object, false);
									localBuilder = il.DeclareLocal(Typeob.Object);
									il.Emit(OpCodes.Stloc, localBuilder);
									obj = il.DefineLabel();
								}
								Type declaringType = methodInfo.DeclaringType;
								il.Emit(OpCodes.Ldloc, localBuilder);
								il.Emit(OpCodes.Isinst, declaringType);
								LocalBuilder localBuilder2 = il.DeclareLocal(declaringType);
								il.Emit(OpCodes.Dup);
								il.Emit(OpCodes.Stloc, localBuilder2);
								il.Emit(OpCodes.Brfalse, label);
								il.Emit(OpCodes.Ldloc, localBuilder2);
								Binding.PlaceArgumentsOnStack(il, methodInfo.GetParameters(), argList, 0, 0, Binding.ReflectionMissingCW);
								methodInfo = this.GetMethodInfoMetadata(methodInfo);
								if (methodInfo.IsVirtual && !methodInfo.IsFinal && (!declaringType.IsSealed || declaringType.IsValueType))
								{
									il.Emit(OpCodes.Callvirt, methodInfo);
								}
								else
								{
									il.Emit(OpCodes.Call, methodInfo);
								}
								Convert.Emit(this, il, methodInfo.ReturnType, rtype);
								il.Emit(OpCodes.Br, (Label)obj);
								il.MarkLabel(label);
								label = il.DefineLabel();
							}
						}
					}
				}
				catch (AmbiguousMatchException)
				{
				}
				IL_0274:;
			}
			il.MarkLabel(label);
			if (!flag)
			{
				il.Emit(OpCodes.Ldloc, localBuilder);
			}
			this.member = null;
			return obj;
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000CF14 File Offset: 0x0000BF14
		internal static void TranslateToStelem(ILGenerator il, Type etype)
		{
			switch (Type.GetTypeCode(etype))
			{
			case TypeCode.Object:
			case TypeCode.Decimal:
			case TypeCode.DateTime:
			case TypeCode.String:
				if (etype.IsValueType)
				{
					il.Emit(OpCodes.Stobj, etype);
					return;
				}
				il.Emit(OpCodes.Stelem_Ref);
				break;
			case TypeCode.DBNull:
			case (TypeCode)17:
				break;
			case TypeCode.Boolean:
			case TypeCode.SByte:
			case TypeCode.Byte:
				il.Emit(OpCodes.Stelem_I1);
				return;
			case TypeCode.Char:
			case TypeCode.Int16:
			case TypeCode.UInt16:
				il.Emit(OpCodes.Stelem_I2);
				return;
			case TypeCode.Int32:
			case TypeCode.UInt32:
				il.Emit(OpCodes.Stelem_I4);
				return;
			case TypeCode.Int64:
			case TypeCode.UInt64:
				il.Emit(OpCodes.Stelem_I8);
				return;
			case TypeCode.Single:
				il.Emit(OpCodes.Stelem_R4);
				return;
			case TypeCode.Double:
				il.Emit(OpCodes.Stelem_R8);
				return;
			default:
				return;
			}
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000CFE4 File Offset: 0x0000BFE4
		private void WarnIfNotFullyResolved()
		{
			if (this.isFullyResolved || this.member == null)
			{
				return;
			}
			if (this.member is JSVariableField && ((JSVariableField)this.member).type == null)
			{
				return;
			}
			if (!base.Engine.doFast && this.member is IWrappedMember)
			{
				return;
			}
			for (ScriptObject scriptObject = base.Globals.ScopeStack.Peek(); scriptObject != null; scriptObject = scriptObject.GetParent())
			{
				if (scriptObject is WithObject && !((WithObject)scriptObject).isKnownAtCompileTime)
				{
					this.context.HandleError(JSError.AmbiguousBindingBecauseOfWith);
					return;
				}
				if (scriptObject is ActivationObject && !((ActivationObject)scriptObject).isKnownAtCompileTime)
				{
					this.context.HandleError(JSError.AmbiguousBindingBecauseOfEval);
					return;
				}
			}
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000D0A6 File Offset: 0x0000C0A6
		private void WarnIfObsolete()
		{
			Binding.WarnIfObsolete(this.member, this.context);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000D0BC File Offset: 0x0000C0BC
		internal static void WarnIfObsolete(MemberInfo member, Context context)
		{
			if (member == null)
			{
				return;
			}
			object[] array = CustomAttribute.GetCustomAttributes(member, typeof(ObsoleteAttribute), false);
			string text;
			bool flag;
			if (array != null && array.Length > 0)
			{
				ObsoleteAttribute obsoleteAttribute = (ObsoleteAttribute)array[0];
				text = obsoleteAttribute.Message;
				flag = obsoleteAttribute.IsError;
			}
			else
			{
				array = CustomAttribute.GetCustomAttributes(member, typeof(NotRecommended), false);
				if (array == null || array.Length <= 0)
				{
					return;
				}
				NotRecommended notRecommended = (NotRecommended)array[0];
				text = ": " + notRecommended.Message;
				flag = false;
			}
			context.HandleError(JSError.Deprecated, text, flag);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000D14E File Offset: 0x0000C14E
		private MethodInfo GetMethodInfoMetadata(MethodInfo method)
		{
			if (method is JSMethod)
			{
				return ((JSMethod)method).GetMethodInfo(base.compilerGlobals);
			}
			if (method is JSMethodInfo)
			{
				return ((JSMethodInfo)method).method;
			}
			return method;
		}

		// Token: 0x0400007F RID: 127
		private IReflect[] argIRs;

		// Token: 0x04000080 RID: 128
		protected MemberInfo defaultMember;

		// Token: 0x04000081 RID: 129
		private IReflect defaultMemberReturnIR;

		// Token: 0x04000082 RID: 130
		private bool isArrayElementAccess;

		// Token: 0x04000083 RID: 131
		private bool isArrayConstructor;

		// Token: 0x04000084 RID: 132
		protected bool isAssignmentToDefaultIndexedProperty;

		// Token: 0x04000085 RID: 133
		protected bool isFullyResolved;

		// Token: 0x04000086 RID: 134
		protected bool isNonVirtual;

		// Token: 0x04000087 RID: 135
		internal MemberInfo[] members;

		// Token: 0x04000088 RID: 136
		internal MemberInfo member;

		// Token: 0x04000089 RID: 137
		protected string name;

		// Token: 0x0400008A RID: 138
		private bool giveErrors;

		// Token: 0x0400008B RID: 139
		internal static ConstantWrapper ReflectionMissingCW = new ConstantWrapper(Missing.Value, null);

		// Token: 0x0400008C RID: 140
		private static ConstantWrapper JScriptMissingCW = new ConstantWrapper(Missing.Value, null);
	}
}
