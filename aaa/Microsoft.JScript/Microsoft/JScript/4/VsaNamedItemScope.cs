using System;
using System.Diagnostics;
using System.Reflection;
using Microsoft.JScript.Vsa;

namespace Microsoft.JScript
{
	// Token: 0x02000140 RID: 320
	internal sealed class VsaNamedItemScope : ScriptObject, IActivationObject
	{
		// Token: 0x06000EA0 RID: 3744 RVA: 0x00062DF8 File Offset: 0x00061DF8
		internal VsaNamedItemScope(object hostObject, ScriptObject parent, VsaEngine engine)
			: base(parent)
		{
			this.namedItem = hostObject;
			if ((this.reflectObj = hostObject as IReflect) == null)
			{
				this.reflectObj = Globals.TypeRefs.ToReferenceContext(hostObject.GetType());
			}
			this.recursive = false;
			this.engine = engine;
		}

		// Token: 0x06000EA1 RID: 3745 RVA: 0x00062E48 File Offset: 0x00061E48
		private static MemberInfo[] GetAndWrapMember(IReflect reflect, object namedItem, string name, BindingFlags bindingAttr)
		{
			PropertyInfo property = reflect.GetProperty(name, bindingAttr);
			if (property != null)
			{
				MethodInfo getMethod = JSProperty.GetGetMethod(property, false);
				MethodInfo setMethod = JSProperty.GetSetMethod(property, false);
				if ((getMethod != null && !getMethod.IsStatic) || (setMethod != null && !setMethod.IsStatic))
				{
					MethodInfo method = reflect.GetMethod(name, bindingAttr);
					if (method != null && !method.IsStatic)
					{
						return new MemberInfo[]
						{
							new JSWrappedPropertyAndMethod(property, method, namedItem)
						};
					}
				}
			}
			MemberInfo[] member = reflect.GetMember(name, bindingAttr);
			if (member != null && member.Length > 0)
			{
				return ScriptObject.WrapMembers(member, namedItem);
			}
			return null;
		}

		// Token: 0x06000EA2 RID: 3746 RVA: 0x00062ED1 File Offset: 0x00061ED1
		public object GetDefaultThisObject()
		{
			return ((IActivationObject)base.GetParent()).GetDefaultThisObject();
		}

		// Token: 0x06000EA3 RID: 3747 RVA: 0x00062EE3 File Offset: 0x00061EE3
		public FieldInfo GetField(string name, int lexLevel)
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x06000EA4 RID: 3748 RVA: 0x00062EEC File Offset: 0x00061EEC
		public GlobalScope GetGlobalScope()
		{
			return ((IActivationObject)base.GetParent()).GetGlobalScope();
		}

		// Token: 0x06000EA5 RID: 3749 RVA: 0x00062EFE File Offset: 0x00061EFE
		FieldInfo IActivationObject.GetLocalField(string name)
		{
			return null;
		}

		// Token: 0x06000EA6 RID: 3750 RVA: 0x00062F04 File Offset: 0x00061F04
		[DebuggerHidden]
		[DebuggerStepThrough]
		public object GetMemberValue(string name, int lexlevel)
		{
			if (lexlevel <= 0)
			{
				return Missing.Value;
			}
			object memberValue = LateBinding.GetMemberValue2(this.namedItem, name);
			if (!(memberValue is Missing))
			{
				return memberValue;
			}
			return ((IActivationObject)this.parent).GetMemberValue(name, lexlevel - 1);
		}

		// Token: 0x06000EA7 RID: 3751 RVA: 0x00062F48 File Offset: 0x00061F48
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			MemberInfo[] array = null;
			if (!this.recursive && this.reflectObj != null)
			{
				this.recursive = true;
				try
				{
					ISite2 site;
					if (!this.reflectObj.GetType().IsCOMObject || (site = this.engine.Site as ISite2) == null)
					{
						array = ScriptObject.WrapMembers(this.reflectObj.GetMember(name, bindingAttr), this.namedItem);
					}
					else if ((array = VsaNamedItemScope.GetAndWrapMember(this.reflectObj, this.namedItem, name, bindingAttr)) == null)
					{
						object[] parentChain = site.GetParentChain(this.reflectObj);
						if (parentChain != null)
						{
							int num = parentChain.Length;
							for (int i = 0; i < num; i++)
							{
								IReflect reflect = parentChain[i] as IReflect;
								if (reflect != null && (array = VsaNamedItemScope.GetAndWrapMember(reflect, reflect, name, bindingAttr)) != null)
								{
									break;
								}
							}
						}
					}
				}
				finally
				{
					this.recursive = false;
				}
			}
			if (array != null)
			{
				return array;
			}
			return new MemberInfo[0];
		}

		// Token: 0x06000EA8 RID: 3752 RVA: 0x00063034 File Offset: 0x00062034
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			MemberInfo[] array = null;
			if (!this.recursive)
			{
				this.recursive = true;
				try
				{
					array = this.reflectObj.GetMembers(bindingAttr);
					if (array != null)
					{
						if (array.Length > 0)
						{
							SimpleHashtable simpleHashtable = this.namedItemWrappedMemberCache;
							if (simpleHashtable == null)
							{
								simpleHashtable = (this.namedItemWrappedMemberCache = new SimpleHashtable(16U));
							}
							array = ScriptObject.WrapMembers(array, this.namedItem, simpleHashtable);
						}
						else
						{
							array = null;
						}
					}
				}
				finally
				{
					this.recursive = false;
				}
			}
			return array;
		}

		// Token: 0x06000EA9 RID: 3753 RVA: 0x000630B0 File Offset: 0x000620B0
		[DebuggerStepThrough]
		[DebuggerHidden]
		internal override object GetMemberValue(string name)
		{
			object obj = Missing.Value;
			if (!this.recursive)
			{
				this.recursive = true;
				try
				{
					FieldInfo field = this.reflectObj.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					if (field == null)
					{
						PropertyInfo property = this.reflectObj.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						if (property != null)
						{
							obj = JSProperty.GetValue(property, this.namedItem, null);
						}
					}
					else
					{
						obj = field.GetValue(this.namedItem);
					}
					if (obj is Missing && this.parent != null)
					{
						obj = this.parent.GetMemberValue(name);
					}
				}
				finally
				{
					this.recursive = false;
				}
			}
			return obj;
		}

		// Token: 0x06000EAA RID: 3754 RVA: 0x0006314C File Offset: 0x0006214C
		[DebuggerHidden]
		[DebuggerStepThrough]
		internal override void SetMemberValue(string name, object value)
		{
			bool flag = false;
			if (!this.recursive)
			{
				this.recursive = true;
				try
				{
					FieldInfo field = this.reflectObj.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
					if (field == null)
					{
						PropertyInfo property = this.reflectObj.GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
						if (property != null)
						{
							JSProperty.SetValue(property, this.namedItem, value, null);
							flag = true;
						}
					}
					else
					{
						field.SetValue(this.namedItem, value);
						flag = true;
					}
					if (!flag && this.parent != null)
					{
						this.parent.SetMemberValue(name, value);
					}
				}
				finally
				{
					this.recursive = false;
				}
			}
		}

		// Token: 0x040007D9 RID: 2009
		internal object namedItem;

		// Token: 0x040007DA RID: 2010
		private SimpleHashtable namedItemWrappedMemberCache;

		// Token: 0x040007DB RID: 2011
		private IReflect reflectObj;

		// Token: 0x040007DC RID: 2012
		private bool recursive;
	}
}
