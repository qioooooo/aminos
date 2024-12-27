using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Services.Diagnostics;

namespace System.Web.Services.Protocols
{
	// Token: 0x0200003D RID: 61
	[ComVisible(true)]
	public abstract class HttpSimpleClientProtocol : HttpWebClientProtocol
	{
		// Token: 0x06000154 RID: 340 RVA: 0x0000594C File Offset: 0x0000494C
		protected HttpSimpleClientProtocol()
		{
			Type type = base.GetType();
			this.clientType = (HttpClientType)WebClientProtocol.GetFromCache(type);
			if (this.clientType == null)
			{
				lock (WebClientProtocol.InternalSyncObject)
				{
					this.clientType = (HttpClientType)WebClientProtocol.GetFromCache(type);
					if (this.clientType == null)
					{
						this.clientType = new HttpClientType(type);
						WebClientProtocol.AddToCache(type, this.clientType);
					}
				}
			}
		}

		// Token: 0x06000155 RID: 341 RVA: 0x000059D8 File Offset: 0x000049D8
		protected object Invoke(string methodName, string requestUrl, object[] parameters)
		{
			HttpClientMethod clientMethod = this.GetClientMethod(methodName);
			MimeParameterWriter parameterWriter = this.GetParameterWriter(clientMethod);
			Uri uri = new Uri(requestUrl);
			if (parameterWriter != null)
			{
				parameterWriter.RequestEncoding = base.RequestEncoding;
				requestUrl = parameterWriter.GetRequestUrl(uri.AbsoluteUri, parameters);
				uri = new Uri(requestUrl, true);
			}
			WebRequest webRequest = null;
			object obj;
			try
			{
				webRequest = this.GetWebRequest(uri);
				base.NotifyClientCallOut(webRequest);
				base.PendingSyncRequest = webRequest;
				if (parameterWriter != null)
				{
					parameterWriter.InitializeRequest(webRequest, parameters);
					if (parameterWriter.UsesWriteRequest)
					{
						if (parameters.Length == 0)
						{
							webRequest.ContentLength = 0L;
						}
						else
						{
							Stream stream = null;
							try
							{
								stream = webRequest.GetRequestStream();
								parameterWriter.WriteRequest(stream, parameters);
							}
							finally
							{
								if (stream != null)
								{
									stream.Close();
								}
							}
						}
					}
				}
				WebResponse webResponse = this.GetWebResponse(webRequest);
				Stream stream2 = null;
				if (webResponse.ContentLength != 0L)
				{
					stream2 = webResponse.GetResponseStream();
				}
				obj = this.ReadResponse(clientMethod, webResponse, stream2);
			}
			finally
			{
				if (webRequest == base.PendingSyncRequest)
				{
					base.PendingSyncRequest = null;
				}
			}
			return obj;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00005AE4 File Offset: 0x00004AE4
		protected IAsyncResult BeginInvoke(string methodName, string requestUrl, object[] parameters, AsyncCallback callback, object asyncState)
		{
			HttpClientMethod clientMethod = this.GetClientMethod(methodName);
			MimeParameterWriter parameterWriter = this.GetParameterWriter(clientMethod);
			Uri uri = new Uri(requestUrl);
			if (parameterWriter != null)
			{
				parameterWriter.RequestEncoding = base.RequestEncoding;
				requestUrl = parameterWriter.GetRequestUrl(uri.AbsoluteUri, parameters);
				uri = new Uri(requestUrl, true);
			}
			HttpSimpleClientProtocol.InvokeAsyncState invokeAsyncState = new HttpSimpleClientProtocol.InvokeAsyncState(clientMethod, parameterWriter, parameters);
			WebClientAsyncResult webClientAsyncResult = new WebClientAsyncResult(this, invokeAsyncState, null, callback, asyncState);
			return base.BeginSend(uri, webClientAsyncResult, parameterWriter.UsesWriteRequest);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x00005B54 File Offset: 0x00004B54
		internal override void InitializeAsyncRequest(WebRequest request, object internalAsyncState)
		{
			HttpSimpleClientProtocol.InvokeAsyncState invokeAsyncState = (HttpSimpleClientProtocol.InvokeAsyncState)internalAsyncState;
			if (invokeAsyncState.ParamWriter.UsesWriteRequest && invokeAsyncState.Parameters.Length == 0)
			{
				request.ContentLength = 0L;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x00005B88 File Offset: 0x00004B88
		internal override void AsyncBufferedSerialize(WebRequest request, Stream requestStream, object internalAsyncState)
		{
			HttpSimpleClientProtocol.InvokeAsyncState invokeAsyncState = (HttpSimpleClientProtocol.InvokeAsyncState)internalAsyncState;
			if (invokeAsyncState.ParamWriter != null)
			{
				invokeAsyncState.ParamWriter.InitializeRequest(request, invokeAsyncState.Parameters);
				if (invokeAsyncState.ParamWriter.UsesWriteRequest && invokeAsyncState.Parameters.Length > 0)
				{
					invokeAsyncState.ParamWriter.WriteRequest(requestStream, invokeAsyncState.Parameters);
				}
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00005BE0 File Offset: 0x00004BE0
		protected object EndInvoke(IAsyncResult asyncResult)
		{
			object obj = null;
			Stream stream = null;
			WebResponse webResponse = base.EndSend(asyncResult, ref obj, ref stream);
			HttpSimpleClientProtocol.InvokeAsyncState invokeAsyncState = (HttpSimpleClientProtocol.InvokeAsyncState)obj;
			return this.ReadResponse(invokeAsyncState.Method, webResponse, stream);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x00005C14 File Offset: 0x00004C14
		private void InvokeAsyncCallback(IAsyncResult result)
		{
			object obj = null;
			Exception ex = null;
			WebClientAsyncResult webClientAsyncResult = (WebClientAsyncResult)result;
			if (webClientAsyncResult.Request != null)
			{
				try
				{
					object obj2 = null;
					Stream stream = null;
					WebResponse webResponse = base.EndSend(webClientAsyncResult, ref obj2, ref stream);
					HttpSimpleClientProtocol.InvokeAsyncState invokeAsyncState = (HttpSimpleClientProtocol.InvokeAsyncState)obj2;
					obj = this.ReadResponse(invokeAsyncState.Method, webResponse, stream);
				}
				catch (Exception ex2)
				{
					if (ex2 is ThreadAbortException || ex2 is StackOverflowException || ex2 is OutOfMemoryException)
					{
						throw;
					}
					ex = ex2;
					if (Tracing.On)
					{
						Tracing.ExceptionCatch(TraceEventType.Error, this, "InvokeAsyncCallback", ex2);
					}
				}
				catch
				{
					ex = new Exception(Res.GetString("NonClsCompliantException"));
				}
			}
			AsyncOperation asyncOperation = (AsyncOperation)result.AsyncState;
			UserToken userToken = (UserToken)asyncOperation.UserSuppliedState;
			base.OperationCompleted(userToken.UserState, new object[] { obj }, ex, false);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00005D08 File Offset: 0x00004D08
		protected void InvokeAsync(string methodName, string requestUrl, object[] parameters, SendOrPostCallback callback)
		{
			this.InvokeAsync(methodName, requestUrl, parameters, callback, null);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00005D18 File Offset: 0x00004D18
		protected void InvokeAsync(string methodName, string requestUrl, object[] parameters, SendOrPostCallback callback, object userState)
		{
			if (userState == null)
			{
				userState = base.NullToken;
			}
			AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(new UserToken(callback, userState));
			WebClientAsyncResult webClientAsyncResult = new WebClientAsyncResult(this, null, null, new AsyncCallback(this.InvokeAsyncCallback), asyncOperation);
			try
			{
				base.AsyncInvokes.Add(userState, webClientAsyncResult);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Error, this, "InvokeAsync", ex);
				}
				Exception ex2 = new ArgumentException(Res.GetString("AsyncDuplicateUserState"), ex);
				object[] array = new object[1];
				InvokeCompletedEventArgs invokeCompletedEventArgs = new InvokeCompletedEventArgs(array, ex2, false, userState);
				asyncOperation.PostOperationCompleted(callback, invokeCompletedEventArgs);
				return;
			}
			catch
			{
				Exception ex3 = new ArgumentException(Res.GetString("AsyncDuplicateUserState"), new Exception(Res.GetString("NonClsCompliantException")));
				object[] array2 = new object[1];
				InvokeCompletedEventArgs invokeCompletedEventArgs2 = new InvokeCompletedEventArgs(array2, ex3, false, userState);
				asyncOperation.PostOperationCompleted(callback, invokeCompletedEventArgs2);
				return;
			}
			try
			{
				HttpClientMethod clientMethod = this.GetClientMethod(methodName);
				MimeParameterWriter parameterWriter = this.GetParameterWriter(clientMethod);
				Uri uri = new Uri(requestUrl);
				if (parameterWriter != null)
				{
					parameterWriter.RequestEncoding = base.RequestEncoding;
					requestUrl = parameterWriter.GetRequestUrl(uri.AbsoluteUri, parameters);
					uri = new Uri(requestUrl, true);
				}
				webClientAsyncResult.InternalAsyncState = new HttpSimpleClientProtocol.InvokeAsyncState(clientMethod, parameterWriter, parameters);
				base.BeginSend(uri, webClientAsyncResult, parameterWriter.UsesWriteRequest);
			}
			catch (Exception ex4)
			{
				if (ex4 is ThreadAbortException || ex4 is StackOverflowException || ex4 is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Error, this, "InvokeAsync", ex4);
				}
				object obj = userState;
				object[] array3 = new object[1];
				base.OperationCompleted(obj, array3, ex4, false);
			}
			catch
			{
				object obj2 = userState;
				object[] array4 = new object[1];
				base.OperationCompleted(obj2, array4, new Exception(Res.GetString("NonClsCompliantException")), false);
			}
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00005F24 File Offset: 0x00004F24
		private MimeParameterWriter GetParameterWriter(HttpClientMethod method)
		{
			if (method.writerType == null)
			{
				return null;
			}
			return (MimeParameterWriter)MimeFormatter.CreateInstance(method.writerType, method.writerInitializer);
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00005F48 File Offset: 0x00004F48
		private HttpClientMethod GetClientMethod(string methodName)
		{
			HttpClientMethod method = this.clientType.GetMethod(methodName);
			if (method == null)
			{
				throw new ArgumentException(Res.GetString("WebInvalidMethodName", new object[] { methodName }), "methodName");
			}
			return method;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00005F88 File Offset: 0x00004F88
		private object ReadResponse(HttpClientMethod method, WebResponse response, Stream responseStream)
		{
			HttpWebResponse httpWebResponse = response as HttpWebResponse;
			if (httpWebResponse != null && httpWebResponse.StatusCode >= HttpStatusCode.MultipleChoices)
			{
				throw new WebException(RequestResponseUtils.CreateResponseExceptionString(httpWebResponse, responseStream), null, WebExceptionStatus.ProtocolError, httpWebResponse);
			}
			if (method.readerType == null)
			{
				return null;
			}
			if (responseStream != null)
			{
				MimeReturnReader mimeReturnReader = (MimeReturnReader)MimeFormatter.CreateInstance(method.readerType, method.readerInitializer);
				return mimeReturnReader.Read(response, responseStream);
			}
			return null;
		}

		// Token: 0x04000286 RID: 646
		private HttpClientType clientType;

		// Token: 0x0200003E RID: 62
		private class InvokeAsyncState
		{
			// Token: 0x06000160 RID: 352 RVA: 0x00005FEA File Offset: 0x00004FEA
			internal InvokeAsyncState(HttpClientMethod method, MimeParameterWriter paramWriter, object[] parameters)
			{
				this.Method = method;
				this.ParamWriter = paramWriter;
				this.Parameters = parameters;
			}

			// Token: 0x04000287 RID: 647
			internal object[] Parameters;

			// Token: 0x04000288 RID: 648
			internal MimeParameterWriter ParamWriter;

			// Token: 0x04000289 RID: 649
			internal HttpClientMethod Method;
		}
	}
}
