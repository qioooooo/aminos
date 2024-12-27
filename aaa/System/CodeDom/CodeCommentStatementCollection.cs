using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200004B RID: 75
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCommentStatementCollection : CollectionBase
	{
		// Token: 0x060002EB RID: 747 RVA: 0x00012E38 File Offset: 0x00011E38
		public CodeCommentStatementCollection()
		{
		}

		// Token: 0x060002EC RID: 748 RVA: 0x00012E40 File Offset: 0x00011E40
		public CodeCommentStatementCollection(CodeCommentStatementCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x060002ED RID: 749 RVA: 0x00012E4F File Offset: 0x00011E4F
		public CodeCommentStatementCollection(CodeCommentStatement[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000066 RID: 102
		public CodeCommentStatement this[int index]
		{
			get
			{
				return (CodeCommentStatement)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00012E80 File Offset: 0x00011E80
		public int Add(CodeCommentStatement value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00012E90 File Offset: 0x00011E90
		public void AddRange(CodeCommentStatement[] value)
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

		// Token: 0x060002F2 RID: 754 RVA: 0x00012EC4 File Offset: 0x00011EC4
		public void AddRange(CodeCommentStatementCollection value)
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

		// Token: 0x060002F3 RID: 755 RVA: 0x00012F00 File Offset: 0x00011F00
		public bool Contains(CodeCommentStatement value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060002F4 RID: 756 RVA: 0x00012F0E File Offset: 0x00011F0E
		public void CopyTo(CodeCommentStatement[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060002F5 RID: 757 RVA: 0x00012F1D File Offset: 0x00011F1D
		public int IndexOf(CodeCommentStatement value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00012F2B File Offset: 0x00011F2B
		public void Insert(int index, CodeCommentStatement value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x00012F3A File Offset: 0x00011F3A
		public void Remove(CodeCommentStatement value)
		{
			base.List.Remove(value);
		}
	}
}
