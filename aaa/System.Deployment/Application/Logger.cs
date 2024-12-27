using System;
using System.Collections;
using System.Deployment.Application.Manifest;
using System.Deployment.Internal.Isolation;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading;
using Microsoft.Win32;

namespace System.Deployment.Application
{
	// Token: 0x02000065 RID: 101
	internal class Logger
	{
		// Token: 0x170000CF RID: 207
		// (get) Token: 0x06000310 RID: 784 RVA: 0x0001189F File Offset: 0x0001089F
		protected Logger.TransactionSection Transactions
		{
			get
			{
				return this._transactions;
			}
		}

		// Token: 0x170000D0 RID: 208
		// (get) Token: 0x06000311 RID: 785 RVA: 0x000118A7 File Offset: 0x000108A7
		protected Logger.ErrorSection Errors
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x06000312 RID: 786 RVA: 0x000118AF File Offset: 0x000108AF
		protected Logger.WarningSection Warnings
		{
			get
			{
				return this._warnings;
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x06000313 RID: 787 RVA: 0x000118B7 File Offset: 0x000108B7
		protected Logger.PhaseSection Phases
		{
			get
			{
				return this._phases;
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x06000314 RID: 788 RVA: 0x000118BF File Offset: 0x000108BF
		protected Logger.SourceSection Sources
		{
			get
			{
				return this._sources;
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x06000315 RID: 789 RVA: 0x000118C7 File Offset: 0x000108C7
		protected Logger.IdentitySection Identities
		{
			get
			{
				return this._identities;
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x06000316 RID: 790 RVA: 0x000118CF File Offset: 0x000108CF
		protected Logger.SummarySection Summary
		{
			get
			{
				return this._summary;
			}
		}

		// Token: 0x06000317 RID: 791 RVA: 0x000118D8 File Offset: 0x000108D8
		protected Logger()
		{
		}

		// Token: 0x170000D6 RID: 214
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00011943 File Offset: 0x00010943
		protected Logger.LogIdentity Identity
		{
			get
			{
				return this._logIdentity;
			}
		}

		// Token: 0x06000319 RID: 793 RVA: 0x0001194C File Offset: 0x0001094C
		protected string GetRegitsryBasedLogFilePath()
		{
			string text = null;
			try
			{
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\Software\\Microsoft\\Windows\\CurrentVersion\\Deployment"))
				{
					if (registryKey != null)
					{
						text = registryKey.GetValue("LogFilePath") as string;
					}
				}
			}
			catch (ArgumentException)
			{
			}
			catch (ObjectDisposedException)
			{
			}
			return text;
		}

		// Token: 0x0600031A RID: 794 RVA: 0x000119BC File Offset: 0x000109BC
		protected string GetWinInetBasedLogFilePath()
		{
			string text2;
			try
			{
				string text = "System_Deployment_Log_";
				if (this.Identities.DeploymentIdentity != null)
				{
					text += this.Identities.DeploymentIdentity.KeyForm;
				}
				text = string.Format(CultureInfo.InvariantCulture, "{0}_{1}", new object[]
				{
					text,
					this.Identity.ToString()
				});
				StringBuilder stringBuilder = new StringBuilder(261);
				if (!NativeMethods.CreateUrlCacheEntry(text, 0, "log", stringBuilder, 0))
				{
					text2 = null;
				}
				else
				{
					this._urlName = text;
					text2 = stringBuilder.ToString();
				}
			}
			catch (COMException)
			{
				text2 = null;
			}
			catch (SEHException)
			{
				text2 = null;
			}
			catch (FormatException)
			{
				text2 = null;
			}
			return text2;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x0600031B RID: 795 RVA: 0x00011A84 File Offset: 0x00010A84
		protected string LogFilePath
		{
			get
			{
				if (this._logFilePath == null)
				{
					this._logFilePath = this.GetRegitsryBasedLogFilePath();
					if (this._logFilePath == null)
					{
						this._logFilePath = this.GetWinInetBasedLogFilePath();
						if (this._logFilePath != null)
						{
							this._logFileLocation = Logger.LogFileLocation.WinInetCache;
						}
					}
					else
					{
						this._logFileLocation = Logger.LogFileLocation.RegistryBased;
					}
				}
				return this._logFilePath;
			}
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00011AD8 File Offset: 0x00010AD8
		protected FileStream CreateLogFileStream()
		{
			FileStream fileStream = null;
			string logFilePath = this.LogFilePath;
			for (uint num = 0U; num < 1000U; num += 1U)
			{
				try
				{
					if (this._logFileLocation == Logger.LogFileLocation.RegistryBased)
					{
						fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.None);
					}
					else
					{
						fileStream = new FileStream(logFilePath, FileMode.Create, FileAccess.Write, FileShare.None);
					}
					break;
				}
				catch (IOException)
				{
					if (num == 1000U)
					{
						throw;
					}
				}
				Thread.Sleep(20);
			}
			return fileStream;
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x0600031D RID: 797 RVA: 0x00011B44 File Offset: 0x00010B44
		protected static Encoding LogFileEncoding
		{
			get
			{
				if (Logger._logFileEncoding == null)
				{
					Encoding encoding;
					if (PlatformSpecific.OnWin9x)
					{
						encoding = Encoding.Default;
					}
					else
					{
						encoding = Encoding.Unicode;
					}
					Interlocked.CompareExchange(ref Logger._logFileEncoding, encoding, null);
				}
				return (Encoding)Logger._logFileEncoding;
			}
		}

		// Token: 0x0600031E RID: 798 RVA: 0x00011B88 File Offset: 0x00010B88
		protected bool FlushLogs()
		{
			FileStream fileStream = null;
			try
			{
				fileStream = this.CreateLogFileStream();
				if (fileStream == null)
				{
					return false;
				}
			}
			catch (IOException)
			{
				return false;
			}
			catch (SecurityException)
			{
				return false;
			}
			catch (UnauthorizedAccessException)
			{
				return false;
			}
			StreamWriter streamWriter = new StreamWriter(fileStream, Logger.LogFileEncoding);
			streamWriter.WriteLine(Logger.Header);
			streamWriter.Write(this.Sources);
			streamWriter.Write(this.Identities);
			streamWriter.Write(this.Summary);
			streamWriter.WriteLine(this.Errors.ErrorSummary);
			streamWriter.WriteLine(this.Transactions.FailureSummary);
			streamWriter.WriteLine(this.Warnings);
			streamWriter.WriteLine(this.Phases);
			streamWriter.WriteLine(this.Errors);
			streamWriter.WriteLine(this.Transactions);
			streamWriter.Close();
			fileStream.Close();
			return true;
		}

		// Token: 0x0600031F RID: 799 RVA: 0x00011C88 File Offset: 0x00010C88
		protected void EndLogOperation()
		{
			if (!this.FlushLogs())
			{
				return;
			}
			if (this._logFileLocation == Logger.LogFileLocation.WinInetCache)
			{
				NativeMethods.CommitUrlCacheEntry(this._urlName, this._logFilePath, 0L, 0L, 4U, null, 0, null, null);
			}
		}

		// Token: 0x06000320 RID: 800 RVA: 0x00011CC2 File Offset: 0x00010CC2
		protected static uint GetCurrentLogThreadId()
		{
			return NativeMethods.GetCurrentThreadId();
		}

		// Token: 0x06000321 RID: 801 RVA: 0x00011CCC File Offset: 0x00010CCC
		protected static Logger GetCurrentThreadLogger()
		{
			Logger logger = null;
			uint currentLogThreadId = Logger.GetCurrentLogThreadId();
			lock (Logger._logAccessLock)
			{
				if (Logger._threadLogIdTable.Contains(currentLogThreadId))
				{
					Logger.LogIdentity logIdentity = (Logger.LogIdentity)Logger._threadLogIdTable[currentLogThreadId];
					if (Logger._loggerCollection.Contains(logIdentity.ToString()))
					{
						logger = (Logger)Logger._loggerCollection[logIdentity.ToString()];
					}
				}
			}
			return logger;
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00011D58 File Offset: 0x00010D58
		protected static Logger GetLogger(Logger.LogIdentity logIdentity)
		{
			Logger logger = null;
			lock (Logger._logAccessLock)
			{
				if (Logger._loggerCollection.Contains(logIdentity.ToString()))
				{
					logger = (Logger)Logger._loggerCollection[logIdentity.ToString()];
				}
			}
			return logger;
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00011DB8 File Offset: 0x00010DB8
		protected static void AddLogger(Logger logger)
		{
			lock (Logger._logAccessLock)
			{
				if (!Logger._loggerCollection.Contains(logger.Identity.ToString()))
				{
					Logger._loggerCollection.Add(logger.Identity.ToString(), logger);
				}
			}
		}

		// Token: 0x06000324 RID: 804 RVA: 0x00011E18 File Offset: 0x00010E18
		protected static void AddCurrentThreadLogger(Logger logger)
		{
			lock (Logger._logAccessLock)
			{
				if (Logger._threadLogIdTable.Contains(logger.Identity.ThreadId))
				{
					Logger._threadLogIdTable.Remove(logger.Identity.ThreadId);
				}
				Logger._threadLogIdTable.Add(logger.Identity.ThreadId, logger.Identity);
				if (!Logger._loggerCollection.Contains(logger.Identity.ToString()))
				{
					Logger._loggerCollection.Add(logger.Identity.ToString(), logger);
				}
			}
		}

		// Token: 0x06000325 RID: 805 RVA: 0x00011ED0 File Offset: 0x00010ED0
		protected static void RemoveLogger(Logger.LogIdentity logIdentity)
		{
			lock (Logger._logAccessLock)
			{
				if (Logger._loggerCollection.Contains(logIdentity.ToString()))
				{
					Logger._loggerCollection.Remove(logIdentity.ToString());
				}
			}
		}

		// Token: 0x06000326 RID: 806 RVA: 0x00011F24 File Offset: 0x00010F24
		protected static void RemoveCurrentThreadLogger()
		{
			lock (Logger._logAccessLock)
			{
				uint currentLogThreadId = Logger.GetCurrentLogThreadId();
				if (Logger._threadLogIdTable.Contains(currentLogThreadId))
				{
					Logger.LogIdentity logIdentity = (Logger.LogIdentity)Logger._threadLogIdTable[currentLogThreadId];
					Logger._threadLogIdTable.Remove(currentLogThreadId);
					if (Logger._loggerCollection.Contains(logIdentity.ToString()))
					{
						Logger._loggerCollection.Remove(logIdentity.ToString());
					}
				}
			}
		}

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00011FB8 File Offset: 0x00010FB8
		protected static Logger.HeaderSection Header
		{
			get
			{
				if (Logger._header == null)
				{
					object obj = new Logger.HeaderSection();
					Interlocked.CompareExchange(ref Logger._header, obj, null);
				}
				return (Logger.HeaderSection)Logger._header;
			}
		}

		// Token: 0x06000328 RID: 808 RVA: 0x00011FEC File Offset: 0x00010FEC
		internal static Logger.LogIdentity StartCurrentThreadLogging()
		{
			Logger.EndCurrentThreadLogging();
			Logger logger = new Logger();
			Logger.AddCurrentThreadLogger(logger);
			return logger.Identity;
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00012010 File Offset: 0x00011010
		internal static void EndCurrentThreadLogging()
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.EndLogOperation();
				}
				Logger.RemoveCurrentThreadLogger();
			}
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00012054 File Offset: 0x00011054
		internal static Logger.LogIdentity StartLogging()
		{
			Logger logger = new Logger();
			Logger.AddLogger(logger);
			return logger.Identity;
		}

		// Token: 0x0600032B RID: 811 RVA: 0x00012074 File Offset: 0x00011074
		internal static void EndLogging(Logger.LogIdentity logIdentity)
		{
			try
			{
				Logger logger = Logger.GetLogger(logIdentity);
				if (logger != null)
				{
					lock (logger)
					{
						logger.EndLogOperation();
					}
				}
				Logger.RemoveLogger(logIdentity);
			}
			catch (Exception)
			{
			}
		}

		// Token: 0x0600032C RID: 812 RVA: 0x000120CC File Offset: 0x000110CC
		internal static void SetSubscriptionUrl(Uri subscriptionUri)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				currentThreadLogger.SetSubscriptionUri(subscriptionUri);
			}
		}

		// Token: 0x0600032D RID: 813 RVA: 0x000120EC File Offset: 0x000110EC
		internal static void SetSubscriptionUrl(Logger.LogIdentity log, Uri subscriptionUri)
		{
			Logger logger = Logger.GetLogger(log);
			if (logger != null)
			{
				logger.SetSubscriptionUri(subscriptionUri);
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0001210C File Offset: 0x0001110C
		private void SetSubscriptionUri(Uri subscriptionUri)
		{
			lock (this)
			{
				this.Sources.SubscriptionUri = subscriptionUri;
			}
		}

		// Token: 0x0600032F RID: 815 RVA: 0x00012148 File Offset: 0x00011148
		internal static void SetSubscriptionServerInformation(ServerInformation serverInformation)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.Sources.SubscriptionServerInformation = serverInformation;
				}
			}
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0001218C File Offset: 0x0001118C
		internal static void SetSubscriptionUrl(string subscrioptionUrl)
		{
			try
			{
				Uri uri = new Uri(subscrioptionUrl);
				Logger.SetSubscriptionUrl(uri);
			}
			catch (UriFormatException)
			{
			}
		}

		// Token: 0x06000331 RID: 817 RVA: 0x000121BC File Offset: 0x000111BC
		internal static void SetDeploymentProviderUrl(Uri deploymentProviderUri)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.Sources.DeploymentProviderUri = deploymentProviderUri;
				}
			}
		}

		// Token: 0x06000332 RID: 818 RVA: 0x00012200 File Offset: 0x00011200
		internal static void SetDeploymentProviderServerInformation(ServerInformation serverInformation)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.Sources.DeploymentProviderServerInformation = serverInformation;
				}
			}
		}

		// Token: 0x06000333 RID: 819 RVA: 0x00012244 File Offset: 0x00011244
		internal static void SetApplicationUrl(Uri applicationUri)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.Sources.ApplicationUri = applicationUri;
				}
			}
		}

		// Token: 0x06000334 RID: 820 RVA: 0x00012288 File Offset: 0x00011288
		internal static void SetApplicationUrl(Logger.LogIdentity log, Uri applicationUri)
		{
			Logger logger = Logger.GetLogger(log);
			if (logger != null)
			{
				lock (logger)
				{
					logger.Sources.ApplicationUri = applicationUri;
				}
			}
		}

		// Token: 0x06000335 RID: 821 RVA: 0x000122CC File Offset: 0x000112CC
		internal static void SetApplicationServerInformation(ServerInformation serverInformation)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.Sources.ApplicationServerInformation = serverInformation;
				}
			}
		}

		// Token: 0x06000336 RID: 822 RVA: 0x00012310 File Offset: 0x00011310
		internal static void SetTextualSubscriptionIdentity(string textualIdentity)
		{
			try
			{
				Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
				if (currentThreadLogger != null)
				{
					currentThreadLogger.SetTextualSubscriptionIdentity(new DefinitionIdentity(textualIdentity));
				}
			}
			catch (COMException)
			{
			}
			catch (SEHException)
			{
			}
		}

		// Token: 0x06000337 RID: 823 RVA: 0x00012358 File Offset: 0x00011358
		internal static void SetTextualSubscriptionIdentity(Logger.LogIdentity log, string textualIdentity)
		{
			try
			{
				Logger logger = Logger.GetLogger(log);
				if (logger != null)
				{
					logger.SetTextualSubscriptionIdentity(new DefinitionIdentity(textualIdentity));
				}
			}
			catch (COMException)
			{
			}
			catch (SEHException)
			{
			}
		}

		// Token: 0x06000338 RID: 824 RVA: 0x000123A0 File Offset: 0x000113A0
		internal void SetTextualSubscriptionIdentity(DefinitionIdentity definitionIdentity)
		{
			lock (this)
			{
				this.Identities.DeploymentIdentity = definitionIdentity;
			}
		}

		// Token: 0x06000339 RID: 825 RVA: 0x000123DC File Offset: 0x000113DC
		internal static void SetDeploymentManifest(AssemblyManifest deploymentManifest)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					if (deploymentManifest.Identity != null)
					{
						currentThreadLogger.Identities.DeploymentIdentity = deploymentManifest.Identity;
					}
					currentThreadLogger.Summary.DeploymentManifest = deploymentManifest;
				}
			}
		}

		// Token: 0x0600033A RID: 826 RVA: 0x00012438 File Offset: 0x00011438
		internal static void SetDeploymentManifest(Logger.LogIdentity log, AssemblyManifest deploymentManifest)
		{
			Logger logger = Logger.GetLogger(log);
			if (logger != null)
			{
				lock (logger)
				{
					if (deploymentManifest.Identity != null)
					{
						logger.Identities.DeploymentIdentity = deploymentManifest.Identity;
					}
					logger.Summary.DeploymentManifest = deploymentManifest;
				}
			}
		}

		// Token: 0x0600033B RID: 827 RVA: 0x00012498 File Offset: 0x00011498
		internal static void SetApplicationManifest(AssemblyManifest applicationManifest)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					if (applicationManifest.Identity != null)
					{
						currentThreadLogger.Identities.ApplicationIdentity = applicationManifest.Identity;
					}
					currentThreadLogger.Summary.ApplicationManifest = applicationManifest;
				}
			}
		}

		// Token: 0x0600033C RID: 828 RVA: 0x000124F4 File Offset: 0x000114F4
		internal static void SetApplicationManifest(Logger.LogIdentity log, AssemblyManifest applicationManifest)
		{
			Logger logger = Logger.GetLogger(log);
			if (logger != null)
			{
				lock (logger)
				{
					if (applicationManifest.Identity != null)
					{
						logger.Identities.ApplicationIdentity = applicationManifest.Identity;
					}
					logger.Summary.ApplicationManifest = applicationManifest;
				}
			}
		}

		// Token: 0x0600033D RID: 829 RVA: 0x00012554 File Offset: 0x00011554
		internal static void AddErrorInformation(string message, Exception exception, DateTime time)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.Errors.AddError(message, exception, time);
				}
			}
		}

		// Token: 0x0600033E RID: 830 RVA: 0x0001259C File Offset: 0x0001159C
		internal static void AddErrorInformation(Logger.LogIdentity log, string message, Exception exception, DateTime time)
		{
			Logger logger = Logger.GetLogger(log);
			if (logger != null)
			{
				lock (logger)
				{
					logger.Errors.AddError(message, exception, time);
				}
			}
		}

		// Token: 0x0600033F RID: 831 RVA: 0x000125E4 File Offset: 0x000115E4
		internal static void AddWarningInformation(string message, DateTime time)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.Warnings.AddWarning(message, time);
				}
			}
		}

		// Token: 0x06000340 RID: 832 RVA: 0x00012628 File Offset: 0x00011628
		internal static void AddPhaseInformation(string message, DateTime time)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.Phases.AddPhaseInformation(message, time);
				}
			}
		}

		// Token: 0x06000341 RID: 833 RVA: 0x0001266C File Offset: 0x0001166C
		internal static void AddTransactionInformation(StoreTransactionOperation[] storeOperations, uint[] rgDispositions, int[] rgResults, DateTime time)
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					currentThreadLogger.Transactions.AddTransactionInformation(storeOperations, rgDispositions, rgResults, time);
				}
			}
		}

		// Token: 0x06000342 RID: 834 RVA: 0x000126B4 File Offset: 0x000116B4
		internal static void AddErrorInformation(string message, Exception exception)
		{
			Logger.AddErrorInformation(message, exception, DateTime.Now);
		}

		// Token: 0x06000343 RID: 835 RVA: 0x000126C2 File Offset: 0x000116C2
		internal static void AddErrorInformation(Logger.LogIdentity log, string message, Exception exception)
		{
			Logger.AddErrorInformation(log, message, exception, DateTime.Now);
		}

		// Token: 0x06000344 RID: 836 RVA: 0x000126D4 File Offset: 0x000116D4
		internal static void AddErrorInformation(Exception exception, string messageFormat, params object[] args)
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(messageFormat, args);
				Logger.AddErrorInformation(stringBuilder.ToString(), exception, DateTime.Now);
			}
			catch (FormatException)
			{
			}
		}

		// Token: 0x06000345 RID: 837 RVA: 0x00012718 File Offset: 0x00011718
		internal static void AddWarningInformation(string message)
		{
			Logger.AddWarningInformation(message, DateTime.Now);
		}

		// Token: 0x06000346 RID: 838 RVA: 0x00012725 File Offset: 0x00011725
		internal static void AddPhaseInformation(string message)
		{
			Logger.AddPhaseInformation(message, DateTime.Now);
		}

		// Token: 0x06000347 RID: 839 RVA: 0x00012734 File Offset: 0x00011734
		internal static void AddPhaseInformation(string messageFormat, params object[] args)
		{
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(messageFormat, args);
				Logger.AddPhaseInformation(stringBuilder.ToString(), DateTime.Now);
			}
			catch (FormatException)
			{
			}
		}

		// Token: 0x06000348 RID: 840 RVA: 0x00012778 File Offset: 0x00011778
		internal static void AddTransactionInformation(StoreTransactionOperation[] storeOperations, uint[] rgDispositions, int[] rgResults)
		{
			Logger.AddTransactionInformation(storeOperations, rgDispositions, rgResults, DateTime.Now);
		}

		// Token: 0x06000349 RID: 841 RVA: 0x00012788 File Offset: 0x00011788
		internal static string GetLogFilePath()
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				return Logger.GetLogFilePath(currentThreadLogger);
			}
			return null;
		}

		// Token: 0x0600034A RID: 842 RVA: 0x000127A8 File Offset: 0x000117A8
		internal static string GetLogFilePath(Logger.LogIdentity log)
		{
			Logger logger = Logger.GetLogger(log);
			if (logger != null)
			{
				return Logger.GetLogFilePath(logger);
			}
			return null;
		}

		// Token: 0x0600034B RID: 843 RVA: 0x000127C8 File Offset: 0x000117C8
		internal static string GetLogFilePath(Logger logger)
		{
			if (logger == null)
			{
				return null;
			}
			string logFilePath;
			lock (logger)
			{
				logFilePath = logger.LogFilePath;
			}
			return logFilePath;
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00012804 File Offset: 0x00011804
		internal static bool FlushCurrentThreadLogs()
		{
			Logger currentThreadLogger = Logger.GetCurrentThreadLogger();
			if (currentThreadLogger != null)
			{
				lock (currentThreadLogger)
				{
					return currentThreadLogger.FlushLogs();
				}
				return false;
			}
			return false;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00012848 File Offset: 0x00011848
		internal static bool FlushLog(Logger.LogIdentity log)
		{
			Logger logger = Logger.GetLogger(log);
			if (logger != null)
			{
				lock (logger)
				{
					return logger.FlushLogs();
				}
				return false;
			}
			return false;
		}

		// Token: 0x04000259 RID: 601
		protected Logger.SourceSection _sources = new Logger.SourceSection();

		// Token: 0x0400025A RID: 602
		protected Logger.IdentitySection _identities = new Logger.IdentitySection();

		// Token: 0x0400025B RID: 603
		protected Logger.SummarySection _summary = new Logger.SummarySection();

		// Token: 0x0400025C RID: 604
		protected Logger.ErrorSection _errors = new Logger.ErrorSection();

		// Token: 0x0400025D RID: 605
		protected Logger.WarningSection _warnings = new Logger.WarningSection();

		// Token: 0x0400025E RID: 606
		protected Logger.PhaseSection _phases = new Logger.PhaseSection();

		// Token: 0x0400025F RID: 607
		protected Logger.TransactionSection _transactions = new Logger.TransactionSection();

		// Token: 0x04000260 RID: 608
		protected Logger.LogIdentity _logIdentity = new Logger.LogIdentity();

		// Token: 0x04000261 RID: 609
		protected string _logFilePath;

		// Token: 0x04000262 RID: 610
		protected string _urlName;

		// Token: 0x04000263 RID: 611
		protected Logger.LogFileLocation _logFileLocation;

		// Token: 0x04000264 RID: 612
		protected static object _logFileEncoding;

		// Token: 0x04000265 RID: 613
		protected static Hashtable _loggerCollection = new Hashtable();

		// Token: 0x04000266 RID: 614
		protected static Hashtable _threadLogIdTable = new Hashtable();

		// Token: 0x04000267 RID: 615
		protected static object _logAccessLock = new object();

		// Token: 0x04000268 RID: 616
		protected static object _header = null;

		// Token: 0x02000066 RID: 102
		protected class LogInformation
		{
			// Token: 0x0600034F RID: 847 RVA: 0x000128B2 File Offset: 0x000118B2
			public LogInformation()
			{
			}

			// Token: 0x06000350 RID: 848 RVA: 0x000128D0 File Offset: 0x000118D0
			public LogInformation(string message, DateTime time)
			{
				if (message != null)
				{
					this._message = message;
				}
				this._time = time;
			}

			// Token: 0x170000DA RID: 218
			// (get) Token: 0x06000351 RID: 849 RVA: 0x000128FF File Offset: 0x000118FF
			public string Message
			{
				get
				{
					return this._message;
				}
			}

			// Token: 0x170000DB RID: 219
			// (get) Token: 0x06000352 RID: 850 RVA: 0x00012907 File Offset: 0x00011907
			public DateTime Time
			{
				get
				{
					return this._time;
				}
			}

			// Token: 0x04000269 RID: 617
			protected string _message = "";

			// Token: 0x0400026A RID: 618
			protected DateTime _time = DateTime.Now;
		}

		// Token: 0x02000067 RID: 103
		protected class ErrorInformation : Logger.LogInformation
		{
			// Token: 0x06000353 RID: 851 RVA: 0x0001290F File Offset: 0x0001190F
			public ErrorInformation(string message, Exception exception, DateTime time)
				: base(message, time)
			{
				this._exception = exception;
			}

			// Token: 0x170000DC RID: 220
			// (get) Token: 0x06000354 RID: 852 RVA: 0x00012920 File Offset: 0x00011920
			public string Summary
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_IndividualErrorSummary"), new object[] { this._message });
					for (Exception ex = this._exception; ex != null; ex = ex.InnerException)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_IndividualErrorSummaryBullets"), new object[] { ex.Message });
					}
					return stringBuilder.ToString();
				}
			}

			// Token: 0x06000355 RID: 853 RVA: 0x0001299C File Offset: 0x0001199C
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (Exception ex = this._exception; ex != null; ex = ex.InnerException)
				{
					string text = null;
					if (ex.StackTrace != null)
					{
						text = ex.StackTrace.Replace("   ", "\t\t\t");
					}
					if (ex == this._exception)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_IndividualErrorOutermostException"), new object[]
						{
							base.Time.ToString(DateTimeFormatInfo.CurrentInfo),
							Logger.ErrorInformation.GetExceptionType(ex),
							ex.Message,
							ex.Source,
							text
						});
					}
					else
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_IndividualErrorInnerException"), new object[]
						{
							Logger.ErrorInformation.GetExceptionType(ex),
							ex.Message,
							ex.Source,
							text
						});
					}
				}
				return stringBuilder.ToString();
			}

			// Token: 0x06000356 RID: 854 RVA: 0x00012A90 File Offset: 0x00011A90
			private static string GetExceptionType(Exception exception)
			{
				if (!(exception is DeploymentException))
				{
					return exception.GetType().ToString();
				}
				DeploymentException ex = (DeploymentException)exception;
				if (ex.SubType != ExceptionTypes.Unknown)
				{
					return string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_ExceptionType"), new object[]
					{
						ex.GetType().ToString(),
						ex.SubType.ToString()
					});
				}
				return string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_ExceptionTypeUnknown"), new object[] { ex.GetType().ToString() });
			}

			// Token: 0x0400026B RID: 619
			protected Exception _exception;
		}

		// Token: 0x02000068 RID: 104
		protected class TransactionInformation : Logger.LogInformation
		{
			// Token: 0x06000357 RID: 855 RVA: 0x00012B28 File Offset: 0x00011B28
			public TransactionInformation(StoreTransactionOperation[] storeOperations, uint[] rgDispositions, int[] rgResults, DateTime time)
				: base(null, time)
			{
				int num = Math.Min(Math.Min(storeOperations.Length, rgDispositions.Length), rgResults.Length);
				for (int i = 0; i < num; i++)
				{
					Logger.TransactionInformation.TransactionOperation transactionOperation = new Logger.TransactionInformation.TransactionOperation(storeOperations[i], rgDispositions[i], rgResults[i]);
					this._operations.Add(transactionOperation);
					if (transactionOperation.Failed)
					{
						this._failed = true;
					}
				}
			}

			// Token: 0x170000DD RID: 221
			// (get) Token: 0x06000358 RID: 856 RVA: 0x00012B9F File Offset: 0x00011B9F
			public bool Failed
			{
				get
				{
					return this._failed;
				}
			}

			// Token: 0x170000DE RID: 222
			// (get) Token: 0x06000359 RID: 857 RVA: 0x00012BA8 File Offset: 0x00011BA8
			public string FailureSummary
			{
				get
				{
					if (this.Failed)
					{
						StringBuilder stringBuilder = new StringBuilder();
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionFailureSummaryItem"), new object[] { base.Time.ToString(DateTimeFormatInfo.CurrentInfo) });
						foreach (object obj in this._operations)
						{
							Logger.TransactionInformation.TransactionOperation transactionOperation = (Logger.TransactionInformation.TransactionOperation)obj;
							if (transactionOperation.Failed)
							{
								stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionFailureSummaryBullets"), new object[] { transactionOperation.FailureMessage });
							}
						}
						return stringBuilder.ToString();
					}
					return Resources.GetString("LogFile_TransactionFailureSummaryNoFailure");
				}
			}

			// Token: 0x0600035A RID: 858 RVA: 0x00012C8C File Offset: 0x00011C8C
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionItem"), new object[] { base.Time.ToString(DateTimeFormatInfo.CurrentInfo) });
				foreach (object obj in this._operations)
				{
					Logger.TransactionInformation.TransactionOperation transactionOperation = (Logger.TransactionInformation.TransactionOperation)obj;
					stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionBullets"), new object[] { transactionOperation });
				}
				return stringBuilder.ToString();
			}

			// Token: 0x0400026C RID: 620
			protected ArrayList _operations = new ArrayList();

			// Token: 0x0400026D RID: 621
			protected bool _failed;

			// Token: 0x02000069 RID: 105
			public class TransactionOperation
			{
				// Token: 0x0600035B RID: 859 RVA: 0x00012D4C File Offset: 0x00011D4C
				public TransactionOperation(StoreTransactionOperation operation, uint disposition, int hresult)
				{
					this.AnalyzeTransactionOperation(operation, disposition, hresult);
				}

				// Token: 0x170000DF RID: 223
				// (get) Token: 0x0600035C RID: 860 RVA: 0x00012D73 File Offset: 0x00011D73
				public bool Failed
				{
					get
					{
						return this._failed;
					}
				}

				// Token: 0x170000E0 RID: 224
				// (get) Token: 0x0600035D RID: 861 RVA: 0x00012D7B File Offset: 0x00011D7B
				public string FailureMessage
				{
					get
					{
						return this._failureMessage;
					}
				}

				// Token: 0x0600035E RID: 862 RVA: 0x00012D83 File Offset: 0x00011D83
				public override string ToString()
				{
					return this._message;
				}

				// Token: 0x0600035F RID: 863 RVA: 0x00012D8C File Offset: 0x00011D8C
				protected void AnalyzeTransactionOperation(StoreTransactionOperation operation, uint dispositionValue, int hresult)
				{
					try
					{
						if (operation.Operation == StoreTransactionOperationType.StageComponent)
						{
							StoreOperationStageComponent storeOperationStageComponent = (StoreOperationStageComponent)Marshal.PtrToStructure(operation.Data.DataPtr, typeof(StoreOperationStageComponent));
							string text = ((StoreOperationStageComponent.Disposition)dispositionValue).ToString();
							this._message = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionStageComponent"), new object[]
							{
								storeOperationStageComponent.GetType().ToString(),
								text,
								hresult,
								Path.GetFileName(storeOperationStageComponent.ManifestPath)
							});
							if (dispositionValue == 0U)
							{
								this._failureMessage = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionStageComponentFailure"), new object[] { Path.GetFileName(storeOperationStageComponent.ManifestPath) });
								this._failed = true;
							}
						}
						else if (operation.Operation == StoreTransactionOperationType.PinDeployment)
						{
							StoreOperationPinDeployment storeOperationPinDeployment = (StoreOperationPinDeployment)Marshal.PtrToStructure(operation.Data.DataPtr, typeof(StoreOperationPinDeployment));
							string text = ((StoreOperationPinDeployment.Disposition)dispositionValue).ToString();
							DefinitionAppId definitionAppId = new DefinitionAppId(storeOperationPinDeployment.Application);
							this._message = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionPinDeployment"), new object[]
							{
								storeOperationPinDeployment.GetType().ToString(),
								text,
								hresult,
								definitionAppId.ToString()
							});
							if (dispositionValue == 0U)
							{
								this._failureMessage = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionPinDeploymentFailure"), new object[] { definitionAppId.ToString() });
								this._failed = true;
							}
						}
						else if (operation.Operation == StoreTransactionOperationType.UnpinDeployment)
						{
							StoreOperationUnpinDeployment storeOperationUnpinDeployment = (StoreOperationUnpinDeployment)Marshal.PtrToStructure(operation.Data.DataPtr, typeof(StoreOperationUnpinDeployment));
							string text = ((StoreOperationUnpinDeployment.Disposition)dispositionValue).ToString();
							DefinitionAppId definitionAppId2 = new DefinitionAppId(storeOperationUnpinDeployment.Application);
							this._message = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionUnPinDeployment"), new object[]
							{
								storeOperationUnpinDeployment.GetType().ToString(),
								text,
								hresult,
								definitionAppId2.ToString()
							});
							if (dispositionValue == 0U)
							{
								this._failureMessage = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionUnPinDeploymentFailure"), new object[] { definitionAppId2.ToString() });
								this._failed = true;
							}
						}
						else if (operation.Operation == StoreTransactionOperationType.InstallDeployment)
						{
							StoreOperationInstallDeployment storeOperationInstallDeployment = (StoreOperationInstallDeployment)Marshal.PtrToStructure(operation.Data.DataPtr, typeof(StoreOperationInstallDeployment));
							string text = ((StoreOperationInstallDeployment.Disposition)dispositionValue).ToString();
							DefinitionAppId definitionAppId3 = new DefinitionAppId(storeOperationInstallDeployment.Application);
							this._message = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionInstallDeployment"), new object[]
							{
								storeOperationInstallDeployment.GetType().ToString(),
								text,
								hresult,
								definitionAppId3.ToString()
							});
							if (dispositionValue == 0U)
							{
								this._failureMessage = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionInstallDeploymentFailure"), new object[] { definitionAppId3.ToString() });
								this._failed = true;
							}
						}
						else if (operation.Operation == StoreTransactionOperationType.UninstallDeployment)
						{
							StoreOperationUninstallDeployment storeOperationUninstallDeployment = (StoreOperationUninstallDeployment)Marshal.PtrToStructure(operation.Data.DataPtr, typeof(StoreOperationUninstallDeployment));
							string text = ((StoreOperationUninstallDeployment.Disposition)dispositionValue).ToString();
							DefinitionAppId definitionAppId4 = new DefinitionAppId(storeOperationUninstallDeployment.Application);
							this._message = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionUninstallDeployment"), new object[]
							{
								storeOperationUninstallDeployment.GetType().ToString(),
								text,
								hresult,
								definitionAppId4.ToString()
							});
							if (dispositionValue == 0U)
							{
								this._failureMessage = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionUninstallDeploymentFailure"), new object[] { definitionAppId4.ToString() });
								this._failed = true;
							}
						}
						else if (operation.Operation == StoreTransactionOperationType.SetDeploymentMetadata)
						{
							StoreOperationSetDeploymentMetadata storeOperationSetDeploymentMetadata = (StoreOperationSetDeploymentMetadata)Marshal.PtrToStructure(operation.Data.DataPtr, typeof(StoreOperationSetDeploymentMetadata));
							string text = ((StoreOperationSetDeploymentMetadata.Disposition)dispositionValue).ToString();
							this._message = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionSetDeploymentMetadata"), new object[]
							{
								storeOperationSetDeploymentMetadata.GetType().ToString(),
								text,
								hresult
							});
							if (dispositionValue == 0U)
							{
								this._failureMessage = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionSetDeploymentMetadataFailure"), new object[0]);
								this._failed = true;
							}
						}
						else if (operation.Operation == StoreTransactionOperationType.StageComponentFile)
						{
							StoreOperationStageComponentFile storeOperationStageComponentFile = (StoreOperationStageComponentFile)Marshal.PtrToStructure(operation.Data.DataPtr, typeof(StoreOperationStageComponentFile));
							string text = ((StoreOperationStageComponentFile.Disposition)dispositionValue).ToString();
							this._message = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionStageComponentFile"), new object[]
							{
								storeOperationStageComponentFile.GetType().ToString(),
								text,
								hresult,
								storeOperationStageComponentFile.ComponentRelativePath
							});
							if (dispositionValue == 0U)
							{
								this._failureMessage = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionStageComponentFileFailure"), new object[] { storeOperationStageComponentFile.ComponentRelativePath });
								this._failed = true;
							}
						}
						else
						{
							this._message = string.Format(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_TransactionUnknownOperation"), new object[]
							{
								operation.Operation.GetType().ToString(),
								(uint)operation.Operation,
								hresult
							});
						}
					}
					catch (FormatException)
					{
					}
					catch (ArgumentException)
					{
					}
				}

				// Token: 0x0400026E RID: 622
				protected bool _failed;

				// Token: 0x0400026F RID: 623
				protected string _message = "";

				// Token: 0x04000270 RID: 624
				protected string _failureMessage = "";
			}
		}

		// Token: 0x0200006A RID: 106
		protected class HeaderSection : Logger.LogInformation
		{
			// Token: 0x06000360 RID: 864 RVA: 0x00013424 File Offset: 0x00012424
			public HeaderSection()
			{
				this._message = Logger.HeaderSection.GenerateLogHeaderText();
			}

			// Token: 0x06000361 RID: 865 RVA: 0x00013437 File Offset: 0x00012437
			public override string ToString()
			{
				return this._message;
			}

			// Token: 0x06000362 RID: 866 RVA: 0x00013440 File Offset: 0x00012440
			protected static string GetModulePathInSystemFolder(string moduleName)
			{
				string text;
				try
				{
					text = Path.Combine(Environment.SystemDirectory, moduleName);
				}
				catch (ArgumentException)
				{
					text = null;
				}
				return text;
			}

			// Token: 0x06000363 RID: 867 RVA: 0x00013474 File Offset: 0x00012474
			protected static string GetModulePathInClrFolder(string moduleName)
			{
				string text = null;
				string loadedModulePath = NativeMethods.GetLoadedModulePath("dfsvc.exe");
				try
				{
					if (loadedModulePath != null)
					{
						string directoryName = Path.GetDirectoryName(loadedModulePath);
						text = Path.Combine(directoryName, moduleName);
					}
				}
				catch (ArgumentException)
				{
					text = null;
				}
				return text;
			}

			// Token: 0x06000364 RID: 868 RVA: 0x000134B8 File Offset: 0x000124B8
			protected static string GetModulePath(string moduleName)
			{
				string text = NativeMethods.GetLoadedModulePath(moduleName);
				if (text == null)
				{
					text = Logger.HeaderSection.GetModulePathInClrFolder(moduleName);
					if (text == null)
					{
						text = Logger.HeaderSection.GetModulePathInSystemFolder(moduleName);
					}
				}
				return text;
			}

			// Token: 0x06000365 RID: 869 RVA: 0x000134E4 File Offset: 0x000124E4
			protected static string GetExecutingAssemblyPath()
			{
				Assembly executingAssembly = Assembly.GetExecutingAssembly();
				return executingAssembly.Location;
			}

			// Token: 0x06000366 RID: 870 RVA: 0x00013500 File Offset: 0x00012500
			protected static FileVersionInfo GetVersionInfo(string modulePath)
			{
				FileVersionInfo fileVersionInfo = null;
				if (modulePath != null && global::System.IO.File.Exists(modulePath))
				{
					try
					{
						fileVersionInfo = FileVersionInfo.GetVersionInfo(modulePath);
					}
					catch (FileNotFoundException)
					{
					}
				}
				return fileVersionInfo;
			}

			// Token: 0x06000367 RID: 871 RVA: 0x00013538 File Offset: 0x00012538
			protected static string GenerateLogHeaderText()
			{
				string text = Logger.HeaderSection.GetExecutingAssemblyPath();
				string modulePath = Logger.HeaderSection.GetModulePath("mscorwks.dll");
				string modulePath2 = Logger.HeaderSection.GetModulePath("dfdll.dll");
				string modulePath3 = Logger.HeaderSection.GetModulePath("dfshim.dll");
				FileVersionInfo fileVersionInfo = Logger.HeaderSection.GetVersionInfo(text);
				if (fileVersionInfo == null)
				{
					text = Logger.HeaderSection.GetModulePathInClrFolder("system.deployment.dll");
					fileVersionInfo = Logger.HeaderSection.GetVersionInfo(text);
				}
				FileVersionInfo versionInfo = Logger.HeaderSection.GetVersionInfo(modulePath);
				FileVersionInfo versionInfo2 = Logger.HeaderSection.GetVersionInfo(modulePath2);
				FileVersionInfo versionInfo3 = Logger.HeaderSection.GetVersionInfo(modulePath3);
				StringBuilder stringBuilder = new StringBuilder();
				try
				{
					stringBuilder.Append(Resources.GetString("LogFile_Header"));
					stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_HeaderOSVersion"), new object[]
					{
						Environment.OSVersion.Platform.ToString(),
						Environment.OSVersion.Version.ToString()
					});
					stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_HeaderCLRVersion"), new object[] { Environment.Version.ToString() });
					if (fileVersionInfo != null)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_HeaderSystemDeploymentVersion"), new object[] { fileVersionInfo.FileVersion });
					}
					if (versionInfo != null)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_HeaderMscorwksVersion"), new object[] { versionInfo.FileVersion });
					}
					if (versionInfo2 != null)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_HeaderDfdllVersion"), new object[] { versionInfo2.FileVersion });
					}
					if (versionInfo3 != null)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_HeaderDfshimVersion"), new object[] { versionInfo3.FileVersion });
					}
				}
				catch (ArgumentException)
				{
				}
				catch (FormatException)
				{
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x0200006B RID: 107
		protected class SourceSection : Logger.LogInformation
		{
			// Token: 0x170000E1 RID: 225
			// (set) Token: 0x06000368 RID: 872 RVA: 0x0001373C File Offset: 0x0001273C
			public Uri SubscriptionUri
			{
				set
				{
					this._subscriptonUri = value;
				}
			}

			// Token: 0x170000E2 RID: 226
			// (set) Token: 0x06000369 RID: 873 RVA: 0x00013745 File Offset: 0x00012745
			public Uri DeploymentProviderUri
			{
				set
				{
					this._deploymentProviderUri = value;
				}
			}

			// Token: 0x170000E3 RID: 227
			// (set) Token: 0x0600036A RID: 874 RVA: 0x0001374E File Offset: 0x0001274E
			public Uri ApplicationUri
			{
				set
				{
					this._applicationUri = value;
				}
			}

			// Token: 0x170000E4 RID: 228
			// (set) Token: 0x0600036B RID: 875 RVA: 0x00013757 File Offset: 0x00012757
			public ServerInformation SubscriptionServerInformation
			{
				set
				{
					this._subscriptionServerInformation = value;
				}
			}

			// Token: 0x170000E5 RID: 229
			// (set) Token: 0x0600036C RID: 876 RVA: 0x00013760 File Offset: 0x00012760
			public ServerInformation DeploymentProviderServerInformation
			{
				set
				{
					this._deploymentProviderServerInformation = value;
				}
			}

			// Token: 0x170000E6 RID: 230
			// (set) Token: 0x0600036D RID: 877 RVA: 0x00013769 File Offset: 0x00012769
			public ServerInformation ApplicationServerInformation
			{
				set
				{
					this._applicationServerInformation = value;
				}
			}

			// Token: 0x0600036E RID: 878 RVA: 0x00013774 File Offset: 0x00012774
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this._subscriptonUri != null || this._deploymentProviderUri != null || this._applicationUri != null)
				{
					stringBuilder.Append(Resources.GetString("LogFile_Source"));
					if (this._subscriptonUri != null)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_SourceDeploymentUrl"), new object[] { this._subscriptonUri.AbsoluteUri });
					}
					if (this._subscriptionServerInformation != null)
					{
						Logger.SourceSection.AppendServerInformation(stringBuilder, this._subscriptionServerInformation);
					}
					if (this._deploymentProviderUri != null)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_SourceDeploymentProviderUrl"), new object[] { this._deploymentProviderUri.AbsoluteUri });
					}
					if (this._deploymentProviderServerInformation != null)
					{
						Logger.SourceSection.AppendServerInformation(stringBuilder, this._deploymentProviderServerInformation);
					}
					if (this._applicationUri != null)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_SourceApplicationUrl"), new object[] { this._applicationUri.AbsoluteUri });
					}
					if (this._applicationServerInformation != null)
					{
						Logger.SourceSection.AppendServerInformation(stringBuilder, this._applicationServerInformation);
					}
					stringBuilder.Append(Environment.NewLine);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x0600036F RID: 879 RVA: 0x000138C4 File Offset: 0x000128C4
			private static void AppendServerInformation(StringBuilder destination, ServerInformation serverInformation)
			{
				if (!string.IsNullOrEmpty(serverInformation.Server))
				{
					destination.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_ServerInformationServer"), new object[] { serverInformation.Server });
				}
				if (!string.IsNullOrEmpty(serverInformation.PoweredBy))
				{
					destination.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_ServerInformationPoweredBy"), new object[] { serverInformation.PoweredBy });
				}
				if (!string.IsNullOrEmpty(serverInformation.AspNetVersion))
				{
					destination.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_ServerInformationAspNetVersion"), new object[] { serverInformation.AspNetVersion });
				}
			}

			// Token: 0x04000271 RID: 625
			protected Uri _subscriptonUri;

			// Token: 0x04000272 RID: 626
			protected Uri _deploymentProviderUri;

			// Token: 0x04000273 RID: 627
			protected Uri _applicationUri;

			// Token: 0x04000274 RID: 628
			protected ServerInformation _subscriptionServerInformation;

			// Token: 0x04000275 RID: 629
			protected ServerInformation _deploymentProviderServerInformation;

			// Token: 0x04000276 RID: 630
			protected ServerInformation _applicationServerInformation;
		}

		// Token: 0x0200006C RID: 108
		protected class IdentitySection : Logger.LogInformation
		{
			// Token: 0x170000E7 RID: 231
			// (get) Token: 0x06000371 RID: 881 RVA: 0x00013975 File Offset: 0x00012975
			// (set) Token: 0x06000372 RID: 882 RVA: 0x0001397D File Offset: 0x0001297D
			public DefinitionIdentity DeploymentIdentity
			{
				get
				{
					return this._deploymentIdentity;
				}
				set
				{
					this._deploymentIdentity = value;
				}
			}

			// Token: 0x170000E8 RID: 232
			// (set) Token: 0x06000373 RID: 883 RVA: 0x00013986 File Offset: 0x00012986
			public DefinitionIdentity ApplicationIdentity
			{
				set
				{
					this._applicationIdentity = value;
				}
			}

			// Token: 0x06000374 RID: 884 RVA: 0x00013990 File Offset: 0x00012990
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this._deploymentIdentity != null || this._applicationIdentity != null)
				{
					stringBuilder.Append(Resources.GetString("LogFile_Identity"));
					if (this._deploymentIdentity != null)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_IdentityDeploymentIdentity"), new object[] { this._deploymentIdentity.ToString() });
					}
					if (this._applicationIdentity != null)
					{
						stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_IdentityApplicationIdentity"), new object[] { this._applicationIdentity.ToString() });
					}
					stringBuilder.Append(Environment.NewLine);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x04000277 RID: 631
			protected DefinitionIdentity _deploymentIdentity;

			// Token: 0x04000278 RID: 632
			protected DefinitionIdentity _applicationIdentity;
		}

		// Token: 0x0200006D RID: 109
		protected class SummarySection : Logger.LogInformation
		{
			// Token: 0x170000E9 RID: 233
			// (set) Token: 0x06000376 RID: 886 RVA: 0x00013A49 File Offset: 0x00012A49
			public AssemblyManifest DeploymentManifest
			{
				set
				{
					this._deploymentManifest = value;
				}
			}

			// Token: 0x170000EA RID: 234
			// (set) Token: 0x06000377 RID: 887 RVA: 0x00013A52 File Offset: 0x00012A52
			public AssemblyManifest ApplicationManifest
			{
				set
				{
					this._applicationManifest = value;
				}
			}

			// Token: 0x06000378 RID: 888 RVA: 0x00013A5C File Offset: 0x00012A5C
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (this._deploymentManifest != null)
				{
					stringBuilder.Append(Resources.GetString("LogFile_Summary"));
					if (this._deploymentManifest.Deployment.Install)
					{
						stringBuilder.Append(Resources.GetString("LogFile_SummaryInstallableApp"));
					}
					else
					{
						stringBuilder.Append(Resources.GetString("LogFile_SummaryOnlineOnlyApp"));
					}
					if (this._deploymentManifest.Deployment.TrustURLParameters)
					{
						stringBuilder.Append(Resources.GetString("LogFile_SummaryTrustUrlParameterSet"));
					}
					if (this._applicationManifest != null && this._applicationManifest.EntryPoints[0].HostInBrowser)
					{
						stringBuilder.Append(Resources.GetString("LogFile_SummaryBrowserHostedApp"));
					}
					stringBuilder.Append(Environment.NewLine);
				}
				return stringBuilder.ToString();
			}

			// Token: 0x04000279 RID: 633
			protected AssemblyManifest _deploymentManifest;

			// Token: 0x0400027A RID: 634
			protected AssemblyManifest _applicationManifest;
		}

		// Token: 0x0200006E RID: 110
		protected class ErrorSection : Logger.LogInformation
		{
			// Token: 0x0600037A RID: 890 RVA: 0x00013B2C File Offset: 0x00012B2C
			public void AddError(string message, Exception exception, DateTime time)
			{
				Logger.ErrorInformation errorInformation = new Logger.ErrorInformation(message, exception, time);
				this._errors.Add(errorInformation);
			}

			// Token: 0x170000EB RID: 235
			// (get) Token: 0x0600037B RID: 891 RVA: 0x00013B50 File Offset: 0x00012B50
			public string ErrorSummary
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(Resources.GetString("LogFile_ErrorSummary"));
					if (this._errors.Count > 0)
					{
						stringBuilder.Append(Resources.GetString("LogFile_ErrorSummaryStatusError"));
						using (IEnumerator enumerator = this._errors.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Logger.ErrorInformation errorInformation = (Logger.ErrorInformation)obj;
								stringBuilder.Append(errorInformation.Summary);
							}
							goto IL_0089;
						}
					}
					stringBuilder.Append(Resources.GetString("LogFile_ErrorSummaryStatusNoError"));
					IL_0089:
					return stringBuilder.ToString();
				}
			}

			// Token: 0x0600037C RID: 892 RVA: 0x00013BFC File Offset: 0x00012BFC
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Resources.GetString("LogFile_Error"));
				if (this._errors.Count > 0)
				{
					stringBuilder.Append(Resources.GetString("LogFile_ErrorStatusError"));
					using (IEnumerator enumerator = this._errors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Logger.ErrorInformation errorInformation = (Logger.ErrorInformation)obj;
							stringBuilder.Append(errorInformation.ToString());
						}
						goto IL_0089;
					}
				}
				stringBuilder.Append(Resources.GetString("LogFile_ErrorStatusNoError"));
				IL_0089:
				return stringBuilder.ToString();
			}

			// Token: 0x0400027B RID: 635
			protected ArrayList _errors = new ArrayList();
		}

		// Token: 0x0200006F RID: 111
		protected class WarningSection : Logger.LogInformation
		{
			// Token: 0x0600037E RID: 894 RVA: 0x00013CBC File Offset: 0x00012CBC
			public void AddWarning(string message, DateTime time)
			{
				Logger.LogInformation logInformation = new Logger.LogInformation(message, time);
				this._warnings.Add(logInformation);
			}

			// Token: 0x0600037F RID: 895 RVA: 0x00013CE0 File Offset: 0x00012CE0
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Resources.GetString("LogFile_Warning"));
				if (this._warnings.Count > 0)
				{
					using (IEnumerator enumerator = this._warnings.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Logger.LogInformation logInformation = (Logger.LogInformation)obj;
							stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_WarningStatusIndivualWarning"), new object[] { logInformation.Message });
						}
						goto IL_0095;
					}
				}
				stringBuilder.Append(Resources.GetString("LogFile_WarningStatusNoWarning"));
				IL_0095:
				return stringBuilder.ToString();
			}

			// Token: 0x0400027C RID: 636
			protected ArrayList _warnings = new ArrayList();
		}

		// Token: 0x02000070 RID: 112
		protected class PhaseSection : Logger.LogInformation
		{
			// Token: 0x06000381 RID: 897 RVA: 0x00013DAC File Offset: 0x00012DAC
			public void AddPhaseInformation(string phaseMessage, DateTime time)
			{
				Logger.LogInformation logInformation = new Logger.LogInformation(phaseMessage, time);
				this._phaseInformations.Add(logInformation);
			}

			// Token: 0x06000382 RID: 898 RVA: 0x00013DD0 File Offset: 0x00012DD0
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Resources.GetString("LogFile_PhaseInformation"));
				if (this._phaseInformations.Count > 0)
				{
					using (IEnumerator enumerator = this._phaseInformations.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Logger.LogInformation logInformation = (Logger.LogInformation)obj;
							stringBuilder.AppendFormat(CultureInfo.CurrentUICulture, Resources.GetString("LogFile_PhaseInformationStatusIndivualPhaseInformation"), new object[]
							{
								logInformation.Time.ToString(DateTimeFormatInfo.CurrentInfo),
								logInformation.Message
							});
						}
						goto IL_00AC;
					}
				}
				stringBuilder.Append(Resources.GetString("LogFile_PhaseInformationStatusNoPhaseInformation"));
				IL_00AC:
				return stringBuilder.ToString();
			}

			// Token: 0x0400027D RID: 637
			protected ArrayList _phaseInformations = new ArrayList();
		}

		// Token: 0x02000071 RID: 113
		protected class TransactionSection : Logger.LogInformation
		{
			// Token: 0x06000384 RID: 900 RVA: 0x00013EB4 File Offset: 0x00012EB4
			public void AddTransactionInformation(StoreTransactionOperation[] storeOperations, uint[] rgDispositions, int[] rgResults, DateTime time)
			{
				Logger.TransactionInformation transactionInformation = new Logger.TransactionInformation(storeOperations, rgDispositions, rgResults, time);
				this._transactionInformations.Add(transactionInformation);
				if (transactionInformation.Failed)
				{
					this._failedTransactionInformations.Add(transactionInformation);
				}
			}

			// Token: 0x06000385 RID: 901 RVA: 0x00013EF0 File Offset: 0x00012EF0
			public override string ToString()
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(Resources.GetString("LogFile_Transaction"));
				if (this._transactionInformations.Count > 0)
				{
					using (IEnumerator enumerator = this._transactionInformations.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Logger.TransactionInformation transactionInformation = (Logger.TransactionInformation)obj;
							stringBuilder.Append(transactionInformation.ToString());
						}
						goto IL_0078;
					}
				}
				stringBuilder.Append(Resources.GetString("LogFile_TransactionNoTransaction"));
				IL_0078:
				return stringBuilder.ToString();
			}

			// Token: 0x170000EC RID: 236
			// (get) Token: 0x06000386 RID: 902 RVA: 0x00013F8C File Offset: 0x00012F8C
			public string FailureSummary
			{
				get
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append(Resources.GetString("LogFile_TransactionFailureSummary"));
					if (this._failedTransactionInformations.Count > 0)
					{
						using (IEnumerator enumerator = this._failedTransactionInformations.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								object obj = enumerator.Current;
								Logger.TransactionInformation transactionInformation = (Logger.TransactionInformation)obj;
								stringBuilder.Append(transactionInformation.FailureSummary);
							}
							goto IL_0078;
						}
					}
					stringBuilder.Append(Resources.GetString("LogFile_TransactionFailureSummaryNoError"));
					IL_0078:
					return stringBuilder.ToString();
				}
			}

			// Token: 0x0400027E RID: 638
			protected ArrayList _transactionInformations = new ArrayList();

			// Token: 0x0400027F RID: 639
			protected ArrayList _failedTransactionInformations = new ArrayList();
		}

		// Token: 0x02000072 RID: 114
		public class LogIdentity
		{
			// Token: 0x170000ED RID: 237
			// (get) Token: 0x06000389 RID: 905 RVA: 0x00014079 File Offset: 0x00013079
			public uint ThreadId
			{
				get
				{
					return this._threadId;
				}
			}

			// Token: 0x0600038A RID: 906 RVA: 0x00014084 File Offset: 0x00013084
			public override string ToString()
			{
				if (this._logIdentityStringForm == null)
				{
					this._logIdentityStringForm = string.Format(CultureInfo.InvariantCulture, "{0:x8}{1:x16}", new object[] { this._threadId, this._ticks });
				}
				return this._logIdentityStringForm;
			}

			// Token: 0x04000280 RID: 640
			protected readonly long _ticks = DateTime.Now.Ticks;

			// Token: 0x04000281 RID: 641
			protected readonly uint _threadId = NativeMethods.GetCurrentThreadId();

			// Token: 0x04000282 RID: 642
			protected string _logIdentityStringForm;
		}

		// Token: 0x02000073 RID: 115
		protected enum LogFileLocation
		{
			// Token: 0x04000284 RID: 644
			NoLogFile,
			// Token: 0x04000285 RID: 645
			RegistryBased,
			// Token: 0x04000286 RID: 646
			WinInetCache
		}
	}
}
