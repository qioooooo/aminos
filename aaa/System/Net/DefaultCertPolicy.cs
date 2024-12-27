using System;
using System.Security.Cryptography.X509Certificates;

namespace System.Net
{
	// Token: 0x0200041D RID: 1053
	internal class DefaultCertPolicy : ICertificatePolicy
	{
		// Token: 0x060020E5 RID: 8421 RVA: 0x0008161F File Offset: 0x0008061F
		public bool CheckValidationResult(ServicePoint sp, X509Certificate cert, WebRequest request, int problem)
		{
			return problem == 0;
		}
	}
}
