using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Web
{
	// Token: 0x02000007 RID: 7
	internal sealed class SR
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002114 File Offset: 0x00001114
		private static object InternalSyncObject
		{
			get
			{
				if (SR.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref SR.s_InternalSyncObject, obj, null);
				}
				return SR.s_InternalSyncObject;
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002140 File Offset: 0x00001140
		internal SR()
		{
			this.resources = new ResourceManager("System.Web", base.GetType().Assembly);
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002164 File Offset: 0x00001164
		private static SR GetLoader()
		{
			if (SR.loader == null)
			{
				lock (SR.InternalSyncObject)
				{
					if (SR.loader == null)
					{
						SR.loader = new SR();
					}
				}
			}
			return SR.loader;
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
				return SR.GetLoader().resources;
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021C4 File Offset: 0x000011C4
		public static string GetString(string name, params object[] args)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			string @string = sr.resources.GetString(name, SR.Culture);
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
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetString(name, SR.Culture);
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002274 File Offset: 0x00001274
		public static object GetObject(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetObject(name, SR.Culture);
		}

		// Token: 0x04000031 RID: 49
		internal const string Parameter_Invalid = "Parameter_Invalid";

		// Token: 0x04000032 RID: 50
		internal const string Parameter_NullOrEmpty = "Parameter_NullOrEmpty";

		// Token: 0x04000033 RID: 51
		internal const string Property_NullOrEmpty = "Property_NullOrEmpty";

		// Token: 0x04000034 RID: 52
		internal const string Property_Invalid = "Property_Invalid";

		// Token: 0x04000035 RID: 53
		internal const string Unexpected_Error = "Unexpected_Error";

		// Token: 0x04000036 RID: 54
		internal const string PropertyCannotBeNullOrEmptyString = "PropertyCannotBeNullOrEmptyString";

		// Token: 0x04000037 RID: 55
		internal const string PropertyCannotBeNull = "PropertyCannotBeNull";

		// Token: 0x04000038 RID: 56
		internal const string Invalid_string_from_browser_caps = "Invalid_string_from_browser_caps";

		// Token: 0x04000039 RID: 57
		internal const string Unrecognized_construct_in_pattern = "Unrecognized_construct_in_pattern";

		// Token: 0x0400003A RID: 58
		internal const string Caps_cannot_be_inited_twice = "Caps_cannot_be_inited_twice";

		// Token: 0x0400003B RID: 59
		internal const string Duplicate_browser_id = "Duplicate_browser_id";

		// Token: 0x0400003C RID: 60
		internal const string Invalid_browser_root = "Invalid_browser_root";

		// Token: 0x0400003D RID: 61
		internal const string Browser_mutually_exclusive_attributes = "Browser_mutually_exclusive_attributes";

		// Token: 0x0400003E RID: 62
		internal const string Browser_refid_prohibits_identification = "Browser_refid_prohibits_identification";

		// Token: 0x0400003F RID: 63
		internal const string Browser_invalid_element = "Browser_invalid_element";

		// Token: 0x04000040 RID: 64
		internal const string Browser_Circular_Reference = "Browser_Circular_Reference";

		// Token: 0x04000041 RID: 65
		internal const string Browser_attributes_required = "Browser_attributes_required";

		// Token: 0x04000042 RID: 66
		internal const string Browser_parentID_Not_Found = "Browser_parentID_Not_Found";

		// Token: 0x04000043 RID: 67
		internal const string Browser_parentID_applied_to_default = "Browser_parentID_applied_to_default";

		// Token: 0x04000044 RID: 68
		internal const string Browser_InvalidID = "Browser_InvalidID";

		// Token: 0x04000045 RID: 69
		internal const string Browser_Not_Allowed_InAppLevel = "Browser_Not_Allowed_InAppLevel";

		// Token: 0x04000046 RID: 70
		internal const string Browser_InvalidStrongNameKey = "Browser_InvalidStrongNameKey";

		// Token: 0x04000047 RID: 71
		internal const string Browser_compile_error = "Browser_compile_error";

		// Token: 0x04000048 RID: 72
		internal const string DefaultBrowser_parentID_Not_Found = "DefaultBrowser_parentID_Not_Found";

		// Token: 0x04000049 RID: 73
		internal const string Browser_empty_identification = "Browser_empty_identification";

		// Token: 0x0400004A RID: 74
		internal const string Browser_W3SVC_Failure_Helper_Text = "Browser_W3SVC_Failure_Helper_Text";

		// Token: 0x0400004B RID: 75
		internal const string DefaultSiteName = "DefaultSiteName";

		// Token: 0x0400004C RID: 76
		internal const string ControlAdapters_TypeNotFound = "ControlAdapters_TypeNotFound";

		// Token: 0x0400004D RID: 77
		internal const string Failed_gac_install = "Failed_gac_install";

		// Token: 0x0400004E RID: 78
		internal const string Failed_gac_uninstall = "Failed_gac_uninstall";

		// Token: 0x0400004F RID: 79
		internal const string WrongType_of_Protected_provider = "WrongType_of_Protected_provider";

		// Token: 0x04000050 RID: 80
		internal const string Make_sure_remote_server_is_enabled_for_config_access = "Make_sure_remote_server_is_enabled_for_config_access";

		// Token: 0x04000051 RID: 81
		internal const string Config_unable_to_get_section = "Config_unable_to_get_section";

		// Token: 0x04000052 RID: 82
		internal const string Config_failed_to_resolve_site_id = "Config_failed_to_resolve_site_id";

		// Token: 0x04000053 RID: 83
		internal const string Config_GetSectionWithPathArgInvalid = "Config_GetSectionWithPathArgInvalid";

		// Token: 0x04000054 RID: 84
		internal const string Unable_to_create_temp_file = "Unable_to_create_temp_file";

		// Token: 0x04000055 RID: 85
		internal const string Config_allow_definition_error_application = "Config_allow_definition_error_application";

		// Token: 0x04000056 RID: 86
		internal const string Config_allow_definition_error_machine = "Config_allow_definition_error_machine";

		// Token: 0x04000057 RID: 87
		internal const string Config_allow_definition_error_webroot = "Config_allow_definition_error_webroot";

		// Token: 0x04000058 RID: 88
		internal const string Config_base_unrecognized_element = "Config_base_unrecognized_element";

		// Token: 0x04000059 RID: 89
		internal const string Config_base_required_attribute_missing = "Config_base_required_attribute_missing";

		// Token: 0x0400005A RID: 90
		internal const string Config_base_required_attribute_empty = "Config_base_required_attribute_empty";

		// Token: 0x0400005B RID: 91
		internal const string Config_base_unrecognized_attribute = "Config_base_unrecognized_attribute";

		// Token: 0x0400005C RID: 92
		internal const string Config_base_duplicate_line = "Config_base_duplicate_line";

		// Token: 0x0400005D RID: 93
		internal const string Config_base_elements_only = "Config_base_elements_only";

		// Token: 0x0400005E RID: 94
		internal const string Config_base_no_child_nodes = "Config_base_no_child_nodes";

		// Token: 0x0400005F RID: 95
		internal const string Config_base_file_load_exception_no_message = "Config_base_file_load_exception_no_message";

		// Token: 0x04000060 RID: 96
		internal const string Config_base_bad_image_exception_no_message = "Config_base_bad_image_exception_no_message";

		// Token: 0x04000061 RID: 97
		internal const string Config_base_report_exception_type = "Config_base_report_exception_type";

		// Token: 0x04000062 RID: 98
		internal const string Config_file_generic = "Config_file_generic";

		// Token: 0x04000063 RID: 99
		internal const string Config_property_generic = "Config_property_generic";

		// Token: 0x04000064 RID: 100
		internal const string Config_section_generic = "Config_section_generic";

		// Token: 0x04000065 RID: 101
		internal const string Unable_To_Register_Assembly = "Unable_To_Register_Assembly";

		// Token: 0x04000066 RID: 102
		internal const string Unable_To_UnRegister_Assembly = "Unable_To_UnRegister_Assembly";

		// Token: 0x04000067 RID: 103
		internal const string Could_not_create_type_instance = "Could_not_create_type_instance";

		// Token: 0x04000068 RID: 104
		internal const string Config_Invalid_enum_value = "Config_Invalid_enum_value";

		// Token: 0x04000069 RID: 105
		internal const string Config_element_below_app_illegal = "Config_element_below_app_illegal";

		// Token: 0x0400006A RID: 106
		internal const string Config_provider_must_exist = "Config_provider_must_exist";

		// Token: 0x0400006B RID: 107
		internal const string File_is_read_only = "File_is_read_only";

		// Token: 0x0400006C RID: 108
		internal const string Can_not_access_files_other_than_config = "Can_not_access_files_other_than_config";

		// Token: 0x0400006D RID: 109
		internal const string Error_loading_XML_file = "Error_loading_XML_file";

		// Token: 0x0400006E RID: 110
		internal const string Unknown_tag_in_caps_config = "Unknown_tag_in_caps_config";

		// Token: 0x0400006F RID: 111
		internal const string Cannot_specify_test_without_match = "Cannot_specify_test_without_match";

		// Token: 0x04000070 RID: 112
		internal const string Result_must_be_at_the_top_browser_section = "Result_must_be_at_the_top_browser_section";

		// Token: 0x04000071 RID: 113
		internal const string Type_doesnt_inherit_from_type = "Type_doesnt_inherit_from_type";

		// Token: 0x04000072 RID: 114
		internal const string Problem_reading_caps_config = "Problem_reading_caps_config";

		// Token: 0x04000073 RID: 115
		internal const string Special_module_cannot_be_added_manually = "Special_module_cannot_be_added_manually";

		// Token: 0x04000074 RID: 116
		internal const string Special_module_cannot_be_removed_manually = "Special_module_cannot_be_removed_manually";

		// Token: 0x04000075 RID: 117
		internal const string Module_not_in_app = "Module_not_in_app";

		// Token: 0x04000076 RID: 118
		internal const string Invalid_credentials = "Invalid_credentials";

		// Token: 0x04000077 RID: 119
		internal const string Invalid_credentials_provided = "Invalid_credentials_provided";

		// Token: 0x04000078 RID: 120
		internal const string Invalid_credentials_provided_2 = "Invalid_credentials_provided_2";

		// Token: 0x04000079 RID: 121
		internal const string Auth_Invalid_Login_Url = "Auth_Invalid_Login_Url";

		// Token: 0x0400007A RID: 122
		internal const string Invalid_value_for_globalization_attr = "Invalid_value_for_globalization_attr";

		// Token: 0x0400007B RID: 123
		internal const string Invalid_credentials_2 = "Invalid_credentials_2";

		// Token: 0x0400007C RID: 124
		internal const string Invalid_registry_config = "Invalid_registry_config";

		// Token: 0x0400007D RID: 125
		internal const string Invalid_Passport_Redirect_URL = "Invalid_Passport_Redirect_URL";

		// Token: 0x0400007E RID: 126
		internal const string Invalid_redirect_return_url = "Invalid_redirect_return_url";

		// Token: 0x0400007F RID: 127
		internal const string Config_section_not_present = "Config_section_not_present";

		// Token: 0x04000080 RID: 128
		internal const string Local_free_threads_cannot_exceed_free_threads = "Local_free_threads_cannot_exceed_free_threads";

		// Token: 0x04000081 RID: 129
		internal const string Min_free_threads_must_be_under_thread_pool_limits = "Min_free_threads_must_be_under_thread_pool_limits";

		// Token: 0x04000082 RID: 130
		internal const string Thread_pool_limit_must_be_greater_than_minFreeThreads = "Thread_pool_limit_must_be_greater_than_minFreeThreads";

		// Token: 0x04000083 RID: 131
		internal const string Config_max_request_length_disk_threshold_exceeds_max_request_length = "Config_max_request_length_disk_threshold_exceeds_max_request_length";

		// Token: 0x04000084 RID: 132
		internal const string Config_max_request_length_smaller_than_max_request_length_disk_threshold = "Config_max_request_length_smaller_than_max_request_length_disk_threshold";

		// Token: 0x04000085 RID: 133
		internal const string Capability_file_root_element = "Capability_file_root_element";

		// Token: 0x04000086 RID: 134
		internal const string File_element_only_valid_in_config = "File_element_only_valid_in_config";

		// Token: 0x04000087 RID: 135
		internal const string Clear_not_valid = "Clear_not_valid";

		// Token: 0x04000088 RID: 136
		internal const string Config_base_cannot_remove_inherited_items = "Config_base_cannot_remove_inherited_items";

		// Token: 0x04000089 RID: 137
		internal const string Nested_group_not_valid = "Nested_group_not_valid";

		// Token: 0x0400008A RID: 138
		internal const string Dup_protocol_id = "Dup_protocol_id";

		// Token: 0x0400008B RID: 139
		internal const string WebPartsSection_NoVerbs = "WebPartsSection_NoVerbs";

		// Token: 0x0400008C RID: 140
		internal const string WebPartsSection_InvalidVerb = "WebPartsSection_InvalidVerb";

		// Token: 0x0400008D RID: 141
		internal const string Transformer_types_already_added = "Transformer_types_already_added";

		// Token: 0x0400008E RID: 142
		internal const string Transformer_attribute_error = "Transformer_attribute_error";

		// Token: 0x0400008F RID: 143
		internal const string File_changed_since_read = "File_changed_since_read";

		// Token: 0x04000090 RID: 144
		internal const string Config_invalid_element = "Config_invalid_element";

		// Token: 0x04000091 RID: 145
		internal const string InvalidExpressionSyntax = "InvalidExpressionSyntax";

		// Token: 0x04000092 RID: 146
		internal const string InvalidExpressionPrefix = "InvalidExpressionPrefix";

		// Token: 0x04000093 RID: 147
		internal const string ExpressionBuilder_InvalidType = "ExpressionBuilder_InvalidType";

		// Token: 0x04000094 RID: 148
		internal const string MissingExpressionPrefix = "MissingExpressionPrefix";

		// Token: 0x04000095 RID: 149
		internal const string MissingExpressionValue = "MissingExpressionValue";

		// Token: 0x04000096 RID: 150
		internal const string ExpressionBuilder_LiteralExpressionsNotAllowed = "ExpressionBuilder_LiteralExpressionsNotAllowed";

		// Token: 0x04000097 RID: 151
		internal const string ResourceExpresionBuilder_PageResourceNotFound = "ResourceExpresionBuilder_PageResourceNotFound";

		// Token: 0x04000098 RID: 152
		internal const string Failed_to_start_monitoring = "Failed_to_start_monitoring";

		// Token: 0x04000099 RID: 153
		internal const string Invalid_file_name_for_monitoring = "Invalid_file_name_for_monitoring";

		// Token: 0x0400009A RID: 154
		internal const string Access_denied_for_monitoring = "Access_denied_for_monitoring";

		// Token: 0x0400009B RID: 155
		internal const string Directory_does_not_exist_for_monitoring = "Directory_does_not_exist_for_monitoring";

		// Token: 0x0400009C RID: 156
		internal const string NetBios_command_limit_reached = "NetBios_command_limit_reached";

		// Token: 0x0400009D RID: 157
		internal const string Directory_rename_notification = "Directory_rename_notification";

		// Token: 0x0400009E RID: 158
		internal const string Change_notification_critical_dir = "Change_notification_critical_dir";

		// Token: 0x0400009F RID: 159
		internal const string Path_not_found = "Path_not_found";

		// Token: 0x040000A0 RID: 160
		internal const string Path_forbidden = "Path_forbidden";

		// Token: 0x040000A1 RID: 161
		internal const string Method_for_path_not_implemented = "Method_for_path_not_implemented";

		// Token: 0x040000A2 RID: 162
		internal const string Method_not_allowed = "Method_not_allowed";

		// Token: 0x040000A3 RID: 163
		internal const string Cannot_call_defaulthttphandler_sync = "Cannot_call_defaulthttphandler_sync";

		// Token: 0x040000A4 RID: 164
		internal const string Handler_access_denied = "Handler_access_denied";

		// Token: 0x040000A5 RID: 165
		internal const string Debugging_forbidden = "Debugging_forbidden";

		// Token: 0x040000A6 RID: 166
		internal const string Debug_Access_Denied = "Debug_Access_Denied";

		// Token: 0x040000A7 RID: 167
		internal const string Invalid_Debug_Request = "Invalid_Debug_Request";

		// Token: 0x040000A8 RID: 168
		internal const string Invalid_Debug_ID = "Invalid_Debug_ID";

		// Token: 0x040000A9 RID: 169
		internal const string Error_Attaching_with_MDM = "Error_Attaching_with_MDM";

		// Token: 0x040000AA RID: 170
		internal const string VaryByCustom_already_set = "VaryByCustom_already_set";

		// Token: 0x040000AB RID: 171
		internal const string CacheProfile_Not_Found = "CacheProfile_Not_Found";

		// Token: 0x040000AC RID: 172
		internal const string Invalid_sqlDependency_argument = "Invalid_sqlDependency_argument";

		// Token: 0x040000AD RID: 173
		internal const string Invalid_sqlDependency_argument2 = "Invalid_sqlDependency_argument2";

		// Token: 0x040000AE RID: 174
		internal const string Etag_already_set = "Etag_already_set";

		// Token: 0x040000AF RID: 175
		internal const string Cant_both_set_and_generate_Etag = "Cant_both_set_and_generate_Etag";

		// Token: 0x040000B0 RID: 176
		internal const string Cacheability_for_field_must_be_private_or_nocache = "Cacheability_for_field_must_be_private_or_nocache";

		// Token: 0x040000B1 RID: 177
		internal const string Cache_dependency_used_more_that_once = "Cache_dependency_used_more_that_once";

		// Token: 0x040000B2 RID: 178
		internal const string Invalid_expiration_combination = "Invalid_expiration_combination";

		// Token: 0x040000B3 RID: 179
		internal const string Invalid_Dependency_Type = "Invalid_Dependency_Type";

		// Token: 0x040000B4 RID: 180
		internal const string Invalid_Parameters_To_Insert = "Invalid_Parameters_To_Insert";

		// Token: 0x040000B5 RID: 181
		internal const string Invalid_sql_cache_dep_polltime = "Invalid_sql_cache_dep_polltime";

		// Token: 0x040000B6 RID: 182
		internal const string Database_not_found = "Database_not_found";

		// Token: 0x040000B7 RID: 183
		internal const string Cant_connect_sql_cache_dep_database_polling = "Cant_connect_sql_cache_dep_database_polling";

		// Token: 0x040000B8 RID: 184
		internal const string Cant_connect_sql_cache_dep_database_admin = "Cant_connect_sql_cache_dep_database_admin";

		// Token: 0x040000B9 RID: 185
		internal const string Cant_connect_sql_cache_dep_database_admin_cmdtxt = "Cant_connect_sql_cache_dep_database_admin_cmdtxt";

		// Token: 0x040000BA RID: 186
		internal const string Database_not_enabled_for_notification = "Database_not_enabled_for_notification";

		// Token: 0x040000BB RID: 187
		internal const string Table_not_enabled_for_notification = "Table_not_enabled_for_notification";

		// Token: 0x040000BC RID: 188
		internal const string Polling_not_enabled_for_sql_cache = "Polling_not_enabled_for_sql_cache";

		// Token: 0x040000BD RID: 189
		internal const string Polltime_zero_for_database_sql_cache = "Polltime_zero_for_database_sql_cache";

		// Token: 0x040000BE RID: 190
		internal const string Permission_denied_database_polling = "Permission_denied_database_polling";

		// Token: 0x040000BF RID: 191
		internal const string Permission_denied_database_enable_notification = "Permission_denied_database_enable_notification";

		// Token: 0x040000C0 RID: 192
		internal const string Permission_denied_table_enable_notification = "Permission_denied_table_enable_notification";

		// Token: 0x040000C1 RID: 193
		internal const string Permission_denied_database_disable_notification = "Permission_denied_database_disable_notification";

		// Token: 0x040000C2 RID: 194
		internal const string Permission_denied_table_disable_notification = "Permission_denied_table_disable_notification";

		// Token: 0x040000C3 RID: 195
		internal const string Cant_get_enabled_tables_sql_cache_dep = "Cant_get_enabled_tables_sql_cache_dep";

		// Token: 0x040000C4 RID: 196
		internal const string Cant_disable_table_sql_cache_dep = "Cant_disable_table_sql_cache_dep";

		// Token: 0x040000C5 RID: 197
		internal const string Cache_null_table = "Cache_null_table";

		// Token: 0x040000C6 RID: 198
		internal const string Cache_null_table_in_tables = "Cache_null_table_in_tables";

		// Token: 0x040000C7 RID: 199
		internal const string Cache_dep_table_not_found = "Cache_dep_table_not_found";

		// Token: 0x040000C8 RID: 200
		internal const string UC_not_cached = "UC_not_cached";

		// Token: 0x040000C9 RID: 201
		internal const string UCCachePolicy_unavailable = "UCCachePolicy_unavailable";

		// Token: 0x040000CA RID: 202
		internal const string SqlCacheDependency_permission_denied = "SqlCacheDependency_permission_denied";

		// Token: 0x040000CB RID: 203
		internal const string No_UniqueId_Cache_Dependency = "No_UniqueId_Cache_Dependency";

		// Token: 0x040000CC RID: 204
		internal const string SqlCacheDependency_OutputCache_Conflict = "SqlCacheDependency_OutputCache_Conflict";

		// Token: 0x040000CD RID: 205
		internal const string Cache_not_available = "Cache_not_available";

		// Token: 0x040000CE RID: 206
		internal const string Http_handler_not_found_for_request_type = "Http_handler_not_found_for_request_type";

		// Token: 0x040000CF RID: 207
		internal const string Request_not_available = "Request_not_available";

		// Token: 0x040000D0 RID: 208
		internal const string Response_not_available = "Response_not_available";

		// Token: 0x040000D1 RID: 209
		internal const string Session_not_available = "Session_not_available";

		// Token: 0x040000D2 RID: 210
		internal const string Server_not_available = "Server_not_available";

		// Token: 0x040000D3 RID: 211
		internal const string User_not_available = "User_not_available";

		// Token: 0x040000D4 RID: 212
		internal const string Sync_not_supported = "Sync_not_supported";

		// Token: 0x040000D5 RID: 213
		internal const string Type_not_factory_or_handler = "Type_not_factory_or_handler";

		// Token: 0x040000D6 RID: 214
		internal const string Type_from_untrusted_assembly = "Type_from_untrusted_assembly";

		// Token: 0x040000D7 RID: 215
		internal const string Type_not_module = "Type_not_module";

		// Token: 0x040000D8 RID: 216
		internal const string Request_timed_out = "Request_timed_out";

		// Token: 0x040000D9 RID: 217
		internal const string Invalid_ControlState = "Invalid_ControlState";

		// Token: 0x040000DA RID: 218
		internal const string Too_late_for_ViewStateUserKey = "Too_late_for_ViewStateUserKey";

		// Token: 0x040000DB RID: 219
		internal const string Too_late_for_RegisterRequiresViewStateEncryption = "Too_late_for_RegisterRequiresViewStateEncryption";

		// Token: 0x040000DC RID: 220
		internal const string Invalid_urlencoded_form_data = "Invalid_urlencoded_form_data";

		// Token: 0x040000DD RID: 221
		internal const string Invalid_request_filter = "Invalid_request_filter";

		// Token: 0x040000DE RID: 222
		internal const string Cannot_map_path_without_context = "Cannot_map_path_without_context";

		// Token: 0x040000DF RID: 223
		internal const string Cross_app_not_allowed = "Cross_app_not_allowed";

		// Token: 0x040000E0 RID: 224
		internal const string Max_request_length_exceeded = "Max_request_length_exceeded";

		// Token: 0x040000E1 RID: 225
		internal const string Dangerous_input_detected = "Dangerous_input_detected";

		// Token: 0x040000E2 RID: 226
		internal const string Dangerous_input_detected_descr = "Dangerous_input_detected_descr";

		// Token: 0x040000E3 RID: 227
		internal const string Invalid_substitution_callback = "Invalid_substitution_callback";

		// Token: 0x040000E4 RID: 228
		internal const string Cannot_get_snapshot_if_not_buffered = "Cannot_get_snapshot_if_not_buffered";

		// Token: 0x040000E5 RID: 229
		internal const string Cannot_use_snapshot_after_headers_sent = "Cannot_use_snapshot_after_headers_sent";

		// Token: 0x040000E6 RID: 230
		internal const string Cannot_use_snapshot_for_TextWriter = "Cannot_use_snapshot_for_TextWriter";

		// Token: 0x040000E7 RID: 231
		internal const string Cannot_set_status_after_headers_sent = "Cannot_set_status_after_headers_sent";

		// Token: 0x040000E8 RID: 232
		internal const string Cannot_set_content_type_after_headers_sent = "Cannot_set_content_type_after_headers_sent";

		// Token: 0x040000E9 RID: 233
		internal const string Cannot_set_content_encoding_after_headers_sent = "Cannot_set_content_encoding_after_headers_sent";

		// Token: 0x040000EA RID: 234
		internal const string Filtering_not_allowed = "Filtering_not_allowed";

		// Token: 0x040000EB RID: 235
		internal const string Cannot_append_header_after_headers_sent = "Cannot_append_header_after_headers_sent";

		// Token: 0x040000EC RID: 236
		internal const string Cannot_append_cookie_after_headers_sent = "Cannot_append_cookie_after_headers_sent";

		// Token: 0x040000ED RID: 237
		internal const string Cannot_modify_cookies_after_headers_sent = "Cannot_modify_cookies_after_headers_sent";

		// Token: 0x040000EE RID: 238
		internal const string Cannot_clear_headers_after_headers_sent = "Cannot_clear_headers_after_headers_sent";

		// Token: 0x040000EF RID: 239
		internal const string Cannot_flush_completed_response = "Cannot_flush_completed_response";

		// Token: 0x040000F0 RID: 240
		internal const string Cannot_redirect_after_headers_sent = "Cannot_redirect_after_headers_sent";

		// Token: 0x040000F1 RID: 241
		internal const string Cannot_set_header_encoding_after_headers_sent = "Cannot_set_header_encoding_after_headers_sent";

		// Token: 0x040000F2 RID: 242
		internal const string Invalid_header_encoding = "Invalid_header_encoding";

		// Token: 0x040000F3 RID: 243
		internal const string Cannot_redirect_to_newline = "Cannot_redirect_to_newline";

		// Token: 0x040000F4 RID: 244
		internal const string Invalid_status_string = "Invalid_status_string";

		// Token: 0x040000F5 RID: 245
		internal const string Invalid_value_for_CacheControl = "Invalid_value_for_CacheControl";

		// Token: 0x040000F6 RID: 246
		internal const string OutputStream_NotAvail = "OutputStream_NotAvail";

		// Token: 0x040000F7 RID: 247
		internal const string Information_Disclosure_Warning = "Information_Disclosure_Warning";

		// Token: 0x040000F8 RID: 248
		internal const string Cannot_ResponseEnd_when_AppInstance_null = "Cannot_ResponseEnd_when_AppInstance_null";

		// Token: 0x040000F9 RID: 249
		internal const string Access_denied_to_app_dir = "Access_denied_to_app_dir";

		// Token: 0x040000FA RID: 250
		internal const string Access_denied_to_unicode_app_dir = "Access_denied_to_unicode_app_dir";

		// Token: 0x040000FB RID: 251
		internal const string Access_denied_to_path = "Access_denied_to_path";

		// Token: 0x040000FC RID: 252
		internal const string Insufficient_trust_for_attribute = "Insufficient_trust_for_attribute";

		// Token: 0x040000FD RID: 253
		internal const string XSP_init_error = "XSP_init_error";

		// Token: 0x040000FE RID: 254
		internal const string Unable_create_app_object = "Unable_create_app_object";

		// Token: 0x040000FF RID: 255
		internal const string Could_not_create_type = "Could_not_create_type";

		// Token: 0x04000100 RID: 256
		internal const string Failed_to_process_request = "Failed_to_process_request";

		// Token: 0x04000101 RID: 257
		internal const string StateManagedCollection_InvalidIndex = "StateManagedCollection_InvalidIndex";

		// Token: 0x04000102 RID: 258
		internal const string StateManagedCollection_NoKnownTypes = "StateManagedCollection_NoKnownTypes";

		// Token: 0x04000103 RID: 259
		internal const string VirtualPath_Length_Zero = "VirtualPath_Length_Zero";

		// Token: 0x04000104 RID: 260
		internal const string Invalid_app_VirtualPath = "Invalid_app_VirtualPath";

		// Token: 0x04000105 RID: 261
		internal const string Collection_CantAddNull = "Collection_CantAddNull";

		// Token: 0x04000106 RID: 262
		internal const string Collection_InvalidType = "Collection_InvalidType";

		// Token: 0x04000107 RID: 263
		internal const string VirtualPath_AllowAppRelativePath = "VirtualPath_AllowAppRelativePath";

		// Token: 0x04000108 RID: 264
		internal const string VirtualPath_AllowRelativePath = "VirtualPath_AllowRelativePath";

		// Token: 0x04000109 RID: 265
		internal const string VirtualPath_AllowAbsolutePath = "VirtualPath_AllowAbsolutePath";

		// Token: 0x0400010A RID: 266
		internal const string VirtualPath_CantMakeAppRelative = "VirtualPath_CantMakeAppRelative";

		// Token: 0x0400010B RID: 267
		internal const string VirtualPath_CantMakeAppAbsolute = "VirtualPath_CantMakeAppAbsolute";

		// Token: 0x0400010C RID: 268
		internal const string Bad_VirtualPath_in_VirtualFileBase = "Bad_VirtualPath_in_VirtualFileBase";

		// Token: 0x0400010D RID: 269
		internal const string ControlRenderedOutsideServerForm = "ControlRenderedOutsideServerForm";

		// Token: 0x0400010E RID: 270
		internal const string RequiresNT = "RequiresNT";

		// Token: 0x0400010F RID: 271
		internal const string ListEnumVersionMismatch = "ListEnumVersionMismatch";

		// Token: 0x04000110 RID: 272
		internal const string ListEnumCurrentOutOfRange = "ListEnumCurrentOutOfRange";

		// Token: 0x04000111 RID: 273
		internal const string HTMLTextWriterUnbalancedPop = "HTMLTextWriterUnbalancedPop";

		// Token: 0x04000112 RID: 274
		internal const string Server_too_busy = "Server_too_busy";

		// Token: 0x04000113 RID: 275
		internal const string InvalidArgumentValue = "InvalidArgumentValue";

		// Token: 0x04000114 RID: 276
		internal const string CompilationMutex_Create = "CompilationMutex_Create";

		// Token: 0x04000115 RID: 277
		internal const string CompilationMutex_Null = "CompilationMutex_Null";

		// Token: 0x04000116 RID: 278
		internal const string CompilationMutex_Drained = "CompilationMutex_Drained";

		// Token: 0x04000117 RID: 279
		internal const string CompilationMutex_Failed = "CompilationMutex_Failed";

		// Token: 0x04000118 RID: 280
		internal const string Failed_to_create_temp_dir = "Failed_to_create_temp_dir";

		// Token: 0x04000119 RID: 281
		internal const string Cannot_impersonate = "Cannot_impersonate";

		// Token: 0x0400011A RID: 282
		internal const string No_codegen_access = "No_codegen_access";

		// Token: 0x0400011B RID: 283
		internal const string Transaction_not_supported_in_low_trust = "Transaction_not_supported_in_low_trust";

		// Token: 0x0400011C RID: 284
		internal const string Debugging_not_supported_in_low_trust = "Debugging_not_supported_in_low_trust";

		// Token: 0x0400011D RID: 285
		internal const string Session_state_need_higher_trust = "Session_state_need_higher_trust";

		// Token: 0x0400011E RID: 286
		internal const string ExecuteUrl_not_supported = "ExecuteUrl_not_supported";

		// Token: 0x0400011F RID: 287
		internal const string Cannot_execute_url_in_this_context = "Cannot_execute_url_in_this_context";

		// Token: 0x04000120 RID: 288
		internal const string Failed_to_execute_url = "Failed_to_execute_url";

		// Token: 0x04000121 RID: 289
		internal const string Aspnet_not_installed = "Aspnet_not_installed";

		// Token: 0x04000122 RID: 290
		internal const string Failed_to_initialize_AppDomain = "Failed_to_initialize_AppDomain";

		// Token: 0x04000123 RID: 291
		internal const string Cannot_create_AppDomain = "Cannot_create_AppDomain";

		// Token: 0x04000124 RID: 292
		internal const string Cannot_create_HostEnv = "Cannot_create_HostEnv";

		// Token: 0x04000125 RID: 293
		internal const string Unknown_protocol_id = "Unknown_protocol_id";

		// Token: 0x04000126 RID: 294
		internal const string Only_1_HostEnv = "Only_1_HostEnv";

		// Token: 0x04000127 RID: 295
		internal const string Not_IRegisteredObject = "Not_IRegisteredObject";

		// Token: 0x04000128 RID: 296
		internal const string Wellknown_object_already_exists = "Wellknown_object_already_exists";

		// Token: 0x04000129 RID: 297
		internal const string Invalid_IIS_app = "Invalid_IIS_app";

		// Token: 0x0400012A RID: 298
		internal const string App_Virtual_Path = "App_Virtual_Path";

		// Token: 0x0400012B RID: 299
		internal const string Hosting_Phys_Path_Changed = "Hosting_Phys_Path_Changed";

		// Token: 0x0400012C RID: 300
		internal const string App_Domain_Restart = "App_Domain_Restart";

		// Token: 0x0400012D RID: 301
		internal const string Hosting_Env_Restart = "Hosting_Env_Restart";

		// Token: 0x0400012E RID: 302
		internal const string Hosting_Env_IdleTimeout = "Hosting_Env_IdleTimeout";

		// Token: 0x0400012F RID: 303
		internal const string Unhandled_Exception = "Unhandled_Exception";

		// Token: 0x04000130 RID: 304
		internal const string Provider_must_implement_the_interface = "Provider_must_implement_the_interface";

		// Token: 0x04000131 RID: 305
		internal const string Server_variable_cannot_be_modified = "Server_variable_cannot_be_modified";

		// Token: 0x04000132 RID: 306
		internal const string Invalid_range = "Invalid_range";

		// Token: 0x04000133 RID: 307
		internal const string Invalid_use_of_response_filter = "Invalid_use_of_response_filter";

		// Token: 0x04000134 RID: 308
		internal const string Invalid_response_filter = "Invalid_response_filter";

		// Token: 0x04000135 RID: 309
		internal const string Invalid_size = "Invalid_size";

		// Token: 0x04000136 RID: 310
		internal const string Process_information_not_available = "Process_information_not_available";

		// Token: 0x04000137 RID: 311
		internal const string Error_trying_to_enumerate_files = "Error_trying_to_enumerate_files";

		// Token: 0x04000138 RID: 312
		internal const string File_enumerator_access_denied = "File_enumerator_access_denied";

		// Token: 0x04000139 RID: 313
		internal const string File_does_not_exist = "File_does_not_exist";

		// Token: 0x0400013A RID: 314
		internal const string File_is_hidden = "File_is_hidden";

		// Token: 0x0400013B RID: 315
		internal const string Missing_star_mapping = "Missing_star_mapping";

		// Token: 0x0400013C RID: 316
		internal const string Resource_access_forbidden = "Resource_access_forbidden";

		// Token: 0x0400013D RID: 317
		internal const string SMTP_TypeCreationError = "SMTP_TypeCreationError";

		// Token: 0x0400013E RID: 318
		internal const string Could_not_create_object_of_type = "Could_not_create_object_of_type";

		// Token: 0x0400013F RID: 319
		internal const string Could_not_create_object_from_clsid = "Could_not_create_object_from_clsid";

		// Token: 0x04000140 RID: 320
		internal const string Error_executing_child_request_for_path = "Error_executing_child_request_for_path";

		// Token: 0x04000141 RID: 321
		internal const string Error_executing_child_request_for_handler = "Error_executing_child_request_for_handler";

		// Token: 0x04000142 RID: 322
		internal const string Invalid_path_for_child_request = "Invalid_path_for_child_request";

		// Token: 0x04000143 RID: 323
		internal const string Transacted_page_calls_aspcompat = "Transacted_page_calls_aspcompat";

		// Token: 0x04000144 RID: 324
		internal const string Invalid_path_for_remove = "Invalid_path_for_remove";

		// Token: 0x04000145 RID: 325
		internal const string Get_computer_name_failed = "Get_computer_name_failed";

		// Token: 0x04000146 RID: 326
		internal const string Cannot_map_path = "Cannot_map_path";

		// Token: 0x04000147 RID: 327
		internal const string Cannot_access_mappath_title = "Cannot_access_mappath_title";

		// Token: 0x04000148 RID: 328
		internal const string Cannot_access_mappath_details = "Cannot_access_mappath_details";

		// Token: 0x04000149 RID: 329
		internal const string Cannot_retrieve_request_data = "Cannot_retrieve_request_data";

		// Token: 0x0400014A RID: 330
		internal const string Cannot_read_posted_data = "Cannot_read_posted_data";

		// Token: 0x0400014B RID: 331
		internal const string Cannot_get_query_string = "Cannot_get_query_string";

		// Token: 0x0400014C RID: 332
		internal const string Cannot_get_query_string_bytes = "Cannot_get_query_string_bytes";

		// Token: 0x0400014D RID: 333
		internal const string Not_supported = "Not_supported";

		// Token: 0x0400014E RID: 334
		internal const string GetGacLocaltion_failed = "GetGacLocaltion_failed";

		// Token: 0x0400014F RID: 335
		internal const string Server_Support_Function_Error = "Server_Support_Function_Error";

		// Token: 0x04000150 RID: 336
		internal const string Server_Support_Function_Error_Disconnect = "Server_Support_Function_Error_Disconnect";

		// Token: 0x04000151 RID: 337
		internal const string Provider_Schema_Version_Not_Match = "Provider_Schema_Version_Not_Match";

		// Token: 0x04000152 RID: 338
		internal const string Could_not_create_passport_identity = "Could_not_create_passport_identity";

		// Token: 0x04000153 RID: 339
		internal const string Passport_method_failed = "Passport_method_failed";

		// Token: 0x04000154 RID: 340
		internal const string Auth_rule_names_cant_contain_char = "Auth_rule_names_cant_contain_char";

		// Token: 0x04000155 RID: 341
		internal const string Auth_rule_must_specify_users_andor_roles = "Auth_rule_must_specify_users_andor_roles";

		// Token: 0x04000156 RID: 342
		internal const string PageIndex_bad = "PageIndex_bad";

		// Token: 0x04000157 RID: 343
		internal const string PageSize_bad = "PageSize_bad";

		// Token: 0x04000158 RID: 344
		internal const string PageIndex_PageSize_bad = "PageIndex_PageSize_bad";

		// Token: 0x04000159 RID: 345
		internal const string Bad_machine_key = "Bad_machine_key";

		// Token: 0x0400015A RID: 346
		internal const string PassportAuthFailed = "PassportAuthFailed";

		// Token: 0x0400015B RID: 347
		internal const string PassportAuthFailed_Title = "PassportAuthFailed_Title";

		// Token: 0x0400015C RID: 348
		internal const string PassportAuthFailed_Description = "PassportAuthFailed_Description";

		// Token: 0x0400015D RID: 349
		internal const string Unable_to_encrypt_cookie_ticket = "Unable_to_encrypt_cookie_ticket";

		// Token: 0x0400015E RID: 350
		internal const string Unable_to_get_cookie_authentication_validation_key = "Unable_to_get_cookie_authentication_validation_key";

		// Token: 0x0400015F RID: 351
		internal const string Unable_to_validate_data = "Unable_to_validate_data";

		// Token: 0x04000160 RID: 352
		internal const string Unable_to_get_policy_file = "Unable_to_get_policy_file";

		// Token: 0x04000161 RID: 353
		internal const string Wrong_decryption_enum = "Wrong_decryption_enum";

		// Token: 0x04000162 RID: 354
		internal const string Role_is_not_empty = "Role_is_not_empty";

		// Token: 0x04000163 RID: 355
		internal const string Assess_Denied_Title = "Assess_Denied_Title";

		// Token: 0x04000164 RID: 356
		internal const string Assess_Denied_Description2 = "Assess_Denied_Description2";

		// Token: 0x04000165 RID: 357
		internal const string Assess_Denied_Section_Title2 = "Assess_Denied_Section_Title2";

		// Token: 0x04000166 RID: 358
		internal const string Assess_Denied_Misc_Content2 = "Assess_Denied_Misc_Content2";

		// Token: 0x04000167 RID: 359
		internal const string Assess_Denied_Description1 = "Assess_Denied_Description1";

		// Token: 0x04000168 RID: 360
		internal const string Assess_Denied_MiscTitle1 = "Assess_Denied_MiscTitle1";

		// Token: 0x04000169 RID: 361
		internal const string Assess_Denied_MiscContent1 = "Assess_Denied_MiscContent1";

		// Token: 0x0400016A RID: 362
		internal const string Assess_Denied_Description3 = "Assess_Denied_Description3";

		// Token: 0x0400016B RID: 363
		internal const string Assess_Denied_Section_Title3 = "Assess_Denied_Section_Title3";

		// Token: 0x0400016C RID: 364
		internal const string Assess_Denied_Misc_Content3 = "Assess_Denied_Misc_Content3";

		// Token: 0x0400016D RID: 365
		internal const string Assess_Denied_Misc_Content3_2 = "Assess_Denied_Misc_Content3_2";

		// Token: 0x0400016E RID: 366
		internal const string Auth_bad_url = "Auth_bad_url";

		// Token: 0x0400016F RID: 367
		internal const string Virtual_path_outside_application_not_supported = "Virtual_path_outside_application_not_supported";

		// Token: 0x04000170 RID: 368
		internal const string Invalid_decryption_key = "Invalid_decryption_key";

		// Token: 0x04000171 RID: 369
		internal const string Invalid_validation_key = "Invalid_validation_key";

		// Token: 0x04000172 RID: 370
		internal const string Passport_not_installed = "Passport_not_installed";

		// Token: 0x04000173 RID: 371
		internal const string DbFileName_can_not_contain_invalid_chars = "DbFileName_can_not_contain_invalid_chars";

		// Token: 0x04000174 RID: 372
		internal const string Provider_can_not_create_file_in_this_trust_level = "Provider_can_not_create_file_in_this_trust_level";

		// Token: 0x04000175 RID: 373
		internal const string Membership_provider_name_invalid = "Membership_provider_name_invalid";

		// Token: 0x04000176 RID: 374
		internal const string Role_provider_name_invalid = "Role_provider_name_invalid";

		// Token: 0x04000177 RID: 375
		internal const string Def_provider_not_found = "Def_provider_not_found";

		// Token: 0x04000178 RID: 376
		internal const string Can_not_use_encrypted_passwords_with_autogen_keys = "Can_not_use_encrypted_passwords_with_autogen_keys";

		// Token: 0x04000179 RID: 377
		internal const string Provider_no_type_name = "Provider_no_type_name";

		// Token: 0x0400017A RID: 378
		internal const string MembershipSqlProvider_description = "MembershipSqlProvider_description";

		// Token: 0x0400017B RID: 379
		internal const string RoleSqlProvider_description = "RoleSqlProvider_description";

		// Token: 0x0400017C RID: 380
		internal const string RoleAuthStoreProvider_description = "RoleAuthStoreProvider_description";

		// Token: 0x0400017D RID: 381
		internal const string RoleWindowsTokenProvider_description = "RoleWindowsTokenProvider_description";

		// Token: 0x0400017E RID: 382
		internal const string ProfileSqlProvider_description = "ProfileSqlProvider_description";

		// Token: 0x0400017F RID: 383
		internal const string Role_Principal_not_fully_constructed = "Role_Principal_not_fully_constructed";

		// Token: 0x04000180 RID: 384
		internal const string Only_one_connection_string_allowed = "Only_one_connection_string_allowed";

		// Token: 0x04000181 RID: 385
		internal const string Must_specify_connection_string_or_name = "Must_specify_connection_string_or_name";

		// Token: 0x04000182 RID: 386
		internal const string Cannot_use_integrated_security = "Cannot_use_integrated_security";

		// Token: 0x04000183 RID: 387
		internal const string Provider_application_name_too_long = "Provider_application_name_too_long";

		// Token: 0x04000184 RID: 388
		internal const string Provider_bad_password_format = "Provider_bad_password_format";

		// Token: 0x04000185 RID: 389
		internal const string Provider_can_not_retrieve_hashed_password = "Provider_can_not_retrieve_hashed_password";

		// Token: 0x04000186 RID: 390
		internal const string Provider_unrecognized_attribute = "Provider_unrecognized_attribute";

		// Token: 0x04000187 RID: 391
		internal const string Provider_can_not_decode_hashed_password = "Provider_can_not_decode_hashed_password";

		// Token: 0x04000188 RID: 392
		internal const string Profile_group_not_found = "Profile_group_not_found";

		// Token: 0x04000189 RID: 393
		internal const string Profile_not_enabled = "Profile_not_enabled";

		// Token: 0x0400018A RID: 394
		internal const string API_supported_for_current_user_only = "API_supported_for_current_user_only";

		// Token: 0x0400018B RID: 395
		internal const string API_failed_due_to_error = "API_failed_due_to_error";

		// Token: 0x0400018C RID: 396
		internal const string Profile_property_already_added = "Profile_property_already_added";

		// Token: 0x0400018D RID: 397
		internal const string Profile_provider_not_found = "Profile_provider_not_found";

		// Token: 0x0400018E RID: 398
		internal const string Can_not_issue_cookie_or_redirect = "Can_not_issue_cookie_or_redirect";

		// Token: 0x0400018F RID: 399
		internal const string Profile_default_provider_not_found = "Profile_default_provider_not_found";

		// Token: 0x04000190 RID: 400
		internal const string Value_must_be_boolean = "Value_must_be_boolean";

		// Token: 0x04000191 RID: 401
		internal const string Value_must_be_positive_integer = "Value_must_be_positive_integer";

		// Token: 0x04000192 RID: 402
		internal const string Value_must_be_non_negative_integer = "Value_must_be_non_negative_integer";

		// Token: 0x04000193 RID: 403
		internal const string Value_too_big = "Value_too_big";

		// Token: 0x04000194 RID: 404
		internal const string Profile_name_can_not_be_empty = "Profile_name_can_not_be_empty";

		// Token: 0x04000195 RID: 405
		internal const string Profile_name_can_not_contain_period = "Profile_name_can_not_contain_period";

		// Token: 0x04000196 RID: 406
		internal const string Provider_user_not_found = "Provider_user_not_found";

		// Token: 0x04000197 RID: 407
		internal const string Provider_role_not_found = "Provider_role_not_found";

		// Token: 0x04000198 RID: 408
		internal const string Provider_unknown_failure = "Provider_unknown_failure";

		// Token: 0x04000199 RID: 409
		internal const string Provider_role_already_exists = "Provider_role_already_exists";

		// Token: 0x0400019A RID: 410
		internal const string Profile_default_provider_not_specified = "Profile_default_provider_not_specified";

		// Token: 0x0400019B RID: 411
		internal const string API_not_supported_at_this_level = "API_not_supported_at_this_level";

		// Token: 0x0400019C RID: 412
		internal const string Profile_bad_name = "Profile_bad_name";

		// Token: 0x0400019D RID: 413
		internal const string Profile_bad_group = "Profile_bad_group";

		// Token: 0x0400019E RID: 414
		internal const string Def_membership_provider_not_specified = "Def_membership_provider_not_specified";

		// Token: 0x0400019F RID: 415
		internal const string Def_membership_provider_not_found = "Def_membership_provider_not_found";

		// Token: 0x040001A0 RID: 416
		internal const string Provider_Error = "Provider_Error";

		// Token: 0x040001A1 RID: 417
		internal const string Roles_feature_not_enabled = "Roles_feature_not_enabled";

		// Token: 0x040001A2 RID: 418
		internal const string Def_role_provider_not_specified = "Def_role_provider_not_specified";

		// Token: 0x040001A3 RID: 419
		internal const string Def_role_provider_not_found = "Def_role_provider_not_found";

		// Token: 0x040001A4 RID: 420
		internal const string Membership_PasswordRetrieval_not_supported = "Membership_PasswordRetrieval_not_supported";

		// Token: 0x040001A5 RID: 421
		internal const string Membership_UserNotFound = "Membership_UserNotFound";

		// Token: 0x040001A6 RID: 422
		internal const string Membership_WrongPassword = "Membership_WrongPassword";

		// Token: 0x040001A7 RID: 423
		internal const string Membership_WrongAnswer = "Membership_WrongAnswer";

		// Token: 0x040001A8 RID: 424
		internal const string Membership_InvalidPassword = "Membership_InvalidPassword";

		// Token: 0x040001A9 RID: 425
		internal const string Membership_InvalidQuestion = "Membership_InvalidQuestion";

		// Token: 0x040001AA RID: 426
		internal const string Membership_InvalidAnswer = "Membership_InvalidAnswer";

		// Token: 0x040001AB RID: 427
		internal const string Membership_InvalidUserName = "Membership_InvalidUserName";

		// Token: 0x040001AC RID: 428
		internal const string Membership_No_error = "Membership_No_error";

		// Token: 0x040001AD RID: 429
		internal const string Membership_InvalidEmail = "Membership_InvalidEmail";

		// Token: 0x040001AE RID: 430
		internal const string Membership_DuplicateUserName = "Membership_DuplicateUserName";

		// Token: 0x040001AF RID: 431
		internal const string Membership_DuplicateEmail = "Membership_DuplicateEmail";

		// Token: 0x040001B0 RID: 432
		internal const string Membership_UserRejected = "Membership_UserRejected";

		// Token: 0x040001B1 RID: 433
		internal const string Membership_InvalidProviderUserKey = "Membership_InvalidProviderUserKey";

		// Token: 0x040001B2 RID: 434
		internal const string Membership_DuplicateProviderUserKey = "Membership_DuplicateProviderUserKey";

		// Token: 0x040001B3 RID: 435
		internal const string Membership_AccountLockOut = "Membership_AccountLockOut";

		// Token: 0x040001B4 RID: 436
		internal const string Membership_Custom_Password_Validation_Failure = "Membership_Custom_Password_Validation_Failure";

		// Token: 0x040001B5 RID: 437
		internal const string MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength = "MinRequiredNonalphanumericCharacters_can_not_be_more_than_MinRequiredPasswordLength";

		// Token: 0x040001B6 RID: 438
		internal const string ADMembership_Description = "ADMembership_Description";

		// Token: 0x040001B7 RID: 439
		internal const string ADMembership_InvalidConnectionProtection = "ADMembership_InvalidConnectionProtection";

		// Token: 0x040001B8 RID: 440
		internal const string ADMembership_Connection_username_must_not_be_empty = "ADMembership_Connection_username_must_not_be_empty";

		// Token: 0x040001B9 RID: 441
		internal const string ADMembership_Connection_password_must_not_be_empty = "ADMembership_Connection_password_must_not_be_empty";

		// Token: 0x040001BA RID: 442
		internal const string ADMembership_Schema_mappings_must_not_be_empty = "ADMembership_Schema_mappings_must_not_be_empty";

		// Token: 0x040001BB RID: 443
		internal const string ADMembership_Username_and_password_reqd = "ADMembership_Username_and_password_reqd";

		// Token: 0x040001BC RID: 444
		internal const string ADMembership_PasswordReset_without_question_not_supported = "ADMembership_PasswordReset_without_question_not_supported";

		// Token: 0x040001BD RID: 445
		internal const string ADMembership_PasswordQuestionAnswerMapping_not_specified = "ADMembership_PasswordQuestionAnswerMapping_not_specified";

		// Token: 0x040001BE RID: 446
		internal const string ADMembership_Provider_not_initialized = "ADMembership_Provider_not_initialized";

		// Token: 0x040001BF RID: 447
		internal const string ADMembership_PasswordQ_not_supported = "ADMembership_PasswordQ_not_supported";

		// Token: 0x040001C0 RID: 448
		internal const string ADMembership_PasswordA_not_supported = "ADMembership_PasswordA_not_supported";

		// Token: 0x040001C1 RID: 449
		internal const string ADMembership_PasswordRetrieval_not_supported_AD = "ADMembership_PasswordRetrieval_not_supported_AD";

		// Token: 0x040001C2 RID: 450
		internal const string ADMembership_Username_mapping_invalid = "ADMembership_Username_mapping_invalid";

		// Token: 0x040001C3 RID: 451
		internal const string ADMembership_Username_mapping_invalid_ADAM = "ADMembership_Username_mapping_invalid_ADAM";

		// Token: 0x040001C4 RID: 452
		internal const string ADMembership_Wrong_syntax = "ADMembership_Wrong_syntax";

		// Token: 0x040001C5 RID: 453
		internal const string ADMembership_MappedAttribute_does_not_exist = "ADMembership_MappedAttribute_does_not_exist";

		// Token: 0x040001C6 RID: 454
		internal const string ADMembership_MappedAttribute_does_not_exist_on_user = "ADMembership_MappedAttribute_does_not_exist_on_user";

		// Token: 0x040001C7 RID: 455
		internal const string ADMembership_OnlyLdap_supported = "ADMembership_OnlyLdap_supported";

		// Token: 0x040001C8 RID: 456
		internal const string ADMembership_ServerlessADsPath_not_supported = "ADMembership_ServerlessADsPath_not_supported";

		// Token: 0x040001C9 RID: 457
		internal const string ADMembership_Secure_connection_not_established = "ADMembership_Secure_connection_not_established";

		// Token: 0x040001CA RID: 458
		internal const string ADMembership_Ssl_connection_not_established = "ADMembership_Ssl_connection_not_established";

		// Token: 0x040001CB RID: 459
		internal const string ADMembership_DefContainer_not_specified = "ADMembership_DefContainer_not_specified";

		// Token: 0x040001CC RID: 460
		internal const string ADMembership_DefContainer_does_not_exist = "ADMembership_DefContainer_does_not_exist";

		// Token: 0x040001CD RID: 461
		internal const string ADMembership_Container_must_be_specified = "ADMembership_Container_must_be_specified";

		// Token: 0x040001CE RID: 462
		internal const string ADMembership_Valid_Targets = "ADMembership_Valid_Targets";

		// Token: 0x040001CF RID: 463
		internal const string ADMembership_OnlineUsers_not_supported = "ADMembership_OnlineUsers_not_supported";

		// Token: 0x040001D0 RID: 464
		internal const string ADMembership_UserProperty_not_supported = "ADMembership_UserProperty_not_supported";

		// Token: 0x040001D1 RID: 465
		internal const string ADMembership_Provider_SearchMethods_not_supported = "ADMembership_Provider_SearchMethods_not_supported";

		// Token: 0x040001D2 RID: 466
		internal const string ADMembership_No_ADAM_Partition = "ADMembership_No_ADAM_Partition";

		// Token: 0x040001D3 RID: 467
		internal const string ADMembership_Setting_UserId_not_supported = "ADMembership_Setting_UserId_not_supported";

		// Token: 0x040001D4 RID: 468
		internal const string ADMembership_Default_Creds_not_supported = "ADMembership_Default_Creds_not_supported";

		// Token: 0x040001D5 RID: 469
		internal const string ADMembership_Container_not_superior = "ADMembership_Container_not_superior";

		// Token: 0x040001D6 RID: 470
		internal const string ADMembership_Container_does_not_exist = "ADMembership_Container_does_not_exist";

		// Token: 0x040001D7 RID: 471
		internal const string ADMembership_Property_not_found_on_object = "ADMembership_Property_not_found_on_object";

		// Token: 0x040001D8 RID: 472
		internal const string ADMembership_Property_not_found = "ADMembership_Property_not_found";

		// Token: 0x040001D9 RID: 473
		internal const string ADMembership_BadPasswordAnswerMappings_not_specified = "ADMembership_BadPasswordAnswerMappings_not_specified";

		// Token: 0x040001DA RID: 474
		internal const string ADMembership_Unknown_Error = "ADMembership_Unknown_Error";

		// Token: 0x040001DB RID: 475
		internal const string ADMembership_GCPortsNotSupported = "ADMembership_GCPortsNotSupported";

		// Token: 0x040001DC RID: 476
		internal const string ADMembership_attribute_not_single_valued = "ADMembership_attribute_not_single_valued";

		// Token: 0x040001DD RID: 477
		internal const string ADMembership_mapping_not_unique = "ADMembership_mapping_not_unique";

		// Token: 0x040001DE RID: 478
		internal const string ADMembership_InvalidProviderUserKey = "ADMembership_InvalidProviderUserKey";

		// Token: 0x040001DF RID: 479
		internal const string ADMembership_unable_to_contact_domain = "ADMembership_unable_to_contact_domain";

		// Token: 0x040001E0 RID: 480
		internal const string ADMembership_unable_to_set_password_port = "ADMembership_unable_to_set_password_port";

		// Token: 0x040001E1 RID: 481
		internal const string ADMembership_invalid_path = "ADMembership_invalid_path";

		// Token: 0x040001E2 RID: 482
		internal const string ADMembership_Setting_ApplicationName_not_supported = "ADMembership_Setting_ApplicationName_not_supported";

		// Token: 0x040001E3 RID: 483
		internal const string ADMembership_Parameter_too_long = "ADMembership_Parameter_too_long";

		// Token: 0x040001E4 RID: 484
		internal const string ADMembership_No_secure_conn_for_password = "ADMembership_No_secure_conn_for_password";

		// Token: 0x040001E5 RID: 485
		internal const string ADMembership_Generated_password_not_complex = "ADMembership_Generated_password_not_complex";

		// Token: 0x040001E6 RID: 486
		internal const string ADMembership_UPN_contains_backslash = "ADMembership_UPN_contains_backslash";

		// Token: 0x040001E7 RID: 487
		internal const string Windows_Token_API_not_supported = "Windows_Token_API_not_supported";

		// Token: 0x040001E8 RID: 488
		internal const string Parameter_can_not_contain_comma = "Parameter_can_not_contain_comma";

		// Token: 0x040001E9 RID: 489
		internal const string Parameter_can_not_be_empty = "Parameter_can_not_be_empty";

		// Token: 0x040001EA RID: 490
		internal const string Parameter_too_long = "Parameter_too_long";

		// Token: 0x040001EB RID: 491
		internal const string Parameter_array_empty = "Parameter_array_empty";

		// Token: 0x040001EC RID: 492
		internal const string Parameter_collection_empty = "Parameter_collection_empty";

		// Token: 0x040001ED RID: 493
		internal const string Parameter_duplicate_array_element = "Parameter_duplicate_array_element";

		// Token: 0x040001EE RID: 494
		internal const string Membership_password_too_long = "Membership_password_too_long";

		// Token: 0x040001EF RID: 495
		internal const string Provider_this_user_not_found = "Provider_this_user_not_found";

		// Token: 0x040001F0 RID: 496
		internal const string Provider_this_user_already_in_role = "Provider_this_user_already_in_role";

		// Token: 0x040001F1 RID: 497
		internal const string Provider_this_user_already_not_in_role = "Provider_this_user_already_not_in_role";

		// Token: 0x040001F2 RID: 498
		internal const string SaveAs_requires_rooted_path = "SaveAs_requires_rooted_path";

		// Token: 0x040001F3 RID: 499
		internal const string Connection_name_not_specified = "Connection_name_not_specified";

		// Token: 0x040001F4 RID: 500
		internal const string Connection_string_not_found = "Connection_string_not_found";

		// Token: 0x040001F5 RID: 501
		internal const string AppSetting_not_found = "AppSetting_not_found";

		// Token: 0x040001F6 RID: 502
		internal const string AppSetting_not_convertible = "AppSetting_not_convertible";

		// Token: 0x040001F7 RID: 503
		internal const string Provider_must_implement_type = "Provider_must_implement_type";

		// Token: 0x040001F8 RID: 504
		internal const string Feature_not_supported_at_this_level = "Feature_not_supported_at_this_level";

		// Token: 0x040001F9 RID: 505
		internal const string Annoymous_id_module_not_enabled = "Annoymous_id_module_not_enabled";

		// Token: 0x040001FA RID: 506
		internal const string Anonymous_ClearAnonymousIdentifierNotSupported = "Anonymous_ClearAnonymousIdentifierNotSupported";

		// Token: 0x040001FB RID: 507
		internal const string Anonymous_id_too_long = "Anonymous_id_too_long";

		// Token: 0x040001FC RID: 508
		internal const string Anonymous_id_too_long_2 = "Anonymous_id_too_long_2";

		// Token: 0x040001FD RID: 509
		internal const string Profile_could_not_create_type = "Profile_could_not_create_type";

		// Token: 0x040001FE RID: 510
		internal const string DataAccessError_CanNotCreateDataDir_Title = "DataAccessError_CanNotCreateDataDir_Title";

		// Token: 0x040001FF RID: 511
		internal const string DataAccessError_CanNotCreateDataDir_Description = "DataAccessError_CanNotCreateDataDir_Description";

		// Token: 0x04000200 RID: 512
		internal const string DataAccessError_CanNotCreateDataDir_Description_2 = "DataAccessError_CanNotCreateDataDir_Description_2";

		// Token: 0x04000201 RID: 513
		internal const string DataAccessError_MiscSectionTitle = "DataAccessError_MiscSectionTitle";

		// Token: 0x04000202 RID: 514
		internal const string DataAccessError_MiscSection_1 = "DataAccessError_MiscSection_1";

		// Token: 0x04000203 RID: 515
		internal const string DataAccessError_MiscSection_2 = "DataAccessError_MiscSection_2";

		// Token: 0x04000204 RID: 516
		internal const string DataAccessError_MiscSection_2_CanNotCreateDataDir = "DataAccessError_MiscSection_2_CanNotCreateDataDir";

		// Token: 0x04000205 RID: 517
		internal const string DataAccessError_MiscSection_2_CanNotWriteToDBFile_a = "DataAccessError_MiscSection_2_CanNotWriteToDBFile_a";

		// Token: 0x04000206 RID: 518
		internal const string DataAccessError_MiscSection_2_CanNotWriteToDBFile_b = "DataAccessError_MiscSection_2_CanNotWriteToDBFile_b";

		// Token: 0x04000207 RID: 519
		internal const string DataAccessError_MiscSection_3 = "DataAccessError_MiscSection_3";

		// Token: 0x04000208 RID: 520
		internal const string DataAccessError_MiscSection_4 = "DataAccessError_MiscSection_4";

		// Token: 0x04000209 RID: 521
		internal const string DataAccessError_MiscSection_4_2 = "DataAccessError_MiscSection_4_2";

		// Token: 0x0400020A RID: 522
		internal const string DataAccessError_MiscSection_ClickAdd = "DataAccessError_MiscSection_ClickAdd";

		// Token: 0x0400020B RID: 523
		internal const string DataAccessError_MiscSection_ClickOK = "DataAccessError_MiscSection_ClickOK";

		// Token: 0x0400020C RID: 524
		internal const string DataAccessError_MiscSection_5 = "DataAccessError_MiscSection_5";

		// Token: 0x0400020D RID: 525
		internal const string SqlExpressError_CanNotWriteToDataDir_Title = "SqlExpressError_CanNotWriteToDataDir_Title";

		// Token: 0x0400020E RID: 526
		internal const string SqlExpressError_CanNotWriteToDbfFile_Title = "SqlExpressError_CanNotWriteToDbfFile_Title";

		// Token: 0x0400020F RID: 527
		internal const string SqlExpressError_CanNotWriteToDataDir_Description = "SqlExpressError_CanNotWriteToDataDir_Description";

		// Token: 0x04000210 RID: 528
		internal const string SqlExpressError_CanNotWriteToDbfFile_Description = "SqlExpressError_CanNotWriteToDbfFile_Description";

		// Token: 0x04000211 RID: 529
		internal const string SqlExpressError_CanNotWriteToDataDir_Description_2 = "SqlExpressError_CanNotWriteToDataDir_Description_2";

		// Token: 0x04000212 RID: 530
		internal const string SqlExpressError_CanNotWriteToDbfFile_Description_2 = "SqlExpressError_CanNotWriteToDbfFile_Description_2";

		// Token: 0x04000213 RID: 531
		internal const string SqlExpressError_Description_1 = "SqlExpressError_Description_1";

		// Token: 0x04000214 RID: 532
		internal const string Membership_password_length_incorrect = "Membership_password_length_incorrect";

		// Token: 0x04000215 RID: 533
		internal const string Membership_min_required_non_alphanumeric_characters_incorrect = "Membership_min_required_non_alphanumeric_characters_incorrect";

		// Token: 0x04000216 RID: 534
		internal const string Membership_more_than_one_user_with_email = "Membership_more_than_one_user_with_email";

		// Token: 0x04000217 RID: 535
		internal const string Password_too_short = "Password_too_short";

		// Token: 0x04000218 RID: 536
		internal const string Password_need_more_non_alpha_numeric_chars = "Password_need_more_non_alpha_numeric_chars";

		// Token: 0x04000219 RID: 537
		internal const string Password_does_not_match_regular_expression = "Password_does_not_match_regular_expression";

		// Token: 0x0400021A RID: 538
		internal const string Not_configured_to_support_password_resets = "Not_configured_to_support_password_resets";

		// Token: 0x0400021B RID: 539
		internal const string Property_not_serializable = "Property_not_serializable";

		// Token: 0x0400021C RID: 540
		internal const string Connection_not_secure_creating_secure_cookie = "Connection_not_secure_creating_secure_cookie";

		// Token: 0x0400021D RID: 541
		internal const string Profile_anonoymous_not_allowed_to_set_property = "Profile_anonoymous_not_allowed_to_set_property";

		// Token: 0x0400021E RID: 542
		internal const string AuthStore_Application_not_found = "AuthStore_Application_not_found";

		// Token: 0x0400021F RID: 543
		internal const string AuthStore_Scope_not_found = "AuthStore_Scope_not_found";

		// Token: 0x04000220 RID: 544
		internal const string AuthStoreNotInstalled_Title = "AuthStoreNotInstalled_Title";

		// Token: 0x04000221 RID: 545
		internal const string AuthStoreNotInstalled_Description = "AuthStoreNotInstalled_Description";

		// Token: 0x04000222 RID: 546
		internal const string AuthStore_policy_file_not_found = "AuthStore_policy_file_not_found";

		// Token: 0x04000223 RID: 547
		internal const string Wrong_profile_base_type = "Wrong_profile_base_type";

		// Token: 0x04000224 RID: 548
		internal const string Command_not_recognized = "Command_not_recognized";

		// Token: 0x04000225 RID: 549
		internal const string Configuration_for_path_not_found = "Configuration_for_path_not_found";

		// Token: 0x04000226 RID: 550
		internal const string Configuration_for_physical_path_not_found = "Configuration_for_physical_path_not_found";

		// Token: 0x04000227 RID: 551
		internal const string Configuration_for_machine_config_not_found = "Configuration_for_machine_config_not_found";

		// Token: 0x04000228 RID: 552
		internal const string Configuration_Section_not_found = "Configuration_Section_not_found";

		// Token: 0x04000229 RID: 553
		internal const string RSA_Key_Container_not_found = "RSA_Key_Container_not_found";

		// Token: 0x0400022A RID: 554
		internal const string RSA_Key_Container_access_denied = "RSA_Key_Container_access_denied";

		// Token: 0x0400022B RID: 555
		internal const string RSA_Key_Container_already_exists = "RSA_Key_Container_already_exists";

		// Token: 0x0400022C RID: 556
		internal const string SqlError_Connection_String = "SqlError_Connection_String";

		// Token: 0x0400022D RID: 557
		internal const string SqlExpress_MDF_File_Auto_Creation_MiscSectionTitle = "SqlExpress_MDF_File_Auto_Creation_MiscSectionTitle";

		// Token: 0x0400022E RID: 558
		internal const string SqlExpress_MDF_File_Auto_Creation = "SqlExpress_MDF_File_Auto_Creation";

		// Token: 0x0400022F RID: 559
		internal const string SqlExpress_MDF_File_Auto_Creation_1 = "SqlExpress_MDF_File_Auto_Creation_1";

		// Token: 0x04000230 RID: 560
		internal const string SqlExpress_MDF_File_Auto_Creation_2 = "SqlExpress_MDF_File_Auto_Creation_2";

		// Token: 0x04000231 RID: 561
		internal const string SqlExpress_MDF_File_Auto_Creation_3 = "SqlExpress_MDF_File_Auto_Creation_3";

		// Token: 0x04000232 RID: 562
		internal const string SqlExpress_MDF_File_Auto_Creation_4 = "SqlExpress_MDF_File_Auto_Creation_4";

		// Token: 0x04000233 RID: 563
		internal const string SqlExpress_file_not_found_in_connection_string = "SqlExpress_file_not_found_in_connection_string";

		// Token: 0x04000234 RID: 564
		internal const string SqlExpress_file_not_found = "SqlExpress_file_not_found";

		// Token: 0x04000235 RID: 565
		internal const string Invalid_value_for_sessionstate_stateConnectionString = "Invalid_value_for_sessionstate_stateConnectionString";

		// Token: 0x04000236 RID: 566
		internal const string No_database_allowed_in_sqlConnectionString = "No_database_allowed_in_sqlConnectionString";

		// Token: 0x04000237 RID: 567
		internal const string No_database_allowed_in_sql_partition_resolver_string = "No_database_allowed_in_sql_partition_resolver_string";

		// Token: 0x04000238 RID: 568
		internal const string Error_parsing_session_sqlConnectionString = "Error_parsing_session_sqlConnectionString";

		// Token: 0x04000239 RID: 569
		internal const string Error_parsing_sql_partition_resolver_string = "Error_parsing_sql_partition_resolver_string";

		// Token: 0x0400023A RID: 570
		internal const string Timeout_must_be_positive = "Timeout_must_be_positive";

		// Token: 0x0400023B RID: 571
		internal const string Cant_make_session_request = "Cant_make_session_request";

		// Token: 0x0400023C RID: 572
		internal const string Cant_make_session_request_partition_resolver = "Cant_make_session_request_partition_resolver";

		// Token: 0x0400023D RID: 573
		internal const string Need_v2_State_Server = "Need_v2_State_Server";

		// Token: 0x0400023E RID: 574
		internal const string Need_v2_State_Server_partition_resolver = "Need_v2_State_Server_partition_resolver";

		// Token: 0x0400023F RID: 575
		internal const string Cant_connect_sql_session_database = "Cant_connect_sql_session_database";

		// Token: 0x04000240 RID: 576
		internal const string Cant_connect_sql_session_database_partition_resolver = "Cant_connect_sql_session_database_partition_resolver";

		// Token: 0x04000241 RID: 577
		internal const string Login_failed_sql_session_database = "Login_failed_sql_session_database";

		// Token: 0x04000242 RID: 578
		internal const string Bad_partition_resolver_connection_string = "Bad_partition_resolver_connection_string";

		// Token: 0x04000243 RID: 579
		internal const string Bad_state_server_request = "Bad_state_server_request";

		// Token: 0x04000244 RID: 580
		internal const string Bad_state_server_request_partition_resolver = "Bad_state_server_request_partition_resolver";

		// Token: 0x04000245 RID: 581
		internal const string State_Server_detailed_error = "State_Server_detailed_error";

		// Token: 0x04000246 RID: 582
		internal const string State_Server_detailed_error_phase0 = "State_Server_detailed_error_phase0";

		// Token: 0x04000247 RID: 583
		internal const string State_Server_detailed_error_phase1 = "State_Server_detailed_error_phase1";

		// Token: 0x04000248 RID: 584
		internal const string State_Server_detailed_error_phase2 = "State_Server_detailed_error_phase2";

		// Token: 0x04000249 RID: 585
		internal const string State_Server_detailed_error_phase3 = "State_Server_detailed_error_phase3";

		// Token: 0x0400024A RID: 586
		internal const string Error_parsing_state_server_partition_resolver_string = "Error_parsing_state_server_partition_resolver_string";

		// Token: 0x0400024B RID: 587
		internal const string SessionIDManager_uninit = "SessionIDManager_uninit";

		// Token: 0x0400024C RID: 588
		internal const string SessionIDManager_InitializeRequest_not_called = "SessionIDManager_InitializeRequest_not_called";

		// Token: 0x0400024D RID: 589
		internal const string Cant_save_session_id_because_response_was_flushed = "Cant_save_session_id_because_response_was_flushed";

		// Token: 0x0400024E RID: 590
		internal const string Cant_save_session_id_because_id_is_invalid = "Cant_save_session_id_because_id_is_invalid";

		// Token: 0x0400024F RID: 591
		internal const string Cant_serialize_session_state = "Cant_serialize_session_state";

		// Token: 0x04000250 RID: 592
		internal const string Null_value_for_SessionStateItemCollection = "Null_value_for_SessionStateItemCollection";

		// Token: 0x04000251 RID: 593
		internal const string Session_id_too_long = "Session_id_too_long";

		// Token: 0x04000252 RID: 594
		internal const string Need_v2_SQL_Server = "Need_v2_SQL_Server";

		// Token: 0x04000253 RID: 595
		internal const string Need_v2_SQL_Server_partition_resolver = "Need_v2_SQL_Server_partition_resolver";

		// Token: 0x04000254 RID: 596
		internal const string Cant_have_multiple_session_module = "Cant_have_multiple_session_module";

		// Token: 0x04000255 RID: 597
		internal const string Missing_session_custom_provider = "Missing_session_custom_provider";

		// Token: 0x04000256 RID: 598
		internal const string Invalid_session_custom_provider = "Invalid_session_custom_provider";

		// Token: 0x04000257 RID: 599
		internal const string Invalid_session_state = "Invalid_session_state";

		// Token: 0x04000258 RID: 600
		internal const string Invalid_cache_based_session_timeout = "Invalid_cache_based_session_timeout";

		// Token: 0x04000259 RID: 601
		internal const string Cant_use_partition_resolve = "Cant_use_partition_resolve";

		// Token: 0x0400025A RID: 602
		internal const string Previous_Page_Not_Authorized = "Previous_Page_Not_Authorized";

		// Token: 0x0400025B RID: 603
		internal const string Empty_path_has_no_directory = "Empty_path_has_no_directory";

		// Token: 0x0400025C RID: 604
		internal const string Path_must_be_rooted = "Path_must_be_rooted";

		// Token: 0x0400025D RID: 605
		internal const string Cannot_exit_up_top_directory = "Cannot_exit_up_top_directory";

		// Token: 0x0400025E RID: 606
		internal const string Physical_path_not_allowed = "Physical_path_not_allowed";

		// Token: 0x0400025F RID: 607
		internal const string Invalid_vpath = "Invalid_vpath";

		// Token: 0x04000260 RID: 608
		internal const string Cannot_access_AspCompat = "Cannot_access_AspCompat";

		// Token: 0x04000261 RID: 609
		internal const string Apartment_component_not_allowed = "Apartment_component_not_allowed";

		// Token: 0x04000262 RID: 610
		internal const string Error_onpagestart = "Error_onpagestart";

		// Token: 0x04000263 RID: 611
		internal const string Cannot_execute_transacted_code = "Cannot_execute_transacted_code";

		// Token: 0x04000264 RID: 612
		internal const string Cannot_post_workitem = "Cannot_post_workitem";

		// Token: 0x04000265 RID: 613
		internal const string Cannot_call_ISAPI_functions = "Cannot_call_ISAPI_functions";

		// Token: 0x04000266 RID: 614
		internal const string Bad_attachment = "Bad_attachment";

		// Token: 0x04000267 RID: 615
		internal const string Wrong_SimpleWorkerRequest = "Wrong_SimpleWorkerRequest";

		// Token: 0x04000268 RID: 616
		internal const string Atio2BadString = "Atio2BadString";

		// Token: 0x04000269 RID: 617
		internal const string MakeMonthBadString = "MakeMonthBadString";

		// Token: 0x0400026A RID: 618
		internal const string UtilParseDateTimeBad = "UtilParseDateTimeBad";

		// Token: 0x0400026B RID: 619
		internal const string SmtpMail_not_supported_on_Win7_and_higher = "SmtpMail_not_supported_on_Win7_and_higher";

		// Token: 0x0400026C RID: 620
		internal const string Illegal_special_dir = "Illegal_special_dir";

		// Token: 0x0400026D RID: 621
		internal const string Illegal_precomp_dir = "Illegal_precomp_dir";

		// Token: 0x0400026E RID: 622
		internal const string Precomp_stub_file = "Precomp_stub_file";

		// Token: 0x0400026F RID: 623
		internal const string Already_precomp = "Already_precomp";

		// Token: 0x04000270 RID: 624
		internal const string Cant_delete_dir = "Cant_delete_dir";

		// Token: 0x04000271 RID: 625
		internal const string Dir_not_empty = "Dir_not_empty";

		// Token: 0x04000272 RID: 626
		internal const string Dir_not_empty_not_precomp = "Dir_not_empty_not_precomp";

		// Token: 0x04000273 RID: 627
		internal const string Cant_update_precompiled_app = "Cant_update_precompiled_app";

		// Token: 0x04000274 RID: 628
		internal const string Too_early_for_webfile = "Too_early_for_webfile";

		// Token: 0x04000275 RID: 629
		internal const string Bar_dir_in_precompiled_app = "Bar_dir_in_precompiled_app";

		// Token: 0x04000276 RID: 630
		internal const string Assembly_already_loaded = "Assembly_already_loaded";

		// Token: 0x04000277 RID: 631
		internal const string Success_precompile = "Success_precompile";

		// Token: 0x04000278 RID: 632
		internal const string Profile_not_precomped = "Profile_not_precomped";

		// Token: 0x04000279 RID: 633
		internal const string Both_culture_and_language = "Both_culture_and_language";

		// Token: 0x0400027A RID: 634
		internal const string Inconsistent_language = "Inconsistent_language";

		// Token: 0x0400027B RID: 635
		internal const string GetGeneratedSourceFile_Directory_Only = "GetGeneratedSourceFile_Directory_Only";

		// Token: 0x0400027C RID: 636
		internal const string Duplicate_appinitialize = "Duplicate_appinitialize";

		// Token: 0x0400027D RID: 637
		internal const string Virtual_codedir = "Virtual_codedir";

		// Token: 0x0400027E RID: 638
		internal const string Unknown_buildprovider_extension = "Unknown_buildprovider_extension";

		// Token: 0x0400027F RID: 639
		internal const string Directory_progress = "Directory_progress";

		// Token: 0x04000280 RID: 640
		internal const string Bad_Base_Class_In_Code_File = "Bad_Base_Class_In_Code_File";

		// Token: 0x04000281 RID: 641
		internal const string Cant_use_default_items_and_filtered_collection = "Cant_use_default_items_and_filtered_collection";

		// Token: 0x04000282 RID: 642
		internal const string Children_not_supported_on_not_controls = "Children_not_supported_on_not_controls";

		// Token: 0x04000283 RID: 643
		internal const string Code_not_supported_on_not_controls = "Code_not_supported_on_not_controls";

		// Token: 0x04000284 RID: 644
		internal const string Code_not_allowed = "Code_not_allowed";

		// Token: 0x04000285 RID: 645
		internal const string Compilmode_not_allowed = "Compilmode_not_allowed";

		// Token: 0x04000286 RID: 646
		internal const string Include_not_allowed = "Include_not_allowed";

		// Token: 0x04000287 RID: 647
		internal const string Attrib_not_allowed = "Attrib_not_allowed";

		// Token: 0x04000288 RID: 648
		internal const string Directive_not_allowed = "Directive_not_allowed";

		// Token: 0x04000289 RID: 649
		internal const string Event_not_allowed = "Event_not_allowed";

		// Token: 0x0400028A RID: 650
		internal const string Literal_content_not_allowed = "Literal_content_not_allowed";

		// Token: 0x0400028B RID: 651
		internal const string Literal_content_not_match_property = "Literal_content_not_match_property";

		// Token: 0x0400028C RID: 652
		internal const string Too_many_controls = "Too_many_controls";

		// Token: 0x0400028D RID: 653
		internal const string Too_many_dependencies = "Too_many_dependencies";

		// Token: 0x0400028E RID: 654
		internal const string Too_many_direct_dependencies = "Too_many_direct_dependencies";

		// Token: 0x0400028F RID: 655
		internal const string Invalid_type = "Invalid_type";

		// Token: 0x04000290 RID: 656
		internal const string Assembly_not_compiled = "Assembly_not_compiled";

		// Token: 0x04000291 RID: 657
		internal const string Not_a_src_file = "Not_a_src_file";

		// Token: 0x04000292 RID: 658
		internal const string Ambiguous_type = "Ambiguous_type";

		// Token: 0x04000293 RID: 659
		internal const string Unsupported_filename = "Unsupported_filename";

		// Token: 0x04000294 RID: 660
		internal const string Cannot_convert_from_to = "Cannot_convert_from_to";

		// Token: 0x04000295 RID: 661
		internal const string Object_tag_must_have_id = "Object_tag_must_have_id";

		// Token: 0x04000296 RID: 662
		internal const string Invalid_scope = "Invalid_scope";

		// Token: 0x04000297 RID: 663
		internal const string Invalid_progid = "Invalid_progid";

		// Token: 0x04000298 RID: 664
		internal const string Invalid_clsid = "Invalid_clsid";

		// Token: 0x04000299 RID: 665
		internal const string Object_tag_must_have_class_classid_or_progid = "Object_tag_must_have_class_classid_or_progid";

		// Token: 0x0400029A RID: 666
		internal const string Session_not_enabled = "Session_not_enabled";

		// Token: 0x0400029B RID: 667
		internal const string Page_ControlState_ControlCannotBeNull = "Page_ControlState_ControlCannotBeNull";

		// Token: 0x0400029C RID: 668
		internal const string Page_theme_not_found = "Page_theme_not_found";

		// Token: 0x0400029D RID: 669
		internal const string Page_theme_invalid_name = "Page_theme_invalid_name";

		// Token: 0x0400029E RID: 670
		internal const string Page_theme_default_theme_already_defined = "Page_theme_default_theme_already_defined";

		// Token: 0x0400029F RID: 671
		internal const string Page_theme_skinID_already_defined = "Page_theme_skinID_already_defined";

		// Token: 0x040002A0 RID: 672
		internal const string Page_theme_requires_page_header = "Page_theme_requires_page_header";

		// Token: 0x040002A1 RID: 673
		internal const string Page_theme_only_controls_allowed = "Page_theme_only_controls_allowed";

		// Token: 0x040002A2 RID: 674
		internal const string Page_theme_skin_file = "Page_theme_skin_file";

		// Token: 0x040002A3 RID: 675
		internal const string Page_Title_Requires_Head = "Page_Title_Requires_Head";

		// Token: 0x040002A4 RID: 676
		internal const string DataBoundLiterals_cant_bind = "DataBoundLiterals_cant_bind";

		// Token: 0x040002A5 RID: 677
		internal const string TwoWayBinding_requires_ID = "TwoWayBinding_requires_ID";

		// Token: 0x040002A6 RID: 678
		internal const string NoCompileBinding_requires_ID = "NoCompileBinding_requires_ID";

		// Token: 0x040002A7 RID: 679
		internal const string BadlyFormattedBind = "BadlyFormattedBind";

		// Token: 0x040002A8 RID: 680
		internal const string Property_readonly = "Property_readonly";

		// Token: 0x040002A9 RID: 681
		internal const string Property_theme_disabled = "Property_theme_disabled";

		// Token: 0x040002AA RID: 682
		internal const string Type_theme_disabled = "Type_theme_disabled";

		// Token: 0x040002AB RID: 683
		internal const string Collection_readonly_Codeblocks = "Collection_readonly_Codeblocks";

		// Token: 0x040002AC RID: 684
		internal const string Parent_collections_readonly = "Parent_collections_readonly";

		// Token: 0x040002AD RID: 685
		internal const string Property_Not_Persistable = "Property_Not_Persistable";

		// Token: 0x040002AE RID: 686
		internal const string Property_Not_Supported = "Property_Not_Supported";

		// Token: 0x040002AF RID: 687
		internal const string Property_Not_ClsCompliant = "Property_Not_ClsCompliant";

		// Token: 0x040002B0 RID: 688
		internal const string Property_Set_Not_Supported = "Property_Set_Not_Supported";

		// Token: 0x040002B1 RID: 689
		internal const string ControlBuilder_InvalidLocalizeValue = "ControlBuilder_InvalidLocalizeValue";

		// Token: 0x040002B2 RID: 690
		internal const string meta_localize_error = "meta_localize_error";

		// Token: 0x040002B3 RID: 691
		internal const string meta_reskey_notallowed = "meta_reskey_notallowed";

		// Token: 0x040002B4 RID: 692
		internal const string meta_localize_notallowed = "meta_localize_notallowed";

		// Token: 0x040002B5 RID: 693
		internal const string Invalid_enum_value = "Invalid_enum_value";

		// Token: 0x040002B6 RID: 694
		internal const string Type_not_creatable_from_string = "Type_not_creatable_from_string";

		// Token: 0x040002B7 RID: 695
		internal const string Invalid_collection_item_type = "Invalid_collection_item_type";

		// Token: 0x040002B8 RID: 696
		internal const string Invalid_template_container = "Invalid_template_container";

		// Token: 0x040002B9 RID: 697
		internal const string Event_handler_cant_be_empty = "Event_handler_cant_be_empty";

		// Token: 0x040002BA RID: 698
		internal const string Events_cant_be_filtered = "Events_cant_be_filtered";

		// Token: 0x040002BB RID: 699
		internal const string Type_doesnt_have_property = "Type_doesnt_have_property";

		// Token: 0x040002BC RID: 700
		internal const string Property_doesnt_have_property = "Property_doesnt_have_property";

		// Token: 0x040002BD RID: 701
		internal const string MasterPage_Multiple_content = "MasterPage_Multiple_content";

		// Token: 0x040002BE RID: 702
		internal const string MasterPage_doesnt_have_contentplaceholder = "MasterPage_doesnt_have_contentplaceholder";

		// Token: 0x040002BF RID: 703
		internal const string MasterPage_MasterPageFile = "MasterPage_MasterPageFile";

		// Token: 0x040002C0 RID: 704
		internal const string MasterPage_MasterPage = "MasterPage_MasterPage";

		// Token: 0x040002C1 RID: 705
		internal const string MasterPage_Circular_Master_Not_Allowed = "MasterPage_Circular_Master_Not_Allowed";

		// Token: 0x040002C2 RID: 706
		internal const string MasterPage_Cannot_ApplyTo_ReadOnly_Collection = "MasterPage_Cannot_ApplyTo_ReadOnly_Collection";

		// Token: 0x040002C3 RID: 707
		internal const string Only_Content_supported_on_content_page = "Only_Content_supported_on_content_page";

		// Token: 0x040002C4 RID: 708
		internal const string Content_allowed_in_top_level_only = "Content_allowed_in_top_level_only";

		// Token: 0x040002C5 RID: 709
		internal const string Content_only_allowed_in_content_page = "Content_only_allowed_in_content_page";

		// Token: 0x040002C6 RID: 710
		internal const string Content_only_one_contentPlaceHolderID_allowed = "Content_only_one_contentPlaceHolderID_allowed";

		// Token: 0x040002C7 RID: 711
		internal const string Invalid_master_base = "Invalid_master_base";

		// Token: 0x040002C8 RID: 712
		internal const string Invalid_typeless_reference = "Invalid_typeless_reference";

		// Token: 0x040002C9 RID: 713
		internal const string Bad_masterPage_ext = "Bad_masterPage_ext";

		// Token: 0x040002CA RID: 714
		internal const string Illegal_Device = "Illegal_Device";

		// Token: 0x040002CB RID: 715
		internal const string Illegal_Resource_Builder = "Illegal_Resource_Builder";

		// Token: 0x040002CC RID: 716
		internal const string Too_many_filters = "Too_many_filters";

		// Token: 0x040002CD RID: 717
		internal const string Device_unsupported_in_directive = "Device_unsupported_in_directive";

		// Token: 0x040002CE RID: 718
		internal const string Cannot_add_value_not_collection = "Cannot_add_value_not_collection";

		// Token: 0x040002CF RID: 719
		internal const string ControlBuilder_CollectionHasNoAddMethod = "ControlBuilder_CollectionHasNoAddMethod";

		// Token: 0x040002D0 RID: 720
		internal const string Cannot_set_property = "Cannot_set_property";

		// Token: 0x040002D1 RID: 721
		internal const string Cannot_set_recursive_skin = "Cannot_set_recursive_skin";

		// Token: 0x040002D2 RID: 722
		internal const string Cannot_evaluate_expression = "Cannot_evaluate_expression";

		// Token: 0x040002D3 RID: 723
		internal const string Cannot_init = "Cannot_init";

		// Token: 0x040002D4 RID: 724
		internal const string Unexpected_Directory = "Unexpected_Directory";

		// Token: 0x040002D5 RID: 725
		internal const string Circular_include = "Circular_include";

		// Token: 0x040002D6 RID: 726
		internal const string Unexpected_eof_looking_for_tag = "Unexpected_eof_looking_for_tag";

		// Token: 0x040002D7 RID: 727
		internal const string Invalid_app_file_content = "Invalid_app_file_content";

		// Token: 0x040002D8 RID: 728
		internal const string Invalid_use_of_config_uc = "Invalid_use_of_config_uc";

		// Token: 0x040002D9 RID: 729
		internal const string Page_scope_in_global_asax = "Page_scope_in_global_asax";

		// Token: 0x040002DA RID: 730
		internal const string App_session_only_valid_in_global_asax = "App_session_only_valid_in_global_asax";

		// Token: 0x040002DB RID: 731
		internal const string Multiple_forms_not_allowed = "Multiple_forms_not_allowed";

		// Token: 0x040002DC RID: 732
		internal const string Postback_ctrl_not_found = "Postback_ctrl_not_found";

		// Token: 0x040002DD RID: 733
		internal const string Ctrl_not_data_handler = "Ctrl_not_data_handler";

		// Token: 0x040002DE RID: 734
		internal const string Transfer_not_allowed_in_callback = "Transfer_not_allowed_in_callback";

		// Token: 0x040002DF RID: 735
		internal const string Redirect_not_allowed_in_callback = "Redirect_not_allowed_in_callback";

		// Token: 0x040002E0 RID: 736
		internal const string Script_tag_without_src_must_have_content = "Script_tag_without_src_must_have_content";

		// Token: 0x040002E1 RID: 737
		internal const string Unknown_server_tag = "Unknown_server_tag";

		// Token: 0x040002E2 RID: 738
		internal const string Ambiguous_server_tag = "Ambiguous_server_tag";

		// Token: 0x040002E3 RID: 739
		internal const string Invalid_type_for_input_tag = "Invalid_type_for_input_tag";

		// Token: 0x040002E4 RID: 740
		internal const string Control_type_not_allowed = "Control_type_not_allowed";

		// Token: 0x040002E5 RID: 741
		internal const string Base_type_not_allowed = "Base_type_not_allowed";

		// Token: 0x040002E6 RID: 742
		internal const string Reference_not_allowed = "Reference_not_allowed";

		// Token: 0x040002E7 RID: 743
		internal const string Id_already_used = "Id_already_used";

		// Token: 0x040002E8 RID: 744
		internal const string Duplicate_id_used = "Duplicate_id_used";

		// Token: 0x040002E9 RID: 745
		internal const string Only_one_directive_allowed = "Only_one_directive_allowed";

		// Token: 0x040002EA RID: 746
		internal const string Invalid_res_expr = "Invalid_res_expr";

		// Token: 0x040002EB RID: 747
		internal const string Res_not_found = "Res_not_found";

		// Token: 0x040002EC RID: 748
		internal const string Res_not_found_with_class_and_key = "Res_not_found_with_class_and_key";

		// Token: 0x040002ED RID: 749
		internal const string Invalid_cache_settings_location = "Invalid_cache_settings_location";

		// Token: 0x040002EE RID: 750
		internal const string Registered_async_tasks_remain = "Registered_async_tasks_remain";

		// Token: 0x040002EF RID: 751
		internal const string Async_tasks_wrong_thread = "Async_tasks_wrong_thread";

		// Token: 0x040002F0 RID: 752
		internal const string ClientScriptManager_RegisterForEventValidation_Too_Early = "ClientScriptManager_RegisterForEventValidation_Too_Early";

		// Token: 0x040002F1 RID: 753
		internal const string ClientScriptManager_InvalidPostBackArgument = "ClientScriptManager_InvalidPostBackArgument";

		// Token: 0x040002F2 RID: 754
		internal const string DesignTimeTemplateParser_ErrorParsingTheme = "DesignTimeTemplateParser_ErrorParsingTheme";

		// Token: 0x040002F3 RID: 755
		internal const string Duplicate_registered_tag = "Duplicate_registered_tag";

		// Token: 0x040002F4 RID: 756
		internal const string Empty_attribute = "Empty_attribute";

		// Token: 0x040002F5 RID: 757
		internal const string Space_attribute = "Space_attribute";

		// Token: 0x040002F6 RID: 758
		internal const string Empty_expression = "Empty_expression";

		// Token: 0x040002F7 RID: 759
		internal const string ControlBuilder_DatabindingRequiresEvent = "ControlBuilder_DatabindingRequiresEvent";

		// Token: 0x040002F8 RID: 760
		internal const string ControlBuilder_TwoWayBindingNonProperty = "ControlBuilder_TwoWayBindingNonProperty";

		// Token: 0x040002F9 RID: 761
		internal const string ControlBuilder_CannotHaveMultipleBoundEntries = "ControlBuilder_CannotHaveMultipleBoundEntries";

		// Token: 0x040002FA RID: 762
		internal const string ControlBuilder_ExpressionsNotAllowedInThemes = "ControlBuilder_ExpressionsNotAllowedInThemes";

		// Token: 0x040002FB RID: 763
		internal const string FilteredAttributeDictionary_ArgumentMustBeString = "FilteredAttributeDictionary_ArgumentMustBeString";

		// Token: 0x040002FC RID: 764
		internal const string HotSpotCollection_InvalidType = "HotSpotCollection_InvalidType";

		// Token: 0x040002FD RID: 765
		internal const string HotSpotCollection_InvalidTypeIndex = "HotSpotCollection_InvalidTypeIndex";

		// Token: 0x040002FE RID: 766
		internal const string Invalid_attribute_value = "Invalid_attribute_value";

		// Token: 0x040002FF RID: 767
		internal const string Invalid_boolean_attribute = "Invalid_boolean_attribute";

		// Token: 0x04000300 RID: 768
		internal const string Invalid_integer_attribute = "Invalid_integer_attribute";

		// Token: 0x04000301 RID: 769
		internal const string Invalid_nonnegative_integer_attribute = "Invalid_nonnegative_integer_attribute";

		// Token: 0x04000302 RID: 770
		internal const string Invalid_positive_integer_attribute = "Invalid_positive_integer_attribute";

		// Token: 0x04000303 RID: 771
		internal const string Invalid_non_zero_hexadecimal_attribute = "Invalid_non_zero_hexadecimal_attribute";

		// Token: 0x04000304 RID: 772
		internal const string Invalid_hash_algorithm_type = "Invalid_hash_algorithm_type";

		// Token: 0x04000305 RID: 773
		internal const string Invalid_enum_attribute = "Invalid_enum_attribute";

		// Token: 0x04000306 RID: 774
		internal const string Invalid_type_attribute = "Invalid_type_attribute";

		// Token: 0x04000307 RID: 775
		internal const string Invalid_culture_attribute = "Invalid_culture_attribute";

		// Token: 0x04000308 RID: 776
		internal const string Invalid_temp_directory = "Invalid_temp_directory";

		// Token: 0x04000309 RID: 777
		internal const string Invalid_reference_directive = "Invalid_reference_directive";

		// Token: 0x0400030A RID: 778
		internal const string Invalid_reference_directive_attrib = "Invalid_reference_directive_attrib";

		// Token: 0x0400030B RID: 779
		internal const string Invalid_typeNameOrVirtualPath_directive = "Invalid_typeNameOrVirtualPath_directive";

		// Token: 0x0400030C RID: 780
		internal const string Invalid_tagprefix_entry = "Invalid_tagprefix_entry";

		// Token: 0x0400030D RID: 781
		internal const string Mapped_type_must_inherit = "Mapped_type_must_inherit";

		// Token: 0x0400030E RID: 782
		internal const string Missing_required_attribute = "Missing_required_attribute";

		// Token: 0x0400030F RID: 783
		internal const string Missing_required_attributes = "Missing_required_attributes";

		// Token: 0x04000310 RID: 784
		internal const string Attr_not_supported_in_directive = "Attr_not_supported_in_directive";

		// Token: 0x04000311 RID: 785
		internal const string Attr_not_supported_in_ucdirective = "Attr_not_supported_in_ucdirective";

		// Token: 0x04000312 RID: 786
		internal const string Attr_not_supported_in_pagedirective = "Attr_not_supported_in_pagedirective";

		// Token: 0x04000313 RID: 787
		internal const string Invalid_attr = "Invalid_attr";

		// Token: 0x04000314 RID: 788
		internal const string Attrib_parse_error = "Attrib_parse_error";

		// Token: 0x04000315 RID: 789
		internal const string Missing_attr = "Missing_attr";

		// Token: 0x04000316 RID: 790
		internal const string Missing_output_cache_attr = "Missing_output_cache_attr";

		// Token: 0x04000317 RID: 791
		internal const string Missing_varybyparam_attr = "Missing_varybyparam_attr";

		// Token: 0x04000318 RID: 792
		internal const string Missing_directive = "Missing_directive";

		// Token: 0x04000319 RID: 793
		internal const string Unknown_directive = "Unknown_directive";

		// Token: 0x0400031A RID: 794
		internal const string Malformed_server_tag = "Malformed_server_tag";

		// Token: 0x0400031B RID: 795
		internal const string Malformed_server_block = "Malformed_server_block";

		// Token: 0x0400031C RID: 796
		internal const string Server_tags_cant_contain_percent_constructs = "Server_tags_cant_contain_percent_constructs";

		// Token: 0x0400031D RID: 797
		internal const string Include_not_allowed_in_server_script_tag = "Include_not_allowed_in_server_script_tag";

		// Token: 0x0400031E RID: 798
		internal const string Only_file_virtual_supported_on_server_include = "Only_file_virtual_supported_on_server_include";

		// Token: 0x0400031F RID: 799
		internal const string Runat_can_only_be_server = "Runat_can_only_be_server";

		// Token: 0x04000320 RID: 800
		internal const string Invalid_identifier = "Invalid_identifier";

		// Token: 0x04000321 RID: 801
		internal const string Invalid_resourcekey = "Invalid_resourcekey";

		// Token: 0x04000322 RID: 802
		internal const string ControlBuilder_IDMustUseAttribute = "ControlBuilder_IDMustUseAttribute";

		// Token: 0x04000323 RID: 803
		internal const string ControlBuilder_CannotHaveComplexString = "ControlBuilder_CannotHaveComplexString";

		// Token: 0x04000324 RID: 804
		internal const string ControlBuilder_ParseTimeDataNotAvailable = "ControlBuilder_ParseTimeDataNotAvailable";

		// Token: 0x04000325 RID: 805
		internal const string Duplicate_attr_in_directive = "Duplicate_attr_in_directive";

		// Token: 0x04000326 RID: 806
		internal const string Duplicate_attr_in_tag = "Duplicate_attr_in_tag";

		// Token: 0x04000327 RID: 807
		internal const string Non_existent_base_type = "Non_existent_base_type";

		// Token: 0x04000328 RID: 808
		internal const string Invalid_type_to_inherit_from = "Invalid_type_to_inherit_from";

		// Token: 0x04000329 RID: 809
		internal const string Invalid_type_to_implement = "Invalid_type_to_implement";

		// Token: 0x0400032A RID: 810
		internal const string Error_page_not_supported_when_buffering_off = "Error_page_not_supported_when_buffering_off";

		// Token: 0x0400032B RID: 811
		internal const string Enablesessionstate_must_be_true_false_or_readonly = "Enablesessionstate_must_be_true_false_or_readonly";

		// Token: 0x0400032C RID: 812
		internal const string Attributes_mutually_exclusive = "Attributes_mutually_exclusive";

		// Token: 0x0400032D RID: 813
		internal const string Async_and_aspcompat = "Async_and_aspcompat";

		// Token: 0x0400032E RID: 814
		internal const string Async_and_transaction = "Async_and_transaction";

		// Token: 0x0400032F RID: 815
		internal const string Async_required = "Async_required";

		// Token: 0x04000330 RID: 816
		internal const string Async_addhandler_too_late = "Async_addhandler_too_late";

		// Token: 0x04000331 RID: 817
		internal const string Async_operation_disabled = "Async_operation_disabled";

		// Token: 0x04000332 RID: 818
		internal const string Async_null_asyncresult = "Async_null_asyncresult";

		// Token: 0x04000333 RID: 819
		internal const string Mixed_lang_not_supported = "Mixed_lang_not_supported";

		// Token: 0x04000334 RID: 820
		internal const string Inconsistent_CodeFile_Language = "Inconsistent_CodeFile_Language";

		// Token: 0x04000335 RID: 821
		internal const string Codefile_without_inherits = "Codefile_without_inherits";

		// Token: 0x04000336 RID: 822
		internal const string CodeFileBaseClass_Without_Codefile = "CodeFileBaseClass_Without_Codefile";

		// Token: 0x04000337 RID: 823
		internal const string Invalid_lang = "Invalid_lang";

		// Token: 0x04000338 RID: 824
		internal const string Invalid_lang_extension = "Invalid_lang_extension";

		// Token: 0x04000339 RID: 825
		internal const string Cant_use_nocompile_uc = "Cant_use_nocompile_uc";

		// Token: 0x0400033A RID: 826
		internal const string Invalid_CodeSubDirectory_Not_Exist = "Invalid_CodeSubDirectory_Not_Exist";

		// Token: 0x0400033B RID: 827
		internal const string Invalid_CodeSubDirectory = "Invalid_CodeSubDirectory";

		// Token: 0x0400033C RID: 828
		internal const string Reserved_AssemblyName = "Reserved_AssemblyName";

		// Token: 0x0400033D RID: 829
		internal const string Empty_extension = "Empty_extension";

		// Token: 0x0400033E RID: 830
		internal const string Base_class_field_with_type_different_from_type_of_control = "Base_class_field_with_type_different_from_type_of_control";

		// Token: 0x0400033F RID: 831
		internal const string ControlSkin_cannot_contain_controls = "ControlSkin_cannot_contain_controls";

		// Token: 0x04000340 RID: 832
		internal const string Inner_Content_not_literal = "Inner_Content_not_literal";

		// Token: 0x04000341 RID: 833
		internal const string Invalid_client_target = "Invalid_client_target";

		// Token: 0x04000342 RID: 834
		internal const string Empty_file_name = "Empty_file_name";

		// Token: 0x04000343 RID: 835
		internal const string SetStyleSheetThemeCannotBeSet = "SetStyleSheetThemeCannotBeSet";

		// Token: 0x04000344 RID: 836
		internal const string PropertySetBeforePageEvent = "PropertySetBeforePageEvent";

		// Token: 0x04000345 RID: 837
		internal const string PropertySetBeforeStyleSheetApplied = "PropertySetBeforeStyleSheetApplied";

		// Token: 0x04000346 RID: 838
		internal const string PropertySetBeforePreInitOrAddToControls = "PropertySetBeforePreInitOrAddToControls";

		// Token: 0x04000347 RID: 839
		internal const string PropertySetAfterFrameworkInitialize = "PropertySetAfterFrameworkInitialize";

		// Token: 0x04000348 RID: 840
		internal const string StyleSheetAreadyAppliedOnControl = "StyleSheetAreadyAppliedOnControl";

		// Token: 0x04000349 RID: 841
		internal const string Control_CannotOwnSelf = "Control_CannotOwnSelf";

		// Token: 0x0400034A RID: 842
		internal const string AdRotator_cant_open_file = "AdRotator_cant_open_file";

		// Token: 0x0400034B RID: 843
		internal const string AdRotator_cant_open_file_no_permission = "AdRotator_cant_open_file_no_permission";

		// Token: 0x0400034C RID: 844
		internal const string AdRotator_parse_error = "AdRotator_parse_error";

		// Token: 0x0400034D RID: 845
		internal const string AdRotator_no_advertisements = "AdRotator_no_advertisements";

		// Token: 0x0400034E RID: 846
		internal const string AdRotator_only_one_datasource = "AdRotator_only_one_datasource";

		// Token: 0x0400034F RID: 847
		internal const string AdRotator_invalid_integer_format = "AdRotator_invalid_integer_format";

		// Token: 0x04000350 RID: 848
		internal const string AdRotator_expect_records_with_advertisement_properties = "AdRotator_expect_records_with_advertisement_properties";

		// Token: 0x04000351 RID: 849
		internal const string Validator_control_blank = "Validator_control_blank";

		// Token: 0x04000352 RID: 850
		internal const string Validator_control_not_found = "Validator_control_not_found";

		// Token: 0x04000353 RID: 851
		internal const string Validator_bad_compare_control = "Validator_bad_compare_control";

		// Token: 0x04000354 RID: 852
		internal const string Validator_bad_control_type = "Validator_bad_control_type";

		// Token: 0x04000355 RID: 853
		internal const string Validator_value_bad_type = "Validator_value_bad_type";

		// Token: 0x04000356 RID: 854
		internal const string Validator_range_overalap = "Validator_range_overalap";

		// Token: 0x04000357 RID: 855
		internal const string Validator_bad_regex = "Validator_bad_regex";

		// Token: 0x04000358 RID: 856
		internal const string ValSummary_error_message_1 = "ValSummary_error_message_1";

		// Token: 0x04000359 RID: 857
		internal const string ValSummary_error_message_2 = "ValSummary_error_message_2";

		// Token: 0x0400035A RID: 858
		internal const string ViewState_MissingViewStateField = "ViewState_MissingViewStateField";

		// Token: 0x0400035B RID: 859
		internal const string ViewState_InvalidViewState = "ViewState_InvalidViewState";

		// Token: 0x0400035C RID: 860
		internal const string ViewState_InvalidViewStatePlus = "ViewState_InvalidViewStatePlus";

		// Token: 0x0400035D RID: 861
		internal const string ViewState_ClientDisconnected = "ViewState_ClientDisconnected";

		// Token: 0x0400035E RID: 862
		internal const string ViewState_AuthenticationFailed = "ViewState_AuthenticationFailed";

		// Token: 0x0400035F RID: 863
		internal const string Control_does_not_allow_children = "Control_does_not_allow_children";

		// Token: 0x04000360 RID: 864
		internal const string DataBinder_Prop_Not_Found = "DataBinder_Prop_Not_Found";

		// Token: 0x04000361 RID: 865
		internal const string DataBinder_Invalid_Indexed_Expr = "DataBinder_Invalid_Indexed_Expr";

		// Token: 0x04000362 RID: 866
		internal const string DataBinder_No_Indexed_Accessor = "DataBinder_No_Indexed_Accessor";

		// Token: 0x04000363 RID: 867
		internal const string XPathBinder_MustBeIXPathNavigable = "XPathBinder_MustBeIXPathNavigable";

		// Token: 0x04000364 RID: 868
		internal const string XPathBinder_MustHaveXmlNodes = "XPathBinder_MustHaveXmlNodes";

		// Token: 0x04000365 RID: 869
		internal const string Field_Not_Found = "Field_Not_Found";

		// Token: 0x04000366 RID: 870
		internal const string DataItem_Not_Found = "DataItem_Not_Found";

		// Token: 0x04000367 RID: 871
		internal const string DataGrid_Missing_VirtualItemCount = "DataGrid_Missing_VirtualItemCount";

		// Token: 0x04000368 RID: 872
		internal const string DataGrid_NoAutoGenColumns = "DataGrid_NoAutoGenColumns";

		// Token: 0x04000369 RID: 873
		internal const string GridView_Missing_VirtualItemCount = "GridView_Missing_VirtualItemCount";

		// Token: 0x0400036A RID: 874
		internal const string GridView_NoAutoGenFields = "GridView_NoAutoGenFields";

		// Token: 0x0400036B RID: 875
		internal const string GridView_DataSourceReturnedNullView = "GridView_DataSourceReturnedNullView";

		// Token: 0x0400036C RID: 876
		internal const string GridView_UnhandledEvent = "GridView_UnhandledEvent";

		// Token: 0x0400036D RID: 877
		internal const string GridView_MustBeParented = "GridView_MustBeParented";

		// Token: 0x0400036E RID: 878
		internal const string GridView_DataKeyNamesMustBeSpecified = "GridView_DataKeyNamesMustBeSpecified";

		// Token: 0x0400036F RID: 879
		internal const string DetailsView_NoAutoGenFields = "DetailsView_NoAutoGenFields";

		// Token: 0x04000370 RID: 880
		internal const string DetailsView_UnhandledEvent = "DetailsView_UnhandledEvent";

		// Token: 0x04000371 RID: 881
		internal const string DetailsView_DataSourceMustBeCollection = "DetailsView_DataSourceMustBeCollection";

		// Token: 0x04000372 RID: 882
		internal const string DetailsView_MustBeParented = "DetailsView_MustBeParented";

		// Token: 0x04000373 RID: 883
		internal const string FileUpload_StreamNotSeekable = "FileUpload_StreamNotSeekable";

		// Token: 0x04000374 RID: 884
		internal const string FileUpload_StreamTooLong = "FileUpload_StreamTooLong";

		// Token: 0x04000375 RID: 885
		internal const string FileUpload_StreamLengthNotReached = "FileUpload_StreamLengthNotReached";

		// Token: 0x04000376 RID: 886
		internal const string FormView_UnhandledEvent = "FormView_UnhandledEvent";

		// Token: 0x04000377 RID: 887
		internal const string FormView_DataSourceMustBeCollection = "FormView_DataSourceMustBeCollection";

		// Token: 0x04000378 RID: 888
		internal const string DetailsViewFormView_ControlMustBeInEditMode = "DetailsViewFormView_ControlMustBeInEditMode";

		// Token: 0x04000379 RID: 889
		internal const string DetailsViewFormView_ControlMustBeInInsertMode = "DetailsViewFormView_ControlMustBeInInsertMode";

		// Token: 0x0400037A RID: 890
		internal const string DataBoundControl_InvalidDataPropertyChange = "DataBoundControl_InvalidDataPropertyChange";

		// Token: 0x0400037B RID: 891
		internal const string DataBoundControl_NullView = "DataBoundControl_NullView";

		// Token: 0x0400037C RID: 892
		internal const string DataBoundControl_InvalidDataSourceType = "DataBoundControl_InvalidDataSourceType";

		// Token: 0x0400037D RID: 893
		internal const string DataBoundControl_DataSourceDoesntSupportPaging = "DataBoundControl_DataSourceDoesntSupportPaging";

		// Token: 0x0400037E RID: 894
		internal const string DataBoundControl_NeedICollectionOrTotalRowCount = "DataBoundControl_NeedICollectionOrTotalRowCount";

		// Token: 0x0400037F RID: 895
		internal const string DataBoundControlHelper_NoNamingContainer = "DataBoundControlHelper_NoNamingContainer";

		// Token: 0x04000380 RID: 896
		internal const string HierarchicalDataBoundControl_InvalidDataSource = "HierarchicalDataBoundControl_InvalidDataSource";

		// Token: 0x04000381 RID: 897
		internal const string HierarchicalDataControl_ViewNotFound = "HierarchicalDataControl_ViewNotFound";

		// Token: 0x04000382 RID: 898
		internal const string HierarchicalDataControl_DataSourceIDMustBeHierarchicalDataControl = "HierarchicalDataControl_DataSourceIDMustBeHierarchicalDataControl";

		// Token: 0x04000383 RID: 899
		internal const string HierarchicalDataControl_DataSourceDoesntExist = "HierarchicalDataControl_DataSourceDoesntExist";

		// Token: 0x04000384 RID: 900
		internal const string DataControl_ViewNotFound = "DataControl_ViewNotFound";

		// Token: 0x04000385 RID: 901
		internal const string DataControl_DataSourceIDMustBeDataControl = "DataControl_DataSourceIDMustBeDataControl";

		// Token: 0x04000386 RID: 902
		internal const string DataControl_DataSourceDoesntExist = "DataControl_DataSourceDoesntExist";

		// Token: 0x04000387 RID: 903
		internal const string DataControl_MultipleDataSources = "DataControl_MultipleDataSources";

		// Token: 0x04000388 RID: 904
		internal const string DataControlField_NoContainer = "DataControlField_NoContainer";

		// Token: 0x04000389 RID: 905
		internal const string DataControlField_CallbacksNotSupported = "DataControlField_CallbacksNotSupported";

		// Token: 0x0400038A RID: 906
		internal const string DataControlFieldCollection_InvalidType = "DataControlFieldCollection_InvalidType";

		// Token: 0x0400038B RID: 907
		internal const string DataControlFieldCollection_InvalidTypeIndex = "DataControlFieldCollection_InvalidTypeIndex";

		// Token: 0x0400038C RID: 908
		internal const string BoundField_WrongControlType = "BoundField_WrongControlType";

		// Token: 0x0400038D RID: 909
		internal const string CheckBoxField_WrongControlType = "CheckBoxField_WrongControlType";

		// Token: 0x0400038E RID: 910
		internal const string CheckBoxField_CouldntParseAsBoolean = "CheckBoxField_CouldntParseAsBoolean";

		// Token: 0x0400038F RID: 911
		internal const string CheckBoxField_NotSupported = "CheckBoxField_NotSupported";

		// Token: 0x04000390 RID: 912
		internal const string CommandField_CallbacksNotSupported = "CommandField_CallbacksNotSupported";

		// Token: 0x04000391 RID: 913
		internal const string ImageField_WrongControlType = "ImageField_WrongControlType";

		// Token: 0x04000392 RID: 914
		internal const string TemplateField_CallbacksNotSupported = "TemplateField_CallbacksNotSupported";

		// Token: 0x04000393 RID: 915
		internal const string PagedDataSource_Cannot_Get_Count = "PagedDataSource_Cannot_Get_Count";

		// Token: 0x04000394 RID: 916
		internal const string Cannot_Have_Children_Of_Type = "Cannot_Have_Children_Of_Type";

		// Token: 0x04000395 RID: 917
		internal const string Control_Cannot_Databind = "Control_Cannot_Databind";

		// Token: 0x04000396 RID: 918
		internal const string InnerHtml_not_supported = "InnerHtml_not_supported";

		// Token: 0x04000397 RID: 919
		internal const string InnerText_not_supported = "InnerText_not_supported";

		// Token: 0x04000398 RID: 920
		internal const string ListControl_SelectionOutOfRange = "ListControl_SelectionOutOfRange";

		// Token: 0x04000399 RID: 921
		internal const string BulletedList_SelectionNotSupported = "BulletedList_SelectionNotSupported";

		// Token: 0x0400039A RID: 922
		internal const string BulletedList_TextNotSupported = "BulletedList_TextNotSupported";

		// Token: 0x0400039B RID: 923
		internal const string CannotUseParentPostBackWhenValidating = "CannotUseParentPostBackWhenValidating";

		// Token: 0x0400039C RID: 924
		internal const string CannotSetValidationOnDataControlButtons = "CannotSetValidationOnDataControlButtons";

		// Token: 0x0400039D RID: 925
		internal const string CannotSetValidationOnPagerButtons = "CannotSetValidationOnPagerButtons";

		// Token: 0x0400039E RID: 926
		internal const string Invalid_DataSource_Type = "Invalid_DataSource_Type";

		// Token: 0x0400039F RID: 927
		internal const string Invalid_CurrentPageIndex = "Invalid_CurrentPageIndex";

		// Token: 0x040003A0 RID: 928
		internal const string ListSource_Without_DataMembers = "ListSource_Without_DataMembers";

		// Token: 0x040003A1 RID: 929
		internal const string ListSource_Missing_DataMember = "ListSource_Missing_DataMember";

		// Token: 0x040003A2 RID: 930
		internal const string Enumerator_MoveNext_Not_Called = "Enumerator_MoveNext_Not_Called";

		// Token: 0x040003A3 RID: 931
		internal const string Sample_Databound_Text = "Sample_Databound_Text";

		// Token: 0x040003A4 RID: 932
		internal const string Resource_problem = "Resource_problem";

		// Token: 0x040003A5 RID: 933
		internal const string Duplicate_Resource_File = "Duplicate_Resource_File";

		// Token: 0x040003A6 RID: 934
		internal const string Property_Had_Malformed_Url = "Property_Had_Malformed_Url";

		// Token: 0x040003A7 RID: 935
		internal const string TypeResService_Needed = "TypeResService_Needed";

		// Token: 0x040003A8 RID: 936
		internal const string DataList_TemplateTableNotFound = "DataList_TemplateTableNotFound";

		// Token: 0x040003A9 RID: 937
		internal const string DataList_DataKeyFieldMustBeSpecified = "DataList_DataKeyFieldMustBeSpecified";

		// Token: 0x040003AA RID: 938
		internal const string EnumAttributeInvalidString = "EnumAttributeInvalidString";

		// Token: 0x040003AB RID: 939
		internal const string UnitParseNumericPart = "UnitParseNumericPart";

		// Token: 0x040003AC RID: 940
		internal const string UnitParseNoDigits = "UnitParseNoDigits";

		// Token: 0x040003AD RID: 941
		internal const string IsValid_Cant_Be_Called = "IsValid_Cant_Be_Called";

		// Token: 0x040003AE RID: 942
		internal const string Invalid_HtmlTextWriter = "Invalid_HtmlTextWriter";

		// Token: 0x040003AF RID: 943
		internal const string Form_Needs_Page = "Form_Needs_Page";

		// Token: 0x040003B0 RID: 944
		internal const string HtmlForm_OnlyIButtonControlCanBeDefaultButton = "HtmlForm_OnlyIButtonControlCanBeDefaultButton";

		// Token: 0x040003B1 RID: 945
		internal const string Head_Needs_Page = "Head_Needs_Page";

		// Token: 0x040003B2 RID: 946
		internal const string HtmlHead_StyleAlreadyRegistered = "HtmlHead_StyleAlreadyRegistered";

		// Token: 0x040003B3 RID: 947
		internal const string HtmlHead_OnlyOneHeadAllowed = "HtmlHead_OnlyOneHeadAllowed";

		// Token: 0x040003B4 RID: 948
		internal const string HtmlHead_OnlyOneTitleAllowed = "HtmlHead_OnlyOneTitleAllowed";

		// Token: 0x040003B5 RID: 949
		internal const string Style_RegisteredStylesAreReadOnly = "Style_RegisteredStylesAreReadOnly";

		// Token: 0x040003B6 RID: 950
		internal const string Style_InvalidBorderWidth = "Style_InvalidBorderWidth";

		// Token: 0x040003B7 RID: 951
		internal const string Style_InvalidWidth = "Style_InvalidWidth";

		// Token: 0x040003B8 RID: 952
		internal const string Style_InvalidHeight = "Style_InvalidHeight";

		// Token: 0x040003B9 RID: 953
		internal const string Cant_Multiselect_In_Single_Mode = "Cant_Multiselect_In_Single_Mode";

		// Token: 0x040003BA RID: 954
		internal const string Cant_Multiselect = "Cant_Multiselect";

		// Token: 0x040003BB RID: 955
		internal const string HtmlSelect_Cant_Multiselect_In_Single_Mode = "HtmlSelect_Cant_Multiselect_In_Single_Mode";

		// Token: 0x040003BC RID: 956
		internal const string Controls_Cant_Change_Between_Posts = "Controls_Cant_Change_Between_Posts";

		// Token: 0x040003BD RID: 957
		internal const string Value_Set_Not_Supported = "Value_Set_Not_Supported";

		// Token: 0x040003BE RID: 958
		internal const string SiteMap_feature_disabled = "SiteMap_feature_disabled";

		// Token: 0x040003BF RID: 959
		internal const string SiteMapNode_readonly = "SiteMapNode_readonly";

		// Token: 0x040003C0 RID: 960
		internal const string SiteMapNodeCollection_Invalid_Type = "SiteMapNodeCollection_Invalid_Type";

		// Token: 0x040003C1 RID: 961
		internal const string SiteMapProvider_Circular_Provider = "SiteMapProvider_Circular_Provider";

		// Token: 0x040003C2 RID: 962
		internal const string SiteMapProvider_Invalid_RootNode = "SiteMapProvider_Invalid_RootNode";

		// Token: 0x040003C3 RID: 963
		internal const string SiteMapProvider_cannot_remove_root_node = "SiteMapProvider_cannot_remove_root_node";

		// Token: 0x040003C4 RID: 964
		internal const string XmlSiteMapProvider_cannot_add_node = "XmlSiteMapProvider_cannot_add_node";

		// Token: 0x040003C5 RID: 965
		internal const string XmlSiteMapProvider_invalid_resource_key = "XmlSiteMapProvider_invalid_resource_key";

		// Token: 0x040003C6 RID: 966
		internal const string XmlSiteMapProvider_resourceKey_cannot_be_empty = "XmlSiteMapProvider_resourceKey_cannot_be_empty";

		// Token: 0x040003C7 RID: 967
		internal const string XmlSiteMapProvider_cannot_find_provider = "XmlSiteMapProvider_cannot_find_provider";

		// Token: 0x040003C8 RID: 968
		internal const string XmlSiteMapProvider_cannot_remove_node = "XmlSiteMapProvider_cannot_remove_node";

		// Token: 0x040003C9 RID: 969
		internal const string XmlSiteMapProvider_missing_siteMapFile = "XmlSiteMapProvider_missing_siteMapFile";

		// Token: 0x040003CA RID: 970
		internal const string XmlSiteMapProvider_Description = "XmlSiteMapProvider_Description";

		// Token: 0x040003CB RID: 971
		internal const string XmlSiteMapProvider_Not_Initialized = "XmlSiteMapProvider_Not_Initialized";

		// Token: 0x040003CC RID: 972
		internal const string XmlSiteMapProvider_Cannot_Be_Inited_Twice = "XmlSiteMapProvider_Cannot_Be_Inited_Twice";

		// Token: 0x040003CD RID: 973
		internal const string XmlSiteMapProvider_Top_Element_Must_Be_SiteMap = "XmlSiteMapProvider_Top_Element_Must_Be_SiteMap";

		// Token: 0x040003CE RID: 974
		internal const string XmlSiteMapProvider_Only_One_SiteMapNode_Required_At_Top = "XmlSiteMapProvider_Only_One_SiteMapNode_Required_At_Top";

		// Token: 0x040003CF RID: 975
		internal const string XmlSiteMapProvider_Only_SiteMapNode_Allowed = "XmlSiteMapProvider_Only_SiteMapNode_Allowed";

		// Token: 0x040003D0 RID: 976
		internal const string XmlSiteMapProvider_invalid_sitemapnode_returned = "XmlSiteMapProvider_invalid_sitemapnode_returned";

		// Token: 0x040003D1 RID: 977
		internal const string XmlSiteMapProvider_invalid_GetRootNodeCore = "XmlSiteMapProvider_invalid_GetRootNodeCore";

		// Token: 0x040003D2 RID: 978
		internal const string XmlSiteMapProvider_Error_loading_Config_file = "XmlSiteMapProvider_Error_loading_Config_file";

		// Token: 0x040003D3 RID: 979
		internal const string XmlSiteMapProvider_FileName_does_not_exist = "XmlSiteMapProvider_FileName_does_not_exist";

		// Token: 0x040003D4 RID: 980
		internal const string XmlSiteMapProvider_FileName_already_in_use = "XmlSiteMapProvider_FileName_already_in_use";

		// Token: 0x040003D5 RID: 981
		internal const string XmlSiteMapProvider_Invalid_Extension = "XmlSiteMapProvider_Invalid_Extension";

		// Token: 0x040003D6 RID: 982
		internal const string XmlSiteMapProvider_multiple_resource_definition = "XmlSiteMapProvider_multiple_resource_definition";

		// Token: 0x040003D7 RID: 983
		internal const string UrlMappings_only_app_relative_url_allowed = "UrlMappings_only_app_relative_url_allowed";

		// Token: 0x040003D8 RID: 984
		internal const string FileName_does_not_exist = "FileName_does_not_exist";

		// Token: 0x040003D9 RID: 985
		internal const string SiteMapProvider_Multiple_Providers_With_Identical_Name = "SiteMapProvider_Multiple_Providers_With_Identical_Name";

		// Token: 0x040003DA RID: 986
		internal const string XmlSiteMapProvider_Multiple_Nodes_With_Identical_Url = "XmlSiteMapProvider_Multiple_Nodes_With_Identical_Url";

		// Token: 0x040003DB RID: 987
		internal const string XmlSiteMapProvider_Multiple_Nodes_With_Identical_Key = "XmlSiteMapProvider_Multiple_Nodes_With_Identical_Key";

		// Token: 0x040003DC RID: 988
		internal const string Provider_Not_Found = "Provider_Not_Found";

		// Token: 0x040003DD RID: 989
		internal const string Collection_readonly = "Collection_readonly";

		// Token: 0x040003DE RID: 990
		internal const string PhoneCall_InvalidPhoneNumberFormat = "PhoneCall_InvalidPhoneNumberFormat";

		// Token: 0x040003DF RID: 991
		internal const string ParameterCollection_NotParameter = "ParameterCollection_NotParameter";

		// Token: 0x040003E0 RID: 992
		internal const string ControlParameter_CouldNotFindControl = "ControlParameter_CouldNotFindControl";

		// Token: 0x040003E1 RID: 993
		internal const string ControlParameter_ControlIDNotSpecified = "ControlParameter_ControlIDNotSpecified";

		// Token: 0x040003E2 RID: 994
		internal const string ControlParameter_PropertyNameNotSpecified = "ControlParameter_PropertyNameNotSpecified";

		// Token: 0x040003E3 RID: 995
		internal const string DataSourceCache_InvalidExpiryPolicy = "DataSourceCache_InvalidExpiryPolicy";

		// Token: 0x040003E4 RID: 996
		internal const string DataSourceCache_InvalidDuration = "DataSourceCache_InvalidDuration";

		// Token: 0x040003E5 RID: 997
		internal const string DataSourceCache_CacheMustBeEnabled = "DataSourceCache_CacheMustBeEnabled";

		// Token: 0x040003E6 RID: 998
		internal const string DataSourceView_NoPaging = "DataSourceView_NoPaging";

		// Token: 0x040003E7 RID: 999
		internal const string DataSourceView_NoSorting = "DataSourceView_NoSorting";

		// Token: 0x040003E8 RID: 1000
		internal const string DataSourceView_NoRowCount = "DataSourceView_NoRowCount";

		// Token: 0x040003E9 RID: 1001
		internal const string AccessDataSource_Description = "AccessDataSource_Description";

		// Token: 0x040003EA RID: 1002
		internal const string AccessDataSource_DisplayName = "AccessDataSource_DisplayName";

		// Token: 0x040003EB RID: 1003
		internal const string AccessDataSource_CannotSetConnectionString = "AccessDataSource_CannotSetConnectionString";

		// Token: 0x040003EC RID: 1004
		internal const string AccessDataSource_CannotSetProvider = "AccessDataSource_CannotSetProvider";

		// Token: 0x040003ED RID: 1005
		internal const string AccessDataSource_SqlCacheDependencyNotSupported = "AccessDataSource_SqlCacheDependencyNotSupported";

		// Token: 0x040003EE RID: 1006
		internal const string AccessDataSource_DesignTimeRelativePathsNotSupported = "AccessDataSource_DesignTimeRelativePathsNotSupported";

		// Token: 0x040003EF RID: 1007
		internal const string AccessDataSource_NoPathDiscoveryPermission = "AccessDataSource_NoPathDiscoveryPermission";

		// Token: 0x040003F0 RID: 1008
		internal const string AccessDataSourceView_SelectRequiresDataFile = "AccessDataSourceView_SelectRequiresDataFile";

		// Token: 0x040003F1 RID: 1009
		internal const string SqlDataSource_Description = "SqlDataSource_Description";

		// Token: 0x040003F2 RID: 1010
		internal const string SqlDataSource_DisplayName = "SqlDataSource_DisplayName";

		// Token: 0x040003F3 RID: 1011
		internal const string SqlDataSource_InvalidMode = "SqlDataSource_InvalidMode";

		// Token: 0x040003F4 RID: 1012
		internal const string SqlDataSource_SqlCacheDependencyNotSupported = "SqlDataSource_SqlCacheDependencyNotSupported";

		// Token: 0x040003F5 RID: 1013
		internal const string SqlDataSource_NoDbPermission = "SqlDataSource_NoDbPermission";

		// Token: 0x040003F6 RID: 1014
		internal const string SqlDataSourceView_SortNotSupported = "SqlDataSourceView_SortNotSupported";

		// Token: 0x040003F7 RID: 1015
		internal const string SqlDataSourceView_FilterNotSupported = "SqlDataSourceView_FilterNotSupported";

		// Token: 0x040003F8 RID: 1016
		internal const string SqlDataSourceView_CacheNotSupported = "SqlDataSourceView_CacheNotSupported";

		// Token: 0x040003F9 RID: 1017
		internal const string SqlDataSourceView_DeleteNotSupported = "SqlDataSourceView_DeleteNotSupported";

		// Token: 0x040003FA RID: 1018
		internal const string SqlDataSourceView_InsertNotSupported = "SqlDataSourceView_InsertNotSupported";

		// Token: 0x040003FB RID: 1019
		internal const string SqlDataSourceView_UpdateNotSupported = "SqlDataSourceView_UpdateNotSupported";

		// Token: 0x040003FC RID: 1020
		internal const string SqlDataSourceView_CouldNotCreateConnection = "SqlDataSourceView_CouldNotCreateConnection";

		// Token: 0x040003FD RID: 1021
		internal const string SqlDataSourceView_NoPaging = "SqlDataSourceView_NoPaging";

		// Token: 0x040003FE RID: 1022
		internal const string SqlDataSourceView_NoSorting = "SqlDataSourceView_NoSorting";

		// Token: 0x040003FF RID: 1023
		internal const string SqlDataSourceView_NoRowCount = "SqlDataSourceView_NoRowCount";

		// Token: 0x04000400 RID: 1024
		internal const string SqlDataSourceView_CountNotValid = "SqlDataSourceView_CountNotValid";

		// Token: 0x04000401 RID: 1025
		internal const string SqlDataSourceView_SortParameterRequiresStoredProcedure = "SqlDataSourceView_SortParameterRequiresStoredProcedure";

		// Token: 0x04000402 RID: 1026
		internal const string SqlDataSourceView_CommandNotificationNotSupported = "SqlDataSourceView_CommandNotificationNotSupported";

		// Token: 0x04000403 RID: 1027
		internal const string SqlDataSourceView_Pessimistic = "SqlDataSourceView_Pessimistic";

		// Token: 0x04000404 RID: 1028
		internal const string SqlDataSourceView_MissingParameters = "SqlDataSourceView_MissingParameters";

		// Token: 0x04000405 RID: 1029
		internal const string SqlDataSourceView_NoParameters = "SqlDataSourceView_NoParameters";

		// Token: 0x04000406 RID: 1030
		internal const string DataSourceView_delete = "DataSourceView_delete";

		// Token: 0x04000407 RID: 1031
		internal const string DataSourceView_update = "DataSourceView_update";

		// Token: 0x04000408 RID: 1032
		internal const string ObjectDataSource_Description = "ObjectDataSource_Description";

		// Token: 0x04000409 RID: 1033
		internal const string ObjectDataSource_DisplayName = "ObjectDataSource_DisplayName";

		// Token: 0x0400040A RID: 1034
		internal const string ObjectDataSourceView_DeleteNotSupported = "ObjectDataSourceView_DeleteNotSupported";

		// Token: 0x0400040B RID: 1035
		internal const string ObjectDataSourceView_InsertNotSupported = "ObjectDataSourceView_InsertNotSupported";

		// Token: 0x0400040C RID: 1036
		internal const string ObjectDataSourceView_UpdateNotSupported = "ObjectDataSourceView_UpdateNotSupported";

		// Token: 0x0400040D RID: 1037
		internal const string ObjectDataSourceView_SelectNotSupported = "ObjectDataSourceView_SelectNotSupported";

		// Token: 0x0400040E RID: 1038
		internal const string ObjectDataSourceView_InsertRequiresValues = "ObjectDataSourceView_InsertRequiresValues";

		// Token: 0x0400040F RID: 1039
		internal const string ObjectDataSourceView_TypeNotSpecified = "ObjectDataSourceView_TypeNotSpecified";

		// Token: 0x04000410 RID: 1040
		internal const string ObjectDataSourceView_TypeNotFound = "ObjectDataSourceView_TypeNotFound";

		// Token: 0x04000411 RID: 1041
		internal const string ObjectDataSourceView_MethodNotFoundNoParams = "ObjectDataSourceView_MethodNotFoundNoParams";

		// Token: 0x04000412 RID: 1042
		internal const string ObjectDataSourceView_MethodNotFoundWithParams = "ObjectDataSourceView_MethodNotFoundWithParams";

		// Token: 0x04000413 RID: 1043
		internal const string ObjectDataSourceView_MethodNotFoundForDataObject = "ObjectDataSourceView_MethodNotFoundForDataObject";

		// Token: 0x04000414 RID: 1044
		internal const string ObjectDataSourceView_DataObjectTypeNotFound = "ObjectDataSourceView_DataObjectTypeNotFound";

		// Token: 0x04000415 RID: 1045
		internal const string ObjectDataSourceView_DataObjectPropertyNotFound = "ObjectDataSourceView_DataObjectPropertyNotFound";

		// Token: 0x04000416 RID: 1046
		internal const string ObjectDataSourceView_DataObjectPropertyReadOnly = "ObjectDataSourceView_DataObjectPropertyReadOnly";

		// Token: 0x04000417 RID: 1047
		internal const string ObjectDataSourceView_MultipleOverloads = "ObjectDataSourceView_MultipleOverloads";

		// Token: 0x04000418 RID: 1048
		internal const string ObjectDataSourceView_CacheNotSupportedOnSortedDataView = "ObjectDataSourceView_CacheNotSupportedOnSortedDataView";

		// Token: 0x04000419 RID: 1049
		internal const string ObjectDataSourceView_CacheNotSupportedOnIDataReader = "ObjectDataSourceView_CacheNotSupportedOnIDataReader";

		// Token: 0x0400041A RID: 1050
		internal const string ObjectDataSourceView_SortNotSupportedOnIEnumerable = "ObjectDataSourceView_SortNotSupportedOnIEnumerable";

		// Token: 0x0400041B RID: 1051
		internal const string ObjectDataSourceView_FilterNotSupported = "ObjectDataSourceView_FilterNotSupported";

		// Token: 0x0400041C RID: 1052
		internal const string ObjectDataSourceView_Pessimistic = "ObjectDataSourceView_Pessimistic";

		// Token: 0x0400041D RID: 1053
		internal const string ObjectDataSourceView_NoOldValuesParams = "ObjectDataSourceView_NoOldValuesParams";

		// Token: 0x0400041E RID: 1054
		internal const string ObjectDataSourceView_MissingPagingSettings = "ObjectDataSourceView_MissingPagingSettings";

		// Token: 0x0400041F RID: 1055
		internal const string ObjectDataSourceView_CannotConvertType = "ObjectDataSourceView_CannotConvertType";

		// Token: 0x04000420 RID: 1056
		internal const string FilteredDataSetHelper_DataSetHasNoTables = "FilteredDataSetHelper_DataSetHasNoTables";

		// Token: 0x04000421 RID: 1057
		internal const string StringPropertyBuilder_CannotHaveChildObjects = "StringPropertyBuilder_CannotHaveChildObjects";

		// Token: 0x04000422 RID: 1058
		internal const string XmlHierarchyData_CouldNotFindNode = "XmlHierarchyData_CouldNotFindNode";

		// Token: 0x04000423 RID: 1059
		internal const string XmlDataSource_Description = "XmlDataSource_Description";

		// Token: 0x04000424 RID: 1060
		internal const string XmlDataSource_DesignTimeRelativePathsNotSupported = "XmlDataSource_DesignTimeRelativePathsNotSupported";

		// Token: 0x04000425 RID: 1061
		internal const string XmlDataSource_DisplayName = "XmlDataSource_DisplayName";

		// Token: 0x04000426 RID: 1062
		internal const string XmlDataSource_SaveNotAllowed = "XmlDataSource_SaveNotAllowed";

		// Token: 0x04000427 RID: 1063
		internal const string XmlDataSource_NoWebPermission = "XmlDataSource_NoWebPermission";

		// Token: 0x04000428 RID: 1064
		internal const string XmlDataSource_CannotChangeWhileLoading = "XmlDataSource_CannotChangeWhileLoading";

		// Token: 0x04000429 RID: 1065
		internal const string XmlDataSource_NeedUniqueIDForCache = "XmlDataSource_NeedUniqueIDForCache";

		// Token: 0x0400042A RID: 1066
		internal const string NeedHeader = "NeedHeader";

		// Token: 0x0400042B RID: 1067
		internal const string Form_Required_For_Focus = "Form_Required_For_Focus";

		// Token: 0x0400042C RID: 1068
		internal const string Page_MustCallBeforeAndDuringPreRender = "Page_MustCallBeforeAndDuringPreRender";

		// Token: 0x0400042D RID: 1069
		internal const string RoleGroupCollection_InvalidType = "RoleGroupCollection_InvalidType";

		// Token: 0x0400042E RID: 1070
		internal const string Page_CallBackError = "Page_CallBackError";

		// Token: 0x0400042F RID: 1071
		internal const string Page_CallBackInvalid = "Page_CallBackInvalid";

		// Token: 0x04000430 RID: 1072
		internal const string Page_CallBackTargetInvalid = "Page_CallBackTargetInvalid";

		// Token: 0x04000431 RID: 1073
		internal const string NoThemingSupport = "NoThemingSupport";

		// Token: 0x04000432 RID: 1074
		internal const string ControlNonVisual = "ControlNonVisual";

		// Token: 0x04000433 RID: 1075
		internal const string NoFocusSupport = "NoFocusSupport";

		// Token: 0x04000434 RID: 1076
		internal const string PageStatePersister_PageCannotBeNull = "PageStatePersister_PageCannotBeNull";

		// Token: 0x04000435 RID: 1077
		internal const string SessionPageStatePersister_SessionMustBeEnabled = "SessionPageStatePersister_SessionMustBeEnabled";

		// Token: 0x04000436 RID: 1078
		internal const string Page_MissingDataBindingContext = "Page_MissingDataBindingContext";

		// Token: 0x04000437 RID: 1079
		internal const string TemplateControl_DataBindingRequiresPage = "TemplateControl_DataBindingRequiresPage";

		// Token: 0x04000438 RID: 1080
		internal const string LabelForNotFound = "LabelForNotFound";

		// Token: 0x04000439 RID: 1081
		internal const string Attrib_Sql9_not_allowed = "Attrib_Sql9_not_allowed";

		// Token: 0x0400043A RID: 1082
		internal const string FactoryGenerator_TypeNotPublic = "FactoryGenerator_TypeNotPublic";

		// Token: 0x0400043B RID: 1083
		internal const string FactoryGenerator_TypeHasNoParameterlessConstructor = "FactoryGenerator_TypeHasNoParameterlessConstructor";

		// Token: 0x0400043C RID: 1084
		internal const string FactoryInterface = "FactoryInterface";

		// Token: 0x0400043D RID: 1085
		internal const string InvalidSerializedData = "InvalidSerializedData";

		// Token: 0x0400043E RID: 1086
		internal const string NonSerializableType = "NonSerializableType";

		// Token: 0x0400043F RID: 1087
		internal const string ErrorSerializingValue = "ErrorSerializingValue";

		// Token: 0x04000440 RID: 1088
		internal const string Control_Controls = "Control_Controls";

		// Token: 0x04000441 RID: 1089
		internal const string Control_ID = "Control_ID";

		// Token: 0x04000442 RID: 1090
		internal const string Control_MaintainState = "Control_MaintainState";

		// Token: 0x04000443 RID: 1091
		internal const string Control_Visible = "Control_Visible";

		// Token: 0x04000444 RID: 1092
		internal const string Control_OnDisposed = "Control_OnDisposed";

		// Token: 0x04000445 RID: 1093
		internal const string Control_OnInit = "Control_OnInit";

		// Token: 0x04000446 RID: 1094
		internal const string Control_OnLoad = "Control_OnLoad";

		// Token: 0x04000447 RID: 1095
		internal const string Control_OnUnload = "Control_OnUnload";

		// Token: 0x04000448 RID: 1096
		internal const string Control_OnPreRender = "Control_OnPreRender";

		// Token: 0x04000449 RID: 1097
		internal const string Control_OnDataBind = "Control_OnDataBind";

		// Token: 0x0400044A RID: 1098
		internal const string Control_NamingContainer = "Control_NamingContainer";

		// Token: 0x0400044B RID: 1099
		internal const string Control_Page = "Control_Page";

		// Token: 0x0400044C RID: 1100
		internal const string Control_Parent = "Control_Parent";

		// Token: 0x0400044D RID: 1101
		internal const string Control_TemplateSourceDirectory = "Control_TemplateSourceDirectory";

		// Token: 0x0400044E RID: 1102
		internal const string Control_TemplateControl = "Control_TemplateControl";

		// Token: 0x0400044F RID: 1103
		internal const string Control_Site = "Control_Site";

		// Token: 0x04000450 RID: 1104
		internal const string Control_State = "Control_State";

		// Token: 0x04000451 RID: 1105
		internal const string Control_UniqueID = "Control_UniqueID";

		// Token: 0x04000452 RID: 1106
		internal const string Control_ClientID = "Control_ClientID";

		// Token: 0x04000453 RID: 1107
		internal const string Control_SkinId = "Control_SkinId";

		// Token: 0x04000454 RID: 1108
		internal const string Control_EnableTheming = "Control_EnableTheming";

		// Token: 0x04000455 RID: 1109
		internal const string Page_ClientTarget = "Page_ClientTarget";

		// Token: 0x04000456 RID: 1110
		internal const string Page_ErrorPage = "Page_ErrorPage";

		// Token: 0x04000457 RID: 1111
		internal const string Page_Error = "Page_Error";

		// Token: 0x04000458 RID: 1112
		internal const string Page_OnCommitTransaction = "Page_OnCommitTransaction";

		// Token: 0x04000459 RID: 1113
		internal const string Page_OnAbortTransaction = "Page_OnAbortTransaction";

		// Token: 0x0400045A RID: 1114
		internal const string Page_Illegal_MaxPageStateFieldLength = "Page_Illegal_MaxPageStateFieldLength";

		// Token: 0x0400045B RID: 1115
		internal const string Page_Illegal_AsyncTimeout = "Page_Illegal_AsyncTimeout";

		// Token: 0x0400045C RID: 1116
		internal const string ObjectDataSource_ConflictDetection = "ObjectDataSource_ConflictDetection";

		// Token: 0x0400045D RID: 1117
		internal const string ObjectDataSource_ConvertNullToDBNull = "ObjectDataSource_ConvertNullToDBNull";

		// Token: 0x0400045E RID: 1118
		internal const string ObjectDataSource_DataObjectTypeName = "ObjectDataSource_DataObjectTypeName";

		// Token: 0x0400045F RID: 1119
		internal const string ObjectDataSource_DeleteMethod = "ObjectDataSource_DeleteMethod";

		// Token: 0x04000460 RID: 1120
		internal const string ObjectDataSource_DeleteParameters = "ObjectDataSource_DeleteParameters";

		// Token: 0x04000461 RID: 1121
		internal const string ObjectDataSource_EnablePaging = "ObjectDataSource_EnablePaging";

		// Token: 0x04000462 RID: 1122
		internal const string ObjectDataSource_FilterExpression = "ObjectDataSource_FilterExpression";

		// Token: 0x04000463 RID: 1123
		internal const string ObjectDataSource_FilterParameters = "ObjectDataSource_FilterParameters";

		// Token: 0x04000464 RID: 1124
		internal const string ObjectDataSource_InsertMethod = "ObjectDataSource_InsertMethod";

		// Token: 0x04000465 RID: 1125
		internal const string ObjectDataSource_InsertParameters = "ObjectDataSource_InsertParameters";

		// Token: 0x04000466 RID: 1126
		internal const string ObjectDataSource_MaximumRowsParameterName = "ObjectDataSource_MaximumRowsParameterName";

		// Token: 0x04000467 RID: 1127
		internal const string ObjectDataSource_SelectCountMethod = "ObjectDataSource_SelectCountMethod";

		// Token: 0x04000468 RID: 1128
		internal const string ObjectDataSource_SelectMethod = "ObjectDataSource_SelectMethod";

		// Token: 0x04000469 RID: 1129
		internal const string ObjectDataSource_SelectParameters = "ObjectDataSource_SelectParameters";

		// Token: 0x0400046A RID: 1130
		internal const string ObjectDataSource_SortParameterName = "ObjectDataSource_SortParameterName";

		// Token: 0x0400046B RID: 1131
		internal const string ObjectDataSource_StartRowIndexParameterName = "ObjectDataSource_StartRowIndexParameterName";

		// Token: 0x0400046C RID: 1132
		internal const string ObjectDataSource_TypeName = "ObjectDataSource_TypeName";

		// Token: 0x0400046D RID: 1133
		internal const string ObjectDataSource_UpdateMethod = "ObjectDataSource_UpdateMethod";

		// Token: 0x0400046E RID: 1134
		internal const string ObjectDataSource_UpdateParameters = "ObjectDataSource_UpdateParameters";

		// Token: 0x0400046F RID: 1135
		internal const string ObjectDataSource_ObjectCreated = "ObjectDataSource_ObjectCreated";

		// Token: 0x04000470 RID: 1136
		internal const string ObjectDataSource_ObjectCreating = "ObjectDataSource_ObjectCreating";

		// Token: 0x04000471 RID: 1137
		internal const string ObjectDataSource_ObjectDisposing = "ObjectDataSource_ObjectDisposing";

		// Token: 0x04000472 RID: 1138
		internal const string ObjectDataSource_Selected = "ObjectDataSource_Selected";

		// Token: 0x04000473 RID: 1139
		internal const string ObjectDataSource_Selecting = "ObjectDataSource_Selecting";

		// Token: 0x04000474 RID: 1140
		internal const string DataSourceCache_Duration = "DataSourceCache_Duration";

		// Token: 0x04000475 RID: 1141
		internal const string DataSourceCache_Enabled = "DataSourceCache_Enabled";

		// Token: 0x04000476 RID: 1142
		internal const string DataSourceCache_ExpirationPolicy = "DataSourceCache_ExpirationPolicy";

		// Token: 0x04000477 RID: 1143
		internal const string DataSourceCache_KeyDependency = "DataSourceCache_KeyDependency";

		// Token: 0x04000478 RID: 1144
		internal const string SqlDataSource_ConflictDetection = "SqlDataSource_ConflictDetection";

		// Token: 0x04000479 RID: 1145
		internal const string SqlDataSource_ConnectionString = "SqlDataSource_ConnectionString";

		// Token: 0x0400047A RID: 1146
		internal const string SqlDataSource_CancelSelectOnNullParameter = "SqlDataSource_CancelSelectOnNullParameter";

		// Token: 0x0400047B RID: 1147
		internal const string SqlDataSource_ProviderName = "SqlDataSource_ProviderName";

		// Token: 0x0400047C RID: 1148
		internal const string SqlDataSource_DataSourceMode = "SqlDataSource_DataSourceMode";

		// Token: 0x0400047D RID: 1149
		internal const string SqlDataSource_DeleteCommand = "SqlDataSource_DeleteCommand";

		// Token: 0x0400047E RID: 1150
		internal const string SqlDataSource_DeleteCommandType = "SqlDataSource_DeleteCommandType";

		// Token: 0x0400047F RID: 1151
		internal const string SqlDataSource_DeleteParameters = "SqlDataSource_DeleteParameters";

		// Token: 0x04000480 RID: 1152
		internal const string SqlDataSource_FilterExpression = "SqlDataSource_FilterExpression";

		// Token: 0x04000481 RID: 1153
		internal const string SqlDataSource_FilterParameters = "SqlDataSource_FilterParameters";

		// Token: 0x04000482 RID: 1154
		internal const string SqlDataSource_InsertCommand = "SqlDataSource_InsertCommand";

		// Token: 0x04000483 RID: 1155
		internal const string SqlDataSource_InsertCommandType = "SqlDataSource_InsertCommandType";

		// Token: 0x04000484 RID: 1156
		internal const string SqlDataSource_InsertParameters = "SqlDataSource_InsertParameters";

		// Token: 0x04000485 RID: 1157
		internal const string SqlDataSource_SelectCommand = "SqlDataSource_SelectCommand";

		// Token: 0x04000486 RID: 1158
		internal const string SqlDataSource_SelectCommandType = "SqlDataSource_SelectCommandType";

		// Token: 0x04000487 RID: 1159
		internal const string SqlDataSource_SelectParameters = "SqlDataSource_SelectParameters";

		// Token: 0x04000488 RID: 1160
		internal const string SqlDataSource_SortParameterName = "SqlDataSource_SortParameterName";

		// Token: 0x04000489 RID: 1161
		internal const string SqlDataSource_UpdateCommand = "SqlDataSource_UpdateCommand";

		// Token: 0x0400048A RID: 1162
		internal const string SqlDataSource_UpdateCommandType = "SqlDataSource_UpdateCommandType";

		// Token: 0x0400048B RID: 1163
		internal const string SqlDataSource_UpdateParameters = "SqlDataSource_UpdateParameters";

		// Token: 0x0400048C RID: 1164
		internal const string SqlDataSource_Selected = "SqlDataSource_Selected";

		// Token: 0x0400048D RID: 1165
		internal const string SqlDataSource_Selecting = "SqlDataSource_Selecting";

		// Token: 0x0400048E RID: 1166
		internal const string SqlDataSourceCache_SqlCacheDependency = "SqlDataSourceCache_SqlCacheDependency";

		// Token: 0x0400048F RID: 1167
		internal const string Parameter_DbType = "Parameter_DbType";

		// Token: 0x04000490 RID: 1168
		internal const string Parameter_DefaultValue = "Parameter_DefaultValue";

		// Token: 0x04000491 RID: 1169
		internal const string Parameter_Direction = "Parameter_Direction";

		// Token: 0x04000492 RID: 1170
		internal const string Parameter_Name = "Parameter_Name";

		// Token: 0x04000493 RID: 1171
		internal const string Parameter_Size = "Parameter_Size";

		// Token: 0x04000494 RID: 1172
		internal const string Parameter_ConvertEmptyStringToNull = "Parameter_ConvertEmptyStringToNull";

		// Token: 0x04000495 RID: 1173
		internal const string Parameter_Type = "Parameter_Type";

		// Token: 0x04000496 RID: 1174
		internal const string Parameter_TypeNotSupported = "Parameter_TypeNotSupported";

		// Token: 0x04000497 RID: 1175
		internal const string ControlParameter_ControlID = "ControlParameter_ControlID";

		// Token: 0x04000498 RID: 1176
		internal const string ControlParameter_PropertyName = "ControlParameter_PropertyName";

		// Token: 0x04000499 RID: 1177
		internal const string CookieParameter_CookieName = "CookieParameter_CookieName";

		// Token: 0x0400049A RID: 1178
		internal const string QueryStringParameter_QueryStringField = "QueryStringParameter_QueryStringField";

		// Token: 0x0400049B RID: 1179
		internal const string FormParameter_FormField = "FormParameter_FormField";

		// Token: 0x0400049C RID: 1180
		internal const string SessionParameter_SessionField = "SessionParameter_SessionField";

		// Token: 0x0400049D RID: 1181
		internal const string ProfileParameter_PropertyName = "ProfileParameter_PropertyName";

		// Token: 0x0400049E RID: 1182
		internal const string HtmlInputHidden_OnServerChange = "HtmlInputHidden_OnServerChange";

		// Token: 0x0400049F RID: 1183
		internal const string HtmlInputImage_OnServerClick = "HtmlInputImage_OnServerClick";

		// Token: 0x040004A0 RID: 1184
		internal const string HtmlInputText_ServerChange = "HtmlInputText_ServerChange";

		// Token: 0x040004A1 RID: 1185
		internal const string HtmlSelect_DataTextField = "HtmlSelect_DataTextField";

		// Token: 0x040004A2 RID: 1186
		internal const string HtmlSelect_DataValueField = "HtmlSelect_DataValueField";

		// Token: 0x040004A3 RID: 1187
		internal const string HtmlSelect_OnServerChange = "HtmlSelect_OnServerChange";

		// Token: 0x040004A4 RID: 1188
		internal const string HtmlSelect_DataMember = "HtmlSelect_DataMember";

		// Token: 0x040004A5 RID: 1189
		internal const string HtmlTextArea_OnServerChange = "HtmlTextArea_OnServerChange";

		// Token: 0x040004A6 RID: 1190
		internal const string AccessDataSource_DataFile = "AccessDataSource_DataFile";

		// Token: 0x040004A7 RID: 1191
		internal const string AdRotator_AdvertisementFile = "AdRotator_AdvertisementFile";

		// Token: 0x040004A8 RID: 1192
		internal const string AdRotator_AlternateTextField = "AdRotator_AlternateTextField";

		// Token: 0x040004A9 RID: 1193
		internal const string AdRotator_ImageUrlField = "AdRotator_ImageUrlField";

		// Token: 0x040004AA RID: 1194
		internal const string AdRotator_KeywordFilter = "AdRotator_KeywordFilter";

		// Token: 0x040004AB RID: 1195
		internal const string AdRotator_NavigateUrlField = "AdRotator_NavigateUrlField";

		// Token: 0x040004AC RID: 1196
		internal const string AdRotator_Target = "AdRotator_Target";

		// Token: 0x040004AD RID: 1197
		internal const string AdRotator_OnAdCreated = "AdRotator_OnAdCreated";

		// Token: 0x040004AE RID: 1198
		internal const string AssemblyResourceLoader_HandlerNotRegistered = "AssemblyResourceLoader_HandlerNotRegistered";

		// Token: 0x040004AF RID: 1199
		internal const string AssemblyResourceLoader_InvalidRequest = "AssemblyResourceLoader_InvalidRequest";

		// Token: 0x040004B0 RID: 1200
		internal const string AssemblyResourceLoader_AssemblyNotFound = "AssemblyResourceLoader_AssemblyNotFound";

		// Token: 0x040004B1 RID: 1201
		internal const string AssemblyResourceLoader_ResourceNotFound = "AssemblyResourceLoader_ResourceNotFound";

		// Token: 0x040004B2 RID: 1202
		internal const string AssemblyResourceLoader_NoCircularReferences = "AssemblyResourceLoader_NoCircularReferences";

		// Token: 0x040004B3 RID: 1203
		internal const string DataControls_ShowFooter = "DataControls_ShowFooter";

		// Token: 0x040004B4 RID: 1204
		internal const string DataControls_ShowHeader = "DataControls_ShowHeader";

		// Token: 0x040004B5 RID: 1205
		internal const string DataControls_AutoGenerateColumns = "DataControls_AutoGenerateColumns";

		// Token: 0x040004B6 RID: 1206
		internal const string Button_CausesValidation = "Button_CausesValidation";

		// Token: 0x040004B7 RID: 1207
		internal const string WebControl_RepeatLayout = "WebControl_RepeatLayout";

		// Token: 0x040004B8 RID: 1208
		internal const string DataSource_Updating = "DataSource_Updating";

		// Token: 0x040004B9 RID: 1209
		internal const string DataSource_Inserting = "DataSource_Inserting";

		// Token: 0x040004BA RID: 1210
		internal const string DataSource_Deleting = "DataSource_Deleting";

		// Token: 0x040004BB RID: 1211
		internal const string DataSource_Updated = "DataSource_Updated";

		// Token: 0x040004BC RID: 1212
		internal const string DataSource_Inserted = "DataSource_Inserted";

		// Token: 0x040004BD RID: 1213
		internal const string DataSource_Deleted = "DataSource_Deleted";

		// Token: 0x040004BE RID: 1214
		internal const string TableItem_VerticalAlign = "TableItem_VerticalAlign";

		// Token: 0x040004BF RID: 1215
		internal const string Button_PostBackUrl = "Button_PostBackUrl";

		// Token: 0x040004C0 RID: 1216
		internal const string LoginControls_DefaultRequiredFieldValidatorText = "LoginControls_DefaultRequiredFieldValidatorText";

		// Token: 0x040004C1 RID: 1217
		internal const string LoginControls_SuccessPageUrl = "LoginControls_SuccessPageUrl";

		// Token: 0x040004C2 RID: 1218
		internal const string LoginControls_EditProfileIconUrl = "LoginControls_EditProfileIconUrl";

		// Token: 0x040004C3 RID: 1219
		internal const string LoginControls_HelpPageIconUrl = "LoginControls_HelpPageIconUrl";

		// Token: 0x040004C4 RID: 1220
		internal const string LoginControls_HelpPageUrl = "LoginControls_HelpPageUrl";

		// Token: 0x040004C5 RID: 1221
		internal const string ChangePassword_ChangePasswordButtonImageUrl = "ChangePassword_ChangePasswordButtonImageUrl";

		// Token: 0x040004C6 RID: 1222
		internal const string ChangePassword_ContinueButtonImageUrl = "ChangePassword_ContinueButtonImageUrl";

		// Token: 0x040004C7 RID: 1223
		internal const string PagerSettings_PreviousPageText = "PagerSettings_PreviousPageText";

		// Token: 0x040004C8 RID: 1224
		internal const string PagerSettings_NextPageText = "PagerSettings_NextPageText";

		// Token: 0x040004C9 RID: 1225
		internal const string ChangePassword_UserNameRequiredErrorMessage = "ChangePassword_UserNameRequiredErrorMessage";

		// Token: 0x040004CA RID: 1226
		internal const string ChangePassword_ConfirmPasswordCompareErrorMessage = "ChangePassword_ConfirmPasswordCompareErrorMessage";

		// Token: 0x040004CB RID: 1227
		internal const string LoginControls_ConfirmPasswordRequiredErrorMessage = "LoginControls_ConfirmPasswordRequiredErrorMessage";

		// Token: 0x040004CC RID: 1228
		internal const string LoginControls_AnswerRequiredErrorMessage = "LoginControls_AnswerRequiredErrorMessage";

		// Token: 0x040004CD RID: 1229
		internal const string LoginControls_TitleText = "LoginControls_TitleText";

		// Token: 0x040004CE RID: 1230
		internal const string ChangePassword_PasswordRecoveryText = "ChangePassword_PasswordRecoveryText";

		// Token: 0x040004CF RID: 1231
		internal const string ChangePassword_ChangePasswordButtonText = "ChangePassword_ChangePasswordButtonText";

		// Token: 0x040004D0 RID: 1232
		internal const string ChangePassword_HelpPageText = "ChangePassword_HelpPageText";

		// Token: 0x040004D1 RID: 1233
		internal const string ChangePassword_CreateUserText = "ChangePassword_CreateUserText";

		// Token: 0x040004D2 RID: 1234
		internal const string ChangePassword_SuccessText = "ChangePassword_SuccessText";

		// Token: 0x040004D3 RID: 1235
		internal const string LoginControls_UserNameLabelText = "LoginControls_UserNameLabelText";

		// Token: 0x040004D4 RID: 1236
		internal const string WebControl_SkipLinkText = "WebControl_SkipLinkText";

		// Token: 0x040004D5 RID: 1237
		internal const string View_HeaderText = "View_HeaderText";

		// Token: 0x040004D6 RID: 1238
		internal const string View_FooterText = "View_FooterText";

		// Token: 0x040004D7 RID: 1239
		internal const string View_EmptyDataText = "View_EmptyDataText";

		// Token: 0x040004D8 RID: 1240
		internal const string BoundField_NullDisplayText = "BoundField_NullDisplayText";

		// Token: 0x040004D9 RID: 1241
		internal const string View_PagerTemplate = "View_PagerTemplate";

		// Token: 0x040004DA RID: 1242
		internal const string WebControl_HeaderTemplate = "WebControl_HeaderTemplate";

		// Token: 0x040004DB RID: 1243
		internal const string View_EmptyDataTemplate = "View_EmptyDataTemplate";

		// Token: 0x040004DC RID: 1244
		internal const string LoginControls_TitleTextStyle = "LoginControls_TitleTextStyle";

		// Token: 0x040004DD RID: 1245
		internal const string LoginControls_TextBoxStyle = "LoginControls_TextBoxStyle";

		// Token: 0x040004DE RID: 1246
		internal const string LoginControls_LabelStyle = "LoginControls_LabelStyle";

		// Token: 0x040004DF RID: 1247
		internal const string WebControl_InstructionTextStyle = "WebControl_InstructionTextStyle";

		// Token: 0x040004E0 RID: 1248
		internal const string WebControl_HyperLinkStyle = "WebControl_HyperLinkStyle";

		// Token: 0x040004E1 RID: 1249
		internal const string WebControl_FailureTextStyle = "WebControl_FailureTextStyle";

		// Token: 0x040004E2 RID: 1250
		internal const string View_EmptyDataRowStyle = "View_EmptyDataRowStyle";

		// Token: 0x040004E3 RID: 1251
		internal const string WebControl_HeaderStyle = "WebControl_HeaderStyle";

		// Token: 0x040004E4 RID: 1252
		internal const string View_RowStyle = "View_RowStyle";

		// Token: 0x040004E5 RID: 1253
		internal const string View_InsertRowStyle = "View_InsertRowStyle";

		// Token: 0x040004E6 RID: 1254
		internal const string View_EditRowStyle = "View_EditRowStyle";

		// Token: 0x040004E7 RID: 1255
		internal const string DataControls_Columns = "DataControls_Columns";

		// Token: 0x040004E8 RID: 1256
		internal const string HotSpot_Target = "HotSpot_Target";

		// Token: 0x040004E9 RID: 1257
		internal const string MembershipProvider_Name = "MembershipProvider_Name";

		// Token: 0x040004EA RID: 1258
		internal const string View_DefaultMode = "View_DefaultMode";

		// Token: 0x040004EB RID: 1259
		internal const string LoginControls_TextLayout = "LoginControls_TextLayout";

		// Token: 0x040004EC RID: 1260
		internal const string UserName_InitialValue = "UserName_InitialValue";

		// Token: 0x040004ED RID: 1261
		internal const string WebControl_SelectedIndex = "WebControl_SelectedIndex";

		// Token: 0x040004EE RID: 1262
		internal const string View_DataSourceReturnedNullView = "View_DataSourceReturnedNullView";

		// Token: 0x040004EF RID: 1263
		internal const string WebControl_HorizontalAlign = "WebControl_HorizontalAlign";

		// Token: 0x040004F0 RID: 1264
		internal const string TableItem_HorizontalAlign = "TableItem_HorizontalAlign";

		// Token: 0x040004F1 RID: 1265
		internal const string DataSource_OldValuesParameterFormatString = "DataSource_OldValuesParameterFormatString";

		// Token: 0x040004F2 RID: 1266
		internal const string Binding_DataMember = "Binding_DataMember";

		// Token: 0x040004F3 RID: 1267
		internal const string Item_RepeatDirection = "Item_RepeatDirection";

		// Token: 0x040004F4 RID: 1268
		internal const string DataControls_Caption = "DataControls_Caption";

		// Token: 0x040004F5 RID: 1269
		internal const string DataSource_InvalidViewName = "DataSource_InvalidViewName";

		// Token: 0x040004F6 RID: 1270
		internal const string WebControl_CommandName = "WebControl_CommandName";

		// Token: 0x040004F7 RID: 1271
		internal const string WebControl_CommandArgument = "WebControl_CommandArgument";

		// Token: 0x040004F8 RID: 1272
		internal const string WebControl_BackImageUrl = "WebControl_BackImageUrl";

		// Token: 0x040004F9 RID: 1273
		internal const string WebControl_TextAlign = "WebControl_TextAlign";

		// Token: 0x040004FA RID: 1274
		internal const string WebControl_CaptionAlign = "WebControl_CaptionAlign";

		// Token: 0x040004FB RID: 1275
		internal const string WebControl_InstructionText = "WebControl_InstructionText";

		// Token: 0x040004FC RID: 1276
		internal const string DataControls_HeaderStyle = "DataControls_HeaderStyle";

		// Token: 0x040004FD RID: 1277
		internal const string DataControls_FooterStyle = "DataControls_FooterStyle";

		// Token: 0x040004FE RID: 1278
		internal const string HotSpot_HotSpotMode = "HotSpot_HotSpotMode";

		// Token: 0x040004FF RID: 1279
		internal const string DataControls_GridLines = "DataControls_GridLines";

		// Token: 0x04000500 RID: 1280
		internal const string Password_InvalidPasswordErrorMessage = "Password_InvalidPasswordErrorMessage";

		// Token: 0x04000501 RID: 1281
		internal const string Table_UseAccessibleHeader = "Table_UseAccessibleHeader";

		// Token: 0x04000502 RID: 1282
		internal const string HtmlControl_OnServerClick = "HtmlControl_OnServerClick";

		// Token: 0x04000503 RID: 1283
		internal const string Button_OnCommand = "Button_OnCommand";

		// Token: 0x04000504 RID: 1284
		internal const string Control_OnServerCheckChanged = "Control_OnServerCheckChanged";

		// Token: 0x04000505 RID: 1285
		internal const string DataControls_OnItemUpdated = "DataControls_OnItemUpdated";

		// Token: 0x04000506 RID: 1286
		internal const string DataControls_OnItemDeleting = "DataControls_OnItemDeleting";

		// Token: 0x04000507 RID: 1287
		internal const string DataControls_OnItemInserting = "DataControls_OnItemInserting";

		// Token: 0x04000508 RID: 1288
		internal const string DataControls_OnItemUpdating = "DataControls_OnItemUpdating";

		// Token: 0x04000509 RID: 1289
		internal const string DataControls_OnItemCreated = "DataControls_OnItemCreated";

		// Token: 0x0400050A RID: 1290
		internal const string DataControls_OnItemDataBound = "DataControls_OnItemDataBound";

		// Token: 0x0400050B RID: 1291
		internal const string DataControls_OnItemDeleted = "DataControls_OnItemDeleted";

		// Token: 0x0400050C RID: 1292
		internal const string DataControls_OnItemInserted = "DataControls_OnItemInserted";

		// Token: 0x0400050D RID: 1293
		internal const string DataControls_DataKeyNames = "DataControls_DataKeyNames";

		// Token: 0x0400050E RID: 1294
		internal const string DataControls_DataSourceMustBeCollectionWhenNotDataBinding = "DataControls_DataSourceMustBeCollectionWhenNotDataBinding";

		// Token: 0x0400050F RID: 1295
		internal const string DataControls_OnRowDeleted = "DataControls_OnRowDeleted";

		// Token: 0x04000510 RID: 1296
		internal const string DataSource_Filtering = "DataSource_Filtering";

		// Token: 0x04000511 RID: 1297
		internal const string WebControl_PagerStyle = "WebControl_PagerStyle";

		// Token: 0x04000512 RID: 1298
		internal const string WebControl_CantFindProvider = "WebControl_CantFindProvider";

		// Token: 0x04000513 RID: 1299
		internal const string BaseDataList_CellPadding = "BaseDataList_CellPadding";

		// Token: 0x04000514 RID: 1300
		internal const string BaseDataList_CellSpacing = "BaseDataList_CellSpacing";

		// Token: 0x04000515 RID: 1301
		internal const string BaseDataList_DataKeyField = "BaseDataList_DataKeyField";

		// Token: 0x04000516 RID: 1302
		internal const string BaseDataList_DataKeys = "BaseDataList_DataKeys";

		// Token: 0x04000517 RID: 1303
		internal const string BaseDataList_DataMember = "BaseDataList_DataMember";

		// Token: 0x04000518 RID: 1304
		internal const string BaseDataList_OnSelectedIndexChanged = "BaseDataList_OnSelectedIndexChanged";

		// Token: 0x04000519 RID: 1305
		internal const string BaseValidator_ControlToValidate = "BaseValidator_ControlToValidate";

		// Token: 0x0400051A RID: 1306
		internal const string BaseValidator_ErrorMessage = "BaseValidator_ErrorMessage";

		// Token: 0x0400051B RID: 1307
		internal const string BaseValidator_IsValid = "BaseValidator_IsValid";

		// Token: 0x0400051C RID: 1308
		internal const string BaseValidator_Display = "BaseValidator_Display";

		// Token: 0x0400051D RID: 1309
		internal const string BaseValidator_EnableClientScript = "BaseValidator_EnableClientScript";

		// Token: 0x0400051E RID: 1310
		internal const string BaseValidator_SetFocusOnError = "BaseValidator_SetFocusOnError";

		// Token: 0x0400051F RID: 1311
		internal const string BaseValidator_Text = "BaseValidator_Text";

		// Token: 0x04000520 RID: 1312
		internal const string BaseValidator_ValidationGroup = "BaseValidator_ValidationGroup";

		// Token: 0x04000521 RID: 1313
		internal const string BaseCompareValidator_CultureInvariantValues = "BaseCompareValidator_CultureInvariantValues";

		// Token: 0x04000522 RID: 1314
		internal const string BoundColumn_DataField = "BoundColumn_DataField";

		// Token: 0x04000523 RID: 1315
		internal const string BoundColumn_DataFormatString = "BoundColumn_DataFormatString";

		// Token: 0x04000524 RID: 1316
		internal const string BoundColumn_ReadOnly = "BoundColumn_ReadOnly";

		// Token: 0x04000525 RID: 1317
		internal const string BoundField_ApplyFormatInEditMode = "BoundField_ApplyFormatInEditMode";

		// Token: 0x04000526 RID: 1318
		internal const string BoundField_DataField = "BoundField_DataField";

		// Token: 0x04000527 RID: 1319
		internal const string BoundField_DataFormatString = "BoundField_DataFormatString";

		// Token: 0x04000528 RID: 1320
		internal const string BoundField_HtmlEncode = "BoundField_HtmlEncode";

		// Token: 0x04000529 RID: 1321
		internal const string BoundField_ReadOnly = "BoundField_ReadOnly";

		// Token: 0x0400052A RID: 1322
		internal const string BoundField_ConvertEmptyStringToNull = "BoundField_ConvertEmptyStringToNull";

		// Token: 0x0400052B RID: 1323
		internal const string BulletedList_BulletedListDisplayMode = "BulletedList_BulletedListDisplayMode";

		// Token: 0x0400052C RID: 1324
		internal const string BulletedList_BulletImageUrl = "BulletedList_BulletImageUrl";

		// Token: 0x0400052D RID: 1325
		internal const string BulletedList_BulletStyle = "BulletedList_BulletStyle";

		// Token: 0x0400052E RID: 1326
		internal const string BulletedList_FirstBulletNumber = "BulletedList_FirstBulletNumber";

		// Token: 0x0400052F RID: 1327
		internal const string BulletedList_Target = "BulletedList_Target";

		// Token: 0x04000530 RID: 1328
		internal const string BulletedList_OnClick = "BulletedList_OnClick";

		// Token: 0x04000531 RID: 1329
		internal const string Button_OnClientClick = "Button_OnClientClick";

		// Token: 0x04000532 RID: 1330
		internal const string ButtonColumn_ButtonType = "ButtonColumn_ButtonType";

		// Token: 0x04000533 RID: 1331
		internal const string ButtonColumn_CausesValidation = "ButtonColumn_CausesValidation";

		// Token: 0x04000534 RID: 1332
		internal const string ButtonColumn_DataTextField = "ButtonColumn_DataTextField";

		// Token: 0x04000535 RID: 1333
		internal const string ButtonColumn_DataTextFormatString = "ButtonColumn_DataTextFormatString";

		// Token: 0x04000536 RID: 1334
		internal const string ButtonColumn_Text = "ButtonColumn_Text";

		// Token: 0x04000537 RID: 1335
		internal const string ButtonColumn_ValidationGroup = "ButtonColumn_ValidationGroup";

		// Token: 0x04000538 RID: 1336
		internal const string Button_Text = "Button_Text";

		// Token: 0x04000539 RID: 1337
		internal const string Button_OnClick = "Button_OnClick";

		// Token: 0x0400053A RID: 1338
		internal const string Button_UseSubmitBehavior = "Button_UseSubmitBehavior";

		// Token: 0x0400053B RID: 1339
		internal const string CheckBox_AutoPostBack = "CheckBox_AutoPostBack";

		// Token: 0x0400053C RID: 1340
		internal const string CheckBox_Checked = "CheckBox_Checked";

		// Token: 0x0400053D RID: 1341
		internal const string CheckBox_InputAttributes = "CheckBox_InputAttributes";

		// Token: 0x0400053E RID: 1342
		internal const string CheckBox_LabelAttributes = "CheckBox_LabelAttributes";

		// Token: 0x0400053F RID: 1343
		internal const string CheckBox_Text = "CheckBox_Text";

		// Token: 0x04000540 RID: 1344
		internal const string CheckBoxField_Text = "CheckBoxField_Text";

		// Token: 0x04000541 RID: 1345
		internal const string CheckBoxList_CellPadding = "CheckBoxList_CellPadding";

		// Token: 0x04000542 RID: 1346
		internal const string CheckBoxList_CellSpacing = "CheckBoxList_CellSpacing";

		// Token: 0x04000543 RID: 1347
		internal const string CheckBoxList_RepeatColumns = "CheckBoxList_RepeatColumns";

		// Token: 0x04000544 RID: 1348
		internal const string CircleHotSpot_X = "CircleHotSpot_X";

		// Token: 0x04000545 RID: 1349
		internal const string CircleHotSpot_Y = "CircleHotSpot_Y";

		// Token: 0x04000546 RID: 1350
		internal const string CircleHotSpot_Radius = "CircleHotSpot_Radius";

		// Token: 0x04000547 RID: 1351
		internal const string CommandField_DefaultCancelCaption = "CommandField_DefaultCancelCaption";

		// Token: 0x04000548 RID: 1352
		internal const string CommandField_DefaultDeleteCaption = "CommandField_DefaultDeleteCaption";

		// Token: 0x04000549 RID: 1353
		internal const string CommandField_DefaultEditCaption = "CommandField_DefaultEditCaption";

		// Token: 0x0400054A RID: 1354
		internal const string CommandField_DefaultInsertCaption = "CommandField_DefaultInsertCaption";

		// Token: 0x0400054B RID: 1355
		internal const string CommandField_DefaultNewCaption = "CommandField_DefaultNewCaption";

		// Token: 0x0400054C RID: 1356
		internal const string CommandField_DefaultSelectCaption = "CommandField_DefaultSelectCaption";

		// Token: 0x0400054D RID: 1357
		internal const string CommandField_DefaultUpdateCaption = "CommandField_DefaultUpdateCaption";

		// Token: 0x0400054E RID: 1358
		internal const string CommandField_CancelImageUrl = "CommandField_CancelImageUrl";

		// Token: 0x0400054F RID: 1359
		internal const string CommandField_DeleteImageUrl = "CommandField_DeleteImageUrl";

		// Token: 0x04000550 RID: 1360
		internal const string CommandField_EditImageUrl = "CommandField_EditImageUrl";

		// Token: 0x04000551 RID: 1361
		internal const string CommandField_InsertImageUrl = "CommandField_InsertImageUrl";

		// Token: 0x04000552 RID: 1362
		internal const string CommandField_NewImageUrl = "CommandField_NewImageUrl";

		// Token: 0x04000553 RID: 1363
		internal const string CommandField_SelectImageUrl = "CommandField_SelectImageUrl";

		// Token: 0x04000554 RID: 1364
		internal const string CommandField_UpdateImageUrl = "CommandField_UpdateImageUrl";

		// Token: 0x04000555 RID: 1365
		internal const string CommandField_ShowDeleteButton = "CommandField_ShowDeleteButton";

		// Token: 0x04000556 RID: 1366
		internal const string CommandField_ShowCancelButton = "CommandField_ShowCancelButton";

		// Token: 0x04000557 RID: 1367
		internal const string CommandField_ShowInsertButton = "CommandField_ShowInsertButton";

		// Token: 0x04000558 RID: 1368
		internal const string CommandField_ShowEditButton = "CommandField_ShowEditButton";

		// Token: 0x04000559 RID: 1369
		internal const string CommandField_ShowSelectButton = "CommandField_ShowSelectButton";

		// Token: 0x0400055A RID: 1370
		internal const string CommandField_CancelText = "CommandField_CancelText";

		// Token: 0x0400055B RID: 1371
		internal const string CommandField_DeleteText = "CommandField_DeleteText";

		// Token: 0x0400055C RID: 1372
		internal const string CommandField_EditText = "CommandField_EditText";

		// Token: 0x0400055D RID: 1373
		internal const string CommandField_InsertText = "CommandField_InsertText";

		// Token: 0x0400055E RID: 1374
		internal const string CommandField_NewText = "CommandField_NewText";

		// Token: 0x0400055F RID: 1375
		internal const string CommandField_SelectText = "CommandField_SelectText";

		// Token: 0x04000560 RID: 1376
		internal const string CommandField_UpdateText = "CommandField_UpdateText";

		// Token: 0x04000561 RID: 1377
		internal const string ButtonFieldBase_ButtonType = "ButtonFieldBase_ButtonType";

		// Token: 0x04000562 RID: 1378
		internal const string ButtonFieldBase_CausesValidation = "ButtonFieldBase_CausesValidation";

		// Token: 0x04000563 RID: 1379
		internal const string ButtonFieldBase_ValidationGroup = "ButtonFieldBase_ValidationGroup";

		// Token: 0x04000564 RID: 1380
		internal const string ButtonField_DataTextField = "ButtonField_DataTextField";

		// Token: 0x04000565 RID: 1381
		internal const string ButtonField_DataTextFormatString = "ButtonField_DataTextFormatString";

		// Token: 0x04000566 RID: 1382
		internal const string ButtonField_ImageUrl = "ButtonField_ImageUrl";

		// Token: 0x04000567 RID: 1383
		internal const string ButtonField_Text = "ButtonField_Text";

		// Token: 0x04000568 RID: 1384
		internal const string ChangePassword_CancelButtonType = "ChangePassword_CancelButtonType";

		// Token: 0x04000569 RID: 1385
		internal const string ChangePassword_ContinueButtonType = "ChangePassword_ContinueButtonType";

		// Token: 0x0400056A RID: 1386
		internal const string ChangePassword_ChangePasswordButtonType = "ChangePassword_ChangePasswordButtonType";

		// Token: 0x0400056B RID: 1387
		internal const string ChangePassword_CancelButtonImageUrl = "ChangePassword_CancelButtonImageUrl";

		// Token: 0x0400056C RID: 1388
		internal const string ChangePassword_CancelButtonText = "ChangePassword_CancelButtonText";

		// Token: 0x0400056D RID: 1389
		internal const string ChangePassword_CancelButtonStyle = "ChangePassword_CancelButtonStyle";

		// Token: 0x0400056E RID: 1390
		internal const string ChangePassword_CancelButtonClick = "ChangePassword_CancelButtonClick";

		// Token: 0x0400056F RID: 1391
		internal const string ChangePassword_CancelDestinationPageUrl = "ChangePassword_CancelDestinationPageUrl";

		// Token: 0x04000570 RID: 1392
		internal const string ChangePassword_ChangePasswordError = "ChangePassword_ChangePasswordError";

		// Token: 0x04000571 RID: 1393
		internal const string ChangePassword_ChangedPassword = "ChangePassword_ChangedPassword";

		// Token: 0x04000572 RID: 1394
		internal const string ChangePassword_ChangingPassword = "ChangePassword_ChangingPassword";

		// Token: 0x04000573 RID: 1395
		internal const string ChangePassword_ChangePasswordFailureText = "ChangePassword_ChangePasswordFailureText";

		// Token: 0x04000574 RID: 1396
		internal const string ChangePassword_ContinueButtonClick = "ChangePassword_ContinueButtonClick";

		// Token: 0x04000575 RID: 1397
		internal const string LoginControls_ContinueDestinationPageUrl = "LoginControls_ContinueDestinationPageUrl";

		// Token: 0x04000576 RID: 1398
		internal const string ChangePassword_ContinueButtonText = "ChangePassword_ContinueButtonText";

		// Token: 0x04000577 RID: 1399
		internal const string ChangePassword_ContinueButtonStyle = "ChangePassword_ContinueButtonStyle";

		// Token: 0x04000578 RID: 1400
		internal const string ChangePassword_CreateUserIconUrl = "ChangePassword_CreateUserIconUrl";

		// Token: 0x04000579 RID: 1401
		internal const string ChangePassword_CreateUserUrl = "ChangePassword_CreateUserUrl";

		// Token: 0x0400057A RID: 1402
		internal const string ChangePassword_DefaultChangePasswordTitleText = "ChangePassword_DefaultChangePasswordTitleText";

		// Token: 0x0400057B RID: 1403
		internal const string ChangePassword_DefaultChangePasswordFailureText = "ChangePassword_DefaultChangePasswordFailureText";

		// Token: 0x0400057C RID: 1404
		internal const string ChangePassword_DefaultCancelButtonText = "ChangePassword_DefaultCancelButtonText";

		// Token: 0x0400057D RID: 1405
		internal const string ChangePassword_DefaultConfirmPasswordRequiredErrorMessage = "ChangePassword_DefaultConfirmPasswordRequiredErrorMessage";

		// Token: 0x0400057E RID: 1406
		internal const string ChangePassword_DefaultConfirmNewPasswordLabelText = "ChangePassword_DefaultConfirmNewPasswordLabelText";

		// Token: 0x0400057F RID: 1407
		internal const string ChangePassword_DefaultContinueButtonText = "ChangePassword_DefaultContinueButtonText";

		// Token: 0x04000580 RID: 1408
		internal const string ChangePassword_DefaultNewPasswordLabelText = "ChangePassword_DefaultNewPasswordLabelText";

		// Token: 0x04000581 RID: 1409
		internal const string ChangePassword_DefaultNewPasswordRequiredErrorMessage = "ChangePassword_DefaultNewPasswordRequiredErrorMessage";

		// Token: 0x04000582 RID: 1410
		internal const string ChangePassword_DefaultConfirmPasswordCompareErrorMessage = "ChangePassword_DefaultConfirmPasswordCompareErrorMessage";

		// Token: 0x04000583 RID: 1411
		internal const string ChangePassword_DefaultPasswordRequiredErrorMessage = "ChangePassword_DefaultPasswordRequiredErrorMessage";

		// Token: 0x04000584 RID: 1412
		internal const string ChangePassword_DefaultChangePasswordButtonText = "ChangePassword_DefaultChangePasswordButtonText";

		// Token: 0x04000585 RID: 1413
		internal const string ChangePassword_DefaultSuccessTitleText = "ChangePassword_DefaultSuccessTitleText";

		// Token: 0x04000586 RID: 1414
		internal const string ChangePassword_DefaultSuccessText = "ChangePassword_DefaultSuccessText";

		// Token: 0x04000587 RID: 1415
		internal const string ChangePassword_DefaultUserNameLabelText = "ChangePassword_DefaultUserNameLabelText";

		// Token: 0x04000588 RID: 1416
		internal const string ChangePassword_DefaultUserNameRequiredErrorMessage = "ChangePassword_DefaultUserNameRequiredErrorMessage";

		// Token: 0x04000589 RID: 1417
		internal const string ChangePassword_EditProfileText = "ChangePassword_EditProfileText";

		// Token: 0x0400058A RID: 1418
		internal const string ChangePassword_EditProfileUrl = "ChangePassword_EditProfileUrl";

		// Token: 0x0400058B RID: 1419
		internal const string ChangePassword_DisplayUserName = "ChangePassword_DisplayUserName";

		// Token: 0x0400058C RID: 1420
		internal const string ChangePassword_InvalidBorderPadding = "ChangePassword_InvalidBorderPadding";

		// Token: 0x0400058D RID: 1421
		internal const string ChangePassword_PasswordHintText = "ChangePassword_PasswordHintText";

		// Token: 0x0400058E RID: 1422
		internal const string ChangePassword_MailDefinition = "ChangePassword_MailDefinition";

		// Token: 0x0400058F RID: 1423
		internal const string ChangePassword_NewPasswordRegularExpressionErrorMessage = "ChangePassword_NewPasswordRegularExpressionErrorMessage";

		// Token: 0x04000590 RID: 1424
		internal const string ChangePassword_NewPasswordLabelText = "ChangePassword_NewPasswordLabelText";

		// Token: 0x04000591 RID: 1425
		internal const string ChangePassword_NewPasswordRegularExpression = "ChangePassword_NewPasswordRegularExpression";

		// Token: 0x04000592 RID: 1426
		internal const string ChangePassword_NewPasswordRequiredErrorMessage = "ChangePassword_NewPasswordRequiredErrorMessage";

		// Token: 0x04000593 RID: 1427
		internal const string ChangePassword_NoCurrentPasswordTextBox = "ChangePassword_NoCurrentPasswordTextBox";

		// Token: 0x04000594 RID: 1428
		internal const string ChangePassword_NoNewPasswordTextBox = "ChangePassword_NoNewPasswordTextBox";

		// Token: 0x04000595 RID: 1429
		internal const string ChangePassword_NoUserNameTextBox = "ChangePassword_NoUserNameTextBox";

		// Token: 0x04000596 RID: 1430
		internal const string ChangePassword_UserNameTextBoxNotAllowed = "ChangePassword_UserNameTextBoxNotAllowed";

		// Token: 0x04000597 RID: 1431
		internal const string ChangePassword_PasswordHintStyle = "ChangePassword_PasswordHintStyle";

		// Token: 0x04000598 RID: 1432
		internal const string ChangePassword_PasswordRecoveryIconUrl = "ChangePassword_PasswordRecoveryIconUrl";

		// Token: 0x04000599 RID: 1433
		internal const string ChangePassword_PasswordRecoveryUrl = "ChangePassword_PasswordRecoveryUrl";

		// Token: 0x0400059A RID: 1434
		internal const string ChangePassword_PasswordRequiredErrorMessage = "ChangePassword_PasswordRequiredErrorMessage";

		// Token: 0x0400059B RID: 1435
		internal const string ChangePassword_SendingMail = "ChangePassword_SendingMail";

		// Token: 0x0400059C RID: 1436
		internal const string ChangePassword_SendMailError = "ChangePassword_SendMailError";

		// Token: 0x0400059D RID: 1437
		internal const string ChangePassword_ChangePasswordButtonStyle = "ChangePassword_ChangePasswordButtonStyle";

		// Token: 0x0400059E RID: 1438
		internal const string ChangePassword_SuccessTitleText = "ChangePassword_SuccessTitleText";

		// Token: 0x0400059F RID: 1439
		internal const string ChangePassword_SuccessTextStyle = "ChangePassword_SuccessTextStyle";

		// Token: 0x040005A0 RID: 1440
		internal const string ChangePassword_ConfirmNewPasswordLabelText = "ChangePassword_ConfirmNewPasswordLabelText";

		// Token: 0x040005A1 RID: 1441
		internal const string ChangePassword_ValidatorTextStyle = "ChangePassword_ValidatorTextStyle";

		// Token: 0x040005A2 RID: 1442
		internal const string CompareValidator_ControlToCompare = "CompareValidator_ControlToCompare";

		// Token: 0x040005A3 RID: 1443
		internal const string CompareValidator_Operator = "CompareValidator_Operator";

		// Token: 0x040005A4 RID: 1444
		internal const string CompareValidator_ValueToCompare = "CompareValidator_ValueToCompare";

		// Token: 0x040005A5 RID: 1445
		internal const string Content_ContentPlaceHolderID = "Content_ContentPlaceHolderID";

		// Token: 0x040005A6 RID: 1446
		internal const string ContentPlaceHolder_only_in_master = "ContentPlaceHolder_only_in_master";

		// Token: 0x040005A7 RID: 1447
		internal const string ContentPlaceHolder_duplicate_contentPlaceHolderID = "ContentPlaceHolder_duplicate_contentPlaceHolderID";

		// Token: 0x040005A8 RID: 1448
		internal const string CreateUserWizard_AutoGeneratePassword = "CreateUserWizard_AutoGeneratePassword";

		// Token: 0x040005A9 RID: 1449
		internal const string CreateUserWizard_Answer = "CreateUserWizard_Answer";

		// Token: 0x040005AA RID: 1450
		internal const string CreateUserWizard_InvalidAnswerErrorMessage = "CreateUserWizard_InvalidAnswerErrorMessage";

		// Token: 0x040005AB RID: 1451
		internal const string CreateUserWizard_AnswerLabelText = "CreateUserWizard_AnswerLabelText";

		// Token: 0x040005AC RID: 1452
		internal const string CreateUserWizard_CompleteSuccessText = "CreateUserWizard_CompleteSuccessText";

		// Token: 0x040005AD RID: 1453
		internal const string CreateUserWizard_ContinueButtonType = "CreateUserWizard_ContinueButtonType";

		// Token: 0x040005AE RID: 1454
		internal const string CreateUserWizard_CreatingUser = "CreateUserWizard_CreatingUser";

		// Token: 0x040005AF RID: 1455
		internal const string CreateUserWizard_CreatedUser = "CreateUserWizard_CreatedUser";

		// Token: 0x040005B0 RID: 1456
		internal const string CreateUserWizard_ConfirmPasswordLabelText = "CreateUserWizard_ConfirmPasswordLabelText";

		// Token: 0x040005B1 RID: 1457
		internal const string CreateUserWizard_ContinueButtonText = "CreateUserWizard_ContinueButtonText";

		// Token: 0x040005B2 RID: 1458
		internal const string CreateUserWizard_ContinueButtonStyle = "CreateUserWizard_ContinueButtonStyle";

		// Token: 0x040005B3 RID: 1459
		internal const string CreateUserWizard_ContinueButtonClick = "CreateUserWizard_ContinueButtonClick";

		// Token: 0x040005B4 RID: 1460
		internal const string CreateUserWizard_CreateUserButtonImageUrl = "CreateUserWizard_CreateUserButtonImageUrl";

		// Token: 0x040005B5 RID: 1461
		internal const string CreateUserWizard_CreateUserButtonType = "CreateUserWizard_CreateUserButtonType";

		// Token: 0x040005B6 RID: 1462
		internal const string CreateUserWizard_CreateUserButtonText = "CreateUserWizard_CreateUserButtonText";

		// Token: 0x040005B7 RID: 1463
		internal const string CreateUserWizard_CreateUserButtonStyle = "CreateUserWizard_CreateUserButtonStyle";

		// Token: 0x040005B8 RID: 1464
		internal const string CreateUserWizard_CreateUserError = "CreateUserWizard_CreateUserError";

		// Token: 0x040005B9 RID: 1465
		internal const string CreateUserWizard_CreateUserStep = "CreateUserWizard_CreateUserStep";

		// Token: 0x040005BA RID: 1466
		internal const string CreateUserWizard_DefaultConfirmPasswordCompareErrorMessage = "CreateUserWizard_DefaultConfirmPasswordCompareErrorMessage";

		// Token: 0x040005BB RID: 1467
		internal const string CreateUserWizard_DefaultConfirmPasswordRequiredErrorMessage = "CreateUserWizard_DefaultConfirmPasswordRequiredErrorMessage";

		// Token: 0x040005BC RID: 1468
		internal const string CreateUserWizard_DefaultConfirmPasswordLabelText = "CreateUserWizard_DefaultConfirmPasswordLabelText";

		// Token: 0x040005BD RID: 1469
		internal const string CreateUserWizard_DefaultContinueButtonText = "CreateUserWizard_DefaultContinueButtonText";

		// Token: 0x040005BE RID: 1470
		internal const string CreateUserWizard_DefaultCreateUserButtonText = "CreateUserWizard_DefaultCreateUserButtonText";

		// Token: 0x040005BF RID: 1471
		internal const string CreateUserWizard_DefaultDuplicateUserNameErrorMessage = "CreateUserWizard_DefaultDuplicateUserNameErrorMessage";

		// Token: 0x040005C0 RID: 1472
		internal const string CreateUserWizard_DefaultDuplicateEmailErrorMessage = "CreateUserWizard_DefaultDuplicateEmailErrorMessage";

		// Token: 0x040005C1 RID: 1473
		internal const string CreateUserWizard_DefaultEmailLabelText = "CreateUserWizard_DefaultEmailLabelText";

		// Token: 0x040005C2 RID: 1474
		internal const string CreateUserWizard_DefaultUnknownErrorMessage = "CreateUserWizard_DefaultUnknownErrorMessage";

		// Token: 0x040005C3 RID: 1475
		internal const string CreateUserWizard_DefaultInvalidEmailErrorMessage = "CreateUserWizard_DefaultInvalidEmailErrorMessage";

		// Token: 0x040005C4 RID: 1476
		internal const string CreateUserWizard_DefaultInvalidPasswordErrorMessage = "CreateUserWizard_DefaultInvalidPasswordErrorMessage";

		// Token: 0x040005C5 RID: 1477
		internal const string CreateUserWizard_DefaultCompleteTitleText = "CreateUserWizard_DefaultCompleteTitleText";

		// Token: 0x040005C6 RID: 1478
		internal const string CreateUserWizard_DefaultPasswordRequiredErrorMessage = "CreateUserWizard_DefaultPasswordRequiredErrorMessage";

		// Token: 0x040005C7 RID: 1479
		internal const string CreateUserWizard_DefaultQuestionLabelText = "CreateUserWizard_DefaultQuestionLabelText";

		// Token: 0x040005C8 RID: 1480
		internal const string CreateUserWizard_DefaultInvalidQuestionErrorMessage = "CreateUserWizard_DefaultInvalidQuestionErrorMessage";

		// Token: 0x040005C9 RID: 1481
		internal const string CreateUserWizard_DefaultInvalidAnswerErrorMessage = "CreateUserWizard_DefaultInvalidAnswerErrorMessage";

		// Token: 0x040005CA RID: 1482
		internal const string CreateUserWizard_DefaultAnswerLabelText = "CreateUserWizard_DefaultAnswerLabelText";

		// Token: 0x040005CB RID: 1483
		internal const string CreateUserWizard_DefaultEmailRegularExpressionErrorMessage = "CreateUserWizard_DefaultEmailRegularExpressionErrorMessage";

		// Token: 0x040005CC RID: 1484
		internal const string CreateUserWizard_DefaultCompleteSuccessText = "CreateUserWizard_DefaultCompleteSuccessText";

		// Token: 0x040005CD RID: 1485
		internal const string CreateUserWizard_DefaultCreateUserTitleText = "CreateUserWizard_DefaultCreateUserTitleText";

		// Token: 0x040005CE RID: 1486
		internal const string CreateUserWizard_DefaultUserNameLabelText = "CreateUserWizard_DefaultUserNameLabelText";

		// Token: 0x040005CF RID: 1487
		internal const string CreateUserWizard_DefaultUserNameRequiredErrorMessage = "CreateUserWizard_DefaultUserNameRequiredErrorMessage";

		// Token: 0x040005D0 RID: 1488
		internal const string CreateUserWizard_DefaultAnswerRequiredErrorMessage = "CreateUserWizard_DefaultAnswerRequiredErrorMessage";

		// Token: 0x040005D1 RID: 1489
		internal const string CreateUserWizard_DefaultEmailRequiredErrorMessage = "CreateUserWizard_DefaultEmailRequiredErrorMessage";

		// Token: 0x040005D2 RID: 1490
		internal const string CreateUserWizard_DefaultQuestionRequiredErrorMessage = "CreateUserWizard_DefaultQuestionRequiredErrorMessage";

		// Token: 0x040005D3 RID: 1491
		internal const string CreateUserWizard_DuplicateEmailErrorMessage = "CreateUserWizard_DuplicateEmailErrorMessage";

		// Token: 0x040005D4 RID: 1492
		internal const string CreateUserWizard_DuplicateUserNameErrorMessage = "CreateUserWizard_DuplicateUserNameErrorMessage";

		// Token: 0x040005D5 RID: 1493
		internal const string CreateUserWizard_EditProfileText = "CreateUserWizard_EditProfileText";

		// Token: 0x040005D6 RID: 1494
		internal const string CreateUserWizard_EditProfileUrl = "CreateUserWizard_EditProfileUrl";

		// Token: 0x040005D7 RID: 1495
		internal const string CreateUserWizard_Email = "CreateUserWizard_Email";

		// Token: 0x040005D8 RID: 1496
		internal const string CreateUserWizard_EmailRegularExpression = "CreateUserWizard_EmailRegularExpression";

		// Token: 0x040005D9 RID: 1497
		internal const string CreateUserWizard_EmailRegularExpressionErrorMessage = "CreateUserWizard_EmailRegularExpressionErrorMessage";

		// Token: 0x040005DA RID: 1498
		internal const string CreateUserWizard_InvalidEmailErrorMessage = "CreateUserWizard_InvalidEmailErrorMessage";

		// Token: 0x040005DB RID: 1499
		internal const string CreateUserWizard_EmailLabelText = "CreateUserWizard_EmailLabelText";

		// Token: 0x040005DC RID: 1500
		internal const string CreateUserWizard_UnknownErrorMessage = "CreateUserWizard_UnknownErrorMessage";

		// Token: 0x040005DD RID: 1501
		internal const string CreateUserWizard_CompleteStep = "CreateUserWizard_CompleteStep";

		// Token: 0x040005DE RID: 1502
		internal const string CreateUserWizard_DisableCreatedUser = "CreateUserWizard_DisableCreatedUser";

		// Token: 0x040005DF RID: 1503
		internal const string CreateUserWizard_LoginCreatedUser = "CreateUserWizard_LoginCreatedUser";

		// Token: 0x040005E0 RID: 1504
		internal const string CreateUserWizard_QuestionAndAnswerRequired = "CreateUserWizard_QuestionAndAnswerRequired";

		// Token: 0x040005E1 RID: 1505
		internal const string CreateUserWizard_RequireEmail = "CreateUserWizard_RequireEmail";

		// Token: 0x040005E2 RID: 1506
		internal const string CreateUserWizard_ErrorMessageStyle = "CreateUserWizard_ErrorMessageStyle";

		// Token: 0x040005E3 RID: 1507
		internal const string CreateUserWizard_PasswordHintStyle = "CreateUserWizard_PasswordHintStyle";

		// Token: 0x040005E4 RID: 1508
		internal const string CreateUserWizard_MailDefinition = "CreateUserWizard_MailDefinition";

		// Token: 0x040005E5 RID: 1509
		internal const string CreateUserWizard_InvalidPasswordErrorMessage = "CreateUserWizard_InvalidPasswordErrorMessage";

		// Token: 0x040005E6 RID: 1510
		internal const string CreateUserWizard_PasswordRegularExpression = "CreateUserWizard_PasswordRegularExpression";

		// Token: 0x040005E7 RID: 1511
		internal const string CreateUserWizard_PasswordRegularExpressionErrorMessage = "CreateUserWizard_PasswordRegularExpressionErrorMessage";

		// Token: 0x040005E8 RID: 1512
		internal const string CreateUserWizard_PasswordRequiredErrorMessage = "CreateUserWizard_PasswordRequiredErrorMessage";

		// Token: 0x040005E9 RID: 1513
		internal const string CreateUserWizard_NoPasswordTextBox = "CreateUserWizard_NoPasswordTextBox";

		// Token: 0x040005EA RID: 1514
		internal const string CreateUserWizard_NoUserNameTextBox = "CreateUserWizard_NoUserNameTextBox";

		// Token: 0x040005EB RID: 1515
		internal const string CreateUserWizard_NoEmailTextBox = "CreateUserWizard_NoEmailTextBox";

		// Token: 0x040005EC RID: 1516
		internal const string CreateUserWizard_NoQuestionTextBox = "CreateUserWizard_NoQuestionTextBox";

		// Token: 0x040005ED RID: 1517
		internal const string CreateUserWizard_NoAnswerTextBox = "CreateUserWizard_NoAnswerTextBox";

		// Token: 0x040005EE RID: 1518
		internal const string CreateUserWizard_Question = "CreateUserWizard_Question";

		// Token: 0x040005EF RID: 1519
		internal const string CreateUserWizard_InvalidQuestionErrorMessage = "CreateUserWizard_InvalidQuestionErrorMessage";

		// Token: 0x040005F0 RID: 1520
		internal const string CreateUserWizard_QuestionLabelText = "CreateUserWizard_QuestionLabelText";

		// Token: 0x040005F1 RID: 1521
		internal const string CreateUserWizard_QuestionRequiredErrorMessage = "CreateUserWizard_QuestionRequiredErrorMessage";

		// Token: 0x040005F2 RID: 1522
		internal const string CreateUserWizard_EmailRequiredErrorMessage = "CreateUserWizard_EmailRequiredErrorMessage";

		// Token: 0x040005F3 RID: 1523
		internal const string CreateUserWizard_SendMailError = "CreateUserWizard_SendMailError";

		// Token: 0x040005F4 RID: 1524
		internal const string CreateUserWizard_SideBar_Label_Not_Found = "CreateUserWizard_SideBar_Label_Not_Found";

		// Token: 0x040005F5 RID: 1525
		internal const string CreateUserWizard_CompleteSuccessTextStyle = "CreateUserWizard_CompleteSuccessTextStyle";

		// Token: 0x040005F6 RID: 1526
		internal const string CreateUserWizard_DuplicateCreateUserWizardStep = "CreateUserWizard_DuplicateCreateUserWizardStep";

		// Token: 0x040005F7 RID: 1527
		internal const string CreateUserWizard_DuplicateCompleteWizardStep = "CreateUserWizard_DuplicateCompleteWizardStep";

		// Token: 0x040005F8 RID: 1528
		internal const string CreateUserWizard_ValidatorTextStyle = "CreateUserWizard_ValidatorTextStyle";

		// Token: 0x040005F9 RID: 1529
		internal const string TemplatedWizardStep_ContentTemplate = "TemplatedWizardStep_ContentTemplate";

		// Token: 0x040005FA RID: 1530
		internal const string TemplatedWizardStep_CustomNavigationTemplate = "TemplatedWizardStep_CustomNavigationTemplate";

		// Token: 0x040005FB RID: 1531
		internal const string CreateUserWizardStep_AllowReturnCannotBeSet = "CreateUserWizardStep_AllowReturnCannotBeSet";

		// Token: 0x040005FC RID: 1532
		internal const string CreateUserWizardStep_StepTypeCannotBeSet = "CreateUserWizardStep_StepTypeCannotBeSet";

		// Token: 0x040005FD RID: 1533
		internal const string CreateUserWizardStep_OnlyAllowedInCreateUserWizard = "CreateUserWizardStep_OnlyAllowedInCreateUserWizard";

		// Token: 0x040005FE RID: 1534
		internal const string CompleteWizardStep_OnlyAllowedInCreateUserWizard = "CompleteWizardStep_OnlyAllowedInCreateUserWizard";

		// Token: 0x040005FF RID: 1535
		internal const string CustomValidator_ClientValidationFunction = "CustomValidator_ClientValidationFunction";

		// Token: 0x04000600 RID: 1536
		internal const string CustomValidator_ValidateEmptyText = "CustomValidator_ValidateEmptyText";

		// Token: 0x04000601 RID: 1537
		internal const string CustomValidator_ServerValidate = "CustomValidator_ServerValidate";

		// Token: 0x04000602 RID: 1538
		internal const string BaseDataBoundControl_DataSourceID = "BaseDataBoundControl_DataSourceID";

		// Token: 0x04000603 RID: 1539
		internal const string BaseDataBoundControl_DataSource = "BaseDataBoundControl_DataSource";

		// Token: 0x04000604 RID: 1540
		internal const string BaseDataBoundControl_OnDataBound = "BaseDataBoundControl_OnDataBound";

		// Token: 0x04000605 RID: 1541
		internal const string DataBoundControl_DataMember = "DataBoundControl_DataMember";

		// Token: 0x04000606 RID: 1542
		internal const string DataBoundControl_EnableModelValidation = "DataBoundControl_EnableModelValidation";

		// Token: 0x04000607 RID: 1543
		internal const string DataControlField_AccessibleHeaderText = "DataControlField_AccessibleHeaderText";

		// Token: 0x04000608 RID: 1544
		internal const string DataControlField_ControlStyle = "DataControlField_ControlStyle";

		// Token: 0x04000609 RID: 1545
		internal const string DataControlField_FooterStyle = "DataControlField_FooterStyle";

		// Token: 0x0400060A RID: 1546
		internal const string DataControlField_FooterText = "DataControlField_FooterText";

		// Token: 0x0400060B RID: 1547
		internal const string DataControlField_HeaderImageUrl = "DataControlField_HeaderImageUrl";

		// Token: 0x0400060C RID: 1548
		internal const string DataControlField_HeaderStyle = "DataControlField_HeaderStyle";

		// Token: 0x0400060D RID: 1549
		internal const string DataControlField_HeaderText = "DataControlField_HeaderText";

		// Token: 0x0400060E RID: 1550
		internal const string DataControlField_InsertVisible = "DataControlField_InsertVisible";

		// Token: 0x0400060F RID: 1551
		internal const string DataControlField_ItemStyle = "DataControlField_ItemStyle";

		// Token: 0x04000610 RID: 1552
		internal const string DataControlField_ShowHeader = "DataControlField_ShowHeader";

		// Token: 0x04000611 RID: 1553
		internal const string DataControlField_SortExpression = "DataControlField_SortExpression";

		// Token: 0x04000612 RID: 1554
		internal const string DataControlField_Visible = "DataControlField_Visible";

		// Token: 0x04000613 RID: 1555
		internal const string DataGrid_AllowCustomPaging = "DataGrid_AllowCustomPaging";

		// Token: 0x04000614 RID: 1556
		internal const string DataGrid_AllowPaging = "DataGrid_AllowPaging";

		// Token: 0x04000615 RID: 1557
		internal const string DataGrid_AllowSorting = "DataGrid_AllowSorting";

		// Token: 0x04000616 RID: 1558
		internal const string DataGrid_AlternatingItemStyle = "DataGrid_AlternatingItemStyle";

		// Token: 0x04000617 RID: 1559
		internal const string DataGrid_CurrentPageIndex = "DataGrid_CurrentPageIndex";

		// Token: 0x04000618 RID: 1560
		internal const string DataGrid_EditItemIndex = "DataGrid_EditItemIndex";

		// Token: 0x04000619 RID: 1561
		internal const string DataGrid_EditItemStyle = "DataGrid_EditItemStyle";

		// Token: 0x0400061A RID: 1562
		internal const string DataGrid_ItemStyle = "DataGrid_ItemStyle";

		// Token: 0x0400061B RID: 1563
		internal const string DataGrid_Items = "DataGrid_Items";

		// Token: 0x0400061C RID: 1564
		internal const string DataGrid_PageCount = "DataGrid_PageCount";

		// Token: 0x0400061D RID: 1565
		internal const string DataGrid_PagerStyle = "DataGrid_PagerStyle";

		// Token: 0x0400061E RID: 1566
		internal const string DataGrid_PageSize = "DataGrid_PageSize";

		// Token: 0x0400061F RID: 1567
		internal const string DataGrid_SelectedItem = "DataGrid_SelectedItem";

		// Token: 0x04000620 RID: 1568
		internal const string DataGrid_SelectedItemStyle = "DataGrid_SelectedItemStyle";

		// Token: 0x04000621 RID: 1569
		internal const string DataGrid_OnCancelCommand = "DataGrid_OnCancelCommand";

		// Token: 0x04000622 RID: 1570
		internal const string DataGrid_OnDeleteCommand = "DataGrid_OnDeleteCommand";

		// Token: 0x04000623 RID: 1571
		internal const string DataGrid_OnEditCommand = "DataGrid_OnEditCommand";

		// Token: 0x04000624 RID: 1572
		internal const string DataGrid_OnItemCommand = "DataGrid_OnItemCommand";

		// Token: 0x04000625 RID: 1573
		internal const string DataGrid_OnPageIndexChanged = "DataGrid_OnPageIndexChanged";

		// Token: 0x04000626 RID: 1574
		internal const string DataGrid_OnSortCommand = "DataGrid_OnSortCommand";

		// Token: 0x04000627 RID: 1575
		internal const string DataGrid_OnUpdateCommand = "DataGrid_OnUpdateCommand";

		// Token: 0x04000628 RID: 1576
		internal const string DataGrid_VisibleItemCount = "DataGrid_VisibleItemCount";

		// Token: 0x04000629 RID: 1577
		internal const string DataGridColumn_FooterStyle = "DataGridColumn_FooterStyle";

		// Token: 0x0400062A RID: 1578
		internal const string DataGridColumn_FooterText = "DataGridColumn_FooterText";

		// Token: 0x0400062B RID: 1579
		internal const string DataGridColumn_HeaderImageUrl = "DataGridColumn_HeaderImageUrl";

		// Token: 0x0400062C RID: 1580
		internal const string DataGridColumn_HeaderStyle = "DataGridColumn_HeaderStyle";

		// Token: 0x0400062D RID: 1581
		internal const string DataGridColumn_HeaderText = "DataGridColumn_HeaderText";

		// Token: 0x0400062E RID: 1582
		internal const string DataGridColumn_ItemStyle = "DataGridColumn_ItemStyle";

		// Token: 0x0400062F RID: 1583
		internal const string DataGridColumn_SortExpression = "DataGridColumn_SortExpression";

		// Token: 0x04000630 RID: 1584
		internal const string DataGridColumn_Visible = "DataGridColumn_Visible";

		// Token: 0x04000631 RID: 1585
		internal const string DataGridPagerStyle_Mode = "DataGridPagerStyle_Mode";

		// Token: 0x04000632 RID: 1586
		internal const string DataGridPagerStyle_PageButtonCount = "DataGridPagerStyle_PageButtonCount";

		// Token: 0x04000633 RID: 1587
		internal const string DataGridPagerStyle_Position = "DataGridPagerStyle_Position";

		// Token: 0x04000634 RID: 1588
		internal const string DataGridPagerStyle_Visible = "DataGridPagerStyle_Visible";

		// Token: 0x04000635 RID: 1589
		internal const string DataList_AlternatingItemStyle = "DataList_AlternatingItemStyle";

		// Token: 0x04000636 RID: 1590
		internal const string DataList_AlternatingItemTemplate = "DataList_AlternatingItemTemplate";

		// Token: 0x04000637 RID: 1591
		internal const string DataList_EditItemIndex = "DataList_EditItemIndex";

		// Token: 0x04000638 RID: 1592
		internal const string DataList_EditItemStyle = "DataList_EditItemStyle";

		// Token: 0x04000639 RID: 1593
		internal const string DataList_EditItemTemplate = "DataList_EditItemTemplate";

		// Token: 0x0400063A RID: 1594
		internal const string DataList_ExtractTemplateRows = "DataList_ExtractTemplateRows";

		// Token: 0x0400063B RID: 1595
		internal const string DataList_FooterTemplate = "DataList_FooterTemplate";

		// Token: 0x0400063C RID: 1596
		internal const string DataList_HeaderTemplate = "DataList_HeaderTemplate";

		// Token: 0x0400063D RID: 1597
		internal const string DataList_ItemStyle = "DataList_ItemStyle";

		// Token: 0x0400063E RID: 1598
		internal const string DataList_Items = "DataList_Items";

		// Token: 0x0400063F RID: 1599
		internal const string DataList_ItemTemplate = "DataList_ItemTemplate";

		// Token: 0x04000640 RID: 1600
		internal const string DataList_RepeatColumns = "DataList_RepeatColumns";

		// Token: 0x04000641 RID: 1601
		internal const string DataList_SelectedItemStyle = "DataList_SelectedItemStyle";

		// Token: 0x04000642 RID: 1602
		internal const string DataList_SelectedItem = "DataList_SelectedItem";

		// Token: 0x04000643 RID: 1603
		internal const string DataList_SelectedItemTemplate = "DataList_SelectedItemTemplate";

		// Token: 0x04000644 RID: 1604
		internal const string DataList_SeparatorStyle = "DataList_SeparatorStyle";

		// Token: 0x04000645 RID: 1605
		internal const string DataList_SeparatorTemplate = "DataList_SeparatorTemplate";

		// Token: 0x04000646 RID: 1606
		internal const string DataList_OnCancelCommand = "DataList_OnCancelCommand";

		// Token: 0x04000647 RID: 1607
		internal const string DataList_OnDeleteCommand = "DataList_OnDeleteCommand";

		// Token: 0x04000648 RID: 1608
		internal const string DataList_OnEditCommand = "DataList_OnEditCommand";

		// Token: 0x04000649 RID: 1609
		internal const string DataList_OnItemCommand = "DataList_OnItemCommand";

		// Token: 0x0400064A RID: 1610
		internal const string DataList_OnUpdateCommand = "DataList_OnUpdateCommand";

		// Token: 0x0400064B RID: 1611
		internal const string DetailsView_AllowPaging = "DetailsView_AllowPaging";

		// Token: 0x0400064C RID: 1612
		internal const string DetailsView_AlternatingRowStyle = "DetailsView_AlternatingRowStyle";

		// Token: 0x0400064D RID: 1613
		internal const string DetailsView_AutoGenerateDeleteButton = "DetailsView_AutoGenerateDeleteButton";

		// Token: 0x0400064E RID: 1614
		internal const string DetailsView_AutoGenerateEditButton = "DetailsView_AutoGenerateEditButton";

		// Token: 0x0400064F RID: 1615
		internal const string DetailsView_AutoGenerateInsertButton = "DetailsView_AutoGenerateInsertButton";

		// Token: 0x04000650 RID: 1616
		internal const string DetailsView_AutoGenerateRows = "DetailsView_AutoGenerateRows";

		// Token: 0x04000651 RID: 1617
		internal const string DetailsView_CellPadding = "DetailsView_CellPadding";

		// Token: 0x04000652 RID: 1618
		internal const string DetailsView_CellSpacing = "DetailsView_CellSpacing";

		// Token: 0x04000653 RID: 1619
		internal const string DetailsView_CommandRowStyle = "DetailsView_CommandRowStyle";

		// Token: 0x04000654 RID: 1620
		internal const string DetailsView_DataKey = "DetailsView_DataKey";

		// Token: 0x04000655 RID: 1621
		internal const string DetailsView_PageIndex = "DetailsView_PageIndex";

		// Token: 0x04000656 RID: 1622
		internal const string DetailsView_EnablePagingCallbacks = "DetailsView_EnablePagingCallbacks";

		// Token: 0x04000657 RID: 1623
		internal const string DetailsView_FooterStyle = "DetailsView_FooterStyle";

		// Token: 0x04000658 RID: 1624
		internal const string DetailsView_FooterTemplate = "DetailsView_FooterTemplate";

		// Token: 0x04000659 RID: 1625
		internal const string DetailsView_FieldHeaderStyle = "DetailsView_FieldHeaderStyle";

		// Token: 0x0400065A RID: 1626
		internal const string DetailsView_OnPageIndexChanged = "DetailsView_OnPageIndexChanged";

		// Token: 0x0400065B RID: 1627
		internal const string DetailsView_OnPageIndexChanging = "DetailsView_OnPageIndexChanging";

		// Token: 0x0400065C RID: 1628
		internal const string DetailsView_OnItemCommand = "DetailsView_OnItemCommand";

		// Token: 0x0400065D RID: 1629
		internal const string DetailsView_OnItemCreated = "DetailsView_OnItemCreated";

		// Token: 0x0400065E RID: 1630
		internal const string DetailsView_OnModeChanged = "DetailsView_OnModeChanged";

		// Token: 0x0400065F RID: 1631
		internal const string DetailsView_OnModeChanging = "DetailsView_OnModeChanging";

		// Token: 0x04000660 RID: 1632
		internal const string DetailsView_PagerSettings = "DetailsView_PagerSettings";

		// Token: 0x04000661 RID: 1633
		internal const string DetailsView_Fields = "DetailsView_Fields";

		// Token: 0x04000662 RID: 1634
		internal const string DetailsView_Rows = "DetailsView_Rows";

		// Token: 0x04000663 RID: 1635
		internal const string FontInfo_Name = "FontInfo_Name";

		// Token: 0x04000664 RID: 1636
		internal const string FontInfo_Names = "FontInfo_Names";

		// Token: 0x04000665 RID: 1637
		internal const string FontInfo_Size = "FontInfo_Size";

		// Token: 0x04000666 RID: 1638
		internal const string FontInfo_Bold = "FontInfo_Bold";

		// Token: 0x04000667 RID: 1639
		internal const string FontInfo_Italic = "FontInfo_Italic";

		// Token: 0x04000668 RID: 1640
		internal const string FontInfo_Underline = "FontInfo_Underline";

		// Token: 0x04000669 RID: 1641
		internal const string FontInfo_Strikeout = "FontInfo_Strikeout";

		// Token: 0x0400066A RID: 1642
		internal const string FontInfo_Overline = "FontInfo_Overline";

		// Token: 0x0400066B RID: 1643
		internal const string FormView_AllowPaging = "FormView_AllowPaging";

		// Token: 0x0400066C RID: 1644
		internal const string FormView_CellPadding = "FormView_CellPadding";

		// Token: 0x0400066D RID: 1645
		internal const string FormView_CellSpacing = "FormView_CellSpacing";

		// Token: 0x0400066E RID: 1646
		internal const string FormView_DataKey = "FormView_DataKey";

		// Token: 0x0400066F RID: 1647
		internal const string FormView_PageIndex = "FormView_PageIndex";

		// Token: 0x04000670 RID: 1648
		internal const string FormView_EditItemTemplate = "FormView_EditItemTemplate";

		// Token: 0x04000671 RID: 1649
		internal const string FormView_FooterStyle = "FormView_FooterStyle";

		// Token: 0x04000672 RID: 1650
		internal const string FormView_FooterTemplate = "FormView_FooterTemplate";

		// Token: 0x04000673 RID: 1651
		internal const string FormView_InsertItemTemplate = "FormView_InsertItemTemplate";

		// Token: 0x04000674 RID: 1652
		internal const string FormView_OnPageIndexChanged = "FormView_OnPageIndexChanged";

		// Token: 0x04000675 RID: 1653
		internal const string FormView_OnPageIndexChanging = "FormView_OnPageIndexChanging";

		// Token: 0x04000676 RID: 1654
		internal const string FormView_OnItemCommand = "FormView_OnItemCommand";

		// Token: 0x04000677 RID: 1655
		internal const string FormView_OnItemCreated = "FormView_OnItemCreated";

		// Token: 0x04000678 RID: 1656
		internal const string FormView_OnModeChanged = "FormView_OnModeChanged";

		// Token: 0x04000679 RID: 1657
		internal const string FormView_OnModeChanging = "FormView_OnModeChanging";

		// Token: 0x0400067A RID: 1658
		internal const string FormView_Rows = "FormView_Rows";

		// Token: 0x0400067B RID: 1659
		internal const string HiddenField_OnValueChanged = "HiddenField_OnValueChanged";

		// Token: 0x0400067C RID: 1660
		internal const string HiddenField_Value = "HiddenField_Value";

		// Token: 0x0400067D RID: 1661
		internal const string HotSpot_AccessKey = "HotSpot_AccessKey";

		// Token: 0x0400067E RID: 1662
		internal const string HotSpot_AlternateText = "HotSpot_AlternateText";

		// Token: 0x0400067F RID: 1663
		internal const string HotSpot_PostBackValue = "HotSpot_PostBackValue";

		// Token: 0x04000680 RID: 1664
		internal const string HotSpot_NavigateUrl = "HotSpot_NavigateUrl";

		// Token: 0x04000681 RID: 1665
		internal const string HotSpot_TabIndex = "HotSpot_TabIndex";

		// Token: 0x04000682 RID: 1666
		internal const string HyperLink_ImageUrl = "HyperLink_ImageUrl";

		// Token: 0x04000683 RID: 1667
		internal const string HyperLink_NavigateUrl = "HyperLink_NavigateUrl";

		// Token: 0x04000684 RID: 1668
		internal const string HyperLink_Target = "HyperLink_Target";

		// Token: 0x04000685 RID: 1669
		internal const string HyperLink_Text = "HyperLink_Text";

		// Token: 0x04000686 RID: 1670
		internal const string HyperLinkColumn_DataNavigateUrlField = "HyperLinkColumn_DataNavigateUrlField";

		// Token: 0x04000687 RID: 1671
		internal const string HyperLinkColumn_DataTextField = "HyperLinkColumn_DataTextField";

		// Token: 0x04000688 RID: 1672
		internal const string HyperLinkColumn_NavigateUrl = "HyperLinkColumn_NavigateUrl";

		// Token: 0x04000689 RID: 1673
		internal const string HyperLinkColumn_Text = "HyperLinkColumn_Text";

		// Token: 0x0400068A RID: 1674
		internal const string HyperLinkField_DataNavigateUrlFields = "HyperLinkField_DataNavigateUrlFields";

		// Token: 0x0400068B RID: 1675
		internal const string HyperLinkField_DataNavigateUrlFormatString = "HyperLinkField_DataNavigateUrlFormatString";

		// Token: 0x0400068C RID: 1676
		internal const string HyperLinkField_DataTextField = "HyperLinkField_DataTextField";

		// Token: 0x0400068D RID: 1677
		internal const string HyperLinkField_DataTextFormatString = "HyperLinkField_DataTextFormatString";

		// Token: 0x0400068E RID: 1678
		internal const string HyperLinkField_NavigateUrl = "HyperLinkField_NavigateUrl";

		// Token: 0x0400068F RID: 1679
		internal const string HyperLinkField_Text = "HyperLinkField_Text";

		// Token: 0x04000690 RID: 1680
		internal const string Image_AlternateText = "Image_AlternateText";

		// Token: 0x04000691 RID: 1681
		internal const string Image_DescriptionUrl = "Image_DescriptionUrl";

		// Token: 0x04000692 RID: 1682
		internal const string Image_GenerateEmptyAlternateText = "Image_GenerateEmptyAlternateText";

		// Token: 0x04000693 RID: 1683
		internal const string Image_ImageAlign = "Image_ImageAlign";

		// Token: 0x04000694 RID: 1684
		internal const string Image_ImageUrl = "Image_ImageUrl";

		// Token: 0x04000695 RID: 1685
		internal const string ImageButton_OnClick = "ImageButton_OnClick";

		// Token: 0x04000696 RID: 1686
		internal const string ImageButton_OnCommand = "ImageButton_OnCommand";

		// Token: 0x04000697 RID: 1687
		internal const string ImageField_AlternateText = "ImageField_AlternateText";

		// Token: 0x04000698 RID: 1688
		internal const string ImageField_DataAlternateTextField = "ImageField_DataAlternateTextField";

		// Token: 0x04000699 RID: 1689
		internal const string ImageField_DataAlternateTextFormatString = "ImageField_DataAlternateTextFormatString";

		// Token: 0x0400069A RID: 1690
		internal const string ImageField_ConvertEmptyStringToNull = "ImageField_ConvertEmptyStringToNull";

		// Token: 0x0400069B RID: 1691
		internal const string ImageField_ImageUrlField = "ImageField_ImageUrlField";

		// Token: 0x0400069C RID: 1692
		internal const string ImageField_ImageUrlFormatString = "ImageField_ImageUrlFormatString";

		// Token: 0x0400069D RID: 1693
		internal const string ImageField_NullImageUrl = "ImageField_NullImageUrl";

		// Token: 0x0400069E RID: 1694
		internal const string ImageField_ReadOnly = "ImageField_ReadOnly";

		// Token: 0x0400069F RID: 1695
		internal const string ImageMap_Click = "ImageMap_Click";

		// Token: 0x040006A0 RID: 1696
		internal const string ImageMap_HotSpots = "ImageMap_HotSpots";

		// Token: 0x040006A1 RID: 1697
		internal const string Label_AssociatedControlID = "Label_AssociatedControlID";

		// Token: 0x040006A2 RID: 1698
		internal const string Label_Text = "Label_Text";

		// Token: 0x040006A3 RID: 1699
		internal const string Literal_Text = "Literal_Text";

		// Token: 0x040006A4 RID: 1700
		internal const string Literal_Mode = "Literal_Mode";

		// Token: 0x040006A5 RID: 1701
		internal const string LinkButton_Text = "LinkButton_Text";

		// Token: 0x040006A6 RID: 1702
		internal const string LinkButton_OnClick = "LinkButton_OnClick";

		// Token: 0x040006A7 RID: 1703
		internal const string ListBox_Rows = "ListBox_Rows";

		// Token: 0x040006A8 RID: 1704
		internal const string ListBox_SelectionMode = "ListBox_SelectionMode";

		// Token: 0x040006A9 RID: 1705
		internal const string ListControl_AppendDataBoundItems = "ListControl_AppendDataBoundItems";

		// Token: 0x040006AA RID: 1706
		internal const string ListControl_AutoPostBack = "ListControl_AutoPostBack";

		// Token: 0x040006AB RID: 1707
		internal const string ListControl_DataTextField = "ListControl_DataTextField";

		// Token: 0x040006AC RID: 1708
		internal const string ListControl_DataTextFormatString = "ListControl_DataTextFormatString";

		// Token: 0x040006AD RID: 1709
		internal const string ListControl_DataValueField = "ListControl_DataValueField";

		// Token: 0x040006AE RID: 1710
		internal const string ListControl_Items = "ListControl_Items";

		// Token: 0x040006AF RID: 1711
		internal const string ListControl_SelectedItem = "ListControl_SelectedItem";

		// Token: 0x040006B0 RID: 1712
		internal const string ListControl_SelectedValue = "ListControl_SelectedValue";

		// Token: 0x040006B1 RID: 1713
		internal const string ListControl_OnSelectedIndexChanged = "ListControl_OnSelectedIndexChanged";

		// Token: 0x040006B2 RID: 1714
		internal const string ListControl_Text = "ListControl_Text";

		// Token: 0x040006B3 RID: 1715
		internal const string ListControl_TextChanged = "ListControl_TextChanged";

		// Token: 0x040006B4 RID: 1716
		internal const string Login_LoggedIn = "Login_LoggedIn";

		// Token: 0x040006B5 RID: 1717
		internal const string Login_Authenticate = "Login_Authenticate";

		// Token: 0x040006B6 RID: 1718
		internal const string Login_LoggingIn = "Login_LoggingIn";

		// Token: 0x040006B7 RID: 1719
		internal const string Login_CheckBoxStyle = "Login_CheckBoxStyle";

		// Token: 0x040006B8 RID: 1720
		internal const string Login_CreateUserUrl = "Login_CreateUserUrl";

		// Token: 0x040006B9 RID: 1721
		internal const string Login_CreateUserIconUrl = "Login_CreateUserIconUrl";

		// Token: 0x040006BA RID: 1722
		internal const string Login_DefaultFailureText = "Login_DefaultFailureText";

		// Token: 0x040006BB RID: 1723
		internal const string LoginControls_DefaultPasswordLabelText = "LoginControls_DefaultPasswordLabelText";

		// Token: 0x040006BC RID: 1724
		internal const string Login_DefaultPasswordRequiredErrorMessage = "Login_DefaultPasswordRequiredErrorMessage";

		// Token: 0x040006BD RID: 1725
		internal const string Login_DefaultRememberMeText = "Login_DefaultRememberMeText";

		// Token: 0x040006BE RID: 1726
		internal const string Login_DefaultLoginButtonText = "Login_DefaultLoginButtonText";

		// Token: 0x040006BF RID: 1727
		internal const string Login_DefaultTitleText = "Login_DefaultTitleText";

		// Token: 0x040006C0 RID: 1728
		internal const string Login_DefaultUserNameLabelText = "Login_DefaultUserNameLabelText";

		// Token: 0x040006C1 RID: 1729
		internal const string Login_DefaultUserNameRequiredErrorMessage = "Login_DefaultUserNameRequiredErrorMessage";

		// Token: 0x040006C2 RID: 1730
		internal const string Login_DestinationPageUrl = "Login_DestinationPageUrl";

		// Token: 0x040006C3 RID: 1731
		internal const string Login_DisplayRememberMe = "Login_DisplayRememberMe";

		// Token: 0x040006C4 RID: 1732
		internal const string Login_HelpPageIconUrl = "Login_HelpPageIconUrl";

		// Token: 0x040006C5 RID: 1733
		internal const string Login_InvalidBorderPadding = "Login_InvalidBorderPadding";

		// Token: 0x040006C6 RID: 1734
		internal const string Login_LoginError = "Login_LoginError";

		// Token: 0x040006C7 RID: 1735
		internal const string Login_FailureAction = "Login_FailureAction";

		// Token: 0x040006C8 RID: 1736
		internal const string Login_FailureText = "Login_FailureText";

		// Token: 0x040006C9 RID: 1737
		internal const string Login_Orientation = "Login_Orientation";

		// Token: 0x040006CA RID: 1738
		internal const string Login_NoPasswordTextBox = "Login_NoPasswordTextBox";

		// Token: 0x040006CB RID: 1739
		internal const string Login_NoUserNameTextBox = "Login_NoUserNameTextBox";

		// Token: 0x040006CC RID: 1740
		internal const string LoginControls_PasswordLabelText = "LoginControls_PasswordLabelText";

		// Token: 0x040006CD RID: 1741
		internal const string Login_PasswordRecoveryUrl = "Login_PasswordRecoveryUrl";

		// Token: 0x040006CE RID: 1742
		internal const string Login_PasswordRecoveryIconUrl = "Login_PasswordRecoveryIconUrl";

		// Token: 0x040006CF RID: 1743
		internal const string Login_PasswordRequiredErrorMessage = "Login_PasswordRequiredErrorMessage";

		// Token: 0x040006D0 RID: 1744
		internal const string Login_RememberMeSet = "Login_RememberMeSet";

		// Token: 0x040006D1 RID: 1745
		internal const string Login_RememberMeText = "Login_RememberMeText";

		// Token: 0x040006D2 RID: 1746
		internal const string Login_LoginButtonImageUrl = "Login_LoginButtonImageUrl";

		// Token: 0x040006D3 RID: 1747
		internal const string Login_LoginButtonStyle = "Login_LoginButtonStyle";

		// Token: 0x040006D4 RID: 1748
		internal const string Login_LoginButtonType = "Login_LoginButtonType";

		// Token: 0x040006D5 RID: 1749
		internal const string Login_LoginButtonText = "Login_LoginButtonText";

		// Token: 0x040006D6 RID: 1750
		internal const string Login_BorderPadding = "Login_BorderPadding";

		// Token: 0x040006D7 RID: 1751
		internal const string Login_ValidatorTextStyle = "Login_ValidatorTextStyle";

		// Token: 0x040006D8 RID: 1752
		internal const string Login_VisibleWhenLoggedIn = "Login_VisibleWhenLoggedIn";

		// Token: 0x040006D9 RID: 1753
		internal const string LoginName_InvalidFormatString = "LoginName_InvalidFormatString";

		// Token: 0x040006DA RID: 1754
		internal const string LoginName_FormatString = "LoginName_FormatString";

		// Token: 0x040006DB RID: 1755
		internal const string LoginName_DesignModeUserName = "LoginName_DesignModeUserName";

		// Token: 0x040006DC RID: 1756
		internal const string LoginStatus_LoginImageUrl = "LoginStatus_LoginImageUrl";

		// Token: 0x040006DD RID: 1757
		internal const string LoginStatus_LoginText = "LoginStatus_LoginText";

		// Token: 0x040006DE RID: 1758
		internal const string LoginStatus_LogoutAction = "LoginStatus_LogoutAction";

		// Token: 0x040006DF RID: 1759
		internal const string LoginStatus_LogoutImageUrl = "LoginStatus_LogoutImageUrl";

		// Token: 0x040006E0 RID: 1760
		internal const string LoginStatus_LogoutPageUrl = "LoginStatus_LogoutPageUrl";

		// Token: 0x040006E1 RID: 1761
		internal const string LoginStatus_LogoutText = "LoginStatus_LogoutText";

		// Token: 0x040006E2 RID: 1762
		internal const string LoginStatus_LoggedOut = "LoginStatus_LoggedOut";

		// Token: 0x040006E3 RID: 1763
		internal const string LoginStatus_LoggingOut = "LoginStatus_LoggingOut";

		// Token: 0x040006E4 RID: 1764
		internal const string LoginStatus_DefaultLoginText = "LoginStatus_DefaultLoginText";

		// Token: 0x040006E5 RID: 1765
		internal const string LoginStatus_DefaultLogoutText = "LoginStatus_DefaultLogoutText";

		// Token: 0x040006E6 RID: 1766
		internal const string LoginView_RoleGroups = "LoginView_RoleGroups";

		// Token: 0x040006E7 RID: 1767
		internal const string LoginView_ViewChanged = "LoginView_ViewChanged";

		// Token: 0x040006E8 RID: 1768
		internal const string LoginView_ViewChanging = "LoginView_ViewChanging";

		// Token: 0x040006E9 RID: 1769
		internal const string EmbeddedMailObject_Name = "EmbeddedMailObject_Name";

		// Token: 0x040006EA RID: 1770
		internal const string EmbeddedMailObject_Path = "EmbeddedMailObject_Path";

		// Token: 0x040006EB RID: 1771
		internal const string MailDefinition_EmbeddedObjects = "MailDefinition_EmbeddedObjects";

		// Token: 0x040006EC RID: 1772
		internal const string MailDefinition_BodyFileName = "MailDefinition_BodyFileName";

		// Token: 0x040006ED RID: 1773
		internal const string MailDefinition_CC = "MailDefinition_CC";

		// Token: 0x040006EE RID: 1774
		internal const string MailDefinition_From = "MailDefinition_From";

		// Token: 0x040006EF RID: 1775
		internal const string MailDefinition_InvalidReplacements = "MailDefinition_InvalidReplacements";

		// Token: 0x040006F0 RID: 1776
		internal const string MailDefinition_IsBodyHtml = "MailDefinition_IsBodyHtml";

		// Token: 0x040006F1 RID: 1777
		internal const string MailDefinition_NoFromAddressSpecified = "MailDefinition_NoFromAddressSpecified";

		// Token: 0x040006F2 RID: 1778
		internal const string MailDefinition_Priority = "MailDefinition_Priority";

		// Token: 0x040006F3 RID: 1779
		internal const string MailDefinition_Subject = "MailDefinition_Subject";

		// Token: 0x040006F4 RID: 1780
		internal const string MenuItemStyle_HorizontalPadding = "MenuItemStyle_HorizontalPadding";

		// Token: 0x040006F5 RID: 1781
		internal const string MenuItemStyle_ItemSpacing = "MenuItemStyle_ItemSpacing";

		// Token: 0x040006F6 RID: 1782
		internal const string MenuItemStyle_VerticalPadding = "MenuItemStyle_VerticalPadding";

		// Token: 0x040006F7 RID: 1783
		internal const string MenuItemStyleCollection_InvalidArgument = "MenuItemStyleCollection_InvalidArgument";

		// Token: 0x040006F8 RID: 1784
		internal const string MenuItemBinding_Depth = "MenuItemBinding_Depth";

		// Token: 0x040006F9 RID: 1785
		internal const string MenuItemBinding_Enabled = "MenuItemBinding_Enabled";

		// Token: 0x040006FA RID: 1786
		internal const string MenuItemBinding_EnabledField = "MenuItemBinding_EnabledField";

		// Token: 0x040006FB RID: 1787
		internal const string MenuItemBinding_FormatString = "MenuItemBinding_FormatString";

		// Token: 0x040006FC RID: 1788
		internal const string MenuItemBinding_ImageUrl = "MenuItemBinding_ImageUrl";

		// Token: 0x040006FD RID: 1789
		internal const string MenuItemBinding_ImageUrlField = "MenuItemBinding_ImageUrlField";

		// Token: 0x040006FE RID: 1790
		internal const string MenuItemBinding_NavigateUrl = "MenuItemBinding_NavigateUrl";

		// Token: 0x040006FF RID: 1791
		internal const string MenuItemBinding_NavigateUrlField = "MenuItemBinding_NavigateUrlField";

		// Token: 0x04000700 RID: 1792
		internal const string MenuItemBinding_PopOutImageUrl = "MenuItemBinding_PopOutImageUrl";

		// Token: 0x04000701 RID: 1793
		internal const string MenuItemBinding_PopOutImageUrlField = "MenuItemBinding_PopOutImageUrlField";

		// Token: 0x04000702 RID: 1794
		internal const string MenuItemBinding_Selectable = "MenuItemBinding_Selectable";

		// Token: 0x04000703 RID: 1795
		internal const string MenuItemBinding_SelectableField = "MenuItemBinding_SelectableField";

		// Token: 0x04000704 RID: 1796
		internal const string MenuItemBinding_SeparatorImageUrl = "MenuItemBinding_SeparatorImageUrl";

		// Token: 0x04000705 RID: 1797
		internal const string MenuItemBinding_SeparatorImageUrlField = "MenuItemBinding_SeparatorImageUrlField";

		// Token: 0x04000706 RID: 1798
		internal const string MenuItemBinding_Target = "MenuItemBinding_Target";

		// Token: 0x04000707 RID: 1799
		internal const string MenuItemBinding_TargetField = "MenuItemBinding_TargetField";

		// Token: 0x04000708 RID: 1800
		internal const string MenuItemBinding_Text = "MenuItemBinding_Text";

		// Token: 0x04000709 RID: 1801
		internal const string MenuItemBinding_TextField = "MenuItemBinding_TextField";

		// Token: 0x0400070A RID: 1802
		internal const string MenuItemBinding_ToolTip = "MenuItemBinding_ToolTip";

		// Token: 0x0400070B RID: 1803
		internal const string MenuItemBinding_ToolTipField = "MenuItemBinding_ToolTipField";

		// Token: 0x0400070C RID: 1804
		internal const string MenuItemBinding_Value = "MenuItemBinding_Value";

		// Token: 0x0400070D RID: 1805
		internal const string MenuItemBinding_ValueField = "MenuItemBinding_ValueField";

		// Token: 0x0400070E RID: 1806
		internal const string MenuItem_Enabled = "MenuItem_Enabled";

		// Token: 0x0400070F RID: 1807
		internal const string MenuItem_ImageUrl = "MenuItem_ImageUrl";

		// Token: 0x04000710 RID: 1808
		internal const string MenuItem_NavigateUrl = "MenuItem_NavigateUrl";

		// Token: 0x04000711 RID: 1809
		internal const string MenuItem_PopOutImageUrl = "MenuItem_PopOutImageUrl";

		// Token: 0x04000712 RID: 1810
		internal const string MenuItem_Selectable = "MenuItem_Selectable";

		// Token: 0x04000713 RID: 1811
		internal const string MenuItem_Selected = "MenuItem_Selected";

		// Token: 0x04000714 RID: 1812
		internal const string MenuItem_SeparatorImageUrl = "MenuItem_SeparatorImageUrl";

		// Token: 0x04000715 RID: 1813
		internal const string MenuItem_Target = "MenuItem_Target";

		// Token: 0x04000716 RID: 1814
		internal const string MenuItem_Text = "MenuItem_Text";

		// Token: 0x04000717 RID: 1815
		internal const string MenuItem_ToolTip = "MenuItem_ToolTip";

		// Token: 0x04000718 RID: 1816
		internal const string MenuItem_Value = "MenuItem_Value";

		// Token: 0x04000719 RID: 1817
		internal const string MenuItemCollection_InvalidArrayType = "MenuItemCollection_InvalidArrayType";

		// Token: 0x0400071A RID: 1818
		internal const string Menu_Bindings = "Menu_Bindings";

		// Token: 0x0400071B RID: 1819
		internal const string Menu_DataSourceReturnedNullView = "Menu_DataSourceReturnedNullView";

		// Token: 0x0400071C RID: 1820
		internal const string Menu_DesignTimeDummyItemText = "Menu_DesignTimeDummyItemText";

		// Token: 0x0400071D RID: 1821
		internal const string Menu_DisappearAfter = "Menu_DisappearAfter";

		// Token: 0x0400071E RID: 1822
		internal const string Menu_DynamicBottomSeparatorImageUrl = "Menu_DynamicBottomSeparatorImageUrl";

		// Token: 0x0400071F RID: 1823
		internal const string Menu_DynamicDisplayPopOutImage = "Menu_DynamicDisplayPopOutImage";

		// Token: 0x04000720 RID: 1824
		internal const string Menu_DynamicHorizontalOffset = "Menu_DynamicHorizontalOffset";

		// Token: 0x04000721 RID: 1825
		internal const string Menu_DynamicHoverStyle = "Menu_DynamicHoverStyle";

		// Token: 0x04000722 RID: 1826
		internal const string Menu_DynamicItemFormatString = "Menu_DynamicItemFormatString";

		// Token: 0x04000723 RID: 1827
		internal const string Menu_DynamicMenuItemStyle = "Menu_DynamicMenuItemStyle";

		// Token: 0x04000724 RID: 1828
		internal const string Menu_DynamicMenuStyle = "Menu_DynamicMenuStyle";

		// Token: 0x04000725 RID: 1829
		internal const string Menu_DynamicPopoutImageUrl = "Menu_DynamicPopoutImageUrl";

		// Token: 0x04000726 RID: 1830
		internal const string Menu_DynamicPopoutImageText = "Menu_DynamicPopoutImageText";

		// Token: 0x04000727 RID: 1831
		internal const string Menu_DynamicSelectedStyle = "Menu_DynamicSelectedStyle";

		// Token: 0x04000728 RID: 1832
		internal const string Menu_DynamicTemplate = "Menu_DynamicTemplate";

		// Token: 0x04000729 RID: 1833
		internal const string Menu_DynamicTopSeparatorImageUrl = "Menu_DynamicTopSeparatorImageUrl";

		// Token: 0x0400072A RID: 1834
		internal const string Menu_DynamicVerticalOffset = "Menu_DynamicVerticalOffset";

		// Token: 0x0400072B RID: 1835
		internal const string Menu_InvalidDataBinding = "Menu_InvalidDataBinding";

		// Token: 0x0400072C RID: 1836
		internal const string Menu_InvalidDepth = "Menu_InvalidDepth";

		// Token: 0x0400072D RID: 1837
		internal const string Menu_InvalidNavigation = "Menu_InvalidNavigation";

		// Token: 0x0400072E RID: 1838
		internal const string Menu_InvalidSelection = "Menu_InvalidSelection";

		// Token: 0x0400072F RID: 1839
		internal const string Menu_Items = "Menu_Items";

		// Token: 0x04000730 RID: 1840
		internal const string Menu_ItemWrap = "Menu_ItemWrap";

		// Token: 0x04000731 RID: 1841
		internal const string Menu_LevelMenuItemStyles = "Menu_LevelMenuItemStyles";

		// Token: 0x04000732 RID: 1842
		internal const string Menu_LevelSelectedStyles = "Menu_LevelSelectedStyles";

		// Token: 0x04000733 RID: 1843
		internal const string Menu_LevelSubMenuStyles = "Menu_LevelSubMenuStyles";

		// Token: 0x04000734 RID: 1844
		internal const string Menu_MaximumDynamicDisplayLevels = "Menu_MaximumDynamicDisplayLevels";

		// Token: 0x04000735 RID: 1845
		internal const string Menu_MaximumDynamicDisplayLevelsInvalid = "Menu_MaximumDynamicDisplayLevelsInvalid";

		// Token: 0x04000736 RID: 1846
		internal const string Menu_MenuItemClick = "Menu_MenuItemClick";

		// Token: 0x04000737 RID: 1847
		internal const string Menu_MenuItemDataBound = "Menu_MenuItemDataBound";

		// Token: 0x04000738 RID: 1848
		internal const string Menu_Orientation = "Menu_Orientation";

		// Token: 0x04000739 RID: 1849
		internal const string Menu_PathSeparator = "Menu_PathSeparator";

		// Token: 0x0400073A RID: 1850
		internal const string Menu_ScrollDown = "Menu_ScrollDown";

		// Token: 0x0400073B RID: 1851
		internal const string Menu_ScrollDownImageUrl = "Menu_ScrollDownImageUrl";

		// Token: 0x0400073C RID: 1852
		internal const string Menu_ScrollDownText = "Menu_ScrollDownText";

		// Token: 0x0400073D RID: 1853
		internal const string Menu_ScrollUpImageUrl = "Menu_ScrollUpImageUrl";

		// Token: 0x0400073E RID: 1854
		internal const string Menu_SkipLinkTextDefault = "Menu_SkipLinkTextDefault";

		// Token: 0x0400073F RID: 1855
		internal const string Menu_ScrollUp = "Menu_ScrollUp";

		// Token: 0x04000740 RID: 1856
		internal const string Menu_ScrollUpText = "Menu_ScrollUpText";

		// Token: 0x04000741 RID: 1857
		internal const string Menu_StaticBottomSeparatorImageUrl = "Menu_StaticBottomSeparatorImageUrl";

		// Token: 0x04000742 RID: 1858
		internal const string Menu_StaticDisplayLevels = "Menu_StaticDisplayLevels";

		// Token: 0x04000743 RID: 1859
		internal const string Menu_StaticDisplayPopOutImage = "Menu_StaticDisplayPopOutImage";

		// Token: 0x04000744 RID: 1860
		internal const string Menu_StaticHoverStyle = "Menu_StaticHoverStyle";

		// Token: 0x04000745 RID: 1861
		internal const string Menu_StaticItemFormatString = "Menu_StaticItemFormatString";

		// Token: 0x04000746 RID: 1862
		internal const string Menu_StaticMenuItemStyle = "Menu_StaticMenuItemStyle";

		// Token: 0x04000747 RID: 1863
		internal const string Menu_StaticMenuStyle = "Menu_StaticMenuStyle";

		// Token: 0x04000748 RID: 1864
		internal const string Menu_StaticPopoutImageText = "Menu_StaticPopoutImageText";

		// Token: 0x04000749 RID: 1865
		internal const string Menu_StaticPopoutImageUrl = "Menu_StaticPopoutImageUrl";

		// Token: 0x0400074A RID: 1866
		internal const string Menu_StaticSelectedStyle = "Menu_StaticSelectedStyle";

		// Token: 0x0400074B RID: 1867
		internal const string Menu_StaticSubMenuIndent = "Menu_StaticSubMenuIndent";

		// Token: 0x0400074C RID: 1868
		internal const string Menu_StaticTemplate = "Menu_StaticTemplate";

		// Token: 0x0400074D RID: 1869
		internal const string Menu_StaticTopSeparatorImageUrl = "Menu_StaticTopSeparatorImageUrl";

		// Token: 0x0400074E RID: 1870
		internal const string MultiView_ActiveView = "MultiView_ActiveView";

		// Token: 0x0400074F RID: 1871
		internal const string MultiView_ActiveViewChanged = "MultiView_ActiveViewChanged";

		// Token: 0x04000750 RID: 1872
		internal const string MultiView_ActiveViewIndex_out_of_range = "MultiView_ActiveViewIndex_out_of_range";

		// Token: 0x04000751 RID: 1873
		internal const string MultiView_cannot_have_children_of_type = "MultiView_cannot_have_children_of_type";

		// Token: 0x04000752 RID: 1874
		internal const string Multiview_rendering_block_not_allowed = "Multiview_rendering_block_not_allowed";

		// Token: 0x04000753 RID: 1875
		internal const string MultiView_Views = "MultiView_Views";

		// Token: 0x04000754 RID: 1876
		internal const string MultiView_invalid_view_id = "MultiView_invalid_view_id";

		// Token: 0x04000755 RID: 1877
		internal const string MultiView_invalid_view_index_format = "MultiView_invalid_view_index_format";

		// Token: 0x04000756 RID: 1878
		internal const string MultiView_view_not_found = "MultiView_view_not_found";

		// Token: 0x04000757 RID: 1879
		internal const string MultiView_ActiveViewIndex_less_than_minus_one = "MultiView_ActiveViewIndex_less_than_minus_one";

		// Token: 0x04000758 RID: 1880
		internal const string MultiView_ActiveViewIndex_equal_or_greater_than_count = "MultiView_ActiveViewIndex_equal_or_greater_than_count";

		// Token: 0x04000759 RID: 1881
		internal const string View_CannotSetVisible = "View_CannotSetVisible";

		// Token: 0x0400075A RID: 1882
		internal const string SiteMapPath_CannotFindUrl = "SiteMapPath_CannotFindUrl";

		// Token: 0x0400075B RID: 1883
		internal const string SiteMapPath_CurrentNodeStyle = "SiteMapPath_CurrentNodeStyle";

		// Token: 0x0400075C RID: 1884
		internal const string SiteMapPath_CurrentNodeTemplate = "SiteMapPath_CurrentNodeTemplate";

		// Token: 0x0400075D RID: 1885
		internal const string SiteMapPath_OnItemDataBound = "SiteMapPath_OnItemDataBound";

		// Token: 0x0400075E RID: 1886
		internal const string SiteMapPath_NodeStyle = "SiteMapPath_NodeStyle";

		// Token: 0x0400075F RID: 1887
		internal const string SiteMapPath_NodeTemplate = "SiteMapPath_NodeTemplate";

		// Token: 0x04000760 RID: 1888
		internal const string SiteMapPath_PathDirection = "SiteMapPath_PathDirection";

		// Token: 0x04000761 RID: 1889
		internal const string SiteMapPath_PathSeparator = "SiteMapPath_PathSeparator";

		// Token: 0x04000762 RID: 1890
		internal const string SiteMapPath_PathSeparatorTemplate = "SiteMapPath_PathSeparatorTemplate";

		// Token: 0x04000763 RID: 1891
		internal const string SiteMapPath_PathSeparatorStyle = "SiteMapPath_PathSeparatorStyle";

		// Token: 0x04000764 RID: 1892
		internal const string SiteMapPath_Provider = "SiteMapPath_Provider";

		// Token: 0x04000765 RID: 1893
		internal const string SiteMapPath_RenderCurrentNodeAsLink = "SiteMapPath_RenderCurrentNodeAsLink";

		// Token: 0x04000766 RID: 1894
		internal const string SiteMapPath_RootNodeStyle = "SiteMapPath_RootNodeStyle";

		// Token: 0x04000767 RID: 1895
		internal const string SiteMapPath_RootNodeTemplate = "SiteMapPath_RootNodeTemplate";

		// Token: 0x04000768 RID: 1896
		internal const string SiteMapPath_SiteMapProvider = "SiteMapPath_SiteMapProvider";

		// Token: 0x04000769 RID: 1897
		internal const string SiteMapPath_SkipToContentText = "SiteMapPath_SkipToContentText";

		// Token: 0x0400076A RID: 1898
		internal const string SiteMapPath_Default_SkipToContentText = "SiteMapPath_Default_SkipToContentText";

		// Token: 0x0400076B RID: 1899
		internal const string SiteMapPath_ShowToolTips = "SiteMapPath_ShowToolTips";

		// Token: 0x0400076C RID: 1900
		internal const string SiteMapPath_ParentLevelsDisplayed = "SiteMapPath_ParentLevelsDisplayed";

		// Token: 0x0400076D RID: 1901
		internal const string SubMenuStyle_HorizontalPadding = "SubMenuStyle_HorizontalPadding";

		// Token: 0x0400076E RID: 1902
		internal const string SubMenuStyle_VerticalPadding = "SubMenuStyle_VerticalPadding";

		// Token: 0x0400076F RID: 1903
		internal const string SubMenuStyleCollection_InvalidArgument = "SubMenuStyleCollection_InvalidArgument";

		// Token: 0x04000770 RID: 1904
		internal const string Panel_AllowPaginate = "Panel_AllowPaginate";

		// Token: 0x04000771 RID: 1905
		internal const string Panel_BackImageUrl = "Panel_BackImageUrl";

		// Token: 0x04000772 RID: 1906
		internal const string Panel_DefaultButton = "Panel_DefaultButton";

		// Token: 0x04000773 RID: 1907
		internal const string Panel_Direction = "Panel_Direction";

		// Token: 0x04000774 RID: 1908
		internal const string Panel_GroupingText = "Panel_GroupingText";

		// Token: 0x04000775 RID: 1909
		internal const string Panel_HorizontalAlign = "Panel_HorizontalAlign";

		// Token: 0x04000776 RID: 1910
		internal const string Panel_ScrollBars = "Panel_ScrollBars";

		// Token: 0x04000777 RID: 1911
		internal const string Panel_Wrap = "Panel_Wrap";

		// Token: 0x04000778 RID: 1912
		internal const string PasswordRecovery_AnswerLabelText = "PasswordRecovery_AnswerLabelText";

		// Token: 0x04000779 RID: 1913
		internal const string PasswordRecovery_AnswerLookupError = "PasswordRecovery_AnswerLookupError";

		// Token: 0x0400077A RID: 1914
		internal const string PasswordRecovery_VerifyingAnswer = "PasswordRecovery_VerifyingAnswer";

		// Token: 0x0400077B RID: 1915
		internal const string PasswordRecovery_SendingMail = "PasswordRecovery_SendingMail";

		// Token: 0x0400077C RID: 1916
		internal const string PasswordRecovery_VerifyingUser = "PasswordRecovery_VerifyingUser";

		// Token: 0x0400077D RID: 1917
		internal const string PasswordRecovery_DefaultAnswerLabelText = "PasswordRecovery_DefaultAnswerLabelText";

		// Token: 0x0400077E RID: 1918
		internal const string PasswordRecovery_DefaultAnswerRequiredErrorMessage = "PasswordRecovery_DefaultAnswerRequiredErrorMessage";

		// Token: 0x0400077F RID: 1919
		internal const string PasswordRecovery_DefaultBody = "PasswordRecovery_DefaultBody";

		// Token: 0x04000780 RID: 1920
		internal const string PasswordRecovery_DefaultGeneralFailureText = "PasswordRecovery_DefaultGeneralFailureText";

		// Token: 0x04000781 RID: 1921
		internal const string PasswordRecovery_DefaultUserNameFailureText = "PasswordRecovery_DefaultUserNameFailureText";

		// Token: 0x04000782 RID: 1922
		internal const string PasswordRecovery_DefaultQuestionInstructionText = "PasswordRecovery_DefaultQuestionInstructionText";

		// Token: 0x04000783 RID: 1923
		internal const string PasswordRecovery_DefaultQuestionFailureText = "PasswordRecovery_DefaultQuestionFailureText";

		// Token: 0x04000784 RID: 1924
		internal const string PasswordRecovery_DefaultQuestionLabelText = "PasswordRecovery_DefaultQuestionLabelText";

		// Token: 0x04000785 RID: 1925
		internal const string PasswordRecovery_DefaultQuestionTitleText = "PasswordRecovery_DefaultQuestionTitleText";

		// Token: 0x04000786 RID: 1926
		internal const string PasswordRecovery_DefaultSubject = "PasswordRecovery_DefaultSubject";

		// Token: 0x04000787 RID: 1927
		internal const string PasswordRecovery_DefaultSubmitButtonText = "PasswordRecovery_DefaultSubmitButtonText";

		// Token: 0x04000788 RID: 1928
		internal const string PasswordRecovery_DefaultSuccessText = "PasswordRecovery_DefaultSuccessText";

		// Token: 0x04000789 RID: 1929
		internal const string PasswordRecovery_DefaultUserNameInstructionText = "PasswordRecovery_DefaultUserNameInstructionText";

		// Token: 0x0400078A RID: 1930
		internal const string PasswordRecovery_DefaultUserNameLabelText = "PasswordRecovery_DefaultUserNameLabelText";

		// Token: 0x0400078B RID: 1931
		internal const string PasswordRecovery_DefaultUserNameRequiredErrorMessage = "PasswordRecovery_DefaultUserNameRequiredErrorMessage";

		// Token: 0x0400078C RID: 1932
		internal const string PasswordRecovery_DefaultUserNameTitleText = "PasswordRecovery_DefaultUserNameTitleText";

		// Token: 0x0400078D RID: 1933
		internal const string PasswordRecovery_GeneralFailureText = "PasswordRecovery_GeneralFailureText";

		// Token: 0x0400078E RID: 1934
		internal const string PasswordRecovery_InvalidBorderPadding = "PasswordRecovery_InvalidBorderPadding";

		// Token: 0x0400078F RID: 1935
		internal const string PasswordRecovery_MailDefinition = "PasswordRecovery_MailDefinition";

		// Token: 0x04000790 RID: 1936
		internal const string PasswordRecovery_NoUserNameTextBox = "PasswordRecovery_NoUserNameTextBox";

		// Token: 0x04000791 RID: 1937
		internal const string PasswordRecovery_NoAnswerTextBox = "PasswordRecovery_NoAnswerTextBox";

		// Token: 0x04000792 RID: 1938
		internal const string PasswordRecovery_QuestionFailureText = "PasswordRecovery_QuestionFailureText";

		// Token: 0x04000793 RID: 1939
		internal const string PasswordRecovery_QuestionInstructionText = "PasswordRecovery_QuestionInstructionText";

		// Token: 0x04000794 RID: 1940
		internal const string PasswordRecovery_QuestionLabelText = "PasswordRecovery_QuestionLabelText";

		// Token: 0x04000795 RID: 1941
		internal const string PasswordRecovery_QuestionTemplate = "PasswordRecovery_QuestionTemplate";

		// Token: 0x04000796 RID: 1942
		internal const string PasswordRecovery_QuestionTemplateContainer = "PasswordRecovery_QuestionTemplateContainer";

		// Token: 0x04000797 RID: 1943
		internal const string PasswordRecovery_QuestionTitleText = "PasswordRecovery_QuestionTitleText";

		// Token: 0x04000798 RID: 1944
		internal const string PasswordRecovery_RecoveryNotSupported = "PasswordRecovery_RecoveryNotSupported";

		// Token: 0x04000799 RID: 1945
		internal const string PasswordRecovery_SubmitButtonStyle = "PasswordRecovery_SubmitButtonStyle";

		// Token: 0x0400079A RID: 1946
		internal const string PasswordRecovery_SubmitButtonType = "PasswordRecovery_SubmitButtonType";

		// Token: 0x0400079B RID: 1947
		internal const string PasswordRecovery_SuccessTemplate = "PasswordRecovery_SuccessTemplate";

		// Token: 0x0400079C RID: 1948
		internal const string PasswordRecovery_SuccessTemplateContainer = "PasswordRecovery_SuccessTemplateContainer";

		// Token: 0x0400079D RID: 1949
		internal const string PasswordRecovery_SuccessText = "PasswordRecovery_SuccessText";

		// Token: 0x0400079E RID: 1950
		internal const string PasswordRecovery_SuccessTextStyle = "PasswordRecovery_SuccessTextStyle";

		// Token: 0x0400079F RID: 1951
		internal const string PasswordRecovery_UserLookupError = "PasswordRecovery_UserLookupError";

		// Token: 0x040007A0 RID: 1952
		internal const string PasswordRecovery_UserNameFailureText = "PasswordRecovery_UserNameFailureText";

		// Token: 0x040007A1 RID: 1953
		internal const string PasswordRecovery_UserNameInstructionText = "PasswordRecovery_UserNameInstructionText";

		// Token: 0x040007A2 RID: 1954
		internal const string PasswordRecovery_UserNameLabelText = "PasswordRecovery_UserNameLabelText";

		// Token: 0x040007A3 RID: 1955
		internal const string PasswordRecovery_UserNameTemplate = "PasswordRecovery_UserNameTemplate";

		// Token: 0x040007A4 RID: 1956
		internal const string PasswordRecovery_UserNameTemplateContainer = "PasswordRecovery_UserNameTemplateContainer";

		// Token: 0x040007A5 RID: 1957
		internal const string PasswordRecovery_UserNameTitleText = "PasswordRecovery_UserNameTitleText";

		// Token: 0x040007A6 RID: 1958
		internal const string PolygonHotSpot_Coordinates = "PolygonHotSpot_Coordinates";

		// Token: 0x040007A7 RID: 1959
		internal const string RadioButton_GroupName = "RadioButton_GroupName";

		// Token: 0x040007A8 RID: 1960
		internal const string RadioButtonList_CellPadding = "RadioButtonList_CellPadding";

		// Token: 0x040007A9 RID: 1961
		internal const string RadioButtonList_CellSpacing = "RadioButtonList_CellSpacing";

		// Token: 0x040007AA RID: 1962
		internal const string RadioButtonList_RepeatColumns = "RadioButtonList_RepeatColumns";

		// Token: 0x040007AB RID: 1963
		internal const string RangeValidator_MaximumValue = "RangeValidator_MaximumValue";

		// Token: 0x040007AC RID: 1964
		internal const string RangeValidator_MinmumValue = "RangeValidator_MinmumValue";

		// Token: 0x040007AD RID: 1965
		internal const string RangeValidator_Type = "RangeValidator_Type";

		// Token: 0x040007AE RID: 1966
		internal const string ReadOnlyHierarchicalDataSourceView_CantAccessPathInEnumerable = "ReadOnlyHierarchicalDataSourceView_CantAccessPathInEnumerable";

		// Token: 0x040007AF RID: 1967
		internal const string RectangleHotSpot_Bottom = "RectangleHotSpot_Bottom";

		// Token: 0x040007B0 RID: 1968
		internal const string RectangleHotSpot_Right = "RectangleHotSpot_Right";

		// Token: 0x040007B1 RID: 1969
		internal const string RectangleHotSpot_Top = "RectangleHotSpot_Top";

		// Token: 0x040007B2 RID: 1970
		internal const string RectangleHotSpot_Left = "RectangleHotSpot_Left";

		// Token: 0x040007B3 RID: 1971
		internal const string RegularExpressionValidator_ValidationExpression = "RegularExpressionValidator_ValidationExpression";

		// Token: 0x040007B4 RID: 1972
		internal const string Repeater_AlternatingItemTemplate = "Repeater_AlternatingItemTemplate";

		// Token: 0x040007B5 RID: 1973
		internal const string Repeater_DataMember = "Repeater_DataMember";

		// Token: 0x040007B6 RID: 1974
		internal const string Repeater_FooterTemplate = "Repeater_FooterTemplate";

		// Token: 0x040007B7 RID: 1975
		internal const string Repeater_Items = "Repeater_Items";

		// Token: 0x040007B8 RID: 1976
		internal const string Repeater_ItemTemplate = "Repeater_ItemTemplate";

		// Token: 0x040007B9 RID: 1977
		internal const string Repeater_OnItemCommand = "Repeater_OnItemCommand";

		// Token: 0x040007BA RID: 1978
		internal const string Repeater_SeparatorTemplate = "Repeater_SeparatorTemplate";

		// Token: 0x040007BB RID: 1979
		internal const string RequiredFieldValidator_InitialValue = "RequiredFieldValidator_InitialValue";

		// Token: 0x040007BC RID: 1980
		internal const string SiteMapDataSource_Description = "SiteMapDataSource_Description";

		// Token: 0x040007BD RID: 1981
		internal const string SiteMapDataSource_DisplayName = "SiteMapDataSource_DisplayName";

		// Token: 0x040007BE RID: 1982
		internal const string SiteMapDataSource_Provider = "SiteMapDataSource_Provider";

		// Token: 0x040007BF RID: 1983
		internal const string SiteMapDataSource_ContainsListCollection = "SiteMapDataSource_ContainsListCollection";

		// Token: 0x040007C0 RID: 1984
		internal const string SiteMapDataSource_StartingNodeOffset = "SiteMapDataSource_StartingNodeOffset";

		// Token: 0x040007C1 RID: 1985
		internal const string SiteMapDataSource_StartingNodeUrl = "SiteMapDataSource_StartingNodeUrl";

		// Token: 0x040007C2 RID: 1986
		internal const string SiteMapDataSource_SiteMapProvider = "SiteMapDataSource_SiteMapProvider";

		// Token: 0x040007C3 RID: 1987
		internal const string SiteMapDataSource_ProviderNotFound = "SiteMapDataSource_ProviderNotFound";

		// Token: 0x040007C4 RID: 1988
		internal const string SiteMapDataSource_DefaultProviderNotFound = "SiteMapDataSource_DefaultProviderNotFound";

		// Token: 0x040007C5 RID: 1989
		internal const string SiteMapDataSource_ShowStartingNode = "SiteMapDataSource_ShowStartingNode";

		// Token: 0x040007C6 RID: 1990
		internal const string SiteMapDataSource_StartFromCurrentNode = "SiteMapDataSource_StartFromCurrentNode";

		// Token: 0x040007C7 RID: 1991
		internal const string SiteMapDataSource_StartingNodeUrlAndStartFromcurrentNode_Defined = "SiteMapDataSource_StartingNodeUrlAndStartFromcurrentNode_Defined";

		// Token: 0x040007C8 RID: 1992
		internal const string GridView_AllowPaging = "GridView_AllowPaging";

		// Token: 0x040007C9 RID: 1993
		internal const string GridView_AllowSorting = "GridView_AllowSorting";

		// Token: 0x040007CA RID: 1994
		internal const string GridView_AlternatingRowStyle = "GridView_AlternatingRowStyle";

		// Token: 0x040007CB RID: 1995
		internal const string GridView_AutoGenerateDeleteButton = "GridView_AutoGenerateDeleteButton";

		// Token: 0x040007CC RID: 1996
		internal const string GridView_AutoGenerateEditButton = "GridView_AutoGenerateEditButton";

		// Token: 0x040007CD RID: 1997
		internal const string GridView_AutoGenerateSelectButton = "GridView_AutoGenerateSelectButton";

		// Token: 0x040007CE RID: 1998
		internal const string GridView_CellPadding = "GridView_CellPadding";

		// Token: 0x040007CF RID: 1999
		internal const string GridView_CellSpacing = "GridView_CellSpacing";

		// Token: 0x040007D0 RID: 2000
		internal const string GridView_DataKeys = "GridView_DataKeys";

		// Token: 0x040007D1 RID: 2001
		internal const string GridView_EditIndex = "GridView_EditIndex";

		// Token: 0x040007D2 RID: 2002
		internal const string GridView_EditRowStyle = "GridView_EditRowStyle";

		// Token: 0x040007D3 RID: 2003
		internal const string GridView_EnableSortingAndPagingCallbacks = "GridView_EnableSortingAndPagingCallbacks";

		// Token: 0x040007D4 RID: 2004
		internal const string GridView_EmptyDataRowStyle = "GridView_EmptyDataRowStyle";

		// Token: 0x040007D5 RID: 2005
		internal const string GridView_OnRowCancelingEdit = "GridView_OnRowCancelingEdit";

		// Token: 0x040007D6 RID: 2006
		internal const string GridView_OnRowEditing = "GridView_OnRowEditing";

		// Token: 0x040007D7 RID: 2007
		internal const string GridView_OnPageIndexChanging = "GridView_OnPageIndexChanging";

		// Token: 0x040007D8 RID: 2008
		internal const string GridView_OnPageIndexChanged = "GridView_OnPageIndexChanged";

		// Token: 0x040007D9 RID: 2009
		internal const string GridView_OnSelectedIndexChanged = "GridView_OnSelectedIndexChanged";

		// Token: 0x040007DA RID: 2010
		internal const string GridView_OnSelectedIndexChanging = "GridView_OnSelectedIndexChanging";

		// Token: 0x040007DB RID: 2011
		internal const string GridView_OnSorted = "GridView_OnSorted";

		// Token: 0x040007DC RID: 2012
		internal const string GridView_OnSorting = "GridView_OnSorting";

		// Token: 0x040007DD RID: 2013
		internal const string GridView_OnRowCommand = "GridView_OnRowCommand";

		// Token: 0x040007DE RID: 2014
		internal const string GridView_OnRowCreated = "GridView_OnRowCreated";

		// Token: 0x040007DF RID: 2015
		internal const string GridView_OnRowDataBound = "GridView_OnRowDataBound";

		// Token: 0x040007E0 RID: 2016
		internal const string GridView_PageCount = "GridView_PageCount";

		// Token: 0x040007E1 RID: 2017
		internal const string GridView_PageIndex = "GridView_PageIndex";

		// Token: 0x040007E2 RID: 2018
		internal const string GridView_PagerSettings = "GridView_PagerSettings";

		// Token: 0x040007E3 RID: 2019
		internal const string GridView_PageSize = "GridView_PageSize";

		// Token: 0x040007E4 RID: 2020
		internal const string GridView_RowHeaderColumn = "GridView_RowHeaderColumn";

		// Token: 0x040007E5 RID: 2021
		internal const string GridView_Rows = "GridView_Rows";

		// Token: 0x040007E6 RID: 2022
		internal const string GridView_SelectedIndex = "GridView_SelectedIndex";

		// Token: 0x040007E7 RID: 2023
		internal const string GridView_SelectedRow = "GridView_SelectedRow";

		// Token: 0x040007E8 RID: 2024
		internal const string GridView_SelectedRowStyle = "GridView_SelectedRowStyle";

		// Token: 0x040007E9 RID: 2025
		internal const string GridView_SortDirection = "GridView_SortDirection";

		// Token: 0x040007EA RID: 2026
		internal const string GridView_SortExpression = "GridView_SortExpression";

		// Token: 0x040007EB RID: 2027
		internal const string PagerSettings_FirstPageImageUrl = "PagerSettings_FirstPageImageUrl";

		// Token: 0x040007EC RID: 2028
		internal const string PagerSettings_FirstPageText = "PagerSettings_FirstPageText";

		// Token: 0x040007ED RID: 2029
		internal const string PagerSettings_LastPageImageUrl = "PagerSettings_LastPageImageUrl";

		// Token: 0x040007EE RID: 2030
		internal const string PagerSettings_LastPageText = "PagerSettings_LastPageText";

		// Token: 0x040007EF RID: 2031
		internal const string PagerSettings_Mode = "PagerSettings_Mode";

		// Token: 0x040007F0 RID: 2032
		internal const string PagerSettings_NextPageImageUrl = "PagerSettings_NextPageImageUrl";

		// Token: 0x040007F1 RID: 2033
		internal const string PagerSettings_PageButtonCount = "PagerSettings_PageButtonCount";

		// Token: 0x040007F2 RID: 2034
		internal const string PagerSettings_PreviousPageImageUrl = "PagerSettings_PreviousPageImageUrl";

		// Token: 0x040007F3 RID: 2035
		internal const string PagerStyle_Position = "PagerStyle_Position";

		// Token: 0x040007F4 RID: 2036
		internal const string PagerStyle_Visible = "PagerStyle_Visible";

		// Token: 0x040007F5 RID: 2037
		internal const string Style_BackColor = "Style_BackColor";

		// Token: 0x040007F6 RID: 2038
		internal const string Style_BorderColor = "Style_BorderColor";

		// Token: 0x040007F7 RID: 2039
		internal const string Style_BorderWidth = "Style_BorderWidth";

		// Token: 0x040007F8 RID: 2040
		internal const string Style_BorderStyle = "Style_BorderStyle";

		// Token: 0x040007F9 RID: 2041
		internal const string Style_CSSClass = "Style_CSSClass";

		// Token: 0x040007FA RID: 2042
		internal const string Style_Font = "Style_Font";

		// Token: 0x040007FB RID: 2043
		internal const string Style_ForeColor = "Style_ForeColor";

		// Token: 0x040007FC RID: 2044
		internal const string Style_Height = "Style_Height";

		// Token: 0x040007FD RID: 2045
		internal const string Style_Width = "Style_Width";

		// Token: 0x040007FE RID: 2046
		internal const string Substitution_MethodNameDescr = "Substitution_MethodNameDescr";

		// Token: 0x040007FF RID: 2047
		internal const string Substitution_CannotBeInCachedControl = "Substitution_CannotBeInCachedControl";

		// Token: 0x04000800 RID: 2048
		internal const string Substitution_BadMethodName = "Substitution_BadMethodName";

		// Token: 0x04000801 RID: 2049
		internal const string Substitution_NotAllowed = "Substitution_NotAllowed";

		// Token: 0x04000802 RID: 2050
		internal const string Substitution_SiteNotAllowed = "Substitution_SiteNotAllowed";

		// Token: 0x04000803 RID: 2051
		internal const string Table_SectionsMustBeInOrder = "Table_SectionsMustBeInOrder";

		// Token: 0x04000804 RID: 2052
		internal const string Table_BackImageUrl = "Table_BackImageUrl";

		// Token: 0x04000805 RID: 2053
		internal const string Table_Caption = "Table_Caption";

		// Token: 0x04000806 RID: 2054
		internal const string Table_CellSpacing = "Table_CellSpacing";

		// Token: 0x04000807 RID: 2055
		internal const string Table_CellPadding = "Table_CellPadding";

		// Token: 0x04000808 RID: 2056
		internal const string Table_GridLines = "Table_GridLines";

		// Token: 0x04000809 RID: 2057
		internal const string Table_HorizontalAlign = "Table_HorizontalAlign";

		// Token: 0x0400080A RID: 2058
		internal const string Table_InvalidNextRowPost = "Table_InvalidNextRowPost";

		// Token: 0x0400080B RID: 2059
		internal const string Table_InvalidPrevRowPost = "Table_InvalidPrevRowPost";

		// Token: 0x0400080C RID: 2060
		internal const string Table_Rows = "Table_Rows";

		// Token: 0x0400080D RID: 2061
		internal const string TableCell_AssociatedHeaderCellNotFound = "TableCell_AssociatedHeaderCellNotFound";

		// Token: 0x0400080E RID: 2062
		internal const string TableCell_AssociatedHeaderCellID = "TableCell_AssociatedHeaderCellID";

		// Token: 0x0400080F RID: 2063
		internal const string TableCell_ColumnSpan = "TableCell_ColumnSpan";

		// Token: 0x04000810 RID: 2064
		internal const string TableCell_RowSpan = "TableCell_RowSpan";

		// Token: 0x04000811 RID: 2065
		internal const string TableCell_Text = "TableCell_Text";

		// Token: 0x04000812 RID: 2066
		internal const string TableCell_Wrap = "TableCell_Wrap";

		// Token: 0x04000813 RID: 2067
		internal const string TableHeaderCell_AbbreviatedText = "TableHeaderCell_AbbreviatedText";

		// Token: 0x04000814 RID: 2068
		internal const string TableHeaderCell_Scope = "TableHeaderCell_Scope";

		// Token: 0x04000815 RID: 2069
		internal const string TableHeaderCell_CategoryText = "TableHeaderCell_CategoryText";

		// Token: 0x04000816 RID: 2070
		internal const string TableItemStyle_Wrap = "TableItemStyle_Wrap";

		// Token: 0x04000817 RID: 2071
		internal const string TableRow_Cells = "TableRow_Cells";

		// Token: 0x04000818 RID: 2072
		internal const string TableRow_TableSection = "TableRow_TableSection";

		// Token: 0x04000819 RID: 2073
		internal const string TableSectionStyle_Visible = "TableSectionStyle_Visible";

		// Token: 0x0400081A RID: 2074
		internal const string TableStyle_BackImageUrl = "TableStyle_BackImageUrl";

		// Token: 0x0400081B RID: 2075
		internal const string TableStyle_CellPadding = "TableStyle_CellPadding";

		// Token: 0x0400081C RID: 2076
		internal const string TableStyle_CellSpacing = "TableStyle_CellSpacing";

		// Token: 0x0400081D RID: 2077
		internal const string TableStyle_GridLines = "TableStyle_GridLines";

		// Token: 0x0400081E RID: 2078
		internal const string TableStyle_InvalidCellSpacing = "TableStyle_InvalidCellSpacing";

		// Token: 0x0400081F RID: 2079
		internal const string TableStyle_InvalidCellPadding = "TableStyle_InvalidCellPadding";

		// Token: 0x04000820 RID: 2080
		internal const string TableStyle_HorizontalAlign = "TableStyle_HorizontalAlign";

		// Token: 0x04000821 RID: 2081
		internal const string Control_Missing_Attribute = "Control_Missing_Attribute";

		// Token: 0x04000822 RID: 2082
		internal const string TemplateColumn_EditItemTemplate = "TemplateColumn_EditItemTemplate";

		// Token: 0x04000823 RID: 2083
		internal const string TemplateColumn_FooterTemplate = "TemplateColumn_FooterTemplate";

		// Token: 0x04000824 RID: 2084
		internal const string TemplateColumn_HeaderTemplate = "TemplateColumn_HeaderTemplate";

		// Token: 0x04000825 RID: 2085
		internal const string TemplateColumn_ItemTemplate = "TemplateColumn_ItemTemplate";

		// Token: 0x04000826 RID: 2086
		internal const string TemplateField_AlternatingItemTemplate = "TemplateField_AlternatingItemTemplate";

		// Token: 0x04000827 RID: 2087
		internal const string TemplateField_EditItemTemplate = "TemplateField_EditItemTemplate";

		// Token: 0x04000828 RID: 2088
		internal const string TemplateField_FooterTemplate = "TemplateField_FooterTemplate";

		// Token: 0x04000829 RID: 2089
		internal const string TemplateField_HeaderTemplate = "TemplateField_HeaderTemplate";

		// Token: 0x0400082A RID: 2090
		internal const string TemplateField_InsertItemTemplate = "TemplateField_InsertItemTemplate";

		// Token: 0x0400082B RID: 2091
		internal const string TemplateField_ItemTemplate = "TemplateField_ItemTemplate";

		// Token: 0x0400082C RID: 2092
		internal const string TextBox_AutoCompleteType = "TextBox_AutoCompleteType";

		// Token: 0x0400082D RID: 2093
		internal const string TextBox_AutoPostBack = "TextBox_AutoPostBack";

		// Token: 0x0400082E RID: 2094
		internal const string TextBox_Columns = "TextBox_Columns";

		// Token: 0x0400082F RID: 2095
		internal const string TextBox_InvalidColumns = "TextBox_InvalidColumns";

		// Token: 0x04000830 RID: 2096
		internal const string TextBox_InvalidRows = "TextBox_InvalidRows";

		// Token: 0x04000831 RID: 2097
		internal const string TextBox_MaxLength = "TextBox_MaxLength";

		// Token: 0x04000832 RID: 2098
		internal const string TextBox_TextMode = "TextBox_TextMode";

		// Token: 0x04000833 RID: 2099
		internal const string TextBox_ReadOnly = "TextBox_ReadOnly";

		// Token: 0x04000834 RID: 2100
		internal const string TextBox_Rows = "TextBox_Rows";

		// Token: 0x04000835 RID: 2101
		internal const string TextBox_Text = "TextBox_Text";

		// Token: 0x04000836 RID: 2102
		internal const string TextBox_Wrap = "TextBox_Wrap";

		// Token: 0x04000837 RID: 2103
		internal const string TextBox_OnTextChanged = "TextBox_OnTextChanged";

		// Token: 0x04000838 RID: 2104
		internal const string TreeNodeStyle_ChildNodesPadding = "TreeNodeStyle_ChildNodesPadding";

		// Token: 0x04000839 RID: 2105
		internal const string TreeNodeStyle_HorizontalPadding = "TreeNodeStyle_HorizontalPadding";

		// Token: 0x0400083A RID: 2106
		internal const string TreeNodeStyle_ImageUrl = "TreeNodeStyle_ImageUrl";

		// Token: 0x0400083B RID: 2107
		internal const string TreeNodeStyle_NodeSpacing = "TreeNodeStyle_NodeSpacing";

		// Token: 0x0400083C RID: 2108
		internal const string TreeNodeStyle_VerticalPadding = "TreeNodeStyle_VerticalPadding";

		// Token: 0x0400083D RID: 2109
		internal const string TreeNodeStyleCollection_InvalidArgument = "TreeNodeStyleCollection_InvalidArgument";

		// Token: 0x0400083E RID: 2110
		internal const string TreeNodeBinding_Depth = "TreeNodeBinding_Depth";

		// Token: 0x0400083F RID: 2111
		internal const string TreeNodeBinding_EmptyBindingText = "TreeNodeBinding_EmptyBindingText";

		// Token: 0x04000840 RID: 2112
		internal const string TreeNodeBinding_FormatString = "TreeNodeBinding_FormatString";

		// Token: 0x04000841 RID: 2113
		internal const string TreeNodeBinding_ImageToolTip = "TreeNodeBinding_ImageToolTip";

		// Token: 0x04000842 RID: 2114
		internal const string TreeNodeBinding_ImageToolTipField = "TreeNodeBinding_ImageToolTipField";

		// Token: 0x04000843 RID: 2115
		internal const string TreeNodeBinding_ImageUrl = "TreeNodeBinding_ImageUrl";

		// Token: 0x04000844 RID: 2116
		internal const string TreeNodeBinding_ImageUrlField = "TreeNodeBinding_ImageUrlField";

		// Token: 0x04000845 RID: 2117
		internal const string TreeNodeBinding_NavigateUrl = "TreeNodeBinding_NavigateUrl";

		// Token: 0x04000846 RID: 2118
		internal const string TreeNodeBinding_NavigateUrlField = "TreeNodeBinding_NavigateUrlField";

		// Token: 0x04000847 RID: 2119
		internal const string TreeNodeBinding_PopulateOnDemand = "TreeNodeBinding_PopulateOnDemand";

		// Token: 0x04000848 RID: 2120
		internal const string TreeNodeBinding_SelectAction = "TreeNodeBinding_SelectAction";

		// Token: 0x04000849 RID: 2121
		internal const string TreeNodeBinding_ShowCheckBox = "TreeNodeBinding_ShowCheckBox";

		// Token: 0x0400084A RID: 2122
		internal const string TreeNodeBinding_Target = "TreeNodeBinding_Target";

		// Token: 0x0400084B RID: 2123
		internal const string TreeNodeBinding_TargetField = "TreeNodeBinding_TargetField";

		// Token: 0x0400084C RID: 2124
		internal const string TreeNodeBinding_Text = "TreeNodeBinding_Text";

		// Token: 0x0400084D RID: 2125
		internal const string TreeNodeBinding_TextField = "TreeNodeBinding_TextField";

		// Token: 0x0400084E RID: 2126
		internal const string TreeNodeBinding_ToolTip = "TreeNodeBinding_ToolTip";

		// Token: 0x0400084F RID: 2127
		internal const string TreeNodeBinding_ToolTipField = "TreeNodeBinding_ToolTipField";

		// Token: 0x04000850 RID: 2128
		internal const string TreeNodeBinding_Value = "TreeNodeBinding_Value";

		// Token: 0x04000851 RID: 2129
		internal const string TreeNodeBinding_ValueField = "TreeNodeBinding_ValueField";

		// Token: 0x04000852 RID: 2130
		internal const string TreeNodeCollection_InvalidArrayType = "TreeNodeCollection_InvalidArrayType";

		// Token: 0x04000853 RID: 2131
		internal const string TreeNode_Checked = "TreeNode_Checked";

		// Token: 0x04000854 RID: 2132
		internal const string TreeView_DataSourceReturnedNullView = "TreeView_DataSourceReturnedNullView";

		// Token: 0x04000855 RID: 2133
		internal const string TreeNode_Expanded = "TreeNode_Expanded";

		// Token: 0x04000856 RID: 2134
		internal const string TreeNode_ImageToolTip = "TreeNode_ImageToolTip";

		// Token: 0x04000857 RID: 2135
		internal const string TreeNode_ImageUrl = "TreeNode_ImageUrl";

		// Token: 0x04000858 RID: 2136
		internal const string TreeView_InvalidDataBinding = "TreeView_InvalidDataBinding";

		// Token: 0x04000859 RID: 2137
		internal const string TreeNode_NavigateUrl = "TreeNode_NavigateUrl";

		// Token: 0x0400085A RID: 2138
		internal const string TreeNode_PopulateOnDemand = "TreeNode_PopulateOnDemand";

		// Token: 0x0400085B RID: 2139
		internal const string TreeView_PopulateOnlyForDataSourceControls = "TreeView_PopulateOnlyForDataSourceControls";

		// Token: 0x0400085C RID: 2140
		internal const string TreeView_PopulateOnlyEmptyNodes = "TreeView_PopulateOnlyEmptyNodes";

		// Token: 0x0400085D RID: 2141
		internal const string TreeNode_Selected = "TreeNode_Selected";

		// Token: 0x0400085E RID: 2142
		internal const string TreeNode_SelectAction = "TreeNode_SelectAction";

		// Token: 0x0400085F RID: 2143
		internal const string TreeNode_ShowCheckBox = "TreeNode_ShowCheckBox";

		// Token: 0x04000860 RID: 2144
		internal const string TreeNode_Target = "TreeNode_Target";

		// Token: 0x04000861 RID: 2145
		internal const string TreeNode_Text = "TreeNode_Text";

		// Token: 0x04000862 RID: 2146
		internal const string TreeNode_ToolTip = "TreeNode_ToolTip";

		// Token: 0x04000863 RID: 2147
		internal const string TreeNode_Value = "TreeNode_Value";

		// Token: 0x04000864 RID: 2148
		internal const string TreeView_AutoGenerateDataBindings = "TreeView_AutoGenerateDataBindings";

		// Token: 0x04000865 RID: 2149
		internal const string TreeView_DataBindings = "TreeView_DataBindings";

		// Token: 0x04000866 RID: 2150
		internal const string TreeView_CollapseImageToolTip = "TreeView_CollapseImageToolTip";

		// Token: 0x04000867 RID: 2151
		internal const string TreeView_CollapseImageToolTipDefaultValue = "TreeView_CollapseImageToolTipDefaultValue";

		// Token: 0x04000868 RID: 2152
		internal const string TreeView_CollapseImageUrl = "TreeView_CollapseImageUrl";

		// Token: 0x04000869 RID: 2153
		internal const string TreeView_Default_SkipLinkText = "TreeView_Default_SkipLinkText";

		// Token: 0x0400086A RID: 2154
		internal const string TreeView_EnableClientScript = "TreeView_EnableClientScript";

		// Token: 0x0400086B RID: 2155
		internal const string TreeView_ExpandImageToolTip = "TreeView_ExpandImageToolTip";

		// Token: 0x0400086C RID: 2156
		internal const string TreeView_ExpandImageToolTipDefaultValue = "TreeView_ExpandImageToolTipDefaultValue";

		// Token: 0x0400086D RID: 2157
		internal const string TreeView_ExpandImageUrl = "TreeView_ExpandImageUrl";

		// Token: 0x0400086E RID: 2158
		internal const string TreeView_HoverNodeStyle = "TreeView_HoverNodeStyle";

		// Token: 0x0400086F RID: 2159
		internal const string TreeView_ExpandDepth = "TreeView_ExpandDepth";

		// Token: 0x04000870 RID: 2160
		internal const string TreeView_ImageSet = "TreeView_ImageSet";

		// Token: 0x04000871 RID: 2161
		internal const string TreeView_LeafNodeStyle = "TreeView_LeafNodeStyle";

		// Token: 0x04000872 RID: 2162
		internal const string TreeView_LevelStyles = "TreeView_LevelStyles";

		// Token: 0x04000873 RID: 2163
		internal const string TreeView_LineImagesFolderUrl = "TreeView_LineImagesFolderUrl";

		// Token: 0x04000874 RID: 2164
		internal const string TreeView_MaxDataBindDepth = "TreeView_MaxDataBindDepth";

		// Token: 0x04000875 RID: 2165
		internal const string TreeView_NoExpandImageUrl = "TreeView_NoExpandImageUrl";

		// Token: 0x04000876 RID: 2166
		internal const string TreeView_NodeIndent = "TreeView_NodeIndent";

		// Token: 0x04000877 RID: 2167
		internal const string TreeView_Nodes = "TreeView_Nodes";

		// Token: 0x04000878 RID: 2168
		internal const string TreeView_NodeStyle = "TreeView_NodeStyle";

		// Token: 0x04000879 RID: 2169
		internal const string TreeView_NodeWrap = "TreeView_NodeWrap";

		// Token: 0x0400087A RID: 2170
		internal const string TreeView_ParentNodeStyle = "TreeView_ParentNodeStyle";

		// Token: 0x0400087B RID: 2171
		internal const string TreeView_PathSeparator = "TreeView_PathSeparator";

		// Token: 0x0400087C RID: 2172
		internal const string TreeView_PopulateNodesFromClient = "TreeView_PopulateNodesFromClient";

		// Token: 0x0400087D RID: 2173
		internal const string TreeView_RootNodeStyle = "TreeView_RootNodeStyle";

		// Token: 0x0400087E RID: 2174
		internal const string TreeView_SelectedNodeStyle = "TreeView_SelectedNodeStyle";

		// Token: 0x0400087F RID: 2175
		internal const string TreeView_ShowCheckBoxes = "TreeView_ShowCheckBoxes";

		// Token: 0x04000880 RID: 2176
		internal const string TreeView_ShowExpandCollapse = "TreeView_ShowExpandCollapse";

		// Token: 0x04000881 RID: 2177
		internal const string TreeView_ShowLines = "TreeView_ShowLines";

		// Token: 0x04000882 RID: 2178
		internal const string TreeView_SkipLinkText = "TreeView_SkipLinkText";

		// Token: 0x04000883 RID: 2179
		internal const string TreeView_CheckChanged = "TreeView_CheckChanged";

		// Token: 0x04000884 RID: 2180
		internal const string TreeView_SelectedNodeChanged = "TreeView_SelectedNodeChanged";

		// Token: 0x04000885 RID: 2181
		internal const string TreeView_TreeNodeCollapsed = "TreeView_TreeNodeCollapsed";

		// Token: 0x04000886 RID: 2182
		internal const string TreeView_TreeNodeExpanded = "TreeView_TreeNodeExpanded";

		// Token: 0x04000887 RID: 2183
		internal const string TreeView_TreeNodeDataBound = "TreeView_TreeNodeDataBound";

		// Token: 0x04000888 RID: 2184
		internal const string TreeView_TreeNodePopulate = "TreeView_TreeNodePopulate";

		// Token: 0x04000889 RID: 2185
		internal const string ValidationSummary_DisplayMode = "ValidationSummary_DisplayMode";

		// Token: 0x0400088A RID: 2186
		internal const string ValidationSummary_HeaderText = "ValidationSummary_HeaderText";

		// Token: 0x0400088B RID: 2187
		internal const string ValidationSummary_ShowMessageBox = "ValidationSummary_ShowMessageBox";

		// Token: 0x0400088C RID: 2188
		internal const string ValidationSummary_ShowSummary = "ValidationSummary_ShowSummary";

		// Token: 0x0400088D RID: 2189
		internal const string ValidationSummary_EnableClientScript = "ValidationSummary_EnableClientScript";

		// Token: 0x0400088E RID: 2190
		internal const string ValidationSummary_ValidationGroup = "ValidationSummary_ValidationGroup";

		// Token: 0x0400088F RID: 2191
		internal const string PostBackControl_ValidationGroup = "PostBackControl_ValidationGroup";

		// Token: 0x04000890 RID: 2192
		internal const string AutoPostBackControl_CausesValidation = "AutoPostBackControl_CausesValidation";

		// Token: 0x04000891 RID: 2193
		internal const string Calendar_Caption = "Calendar_Caption";

		// Token: 0x04000892 RID: 2194
		internal const string Calendar_CellPadding = "Calendar_CellPadding";

		// Token: 0x04000893 RID: 2195
		internal const string Calendar_CellSpacing = "Calendar_CellSpacing";

		// Token: 0x04000894 RID: 2196
		internal const string Calendar_DayHeaderStyle = "Calendar_DayHeaderStyle";

		// Token: 0x04000895 RID: 2197
		internal const string Calendar_DayNameFormat = "Calendar_DayNameFormat";

		// Token: 0x04000896 RID: 2198
		internal const string Calendar_DayStyle = "Calendar_DayStyle";

		// Token: 0x04000897 RID: 2199
		internal const string Calendar_FirstDayOfWeek = "Calendar_FirstDayOfWeek";

		// Token: 0x04000898 RID: 2200
		internal const string Calendar_NextMonthText = "Calendar_NextMonthText";

		// Token: 0x04000899 RID: 2201
		internal const string Calendar_NextPrevFormat = "Calendar_NextPrevFormat";

		// Token: 0x0400089A RID: 2202
		internal const string Calendar_NextPrevStyle = "Calendar_NextPrevStyle";

		// Token: 0x0400089B RID: 2203
		internal const string Calendar_OtherMonthDayStyle = "Calendar_OtherMonthDayStyle";

		// Token: 0x0400089C RID: 2204
		internal const string Calendar_PrevMonthText = "Calendar_PrevMonthText";

		// Token: 0x0400089D RID: 2205
		internal const string Calendar_SelectedDate = "Calendar_SelectedDate";

		// Token: 0x0400089E RID: 2206
		internal const string Calendar_SelectedDates = "Calendar_SelectedDates";

		// Token: 0x0400089F RID: 2207
		internal const string Calendar_SelectedDayStyle = "Calendar_SelectedDayStyle";

		// Token: 0x040008A0 RID: 2208
		internal const string Calendar_SelectionMode = "Calendar_SelectionMode";

		// Token: 0x040008A1 RID: 2209
		internal const string Calendar_SelectMonthText = "Calendar_SelectMonthText";

		// Token: 0x040008A2 RID: 2210
		internal const string Calendar_SelectorStyle = "Calendar_SelectorStyle";

		// Token: 0x040008A3 RID: 2211
		internal const string Calendar_SelectWeekText = "Calendar_SelectWeekText";

		// Token: 0x040008A4 RID: 2212
		internal const string Calendar_ShowDayHeader = "Calendar_ShowDayHeader";

		// Token: 0x040008A5 RID: 2213
		internal const string Calendar_ShowGridLines = "Calendar_ShowGridLines";

		// Token: 0x040008A6 RID: 2214
		internal const string Calendar_ShowNextPrevMonth = "Calendar_ShowNextPrevMonth";

		// Token: 0x040008A7 RID: 2215
		internal const string Calendar_ShowTitle = "Calendar_ShowTitle";

		// Token: 0x040008A8 RID: 2216
		internal const string Calendar_TitleFormat = "Calendar_TitleFormat";

		// Token: 0x040008A9 RID: 2217
		internal const string Calendar_TitleStyle = "Calendar_TitleStyle";

		// Token: 0x040008AA RID: 2218
		internal const string Calendar_TodayDayStyle = "Calendar_TodayDayStyle";

		// Token: 0x040008AB RID: 2219
		internal const string Calendar_TodaysDate = "Calendar_TodaysDate";

		// Token: 0x040008AC RID: 2220
		internal const string Calendar_VisibleDate = "Calendar_VisibleDate";

		// Token: 0x040008AD RID: 2221
		internal const string Calendar_WeekendDayStyle = "Calendar_WeekendDayStyle";

		// Token: 0x040008AE RID: 2222
		internal const string Calendar_OnDayRender = "Calendar_OnDayRender";

		// Token: 0x040008AF RID: 2223
		internal const string Calendar_OnSelectionChanged = "Calendar_OnSelectionChanged";

		// Token: 0x040008B0 RID: 2224
		internal const string Calendar_OnVisibleMonthChanged = "Calendar_OnVisibleMonthChanged";

		// Token: 0x040008B1 RID: 2225
		internal const string Calendar_TitleText = "Calendar_TitleText";

		// Token: 0x040008B2 RID: 2226
		internal const string Calendar_PreviousMonthTitle = "Calendar_PreviousMonthTitle";

		// Token: 0x040008B3 RID: 2227
		internal const string Calendar_NextMonthTitle = "Calendar_NextMonthTitle";

		// Token: 0x040008B4 RID: 2228
		internal const string Calendar_SelectMonthTitle = "Calendar_SelectMonthTitle";

		// Token: 0x040008B5 RID: 2229
		internal const string Calendar_SelectWeekTitle = "Calendar_SelectWeekTitle";

		// Token: 0x040008B6 RID: 2230
		internal const string View_Activate = "View_Activate";

		// Token: 0x040008B7 RID: 2231
		internal const string View_Deactivate = "View_Deactivate";

		// Token: 0x040008B8 RID: 2232
		internal const string ViewCollection_must_contain_view = "ViewCollection_must_contain_view";

		// Token: 0x040008B9 RID: 2233
		internal const string WebControl_AccessKey = "WebControl_AccessKey";

		// Token: 0x040008BA RID: 2234
		internal const string WebControl_InvalidAccessKey = "WebControl_InvalidAccessKey";

		// Token: 0x040008BB RID: 2235
		internal const string WebControl_Attributes = "WebControl_Attributes";

		// Token: 0x040008BC RID: 2236
		internal const string WebControl_BackColor = "WebControl_BackColor";

		// Token: 0x040008BD RID: 2237
		internal const string WebControl_BorderColor = "WebControl_BorderColor";

		// Token: 0x040008BE RID: 2238
		internal const string WebControl_BorderWidth = "WebControl_BorderWidth";

		// Token: 0x040008BF RID: 2239
		internal const string WebControl_BorderStyle = "WebControl_BorderStyle";

		// Token: 0x040008C0 RID: 2240
		internal const string WebControl_CSSClassName = "WebControl_CSSClassName";

		// Token: 0x040008C1 RID: 2241
		internal const string WebControl_ControlStyle = "WebControl_ControlStyle";

		// Token: 0x040008C2 RID: 2242
		internal const string WebControl_ControlStyleCreated = "WebControl_ControlStyleCreated";

		// Token: 0x040008C3 RID: 2243
		internal const string WebControl_Enabled = "WebControl_Enabled";

		// Token: 0x040008C4 RID: 2244
		internal const string WebControl_Font = "WebControl_Font";

		// Token: 0x040008C5 RID: 2245
		internal const string WebControl_ForeColor = "WebControl_ForeColor";

		// Token: 0x040008C6 RID: 2246
		internal const string WebControl_Height = "WebControl_Height";

		// Token: 0x040008C7 RID: 2247
		internal const string WebControl_Style = "WebControl_Style";

		// Token: 0x040008C8 RID: 2248
		internal const string WebControl_TabIndex = "WebControl_TabIndex";

		// Token: 0x040008C9 RID: 2249
		internal const string WebControl_Tooltip = "WebControl_Tooltip";

		// Token: 0x040008CA RID: 2250
		internal const string WebControl_Width = "WebControl_Width";

		// Token: 0x040008CB RID: 2251
		internal const string Wizard_ActiveStep = "Wizard_ActiveStep";

		// Token: 0x040008CC RID: 2252
		internal const string Wizard_ActiveStepIndex = "Wizard_ActiveStepIndex";

		// Token: 0x040008CD RID: 2253
		internal const string Wizard_ActiveStepIndex_out_of_range = "Wizard_ActiveStepIndex_out_of_range";

		// Token: 0x040008CE RID: 2254
		internal const string Wizard_CancelButtonClick = "Wizard_CancelButtonClick";

		// Token: 0x040008CF RID: 2255
		internal const string Wizard_CancelButtonImageUrl = "Wizard_CancelButtonImageUrl";

		// Token: 0x040008D0 RID: 2256
		internal const string Wizard_CancelButtonText = "Wizard_CancelButtonText";

		// Token: 0x040008D1 RID: 2257
		internal const string Wizard_CancelButtonType = "Wizard_CancelButtonType";

		// Token: 0x040008D2 RID: 2258
		internal const string Wizard_CancelButtonStyle = "Wizard_CancelButtonStyle";

		// Token: 0x040008D3 RID: 2259
		internal const string Wizard_CancelDestinationPageUrl = "Wizard_CancelDestinationPageUrl";

		// Token: 0x040008D4 RID: 2260
		internal const string Wizard_CellPadding = "Wizard_CellPadding";

		// Token: 0x040008D5 RID: 2261
		internal const string Wizard_CellSpacing = "Wizard_CellSpacing";

		// Token: 0x040008D6 RID: 2262
		internal const string Wizard_Default_CancelButtonText = "Wizard_Default_CancelButtonText";

		// Token: 0x040008D7 RID: 2263
		internal const string Wizard_DisplayCancelButton = "Wizard_DisplayCancelButton";

		// Token: 0x040008D8 RID: 2264
		internal const string Wizard_FinishDestinationPageUrl = "Wizard_FinishDestinationPageUrl";

		// Token: 0x040008D9 RID: 2265
		internal const string Wizard_FinishCompleteButtonStyle = "Wizard_FinishCompleteButtonStyle";

		// Token: 0x040008DA RID: 2266
		internal const string Wizard_FinishCompleteButtonText = "Wizard_FinishCompleteButtonText";

		// Token: 0x040008DB RID: 2267
		internal const string Wizard_FinishCompleteButtonType = "Wizard_FinishCompleteButtonType";

		// Token: 0x040008DC RID: 2268
		internal const string Wizard_FinishCompleteButtonImageUrl = "Wizard_FinishCompleteButtonImageUrl";

		// Token: 0x040008DD RID: 2269
		internal const string Wizard_FinishPreviousButtonStyle = "Wizard_FinishPreviousButtonStyle";

		// Token: 0x040008DE RID: 2270
		internal const string Wizard_FinishPreviousButtonText = "Wizard_FinishPreviousButtonText";

		// Token: 0x040008DF RID: 2271
		internal const string Wizard_FinishPreviousButtonType = "Wizard_FinishPreviousButtonType";

		// Token: 0x040008E0 RID: 2272
		internal const string Wizard_FinishPreviousButtonImageUrl = "Wizard_FinishPreviousButtonImageUrl";

		// Token: 0x040008E1 RID: 2273
		internal const string Wizard_FinishNavigationTemplate = "Wizard_FinishNavigationTemplate";

		// Token: 0x040008E2 RID: 2274
		internal const string Wizard_InvalidBubbleEvent = "Wizard_InvalidBubbleEvent";

		// Token: 0x040008E3 RID: 2275
		internal const string Wizard_NavigationButtonStyle = "Wizard_NavigationButtonStyle";

		// Token: 0x040008E4 RID: 2276
		internal const string Wizard_NavigationStyle = "Wizard_NavigationStyle";

		// Token: 0x040008E5 RID: 2277
		internal const string Wizard_StepNextButtonStyle = "Wizard_StepNextButtonStyle";

		// Token: 0x040008E6 RID: 2278
		internal const string Wizard_StepNextButtonText = "Wizard_StepNextButtonText";

		// Token: 0x040008E7 RID: 2279
		internal const string Wizard_StepNextButtonType = "Wizard_StepNextButtonType";

		// Token: 0x040008E8 RID: 2280
		internal const string Wizard_StepNextButtonImageUrl = "Wizard_StepNextButtonImageUrl";

		// Token: 0x040008E9 RID: 2281
		internal const string Wizard_StepPreviousButtonStyle = "Wizard_StepPreviousButtonStyle";

		// Token: 0x040008EA RID: 2282
		internal const string Wizard_StepPreviousButtonText = "Wizard_StepPreviousButtonText";

		// Token: 0x040008EB RID: 2283
		internal const string Wizard_StepPreviousButtonType = "Wizard_StepPreviousButtonType";

		// Token: 0x040008EC RID: 2284
		internal const string Wizard_StepPreviousButtonImageUrl = "Wizard_StepPreviousButtonImageUrl";

		// Token: 0x040008ED RID: 2285
		internal const string Wizard_SideBarButtonStyle = "Wizard_SideBarButtonStyle";

		// Token: 0x040008EE RID: 2286
		internal const string Wizard_DisplaySideBar = "Wizard_DisplaySideBar";

		// Token: 0x040008EF RID: 2287
		internal const string Wizard_SideBarStyle = "Wizard_SideBarStyle";

		// Token: 0x040008F0 RID: 2288
		internal const string Wizard_SideBarTemplate = "Wizard_SideBarTemplate";

		// Token: 0x040008F1 RID: 2289
		internal const string Wizard_StartNavigationTemplate = "Wizard_StartNavigationTemplate";

		// Token: 0x040008F2 RID: 2290
		internal const string Wizard_StartNextButtonStyle = "Wizard_StartNextButtonStyle";

		// Token: 0x040008F3 RID: 2291
		internal const string Wizard_StartNextButtonText = "Wizard_StartNextButtonText";

		// Token: 0x040008F4 RID: 2292
		internal const string Wizard_StartNextButtonType = "Wizard_StartNextButtonType";

		// Token: 0x040008F5 RID: 2293
		internal const string Wizard_StartNextButtonImageUrl = "Wizard_StartNextButtonImageUrl";

		// Token: 0x040008F6 RID: 2294
		internal const string Wizard_Step_Not_In_Wizard = "Wizard_Step_Not_In_Wizard";

		// Token: 0x040008F7 RID: 2295
		internal const string Wizard_StepNavigationTemplate = "Wizard_StepNavigationTemplate";

		// Token: 0x040008F8 RID: 2296
		internal const string Wizard_StepStyle = "Wizard_StepStyle";

		// Token: 0x040008F9 RID: 2297
		internal const string Wizard_WizardSteps = "Wizard_WizardSteps";

		// Token: 0x040008FA RID: 2298
		internal const string Wizard_HeaderText = "Wizard_HeaderText";

		// Token: 0x040008FB RID: 2299
		internal const string Wizard_Default_SkipToContentText = "Wizard_Default_SkipToContentText";

		// Token: 0x040008FC RID: 2300
		internal const string Wizard_ActiveStepChanged = "Wizard_ActiveStepChanged";

		// Token: 0x040008FD RID: 2301
		internal const string Wizard_FinishButtonClick = "Wizard_FinishButtonClick";

		// Token: 0x040008FE RID: 2302
		internal const string Wizard_NextButtonClick = "Wizard_NextButtonClick";

		// Token: 0x040008FF RID: 2303
		internal const string Wizard_PreviousButtonClick = "Wizard_PreviousButtonClick";

		// Token: 0x04000900 RID: 2304
		internal const string Wizard_SideBarButtonClick = "Wizard_SideBarButtonClick";

		// Token: 0x04000901 RID: 2305
		internal const string Wizard_Default_StepPreviousButtonText = "Wizard_Default_StepPreviousButtonText";

		// Token: 0x04000902 RID: 2306
		internal const string Wizard_Default_StepNextButtonText = "Wizard_Default_StepNextButtonText";

		// Token: 0x04000903 RID: 2307
		internal const string Wizard_Default_FinishButtonText = "Wizard_Default_FinishButtonText";

		// Token: 0x04000904 RID: 2308
		internal const string Wizard_SideBar_Button_Not_Found = "Wizard_SideBar_Button_Not_Found";

		// Token: 0x04000905 RID: 2309
		internal const string Wizard_DataList_Not_Found = "Wizard_DataList_Not_Found";

		// Token: 0x04000906 RID: 2310
		internal const string Wizard_Cannot_Modify_ControlCollection = "Wizard_Cannot_Modify_ControlCollection";

		// Token: 0x04000907 RID: 2311
		internal const string Wizard_WizardStepOnly = "Wizard_WizardStepOnly";

		// Token: 0x04000908 RID: 2312
		internal const string WizardStep_AllowReturn = "WizardStep_AllowReturn";

		// Token: 0x04000909 RID: 2313
		internal const string WizardStep_Name = "WizardStep_Name";

		// Token: 0x0400090A RID: 2314
		internal const string WizardStep_Title = "WizardStep_Title";

		// Token: 0x0400090B RID: 2315
		internal const string WizardStep_StepType = "WizardStep_StepType";

		// Token: 0x0400090C RID: 2316
		internal const string WizardStep_WrongContainment = "WizardStep_WrongContainment";

		// Token: 0x0400090D RID: 2317
		internal const string Xml_DocumentContent = "Xml_DocumentContent";

		// Token: 0x0400090E RID: 2318
		internal const string Xml_DocumentSource = "Xml_DocumentSource";

		// Token: 0x0400090F RID: 2319
		internal const string Xml_TransformSource = "Xml_TransformSource";

		// Token: 0x04000910 RID: 2320
		internal const string Xml_Document = "Xml_Document";

		// Token: 0x04000911 RID: 2321
		internal const string Xml_Transform = "Xml_Transform";

		// Token: 0x04000912 RID: 2322
		internal const string Xml_TransformArgumentList = "Xml_TransformArgumentList";

		// Token: 0x04000913 RID: 2323
		internal const string Xml_XPathNavigator = "Xml_XPathNavigator";

		// Token: 0x04000914 RID: 2324
		internal const string XmlDataSource_Data = "XmlDataSource_Data";

		// Token: 0x04000915 RID: 2325
		internal const string XmlDataSource_DataFile = "XmlDataSource_DataFile";

		// Token: 0x04000916 RID: 2326
		internal const string XmlDataSource_Transform = "XmlDataSource_Transform";

		// Token: 0x04000917 RID: 2327
		internal const string XmlDataSource_TransformFile = "XmlDataSource_TransformFile";

		// Token: 0x04000918 RID: 2328
		internal const string XmlDataSource_XPath = "XmlDataSource_XPath";

		// Token: 0x04000919 RID: 2329
		internal const string XmlDataSource_Transforming = "XmlDataSource_Transforming";

		// Token: 0x0400091A RID: 2330
		internal const string AccessPersonalizationProvider_Description = "AccessPersonalizationProvider_Description";

		// Token: 0x0400091B RID: 2331
		internal const string AppearanceEditorPart_Title = "AppearanceEditorPart_Title";

		// Token: 0x0400091C RID: 2332
		internal const string AppearanceEditorPart_Height = "AppearanceEditorPart_Height";

		// Token: 0x0400091D RID: 2333
		internal const string AppearanceEditorPart_Width = "AppearanceEditorPart_Width";

		// Token: 0x0400091E RID: 2334
		internal const string AppearanceEditorPart_ChromeType = "AppearanceEditorPart_ChromeType";

		// Token: 0x0400091F RID: 2335
		internal const string AppearanceEditorPart_Hidden = "AppearanceEditorPart_Hidden";

		// Token: 0x04000920 RID: 2336
		internal const string AppearanceEditorPart_Direction = "AppearanceEditorPart_Direction";

		// Token: 0x04000921 RID: 2337
		internal const string AppearanceEditorPart_PartTitle = "AppearanceEditorPart_PartTitle";

		// Token: 0x04000922 RID: 2338
		internal const string AppearanceEditorPart_Pixels = "AppearanceEditorPart_Pixels";

		// Token: 0x04000923 RID: 2339
		internal const string AppearanceEditorPart_Points = "AppearanceEditorPart_Points";

		// Token: 0x04000924 RID: 2340
		internal const string AppearanceEditorPart_Picas = "AppearanceEditorPart_Picas";

		// Token: 0x04000925 RID: 2341
		internal const string AppearanceEditorPart_Inches = "AppearanceEditorPart_Inches";

		// Token: 0x04000926 RID: 2342
		internal const string AppearanceEditorPart_Millimeters = "AppearanceEditorPart_Millimeters";

		// Token: 0x04000927 RID: 2343
		internal const string AppearanceEditorPart_Centimeters = "AppearanceEditorPart_Centimeters";

		// Token: 0x04000928 RID: 2344
		internal const string AppearanceEditorPart_Percent = "AppearanceEditorPart_Percent";

		// Token: 0x04000929 RID: 2345
		internal const string AppearanceEditorPart_Em = "AppearanceEditorPart_Em";

		// Token: 0x0400092A RID: 2346
		internal const string AppearanceEditorPart_Ex = "AppearanceEditorPart_Ex";

		// Token: 0x0400092B RID: 2347
		internal const string BehaviorEditorPart_AllowClose = "BehaviorEditorPart_AllowClose";

		// Token: 0x0400092C RID: 2348
		internal const string BehaviorEditorPart_AllowConnect = "BehaviorEditorPart_AllowConnect";

		// Token: 0x0400092D RID: 2349
		internal const string BehaviorEditorPart_AllowHide = "BehaviorEditorPart_AllowHide";

		// Token: 0x0400092E RID: 2350
		internal const string BehaviorEditorPart_AllowMinimize = "BehaviorEditorPart_AllowMinimize";

		// Token: 0x0400092F RID: 2351
		internal const string BehaviorEditorPart_AllowZoneChange = "BehaviorEditorPart_AllowZoneChange";

		// Token: 0x04000930 RID: 2352
		internal const string BehaviorEditorPart_ExportMode = "BehaviorEditorPart_ExportMode";

		// Token: 0x04000931 RID: 2353
		internal const string BehaviorEditorPart_ExportModeNone = "BehaviorEditorPart_ExportModeNone";

		// Token: 0x04000932 RID: 2354
		internal const string BehaviorEditorPart_ExportModeAll = "BehaviorEditorPart_ExportModeAll";

		// Token: 0x04000933 RID: 2355
		internal const string BehaviorEditorPart_ExportModeNonSensitiveData = "BehaviorEditorPart_ExportModeNonSensitiveData";

		// Token: 0x04000934 RID: 2356
		internal const string BehaviorEditorPart_HelpMode = "BehaviorEditorPart_HelpMode";

		// Token: 0x04000935 RID: 2357
		internal const string BehaviorEditorPart_HelpModeModal = "BehaviorEditorPart_HelpModeModal";

		// Token: 0x04000936 RID: 2358
		internal const string BehaviorEditorPart_HelpModeModeless = "BehaviorEditorPart_HelpModeModeless";

		// Token: 0x04000937 RID: 2359
		internal const string BehaviorEditorPart_HelpModeNavigate = "BehaviorEditorPart_HelpModeNavigate";

		// Token: 0x04000938 RID: 2360
		internal const string BehaviorEditorPart_Description = "BehaviorEditorPart_Description";

		// Token: 0x04000939 RID: 2361
		internal const string BehaviorEditorPart_TitleLink = "BehaviorEditorPart_TitleLink";

		// Token: 0x0400093A RID: 2362
		internal const string BehaviorEditorPart_TitleIconImageLink = "BehaviorEditorPart_TitleIconImageLink";

		// Token: 0x0400093B RID: 2363
		internal const string BehaviorEditorPart_CatalogIconImageLink = "BehaviorEditorPart_CatalogIconImageLink";

		// Token: 0x0400093C RID: 2364
		internal const string BehaviorEditorPart_HelpLink = "BehaviorEditorPart_HelpLink";

		// Token: 0x0400093D RID: 2365
		internal const string BehaviorEditorPart_ImportErrorMessage = "BehaviorEditorPart_ImportErrorMessage";

		// Token: 0x0400093E RID: 2366
		internal const string BehaviorEditorPart_AuthorizationFilter = "BehaviorEditorPart_AuthorizationFilter";

		// Token: 0x0400093F RID: 2367
		internal const string BehaviorEditorPart_AllowEdit = "BehaviorEditorPart_AllowEdit";

		// Token: 0x04000940 RID: 2368
		internal const string BehaviorEditorPart_PartTitle = "BehaviorEditorPart_PartTitle";

		// Token: 0x04000941 RID: 2369
		internal const string BlobPersonalizationState_CantApply = "BlobPersonalizationState_CantApply";

		// Token: 0x04000942 RID: 2370
		internal const string BlobPersonalizationState_CantExtract = "BlobPersonalizationState_CantExtract";

		// Token: 0x04000943 RID: 2371
		internal const string BlobPersonalizationState_DeserializeError = "BlobPersonalizationState_DeserializeError";

		// Token: 0x04000944 RID: 2372
		internal const string BlobPersonalizationState_NotApplied = "BlobPersonalizationState_NotApplied";

		// Token: 0x04000945 RID: 2373
		internal const string BlobPersonalizationState_NotLoaded = "BlobPersonalizationState_NotLoaded";

		// Token: 0x04000946 RID: 2374
		internal const string CatalogPart_MustBeInZone = "CatalogPart_MustBeInZone";

		// Token: 0x04000947 RID: 2375
		internal const string CatalogPart_SampleWebPartTitle = "CatalogPart_SampleWebPartTitle";

		// Token: 0x04000948 RID: 2376
		internal const string CatalogPart_UnknownDescription = "CatalogPart_UnknownDescription";

		// Token: 0x04000949 RID: 2377
		internal const string CatalogZone_OnlyCatalogParts = "CatalogZone_OnlyCatalogParts";

		// Token: 0x0400094A RID: 2378
		internal const string CatalogZoneBase_AddVerb = "CatalogZoneBase_AddVerb";

		// Token: 0x0400094B RID: 2379
		internal const string CatalogZoneBase_CloseVerb = "CatalogZoneBase_CloseVerb";

		// Token: 0x0400094C RID: 2380
		internal const string CatalogZoneBase_DefaultEmptyZoneText = "CatalogZoneBase_DefaultEmptyZoneText";

		// Token: 0x0400094D RID: 2381
		internal const string CatalogZoneBase_DefaultSelectTargetZoneText = "CatalogZoneBase_DefaultSelectTargetZoneText";

		// Token: 0x0400094E RID: 2382
		internal const string CatalogZoneBase_HeaderText = "CatalogZoneBase_HeaderText";

		// Token: 0x0400094F RID: 2383
		internal const string CatalogZoneBase_InstructionText = "CatalogZoneBase_InstructionText";

		// Token: 0x04000950 RID: 2384
		internal const string CatalogZoneBase_NoCatalogPartID = "CatalogZoneBase_NoCatalogPartID";

		// Token: 0x04000951 RID: 2385
		internal const string CatalogZoneBase_PartLinkStyle = "CatalogZoneBase_PartLinkStyle";

		// Token: 0x04000952 RID: 2386
		internal const string CatalogZoneBase_SelectCatalogPart = "CatalogZoneBase_SelectCatalogPart";

		// Token: 0x04000953 RID: 2387
		internal const string CatalogZoneBase_SelectedCatalogPartID = "CatalogZoneBase_SelectedCatalogPartID";

		// Token: 0x04000954 RID: 2388
		internal const string CatalogZoneBase_SelectedPartLinkStyle = "CatalogZoneBase_SelectedPartLinkStyle";

		// Token: 0x04000955 RID: 2389
		internal const string CatalogZoneBase_SelectTargetZoneText = "CatalogZoneBase_SelectTargetZoneText";

		// Token: 0x04000956 RID: 2390
		internal const string CatalogZoneBase_ShowCatalogIcons = "CatalogZoneBase_ShowCatalogIcons";

		// Token: 0x04000957 RID: 2391
		internal const string ConnectionConsumerAttribute_InvalidConnectionPointType = "ConnectionConsumerAttribute_InvalidConnectionPointType";

		// Token: 0x04000958 RID: 2392
		internal const string ConnectionProviderAttribute_InvalidConnectionPointType = "ConnectionProviderAttribute_InvalidConnectionPointType";

		// Token: 0x04000959 RID: 2393
		internal const string ConnectionsZone_CancelVerb = "ConnectionsZone_CancelVerb";

		// Token: 0x0400095A RID: 2394
		internal const string ConnectionsZone_ConfigureConnectionTitle = "ConnectionsZone_ConfigureConnectionTitle";

		// Token: 0x0400095B RID: 2395
		internal const string ConnectionsZone_ConfigureConnectionTitleDescription = "ConnectionsZone_ConfigureConnectionTitleDescription";

		// Token: 0x0400095C RID: 2396
		internal const string ConnectionsZone_ConfigureVerb = "ConnectionsZone_ConfigureVerb";

		// Token: 0x0400095D RID: 2397
		internal const string ConnectionsZone_ConnectToConsumerInstructionText = "ConnectionsZone_ConnectToConsumerInstructionText";

		// Token: 0x0400095E RID: 2398
		internal const string ConnectionsZone_ConnectToConsumerInstructionTextDescription = "ConnectionsZone_ConnectToConsumerInstructionTextDescription";

		// Token: 0x0400095F RID: 2399
		internal const string ConnectionsZone_ConnectToConsumerText = "ConnectionsZone_ConnectToConsumerText";

		// Token: 0x04000960 RID: 2400
		internal const string ConnectionsZone_ConnectToConsumerTextDescription = "ConnectionsZone_ConnectToConsumerTextDescription";

		// Token: 0x04000961 RID: 2401
		internal const string ConnectionsZone_ConnectToConsumerTitle = "ConnectionsZone_ConnectToConsumerTitle";

		// Token: 0x04000962 RID: 2402
		internal const string ConnectionsZone_ConnectToConsumerTitleDescription = "ConnectionsZone_ConnectToConsumerTitleDescription";

		// Token: 0x04000963 RID: 2403
		internal const string ConnectionsZone_ConnectToProviderInstructionText = "ConnectionsZone_ConnectToProviderInstructionText";

		// Token: 0x04000964 RID: 2404
		internal const string ConnectionsZone_ConnectToProviderInstructionTextDescription = "ConnectionsZone_ConnectToProviderInstructionTextDescription";

		// Token: 0x04000965 RID: 2405
		internal const string ConnectionsZone_ConnectToProviderText = "ConnectionsZone_ConnectToProviderText";

		// Token: 0x04000966 RID: 2406
		internal const string ConnectionsZone_ConnectToProviderTextDescription = "ConnectionsZone_ConnectToProviderTextDescription";

		// Token: 0x04000967 RID: 2407
		internal const string ConnectionsZone_ConnectToProviderTitle = "ConnectionsZone_ConnectToProviderTitle";

		// Token: 0x04000968 RID: 2408
		internal const string ConnectionsZone_ConnectToProviderTitleDescription = "ConnectionsZone_ConnectToProviderTitleDescription";

		// Token: 0x04000969 RID: 2409
		internal const string ConnectionsZone_ConnectVerb = "ConnectionsZone_ConnectVerb";

		// Token: 0x0400096A RID: 2410
		internal const string ConnectionsZone_ConsumersInstructionText = "ConnectionsZone_ConsumersInstructionText";

		// Token: 0x0400096B RID: 2411
		internal const string ConnectionsZone_ConsumersInstructionTextDescription = "ConnectionsZone_ConsumersInstructionTextDescription";

		// Token: 0x0400096C RID: 2412
		internal const string ConnectionsZone_ConsumersTitle = "ConnectionsZone_ConsumersTitle";

		// Token: 0x0400096D RID: 2413
		internal const string ConnectionsZone_ConsumersTitleDescription = "ConnectionsZone_ConsumersTitleDescription";

		// Token: 0x0400096E RID: 2414
		internal const string ConnectionsZone_CloseVerb = "ConnectionsZone_CloseVerb";

		// Token: 0x0400096F RID: 2415
		internal const string ConnectionsZone_DisconnectVerb = "ConnectionsZone_DisconnectVerb";

		// Token: 0x04000970 RID: 2416
		internal const string ConnectionsZone_DisconnectInvalid = "ConnectionsZone_DisconnectInvalid";

		// Token: 0x04000971 RID: 2417
		internal const string ConnectionsZone_ErrorCantContinueConnectionCreation = "ConnectionsZone_ErrorCantContinueConnectionCreation";

		// Token: 0x04000972 RID: 2418
		internal const string ConnectionsZone_ErrorMessage = "ConnectionsZone_ErrorMessage";

		// Token: 0x04000973 RID: 2419
		internal const string ConnectionsZone_Get = "ConnectionsZone_Get";

		// Token: 0x04000974 RID: 2420
		internal const string ConnectionsZone_GetDescription = "ConnectionsZone_GetDescription";

		// Token: 0x04000975 RID: 2421
		internal const string ConnectionsZone_GetFromText = "ConnectionsZone_GetFromText";

		// Token: 0x04000976 RID: 2422
		internal const string ConnectionsZone_GetFromTextDescription = "ConnectionsZone_GetFromTextDescription";

		// Token: 0x04000977 RID: 2423
		internal const string ConnectionsZone_HeaderText = "ConnectionsZone_HeaderText";

		// Token: 0x04000978 RID: 2424
		internal const string ConnectionsZone_HeaderTextDescription = "ConnectionsZone_HeaderTextDescription";

		// Token: 0x04000979 RID: 2425
		internal const string ConnectionsZone_InstructionText = "ConnectionsZone_InstructionText";

		// Token: 0x0400097A RID: 2426
		internal const string ConnectionsZone_InstructionTextDescription = "ConnectionsZone_InstructionTextDescription";

		// Token: 0x0400097B RID: 2427
		internal const string ConnectionsZone_InstructionTitle = "ConnectionsZone_InstructionTitle";

		// Token: 0x0400097C RID: 2428
		internal const string ConnectionsZone_InstructionTitleDescription = "ConnectionsZone_InstructionTitleDescription";

		// Token: 0x0400097D RID: 2429
		internal const string ConnectionsZone_MustImplementITransformerConfigurationControl = "ConnectionsZone_MustImplementITransformerConfigurationControl";

		// Token: 0x0400097E RID: 2430
		internal const string ConnectionsZone_NoConsumers = "ConnectionsZone_NoConsumers";

		// Token: 0x0400097F RID: 2431
		internal const string ConnectionsZone_NoExistingConnectionTitle = "ConnectionsZone_NoExistingConnectionTitle";

		// Token: 0x04000980 RID: 2432
		internal const string ConnectionsZone_NoExistingConnectionTitleDescription = "ConnectionsZone_NoExistingConnectionTitleDescription";

		// Token: 0x04000981 RID: 2433
		internal const string ConnectionsZone_NoExistingConnectionInstructionText = "ConnectionsZone_NoExistingConnectionInstructionText";

		// Token: 0x04000982 RID: 2434
		internal const string ConnectionsZone_NoExistingConnectionInstructionTextDescription = "ConnectionsZone_NoExistingConnectionInstructionTextDescription";

		// Token: 0x04000983 RID: 2435
		internal const string ConnectionsZone_NoProviders = "ConnectionsZone_NoProviders";

		// Token: 0x04000984 RID: 2436
		internal const string ConnectionsZone_ProvidersInstructionText = "ConnectionsZone_ProvidersInstructionText";

		// Token: 0x04000985 RID: 2437
		internal const string ConnectionsZone_ProvidersInstructionTextDescription = "ConnectionsZone_ProvidersInstructionTextDescription";

		// Token: 0x04000986 RID: 2438
		internal const string ConnectionsZone_ProvidersTitle = "ConnectionsZone_ProvidersTitle";

		// Token: 0x04000987 RID: 2439
		internal const string ConnectionsZone_ProvidersTitleDescription = "ConnectionsZone_ProvidersTitleDescription";

		// Token: 0x04000988 RID: 2440
		internal const string ConnectionsZone_SendText = "ConnectionsZone_SendText";

		// Token: 0x04000989 RID: 2441
		internal const string ConnectionsZone_SendTextDescription = "ConnectionsZone_SendTextDescription";

		// Token: 0x0400098A RID: 2442
		internal const string ConnectionsZone_SendToText = "ConnectionsZone_SendToText";

		// Token: 0x0400098B RID: 2443
		internal const string ConnectionsZone_SendToTextDescription = "ConnectionsZone_SendToTextDescription";

		// Token: 0x0400098C RID: 2444
		internal const string ConnectionsZone_WarningConnectionDisabled = "ConnectionsZone_WarningConnectionDisabled";

		// Token: 0x0400098D RID: 2445
		internal const string ConnectionsZone_WarningMessage = "ConnectionsZone_WarningMessage";

		// Token: 0x0400098E RID: 2446
		internal const string ConnectionPoint_InvalidControlType = "ConnectionPoint_InvalidControlType";

		// Token: 0x0400098F RID: 2447
		internal const string ContentDirection_NotSet = "ContentDirection_NotSet";

		// Token: 0x04000990 RID: 2448
		internal const string ContentDirection_LeftToRight = "ContentDirection_LeftToRight";

		// Token: 0x04000991 RID: 2449
		internal const string ContentDirection_RightToLeft = "ContentDirection_RightToLeft";

		// Token: 0x04000992 RID: 2450
		internal const string DeclarativeCatalogPart_PartTitle = "DeclarativeCatalogPart_PartTitle";

		// Token: 0x04000993 RID: 2451
		internal const string DeclarativeCatlaogPart_WebPartsListUserControlPath = "DeclarativeCatlaogPart_WebPartsListUserControlPath";

		// Token: 0x04000994 RID: 2452
		internal const string EditorPart_MustBeInZone = "EditorPart_MustBeInZone";

		// Token: 0x04000995 RID: 2453
		internal const string EditorPart_ErrorBadUrl = "EditorPart_ErrorBadUrl";

		// Token: 0x04000996 RID: 2454
		internal const string EditorPart_ErrorConvertingProperty = "EditorPart_ErrorConvertingProperty";

		// Token: 0x04000997 RID: 2455
		internal const string EditorPart_ErrorConvertingPropertyWithType = "EditorPart_ErrorConvertingPropertyWithType";

		// Token: 0x04000998 RID: 2456
		internal const string EditorPart_ErrorSettingProperty = "EditorPart_ErrorSettingProperty";

		// Token: 0x04000999 RID: 2457
		internal const string EditorPart_ErrorSettingPropertyWithExceptionMessage = "EditorPart_ErrorSettingPropertyWithExceptionMessage";

		// Token: 0x0400099A RID: 2458
		internal const string EditorPart_PropertyMaxValue = "EditorPart_PropertyMaxValue";

		// Token: 0x0400099B RID: 2459
		internal const string EditorPart_PropertyMinValue = "EditorPart_PropertyMinValue";

		// Token: 0x0400099C RID: 2460
		internal const string EditorPart_PropertyMustBeDecimal = "EditorPart_PropertyMustBeDecimal";

		// Token: 0x0400099D RID: 2461
		internal const string EditorPart_PropertyMustBeInteger = "EditorPart_PropertyMustBeInteger";

		// Token: 0x0400099E RID: 2462
		internal const string EditorZone_OnlyEditorParts = "EditorZone_OnlyEditorParts";

		// Token: 0x0400099F RID: 2463
		internal const string EditorZoneBase_ApplyVerb = "EditorZoneBase_ApplyVerb";

		// Token: 0x040009A0 RID: 2464
		internal const string EditorZoneBase_CancelVerb = "EditorZoneBase_CancelVerb";

		// Token: 0x040009A1 RID: 2465
		internal const string EditorZoneBase_DefaultEmptyZoneText = "EditorZoneBase_DefaultEmptyZoneText";

		// Token: 0x040009A2 RID: 2466
		internal const string EditorZoneBase_DefaultErrorText = "EditorZoneBase_DefaultErrorText";

		// Token: 0x040009A3 RID: 2467
		internal const string EditorZoneBase_DefaultHeaderText = "EditorZoneBase_DefaultHeaderText";

		// Token: 0x040009A4 RID: 2468
		internal const string EditorZoneBase_DefaultInstructionText = "EditorZoneBase_DefaultInstructionText";

		// Token: 0x040009A5 RID: 2469
		internal const string EditorZoneBase_ErrorText = "EditorZoneBase_ErrorText";

		// Token: 0x040009A6 RID: 2470
		internal const string EditorZoneBase_NoEditorPartID = "EditorZoneBase_NoEditorPartID";

		// Token: 0x040009A7 RID: 2471
		internal const string EditorZoneBase_OKVerb = "EditorZoneBase_OKVerb";

		// Token: 0x040009A8 RID: 2472
		internal const string ErrorWebPart_ErrorText = "ErrorWebPart_ErrorText";

		// Token: 0x040009A9 RID: 2473
		internal const string GenericWebPart_CannotWrapWebPart = "GenericWebPart_CannotWrapWebPart";

		// Token: 0x040009AA RID: 2474
		internal const string GenericWebPart_CannotWrapOutputCachedControl = "GenericWebPart_CannotWrapOutputCachedControl";

		// Token: 0x040009AB RID: 2475
		internal const string GenericWebPart_NoID = "GenericWebPart_NoID";

		// Token: 0x040009AC RID: 2476
		internal const string GenericWebPart_CannotModify = "GenericWebPart_CannotModify";

		// Token: 0x040009AD RID: 2477
		internal const string GenericWebPart_ChildControlIsNull = "GenericWebPart_ChildControlIsNull";

		// Token: 0x040009AE RID: 2478
		internal const string ImportCatalogPart_PartTitle = "ImportCatalogPart_PartTitle";

		// Token: 0x040009AF RID: 2479
		internal const string ImportCatalogPart_Browse = "ImportCatalogPart_Browse";

		// Token: 0x040009B0 RID: 2480
		internal const string ImportCatalogPart_BrowseHelpText = "ImportCatalogPart_BrowseHelpText";

		// Token: 0x040009B1 RID: 2481
		internal const string ImportCatalogPart_Upload = "ImportCatalogPart_Upload";

		// Token: 0x040009B2 RID: 2482
		internal const string ImportCatalogPart_UploadHelpText = "ImportCatalogPart_UploadHelpText";

		// Token: 0x040009B3 RID: 2483
		internal const string ImportCatalogPart_UploadButton = "ImportCatalogPart_UploadButton";

		// Token: 0x040009B4 RID: 2484
		internal const string ImportCatalogPart_UploadButtonText = "ImportCatalogPart_UploadButtonText";

		// Token: 0x040009B5 RID: 2485
		internal const string ImportCatalogPart_ImportedPartLabel = "ImportCatalogPart_ImportedPartLabel";

		// Token: 0x040009B6 RID: 2486
		internal const string ImportCatalogPart_ImportedPartErrorLabel = "ImportCatalogPart_ImportedPartErrorLabel";

		// Token: 0x040009B7 RID: 2487
		internal const string ImportCatalogPart_PartImportErrorLabelText = "ImportCatalogPart_PartImportErrorLabelText";

		// Token: 0x040009B8 RID: 2488
		internal const string ImportCatalogPart_ImportedPartLabelText = "ImportCatalogPart_ImportedPartLabelText";

		// Token: 0x040009B9 RID: 2489
		internal const string ImportCatalogPart_NoFileName = "ImportCatalogPart_NoFileName";

		// Token: 0x040009BA RID: 2490
		internal const string LayoutEditorPart_ChromeState = "LayoutEditorPart_ChromeState";

		// Token: 0x040009BB RID: 2491
		internal const string LayoutEditorPart_Zone = "LayoutEditorPart_Zone";

		// Token: 0x040009BC RID: 2492
		internal const string LayoutEditorPart_ZoneIndex = "LayoutEditorPart_ZoneIndex";

		// Token: 0x040009BD RID: 2493
		internal const string LayoutEditorPart_PartTitle = "LayoutEditorPart_PartTitle";

		// Token: 0x040009BE RID: 2494
		internal const string PageCatalogPart_PartTitle = "PageCatalogPart_PartTitle";

		// Token: 0x040009BF RID: 2495
		internal const string Part_Description = "Part_Description";

		// Token: 0x040009C0 RID: 2496
		internal const string Part_ChromeState = "Part_ChromeState";

		// Token: 0x040009C1 RID: 2497
		internal const string Part_ChromeType = "Part_ChromeType";

		// Token: 0x040009C2 RID: 2498
		internal const string Part_Title = "Part_Title";

		// Token: 0x040009C3 RID: 2499
		internal const string Part_Unknown = "Part_Unknown";

		// Token: 0x040009C4 RID: 2500
		internal const string Part_Untitled = "Part_Untitled";

		// Token: 0x040009C5 RID: 2501
		internal const string PartChromeState_Normal = "PartChromeState_Normal";

		// Token: 0x040009C6 RID: 2502
		internal const string PartChromeState_Minimized = "PartChromeState_Minimized";

		// Token: 0x040009C7 RID: 2503
		internal const string PartChromeType_Default = "PartChromeType_Default";

		// Token: 0x040009C8 RID: 2504
		internal const string PartChromeType_TitleAndBorder = "PartChromeType_TitleAndBorder";

		// Token: 0x040009C9 RID: 2505
		internal const string PartChromeType_TitleOnly = "PartChromeType_TitleOnly";

		// Token: 0x040009CA RID: 2506
		internal const string PartChromeType_BorderOnly = "PartChromeType_BorderOnly";

		// Token: 0x040009CB RID: 2507
		internal const string PartChromeType_None = "PartChromeType_None";

		// Token: 0x040009CC RID: 2508
		internal const string PersonalizableTypeEntry_InvalidProperty = "PersonalizableTypeEntry_InvalidProperty";

		// Token: 0x040009CD RID: 2509
		internal const string PersonalizationDictionary_MustBeTypeString = "PersonalizationDictionary_MustBeTypeString";

		// Token: 0x040009CE RID: 2510
		internal const string PersonalizationDictionary_MustBeTypePersonalizationEntry = "PersonalizationDictionary_MustBeTypePersonalizationEntry";

		// Token: 0x040009CF RID: 2511
		internal const string PersonalizationDictionary_MustBeTypeDictionaryEntryArray = "PersonalizationDictionary_MustBeTypeDictionaryEntryArray";

		// Token: 0x040009D0 RID: 2512
		internal const string PersonalizationProvider_ApplicationNameExceedMaxLength = "PersonalizationProvider_ApplicationNameExceedMaxLength";

		// Token: 0x040009D1 RID: 2513
		internal const string PersonalizationProvider_BadConnection = "PersonalizationProvider_BadConnection";

		// Token: 0x040009D2 RID: 2514
		internal const string PersonalizationProvider_CantAccess = "PersonalizationProvider_CantAccess";

		// Token: 0x040009D3 RID: 2515
		internal const string PersonalizationProvider_NoConnection = "PersonalizationProvider_NoConnection";

		// Token: 0x040009D4 RID: 2516
		internal const string PersonalizationProvider_UnknownProp = "PersonalizationProvider_UnknownProp";

		// Token: 0x040009D5 RID: 2517
		internal const string PersonalizationProvider_WrongType = "PersonalizationProvider_WrongType";

		// Token: 0x040009D6 RID: 2518
		internal const string PropertyGridEditorPart_PartTitle = "PropertyGridEditorPart_PartTitle";

		// Token: 0x040009D7 RID: 2519
		internal const string PropertyGridEditorPart_DesignModeWebPart_BoolProperty = "PropertyGridEditorPart_DesignModeWebPart_BoolProperty";

		// Token: 0x040009D8 RID: 2520
		internal const string PropertyGridEditorPart_DesignModeWebPart_EnumProperty = "PropertyGridEditorPart_DesignModeWebPart_EnumProperty";

		// Token: 0x040009D9 RID: 2521
		internal const string PropertyGridEditorPart_DesignModeWebPart_StringProperty = "PropertyGridEditorPart_DesignModeWebPart_StringProperty";

		// Token: 0x040009DA RID: 2522
		internal const string ProxyWebPartConnectionCollection_ReadOnly = "ProxyWebPartConnectionCollection_ReadOnly";

		// Token: 0x040009DB RID: 2523
		internal const string RowToFieldTransformer_FieldName = "RowToFieldTransformer_FieldName";

		// Token: 0x040009DC RID: 2524
		internal const string RowToFieldTransformer_NoProviderSchema = "RowToFieldTransformer_NoProviderSchema";

		// Token: 0x040009DD RID: 2525
		internal const string RowToParametersTransformer_DifferentFieldNamesLength = "RowToParametersTransformer_DifferentFieldNamesLength";

		// Token: 0x040009DE RID: 2526
		internal const string RowToParametersTransformer_ConsumerFieldName = "RowToParametersTransformer_ConsumerFieldName";

		// Token: 0x040009DF RID: 2527
		internal const string RowToParametersTransformer_NoConsumerSchema = "RowToParametersTransformer_NoConsumerSchema";

		// Token: 0x040009E0 RID: 2528
		internal const string RowToParametersTransformer_ProviderFieldName = "RowToParametersTransformer_ProviderFieldName";

		// Token: 0x040009E1 RID: 2529
		internal const string RowToParametersTransformer_NoProviderSchema = "RowToParametersTransformer_NoProviderSchema";

		// Token: 0x040009E2 RID: 2530
		internal const string SqlPersonalizationProvider_Description = "SqlPersonalizationProvider_Description";

		// Token: 0x040009E3 RID: 2531
		internal const string ToolZone_CantSetVisible = "ToolZone_CantSetVisible";

		// Token: 0x040009E4 RID: 2532
		internal const string ToolZone_EditUIStyle = "ToolZone_EditUIStyle";

		// Token: 0x040009E5 RID: 2533
		internal const string ToolZone_HeaderCloseVerb = "ToolZone_HeaderCloseVerb";

		// Token: 0x040009E6 RID: 2534
		internal const string ToolZone_HeaderVerbStyle = "ToolZone_HeaderVerbStyle";

		// Token: 0x040009E7 RID: 2535
		internal const string ToolZone_InstructionText = "ToolZone_InstructionText";

		// Token: 0x040009E8 RID: 2536
		internal const string ToolZone_InstructionTextStyle = "ToolZone_InstructionTextStyle";

		// Token: 0x040009E9 RID: 2537
		internal const string ToolZone_LabelStyle = "ToolZone_LabelStyle";

		// Token: 0x040009EA RID: 2538
		internal const string ToolZone_DisplayModesReadOnly = "ToolZone_DisplayModesReadOnly";

		// Token: 0x040009EB RID: 2539
		internal const string WebPartTransformerAttribute_Missing = "WebPartTransformerAttribute_Missing";

		// Token: 0x040009EC RID: 2540
		internal const string WebPartTransformerAttribute_NotTransformer = "WebPartTransformerAttribute_NotTransformer";

		// Token: 0x040009ED RID: 2541
		internal const string WebPartTransformerAttribute_SameTypes = "WebPartTransformerAttribute_SameTypes";

		// Token: 0x040009EE RID: 2542
		internal const string WebPartTransformerCollection_NotEmpty = "WebPartTransformerCollection_NotEmpty";

		// Token: 0x040009EF RID: 2543
		internal const string WebPartTransformerCollection_ReadOnly = "WebPartTransformerCollection_ReadOnly";

		// Token: 0x040009F0 RID: 2544
		internal const string UnknownWebPart = "UnknownWebPart";

		// Token: 0x040009F1 RID: 2545
		internal const string WebPart_AllowClose = "WebPart_AllowClose";

		// Token: 0x040009F2 RID: 2546
		internal const string WebPart_AllowConnect = "WebPart_AllowConnect";

		// Token: 0x040009F3 RID: 2547
		internal const string WebPart_AllowEdit = "WebPart_AllowEdit";

		// Token: 0x040009F4 RID: 2548
		internal const string WebPart_AllowHide = "WebPart_AllowHide";

		// Token: 0x040009F5 RID: 2549
		internal const string WebPart_AllowMinimize = "WebPart_AllowMinimize";

		// Token: 0x040009F6 RID: 2550
		internal const string WebPart_AllowZoneChange = "WebPart_AllowZoneChange";

		// Token: 0x040009F7 RID: 2551
		internal const string WebPart_AuthorizationFilter = "WebPart_AuthorizationFilter";

		// Token: 0x040009F8 RID: 2552
		internal const string WebPart_BadUrl = "WebPart_BadUrl";

		// Token: 0x040009F9 RID: 2553
		internal const string WebPart_CatalogIconImageUrl = "WebPart_CatalogIconImageUrl";

		// Token: 0x040009FA RID: 2554
		internal const string WebPart_CantSetExportMode = "WebPart_CantSetExportMode";

		// Token: 0x040009FB RID: 2555
		internal const string WebPart_DefaultImportErrorMessage = "WebPart_DefaultImportErrorMessage";

		// Token: 0x040009FC RID: 2556
		internal const string WebPart_ErrorFormatString = "WebPart_ErrorFormatString";

		// Token: 0x040009FD RID: 2557
		internal const string WebPart_ExportMode = "WebPart_ExportMode";

		// Token: 0x040009FE RID: 2558
		internal const string WebPart_HelpMode = "WebPart_HelpMode";

		// Token: 0x040009FF RID: 2559
		internal const string WebPart_HelpUrl = "WebPart_HelpUrl";

		// Token: 0x04000A00 RID: 2560
		internal const string WebPart_Hidden = "WebPart_Hidden";

		// Token: 0x04000A01 RID: 2561
		internal const string WebPart_HiddenFormatString = "WebPart_HiddenFormatString";

		// Token: 0x04000A02 RID: 2562
		internal const string WebPart_ImportErrorInvalidVersion = "WebPart_ImportErrorInvalidVersion";

		// Token: 0x04000A03 RID: 2563
		internal const string WebPart_ImportErrorMessage = "WebPart_ImportErrorMessage";

		// Token: 0x04000A04 RID: 2564
		internal const string WebPart_ImportErrorNoVersion = "WebPart_ImportErrorNoVersion";

		// Token: 0x04000A05 RID: 2565
		internal const string WebPart_NonWebPart = "WebPart_NonWebPart";

		// Token: 0x04000A06 RID: 2566
		internal const string WebPart_NotStandalone = "WebPart_NotStandalone";

		// Token: 0x04000A07 RID: 2567
		internal const string WebPart_OnlyStandalone = "WebPart_OnlyStandalone";

		// Token: 0x04000A08 RID: 2568
		internal const string WebPart_SetZoneTemplateTooLate = "WebPart_SetZoneTemplateTooLate";

		// Token: 0x04000A09 RID: 2569
		internal const string WebPart_TitleIconImageUrl = "WebPart_TitleIconImageUrl";

		// Token: 0x04000A0A RID: 2570
		internal const string WebPart_TitleUrl = "WebPart_TitleUrl";

		// Token: 0x04000A0B RID: 2571
		internal const string WebPart_Collection_DuplicateID = "WebPart_Collection_DuplicateID";

		// Token: 0x04000A0C RID: 2572
		internal const string WebPartActionVerb_CantSetChecked = "WebPartActionVerb_CantSetChecked";

		// Token: 0x04000A0D RID: 2573
		internal const string WebPartBrowseModeVerb_Description = "WebPartBrowseModeVerb_Description";

		// Token: 0x04000A0E RID: 2574
		internal const string WebPartBrowseModeVerb_Text = "WebPartBrowseModeVerb_Text";

		// Token: 0x04000A0F RID: 2575
		internal const string WebPartCatalogAddVerb_Description = "WebPartCatalogAddVerb_Description";

		// Token: 0x04000A10 RID: 2576
		internal const string WebPartCatalogAddVerb_Text = "WebPartCatalogAddVerb_Text";

		// Token: 0x04000A11 RID: 2577
		internal const string WebPartCatalogCloseVerb_Description = "WebPartCatalogCloseVerb_Description";

		// Token: 0x04000A12 RID: 2578
		internal const string WebPartCatalogCloseVerb_Text = "WebPartCatalogCloseVerb_Text";

		// Token: 0x04000A13 RID: 2579
		internal const string WebPartCatalogModeVerb_Description = "WebPartCatalogModeVerb_Description";

		// Token: 0x04000A14 RID: 2580
		internal const string WebPartCatalogModeVerb_Text = "WebPartCatalogModeVerb_Text";

		// Token: 0x04000A15 RID: 2581
		internal const string WebPartChrome_ConfirmExportSensitive = "WebPartChrome_ConfirmExportSensitive";

		// Token: 0x04000A16 RID: 2582
		internal const string WebPartCloseVerb_Description = "WebPartCloseVerb_Description";

		// Token: 0x04000A17 RID: 2583
		internal const string WebPartCloseVerb_Text = "WebPartCloseVerb_Text";

		// Token: 0x04000A18 RID: 2584
		internal const string WebPartConnectVerb_Description = "WebPartConnectVerb_Description";

		// Token: 0x04000A19 RID: 2585
		internal const string WebPartConnectVerb_Text = "WebPartConnectVerb_Text";

		// Token: 0x04000A1A RID: 2586
		internal const string WebPartConnection_ConsumerIDNotSet = "WebPartConnection_ConsumerIDNotSet";

		// Token: 0x04000A1B RID: 2587
		internal const string WebPartConnection_ConsumerRequiresSecondaryInterfaces = "WebPartConnection_ConsumerRequiresSecondaryInterfaces";

		// Token: 0x04000A1C RID: 2588
		internal const string WebPartConnection_DisabledConnectionPoint = "WebPartConnection_DisabledConnectionPoint";

		// Token: 0x04000A1D RID: 2589
		internal const string WebPartConnection_Duplicate = "WebPartConnection_Duplicate";

		// Token: 0x04000A1E RID: 2590
		internal const string WebPartConnection_IncompatibleConsumerTransformer = "WebPartConnection_IncompatibleConsumerTransformer";

		// Token: 0x04000A1F RID: 2591
		internal const string WebPartConnection_IncompatibleConsumerTransformerWithType = "WebPartConnection_IncompatibleConsumerTransformerWithType";

		// Token: 0x04000A20 RID: 2592
		internal const string WebPartConnection_IncompatibleProviderTransformer = "WebPartConnection_IncompatibleProviderTransformer";

		// Token: 0x04000A21 RID: 2593
		internal const string WebPartConnection_IncompatibleProviderTransformerWithType = "WebPartConnection_IncompatibleProviderTransformerWithType";

		// Token: 0x04000A22 RID: 2594
		internal const string WebPartConnection_IncompatibleSecondaryInterfaces = "WebPartConnection_IncompatibleSecondaryInterfaces";

		// Token: 0x04000A23 RID: 2595
		internal const string WebPartConnection_NoCommonInterface = "WebPartConnection_NoCommonInterface";

		// Token: 0x04000A24 RID: 2596
		internal const string WebPartConnection_NoConsumer = "WebPartConnection_NoConsumer";

		// Token: 0x04000A25 RID: 2597
		internal const string WebPartConnection_NoConsumerConnectionPoint = "WebPartConnection_NoConsumerConnectionPoint";

		// Token: 0x04000A26 RID: 2598
		internal const string WebPartConnection_NoID = "WebPartConnection_NoID";

		// Token: 0x04000A27 RID: 2599
		internal const string WebPartConnection_NoProvider = "WebPartConnection_NoProvider";

		// Token: 0x04000A28 RID: 2600
		internal const string WebPartConnection_NoProviderConnectionPoint = "WebPartConnection_NoProviderConnectionPoint";

		// Token: 0x04000A29 RID: 2601
		internal const string WebPartConnection_ProviderIDNotSet = "WebPartConnection_ProviderIDNotSet";

		// Token: 0x04000A2A RID: 2602
		internal const string WebPartConnection_TransformerNotAvailable = "WebPartConnection_TransformerNotAvailable";

		// Token: 0x04000A2B RID: 2603
		internal const string WebPartConnection_TransformerNotAvailableWithType = "WebPartConnection_TransformerNotAvailableWithType";

		// Token: 0x04000A2C RID: 2604
		internal const string WebPartConnectionsCancelVerb_Description = "WebPartConnectionsCancelVerb_Description";

		// Token: 0x04000A2D RID: 2605
		internal const string WebPartConnectionsCancelVerb_Text = "WebPartConnectionsCancelVerb_Text";

		// Token: 0x04000A2E RID: 2606
		internal const string WebPartConnectionsCloseVerb_Description = "WebPartConnectionsCloseVerb_Description";

		// Token: 0x04000A2F RID: 2607
		internal const string WebPartConnectionsCloseVerb_Text = "WebPartConnectionsCloseVerb_Text";

		// Token: 0x04000A30 RID: 2608
		internal const string WebPartConnectionsConfigureVerb_Description = "WebPartConnectionsConfigureVerb_Description";

		// Token: 0x04000A31 RID: 2609
		internal const string WebPartConnectionsConfigureVerb_Text = "WebPartConnectionsConfigureVerb_Text";

		// Token: 0x04000A32 RID: 2610
		internal const string WebPartConnectionsConnectVerb_Description = "WebPartConnectionsConnectVerb_Description";

		// Token: 0x04000A33 RID: 2611
		internal const string WebPartConnectionsConnectVerb_Text = "WebPartConnectionsConnectVerb_Text";

		// Token: 0x04000A34 RID: 2612
		internal const string WebPartConnectionsDisconnectVerb_Description = "WebPartConnectionsDisconnectVerb_Description";

		// Token: 0x04000A35 RID: 2613
		internal const string WebPartConnectionsDisconnectVerb_Text = "WebPartConnectionsDisconnectVerb_Text";

		// Token: 0x04000A36 RID: 2614
		internal const string WebPartConnectModeVerb_Description = "WebPartConnectModeVerb_Description";

		// Token: 0x04000A37 RID: 2615
		internal const string WebPartConnectModeVerb_Text = "WebPartConnectModeVerb_Text";

		// Token: 0x04000A38 RID: 2616
		internal const string WebPartDeleteVerb_Description = "WebPartDeleteVerb_Description";

		// Token: 0x04000A39 RID: 2617
		internal const string WebPartDeleteVerb_Text = "WebPartDeleteVerb_Text";

		// Token: 0x04000A3A RID: 2618
		internal const string WebPartResetVerb_Description = "WebPartResetVerb_Description";

		// Token: 0x04000A3B RID: 2619
		internal const string WebPartResetVerb_Text = "WebPartResetVerb_Text";

		// Token: 0x04000A3C RID: 2620
		internal const string WebPartSharedScopeVerb_Description = "WebPartSharedScopeVerb_Description";

		// Token: 0x04000A3D RID: 2621
		internal const string WebPartSharedScopeVerb_Text = "WebPartSharedScopeVerb_Text";

		// Token: 0x04000A3E RID: 2622
		internal const string WebPartDesignModeVerb_Description = "WebPartDesignModeVerb_Description";

		// Token: 0x04000A3F RID: 2623
		internal const string WebPartDesignModeVerb_Text = "WebPartDesignModeVerb_Text";

		// Token: 0x04000A40 RID: 2624
		internal const string WebPartDisplayModeCollection_CantRemove = "WebPartDisplayModeCollection_CantRemove";

		// Token: 0x04000A41 RID: 2625
		internal const string WebPartDisplayModeCollection_CantSet = "WebPartDisplayModeCollection_CantSet";

		// Token: 0x04000A42 RID: 2626
		internal const string WebPartDisplayModeCollection_DuplicateName = "WebPartDisplayModeCollection_DuplicateName";

		// Token: 0x04000A43 RID: 2627
		internal const string WebPartEditModeVerb_Description = "WebPartEditModeVerb_Description";

		// Token: 0x04000A44 RID: 2628
		internal const string WebPartEditModeVerb_Text = "WebPartEditModeVerb_Text";

		// Token: 0x04000A45 RID: 2629
		internal const string WebPartEditorApplyVerb_Description = "WebPartEditorApplyVerb_Description";

		// Token: 0x04000A46 RID: 2630
		internal const string WebPartEditorApplyVerb_Text = "WebPartEditorApplyVerb_Text";

		// Token: 0x04000A47 RID: 2631
		internal const string WebPartEditorCancelVerb_Description = "WebPartEditorCancelVerb_Description";

		// Token: 0x04000A48 RID: 2632
		internal const string WebPartEditorCancelVerb_Text = "WebPartEditorCancelVerb_Text";

		// Token: 0x04000A49 RID: 2633
		internal const string WebPartEditorOKVerb_Description = "WebPartEditorOKVerb_Description";

		// Token: 0x04000A4A RID: 2634
		internal const string WebPartEditorOKVerb_Text = "WebPartEditorOKVerb_Text";

		// Token: 0x04000A4B RID: 2635
		internal const string WebPartEditVerb_Description = "WebPartEditVerb_Description";

		// Token: 0x04000A4C RID: 2636
		internal const string WebPartEditVerb_Text = "WebPartEditVerb_Text";

		// Token: 0x04000A4D RID: 2637
		internal const string WebPartExportHandler_InvalidArgument = "WebPartExportHandler_InvalidArgument";

		// Token: 0x04000A4E RID: 2638
		internal const string WebPartExportHandler_DisabledExportHandler = "WebPartExportHandler_DisabledExportHandler";

		// Token: 0x04000A4F RID: 2639
		internal const string WebPartExportVerb_Description = "WebPartExportVerb_Description";

		// Token: 0x04000A50 RID: 2640
		internal const string WebPartExportVerb_Text = "WebPartExportVerb_Text";

		// Token: 0x04000A51 RID: 2641
		internal const string WebPartHeaderCloseVerb_Description = "WebPartHeaderCloseVerb_Description";

		// Token: 0x04000A52 RID: 2642
		internal const string WebPartHeaderCloseVerb_Text = "WebPartHeaderCloseVerb_Text";

		// Token: 0x04000A53 RID: 2643
		internal const string WebPartHelpVerb_Description = "WebPartHelpVerb_Description";

		// Token: 0x04000A54 RID: 2644
		internal const string WebPartHelpVerb_Text = "WebPartHelpVerb_Text";

		// Token: 0x04000A55 RID: 2645
		internal const string WebPartManager_Personalization = "WebPartManager_Personalization";

		// Token: 0x04000A56 RID: 2646
		internal const string WebPartManager_MustRegister = "WebPartManager_MustRegister";

		// Token: 0x04000A57 RID: 2647
		internal const string WebPartManager_UnknownConnection = "WebPartManager_UnknownConnection";

		// Token: 0x04000A58 RID: 2648
		internal const string WebPartManager_AlreadyInConnect = "WebPartManager_AlreadyInConnect";

		// Token: 0x04000A59 RID: 2649
		internal const string WebPartManager_AlreadyInZone = "WebPartManager_AlreadyInZone";

		// Token: 0x04000A5A RID: 2650
		internal const string WebPartManager_MustBeInConnect = "WebPartManager_MustBeInConnect";

		// Token: 0x04000A5B RID: 2651
		internal const string WebPartManager_AlreadyInEdit = "WebPartManager_AlreadyInEdit";

		// Token: 0x04000A5C RID: 2652
		internal const string WebPartManager_MustBeInEdit = "WebPartManager_MustBeInEdit";

		// Token: 0x04000A5D RID: 2653
		internal const string WebPartManager_InvalidConnectionPoint = "WebPartManager_InvalidConnectionPoint";

		// Token: 0x04000A5E RID: 2654
		internal const string WebPartManager_NoSelectedWebPartConnect = "WebPartManager_NoSelectedWebPartConnect";

		// Token: 0x04000A5F RID: 2655
		internal const string WebPartManager_NoSelectedWebPartEdit = "WebPartManager_NoSelectedWebPartEdit";

		// Token: 0x04000A60 RID: 2656
		internal const string WebPartManager_MustBeInZone = "WebPartManager_MustBeInZone";

		// Token: 0x04000A61 RID: 2657
		internal const string WebPartManager_OnlyOneInstance = "WebPartManager_OnlyOneInstance";

		// Token: 0x04000A62 RID: 2658
		internal const string WebPartManager_AlreadyRegistered = "WebPartManager_AlreadyRegistered";

		// Token: 0x04000A63 RID: 2659
		internal const string WebPartManager_NoZoneID = "WebPartManager_NoZoneID";

		// Token: 0x04000A64 RID: 2660
		internal const string WebPartManager_DuplicateZoneID = "WebPartManager_DuplicateZoneID";

		// Token: 0x04000A65 RID: 2661
		internal const string WebPartManager_CannotModify = "WebPartManager_CannotModify";

		// Token: 0x04000A66 RID: 2662
		internal const string WebPartManager_NoWebPartID = "WebPartManager_NoWebPartID";

		// Token: 0x04000A67 RID: 2663
		internal const string WebPartManager_NoChildControlID = "WebPartManager_NoChildControlID";

		// Token: 0x04000A68 RID: 2664
		internal const string WebPartManager_DuplicateWebPartID = "WebPartManager_DuplicateWebPartID";

		// Token: 0x04000A69 RID: 2665
		internal const string WebPartManager_StaticConnections = "WebPartManager_StaticConnections";

		// Token: 0x04000A6A RID: 2666
		internal const string WebPartManager_InvalidConsumerSignature = "WebPartManager_InvalidConsumerSignature";

		// Token: 0x04000A6B RID: 2667
		internal const string WebPartManager_InvalidProviderSignature = "WebPartManager_InvalidProviderSignature";

		// Token: 0x04000A6C RID: 2668
		internal const string WebPartManager_ConnectTooLate = "WebPartManager_ConnectTooLate";

		// Token: 0x04000A6D RID: 2669
		internal const string WebPartManager_DisconnectTooLate = "WebPartManager_DisconnectTooLate";

		// Token: 0x04000A6E RID: 2670
		internal const string WebPartManager_EnableClientScript = "WebPartManager_EnableClientScript";

		// Token: 0x04000A6F RID: 2671
		internal const string WebPartManager_ForbiddenType = "WebPartManager_ForbiddenType";

		// Token: 0x04000A70 RID: 2672
		internal const string WebPartManager_PartNotExportable = "WebPartManager_PartNotExportable";

		// Token: 0x04000A71 RID: 2673
		internal const string WebPartManager_ImportInvalidFormat = "WebPartManager_ImportInvalidFormat";

		// Token: 0x04000A72 RID: 2674
		internal const string WebPartManager_ImportInvalidData = "WebPartManager_ImportInvalidData";

		// Token: 0x04000A73 RID: 2675
		internal const string WebPartManager_RegisterTooLate = "WebPartManager_RegisterTooLate";

		// Token: 0x04000A74 RID: 2676
		internal const string WebPartManager_ExportSensitiveDataWarning = "WebPartManager_ExportSensitiveDataWarning";

		// Token: 0x04000A75 RID: 2677
		internal const string WebPartManager_AlreadyDisconnected = "WebPartManager_AlreadyDisconnected";

		// Token: 0x04000A76 RID: 2678
		internal const string WebPartManager_ConnectionsReadOnly = "WebPartManager_ConnectionsReadOnly";

		// Token: 0x04000A77 RID: 2679
		internal const string WebPartManager_DynamicConnectionsReadOnly = "WebPartManager_DynamicConnectionsReadOnly";

		// Token: 0x04000A78 RID: 2680
		internal const string WebPartManager_StaticConnectionsReadOnly = "WebPartManager_StaticConnectionsReadOnly";

		// Token: 0x04000A79 RID: 2681
		internal const string WebPartManager_DisplayModesReadOnly = "WebPartManager_DisplayModesReadOnly";

		// Token: 0x04000A7A RID: 2682
		internal const string WebPartManager_InvalidDisplayMode = "WebPartManager_InvalidDisplayMode";

		// Token: 0x04000A7B RID: 2683
		internal const string WebPartManager_DisabledDisplayMode = "WebPartManager_DisabledDisplayMode";

		// Token: 0x04000A7C RID: 2684
		internal const string WebPartManager_CloseProviderWarning = "WebPartManager_CloseProviderWarning";

		// Token: 0x04000A7D RID: 2685
		internal const string WebPartManager_DefaultCloseProviderWarning = "WebPartManager_DefaultCloseProviderWarning";

		// Token: 0x04000A7E RID: 2686
		internal const string WebPartManager_DeleteWarning = "WebPartManager_DeleteWarning";

		// Token: 0x04000A7F RID: 2687
		internal const string WebPartManager_DefaultDeleteWarning = "WebPartManager_DefaultDeleteWarning";

		// Token: 0x04000A80 RID: 2688
		internal const string WebPartManager_CantConnectClosed = "WebPartManager_CantConnectClosed";

		// Token: 0x04000A81 RID: 2689
		internal const string WebPartManager_DuplicateConnectionID = "WebPartManager_DuplicateConnectionID";

		// Token: 0x04000A82 RID: 2690
		internal const string WebPartManager_AuthorizeWebPart = "WebPartManager_AuthorizeWebPart";

		// Token: 0x04000A83 RID: 2691
		internal const string WebPartManager_ConnectionsActivated = "WebPartManager_ConnectionsActivated";

		// Token: 0x04000A84 RID: 2692
		internal const string WebPartManager_ConnectionsActivating = "WebPartManager_ConnectionsActivating";

		// Token: 0x04000A85 RID: 2693
		internal const string WebPartManager_DisplayModeChanged = "WebPartManager_DisplayModeChanged";

		// Token: 0x04000A86 RID: 2694
		internal const string WebPartManager_DisplayModeChanging = "WebPartManager_DisplayModeChanging";

		// Token: 0x04000A87 RID: 2695
		internal const string WebPartManager_SelectedWebPartChanged = "WebPartManager_SelectedWebPartChanged";

		// Token: 0x04000A88 RID: 2696
		internal const string WebPartManager_SelectedWebPartChanging = "WebPartManager_SelectedWebPartChanging";

		// Token: 0x04000A89 RID: 2697
		internal const string WebPartManager_WebPartAdded = "WebPartManager_WebPartAdded";

		// Token: 0x04000A8A RID: 2698
		internal const string WebPartManager_WebPartAdding = "WebPartManager_WebPartAdding";

		// Token: 0x04000A8B RID: 2699
		internal const string WebPartManager_WebPartClosed = "WebPartManager_WebPartClosed";

		// Token: 0x04000A8C RID: 2700
		internal const string WebPartManager_WebPartClosing = "WebPartManager_WebPartClosing";

		// Token: 0x04000A8D RID: 2701
		internal const string WebPartManager_WebPartDeleted = "WebPartManager_WebPartDeleted";

		// Token: 0x04000A8E RID: 2702
		internal const string WebPartManager_WebPartDeleting = "WebPartManager_WebPartDeleting";

		// Token: 0x04000A8F RID: 2703
		internal const string WebPartManager_WebPartMoved = "WebPartManager_WebPartMoved";

		// Token: 0x04000A90 RID: 2704
		internal const string WebPartManager_WebPartMoving = "WebPartManager_WebPartMoving";

		// Token: 0x04000A91 RID: 2705
		internal const string WebPartManager_WebPartsConnected = "WebPartManager_WebPartsConnected";

		// Token: 0x04000A92 RID: 2706
		internal const string WebPartManager_WebPartsConnecting = "WebPartManager_WebPartsConnecting";

		// Token: 0x04000A93 RID: 2707
		internal const string WebPartManager_WebPartsDisconnected = "WebPartManager_WebPartsDisconnected";

		// Token: 0x04000A94 RID: 2708
		internal const string WebPartManager_WebPartsDisconnecting = "WebPartManager_WebPartsDisconnecting";

		// Token: 0x04000A95 RID: 2709
		internal const string WebPartManager_CantDeleteStatic = "WebPartManager_CantDeleteStatic";

		// Token: 0x04000A96 RID: 2710
		internal const string WebPartManager_CantDeleteSharedInUserScope = "WebPartManager_CantDeleteSharedInUserScope";

		// Token: 0x04000A97 RID: 2711
		internal const string WebPartManager_CantAddControlType = "WebPartManager_CantAddControlType";

		// Token: 0x04000A98 RID: 2712
		internal const string WebPartManager_PathCannotBeEmpty = "WebPartManager_PathCannotBeEmpty";

		// Token: 0x04000A99 RID: 2713
		internal const string WebPartManager_PathMustBeEmpty = "WebPartManager_PathMustBeEmpty";

		// Token: 0x04000A9A RID: 2714
		internal const string WebPartManager_CantCreateInstance = "WebPartManager_CantCreateInstance";

		// Token: 0x04000A9B RID: 2715
		internal const string WebPartManager_CantCreateInstanceWithType = "WebPartManager_CantCreateInstanceWithType";

		// Token: 0x04000A9C RID: 2716
		internal const string WebPartManager_TypeMustDeriveFromControl = "WebPartManager_TypeMustDeriveFromControl";

		// Token: 0x04000A9D RID: 2717
		internal const string WebPartManager_TypeMustDeriveFromControlWithType = "WebPartManager_TypeMustDeriveFromControlWithType";

		// Token: 0x04000A9E RID: 2718
		internal const string WebPartManager_InvalidPath = "WebPartManager_InvalidPath";

		// Token: 0x04000A9F RID: 2719
		internal const string WebPartManager_InvalidPathWithPath = "WebPartManager_InvalidPathWithPath";

		// Token: 0x04000AA0 RID: 2720
		internal const string WebPartManager_CantCreateGeneric = "WebPartManager_CantCreateGeneric";

		// Token: 0x04000AA1 RID: 2721
		internal const string WebPartManager_CantBeginConnectingClosed = "WebPartManager_CantBeginConnectingClosed";

		// Token: 0x04000AA2 RID: 2722
		internal const string WebPartManager_CantBeginEditingClosed = "WebPartManager_CantBeginEditingClosed";

		// Token: 0x04000AA3 RID: 2723
		internal const string WebPartManager_AlreadyClosed = "WebPartManager_AlreadyClosed";

		// Token: 0x04000AA4 RID: 2724
		internal const string WebPartManager_CantSetEnableTheming = "WebPartManager_CantSetEnableTheming";

		// Token: 0x04000AA5 RID: 2725
		internal const string WebPartManager_CantConnectToSelf = "WebPartManager_CantConnectToSelf";

		// Token: 0x04000AA6 RID: 2726
		internal const string WebPartManager_ErrorLoadingWebPartType = "WebPartManager_ErrorLoadingWebPartType";

		// Token: 0x04000AA7 RID: 2727
		internal const string WebPartManagerRequired = "WebPartManagerRequired";

		// Token: 0x04000AA8 RID: 2728
		internal const string WebPartMenu_DefaultDropDownAlternateText = "WebPartMenu_DefaultDropDownAlternateText";

		// Token: 0x04000AA9 RID: 2729
		internal const string WebPartMenuStyle_ShadowColor = "WebPartMenuStyle_ShadowColor";

		// Token: 0x04000AAA RID: 2730
		internal const string WebPartMinimizeVerb_Description = "WebPartMinimizeVerb_Description";

		// Token: 0x04000AAB RID: 2731
		internal const string WebPartMinimizeVerb_Text = "WebPartMinimizeVerb_Text";

		// Token: 0x04000AAC RID: 2732
		internal const string WebPartPageMenu_BrowseModeVerb = "WebPartPageMenu_BrowseModeVerb";

		// Token: 0x04000AAD RID: 2733
		internal const string WebPartPageMenu_CatalogModeVerb = "WebPartPageMenu_CatalogModeVerb";

		// Token: 0x04000AAE RID: 2734
		internal const string WebPartPageMenu_CheckImageStyle = "WebPartPageMenu_CheckImageStyle";

		// Token: 0x04000AAF RID: 2735
		internal const string WebPartPageMenu_CheckImageUrl = "WebPartPageMenu_CheckImageUrl";

		// Token: 0x04000AB0 RID: 2736
		internal const string WebPartPageMenu_ConnectModeVerb = "WebPartPageMenu_ConnectModeVerb";

		// Token: 0x04000AB1 RID: 2737
		internal const string WebPartPageMenu_DefaultResetWarning = "WebPartPageMenu_DefaultResetWarning";

		// Token: 0x04000AB2 RID: 2738
		internal const string WebPartPageMenu_DesignModeVerb = "WebPartPageMenu_DesignModeVerb";

		// Token: 0x04000AB3 RID: 2739
		internal const string WebPartPageMenu_DropDownAutoPostBack = "WebPartPageMenu_DropDownAutoPostBack";

		// Token: 0x04000AB4 RID: 2740
		internal const string WebPartPageMenu_DropDownButtonText = "WebPartPageMenu_DropDownButtonText";

		// Token: 0x04000AB5 RID: 2741
		internal const string WebPartPageMenu_DropDownButtonText_Default = "WebPartPageMenu_DropDownButtonText_Default";

		// Token: 0x04000AB6 RID: 2742
		internal const string WebPartPageMenu_DropDownOrientation = "WebPartPageMenu_DropDownOrientation";

		// Token: 0x04000AB7 RID: 2743
		internal const string WebPartPageMenu_EditModeVerb = "WebPartPageMenu_EditModeVerb";

		// Token: 0x04000AB8 RID: 2744
		internal const string WebPartPageMenu_ResetVerb = "WebPartPageMenu_ResetVerb";

		// Token: 0x04000AB9 RID: 2745
		internal const string WebPartPageMenu_ResetWarning = "WebPartPageMenu_ResetWarning";

		// Token: 0x04000ABA RID: 2746
		internal const string WebPartPageMenu_SharedScopeVerb = "WebPartPageMenu_SharedScopeVerb";

		// Token: 0x04000ABB RID: 2747
		internal const string WebPartPageMenu_UserScopeVerb = "WebPartPageMenu_UserScopeVerb";

		// Token: 0x04000ABC RID: 2748
		internal const string WebPartPageMenu_MenuPopupImageUrl = "WebPartPageMenu_MenuPopupImageUrl";

		// Token: 0x04000ABD RID: 2749
		internal const string WebPartPageMenu_MenuStyle = "WebPartPageMenu_MenuStyle";

		// Token: 0x04000ABE RID: 2750
		internal const string WebPartPageMenu_Mode = "WebPartPageMenu_Mode";

		// Token: 0x04000ABF RID: 2751
		internal const string WebPartPageMenu_VerbStyle = "WebPartPageMenu_VerbStyle";

		// Token: 0x04000AC0 RID: 2752
		internal const string WebPartPageMenu_VerbHoverStyle = "WebPartPageMenu_VerbHoverStyle";

		// Token: 0x04000AC1 RID: 2753
		internal const string WebPartPageMenu_HoverStyle = "WebPartPageMenu_HoverStyle";

		// Token: 0x04000AC2 RID: 2754
		internal const string WebPartPageMenu_ImageUrl = "WebPartPageMenu_ImageUrl";

		// Token: 0x04000AC3 RID: 2755
		internal const string WebPartPageMenu_Text = "WebPartPageMenu_Text";

		// Token: 0x04000AC4 RID: 2756
		internal const string WebPartPageMenu_Text_Default = "WebPartPageMenu_Text_Default";

		// Token: 0x04000AC5 RID: 2757
		internal const string WebPartPageMenu_ToolTipDefault = "WebPartPageMenu_ToolTipDefault";

		// Token: 0x04000AC6 RID: 2758
		internal const string WebPartPersonalization_CannotLoadPersonalization = "WebPartPersonalization_CannotLoadPersonalization";

		// Token: 0x04000AC7 RID: 2759
		internal const string WebPartPersonalization_CannotEnterSharedScope = "WebPartPersonalization_CannotEnterSharedScope";

		// Token: 0x04000AC8 RID: 2760
		internal const string WebPartPersonalization_CantCallMethodBeforeInit = "WebPartPersonalization_CantCallMethodBeforeInit";

		// Token: 0x04000AC9 RID: 2761
		internal const string WebPartPersonalization_CantUsePropertyBeforeInit = "WebPartPersonalization_CantUsePropertyBeforeInit";

		// Token: 0x04000ACA RID: 2762
		internal const string WebPartPersonalization_Enabled = "WebPartPersonalization_Enabled";

		// Token: 0x04000ACB RID: 2763
		internal const string WebPartPersonalization_InitialScope = "WebPartPersonalization_InitialScope";

		// Token: 0x04000ACC RID: 2764
		internal const string WebPartPersonalization_MustSetBeforeInit = "WebPartPersonalization_MustSetBeforeInit";

		// Token: 0x04000ACD RID: 2765
		internal const string WebPartPersonalization_PersonalizationNotEnabled = "WebPartPersonalization_PersonalizationNotEnabled";

		// Token: 0x04000ACE RID: 2766
		internal const string WebPartPersonalization_PersonalizationNotModifiable = "WebPartPersonalization_PersonalizationNotModifiable";

		// Token: 0x04000ACF RID: 2767
		internal const string WebPartPersonalization_PersonalizationStateNotLoaded = "WebPartPersonalization_PersonalizationStateNotLoaded";

		// Token: 0x04000AD0 RID: 2768
		internal const string WebPartPersonalization_ProviderName = "WebPartPersonalization_ProviderName";

		// Token: 0x04000AD1 RID: 2769
		internal const string WebPartPersonalization_ProviderNotFound = "WebPartPersonalization_ProviderNotFound";

		// Token: 0x04000AD2 RID: 2770
		internal const string WebPartPersonalization_SameType = "WebPartPersonalization_SameType";

		// Token: 0x04000AD3 RID: 2771
		internal const string WebPartUserScopeVerb_Description = "WebPartUserScopeVerb_Description";

		// Token: 0x04000AD4 RID: 2772
		internal const string WebPartUserScopeVerb_Text = "WebPartUserScopeVerb_Text";

		// Token: 0x04000AD5 RID: 2773
		internal const string WebPartRestoreVerb_Description = "WebPartRestoreVerb_Description";

		// Token: 0x04000AD6 RID: 2774
		internal const string WebPartRestoreVerb_Text = "WebPartRestoreVerb_Text";

		// Token: 0x04000AD7 RID: 2775
		internal const string WebPartTracker_CircularConnection = "WebPartTracker_CircularConnection";

		// Token: 0x04000AD8 RID: 2776
		internal const string WebPartVerb_Checked = "WebPartVerb_Checked";

		// Token: 0x04000AD9 RID: 2777
		internal const string WebPartVerb_Description = "WebPartVerb_Description";

		// Token: 0x04000ADA RID: 2778
		internal const string WebPartVerb_Enabled = "WebPartVerb_Enabled";

		// Token: 0x04000ADB RID: 2779
		internal const string WebPartVerb_ImageUrl = "WebPartVerb_ImageUrl";

		// Token: 0x04000ADC RID: 2780
		internal const string WebPartVerb_Text = "WebPartVerb_Text";

		// Token: 0x04000ADD RID: 2781
		internal const string WebPartVerb_Visible = "WebPartVerb_Visible";

		// Token: 0x04000ADE RID: 2782
		internal const string WebPartZoneBase_AllowLayoutChange = "WebPartZoneBase_AllowLayoutChange";

		// Token: 0x04000ADF RID: 2783
		internal const string WebPartZoneBase_CloseVerb = "WebPartZoneBase_CloseVerb";

		// Token: 0x04000AE0 RID: 2784
		internal const string WebPartZoneBase_ConnectVerb = "WebPartZoneBase_ConnectVerb";

		// Token: 0x04000AE1 RID: 2785
		internal const string WebPartZoneBase_CreateVerbs = "WebPartZoneBase_CreateVerbs";

		// Token: 0x04000AE2 RID: 2786
		internal const string WebPartZoneBase_DefaultEmptyZoneText = "WebPartZoneBase_DefaultEmptyZoneText";

		// Token: 0x04000AE3 RID: 2787
		internal const string WebPartZoneBase_DeleteVerb = "WebPartZoneBase_DeleteVerb";

		// Token: 0x04000AE4 RID: 2788
		internal const string WebPartZoneBase_DisplayTitleFallback = "WebPartZoneBase_DisplayTitleFallback";

		// Token: 0x04000AE5 RID: 2789
		internal const string WebPartZoneBase_DragHighlightColor = "WebPartZoneBase_DragHighlightColor";

		// Token: 0x04000AE6 RID: 2790
		internal const string WebPartZoneBase_EditVerb = "WebPartZoneBase_EditVerb";

		// Token: 0x04000AE7 RID: 2791
		internal const string WebPartZoneBase_ExportVerb = "WebPartZoneBase_ExportVerb";

		// Token: 0x04000AE8 RID: 2792
		internal const string WebPartZoneBase_HelpVerb = "WebPartZoneBase_HelpVerb";

		// Token: 0x04000AE9 RID: 2793
		internal const string WebPartZoneBase_LayoutOrientation = "WebPartZoneBase_LayoutOrientation";

		// Token: 0x04000AEA RID: 2794
		internal const string WebPartZoneBase_MenuPopupStyle = "WebPartZoneBase_MenuPopupStyle";

		// Token: 0x04000AEB RID: 2795
		internal const string WebPartZoneBase_MenuCheckImageStyle = "WebPartZoneBase_MenuCheckImageStyle";

		// Token: 0x04000AEC RID: 2796
		internal const string WebPartZoneBase_MenuCheckImageUrl = "WebPartZoneBase_MenuCheckImageUrl";

		// Token: 0x04000AED RID: 2797
		internal const string WebPartZoneBase_MenuLabelHoverStyle = "WebPartZoneBase_MenuLabelHoverStyle";

		// Token: 0x04000AEE RID: 2798
		internal const string WebPartZoneBase_MenuLabelStyle = "WebPartZoneBase_MenuLabelStyle";

		// Token: 0x04000AEF RID: 2799
		internal const string WebPartZoneBase_MenuLabelText = "WebPartZoneBase_MenuLabelText";

		// Token: 0x04000AF0 RID: 2800
		internal const string WebPartZoneBase_MenuPopupImageUrl = "WebPartZoneBase_MenuPopupImageUrl";

		// Token: 0x04000AF1 RID: 2801
		internal const string WebPartZoneBase_MenuVerbHoverStyle = "WebPartZoneBase_MenuVerbHoverStyle";

		// Token: 0x04000AF2 RID: 2802
		internal const string WebPartZoneBase_MenuVerbStyle = "WebPartZoneBase_MenuVerbStyle";

		// Token: 0x04000AF3 RID: 2803
		internal const string WebPartZoneBase_MinimizeVerb = "WebPartZoneBase_MinimizeVerb";

		// Token: 0x04000AF4 RID: 2804
		internal const string WebPartZoneBase_RestoreVerb = "WebPartZoneBase_RestoreVerb";

		// Token: 0x04000AF5 RID: 2805
		internal const string WebPartZoneBase_SelectedPartChromeStyle = "WebPartZoneBase_SelectedPartChromeStyle";

		// Token: 0x04000AF6 RID: 2806
		internal const string WebPartZoneBase_ShowTitleIcons = "WebPartZoneBase_ShowTitleIcons";

		// Token: 0x04000AF7 RID: 2807
		internal const string WebPartZoneBase_TitleBarVerbButtonType = "WebPartZoneBase_TitleBarVerbButtonType";

		// Token: 0x04000AF8 RID: 2808
		internal const string WebPartZoneBase_TitleBarVerbStyle = "WebPartZoneBase_TitleBarVerbStyle";

		// Token: 0x04000AF9 RID: 2809
		internal const string WebPartZoneBase_WebPartVerbRenderMode = "WebPartZoneBase_WebPartVerbRenderMode";

		// Token: 0x04000AFA RID: 2810
		internal const string Zone_AddedTooLate = "Zone_AddedTooLate";

		// Token: 0x04000AFB RID: 2811
		internal const string Zone_EmptyZoneText = "Zone_EmptyZoneText";

		// Token: 0x04000AFC RID: 2812
		internal const string Zone_EmptyZoneTextStyle = "Zone_EmptyZoneTextStyle";

		// Token: 0x04000AFD RID: 2813
		internal const string Zone_ErrorStyle = "Zone_ErrorStyle";

		// Token: 0x04000AFE RID: 2814
		internal const string Zone_FooterStyle = "Zone_FooterStyle";

		// Token: 0x04000AFF RID: 2815
		internal const string Zone_HeaderStyle = "Zone_HeaderStyle";

		// Token: 0x04000B00 RID: 2816
		internal const string Zone_HeaderText = "Zone_HeaderText";

		// Token: 0x04000B01 RID: 2817
		internal const string Zone_InvalidParent = "Zone_InvalidParent";

		// Token: 0x04000B02 RID: 2818
		internal const string Zone_Padding = "Zone_Padding";

		// Token: 0x04000B03 RID: 2819
		internal const string Zone_PartStyle = "Zone_PartStyle";

		// Token: 0x04000B04 RID: 2820
		internal const string Zone_PartChromePadding = "Zone_PartChromePadding";

		// Token: 0x04000B05 RID: 2821
		internal const string Zone_PartChromeStyle = "Zone_PartChromeStyle";

		// Token: 0x04000B06 RID: 2822
		internal const string Zone_PartChromeType = "Zone_PartChromeType";

		// Token: 0x04000B07 RID: 2823
		internal const string Zone_PartTitleStyle = "Zone_PartTitleStyle";

		// Token: 0x04000B08 RID: 2824
		internal const string Zone_VerbButtonType = "Zone_VerbButtonType";

		// Token: 0x04000B09 RID: 2825
		internal const string Zone_VerbStyle = "Zone_VerbStyle";

		// Token: 0x04000B0A RID: 2826
		internal const string Zone_SampleHeaderText = "Zone_SampleHeaderText";

		// Token: 0x04000B0B RID: 2827
		internal const string PersonalizationAdmin_UnexpectedResetSharedStateReturnValue = "PersonalizationAdmin_UnexpectedResetSharedStateReturnValue";

		// Token: 0x04000B0C RID: 2828
		internal const string PersonalizationAdmin_UnexpectedResetUserStateReturnValue = "PersonalizationAdmin_UnexpectedResetUserStateReturnValue";

		// Token: 0x04000B0D RID: 2829
		internal const string PersonalizationAdmin_UnexpectedPersonalizationProviderReturnValue = "PersonalizationAdmin_UnexpectedPersonalizationProviderReturnValue";

		// Token: 0x04000B0E RID: 2830
		internal const string PersonalizationStateInfoCollection_CouldNotAddSharedStateInfo = "PersonalizationStateInfoCollection_CouldNotAddSharedStateInfo";

		// Token: 0x04000B0F RID: 2831
		internal const string PersonalizationStateInfoCollection_CouldNotAddUserStateInfo = "PersonalizationStateInfoCollection_CouldNotAddUserStateInfo";

		// Token: 0x04000B10 RID: 2832
		internal const string PersonalizationStateQuery_IncorrectValueType = "PersonalizationStateQuery_IncorrectValueType";

		// Token: 0x04000B11 RID: 2833
		internal const string PersonalizationProviderHelper_CannotHaveCommaInString = "PersonalizationProviderHelper_CannotHaveCommaInString";

		// Token: 0x04000B12 RID: 2834
		internal const string PersonalizationProviderHelper_Empty_Collection = "PersonalizationProviderHelper_Empty_Collection";

		// Token: 0x04000B13 RID: 2835
		internal const string PersonalizationProviderHelper_Invalid_Less_Than_Parameter = "PersonalizationProviderHelper_Invalid_Less_Than_Parameter";

		// Token: 0x04000B14 RID: 2836
		internal const string PersonalizationProviderHelper_More_Than_One_Path = "PersonalizationProviderHelper_More_Than_One_Path";

		// Token: 0x04000B15 RID: 2837
		internal const string PersonalizationProviderHelper_Negative_Integer = "PersonalizationProviderHelper_Negative_Integer";

		// Token: 0x04000B16 RID: 2838
		internal const string PersonalizationProviderHelper_No_Usernames_Set_In_Shared_Scope = "PersonalizationProviderHelper_No_Usernames_Set_In_Shared_Scope";

		// Token: 0x04000B17 RID: 2839
		internal const string PersonalizationProviderHelper_Null_Entries = "PersonalizationProviderHelper_Null_Entries";

		// Token: 0x04000B18 RID: 2840
		internal const string PersonalizationProviderHelper_Null_Or_Empty_String_Entries = "PersonalizationProviderHelper_Null_Or_Empty_String_Entries";

		// Token: 0x04000B19 RID: 2841
		internal const string PersonalizationProviderHelper_TrimmedEmptyString = "PersonalizationProviderHelper_TrimmedEmptyString";

		// Token: 0x04000B1A RID: 2842
		internal const string PersonalizationProviderHelper_Trimmed_Entry_Value_Exceed_Maximum_Length = "PersonalizationProviderHelper_Trimmed_Entry_Value_Exceed_Maximum_Length";

		// Token: 0x04000B1B RID: 2843
		internal const string StringUtil_Trimmed_String_Exceed_Maximum_Length = "StringUtil_Trimmed_String_Exceed_Maximum_Length";

		// Token: 0x04000B1C RID: 2844
		internal const string Category_Accessibility = "Category_Accessibility";

		// Token: 0x04000B1D RID: 2845
		internal const string Category_Cache = "Category_Cache";

		// Token: 0x04000B1E RID: 2846
		internal const string Category_Control = "Category_Control";

		// Token: 0x04000B1F RID: 2847
		internal const string Category_Databindings = "Category_Databindings";

		// Token: 0x04000B20 RID: 2848
		internal const string Category_DefaultProperties = "Category_DefaultProperties";

		// Token: 0x04000B21 RID: 2849
		internal const string Category_Links = "Category_Links";

		// Token: 0x04000B22 RID: 2850
		internal const string Category_Navigation = "Category_Navigation";

		// Token: 0x04000B23 RID: 2851
		internal const string Category_Paging = "Category_Paging";

		// Token: 0x04000B24 RID: 2852
		internal const string Category_Parameter = "Category_Parameter";

		// Token: 0x04000B25 RID: 2853
		internal const string Category_Styles = "Category_Styles";

		// Token: 0x04000B26 RID: 2854
		internal const string Category_Validation = "Category_Validation";

		// Token: 0x04000B27 RID: 2855
		internal const string Category_Verbs = "Category_Verbs";

		// Token: 0x04000B28 RID: 2856
		internal const string Category_WebPart = "Category_WebPart";

		// Token: 0x04000B29 RID: 2857
		internal const string Category_WebPartAppearance = "Category_WebPartAppearance";

		// Token: 0x04000B2A RID: 2858
		internal const string Category_WebPartBehavior = "Category_WebPartBehavior";

		// Token: 0x04000B2B RID: 2859
		internal const string Error_Formatter_ASPNET_Error = "Error_Formatter_ASPNET_Error";

		// Token: 0x04000B2C RID: 2860
		internal const string Error_Formatter_Description = "Error_Formatter_Description";

		// Token: 0x04000B2D RID: 2861
		internal const string Error_Formatter_Source_File = "Error_Formatter_Source_File";

		// Token: 0x04000B2E RID: 2862
		internal const string Error_Formatter_No_Source_File = "Error_Formatter_No_Source_File";

		// Token: 0x04000B2F RID: 2863
		internal const string Error_Formatter_Version = "Error_Formatter_Version";

		// Token: 0x04000B30 RID: 2864
		internal const string Error_Formatter_CLR_Build = "Error_Formatter_CLR_Build";

		// Token: 0x04000B31 RID: 2865
		internal const string Error_Formatter_ASPNET_Build = "Error_Formatter_ASPNET_Build";

		// Token: 0x04000B32 RID: 2866
		internal const string Error_Formatter_Line = "Error_Formatter_Line";

		// Token: 0x04000B33 RID: 2867
		internal const string Error_Formatter_FusionLog = "Error_Formatter_FusionLog";

		// Token: 0x04000B34 RID: 2868
		internal const string Error_Formatter_FusionLogDesc = "Error_Formatter_FusionLogDesc";

		// Token: 0x04000B35 RID: 2869
		internal const string Unhandled_Err_Error = "Unhandled_Err_Error";

		// Token: 0x04000B36 RID: 2870
		internal const string Unhandled_Err_Desc = "Unhandled_Err_Desc";

		// Token: 0x04000B37 RID: 2871
		internal const string Unhandled_Err_Exception_Details = "Unhandled_Err_Exception_Details";

		// Token: 0x04000B38 RID: 2872
		internal const string Unhandled_Err_Stack_Trace = "Unhandled_Err_Stack_Trace";

		// Token: 0x04000B39 RID: 2873
		internal const string Unauthorized_Err_Desc1 = "Unauthorized_Err_Desc1";

		// Token: 0x04000B3A RID: 2874
		internal const string Unauthorized_Err_Desc2 = "Unauthorized_Err_Desc2";

		// Token: 0x04000B3B RID: 2875
		internal const string Security_Err_Error = "Security_Err_Error";

		// Token: 0x04000B3C RID: 2876
		internal const string Security_Err_Desc = "Security_Err_Desc";

		// Token: 0x04000B3D RID: 2877
		internal const string NotFound_Resource_Not_Found = "NotFound_Resource_Not_Found";

		// Token: 0x04000B3E RID: 2878
		internal const string NotFound_Http_404 = "NotFound_Http_404";

		// Token: 0x04000B3F RID: 2879
		internal const string NotFound_Requested_Url = "NotFound_Requested_Url";

		// Token: 0x04000B40 RID: 2880
		internal const string Forbidden_Type_Not_Served = "Forbidden_Type_Not_Served";

		// Token: 0x04000B41 RID: 2881
		internal const string Forbidden_Extension_Incorrect = "Forbidden_Extension_Incorrect";

		// Token: 0x04000B42 RID: 2882
		internal const string Forbidden_Extension_Desc = "Forbidden_Extension_Desc";

		// Token: 0x04000B43 RID: 2883
		internal const string Generic_Err_Title = "Generic_Err_Title";

		// Token: 0x04000B44 RID: 2884
		internal const string Generic_Err_Local_Desc = "Generic_Err_Local_Desc";

		// Token: 0x04000B45 RID: 2885
		internal const string Generic_Err_Remote_Desc = "Generic_Err_Remote_Desc";

		// Token: 0x04000B46 RID: 2886
		internal const string Generic_Err_Details_Title = "Generic_Err_Details_Title";

		// Token: 0x04000B47 RID: 2887
		internal const string Generic_Err_Local_Details_Desc = "Generic_Err_Local_Details_Desc";

		// Token: 0x04000B48 RID: 2888
		internal const string Generic_Err_Remote_Details_Desc = "Generic_Err_Remote_Details_Desc";

		// Token: 0x04000B49 RID: 2889
		internal const string Generic_Err_Local_Details_Sample = "Generic_Err_Local_Details_Sample";

		// Token: 0x04000B4A RID: 2890
		internal const string Generic_Err_Remote_Details_Sample = "Generic_Err_Remote_Details_Sample";

		// Token: 0x04000B4B RID: 2891
		internal const string Generic_Err_Notes_Title = "Generic_Err_Notes_Title";

		// Token: 0x04000B4C RID: 2892
		internal const string Generic_Err_Notes_Desc = "Generic_Err_Notes_Desc";

		// Token: 0x04000B4D RID: 2893
		internal const string Generic_Err_Local_Notes_Sample = "Generic_Err_Local_Notes_Sample";

		// Token: 0x04000B4E RID: 2894
		internal const string Generic_Err_Remote_Notes_Sample = "Generic_Err_Remote_Notes_Sample";

		// Token: 0x04000B4F RID: 2895
		internal const string WithFile_No_Relevant_Line = "WithFile_No_Relevant_Line";

		// Token: 0x04000B50 RID: 2896
		internal const string Src_not_available = "Src_not_available";

		// Token: 0x04000B51 RID: 2897
		internal const string Src_not_available_nodebug = "Src_not_available_nodebug";

		// Token: 0x04000B52 RID: 2898
		internal const string WithFile_Line_Num = "WithFile_Line_Num";

		// Token: 0x04000B53 RID: 2899
		internal const string TmplCompilerErrorTitle = "TmplCompilerErrorTitle";

		// Token: 0x04000B54 RID: 2900
		internal const string TmplCompilerErrorDesc = "TmplCompilerErrorDesc";

		// Token: 0x04000B55 RID: 2901
		internal const string TmplCompilerCompleteOutput = "TmplCompilerCompleteOutput";

		// Token: 0x04000B56 RID: 2902
		internal const string TmplCompilerGeneratedFile = "TmplCompilerGeneratedFile";

		// Token: 0x04000B57 RID: 2903
		internal const string TmplConfigurationAdditionalError = "TmplConfigurationAdditionalError";

		// Token: 0x04000B58 RID: 2904
		internal const string TmplCompilerErrorSecTitle = "TmplCompilerErrorSecTitle";

		// Token: 0x04000B59 RID: 2905
		internal const string TmplCompilerFatalError = "TmplCompilerFatalError";

		// Token: 0x04000B5A RID: 2906
		internal const string TmplCompilerWarningBanner = "TmplCompilerWarningBanner";

		// Token: 0x04000B5B RID: 2907
		internal const string TmplCompilerWarningSecTitle = "TmplCompilerWarningSecTitle";

		// Token: 0x04000B5C RID: 2908
		internal const string TmplCompilerSourceSecTitle = "TmplCompilerSourceSecTitle";

		// Token: 0x04000B5D RID: 2909
		internal const string TmplCompilerSourceFileTitle = "TmplCompilerSourceFileTitle";

		// Token: 0x04000B5E RID: 2910
		internal const string TmplCompilerSourceFileLine = "TmplCompilerSourceFileLine";

		// Token: 0x04000B5F RID: 2911
		internal const string TmplCompilerLineHeader = "TmplCompilerLineHeader";

		// Token: 0x04000B60 RID: 2912
		internal const string Parser_Error = "Parser_Error";

		// Token: 0x04000B61 RID: 2913
		internal const string Parser_Desc = "Parser_Desc";

		// Token: 0x04000B62 RID: 2914
		internal const string Parser_Error_Message = "Parser_Error_Message";

		// Token: 0x04000B63 RID: 2915
		internal const string Parser_Source_Error = "Parser_Source_Error";

		// Token: 0x04000B64 RID: 2916
		internal const string Config_Error = "Config_Error";

		// Token: 0x04000B65 RID: 2917
		internal const string Config_Desc = "Config_Desc";

		// Token: 0x04000B66 RID: 2918
		internal const string File_Circular_Reference = "File_Circular_Reference";

		// Token: 0x04000B67 RID: 2919
		internal const string CantGenPropertySet = "CantGenPropertySet";

		// Token: 0x04000B68 RID: 2920
		internal const string Trace_Request = "Trace_Request";

		// Token: 0x04000B69 RID: 2921
		internal const string Trace_Status_Code = "Trace_Status_Code";

		// Token: 0x04000B6A RID: 2922
		internal const string Trace_Trace_Information = "Trace_Trace_Information";

		// Token: 0x04000B6B RID: 2923
		internal const string Trace_Category = "Trace_Category";

		// Token: 0x04000B6C RID: 2924
		internal const string Trace_From_First = "Trace_From_First";

		// Token: 0x04000B6D RID: 2925
		internal const string Trace_Message = "Trace_Message";

		// Token: 0x04000B6E RID: 2926
		internal const string Trace_Warning = "Trace_Warning";

		// Token: 0x04000B6F RID: 2927
		internal const string Trace_From_Last = "Trace_From_Last";

		// Token: 0x04000B70 RID: 2928
		internal const string Trace_Control_Tree = "Trace_Control_Tree";

		// Token: 0x04000B71 RID: 2929
		internal const string Trace_Control_Id = "Trace_Control_Id";

		// Token: 0x04000B72 RID: 2930
		internal const string Trace_Parent_Id = "Trace_Parent_Id";

		// Token: 0x04000B73 RID: 2931
		internal const string Trace_Type = "Trace_Type";

		// Token: 0x04000B74 RID: 2932
		internal const string Trace_Viewstate_Size = "Trace_Viewstate_Size";

		// Token: 0x04000B75 RID: 2933
		internal const string Trace_Controlstate_Size = "Trace_Controlstate_Size";

		// Token: 0x04000B76 RID: 2934
		internal const string Trace_Render_Size = "Trace_Render_Size";

		// Token: 0x04000B77 RID: 2935
		internal const string Trace_Session_State = "Trace_Session_State";

		// Token: 0x04000B78 RID: 2936
		internal const string Trace_Application_State = "Trace_Application_State";

		// Token: 0x04000B79 RID: 2937
		internal const string Trace_Request_Cookies_Collection = "Trace_Request_Cookies_Collection";

		// Token: 0x04000B7A RID: 2938
		internal const string Trace_Response_Cookies_Collection = "Trace_Response_Cookies_Collection";

		// Token: 0x04000B7B RID: 2939
		internal const string Trace_Headers_Collection = "Trace_Headers_Collection";

		// Token: 0x04000B7C RID: 2940
		internal const string Trace_Response_Headers_Collection = "Trace_Response_Headers_Collection";

		// Token: 0x04000B7D RID: 2941
		internal const string Trace_Form_Collection = "Trace_Form_Collection";

		// Token: 0x04000B7E RID: 2942
		internal const string Trace_Querystring_Collection = "Trace_Querystring_Collection";

		// Token: 0x04000B7F RID: 2943
		internal const string Trace_Server_Variables = "Trace_Server_Variables";

		// Token: 0x04000B80 RID: 2944
		internal const string Trace_Time_of_Request = "Trace_Time_of_Request";

		// Token: 0x04000B81 RID: 2945
		internal const string Trace_Url = "Trace_Url";

		// Token: 0x04000B82 RID: 2946
		internal const string Trace_Request_Type = "Trace_Request_Type";

		// Token: 0x04000B83 RID: 2947
		internal const string Trace_Request_Encoding = "Trace_Request_Encoding";

		// Token: 0x04000B84 RID: 2948
		internal const string Trace_Name = "Trace_Name";

		// Token: 0x04000B85 RID: 2949
		internal const string Trace_Value = "Trace_Value";

		// Token: 0x04000B86 RID: 2950
		internal const string Trace_Response_Encoding = "Trace_Response_Encoding";

		// Token: 0x04000B87 RID: 2951
		internal const string Trace_Session_Id = "Trace_Session_Id";

		// Token: 0x04000B88 RID: 2952
		internal const string Trace_No = "Trace_No";

		// Token: 0x04000B89 RID: 2953
		internal const string Trace_Application_Key = "Trace_Application_Key";

		// Token: 0x04000B8A RID: 2954
		internal const string Trace_Session_Key = "Trace_Session_Key";

		// Token: 0x04000B8B RID: 2955
		internal const string Trace_Size = "Trace_Size";

		// Token: 0x04000B8C RID: 2956
		internal const string Trace_Request_Details = "Trace_Request_Details";

		// Token: 0x04000B8D RID: 2957
		internal const string Trace_Application_Trace = "Trace_Application_Trace";

		// Token: 0x04000B8E RID: 2958
		internal const string Trace_Clear_Current = "Trace_Clear_Current";

		// Token: 0x04000B8F RID: 2959
		internal const string Trace_Physical_Directory = "Trace_Physical_Directory";

		// Token: 0x04000B90 RID: 2960
		internal const string Trace_Requests_This = "Trace_Requests_This";

		// Token: 0x04000B91 RID: 2961
		internal const string Trace_Remaining = "Trace_Remaining";

		// Token: 0x04000B92 RID: 2962
		internal const string Trace_File = "Trace_File";

		// Token: 0x04000B93 RID: 2963
		internal const string Trace_Verb = "Trace_Verb";

		// Token: 0x04000B94 RID: 2964
		internal const string Trace_View_Details = "Trace_View_Details";

		// Token: 0x04000B95 RID: 2965
		internal const string Trace_Render_Size_children = "Trace_Render_Size_children";

		// Token: 0x04000B96 RID: 2966
		internal const string Trace_Viewstate_Size_Nochildren = "Trace_Viewstate_Size_Nochildren";

		// Token: 0x04000B97 RID: 2967
		internal const string Trace_Controlstate_Size_Nochildren = "Trace_Controlstate_Size_Nochildren";

		// Token: 0x04000B98 RID: 2968
		internal const string Trace_Page = "Trace_Page";

		// Token: 0x04000B99 RID: 2969
		internal const string Trace_Error_Title = "Trace_Error_Title";

		// Token: 0x04000B9A RID: 2970
		internal const string Trace_Error_LocalOnly_Description = "Trace_Error_LocalOnly_Description";

		// Token: 0x04000B9B RID: 2971
		internal const string Trace_Error_LocalOnly_Details_Desc = "Trace_Error_LocalOnly_Details_Desc";

		// Token: 0x04000B9C RID: 2972
		internal const string Trace_Error_LocalOnly_Details_Sample = "Trace_Error_LocalOnly_Details_Sample";

		// Token: 0x04000B9D RID: 2973
		internal const string Trace_Error_Enabled_Description = "Trace_Error_Enabled_Description";

		// Token: 0x04000B9E RID: 2974
		internal const string Trace_Error_Enabled_Details_Desc = "Trace_Error_Enabled_Details_Desc";

		// Token: 0x04000B9F RID: 2975
		internal const string Trace_Error_Enabled_Details_Sample = "Trace_Error_Enabled_Details_Sample";

		// Token: 0x04000BA0 RID: 2976
		internal const string WebPageTraceListener_Event = "WebPageTraceListener_Event";

		// Token: 0x04000BA1 RID: 2977
		internal const string Adapter_GoLabel = "Adapter_GoLabel";

		// Token: 0x04000BA2 RID: 2978
		internal const string Adapter_OKLabel = "Adapter_OKLabel";

		// Token: 0x04000BA3 RID: 2979
		internal const string Adapter_NextLabel = "Adapter_NextLabel";

		// Token: 0x04000BA4 RID: 2980
		internal const string Adapter_PreviousLabel = "Adapter_PreviousLabel";

		// Token: 0x04000BA5 RID: 2981
		internal const string CalendarAdapter_FirstPrompt = "CalendarAdapter_FirstPrompt";

		// Token: 0x04000BA6 RID: 2982
		internal const string CalendarAdapter_OptionPrompt = "CalendarAdapter_OptionPrompt";

		// Token: 0x04000BA7 RID: 2983
		internal const string CalendarAdapter_OptionType = "CalendarAdapter_OptionType";

		// Token: 0x04000BA8 RID: 2984
		internal const string CalendarAdapter_OptionEra = "CalendarAdapter_OptionEra";

		// Token: 0x04000BA9 RID: 2985
		internal const string CalendarAdapter_OptionChooseDate = "CalendarAdapter_OptionChooseDate";

		// Token: 0x04000BAA RID: 2986
		internal const string CalendarAdapter_OptionChooseWeek = "CalendarAdapter_OptionChooseWeek";

		// Token: 0x04000BAB RID: 2987
		internal const string CalendarAdapter_OptionChooseMonth = "CalendarAdapter_OptionChooseMonth";

		// Token: 0x04000BAC RID: 2988
		internal const string CalendarAdapter_TextBoxErrorMessage = "CalendarAdapter_TextBoxErrorMessage";

		// Token: 0x04000BAD RID: 2989
		internal const string HtmlImageButtonAdapter_DefaultAlternateText = "HtmlImageButtonAdapter_DefaultAlternateText";

		// Token: 0x04000BAE RID: 2990
		internal const string MenuAdapter_Up = "MenuAdapter_Up";

		// Token: 0x04000BAF RID: 2991
		internal const string MenuAdapter_UpOneLevel = "MenuAdapter_UpOneLevel";

		// Token: 0x04000BB0 RID: 2992
		internal const string MenuAdapter_Expand = "MenuAdapter_Expand";

		// Token: 0x04000BB1 RID: 2993
		internal const string PageAdapter_MustHaveFormRunatServer = "PageAdapter_MustHaveFormRunatServer";

		// Token: 0x04000BB2 RID: 2994
		internal const string PageAdapter_RenderDelegateMustBeInServerForm = "PageAdapter_RenderDelegateMustBeInServerForm";

		// Token: 0x04000BB3 RID: 2995
		internal const string TreeViewAdapter_Content = "TreeViewAdapter_Content";

		// Token: 0x04000BB4 RID: 2996
		internal const string TreeViewAdapter_Nav = "TreeViewAdapter_Nav";

		// Token: 0x04000BB5 RID: 2997
		internal const string TreeViewAdapter_Go = "TreeViewAdapter_Go";

		// Token: 0x04000BB6 RID: 2998
		internal const string TreeViewAdapter_Up = "TreeViewAdapter_Up";

		// Token: 0x04000BB7 RID: 2999
		internal const string TreeViewAdapter_UpOneLevel = "TreeViewAdapter_UpOneLevel";

		// Token: 0x04000BB8 RID: 3000
		internal const string SQL_Services_Database_Empty_Or_Space_Only_Arg = "SQL_Services_Database_Empty_Or_Space_Only_Arg";

		// Token: 0x04000BB9 RID: 3001
		internal const string SQL_Services_Cant_connect_sql_database = "SQL_Services_Cant_connect_sql_database";

		// Token: 0x04000BBA RID: 3002
		internal const string SQL_Services_Invalid_Feature = "SQL_Services_Invalid_Feature";

		// Token: 0x04000BBB RID: 3003
		internal const string SQL_Services_Error_Deleting_Session_Job = "SQL_Services_Error_Deleting_Session_Job";

		// Token: 0x04000BBC RID: 3004
		internal const string SQL_Services_Error_Executing_Command = "SQL_Services_Error_Executing_Command";

		// Token: 0x04000BBD RID: 3005
		internal const string SQL_Services_Error_Cant_Uninstall_Nonempty_Table = "SQL_Services_Error_Cant_Uninstall_Nonempty_Table";

		// Token: 0x04000BBE RID: 3006
		internal const string SQL_Services_Error_Cant_Uninstall_Nonexisting_Database = "SQL_Services_Error_Cant_Uninstall_Nonexisting_Database";

		// Token: 0x04000BBF RID: 3007
		internal const string SQL_Services_Error_Cant_use_custom_database = "SQL_Services_Error_Cant_use_custom_database";

		// Token: 0x04000BC0 RID: 3008
		internal const string SQL_Services_Error_missing_custom_database = "SQL_Services_Error_missing_custom_database";

		// Token: 0x04000BC1 RID: 3009
		internal const string SQL_Services_Database_contains_invalid_chars = "SQL_Services_Database_contains_invalid_chars";

		// Token: 0x04000BC2 RID: 3010
		internal const string Provider_missing_attribute = "Provider_missing_attribute";

		// Token: 0x04000BC3 RID: 3011
		internal const string Invalid_provider_attribute = "Invalid_provider_attribute";

		// Token: 0x04000BC4 RID: 3012
		internal const string Invalid_mail_priority_provider_attribute = "Invalid_mail_priority_provider_attribute";

		// Token: 0x04000BC5 RID: 3013
		internal const string Invalid_mail_template_provider_attribute = "Invalid_mail_template_provider_attribute";

		// Token: 0x04000BC6 RID: 3014
		internal const string Unexpected_provider_attribute = "Unexpected_provider_attribute";

		// Token: 0x04000BC7 RID: 3015
		internal const string Invalid_provider_positive_attributes = "Invalid_provider_positive_attributes";

		// Token: 0x04000BC8 RID: 3016
		internal const string Invalid_provider_non_zero_positive_attributes = "Invalid_provider_non_zero_positive_attributes";

		// Token: 0x04000BC9 RID: 3017
		internal const string Event_name_not_found = "Event_name_not_found";

		// Token: 0x04000BCA RID: 3018
		internal const string Event_name_invalid_code_range = "Event_name_invalid_code_range";

		// Token: 0x04000BCB RID: 3019
		internal const string Health_mon_profile_not_found = "Health_mon_profile_not_found";

		// Token: 0x04000BCC RID: 3020
		internal const string Health_mon_provider_not_found = "Health_mon_provider_not_found";

		// Token: 0x04000BCD RID: 3021
		internal const string Wmi_provider_cant_initialize = "Wmi_provider_cant_initialize";

		// Token: 0x04000BCE RID: 3022
		internal const string Invalid_max_event_details_length = "Invalid_max_event_details_length";

		// Token: 0x04000BCF RID: 3023
		internal const string Health_mon_buffer_mode_not_found = "Health_mon_buffer_mode_not_found";

		// Token: 0x04000BD0 RID: 3024
		internal const string Invalid_attribute1_must_less_than_or_equal_attribute2 = "Invalid_attribute1_must_less_than_or_equal_attribute2";

		// Token: 0x04000BD1 RID: 3025
		internal const string Invalid_attribute1_must_less_than_attribute2 = "Invalid_attribute1_must_less_than_attribute2";

		// Token: 0x04000BD2 RID: 3026
		internal const string MailWebEventProvider_discard_warning = "MailWebEventProvider_discard_warning";

		// Token: 0x04000BD3 RID: 3027
		internal const string MailWebEventProvider_events_drop_warning = "MailWebEventProvider_events_drop_warning";

		// Token: 0x04000BD4 RID: 3028
		internal const string MailWebEventProvider_summary_body = "MailWebEventProvider_summary_body";

		// Token: 0x04000BD5 RID: 3029
		internal const string WebEvent_event_email_subject = "WebEvent_event_email_subject";

		// Token: 0x04000BD6 RID: 3030
		internal const string WebEvent_event_group_email_subject = "WebEvent_event_group_email_subject";

		// Token: 0x04000BD7 RID: 3031
		internal const string WebEvent_event_email_subject_template_error = "WebEvent_event_email_subject_template_error";

		// Token: 0x04000BD8 RID: 3032
		internal const string MailWebEventProvider_Warnings = "MailWebEventProvider_Warnings";

		// Token: 0x04000BD9 RID: 3033
		internal const string MailWebEventProvider_Summary = "MailWebEventProvider_Summary";

		// Token: 0x04000BDA RID: 3034
		internal const string MailWebEventProvider_Application_Info = "MailWebEventProvider_Application_Info";

		// Token: 0x04000BDB RID: 3035
		internal const string MailWebEventProvider_Events = "MailWebEventProvider_Events";

		// Token: 0x04000BDC RID: 3036
		internal const string MailWebEventProvider_template_file_not_found_error = "MailWebEventProvider_template_file_not_found_error";

		// Token: 0x04000BDD RID: 3037
		internal const string MailWebEventProvider_template_runtime_error = "MailWebEventProvider_template_runtime_error";

		// Token: 0x04000BDE RID: 3038
		internal const string MailWebEventProvider_template_compile_error = "MailWebEventProvider_template_compile_error";

		// Token: 0x04000BDF RID: 3039
		internal const string MailWebEventProvider_template_error_no_details = "MailWebEventProvider_template_error_no_details";

		// Token: 0x04000BE0 RID: 3040
		internal const string MailWebEventProvider_no_recipient_error = "MailWebEventProvider_no_recipient_error";

		// Token: 0x04000BE1 RID: 3041
		internal const string Sql_webevent_provider_events_dropped = "Sql_webevent_provider_events_dropped";

		// Token: 0x04000BE2 RID: 3042
		internal const string MailWebEventProvider_cannot_send_mail = "MailWebEventProvider_cannot_send_mail";

		// Token: 0x04000BE3 RID: 3043
		internal const string Invalid_eventCode_error = "Invalid_eventCode_error";

		// Token: 0x04000BE4 RID: 3044
		internal const string Invalid_eventDetailCode_error = "Invalid_eventDetailCode_error";

		// Token: 0x04000BE5 RID: 3045
		internal const string System_eventCode_not_allowed = "System_eventCode_not_allowed";

		// Token: 0x04000BE6 RID: 3046
		internal const string Event_log_provider_error = "Event_log_provider_error";

		// Token: 0x04000BE7 RID: 3047
		internal const string Wmi_provider_error = "Wmi_provider_error";

		// Token: 0x04000BE8 RID: 3048
		internal const string Webevent_msg_ApplicationStart = "Webevent_msg_ApplicationStart";

		// Token: 0x04000BE9 RID: 3049
		internal const string Webevent_msg_ApplicationShutdown = "Webevent_msg_ApplicationShutdown";

		// Token: 0x04000BEA RID: 3050
		internal const string Webevent_msg_ApplicationCompilationStart = "Webevent_msg_ApplicationCompilationStart";

		// Token: 0x04000BEB RID: 3051
		internal const string Webevent_msg_ApplicationCompilationEnd = "Webevent_msg_ApplicationCompilationEnd";

		// Token: 0x04000BEC RID: 3052
		internal const string Webevent_msg_ApplicationHeartbeat = "Webevent_msg_ApplicationHeartbeat";

		// Token: 0x04000BED RID: 3053
		internal const string Webevent_msg_RequestTransactionComplete = "Webevent_msg_RequestTransactionComplete";

		// Token: 0x04000BEE RID: 3054
		internal const string Webevent_msg_RequestTransactionAbort = "Webevent_msg_RequestTransactionAbort";

		// Token: 0x04000BEF RID: 3055
		internal const string Webevent_msg_RuntimeErrorRequestAbort = "Webevent_msg_RuntimeErrorRequestAbort";

		// Token: 0x04000BF0 RID: 3056
		internal const string Webevent_msg_RuntimeErrorViewStateFailure = "Webevent_msg_RuntimeErrorViewStateFailure";

		// Token: 0x04000BF1 RID: 3057
		internal const string Webevent_msg_RuntimeErrorValidationFailure = "Webevent_msg_RuntimeErrorValidationFailure";

		// Token: 0x04000BF2 RID: 3058
		internal const string Webevent_msg_RuntimeErrorPostTooLarge = "Webevent_msg_RuntimeErrorPostTooLarge";

		// Token: 0x04000BF3 RID: 3059
		internal const string Webevent_msg_RuntimeErrorUnhandledException = "Webevent_msg_RuntimeErrorUnhandledException";

		// Token: 0x04000BF4 RID: 3060
		internal const string Webevent_msg_WebErrorParserError = "Webevent_msg_WebErrorParserError";

		// Token: 0x04000BF5 RID: 3061
		internal const string Webevent_msg_WebErrorCompilationError = "Webevent_msg_WebErrorCompilationError";

		// Token: 0x04000BF6 RID: 3062
		internal const string Webevent_msg_WebErrorConfigurationError = "Webevent_msg_WebErrorConfigurationError";

		// Token: 0x04000BF7 RID: 3063
		internal const string Webevent_msg_AuditUnhandledSecurityException = "Webevent_msg_AuditUnhandledSecurityException";

		// Token: 0x04000BF8 RID: 3064
		internal const string Webevent_msg_AuditInvalidViewStateFailure = "Webevent_msg_AuditInvalidViewStateFailure";

		// Token: 0x04000BF9 RID: 3065
		internal const string Webevent_msg_AuditFormsAuthenticationSuccess = "Webevent_msg_AuditFormsAuthenticationSuccess";

		// Token: 0x04000BFA RID: 3066
		internal const string Webevent_msg_AuditUrlAuthorizationSuccess = "Webevent_msg_AuditUrlAuthorizationSuccess";

		// Token: 0x04000BFB RID: 3067
		internal const string Webevent_msg_AuditFileAuthorizationFailure = "Webevent_msg_AuditFileAuthorizationFailure";

		// Token: 0x04000BFC RID: 3068
		internal const string Webevent_msg_AuditFormsAuthenticationFailure = "Webevent_msg_AuditFormsAuthenticationFailure";

		// Token: 0x04000BFD RID: 3069
		internal const string Webevent_msg_AuditFileAuthorizationSuccess = "Webevent_msg_AuditFileAuthorizationSuccess";

		// Token: 0x04000BFE RID: 3070
		internal const string Webevent_msg_AuditMembershipAuthenticationSuccess = "Webevent_msg_AuditMembershipAuthenticationSuccess";

		// Token: 0x04000BFF RID: 3071
		internal const string Webevent_msg_AuditMembershipAuthenticationFailure = "Webevent_msg_AuditMembershipAuthenticationFailure";

		// Token: 0x04000C00 RID: 3072
		internal const string Webevent_msg_AuditUrlAuthorizationFailure = "Webevent_msg_AuditUrlAuthorizationFailure";

		// Token: 0x04000C01 RID: 3073
		internal const string Webevent_msg_AuditUnhandledAccessException = "Webevent_msg_AuditUnhandledAccessException";

		// Token: 0x04000C02 RID: 3074
		internal const string Webevent_msg_OSF_Deserialization_String = "Webevent_msg_OSF_Deserialization_String";

		// Token: 0x04000C03 RID: 3075
		internal const string Webevent_msg_OSF_Deserialization_Binary = "Webevent_msg_OSF_Deserialization_Binary";

		// Token: 0x04000C04 RID: 3076
		internal const string Webevent_msg_OSF_Deserialization_Type = "Webevent_msg_OSF_Deserialization_Type";

		// Token: 0x04000C05 RID: 3077
		internal const string Webevent_msg_Property_Deserialization = "Webevent_msg_Property_Deserialization";

		// Token: 0x04000C06 RID: 3078
		internal const string Webevent_detail_ApplicationShutdownUnknown = "Webevent_detail_ApplicationShutdownUnknown";

		// Token: 0x04000C07 RID: 3079
		internal const string Webevent_detail_ApplicationShutdownHostingEnvironment = "Webevent_detail_ApplicationShutdownHostingEnvironment";

		// Token: 0x04000C08 RID: 3080
		internal const string Webevent_detail_ApplicationShutdownChangeInGlobalAsax = "Webevent_detail_ApplicationShutdownChangeInGlobalAsax";

		// Token: 0x04000C09 RID: 3081
		internal const string Webevent_detail_ApplicationShutdownConfigurationChange = "Webevent_detail_ApplicationShutdownConfigurationChange";

		// Token: 0x04000C0A RID: 3082
		internal const string Webevent_detail_ApplicationShutdownUnloadAppDomainCalled = "Webevent_detail_ApplicationShutdownUnloadAppDomainCalled";

		// Token: 0x04000C0B RID: 3083
		internal const string Webevent_detail_ApplicationShutdownChangeInSecurityPolicyFile = "Webevent_detail_ApplicationShutdownChangeInSecurityPolicyFile";

		// Token: 0x04000C0C RID: 3084
		internal const string Webevent_detail_ApplicationShutdownBinDirChangeOrDirectoryRename = "Webevent_detail_ApplicationShutdownBinDirChangeOrDirectoryRename";

		// Token: 0x04000C0D RID: 3085
		internal const string Webevent_detail_ApplicationShutdownBrowsersDirChangeOrDirectoryRename = "Webevent_detail_ApplicationShutdownBrowsersDirChangeOrDirectoryRename";

		// Token: 0x04000C0E RID: 3086
		internal const string Webevent_detail_ApplicationShutdownCodeDirChangeOrDirectoryRename = "Webevent_detail_ApplicationShutdownCodeDirChangeOrDirectoryRename";

		// Token: 0x04000C0F RID: 3087
		internal const string Webevent_detail_ApplicationShutdownResourcesDirChangeOrDirectoryRename = "Webevent_detail_ApplicationShutdownResourcesDirChangeOrDirectoryRename";

		// Token: 0x04000C10 RID: 3088
		internal const string Webevent_detail_ApplicationShutdownIdleTimeout = "Webevent_detail_ApplicationShutdownIdleTimeout";

		// Token: 0x04000C11 RID: 3089
		internal const string Webevent_detail_ApplicationShutdownPhysicalApplicationPathChanged = "Webevent_detail_ApplicationShutdownPhysicalApplicationPathChanged";

		// Token: 0x04000C12 RID: 3090
		internal const string Webevent_detail_ApplicationShutdownHttpRuntimeClose = "Webevent_detail_ApplicationShutdownHttpRuntimeClose";

		// Token: 0x04000C13 RID: 3091
		internal const string Webevent_detail_ApplicationShutdownInitializationError = "Webevent_detail_ApplicationShutdownInitializationError";

		// Token: 0x04000C14 RID: 3092
		internal const string Webevent_detail_ApplicationShutdownMaxRecompilationsReached = "Webevent_detail_ApplicationShutdownMaxRecompilationsReached";

		// Token: 0x04000C15 RID: 3093
		internal const string Webevent_detail_ApplicationShutdownBuildManagerChange = "Webevent_detail_ApplicationShutdownBuildManagerChange";

		// Token: 0x04000C16 RID: 3094
		internal const string Webevent_detail_StateServerConnectionError = "Webevent_detail_StateServerConnectionError";

		// Token: 0x04000C17 RID: 3095
		internal const string Webevent_detail_InvalidTicketFailure = "Webevent_detail_InvalidTicketFailure";

		// Token: 0x04000C18 RID: 3096
		internal const string Webevent_detail_ExpiredTicketFailure = "Webevent_detail_ExpiredTicketFailure";

		// Token: 0x04000C19 RID: 3097
		internal const string Webevent_detail_InvalidViewStateMac = "Webevent_detail_InvalidViewStateMac";

		// Token: 0x04000C1A RID: 3098
		internal const string Webevent_detail_InvalidViewState = "Webevent_detail_InvalidViewState";

		// Token: 0x04000C1B RID: 3099
		internal const string Webevent_detail_SqlProviderEventsDropped = "Webevent_detail_SqlProviderEventsDropped";

		// Token: 0x04000C1C RID: 3100
		internal const string Webevent_event_code = "Webevent_event_code";

		// Token: 0x04000C1D RID: 3101
		internal const string Webevent_event_message = "Webevent_event_message";

		// Token: 0x04000C1E RID: 3102
		internal const string Webevent_event_time = "Webevent_event_time";

		// Token: 0x04000C1F RID: 3103
		internal const string Webevent_event_time_Utc = "Webevent_event_time_Utc";

		// Token: 0x04000C20 RID: 3104
		internal const string Webevent_event_sequence = "Webevent_event_sequence";

		// Token: 0x04000C21 RID: 3105
		internal const string Webevent_event_occurrence = "Webevent_event_occurrence";

		// Token: 0x04000C22 RID: 3106
		internal const string Webevent_event_id = "Webevent_event_id";

		// Token: 0x04000C23 RID: 3107
		internal const string Webevent_event_detail_code = "Webevent_event_detail_code";

		// Token: 0x04000C24 RID: 3108
		internal const string Webevent_event_process_information = "Webevent_event_process_information";

		// Token: 0x04000C25 RID: 3109
		internal const string Webevent_event_application_information = "Webevent_event_application_information";

		// Token: 0x04000C26 RID: 3110
		internal const string Webevent_event_process_statistics = "Webevent_event_process_statistics";

		// Token: 0x04000C27 RID: 3111
		internal const string Webevent_event_request_information = "Webevent_event_request_information";

		// Token: 0x04000C28 RID: 3112
		internal const string Webevent_event_exception_information = "Webevent_event_exception_information";

		// Token: 0x04000C29 RID: 3113
		internal const string Webevent_event_inner_exception_information = "Webevent_event_inner_exception_information";

		// Token: 0x04000C2A RID: 3114
		internal const string Webevent_event_exception_type = "Webevent_event_exception_type";

		// Token: 0x04000C2B RID: 3115
		internal const string Webevent_event_exception_message = "Webevent_event_exception_message";

		// Token: 0x04000C2C RID: 3116
		internal const string Webevent_event_thread_information = "Webevent_event_thread_information";

		// Token: 0x04000C2D RID: 3117
		internal const string Webevent_event_process_id = "Webevent_event_process_id";

		// Token: 0x04000C2E RID: 3118
		internal const string Webevent_event_process_name = "Webevent_event_process_name";

		// Token: 0x04000C2F RID: 3119
		internal const string Webevent_event_account_name = "Webevent_event_account_name";

		// Token: 0x04000C30 RID: 3120
		internal const string Webevent_event_machine_name = "Webevent_event_machine_name";

		// Token: 0x04000C31 RID: 3121
		internal const string Webevent_event_application_domain = "Webevent_event_application_domain";

		// Token: 0x04000C32 RID: 3122
		internal const string Webevent_event_trust_level = "Webevent_event_trust_level";

		// Token: 0x04000C33 RID: 3123
		internal const string Webevent_event_application_virtual_path = "Webevent_event_application_virtual_path";

		// Token: 0x04000C34 RID: 3124
		internal const string Webevent_event_application_path = "Webevent_event_application_path";

		// Token: 0x04000C35 RID: 3125
		internal const string Webevent_event_request_url = "Webevent_event_request_url";

		// Token: 0x04000C36 RID: 3126
		internal const string Webevent_event_request_path = "Webevent_event_request_path";

		// Token: 0x04000C37 RID: 3127
		internal const string Webevent_event_user = "Webevent_event_user";

		// Token: 0x04000C38 RID: 3128
		internal const string Webevent_event_is_authenticated = "Webevent_event_is_authenticated";

		// Token: 0x04000C39 RID: 3129
		internal const string Webevent_event_is_not_authenticated = "Webevent_event_is_not_authenticated";

		// Token: 0x04000C3A RID: 3130
		internal const string Webevent_event_authentication_type = "Webevent_event_authentication_type";

		// Token: 0x04000C3B RID: 3131
		internal const string Webevent_event_process_start_time = "Webevent_event_process_start_time";

		// Token: 0x04000C3C RID: 3132
		internal const string Webevent_event_thread_count = "Webevent_event_thread_count";

		// Token: 0x04000C3D RID: 3133
		internal const string Webevent_event_working_set = "Webevent_event_working_set";

		// Token: 0x04000C3E RID: 3134
		internal const string Webevent_event_peak_working_set = "Webevent_event_peak_working_set";

		// Token: 0x04000C3F RID: 3135
		internal const string Webevent_event_managed_heap_size = "Webevent_event_managed_heap_size";

		// Token: 0x04000C40 RID: 3136
		internal const string Webevent_event_application_domain_count = "Webevent_event_application_domain_count";

		// Token: 0x04000C41 RID: 3137
		internal const string Webevent_event_requests_executing = "Webevent_event_requests_executing";

		// Token: 0x04000C42 RID: 3138
		internal const string Webevent_event_request_queued = "Webevent_event_request_queued";

		// Token: 0x04000C43 RID: 3139
		internal const string Webevent_event_request_rejected = "Webevent_event_request_rejected";

		// Token: 0x04000C44 RID: 3140
		internal const string Webevent_event_thread_id = "Webevent_event_thread_id";

		// Token: 0x04000C45 RID: 3141
		internal const string Webevent_event_thread_account_name = "Webevent_event_thread_account_name";

		// Token: 0x04000C46 RID: 3142
		internal const string Webevent_event_is_impersonating = "Webevent_event_is_impersonating";

		// Token: 0x04000C47 RID: 3143
		internal const string Webevent_event_is_not_impersonating = "Webevent_event_is_not_impersonating";

		// Token: 0x04000C48 RID: 3144
		internal const string Webevent_event_stack_trace = "Webevent_event_stack_trace";

		// Token: 0x04000C49 RID: 3145
		internal const string Webevent_event_user_host_address = "Webevent_event_user_host_address";

		// Token: 0x04000C4A RID: 3146
		internal const string Webevent_event_name_to_authenticate = "Webevent_event_name_to_authenticate";

		// Token: 0x04000C4B RID: 3147
		internal const string Webevent_event_custom_event_details = "Webevent_event_custom_event_details";

		// Token: 0x04000C4C RID: 3148
		internal const string Webevent_event_ViewStateException_information = "Webevent_event_ViewStateException_information";

		// Token: 0x04000C4D RID: 3149
		internal const string Etw_Batch_Compilation = "Etw_Batch_Compilation";

		// Token: 0x04000C4E RID: 3150
		internal const string Etw_Success = "Etw_Success";

		// Token: 0x04000C4F RID: 3151
		internal const string Etw_Failure = "Etw_Failure";

		// Token: 0x04000C50 RID: 3152
		internal const string Config_collection_add_element_without_key = "Config_collection_add_element_without_key";

		// Token: 0x04000C51 RID: 3153
		internal const string Config_VDir_Path_invalid = "Config_VDir_Path_invalid";

		// Token: 0x04000C52 RID: 3154
		internal const string Invalid_priority_name = "Invalid_priority_name";

		// Token: 0x04000C53 RID: 3155
		internal const string Failed_Pipeline_Subscription = "Failed_Pipeline_Subscription";

		// Token: 0x04000C54 RID: 3156
		internal const string Cant_Unwrap_Context_Handle = "Cant_Unwrap_Context_Handle";

		// Token: 0x04000C55 RID: 3157
		internal const string Invalid_event_delivery = "Invalid_event_delivery";

		// Token: 0x04000C56 RID: 3158
		internal const string Failed_to_init_application = "Failed_to_init_application";

		// Token: 0x04000C57 RID: 3159
		internal const string Application_Not_In_Collection = "Application_Not_In_Collection";

		// Token: 0x04000C58 RID: 3160
		internal const string Cant_Init_Native_Config = "Cant_Init_Native_Config";

		// Token: 0x04000C59 RID: 3161
		internal const string Cant_Enumerate_NativeDirs = "Cant_Enumerate_NativeDirs";

		// Token: 0x04000C5A RID: 3162
		internal const string Cant_Read_Native_Modules = "Cant_Read_Native_Modules";

		// Token: 0x04000C5B RID: 3163
		internal const string Call_Invalid_AppDomain = "Call_Invalid_AppDomain";

		// Token: 0x04000C5C RID: 3164
		internal const string Cant_Create_Process_Host = "Cant_Create_Process_Host";

		// Token: 0x04000C5D RID: 3165
		internal const string Invalid_AppDomain_Prot_Type = "Invalid_AppDomain_Prot_Type";

		// Token: 0x04000C5E RID: 3166
		internal const string Invalid_Process_Prot_Type = "Invalid_Process_Prot_Type";

		// Token: 0x04000C5F RID: 3167
		internal const string Failure_Stop_Listener_Channel = "Failure_Stop_Listener_Channel";

		// Token: 0x04000C60 RID: 3168
		internal const string Failure_Stop_Process_Prot = "Failure_Stop_Process_Prot";

		// Token: 0x04000C61 RID: 3169
		internal const string Failure_Start_AppDomain_Listener = "Failure_Start_AppDomain_Listener";

		// Token: 0x04000C62 RID: 3170
		internal const string Failure_Stop_AppDomain_Listener = "Failure_Stop_AppDomain_Listener";

		// Token: 0x04000C63 RID: 3171
		internal const string Failure_Stop_AppDomain_Protocol = "Failure_Stop_AppDomain_Protocol";

		// Token: 0x04000C64 RID: 3172
		internal const string Failure_Start_Integrated_App = "Failure_Start_Integrated_App";

		// Token: 0x04000C65 RID: 3173
		internal const string Failure_Stop_Integrated_App = "Failure_Stop_Integrated_App";

		// Token: 0x04000C66 RID: 3174
		internal const string Failure_Shutdown_ProcessHost = "Failure_Shutdown_ProcessHost";

		// Token: 0x04000C67 RID: 3175
		internal const string Failure_AppDomain_Enum = "Failure_AppDomain_Enum";

		// Token: 0x04000C68 RID: 3176
		internal const string Failure_PMH_Ping = "Failure_PMH_Ping";

		// Token: 0x04000C69 RID: 3177
		internal const string Failure_PMH_Idle = "Failure_PMH_Idle";

		// Token: 0x04000C6A RID: 3178
		internal const string Failure_Create_Listener_Shim = "Failure_Create_Listener_Shim";

		// Token: 0x04000C6B RID: 3179
		internal const string Failure_Reentered_Domain = "Failure_Reentered_Domain";

		// Token: 0x04000C6C RID: 3180
		internal const string Event_Binding_Disallowed = "Event_Binding_Disallowed";

		// Token: 0x04000C6D RID: 3181
		internal const string Requires_Iis_Integrated_Mode = "Requires_Iis_Integrated_Mode";

		// Token: 0x04000C6E RID: 3182
		internal const string Method_Not_Supported_By_Iis_Integrated_Mode = "Method_Not_Supported_By_Iis_Integrated_Mode";

		// Token: 0x04000C6F RID: 3183
		internal const string Requires_Iis_7 = "Requires_Iis_7";

		// Token: 0x04000C70 RID: 3184
		internal const string Invalid_before_authentication = "Invalid_before_authentication";

		// Token: 0x04000C71 RID: 3185
		internal const string Application_instance_cannot_be_changed = "Application_instance_cannot_be_changed";

		// Token: 0x04000C72 RID: 3186
		internal const string Invalid_http_data_chunk = "Invalid_http_data_chunk";

		// Token: 0x04000C73 RID: 3187
		internal const string Substitution_blocks_cannot_be_modified = "Substitution_blocks_cannot_be_modified";

		// Token: 0x04000C74 RID: 3188
		internal const string TransferRequest_cannot_be_invoked_more_than_once = "TransferRequest_cannot_be_invoked_more_than_once";

		// Token: 0x04000C75 RID: 3189
		internal const string Invoke_before_pipeline_event = "Invoke_before_pipeline_event";

		// Token: 0x04000C76 RID: 3190
		private static SR loader;

		// Token: 0x04000C77 RID: 3191
		private ResourceManager resources;

		// Token: 0x04000C78 RID: 3192
		private static object s_InternalSyncObject;
	}
}
