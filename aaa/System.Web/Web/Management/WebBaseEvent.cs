using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Runtime.Remoting.Messaging;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002E2 RID: 738
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WebBaseEvent
	{
		// Token: 0x06002538 RID: 9528 RVA: 0x0009FE2C File Offset: 0x0009EE2C
		static WebBaseEvent()
		{
			for (int i = 0; i < WebBaseEvent.s_eventCodeToSystemEventTypeMappings.GetLength(0); i++)
			{
				for (int j = 0; j < WebBaseEvent.s_eventCodeToSystemEventTypeMappings.GetLength(1); j++)
				{
					WebBaseEvent.s_eventCodeToSystemEventTypeMappings[i, j] = WebBaseEvent.SystemEventType.Unknown;
				}
			}
			for (int k = 0; k < WebBaseEvent.s_eventCodeOccurrence.GetLength(0); k++)
			{
				for (int l = 0; l < WebBaseEvent.s_eventCodeOccurrence.GetLength(1); l++)
				{
					WebBaseEvent.s_eventCodeOccurrence[k, l] = 0L;
				}
			}
		}

		// Token: 0x06002539 RID: 9529 RVA: 0x0009FF00 File Offset: 0x0009EF00
		private void Init(string message, object eventSource, int eventCode, int eventDetailCode)
		{
			if (eventCode < 0)
			{
				throw new ArgumentOutOfRangeException("eventCode", SR.GetString("Invalid_eventCode_error"));
			}
			if (eventDetailCode < 0)
			{
				throw new ArgumentOutOfRangeException("eventDetailCode", SR.GetString("Invalid_eventDetailCode_error"));
			}
			this._code = eventCode;
			this._detailCode = eventDetailCode;
			this._source = eventSource;
			this._eventTimeUtc = DateTime.UtcNow;
			this._message = message;
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x0009FF68 File Offset: 0x0009EF68
		protected internal WebBaseEvent(string message, object eventSource, int eventCode)
		{
			this.Init(message, eventSource, eventCode, 0);
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x0009FF85 File Offset: 0x0009EF85
		protected internal WebBaseEvent(string message, object eventSource, int eventCode, int eventDetailCode)
		{
			this.Init(message, eventSource, eventCode, eventDetailCode);
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x0009FFA3 File Offset: 0x0009EFA3
		internal WebBaseEvent()
		{
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x0600253D RID: 9533 RVA: 0x0009FFB6 File Offset: 0x0009EFB6
		internal bool IsSystemEvent
		{
			get
			{
				return this._code < 100000;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x0600253E RID: 9534 RVA: 0x0009FFC5 File Offset: 0x0009EFC5
		public DateTime EventTime
		{
			get
			{
				return this._eventTimeUtc.ToLocalTime();
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x0600253F RID: 9535 RVA: 0x0009FFD2 File Offset: 0x0009EFD2
		public DateTime EventTimeUtc
		{
			get
			{
				return this._eventTimeUtc;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x06002540 RID: 9536 RVA: 0x0009FFDA File Offset: 0x0009EFDA
		public string Message
		{
			get
			{
				return this._message;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x06002541 RID: 9537 RVA: 0x0009FFE2 File Offset: 0x0009EFE2
		public object EventSource
		{
			get
			{
				return this._source;
			}
		}

		// Token: 0x170007BB RID: 1979
		// (get) Token: 0x06002542 RID: 9538 RVA: 0x0009FFEA File Offset: 0x0009EFEA
		public long EventSequence
		{
			get
			{
				return this._sequenceNumber;
			}
		}

		// Token: 0x170007BC RID: 1980
		// (get) Token: 0x06002543 RID: 9539 RVA: 0x0009FFF2 File Offset: 0x0009EFF2
		public long EventOccurrence
		{
			get
			{
				return this._occurrenceNumber;
			}
		}

		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x0009FFFA File Offset: 0x0009EFFA
		public int EventCode
		{
			get
			{
				return this._code;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x06002545 RID: 9541 RVA: 0x000A0002 File Offset: 0x0009F002
		public int EventDetailCode
		{
			get
			{
				return this._detailCode;
			}
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x000A000C File Offset: 0x0009F00C
		public Guid EventID
		{
			get
			{
				if (this._id == Guid.Empty)
				{
					lock (this)
					{
						if (this._id == Guid.Empty)
						{
							this._id = Guid.NewGuid();
						}
					}
				}
				return this._id;
			}
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x06002547 RID: 9543 RVA: 0x000A0070 File Offset: 0x0009F070
		public static WebApplicationInformation ApplicationInformation
		{
			get
			{
				return WebBaseEvent.s_applicationInfo;
			}
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x000A0078 File Offset: 0x0009F078
		internal virtual void FormatToString(WebEventFormatter formatter, bool includeAppInfo)
		{
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_code", this.EventCode.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_message", this.Message));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_time", this.EventTime.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_time_Utc", this.EventTimeUtc.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_id", this.EventID.ToString("N", CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_sequence", this.EventSequence.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_occurrence", this.EventOccurrence.ToString(CultureInfo.InstalledUICulture)));
			formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_detail_code", this.EventDetailCode.ToString(CultureInfo.InstalledUICulture)));
			if (includeAppInfo)
			{
				formatter.AppendLine(string.Empty);
				formatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_application_information"));
				formatter.IndentationLevel++;
				WebBaseEvent.ApplicationInformation.FormatToString(formatter);
				formatter.IndentationLevel--;
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x000A01DD File Offset: 0x0009F1DD
		public override string ToString()
		{
			return this.ToString(true, true);
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x000A01E8 File Offset: 0x0009F1E8
		public virtual string ToString(bool includeAppInfo, bool includeCustomEventDetails)
		{
			WebEventFormatter webEventFormatter = new WebEventFormatter();
			this.FormatToString(webEventFormatter, includeAppInfo);
			if (!this.IsSystemEvent && includeCustomEventDetails)
			{
				webEventFormatter.AppendLine(string.Empty);
				webEventFormatter.AppendLine(WebBaseEvent.FormatResourceStringWithCache("Webevent_event_custom_event_details"));
				webEventFormatter.IndentationLevel++;
				this.FormatCustomEventDetails(webEventFormatter);
				webEventFormatter.IndentationLevel--;
			}
			return webEventFormatter.ToString();
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x000A0252 File Offset: 0x0009F252
		public virtual void FormatCustomEventDetails(WebEventFormatter formatter)
		{
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x000A0254 File Offset: 0x0009F254
		internal int InferEtwTraceVerbosity()
		{
			switch (WebBaseEvent.WebEventTypeFromWebEvent(this))
			{
			case WebEventType.WEBEVENT_BASE_ERROR_EVENT:
			case WebEventType.WEBEVENT_REQUEST_ERROR_EVENT:
			case WebEventType.WEBEVENT_ERROR_EVENT:
			case WebEventType.WEBEVENT_FAILURE_AUDIT_EVENT:
			case WebEventType.WEBEVENT_AUTHENTICATION_FAILURE_AUDIT_EVENT:
			case WebEventType.WEBEVENT_VIEWSTATE_FAILURE_AUDIT_EVENT:
				return 3;
			case WebEventType.WEBEVENT_AUDIT_EVENT:
			case WebEventType.WEBEVENT_SUCCESS_AUDIT_EVENT:
			case WebEventType.WEBEVENT_AUTHENTICATION_SUCCESS_AUDIT_EVENT:
				return 4;
			}
			return 5;
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x000A02B0 File Offset: 0x0009F2B0
		internal void DeconstructWebEvent(out int eventType, out int fieldCount, out string[] fieldNames, out int[] fieldTypes, out string[] fieldData)
		{
			List<WebEventFieldData> list = new List<WebEventFieldData>();
			eventType = (int)WebBaseEvent.WebEventTypeFromWebEvent(this);
			this.GenerateFieldsForMarshal(list);
			fieldCount = list.Count;
			fieldNames = new string[fieldCount];
			fieldData = new string[fieldCount];
			fieldTypes = new int[fieldCount];
			for (int i = 0; i < fieldCount; i++)
			{
				fieldNames[i] = list[i].Name;
				fieldData[i] = list[i].Data;
				fieldTypes[i] = (int)list[i].Type;
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x000A0338 File Offset: 0x0009F338
		internal virtual void GenerateFieldsForMarshal(List<WebEventFieldData> fields)
		{
			fields.Add(new WebEventFieldData("EventTime", this.EventTimeUtc.ToString(), WebEventFieldType.String));
			fields.Add(new WebEventFieldData("EventID", this.EventID.ToString(), WebEventFieldType.String));
			fields.Add(new WebEventFieldData("EventMessage", this.Message, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("ApplicationDomain", WebBaseEvent.ApplicationInformation.ApplicationDomain, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("TrustLevel", WebBaseEvent.ApplicationInformation.TrustLevel, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("ApplicationVirtualPath", WebBaseEvent.ApplicationInformation.ApplicationVirtualPath, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("ApplicationPath", WebBaseEvent.ApplicationInformation.ApplicationPath, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("MachineName", WebBaseEvent.ApplicationInformation.MachineName, WebEventFieldType.String));
			fields.Add(new WebEventFieldData("EventCode", this.EventCode.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Int));
			fields.Add(new WebEventFieldData("EventDetailCode", this.EventDetailCode.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Int));
			fields.Add(new WebEventFieldData("SequenceNumber", this.EventSequence.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Long));
			fields.Add(new WebEventFieldData("Occurrence", this.EventOccurrence.ToString(CultureInfo.InstalledUICulture), WebEventFieldType.Long));
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x000A04BF File Offset: 0x0009F4BF
		internal virtual void PreProcessEventInit()
		{
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x000A04C4 File Offset: 0x0009F4C4
		private static void FindEventCode(Exception e, ref int eventCode, ref int eventDetailsCode, ref Exception eStack)
		{
			eventDetailsCode = 0;
			if (e is ConfigurationException)
			{
				eventCode = 3008;
			}
			else if (e is HttpRequestValidationException)
			{
				eventCode = 3003;
			}
			else if (e is HttpCompileException)
			{
				eventCode = 3007;
			}
			else if (e is SecurityException)
			{
				eventCode = 4010;
			}
			else if (e is UnauthorizedAccessException)
			{
				eventCode = 4011;
			}
			else if (e is HttpParseException)
			{
				eventCode = 3006;
			}
			else if (e is HttpException && e.InnerException is ViewStateException)
			{
				ViewStateException ex = (ViewStateException)e.InnerException;
				eventCode = 4009;
				if (ex._macValidationError)
				{
					eventDetailsCode = 50203;
				}
				else
				{
					eventDetailsCode = 50204;
				}
				eStack = ex;
			}
			else if (e is HttpException && ((HttpException)e).WebEventCode != 0)
			{
				eventCode = ((HttpException)e).WebEventCode;
			}
			else if (e.InnerException != null)
			{
				if (eStack == null)
				{
					eStack = e.InnerException;
				}
				WebBaseEvent.FindEventCode(e.InnerException, ref eventCode, ref eventDetailsCode, ref eStack);
			}
			else
			{
				eventCode = 3005;
			}
			if (eStack == null)
			{
				eStack = e;
			}
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000A05E8 File Offset: 0x0009F5E8
		internal static void RaiseRuntimeError(Exception e, object source)
		{
			if (!HealthMonitoringManager.Enabled)
			{
				return;
			}
			try
			{
				int num = 0;
				int num2 = 0;
				HttpContext httpContext = HttpContext.Current;
				Exception ex = null;
				if (httpContext != null)
				{
					Page page = httpContext.Handler as Page;
					if (page != null && page.IsTransacted && e.GetType() == typeof(HttpException) && e.InnerException != null)
					{
						e = e.InnerException;
					}
				}
				WebBaseEvent.FindEventCode(e, ref num, ref num2, ref ex);
				WebBaseEvent.RaiseSystemEvent(source, num, num2, ex);
			}
			catch
			{
			}
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000A0674 File Offset: 0x0009F674
		protected internal virtual void IncrementPerfCounters()
		{
			PerfCounters.IncrementCounter(AppPerfCounter.EVENTS_TOTAL);
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000A0680 File Offset: 0x0009F680
		internal void IncrementTotalCounters(int index0, int index1)
		{
			this._sequenceNumber = Interlocked.Increment(ref WebBaseEvent.s_globalSequenceNumber);
			if (index0 != -1)
			{
				this._occurrenceNumber = Interlocked.Increment(ref WebBaseEvent.s_eventCodeOccurrence[index0, index1]);
				return;
			}
			WebBaseEvent.CustomEventCodeOccurrence customEventCodeOccurrence = (WebBaseEvent.CustomEventCodeOccurrence)WebBaseEvent.s_customEventCodeOccurrence[this._code];
			if (customEventCodeOccurrence == null)
			{
				WebBaseEvent.s_lockCustomEventCodeOccurrence.AcquireWriterLock();
				try
				{
					customEventCodeOccurrence = (WebBaseEvent.CustomEventCodeOccurrence)WebBaseEvent.s_customEventCodeOccurrence[this._code];
					if (customEventCodeOccurrence == null)
					{
						customEventCodeOccurrence = new WebBaseEvent.CustomEventCodeOccurrence();
						WebBaseEvent.s_customEventCodeOccurrence[this._code] = customEventCodeOccurrence;
					}
				}
				finally
				{
					WebBaseEvent.s_lockCustomEventCodeOccurrence.ReleaseWriterLock();
				}
			}
			this._occurrenceNumber = Interlocked.Increment(ref customEventCodeOccurrence._occurrence);
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000A074C File Offset: 0x0009F74C
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
		public virtual void Raise()
		{
			WebBaseEvent.Raise(this);
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x000A0754 File Offset: 0x0009F754
		[AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Medium)]
		public static void Raise(WebBaseEvent eventRaised)
		{
			if (eventRaised.EventCode < 100000)
			{
				throw new HttpException(SR.GetString("System_eventCode_not_allowed", new object[]
				{
					eventRaised.EventCode.ToString(CultureInfo.CurrentCulture),
					100000.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (!HealthMonitoringManager.Enabled)
			{
				return;
			}
			WebBaseEvent.RaiseInternal(eventRaised, null, -1, -1);
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000A07C4 File Offset: 0x0009F7C4
		internal static void RaiseInternal(WebBaseEvent eventRaised, ArrayList firingRuleInfos, int index0, int index1)
		{
			bool flag = false;
			bool flag2 = false;
			ProcessImpersonationContext processImpersonationContext = null;
			HttpContext httpContext = HttpContext.Current;
			object data = CallContext.GetData("_WEvtRIP");
			if (data != null && (bool)data)
			{
				return;
			}
			eventRaised.IncrementPerfCounters();
			eventRaised.IncrementTotalCounters(index0, index1);
			if (firingRuleInfos == null)
			{
				HealthMonitoringManager healthMonitoringManager = HealthMonitoringManager.Manager();
				firingRuleInfos = healthMonitoringManager._sectionHelper.FindFiringRuleInfos(eventRaised.GetType(), eventRaised.EventCode);
			}
			if (firingRuleInfos.Count == 0)
			{
				return;
			}
			try
			{
				bool[] array = null;
				if (EtwTrace.IsTraceEnabled(5, 1) && httpContext != null)
				{
					EtwTrace.Trace(EtwTraceType.ETW_TYPE_WEB_EVENT_RAISE_START, httpContext.WorkerRequest, eventRaised.GetType().FullName, eventRaised.EventCode.ToString(CultureInfo.InstalledUICulture), eventRaised.EventDetailCode.ToString(CultureInfo.InstalledUICulture), null);
				}
				try
				{
					foreach (object obj in firingRuleInfos)
					{
						HealthMonitoringSectionHelper.FiringRuleInfo firingRuleInfo = (HealthMonitoringSectionHelper.FiringRuleInfo)obj;
						HealthMonitoringSectionHelper.RuleInfo ruleInfo = firingRuleInfo._ruleInfo;
						RuleFiringRecord ruleFiringRecord = ruleInfo._ruleFiringRecord;
						if (ruleFiringRecord.CheckAndUpdate(eventRaised) && ruleInfo._referencedProvider != null)
						{
							if (!flag)
							{
								eventRaised.PreProcessEventInit();
								flag = true;
							}
							if (firingRuleInfo._indexOfFirstRuleInfoWithSameProvider != -1)
							{
								if (array == null)
								{
									array = new bool[firingRuleInfos.Count];
								}
								if (array[firingRuleInfo._indexOfFirstRuleInfoWithSameProvider])
								{
									continue;
								}
								array[firingRuleInfo._indexOfFirstRuleInfoWithSameProvider] = true;
							}
							if (EtwTrace.IsTraceEnabled(5, 1) && httpContext != null)
							{
								EtwTrace.Trace(EtwTraceType.ETW_TYPE_WEB_EVENT_DELIVER_START, httpContext.WorkerRequest, ruleInfo._ruleSettings.Provider, ruleInfo._ruleSettings.Name, ruleInfo._ruleSettings.EventName, null);
							}
							try
							{
								if (processImpersonationContext == null)
								{
									processImpersonationContext = new ProcessImpersonationContext();
								}
								if (!flag2)
								{
									CallContext.SetData("_WEvtRIP", true);
									flag2 = true;
								}
								ruleInfo._referencedProvider.ProcessEvent(eventRaised);
							}
							catch (Exception ex)
							{
								try
								{
									ruleInfo._referencedProvider.LogException(ex);
								}
								catch
								{
								}
							}
							finally
							{
								if (EtwTrace.IsTraceEnabled(5, 1) && httpContext != null)
								{
									EtwTrace.Trace(EtwTraceType.ETW_TYPE_WEB_EVENT_DELIVER_END, httpContext.WorkerRequest);
								}
							}
						}
					}
				}
				finally
				{
					if (processImpersonationContext != null)
					{
						processImpersonationContext.Undo();
					}
					if (flag2)
					{
						CallContext.FreeNamedDataSlot("_WEvtRIP");
					}
					if (EtwTrace.IsTraceEnabled(5, 1) && httpContext != null)
					{
						EtwTrace.Trace(EtwTraceType.ETW_TYPE_WEB_EVENT_RAISE_END, httpContext.WorkerRequest);
					}
				}
			}
			catch
			{
				throw;
			}
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000A0A9C File Offset: 0x0009FA9C
		internal static void RaiseSystemEvent(string message, object source, int eventCode, int eventDetailCode, Exception exception)
		{
			WebBaseEvent.RaiseSystemEventInternal(message, source, eventCode, eventDetailCode, exception, null);
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000A0AAA File Offset: 0x0009FAAA
		internal static void RaiseSystemEvent(object source, int eventCode)
		{
			WebBaseEvent.RaiseSystemEventInternal(null, source, eventCode, 0, null, null);
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000A0AB7 File Offset: 0x0009FAB7
		internal static void RaiseSystemEvent(object source, int eventCode, int eventDetailCode)
		{
			WebBaseEvent.RaiseSystemEventInternal(null, source, eventCode, eventDetailCode, null, null);
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000A0AC4 File Offset: 0x0009FAC4
		internal static void RaiseSystemEvent(object source, int eventCode, int eventDetailCode, Exception exception)
		{
			WebBaseEvent.RaiseSystemEventInternal(null, source, eventCode, eventDetailCode, exception, null);
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000A0AD1 File Offset: 0x0009FAD1
		internal static void RaiseSystemEvent(object source, int eventCode, string nameToAuthenticate)
		{
			WebBaseEvent.RaiseSystemEventInternal(null, source, eventCode, 0, null, nameToAuthenticate);
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000A0AE0 File Offset: 0x0009FAE0
		private static void RaiseSystemEventInternal(string message, object source, int eventCode, int eventDetailCode, Exception exception, string nameToAuthenticate)
		{
			if (!HealthMonitoringManager.Enabled)
			{
				return;
			}
			int num;
			int num2;
			WebEventCodes.GetEventArrayIndexsFromEventCode(eventCode, out num, out num2);
			WebBaseEvent.SystemEventTypeInfo systemEventTypeInfo;
			WebBaseEvent.SystemEventType systemEventType;
			WebBaseEvent.GetSystemEventTypeInfo(eventCode, num, num2, out systemEventTypeInfo, out systemEventType);
			if (systemEventTypeInfo == null)
			{
				return;
			}
			HealthMonitoringManager healthMonitoringManager = HealthMonitoringManager.Manager();
			ArrayList arrayList = healthMonitoringManager._sectionHelper.FindFiringRuleInfos(systemEventTypeInfo._type, eventCode);
			if (arrayList.Count == 0)
			{
				systemEventTypeInfo._dummyEvent.IncrementPerfCounters();
				systemEventTypeInfo._dummyEvent.IncrementTotalCounters(num, num2);
				return;
			}
			WebBaseEvent.RaiseInternal(WebBaseEvent.NewEventFromSystemEventType(false, systemEventType, message, source, eventCode, eventDetailCode, exception, nameToAuthenticate), arrayList, num, num2);
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000A0B68 File Offset: 0x0009FB68
		private static void GetSystemEventTypeInfo(int eventCode, int index0, int index1, out WebBaseEvent.SystemEventTypeInfo info, out WebBaseEvent.SystemEventType systemEventType)
		{
			systemEventType = WebBaseEvent.s_eventCodeToSystemEventTypeMappings[index0, index1];
			if (systemEventType == WebBaseEvent.SystemEventType.Unknown)
			{
				systemEventType = WebBaseEvent.SystemEventTypeFromEventCode(eventCode);
				WebBaseEvent.s_eventCodeToSystemEventTypeMappings[index0, index1] = systemEventType;
			}
			info = WebBaseEvent.s_systemEventTypeInfos[(int)systemEventType];
			if (info != null)
			{
				return;
			}
			info = new WebBaseEvent.SystemEventTypeInfo(WebBaseEvent.CreateDummySystemEvent(systemEventType));
			WebBaseEvent.s_systemEventTypeInfos[(int)systemEventType] = info;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000A0BCC File Offset: 0x0009FBCC
		private static WebBaseEvent.SystemEventType SystemEventTypeFromEventCode(int eventCode)
		{
			if (eventCode >= 1000 && eventCode <= 1005)
			{
				switch (eventCode)
				{
				case 1001:
				case 1002:
				case 1003:
				case 1004:
					return WebBaseEvent.SystemEventType.WebApplicationLifetimeEvent;
				case 1005:
					return WebBaseEvent.SystemEventType.WebHeartbeatEvent;
				}
			}
			if (eventCode >= 2000 && eventCode <= 2002)
			{
				switch (eventCode)
				{
				case 2001:
				case 2002:
					return WebBaseEvent.SystemEventType.WebRequestEvent;
				}
			}
			if (eventCode >= 3000 && eventCode <= 3011)
			{
				switch (eventCode)
				{
				case 3001:
				case 3002:
				case 3003:
				case 3004:
				case 3005:
					return WebBaseEvent.SystemEventType.WebRequestErrorEvent;
				case 3006:
				case 3007:
				case 3008:
				case 3009:
				case 3010:
				case 3011:
					return WebBaseEvent.SystemEventType.WebErrorEvent;
				}
			}
			if (eventCode >= 4000 && eventCode <= 4011)
			{
				switch (eventCode)
				{
				case 4001:
				case 4002:
					return WebBaseEvent.SystemEventType.WebAuthenticationSuccessAuditEvent;
				case 4003:
				case 4004:
					return WebBaseEvent.SystemEventType.WebSuccessAuditEvent;
				case 4005:
				case 4006:
					return WebBaseEvent.SystemEventType.WebAuthenticationFailureAuditEvent;
				case 4007:
				case 4008:
				case 4010:
				case 4011:
					return WebBaseEvent.SystemEventType.WebFailureAuditEvent;
				case 4009:
					return WebBaseEvent.SystemEventType.WebViewStateFailureAuditEvent;
				}
			}
			if (eventCode >= 6000 && eventCode <= 6001 && eventCode == 6001)
			{
				return WebBaseEvent.SystemEventType.Unknown;
			}
			return WebBaseEvent.SystemEventType.Unknown;
		}

		// Token: 0x0600255F RID: 9567 RVA: 0x000A0D01 File Offset: 0x0009FD01
		private static WebBaseEvent CreateDummySystemEvent(WebBaseEvent.SystemEventType systemEventType)
		{
			return WebBaseEvent.NewEventFromSystemEventType(true, systemEventType, null, null, 0, 0, null, null);
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000A0D10 File Offset: 0x0009FD10
		private static WebBaseEvent NewEventFromSystemEventType(bool createDummy, WebBaseEvent.SystemEventType systemEventType, string message, object source, int eventCode, int eventDetailCode, Exception exception, string nameToAuthenticate)
		{
			if (!createDummy && message == null)
			{
				message = WebEventCodes.MessageFromEventCode(eventCode, eventDetailCode);
			}
			switch (systemEventType)
			{
			case WebBaseEvent.SystemEventType.WebApplicationLifetimeEvent:
				if (!createDummy)
				{
					return new WebApplicationLifetimeEvent(message, source, eventCode, eventDetailCode);
				}
				return new WebApplicationLifetimeEvent();
			case WebBaseEvent.SystemEventType.WebHeartbeatEvent:
				if (!createDummy)
				{
					return new WebHeartbeatEvent(message, eventCode);
				}
				return new WebHeartbeatEvent();
			case WebBaseEvent.SystemEventType.WebRequestEvent:
				if (!createDummy)
				{
					return new WebRequestEvent(message, source, eventCode, eventDetailCode);
				}
				return new WebRequestEvent();
			case WebBaseEvent.SystemEventType.WebRequestErrorEvent:
				if (!createDummy)
				{
					return new WebRequestErrorEvent(message, source, eventCode, eventDetailCode, exception);
				}
				return new WebRequestErrorEvent();
			case WebBaseEvent.SystemEventType.WebErrorEvent:
				if (!createDummy)
				{
					return new WebErrorEvent(message, source, eventCode, eventDetailCode, exception);
				}
				return new WebErrorEvent();
			case WebBaseEvent.SystemEventType.WebAuthenticationSuccessAuditEvent:
				if (!createDummy)
				{
					return new WebAuthenticationSuccessAuditEvent(message, source, eventCode, eventDetailCode, nameToAuthenticate);
				}
				return new WebAuthenticationSuccessAuditEvent();
			case WebBaseEvent.SystemEventType.WebSuccessAuditEvent:
				if (!createDummy)
				{
					return new WebSuccessAuditEvent(message, source, eventCode, eventDetailCode);
				}
				return new WebSuccessAuditEvent();
			case WebBaseEvent.SystemEventType.WebAuthenticationFailureAuditEvent:
				if (!createDummy)
				{
					return new WebAuthenticationFailureAuditEvent(message, source, eventCode, eventDetailCode, nameToAuthenticate);
				}
				return new WebAuthenticationFailureAuditEvent();
			case WebBaseEvent.SystemEventType.WebFailureAuditEvent:
				if (!createDummy)
				{
					return new WebFailureAuditEvent(message, source, eventCode, eventDetailCode);
				}
				return new WebFailureAuditEvent();
			case WebBaseEvent.SystemEventType.WebViewStateFailureAuditEvent:
				if (!createDummy)
				{
					return new WebViewStateFailureAuditEvent(message, source, eventCode, eventDetailCode, (ViewStateException)exception);
				}
				return new WebViewStateFailureAuditEvent();
			default:
				return null;
			}
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000A0E42 File Offset: 0x0009FE42
		private static string CreateWebEventResourceCacheKey(string key)
		{
			return "x" + key;
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000A0E50 File Offset: 0x0009FE50
		internal static string FormatResourceStringWithCache(string key)
		{
			CacheInternal cacheInternal = HttpRuntime.CacheInternal;
			string text = WebBaseEvent.CreateWebEventResourceCacheKey(key);
			string text2 = (string)cacheInternal.Get(text);
			if (text2 != null)
			{
				return text2;
			}
			text2 = SR.Resources.GetString(key, CultureInfo.InstalledUICulture);
			if (text2 != null)
			{
				cacheInternal.UtcInsert(text, text2);
			}
			return text2;
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000A0E9C File Offset: 0x0009FE9C
		internal static string FormatResourceStringWithCache(string key, string arg0)
		{
			string text = WebBaseEvent.FormatResourceStringWithCache(key);
			if (text == null)
			{
				return null;
			}
			return string.Format(text, arg0);
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000A0EBC File Offset: 0x0009FEBC
		internal static WebEventType WebEventTypeFromWebEvent(WebBaseEvent eventRaised)
		{
			if (eventRaised is WebAuthenticationSuccessAuditEvent)
			{
				return WebEventType.WEBEVENT_AUTHENTICATION_SUCCESS_AUDIT_EVENT;
			}
			if (eventRaised is WebAuthenticationFailureAuditEvent)
			{
				return WebEventType.WEBEVENT_AUTHENTICATION_FAILURE_AUDIT_EVENT;
			}
			if (eventRaised is WebViewStateFailureAuditEvent)
			{
				return WebEventType.WEBEVENT_VIEWSTATE_FAILURE_AUDIT_EVENT;
			}
			if (eventRaised is WebRequestErrorEvent)
			{
				return WebEventType.WEBEVENT_REQUEST_ERROR_EVENT;
			}
			if (eventRaised is WebErrorEvent)
			{
				return WebEventType.WEBEVENT_ERROR_EVENT;
			}
			if (eventRaised is WebSuccessAuditEvent)
			{
				return WebEventType.WEBEVENT_SUCCESS_AUDIT_EVENT;
			}
			if (eventRaised is WebFailureAuditEvent)
			{
				return WebEventType.WEBEVENT_FAILURE_AUDIT_EVENT;
			}
			if (eventRaised is WebHeartbeatEvent)
			{
				return WebEventType.WEBEVENT_HEARTBEAT_EVENT;
			}
			if (eventRaised is WebApplicationLifetimeEvent)
			{
				return WebEventType.WEBEVENT_APP_LIFETIME_EVENT;
			}
			if (eventRaised is WebRequestEvent)
			{
				return WebEventType.WEBEVENT_REQUEST_EVENT;
			}
			if (eventRaised is WebBaseErrorEvent)
			{
				return WebEventType.WEBEVENT_BASE_ERROR_EVENT;
			}
			if (eventRaised is WebAuditEvent)
			{
				return WebEventType.WEBEVENT_AUDIT_EVENT;
			}
			if (eventRaised is WebManagementEvent)
			{
				return WebEventType.WEBEVENT_MANAGEMENT_EVENT;
			}
			return WebEventType.WEBEVENT_BASE_EVENT;
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000A0F54 File Offset: 0x0009FF54
		internal static void RaisePropertyDeserializationWebErrorEvent(SettingsProperty property, object source, Exception exception)
		{
			if (HttpContext.Current == null)
			{
				return;
			}
			WebBaseEvent.RaiseSystemEvent(SR.GetString("Webevent_msg_Property_Deserialization", new object[]
			{
				property.Name,
				property.SerializeAs.ToString(),
				property.PropertyType.AssemblyQualifiedName
			}), source, 3010, 0, exception);
		}

		// Token: 0x04001D40 RID: 7488
		private const string WEBEVENT_RAISE_IN_PROGRESS = "_WEvtRIP";

		// Token: 0x04001D41 RID: 7489
		private DateTime _eventTimeUtc;

		// Token: 0x04001D42 RID: 7490
		private int _code;

		// Token: 0x04001D43 RID: 7491
		private int _detailCode;

		// Token: 0x04001D44 RID: 7492
		private object _source;

		// Token: 0x04001D45 RID: 7493
		private string _message;

		// Token: 0x04001D46 RID: 7494
		private long _sequenceNumber;

		// Token: 0x04001D47 RID: 7495
		private long _occurrenceNumber;

		// Token: 0x04001D48 RID: 7496
		private Guid _id = Guid.Empty;

		// Token: 0x04001D49 RID: 7497
		private static long s_globalSequenceNumber = 0L;

		// Token: 0x04001D4A RID: 7498
		private static WebApplicationInformation s_applicationInfo = new WebApplicationInformation();

		// Token: 0x04001D4B RID: 7499
		private static readonly WebBaseEvent.SystemEventType[,] s_eventCodeToSystemEventTypeMappings = new WebBaseEvent.SystemEventType[WebEventCodes.GetEventArrayDimensionSize(0), WebEventCodes.GetEventArrayDimensionSize(1)];

		// Token: 0x04001D4C RID: 7500
		private static readonly long[,] s_eventCodeOccurrence = new long[WebEventCodes.GetEventArrayDimensionSize(0), WebEventCodes.GetEventArrayDimensionSize(1)];

		// Token: 0x04001D4D RID: 7501
		private static Hashtable s_customEventCodeOccurrence = new Hashtable();

		// Token: 0x04001D4E RID: 7502
		private static ReadWriteSpinLock s_lockCustomEventCodeOccurrence;

		// Token: 0x04001D4F RID: 7503
		private static WebBaseEvent.SystemEventTypeInfo[] s_systemEventTypeInfos = new WebBaseEvent.SystemEventTypeInfo[10];

		// Token: 0x020002E3 RID: 739
		private class CustomEventCodeOccurrence
		{
			// Token: 0x04001D50 RID: 7504
			internal long _occurrence;
		}

		// Token: 0x020002E4 RID: 740
		private enum SystemEventType
		{
			// Token: 0x04001D52 RID: 7506
			Unknown = -1,
			// Token: 0x04001D53 RID: 7507
			WebApplicationLifetimeEvent,
			// Token: 0x04001D54 RID: 7508
			WebHeartbeatEvent,
			// Token: 0x04001D55 RID: 7509
			WebRequestEvent,
			// Token: 0x04001D56 RID: 7510
			WebRequestErrorEvent,
			// Token: 0x04001D57 RID: 7511
			WebErrorEvent,
			// Token: 0x04001D58 RID: 7512
			WebAuthenticationSuccessAuditEvent,
			// Token: 0x04001D59 RID: 7513
			WebSuccessAuditEvent,
			// Token: 0x04001D5A RID: 7514
			WebAuthenticationFailureAuditEvent,
			// Token: 0x04001D5B RID: 7515
			WebFailureAuditEvent,
			// Token: 0x04001D5C RID: 7516
			WebViewStateFailureAuditEvent,
			// Token: 0x04001D5D RID: 7517
			Last
		}

		// Token: 0x020002E5 RID: 741
		private class SystemEventTypeInfo
		{
			// Token: 0x06002567 RID: 9575 RVA: 0x000A0FBA File Offset: 0x0009FFBA
			internal SystemEventTypeInfo(WebBaseEvent dummyEvent)
			{
				this._dummyEvent = dummyEvent;
				this._type = dummyEvent.GetType();
			}

			// Token: 0x04001D5E RID: 7518
			internal WebBaseEvent _dummyEvent;

			// Token: 0x04001D5F RID: 7519
			internal Type _type;
		}
	}
}
