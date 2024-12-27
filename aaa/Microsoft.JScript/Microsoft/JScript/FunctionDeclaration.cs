using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000082 RID: 130
	public sealed class FunctionDeclaration : AST
	{
		// Token: 0x060005C7 RID: 1479 RVA: 0x00028690 File Offset: 0x00027690
		internal FunctionDeclaration(Context context, AST ifaceId, IdentifierLiteral id, ParameterDeclaration[] formal_parameters, TypeExpression return_type, Block body, FunctionScope own_scope, FieldAttributes attributes, bool isMethod, bool isGetter, bool isSetter, bool isAbstract, bool isFinal, CustomAttributeList customAttributes)
			: base(context)
		{
			MethodAttributes methodAttributes;
			if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Public)
			{
				methodAttributes = MethodAttributes.Public;
			}
			else if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Private)
			{
				methodAttributes = MethodAttributes.Private;
			}
			else if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Assembly)
			{
				methodAttributes = MethodAttributes.Assembly;
			}
			else if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.Family)
			{
				methodAttributes = MethodAttributes.Family;
			}
			else if ((attributes & FieldAttributes.FieldAccessMask) == FieldAttributes.FamORAssem)
			{
				methodAttributes = MethodAttributes.FamORAssem;
			}
			else
			{
				methodAttributes = MethodAttributes.Public;
			}
			if ((attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope || !isMethod)
			{
				methodAttributes |= MethodAttributes.Static;
			}
			else
			{
				methodAttributes |= MethodAttributes.Virtual | MethodAttributes.VtableLayoutMask;
			}
			if (isAbstract)
			{
				methodAttributes |= MethodAttributes.Abstract;
			}
			if (isFinal)
			{
				methodAttributes |= MethodAttributes.Final;
			}
			this.name = id.ToString();
			this.isMethod = isMethod;
			if (ifaceId != null)
			{
				if (isMethod)
				{
					this.ifaceId = new TypeExpression(ifaceId);
					methodAttributes &= ~MethodAttributes.MemberAccessMask;
					methodAttributes |= MethodAttributes.Private | MethodAttributes.Final;
				}
				else
				{
					this.declaringObject = new Member(ifaceId.context, ifaceId, id);
					this.name = this.declaringObject.ToString();
				}
			}
			ScriptObject scriptObject = base.Globals.ScopeStack.Peek();
			if (attributes == FieldAttributes.PrivateScope && !isAbstract && !isFinal)
			{
				if (scriptObject is ClassScope)
				{
					attributes |= FieldAttributes.Public;
				}
			}
			else if (!(scriptObject is ClassScope))
			{
				this.context.HandleError(JSError.NotInsideClass);
				attributes = FieldAttributes.PrivateScope;
				methodAttributes = MethodAttributes.Public;
			}
			if (scriptObject is ActivationObject)
			{
				this.inFastScope = ((ActivationObject)scriptObject).fast;
				string text = this.name;
				if (isGetter)
				{
					methodAttributes |= MethodAttributes.SpecialName;
					this.name = "get_" + this.name;
					if (return_type == null)
					{
						return_type = new TypeExpression(new ConstantWrapper(Typeob.Object, context));
					}
				}
				else if (isSetter)
				{
					methodAttributes |= MethodAttributes.SpecialName;
					this.name = "set_" + this.name;
					return_type = new TypeExpression(new ConstantWrapper(Typeob.Void, context));
				}
				attributes &= FieldAttributes.FieldAccessMask;
				MethodAttributes methodAttributes2 = methodAttributes & MethodAttributes.MemberAccessMask;
				if ((methodAttributes & MethodAttributes.Virtual) != MethodAttributes.PrivateScope && (methodAttributes & MethodAttributes.Final) == MethodAttributes.PrivateScope && (methodAttributes2 == MethodAttributes.Private || methodAttributes2 == MethodAttributes.Assembly || methodAttributes2 == MethodAttributes.FamANDAssem))
				{
					methodAttributes |= MethodAttributes.CheckAccessOnOverride;
				}
				this.func = new FunctionObject(this.name, formal_parameters, return_type, body, own_scope, scriptObject, this.context, methodAttributes, customAttributes, this.isMethod);
				if (this.declaringObject != null)
				{
					return;
				}
				string text2 = this.name;
				if (this.ifaceId != null)
				{
					text2 = ifaceId.ToString() + "." + text2;
				}
				JSVariableField jsvariableField = (JSVariableField)((ActivationObject)scriptObject).name_table[text2];
				if (jsvariableField != null && (!(jsvariableField is JSMemberField) || !(((JSMemberField)jsvariableField).value is FunctionObject) || this.func.isExpandoMethod))
				{
					if (text != this.name)
					{
						jsvariableField.originalContext.HandleError(JSError.ClashWithProperty);
					}
					else
					{
						id.context.HandleError(JSError.DuplicateName, this.func.isExpandoMethod);
						if (jsvariableField.value is FunctionObject)
						{
							((FunctionObject)jsvariableField.value).suppressIL = true;
						}
					}
				}
				if (this.isMethod)
				{
					if (!(jsvariableField is JSMemberField) || !(((JSMemberField)jsvariableField).value is FunctionObject) || text != this.name)
					{
						this.field = ((ActivationObject)scriptObject).AddNewField(text2, this.func, attributes | FieldAttributes.Literal);
						if (text == this.name)
						{
							this.field.type = new TypeExpression(new ConstantWrapper(Typeob.FunctionWrapper, this.context));
						}
					}
					else
					{
						this.field = ((JSMemberField)jsvariableField).AddOverload(this.func, attributes | FieldAttributes.Literal);
					}
				}
				else if (scriptObject is FunctionScope)
				{
					if (this.inFastScope)
					{
						attributes |= FieldAttributes.Literal;
					}
					this.field = ((FunctionScope)scriptObject).AddNewField(this.name, attributes, this.func);
					if (this.field is JSLocalField)
					{
						JSLocalField jslocalField = (JSLocalField)this.field;
						if (this.inFastScope)
						{
							jslocalField.type = new TypeExpression(new ConstantWrapper(Typeob.ScriptFunction, this.context));
							jslocalField.attributeFlags |= FieldAttributes.Literal;
						}
						jslocalField.debugOn = this.context.document.debugOn;
						jslocalField.isDefined = true;
					}
				}
				else if (this.inFastScope)
				{
					this.field = ((ActivationObject)scriptObject).AddNewField(this.name, this.func, attributes | FieldAttributes.Literal);
					this.field.type = new TypeExpression(new ConstantWrapper(Typeob.ScriptFunction, this.context));
				}
				else
				{
					this.field = ((ActivationObject)scriptObject).AddNewField(this.name, this.func, attributes | FieldAttributes.Static);
				}
				this.field.originalContext = context;
				if (text != this.name)
				{
					string text3 = text;
					if (this.ifaceId != null)
					{
						text3 = ifaceId.ToString() + "." + text;
					}
					FieldInfo fieldInfo = (FieldInfo)((ClassScope)scriptObject).name_table[text3];
					if (fieldInfo != null)
					{
						if (fieldInfo.IsLiteral)
						{
							object value = ((JSVariableField)fieldInfo).value;
							if (value is JSProperty)
							{
								this.enclosingProperty = (JSProperty)value;
							}
						}
						if (this.enclosingProperty == null)
						{
							id.context.HandleError(JSError.DuplicateName, true);
						}
					}
					if (this.enclosingProperty == null)
					{
						this.enclosingProperty = new JSProperty(text);
						fieldInfo = ((ActivationObject)scriptObject).AddNewField(text3, this.enclosingProperty, attributes | FieldAttributes.Literal);
						((JSMemberField)fieldInfo).originalContext = this.context;
					}
					else if ((isGetter && this.enclosingProperty.getter != null) || (isSetter && this.enclosingProperty.setter != null))
					{
						id.context.HandleError(JSError.DuplicateName, true);
					}
					if (isGetter)
					{
						this.enclosingProperty.getter = new JSFieldMethod(this.field, scriptObject);
						return;
					}
					this.enclosingProperty.setter = new JSFieldMethod(this.field, scriptObject);
					return;
				}
			}
			else
			{
				this.inFastScope = false;
				this.func = new FunctionObject(this.name, formal_parameters, return_type, body, own_scope, scriptObject, this.context, MethodAttributes.Public, null, false);
				this.field = ((StackFrame)scriptObject).AddNewField(this.name, new Closure(this.func), attributes | FieldAttributes.Static);
			}
		}

		// Token: 0x060005C8 RID: 1480 RVA: 0x00028CCC File Offset: 0x00027CCC
		internal override object Evaluate()
		{
			if (this.declaringObject != null)
			{
				this.declaringObject.SetValue(this.func);
			}
			return this.completion;
		}

		// Token: 0x060005C9 RID: 1481 RVA: 0x00028CF0 File Offset: 0x00027CF0
		public static Closure JScriptFunctionDeclaration(RuntimeTypeHandle handle, string name, string method_name, string[] formal_parameters, JSLocalField[] fields, bool must_save_stack_locals, bool hasArgumentsObject, string text, object declaringObject, VsaEngine engine)
		{
			Type typeFromHandle = Type.GetTypeFromHandle(handle);
			FunctionObject functionObject = new FunctionObject(typeFromHandle, name, method_name, formal_parameters, fields, must_save_stack_locals, hasArgumentsObject, text, engine);
			return new Closure(functionObject, declaringObject);
		}

		// Token: 0x060005CA RID: 1482 RVA: 0x00028D20 File Offset: 0x00027D20
		internal override Context GetFirstExecutableContext()
		{
			return null;
		}

		// Token: 0x060005CB RID: 1483 RVA: 0x00028D24 File Offset: 0x00027D24
		internal override AST PartiallyEvaluate()
		{
			if (this.ifaceId != null)
			{
				this.ifaceId.PartiallyEvaluate();
				this.func.implementedIface = this.ifaceId.ToIReflect();
				Type type = this.func.implementedIface as Type;
				ClassScope classScope = this.func.implementedIface as ClassScope;
				if ((type != null && !type.IsInterface) || (classScope != null && !classScope.owner.isInterface))
				{
					this.ifaceId.context.HandleError(JSError.NeedInterface);
					this.func.implementedIface = null;
				}
				if ((this.func.attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope)
				{
					this.func.funcContext.HandleError(JSError.AbstractCannotBePrivate);
				}
			}
			else if (this.declaringObject != null)
			{
				this.declaringObject.PartiallyEvaluateAsCallable();
			}
			this.func.PartiallyEvaluate();
			if (this.inFastScope && this.func.isExpandoMethod && this.field != null && this.field.type != null)
			{
				this.field.type.expression = new ConstantWrapper(Typeob.ScriptFunction, null);
			}
			if ((this.func.attributes & MethodAttributes.Abstract) != MethodAttributes.PrivateScope && !((ClassScope)this.func.enclosing_scope).owner.isAbstract)
			{
				((ClassScope)this.func.enclosing_scope).owner.attributes |= TypeAttributes.Abstract;
				((ClassScope)this.func.enclosing_scope).owner.context.HandleError(JSError.CannotBeAbstract, this.name);
			}
			if (this.enclosingProperty != null && !this.enclosingProperty.GetterAndSetterAreConsistent())
			{
				this.context.HandleError(JSError.GetAndSetAreInconsistent);
			}
			return this;
		}

		// Token: 0x060005CC RID: 1484 RVA: 0x00028EF0 File Offset: 0x00027EF0
		private void TranslateToILClosure(ILGenerator il)
		{
			if (!this.func.isStatic)
			{
				il.Emit(OpCodes.Ldarg_0);
			}
			il.Emit(OpCodes.Ldtoken, (this.func.classwriter != null) ? this.func.classwriter : base.compilerGlobals.classwriter);
			il.Emit(OpCodes.Ldstr, this.name);
			il.Emit(OpCodes.Ldstr, this.func.GetName());
			int num = this.func.formal_parameters.Length;
			ConstantWrapper.TranslateToILInt(il, num);
			il.Emit(OpCodes.Newarr, Typeob.String);
			for (int i = 0; i < num; i++)
			{
				il.Emit(OpCodes.Dup);
				ConstantWrapper.TranslateToILInt(il, i);
				il.Emit(OpCodes.Ldstr, this.func.formal_parameters[i]);
				il.Emit(OpCodes.Stelem_Ref);
			}
			num = this.func.fields.Length;
			ConstantWrapper.TranslateToILInt(il, num);
			il.Emit(OpCodes.Newarr, Typeob.JSLocalField);
			for (int j = 0; j < num; j++)
			{
				JSLocalField jslocalField = this.func.fields[j];
				il.Emit(OpCodes.Dup);
				ConstantWrapper.TranslateToILInt(il, j);
				il.Emit(OpCodes.Ldstr, jslocalField.Name);
				il.Emit(OpCodes.Ldtoken, jslocalField.FieldType);
				ConstantWrapper.TranslateToILInt(il, jslocalField.slotNumber);
				il.Emit(OpCodes.Newobj, CompilerGlobals.jsLocalFieldConstructor);
				il.Emit(OpCodes.Stelem_Ref);
			}
			if (this.func.must_save_stack_locals)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			if (this.func.hasArgumentsObject)
			{
				il.Emit(OpCodes.Ldc_I4_1);
			}
			else
			{
				il.Emit(OpCodes.Ldc_I4_0);
			}
			il.Emit(OpCodes.Ldstr, this.func.ToString());
			if (!this.func.isStatic)
			{
				il.Emit(OpCodes.Ldarg_0);
			}
			else
			{
				il.Emit(OpCodes.Ldnull);
			}
			base.EmitILToLoadEngine(il);
			il.Emit(OpCodes.Call, CompilerGlobals.jScriptFunctionDeclarationMethod);
		}

		// Token: 0x060005CD RID: 1485 RVA: 0x00029108 File Offset: 0x00028108
		internal override void TranslateToIL(ILGenerator il, Type rtype)
		{
		}

		// Token: 0x060005CE RID: 1486 RVA: 0x0002910C File Offset: 0x0002810C
		internal override void TranslateToILInitializer(ILGenerator il)
		{
			if (this.func.suppressIL)
			{
				return;
			}
			this.func.TranslateToIL(base.compilerGlobals);
			if (this.declaringObject != null)
			{
				this.declaringObject.TranslateToILInitializer(il);
				this.declaringObject.TranslateToILPreSet(il);
				this.TranslateToILClosure(il);
				this.declaringObject.TranslateToILSet(il);
				return;
			}
			object metaData = this.field.metaData;
			if (this.func.isMethod)
			{
				if (metaData is FunctionDeclaration)
				{
					this.field.metaData = null;
					return;
				}
				this.TranslateToILSourceTextProvider();
				return;
			}
			else
			{
				if (metaData == null)
				{
					return;
				}
				this.TranslateToILClosure(il);
				if (metaData is LocalBuilder)
				{
					il.Emit(OpCodes.Stloc, (LocalBuilder)metaData);
					return;
				}
				if (this.func.isStatic)
				{
					il.Emit(OpCodes.Stsfld, (FieldInfo)metaData);
					return;
				}
				il.Emit(OpCodes.Stfld, (FieldInfo)metaData);
				return;
			}
		}

		// Token: 0x060005CF RID: 1487 RVA: 0x000291F8 File Offset: 0x000281F8
		private void TranslateToILSourceTextProvider()
		{
			if (base.Engine.doFast)
			{
				return;
			}
			if (string.Compare(this.name, this.field.Name, StringComparison.Ordinal) != 0)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder(this.func.ToString());
			for (JSMemberField jsmemberField = ((JSMemberField)this.field).nextOverload; jsmemberField != null; jsmemberField = jsmemberField.nextOverload)
			{
				jsmemberField.metaData = this;
				stringBuilder.Append('\n');
				stringBuilder.Append(jsmemberField.value.ToString());
			}
			MethodAttributes methodAttributes = MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static;
			MethodBuilder methodBuilder = ((ClassScope)this.func.enclosing_scope).GetTypeBuilder().DefineMethod(this.name + " source", methodAttributes, Typeob.String, new Type[0]);
			ILGenerator ilgenerator = methodBuilder.GetILGenerator();
			ilgenerator.Emit(OpCodes.Ldstr, stringBuilder.ToString());
			ilgenerator.Emit(OpCodes.Ret);
		}

		// Token: 0x04000282 RID: 642
		internal FunctionObject func;

		// Token: 0x04000283 RID: 643
		private Member declaringObject;

		// Token: 0x04000284 RID: 644
		private TypeExpression ifaceId;

		// Token: 0x04000285 RID: 645
		private string name;

		// Token: 0x04000286 RID: 646
		internal bool isMethod;

		// Token: 0x04000287 RID: 647
		private bool inFastScope;

		// Token: 0x04000288 RID: 648
		private JSVariableField field;

		// Token: 0x04000289 RID: 649
		internal JSProperty enclosingProperty;

		// Token: 0x0400028A RID: 650
		private Completion completion = new Completion();
	}
}
