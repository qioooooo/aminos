using System;
using System.Diagnostics;
using System.Reflection;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.JScript
{
	// Token: 0x02000147 RID: 327
	internal sealed class WithObject : ScriptObject, IActivationObject
	{
		// Token: 0x06000F00 RID: 3840 RVA: 0x000651E1 File Offset: 0x000641E1
		internal WithObject(ScriptObject parent, object contained_object)
			: this(parent, contained_object, false)
		{
		}

		// Token: 0x06000F01 RID: 3841 RVA: 0x000651EC File Offset: 0x000641EC
		internal WithObject(ScriptObject parent, object contained_object, bool isSuperType)
			: base(parent)
		{
			this.contained_object = contained_object;
			this.isKnownAtCompileTime = contained_object is Type || (contained_object is ClassScope && ((ClassScope)contained_object).noExpando) || (contained_object is JSObject && ((JSObject)contained_object).noExpando);
			this.isSuperType = isSuperType;
		}

		// Token: 0x06000F02 RID: 3842 RVA: 0x0006524A File Offset: 0x0006424A
		public object GetDefaultThisObject()
		{
			return this.contained_object;
		}

		// Token: 0x06000F03 RID: 3843 RVA: 0x00065254 File Offset: 0x00064254
		public FieldInfo GetField(string name, int lexLevel)
		{
			if (lexLevel <= 0)
			{
				return null;
			}
			IReflect reflect;
			if (this.contained_object is IReflect)
			{
				reflect = (IReflect)this.contained_object;
			}
			else
			{
				reflect = Globals.TypeRefs.ToReferenceContext(this.contained_object.GetType());
			}
			FieldInfo fieldInfo = reflect.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if (fieldInfo != null)
			{
				return new JSWrappedField(fieldInfo, this.contained_object);
			}
			PropertyInfo property = reflect.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public);
			if (property != null)
			{
				return new JSPropertyField(property, this.contained_object);
			}
			if (this.parent != null && lexLevel > 1)
			{
				fieldInfo = ((IActivationObject)this.parent).GetField(name, lexLevel - 1);
				if (fieldInfo != null)
				{
					return new JSWrappedField(fieldInfo, this.parent);
				}
			}
			return null;
		}

		// Token: 0x06000F04 RID: 3844 RVA: 0x000652FE File Offset: 0x000642FE
		public GlobalScope GetGlobalScope()
		{
			return ((IActivationObject)base.GetParent()).GetGlobalScope();
		}

		// Token: 0x06000F05 RID: 3845 RVA: 0x00065310 File Offset: 0x00064310
		FieldInfo IActivationObject.GetLocalField(string name)
		{
			return null;
		}

		// Token: 0x06000F06 RID: 3846 RVA: 0x00065313 File Offset: 0x00064313
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			return this.GetMember(name, bindingAttr, true);
		}

		// Token: 0x06000F07 RID: 3847 RVA: 0x00065320 File Offset: 0x00064320
		internal MemberInfo[] GetMember(string name, BindingFlags bindingAttr, bool forceInstanceLookup)
		{
			Type type = null;
			BindingFlags bindingFlags = bindingAttr;
			if (forceInstanceLookup && this.isSuperType && (bindingAttr & BindingFlags.FlattenHierarchy) == BindingFlags.Default)
			{
				bindingFlags |= BindingFlags.Instance;
			}
			object value = this.contained_object;
			MemberInfo[] array;
			for (;;)
			{
				IReflect reflect;
				if (value is IReflect)
				{
					reflect = (IReflect)value;
					if (value is Type && !this.isSuperType)
					{
						bindingFlags &= ~BindingFlags.Instance;
					}
				}
				else
				{
					type = (reflect = Globals.TypeRefs.ToReferenceContext(value.GetType()));
				}
				array = reflect.GetMember(name, bindingFlags & ~BindingFlags.DeclaredOnly);
				if (array.Length > 0)
				{
					break;
				}
				if (value is Type && !this.isSuperType)
				{
					array = Typeob.Type.GetMember(name, BindingFlags.Instance | BindingFlags.Public);
				}
				if (array.Length > 0)
				{
					goto Block_10;
				}
				if (type != null && type.IsNestedPublic)
				{
					try
					{
						new ReflectionPermission(ReflectionPermissionFlag.TypeInformation | ReflectionPermissionFlag.MemberAccess).Assert();
						FieldInfo field = type.GetField("outer class instance", BindingFlags.Instance | BindingFlags.NonPublic);
						if (field != null)
						{
							value = field.GetValue(value);
							continue;
						}
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					goto IL_00E4;
				}
				goto IL_00E4;
			}
			return ScriptObject.WrapMembers(array, value);
			Block_10:
			return ScriptObject.WrapMembers(array, value);
			IL_00E4:
			if (array.Length > 0)
			{
				return ScriptObject.WrapMembers(array, value);
			}
			return new MemberInfo[0];
		}

		// Token: 0x06000F08 RID: 3848 RVA: 0x00065438 File Offset: 0x00064438
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			return ((IReflect)this.contained_object).GetMembers(bindingAttr);
		}

		// Token: 0x06000F09 RID: 3849 RVA: 0x0006544C File Offset: 0x0006444C
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override object GetMemberValue(string name)
		{
			object memberValue = LateBinding.GetMemberValue2(this.contained_object, name);
			if (!(memberValue is Missing))
			{
				return memberValue;
			}
			if (this.parent != null)
			{
				return this.parent.GetMemberValue(name);
			}
			return Missing.Value;
		}

		// Token: 0x06000F0A RID: 3850 RVA: 0x0006548C File Offset: 0x0006448C
		[DebuggerStepThrough]
		[DebuggerHidden]
		public object GetMemberValue(string name, int lexlevel)
		{
			if (lexlevel <= 0)
			{
				return Missing.Value;
			}
			object memberValue = LateBinding.GetMemberValue2(this.contained_object, name);
			if (memberValue != Missing.Value)
			{
				return memberValue;
			}
			return ((IActivationObject)this.parent).GetMemberValue(name, lexlevel - 1);
		}

		// Token: 0x06000F0B RID: 3851 RVA: 0x000654CE File Offset: 0x000644CE
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override void SetMemberValue(string name, object value)
		{
			if (LateBinding.GetMemberValue2(this.contained_object, name) is Missing)
			{
				this.parent.SetMemberValue(name, value);
				return;
			}
			LateBinding.SetMemberValue(this.contained_object, name, value);
		}

		// Token: 0x040007F6 RID: 2038
		internal object contained_object;

		// Token: 0x040007F7 RID: 2039
		internal bool isKnownAtCompileTime;

		// Token: 0x040007F8 RID: 2040
		private bool isSuperType;
	}
}
