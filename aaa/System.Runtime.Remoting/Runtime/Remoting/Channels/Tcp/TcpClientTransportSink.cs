using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Security.Principal;
using System.Threading;

namespace System.Runtime.Remoting.Channels.Tcp
{
	// Token: 0x02000040 RID: 64
	internal class TcpClientTransportSink : BaseChannelSinkWithProperties, IClientChannelSink, IChannelSinkBase
	{
		// Token: 0x0600020D RID: 525 RVA: 0x0000A3F4 File Offset: 0x000093F4
		private SocketHandler CreateSocketHandler(Socket socket, SocketCache socketCache, string machinePortAndSid)
		{
			Stream stream = new SocketStream(socket);
			if (this._channel.IsSecured)
			{
				stream = this.CreateAuthenticatedStream(stream, machinePortAndSid);
			}
			return new TcpClientSocketHandler(socket, machinePortAndSid, stream, this);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000A428 File Offset: 0x00009428
		private Stream CreateAuthenticatedStream(Stream netStream, string machinePortAndSid)
		{
			NegotiateStream negotiateStream = null;
			NetworkCredential networkCredential;
			if (this._securityUserName != null)
			{
				networkCredential = new NetworkCredential(this._securityUserName, this._securityPassword, this._securityDomain);
			}
			else
			{
				networkCredential = (NetworkCredential)CredentialCache.DefaultCredentials;
			}
			try
			{
				negotiateStream = new NegotiateStream(netStream);
				negotiateStream.AuthenticateAsClient(networkCredential, this._spn, this._protectionLevel, this._tokenImpersonationLevel);
			}
			catch (IOException ex)
			{
				throw new RemotingException(string.Format(CultureInfo.CurrentCulture, CoreChannel.GetResourceString("Remoting_Tcp_AuthenticationFailed"), new object[0]), ex);
			}
			return negotiateStream;
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0000A4BC File Offset: 0x000094BC
		private string GetSid()
		{
			if (this._connectionGroupName != null)
			{
				return this._connectionGroupName;
			}
			return CoreChannel.GetCurrentSidString();
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0000A4D4 File Offset: 0x000094D4
		internal TcpClientTransportSink(string channelURI, TcpClientChannel channel)
		{
			/*
An exception occurred when decompiling this method (06000210)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.Void System.Runtime.Remoting.Channels.Tcp.TcpClientTransportSink::.ctor(System.String,System.Runtime.Remoting.Channels.Tcp.TcpClientChannel)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at dnlib.DotNet.AssemblyNameComparer.CompareTo(IAssembly a, IAssembly b)
   at dnSpy.Documents.DsDocumentService.FindAssembly(IAssembly assembly, FindAssemblyOptions options) in D:\a\dnSpy\dnSpy\dnSpy\dnSpy\Documents\DsDocumentService.cs:line 178
   at dnSpy.Documents.AssemblyResolver.ResolveNormal(IAssembly assembly, ModuleDef sourceModule) in D:\a\dnSpy\dnSpy\dnSpy\dnSpy\Documents\AssemblyResolver.cs:line 582
   at dnlib.DotNet.Resolver.Resolve(TypeRef typeRef, ModuleDef sourceModule)
   at dnlib.DotNet.Extensions.ToTypeSig(ITypeDefOrRef type, Boolean resolveToCheckValueType)
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.DoInferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 440
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.InferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 309
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.DoInferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 908
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.InferTypeForExpression(ILExpression expr, TypeSig expectedType, Boolean forceInferChildren) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 309
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 284
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference() in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 220
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.Run(DecompilerContext context, ILBlock method) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 49
   at ICSharpCode.Decompiler.ILAst.ILAstOptimizer.Optimize(DecompilerContext context, ILBlock method, AutoPropertyProvider autoPropertyProvider, StateMachineKind& stateMachineKind, MethodDef& inlinedMethod, AsyncMethodDebugInfo& asyncInfo, ILAstOptimizationStep abortBeforeStep) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\ILAstOptimizer.cs:line 264
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(IEnumerable`1 parameters, MethodDebugInfoBuilder& builder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 123
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 88
   --- End of inner exception stack trace ---
   at ICSharpCode.Decompiler.Ast.AstMethodBodyBuilder.CreateMethodBody(MethodDef methodDef, DecompilerContext context, AutoPropertyProvider autoPropertyProvider, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, StringBuilder sb, MethodDebugInfoBuilder& stmtsBuilder) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstMethodBodyBuilder.cs:line 92
   at ICSharpCode.Decompiler.Ast.AstBuilder.AddMethodBody(EntityDeclaration methodNode, EntityDeclaration& updatedNode, MethodDef method, IEnumerable`1 parameters, Boolean valueParameterIsKeyword, MethodKind methodKind) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\Ast\AstBuilder.cs:line 1683
*/;
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0000A600 File Offset: 0x00009600
		public void ProcessMessage(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream, out ITransportHeaders responseHeaders, out Stream responseStream)
		{
			TcpClientSocketHandler tcpClientSocketHandler = this.SendRequestWithRetry(msg, requestHeaders, requestStream);
			responseHeaders = tcpClientSocketHandler.ReadHeaders();
			responseStream = tcpClientSocketHandler.GetResponseStream();
		}

		// Token: 0x06000212 RID: 530 RVA: 0x0000A62C File Offset: 0x0000962C
		public void AsyncProcessRequest(IClientChannelSinkStack sinkStack, IMessage msg, ITransportHeaders headers, Stream stream)
		{
			TcpClientSocketHandler tcpClientSocketHandler = this.SendRequestWithRetry(msg, headers, stream);
			if (tcpClientSocketHandler.OneWayRequest)
			{
				tcpClientSocketHandler.ReturnToCache();
				return;
			}
			tcpClientSocketHandler.DataArrivedCallback = new WaitCallback(this.ReceiveCallback);
			tcpClientSocketHandler.DataArrivedCallbackState = sinkStack;
			tcpClientSocketHandler.BeginReadMessage();
		}

		// Token: 0x06000213 RID: 531 RVA: 0x0000A672 File Offset: 0x00009672
		public void AsyncProcessResponse(IClientResponseChannelSinkStack sinkStack, object state, ITransportHeaders headers, Stream stream)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000A679 File Offset: 0x00009679
		public Stream GetRequestStream(IMessage msg, ITransportHeaders headers)
		{
			return null;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000215 RID: 533 RVA: 0x0000A67C File Offset: 0x0000967C
		public IClientChannelSink NextChannelSink
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06000216 RID: 534 RVA: 0x0000A680 File Offset: 0x00009680
		private TcpClientSocketHandler SendRequestWithRetry(IMessage msg, ITransportHeaders requestHeaders, Stream requestStream)
		{
			long num = 0L;
			bool flag = true;
			bool canSeek = requestStream.CanSeek;
			if (canSeek)
			{
				num = requestStream.Position;
			}
			TcpClientSocketHandler tcpClientSocketHandler = null;
			string text = this._machineAndPort + (this._channel.IsSecured ? ("/" + this.GetSid()) : null);
			if (this.authSet && !this._channel.IsSecured)
			{
				throw new RemotingException(CoreChannel.GetResourceString("Remoting_Tcp_AuthenticationConfigClient"));
			}
			bool flag2 = this._channel.IsSecured && this._securityUserName != null && this._connectionGroupName == null;
			try
			{
				tcpClientSocketHandler = (TcpClientSocketHandler)this.ClientSocketCache.GetSocket(text, flag2);
				tcpClientSocketHandler.SendRequest(msg, requestHeaders, requestStream);
			}
			catch (SocketException)
			{
				int num2 = 0;
				while (num2 < this._retryCount && canSeek && flag)
				{
					try
					{
						requestStream.Position = num;
						tcpClientSocketHandler = (TcpClientSocketHandler)this.ClientSocketCache.GetSocket(text, flag2);
						tcpClientSocketHandler.SendRequest(msg, requestHeaders, requestStream);
						flag = false;
					}
					catch (SocketException)
					{
					}
					num2++;
				}
				if (flag)
				{
					throw;
				}
			}
			requestStream.Close();
			return tcpClientSocketHandler;
		}

		// Token: 0x06000217 RID: 535 RVA: 0x0000A7AC File Offset: 0x000097AC
		private void ReceiveCallback(object state)
		{
			IClientChannelSinkStack clientChannelSinkStack = null;
			try
			{
				TcpClientSocketHandler tcpClientSocketHandler = (TcpClientSocketHandler)state;
				clientChannelSinkStack = (IClientChannelSinkStack)tcpClientSocketHandler.DataArrivedCallbackState;
				ITransportHeaders transportHeaders = tcpClientSocketHandler.ReadHeaders();
				Stream responseStream = tcpClientSocketHandler.GetResponseStream();
				clientChannelSinkStack.AsyncProcessResponse(transportHeaders, responseStream);
			}
			catch (Exception ex)
			{
				try
				{
					if (clientChannelSinkStack != null)
					{
						clientChannelSinkStack.DispatchException(ex);
					}
				}
				catch
				{
				}
			}
			catch
			{
				try
				{
					if (clientChannelSinkStack != null)
					{
						clientChannelSinkStack.DispatchException(new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")));
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x1700007D RID: 125
		public override object this[object key]
		{
			get
			{
				string text = key as string;
				if (text == null)
				{
					return null;
				}
				string text2;
				switch (text2 = text.ToLower(CultureInfo.InvariantCulture))
				{
				case "username":
					return this._securityUserName;
				case "password":
					return null;
				case "domain":
					return this._securityDomain;
				case "socketcachetimeout":
					return this._socketCacheTimeout;
				case "timeout":
					return this._receiveTimeout;
				case "socketcachepolicy":
					return this._socketCachePolicy.ToString();
				case "retrycount":
					return this._retryCount;
				case "connectiongroupname":
					return this._connectionGroupName;
				case "tokenimpersonationlevel":
					if (this.authSet)
					{
						return this._tokenImpersonationLevel.ToString();
					}
					break;
				case "protectionlevel":
					if (this.authSet)
					{
						return this._protectionLevel.ToString();
					}
					break;
				}
				return null;
			}
			set
			{
				string text = key as string;
				if (text == null)
				{
					return;
				}
				string text2;
				switch (text2 = text.ToLower(CultureInfo.InvariantCulture))
				{
				case "username":
					this._securityUserName = (string)value;
					return;
				case "password":
					this._securityPassword = (string)value;
					return;
				case "domain":
					this._securityDomain = (string)value;
					return;
				case "socketcachetimeout":
				{
					int num2 = Convert.ToInt32(value, CultureInfo.InvariantCulture);
					if (num2 < 0)
					{
						throw new RemotingException(CoreChannel.GetResourceString("Remoting_Tcp_SocketTimeoutNegative"));
					}
					this._socketCacheTimeout = TimeSpan.FromSeconds((double)num2);
					this.ClientSocketCache.SocketTimeout = this._socketCacheTimeout;
					return;
				}
				case "timeout":
					this._receiveTimeout = Convert.ToInt32(value, CultureInfo.InvariantCulture);
					this.ClientSocketCache.ReceiveTimeout = this._receiveTimeout;
					return;
				case "socketcachepolicy":
					this._socketCachePolicy = (SocketCachePolicy)((value is SocketCachePolicy) ? value : Enum.Parse(typeof(SocketCachePolicy), (string)value, true));
					this.ClientSocketCache.CachePolicy = this._socketCachePolicy;
					return;
				case "retrycount":
					this._retryCount = Convert.ToInt32(value, CultureInfo.InvariantCulture);
					return;
				case "connectiongroupname":
					this._connectionGroupName = (string)value;
					return;
				case "tokenimpersonationlevel":
					this._tokenImpersonationLevel = (TokenImpersonationLevel)((value is TokenImpersonationLevel) ? value : Enum.Parse(typeof(TokenImpersonationLevel), (string)value, true));
					this.authSet = true;
					return;
				case "protectionlevel":
					this._protectionLevel = (ProtectionLevel)((value is ProtectionLevel) ? value : Enum.Parse(typeof(ProtectionLevel), (string)value, true));
					this.authSet = true;
					return;
				case "serviceprincipalname":
					this._spn = (string)value;
					this.authSet = true;
					break;

					return;
				}
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600021A RID: 538 RVA: 0x0000AC48 File Offset: 0x00009C48
		public override ICollection Keys
		{
			get
			{
				if (TcpClientTransportSink.s_keySet == null)
				{
					TcpClientTransportSink.s_keySet = new ArrayList(6) { "username", "password", "domain", "socketcachetimeout", "socketcachepolicy", "retrycount", "tokenimpersonationlevel", "protectionlevel", "connectiongroupname", "timeout" };
				}
				return TcpClientTransportSink.s_keySet;
			}
		}

		// Token: 0x0400016B RID: 363
		private const string UserNameKey = "username";

		// Token: 0x0400016C RID: 364
		private const string PasswordKey = "password";

		// Token: 0x0400016D RID: 365
		private const string DomainKey = "domain";

		// Token: 0x0400016E RID: 366
		private const string ProtectionLevelKey = "protectionlevel";

		// Token: 0x0400016F RID: 367
		private const string ConnectionGroupNameKey = "connectiongroupname";

		// Token: 0x04000170 RID: 368
		private const string TokenImpersonationLevelKey = "tokenimpersonationlevel";

		// Token: 0x04000171 RID: 369
		private const string SocketCacheTimeoutKey = "socketcachetimeout";

		// Token: 0x04000172 RID: 370
		private const string ReceiveTimeoutKey = "timeout";

		// Token: 0x04000173 RID: 371
		private const string SocketCachePolicyKey = "socketcachepolicy";

		// Token: 0x04000174 RID: 372
		private const string SPNKey = "serviceprincipalname";

		// Token: 0x04000175 RID: 373
		private const string RetryCountKey = "retrycount";

		// Token: 0x04000176 RID: 374
		internal SocketCache ClientSocketCache;

		// Token: 0x04000177 RID: 375
		private bool authSet;

		// Token: 0x04000178 RID: 376
		private string m_machineName;

		// Token: 0x04000179 RID: 377
		private int m_port;

		// Token: 0x0400017A RID: 378
		private TcpClientChannel _channel;

		// Token: 0x0400017B RID: 379
		private string _machineAndPort;

		// Token: 0x0400017C RID: 380
		private string _securityUserName;

		// Token: 0x0400017D RID: 381
		private string _securityPassword;

		// Token: 0x0400017E RID: 382
		private string _securityDomain;

		// Token: 0x0400017F RID: 383
		private string _connectionGroupName;

		// Token: 0x04000180 RID: 384
		private TimeSpan _socketCacheTimeout;

		// Token: 0x04000181 RID: 385
		private int _receiveTimeout;

		// Token: 0x04000182 RID: 386
		private SocketCachePolicy _socketCachePolicy;

		// Token: 0x04000183 RID: 387
		private string _spn;

		// Token: 0x04000184 RID: 388
		private int _retryCount;

		// Token: 0x04000185 RID: 389
		private TokenImpersonationLevel _tokenImpersonationLevel;

		// Token: 0x04000186 RID: 390
		private ProtectionLevel _protectionLevel;

		// Token: 0x04000187 RID: 391
		private static ICollection s_keySet;
	}
}
