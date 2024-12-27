using System;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002DD RID: 733
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebEventCodes
	{
		// Token: 0x0600252D RID: 9517 RVA: 0x0009F94D File Offset: 0x0009E94D
		private WebEventCodes()
		{
		}

		// Token: 0x0600252E RID: 9518 RVA: 0x0009F955 File Offset: 0x0009E955
		static WebEventCodes()
		{
			WebEventCodes.InitEventArrayDimensions();
		}

		// Token: 0x0600252F RID: 9519 RVA: 0x0009F968 File Offset: 0x0009E968
		internal static string MessageFromEventCode(int eventCode, int eventDetailCode)
		{
			string text = null;
			if (eventDetailCode != 0)
			{
				switch (eventDetailCode)
				{
				case 50001:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownUnknown");
					break;
				case 50002:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownHostingEnvironment");
					break;
				case 50003:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownChangeInGlobalAsax");
					break;
				case 50004:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownConfigurationChange");
					break;
				case 50005:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownUnloadAppDomainCalled");
					break;
				case 50006:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownChangeInSecurityPolicyFile");
					break;
				case 50007:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownBinDirChangeOrDirectoryRename");
					break;
				case 50008:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownBrowsersDirChangeOrDirectoryRename");
					break;
				case 50009:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownCodeDirChangeOrDirectoryRename");
					break;
				case 50010:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownResourcesDirChangeOrDirectoryRename");
					break;
				case 50011:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownIdleTimeout");
					break;
				case 50012:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownPhysicalApplicationPathChanged");
					break;
				case 50013:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownHttpRuntimeClose");
					break;
				case 50014:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownInitializationError");
					break;
				case 50015:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownMaxRecompilationsReached");
					break;
				case 50016:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_StateServerConnectionError");
					break;
				case 50017:
					text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ApplicationShutdownBuildManagerChange");
					break;
				default:
					switch (eventDetailCode)
					{
					case 50201:
						text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_InvalidTicketFailure");
						break;
					case 50202:
						text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_ExpiredTicketFailure");
						break;
					case 50203:
						text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_InvalidViewStateMac");
						break;
					case 50204:
						text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_InvalidViewState");
						break;
					default:
						if (eventDetailCode == 50301)
						{
							text = WebBaseEvent.FormatResourceStringWithCache("Webevent_detail_SqlProviderEventsDropped");
						}
						break;
					}
					break;
				}
			}
			string text2;
			if (eventCode <= 2002)
			{
				switch (eventCode)
				{
				case 1001:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_ApplicationStart");
					goto IL_03FC;
				case 1002:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_ApplicationShutdown");
					goto IL_03FC;
				case 1003:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_ApplicationCompilationStart");
					goto IL_03FC;
				case 1004:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_ApplicationCompilationEnd");
					goto IL_03FC;
				case 1005:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_ApplicationHeartbeat");
					goto IL_03FC;
				default:
					switch (eventCode)
					{
					case 2001:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_RequestTransactionComplete");
						goto IL_03FC;
					case 2002:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_RequestTransactionAbort");
						goto IL_03FC;
					}
					break;
				}
			}
			else
			{
				switch (eventCode)
				{
				case 3001:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_RuntimeErrorRequestAbort");
					goto IL_03FC;
				case 3002:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_RuntimeErrorViewStateFailure");
					goto IL_03FC;
				case 3003:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_RuntimeErrorValidationFailure");
					goto IL_03FC;
				case 3004:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_RuntimeErrorPostTooLarge");
					goto IL_03FC;
				case 3005:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_RuntimeErrorUnhandledException");
					goto IL_03FC;
				case 3006:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_WebErrorParserError");
					goto IL_03FC;
				case 3007:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_WebErrorCompilationError");
					goto IL_03FC;
				case 3008:
					text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_WebErrorConfigurationError");
					goto IL_03FC;
				default:
					switch (eventCode)
					{
					case 4001:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditFormsAuthenticationSuccess");
						goto IL_03FC;
					case 4002:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditMembershipAuthenticationSuccess");
						goto IL_03FC;
					case 4003:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditUrlAuthorizationSuccess");
						goto IL_03FC;
					case 4004:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditFileAuthorizationSuccess");
						goto IL_03FC;
					case 4005:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditFormsAuthenticationFailure");
						goto IL_03FC;
					case 4006:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditMembershipAuthenticationFailure");
						goto IL_03FC;
					case 4007:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditUrlAuthorizationFailure");
						goto IL_03FC;
					case 4008:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditFileAuthorizationFailure");
						goto IL_03FC;
					case 4009:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditInvalidViewStateFailure");
						goto IL_03FC;
					case 4010:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditUnhandledSecurityException");
						goto IL_03FC;
					case 4011:
						text2 = WebBaseEvent.FormatResourceStringWithCache("Webevent_msg_AuditUnhandledAccessException");
						goto IL_03FC;
					}
					break;
				}
			}
			return string.Empty;
			IL_03FC:
			if (text != null)
			{
				text2 = text2 + " " + text;
			}
			return text2;
		}

		// Token: 0x06002530 RID: 9520 RVA: 0x0009FD82 File Offset: 0x0009ED82
		internal static int GetEventArrayDimensionSize(int dim)
		{
			return WebEventCodes.s_eventArrayDimensionSizes[dim];
		}

		// Token: 0x06002531 RID: 9521 RVA: 0x0009FD8B File Offset: 0x0009ED8B
		internal static void GetEventArrayIndexsFromEventCode(int eventCode, out int index0, out int index1)
		{
			index0 = eventCode / 1000 - 1;
			index1 = eventCode - eventCode / 1000 * 1000 - 1;
		}

		// Token: 0x06002532 RID: 9522 RVA: 0x0009FDAC File Offset: 0x0009EDAC
		private static void InitEventArrayDimensions()
		{
			int num = 0;
			int num2 = 5;
			if (num2 > num)
			{
				num = num2;
			}
			num2 = 2;
			if (num2 > num)
			{
				num = num2;
			}
			num2 = 11;
			if (num2 > num)
			{
				num = num2;
			}
			num2 = 11;
			if (num2 > num)
			{
				num = num2;
			}
			num2 = 1;
			if (num2 > num)
			{
				num = num2;
			}
			WebEventCodes.s_eventArrayDimensionSizes[0] = 6;
			WebEventCodes.s_eventArrayDimensionSizes[1] = num;
		}

		// Token: 0x04001CE1 RID: 7393
		public const int InvalidEventCode = -1;

		// Token: 0x04001CE2 RID: 7394
		public const int UndefinedEventCode = 0;

		// Token: 0x04001CE3 RID: 7395
		public const int UndefinedEventDetailCode = 0;

		// Token: 0x04001CE4 RID: 7396
		public const int ApplicationCodeBase = 1000;

		// Token: 0x04001CE5 RID: 7397
		public const int ApplicationStart = 1001;

		// Token: 0x04001CE6 RID: 7398
		public const int ApplicationShutdown = 1002;

		// Token: 0x04001CE7 RID: 7399
		public const int ApplicationCompilationStart = 1003;

		// Token: 0x04001CE8 RID: 7400
		public const int ApplicationCompilationEnd = 1004;

		// Token: 0x04001CE9 RID: 7401
		public const int ApplicationHeartbeat = 1005;

		// Token: 0x04001CEA RID: 7402
		internal const int ApplicationCodeBaseLast = 1005;

		// Token: 0x04001CEB RID: 7403
		public const int RequestCodeBase = 2000;

		// Token: 0x04001CEC RID: 7404
		public const int RequestTransactionComplete = 2001;

		// Token: 0x04001CED RID: 7405
		public const int RequestTransactionAbort = 2002;

		// Token: 0x04001CEE RID: 7406
		internal const int RequestCodeBaseLast = 2002;

		// Token: 0x04001CEF RID: 7407
		public const int ErrorCodeBase = 3000;

		// Token: 0x04001CF0 RID: 7408
		public const int RuntimeErrorRequestAbort = 3001;

		// Token: 0x04001CF1 RID: 7409
		public const int RuntimeErrorViewStateFailure = 3002;

		// Token: 0x04001CF2 RID: 7410
		public const int RuntimeErrorValidationFailure = 3003;

		// Token: 0x04001CF3 RID: 7411
		public const int RuntimeErrorPostTooLarge = 3004;

		// Token: 0x04001CF4 RID: 7412
		public const int RuntimeErrorUnhandledException = 3005;

		// Token: 0x04001CF5 RID: 7413
		public const int WebErrorParserError = 3006;

		// Token: 0x04001CF6 RID: 7414
		public const int WebErrorCompilationError = 3007;

		// Token: 0x04001CF7 RID: 7415
		public const int WebErrorConfigurationError = 3008;

		// Token: 0x04001CF8 RID: 7416
		public const int WebErrorOtherError = 3009;

		// Token: 0x04001CF9 RID: 7417
		public const int WebErrorPropertyDeserializationError = 3010;

		// Token: 0x04001CFA RID: 7418
		public const int WebErrorObjectStateFormatterDeserializationError = 3011;

		// Token: 0x04001CFB RID: 7419
		internal const int ErrorCodeBaseLast = 3011;

		// Token: 0x04001CFC RID: 7420
		public const int AuditCodeBase = 4000;

		// Token: 0x04001CFD RID: 7421
		public const int AuditFormsAuthenticationSuccess = 4001;

		// Token: 0x04001CFE RID: 7422
		public const int AuditMembershipAuthenticationSuccess = 4002;

		// Token: 0x04001CFF RID: 7423
		public const int AuditUrlAuthorizationSuccess = 4003;

		// Token: 0x04001D00 RID: 7424
		public const int AuditFileAuthorizationSuccess = 4004;

		// Token: 0x04001D01 RID: 7425
		public const int AuditFormsAuthenticationFailure = 4005;

		// Token: 0x04001D02 RID: 7426
		public const int AuditMembershipAuthenticationFailure = 4006;

		// Token: 0x04001D03 RID: 7427
		public const int AuditUrlAuthorizationFailure = 4007;

		// Token: 0x04001D04 RID: 7428
		public const int AuditFileAuthorizationFailure = 4008;

		// Token: 0x04001D05 RID: 7429
		public const int AuditInvalidViewStateFailure = 4009;

		// Token: 0x04001D06 RID: 7430
		public const int AuditUnhandledSecurityException = 4010;

		// Token: 0x04001D07 RID: 7431
		public const int AuditUnhandledAccessException = 4011;

		// Token: 0x04001D08 RID: 7432
		internal const int AuditCodeBaseLast = 4011;

		// Token: 0x04001D09 RID: 7433
		public const int MiscCodeBase = 6000;

		// Token: 0x04001D0A RID: 7434
		public const int WebEventProviderInformation = 6001;

		// Token: 0x04001D0B RID: 7435
		internal const int MiscCodeBaseLast = 6001;

		// Token: 0x04001D0C RID: 7436
		internal const int LastCodeBase = 6000;

		// Token: 0x04001D0D RID: 7437
		public const int ApplicationDetailCodeBase = 50000;

		// Token: 0x04001D0E RID: 7438
		public const int ApplicationShutdownUnknown = 50001;

		// Token: 0x04001D0F RID: 7439
		public const int ApplicationShutdownHostingEnvironment = 50002;

		// Token: 0x04001D10 RID: 7440
		public const int ApplicationShutdownChangeInGlobalAsax = 50003;

		// Token: 0x04001D11 RID: 7441
		public const int ApplicationShutdownConfigurationChange = 50004;

		// Token: 0x04001D12 RID: 7442
		public const int ApplicationShutdownUnloadAppDomainCalled = 50005;

		// Token: 0x04001D13 RID: 7443
		public const int ApplicationShutdownChangeInSecurityPolicyFile = 50006;

		// Token: 0x04001D14 RID: 7444
		public const int ApplicationShutdownBinDirChangeOrDirectoryRename = 50007;

		// Token: 0x04001D15 RID: 7445
		public const int ApplicationShutdownBrowsersDirChangeOrDirectoryRename = 50008;

		// Token: 0x04001D16 RID: 7446
		public const int ApplicationShutdownCodeDirChangeOrDirectoryRename = 50009;

		// Token: 0x04001D17 RID: 7447
		public const int ApplicationShutdownResourcesDirChangeOrDirectoryRename = 50010;

		// Token: 0x04001D18 RID: 7448
		public const int ApplicationShutdownIdleTimeout = 50011;

		// Token: 0x04001D19 RID: 7449
		public const int ApplicationShutdownPhysicalApplicationPathChanged = 50012;

		// Token: 0x04001D1A RID: 7450
		public const int ApplicationShutdownHttpRuntimeClose = 50013;

		// Token: 0x04001D1B RID: 7451
		public const int ApplicationShutdownInitializationError = 50014;

		// Token: 0x04001D1C RID: 7452
		public const int ApplicationShutdownMaxRecompilationsReached = 50015;

		// Token: 0x04001D1D RID: 7453
		public const int StateServerConnectionError = 50016;

		// Token: 0x04001D1E RID: 7454
		public const int ApplicationShutdownBuildManagerChange = 50017;

		// Token: 0x04001D1F RID: 7455
		public const int AuditDetailCodeBase = 50200;

		// Token: 0x04001D20 RID: 7456
		public const int InvalidTicketFailure = 50201;

		// Token: 0x04001D21 RID: 7457
		public const int ExpiredTicketFailure = 50202;

		// Token: 0x04001D22 RID: 7458
		public const int InvalidViewStateMac = 50203;

		// Token: 0x04001D23 RID: 7459
		public const int InvalidViewState = 50204;

		// Token: 0x04001D24 RID: 7460
		public const int WebEventDetailCodeBase = 50300;

		// Token: 0x04001D25 RID: 7461
		public const int SqlProviderEventsDropped = 50301;

		// Token: 0x04001D26 RID: 7462
		public const int WebExtendedBase = 100000;

		// Token: 0x04001D27 RID: 7463
		internal static int[] s_eventArrayDimensionSizes = new int[2];
	}
}
