using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x0200003E RID: 62
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeAttributeArgumentCollection : CollectionBase
	{
		// Token: 0x0600028B RID: 651 RVA: 0x00012774 File Offset: 0x00011774
		public CodeAttributeArgumentCollection()
		{
		}

		// Token: 0x0600028C RID: 652 RVA: 0x0001277C File Offset: 0x0001177C
		public CodeAttributeArgumentCollection(CodeAttributeArgumentCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x0001278B File Offset: 0x0001178B
		public CodeAttributeArgumentCollection(CodeAttributeArgument[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000052 RID: 82
		public CodeAttributeArgument this[int index]
		{
			get
			{
				return (CodeAttributeArgument)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000127BC File Offset: 0x000117BC
		public int Add(CodeAttributeArgument value)
		{
			return base.List.Add(value);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000127CC File Offset: 0x000117CC
		public void AddRange(CodeAttributeArgument[] value)
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

		// Token: 0x06000292 RID: 658 RVA: 0x00012800 File Offset: 0x00011800
		public void AddRange(CodeAttributeArgumentCollection value)
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

		// Token: 0x06000293 RID: 659 RVA: 0x0001283C File Offset: 0x0001183C
		public bool Contains(CodeAttributeArgument value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0001284A File Offset: 0x0001184A
		public void CopyTo(CodeAttributeArgument[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000295 RID: 661 RVA: 0x00012859 File Offset: 0x00011859
		public int IndexOf(CodeAttributeArgument value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00012867 File Offset: 0x00011867
		public void Insert(int index, CodeAttributeArgument value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00012876 File Offset: 0x00011876
		public void Remove(CodeAttributeArgument value)
		{
			base.List.Remove(value);
		}
	}
}
