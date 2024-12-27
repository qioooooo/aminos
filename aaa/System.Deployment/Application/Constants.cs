using System;

namespace System.Deployment.Application
{
	// Token: 0x02000038 RID: 56
	internal static class Constants
	{
		// Token: 0x0400011B RID: 283
		public const string ShimDll = "dfshim.dll";

		// Token: 0x0400011C RID: 284
		public const string DfDll = "dfdll.dll";

		// Token: 0x0400011D RID: 285
		public const string DeploymentFolder = "Deployment";

		// Token: 0x0400011E RID: 286
		public const string Dfsvc = "dfsvc.exe";

		// Token: 0x0400011F RID: 287
		public const string SystemDeploymentDll = "system.deployment.dll";

		// Token: 0x04000120 RID: 288
		public const string Kernel32Dll = "kernel32.dll";

		// Token: 0x04000121 RID: 289
		public const string MscoreeDll = "mscoree.dll";

		// Token: 0x04000122 RID: 290
		public const string WininetDll = "wininet.dll";

		// Token: 0x04000123 RID: 291
		public const string MscorwksDll = "mscorwks.dll";

		// Token: 0x04000124 RID: 292
		public const string SrClientDll = "srclient.dll";

		// Token: 0x04000125 RID: 293
		public const string WinInetDll = "wininet.dll";

		// Token: 0x04000126 RID: 294
		public const string Shell32Dll = "shell32.dll";

		// Token: 0x04000127 RID: 295
		public const string ShellAppShortcutExtension = ".appref-ms";

		// Token: 0x04000128 RID: 296
		public const string ShellSupportShortcutExtension = ".url";

		// Token: 0x04000129 RID: 297
		public const int SupportIconIndex = 0;

		// Token: 0x0400012A RID: 298
		public const string UninstallSubkeyName = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";

		// Token: 0x0400012B RID: 299
		public const string DeploymentSubkeyName = "SOFTWARE\\Classes\\Software\\Microsoft\\Windows\\CurrentVersion\\Deployment";

		// Token: 0x0400012C RID: 300
		public const string RandomKeyName = "SideBySide\\2.0";

		// Token: 0x0400012D RID: 301
		public const string RandomValueName = "ComponentStore_RandomString";

		// Token: 0x0400012E RID: 302
		public const string Sentinel35SP1Update = "ClickOnce35SP1Update";

		// Token: 0x0400012F RID: 303
		public const string LUAPolicyKeyName = "Software\\Microsoft\\Windows\\CurrentVersion\\Policies\\System";

		// Token: 0x04000130 RID: 304
		public const string OnlineAppQuotaInKBValueName = "OnlineAppQuotaInKB";

		// Token: 0x04000131 RID: 305
		public const string OnlineAppQuotaUsageEstimateValueName = "OnlineAppQuotaUsageEstimate";

		// Token: 0x04000132 RID: 306
		public const string LUAPolicyValueName = "EnableLUA";

		// Token: 0x04000133 RID: 307
		public const string ClassesSubKeyName = "Software\\Classes";

		// Token: 0x04000134 RID: 308
		public const string AppIdValueName = "AppId";

		// Token: 0x04000135 RID: 309
		public const string DPUrlValueName = "DeploymentProviderUrl";

		// Token: 0x04000136 RID: 310
		public const string IconFileValueName = "IconFile";

		// Token: 0x04000137 RID: 311
		public const string ContentTypeValueName = "Content Type";

		// Token: 0x04000138 RID: 312
		public const string ShellKeyName = "shell";

		// Token: 0x04000139 RID: 313
		public const string OpenCommandKeyName = "open\\command";

		// Token: 0x0400013A RID: 314
		public const string IconHandlerKeyName = "shellex\\IconHandler";

		// Token: 0x0400013B RID: 315
		public const string CLSIDKeyName = "CLSID";

		// Token: 0x0400013C RID: 316
		public const string InProcServerKeyName = "InProcServer32";

		// Token: 0x0400013D RID: 317
		public const string GuidValueName = "Guid";

		// Token: 0x0400013E RID: 318
		public const string RootKeyName = "Software\\Microsoft\\.NETFramework\\DeploymentFramework";

		// Token: 0x0400013F RID: 319
		public const string RequireSignedManifests = "RequireSignedManifests";

		// Token: 0x04000140 RID: 320
		public const string RequireHashInManifests = "RequireHashInManifests";

		// Token: 0x04000141 RID: 321
		public const string SkipSignatureValidationValueName = "SkipSignatureValidation";

		// Token: 0x04000142 RID: 322
		public const string SkipDeploymentProviderValueName = "SkipDeploymentProvider";

		// Token: 0x04000143 RID: 323
		public const string SkipSchemaValidationValueName = "SkipSchemaValidation";

		// Token: 0x04000144 RID: 324
		public const string SkipSemanticValidationValueName = "SkipSemanticValidation";

		// Token: 0x04000145 RID: 325
		public const string SkipApplicationDependencyHashCheckValueName = "SkipApplicationDependencyHashCheck";

		// Token: 0x04000146 RID: 326
		public const string SuppressLimitOnNumberOfActivationsValueName = "SuppressLimitOnNumberOfActivations";

		// Token: 0x04000147 RID: 327
		public const string DisableGenericExceptionHandler = "DisableGenericExceptionHandler";

		// Token: 0x04000148 RID: 328
		public const string DeploymentManifestSuffix = ".application";

		// Token: 0x04000149 RID: 329
		public const string ManifestSuffix = ".manifest";

		// Token: 0x0400014A RID: 330
		public const string DllSuffix = ".dll";

		// Token: 0x0400014B RID: 331
		public const string ExeSuffix = ".exe";

		// Token: 0x0400014C RID: 332
		public const string MapFileExtensionsSuffix = ".deploy";

		// Token: 0x0400014D RID: 333
		public const string InstallReferenceIdentifier = "{3f471841-eef2-47d6-89c0-d028f03a4ad5}";

		// Token: 0x0400014E RID: 334
		public const string SubscriptionStoreLock = "__SubscriptionStoreLock__";

		// Token: 0x0400014F RID: 335
		public const string IsShellVisible = "IsShellVisible";

		// Token: 0x04000150 RID: 336
		public const string CurrentBind = "CurrentBind";

		// Token: 0x04000151 RID: 337
		public const string PreviousBind = "PreviousBind";

		// Token: 0x04000152 RID: 338
		public const string PendingBind = "PendingBind";

		// Token: 0x04000153 RID: 339
		public const string ExcludedDeployment = "ExcludedDeployment";

		// Token: 0x04000154 RID: 340
		public const string PendingDeployment = "PendingDeployment";

		// Token: 0x04000155 RID: 341
		public const string DeploymentProviderUri = "DeploymentProviderUri";

		// Token: 0x04000156 RID: 342
		public const string MinimumRequiredVersion = "MinimumRequiredVersion";

		// Token: 0x04000157 RID: 343
		public const string LastCheckTime = "LastCheckTime";

		// Token: 0x04000158 RID: 344
		public const string UpdateSkipTime = "UpdateSkipTime";

		// Token: 0x04000159 RID: 345
		public const string UpdateSkippedDeployment = "UpdateSkippedDeployment";

		// Token: 0x0400015A RID: 346
		public const string AppType = "AppType";

		// Token: 0x0400015B RID: 347
		public const string UseApplicationManifestDescription = "UseApplicationManifestDescription";

		// Token: 0x0400015C RID: 348
		public const string DeploymentSourceUri = "DeploymentSourceUri";

		// Token: 0x0400015D RID: 349
		public const string ApplicationSourceUri = "ApplicationSourceUri";

		// Token: 0x0400015E RID: 350
		public const string IsFullTrust = "IsFullTrust";

		// Token: 0x0400015F RID: 351
		public const string CLRCoreComp = "Microsoft-Windows-CLRCoreComp";

		// Token: 0x04000160 RID: 352
		public const string CommonLanguageRuntime = "Microsoft.Windows.CommonLanguageRuntime";

		// Token: 0x04000161 RID: 353
		public const uint MinVersionCLRMajor = 2U;

		// Token: 0x04000162 RID: 354
		public const string MSIL = "msil";

		// Token: 0x04000163 RID: 355
		public const string X86 = "x86";

		// Token: 0x04000164 RID: 356
		public const string AMD64 = "amd64";

		// Token: 0x04000165 RID: 357
		public const string IA64 = "ia64";

		// Token: 0x04000166 RID: 358
		public const string AsmV1Namespace = "urn:schemas-microsoft-com:asm.v1";

		// Token: 0x04000167 RID: 359
		public const string AsmV2Namespace = "urn:schemas-microsoft-com:asm.v2";

		// Token: 0x04000168 RID: 360
		public const string XmlDSigNamespace = "http://www.w3.org/2000/09/xmldsig#";

		// Token: 0x04000169 RID: 361
		public const string AdaptiveSchemaResourceName = "manifest.2.0.0.15-pre.adaptive.xsd";

		// Token: 0x0400016A RID: 362
		public const string PublicIdForDTD4XMLSchemas = "-//W3C//DTD XMLSCHEMA 200102//EN";

		// Token: 0x0400016B RID: 363
		public const string XMLSchemaDTDResourceName = "XMLSchema.dtd";

		// Token: 0x0400016C RID: 364
		public const string PublicIdForDTD4DataTypes = "xs-datatypes";

		// Token: 0x0400016D RID: 365
		public const string DataTypesDTDResourceName = "datatypes.dtd";

		// Token: 0x0400016E RID: 366
		public const string UnsignedPublicKeyToken = "0000000000000000";

		// Token: 0x0400016F RID: 367
		public const string RequireAdministrator = "requireAdministrator";

		// Token: 0x04000170 RID: 368
		public const string HighestAvailable = "highestAvailable";

		// Token: 0x04000171 RID: 369
		public const int MaxNumberOfFilesInApplication = 24576;

		// Token: 0x04000172 RID: 370
		public const int MaxNumberOfFileAssociationsInApplication = 8;

		// Token: 0x04000173 RID: 371
		public const int MaxNumberOfFileExtensionLength = 24;

		// Token: 0x04000174 RID: 372
		public const int MaxNumberOfAssembliesInApplication = 24576;

		// Token: 0x04000175 RID: 373
		public const int MaxNumberOfGroupsInApplication = 49152;

		// Token: 0x04000176 RID: 374
		public const int MaxUrlLength = 16384;

		// Token: 0x04000177 RID: 375
		public const int MaxLiveActivation = 8;

		// Token: 0x04000178 RID: 376
		public const int MaxIdentityLength = 2048;

		// Token: 0x04000179 RID: 377
		public const int MaxAppIdLength = 65536;

		// Token: 0x0400017A RID: 378
		public const int MaxShortcutFileSize = 65536;

		// Token: 0x0400017B RID: 379
		public const int MaxManifestFileSize = 16777216;

		// Token: 0x0400017C RID: 380
		public const int MaxValueForMaximumAge = 365;

		// Token: 0x0400017D RID: 381
		public const int MaxErrorUrlLength = 2048;

		// Token: 0x0400017E RID: 382
		public const uint DefaultOnlineAppQuotaInKB = 256000U;

		// Token: 0x0400017F RID: 383
		public const int LifetimeDefaultMinutes = 10;

		// Token: 0x04000180 RID: 384
		public const int LockRetryIntervalMs = 1;

		// Token: 0x04000181 RID: 385
		public const int MinProgressCallbackIntervalInMs = 100;

		// Token: 0x04000182 RID: 386
		public const string DefaultLogTextualId = "__DeploymentDefaultLogFile__";

		// Token: 0x04000183 RID: 387
		public const int MAX_PATH = 260;

		// Token: 0x04000184 RID: 388
		public const string LogFileExtension = "log";

		// Token: 0x04000185 RID: 389
		public const string LogFilePathRegistryString = "LogFilePath";

		// Token: 0x04000186 RID: 390
		public const string WininetCacheLogUrlPrefix = "System_Deployment_Log_";

		// Token: 0x04000187 RID: 391
		public const string GACDetectionTempManifestAsmIdText = "GACDetectionTempManifest, version=1.0.0.0, type=win32";

		// Token: 0x04000188 RID: 392
		public const string DataDirectory = "DataDirectory";

		// Token: 0x04000189 RID: 393
		public const int HRESULT_DiskFull = -2147024784;

		// Token: 0x0400018A RID: 394
		public const uint MASK_NOTPINNABLE = 2147483648U;

		// Token: 0x0400018B RID: 395
		public const string Client35SP1SignatureAssembly = "Sentinel.v3.5Client, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a,processorArchitecture=msil";

		// Token: 0x0400018C RID: 396
		public const string Full35SP1SignatureAssembly = "System.Data.Entity, Version=3.5.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089,processorArchitecture=msil";

		// Token: 0x0400018D RID: 397
		public const string DotNetFX35SP1 = ".NET Framework 3.5 SP1";

		// Token: 0x0400018E RID: 398
		public const string SkipSKUDetectionKeyName = "SOFTWARE\\Microsoft\\Fusion";

		// Token: 0x0400018F RID: 399
		public const string SkipSKUDetectionValueName = "NoClientChecks";

		// Token: 0x04000190 RID: 400
		public const int CLSCTX_INPROC_SERVER = 1;

		// Token: 0x04000191 RID: 401
		public static Guid DeploymentPropertySet = new Guid("2ad613da-6fdb-4671-af9e-18ab2e4df4d8");

		// Token: 0x04000192 RID: 402
		public static TimeSpan OnlineAppScavengingGracePeriod = TimeSpan.FromMinutes(1.0);

		// Token: 0x04000193 RID: 403
		public static TimeSpan LockTimeout = TimeSpan.FromMinutes(2.0);

		// Token: 0x04000194 RID: 404
		public static TimeSpan AssertApplicationRequirementsTimeout = TimeSpan.FromMinutes(2.0);

		// Token: 0x04000195 RID: 405
		public static Guid CLSID_StartMenuPin = new Guid("A2A9545D-A0C2-42B4-9708-A0B2BADD77C8");

		// Token: 0x04000196 RID: 406
		public static Guid IID_IUnknown = new Guid("00000000-0000-0000-C000-000000000046");

		// Token: 0x04000197 RID: 407
		public static Guid uuid = new Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe");
	}
}
