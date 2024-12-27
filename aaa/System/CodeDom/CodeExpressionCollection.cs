using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000058 RID: 88
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeExpressionCollection : CollectionBase
	{
		// Token: 0x06000350 RID: 848 RVA: 0x0001363E File Offset: 0x0001263E
		public CodeExpressionCollection()
		{
		}

		// Token: 0x06000351 RID: 849 RVA: 0x00013646 File Offset: 0x00012646
		public CodeExpressionCollection(CodeExpressionCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00013655 File Offset: 0x00012655
		public CodeExpressionCollection(CodeExpression[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x1700008A RID: 138
		public CodeExpression this[int index]
		{
			get
			{
				return (CodeExpression)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00013686 File Offset: 0x00012686
		public int Add(CodeExpression value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00013694 File Offset: 0x00012694
		public void AddRange(CodeExpression[] value)
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

		// Token: 0x06000357 RID: 855 RVA: 0x000136C8 File Offset: 0x000126C8
		public void AddRange(CodeExpressionCollection value)
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

		// Token: 0x06000358 RID: 856 RVA: 0x00013704 File Offset: 0x00012704
		public bool Contains(CodeExpression value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000359 RID: 857 RVA: 0x00013712 File Offset: 0x00012712
		public void CopyTo(CodeExpression[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00013721 File Offset: 0x00012721
		public int IndexOf(CodeExpression value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x0600035B RID: 859 RVA: 0x0001372F File Offset: 0x0001272F
		public void Insert(int index, CodeExpression value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x0600035C RID: 860 RVA: 0x0001373E File Offset: 0x0001273E
		public void Remove(CodeExpression value)
		{
			base.List.Remove(value);
		}
	}
}
