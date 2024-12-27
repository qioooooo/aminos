using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x02000022 RID: 34
	[ToolboxItem(false)]
	public class ManagementObjectSearcher : Component
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00007836 File Offset: 0x00006836
		public ManagementObjectSearcher()
			: this(null, null, null)
		{
		}

		// Token: 0x06000115 RID: 277 RVA: 0x00007841 File Offset: 0x00006841
		public ManagementObjectSearcher(string queryString)
			: this(null, new ObjectQuery(queryString), null)
		{
		}

		// Token: 0x06000116 RID: 278 RVA: 0x00007851 File Offset: 0x00006851
		public ManagementObjectSearcher(ObjectQuery query)
			: this(null, query, null)
		{
		}

		// Token: 0x06000117 RID: 279 RVA: 0x0000785C File Offset: 0x0000685C
		public ManagementObjectSearcher(string scope, string queryString)
			: this(new ManagementScope(scope), new ObjectQuery(queryString), null)
		{
		}

		// Token: 0x06000118 RID: 280 RVA: 0x00007871 File Offset: 0x00006871
		public ManagementObjectSearcher(ManagementScope scope, ObjectQuery query)
			: this(scope, query, null)
		{
		}

		// Token: 0x06000119 RID: 281 RVA: 0x0000787C File Offset: 0x0000687C
		public ManagementObjectSearcher(string scope, string queryString, EnumerationOptions options)
			: this(new ManagementScope(scope), new ObjectQuery(queryString), options)
		{
		}

		// Token: 0x0600011A RID: 282 RVA: 0x00007894 File Offset: 0x00006894
		public ManagementObjectSearcher(ManagementScope scope, ObjectQuery query, EnumerationOptions options)
		{
			this.scope = ManagementScope._Clone(scope);
			if (query != null)
			{
				this.query = (ObjectQuery)query.Clone();
			}
			else
			{
				this.query = new ObjectQuery();
			}
			if (options != null)
			{
				this.options = (EnumerationOptions)options.Clone();
				return;
			}
			this.options = new EnumerationOptions();
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x0600011B RID: 283 RVA: 0x000078F4 File Offset: 0x000068F4
		// (set) Token: 0x0600011C RID: 284 RVA: 0x000078FC File Offset: 0x000068FC
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
					this.scope = value.Clone();
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x0600011D RID: 285 RVA: 0x00007918 File Offset: 0x00006918
		// (set) Token: 0x0600011E RID: 286 RVA: 0x00007920 File Offset: 0x00006920
		public ObjectQuery Query
		{
			get
			{
				return this.query;
			}
			set
			{
				if (value != null)
				{
					this.query = (ObjectQuery)value.Clone();
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x0600011F RID: 287 RVA: 0x00007941 File Offset: 0x00006941
		// (set) Token: 0x06000120 RID: 288 RVA: 0x00007949 File Offset: 0x00006949
		public EnumerationOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				if (value != null)
				{
					this.options = (EnumerationOptions)value.Clone();
					return;
				}
				throw new ArgumentNullException("value");
			}
		}

		// Token: 0x06000121 RID: 289 RVA: 0x0000796C File Offset: 0x0000696C
		public ManagementObjectCollection Get()
		{
			this.Initialize();
			IEnumWbemClassObject enumWbemClassObject = null;
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			EnumerationOptions enumerationOptions = (EnumerationOptions)this.options.Clone();
			int num = 0;
			try
			{
				if (this.query.GetType() == typeof(SelectQuery) && ((SelectQuery)this.query).Condition == null && ((SelectQuery)this.query).SelectedProperties == null && this.options.EnumerateDeep)
				{
					enumerationOptions.EnsureLocatable = false;
					enumerationOptions.PrototypeOnly = false;
					if (!((SelectQuery)this.query).IsSchemaQuery)
					{
						num = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).CreateInstanceEnum_(((SelectQuery)this.query).ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbemClassObject);
					}
					else
					{
						num = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).CreateClassEnum_(((SelectQuery)this.query).ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbemClassObject);
					}
				}
				else
				{
					enumerationOptions.EnumerateDeep = true;
					num = this.scope.GetSecurityIWbemServicesHandler(this.scope.GetIWbemServices()).ExecQuery_(this.query.QueryLanguage, this.query.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), ref enumWbemClassObject);
				}
			}
			catch (COMException ex)
			{
				ManagementException.ThrowWithExtendedInfo(ex);
			}
			finally
			{
				securityHandler.Reset();
			}
			if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
			}
			else if (((long)num & (long)((ulong)(-2147483648))) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
			return new ManagementObjectCollection(this.scope, this.options, enumWbemClassObject);
		}

		// Token: 0x06000122 RID: 290 RVA: 0x00007B68 File Offset: 0x00006B68
		public void Get(ManagementOperationObserver watcher)
		{
			if (watcher == null)
			{
				throw new ArgumentNullException("watcher");
			}
			this.Initialize();
			IWbemServices iwbemServices = this.scope.GetIWbemServices();
			EnumerationOptions enumerationOptions = (EnumerationOptions)this.options.Clone();
			enumerationOptions.ReturnImmediately = false;
			if (watcher.HaveListenersForProgress)
			{
				enumerationOptions.SendStatus = true;
			}
			WmiEventSink newSink = watcher.GetNewSink(this.scope, enumerationOptions.Context);
			SecurityHandler securityHandler = this.scope.GetSecurityHandler();
			int num = 0;
			try
			{
				if (this.query.GetType() == typeof(SelectQuery) && ((SelectQuery)this.query).Condition == null && ((SelectQuery)this.query).SelectedProperties == null && this.options.EnumerateDeep)
				{
					enumerationOptions.EnsureLocatable = false;
					enumerationOptions.PrototypeOnly = false;
					if (!((SelectQuery)this.query).IsSchemaQuery)
					{
						num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).CreateInstanceEnumAsync_(((SelectQuery)this.query).ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
					}
					else
					{
						num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).CreateClassEnumAsync_(((SelectQuery)this.query).ClassName, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
					}
				}
				else
				{
					enumerationOptions.EnumerateDeep = true;
					num = this.scope.GetSecurityIWbemServicesHandler(iwbemServices).ExecQueryAsync_(this.query.QueryLanguage, this.query.QueryString, enumerationOptions.Flags, enumerationOptions.GetContext(), newSink.Stub);
				}
			}
			catch (COMException ex)
			{
				watcher.RemoveSink(newSink);
				ManagementException.ThrowWithExtendedInfo(ex);
			}
			finally
			{
				securityHandler.Reset();
			}
			if (((long)num & (long)((ulong)(-4096))) == (long)((ulong)(-2147217408)))
			{
				ManagementException.ThrowWithExtendedInfo((ManagementStatus)num);
				return;
			}
			if (((long)num & (long)((ulong)(-2147483648))) != 0L)
			{
				Marshal.ThrowExceptionForHR(num, WmiNetUtilsHelper.GetErrorInfo_f());
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x00007D8C File Offset: 0x00006D8C
		private void Initialize()
		{
			if (this.query == null)
			{
				throw new InvalidOperationException();
			}
			lock (this)
			{
				if (this.scope == null)
				{
					this.scope = ManagementScope._Clone(null);
				}
			}
			lock (this.scope)
			{
				if (!this.scope.IsConnected)
				{
					this.scope.Initialize();
				}
			}
		}

		// Token: 0x04000117 RID: 279
		private ManagementScope scope;

		// Token: 0x04000118 RID: 280
		private ObjectQuery query;

		// Token: 0x04000119 RID: 281
		private EnumerationOptions options;
	}
}
