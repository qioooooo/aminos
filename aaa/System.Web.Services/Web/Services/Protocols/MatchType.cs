using System;
using System.Collections;
using System.Reflection;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000050 RID: 80
	internal class MatchType
	{
		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060001BD RID: 445 RVA: 0x000075EA File Offset: 0x000065EA
		internal Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x060001BE RID: 446 RVA: 0x000075F4 File Offset: 0x000065F4
		internal static MatchType Reflect(Type type)
		{
			MatchType matchType = new MatchType();
			matchType.type = type;
			MemberInfo[] members = type.GetMembers(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public);
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < members.Length; i++)
			{
				MatchMember matchMember = MatchMember.Reflect(members[i]);
				if (matchMember != null)
				{
					arrayList.Add(matchMember);
				}
			}
			matchType.fields = (MatchMember[])arrayList.ToArray(typeof(MatchMember));
			return matchType;
		}

		// Token: 0x060001BF RID: 447 RVA: 0x00007660 File Offset: 0x00006660
		internal object Match(string text)
		{
			object obj = Activator.CreateInstance(this.type);
			for (int i = 0; i < this.fields.Length; i++)
			{
				this.fields[i].Match(obj, text);
			}
			return obj;
		}

		// Token: 0x040002B0 RID: 688
		private Type type;

		// Token: 0x040002B1 RID: 689
		private MatchMember[] fields;
	}
}
