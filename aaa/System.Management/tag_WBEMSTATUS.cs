﻿using System;

namespace System.Management
{
	// Token: 0x020000E4 RID: 228
	internal enum tag_WBEMSTATUS
	{
		// Token: 0x04000368 RID: 872
		WBEM_NO_ERROR,
		// Token: 0x04000369 RID: 873
		WBEM_S_NO_ERROR = 0,
		// Token: 0x0400036A RID: 874
		WBEM_S_SAME = 0,
		// Token: 0x0400036B RID: 875
		WBEM_S_FALSE,
		// Token: 0x0400036C RID: 876
		WBEM_S_ALREADY_EXISTS = 262145,
		// Token: 0x0400036D RID: 877
		WBEM_S_RESET_TO_DEFAULT,
		// Token: 0x0400036E RID: 878
		WBEM_S_DIFFERENT,
		// Token: 0x0400036F RID: 879
		WBEM_S_TIMEDOUT,
		// Token: 0x04000370 RID: 880
		WBEM_S_NO_MORE_DATA,
		// Token: 0x04000371 RID: 881
		WBEM_S_OPERATION_CANCELLED,
		// Token: 0x04000372 RID: 882
		WBEM_S_PENDING,
		// Token: 0x04000373 RID: 883
		WBEM_S_DUPLICATE_OBJECTS,
		// Token: 0x04000374 RID: 884
		WBEM_S_ACCESS_DENIED,
		// Token: 0x04000375 RID: 885
		WBEM_S_PARTIAL_RESULTS = 262160,
		// Token: 0x04000376 RID: 886
		WBEM_S_NO_POSTHOOK,
		// Token: 0x04000377 RID: 887
		WBEM_S_POSTHOOK_WITH_BOTH,
		// Token: 0x04000378 RID: 888
		WBEM_S_POSTHOOK_WITH_NEW,
		// Token: 0x04000379 RID: 889
		WBEM_S_POSTHOOK_WITH_STATUS,
		// Token: 0x0400037A RID: 890
		WBEM_S_POSTHOOK_WITH_OLD,
		// Token: 0x0400037B RID: 891
		WBEM_S_REDO_PREHOOK_WITH_ORIGINAL_OBJECT,
		// Token: 0x0400037C RID: 892
		WBEM_S_SOURCE_NOT_AVAILABLE,
		// Token: 0x0400037D RID: 893
		WBEM_E_FAILED = -2147217407,
		// Token: 0x0400037E RID: 894
		WBEM_E_NOT_FOUND,
		// Token: 0x0400037F RID: 895
		WBEM_E_ACCESS_DENIED,
		// Token: 0x04000380 RID: 896
		WBEM_E_PROVIDER_FAILURE,
		// Token: 0x04000381 RID: 897
		WBEM_E_TYPE_MISMATCH,
		// Token: 0x04000382 RID: 898
		WBEM_E_OUT_OF_MEMORY,
		// Token: 0x04000383 RID: 899
		WBEM_E_INVALID_CONTEXT,
		// Token: 0x04000384 RID: 900
		WBEM_E_INVALID_PARAMETER,
		// Token: 0x04000385 RID: 901
		WBEM_E_NOT_AVAILABLE,
		// Token: 0x04000386 RID: 902
		WBEM_E_CRITICAL_ERROR,
		// Token: 0x04000387 RID: 903
		WBEM_E_INVALID_STREAM,
		// Token: 0x04000388 RID: 904
		WBEM_E_NOT_SUPPORTED,
		// Token: 0x04000389 RID: 905
		WBEM_E_INVALID_SUPERCLASS,
		// Token: 0x0400038A RID: 906
		WBEM_E_INVALID_NAMESPACE,
		// Token: 0x0400038B RID: 907
		WBEM_E_INVALID_OBJECT,
		// Token: 0x0400038C RID: 908
		WBEM_E_INVALID_CLASS,
		// Token: 0x0400038D RID: 909
		WBEM_E_PROVIDER_NOT_FOUND,
		// Token: 0x0400038E RID: 910
		WBEM_E_INVALID_PROVIDER_REGISTRATION,
		// Token: 0x0400038F RID: 911
		WBEM_E_PROVIDER_LOAD_FAILURE,
		// Token: 0x04000390 RID: 912
		WBEM_E_INITIALIZATION_FAILURE,
		// Token: 0x04000391 RID: 913
		WBEM_E_TRANSPORT_FAILURE,
		// Token: 0x04000392 RID: 914
		WBEM_E_INVALID_OPERATION,
		// Token: 0x04000393 RID: 915
		WBEM_E_INVALID_QUERY,
		// Token: 0x04000394 RID: 916
		WBEM_E_INVALID_QUERY_TYPE,
		// Token: 0x04000395 RID: 917
		WBEM_E_ALREADY_EXISTS,
		// Token: 0x04000396 RID: 918
		WBEM_E_OVERRIDE_NOT_ALLOWED,
		// Token: 0x04000397 RID: 919
		WBEM_E_PROPAGATED_QUALIFIER,
		// Token: 0x04000398 RID: 920
		WBEM_E_PROPAGATED_PROPERTY,
		// Token: 0x04000399 RID: 921
		WBEM_E_UNEXPECTED,
		// Token: 0x0400039A RID: 922
		WBEM_E_ILLEGAL_OPERATION,
		// Token: 0x0400039B RID: 923
		WBEM_E_CANNOT_BE_KEY,
		// Token: 0x0400039C RID: 924
		WBEM_E_INCOMPLETE_CLASS,
		// Token: 0x0400039D RID: 925
		WBEM_E_INVALID_SYNTAX,
		// Token: 0x0400039E RID: 926
		WBEM_E_NONDECORATED_OBJECT,
		// Token: 0x0400039F RID: 927
		WBEM_E_READ_ONLY,
		// Token: 0x040003A0 RID: 928
		WBEM_E_PROVIDER_NOT_CAPABLE,
		// Token: 0x040003A1 RID: 929
		WBEM_E_CLASS_HAS_CHILDREN,
		// Token: 0x040003A2 RID: 930
		WBEM_E_CLASS_HAS_INSTANCES,
		// Token: 0x040003A3 RID: 931
		WBEM_E_QUERY_NOT_IMPLEMENTED,
		// Token: 0x040003A4 RID: 932
		WBEM_E_ILLEGAL_NULL,
		// Token: 0x040003A5 RID: 933
		WBEM_E_INVALID_QUALIFIER_TYPE,
		// Token: 0x040003A6 RID: 934
		WBEM_E_INVALID_PROPERTY_TYPE,
		// Token: 0x040003A7 RID: 935
		WBEM_E_VALUE_OUT_OF_RANGE,
		// Token: 0x040003A8 RID: 936
		WBEM_E_CANNOT_BE_SINGLETON,
		// Token: 0x040003A9 RID: 937
		WBEM_E_INVALID_CIM_TYPE,
		// Token: 0x040003AA RID: 938
		WBEM_E_INVALID_METHOD,
		// Token: 0x040003AB RID: 939
		WBEM_E_INVALID_METHOD_PARAMETERS,
		// Token: 0x040003AC RID: 940
		WBEM_E_SYSTEM_PROPERTY,
		// Token: 0x040003AD RID: 941
		WBEM_E_INVALID_PROPERTY,
		// Token: 0x040003AE RID: 942
		WBEM_E_CALL_CANCELLED,
		// Token: 0x040003AF RID: 943
		WBEM_E_SHUTTING_DOWN,
		// Token: 0x040003B0 RID: 944
		WBEM_E_PROPAGATED_METHOD,
		// Token: 0x040003B1 RID: 945
		WBEM_E_UNSUPPORTED_PARAMETER,
		// Token: 0x040003B2 RID: 946
		WBEM_E_MISSING_PARAMETER_ID,
		// Token: 0x040003B3 RID: 947
		WBEM_E_INVALID_PARAMETER_ID,
		// Token: 0x040003B4 RID: 948
		WBEM_E_NONCONSECUTIVE_PARAMETER_IDS,
		// Token: 0x040003B5 RID: 949
		WBEM_E_PARAMETER_ID_ON_RETVAL,
		// Token: 0x040003B6 RID: 950
		WBEM_E_INVALID_OBJECT_PATH,
		// Token: 0x040003B7 RID: 951
		WBEM_E_OUT_OF_DISK_SPACE,
		// Token: 0x040003B8 RID: 952
		WBEM_E_BUFFER_TOO_SMALL,
		// Token: 0x040003B9 RID: 953
		WBEM_E_UNSUPPORTED_PUT_EXTENSION,
		// Token: 0x040003BA RID: 954
		WBEM_E_UNKNOWN_OBJECT_TYPE,
		// Token: 0x040003BB RID: 955
		WBEM_E_UNKNOWN_PACKET_TYPE,
		// Token: 0x040003BC RID: 956
		WBEM_E_MARSHAL_VERSION_MISMATCH,
		// Token: 0x040003BD RID: 957
		WBEM_E_MARSHAL_INVALID_SIGNATURE,
		// Token: 0x040003BE RID: 958
		WBEM_E_INVALID_QUALIFIER,
		// Token: 0x040003BF RID: 959
		WBEM_E_INVALID_DUPLICATE_PARAMETER,
		// Token: 0x040003C0 RID: 960
		WBEM_E_TOO_MUCH_DATA,
		// Token: 0x040003C1 RID: 961
		WBEM_E_SERVER_TOO_BUSY,
		// Token: 0x040003C2 RID: 962
		WBEM_E_INVALID_FLAVOR,
		// Token: 0x040003C3 RID: 963
		WBEM_E_CIRCULAR_REFERENCE,
		// Token: 0x040003C4 RID: 964
		WBEM_E_UNSUPPORTED_CLASS_UPDATE,
		// Token: 0x040003C5 RID: 965
		WBEM_E_CANNOT_CHANGE_KEY_INHERITANCE,
		// Token: 0x040003C6 RID: 966
		WBEM_E_CANNOT_CHANGE_INDEX_INHERITANCE = -2147217328,
		// Token: 0x040003C7 RID: 967
		WBEM_E_TOO_MANY_PROPERTIES,
		// Token: 0x040003C8 RID: 968
		WBEM_E_UPDATE_TYPE_MISMATCH,
		// Token: 0x040003C9 RID: 969
		WBEM_E_UPDATE_OVERRIDE_NOT_ALLOWED,
		// Token: 0x040003CA RID: 970
		WBEM_E_UPDATE_PROPAGATED_METHOD,
		// Token: 0x040003CB RID: 971
		WBEM_E_METHOD_NOT_IMPLEMENTED,
		// Token: 0x040003CC RID: 972
		WBEM_E_METHOD_DISABLED,
		// Token: 0x040003CD RID: 973
		WBEM_E_REFRESHER_BUSY,
		// Token: 0x040003CE RID: 974
		WBEM_E_UNPARSABLE_QUERY,
		// Token: 0x040003CF RID: 975
		WBEM_E_NOT_EVENT_CLASS,
		// Token: 0x040003D0 RID: 976
		WBEM_E_MISSING_GROUP_WITHIN,
		// Token: 0x040003D1 RID: 977
		WBEM_E_MISSING_AGGREGATION_LIST,
		// Token: 0x040003D2 RID: 978
		WBEM_E_PROPERTY_NOT_AN_OBJECT,
		// Token: 0x040003D3 RID: 979
		WBEM_E_AGGREGATING_BY_OBJECT,
		// Token: 0x040003D4 RID: 980
		WBEM_E_UNINTERPRETABLE_PROVIDER_QUERY = -2147217313,
		// Token: 0x040003D5 RID: 981
		WBEM_E_BACKUP_RESTORE_WINMGMT_RUNNING,
		// Token: 0x040003D6 RID: 982
		WBEM_E_QUEUE_OVERFLOW,
		// Token: 0x040003D7 RID: 983
		WBEM_E_PRIVILEGE_NOT_HELD,
		// Token: 0x040003D8 RID: 984
		WBEM_E_INVALID_OPERATOR,
		// Token: 0x040003D9 RID: 985
		WBEM_E_LOCAL_CREDENTIALS,
		// Token: 0x040003DA RID: 986
		WBEM_E_CANNOT_BE_ABSTRACT,
		// Token: 0x040003DB RID: 987
		WBEM_E_AMENDED_OBJECT,
		// Token: 0x040003DC RID: 988
		WBEM_E_CLIENT_TOO_SLOW,
		// Token: 0x040003DD RID: 989
		WBEM_E_NULL_SECURITY_DESCRIPTOR,
		// Token: 0x040003DE RID: 990
		WBEM_E_TIMED_OUT,
		// Token: 0x040003DF RID: 991
		WBEM_E_INVALID_ASSOCIATION,
		// Token: 0x040003E0 RID: 992
		WBEM_E_AMBIGUOUS_OPERATION,
		// Token: 0x040003E1 RID: 993
		WBEM_E_QUOTA_VIOLATION,
		// Token: 0x040003E2 RID: 994
		WBEM_E_RESERVED_001,
		// Token: 0x040003E3 RID: 995
		WBEM_E_RESERVED_002,
		// Token: 0x040003E4 RID: 996
		WBEM_E_UNSUPPORTED_LOCALE,
		// Token: 0x040003E5 RID: 997
		WBEM_E_HANDLE_OUT_OF_DATE,
		// Token: 0x040003E6 RID: 998
		WBEM_E_CONNECTION_FAILED,
		// Token: 0x040003E7 RID: 999
		WBEM_E_INVALID_HANDLE_REQUEST,
		// Token: 0x040003E8 RID: 1000
		WBEM_E_PROPERTY_NAME_TOO_WIDE,
		// Token: 0x040003E9 RID: 1001
		WBEM_E_CLASS_NAME_TOO_WIDE,
		// Token: 0x040003EA RID: 1002
		WBEM_E_METHOD_NAME_TOO_WIDE,
		// Token: 0x040003EB RID: 1003
		WBEM_E_QUALIFIER_NAME_TOO_WIDE,
		// Token: 0x040003EC RID: 1004
		WBEM_E_RERUN_COMMAND,
		// Token: 0x040003ED RID: 1005
		WBEM_E_DATABASE_VER_MISMATCH,
		// Token: 0x040003EE RID: 1006
		WBEM_E_VETO_DELETE,
		// Token: 0x040003EF RID: 1007
		WBEM_E_VETO_PUT,
		// Token: 0x040003F0 RID: 1008
		WBEM_E_INVALID_LOCALE = -2147217280,
		// Token: 0x040003F1 RID: 1009
		WBEM_E_PROVIDER_SUSPENDED,
		// Token: 0x040003F2 RID: 1010
		WBEM_E_SYNCHRONIZATION_REQUIRED,
		// Token: 0x040003F3 RID: 1011
		WBEM_E_NO_SCHEMA,
		// Token: 0x040003F4 RID: 1012
		WBEM_E_PROVIDER_ALREADY_REGISTERED,
		// Token: 0x040003F5 RID: 1013
		WBEM_E_PROVIDER_NOT_REGISTERED,
		// Token: 0x040003F6 RID: 1014
		WBEM_E_FATAL_TRANSPORT_ERROR,
		// Token: 0x040003F7 RID: 1015
		WBEM_E_ENCRYPTED_CONNECTION_REQUIRED,
		// Token: 0x040003F8 RID: 1016
		WBEM_E_PROVIDER_TIMED_OUT,
		// Token: 0x040003F9 RID: 1017
		WBEM_E_NO_KEY,
		// Token: 0x040003FA RID: 1018
		WBEMESS_E_REGISTRATION_TOO_BROAD = -2147213311,
		// Token: 0x040003FB RID: 1019
		WBEMESS_E_REGISTRATION_TOO_PRECISE,
		// Token: 0x040003FC RID: 1020
		WBEMMOF_E_EXPECTED_QUALIFIER_NAME = -2147205119,
		// Token: 0x040003FD RID: 1021
		WBEMMOF_E_EXPECTED_SEMI,
		// Token: 0x040003FE RID: 1022
		WBEMMOF_E_EXPECTED_OPEN_BRACE,
		// Token: 0x040003FF RID: 1023
		WBEMMOF_E_EXPECTED_CLOSE_BRACE,
		// Token: 0x04000400 RID: 1024
		WBEMMOF_E_EXPECTED_CLOSE_BRACKET,
		// Token: 0x04000401 RID: 1025
		WBEMMOF_E_EXPECTED_CLOSE_PAREN,
		// Token: 0x04000402 RID: 1026
		WBEMMOF_E_ILLEGAL_CONSTANT_VALUE,
		// Token: 0x04000403 RID: 1027
		WBEMMOF_E_EXPECTED_TYPE_IDENTIFIER,
		// Token: 0x04000404 RID: 1028
		WBEMMOF_E_EXPECTED_OPEN_PAREN,
		// Token: 0x04000405 RID: 1029
		WBEMMOF_E_UNRECOGNIZED_TOKEN,
		// Token: 0x04000406 RID: 1030
		WBEMMOF_E_UNRECOGNIZED_TYPE,
		// Token: 0x04000407 RID: 1031
		WBEMMOF_E_EXPECTED_PROPERTY_NAME,
		// Token: 0x04000408 RID: 1032
		WBEMMOF_E_TYPEDEF_NOT_SUPPORTED,
		// Token: 0x04000409 RID: 1033
		WBEMMOF_E_UNEXPECTED_ALIAS,
		// Token: 0x0400040A RID: 1034
		WBEMMOF_E_UNEXPECTED_ARRAY_INIT,
		// Token: 0x0400040B RID: 1035
		WBEMMOF_E_INVALID_AMENDMENT_SYNTAX,
		// Token: 0x0400040C RID: 1036
		WBEMMOF_E_INVALID_DUPLICATE_AMENDMENT,
		// Token: 0x0400040D RID: 1037
		WBEMMOF_E_INVALID_PRAGMA,
		// Token: 0x0400040E RID: 1038
		WBEMMOF_E_INVALID_NAMESPACE_SYNTAX,
		// Token: 0x0400040F RID: 1039
		WBEMMOF_E_EXPECTED_CLASS_NAME,
		// Token: 0x04000410 RID: 1040
		WBEMMOF_E_TYPE_MISMATCH,
		// Token: 0x04000411 RID: 1041
		WBEMMOF_E_EXPECTED_ALIAS_NAME,
		// Token: 0x04000412 RID: 1042
		WBEMMOF_E_INVALID_CLASS_DECLARATION,
		// Token: 0x04000413 RID: 1043
		WBEMMOF_E_INVALID_INSTANCE_DECLARATION,
		// Token: 0x04000414 RID: 1044
		WBEMMOF_E_EXPECTED_DOLLAR,
		// Token: 0x04000415 RID: 1045
		WBEMMOF_E_CIMTYPE_QUALIFIER,
		// Token: 0x04000416 RID: 1046
		WBEMMOF_E_DUPLICATE_PROPERTY,
		// Token: 0x04000417 RID: 1047
		WBEMMOF_E_INVALID_NAMESPACE_SPECIFICATION,
		// Token: 0x04000418 RID: 1048
		WBEMMOF_E_OUT_OF_RANGE,
		// Token: 0x04000419 RID: 1049
		WBEMMOF_E_INVALID_FILE,
		// Token: 0x0400041A RID: 1050
		WBEMMOF_E_ALIASES_IN_EMBEDDED,
		// Token: 0x0400041B RID: 1051
		WBEMMOF_E_NULL_ARRAY_ELEM,
		// Token: 0x0400041C RID: 1052
		WBEMMOF_E_DUPLICATE_QUALIFIER,
		// Token: 0x0400041D RID: 1053
		WBEMMOF_E_EXPECTED_FLAVOR_TYPE,
		// Token: 0x0400041E RID: 1054
		WBEMMOF_E_INCOMPATIBLE_FLAVOR_TYPES,
		// Token: 0x0400041F RID: 1055
		WBEMMOF_E_MULTIPLE_ALIASES,
		// Token: 0x04000420 RID: 1056
		WBEMMOF_E_INCOMPATIBLE_FLAVOR_TYPES2,
		// Token: 0x04000421 RID: 1057
		WBEMMOF_E_NO_ARRAYS_RETURNED,
		// Token: 0x04000422 RID: 1058
		WBEMMOF_E_MUST_BE_IN_OR_OUT,
		// Token: 0x04000423 RID: 1059
		WBEMMOF_E_INVALID_FLAGS_SYNTAX,
		// Token: 0x04000424 RID: 1060
		WBEMMOF_E_EXPECTED_BRACE_OR_BAD_TYPE,
		// Token: 0x04000425 RID: 1061
		WBEMMOF_E_UNSUPPORTED_CIMV22_QUAL_VALUE,
		// Token: 0x04000426 RID: 1062
		WBEMMOF_E_UNSUPPORTED_CIMV22_DATA_TYPE,
		// Token: 0x04000427 RID: 1063
		WBEMMOF_E_INVALID_DELETEINSTANCE_SYNTAX,
		// Token: 0x04000428 RID: 1064
		WBEMMOF_E_INVALID_QUALIFIER_SYNTAX,
		// Token: 0x04000429 RID: 1065
		WBEMMOF_E_QUALIFIER_USED_OUTSIDE_SCOPE,
		// Token: 0x0400042A RID: 1066
		WBEMMOF_E_ERROR_CREATING_TEMP_FILE,
		// Token: 0x0400042B RID: 1067
		WBEMMOF_E_ERROR_INVALID_INCLUDE_FILE,
		// Token: 0x0400042C RID: 1068
		WBEMMOF_E_INVALID_DELETECLASS_SYNTAX
	}
}
