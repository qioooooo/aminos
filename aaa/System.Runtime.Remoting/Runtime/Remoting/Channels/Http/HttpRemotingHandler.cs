using System;
using System.IO;
using System.Text;
using System.Web;

namespace System.Runtime.Remoting.Channels.Http
{
	// Token: 0x02000038 RID: 56
	public class HttpRemotingHandler : IHttpHandler
	{
		// Token: 0x060001D0 RID: 464 RVA: 0x000091EF File Offset: 0x000081EF
		public HttpRemotingHandler()
		{
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x000091F7 File Offset: 0x000081F7
		public HttpRemotingHandler(Type type, object srvID)
		{
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x000091FF File Offset: 0x000081FF
		public void ProcessRequest(HttpContext context)
		{
			this.InternalProcessRequest(context);
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00009208 File Offset: 0x00008208
		private void InternalProcessRequest(HttpContext context)
		{
			try
			{
				HttpRequest request = context.Request;
				if (!HttpRemotingHandler.bLoadedConfiguration)
				{
					lock (HttpRemotingHandler.ApplicationConfigurationFile)
					{
						if (!HttpRemotingHandler.bLoadedConfiguration)
						{
							IisHelper.Initialize();
							if (RemotingConfiguration.ApplicationName == null)
							{
								RemotingConfiguration.ApplicationName = request.ApplicationPath;
							}
							string text = request.PhysicalApplicationPath + HttpRemotingHandler.ApplicationConfigurationFile;
							if (File.Exists(text))
							{
								try
								{
									RemotingConfiguration.Configure(text, false);
								}
								catch (Exception ex)
								{
									HttpRemotingHandler.s_fatalException = ex;
									this.WriteException(context, ex);
									return;
								}
								catch
								{
									Exception ex2 = new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException"));
									HttpRemotingHandler.s_fatalException = ex2;
									this.WriteException(context, ex2);
									return;
								}
							}
							try
							{
								IChannelReceiverHook channelReceiverHook = null;
								IChannel[] registeredChannels = ChannelServices.RegisteredChannels;
								foreach (IChannel channel in registeredChannels)
								{
									IChannelReceiverHook channelReceiverHook2 = channel as IChannelReceiverHook;
									if (channelReceiverHook2 != null && string.Compare(channelReceiverHook2.ChannelScheme, "http", StringComparison.OrdinalIgnoreCase) == 0 && channelReceiverHook2.WantsToListen)
									{
										channelReceiverHook = channelReceiverHook2;
										break;
									}
								}
								if (channelReceiverHook == null)
								{
									HttpChannel httpChannel = new HttpChannel();
									ChannelServices.RegisterChannel(httpChannel, false);
									channelReceiverHook = httpChannel;
								}
								string text2;
								if (IisHelper.IsSslRequired)
								{
									text2 = "https";
								}
								else
								{
									text2 = "http";
								}
								string text3 = text2 + "://" + CoreChannel.GetMachineIp();
								int port = context.Request.Url.Port;
								string text4 = string.Concat(new object[]
								{
									":",
									port,
									"/",
									RemotingConfiguration.ApplicationName
								});
								text3 += text4;
								channelReceiverHook.AddHookChannelUri(text3);
								ChannelDataStore channelDataStore = ((IChannelReceiver)channelReceiverHook).ChannelData as ChannelDataStore;
								if (channelDataStore != null)
								{
									text3 = channelDataStore.ChannelUris[0];
								}
								IisHelper.ApplicationUrl = text3;
								ChannelServices.UnregisterChannel(null);
								HttpRemotingHandler.s_transportSink = new HttpHandlerTransportSink(channelReceiverHook.ChannelSinkChain);
							}
							catch (Exception ex3)
							{
								HttpRemotingHandler.s_fatalException = ex3;
								this.WriteException(context, ex3);
								return;
							}
							catch
							{
								Exception ex4 = new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException"));
								HttpRemotingHandler.s_fatalException = ex4;
								this.WriteException(context, ex4);
								return;
							}
							HttpRemotingHandler.bLoadedConfiguration = true;
						}
					}
				}
				if (HttpRemotingHandler.s_fatalException == null)
				{
					if (!this.CanServiceRequest(context))
					{
						this.WriteException(context, new RemotingException(CoreChannel.GetResourceString("Remoting_ChnlSink_UriNotPublished")));
					}
					else
					{
						HttpRemotingHandler.s_transportSink.HandleRequest(context);
					}
				}
				else
				{
					this.WriteException(context, HttpRemotingHandler.s_fatalException);
				}
			}
			catch (Exception ex5)
			{
				this.WriteException(context, ex5);
			}
			catch
			{
				this.WriteException(context, new Exception(CoreChannel.GetResourceString("Remoting_nonClsCompliantException")));
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060001D4 RID: 468 RVA: 0x0000954C File Offset: 0x0000854C
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x00009550 File Offset: 0x00008550
		private string ComposeContentType(string contentType, Encoding encoding)
		{
			if (encoding != null)
			{
				StringBuilder stringBuilder = new StringBuilder(contentType);
				stringBuilder.Append("; charset=");
				stringBuilder.Append(encoding.WebName);
				return stringBuilder.ToString();
			}
			return contentType;
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x00009588 File Offset: 0x00008588
		private bool CanServiceRequest(HttpContext context)
		{
			string text = this.GetRequestUriForCurrentRequest(context);
			string objectUriFromRequestUri = HttpChannelHelper.GetObjectUriFromRequestUri(text);
			context.Items["__requestUri"] = text;
			if (string.Compare(context.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase) != 0)
			{
				if (RemotingServices.GetServerTypeForUri(text) != null)
				{
					return true;
				}
			}
			else
			{
				if (context.Request.QueryString.Count != 1)
				{
					return false;
				}
				string[] values = context.Request.QueryString.GetValues(0);
				if (values.Length != 1 || string.Compare(values[0], "wsdl", StringComparison.OrdinalIgnoreCase) != 0)
				{
					return false;
				}
				if (string.Compare(objectUriFromRequestUri, "RemoteApplicationMetadata.rem", StringComparison.OrdinalIgnoreCase) == 0)
				{
					return true;
				}
				int num = text.LastIndexOf('?');
				if (num != -1)
				{
					text = text.Substring(0, num);
				}
				if (RemotingServices.GetServerTypeForUri(text) != null)
				{
					return true;
				}
			}
			return File.Exists(context.Request.PhysicalPath);
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000965C File Offset: 0x0000865C
		private string GetRequestUriForCurrentRequest(HttpContext context)
		{
			string rawUrl = context.Request.RawUrl;
			string text;
			if (HttpChannelHelper.ParseURL(rawUrl, out text) == null)
			{
				text = rawUrl;
			}
			string applicationName = RemotingConfiguration.ApplicationName;
			if (applicationName != null && applicationName.Length > 0 && text.Length > applicationName.Length)
			{
				text = text.Substring(applicationName.Length + 1);
			}
			return text;
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x000096B3 File Offset: 0x000086B3
		private string GenerateFaultString(HttpContext context, Exception e)
		{
			/*
An exception occurred when decompiling this method (060001D8)

ICSharpCode.Decompiler.DecompilerException: Error decompiling System.String System.Runtime.Remoting.Channels.Http.HttpRemotingHandler::GenerateFaultString(System.Web.HttpContext,System.Exception)

 ---> System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
   at System.Collections.Generic.List`1.System.Collections.Generic.IEnumerable<T>.GetEnumerator()
   at System.Linq.Enumerable.Any[TSource](IEnumerable`1 source, Func`2 predicate)
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 282
   at ICSharpCode.Decompiler.ILAst.TypeAnalysis.RunInference(ILExpression expr) in D:\a\dnSpy\dnSpy\Extensions\ILSpy.Decompiler\ICSharpCode.Decompiler\ICSharpCode.Decompiler\ILAst\TypeAnalysis.cs:line 288
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

		// Token: 0x060001D9 RID: 473 RVA: 0x000096D0 File Offset: 0x000086D0
		private void WriteException(HttpContext context, Exception e)
		{
			Stream outputStream = context.Response.OutputStream;
			context.Response.Clear();
			context.Response.ClearHeaders();
			context.Response.ContentType = this.ComposeContentType("text/plain", Encoding.UTF8);
			HttpRemotingHandler.SetHttpResponseStatusCode(context.Response, 500);
			context.Response.StatusDescription = CoreChannel.GetResourceString("Remoting_InternalError");
			StreamWriter streamWriter = new StreamWriter(outputStream, new UTF8Encoding(false));
			streamWriter.WriteLine(this.GenerateFaultString(context, e));
			streamWriter.Flush();
		}

		// Token: 0x060001DA RID: 474 RVA: 0x00009760 File Offset: 0x00008760
		internal static bool IsLocal(HttpContext context)
		{
			string text = context.Request.ServerVariables["LOCAL_ADDR"];
			string userHostAddress = context.Request.UserHostAddress;
			return context.Request.Url.IsLoopback || (text != null && userHostAddress != null && text == userHostAddress);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x000097B4 File Offset: 0x000087B4
		internal static bool CustomErrorsEnabled(HttpContext context)
		{
			bool flag;
			try
			{
				if (!context.IsCustomErrorEnabled)
				{
					flag = false;
				}
				else
				{
					flag = RemotingConfiguration.CustomErrorsEnabled(HttpRemotingHandler.IsLocal(context));
				}
			}
			catch
			{
				flag = true;
			}
			return flag;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x000097F4 File Offset: 0x000087F4
		internal static void SetHttpResponseStatusCode(HttpResponse httpResponse, int statusCode)
		{
			httpResponse.TrySkipIisCustomErrors = true;
			httpResponse.StatusCode = statusCode;
		}

		// Token: 0x04000156 RID: 342
		private static string ApplicationConfigurationFile = "web.config";

		// Token: 0x04000157 RID: 343
		private static bool bLoadedConfiguration = false;

		// Token: 0x04000158 RID: 344
		private static HttpHandlerTransportSink s_transportSink = null;

		// Token: 0x04000159 RID: 345
		private static Exception s_fatalException = null;
	}
}
