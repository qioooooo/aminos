using System;
using System.ComponentModel;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000021 RID: 33
	public class SearchOptionsControl : DirectoryControl
	{
		// Token: 0x0600008C RID: 140 RVA: 0x0000440D File Offset: 0x0000340D
		public SearchOptionsControl()
			: base("1.2.840.113556.1.4.1340", null, true, true)
		{
		}

		// Token: 0x0600008D RID: 141 RVA: 0x00004424 File Offset: 0x00003424
		public SearchOptionsControl(SearchOption flags)
			: this()
		{
			this.SearchOption = flags;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x0600008E RID: 142 RVA: 0x00004433 File Offset: 0x00003433
		// (set) Token: 0x0600008F RID: 143 RVA: 0x0000443B File Offset: 0x0000343B
		public SearchOption SearchOption
		{
			get
			{
				return this.flag;
			}
			set
			{
				if (value < SearchOption.DomainScope || value > SearchOption.PhantomRoot)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SearchOption));
				}
				this.flag = value;
			}
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00004464 File Offset: 0x00003464
		public override byte[] GetValue()
		{
			this.directoryControlValue = BerConverter.Encode("{i}", new object[] { (int)this.flag });
			return base.GetValue();
		}

		// Token: 0x040000E6 RID: 230
		private SearchOption flag = SearchOption.DomainScope;
	}
}
