using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Xml;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000073 RID: 115
	public class LdapConnection : DirectoryConnection, IDisposable
	{
		// Token: 0x0600024B RID: 587 RVA: 0x0000A744 File Offset: 0x00009744
		static LdapConnection()
		{
			Hashtable hashtable = new Hashtable();
			LdapConnection.asyncResultTable = Hashtable.Synchronized(hashtable);
			LdapConnection.waitHandle = new ManualResetEvent(false);
			LdapConnection.partialResultsProcessor = new LdapPartialResultsProcessor(LdapConnection.waitHandle);
			LdapConnection.retriever = new PartialResultsRetriever(LdapConnection.waitHandle, LdapConnection.partialResultsProcessor);
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000A7A4 File Offset: 0x000097A4
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public LdapConnection(string server)
			: this(new LdapDirectoryIdentifier(server))
		{
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000A7B2 File Offset: 0x000097B2
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public LdapConnection(LdapDirectoryIdentifier identifier)
			: this(identifier, null, AuthType.Negotiate)
		{
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0000A7BD File Offset: 0x000097BD
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public LdapConnection(LdapDirectoryIdentifier identifier, NetworkCredential credential)
			: this(identifier, credential, AuthType.Negotiate)
		{
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0000A7C8 File Offset: 0x000097C8
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		public LdapConnection(LdapDirectoryIdentifier identifier, NetworkCredential credential, AuthType authType)
		{
			this.connectionAuthType = AuthType.Negotiate;
			this.ldapHandle = (IntPtr)0;
			this.automaticBind = true;
			this.needDispose = true;
			base..ctor();
			this.fd = new GetLdapResponseCallback(this.ConstructResponse);
			this.directoryIdentifier = identifier;
			this.directoryCredential = ((credential != null) ? new NetworkCredential(credential.UserName, credential.Password, credential.Domain) : null);
			this.connectionAuthType = authType;
			if (authType < AuthType.Anonymous || authType > AuthType.Kerberos)
			{
				throw new InvalidEnumArgumentException("authType", (int)authType, typeof(AuthType));
			}
			if (this.AuthType == AuthType.Anonymous && this.directoryCredential != null && ((this.directoryCredential.Password != null && this.directoryCredential.Password.Length != 0) || (this.directoryCredential.UserName != null && this.directoryCredential.UserName.Length != 0)))
			{
				throw new ArgumentException(Res.GetString("InvalidAuthCredential"));
			}
			this.Init();
			this.options = new LdapSessionOptions(this);
			this.clientCertificateRoutine = new QUERYCLIENTCERT(this.ProcessClientCertificate);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0000A8E0 File Offset: 0x000098E0
		internal LdapConnection(LdapDirectoryIdentifier identifier, NetworkCredential credential, AuthType authType, IntPtr handle)
		{
			this.connectionAuthType = AuthType.Negotiate;
			this.ldapHandle = (IntPtr)0;
			this.automaticBind = true;
			this.needDispose = true;
			base..ctor();
			this.directoryIdentifier = identifier;
			this.ldapHandle = handle;
			this.directoryCredential = credential;
			this.connectionAuthType = authType;
			this.options = new LdapSessionOptions(this);
			this.needDispose = false;
			this.clientCertificateRoutine = new QUERYCLIENTCERT(this.ProcessClientCertificate);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0000A958 File Offset: 0x00009958
		~LdapConnection()
		{
			this.Dispose(false);
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000252 RID: 594 RVA: 0x0000A988 File Offset: 0x00009988
		// (set) Token: 0x06000253 RID: 595 RVA: 0x0000A990 File Offset: 0x00009990
		public override TimeSpan Timeout
		{
			get
			{
				return this.connectionTimeOut;
			}
			set
			{
				if (value < TimeSpan.Zero)
				{
					throw new ArgumentException(Res.GetString("NoNegativeTime"), "value");
				}
				if (value.TotalSeconds > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("TimespanExceedMax"), "value");
				}
				this.connectionTimeOut = value;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000254 RID: 596 RVA: 0x0000A9ED File Offset: 0x000099ED
		// (set) Token: 0x06000255 RID: 597 RVA: 0x0000A9F5 File Offset: 0x000099F5
		public AuthType AuthType
		{
			get
			{
				return this.connectionAuthType;
			}
			set
			{
				if (value < AuthType.Anonymous || value > AuthType.Kerberos)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(AuthType));
				}
				if (this.bounded && value != this.connectionAuthType)
				{
					this.needRebind = true;
				}
				this.connectionAuthType = value;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000256 RID: 598 RVA: 0x0000AA35 File Offset: 0x00009A35
		public LdapSessionOptions SessionOptions
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (set) Token: 0x06000257 RID: 599 RVA: 0x0000AA40 File Offset: 0x00009A40
		public override NetworkCredential Credential
		{
			[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			set
			{
				if (this.bounded && !this.SameCredential(this.directoryCredential, value))
				{
					this.needRebind = true;
				}
				this.directoryCredential = ((value != null) ? new NetworkCredential(value.UserName, value.Password, value.Domain) : null);
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000258 RID: 600 RVA: 0x0000AA8E File Offset: 0x00009A8E
		// (set) Token: 0x06000259 RID: 601 RVA: 0x0000AA96 File Offset: 0x00009A96
		public bool AutoBind
		{
			get
			{
				return this.automaticBind;
			}
			set
			{
				this.automaticBind = value;
			}
		}

		// Token: 0x0600025A RID: 602 RVA: 0x0000AAA0 File Offset: 0x00009AA0
		internal void Init()
		{
			string text = null;
			string[] array = ((this.directoryIdentifier == null) ? null : ((LdapDirectoryIdentifier)this.directoryIdentifier).Servers);
			if (array != null && array.Length != 0)
			{
				StringBuilder stringBuilder = new StringBuilder(200);
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] != null)
					{
						stringBuilder.Append(array[i]);
						if (i < array.Length - 1)
						{
							stringBuilder.Append(" ");
						}
					}
				}
				if (stringBuilder.Length != 0)
				{
					text = stringBuilder.ToString();
				}
			}
			if (((LdapDirectoryIdentifier)this.directoryIdentifier).Connectionless)
			{
				this.ldapHandle = Wldap32.cldap_open(text, ((LdapDirectoryIdentifier)this.directoryIdentifier).PortNumber);
			}
			else
			{
				this.ldapHandle = Wldap32.ldap_init(text, ((LdapDirectoryIdentifier)this.directoryIdentifier).PortNumber);
			}
			if (!(this.ldapHandle == (IntPtr)0))
			{
				lock (LdapConnection.objectLock)
				{
					if (LdapConnection.handleTable[this.ldapHandle] != null)
					{
						LdapConnection.handleTable.Remove(this.ldapHandle);
					}
					LdapConnection.handleTable.Add(this.ldapHandle, new WeakReference(this));
				}
				return;
			}
			int num = Wldap32.LdapGetLastError();
			if (Utility.IsLdapError((LdapError)num))
			{
				string text2 = LdapErrorMappings.MapResultCode(num);
				throw new LdapException(num, text2);
			}
			throw new LdapException(num);
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0000AC18 File Offset: 0x00009C18
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override DirectoryResponse SendRequest(DirectoryRequest request)
		{
			return this.SendRequest(request, this.connectionTimeOut);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0000AC28 File Offset: 0x00009C28
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public DirectoryResponse SendRequest(DirectoryRequest request, TimeSpan requestTimeout)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (request is DsmlAuthRequest)
			{
				throw new NotSupportedException(Res.GetString("DsmlAuthRequestNotSupported"));
			}
			int num = 0;
			int num2 = this.SendRequestHelper(request, ref num);
			LdapOperation ldapOperation = LdapOperation.LdapSearch;
			if (request is DeleteRequest)
			{
				ldapOperation = LdapOperation.LdapDelete;
			}
			else if (request is AddRequest)
			{
				ldapOperation = LdapOperation.LdapAdd;
			}
			else if (request is ModifyRequest)
			{
				ldapOperation = LdapOperation.LdapModify;
			}
			else if (request is SearchRequest)
			{
				ldapOperation = LdapOperation.LdapSearch;
			}
			else if (request is ModifyDNRequest)
			{
				ldapOperation = LdapOperation.LdapModifyDn;
			}
			else if (request is CompareRequest)
			{
				ldapOperation = LdapOperation.LdapCompare;
			}
			else if (request is ExtendedRequest)
			{
				ldapOperation = LdapOperation.LdapExtendedRequest;
			}
			if (num2 == 0 && num != -1)
			{
				return this.ConstructResponse(num, ldapOperation, ResultAll.LDAP_MSG_ALL, requestTimeout, true);
			}
			if (num2 == 0)
			{
				num2 = Wldap32.LdapGetLastError();
			}
			throw this.ConstructException(num2, ldapOperation);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0000ACF8 File Offset: 0x00009CF8
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public IAsyncResult BeginSendRequest(DirectoryRequest request, PartialResultProcessing partialMode, AsyncCallback callback, object state)
		{
			return this.BeginSendRequest(request, this.connectionTimeOut, partialMode, callback, state);
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000AD0C File Offset: 0x00009D0C
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public IAsyncResult BeginSendRequest(DirectoryRequest request, TimeSpan requestTimeout, PartialResultProcessing partialMode, AsyncCallback callback, object state)
		{
			int num = 0;
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (request == null)
			{
				throw new ArgumentNullException("request");
			}
			if (partialMode < PartialResultProcessing.NoPartialResultSupport || partialMode > PartialResultProcessing.ReturnPartialResultsAndNotifyCallback)
			{
				throw new InvalidEnumArgumentException("partialMode", (int)partialMode, typeof(PartialResultProcessing));
			}
			if (partialMode != PartialResultProcessing.NoPartialResultSupport && !(request is SearchRequest))
			{
				throw new NotSupportedException(Res.GetString("PartialResultsNotSupported"));
			}
			if (partialMode == PartialResultProcessing.ReturnPartialResultsAndNotifyCallback && callback == null)
			{
				throw new ArgumentException(Res.GetString("CallBackIsNull"), "callback");
			}
			int num2 = this.SendRequestHelper(request, ref num);
			LdapOperation ldapOperation = LdapOperation.LdapSearch;
			if (request is DeleteRequest)
			{
				ldapOperation = LdapOperation.LdapDelete;
			}
			else if (request is AddRequest)
			{
				ldapOperation = LdapOperation.LdapAdd;
			}
			else if (request is ModifyRequest)
			{
				ldapOperation = LdapOperation.LdapModify;
			}
			else if (request is SearchRequest)
			{
				ldapOperation = LdapOperation.LdapSearch;
			}
			else if (request is ModifyDNRequest)
			{
				ldapOperation = LdapOperation.LdapModifyDn;
			}
			else if (request is CompareRequest)
			{
				ldapOperation = LdapOperation.LdapCompare;
			}
			else if (request is ExtendedRequest)
			{
				ldapOperation = LdapOperation.LdapExtendedRequest;
			}
			if (num2 != 0 || num == -1)
			{
				if (num2 == 0)
				{
					num2 = Wldap32.LdapGetLastError();
				}
				throw this.ConstructException(num2, ldapOperation);
			}
			if (partialMode == PartialResultProcessing.NoPartialResultSupport)
			{
				LdapRequestState ldapRequestState = new LdapRequestState();
				LdapAsyncResult ldapAsyncResult = new LdapAsyncResult(callback, state, false);
				ldapRequestState.ldapAsync = ldapAsyncResult;
				ldapAsyncResult.resultObject = ldapRequestState;
				LdapConnection.asyncResultTable.Add(ldapAsyncResult, num);
				this.fd.BeginInvoke(num, ldapOperation, ResultAll.LDAP_MSG_ALL, requestTimeout, true, new AsyncCallback(this.ResponseCallback), ldapRequestState);
				return ldapAsyncResult;
			}
			bool flag = false;
			if (partialMode == PartialResultProcessing.ReturnPartialResultsAndNotifyCallback)
			{
				flag = true;
			}
			LdapPartialAsyncResult ldapPartialAsyncResult = new LdapPartialAsyncResult(num, callback, state, true, this, flag, requestTimeout);
			LdapConnection.partialResultsProcessor.Add(ldapPartialAsyncResult);
			return ldapPartialAsyncResult;
		}

		// Token: 0x0600025F RID: 607 RVA: 0x0000AE98 File Offset: 0x00009E98
		private void ResponseCallback(IAsyncResult asyncResult)
		{
			LdapRequestState ldapRequestState = (LdapRequestState)asyncResult.AsyncState;
			try
			{
				DirectoryResponse directoryResponse = this.fd.EndInvoke(asyncResult);
				ldapRequestState.response = directoryResponse;
			}
			catch (Exception ex)
			{
				ldapRequestState.exception = ex;
				ldapRequestState.response = null;
			}
			catch
			{
				ldapRequestState.exception = new Exception(Res.GetString("NonCLSException"));
				ldapRequestState.response = null;
			}
			ldapRequestState.ldapAsync.manualResetEvent.Set();
			ldapRequestState.ldapAsync.completed = true;
			if (ldapRequestState.ldapAsync.callback != null && !ldapRequestState.abortCalled)
			{
				ldapRequestState.ldapAsync.callback(ldapRequestState.ldapAsync);
			}
		}

		// Token: 0x06000260 RID: 608 RVA: 0x0000AF5C File Offset: 0x00009F5C
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public void Abort(IAsyncResult asyncResult)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!(asyncResult is LdapAsyncResult))
			{
				throw new ArgumentException(Res.GetString("NotReturnedAsyncResult", new object[] { "asyncResult" }));
			}
			LdapAsyncResult ldapAsyncResult = (LdapAsyncResult)asyncResult;
			int num;
			if (!ldapAsyncResult.partialResults)
			{
				if (!LdapConnection.asyncResultTable.Contains(asyncResult))
				{
					throw new ArgumentException(Res.GetString("InvalidAsyncResult"));
				}
				num = (int)LdapConnection.asyncResultTable[asyncResult];
				LdapConnection.asyncResultTable.Remove(asyncResult);
			}
			else
			{
				LdapConnection.partialResultsProcessor.Remove((LdapPartialAsyncResult)asyncResult);
				num = ((LdapPartialAsyncResult)asyncResult).messageID;
			}
			Wldap32.ldap_abandon(this.ldapHandle, num);
			LdapRequestState resultObject = ldapAsyncResult.resultObject;
			if (resultObject != null)
			{
				resultObject.abortCalled = true;
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0000B040 File Offset: 0x0000A040
		public PartialResultsCollection GetPartialResults(IAsyncResult asyncResult)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!(asyncResult is LdapAsyncResult))
			{
				throw new ArgumentException(Res.GetString("NotReturnedAsyncResult", new object[] { "asyncResult" }));
			}
			if (!(asyncResult is LdapPartialAsyncResult))
			{
				throw new InvalidOperationException(Res.GetString("NoPartialResults"));
			}
			return LdapConnection.partialResultsProcessor.GetPartialResults((LdapPartialAsyncResult)asyncResult);
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0000B0C4 File Offset: 0x0000A0C4
		public DirectoryResponse EndSendRequest(IAsyncResult asyncResult)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (asyncResult == null)
			{
				throw new ArgumentNullException("asyncResult");
			}
			if (!(asyncResult is LdapAsyncResult))
			{
				throw new ArgumentException(Res.GetString("NotReturnedAsyncResult", new object[] { "asyncResult" }));
			}
			LdapAsyncResult ldapAsyncResult = (LdapAsyncResult)asyncResult;
			if (ldapAsyncResult.partialResults)
			{
				LdapConnection.partialResultsProcessor.NeedCompleteResult((LdapPartialAsyncResult)asyncResult);
				asyncResult.AsyncWaitHandle.WaitOne();
				return LdapConnection.partialResultsProcessor.GetCompleteResult((LdapPartialAsyncResult)asyncResult);
			}
			if (!LdapConnection.asyncResultTable.Contains(asyncResult))
			{
				throw new ArgumentException(Res.GetString("InvalidAsyncResult"));
			}
			LdapConnection.asyncResultTable.Remove(asyncResult);
			asyncResult.AsyncWaitHandle.WaitOne();
			if (ldapAsyncResult.resultObject.exception != null)
			{
				throw ldapAsyncResult.resultObject.exception;
			}
			return ldapAsyncResult.resultObject.response;
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000B1B4 File Offset: 0x0000A1B4
		private int SendRequestHelper(DirectoryRequest request, ref int messageID)
		{
			IntPtr intPtr = (IntPtr)0;
			LdapControl[] array = null;
			IntPtr intPtr2 = (IntPtr)0;
			LdapControl[] array2 = null;
			string text = null;
			ArrayList arrayList = new ArrayList();
			LdapMod[] array3 = null;
			IntPtr intPtr3 = (IntPtr)0;
			int num = 0;
			berval berval = null;
			IntPtr intPtr4 = (IntPtr)0;
			int num2 = 0;
			int num3 = 0;
			if (!this.connected)
			{
				this.Connect();
				this.connected = true;
			}
			if (this.AutoBind && (!this.bounded || this.needRebind) && !((LdapDirectoryIdentifier)this.Directory).Connectionless)
			{
				this.Bind();
			}
			int num7;
			try
			{
				IntPtr intPtr5 = (IntPtr)0;
				IntPtr intPtr6 = (IntPtr)0;
				array = this.BuildControlArray(request.Controls, true);
				int num4 = Marshal.SizeOf(typeof(LdapControl));
				if (array != null)
				{
					intPtr = Utility.AllocHGlobalIntPtrArray(array.Length + 1);
					for (int i = 0; i < array.Length; i++)
					{
						intPtr5 = Marshal.AllocHGlobal(num4);
						Marshal.StructureToPtr(array[i], intPtr5, false);
						intPtr6 = (IntPtr)((long)intPtr + (long)(Marshal.SizeOf(typeof(IntPtr)) * i));
						Marshal.WriteIntPtr(intPtr6, intPtr5);
					}
					intPtr6 = (IntPtr)((long)intPtr + (long)(Marshal.SizeOf(typeof(IntPtr)) * array.Length));
					Marshal.WriteIntPtr(intPtr6, (IntPtr)0);
				}
				array2 = this.BuildControlArray(request.Controls, false);
				if (array2 != null)
				{
					intPtr2 = Utility.AllocHGlobalIntPtrArray(array2.Length + 1);
					for (int j = 0; j < array2.Length; j++)
					{
						intPtr5 = Marshal.AllocHGlobal(num4);
						Marshal.StructureToPtr(array2[j], intPtr5, false);
						intPtr6 = (IntPtr)((long)intPtr2 + (long)(Marshal.SizeOf(typeof(IntPtr)) * j));
						Marshal.WriteIntPtr(intPtr6, intPtr5);
					}
					intPtr6 = (IntPtr)((long)intPtr2 + (long)(Marshal.SizeOf(typeof(IntPtr)) * array2.Length));
					Marshal.WriteIntPtr(intPtr6, (IntPtr)0);
				}
				if (request is DeleteRequest)
				{
					num3 = Wldap32.ldap_delete_ext(this.ldapHandle, ((DeleteRequest)request).DistinguishedName, intPtr, intPtr2, ref messageID);
				}
				else if (request is ModifyDNRequest)
				{
					num3 = Wldap32.ldap_rename(this.ldapHandle, ((ModifyDNRequest)request).DistinguishedName, ((ModifyDNRequest)request).NewName, ((ModifyDNRequest)request).NewParentDistinguishedName, ((ModifyDNRequest)request).DeleteOldRdn ? 1 : 0, intPtr, intPtr2, ref messageID);
				}
				else if (request is CompareRequest)
				{
					DirectoryAttribute assertion = ((CompareRequest)request).Assertion;
					if (assertion == null)
					{
						throw new ArgumentException(Res.GetString("WrongAssertionCompare"));
					}
					if (assertion.Count != 1)
					{
						throw new ArgumentException(Res.GetString("WrongNumValuesCompare"));
					}
					byte[] array4 = assertion[0] as byte[];
					if (array4 != null)
					{
						if (array4 != null && array4.Length != 0)
						{
							berval = new berval();
							berval.bv_len = array4.Length;
							berval.bv_val = Marshal.AllocHGlobal(array4.Length);
							Marshal.Copy(array4, 0, berval.bv_val, array4.Length);
						}
					}
					else
					{
						text = assertion[0].ToString();
					}
					num3 = Wldap32.ldap_compare(this.ldapHandle, ((CompareRequest)request).DistinguishedName, assertion.Name, text, berval, intPtr, intPtr2, ref messageID);
				}
				else if (request is AddRequest || request is ModifyRequest)
				{
					if (request is AddRequest)
					{
						array3 = this.BuildAttributes(((AddRequest)request).Attributes, arrayList);
					}
					else
					{
						array3 = this.BuildAttributes(((ModifyRequest)request).Modifications, arrayList);
					}
					num = ((array3 == null) ? 1 : (array3.Length + 1));
					intPtr3 = Utility.AllocHGlobalIntPtrArray(num);
					int num5 = Marshal.SizeOf(typeof(LdapMod));
					int k;
					for (k = 0; k < num - 1; k++)
					{
						intPtr5 = Marshal.AllocHGlobal(num5);
						Marshal.StructureToPtr(array3[k], intPtr5, false);
						intPtr6 = (IntPtr)((long)intPtr3 + (long)(Marshal.SizeOf(typeof(IntPtr)) * k));
						Marshal.WriteIntPtr(intPtr6, intPtr5);
					}
					intPtr6 = (IntPtr)((long)intPtr3 + (long)(Marshal.SizeOf(typeof(IntPtr)) * k));
					Marshal.WriteIntPtr(intPtr6, (IntPtr)0);
					if (request is AddRequest)
					{
						num3 = Wldap32.ldap_add(this.ldapHandle, ((AddRequest)request).DistinguishedName, intPtr3, intPtr, intPtr2, ref messageID);
					}
					else
					{
						num3 = Wldap32.ldap_modify(this.ldapHandle, ((ModifyRequest)request).DistinguishedName, intPtr3, intPtr, intPtr2, ref messageID);
					}
				}
				else
				{
					if (!(request is ExtendedRequest))
					{
						if (request is SearchRequest)
						{
							SearchRequest searchRequest = (SearchRequest)request;
							object filter = searchRequest.Filter;
							if (filter != null && filter is XmlDocument)
							{
								throw new ArgumentException(Res.GetString("InvalidLdapSearchRequestFilter"));
							}
							string text2 = (string)filter;
							num2 = ((searchRequest.Attributes == null) ? 0 : searchRequest.Attributes.Count);
							if (num2 != 0)
							{
								intPtr4 = Utility.AllocHGlobalIntPtrArray(num2 + 1);
								int l;
								for (l = 0; l < num2; l++)
								{
									intPtr5 = Marshal.StringToHGlobalUni(searchRequest.Attributes[l]);
									intPtr6 = (IntPtr)((long)intPtr4 + (long)(Marshal.SizeOf(typeof(IntPtr)) * l));
									Marshal.WriteIntPtr(intPtr6, intPtr5);
								}
								intPtr6 = (IntPtr)((long)intPtr4 + (long)(Marshal.SizeOf(typeof(IntPtr)) * l));
								Marshal.WriteIntPtr(intPtr6, (IntPtr)0);
							}
							int scope = (int)searchRequest.Scope;
							int num6 = (int)(searchRequest.TimeLimit.Ticks / 10000000L);
							DereferenceAlias derefAlias = this.options.DerefAlias;
							this.options.DerefAlias = searchRequest.Aliases;
							try
							{
								num3 = Wldap32.ldap_search(this.ldapHandle, searchRequest.DistinguishedName, scope, text2, intPtr4, searchRequest.TypesOnly, intPtr, intPtr2, num6, searchRequest.SizeLimit, ref messageID);
								goto IL_0666;
							}
							finally
							{
								this.options.DerefAlias = derefAlias;
							}
						}
						throw new NotSupportedException(Res.GetString("InvliadRequestType"));
					}
					string requestName = ((ExtendedRequest)request).RequestName;
					byte[] requestValue = ((ExtendedRequest)request).RequestValue;
					if (requestValue != null && requestValue.Length != 0)
					{
						berval = new berval();
						berval.bv_len = requestValue.Length;
						berval.bv_val = Marshal.AllocHGlobal(requestValue.Length);
						Marshal.Copy(requestValue, 0, berval.bv_val, requestValue.Length);
					}
					num3 = Wldap32.ldap_extended_operation(this.ldapHandle, requestName, berval, intPtr, intPtr2, ref messageID);
				}
				IL_0666:
				if (num3 == 85)
				{
					num3 = 112;
				}
				num7 = num3;
			}
			finally
			{
				GC.KeepAlive(array3);
				if (intPtr != (IntPtr)0)
				{
					for (int m = 0; m < array.Length; m++)
					{
						IntPtr intPtr7 = Marshal.ReadIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * m);
						if (intPtr7 != (IntPtr)0)
						{
							Marshal.FreeHGlobal(intPtr7);
						}
					}
					Marshal.FreeHGlobal(intPtr);
				}
				if (array != null)
				{
					for (int n = 0; n < array.Length; n++)
					{
						if (array[n].ldctl_oid != (IntPtr)0)
						{
							Marshal.FreeHGlobal(array[n].ldctl_oid);
						}
						if (array[n].ldctl_value != null && array[n].ldctl_value.bv_val != (IntPtr)0)
						{
							Marshal.FreeHGlobal(array[n].ldctl_value.bv_val);
						}
					}
				}
				if (intPtr2 != (IntPtr)0)
				{
					for (int num8 = 0; num8 < array2.Length; num8++)
					{
						IntPtr intPtr8 = Marshal.ReadIntPtr(intPtr2, Marshal.SizeOf(typeof(IntPtr)) * num8);
						if (intPtr8 != (IntPtr)0)
						{
							Marshal.FreeHGlobal(intPtr8);
						}
					}
					Marshal.FreeHGlobal(intPtr2);
				}
				if (array2 != null)
				{
					for (int num9 = 0; num9 < array2.Length; num9++)
					{
						if (array2[num9].ldctl_oid != (IntPtr)0)
						{
							Marshal.FreeHGlobal(array2[num9].ldctl_oid);
						}
						if (array2[num9].ldctl_value != null && array2[num9].ldctl_value.bv_val != (IntPtr)0)
						{
							Marshal.FreeHGlobal(array2[num9].ldctl_value.bv_val);
						}
					}
				}
				if (intPtr3 != (IntPtr)0)
				{
					for (int num10 = 0; num10 < num - 1; num10++)
					{
						IntPtr intPtr9 = Marshal.ReadIntPtr(intPtr3, Marshal.SizeOf(typeof(IntPtr)) * num10);
						if (intPtr9 != (IntPtr)0)
						{
							Marshal.FreeHGlobal(intPtr9);
						}
					}
					Marshal.FreeHGlobal(intPtr3);
				}
				for (int num11 = 0; num11 < arrayList.Count; num11++)
				{
					IntPtr intPtr10 = (IntPtr)arrayList[num11];
					Marshal.FreeHGlobal(intPtr10);
				}
				if (berval != null && berval.bv_val != (IntPtr)0)
				{
					Marshal.FreeHGlobal(berval.bv_val);
				}
				if (intPtr4 != (IntPtr)0)
				{
					for (int num12 = 0; num12 < num2; num12++)
					{
						IntPtr intPtr11 = Marshal.ReadIntPtr(intPtr4, Marshal.SizeOf(typeof(IntPtr)) * num12);
						if (intPtr11 != (IntPtr)0)
						{
							Marshal.FreeHGlobal(intPtr11);
						}
					}
					Marshal.FreeHGlobal(intPtr4);
				}
			}
			return num7;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000BB0C File Offset: 0x0000AB0C
		private bool ProcessClientCertificate(IntPtr ldapHandle, IntPtr CAs, ref IntPtr certificate)
		{
			ArrayList arrayList = new ArrayList();
			byte[][] array = null;
			if ((base.ClientCertificates == null || base.ClientCertificates.Count == 0) && this.options.clientCertificateDelegate == null)
			{
				return false;
			}
			if (this.options.clientCertificateDelegate == null)
			{
				certificate = base.ClientCertificates[0].Handle;
				return true;
			}
			if (CAs != (IntPtr)0)
			{
				SecPkgContext_IssuerListInfoEx secPkgContext_IssuerListInfoEx = (SecPkgContext_IssuerListInfoEx)Marshal.PtrToStructure(CAs, typeof(SecPkgContext_IssuerListInfoEx));
				int cIssuers = secPkgContext_IssuerListInfoEx.cIssuers;
				IntPtr intPtr = (IntPtr)0;
				for (int i = 0; i < cIssuers; i++)
				{
					intPtr = (IntPtr)((long)secPkgContext_IssuerListInfoEx.aIssuers + (long)(Marshal.SizeOf(typeof(CRYPTOAPI_BLOB)) * i));
					CRYPTOAPI_BLOB cryptoapi_BLOB = (CRYPTOAPI_BLOB)Marshal.PtrToStructure(intPtr, typeof(CRYPTOAPI_BLOB));
					int cbData = cryptoapi_BLOB.cbData;
					byte[] array2 = new byte[cbData];
					Marshal.Copy(cryptoapi_BLOB.pbData, array2, 0, cbData);
					arrayList.Add(array2);
				}
			}
			if (arrayList.Count != 0)
			{
				array = new byte[arrayList.Count][];
				for (int j = 0; j < arrayList.Count; j++)
				{
					array[j] = (byte[])arrayList[j];
				}
			}
			X509Certificate x509Certificate = this.options.clientCertificateDelegate(this, array);
			if (x509Certificate != null)
			{
				certificate = x509Certificate.Handle;
				return true;
			}
			certificate = (IntPtr)0;
			return false;
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000BC94 File Offset: 0x0000AC94
		private void Connect()
		{
			if (base.ClientCertificates.Count > 1)
			{
				throw new InvalidOperationException(Res.GetString("InvalidClientCertificates"));
			}
			if (base.ClientCertificates.Count != 0)
			{
				int num = Wldap32.ldap_set_option_clientcert(this.ldapHandle, LdapOption.LDAP_OPT_CLIENT_CERTIFICATE, this.clientCertificateRoutine);
				if (num != 0)
				{
					if (Utility.IsLdapError((LdapError)num))
					{
						string text = LdapErrorMappings.MapResultCode(num);
						throw new LdapException(num, text);
					}
					throw new LdapException(num);
				}
				else
				{
					this.automaticBind = false;
				}
			}
			if (((LdapDirectoryIdentifier)this.Directory).FullyQualifiedDnsHostName && !this.setFQDNDone)
			{
				this.SessionOptions.FQDN = true;
				this.setFQDNDone = true;
			}
			LDAP_TIMEVAL ldap_TIMEVAL = new LDAP_TIMEVAL();
			ldap_TIMEVAL.tv_sec = (int)(this.connectionTimeOut.Ticks / 10000000L);
			int num2 = Wldap32.ldap_connect(this.ldapHandle, ldap_TIMEVAL);
			if (num2 == 0)
			{
				return;
			}
			if (Utility.IsLdapError((LdapError)num2))
			{
				string text2 = LdapErrorMappings.MapResultCode(num2);
				throw new LdapException(num2, text2);
			}
			throw new LdapException(num2);
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0000BD8A File Offset: 0x0000AD8A
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public void Bind()
		{
			this.BindHelper(this.directoryCredential, false);
		}

		// Token: 0x06000267 RID: 615 RVA: 0x0000BD99 File Offset: 0x0000AD99
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public void Bind(NetworkCredential newCredential)
		{
			this.BindHelper(newCredential, true);
		}

		// Token: 0x06000268 RID: 616 RVA: 0x0000BDA4 File Offset: 0x0000ADA4
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		[EnvironmentPermission(SecurityAction.Assert, Unrestricted = true)]
		private void BindHelper(NetworkCredential newCredential, bool needSetCredential)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (this.AuthType == AuthType.Anonymous && newCredential != null && ((newCredential.Password != null && newCredential.Password.Length != 0) || (newCredential.UserName != null && newCredential.UserName.Length != 0)))
			{
				throw new InvalidOperationException(Res.GetString("InvalidAuthCredential"));
			}
			NetworkCredential networkCredential;
			if (needSetCredential)
			{
				networkCredential = (this.directoryCredential = ((newCredential != null) ? new NetworkCredential(newCredential.UserName, newCredential.Password, newCredential.Domain) : null));
			}
			else
			{
				networkCredential = this.directoryCredential;
			}
			if (!this.connected)
			{
				this.Connect();
				this.connected = true;
			}
			string text;
			string text2;
			string text3;
			if (networkCredential != null && networkCredential.UserName.Length == 0 && networkCredential.Password.Length == 0 && networkCredential.Domain.Length == 0)
			{
				text = null;
				text2 = null;
				text3 = null;
			}
			else
			{
				text = ((networkCredential == null) ? null : networkCredential.UserName);
				text2 = ((networkCredential == null) ? null : networkCredential.Domain);
				text3 = ((networkCredential == null) ? null : networkCredential.Password);
			}
			int num;
			if (this.AuthType == AuthType.Anonymous)
			{
				num = Wldap32.ldap_simple_bind_s(this.ldapHandle, null, null);
			}
			else if (this.AuthType == AuthType.Basic)
			{
				StringBuilder stringBuilder = new StringBuilder(100);
				if (text2 != null && text2.Length != 0)
				{
					stringBuilder.Append(text2);
					stringBuilder.Append("\\");
				}
				stringBuilder.Append(text);
				num = Wldap32.ldap_simple_bind_s(this.ldapHandle, stringBuilder.ToString(), text3);
			}
			else
			{
				SEC_WINNT_AUTH_IDENTITY_EX sec_WINNT_AUTH_IDENTITY_EX = new SEC_WINNT_AUTH_IDENTITY_EX();
				sec_WINNT_AUTH_IDENTITY_EX.version = 512;
				sec_WINNT_AUTH_IDENTITY_EX.length = Marshal.SizeOf(typeof(SEC_WINNT_AUTH_IDENTITY_EX));
				sec_WINNT_AUTH_IDENTITY_EX.flags = 2;
				if (this.AuthType == AuthType.Kerberos)
				{
					sec_WINNT_AUTH_IDENTITY_EX.packageList = "Kerberos";
					sec_WINNT_AUTH_IDENTITY_EX.packageListLength = sec_WINNT_AUTH_IDENTITY_EX.packageList.Length;
				}
				if (networkCredential != null)
				{
					sec_WINNT_AUTH_IDENTITY_EX.user = text;
					sec_WINNT_AUTH_IDENTITY_EX.userLength = ((text == null) ? 0 : text.Length);
					sec_WINNT_AUTH_IDENTITY_EX.domain = text2;
					sec_WINNT_AUTH_IDENTITY_EX.domainLength = ((text2 == null) ? 0 : text2.Length);
					sec_WINNT_AUTH_IDENTITY_EX.password = text3;
					sec_WINNT_AUTH_IDENTITY_EX.passwordLength = ((text3 == null) ? 0 : text3.Length);
				}
				BindMethod bindMethod = BindMethod.LDAP_AUTH_NEGOTIATE;
				switch (this.AuthType)
				{
				case AuthType.Negotiate:
					bindMethod = BindMethod.LDAP_AUTH_NEGOTIATE;
					break;
				case AuthType.Ntlm:
					bindMethod = BindMethod.LDAP_AUTH_NTLM;
					break;
				case AuthType.Digest:
					bindMethod = BindMethod.LDAP_AUTH_DIGEST;
					break;
				case AuthType.Sicily:
					bindMethod = BindMethod.LDAP_AUTH_SICILY;
					break;
				case AuthType.Dpa:
					bindMethod = BindMethod.LDAP_AUTH_DPA;
					break;
				case AuthType.Msn:
					bindMethod = BindMethod.LDAP_AUTH_MSN;
					break;
				case AuthType.External:
					bindMethod = BindMethod.LDAP_AUTH_EXTERNAL;
					break;
				case AuthType.Kerberos:
					bindMethod = BindMethod.LDAP_AUTH_NEGOTIATE;
					break;
				}
				if (networkCredential == null && this.AuthType == AuthType.External)
				{
					num = Wldap32.ldap_bind_s(this.ldapHandle, null, null, bindMethod);
				}
				else
				{
					num = Wldap32.ldap_bind_s(this.ldapHandle, null, sec_WINNT_AUTH_IDENTITY_EX, bindMethod);
				}
			}
			if (num == 0)
			{
				this.bounded = true;
				this.needRebind = false;
				return;
			}
			string text4;
			if (Utility.IsResultCode((ResultCode)num))
			{
				text4 = OperationErrorMappings.MapResultCode(num);
				throw new DirectoryOperationException(null, text4);
			}
			if (!Utility.IsLdapError((LdapError)num))
			{
				throw new LdapException(num);
			}
			text4 = LdapErrorMappings.MapResultCode(num);
			string serverErrorMessage = this.options.ServerErrorMessage;
			if (serverErrorMessage != null && serverErrorMessage.Length > 0)
			{
				throw new LdapException(num, text4, serverErrorMessage);
			}
			throw new LdapException(num, text4);
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0000C0FF File Offset: 0x0000B0FF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600026A RID: 618 RVA: 0x0000C110 File Offset: 0x0000B110
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (LdapConnection.objectLock)
				{
					LdapConnection.handleTable.Remove(this.ldapHandle);
				}
			}
			if (this.needDispose && this.ldapHandle != (IntPtr)0)
			{
				Wldap32.ldap_unbind(this.ldapHandle);
			}
			this.ldapHandle = (IntPtr)0;
			this.disposed = true;
		}

		// Token: 0x0600026B RID: 619 RVA: 0x0000C194 File Offset: 0x0000B194
		internal LdapControl[] BuildControlArray(DirectoryControlCollection controls, bool serverControl)
		{
			LdapControl[] array = null;
			if (controls != null && controls.Count != 0)
			{
				ArrayList arrayList = new ArrayList();
				foreach (object obj in controls)
				{
					DirectoryControl directoryControl = (DirectoryControl)obj;
					if (serverControl)
					{
						if (directoryControl.ServerSide)
						{
							arrayList.Add(directoryControl);
						}
					}
					else if (!directoryControl.ServerSide)
					{
						arrayList.Add(directoryControl);
					}
				}
				if (arrayList.Count != 0)
				{
					int count = arrayList.Count;
					array = new LdapControl[count];
					for (int i = 0; i < count; i++)
					{
						array[i] = new LdapControl();
						array[i].ldctl_oid = Marshal.StringToHGlobalUni(((DirectoryControl)arrayList[i]).Type);
						array[i].ldctl_iscritical = ((DirectoryControl)arrayList[i]).IsCritical;
						DirectoryControl directoryControl2 = (DirectoryControl)arrayList[i];
						byte[] value = directoryControl2.GetValue();
						if (value == null || value.Length == 0)
						{
							array[i].ldctl_value = new berval();
							array[i].ldctl_value.bv_len = 0;
							array[i].ldctl_value.bv_val = (IntPtr)0;
						}
						else
						{
							array[i].ldctl_value = new berval();
							array[i].ldctl_value.bv_len = value.Length;
							array[i].ldctl_value.bv_val = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(byte)) * array[i].ldctl_value.bv_len);
							Marshal.Copy(value, 0, array[i].ldctl_value.bv_val, array[i].ldctl_value.bv_len);
						}
					}
				}
			}
			return array;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0000C368 File Offset: 0x0000B368
		internal LdapMod[] BuildAttributes(CollectionBase directoryAttributes, ArrayList ptrToFree)
		{
			LdapMod[] array = null;
			UTF8Encoding utf8Encoding = new UTF8Encoding();
			DirectoryAttributeCollection directoryAttributeCollection = null;
			DirectoryAttributeModificationCollection directoryAttributeModificationCollection = null;
			if (directoryAttributes != null && directoryAttributes.Count != 0)
			{
				if (directoryAttributes is DirectoryAttributeModificationCollection)
				{
					directoryAttributeModificationCollection = (DirectoryAttributeModificationCollection)directoryAttributes;
				}
				else
				{
					directoryAttributeCollection = (DirectoryAttributeCollection)directoryAttributes;
				}
				array = new LdapMod[directoryAttributes.Count];
				for (int i = 0; i < directoryAttributes.Count; i++)
				{
					DirectoryAttribute directoryAttribute;
					if (directoryAttributeCollection != null)
					{
						directoryAttribute = directoryAttributeCollection[i];
					}
					else
					{
						directoryAttribute = directoryAttributeModificationCollection[i];
					}
					array[i] = new LdapMod();
					if (directoryAttribute is DirectoryAttributeModification)
					{
						array[i].type = (int)((DirectoryAttributeModification)directoryAttribute).Operation;
					}
					else
					{
						array[i].type = 0;
					}
					array[i].type |= 128;
					array[i].attribute = Marshal.StringToHGlobalUni(directoryAttribute.Name);
					int num = 0;
					berval[] array2 = null;
					if (directoryAttribute.Count > 0)
					{
						num = directoryAttribute.Count;
						array2 = new berval[num];
						for (int j = 0; j < num; j++)
						{
							byte[] array3;
							if (directoryAttribute[j] is string)
							{
								array3 = utf8Encoding.GetBytes((string)directoryAttribute[j]);
							}
							else if (directoryAttribute[j] is Uri)
							{
								array3 = utf8Encoding.GetBytes(((Uri)directoryAttribute[j]).ToString());
							}
							else
							{
								array3 = (byte[])directoryAttribute[j];
							}
							array2[j] = new berval();
							array2[j].bv_len = array3.Length;
							array2[j].bv_val = Marshal.AllocHGlobal(array2[j].bv_len);
							ptrToFree.Add(array2[j].bv_val);
							Marshal.Copy(array3, 0, array2[j].bv_val, array2[j].bv_len);
						}
					}
					array[i].values = Utility.AllocHGlobalIntPtrArray(num + 1);
					int num2 = Marshal.SizeOf(typeof(berval));
					IntPtr intPtr = (IntPtr)0;
					IntPtr intPtr2 = (IntPtr)0;
					int k;
					for (k = 0; k < num; k++)
					{
						intPtr = Marshal.AllocHGlobal(num2);
						ptrToFree.Add(intPtr);
						Marshal.StructureToPtr(array2[k], intPtr, false);
						intPtr2 = (IntPtr)((long)array[i].values + (long)(Marshal.SizeOf(typeof(IntPtr)) * k));
						Marshal.WriteIntPtr(intPtr2, intPtr);
					}
					intPtr2 = (IntPtr)((long)array[i].values + (long)(Marshal.SizeOf(typeof(IntPtr)) * k));
					Marshal.WriteIntPtr(intPtr2, (IntPtr)0);
				}
			}
			return array;
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0000C628 File Offset: 0x0000B628
		internal DirectoryResponse ConstructResponse(int messageId, LdapOperation operation, ResultAll resultType, TimeSpan requestTimeOut, bool exceptionOnTimeOut)
		{
			LDAP_TIMEVAL ldap_TIMEVAL = new LDAP_TIMEVAL();
			ldap_TIMEVAL.tv_sec = (int)(requestTimeOut.Ticks / 10000000L);
			IntPtr intPtr = (IntPtr)0;
			DirectoryResponse directoryResponse = null;
			IntPtr intPtr2 = (IntPtr)0;
			IntPtr intPtr3 = (IntPtr)0;
			IntPtr intPtr4 = (IntPtr)0;
			bool flag = true;
			if (resultType != ResultAll.LDAP_MSG_ALL)
			{
				ldap_TIMEVAL.tv_sec = 0;
				ldap_TIMEVAL.tv_usec = 0;
				if (resultType == ResultAll.LDAP_MSG_POLLINGALL)
				{
					resultType = ResultAll.LDAP_MSG_ALL;
				}
				flag = false;
			}
			int num = Wldap32.ldap_result(this.ldapHandle, messageId, (int)resultType, ldap_TIMEVAL, ref intPtr);
			if (num != -1 && num != 0)
			{
				int num2 = 0;
				try
				{
					int num3 = 0;
					string text = null;
					string text2 = null;
					Uri[] array = null;
					DirectoryControl[] array2 = null;
					if (num != 100 && num != 115)
					{
						num3 = this.ConstructParsedResult(intPtr, ref num2, ref text, ref text2, ref array, ref array2);
					}
					if (num3 != 0)
					{
						num = num3;
						goto IL_03A7;
					}
					num3 = num2;
					if (num == 105)
					{
						directoryResponse = new AddResponse(text, array2, (ResultCode)num3, text2, array);
					}
					else if (num == 103)
					{
						directoryResponse = new ModifyResponse(text, array2, (ResultCode)num3, text2, array);
					}
					else if (num == 107)
					{
						directoryResponse = new DeleteResponse(text, array2, (ResultCode)num3, text2, array);
					}
					else if (num == 109)
					{
						directoryResponse = new ModifyDNResponse(text, array2, (ResultCode)num3, text2, array);
					}
					else if (num == 111)
					{
						directoryResponse = new CompareResponse(text, array2, (ResultCode)num3, text2, array);
					}
					else if (num == 120)
					{
						directoryResponse = new ExtendedResponse(text, array2, (ResultCode)num3, text2, array);
						if (num3 == 0)
						{
							num3 = Wldap32.ldap_parse_extended_result(this.ldapHandle, intPtr, ref intPtr2, ref intPtr3, 0);
							if (num3 == 0)
							{
								string text3 = null;
								if (intPtr2 != (IntPtr)0)
								{
									text3 = Marshal.PtrToStringUni(intPtr2);
								}
								byte[] array3 = null;
								if (intPtr3 != (IntPtr)0)
								{
									berval berval = new berval();
									Marshal.PtrToStructure(intPtr3, berval);
									if (berval.bv_len != 0 && berval.bv_val != (IntPtr)0)
									{
										array3 = new byte[berval.bv_len];
										Marshal.Copy(berval.bv_val, array3, 0, berval.bv_len);
									}
								}
								((ExtendedResponse)directoryResponse).name = text3;
								((ExtendedResponse)directoryResponse).value = array3;
							}
						}
					}
					else if (num == 101 || num == 100 || num == 115)
					{
						directoryResponse = new SearchResponse(text, array2, (ResultCode)num3, text2, array);
						if (num == 101)
						{
							((SearchResponse)directoryResponse).searchDone = true;
						}
						SearchResultEntryCollection searchResultEntryCollection = new SearchResultEntryCollection();
						SearchResultReferenceCollection searchResultReferenceCollection = new SearchResultReferenceCollection();
						intPtr4 = Wldap32.ldap_first_entry(this.ldapHandle, intPtr);
						int num4 = 0;
						while (intPtr4 != (IntPtr)0)
						{
							SearchResultEntry searchResultEntry = this.ConstructEntry(intPtr4);
							if (searchResultEntry != null)
							{
								searchResultEntryCollection.Add(searchResultEntry);
							}
							num4++;
							intPtr4 = Wldap32.ldap_next_entry(this.ldapHandle, intPtr4);
						}
						IntPtr intPtr5 = Wldap32.ldap_first_reference(this.ldapHandle, intPtr);
						while (intPtr5 != (IntPtr)0)
						{
							SearchResultReference searchResultReference = this.ConstructReference(intPtr5);
							if (searchResultReference != null)
							{
								searchResultReferenceCollection.Add(searchResultReference);
							}
							intPtr5 = Wldap32.ldap_next_reference(this.ldapHandle, intPtr5);
						}
						((SearchResponse)directoryResponse).SetEntries(searchResultEntryCollection);
						((SearchResponse)directoryResponse).SetReferences(searchResultReferenceCollection);
					}
					if (num3 == 0 || num3 == 5 || num3 == 6 || num3 == 10 || num3 == 9)
					{
						return directoryResponse;
					}
					if (Utility.IsResultCode((ResultCode)num3))
					{
						throw new DirectoryOperationException(directoryResponse, OperationErrorMappings.MapResultCode(num3));
					}
					throw new DirectoryOperationException(directoryResponse);
				}
				finally
				{
					if (intPtr2 != (IntPtr)0)
					{
						Wldap32.ldap_memfree(intPtr2);
					}
					if (intPtr3 != (IntPtr)0)
					{
						Wldap32.ldap_memfree(intPtr3);
					}
					if (intPtr != (IntPtr)0)
					{
						Wldap32.ldap_msgfree(intPtr);
					}
				}
			}
			if (num == 0)
			{
				if (!exceptionOnTimeOut)
				{
					return null;
				}
				num = 85;
			}
			else
			{
				num = Wldap32.LdapGetLastError();
			}
			if (flag)
			{
				Wldap32.ldap_abandon(this.ldapHandle, messageId);
			}
			IL_03A7:
			throw this.ConstructException(num, operation);
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0000CA04 File Offset: 0x0000BA04
		internal unsafe int ConstructParsedResult(IntPtr ldapResult, ref int serverError, ref string responseDn, ref string responseMessage, ref Uri[] responseReferral, ref DirectoryControl[] responseControl)
		{
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			IntPtr intPtr3 = (IntPtr)0;
			IntPtr intPtr4 = (IntPtr)0;
			int num = 0;
			try
			{
				num = Wldap32.ldap_parse_result(this.ldapHandle, ldapResult, ref serverError, ref intPtr, ref intPtr2, ref intPtr3, ref intPtr4, 0);
				if (num == 0)
				{
					responseDn = Marshal.PtrToStringUni(intPtr);
					responseMessage = Marshal.PtrToStringUni(intPtr2);
					if (intPtr3 != (IntPtr)0)
					{
						char** ptr = (char**)(void*)intPtr3;
						char* ptr2 = *(IntPtr*)(ptr + 0 / sizeof(char*));
						int num2 = 0;
						ArrayList arrayList = new ArrayList();
						while (ptr2 != null)
						{
							string text = Marshal.PtrToStringUni((IntPtr)((void*)ptr2));
							arrayList.Add(text);
							num2++;
							ptr2 = *(IntPtr*)(ptr + (IntPtr)num2 * (IntPtr)sizeof(char*) / (IntPtr)sizeof(char*));
						}
						if (arrayList.Count > 0)
						{
							responseReferral = new Uri[arrayList.Count];
							for (int i = 0; i < arrayList.Count; i++)
							{
								responseReferral[i] = new Uri((string)arrayList[i]);
							}
						}
					}
					if (intPtr4 != (IntPtr)0)
					{
						int num3 = 0;
						IntPtr intPtr5 = intPtr4;
						IntPtr intPtr6 = Marshal.ReadIntPtr(intPtr5, 0);
						ArrayList arrayList2 = new ArrayList();
						while (intPtr6 != (IntPtr)0)
						{
							DirectoryControl directoryControl = this.ConstructControl(intPtr6);
							arrayList2.Add(directoryControl);
							num3++;
							intPtr6 = Marshal.ReadIntPtr(intPtr5, num3 * Marshal.SizeOf(typeof(IntPtr)));
						}
						responseControl = new DirectoryControl[arrayList2.Count];
						arrayList2.CopyTo(responseControl);
					}
				}
				else if (num == 82)
				{
					int num4 = Wldap32.ldap_result2error(this.ldapHandle, ldapResult, 0);
					if (num4 != 0)
					{
						num = num4;
					}
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Wldap32.ldap_memfree(intPtr);
				}
				if (intPtr2 != (IntPtr)0)
				{
					Wldap32.ldap_memfree(intPtr2);
				}
				if (intPtr3 != (IntPtr)0)
				{
					Wldap32.ldap_value_free(intPtr3);
				}
				if (intPtr4 != (IntPtr)0)
				{
					Wldap32.ldap_controls_free(intPtr4);
				}
			}
			return num;
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0000CC24 File Offset: 0x0000BC24
		internal SearchResultEntry ConstructEntry(IntPtr entryMessage)
		{
			IntPtr intPtr = (IntPtr)0;
			string text = null;
			IntPtr intPtr2 = (IntPtr)0;
			IntPtr intPtr3 = (IntPtr)0;
			SearchResultEntry searchResultEntry2;
			try
			{
				intPtr = Wldap32.ldap_get_dn(this.ldapHandle, entryMessage);
				if (intPtr != (IntPtr)0)
				{
					text = Marshal.PtrToStringUni(intPtr);
					Wldap32.ldap_memfree(intPtr);
					intPtr = (IntPtr)0;
				}
				SearchResultEntry searchResultEntry = new SearchResultEntry(text);
				SearchResultAttributeCollection attributes = searchResultEntry.Attributes;
				intPtr2 = Wldap32.ldap_first_attribute(this.ldapHandle, entryMessage, ref intPtr3);
				int num = 0;
				while (intPtr2 != (IntPtr)0)
				{
					DirectoryAttribute directoryAttribute = this.ConstructAttribute(entryMessage, intPtr2);
					attributes.Add(directoryAttribute.Name, directoryAttribute);
					Wldap32.ldap_memfree(intPtr2);
					num++;
					intPtr2 = Wldap32.ldap_next_attribute(this.ldapHandle, entryMessage, intPtr3);
				}
				if (intPtr3 != (IntPtr)0)
				{
					Wldap32.ber_free(intPtr3, 0);
					intPtr3 = (IntPtr)0;
				}
				searchResultEntry2 = searchResultEntry;
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Wldap32.ldap_memfree(intPtr);
				}
				if (intPtr2 != (IntPtr)0)
				{
					Wldap32.ldap_memfree(intPtr2);
				}
				if (intPtr3 != (IntPtr)0)
				{
					Wldap32.ber_free(intPtr3, 0);
				}
			}
			return searchResultEntry2;
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0000CD58 File Offset: 0x0000BD58
		internal DirectoryAttribute ConstructAttribute(IntPtr entryMessage, IntPtr attributeName)
		{
			DirectoryAttribute directoryAttribute = new DirectoryAttribute();
			directoryAttribute.isSearchResult = true;
			string text = Marshal.PtrToStringUni(attributeName);
			directoryAttribute.Name = text;
			IntPtr intPtr = Wldap32.ldap_get_values_len(this.ldapHandle, entryMessage, text);
			try
			{
				IntPtr intPtr2 = (IntPtr)0;
				int num = 0;
				if (intPtr != (IntPtr)0)
				{
					intPtr2 = Marshal.ReadIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * num);
					while (intPtr2 != (IntPtr)0)
					{
						berval berval = new berval();
						Marshal.PtrToStructure(intPtr2, berval);
						if (berval.bv_len > 0 && berval.bv_val != (IntPtr)0)
						{
							byte[] array = new byte[berval.bv_len];
							Marshal.Copy(berval.bv_val, array, 0, berval.bv_len);
							directoryAttribute.Add(array);
						}
						num++;
						intPtr2 = Marshal.ReadIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * num);
					}
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Wldap32.ldap_value_free_len(intPtr);
				}
			}
			return directoryAttribute;
		}

		// Token: 0x06000271 RID: 625 RVA: 0x0000CE7C File Offset: 0x0000BE7C
		internal SearchResultReference ConstructReference(IntPtr referenceMessage)
		{
			SearchResultReference searchResultReference = null;
			ArrayList arrayList = new ArrayList();
			IntPtr intPtr = (IntPtr)0;
			int num = Wldap32.ldap_parse_reference(this.ldapHandle, referenceMessage, ref intPtr);
			try
			{
				if (num == 0)
				{
					IntPtr intPtr2 = (IntPtr)0;
					int num2 = 0;
					if (intPtr != (IntPtr)0)
					{
						intPtr2 = Marshal.ReadIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * num2);
						while (intPtr2 != (IntPtr)0)
						{
							string text = Marshal.PtrToStringUni(intPtr2);
							arrayList.Add(text);
							num2++;
							intPtr2 = Marshal.ReadIntPtr(intPtr, Marshal.SizeOf(typeof(IntPtr)) * num2);
						}
						Wldap32.ldap_value_free(intPtr);
						intPtr = (IntPtr)0;
					}
					if (arrayList.Count > 0)
					{
						Uri[] array = new Uri[arrayList.Count];
						for (int i = 0; i < arrayList.Count; i++)
						{
							array[i] = new Uri((string)arrayList[i]);
						}
						searchResultReference = new SearchResultReference(array);
					}
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Wldap32.ldap_value_free(intPtr);
				}
			}
			return searchResultReference;
		}

		// Token: 0x06000272 RID: 626 RVA: 0x0000CFA8 File Offset: 0x0000BFA8
		private DirectoryException ConstructException(int error, LdapOperation operation)
		{
			DirectoryResponse directoryResponse = null;
			if (Utility.IsResultCode((ResultCode)error))
			{
				if (operation == LdapOperation.LdapAdd)
				{
					directoryResponse = new AddResponse(null, null, (ResultCode)error, OperationErrorMappings.MapResultCode(error), null);
				}
				else if (operation == LdapOperation.LdapModify)
				{
					directoryResponse = new ModifyResponse(null, null, (ResultCode)error, OperationErrorMappings.MapResultCode(error), null);
				}
				else if (operation == LdapOperation.LdapDelete)
				{
					directoryResponse = new DeleteResponse(null, null, (ResultCode)error, OperationErrorMappings.MapResultCode(error), null);
				}
				else if (operation == LdapOperation.LdapModifyDn)
				{
					directoryResponse = new ModifyDNResponse(null, null, (ResultCode)error, OperationErrorMappings.MapResultCode(error), null);
				}
				else if (operation == LdapOperation.LdapCompare)
				{
					directoryResponse = new CompareResponse(null, null, (ResultCode)error, OperationErrorMappings.MapResultCode(error), null);
				}
				else if (operation == LdapOperation.LdapSearch)
				{
					directoryResponse = new SearchResponse(null, null, (ResultCode)error, OperationErrorMappings.MapResultCode(error), null);
				}
				else if (operation == LdapOperation.LdapExtendedRequest)
				{
					directoryResponse = new ExtendedResponse(null, null, (ResultCode)error, OperationErrorMappings.MapResultCode(error), null);
				}
				string text = OperationErrorMappings.MapResultCode(error);
				return new DirectoryOperationException(directoryResponse, text);
			}
			if (!Utility.IsLdapError((LdapError)error))
			{
				return new LdapException(error);
			}
			string text2 = LdapErrorMappings.MapResultCode(error);
			string serverErrorMessage = this.options.ServerErrorMessage;
			if (serverErrorMessage != null && serverErrorMessage.Length > 0)
			{
				throw new LdapException(error, text2, serverErrorMessage);
			}
			return new LdapException(error, text2);
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0000D0AC File Offset: 0x0000C0AC
		private DirectoryControl ConstructControl(IntPtr controlPtr)
		{
			LdapControl ldapControl = new LdapControl();
			Marshal.PtrToStructure(controlPtr, ldapControl);
			string text = Marshal.PtrToStringUni(ldapControl.ldctl_oid);
			byte[] array = new byte[ldapControl.ldctl_value.bv_len];
			Marshal.Copy(ldapControl.ldctl_value.bv_val, array, 0, ldapControl.ldctl_value.bv_len);
			bool ldctl_iscritical = ldapControl.ldctl_iscritical;
			return new DirectoryControl(text, array, ldctl_iscritical, true);
		}

		// Token: 0x06000274 RID: 628 RVA: 0x0000D110 File Offset: 0x0000C110
		private bool SameCredential(NetworkCredential oldCredential, NetworkCredential newCredential)
		{
			return (oldCredential == null && newCredential == null) || ((oldCredential != null || newCredential == null) && (oldCredential == null || newCredential != null) && (oldCredential.Domain == newCredential.Domain && oldCredential.UserName == newCredential.UserName && oldCredential.Password == newCredential.Password));
		}

		// Token: 0x04000235 RID: 565
		private const int LDAP_MOD_BVALUES = 128;

		// Token: 0x04000236 RID: 566
		private AuthType connectionAuthType;

		// Token: 0x04000237 RID: 567
		private LdapSessionOptions options;

		// Token: 0x04000238 RID: 568
		internal IntPtr ldapHandle;

		// Token: 0x04000239 RID: 569
		internal bool disposed;

		// Token: 0x0400023A RID: 570
		private bool bounded;

		// Token: 0x0400023B RID: 571
		private bool needRebind;

		// Token: 0x0400023C RID: 572
		internal static Hashtable handleTable = new Hashtable();

		// Token: 0x0400023D RID: 573
		internal static object objectLock = new object();

		// Token: 0x0400023E RID: 574
		private GetLdapResponseCallback fd;

		// Token: 0x0400023F RID: 575
		private static Hashtable asyncResultTable;

		// Token: 0x04000240 RID: 576
		private static LdapPartialResultsProcessor partialResultsProcessor;

		// Token: 0x04000241 RID: 577
		private static ManualResetEvent waitHandle;

		// Token: 0x04000242 RID: 578
		private static PartialResultsRetriever retriever;

		// Token: 0x04000243 RID: 579
		private bool setFQDNDone;

		// Token: 0x04000244 RID: 580
		internal bool automaticBind;

		// Token: 0x04000245 RID: 581
		internal bool needDispose;

		// Token: 0x04000246 RID: 582
		private bool connected;

		// Token: 0x04000247 RID: 583
		internal QUERYCLIENTCERT clientCertificateRoutine;

		// Token: 0x02000074 RID: 116
		internal enum LdapResult
		{
			// Token: 0x04000249 RID: 585
			LDAP_RES_SEARCH_RESULT = 101,
			// Token: 0x0400024A RID: 586
			LDAP_RES_SEARCH_ENTRY = 100,
			// Token: 0x0400024B RID: 587
			LDAP_RES_MODIFY = 103,
			// Token: 0x0400024C RID: 588
			LDAP_RES_ADD = 105,
			// Token: 0x0400024D RID: 589
			LDAP_RES_DELETE = 107,
			// Token: 0x0400024E RID: 590
			LDAP_RES_MODRDN = 109,
			// Token: 0x0400024F RID: 591
			LDAP_RES_COMPARE = 111,
			// Token: 0x04000250 RID: 592
			LDAP_RES_REFERRAL = 115,
			// Token: 0x04000251 RID: 593
			LDAP_RES_EXTENDED = 120
		}
	}
}
