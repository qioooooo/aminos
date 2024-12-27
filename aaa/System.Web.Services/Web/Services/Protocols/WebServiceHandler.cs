using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Web.Services.Diagnostics;
using System.Web.Util;

namespace System.Web.Services.Protocols
{
	// Token: 0x02000089 RID: 137
	internal class WebServiceHandler
	{
		// Token: 0x06000398 RID: 920 RVA: 0x00011E30 File Offset: 0x00010E30
		internal WebServiceHandler(ServerProtocol protocol)
		{
			this.protocol = protocol;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00011E3F File Offset: 0x00010E3F
		private static void TraceFlush()
		{
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00011E44 File Offset: 0x00010E44
		private void PrepareContext()
		{
			this.exception = null;
			this.wroteException = false;
			this.asyncCallback = null;
			this.asyncBeginComplete = new ManualResetEvent(false);
			this.asyncCallbackCalls = 0;
			if (this.protocol.IsOneWay)
			{
				return;
			}
			HttpContext context = this.protocol.Context;
			if (context == null)
			{
				return;
			}
			int cacheDuration = this.protocol.MethodAttribute.CacheDuration;
			if (cacheDuration > 0)
			{
				context.Response.Cache.SetCacheability(HttpCacheability.Server);
				context.Response.Cache.SetExpires(DateTime.Now.AddSeconds((double)cacheDuration));
				context.Response.Cache.SetSlidingExpiration(false);
				context.Response.Cache.VaryByHeaders["Content-type"] = true;
				context.Response.Cache.VaryByHeaders["SOAPAction"] = true;
				context.Response.Cache.VaryByParams["*"] = true;
			}
			else
			{
				context.Response.Cache.SetNoServerCaching();
				context.Response.Cache.SetMaxAge(TimeSpan.Zero);
			}
			context.Response.BufferOutput = this.protocol.MethodAttribute.BufferResponse;
			context.Response.ContentType = null;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00011F90 File Offset: 0x00010F90
		private void WriteException(Exception e)
		{
			if (this.wroteException)
			{
				return;
			}
			bool traceVerbose = CompModSwitches.Remote.TraceVerbose;
			if (e is TargetInvocationException)
			{
				bool traceVerbose2 = CompModSwitches.Remote.TraceVerbose;
				e = e.InnerException;
			}
			this.wroteException = this.protocol.WriteException(e, this.protocol.Response.OutputStream);
			if (!this.wroteException)
			{
				throw e;
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00011FF8 File Offset: 0x00010FF8
		private void Invoke()
		{
			this.PrepareContext();
			this.protocol.CreateServerInstance();
			RemoteDebugger remoteDebugger = null;
			string text;
			if (!this.protocol.IsOneWay && RemoteDebugger.IsServerCallInEnabled(this.protocol, out text))
			{
				remoteDebugger = new RemoteDebugger();
				remoteDebugger.NotifyServerCallEnter(this.protocol, text);
			}
			try
			{
				TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "Invoke", new object[0]) : null);
				TraceMethod traceMethod2 = (Tracing.On ? new TraceMethod(this.protocol.Target, this.protocol.MethodInfo.Name, this.parameters) : null);
				if (Tracing.On)
				{
					Tracing.Enter(this.protocol.MethodInfo.ToString(), traceMethod, traceMethod2);
				}
				object[] array = this.protocol.MethodInfo.Invoke(this.protocol.Target, this.parameters);
				if (Tracing.On)
				{
					Tracing.Exit(this.protocol.MethodInfo.ToString(), traceMethod);
				}
				this.WriteReturns(array);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Error, this, "Invoke", ex);
				}
				if (!this.protocol.IsOneWay)
				{
					this.WriteException(ex);
					throw;
				}
			}
			catch
			{
			}
			finally
			{
				this.protocol.DisposeServerInstance();
				if (remoteDebugger != null)
				{
					remoteDebugger.NotifyServerCallExit(this.protocol.Response);
				}
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x000121BC File Offset: 0x000111BC
		private void InvokeTransacted()
		{
			Transactions.InvokeTransacted(new TransactedCallback(this.Invoke), this.protocol.MethodAttribute.TransactionOption);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000121DF File Offset: 0x000111DF
		private void ThrowInitException()
		{
			this.HandleOneWayException(new Exception(Res.GetString("WebConfigExtensionError"), this.protocol.OnewayInitException), "ThrowInitException");
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00012206 File Offset: 0x00011206
		private void HandleOneWayException(Exception e, string method)
		{
			if (Tracing.On)
			{
				Tracing.ExceptionCatch(TraceEventType.Error, this, string.IsNullOrEmpty(method) ? "HandleOneWayException" : method, e);
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00012228 File Offset: 0x00011228
		protected void CoreProcessRequest()
		{
			try
			{
				bool transactionEnabled = this.protocol.MethodAttribute.TransactionEnabled;
				if (this.protocol.IsOneWay)
				{
					WorkItemCallback workItemCallback;
					TraceMethod traceMethod;
					if (this.protocol.OnewayInitException != null)
					{
						workItemCallback = new WorkItemCallback(this.ThrowInitException);
						traceMethod = (Tracing.On ? new TraceMethod(this, "ThrowInitException", new object[0]) : null);
					}
					else
					{
						this.parameters = this.protocol.ReadParameters();
						workItemCallback = (transactionEnabled ? new WorkItemCallback(this.OneWayInvokeTransacted) : new WorkItemCallback(this.OneWayInvoke));
						traceMethod = (Tracing.On ? (transactionEnabled ? new TraceMethod(this, "OneWayInvokeTransacted", new object[0]) : new TraceMethod(this, "OneWayInvoke", new object[0])) : null);
					}
					if (Tracing.On)
					{
						Tracing.Information("TracePostWorkItemIn", new object[] { traceMethod });
					}
					WorkItem.Post(workItemCallback);
					if (Tracing.On)
					{
						Tracing.Information("TracePostWorkItemOut", new object[] { traceMethod });
					}
					this.protocol.WriteOneWayResponse();
				}
				else if (transactionEnabled)
				{
					this.parameters = this.protocol.ReadParameters();
					this.InvokeTransacted();
				}
				else
				{
					this.parameters = this.protocol.ReadParameters();
					this.Invoke();
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Error, this, "CoreProcessRequest", ex);
				}
				if (!this.protocol.IsOneWay)
				{
					this.WriteException(ex);
				}
			}
			catch
			{
				if (!this.protocol.IsOneWay)
				{
					this.WriteException(new Exception(Res.GetString("NonClsCompliantException")));
				}
			}
			WebServiceHandler.TraceFlush();
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00012420 File Offset: 0x00011420
		private HttpContext SwitchContext(HttpContext context)
		{
			HttpContext httpContext = HttpContext.Current;
			HttpContext.Current = context;
			return httpContext;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001243C File Offset: 0x0001143C
		private void OneWayInvoke()
		{
			HttpContext httpContext = null;
			if (this.protocol.Context != null)
			{
				httpContext = this.SwitchContext(this.protocol.Context);
			}
			try
			{
				this.Invoke();
			}
			catch (Exception ex)
			{
				this.HandleOneWayException(ex, "OneWayInvoke");
			}
			finally
			{
				if (httpContext != null)
				{
					this.SwitchContext(httpContext);
				}
			}
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x000124AC File Offset: 0x000114AC
		private void OneWayInvokeTransacted()
		{
			HttpContext httpContext = null;
			if (this.protocol.Context != null)
			{
				httpContext = this.SwitchContext(this.protocol.Context);
			}
			try
			{
				this.InvokeTransacted();
			}
			catch (Exception ex)
			{
				this.HandleOneWayException(ex, "OneWayInvokeTransacted");
			}
			finally
			{
				if (httpContext != null)
				{
					this.SwitchContext(httpContext);
				}
			}
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001251C File Offset: 0x0001151C
		private void Callback(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				this.asyncBeginComplete.WaitOne();
			}
			this.DoCallback(result);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00012539 File Offset: 0x00011539
		private void DoCallback(IAsyncResult result)
		{
			if (this.asyncCallback != null && Interlocked.Increment(ref this.asyncCallbackCalls) == 1)
			{
				this.asyncCallback(result);
			}
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x00012560 File Offset: 0x00011560
		protected IAsyncResult BeginCoreProcessRequest(AsyncCallback callback, object asyncState)
		{
			if (this.protocol.MethodAttribute.TransactionEnabled)
			{
				throw new InvalidOperationException(Res.GetString("WebAsyncTransaction"));
			}
			this.parameters = this.protocol.ReadParameters();
			IAsyncResult asyncResult;
			if (this.protocol.IsOneWay)
			{
				TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "OneWayAsyncInvoke", new object[0]) : null);
				if (Tracing.On)
				{
					Tracing.Information("TracePostWorkItemIn", new object[] { traceMethod });
				}
				WorkItem.Post(new WorkItemCallback(this.OneWayAsyncInvoke));
				if (Tracing.On)
				{
					Tracing.Information("TracePostWorkItemOut", new object[] { traceMethod });
				}
				asyncResult = new CompletedAsyncResult(asyncState, true);
				if (callback != null)
				{
					callback(asyncResult);
				}
			}
			else
			{
				asyncResult = this.BeginInvoke(callback, asyncState);
			}
			return asyncResult;
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00012634 File Offset: 0x00011634
		private void OneWayAsyncInvoke()
		{
			if (this.protocol.OnewayInitException != null)
			{
				this.HandleOneWayException(new Exception(Res.GetString("WebConfigExtensionError"), this.protocol.OnewayInitException), "OneWayAsyncInvoke");
				return;
			}
			HttpContext httpContext = null;
			if (this.protocol.Context != null)
			{
				httpContext = this.SwitchContext(this.protocol.Context);
			}
			try
			{
				this.BeginInvoke(new AsyncCallback(this.OneWayCallback), null);
			}
			catch (Exception ex)
			{
				this.HandleOneWayException(ex, "OneWayAsyncInvoke");
			}
			finally
			{
				if (httpContext != null)
				{
					this.SwitchContext(httpContext);
				}
			}
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x000126E4 File Offset: 0x000116E4
		private IAsyncResult BeginInvoke(AsyncCallback callback, object asyncState)
		{
			IAsyncResult asyncResult;
			try
			{
				this.PrepareContext();
				this.protocol.CreateServerInstance();
				this.asyncCallback = callback;
				TraceMethod traceMethod = (Tracing.On ? new TraceMethod(this, "BeginInvoke", new object[0]) : null);
				TraceMethod traceMethod2 = (Tracing.On ? new TraceMethod(this.protocol.Target, this.protocol.MethodInfo.Name, this.parameters) : null);
				if (Tracing.On)
				{
					Tracing.Enter(this.protocol.MethodInfo.ToString(), traceMethod, traceMethod2);
				}
				asyncResult = this.protocol.MethodInfo.BeginInvoke(this.protocol.Target, this.parameters, new AsyncCallback(this.Callback), asyncState);
				if (Tracing.On)
				{
					Tracing.Enter(this.protocol.MethodInfo.ToString(), traceMethod);
				}
				if (asyncResult == null)
				{
					throw new InvalidOperationException(Res.GetString("WebNullAsyncResultInBegin"));
				}
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Error, this, "BeginInvoke", ex);
				}
				this.exception = ex;
				asyncResult = new CompletedAsyncResult(asyncState, true);
				this.asyncCallback = callback;
				this.DoCallback(asyncResult);
			}
			catch
			{
				this.exception = new Exception(Res.GetString("NonClsCompliantException"));
				asyncResult = new CompletedAsyncResult(asyncState, true);
				this.asyncCallback = callback;
				this.DoCallback(asyncResult);
			}
			this.asyncBeginComplete.Set();
			WebServiceHandler.TraceFlush();
			return asyncResult;
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00012880 File Offset: 0x00011880
		private void OneWayCallback(IAsyncResult asyncResult)
		{
			this.EndInvoke(asyncResult);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00012889 File Offset: 0x00011889
		protected void EndCoreProcessRequest(IAsyncResult asyncResult)
		{
			if (asyncResult == null)
			{
				return;
			}
			if (this.protocol.IsOneWay)
			{
				this.protocol.WriteOneWayResponse();
				return;
			}
			this.EndInvoke(asyncResult);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000128B0 File Offset: 0x000118B0
		private void EndInvoke(IAsyncResult asyncResult)
		{
			try
			{
				if (this.exception != null)
				{
					throw this.exception;
				}
				object[] array = this.protocol.MethodInfo.EndInvoke(this.protocol.Target, asyncResult);
				this.WriteReturns(array);
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (Tracing.On)
				{
					Tracing.ExceptionCatch(TraceEventType.Error, this, "EndInvoke", ex);
				}
				if (!this.protocol.IsOneWay)
				{
					this.WriteException(ex);
				}
			}
			catch
			{
				if (!this.protocol.IsOneWay)
				{
					this.WriteException(new Exception(Res.GetString("NonClsCompliantException")));
				}
			}
			finally
			{
				this.protocol.DisposeServerInstance();
			}
			WebServiceHandler.TraceFlush();
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00012998 File Offset: 0x00011998
		private void WriteReturns(object[] returnValues)
		{
			if (this.protocol.IsOneWay)
			{
				return;
			}
			bool bufferResponse = this.protocol.MethodAttribute.BufferResponse;
			Stream stream = this.protocol.Response.OutputStream;
			if (!bufferResponse)
			{
				stream = new BufferedResponseStream(stream, 16384);
				((BufferedResponseStream)stream).FlushEnabled = false;
			}
			this.protocol.WriteReturns(returnValues, stream);
			if (!bufferResponse)
			{
				((BufferedResponseStream)stream).FlushEnabled = true;
				stream.Flush();
			}
		}

		// Token: 0x0400038C RID: 908
		private ServerProtocol protocol;

		// Token: 0x0400038D RID: 909
		private Exception exception;

		// Token: 0x0400038E RID: 910
		private AsyncCallback asyncCallback;

		// Token: 0x0400038F RID: 911
		private ManualResetEvent asyncBeginComplete;

		// Token: 0x04000390 RID: 912
		private int asyncCallbackCalls;

		// Token: 0x04000391 RID: 913
		private bool wroteException;

		// Token: 0x04000392 RID: 914
		private object[] parameters;
	}
}
