using System;

namespace System.Security.Cryptography.X509Certificates
{
	// Token: 0x02000328 RID: 808
	public enum X509FindType
	{
		// Token: 0x04001AAB RID: 6827
		FindByThumbprint,
		// Token: 0x04001AAC RID: 6828
		FindBySubjectName,
		// Token: 0x04001AAD RID: 6829
		FindBySubjectDistinguishedName,
		// Token: 0x04001AAE RID: 6830
		FindByIssuerName,
		// Token: 0x04001AAF RID: 6831
		FindByIssuerDistinguishedName,
		// Token: 0x04001AB0 RID: 6832
		FindBySerialNumber,
		// Token: 0x04001AB1 RID: 6833
		FindByTimeValid,
		// Token: 0x04001AB2 RID: 6834
		FindByTimeNotYetValid,
		// Token: 0x04001AB3 RID: 6835
		FindByTimeExpired,
		// Token: 0x04001AB4 RID: 6836
		FindByTemplateName,
		// Token: 0x04001AB5 RID: 6837
		FindByApplicationPolicy,
		// Token: 0x04001AB6 RID: 6838
		FindByCertificatePolicy,
		// Token: 0x04001AB7 RID: 6839
		FindByExtension,
		// Token: 0x04001AB8 RID: 6840
		FindByKeyUsage,
		// Token: 0x04001AB9 RID: 6841
		FindBySubjectKeyIdentifier
	}
}
