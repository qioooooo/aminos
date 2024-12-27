using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Permissions;
using System.Threading;
using System.Transactions.Configuration;
using System.Transactions.Diagnostics;
using System.Transactions.Oletx;

namespace System.Transactions
{
	// Token: 0x02000069 RID: 105
	public static class TransactionManager
	{
		// Token: 0x14000002 RID: 2
		// (add) Token: 0x060002DE RID: 734 RVA: 0x00031124 File Offset: 0x00030524
		// (remove) Token: 0x060002DF RID: 735 RVA: 0x0003118C File Offset: 0x0003058C
		public static event TransactionStartedEventHandler DistributedTransactionStarted
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			add
			{
				if (!TransactionManager._platformValidated)
				{
					TransactionManager.ValidatePlatform();
				}
				lock (TransactionManager.ClassSyncObject)
				{
					TransactionManager.distributedTransactionStartedDelegate = (TransactionStartedEventHandler)Delegate.Combine(TransactionManager.distributedTransactionStartedDelegate, value);
					if (value != null)
					{
						TransactionManager.ProcessExistingTransactions(value);
					}
				}
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			remove
			{
				if (!TransactionManager._platformValidated)
				{
					TransactionManager.ValidatePlatform();
				}
				lock (TransactionManager.ClassSyncObject)
				{
					TransactionManager.distributedTransactionStartedDelegate = (TransactionStartedEventHandler)Delegate.Remove(TransactionManager.distributedTransactionStartedDelegate, value);
				}
			}
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x000311EC File Offset: 0x000305EC
		internal static void ProcessExistingTransactions(TransactionStartedEventHandler eventHandler)
		{
			lock (TransactionManager.PromotedTransactionTable)
			{
				foreach (object obj in TransactionManager.PromotedTransactionTable)
				{
					WeakReference weakReference = (WeakReference)((DictionaryEntry)obj).Value;
					Transaction transaction = (Transaction)weakReference.Target;
					if (transaction != null)
					{
						TransactionEventArgs transactionEventArgs = new TransactionEventArgs();
						transactionEventArgs.transaction = transaction.InternalClone();
						eventHandler(transactionEventArgs.transaction, transactionEventArgs);
					}
				}
			}
		}

		// Token: 0x060002E1 RID: 737 RVA: 0x000312BC File Offset: 0x000306BC
		internal static void FireDistributedTransactionStarted(Transaction transaction)
		{
			TransactionStartedEventHandler transactionStartedEventHandler = null;
			lock (TransactionManager.ClassSyncObject)
			{
				transactionStartedEventHandler = TransactionManager.distributedTransactionStartedDelegate;
			}
			if (transactionStartedEventHandler != null)
			{
				TransactionEventArgs transactionEventArgs = new TransactionEventArgs();
				transactionEventArgs.transaction = transaction.InternalClone();
				transactionStartedEventHandler(transactionEventArgs.transaction, transactionEventArgs);
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x00031324 File Offset: 0x00030724
		// (set) Token: 0x060002E3 RID: 739 RVA: 0x00031344 File Offset: 0x00030744
		public static HostCurrentTransactionCallback HostCurrentCallback
		{
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			get
			{
				if (!TransactionManager._platformValidated)
				{
					TransactionManager.ValidatePlatform();
				}
				return TransactionManager.currentDelegate;
			}
			[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
			set
			{
				if (!TransactionManager._platformValidated)
				{
					TransactionManager.ValidatePlatform();
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				lock (TransactionManager.ClassSyncObject)
				{
					if (TransactionManager.currentDelegateSet)
					{
						throw new InvalidOperationException(SR.GetString("CurrentDelegateSet"));
					}
					TransactionManager.currentDelegateSet = true;
				}
				TransactionManager.currentDelegate = value;
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x000313C0 File Offset: 0x000307C0
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static Enlistment Reenlist(Guid resourceManagerIdentifier, byte[] recoveryInformation, IEnlistmentNotification enlistmentNotification)
		{
			if (resourceManagerIdentifier == Guid.Empty)
			{
				throw new ArgumentException(SR.GetString("BadResourceManagerId"), "resourceManagerIdentifier");
			}
			if (recoveryInformation == null)
			{
				throw new ArgumentNullException("recoveryInformation");
			}
			if (enlistmentNotification == null)
			{
				throw new ArgumentNullException("enlistmentNotification");
			}
			if (!TransactionManager._platformValidated)
			{
				TransactionManager.ValidatePlatform();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.Reenlist");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.ReenlistTraceRecord.Trace(SR.GetString("TraceSourceBase"), resourceManagerIdentifier);
			}
			MemoryStream memoryStream = new MemoryStream(recoveryInformation);
			string text = null;
			byte[] array = null;
			try
			{
				BinaryReader binaryReader = new BinaryReader(memoryStream);
				int num = binaryReader.ReadInt32();
				if (num != 1)
				{
					if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
					{
						global::System.Transactions.Diagnostics.TransactionExceptionTraceRecord.Trace(SR.GetString("TraceSourceBase"), SR.GetString("UnrecognizedRecoveryInformation"));
					}
					throw new ArgumentException(SR.GetString("UnrecognizedRecoveryInformation"), "recoveryInformation");
				}
				text = binaryReader.ReadString();
				array = binaryReader.ReadBytes(recoveryInformation.Length - checked((int)memoryStream.Position));
			}
			catch (EndOfStreamException ex)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
				{
					global::System.Transactions.Diagnostics.TransactionExceptionTraceRecord.Trace(SR.GetString("TraceSourceBase"), SR.GetString("UnrecognizedRecoveryInformation"));
				}
				throw new ArgumentException(SR.GetString("UnrecognizedRecoveryInformation"), "recoveryInformation", ex);
			}
			catch (FormatException ex2)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Error)
				{
					global::System.Transactions.Diagnostics.TransactionExceptionTraceRecord.Trace(SR.GetString("TraceSourceBase"), SR.GetString("UnrecognizedRecoveryInformation"));
				}
				throw new ArgumentException(SR.GetString("UnrecognizedRecoveryInformation"), "recoveryInformation", ex2);
			}
			finally
			{
				memoryStream.Close();
			}
			global::System.Transactions.Oletx.OletxTransactionManager oletxTransactionManager = TransactionManager.CheckTransactionManager(text);
			object obj = new object();
			Enlistment enlistment = new Enlistment(enlistmentNotification, obj);
			EnlistmentState._EnlistmentStatePromoted.EnterState(enlistment.InternalEnlistment);
			enlistment.InternalEnlistment.PromotedEnlistment = oletxTransactionManager.ReenlistTransaction(resourceManagerIdentifier, array, (RecoveringInternalEnlistment)enlistment.InternalEnlistment);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.Reenlist");
			}
			return enlistment;
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x000315F4 File Offset: 0x000309F4
		private static global::System.Transactions.Oletx.OletxTransactionManager CheckTransactionManager(string nodeName)
		{
			global::System.Transactions.Oletx.OletxTransactionManager oletxTransactionManager = TransactionManager.DistributedTransactionManager;
			if ((oletxTransactionManager.NodeName != null || (nodeName != null && nodeName.Length != 0)) && (oletxTransactionManager.NodeName == null || !oletxTransactionManager.NodeName.Equals(nodeName)))
			{
				throw new ArgumentException(SR.GetString("InvalidRecoveryInformation"), "recoveryInformation");
			}
			return oletxTransactionManager;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x00031648 File Offset: 0x00030A48
		[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
		public static void RecoveryComplete(Guid resourceManagerIdentifier)
		{
			if (resourceManagerIdentifier == Guid.Empty)
			{
				throw new ArgumentException(SR.GetString("BadResourceManagerId"), "resourceManagerIdentifier");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.RecoveryComplete");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.RecoveryCompleteTraceRecord.Trace(SR.GetString("TraceSourceBase"), resourceManagerIdentifier);
			}
			TransactionManager.DistributedTransactionManager.ResourceManagerRecoveryComplete(resourceManagerIdentifier);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.RecoveryComplete");
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x060002E7 RID: 743 RVA: 0x000316D0 File Offset: 0x00030AD0
		private static object ClassSyncObject
		{
			get
			{
				if (TransactionManager.classSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref TransactionManager.classSyncObject, obj, null);
				}
				return TransactionManager.classSyncObject;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x060002E8 RID: 744 RVA: 0x000316FC File Offset: 0x00030AFC
		internal static IsolationLevel DefaultIsolationLevel
		{
			get
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.get_DefaultIsolationLevel");
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.get_DefaultIsolationLevel");
				}
				return IsolationLevel.Serializable;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x060002E9 RID: 745 RVA: 0x0003173C File Offset: 0x00030B3C
		private static global::System.Transactions.Configuration.DefaultSettingsSection DefaultSettings
		{
			get
			{
				if (TransactionManager.defaultSettings == null)
				{
					TransactionManager.defaultSettings = global::System.Transactions.Configuration.DefaultSettingsSection.GetSection();
				}
				return TransactionManager.defaultSettings;
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x060002EA RID: 746 RVA: 0x00031760 File Offset: 0x00030B60
		private static global::System.Transactions.Configuration.MachineSettingsSection MachineSettings
		{
			get
			{
				if (TransactionManager.machineSettings == null)
				{
					TransactionManager.machineSettings = global::System.Transactions.Configuration.MachineSettingsSection.GetSection();
				}
				return TransactionManager.machineSettings;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x060002EB RID: 747 RVA: 0x00031784 File Offset: 0x00030B84
		public static TimeSpan DefaultTimeout
		{
			get
			{
				if (!TransactionManager._platformValidated)
				{
					TransactionManager.ValidatePlatform();
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.get_DefaultTimeout");
				}
				if (!TransactionManager._defaultTimeoutValidated)
				{
					TransactionManager._defaultTimeout = TransactionManager.ValidateTimeout(TransactionManager.DefaultSettings.Timeout);
					if (TransactionManager._defaultTimeout != TransactionManager.DefaultSettings.Timeout && global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
					{
						global::System.Transactions.Diagnostics.ConfiguredDefaultTimeoutAdjustedTraceRecord.Trace(SR.GetString("TraceSourceBase"));
					}
					TransactionManager._defaultTimeoutValidated = true;
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.get_DefaultTimeout");
				}
				return TransactionManager._defaultTimeout;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x060002EC RID: 748 RVA: 0x00031828 File Offset: 0x00030C28
		public static TimeSpan MaximumTimeout
		{
			get
			{
				if (!TransactionManager._platformValidated)
				{
					TransactionManager.ValidatePlatform();
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.get_DefaultMaximumTimeout");
				}
				if (!TransactionManager._cachedMaxTimeout)
				{
					lock (TransactionManager.ClassSyncObject)
					{
						if (!TransactionManager._cachedMaxTimeout)
						{
							TimeSpan maxTimeout = TransactionManager.MachineSettings.MaxTimeout;
							Thread.MemoryBarrier();
							TransactionManager._maximumTimeout = maxTimeout;
							TransactionManager._cachedMaxTimeout = true;
						}
					}
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.get_DefaultMaximumTimeout");
				}
				return TransactionManager._maximumTimeout;
			}
		}

		// Token: 0x060002ED RID: 749 RVA: 0x000318D8 File Offset: 0x00030CD8
		internal static byte[] GetRecoveryInformation(string startupInfo, byte[] resourceManagerRecoveryInformation)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.GetRecoveryInformation");
			}
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = null;
			try
			{
				BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
				binaryWriter.Write(1);
				if (startupInfo != null)
				{
					binaryWriter.Write(startupInfo);
				}
				else
				{
					binaryWriter.Write("");
				}
				binaryWriter.Write(resourceManagerRecoveryInformation);
				binaryWriter.Flush();
				array = memoryStream.ToArray();
			}
			finally
			{
				memoryStream.Close();
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceBase"), "TransactionManager.GetRecoveryInformation");
			}
			return array;
		}

		// Token: 0x060002EE RID: 750 RVA: 0x00031984 File Offset: 0x00030D84
		internal static byte[] ConvertToByteArray(object thingToConvert)
		{
			MemoryStream memoryStream = new MemoryStream();
			byte[] array = null;
			try
			{
				IFormatter formatter = new BinaryFormatter();
				formatter.Serialize(memoryStream, thingToConvert);
				array = new byte[memoryStream.Length];
				memoryStream.Position = 0L;
				memoryStream.Read(array, 0, Convert.ToInt32(memoryStream.Length, CultureInfo.InvariantCulture));
			}
			finally
			{
				memoryStream.Close();
			}
			return array;
		}

		// Token: 0x060002EF RID: 751 RVA: 0x00031A00 File Offset: 0x00030E00
		internal static void ValidateIsolationLevel(IsolationLevel transactionIsolationLevel)
		{
			switch (transactionIsolationLevel)
			{
			case IsolationLevel.Serializable:
			case IsolationLevel.RepeatableRead:
			case IsolationLevel.ReadCommitted:
			case IsolationLevel.ReadUncommitted:
			case IsolationLevel.Snapshot:
			case IsolationLevel.Chaos:
			case IsolationLevel.Unspecified:
				return;
			default:
				throw new ArgumentOutOfRangeException("transactionIsolationLevel");
			}
		}

		// Token: 0x060002F0 RID: 752 RVA: 0x00031A3C File Offset: 0x00030E3C
		internal static TimeSpan ValidateTimeout(TimeSpan transactionTimeout)
		{
			if (transactionTimeout < TimeSpan.Zero)
			{
				throw new ArgumentOutOfRangeException("transactionTimeout");
			}
			if (TransactionManager.MaximumTimeout != TimeSpan.Zero && (transactionTimeout > TransactionManager.MaximumTimeout || transactionTimeout == TimeSpan.Zero))
			{
				return TransactionManager.MaximumTimeout;
			}
			return transactionTimeout;
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x00031A94 File Offset: 0x00030E94
		internal static Transaction FindPromotedTransaction(Guid transactionIdentifier)
		{
			Hashtable hashtable = TransactionManager.PromotedTransactionTable;
			WeakReference weakReference = (WeakReference)hashtable[transactionIdentifier];
			if (weakReference != null)
			{
				Transaction transaction = weakReference.Target as Transaction;
				if (null != transaction)
				{
					return transaction.InternalClone();
				}
				lock (hashtable)
				{
					hashtable.Remove(transactionIdentifier);
				}
			}
			return null;
		}

		// Token: 0x060002F2 RID: 754 RVA: 0x00031B14 File Offset: 0x00030F14
		internal static Transaction FindOrCreatePromotedTransaction(Guid transactionIdentifier, global::System.Transactions.Oletx.OletxTransaction oletx)
		{
			Transaction transaction = null;
			Hashtable hashtable = TransactionManager.PromotedTransactionTable;
			lock (hashtable)
			{
				WeakReference weakReference = (WeakReference)hashtable[transactionIdentifier];
				if (weakReference != null)
				{
					transaction = weakReference.Target as Transaction;
					if (null != transaction)
					{
						oletx.Dispose();
						return transaction.InternalClone();
					}
					lock (hashtable)
					{
						hashtable.Remove(transactionIdentifier);
					}
				}
				transaction = new Transaction(oletx);
				transaction.internalTransaction.finalizedObject = new FinalizedObject(transaction.internalTransaction, oletx.Identifier);
				weakReference = new WeakReference(transaction, false);
				hashtable[oletx.Identifier] = weakReference;
			}
			oletx.savedLtmPromotedTransaction = transaction;
			TransactionManager.FireDistributedTransactionStarted(transaction);
			return transaction;
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x060002F3 RID: 755 RVA: 0x00031C18 File Offset: 0x00031018
		internal static Hashtable PromotedTransactionTable
		{
			get
			{
				if (TransactionManager.promotedTransactionTable == null)
				{
					lock (TransactionManager.ClassSyncObject)
					{
						if (TransactionManager.promotedTransactionTable == null)
						{
							Hashtable hashtable = new Hashtable(100);
							Thread.MemoryBarrier();
							TransactionManager.promotedTransactionTable = hashtable;
						}
					}
				}
				return TransactionManager.promotedTransactionTable;
			}
		}

		// Token: 0x1700006E RID: 110
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x00031C7C File Offset: 0x0003107C
		internal static TransactionTable TransactionTable
		{
			get
			{
				if (TransactionManager.transactionTable == null)
				{
					lock (TransactionManager.ClassSyncObject)
					{
						if (TransactionManager.transactionTable == null)
						{
							TransactionTable transactionTable = new TransactionTable();
							Thread.MemoryBarrier();
							TransactionManager.transactionTable = transactionTable;
						}
					}
				}
				return TransactionManager.transactionTable;
			}
		}

		// Token: 0x1700006F RID: 111
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x00031CE0 File Offset: 0x000310E0
		internal static global::System.Transactions.Oletx.OletxTransactionManager DistributedTransactionManager
		{
			get
			{
				if (TransactionManager.distributedTransactionManager == null)
				{
					lock (TransactionManager.ClassSyncObject)
					{
						if (TransactionManager.distributedTransactionManager == null)
						{
							global::System.Transactions.Oletx.OletxTransactionManager oletxTransactionManager = new global::System.Transactions.Oletx.OletxTransactionManager(TransactionManager.DefaultSettings.DistributedTransactionManagerName);
							Thread.MemoryBarrier();
							TransactionManager.distributedTransactionManager = oletxTransactionManager;
						}
					}
				}
				return TransactionManager.distributedTransactionManager;
			}
		}

		// Token: 0x060002F6 RID: 758 RVA: 0x00031D4C File Offset: 0x0003114C
		internal static void ValidatePlatform()
		{
			if (PlatformID.Win32NT != Environment.OSVersion.Platform)
			{
				throw new PlatformNotSupportedException(SR.GetString("OnlySupportedOnWinNT"));
			}
			TransactionManager._platformValidated = true;
		}

		// Token: 0x04000117 RID: 279
		private const int recoveryInformationVersion1 = 1;

		// Token: 0x04000118 RID: 280
		private const int currentRecoveryVersion = 1;

		// Token: 0x04000119 RID: 281
		internal static bool _platformValidated;

		// Token: 0x0400011A RID: 282
		private static Hashtable promotedTransactionTable;

		// Token: 0x0400011B RID: 283
		private static TransactionTable transactionTable;

		// Token: 0x0400011C RID: 284
		private static TransactionStartedEventHandler distributedTransactionStartedDelegate;

		// Token: 0x0400011D RID: 285
		internal static HostCurrentTransactionCallback currentDelegate;

		// Token: 0x0400011E RID: 286
		internal static bool currentDelegateSet;

		// Token: 0x0400011F RID: 287
		private static object classSyncObject;

		// Token: 0x04000120 RID: 288
		private static global::System.Transactions.Configuration.DefaultSettingsSection defaultSettings;

		// Token: 0x04000121 RID: 289
		private static global::System.Transactions.Configuration.MachineSettingsSection machineSettings;

		// Token: 0x04000122 RID: 290
		private static bool _defaultTimeoutValidated;

		// Token: 0x04000123 RID: 291
		private static TimeSpan _defaultTimeout;

		// Token: 0x04000124 RID: 292
		private static bool _cachedMaxTimeout;

		// Token: 0x04000125 RID: 293
		private static TimeSpan _maximumTimeout;

		// Token: 0x04000126 RID: 294
		internal static global::System.Transactions.Oletx.OletxTransactionManager distributedTransactionManager;
	}
}
