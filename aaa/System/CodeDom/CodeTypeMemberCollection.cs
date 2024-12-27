using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200007F RID: 127
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeTypeMemberCollection : CollectionBase
	{
		// Token: 0x06000478 RID: 1144 RVA: 0x00014D76 File Offset: 0x00013D76
		public CodeTypeMemberCollection()
		{
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00014D7E File Offset: 0x00013D7E
		public CodeTypeMemberCollection(CodeTypeMemberCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00014D8D File Offset: 0x00013D8D
		public CodeTypeMemberCollection(CodeTypeMember[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170000DF RID: 223
		public CodeTypeMember this[int index]
		{
			get
			{
				return (CodeTypeMember)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00014DBE File Offset: 0x00013DBE
		public int Add(CodeTypeMember value)
		{
			return base.List.Add(value);
		}

		// Token: 0x0600047E RID: 1150 RVA: 0x00014DCC File Offset: 0x00013DCC
		public void AddRange(CodeTypeMember[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x00014E00 File Offset: 0x00013E00
		public void AddRange(CodeTypeMemberCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x00014E3C File Offset: 0x00013E3C
		public bool Contains(CodeTypeMember value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00014E4A File Offset: 0x00013E4A
		public void CopyTo(CodeTypeMember[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00014E59 File Offset: 0x00013E59
		public int IndexOf(CodeTypeMember value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00014E67 File Offset: 0x00013E67
		public void Insert(int index, CodeTypeMember value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x00014E76 File Offset: 0x00013E76
		public void Remove(CodeTypeMember value)
		{
			base.List.Remove(value);
		}
	}
}
