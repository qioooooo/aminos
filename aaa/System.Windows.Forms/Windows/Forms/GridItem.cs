using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000418 RID: 1048
	public abstract class GridItem
	{
		// Token: 0x17000BEF RID: 3055
		// (get) Token: 0x06003E5E RID: 15966 RVA: 0x000E3307 File Offset: 0x000E2307
		// (set) Token: 0x06003E5F RID: 15967 RVA: 0x000E330F File Offset: 0x000E230F
		[SRDescription("ControlTagDescr")]
		[Localizable(false)]
		[DefaultValue(null)]
		[Bindable(true)]
		[TypeConverter(typeof(StringConverter))]
		[SRCategory("CatData")]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x17000BF0 RID: 3056
		// (get) Token: 0x06003E60 RID: 15968
		public abstract GridItemCollection GridItems { get; }

		// Token: 0x17000BF1 RID: 3057
		// (get) Token: 0x06003E61 RID: 15969
		public abstract GridItemType GridItemType { get; }

		// Token: 0x17000BF2 RID: 3058
		// (get) Token: 0x06003E62 RID: 15970
		public abstract string Label { get; }

		// Token: 0x17000BF3 RID: 3059
		// (get) Token: 0x06003E63 RID: 15971
		public abstract GridItem Parent { get; }

		// Token: 0x17000BF4 RID: 3060
		// (get) Token: 0x06003E64 RID: 15972
		public abstract PropertyDescriptor PropertyDescriptor { get; }

		// Token: 0x17000BF5 RID: 3061
		// (get) Token: 0x06003E65 RID: 15973
		public abstract object Value { get; }

		// Token: 0x17000BF6 RID: 3062
		// (get) Token: 0x06003E66 RID: 15974 RVA: 0x000E3318 File Offset: 0x000E2318
		public virtual bool Expandable
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000BF7 RID: 3063
		// (get) Token: 0x06003E67 RID: 15975 RVA: 0x000E331B File Offset: 0x000E231B
		// (set) Token: 0x06003E68 RID: 15976 RVA: 0x000E331E File Offset: 0x000E231E
		public virtual bool Expanded
		{
			get
			{
				return false;
			}
			set
			{
				throw new NotSupportedException(SR.GetString("GridItemNotExpandable"));
			}
		}

		// Token: 0x06003E69 RID: 15977
		public abstract bool Select();

		// Token: 0x04001EC2 RID: 7874
		private object userData;
	}
}
