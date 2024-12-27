using System;
using System.Collections;
using System.Threading;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200007B RID: 123
	internal class LdapPartialResultsProcessor
	{
		// Token: 0x06000297 RID: 663 RVA: 0x0000D705 File Offset: 0x0000C705
		internal LdapPartialResultsProcessor(ManualResetEvent eventHandle)
		{
			this.resultList = new ArrayList();
			this.workThreadWaitHandle = eventHandle;
		}

		// Token: 0x06000298 RID: 664 RVA: 0x0000D720 File Offset: 0x0000C720
		public void Add(LdapPartialAsyncResult asyncResult)
		{
			lock (this)
			{
				this.resultList.Add(asyncResult);
				if (!this.workToDo)
				{
					this.workThreadWaitHandle.Set();
					this.workToDo = true;
				}
			}
		}

		// Token: 0x06000299 RID: 665 RVA: 0x0000D778 File Offset: 0x0000C778
		public void Remove(LdapPartialAsyncResult asyncResult)
		{
			lock (this)
			{
				if (!this.resultList.Contains(asyncResult))
				{
					throw new ArgumentException(Res.GetString("InvalidAsyncResult"));
				}
				this.resultList.Remove(asyncResult);
			}
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0000D7D0 File Offset: 0x0000C7D0
		public void RetrievingSearchResults()
		{
			int num = 0;
			LdapPartialAsyncResult ldapPartialAsyncResult = null;
			AsyncCallback asyncCallback = null;
			lock (this)
			{
				int count = this.resultList.Count;
				if (count == 0)
				{
					this.workThreadWaitHandle.Reset();
					this.workToDo = false;
					return;
				}
				do
				{
					if (this.currentIndex >= count)
					{
						this.currentIndex = 0;
					}
					ldapPartialAsyncResult = (LdapPartialAsyncResult)this.resultList[this.currentIndex];
					num++;
					this.currentIndex++;
					if (ldapPartialAsyncResult.resultStatus != ResultsStatus.Done)
					{
						goto IL_0096;
					}
				}
				while (num < count);
				this.workToDo = false;
				this.workThreadWaitHandle.Reset();
				return;
				IL_0096:
				this.GetResultsHelper(ldapPartialAsyncResult);
				if (ldapPartialAsyncResult.resultStatus == ResultsStatus.Done)
				{
					ldapPartialAsyncResult.manualResetEvent.Set();
					ldapPartialAsyncResult.completed = true;
					if (ldapPartialAsyncResult.callback != null)
					{
						asyncCallback = ldapPartialAsyncResult.callback;
					}
				}
				else if (ldapPartialAsyncResult.callback != null && ldapPartialAsyncResult.partialCallback && ldapPartialAsyncResult.response != null && (ldapPartialAsyncResult.response.Entries.Count > 0 || ldapPartialAsyncResult.response.References.Count > 0))
				{
					asyncCallback = ldapPartialAsyncResult.callback;
				}
			}
			if (asyncCallback != null)
			{
				asyncCallback(ldapPartialAsyncResult);
			}
		}

		// Token: 0x0600029B RID: 667 RVA: 0x0000D91C File Offset: 0x0000C91C
		private void GetResultsHelper(LdapPartialAsyncResult asyncResult)
		{
			LdapConnection con = asyncResult.con;
			IntPtr ldapHandle = con.ldapHandle;
			(IntPtr)0;
			(IntPtr)0;
			ResultAll resultAll = ResultAll.LDAP_MSG_RECEIVED;
			if (asyncResult.resultStatus == ResultsStatus.CompleteResult)
			{
				resultAll = ResultAll.LDAP_MSG_POLLINGALL;
			}
			try
			{
				SearchResponse searchResponse = (SearchResponse)con.ConstructResponse(asyncResult.messageID, LdapOperation.LdapSearch, resultAll, asyncResult.requestTimeout, false);
				if (searchResponse == null)
				{
					if (asyncResult.startTime.Ticks + asyncResult.requestTimeout.Ticks <= DateTime.Now.Ticks)
					{
						throw new LdapException(85, LdapErrorMappings.MapResultCode(85));
					}
				}
				else
				{
					if (asyncResult.response != null)
					{
						this.AddResult(asyncResult.response, searchResponse);
					}
					else
					{
						asyncResult.response = searchResponse;
					}
					if (searchResponse.searchDone)
					{
						asyncResult.resultStatus = ResultsStatus.Done;
					}
				}
			}
			catch (Exception ex)
			{
				if (ex is DirectoryOperationException)
				{
					SearchResponse searchResponse2 = (SearchResponse)((DirectoryOperationException)ex).Response;
					if (asyncResult.response != null)
					{
						this.AddResult(asyncResult.response, searchResponse2);
					}
					else
					{
						asyncResult.response = searchResponse2;
					}
					((DirectoryOperationException)ex).response = asyncResult.response;
				}
				else if (ex is LdapException)
				{
					LdapException ex2 = (LdapException)ex;
					int errorCode = ex2.ErrorCode;
					if (asyncResult.response != null)
					{
						if (asyncResult.response.Entries != null)
						{
							for (int i = 0; i < asyncResult.response.Entries.Count; i++)
							{
								ex2.results.Add(asyncResult.response.Entries[i]);
							}
						}
						if (asyncResult.response.References != null)
						{
							for (int j = 0; j < asyncResult.response.References.Count; j++)
							{
								ex2.results.Add(asyncResult.response.References[j]);
							}
						}
					}
				}
				asyncResult.exception = ex;
				asyncResult.resultStatus = ResultsStatus.Done;
				Wldap32.ldap_abandon(con.ldapHandle, asyncResult.messageID);
			}
			catch
			{
				asyncResult.exception = new Exception(Res.GetString("NonCLSException"));
				asyncResult.resultStatus = ResultsStatus.Done;
				Wldap32.ldap_abandon(con.ldapHandle, asyncResult.messageID);
			}
		}

		// Token: 0x0600029C RID: 668 RVA: 0x0000DB74 File Offset: 0x0000CB74
		public void NeedCompleteResult(LdapPartialAsyncResult asyncResult)
		{
			lock (this)
			{
				if (!this.resultList.Contains(asyncResult))
				{
					throw new ArgumentException(Res.GetString("InvalidAsyncResult"));
				}
				if (asyncResult.resultStatus == ResultsStatus.PartialResult)
				{
					asyncResult.resultStatus = ResultsStatus.CompleteResult;
				}
			}
		}

		// Token: 0x0600029D RID: 669 RVA: 0x0000DBD4 File Offset: 0x0000CBD4
		public PartialResultsCollection GetPartialResults(LdapPartialAsyncResult asyncResult)
		{
			PartialResultsCollection partialResultsCollection2;
			lock (this)
			{
				if (!this.resultList.Contains(asyncResult))
				{
					throw new ArgumentException(Res.GetString("InvalidAsyncResult"));
				}
				if (asyncResult.exception != null)
				{
					this.resultList.Remove(asyncResult);
					throw asyncResult.exception;
				}
				PartialResultsCollection partialResultsCollection = new PartialResultsCollection();
				if (asyncResult.response != null)
				{
					if (asyncResult.response.Entries != null)
					{
						for (int i = 0; i < asyncResult.response.Entries.Count; i++)
						{
							partialResultsCollection.Add(asyncResult.response.Entries[i]);
						}
						asyncResult.response.Entries.Clear();
					}
					if (asyncResult.response.References != null)
					{
						for (int j = 0; j < asyncResult.response.References.Count; j++)
						{
							partialResultsCollection.Add(asyncResult.response.References[j]);
						}
						asyncResult.response.References.Clear();
					}
				}
				partialResultsCollection2 = partialResultsCollection;
			}
			return partialResultsCollection2;
		}

		// Token: 0x0600029E RID: 670 RVA: 0x0000DCF4 File Offset: 0x0000CCF4
		public DirectoryResponse GetCompleteResult(LdapPartialAsyncResult asyncResult)
		{
			DirectoryResponse response;
			lock (this)
			{
				if (!this.resultList.Contains(asyncResult))
				{
					throw new ArgumentException(Res.GetString("InvalidAsyncResult"));
				}
				this.resultList.Remove(asyncResult);
				if (asyncResult.exception != null)
				{
					throw asyncResult.exception;
				}
				response = asyncResult.response;
			}
			return response;
		}

		// Token: 0x0600029F RID: 671 RVA: 0x0000DD64 File Offset: 0x0000CD64
		private void AddResult(SearchResponse partialResults, SearchResponse newResult)
		{
			if (newResult == null)
			{
				return;
			}
			if (newResult.Entries != null)
			{
				for (int i = 0; i < newResult.Entries.Count; i++)
				{
					partialResults.Entries.Add(newResult.Entries[i]);
				}
			}
			if (newResult.References != null)
			{
				for (int j = 0; j < newResult.References.Count; j++)
				{
					partialResults.References.Add(newResult.References[j]);
				}
			}
		}

		// Token: 0x0400026F RID: 623
		private ArrayList resultList;

		// Token: 0x04000270 RID: 624
		private ManualResetEvent workThreadWaitHandle;

		// Token: 0x04000271 RID: 625
		private bool workToDo;

		// Token: 0x04000272 RID: 626
		private int currentIndex;
	}
}
