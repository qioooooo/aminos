using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Data
{
	// Token: 0x02000028 RID: 40
	internal sealed class Res
	{
		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600009C RID: 156 RVA: 0x001C85E8 File Offset: 0x001C79E8
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

		// Token: 0x0600009D RID: 157 RVA: 0x001C8614 File Offset: 0x001C7A14
		internal Res()
		{
			this.resources = new ResourceManager("System.Data", base.GetType().Assembly);
		}

		// Token: 0x0600009E RID: 158 RVA: 0x001C8644 File Offset: 0x001C7A44
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

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600009F RID: 159 RVA: 0x001C86A0 File Offset: 0x001C7AA0
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x060000A0 RID: 160 RVA: 0x001C86B0 File Offset: 0x001C7AB0
		public static ResourceManager Resources
		{
			get
			{
				return Res.GetLoader().resources;
			}
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x001C86C8 File Offset: 0x001C7AC8
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

		// Token: 0x060000A2 RID: 162 RVA: 0x001C874C File Offset: 0x001C7B4C
		public static string GetString(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetString(name, Res.Culture);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x001C8778 File Offset: 0x001C7B78
		public static object GetObject(string name)
		{
			Res res = Res.GetLoader();
			if (res == null)
			{
				return null;
			}
			return res.resources.GetObject(name, Res.Culture);
		}

		// Token: 0x04000074 RID: 116
		internal const string ADP_Ascending = "ADP_Ascending";

		// Token: 0x04000075 RID: 117
		internal const string ADP_CollectionIndexInt32 = "ADP_CollectionIndexInt32";

		// Token: 0x04000076 RID: 118
		internal const string ADP_CollectionIndexString = "ADP_CollectionIndexString";

		// Token: 0x04000077 RID: 119
		internal const string ADP_CollectionInvalidType = "ADP_CollectionInvalidType";

		// Token: 0x04000078 RID: 120
		internal const string ADP_CollectionIsNotParent = "ADP_CollectionIsNotParent";

		// Token: 0x04000079 RID: 121
		internal const string ADP_CollectionIsParent = "ADP_CollectionIsParent";

		// Token: 0x0400007A RID: 122
		internal const string ADP_CollectionNullValue = "ADP_CollectionNullValue";

		// Token: 0x0400007B RID: 123
		internal const string ADP_CollectionRemoveInvalidObject = "ADP_CollectionRemoveInvalidObject";

		// Token: 0x0400007C RID: 124
		internal const string ADP_CollectionUniqueValue = "ADP_CollectionUniqueValue";

		// Token: 0x0400007D RID: 125
		internal const string ADP_ConnectionAlreadyOpen = "ADP_ConnectionAlreadyOpen";

		// Token: 0x0400007E RID: 126
		internal const string ADP_ConnectionStateMsg_Closed = "ADP_ConnectionStateMsg_Closed";

		// Token: 0x0400007F RID: 127
		internal const string ADP_ConnectionStateMsg_Connecting = "ADP_ConnectionStateMsg_Connecting";

		// Token: 0x04000080 RID: 128
		internal const string ADP_ConnectionStateMsg_Open = "ADP_ConnectionStateMsg_Open";

		// Token: 0x04000081 RID: 129
		internal const string ADP_ConnectionStateMsg_OpenExecuting = "ADP_ConnectionStateMsg_OpenExecuting";

		// Token: 0x04000082 RID: 130
		internal const string ADP_ConnectionStateMsg_OpenFetching = "ADP_ConnectionStateMsg_OpenFetching";

		// Token: 0x04000083 RID: 131
		internal const string ADP_ConnectionStateMsg = "ADP_ConnectionStateMsg";

		// Token: 0x04000084 RID: 132
		internal const string ADP_ConnectionStringSyntax = "ADP_ConnectionStringSyntax";

		// Token: 0x04000085 RID: 133
		internal const string ADP_DataReaderClosed = "ADP_DataReaderClosed";

		// Token: 0x04000086 RID: 134
		internal const string ADP_DelegatedTransactionPresent = "ADP_DelegatedTransactionPresent";

		// Token: 0x04000087 RID: 135
		internal const string ADP_Descending = "ADP_Descending";

		// Token: 0x04000088 RID: 136
		internal const string ADP_EmptyString = "ADP_EmptyString";

		// Token: 0x04000089 RID: 137
		internal const string ADP_InternalConnectionError = "ADP_InternalConnectionError";

		// Token: 0x0400008A RID: 138
		internal const string ADP_InvalidDataDirectory = "ADP_InvalidDataDirectory";

		// Token: 0x0400008B RID: 139
		internal const string ADP_InvalidEnumerationValue = "ADP_InvalidEnumerationValue";

		// Token: 0x0400008C RID: 140
		internal const string ADP_InvalidKey = "ADP_InvalidKey";

		// Token: 0x0400008D RID: 141
		internal const string ADP_InvalidOffsetValue = "ADP_InvalidOffsetValue";

		// Token: 0x0400008E RID: 142
		internal const string ADP_InvalidValue = "ADP_InvalidValue";

		// Token: 0x0400008F RID: 143
		internal const string ADP_InvalidXMLBadVersion = "ADP_InvalidXMLBadVersion";

		// Token: 0x04000090 RID: 144
		internal const string ADP_NoConnectionString = "ADP_NoConnectionString";

		// Token: 0x04000091 RID: 145
		internal const string ADP_NonCLSException = "ADP_NonCLSException";

		// Token: 0x04000092 RID: 146
		internal const string ADP_NotAPermissionElement = "ADP_NotAPermissionElement";

		// Token: 0x04000093 RID: 147
		internal const string ADP_OpenConnectionPropertySet = "ADP_OpenConnectionPropertySet";

		// Token: 0x04000094 RID: 148
		internal const string ADP_PermissionTypeMismatch = "ADP_PermissionTypeMismatch";

		// Token: 0x04000095 RID: 149
		internal const string ADP_PooledOpenTimeout = "ADP_PooledOpenTimeout";

		// Token: 0x04000096 RID: 150
		internal const string DataCategory_Data = "DataCategory_Data";

		// Token: 0x04000097 RID: 151
		internal const string DataCategory_StateChange = "DataCategory_StateChange";

		// Token: 0x04000098 RID: 152
		internal const string DataCategory_Update = "DataCategory_Update";

		// Token: 0x04000099 RID: 153
		internal const string DbCommand_CommandTimeout = "DbCommand_CommandTimeout";

		// Token: 0x0400009A RID: 154
		internal const string DbConnection_State = "DbConnection_State";

		// Token: 0x0400009B RID: 155
		internal const string DbConnection_StateChange = "DbConnection_StateChange";

		// Token: 0x0400009C RID: 156
		internal const string DbParameter_DbType = "DbParameter_DbType";

		// Token: 0x0400009D RID: 157
		internal const string DbParameter_Direction = "DbParameter_Direction";

		// Token: 0x0400009E RID: 158
		internal const string DbParameter_IsNullable = "DbParameter_IsNullable";

		// Token: 0x0400009F RID: 159
		internal const string DbParameter_Offset = "DbParameter_Offset";

		// Token: 0x040000A0 RID: 160
		internal const string DbParameter_ParameterName = "DbParameter_ParameterName";

		// Token: 0x040000A1 RID: 161
		internal const string DbParameter_Size = "DbParameter_Size";

		// Token: 0x040000A2 RID: 162
		internal const string DbParameter_SourceColumn = "DbParameter_SourceColumn";

		// Token: 0x040000A3 RID: 163
		internal const string DbParameter_SourceVersion = "DbParameter_SourceVersion";

		// Token: 0x040000A4 RID: 164
		internal const string DbParameter_SourceColumnNullMapping = "DbParameter_SourceColumnNullMapping";

		// Token: 0x040000A5 RID: 165
		internal const string DbParameter_Value = "DbParameter_Value";

		// Token: 0x040000A6 RID: 166
		internal const string MDF_QueryFailed = "MDF_QueryFailed";

		// Token: 0x040000A7 RID: 167
		internal const string MDF_TooManyRestrictions = "MDF_TooManyRestrictions";

		// Token: 0x040000A8 RID: 168
		internal const string MDF_InvalidRestrictionValue = "MDF_InvalidRestrictionValue";

		// Token: 0x040000A9 RID: 169
		internal const string MDF_UndefinedCollection = "MDF_UndefinedCollection";

		// Token: 0x040000AA RID: 170
		internal const string MDF_UndefinedPopulationMechanism = "MDF_UndefinedPopulationMechanism";

		// Token: 0x040000AB RID: 171
		internal const string MDF_UnsupportedVersion = "MDF_UnsupportedVersion";

		// Token: 0x040000AC RID: 172
		internal const string MDF_MissingDataSourceInformationColumn = "MDF_MissingDataSourceInformationColumn";

		// Token: 0x040000AD RID: 173
		internal const string MDF_IncorrectNumberOfDataSourceInformationRows = "MDF_IncorrectNumberOfDataSourceInformationRows";

		// Token: 0x040000AE RID: 174
		internal const string MDF_MissingRestrictionColumn = "MDF_MissingRestrictionColumn";

		// Token: 0x040000AF RID: 175
		internal const string MDF_MissingRestrictionRow = "MDF_MissingRestrictionRow";

		// Token: 0x040000B0 RID: 176
		internal const string MDF_NoColumns = "MDF_NoColumns";

		// Token: 0x040000B1 RID: 177
		internal const string MDF_UnableToBuildCollection = "MDF_UnableToBuildCollection";

		// Token: 0x040000B2 RID: 178
		internal const string MDF_AmbigousCollectionName = "MDF_AmbigousCollectionName";

		// Token: 0x040000B3 RID: 179
		internal const string MDF_CollectionNameISNotUnique = "MDF_CollectionNameISNotUnique";

		// Token: 0x040000B4 RID: 180
		internal const string MDF_DataTableDoesNotExist = "MDF_DataTableDoesNotExist";

		// Token: 0x040000B5 RID: 181
		internal const string MDF_InvalidXml = "MDF_InvalidXml";

		// Token: 0x040000B6 RID: 182
		internal const string MDF_InvalidXmlMissingColumn = "MDF_InvalidXmlMissingColumn";

		// Token: 0x040000B7 RID: 183
		internal const string MDF_InvalidXmlInvalidValue = "MDF_InvalidXmlInvalidValue";

		// Token: 0x040000B8 RID: 184
		internal const string DataCategory_Action = "DataCategory_Action";

		// Token: 0x040000B9 RID: 185
		internal const string DataCategory_Behavior = "DataCategory_Behavior";

		// Token: 0x040000BA RID: 186
		internal const string DataCategory_Fill = "DataCategory_Fill";

		// Token: 0x040000BB RID: 187
		internal const string DataCategory_InfoMessage = "DataCategory_InfoMessage";

		// Token: 0x040000BC RID: 188
		internal const string DataCategory_Mapping = "DataCategory_Mapping";

		// Token: 0x040000BD RID: 189
		internal const string DataCategory_StatementCompleted = "DataCategory_StatementCompleted";

		// Token: 0x040000BE RID: 190
		internal const string DataCategory_Udt = "DataCategory_Udt";

		// Token: 0x040000BF RID: 191
		internal const string DataCategory_Notification = "DataCategory_Notification";

		// Token: 0x040000C0 RID: 192
		internal const string DataCategory_Schema = "DataCategory_Schema";

		// Token: 0x040000C1 RID: 193
		internal const string DataCategory_Xml = "DataCategory_Xml";

		// Token: 0x040000C2 RID: 194
		internal const string DataCategory_Advanced = "DataCategory_Advanced";

		// Token: 0x040000C3 RID: 195
		internal const string DataCategory_Context = "DataCategory_Context";

		// Token: 0x040000C4 RID: 196
		internal const string DataCategory_Initialization = "DataCategory_Initialization";

		// Token: 0x040000C5 RID: 197
		internal const string DataCategory_Pooling = "DataCategory_Pooling";

		// Token: 0x040000C6 RID: 198
		internal const string DataCategory_NamedConnectionString = "DataCategory_NamedConnectionString";

		// Token: 0x040000C7 RID: 199
		internal const string DataCategory_Security = "DataCategory_Security";

		// Token: 0x040000C8 RID: 200
		internal const string DataCategory_Source = "DataCategory_Source";

		// Token: 0x040000C9 RID: 201
		internal const string DataCategory_Replication = "DataCategory_Replication";

		// Token: 0x040000CA RID: 202
		internal const string ExtendedPropertiesDescr = "ExtendedPropertiesDescr";

		// Token: 0x040000CB RID: 203
		internal const string DataSetCaseSensitiveDescr = "DataSetCaseSensitiveDescr";

		// Token: 0x040000CC RID: 204
		internal const string DataSetDataSetNameDescr = "DataSetDataSetNameDescr";

		// Token: 0x040000CD RID: 205
		internal const string DataSetDefaultViewDescr = "DataSetDefaultViewDescr";

		// Token: 0x040000CE RID: 206
		internal const string DataSetEnforceConstraintsDescr = "DataSetEnforceConstraintsDescr";

		// Token: 0x040000CF RID: 207
		internal const string DataSetHasErrorsDescr = "DataSetHasErrorsDescr";

		// Token: 0x040000D0 RID: 208
		internal const string DataSetLocaleDescr = "DataSetLocaleDescr";

		// Token: 0x040000D1 RID: 209
		internal const string DataSetNamespaceDescr = "DataSetNamespaceDescr";

		// Token: 0x040000D2 RID: 210
		internal const string DataSetPrefixDescr = "DataSetPrefixDescr";

		// Token: 0x040000D3 RID: 211
		internal const string DataSetRelationsDescr = "DataSetRelationsDescr";

		// Token: 0x040000D4 RID: 212
		internal const string DataSetTablesDescr = "DataSetTablesDescr";

		// Token: 0x040000D5 RID: 213
		internal const string DataSetMergeFailedDescr = "DataSetMergeFailedDescr";

		// Token: 0x040000D6 RID: 214
		internal const string DataSetInitializedDescr = "DataSetInitializedDescr";

		// Token: 0x040000D7 RID: 215
		internal const string DataSetDescr = "DataSetDescr";

		// Token: 0x040000D8 RID: 216
		internal const string DataTableCaseSensitiveDescr = "DataTableCaseSensitiveDescr";

		// Token: 0x040000D9 RID: 217
		internal const string DataTableChildRelationsDescr = "DataTableChildRelationsDescr";

		// Token: 0x040000DA RID: 218
		internal const string DataTableColumnsDescr = "DataTableColumnsDescr";

		// Token: 0x040000DB RID: 219
		internal const string DataTableConstraintsDescr = "DataTableConstraintsDescr";

		// Token: 0x040000DC RID: 220
		internal const string DataTableDataSetDescr = "DataTableDataSetDescr";

		// Token: 0x040000DD RID: 221
		internal const string DataTableDefaultViewDescr = "DataTableDefaultViewDescr";

		// Token: 0x040000DE RID: 222
		internal const string DataTableDisplayExpressionDescr = "DataTableDisplayExpressionDescr";

		// Token: 0x040000DF RID: 223
		internal const string DataTableHasErrorsDescr = "DataTableHasErrorsDescr";

		// Token: 0x040000E0 RID: 224
		internal const string DataTableLocaleDescr = "DataTableLocaleDescr";

		// Token: 0x040000E1 RID: 225
		internal const string DataTableMinimumCapacityDescr = "DataTableMinimumCapacityDescr";

		// Token: 0x040000E2 RID: 226
		internal const string DataTableNamespaceDescr = "DataTableNamespaceDescr";

		// Token: 0x040000E3 RID: 227
		internal const string DataTablePrefixDescr = "DataTablePrefixDescr";

		// Token: 0x040000E4 RID: 228
		internal const string DataTableParentRelationsDescr = "DataTableParentRelationsDescr";

		// Token: 0x040000E5 RID: 229
		internal const string DataTablePrimaryKeyDescr = "DataTablePrimaryKeyDescr";

		// Token: 0x040000E6 RID: 230
		internal const string DataTableRowsDescr = "DataTableRowsDescr";

		// Token: 0x040000E7 RID: 231
		internal const string DataTableTableNameDescr = "DataTableTableNameDescr";

		// Token: 0x040000E8 RID: 232
		internal const string DataTableRowChangedDescr = "DataTableRowChangedDescr";

		// Token: 0x040000E9 RID: 233
		internal const string DataTableRowChangingDescr = "DataTableRowChangingDescr";

		// Token: 0x040000EA RID: 234
		internal const string DataTableRowDeletedDescr = "DataTableRowDeletedDescr";

		// Token: 0x040000EB RID: 235
		internal const string DataTableRowDeletingDescr = "DataTableRowDeletingDescr";

		// Token: 0x040000EC RID: 236
		internal const string DataTableColumnChangingDescr = "DataTableColumnChangingDescr";

		// Token: 0x040000ED RID: 237
		internal const string DataTableColumnChangedDescr = "DataTableColumnChangedDescr";

		// Token: 0x040000EE RID: 238
		internal const string DataTableRowsClearingDescr = "DataTableRowsClearingDescr";

		// Token: 0x040000EF RID: 239
		internal const string DataTableRowsClearedDescr = "DataTableRowsClearedDescr";

		// Token: 0x040000F0 RID: 240
		internal const string DataTableRowsNewRowDescr = "DataTableRowsNewRowDescr";

		// Token: 0x040000F1 RID: 241
		internal const string DataRelationRelationNameDescr = "DataRelationRelationNameDescr";

		// Token: 0x040000F2 RID: 242
		internal const string DataRelationChildColumnsDescr = "DataRelationChildColumnsDescr";

		// Token: 0x040000F3 RID: 243
		internal const string DataRelationParentColumnsDescr = "DataRelationParentColumnsDescr";

		// Token: 0x040000F4 RID: 244
		internal const string DataRelationNested = "DataRelationNested";

		// Token: 0x040000F5 RID: 245
		internal const string ForeignKeyConstraintDeleteRuleDescr = "ForeignKeyConstraintDeleteRuleDescr";

		// Token: 0x040000F6 RID: 246
		internal const string ForeignKeyConstraintUpdateRuleDescr = "ForeignKeyConstraintUpdateRuleDescr";

		// Token: 0x040000F7 RID: 247
		internal const string ForeignKeyConstraintAcceptRejectRuleDescr = "ForeignKeyConstraintAcceptRejectRuleDescr";

		// Token: 0x040000F8 RID: 248
		internal const string ForeignKeyConstraintChildColumnsDescr = "ForeignKeyConstraintChildColumnsDescr";

		// Token: 0x040000F9 RID: 249
		internal const string ForeignKeyConstraintParentColumnsDescr = "ForeignKeyConstraintParentColumnsDescr";

		// Token: 0x040000FA RID: 250
		internal const string ForeignKeyRelatedTableDescr = "ForeignKeyRelatedTableDescr";

		// Token: 0x040000FB RID: 251
		internal const string KeyConstraintColumnsDescr = "KeyConstraintColumnsDescr";

		// Token: 0x040000FC RID: 252
		internal const string KeyConstraintIsPrimaryKeyDescr = "KeyConstraintIsPrimaryKeyDescr";

		// Token: 0x040000FD RID: 253
		internal const string ConstraintNameDescr = "ConstraintNameDescr";

		// Token: 0x040000FE RID: 254
		internal const string ConstraintTableDescr = "ConstraintTableDescr";

		// Token: 0x040000FF RID: 255
		internal const string DataColumnAllowNullDescr = "DataColumnAllowNullDescr";

		// Token: 0x04000100 RID: 256
		internal const string DataColumnAutoIncrementDescr = "DataColumnAutoIncrementDescr";

		// Token: 0x04000101 RID: 257
		internal const string DataColumnAutoIncrementSeedDescr = "DataColumnAutoIncrementSeedDescr";

		// Token: 0x04000102 RID: 258
		internal const string DataColumnAutoIncrementStepDescr = "DataColumnAutoIncrementStepDescr";

		// Token: 0x04000103 RID: 259
		internal const string DataColumnCaptionDescr = "DataColumnCaptionDescr";

		// Token: 0x04000104 RID: 260
		internal const string DataColumnColumnNameDescr = "DataColumnColumnNameDescr";

		// Token: 0x04000105 RID: 261
		internal const string DataColumnDataTableDescr = "DataColumnDataTableDescr";

		// Token: 0x04000106 RID: 262
		internal const string DataColumnDataTypeDescr = "DataColumnDataTypeDescr";

		// Token: 0x04000107 RID: 263
		internal const string DataColumnDefaultValueDescr = "DataColumnDefaultValueDescr";

		// Token: 0x04000108 RID: 264
		internal const string DataColumnExpressionDescr = "DataColumnExpressionDescr";

		// Token: 0x04000109 RID: 265
		internal const string DataColumnMappingDescr = "DataColumnMappingDescr";

		// Token: 0x0400010A RID: 266
		internal const string DataColumnNamespaceDescr = "DataColumnNamespaceDescr";

		// Token: 0x0400010B RID: 267
		internal const string DataColumnPrefixDescr = "DataColumnPrefixDescr";

		// Token: 0x0400010C RID: 268
		internal const string DataColumnOrdinalDescr = "DataColumnOrdinalDescr";

		// Token: 0x0400010D RID: 269
		internal const string DataColumnReadOnlyDescr = "DataColumnReadOnlyDescr";

		// Token: 0x0400010E RID: 270
		internal const string DataColumnUniqueDescr = "DataColumnUniqueDescr";

		// Token: 0x0400010F RID: 271
		internal const string DataColumnMaxLengthDescr = "DataColumnMaxLengthDescr";

		// Token: 0x04000110 RID: 272
		internal const string DataColumnDateTimeModeDescr = "DataColumnDateTimeModeDescr";

		// Token: 0x04000111 RID: 273
		internal const string DataViewAllowDeleteDescr = "DataViewAllowDeleteDescr";

		// Token: 0x04000112 RID: 274
		internal const string DataViewAllowEditDescr = "DataViewAllowEditDescr";

		// Token: 0x04000113 RID: 275
		internal const string DataViewAllowNewDescr = "DataViewAllowNewDescr";

		// Token: 0x04000114 RID: 276
		internal const string DataViewCountDescr = "DataViewCountDescr";

		// Token: 0x04000115 RID: 277
		internal const string DataViewDataViewManagerDescr = "DataViewDataViewManagerDescr";

		// Token: 0x04000116 RID: 278
		internal const string DataViewIsOpenDescr = "DataViewIsOpenDescr";

		// Token: 0x04000117 RID: 279
		internal const string DataViewRowFilterDescr = "DataViewRowFilterDescr";

		// Token: 0x04000118 RID: 280
		internal const string DataViewRowStateFilterDescr = "DataViewRowStateFilterDescr";

		// Token: 0x04000119 RID: 281
		internal const string DataViewSortDescr = "DataViewSortDescr";

		// Token: 0x0400011A RID: 282
		internal const string DataViewApplyDefaultSortDescr = "DataViewApplyDefaultSortDescr";

		// Token: 0x0400011B RID: 283
		internal const string DataViewTableDescr = "DataViewTableDescr";

		// Token: 0x0400011C RID: 284
		internal const string DataViewListChangedDescr = "DataViewListChangedDescr";

		// Token: 0x0400011D RID: 285
		internal const string DataViewManagerDataSetDescr = "DataViewManagerDataSetDescr";

		// Token: 0x0400011E RID: 286
		internal const string DataViewManagerTableSettingsDescr = "DataViewManagerTableSettingsDescr";

		// Token: 0x0400011F RID: 287
		internal const string Xml_SimpleTypeNotSupported = "Xml_SimpleTypeNotSupported";

		// Token: 0x04000120 RID: 288
		internal const string Xml_MissingAttribute = "Xml_MissingAttribute";

		// Token: 0x04000121 RID: 289
		internal const string Xml_ValueOutOfRange = "Xml_ValueOutOfRange";

		// Token: 0x04000122 RID: 290
		internal const string Xml_AttributeValues = "Xml_AttributeValues";

		// Token: 0x04000123 RID: 291
		internal const string Xml_ElementTypeNotFound = "Xml_ElementTypeNotFound";

		// Token: 0x04000124 RID: 292
		internal const string Xml_RelationParentNameMissing = "Xml_RelationParentNameMissing";

		// Token: 0x04000125 RID: 293
		internal const string Xml_RelationChildNameMissing = "Xml_RelationChildNameMissing";

		// Token: 0x04000126 RID: 294
		internal const string Xml_RelationTableKeyMissing = "Xml_RelationTableKeyMissing";

		// Token: 0x04000127 RID: 295
		internal const string Xml_RelationChildKeyMissing = "Xml_RelationChildKeyMissing";

		// Token: 0x04000128 RID: 296
		internal const string Xml_UndefinedDatatype = "Xml_UndefinedDatatype";

		// Token: 0x04000129 RID: 297
		internal const string Xml_DatatypeNotDefined = "Xml_DatatypeNotDefined";

		// Token: 0x0400012A RID: 298
		internal const string Xml_InvalidField = "Xml_InvalidField";

		// Token: 0x0400012B RID: 299
		internal const string Xml_InvalidSelector = "Xml_InvalidSelector";

		// Token: 0x0400012C RID: 300
		internal const string Xml_InvalidKey = "Xml_InvalidKey";

		// Token: 0x0400012D RID: 301
		internal const string Xml_DuplicateConstraint = "Xml_DuplicateConstraint";

		// Token: 0x0400012E RID: 302
		internal const string Xml_CannotConvert = "Xml_CannotConvert";

		// Token: 0x0400012F RID: 303
		internal const string Xml_MissingRefer = "Xml_MissingRefer";

		// Token: 0x04000130 RID: 304
		internal const string Xml_MismatchKeyLength = "Xml_MismatchKeyLength";

		// Token: 0x04000131 RID: 305
		internal const string Xml_CircularComplexType = "Xml_CircularComplexType";

		// Token: 0x04000132 RID: 306
		internal const string Xml_CannotInstantiateAbstract = "Xml_CannotInstantiateAbstract";

		// Token: 0x04000133 RID: 307
		internal const string Xml_MergeDuplicateDeclaration = "Xml_MergeDuplicateDeclaration";

		// Token: 0x04000134 RID: 308
		internal const string Xml_MissingTable = "Xml_MissingTable";

		// Token: 0x04000135 RID: 309
		internal const string Xml_MissingSQL = "Xml_MissingSQL";

		// Token: 0x04000136 RID: 310
		internal const string Xml_ColumnConflict = "Xml_ColumnConflict";

		// Token: 0x04000137 RID: 311
		internal const string Xml_InvalidPrefix = "Xml_InvalidPrefix";

		// Token: 0x04000138 RID: 312
		internal const string Xml_NestedCircular = "Xml_NestedCircular";

		// Token: 0x04000139 RID: 313
		internal const string Xml_FoundEntity = "Xml_FoundEntity";

		// Token: 0x0400013A RID: 314
		internal const string Xml_PolymorphismNotSupported = "Xml_PolymorphismNotSupported";

		// Token: 0x0400013B RID: 315
		internal const string Xml_CanNotDeserializeObjectType = "Xml_CanNotDeserializeObjectType";

		// Token: 0x0400013C RID: 316
		internal const string Xml_DataTableInferenceNotSupported = "Xml_DataTableInferenceNotSupported";

		// Token: 0x0400013D RID: 317
		internal const string Xml_MultipleParentRows = "Xml_MultipleParentRows";

		// Token: 0x0400013E RID: 318
		internal const string Xml_IsDataSetAttributeMissingInSchema = "Xml_IsDataSetAttributeMissingInSchema";

		// Token: 0x0400013F RID: 319
		internal const string Xml_TooManyIsDataSetAtributeInSchema = "Xml_TooManyIsDataSetAtributeInSchema";

		// Token: 0x04000140 RID: 320
		internal const string Expr_NYI = "Expr_NYI";

		// Token: 0x04000141 RID: 321
		internal const string Expr_MissingOperand = "Expr_MissingOperand";

		// Token: 0x04000142 RID: 322
		internal const string Expr_TypeMismatch = "Expr_TypeMismatch";

		// Token: 0x04000143 RID: 323
		internal const string Expr_ExpressionTooComplex = "Expr_ExpressionTooComplex";

		// Token: 0x04000144 RID: 324
		internal const string Expr_UnboundName = "Expr_UnboundName";

		// Token: 0x04000145 RID: 325
		internal const string Expr_InvalidString = "Expr_InvalidString";

		// Token: 0x04000146 RID: 326
		internal const string Expr_UndefinedFunction = "Expr_UndefinedFunction";

		// Token: 0x04000147 RID: 327
		internal const string Expr_Syntax = "Expr_Syntax";

		// Token: 0x04000148 RID: 328
		internal const string Expr_FunctionArgumentCount = "Expr_FunctionArgumentCount";

		// Token: 0x04000149 RID: 329
		internal const string Expr_MissingRightParen = "Expr_MissingRightParen";

		// Token: 0x0400014A RID: 330
		internal const string Expr_UnknownToken = "Expr_UnknownToken";

		// Token: 0x0400014B RID: 331
		internal const string Expr_UnknownToken1 = "Expr_UnknownToken1";

		// Token: 0x0400014C RID: 332
		internal const string Expr_DatatypeConvertion = "Expr_DatatypeConvertion";

		// Token: 0x0400014D RID: 333
		internal const string Expr_DatavalueConvertion = "Expr_DatavalueConvertion";

		// Token: 0x0400014E RID: 334
		internal const string Expr_InvalidName = "Expr_InvalidName";

		// Token: 0x0400014F RID: 335
		internal const string Expr_InvalidDate = "Expr_InvalidDate";

		// Token: 0x04000150 RID: 336
		internal const string Expr_NonConstantArgument = "Expr_NonConstantArgument";

		// Token: 0x04000151 RID: 337
		internal const string Expr_InvalidPattern = "Expr_InvalidPattern";

		// Token: 0x04000152 RID: 338
		internal const string Expr_InWithoutParentheses = "Expr_InWithoutParentheses";

		// Token: 0x04000153 RID: 339
		internal const string Expr_ArgumentType = "Expr_ArgumentType";

		// Token: 0x04000154 RID: 340
		internal const string Expr_ArgumentTypeInteger = "Expr_ArgumentTypeInteger";

		// Token: 0x04000155 RID: 341
		internal const string Expr_TypeMismatchInBinop = "Expr_TypeMismatchInBinop";

		// Token: 0x04000156 RID: 342
		internal const string Expr_AmbiguousBinop = "Expr_AmbiguousBinop";

		// Token: 0x04000157 RID: 343
		internal const string Expr_InWithoutList = "Expr_InWithoutList";

		// Token: 0x04000158 RID: 344
		internal const string Expr_UnsupportedOperator = "Expr_UnsupportedOperator";

		// Token: 0x04000159 RID: 345
		internal const string Expr_InvalidNameBracketing = "Expr_InvalidNameBracketing";

		// Token: 0x0400015A RID: 346
		internal const string Expr_MissingOperandBefore = "Expr_MissingOperandBefore";

		// Token: 0x0400015B RID: 347
		internal const string Expr_TooManyRightParentheses = "Expr_TooManyRightParentheses";

		// Token: 0x0400015C RID: 348
		internal const string Expr_UnresolvedRelation = "Expr_UnresolvedRelation";

		// Token: 0x0400015D RID: 349
		internal const string Expr_AggregateArgument = "Expr_AggregateArgument";

		// Token: 0x0400015E RID: 350
		internal const string Expr_AggregateUnbound = "Expr_AggregateUnbound";

		// Token: 0x0400015F RID: 351
		internal const string Expr_EvalNoContext = "Expr_EvalNoContext";

		// Token: 0x04000160 RID: 352
		internal const string Expr_ExpressionUnbound = "Expr_ExpressionUnbound";

		// Token: 0x04000161 RID: 353
		internal const string Expr_ComputeNotAggregate = "Expr_ComputeNotAggregate";

		// Token: 0x04000162 RID: 354
		internal const string Expr_FilterConvertion = "Expr_FilterConvertion";

		// Token: 0x04000163 RID: 355
		internal const string Expr_InvalidType = "Expr_InvalidType";

		// Token: 0x04000164 RID: 356
		internal const string Expr_LookupArgument = "Expr_LookupArgument";

		// Token: 0x04000165 RID: 357
		internal const string Expr_InvokeArgument = "Expr_InvokeArgument";

		// Token: 0x04000166 RID: 358
		internal const string Expr_ArgumentOutofRange = "Expr_ArgumentOutofRange";

		// Token: 0x04000167 RID: 359
		internal const string Expr_IsSyntax = "Expr_IsSyntax";

		// Token: 0x04000168 RID: 360
		internal const string Expr_Overflow = "Expr_Overflow";

		// Token: 0x04000169 RID: 361
		internal const string Expr_DivideByZero = "Expr_DivideByZero";

		// Token: 0x0400016A RID: 362
		internal const string Expr_BindFailure = "Expr_BindFailure";

		// Token: 0x0400016B RID: 363
		internal const string Expr_InvalidHoursArgument = "Expr_InvalidHoursArgument";

		// Token: 0x0400016C RID: 364
		internal const string Expr_InvalidMinutesArgument = "Expr_InvalidMinutesArgument";

		// Token: 0x0400016D RID: 365
		internal const string Expr_InvalidTimeZoneRange = "Expr_InvalidTimeZoneRange";

		// Token: 0x0400016E RID: 366
		internal const string Expr_MismatchKindandTimeSpan = "Expr_MismatchKindandTimeSpan";

		// Token: 0x0400016F RID: 367
		internal const string Data_EnforceConstraints = "Data_EnforceConstraints";

		// Token: 0x04000170 RID: 368
		internal const string Data_CannotModifyCollection = "Data_CannotModifyCollection";

		// Token: 0x04000171 RID: 369
		internal const string Data_CaseInsensitiveNameConflict = "Data_CaseInsensitiveNameConflict";

		// Token: 0x04000172 RID: 370
		internal const string Data_NamespaceNameConflict = "Data_NamespaceNameConflict";

		// Token: 0x04000173 RID: 371
		internal const string Data_InvalidOffsetLength = "Data_InvalidOffsetLength";

		// Token: 0x04000174 RID: 372
		internal const string Data_ArgumentOutOfRange = "Data_ArgumentOutOfRange";

		// Token: 0x04000175 RID: 373
		internal const string Data_ArgumentNull = "Data_ArgumentNull";

		// Token: 0x04000176 RID: 374
		internal const string Data_ArgumentContainsNull = "Data_ArgumentContainsNull";

		// Token: 0x04000177 RID: 375
		internal const string Data_TypeNotAllowed = "Data_TypeNotAllowed";

		// Token: 0x04000178 RID: 376
		internal const string Config_ElementNotAllowed = "Config_ElementNotAllowed";

		// Token: 0x04000179 RID: 377
		internal const string DataColumns_OutOfRange = "DataColumns_OutOfRange";

		// Token: 0x0400017A RID: 378
		internal const string DataColumns_Add1 = "DataColumns_Add1";

		// Token: 0x0400017B RID: 379
		internal const string DataColumns_Add2 = "DataColumns_Add2";

		// Token: 0x0400017C RID: 380
		internal const string DataColumns_Add3 = "DataColumns_Add3";

		// Token: 0x0400017D RID: 381
		internal const string DataColumns_Add4 = "DataColumns_Add4";

		// Token: 0x0400017E RID: 382
		internal const string DataColumns_AddDuplicate = "DataColumns_AddDuplicate";

		// Token: 0x0400017F RID: 383
		internal const string DataColumns_AddDuplicate2 = "DataColumns_AddDuplicate2";

		// Token: 0x04000180 RID: 384
		internal const string DataColumns_AddDuplicate3 = "DataColumns_AddDuplicate3";

		// Token: 0x04000181 RID: 385
		internal const string DataColumns_Remove = "DataColumns_Remove";

		// Token: 0x04000182 RID: 386
		internal const string DataColumns_RemovePrimaryKey = "DataColumns_RemovePrimaryKey";

		// Token: 0x04000183 RID: 387
		internal const string DataColumns_RemoveChildKey = "DataColumns_RemoveChildKey";

		// Token: 0x04000184 RID: 388
		internal const string DataColumns_RemoveConstraint = "DataColumns_RemoveConstraint";

		// Token: 0x04000185 RID: 389
		internal const string DataColumns_RemoveExpression = "DataColumns_RemoveExpression";

		// Token: 0x04000186 RID: 390
		internal const string DataColumn_AutoIncrementAndExpression = "DataColumn_AutoIncrementAndExpression";

		// Token: 0x04000187 RID: 391
		internal const string DataColumn_AutoIncrementAndDefaultValue = "DataColumn_AutoIncrementAndDefaultValue";

		// Token: 0x04000188 RID: 392
		internal const string DataColumn_DefaultValueAndAutoIncrement = "DataColumn_DefaultValueAndAutoIncrement";

		// Token: 0x04000189 RID: 393
		internal const string DataColumn_AutoIncrementSeed = "DataColumn_AutoIncrementSeed";

		// Token: 0x0400018A RID: 394
		internal const string DataColumn_NameRequired = "DataColumn_NameRequired";

		// Token: 0x0400018B RID: 395
		internal const string DataColumn_ChangeDataType = "DataColumn_ChangeDataType";

		// Token: 0x0400018C RID: 396
		internal const string DataColumn_NullDataType = "DataColumn_NullDataType";

		// Token: 0x0400018D RID: 397
		internal const string DataColumn_DefaultValueDataType = "DataColumn_DefaultValueDataType";

		// Token: 0x0400018E RID: 398
		internal const string DataColumn_DefaultValueDataType1 = "DataColumn_DefaultValueDataType1";

		// Token: 0x0400018F RID: 399
		internal const string DataColumn_DefaultValueColumnDataType = "DataColumn_DefaultValueColumnDataType";

		// Token: 0x04000190 RID: 400
		internal const string DataColumn_ReadOnlyAndExpression = "DataColumn_ReadOnlyAndExpression";

		// Token: 0x04000191 RID: 401
		internal const string DataColumn_UniqueAndExpression = "DataColumn_UniqueAndExpression";

		// Token: 0x04000192 RID: 402
		internal const string DataColumn_ExpressionAndUnique = "DataColumn_ExpressionAndUnique";

		// Token: 0x04000193 RID: 403
		internal const string DataColumn_ExpressionAndReadOnly = "DataColumn_ExpressionAndReadOnly";

		// Token: 0x04000194 RID: 404
		internal const string DataColumn_ExpressionAndConstraint = "DataColumn_ExpressionAndConstraint";

		// Token: 0x04000195 RID: 405
		internal const string DataColumn_ExpressionInConstraint = "DataColumn_ExpressionInConstraint";

		// Token: 0x04000196 RID: 406
		internal const string DataColumn_ExpressionCircular = "DataColumn_ExpressionCircular";

		// Token: 0x04000197 RID: 407
		internal const string DataColumn_NullKeyValues = "DataColumn_NullKeyValues";

		// Token: 0x04000198 RID: 408
		internal const string DataColumn_NullValues = "DataColumn_NullValues";

		// Token: 0x04000199 RID: 409
		internal const string DataColumn_ReadOnly = "DataColumn_ReadOnly";

		// Token: 0x0400019A RID: 410
		internal const string DataColumn_NonUniqueValues = "DataColumn_NonUniqueValues";

		// Token: 0x0400019B RID: 411
		internal const string DataColumn_NotInTheTable = "DataColumn_NotInTheTable";

		// Token: 0x0400019C RID: 412
		internal const string DataColumn_NotInAnyTable = "DataColumn_NotInAnyTable";

		// Token: 0x0400019D RID: 413
		internal const string DataColumn_SetFailed = "DataColumn_SetFailed";

		// Token: 0x0400019E RID: 414
		internal const string DataColumn_CannotSetToNull = "DataColumn_CannotSetToNull";

		// Token: 0x0400019F RID: 415
		internal const string DataColumn_LongerThanMaxLength = "DataColumn_LongerThanMaxLength";

		// Token: 0x040001A0 RID: 416
		internal const string DataColumn_HasToBeStringType = "DataColumn_HasToBeStringType";

		// Token: 0x040001A1 RID: 417
		internal const string DataColumn_CannotSetMaxLength = "DataColumn_CannotSetMaxLength";

		// Token: 0x040001A2 RID: 418
		internal const string DataColumn_CannotSetMaxLength2 = "DataColumn_CannotSetMaxLength2";

		// Token: 0x040001A3 RID: 419
		internal const string DataColumn_CannotSimpleContentType = "DataColumn_CannotSimpleContentType";

		// Token: 0x040001A4 RID: 420
		internal const string DataColumn_CannotSimpleContent = "DataColumn_CannotSimpleContent";

		// Token: 0x040001A5 RID: 421
		internal const string DataColumn_ExceedMaxLength = "DataColumn_ExceedMaxLength";

		// Token: 0x040001A6 RID: 422
		internal const string DataColumn_NotAllowDBNull = "DataColumn_NotAllowDBNull";

		// Token: 0x040001A7 RID: 423
		internal const string DataColumn_CannotChangeNamespace = "DataColumn_CannotChangeNamespace";

		// Token: 0x040001A8 RID: 424
		internal const string DataColumn_AutoIncrementCannotSetIfHasData = "DataColumn_AutoIncrementCannotSetIfHasData";

		// Token: 0x040001A9 RID: 425
		internal const string DataColumn_NotInTheUnderlyingTable = "DataColumn_NotInTheUnderlyingTable";

		// Token: 0x040001AA RID: 426
		internal const string DataColumn_InvalidDataColumnMapping = "DataColumn_InvalidDataColumnMapping";

		// Token: 0x040001AB RID: 427
		internal const string DataColumn_CannotSetDateTimeModeForNonDateTimeColumns = "DataColumn_CannotSetDateTimeModeForNonDateTimeColumns";

		// Token: 0x040001AC RID: 428
		internal const string DataColumn_InvalidDateTimeMode = "DataColumn_InvalidDateTimeMode";

		// Token: 0x040001AD RID: 429
		internal const string DataColumn_DateTimeMode = "DataColumn_DateTimeMode";

		// Token: 0x040001AE RID: 430
		internal const string DataColumn_INullableUDTwithoutStaticNull = "DataColumn_INullableUDTwithoutStaticNull";

		// Token: 0x040001AF RID: 431
		internal const string DataColumn_UDTImplementsIChangeTrackingButnotIRevertible = "DataColumn_UDTImplementsIChangeTrackingButnotIRevertible";

		// Token: 0x040001B0 RID: 432
		internal const string DataColumn_SetAddedAndModifiedCalledOnNonUnchanged = "DataColumn_SetAddedAndModifiedCalledOnNonUnchanged";

		// Token: 0x040001B1 RID: 433
		internal const string DataColumn_OrdinalExceedMaximun = "DataColumn_OrdinalExceedMaximun";

		// Token: 0x040001B2 RID: 434
		internal const string DataColumn_NullableTypesNotSupported = "DataColumn_NullableTypesNotSupported";

		// Token: 0x040001B3 RID: 435
		internal const string DataConstraint_NoName = "DataConstraint_NoName";

		// Token: 0x040001B4 RID: 436
		internal const string DataConstraint_Violation = "DataConstraint_Violation";

		// Token: 0x040001B5 RID: 437
		internal const string DataConstraint_ViolationValue = "DataConstraint_ViolationValue";

		// Token: 0x040001B6 RID: 438
		internal const string DataConstraint_NotInTheTable = "DataConstraint_NotInTheTable";

		// Token: 0x040001B7 RID: 439
		internal const string DataConstraint_OutOfRange = "DataConstraint_OutOfRange";

		// Token: 0x040001B8 RID: 440
		internal const string DataConstraint_Duplicate = "DataConstraint_Duplicate";

		// Token: 0x040001B9 RID: 441
		internal const string DataConstraint_DuplicateName = "DataConstraint_DuplicateName";

		// Token: 0x040001BA RID: 442
		internal const string DataConstraint_UniqueViolation = "DataConstraint_UniqueViolation";

		// Token: 0x040001BB RID: 443
		internal const string DataConstraint_ForeignTable = "DataConstraint_ForeignTable";

		// Token: 0x040001BC RID: 444
		internal const string DataConstraint_ParentValues = "DataConstraint_ParentValues";

		// Token: 0x040001BD RID: 445
		internal const string DataConstraint_AddFailed = "DataConstraint_AddFailed";

		// Token: 0x040001BE RID: 446
		internal const string DataConstraint_RemoveFailed = "DataConstraint_RemoveFailed";

		// Token: 0x040001BF RID: 447
		internal const string DataConstraint_NeededForForeignKeyConstraint = "DataConstraint_NeededForForeignKeyConstraint";

		// Token: 0x040001C0 RID: 448
		internal const string DataConstraint_CascadeDelete = "DataConstraint_CascadeDelete";

		// Token: 0x040001C1 RID: 449
		internal const string DataConstraint_CascadeUpdate = "DataConstraint_CascadeUpdate";

		// Token: 0x040001C2 RID: 450
		internal const string DataConstraint_ClearParentTable = "DataConstraint_ClearParentTable";

		// Token: 0x040001C3 RID: 451
		internal const string DataConstraint_ForeignKeyViolation = "DataConstraint_ForeignKeyViolation";

		// Token: 0x040001C4 RID: 452
		internal const string DataConstraint_BadObjectPropertyAccess = "DataConstraint_BadObjectPropertyAccess";

		// Token: 0x040001C5 RID: 453
		internal const string DataConstraint_RemoveParentRow = "DataConstraint_RemoveParentRow";

		// Token: 0x040001C6 RID: 454
		internal const string DataConstraint_AddPrimaryKeyConstraint = "DataConstraint_AddPrimaryKeyConstraint";

		// Token: 0x040001C7 RID: 455
		internal const string DataConstraint_CantAddConstraintToMultipleNestedTable = "DataConstraint_CantAddConstraintToMultipleNestedTable";

		// Token: 0x040001C8 RID: 456
		internal const string DataKey_TableMismatch = "DataKey_TableMismatch";

		// Token: 0x040001C9 RID: 457
		internal const string DataKey_NoColumns = "DataKey_NoColumns";

		// Token: 0x040001CA RID: 458
		internal const string DataKey_TooManyColumns = "DataKey_TooManyColumns";

		// Token: 0x040001CB RID: 459
		internal const string DataKey_DuplicateColumns = "DataKey_DuplicateColumns";

		// Token: 0x040001CC RID: 460
		internal const string DataKey_RemovePrimaryKey = "DataKey_RemovePrimaryKey";

		// Token: 0x040001CD RID: 461
		internal const string DataKey_RemovePrimaryKey1 = "DataKey_RemovePrimaryKey1";

		// Token: 0x040001CE RID: 462
		internal const string DataRelation_ColumnsTypeMismatch = "DataRelation_ColumnsTypeMismatch";

		// Token: 0x040001CF RID: 463
		internal const string DataRelation_KeyColumnsIdentical = "DataRelation_KeyColumnsIdentical";

		// Token: 0x040001D0 RID: 464
		internal const string DataRelation_KeyLengthMismatch = "DataRelation_KeyLengthMismatch";

		// Token: 0x040001D1 RID: 465
		internal const string DataRelation_KeyZeroLength = "DataRelation_KeyZeroLength";

		// Token: 0x040001D2 RID: 466
		internal const string DataRelation_ForeignRow = "DataRelation_ForeignRow";

		// Token: 0x040001D3 RID: 467
		internal const string DataRelation_NoName = "DataRelation_NoName";

		// Token: 0x040001D4 RID: 468
		internal const string DataRelation_ForeignTable = "DataRelation_ForeignTable";

		// Token: 0x040001D5 RID: 469
		internal const string DataRelation_ForeignDataSet = "DataRelation_ForeignDataSet";

		// Token: 0x040001D6 RID: 470
		internal const string DataRelation_GetParentRowTableMismatch = "DataRelation_GetParentRowTableMismatch";

		// Token: 0x040001D7 RID: 471
		internal const string DataRelation_SetParentRowTableMismatch = "DataRelation_SetParentRowTableMismatch";

		// Token: 0x040001D8 RID: 472
		internal const string DataRelation_DataSetMismatch = "DataRelation_DataSetMismatch";

		// Token: 0x040001D9 RID: 473
		internal const string DataRelation_TablesInDifferentSets = "DataRelation_TablesInDifferentSets";

		// Token: 0x040001DA RID: 474
		internal const string DataRelation_AlreadyExists = "DataRelation_AlreadyExists";

		// Token: 0x040001DB RID: 475
		internal const string DataRelation_DoesNotExist = "DataRelation_DoesNotExist";

		// Token: 0x040001DC RID: 476
		internal const string DataRelation_AlreadyInOtherDataSet = "DataRelation_AlreadyInOtherDataSet";

		// Token: 0x040001DD RID: 477
		internal const string DataRelation_AlreadyInTheDataSet = "DataRelation_AlreadyInTheDataSet";

		// Token: 0x040001DE RID: 478
		internal const string DataRelation_DuplicateName = "DataRelation_DuplicateName";

		// Token: 0x040001DF RID: 479
		internal const string DataRelation_NotInTheDataSet = "DataRelation_NotInTheDataSet";

		// Token: 0x040001E0 RID: 480
		internal const string DataRelation_OutOfRange = "DataRelation_OutOfRange";

		// Token: 0x040001E1 RID: 481
		internal const string DataRelation_TableNull = "DataRelation_TableNull";

		// Token: 0x040001E2 RID: 482
		internal const string DataRelation_TableWasRemoved = "DataRelation_TableWasRemoved";

		// Token: 0x040001E3 RID: 483
		internal const string DataRelation_ChildTableMismatch = "DataRelation_ChildTableMismatch";

		// Token: 0x040001E4 RID: 484
		internal const string DataRelation_ParentTableMismatch = "DataRelation_ParentTableMismatch";

		// Token: 0x040001E5 RID: 485
		internal const string DataRelation_RelationNestedReadOnly = "DataRelation_RelationNestedReadOnly";

		// Token: 0x040001E6 RID: 486
		internal const string DataRelation_TableCantBeNestedInTwoTables = "DataRelation_TableCantBeNestedInTwoTables";

		// Token: 0x040001E7 RID: 487
		internal const string DataRelation_LoopInNestedRelations = "DataRelation_LoopInNestedRelations";

		// Token: 0x040001E8 RID: 488
		internal const string DataRelation_CaseLocaleMismatch = "DataRelation_CaseLocaleMismatch";

		// Token: 0x040001E9 RID: 489
		internal const string DataRelation_ParentOrChildColumnsDoNotHaveDataSet = "DataRelation_ParentOrChildColumnsDoNotHaveDataSet";

		// Token: 0x040001EA RID: 490
		internal const string DataRelation_InValidNestedRelation = "DataRelation_InValidNestedRelation";

		// Token: 0x040001EB RID: 491
		internal const string DataRelation_InValidNamespaceInNestedRelation = "DataRelation_InValidNamespaceInNestedRelation";

		// Token: 0x040001EC RID: 492
		internal const string DataRow_NotInTheDataSet = "DataRow_NotInTheDataSet";

		// Token: 0x040001ED RID: 493
		internal const string DataRow_NotInTheTable = "DataRow_NotInTheTable";

		// Token: 0x040001EE RID: 494
		internal const string DataRow_ParentRowNotInTheDataSet = "DataRow_ParentRowNotInTheDataSet";

		// Token: 0x040001EF RID: 495
		internal const string DataRow_EditInRowChanging = "DataRow_EditInRowChanging";

		// Token: 0x040001F0 RID: 496
		internal const string DataRow_EndEditInRowChanging = "DataRow_EndEditInRowChanging";

		// Token: 0x040001F1 RID: 497
		internal const string DataRow_BeginEditInRowChanging = "DataRow_BeginEditInRowChanging";

		// Token: 0x040001F2 RID: 498
		internal const string DataRow_CancelEditInRowChanging = "DataRow_CancelEditInRowChanging";

		// Token: 0x040001F3 RID: 499
		internal const string DataRow_DeleteInRowDeleting = "DataRow_DeleteInRowDeleting";

		// Token: 0x040001F4 RID: 500
		internal const string DataRow_ValuesArrayLength = "DataRow_ValuesArrayLength";

		// Token: 0x040001F5 RID: 501
		internal const string DataRow_NoCurrentData = "DataRow_NoCurrentData";

		// Token: 0x040001F6 RID: 502
		internal const string DataRow_NoOriginalData = "DataRow_NoOriginalData";

		// Token: 0x040001F7 RID: 503
		internal const string DataRow_NoProposedData = "DataRow_NoProposedData";

		// Token: 0x040001F8 RID: 504
		internal const string DataRow_RemovedFromTheTable = "DataRow_RemovedFromTheTable";

		// Token: 0x040001F9 RID: 505
		internal const string DataRow_DeletedRowInaccessible = "DataRow_DeletedRowInaccessible";

		// Token: 0x040001FA RID: 506
		internal const string DataRow_InvalidVersion = "DataRow_InvalidVersion";

		// Token: 0x040001FB RID: 507
		internal const string DataRow_OutOfRange = "DataRow_OutOfRange";

		// Token: 0x040001FC RID: 508
		internal const string DataRow_RowInsertOutOfRange = "DataRow_RowInsertOutOfRange";

		// Token: 0x040001FD RID: 509
		internal const string DataRow_RowInsertTwice = "DataRow_RowInsertTwice";

		// Token: 0x040001FE RID: 510
		internal const string DataRow_RowInsertMissing = "DataRow_RowInsertMissing";

		// Token: 0x040001FF RID: 511
		internal const string DataRow_RowOutOfRange = "DataRow_RowOutOfRange";

		// Token: 0x04000200 RID: 512
		internal const string DataRow_AlreadyInOtherCollection = "DataRow_AlreadyInOtherCollection";

		// Token: 0x04000201 RID: 513
		internal const string DataRow_AlreadyInTheCollection = "DataRow_AlreadyInTheCollection";

		// Token: 0x04000202 RID: 514
		internal const string DataRow_AlreadyDeleted = "DataRow_AlreadyDeleted";

		// Token: 0x04000203 RID: 515
		internal const string DataRow_Empty = "DataRow_Empty";

		// Token: 0x04000204 RID: 516
		internal const string DataRow_AlreadyRemoved = "DataRow_AlreadyRemoved";

		// Token: 0x04000205 RID: 517
		internal const string DataRow_MultipleParents = "DataRow_MultipleParents";

		// Token: 0x04000206 RID: 518
		internal const string DataRow_InvalidRowBitPattern = "DataRow_InvalidRowBitPattern";

		// Token: 0x04000207 RID: 519
		internal const string DataSet_SetNameToEmpty = "DataSet_SetNameToEmpty";

		// Token: 0x04000208 RID: 520
		internal const string DataSet_SetDataSetNameConflicting = "DataSet_SetDataSetNameConflicting";

		// Token: 0x04000209 RID: 521
		internal const string DataSet_UnsupportedSchema = "DataSet_UnsupportedSchema";

		// Token: 0x0400020A RID: 522
		internal const string DataSet_CannotChangeCaseLocale = "DataSet_CannotChangeCaseLocale";

		// Token: 0x0400020B RID: 523
		internal const string DataSet_CannotChangeSchemaSerializationMode = "DataSet_CannotChangeSchemaSerializationMode";

		// Token: 0x0400020C RID: 524
		internal const string DataTable_ForeignPrimaryKey = "DataTable_ForeignPrimaryKey";

		// Token: 0x0400020D RID: 525
		internal const string DataTable_CannotAddToSimpleContent = "DataTable_CannotAddToSimpleContent";

		// Token: 0x0400020E RID: 526
		internal const string DataTable_NoName = "DataTable_NoName";

		// Token: 0x0400020F RID: 527
		internal const string DataTable_MultipleSimpleContentColumns = "DataTable_MultipleSimpleContentColumns";

		// Token: 0x04000210 RID: 528
		internal const string DataTable_MissingPrimaryKey = "DataTable_MissingPrimaryKey";

		// Token: 0x04000211 RID: 529
		internal const string DataTable_InvalidSortString = "DataTable_InvalidSortString";

		// Token: 0x04000212 RID: 530
		internal const string DataTable_CanNotSerializeDataTableHierarchy = "DataTable_CanNotSerializeDataTableHierarchy";

		// Token: 0x04000213 RID: 531
		internal const string DataTable_CanNotRemoteDataTable = "DataTable_CanNotRemoteDataTable";

		// Token: 0x04000214 RID: 532
		internal const string DataTable_CanNotSetRemotingFormat = "DataTable_CanNotSetRemotingFormat";

		// Token: 0x04000215 RID: 533
		internal const string DataTable_CanNotSerializeDataTableWithEmptyName = "DataTable_CanNotSerializeDataTableWithEmptyName";

		// Token: 0x04000216 RID: 534
		internal const string DataTable_DuplicateName = "DataTable_DuplicateName";

		// Token: 0x04000217 RID: 535
		internal const string DataTable_DuplicateName2 = "DataTable_DuplicateName2";

		// Token: 0x04000218 RID: 536
		internal const string DataTable_SelfnestedDatasetConflictingName = "DataTable_SelfnestedDatasetConflictingName";

		// Token: 0x04000219 RID: 537
		internal const string DataTable_DatasetConflictingName = "DataTable_DatasetConflictingName";

		// Token: 0x0400021A RID: 538
		internal const string DataTable_AlreadyInOtherDataSet = "DataTable_AlreadyInOtherDataSet";

		// Token: 0x0400021B RID: 539
		internal const string DataTable_AlreadyInTheDataSet = "DataTable_AlreadyInTheDataSet";

		// Token: 0x0400021C RID: 540
		internal const string DataTable_NotInTheDataSet = "DataTable_NotInTheDataSet";

		// Token: 0x0400021D RID: 541
		internal const string DataTable_OutOfRange = "DataTable_OutOfRange";

		// Token: 0x0400021E RID: 542
		internal const string DataTable_InRelation = "DataTable_InRelation";

		// Token: 0x0400021F RID: 543
		internal const string DataTable_InConstraint = "DataTable_InConstraint";

		// Token: 0x04000220 RID: 544
		internal const string DataTable_TableNotFound = "DataTable_TableNotFound";

		// Token: 0x04000221 RID: 545
		internal const string DataMerge_MissingDefinition = "DataMerge_MissingDefinition";

		// Token: 0x04000222 RID: 546
		internal const string DataMerge_MissingConstraint = "DataMerge_MissingConstraint";

		// Token: 0x04000223 RID: 547
		internal const string DataMerge_DataTypeMismatch = "DataMerge_DataTypeMismatch";

		// Token: 0x04000224 RID: 548
		internal const string DataMerge_PrimaryKeyMismatch = "DataMerge_PrimaryKeyMismatch";

		// Token: 0x04000225 RID: 549
		internal const string DataMerge_PrimaryKeyColumnsMismatch = "DataMerge_PrimaryKeyColumnsMismatch";

		// Token: 0x04000226 RID: 550
		internal const string DataMerge_ReltionKeyColumnsMismatch = "DataMerge_ReltionKeyColumnsMismatch";

		// Token: 0x04000227 RID: 551
		internal const string DataMerge_MissingColumnDefinition = "DataMerge_MissingColumnDefinition";

		// Token: 0x04000228 RID: 552
		internal const string DataMerge_MissingPrimaryKeyColumnInSource = "DataMerge_MissingPrimaryKeyColumnInSource";

		// Token: 0x04000229 RID: 553
		internal const string DataIndex_RecordStateRange = "DataIndex_RecordStateRange";

		// Token: 0x0400022A RID: 554
		internal const string DataIndex_FindWithoutSortOrder = "DataIndex_FindWithoutSortOrder";

		// Token: 0x0400022B RID: 555
		internal const string DataIndex_KeyLength = "DataIndex_KeyLength";

		// Token: 0x0400022C RID: 556
		internal const string DataStorage_AggregateException = "DataStorage_AggregateException";

		// Token: 0x0400022D RID: 557
		internal const string DataStorage_InvalidStorageType = "DataStorage_InvalidStorageType";

		// Token: 0x0400022E RID: 558
		internal const string DataStorage_ProblematicChars = "DataStorage_ProblematicChars";

		// Token: 0x0400022F RID: 559
		internal const string DataStorage_SetInvalidDataType = "DataStorage_SetInvalidDataType";

		// Token: 0x04000230 RID: 560
		internal const string DataStorage_IComparableNotDefined = "DataStorage_IComparableNotDefined";

		// Token: 0x04000231 RID: 561
		internal const string DataView_SetFailed = "DataView_SetFailed";

		// Token: 0x04000232 RID: 562
		internal const string DataView_SetDataSetFailed = "DataView_SetDataSetFailed";

		// Token: 0x04000233 RID: 563
		internal const string DataView_SetRowStateFilter = "DataView_SetRowStateFilter";

		// Token: 0x04000234 RID: 564
		internal const string DataView_SetTable = "DataView_SetTable";

		// Token: 0x04000235 RID: 565
		internal const string DataView_CanNotSetDataSet = "DataView_CanNotSetDataSet";

		// Token: 0x04000236 RID: 566
		internal const string DataView_CanNotUseDataViewManager = "DataView_CanNotUseDataViewManager";

		// Token: 0x04000237 RID: 567
		internal const string DataView_CanNotSetTable = "DataView_CanNotSetTable";

		// Token: 0x04000238 RID: 568
		internal const string DataView_CanNotUse = "DataView_CanNotUse";

		// Token: 0x04000239 RID: 569
		internal const string DataView_CanNotBindTable = "DataView_CanNotBindTable";

		// Token: 0x0400023A RID: 570
		internal const string DataView_SetIListObject = "DataView_SetIListObject";

		// Token: 0x0400023B RID: 571
		internal const string DataView_AddNewNotAllowNull = "DataView_AddNewNotAllowNull";

		// Token: 0x0400023C RID: 572
		internal const string DataView_NotOpen = "DataView_NotOpen";

		// Token: 0x0400023D RID: 573
		internal const string DataView_CreateChildView = "DataView_CreateChildView";

		// Token: 0x0400023E RID: 574
		internal const string DataView_CanNotDelete = "DataView_CanNotDelete";

		// Token: 0x0400023F RID: 575
		internal const string DataView_CanNotEdit = "DataView_CanNotEdit";

		// Token: 0x04000240 RID: 576
		internal const string DataView_GetElementIndex = "DataView_GetElementIndex";

		// Token: 0x04000241 RID: 577
		internal const string DataView_AddExternalObject = "DataView_AddExternalObject";

		// Token: 0x04000242 RID: 578
		internal const string DataView_CanNotClear = "DataView_CanNotClear";

		// Token: 0x04000243 RID: 579
		internal const string DataView_InsertExternalObject = "DataView_InsertExternalObject";

		// Token: 0x04000244 RID: 580
		internal const string DataView_RemoveExternalObject = "DataView_RemoveExternalObject";

		// Token: 0x04000245 RID: 581
		internal const string DataROWView_PropertyNotFound = "DataROWView_PropertyNotFound";

		// Token: 0x04000246 RID: 582
		internal const string Range_Argument = "Range_Argument";

		// Token: 0x04000247 RID: 583
		internal const string Range_NullRange = "Range_NullRange";

		// Token: 0x04000248 RID: 584
		internal const string RecordManager_MinimumCapacity = "RecordManager_MinimumCapacity";

		// Token: 0x04000249 RID: 585
		internal const string CodeGen_InvalidIdentifier = "CodeGen_InvalidIdentifier";

		// Token: 0x0400024A RID: 586
		internal const string CodeGen_DuplicateTableName = "CodeGen_DuplicateTableName";

		// Token: 0x0400024B RID: 587
		internal const string CodeGen_TypeCantBeNull = "CodeGen_TypeCantBeNull";

		// Token: 0x0400024C RID: 588
		internal const string CodeGen_NoCtor0 = "CodeGen_NoCtor0";

		// Token: 0x0400024D RID: 589
		internal const string CodeGen_NoCtor1 = "CodeGen_NoCtor1";

		// Token: 0x0400024E RID: 590
		internal const string SqlConvert_ConvertFailed = "SqlConvert_ConvertFailed";

		// Token: 0x0400024F RID: 591
		internal const string DataSet_DefaultDataException = "DataSet_DefaultDataException";

		// Token: 0x04000250 RID: 592
		internal const string DataSet_DefaultConstraintException = "DataSet_DefaultConstraintException";

		// Token: 0x04000251 RID: 593
		internal const string DataSet_DefaultDeletedRowInaccessibleException = "DataSet_DefaultDeletedRowInaccessibleException";

		// Token: 0x04000252 RID: 594
		internal const string DataSet_DefaultDuplicateNameException = "DataSet_DefaultDuplicateNameException";

		// Token: 0x04000253 RID: 595
		internal const string DataSet_DefaultInRowChangingEventException = "DataSet_DefaultInRowChangingEventException";

		// Token: 0x04000254 RID: 596
		internal const string DataSet_DefaultInvalidConstraintException = "DataSet_DefaultInvalidConstraintException";

		// Token: 0x04000255 RID: 597
		internal const string DataSet_DefaultMissingPrimaryKeyException = "DataSet_DefaultMissingPrimaryKeyException";

		// Token: 0x04000256 RID: 598
		internal const string DataSet_DefaultNoNullAllowedException = "DataSet_DefaultNoNullAllowedException";

		// Token: 0x04000257 RID: 599
		internal const string DataSet_DefaultReadOnlyException = "DataSet_DefaultReadOnlyException";

		// Token: 0x04000258 RID: 600
		internal const string DataSet_DefaultRowNotInTableException = "DataSet_DefaultRowNotInTableException";

		// Token: 0x04000259 RID: 601
		internal const string DataSet_DefaultVersionNotFoundException = "DataSet_DefaultVersionNotFoundException";

		// Token: 0x0400025A RID: 602
		internal const string Load_ReadOnlyDataModified = "Load_ReadOnlyDataModified";

		// Token: 0x0400025B RID: 603
		internal const string DataTableReader_InvalidDataTableReader = "DataTableReader_InvalidDataTableReader";

		// Token: 0x0400025C RID: 604
		internal const string DataTableReader_SchemaInvalidDataTableReader = "DataTableReader_SchemaInvalidDataTableReader";

		// Token: 0x0400025D RID: 605
		internal const string DataTableReader_CannotCreateDataReaderOnEmptyDataSet = "DataTableReader_CannotCreateDataReaderOnEmptyDataSet";

		// Token: 0x0400025E RID: 606
		internal const string DataTableReader_DataTableReaderArgumentIsEmpty = "DataTableReader_DataTableReaderArgumentIsEmpty";

		// Token: 0x0400025F RID: 607
		internal const string DataTableReader_ArgumentContainsNullValue = "DataTableReader_ArgumentContainsNullValue";

		// Token: 0x04000260 RID: 608
		internal const string DataTableReader_InvalidRowInDataTableReader = "DataTableReader_InvalidRowInDataTableReader";

		// Token: 0x04000261 RID: 609
		internal const string DataTableReader_DataTableCleared = "DataTableReader_DataTableCleared";

		// Token: 0x04000262 RID: 610
		internal const string RbTree_InvalidState = "RbTree_InvalidState";

		// Token: 0x04000263 RID: 611
		internal const string RbTree_EnumerationBroken = "RbTree_EnumerationBroken";

		// Token: 0x04000264 RID: 612
		internal const string NamedSimpleType_InvalidDuplicateNamedSimpleTypeDelaration = "NamedSimpleType_InvalidDuplicateNamedSimpleTypeDelaration";

		// Token: 0x04000265 RID: 613
		internal const string DataDom_Foliation = "DataDom_Foliation";

		// Token: 0x04000266 RID: 614
		internal const string DataDom_TableNameChange = "DataDom_TableNameChange";

		// Token: 0x04000267 RID: 615
		internal const string DataDom_TableNamespaceChange = "DataDom_TableNamespaceChange";

		// Token: 0x04000268 RID: 616
		internal const string DataDom_ColumnNameChange = "DataDom_ColumnNameChange";

		// Token: 0x04000269 RID: 617
		internal const string DataDom_ColumnNamespaceChange = "DataDom_ColumnNamespaceChange";

		// Token: 0x0400026A RID: 618
		internal const string DataDom_ColumnMappingChange = "DataDom_ColumnMappingChange";

		// Token: 0x0400026B RID: 619
		internal const string DataDom_TableColumnsChange = "DataDom_TableColumnsChange";

		// Token: 0x0400026C RID: 620
		internal const string DataDom_DataSetTablesChange = "DataDom_DataSetTablesChange";

		// Token: 0x0400026D RID: 621
		internal const string DataDom_DataSetNestedRelationsChange = "DataDom_DataSetNestedRelationsChange";

		// Token: 0x0400026E RID: 622
		internal const string DataDom_DataSetNull = "DataDom_DataSetNull";

		// Token: 0x0400026F RID: 623
		internal const string DataDom_DataSetNameChange = "DataDom_DataSetNameChange";

		// Token: 0x04000270 RID: 624
		internal const string DataDom_CloneNode = "DataDom_CloneNode";

		// Token: 0x04000271 RID: 625
		internal const string DataDom_MultipleLoad = "DataDom_MultipleLoad";

		// Token: 0x04000272 RID: 626
		internal const string DataDom_MultipleDataSet = "DataDom_MultipleDataSet";

		// Token: 0x04000273 RID: 627
		internal const string DataDom_EnforceConstraintsShouldBeOff = "DataDom_EnforceConstraintsShouldBeOff";

		// Token: 0x04000274 RID: 628
		internal const string DataDom_NotSupport_GetElementById = "DataDom_NotSupport_GetElementById";

		// Token: 0x04000275 RID: 629
		internal const string DataDom_NotSupport_EntRef = "DataDom_NotSupport_EntRef";

		// Token: 0x04000276 RID: 630
		internal const string DataDom_NotSupport_Clear = "DataDom_NotSupport_Clear";

		// Token: 0x04000277 RID: 631
		internal const string StrongTyping_CannotRemoveColumn = "StrongTyping_CannotRemoveColumn";

		// Token: 0x04000278 RID: 632
		internal const string StrongTyping_CananotRemoveRelation = "StrongTyping_CananotRemoveRelation";

		// Token: 0x04000279 RID: 633
		internal const string propertyChangedEventDescr = "propertyChangedEventDescr";

		// Token: 0x0400027A RID: 634
		internal const string collectionChangedEventDescr = "collectionChangedEventDescr";

		// Token: 0x0400027B RID: 635
		internal const string StrongTyping_CananotAccessDBNull = "StrongTyping_CananotAccessDBNull";

		// Token: 0x0400027C RID: 636
		internal const string ADP_PropertyNotSupported = "ADP_PropertyNotSupported";

		// Token: 0x0400027D RID: 637
		internal const string ConfigProviderNotFound = "ConfigProviderNotFound";

		// Token: 0x0400027E RID: 638
		internal const string ConfigProviderInvalid = "ConfigProviderInvalid";

		// Token: 0x0400027F RID: 639
		internal const string ConfigProviderNotInstalled = "ConfigProviderNotInstalled";

		// Token: 0x04000280 RID: 640
		internal const string ConfigProviderMissing = "ConfigProviderMissing";

		// Token: 0x04000281 RID: 641
		internal const string ConfigBaseElementsOnly = "ConfigBaseElementsOnly";

		// Token: 0x04000282 RID: 642
		internal const string ConfigBaseNoChildNodes = "ConfigBaseNoChildNodes";

		// Token: 0x04000283 RID: 643
		internal const string ConfigUnrecognizedAttributes = "ConfigUnrecognizedAttributes";

		// Token: 0x04000284 RID: 644
		internal const string ConfigUnrecognizedElement = "ConfigUnrecognizedElement";

		// Token: 0x04000285 RID: 645
		internal const string ConfigSectionsUnique = "ConfigSectionsUnique";

		// Token: 0x04000286 RID: 646
		internal const string ConfigRequiredAttributeMissing = "ConfigRequiredAttributeMissing";

		// Token: 0x04000287 RID: 647
		internal const string ConfigRequiredAttributeEmpty = "ConfigRequiredAttributeEmpty";

		// Token: 0x04000288 RID: 648
		internal const string ADP_EmptyArray = "ADP_EmptyArray";

		// Token: 0x04000289 RID: 649
		internal const string ADP_SingleValuedProperty = "ADP_SingleValuedProperty";

		// Token: 0x0400028A RID: 650
		internal const string ADP_DoubleValuedProperty = "ADP_DoubleValuedProperty";

		// Token: 0x0400028B RID: 651
		internal const string ADP_InvalidPrefixSuffix = "ADP_InvalidPrefixSuffix";

		// Token: 0x0400028C RID: 652
		internal const string ADP_InvalidArgumentLength = "ADP_InvalidArgumentLength";

		// Token: 0x0400028D RID: 653
		internal const string SQL_WrongType = "SQL_WrongType";

		// Token: 0x0400028E RID: 654
		internal const string ADP_InvalidConnectionOptionValue = "ADP_InvalidConnectionOptionValue";

		// Token: 0x0400028F RID: 655
		internal const string ADP_MissingConnectionOptionValue = "ADP_MissingConnectionOptionValue";

		// Token: 0x04000290 RID: 656
		internal const string ADP_InvalidConnectionOptionValueLength = "ADP_InvalidConnectionOptionValueLength";

		// Token: 0x04000291 RID: 657
		internal const string ADP_KeywordNotSupported = "ADP_KeywordNotSupported";

		// Token: 0x04000292 RID: 658
		internal const string ADP_UdlFileError = "ADP_UdlFileError";

		// Token: 0x04000293 RID: 659
		internal const string ADP_InvalidUDL = "ADP_InvalidUDL";

		// Token: 0x04000294 RID: 660
		internal const string ADP_InternalProviderError = "ADP_InternalProviderError";

		// Token: 0x04000295 RID: 661
		internal const string ADP_NoQuoteChange = "ADP_NoQuoteChange";

		// Token: 0x04000296 RID: 662
		internal const string ADP_MissingSourceCommand = "ADP_MissingSourceCommand";

		// Token: 0x04000297 RID: 663
		internal const string ADP_MissingSourceCommandConnection = "ADP_MissingSourceCommandConnection";

		// Token: 0x04000298 RID: 664
		internal const string ADP_InvalidMultipartName = "ADP_InvalidMultipartName";

		// Token: 0x04000299 RID: 665
		internal const string ADP_InvalidMultipartNameQuoteUsage = "ADP_InvalidMultipartNameQuoteUsage";

		// Token: 0x0400029A RID: 666
		internal const string ADP_InvalidMultipartNameToManyParts = "ADP_InvalidMultipartNameToManyParts";

		// Token: 0x0400029B RID: 667
		internal const string SQL_BulkCopyDestinationTableName = "SQL_BulkCopyDestinationTableName";

		// Token: 0x0400029C RID: 668
		internal const string SQL_TDSParserTableName = "SQL_TDSParserTableName";

		// Token: 0x0400029D RID: 669
		internal const string SQL_UDTTypeName = "SQL_UDTTypeName";

		// Token: 0x0400029E RID: 670
		internal const string SQL_TypeName = "SQL_TypeName";

		// Token: 0x0400029F RID: 671
		internal const string SQL_SqlCommandCommandText = "SQL_SqlCommandCommandText";

		// Token: 0x040002A0 RID: 672
		internal const string ODBC_ODBCCommandText = "ODBC_ODBCCommandText";

		// Token: 0x040002A1 RID: 673
		internal const string OLEDB_OLEDBCommandText = "OLEDB_OLEDBCommandText";

		// Token: 0x040002A2 RID: 674
		internal const string SQLMSF_FailoverPartnerNotSupported = "SQLMSF_FailoverPartnerNotSupported";

		// Token: 0x040002A3 RID: 675
		internal const string ADP_ColumnSchemaExpression = "ADP_ColumnSchemaExpression";

		// Token: 0x040002A4 RID: 676
		internal const string ADP_ColumnSchemaMismatch = "ADP_ColumnSchemaMismatch";

		// Token: 0x040002A5 RID: 677
		internal const string ADP_ColumnSchemaMissing1 = "ADP_ColumnSchemaMissing1";

		// Token: 0x040002A6 RID: 678
		internal const string ADP_ColumnSchemaMissing2 = "ADP_ColumnSchemaMissing2";

		// Token: 0x040002A7 RID: 679
		internal const string ADP_InvalidSourceColumn = "ADP_InvalidSourceColumn";

		// Token: 0x040002A8 RID: 680
		internal const string ADP_MissingColumnMapping = "ADP_MissingColumnMapping";

		// Token: 0x040002A9 RID: 681
		internal const string ADP_NotSupportedEnumerationValue = "ADP_NotSupportedEnumerationValue";

		// Token: 0x040002AA RID: 682
		internal const string ODBC_NotSupportedEnumerationValue = "ODBC_NotSupportedEnumerationValue";

		// Token: 0x040002AB RID: 683
		internal const string OLEDB_NotSupportedEnumerationValue = "OLEDB_NotSupportedEnumerationValue";

		// Token: 0x040002AC RID: 684
		internal const string SQL_NotSupportedEnumerationValue = "SQL_NotSupportedEnumerationValue";

		// Token: 0x040002AD RID: 685
		internal const string ADP_ComputerNameEx = "ADP_ComputerNameEx";

		// Token: 0x040002AE RID: 686
		internal const string ADP_MissingTableSchema = "ADP_MissingTableSchema";

		// Token: 0x040002AF RID: 687
		internal const string ADP_InvalidSourceTable = "ADP_InvalidSourceTable";

		// Token: 0x040002B0 RID: 688
		internal const string ADP_MissingTableMapping = "ADP_MissingTableMapping";

		// Token: 0x040002B1 RID: 689
		internal const string ADP_CommandTextRequired = "ADP_CommandTextRequired";

		// Token: 0x040002B2 RID: 690
		internal const string ADP_ConnectionRequired = "ADP_ConnectionRequired";

		// Token: 0x040002B3 RID: 691
		internal const string ADP_OpenConnectionRequired = "ADP_OpenConnectionRequired";

		// Token: 0x040002B4 RID: 692
		internal const string ADP_ConnectionRequired_Fill = "ADP_ConnectionRequired_Fill";

		// Token: 0x040002B5 RID: 693
		internal const string ADP_ConnectionRequired_FillPage = "ADP_ConnectionRequired_FillPage";

		// Token: 0x040002B6 RID: 694
		internal const string ADP_ConnectionRequired_FillSchema = "ADP_ConnectionRequired_FillSchema";

		// Token: 0x040002B7 RID: 695
		internal const string ADP_ConnectionRequired_Insert = "ADP_ConnectionRequired_Insert";

		// Token: 0x040002B8 RID: 696
		internal const string ADP_ConnectionRequired_Update = "ADP_ConnectionRequired_Update";

		// Token: 0x040002B9 RID: 697
		internal const string ADP_ConnectionRequired_Delete = "ADP_ConnectionRequired_Delete";

		// Token: 0x040002BA RID: 698
		internal const string ADP_ConnectionRequired_Batch = "ADP_ConnectionRequired_Batch";

		// Token: 0x040002BB RID: 699
		internal const string ADP_ConnectionRequired_Clone = "ADP_ConnectionRequired_Clone";

		// Token: 0x040002BC RID: 700
		internal const string ADP_ConnecitonRequired_UpdateRows = "ADP_ConnecitonRequired_UpdateRows";

		// Token: 0x040002BD RID: 701
		internal const string ADP_OpenConnectionRequired_Insert = "ADP_OpenConnectionRequired_Insert";

		// Token: 0x040002BE RID: 702
		internal const string ADP_OpenConnectionRequired_Update = "ADP_OpenConnectionRequired_Update";

		// Token: 0x040002BF RID: 703
		internal const string ADP_OpenConnectionRequired_Delete = "ADP_OpenConnectionRequired_Delete";

		// Token: 0x040002C0 RID: 704
		internal const string ADP_OpenConnectionRequired_Clone = "ADP_OpenConnectionRequired_Clone";

		// Token: 0x040002C1 RID: 705
		internal const string ADP_NoStoredProcedureExists = "ADP_NoStoredProcedureExists";

		// Token: 0x040002C2 RID: 706
		internal const string ADP_TransactionCompleted = "ADP_TransactionCompleted";

		// Token: 0x040002C3 RID: 707
		internal const string ADP_TransactionConnectionMismatch = "ADP_TransactionConnectionMismatch";

		// Token: 0x040002C4 RID: 708
		internal const string ADP_TransactionRequired = "ADP_TransactionRequired";

		// Token: 0x040002C5 RID: 709
		internal const string ADP_OpenResultSetExists = "ADP_OpenResultSetExists";

		// Token: 0x040002C6 RID: 710
		internal const string ADP_OpenReaderExists = "ADP_OpenReaderExists";

		// Token: 0x040002C7 RID: 711
		internal const string ADP_DeriveParametersNotSupported = "ADP_DeriveParametersNotSupported";

		// Token: 0x040002C8 RID: 712
		internal const string ADP_CalledTwice = "ADP_CalledTwice";

		// Token: 0x040002C9 RID: 713
		internal const string ADP_IncorrectAsyncResult = "ADP_IncorrectAsyncResult";

		// Token: 0x040002CA RID: 714
		internal const string ADP_MissingSelectCommand = "ADP_MissingSelectCommand";

		// Token: 0x040002CB RID: 715
		internal const string ADP_UnwantedStatementType = "ADP_UnwantedStatementType";

		// Token: 0x040002CC RID: 716
		internal const string ADP_FillSchemaRequiresSourceTableName = "ADP_FillSchemaRequiresSourceTableName";

		// Token: 0x040002CD RID: 717
		internal const string ADP_InvalidMaxRecords = "ADP_InvalidMaxRecords";

		// Token: 0x040002CE RID: 718
		internal const string ADP_InvalidStartRecord = "ADP_InvalidStartRecord";

		// Token: 0x040002CF RID: 719
		internal const string ADP_FillRequiresSourceTableName = "ADP_FillRequiresSourceTableName";

		// Token: 0x040002D0 RID: 720
		internal const string ADP_FillChapterAutoIncrement = "ADP_FillChapterAutoIncrement";

		// Token: 0x040002D1 RID: 721
		internal const string ADP_MissingDataReaderFieldType = "ADP_MissingDataReaderFieldType";

		// Token: 0x040002D2 RID: 722
		internal const string ADP_OnlyOneTableForStartRecordOrMaxRecords = "ADP_OnlyOneTableForStartRecordOrMaxRecords";

		// Token: 0x040002D3 RID: 723
		internal const string ADP_UpdateRequiresSourceTable = "ADP_UpdateRequiresSourceTable";

		// Token: 0x040002D4 RID: 724
		internal const string ADP_UpdateRequiresSourceTableName = "ADP_UpdateRequiresSourceTableName";

		// Token: 0x040002D5 RID: 725
		internal const string ADP_MissingTableMappingDestination = "ADP_MissingTableMappingDestination";

		// Token: 0x040002D6 RID: 726
		internal const string ADP_UpdateRequiresCommandClone = "ADP_UpdateRequiresCommandClone";

		// Token: 0x040002D7 RID: 727
		internal const string ADP_UpdateRequiresCommandSelect = "ADP_UpdateRequiresCommandSelect";

		// Token: 0x040002D8 RID: 728
		internal const string ADP_UpdateRequiresCommandInsert = "ADP_UpdateRequiresCommandInsert";

		// Token: 0x040002D9 RID: 729
		internal const string ADP_UpdateRequiresCommandUpdate = "ADP_UpdateRequiresCommandUpdate";

		// Token: 0x040002DA RID: 730
		internal const string ADP_UpdateRequiresCommandDelete = "ADP_UpdateRequiresCommandDelete";

		// Token: 0x040002DB RID: 731
		internal const string ADP_UpdateMismatchRowTable = "ADP_UpdateMismatchRowTable";

		// Token: 0x040002DC RID: 732
		internal const string ADP_RowUpdatedErrors = "ADP_RowUpdatedErrors";

		// Token: 0x040002DD RID: 733
		internal const string ADP_RowUpdatingErrors = "ADP_RowUpdatingErrors";

		// Token: 0x040002DE RID: 734
		internal const string ADP_ResultsNotAllowedDuringBatch = "ADP_ResultsNotAllowedDuringBatch";

		// Token: 0x040002DF RID: 735
		internal const string ADP_UpdateConcurrencyViolation_Update = "ADP_UpdateConcurrencyViolation_Update";

		// Token: 0x040002E0 RID: 736
		internal const string ADP_UpdateConcurrencyViolation_Delete = "ADP_UpdateConcurrencyViolation_Delete";

		// Token: 0x040002E1 RID: 737
		internal const string ADP_UpdateConcurrencyViolation_Batch = "ADP_UpdateConcurrencyViolation_Batch";

		// Token: 0x040002E2 RID: 738
		internal const string ADP_InvalidCommandTimeout = "ADP_InvalidCommandTimeout";

		// Token: 0x040002E3 RID: 739
		internal const string ADP_UninitializedParameterSize = "ADP_UninitializedParameterSize";

		// Token: 0x040002E4 RID: 740
		internal const string ADP_PrepareParameterType = "ADP_PrepareParameterType";

		// Token: 0x040002E5 RID: 741
		internal const string ADP_PrepareParameterSize = "ADP_PrepareParameterSize";

		// Token: 0x040002E6 RID: 742
		internal const string ADP_PrepareParameterScale = "ADP_PrepareParameterScale";

		// Token: 0x040002E7 RID: 743
		internal const string ADP_MismatchedAsyncResult = "ADP_MismatchedAsyncResult";

		// Token: 0x040002E8 RID: 744
		internal const string ADP_ClosedConnectionError = "ADP_ClosedConnectionError";

		// Token: 0x040002E9 RID: 745
		internal const string ADP_ConnectionIsDisabled = "ADP_ConnectionIsDisabled";

		// Token: 0x040002EA RID: 746
		internal const string ADP_LocalTransactionPresent = "ADP_LocalTransactionPresent";

		// Token: 0x040002EB RID: 747
		internal const string ADP_TransactionPresent = "ADP_TransactionPresent";

		// Token: 0x040002EC RID: 748
		internal const string ADP_EmptyDatabaseName = "ADP_EmptyDatabaseName";

		// Token: 0x040002ED RID: 749
		internal const string ADP_DatabaseNameTooLong = "ADP_DatabaseNameTooLong";

		// Token: 0x040002EE RID: 750
		internal const string ADP_InvalidConnectTimeoutValue = "ADP_InvalidConnectTimeoutValue";

		// Token: 0x040002EF RID: 751
		internal const string ADP_InvalidSourceBufferIndex = "ADP_InvalidSourceBufferIndex";

		// Token: 0x040002F0 RID: 752
		internal const string ADP_InvalidDestinationBufferIndex = "ADP_InvalidDestinationBufferIndex";

		// Token: 0x040002F1 RID: 753
		internal const string ADP_DataReaderNoData = "ADP_DataReaderNoData";

		// Token: 0x040002F2 RID: 754
		internal const string ADP_NumericToDecimalOverflow = "ADP_NumericToDecimalOverflow";

		// Token: 0x040002F3 RID: 755
		internal const string ADP_StreamClosed = "ADP_StreamClosed";

		// Token: 0x040002F4 RID: 756
		internal const string ADP_InvalidSeekOrigin = "ADP_InvalidSeekOrigin";

		// Token: 0x040002F5 RID: 757
		internal const string ADP_DynamicSQLJoinUnsupported = "ADP_DynamicSQLJoinUnsupported";

		// Token: 0x040002F6 RID: 758
		internal const string ADP_DynamicSQLNoTableInfo = "ADP_DynamicSQLNoTableInfo";

		// Token: 0x040002F7 RID: 759
		internal const string ADP_DynamicSQLNoKeyInfoDelete = "ADP_DynamicSQLNoKeyInfoDelete";

		// Token: 0x040002F8 RID: 760
		internal const string ADP_DynamicSQLNoKeyInfoUpdate = "ADP_DynamicSQLNoKeyInfoUpdate";

		// Token: 0x040002F9 RID: 761
		internal const string ADP_DynamicSQLNoKeyInfoRowVersionDelete = "ADP_DynamicSQLNoKeyInfoRowVersionDelete";

		// Token: 0x040002FA RID: 762
		internal const string ADP_DynamicSQLNoKeyInfoRowVersionUpdate = "ADP_DynamicSQLNoKeyInfoRowVersionUpdate";

		// Token: 0x040002FB RID: 763
		internal const string ADP_DynamicSQLNestedQuote = "ADP_DynamicSQLNestedQuote";

		// Token: 0x040002FC RID: 764
		internal const string ADP_NonSequentialColumnAccess = "ADP_NonSequentialColumnAccess";

		// Token: 0x040002FD RID: 765
		internal const string ADP_InvalidDateTimeDigits = "ADP_InvalidDateTimeDigits";

		// Token: 0x040002FE RID: 766
		internal const string ADP_InvalidFormatValue = "ADP_InvalidFormatValue";

		// Token: 0x040002FF RID: 767
		internal const string ADP_InvalidMaximumScale = "ADP_InvalidMaximumScale";

		// Token: 0x04000300 RID: 768
		internal const string ADP_LiteralValueIsInvalid = "ADP_LiteralValueIsInvalid";

		// Token: 0x04000301 RID: 769
		internal const string ADP_EvenLengthLiteralValue = "ADP_EvenLengthLiteralValue";

		// Token: 0x04000302 RID: 770
		internal const string ADP_HexDigitLiteralValue = "ADP_HexDigitLiteralValue";

		// Token: 0x04000303 RID: 771
		internal const string ADP_QuotePrefixNotSet = "ADP_QuotePrefixNotSet";

		// Token: 0x04000304 RID: 772
		internal const string ADP_UnableToCreateBooleanLiteral = "ADP_UnableToCreateBooleanLiteral";

		// Token: 0x04000305 RID: 773
		internal const string ADP_UnsupportedNativeDataTypeOleDb = "ADP_UnsupportedNativeDataTypeOleDb";

		// Token: 0x04000306 RID: 774
		internal const string ADP_InvalidDataType = "ADP_InvalidDataType";

		// Token: 0x04000307 RID: 775
		internal const string ADP_UnknownDataType = "ADP_UnknownDataType";

		// Token: 0x04000308 RID: 776
		internal const string ADP_UnknownDataTypeCode = "ADP_UnknownDataTypeCode";

		// Token: 0x04000309 RID: 777
		internal const string ADP_DbTypeNotSupported = "ADP_DbTypeNotSupported";

		// Token: 0x0400030A RID: 778
		internal const string ADP_VersionDoesNotSupportDataType = "ADP_VersionDoesNotSupportDataType";

		// Token: 0x0400030B RID: 779
		internal const string ADP_ParameterValueOutOfRange = "ADP_ParameterValueOutOfRange";

		// Token: 0x0400030C RID: 780
		internal const string ADP_BadParameterName = "ADP_BadParameterName";

		// Token: 0x0400030D RID: 781
		internal const string ADP_MultipleReturnValue = "ADP_MultipleReturnValue";

		// Token: 0x0400030E RID: 782
		internal const string ADP_InvalidSizeValue = "ADP_InvalidSizeValue";

		// Token: 0x0400030F RID: 783
		internal const string ADP_NegativeParameter = "ADP_NegativeParameter";

		// Token: 0x04000310 RID: 784
		internal const string ADP_InvalidMetaDataValue = "ADP_InvalidMetaDataValue";

		// Token: 0x04000311 RID: 785
		internal const string ADP_NotRowType = "ADP_NotRowType";

		// Token: 0x04000312 RID: 786
		internal const string ADP_ParameterConversionFailed = "ADP_ParameterConversionFailed";

		// Token: 0x04000313 RID: 787
		internal const string ADP_ParallelTransactionsNotSupported = "ADP_ParallelTransactionsNotSupported";

		// Token: 0x04000314 RID: 788
		internal const string ADP_TransactionZombied = "ADP_TransactionZombied";

		// Token: 0x04000315 RID: 789
		internal const string ADP_DbRecordReadOnly = "ADP_DbRecordReadOnly";

		// Token: 0x04000316 RID: 790
		internal const string ADP_DbDataUpdatableRecordReadOnly = "ADP_DbDataUpdatableRecordReadOnly";

		// Token: 0x04000317 RID: 791
		internal const string ADP_InvalidImplicitConversion = "ADP_InvalidImplicitConversion";

		// Token: 0x04000318 RID: 792
		internal const string ADP_InvalidBufferSizeOrIndex = "ADP_InvalidBufferSizeOrIndex";

		// Token: 0x04000319 RID: 793
		internal const string ADP_InvalidDataLength = "ADP_InvalidDataLength";

		// Token: 0x0400031A RID: 794
		internal const string ADP_InvalidDataLength2 = "ADP_InvalidDataLength2";

		// Token: 0x0400031B RID: 795
		internal const string ADP_NonSeqByteAccess = "ADP_NonSeqByteAccess";

		// Token: 0x0400031C RID: 796
		internal const string ADP_OffsetOutOfRangeException = "ADP_OffsetOutOfRangeException";

		// Token: 0x0400031D RID: 797
		internal const string ODBC_GetSchemaRestrictionRequired = "ODBC_GetSchemaRestrictionRequired";

		// Token: 0x0400031E RID: 798
		internal const string ADP_InvalidArgumentValue = "ADP_InvalidArgumentValue";

		// Token: 0x0400031F RID: 799
		internal const string ADP_NullDataTable = "ADP_NullDataTable";

		// Token: 0x04000320 RID: 800
		internal const string ADP_NullDataSet = "ADP_NullDataSet";

		// Token: 0x04000321 RID: 801
		internal const string OdbcConnection_ConnectionStringTooLong = "OdbcConnection_ConnectionStringTooLong";

		// Token: 0x04000322 RID: 802
		internal const string Odbc_GetTypeMapping_UnknownType = "Odbc_GetTypeMapping_UnknownType";

		// Token: 0x04000323 RID: 803
		internal const string Odbc_UnknownSQLType = "Odbc_UnknownSQLType";

		// Token: 0x04000324 RID: 804
		internal const string Odbc_UnknownURTType = "Odbc_UnknownURTType";

		// Token: 0x04000325 RID: 805
		internal const string Odbc_NegativeArgument = "Odbc_NegativeArgument";

		// Token: 0x04000326 RID: 806
		internal const string Odbc_CantSetPropertyOnOpenConnection = "Odbc_CantSetPropertyOnOpenConnection";

		// Token: 0x04000327 RID: 807
		internal const string Odbc_NoMappingForSqlTransactionLevel = "Odbc_NoMappingForSqlTransactionLevel";

		// Token: 0x04000328 RID: 808
		internal const string Odbc_CantEnableConnectionpooling = "Odbc_CantEnableConnectionpooling";

		// Token: 0x04000329 RID: 809
		internal const string Odbc_CantAllocateEnvironmentHandle = "Odbc_CantAllocateEnvironmentHandle";

		// Token: 0x0400032A RID: 810
		internal const string Odbc_FailedToGetDescriptorHandle = "Odbc_FailedToGetDescriptorHandle";

		// Token: 0x0400032B RID: 811
		internal const string Odbc_NotInTransaction = "Odbc_NotInTransaction";

		// Token: 0x0400032C RID: 812
		internal const string Odbc_UnknownOdbcType = "Odbc_UnknownOdbcType";

		// Token: 0x0400032D RID: 813
		internal const string Odbc_NullData = "Odbc_NullData";

		// Token: 0x0400032E RID: 814
		internal const string Odbc_ExceptionMessage = "Odbc_ExceptionMessage";

		// Token: 0x0400032F RID: 815
		internal const string Odbc_ExceptionNoInfoMsg = "Odbc_ExceptionNoInfoMsg";

		// Token: 0x04000330 RID: 816
		internal const string Odbc_MDACWrongVersion = "Odbc_MDACWrongVersion";

		// Token: 0x04000331 RID: 817
		internal const string OleDb_MDACWrongVersion = "OleDb_MDACWrongVersion";

		// Token: 0x04000332 RID: 818
		internal const string OleDb_SchemaRowsetsNotSupported = "OleDb_SchemaRowsetsNotSupported";

		// Token: 0x04000333 RID: 819
		internal const string OleDb_NoErrorInformation2 = "OleDb_NoErrorInformation2";

		// Token: 0x04000334 RID: 820
		internal const string OleDb_NoErrorInformation = "OleDb_NoErrorInformation";

		// Token: 0x04000335 RID: 821
		internal const string OleDb_MDACNotAvailable = "OleDb_MDACNotAvailable";

		// Token: 0x04000336 RID: 822
		internal const string OleDb_MSDASQLNotSupported = "OleDb_MSDASQLNotSupported";

		// Token: 0x04000337 RID: 823
		internal const string OleDb_PossiblePromptNotUserInteractive = "OleDb_PossiblePromptNotUserInteractive";

		// Token: 0x04000338 RID: 824
		internal const string OleDb_ProviderUnavailable = "OleDb_ProviderUnavailable";

		// Token: 0x04000339 RID: 825
		internal const string OleDb_CommandTextNotSupported = "OleDb_CommandTextNotSupported";

		// Token: 0x0400033A RID: 826
		internal const string OleDb_TransactionsNotSupported = "OleDb_TransactionsNotSupported";

		// Token: 0x0400033B RID: 827
		internal const string OleDb_ConnectionStringSyntax = "OleDb_ConnectionStringSyntax";

		// Token: 0x0400033C RID: 828
		internal const string OleDb_AsynchronousNotSupported = "OleDb_AsynchronousNotSupported";

		// Token: 0x0400033D RID: 829
		internal const string OleDb_NoProviderSpecified = "OleDb_NoProviderSpecified";

		// Token: 0x0400033E RID: 830
		internal const string OleDb_InvalidProviderSpecified = "OleDb_InvalidProviderSpecified";

		// Token: 0x0400033F RID: 831
		internal const string OleDb_InvalidRestrictionsDbInfoKeywords = "OleDb_InvalidRestrictionsDbInfoKeywords";

		// Token: 0x04000340 RID: 832
		internal const string OleDb_InvalidRestrictionsDbInfoLiteral = "OleDb_InvalidRestrictionsDbInfoLiteral";

		// Token: 0x04000341 RID: 833
		internal const string OleDb_InvalidRestrictionsSchemaGuids = "OleDb_InvalidRestrictionsSchemaGuids";

		// Token: 0x04000342 RID: 834
		internal const string OleDb_NotSupportedSchemaTable = "OleDb_NotSupportedSchemaTable";

		// Token: 0x04000343 RID: 835
		internal const string OleDb_ConfigWrongNumberOfValues = "OleDb_ConfigWrongNumberOfValues";

		// Token: 0x04000344 RID: 836
		internal const string OleDb_ConfigUnableToLoadXmlMetaDataFile = "OleDb_ConfigUnableToLoadXmlMetaDataFile";

		// Token: 0x04000345 RID: 837
		internal const string OleDb_CommandParameterBadAccessor = "OleDb_CommandParameterBadAccessor";

		// Token: 0x04000346 RID: 838
		internal const string OleDb_CommandParameterCantConvertValue = "OleDb_CommandParameterCantConvertValue";

		// Token: 0x04000347 RID: 839
		internal const string OleDb_CommandParameterSignMismatch = "OleDb_CommandParameterSignMismatch";

		// Token: 0x04000348 RID: 840
		internal const string OleDb_CommandParameterDataOverflow = "OleDb_CommandParameterDataOverflow";

		// Token: 0x04000349 RID: 841
		internal const string OleDb_CommandParameterUnavailable = "OleDb_CommandParameterUnavailable";

		// Token: 0x0400034A RID: 842
		internal const string OleDb_CommandParameterDefault = "OleDb_CommandParameterDefault";

		// Token: 0x0400034B RID: 843
		internal const string OleDb_CommandParameterError = "OleDb_CommandParameterError";

		// Token: 0x0400034C RID: 844
		internal const string OleDb_BadStatus_ParamAcc = "OleDb_BadStatus_ParamAcc";

		// Token: 0x0400034D RID: 845
		internal const string OleDb_UninitializedParameters = "OleDb_UninitializedParameters";

		// Token: 0x0400034E RID: 846
		internal const string OleDb_NoProviderSupportForParameters = "OleDb_NoProviderSupportForParameters";

		// Token: 0x0400034F RID: 847
		internal const string OleDb_NoProviderSupportForSProcResetParameters = "OleDb_NoProviderSupportForSProcResetParameters";

		// Token: 0x04000350 RID: 848
		internal const string OleDb_CanNotDetermineDecimalSeparator = "OleDb_CanNotDetermineDecimalSeparator";

		// Token: 0x04000351 RID: 849
		internal const string OleDb_Fill_NotADODB = "OleDb_Fill_NotADODB";

		// Token: 0x04000352 RID: 850
		internal const string OleDb_Fill_EmptyRecordSet = "OleDb_Fill_EmptyRecordSet";

		// Token: 0x04000353 RID: 851
		internal const string OleDb_Fill_EmptyRecord = "OleDb_Fill_EmptyRecord";

		// Token: 0x04000354 RID: 852
		internal const string OleDb_ISourcesRowsetNotSupported = "OleDb_ISourcesRowsetNotSupported";

		// Token: 0x04000355 RID: 853
		internal const string OleDb_IDBInfoNotSupported = "OleDb_IDBInfoNotSupported";

		// Token: 0x04000356 RID: 854
		internal const string OleDb_PropertyNotSupported = "OleDb_PropertyNotSupported";

		// Token: 0x04000357 RID: 855
		internal const string OleDb_PropertyBadValue = "OleDb_PropertyBadValue";

		// Token: 0x04000358 RID: 856
		internal const string OleDb_PropertyBadOption = "OleDb_PropertyBadOption";

		// Token: 0x04000359 RID: 857
		internal const string OleDb_PropertyBadColumn = "OleDb_PropertyBadColumn";

		// Token: 0x0400035A RID: 858
		internal const string OleDb_PropertyNotAllSettable = "OleDb_PropertyNotAllSettable";

		// Token: 0x0400035B RID: 859
		internal const string OleDb_PropertyNotSettable = "OleDb_PropertyNotSettable";

		// Token: 0x0400035C RID: 860
		internal const string OleDb_PropertyNotSet = "OleDb_PropertyNotSet";

		// Token: 0x0400035D RID: 861
		internal const string OleDb_PropertyConflicting = "OleDb_PropertyConflicting";

		// Token: 0x0400035E RID: 862
		internal const string OleDb_PropertyNotAvailable = "OleDb_PropertyNotAvailable";

		// Token: 0x0400035F RID: 863
		internal const string OleDb_PropertyStatusUnknown = "OleDb_PropertyStatusUnknown";

		// Token: 0x04000360 RID: 864
		internal const string OleDb_BadAccessor = "OleDb_BadAccessor";

		// Token: 0x04000361 RID: 865
		internal const string OleDb_BadStatusRowAccessor = "OleDb_BadStatusRowAccessor";

		// Token: 0x04000362 RID: 866
		internal const string OleDb_CantConvertValue = "OleDb_CantConvertValue";

		// Token: 0x04000363 RID: 867
		internal const string OleDb_CantCreate = "OleDb_CantCreate";

		// Token: 0x04000364 RID: 868
		internal const string OleDb_DataOverflow = "OleDb_DataOverflow";

		// Token: 0x04000365 RID: 869
		internal const string OleDb_GVtUnknown = "OleDb_GVtUnknown";

		// Token: 0x04000366 RID: 870
		internal const string OleDb_SignMismatch = "OleDb_SignMismatch";

		// Token: 0x04000367 RID: 871
		internal const string OleDb_SVtUnknown = "OleDb_SVtUnknown";

		// Token: 0x04000368 RID: 872
		internal const string OleDb_Unavailable = "OleDb_Unavailable";

		// Token: 0x04000369 RID: 873
		internal const string OleDb_UnexpectedStatusValue = "OleDb_UnexpectedStatusValue";

		// Token: 0x0400036A RID: 874
		internal const string OleDb_ThreadApartmentState = "OleDb_ThreadApartmentState";

		// Token: 0x0400036B RID: 875
		internal const string OleDb_NoErrorMessage = "OleDb_NoErrorMessage";

		// Token: 0x0400036C RID: 876
		internal const string OleDb_FailedGetDescription = "OleDb_FailedGetDescription";

		// Token: 0x0400036D RID: 877
		internal const string OleDb_FailedGetSource = "OleDb_FailedGetSource";

		// Token: 0x0400036E RID: 878
		internal const string OleDb_DBBindingGetVector = "OleDb_DBBindingGetVector";

		// Token: 0x0400036F RID: 879
		internal const string ADP_InvalidMinMaxPoolSizeValues = "ADP_InvalidMinMaxPoolSizeValues";

		// Token: 0x04000370 RID: 880
		internal const string ADP_ObsoleteKeyword = "ADP_ObsoleteKeyword";

		// Token: 0x04000371 RID: 881
		internal const string SQL_CannotGetDTCAddress = "SQL_CannotGetDTCAddress";

		// Token: 0x04000372 RID: 882
		internal const string SQL_InvalidOptionLength = "SQL_InvalidOptionLength";

		// Token: 0x04000373 RID: 883
		internal const string SQL_InvalidPacketSizeValue = "SQL_InvalidPacketSizeValue";

		// Token: 0x04000374 RID: 884
		internal const string SQL_NullEmptyTransactionName = "SQL_NullEmptyTransactionName";

		// Token: 0x04000375 RID: 885
		internal const string SQL_SnapshotNotSupported = "SQL_SnapshotNotSupported";

		// Token: 0x04000376 RID: 886
		internal const string SQL_UserInstanceFailoverNotCompatible = "SQL_UserInstanceFailoverNotCompatible";

		// Token: 0x04000377 RID: 887
		internal const string SQL_EncryptionNotSupportedByClient = "SQL_EncryptionNotSupportedByClient";

		// Token: 0x04000378 RID: 888
		internal const string SQL_EncryptionNotSupportedByServer = "SQL_EncryptionNotSupportedByServer";

		// Token: 0x04000379 RID: 889
		internal const string SQL_InvalidSQLServerVersionUnknown = "SQL_InvalidSQLServerVersionUnknown";

		// Token: 0x0400037A RID: 890
		internal const string SQL_CannotModifyPropertyAsyncOperationInProgress = "SQL_CannotModifyPropertyAsyncOperationInProgress";

		// Token: 0x0400037B RID: 891
		internal const string SQL_AsyncConnectionRequired = "SQL_AsyncConnectionRequired";

		// Token: 0x0400037C RID: 892
		internal const string SQL_FatalTimeout = "SQL_FatalTimeout";

		// Token: 0x0400037D RID: 893
		internal const string SQL_InstanceFailure = "SQL_InstanceFailure";

		// Token: 0x0400037E RID: 894
		internal const string SQL_ChangePasswordArgumentMissing = "SQL_ChangePasswordArgumentMissing";

		// Token: 0x0400037F RID: 895
		internal const string SQL_ChangePasswordConflictsWithSSPI = "SQL_ChangePasswordConflictsWithSSPI";

		// Token: 0x04000380 RID: 896
		internal const string SQL_ChangePasswordUseOfUnallowedKey = "SQL_ChangePasswordUseOfUnallowedKey";

		// Token: 0x04000381 RID: 897
		internal const string SQL_UnknownSysTxIsolationLevel = "SQL_UnknownSysTxIsolationLevel";

		// Token: 0x04000382 RID: 898
		internal const string SQL_InvalidPartnerConfiguration = "SQL_InvalidPartnerConfiguration";

		// Token: 0x04000383 RID: 899
		internal const string SQL_MarsUnsupportedOnConnection = "SQL_MarsUnsupportedOnConnection";

		// Token: 0x04000384 RID: 900
		internal const string SQL_ChangePasswordRequiresYukon = "SQL_ChangePasswordRequiresYukon";

		// Token: 0x04000385 RID: 901
		internal const string SQL_AsyncInProcNotSupported = "SQL_AsyncInProcNotSupported";

		// Token: 0x04000386 RID: 902
		internal const string SQL_NonLocalSSEInstance = "SQL_NonLocalSSEInstance";

		// Token: 0x04000387 RID: 903
		internal const string SQL_AsyncOperationCompleted = "SQL_AsyncOperationCompleted";

		// Token: 0x04000388 RID: 904
		internal const string SQL_PendingBeginXXXExists = "SQL_PendingBeginXXXExists";

		// Token: 0x04000389 RID: 905
		internal const string SQL_NonXmlResult = "SQL_NonXmlResult";

		// Token: 0x0400038A RID: 906
		internal const string SQL_NotificationsRequireYukon = "SQL_NotificationsRequireYukon";

		// Token: 0x0400038B RID: 907
		internal const string SQL_InvalidUdt3PartNameFormat = "SQL_InvalidUdt3PartNameFormat";

		// Token: 0x0400038C RID: 908
		internal const string SQL_InvalidParameterTypeNameFormat = "SQL_InvalidParameterTypeNameFormat";

		// Token: 0x0400038D RID: 909
		internal const string SQL_InvalidParameterNameLength = "SQL_InvalidParameterNameLength";

		// Token: 0x0400038E RID: 910
		internal const string SQL_PrecisionValueOutOfRange = "SQL_PrecisionValueOutOfRange";

		// Token: 0x0400038F RID: 911
		internal const string SQL_ScaleValueOutOfRange = "SQL_ScaleValueOutOfRange";

		// Token: 0x04000390 RID: 912
		internal const string SQL_TimeScaleValueOutOfRange = "SQL_TimeScaleValueOutOfRange";

		// Token: 0x04000391 RID: 913
		internal const string SQL_ParameterInvalidVariant = "SQL_ParameterInvalidVariant";

		// Token: 0x04000392 RID: 914
		internal const string SQL_ParameterTypeNameRequired = "SQL_ParameterTypeNameRequired";

		// Token: 0x04000393 RID: 915
		internal const string SQL_InvalidInternalPacketSize = "SQL_InvalidInternalPacketSize";

		// Token: 0x04000394 RID: 916
		internal const string SQL_InvalidTDSVersion = "SQL_InvalidTDSVersion";

		// Token: 0x04000395 RID: 917
		internal const string SQL_InvalidTDSPacketSize = "SQL_InvalidTDSPacketSize";

		// Token: 0x04000396 RID: 918
		internal const string SQL_ParsingError = "SQL_ParsingError";

		// Token: 0x04000397 RID: 919
		internal const string SQL_ConnectionLockedForBcpEvent = "SQL_ConnectionLockedForBcpEvent";

		// Token: 0x04000398 RID: 920
		internal const string SQL_SNIPacketAllocationFailure = "SQL_SNIPacketAllocationFailure";

		// Token: 0x04000399 RID: 921
		internal const string SQL_SmallDateTimeOverflow = "SQL_SmallDateTimeOverflow";

		// Token: 0x0400039A RID: 922
		internal const string SQL_TimeOverflow = "SQL_TimeOverflow";

		// Token: 0x0400039B RID: 923
		internal const string SQL_MoneyOverflow = "SQL_MoneyOverflow";

		// Token: 0x0400039C RID: 924
		internal const string SQL_CultureIdError = "SQL_CultureIdError";

		// Token: 0x0400039D RID: 925
		internal const string SQL_OperationCancelled = "SQL_OperationCancelled";

		// Token: 0x0400039E RID: 926
		internal const string SQL_SevereError = "SQL_SevereError";

		// Token: 0x0400039F RID: 927
		internal const string SQL_SSPIGenerateError = "SQL_SSPIGenerateError";

		// Token: 0x040003A0 RID: 928
		internal const string SQL_InvalidSSPIPacketSize = "SQL_InvalidSSPIPacketSize";

		// Token: 0x040003A1 RID: 929
		internal const string SQL_SSPIInitializeError = "SQL_SSPIInitializeError";

		// Token: 0x040003A2 RID: 930
		internal const string SQL_Timeout = "SQL_Timeout";

		// Token: 0x040003A3 RID: 931
		internal const string SQL_UserInstanceFailure = "SQL_UserInstanceFailure";

		// Token: 0x040003A4 RID: 932
		internal const string SQL_ExceedsMaxDataLength = "SQL_ExceedsMaxDataLength";

		// Token: 0x040003A5 RID: 933
		internal const string SQL_InvalidRead = "SQL_InvalidRead";

		// Token: 0x040003A6 RID: 934
		internal const string SQL_NonBlobColumn = "SQL_NonBlobColumn";

		// Token: 0x040003A7 RID: 935
		internal const string SQL_NonCharColumn = "SQL_NonCharColumn";

		// Token: 0x040003A8 RID: 936
		internal const string SQL_InvalidBufferSizeOrIndex = "SQL_InvalidBufferSizeOrIndex";

		// Token: 0x040003A9 RID: 937
		internal const string SQL_InvalidDataLength = "SQL_InvalidDataLength";

		// Token: 0x040003AA RID: 938
		internal const string SQL_SqlResultSetClosed = "SQL_SqlResultSetClosed";

		// Token: 0x040003AB RID: 939
		internal const string SQL_SqlResultSetClosed2 = "SQL_SqlResultSetClosed2";

		// Token: 0x040003AC RID: 940
		internal const string SQL_SqlRecordReadOnly = "SQL_SqlRecordReadOnly";

		// Token: 0x040003AD RID: 941
		internal const string SQL_SqlRecordReadOnly2 = "SQL_SqlRecordReadOnly2";

		// Token: 0x040003AE RID: 942
		internal const string SQL_SqlResultSetRowDeleted = "SQL_SqlResultSetRowDeleted";

		// Token: 0x040003AF RID: 943
		internal const string SQL_SqlResultSetRowDeleted2 = "SQL_SqlResultSetRowDeleted2";

		// Token: 0x040003B0 RID: 944
		internal const string SQL_SqlResultSetCommandNotInSameConnection = "SQL_SqlResultSetCommandNotInSameConnection";

		// Token: 0x040003B1 RID: 945
		internal const string SQL_SqlResultSetNoAcceptableCursor = "SQL_SqlResultSetNoAcceptableCursor";

		// Token: 0x040003B2 RID: 946
		internal const string SQL_SqlUpdatableRecordReadOnly = "SQL_SqlUpdatableRecordReadOnly";

		// Token: 0x040003B3 RID: 947
		internal const string SQL_BulkLoadMappingInaccessible = "SQL_BulkLoadMappingInaccessible";

		// Token: 0x040003B4 RID: 948
		internal const string SQL_BulkLoadMappingsNamesOrOrdinalsOnly = "SQL_BulkLoadMappingsNamesOrOrdinalsOnly";

		// Token: 0x040003B5 RID: 949
		internal const string SQL_BulkLoadCannotConvertValue = "SQL_BulkLoadCannotConvertValue";

		// Token: 0x040003B6 RID: 950
		internal const string SQL_BulkLoadNonMatchingColumnMapping = "SQL_BulkLoadNonMatchingColumnMapping";

		// Token: 0x040003B7 RID: 951
		internal const string SQL_BulkLoadNonMatchingColumnName = "SQL_BulkLoadNonMatchingColumnName";

		// Token: 0x040003B8 RID: 952
		internal const string SQL_BulkLoadStringTooLong = "SQL_BulkLoadStringTooLong";

		// Token: 0x040003B9 RID: 953
		internal const string SQL_BulkLoadInvalidTimeout = "SQL_BulkLoadInvalidTimeout";

		// Token: 0x040003BA RID: 954
		internal const string SQL_BulkLoadInvalidVariantValue = "SQL_BulkLoadInvalidVariantValue";

		// Token: 0x040003BB RID: 955
		internal const string SQL_BulkLoadExistingTransaction = "SQL_BulkLoadExistingTransaction";

		// Token: 0x040003BC RID: 956
		internal const string SQL_BulkLoadNoCollation = "SQL_BulkLoadNoCollation";

		// Token: 0x040003BD RID: 957
		internal const string SQL_BulkLoadConflictingTransactionOption = "SQL_BulkLoadConflictingTransactionOption";

		// Token: 0x040003BE RID: 958
		internal const string SQL_BulkLoadInvalidOperationInsideEvent = "SQL_BulkLoadInvalidOperationInsideEvent";

		// Token: 0x040003BF RID: 959
		internal const string SQL_BulkLoadMissingDestinationTable = "SQL_BulkLoadMissingDestinationTable";

		// Token: 0x040003C0 RID: 960
		internal const string SQL_BulkLoadInvalidDestinationTable = "SQL_BulkLoadInvalidDestinationTable";

		// Token: 0x040003C1 RID: 961
		internal const string SQL_BulkLoadNotAllowDBNull = "SQL_BulkLoadNotAllowDBNull";

		// Token: 0x040003C2 RID: 962
		internal const string Sql_BulkLoadLcidMismatch = "Sql_BulkLoadLcidMismatch";

		// Token: 0x040003C3 RID: 963
		internal const string SQL_ConnectionDoomed = "SQL_ConnectionDoomed";

		// Token: 0x040003C4 RID: 964
		internal const string SQL_BatchedUpdatesNotAvailableOnContextConnection = "SQL_BatchedUpdatesNotAvailableOnContextConnection";

		// Token: 0x040003C5 RID: 965
		internal const string SQL_ContextAllowsLimitedKeywords = "SQL_ContextAllowsLimitedKeywords";

		// Token: 0x040003C6 RID: 966
		internal const string SQL_ContextAllowsOnlyTypeSystem2005 = "SQL_ContextAllowsOnlyTypeSystem2005";

		// Token: 0x040003C7 RID: 967
		internal const string SQL_ContextConnectionIsInUse = "SQL_ContextConnectionIsInUse";

		// Token: 0x040003C8 RID: 968
		internal const string SQL_ContextUnavailableOutOfProc = "SQL_ContextUnavailableOutOfProc";

		// Token: 0x040003C9 RID: 969
		internal const string SQL_ContextUnavailableWhileInProc = "SQL_ContextUnavailableWhileInProc";

		// Token: 0x040003CA RID: 970
		internal const string SQL_NestedTransactionScopesNotSupported = "SQL_NestedTransactionScopesNotSupported";

		// Token: 0x040003CB RID: 971
		internal const string SQL_NotAvailableOnContextConnection = "SQL_NotAvailableOnContextConnection";

		// Token: 0x040003CC RID: 972
		internal const string SQL_NotificationsNotAvailableOnContextConnection = "SQL_NotificationsNotAvailableOnContextConnection";

		// Token: 0x040003CD RID: 973
		internal const string SQL_UnexpectedSmiEvent = "SQL_UnexpectedSmiEvent";

		// Token: 0x040003CE RID: 974
		internal const string SQL_UserInstanceNotAvailableInProc = "SQL_UserInstanceNotAvailableInProc";

		// Token: 0x040003CF RID: 975
		internal const string SQL_ArgumentLengthMismatch = "SQL_ArgumentLengthMismatch";

		// Token: 0x040003D0 RID: 976
		internal const string SQL_InvalidSqlDbTypeWithOneAllowedType = "SQL_InvalidSqlDbTypeWithOneAllowedType";

		// Token: 0x040003D1 RID: 977
		internal const string SQL_PipeErrorRequiresSendEnd = "SQL_PipeErrorRequiresSendEnd";

		// Token: 0x040003D2 RID: 978
		internal const string SQL_TooManyValues = "SQL_TooManyValues";

		// Token: 0x040003D3 RID: 979
		internal const string SQL_StreamWriteNotSupported = "SQL_StreamWriteNotSupported";

		// Token: 0x040003D4 RID: 980
		internal const string SQL_StreamReadNotSupported = "SQL_StreamReadNotSupported";

		// Token: 0x040003D5 RID: 981
		internal const string SQL_StreamSeekNotSupported = "SQL_StreamSeekNotSupported";

		// Token: 0x040003D6 RID: 982
		internal const string SqlMisc_NullString = "SqlMisc_NullString";

		// Token: 0x040003D7 RID: 983
		internal const string SqlMisc_MessageString = "SqlMisc_MessageString";

		// Token: 0x040003D8 RID: 984
		internal const string SqlMisc_ArithOverflowMessage = "SqlMisc_ArithOverflowMessage";

		// Token: 0x040003D9 RID: 985
		internal const string SqlMisc_DivideByZeroMessage = "SqlMisc_DivideByZeroMessage";

		// Token: 0x040003DA RID: 986
		internal const string SqlMisc_NullValueMessage = "SqlMisc_NullValueMessage";

		// Token: 0x040003DB RID: 987
		internal const string SqlMisc_TruncationMessage = "SqlMisc_TruncationMessage";

		// Token: 0x040003DC RID: 988
		internal const string SqlMisc_DateTimeOverflowMessage = "SqlMisc_DateTimeOverflowMessage";

		// Token: 0x040003DD RID: 989
		internal const string SqlMisc_ConcatDiffCollationMessage = "SqlMisc_ConcatDiffCollationMessage";

		// Token: 0x040003DE RID: 990
		internal const string SqlMisc_CompareDiffCollationMessage = "SqlMisc_CompareDiffCollationMessage";

		// Token: 0x040003DF RID: 991
		internal const string SqlMisc_InvalidFlagMessage = "SqlMisc_InvalidFlagMessage";

		// Token: 0x040003E0 RID: 992
		internal const string SqlMisc_NumeToDecOverflowMessage = "SqlMisc_NumeToDecOverflowMessage";

		// Token: 0x040003E1 RID: 993
		internal const string SqlMisc_ConversionOverflowMessage = "SqlMisc_ConversionOverflowMessage";

		// Token: 0x040003E2 RID: 994
		internal const string SqlMisc_InvalidDateTimeMessage = "SqlMisc_InvalidDateTimeMessage";

		// Token: 0x040003E3 RID: 995
		internal const string SqlMisc_TimeZoneSpecifiedMessage = "SqlMisc_TimeZoneSpecifiedMessage";

		// Token: 0x040003E4 RID: 996
		internal const string SqlMisc_InvalidArraySizeMessage = "SqlMisc_InvalidArraySizeMessage";

		// Token: 0x040003E5 RID: 997
		internal const string SqlMisc_InvalidPrecScaleMessage = "SqlMisc_InvalidPrecScaleMessage";

		// Token: 0x040003E6 RID: 998
		internal const string SqlMisc_FormatMessage = "SqlMisc_FormatMessage";

		// Token: 0x040003E7 RID: 999
		internal const string SqlMisc_SqlTypeMessage = "SqlMisc_SqlTypeMessage";

		// Token: 0x040003E8 RID: 1000
		internal const string SqlMisc_LenTooLargeMessage = "SqlMisc_LenTooLargeMessage";

		// Token: 0x040003E9 RID: 1001
		internal const string SqlMisc_StreamErrorMessage = "SqlMisc_StreamErrorMessage";

		// Token: 0x040003EA RID: 1002
		internal const string SqlMisc_StreamClosedMessage = "SqlMisc_StreamClosedMessage";

		// Token: 0x040003EB RID: 1003
		internal const string SqlMisc_NoBufferMessage = "SqlMisc_NoBufferMessage";

		// Token: 0x040003EC RID: 1004
		internal const string SqlMisc_SetNonZeroLenOnNullMessage = "SqlMisc_SetNonZeroLenOnNullMessage";

		// Token: 0x040003ED RID: 1005
		internal const string SqlMisc_BufferInsufficientMessage = "SqlMisc_BufferInsufficientMessage";

		// Token: 0x040003EE RID: 1006
		internal const string SqlMisc_WriteNonZeroOffsetOnNullMessage = "SqlMisc_WriteNonZeroOffsetOnNullMessage";

		// Token: 0x040003EF RID: 1007
		internal const string SqlMisc_WriteOffsetLargerThanLenMessage = "SqlMisc_WriteOffsetLargerThanLenMessage";

		// Token: 0x040003F0 RID: 1008
		internal const string SqlMisc_TruncationMaxDataMessage = "SqlMisc_TruncationMaxDataMessage";

		// Token: 0x040003F1 RID: 1009
		internal const string SqlMisc_InvalidFirstDayMessage = "SqlMisc_InvalidFirstDayMessage";

		// Token: 0x040003F2 RID: 1010
		internal const string SqlMisc_NotFilledMessage = "SqlMisc_NotFilledMessage";

		// Token: 0x040003F3 RID: 1011
		internal const string SqlMisc_AlreadyFilledMessage = "SqlMisc_AlreadyFilledMessage";

		// Token: 0x040003F4 RID: 1012
		internal const string SqlMisc_ClosedXmlReaderMessage = "SqlMisc_ClosedXmlReaderMessage";

		// Token: 0x040003F5 RID: 1013
		internal const string SqlMisc_InvalidOpStreamClosed = "SqlMisc_InvalidOpStreamClosed";

		// Token: 0x040003F6 RID: 1014
		internal const string SqlMisc_InvalidOpStreamNonWritable = "SqlMisc_InvalidOpStreamNonWritable";

		// Token: 0x040003F7 RID: 1015
		internal const string SqlMisc_InvalidOpStreamNonReadable = "SqlMisc_InvalidOpStreamNonReadable";

		// Token: 0x040003F8 RID: 1016
		internal const string SqlMisc_InvalidOpStreamNonSeekable = "SqlMisc_InvalidOpStreamNonSeekable";

		// Token: 0x040003F9 RID: 1017
		internal const string SqlMisc_SubclassMustOverride = "SqlMisc_SubclassMustOverride";

		// Token: 0x040003FA RID: 1018
		internal const string Sql_CanotCreateNormalizer = "Sql_CanotCreateNormalizer";

		// Token: 0x040003FB RID: 1019
		internal const string Sql_InternalError = "Sql_InternalError";

		// Token: 0x040003FC RID: 1020
		internal const string Sql_NullCommandText = "Sql_NullCommandText";

		// Token: 0x040003FD RID: 1021
		internal const string Sql_MismatchedMetaDataDirectionArrayLengths = "Sql_MismatchedMetaDataDirectionArrayLengths";

		// Token: 0x040003FE RID: 1022
		internal const string ADP_AdapterMappingExceptionMessage = "ADP_AdapterMappingExceptionMessage";

		// Token: 0x040003FF RID: 1023
		internal const string ADP_DataAdapterExceptionMessage = "ADP_DataAdapterExceptionMessage";

		// Token: 0x04000400 RID: 1024
		internal const string ADP_DBConcurrencyExceptionMessage = "ADP_DBConcurrencyExceptionMessage";

		// Token: 0x04000401 RID: 1025
		internal const string ADP_OperationAborted = "ADP_OperationAborted";

		// Token: 0x04000402 RID: 1026
		internal const string ADP_OperationAbortedExceptionMessage = "ADP_OperationAbortedExceptionMessage";

		// Token: 0x04000403 RID: 1027
		internal const string DataAdapter_AcceptChangesDuringFill = "DataAdapter_AcceptChangesDuringFill";

		// Token: 0x04000404 RID: 1028
		internal const string DataAdapter_AcceptChangesDuringUpdate = "DataAdapter_AcceptChangesDuringUpdate";

		// Token: 0x04000405 RID: 1029
		internal const string DataAdapter_ContinueUpdateOnError = "DataAdapter_ContinueUpdateOnError";

		// Token: 0x04000406 RID: 1030
		internal const string DataAdapter_FillLoadOption = "DataAdapter_FillLoadOption";

		// Token: 0x04000407 RID: 1031
		internal const string DataAdapter_MissingMappingAction = "DataAdapter_MissingMappingAction";

		// Token: 0x04000408 RID: 1032
		internal const string DataAdapter_MissingSchemaAction = "DataAdapter_MissingSchemaAction";

		// Token: 0x04000409 RID: 1033
		internal const string DataAdapter_TableMappings = "DataAdapter_TableMappings";

		// Token: 0x0400040A RID: 1034
		internal const string DataAdapter_FillError = "DataAdapter_FillError";

		// Token: 0x0400040B RID: 1035
		internal const string DataAdapter_ReturnProviderSpecificTypes = "DataAdapter_ReturnProviderSpecificTypes";

		// Token: 0x0400040C RID: 1036
		internal const string DataColumnMapping_DataSetColumn = "DataColumnMapping_DataSetColumn";

		// Token: 0x0400040D RID: 1037
		internal const string DataColumnMapping_SourceColumn = "DataColumnMapping_SourceColumn";

		// Token: 0x0400040E RID: 1038
		internal const string DataColumnMappings_Count = "DataColumnMappings_Count";

		// Token: 0x0400040F RID: 1039
		internal const string DataColumnMappings_Item = "DataColumnMappings_Item";

		// Token: 0x04000410 RID: 1040
		internal const string DataTableMapping_ColumnMappings = "DataTableMapping_ColumnMappings";

		// Token: 0x04000411 RID: 1041
		internal const string DataTableMapping_DataSetTable = "DataTableMapping_DataSetTable";

		// Token: 0x04000412 RID: 1042
		internal const string DataTableMapping_SourceTable = "DataTableMapping_SourceTable";

		// Token: 0x04000413 RID: 1043
		internal const string DataTableMappings_Count = "DataTableMappings_Count";

		// Token: 0x04000414 RID: 1044
		internal const string DataTableMappings_Item = "DataTableMappings_Item";

		// Token: 0x04000415 RID: 1045
		internal const string DbDataAdapter_DeleteCommand = "DbDataAdapter_DeleteCommand";

		// Token: 0x04000416 RID: 1046
		internal const string DbDataAdapter_InsertCommand = "DbDataAdapter_InsertCommand";

		// Token: 0x04000417 RID: 1047
		internal const string DbDataAdapter_SelectCommand = "DbDataAdapter_SelectCommand";

		// Token: 0x04000418 RID: 1048
		internal const string DbDataAdapter_UpdateCommand = "DbDataAdapter_UpdateCommand";

		// Token: 0x04000419 RID: 1049
		internal const string DbDataAdapter_RowUpdated = "DbDataAdapter_RowUpdated";

		// Token: 0x0400041A RID: 1050
		internal const string DbDataAdapter_RowUpdating = "DbDataAdapter_RowUpdating";

		// Token: 0x0400041B RID: 1051
		internal const string DbDataAdapter_UpdateBatchSize = "DbDataAdapter_UpdateBatchSize";

		// Token: 0x0400041C RID: 1052
		internal const string DbTable_Connection = "DbTable_Connection";

		// Token: 0x0400041D RID: 1053
		internal const string DbTable_DeleteCommand = "DbTable_DeleteCommand";

		// Token: 0x0400041E RID: 1054
		internal const string DbTable_InsertCommand = "DbTable_InsertCommand";

		// Token: 0x0400041F RID: 1055
		internal const string DbTable_SelectCommand = "DbTable_SelectCommand";

		// Token: 0x04000420 RID: 1056
		internal const string DbTable_UpdateCommand = "DbTable_UpdateCommand";

		// Token: 0x04000421 RID: 1057
		internal const string DbTable_ReturnProviderSpecificTypes = "DbTable_ReturnProviderSpecificTypes";

		// Token: 0x04000422 RID: 1058
		internal const string DbTable_TableMapping = "DbTable_TableMapping";

		// Token: 0x04000423 RID: 1059
		internal const string DbTable_ConflictDetection = "DbTable_ConflictDetection";

		// Token: 0x04000424 RID: 1060
		internal const string DbTable_UpdateBatchSize = "DbTable_UpdateBatchSize";

		// Token: 0x04000425 RID: 1061
		internal const string DbConnectionString_ConnectionString = "DbConnectionString_ConnectionString";

		// Token: 0x04000426 RID: 1062
		internal const string DbConnectionString_Driver = "DbConnectionString_Driver";

		// Token: 0x04000427 RID: 1063
		internal const string DbConnectionString_DSN = "DbConnectionString_DSN";

		// Token: 0x04000428 RID: 1064
		internal const string DbConnectionString_AdoNetPooler = "DbConnectionString_AdoNetPooler";

		// Token: 0x04000429 RID: 1065
		internal const string DbConnectionString_FileName = "DbConnectionString_FileName";

		// Token: 0x0400042A RID: 1066
		internal const string DbConnectionString_OleDbServices = "DbConnectionString_OleDbServices";

		// Token: 0x0400042B RID: 1067
		internal const string DbConnectionString_Provider = "DbConnectionString_Provider";

		// Token: 0x0400042C RID: 1068
		internal const string DbConnectionString_ApplicationName = "DbConnectionString_ApplicationName";

		// Token: 0x0400042D RID: 1069
		internal const string DbConnectionString_AsynchronousProcessing = "DbConnectionString_AsynchronousProcessing";

		// Token: 0x0400042E RID: 1070
		internal const string DbConnectionString_AttachDBFilename = "DbConnectionString_AttachDBFilename";

		// Token: 0x0400042F RID: 1071
		internal const string DbConnectionString_ConnectTimeout = "DbConnectionString_ConnectTimeout";

		// Token: 0x04000430 RID: 1072
		internal const string DbConnectionString_ConnectionReset = "DbConnectionString_ConnectionReset";

		// Token: 0x04000431 RID: 1073
		internal const string DbConnectionString_ContextConnection = "DbConnectionString_ContextConnection";

		// Token: 0x04000432 RID: 1074
		internal const string DbConnectionString_CurrentLanguage = "DbConnectionString_CurrentLanguage";

		// Token: 0x04000433 RID: 1075
		internal const string DbConnectionString_DataSource = "DbConnectionString_DataSource";

		// Token: 0x04000434 RID: 1076
		internal const string DbConnectionString_Encrypt = "DbConnectionString_Encrypt";

		// Token: 0x04000435 RID: 1077
		internal const string DbConnectionString_Enlist = "DbConnectionString_Enlist";

		// Token: 0x04000436 RID: 1078
		internal const string DbConnectionString_InitialCatalog = "DbConnectionString_InitialCatalog";

		// Token: 0x04000437 RID: 1079
		internal const string DbConnectionString_FailoverPartner = "DbConnectionString_FailoverPartner";

		// Token: 0x04000438 RID: 1080
		internal const string DbConnectionString_IntegratedSecurity = "DbConnectionString_IntegratedSecurity";

		// Token: 0x04000439 RID: 1081
		internal const string DbConnectionString_LoadBalanceTimeout = "DbConnectionString_LoadBalanceTimeout";

		// Token: 0x0400043A RID: 1082
		internal const string DbConnectionString_MaxPoolSize = "DbConnectionString_MaxPoolSize";

		// Token: 0x0400043B RID: 1083
		internal const string DbConnectionString_MinPoolSize = "DbConnectionString_MinPoolSize";

		// Token: 0x0400043C RID: 1084
		internal const string DbConnectionString_MultipleActiveResultSets = "DbConnectionString_MultipleActiveResultSets";

		// Token: 0x0400043D RID: 1085
		internal const string DbConnectionString_MultiSubnetFailover = "DbConnectionString_MultiSubnetFailover";

		// Token: 0x0400043E RID: 1086
		internal const string DbConnectionString_NetworkLibrary = "DbConnectionString_NetworkLibrary";

		// Token: 0x0400043F RID: 1087
		internal const string DbConnectionString_PacketSize = "DbConnectionString_PacketSize";

		// Token: 0x04000440 RID: 1088
		internal const string DbConnectionString_Password = "DbConnectionString_Password";

		// Token: 0x04000441 RID: 1089
		internal const string DbConnectionString_PersistSecurityInfo = "DbConnectionString_PersistSecurityInfo";

		// Token: 0x04000442 RID: 1090
		internal const string DbConnectionString_Pooling = "DbConnectionString_Pooling";

		// Token: 0x04000443 RID: 1091
		internal const string DbConnectionString_Replication = "DbConnectionString_Replication";

		// Token: 0x04000444 RID: 1092
		internal const string DbConnectionString_TransactionBinding = "DbConnectionString_TransactionBinding";

		// Token: 0x04000445 RID: 1093
		internal const string DbConnectionString_TrustServerCertificate = "DbConnectionString_TrustServerCertificate";

		// Token: 0x04000446 RID: 1094
		internal const string DbConnectionString_TypeSystemVersion = "DbConnectionString_TypeSystemVersion";

		// Token: 0x04000447 RID: 1095
		internal const string DbConnectionString_UserID = "DbConnectionString_UserID";

		// Token: 0x04000448 RID: 1096
		internal const string DbConnectionString_UserInstance = "DbConnectionString_UserInstance";

		// Token: 0x04000449 RID: 1097
		internal const string DbConnectionString_WorkstationID = "DbConnectionString_WorkstationID";

		// Token: 0x0400044A RID: 1098
		internal const string DbConnectionString_ApplicationIntent = "DbConnectionString_ApplicationIntent";

		// Token: 0x0400044B RID: 1099
		internal const string OdbcConnection_ConnectionString = "OdbcConnection_ConnectionString";

		// Token: 0x0400044C RID: 1100
		internal const string OdbcConnection_ConnectionTimeout = "OdbcConnection_ConnectionTimeout";

		// Token: 0x0400044D RID: 1101
		internal const string OdbcConnection_Database = "OdbcConnection_Database";

		// Token: 0x0400044E RID: 1102
		internal const string OdbcConnection_DataSource = "OdbcConnection_DataSource";

		// Token: 0x0400044F RID: 1103
		internal const string OdbcConnection_Driver = "OdbcConnection_Driver";

		// Token: 0x04000450 RID: 1104
		internal const string OdbcConnection_ServerVersion = "OdbcConnection_ServerVersion";

		// Token: 0x04000451 RID: 1105
		internal const string OleDbConnection_ConnectionString = "OleDbConnection_ConnectionString";

		// Token: 0x04000452 RID: 1106
		internal const string OleDbConnection_ConnectionTimeout = "OleDbConnection_ConnectionTimeout";

		// Token: 0x04000453 RID: 1107
		internal const string OleDbConnection_Database = "OleDbConnection_Database";

		// Token: 0x04000454 RID: 1108
		internal const string OleDbConnection_DataSource = "OleDbConnection_DataSource";

		// Token: 0x04000455 RID: 1109
		internal const string OleDbConnection_Provider = "OleDbConnection_Provider";

		// Token: 0x04000456 RID: 1110
		internal const string OleDbConnection_ServerVersion = "OleDbConnection_ServerVersion";

		// Token: 0x04000457 RID: 1111
		internal const string SqlConnection_Asynchronous = "SqlConnection_Asynchronous";

		// Token: 0x04000458 RID: 1112
		internal const string SqlConnection_Replication = "SqlConnection_Replication";

		// Token: 0x04000459 RID: 1113
		internal const string SqlConnection_ConnectionString = "SqlConnection_ConnectionString";

		// Token: 0x0400045A RID: 1114
		internal const string SqlConnection_ConnectionTimeout = "SqlConnection_ConnectionTimeout";

		// Token: 0x0400045B RID: 1115
		internal const string SqlConnection_Database = "SqlConnection_Database";

		// Token: 0x0400045C RID: 1116
		internal const string SqlConnection_DataSource = "SqlConnection_DataSource";

		// Token: 0x0400045D RID: 1117
		internal const string SqlConnection_PacketSize = "SqlConnection_PacketSize";

		// Token: 0x0400045E RID: 1118
		internal const string SqlConnection_ServerVersion = "SqlConnection_ServerVersion";

		// Token: 0x0400045F RID: 1119
		internal const string SqlConnection_WorkstationId = "SqlConnection_WorkstationId";

		// Token: 0x04000460 RID: 1120
		internal const string SqlConnection_StatisticsEnabled = "SqlConnection_StatisticsEnabled";

		// Token: 0x04000461 RID: 1121
		internal const string DbConnection_InfoMessage = "DbConnection_InfoMessage";

		// Token: 0x04000462 RID: 1122
		internal const string DbCommand_CommandText = "DbCommand_CommandText";

		// Token: 0x04000463 RID: 1123
		internal const string DbCommand_CommandType = "DbCommand_CommandType";

		// Token: 0x04000464 RID: 1124
		internal const string DbCommand_Connection = "DbCommand_Connection";

		// Token: 0x04000465 RID: 1125
		internal const string DbCommand_Parameters = "DbCommand_Parameters";

		// Token: 0x04000466 RID: 1126
		internal const string DbCommand_Transaction = "DbCommand_Transaction";

		// Token: 0x04000467 RID: 1127
		internal const string DbCommand_UpdatedRowSource = "DbCommand_UpdatedRowSource";

		// Token: 0x04000468 RID: 1128
		internal const string DbCommand_StatementCompleted = "DbCommand_StatementCompleted";

		// Token: 0x04000469 RID: 1129
		internal const string SqlCommand_Notification = "SqlCommand_Notification";

		// Token: 0x0400046A RID: 1130
		internal const string SqlCommand_NotificationAutoEnlist = "SqlCommand_NotificationAutoEnlist";

		// Token: 0x0400046B RID: 1131
		internal const string DbCommandBuilder_ConflictOption = "DbCommandBuilder_ConflictOption";

		// Token: 0x0400046C RID: 1132
		internal const string DbCommandBuilder_CatalogLocation = "DbCommandBuilder_CatalogLocation";

		// Token: 0x0400046D RID: 1133
		internal const string DbCommandBuilder_CatalogSeparator = "DbCommandBuilder_CatalogSeparator";

		// Token: 0x0400046E RID: 1134
		internal const string DbCommandBuilder_SchemaSeparator = "DbCommandBuilder_SchemaSeparator";

		// Token: 0x0400046F RID: 1135
		internal const string DbCommandBuilder_QuotePrefix = "DbCommandBuilder_QuotePrefix";

		// Token: 0x04000470 RID: 1136
		internal const string DbCommandBuilder_QuoteSuffix = "DbCommandBuilder_QuoteSuffix";

		// Token: 0x04000471 RID: 1137
		internal const string DbCommandBuilder_DataAdapter = "DbCommandBuilder_DataAdapter";

		// Token: 0x04000472 RID: 1138
		internal const string DbCommandBuilder_SchemaLocation = "DbCommandBuilder_SchemaLocation";

		// Token: 0x04000473 RID: 1139
		internal const string DbCommandBuilder_SetAllValues = "DbCommandBuilder_SetAllValues";

		// Token: 0x04000474 RID: 1140
		internal const string OdbcCommandBuilder_DataAdapter = "OdbcCommandBuilder_DataAdapter";

		// Token: 0x04000475 RID: 1141
		internal const string OdbcCommandBuilder_QuotePrefix = "OdbcCommandBuilder_QuotePrefix";

		// Token: 0x04000476 RID: 1142
		internal const string OdbcCommandBuilder_QuoteSuffix = "OdbcCommandBuilder_QuoteSuffix";

		// Token: 0x04000477 RID: 1143
		internal const string OleDbCommandBuilder_DataAdapter = "OleDbCommandBuilder_DataAdapter";

		// Token: 0x04000478 RID: 1144
		internal const string OleDbCommandBuilder_DecimalSeparator = "OleDbCommandBuilder_DecimalSeparator";

		// Token: 0x04000479 RID: 1145
		internal const string OleDbCommandBuilder_QuotePrefix = "OleDbCommandBuilder_QuotePrefix";

		// Token: 0x0400047A RID: 1146
		internal const string OleDbCommandBuilder_QuoteSuffix = "OleDbCommandBuilder_QuoteSuffix";

		// Token: 0x0400047B RID: 1147
		internal const string SqlCommandBuilder_DataAdapter = "SqlCommandBuilder_DataAdapter";

		// Token: 0x0400047C RID: 1148
		internal const string SqlCommandBuilder_DecimalSeparator = "SqlCommandBuilder_DecimalSeparator";

		// Token: 0x0400047D RID: 1149
		internal const string SqlCommandBuilder_QuotePrefix = "SqlCommandBuilder_QuotePrefix";

		// Token: 0x0400047E RID: 1150
		internal const string SqlCommandBuilder_QuoteSuffix = "SqlCommandBuilder_QuoteSuffix";

		// Token: 0x0400047F RID: 1151
		internal const string DbDataParameter_Precision = "DbDataParameter_Precision";

		// Token: 0x04000480 RID: 1152
		internal const string DbDataParameter_Scale = "DbDataParameter_Scale";

		// Token: 0x04000481 RID: 1153
		internal const string OdbcParameter_OdbcType = "OdbcParameter_OdbcType";

		// Token: 0x04000482 RID: 1154
		internal const string OleDbParameter_OleDbType = "OleDbParameter_OleDbType";

		// Token: 0x04000483 RID: 1155
		internal const string SqlParameter_ParameterName = "SqlParameter_ParameterName";

		// Token: 0x04000484 RID: 1156
		internal const string SqlParameter_SqlDbType = "SqlParameter_SqlDbType";

		// Token: 0x04000485 RID: 1157
		internal const string SqlParameter_TypeName = "SqlParameter_TypeName";

		// Token: 0x04000486 RID: 1158
		internal const string SqlParameter_Offset = "SqlParameter_Offset";

		// Token: 0x04000487 RID: 1159
		internal const string SqlParameter_XmlSchemaCollectionDatabase = "SqlParameter_XmlSchemaCollectionDatabase";

		// Token: 0x04000488 RID: 1160
		internal const string SqlParameter_XmlSchemaCollectionOwningSchema = "SqlParameter_XmlSchemaCollectionOwningSchema";

		// Token: 0x04000489 RID: 1161
		internal const string SqlParameter_XmlSchemaCollectionName = "SqlParameter_XmlSchemaCollectionName";

		// Token: 0x0400048A RID: 1162
		internal const string SqlParameter_UnsupportedTVPOutputParameter = "SqlParameter_UnsupportedTVPOutputParameter";

		// Token: 0x0400048B RID: 1163
		internal const string SqlParameter_DBNullNotSupportedForTVP = "SqlParameter_DBNullNotSupportedForTVP";

		// Token: 0x0400048C RID: 1164
		internal const string SqlParameter_InvalidTableDerivedPrecisionForTvp = "SqlParameter_InvalidTableDerivedPrecisionForTvp";

		// Token: 0x0400048D RID: 1165
		internal const string SqlParameter_UnexpectedTypeNameForNonStruct = "SqlParameter_UnexpectedTypeNameForNonStruct";

		// Token: 0x0400048E RID: 1166
		internal const string MetaType_SingleValuedStructNotSupported = "MetaType_SingleValuedStructNotSupported";

		// Token: 0x0400048F RID: 1167
		internal const string NullSchemaTableDataTypeNotSupported = "NullSchemaTableDataTypeNotSupported";

		// Token: 0x04000490 RID: 1168
		internal const string InvalidSchemaTableOrdinals = "InvalidSchemaTableOrdinals";

		// Token: 0x04000491 RID: 1169
		internal const string SQL_EnumeratedRecordMetaDataChanged = "SQL_EnumeratedRecordMetaDataChanged";

		// Token: 0x04000492 RID: 1170
		internal const string SQL_EnumeratedRecordFieldCountChanged = "SQL_EnumeratedRecordFieldCountChanged";

		// Token: 0x04000493 RID: 1171
		internal const string SQLUDT_MaxByteSizeValue = "SQLUDT_MaxByteSizeValue";

		// Token: 0x04000494 RID: 1172
		internal const string SQLUDT_Unexpected = "SQLUDT_Unexpected";

		// Token: 0x04000495 RID: 1173
		internal const string SQLUDT_InvalidDbId = "SQLUDT_InvalidDbId";

		// Token: 0x04000496 RID: 1174
		internal const string SQLUDT_CantLoadAssembly = "SQLUDT_CantLoadAssembly";

		// Token: 0x04000497 RID: 1175
		internal const string SQLUDT_InvalidUdtTypeName = "SQLUDT_InvalidUdtTypeName";

		// Token: 0x04000498 RID: 1176
		internal const string SQLUDT_UnexpectedUdtTypeName = "SQLUDT_UnexpectedUdtTypeName";

		// Token: 0x04000499 RID: 1177
		internal const string SQLUDT_InvalidSqlType = "SQLUDT_InvalidSqlType";

		// Token: 0x0400049A RID: 1178
		internal const string SQLUDT_InWhereClause = "SQLUDT_InWhereClause";

		// Token: 0x0400049B RID: 1179
		internal const string SqlUdt_InvalidUdtMessage = "SqlUdt_InvalidUdtMessage";

		// Token: 0x0400049C RID: 1180
		internal const string SqlUdtReason_MultipleSerFormats = "SqlUdtReason_MultipleSerFormats";

		// Token: 0x0400049D RID: 1181
		internal const string SqlUdtReason_CannotSupportNative = "SqlUdtReason_CannotSupportNative";

		// Token: 0x0400049E RID: 1182
		internal const string SqlUdtReason_CannotSupportUserDefined = "SqlUdtReason_CannotSupportUserDefined";

		// Token: 0x0400049F RID: 1183
		internal const string SqlUdtReason_NotSerializable = "SqlUdtReason_NotSerializable";

		// Token: 0x040004A0 RID: 1184
		internal const string SqlUdtReason_NoPublicConstructors = "SqlUdtReason_NoPublicConstructors";

		// Token: 0x040004A1 RID: 1185
		internal const string SqlUdtReason_NotNullable = "SqlUdtReason_NotNullable";

		// Token: 0x040004A2 RID: 1186
		internal const string SqlUdtReason_NoPublicConstructor = "SqlUdtReason_NoPublicConstructor";

		// Token: 0x040004A3 RID: 1187
		internal const string SqlUdtReason_NoUdtAttribute = "SqlUdtReason_NoUdtAttribute";

		// Token: 0x040004A4 RID: 1188
		internal const string SqlUdtReason_MaplessNotYetSupported = "SqlUdtReason_MaplessNotYetSupported";

		// Token: 0x040004A5 RID: 1189
		internal const string SqlUdtReason_ParseMethodMissing = "SqlUdtReason_ParseMethodMissing";

		// Token: 0x040004A6 RID: 1190
		internal const string SqlUdtReason_ToStringMethodMissing = "SqlUdtReason_ToStringMethodMissing";

		// Token: 0x040004A7 RID: 1191
		internal const string SqlUdtReason_NullPropertyMissing = "SqlUdtReason_NullPropertyMissing";

		// Token: 0x040004A8 RID: 1192
		internal const string SqlUdtReason_NativeFormatNoFieldSupport = "SqlUdtReason_NativeFormatNoFieldSupport";

		// Token: 0x040004A9 RID: 1193
		internal const string SqlUdtReason_TypeNotPublic = "SqlUdtReason_TypeNotPublic";

		// Token: 0x040004AA RID: 1194
		internal const string SqlUdtReason_NativeUdtNotSequentialLayout = "SqlUdtReason_NativeUdtNotSequentialLayout";

		// Token: 0x040004AB RID: 1195
		internal const string SqlUdtReason_NativeUdtMaxByteSize = "SqlUdtReason_NativeUdtMaxByteSize";

		// Token: 0x040004AC RID: 1196
		internal const string SqlUdtReason_NonSerializableField = "SqlUdtReason_NonSerializableField";

		// Token: 0x040004AD RID: 1197
		internal const string SqlUdtReason_NativeFormatExplictLayoutNotAllowed = "SqlUdtReason_NativeFormatExplictLayoutNotAllowed";

		// Token: 0x040004AE RID: 1198
		internal const string SqlUdtReason_MultivaluedAssemblyId = "SqlUdtReason_MultivaluedAssemblyId";

		// Token: 0x040004AF RID: 1199
		internal const string SQLTVP_TableTypeCanOnlyBeParameter = "SQLTVP_TableTypeCanOnlyBeParameter";

		// Token: 0x040004B0 RID: 1200
		internal const string SqlFileStream_InvalidPath = "SqlFileStream_InvalidPath";

		// Token: 0x040004B1 RID: 1201
		internal const string SqlFileStream_InvalidParameter = "SqlFileStream_InvalidParameter";

		// Token: 0x040004B2 RID: 1202
		internal const string SqlFileStream_FileAlreadyInTransaction = "SqlFileStream_FileAlreadyInTransaction";

		// Token: 0x040004B3 RID: 1203
		internal const string SqlFileStream_PathNotValidDiskResource = "SqlFileStream_PathNotValidDiskResource";

		// Token: 0x040004B4 RID: 1204
		internal const string SqlDelegatedTransaction_PromotionFailed = "SqlDelegatedTransaction_PromotionFailed";

		// Token: 0x040004B5 RID: 1205
		internal const string SqlDependency_SqlDependency = "SqlDependency_SqlDependency";

		// Token: 0x040004B6 RID: 1206
		internal const string SqlDependency_HasChanges = "SqlDependency_HasChanges";

		// Token: 0x040004B7 RID: 1207
		internal const string SqlDependency_Id = "SqlDependency_Id";

		// Token: 0x040004B8 RID: 1208
		internal const string SqlDependency_OnChange = "SqlDependency_OnChange";

		// Token: 0x040004B9 RID: 1209
		internal const string SqlDependency_AddCommandDependency = "SqlDependency_AddCommandDependency";

		// Token: 0x040004BA RID: 1210
		internal const string SqlDependency_Duplicate = "SqlDependency_Duplicate";

		// Token: 0x040004BB RID: 1211
		internal const string SQLNotify_AlreadyHasCommand = "SQLNotify_AlreadyHasCommand";

		// Token: 0x040004BC RID: 1212
		internal const string SqlNotify_SqlDepCannotBeCreatedInProc = "SqlNotify_SqlDepCannotBeCreatedInProc";

		// Token: 0x040004BD RID: 1213
		internal const string SqlDependency_DatabaseBrokerDisabled = "SqlDependency_DatabaseBrokerDisabled";

		// Token: 0x040004BE RID: 1214
		internal const string SqlDependency_DefaultOptionsButNoStart = "SqlDependency_DefaultOptionsButNoStart";

		// Token: 0x040004BF RID: 1215
		internal const string SqlDependency_EventNoDuplicate = "SqlDependency_EventNoDuplicate";

		// Token: 0x040004C0 RID: 1216
		internal const string SqlDependency_DuplicateStart = "SqlDependency_DuplicateStart";

		// Token: 0x040004C1 RID: 1217
		internal const string SqlDependency_IdMismatch = "SqlDependency_IdMismatch";

		// Token: 0x040004C2 RID: 1218
		internal const string SqlDependency_NoMatchingServerStart = "SqlDependency_NoMatchingServerStart";

		// Token: 0x040004C3 RID: 1219
		internal const string SqlDependency_NoMatchingServerDatabaseStart = "SqlDependency_NoMatchingServerDatabaseStart";

		// Token: 0x040004C4 RID: 1220
		internal const string SqlDependency_InvalidTimeout = "SqlDependency_InvalidTimeout";

		// Token: 0x040004C5 RID: 1221
		internal const string SQLNotify_ErrorFormat = "SQLNotify_ErrorFormat";

		// Token: 0x040004C6 RID: 1222
		internal const string SqlMetaData_NoMetadata = "SqlMetaData_NoMetadata";

		// Token: 0x040004C7 RID: 1223
		internal const string SqlMetaData_InvalidSqlDbTypeForConstructorFormat = "SqlMetaData_InvalidSqlDbTypeForConstructorFormat";

		// Token: 0x040004C8 RID: 1224
		internal const string SqlMetaData_NameTooLong = "SqlMetaData_NameTooLong";

		// Token: 0x040004C9 RID: 1225
		internal const string SqlMetaData_SpecifyBothSortOrderAndOrdinal = "SqlMetaData_SpecifyBothSortOrderAndOrdinal";

		// Token: 0x040004CA RID: 1226
		internal const string SqlProvider_InvalidDataColumnType = "SqlProvider_InvalidDataColumnType";

		// Token: 0x040004CB RID: 1227
		internal const string SqlProvider_InvalidDataColumnMaxLength = "SqlProvider_InvalidDataColumnMaxLength";

		// Token: 0x040004CC RID: 1228
		internal const string SqlProvider_NotEnoughColumnsInStructuredType = "SqlProvider_NotEnoughColumnsInStructuredType";

		// Token: 0x040004CD RID: 1229
		internal const string SqlProvider_DuplicateSortOrdinal = "SqlProvider_DuplicateSortOrdinal";

		// Token: 0x040004CE RID: 1230
		internal const string SqlProvider_MissingSortOrdinal = "SqlProvider_MissingSortOrdinal";

		// Token: 0x040004CF RID: 1231
		internal const string SqlProvider_SortOrdinalGreaterThanFieldCount = "SqlProvider_SortOrdinalGreaterThanFieldCount";

		// Token: 0x040004D0 RID: 1232
		internal const string IEnumerableOfSqlDataRecordHasNoRows = "IEnumerableOfSqlDataRecordHasNoRows";

		// Token: 0x040004D1 RID: 1233
		internal const string SqlPipe_CommandHookedUpToNonContextConnection = "SqlPipe_CommandHookedUpToNonContextConnection";

		// Token: 0x040004D2 RID: 1234
		internal const string SqlPipe_MessageTooLong = "SqlPipe_MessageTooLong";

		// Token: 0x040004D3 RID: 1235
		internal const string SqlPipe_IsBusy = "SqlPipe_IsBusy";

		// Token: 0x040004D4 RID: 1236
		internal const string SqlPipe_AlreadyHasAnOpenResultSet = "SqlPipe_AlreadyHasAnOpenResultSet";

		// Token: 0x040004D5 RID: 1237
		internal const string SqlPipe_DoesNotHaveAnOpenResultSet = "SqlPipe_DoesNotHaveAnOpenResultSet";

		// Token: 0x040004D6 RID: 1238
		internal const string SNI_PN0 = "SNI_PN0";

		// Token: 0x040004D7 RID: 1239
		internal const string SNI_PN1 = "SNI_PN1";

		// Token: 0x040004D8 RID: 1240
		internal const string SNI_PN2 = "SNI_PN2";

		// Token: 0x040004D9 RID: 1241
		internal const string SNI_PN3 = "SNI_PN3";

		// Token: 0x040004DA RID: 1242
		internal const string SNI_PN4 = "SNI_PN4";

		// Token: 0x040004DB RID: 1243
		internal const string SNI_PN5 = "SNI_PN5";

		// Token: 0x040004DC RID: 1244
		internal const string SNI_PN6 = "SNI_PN6";

		// Token: 0x040004DD RID: 1245
		internal const string SNI_PN7 = "SNI_PN7";

		// Token: 0x040004DE RID: 1246
		internal const string SNI_PN8 = "SNI_PN8";

		// Token: 0x040004DF RID: 1247
		internal const string SNI_PN9 = "SNI_PN9";

		// Token: 0x040004E0 RID: 1248
		internal const string SNI_PN10 = "SNI_PN10";

		// Token: 0x040004E1 RID: 1249
		internal const string SNI_ERROR_1 = "SNI_ERROR_1";

		// Token: 0x040004E2 RID: 1250
		internal const string SNI_ERROR_2 = "SNI_ERROR_2";

		// Token: 0x040004E3 RID: 1251
		internal const string SNI_ERROR_3 = "SNI_ERROR_3";

		// Token: 0x040004E4 RID: 1252
		internal const string SNI_ERROR_4 = "SNI_ERROR_4";

		// Token: 0x040004E5 RID: 1253
		internal const string SNI_ERROR_5 = "SNI_ERROR_5";

		// Token: 0x040004E6 RID: 1254
		internal const string SNI_ERROR_6 = "SNI_ERROR_6";

		// Token: 0x040004E7 RID: 1255
		internal const string SNI_ERROR_7 = "SNI_ERROR_7";

		// Token: 0x040004E8 RID: 1256
		internal const string SNI_ERROR_8 = "SNI_ERROR_8";

		// Token: 0x040004E9 RID: 1257
		internal const string SNI_ERROR_9 = "SNI_ERROR_9";

		// Token: 0x040004EA RID: 1258
		internal const string SNI_ERROR_10 = "SNI_ERROR_10";

		// Token: 0x040004EB RID: 1259
		internal const string SNI_ERROR_11 = "SNI_ERROR_11";

		// Token: 0x040004EC RID: 1260
		internal const string SNI_ERROR_12 = "SNI_ERROR_12";

		// Token: 0x040004ED RID: 1261
		internal const string SNI_ERROR_13 = "SNI_ERROR_13";

		// Token: 0x040004EE RID: 1262
		internal const string SNI_ERROR_14 = "SNI_ERROR_14";

		// Token: 0x040004EF RID: 1263
		internal const string SNI_ERROR_15 = "SNI_ERROR_15";

		// Token: 0x040004F0 RID: 1264
		internal const string SNI_ERROR_16 = "SNI_ERROR_16";

		// Token: 0x040004F1 RID: 1265
		internal const string SNI_ERROR_17 = "SNI_ERROR_17";

		// Token: 0x040004F2 RID: 1266
		internal const string SNI_ERROR_18 = "SNI_ERROR_18";

		// Token: 0x040004F3 RID: 1267
		internal const string SNI_ERROR_19 = "SNI_ERROR_19";

		// Token: 0x040004F4 RID: 1268
		internal const string SNI_ERROR_20 = "SNI_ERROR_20";

		// Token: 0x040004F5 RID: 1269
		internal const string SNI_ERROR_21 = "SNI_ERROR_21";

		// Token: 0x040004F6 RID: 1270
		internal const string SNI_ERROR_22 = "SNI_ERROR_22";

		// Token: 0x040004F7 RID: 1271
		internal const string SNI_ERROR_23 = "SNI_ERROR_23";

		// Token: 0x040004F8 RID: 1272
		internal const string SNI_ERROR_24 = "SNI_ERROR_24";

		// Token: 0x040004F9 RID: 1273
		internal const string SNI_ERROR_25 = "SNI_ERROR_25";

		// Token: 0x040004FA RID: 1274
		internal const string SNI_ERROR_26 = "SNI_ERROR_26";

		// Token: 0x040004FB RID: 1275
		internal const string SNI_ERROR_27 = "SNI_ERROR_27";

		// Token: 0x040004FC RID: 1276
		internal const string SNI_ERROR_28 = "SNI_ERROR_28";

		// Token: 0x040004FD RID: 1277
		internal const string SNI_ERROR_29 = "SNI_ERROR_29";

		// Token: 0x040004FE RID: 1278
		internal const string SNI_ERROR_30 = "SNI_ERROR_30";

		// Token: 0x040004FF RID: 1279
		internal const string SNI_ERROR_31 = "SNI_ERROR_31";

		// Token: 0x04000500 RID: 1280
		internal const string SNI_ERROR_32 = "SNI_ERROR_32";

		// Token: 0x04000501 RID: 1281
		internal const string SNI_ERROR_33 = "SNI_ERROR_33";

		// Token: 0x04000502 RID: 1282
		internal const string SNI_ERROR_34 = "SNI_ERROR_34";

		// Token: 0x04000503 RID: 1283
		internal const string SNI_ERROR_35 = "SNI_ERROR_35";

		// Token: 0x04000504 RID: 1284
		internal const string SNI_ERROR_36 = "SNI_ERROR_36";

		// Token: 0x04000505 RID: 1285
		internal const string SNI_ERROR_37 = "SNI_ERROR_37";

		// Token: 0x04000506 RID: 1286
		internal const string SNI_ERROR_38 = "SNI_ERROR_38";

		// Token: 0x04000507 RID: 1287
		internal const string SNI_ERROR_39 = "SNI_ERROR_39";

		// Token: 0x04000508 RID: 1288
		internal const string SNI_ERROR_40 = "SNI_ERROR_40";

		// Token: 0x04000509 RID: 1289
		internal const string SNI_ERROR_41 = "SNI_ERROR_41";

		// Token: 0x0400050A RID: 1290
		internal const string SNI_ERROR_42 = "SNI_ERROR_42";

		// Token: 0x0400050B RID: 1291
		internal const string SNI_ERROR_43 = "SNI_ERROR_43";

		// Token: 0x0400050C RID: 1292
		internal const string SNI_ERROR_44 = "SNI_ERROR_44";

		// Token: 0x0400050D RID: 1293
		internal const string SNI_ERROR_47 = "SNI_ERROR_47";

		// Token: 0x0400050E RID: 1294
		internal const string SNI_ERROR_48 = "SNI_ERROR_48";

		// Token: 0x0400050F RID: 1295
		internal const string SNI_ERROR_49 = "SNI_ERROR_49";

		// Token: 0x04000510 RID: 1296
		internal const string SNI_ERROR_50 = "SNI_ERROR_50";

		// Token: 0x04000511 RID: 1297
		internal const string SNI_ERROR_51 = "SNI_ERROR_51";

		// Token: 0x04000512 RID: 1298
		internal const string SNI_ERROR_52 = "SNI_ERROR_52";

		// Token: 0x04000513 RID: 1299
		internal const string SNI_ERROR_53 = "SNI_ERROR_53";

		// Token: 0x04000514 RID: 1300
		internal const string SNI_ERROR_54 = "SNI_ERROR_54";

		// Token: 0x04000515 RID: 1301
		internal const string SNI_ERROR_55 = "SNI_ERROR_55";

		// Token: 0x04000516 RID: 1302
		internal const string SNI_ERROR_56 = "SNI_ERROR_56";

		// Token: 0x04000517 RID: 1303
		internal const string SNI_ERROR_57 = "SNI_ERROR_57";

		// Token: 0x04000518 RID: 1304
		internal const string Snix_Connect = "Snix_Connect";

		// Token: 0x04000519 RID: 1305
		internal const string Snix_PreLoginBeforeSuccessfullWrite = "Snix_PreLoginBeforeSuccessfullWrite";

		// Token: 0x0400051A RID: 1306
		internal const string Snix_PreLogin = "Snix_PreLogin";

		// Token: 0x0400051B RID: 1307
		internal const string Snix_LoginSspi = "Snix_LoginSspi";

		// Token: 0x0400051C RID: 1308
		internal const string Snix_Login = "Snix_Login";

		// Token: 0x0400051D RID: 1309
		internal const string Snix_EnableMars = "Snix_EnableMars";

		// Token: 0x0400051E RID: 1310
		internal const string Snix_AutoEnlist = "Snix_AutoEnlist";

		// Token: 0x0400051F RID: 1311
		internal const string Snix_GetMarsSession = "Snix_GetMarsSession";

		// Token: 0x04000520 RID: 1312
		internal const string Snix_Execute = "Snix_Execute";

		// Token: 0x04000521 RID: 1313
		internal const string Snix_Read = "Snix_Read";

		// Token: 0x04000522 RID: 1314
		internal const string Snix_Close = "Snix_Close";

		// Token: 0x04000523 RID: 1315
		internal const string Snix_SendRows = "Snix_SendRows";

		// Token: 0x04000524 RID: 1316
		internal const string Snix_ProcessSspi = "Snix_ProcessSspi";

		// Token: 0x04000525 RID: 1317
		internal const string SQLROR_RecursiveRoutingNotSupported = "SQLROR_RecursiveRoutingNotSupported";

		// Token: 0x04000526 RID: 1318
		internal const string SQLROR_FailoverNotSupported = "SQLROR_FailoverNotSupported";

		// Token: 0x04000527 RID: 1319
		internal const string SQLROR_UnexpectedRoutingInfo = "SQLROR_UnexpectedRoutingInfo";

		// Token: 0x04000528 RID: 1320
		internal const string SQLROR_InvalidRoutingInfo = "SQLROR_InvalidRoutingInfo";

		// Token: 0x04000529 RID: 1321
		internal const string SQLROR_TimeoutAfterRoutingInfo = "SQLROR_TimeoutAfterRoutingInfo";

		// Token: 0x0400052A RID: 1322
		internal const string LocalDB_CreateFailed = "LocalDB_CreateFailed";

		// Token: 0x0400052B RID: 1323
		internal const string LocalDB_BadConfigSectionType = "LocalDB_BadConfigSectionType";

		// Token: 0x0400052C RID: 1324
		internal const string LocalDB_FailedGetDLLHandle = "LocalDB_FailedGetDLLHandle";

		// Token: 0x0400052D RID: 1325
		internal const string LocalDB_MethodNotFound = "LocalDB_MethodNotFound";

		// Token: 0x0400052E RID: 1326
		internal const string LocalDB_UnobtainableMessage = "LocalDB_UnobtainableMessage";

		// Token: 0x0400052F RID: 1327
		internal const string LocalDB_InvalidVersion = "LocalDB_InvalidVersion";

		// Token: 0x04000530 RID: 1328
		private static Res loader;

		// Token: 0x04000531 RID: 1329
		private ResourceManager resources;

		// Token: 0x04000532 RID: 1330
		private static object s_InternalSyncObject;
	}
}
