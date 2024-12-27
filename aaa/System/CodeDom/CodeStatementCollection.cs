using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000077 RID: 119
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeStatementCollection : CollectionBase
	{
		// Token: 0x06000437 RID: 1079 RVA: 0x00014712 File Offset: 0x00013712
		public CodeStatementCollection()
		{
		}

		// Token: 0x06000438 RID: 1080 RVA: 0x0001471A File Offset: 0x0001371A
		public CodeStatementCollection(CodeStatementCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06000439 RID: 1081 RVA: 0x00014729 File Offset: 0x00013729
		public CodeStatementCollection(CodeStatement[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170000CE RID: 206
		public CodeStatement this[int index]
		{
			get
			{
				return (CodeStatement)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600043C RID: 1084 RVA: 0x0001475A File Offset: 0x0001375A
		public int Add(CodeStatement value)
		{
			return base.List.Add(value);
		}

		// Token: 0x0600043D RID: 1085 RVA: 0x00014768 File Offset: 0x00013768
		public int Add(CodeExpression value)
		{
			return this.Add(new CodeExpressionStatement(value));
		}

		// Token: 0x0600043E RID: 1086 RVA: 0x00014778 File Offset: 0x00013778
		public void AddRange(CodeStatement[] value)
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

		// Token: 0x0600043F RID: 1087 RVA: 0x000147AC File Offset: 0x000137AC
		public void AddRange(CodeStatementCollection value)
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

		// Token: 0x06000440 RID: 1088 RVA: 0x000147E8 File Offset: 0x000137E8
		public bool Contains(CodeStatement value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000441 RID: 1089 RVA: 0x000147F6 File Offset: 0x000137F6
		public void CopyTo(CodeStatement[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000442 RID: 1090 RVA: 0x00014805 File Offset: 0x00013805
		public int IndexOf(CodeStatement value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000443 RID: 1091 RVA: 0x00014813 File Offset: 0x00013813
		public void Insert(int index, CodeStatement value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000444 RID: 1092 RVA: 0x00014822 File Offset: 0x00013822
		public void Remove(CodeStatement value)
		{
			base.List.Remove(value);
		}
	}
}
