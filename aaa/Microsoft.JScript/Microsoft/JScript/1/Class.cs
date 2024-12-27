using System;
using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000038 RID: 56
	internal class Class : AST
	{
		// Token: 0x06000230 RID: 560 RVA: 0x000111B0 File Offset: 0x000101B0
		internal Class(Context context, AST id, TypeExpression superTypeExpression, TypeExpression[] interfaces, Block body, FieldAttributes attributes, bool isAbstract, bool isFinal, bool isStatic, bool isInterface, CustomAttributeList customAttributes)
			: base(context)
		{
			this.name = id.ToString();
			this.superTypeExpression = superTypeExpression;
			this.interfaces = interfaces;
			this.body = body;
			this.enclosingScope = (ScriptObject)base.Globals.ScopeStack.Peek(1);
			this.attributes = TypeAttributes.Serializable;
			this.SetAccessibility(attributes);
			if (isAbstract)
			{
				this.attributes |= TypeAttributes.Abstract;
			}
			this.isAbstract = isAbstract || isInterface;
			this.isAlreadyPartiallyEvaluated = false;
			if (isFinal)
			{
				this.attributes |= TypeAttributes.Sealed;
			}
			if (isInterface)
			{
				this.attributes |= TypeAttributes.ClassSemanticsMask | TypeAttributes.Abstract;
			}
			this.isCooked = false;
			this.cookedType = null;
			this.isExpando = false;
			this.isInterface = isInterface;
			this.isStatic = isStatic;
			this.needsEngine = !isInterface;
			this.validOn = (AttributeTargets)0;
			this.allowMultiple = true;
			this.classob = (ClassScope)base.Globals.ScopeStack.Peek();
			this.classob.name = this.name;
			this.classob.owner = this;
			this.implicitDefaultConstructor = null;
			if (!isInterface && !(this is EnumDeclaration))
			{
				this.SetupConstructors();
			}
			this.EnterNameIntoEnclosingScopeAndGetOwnField(id, isStatic);
			this.fields = this.classob.GetMemberFields();
			this.superClass = null;
			this.superIR = null;
			this.superMembers = null;
			this.firstIndex = null;
			this.fieldInitializer = null;
			this.customAttributes = customAttributes;
			this.clsCompliance = CLSComplianceSpec.NotAttributed;
			this.generateCodeForExpando = false;
			this.expandoItemProp = null;
			this.getHashTableMethod = null;
			this.getItem = null;
			this.setItem = null;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0001136C File Offset: 0x0001036C
		private void AddImplicitInterfaces(IReflect iface, IReflect[] explicitInterfaces, ArrayList implicitInterfaces)
		{
			Type type = iface as Type;
			if (type == null)
			{
				foreach (TypeExpression typeExpression in ((ClassScope)iface).owner.interfaces)
				{
					IReflect reflect = typeExpression.ToIReflect();
					if (Array.IndexOf<IReflect>(explicitInterfaces, reflect, 0) >= 0)
					{
						return;
					}
					if (implicitInterfaces.IndexOf(reflect, 0) >= 0)
					{
						return;
					}
					implicitInterfaces.Add(reflect);
				}
				return;
			}
			Type[] array2 = type.GetInterfaces();
			foreach (Type type2 in array2)
			{
				if (Array.IndexOf(explicitInterfaces, type2, 0) >= 0)
				{
					break;
				}
				if (implicitInterfaces.IndexOf(type2, 0) >= 0)
				{
					break;
				}
				implicitInterfaces.Add(type2);
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x00011424 File Offset: 0x00010424
		private void AllocateImplicitDefaultConstructor()
		{
			this.implicitDefaultConstructor = new FunctionObject(".ctor", new ParameterDeclaration[0], null, new Block(this.context), new FunctionScope(this.classob, true), this.classob, this.context, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask, null, true);
			this.implicitDefaultConstructor.isImplicitCtor = true;
			this.implicitDefaultConstructor.isConstructor = true;
			this.implicitDefaultConstructor.proto = this.classob;
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0001149C File Offset: 0x0001049C
		private bool CanSee(MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType <= MemberTypes.Method)
			{
				switch (memberType)
				{
				case MemberTypes.Event:
				{
					MethodBase addMethod = ((EventInfo)member).GetAddMethod();
					if (addMethod == null)
					{
						return false;
					}
					MethodAttributes methodAttributes = addMethod.Attributes & MethodAttributes.MemberAccessMask;
					return methodAttributes != MethodAttributes.Private && methodAttributes != MethodAttributes.PrivateScope && methodAttributes != MethodAttributes.FamANDAssem && (methodAttributes != MethodAttributes.Assembly || this.IsInTheSamePackage(member));
				}
				case MemberTypes.Constructor | MemberTypes.Event:
					break;
				case MemberTypes.Field:
				{
					FieldAttributes fieldAttributes = ((FieldInfo)member).Attributes & FieldAttributes.FieldAccessMask;
					return fieldAttributes != FieldAttributes.Private && fieldAttributes != FieldAttributes.PrivateScope && fieldAttributes != FieldAttributes.FamANDAssem && (fieldAttributes != FieldAttributes.Assembly || this.IsInTheSamePackage(member));
				}
				default:
					if (memberType == MemberTypes.Method)
					{
						MethodAttributes methodAttributes2 = ((MethodBase)member).Attributes & MethodAttributes.MemberAccessMask;
						return methodAttributes2 != MethodAttributes.Private && methodAttributes2 != MethodAttributes.PrivateScope && methodAttributes2 != MethodAttributes.FamANDAssem && (methodAttributes2 != MethodAttributes.Assembly || this.IsInTheSamePackage(member));
					}
					break;
				}
			}
			else if (memberType != MemberTypes.Property)
			{
				if (memberType == MemberTypes.TypeInfo || memberType == MemberTypes.NestedType)
				{
					TypeAttributes typeAttributes = ((Type)member).Attributes & TypeAttributes.VisibilityMask;
					return typeAttributes != TypeAttributes.NestedPrivate && typeAttributes != TypeAttributes.NestedFamANDAssem && (typeAttributes != TypeAttributes.NestedAssembly || this.IsInTheSamePackage(member));
				}
			}
			else
			{
				MethodBase methodBase = JSProperty.GetGetMethod((PropertyInfo)member, true);
				if (methodBase == null)
				{
					methodBase = JSProperty.GetSetMethod((PropertyInfo)member, true);
				}
				if (methodBase == null)
				{
					return false;
				}
				MethodAttributes methodAttributes3 = methodBase.Attributes & MethodAttributes.MemberAccessMask;
				return methodAttributes3 != MethodAttributes.Private && methodAttributes3 != MethodAttributes.PrivateScope && methodAttributes3 != MethodAttributes.FamANDAssem && (methodAttributes3 != MethodAttributes.Assembly || this.IsInTheSamePackage(member));
			}
			return true;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x000115F8 File Offset: 0x000105F8
		private void CheckFieldDeclarationConsistency(JSMemberField field)
		{
			object obj = this.firstIndex[field.Name];
			if (obj == null)
			{
				return;
			}
			int i = (int)obj;
			int num = this.superMembers.Length;
			while (i < num)
			{
				object obj2 = this.superMembers[i];
				if (!(obj2 is MemberInfo))
				{
					return;
				}
				MemberInfo memberInfo = (MemberInfo)obj2;
				if (!memberInfo.Name.Equals(field.Name))
				{
					return;
				}
				if (this.CanSee(memberInfo))
				{
					string fullNameFor = this.GetFullNameFor(memberInfo);
					field.originalContext.HandleError(JSError.HidesParentMember, fullNameFor, this.IsInTheSameCompilationUnit(memberInfo));
					return;
				}
				i++;
			}
		}

		// Token: 0x06000235 RID: 565 RVA: 0x00011694 File Offset: 0x00010694
		private void CheckIfOKToGenerateCodeForExpando(bool superClassIsExpando)
		{
			if (superClassIsExpando)
			{
				this.context.HandleError(JSError.BaseClassIsExpandoAlready);
				this.generateCodeForExpando = false;
				return;
			}
			if (this.classob.GetMember("Item", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
			{
				this.context.HandleError(JSError.ItemNotAllowedOnExpandoClass);
				this.generateCodeForExpando = false;
				return;
			}
			if (this.classob.GetMember("get_Item", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length > 0 || this.classob.GetMember("set_Item", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
			{
				this.context.HandleError(JSError.MethodNotAllowedOnExpandoClass);
				this.generateCodeForExpando = false;
				return;
			}
			if (this.ImplementsInterface(Typeob.IEnumerable))
			{
				this.context.HandleError(JSError.ExpandoClassShouldNotImpleEnumerable);
				this.generateCodeForExpando = false;
				return;
			}
			if (this.superIR.GetMember("Item", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0 || this.superIR.GetMember("get_Item", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0 || this.superIR.GetMember("set_Item", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
			{
				this.context.HandleError(JSError.MethodClashOnExpandoSuperClass);
				this.generateCodeForExpando = false;
				return;
			}
			JSProperty jsproperty = (this.classob.itemProp = new JSProperty("Item"));
			jsproperty.getter = new JSExpandoIndexerMethod(this.classob, true);
			jsproperty.setter = new JSExpandoIndexerMethod(this.classob, false);
			this.classob.AddNewField("Item", jsproperty, FieldAttributes.Literal);
		}

		// Token: 0x06000236 RID: 566 RVA: 0x00011804 File Offset: 0x00010804
		private string GetFullName()
		{
			string rootNamespace = ((ActivationObject)this.enclosingScope).GetName();
			if (rootNamespace == null)
			{
				VsaEngine engine = this.context.document.engine;
				if (engine != null && engine.genStartupClass)
				{
					rootNamespace = engine.RootNamespace;
				}
			}
			if (rootNamespace != null)
			{
				return rootNamespace + "." + this.name;
			}
			return this.name;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x00011864 File Offset: 0x00010864
		protected void CheckMemberNamesForCLSCompliance()
		{
			if (!(this.enclosingScope is ClassScope))
			{
				base.Engine.CheckTypeNameForCLSCompliance(this.name, this.GetFullName(), this.context);
			}
			Hashtable hashtable = new Hashtable(StringComparer.OrdinalIgnoreCase);
			int i = 0;
			int num = this.fields.Length;
			while (i < num)
			{
				JSMemberField jsmemberField = this.fields[i];
				if (!jsmemberField.IsPrivate)
				{
					if (!VsaEngine.CheckIdentifierForCLSCompliance(jsmemberField.Name))
					{
						jsmemberField.originalContext.HandleError(JSError.NonCLSCompliantMember);
					}
					else if ((JSMemberField)hashtable[jsmemberField.Name] == null)
					{
						hashtable.Add(jsmemberField.Name, jsmemberField);
					}
					else
					{
						jsmemberField.originalContext.HandleError(JSError.NonCLSCompliantMember);
					}
				}
				i++;
			}
		}

		// Token: 0x06000238 RID: 568 RVA: 0x00011920 File Offset: 0x00010920
		private void CheckIfValidExtensionOfSuperType()
		{
			this.GetIRForSuperType();
			ClassScope classScope = this.superIR as ClassScope;
			if (classScope != null)
			{
				if (this.IsStatic)
				{
					if (!classScope.owner.IsStatic)
					{
						this.superTypeExpression.context.HandleError(JSError.NestedInstanceTypeCannotBeExtendedByStatic);
						this.superIR = Typeob.Object;
						this.superTypeExpression = null;
					}
				}
				else if (!classScope.owner.IsStatic && this.enclosingScope != classScope.owner.enclosingScope)
				{
					this.superTypeExpression.context.HandleError(JSError.NestedInstanceTypeCannotBeExtendedByStatic);
					this.superIR = Typeob.Object;
					this.superTypeExpression = null;
				}
			}
			this.GetSuperTypeMembers();
			this.GetStartIndexForEachName();
			bool flag = this.NeedsToBeCheckedForCLSCompliance();
			if (flag)
			{
				this.CheckMemberNamesForCLSCompliance();
			}
			int i = 0;
			int num = this.fields.Length;
			while (i < num)
			{
				JSMemberField jsmemberField = this.fields[i];
				if (jsmemberField.IsLiteral)
				{
					object obj = jsmemberField.value;
					if (obj is FunctionObject)
					{
						for (;;)
						{
							FunctionObject functionObject = (FunctionObject)obj;
							if (functionObject.implementedIface == null)
							{
								break;
							}
							this.CheckMethodDeclarationConsistency(functionObject);
							if (functionObject.implementedIfaceMethod == null)
							{
								functionObject.funcContext.HandleError(JSError.NoMethodInBaseToOverride);
							}
							if (jsmemberField.IsPublic || jsmemberField.IsFamily || jsmemberField.IsFamilyOrAssembly)
							{
								functionObject.CheckCLSCompliance(flag);
							}
							jsmemberField = jsmemberField.nextOverload;
							if (jsmemberField == null)
							{
								break;
							}
							obj = jsmemberField.value;
						}
					}
					else
					{
						JSProperty jsproperty = obj as JSProperty;
					}
				}
				i++;
			}
			int j = 0;
			int num2 = this.fields.Length;
			while (j < num2)
			{
				JSMemberField jsmemberField2 = this.fields[j];
				if (!jsmemberField2.IsLiteral)
				{
					goto IL_0209;
				}
				object obj2 = jsmemberField2.value;
				if (obj2 is FunctionObject)
				{
					for (;;)
					{
						FunctionObject functionObject2 = (FunctionObject)obj2;
						if (functionObject2.implementedIface != null)
						{
							break;
						}
						this.CheckMethodDeclarationConsistency(functionObject2);
						if (jsmemberField2.IsPublic || jsmemberField2.IsFamily || jsmemberField2.IsFamilyOrAssembly)
						{
							functionObject2.CheckCLSCompliance(flag);
						}
						jsmemberField2 = jsmemberField2.nextOverload;
						if (jsmemberField2 == null)
						{
							break;
						}
						obj2 = jsmemberField2.value;
					}
				}
				else if (!(obj2 is JSProperty))
				{
					goto IL_0209;
				}
				IL_0234:
				j++;
				continue;
				IL_0209:
				this.CheckFieldDeclarationConsistency(jsmemberField2);
				if (jsmemberField2.IsPublic || jsmemberField2.IsFamily || jsmemberField2.IsFamilyOrAssembly)
				{
					jsmemberField2.CheckCLSCompliance(flag);
					goto IL_0234;
				}
				goto IL_0234;
			}
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00011B70 File Offset: 0x00010B70
		private void CheckMethodDeclarationConsistency(FunctionObject func)
		{
			if (func.isStatic && !func.isExpandoMethod)
			{
				return;
			}
			if (func.isConstructor)
			{
				return;
			}
			object obj = this.firstIndex[func.name];
			if (obj == null)
			{
				this.CheckThatMethodIsNotMarkedWithOverrideOrHide(func);
				if ((func.attributes & MethodAttributes.Final) != MethodAttributes.PrivateScope)
				{
					func.attributes &= ~(MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask);
				}
				return;
			}
			MemberInfo memberInfo = null;
			int i = (int)obj;
			int num = this.superMembers.Length;
			while (i < num)
			{
				MemberInfo memberInfo2 = this.superMembers[i] as MemberInfo;
				if (memberInfo2 != null)
				{
					if (!memberInfo2.Name.Equals(func.name))
					{
						break;
					}
					if (this.CanSee(memberInfo2))
					{
						if (memberInfo2.MemberType != MemberTypes.Method)
						{
							memberInfo = memberInfo2;
						}
						else
						{
							if (func.isExpandoMethod)
							{
								memberInfo = memberInfo2;
								break;
							}
							MethodInfo methodInfo = (MethodInfo)memberInfo2;
							if (func.implementedIface != null)
							{
								if (methodInfo is JSFieldMethod)
								{
									if (((JSFieldMethod)methodInfo).EnclosingScope() != func.implementedIface)
									{
										goto IL_0158;
									}
								}
								else if (methodInfo.DeclaringType != func.implementedIface)
								{
									goto IL_0158;
								}
							}
							if (Class.ParametersMatch(methodInfo.GetParameters(), func.parameter_declarations))
							{
								if (methodInfo is JSWrappedMethod)
								{
									methodInfo = ((JSWrappedMethod)methodInfo).method;
								}
								if (func.noVersionSafeAttributeSpecified || (func.attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.VtableLayoutMask)
								{
									this.CheckMatchingMethodForConsistency(methodInfo, func, i, num);
								}
								return;
							}
						}
					}
				}
				IL_0158:
				i++;
			}
			if (memberInfo != null)
			{
				if (func.noVersionSafeAttributeSpecified || ((func.attributes & MethodAttributes.VtableLayoutMask) != MethodAttributes.VtableLayoutMask && !func.isExpandoMethod))
				{
					string fullNameFor = this.GetFullNameFor(memberInfo);
					func.funcContext.HandleError(JSError.HidesParentMember, fullNameFor, this.IsInTheSameCompilationUnit(memberInfo));
				}
				return;
			}
			this.CheckThatMethodIsNotMarkedWithOverrideOrHide(func);
			if ((func.attributes & MethodAttributes.Final) != MethodAttributes.PrivateScope)
			{
				func.attributes &= ~(MethodAttributes.Final | MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask);
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x00011D50 File Offset: 0x00010D50
		private void CheckMatchingMethodForConsistency(MethodInfo matchingMethod, FunctionObject func, int i, int n)
		{
			IReflect reflect = func.ReturnType(null);
			IReflect reflect2 = ((matchingMethod is JSFieldMethod) ? ((JSFieldMethod)matchingMethod).func.ReturnType(null) : matchingMethod.ReturnType);
			if (!reflect.Equals(reflect2))
			{
				func.funcContext.HandleError(JSError.DifferentReturnTypeFromBase, func.name, true);
				return;
			}
			if (func.implementedIface != null)
			{
				func.implementedIfaceMethod = matchingMethod;
				this.superMembers[i] = func.name;
				return;
			}
			MethodAttributes methodAttributes = func.attributes & MethodAttributes.MemberAccessMask;
			if ((matchingMethod.Attributes & MethodAttributes.MemberAccessMask) != methodAttributes && ((matchingMethod.Attributes & MethodAttributes.MemberAccessMask) != MethodAttributes.FamORAssem || methodAttributes != MethodAttributes.Family))
			{
				func.funcContext.HandleError(JSError.CannotChangeVisibility);
			}
			if (func.noVersionSafeAttributeSpecified)
			{
				if (base.Engine.versionSafe)
				{
					if ((matchingMethod.Attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
					{
						func.funcContext.HandleError(JSError.HidesAbstractInBase, this.name + "." + func.name);
						func.attributes &= ~MethodAttributes.VtableLayoutMask;
					}
					else
					{
						func.funcContext.HandleError(JSError.NewNotSpecifiedInMethodDeclaration, this.IsInTheSameCompilationUnit(matchingMethod));
						i = -1;
					}
				}
				else if ((matchingMethod.Attributes & MethodAttributes.Virtual) == MethodAttributes.PrivateScope || (matchingMethod.Attributes & MethodAttributes.Final) != MethodAttributes.PrivateScope)
				{
					i = -1;
				}
				else
				{
					func.attributes &= ~MethodAttributes.VtableLayoutMask;
					if ((matchingMethod.Attributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope)
					{
						i = -1;
					}
				}
			}
			else if ((func.attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope)
			{
				if ((matchingMethod.Attributes & MethodAttributes.Virtual) == MethodAttributes.PrivateScope || (matchingMethod.Attributes & MethodAttributes.Final) != MethodAttributes.PrivateScope)
				{
					func.funcContext.HandleError(JSError.MethodInBaseIsNotVirtual);
					i = -1;
				}
				else
				{
					func.attributes &= ~MethodAttributes.VtableLayoutMask;
					if ((matchingMethod.Attributes & MethodAttributes.Abstract) == MethodAttributes.PrivateScope)
					{
						i = -1;
					}
				}
			}
			else if ((matchingMethod.Attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
			{
				func.funcContext.HandleError(JSError.HidesAbstractInBase, this.name + "." + func.name);
				func.attributes &= ~MethodAttributes.VtableLayoutMask;
			}
			else
			{
				i = -1;
			}
			if (i >= 0)
			{
				this.superMembers[i] = func.name;
				for (int j = i + 1; j < n; j++)
				{
					MemberInfo memberInfo = this.superMembers[j] as MemberInfo;
					if (memberInfo != null)
					{
						if (memberInfo.Name != matchingMethod.Name)
						{
							return;
						}
						MethodInfo methodInfo = memberInfo as MethodInfo;
						if (methodInfo != null && methodInfo.IsAbstract && Class.ParametersMatch(methodInfo.GetParameters(), matchingMethod.GetParameters()))
						{
							IReflect reflect3 = ((matchingMethod is JSFieldMethod) ? ((JSFieldMethod)matchingMethod).ReturnIR() : matchingMethod.ReturnType);
							IReflect reflect4 = ((methodInfo is JSFieldMethod) ? ((JSFieldMethod)methodInfo).ReturnIR() : methodInfo.ReturnType);
							if (reflect3 == reflect4)
							{
								this.superMembers[j] = func.name;
							}
						}
					}
				}
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x00012040 File Offset: 0x00011040
		private void CheckThatAllAbstractSuperClassMethodsAreImplemented()
		{
			int i = 0;
			int num = this.superMembers.Length;
			while (i < num)
			{
				object obj = this.superMembers[i];
				MethodInfo methodInfo = obj as MethodInfo;
				if (methodInfo != null && methodInfo.IsAbstract)
				{
					for (int j = i - 1; j >= 0; j--)
					{
						object obj2 = this.superMembers[j];
						if (obj2 is MethodInfo)
						{
							MethodInfo methodInfo2 = (MethodInfo)obj2;
							if (methodInfo2.Name != methodInfo.Name)
							{
								break;
							}
							if (!methodInfo2.IsAbstract && Class.ParametersMatch(methodInfo2.GetParameters(), methodInfo.GetParameters()))
							{
								IReflect reflect = ((methodInfo is JSFieldMethod) ? ((JSFieldMethod)methodInfo).ReturnIR() : methodInfo.ReturnType);
								IReflect reflect2 = ((methodInfo2 is JSFieldMethod) ? ((JSFieldMethod)methodInfo2).ReturnIR() : methodInfo2.ReturnType);
								if (reflect == reflect2)
								{
									this.superMembers[i] = methodInfo.Name;
									goto IL_01F3;
								}
							}
						}
					}
					if (!this.isAbstract || (!this.isInterface && Class.DefinedOnInterface(methodInfo)))
					{
						StringBuilder stringBuilder = new StringBuilder(methodInfo.DeclaringType.ToString());
						stringBuilder.Append('.');
						stringBuilder.Append(methodInfo.Name);
						stringBuilder.Append('(');
						ParameterInfo[] parameters = methodInfo.GetParameters();
						int k = 0;
						int num2 = parameters.Length;
						while (k < num2)
						{
							stringBuilder.Append(parameters[k].ParameterType.FullName);
							if (k < num2 - 1)
							{
								stringBuilder.Append(", ");
							}
							k++;
						}
						stringBuilder.Append(")");
						if (methodInfo.ReturnType != Typeob.Void)
						{
							stringBuilder.Append(" : ");
							stringBuilder.Append(methodInfo.ReturnType.FullName);
						}
						this.context.HandleError(JSError.MustImplementMethod, stringBuilder.ToString());
						this.attributes |= TypeAttributes.Abstract;
					}
				}
				IL_01F3:
				i++;
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0001224B File Offset: 0x0001124B
		private void CheckThatMethodIsNotMarkedWithOverrideOrHide(FunctionObject func)
		{
			if (func.noVersionSafeAttributeSpecified)
			{
				return;
			}
			if ((func.attributes & MethodAttributes.VtableLayoutMask) == MethodAttributes.PrivateScope)
			{
				func.funcContext.HandleError(JSError.NoMethodInBaseToOverride);
				return;
			}
			func.funcContext.HandleError(JSError.NoMethodInBaseToNew);
		}

		// Token: 0x0600023D RID: 573 RVA: 0x00012288 File Offset: 0x00011288
		private static bool DefinedOnInterface(MethodInfo meth)
		{
			JSFieldMethod jsfieldMethod = meth as JSFieldMethod;
			if (jsfieldMethod != null)
			{
				return ((ClassScope)jsfieldMethod.func.enclosing_scope).owner.isInterface;
			}
			return meth.DeclaringType.IsInterface;
		}

		// Token: 0x0600023E RID: 574 RVA: 0x000122C8 File Offset: 0x000112C8
		private void EmitILForINeedEngineMethods()
		{
			if (!this.needsEngine)
			{
				return;
			}
			TypeBuilder typeBuilder = (TypeBuilder)this.classob.classwriter;
			FieldBuilder fieldBuilder = typeBuilder.DefineField("vsa Engine", Typeob.VsaEngine, FieldAttributes.Private | FieldAttributes.NotSerialized);
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("GetEngine", MethodAttributes.Private | MethodAttributes.Virtual, Typeob.VsaEngine, null);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldfld, fieldBuilder);
			ilgenerator.Emit(OpCodes.Ldnull);
			Label label = ilgenerator.DefineLabel();
			ilgenerator.Emit(OpCodes.Bne_Un_S, label);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			if (this.body.Engine.doCRS)
			{
				ilgenerator.Emit(OpCodes.Ldsfld, CompilerGlobals.contextEngineField);
			}
			else if (this.context.document.engine.PEFileKind == PEFileKinds.Dll)
			{
				ilgenerator.Emit(OpCodes.Ldtoken, typeBuilder);
				ilgenerator.Emit(OpCodes.Call, CompilerGlobals.createVsaEngineWithType);
			}
			else
			{
				ilgenerator.Emit(OpCodes.Call, CompilerGlobals.createVsaEngine);
			}
			ilgenerator.Emit(OpCodes.Stfld, fieldBuilder);
			ilgenerator.MarkLabel(label);
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldfld, fieldBuilder);
			ilgenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineMethodOverride(methodBuilder, CompilerGlobals.getEngineMethod);
			MethodBuilder methodBuilder2 = typeBuilder.DefineMethod("SetEngine", MethodAttributes.Private | MethodAttributes.Virtual, Typeob.Void, new Type[] { Typeob.VsaEngine });
			ilgenerator = methodBuilder2.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Stfld, fieldBuilder);
			ilgenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineMethodOverride(methodBuilder2, CompilerGlobals.setEngineMethod);
		}

		// Token: 0x0600023F RID: 575 RVA: 0x00012478 File Offset: 0x00011478
		internal void EmitInitialCalls(ILGenerator il, MethodBase supcons, ParameterInfo[] pars, ASTList argAST, int callerParameterCount)
		{
			bool flag = true;
			if (supcons != null)
			{
				il.Emit(OpCodes.Ldarg_0);
				int num = pars.Length;
				int num2 = ((argAST == null) ? 0 : argAST.count);
				object[] array = new object[num];
				for (int i = 0; i < num; i++)
				{
					AST ast = ((i < num2) ? argAST[i] : new ConstantWrapper(null, null));
					if (pars[i].ParameterType.IsByRef)
					{
						array[i] = ast.TranslateToILReference(il, pars[i].ParameterType.GetElementType());
					}
					else
					{
						ast.TranslateToIL(il, pars[i].ParameterType);
						array[i] = null;
					}
				}
				if (supcons is JSConstructor)
				{
					JSConstructor jsconstructor = (JSConstructor)supcons;
					flag = jsconstructor.GetClassScope() != this.classob;
					supcons = jsconstructor.GetConstructorInfo(base.compilerGlobals);
					if (jsconstructor.GetClassScope().outerClassField != null)
					{
						Convert.EmitLdarg(il, (short)callerParameterCount);
					}
				}
				il.Emit(OpCodes.Call, (ConstructorInfo)supcons);
				for (int j = 0; j < num2; j++)
				{
					AST ast2 = argAST[j];
					if (ast2 is AddressOf && array[j] != null)
					{
						Type type = Convert.ToType(ast2.InferType(null));
						ast2.TranslateToILPreSet(il);
						il.Emit(OpCodes.Ldloc, (LocalBuilder)array[j]);
						Convert.Emit(this, il, pars[j].ParameterType, type);
						ast2.TranslateToILSet(il);
					}
				}
			}
			if (this.classob.outerClassField != null)
			{
				il.Emit(OpCodes.Ldarg_0);
				Convert.EmitLdarg(il, (short)callerParameterCount);
				il.Emit(OpCodes.Stfld, this.classob.outerClassField);
			}
			if (flag)
			{
				il.Emit(OpCodes.Ldarg_0);
				il.Emit(OpCodes.Call, this.fieldInitializer);
				this.body.TranslateToILInitOnlyInitializers(il);
			}
		}

		// Token: 0x06000240 RID: 576 RVA: 0x00012648 File Offset: 0x00011648
		private void EnterNameIntoEnclosingScopeAndGetOwnField(AST id, bool isStatic)
		{
			if (((IActivationObject)this.enclosingScope).GetLocalField(this.name) != null)
			{
				id.context.HandleError(JSError.DuplicateName, true);
				this.name += " class";
			}
			FieldAttributes fieldAttributes = FieldAttributes.Literal;
			switch (this.attributes & TypeAttributes.VisibilityMask)
			{
			case TypeAttributes.NestedPrivate:
				fieldAttributes |= FieldAttributes.Private;
				break;
			case TypeAttributes.NestedFamily:
				fieldAttributes |= FieldAttributes.Family;
				break;
			case TypeAttributes.NestedAssembly:
				fieldAttributes |= FieldAttributes.Assembly;
				break;
			case TypeAttributes.NestedFamANDAssem:
				fieldAttributes |= FieldAttributes.FamANDAssem;
				break;
			case TypeAttributes.VisibilityMask:
				fieldAttributes |= FieldAttributes.FamORAssem;
				break;
			default:
				fieldAttributes |= FieldAttributes.Public;
				break;
			}
			ScriptObject parent = this.enclosingScope;
			while (parent is BlockScope)
			{
				parent = parent.GetParent();
			}
			if (!(parent is GlobalScope) && !(parent is PackageScope) && !(parent is ClassScope))
			{
				isStatic = false;
				if (this is EnumDeclaration)
				{
					this.context.HandleError(JSError.EnumNotAllowed);
				}
				else
				{
					this.context.HandleError(JSError.ClassNotAllowed);
				}
			}
			if (isStatic)
			{
				fieldAttributes |= FieldAttributes.Static;
			}
			if (this.enclosingScope is ActivationObject)
			{
				if (this.enclosingScope is ClassScope && this.name == ((ClassScope)this.enclosingScope).name)
				{
					this.context.HandleError(JSError.CannotUseNameOfClass);
					this.name += " nested class";
				}
				this.ownField = ((ActivationObject)this.enclosingScope).AddNewField(this.name, this.classob, fieldAttributes);
				if (this.ownField is JSLocalField)
				{
					((JSLocalField)this.ownField).isDefined = true;
				}
			}
			else
			{
				this.ownField = ((StackFrame)this.enclosingScope).AddNewField(this.name, this.classob, fieldAttributes);
			}
			this.ownField.originalContext = id.context;
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0001281C File Offset: 0x0001181C
		internal override object Evaluate()
		{
			base.Globals.ScopeStack.GuardedPush(this.classob);
			try
			{
				this.body.EvaluateStaticVariableInitializers();
			}
			finally
			{
				base.Globals.ScopeStack.Pop();
			}
			return new Completion();
		}

		// Token: 0x06000242 RID: 578 RVA: 0x00012874 File Offset: 0x00011874
		private void GenerateGetEnumerator()
		{
			TypeBuilder typeBuilder = this.classob.GetTypeBuilder();
			MethodBuilder methodBuilder = typeBuilder.DefineMethod("get enumerator", MethodAttributes.Private | MethodAttributes.Virtual, Typeob.IEnumerator, null);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, this.getHashTableMethod);
			ilgenerator.Emit(OpCodes.Call, CompilerGlobals.hashTableGetEnumerator);
			ilgenerator.Emit(OpCodes.Ret);
			typeBuilder.DefineMethodOverride(methodBuilder, CompilerGlobals.getEnumeratorMethod);
		}

		// Token: 0x06000243 RID: 579 RVA: 0x000128EC File Offset: 0x000118EC
		private void GetExpandoFieldGetter(TypeBuilder classwriter)
		{
			if (this.expandoItemProp == null)
			{
				this.expandoItemProp = classwriter.DefineProperty("Item", PropertyAttributes.None, Typeob.Object, new Type[] { Typeob.String });
				FieldInfo fieldInfo = classwriter.DefineField("expando table", Typeob.SimpleHashtable, FieldAttributes.Private);
				this.getHashTableMethod = classwriter.DefineMethod("get expando table", MethodAttributes.Private, Typeob.SimpleHashtable, null);
				ILGenerator ilgenerator = this.getHashTableMethod.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
				ilgenerator.Emit(OpCodes.Ldnull);
				Label label = ilgenerator.DefineLabel();
				ilgenerator.Emit(OpCodes.Bne_Un_S, label);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldc_I4_8);
				ilgenerator.Emit(OpCodes.Newobj, CompilerGlobals.hashtableCtor);
				ilgenerator.Emit(OpCodes.Stfld, fieldInfo);
				ilgenerator.MarkLabel(label);
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
				ilgenerator.Emit(OpCodes.Ret);
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x000129F4 File Offset: 0x000119F4
		internal MethodInfo GetExpandoIndexerGetter()
		{
			if (this.getItem == null)
			{
				TypeBuilder typeBuilder = this.classob.GetTypeBuilder();
				this.GetExpandoFieldGetter(typeBuilder);
				this.getItem = typeBuilder.DefineMethod("get_Item", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.SpecialName, Typeob.Object, new Type[] { Typeob.String });
				ILGenerator ilgenerator = this.getItem.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Call, this.getHashTableMethod);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Call, CompilerGlobals.hashtableGetItem);
				ilgenerator.Emit(OpCodes.Dup);
				Label label = ilgenerator.DefineLabel();
				ilgenerator.Emit(OpCodes.Brtrue_S, label);
				ilgenerator.Emit(OpCodes.Pop);
				ilgenerator.Emit(OpCodes.Ldsfld, CompilerGlobals.missingField);
				ilgenerator.MarkLabel(label);
				ilgenerator.Emit(OpCodes.Ret);
				this.expandoItemProp.SetGetMethod(this.getItem);
			}
			return this.getItem;
		}

		// Token: 0x06000245 RID: 581 RVA: 0x00012AF0 File Offset: 0x00011AF0
		internal MethodInfo GetExpandoIndexerSetter()
		{
			if (this.setItem == null)
			{
				TypeBuilder typeBuilder = this.classob.GetTypeBuilder();
				this.GetExpandoFieldGetter(typeBuilder);
				this.setItem = typeBuilder.DefineMethod("set_Item", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.SpecialName, Typeob.Void, new Type[]
				{
					Typeob.String,
					Typeob.Object
				});
				ILGenerator ilgenerator = this.setItem.GetILGenerator();
				ilgenerator.Emit(OpCodes.Ldarg_0);
				ilgenerator.Emit(OpCodes.Call, this.getHashTableMethod);
				ilgenerator.Emit(OpCodes.Ldarg_2);
				ilgenerator.Emit(OpCodes.Ldsfld, CompilerGlobals.missingField);
				Label label = ilgenerator.DefineLabel();
				ilgenerator.Emit(OpCodes.Beq_S, label);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Ldarg_2);
				ilgenerator.Emit(OpCodes.Call, CompilerGlobals.hashtableSetItem);
				ilgenerator.Emit(OpCodes.Ret);
				ilgenerator.MarkLabel(label);
				ilgenerator.Emit(OpCodes.Ldarg_1);
				ilgenerator.Emit(OpCodes.Call, CompilerGlobals.hashtableRemove);
				ilgenerator.Emit(OpCodes.Ret);
				this.expandoItemProp.SetSetMethod(this.setItem);
			}
			return this.setItem;
		}

		// Token: 0x06000246 RID: 582 RVA: 0x00012C1C File Offset: 0x00011C1C
		private void GetExpandoDeleteMethod()
		{
			TypeBuilder typeBuilder = this.classob.GetTypeBuilder();
			MethodBuilder methodBuilder = (this.deleteOpMethod = typeBuilder.DefineMethod("op_Delete", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static | MethodAttributes.SpecialName, Typeob.Boolean, new Type[]
			{
				typeBuilder,
				Typeob.ArrayOfObject
			}));
			ParameterBuilder parameterBuilder = methodBuilder.DefineParameter(2, ParameterAttributes.None, null);
			parameterBuilder.SetCustomAttribute(new CustomAttributeBuilder(Typeob.ParamArrayAttribute.GetConstructor(Type.EmptyTypes), new object[0]));
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldarg_0);
			ilgenerator.Emit(OpCodes.Call, this.getHashTableMethod);
			ilgenerator.Emit(OpCodes.Ldarg_1);
			ilgenerator.Emit(OpCodes.Dup);
			ilgenerator.Emit(OpCodes.Ldlen);
			ilgenerator.Emit(OpCodes.Ldc_I4_1);
			ilgenerator.Emit(OpCodes.Sub);
			ilgenerator.Emit(OpCodes.Ldelem_Ref);
			ilgenerator.Emit(OpCodes.Call, CompilerGlobals.hashtableRemove);
			ilgenerator.Emit(OpCodes.Ldc_I4_1);
			ilgenerator.Emit(OpCodes.Ret);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x00012D24 File Offset: 0x00011D24
		private string GetFullNameFor(MemberInfo supMem)
		{
			string text;
			if (supMem is JSField)
			{
				text = ((JSField)supMem).GetClassFullName();
			}
			else if (supMem is JSConstructor)
			{
				text = ((JSConstructor)supMem).GetClassFullName();
			}
			else if (supMem is JSMethod)
			{
				text = ((JSMethod)supMem).GetClassFullName();
			}
			else if (supMem is JSProperty)
			{
				text = ((JSProperty)supMem).GetClassFullName();
			}
			else if (supMem is JSWrappedProperty)
			{
				text = ((JSWrappedProperty)supMem).GetClassFullName();
			}
			else
			{
				text = supMem.DeclaringType.FullName;
			}
			return text + "." + supMem.Name;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x00012DBC File Offset: 0x00011DBC
		internal MemberInfo[] GetInterfaceMember(string name)
		{
			this.PartiallyEvaluate();
			if (this.isInterface)
			{
				MemberInfo[] array = this.classob.GetMember(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				if (array != null && array.Length > 0)
				{
					return array;
				}
			}
			foreach (TypeExpression typeExpression in this.interfaces)
			{
				IReflect reflect = typeExpression.ToIReflect();
				MemberInfo[] array = reflect.GetMember(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
				if (array != null && array.Length > 0)
				{
					return array;
				}
			}
			return new MemberInfo[0];
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00012E3C File Offset: 0x00011E3C
		private void GetIRForSuperType()
		{
			IReflect reflect = (this.superIR = Typeob.Object);
			if (this.superTypeExpression != null)
			{
				this.superTypeExpression.PartiallyEvaluate();
				reflect = (this.superIR = this.superTypeExpression.ToIReflect());
			}
			Type type = reflect as Type;
			if (type != null)
			{
				if (type.IsSealed || type.IsInterface || type == Typeob.ValueType || type == Typeob.ArrayObject)
				{
					if (this.superTypeExpression.Evaluate() is Namespace)
					{
						this.superTypeExpression.context.HandleError(JSError.NeedType);
					}
					else
					{
						this.superTypeExpression.context.HandleError(JSError.TypeCannotBeExtended, type.FullName);
					}
					this.superTypeExpression = null;
					this.superIR = Typeob.Object;
					return;
				}
				if (Typeob.INeedEngine.IsAssignableFrom(type))
				{
					this.needsEngine = false;
					return;
				}
			}
			else if (reflect is ClassScope)
			{
				if (((ClassScope)reflect).owner.IsASubClassOf(this))
				{
					this.superTypeExpression.context.HandleError(JSError.CircularDefinition);
					this.superTypeExpression = null;
					this.superIR = Typeob.Object;
					return;
				}
				this.needsEngine = false;
				this.superClass = ((ClassScope)reflect).owner;
				if ((this.superClass.attributes & TypeAttributes.Sealed) != TypeAttributes.NotPublic)
				{
					this.superTypeExpression.context.HandleError(JSError.TypeCannotBeExtended, this.superClass.name);
					this.superClass.attributes &= ~TypeAttributes.Sealed;
					this.superTypeExpression = null;
					return;
				}
				if (this.superClass.isInterface)
				{
					this.superTypeExpression.context.HandleError(JSError.TypeCannotBeExtended, this.superClass.name);
					this.superIR = Typeob.Object;
					this.superTypeExpression = null;
					return;
				}
			}
			else
			{
				this.superTypeExpression.context.HandleError(JSError.TypeCannotBeExtended);
				this.superIR = Typeob.Object;
				this.superTypeExpression = null;
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x00013034 File Offset: 0x00012034
		private void GetStartIndexForEachName()
		{
			SimpleHashtable simpleHashtable = new SimpleHashtable(32U);
			string text = null;
			int i = 0;
			int num = this.superMembers.Length;
			while (i < num)
			{
				string text2 = ((MemberInfo)this.superMembers[i]).Name;
				if (text2 != text)
				{
					simpleHashtable[text = text2] = i;
				}
				i++;
			}
			this.firstIndex = simpleHashtable;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00013098 File Offset: 0x00012098
		internal ConstructorInfo GetSuperConstructor(IReflect[] argIRs)
		{
			object obj;
			if (this.superTypeExpression != null)
			{
				obj = this.superTypeExpression.Evaluate();
			}
			else
			{
				obj = Typeob.Object;
			}
			if (obj is ClassScope)
			{
				return JSBinder.SelectConstructor(((ClassScope)obj).constructors, argIRs);
			}
			return JSBinder.SelectConstructor(((Type)obj).GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic), argIRs);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x000130F0 File Offset: 0x000120F0
		private void GetSuperTypeMembers()
		{
			SuperTypeMembersSorter superTypeMembersSorter = new SuperTypeMembersSorter();
			IReflect reflect = this.superIR;
			while (reflect != null)
			{
				superTypeMembersSorter.Add(reflect.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
				if (reflect is Type)
				{
					reflect = ((Type)reflect).BaseType;
				}
				else
				{
					reflect = ((ClassScope)reflect).GetSuperType();
				}
			}
			ArrayList arrayList = new ArrayList();
			int num = this.interfaces.Length;
			IReflect[] array = new IReflect[num];
			for (int i = 0; i < num; i++)
			{
				IReflect reflect2 = (array[i] = this.interfaces[i].ToIReflect());
				Type type = reflect2 as Type;
				bool flag;
				if (type != null)
				{
					flag = type.IsInterface;
				}
				else
				{
					ClassScope classScope = (ClassScope)reflect2;
					flag = classScope.owner.isInterface;
				}
				if (!flag)
				{
					this.interfaces[i].context.HandleError(JSError.NeedInterface);
				}
			}
			foreach (IReflect reflect3 in array)
			{
				this.AddImplicitInterfaces(reflect3, array, arrayList);
			}
			for (int k = 0; k < arrayList.Count; k++)
			{
				IReflect reflect4 = (IReflect)arrayList[k];
				this.AddImplicitInterfaces(reflect4, array, arrayList);
			}
			int count = arrayList.Count;
			if (count > 0)
			{
				TypeExpression[] array3 = new TypeExpression[num + count];
				for (int l = 0; l < num; l++)
				{
					array3[l] = this.interfaces[l];
				}
				for (int m = 0; m < count; m++)
				{
					array3[m + num] = new TypeExpression(new ConstantWrapper(arrayList[m], null));
				}
				this.interfaces = array3;
			}
			foreach (TypeExpression typeExpression in this.interfaces)
			{
				ClassScope classScope2 = typeExpression.ToIReflect() as ClassScope;
				if (classScope2 != null && classScope2.owner.ImplementsInterface(this.classob))
				{
					this.context.HandleError(JSError.CircularDefinition);
					this.interfaces = new TypeExpression[0];
					break;
				}
				superTypeMembersSorter.Add(typeExpression.ToIReflect().GetMembers(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
			}
			reflect = this.superIR;
			while (reflect != null)
			{
				Type type2 = reflect as Type;
				if (type2 != null)
				{
					if (!type2.IsAbstract)
					{
						break;
					}
					Class.GetUnimplementedInferfaceMembersFor(type2, superTypeMembersSorter);
					reflect = type2.BaseType;
				}
				else
				{
					ClassScope classScope3 = (ClassScope)reflect;
					if (!classScope3.owner.isAbstract)
					{
						break;
					}
					classScope3.owner.GetUnimplementedInferfaceMembers(superTypeMembersSorter);
					reflect = null;
				}
			}
			this.superMembers = superTypeMembersSorter.GetMembers();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0001336A File Offset: 0x0001236A
		internal TypeBuilder GetTypeBuilder()
		{
			return (TypeBuilder)this.GetTypeBuilderOrEnumBuilder();
		}

		// Token: 0x0600024E RID: 590 RVA: 0x00013378 File Offset: 0x00012378
		internal virtual Type GetTypeBuilderOrEnumBuilder()
		{
			if (this.classob.classwriter != null)
			{
				return this.classob.classwriter;
			}
			if (!this.isAlreadyPartiallyEvaluated)
			{
				this.PartiallyEvaluate();
			}
			Type type;
			if (this.superTypeExpression != null)
			{
				type = this.superTypeExpression.ToType();
			}
			else
			{
				type = (this.isInterface ? null : Typeob.Object);
			}
			int num = (this.needsEngine ? 1 : 0) + (this.generateCodeForExpando ? 1 : 0);
			int num2 = this.interfaces.Length + num;
			Type[] array = new Type[num2];
			for (int i = num; i < num2; i++)
			{
				array[i] = this.interfaces[i - num].ToType();
			}
			if (this.needsEngine)
			{
				array[--num] = Typeob.INeedEngine;
			}
			if (this.generateCodeForExpando)
			{
				array[num - 1] = Typeob.IEnumerable;
			}
			TypeBuilder typeBuilder;
			if (this.enclosingScope is ClassScope)
			{
				if ((typeBuilder = (TypeBuilder)this.classob.classwriter) == null)
				{
					TypeBuilder typeBuilder2 = ((ClassScope)this.enclosingScope).owner.GetTypeBuilder();
					if (this.classob.classwriter != null)
					{
						return this.classob.classwriter;
					}
					typeBuilder = typeBuilder2.DefineNestedType(this.name, this.attributes, type, array);
					this.classob.classwriter = typeBuilder;
					if (!this.isStatic && !this.isInterface)
					{
						this.classob.outerClassField = typeBuilder.DefineField("outer class instance", typeBuilder2, FieldAttributes.Private);
					}
				}
			}
			else
			{
				string rootNamespace = ((ActivationObject)this.enclosingScope).GetName();
				if (rootNamespace == null)
				{
					VsaEngine engine = this.context.document.engine;
					if (engine != null && engine.genStartupClass)
					{
						rootNamespace = engine.RootNamespace;
					}
				}
				if ((typeBuilder = (TypeBuilder)this.classob.classwriter) == null)
				{
					string text = this.name;
					if (rootNamespace != null)
					{
						text = rootNamespace + "." + text;
					}
					if (text.Length >= 1024)
					{
						this.context.HandleError(JSError.TypeNameTooLong, text);
						text = "bad type name " + Class.badTypeNameCount.ToString(CultureInfo.InvariantCulture);
						Class.badTypeNameCount++;
					}
					typeBuilder = base.compilerGlobals.module.DefineType(text, this.attributes, type, array);
					this.classob.classwriter = typeBuilder;
				}
			}
			if (this.customAttributes != null)
			{
				CustomAttributeBuilder[] customAttributeBuilders = this.customAttributes.GetCustomAttributeBuilders(false);
				for (int j = 0; j < customAttributeBuilders.Length; j++)
				{
					typeBuilder.SetCustomAttribute(customAttributeBuilders[j]);
				}
			}
			if (this.clsCompliance == CLSComplianceSpec.CLSCompliant)
			{
				typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[] { true }));
			}
			else if (this.clsCompliance == CLSComplianceSpec.NonCLSCompliant)
			{
				typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.clsCompliantAttributeCtor, new object[] { false }));
			}
			if (this.generateCodeForExpando)
			{
				typeBuilder.SetCustomAttribute(new CustomAttributeBuilder(CompilerGlobals.defaultMemberAttributeCtor, new object[] { "Item" }));
			}
			int k = 0;
			int num3 = this.fields.Length;
			while (k < num3)
			{
				JSMemberField jsmemberField = this.fields[k];
				if (jsmemberField.IsLiteral)
				{
					object value = jsmemberField.value;
					if (value is JSProperty)
					{
						JSProperty jsproperty = (JSProperty)value;
						ParameterInfo[] indexParameters = jsproperty.GetIndexParameters();
						int num4 = indexParameters.Length;
						Type[] array2 = new Type[num4];
						for (int l = 0; l < num4; l++)
						{
							array2[l] = indexParameters[l].ParameterType;
						}
						PropertyBuilder propertyBuilder = (jsproperty.metaData = typeBuilder.DefineProperty(jsmemberField.Name, jsproperty.Attributes, jsproperty.PropertyType, array2));
						if (jsproperty.getter != null)
						{
							CustomAttributeList customAttributeList = ((JSFieldMethod)jsproperty.getter).func.customAttributes;
							if (customAttributeList != null)
							{
								CustomAttributeBuilder[] customAttributeBuilders2 = customAttributeList.GetCustomAttributeBuilders(true);
								foreach (CustomAttributeBuilder customAttributeBuilder in customAttributeBuilders2)
								{
									propertyBuilder.SetCustomAttribute(customAttributeBuilder);
								}
							}
							propertyBuilder.SetGetMethod((MethodBuilder)jsproperty.getter.GetMethodInfo(base.compilerGlobals));
						}
						if (jsproperty.setter != null)
						{
							CustomAttributeList customAttributeList2 = ((JSFieldMethod)jsproperty.setter).func.customAttributes;
							if (customAttributeList2 != null)
							{
								CustomAttributeBuilder[] customAttributeBuilders3 = customAttributeList2.GetCustomAttributeBuilders(true);
								foreach (CustomAttributeBuilder customAttributeBuilder2 in customAttributeBuilders3)
								{
									propertyBuilder.SetCustomAttribute(customAttributeBuilder2);
								}
							}
							propertyBuilder.SetSetMethod((MethodBuilder)jsproperty.setter.GetMethodInfo(base.compilerGlobals));
						}
					}
					else if (value is ClassScope)
					{
						((ClassScope)value).GetTypeBuilderOrEnumBuilder();
					}
					else if (Convert.GetTypeCode(value) != TypeCode.Object)
					{
						FieldBuilder fieldBuilder = typeBuilder.DefineField(jsmemberField.Name, jsmemberField.FieldType, jsmemberField.Attributes);
						fieldBuilder.SetConstant(jsmemberField.value);
						jsmemberField.metaData = fieldBuilder;
						jsmemberField.WriteCustomAttribute(base.Engine.doCRS);
					}
					else if (value is FunctionObject)
					{
						FunctionObject functionObject = (FunctionObject)value;
						if (functionObject.isExpandoMethod)
						{
							jsmemberField.metaData = typeBuilder.DefineField(jsmemberField.Name, Typeob.ScriptFunction, jsmemberField.Attributes & ~(FieldAttributes.Static | FieldAttributes.Literal));
							functionObject.isStatic = false;
						}
						if (this.isInterface)
						{
							for (;;)
							{
								functionObject.GetMethodInfo(base.compilerGlobals);
								jsmemberField = jsmemberField.nextOverload;
								if (jsmemberField == null)
								{
									break;
								}
								functionObject = (FunctionObject)jsmemberField.value;
							}
						}
					}
				}
				else
				{
					jsmemberField.metaData = typeBuilder.DefineField(jsmemberField.Name, jsmemberField.FieldType, jsmemberField.Attributes);
					jsmemberField.WriteCustomAttribute(base.Engine.doCRS);
				}
				k++;
			}
			return typeBuilder;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00013970 File Offset: 0x00012970
		private void GetUnimplementedInferfaceMembers(SuperTypeMembersSorter sorter)
		{
			int i = 0;
			int num = this.superMembers.Length;
			while (i < num)
			{
				MethodInfo methodInfo = this.superMembers[i] as MethodInfo;
				if (methodInfo != null && methodInfo.DeclaringType.IsInterface)
				{
					sorter.Add(methodInfo);
				}
				i++;
			}
		}

		// Token: 0x06000250 RID: 592 RVA: 0x000139B8 File Offset: 0x000129B8
		private static void GetUnimplementedInferfaceMembersFor(Type type, SuperTypeMembersSorter sorter)
		{
			foreach (Type type2 in type.GetInterfaces())
			{
				InterfaceMapping interfaceMap = type.GetInterfaceMap(type2);
				MethodInfo[] interfaceMethods = interfaceMap.InterfaceMethods;
				MethodInfo[] targetMethods = interfaceMap.TargetMethods;
				int j = 0;
				int num = interfaceMethods.Length;
				while (j < num)
				{
					if (targetMethods[j] == null || targetMethods[j].IsAbstract)
					{
						sorter.Add(interfaceMethods[j]);
					}
					j++;
				}
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00013A30 File Offset: 0x00012A30
		internal bool ImplementsInterface(IReflect iface)
		{
			TypeExpression[] array = this.interfaces;
			int i = 0;
			while (i < array.Length)
			{
				TypeExpression typeExpression = array[i];
				IReflect reflect = typeExpression.ToIReflect();
				bool flag;
				if (reflect == iface)
				{
					flag = true;
				}
				else if (reflect is ClassScope && ((ClassScope)reflect).ImplementsInterface(iface))
				{
					flag = true;
				}
				else
				{
					if (!(reflect is Type) || !(iface is Type) || !((Type)iface).IsAssignableFrom((Type)reflect))
					{
						i++;
						continue;
					}
					flag = true;
				}
				return flag;
			}
			return false;
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00013AB0 File Offset: 0x00012AB0
		private bool IsASubClassOf(Class cl)
		{
			if (this.superTypeExpression != null)
			{
				this.superTypeExpression.PartiallyEvaluate();
				IReflect reflect = this.superTypeExpression.ToIReflect();
				if (reflect is ClassScope)
				{
					Class owner = ((ClassScope)reflect).owner;
					return owner == cl || owner.IsASubClassOf(cl);
				}
			}
			return false;
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00013B00 File Offset: 0x00012B00
		internal bool IsCustomAttribute()
		{
			this.GetIRForSuperType();
			if (this.superIR != Typeob.Attribute)
			{
				return false;
			}
			if (this.customAttributes == null)
			{
				return false;
			}
			this.customAttributes.PartiallyEvaluate();
			return this.validOn != (AttributeTargets)0;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00013B38 File Offset: 0x00012B38
		internal bool IsExpando()
		{
			if (this.hasAlreadyBeenAskedAboutExpando)
			{
				return this.isExpando;
			}
			if (this.customAttributes != null)
			{
				this.customAttributes.PartiallyEvaluate();
				if (this.customAttributes.GetAttribute(Typeob.Expando) != null)
				{
					this.generateCodeForExpando = (this.isExpando = true);
				}
			}
			bool flag = false;
			this.GetIRForSuperType();
			ClassScope classScope = this.superIR as ClassScope;
			if (classScope != null)
			{
				classScope.owner.PartiallyEvaluate();
				if (classScope.owner.IsExpando())
				{
					flag = (this.isExpando = true);
				}
			}
			else if (CustomAttribute.IsDefined((Type)this.superIR, typeof(Expando), true))
			{
				flag = (this.isExpando = true);
			}
			this.hasAlreadyBeenAskedAboutExpando = true;
			if (this.generateCodeForExpando)
			{
				this.CheckIfOKToGenerateCodeForExpando(flag);
			}
			if (this.isExpando)
			{
				this.classob.noExpando = false;
				return true;
			}
			return false;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00013C18 File Offset: 0x00012C18
		private bool IsInTheSameCompilationUnit(MemberInfo member)
		{
			return member is JSField || member is JSMethod;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00013C30 File Offset: 0x00012C30
		private bool IsInTheSamePackage(MemberInfo member)
		{
			if (member is JSMethod || member is JSField)
			{
				PackageScope packageScope;
				if (member is JSMethod)
				{
					packageScope = ((JSMethod)member).GetPackage();
				}
				else if (member is JSConstructor)
				{
					packageScope = ((JSConstructor)member).GetPackage();
				}
				else
				{
					packageScope = ((JSField)member).GetPackage();
				}
				PackageScope package = this.classob.GetPackage();
				return package == packageScope;
			}
			return false;
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000257 RID: 599 RVA: 0x00013C99 File Offset: 0x00012C99
		internal bool IsStatic
		{
			get
			{
				return this.isStatic || !(this.enclosingScope is ClassScope);
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00013CB8 File Offset: 0x00012CB8
		protected bool NeedsToBeCheckedForCLSCompliance()
		{
			bool flag = false;
			this.clsCompliance = CLSComplianceSpec.NotAttributed;
			if (this.customAttributes != null)
			{
				CustomAttribute attribute = this.customAttributes.GetAttribute(Typeob.CLSCompliantAttribute);
				if (attribute != null)
				{
					this.clsCompliance = attribute.GetCLSComplianceValue();
					flag = this.clsCompliance == CLSComplianceSpec.CLSCompliant;
					this.customAttributes.Remove(attribute);
				}
			}
			if (this.clsCompliance == CLSComplianceSpec.CLSCompliant && !base.Engine.isCLSCompliant)
			{
				this.context.HandleError(JSError.TypeAssemblyCLSCompliantMismatch);
			}
			if (this.clsCompliance == CLSComplianceSpec.NotAttributed && (this.attributes & TypeAttributes.Public) != TypeAttributes.NotPublic)
			{
				flag = base.Engine.isCLSCompliant;
			}
			return flag;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x00013D54 File Offset: 0x00012D54
		internal static bool ParametersMatch(ParameterInfo[] suppars, ParameterInfo[] pars)
		{
			if (suppars.Length != pars.Length)
			{
				return false;
			}
			int i = 0;
			int num = pars.Length;
			while (i < num)
			{
				IReflect reflect = ((suppars[i] is ParameterDeclaration) ? ((ParameterDeclaration)suppars[i]).ParameterIReflect : suppars[i].ParameterType);
				IReflect reflect2 = ((pars[i] is ParameterDeclaration) ? ((ParameterDeclaration)pars[i]).ParameterIReflect : pars[i].ParameterType);
				if (!reflect2.Equals(reflect))
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x00013DCC File Offset: 0x00012DCC
		internal override AST PartiallyEvaluate()
		{
			if (this.isAlreadyPartiallyEvaluated)
			{
				return this;
			}
			this.isAlreadyPartiallyEvaluated = true;
			this.IsExpando();
			this.classob.SetParent(new WithObject(this.enclosingScope, this.superIR, true));
			base.Globals.ScopeStack.Push(this.classob);
			try
			{
				this.body.PartiallyEvaluate();
				if (this.implicitDefaultConstructor != null)
				{
					this.implicitDefaultConstructor.PartiallyEvaluate();
				}
			}
			finally
			{
				base.Globals.ScopeStack.Pop();
			}
			foreach (JSMemberField jsmemberField in this.fields)
			{
				jsmemberField.CheckOverloadsForDuplicates();
			}
			this.CheckIfValidExtensionOfSuperType();
			this.CheckThatAllAbstractSuperClassMethodsAreImplemented();
			return this;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x00013E94 File Offset: 0x00012E94
		private void SetAccessibility(FieldAttributes attributes)
		{
			FieldAttributes fieldAttributes = attributes & FieldAttributes.FieldAccessMask;
			if (!(this.enclosingScope is ClassScope))
			{
				if (fieldAttributes == FieldAttributes.Public || fieldAttributes == FieldAttributes.PrivateScope)
				{
					this.attributes |= TypeAttributes.Public;
				}
				return;
			}
			if (fieldAttributes == FieldAttributes.Public)
			{
				this.attributes |= TypeAttributes.NestedPublic;
				return;
			}
			if (fieldAttributes == FieldAttributes.Family)
			{
				this.attributes |= TypeAttributes.NestedFamily;
				return;
			}
			if (fieldAttributes == FieldAttributes.Assembly)
			{
				this.attributes |= TypeAttributes.NestedAssembly;
				return;
			}
			if (fieldAttributes == FieldAttributes.Private)
			{
				this.attributes |= TypeAttributes.NestedPrivate;
				return;
			}
			if (fieldAttributes == FieldAttributes.FamORAssem)
			{
				this.attributes |= TypeAttributes.VisibilityMask;
				return;
			}
			this.attributes |= TypeAttributes.NestedPublic;
		}

		// Token: 0x0600025C RID: 604 RVA: 0x00013F38 File Offset: 0x00012F38
		private void SetupConstructors()
		{
			MemberInfo[] member = this.classob.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (member == null)
			{
				this.AllocateImplicitDefaultConstructor();
				this.classob.AddNewField(this.name, this.implicitDefaultConstructor, FieldAttributes.Literal);
				this.classob.constructors = new ConstructorInfo[]
				{
					new JSConstructor(this.implicitDefaultConstructor)
				};
				return;
			}
			MemberInfo memberInfo = null;
			foreach (MemberInfo memberInfo2 in member)
			{
				if (memberInfo2 is JSFieldMethod)
				{
					FunctionObject func = ((JSFieldMethod)memberInfo2).func;
					if (memberInfo == null)
					{
						memberInfo = memberInfo2;
					}
					if (func.return_type_expr != null)
					{
						func.return_type_expr.context.HandleError(JSError.ConstructorMayNotHaveReturnType);
					}
					if ((func.attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope || (func.attributes & MethodAttributes.Static) != MethodAttributes.PrivateScope)
					{
						func.isStatic = false;
						JSVariableField jsvariableField = (JSVariableField)((JSFieldMethod)memberInfo2).field;
						jsvariableField.attributeFlags &= ~FieldAttributes.Static;
						jsvariableField.originalContext.HandleError(JSError.NotValidForConstructor);
					}
					func.return_type_expr = new TypeExpression(new ConstantWrapper(Typeob.Void, this.context));
					func.own_scope.AddReturnValueField();
				}
			}
			if (memberInfo != null)
			{
				this.classob.constructors = ((JSMemberField)((JSFieldMethod)memberInfo).field).GetAsConstructors(this.classob);
				return;
			}
			this.AllocateImplicitDefaultConstructor();
			this.classob.constructors = new ConstructorInfo[]
			{
				new JSConstructor(this.implicitDefaultConstructor)
			};
		}

		// Token: 0x0600025D RID: 605 RVA: 0x000140CC File Offset: 0x000130CC
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
			this.GetTypeBuilderOrEnumBuilder();
			this.TranslateToCOMPlusClass();
			object metaData = this.ownField.GetMetaData();
			if (metaData != null)
			{
				il.Emit(OpCodes.Ldtoken, this.classob.classwriter);
				il.Emit(OpCodes.Call, CompilerGlobals.getTypeFromHandleMethod);
				if (metaData is LocalBuilder)
				{
					il.Emit(OpCodes.Stloc, (LocalBuilder)metaData);
					return;
				}
				il.Emit(OpCodes.Stsfld, (FieldInfo)metaData);
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x00014146 File Offset: 0x00013146
		internal override void TranslateToILInitializer(ILGenerator il)
		{
		}

		// Token: 0x0600025F RID: 607 RVA: 0x00014148 File Offset: 0x00013148
		private void EmitUsingNamespaces(ILGenerator il)
		{
			if (this.body.Engine.GenerateDebugInfo)
			{
				for (ScriptObject parent = this.enclosingScope; parent != null; parent = parent.GetParent())
				{
					if (parent is PackageScope)
					{
						il.UsingNamespace(((PackageScope)parent).name);
					}
					else if (parent is WrappedNamespace && !((WrappedNamespace)parent).name.Equals(""))
					{
						il.UsingNamespace(((WrappedNamespace)parent).name);
					}
				}
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x000141C8 File Offset: 0x000131C8
		private void TranslateToCOMPlusClass()
		{
			if (this.isCooked)
			{
				return;
			}
			this.isCooked = true;
			if (this is EnumDeclaration)
			{
				if (!(this.enclosingScope is ClassScope))
				{
					this.TranslateToCreateTypeCall();
				}
				return;
			}
			if (this.superClass != null)
			{
				this.superClass.TranslateToCOMPlusClass();
			}
			int i = 0;
			int num = this.interfaces.Length;
			while (i < num)
			{
				IReflect reflect = this.interfaces[i].ToIReflect();
				if (reflect is ClassScope)
				{
					((ClassScope)reflect).owner.TranslateToCOMPlusClass();
				}
				i++;
			}
			base.Globals.ScopeStack.Push(this.classob);
			TypeBuilder classwriter = base.compilerGlobals.classwriter;
			base.compilerGlobals.classwriter = (TypeBuilder)this.classob.classwriter;
			if (!this.isInterface)
			{
				ConstructorBuilder constructorBuilder = base.compilerGlobals.classwriter.DefineTypeInitializer();
				ILGenerator ilgenerator = constructorBuilder.GetILGenerator();
				LocalBuilder localBuilder = null;
				if (this.classob.staticInitializerUsesEval)
				{
					localBuilder = ilgenerator.DeclareLocal(Typeob.VsaEngine);
					ilgenerator.Emit(OpCodes.Ldtoken, this.classob.GetTypeBuilder());
					ConstantWrapper.TranslateToILInt(ilgenerator, 0);
					ilgenerator.Emit(OpCodes.Newarr, Typeob.JSLocalField);
					if (base.Engine.PEFileKind == PEFileKinds.Dll)
					{
						ilgenerator.Emit(OpCodes.Ldtoken, this.classob.GetTypeBuilder());
						ilgenerator.Emit(OpCodes.Call, CompilerGlobals.createVsaEngineWithType);
					}
					else
					{
						ilgenerator.Emit(OpCodes.Call, CompilerGlobals.createVsaEngine);
					}
					ilgenerator.Emit(OpCodes.Dup);
					ilgenerator.Emit(OpCodes.Stloc, localBuilder);
					ilgenerator.Emit(OpCodes.Call, CompilerGlobals.pushStackFrameForStaticMethod);
					ilgenerator.BeginExceptionBlock();
				}
				this.body.TranslateToILStaticInitializers(ilgenerator);
				if (this.classob.staticInitializerUsesEval)
				{
					ilgenerator.BeginFinallyBlock();
					ilgenerator.Emit(OpCodes.Ldloc, localBuilder);
					ilgenerator.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
					ilgenerator.Emit(OpCodes.Pop);
					ilgenerator.EndExceptionBlock();
				}
				ilgenerator.Emit(OpCodes.Ret);
				this.EmitUsingNamespaces(ilgenerator);
				MethodBuilder methodBuilder = base.compilerGlobals.classwriter.DefineMethod(".init", MethodAttributes.Private, Typeob.Void, new Type[0]);
				this.fieldInitializer = methodBuilder;
				ilgenerator = methodBuilder.GetILGenerator();
				if (this.classob.instanceInitializerUsesEval)
				{
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ConstantWrapper.TranslateToILInt(ilgenerator, 0);
					ilgenerator.Emit(OpCodes.Newarr, Typeob.JSLocalField);
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ilgenerator.Emit(OpCodes.Callvirt, CompilerGlobals.getEngineMethod);
					ilgenerator.Emit(OpCodes.Call, CompilerGlobals.pushStackFrameForMethod);
					ilgenerator.BeginExceptionBlock();
				}
				this.body.TranslateToILInstanceInitializers(ilgenerator);
				if (this.classob.instanceInitializerUsesEval)
				{
					ilgenerator.BeginFinallyBlock();
					ilgenerator.Emit(OpCodes.Ldarg_0);
					ilgenerator.Emit(OpCodes.Callvirt, CompilerGlobals.getEngineMethod);
					ilgenerator.Emit(OpCodes.Call, CompilerGlobals.popScriptObjectMethod);
					ilgenerator.Emit(OpCodes.Pop);
					ilgenerator.EndExceptionBlock();
				}
				ilgenerator.Emit(OpCodes.Ret);
				this.EmitUsingNamespaces(ilgenerator);
				if (this.implicitDefaultConstructor != null)
				{
					this.implicitDefaultConstructor.TranslateToIL(base.compilerGlobals);
				}
				if (this.generateCodeForExpando)
				{
					this.GetExpandoIndexerGetter();
					this.GetExpandoIndexerSetter();
					this.GetExpandoDeleteMethod();
					this.GenerateGetEnumerator();
				}
				this.EmitILForINeedEngineMethods();
			}
			if (!(this.enclosingScope is ClassScope))
			{
				this.TranslateToCreateTypeCall();
			}
			base.compilerGlobals.classwriter = classwriter;
			base.Globals.ScopeStack.Pop();
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0001456C File Offset: 0x0001356C
		private void TranslateToCreateTypeCall()
		{
			if (this.cookedType != null)
			{
				return;
			}
			if (!(this is EnumDeclaration))
			{
				if (this.superClass != null)
				{
					this.superClass.TranslateToCreateTypeCall();
				}
				AppDomain domain = Thread.GetDomain();
				ResolveEventHandler resolveEventHandler = new ResolveEventHandler(this.ResolveEnum);
				domain.TypeResolve += resolveEventHandler;
				this.cookedType = ((TypeBuilder)this.classob.classwriter).CreateType();
				domain.TypeResolve -= resolveEventHandler;
				foreach (JSMemberField jsmemberField in this.fields)
				{
					ClassScope classScope = jsmemberField.value as ClassScope;
					if (classScope != null)
					{
						classScope.owner.TranslateToCreateTypeCall();
					}
				}
				return;
			}
			EnumBuilder enumBuilder = this.classob.classwriter as EnumBuilder;
			if (enumBuilder != null)
			{
				this.cookedType = enumBuilder.CreateType();
				return;
			}
			this.cookedType = ((TypeBuilder)this.classob.classwriter).CreateType();
		}

		// Token: 0x06000262 RID: 610 RVA: 0x00014654 File Offset: 0x00013654
		private Assembly ResolveEnum(object sender, ResolveEventArgs args)
		{
			FieldInfo field = this.classob.GetField(args.Name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
			if (field != null && field.IsLiteral)
			{
				ClassScope classScope = TypeReferences.GetConstantValue(field) as ClassScope;
				if (classScope != null)
				{
					classScope.owner.TranslateToCreateTypeCall();
				}
			}
			return base.compilerGlobals.assemblyBuilder;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x000146A5 File Offset: 0x000136A5
		internal override Context GetFirstExecutableContext()
		{
			return null;
		}

		// Token: 0x0400014F RID: 335
		internal string name;

		// Token: 0x04000150 RID: 336
		private TypeExpression superTypeExpression;

		// Token: 0x04000151 RID: 337
		private TypeExpression[] interfaces;

		// Token: 0x04000152 RID: 338
		internal Block body;

		// Token: 0x04000153 RID: 339
		internal ScriptObject enclosingScope;

		// Token: 0x04000154 RID: 340
		internal TypeAttributes attributes;

		// Token: 0x04000155 RID: 341
		private bool hasAlreadyBeenAskedAboutExpando;

		// Token: 0x04000156 RID: 342
		internal bool isAbstract;

		// Token: 0x04000157 RID: 343
		private bool isAlreadyPartiallyEvaluated;

		// Token: 0x04000158 RID: 344
		private bool isCooked;

		// Token: 0x04000159 RID: 345
		private Type cookedType;

		// Token: 0x0400015A RID: 346
		private bool isExpando;

		// Token: 0x0400015B RID: 347
		internal bool isInterface;

		// Token: 0x0400015C RID: 348
		internal bool isStatic;

		// Token: 0x0400015D RID: 349
		protected bool needsEngine;

		// Token: 0x0400015E RID: 350
		internal AttributeTargets validOn;

		// Token: 0x0400015F RID: 351
		internal bool allowMultiple;

		// Token: 0x04000160 RID: 352
		protected ClassScope classob;

		// Token: 0x04000161 RID: 353
		private FunctionObject implicitDefaultConstructor;

		// Token: 0x04000162 RID: 354
		private JSVariableField ownField;

		// Token: 0x04000163 RID: 355
		protected JSMemberField[] fields;

		// Token: 0x04000164 RID: 356
		private Class superClass;

		// Token: 0x04000165 RID: 357
		private IReflect superIR;

		// Token: 0x04000166 RID: 358
		private object[] superMembers;

		// Token: 0x04000167 RID: 359
		private SimpleHashtable firstIndex;

		// Token: 0x04000168 RID: 360
		private MethodInfo fieldInitializer;

		// Token: 0x04000169 RID: 361
		internal CustomAttributeList customAttributes;

		// Token: 0x0400016A RID: 362
		internal CLSComplianceSpec clsCompliance;

		// Token: 0x0400016B RID: 363
		private bool generateCodeForExpando;

		// Token: 0x0400016C RID: 364
		private PropertyBuilder expandoItemProp;

		// Token: 0x0400016D RID: 365
		private MethodBuilder getHashTableMethod;

		// Token: 0x0400016E RID: 366
		private MethodBuilder getItem;

		// Token: 0x0400016F RID: 367
		private MethodBuilder setItem;

		// Token: 0x04000170 RID: 368
		internal MethodBuilder deleteOpMethod;

		// Token: 0x04000171 RID: 369
		private static int badTypeNameCount;
	}
}
