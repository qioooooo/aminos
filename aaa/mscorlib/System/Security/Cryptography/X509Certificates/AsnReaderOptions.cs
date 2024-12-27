using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008B5 RID: 2229
	internal struct AsnReaderOptions
	{
		// Token: 0x17000E34 RID: 3636
		// (get) Token: 0x060051D4 RID: 20948 RVA: 0x00126BE3 File Offset: 0x00125BE3
		// (set) Token: 0x060051D5 RID: 20949 RVA: 0x00126BF9 File Offset: 0x00125BF9
		public int UtcTimeTwoDigitYearMax
		{
			get
			{
				if (this._twoDigitYearMax == 0)
				{
					return 2049;
				}
				return (int)this._twoDigitYearMax;
			}
			set
			{
				if (value < 1 || value > 9999)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this._twoDigitYearMax = (ushort)value;
			}
		}

		// Token: 0x17000E35 RID: 3637
		// (get) Token: 0x060051D6 RID: 20950 RVA: 0x00126C1A File Offset: 0x00125C1A
		// (set) Token: 0x060051D7 RID: 20951 RVA: 0x00126C22 File Offset: 0x00125C22
		public bool SkipSetSortOrderVerification
		{
			get
			{
				return this._skipSetSortOrderVerification;
			}
			set
			{
				this._skipSetSortOrderVerification = value;
			}
		}

		// Token: 0x04002A01 RID: 10753
		private const int DefaultTwoDigitMax = 2049;

		// Token: 0x04002A02 RID: 10754
		private ushort _twoDigitYearMax;

		// Token: 0x04002A03 RID: 10755
		private bool _skipSetSortOrderVerification;
	}
}
