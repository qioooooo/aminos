using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x02000004 RID: 4
	internal sealed class SR
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x0002747C File Offset: 0x0002687C
		private static object InternalSyncObject
		{
			get
			{
				if (SR.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref SR.s_InternalSyncObject, obj, null);
				}
				return SR.s_InternalSyncObject;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000274A8 File Offset: 0x000268A8
		internal SR()
		{
			this.resources = new ResourceManager("Resources", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000274D8 File Offset: 0x000268D8
		private static SR GetLoader()
		{
			if (SR.loader == null)
			{
				lock (SR.InternalSyncObject)
				{
					if (SR.loader == null)
					{
						SR.loader = new SR();
					}
				}
			}
			return SR.loader;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x00027534 File Offset: 0x00026934
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00027544 File Offset: 0x00026944
		public static ResourceManager Resources
		{
			get
			{
				return SR.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x0002755C File Offset: 0x0002695C
		public static string GetString(string name, params object[] args)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			string @string = sr.resources.GetString(name, SR.Culture);
			if (args != null && args.Length > 0)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string text = args[i] as string;
					if (text != null && text.Length > 1024)
					{
						args[i] = text.Substring(0, 1021) + "...";
					}
				}
				return string.Format(CultureInfo.CurrentCulture, @string, args);
			}
			return @string;
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000275E0 File Offset: 0x000269E0
		public static string GetString(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetString(name, SR.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0002760C File Offset: 0x00026A0C
		public static object GetObject(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetObject(name, SR.Culture);
		}

		// Token: 0x04000002 RID: 2
		internal const string ArgumentWrongType = "ArgumentWrongType";

		// Token: 0x04000003 RID: 3
		internal const string BadAsyncResult = "BadAsyncResult";

		// Token: 0x04000004 RID: 4
		internal const string BadResourceManagerId = "BadResourceManagerId";

		// Token: 0x04000005 RID: 5
		internal const string CannotGetPrepareInfo = "CannotGetPrepareInfo";

		// Token: 0x04000006 RID: 6
		internal const string CannotGetTransactionIdentifier = "CannotGetTransactionIdentifier";

		// Token: 0x04000007 RID: 7
		internal const string CannotPromoteSnapshot = "CannotPromoteSnapshot";

		// Token: 0x04000008 RID: 8
		internal const string CannotSetCurrent = "CannotSetCurrent";

		// Token: 0x04000009 RID: 9
		internal const string CannotSupportNodeNameSpecification = "CannotSupportNodeNameSpecification";

		// Token: 0x0400000A RID: 10
		internal const string ConfigInvalidConfigurationValue = "ConfigInvalidConfigurationValue";

		// Token: 0x0400000B RID: 11
		internal const string ConfigNull = "ConfigNull";

		// Token: 0x0400000C RID: 12
		internal const string ConfigDistributedTransactionManagerName = "ConfigDistributedTransactionManagerName";

		// Token: 0x0400000D RID: 13
		internal const string ConfigInvalidTimeSpanValue = "ConfigInvalidTimeSpanValue";

		// Token: 0x0400000E RID: 14
		internal const string ConfigurationSectionNotFound = "ConfigurationSectionNotFound";

		// Token: 0x0400000F RID: 15
		internal const string CurrentDelegateSet = "CurrentDelegateSet";

		// Token: 0x04000010 RID: 16
		internal const string DistributedTransactionManager = "DistributedTransactionManager";

		// Token: 0x04000011 RID: 17
		internal const string DisposeScope = "DisposeScope";

		// Token: 0x04000012 RID: 18
		internal const string DtcTransactionManagerUnavailable = "DtcTransactionManagerUnavailable";

		// Token: 0x04000013 RID: 19
		internal const string DuplicateRecoveryComplete = "DuplicateRecoveryComplete";

		// Token: 0x04000014 RID: 20
		internal const string EnlistmentStateException = "EnlistmentStateException";

		// Token: 0x04000015 RID: 21
		internal const string EsNotSupported = "EsNotSupported";

		// Token: 0x04000016 RID: 22
		internal const string FailedToCreateTraceSource = "FailedToCreateTraceSource";

		// Token: 0x04000017 RID: 23
		internal const string FailedToInitializeTraceSource = "FailedToInitializeTraceSource";

		// Token: 0x04000018 RID: 24
		internal const string FailedToTraceEvent = "FailedToTraceEvent";

		// Token: 0x04000019 RID: 25
		internal const string InternalError = "InternalError";

		// Token: 0x0400001A RID: 26
		internal const string InvalidArgument = "InvalidArgument";

		// Token: 0x0400001B RID: 27
		internal const string InvalidRecoveryInformation = "InvalidRecoveryInformation";

		// Token: 0x0400001C RID: 28
		internal const string InvalidScopeThread = "InvalidScopeThread";

		// Token: 0x0400001D RID: 29
		internal const string NetworkTransactionsDisabled = "NetworkTransactionsDisabled";

		// Token: 0x0400001E RID: 30
		internal const string OletxEnlistmentUnexpectedTransactionStatus = "OletxEnlistmentUnexpectedTransactionStatus";

		// Token: 0x0400001F RID: 31
		internal const string OletxTooManyEnlistments = "OletxTooManyEnlistments";

		// Token: 0x04000020 RID: 32
		internal const string OnlySupportedOnWinNT = "OnlySupportedOnWinNT";

		// Token: 0x04000021 RID: 33
		internal const string PrepareInfo = "PrepareInfo";

		// Token: 0x04000022 RID: 34
		internal const string PromotionFailed = "PromotionFailed";

		// Token: 0x04000023 RID: 35
		internal const string PromotedReturnedInvalidValue = "PromotedReturnedInvalidValue";

		// Token: 0x04000024 RID: 36
		internal const string PromotedTransactionExists = "PromotedTransactionExists";

		// Token: 0x04000025 RID: 37
		internal const string ProxyCannotSupportMultipleNodeNames = "ProxyCannotSupportMultipleNodeNames";

		// Token: 0x04000026 RID: 38
		internal const string ReenlistAfterRecoveryComplete = "ReenlistAfterRecoveryComplete";

		// Token: 0x04000027 RID: 39
		internal const string ResourceManagerIdDoesNotMatchRecoveryInformation = "ResourceManagerIdDoesNotMatchRecoveryInformation";

		// Token: 0x04000028 RID: 40
		internal const string TooLate = "TooLate";

		// Token: 0x04000029 RID: 41
		internal const string TraceActivityIdSet = "TraceActivityIdSet";

		// Token: 0x0400002A RID: 42
		internal const string TraceCloneCreated = "TraceCloneCreated";

		// Token: 0x0400002B RID: 43
		internal const string TraceConfiguredDefaultTimeoutAdjusted = "TraceConfiguredDefaultTimeoutAdjusted";

		// Token: 0x0400002C RID: 44
		internal const string TraceDependentCloneComplete = "TraceDependentCloneComplete";

		// Token: 0x0400002D RID: 45
		internal const string TraceDependentCloneCreated = "TraceDependentCloneCreated";

		// Token: 0x0400002E RID: 46
		internal const string TraceEnlistment = "TraceEnlistment";

		// Token: 0x0400002F RID: 47
		internal const string TraceEnlistmentCallbackNegative = "TraceEnlistmentCallbackNegative";

		// Token: 0x04000030 RID: 48
		internal const string TraceEnlistmentCallbackPositive = "TraceEnlistmentCallbackPositive";

		// Token: 0x04000031 RID: 49
		internal const string TraceEnlistmentNotificationCall = "TraceEnlistmentNotificationCall";

		// Token: 0x04000032 RID: 50
		internal const string TraceExceptionConsumed = "TraceExceptionConsumed";

		// Token: 0x04000033 RID: 51
		internal const string TraceInternalError = "TraceInternalError";

		// Token: 0x04000034 RID: 52
		internal const string TraceInvalidOperationException = "TraceInvalidOperationException";

		// Token: 0x04000035 RID: 53
		internal const string TraceMethodEntered = "TraceMethodEntered";

		// Token: 0x04000036 RID: 54
		internal const string TraceMethodExited = "TraceMethodExited";

		// Token: 0x04000037 RID: 55
		internal const string TraceNewActivityIdIssued = "TraceNewActivityIdIssued";

		// Token: 0x04000038 RID: 56
		internal const string TraceRecoveryComplete = "TraceRecoveryComplete";

		// Token: 0x04000039 RID: 57
		internal const string TraceReenlist = "TraceReenlist";

		// Token: 0x0400003A RID: 58
		internal const string TraceSourceBase = "TraceSourceBase";

		// Token: 0x0400003B RID: 59
		internal const string TraceSourceLtm = "TraceSourceLtm";

		// Token: 0x0400003C RID: 60
		internal const string TraceSourceOletx = "TraceSourceOletx";

		// Token: 0x0400003D RID: 61
		internal const string TraceTransactionAborted = "TraceTransactionAborted";

		// Token: 0x0400003E RID: 62
		internal const string TraceTransactionCommitCalled = "TraceTransactionCommitCalled";

		// Token: 0x0400003F RID: 63
		internal const string TraceTransactionCommitted = "TraceTransactionCommitted";

		// Token: 0x04000040 RID: 64
		internal const string TraceTransactionCreated = "TraceTransactionCreated";

		// Token: 0x04000041 RID: 65
		internal const string TraceTransactionDeserialized = "TraceTransactionDeserialized";

		// Token: 0x04000042 RID: 66
		internal const string TraceTransactionException = "TraceTransactionException";

		// Token: 0x04000043 RID: 67
		internal const string TraceTransactionInDoubt = "TraceTransactionInDoubt";

		// Token: 0x04000044 RID: 68
		internal const string TraceTransactionManagerCreated = "TraceTransactionManagerCreated";

		// Token: 0x04000045 RID: 69
		internal const string TraceTransactionPromoted = "TraceTransactionPromoted";

		// Token: 0x04000046 RID: 70
		internal const string TraceTransactionRollbackCalled = "TraceTransactionRollbackCalled";

		// Token: 0x04000047 RID: 71
		internal const string TraceTransactionScopeCreated = "TraceTransactionScopeCreated";

		// Token: 0x04000048 RID: 72
		internal const string TraceTransactionScopeCurrentTransactionChanged = "TraceTransactionScopeCurrentTransactionChanged";

		// Token: 0x04000049 RID: 73
		internal const string TraceTransactionScopeDisposed = "TraceTransactionScopeDisposed";

		// Token: 0x0400004A RID: 74
		internal const string TraceTransactionScopeIncomplete = "TraceTransactionScopeIncomplete";

		// Token: 0x0400004B RID: 75
		internal const string TraceTransactionScopeNestedIncorrectly = "TraceTransactionScopeNestedIncorrectly";

		// Token: 0x0400004C RID: 76
		internal const string TraceTransactionScopeTimeout = "TraceTransactionScopeTimeout";

		// Token: 0x0400004D RID: 77
		internal const string TraceTransactionSerialized = "TraceTransactionSerialized";

		// Token: 0x0400004E RID: 78
		internal const string TraceTransactionTimeout = "TraceTransactionTimeout";

		// Token: 0x0400004F RID: 79
		internal const string TraceUnhandledException = "TraceUnhandledException";

		// Token: 0x04000050 RID: 80
		internal const string TransactionAborted = "TransactionAborted";

		// Token: 0x04000051 RID: 81
		internal const string TransactionAlreadyCompleted = "TransactionAlreadyCompleted";

		// Token: 0x04000052 RID: 82
		internal const string TransactionAlreadyOver = "TransactionAlreadyOver";

		// Token: 0x04000053 RID: 83
		internal const string TransactionIndoubt = "TransactionIndoubt";

		// Token: 0x04000054 RID: 84
		internal const string TransactionManagerCommunicationException = "TransactionManagerCommunicationException";

		// Token: 0x04000055 RID: 85
		internal const string TransactionScopeComplete = "TransactionScopeComplete";

		// Token: 0x04000056 RID: 86
		internal const string TransactionScopeIncorrectCurrent = "TransactionScopeIncorrectCurrent";

		// Token: 0x04000057 RID: 87
		internal const string TransactionScopeInvalidNesting = "TransactionScopeInvalidNesting";

		// Token: 0x04000058 RID: 88
		internal const string TransactionScopeIsolationLevelDifferentFromTransaction = "TransactionScopeIsolationLevelDifferentFromTransaction";

		// Token: 0x04000059 RID: 89
		internal const string TransactionScopeTimerObjectInvalid = "TransactionScopeTimerObjectInvalid";

		// Token: 0x0400005A RID: 90
		internal const string TransactionStateException = "TransactionStateException";

		// Token: 0x0400005B RID: 91
		internal const string UnableToDeserializeTransaction = "UnableToDeserializeTransaction";

		// Token: 0x0400005C RID: 92
		internal const string UnableToDeserializeTransactionInternalError = "UnableToDeserializeTransactionInternalError";

		// Token: 0x0400005D RID: 93
		internal const string UnableToGetNotificationShimFactory = "UnableToGetNotificationShimFactory";

		// Token: 0x0400005E RID: 94
		internal const string UnexpectedTransactionManagerConfigurationValue = "UnexpectedTransactionManagerConfigurationValue";

		// Token: 0x0400005F RID: 95
		internal const string UnexpectedFailureOfThreadPool = "UnexpectedFailureOfThreadPool";

		// Token: 0x04000060 RID: 96
		internal const string UnexpectedTimerFailure = "UnexpectedTimerFailure";

		// Token: 0x04000061 RID: 97
		internal const string UnrecognizedRecoveryInformation = "UnrecognizedRecoveryInformation";

		// Token: 0x04000062 RID: 98
		internal const string VolEnlistNoRecoveryInfo = "VolEnlistNoRecoveryInfo";

		// Token: 0x04000063 RID: 99
		internal const string CannotAddToClosedDocument = "CannotAddToClosedDocument";

		// Token: 0x04000064 RID: 100
		internal const string DocumentAlreadyClosed = "DocumentAlreadyClosed";

		// Token: 0x04000065 RID: 101
		internal const string EventLogValue = "EventLogValue";

		// Token: 0x04000066 RID: 102
		internal const string EventLogEventIdValue = "EventLogEventIdValue";

		// Token: 0x04000067 RID: 103
		internal const string EventLogExceptionValue = "EventLogExceptionValue";

		// Token: 0x04000068 RID: 104
		internal const string EventLogSourceValue = "EventLogSourceValue";

		// Token: 0x04000069 RID: 105
		internal const string EventLogTraceValue = "EventLogTraceValue";

		// Token: 0x0400006A RID: 106
		internal const string NamedActivity = "NamedActivity";

		// Token: 0x0400006B RID: 107
		internal const string OperationInvalidOnAnEmptyDocument = "OperationInvalidOnAnEmptyDocument";

		// Token: 0x0400006C RID: 108
		internal const string TextNodeAlreadyPopulated = "TextNodeAlreadyPopulated";

		// Token: 0x0400006D RID: 109
		internal const string ThrowingException = "ThrowingException";

		// Token: 0x0400006E RID: 110
		internal const string TracingException = "TracingException";

		// Token: 0x0400006F RID: 111
		internal const string TraceCodeAppDomainUnloading = "TraceCodeAppDomainUnloading";

		// Token: 0x04000070 RID: 112
		internal const string TraceFailure = "TraceFailure";

		// Token: 0x04000071 RID: 113
		internal const string UnhandledException = "UnhandledException";

		// Token: 0x04000072 RID: 114
		private static SR loader;

		// Token: 0x04000073 RID: 115
		private ResourceManager resources;

		// Token: 0x04000074 RID: 116
		private static object s_InternalSyncObject;
	}
}
