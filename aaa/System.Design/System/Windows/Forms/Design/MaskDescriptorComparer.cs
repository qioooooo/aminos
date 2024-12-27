using System;
using System.Collections.Generic;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x0200026A RID: 618
	internal class MaskDescriptorComparer : IComparer<MaskDescriptor>
	{
		// Token: 0x06001754 RID: 5972 RVA: 0x00078372 File Offset: 0x00077372
		public MaskDescriptorComparer(MaskDescriptorComparer.SortType sortType, SortOrder sortOrder)
		{
			this.sortType = sortType;
			this.sortOrder = sortOrder;
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x00078388 File Offset: 0x00077388
		public int Compare(MaskDescriptor maskDescriptorA, MaskDescriptor maskDescriptorB)
		{
			if (maskDescriptorA == null || maskDescriptorB == null)
			{
				return 0;
			}
			string text;
			string text2;
			switch (this.sortType)
			{
			default:
				text = maskDescriptorA.Name;
				text2 = maskDescriptorB.Name;
				break;
			case MaskDescriptorComparer.SortType.BySample:
				text = maskDescriptorA.Sample;
				text2 = maskDescriptorB.Sample;
				break;
			case MaskDescriptorComparer.SortType.ByValidatingTypeName:
				text = ((maskDescriptorA.ValidatingType == null) ? SR.GetString("MaskDescriptorValidatingTypeNone") : maskDescriptorA.ValidatingType.Name);
				text2 = ((maskDescriptorB.ValidatingType == null) ? SR.GetString("MaskDescriptorValidatingTypeNone") : maskDescriptorB.ValidatingType.Name);
				break;
			}
			int num = string.Compare(text, text2);
			if (this.sortOrder != SortOrder.Descending)
			{
				return num;
			}
			return -num;
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x0007842B File Offset: 0x0007742B
		public int GetHashCode(MaskDescriptor maskDescriptor)
		{
			if (maskDescriptor != null)
			{
				return maskDescriptor.GetHashCode();
			}
			return 0;
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x00078438 File Offset: 0x00077438
		public bool Equals(MaskDescriptor maskDescriptorA, MaskDescriptor maskDescriptorB)
		{
			if (!MaskDescriptor.IsValidMaskDescriptor(maskDescriptorA) || !MaskDescriptor.IsValidMaskDescriptor(maskDescriptorB))
			{
				return maskDescriptorA == maskDescriptorB;
			}
			return maskDescriptorA.Equals(maskDescriptorB);
		}

		// Token: 0x04001325 RID: 4901
		private SortOrder sortOrder;

		// Token: 0x04001326 RID: 4902
		private MaskDescriptorComparer.SortType sortType;

		// Token: 0x0200026B RID: 619
		public enum SortType
		{
			// Token: 0x04001328 RID: 4904
			ByName,
			// Token: 0x04001329 RID: 4905
			BySample,
			// Token: 0x0400132A RID: 4906
			ByValidatingTypeName
		}
	}
}
