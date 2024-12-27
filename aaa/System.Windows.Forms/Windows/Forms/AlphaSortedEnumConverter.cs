using System;
using System.Collections;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020001DA RID: 474
	internal class AlphaSortedEnumConverter : EnumConverter
	{
		// Token: 0x0600127F RID: 4735 RVA: 0x00010E24 File Offset: 0x0000FE24
		public AlphaSortedEnumConverter(Type type)
			: base(type)
		{
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06001280 RID: 4736 RVA: 0x00010E2D File Offset: 0x0000FE2D
		protected override IComparer Comparer
		{
			get
			{
				return EnumValAlphaComparer.Default;
			}
		}
	}
}
