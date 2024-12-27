using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Configuration.Internal
{
	// Token: 0x02000018 RID: 24
	[ComVisible(false)]
	public interface IInternalConfigHost
	{
		// Token: 0x06000121 RID: 289
		void Init(IInternalConfigRoot configRoot, params object[] hostInitParams);

		// Token: 0x06000122 RID: 290
		void InitForConfiguration(ref string locationSubPath, out string configPath, out string locationConfigPath, IInternalConfigRoot configRoot, params object[] hostInitConfigurationParams);

		// Token: 0x06000123 RID: 291
		bool IsConfigRecordRequired(string configPath);

		// Token: 0x06000124 RID: 292
		bool IsInitDelayed(IInternalConfigRecord configRecord);

		// Token: 0x06000125 RID: 293
		void RequireCompleteInit(IInternalConfigRecord configRecord);

		// Token: 0x06000126 RID: 294
		bool IsSecondaryRoot(string configPath);

		// Token: 0x06000127 RID: 295
		string GetStreamName(string configPath);

		// Token: 0x06000128 RID: 296
		string GetStreamNameForConfigSource(string streamName, string configSource);

		// Token: 0x06000129 RID: 297
		object GetStreamVersion(string streamName);

		// Token: 0x0600012A RID: 298
		Stream OpenStreamForRead(string streamName);

		// Token: 0x0600012B RID: 299
		Stream OpenStreamForRead(string streamName, bool assertPermissions);

		// Token: 0x0600012C RID: 300
		Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext);

		// Token: 0x0600012D RID: 301
		Stream OpenStreamForWrite(string streamName, string templateStreamName, ref object writeContext, bool assertPermissions);

		// Token: 0x0600012E RID: 302
		void WriteCompleted(string streamName, bool success, object writeContext);

		// Token: 0x0600012F RID: 303
		void WriteCompleted(string streamName, bool success, object writeContext, bool assertPermissions);

		// Token: 0x06000130 RID: 304
		void DeleteStream(string streamName);

		// Token: 0x06000131 RID: 305
		bool IsFile(string streamName);

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000132 RID: 306
		bool SupportsChangeNotifications { get; }

		// Token: 0x06000133 RID: 307
		object StartMonitoringStreamForChanges(string streamName, StreamChangeCallback callback);

		// Token: 0x06000134 RID: 308
		void StopMonitoringStreamForChanges(string streamName, StreamChangeCallback callback);

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000135 RID: 309
		bool SupportsRefresh { get; }

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000136 RID: 310
		bool SupportsPath { get; }

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000137 RID: 311
		bool SupportsLocation { get; }

		// Token: 0x06000138 RID: 312
		bool IsAboveApplication(string configPath);

		// Token: 0x06000139 RID: 313
		string GetConfigPathFromLocationSubPath(string configPath, string locationSubPath);

		// Token: 0x0600013A RID: 314
		bool IsLocationApplicable(string configPath);

		// Token: 0x0600013B RID: 315
		bool IsDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition);

		// Token: 0x0600013C RID: 316
		void VerifyDefinitionAllowed(string configPath, ConfigurationAllowDefinition allowDefinition, ConfigurationAllowExeDefinition allowExeDefinition, IConfigErrorInfo errorInfo);

		// Token: 0x0600013D RID: 317
		bool IsTrustedConfigPath(string configPath);

		// Token: 0x0600013E RID: 318
		bool IsFullTrustSectionWithoutAptcaAllowed(IInternalConfigRecord configRecord);

		// Token: 0x0600013F RID: 319
		void GetRestrictedPermissions(IInternalConfigRecord configRecord, out PermissionSet permissionSet, out bool isHostReady);

		// Token: 0x06000140 RID: 320
		IDisposable Impersonate();

		// Token: 0x06000141 RID: 321
		bool PrefetchAll(string configPath, string streamName);

		// Token: 0x06000142 RID: 322
		bool PrefetchSection(string sectionGroupName, string sectionName);

		// Token: 0x06000143 RID: 323
		object CreateDeprecatedConfigContext(string configPath);

		// Token: 0x06000144 RID: 324
		object CreateConfigurationContext(string configPath, string locationSubPath);

		// Token: 0x06000145 RID: 325
		string DecryptSection(string encryptedXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection);

		// Token: 0x06000146 RID: 326
		string EncryptSection(string clearTextXml, ProtectedConfigurationProvider protectionProvider, ProtectedConfigurationSection protectedConfigSection);

		// Token: 0x06000147 RID: 327
		Type GetConfigType(string typeName, bool throwOnError);

		// Token: 0x06000148 RID: 328
		string GetConfigTypeName(Type t);

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000149 RID: 329
		bool IsRemote { get; }
	}
}
