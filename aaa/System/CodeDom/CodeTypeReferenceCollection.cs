using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.CodeDom
{
	// Token: 0x02000085 RID: 133
	[ComVisible(true)]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[Serializable]
	public class CodeTypeReferenceCollection : CollectionBase
	{
		// Token: 0x060004B6 RID: 1206 RVA: 0x00015639 File Offset: 0x00014639
		public CodeTypeReferenceCollection()
		{
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00015641 File Offset: 0x00014641
		public CodeTypeReferenceCollection(CodeTypeReferenceCollection value)
		{
			this.AddRange(value);
		}

		// Token: 0x060004B8 RID: 1208 RVA: 0x00015650 File Offset: 0x00014650
		public CodeTypeReferenceCollection(CodeTypeReference[] value)
		{
			this.AddRange(value);
		}

		// Token: 0x170000EC RID: 236
		public CodeTypeReference this[int index]
		{
			get
			{
				return (CodeTypeReference)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x060004BB RID: 1211 RVA: 0x00015681 File Offset: 0x00014681
		public int Add(CodeTypeReference value)
		{
			return base.List.Add(value);
		}

		// Token: 0x060004BC RID: 1212 RVA: 0x0001568F File Offset: 0x0001468F
		public void Add(string value)
		{
			this.Add(new CodeTypeReference(value));
		}

		// Token: 0x060004BD RID: 1213 RVA: 0x0001569E File Offset: 0x0001469E
		public void Add(Type value)
		{
			this.Add(new CodeTypeReference(value));
		}

		// Token: 0x060004BE RID: 1214 RVA: 0x000156B0 File Offset: 0x000146B0
		public void AddRange(CodeTypeReference[] value)
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

		// Token: 0x060004BF RID: 1215 RVA: 0x000156E4 File Offset: 0x000146E4
		public void AddRange(CodeTypeReferenceCollection value)
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

		// Token: 0x060004C0 RID: 1216 RVA: 0x00015720 File Offset: 0x00014720
		public bool Contains(CodeTypeReference value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060004C1 RID: 1217 RVA: 0x0001572E File Offset: 0x0001472E
		public void CopyTo(CodeTypeReference[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060004C2 RID: 1218 RVA: 0x0001573D File Offset: 0x0001473D
		public int IndexOf(CodeTypeReference value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060004C3 RID: 1219 RVA: 0x0001574B File Offset: 0x0001474B
		public void Insert(int index, CodeTypeReference value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x060004C4 RID: 1220 RVA: 0x0001575A File Offset: 0x0001475A
		public void Remove(CodeTypeReference value)
		{
			base.List.Remove(value);
		}
	}
}
