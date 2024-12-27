using System;
using System.Reflection;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000148 RID: 328
	internal sealed class WrappedNamespace : ActivationObject
	{
		// Token: 0x06000F0C RID: 3852 RVA: 0x000654FE File Offset: 0x000644FE
		internal WrappedNamespace(string name, VsaEngine engine)
			: this(name, engine, true)
		{
		}

		// Token: 0x06000F0D RID: 3853 RVA: 0x00065509 File Offset: 0x00064509
		internal WrappedNamespace(string name, VsaEngine engine, bool AddReferences)
			: base(null)
		{
			this.name = name;
			this.engine = engine;
			this.isKnownAtCompileTime = true;
			if (name.Length > 0 && AddReferences)
			{
				engine.TryToAddImplicitAssemblyReference(name);
			}
		}

		// Token: 0x06000F0E RID: 3854 RVA: 0x0006553C File Offset: 0x0006453C
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			FieldInfo fieldInfo = (FieldInfo)this.name_table[name];
			if (fieldInfo != null)
			{
				return new MemberInfo[] { fieldInfo };
			}
			FieldAttributes fieldAttributes = FieldAttributes.Literal;
			string text = ((this.name == null || this.name.Length == 0) ? name : (this.name + "." + name));
			object obj = null;
			if (this.name != null && this.name.Length > 0)
			{
				obj = this.engine.GetClass(text);
			}
			if (obj == null)
			{
				obj = this.engine.GetType(text);
				if (obj != null && !((Type)obj).IsPublic)
				{
					if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
					{
						obj = null;
					}
					else
					{
						fieldAttributes |= FieldAttributes.Private;
					}
				}
			}
			else if ((((ClassScope)obj).owner.attributes & TypeAttributes.Public) == TypeAttributes.NotPublic)
			{
				if ((bindingAttr & BindingFlags.NonPublic) == BindingFlags.Default)
				{
					obj = null;
				}
				else
				{
					fieldAttributes |= FieldAttributes.Private;
				}
			}
			if (obj != null)
			{
				JSGlobalField jsglobalField = (JSGlobalField)this.CreateField(name, fieldAttributes, obj);
				if (this.engine.doFast)
				{
					jsglobalField.type = new TypeExpression(new ConstantWrapper(Typeob.Type, null));
				}
				this.name_table[name] = jsglobalField;
				this.field_table.Add(jsglobalField);
				return new MemberInfo[] { jsglobalField };
			}
			if (this.parent != null && (bindingAttr & BindingFlags.DeclaredOnly) == BindingFlags.Default)
			{
				return this.parent.GetMember(name, bindingAttr);
			}
			return new MemberInfo[0];
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x00065697 File Offset: 0x00064697
		public override string ToString()
		{
			return this.name;
		}

		// Token: 0x040007F9 RID: 2041
		internal string name;
	}
}
