using System;
using System.Security.Policy;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x0200000F RID: 15
	internal static class ApplicationTrust
	{
		// Token: 0x06000082 RID: 130 RVA: 0x00004F40 File Offset: 0x00003F40
		public static ApplicationTrust RequestTrust(SubscriptionState subState, bool isShellVisible, bool isUpdate, ActivationContext actCtx)
		{
			return ApplicationTrust.RequestTrust(subState, isShellVisible, isUpdate, actCtx, new TrustManagerContext
			{
				IgnorePersistedDecision = false,
				NoPrompt = false,
				Persist = true
			});
		}

		// Token: 0x06000083 RID: 131 RVA: 0x00004F74 File Offset: 0x00003F74
		public static ApplicationTrust RequestTrust(SubscriptionState subState, bool isShellVisible, bool isUpdate, ActivationContext actCtx, TrustManagerContext tmc)
		{
			if (!subState.IsInstalled || subState.IsShellVisible != isShellVisible)
			{
				tmc.IgnorePersistedDecision = true;
			}
			if (isUpdate)
			{
				tmc.PreviousApplicationIdentity = subState.CurrentBind.ToApplicationIdentity();
			}
			bool flag = false;
			try
			{
				flag = ApplicationSecurityManager.DetermineApplicationTrust(actCtx, tmc);
			}
			catch (TypeLoadException ex)
			{
				throw new InvalidDeploymentException(Resources.GetString("Ex_InvalidTrustInfo"), ex);
			}
			if (!flag)
			{
				throw new TrustNotGrantedException(Resources.GetString("Ex_NoTrust"));
			}
			ApplicationTrust applicationTrust = null;
			for (int i = 0; i < 5; i++)
			{
				applicationTrust = ApplicationSecurityManager.UserApplicationTrusts[actCtx.Identity.FullName];
				if (applicationTrust != null)
				{
					break;
				}
				Thread.Sleep(10);
			}
			if (applicationTrust == null)
			{
				throw new InvalidDeploymentException(Resources.GetString("Ex_InvalidMatchTrust"));
			}
			return applicationTrust;
		}

		// Token: 0x06000084 RID: 132 RVA: 0x00005034 File Offset: 0x00004034
		public static void RemoveCachedTrust(DefinitionAppId appId)
		{
			ApplicationSecurityManager.UserApplicationTrusts.Remove(appId.ToApplicationIdentity(), ApplicationVersionMatch.MatchExactVersion);
		}

		// Token: 0x06000085 RID: 133 RVA: 0x00005048 File Offset: 0x00004048
		public static ApplicationTrust PersistTrustWithoutEvaluation(ActivationContext actCtx)
		{
			ApplicationSecurityInfo applicationSecurityInfo = new ApplicationSecurityInfo(actCtx);
			ApplicationTrust applicationTrust = new ApplicationTrust(actCtx.Identity);
			applicationTrust.IsApplicationTrustedToRun = true;
			applicationTrust.DefaultGrantSet = new PolicyStatement(applicationSecurityInfo.DefaultRequestSet, PolicyStatementAttribute.Nothing);
			applicationTrust.Persist = true;
			applicationTrust.ApplicationIdentity = actCtx.Identity;
			ApplicationSecurityManager.UserApplicationTrusts.Add(applicationTrust);
			return applicationTrust;
		}
	}
}
