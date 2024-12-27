using System;
using System.Runtime.InteropServices;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x020008AA RID: 2218
	[ComVisible(true)]
	public enum X509ContentType
	{
		// Token: 0x040029BA RID: 10682
		Unknown,
		// Token: 0x040029BB RID: 10683
		Cert,
		// Token: 0x040029BC RID: 10684
		SerializedCert,
		// Token: 0x040029BD RID: 10685
		Pfx,
		// Token: 0x040029BE RID: 10686
		Pkcs12 = 3,
		// Token: 0x040029BF RID: 10687
		SerializedStore,
		// Token: 0x040029C0 RID: 10688
		Pkcs7,
		// Token: 0x040029C1 RID: 10689
		Authenticode
	}
}
