﻿using System;
using System.Deployment.Internal.Isolation.Manifest;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security;
using System.Security.Policy;

namespace System.Runtime.Hosting
{
	// Token: 0x02000049 RID: 73
	[ComVisible(true)]
	public class ApplicationActivator
	{
		// Token: 0x0600041A RID: 1050 RVA: 0x00010840 File Offset: 0x0000F840
		public virtual ObjectHandle CreateInstance(ActivationContext activationContext)
		{
			return this.CreateInstance(activationContext, null);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x0001084C File Offset: 0x0000F84C
		public virtual ObjectHandle CreateInstance(ActivationContext activationContext, string[] activationCustomData)
		{
			if (activationContext == null)
			{
				throw new ArgumentNullException("activationContext");
			}
			if (CmsUtils.CompareIdentities(AppDomain.CurrentDomain.ActivationContext, activationContext))
			{
				ManifestRunner manifestRunner = new ManifestRunner(AppDomain.CurrentDomain, activationContext);
				return new ObjectHandle(manifestRunner.ExecuteAsAssembly());
			}
			AppDomainSetup appDomainSetup = new AppDomainSetup(new ActivationArguments(activationContext, activationCustomData));
			return ApplicationActivator.CreateInstanceHelper(appDomainSetup);
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x000108AC File Offset: 0x0000F8AC
		protected static ObjectHandle CreateInstanceHelper(AppDomainSetup adSetup)
		{
			if (adSetup.ActivationArguments == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MissingActivationArguments"));
			}
			adSetup.ActivationArguments.ActivateInstance = true;
			Evidence evidence = AppDomain.CurrentDomain.Evidence;
			Evidence evidence2 = CmsUtils.MergeApplicationEvidence(null, adSetup.ActivationArguments.ApplicationIdentity, adSetup.ActivationArguments.ActivationContext, adSetup.ActivationArguments.ActivationData);
			HostSecurityManager hostSecurityManager = AppDomain.CurrentDomain.HostSecurityManager;
			ApplicationTrust applicationTrust = hostSecurityManager.DetermineApplicationTrust(evidence2, evidence, new TrustManagerContext());
			if (applicationTrust == null || !applicationTrust.IsApplicationTrustedToRun)
			{
				throw new PolicyException(Environment.GetResourceString("Policy_NoExecutionPermission"), -2146233320, null);
			}
			ObjRef objRef = AppDomain.nCreateInstance(adSetup.ActivationArguments.ApplicationIdentity.FullName, adSetup, evidence2, (evidence2 == null) ? AppDomain.CurrentDomain.InternalEvidence : null, AppDomain.CurrentDomain.GetSecurityDescriptor());
			if (objRef == null)
			{
				return null;
			}
			return RemotingServices.Unmarshal(objRef) as ObjectHandle;
		}
	}
}
