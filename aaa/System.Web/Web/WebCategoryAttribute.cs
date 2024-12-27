using System;
using System.ComponentModel;

namespace System.Web
{
	// Token: 0x020000EA RID: 234
	[AttributeUsage(AttributeTargets.All)]
	internal sealed class WebCategoryAttribute : CategoryAttribute
	{
		// Token: 0x06000B05 RID: 2821 RVA: 0x0002C1EE File Offset: 0x0002B1EE
		internal WebCategoryAttribute(string category)
			: base(category)
		{
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x0002C1F7 File Offset: 0x0002B1F7
		public override object TypeId
		{
			get
			{
				return typeof(CategoryAttribute);
			}
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x0002C204 File Offset: 0x0002B204
		protected override string GetLocalizedString(string value)
		{
			string text = base.GetLocalizedString(value);
			if (text == null)
			{
				text = SR.GetString("Category_" + value);
			}
			return text;
		}
	}
}
