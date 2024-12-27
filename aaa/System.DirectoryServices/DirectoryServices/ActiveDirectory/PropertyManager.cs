using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000D0 RID: 208
	internal class PropertyManager
	{
		// Token: 0x06000677 RID: 1655 RVA: 0x00022654 File Offset: 0x00021654
		public static object GetPropertyValue(DirectoryEntry directoryEntry, string propertyName)
		{
			return PropertyManager.GetPropertyValue(null, directoryEntry, propertyName);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x00022660 File Offset: 0x00021660
		public static object GetPropertyValue(DirectoryContext context, DirectoryEntry directoryEntry, string propertyName)
		{
			try
			{
				if (directoryEntry.Properties[propertyName].Count == 0)
				{
					if (directoryEntry.Properties[PropertyManager.DistinguishedName].Count != 0)
					{
						throw new ActiveDirectoryOperationException(Res.GetString("PropertyNotFoundOnObject", new object[]
						{
							propertyName,
							directoryEntry.Properties[PropertyManager.DistinguishedName].Value
						}));
					}
					throw new ActiveDirectoryOperationException(Res.GetString("PropertyNotFound", new object[] { propertyName }));
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			return directoryEntry.Properties[propertyName].Value;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x00022714 File Offset: 0x00021714
		public static object GetSearchResultPropertyValue(SearchResult res, string propertyName)
		{
			ResultPropertyValueCollection resultPropertyValueCollection = null;
			try
			{
				resultPropertyValueCollection = res.Properties[propertyName];
				if (resultPropertyValueCollection == null || resultPropertyValueCollection.Count < 1)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("PropertyNotFound", new object[] { propertyName }));
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(ex);
			}
			return resultPropertyValueCollection[0];
		}

		// Token: 0x040004FF RID: 1279
		public static string DefaultNamingContext = "defaultNamingContext";

		// Token: 0x04000500 RID: 1280
		public static string SchemaNamingContext = "schemaNamingContext";

		// Token: 0x04000501 RID: 1281
		public static string ConfigurationNamingContext = "configurationNamingContext";

		// Token: 0x04000502 RID: 1282
		public static string RootDomainNamingContext = "rootDomainNamingContext";

		// Token: 0x04000503 RID: 1283
		public static string MsDSBehaviorVersion = "msDS-Behavior-Version";

		// Token: 0x04000504 RID: 1284
		public static string FsmoRoleOwner = "fsmoRoleOwner";

		// Token: 0x04000505 RID: 1285
		public static string ForestFunctionality = "forestFunctionality";

		// Token: 0x04000506 RID: 1286
		public static string NTMixedDomain = "ntMixedDomain";

		// Token: 0x04000507 RID: 1287
		public static string DomainFunctionality = "domainFunctionality";

		// Token: 0x04000508 RID: 1288
		public static string ObjectCategory = "objectCategory";

		// Token: 0x04000509 RID: 1289
		public static string SystemFlags = "systemFlags";

		// Token: 0x0400050A RID: 1290
		public static string DnsRoot = "dnsRoot";

		// Token: 0x0400050B RID: 1291
		public static string DistinguishedName = "distinguishedName";

		// Token: 0x0400050C RID: 1292
		public static string TrustParent = "trustParent";

		// Token: 0x0400050D RID: 1293
		public static string FlatName = "flatName";

		// Token: 0x0400050E RID: 1294
		public static string Name = "name";

		// Token: 0x0400050F RID: 1295
		public static string Flags = "flags";

		// Token: 0x04000510 RID: 1296
		public static string TrustType = "trustType";

		// Token: 0x04000511 RID: 1297
		public static string TrustAttributes = "trustAttributes";

		// Token: 0x04000512 RID: 1298
		public static string BecomeSchemaMaster = "becomeSchemaMaster";

		// Token: 0x04000513 RID: 1299
		public static string BecomeDomainMaster = "becomeDomainMaster";

		// Token: 0x04000514 RID: 1300
		public static string BecomePdc = "becomePdc";

		// Token: 0x04000515 RID: 1301
		public static string BecomeRidMaster = "becomeRidMaster";

		// Token: 0x04000516 RID: 1302
		public static string BecomeInfrastructureMaster = "becomeInfrastructureMaster";

		// Token: 0x04000517 RID: 1303
		public static string DnsHostName = "dnsHostName";

		// Token: 0x04000518 RID: 1304
		public static string Options = "options";

		// Token: 0x04000519 RID: 1305
		public static string CurrentTime = "currentTime";

		// Token: 0x0400051A RID: 1306
		public static string HighestCommittedUSN = "highestCommittedUSN";

		// Token: 0x0400051B RID: 1307
		public static string OperatingSystem = "operatingSystem";

		// Token: 0x0400051C RID: 1308
		public static string HasMasterNCs = "hasMasterNCs";

		// Token: 0x0400051D RID: 1309
		public static string MsDSHasMasterNCs = "msDS-HasMasterNCs";

		// Token: 0x0400051E RID: 1310
		public static string MsDSHasFullReplicaNCs = "msDS-hasFullReplicaNCs";

		// Token: 0x0400051F RID: 1311
		public static string NCName = "nCName";

		// Token: 0x04000520 RID: 1312
		public static string Cn = "cn";

		// Token: 0x04000521 RID: 1313
		public static string NETBIOSName = "nETBIOSName";

		// Token: 0x04000522 RID: 1314
		public static string DomainDNS = "domainDNS";

		// Token: 0x04000523 RID: 1315
		public static string InstanceType = "instanceType";

		// Token: 0x04000524 RID: 1316
		public static string MsDSSDReferenceDomain = "msDS-SDReferenceDomain";

		// Token: 0x04000525 RID: 1317
		public static string MsDSPortLDAP = "msDS-PortLDAP";

		// Token: 0x04000526 RID: 1318
		public static string MsDSPortSSL = "msDS-PortSSL";

		// Token: 0x04000527 RID: 1319
		public static string MsDSNCReplicaLocations = "msDS-NC-Replica-Locations";

		// Token: 0x04000528 RID: 1320
		public static string MsDSNCROReplicaLocations = "msDS-NC-RO-Replica-Locations";

		// Token: 0x04000529 RID: 1321
		public static string SupportedCapabilities = "supportedCapabilities";

		// Token: 0x0400052A RID: 1322
		public static string ServerName = "serverName";

		// Token: 0x0400052B RID: 1323
		public static string Enabled = "Enabled";

		// Token: 0x0400052C RID: 1324
		public static string ObjectGuid = "objectGuid";

		// Token: 0x0400052D RID: 1325
		public static string Keywords = "keywords";

		// Token: 0x0400052E RID: 1326
		public static string ServiceBindingInformation = "serviceBindingInformation";

		// Token: 0x0400052F RID: 1327
		public static string MsDSReplAuthenticationMode = "msDS-ReplAuthenticationMode";

		// Token: 0x04000530 RID: 1328
		public static string HasPartialReplicaNCs = "hasPartialReplicaNCs";

		// Token: 0x04000531 RID: 1329
		public static string Container = "container";

		// Token: 0x04000532 RID: 1330
		public static string LdapDisplayName = "ldapDisplayName";

		// Token: 0x04000533 RID: 1331
		public static string AttributeID = "attributeID";

		// Token: 0x04000534 RID: 1332
		public static string AttributeSyntax = "attributeSyntax";

		// Token: 0x04000535 RID: 1333
		public static string Description = "description";

		// Token: 0x04000536 RID: 1334
		public static string SearchFlags = "searchFlags";

		// Token: 0x04000537 RID: 1335
		public static string OMSyntax = "oMSyntax";

		// Token: 0x04000538 RID: 1336
		public static string OMObjectClass = "oMObjectClass";

		// Token: 0x04000539 RID: 1337
		public static string IsSingleValued = "isSingleValued";

		// Token: 0x0400053A RID: 1338
		public static string IsDefunct = "isDefunct";

		// Token: 0x0400053B RID: 1339
		public static string RangeUpper = "rangeUpper";

		// Token: 0x0400053C RID: 1340
		public static string RangeLower = "rangeLower";

		// Token: 0x0400053D RID: 1341
		public static string IsMemberOfPartialAttributeSet = "isMemberOfPartialAttributeSet";

		// Token: 0x0400053E RID: 1342
		public static string ObjectVersion = "objectVersion";

		// Token: 0x0400053F RID: 1343
		public static string LinkID = "linkID";

		// Token: 0x04000540 RID: 1344
		public static string ObjectClassCategory = "objectClassCategory";

		// Token: 0x04000541 RID: 1345
		public static string SchemaUpdateNow = "schemaUpdateNow";

		// Token: 0x04000542 RID: 1346
		public static string SubClassOf = "subClassOf";

		// Token: 0x04000543 RID: 1347
		public static string SchemaIDGuid = "schemaIDGUID";

		// Token: 0x04000544 RID: 1348
		public static string PossibleSuperiors = "possSuperiors";

		// Token: 0x04000545 RID: 1349
		public static string PossibleInferiors = "possibleInferiors";

		// Token: 0x04000546 RID: 1350
		public static string MustContain = "mustContain";

		// Token: 0x04000547 RID: 1351
		public static string MayContain = "mayContain";

		// Token: 0x04000548 RID: 1352
		public static string SystemMustContain = "systemMustContain";

		// Token: 0x04000549 RID: 1353
		public static string SystemMayContain = "systemMayContain";

		// Token: 0x0400054A RID: 1354
		public static string GovernsID = "governsID";

		// Token: 0x0400054B RID: 1355
		public static string IsGlobalCatalogReady = "isGlobalCatalogReady";

		// Token: 0x0400054C RID: 1356
		public static string NTSecurityDescriptor = "ntSecurityDescriptor";

		// Token: 0x0400054D RID: 1357
		public static string DsServiceName = "dsServiceName";

		// Token: 0x0400054E RID: 1358
		public static string ReplicateSingleObject = "replicateSingleObject";

		// Token: 0x0400054F RID: 1359
		public static string MsDSMasteredBy = "msDS-masteredBy";

		// Token: 0x04000550 RID: 1360
		public static string DefaultSecurityDescriptor = "defaultSecurityDescriptor";

		// Token: 0x04000551 RID: 1361
		public static string NamingContexts = "namingContexts";

		// Token: 0x04000552 RID: 1362
		public static string MsDSDefaultNamingContext = "msDS-DefaultNamingContext";

		// Token: 0x04000553 RID: 1363
		public static string OperatingSystemVersion = "operatingSystemVersion";

		// Token: 0x04000554 RID: 1364
		public static string AuxiliaryClass = "auxiliaryClass";

		// Token: 0x04000555 RID: 1365
		public static string SystemAuxiliaryClass = "systemAuxiliaryClass";

		// Token: 0x04000556 RID: 1366
		public static string SystemPossibleSuperiors = "systemPossSuperiors";

		// Token: 0x04000557 RID: 1367
		public static string InterSiteTopologyGenerator = "interSiteTopologyGenerator";

		// Token: 0x04000558 RID: 1368
		public static string FromServer = "fromServer";

		// Token: 0x04000559 RID: 1369
		public static string SiteList = "siteList";

		// Token: 0x0400055A RID: 1370
		public static string MsDSHasInstantiatedNCs = "msDS-HasInstantiatedNCs";
	}
}
