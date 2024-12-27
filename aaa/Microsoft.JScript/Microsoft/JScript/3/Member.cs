using System;
using System.Reflection;
using System.Reflection.Emit;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x020000E6 RID: 230
	internal sealed class Member : Binding
	{
		// Token: 0x06000A3F RID: 2623 RVA: 0x0004DC50 File Offset: 0x0004CC50
		internal Member(Context context, AST rootObject, AST memberName)
			: base(context, memberName.context.GetCode())
		{
			this.fast = base.Engine.doFast;
			this.isImplicitWrapper = false;
			this.isNonVirtual = rootObject is ThisLiteral && ((ThisLiteral)rootObject).isSuper;
			this.lateBinding = null;
			this.memberNameContext = memberName.context;
			this.rootObject = rootObject;
			this.rootObjectInferredType = null;
			this.refLoc = null;
			this.temp = null;
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x0004DCD4 File Offset: 0x0004CCD4
		private void BindName(JSField inferenceTarget)
		{
			this.rootObject = this.rootObject.PartiallyEvaluate();
			IReflect reflect = (this.rootObjectInferredType = this.rootObject.InferType(inferenceTarget));
			if (this.rootObject is ConstantWrapper)
			{
				object obj = Convert.ToObject2(this.rootObject.Evaluate(), base.Engine);
				if (obj == null)
				{
					this.rootObject.context.HandleError(JSError.ObjectExpected);
					return;
				}
				ClassScope classScope = obj as ClassScope;
				Type type = obj as Type;
				if (classScope != null || type != null)
				{
					MemberInfo[] array;
					if (classScope != null)
					{
						array = (this.members = classScope.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
					}
					else
					{
						array = (this.members = type.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic));
					}
					if (array.Length > 0)
					{
						return;
					}
					this.members = Typeob.Type.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public);
					return;
				}
				else
				{
					Namespace @namespace = obj as Namespace;
					if (@namespace != null)
					{
						string text = @namespace.Name + "." + this.name;
						classScope = base.Engine.GetClass(text);
						if (classScope != null)
						{
							FieldAttributes fieldAttributes = FieldAttributes.Literal;
							if ((classScope.owner.attributes & TypeAttributes.Public) == TypeAttributes.NotPublic)
							{
								fieldAttributes |= FieldAttributes.Private;
							}
							this.members = new MemberInfo[]
							{
								new JSGlobalField(null, this.name, classScope, fieldAttributes)
							};
							return;
						}
						type = base.Engine.GetType(text);
						if (type != null)
						{
							this.members = new MemberInfo[] { type };
							return;
						}
					}
					else if (obj is MathObject || (obj is ScriptFunction && !(obj is FunctionObject)))
					{
						reflect = (IReflect)obj;
					}
				}
			}
			reflect = this.ProvideWrapperForPrototypeProperties(reflect);
			if (reflect == Typeob.Object && !this.isNonVirtual)
			{
				this.members = new MemberInfo[0];
				return;
			}
			Type type2 = reflect as Type;
			if (type2 != null && type2.IsInterface)
			{
				this.members = JSBinder.GetInterfaceMembers(this.name, type2);
				return;
			}
			ClassScope classScope2 = reflect as ClassScope;
			if (classScope2 != null && classScope2.owner.isInterface)
			{
				this.members = classScope2.owner.GetInterfaceMember(this.name);
				return;
			}
			while (reflect != null)
			{
				classScope2 = reflect as ClassScope;
				if (classScope2 != null)
				{
					MemberInfo[] array = (this.members = reflect.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy));
					if (array.Length > 0)
					{
						return;
					}
					reflect = classScope2.GetSuperType();
				}
				else
				{
					type2 = reflect as Type;
					if (type2 == null)
					{
						this.members = reflect.GetMember(this.name, BindingFlags.Instance | BindingFlags.Public);
						return;
					}
					MemberInfo[] array = (this.members = type2.GetMember(this.name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
					if (array.Length > 0)
					{
						if (LateBinding.SelectMember(array) == null)
						{
							array = (this.members = type2.GetMember(this.name, MemberTypes.Method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic));
							if (array.Length == 0)
							{
								this.members = type2.GetMember(this.name, MemberTypes.Property, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
							}
						}
						return;
					}
					reflect = type2.BaseType;
				}
			}
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x0004DFCC File Offset: 0x0004CFCC
		internal override object Evaluate()
		{
			object obj = base.Evaluate();
			if (obj is Missing)
			{
				obj = null;
			}
			return obj;
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x0004DFEC File Offset: 0x0004CFEC
		internal override LateBinding EvaluateAsLateBinding()
		{
			LateBinding lateBinding = this.lateBinding;
			if (lateBinding == null)
			{
				if (this.member != null && !this.rootObjectInferredType.Equals(this.rootObject.InferType(null)))
				{
					base.InvalidateBinding();
				}
				lateBinding = (this.lateBinding = new LateBinding(this.name, null, VsaEngine.executeForJSEE));
				lateBinding.last_member = this.member;
			}
			object obj = this.rootObject.Evaluate();
			try
			{
				obj = (lateBinding.obj = Convert.ToObject(obj, base.Engine));
				if (this.defaultMember == null && this.member != null)
				{
					lateBinding.last_object = obj;
				}
			}
			catch (JScriptException ex)
			{
				if (ex.context == null)
				{
					ex.context = this.rootObject.context;
				}
				throw ex;
			}
			return lateBinding;
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x0004E0B8 File Offset: 0x0004D0B8
		internal object EvaluateAsType()
		{
			WrappedNamespace wrappedNamespace = this.rootObject.EvaluateAsWrappedNamespace(false);
			object memberValue = wrappedNamespace.GetMemberValue(this.name);
			if (memberValue != null && !(memberValue is Missing))
			{
				return memberValue;
			}
			Member member = this.rootObject as Member;
			object obj;
			if (member == null)
			{
				Lookup lookup = this.rootObject as Lookup;
				if (lookup == null)
				{
					return null;
				}
				obj = lookup.PartiallyEvaluate();
				ConstantWrapper constantWrapper = obj as ConstantWrapper;
				if (constantWrapper != null)
				{
					obj = constantWrapper.value;
				}
				else
				{
					JSGlobalField jsglobalField = lookup.member as JSGlobalField;
					if (jsglobalField == null || !jsglobalField.IsLiteral)
					{
						return null;
					}
					obj = jsglobalField.value;
				}
			}
			else
			{
				obj = member.EvaluateAsType();
			}
			ClassScope classScope = obj as ClassScope;
			if (classScope != null)
			{
				MemberInfo[] member2 = classScope.GetMember(this.name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (member2.Length == 0)
				{
					return null;
				}
				JSMemberField jsmemberField = member2[0] as JSMemberField;
				if (jsmemberField == null || !jsmemberField.IsLiteral || !(jsmemberField.value is ClassScope) || (!jsmemberField.IsPublic && !jsmemberField.IsAccessibleFrom(base.Engine.ScriptObjectStackTop())))
				{
					return null;
				}
				return jsmemberField.value;
			}
			else
			{
				Type type = obj as Type;
				if (type != null)
				{
					return type.GetNestedType(this.name);
				}
				return null;
			}
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x0004E1EC File Offset: 0x0004D1EC
		internal override WrappedNamespace EvaluateAsWrappedNamespace(bool giveErrorIfNameInUse)
		{
			WrappedNamespace wrappedNamespace = this.rootObject.EvaluateAsWrappedNamespace(giveErrorIfNameInUse);
			string name = this.name;
			wrappedNamespace.AddFieldOrUseExistingField(name, Namespace.GetNamespace(wrappedNamespace.ToString() + "." + name, base.Engine), FieldAttributes.Literal);
			return new WrappedNamespace(wrappedNamespace.ToString() + "." + name, base.Engine);
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x0004E24F File Offset: 0x0004D24F
		protected override object GetObject()
		{
			return Convert.ToObject(this.rootObject.Evaluate(), base.Engine);
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x0004E268 File Offset: 0x0004D268
		protected override void HandleNoSuchMemberError()
		{
			IReflect reflect = this.rootObject.InferType(null);
			object obj = null;
			if (this.rootObject is ConstantWrapper)
			{
				obj = this.rootObject.Evaluate();
			}
			if ((reflect == Typeob.Object && !this.isNonVirtual) || (reflect is JSObject && !((JSObject)reflect).noExpando) || (reflect is GlobalScope && !((GlobalScope)reflect).isKnownAtCompileTime))
			{
				return;
			}
			if (reflect is Type)
			{
				Type type = (Type)reflect;
				if (Typeob.ScriptFunction.IsAssignableFrom(type) || type == Typeob.MathObject)
				{
					this.memberNameContext.HandleError(JSError.OLENoPropOrMethod);
					return;
				}
				if (Typeob.IExpando.IsAssignableFrom(type))
				{
					return;
				}
				if (!this.fast && (type == Typeob.Boolean || type == Typeob.String || Convert.IsPrimitiveNumericType(type)))
				{
					return;
				}
				if (obj is ClassScope)
				{
					MemberInfo[] member = ((ClassScope)obj).GetMember(this.name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
					if (member.Length > 0)
					{
						this.memberNameContext.HandleError(JSError.NonStaticWithTypeName);
						return;
					}
				}
			}
			if (obj is FunctionObject)
			{
				this.rootObject = new ConstantWrapper(((FunctionObject)obj).name, this.rootObject.context);
				this.memberNameContext.HandleError(JSError.OLENoPropOrMethod);
				return;
			}
			if (reflect is ClassScope)
			{
				MemberInfo[] member2 = ((ClassScope)reflect).GetMember(this.name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
				if (member2.Length > 0)
				{
					this.memberNameContext.HandleError(JSError.StaticRequiresTypeName);
					return;
				}
			}
			if (obj is Type)
			{
				this.memberNameContext.HandleError(JSError.NoSuchStaticMember, Convert.ToTypeName((Type)obj));
				return;
			}
			if (obj is ClassScope)
			{
				this.memberNameContext.HandleError(JSError.NoSuchStaticMember, Convert.ToTypeName((ClassScope)obj));
				return;
			}
			if (obj is Namespace)
			{
				this.memberNameContext.HandleError(JSError.NoSuchType, ((Namespace)obj).Name + "." + this.name);
				return;
			}
			if (reflect == FunctionPrototype.ob && this.rootObject is Binding && ((Binding)this.rootObject).member is JSVariableField && ((JSVariableField)((Binding)this.rootObject).member).value is FunctionObject)
			{
				return;
			}
			this.memberNameContext.HandleError(JSError.NoSuchMember, Convert.ToTypeName(reflect));
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x0004E4C1 File Offset: 0x0004D4C1
		internal override IReflect InferType(JSField inference_target)
		{
			if (this.members == null)
			{
				this.BindName(inference_target);
			}
			else if (!this.rootObjectInferredType.Equals(this.rootObject.InferType(inference_target)))
			{
				base.InvalidateBinding();
			}
			return base.InferType(null);
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0004E4FA File Offset: 0x0004D4FA
		internal override IReflect InferTypeOfCall(JSField inference_target, bool isConstructor)
		{
			if (!this.rootObjectInferredType.Equals(this.rootObject.InferType(inference_target)))
			{
				base.InvalidateBinding();
			}
			return base.InferTypeOfCall(null, isConstructor);
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0004E524 File Offset: 0x0004D524
		internal override AST PartiallyEvaluate()
		{
			this.BindName(null);
			if (this.members == null || this.members.Length == 0)
			{
				if (this.rootObject is ConstantWrapper)
				{
					object obj = this.rootObject.Evaluate();
					if (obj is Namespace)
					{
						return new ConstantWrapper(Namespace.GetNamespace(((Namespace)obj).Name + "." + this.name, base.Engine), this.context);
					}
				}
				this.HandleNoSuchMemberError();
				return this;
			}
			base.ResolveRHValue();
			if (this.member is FieldInfo && ((FieldInfo)this.member).IsLiteral)
			{
				object obj2 = ((this.member is JSVariableField) ? ((JSVariableField)this.member).value : TypeReferences.GetConstantValue((FieldInfo)this.member));
				if (obj2 is AST)
				{
					AST ast = ((AST)obj2).PartiallyEvaluate();
					if (ast is ConstantWrapper)
					{
						return ast;
					}
					obj2 = null;
				}
				if (!(obj2 is FunctionObject) && (!(obj2 is ClassScope) || ((ClassScope)obj2).owner.IsStatic))
				{
					return new ConstantWrapper(obj2, this.context);
				}
			}
			else if (this.member is Type)
			{
				return new ConstantWrapper(this.member, this.context);
			}
			return this;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0004E66C File Offset: 0x0004D66C
		internal override AST PartiallyEvaluateAsCallable()
		{
			this.BindName(null);
			return this;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0004E678 File Offset: 0x0004D678
		internal override AST PartiallyEvaluateAsReference()
		{
			this.BindName(null);
			if (this.members == null || this.members.Length == 0)
			{
				if (this.isImplicitWrapper && !Convert.IsArray(this.rootObjectInferredType))
				{
					this.context.HandleError(JSError.UselessAssignment);
				}
				else
				{
					this.HandleNoSuchMemberError();
				}
				return this;
			}
			base.ResolveLHValue();
			if (this.isImplicitWrapper && (this.member == null || (!(this.member is JSField) && Typeob.JSObject.IsAssignableFrom(this.member.DeclaringType))))
			{
				this.context.HandleError(JSError.UselessAssignment);
			}
			return this;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0004E718 File Offset: 0x0004D718
		private IReflect ProvideWrapperForPrototypeProperties(IReflect obType)
		{
			if (obType == Typeob.String)
			{
				obType = base.Globals.globalObject.originalString.Construct();
				((JSObject)obType).noExpando = this.fast;
				this.isImplicitWrapper = true;
			}
			else if ((obType is Type && Typeob.Array.IsAssignableFrom((Type)obType)) || obType is TypedArray)
			{
				obType = base.Globals.globalObject.originalArray.ConstructWrapper();
				((JSObject)obType).noExpando = this.fast;
				this.isImplicitWrapper = true;
			}
			else if (obType == Typeob.Boolean)
			{
				obType = base.Globals.globalObject.originalBoolean.Construct();
				((JSObject)obType).noExpando = this.fast;
				this.isImplicitWrapper = true;
			}
			else if (Convert.IsPrimitiveNumericType(obType))
			{
				Type type = (Type)obType;
				obType = base.Globals.globalObject.originalNumber.Construct();
				((JSObject)obType).noExpando = this.fast;
				((NumberObject)obType).baseType = type;
				this.isImplicitWrapper = true;
			}
			else if (obType is Type)
			{
				obType = Convert.ToIReflect((Type)obType, base.Engine);
			}
			return obType;
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0004E858 File Offset: 0x0004D858
		internal override object ResolveCustomAttribute(ASTList args, IReflect[] argIRs, AST target)
		{
			this.name += "Attribute";
			this.BindName(null);
			if (this.members == null || this.members.Length == 0)
			{
				this.name = this.name.Substring(0, this.name.Length - 9);
				this.BindName(null);
			}
			return base.ResolveCustomAttribute(args, argIRs, target);
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0004E8C4 File Offset: 0x0004D8C4
		public override string ToString()
		{
			return this.rootObject.ToString() + "." + this.name;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0004E8E4 File Offset: 0x0004D8E4
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			this.rootObject.TranslateToILInitializer(il);
			if (!this.rootObjectInferredType.Equals(this.rootObject.InferType(null)))
			{
				base.InvalidateBinding();
			}
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
								goto IL_0084;
							}
						}
						else
						{
							if (this.member is JSExpandoField)
							{
								this.member = null;
								goto IL_0084;
							}
							return;
						}
					}
				}
				else if (memberType != MemberTypes.Property && memberType != MemberTypes.TypeInfo && memberType != MemberTypes.NestedType)
				{
					goto IL_0084;
				}
				return;
			}
			IL_0084:
			this.refLoc = il.DeclareLocal(Typeob.LateBinding);
			il.Emit(OpCodes.Ldstr, this.name);
			il.Emit(OpCodes.Newobj, CompilerGlobals.lateBindingConstructor);
			il.Emit(OpCodes.Stloc, this.refLoc);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0004E9B8 File Offset: 0x0004D9B8
		protected override void TranslateToILObject(ILGenerator il, Type obType, bool noValue)
		{
			if (noValue && obType.IsValueType && obType != Typeob.Enum)
			{
				if (this.temp == null)
				{
					this.rootObject.TranslateToILReference(il, obType);
					return;
				}
				Type type = Convert.ToType(this.rootObject.InferType(null));
				if (type == obType)
				{
					il.Emit(OpCodes.Ldloca, this.temp);
					return;
				}
				il.Emit(OpCodes.Ldloc, this.temp);
				Convert.Emit(this, il, type, obType);
				Convert.EmitLdloca(il, obType);
				return;
			}
			else
			{
				if (this.temp == null || this.rootObject is ThisLiteral)
				{
					this.rootObject.TranslateToIL(il, obType);
					return;
				}
				il.Emit(OpCodes.Ldloc, this.temp);
				Type type2 = Convert.ToType(this.rootObject.InferType(null));
				Convert.Emit(this, il, type2, obType);
				return;
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0004EA88 File Offset: 0x0004DA88
		protected override void TranslateToILWithDupOfThisOb(ILGenerator il)
		{
			IReflect reflect = this.rootObject.InferType(null);
			Type type = Convert.ToType(reflect);
			this.rootObject.TranslateToIL(il, type);
			if (reflect == Typeob.Object || reflect == Typeob.String || reflect is TypedArray || (reflect == type && Typeob.Array.IsAssignableFrom(type)))
			{
				type = Typeob.Object;
				base.EmitILToLoadEngine(il);
				il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
			}
			il.Emit(OpCodes.Dup);
			this.temp = il.DeclareLocal(type);
			il.Emit(OpCodes.Stloc, this.temp);
			Convert.Emit(this, il, type, Typeob.Object);
			this.TranslateToIL(il, Typeob.Object);
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x0004EB40 File Offset: 0x0004DB40
		internal void TranslateToLateBinding(ILGenerator il, bool speculativeEarlyBindingsExist)
		{
			if (speculativeEarlyBindingsExist)
			{
				LocalBuilder localBuilder = il.DeclareLocal(Typeob.Object);
				il.Emit(OpCodes.Stloc, localBuilder);
				il.Emit(OpCodes.Ldloc, this.refLoc);
				il.Emit(OpCodes.Dup);
				il.Emit(OpCodes.Ldloc, localBuilder);
			}
			else
			{
				il.Emit(OpCodes.Ldloc, this.refLoc);
				il.Emit(OpCodes.Dup);
				this.TranslateToILObject(il, Typeob.Object, false);
			}
			IReflect reflect = this.rootObject.InferType(null);
			if (reflect == Typeob.Object || reflect == Typeob.String || reflect is TypedArray || (reflect is Type && ((Type)reflect).IsPrimitive) || (reflect is Type && Typeob.Array.IsAssignableFrom((Type)reflect)))
			{
				base.EmitILToLoadEngine(il);
				il.Emit(OpCodes.Call, CompilerGlobals.toObjectMethod);
			}
			il.Emit(OpCodes.Stfld, CompilerGlobals.objectField);
		}

		// Token: 0x04000661 RID: 1633
		private bool fast;

		// Token: 0x04000662 RID: 1634
		private bool isImplicitWrapper;

		// Token: 0x04000663 RID: 1635
		private LateBinding lateBinding;

		// Token: 0x04000664 RID: 1636
		private Context memberNameContext;

		// Token: 0x04000665 RID: 1637
		internal AST rootObject;

		// Token: 0x04000666 RID: 1638
		private IReflect rootObjectInferredType;

		// Token: 0x04000667 RID: 1639
		private LocalBuilder refLoc;

		// Token: 0x04000668 RID: 1640
		private LocalBuilder temp;
	}
}
