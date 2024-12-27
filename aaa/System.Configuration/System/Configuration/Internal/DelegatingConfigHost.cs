using System;
using System.IO;
using System.Security;

namespace System.Configuration.Internal
{
	// Token: 0x02000019 RID: 25
	public class DelegatingConfigHost : IInternalConfigHost
	{
		// Token: 0x0600014A RID: 330 RVA: 0x0000B641 File Offset: 0x0000A641
		protected DelegatingConfigHost()
		{
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000B649 File Offset: 0x0000A649
		// (set) Token: 0x0600014C RID: 332 RVA: 0x0000B651 File Offset: 0x0000A651
		protected IInternalConfigHost Host
		{
			get
			{
				return this._host;
			}
			set
			{
				this._host = value;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000B65A File Offset: 0x0000A65A
		public virtual void Init(IInternalConfigRoot configRoot, params object[] hostInitParams)
		{
			this.Host.Init(configRoot, hostInitParams);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0000B669 File Offset: 0x0000A669
		public virtual void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams)
		{
			this.Host.InitForConfiguration(ref locationSubPath, out configPath, out locationConfigPath, configRoot, hostInitConfigurationParams);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0000B67D File Offset: 0x0000A67D
		public virtual bool IsConfigRecordRequired(string configPath)
		{
			return this.Host.IsConfigRecordRequired(configPath);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000B68B File Offset: 0x0000A68B
		public virtual bool IsInitDelayed(IInternalConfigRecord configRecord)
		{
			return this.Host.IsInitDelayed(configRecord);
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0000B699 File Offset: 0x0000A699
		public virtual void RequireCompleteInit(IInternalConfigRecord configRecord)
		{
			this.Host.RequireCompleteInit(configRecord);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000B6A7 File Offset: 0x0000A6A7
		public virtual bool IsSecondaryRoot(string configPath)
		{
			return this.Host.IsSecondaryRoot(configPath);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000B6B5 File Offset: 0x0000A6B5
		public virtual string GetStreamName(string configPath)
		{
			return this.Host.GetStreamName(configPath);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000B6C3 File Offset: 0x0000A6C3
		public virtual string GetStreamNameForConfigSource(string streamName, string configSource)
		{
			return this.Host.GetStreamNameForConfigSource(streamName, configSource);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000B6D2 File Offset: 0x0000A6D2
		public virtual object GetStreamVersion(string streamName)
		{
			return this.Host.GetStreamVersion(streamName);
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000B6E0 File Offset: 0x0000A6E0
		public virtual Stream OpenStreamForRead(string streamName)
		{
			return this.Host.OpenStreamForRead(streamName);
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0000B6EE File Offset: 0x0000A6EE
		public virtual Stream OpenStreamForRead(string streamName, bool assertPermissions)
		{
			return this.Host.OpenStreamForRead(streamName, assertPermissions);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000B6FD File Offset: 0x0000A6FD
		public virtual Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext)
		{
			return this.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext);
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000B70D File Offset: 0x0000A70D
		public virtual Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions)
		{
			return this.Host.OpenStreamForWrite(streamName, templateStreamName, ref writeContext, assertPermissions);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000B71F File Offset: 0x0000A71F
		public virtual void WriteCompleted(string streamName, bool success, object writeContext)
		{
			this.Host.WriteCompleted(streamName, success, writeContext);
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000B72F File Offset: 0x0000A72F
		public virtual void WriteCompleted(string streamName, bool success, object writeContext, bool assertPermissions)
		{
			this.Host.WriteCompleted(streamName, success, writeContext, assertPermissions);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000B741 File Offset: 0x0000A741
		public virtual void DeleteStream(string streamName)
		{
			this.Host.DeleteStream(streamName);
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000B74F File Offset: 0x0000A74F
		public virtual bool IsFile(string streamName)
		{
			return this.Host.IsFile(streamName);
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600015E RID: 350 RVA: 0x0000B75D File Offset: 0x0000A75D
		public virtual bool SupportsChangeNotifications
		{
			get
			{
				return this.Host.SupportsChangeNotifications;
			}
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000B76A File Offset: 0x0000A76A
		public virtual object StartMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
		{
			return this.Host.StartMonitoringStreamForChanges(streamName, callback);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000B779 File Offset: 0x0000A779
		public virtual void StopMonitoringStreamForChanges(string streamName, StreamChangeCallback callback)
		{
			this.Host.StopMonitoringStreamForChanges(streamName, callback);
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000161 RID: 353 RVA: 0x0000B788 File Offset: 0x0000A788
		public virtual bool SupportsRefresh
		{
			get
			{
				return this.Host.SupportsRefresh;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000162 RID: 354 RVA: 0x0000B795 File Offset: 0x0000A795
		public virtual bool SupportsPath
		{
			get
			{
				return this.Host.SupportsPath;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0000B7A2 File Offset: 0x0000A7A2
		public virtual bool SupportsLocation
		{
			get
			{
				return this.Host.SupportsLocation;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000B7AF File Offset: 0x0000A7AF
		public virtual bool IsAboveApplication(string configPath)
		{
			return this.Host.IsAboveApplication(configPath);
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000B7BD File Offset: 0x0000A7BD
		public virtual bool IsDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition)
		{
			return this.Host.IsDefinitionAllowed(configPath, allowDefinition, allowExeDefinition);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000B7CD File Offset: 0x0000A7CD
		public virtual void VerifyDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, IConfigErrorInfo errorInfo)
		{
			this.Host.VerifyDefinitionAllowed(configPath, allowDefinition, allowExeDefinition, errorInfo);
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000B7DF File Offset: 0x0000A7DF
		public virtual string GetConfigPathFromLocationSubPath(string configPath, string locationSubPath)
		{
			return this.Host.GetConfigPathFromLocationSubPath(configPath, locationSubPath);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000B7EE File Offset: 0x0000A7EE
		public virtual bool IsLocationApplicable(string configPath)
		{
			return this.Host.IsLocationApplicable(configPath);
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000B7FC File Offset: 0x0000A7FC
		public virtual bool IsTrustedConfigPath(string configPath)
		{
			return this.Host.IsTrustedConfigPath(configPath);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000B80A File Offset: 0x0000A80A
		public virtual bool IsFullTrustSectionWithoutAptcaAllowed(IInternalConfigRecord configRecord)
		{
			return this.Host.IsFullTrustSectionWithoutAptcaAllowed(configRecord);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000B818 File Offset: 0x0000A818
		public virtual void GetRestrictedPermissions(IInternalConfigRecord configRecord, out PermissionSet permissionSet, out bool isHostReady)
		{
			this.Host.GetRestrictedPermissions(configRecord, out permissionSet, out isHostReady);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000B828 File Offset: 0x0000A828
		public virtual IDisposable Impersonate()
		{
			return this.Host.Impersonate();
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000B835 File Offset: 0x0000A835
		public virtual bool PrefetchAll(string configPath, string streamName)
		{
			return this.Host.PrefetchAll(configPath, streamName);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000B844 File Offset: 0x0000A844
		public virtual bool PrefetchSection(string sectionGroupName, string sectionName)
		{
			return this.Host.PrefetchSection(sectionGroupName, sectionName);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000B853 File Offset: 0x0000A853
		public virtual object CreateDeprecatedConfigContext(string configPath)
		{
			return this.Host.CreateDeprecatedConfigContext(configPath);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000B861 File Offset: 0x0000A861
		public virtual object CreateConfigurationContext(string configPath, string locationSubPath)
		{
			return this.Host.CreateConfigurationContext(configPath, locationSubPath);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000B870 File Offset: 0x0000A870
		public virtual string DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
		{
			return this.Host.DecryptSection(encryptedXml, protectionProvider, protectedConfigSection);
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000B880 File Offset: 0x0000A880
		public virtual string EncryptSection(string clearTextXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection)
		{
			return this.Host.EncryptSection(clearTextXml, protectionProvider, protectedConfigSection);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000B890 File Offset: 0x0000A890
		public virtual Type GetConfigType(string typeName, bool throwOnError)
		{
			return this.Host.GetConfigType(typeName, throwOnError);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000B89F File Offset: 0x0000A89F
		public virtual string GetConfigTypeName(Type t)
		{
			return this.Host.GetConfigTypeName(t);
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000175 RID: 373 RVA: 0x0000B8AD File Offset: 0x0000A8AD
		public virtual bool IsRemote
		{
			get
			{
				return this.Host.IsRemote;
			}
		}

		// Token: 0x040001F1 RID: 497
		private IInternalConfigHost _host;
	}
}
