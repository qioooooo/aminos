using System;
using System.Collections;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x02000007 RID: 7
	[ComVisible(true)]
	public abstract class ActivationObject : ScriptObject, IActivationObject
	{
		// Token: 0x06000037 RID: 55 RVA: 0x000031CD File Offset: 0x000021CD
		internal ActivationObject(ScriptObject parent)
			: base(parent)
		{
			this.name_table = new SimpleHashtable(32U);
			this.field_table = new ArrayList();
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000031F0 File Offset: 0x000021F0
		internal virtual JSVariableField AddFieldOrUseExistingField(string name, object value, FieldAttributes attributeFlags)
		{
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo is JSVariableField)
			{
				if (!(value is Missing))
				{
					((JSVariableField)fieldInfo).value = value;
				}
				return (JSVariableField)fieldInfo;
			}
			if (value is Missing)
			{
				value = null;
			}
			return this.AddNewField(name, value, attributeFlags);
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003248 File Offset: 0x00002248
		internal void AddClassesExcluding(ClassScope excludedClass, string name, ArrayList result)
		{
			ArrayList arrayList = new ArrayList();
			foreach (MemberInfo memberInfo in this.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (memberInfo is JSVariableField && ((JSVariableField)memberInfo).IsLiteral)
				{
					object value = ((JSVariableField)memberInfo).value;
					if (value is ClassScope)
					{
						ClassScope classScope = (ClassScope)value;
						if (!(classScope.name != memberInfo.Name) && (excludedClass == null || !excludedClass.IsSameOrDerivedFrom(classScope)) && classScope.GetMember(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Length > 0)
						{
							arrayList.Add(classScope);
						}
					}
				}
			}
			if (arrayList.Count == 0)
			{
				return;
			}
			ClassScope[] array = new ClassScope[arrayList.Count];
			arrayList.CopyTo(array);
			Array.Sort<ClassScope>(array);
			result.AddRange(array);
		}

		// Token: 0x0600003A RID: 58 RVA: 0x00003310 File Offset: 0x00002310
		internal virtual JSVariableField AddNewField(string name, object value, FieldAttributes attributeFlags)
		{
			JSVariableField jsvariableField = this.CreateField(name, attributeFlags, value);
			this.name_table[name] = jsvariableField;
			this.field_table.Add(jsvariableField);
			return jsvariableField;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00003342 File Offset: 0x00002342
		protected virtual JSVariableField CreateField(string name, FieldAttributes attributeFlags, object value)
		{
			return new JSGlobalField(this, name, value, attributeFlags | FieldAttributes.Static);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00003350 File Offset: 0x00002350
		public virtual FieldInfo GetField(string name, int lexLevel)
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00003359 File Offset: 0x00002359
		internal virtual string GetName()
		{
			return null;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000335C File Offset: 0x0000235C
		public virtual object GetDefaultThisObject()
		{
			return ((IActivationObject)base.GetParent()).GetDefaultThisObject();
		}

		// Token: 0x0600003F RID: 63 RVA: 0x0000336E File Offset: 0x0000236E
		public virtual GlobalScope GetGlobalScope()
		{
			return ((IActivationObject)base.GetParent()).GetGlobalScope();
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00003380 File Offset: 0x00002380
		public virtual FieldInfo GetLocalField(string name)
		{
			return (FieldInfo)this.name_table[name];
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003394 File Offset: 0x00002394
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo != null)
			{
				return new MemberInfo[] { fieldInfo };
			}
			if (this.parent != null && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default)
			{
				return ScriptObject.WrapMembers(this.parent.GetMember(name, bindingAttr), this.parent);
			}
			return new MemberInfo[0];
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000033F0 File Offset: 0x000023F0
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			int count = this.field_table.Count;
			MemberInfo[] array = new MemberInfo[count];
			for (int i = 0; i < count; i++)
			{
				array[i] = (MemberInfo)this.field_table[i];
			}
			return array;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003434 File Offset: 0x00002434
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object GetMemberValue(string name, int lexlevel)
		{
			if (lexlevel <= 0)
			{
				return Missing.Value;
			}
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo != null)
			{
				return fieldInfo.GetValue(this);
			}
			if (this.parent != null)
			{
				return ((IActivationObject)this.parent).GetMemberValue(name, lexlevel - 1);
			}
			return Missing.Value;
		}

		// Token: 0x04000013 RID: 19
		internal bool isKnownAtCompileTime;

		// Token: 0x04000014 RID: 20
		internal bool fast;

		// Token: 0x04000015 RID: 21
		internal SimpleHashtable name_table;

		// Token: 0x04000016 RID: 22
		protected ArrayList field_table;
	}
}
