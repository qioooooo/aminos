using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000067 RID: 103
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeNamespaceCollection : CollectionBase
	{
		// Token: 0x060003C1 RID: 961 RVA: 0x00013EB8 File Offset: 0x00012EB8
		public CodeNamespaceCollection()
		{
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x00013EC0 File Offset: 0x00012EC0
		public CodeNamespaceCollection(CodeNamespaceCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00013ECF File Offset: 0x00012ECF
		public CodeNamespaceCollection(CodeNamespace[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170000B0 RID: 176
		public CodeNamespace this[int index]
		{
			get
			{
				return (CodeNamespace)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00013F00 File Offset: 0x00012F00
		public int Add(CodeNamespace value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x00013F10 File Offset: 0x00012F10
		public void AddRange(CodeNamespace[] value)
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

		// Token: 0x060003C8 RID: 968 RVA: 0x00013F44 File Offset: 0x00012F44
		public void AddRange(CodeNamespaceCollection value)
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

		// Token: 0x060003C9 RID: 969 RVA: 0x00013F80 File Offset: 0x00012F80
		public bool Contains(CodeNamespace value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00013F8E File Offset: 0x00012F8E
		public void CopyTo(CodeNamespace[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00013F9D File Offset: 0x00012F9D
		public int IndexOf(CodeNamespace value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x00013FAB File Offset: 0x00012FAB
		public void Insert(int index, CodeNamespace value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x00013FBA File Offset: 0x00012FBA
		public void Remove(CodeNamespace value)
		{
			base.List.Remove(value);
		}
	}
}
