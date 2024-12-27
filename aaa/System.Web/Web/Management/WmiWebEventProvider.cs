using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Web.UI;
using System.Web.Util;

namespace System.Web.Management
{
	// Token: 0x020002FE RID: 766
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class WmiWebEventProvider : WebEventProvider
	{
		// Token: 0x06002613 RID: 9747 RVA: 0x000A2EE0 File Offset: 0x000A1EE0
		public override void Initialize(string name, NameValueCollection config)
		{
			int num = UnsafeNativeMethods.InitializeWmiManager();
			if (num != 0)
			{
				throw new ConfigurationErrorsException(SR.GetString("Wmi_provider_cant_initialize", new object[] { "0x" + num.ToString("X8", CultureInfo.CurrentCulture) }));
			}
			base.Initialize(name, config);
			ProviderUtil.CheckUnrecognizedAttributes(config, name);
		}

		// Token: 0x06002614 RID: 9748 RVA: 0x000A2F3C File Offset: 0x000A1F3C
		private string WmiFormatTime(DateTime dt)
		{
			StringBuilder stringBuilder = new StringBuilder(26);
			stringBuilder.Append(dt.ToString("yyyyMMddHHmmss.ffffff", CultureInfo.InstalledUICulture));
			double totalMinutes = TimeZone.CurrentTimeZone.GetUtcOffset(dt).TotalMinutes;
			if (totalMinutes >= 0.0)
			{
				stringBuilder.Append('+');
			}
			stringBuilder.Append(totalMinutes);
			return stringBuilder.ToString();
		}

		// Token: 0x06002615 RID: 9749 RVA: 0x000A2FA0 File Offset: 0x000A1FA0
		private void FillBasicWmiDataFields(ref UnsafeNativeMethods.WmiData wmiData, WebBaseEvent eventRaised)
		{
			WebApplicationInformation applicationInformation = WebBaseEvent.ApplicationInformation;
			wmiData.eventType = (int)WebBaseEvent.WebEventTypeFromWebEvent(eventRaised);
			wmiData.eventCode = eventRaised.EventCode;
			wmiData.eventDetailCode = eventRaised.EventDetailCode;
			wmiData.eventTime = this.WmiFormatTime(eventRaised.EventTime);
			wmiData.eventMessage = eventRaised.Message;
			wmiData.sequenceNumber = eventRaised.EventSequence.ToString(CultureInfo.InstalledUICulture);
			wmiData.occurrence = eventRaised.EventOccurrence.ToString(CultureInfo.InstalledUICulture);
			wmiData.eventId = eventRaised.EventID.ToString("N", CultureInfo.InstalledUICulture);
			wmiData.appDomain = applicationInformation.ApplicationDomain;
			wmiData.trustLevel = applicationInformation.TrustLevel;
			wmiData.appVirtualPath = applicationInformation.ApplicationVirtualPath;
			wmiData.appPath = applicationInformation.ApplicationPath;
			wmiData.machineName = applicationInformation.MachineName;
			if (eventRaised.IsSystemEvent)
			{
				wmiData.details = string.Empty;
				return;
			}
			WebEventFormatter webEventFormatter = new WebEventFormatter();
			eventRaised.FormatCustomEventDetails(webEventFormatter);
			wmiData.details = webEventFormatter.ToString();
		}

		// Token: 0x06002616 RID: 9750 RVA: 0x000A30B0 File Offset: 0x000A20B0
		private void FillRequestWmiDataFields(ref UnsafeNativeMethods.WmiData wmiData, WebRequestInformation reqInfo)
		{
			IPrincipal principal = reqInfo.Principal;
			string text;
			string text2;
			bool flag;
			if (principal == null)
			{
				text = string.Empty;
				text2 = string.Empty;
				flag = false;
			}
			else
			{
				IIdentity identity = principal.Identity;
				text = identity.Name;
				flag = identity.IsAuthenticated;
				text2 = identity.AuthenticationType;
			}
			wmiData.requestUrl = reqInfo.RequestUrl;
			wmiData.requestPath = reqInfo.RequestPath;
			wmiData.userHostAddress = reqInfo.UserHostAddress;
			wmiData.userName = text;
			wmiData.userAuthenticated = flag;
			wmiData.userAuthenticationType = text2;
			wmiData.requestThreadAccountName = reqInfo.ThreadAccountName;
		}

		// Token: 0x06002617 RID: 9751 RVA: 0x000A313C File Offset: 0x000A213C
		private void FillErrorWmiDataFields(ref UnsafeNativeMethods.WmiData wmiData, WebThreadInformation threadInfo)
		{
			wmiData.threadId = threadInfo.ThreadID;
			wmiData.threadAccountName = threadInfo.ThreadAccountName;
			wmiData.stackTrace = threadInfo.StackTrace;
			wmiData.isImpersonating = threadInfo.IsImpersonating;
		}

		// Token: 0x06002618 RID: 9752 RVA: 0x000A3170 File Offset: 0x000A2170
		public override void ProcessEvent(WebBaseEvent eventRaised)
		{
			UnsafeNativeMethods.WmiData wmiData = default(UnsafeNativeMethods.WmiData);
			this.FillBasicWmiDataFields(ref wmiData, eventRaised);
			WebApplicationLifetimeEvent webApplicationLifetimeEvent = eventRaised as WebApplicationLifetimeEvent;
			if (eventRaised is WebManagementEvent)
			{
				WebProcessInformation processInformation = ((WebManagementEvent)eventRaised).ProcessInformation;
				wmiData.processId = processInformation.ProcessID;
				wmiData.processName = processInformation.ProcessName;
				wmiData.accountName = processInformation.AccountName;
			}
			if (eventRaised is WebRequestEvent)
			{
				this.FillRequestWmiDataFields(ref wmiData, ((WebRequestEvent)eventRaised).RequestInformation);
			}
			if (eventRaised is WebAuditEvent)
			{
				this.FillRequestWmiDataFields(ref wmiData, ((WebAuditEvent)eventRaised).RequestInformation);
			}
			if (eventRaised is WebAuthenticationSuccessAuditEvent)
			{
				wmiData.nameToAuthenticate = ((WebAuthenticationSuccessAuditEvent)eventRaised).NameToAuthenticate;
			}
			if (eventRaised is WebAuthenticationFailureAuditEvent)
			{
				wmiData.nameToAuthenticate = ((WebAuthenticationFailureAuditEvent)eventRaised).NameToAuthenticate;
			}
			if (eventRaised is WebViewStateFailureAuditEvent)
			{
				ViewStateException viewStateException = ((WebViewStateFailureAuditEvent)eventRaised).ViewStateException;
				wmiData.exceptionMessage = SR.GetString(viewStateException.ShortMessage);
				wmiData.remoteAddress = viewStateException.RemoteAddress;
				wmiData.remotePort = viewStateException.RemotePort;
				wmiData.userAgent = viewStateException.UserAgent;
				wmiData.persistedState = viewStateException.PersistedState;
				wmiData.referer = viewStateException.Referer;
				wmiData.path = viewStateException.Path;
			}
			if (eventRaised is WebHeartbeatEvent)
			{
				WebHeartbeatEvent webHeartbeatEvent = eventRaised as WebHeartbeatEvent;
				WebProcessStatistics processStatistics = webHeartbeatEvent.ProcessStatistics;
				wmiData.processStartTime = this.WmiFormatTime(processStatistics.ProcessStartTime);
				wmiData.threadCount = processStatistics.ThreadCount;
				wmiData.workingSet = processStatistics.WorkingSet.ToString(CultureInfo.InstalledUICulture);
				wmiData.peakWorkingSet = processStatistics.PeakWorkingSet.ToString(CultureInfo.InstalledUICulture);
				wmiData.managedHeapSize = processStatistics.ManagedHeapSize.ToString(CultureInfo.InstalledUICulture);
				wmiData.appdomainCount = processStatistics.AppDomainCount;
				wmiData.requestsExecuting = processStatistics.RequestsExecuting;
				wmiData.requestsQueued = processStatistics.RequestsQueued;
				wmiData.requestsRejected = processStatistics.RequestsRejected;
			}
			if (eventRaised is WebBaseErrorEvent)
			{
				Exception errorException = ((WebBaseErrorEvent)eventRaised).ErrorException;
				if (errorException == null)
				{
					wmiData.exceptionType = string.Empty;
					wmiData.exceptionMessage = string.Empty;
				}
				else
				{
					wmiData.exceptionType = errorException.GetType().Name;
					wmiData.exceptionMessage = errorException.Message;
				}
			}
			if (eventRaised is WebRequestErrorEvent)
			{
				WebRequestErrorEvent webRequestErrorEvent = eventRaised as WebRequestErrorEvent;
				WebRequestInformation requestInformation = webRequestErrorEvent.RequestInformation;
				WebThreadInformation threadInformation = webRequestErrorEvent.ThreadInformation;
				this.FillRequestWmiDataFields(ref wmiData, requestInformation);
				this.FillErrorWmiDataFields(ref wmiData, threadInformation);
			}
			if (eventRaised is WebErrorEvent)
			{
				WebErrorEvent webErrorEvent = eventRaised as WebErrorEvent;
				WebRequestInformation requestInformation2 = webErrorEvent.RequestInformation;
				WebThreadInformation threadInformation2 = webErrorEvent.ThreadInformation;
				this.FillRequestWmiDataFields(ref wmiData, requestInformation2);
				this.FillErrorWmiDataFields(ref wmiData, threadInformation2);
			}
			int num = UnsafeNativeMethods.RaiseWmiEvent(ref wmiData, AspCompatApplicationStep.IsInAspCompatMode);
			if (num != 0)
			{
				throw new HttpException(SR.GetString("Wmi_provider_error", new object[] { "0x" + num.ToString("X8", CultureInfo.InstalledUICulture) }));
			}
		}

		// Token: 0x06002619 RID: 9753 RVA: 0x000A3481 File Offset: 0x000A2481
		public override void Flush()
		{
		}

		// Token: 0x0600261A RID: 9754 RVA: 0x000A3483 File Offset: 0x000A2483
		public override void Shutdown()
		{
		}
	}
}
