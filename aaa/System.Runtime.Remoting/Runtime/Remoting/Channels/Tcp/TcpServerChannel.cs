using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Permissions;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x02000043 RID: 67
	public class TcpServerChannel : IChannelReceiver, IChannel, ISecurableChannel
	{
		// Token: 0x06000233 RID: 563 RVA: 0x0000B408 File Offset: 0x0000A408
		public TcpServerChannel(int port)
		{
			this._port = port;
			this.SetupMachineName();
			this.SetupChannel();
		}

		// Token: 0x06000234 RID: 564 RVA: 0x0000B478 File Offset: 0x0000A478
		public TcpServerChannel(string name, int port)
		{
			this._channelName = name;
			this._port = port;
			this.SetupMachineName();
			this.SetupChannel();
		}

		// Token: 0x06000235 RID: 565 RVA: 0x0000B4EC File Offset: 0x0000A4EC
		public TcpServerChannel(string name, int port, IServerChannelSinkProvider sinkProvider)
		{
			this._channelName = name;
			this._port = port;
			this._sinkProvider = sinkProvider;
			this.SetupMachineName();
			this.SetupChannel();
		}

		// Token: 0x06000236 RID: 566 RVA: 0x0000B567 File Offset: 0x0000A567
		public TcpServerChannel(IDictionary properties, IServerChannelSinkProvider sinkProvider)
			: this(properties, sinkProvider, null)
		{
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000B574 File Offset: 0x0000A574
		public TcpServerChannel(IDictionary properties, IServerChannelSinkProvider sinkProvider, IAuthorizeRemotingConnection authorizeCallback)
		{
			this._authorizeRemotingConnection = authorizeCallback;
			if (properties != null)
			{
				foreach (object obj in properties)
				{
					DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
					string text;
					switch (text = (string)dictionaryEntry.Key)
					{
					case "name":
						this._channelName = (string)dictionaryEntry.Value;
						break;
					case "bindTo":
						this._bindToAddr = IPAddress.Parse((string)dictionaryEntry.Value);
						break;
					case "port":
						this._port = Convert.ToInt32(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "priority":
						this._channelPriority = Convert.ToInt32(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "secure":
						this._secure = Convert.ToBoolean(dictionaryEntry.Value);
						break;
					case "impersonate":
						this._impersonate = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						this.authSet = true;
						break;
					case "protectionLevel":
						this._protectionLevel = (ProtectionLevel)((dictionaryEntry.Value is ProtectionLevel) ? dictionaryEntry.Value : Enum.Parse(typeof(ProtectionLevel), (string)dictionaryEntry.Value, true));
						this.authSet = true;
						break;
					case "machineName":
						this._forcedMachineName = (string)dictionaryEntry.Value;
						break;
					case "rejectRemoteRequests":
					{
						bool flag = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						if (flag)
						{
							if (Socket.SupportsIPv4)
							{
								this._bindToAddr = IPAddress.Loopback;
							}
							else
							{
								this._bindToAddr = IPAddress.IPv6Loopback;
							}
						}
						break;
					}
					case "suppressChannelData":
						this._bSuppressChannelData = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "useIpAddress":
						this._bUseIpAddress = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "exclusiveAddressUse":
						this._bExclusiveAddressUse = Convert.ToBoolean(dictionaryEntry.Value, CultureInfo.InvariantCulture);
						break;
					case "authorizationModule":
						this._authorizeRemotingConnection = (IAuthorizeRemotingConnection)Activator.CreateInstance(Type.GetType((string)dictionaryEntry.Value, true));
						break;
					}
				}
			}
			this._sinkProvider = sinkProvider;
			this.SetupMachineName();
			this.SetupChannel();
		}

		// Token: 0x17000080 RID: 128
		// (get) Token: 0x06000238 RID: 568 RVA: 0x0000B910 File Offset: 0x0000A910
		// (set) Token: 0x06000239 RID: 569 RVA: 0x0000B918 File Offset: 0x0000A918
		public bool IsSecured
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._secure;
			}
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			set
			{
				this._secure = value;
			}
		}

		// Token: 0x0600023A RID: 570 RVA: 0x0000B924 File Offset: 0x0000A924
		private void SetupMachineName()
		{
			if (this._forcedMachineName != null)
			{
				this._machineName = CoreChannel.DecodeMachineName(this._forcedMachineName);
				return;
			}
			if (!this._bUseIpAddress)
			{
				this._machineName = CoreChannel.GetMachineName();
				return;
			}
			if (this._bindToAddr == IPAddress.Any || this._bindToAddr == IPAddress.IPv6Any)
			{
				this._machineName = CoreChannel.GetMachineIp();
			}
			else
			{
				this._machineName = this._bindToAddr.ToString();
			}
			if (this._bindToAddr.AddressFamily == AddressFamily.InterNetworkV6)
			{
				this._machineName = "[" + this._machineName + "]";
			}
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0000B9C4 File Offset: 0x0000A9C4
		private void SetupChannel()
		{
			if (this.authSet && !this._secure)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Tcp_AuthenticationConfigServer"));
			}
			this._channelData = new ChannelDataStore(null);
			if (this._port > 0)
			{
				this._channelData.ChannelUris = new string[1];
				this._channelData.ChannelUris[0] = this.GetChannelUri();
			}
			if (this._sinkProvider == null)
			{
				this._sinkProvider = this.CreateDefaultServerProviderChain();
			}
			CoreChannel.CollectChannelDataFromServerSinkProviders(this._channelData, this._sinkProvider);
			IServerChannelSink serverChannelSink = ChannelServices.CreateServerChannelSinkChain(this._sinkProvider, this);
			this._transportSink = new TcpServerTransportSink(serverChannelSink, this._impersonate);
			this._acceptSocketCallback = new AsyncCallback(this.AcceptSocketCallbackHelper);
			if (this._port >= 0)
			{
				this._tcpListener = new ExclusiveTcpListener(this._bindToAddr, this._port);
				this.StartListening(null);
			}
		}

		// Token: 0x0600023C RID: 572 RVA: 0x0000BAA8 File Offset: 0x0000AAA8
		private IServerChannelSinkProvider CreateDefaultServerProviderChain()
		{
			IServerChannelSinkProvider serverChannelSinkProvider = new BinaryServerFormatterSinkProvider();
			IServerChannelSinkProvider serverChannelSinkProvider2 = serverChannelSinkProvider;
			serverChannelSinkProvider2.Next = new SoapServerFormatterSinkProvider();
			return serverChannelSinkProvider;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000BAC9 File Offset: 0x0000AAC9
		public int ChannelPriority
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelPriority;
			}
		}

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0000BAD1 File Offset: 0x0000AAD1
		public string ChannelName
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				return this._channelName;
			}
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000BAD9 File Offset: 0x0000AAD9
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public string Parse(string url, out string objectURI)
		{
			return TcpChannelHelper.ParseURL(url, out objectURI);
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x06000240 RID: 576 RVA: 0x0000BAE2 File Offset: 0x0000AAE2
		public object ChannelData
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
			get
			{
				if (this._bSuppressChannelData || !this._bListening)
				{
					return null;
				}
				return this._channelData;
			}
		}

		// Token: 0x06000241 RID: 577 RVA: 0x0000BAFC File Offset: 0x0000AAFC
		public string GetChannelUri()
		{
			return string.Concat(new object[] { "tcp://", this._machineName, ":", this._port });
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0000BB40 File Offset: 0x0000AB40
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public virtual string[] GetUrlsForUri(string objectUri)
		{
			string[] array = new string[1];
			if (!objectUri.StartsWith("/", StringComparison.Ordinal))
			{
				objectUri = "/" + objectUri;
			}
			array[0] = this.GetChannelUri() + objectUri;
			return array;
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0000BB80 File Offset: 0x0000AB80
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StartListening(object data)
		{
			if (this._port >= 0)
			{
				this._tcpListener.Start(this._bExclusiveAddressUse);
				this._bListening = true;
				if (this._port == 0)
				{
					this._port = ((IPEndPoint)this._tcpListener.LocalEndpoint).Port;
					if (this._channelData != null)
					{
						this._channelData.ChannelUris = new string[1];
						this._channelData.ChannelUris[0] = this.GetChannelUri();
					}
				}
				this._tcpListener.BeginAcceptSocket(this._acceptSocketCallback, null);
			}
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0000BC10 File Offset: 0x0000AC10
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure, Infrastructure = true)]
		public void StopListening(object data)
		{
			if (this._port > 0)
			{
				this._bListening = false;
				if (this._tcpListener != null)
				{
					this._tcpListener.Stop();
				}
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000BC35 File Offset: 0x0000AC35
		private void AcceptSocketCallbackHelper(IAsyncResult ar)
		{
			if (ar.CompletedSynchronously)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.AcceptSocketCallbackAsync), ar);
				return;
			}
			this.AcceptSocketCallback(ar);
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000BC5A File Offset: 0x0000AC5A
		private void AcceptSocketCallbackAsync(object state)
		{
			this.AcceptSocketCallback((IAsyncResult)state);
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0000BC68 File Offset: 0x0000AC68
		private void AcceptSocketCallback(IAsyncResult ar)
		{
			/*
An exception occurred when decompiling this method (06000247)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Runtime.Remoting.Channels.Tcp.TcpServerChannel::AcceptSocketCallback(System.IAsyncResult)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at ICSharpCode.Decompiler.ILAst.ILExpression.GetBranchTargets() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstTypes.cs:line 703
   at ICSharpCode.Decompiler.ILAst.SimpleControlFlow.Initialize(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\SimpleControlFlow.cs:line 50
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 283
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0000BE40 File Offset: 0x0000AE40
		private WindowsIdentity Authenticate(ref Stream netStream, TcpServerSocketHandler streamManager)
		{
			NegotiateStream negotiateStream = null;
			WindowsIdentity windowsIdentity;
			try
			{
				negotiateStream = new NegotiateStream(netStream);
				TokenImpersonationLevel tokenImpersonationLevel = TokenImpersonationLevel.Identification;
				if (this._impersonate)
				{
					tokenImpersonationLevel = TokenImpersonationLevel.Impersonation;
				}
				negotiateStream.AuthenticateAsServer((NetworkCredential)CredentialCache.DefaultCredentials, this._protectionLevel, tokenImpersonationLevel);
				netStream = negotiateStream;
				windowsIdentity = (WindowsIdentity)negotiateStream.RemoteIdentity;
			}
			catch
			{
				streamManager.SendErrorResponse(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_ServerAuthenticationFailed"), new object[0]), false);
				if (negotiateStream != null)
				{
					negotiateStream.Close();
				}
				throw;
			}
			return windowsIdentity;
		}

		// Token: 0x04000192 RID: 402
		private int _channelPriority = 1;

		// Token: 0x04000193 RID: 403
		private string _channelName = "tcp";

		// Token: 0x04000194 RID: 404
		private string _machineName;

		// Token: 0x04000195 RID: 405
		private int _port = -1;

		// Token: 0x04000196 RID: 406
		private ChannelDataStore _channelData;

		// Token: 0x04000197 RID: 407
		private string _forcedMachineName;

		// Token: 0x04000198 RID: 408
		private bool _bUseIpAddress = true;

		// Token: 0x04000199 RID: 409
		private IPAddress _bindToAddr = (Socket.SupportsIPv4 ? IPAddress.Any : IPAddress.IPv6Any);

		// Token: 0x0400019A RID: 410
		private bool _bSuppressChannelData;

		// Token: 0x0400019B RID: 411
		private bool _impersonate;

		// Token: 0x0400019C RID: 412
		private ProtectionLevel _protectionLevel = ProtectionLevel.EncryptAndSign;

		// Token: 0x0400019D RID: 413
		private bool _secure;

		// Token: 0x0400019E RID: 414
		private AsyncCallback _acceptSocketCallback;

		// Token: 0x0400019F RID: 415
		private IAuthorizeRemotingConnection _authorizeRemotingConnection;

		// Token: 0x040001A0 RID: 416
		private bool authSet;

		// Token: 0x040001A1 RID: 417
		private IServerChannelSinkProvider _sinkProvider;

		// Token: 0x040001A2 RID: 418
		private TcpServerTransportSink _transportSink;

		// Token: 0x040001A3 RID: 419
		private ExclusiveTcpListener _tcpListener;

		// Token: 0x040001A4 RID: 420
		private bool _bExclusiveAddressUse = true;

		// Token: 0x040001A5 RID: 421
		private bool _bListening;
	}
}
