using System;
using System.Deployment.Application.Manifest;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Deployment.Application
{
	// Token: 0x02000045 RID: 69
	[Guid("33246f92-d56f-4e34-837a-9a49bfc91df3")]
	[ClassInterface(ClassInterfaceType.AutoDispatch)]
	[ComVisible(true)]
	[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
	[StructLayout(LayoutKind.Sequential)]
	public class DeploymentServiceCom
	{
		// Token: 0x06000228 RID: 552 RVA: 0x0000DD83 File Offset: 0x0000CD83
		public DeploymentServiceCom()
		{
			LifetimeManager.ExtendLifetime();
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000DD90 File Offset: 0x0000CD90
		public void ActivateDeployment(string deploymentLocation, bool isShortcut)
		{
			new ApplicationActivator().ActivateDeployment(deploymentLocation, isShortcut);
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000DD9E File Offset: 0x0000CD9E
		public void ActivateDeploymentEx(string deploymentLocation, int unsignedPolicy, int signedPolicy)
		{
			new ApplicationActivator().ActivateDeploymentEx(deploymentLocation, unsignedPolicy, signedPolicy);
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000DDAD File Offset: 0x0000CDAD
		public void ActivateApplicationExtension(string textualSubId, string deploymentProviderUrl, string targetAssociatedFile)
		{
			new ApplicationActivator().ActivateApplicationExtension(textualSubId, deploymentProviderUrl, targetAssociatedFile);
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000DDBC File Offset: 0x0000CDBC
		public void MaintainSubscription(string textualSubId)
		{
			LifetimeManager.StartOperation();
			try
			{
				this.MaintainSubscriptionInternal(textualSubId);
			}
			finally
			{
				LifetimeManager.EndOperation();
			}
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0000DDF0 File Offset: 0x0000CDF0
		public void CheckForDeploymentUpdate(string textualSubId)
		{
			LifetimeManager.StartOperation();
			try
			{
				this.CheckForDeploymentUpdateInternal(textualSubId);
			}
			finally
			{
				LifetimeManager.EndOperation();
			}
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0000DE24 File Offset: 0x0000CE24
		public void EndServiceRightNow()
		{
			LifetimeManager.EndImmediately();
		}

		// Token: 0x0600022F RID: 559 RVA: 0x0000DE2C File Offset: 0x0000CE2C
		public void CleanOnlineAppCache()
		{
			LifetimeManager.StartOperation();
			try
			{
				this.CleanOnlineAppCacheInternal();
			}
			finally
			{
				LifetimeManager.EndOperation();
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000DE5C File Offset: 0x0000CE5C
		private void MaintainSubscriptionInternal(string textualSubId)
		{
			bool flag = false;
			string[] array = new string[] { "Maintain_Exception", "Maintain_Completed", "Maintain_Failed", "Maintain_FailedMsg" };
			bool flag2 = false;
			Exception ex = null;
			bool flag3 = false;
			bool flag4 = false;
			string @string = Resources.GetString("ErrorMessage_GenericLinkUrlMessage");
			string text = null;
			string text2 = null;
			Logger.StartCurrentThreadLogging();
			Logger.SetTextualSubscriptionIdentity(textualSubId);
			using (UserInterface userInterface = new UserInterface())
			{
				MaintenanceInfo maintenanceInfo = new MaintenanceInfo();
				try
				{
					UserInterfaceInfo userInterfaceInfo = new UserInterfaceInfo();
					Logger.AddPhaseInformation(Resources.GetString("PhaseLog_StoreQueryForMaintenanceInfo"));
					SubscriptionState subscriptionState = this.GetSubscriptionState(textualSubId);
					try
					{
						subscriptionState.SubscriptionStore.CheckInstalledAndShellVisible(subscriptionState);
						if (subscriptionState.RollbackDeployment == null)
						{
							maintenanceInfo.maintenanceFlags |= MaintenanceFlags.RemoveSelected;
						}
						else
						{
							maintenanceInfo.maintenanceFlags |= MaintenanceFlags.RestorationPossible;
							maintenanceInfo.maintenanceFlags |= MaintenanceFlags.RestoreSelected;
						}
						AssemblyManifest currentDeploymentManifest = subscriptionState.CurrentDeploymentManifest;
						if (currentDeploymentManifest != null && currentDeploymentManifest.Description != null)
						{
							text2 = currentDeploymentManifest.Description.ErrorReportUrl;
						}
						Description effectiveDescription = subscriptionState.EffectiveDescription;
						userInterfaceInfo.productName = effectiveDescription.Product;
						userInterfaceInfo.supportUrl = effectiveDescription.SupportUrl;
						userInterfaceInfo.formTitle = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("UI_MaintenanceTitle"), new object[] { userInterfaceInfo.productName });
						flag3 = true;
					}
					catch (DeploymentException ex2)
					{
						flag3 = false;
						Logger.AddErrorInformation(Resources.GetString("MaintainLogMsg_FailedStoreLookup"), ex2);
						maintenanceInfo.maintenanceFlags |= MaintenanceFlags.RemoveSelected;
					}
					catch (FormatException ex3)
					{
						flag3 = false;
						Logger.AddErrorInformation(Resources.GetString("MaintainLogMsg_FailedStoreLookup"), ex3);
						maintenanceInfo.maintenanceFlags |= MaintenanceFlags.RemoveSelected;
					}
					bool flag5 = false;
					if (flag3)
					{
						if (userInterface.ShowMaintenance(userInterfaceInfo, maintenanceInfo) == UserInterfaceModalResult.Ok)
						{
							flag5 = true;
						}
					}
					else
					{
						maintenanceInfo.maintenanceFlags = MaintenanceFlags.RemoveSelected;
						flag5 = true;
					}
					if (flag5)
					{
						flag2 = true;
						if ((maintenanceInfo.maintenanceFlags & MaintenanceFlags.RestoreSelected) != MaintenanceFlags.ClearFlag)
						{
							array = new string[] { "Rollback_Exception", "Rollback_Completed", "Rollback_Failed", "Rollback_FailedMsg" };
							subscriptionState.SubscriptionStore.RollbackSubscription(subscriptionState);
							flag2 = false;
							userInterface.ShowMessage(Resources.GetString("UI_RollbackCompletedMsg"), Resources.GetString("UI_RollbackCompletedTitle"));
						}
						else if ((maintenanceInfo.maintenanceFlags & MaintenanceFlags.RemoveSelected) != MaintenanceFlags.ClearFlag)
						{
							array = new string[] { "Uninstall_Exception", "Uninstall_Completed", "Uninstall_Failed", "Uninstall_FailedMsg" };
							try
							{
								subscriptionState.SubscriptionStore.UninstallSubscription(subscriptionState);
								flag2 = false;
							}
							catch (DeploymentException ex4)
							{
								Logger.AddErrorInformation(Resources.GetString("MaintainLogMsg_UninstallFailed"), ex4);
								flag4 = true;
								ShellExposure.RemoveSubscriptionShellExposure(subscriptionState);
								flag4 = false;
							}
						}
						flag = true;
					}
				}
				catch (DeploymentException ex5)
				{
					Logger.AddErrorInformation(ex5, Resources.GetString(array[0]), new object[] { textualSubId });
					ex = ex5;
				}
				finally
				{
					Logger.AddPhaseInformation(Resources.GetString(flag ? array[1] : array[2]), new object[] { textualSubId });
					if (((maintenanceInfo.maintenanceFlags & MaintenanceFlags.RestoreSelected) != MaintenanceFlags.ClearFlag && flag2) || ((maintenanceInfo.maintenanceFlags & MaintenanceFlags.RemoveSelected) != MaintenanceFlags.ClearFlag && flag4 && flag2))
					{
						string text3 = Logger.GetLogFilePath();
						if (!Logger.FlushCurrentThreadLogs())
						{
							text3 = null;
						}
						if (text2 != null && ex != null)
						{
							Exception innerMostException = this.GetInnerMostException(ex);
							text = string.Format("{0}?outer={1}&&inner={2}&&msg={3}", new object[]
							{
								text2,
								ex.GetType().ToString(),
								innerMostException.GetType().ToString(),
								innerMostException.Message
							});
							if (text.Length > 2048)
							{
								text = text.Substring(0, 2048);
							}
						}
						userInterface.ShowError(Resources.GetString("UI_MaintenceErrorTitle"), Resources.GetString(array[3]), text3, text, @string);
					}
					Logger.EndCurrentThreadLogging();
				}
			}
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000E2CC File Offset: 0x0000D2CC
		private void CheckForDeploymentUpdateInternal(string textualSubId)
		{
			bool flag = false;
			Logger.StartCurrentThreadLogging();
			Logger.SetTextualSubscriptionIdentity(textualSubId);
			try
			{
				SubscriptionState shellVisibleSubscriptionState = this.GetShellVisibleSubscriptionState(textualSubId);
				shellVisibleSubscriptionState.SubscriptionStore.CheckForDeploymentUpdate(shellVisibleSubscriptionState);
				flag = true;
			}
			catch (DeploymentException ex)
			{
				Logger.AddErrorInformation(Resources.GetString("Upd_Exception"), ex);
			}
			finally
			{
				Logger.AddPhaseInformation(Resources.GetString(flag ? "Upd_Completed" : "Upd_Failed"));
				Logger.EndCurrentThreadLogging();
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000E350 File Offset: 0x0000D350
		private void CleanOnlineAppCacheInternal()
		{
			bool flag = false;
			Logger.StartCurrentThreadLogging();
			try
			{
				SubscriptionStore.CurrentUser.CleanOnlineAppCache();
				flag = true;
			}
			catch (Exception ex)
			{
				Logger.AddErrorInformation(Resources.GetString("Ex_CleanOnlineAppCache"), ex);
				throw;
			}
			finally
			{
				Logger.AddPhaseInformation(Resources.GetString(flag ? "CleanOnlineCache_Completed" : "CleanOnlineCache_Failed"));
				Logger.EndCurrentThreadLogging();
			}
		}

		// Token: 0x06000233 RID: 563 RVA: 0x0000E3C4 File Offset: 0x0000D3C4
		private SubscriptionState GetShellVisibleSubscriptionState(string textualSubId)
		{
			SubscriptionState subscriptionState = this.GetSubscriptionState(textualSubId);
			subscriptionState.SubscriptionStore.CheckInstalledAndShellVisible(subscriptionState);
			return subscriptionState;
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000E3E8 File Offset: 0x0000D3E8
		private SubscriptionState GetSubscriptionState(string textualSubId)
		{
			if (textualSubId == null)
			{
				throw new ArgumentNullException("textualSubId", Resources.GetString("Ex_ComArgSubIdentityNull"));
			}
			DefinitionIdentity definitionIdentity = null;
			try
			{
				definitionIdentity = new DefinitionIdentity(textualSubId);
			}
			catch (COMException ex)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentCulture, Resources.GetString("Ex_ComArgSubIdentityNotValid"), new object[] { textualSubId }), ex);
			}
			catch (SEHException ex2)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentCulture, Resources.GetString("Ex_ComArgSubIdentityNotValid"), new object[] { textualSubId }), ex2);
			}
			if (definitionIdentity.Version != null)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, Resources.GetString("Ex_ComArgSubIdentityWithVersion"));
			}
			SubscriptionStore currentUser = SubscriptionStore.CurrentUser;
			currentUser.RefreshStorePointer();
			return currentUser.GetSubscriptionState(definitionIdentity);
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000E4C0 File Offset: 0x0000D4C0
		private Exception GetInnerMostException(Exception exception)
		{
			if (exception.InnerException != null)
			{
				return this.GetInnerMostException(exception.InnerException);
			}
			return exception;
		}
	}
}
