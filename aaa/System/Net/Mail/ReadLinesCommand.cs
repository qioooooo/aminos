using System;
using System.Net.Mime;

namespace System.Net.Mail
{
	// Token: 0x020006BD RID: 1725
	internal static class ReadLinesCommand
	{
		// Token: 0x06003555 RID: 13653 RVA: 0x000E3210 File Offset: 0x000E2210
		internal static IAsyncResult BeginSend(SmtpConnection conn, AsyncCallback callback, object state)
		{
			MultiAsyncResult multiAsyncResult = new MultiAsyncResult(conn, callback, state);
			multiAsyncResult.Enter();
			IAsyncResult asyncResult = conn.BeginFlush(ReadLinesCommand.onWrite, multiAsyncResult);
			if (asyncResult.CompletedSynchronously)
			{
				conn.EndFlush(asyncResult);
				multiAsyncResult.Leave();
			}
			SmtpReplyReader nextReplyReader = conn.Reader.GetNextReplyReader();
			multiAsyncResult.Enter();
			IAsyncResult asyncResult2 = nextReplyReader.BeginReadLines(ReadLinesCommand.onReadLines, multiAsyncResult);
			if (asyncResult2.CompletedSynchronously)
			{
				LineInfo[] array = conn.Reader.CurrentReader.EndReadLines(asyncResult2);
				if (!(multiAsyncResult.Result is Exception))
				{
					multiAsyncResult.Result = array;
				}
				multiAsyncResult.Leave();
			}
			multiAsyncResult.CompleteSequence();
			return multiAsyncResult;
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x000E32AC File Offset: 0x000E22AC
		internal static LineInfo[] EndSend(IAsyncResult result)
		{
			object obj = MultiAsyncResult.End(result);
			if (obj is Exception)
			{
				throw (Exception)obj;
			}
			return (LineInfo[])obj;
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x000E32D8 File Offset: 0x000E22D8
		private static void OnReadLines(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				try
				{
					SmtpConnection smtpConnection = (SmtpConnection)multiAsyncResult.Context;
					LineInfo[] array = smtpConnection.Reader.CurrentReader.EndReadLines(result);
					if (!(multiAsyncResult.Result is Exception))
					{
						multiAsyncResult.Result = array;
					}
					multiAsyncResult.Leave();
				}
				catch (Exception ex)
				{
					multiAsyncResult.Leave(ex);
				}
				catch
				{
					multiAsyncResult.Leave(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003558 RID: 13656 RVA: 0x000E3374 File Offset: 0x000E2374
		private static void OnWrite(IAsyncResult result)
		{
			if (!result.CompletedSynchronously)
			{
				MultiAsyncResult multiAsyncResult = (MultiAsyncResult)result.AsyncState;
				try
				{
					SmtpConnection smtpConnection = (SmtpConnection)multiAsyncResult.Context;
					smtpConnection.EndFlush(result);
					multiAsyncResult.Leave();
				}
				catch (Exception ex)
				{
					multiAsyncResult.Leave(ex);
				}
				catch
				{
					multiAsyncResult.Leave(new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
		}

		// Token: 0x06003559 RID: 13657 RVA: 0x000E33F0 File Offset: 0x000E23F0
		internal static LineInfo[] Send(SmtpConnection conn)
		{
			conn.Flush();
			return conn.Reader.GetNextReplyReader().ReadLines();
		}

		// Token: 0x040030C9 RID: 12489
		private static AsyncCallback onReadLines = new AsyncCallback(ReadLinesCommand.OnReadLines);

		// Token: 0x040030CA RID: 12490
		private static AsyncCallback onWrite = new AsyncCallback(ReadLinesCommand.OnWrite);
	}
}
