using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.DirectoryServices
{
	// Token: 0x02000007 RID: 7
	internal sealed class Res
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002114 File Offset: 0x00001114
		private static object InternalSyncObject
		{
			get
			{
				if (Res.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref Res.s_InternalSyncObject, obj, null);
				}
				return Res.s_InternalSyncObject;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002140 File Offset: 0x00001140
		internal Res()
		{
			this.resources = new ResourceManager("System.DirectoryServices", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00001164
		private static Res GetLoader()
		{
			if (Res.loader == null)
			{
				lock (Res.InternalSyncObject)
				{
					if (Res.loader == null)
					{
						Res.loader = new Res();
					}
				}
			}
			return Res.loader;
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021B4 File Offset: 0x000011B4
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021B7 File Offset: 0x000011B7
		public static ResourceManager Resources
		{
			get
			{
				return Res.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000011C4
		public static string GetString(string name, params object[] args)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			string @string = res.resources.GetString(name, Res.Culture);
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

		// Token: 0x0600000B RID: 11 RVA: 0x00002248 File Offset: 0x00001248
		public static string GetString(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetString(name, Res.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002274 File Offset: 0x00001274
		public static object GetObject(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetObject(name, Res.Culture);
		}

		// Token: 0x04000031 RID: 49
		internal const string DSDoesNotImplementIADs = "DSDoesNotImplementIADs";

		// Token: 0x04000032 RID: 50
		internal const string DSNoObject = "DSNoObject";

		// Token: 0x04000033 RID: 51
		internal const string DSInvalidPath = "DSInvalidPath";

		// Token: 0x04000034 RID: 52
		internal const string DSNotAContainer = "DSNotAContainer";

		// Token: 0x04000035 RID: 53
		internal const string DSCannotDelete = "DSCannotDelete";

		// Token: 0x04000036 RID: 54
		internal const string DSNotInCollection = "DSNotInCollection";

		// Token: 0x04000037 RID: 55
		internal const string DSNoCurrentChild = "DSNoCurrentChild";

		// Token: 0x04000038 RID: 56
		internal const string DSCannotBeIndexed = "DSCannotBeIndexed";

		// Token: 0x04000039 RID: 57
		internal const string DSCannotCount = "DSCannotCount";

		// Token: 0x0400003A RID: 58
		internal const string DSCannotGetKeys = "DSCannotGetKeys";

		// Token: 0x0400003B RID: 59
		internal const string DSCannotEmunerate = "DSCannotEmunerate";

		// Token: 0x0400003C RID: 60
		internal const string DSNoCurrentProperty = "DSNoCurrentProperty";

		// Token: 0x0400003D RID: 61
		internal const string DSNoCurrentValue = "DSNoCurrentValue";

		// Token: 0x0400003E RID: 62
		internal const string DSBadPageSize = "DSBadPageSize";

		// Token: 0x0400003F RID: 63
		internal const string DSBadSizeLimit = "DSBadSizeLimit";

		// Token: 0x04000040 RID: 64
		internal const string DSSearchUnsupported = "DSSearchUnsupported";

		// Token: 0x04000041 RID: 65
		internal const string DSNoCurrentEntry = "DSNoCurrentEntry";

		// Token: 0x04000042 RID: 66
		internal const string DSInvalidSearchFilter = "DSInvalidSearchFilter";

		// Token: 0x04000043 RID: 67
		internal const string DSPropertyNotFound = "DSPropertyNotFound";

		// Token: 0x04000044 RID: 68
		internal const string DSConvertFailed = "DSConvertFailed";

		// Token: 0x04000045 RID: 69
		internal const string DSConvertTypeInvalid = "DSConvertTypeInvalid";

		// Token: 0x04000046 RID: 70
		internal const string DSAdsvalueTypeNYI = "DSAdsvalueTypeNYI";

		// Token: 0x04000047 RID: 71
		internal const string DSAdsiNotInstalled = "DSAdsiNotInstalled";

		// Token: 0x04000048 RID: 72
		internal const string DSNotSet = "DSNotSet";

		// Token: 0x04000049 RID: 73
		internal const string DSEnumerator = "DSEnumerator";

		// Token: 0x0400004A RID: 74
		internal const string DSPathIsNotSet = "DSPathIsNotSet";

		// Token: 0x0400004B RID: 75
		internal const string DSPropertySetSupported = "DSPropertySetSupported";

		// Token: 0x0400004C RID: 76
		internal const string DSAddNotSupported = "DSAddNotSupported";

		// Token: 0x0400004D RID: 77
		internal const string DSClearNotSupported = "DSClearNotSupported";

		// Token: 0x0400004E RID: 78
		internal const string DSRemoveNotSupported = "DSRemoveNotSupported";

		// Token: 0x0400004F RID: 79
		internal const string DSSearchPreferencesNotAccepted = "DSSearchPreferencesNotAccepted";

		// Token: 0x04000050 RID: 80
		internal const string DSBeforeCount = "DSBeforeCount";

		// Token: 0x04000051 RID: 81
		internal const string DSBadBeforeCount = "DSBadBeforeCount";

		// Token: 0x04000052 RID: 82
		internal const string DSAfterCount = "DSAfterCount";

		// Token: 0x04000053 RID: 83
		internal const string DSBadAfterCount = "DSBadAfterCount";

		// Token: 0x04000054 RID: 84
		internal const string DSOffset = "DSOffset";

		// Token: 0x04000055 RID: 85
		internal const string DSBadOffset = "DSBadOffset";

		// Token: 0x04000056 RID: 86
		internal const string DSTargetPercentage = "DSTargetPercentage";

		// Token: 0x04000057 RID: 87
		internal const string DSBadTargetPercentage = "DSBadTargetPercentage";

		// Token: 0x04000058 RID: 88
		internal const string DSTarget = "DSTarget";

		// Token: 0x04000059 RID: 89
		internal const string DSApproximateTotal = "DSApproximateTotal";

		// Token: 0x0400005A RID: 90
		internal const string DSBadApproximateTotal = "DSBadApproximateTotal";

		// Token: 0x0400005B RID: 91
		internal const string DSDirectoryVirtualListViewContext = "DSDirectoryVirtualListViewContext";

		// Token: 0x0400005C RID: 92
		internal const string DSVirtualListView = "DSVirtualListView";

		// Token: 0x0400005D RID: 93
		internal const string DSBadPageSizeDirsync = "DSBadPageSizeDirsync";

		// Token: 0x0400005E RID: 94
		internal const string DSBadCacheResultsVLV = "DSBadCacheResultsVLV";

		// Token: 0x0400005F RID: 95
		internal const string DSBadDirectorySynchronizationFlag = "DSBadDirectorySynchronizationFlag";

		// Token: 0x04000060 RID: 96
		internal const string DSBadASQSearchScope = "DSBadASQSearchScope";

		// Token: 0x04000061 RID: 97
		internal const string DSDoesNotImplementIADsObjectOptions = "DSDoesNotImplementIADsObjectOptions";

		// Token: 0x04000062 RID: 98
		internal const string DSPropertyValueSupportOneOperation = "DSPropertyValueSupportOneOperation";

		// Token: 0x04000063 RID: 99
		internal const string ConfigSectionsUnique = "ConfigSectionsUnique";

		// Token: 0x04000064 RID: 100
		internal const string Invalid_boolean_attribute = "Invalid_boolean_attribute";

		// Token: 0x04000065 RID: 101
		internal const string DSUnknownFailure = "DSUnknownFailure";

		// Token: 0x04000066 RID: 102
		internal const string DSNotSupportOnClient = "DSNotSupportOnClient";

		// Token: 0x04000067 RID: 103
		internal const string DSNotSupportOnDC = "DSNotSupportOnDC";

		// Token: 0x04000068 RID: 104
		internal const string DirectoryContextNeedHost = "DirectoryContextNeedHost";

		// Token: 0x04000069 RID: 105
		internal const string DSSyncAllFailure = "DSSyncAllFailure";

		// Token: 0x0400006A RID: 106
		internal const string UnknownTransport = "UnknownTransport";

		// Token: 0x0400006B RID: 107
		internal const string NotSupportTransportSMTP = "NotSupportTransportSMTP";

		// Token: 0x0400006C RID: 108
		internal const string CannotDelete = "CannotDelete";

		// Token: 0x0400006D RID: 109
		internal const string CannotGetObject = "CannotGetObject";

		// Token: 0x0400006E RID: 110
		internal const string DSNotFound = "DSNotFound";

		// Token: 0x0400006F RID: 111
		internal const string InvalidContextTarget = "InvalidContextTarget";

		// Token: 0x04000070 RID: 112
		internal const string TransportNotFound = "TransportNotFound";

		// Token: 0x04000071 RID: 113
		internal const string SiteNotExist = "SiteNotExist";

		// Token: 0x04000072 RID: 114
		internal const string SiteNotCommitted = "SiteNotCommitted";

		// Token: 0x04000073 RID: 115
		internal const string NoCurrentSite = "NoCurrentSite";

		// Token: 0x04000074 RID: 116
		internal const string SubnetNotCommitted = "SubnetNotCommitted";

		// Token: 0x04000075 RID: 117
		internal const string SiteLinkNotCommitted = "SiteLinkNotCommitted";

		// Token: 0x04000076 RID: 118
		internal const string ConnectionNotCommitted = "ConnectionNotCommitted";

		// Token: 0x04000077 RID: 119
		internal const string AlreadyExistingForestTrust = "AlreadyExistingForestTrust";

		// Token: 0x04000078 RID: 120
		internal const string AlreadyExistingDomainTrust = "AlreadyExistingDomainTrust";

		// Token: 0x04000079 RID: 121
		internal const string NotFoundInCollection = "NotFoundInCollection";

		// Token: 0x0400007A RID: 122
		internal const string AlreadyExistingInCollection = "AlreadyExistingInCollection";

		// Token: 0x0400007B RID: 123
		internal const string NTDSSiteSetting = "NTDSSiteSetting";

		// Token: 0x0400007C RID: 124
		internal const string NotWithinSite = "NotWithinSite";

		// Token: 0x0400007D RID: 125
		internal const string InvalidTime = "InvalidTime";

		// Token: 0x0400007E RID: 126
		internal const string EmptyStringParameter = "EmptyStringParameter";

		// Token: 0x0400007F RID: 127
		internal const string SupportedPlatforms = "SupportedPlatforms";

		// Token: 0x04000080 RID: 128
		internal const string TargetShouldBeADAMServer = "TargetShouldBeADAMServer";

		// Token: 0x04000081 RID: 129
		internal const string TargetShouldBeDC = "TargetShouldBeDC";

		// Token: 0x04000082 RID: 130
		internal const string TargetShouldBeAppNCDnsName = "TargetShouldBeAppNCDnsName";

		// Token: 0x04000083 RID: 131
		internal const string TargetShouldBeServerORForest = "TargetShouldBeServerORForest";

		// Token: 0x04000084 RID: 132
		internal const string TargetShouldBeServerORDomain = "TargetShouldBeServerORDomain";

		// Token: 0x04000085 RID: 133
		internal const string TargetShouldBeDomain = "TargetShouldBeDomain";

		// Token: 0x04000086 RID: 134
		internal const string TargetShouldBeForest = "TargetShouldBeForest";

		// Token: 0x04000087 RID: 135
		internal const string TargetShouldBeConfigSet = "TargetShouldBeConfigSet";

		// Token: 0x04000088 RID: 136
		internal const string TargetShouldBeServerORConfigSet = "TargetShouldBeServerORConfigSet";

		// Token: 0x04000089 RID: 137
		internal const string TargetShouldBeGC = "TargetShouldBeGC";

		// Token: 0x0400008A RID: 138
		internal const string TargetShouldBeServer = "TargetShouldBeServer";

		// Token: 0x0400008B RID: 139
		internal const string NotADOrADAM = "NotADOrADAM";

		// Token: 0x0400008C RID: 140
		internal const string ServerNotAReplica = "ServerNotAReplica";

		// Token: 0x0400008D RID: 141
		internal const string AppNCNotFound = "AppNCNotFound";

		// Token: 0x0400008E RID: 142
		internal const string ReplicaNotFound = "ReplicaNotFound";

		// Token: 0x0400008F RID: 143
		internal const string GCNotFoundInForest = "GCNotFoundInForest";

		// Token: 0x04000090 RID: 144
		internal const string DCNotFoundInDomain = "DCNotFoundInDomain";

		// Token: 0x04000091 RID: 145
		internal const string ADAMInstanceNotFoundInConfigSet = "ADAMInstanceNotFoundInConfigSet";

		// Token: 0x04000092 RID: 146
		internal const string DCNotFound = "DCNotFound";

		// Token: 0x04000093 RID: 147
		internal const string GCNotFound = "GCNotFound";

		// Token: 0x04000094 RID: 148
		internal const string AINotFound = "AINotFound";

		// Token: 0x04000095 RID: 149
		internal const string ServerNotFound = "ServerNotFound";

		// Token: 0x04000096 RID: 150
		internal const string DomainNotFound = "DomainNotFound";

		// Token: 0x04000097 RID: 151
		internal const string ForestNotFound = "ForestNotFound";

		// Token: 0x04000098 RID: 152
		internal const string ConfigSetNotFound = "ConfigSetNotFound";

		// Token: 0x04000099 RID: 153
		internal const string NDNCNotFound = "NDNCNotFound";

		// Token: 0x0400009A RID: 154
		internal const string PropertyNotFoundOnObject = "PropertyNotFoundOnObject";

		// Token: 0x0400009B RID: 155
		internal const string PropertyNotFound = "PropertyNotFound";

		// Token: 0x0400009C RID: 156
		internal const string PropertyNotSet = "PropertyNotSet";

		// Token: 0x0400009D RID: 157
		internal const string ADAMInstanceNotFound = "ADAMInstanceNotFound";

		// Token: 0x0400009E RID: 158
		internal const string CannotPerformOperationOnUncommittedObject = "CannotPerformOperationOnUncommittedObject";

		// Token: 0x0400009F RID: 159
		internal const string LinkIdNotEvenNumber = "LinkIdNotEvenNumber";

		// Token: 0x040000A0 RID: 160
		internal const string InvalidServerNameFormat = "InvalidServerNameFormat";

		// Token: 0x040000A1 RID: 161
		internal const string NoObjectClassForADPartition = "NoObjectClassForADPartition";

		// Token: 0x040000A2 RID: 162
		internal const string InvalidDNFormat = "InvalidDNFormat";

		// Token: 0x040000A3 RID: 163
		internal const string InvalidDnsName = "InvalidDnsName";

		// Token: 0x040000A4 RID: 164
		internal const string ApplicationPartitionTypeUnknown = "ApplicationPartitionTypeUnknown";

		// Token: 0x040000A5 RID: 165
		internal const string UnknownSyntax = "UnknownSyntax";

		// Token: 0x040000A6 RID: 166
		internal const string InvalidMode = "InvalidMode";

		// Token: 0x040000A7 RID: 167
		internal const string NoW2K3DCs = "NoW2K3DCs";

		// Token: 0x040000A8 RID: 168
		internal const string DCInfoNotFound = "DCInfoNotFound";

		// Token: 0x040000A9 RID: 169
		internal const string NoW2K3DCsInForest = "NoW2K3DCsInForest";

		// Token: 0x040000AA RID: 170
		internal const string SchemaObjectNotCommitted = "SchemaObjectNotCommitted";

		// Token: 0x040000AB RID: 171
		internal const string InvalidFlags = "InvalidFlags";

		// Token: 0x040000AC RID: 172
		internal const string CannotPerformOnGCObject = "CannotPerformOnGCObject";

		// Token: 0x040000AD RID: 173
		internal const string CannotPerformOnGC = "CannotPerformOnGC";

		// Token: 0x040000AE RID: 174
		internal const string ValueCannotBeModified = "ValueCannotBeModified";

		// Token: 0x040000AF RID: 175
		internal const string ServerShouldBeW2K3 = "ServerShouldBeW2K3";

		// Token: 0x040000B0 RID: 176
		internal const string LinkedPropertyNotFound = "LinkedPropertyNotFound";

		// Token: 0x040000B1 RID: 177
		internal const string GCDisabled = "GCDisabled";

		// Token: 0x040000B2 RID: 178
		internal const string PropertyInvalidForADAM = "PropertyInvalidForADAM";

		// Token: 0x040000B3 RID: 179
		internal const string OperationInvalidForADAM = "OperationInvalidForADAM";

		// Token: 0x040000B4 RID: 180
		internal const string ContextNotAssociatedWithDomain = "ContextNotAssociatedWithDomain";

		// Token: 0x040000B5 RID: 181
		internal const string ComputerNotJoinedToDomain = "ComputerNotJoinedToDomain";

		// Token: 0x040000B6 RID: 182
		internal const string VersionFailure = "VersionFailure";

		// Token: 0x040000B7 RID: 183
		internal const string NoHostName = "NoHostName";

		// Token: 0x040000B8 RID: 184
		internal const string NoHostNameOrPortNumber = "NoHostNameOrPortNumber";

		// Token: 0x040000B9 RID: 185
		internal const string NTAuthority = "NTAuthority";

		// Token: 0x040000BA RID: 186
		internal const string Name = "Name";

		// Token: 0x040000BB RID: 187
		internal const string OneLevelPartitionNotSupported = "OneLevelPartitionNotSupported";

		// Token: 0x040000BC RID: 188
		internal const string SiteNameNotFound = "SiteNameNotFound";

		// Token: 0x040000BD RID: 189
		internal const string SiteObjectNameNotFound = "SiteObjectNameNotFound";

		// Token: 0x040000BE RID: 190
		internal const string ComputerObjectNameNotFound = "ComputerObjectNameNotFound";

		// Token: 0x040000BF RID: 191
		internal const string ServerObjectNameNotFound = "ServerObjectNameNotFound";

		// Token: 0x040000C0 RID: 192
		internal const string NtdsaObjectNameNotFound = "NtdsaObjectNameNotFound";

		// Token: 0x040000C1 RID: 193
		internal const string NtdsaObjectGuidNotFound = "NtdsaObjectGuidNotFound";

		// Token: 0x040000C2 RID: 194
		internal const string OnlyDomainOrForest = "OnlyDomainOrForest";

		// Token: 0x040000C3 RID: 195
		internal const string ServerShouldBeDC = "ServerShouldBeDC";

		// Token: 0x040000C4 RID: 196
		internal const string ServerShouldBeAI = "ServerShouldBeAI";

		// Token: 0x040000C5 RID: 197
		internal const string CannotModifySacl = "CannotModifySacl";

		// Token: 0x040000C6 RID: 198
		internal const string CannotModifyDacl = "CannotModifyDacl";

		// Token: 0x040000C7 RID: 199
		internal const string ForestTrustCollision = "ForestTrustCollision";

		// Token: 0x040000C8 RID: 200
		internal const string ForestTrustDoesNotExist = "ForestTrustDoesNotExist";

		// Token: 0x040000C9 RID: 201
		internal const string DomainTrustDoesNotExist = "DomainTrustDoesNotExist";

		// Token: 0x040000CA RID: 202
		internal const string WrongForestTrust = "WrongForestTrust";

		// Token: 0x040000CB RID: 203
		internal const string WrongTrustDirection = "WrongTrustDirection";

		// Token: 0x040000CC RID: 204
		internal const string NT4NotSupported = "NT4NotSupported";

		// Token: 0x040000CD RID: 205
		internal const string KerberosNotSupported = "KerberosNotSupported";

		// Token: 0x040000CE RID: 206
		internal const string DSPropertyListUnsupported = "DSPropertyListUnsupported";

		// Token: 0x040000CF RID: 207
		internal const string DSMultipleSDNotSupported = "DSMultipleSDNotSupported";

		// Token: 0x040000D0 RID: 208
		internal const string DSSDNoValues = "DSSDNoValues";

		// Token: 0x040000D1 RID: 209
		internal const string ConnectionSourcServerShouldBeDC = "ConnectionSourcServerShouldBeDC";

		// Token: 0x040000D2 RID: 210
		internal const string ConnectionSourcServerShouldBeADAM = "ConnectionSourcServerShouldBeADAM";

		// Token: 0x040000D3 RID: 211
		internal const string ConnectionSourcServerSameForest = "ConnectionSourcServerSameForest";

		// Token: 0x040000D4 RID: 212
		internal const string ConnectionSourcServerSameConfigSet = "ConnectionSourcServerSameConfigSet";

		// Token: 0x040000D5 RID: 213
		internal const string TrustVerificationNotSupport = "TrustVerificationNotSupport";

		// Token: 0x040000D6 RID: 214
		internal const string DSChildren = "DSChildren";

		// Token: 0x040000D7 RID: 215
		internal const string DSGuid = "DSGuid";

		// Token: 0x040000D8 RID: 216
		internal const string DSName = "DSName";

		// Token: 0x040000D9 RID: 217
		internal const string DSNativeObject = "DSNativeObject";

		// Token: 0x040000DA RID: 218
		internal const string DSParent = "DSParent";

		// Token: 0x040000DB RID: 219
		internal const string DSPassword = "DSPassword";

		// Token: 0x040000DC RID: 220
		internal const string DSPath = "DSPath";

		// Token: 0x040000DD RID: 221
		internal const string DSProperties = "DSProperties";

		// Token: 0x040000DE RID: 222
		internal const string DSSchemaClassName = "DSSchemaClassName";

		// Token: 0x040000DF RID: 223
		internal const string DSSchemaEntry = "DSSchemaEntry";

		// Token: 0x040000E0 RID: 224
		internal const string DSUsePropertyCache = "DSUsePropertyCache";

		// Token: 0x040000E1 RID: 225
		internal const string DSUsername = "DSUsername";

		// Token: 0x040000E2 RID: 226
		internal const string DSAuthenticationType = "DSAuthenticationType";

		// Token: 0x040000E3 RID: 227
		internal const string DSNativeGuid = "DSNativeGuid";

		// Token: 0x040000E4 RID: 228
		internal const string DSCacheResults = "DSCacheResults";

		// Token: 0x040000E5 RID: 229
		internal const string DSClientTimeout = "DSClientTimeout";

		// Token: 0x040000E6 RID: 230
		internal const string DSPropertyNamesOnly = "DSPropertyNamesOnly";

		// Token: 0x040000E7 RID: 231
		internal const string DSFilter = "DSFilter";

		// Token: 0x040000E8 RID: 232
		internal const string DSPageSize = "DSPageSize";

		// Token: 0x040000E9 RID: 233
		internal const string DSPropertiesToLoad = "DSPropertiesToLoad";

		// Token: 0x040000EA RID: 234
		internal const string DSReferralChasing = "DSReferralChasing";

		// Token: 0x040000EB RID: 235
		internal const string DSSearchScope = "DSSearchScope";

		// Token: 0x040000EC RID: 236
		internal const string DSServerPageTimeLimit = "DSServerPageTimeLimit";

		// Token: 0x040000ED RID: 237
		internal const string DSServerTimeLimit = "DSServerTimeLimit";

		// Token: 0x040000EE RID: 238
		internal const string DSSizeLimit = "DSSizeLimit";

		// Token: 0x040000EF RID: 239
		internal const string DSSearchRoot = "DSSearchRoot";

		// Token: 0x040000F0 RID: 240
		internal const string DSSort = "DSSort";

		// Token: 0x040000F1 RID: 241
		internal const string DSSortName = "DSSortName";

		// Token: 0x040000F2 RID: 242
		internal const string DSSortDirection = "DSSortDirection";

		// Token: 0x040000F3 RID: 243
		internal const string DSAsynchronous = "DSAsynchronous";

		// Token: 0x040000F4 RID: 244
		internal const string DSTombstone = "DSTombstone";

		// Token: 0x040000F5 RID: 245
		internal const string DSAttributeQuery = "DSAttributeQuery";

		// Token: 0x040000F6 RID: 246
		internal const string DSDerefAlias = "DSDerefAlias";

		// Token: 0x040000F7 RID: 247
		internal const string DSSecurityMasks = "DSSecurityMasks";

		// Token: 0x040000F8 RID: 248
		internal const string DSExtendedDn = "DSExtendedDn";

		// Token: 0x040000F9 RID: 249
		internal const string DSDirectorySynchronizationFlag = "DSDirectorySynchronizationFlag";

		// Token: 0x040000FA RID: 250
		internal const string DSDirectorySynchronizationCookie = "DSDirectorySynchronizationCookie";

		// Token: 0x040000FB RID: 251
		internal const string DSDirectorySynchronization = "DSDirectorySynchronization";

		// Token: 0x040000FC RID: 252
		internal const string DSUnknown = "DSUnknown";

		// Token: 0x040000FD RID: 253
		internal const string DSOptions = "DSOptions";

		// Token: 0x040000FE RID: 254
		internal const string DSObjectSecurity = "DSObjectSecurity";

		// Token: 0x040000FF RID: 255
		internal const string DirectoryEntryDesc = "DirectoryEntryDesc";

		// Token: 0x04000100 RID: 256
		internal const string DirectorySearcherDesc = "DirectorySearcherDesc";

		// Token: 0x04000101 RID: 257
		internal const string OnlyAllowSingleDimension = "OnlyAllowSingleDimension";

		// Token: 0x04000102 RID: 258
		internal const string LessThanZero = "LessThanZero";

		// Token: 0x04000103 RID: 259
		internal const string DestinationArrayNotLargeEnough = "DestinationArrayNotLargeEnough";

		// Token: 0x04000104 RID: 260
		internal const string NoNegativeTime = "NoNegativeTime";

		// Token: 0x04000105 RID: 261
		internal const string ReplicationIntervalExceedMax = "ReplicationIntervalExceedMax";

		// Token: 0x04000106 RID: 262
		internal const string ReplicationIntervalInMinutes = "ReplicationIntervalInMinutes";

		// Token: 0x04000107 RID: 263
		internal const string TimespanExceedMax = "TimespanExceedMax";

		// Token: 0x04000108 RID: 264
		private static Res loader;

		// Token: 0x04000109 RID: 265
		private ResourceManager resources;

		// Token: 0x0400010A RID: 266
		private static object s_InternalSyncObject;
	}
}
