using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Data.OracleClient
{
	// Token: 0x02000004 RID: 4
	internal sealed class Res
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000541FC File Offset: 0x000535FC
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

		// Token: 0x06000006 RID: 6 RVA: 0x00054228 File Offset: 0x00053628
		internal Res()
		{
			this.resources = new ResourceManager("System.Data.OracleClient", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00054258 File Offset: 0x00053658
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
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000542B4 File Offset: 0x000536B4
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000542C4 File Offset: 0x000536C4
		public static ResourceManager Resources
		{
			get
			{
				return Res.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000542DC File Offset: 0x000536DC
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

		// Token: 0x0600000B RID: 11 RVA: 0x00054360 File Offset: 0x00053760
		public static string GetString(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetString(name, Res.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x0005438C File Offset: 0x0005378C
		public static object GetObject(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetObject(name, Res.Culture);
		}

		// Token: 0x04000002 RID: 2
		internal const string ADP_CollectionIndexInt32 = "ADP_CollectionIndexInt32";

		// Token: 0x04000003 RID: 3
		internal const string ADP_CollectionIndexString = "ADP_CollectionIndexString";

		// Token: 0x04000004 RID: 4
		internal const string ADP_CollectionInvalidType = "ADP_CollectionInvalidType";

		// Token: 0x04000005 RID: 5
		internal const string ADP_CollectionIsNotParent = "ADP_CollectionIsNotParent";

		// Token: 0x04000006 RID: 6
		internal const string ADP_CollectionIsParent = "ADP_CollectionIsParent";

		// Token: 0x04000007 RID: 7
		internal const string ADP_CollectionNullValue = "ADP_CollectionNullValue";

		// Token: 0x04000008 RID: 8
		internal const string ADP_CollectionRemoveInvalidObject = "ADP_CollectionRemoveInvalidObject";

		// Token: 0x04000009 RID: 9
		internal const string ADP_ConnectionAlreadyOpen = "ADP_ConnectionAlreadyOpen";

		// Token: 0x0400000A RID: 10
		internal const string ADP_ConnectionStateMsg_Closed = "ADP_ConnectionStateMsg_Closed";

		// Token: 0x0400000B RID: 11
		internal const string ADP_ConnectionStateMsg_Connecting = "ADP_ConnectionStateMsg_Connecting";

		// Token: 0x0400000C RID: 12
		internal const string ADP_ConnectionStateMsg_Open = "ADP_ConnectionStateMsg_Open";

		// Token: 0x0400000D RID: 13
		internal const string ADP_ConnectionStateMsg_OpenExecuting = "ADP_ConnectionStateMsg_OpenExecuting";

		// Token: 0x0400000E RID: 14
		internal const string ADP_ConnectionStateMsg_OpenFetching = "ADP_ConnectionStateMsg_OpenFetching";

		// Token: 0x0400000F RID: 15
		internal const string ADP_ConnectionStateMsg = "ADP_ConnectionStateMsg";

		// Token: 0x04000010 RID: 16
		internal const string ADP_ConnectionStringSyntax = "ADP_ConnectionStringSyntax";

		// Token: 0x04000011 RID: 17
		internal const string ADP_DataReaderClosed = "ADP_DataReaderClosed";

		// Token: 0x04000012 RID: 18
		internal const string ADP_EmptyString = "ADP_EmptyString";

		// Token: 0x04000013 RID: 19
		internal const string ADP_InternalConnectionError = "ADP_InternalConnectionError";

		// Token: 0x04000014 RID: 20
		internal const string ADP_InvalidDataDirectory = "ADP_InvalidDataDirectory";

		// Token: 0x04000015 RID: 21
		internal const string ADP_InvalidEnumerationValue = "ADP_InvalidEnumerationValue";

		// Token: 0x04000016 RID: 22
		internal const string ADP_InvalidKey = "ADP_InvalidKey";

		// Token: 0x04000017 RID: 23
		internal const string ADP_InvalidOffsetValue = "ADP_InvalidOffsetValue";

		// Token: 0x04000018 RID: 24
		internal const string ADP_InvalidValue = "ADP_InvalidValue";

		// Token: 0x04000019 RID: 25
		internal const string ADP_InvalidXMLBadVersion = "ADP_InvalidXMLBadVersion";

		// Token: 0x0400001A RID: 26
		internal const string ADP_NoConnectionString = "ADP_NoConnectionString";

		// Token: 0x0400001B RID: 27
		internal const string ADP_NotAPermissionElement = "ADP_NotAPermissionElement";

		// Token: 0x0400001C RID: 28
		internal const string ADP_OpenConnectionPropertySet = "ADP_OpenConnectionPropertySet";

		// Token: 0x0400001D RID: 29
		internal const string ADP_PermissionTypeMismatch = "ADP_PermissionTypeMismatch";

		// Token: 0x0400001E RID: 30
		internal const string ADP_PooledOpenTimeout = "ADP_PooledOpenTimeout";

		// Token: 0x0400001F RID: 31
		internal const string DataCategory_Data = "DataCategory_Data";

		// Token: 0x04000020 RID: 32
		internal const string DataCategory_StateChange = "DataCategory_StateChange";

		// Token: 0x04000021 RID: 33
		internal const string DataCategory_Update = "DataCategory_Update";

		// Token: 0x04000022 RID: 34
		internal const string DbCommand_CommandTimeout = "DbCommand_CommandTimeout";

		// Token: 0x04000023 RID: 35
		internal const string DbConnection_State = "DbConnection_State";

		// Token: 0x04000024 RID: 36
		internal const string DbConnection_StateChange = "DbConnection_StateChange";

		// Token: 0x04000025 RID: 37
		internal const string DbParameter_DbType = "DbParameter_DbType";

		// Token: 0x04000026 RID: 38
		internal const string DbParameter_Direction = "DbParameter_Direction";

		// Token: 0x04000027 RID: 39
		internal const string DbParameter_IsNullable = "DbParameter_IsNullable";

		// Token: 0x04000028 RID: 40
		internal const string DbParameter_Offset = "DbParameter_Offset";

		// Token: 0x04000029 RID: 41
		internal const string DbParameter_ParameterName = "DbParameter_ParameterName";

		// Token: 0x0400002A RID: 42
		internal const string DbParameter_Size = "DbParameter_Size";

		// Token: 0x0400002B RID: 43
		internal const string DbParameter_SourceColumn = "DbParameter_SourceColumn";

		// Token: 0x0400002C RID: 44
		internal const string DbParameter_SourceVersion = "DbParameter_SourceVersion";

		// Token: 0x0400002D RID: 45
		internal const string DbParameter_SourceColumnNullMapping = "DbParameter_SourceColumnNullMapping";

		// Token: 0x0400002E RID: 46
		internal const string DbParameter_Value = "DbParameter_Value";

		// Token: 0x0400002F RID: 47
		internal const string MDF_QueryFailed = "MDF_QueryFailed";

		// Token: 0x04000030 RID: 48
		internal const string MDF_TooManyRestrictions = "MDF_TooManyRestrictions";

		// Token: 0x04000031 RID: 49
		internal const string MDF_InvalidRestrictionValue = "MDF_InvalidRestrictionValue";

		// Token: 0x04000032 RID: 50
		internal const string MDF_UndefinedCollection = "MDF_UndefinedCollection";

		// Token: 0x04000033 RID: 51
		internal const string MDF_UndefinedPopulationMechanism = "MDF_UndefinedPopulationMechanism";

		// Token: 0x04000034 RID: 52
		internal const string MDF_UnsupportedVersion = "MDF_UnsupportedVersion";

		// Token: 0x04000035 RID: 53
		internal const string MDF_MissingDataSourceInformationColumn = "MDF_MissingDataSourceInformationColumn";

		// Token: 0x04000036 RID: 54
		internal const string MDF_IncorrectNumberOfDataSourceInformationRows = "MDF_IncorrectNumberOfDataSourceInformationRows";

		// Token: 0x04000037 RID: 55
		internal const string MDF_MissingRestrictionColumn = "MDF_MissingRestrictionColumn";

		// Token: 0x04000038 RID: 56
		internal const string MDF_MissingRestrictionRow = "MDF_MissingRestrictionRow";

		// Token: 0x04000039 RID: 57
		internal const string MDF_NoColumns = "MDF_NoColumns";

		// Token: 0x0400003A RID: 58
		internal const string MDF_UnableToBuildCollection = "MDF_UnableToBuildCollection";

		// Token: 0x0400003B RID: 59
		internal const string MDF_AmbigousCollectionName = "MDF_AmbigousCollectionName";

		// Token: 0x0400003C RID: 60
		internal const string MDF_CollectionNameISNotUnique = "MDF_CollectionNameISNotUnique";

		// Token: 0x0400003D RID: 61
		internal const string MDF_DataTableDoesNotExist = "MDF_DataTableDoesNotExist";

		// Token: 0x0400003E RID: 62
		internal const string MDF_InvalidXml = "MDF_InvalidXml";

		// Token: 0x0400003F RID: 63
		internal const string MDF_InvalidXmlMissingColumn = "MDF_InvalidXmlMissingColumn";

		// Token: 0x04000040 RID: 64
		internal const string MDF_InvalidXmlInvalidValue = "MDF_InvalidXmlInvalidValue";

		// Token: 0x04000041 RID: 65
		internal const string ADP_InternalError = "ADP_InternalError";

		// Token: 0x04000042 RID: 66
		internal const string ADP_NoMessageAvailable = "ADP_NoMessageAvailable";

		// Token: 0x04000043 RID: 67
		internal const string ADP_BadBindValueType = "ADP_BadBindValueType";

		// Token: 0x04000044 RID: 68
		internal const string ADP_BadOracleClientImageFormat = "ADP_BadOracleClientImageFormat";

		// Token: 0x04000045 RID: 69
		internal const string ADP_BadOracleClientVersion = "ADP_BadOracleClientVersion";

		// Token: 0x04000046 RID: 70
		internal const string ADP_BufferExceeded = "ADP_BufferExceeded";

		// Token: 0x04000047 RID: 71
		internal const string ADP_CannotDeriveOverloaded = "ADP_CannotDeriveOverloaded";

		// Token: 0x04000048 RID: 72
		internal const string ADP_CannotOpenLobWithDifferentMode = "ADP_CannotOpenLobWithDifferentMode";

		// Token: 0x04000049 RID: 73
		internal const string ADP_ChangeDatabaseNotSupported = "ADP_ChangeDatabaseNotSupported";

		// Token: 0x0400004A RID: 74
		internal const string ADP_ClosedConnectionError = "ADP_ClosedConnectionError";

		// Token: 0x0400004B RID: 75
		internal const string ADP_ClosedDataReaderError = "ADP_ClosedDataReaderError";

		// Token: 0x0400004C RID: 76
		internal const string ADP_CommandTextRequired = "ADP_CommandTextRequired";

		// Token: 0x0400004D RID: 77
		internal const string ADP_ConfigWrongNumberOfValues = "ADP_ConfigWrongNumberOfValues";

		// Token: 0x0400004E RID: 78
		internal const string ADP_ConfigUnableToLoadXmlMetaDataFile = "ADP_ConfigUnableToLoadXmlMetaDataFile";

		// Token: 0x0400004F RID: 79
		internal const string ADP_ConnectionRequired = "ADP_ConnectionRequired";

		// Token: 0x04000050 RID: 80
		internal const string ADP_CouldNotCreateEnvironment = "ADP_CouldNotCreateEnvironment";

		// Token: 0x04000051 RID: 81
		internal const string ADP_ConvertFailed = "ADP_ConvertFailed";

		// Token: 0x04000052 RID: 82
		internal const string ADP_DataIsNull = "ADP_DataIsNull";

		// Token: 0x04000053 RID: 83
		internal const string ADP_DataReaderNoData = "ADP_DataReaderNoData";

		// Token: 0x04000054 RID: 84
		internal const string ADP_DeriveParametersNotSupported = "ADP_DeriveParametersNotSupported";

		// Token: 0x04000055 RID: 85
		internal const string ADP_DistribTxRequiresOracle9i = "ADP_DistribTxRequiresOracle9i";

		// Token: 0x04000056 RID: 86
		internal const string ADP_DistribTxRequiresOracleServicesForMTS = "ADP_DistribTxRequiresOracleServicesForMTS";

		// Token: 0x04000057 RID: 87
		internal const string ADP_IdentifierIsNotQuoted = "ADP_IdentifierIsNotQuoted";

		// Token: 0x04000058 RID: 88
		internal const string ADP_InputRefCursorNotSupported = "ADP_InputRefCursorNotSupported";

		// Token: 0x04000059 RID: 89
		internal const string ADP_InternalProviderError = "ADP_InternalProviderError";

		// Token: 0x0400005A RID: 90
		internal const string ADP_InvalidCommandType = "ADP_InvalidCommandType";

		// Token: 0x0400005B RID: 91
		internal const string ADP_InvalidConnectionOptionLength = "ADP_InvalidConnectionOptionLength";

		// Token: 0x0400005C RID: 92
		internal const string ADP_InvalidConnectionOptionValue = "ADP_InvalidConnectionOptionValue";

		// Token: 0x0400005D RID: 93
		internal const string ADP_InvalidDataLength = "ADP_InvalidDataLength";

		// Token: 0x0400005E RID: 94
		internal const string ADP_InvalidDataType = "ADP_InvalidDataType";

		// Token: 0x0400005F RID: 95
		internal const string ADP_InvalidDataTypeForValue = "ADP_InvalidDataTypeForValue";

		// Token: 0x04000060 RID: 96
		internal const string ADP_InvalidDbType = "ADP_InvalidDbType";

		// Token: 0x04000061 RID: 97
		internal const string ADP_InvalidDestinationBufferIndex = "ADP_InvalidDestinationBufferIndex";

		// Token: 0x04000062 RID: 98
		internal const string ADP_InvalidLobType = "ADP_InvalidLobType";

		// Token: 0x04000063 RID: 99
		internal const string ADP_InvalidMinMaxPoolSizeValues = "ADP_InvalidMinMaxPoolSizeValues";

		// Token: 0x04000064 RID: 100
		internal const string ADP_InvalidOracleType = "ADP_InvalidOracleType";

		// Token: 0x04000065 RID: 101
		internal const string ADP_InvalidSeekOrigin = "ADP_InvalidSeekOrigin";

		// Token: 0x04000066 RID: 102
		internal const string ADP_InvalidSizeValue = "ADP_InvalidSizeValue";

		// Token: 0x04000067 RID: 103
		internal const string ADP_InvalidSourceBufferIndex = "ADP_InvalidSourceBufferIndex";

		// Token: 0x04000068 RID: 104
		internal const string ADP_InvalidSourceOffset = "ADP_InvalidSourceOffset";

		// Token: 0x04000069 RID: 105
		internal const string ADP_KeywordNotSupported = "ADP_KeywordNotSupported";

		// Token: 0x0400006A RID: 106
		internal const string ADP_LobAmountExceeded = "ADP_LobAmountExceeded";

		// Token: 0x0400006B RID: 107
		internal const string ADP_LobAmountMustBeEven = "ADP_LobAmountMustBeEven";

		// Token: 0x0400006C RID: 108
		internal const string ADP_LobPositionMustBeEven = "ADP_LobPositionMustBeEven";

		// Token: 0x0400006D RID: 109
		internal const string ADP_LobWriteInvalidOnNull = "ADP_LobWriteInvalidOnNull";

		// Token: 0x0400006E RID: 110
		internal const string ADP_LobWriteRequiresTransaction = "ADP_LobWriteRequiresTransaction";

		// Token: 0x0400006F RID: 111
		internal const string ADP_MonthOutOfRange = "ADP_MonthOutOfRange";

		// Token: 0x04000070 RID: 112
		internal const string ADP_MustBePositive = "ADP_MustBePositive";

		// Token: 0x04000071 RID: 113
		internal const string ADP_NoCommandText = "ADP_NoCommandText";

		// Token: 0x04000072 RID: 114
		internal const string ADP_NoData = "ADP_NoData";

		// Token: 0x04000073 RID: 115
		internal const string ADP_NoLocalTransactionInDistributedContext = "ADP_NoLocalTransactionInDistributedContext";

		// Token: 0x04000074 RID: 116
		internal const string ADP_NoOptimizedDirectTableAccess = "ADP_NoOptimizedDirectTableAccess";

		// Token: 0x04000075 RID: 117
		internal const string ADP_NoParallelTransactions = "ADP_NoParallelTransactions";

		// Token: 0x04000076 RID: 118
		internal const string ADP_OpenConnectionRequired = "ADP_OpenConnectionRequired";

		// Token: 0x04000077 RID: 119
		internal const string ADP_OperationFailed = "ADP_OperationFailed";

		// Token: 0x04000078 RID: 120
		internal const string ADP_OperationResultedInOverflow = "ADP_OperationResultedInOverflow";

		// Token: 0x04000079 RID: 121
		internal const string ADP_ParameterConversionFailed = "ADP_ParameterConversionFailed";

		// Token: 0x0400007A RID: 122
		internal const string ADP_ParameterSizeIsMissing = "ADP_ParameterSizeIsMissing";

		// Token: 0x0400007B RID: 123
		internal const string ADP_ParameterSizeIsTooLarge = "ADP_ParameterSizeIsTooLarge";

		// Token: 0x0400007C RID: 124
		internal const string ADP_PleaseUninstallTheBeta = "ADP_PleaseUninstallTheBeta";

		// Token: 0x0400007D RID: 125
		internal const string ADP_ReadOnlyLob = "ADP_ReadOnlyLob";

		// Token: 0x0400007E RID: 126
		internal const string ADP_SeekBeyondEnd = "ADP_SeekBeyondEnd";

		// Token: 0x0400007F RID: 127
		internal const string ADP_SQLParserInternalError = "ADP_SQLParserInternalError";

		// Token: 0x04000080 RID: 128
		internal const string ADP_SyntaxErrorExpectedCommaAfterColumn = "ADP_SyntaxErrorExpectedCommaAfterColumn";

		// Token: 0x04000081 RID: 129
		internal const string ADP_SyntaxErrorExpectedCommaAfterTable = "ADP_SyntaxErrorExpectedCommaAfterTable";

		// Token: 0x04000082 RID: 130
		internal const string ADP_SyntaxErrorExpectedIdentifier = "ADP_SyntaxErrorExpectedIdentifier";

		// Token: 0x04000083 RID: 131
		internal const string ADP_SyntaxErrorExpectedNextPart = "ADP_SyntaxErrorExpectedNextPart";

		// Token: 0x04000084 RID: 132
		internal const string ADP_SyntaxErrorMissingParenthesis = "ADP_SyntaxErrorMissingParenthesis";

		// Token: 0x04000085 RID: 133
		internal const string ADP_SyntaxErrorTooManyNameParts = "ADP_SyntaxErrorTooManyNameParts";

		// Token: 0x04000086 RID: 134
		internal const string ADP_TransactionCompleted = "ADP_TransactionCompleted";

		// Token: 0x04000087 RID: 135
		internal const string ADP_TransactionConnectionMismatch = "ADP_TransactionConnectionMismatch";

		// Token: 0x04000088 RID: 136
		internal const string ADP_TransactionPresent = "ADP_TransactionPresent";

		// Token: 0x04000089 RID: 137
		internal const string ADP_TransactionRequired_Execute = "ADP_TransactionRequired_Execute";

		// Token: 0x0400008A RID: 138
		internal const string ADP_TypeNotSupported = "ADP_TypeNotSupported";

		// Token: 0x0400008B RID: 139
		internal const string ADP_UnexpectedReturnCode = "ADP_UnexpectedReturnCode";

		// Token: 0x0400008C RID: 140
		internal const string ADP_UnknownDataTypeCode = "ADP_UnknownDataTypeCode";

		// Token: 0x0400008D RID: 141
		internal const string ADP_UnsupportedIsolationLevel = "ADP_UnsupportedIsolationLevel";

		// Token: 0x0400008E RID: 142
		internal const string ADP_WriteByteForBinaryLobsOnly = "ADP_WriteByteForBinaryLobsOnly";

		// Token: 0x0400008F RID: 143
		internal const string ADP_WrongType = "ADP_WrongType";

		// Token: 0x04000090 RID: 144
		internal const string DataCategory_Advanced = "DataCategory_Advanced";

		// Token: 0x04000091 RID: 145
		internal const string DataCategory_Initialization = "DataCategory_Initialization";

		// Token: 0x04000092 RID: 146
		internal const string DataCategory_Pooling = "DataCategory_Pooling";

		// Token: 0x04000093 RID: 147
		internal const string DataCategory_Security = "DataCategory_Security";

		// Token: 0x04000094 RID: 148
		internal const string DataCategory_Source = "DataCategory_Source";

		// Token: 0x04000095 RID: 149
		internal const string OracleCategory_Behavior = "OracleCategory_Behavior";

		// Token: 0x04000096 RID: 150
		internal const string OracleCategory_Data = "OracleCategory_Data";

		// Token: 0x04000097 RID: 151
		internal const string OracleCategory_Fill = "OracleCategory_Fill";

		// Token: 0x04000098 RID: 152
		internal const string OracleCategory_InfoMessage = "OracleCategory_InfoMessage";

		// Token: 0x04000099 RID: 153
		internal const string OracleCategory_StateChange = "OracleCategory_StateChange";

		// Token: 0x0400009A RID: 154
		internal const string OracleCategory_Update = "OracleCategory_Update";

		// Token: 0x0400009B RID: 155
		internal const string DbCommand_CommandText = "DbCommand_CommandText";

		// Token: 0x0400009C RID: 156
		internal const string DbCommand_CommandType = "DbCommand_CommandType";

		// Token: 0x0400009D RID: 157
		internal const string DbCommand_Connection = "DbCommand_Connection";

		// Token: 0x0400009E RID: 158
		internal const string DbCommand_Transaction = "DbCommand_Transaction";

		// Token: 0x0400009F RID: 159
		internal const string DbCommand_UpdatedRowSource = "DbCommand_UpdatedRowSource";

		// Token: 0x040000A0 RID: 160
		internal const string DbCommand_Parameters = "DbCommand_Parameters";

		// Token: 0x040000A1 RID: 161
		internal const string OracleCommandBuilder_DataAdapter = "OracleCommandBuilder_DataAdapter";

		// Token: 0x040000A2 RID: 162
		internal const string OracleCommandBuilder_QuotePrefix = "OracleCommandBuilder_QuotePrefix";

		// Token: 0x040000A3 RID: 163
		internal const string OracleCommandBuilder_QuoteSuffix = "OracleCommandBuilder_QuoteSuffix";

		// Token: 0x040000A4 RID: 164
		internal const string OracleConnection_ConnectionString = "OracleConnection_ConnectionString";

		// Token: 0x040000A5 RID: 165
		internal const string OracleConnection_DataSource = "OracleConnection_DataSource";

		// Token: 0x040000A6 RID: 166
		internal const string OracleConnection_InfoMessage = "OracleConnection_InfoMessage";

		// Token: 0x040000A7 RID: 167
		internal const string OracleConnection_StateChange = "OracleConnection_StateChange";

		// Token: 0x040000A8 RID: 168
		internal const string OracleConnection_State = "OracleConnection_State";

		// Token: 0x040000A9 RID: 169
		internal const string OracleConnection_ServerVersion = "OracleConnection_ServerVersion";

		// Token: 0x040000AA RID: 170
		internal const string DbConnectionString_ConnectionString = "DbConnectionString_ConnectionString";

		// Token: 0x040000AB RID: 171
		internal const string DbConnectionString_DataSource = "DbConnectionString_DataSource";

		// Token: 0x040000AC RID: 172
		internal const string DbConnectionString_Enlist = "DbConnectionString_Enlist";

		// Token: 0x040000AD RID: 173
		internal const string DbConnectionString_IntegratedSecurity = "DbConnectionString_IntegratedSecurity";

		// Token: 0x040000AE RID: 174
		internal const string DbConnectionString_LoadBalanceTimeout = "DbConnectionString_LoadBalanceTimeout";

		// Token: 0x040000AF RID: 175
		internal const string DbConnectionString_MaxPoolSize = "DbConnectionString_MaxPoolSize";

		// Token: 0x040000B0 RID: 176
		internal const string DbConnectionString_MinPoolSize = "DbConnectionString_MinPoolSize";

		// Token: 0x040000B1 RID: 177
		internal const string DbConnectionString_OmitOracleConnectionName = "DbConnectionString_OmitOracleConnectionName";

		// Token: 0x040000B2 RID: 178
		internal const string DbConnectionString_Password = "DbConnectionString_Password";

		// Token: 0x040000B3 RID: 179
		internal const string DbConnectionString_PersistSecurityInfo = "DbConnectionString_PersistSecurityInfo";

		// Token: 0x040000B4 RID: 180
		internal const string DbConnectionString_Pooling = "DbConnectionString_Pooling";

		// Token: 0x040000B5 RID: 181
		internal const string DbConnectionString_Unicode = "DbConnectionString_Unicode";

		// Token: 0x040000B6 RID: 182
		internal const string DbConnectionString_UserID = "DbConnectionString_UserID";

		// Token: 0x040000B7 RID: 183
		internal const string DbDataAdapter_DeleteCommand = "DbDataAdapter_DeleteCommand";

		// Token: 0x040000B8 RID: 184
		internal const string DbDataAdapter_InsertCommand = "DbDataAdapter_InsertCommand";

		// Token: 0x040000B9 RID: 185
		internal const string DbDataAdapter_RowUpdated = "DbDataAdapter_RowUpdated";

		// Token: 0x040000BA RID: 186
		internal const string DbDataAdapter_RowUpdating = "DbDataAdapter_RowUpdating";

		// Token: 0x040000BB RID: 187
		internal const string DbDataAdapter_SelectCommand = "DbDataAdapter_SelectCommand";

		// Token: 0x040000BC RID: 188
		internal const string DbDataAdapter_UpdateCommand = "DbDataAdapter_UpdateCommand";

		// Token: 0x040000BD RID: 189
		internal const string DbTable_Connection = "DbTable_Connection";

		// Token: 0x040000BE RID: 190
		internal const string DbTable_DeleteCommand = "DbTable_DeleteCommand";

		// Token: 0x040000BF RID: 191
		internal const string DbTable_InsertCommand = "DbTable_InsertCommand";

		// Token: 0x040000C0 RID: 192
		internal const string DbTable_SelectCommand = "DbTable_SelectCommand";

		// Token: 0x040000C1 RID: 193
		internal const string DbTable_UpdateCommand = "DbTable_UpdateCommand";

		// Token: 0x040000C2 RID: 194
		internal const string OracleParameter_OracleType = "OracleParameter_OracleType";

		// Token: 0x040000C3 RID: 195
		internal const string OracleMetaDataFactory_XML = "OracleMetaDataFactory_XML";

		// Token: 0x040000C4 RID: 196
		internal const string SqlMisc_NullString = "SqlMisc_NullString";

		// Token: 0x040000C5 RID: 197
		private static Res loader;

		// Token: 0x040000C6 RID: 198
		private ResourceManager resources;

		// Token: 0x040000C7 RID: 199
		private static object s_InternalSyncObject;
	}
}
