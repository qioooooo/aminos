using System;

namespace System.ComponentModel
{
	// Token: 0x02000140 RID: 320
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	[Serializable]
	public sealed class ToolboxItemFilterAttribute : Attribute
	{
		// Token: 0x06000A61 RID: 2657 RVA: 0x000240B6 File Offset: 0x000230B6
		public ToolboxItemFilterAttribute(string filterString)
			: this(filterString, ToolboxItemFilterType.Allow)
		{
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x000240C0 File Offset: 0x000230C0
		public ToolboxItemFilterAttribute(string filterString, ToolboxItemFilterType filterType)
		{
			if (filterString == null)
			{
				filterString = string.Empty;
			}
			this.filterString = filterString;
			this.filterType = filterType;
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000A63 RID: 2659 RVA: 0x000240E0 File Offset: 0x000230E0
		public string FilterString
		{
			get
			{
				return this.filterString;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x06000A64 RID: 2660 RVA: 0x000240E8 File Offset: 0x000230E8
		public ToolboxItemFilterType FilterType
		{
			get
			{
				return this.filterType;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x06000A65 RID: 2661 RVA: 0x000240F0 File Offset: 0x000230F0
		public override object TypeId
		{
			get
			{
				if (this.typeId == null)
				{
					this.typeId = base.GetType().FullName + this.filterString;
				}
				return this.typeId;
			}
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x0002411C File Offset: 0x0002311C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ToolboxItemFilterAttribute toolboxItemFilterAttribute = obj as ToolboxItemFilterAttribute;
			return toolboxItemFilterAttribute != null && toolboxItemFilterAttribute.FilterType.Equals(this.FilterType) && toolboxItemFilterAttribute.FilterString.Equals(this.FilterString);
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x00024169 File Offset: 0x00023169
		public override int GetHashCode()
		{
			return this.filterString.GetHashCode();
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00024178 File Offset: 0x00023178
		public override bool Match(object obj)
		{
			ToolboxItemFilterAttribute toolboxItemFilterAttribute = obj as ToolboxItemFilterAttribute;
			return toolboxItemFilterAttribute != null && toolboxItemFilterAttribute.FilterString.Equals(this.FilterString);
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x000241A7 File Offset: 0x000231A7
		public override string ToString()
		{
			return this.filterString + "," + Enum.GetName(typeof(ToolboxItemFilterType), this.filterType);
		}

		// Token: 0x04000A6C RID: 2668
		private ToolboxItemFilterType filterType;

		// Token: 0x04000A6D RID: 2669
		private string filterString;

		// Token: 0x04000A6E RID: 2670
		private string typeId;
	}
}
