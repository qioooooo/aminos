using System;
using System.Collections.Generic;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class MaskDescriptorComparer : IComparer<MaskDescriptor>
	{
		public MaskDescriptorComparer(MaskDescriptorComparer.SortType sortType, SortOrder sortOrder)
		{
			this.sortType = sortType;
			this.sortOrder = sortOrder;
		}

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

		public int GetHashCode(MaskDescriptor maskDescriptor)
		{
			if (maskDescriptor != null)
			{
				return maskDescriptor.GetHashCode();
			}
			return 0;
		}

		public bool Equals(MaskDescriptor maskDescriptorA, MaskDescriptor maskDescriptorB)
		{
			if (!MaskDescriptor.IsValidMaskDescriptor(maskDescriptorA) || !MaskDescriptor.IsValidMaskDescriptor(maskDescriptorB))
			{
				return maskDescriptorA == maskDescriptorB;
			}
			return maskDescriptorA.Equals(maskDescriptorB);
		}

		private SortOrder sortOrder;

		private MaskDescriptorComparer.SortType sortType;

		public enum SortType
		{
			ByName,
			BySample,
			ByValidatingTypeName
		}
	}
}
