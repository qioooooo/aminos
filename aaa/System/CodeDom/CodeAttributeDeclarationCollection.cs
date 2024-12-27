using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000040 RID: 64
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeAttributeDeclarationCollection : CollectionBase
	{
		// Token: 0x060002A1 RID: 673 RVA: 0x00012959 File Offset: 0x00011959
		public CodeAttributeDeclarationCollection()
		{
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x00012961 File Offset: 0x00011961
		public CodeAttributeDeclarationCollection(CodeAttributeDeclarationCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x00012970 File Offset: 0x00011970
		public CodeAttributeDeclarationCollection(CodeAttributeDeclaration[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x17000056 RID: 86
		public CodeAttributeDeclaration this[int index]
		{
			get
			{
				return (CodeAttributeDeclaration)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060002A6 RID: 678 RVA: 0x000129A1 File Offset: 0x000119A1
		public int Add(CodeAttributeDeclaration value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060002A7 RID: 679 RVA: 0x000129B0 File Offset: 0x000119B0
		public void AddRange(CodeAttributeDeclaration[] value)
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

		// Token: 0x060002A8 RID: 680 RVA: 0x000129E4 File Offset: 0x000119E4
		public void AddRange(CodeAttributeDeclarationCollection value)
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

		// Token: 0x060002A9 RID: 681 RVA: 0x00012A20 File Offset: 0x00011A20
		public bool Contains(CodeAttributeDeclaration value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060002AA RID: 682 RVA: 0x00012A2E File Offset: 0x00011A2E
		public void CopyTo(CodeAttributeDeclaration[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060002AB RID: 683 RVA: 0x00012A3D File Offset: 0x00011A3D
		public int IndexOf(CodeAttributeDeclaration value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x00012A4B File Offset: 0x00011A4B
		public void Insert(int index, CodeAttributeDeclaration value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060002AD RID: 685 RVA: 0x00012A5A File Offset: 0x00011A5A
		public void Remove(CodeAttributeDeclaration value)
		{
			base.List.Remove(value);
		}
	}
}
