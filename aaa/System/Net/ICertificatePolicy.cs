using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x020003E9 RID: 1001
	public interface ICertificatePolicy
	{
		// Token: 0x06002073 RID: 8307
		bool CheckValidationResult(ServicePoint srvPoint, X509Certificate certificate, WebRequest request, int certificateProblem);
	}
}
