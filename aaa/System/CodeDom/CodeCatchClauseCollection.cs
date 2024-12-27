using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000046 RID: 70
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeCatchClauseCollection : CollectionBase
	{
		// Token: 0x060002C8 RID: 712 RVA: 0x00012C0D File Offset: 0x00011C0D
		public CodeCatchClauseCollection()
		{
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x00012C15 File Offset: 0x00011C15
		public CodeCatchClauseCollection(CodeCatchClauseCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x060002CA RID: 714 RVA: 0x00012C24 File Offset: 0x00011C24
		public CodeCatchClauseCollection(CodeCatchClause[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x1700005F RID: 95
		public CodeCatchClause this[int index]
		{
			get
			{
				return (CodeCatchClause)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060002CD RID: 717 RVA: 0x00012C55 File Offset: 0x00011C55
		public int Add(CodeCatchClause value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060002CE RID: 718 RVA: 0x00012C64 File Offset: 0x00011C64
		public void AddRange(CodeCatchClause[] value)
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

		// Token: 0x060002CF RID: 719 RVA: 0x00012C98 File Offset: 0x00011C98
		public void AddRange(CodeCatchClauseCollection value)
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

		// Token: 0x060002D0 RID: 720 RVA: 0x00012CD4 File Offset: 0x00011CD4
		public bool Contains(CodeCatchClause value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x00012CE2 File Offset: 0x00011CE2
		public void CopyTo(CodeCatchClause[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x00012CF1 File Offset: 0x00011CF1
		public int IndexOf(CodeCatchClause value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x00012CFF File Offset: 0x00011CFF
		public void Insert(int index, CodeCatchClause value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00012D0E File Offset: 0x00011D0E
		public void Remove(CodeCatchClause value)
		{
			base.List.Remove(value);
		}
	}
}
