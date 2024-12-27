using System;
using System.Collections;
using System.Deployment.Application.Manifest;
using System.Deployment.Internal;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading;
using Microsoft.Internal.Performance;

namespace System.Deployment.Application
{
	// Token: 0x02000002 RID: 2
	internal class ApplicationActivator
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		private void DisplayActivationFailureReason(Exception exception, string errorPageUrl)
		{
			string text = Resources.GetString("ErrorMessage_GenericActivationFailure");
			string @string = Resources.GetString("ErrorMessage_GenericLinkUrlMessage");
			Exception innerMostException = this.GetInnerMostException(exception);
			if (exception is DeploymentDownloadException)
			{
				text = Resources.GetString("ErrorMessage_NetworkError");
				DeploymentDownloadException ex = (DeploymentDownloadException)exception;
				if (ex.SubType == ExceptionTypes.SizeLimitForPartialTrustOnlineAppExceeded)
				{
					text = Resources.GetString("ErrorMessage_SizeLimitForPartialTrustOnlineAppExceeded");
				}
				if (innerMostException is WebException)
				{
					WebException ex2 = (WebException)innerMostException;
					if (ex2.Response != null && ex2.Response is HttpWebResponse)
					{
						HttpWebResponse httpWebResponse = (HttpWebResponse)ex2.Response;
						if (httpWebResponse.StatusCode == HttpStatusCode.NotFound)
						{
							text = Resources.GetString("ErrorMessage_FileMissing");
						}
						else if (httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
						{
							text = Resources.GetString("ErrorMessage_AuthenticationError");
						}
						else if (httpWebResponse.StatusCode == HttpStatusCode.Forbidden)
						{
							text = Resources.GetString("ErrorMessage_Forbidden");
						}
					}
				}
				else if (innerMostException is FileNotFoundException || innerMostException is DirectoryNotFoundException)
				{
					text = Resources.GetString("ErrorMessage_FileMissing");
				}
				else if (innerMostException is UnauthorizedAccessException)
				{
					text = Resources.GetString("ErrorMessage_AuthenticationError");
				}
				else if (innerMostException is IOException && !this.IsWebExceptionInExceptionStack(exception))
				{
					text = Resources.GetString("ErrorMessage_DownloadIOError");
				}
			}
			else if (exception is InvalidDeploymentException)
			{
				InvalidDeploymentException ex3 = (InvalidDeploymentException)exception;
				if (ex3.SubType == ExceptionTypes.ManifestLoad)
				{
					text = Resources.GetString("ErrorMessage_ManifestCannotBeLoaded");
				}
				else if (ex3.SubType == ExceptionTypes.Manifest || ex3.SubType == ExceptionTypes.ManifestParse || ex3.SubType == ExceptionTypes.ManifestSemanticValidation)
				{
					text = Resources.GetString("ErrorMessage_InvalidManifest");
				}
				else if (ex3.SubType == ExceptionTypes.Validation || ex3.SubType == ExceptionTypes.HashValidation || ex3.SubType == ExceptionTypes.SignatureValidation || ex3.SubType == ExceptionTypes.RefDefValidation || ex3.SubType == ExceptionTypes.ClrValidation || ex3.SubType == ExceptionTypes.StronglyNamedAssemblyVerification || ex3.SubType == ExceptionTypes.IdentityMatchValidationForMixedModeAssembly || ex3.SubType == ExceptionTypes.AppFileLocationValidation || ex3.SubType == ExceptionTypes.FileSizeValidation)
				{
					text = Resources.GetString("ErrorMessage_ValidationFailed");
				}
				else if (ex3.SubType == ExceptionTypes.UnsupportedElevetaionRequest)
				{
					text = Resources.GetString("ErrorMessage_ManifestExecutionLevelNotSupported");
				}
			}
			else if (exception is DeploymentException)
			{
				if (((DeploymentException)exception).SubType == ExceptionTypes.ComponentStore)
				{
					text = Resources.GetString("ErrorMessage_StoreError");
				}
				else if (((DeploymentException)exception).SubType == ExceptionTypes.ActivationLimitExceeded)
				{
					text = Resources.GetString("ErrorMessage_ConcurrentActivationLimitExceeded");
				}
				else if (((DeploymentException)exception).SubType == ExceptionTypes.DiskIsFull)
				{
					text = Resources.GetString("ErrorMessage_DiskIsFull");
				}
				else if (((DeploymentException)exception).SubType == ExceptionTypes.DeploymentUriDifferent)
				{
					text = exception.Message;
				}
				else if (((DeploymentException)exception).SubType == ExceptionTypes.GroupMultipleMatch)
				{
					text = exception.Message;
				}
			}
			string text2 = Logger.GetLogFilePath();
			if (!Logger.FlushCurrentThreadLogs())
			{
				text2 = null;
			}
			string text3 = null;
			if (errorPageUrl != null)
			{
				text3 = string.Format("{0}?outer={1}&&inner={2}&&msg={3}", new object[]
				{
					errorPageUrl,
					exception.GetType().ToString(),
					innerMostException.GetType().ToString(),
					innerMostException.Message
				});
				if (text3.Length > 2048)
				{
					text3 = text3.Substring(0, 2048);
				}
			}
			this._ui.ShowError(Resources.GetString("UI_ErrorTitle"), text, text2, text3, @string);
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002438 File Offset: 0x00001438
		private void DisplayPlatformDetectionFailureUI(DependentPlatformMissingException ex)
		{
			Uri uri = null;
			if (this._fullTrust)
			{
				uri = ex.SupportUrl;
			}
			this._ui.ShowPlatform(ex.Message, uri);
		}

		// Token: 0x06000003 RID: 3 RVA: 0x00002468 File Offset: 0x00001468
		public void ActivateDeployment(string activationUrl, bool isShortcut)
		{
			LifetimeManager.StartOperation();
			bool flag = false;
			try
			{
				object[] array = new object[] { activationUrl, isShortcut, null, null, null };
				flag = ThreadPool.QueueUserWorkItem(new WaitCallback(this.ActivateDeploymentWorker), array);
				if (!flag)
				{
					throw new OutOfMemoryException();
				}
			}
			finally
			{
				if (!flag)
				{
					LifetimeManager.EndOperation();
				}
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000024D4 File Offset: 0x000014D4
		public void ActivateDeploymentEx(string activationUrl, int unsignedPolicy, int signedPolicy)
		{
			LifetimeManager.StartOperation();
			bool flag = false;
			try
			{
				ApplicationActivator.BrowserSettings browserSettings = new ApplicationActivator.BrowserSettings();
				browserSettings.ManagedSignedFlag = ApplicationActivator.BrowserSettings.GetManagedFlagValue(signedPolicy);
				browserSettings.ManagedUnSignedFlag = ApplicationActivator.BrowserSettings.GetManagedFlagValue(unsignedPolicy);
				object[] array = new object[] { activationUrl, false, null, null, browserSettings };
				flag = ThreadPool.QueueUserWorkItem(new WaitCallback(this.ActivateDeploymentWorker), array);
				if (!flag)
				{
					throw new OutOfMemoryException();
				}
			}
			finally
			{
				if (!flag)
				{
					LifetimeManager.EndOperation();
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002560 File Offset: 0x00001560
		public void ActivateApplicationExtension(string textualSubId, string deploymentProviderUrl, string targetAssociatedFile)
		{
			LifetimeManager.StartOperation();
			bool flag = false;
			try
			{
				object[] array = new object[] { targetAssociatedFile, false, textualSubId, deploymentProviderUrl, null };
				flag = ThreadPool.QueueUserWorkItem(new WaitCallback(this.ActivateDeploymentWorker), array);
				if (!flag)
				{
					throw new OutOfMemoryException();
				}
			}
			finally
			{
				if (!flag)
				{
					LifetimeManager.EndOperation();
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000025CC File Offset: 0x000015CC
		private void ActivateDeploymentWorker(object state)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			try
			{
				CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfNewApptBegin);
				object[] array = (object[])state;
				text = (string)array[0];
				bool flag = (bool)array[1];
				if (array[2] != null)
				{
					text2 = (string)array[2];
				}
				if (array[3] != null)
				{
					text3 = (string)array[3];
				}
				ApplicationActivator.BrowserSettings browserSettings = null;
				if (array[4] != null)
				{
					browserSettings = (ApplicationActivator.BrowserSettings)array[4];
				}
				Logger.StartCurrentThreadLogging();
				Logger.SetSubscriptionUrl(text);
				Uri uri = null;
				string text4 = null;
				try
				{
					int num = this.CheckActivationInProgress(text);
					this._ui = new UserInterface(false);
					if (!PolicyKeys.SuppressLimitOnNumberOfActivations() && num > 8)
					{
						throw new DeploymentException(ExceptionTypes.ActivationLimitExceeded, Resources.GetString("Ex_TooManyLiveActivation"));
					}
					if (text.Length > 16384)
					{
						throw new DeploymentException(ExceptionTypes.Activation, Resources.GetString("Ex_UrlTooLong"));
					}
					uri = new Uri(text);
					try
					{
						UriHelper.ValidateSupportedSchemeInArgument(uri, "activationUrl");
					}
					catch (ArgumentException ex)
					{
						throw new InvalidDeploymentException(ExceptionTypes.UriSchemeNotSupported, Resources.GetString("Ex_NotSupportedUriScheme"), ex);
					}
					Logger.AddPhaseInformation(Resources.GetString("PhaseLog_StartOfActivation"), new object[] { text });
					this.PerformDeploymentActivation(uri, flag, text2, text3, browserSettings, ref text4);
					Logger.AddPhaseInformation(Resources.GetString("ActivateManifestSucceeded"), new object[] { text });
				}
				catch (DependentPlatformMissingException ex2)
				{
					Logger.AddErrorInformation(ex2, Resources.GetString("ActivateManifestException"), new object[] { text });
					if (this._ui == null)
					{
						this._ui = new UserInterface();
					}
					if (!this._ui.SplashCancelled())
					{
						this.DisplayPlatformDetectionFailureUI(ex2);
					}
				}
				catch (DownloadCancelledException ex3)
				{
					Logger.AddErrorInformation(ex3, Resources.GetString("ActivateManifestException"), new object[] { text });
				}
				catch (TrustNotGrantedException ex4)
				{
					Logger.AddErrorInformation(ex4, Resources.GetString("ActivateManifestException"), new object[] { text });
				}
				catch (DeploymentException ex5)
				{
					Logger.AddErrorInformation(ex5, Resources.GetString("ActivateManifestException"), new object[] { text });
					if (ex5.SubType != ExceptionTypes.ActivationInProgress)
					{
						if (this._ui == null)
						{
							this._ui = new UserInterface();
						}
						if (!this._ui.SplashCancelled())
						{
							if (ex5.SubType == ExceptionTypes.ActivationLimitExceeded)
							{
								if (Interlocked.CompareExchange(ref ApplicationActivator._liveActivationLimitUIStatus, 1, 0) == 0)
								{
									this.DisplayActivationFailureReason(ex5, text4);
									Interlocked.CompareExchange(ref ApplicationActivator._liveActivationLimitUIStatus, 0, 1);
								}
							}
							else
							{
								this.DisplayActivationFailureReason(ex5, text4);
							}
						}
					}
				}
				catch (Exception ex6)
				{
					if (ex6 is AccessViolationException || ex6 is OutOfMemoryException)
					{
						throw;
					}
					if (PolicyKeys.DisableGenericExceptionHandler())
					{
						throw;
					}
					Logger.AddErrorInformation(ex6, Resources.GetString("ActivateManifestException"), new object[] { text });
					if (this._ui == null)
					{
						this._ui = new UserInterface();
					}
					if (!this._ui.SplashCancelled())
					{
						this.DisplayActivationFailureReason(ex6, text4);
					}
				}
			}
			finally
			{
				this.RemoveActivationInProgressEntry(text);
				if (this._ui != null)
				{
					this._ui.Dispose();
					this._ui = null;
				}
				CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfNewApptEnd);
				Logger.EndCurrentThreadLogging();
				LifetimeManager.EndOperation();
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002998 File Offset: 0x00001998
		private void PerformDeploymentActivation(Uri activationUri, bool isShortcut, string textualSubId, string deploymentProviderUrlFromExtension, ApplicationActivator.BrowserSettings browserSettings, ref string errorPageUrl)
		{
			TempFile tempFile = null;
			try
			{
				string text = null;
				Uri uri = null;
				bool flag = false;
				this._subStore = SubscriptionStore.CurrentUser;
				this._subStore.RefreshStorePointer();
				Uri uri2 = activationUri;
				bool flag2 = false;
				ActivationDescription activationDescription;
				if (textualSubId != null)
				{
					flag2 = true;
					activationDescription = this.ProcessOrFollowExtension(activationUri, textualSubId, deploymentProviderUrlFromExtension, ref errorPageUrl, out tempFile);
					if (activationDescription == null)
					{
						return;
					}
				}
				else if (isShortcut)
				{
					text = activationUri.LocalPath;
					activationDescription = this.ProcessOrFollowShortcut(text, ref errorPageUrl, out tempFile);
					if (activationDescription == null)
					{
						return;
					}
				}
				else
				{
					SubscriptionState subscriptionState;
					AssemblyManifest assemblyManifest = DownloadManager.DownloadDeploymentManifestBypass(this._subStore, ref uri2, out tempFile, out subscriptionState, null, null);
					if (browserSettings != null && tempFile != null)
					{
						browserSettings.Validate(tempFile.Path);
					}
					if (assemblyManifest.Description != null)
					{
						errorPageUrl = assemblyManifest.Description.ErrorReportUrl;
					}
					activationDescription = new ActivationDescription();
					if (subscriptionState != null)
					{
						text = null;
						activationDescription.SetApplicationManifest(subscriptionState.CurrentApplicationManifest, null, null);
						activationDescription.AppId = subscriptionState.CurrentBind;
						flag = true;
					}
					else
					{
						text = tempFile.Path;
					}
					Logger.SetDeploymentManifest(assemblyManifest);
					Logger.AddPhaseInformation(Resources.GetString("PhaseLog_ProcessingDeploymentManifestComplete"));
					activationDescription.SetDeploymentManifest(assemblyManifest, uri2, text);
					activationDescription.IsUpdate = false;
					activationDescription.ActType = ActivationType.InstallViaDotApplication;
					uri = activationUri;
				}
				if (this._ui.SplashCancelled())
				{
					throw new DownloadCancelledException();
				}
				if (activationDescription.DeployManifest.Deployment == null)
				{
					throw new DeploymentException(ExceptionTypes.Activation, Resources.GetString("Ex_NotDeploymentOrShortcut"));
				}
				bool flag3 = false;
				SubscriptionState subscriptionState2 = this._subStore.GetSubscriptionState(activationDescription.DeployManifest);
				this.CheckDeploymentProviderValidity(activationDescription, subscriptionState2);
				if (!flag)
				{
					flag3 = this.InstallApplication(ref subscriptionState2, activationDescription);
					Logger.AddPhaseInformation(Resources.GetString("PhaseLog_InstallationComplete"));
				}
				else
				{
					this._subStore.SetLastCheckTimeToNow(subscriptionState2);
				}
				if (activationDescription.DeployManifest.Deployment.DisallowUrlActivation && !isShortcut && (!activationUri.IsFile || activationUri.IsUnc))
				{
					if (flag3)
					{
						this._ui.ShowMessage(Resources.GetString("Activation_DisallowUrlActivationMessageAfterInstall"), Resources.GetString("Activation_DisallowUrlActivationCaptionAfterInstall"));
					}
					else
					{
						this._ui.ShowMessage(Resources.GetString("Activation_DisallowUrlActivationMessage"), Resources.GetString("Activation_DisallowUrlActivationCaption"));
					}
				}
				else if (flag2)
				{
					this.Activate(activationDescription.AppId, activationDescription.AppManifest, activationUri.AbsoluteUri, true);
				}
				else if (isShortcut)
				{
					string text2 = null;
					int num = text.IndexOf('|', 0);
					if (num > 0 && num + 1 < text.Length)
					{
						text2 = text.Substring(num + 1);
					}
					if (text2 == null)
					{
						this.Activate(activationDescription.AppId, activationDescription.AppManifest, null, false);
					}
					else
					{
						this.Activate(activationDescription.AppId, activationDescription.AppManifest, text2, true);
					}
				}
				else
				{
					this.Activate(activationDescription.AppId, activationDescription.AppManifest, uri.AbsoluteUri, false);
				}
			}
			finally
			{
				if (tempFile != null)
				{
					tempFile.Dispose();
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x00002C7C File Offset: 0x00001C7C
		private ActivationDescription ProcessOrFollowExtension(Uri associatedFile, string textualSubId, string deploymentProviderUrlFromExtension, ref string errorPageUrl, out TempFile deployFile)
		{
			deployFile = null;
			DefinitionIdentity definitionIdentity = new DefinitionIdentity(textualSubId);
			SubscriptionState subscriptionState = this._subStore.GetSubscriptionState(definitionIdentity);
			ActivationDescription activationDescription = null;
			if (subscriptionState.IsInstalled && subscriptionState.IsShellVisible)
			{
				this.PerformDeploymentUpdate(ref subscriptionState, ref errorPageUrl);
				this.Activate(subscriptionState.CurrentBind, subscriptionState.CurrentApplicationManifest, associatedFile.AbsoluteUri, true);
			}
			else
			{
				if (string.IsNullOrEmpty(deploymentProviderUrlFromExtension))
				{
					throw new DeploymentException(ExceptionTypes.Activation, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_FileAssociationNoDpUrl"), new object[] { textualSubId }));
				}
				Uri uri = new Uri(deploymentProviderUrlFromExtension);
				AssemblyManifest assemblyManifest = DownloadManager.DownloadDeploymentManifest(this._subStore, ref uri, out deployFile);
				if (assemblyManifest.Description != null)
				{
					errorPageUrl = assemblyManifest.Description.ErrorReportUrl;
				}
				if (!assemblyManifest.Deployment.Install)
				{
					throw new DeploymentException(ExceptionTypes.Activation, Resources.GetString("Ex_FileAssociationRefOnline"));
				}
				activationDescription = new ActivationDescription();
				activationDescription.SetDeploymentManifest(assemblyManifest, uri, deployFile.Path);
				activationDescription.IsUpdate = false;
				activationDescription.ActType = ActivationType.InstallViaFileAssociation;
			}
			return activationDescription;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002D84 File Offset: 0x00001D84
		private ActivationDescription ProcessOrFollowShortcut(string shortcutFile, ref string errorPageUrl, out TempFile deployFile)
		{
			deployFile = null;
			string text = shortcutFile;
			string text2 = null;
			int num = shortcutFile.IndexOf('|', 0);
			if (num > 0)
			{
				text = shortcutFile.Substring(0, num);
				if (num + 1 < shortcutFile.Length)
				{
					text2 = shortcutFile.Substring(num + 1);
				}
			}
			DefinitionIdentity definitionIdentity;
			Uri uri;
			ShellExposure.ParseAppShortcut(text, out definitionIdentity, out uri);
			SubscriptionState subscriptionState = this._subStore.GetSubscriptionState(definitionIdentity);
			ActivationDescription activationDescription = null;
			if (subscriptionState.IsInstalled && subscriptionState.IsShellVisible)
			{
				this.PerformDeploymentUpdate(ref subscriptionState, ref errorPageUrl);
				if (text2 == null)
				{
					this.Activate(subscriptionState.CurrentBind, subscriptionState.CurrentApplicationManifest, null, false);
				}
				else
				{
					this.Activate(subscriptionState.CurrentBind, subscriptionState.CurrentApplicationManifest, text2, true);
				}
			}
			else
			{
				Uri uri2 = uri;
				AssemblyManifest assemblyManifest = DownloadManager.DownloadDeploymentManifest(this._subStore, ref uri2, out deployFile);
				if (assemblyManifest.Description != null)
				{
					errorPageUrl = assemblyManifest.Description.ErrorReportUrl;
				}
				if (!assemblyManifest.Deployment.Install)
				{
					throw new DeploymentException(ExceptionTypes.Activation, Resources.GetString("Ex_ShortcutRefOnlineOnly"));
				}
				activationDescription = new ActivationDescription();
				activationDescription.SetDeploymentManifest(assemblyManifest, uri2, deployFile.Path);
				activationDescription.IsUpdate = false;
				activationDescription.ActType = ActivationType.InstallViaShortcut;
			}
			return activationDescription;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002EA8 File Offset: 0x00001EA8
		private void Activate(DefinitionAppId appId, AssemblyManifest appManifest, string activationParameter, bool useActivationParameter)
		{
			using (ActivationContext activationContext = ActivationContext.CreatePartialActivationContext(appId.ToApplicationIdentity()))
			{
				InternalActivationContextHelper.PrepareForExecution(activationContext);
				this._subStore.ActivateApplication(appId, activationParameter, useActivationParameter);
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x00002EF4 File Offset: 0x00001EF4
		private void PerformDeploymentUpdate(ref SubscriptionState subState, ref string errorPageUrl)
		{
			DeploymentUpdate deploymentUpdate = subState.CurrentDeploymentManifest.Deployment.DeploymentUpdate;
			bool flag = deploymentUpdate != null && deploymentUpdate.BeforeApplicationStartup;
			Logger.AddPhaseInformation(Resources.GetString("PhaseLog_DeploymentUpdateCheck"));
			if (flag || (subState.PendingDeployment != null && !ApplicationActivator.SkipUpdate(subState, subState.PendingDeployment)))
			{
				TempFile tempFile = null;
				try
				{
					Uri deploymentProviderUri = subState.DeploymentProviderUri;
					AssemblyManifest assemblyManifest;
					try
					{
						assemblyManifest = DownloadManager.DownloadDeploymentManifest(this._subStore, ref deploymentProviderUri, out tempFile);
						if (assemblyManifest.Description != null)
						{
							errorPageUrl = assemblyManifest.Description.ErrorReportUrl;
						}
					}
					catch (DeploymentDownloadException ex)
					{
						Logger.AddErrorInformation(ex, Resources.GetString("Upd_UpdateCheckDownloadFailed"), new object[] { subState.SubscriptionId.ToString() });
						return;
					}
					if (this._ui.SplashCancelled())
					{
						throw new DownloadCancelledException();
					}
					if (!ApplicationActivator.SkipUpdate(subState, assemblyManifest.Identity) && this._subStore.CheckUpdateInManifest(subState, deploymentProviderUri, assemblyManifest, subState.CurrentDeployment.Version) != null && !assemblyManifest.Identity.Equals(subState.ExcludedDeployment))
					{
						ActivationDescription activationDescription = new ActivationDescription();
						activationDescription.SetDeploymentManifest(assemblyManifest, deploymentProviderUri, tempFile.Path);
						activationDescription.IsUpdate = true;
						activationDescription.IsRequiredUpdate = false;
						activationDescription.ActType = ActivationType.UpdateViaShortcutOrFA;
						if (assemblyManifest.Deployment.MinimumRequiredVersion != null && assemblyManifest.Deployment.MinimumRequiredVersion.CompareTo(subState.CurrentDeployment.Version) > 0)
						{
							activationDescription.IsRequiredUpdate = true;
						}
						this.CheckDeploymentProviderValidity(activationDescription, subState);
						this.ConsumeUpdatedDeployment(ref subState, activationDescription);
					}
				}
				finally
				{
					if (tempFile != null)
					{
						tempFile.Dispose();
					}
				}
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000030D4 File Offset: 0x000020D4
		private void CheckDeploymentProviderValidity(ActivationDescription actDesc, SubscriptionState subState)
		{
			if (actDesc.DeployManifest.Deployment.Install && actDesc.DeployManifest.Deployment.ProviderCodebaseUri == null && subState != null && subState.DeploymentProviderUri != null)
			{
				Uri uri = ((subState.DeploymentProviderUri.Query != null && subState.DeploymentProviderUri.Query.Length > 0) ? new Uri(subState.DeploymentProviderUri.GetLeftPart(UriPartial.Path)) : subState.DeploymentProviderUri);
				if (!uri.Equals(actDesc.ToAppCodebase()))
				{
					throw new DeploymentException(ExceptionTypes.DeploymentUriDifferent, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("ErrorMessage_DeploymentUriDifferent"), new object[] { actDesc.DeployManifest.Description.FilteredProduct }), new DeploymentException(ExceptionTypes.DeploymentUriDifferent, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DeploymentUriDifferentExText"), new object[]
					{
						actDesc.DeployManifest.Description.FilteredProduct,
						actDesc.DeploySourceUri.AbsoluteUri,
						subState.DeploymentProviderUri.AbsoluteUri
					})));
				}
			}
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000031FC File Offset: 0x000021FC
		private void ConsumeUpdatedDeployment(ref SubscriptionState subState, ActivationDescription actDesc)
		{
			AssemblyManifest deployManifest = actDesc.DeployManifest;
			DefinitionIdentity identity = deployManifest.Identity;
			Uri deploySourceUri = actDesc.DeploySourceUri;
			Logger.AddPhaseInformation(Resources.GetString("PhaseLog_ConsumeUpdatedDeployment"));
			if (!actDesc.IsRequiredUpdate)
			{
				Description effectiveDescription = subState.EffectiveDescription;
				UserInterfaceInfo userInterfaceInfo = new UserInterfaceInfo();
				userInterfaceInfo.formTitle = Resources.GetString("UI_UpdateTitle");
				userInterfaceInfo.productName = effectiveDescription.Product;
				userInterfaceInfo.supportUrl = effectiveDescription.SupportUrl;
				userInterfaceInfo.sourceSite = UserInterface.GetDisplaySite(deploySourceUri);
				UserInterfaceModalResult userInterfaceModalResult = this._ui.ShowUpdate(userInterfaceInfo);
				if (userInterfaceModalResult == UserInterfaceModalResult.Skip)
				{
					TimeSpan timeSpan = new TimeSpan(7, 0, 0, 0);
					DateTime dateTime = DateTime.UtcNow + timeSpan;
					this._subStore.SetUpdateSkipTime(subState, identity, dateTime);
					Logger.AddPhaseInformation(Resources.GetString("Upd_DeployUpdateSkipping"));
					return;
				}
				if (userInterfaceModalResult == UserInterfaceModalResult.Cancel)
				{
					return;
				}
			}
			this.InstallApplication(ref subState, actDesc);
			Logger.AddPhaseInformation(Resources.GetString("Upd_Consumed"), new object[]
			{
				identity.ToString(),
				deploySourceUri
			});
		}

		// Token: 0x0600000E RID: 14 RVA: 0x00003304 File Offset: 0x00002304
		private bool InstallApplication(ref SubscriptionState subState, ActivationDescription actDesc)
		{
			bool flag = false;
			Logger.AddPhaseInformation(Resources.GetString("PhaseLog_InstallApplication"));
			this._subStore.CheckDeploymentSubscriptionState(subState, actDesc.DeployManifest);
			long num;
			using (this._subStore.AcquireReferenceTransaction(out num))
			{
				TempDirectory tempDirectory = null;
				try
				{
					flag = this.DownloadApplication(subState, actDesc, num, out tempDirectory);
					actDesc.CommitDeploy = true;
					actDesc.IsConfirmed = true;
					actDesc.TimeStamp = DateTime.UtcNow;
					Logger.AddPhaseInformation(Resources.GetString("PhaseLog_CommitApplication"));
					this._subStore.CommitApplication(ref subState, actDesc);
				}
				finally
				{
					if (tempDirectory != null)
					{
						tempDirectory.Dispose();
					}
				}
			}
			return flag;
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000033BC File Offset: 0x000023BC
		private bool DownloadApplication(SubscriptionState subState, ActivationDescription actDesc, long transactionId, out TempDirectory downloadTemp)
		{
			bool flag = false;
			downloadTemp = this._subStore.AcquireTempDirectory();
			Uri uri;
			string text;
			AssemblyManifest assemblyManifest = DownloadManager.DownloadApplicationManifest(actDesc.DeployManifest, downloadTemp.Path, actDesc.DeploySourceUri, out uri, out text);
			AssemblyManifest.ReValidateManifestSignatures(actDesc.DeployManifest, assemblyManifest);
			if (assemblyManifest.EntryPoints[0].HostInBrowser)
			{
				throw new DeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_HostInBrowserAppNotSupported"));
			}
			if (assemblyManifest.EntryPoints[0].CustomHostSpecified)
			{
				throw new DeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_CustomHostSpecifiedAppNotSupported"));
			}
			if (assemblyManifest.EntryPoints[0].CustomUX && (actDesc.ActType == ActivationType.InstallViaDotApplication || actDesc.ActType == ActivationType.InstallViaFileAssociation || actDesc.ActType == ActivationType.InstallViaShortcut || actDesc.ActType == ActivationType.None))
			{
				throw new DeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_CustomUXAppNotSupported"));
			}
			Logger.AddPhaseInformation(Resources.GetString("PhaseLog_ProcessingApplicationManifestComplete"));
			actDesc.SetApplicationManifest(assemblyManifest, uri, text);
			Logger.SetApplicationManifest(assemblyManifest);
			this._subStore.CheckCustomUXFlag(subState, actDesc.AppManifest);
			actDesc.AppId = new DefinitionAppId(actDesc.ToAppCodebase(), new DefinitionIdentity[]
			{
				actDesc.DeployManifest.Identity,
				actDesc.AppManifest.Identity
			});
			if (assemblyManifest.EntryPoints[0].CustomUX)
			{
				actDesc.Trust = ApplicationTrust.PersistTrustWithoutEvaluation(actDesc.ToActivationContext());
			}
			else
			{
				this._ui.Hide();
				if (this._ui.SplashCancelled())
				{
					throw new DownloadCancelledException();
				}
				if (subState.IsInstalled && !string.Equals(subState.EffectiveCertificatePublicKeyToken, actDesc.EffectiveCertificatePublicKeyToken, StringComparison.Ordinal))
				{
					ApplicationTrust.RemoveCachedTrust(subState.CurrentBind);
				}
				actDesc.Trust = ApplicationTrust.RequestTrust(subState, actDesc.DeployManifest.Deployment.Install, actDesc.IsUpdate, actDesc.ToActivationContext());
			}
			this._fullTrust = actDesc.Trust.DefaultGrantSet.PermissionSet.IsUnrestricted();
			if (!this._fullTrust && actDesc.AppManifest.FileAssociations.Length > 0)
			{
				throw new DeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_FileExtensionNotSupported"));
			}
			PlatformDetector.VerifyPlatformDependencies(actDesc.AppManifest, actDesc.DeployManifest.Description.SupportUri, downloadTemp.Path);
			Logger.AddPhaseInformation(Resources.GetString("PhaseLog_PlatformDetectAndTrustGrantComplete"));
			if (!this._subStore.CheckAndReferenceApplication(subState, actDesc.AppId, transactionId))
			{
				flag = true;
				Description effectiveDescription = actDesc.EffectiveDescription;
				UserInterfaceInfo userInterfaceInfo = new UserInterfaceInfo();
				userInterfaceInfo.productName = effectiveDescription.Product;
				if (actDesc.IsUpdate)
				{
					if (actDesc.IsRequiredUpdate)
					{
						userInterfaceInfo.formTitle = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UI_ProgressTitleRequiredUpdate"), new object[] { userInterfaceInfo.productName });
					}
					else
					{
						userInterfaceInfo.formTitle = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UI_ProgressTitleUpdate"), new object[] { userInterfaceInfo.productName });
					}
				}
				else if (!actDesc.DeployManifest.Deployment.Install)
				{
					userInterfaceInfo.formTitle = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UI_ProgressTitleDownload"), new object[] { userInterfaceInfo.productName });
				}
				else
				{
					userInterfaceInfo.formTitle = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UI_ProgressTitleInstall"), new object[] { userInterfaceInfo.productName });
				}
				userInterfaceInfo.supportUrl = effectiveDescription.SupportUrl;
				userInterfaceInfo.sourceSite = UserInterface.GetDisplaySite(actDesc.DeploySourceUri);
				if (assemblyManifest.Description != null && assemblyManifest.Description.IconFileFS != null)
				{
					userInterfaceInfo.iconFilePath = Path.Combine(downloadTemp.Path, assemblyManifest.Description.IconFileFS);
				}
				ProgressPiece progressPiece = this._ui.ShowProgress(userInterfaceInfo);
				DownloadOptions downloadOptions = null;
				bool flag2 = !actDesc.DeployManifest.Deployment.Install;
				if (!this._fullTrust && flag2)
				{
					downloadOptions = new DownloadOptions();
					downloadOptions.EnforceSizeLimit = true;
					downloadOptions.SizeLimit = this._subStore.GetSizeLimitInBytesForSemiTrustApps();
					downloadOptions.Size = actDesc.DeployManifest.SizeInBytes + actDesc.AppManifest.SizeInBytes;
				}
				DownloadManager.DownloadDependencies(subState, actDesc.DeployManifest, actDesc.AppManifest, actDesc.AppSourceUri, downloadTemp.Path, null, progressPiece, downloadOptions);
				Logger.AddPhaseInformation(Resources.GetString("PhaseLog_DownloadDependenciesComplete"));
				actDesc.CommitApp = true;
				actDesc.AppPayloadPath = downloadTemp.Path;
				actDesc.AppGroup = null;
			}
			return flag;
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00003831 File Offset: 0x00002831
		private static bool SkipUpdate(SubscriptionState subState, DefinitionIdentity targetIdentity)
		{
			return subState.UpdateSkippedDeployment != null && targetIdentity != null && subState.UpdateSkippedDeployment.Equals(targetIdentity) && subState.UpdateSkipTime > DateTime.UtcNow;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x00003861 File Offset: 0x00002861
		private Exception GetInnerMostException(Exception exception)
		{
			if (exception.InnerException != null)
			{
				return this.GetInnerMostException(exception.InnerException);
			}
			return exception;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00003879 File Offset: 0x00002879
		private bool IsWebExceptionInExceptionStack(Exception exception)
		{
			return exception != null && (exception is WebException || this.IsWebExceptionInExceptionStack(exception.InnerException));
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00003898 File Offset: 0x00002898
		private int CheckActivationInProgress(string activationUrl)
		{
			int count;
			lock (ApplicationActivator._activationsInProgress.SyncRoot)
			{
				if (ApplicationActivator._activationsInProgress.Contains(activationUrl))
				{
					ApplicationActivator applicationActivator = (ApplicationActivator)ApplicationActivator._activationsInProgress[activationUrl];
					applicationActivator.ActivateUI();
					this._remActivationInProgressEntry = false;
					throw new DeploymentException(ExceptionTypes.ActivationInProgress, Resources.GetString("Ex_ActivationInProgressException"));
				}
				ApplicationActivator._activationsInProgress.Add(activationUrl, this);
				this._remActivationInProgressEntry = true;
				count = ApplicationActivator._activationsInProgress.Count;
			}
			return count;
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000392C File Offset: 0x0000292C
		private void RemoveActivationInProgressEntry(string activationUrl)
		{
			if (!this._remActivationInProgressEntry)
			{
				return;
			}
			if (activationUrl == null)
			{
				return;
			}
			lock (ApplicationActivator._activationsInProgress.SyncRoot)
			{
				ApplicationActivator._activationsInProgress.Remove(activationUrl);
			}
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000397C File Offset: 0x0000297C
		private void ActivateUI()
		{
			if (this._ui == null)
			{
				return;
			}
			this._ui.Activate();
		}

		// Token: 0x04000001 RID: 1
		private const int _liveActivationLimitUINotVisible = 0;

		// Token: 0x04000002 RID: 2
		private const int _liveActivationLimitUIVisible = 1;

		// Token: 0x04000003 RID: 3
		private const int ActivateArgumentCount = 5;

		// Token: 0x04000004 RID: 4
		private static Hashtable _activationsInProgress = new Hashtable();

		// Token: 0x04000005 RID: 5
		private bool _remActivationInProgressEntry;

		// Token: 0x04000006 RID: 6
		private SubscriptionStore _subStore;

		// Token: 0x04000007 RID: 7
		private UserInterface _ui;

		// Token: 0x04000008 RID: 8
		private bool _fullTrust;

		// Token: 0x04000009 RID: 9
		private static int _liveActivationLimitUIStatus = 0;

		// Token: 0x02000003 RID: 3
		private class BrowserSettings
		{
			// Token: 0x06000018 RID: 24 RVA: 0x000039AC File Offset: 0x000029AC
			public void Validate(string manifestPath)
			{
				AssemblyManifest.CertificateStatus certificateStatus = AssemblyManifest.AnalyzeManifestCertificate(manifestPath);
				if (certificateStatus == AssemblyManifest.CertificateStatus.TrustedPublisher || certificateStatus == AssemblyManifest.CertificateStatus.AuthenticodedNotInTrustedList)
				{
					if (this.ManagedSignedFlag != ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_ALLOW && this.ManagedSignedFlag != ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_QUERY)
					{
						throw new InvalidDeploymentException(ExceptionTypes.Manifest, Resources.GetString("Ex_SignedManifestDisallow"));
					}
				}
				else if (this.ManagedUnSignedFlag != ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_ALLOW && this.ManagedUnSignedFlag != ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_QUERY)
				{
					throw new InvalidDeploymentException(ExceptionTypes.Manifest, Resources.GetString("Ex_UnSignedManifestDisallow"));
				}
			}

			// Token: 0x06000019 RID: 25 RVA: 0x00003A10 File Offset: 0x00002A10
			public static ApplicationActivator.BrowserSettings.ManagedFlags GetManagedFlagValue(int policyValue)
			{
				switch (policyValue)
				{
				case 0:
					return ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_ALLOW;
				case 1:
					return ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_QUERY;
				case 3:
					return ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_DISALLOW;
				}
				return ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_DISALLOW;
			}

			// Token: 0x0400000A RID: 10
			public ApplicationActivator.BrowserSettings.ManagedFlags ManagedSignedFlag = ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_DISALLOW;

			// Token: 0x0400000B RID: 11
			public ApplicationActivator.BrowserSettings.ManagedFlags ManagedUnSignedFlag = ApplicationActivator.BrowserSettings.ManagedFlags.URLPOLICY_DISALLOW;

			// Token: 0x02000004 RID: 4
			public enum ManagedFlags
			{
				// Token: 0x0400000D RID: 13
				URLPOLICY_ALLOW,
				// Token: 0x0400000E RID: 14
				URLPOLICY_QUERY,
				// Token: 0x0400000F RID: 15
				URLPOLICY_DISALLOW = 3
			}
		}
	}
}
