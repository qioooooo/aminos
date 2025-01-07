using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using Microsoft.VisualBasic.CompilerServices;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualBasic.MyServices.Internal;

namespace Microsoft.VisualBasic.Devices
{
	[HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
	public class Network
	{
		public event NetworkAvailableEventHandler NetworkAvailabilityChanged
		{
			add
			{
				try
				{
					this.m_Connected = this.IsAvailable;
				}
				catch (SecurityException ex)
				{
					return;
				}
				catch (PlatformNotSupportedException ex2)
				{
					return;
				}
				object syncObject = this.m_SyncObject;
				ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
				lock (syncObject)
				{
					if (this.m_NetworkAvailabilityEventHandlers == null)
					{
						this.m_NetworkAvailabilityEventHandlers = new ArrayList();
					}
					this.m_NetworkAvailabilityEventHandlers.Add(value);
					if (this.m_NetworkAvailabilityEventHandlers.Count == 1)
					{
						this.m_NetworkAvailabilityChangedCallback = new SendOrPostCallback(this.NetworkAvailabilityChangedHandler);
						if (AsyncOperationManager.SynchronizationContext != null)
						{
							this.m_SynchronizationContext = AsyncOperationManager.SynchronizationContext;
							try
							{
								NetworkChange.NetworkAddressChanged += this.OS_NetworkAvailabilityChangedListener;
							}
							catch (PlatformNotSupportedException ex3)
							{
							}
							catch (NetworkInformationException ex4)
							{
							}
						}
					}
				}
			}
			remove
			{
				if (this.m_NetworkAvailabilityEventHandlers != null && this.m_NetworkAvailabilityEventHandlers.Count > 0)
				{
					this.m_NetworkAvailabilityEventHandlers.Remove(value);
					if (this.m_NetworkAvailabilityEventHandlers.Count == 0)
					{
						NetworkChange.NetworkAddressChanged -= this.OS_NetworkAvailabilityChangedListener;
						this.DisconnectListener();
					}
				}
			}
		}

		// Note: this method is marked as 'fire'.
		private void raise_NetworkAvailabilityChanged(object sender, NetworkAvailableEventArgs e)
		{
			if (this.m_NetworkAvailabilityEventHandlers != null)
			{
				try
				{
					foreach (object obj in this.m_NetworkAvailabilityEventHandlers)
					{
						NetworkAvailableEventHandler networkAvailableEventHandler = (NetworkAvailableEventHandler)obj;
						if (networkAvailableEventHandler != null)
						{
							networkAvailableEventHandler(sender, e);
						}
					}
				}
				finally
				{
					IEnumerator enumerator;
					if (enumerator is IDisposable)
					{
						(enumerator as IDisposable).Dispose();
					}
				}
			}
		}

		public Network()
		{
			this.m_SyncObject = new object();
		}

		public bool IsAvailable
		{
			get
			{
				return NetworkInterface.GetIsNetworkAvailable();
			}
		}

		public bool Ping(string hostNameOrAddress)
		{
			return this.Ping(hostNameOrAddress, 1000);
		}

		public bool Ping(Uri address)
		{
			if (address == null)
			{
				throw ExceptionUtils.GetArgumentNullException("address");
			}
			return this.Ping(address.Host, 1000);
		}

		public bool Ping(string hostNameOrAddress, int timeout)
		{
			if (!this.IsAvailable)
			{
				throw ExceptionUtils.GetInvalidOperationException("Network_NetworkNotAvailable", new string[0]);
			}
			Ping ping = new Ping();
			PingReply pingReply = ping.Send(hostNameOrAddress, timeout, this.PingBuffer);
			return pingReply.Status == IPStatus.Success;
		}

		public bool Ping(Uri address, int timeout)
		{
			if (address == null)
			{
				throw ExceptionUtils.GetArgumentNullException("address");
			}
			return this.Ping(address.Host, timeout);
		}

		public void DownloadFile(string address, string destinationFileName)
		{
			this.DownloadFile(address, destinationFileName, "", "", false, 100000, false);
		}

		public void DownloadFile(Uri address, string destinationFileName)
		{
			this.DownloadFile(address, destinationFileName, "", "", false, 100000, false);
		}

		public void DownloadFile(string address, string destinationFileName, string userName, string password)
		{
			this.DownloadFile(address, destinationFileName, userName, password, false, 100000, false);
		}

		public void DownloadFile(Uri address, string destinationFileName, string userName, string password)
		{
			this.DownloadFile(address, destinationFileName, userName, password, false, 100000, false);
		}

		public void DownloadFile(string address, string destinationFileName, string userName, string password, bool showUI, int connectionTimeout, bool overwrite)
		{
			this.DownloadFile(address, destinationFileName, userName, password, showUI, connectionTimeout, overwrite, UICancelOption.ThrowException);
		}

		public void DownloadFile(string address, string destinationFileName, string userName, string password, bool showUI, int connectionTimeout, bool overwrite, UICancelOption onUserCancel)
		{
			if (string.IsNullOrEmpty(address) || Operators.CompareString(address.Trim(), "", false) == 0)
			{
				throw ExceptionUtils.GetArgumentNullException("address");
			}
			Uri uri = this.GetUri(address.Trim());
			ICredentials networkCredentials = this.GetNetworkCredentials(userName, password);
			this.DownloadFile(uri, destinationFileName, networkCredentials, showUI, connectionTimeout, overwrite, onUserCancel);
		}

		public void DownloadFile(Uri address, string destinationFileName, string userName, string password, bool showUI, int connectionTimeout, bool overwrite)
		{
			this.DownloadFile(address, destinationFileName, userName, password, showUI, connectionTimeout, overwrite, UICancelOption.ThrowException);
		}

		public void DownloadFile(Uri address, string destinationFileName, string userName, string password, bool showUI, int connectionTimeout, bool overwrite, UICancelOption onUserCancel)
		{
			ICredentials networkCredentials = this.GetNetworkCredentials(userName, password);
			this.DownloadFile(address, destinationFileName, networkCredentials, showUI, connectionTimeout, overwrite, onUserCancel);
		}

		public void DownloadFile(Uri address, string destinationFileName, ICredentials networkCredentials, bool showUI, int connectionTimeout, bool overwrite)
		{
			this.DownloadFile(address, destinationFileName, networkCredentials, showUI, connectionTimeout, overwrite, UICancelOption.ThrowException);
		}

		public void DownloadFile(Uri address, string destinationFileName, ICredentials networkCredentials, bool showUI, int connectionTimeout, bool overwrite, UICancelOption onUserCancel)
		{
			if (connectionTimeout <= 0)
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("connectionTimeOut", "Network_BadConnectionTimeout", new string[0]);
			}
			if (address == null)
			{
				throw ExceptionUtils.GetArgumentNullException("address");
			}
			using (WebClientExtended webClientExtended = new WebClientExtended())
			{
				webClientExtended.Timeout = connectionTimeout;
				webClientExtended.UseNonPassiveFtp = showUI;
				string text = FileSystem.NormalizeFilePath(destinationFileName, "destinationFileName");
				if (Directory.Exists(text))
				{
					throw ExceptionUtils.GetInvalidOperationException("Network_DownloadNeedsFilename", new string[0]);
				}
				if (File.Exists(text) & !overwrite)
				{
					throw new IOException(Utils.GetResourceString("IO_FileExists_Path", new string[] { destinationFileName }));
				}
				if (networkCredentials != null)
				{
					webClientExtended.Credentials = networkCredentials;
				}
				ProgressDialog progressDialog = null;
				if (showUI && Environment.UserInteractive)
				{
					UIPermission uipermission = new UIPermission(UIPermissionWindow.SafeSubWindows);
					uipermission.Demand();
					progressDialog = new ProgressDialog();
					progressDialog.Text = Utils.GetResourceString("ProgressDialogDownloadingTitle", new string[] { address.AbsolutePath });
					progressDialog.LabelText = Utils.GetResourceString("ProgressDialogDownloadingLabel", new string[] { address.AbsolutePath, text });
				}
				string directoryName = Path.GetDirectoryName(text);
				if (Operators.CompareString(directoryName, "", false) == 0)
				{
					throw ExceptionUtils.GetInvalidOperationException("Network_DownloadNeedsFilename", new string[0]);
				}
				if (!Directory.Exists(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				WebClientCopy webClientCopy = new WebClientCopy(webClientExtended, progressDialog);
				webClientCopy.DownloadFile(address, text);
				if (showUI && Environment.UserInteractive && ((onUserCancel == UICancelOption.ThrowException) & progressDialog.UserCanceledTheDialog))
				{
					throw new OperationCanceledException();
				}
			}
		}

		public void UploadFile(string sourceFileName, string address)
		{
			this.UploadFile(sourceFileName, address, "", "", false, 100000);
		}

		public void UploadFile(string sourceFileName, Uri address)
		{
			this.UploadFile(sourceFileName, address, "", "", false, 100000);
		}

		public void UploadFile(string sourceFileName, string address, string userName, string password)
		{
			this.UploadFile(sourceFileName, address, userName, password, false, 100000);
		}

		public void UploadFile(string sourceFileName, Uri address, string userName, string password)
		{
			this.UploadFile(sourceFileName, address, userName, password, false, 100000);
		}

		public void UploadFile(string sourceFileName, string address, string userName, string password, bool showUI, int connectionTimeout)
		{
			this.UploadFile(sourceFileName, address, userName, password, showUI, connectionTimeout, UICancelOption.ThrowException);
		}

		public void UploadFile(string sourceFileName, string address, string userName, string password, bool showUI, int connectionTimeout, UICancelOption onUserCancel)
		{
			if (string.IsNullOrEmpty(address) || Operators.CompareString(address.Trim(), "", false) == 0)
			{
				throw ExceptionUtils.GetArgumentNullException("address");
			}
			Uri uri = this.GetUri(address.Trim());
			if (Operators.CompareString(Path.GetFileName(uri.AbsolutePath), "", false) == 0)
			{
				throw ExceptionUtils.GetInvalidOperationException("Network_UploadAddressNeedsFilename", new string[0]);
			}
			this.UploadFile(sourceFileName, uri, userName, password, showUI, connectionTimeout, onUserCancel);
		}

		public void UploadFile(string sourceFileName, Uri address, string userName, string password, bool showUI, int connectionTimeout)
		{
			this.UploadFile(sourceFileName, address, userName, password, showUI, connectionTimeout, UICancelOption.ThrowException);
		}

		public void UploadFile(string sourceFileName, Uri address, string userName, string password, bool showUI, int connectionTimeout, UICancelOption onUserCancel)
		{
			ICredentials networkCredentials = this.GetNetworkCredentials(userName, password);
			this.UploadFile(sourceFileName, address, networkCredentials, showUI, connectionTimeout, onUserCancel);
		}

		public void UploadFile(string sourceFileName, Uri address, ICredentials networkCredentials, bool showUI, int connectionTimeout)
		{
			this.UploadFile(sourceFileName, address, networkCredentials, showUI, connectionTimeout, UICancelOption.ThrowException);
		}

		public void UploadFile(string sourceFileName, Uri address, ICredentials networkCredentials, bool showUI, int connectionTimeout, UICancelOption onUserCancel)
		{
			sourceFileName = FileSystem.NormalizeFilePath(sourceFileName, "sourceFileName");
			if (!File.Exists(sourceFileName))
			{
				throw new FileNotFoundException(Utils.GetResourceString("IO_FileNotFound_Path", new string[] { sourceFileName }));
			}
			if (connectionTimeout <= 0)
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("connectionTimeout", "Network_BadConnectionTimeout", new string[0]);
			}
			if (address == null)
			{
				throw ExceptionUtils.GetArgumentNullException("address");
			}
			using (WebClientExtended webClientExtended = new WebClientExtended())
			{
				webClientExtended.Timeout = connectionTimeout;
				if (networkCredentials != null)
				{
					webClientExtended.Credentials = networkCredentials;
				}
				ProgressDialog progressDialog = null;
				if (showUI && Environment.UserInteractive)
				{
					progressDialog = new ProgressDialog();
					progressDialog.Text = Utils.GetResourceString("ProgressDialogUploadingTitle", new string[] { sourceFileName });
					progressDialog.LabelText = Utils.GetResourceString("ProgressDialogUploadingLabel", new string[] { sourceFileName, address.AbsolutePath });
				}
				WebClientCopy webClientCopy = new WebClientCopy(webClientExtended, progressDialog);
				webClientCopy.UploadFile(sourceFileName, address);
				if (showUI && Environment.UserInteractive && ((onUserCancel == UICancelOption.ThrowException) & progressDialog.UserCanceledTheDialog))
				{
					throw new OperationCanceledException();
				}
			}
		}

		internal void DisconnectListener()
		{
			NetworkChange.NetworkAddressChanged -= this.OS_NetworkAvailabilityChangedListener;
		}

		private void OS_NetworkAvailabilityChangedListener(object sender, EventArgs e)
		{
			object syncObject = this.m_SyncObject;
			ObjectFlowControl.CheckForSyncLockOnValueType(syncObject);
			lock (syncObject)
			{
				this.m_SynchronizationContext.Post(this.m_NetworkAvailabilityChangedCallback, null);
			}
		}

		private void NetworkAvailabilityChangedHandler(object state)
		{
			bool isAvailable = this.IsAvailable;
			if (this.m_Connected != isAvailable)
			{
				this.m_Connected = isAvailable;
				this.raise_NetworkAvailabilityChanged(this, new NetworkAvailableEventArgs(isAvailable));
			}
		}

		private byte[] PingBuffer
		{
			get
			{
				checked
				{
					if (this.m_PingBuffer == null)
					{
						this.m_PingBuffer = new byte[32];
						int num = 0;
						do
						{
							this.m_PingBuffer[num] = Convert.ToByte(97 + num % 23, CultureInfo.InvariantCulture);
							num++;
						}
						while (num <= 31);
					}
					return this.m_PingBuffer;
				}
			}
		}

		private Uri GetUri(string address)
		{
			Uri uri;
			try
			{
				uri = new Uri(address);
			}
			catch (UriFormatException ex)
			{
				throw ExceptionUtils.GetArgumentExceptionWithArgName("address", "Network_InvalidUriString", new string[] { address });
			}
			return uri;
		}

		private ICredentials GetNetworkCredentials(string userName, string password)
		{
			if (userName == null)
			{
				userName = "";
			}
			if (password == null)
			{
				password = "";
			}
			if ((Operators.CompareString(userName, "", false) == 0) & (Operators.CompareString(password, "", false) == 0))
			{
				return null;
			}
			return new NetworkCredential(userName, password);
		}

		private byte[] m_PingBuffer;

		private const int BUFFER_SIZE = 32;

		private const int DEFAULT_TIMEOUT = 100000;

		private const int DEFAULT_PING_TIMEOUT = 1000;

		private const string DEFAULT_USERNAME = "";

		private const string DEFAULT_PASSWORD = "";

		private bool m_Connected;

		private object m_SyncObject;

		private ArrayList m_NetworkAvailabilityEventHandlers;

		private SynchronizationContext m_SynchronizationContext;

		private SendOrPostCallback m_NetworkAvailabilityChangedCallback;
	}
}
