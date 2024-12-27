using System;
using System.Collections;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x02000127 RID: 295
	public sealed class TypeReflector : ScriptObject
	{
		// Token: 0x06000C87 RID: 3207 RVA: 0x0005B7A0 File Offset: 0x0005A7A0
		internal TypeReflector(Type type)
			: base(null)
		{
			this.defaultMembers = null;
			ArrayList arrayList = new ArrayList(512);
			int num = 0;
			SimpleHashtable simpleHashtable = new SimpleHashtable(256U);
			foreach (MemberInfo memberInfo in type.GetMembers(BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy))
			{
				string name = memberInfo.Name;
				object obj = simpleHashtable[name];
				if (obj == null)
				{
					simpleHashtable[name] = num++;
					arrayList.Add(memberInfo);
				}
				else
				{
					int num2 = (int)obj;
					obj = arrayList[num2];
					MemberInfo memberInfo2 = obj as MemberInfo;
					if (memberInfo2 != null)
					{
						MemberInfoList memberInfoList = new MemberInfoList();
						memberInfoList.Add(memberInfo2);
						memberInfoList.Add(memberInfo);
						arrayList[num2] = memberInfoList;
					}
					else
					{
						((MemberInfoList)obj).Add(memberInfo);
					}
				}
			}
			this.staticMembers = simpleHashtable;
			SimpleHashtable simpleHashtable2 = new SimpleHashtable(256U);
			foreach (MemberInfo memberInfo3 in type.GetMembers(BindingFlags.Instance | BindingFlags.Public))
			{
				string name2 = memberInfo3.Name;
				object obj2 = simpleHashtable2[name2];
				if (obj2 == null)
				{
					simpleHashtable2[name2] = num++;
					arrayList.Add(memberInfo3);
				}
				else
				{
					int num3 = (int)obj2;
					obj2 = arrayList[num3];
					MemberInfo memberInfo4 = obj2 as MemberInfo;
					if (memberInfo4 != null)
					{
						MemberInfoList memberInfoList2 = new MemberInfoList();
						memberInfoList2.Add(memberInfo4);
						memberInfoList2.Add(memberInfo3);
						arrayList[num3] = memberInfoList2;
					}
					else
					{
						((MemberInfoList)obj2).Add(memberInfo3);
					}
				}
			}
			this.instanceMembers = simpleHashtable2;
			this.memberLookupTable = arrayList;
			this.memberInfos = new MemberInfo[num][];
			this.type = type;
			this.implementsIReflect = null;
			this.is__ComObject = null;
			this.hashCode = (uint)type.GetHashCode();
			this.next = null;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0005B988 File Offset: 0x0005A988
		internal MemberInfo[] GetDefaultMembers()
		{
			MemberInfo[] array = this.defaultMembers;
			if (array == null)
			{
				array = JSBinder.GetDefaultMembers(this.type);
				if (array == null)
				{
					array = new MemberInfo[0];
				}
				TypeReflector.WrapMembers(this.defaultMembers = array);
			}
			return array;
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0005B9C8 File Offset: 0x0005A9C8
		public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr)
		{
			bool flag = (bindingAttr & BindingFlags.Instance) != BindingFlags.Default;
			SimpleHashtable simpleHashtable = (flag ? this.instanceMembers : this.staticMembers);
			object obj = simpleHashtable[name];
			if (obj == null)
			{
				if ((bindingAttr & BindingFlags.IgnoreCase) != BindingFlags.Default)
				{
					obj = simpleHashtable.IgnoreCaseGet(name);
				}
				if (obj == null)
				{
					if (flag && (bindingAttr & BindingFlags.Static) != BindingFlags.Default)
					{
						return this.GetMember(name, bindingAttr & ~BindingFlags.Instance);
					}
					return TypeReflector.EmptyMembers;
				}
			}
			int num = (int)obj;
			MemberInfo[] array = this.memberInfos[num];
			if (array == null)
			{
				return this.GetNewMemberArray(name, num);
			}
			return array;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0005BA48 File Offset: 0x0005AA48
		private MemberInfo[] GetNewMemberArray(string name, int index)
		{
			object obj = this.memberLookupTable[index];
			if (obj == null)
			{
				return this.memberInfos[index];
			}
			MemberInfo memberInfo = obj as MemberInfo;
			MemberInfo[] array;
			if (memberInfo != null)
			{
				array = new MemberInfo[] { memberInfo };
			}
			else
			{
				array = ((MemberInfoList)obj).ToArray();
			}
			this.memberInfos[index] = array;
			this.memberLookupTable[index] = null;
			TypeReflector.WrapMembers(array);
			return array;
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0005BAB2 File Offset: 0x0005AAB2
		public override MemberInfo[] GetMembers(BindingFlags bindingAttr)
		{
			throw new JScriptException(JSError.InternalError);
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x0005BABC File Offset: 0x0005AABC
		internal static TypeReflector GetTypeReflectorFor(Type type)
		{
			TypeReflector typeReflector = TypeReflector.Table[type];
			if (typeReflector != null)
			{
				return typeReflector;
			}
			typeReflector = new TypeReflector(type);
			lock (TypeReflector.Table)
			{
				TypeReflector typeReflector2 = TypeReflector.Table[type];
				if (typeReflector2 != null)
				{
					return typeReflector2;
				}
				TypeReflector.Table[type] = typeReflector;
			}
			return typeReflector;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x0005BB28 File Offset: 0x0005AB28
		internal bool ImplementsIReflect()
		{
			object obj = this.implementsIReflect;
			if (obj != null)
			{
				return (bool)obj;
			}
			bool flag = typeof(IReflect).IsAssignableFrom(this.type);
			this.implementsIReflect = flag;
			return flag;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x0005BB6C File Offset: 0x0005AB6C
		internal bool Is__ComObject()
		{
			object obj = this.is__ComObject;
			if (obj != null)
			{
				return (bool)obj;
			}
			bool flag = this.type.ToString() == "System.__ComObject";
			this.is__ComObject = flag;
			return flag;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x0005BBB0 File Offset: 0x0005ABB0
		private static void WrapMembers(MemberInfo[] members)
		{
			int i = 0;
			int num = members.Length;
			while (i < num)
			{
				MemberInfo memberInfo = members[i];
				MemberTypes memberType = memberInfo.MemberType;
				if (memberType != MemberTypes.Field)
				{
					if (memberType != MemberTypes.Method)
					{
						if (memberType == MemberTypes.Property)
						{
							members[i] = new JSPropertyInfo((PropertyInfo)memberInfo);
						}
					}
					else
					{
						members[i] = new JSMethodInfo((MethodInfo)memberInfo);
					}
				}
				else
				{
					members[i] = new JSFieldInfo((FieldInfo)memberInfo);
				}
				i++;
			}
		}

		// Token: 0x04000717 RID: 1815
		private MemberInfo[] defaultMembers;

		// Token: 0x04000718 RID: 1816
		private SimpleHashtable staticMembers;

		// Token: 0x04000719 RID: 1817
		private SimpleHashtable instanceMembers;

		// Token: 0x0400071A RID: 1818
		private MemberInfo[][] memberInfos;

		// Token: 0x0400071B RID: 1819
		private ArrayList memberLookupTable;

		// Token: 0x0400071C RID: 1820
		internal Type type;

		// Token: 0x0400071D RID: 1821
		private object implementsIReflect;

		// Token: 0x0400071E RID: 1822
		private object is__ComObject;

		// Token: 0x0400071F RID: 1823
		internal uint hashCode;

		// Token: 0x04000720 RID: 1824
		internal TypeReflector next;

		// Token: 0x04000721 RID: 1825
		private static MemberInfo[] EmptyMembers = new MemberInfo[0];

		// Token: 0x04000722 RID: 1826
		private static TRHashtable Table = new TRHashtable();
	}
}
