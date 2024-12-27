using System;

namespace System.Xml.Serialization
{
	// Token: 0x020002C8 RID: 712
	internal class ArrayMapping : TypeMapping
	{
		// Token: 0x1700081E RID: 2078
		// (get) Token: 0x060021BD RID: 8637 RVA: 0x0009F293 File Offset: 0x0009E293
		// (set) Token: 0x060021BE RID: 8638 RVA: 0x0009F29B File Offset: 0x0009E29B
		internal ElementAccessor[] Elements
		{
			get
			{
				return this.elements;
			}
			set
			{
				this.elements = value;
				this.sortedElements = null;
			}
		}

		// Token: 0x1700081F RID: 2079
		// (get) Token: 0x060021BF RID: 8639 RVA: 0x0009F2AC File Offset: 0x0009E2AC
		internal ElementAccessor[] ElementsSortedByDerivation
		{
			get
			{
				if (this.sortedElements != null)
				{
					return this.sortedElements;
				}
				if (this.elements == null)
				{
					return null;
				}
				this.sortedElements = new ElementAccessor[this.elements.Length];
				Array.Copy(this.elements, 0, this.sortedElements, 0, this.elements.Length);
				AccessorMapping.SortMostToLeastDerived(this.sortedElements);
				return this.sortedElements;
			}
		}

		// Token: 0x17000820 RID: 2080
		// (get) Token: 0x060021C0 RID: 8640 RVA: 0x0009F311 File Offset: 0x0009E311
		// (set) Token: 0x060021C1 RID: 8641 RVA: 0x0009F319 File Offset: 0x0009E319
		internal ArrayMapping Next
		{
			get
			{
				return this.next;
			}
			set
			{
				this.next = value;
			}
		}

		// Token: 0x17000821 RID: 2081
		// (get) Token: 0x060021C2 RID: 8642 RVA: 0x0009F322 File Offset: 0x0009E322
		// (set) Token: 0x060021C3 RID: 8643 RVA: 0x0009F32A File Offset: 0x0009E32A
		internal StructMapping TopLevelMapping
		{
			get
			{
				return this.topLevelMapping;
			}
			set
			{
				this.topLevelMapping = value;
			}
		}

		// Token: 0x04001477 RID: 5239
		private ElementAccessor[] elements;

		// Token: 0x04001478 RID: 5240
		private ElementAccessor[] sortedElements;

		// Token: 0x04001479 RID: 5241
		private ArrayMapping next;

		// Token: 0x0400147A RID: 5242
		private StructMapping topLevelMapping;
	}
}
