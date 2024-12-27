using System;

namespace System.Web.Services.Description
{
	// Token: 0x020000D5 RID: 213
	internal class MimeTextReturn : MimeReturn
	{
		// Token: 0x1700019E RID: 414
		// (get) Token: 0x060005B2 RID: 1458 RVA: 0x0001B76E File Offset: 0x0001A76E
		// (set) Token: 0x060005B3 RID: 1459 RVA: 0x0001B776 File Offset: 0x0001A776
		internal MimeTextBinding TextBinding
		{
			get
			{
				return this.textBinding;
			}
			set
			{
				this.textBinding = value;
			}
		}

		// Token: 0x0400042D RID: 1069
		private MimeTextBinding textBinding;
	}
}
