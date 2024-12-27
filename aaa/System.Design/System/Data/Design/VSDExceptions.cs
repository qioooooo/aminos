using System;

namespace System.Data.Design
{
	// Token: 0x020000D8 RID: 216
	internal class VSDExceptions
	{
		// Token: 0x06000903 RID: 2307 RVA: 0x0001E8ED File Offset: 0x0001D8ED
		private VSDExceptions()
		{
		}

		// Token: 0x020000D9 RID: 217
		internal class COMMON
		{
			// Token: 0x06000904 RID: 2308 RVA: 0x0001E8F5 File Offset: 0x0001D8F5
			private COMMON()
			{
			}

			// Token: 0x04000CB9 RID: 3257
			internal const int START_CODE = 0;

			// Token: 0x04000CBA RID: 3258
			internal const string SIMPLENAMESERVICE_NAMEOVERFLOW_MSG = "Failed to create unique name after many attempts";

			// Token: 0x04000CBB RID: 3259
			internal const int SIMPLENAMESERVICE_NAMEOVERFLOW_CODE = 1;

			// Token: 0x04000CBC RID: 3260
			internal const string NOT_A_NAMED_OBJECT_MSG = "Named object collection holds something that is not a named object";

			// Token: 0x04000CBD RID: 3261
			internal const int NOT_A_NAMED_OBJECT_CODE = 2;
		}

		// Token: 0x020000DA RID: 218
		internal class DataSource
		{
			// Token: 0x06000905 RID: 2309 RVA: 0x0001E8FD File Offset: 0x0001D8FD
			private DataSource()
			{
			}

			// Token: 0x04000CBE RID: 3262
			internal const int START_CODE = 20000;

			// Token: 0x04000CBF RID: 3263
			internal const string NO_CONNECTION_NAME_SERVICE_MSG = "Failed to obtain name service for connection collection";

			// Token: 0x04000CC0 RID: 3264
			internal const int NO_CONNECTION_NAME_SERVICE_CODE = 20001;

			// Token: 0x04000CC1 RID: 3265
			internal const string TABLE_BELONGS_TO_OTHER_DATA_SOURCE_MSG = "This table belongs to another DataSource already";

			// Token: 0x04000CC2 RID: 3266
			internal const int TABLE_BELONGS_TO_OTHER_DATA_SOURCE_CODE = 20002;

			// Token: 0x04000CC3 RID: 3267
			internal const string PARAMETER_NOT_FOUND_MSG = "No parameter named '{0}' found";

			// Token: 0x04000CC4 RID: 3268
			internal const int PARAMETER_NOT_FOUND_CODE = 20004;

			// Token: 0x04000CC5 RID: 3269
			internal const string INVALID_PARAMETER_VALUE_MSG = "Invalid parameter value (the object passed in must support IDbDataParameter interface)";

			// Token: 0x04000CC6 RID: 3270
			internal const int INVALID_PARAMETER_VALUE_CODE = 20005;

			// Token: 0x04000CC7 RID: 3271
			internal const string INVALID_SOURCE_VALUE_MSG = "SourceCollection can only hold objects of type Source";

			// Token: 0x04000CC8 RID: 3272
			internal const int INVALID_SOURCE_VALUE_CODE = 20006;

			// Token: 0x04000CC9 RID: 3273
			internal const string OP_VALID_FOR_RAD_TABLE_ONLY_MSG = "Operation invalid. Table gets data from something else than a database.";

			// Token: 0x04000CCA RID: 3274
			internal const int OP_VALID_FOR_RAD_TABLE_ONLY_CODE = 20007;

			// Token: 0x04000CCB RID: 3275
			internal const string INVALID_COLUMN_VALUE_MSG = "DesignColumnCollection can only hold objects of type DesignColumn";

			// Token: 0x04000CCC RID: 3276
			internal const int INVALID_COLUMN_VALUE_CODE = 20008;

			// Token: 0x04000CCD RID: 3277
			internal const string DESIGN_COLUMN_NEEDS_DATA_COLUMN_MSG = "DesignColumn object needs a valid DataColumn";

			// Token: 0x04000CCE RID: 3278
			internal const int DESIGN_COLUMN_NEEDS_DATA_COLUMN_CODE = 20009;

			// Token: 0x04000CCF RID: 3279
			internal const string RELATION_BELONGS_TO_OTHER_DATA_SOURCE_MSG = "This relation belongs to another DataSource already";

			// Token: 0x04000CD0 RID: 3280
			internal const int RELATION_BELONGS_TO_OTHER_DATA_SOURCE_CODE = 20010;

			// Token: 0x04000CD1 RID: 3281
			internal const string INVALID_COLUMN_INDEX_MSG = "Index out of range in getting DesignColumn";

			// Token: 0x04000CD2 RID: 3282
			internal const int INVALID_COLUMN_INDEX_CODE = 20011;

			// Token: 0x04000CD3 RID: 3283
			internal const string COMMAND_NOT_SET_MSG = "Command not set. Operation cannot be performed";

			// Token: 0x04000CD4 RID: 3284
			internal const int COMMAND_NOT_SET_CODE = 20012;

			// Token: 0x04000CD5 RID: 3285
			internal const string CONNECTION_NOT_SET_MSG = "Connection not set. Operation cannot be performed";

			// Token: 0x04000CD6 RID: 3286
			internal const int CONNECTION_NOT_SET_CODE = 20013;

			// Token: 0x04000CD7 RID: 3287
			internal const string BAD_QUERY_FOR_GENERATING_IUD_MSG = "Cannot generate updating statements from query that is not a SELECT query";

			// Token: 0x04000CD8 RID: 3288
			internal const int BAD_QUERY_FOR_GENERATING_IUD_CODE = 20014;

			// Token: 0x04000CD9 RID: 3289
			internal const string INVALID_DATA_SOURCE_NAME_MSG = "Data source name is empty or invalid";

			// Token: 0x04000CDA RID: 3290
			internal const int INVALID_DATA_SOURCE_NAME_CODE = 20015;

			// Token: 0x04000CDB RID: 3291
			internal const string INVALID_COLLECTIONTYPE_MSG = "{0} can hold only {1} objects";

			// Token: 0x04000CDC RID: 3292
			internal const int INVALID_COLLECTIONTYPE_CODE = 20016;

			// Token: 0x04000CDD RID: 3293
			internal const string CANNOT_GET_DATA_CONFIGURATION_CONTEXT_MSG = "Data configuration context could not be obtained";

			// Token: 0x04000CDE RID: 3294
			internal const int CANNOT_GET_DATA_CONFIGURATION_CONTEXT_CODE = 20017;

			// Token: 0x04000CDF RID: 3295
			internal const string CANNOT_GET_IDBPROVIDER_MSG = "Data provider information could not be obtained";

			// Token: 0x04000CE0 RID: 3296
			internal const int CANNOT_GET_IDBPROVIDER_CODE = 20018;

			// Token: 0x04000CE1 RID: 3297
			internal const string DBSOURCECMD_HAS_INVALID_PARENT_MSG = "Parent of the DbSourceCommand is invalid";

			// Token: 0x04000CE2 RID: 3298
			internal const int DBSOURCECMD_HAS_INVALID_PARENT_CODE = 20019;

			// Token: 0x04000CE3 RID: 3299
			internal const string DATASOURCECOLLECTIONUNDOUNIT_INVALIDPARA_MSG = "Invalid parameters in DataSourceCollectionUndounit";

			// Token: 0x04000CE4 RID: 3300
			internal const int DATASOURCECOLLECTIONUNDOUNIT_INVALIDPARA_CODE = 20020;

			// Token: 0x04000CE5 RID: 3301
			internal const string CANNOT_GET_IDBPROVIDER_FOR_CONNECTION_MSG = "Could not get data provider information for connection of type {0}";

			// Token: 0x04000CE6 RID: 3302
			internal const int CANNOT_GET_IDBPROVIDER_FOR_CONNECTION_CODE = 20021;

			// Token: 0x04000CE7 RID: 3303
			internal const string CANNOT_GET_IDBPROVIDER_FOR_CSTRING_MSG = "Could not get data provider information for the following connection string:\r\n{0}\r\n";

			// Token: 0x04000CE8 RID: 3304
			internal const int CANNOT_GET_IDBPROVIDER_FOR_CSTRING_CODE = 20022;

			// Token: 0x04000CE9 RID: 3305
			internal const string INVALID_DATA_SOURCE_MSG = "Data source is invalid (null)";

			// Token: 0x04000CEA RID: 3306
			internal const int INVALID_DATA_SOURCE_CODE = 20023;

			// Token: 0x04000CEB RID: 3307
			internal const string SELECT_COMMAND_TEXT_EMPTY_MSG = "Select command text is null or empty";

			// Token: 0x04000CEC RID: 3308
			internal const int SELECT_COMMAND_TEXT_EMPTY_CODE = 20024;

			// Token: 0x04000CED RID: 3309
			internal const string PK_COLUMNS_MISSING_MSG = "Some primary key columns are missing";

			// Token: 0x04000CEE RID: 3310
			internal const int PK_COLUMNS_MISSING_CODE = 20025;

			// Token: 0x04000CEF RID: 3311
			internal const string CONF_CONTEXT_NULL_MSG = "Data configuration context is null";

			// Token: 0x04000CF0 RID: 3312
			internal const int CONF_CONTEXT_NULL_CODE = 20026;

			// Token: 0x04000CF1 RID: 3313
			internal const string COMMAND_OPERATION_INVALID_MSG = "Specified command operation is invalid (can only be INSERT, UPDATE or DELETE)";

			// Token: 0x04000CF2 RID: 3314
			internal const int COMMAND_OPERATION_INVALID_CODE = 20027;

			// Token: 0x04000CF3 RID: 3315
			internal const string SPROC_NAME_EMPTY_MSG = "The stored procedure name is null or empty";

			// Token: 0x04000CF4 RID: 3316
			internal const int SPROC_NAME_EMPTY_CODE = 20028;

			// Token: 0x04000CF5 RID: 3317
			internal const string NO_SPROC_GENERATOR_MSG = "We have no stored procedure generator for specified backend";

			// Token: 0x04000CF6 RID: 3318
			internal const int NO_SPROC_GENERATOR_CODE = 20028;

			// Token: 0x04000CF7 RID: 3319
			internal const string SOURCE_IS_NOT_DBSOURCE_MSG = "The source for this table is not a DbSource";

			// Token: 0x04000CF8 RID: 3320
			internal const int SOURCE_IS_NOT_DBSOURCE_CODE = 20029;

			// Token: 0x04000CF9 RID: 3321
			internal const string BINDABLE_CTRL_NOT_FOUND_MSG = "Bindable control could not be found; Visual Studio data binding configuration file might be corrupt";

			// Token: 0x04000CFA RID: 3322
			internal const int BINDABLE_CTRL_NOT_FOUND_CODE = 20030;

			// Token: 0x04000CFB RID: 3323
			internal const string CANNOT_GET_PROVIDER_FACTORY_MSG = "Could not retrieve the provider factory.";

			// Token: 0x04000CFC RID: 3324
			internal const int CANNOT_GET_PROVIDER_FACTORY_CODE = 20031;

			// Token: 0x04000CFD RID: 3325
			internal const string FACTORY_CANNOT_CREATE_ADAPTERS_MSG = "The provider factory does not support creating adapters.";

			// Token: 0x04000CFE RID: 3326
			internal const int FACTORY_CANNOT_CREATE_ADAPTERS_CODE = 20032;

			// Token: 0x04000CFF RID: 3327
			internal const string FACTORY_CANNOT_CREATE_COMMAND_BUILDERS_MSG = "The provider factory does not support creating command builders.";

			// Token: 0x04000D00 RID: 3328
			internal const int FACTORY_CANNOT_CREATE_COMMAND_BUILDERS_CODE = 20033;

			// Token: 0x04000D01 RID: 3329
			internal const string FACTORY_COULD_NOT_CREATE_ADAPTER_MSG = "The provider factory failed to create an adapter.";

			// Token: 0x04000D02 RID: 3330
			internal const int FACTORY_COULD_NOT_CREATE_ADAPTER_CODE = 20034;

			// Token: 0x04000D03 RID: 3331
			internal const string FACTORY_COULD_NOT_CREATE_COMMAND_BUILDER_MSG = "The provider factory failed to create a command builder.";

			// Token: 0x04000D04 RID: 3332
			internal const int FACTORY_COULD_NOT_CREATE_COMMAND_BUILDER_CODE = 20035;

			// Token: 0x04000D05 RID: 3333
			internal const string CANNOT_GET_COMMAND_BUILDER_FROM_DBSOURCE_MSG = "Failed to get a command builder for the DbSource.";

			// Token: 0x04000D06 RID: 3334
			internal const int CANNOT_GET_COMMAND_BUILDER_FROM_DBSOURCE_CODE = 20036;
		}
	}
}
