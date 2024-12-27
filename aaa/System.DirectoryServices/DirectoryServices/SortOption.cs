using System;
using System.ComponentModel;

namespace System.DirectoryServices
{
	// Token: 0x02000045 RID: 69
	[TypeConverter(typeof(ExpandableObjectConverter))]
	public class SortOption
	{
		// Token: 0x060001DE RID: 478 RVA: 0x00007C6C File Offset: 0x00006C6C
		public SortOption()
		{
		}

		// Token: 0x060001DF RID: 479 RVA: 0x00007C74 File Offset: 0x00006C74
		public SortOption(string propertyName, SortDirection direction)
		{
			this.PropertyName = propertyName;
			this.Direction = this.sortDirection;
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x00007C8F File Offset: 0x00006C8F
		// (set) Token: 0x060001E1 RID: 481 RVA: 0x00007C97 File Offset: 0x00006C97
		[DefaultValue(null)]
		[DSDescription("DSSortName")]
		public string PropertyName
		{
			get
			{
				return this.propertyName;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.propertyName = value;
			}
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x00007CAE File Offset: 0x00006CAE
		// (set) Token: 0x060001E3 RID: 483 RVA: 0x00007CB6 File Offset: 0x00006CB6
		[DefaultValue(SortDirection.Ascending)]
		[DSDescription("DSSortDirection")]
		public SortDirection Direction
		{
			get
			{
				return this.sortDirection;
			}
			set
			{
				if (value < SortDirection.Ascending || value > SortDirection.Descending)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SortDirection));
				}
				this.sortDirection = value;
			}
		}

		// Token: 0x04000200 RID: 512
		private string propertyName;

		// Token: 0x04000201 RID: 513
		private SortDirection sortDirection;
	}
}
