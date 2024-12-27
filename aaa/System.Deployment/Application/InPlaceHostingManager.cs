using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Permissions;

namespace System.Deployment.Application
{
	// Token: 0x0200005D RID: 93
	[SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
	public class InPlaceHostingManager : IDisposable
	{
		// Token: 0x060002CF RID: 719 RVA: 0x00010678 File Offset: 0x0000F678
		public InPlaceHostingManager(Uri deploymentManifest, bool launchInHostProcess)
		{
			if (!PlatformSpecific.OnXPOrAbove)
			{
				throw new PlatformNotSupportedException(Resources.GetString("Ex_RequiresXPOrHigher"));
			}
			if (deploymentManifest == null)
			{
				throw new ArgumentNullException("deploymentManifest");
			}
			UriHelper.ValidateSupportedSchemeInArgument(deploymentManifest, "deploymentSource");
			this._deploymentManager = new DeploymentManager(deploymentManifest, false, true, null, null);
			this._isLaunchInHostProcess = launchInHostProcess;
			this._Initialize();
		}

		// Token: 0x060002D0 RID: 720 RVA: 0x000106DE File Offset: 0x0000F6DE
		public InPlaceHostingManager(Uri deploymentManifest)
			: this(deploymentManifest, true)
		{
		}

		// Token: 0x060002D1 RID: 721 RVA: 0x000106E8 File Offset: 0x0000F6E8
		private void _Initialize()
		{
			this._lock = new object();
			this._deploymentManager.BindCompleted += this.OnBindCompleted;
			this._deploymentManager.SynchronizeCompleted += this.OnSynchronizeCompleted;
			this._deploymentManager.ProgressChanged += this.OnProgressChanged;
			this._state = InPlaceHostingManager.State.Ready;
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0001074C File Offset: 0x0000F74C
		public void GetManifestAsync()
		{
			lock (this._lock)
			{
				this.AssertState(InPlaceHostingManager.State.Ready);
				try
				{
					this.ChangeState(InPlaceHostingManager.State.GettingManifest);
					this._deploymentManager.BindAsync();
				}
				catch
				{
					this.ChangeState(InPlaceHostingManager.State.Done);
					throw;
				}
			}
		}

		// Token: 0x060002D3 RID: 723 RVA: 0x000107B0 File Offset: 0x0000F7B0
		public void AssertApplicationRequirements()
		{
			lock (this._lock)
			{
				if (this._appType == AppType.CustomHostSpecified)
				{
					throw new InvalidOperationException(Resources.GetString("Ex_CannotCallAssertApplicationRequirements"));
				}
				this.AssertApplicationRequirements(false);
			}
		}

		// Token: 0x060002D4 RID: 724 RVA: 0x00010804 File Offset: 0x0000F804
		public void AssertApplicationRequirements(bool grantApplicationTrust)
		{
			lock (this._lock)
			{
				if (this._appType == AppType.CustomHostSpecified)
				{
					throw new InvalidOperationException(Resources.GetString("Ex_CannotCallAssertApplicationRequirements"));
				}
				this.AssertState(InPlaceHostingManager.State.GetManifestSucceeded, InPlaceHostingManager.State.DownloadingApplication);
				try
				{
					this.ChangeState(InPlaceHostingManager.State.VerifyingRequirements);
					if (grantApplicationTrust)
					{
						this._deploymentManager.PersistTrustWithoutEvaluation();
					}
					else
					{
						TrustParams trustParams = new TrustParams();
						trustParams.NoPrompt = true;
						this._deploymentManager.DetermineTrust(trustParams);
					}
					this._deploymentManager.DeterminePlatformRequirements();
					this.ChangeState(InPlaceHostingManager.State.VerifyRequirementsSucceeded);
				}
				catch
				{
					this.ChangeState(InPlaceHostingManager.State.Done);
					throw;
				}
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x000108B4 File Offset: 0x0000F8B4
		public void DownloadApplicationAsync()
		{
			lock (this._lock)
			{
				if (this._appType == AppType.CustomHostSpecified)
				{
					this.AssertState(InPlaceHostingManager.State.GetManifestSucceeded);
				}
				else if (this._isCached)
				{
					this.AssertState(InPlaceHostingManager.State.GetManifestSucceeded, InPlaceHostingManager.State.VerifyRequirementsSucceeded);
				}
				else
				{
					this.AssertState(InPlaceHostingManager.State.GetManifestSucceeded, InPlaceHostingManager.State.VerifyRequirementsSucceeded);
				}
				try
				{
					this.ChangeState(InPlaceHostingManager.State.DownloadingApplication);
					this._deploymentManager.SynchronizeAsync();
				}
				catch
				{
					this.ChangeState(InPlaceHostingManager.State.Done);
					throw;
				}
			}
		}

		// Token: 0x060002D6 RID: 726 RVA: 0x00010940 File Offset: 0x0000F940
		public ObjectHandle Execute()
		{
			ObjectHandle objectHandle;
			lock (this._lock)
			{
				this.AssertState(InPlaceHostingManager.State.DownloadApplicationSucceeded);
				this.ChangeState(InPlaceHostingManager.State.Done);
				objectHandle = this._deploymentManager.ExecuteNewDomain();
			}
			return objectHandle;
		}

		// Token: 0x060002D7 RID: 727 RVA: 0x00010990 File Offset: 0x0000F990
		public void CancelAsync()
		{
			lock (this._lock)
			{
				this.ChangeState(InPlaceHostingManager.State.Done);
				this._deploymentManager.CancelAsync();
			}
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x000109D8 File Offset: 0x0000F9D8
		public void Dispose()
		{
			lock (this._lock)
			{
				this.ChangeState(InPlaceHostingManager.State.Done);
				this._deploymentManager.BindCompleted -= this.OnBindCompleted;
				this._deploymentManager.SynchronizeCompleted -= this.OnSynchronizeCompleted;
				this._deploymentManager.ProgressChanged -= this.OnProgressChanged;
				this._deploymentManager.Dispose();
			}
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x00010A64 File Offset: 0x0000FA64
		public static void UninstallCustomUXApplication(string subscriptionId)
		{
			DefinitionIdentity subIdAndValidate = InPlaceHostingManager.GetSubIdAndValidate(subscriptionId);
			SubscriptionStore currentUser = SubscriptionStore.CurrentUser;
			currentUser.RefreshStorePointer();
			SubscriptionState subscriptionState = currentUser.GetSubscriptionState(subIdAndValidate);
			subscriptionState.SubscriptionStore.UninstallCustomUXSubscription(subscriptionState);
		}

		// Token: 0x060002DA RID: 730 RVA: 0x00010A9C File Offset: 0x0000FA9C
		public static void UninstallCustomAddIn(string subscriptionId)
		{
			DefinitionIdentity subIdAndValidate = InPlaceHostingManager.GetSubIdAndValidate(subscriptionId);
			SubscriptionStore currentUser = SubscriptionStore.CurrentUser;
			currentUser.RefreshStorePointer();
			SubscriptionState subscriptionState = currentUser.GetSubscriptionState(subIdAndValidate);
			subscriptionState.SubscriptionStore.UninstallCustomHostSpecifiedSubscription(subscriptionState);
		}

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x060002DB RID: 731 RVA: 0x00010AD2 File Offset: 0x0000FAD2
		// (remove) Token: 0x060002DC RID: 732 RVA: 0x00010AEB File Offset: 0x0000FAEB
		public event EventHandler<GetManifestCompletedEventArgs> GetManifestCompleted;

		// Token: 0x1400000D RID: 13
		// (add) Token: 0x060002DD RID: 733 RVA: 0x00010B04 File Offset: 0x0000FB04
		// (remove) Token: 0x060002DE RID: 734 RVA: 0x00010B1D File Offset: 0x0000FB1D
		public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x060002DF RID: 735 RVA: 0x00010B36 File Offset: 0x0000FB36
		// (remove) Token: 0x060002E0 RID: 736 RVA: 0x00010B4F File Offset: 0x0000FB4F
		public event EventHandler<DownloadApplicationCompletedEventArgs> DownloadApplicationCompleted;

		// Token: 0x060002E1 RID: 737 RVA: 0x00010B68 File Offset: 0x0000FB68
		private static DefinitionIdentity GetSubIdAndValidate(string subscriptionId)
		{
			if (subscriptionId == null)
			{
				throw new ArgumentNullException("subscriptionId", Resources.GetString("Ex_ComArgSubIdentityNull"));
			}
			DefinitionIdentity definitionIdentity = null;
			try
			{
				definitionIdentity = new DefinitionIdentity(subscriptionId);
			}
			catch (COMException ex)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.GetString("Ex_ComArgSubIdentityNotValid"), new object[] { subscriptionId }), ex);
			}
			catch (SEHException ex2)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.GetString("Ex_ComArgSubIdentityNotValid"), new object[] { subscriptionId }), ex2);
			}
			catch (ArgumentException ex3)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.GetString("Ex_ComArgSubIdentityNotValid"), new object[] { subscriptionId }), ex3);
			}
			if (definitionIdentity.Name == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.GetString("Ex_ComArgSubIdentityNotValid"), new object[] { subscriptionId }));
			}
			if (definitionIdentity.PublicKeyToken == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.GetString("Ex_ComArgSubIdentityNotValid"), new object[] { subscriptionId }));
			}
			if (definitionIdentity.ProcessorArchitecture == null)
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.GetString("Ex_ComArgSubIdentityNotValid"), new object[] { subscriptionId }));
			}
			if (definitionIdentity.Version != null)
			{
				throw new ArgumentException(Resources.GetString("Ex_ComArgSubIdentityWithVersion"));
			}
			return definitionIdentity;
		}

		// Token: 0x060002E2 RID: 738 RVA: 0x00010CF8 File Offset: 0x0000FCF8
		private void OnBindCompleted(object sender, BindCompletedEventArgs e)
		{
			lock (this._lock)
			{
				this.AssertState(InPlaceHostingManager.State.GettingManifest, InPlaceHostingManager.State.Done);
				GetManifestCompletedEventArgs getManifestCompletedEventArgs = null;
				try
				{
					if (this._state != InPlaceHostingManager.State.Done)
					{
						if (e.Cancelled || e.Error != null)
						{
							this.ChangeState(InPlaceHostingManager.State.Done);
						}
						else
						{
							this.ChangeState(InPlaceHostingManager.State.GetManifestSucceeded, e);
						}
					}
					if (this.GetManifestCompleted == null)
					{
						return;
					}
					if (e.Error != null || e.Cancelled)
					{
						getManifestCompletedEventArgs = new GetManifestCompletedEventArgs(e, this._deploymentManager.LogFilePath);
					}
					else
					{
						this._isCached = e.IsCached;
						this._applicationId = e.ActivationContext.Identity;
						bool install = this._deploymentManager.ActivationDescription.DeployManifest.Deployment.Install;
						bool hostInBrowser = this._deploymentManager.ActivationDescription.AppManifest.EntryPoints[0].HostInBrowser;
						this._appType = this._deploymentManager.ActivationDescription.appType;
						bool useManifestForTrust = this._deploymentManager.ActivationDescription.AppManifest.UseManifestForTrust;
						Uri providerCodebaseUri = this._deploymentManager.ActivationDescription.DeployManifest.Deployment.ProviderCodebaseUri;
						if (this._isLaunchInHostProcess && this._appType != AppType.CustomHostSpecified && !hostInBrowser)
						{
							getManifestCompletedEventArgs = new GetManifestCompletedEventArgs(e, new InvalidOperationException(Resources.GetString("Ex_HostInBrowserFlagMustBeTrue")), this._deploymentManager.LogFilePath);
						}
						else if (install && (this._isLaunchInHostProcess || this._appType == AppType.CustomHostSpecified))
						{
							getManifestCompletedEventArgs = new GetManifestCompletedEventArgs(e, new InvalidOperationException(Resources.GetString("Ex_InstallFlagMustBeFalse")), this._deploymentManager.LogFilePath);
						}
						else if (useManifestForTrust && this._appType == AppType.CustomHostSpecified)
						{
							getManifestCompletedEventArgs = new GetManifestCompletedEventArgs(e, new InvalidOperationException(Resources.GetString("Ex_CannotHaveUseManifestForTrustFlag")), this._deploymentManager.LogFilePath);
						}
						else if (providerCodebaseUri != null && this._appType == AppType.CustomHostSpecified)
						{
							getManifestCompletedEventArgs = new GetManifestCompletedEventArgs(e, new InvalidOperationException(Resources.GetString("Ex_CannotHaveDeploymentProvider")), this._deploymentManager.LogFilePath);
						}
						else if (hostInBrowser && this._appType == AppType.CustomUX)
						{
							getManifestCompletedEventArgs = new GetManifestCompletedEventArgs(e, new InvalidOperationException(Resources.GetString("Ex_CannotHaveCustomUXFlag")), this._deploymentManager.LogFilePath);
						}
						else
						{
							getManifestCompletedEventArgs = new GetManifestCompletedEventArgs(e, this._deploymentManager.ActivationDescription, this._deploymentManager.LogFilePath);
						}
					}
				}
				catch
				{
					this.ChangeState(InPlaceHostingManager.State.Done);
					throw;
				}
				this.GetManifestCompleted(this, getManifestCompletedEventArgs);
			}
		}

		// Token: 0x060002E3 RID: 739 RVA: 0x00010F98 File Offset: 0x0000FF98
		private void OnSynchronizeCompleted(object sender, SynchronizeCompletedEventArgs e)
		{
			lock (this._lock)
			{
				this.AssertState(InPlaceHostingManager.State.DownloadingApplication, InPlaceHostingManager.State.VerifyRequirementsSucceeded, InPlaceHostingManager.State.Done);
				if (this._state != InPlaceHostingManager.State.Done)
				{
					if (e.Cancelled || e.Error != null)
					{
						this.ChangeState(InPlaceHostingManager.State.Done);
					}
					else
					{
						this.ChangeState(InPlaceHostingManager.State.DownloadApplicationSucceeded, e);
					}
				}
				if ((!this._isLaunchInHostProcess || this._appType == AppType.CustomHostSpecified) && this._appType != AppType.CustomUX)
				{
					this.ChangeState(InPlaceHostingManager.State.Done);
				}
				if (this.DownloadApplicationCompleted != null)
				{
					DownloadApplicationCompletedEventArgs downloadApplicationCompletedEventArgs = new DownloadApplicationCompletedEventArgs(e, this._deploymentManager.LogFilePath, this._deploymentManager.ShortcutAppId);
					this.DownloadApplicationCompleted(this, downloadApplicationCompletedEventArgs);
				}
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x00011054 File Offset: 0x00010054
		private void OnProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
		{
			lock (this._lock)
			{
				if (this.DownloadProgressChanged != null)
				{
					DownloadProgressChangedEventArgs downloadProgressChangedEventArgs = new DownloadProgressChangedEventArgs(e.ProgressPercentage, e.UserState, e.BytesCompleted, e.BytesTotal, e.State);
					this.DownloadProgressChanged(this, downloadProgressChangedEventArgs);
				}
			}
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000110C0 File Offset: 0x000100C0
		private void AssertState(InPlaceHostingManager.State validState)
		{
			if (this._state == InPlaceHostingManager.State.Done)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_NoFurtherOperations"));
			}
			if (validState != this._state)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_InvalidSequence"));
			}
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000110F4 File Offset: 0x000100F4
		private void AssertState(InPlaceHostingManager.State validState0, InPlaceHostingManager.State validState1)
		{
			if (this._state == InPlaceHostingManager.State.Done && validState0 != this._state && validState1 != this._state)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_NoFurtherOperations"));
			}
			if (validState0 != this._state && validState1 != this._state)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_InvalidSequence"));
			}
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x00011150 File Offset: 0x00010150
		private void AssertState(InPlaceHostingManager.State validState0, InPlaceHostingManager.State validState1, InPlaceHostingManager.State validState2)
		{
			if (this._state == InPlaceHostingManager.State.Done && validState0 != this._state && validState1 != this._state && validState2 != this._state)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_NoFurtherOperations"));
			}
			if (validState0 != this._state && validState1 != this._state && validState2 != this._state)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_InvalidSequence"));
			}
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x000111BC File Offset: 0x000101BC
		private void ChangeState(InPlaceHostingManager.State nextState, AsyncCompletedEventArgs e)
		{
			if (e.Cancelled || e.Error != null)
			{
				this._state = InPlaceHostingManager.State.Done;
				return;
			}
			this._state = nextState;
		}

		// Token: 0x060002E9 RID: 745 RVA: 0x000111DD File Offset: 0x000101DD
		private void ChangeState(InPlaceHostingManager.State nextState)
		{
			this._state = nextState;
		}

		// Token: 0x0400022A RID: 554
		private DeploymentManager _deploymentManager;

		// Token: 0x0400022B RID: 555
		private ApplicationIdentity _applicationId;

		// Token: 0x0400022C RID: 556
		private InPlaceHostingManager.State _state;

		// Token: 0x0400022D RID: 557
		private bool _isCached;

		// Token: 0x0400022E RID: 558
		private bool _isLaunchInHostProcess;

		// Token: 0x0400022F RID: 559
		private object _lock;

		// Token: 0x04000230 RID: 560
		private AppType _appType;

		// Token: 0x0200005E RID: 94
		private enum State
		{
			// Token: 0x04000235 RID: 565
			Ready,
			// Token: 0x04000236 RID: 566
			GettingManifest,
			// Token: 0x04000237 RID: 567
			GetManifestSucceeded,
			// Token: 0x04000238 RID: 568
			VerifyingRequirements,
			// Token: 0x04000239 RID: 569
			VerifyRequirementsSucceeded,
			// Token: 0x0400023A RID: 570
			DownloadingApplication,
			// Token: 0x0400023B RID: 571
			DownloadApplicationSucceeded,
			// Token: 0x0400023C RID: 572
			Done
		}
	}
}
