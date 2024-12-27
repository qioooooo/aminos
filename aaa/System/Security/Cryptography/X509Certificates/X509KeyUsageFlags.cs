using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x0200033B RID: 827
	[Flags]
	public enum X509KeyUsageFlags
	{
		// Token: 0x04001B09 RID: 6921
		None = 0,
		// Token: 0x04001B0A RID: 6922
		EncipherOnly = 1,
		// Token: 0x04001B0B RID: 6923
		CrlSign = 2,
		// Token: 0x04001B0C RID: 6924
		KeyCertSign = 4,
		// Token: 0x04001B0D RID: 6925
		KeyAgreement = 8,
		// Token: 0x04001B0E RID: 6926
		DataEncipherment = 16,
		// Token: 0x04001B0F RID: 6927
		KeyEncipherment = 32,
		// Token: 0x04001B10 RID: 6928
		NonRepudiation = 64,
		// Token: 0x04001B11 RID: 6929
		DigitalSignature = 128,
		// Token: 0x04001B12 RID: 6930
		DecipherOnly = 32768
	}
}
