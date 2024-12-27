using System;
using System.Collections;
using System.Configuration;
using System.DirectoryServices.Interop;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;

namespace System.DirectoryServices
{
	// Token: 0x0200003E RID: 62
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class SearchResultCollection : MarshalByRefObject, ICollection, IEnumerable, IDisposable
	{
		// Token: 0x060001BC RID: 444 RVA: 0x00007340 File Offset: 0x00006340
		internal SearchResultCollection(DirectoryEntry root, IntPtr searchHandle, string[] propertiesLoaded, DirectorySearcher srch)
		{
			this.handle = searchHandle;
			this.properties = propertiesLoaded;
			this.filter = srch.Filter;
			this.rootEntry = root;
			this.srch = srch;
		}

		// Token: 0x17000075 RID: 117
		public SearchResult this[int index]
		{
			get
			{
				return (SearchResult)this.InnerList[index];
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060001BE RID: 446 RVA: 0x000073B0 File Offset: 0x000063B0
		public int Count
		{
			get
			{
				return this.InnerList.Count;
			}
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060001BF RID: 447 RVA: 0x000073BD File Offset: 0x000063BD
		internal string Filter
		{
			get
			{
				return this.filter;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060001C0 RID: 448 RVA: 0x000073C8 File Offset: 0x000063C8
		private ArrayList InnerList
		{
			get
			{
				if (this.innerList == null)
				{
					this.innerList = new ArrayList();
					IEnumerator enumerator = new SearchResultCollection.ResultsEnumerator(this, this.rootEntry.GetUsername(), this.rootEntry.GetPassword(), this.rootEntry.AuthenticationType);
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						this.innerList.Add(obj);
					}
				}
				return this.innerList;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060001C1 RID: 449 RVA: 0x00007432 File Offset: 0x00006432
		internal UnsafeNativeMethods.IDirectorySearch SearchObject
		{
			get
			{
				if (this.searchObject == null)
				{
					this.searchObject = (UnsafeNativeMethods.IDirectorySearch)this.rootEntry.AdsObject;
				}
				return this.searchObject;
			}
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060001C2 RID: 450 RVA: 0x00007458 File Offset: 0x00006458
		public IntPtr Handle
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.handle;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060001C3 RID: 451 RVA: 0x00007479 File Offset: 0x00006479
		public string[] PropertiesLoaded
		{
			get
			{
				return this.properties;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060001C4 RID: 452 RVA: 0x00007481 File Offset: 0x00006481
		internal byte[] DirsyncCookie
		{
			get
			{
				return this.RetrieveDirectorySynchronizationCookie();
			}
		}

		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x00007489 File Offset: 0x00006489
		internal DirectoryVirtualListView VLVResponse
		{
			get
			{
				return this.RetrieveVLVResponse();
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x00007494 File Offset: 0x00006494
		internal unsafe byte[] RetrieveDirectorySynchronizationCookie()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			AdsSearchColumn adsSearchColumn = default(AdsSearchColumn);
			AdsSearchColumn* ptr = &adsSearchColumn;
			this.SearchObject.GetColumn(this.Handle, this.AdsDirsynCookieName, (IntPtr)((void*)ptr));
			byte[] array2;
			try
			{
				AdsValue* pADsValues = adsSearchColumn.pADsValues;
				byte[] array = (byte[])new AdsValueHelper(*pADsValues).GetValue();
				array2 = array;
			}
			finally
			{
				try
				{
					this.SearchObject.FreeColumn((IntPtr)((void*)ptr));
				}
				catch (COMException)
				{
				}
			}
			return array2;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000753C File Offset: 0x0000653C
		internal unsafe DirectoryVirtualListView RetrieveVLVResponse()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			AdsSearchColumn adsSearchColumn = default(AdsSearchColumn);
			AdsSearchColumn* ptr = &adsSearchColumn;
			this.SearchObject.GetColumn(this.Handle, this.AdsVLVResponseName, (IntPtr)((void*)ptr));
			DirectoryVirtualListView directoryVirtualListView2;
			try
			{
				AdsValue* pADsValues = adsSearchColumn.pADsValues;
				DirectoryVirtualListView directoryVirtualListView = (DirectoryVirtualListView)new AdsValueHelper(*pADsValues).GetVlvValue();
				directoryVirtualListView2 = directoryVirtualListView;
			}
			finally
			{
				try
				{
					this.SearchObject.FreeColumn((IntPtr)((void*)ptr));
				}
				catch (COMException)
				{
				}
			}
			return directoryVirtualListView2;
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x000075E4 File Offset: 0x000065E4
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x000075F4 File Offset: 0x000065F4
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (this.handle != (IntPtr)0 && this.searchObject != null && disposing)
				{
					this.searchObject.CloseSearchHandle(this.handle);
					this.handle = (IntPtr)0;
				}
				if (disposing)
				{
					this.rootEntry.Dispose();
				}
				if (this.AdsDirsynCookieName != (IntPtr)0)
				{
					Marshal.FreeCoTaskMem(this.AdsDirsynCookieName);
				}
				if (this.AdsVLVResponseName != (IntPtr)0)
				{
					Marshal.FreeCoTaskMem(this.AdsVLVResponseName);
				}
				this.disposed = true;
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x00007698 File Offset: 0x00006698
		~SearchResultCollection()
		{
			this.Dispose(false);
		}

		// Token: 0x060001CB RID: 459 RVA: 0x000076C8 File Offset: 0x000066C8
		public IEnumerator GetEnumerator()
		{
			return new SearchResultCollection.ResultsEnumerator(this, this.rootEntry.GetUsername(), this.rootEntry.GetPassword(), this.rootEntry.AuthenticationType);
		}

		// Token: 0x060001CC RID: 460 RVA: 0x000076F1 File Offset: 0x000066F1
		public bool Contains(SearchResult result)
		{
			return this.InnerList.Contains(result);
		}

		// Token: 0x060001CD RID: 461 RVA: 0x000076FF File Offset: 0x000066FF
		public void CopyTo(SearchResult[] results, int index)
		{
			this.InnerList.CopyTo(results, index);
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000770E File Offset: 0x0000670E
		public int IndexOf(SearchResult result)
		{
			return this.InnerList.IndexOf(result);
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060001CF RID: 463 RVA: 0x0000771C File Offset: 0x0000671C
		bool ICollection.IsSynchronized
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060001D0 RID: 464 RVA: 0x0000771F File Offset: 0x0000671F
		object ICollection.SyncRoot
		{
			get
			{
				return this;
			}
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x00007722 File Offset: 0x00006722
		void ICollection.CopyTo(Array array, int index)
		{
			this.InnerList.CopyTo(array, index);
		}

		// Token: 0x040001E0 RID: 480
		private const string ADS_DIRSYNC_COOKIE = "fc8cb04d-311d-406c-8cb9-1ae8b843b418";

		// Token: 0x040001E1 RID: 481
		private const string ADS_VLV_RESPONSE = "fc8cb04d-311d-406c-8cb9-1ae8b843b419";

		// Token: 0x040001E2 RID: 482
		private IntPtr handle;

		// Token: 0x040001E3 RID: 483
		private string[] properties;

		// Token: 0x040001E4 RID: 484
		private UnsafeNativeMethods.IDirectorySearch searchObject;

		// Token: 0x040001E5 RID: 485
		private string filter;

		// Token: 0x040001E6 RID: 486
		private ArrayList innerList;

		// Token: 0x040001E7 RID: 487
		private bool disposed;

		// Token: 0x040001E8 RID: 488
		private DirectoryEntry rootEntry;

		// Token: 0x040001E9 RID: 489
		private IntPtr AdsDirsynCookieName = Marshal.StringToCoTaskMemUni("fc8cb04d-311d-406c-8cb9-1ae8b843b418");

		// Token: 0x040001EA RID: 490
		private IntPtr AdsVLVResponseName = Marshal.StringToCoTaskMemUni("fc8cb04d-311d-406c-8cb9-1ae8b843b419");

		// Token: 0x040001EB RID: 491
		internal DirectorySearcher srch;

		// Token: 0x0200003F RID: 63
		private class ResultsEnumerator : IEnumerator
		{
			// Token: 0x060001D2 RID: 466 RVA: 0x00007734 File Offset: 0x00006734
			internal ResultsEnumerator(SearchResultCollection results, string parentUserName, string parentPassword, AuthenticationTypes parentAuthenticationType)
			{
				if (parentUserName != null && parentPassword != null)
				{
					this.parentCredentials = new NetworkCredential(parentUserName, parentPassword);
				}
				this.parentAuthenticationType = parentAuthenticationType;
				this.results = results;
				this.initialized = false;
				object section = PrivilegedConfigurationManager.GetSection("system.directoryservices");
				if (section != null && section is bool)
				{
					this.waitForResult = (bool)section;
				}
			}

			// Token: 0x17000080 RID: 128
			// (get) Token: 0x060001D3 RID: 467 RVA: 0x00007792 File Offset: 0x00006792
			public SearchResult Current
			{
				get
				{
					if (!this.initialized || this.eof)
					{
						throw new InvalidOperationException(Res.GetString("DSNoCurrentEntry"));
					}
					if (this.currentResult == null)
					{
						this.currentResult = this.GetCurrentResult();
					}
					return this.currentResult;
				}
			}

			// Token: 0x060001D4 RID: 468 RVA: 0x000077D0 File Offset: 0x000067D0
			private unsafe SearchResult GetCurrentResult()
			{
				SearchResult searchResult = new SearchResult(this.parentCredentials, this.parentAuthenticationType);
				IntPtr intPtr = (IntPtr)0;
				for (int num = this.results.SearchObject.GetNextColumnName(this.results.Handle, (IntPtr)((void*)(&intPtr))); num == 0; num = this.results.SearchObject.GetNextColumnName(this.results.Handle, (IntPtr)((void*)(&intPtr))))
				{
					try
					{
						AdsSearchColumn adsSearchColumn = default(AdsSearchColumn);
						AdsSearchColumn* ptr = &adsSearchColumn;
						this.results.SearchObject.GetColumn(this.results.Handle, intPtr, (IntPtr)((void*)ptr));
						try
						{
							int dwNumValues = adsSearchColumn.dwNumValues;
							AdsValue* ptr2 = adsSearchColumn.pADsValues;
							object[] array = new object[dwNumValues];
							for (int i = 0; i < dwNumValues; i++)
							{
								array[i] = new AdsValueHelper(*ptr2).GetValue();
								ptr2++;
							}
							searchResult.Properties.Add(Marshal.PtrToStringUni(intPtr), new ResultPropertyValueCollection(array));
						}
						finally
						{
							try
							{
								this.results.SearchObject.FreeColumn((IntPtr)((void*)ptr));
							}
							catch (COMException)
							{
							}
						}
					}
					finally
					{
						SafeNativeMethods.FreeADsMem(intPtr);
					}
				}
				return searchResult;
			}

			// Token: 0x060001D5 RID: 469 RVA: 0x00007934 File Offset: 0x00006934
			public bool MoveNext()
			{
				int num = 0;
				if (this.eof)
				{
					return false;
				}
				this.currentResult = null;
				if (!this.initialized)
				{
					int firstRow = this.results.SearchObject.GetFirstRow(this.results.Handle);
					if (firstRow != 20498)
					{
						if (firstRow == -2147016642)
						{
							throw new ArgumentException(Res.GetString("DSInvalidSearchFilter", new object[] { this.results.Filter }));
						}
						if (firstRow != 0)
						{
							throw COMExceptionHelper.CreateFormattedComException(firstRow);
						}
						this.eof = false;
						this.initialized = true;
						return true;
					}
					else
					{
						this.initialized = true;
					}
				}
				int num2;
				for (;;)
				{
					this.CleanLastError();
					num = 0;
					num2 = this.results.SearchObject.GetNextRow(this.results.Handle);
					if (num2 != 20498 && num2 != -2147016669)
					{
						goto IL_017A;
					}
					if (num2 == 20498)
					{
						num2 = this.GetLastError(ref num);
						if (num2 != 0)
						{
							break;
						}
					}
					if (num != 234)
					{
						goto Block_9;
					}
					if (!this.waitForResult)
					{
						goto Block_12;
					}
				}
				throw COMExceptionHelper.CreateFormattedComException(num2);
				Block_9:
				if (this.results.srch.directorySynchronizationSpecified)
				{
					DirectorySynchronization directorySynchronization = this.results.srch.DirectorySynchronization;
				}
				if (this.results.srch.directoryVirtualListViewSpecified)
				{
					DirectoryVirtualListView virtualListView = this.results.srch.VirtualListView;
				}
				this.results.srch.searchResult = null;
				this.eof = true;
				this.initialized = false;
				return false;
				Block_12:
				uint num3 = (uint)num;
				num3 = (num3 & 65535U) | 458752U | 2147483648U;
				throw COMExceptionHelper.CreateFormattedComException((int)num3);
				IL_017A:
				if (num2 == -2147016642)
				{
					throw new ArgumentException(Res.GetString("DSInvalidSearchFilter", new object[] { this.results.Filter }));
				}
				if (num2 != 0)
				{
					throw COMExceptionHelper.CreateFormattedComException(num2);
				}
				this.eof = false;
				return true;
			}

			// Token: 0x060001D6 RID: 470 RVA: 0x00007AFE File Offset: 0x00006AFE
			public void Reset()
			{
				this.eof = false;
				this.initialized = false;
			}

			// Token: 0x17000081 RID: 129
			// (get) Token: 0x060001D7 RID: 471 RVA: 0x00007B0E File Offset: 0x00006B0E
			object IEnumerator.Current
			{
				get
				{
					return this.Current;
				}
			}

			// Token: 0x060001D8 RID: 472 RVA: 0x00007B16 File Offset: 0x00006B16
			private void CleanLastError()
			{
				SafeNativeMethods.ADsSetLastError(0, null, null);
			}

			// Token: 0x060001D9 RID: 473 RVA: 0x00007B24 File Offset: 0x00006B24
			private int GetLastError(ref int errorCode)
			{
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				errorCode = 0;
				return SafeNativeMethods.ADsGetLastError(out errorCode, stringBuilder, 0, stringBuilder2, 0);
			}

			// Token: 0x040001EC RID: 492
			private NetworkCredential parentCredentials;

			// Token: 0x040001ED RID: 493
			private AuthenticationTypes parentAuthenticationType;

			// Token: 0x040001EE RID: 494
			private SearchResultCollection results;

			// Token: 0x040001EF RID: 495
			private bool initialized;

			// Token: 0x040001F0 RID: 496
			private SearchResult currentResult;

			// Token: 0x040001F1 RID: 497
			private bool eof;

			// Token: 0x040001F2 RID: 498
			private bool waitForResult;
		}
	}
}
