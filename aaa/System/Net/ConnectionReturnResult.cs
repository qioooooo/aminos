using System;
using System.Collections.Generic;
using System.Threading;

namespace System.Net
{
	// Token: 0x020004C8 RID: 1224
	internal class ConnectionReturnResult
	{
		// Token: 0x060025B8 RID: 9656 RVA: 0x00096326 File Offset: 0x00095326
		internal ConnectionReturnResult()
		{
			this.m_Context = new List<ConnectionReturnResult.RequestContext>(5);
		}

		// Token: 0x060025B9 RID: 9657 RVA: 0x0009633A File Offset: 0x0009533A
		internal ConnectionReturnResult(int capacity)
		{
			this.m_Context = new List<ConnectionReturnResult.RequestContext>(capacity);
		}

		// Token: 0x170007D2 RID: 2002
		// (get) Token: 0x060025BA RID: 9658 RVA: 0x0009634E File Offset: 0x0009534E
		internal bool IsNotEmpty
		{
			get
			{
				return this.m_Context.Count != 0;
			}
		}

		// Token: 0x060025BB RID: 9659 RVA: 0x00096361 File Offset: 0x00095361
		internal static void Add(ref ConnectionReturnResult returnResult, HttpWebRequest request, CoreResponseData coreResponseData)
		{
			if (coreResponseData == null)
			{
				throw new InternalException();
			}
			if (returnResult == null)
			{
				returnResult = new ConnectionReturnResult();
			}
			returnResult.m_Context.Add(new ConnectionReturnResult.RequestContext(request, coreResponseData));
		}

		// Token: 0x060025BC RID: 9660 RVA: 0x0009638A File Offset: 0x0009538A
		internal static void AddExceptionRange(ref ConnectionReturnResult returnResult, HttpWebRequest[] requests, Exception exception)
		{
			ConnectionReturnResult.AddExceptionRange(ref returnResult, requests, exception, exception);
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x00096398 File Offset: 0x00095398
		internal static void AddExceptionRange(ref ConnectionReturnResult returnResult, HttpWebRequest[] requests, Exception exception, Exception firstRequestException)
		{
			if (exception == null)
			{
				throw new InternalException();
			}
			if (returnResult == null)
			{
				returnResult = new ConnectionReturnResult(requests.Length);
			}
			for (int i = 0; i < requests.Length; i++)
			{
				if (i == 0)
				{
					returnResult.m_Context.Add(new ConnectionReturnResult.RequestContext(requests[i], firstRequestException));
				}
				else
				{
					returnResult.m_Context.Add(new ConnectionReturnResult.RequestContext(requests[i], exception));
				}
			}
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x000963FC File Offset: 0x000953FC
		internal static void SetResponses(ConnectionReturnResult returnResult)
		{
			if (returnResult == null)
			{
				return;
			}
			for (int i = 0; i < returnResult.m_Context.Count; i++)
			{
				try
				{
					HttpWebRequest request = returnResult.m_Context[i].Request;
					request.SetAndOrProcessResponse(returnResult.m_Context[i].CoreResponse);
				}
				catch (Exception)
				{
					returnResult.m_Context.RemoveRange(0, i + 1);
					if (returnResult.m_Context.Count > 0)
					{
						ThreadPool.UnsafeQueueUserWorkItem(ConnectionReturnResult.s_InvokeConnectionCallback, returnResult);
					}
					throw;
				}
			}
			returnResult.m_Context.Clear();
		}

		// Token: 0x060025BF RID: 9663 RVA: 0x00096498 File Offset: 0x00095498
		private static void InvokeConnectionCallback(object objectReturnResult)
		{
			ConnectionReturnResult connectionReturnResult = (ConnectionReturnResult)objectReturnResult;
			ConnectionReturnResult.SetResponses(connectionReturnResult);
		}

		// Token: 0x04002576 RID: 9590
		private static readonly WaitCallback s_InvokeConnectionCallback = new WaitCallback(ConnectionReturnResult.InvokeConnectionCallback);

		// Token: 0x04002577 RID: 9591
		private List<ConnectionReturnResult.RequestContext> m_Context;

		// Token: 0x020004C9 RID: 1225
		private struct RequestContext
		{
			// Token: 0x060025C1 RID: 9665 RVA: 0x000964C5 File Offset: 0x000954C5
			internal RequestContext(HttpWebRequest request, object coreResponse)
			{
				this.Request = request;
				this.CoreResponse = coreResponse;
			}

			// Token: 0x04002578 RID: 9592
			internal HttpWebRequest Request;

			// Token: 0x04002579 RID: 9593
			internal object CoreResponse;
		}
	}
}
