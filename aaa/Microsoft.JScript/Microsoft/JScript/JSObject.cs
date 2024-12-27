using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices.Expando;

namespace Microsoft.JScript
{
	// Token: 0x02000008 RID: 8
	public class JSObject : ScriptObject, IEnumerable, IExpando, IReflect
	{
		// Token: 0x06000044 RID: 68 RVA: 0x0000348A File Offset: 0x0000248A
		public JSObject()
			: this(null, false)
		{
			this.noExpando = false;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x0000349B File Offset: 0x0000249B
		internal JSObject(ScriptObject parent)
			: this(parent, true)
		{
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000034A8 File Offset: 0x000024A8
		internal JSObject(ScriptObject parent, bool checkSubType)
			: base(parent)
		{
			this.memberCache = null;
			this.isASubClass = false;
			this.subClassIR = null;
			if (checkSubType)
			{
				Type type = Globals.TypeRefs.ToReferenceContext(base.GetType());
				if (type != Typeob.JSObject)
				{
					this.isASubClass = true;
					this.subClassIR = TypeReflector.GetTypeReflectorFor(type);
				}
			}
			this.noExpando = this.isASubClass;
			this.name_table = null;
			this.field_table = null;
			this.outer_class_instance = null;
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003524 File Offset: 0x00002524
		internal JSObject(ScriptObject parent, Type subType)
			: base(parent)
		{
			this.memberCache = null;
			this.isASubClass = false;
			this.subClassIR = null;
			subType = Globals.TypeRefs.ToReferenceContext(subType);
			if (subType != Typeob.JSObject)
			{
				this.isASubClass = true;
				this.subClassIR = TypeReflector.GetTypeReflectorFor(subType);
			}
			this.noExpando = this.isASubClass;
			this.name_table = null;
			this.field_table = null;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003590 File Offset: 0x00002590
		public FieldInfo AddField(string name)
		{
			if (this.noExpando)
			{
				return null;
			}
			FieldInfo fieldInfo = (FieldInfo)this.NameTable[name];
			if (fieldInfo == null)
			{
				fieldInfo = new JSExpandoField(name);
				this.name_table[name] = fieldInfo;
				this.field_table.Add(fieldInfo);
			}
			return fieldInfo;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x000035DE File Offset: 0x000025DE
		MethodInfo IExpando.AddMethod(string name, Delegate method)
		{
			return null;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x000035E1 File Offset: 0x000025E1
		PropertyInfo IExpando.AddProperty(string name)
		{
			return null;
		}

		// Token: 0x0600004B RID: 75 RVA: 0x000035E4 File Offset: 0x000025E4
		internal override bool DeleteMember(string name)
		{
			FieldInfo fieldInfo = (FieldInfo)this.NameTable[name];
			if (fieldInfo == null)
			{
				return this.parent != null && LateBinding.DeleteMember(this.parent, name);
			}
			if (fieldInfo is JSExpandoField)
			{
				fieldInfo.SetValue(this, Missing.Value);
				this.name_table.Remove(name);
				this.field_table.Remove(fieldInfo);
				return true;
			}
			if (fieldInfo is JSPrototypeField)
			{
				fieldInfo.SetValue(this, Missing.Value);
				return true;
			}
			return false;
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00003662 File Offset: 0x00002662
		internal virtual string GetClassName()
		{
			return "Object";
		}

		// Token: 0x0600004D RID: 77 RVA: 0x0000366C File Offset: 0x0000266C
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object GetDefaultValue(PreferredType preferred_type)
		{
			if (preferred_type == PreferredType.String)
			{
				ScriptFunction scriptFunction = this.GetMemberValue("toString") as ScriptFunction;
				if (scriptFunction != null)
				{
					object obj = scriptFunction.Call(new object[0], this);
					if (obj == null)
					{
						return obj;
					}
					IConvertible iconvertible = Convert.GetIConvertible(obj);
					if (iconvertible != null && iconvertible.GetTypeCode() != TypeCode.Object)
					{
						return obj;
					}
				}
				ScriptFunction scriptFunction2 = this.GetMemberValue("valueOf") as ScriptFunction;
				if (scriptFunction2 != null)
				{
					object obj2 = scriptFunction2.Call(new object[0], this);
					if (obj2 == null)
					{
						return obj2;
					}
					IConvertible iconvertible2 = Convert.GetIConvertible(obj2);
					if (iconvertible2 != null && iconvertible2.GetTypeCode() != TypeCode.Object)
					{
						return obj2;
					}
				}
			}
			else if (preferred_type == PreferredType.LocaleString)
			{
				ScriptFunction scriptFunction3 = this.GetMemberValue("toLocaleString") as ScriptFunction;
				if (scriptFunction3 != null)
				{
					return scriptFunction3.Call(new object[0], this);
				}
			}
			else
			{
				if (preferred_type == PreferredType.Either && this is DateObject)
				{
					return this.GetDefaultValue(PreferredType.String);
				}
				ScriptFunction scriptFunction4 = this.GetMemberValue("valueOf") as ScriptFunction;
				if (scriptFunction4 != null)
				{
					object obj3 = scriptFunction4.Call(new object[0], this);
					if (obj3 == null)
					{
						return obj3;
					}
					IConvertible iconvertible3 = Convert.GetIConvertible(obj3);
					if (iconvertible3 != null && iconvertible3.GetTypeCode() != TypeCode.Object)
					{
						return obj3;
					}
				}
				ScriptFunction scriptFunction5 = this.GetMemberValue("toString") as ScriptFunction;
				if (scriptFunction5 != null)
				{
					object obj4 = scriptFunction5.Call(new object[0], this);
					if (obj4 == null)
					{
						return obj4;
					}
					IConvertible iconvertible4 = Convert.GetIConvertible(obj4);
					if (iconvertible4 != null && iconvertible4.GetTypeCode() != TypeCode.Object)
					{
						return obj4;
					}
				}
			}
			return this;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x000037D7 File Offset: 0x000027D7
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ForIn.JScriptGetEnumerator(this);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x000037E0 File Offset: 0x000027E0
		private static bool IsHiddenMember(MemberInfo mem)
		{
			Type declaringType = mem.DeclaringType;
			return declaringType == Typeob.JSObject || declaringType == Typeob.ScriptObject || (declaringType == Typeob.ArrayWrapper && mem.Name != "length");
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00003824 File Offset: 0x00002824
		private MemberInfo[] GetLocalMember(string name, BindingFlags bindingAttr, bool wrapMembers)
		{
			MemberInfo[] array = null;
			FieldInfo fieldInfo = ((this.name_table == null) ? null : ((FieldInfo)this.name_table[name]));
			if (fieldInfo == null && this.isASubClass)
			{
				if (this.memberCache != null)
				{
					array = (MemberInfo[])this.memberCache[name];
					if (array != null)
					{
						return array;
					}
				}
				bindingAttr &= ~BindingFlags.NonPublic;
				array = this.subClassIR.GetMember(name, bindingAttr);
				if (array.Length == 0)
				{
					array = this.subClassIR.GetMember(name, (bindingAttr & ~BindingFlags.Instance) | BindingFlags.Static);
				}
				int num = array.Length;
				if (num > 0)
				{
					int num2 = 0;
					foreach (MemberInfo memberInfo in array)
					{
						if (JSObject.IsHiddenMember(memberInfo))
						{
							num2++;
						}
					}
					if (num2 > 0 && (num != 1 || !(this is ObjectPrototype) || !(name == "ToString")))
					{
						MemberInfo[] array3 = new MemberInfo[num - num2];
						int num3 = 0;
						foreach (MemberInfo memberInfo2 in array)
						{
							if (!JSObject.IsHiddenMember(memberInfo2))
							{
								array3[num3++] = memberInfo2;
							}
						}
						array = array3;
					}
				}
				if ((array == null || array.Length == 0) && (bindingAttr & BindingFlags.Public) != BindingFlags.Default && (bindingAttr & BindingFlags.Instance) != BindingFlags.Default)
				{
					BindingFlags bindingFlags = (bindingAttr & BindingFlags.IgnoreCase) | BindingFlags.Public | BindingFlags.Instance;
					if (this is StringObject)
					{
						array = TypeReflector.GetTypeReflectorFor(Typeob.String).GetMember(name, bindingFlags);
					}
					else if (this is NumberObject)
					{
						array = TypeReflector.GetTypeReflectorFor(((NumberObject)this).baseType).GetMember(name, bindingFlags);
					}
					else if (this is BooleanObject)
					{
						array = TypeReflector.GetTypeReflectorFor(Typeob.Boolean).GetMember(name, bindingFlags);
					}
					else if (this is StringConstructor)
					{
						array = TypeReflector.GetTypeReflectorFor(Typeob.String).GetMember(name, (bindingFlags | BindingFlags.Static) & ~BindingFlags.Instance);
					}
					else if (this is BooleanConstructor)
					{
						array = TypeReflector.GetTypeReflectorFor(Typeob.Boolean).GetMember(name, (bindingFlags | BindingFlags.Static) & ~BindingFlags.Instance);
					}
					else if (this is ArrayWrapper)
					{
						array = TypeReflector.GetTypeReflectorFor(Typeob.Array).GetMember(name, bindingFlags);
					}
				}
				if (array != null && array.Length > 0)
				{
					if (wrapMembers)
					{
						array = ScriptObject.WrapMembers(array, this);
					}
					if (this.memberCache == null)
					{
						this.memberCache = new SimpleHashtable(32U);
					}
					this.memberCache[name] = array;
					return array;
				}
			}
			if ((bindingAttr & BindingFlags.IgnoreCase) != BindingFlags.Default && (array == null || array.Length == 0))
			{
				array = null;
				IDictionaryEnumerator enumerator = this.name_table.GetEnumerator();
				while (enumerator.MoveNext())
				{
					if (string.Compare(enumerator.Key.ToString(), name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						fieldInfo = (FieldInfo)enumerator.Value;
						break;
					}
				}
			}
			if (fieldInfo != null)
			{
				return new MemberInfo[] { fieldInfo };
			}
			if (array == null)
			{
				array = new MemberInfo[0];
			}
			return array;
		}

		// Token: 0x06000051 RID: 81 RVA: 0x00003AC8 File Offset: 0x00002AC8
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			return this.GetMember(name, bindingAttr, false);
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00003AD4 File Offset: 0x00002AD4
		private MemberInfo[] GetMember(string name, BindingFlags bindingAttr, bool wrapMembers)
		{
			MemberInfo[] array = this.GetLocalMember(name, bindingAttr, wrapMembers);
			if (array.Length > 0)
			{
				return array;
			}
			if (this.parent == null)
			{
				return new MemberInfo[0];
			}
			if (this.parent is JSObject)
			{
				array = ((JSObject)this.parent).GetMember(name, bindingAttr, true);
				wrapMembers = false;
			}
			else
			{
				array = this.parent.GetMember(name, bindingAttr);
			}
			MemberInfo[] array2 = array;
			int i = 0;
			while (i < array2.Length)
			{
				MemberInfo memberInfo = array2[i];
				MemberInfo[] array3;
				if (memberInfo.MemberType == MemberTypes.Field)
				{
					FieldInfo fieldInfo = (FieldInfo)memberInfo;
					JSMemberField jsmemberField = memberInfo as JSMemberField;
					if (jsmemberField != null)
					{
						if (!jsmemberField.IsStatic)
						{
							JSGlobalField jsglobalField = new JSGlobalField(this, name, jsmemberField.value, FieldAttributes.Public);
							this.NameTable[name] = jsglobalField;
							this.field_table.Add(jsglobalField);
							fieldInfo = jsmemberField;
						}
					}
					else
					{
						fieldInfo = new JSPrototypeField(this.parent, (FieldInfo)memberInfo);
						if (!this.noExpando)
						{
							this.NameTable[name] = fieldInfo;
							this.field_table.Add(fieldInfo);
						}
					}
					array3 = new MemberInfo[] { fieldInfo };
				}
				else
				{
					if (this.noExpando || memberInfo.MemberType != MemberTypes.Method)
					{
						i++;
						continue;
					}
					FieldInfo fieldInfo2 = new JSPrototypeField(this.parent, new JSGlobalField(this, name, LateBinding.GetMemberValue(this.parent, name, null, array), FieldAttributes.FamANDAssem | FieldAttributes.Family | FieldAttributes.InitOnly));
					this.NameTable[name] = fieldInfo2;
					this.field_table.Add(fieldInfo2);
					array3 = new MemberInfo[] { fieldInfo2 };
				}
				return array3;
			}
			if (wrapMembers)
			{
				return ScriptObject.WrapMembers(array, this.parent);
			}
			return array;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00003C74 File Offset: 0x00002C74
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			MemberInfoList memberInfoList = new MemberInfoList();
			SimpleHashtable simpleHashtable = new SimpleHashtable(32U);
			if (!this.noExpando && this.field_table != null)
			{
				foreach (object obj in this.field_table)
				{
					FieldInfo fieldInfo = (FieldInfo)obj;
					memberInfoList.Add(fieldInfo);
					simpleHashtable[fieldInfo.Name] = fieldInfo;
				}
			}
			if (this.isASubClass)
			{
				MemberInfo[] members = base.GetType().GetMembers(bindingAttr & ~BindingFlags.NonPublic);
				int i = 0;
				int num = members.Length;
				while (i < num)
				{
					MemberInfo memberInfo = members[i];
					if (!memberInfo.DeclaringType.IsAssignableFrom(Typeob.JSObject) && simpleHashtable[memberInfo.Name] == null)
					{
						MethodInfo methodInfo = memberInfo as MethodInfo;
						if (methodInfo == null || !methodInfo.IsSpecialName)
						{
							memberInfoList.Add(memberInfo);
							simpleHashtable[memberInfo.Name] = memberInfo;
						}
					}
					i++;
				}
			}
			if (this.parent != null)
			{
				SimpleHashtable simpleHashtable2 = this.parent.wrappedMemberCache;
				if (simpleHashtable2 == null)
				{
					simpleHashtable2 = (this.parent.wrappedMemberCache = new SimpleHashtable(8U));
				}
				MemberInfo[] array = ScriptObject.WrapMembers(((IReflect)this.parent).GetMembers(bindingAttr & ~BindingFlags.NonPublic), this.parent, simpleHashtable2);
				int j = 0;
				int num2 = array.Length;
				while (j < num2)
				{
					MemberInfo memberInfo2 = array[j];
					if (simpleHashtable[memberInfo2.Name] == null)
					{
						memberInfoList.Add(memberInfo2);
					}
					j++;
				}
			}
			return memberInfoList.ToArray();
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00003DE8 File Offset: 0x00002DE8
		internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
		{
			if (this.field_table == null)
			{
				this.field_table = new ArrayList();
			}
			enums.Add(new ListEnumerator(this.field_table));
			objects.Add(this);
			if (this.parent != null)
			{
				this.parent.GetPropertyEnumerator(enums, objects);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00003E38 File Offset: 0x00002E38
		internal override object GetValueAtIndex(uint index)
		{
			string text = Convert.ToString(index, CultureInfo.InvariantCulture);
			FieldInfo fieldInfo = (FieldInfo)this.NameTable[text];
			if (fieldInfo != null)
			{
				return fieldInfo.GetValue(this);
			}
			object obj;
			if (this.parent != null)
			{
				obj = this.parent.GetMemberValue(text);
			}
			else
			{
				obj = Missing.Value;
			}
			if (this is StringObject && obj == Missing.Value)
			{
				string value = ((StringObject)this).value;
				if ((ulong)index < (ulong)((long)value.Length))
				{
					return value[(int)index];
				}
			}
			return obj;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00003EC4 File Offset: 0x00002EC4
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object GetMemberValue(string name)
		{
			FieldInfo fieldInfo = (FieldInfo)this.NameTable[name];
			if (fieldInfo == null && this.isASubClass)
			{
				fieldInfo = this.subClassIR.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
				if (fieldInfo != null)
				{
					if (fieldInfo.DeclaringType == Typeob.ScriptObject)
					{
						return Missing.Value;
					}
				}
				else
				{
					PropertyInfo property = this.subClassIR.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
					if (property != null && !property.DeclaringType.IsAssignableFrom(Typeob.JSObject))
					{
						return JSProperty.GetGetMethod(property, false).Invoke(this, BindingFlags.SuppressChangeType, null, null, null);
					}
					try
					{
						MethodInfo method = this.subClassIR.GetMethod(name, BindingFlags.Static | BindingFlags.Public);
						if (method != null)
						{
							Type declaringType = method.DeclaringType;
							if (declaringType != Typeob.JSObject && declaringType != Typeob.ScriptObject && declaringType != Typeob.Object)
							{
								return new BuiltinFunction(this, method);
							}
						}
					}
					catch (AmbiguousMatchException)
					{
					}
				}
			}
			if (fieldInfo != null)
			{
				return fieldInfo.GetValue(this);
			}
			if (this.parent != null)
			{
				return this.parent.GetMemberValue(name);
			}
			return Missing.Value;
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000057 RID: 87 RVA: 0x00003FD0 File Offset: 0x00002FD0
		internal SimpleHashtable NameTable
		{
			get
			{
				SimpleHashtable simpleHashtable = this.name_table;
				if (simpleHashtable == null)
				{
					simpleHashtable = (this.name_table = new SimpleHashtable(16U));
					this.field_table = new ArrayList();
				}
				return simpleHashtable;
			}
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004002 File Offset: 0x00003002
		void IExpando.RemoveMember(MemberInfo m)
		{
			this.DeleteMember(m.Name);
		}

		// Token: 0x06000059 RID: 89 RVA: 0x00004011 File Offset: 0x00003011
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override void SetMemberValue(string name, object value)
		{
			this.SetMemberValue2(name, value);
		}

		// Token: 0x0600005A RID: 90 RVA: 0x0000401C File Offset: 0x0000301C
		public void SetMemberValue2(string name, object value)
		{
			FieldInfo fieldInfo = (FieldInfo)this.NameTable[name];
			if (fieldInfo == null && this.isASubClass)
			{
				fieldInfo = base.GetType().GetField(name);
			}
			if (fieldInfo == null)
			{
				if (this.noExpando)
				{
					return;
				}
				fieldInfo = new JSExpandoField(name);
				this.name_table[name] = fieldInfo;
				this.field_table.Add(fieldInfo);
			}
			if (!fieldInfo.IsInitOnly && !fieldInfo.IsLiteral)
			{
				fieldInfo.SetValue(this, value);
			}
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00004098 File Offset: 0x00003098
		internal override void SetValueAtIndex(uint index, object value)
		{
			this.SetMemberValue(Convert.ToString(index, CultureInfo.InvariantCulture), value);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x000040AC File Offset: 0x000030AC
		internal virtual void SwapValues(uint left, uint right)
		{
			string text = Convert.ToString(left, CultureInfo.InvariantCulture);
			string text2 = Convert.ToString(right, CultureInfo.InvariantCulture);
			FieldInfo fieldInfo = (FieldInfo)this.NameTable[text];
			FieldInfo fieldInfo2 = (FieldInfo)this.name_table[text2];
			if (fieldInfo == null)
			{
				if (fieldInfo2 == null)
				{
					return;
				}
				this.name_table[text] = fieldInfo2;
				this.name_table.Remove(text2);
				return;
			}
			else
			{
				if (fieldInfo2 == null)
				{
					this.name_table[text2] = fieldInfo;
					this.name_table.Remove(text);
					return;
				}
				this.name_table[text] = fieldInfo2;
				this.name_table[text2] = fieldInfo;
				return;
			}
		}

		// Token: 0x0600005D RID: 93 RVA: 0x0000414D File Offset: 0x0000314D
		public override string ToString()
		{
			return Convert.ToString(this);
		}

		// Token: 0x04000017 RID: 23
		private bool isASubClass;

		// Token: 0x04000018 RID: 24
		private IReflect subClassIR;

		// Token: 0x04000019 RID: 25
		private SimpleHashtable memberCache;

		// Token: 0x0400001A RID: 26
		internal bool noExpando;

		// Token: 0x0400001B RID: 27
		internal SimpleHashtable name_table;

		// Token: 0x0400001C RID: 28
		protected ArrayList field_table;

		// Token: 0x0400001D RID: 29
		internal JSObject outer_class_instance;
	}
}
