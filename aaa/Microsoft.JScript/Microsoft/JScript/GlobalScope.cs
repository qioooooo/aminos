using System;
using System.Collections;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Expando;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x0200008C RID: 140
	[ComVisible(true)]
	public class GlobalScope : ActivationObject, IExpando, IReflect
	{
		// Token: 0x0600066F RID: 1647 RVA: 0x0002DB90 File Offset: 0x0002CB90
		public GlobalScope(GlobalScope parent, VsaEngine engine)
			: this(parent, engine, parent != null)
		{
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x0002DBA4 File Offset: 0x0002CBA4
		internal GlobalScope(GlobalScope parent, VsaEngine engine, bool isComponentScope)
			: base(parent)
		{
			this.componentScopes = null;
			this.recursive = false;
			this.isComponentScope = isComponentScope;
			if (parent == null)
			{
				this.globalObject = engine.Globals.globalObject;
				this.globalObjectTR = TypeReflector.GetTypeReflectorFor(Globals.TypeRefs.ToReferenceContext(this.globalObject.GetType()));
				this.fast = !(this.globalObject is LenientGlobalObject);
			}
			else
			{
				this.globalObject = null;
				this.globalObjectTR = null;
				this.fast = parent.fast;
				if (isComponentScope)
				{
					((GlobalScope)this.parent).AddComponentScope(this);
				}
			}
			this.engine = engine;
			this.isKnownAtCompileTime = this.fast;
			this.evilScript = true;
			this.thisObject = this;
			this.typeReflector = TypeReflector.GetTypeReflectorFor(Globals.TypeRefs.ToReferenceContext(base.GetType()));
			if (isComponentScope)
			{
				engine.Scopes.Add(this);
			}
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x0002DC94 File Offset: 0x0002CC94
		internal void AddComponentScope(GlobalScope component)
		{
			if (this.componentScopes == null)
			{
				this.componentScopes = new ArrayList();
			}
			this.componentScopes.Add(component);
			component.thisObject = this.thisObject;
		}

		// Token: 0x06000672 RID: 1650 RVA: 0x0002DCC4 File Offset: 0x0002CCC4
		public FieldInfo AddField(string name)
		{
			if (this.fast)
			{
				return null;
			}
			if (this.isComponentScope)
			{
				return ((GlobalScope)this.parent).AddField(name);
			}
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo == null)
			{
				fieldInfo = new JSExpandoField(name);
				this.name_table[name] = fieldInfo;
				this.field_table.Add(fieldInfo);
			}
			return fieldInfo;
		}

		// Token: 0x06000673 RID: 1651 RVA: 0x0002DD2C File Offset: 0x0002CD2C
		MethodInfo IExpando.AddMethod(string name, Delegate method)
		{
			return null;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0002DD2F File Offset: 0x0002CD2F
		internal override JSVariableField AddNewField(string name, object value, FieldAttributes attributeFlags)
		{
			if (!this.isComponentScope)
			{
				return base.AddNewField(name, value, attributeFlags);
			}
			return ((GlobalScope)this.parent).AddNewField(name, value, attributeFlags);
		}

		// Token: 0x06000675 RID: 1653 RVA: 0x0002DD56 File Offset: 0x0002CD56
		PropertyInfo IExpando.AddProperty(string name)
		{
			return null;
		}

		// Token: 0x06000676 RID: 1654 RVA: 0x0002DD5C File Offset: 0x0002CD5C
		internal override bool DeleteMember(string name)
		{
			if (this.isComponentScope)
			{
				return this.parent.DeleteMember(name);
			}
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo == null)
			{
				return false;
			}
			if (fieldInfo is JSExpandoField)
			{
				fieldInfo.SetValue(this, Missing.Value);
				this.name_table.Remove(name);
				this.field_table.Remove(fieldInfo);
				return true;
			}
			return false;
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0002DDC4 File Offset: 0x0002CDC4
		public override object GetDefaultThisObject()
		{
			return this;
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0002DDC7 File Offset: 0x0002CDC7
		internal override object GetDefaultValue(PreferredType preferred_type)
		{
			if (preferred_type == PreferredType.String || preferred_type == PreferredType.LocaleString)
			{
				return "";
			}
			return double.NaN;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0002DDE5 File Offset: 0x0002CDE5
		public override FieldInfo GetField(string name, int lexLevel)
		{
			return base.GetField(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600067A RID: 1658 RVA: 0x0002DDF0 File Offset: 0x0002CDF0
		internal JSField[] GetFields()
		{
			int count = this.field_table.Count;
			JSField[] array = new JSField[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = (JSField)this.field_table[i];
			}
			return array;
		}

		// Token: 0x0600067B RID: 1659 RVA: 0x0002DE31 File Offset: 0x0002CE31
		public override FieldInfo[] GetFields(BindingFlags bindingAttr)
		{
			return base.GetFields(bindingAttr | BindingFlags.DeclaredOnly);
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x0002DE3C File Offset: 0x0002CE3C
		public override GlobalScope GetGlobalScope()
		{
			return this;
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x0002DE3F File Offset: 0x0002CE3F
		public override FieldInfo GetLocalField(string name)
		{
			return base.GetField(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x0002DE4A File Offset: 0x0002CE4A
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			return this.GetMember(name, bindingAttr, false);
		}

		// Token: 0x0600067F RID: 1663 RVA: 0x0002DE58 File Offset: 0x0002CE58
		private MemberInfo[] GetMember(string name, BindingFlags bindingAttr, bool calledFromParent)
		{
			if (this.recursive)
			{
				return new MemberInfo[0];
			}
			MemberInfo[] array = null;
			if (!this.isComponentScope)
			{
				MemberInfo[] member = base.GetMember(name, bindingAttr | BindingFlags.DeclaredOnly);
				if (member.Length > 0)
				{
					return member;
				}
				if (this.componentScopes != null)
				{
					int i = 0;
					int count = this.componentScopes.Count;
					while (i < count)
					{
						GlobalScope globalScope = (GlobalScope)this.componentScopes[i];
						array = globalScope.GetMember(name, bindingAttr | BindingFlags.DeclaredOnly, true);
						if (array.Length > 0)
						{
							return array;
						}
						i++;
					}
				}
				if (this.globalObject != null)
				{
					array = this.globalObjectTR.GetMember(name, (bindingAttr & ~BindingFlags.NonPublic) | BindingFlags.Static);
				}
				if (array != null && array.Length > 0)
				{
					return ScriptObject.WrapMembers(array, this.globalObject);
				}
			}
			else
			{
				array = this.typeReflector.GetMember(name, (bindingAttr & ~BindingFlags.NonPublic) | BindingFlags.Static);
				int num = array.Length;
				if (num > 0)
				{
					int num2 = 0;
					MemberInfo[] array2 = new MemberInfo[num];
					for (int j = 0; j < num; j++)
					{
						MemberInfo memberInfo = (array2[j] = array[j]);
						if (memberInfo.DeclaringType.IsAssignableFrom(Typeob.GlobalScope))
						{
							array2[j] = null;
							num2++;
						}
						else if (memberInfo is FieldInfo)
						{
							FieldInfo fieldInfo = (FieldInfo)memberInfo;
							if (fieldInfo.IsStatic && fieldInfo.FieldType == Typeob.Type)
							{
								Type type = (Type)fieldInfo.GetValue(null);
								if (type != null)
								{
									array2[j] = type;
								}
							}
						}
					}
					if (num2 == 0)
					{
						return array;
					}
					if (num2 == num)
					{
						return new MemberInfo[0];
					}
					MemberInfo[] array3 = new MemberInfo[num - num2];
					int num3 = 0;
					foreach (MemberInfo memberInfo2 in array2)
					{
						if (memberInfo2 != null)
						{
							array3[num3++] = memberInfo2;
						}
					}
					return array3;
				}
			}
			if (this.parent != null && !calledFromParent && ((bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default || this.isComponentScope))
			{
				this.recursive = true;
				try
				{
					array = this.parent.GetMember(name, bindingAttr);
				}
				finally
				{
					this.recursive = false;
				}
				if (array != null && array.Length > 0)
				{
					return array;
				}
			}
			return new MemberInfo[0];
		}

		// Token: 0x06000680 RID: 1664 RVA: 0x0002E070 File Offset: 0x0002D070
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			if (this.recursive)
			{
				return new MemberInfo[0];
			}
			MemberInfoList memberInfoList = new MemberInfoList();
			if (this.isComponentScope)
			{
				MemberInfo[] members = Globals.TypeRefs.ToReferenceContext(base.GetType()).GetMembers(bindingAttr | BindingFlags.DeclaredOnly);
				if (members != null)
				{
					foreach (MemberInfo memberInfo in members)
					{
						memberInfoList.Add(memberInfo);
					}
				}
			}
			else
			{
				if (this.componentScopes != null)
				{
					int j = 0;
					int count = this.componentScopes.Count;
					while (j < count)
					{
						GlobalScope globalScope = (GlobalScope)this.componentScopes[j];
						this.recursive = true;
						MemberInfo[] array2 = null;
						try
						{
							array2 = globalScope.GetMembers(bindingAttr);
						}
						finally
						{
							this.recursive = false;
						}
						if (array2 != null)
						{
							foreach (MemberInfo memberInfo2 in array2)
							{
								memberInfoList.Add(memberInfo2);
							}
						}
						j++;
					}
				}
				foreach (object obj in this.field_table)
				{
					FieldInfo fieldInfo = (FieldInfo)obj;
					memberInfoList.Add(fieldInfo);
				}
			}
			if (this.parent != null && (this.isComponentScope || (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default))
			{
				this.recursive = true;
				MemberInfo[] array4 = null;
				try
				{
					array4 = this.parent.GetMembers(bindingAttr);
				}
				finally
				{
					this.recursive = false;
				}
				if (array4 != null)
				{
					foreach (MemberInfo memberInfo3 in array4)
					{
						memberInfoList.Add(memberInfo3);
					}
				}
			}
			return memberInfoList.ToArray();
		}

		// Token: 0x06000681 RID: 1665 RVA: 0x0002E210 File Offset: 0x0002D210
		public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
		{
			return base.GetMethods(bindingAttr | BindingFlags.DeclaredOnly);
		}

		// Token: 0x06000682 RID: 1666 RVA: 0x0002E21B File Offset: 0x0002D21B
		public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
		{
			return base.GetProperties(bindingAttr | BindingFlags.DeclaredOnly);
		}

		// Token: 0x06000683 RID: 1667 RVA: 0x0002E228 File Offset: 0x0002D228
		internal override void GetPropertyEnumerator(ArrayList enums, ArrayList objects)
		{
			FieldInfo[] fields = this.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			if (fields.Length > 0)
			{
				enums.Add(fields.GetEnumerator());
				objects.Add(this);
			}
			ScriptObject parent = base.GetParent();
			if (parent != null)
			{
				parent.GetPropertyEnumerator(enums, objects);
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x0002E26C File Offset: 0x0002D26C
		internal void SetFast()
		{
			this.fast = true;
			this.isKnownAtCompileTime = true;
			if (this.globalObject != null)
			{
				this.globalObject = GlobalObject.commonInstance;
				this.globalObjectTR = TypeReflector.GetTypeReflectorFor(Globals.TypeRefs.ToReferenceContext(this.globalObject.GetType()));
			}
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x0002E2BA File Offset: 0x0002D2BA
		void IExpando.RemoveMember(MemberInfo m)
		{
			this.DeleteMember(m.Name);
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x0002E2CC File Offset: 0x0002D2CC
		internal override void SetMemberValue(string name, object value)
		{
			MemberInfo[] member = this.GetMember(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			if (member.Length == 0)
			{
				if (VsaEngine.executeForJSEE)
				{
					throw new JScriptException(JSError.UndefinedIdentifier, new Context(new DocumentContext("", null), name));
				}
				FieldInfo fieldInfo = this.AddField(name);
				if (fieldInfo != null)
				{
					fieldInfo.SetValue(this, value);
				}
				return;
			}
			else
			{
				MemberInfo memberInfo = LateBinding.SelectMember(member);
				if (memberInfo == null)
				{
					throw new JScriptException(JSError.AssignmentToReadOnly);
				}
				LateBinding.SetMemberValue(this, name, value, memberInfo, member);
				return;
			}
		}

		// Token: 0x040002F7 RID: 759
		private ArrayList componentScopes;

		// Token: 0x040002F8 RID: 760
		internal GlobalObject globalObject;

		// Token: 0x040002F9 RID: 761
		private bool recursive;

		// Token: 0x040002FA RID: 762
		internal bool evilScript;

		// Token: 0x040002FB RID: 763
		internal object thisObject;

		// Token: 0x040002FC RID: 764
		internal bool isComponentScope;

		// Token: 0x040002FD RID: 765
		private TypeReflector globalObjectTR;

		// Token: 0x040002FE RID: 766
		private TypeReflector typeReflector;
	}
}
