using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.Util;

namespace System.Web.SessionState
{
	// Token: 0x02000364 RID: 868
	internal sealed class OutOfProcSessionStateStore : SessionStateStoreProviderBase
	{
		// Token: 0x06002A05 RID: 10757 RVA: 0x000BB58D File Offset: 0x000BA58D
		internal override void Initialize(string name, NameValueCollection config, IPartitionResolver partitionResolver)
		{
			this._partitionResolver = partitionResolver;
			this.Initialize(name, config);
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000BB5A0 File Offset: 0x000BA5A0
		public override void Initialize(string name, NameValueCollection config)
		{
			if (string.IsNullOrEmpty(name))
			{
				name = "State Server Session State Provider";
			}
			base.Initialize(name, config);
			if (!OutOfProcSessionStateStore.s_oneTimeInited)
			{
				OutOfProcSessionStateStore.s_lock.AcquireWriterLock();
				try
				{
					if (!OutOfProcSessionStateStore.s_oneTimeInited)
					{
						this.OneTimeInit();
					}
				}
				finally
				{
					OutOfProcSessionStateStore.s_lock.ReleaseWriterLock();
				}
			}
			if (!OutOfProcSessionStateStore.s_usePartition)
			{
				this._partitionInfo = OutOfProcSessionStateStore.s_singlePartitionInfo;
			}
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000BB614 File Offset: 0x000BA614
		private void OneTimeInit()
		{
			SessionStateSection sessionState = RuntimeConfig.GetAppConfig().SessionState;
			OutOfProcSessionStateStore.s_configPartitionResolverType = sessionState.PartitionResolverType;
			OutOfProcSessionStateStore.s_configStateConnectionString = sessionState.StateConnectionString;
			OutOfProcSessionStateStore.s_configStateConnectionStringFileName = sessionState.ElementInformation.Properties["stateConnectionString"].Source;
			OutOfProcSessionStateStore.s_configStateConnectionStringLineNumber = sessionState.ElementInformation.Properties["stateConnectionString"].LineNumber;
			if (this._partitionResolver == null)
			{
				string stateConnectionString = sessionState.StateConnectionString;
				SessionStateModule.ReadConnectionString(sessionState, ref stateConnectionString, "stateConnectionString");
				OutOfProcSessionStateStore.s_singlePartitionInfo = (OutOfProcSessionStateStore.StateServerPartitionInfo)this.CreatePartitionInfo(stateConnectionString);
			}
			else
			{
				OutOfProcSessionStateStore.s_usePartition = true;
				OutOfProcSessionStateStore.s_partitionManager = new PartitionManager(new CreatePartitionInfo(this.CreatePartitionInfo));
			}
			OutOfProcSessionStateStore.s_networkTimeout = (int)sessionState.StateNetworkTimeout.TotalSeconds;
			string appDomainAppIdInternal = HttpRuntime.AppDomainAppIdInternal;
			string text = MachineKeySection.HashAndBase64EncodeString(appDomainAppIdInternal);
			if (appDomainAppIdInternal.StartsWith("/", StringComparison.Ordinal))
			{
				OutOfProcSessionStateStore.s_uribase = appDomainAppIdInternal + "(" + text + ")/";
			}
			else
			{
				OutOfProcSessionStateStore.s_uribase = string.Concat(new string[] { "/", appDomainAppIdInternal, "(", text, ")/" });
			}
			OutOfProcSessionStateStore.s_onAppDomainUnload = new EventHandler(this.OnAppDomainUnload);
			Thread.GetDomain().DomainUnload += OutOfProcSessionStateStore.s_onAppDomainUnload;
			OutOfProcSessionStateStore.s_oneTimeInited = true;
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000BB772 File Offset: 0x000BA772
		private void OnAppDomainUnload(object unusedObject, EventArgs unusedEventArgs)
		{
			Thread.GetDomain().DomainUnload -= OutOfProcSessionStateStore.s_onAppDomainUnload;
			if (this._partitionResolver == null)
			{
				if (OutOfProcSessionStateStore.s_singlePartitionInfo != null)
				{
					OutOfProcSessionStateStore.s_singlePartitionInfo.Dispose();
					return;
				}
			}
			else if (OutOfProcSessionStateStore.s_partitionManager != null)
			{
				OutOfProcSessionStateStore.s_partitionManager.Dispose();
			}
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000BB7B0 File Offset: 0x000BA7B0
		internal IPartitionInfo CreatePartitionInfo(string stateConnectionString)
		{
			string text;
			int num;
			try
			{
				string[] array = stateConnectionString.Split(new char[] { '=' });
				if (array.Length != 2 || array[0] != "tcpip")
				{
					throw new ArgumentException("stateConnectionString");
				}
				array = array[1].Split(new char[] { ':' });
				if (array.Length != 2)
				{
					throw new ArgumentException("stateConnectionString");
				}
				text = array[0];
				num = (int)ushort.Parse(array[1], CultureInfo.InvariantCulture);
				for (int i = 0; i < text.Length; i++)
				{
					if (text[i] > '\u007f')
					{
						throw new ArgumentException("stateConnectionString");
					}
				}
			}
			catch
			{
				if (OutOfProcSessionStateStore.s_usePartition)
				{
					throw new HttpException(SR.GetString("Error_parsing_state_server_partition_resolver_string", new object[] { OutOfProcSessionStateStore.s_configPartitionResolverType }));
				}
				throw new ConfigurationErrorsException(SR.GetString("Invalid_value_for_sessionstate_stateConnectionString", new object[] { OutOfProcSessionStateStore.s_configStateConnectionString }), OutOfProcSessionStateStore.s_configStateConnectionStringFileName, OutOfProcSessionStateStore.s_configStateConnectionStringLineNumber);
			}
			int num2 = UnsafeNativeMethods.SessionNDConnectToService(text);
			if (num2 != 0)
			{
				throw OutOfProcSessionStateStore.CreateConnectionException(text, num, num2);
			}
			return new OutOfProcSessionStateStore.StateServerPartitionInfo(new ResourcePool(new TimeSpan(0, 0, 5), int.MaxValue), text, num);
		}

		// Token: 0x06002A0A RID: 10762 RVA: 0x000BB8F4 File Offset: 0x000BA8F4
		internal static HttpException CreateConnectionException(string server, int port, int hr)
		{
			if (OutOfProcSessionStateStore.s_usePartition)
			{
				return new HttpException(SR.GetString("Cant_make_session_request_partition_resolver", new object[]
				{
					OutOfProcSessionStateStore.s_configPartitionResolverType,
					server,
					port.ToString(CultureInfo.InvariantCulture)
				}), hr);
			}
			return new HttpException(SR.GetString("Cant_make_session_request"), hr);
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000BB94C File Offset: 0x000BA94C
		public override bool SetItemExpireCallback(SessionStateItemExpireCallback expireCallback)
		{
			return false;
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000BB94F File Offset: 0x000BA94F
		public override void Dispose()
		{
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000BB951 File Offset: 0x000BA951
		public override void InitializeRequest(HttpContext context)
		{
			if (OutOfProcSessionStateStore.s_usePartition)
			{
				this._partitionInfo = null;
			}
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000BB964 File Offset: 0x000BA964
		private void MakeRequest(UnsafeNativeMethods.StateProtocolVerb verb, string id, UnsafeNativeMethods.StateProtocolExclusive exclusiveAccess, int extraFlags, int timeout, int lockCookie, byte[] buf, int cb, int networkTimeout, out UnsafeNativeMethods.SessionNDMakeRequestResults results)
		{
			OutOfProcSessionStateStore.OutOfProcConnection outOfProcConnection = null;
			bool flag = false;
			SessionIDManager.CheckIdLength(id, true);
			if (this._partitionInfo == null)
			{
				this._partitionInfo = (OutOfProcSessionStateStore.StateServerPartitionInfo)OutOfProcSessionStateStore.s_partitionManager.GetPartition(this._partitionResolver, id);
				if (this._partitionInfo == null)
				{
					throw new HttpException(SR.GetString("Bad_partition_resolver_connection_string", new object[] { "PartitionManager" }));
				}
			}
			int num;
			try
			{
				outOfProcConnection = (OutOfProcSessionStateStore.OutOfProcConnection)this._partitionInfo.RetrieveResource();
				HandleRef handleRef;
				if (outOfProcConnection != null)
				{
					handleRef = new HandleRef(this, outOfProcConnection._socketHandle.Handle);
				}
				else
				{
					handleRef = new HandleRef(this, OutOfProcSessionStateStore.INVALID_SOCKET);
				}
				if (this._partitionInfo.StateServerVersion == -1)
				{
					flag = true;
				}
				string text = HttpUtility.UrlEncode(OutOfProcSessionStateStore.s_uribase + id);
				num = UnsafeNativeMethods.SessionNDMakeRequest(handleRef, this._partitionInfo.Server, this._partitionInfo.Port, networkTimeout, verb, text, exclusiveAccess, extraFlags, timeout, lockCookie, buf, cb, flag, out results);
				if (outOfProcConnection != null)
				{
					if (results.socket == OutOfProcSessionStateStore.INVALID_SOCKET)
					{
						outOfProcConnection.Detach();
						outOfProcConnection = null;
					}
					else if (results.socket != handleRef.Handle)
					{
						outOfProcConnection._socketHandle = new HandleRef(this, results.socket);
					}
				}
				else if (results.socket != OutOfProcSessionStateStore.INVALID_SOCKET)
				{
					outOfProcConnection = new OutOfProcSessionStateStore.OutOfProcConnection(results.socket);
				}
				if (outOfProcConnection != null)
				{
					this._partitionInfo.StoreResource(outOfProcConnection);
				}
			}
			catch
			{
				if (outOfProcConnection != null)
				{
					outOfProcConnection.Dispose();
				}
				throw;
			}
			if (num != 0)
			{
				HttpException ex = OutOfProcSessionStateStore.CreateConnectionException(this._partitionInfo.Server, this._partitionInfo.Port, num);
				string text2 = null;
				switch (results.lastPhase)
				{
				case 0:
					text2 = SR.GetString("State_Server_detailed_error_phase0");
					break;
				case 1:
					text2 = SR.GetString("State_Server_detailed_error_phase1");
					break;
				case 2:
					text2 = SR.GetString("State_Server_detailed_error_phase2");
					break;
				case 3:
					text2 = SR.GetString("State_Server_detailed_error_phase3");
					break;
				}
				WebBaseEvent.RaiseSystemEvent(SR.GetString("State_Server_detailed_error", new object[]
				{
					text2,
					"0x" + num.ToString("X08", CultureInfo.InvariantCulture),
					cb.ToString(CultureInfo.InvariantCulture)
				}), this, 3009, 50016, ex);
				throw ex;
			}
			if (results.httpStatus != 400)
			{
				if (flag)
				{
					this._partitionInfo.StateServerVersion = results.stateServerMajVer;
					if (this._partitionInfo.StateServerVersion < OutOfProcSessionStateStore.WHIDBEY_MAJOR_VERSION)
					{
						if (OutOfProcSessionStateStore.s_usePartition)
						{
							throw new HttpException(SR.GetString("Need_v2_State_Server_partition_resolver", new object[]
							{
								OutOfProcSessionStateStore.s_configPartitionResolverType,
								this._partitionInfo.Server,
								this._partitionInfo.Port.ToString(CultureInfo.InvariantCulture)
							}));
						}
						throw new HttpException(SR.GetString("Need_v2_State_Server"));
					}
				}
				return;
			}
			if (OutOfProcSessionStateStore.s_usePartition)
			{
				throw new HttpException(SR.GetString("Bad_state_server_request_partition_resolver", new object[]
				{
					OutOfProcSessionStateStore.s_configPartitionResolverType,
					this._partitionInfo.Server,
					this._partitionInfo.Port.ToString(CultureInfo.InvariantCulture)
				}));
			}
			throw new HttpException(SR.GetString("Bad_state_server_request"));
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x000BBCD8 File Offset: 0x000BACD8
		internal unsafe SessionStateStoreData DoGet(HttpContext context, string id, UnsafeNativeMethods.StateProtocolExclusive exclusiveAccess, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			SessionStateStoreData sessionStateStoreData = null;
			locked = false;
			lockId = null;
			lockAge = TimeSpan.Zero;
			actionFlags = SessionStateActions.None;
			UnsafeNativeMethods.SessionNDMakeRequestResults sessionNDMakeRequestResults;
			sessionNDMakeRequestResults.content = IntPtr.Zero;
			try
			{
				this.MakeRequest(UnsafeNativeMethods.StateProtocolVerb.GET, id, exclusiveAccess, 0, 0, 0, null, 0, OutOfProcSessionStateStore.s_networkTimeout, out sessionNDMakeRequestResults);
				int httpStatus = sessionNDMakeRequestResults.httpStatus;
				if (httpStatus != 200)
				{
					if (httpStatus == 423)
					{
						if (0 <= sessionNDMakeRequestResults.lockAge)
						{
							if (sessionNDMakeRequestResults.lockAge < 31536000)
							{
								lockAge = new TimeSpan(0, 0, sessionNDMakeRequestResults.lockAge);
							}
							else
							{
								lockAge = TimeSpan.Zero;
							}
						}
						else
						{
							DateTime now = DateTime.Now;
							if (0L < sessionNDMakeRequestResults.lockDate && sessionNDMakeRequestResults.lockDate < now.Ticks)
							{
								lockAge = now - new DateTime(sessionNDMakeRequestResults.lockDate);
							}
							else
							{
								lockAge = TimeSpan.Zero;
							}
						}
						locked = true;
						lockId = sessionNDMakeRequestResults.lockCookie;
					}
				}
				else
				{
					int contentLength = sessionNDMakeRequestResults.contentLength;
					if (contentLength > 0)
					{
						UnmanagedMemoryStream unmanagedMemoryStream = new UnmanagedMemoryStream((byte*)(void*)sessionNDMakeRequestResults.content, (long)contentLength);
						sessionStateStoreData = SessionStateUtility.Deserialize(context, unmanagedMemoryStream);
						unmanagedMemoryStream.Close();
						lockId = sessionNDMakeRequestResults.lockCookie;
						actionFlags = (SessionStateActions)sessionNDMakeRequestResults.actionFlags;
					}
				}
			}
			finally
			{
				if (sessionNDMakeRequestResults.content != IntPtr.Zero)
				{
					UnsafeNativeMethods.SessionNDFreeBody(new HandleRef(this, sessionNDMakeRequestResults.content));
				}
			}
			return sessionStateStoreData;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000BBE70 File Offset: 0x000BAE70
		public override SessionStateStoreData GetItem(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			return this.DoGet(context, id, UnsafeNativeMethods.StateProtocolExclusive.NONE, out locked, out lockAge, out lockId, out actionFlags);
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000BBE82 File Offset: 0x000BAE82
		public override SessionStateStoreData GetItemExclusive(HttpContext context, string id, out bool locked, out TimeSpan lockAge, out object lockId, out SessionStateActions actionFlags)
		{
			return this.DoGet(context, id, UnsafeNativeMethods.StateProtocolExclusive.ACQUIRE, out locked, out lockAge, out lockId, out actionFlags);
		}

		// Token: 0x06002A12 RID: 10770 RVA: 0x000BBE94 File Offset: 0x000BAE94
		public override void ReleaseItemExclusive(HttpContext context, string id, object lockId)
		{
			int num = (int)lockId;
			UnsafeNativeMethods.SessionNDMakeRequestResults sessionNDMakeRequestResults;
			this.MakeRequest(UnsafeNativeMethods.StateProtocolVerb.GET, id, UnsafeNativeMethods.StateProtocolExclusive.RELEASE, 0, 0, num, null, 0, OutOfProcSessionStateStore.s_networkTimeout, out sessionNDMakeRequestResults);
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000BBEC0 File Offset: 0x000BAEC0
		public override void SetAndReleaseItemExclusive(HttpContext context, string id, SessionStateStoreData item, object lockId, bool newItem)
		{
			byte[] array;
			int num;
			try
			{
				SessionStateUtility.SerializeStoreData(item, 0, out array, out num);
			}
			catch
			{
				if (!newItem)
				{
					this.ReleaseItemExclusive(context, id, lockId);
				}
				throw;
			}
			int num2;
			if (lockId == null)
			{
				num2 = 0;
			}
			else
			{
				num2 = (int)lockId;
			}
			UnsafeNativeMethods.SessionNDMakeRequestResults sessionNDMakeRequestResults;
			this.MakeRequest(UnsafeNativeMethods.StateProtocolVerb.PUT, id, UnsafeNativeMethods.StateProtocolExclusive.NONE, 0, item.Timeout, num2, array, num, OutOfProcSessionStateStore.s_networkTimeout, out sessionNDMakeRequestResults);
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000BBF28 File Offset: 0x000BAF28
		public override void RemoveItem(HttpContext context, string id, object lockId, SessionStateStoreData item)
		{
			int num = (int)lockId;
			UnsafeNativeMethods.SessionNDMakeRequestResults sessionNDMakeRequestResults;
			this.MakeRequest(UnsafeNativeMethods.StateProtocolVerb.DELETE, id, UnsafeNativeMethods.StateProtocolExclusive.NONE, 0, 0, num, null, 0, OutOfProcSessionStateStore.s_networkTimeout, out sessionNDMakeRequestResults);
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000BBF54 File Offset: 0x000BAF54
		public override void ResetItemTimeout(HttpContext context, string id)
		{
			UnsafeNativeMethods.SessionNDMakeRequestResults sessionNDMakeRequestResults;
			this.MakeRequest(UnsafeNativeMethods.StateProtocolVerb.HEAD, id, UnsafeNativeMethods.StateProtocolExclusive.NONE, 0, 0, 0, null, 0, OutOfProcSessionStateStore.s_networkTimeout, out sessionNDMakeRequestResults);
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000BBF76 File Offset: 0x000BAF76
		public override SessionStateStoreData CreateNewStoreData(HttpContext context, int timeout)
		{
			return SessionStateUtility.CreateLegitStoreData(context, null, null, timeout);
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000BBF84 File Offset: 0x000BAF84
		public override void CreateUninitializedItem(HttpContext context, string id, int timeout)
		{
			byte[] array;
			int num;
			SessionStateUtility.SerializeStoreData(this.CreateNewStoreData(context, timeout), 0, out array, out num);
			UnsafeNativeMethods.SessionNDMakeRequestResults sessionNDMakeRequestResults;
			this.MakeRequest(UnsafeNativeMethods.StateProtocolVerb.PUT, id, UnsafeNativeMethods.StateProtocolExclusive.NONE, 1, timeout, 0, array, num, OutOfProcSessionStateStore.s_networkTimeout, out sessionNDMakeRequestResults);
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000BBFB8 File Offset: 0x000BAFB8
		public override void EndRequest(HttpContext context)
		{
		}

		// Token: 0x04001F3A RID: 7994
		internal const int STATE_NETWORK_TIMEOUT_DEFAULT = 10;

		// Token: 0x04001F3B RID: 7995
		internal static readonly IntPtr INVALID_SOCKET = UnsafeNativeMethods.INVALID_HANDLE_VALUE;

		// Token: 0x04001F3C RID: 7996
		internal static readonly int WHIDBEY_MAJOR_VERSION = 2;

		// Token: 0x04001F3D RID: 7997
		private static string s_uribase;

		// Token: 0x04001F3E RID: 7998
		private static int s_networkTimeout;

		// Token: 0x04001F3F RID: 7999
		private static ReadWriteSpinLock s_lock;

		// Token: 0x04001F40 RID: 8000
		private static bool s_oneTimeInited;

		// Token: 0x04001F41 RID: 8001
		private static OutOfProcSessionStateStore.StateServerPartitionInfo s_singlePartitionInfo;

		// Token: 0x04001F42 RID: 8002
		private static PartitionManager s_partitionManager;

		// Token: 0x04001F43 RID: 8003
		private static bool s_usePartition;

		// Token: 0x04001F44 RID: 8004
		private static EventHandler s_onAppDomainUnload;

		// Token: 0x04001F45 RID: 8005
		private static string s_configPartitionResolverType;

		// Token: 0x04001F46 RID: 8006
		private static string s_configStateConnectionString;

		// Token: 0x04001F47 RID: 8007
		private static string s_configStateConnectionStringFileName;

		// Token: 0x04001F48 RID: 8008
		private static int s_configStateConnectionStringLineNumber;

		// Token: 0x04001F49 RID: 8009
		private IPartitionResolver _partitionResolver;

		// Token: 0x04001F4A RID: 8010
		private OutOfProcSessionStateStore.StateServerPartitionInfo _partitionInfo;

		// Token: 0x02000365 RID: 869
		private class StateServerPartitionInfo : PartitionInfo
		{
			// Token: 0x06002A1B RID: 10779 RVA: 0x000BBFD4 File Offset: 0x000BAFD4
			internal StateServerPartitionInfo(ResourcePool rpool, string server, int port)
				: base(rpool)
			{
				this._server = server;
				this._port = port;
				this._stateServerVersion = -1;
			}

			// Token: 0x170008EA RID: 2282
			// (get) Token: 0x06002A1C RID: 10780 RVA: 0x000BBFF2 File Offset: 0x000BAFF2
			internal string Server
			{
				get
				{
					return this._server;
				}
			}

			// Token: 0x170008EB RID: 2283
			// (get) Token: 0x06002A1D RID: 10781 RVA: 0x000BBFFA File Offset: 0x000BAFFA
			internal int Port
			{
				get
				{
					return this._port;
				}
			}

			// Token: 0x170008EC RID: 2284
			// (get) Token: 0x06002A1E RID: 10782 RVA: 0x000BC002 File Offset: 0x000BB002
			// (set) Token: 0x06002A1F RID: 10783 RVA: 0x000BC00A File Offset: 0x000BB00A
			internal int StateServerVersion
			{
				get
				{
					return this._stateServerVersion;
				}
				set
				{
					this._stateServerVersion = value;
				}
			}

			// Token: 0x170008ED RID: 2285
			// (get) Token: 0x06002A20 RID: 10784 RVA: 0x000BC013 File Offset: 0x000BB013
			protected override string TracingPartitionString
			{
				get
				{
					return this.Server + ":" + this.Port;
				}
			}

			// Token: 0x04001F4B RID: 8011
			private string _server;

			// Token: 0x04001F4C RID: 8012
			private int _port;

			// Token: 0x04001F4D RID: 8013
			private int _stateServerVersion;
		}

		// Token: 0x02000366 RID: 870
		private class OutOfProcConnection : IDisposable
		{
			// Token: 0x06002A21 RID: 10785 RVA: 0x000BC030 File Offset: 0x000BB030
			internal OutOfProcConnection(IntPtr socket)
			{
				this._socketHandle = new HandleRef(this, socket);
				PerfCounters.IncrementCounter(AppPerfCounter.SESSION_STATE_SERVER_CONNECTIONS);
			}

			// Token: 0x06002A22 RID: 10786 RVA: 0x000BC04C File Offset: 0x000BB04C
			~OutOfProcConnection()
			{
				this.Dispose(false);
			}

			// Token: 0x06002A23 RID: 10787 RVA: 0x000BC07C File Offset: 0x000BB07C
			public void Dispose()
			{
				this.Dispose(true);
				GC.SuppressFinalize(this);
			}

			// Token: 0x06002A24 RID: 10788 RVA: 0x000BC08B File Offset: 0x000BB08B
			private void Dispose(bool dummy)
			{
				if (this._socketHandle.Handle != OutOfProcSessionStateStore.INVALID_SOCKET)
				{
					UnsafeNativeMethods.SessionNDCloseConnection(this._socketHandle);
					this._socketHandle = new HandleRef(this, OutOfProcSessionStateStore.INVALID_SOCKET);
					PerfCounters.DecrementCounter(AppPerfCounter.SESSION_STATE_SERVER_CONNECTIONS);
				}
			}

			// Token: 0x06002A25 RID: 10789 RVA: 0x000BC0C7 File Offset: 0x000BB0C7
			internal void Detach()
			{
				this._socketHandle = new HandleRef(this, OutOfProcSessionStateStore.INVALID_SOCKET);
			}

			// Token: 0x04001F4E RID: 8014
			internal HandleRef _socketHandle;
		}
	}
}
