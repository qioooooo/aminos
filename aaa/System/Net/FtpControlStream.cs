using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net.Cache;
using System.Net.Sockets;
using System.Security;
using System.Security.Permissions;
using System.Text;

namespace System.Net
{
	// Token: 0x020004DC RID: 1244
	internal class FtpControlStream : CommandStream
	{
		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x0600269A RID: 9882 RVA: 0x0009DDC1 File Offset: 0x0009CDC1
		// (set) Token: 0x0600269B RID: 9883 RVA: 0x0009DDEA File Offset: 0x0009CDEA
		internal NetworkCredential Credentials
		{
			get
			{
				if (this.m_Credentials != null && this.m_Credentials.IsAlive)
				{
					return (NetworkCredential)this.m_Credentials.Target;
				}
				return null;
			}
			set
			{
				if (this.m_Credentials == null)
				{
					this.m_Credentials = new WeakReference(null);
				}
				this.m_Credentials.Target = value;
			}
		}

		// Token: 0x0600269C RID: 9884 RVA: 0x0009DE0C File Offset: 0x0009CE0C
		internal FtpControlStream(ConnectionPool connectionPool, TimeSpan lifetime, bool checkLifetime)
			: base(connectionPool, lifetime, checkLifetime)
		{
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x0009DE20 File Offset: 0x0009CE20
		internal void AbortConnect()
		{
			Socket dataSocket = this.m_DataSocket;
			if (dataSocket != null)
			{
				try
				{
					dataSocket.Close();
				}
				catch (ObjectDisposedException)
				{
				}
			}
		}

		// Token: 0x0600269E RID: 9886 RVA: 0x0009DE54 File Offset: 0x0009CE54
		private static void AcceptCallback(IAsyncResult asyncResult)
		{
			FtpControlStream ftpControlStream = (FtpControlStream)asyncResult.AsyncState;
			LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
			Socket socket = (Socket)lazyAsyncResult.AsyncObject;
			try
			{
				ftpControlStream.m_DataSocket = socket.EndAccept(asyncResult);
				if (!ftpControlStream.ServerAddress.Equals(((IPEndPoint)ftpControlStream.m_DataSocket.RemoteEndPoint).Address))
				{
					ftpControlStream.m_DataSocket.Close();
					throw new WebException(SR.GetString("net_ftp_active_address_different"), WebExceptionStatus.ProtocolError);
				}
				ftpControlStream.ContinueCommandPipeline();
			}
			catch (Exception ex)
			{
				ftpControlStream.CloseSocket();
				ftpControlStream.InvokeRequestCallback(ex);
			}
			finally
			{
				socket.Close();
			}
		}

		// Token: 0x0600269F RID: 9887 RVA: 0x0009DF0C File Offset: 0x0009CF0C
		private static void ConnectCallback(IAsyncResult asyncResult)
		{
			FtpControlStream ftpControlStream = (FtpControlStream)asyncResult.AsyncState;
			try
			{
				LazyAsyncResult lazyAsyncResult = asyncResult as LazyAsyncResult;
				Socket socket = (Socket)lazyAsyncResult.AsyncObject;
				socket.EndConnect(asyncResult);
				ftpControlStream.ContinueCommandPipeline();
			}
			catch (Exception ex)
			{
				ftpControlStream.CloseSocket();
				ftpControlStream.InvokeRequestCallback(ex);
			}
		}

		// Token: 0x060026A0 RID: 9888 RVA: 0x0009DF68 File Offset: 0x0009CF68
		private static void SSLHandshakeCallback(IAsyncResult asyncResult)
		{
			FtpControlStream ftpControlStream = (FtpControlStream)asyncResult.AsyncState;
			try
			{
				ftpControlStream.ContinueCommandPipeline();
			}
			catch (Exception ex)
			{
				ftpControlStream.CloseSocket();
				ftpControlStream.InvokeRequestCallback(ex);
			}
		}

		// Token: 0x060026A1 RID: 9889 RVA: 0x0009DFAC File Offset: 0x0009CFAC
		private CommandStream.PipelineInstruction QueueOrCreateFtpDataStream(ref Stream stream)
		{
			if (this.m_DataSocket == null)
			{
				throw new InternalException();
			}
			if (this.m_TlsStream != null)
			{
				stream = new FtpDataStream(this.m_TlsStream, (FtpWebRequest)this.m_Request, this.IsFtpDataStreamWriteable());
				this.m_TlsStream = null;
				return CommandStream.PipelineInstruction.GiveStream;
			}
			NetworkStream networkStream = new NetworkStream(this.m_DataSocket, true);
			if (base.UsingSecureStream)
			{
				FtpWebRequest ftpWebRequest = (FtpWebRequest)this.m_Request;
				TlsStream tlsStream = new TlsStream(ftpWebRequest.RequestUri.Host, networkStream, ftpWebRequest.ClientCertificates, base.Pool.ServicePoint, ftpWebRequest, this.m_Async ? ftpWebRequest.GetWritingContext().ContextCopy : null);
				networkStream = tlsStream;
				if (this.m_Async)
				{
					this.m_TlsStream = tlsStream;
					LazyAsyncResult lazyAsyncResult = new LazyAsyncResult(null, this, FtpControlStream.m_SSLHandshakeCallback);
					tlsStream.ProcessAuthentication(lazyAsyncResult);
					return CommandStream.PipelineInstruction.Pause;
				}
				tlsStream.ProcessAuthentication(null);
			}
			stream = new FtpDataStream(networkStream, (FtpWebRequest)this.m_Request, this.IsFtpDataStreamWriteable());
			return CommandStream.PipelineInstruction.GiveStream;
		}

		// Token: 0x060026A2 RID: 9890 RVA: 0x0009E09C File Offset: 0x0009D09C
		protected override void ClearState()
		{
			this.m_ContentLength = -1L;
			this.m_LastModified = DateTime.MinValue;
			this.m_ResponseUri = null;
			this.m_DataHandshakeStarted = false;
			this.StatusCode = FtpStatusCode.Undefined;
			this.StatusLine = null;
			this.m_DataSocket = null;
			this.m_PassiveEndPoint = null;
			this.m_TlsStream = null;
			base.ClearState();
		}

		// Token: 0x060026A3 RID: 9891 RVA: 0x0009E0F4 File Offset: 0x0009D0F4
		protected override CommandStream.PipelineInstruction PipelineCallback(CommandStream.PipelineEntry entry, ResponseDescription response, bool timeout, ref Stream stream)
		{
			if (response == null)
			{
				return CommandStream.PipelineInstruction.Abort;
			}
			FtpStatusCode status = (FtpStatusCode)response.Status;
			if (status != FtpStatusCode.ClosingControl)
			{
				this.StatusCode = status;
				this.StatusLine = response.StatusDescription;
			}
			if (response.InvalidStatusCode)
			{
				throw new WebException(SR.GetString("net_InvalidStatusCode"), WebExceptionStatus.ProtocolError);
			}
			if (this.m_Index == -1)
			{
				if (status == FtpStatusCode.SendUserCommand)
				{
					this.m_BannerMessage = new StringBuilder();
					this.m_BannerMessage.Append(this.StatusLine);
					return CommandStream.PipelineInstruction.Advance;
				}
				if (status == FtpStatusCode.ServiceTemporarilyNotAvailable)
				{
					return CommandStream.PipelineInstruction.Reread;
				}
				throw base.GenerateException(status, response.StatusDescription, null);
			}
			else
			{
				if (entry.Command == "OPTS utf8 on\r\n")
				{
					if (response.PositiveCompletion)
					{
						base.Encoding = Encoding.UTF8;
					}
					else
					{
						base.Encoding = Encoding.Default;
					}
					return CommandStream.PipelineInstruction.Advance;
				}
				if (entry.Command.IndexOf("USER") != -1)
				{
					if (status == FtpStatusCode.LoggedInProceed)
					{
						this.m_LoginState = FtpLoginState.LoggedIn;
						this.m_Index++;
					}
					else if (status == FtpStatusCode.NotLoggedIn && this.m_LoginState != FtpLoginState.NotLoggedIn)
					{
						this.m_LoginState = FtpLoginState.ReloginFailed;
						throw ExceptionHelper.IsolatedException;
					}
				}
				if (response.TransientFailure || response.PermanentFailure)
				{
					if (status == FtpStatusCode.ServiceNotAvailable)
					{
						base.MarkAsRecoverableFailure();
					}
					throw base.GenerateException(status, response.StatusDescription, null);
				}
				if (this.m_LoginState != FtpLoginState.LoggedIn && entry.Command.IndexOf("PASS") != -1)
				{
					if (status != FtpStatusCode.NeedLoginAccount && status != FtpStatusCode.LoggedInProceed)
					{
						throw base.GenerateException(status, response.StatusDescription, null);
					}
					this.m_LoginState = FtpLoginState.LoggedIn;
				}
				if (entry.HasFlag(CommandStream.PipelineEntryFlags.CreateDataConnection) && (response.PositiveCompletion || response.PositiveIntermediate))
				{
					bool flag;
					CommandStream.PipelineInstruction pipelineInstruction = this.QueueOrCreateDataConection(entry, response, timeout, ref stream, out flag);
					if (!flag)
					{
						return pipelineInstruction;
					}
				}
				if (status == FtpStatusCode.OpeningData || status == FtpStatusCode.DataAlreadyOpen)
				{
					if (this.m_DataSocket == null)
					{
						return CommandStream.PipelineInstruction.Abort;
					}
					if (!entry.HasFlag(CommandStream.PipelineEntryFlags.GiveDataStream))
					{
						this.m_AbortReason = SR.GetString("net_ftp_invalid_status_response", new object[] { status, entry.Command });
						return CommandStream.PipelineInstruction.Abort;
					}
					this.TryUpdateContentLength(response.StatusDescription);
					if (status == FtpStatusCode.OpeningData)
					{
						FtpWebRequest ftpWebRequest = (FtpWebRequest)this.m_Request;
						if (ftpWebRequest.MethodInfo.ShouldParseForResponseUri)
						{
							this.TryUpdateResponseUri(response.StatusDescription, ftpWebRequest);
						}
					}
					return this.QueueOrCreateFtpDataStream(ref stream);
				}
				else
				{
					if (status == FtpStatusCode.LoggedInProceed)
					{
						if (this.StatusLine.ToLower(CultureInfo.InvariantCulture).IndexOf("alias") > 0)
						{
							int i = this.StatusLine.IndexOf("230-", 3);
							if (i > 0)
							{
								i += 4;
								while (i < this.StatusLine.Length && this.StatusLine[i] == ' ')
								{
									i++;
								}
								if (i < this.StatusLine.Length)
								{
									int num = this.StatusLine.IndexOf(' ', i);
									if (num < 0)
									{
										num = this.StatusLine.Length;
									}
									this.m_Alias = this.StatusLine.Substring(i, num - i);
									if (!this.m_IsRootPath)
									{
										for (i = 0; i < this.m_Commands.Length; i++)
										{
											if (this.m_Commands[i].Command.IndexOf("CWD") == 0)
											{
												string text = this.m_Alias + this.m_NewServerPath;
												this.m_Commands[i] = new CommandStream.PipelineEntry(this.FormatFtpCommand("CWD", text));
												break;
											}
										}
									}
								}
							}
						}
						this.m_WelcomeMessage.Append(this.StatusLine);
					}
					else if (status == FtpStatusCode.ClosingControl)
					{
						this.m_ExitMessage.Append(response.StatusDescription);
						base.CloseSocket();
					}
					else if (status == FtpStatusCode.ServerWantsSecureSession)
					{
						FtpWebRequest ftpWebRequest2 = (FtpWebRequest)this.m_Request;
						TlsStream tlsStream = new TlsStream(ftpWebRequest2.RequestUri.Host, base.NetworkStream, ftpWebRequest2.ClientCertificates, base.Pool.ServicePoint, ftpWebRequest2, this.m_Async ? ftpWebRequest2.GetWritingContext().ContextCopy : null);
						base.NetworkStream = tlsStream;
					}
					else if (status == FtpStatusCode.FileStatus)
					{
						FtpWebRequest ftpWebRequest3 = (FtpWebRequest)this.m_Request;
						if (entry.Command.StartsWith("SIZE "))
						{
							this.m_ContentLength = this.GetContentLengthFrom213Response(response.StatusDescription);
						}
						else if (entry.Command.StartsWith("MDTM "))
						{
							this.m_LastModified = this.GetLastModifiedFrom213Response(response.StatusDescription);
						}
					}
					else if (status == FtpStatusCode.PathnameCreated)
					{
						if (entry.Command == "PWD\r\n" && !entry.HasFlag(CommandStream.PipelineEntryFlags.UserCommand))
						{
							this.m_LoginDirectory = this.GetLoginDirectory(response.StatusDescription);
							if (!this.m_IsRootPath && this.m_LoginDirectory != "\\" && this.m_LoginDirectory != "/" && this.m_Alias == null)
							{
								for (int j = 0; j < this.m_Commands.Length; j++)
								{
									if (this.m_Commands[j].Command.IndexOf("CWD") == 0)
									{
										string text2 = this.m_LoginDirectory + this.m_NewServerPath;
										this.m_Commands[j] = new CommandStream.PipelineEntry(this.FormatFtpCommand("CWD", text2));
										break;
									}
								}
							}
						}
					}
					else if (entry.Command.IndexOf("CWD") != -1)
					{
						this.m_PreviousServerPath = this.m_NewServerPath;
					}
					if (response.PositiveIntermediate || (!base.UsingSecureStream && entry.Command == "AUTH TLS\r\n"))
					{
						return CommandStream.PipelineInstruction.Reread;
					}
					return CommandStream.PipelineInstruction.Advance;
				}
			}
		}

		// Token: 0x060026A4 RID: 9892 RVA: 0x0009E698 File Offset: 0x0009D698
		protected override CommandStream.PipelineEntry[] BuildCommandsList(WebRequest req)
		{
			FtpWebRequest ftpWebRequest = (FtpWebRequest)req;
			this.m_ResponseUri = ftpWebRequest.RequestUri;
			ArrayList arrayList = new ArrayList();
			if ((this.m_LastRequestWasUnknownMethod && !ftpWebRequest.MethodInfo.IsUnknownMethod) || this.Credentials == null || !this.Credentials.IsEqualTo(ftpWebRequest.Credentials.GetCredential(ftpWebRequest.RequestUri, "basic")))
			{
				this.m_PreviousServerPath = null;
				this.m_NewServerPath = null;
				this.m_LoginDirectory = null;
				if (this.m_LoginState == FtpLoginState.LoggedIn)
				{
					this.m_LoginState = FtpLoginState.LoggedInButNeedsRelogin;
				}
			}
			this.m_LastRequestWasUnknownMethod = ftpWebRequest.MethodInfo.IsUnknownMethod;
			if (ftpWebRequest.EnableSsl && !base.UsingSecureStream)
			{
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("AUTH", "TLS")));
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("PBSZ", "0")));
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("PROT", "P")));
				if (this.m_LoginState == FtpLoginState.LoggedIn)
				{
					this.m_LoginState = FtpLoginState.LoggedInButNeedsRelogin;
				}
			}
			if (this.m_LoginState != FtpLoginState.LoggedIn)
			{
				this.Credentials = ftpWebRequest.Credentials.GetCredential(ftpWebRequest.RequestUri, "basic");
				this.m_WelcomeMessage = new StringBuilder();
				this.m_ExitMessage = new StringBuilder();
				string text = string.Empty;
				string text2 = string.Empty;
				if (this.Credentials != null)
				{
					text = this.Credentials.InternalGetDomainUserName();
					text2 = this.Credentials.InternalGetPassword();
				}
				if (text.Length == 0 && text2.Length == 0)
				{
					text = "anonymous";
					text2 = "anonymous@";
				}
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("USER", text)));
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("PASS", text2), CommandStream.PipelineEntryFlags.DontLogParameter));
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("OPTS", "utf8 on")));
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("PWD", null)));
			}
			FtpControlStream.GetPathOption getPathOption = FtpControlStream.GetPathOption.Normal;
			if (ftpWebRequest.MethodInfo.HasFlag(FtpMethodFlags.DoesNotTakeParameter))
			{
				getPathOption = FtpControlStream.GetPathOption.AssumeNoFilename;
			}
			else if (ftpWebRequest.MethodInfo.HasFlag(FtpMethodFlags.ParameterIsDirectory))
			{
				getPathOption = FtpControlStream.GetPathOption.AssumeFilename;
			}
			string text3 = null;
			string text4 = null;
			FtpControlStream.GetPathAndFilename(getPathOption, ftpWebRequest.RequestUri, ref text3, ref text4, ref this.m_IsRootPath);
			if (text4.Length == 0 && ftpWebRequest.MethodInfo.HasFlag(FtpMethodFlags.TakesParameter))
			{
				throw new WebException(SR.GetString("net_ftp_invalid_uri"));
			}
			string text5 = text3;
			if (this.m_PreviousServerPath != text5)
			{
				if (!this.m_IsRootPath && this.m_LoginState == FtpLoginState.LoggedIn && this.m_LoginDirectory != null)
				{
					text5 = this.m_LoginDirectory + text5;
				}
				this.m_NewServerPath = text5;
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("CWD", text5), CommandStream.PipelineEntryFlags.UserCommand));
			}
			if (ftpWebRequest.CacheProtocol != null && ftpWebRequest.CacheProtocol.ProtocolStatus == CacheValidationStatus.DoNotTakeFromCache && ftpWebRequest.MethodInfo.Operation == FtpOperation.DownloadFile)
			{
				arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("MDTM", text4)));
			}
			if (!ftpWebRequest.MethodInfo.IsCommandOnly)
			{
				if (ftpWebRequest.CacheProtocol == null || ftpWebRequest.CacheProtocol.ProtocolStatus != CacheValidationStatus.Continue)
				{
					if (ftpWebRequest.UseBinary)
					{
						arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("TYPE", "I")));
					}
					else
					{
						arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("TYPE", "A")));
					}
					if (ftpWebRequest.UsePassive)
					{
						string text6 = ((base.ServerAddress.AddressFamily == AddressFamily.InterNetwork) ? "PASV" : "EPSV");
						arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand(text6, null), CommandStream.PipelineEntryFlags.CreateDataConnection));
					}
					else
					{
						string text7 = ((base.ServerAddress.AddressFamily == AddressFamily.InterNetwork) ? "PORT" : "EPRT");
						this.CreateFtpListenerSocket(ftpWebRequest);
						arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand(text7, this.GetPortCommandLine(ftpWebRequest))));
					}
					if (ftpWebRequest.CacheProtocol != null && ftpWebRequest.CacheProtocol.ProtocolStatus == CacheValidationStatus.CombineCachedAndServerResponse)
					{
						if (ftpWebRequest.CacheProtocol.Validator.CacheEntry.StreamSize > 0L)
						{
							arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("REST", ftpWebRequest.CacheProtocol.Validator.CacheEntry.StreamSize.ToString(CultureInfo.InvariantCulture))));
						}
					}
					else if (ftpWebRequest.ContentOffset > 0L)
					{
						arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("REST", ftpWebRequest.ContentOffset.ToString(CultureInfo.InvariantCulture))));
					}
				}
				else
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("SIZE", text4)));
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("MDTM", text4)));
				}
			}
			if (ftpWebRequest.CacheProtocol == null || ftpWebRequest.CacheProtocol.ProtocolStatus != CacheValidationStatus.Continue)
			{
				CommandStream.PipelineEntryFlags pipelineEntryFlags = CommandStream.PipelineEntryFlags.UserCommand;
				if (!ftpWebRequest.MethodInfo.IsCommandOnly)
				{
					pipelineEntryFlags |= CommandStream.PipelineEntryFlags.GiveDataStream;
					if (!ftpWebRequest.UsePassive)
					{
						pipelineEntryFlags |= CommandStream.PipelineEntryFlags.CreateDataConnection;
					}
				}
				if (ftpWebRequest.MethodInfo.Operation == FtpOperation.Rename)
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("RNFR", text4), pipelineEntryFlags));
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("RNTO", ftpWebRequest.RenameTo), pipelineEntryFlags));
				}
				else
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand(ftpWebRequest.Method, text4), pipelineEntryFlags));
				}
				if (!ftpWebRequest.KeepAlive)
				{
					arrayList.Add(new CommandStream.PipelineEntry(this.FormatFtpCommand("QUIT", null)));
				}
			}
			return (CommandStream.PipelineEntry[])arrayList.ToArray(typeof(CommandStream.PipelineEntry));
		}

		// Token: 0x060026A5 RID: 9893 RVA: 0x0009EC30 File Offset: 0x0009DC30
		private CommandStream.PipelineInstruction QueueOrCreateDataConection(CommandStream.PipelineEntry entry, ResponseDescription response, bool timeout, ref Stream stream, out bool isSocketReady)
		{
			isSocketReady = false;
			if (this.m_DataHandshakeStarted)
			{
				isSocketReady = true;
				return CommandStream.PipelineInstruction.Pause;
			}
			this.m_DataHandshakeStarted = true;
			bool flag = false;
			int num = -1;
			if (entry.Command == "PASV\r\n" || entry.Command == "EPSV\r\n")
			{
				if (!response.PositiveCompletion)
				{
					this.m_AbortReason = SR.GetString("net_ftp_server_failed_passive", new object[] { response.Status });
					return CommandStream.PipelineInstruction.Abort;
				}
				if (entry.Command == "PASV\r\n")
				{
					IPAddress ipaddress = null;
					num = this.GetAddressAndPort(response.StatusDescription, ref ipaddress);
					if (!base.ServerAddress.Equals(ipaddress))
					{
						throw new WebException(SR.GetString("net_ftp_passive_address_different"));
					}
				}
				else
				{
					num = this.GetPortV6(response.StatusDescription);
				}
				flag = true;
			}
			new SocketPermission(PermissionState.Unrestricted).Assert();
			CommandStream.PipelineInstruction pipelineInstruction2;
			try
			{
				if (flag)
				{
					try
					{
						this.m_DataSocket = this.CreateFtpDataSocket((FtpWebRequest)this.m_Request, base.Socket);
					}
					catch (ObjectDisposedException)
					{
						throw ExceptionHelper.RequestAbortedException;
					}
					IPEndPoint ipendPoint = new IPEndPoint(((IPEndPoint)base.Socket.LocalEndPoint).Address, 0);
					this.m_DataSocket.Bind(ipendPoint);
					this.m_PassiveEndPoint = new IPEndPoint(base.ServerAddress, num);
				}
				CommandStream.PipelineInstruction pipelineInstruction;
				if (this.m_PassiveEndPoint != null)
				{
					IPEndPoint passiveEndPoint = this.m_PassiveEndPoint;
					this.m_PassiveEndPoint = null;
					if (this.m_Async)
					{
						this.m_DataSocket.BeginConnect(passiveEndPoint, FtpControlStream.m_ConnectCallbackDelegate, this);
						pipelineInstruction = CommandStream.PipelineInstruction.Pause;
					}
					else
					{
						this.m_DataSocket.Connect(passiveEndPoint);
						pipelineInstruction = CommandStream.PipelineInstruction.Advance;
					}
				}
				else if (this.m_Async)
				{
					this.m_DataSocket.BeginAccept(FtpControlStream.m_AcceptCallbackDelegate, this);
					pipelineInstruction = CommandStream.PipelineInstruction.Pause;
				}
				else
				{
					Socket dataSocket = this.m_DataSocket;
					try
					{
						this.m_DataSocket = this.m_DataSocket.Accept();
						if (!base.ServerAddress.Equals(((IPEndPoint)this.m_DataSocket.RemoteEndPoint).Address))
						{
							this.m_DataSocket.Close();
							throw new WebException(SR.GetString("net_ftp_active_address_different"), WebExceptionStatus.ProtocolError);
						}
						isSocketReady = true;
						pipelineInstruction = CommandStream.PipelineInstruction.Pause;
					}
					finally
					{
						dataSocket.Close();
					}
				}
				pipelineInstruction2 = pipelineInstruction;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
			return pipelineInstruction2;
		}

		// Token: 0x060026A6 RID: 9894 RVA: 0x0009EEA4 File Offset: 0x0009DEA4
		internal void Quit()
		{
			base.CloseSocket();
		}

		// Token: 0x060026A7 RID: 9895 RVA: 0x0009EEAC File Offset: 0x0009DEAC
		private static void GetPathAndFilename(FtpControlStream.GetPathOption pathOption, Uri uri, ref string path, ref string filename, ref bool isRoot)
		{
			string text = uri.GetParts(UriComponents.Path | UriComponents.KeepDelimiter, UriFormat.Unescaped);
			isRoot = false;
			if (text.StartsWith("//"))
			{
				isRoot = true;
				text = text.Substring(1, text.Length - 1);
			}
			int num = text.LastIndexOf('/');
			switch (pathOption)
			{
			case FtpControlStream.GetPathOption.AssumeFilename:
				if (num != -1 && num == text.Length - 1)
				{
					text = text.Substring(0, text.Length - 1);
					num = text.LastIndexOf('/');
				}
				path = text.Substring(0, num + 1);
				filename = text.Substring(num + 1, text.Length - (num + 1));
				goto IL_00C9;
			case FtpControlStream.GetPathOption.AssumeNoFilename:
				path = text;
				filename = "";
				goto IL_00C9;
			}
			path = text.Substring(0, num + 1);
			filename = text.Substring(num + 1, text.Length - (num + 1));
			IL_00C9:
			if (path.Length == 0)
			{
				path = "/";
			}
		}

		// Token: 0x060026A8 RID: 9896 RVA: 0x0009EF94 File Offset: 0x0009DF94
		private string FormatAddress(IPAddress address, int Port)
		{
			byte[] addressBytes = address.GetAddressBytes();
			StringBuilder stringBuilder = new StringBuilder(32);
			foreach (byte b in addressBytes)
			{
				stringBuilder.Append(b);
				stringBuilder.Append(',');
			}
			stringBuilder.Append(Port / 256);
			stringBuilder.Append(',');
			stringBuilder.Append(Port % 256);
			return stringBuilder.ToString();
		}

		// Token: 0x060026A9 RID: 9897 RVA: 0x0009F008 File Offset: 0x0009E008
		private string FormatAddressV6(IPAddress address, int port)
		{
			StringBuilder stringBuilder = new StringBuilder(43);
			string text = address.ToString();
			stringBuilder.Append("|2|");
			stringBuilder.Append(text);
			stringBuilder.Append('|');
			stringBuilder.Append(port.ToString(NumberFormatInfo.InvariantInfo));
			stringBuilder.Append('|');
			return stringBuilder.ToString();
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x060026AA RID: 9898 RVA: 0x0009F063 File Offset: 0x0009E063
		internal long ContentLength
		{
			get
			{
				return this.m_ContentLength;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x060026AB RID: 9899 RVA: 0x0009F06B File Offset: 0x0009E06B
		internal DateTime LastModified
		{
			get
			{
				return this.m_LastModified;
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x060026AC RID: 9900 RVA: 0x0009F073 File Offset: 0x0009E073
		internal Uri ResponseUri
		{
			get
			{
				return this.m_ResponseUri;
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x060026AD RID: 9901 RVA: 0x0009F07B File Offset: 0x0009E07B
		internal string BannerMessage
		{
			get
			{
				if (this.m_BannerMessage == null)
				{
					return null;
				}
				return this.m_BannerMessage.ToString();
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x060026AE RID: 9902 RVA: 0x0009F092 File Offset: 0x0009E092
		internal string WelcomeMessage
		{
			get
			{
				if (this.m_WelcomeMessage == null)
				{
					return null;
				}
				return this.m_WelcomeMessage.ToString();
			}
		}

		// Token: 0x17000804 RID: 2052
		// (get) Token: 0x060026AF RID: 9903 RVA: 0x0009F0A9 File Offset: 0x0009E0A9
		internal string ExitMessage
		{
			get
			{
				if (this.m_ExitMessage == null)
				{
					return null;
				}
				return this.m_ExitMessage.ToString();
			}
		}

		// Token: 0x060026B0 RID: 9904 RVA: 0x0009F0C0 File Offset: 0x0009E0C0
		private long GetContentLengthFrom213Response(string responseString)
		{
			string[] array = responseString.Split(new char[] { ' ' });
			if (array.Length < 2)
			{
				throw new FormatException(SR.GetString("net_ftp_response_invalid_format", new object[] { responseString }));
			}
			return Convert.ToInt64(array[1], NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x060026B1 RID: 9905 RVA: 0x0009F110 File Offset: 0x0009E110
		private DateTime GetLastModifiedFrom213Response(string str)
		{
			DateTime dateTime = this.m_LastModified;
			string[] array = str.Split(new char[] { ' ', '.' });
			if (array.Length < 2)
			{
				return dateTime;
			}
			string text = array[1];
			if (text.Length < 14)
			{
				return dateTime;
			}
			int num = Convert.ToInt32(text.Substring(0, 4), NumberFormatInfo.InvariantInfo);
			int num2 = (int)Convert.ToInt16(text.Substring(4, 2), NumberFormatInfo.InvariantInfo);
			int num3 = (int)Convert.ToInt16(text.Substring(6, 2), NumberFormatInfo.InvariantInfo);
			int num4 = (int)Convert.ToInt16(text.Substring(8, 2), NumberFormatInfo.InvariantInfo);
			int num5 = (int)Convert.ToInt16(text.Substring(10, 2), NumberFormatInfo.InvariantInfo);
			int num6 = (int)Convert.ToInt16(text.Substring(12, 2), NumberFormatInfo.InvariantInfo);
			int num7 = 0;
			if (array.Length > 2)
			{
				num7 = (int)Convert.ToInt16(array[2], NumberFormatInfo.InvariantInfo);
			}
			try
			{
				dateTime = new DateTime(num, num2, num3, num4, num5, num6, num7);
				dateTime = dateTime.ToLocalTime();
			}
			catch (ArgumentOutOfRangeException)
			{
			}
			catch (ArgumentException)
			{
			}
			return dateTime;
		}

		// Token: 0x060026B2 RID: 9906 RVA: 0x0009F238 File Offset: 0x0009E238
		private void TryUpdateResponseUri(string str, FtpWebRequest request)
		{
			Uri uri = request.RequestUri;
			int num = str.IndexOf("for ");
			if (num == -1)
			{
				return;
			}
			num += 4;
			int num2 = str.LastIndexOf('(');
			if (num2 == -1)
			{
				num2 = str.Length;
			}
			if (num2 <= num)
			{
				return;
			}
			string text = str.Substring(num, num2 - num);
			text = text.TrimEnd(new char[] { ' ', '.', '\r', '\n' });
			string text2 = text.Replace("%", "%25");
			text2 = text2.Replace("#", "%23");
			string absolutePath = uri.AbsolutePath;
			if (absolutePath.Length > 0 && absolutePath[absolutePath.Length - 1] != '/')
			{
				uri = new UriBuilder(uri)
				{
					Path = absolutePath + "/"
				}.Uri;
			}
			Uri uri2;
			if (!Uri.TryCreate(uri, text2, out uri2))
			{
				throw new FormatException(SR.GetString("net_ftp_invalid_response_filename", new object[] { text }));
			}
			if (!uri.IsBaseOf(uri2) || uri.Segments.Length != uri2.Segments.Length - 1)
			{
				throw new FormatException(SR.GetString("net_ftp_invalid_response_filename", new object[] { text }));
			}
			this.m_ResponseUri = uri2;
		}

		// Token: 0x060026B3 RID: 9907 RVA: 0x0009F37C File Offset: 0x0009E37C
		private void TryUpdateContentLength(string str)
		{
			int num = str.LastIndexOf("(");
			if (num != -1)
			{
				int num2 = str.IndexOf(" bytes).");
				if (num2 != -1 && num2 > num)
				{
					num++;
					long num3;
					if (long.TryParse(str.Substring(num, num2 - num), NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite, NumberFormatInfo.InvariantInfo, out num3))
					{
						this.m_ContentLength = num3;
					}
				}
			}
		}

		// Token: 0x060026B4 RID: 9908 RVA: 0x0009F3D4 File Offset: 0x0009E3D4
		private string GetLoginDirectory(string str)
		{
			int num = str.IndexOf('"');
			int num2 = str.LastIndexOf('"');
			if (num != -1 && num2 != -1 && num != num2)
			{
				return str.Substring(num + 1, num2 - num - 1);
			}
			return string.Empty;
		}

		// Token: 0x060026B5 RID: 9909 RVA: 0x0009F420 File Offset: 0x0009E420
		private int GetAddressAndPort(string responseString, ref IPAddress ipAddress)
		{
			int num = 0;
			string[] array = responseString.Split(new char[] { '(', ',', ')' });
			if (6 >= array.Length)
			{
				throw new FormatException(SR.GetString("net_ftp_response_invalid_format", new object[] { responseString }));
			}
			num = Convert.ToInt32(array[5], NumberFormatInfo.InvariantInfo) * 256;
			num += Convert.ToInt32(array[6], NumberFormatInfo.InvariantInfo);
			long num2 = 0L;
			try
			{
				for (int i = 4; i > 0; i--)
				{
					num2 = (num2 << 8) + (long)((ulong)Convert.ToByte(array[i], NumberFormatInfo.InvariantInfo));
				}
			}
			catch
			{
				throw new FormatException(SR.GetString("net_ftp_response_invalid_format", new object[] { responseString }));
			}
			ipAddress = new IPAddress(num2);
			return num;
		}

		// Token: 0x060026B6 RID: 9910 RVA: 0x0009F4F0 File Offset: 0x0009E4F0
		private int GetPortV6(string responseString)
		{
			int num = responseString.LastIndexOf("(");
			int num2 = responseString.LastIndexOf(")");
			if (num == -1 || num2 <= num)
			{
				throw new FormatException(SR.GetString("net_ftp_response_invalid_format", new object[] { responseString }));
			}
			string text = responseString.Substring(num + 1, num2 - num - 1);
			string[] array = text.Split(new char[] { '|' });
			if (array.Length < 4)
			{
				throw new FormatException(SR.GetString("net_ftp_response_invalid_format", new object[] { responseString }));
			}
			return Convert.ToInt32(array[3], NumberFormatInfo.InvariantInfo);
		}

		// Token: 0x060026B7 RID: 9911 RVA: 0x0009F594 File Offset: 0x0009E594
		private void CreateFtpListenerSocket(FtpWebRequest request)
		{
			IPEndPoint ipendPoint = new IPEndPoint(((IPEndPoint)base.Socket.LocalEndPoint).Address, 0);
			try
			{
				this.m_DataSocket = this.CreateFtpDataSocket(request, base.Socket);
			}
			catch (ObjectDisposedException)
			{
				throw ExceptionHelper.RequestAbortedException;
			}
			new SocketPermission(PermissionState.Unrestricted).Assert();
			try
			{
				this.m_DataSocket.Bind(ipendPoint);
				this.m_DataSocket.Listen(1);
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
			}
		}

		// Token: 0x060026B8 RID: 9912 RVA: 0x0009F620 File Offset: 0x0009E620
		private string GetPortCommandLine(FtpWebRequest request)
		{
			string text;
			try
			{
				IPEndPoint ipendPoint = (IPEndPoint)this.m_DataSocket.LocalEndPoint;
				if (base.ServerAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					text = this.FormatAddress(ipendPoint.Address, ipendPoint.Port);
				}
				else
				{
					if (base.ServerAddress.AddressFamily != AddressFamily.InterNetworkV6)
					{
						throw new InternalException();
					}
					text = this.FormatAddressV6(ipendPoint.Address, ipendPoint.Port);
				}
			}
			catch (Exception ex)
			{
				throw base.GenerateException(WebExceptionStatus.ProtocolError, ex);
			}
			catch
			{
				throw base.GenerateException(WebExceptionStatus.ProtocolError, new Exception(SR.GetString("net_nonClsCompliantException")));
			}
			return text;
		}

		// Token: 0x060026B9 RID: 9913 RVA: 0x0009F6CC File Offset: 0x0009E6CC
		private string FormatFtpCommand(string command, string parameter)
		{
			if (!ServicePointManager.AllowNewLineInFtpCommand && parameter != null)
			{
				int num = parameter.IndexOf("\r\n");
				if (num != -1)
				{
					parameter = parameter.Substring(0, num);
				}
			}
			StringBuilder stringBuilder = new StringBuilder(command.Length + ((parameter != null) ? parameter.Length : 0) + 3);
			stringBuilder.Append(command);
			if (!ValidationHelper.IsBlankString(parameter))
			{
				stringBuilder.Append(' ');
				stringBuilder.Append(parameter);
			}
			stringBuilder.Append("\r\n");
			return stringBuilder.ToString();
		}

		// Token: 0x060026BA RID: 9914 RVA: 0x0009F74C File Offset: 0x0009E74C
		protected Socket CreateFtpDataSocket(FtpWebRequest request, Socket templateSocket)
		{
			return new Socket(templateSocket.AddressFamily, templateSocket.SocketType, templateSocket.ProtocolType);
		}

		// Token: 0x060026BB RID: 9915 RVA: 0x0009F774 File Offset: 0x0009E774
		protected override bool CheckValid(ResponseDescription response, ref int validThrough, ref int completeLength)
		{
			if (response.StatusBuffer.Length < 4)
			{
				return true;
			}
			string text = response.StatusBuffer.ToString();
			if (response.Status == -1)
			{
				if (!char.IsDigit(text[0]) || !char.IsDigit(text[1]) || !char.IsDigit(text[2]) || (text[3] != ' ' && text[3] != '-'))
				{
					return false;
				}
				response.StatusCodeString = text.Substring(0, 3);
				response.Status = (int)Convert.ToInt16(response.StatusCodeString, NumberFormatInfo.InvariantInfo);
				if (text[3] == '-')
				{
					response.Multiline = true;
				}
			}
			int num;
			while ((num = text.IndexOf("\r\n", validThrough)) != -1)
			{
				int num2 = validThrough;
				validThrough = num + 2;
				if (!response.Multiline)
				{
					completeLength = validThrough;
					return true;
				}
				if (text.Length > num2 + 4 && text.Substring(num2, 3) == response.StatusCodeString && text[num2 + 3] == ' ')
				{
					completeLength = validThrough;
					return true;
				}
			}
			return true;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x0009F880 File Offset: 0x0009E880
		private TriState IsFtpDataStreamWriteable()
		{
			FtpWebRequest ftpWebRequest = this.m_Request as FtpWebRequest;
			if (ftpWebRequest != null)
			{
				if (ftpWebRequest.MethodInfo.IsUpload)
				{
					return TriState.True;
				}
				if (ftpWebRequest.MethodInfo.IsDownload)
				{
					return TriState.False;
				}
			}
			return TriState.Unspecified;
		}

		// Token: 0x04002638 RID: 9784
		private Socket m_DataSocket;

		// Token: 0x04002639 RID: 9785
		private IPEndPoint m_PassiveEndPoint;

		// Token: 0x0400263A RID: 9786
		private TlsStream m_TlsStream;

		// Token: 0x0400263B RID: 9787
		private StringBuilder m_BannerMessage;

		// Token: 0x0400263C RID: 9788
		private StringBuilder m_WelcomeMessage;

		// Token: 0x0400263D RID: 9789
		private StringBuilder m_ExitMessage;

		// Token: 0x0400263E RID: 9790
		private WeakReference m_Credentials;

		// Token: 0x0400263F RID: 9791
		private string m_Alias;

		// Token: 0x04002640 RID: 9792
		private bool m_IsRootPath;

		// Token: 0x04002641 RID: 9793
		private long m_ContentLength = -1L;

		// Token: 0x04002642 RID: 9794
		private DateTime m_LastModified;

		// Token: 0x04002643 RID: 9795
		private bool m_DataHandshakeStarted;

		// Token: 0x04002644 RID: 9796
		private string m_LoginDirectory;

		// Token: 0x04002645 RID: 9797
		private string m_PreviousServerPath;

		// Token: 0x04002646 RID: 9798
		private string m_NewServerPath;

		// Token: 0x04002647 RID: 9799
		private Uri m_ResponseUri;

		// Token: 0x04002648 RID: 9800
		private bool m_LastRequestWasUnknownMethod;

		// Token: 0x04002649 RID: 9801
		private FtpLoginState m_LoginState;

		// Token: 0x0400264A RID: 9802
		internal FtpStatusCode StatusCode;

		// Token: 0x0400264B RID: 9803
		internal string StatusLine;

		// Token: 0x0400264C RID: 9804
		private static readonly AsyncCallback m_AcceptCallbackDelegate = new AsyncCallback(FtpControlStream.AcceptCallback);

		// Token: 0x0400264D RID: 9805
		private static readonly AsyncCallback m_ConnectCallbackDelegate = new AsyncCallback(FtpControlStream.ConnectCallback);

		// Token: 0x0400264E RID: 9806
		private static readonly AsyncCallback m_SSLHandshakeCallback = new AsyncCallback(FtpControlStream.SSLHandshakeCallback);

		// Token: 0x020004DD RID: 1245
		private enum GetPathOption
		{
			// Token: 0x04002650 RID: 9808
			Normal,
			// Token: 0x04002651 RID: 9809
			AssumeFilename,
			// Token: 0x04002652 RID: 9810
			AssumeNoFilename
		}
	}
}
