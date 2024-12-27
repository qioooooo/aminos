using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000055 RID: 85
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[Serializable]
	public class CodeDirectiveCollection : CollectionBase
	{
		// Token: 0x0600033C RID: 828 RVA: 0x000134DA File Offset: 0x000124DA
		public CodeDirectiveCollection()
		{
		}

		// Token: 0x0600033D RID: 829 RVA: 0x000134E2 File Offset: 0x000124E2
		public CodeDirectiveCollection(CodeDirectiveCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x0600033E RID: 830 RVA: 0x000134F1 File Offset: 0x000124F1
		public CodeDirectiveCollection(CodeDirective[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000087 RID: 135
		public CodeDirective this[int index]
		{
			get
			{
				return (CodeDirective)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x00013522 File Offset: 0x00012522
		public int Add(CodeDirective value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000342 RID: 834 RVA: 0x00013530 File Offset: 0x00012530
		public void AddRange(CodeDirective[] value)
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

		// Token: 0x06000343 RID: 835 RVA: 0x00013564 File Offset: 0x00012564
		public void AddRange(CodeDirectiveCollection value)
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

		// Token: 0x06000344 RID: 836 RVA: 0x000135A0 File Offset: 0x000125A0
		public bool Contains(CodeDirective value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000345 RID: 837 RVA: 0x000135AE File Offset: 0x000125AE
		public void CopyTo(CodeDirective[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x000135BD File Offset: 0x000125BD
		public int IndexOf(CodeDirective value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x000135CB File Offset: 0x000125CB
		public void Insert(int index, CodeDirective value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000348 RID: 840 RVA: 0x000135DA File Offset: 0x000125DA
		public void Remove(CodeDirective value)
		{
			base.List.Remove(value);
		}
	}
}
