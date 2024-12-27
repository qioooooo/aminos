using System;
using System.Collections;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x02000086 RID: 134
	internal sealed class FunctionScope : ActivationObject
	{
		// Token: 0x06000602 RID: 1538 RVA: 0x0002C2F0 File Offset: 0x0002B2F0
		internal FunctionScope(ScriptObject parent)
			: this(parent, false)
		{
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x0002C2FC File Offset: 0x0002B2FC
		internal FunctionScope(ScriptObject parent, bool isMethod)
			: base(parent)
		{
			this.isKnownAtCompileTime = true;
			this.isMethod = isMethod;
			this.mustSaveStackLocals = false;
			if (parent != null && parent is ActivationObject)
			{
				this.fast = ((ActivationObject)parent).fast;
			}
			else
			{
				this.fast = false;
			}
			this.returnVar = null;
			this.owner = null;
			this.isStatic = false;
			this.nested_functions = null;
			this.fields_for_nested_functions = null;
			if (parent is FunctionScope)
			{
				this.ProvidesOuterScopeLocals = new SimpleHashtable(16U);
			}
			else
			{
				this.ProvidesOuterScopeLocals = null;
			}
			this.closuresMightEscape = false;
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0002C394 File Offset: 0x0002B394
		internal JSVariableField AddNewField(string name, FieldAttributes attributeFlags, FunctionObject func)
		{
			if (this.nested_functions == null)
			{
				this.nested_functions = new ArrayList();
				this.fields_for_nested_functions = new ArrayList();
			}
			this.nested_functions.Add(func);
			JSVariableField jsvariableField = this.AddNewField(name, func, attributeFlags);
			this.fields_for_nested_functions.Add(jsvariableField);
			return jsvariableField;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x0002C3E4 File Offset: 0x0002B3E4
		protected override JSVariableField CreateField(string name, FieldAttributes attributeFlags, object value)
		{
			if ((attributeFlags & FieldAttributes.Static) != FieldAttributes.PrivateScope)
			{
				return new JSGlobalField(this, name, value, attributeFlags);
			}
			return new JSLocalField(name, this, this.field_table.Count, value);
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0002C409 File Offset: 0x0002B409
		internal void AddOuterScopeField(string name, JSLocalField field)
		{
			this.name_table[name] = field;
			this.field_table.Add(field);
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x0002C428 File Offset: 0x0002B428
		internal void AddReturnValueField()
		{
			if (this.name_table["return value"] != null)
			{
				return;
			}
			this.returnVar = new JSLocalField("return value", this, this.field_table.Count, Missing.Value);
			this.name_table["return value"] = this.returnVar;
			this.field_table.Add(this.returnVar);
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x0002C494 File Offset: 0x0002B494
		internal void CloseNestedFunctions(StackFrame sf)
		{
			if (this.nested_functions == null)
			{
				return;
			}
			IEnumerator enumerator = this.nested_functions.GetEnumerator();
			IEnumerator enumerator2 = this.fields_for_nested_functions.GetEnumerator();
			while (enumerator.MoveNext() && enumerator2.MoveNext())
			{
				FieldInfo fieldInfo = (FieldInfo)enumerator2.Current;
				FunctionObject functionObject = (FunctionObject)enumerator.Current;
				functionObject.enclosing_scope = sf;
				fieldInfo.SetValue(sf, new Closure(functionObject));
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000609 RID: 1545 RVA: 0x0002C500 File Offset: 0x0002B500
		// (set) Token: 0x0600060A RID: 1546 RVA: 0x0002C550 File Offset: 0x0002B550
		internal BitArray DefinedFlags
		{
			get
			{
				int count = this.field_table.Count;
				BitArray bitArray = new BitArray(count);
				for (int i = 0; i < count; i++)
				{
					JSLocalField jslocalField = (JSLocalField)this.field_table[i];
					if (jslocalField.isDefined)
					{
						bitArray[i] = true;
					}
				}
				return bitArray;
			}
			set
			{
				int count = value.Count;
				for (int i = 0; i < count; i++)
				{
					JSLocalField jslocalField = (JSLocalField)this.field_table[i];
					jslocalField.isDefined = value[i];
				}
			}
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x0002C590 File Offset: 0x0002B590
		internal JSLocalField[] GetLocalFields()
		{
			int count = this.field_table.Count;
			JSLocalField[] array = new JSLocalField[this.field_table.Count];
			for (int i = 0; i < count; i++)
			{
				array[i] = (JSLocalField)this.field_table[i];
			}
			return array;
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x0002C5DC File Offset: 0x0002B5DC
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo != null)
			{
				return new MemberInfo[] { fieldInfo };
			}
			bool flag = false;
			ScriptObject scriptObject = this.parent;
			while (scriptObject is FunctionScope)
			{
				FunctionScope functionScope = (FunctionScope)scriptObject;
				flag = functionScope.isMethod && !functionScope.isStatic;
				JSLocalField jslocalField = (JSLocalField)functionScope.name_table[name];
				if (jslocalField == null)
				{
					scriptObject = scriptObject.GetParent();
				}
				else
				{
					if (jslocalField.IsLiteral && !(jslocalField.value is FunctionObject))
					{
						return new MemberInfo[] { jslocalField };
					}
					JSLocalField jslocalField2 = new JSLocalField(jslocalField.Name, this, this.field_table.Count, Missing.Value);
					jslocalField2.outerField = jslocalField;
					jslocalField2.debugOn = jslocalField.debugOn;
					if (!jslocalField2.debugOn && this.owner.funcContext.document.debugOn && functionScope.owner.funcContext.document.debugOn)
					{
						jslocalField2.debugOn = Array.IndexOf<string>(functionScope.owner.formal_parameters, jslocalField.Name) >= 0;
					}
					jslocalField2.isDefined = jslocalField.isDefined;
					jslocalField2.debuggerName = "outer." + jslocalField2.Name;
					if (jslocalField.IsLiteral)
					{
						jslocalField2.attributeFlags |= FieldAttributes.Literal;
						jslocalField2.value = jslocalField.value;
					}
					this.AddOuterScopeField(name, jslocalField2);
					if (this.ProvidesOuterScopeLocals[scriptObject] == null)
					{
						this.ProvidesOuterScopeLocals[scriptObject] = scriptObject;
					}
					((FunctionScope)scriptObject).mustSaveStackLocals = true;
					return new MemberInfo[] { jslocalField2 };
				}
			}
			if (scriptObject is ClassScope && flag)
			{
				MemberInfo[] member = scriptObject.GetMember(name, bindingAttr & ~BindingFlags.DeclaredOnly);
				int num = member.Length;
				bool flag2 = false;
				for (int i = 0; i < num; i++)
				{
					MemberInfo memberInfo = member[i];
					MemberTypes memberType = memberInfo.MemberType;
					if (memberType != MemberTypes.Field)
					{
						if (memberType != MemberTypes.Method)
						{
							if (memberType == MemberTypes.Property)
							{
								PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
								MethodInfo methodInfo = JSProperty.GetGetMethod(propertyInfo, (bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default);
								MethodInfo methodInfo2 = JSProperty.GetSetMethod(propertyInfo, (bindingAttr & BindingFlags.NonPublic) != BindingFlags.Default);
								bool flag3 = false;
								if (methodInfo != null && !methodInfo.IsStatic)
								{
									flag3 = true;
									methodInfo = new JSClosureMethod(methodInfo);
								}
								if (methodInfo2 != null && !methodInfo2.IsStatic)
								{
									flag3 = true;
									methodInfo2 = new JSClosureMethod(methodInfo2);
								}
								if (flag3)
								{
									member[i] = new JSClosureProperty(propertyInfo, methodInfo, methodInfo2);
									flag2 = true;
								}
							}
						}
						else
						{
							MethodInfo methodInfo3 = (MethodInfo)memberInfo;
							if (!methodInfo3.IsStatic)
							{
								member[i] = new JSClosureMethod(methodInfo3);
								flag2 = true;
							}
						}
					}
					else
					{
						fieldInfo = (FieldInfo)memberInfo;
						if (fieldInfo.IsLiteral)
						{
							JSMemberField jsmemberField = fieldInfo as JSMemberField;
							if (jsmemberField != null && jsmemberField.value is ClassScope && !((ClassScope)jsmemberField.value).owner.IsStatic)
							{
								flag2 = true;
							}
						}
						if (!fieldInfo.IsStatic && !fieldInfo.IsLiteral)
						{
							member[i] = new JSClosureField(fieldInfo);
							flag2 = true;
						}
					}
				}
				if (flag2)
				{
					this.GiveOuterFunctionsTheBadNews();
				}
				if (num > 0)
				{
					return member;
				}
			}
			if ((bindingAttr & BindingFlags.DeclaredOnly) != BindingFlags.Default)
			{
				return new MemberInfo[0];
			}
			return scriptObject.GetMember(name, bindingAttr);
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x0002C948 File Offset: 0x0002B948
		internal override string GetName()
		{
			string text = null;
			if (this.parent != null)
			{
				text = ((ActivationObject)this.parent).GetName();
			}
			if (text != null)
			{
				return text + "." + this.owner.name;
			}
			return this.owner.name;
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x0002C995 File Offset: 0x0002B995
		internal int GetNextSlotNumber()
		{
			return this.field_table.Count;
		}

		// Token: 0x0600060F RID: 1551 RVA: 0x0002C9A4 File Offset: 0x0002B9A4
		internal JSLocalField GetOuterLocalField(string name)
		{
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo != null && fieldInfo is JSLocalField && ((JSLocalField)fieldInfo).outerField != null)
			{
				return (JSLocalField)fieldInfo;
			}
			return null;
		}

		// Token: 0x06000610 RID: 1552 RVA: 0x0002C9E4 File Offset: 0x0002B9E4
		private void GiveOuterFunctionsTheBadNews()
		{
			FunctionScope functionScope = (FunctionScope)this.parent;
			functionScope.mustSaveStackLocals = true;
			while (!functionScope.isMethod)
			{
				functionScope = (FunctionScope)functionScope.GetParent();
				functionScope.mustSaveStackLocals = true;
			}
		}

		// Token: 0x06000611 RID: 1553 RVA: 0x0002CA24 File Offset: 0x0002BA24
		internal void HandleUnitializedVariables()
		{
			int i = 0;
			int count = this.field_table.Count;
			while (i < count)
			{
				JSLocalField jslocalField = (JSLocalField)this.field_table[i];
				if (jslocalField.isUsedBeforeDefinition)
				{
					jslocalField.SetInferredType(Typeob.Object, null);
				}
				i++;
			}
		}

		// Token: 0x06000612 RID: 1554 RVA: 0x0002CA70 File Offset: 0x0002BA70
		internal override void SetMemberValue(string name, object value)
		{
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(this, value);
				return;
			}
			this.parent.SetMemberValue(name, value);
		}

		// Token: 0x06000613 RID: 1555 RVA: 0x0002CAA8 File Offset: 0x0002BAA8
		internal void SetMemberValue(string name, object value, StackFrame sf)
		{
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo != null)
			{
				fieldInfo.SetValue(sf, value);
				return;
			}
			this.parent.SetMemberValue(name, value);
		}

		// Token: 0x040002B5 RID: 693
		internal bool isMethod;

		// Token: 0x040002B6 RID: 694
		internal bool isStatic;

		// Token: 0x040002B7 RID: 695
		internal bool mustSaveStackLocals;

		// Token: 0x040002B8 RID: 696
		internal JSLocalField returnVar;

		// Token: 0x040002B9 RID: 697
		internal FunctionObject owner;

		// Token: 0x040002BA RID: 698
		internal ArrayList nested_functions;

		// Token: 0x040002BB RID: 699
		private ArrayList fields_for_nested_functions;

		// Token: 0x040002BC RID: 700
		internal SimpleHashtable ProvidesOuterScopeLocals;

		// Token: 0x040002BD RID: 701
		internal bool closuresMightEscape;
	}
}
