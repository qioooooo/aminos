using System;
using System.Collections;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x0200011B RID: 283
	public sealed class SuperTypeMembersSorter
	{
		// Token: 0x06000BA7 RID: 2983 RVA: 0x00058ADE File Offset: 0x00057ADE
		internal SuperTypeMembersSorter()
		{
			this.members = new SimpleHashtable(64U);
			this.names = new ArrayList();
			this.count = 0;
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x00058B08 File Offset: 0x00057B08
		internal void Add(MemberInfo[] members)
		{
			foreach (MemberInfo memberInfo in members)
			{
				this.Add(memberInfo);
			}
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x00058B30 File Offset: 0x00057B30
		internal void Add(MemberInfo member)
		{
			this.count++;
			string name = member.Name;
			object obj = this.members[name];
			if (obj == null)
			{
				this.members[name] = member;
				this.names.Add(name);
				return;
			}
			if (obj is MemberInfo)
			{
				ArrayList arrayList = new ArrayList(8);
				arrayList.Add(obj);
				arrayList.Add(member);
				this.members[name] = arrayList;
				return;
			}
			((ArrayList)obj).Add(member);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x00058BB8 File Offset: 0x00057BB8
		internal object[] GetMembers()
		{
			object[] array = new object[this.count];
			int num = 0;
			foreach (object obj in this.names)
			{
				object obj2 = this.members[obj];
				if (obj2 is MemberInfo)
				{
					array[num++] = obj2;
				}
				else
				{
					foreach (object obj3 in ((ArrayList)obj2))
					{
						array[num++] = obj3;
					}
				}
			}
			return array;
		}

		// Token: 0x040006FA RID: 1786
		private SimpleHashtable members;

		// Token: 0x040006FB RID: 1787
		private ArrayList names;

		// Token: 0x040006FC RID: 1788
		private int count;
	}
}
