using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading;

namespace System.Net
{
	// Token: 0x0200043A RID: 1082
	internal class ServerCertValidationCallback
	{
		// Token: 0x060021BD RID: 8637 RVA: 0x00085928 File Offset: 0x00084928
		internal ServerCertValidationCallback(RemoteCertificateValidationCallback validationCallback)
		{
			this.m_ValidationCallback = validationCallback;
			this.m_Context = ExecutionContext.Capture();
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060021BE RID: 8638 RVA: 0x00085942 File Offset: 0x00084942
		internal RemoteCertificateValidationCallback ValidationCallback
		{
			get
			{
				return this.m_ValidationCallback;
			}
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x0008594C File Offset: 0x0008494C
		internal void Callback(object state)
		{
			ServerCertValidationCallback.CallbackContext callbackContext = (ServerCertValidationCallback.CallbackContext)state;
			callbackContext.result = this.m_ValidationCallback(callbackContext.request, callbackContext.certificate, callbackContext.chain, callbackContext.sslPolicyErrors);
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x0008598C File Offset: 0x0008498C
		internal bool Invoke(object request, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
		{
			if (this.m_Context == null)
			{
				return this.m_ValidationCallback(request, certificate, chain, sslPolicyErrors);
			}
			ExecutionContext executionContext = this.m_Context.CreateCopy();
			ServerCertValidationCallback.CallbackContext callbackContext = new ServerCertValidationCallback.CallbackContext(request, certificate, chain, sslPolicyErrors);
			ExecutionContext.Run(executionContext, new ContextCallback(this.Callback), callbackContext);
			return callbackContext.result;
		}

		// Token: 0x040021C9 RID: 8649
		private RemoteCertificateValidationCallback m_ValidationCallback;

		// Token: 0x040021CA RID: 8650
		private ExecutionContext m_Context;

		// Token: 0x0200043B RID: 1083
		private class CallbackContext
		{
			// Token: 0x060021C1 RID: 8641 RVA: 0x000859E2 File Offset: 0x000849E2
			internal CallbackContext(object request, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
			{
				this.request = request;
				this.certificate = certificate;
				this.chain = chain;
				this.sslPolicyErrors = sslPolicyErrors;
			}

			// Token: 0x040021CB RID: 8651
			internal readonly object request;

			// Token: 0x040021CC RID: 8652
			internal readonly X509Certificate certificate;

			// Token: 0x040021CD RID: 8653
			internal readonly X509Chain chain;

			// Token: 0x040021CE RID: 8654
			internal readonly SslPolicyErrors sslPolicyErrors;

			// Token: 0x040021CF RID: 8655
			internal bool result;
		}
	}
}
