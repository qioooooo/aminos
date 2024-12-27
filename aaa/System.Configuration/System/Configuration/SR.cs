using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System.Configuration
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
			this.resources = new ResourceManager("System.Configuration", base.GetType().Assembly);
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
		internal const string Wrapped_exception_message = "Wrapped_exception_message";

		// Token: 0x04000037 RID: 55
		internal const string Config_error_loading_XML_file = "Config_error_loading_XML_file";

		// Token: 0x04000038 RID: 56
		internal const string Config_exception_creating_section_handler = "Config_exception_creating_section_handler";

		// Token: 0x04000039 RID: 57
		internal const string Config_exception_creating_section = "Config_exception_creating_section";

		// Token: 0x0400003A RID: 58
		internal const string Config_tag_name_invalid = "Config_tag_name_invalid";

		// Token: 0x0400003B RID: 59
		internal const string Argument_AddingDuplicate = "Argument_AddingDuplicate";

		// Token: 0x0400003C RID: 60
		internal const string Config_add_configurationsection_already_added = "Config_add_configurationsection_already_added";

		// Token: 0x0400003D RID: 61
		internal const string Config_add_configurationsection_already_exists = "Config_add_configurationsection_already_exists";

		// Token: 0x0400003E RID: 62
		internal const string Config_add_configurationsection_in_location_config = "Config_add_configurationsection_in_location_config";

		// Token: 0x0400003F RID: 63
		internal const string Config_add_configurationsectiongroup_already_added = "Config_add_configurationsectiongroup_already_added";

		// Token: 0x04000040 RID: 64
		internal const string Config_add_configurationsectiongroup_already_exists = "Config_add_configurationsectiongroup_already_exists";

		// Token: 0x04000041 RID: 65
		internal const string Config_add_configurationsectiongroup_in_location_config = "Config_add_configurationsectiongroup_in_location_config";

		// Token: 0x04000042 RID: 66
		internal const string Config_allow_exedefinition_error_application = "Config_allow_exedefinition_error_application";

		// Token: 0x04000043 RID: 67
		internal const string Config_allow_exedefinition_error_machine = "Config_allow_exedefinition_error_machine";

		// Token: 0x04000044 RID: 68
		internal const string Config_allow_exedefinition_error_roaminguser = "Config_allow_exedefinition_error_roaminguser";

		// Token: 0x04000045 RID: 69
		internal const string Config_appsettings_declaration_invalid = "Config_appsettings_declaration_invalid";

		// Token: 0x04000046 RID: 70
		internal const string Config_base_attribute_locked = "Config_base_attribute_locked";

		// Token: 0x04000047 RID: 71
		internal const string Config_base_collection_item_locked_cannot_clear = "Config_base_collection_item_locked_cannot_clear";

		// Token: 0x04000048 RID: 72
		internal const string Config_base_collection_item_locked = "Config_base_collection_item_locked";

		// Token: 0x04000049 RID: 73
		internal const string Config_base_cannot_add_items_above_inherited_items = "Config_base_cannot_add_items_above_inherited_items";

		// Token: 0x0400004A RID: 74
		internal const string Config_base_cannot_add_items_below_inherited_items = "Config_base_cannot_add_items_below_inherited_items";

		// Token: 0x0400004B RID: 75
		internal const string Config_base_cannot_remove_inherited_items = "Config_base_cannot_remove_inherited_items";

		// Token: 0x0400004C RID: 76
		internal const string Config_base_collection_elements_may_not_be_removed = "Config_base_collection_elements_may_not_be_removed";

		// Token: 0x0400004D RID: 77
		internal const string Config_base_collection_entry_already_exists = "Config_base_collection_entry_already_exists";

		// Token: 0x0400004E RID: 78
		internal const string Config_base_collection_entry_already_removed = "Config_base_collection_entry_already_removed";

		// Token: 0x0400004F RID: 79
		internal const string Config_base_collection_entry_not_found = "Config_base_collection_entry_not_found";

		// Token: 0x04000050 RID: 80
		internal const string Config_base_element_cannot_have_multiple_child_elements = "Config_base_element_cannot_have_multiple_child_elements";

		// Token: 0x04000051 RID: 81
		internal const string Config_base_element_default_collection_cannot_be_locked = "Config_base_element_default_collection_cannot_be_locked";

		// Token: 0x04000052 RID: 82
		internal const string Config_base_element_locked = "Config_base_element_locked";

		// Token: 0x04000053 RID: 83
		internal const string Config_base_expected_enum = "Config_base_expected_enum";

		// Token: 0x04000054 RID: 84
		internal const string Config_base_expected_to_find_element = "Config_base_expected_to_find_element";

		// Token: 0x04000055 RID: 85
		internal const string Config_base_invalid_attribute_to_lock = "Config_base_invalid_attribute_to_lock";

		// Token: 0x04000056 RID: 86
		internal const string Config_base_invalid_attribute_to_lock_by_add = "Config_base_invalid_attribute_to_lock_by_add";

		// Token: 0x04000057 RID: 87
		internal const string Config_base_invalid_element_key = "Config_base_invalid_element_key";

		// Token: 0x04000058 RID: 88
		internal const string Config_base_invalid_element_to_lock = "Config_base_invalid_element_to_lock";

		// Token: 0x04000059 RID: 89
		internal const string Config_base_invalid_element_to_lock_by_add = "Config_base_invalid_element_to_lock_by_add";

		// Token: 0x0400005A RID: 90
		internal const string Config_base_property_is_not_a_configuration_element = "Config_base_property_is_not_a_configuration_element";

		// Token: 0x0400005B RID: 91
		internal const string Config_base_read_only = "Config_base_read_only";

		// Token: 0x0400005C RID: 92
		internal const string Config_base_required_attribute_locked = "Config_base_required_attribute_locked";

		// Token: 0x0400005D RID: 93
		internal const string Config_base_required_attribute_lock_attempt = "Config_base_required_attribute_lock_attempt";

		// Token: 0x0400005E RID: 94
		internal const string Config_base_required_attribute_missing = "Config_base_required_attribute_missing";

		// Token: 0x0400005F RID: 95
		internal const string Config_base_section_cannot_contain_cdata = "Config_base_section_cannot_contain_cdata";

		// Token: 0x04000060 RID: 96
		internal const string Config_base_section_invalid_content = "Config_base_section_invalid_content";

		// Token: 0x04000061 RID: 97
		internal const string Config_base_unrecognized_attribute = "Config_base_unrecognized_attribute";

		// Token: 0x04000062 RID: 98
		internal const string Config_base_unrecognized_element = "Config_base_unrecognized_element";

		// Token: 0x04000063 RID: 99
		internal const string Config_base_unrecognized_element_name = "Config_base_unrecognized_element_name";

		// Token: 0x04000064 RID: 100
		internal const string Config_base_value_cannot_contain = "Config_base_value_cannot_contain";

		// Token: 0x04000065 RID: 101
		internal const string Config_cannot_edit_configurationsection_in_location_config = "Config_cannot_edit_configurationsection_in_location_config";

		// Token: 0x04000066 RID: 102
		internal const string Config_cannot_edit_configurationsection_parentsection = "Config_cannot_edit_configurationsection_parentsection";

		// Token: 0x04000067 RID: 103
		internal const string Config_cannot_edit_configurationsection_when_location_locked = "Config_cannot_edit_configurationsection_when_location_locked";

		// Token: 0x04000068 RID: 104
		internal const string Config_cannot_edit_configurationsection_when_locked = "Config_cannot_edit_configurationsection_when_locked";

		// Token: 0x04000069 RID: 105
		internal const string Config_cannot_edit_configurationsection_when_not_attached = "Config_cannot_edit_configurationsection_when_not_attached";

		// Token: 0x0400006A RID: 106
		internal const string Config_cannot_edit_configurationsection_when_it_is_implicit = "Config_cannot_edit_configurationsection_when_it_is_implicit";

		// Token: 0x0400006B RID: 107
		internal const string Config_cannot_edit_configurationsection_when_it_is_undeclared = "Config_cannot_edit_configurationsection_when_it_is_undeclared";

		// Token: 0x0400006C RID: 108
		internal const string Config_cannot_edit_configurationsectiongroup_in_location_config = "Config_cannot_edit_configurationsectiongroup_in_location_config";

		// Token: 0x0400006D RID: 109
		internal const string Config_cannot_edit_configurationsectiongroup_when_not_attached = "Config_cannot_edit_configurationsectiongroup_when_not_attached";

		// Token: 0x0400006E RID: 110
		internal const string Config_cannot_edit_locationattriubtes = "Config_cannot_edit_locationattriubtes";

		// Token: 0x0400006F RID: 111
		internal const string Config_cannot_open_config_source = "Config_cannot_open_config_source";

		// Token: 0x04000070 RID: 112
		internal const string Config_client_config_init_error = "Config_client_config_init_error";

		// Token: 0x04000071 RID: 113
		internal const string Config_client_config_init_security = "Config_client_config_init_security";

		// Token: 0x04000072 RID: 114
		internal const string Config_client_config_too_many_configsections_elements = "Config_client_config_too_many_configsections_elements";

		// Token: 0x04000073 RID: 115
		internal const string Config_configmanager_open_noexe = "Config_configmanager_open_noexe";

		// Token: 0x04000074 RID: 116
		internal const string Config_configsection_parentnotvalid = "Config_configsection_parentnotvalid";

		// Token: 0x04000075 RID: 117
		internal const string Config_connectionstrings_declaration_invalid = "Config_connectionstrings_declaration_invalid";

		// Token: 0x04000076 RID: 118
		internal const string Config_data_read_count_mismatch = "Config_data_read_count_mismatch";

		// Token: 0x04000077 RID: 119
		internal const string Config_element_no_context = "Config_element_no_context";

		// Token: 0x04000078 RID: 120
		internal const string Config_empty_lock_attributes_except = "Config_empty_lock_attributes_except";

		// Token: 0x04000079 RID: 121
		internal const string Config_empty_lock_attributes_except_effective = "Config_empty_lock_attributes_except_effective";

		// Token: 0x0400007A RID: 122
		internal const string Config_empty_lock_element_except = "Config_empty_lock_element_except";

		// Token: 0x0400007B RID: 123
		internal const string Config_exception_in_config_section_handler = "Config_exception_in_config_section_handler";

		// Token: 0x0400007C RID: 124
		internal const string Config_file_doesnt_have_root_configuration = "Config_file_doesnt_have_root_configuration";

		// Token: 0x0400007D RID: 125
		internal const string Config_file_has_changed = "Config_file_has_changed";

		// Token: 0x0400007E RID: 126
		internal const string Config_getparentconfigurationsection_first_instance = "Config_getparentconfigurationsection_first_instance";

		// Token: 0x0400007F RID: 127
		internal const string Config_inconsistent_location_attributes = "Config_inconsistent_location_attributes";

		// Token: 0x04000080 RID: 128
		internal const string Config_invalid_attributes_for_write = "Config_invalid_attributes_for_write";

		// Token: 0x04000081 RID: 129
		internal const string Config_invalid_boolean_attribute = "Config_invalid_boolean_attribute";

		// Token: 0x04000082 RID: 130
		internal const string Config_invalid_configurationsection_constructor = "Config_invalid_configurationsection_constructor";

		// Token: 0x04000083 RID: 131
		internal const string Config_invalid_node_type = "Config_invalid_node_type";

		// Token: 0x04000084 RID: 132
		internal const string Config_location_location_not_allowed = "Config_location_location_not_allowed";

		// Token: 0x04000085 RID: 133
		internal const string Config_location_path_invalid_character = "Config_location_path_invalid_character";

		// Token: 0x04000086 RID: 134
		internal const string Config_location_path_invalid_first_character = "Config_location_path_invalid_first_character";

		// Token: 0x04000087 RID: 135
		internal const string Config_location_path_invalid_last_character = "Config_location_path_invalid_last_character";

		// Token: 0x04000088 RID: 136
		internal const string Config_missing_required_attribute = "Config_missing_required_attribute";

		// Token: 0x04000089 RID: 137
		internal const string Config_more_data_than_expected = "Config_more_data_than_expected";

		// Token: 0x0400008A RID: 138
		internal const string Config_name_value_file_section_file_invalid_root = "Config_name_value_file_section_file_invalid_root";

		// Token: 0x0400008B RID: 139
		internal const string Config_namespace_invalid = "Config_namespace_invalid";

		// Token: 0x0400008C RID: 140
		internal const string Config_no_stream_to_write = "Config_no_stream_to_write";

		// Token: 0x0400008D RID: 141
		internal const string Config_not_allowed_to_encrypt_this_section = "Config_not_allowed_to_encrypt_this_section";

		// Token: 0x0400008E RID: 142
		internal const string Config_object_is_null = "Config_object_is_null";

		// Token: 0x0400008F RID: 143
		internal const string Config_operation_not_runtime = "Config_operation_not_runtime";

		// Token: 0x04000090 RID: 144
		internal const string Config_properties_may_not_be_derived_from_configuration_section = "Config_properties_may_not_be_derived_from_configuration_section";

		// Token: 0x04000091 RID: 145
		internal const string Config_protection_section_not_found = "Config_protection_section_not_found";

		// Token: 0x04000092 RID: 146
		internal const string Config_provider_must_implement_type = "Config_provider_must_implement_type";

		// Token: 0x04000093 RID: 147
		internal const string Config_root_section_group_cannot_be_edited = "Config_root_section_group_cannot_be_edited";

		// Token: 0x04000094 RID: 148
		internal const string Config_section_allow_definition_attribute_invalid = "Config_section_allow_definition_attribute_invalid";

		// Token: 0x04000095 RID: 149
		internal const string Config_section_allow_exe_definition_attribute_invalid = "Config_section_allow_exe_definition_attribute_invalid";

		// Token: 0x04000096 RID: 150
		internal const string Config_section_cannot_be_used_in_location = "Config_section_cannot_be_used_in_location";

		// Token: 0x04000097 RID: 151
		internal const string Config_section_group_missing_public_constructor = "Config_section_group_missing_public_constructor";

		// Token: 0x04000098 RID: 152
		internal const string Config_section_locked = "Config_section_locked";

		// Token: 0x04000099 RID: 153
		internal const string Config_sections_must_be_unique = "Config_sections_must_be_unique";

		// Token: 0x0400009A RID: 154
		internal const string Config_source_cannot_be_shared = "Config_source_cannot_be_shared";

		// Token: 0x0400009B RID: 155
		internal const string Config_source_parent_conflict = "Config_source_parent_conflict";

		// Token: 0x0400009C RID: 156
		internal const string Config_source_file_format = "Config_source_file_format";

		// Token: 0x0400009D RID: 157
		internal const string Config_source_invalid_format = "Config_source_invalid_format";

		// Token: 0x0400009E RID: 158
		internal const string Config_source_invalid_chars = "Config_source_invalid_chars";

		// Token: 0x0400009F RID: 159
		internal const string Config_source_requires_file = "Config_source_requires_file";

		// Token: 0x040000A0 RID: 160
		internal const string Config_source_syntax_error = "Config_source_syntax_error";

		// Token: 0x040000A1 RID: 161
		internal const string Config_system_already_set = "Config_system_already_set";

		// Token: 0x040000A2 RID: 162
		internal const string Config_tag_name_already_defined = "Config_tag_name_already_defined";

		// Token: 0x040000A3 RID: 163
		internal const string Config_tag_name_already_defined_at_this_level = "Config_tag_name_already_defined_at_this_level";

		// Token: 0x040000A4 RID: 164
		internal const string Config_tag_name_cannot_be_location = "Config_tag_name_cannot_be_location";

		// Token: 0x040000A5 RID: 165
		internal const string Config_tag_name_cannot_begin_with_config = "Config_tag_name_cannot_begin_with_config";

		// Token: 0x040000A6 RID: 166
		internal const string Config_type_doesnt_inherit_from_type = "Config_type_doesnt_inherit_from_type";

		// Token: 0x040000A7 RID: 167
		internal const string Config_unexpected_element_end = "Config_unexpected_element_end";

		// Token: 0x040000A8 RID: 168
		internal const string Config_unexpected_element_name = "Config_unexpected_element_name";

		// Token: 0x040000A9 RID: 169
		internal const string Config_unexpected_node_type = "Config_unexpected_node_type";

		// Token: 0x040000AA RID: 170
		internal const string Config_unrecognized_configuration_section = "Config_unrecognized_configuration_section";

		// Token: 0x040000AB RID: 171
		internal const string Config_write_failed = "Config_write_failed";

		// Token: 0x040000AC RID: 172
		internal const string Converter_timespan_not_in_second = "Converter_timespan_not_in_second";

		// Token: 0x040000AD RID: 173
		internal const string Converter_unsupported_value_type = "Converter_unsupported_value_type";

		// Token: 0x040000AE RID: 174
		internal const string Decryption_failed = "Decryption_failed";

		// Token: 0x040000AF RID: 175
		internal const string Default_value_conversion_error_from_string = "Default_value_conversion_error_from_string";

		// Token: 0x040000B0 RID: 176
		internal const string Default_value_wrong_type = "Default_value_wrong_type";

		// Token: 0x040000B1 RID: 177
		internal const string DPAPI_bad_data = "DPAPI_bad_data";

		// Token: 0x040000B2 RID: 178
		internal const string Empty_attribute = "Empty_attribute";

		// Token: 0x040000B3 RID: 179
		internal const string EncryptedNode_not_found = "EncryptedNode_not_found";

		// Token: 0x040000B4 RID: 180
		internal const string EncryptedNode_is_in_invalid_format = "EncryptedNode_is_in_invalid_format";

		// Token: 0x040000B5 RID: 181
		internal const string Encryption_failed = "Encryption_failed";

		// Token: 0x040000B6 RID: 182
		internal const string Expect_bool_value_for_DoNotShowUI = "Expect_bool_value_for_DoNotShowUI";

		// Token: 0x040000B7 RID: 183
		internal const string Expect_bool_value_for_useMachineProtection = "Expect_bool_value_for_useMachineProtection";

		// Token: 0x040000B8 RID: 184
		internal const string IndexOutOfRange = "IndexOutOfRange";

		// Token: 0x040000B9 RID: 185
		internal const string Invalid_enum_value = "Invalid_enum_value";

		// Token: 0x040000BA RID: 186
		internal const string Key_container_doesnt_exist_or_access_denied = "Key_container_doesnt_exist_or_access_denied";

		// Token: 0x040000BB RID: 187
		internal const string Must_add_to_config_before_protecting_it = "Must_add_to_config_before_protecting_it";

		// Token: 0x040000BC RID: 188
		internal const string No_converter = "No_converter";

		// Token: 0x040000BD RID: 189
		internal const string No_exception_information_available = "No_exception_information_available";

		// Token: 0x040000BE RID: 190
		internal const string Property_name_reserved = "Property_name_reserved";

		// Token: 0x040000BF RID: 191
		internal const string Item_name_reserved = "Item_name_reserved";

		// Token: 0x040000C0 RID: 192
		internal const string Basicmap_item_name_reserved = "Basicmap_item_name_reserved";

		// Token: 0x040000C1 RID: 193
		internal const string ProtectedConfigurationProvider_not_found = "ProtectedConfigurationProvider_not_found";

		// Token: 0x040000C2 RID: 194
		internal const string Regex_validator_error = "Regex_validator_error";

		// Token: 0x040000C3 RID: 195
		internal const string String_null_or_empty = "String_null_or_empty";

		// Token: 0x040000C4 RID: 196
		internal const string Subclass_validator_error = "Subclass_validator_error";

		// Token: 0x040000C5 RID: 197
		internal const string Top_level_conversion_error_from_string = "Top_level_conversion_error_from_string";

		// Token: 0x040000C6 RID: 198
		internal const string Top_level_conversion_error_to_string = "Top_level_conversion_error_to_string";

		// Token: 0x040000C7 RID: 199
		internal const string Top_level_validation_error = "Top_level_validation_error";

		// Token: 0x040000C8 RID: 200
		internal const string Type_cannot_be_resolved = "Type_cannot_be_resolved";

		// Token: 0x040000C9 RID: 201
		internal const string TypeNotPublic = "TypeNotPublic";

		// Token: 0x040000CA RID: 202
		internal const string Unrecognized_initialization_value = "Unrecognized_initialization_value";

		// Token: 0x040000CB RID: 203
		internal const string UseMachineContainer_must_be_bool = "UseMachineContainer_must_be_bool";

		// Token: 0x040000CC RID: 204
		internal const string UseOAEP_must_be_bool = "UseOAEP_must_be_bool";

		// Token: 0x040000CD RID: 205
		internal const string Validation_scalar_range_violation_not_different = "Validation_scalar_range_violation_not_different";

		// Token: 0x040000CE RID: 206
		internal const string Validation_scalar_range_violation_not_equal = "Validation_scalar_range_violation_not_equal";

		// Token: 0x040000CF RID: 207
		internal const string Validation_scalar_range_violation_not_in_range = "Validation_scalar_range_violation_not_in_range";

		// Token: 0x040000D0 RID: 208
		internal const string Validation_scalar_range_violation_not_outside_range = "Validation_scalar_range_violation_not_outside_range";

		// Token: 0x040000D1 RID: 209
		internal const string Validator_Attribute_param_not_validator = "Validator_Attribute_param_not_validator";

		// Token: 0x040000D2 RID: 210
		internal const string Validator_does_not_support_elem_type = "Validator_does_not_support_elem_type";

		// Token: 0x040000D3 RID: 211
		internal const string Validator_does_not_support_prop_type = "Validator_does_not_support_prop_type";

		// Token: 0x040000D4 RID: 212
		internal const string Validator_element_not_valid = "Validator_element_not_valid";

		// Token: 0x040000D5 RID: 213
		internal const string Validator_method_not_found = "Validator_method_not_found";

		// Token: 0x040000D6 RID: 214
		internal const string Validator_min_greater_than_max = "Validator_min_greater_than_max";

		// Token: 0x040000D7 RID: 215
		internal const string Validator_scalar_resolution_violation = "Validator_scalar_resolution_violation";

		// Token: 0x040000D8 RID: 216
		internal const string Validator_string_invalid_chars = "Validator_string_invalid_chars";

		// Token: 0x040000D9 RID: 217
		internal const string Validator_string_max_length = "Validator_string_max_length";

		// Token: 0x040000DA RID: 218
		internal const string Validator_string_min_length = "Validator_string_min_length";

		// Token: 0x040000DB RID: 219
		internal const string Validator_value_type_invalid = "Validator_value_type_invalid";

		// Token: 0x040000DC RID: 220
		internal const string Validator_multiple_validator_attributes = "Validator_multiple_validator_attributes";

		// Token: 0x040000DD RID: 221
		internal const string Validator_timespan_value_must_be_positive = "Validator_timespan_value_must_be_positive";

		// Token: 0x040000DE RID: 222
		internal const string WrongType_of_Protected_provider = "WrongType_of_Protected_provider";

		// Token: 0x040000DF RID: 223
		internal const string Type_from_untrusted_assembly = "Type_from_untrusted_assembly";

		// Token: 0x040000E0 RID: 224
		internal const string Config_element_locking_not_supported = "Config_element_locking_not_supported";

		// Token: 0x040000E1 RID: 225
		internal const string Config_element_null_instance = "Config_element_null_instance";

		// Token: 0x040000E2 RID: 226
		internal const string ConfigurationPermissionBadXml = "ConfigurationPermissionBadXml";

		// Token: 0x040000E3 RID: 227
		internal const string ConfigurationPermission_Denied = "ConfigurationPermission_Denied";

		// Token: 0x040000E4 RID: 228
		internal const string Section_from_untrusted_assembly = "Section_from_untrusted_assembly";

		// Token: 0x040000E5 RID: 229
		internal const string Protection_provider_syntax_error = "Protection_provider_syntax_error";

		// Token: 0x040000E6 RID: 230
		internal const string Protection_provider_invalid_format = "Protection_provider_invalid_format";

		// Token: 0x040000E7 RID: 231
		internal const string Cannot_declare_or_remove_implicit_section = "Cannot_declare_or_remove_implicit_section";

		// Token: 0x040000E8 RID: 232
		internal const string Config_reserved_attribute = "Config_reserved_attribute";

		// Token: 0x040000E9 RID: 233
		internal const string Filename_in_SaveAs_is_used_already = "Filename_in_SaveAs_is_used_already";

		// Token: 0x040000EA RID: 234
		internal const string Provider_Already_Initialized = "Provider_Already_Initialized";

		// Token: 0x040000EB RID: 235
		internal const string Config_provider_name_null_or_empty = "Config_provider_name_null_or_empty";

		// Token: 0x040000EC RID: 236
		internal const string CollectionReadOnly = "CollectionReadOnly";

		// Token: 0x040000ED RID: 237
		internal const string Config_source_not_under_config_dir = "Config_source_not_under_config_dir";

		// Token: 0x040000EE RID: 238
		internal const string Config_source_invalid = "Config_source_invalid";

		// Token: 0x040000EF RID: 239
		internal const string Location_invalid_inheritInChildApplications_in_machine_or_root_web_config = "Location_invalid_inheritInChildApplications_in_machine_or_root_web_config";

		// Token: 0x040000F0 RID: 240
		internal const string Cannot_change_both_AllowOverride_and_OverrideMode = "Cannot_change_both_AllowOverride_and_OverrideMode";

		// Token: 0x040000F1 RID: 241
		internal const string Config_section_override_mode_attribute_invalid = "Config_section_override_mode_attribute_invalid";

		// Token: 0x040000F2 RID: 242
		internal const string Invalid_override_mode_declaration = "Invalid_override_mode_declaration";

		// Token: 0x040000F3 RID: 243
		internal const string Config_cannot_edit_locked_configurationsection_when_mode_is_not_allow = "Config_cannot_edit_locked_configurationsection_when_mode_is_not_allow";

		// Token: 0x040000F4 RID: 244
		private static SR loader;

		// Token: 0x040000F5 RID: 245
		private ResourceManager resources;

		// Token: 0x040000F6 RID: 246
		private static object s_InternalSyncObject;
	}
}
