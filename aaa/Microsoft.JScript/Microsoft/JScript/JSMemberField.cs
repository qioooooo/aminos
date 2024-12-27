using System;
using System.Globalization;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000B2 RID: 178
	internal sealed class JSMemberField : JSVariableField
	{
		// Token: 0x06000809 RID: 2057 RVA: 0x000383A0 File Offset: 0x000373A0
		internal JSMemberField(ClassScope obj, string name, object value, FieldAttributes attributeFlags)
			: base(name, obj, attributeFlags)
		{
			this.value = value;
			this.nextOverload = null;
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x000383BC File Offset: 0x000373BC
		internal JSMemberField AddOverload(FunctionObject func, FieldAttributes attributeFlags)
		{
			JSMemberField jsmemberField = this;
			while (jsmemberField.nextOverload != null)
			{
				jsmemberField = jsmemberField.nextOverload;
			}
			JSMemberField jsmemberField2 = (jsmemberField.nextOverload = new JSMemberField((ClassScope)this.obj, this.Name, func, attributeFlags));
			jsmemberField2.type = this.type;
			return jsmemberField2;
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x0003840C File Offset: 0x0003740C
		internal void AddOverloadedMembers(MemberInfoList mems, ClassScope scope, BindingFlags attrs)
		{
			JSMemberField jsmemberField = this;
			while (jsmemberField != null)
			{
				MethodInfo asMethod = jsmemberField.GetAsMethod(scope);
				if (asMethod.IsStatic)
				{
					if ((attrs & BindingFlags.Static) != BindingFlags.Default)
					{
						goto IL_0020;
					}
				}
				else if ((attrs & BindingFlags.Instance) != BindingFlags.Default)
				{
					goto IL_0020;
				}
				IL_003D:
				jsmemberField = jsmemberField.nextOverload;
				continue;
				IL_0020:
				if (asMethod.IsPublic)
				{
					if ((attrs & BindingFlags.Public) == BindingFlags.Default)
					{
						goto IL_003D;
					}
				}
				else if ((attrs & BindingFlags.NonPublic) == BindingFlags.Default)
				{
					goto IL_003D;
				}
				mems.Add(asMethod);
				goto IL_003D;
			}
			if ((attrs & BindingFlags.DeclaredOnly) != BindingFlags.Default && (attrs & BindingFlags.FlattenHierarchy) == BindingFlags.Default)
			{
				return;
			}
			IReflect superType = scope.GetSuperType();
			MemberInfo[] member = superType.GetMember(this.Name, attrs & ~BindingFlags.DeclaredOnly);
			foreach (MemberInfo memberInfo in member)
			{
				if (memberInfo.MemberType == MemberTypes.Method)
				{
					mems.Add(memberInfo);
				}
			}
		}

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x0600080C RID: 2060 RVA: 0x000384B4 File Offset: 0x000374B4
		public override FieldAttributes Attributes
		{
			get
			{
				if ((this.attributeFlags & FieldAttributes.Literal) == FieldAttributes.PrivateScope)
				{
					return this.attributeFlags;
				}
				if (this.value is FunctionObject && !((FunctionObject)this.value).isStatic)
				{
					return this.attributeFlags;
				}
				if (!(this.value is JSProperty))
				{
					return this.attributeFlags;
				}
				JSProperty jsproperty = (JSProperty)this.value;
				if (jsproperty.getter != null && !jsproperty.getter.IsStatic)
				{
					return this.attributeFlags;
				}
				if (jsproperty.setter != null && !jsproperty.setter.IsStatic)
				{
					return this.attributeFlags;
				}
				return this.attributeFlags | FieldAttributes.Static;
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00038560 File Offset: 0x00037560
		internal void CheckOverloadsForDuplicates()
		{
			for (JSMemberField jsmemberField = this; jsmemberField != null; jsmemberField = jsmemberField.nextOverload)
			{
				FunctionObject functionObject = jsmemberField.value as FunctionObject;
				if (functionObject == null)
				{
					return;
				}
				for (JSMemberField jsmemberField2 = jsmemberField.nextOverload; jsmemberField2 != null; jsmemberField2 = jsmemberField2.nextOverload)
				{
					FunctionObject functionObject2 = (FunctionObject)jsmemberField2.value;
					if (functionObject2.implementedIface == functionObject.implementedIface && Class.ParametersMatch(functionObject2.parameter_declarations, functionObject.parameter_declarations))
					{
						functionObject.funcContext.HandleError(JSError.DuplicateMethod);
						functionObject2.funcContext.HandleError(JSError.DuplicateMethod);
						break;
					}
				}
			}
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x000385ED File Offset: 0x000375ED
		internal override object GetMetaData()
		{
			if (this.metaData == null)
			{
				((ClassScope)this.obj).GetTypeBuilderOrEnumBuilder();
			}
			return this.metaData;
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x0003860E File Offset: 0x0003760E
		public override object GetValue(object obj)
		{
			if (obj is StackFrame)
			{
				return this.GetValue(((StackFrame)obj).closureInstance, (StackFrame)obj);
			}
			if (obj is ScriptObject)
			{
				return this.GetValue(obj, (ScriptObject)obj);
			}
			return this.GetValue(obj, null);
		}

		// Token: 0x06000810 RID: 2064 RVA: 0x00038650 File Offset: 0x00037650
		private object GetValue(object obj, ScriptObject scope)
		{
			if (base.IsStatic || base.IsLiteral)
			{
				return this.value;
			}
			if (this.obj != obj)
			{
				JSObject jsobject = obj as JSObject;
				if (jsobject != null)
				{
					FieldInfo field = jsobject.GetField(this.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
					if (field != null)
					{
						return field.GetValue(obj);
					}
					if (jsobject.outer_class_instance != null)
					{
						return this.GetValue(jsobject.outer_class_instance, null);
					}
				}
				throw new TargetException();
			}
			if (base.IsPublic || (scope != null && this.IsAccessibleFrom(scope)))
			{
				return this.value;
			}
			if (((JSObject)this.obj).noExpando)
			{
				throw new JScriptException(JSError.NotAccessible, new Context(new DocumentContext("", null), this.Name));
			}
			return this.expandoValue;
		}

		// Token: 0x06000811 RID: 2065 RVA: 0x00038714 File Offset: 0x00037714
		internal bool IsAccessibleFrom(ScriptObject scope)
		{
			while (scope != null && !(scope is ClassScope))
			{
				scope = scope.GetParent();
			}
			ClassScope classScope;
			if (this.obj is ClassScope)
			{
				classScope = (ClassScope)this.obj;
			}
			else
			{
				classScope = (ClassScope)this.obj.GetParent();
			}
			if (base.IsPrivate)
			{
				return scope != null && (scope == classScope || ((ClassScope)scope).IsNestedIn(classScope, base.IsStatic));
			}
			if (base.IsFamily)
			{
				return scope != null && (((ClassScope)scope).IsSameOrDerivedFrom(classScope) || ((ClassScope)scope).IsNestedIn(classScope, base.IsStatic));
			}
			if (base.IsFamilyOrAssembly && scope != null && (((ClassScope)scope).IsSameOrDerivedFrom(classScope) || ((ClassScope)scope).IsNestedIn(classScope, base.IsStatic)))
			{
				return true;
			}
			if (scope == null)
			{
				return classScope.GetPackage() == null;
			}
			return classScope.GetPackage() == ((ClassScope)scope).GetPackage();
		}

		// Token: 0x06000812 RID: 2066 RVA: 0x0003880C File Offset: 0x0003780C
		internal ConstructorInfo[] GetAsConstructors(object proto)
		{
			JSMemberField jsmemberField = this;
			int num = 0;
			while (jsmemberField != null)
			{
				jsmemberField = jsmemberField.nextOverload;
				num++;
			}
			ConstructorInfo[] array = new ConstructorInfo[num];
			jsmemberField = this;
			num = 0;
			while (jsmemberField != null)
			{
				FunctionObject functionObject = (FunctionObject)jsmemberField.value;
				functionObject.isConstructor = true;
				functionObject.proto = proto;
				array[num++] = new JSConstructor(functionObject);
				jsmemberField = jsmemberField.nextOverload;
			}
			return array;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x0003886C File Offset: 0x0003786C
		public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo locale)
		{
			if (obj is StackFrame)
			{
				this.SetValue(((StackFrame)obj).closureInstance, value, invokeAttr, binder, locale, (StackFrame)obj);
				return;
			}
			if (obj is ScriptObject)
			{
				this.SetValue(obj, value, invokeAttr, binder, locale, (ScriptObject)obj);
				return;
			}
			this.SetValue(obj, value, invokeAttr, binder, locale, null);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x000388CC File Offset: 0x000378CC
		private void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo locale, ScriptObject scope)
		{
			if (base.IsStatic || base.IsLiteral)
			{
				if ((base.IsLiteral || base.IsInitOnly) && !(this.value is Missing))
				{
					throw new JScriptException(JSError.AssignmentToReadOnly);
				}
			}
			else
			{
				if (this.obj != obj)
				{
					if (obj is JSObject)
					{
						FieldInfo field = ((JSObject)obj).GetField(this.Name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
						if (field != null)
						{
							field.SetValue(obj, value, invokeAttr, binder, locale);
							return;
						}
					}
					throw new TargetException();
				}
				if (!base.IsPublic && (scope == null || !this.IsAccessibleFrom(scope)))
				{
					if (((JSObject)this.obj).noExpando)
					{
						throw new JScriptException(JSError.NotAccessible, new Context(new DocumentContext("", null), this.Name));
					}
					this.expandoValue = value;
					return;
				}
			}
			if (this.type != null)
			{
				this.value = Convert.Coerce(value, this.type);
				return;
			}
			this.value = value;
		}

		// Token: 0x04000454 RID: 1108
		private object expandoValue;

		// Token: 0x04000455 RID: 1109
		internal JSMemberField nextOverload;
	}
}
