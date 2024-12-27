using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000018 RID: 24
	[ToolboxItem(false)]
	public class ManagementEventWatcher : Component
	{
		// Token: 0x060000BB RID: 187 RVA: 0x000061FE File Offset: 0x000051FE
		private void HandleIdentifierChange(object sender, IdentifierChangedEventArgs e)
		{
			this.Stop();
		}

		// Token: 0x060000BC RID: 188 RVA: 0x00006206 File Offset: 0x00005206
		public ManagementEventWatcher()
			: this(null, null, null)
		{
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00006211 File Offset: 0x00005211
		public ManagementEventWatcher(EventQuery query)
			: this(null, query, null)
		{
		}

		// Token: 0x060000BE RID: 190 RVA: 0x0000621C File Offset: 0x0000521C
		public ManagementEventWatcher(string query)
			: this(null, new EventQuery(query), null)
		{
		}

		// Token: 0x060000BF RID: 191 RVA: 0x0000622C File Offset: 0x0000522C
		public ManagementEventWatcher(ManagementScope scope, EventQuery query)
			: this(scope, query, null)
		{
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00006237 File Offset: 0x00005237
		public ManagementEventWatcher(string scope, string query)
			: this(new ManagementScope(scope), new EventQuery(query), null)
		{
		}

		// Token: 0x060000C1 RID: 193 RVA: 0x0000624C File Offset: 0x0000524C
		public ManagementEventWatcher(string scope, string query, EventWatcherOptions options)
			: this(new ManagementScope(scope), new EventQuery(query), options)
		{
		}

		// Token: 0x060000C2 RID: 194 RVA: 0x00006264 File Offset: 0x00005264
		public ManagementEventWatcher(ManagementScope scope, EventQuery query, EventWatcherOptions options)
		{
			if (scope != null)
			{
				this.scope = ManagementScope._Clone(scope, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
			}
			else
			{
				this.scope = ManagementScope._Clone(null, new IdentifierChangedEventHandler(this.HandleIdentifierChange));
			}
			if (query != null)
			{
				this.query = (EventQuery)query.Clone();
			}
			else
			{
				this.query = new EventQuery();
			}
			this.query.IdentifierChanged += this.HandleIdentifierChange;
			if (options != null)
			{
				this.options = (EventWatcherOptions)options.Clone();
			}
			else
			{
				this.options = new EventWatcherOptions();
			}
			this.options.IdentifierChanged += this.HandleIdentifierChange;
			this.enumWbem = null;
			this.cachedCount = 0U;
			this.cacheIndex = 0U;
			this.sink = null;
			this.delegateInvoker = new WmiDelegateInvoker(this);
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x00006344 File Offset: 0x00005344
		~ManagementEventWatcher()
		{
			this.Stop();
			if (this.scope != null)
			{
				this.scope.IdentifierChanged -= this.HandleIdentifierChange;
			}
			if (this.options != null)
			{
				this.options.IdentifierChanged -= this.HandleIdentifierChange;
			}
			if (this.query != null)
			{
				this.query.IdentifierChanged -= this.HandleIdentifierChange;
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060000C4 RID: 196 RVA: 0x000063D0 File Offset: 0x000053D0
		// (remove) Token: 0x060000C5 RID: 197 RVA: 0x000063E9 File Offset: 0x000053E9
		public event EventArrivedEventHandler EventArrived;

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x060000C6 RID: 198 RVA: 0x00006402 File Offset: 0x00005402
		// (remove) Token: 0x060000C7 RID: 199 RVA: 0x0000641B File Offset: 0x0000541B
		public event StoppedEventHandler Stopped;

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x060000C8 RID: 200 RVA: 0x00006434 File Offset: 0x00005434
		// (set) Token: 0x060000C9 RID: 201 RVA: 0x0000643C File Offset: 0x0000543C
		public ManagementScope Scope
		{
			get
			{
				return this.scope;
			}
			set
			{
				if (value != null)
				{
					ManagementScope managementScope = this.scope;
					this.scope = value.Clone();
					if (managementScope != null)
					{
						managementScope.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.scope.IdentifierChanged += this.HandleIdentifierChange;
					this.HandleIdentifierChange(this, null);
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x060000CA RID: 202 RVA: 0x0000649E File Offset: 0x0000549E
		// (set) Token: 0x060000CB RID: 203 RVA: 0x000064A8 File Offset: 0x000054A8
		public EventQuery Query
		{
			get
			{
				return this.query;
			}
			set
			{
				if (value != null)
				{
					ManagementQuery managementQuery = this.query;
					this.query = (EventQuery)value.Clone();
					if (managementQuery != null)
					{
						managementQuery.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.query.IdentifierChanged += this.HandleIdentifierChange;
					this.HandleIdentifierChange(this, null);
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x060000CC RID: 204 RVA: 0x0000650F File Offset: 0x0000550F
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00006518 File Offset: 0x00005518
		public EventWatcherOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				if (value != null)
				{
					EventWatcherOptions eventWatcherOptions = this.options;
					this.options = (EventWatcherOptions)value.Clone();
					if (eventWatcherOptions != null)
					{
						eventWatcherOptions.IdentifierChanged -= this.HandleIdentifierChange;
					}
					this.cachedObjects = new IWbemClassObjectFreeThreaded[this.options.BlockSize];
					this.options.IdentifierChanged += this.HandleIdentifierChange;
					this.HandleIdentifierChange(this, null);
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00006598 File Offset: 0x00005598
		public ManagementBaseObject WaitForNextEvent()
		{
			ManagementBaseObject managementBaseObject = null;
			this.Initialize();
			lock (this)
			{
				SecurityHandler securityHandler = this.Scope.GetSecurityHandler();
				int num = 0;
				try
				{
					if (this.enumWbem == null)
					{
						num = this.scope.GetSecurityIWbemServicesHandler(this.Scope.GetIWbemServices()).ExecNotificationQuery_(this.query.QueryLanguage, this.query.QueryString, this.options.Flags, this.options.GetContext(), ref this.enumWbem);
					}
					if (num >= 0)
					{
						if (this.cachedCount - this.cacheIndex == 0U)
						{
							IWbemClassObject_DoNotMarshal[] array = new IWbemClassObject_DoNotMarshal[this.options.BlockSize];
							int num2 = ((ManagementOptions.InfiniteTimeout == this.options.Timeout) ? (-1) : ((int)this.options.Timeout.TotalMilliseconds));
							num = this.scope.GetSecuredIEnumWbemClassObjectHandler(this.enumWbem).Next_(num2, (uint)this.options.BlockSize, array, ref this.cachedCount);
							this.cacheIndex = 0U;
							if (num >= 0)
							{
								if (this.cachedCount == 0U)
								{
									ManagementException.ThrowWithExtendedInfo(ManagementStatus.Timedout);
								}
								int num3 = 0;
								while ((long)num3 < (long)((ulong)this.cachedCount))
								{
									this.cachedObjects[num3] = new IWbemClassObjectFreeThreaded(Marshal.GetIUnknownForObject(array[num3]));
									num3++;
								}
							}
						}
						if (num >= 0)
						{
							managementBaseObject = new ManagementBaseObject(this.cachedObjects[(int)((UIntPtr)this.cacheIndex)]);
							this.cacheIndex += 1U;
						}
					}
				}
				finally
				{
					securityHandler.Reset();
				}
				if (num < 0)
				{
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			return managementBaseObject;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00006788 File Offset: 0x00005788
		public void Start()
		{
			this.Initialize();
			this.Stop();
			SecurityHandler securityHandler = this.Scope.GetSecurityHandler();
			IWbemServices iwbemServices = this.scope.GetIWbemServices();
			try
			{
				this.sink = new SinkForEventQuery(this, this.options.Context, iwbemServices);
				if (this.sink.Status < 0)
				{
					Marshal.ThrowExceptionForHR(this.sink.Status, WmiNetUtilsHelper.GetErrorInfo_f());
				}
				int num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).ExecNotificationQueryAsync_(this.query.QueryLanguage, this.query.QueryString, 0, this.options.GetContext(), this.sink.Stub);
				if (num < 0)
				{
					if (this.sink != null)
					{
						this.sink.ReleaseStub();
						this.sink = null;
					}
					if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
					{
						ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
					}
					else
					{
						Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
					}
				}
			}
			finally
			{
				securityHandler.Reset();
			}
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00006898 File Offset: 0x00005898
		public void Stop()
		{
			if (this.enumWbem != null)
			{
				Marshal.ReleaseComObject(this.enumWbem);
				this.enumWbem = null;
				this.FireStopped(new StoppedEventArgs(this.options.Context, 262150));
			}
			if (this.sink != null)
			{
				this.sink.Cancel();
				this.sink = null;
			}
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x000068F8 File Offset: 0x000058F8
		private void Initialize()
		{
			if (this.query == null)
			{
				throw new InvalidOperationException();
			}
			if (this.options == null)
			{
				this.Options = new EventWatcherOptions();
			}
			lock (this)
			{
				if (this.scope == null)
				{
					this.Scope = new ManagementScope();
				}
				if (this.cachedObjects == null)
				{
					this.cachedObjects = new IWbemClassObjectFreeThreaded[this.options.BlockSize];
				}
			}
			lock (this.scope)
			{
				this.scope.Initialize();
			}
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x000069A8 File Offset: 0x000059A8
		internal void FireStopped(StoppedEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.Stopped, args);
			}
			catch
			{
			}
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x000069DC File Offset: 0x000059DC
		internal void FireEventArrived(EventArrivedEventArgs args)
		{
			try
			{
				this.delegateInvoker.FireEventToDelegates(this.EventArrived, args);
			}
			catch
			{
			}
		}

		// Token: 0x04000089 RID: 137
		private ManagementScope scope;

		// Token: 0x0400008A RID: 138
		private EventQuery query;

		// Token: 0x0400008B RID: 139
		private EventWatcherOptions options;

		// Token: 0x0400008C RID: 140
		private IEnumWbemClassObject enumWbem;

		// Token: 0x0400008D RID: 141
		private IWbemClassObjectFreeThreaded[] cachedObjects;

		// Token: 0x0400008E RID: 142
		private uint cachedCount;

		// Token: 0x0400008F RID: 143
		private uint cacheIndex;

		// Token: 0x04000090 RID: 144
		private SinkForEventQuery sink;

		// Token: 0x04000091 RID: 145
		private WmiDelegateInvoker delegateInvoker;
	}
}
