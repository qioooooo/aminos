using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200007D RID: 125
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeTypeDeclarationCollection : CollectionBase
	{
		// Token: 0x06000466 RID: 1126 RVA: 0x00014BCD File Offset: 0x00013BCD
		public CodeTypeDeclarationCollection()
		{
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00014BD5 File Offset: 0x00013BD5
		public CodeTypeDeclarationCollection(CodeTypeDeclarationCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00014BE4 File Offset: 0x00013BE4
		public CodeTypeDeclarationCollection(CodeTypeDeclaration[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170000DC RID: 220
		public CodeTypeDeclaration this[int index]
		{
			get
			{
				return (CodeTypeDeclaration)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x00014C15 File Offset: 0x00013C15
		public int Add(CodeTypeDeclaration value)
		{
			return base.List.Add(value);
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00014C24 File Offset: 0x00013C24
		public void AddRange(CodeTypeDeclaration[] value)
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

		// Token: 0x0600046D RID: 1133 RVA: 0x00014C58 File Offset: 0x00013C58
		public void AddRange(CodeTypeDeclarationCollection value)
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

		// Token: 0x0600046E RID: 1134 RVA: 0x00014C94 File Offset: 0x00013C94
		public bool Contains(CodeTypeDeclaration value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x00014CA2 File Offset: 0x00013CA2
		public void CopyTo(CodeTypeDeclaration[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000470 RID: 1136 RVA: 0x00014CB1 File Offset: 0x00013CB1
		public int IndexOf(CodeTypeDeclaration value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x00014CBF File Offset: 0x00013CBF
		public void Insert(int index, CodeTypeDeclaration value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00014CCE File Offset: 0x00013CCE
		public void Remove(CodeTypeDeclaration value)
		{
			base.List.Remove(value);
		}
	}
}
