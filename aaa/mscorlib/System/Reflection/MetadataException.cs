using System;
using System.Globalization;

namespace System.Reflection
{
	// Token: 0x02000313 RID: 787
	internal class MetadataException : Exception
	{
		// Token: 0x06001EB3 RID: 7859 RVA: 0x0004D9DA File Offset: 0x0004C9DA
		internal MetadataException(int hr)
		{
			this.m_hr = hr;
		}

		// Token: 0x06001EB4 RID: 7860 RVA: 0x0004D9EC File Offset: 0x0004C9EC
		public override string ToString()
		{
			return string.Format(CultureInfo.CurrentCulture, "MetadataException HResult = {0:x}.", new object[] { this.m_hr });
		}

		// Token: 0x04000CE2 RID: 3298
		private int m_hr;
	}
}
