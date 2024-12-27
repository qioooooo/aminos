using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002C5 RID: 709
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class EventLogWebEventProvider : WebEventProvider, IInternalWebEventProvider
	{
		// Token: 0x06002473 RID: 9331 RVA: 0x0009B8AD File Offset: 0x0009A8AD
		internal EventLogWebEventProvider()
		{
		}

		// Token: 0x06002474 RID: 9332 RVA: 0x0009B8B5 File Offset: 0x0009A8B5
		public override void Initialize(string name, NameValueCollection config)
		{
			this._maxTruncatedParamLen = 32766 - "...".Length;
			base.Initialize(name, config);
			ProviderUtil.CheckUnrecognizedAttributes(config, name);
		}

		// Token: 0x06002475 RID: 9333 RVA: 0x0009B8DC File Offset: 0x0009A8DC
		private void AddBasicDataFields(ArrayList dataFields, WebBaseEvent eventRaised)
		{
			WebApplicationInformation applicationInformation = WebBaseEvent.ApplicationInformation;
			dataFields.Add(eventRaised.EventCode.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(eventRaised.Message);
			dataFields.Add(eventRaised.EventTime.ToString());
			dataFields.Add(eventRaised.EventTimeUtc.ToString());
			dataFields.Add(eventRaised.EventID.ToString("N", CultureInfo.InstalledUICulture));
			dataFields.Add(eventRaised.EventSequence.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(eventRaised.EventOccurrence.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(eventRaised.EventDetailCode.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(applicationInformation.ApplicationDomain);
			dataFields.Add(applicationInformation.TrustLevel);
			dataFields.Add(applicationInformation.ApplicationVirtualPath);
			dataFields.Add(applicationInformation.ApplicationPath);
			dataFields.Add(applicationInformation.MachineName);
			if (eventRaised.IsSystemEvent)
			{
				dataFields.Add(null);
				return;
			}
			WebEventFormatter webEventFormatter = new WebEventFormatter();
			eventRaised.FormatCustomEventDetails(webEventFormatter);
			dataFields.Add(webEventFormatter.ToString());
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x0009BA2C File Offset: 0x0009AA2C
		private void AddWebProcessInformationDataFields(ArrayList dataFields, WebProcessInformation processEventInfo)
		{
			dataFields.Add(processEventInfo.ProcessID.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(processEventInfo.ProcessName);
			dataFields.Add(processEventInfo.AccountName);
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x0009BA70 File Offset: 0x0009AA70
		private void AddWebRequestInformationDataFields(ArrayList dataFields, WebRequestInformation reqInfo)
		{
			IPrincipal principal = reqInfo.Principal;
			string text;
			bool flag;
			string text2;
			if (principal == null)
			{
				text = null;
				flag = false;
				text2 = null;
			}
			else
			{
				IIdentity identity = principal.Identity;
				text = identity.Name;
				flag = identity.IsAuthenticated;
				text2 = identity.AuthenticationType;
			}
			dataFields.Add(reqInfo.RequestUrl);
			dataFields.Add(reqInfo.RequestPath);
			dataFields.Add(reqInfo.UserHostAddress);
			dataFields.Add(text);
			dataFields.Add(flag.ToString());
			dataFields.Add(text2);
			dataFields.Add(reqInfo.ThreadAccountName);
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x0009BB04 File Offset: 0x0009AB04
		private void AddWebProcessStatisticsDataFields(ArrayList dataFields, WebProcessStatistics procStats)
		{
			dataFields.Add(procStats.ProcessStartTime.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(procStats.ThreadCount.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(procStats.WorkingSet.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(procStats.PeakWorkingSet.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(procStats.ManagedHeapSize.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(procStats.AppDomainCount.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(procStats.RequestsExecuting.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(procStats.RequestsQueued.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(procStats.RequestsRejected.ToString(CultureInfo.InstalledUICulture));
		}

		// Token: 0x06002479 RID: 9337 RVA: 0x0009BC00 File Offset: 0x0009AC00
		private void AddExceptionDataFields(ArrayList dataFields, Exception exception)
		{
			if (exception == null)
			{
				dataFields.Add(null);
				dataFields.Add(null);
				return;
			}
			dataFields.Add(exception.GetType().Name);
			dataFields.Add(exception.Message);
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x0009BC38 File Offset: 0x0009AC38
		private void AddWebThreadInformationDataFields(ArrayList dataFields, WebThreadInformation threadInfo)
		{
			dataFields.Add(threadInfo.ThreadID.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(threadInfo.ThreadAccountName);
			dataFields.Add(threadInfo.IsImpersonating.ToString(CultureInfo.InstalledUICulture));
			dataFields.Add(threadInfo.StackTrace);
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x0009BC94 File Offset: 0x0009AC94
		private void AddViewStateExceptionDataFields(ArrayList dataFields, ViewStateException vse)
		{
			dataFields.Add(SR.GetString(vse.ShortMessage));
			dataFields.Add(vse.RemoteAddress);
			dataFields.Add(vse.RemotePort);
			dataFields.Add(vse.UserAgent);
			dataFields.Add(vse.PersistedState);
			dataFields.Add(vse.Referer);
			dataFields.Add(vse.Path);
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x0009BD04 File Offset: 0x0009AD04
		public override void ProcessEvent(WebBaseEvent eventRaised)
		{
			ArrayList arrayList = new ArrayList(35);
			WebEventType webEventType = WebBaseEvent.WebEventTypeFromWebEvent(eventRaised);
			this.AddBasicDataFields(arrayList, eventRaised);
			if (eventRaised is WebManagementEvent)
			{
				this.AddWebProcessInformationDataFields(arrayList, ((WebManagementEvent)eventRaised).ProcessInformation);
			}
			if (eventRaised is WebHeartbeatEvent)
			{
				this.AddWebProcessStatisticsDataFields(arrayList, ((WebHeartbeatEvent)eventRaised).ProcessStatistics);
			}
			if (eventRaised is WebRequestEvent)
			{
				this.AddWebRequestInformationDataFields(arrayList, ((WebRequestEvent)eventRaised).RequestInformation);
			}
			if (eventRaised is WebBaseErrorEvent)
			{
				this.AddExceptionDataFields(arrayList, ((WebBaseErrorEvent)eventRaised).ErrorException);
			}
			if (eventRaised is WebAuditEvent)
			{
				this.AddWebRequestInformationDataFields(arrayList, ((WebAuditEvent)eventRaised).RequestInformation);
			}
			if (eventRaised is WebRequestErrorEvent)
			{
				this.AddWebRequestInformationDataFields(arrayList, ((WebRequestErrorEvent)eventRaised).RequestInformation);
				this.AddWebThreadInformationDataFields(arrayList, ((WebRequestErrorEvent)eventRaised).ThreadInformation);
			}
			if (eventRaised is WebErrorEvent)
			{
				this.AddWebRequestInformationDataFields(arrayList, ((WebErrorEvent)eventRaised).RequestInformation);
				this.AddWebThreadInformationDataFields(arrayList, ((WebErrorEvent)eventRaised).ThreadInformation);
			}
			if (eventRaised is WebAuthenticationSuccessAuditEvent)
			{
				arrayList.Add(((WebAuthenticationSuccessAuditEvent)eventRaised).NameToAuthenticate);
			}
			if (eventRaised is WebAuthenticationFailureAuditEvent)
			{
				arrayList.Add(((WebAuthenticationFailureAuditEvent)eventRaised).NameToAuthenticate);
			}
			if (eventRaised is WebViewStateFailureAuditEvent)
			{
				this.AddViewStateExceptionDataFields(arrayList, ((WebViewStateFailureAuditEvent)eventRaised).ViewStateException);
			}
			for (int i = 0; i < arrayList.Count; i++)
			{
				object obj = arrayList[i];
				if (obj != null)
				{
					int length = ((string)obj).Length;
					if (length > 32766)
					{
						arrayList[i] = ((string)obj).Substring(0, this._maxTruncatedParamLen) + "...";
					}
				}
			}
			int num = UnsafeNativeMethods.RaiseEventlogEvent((int)webEventType, (string[])arrayList.ToArray(typeof(string)), arrayList.Count);
			if (num != 0)
			{
				throw new HttpException(SR.GetString("Event_log_provider_error", new object[] { "0x" + num.ToString("X8", CultureInfo.InstalledUICulture) }));
			}
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x0009BF07 File Offset: 0x0009AF07
		public override void Flush()
		{
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x0009BF09 File Offset: 0x0009AF09
		public override void Shutdown()
		{
		}

		// Token: 0x04001C48 RID: 7240
		private const int EventLogParameterMaxLength = 32766;

		// Token: 0x04001C49 RID: 7241
		private const string _truncateWarning = "...";

		// Token: 0x04001C4A RID: 7242
		private int _maxTruncatedParamLen;
	}
}
