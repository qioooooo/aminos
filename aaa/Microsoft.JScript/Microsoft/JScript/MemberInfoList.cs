using System;
using System.Reflection;

namespace Microsoft.JScript
{
	// Token: 0x020000E7 RID: 231
	public sealed class MemberInfoList
	{
		// Token: 0x06000A53 RID: 2643 RVA: 0x0004EC36 File Offset: 0x0004DC36
		internal MemberInfoList()
		{
			this.count = 0;
			this.list = new MemberInfo[16];
		}

		// Token: 0x06000A54 RID: 2644 RVA: 0x0004EC54 File Offset: 0x0004DC54
		internal void Add(MemberInfo elem)
		{
			int num = this.count++;
			if (this.list.Length == num)
			{
				this.Grow();
			}
			this.list[num] = elem;
		}

		// Token: 0x06000A55 RID: 2645 RVA: 0x0004EC90 File Offset: 0x0004DC90
		internal void AddRange(MemberInfo[] elems)
		{
			foreach (MemberInfo memberInfo in elems)
			{
				this.Add(memberInfo);
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0004ECB8 File Offset: 0x0004DCB8
		private void Grow()
		{
			MemberInfo[] array = this.list;
			int num = array.Length;
			MemberInfo[] array2 = (this.list = new MemberInfo[num + 16]);
			for (int i = 0; i < num; i++)
			{
				array2[i] = array[i];
			}
		}

		// Token: 0x170001D9 RID: 473
		internal MemberInfo this[int i]
		{
			get
			{
				return this.list[i];
			}
			set
			{
				this.list[i] = value;
			}
		}

		// Token: 0x06000A59 RID: 2649 RVA: 0x0004ED0C File Offset: 0x0004DD0C
		internal MemberInfo[] ToArray()
		{
			int num = this.count;
			MemberInfo[] array = new MemberInfo[num];
			MemberInfo[] array2 = this.list;
			for (int i = 0; i < num; i++)
			{
				array[i] = array2[i];
			}
			return array;
		}

		// Token: 0x04000669 RID: 1641
		internal int count;

		// Token: 0x0400066A RID: 1642
		private MemberInfo[] list;
	}
}
