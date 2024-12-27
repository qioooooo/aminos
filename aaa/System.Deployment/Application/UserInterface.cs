using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;

namespace System.Deployment.Application
{
	// Token: 0x020000D1 RID: 209
	internal class UserInterface : IDisposable
	{
		// Token: 0x06000578 RID: 1400 RVA: 0x0001D6BC File Offset: 0x0001C6BC
		public UserInterface(bool wait)
		{
			this._splashInfo = new SplashInfo();
			this._splashInfo.initializedAsWait = wait;
			this._uiThread = new Thread(new ThreadStart(this.UIThread));
			this._uiThread.Name = "UIThread";
			this._uiThread.Start();
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x0001D73C File Offset: 0x0001C73C
		public UserInterface()
			: this(true)
		{
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x0001D745 File Offset: 0x0001C745
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x0001D754 File Offset: 0x0001C754
		public ProgressPiece ShowProgress(UserInterfaceInfo info)
		{
			this.WaitReady();
			return (ProgressPiece)this._uiForm.Invoke(new UserInterfaceForm.ConstructProgressPieceDelegate(this._uiForm.ConstructProgressPiece), new object[] { info });
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x0001D794 File Offset: 0x0001C794
		public UserInterfaceModalResult ShowUpdate(UserInterfaceInfo info)
		{
			this.WaitReady();
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			UpdatePiece updatePiece = (UpdatePiece)this._uiForm.Invoke(new UserInterfaceForm.ConstructUpdatePieceDelegate(this._uiForm.ConstructUpdatePiece), new object[] { info, manualResetEvent });
			manualResetEvent.WaitOne();
			return updatePiece.ModalResult;
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x0001D7F0 File Offset: 0x0001C7F0
		public UserInterfaceModalResult ShowMaintenance(UserInterfaceInfo info, MaintenanceInfo maintenanceInfo)
		{
			this.WaitReady();
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			MaintenancePiece maintenancePiece = (MaintenancePiece)this._uiForm.Invoke(new UserInterfaceForm.ConstructMaintenancePieceDelegate(this._uiForm.ConstructMaintenancePiece), new object[] { info, maintenanceInfo, manualResetEvent });
			manualResetEvent.WaitOne();
			return maintenancePiece.ModalResult;
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x0001D850 File Offset: 0x0001C850
		public void ShowMessage(string message, string caption)
		{
			this.WaitReady();
			this._uiForm.Invoke(new UserInterfaceForm.ShowSimpleMessageBoxDelegate(this._uiForm.ShowSimpleMessageBox), new object[] { message, caption });
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x0001D890 File Offset: 0x0001C890
		public void ShowPlatform(string platformDetectionErrorMsg, Uri supportUrl)
		{
			this.WaitReady();
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			this._uiForm.BeginInvoke(new UserInterfaceForm.ConstructPlatformPieceDelegate(this._uiForm.ConstructPlatformPiece), new object[] { platformDetectionErrorMsg, supportUrl, manualResetEvent });
			manualResetEvent.WaitOne();
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x0001D8E4 File Offset: 0x0001C8E4
		public void ShowError(string title, string message, string logFileLocation, string linkUrl, string linkUrlMessage)
		{
			this.WaitReady();
			ManualResetEvent manualResetEvent = new ManualResetEvent(false);
			this._uiForm.BeginInvoke(new UserInterfaceForm.ConstructErrorPieceDelegate(this._uiForm.ConstructErrorPiece), new object[] { title, message, logFileLocation, linkUrl, linkUrlMessage, manualResetEvent });
			manualResetEvent.WaitOne();
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x0001D944 File Offset: 0x0001C944
		public void Hide()
		{
			this.WaitReady();
			this._uiForm.BeginInvoke(new MethodInvoker(this._uiForm.Hide));
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x0001D969 File Offset: 0x0001C969
		public void Activate()
		{
			this.WaitReady();
			this._uiForm.BeginInvoke(new MethodInvoker(this._uiForm.Activate));
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x0001D98E File Offset: 0x0001C98E
		public bool SplashCancelled()
		{
			return this._splashInfo.cancelled;
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x0001D99B File Offset: 0x0001C99B
		private void WaitReady()
		{
			this._uiConstructed.WaitOne();
			this._uiReady.WaitOne();
			this._splashInfo.pieceReady.WaitOne();
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x0001D9C8 File Offset: 0x0001C9C8
		private void UIThread()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			using (this._uiForm = new UserInterfaceForm(this._uiReady, this._splashInfo))
			{
				this._uiConstructed.Set();
				this._appctx = new ApplicationContext(this._uiForm);
				Application.Run(this._appctx);
				this._appctxExitThreadFinished.WaitOne();
				Application.ExitThread();
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x0001DA50 File Offset: 0x0001CA50
		private void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this.WaitReady();
					this._appctx.ExitThread();
					this._appctxExitThreadFinished.Set();
				}
				this._disposed = true;
			}
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0001DA84 File Offset: 0x0001CA84
		public static string GetDisplaySite(Uri sourceUri)
		{
			string text = null;
			if (sourceUri.IsUnc)
			{
				try
				{
					return Path.GetDirectoryName(sourceUri.LocalPath);
				}
				catch (ArgumentException)
				{
					return text;
				}
			}
			text = sourceUri.Host;
			if (string.IsNullOrEmpty(text))
			{
				try
				{
					text = Path.GetDirectoryName(sourceUri.LocalPath);
				}
				catch (ArgumentException)
				{
				}
			}
			return text;
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x0001DAEC File Offset: 0x0001CAEC
		public static string LimitDisplayTextLength(string displayText)
		{
			if (displayText.Length > 50)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(displayText, 0, 47);
				stringBuilder.Append("...");
				return stringBuilder.ToString();
			}
			return displayText;
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0001DB28 File Offset: 0x0001CB28
		public static bool IsValidHttpUrl(string url)
		{
			bool flag = false;
			if (url != null && url.Length > 0 && (url.StartsWith(Uri.UriSchemeHttp + Uri.SchemeDelimiter, StringComparison.Ordinal) || url.StartsWith(Uri.UriSchemeHttps + Uri.SchemeDelimiter, StringComparison.Ordinal)))
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0001DB78 File Offset: 0x0001CB78
		public static void LaunchUrlInBrowser(string url)
		{
			try
			{
				Process.Start(UserInterface.DefaultBrowserExePath, url);
			}
			catch (Win32Exception)
			{
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x0600058B RID: 1419 RVA: 0x0001DBA8 File Offset: 0x0001CBA8
		private static string DefaultBrowserExePath
		{
			get
			{
				string text = null;
				RegistryKey registryKey = Registry.ClassesRoot.OpenSubKey("http\\shell\\open\\command");
				if (registryKey != null)
				{
					string text2 = (string)registryKey.GetValue(string.Empty);
					if (text2 != null)
					{
						text2 = text2.Trim();
						if (text2.Length != 0)
						{
							if (text2[0] == '"')
							{
								int num = text2.IndexOf('"', 1);
								if (num != -1)
								{
									text = text2.Substring(1, num - 1);
								}
							}
							else
							{
								int num2 = text2.IndexOf(' ');
								if (num2 != -1)
								{
									text = text2.Substring(0, num2);
								}
								else
								{
									text = text2;
								}
							}
						}
					}
				}
				return text;
			}
		}

		// Token: 0x0400048C RID: 1164
		private UserInterfaceForm _uiForm;

		// Token: 0x0400048D RID: 1165
		private ApplicationContext _appctx;

		// Token: 0x0400048E RID: 1166
		private ManualResetEvent _appctxExitThreadFinished = new ManualResetEvent(false);

		// Token: 0x0400048F RID: 1167
		private Thread _uiThread;

		// Token: 0x04000490 RID: 1168
		private ManualResetEvent _uiConstructed = new ManualResetEvent(false);

		// Token: 0x04000491 RID: 1169
		private ManualResetEvent _uiReady = new ManualResetEvent(false);

		// Token: 0x04000492 RID: 1170
		private SplashInfo _splashInfo;

		// Token: 0x04000493 RID: 1171
		private bool _disposed;
	}
}
