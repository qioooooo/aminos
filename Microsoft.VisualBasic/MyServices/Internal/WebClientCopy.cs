using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Microsoft.VisualBasic.MyServices.Internal
{
	internal class WebClientCopy
	{
		public WebClientCopy(WebClient client, ProgressDialog dialog)
		{
			this.m_Percentage = 0;
			this.m_WebClient = client;
			this.m_ProgressDialog = dialog;
		}

		public void DownloadFile(Uri address, string destinationFileName)
		{
			if (this.m_ProgressDialog != null)
			{
				this.m_WebClient.DownloadFileAsync(address, destinationFileName);
				this.m_ProgressDialog.ShowProgressDialog();
			}
			else
			{
				this.m_WebClient.DownloadFile(address, destinationFileName);
			}
			if (this.m_ExceptionEncounteredDuringFileTransfer != null && (this.m_ProgressDialog == null || !this.m_ProgressDialog.UserCanceledTheDialog))
			{
				throw this.m_ExceptionEncounteredDuringFileTransfer;
			}
		}

		public void UploadFile(string sourceFileName, Uri address)
		{
			if (this.m_ProgressDialog != null)
			{
				this.m_WebClient.UploadFileAsync(address, sourceFileName);
				this.m_ProgressDialog.ShowProgressDialog();
			}
			else
			{
				this.m_WebClient.UploadFile(address, sourceFileName);
			}
			if (this.m_ExceptionEncounteredDuringFileTransfer != null && (this.m_ProgressDialog == null || !this.m_ProgressDialog.UserCanceledTheDialog))
			{
				throw this.m_ExceptionEncounteredDuringFileTransfer;
			}
		}

		private void InvokeIncrement(int progressPercentage)
		{
			if (this.m_ProgressDialog != null && this.m_ProgressDialog.IsHandleCreated)
			{
				int num = checked(progressPercentage - this.m_Percentage);
				this.m_Percentage = progressPercentage;
				if (num > 0)
				{
					this.m_ProgressDialog.BeginInvoke(new WebClientCopy.DoIncrement(this.m_ProgressDialog.Increment), new object[] { num });
				}
			}
		}

		private void CloseProgressDialog()
		{
			if (this.m_ProgressDialog != null)
			{
				this.m_ProgressDialog.IndicateClosing();
				if (this.m_ProgressDialog.IsHandleCreated)
				{
					this.m_ProgressDialog.BeginInvoke(new MethodInvoker(this.m_ProgressDialog.CloseDialog));
				}
				else
				{
					this.m_ProgressDialog.Close();
				}
			}
		}

		private void m_WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			try
			{
				if (e.Error != null)
				{
					this.m_ExceptionEncounteredDuringFileTransfer = e.Error;
				}
				if (!e.Cancelled && e.Error == null)
				{
					this.InvokeIncrement(100);
				}
			}
			finally
			{
				this.CloseProgressDialog();
			}
		}

		private void m_WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.InvokeIncrement(e.ProgressPercentage);
		}

		private void m_WebClient_UploadFileCompleted(object sender, UploadFileCompletedEventArgs e)
		{
			try
			{
				if (e.Error != null)
				{
					this.m_ExceptionEncounteredDuringFileTransfer = e.Error;
				}
				if (!e.Cancelled && e.Error == null)
				{
					this.InvokeIncrement(100);
				}
			}
			finally
			{
				this.CloseProgressDialog();
			}
		}

		private void m_WebClient_UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
		{
			checked
			{
				long num = e.BytesSent * 100L / e.TotalBytesToSend;
				this.InvokeIncrement((int)num);
			}
		}

		private void m_ProgressDialog_UserCancelledEvent()
		{
			this.m_WebClient.CancelAsync();
		}

		private virtual WebClient m_WebClient
		{
			get
			{
				return this._m_WebClient;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				if (this._m_WebClient != null)
				{
					this._m_WebClient.DownloadFileCompleted -= this.m_WebClient_DownloadFileCompleted;
					this._m_WebClient.UploadProgressChanged -= this.m_WebClient_UploadProgressChanged;
					this._m_WebClient.UploadFileCompleted -= this.m_WebClient_UploadFileCompleted;
					this._m_WebClient.DownloadProgressChanged -= this.m_WebClient_DownloadProgressChanged;
				}
				this._m_WebClient = value;
				if (this._m_WebClient != null)
				{
					this._m_WebClient.DownloadFileCompleted += this.m_WebClient_DownloadFileCompleted;
					this._m_WebClient.UploadProgressChanged += this.m_WebClient_UploadProgressChanged;
					this._m_WebClient.UploadFileCompleted += this.m_WebClient_UploadFileCompleted;
					this._m_WebClient.DownloadProgressChanged += this.m_WebClient_DownloadProgressChanged;
				}
			}
		}

		private virtual ProgressDialog m_ProgressDialog
		{
			get
			{
				return this._m_ProgressDialog;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				if (this._m_ProgressDialog != null)
				{
					this._m_ProgressDialog.UserHitCancel -= this.m_ProgressDialog_UserCancelledEvent;
				}
				this._m_ProgressDialog = value;
				if (this._m_ProgressDialog != null)
				{
					this._m_ProgressDialog.UserHitCancel += this.m_ProgressDialog_UserCancelledEvent;
				}
			}
		}

		[AccessedThroughProperty("m_WebClient")]
		private WebClient _m_WebClient;

		[AccessedThroughProperty("m_ProgressDialog")]
		private ProgressDialog _m_ProgressDialog;

		private Exception m_ExceptionEncounteredDuringFileTransfer;

		private int m_Percentage;

		private delegate void DoIncrement(int Increment);
	}
}
