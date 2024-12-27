using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000438 RID: 1080
	internal class CertPolicyValidationCallback
	{
		// Token: 0x060021B6 RID: 8630 RVA: 0x00085811 File Offset: 0x00084811
		internal CertPolicyValidationCallback()
		{
			this.m_CertificatePolicy = new DefaultCertPolicy();
			this.m_Context = null;
		}

		// Token: 0x060021B7 RID: 8631 RVA: 0x0008582B File Offset: 0x0008482B
		internal CertPolicyValidationCallback(ICertificatePolicy certificatePolicy)
		{
			this.m_CertificatePolicy = certificatePolicy;
			this.m_Context = ExecutionContext.Capture();
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060021B8 RID: 8632 RVA: 0x00085845 File Offset: 0x00084845
		internal ICertificatePolicy CertificatePolicy
		{
			get
			{
				return this.m_CertificatePolicy;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060021B9 RID: 8633 RVA: 0x0008584D File Offset: 0x0008484D
		internal bool UsesDefault
		{
			get
			{
				return this.m_Context == null;
			}
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x00085858 File Offset: 0x00084858
		internal void Callback(object state)
		{
			CertPolicyValidationCallback.CallbackContext callbackContext = (CertPolicyValidationCallback.CallbackContext)state;
			callbackContext.result = callbackContext.policyWrapper.CheckErrors(callbackContext.hostName, callbackContext.certificate, callbackContext.chain, callbackContext.sslPolicyErrors);
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x00085898 File Offset: 0x00084898
		internal bool Invoke(string hostName, ServicePoint servicePoint, X509Certificate certificate, WebRequest request, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			PolicyWrapper policyWrapper = new PolicyWrapper(this.m_CertificatePolicy, servicePoint, request);
			if (this.m_Context == null)
			{
				return policyWrapper.CheckErrors(hostName, certificate, chain, sslPolicyErrors);
			}
			ExecutionContext executionContext = this.m_Context.CreateCopy();
			CertPolicyValidationCallback.CallbackContext callbackContext = new CertPolicyValidationCallback.CallbackContext(policyWrapper, hostName, certificate, chain, sslPolicyErrors);
			ExecutionContext.Run(executionContext, new ContextCallback(this.Callback), callbackContext);
			return callbackContext.result;
		}

		// Token: 0x040021C1 RID: 8641
		private ICertificatePolicy m_CertificatePolicy;

		// Token: 0x040021C2 RID: 8642
		private ExecutionContext m_Context;

		// Token: 0x02000439 RID: 1081
		private class CallbackContext
		{
			// Token: 0x060021BC RID: 8636 RVA: 0x000858FB File Offset: 0x000848FB
			internal CallbackContext(PolicyWrapper policyWrapper, string hostName, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{
				this.policyWrapper = policyWrapper;
				this.hostName = hostName;
				this.certificate = certificate;
				this.chain = chain;
				this.sslPolicyErrors = sslPolicyErrors;
			}

			// Token: 0x040021C3 RID: 8643
			internal readonly PolicyWrapper policyWrapper;

			// Token: 0x040021C4 RID: 8644
			internal readonly string hostName;

			// Token: 0x040021C5 RID: 8645
			internal readonly X509Certificate certificate;

			// Token: 0x040021C6 RID: 8646
			internal readonly X509Chain chain;

			// Token: 0x040021C7 RID: 8647
			internal readonly SslPolicyErrors sslPolicyErrors;

			// Token: 0x040021C8 RID: 8648
			internal bool result;
		}
	}
}
