using System;
using System.Globalization;
using System.Resources;
using System.Threading;

namespace System
{
	// Token: 0x02000009 RID: 9
	internal sealed class SR
	{
		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000023 RID: 35 RVA: 0x000023F4 File Offset: 0x000013F4
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

		// Token: 0x06000024 RID: 36 RVA: 0x00002420 File Offset: 0x00001420
		internal SR()
		{
			this.resources = new ResourceManager("System", base.GetType().Assembly);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002444 File Offset: 0x00001444
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

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000026 RID: 38 RVA: 0x00002494 File Offset: 0x00001494
		private static CultureInfo Culture
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00002497 File Offset: 0x00001497
		public static ResourceManager Resources
		{
			get
			{
				return SR.GetLoader().resources;
			}
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000024A4 File Offset: 0x000014A4
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

		// Token: 0x06000029 RID: 41 RVA: 0x00002528 File Offset: 0x00001528
		public static string GetString(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetString(name, SR.Culture);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002554 File Offset: 0x00001554
		public static object GetObject(string name)
		{
			SR sr = SR.GetLoader();
			if (sr == null)
			{
				return null;
			}
			return sr.resources.GetObject(name, SR.Culture);
		}

		// Token: 0x04000048 RID: 72
		internal const string RTL = "RTL";

		// Token: 0x04000049 RID: 73
		internal const string DebugLaunchFailed = "DebugLaunchFailed";

		// Token: 0x0400004A RID: 74
		internal const string DebugLaunchFailedTitle = "DebugLaunchFailedTitle";

		// Token: 0x0400004B RID: 75
		internal const string DebugAssertTitle = "DebugAssertTitle";

		// Token: 0x0400004C RID: 76
		internal const string DebugAssertBanner = "DebugAssertBanner";

		// Token: 0x0400004D RID: 77
		internal const string DebugAssertShortMessage = "DebugAssertShortMessage";

		// Token: 0x0400004E RID: 78
		internal const string DebugAssertLongMessage = "DebugAssertLongMessage";

		// Token: 0x0400004F RID: 79
		internal const string DebugMessageTruncated = "DebugMessageTruncated";

		// Token: 0x04000050 RID: 80
		internal const string ObjectDisposed = "ObjectDisposed";

		// Token: 0x04000051 RID: 81
		internal const string NotSupported = "NotSupported";

		// Token: 0x04000052 RID: 82
		internal const string ExceptionOccurred = "ExceptionOccurred";

		// Token: 0x04000053 RID: 83
		internal const string MustAddListener = "MustAddListener";

		// Token: 0x04000054 RID: 84
		internal const string ToStringNull = "ToStringNull";

		// Token: 0x04000055 RID: 85
		internal const string EnumConverterInvalidValue = "EnumConverterInvalidValue";

		// Token: 0x04000056 RID: 86
		internal const string ConvertFromException = "ConvertFromException";

		// Token: 0x04000057 RID: 87
		internal const string ConvertToException = "ConvertToException";

		// Token: 0x04000058 RID: 88
		internal const string ConvertInvalidPrimitive = "ConvertInvalidPrimitive";

		// Token: 0x04000059 RID: 89
		internal const string ErrorMissingPropertyAccessors = "ErrorMissingPropertyAccessors";

		// Token: 0x0400005A RID: 90
		internal const string ErrorInvalidPropertyType = "ErrorInvalidPropertyType";

		// Token: 0x0400005B RID: 91
		internal const string ErrorMissingEventAccessors = "ErrorMissingEventAccessors";

		// Token: 0x0400005C RID: 92
		internal const string ErrorInvalidEventHandler = "ErrorInvalidEventHandler";

		// Token: 0x0400005D RID: 93
		internal const string ErrorInvalidEventType = "ErrorInvalidEventType";

		// Token: 0x0400005E RID: 94
		internal const string InvalidMemberName = "InvalidMemberName";

		// Token: 0x0400005F RID: 95
		internal const string ErrorBadExtenderType = "ErrorBadExtenderType";

		// Token: 0x04000060 RID: 96
		internal const string NullableConverterBadCtorArg = "NullableConverterBadCtorArg";

		// Token: 0x04000061 RID: 97
		internal const string TypeDescriptorExpectedElementType = "TypeDescriptorExpectedElementType";

		// Token: 0x04000062 RID: 98
		internal const string TypeDescriptorSameAssociation = "TypeDescriptorSameAssociation";

		// Token: 0x04000063 RID: 99
		internal const string TypeDescriptorAlreadyAssociated = "TypeDescriptorAlreadyAssociated";

		// Token: 0x04000064 RID: 100
		internal const string TypeDescriptorProviderError = "TypeDescriptorProviderError";

		// Token: 0x04000065 RID: 101
		internal const string TypeDescriptorUnsupportedRemoteObject = "TypeDescriptorUnsupportedRemoteObject";

		// Token: 0x04000066 RID: 102
		internal const string TypeDescriptorArgsCountMismatch = "TypeDescriptorArgsCountMismatch";

		// Token: 0x04000067 RID: 103
		internal const string ErrorCreateSystemEvents = "ErrorCreateSystemEvents";

		// Token: 0x04000068 RID: 104
		internal const string ErrorCreateTimer = "ErrorCreateTimer";

		// Token: 0x04000069 RID: 105
		internal const string ErrorKillTimer = "ErrorKillTimer";

		// Token: 0x0400006A RID: 106
		internal const string ErrorSystemEventsNotSupported = "ErrorSystemEventsNotSupported";

		// Token: 0x0400006B RID: 107
		internal const string ErrorGetTempPath = "ErrorGetTempPath";

		// Token: 0x0400006C RID: 108
		internal const string CHECKOUTCanceled = "CHECKOUTCanceled";

		// Token: 0x0400006D RID: 109
		internal const string ErrorInvalidServiceInstance = "ErrorInvalidServiceInstance";

		// Token: 0x0400006E RID: 110
		internal const string ErrorServiceExists = "ErrorServiceExists";

		// Token: 0x0400006F RID: 111
		internal const string ArgumentNull_Key = "ArgumentNull_Key";

		// Token: 0x04000070 RID: 112
		internal const string Argument_AddingDuplicate = "Argument_AddingDuplicate";

		// Token: 0x04000071 RID: 113
		internal const string Argument_InvalidValue = "Argument_InvalidValue";

		// Token: 0x04000072 RID: 114
		internal const string ArgumentOutOfRange_NeedNonNegNum = "ArgumentOutOfRange_NeedNonNegNum";

		// Token: 0x04000073 RID: 115
		internal const string ArgumentOutOfRange_InvalidThreshold = "ArgumentOutOfRange_InvalidThreshold";

		// Token: 0x04000074 RID: 116
		internal const string InvalidOperation_EnumFailedVersion = "InvalidOperation_EnumFailedVersion";

		// Token: 0x04000075 RID: 117
		internal const string InvalidOperation_EnumOpCantHappen = "InvalidOperation_EnumOpCantHappen";

		// Token: 0x04000076 RID: 118
		internal const string Arg_MultiRank = "Arg_MultiRank";

		// Token: 0x04000077 RID: 119
		internal const string Arg_NonZeroLowerBound = "Arg_NonZeroLowerBound";

		// Token: 0x04000078 RID: 120
		internal const string Arg_InsufficientSpace = "Arg_InsufficientSpace";

		// Token: 0x04000079 RID: 121
		internal const string NotSupported_EnumeratorReset = "NotSupported_EnumeratorReset";

		// Token: 0x0400007A RID: 122
		internal const string Invalid_Array_Type = "Invalid_Array_Type";

		// Token: 0x0400007B RID: 123
		internal const string Serialization_InvalidOnDeser = "Serialization_InvalidOnDeser";

		// Token: 0x0400007C RID: 124
		internal const string Serialization_MissingValues = "Serialization_MissingValues";

		// Token: 0x0400007D RID: 125
		internal const string Serialization_MismatchedCount = "Serialization_MismatchedCount";

		// Token: 0x0400007E RID: 126
		internal const string ExternalLinkedListNode = "ExternalLinkedListNode";

		// Token: 0x0400007F RID: 127
		internal const string LinkedListNodeIsAttached = "LinkedListNodeIsAttached";

		// Token: 0x04000080 RID: 128
		internal const string LinkedListEmpty = "LinkedListEmpty";

		// Token: 0x04000081 RID: 129
		internal const string Arg_WrongType = "Arg_WrongType";

		// Token: 0x04000082 RID: 130
		internal const string Argument_ItemNotExist = "Argument_ItemNotExist";

		// Token: 0x04000083 RID: 131
		internal const string Argument_ImplementIComparable = "Argument_ImplementIComparable";

		// Token: 0x04000084 RID: 132
		internal const string InvalidOperation_EmptyCollection = "InvalidOperation_EmptyCollection";

		// Token: 0x04000085 RID: 133
		internal const string InvalidOperation_EmptyQueue = "InvalidOperation_EmptyQueue";

		// Token: 0x04000086 RID: 134
		internal const string InvalidOperation_EmptyStack = "InvalidOperation_EmptyStack";

		// Token: 0x04000087 RID: 135
		internal const string InvalidOperation_CannotRemoveFromStackOrQueue = "InvalidOperation_CannotRemoveFromStackOrQueue";

		// Token: 0x04000088 RID: 136
		internal const string ArgumentOutOfRange_Index = "ArgumentOutOfRange_Index";

		// Token: 0x04000089 RID: 137
		internal const string ArgumentOutOfRange_SmallCapacity = "ArgumentOutOfRange_SmallCapacity";

		// Token: 0x0400008A RID: 138
		internal const string Arg_ArrayPlusOffTooSmall = "Arg_ArrayPlusOffTooSmall";

		// Token: 0x0400008B RID: 139
		internal const string NotSupported_KeyCollectionSet = "NotSupported_KeyCollectionSet";

		// Token: 0x0400008C RID: 140
		internal const string NotSupported_ValueCollectionSet = "NotSupported_ValueCollectionSet";

		// Token: 0x0400008D RID: 141
		internal const string NotSupported_ReadOnlyCollection = "NotSupported_ReadOnlyCollection";

		// Token: 0x0400008E RID: 142
		internal const string NotSupported_SortedListNestedWrite = "NotSupported_SortedListNestedWrite";

		// Token: 0x0400008F RID: 143
		internal const string CantModifyListSortDescriptionCollection = "CantModifyListSortDescriptionCollection";

		// Token: 0x04000090 RID: 144
		internal const string InvalidPrimitiveType = "InvalidPrimitiveType";

		// Token: 0x04000091 RID: 145
		internal const string CodeGenOutputWriter = "CodeGenOutputWriter";

		// Token: 0x04000092 RID: 146
		internal const string CodeGenReentrance = "CodeGenReentrance";

		// Token: 0x04000093 RID: 147
		internal const string InvalidLanguageIdentifier = "InvalidLanguageIdentifier";

		// Token: 0x04000094 RID: 148
		internal const string InvalidTypeName = "InvalidTypeName";

		// Token: 0x04000095 RID: 149
		internal const string Empty_attribute = "Empty_attribute";

		// Token: 0x04000096 RID: 150
		internal const string Invalid_nonnegative_integer_attribute = "Invalid_nonnegative_integer_attribute";

		// Token: 0x04000097 RID: 151
		internal const string CodeDomProvider_NotDefined = "CodeDomProvider_NotDefined";

		// Token: 0x04000098 RID: 152
		internal const string Language_Names_Cannot_Be_Empty = "Language_Names_Cannot_Be_Empty";

		// Token: 0x04000099 RID: 153
		internal const string Extension_Names_Cannot_Be_Empty_Or_Non_Period_Based = "Extension_Names_Cannot_Be_Empty_Or_Non_Period_Based";

		// Token: 0x0400009A RID: 154
		internal const string Unable_To_Locate_Type = "Unable_To_Locate_Type";

		// Token: 0x0400009B RID: 155
		internal const string NotSupported_CodeDomAPI = "NotSupported_CodeDomAPI";

		// Token: 0x0400009C RID: 156
		internal const string ArityDoesntMatch = "ArityDoesntMatch";

		// Token: 0x0400009D RID: 157
		internal const string PartialTrustErrorTextReplacement = "PartialTrustErrorTextReplacement";

		// Token: 0x0400009E RID: 158
		internal const string PartialTrustIllegalProvider = "PartialTrustIllegalProvider";

		// Token: 0x0400009F RID: 159
		internal const string IllegalAssemblyReference = "IllegalAssemblyReference";

		// Token: 0x040000A0 RID: 160
		internal const string AutoGen_Comment_Line1 = "AutoGen_Comment_Line1";

		// Token: 0x040000A1 RID: 161
		internal const string AutoGen_Comment_Line2 = "AutoGen_Comment_Line2";

		// Token: 0x040000A2 RID: 162
		internal const string AutoGen_Comment_Line3 = "AutoGen_Comment_Line3";

		// Token: 0x040000A3 RID: 163
		internal const string AutoGen_Comment_Line4 = "AutoGen_Comment_Line4";

		// Token: 0x040000A4 RID: 164
		internal const string AutoGen_Comment_Line5 = "AutoGen_Comment_Line5";

		// Token: 0x040000A5 RID: 165
		internal const string CantContainNullEntries = "CantContainNullEntries";

		// Token: 0x040000A6 RID: 166
		internal const string InvalidPathCharsInChecksum = "InvalidPathCharsInChecksum";

		// Token: 0x040000A7 RID: 167
		internal const string InvalidRegion = "InvalidRegion";

		// Token: 0x040000A8 RID: 168
		internal const string MetaExtenderName = "MetaExtenderName";

		// Token: 0x040000A9 RID: 169
		internal const string InvalidEnumArgument = "InvalidEnumArgument";

		// Token: 0x040000AA RID: 170
		internal const string InvalidArgument = "InvalidArgument";

		// Token: 0x040000AB RID: 171
		internal const string InvalidNullArgument = "InvalidNullArgument";

		// Token: 0x040000AC RID: 172
		internal const string InvalidNullEmptyArgument = "InvalidNullEmptyArgument";

		// Token: 0x040000AD RID: 173
		internal const string LicExceptionTypeOnly = "LicExceptionTypeOnly";

		// Token: 0x040000AE RID: 174
		internal const string LicExceptionTypeAndInstance = "LicExceptionTypeAndInstance";

		// Token: 0x040000AF RID: 175
		internal const string LicMgrContextCannotBeChanged = "LicMgrContextCannotBeChanged";

		// Token: 0x040000B0 RID: 176
		internal const string LicMgrAlreadyLocked = "LicMgrAlreadyLocked";

		// Token: 0x040000B1 RID: 177
		internal const string LicMgrDifferentUser = "LicMgrDifferentUser";

		// Token: 0x040000B2 RID: 178
		internal const string InvalidElementType = "InvalidElementType";

		// Token: 0x040000B3 RID: 179
		internal const string InvalidIdentifier = "InvalidIdentifier";

		// Token: 0x040000B4 RID: 180
		internal const string ExecFailedToCreate = "ExecFailedToCreate";

		// Token: 0x040000B5 RID: 181
		internal const string ExecTimeout = "ExecTimeout";

		// Token: 0x040000B6 RID: 182
		internal const string ExecBadreturn = "ExecBadreturn";

		// Token: 0x040000B7 RID: 183
		internal const string ExecCantGetRetCode = "ExecCantGetRetCode";

		// Token: 0x040000B8 RID: 184
		internal const string ExecCantExec = "ExecCantExec";

		// Token: 0x040000B9 RID: 185
		internal const string ExecCantRevert = "ExecCantRevert";

		// Token: 0x040000BA RID: 186
		internal const string CompilerNotFound = "CompilerNotFound";

		// Token: 0x040000BB RID: 187
		internal const string DuplicateFileName = "DuplicateFileName";

		// Token: 0x040000BC RID: 188
		internal const string CollectionReadOnly = "CollectionReadOnly";

		// Token: 0x040000BD RID: 189
		internal const string BitVectorFull = "BitVectorFull";

		// Token: 0x040000BE RID: 190
		internal const string ISupportInitializeDescr = "ISupportInitializeDescr";

		// Token: 0x040000BF RID: 191
		internal const string ArrayConverterText = "ArrayConverterText";

		// Token: 0x040000C0 RID: 192
		internal const string CollectionConverterText = "CollectionConverterText";

		// Token: 0x040000C1 RID: 193
		internal const string MultilineStringConverterText = "MultilineStringConverterText";

		// Token: 0x040000C2 RID: 194
		internal const string CultureInfoConverterDefaultCultureString = "CultureInfoConverterDefaultCultureString";

		// Token: 0x040000C3 RID: 195
		internal const string CultureInfoConverterInvalidCulture = "CultureInfoConverterInvalidCulture";

		// Token: 0x040000C4 RID: 196
		internal const string InvalidPrimitive = "InvalidPrimitive";

		// Token: 0x040000C5 RID: 197
		internal const string TimerInvalidInterval = "TimerInvalidInterval";

		// Token: 0x040000C6 RID: 198
		internal const string TraceSwitchLevelTooHigh = "TraceSwitchLevelTooHigh";

		// Token: 0x040000C7 RID: 199
		internal const string TraceSwitchLevelTooLow = "TraceSwitchLevelTooLow";

		// Token: 0x040000C8 RID: 200
		internal const string TraceSwitchInvalidLevel = "TraceSwitchInvalidLevel";

		// Token: 0x040000C9 RID: 201
		internal const string TraceListenerIndentSize = "TraceListenerIndentSize";

		// Token: 0x040000CA RID: 202
		internal const string TraceListenerFail = "TraceListenerFail";

		// Token: 0x040000CB RID: 203
		internal const string TraceAsTraceSource = "TraceAsTraceSource";

		// Token: 0x040000CC RID: 204
		internal const string InvalidLowBoundArgument = "InvalidLowBoundArgument";

		// Token: 0x040000CD RID: 205
		internal const string DuplicateComponentName = "DuplicateComponentName";

		// Token: 0x040000CE RID: 206
		internal const string NotImplemented = "NotImplemented";

		// Token: 0x040000CF RID: 207
		internal const string OutOfMemory = "OutOfMemory";

		// Token: 0x040000D0 RID: 208
		internal const string EOF = "EOF";

		// Token: 0x040000D1 RID: 209
		internal const string IOError = "IOError";

		// Token: 0x040000D2 RID: 210
		internal const string BadChar = "BadChar";

		// Token: 0x040000D3 RID: 211
		internal const string toStringNone = "toStringNone";

		// Token: 0x040000D4 RID: 212
		internal const string toStringUnknown = "toStringUnknown";

		// Token: 0x040000D5 RID: 213
		internal const string InvalidEnum = "InvalidEnum";

		// Token: 0x040000D6 RID: 214
		internal const string IndexOutOfRange = "IndexOutOfRange";

		// Token: 0x040000D7 RID: 215
		internal const string ErrorPropertyAccessorException = "ErrorPropertyAccessorException";

		// Token: 0x040000D8 RID: 216
		internal const string InvalidOperation = "InvalidOperation";

		// Token: 0x040000D9 RID: 217
		internal const string EmptyStack = "EmptyStack";

		// Token: 0x040000DA RID: 218
		internal const string PerformanceCounterDesc = "PerformanceCounterDesc";

		// Token: 0x040000DB RID: 219
		internal const string PCCategoryName = "PCCategoryName";

		// Token: 0x040000DC RID: 220
		internal const string PCCounterName = "PCCounterName";

		// Token: 0x040000DD RID: 221
		internal const string PCInstanceName = "PCInstanceName";

		// Token: 0x040000DE RID: 222
		internal const string PCMachineName = "PCMachineName";

		// Token: 0x040000DF RID: 223
		internal const string PCInstanceLifetime = "PCInstanceLifetime";

		// Token: 0x040000E0 RID: 224
		internal const string PropertyCategoryAction = "PropertyCategoryAction";

		// Token: 0x040000E1 RID: 225
		internal const string PropertyCategoryAppearance = "PropertyCategoryAppearance";

		// Token: 0x040000E2 RID: 226
		internal const string PropertyCategoryAsynchronous = "PropertyCategoryAsynchronous";

		// Token: 0x040000E3 RID: 227
		internal const string PropertyCategoryBehavior = "PropertyCategoryBehavior";

		// Token: 0x040000E4 RID: 228
		internal const string PropertyCategoryData = "PropertyCategoryData";

		// Token: 0x040000E5 RID: 229
		internal const string PropertyCategoryDDE = "PropertyCategoryDDE";

		// Token: 0x040000E6 RID: 230
		internal const string PropertyCategoryDesign = "PropertyCategoryDesign";

		// Token: 0x040000E7 RID: 231
		internal const string PropertyCategoryDragDrop = "PropertyCategoryDragDrop";

		// Token: 0x040000E8 RID: 232
		internal const string PropertyCategoryFocus = "PropertyCategoryFocus";

		// Token: 0x040000E9 RID: 233
		internal const string PropertyCategoryFont = "PropertyCategoryFont";

		// Token: 0x040000EA RID: 234
		internal const string PropertyCategoryFormat = "PropertyCategoryFormat";

		// Token: 0x040000EB RID: 235
		internal const string PropertyCategoryKey = "PropertyCategoryKey";

		// Token: 0x040000EC RID: 236
		internal const string PropertyCategoryList = "PropertyCategoryList";

		// Token: 0x040000ED RID: 237
		internal const string PropertyCategoryLayout = "PropertyCategoryLayout";

		// Token: 0x040000EE RID: 238
		internal const string PropertyCategoryDefault = "PropertyCategoryDefault";

		// Token: 0x040000EF RID: 239
		internal const string PropertyCategoryMouse = "PropertyCategoryMouse";

		// Token: 0x040000F0 RID: 240
		internal const string PropertyCategoryPosition = "PropertyCategoryPosition";

		// Token: 0x040000F1 RID: 241
		internal const string PropertyCategoryText = "PropertyCategoryText";

		// Token: 0x040000F2 RID: 242
		internal const string PropertyCategoryScale = "PropertyCategoryScale";

		// Token: 0x040000F3 RID: 243
		internal const string PropertyCategoryWindowStyle = "PropertyCategoryWindowStyle";

		// Token: 0x040000F4 RID: 244
		internal const string PropertyCategoryConfig = "PropertyCategoryConfig";

		// Token: 0x040000F5 RID: 245
		internal const string OnlyAllowedOnce = "OnlyAllowedOnce";

		// Token: 0x040000F6 RID: 246
		internal const string BeginIndexNotNegative = "BeginIndexNotNegative";

		// Token: 0x040000F7 RID: 247
		internal const string LengthNotNegative = "LengthNotNegative";

		// Token: 0x040000F8 RID: 248
		internal const string UnimplementedState = "UnimplementedState";

		// Token: 0x040000F9 RID: 249
		internal const string UnexpectedOpcode = "UnexpectedOpcode";

		// Token: 0x040000FA RID: 250
		internal const string NoResultOnFailed = "NoResultOnFailed";

		// Token: 0x040000FB RID: 251
		internal const string UnterminatedBracket = "UnterminatedBracket";

		// Token: 0x040000FC RID: 252
		internal const string TooManyParens = "TooManyParens";

		// Token: 0x040000FD RID: 253
		internal const string NestedQuantify = "NestedQuantify";

		// Token: 0x040000FE RID: 254
		internal const string QuantifyAfterNothing = "QuantifyAfterNothing";

		// Token: 0x040000FF RID: 255
		internal const string InternalError = "InternalError";

		// Token: 0x04000100 RID: 256
		internal const string IllegalRange = "IllegalRange";

		// Token: 0x04000101 RID: 257
		internal const string NotEnoughParens = "NotEnoughParens";

		// Token: 0x04000102 RID: 258
		internal const string BadClassInCharRange = "BadClassInCharRange";

		// Token: 0x04000103 RID: 259
		internal const string ReversedCharRange = "ReversedCharRange";

		// Token: 0x04000104 RID: 260
		internal const string UndefinedReference = "UndefinedReference";

		// Token: 0x04000105 RID: 261
		internal const string MalformedReference = "MalformedReference";

		// Token: 0x04000106 RID: 262
		internal const string UnrecognizedGrouping = "UnrecognizedGrouping";

		// Token: 0x04000107 RID: 263
		internal const string UnterminatedComment = "UnterminatedComment";

		// Token: 0x04000108 RID: 264
		internal const string IllegalEndEscape = "IllegalEndEscape";

		// Token: 0x04000109 RID: 265
		internal const string MalformedNameRef = "MalformedNameRef";

		// Token: 0x0400010A RID: 266
		internal const string UndefinedBackref = "UndefinedBackref";

		// Token: 0x0400010B RID: 267
		internal const string UndefinedNameRef = "UndefinedNameRef";

		// Token: 0x0400010C RID: 268
		internal const string TooFewHex = "TooFewHex";

		// Token: 0x0400010D RID: 269
		internal const string MissingControl = "MissingControl";

		// Token: 0x0400010E RID: 270
		internal const string UnrecognizedControl = "UnrecognizedControl";

		// Token: 0x0400010F RID: 271
		internal const string UnrecognizedEscape = "UnrecognizedEscape";

		// Token: 0x04000110 RID: 272
		internal const string IllegalCondition = "IllegalCondition";

		// Token: 0x04000111 RID: 273
		internal const string TooManyAlternates = "TooManyAlternates";

		// Token: 0x04000112 RID: 274
		internal const string MakeException = "MakeException";

		// Token: 0x04000113 RID: 275
		internal const string IncompleteSlashP = "IncompleteSlashP";

		// Token: 0x04000114 RID: 276
		internal const string MalformedSlashP = "MalformedSlashP";

		// Token: 0x04000115 RID: 277
		internal const string InvalidGroupName = "InvalidGroupName";

		// Token: 0x04000116 RID: 278
		internal const string CapnumNotZero = "CapnumNotZero";

		// Token: 0x04000117 RID: 279
		internal const string AlternationCantCapture = "AlternationCantCapture";

		// Token: 0x04000118 RID: 280
		internal const string AlternationCantHaveComment = "AlternationCantHaveComment";

		// Token: 0x04000119 RID: 281
		internal const string CaptureGroupOutOfRange = "CaptureGroupOutOfRange";

		// Token: 0x0400011A RID: 282
		internal const string SubtractionMustBeLast = "SubtractionMustBeLast";

		// Token: 0x0400011B RID: 283
		internal const string UnknownProperty = "UnknownProperty";

		// Token: 0x0400011C RID: 284
		internal const string ReplacementError = "ReplacementError";

		// Token: 0x0400011D RID: 285
		internal const string CountTooSmall = "CountTooSmall";

		// Token: 0x0400011E RID: 286
		internal const string EnumNotStarted = "EnumNotStarted";

		// Token: 0x0400011F RID: 287
		internal const string FileObject_AlreadyOpen = "FileObject_AlreadyOpen";

		// Token: 0x04000120 RID: 288
		internal const string FileObject_Closed = "FileObject_Closed";

		// Token: 0x04000121 RID: 289
		internal const string FileObject_NotWhileWriting = "FileObject_NotWhileWriting";

		// Token: 0x04000122 RID: 290
		internal const string FileObject_FileDoesNotExist = "FileObject_FileDoesNotExist";

		// Token: 0x04000123 RID: 291
		internal const string FileObject_MustBeClosed = "FileObject_MustBeClosed";

		// Token: 0x04000124 RID: 292
		internal const string FileObject_MustBeFileName = "FileObject_MustBeFileName";

		// Token: 0x04000125 RID: 293
		internal const string FileObject_InvalidInternalState = "FileObject_InvalidInternalState";

		// Token: 0x04000126 RID: 294
		internal const string FileObject_PathNotSet = "FileObject_PathNotSet";

		// Token: 0x04000127 RID: 295
		internal const string FileObject_Reading = "FileObject_Reading";

		// Token: 0x04000128 RID: 296
		internal const string FileObject_Writing = "FileObject_Writing";

		// Token: 0x04000129 RID: 297
		internal const string FileObject_InvalidEnumeration = "FileObject_InvalidEnumeration";

		// Token: 0x0400012A RID: 298
		internal const string FileObject_NoReset = "FileObject_NoReset";

		// Token: 0x0400012B RID: 299
		internal const string DirectoryObject_MustBeDirName = "DirectoryObject_MustBeDirName";

		// Token: 0x0400012C RID: 300
		internal const string DirectoryObjectPathDescr = "DirectoryObjectPathDescr";

		// Token: 0x0400012D RID: 301
		internal const string FileObjectDetectEncodingDescr = "FileObjectDetectEncodingDescr";

		// Token: 0x0400012E RID: 302
		internal const string FileObjectEncodingDescr = "FileObjectEncodingDescr";

		// Token: 0x0400012F RID: 303
		internal const string FileObjectPathDescr = "FileObjectPathDescr";

		// Token: 0x04000130 RID: 304
		internal const string Arg_RankMultiDimNotSupported = "Arg_RankMultiDimNotSupported";

		// Token: 0x04000131 RID: 305
		internal const string Arg_EnumIllegalVal = "Arg_EnumIllegalVal";

		// Token: 0x04000132 RID: 306
		internal const string Arg_OutOfRange_NeedNonNegNum = "Arg_OutOfRange_NeedNonNegNum";

		// Token: 0x04000133 RID: 307
		internal const string Argument_InvalidPermissionState = "Argument_InvalidPermissionState";

		// Token: 0x04000134 RID: 308
		internal const string Argument_InvalidOidValue = "Argument_InvalidOidValue";

		// Token: 0x04000135 RID: 309
		internal const string Argument_WrongType = "Argument_WrongType";

		// Token: 0x04000136 RID: 310
		internal const string Arg_EmptyOrNullString = "Arg_EmptyOrNullString";

		// Token: 0x04000137 RID: 311
		internal const string Arg_EmptyOrNullArray = "Arg_EmptyOrNullArray";

		// Token: 0x04000138 RID: 312
		internal const string Argument_InvalidClassAttribute = "Argument_InvalidClassAttribute";

		// Token: 0x04000139 RID: 313
		internal const string Argument_InvalidNameType = "Argument_InvalidNameType";

		// Token: 0x0400013A RID: 314
		internal const string InvalidOperation_EnumNotStarted = "InvalidOperation_EnumNotStarted";

		// Token: 0x0400013B RID: 315
		internal const string InvalidOperation_DuplicateItemNotAllowed = "InvalidOperation_DuplicateItemNotAllowed";

		// Token: 0x0400013C RID: 316
		internal const string Cryptography_Asn_MismatchedOidInCollection = "Cryptography_Asn_MismatchedOidInCollection";

		// Token: 0x0400013D RID: 317
		internal const string Cryptography_Cms_Envelope_Empty_Content = "Cryptography_Cms_Envelope_Empty_Content";

		// Token: 0x0400013E RID: 318
		internal const string Cryptography_Cms_Invalid_Recipient_Info_Type = "Cryptography_Cms_Invalid_Recipient_Info_Type";

		// Token: 0x0400013F RID: 319
		internal const string Cryptography_Cms_Invalid_Subject_Identifier_Type = "Cryptography_Cms_Invalid_Subject_Identifier_Type";

		// Token: 0x04000140 RID: 320
		internal const string Cryptography_Cms_Invalid_Subject_Identifier_Type_Value_Mismatch = "Cryptography_Cms_Invalid_Subject_Identifier_Type_Value_Mismatch";

		// Token: 0x04000141 RID: 321
		internal const string Cryptography_Cms_Key_Agree_Date_Not_Available = "Cryptography_Cms_Key_Agree_Date_Not_Available";

		// Token: 0x04000142 RID: 322
		internal const string Cryptography_Cms_Key_Agree_Other_Key_Attribute_Not_Available = "Cryptography_Cms_Key_Agree_Other_Key_Attribute_Not_Available";

		// Token: 0x04000143 RID: 323
		internal const string Cryptography_Cms_MessageNotSigned = "Cryptography_Cms_MessageNotSigned";

		// Token: 0x04000144 RID: 324
		internal const string Cryptography_Cms_MessageNotSignedByNoSignature = "Cryptography_Cms_MessageNotSignedByNoSignature";

		// Token: 0x04000145 RID: 325
		internal const string Cryptography_Cms_MessageNotEncrypted = "Cryptography_Cms_MessageNotEncrypted";

		// Token: 0x04000146 RID: 326
		internal const string Cryptography_Cms_Not_Supported = "Cryptography_Cms_Not_Supported";

		// Token: 0x04000147 RID: 327
		internal const string Cryptography_Cms_RecipientCertificateNotFound = "Cryptography_Cms_RecipientCertificateNotFound";

		// Token: 0x04000148 RID: 328
		internal const string Cryptography_Cms_Sign_Empty_Content = "Cryptography_Cms_Sign_Empty_Content";

		// Token: 0x04000149 RID: 329
		internal const string Cryptography_Cms_Sign_No_Signature_First_Signer = "Cryptography_Cms_Sign_No_Signature_First_Signer";

		// Token: 0x0400014A RID: 330
		internal const string Cryptography_DpApi_InvalidMemoryLength = "Cryptography_DpApi_InvalidMemoryLength";

		// Token: 0x0400014B RID: 331
		internal const string Cryptography_InvalidHandle = "Cryptography_InvalidHandle";

		// Token: 0x0400014C RID: 332
		internal const string Cryptography_InvalidContextHandle = "Cryptography_InvalidContextHandle";

		// Token: 0x0400014D RID: 333
		internal const string Cryptography_InvalidStoreHandle = "Cryptography_InvalidStoreHandle";

		// Token: 0x0400014E RID: 334
		internal const string Cryptography_Oid_InvalidValue = "Cryptography_Oid_InvalidValue";

		// Token: 0x0400014F RID: 335
		internal const string Cryptography_Pkcs9_ExplicitAddNotAllowed = "Cryptography_Pkcs9_ExplicitAddNotAllowed";

		// Token: 0x04000150 RID: 336
		internal const string Cryptography_Pkcs9_InvalidOid = "Cryptography_Pkcs9_InvalidOid";

		// Token: 0x04000151 RID: 337
		internal const string Cryptography_Pkcs9_MultipleSigningTimeNotAllowed = "Cryptography_Pkcs9_MultipleSigningTimeNotAllowed";

		// Token: 0x04000152 RID: 338
		internal const string Cryptography_Pkcs9_AttributeMismatch = "Cryptography_Pkcs9_AttributeMismatch";

		// Token: 0x04000153 RID: 339
		internal const string Cryptography_X509_AddFailed = "Cryptography_X509_AddFailed";

		// Token: 0x04000154 RID: 340
		internal const string Cryptography_X509_BadEncoding = "Cryptography_X509_BadEncoding";

		// Token: 0x04000155 RID: 341
		internal const string Cryptography_X509_ExportFailed = "Cryptography_X509_ExportFailed";

		// Token: 0x04000156 RID: 342
		internal const string Cryptography_X509_ExtensionMismatch = "Cryptography_X509_ExtensionMismatch";

		// Token: 0x04000157 RID: 343
		internal const string Cryptography_X509_InvalidFindType = "Cryptography_X509_InvalidFindType";

		// Token: 0x04000158 RID: 344
		internal const string Cryptography_X509_InvalidFindValue = "Cryptography_X509_InvalidFindValue";

		// Token: 0x04000159 RID: 345
		internal const string Cryptography_X509_InvalidEncodingFormat = "Cryptography_X509_InvalidEncodingFormat";

		// Token: 0x0400015A RID: 346
		internal const string Cryptography_X509_InvalidContentType = "Cryptography_X509_InvalidContentType";

		// Token: 0x0400015B RID: 347
		internal const string Cryptography_X509_KeyMismatch = "Cryptography_X509_KeyMismatch";

		// Token: 0x0400015C RID: 348
		internal const string Cryptography_X509_RemoveFailed = "Cryptography_X509_RemoveFailed";

		// Token: 0x0400015D RID: 349
		internal const string Environment_NotInteractive = "Environment_NotInteractive";

		// Token: 0x0400015E RID: 350
		internal const string NotSupported_InvalidKeyImpl = "NotSupported_InvalidKeyImpl";

		// Token: 0x0400015F RID: 351
		internal const string NotSupported_KeyAlgorithm = "NotSupported_KeyAlgorithm";

		// Token: 0x04000160 RID: 352
		internal const string NotSupported_PlatformRequiresNT = "NotSupported_PlatformRequiresNT";

		// Token: 0x04000161 RID: 353
		internal const string NotSupported_UnreadableStream = "NotSupported_UnreadableStream";

		// Token: 0x04000162 RID: 354
		internal const string Security_InvalidValue = "Security_InvalidValue";

		// Token: 0x04000163 RID: 355
		internal const string Unknown_Error = "Unknown_Error";

		// Token: 0x04000164 RID: 356
		internal const string security_ServiceNameCollection_EmptyServiceName = "security_ServiceNameCollection_EmptyServiceName";

		// Token: 0x04000165 RID: 357
		internal const string security_ExtendedProtectionPolicy_UseDifferentConstructorForNever = "security_ExtendedProtectionPolicy_UseDifferentConstructorForNever";

		// Token: 0x04000166 RID: 358
		internal const string security_ExtendedProtectionPolicy_NoEmptyServiceNameCollection = "security_ExtendedProtectionPolicy_NoEmptyServiceNameCollection";

		// Token: 0x04000167 RID: 359
		internal const string security_ExtendedProtection_NoOSSupport = "security_ExtendedProtection_NoOSSupport";

		// Token: 0x04000168 RID: 360
		internal const string net_nonClsCompliantException = "net_nonClsCompliantException";

		// Token: 0x04000169 RID: 361
		internal const string net_illegalConfigWith = "net_illegalConfigWith";

		// Token: 0x0400016A RID: 362
		internal const string net_illegalConfigWithout = "net_illegalConfigWithout";

		// Token: 0x0400016B RID: 363
		internal const string net_baddate = "net_baddate";

		// Token: 0x0400016C RID: 364
		internal const string net_writestarted = "net_writestarted";

		// Token: 0x0400016D RID: 365
		internal const string net_clsmall = "net_clsmall";

		// Token: 0x0400016E RID: 366
		internal const string net_reqsubmitted = "net_reqsubmitted";

		// Token: 0x0400016F RID: 367
		internal const string net_rspsubmitted = "net_rspsubmitted";

		// Token: 0x04000170 RID: 368
		internal const string net_ftp_no_http_cmd = "net_ftp_no_http_cmd";

		// Token: 0x04000171 RID: 369
		internal const string net_ftp_invalid_method_name = "net_ftp_invalid_method_name";

		// Token: 0x04000172 RID: 370
		internal const string net_ftp_invalid_renameto = "net_ftp_invalid_renameto";

		// Token: 0x04000173 RID: 371
		internal const string net_ftp_no_defaultcreds = "net_ftp_no_defaultcreds";

		// Token: 0x04000174 RID: 372
		internal const string net_ftpnoresponse = "net_ftpnoresponse";

		// Token: 0x04000175 RID: 373
		internal const string net_ftp_response_invalid_format = "net_ftp_response_invalid_format";

		// Token: 0x04000176 RID: 374
		internal const string net_ftp_no_offsetforhttp = "net_ftp_no_offsetforhttp";

		// Token: 0x04000177 RID: 375
		internal const string net_ftp_invalid_uri = "net_ftp_invalid_uri";

		// Token: 0x04000178 RID: 376
		internal const string net_ftp_invalid_status_response = "net_ftp_invalid_status_response";

		// Token: 0x04000179 RID: 377
		internal const string net_ftp_server_failed_passive = "net_ftp_server_failed_passive";

		// Token: 0x0400017A RID: 378
		internal const string net_ftp_passive_address_different = "net_ftp_passive_address_different";

		// Token: 0x0400017B RID: 379
		internal const string net_ftp_active_address_different = "net_ftp_active_address_different";

		// Token: 0x0400017C RID: 380
		internal const string net_ftp_proxy_does_not_support_ssl = "net_ftp_proxy_does_not_support_ssl";

		// Token: 0x0400017D RID: 381
		internal const string net_ftp_invalid_response_filename = "net_ftp_invalid_response_filename";

		// Token: 0x0400017E RID: 382
		internal const string net_ftp_unsupported_method = "net_ftp_unsupported_method";

		// Token: 0x0400017F RID: 383
		internal const string net_resubmitcanceled = "net_resubmitcanceled";

		// Token: 0x04000180 RID: 384
		internal const string net_redirect_perm = "net_redirect_perm";

		// Token: 0x04000181 RID: 385
		internal const string net_resubmitprotofailed = "net_resubmitprotofailed";

		// Token: 0x04000182 RID: 386
		internal const string net_needchunked = "net_needchunked";

		// Token: 0x04000183 RID: 387
		internal const string net_nochunked = "net_nochunked";

		// Token: 0x04000184 RID: 388
		internal const string net_nochunkuploadonhttp10 = "net_nochunkuploadonhttp10";

		// Token: 0x04000185 RID: 389
		internal const string net_connarg = "net_connarg";

		// Token: 0x04000186 RID: 390
		internal const string net_no100 = "net_no100";

		// Token: 0x04000187 RID: 391
		internal const string net_fromto = "net_fromto";

		// Token: 0x04000188 RID: 392
		internal const string net_rangetoosmall = "net_rangetoosmall";

		// Token: 0x04000189 RID: 393
		internal const string net_entitytoobig = "net_entitytoobig";

		// Token: 0x0400018A RID: 394
		internal const string net_invalidversion = "net_invalidversion";

		// Token: 0x0400018B RID: 395
		internal const string net_invalidstatus = "net_invalidstatus";

		// Token: 0x0400018C RID: 396
		internal const string net_toosmall = "net_toosmall";

		// Token: 0x0400018D RID: 397
		internal const string net_toolong = "net_toolong";

		// Token: 0x0400018E RID: 398
		internal const string net_connclosed = "net_connclosed";

		// Token: 0x0400018F RID: 399
		internal const string net_headerrestrict = "net_headerrestrict";

		// Token: 0x04000190 RID: 400
		internal const string net_headerrestrict_resp = "net_headerrestrict_resp";

		// Token: 0x04000191 RID: 401
		internal const string net_noseek = "net_noseek";

		// Token: 0x04000192 RID: 402
		internal const string net_servererror = "net_servererror";

		// Token: 0x04000193 RID: 403
		internal const string net_nouploadonget = "net_nouploadonget";

		// Token: 0x04000194 RID: 404
		internal const string net_mutualauthfailed = "net_mutualauthfailed";

		// Token: 0x04000195 RID: 405
		internal const string net_invasync = "net_invasync";

		// Token: 0x04000196 RID: 406
		internal const string net_inasync = "net_inasync";

		// Token: 0x04000197 RID: 407
		internal const string net_mustbeuri = "net_mustbeuri";

		// Token: 0x04000198 RID: 408
		internal const string net_format_shexp = "net_format_shexp";

		// Token: 0x04000199 RID: 409
		internal const string net_cannot_load_proxy_helper = "net_cannot_load_proxy_helper";

		// Token: 0x0400019A RID: 410
		internal const string net_repcall = "net_repcall";

		// Token: 0x0400019B RID: 411
		internal const string net_wrongversion = "net_wrongversion";

		// Token: 0x0400019C RID: 412
		internal const string net_badmethod = "net_badmethod";

		// Token: 0x0400019D RID: 413
		internal const string net_io_notenoughbyteswritten = "net_io_notenoughbyteswritten";

		// Token: 0x0400019E RID: 414
		internal const string net_io_timeout_use_ge_zero = "net_io_timeout_use_ge_zero";

		// Token: 0x0400019F RID: 415
		internal const string net_io_timeout_use_gt_zero = "net_io_timeout_use_gt_zero";

		// Token: 0x040001A0 RID: 416
		internal const string net_io_no_0timeouts = "net_io_no_0timeouts";

		// Token: 0x040001A1 RID: 417
		internal const string net_requestaborted = "net_requestaborted";

		// Token: 0x040001A2 RID: 418
		internal const string net_tooManyRedirections = "net_tooManyRedirections";

		// Token: 0x040001A3 RID: 419
		internal const string net_authmodulenotregistered = "net_authmodulenotregistered";

		// Token: 0x040001A4 RID: 420
		internal const string net_authschemenotregistered = "net_authschemenotregistered";

		// Token: 0x040001A5 RID: 421
		internal const string net_proxyschemenotsupported = "net_proxyschemenotsupported";

		// Token: 0x040001A6 RID: 422
		internal const string net_maxsrvpoints = "net_maxsrvpoints";

		// Token: 0x040001A7 RID: 423
		internal const string net_maxbinddelegateretry = "net_maxbinddelegateretry";

		// Token: 0x040001A8 RID: 424
		internal const string net_unknown_prefix = "net_unknown_prefix";

		// Token: 0x040001A9 RID: 425
		internal const string net_notconnected = "net_notconnected";

		// Token: 0x040001AA RID: 426
		internal const string net_notstream = "net_notstream";

		// Token: 0x040001AB RID: 427
		internal const string net_timeout = "net_timeout";

		// Token: 0x040001AC RID: 428
		internal const string net_nocontentlengthonget = "net_nocontentlengthonget";

		// Token: 0x040001AD RID: 429
		internal const string net_contentlengthmissing = "net_contentlengthmissing";

		// Token: 0x040001AE RID: 430
		internal const string net_nonhttpproxynotallowed = "net_nonhttpproxynotallowed";

		// Token: 0x040001AF RID: 431
		internal const string net_nottoken = "net_nottoken";

		// Token: 0x040001B0 RID: 432
		internal const string net_rangetype = "net_rangetype";

		// Token: 0x040001B1 RID: 433
		internal const string net_need_writebuffering = "net_need_writebuffering";

		// Token: 0x040001B2 RID: 434
		internal const string net_securitypackagesupport = "net_securitypackagesupport";

		// Token: 0x040001B3 RID: 435
		internal const string net_securityprotocolnotsupported = "net_securityprotocolnotsupported";

		// Token: 0x040001B4 RID: 436
		internal const string net_nodefaultcreds = "net_nodefaultcreds";

		// Token: 0x040001B5 RID: 437
		internal const string net_stopped = "net_stopped";

		// Token: 0x040001B6 RID: 438
		internal const string net_udpconnected = "net_udpconnected";

		// Token: 0x040001B7 RID: 439
		internal const string net_readonlystream = "net_readonlystream";

		// Token: 0x040001B8 RID: 440
		internal const string net_writeonlystream = "net_writeonlystream";

		// Token: 0x040001B9 RID: 441
		internal const string net_no_concurrent_io_allowed = "net_no_concurrent_io_allowed";

		// Token: 0x040001BA RID: 442
		internal const string net_needmorethreads = "net_needmorethreads";

		// Token: 0x040001BB RID: 443
		internal const string net_MethodNotImplementedException = "net_MethodNotImplementedException";

		// Token: 0x040001BC RID: 444
		internal const string net_PropertyNotImplementedException = "net_PropertyNotImplementedException";

		// Token: 0x040001BD RID: 445
		internal const string net_MethodNotSupportedException = "net_MethodNotSupportedException";

		// Token: 0x040001BE RID: 446
		internal const string net_PropertyNotSupportedException = "net_PropertyNotSupportedException";

		// Token: 0x040001BF RID: 447
		internal const string net_ProtocolNotSupportedException = "net_ProtocolNotSupportedException";

		// Token: 0x040001C0 RID: 448
		internal const string net_HashAlgorithmNotSupportedException = "net_HashAlgorithmNotSupportedException";

		// Token: 0x040001C1 RID: 449
		internal const string net_QOPNotSupportedException = "net_QOPNotSupportedException";

		// Token: 0x040001C2 RID: 450
		internal const string net_SelectModeNotSupportedException = "net_SelectModeNotSupportedException";

		// Token: 0x040001C3 RID: 451
		internal const string net_InvalidSocketHandle = "net_InvalidSocketHandle";

		// Token: 0x040001C4 RID: 452
		internal const string net_InvalidAddressFamily = "net_InvalidAddressFamily";

		// Token: 0x040001C5 RID: 453
		internal const string net_InvalidSocketAddressSize = "net_InvalidSocketAddressSize";

		// Token: 0x040001C6 RID: 454
		internal const string net_invalidAddressList = "net_invalidAddressList";

		// Token: 0x040001C7 RID: 455
		internal const string net_invalidPingBufferSize = "net_invalidPingBufferSize";

		// Token: 0x040001C8 RID: 456
		internal const string net_cant_perform_during_shutdown = "net_cant_perform_during_shutdown";

		// Token: 0x040001C9 RID: 457
		internal const string net_cant_create_environment = "net_cant_create_environment";

		// Token: 0x040001CA RID: 458
		internal const string net_completed_result = "net_completed_result";

		// Token: 0x040001CB RID: 459
		internal const string net_protocol_invalid_family = "net_protocol_invalid_family";

		// Token: 0x040001CC RID: 460
		internal const string net_protocol_invalid_multicast_family = "net_protocol_invalid_multicast_family";

		// Token: 0x040001CD RID: 461
		internal const string net_ssp_dont_support_cbt = "net_ssp_dont_support_cbt";

		// Token: 0x040001CE RID: 462
		internal const string net_unknown_osinstalltype = "net_unknown_osinstalltype";

		// Token: 0x040001CF RID: 463
		internal const string net_sockets_zerolist = "net_sockets_zerolist";

		// Token: 0x040001D0 RID: 464
		internal const string net_sockets_blocking = "net_sockets_blocking";

		// Token: 0x040001D1 RID: 465
		internal const string net_sockets_useblocking = "net_sockets_useblocking";

		// Token: 0x040001D2 RID: 466
		internal const string net_sockets_select = "net_sockets_select";

		// Token: 0x040001D3 RID: 467
		internal const string net_sockets_toolarge_select = "net_sockets_toolarge_select";

		// Token: 0x040001D4 RID: 468
		internal const string net_sockets_empty_select = "net_sockets_empty_select";

		// Token: 0x040001D5 RID: 469
		internal const string net_sockets_mustbind = "net_sockets_mustbind";

		// Token: 0x040001D6 RID: 470
		internal const string net_sockets_mustlisten = "net_sockets_mustlisten";

		// Token: 0x040001D7 RID: 471
		internal const string net_sockets_mustnotlisten = "net_sockets_mustnotlisten";

		// Token: 0x040001D8 RID: 472
		internal const string net_sockets_mustnotbebound = "net_sockets_mustnotbebound";

		// Token: 0x040001D9 RID: 473
		internal const string net_sockets_namedmustnotbebound = "net_sockets_namedmustnotbebound";

		// Token: 0x040001DA RID: 474
		internal const string net_sockets_invalid_socketinformation = "net_sockets_invalid_socketinformation";

		// Token: 0x040001DB RID: 475
		internal const string net_sockets_invalid_ipaddress_length = "net_sockets_invalid_ipaddress_length";

		// Token: 0x040001DC RID: 476
		internal const string net_sockets_invalid_optionValue = "net_sockets_invalid_optionValue";

		// Token: 0x040001DD RID: 477
		internal const string net_sockets_invalid_optionValue_all = "net_sockets_invalid_optionValue_all";

		// Token: 0x040001DE RID: 478
		internal const string net_sockets_disconnectedConnect = "net_sockets_disconnectedConnect";

		// Token: 0x040001DF RID: 479
		internal const string net_sockets_disconnectedAccept = "net_sockets_disconnectedAccept";

		// Token: 0x040001E0 RID: 480
		internal const string net_tcplistener_mustbestopped = "net_tcplistener_mustbestopped";

		// Token: 0x040001E1 RID: 481
		internal const string net_sockets_no_duplicate_async = "net_sockets_no_duplicate_async";

		// Token: 0x040001E2 RID: 482
		internal const string net_socketopinprogress = "net_socketopinprogress";

		// Token: 0x040001E3 RID: 483
		internal const string net_buffercounttoosmall = "net_buffercounttoosmall";

		// Token: 0x040001E4 RID: 484
		internal const string net_multibuffernotsupported = "net_multibuffernotsupported";

		// Token: 0x040001E5 RID: 485
		internal const string net_ambiguousbuffers = "net_ambiguousbuffers";

		// Token: 0x040001E6 RID: 486
		internal const string net_config_proxy = "net_config_proxy";

		// Token: 0x040001E7 RID: 487
		internal const string net_config_proxy_module_not_public = "net_config_proxy_module_not_public";

		// Token: 0x040001E8 RID: 488
		internal const string net_config_authenticationmodules = "net_config_authenticationmodules";

		// Token: 0x040001E9 RID: 489
		internal const string net_config_webrequestmodules = "net_config_webrequestmodules";

		// Token: 0x040001EA RID: 490
		internal const string net_config_requestcaching = "net_config_requestcaching";

		// Token: 0x040001EB RID: 491
		internal const string net_config_section_permission = "net_config_section_permission";

		// Token: 0x040001EC RID: 492
		internal const string net_config_element_permission = "net_config_element_permission";

		// Token: 0x040001ED RID: 493
		internal const string net_config_property_permission = "net_config_property_permission";

		// Token: 0x040001EE RID: 494
		internal const string net_WebResponseParseError_InvalidHeaderName = "net_WebResponseParseError_InvalidHeaderName";

		// Token: 0x040001EF RID: 495
		internal const string net_WebResponseParseError_InvalidContentLength = "net_WebResponseParseError_InvalidContentLength";

		// Token: 0x040001F0 RID: 496
		internal const string net_WebResponseParseError_IncompleteHeaderLine = "net_WebResponseParseError_IncompleteHeaderLine";

		// Token: 0x040001F1 RID: 497
		internal const string net_WebResponseParseError_CrLfError = "net_WebResponseParseError_CrLfError";

		// Token: 0x040001F2 RID: 498
		internal const string net_WebResponseParseError_InvalidChunkFormat = "net_WebResponseParseError_InvalidChunkFormat";

		// Token: 0x040001F3 RID: 499
		internal const string net_WebResponseParseError_UnexpectedServerResponse = "net_WebResponseParseError_UnexpectedServerResponse";

		// Token: 0x040001F4 RID: 500
		internal const string net_WebHeaderInvalidControlChars = "net_WebHeaderInvalidControlChars";

		// Token: 0x040001F5 RID: 501
		internal const string net_WebHeaderInvalidCRLFChars = "net_WebHeaderInvalidCRLFChars";

		// Token: 0x040001F6 RID: 502
		internal const string net_WebHeaderInvalidHeaderChars = "net_WebHeaderInvalidHeaderChars";

		// Token: 0x040001F7 RID: 503
		internal const string net_WebHeaderInvalidNonAsciiChars = "net_WebHeaderInvalidNonAsciiChars";

		// Token: 0x040001F8 RID: 504
		internal const string net_WebHeaderMissingColon = "net_WebHeaderMissingColon";

		// Token: 0x040001F9 RID: 505
		internal const string net_webstatus_Success = "net_webstatus_Success";

		// Token: 0x040001FA RID: 506
		internal const string net_webstatus_NameResolutionFailure = "net_webstatus_NameResolutionFailure";

		// Token: 0x040001FB RID: 507
		internal const string net_webstatus_ConnectFailure = "net_webstatus_ConnectFailure";

		// Token: 0x040001FC RID: 508
		internal const string net_webstatus_ReceiveFailure = "net_webstatus_ReceiveFailure";

		// Token: 0x040001FD RID: 509
		internal const string net_webstatus_SendFailure = "net_webstatus_SendFailure";

		// Token: 0x040001FE RID: 510
		internal const string net_webstatus_PipelineFailure = "net_webstatus_PipelineFailure";

		// Token: 0x040001FF RID: 511
		internal const string net_webstatus_RequestCanceled = "net_webstatus_RequestCanceled";

		// Token: 0x04000200 RID: 512
		internal const string net_webstatus_ConnectionClosed = "net_webstatus_ConnectionClosed";

		// Token: 0x04000201 RID: 513
		internal const string net_webstatus_TrustFailure = "net_webstatus_TrustFailure";

		// Token: 0x04000202 RID: 514
		internal const string net_webstatus_SecureChannelFailure = "net_webstatus_SecureChannelFailure";

		// Token: 0x04000203 RID: 515
		internal const string net_webstatus_ServerProtocolViolation = "net_webstatus_ServerProtocolViolation";

		// Token: 0x04000204 RID: 516
		internal const string net_webstatus_KeepAliveFailure = "net_webstatus_KeepAliveFailure";

		// Token: 0x04000205 RID: 517
		internal const string net_webstatus_ProxyNameResolutionFailure = "net_webstatus_ProxyNameResolutionFailure";

		// Token: 0x04000206 RID: 518
		internal const string net_webstatus_MessageLengthLimitExceeded = "net_webstatus_MessageLengthLimitExceeded";

		// Token: 0x04000207 RID: 519
		internal const string net_webstatus_CacheEntryNotFound = "net_webstatus_CacheEntryNotFound";

		// Token: 0x04000208 RID: 520
		internal const string net_webstatus_RequestProhibitedByCachePolicy = "net_webstatus_RequestProhibitedByCachePolicy";

		// Token: 0x04000209 RID: 521
		internal const string net_webstatus_Timeout = "net_webstatus_Timeout";

		// Token: 0x0400020A RID: 522
		internal const string net_webstatus_RequestProhibitedByProxy = "net_webstatus_RequestProhibitedByProxy";

		// Token: 0x0400020B RID: 523
		internal const string net_InvalidStatusCode = "net_InvalidStatusCode";

		// Token: 0x0400020C RID: 524
		internal const string net_ftpstatuscode_ServiceNotAvailable = "net_ftpstatuscode_ServiceNotAvailable";

		// Token: 0x0400020D RID: 525
		internal const string net_ftpstatuscode_CantOpenData = "net_ftpstatuscode_CantOpenData";

		// Token: 0x0400020E RID: 526
		internal const string net_ftpstatuscode_ConnectionClosed = "net_ftpstatuscode_ConnectionClosed";

		// Token: 0x0400020F RID: 527
		internal const string net_ftpstatuscode_ActionNotTakenFileUnavailableOrBusy = "net_ftpstatuscode_ActionNotTakenFileUnavailableOrBusy";

		// Token: 0x04000210 RID: 528
		internal const string net_ftpstatuscode_ActionAbortedLocalProcessingError = "net_ftpstatuscode_ActionAbortedLocalProcessingError";

		// Token: 0x04000211 RID: 529
		internal const string net_ftpstatuscode_ActionNotTakenInsufficentSpace = "net_ftpstatuscode_ActionNotTakenInsufficentSpace";

		// Token: 0x04000212 RID: 530
		internal const string net_ftpstatuscode_CommandSyntaxError = "net_ftpstatuscode_CommandSyntaxError";

		// Token: 0x04000213 RID: 531
		internal const string net_ftpstatuscode_ArgumentSyntaxError = "net_ftpstatuscode_ArgumentSyntaxError";

		// Token: 0x04000214 RID: 532
		internal const string net_ftpstatuscode_CommandNotImplemented = "net_ftpstatuscode_CommandNotImplemented";

		// Token: 0x04000215 RID: 533
		internal const string net_ftpstatuscode_BadCommandSequence = "net_ftpstatuscode_BadCommandSequence";

		// Token: 0x04000216 RID: 534
		internal const string net_ftpstatuscode_NotLoggedIn = "net_ftpstatuscode_NotLoggedIn";

		// Token: 0x04000217 RID: 535
		internal const string net_ftpstatuscode_AccountNeeded = "net_ftpstatuscode_AccountNeeded";

		// Token: 0x04000218 RID: 536
		internal const string net_ftpstatuscode_ActionNotTakenFileUnavailable = "net_ftpstatuscode_ActionNotTakenFileUnavailable";

		// Token: 0x04000219 RID: 537
		internal const string net_ftpstatuscode_ActionAbortedUnknownPageType = "net_ftpstatuscode_ActionAbortedUnknownPageType";

		// Token: 0x0400021A RID: 538
		internal const string net_ftpstatuscode_FileActionAborted = "net_ftpstatuscode_FileActionAborted";

		// Token: 0x0400021B RID: 539
		internal const string net_ftpstatuscode_ActionNotTakenFilenameNotAllowed = "net_ftpstatuscode_ActionNotTakenFilenameNotAllowed";

		// Token: 0x0400021C RID: 540
		internal const string net_httpstatuscode_NoContent = "net_httpstatuscode_NoContent";

		// Token: 0x0400021D RID: 541
		internal const string net_httpstatuscode_NonAuthoritativeInformation = "net_httpstatuscode_NonAuthoritativeInformation";

		// Token: 0x0400021E RID: 542
		internal const string net_httpstatuscode_ResetContent = "net_httpstatuscode_ResetContent";

		// Token: 0x0400021F RID: 543
		internal const string net_httpstatuscode_PartialContent = "net_httpstatuscode_PartialContent";

		// Token: 0x04000220 RID: 544
		internal const string net_httpstatuscode_MultipleChoices = "net_httpstatuscode_MultipleChoices";

		// Token: 0x04000221 RID: 545
		internal const string net_httpstatuscode_Ambiguous = "net_httpstatuscode_Ambiguous";

		// Token: 0x04000222 RID: 546
		internal const string net_httpstatuscode_MovedPermanently = "net_httpstatuscode_MovedPermanently";

		// Token: 0x04000223 RID: 547
		internal const string net_httpstatuscode_Moved = "net_httpstatuscode_Moved";

		// Token: 0x04000224 RID: 548
		internal const string net_httpstatuscode_Found = "net_httpstatuscode_Found";

		// Token: 0x04000225 RID: 549
		internal const string net_httpstatuscode_Redirect = "net_httpstatuscode_Redirect";

		// Token: 0x04000226 RID: 550
		internal const string net_httpstatuscode_SeeOther = "net_httpstatuscode_SeeOther";

		// Token: 0x04000227 RID: 551
		internal const string net_httpstatuscode_RedirectMethod = "net_httpstatuscode_RedirectMethod";

		// Token: 0x04000228 RID: 552
		internal const string net_httpstatuscode_NotModified = "net_httpstatuscode_NotModified";

		// Token: 0x04000229 RID: 553
		internal const string net_httpstatuscode_UseProxy = "net_httpstatuscode_UseProxy";

		// Token: 0x0400022A RID: 554
		internal const string net_httpstatuscode_TemporaryRedirect = "net_httpstatuscode_TemporaryRedirect";

		// Token: 0x0400022B RID: 555
		internal const string net_httpstatuscode_RedirectKeepVerb = "net_httpstatuscode_RedirectKeepVerb";

		// Token: 0x0400022C RID: 556
		internal const string net_httpstatuscode_BadRequest = "net_httpstatuscode_BadRequest";

		// Token: 0x0400022D RID: 557
		internal const string net_httpstatuscode_Unauthorized = "net_httpstatuscode_Unauthorized";

		// Token: 0x0400022E RID: 558
		internal const string net_httpstatuscode_PaymentRequired = "net_httpstatuscode_PaymentRequired";

		// Token: 0x0400022F RID: 559
		internal const string net_httpstatuscode_Forbidden = "net_httpstatuscode_Forbidden";

		// Token: 0x04000230 RID: 560
		internal const string net_httpstatuscode_NotFound = "net_httpstatuscode_NotFound";

		// Token: 0x04000231 RID: 561
		internal const string net_httpstatuscode_MethodNotAllowed = "net_httpstatuscode_MethodNotAllowed";

		// Token: 0x04000232 RID: 562
		internal const string net_httpstatuscode_NotAcceptable = "net_httpstatuscode_NotAcceptable";

		// Token: 0x04000233 RID: 563
		internal const string net_httpstatuscode_ProxyAuthenticationRequired = "net_httpstatuscode_ProxyAuthenticationRequired";

		// Token: 0x04000234 RID: 564
		internal const string net_httpstatuscode_RequestTimeout = "net_httpstatuscode_RequestTimeout";

		// Token: 0x04000235 RID: 565
		internal const string net_httpstatuscode_Conflict = "net_httpstatuscode_Conflict";

		// Token: 0x04000236 RID: 566
		internal const string net_httpstatuscode_Gone = "net_httpstatuscode_Gone";

		// Token: 0x04000237 RID: 567
		internal const string net_httpstatuscode_LengthRequired = "net_httpstatuscode_LengthRequired";

		// Token: 0x04000238 RID: 568
		internal const string net_httpstatuscode_InternalServerError = "net_httpstatuscode_InternalServerError";

		// Token: 0x04000239 RID: 569
		internal const string net_httpstatuscode_NotImplemented = "net_httpstatuscode_NotImplemented";

		// Token: 0x0400023A RID: 570
		internal const string net_httpstatuscode_BadGateway = "net_httpstatuscode_BadGateway";

		// Token: 0x0400023B RID: 571
		internal const string net_httpstatuscode_ServiceUnavailable = "net_httpstatuscode_ServiceUnavailable";

		// Token: 0x0400023C RID: 572
		internal const string net_httpstatuscode_GatewayTimeout = "net_httpstatuscode_GatewayTimeout";

		// Token: 0x0400023D RID: 573
		internal const string net_httpstatuscode_HttpVersionNotSupported = "net_httpstatuscode_HttpVersionNotSupported";

		// Token: 0x0400023E RID: 574
		internal const string net_uri_BadScheme = "net_uri_BadScheme";

		// Token: 0x0400023F RID: 575
		internal const string net_uri_BadFormat = "net_uri_BadFormat";

		// Token: 0x04000240 RID: 576
		internal const string net_uri_BadUserPassword = "net_uri_BadUserPassword";

		// Token: 0x04000241 RID: 577
		internal const string net_uri_BadHostName = "net_uri_BadHostName";

		// Token: 0x04000242 RID: 578
		internal const string net_uri_BadAuthority = "net_uri_BadAuthority";

		// Token: 0x04000243 RID: 579
		internal const string net_uri_BadAuthorityTerminator = "net_uri_BadAuthorityTerminator";

		// Token: 0x04000244 RID: 580
		internal const string net_uri_BadFileName = "net_uri_BadFileName";

		// Token: 0x04000245 RID: 581
		internal const string net_uri_EmptyUri = "net_uri_EmptyUri";

		// Token: 0x04000246 RID: 582
		internal const string net_uri_BadString = "net_uri_BadString";

		// Token: 0x04000247 RID: 583
		internal const string net_uri_MustRootedPath = "net_uri_MustRootedPath";

		// Token: 0x04000248 RID: 584
		internal const string net_uri_BadPort = "net_uri_BadPort";

		// Token: 0x04000249 RID: 585
		internal const string net_uri_SizeLimit = "net_uri_SizeLimit";

		// Token: 0x0400024A RID: 586
		internal const string net_uri_SchemeLimit = "net_uri_SchemeLimit";

		// Token: 0x0400024B RID: 587
		internal const string net_uri_NotAbsolute = "net_uri_NotAbsolute";

		// Token: 0x0400024C RID: 588
		internal const string net_uri_SpecialUriComponent = "net_uri_SpecialUriComponent";

		// Token: 0x0400024D RID: 589
		internal const string net_uri_CustomValidationFailed = "net_uri_CustomValidationFailed";

		// Token: 0x0400024E RID: 590
		internal const string net_uri_PortOutOfRange = "net_uri_PortOutOfRange";

		// Token: 0x0400024F RID: 591
		internal const string net_uri_UserDrivenParsing = "net_uri_UserDrivenParsing";

		// Token: 0x04000250 RID: 592
		internal const string net_uri_AlreadyRegistered = "net_uri_AlreadyRegistered";

		// Token: 0x04000251 RID: 593
		internal const string net_uri_NeedFreshParser = "net_uri_NeedFreshParser";

		// Token: 0x04000252 RID: 594
		internal const string net_uri_CannotCreateRelative = "net_uri_CannotCreateRelative";

		// Token: 0x04000253 RID: 595
		internal const string net_uri_InvalidUriKind = "net_uri_InvalidUriKind";

		// Token: 0x04000254 RID: 596
		internal const string net_uri_BadUnicodeHostForIdn = "net_uri_BadUnicodeHostForIdn";

		// Token: 0x04000255 RID: 597
		internal const string net_io_completionportwasbound = "net_io_completionportwasbound";

		// Token: 0x04000256 RID: 598
		internal const string net_io_writefailure = "net_io_writefailure";

		// Token: 0x04000257 RID: 599
		internal const string net_io_readfailure = "net_io_readfailure";

		// Token: 0x04000258 RID: 600
		internal const string net_io_connectionclosed = "net_io_connectionclosed";

		// Token: 0x04000259 RID: 601
		internal const string net_io_transportfailure = "net_io_transportfailure";

		// Token: 0x0400025A RID: 602
		internal const string net_io_internal_bind = "net_io_internal_bind";

		// Token: 0x0400025B RID: 603
		internal const string net_io_invalidasyncresult = "net_io_invalidasyncresult";

		// Token: 0x0400025C RID: 604
		internal const string net_io_invalidnestedcall = "net_io_invalidnestedcall";

		// Token: 0x0400025D RID: 605
		internal const string net_io_invalidendcall = "net_io_invalidendcall";

		// Token: 0x0400025E RID: 606
		internal const string net_io_must_be_rw_stream = "net_io_must_be_rw_stream";

		// Token: 0x0400025F RID: 607
		internal const string net_io_header_id = "net_io_header_id";

		// Token: 0x04000260 RID: 608
		internal const string net_io_out_range = "net_io_out_range";

		// Token: 0x04000261 RID: 609
		internal const string net_io_encrypt = "net_io_encrypt";

		// Token: 0x04000262 RID: 610
		internal const string net_io_decrypt = "net_io_decrypt";

		// Token: 0x04000263 RID: 611
		internal const string net_io_read = "net_io_read";

		// Token: 0x04000264 RID: 612
		internal const string net_io_write = "net_io_write";

		// Token: 0x04000265 RID: 613
		internal const string net_io_eof = "net_io_eof";

		// Token: 0x04000266 RID: 614
		internal const string net_io_async_result = "net_io_async_result";

		// Token: 0x04000267 RID: 615
		internal const string net_headers_req = "net_headers_req";

		// Token: 0x04000268 RID: 616
		internal const string net_headers_rsp = "net_headers_rsp";

		// Token: 0x04000269 RID: 617
		internal const string net_headers_toolong = "net_headers_toolong";

		// Token: 0x0400026A RID: 618
		internal const string net_emptystringset = "net_emptystringset";

		// Token: 0x0400026B RID: 619
		internal const string net_emptystringcall = "net_emptystringcall";

		// Token: 0x0400026C RID: 620
		internal const string net_listener_mustcall = "net_listener_mustcall";

		// Token: 0x0400026D RID: 621
		internal const string net_listener_mustcompletecall = "net_listener_mustcompletecall";

		// Token: 0x0400026E RID: 622
		internal const string net_listener_callinprogress = "net_listener_callinprogress";

		// Token: 0x0400026F RID: 623
		internal const string net_listener_scheme = "net_listener_scheme";

		// Token: 0x04000270 RID: 624
		internal const string net_listener_host = "net_listener_host";

		// Token: 0x04000271 RID: 625
		internal const string net_listener_slash = "net_listener_slash";

		// Token: 0x04000272 RID: 626
		internal const string net_listener_repcall = "net_listener_repcall";

		// Token: 0x04000273 RID: 627
		internal const string net_listener_invalid_cbt_type = "net_listener_invalid_cbt_type";

		// Token: 0x04000274 RID: 628
		internal const string net_listener_no_spns = "net_listener_no_spns";

		// Token: 0x04000275 RID: 629
		internal const string net_listener_cannot_set_custom_cbt = "net_listener_cannot_set_custom_cbt";

		// Token: 0x04000276 RID: 630
		internal const string net_listener_cbt_not_supported = "net_listener_cbt_not_supported";

		// Token: 0x04000277 RID: 631
		internal const string net_tls_version = "net_tls_version";

		// Token: 0x04000278 RID: 632
		internal const string net_perm_target = "net_perm_target";

		// Token: 0x04000279 RID: 633
		internal const string net_perm_both_regex = "net_perm_both_regex";

		// Token: 0x0400027A RID: 634
		internal const string net_perm_none = "net_perm_none";

		// Token: 0x0400027B RID: 635
		internal const string net_perm_attrib_count = "net_perm_attrib_count";

		// Token: 0x0400027C RID: 636
		internal const string net_perm_invalid_val = "net_perm_invalid_val";

		// Token: 0x0400027D RID: 637
		internal const string net_perm_attrib_multi = "net_perm_attrib_multi";

		// Token: 0x0400027E RID: 638
		internal const string net_perm_epname = "net_perm_epname";

		// Token: 0x0400027F RID: 639
		internal const string net_perm_invalid_val_in_element = "net_perm_invalid_val_in_element";

		// Token: 0x04000280 RID: 640
		internal const string net_invalid_ip_addr = "net_invalid_ip_addr";

		// Token: 0x04000281 RID: 641
		internal const string dns_bad_ip_address = "dns_bad_ip_address";

		// Token: 0x04000282 RID: 642
		internal const string net_bad_mac_address = "net_bad_mac_address";

		// Token: 0x04000283 RID: 643
		internal const string net_ping = "net_ping";

		// Token: 0x04000284 RID: 644
		internal const string net_bad_ip_address_prefix = "net_bad_ip_address_prefix";

		// Token: 0x04000285 RID: 645
		internal const string net_max_ip_address_list_length_exceeded = "net_max_ip_address_list_length_exceeded";

		// Token: 0x04000286 RID: 646
		internal const string net_webclient = "net_webclient";

		// Token: 0x04000287 RID: 647
		internal const string net_webclient_ContentType = "net_webclient_ContentType";

		// Token: 0x04000288 RID: 648
		internal const string net_webclient_Multipart = "net_webclient_Multipart";

		// Token: 0x04000289 RID: 649
		internal const string net_webclient_no_concurrent_io_allowed = "net_webclient_no_concurrent_io_allowed";

		// Token: 0x0400028A RID: 650
		internal const string net_webclient_invalid_baseaddress = "net_webclient_invalid_baseaddress";

		// Token: 0x0400028B RID: 651
		internal const string net_container_add_cookie = "net_container_add_cookie";

		// Token: 0x0400028C RID: 652
		internal const string net_cookie_invalid = "net_cookie_invalid";

		// Token: 0x0400028D RID: 653
		internal const string net_cookie_size = "net_cookie_size";

		// Token: 0x0400028E RID: 654
		internal const string net_cookie_parse_header = "net_cookie_parse_header";

		// Token: 0x0400028F RID: 655
		internal const string net_cookie_attribute = "net_cookie_attribute";

		// Token: 0x04000290 RID: 656
		internal const string net_cookie_format = "net_cookie_format";

		// Token: 0x04000291 RID: 657
		internal const string net_cookie_exists = "net_cookie_exists";

		// Token: 0x04000292 RID: 658
		internal const string net_cookie_capacity_range = "net_cookie_capacity_range";

		// Token: 0x04000293 RID: 659
		internal const string net_set_token = "net_set_token";

		// Token: 0x04000294 RID: 660
		internal const string net_revert_token = "net_revert_token";

		// Token: 0x04000295 RID: 661
		internal const string net_ssl_io_async_context = "net_ssl_io_async_context";

		// Token: 0x04000296 RID: 662
		internal const string net_ssl_io_encrypt = "net_ssl_io_encrypt";

		// Token: 0x04000297 RID: 663
		internal const string net_ssl_io_decrypt = "net_ssl_io_decrypt";

		// Token: 0x04000298 RID: 664
		internal const string net_ssl_io_context_expired = "net_ssl_io_context_expired";

		// Token: 0x04000299 RID: 665
		internal const string net_ssl_io_handshake_start = "net_ssl_io_handshake_start";

		// Token: 0x0400029A RID: 666
		internal const string net_ssl_io_handshake = "net_ssl_io_handshake";

		// Token: 0x0400029B RID: 667
		internal const string net_ssl_io_frame = "net_ssl_io_frame";

		// Token: 0x0400029C RID: 668
		internal const string net_ssl_io_corrupted = "net_ssl_io_corrupted";

		// Token: 0x0400029D RID: 669
		internal const string net_ssl_io_cert_validation = "net_ssl_io_cert_validation";

		// Token: 0x0400029E RID: 670
		internal const string net_ssl_io_invalid_end_call = "net_ssl_io_invalid_end_call";

		// Token: 0x0400029F RID: 671
		internal const string net_ssl_io_invalid_begin_call = "net_ssl_io_invalid_begin_call";

		// Token: 0x040002A0 RID: 672
		internal const string net_ssl_io_no_server_cert = "net_ssl_io_no_server_cert";

		// Token: 0x040002A1 RID: 673
		internal const string net_auth_bad_client_creds = "net_auth_bad_client_creds";

		// Token: 0x040002A2 RID: 674
		internal const string net_auth_bad_client_creds_or_target_mismatch = "net_auth_bad_client_creds_or_target_mismatch";

		// Token: 0x040002A3 RID: 675
		internal const string net_auth_context_expectation = "net_auth_context_expectation";

		// Token: 0x040002A4 RID: 676
		internal const string net_auth_context_expectation_remote = "net_auth_context_expectation_remote";

		// Token: 0x040002A5 RID: 677
		internal const string net_auth_supported_impl_levels = "net_auth_supported_impl_levels";

		// Token: 0x040002A6 RID: 678
		internal const string net_auth_no_protection_on_win9x = "net_auth_no_protection_on_win9x";

		// Token: 0x040002A7 RID: 679
		internal const string net_auth_no_anonymous_support = "net_auth_no_anonymous_support";

		// Token: 0x040002A8 RID: 680
		internal const string net_auth_reauth = "net_auth_reauth";

		// Token: 0x040002A9 RID: 681
		internal const string net_auth_noauth = "net_auth_noauth";

		// Token: 0x040002AA RID: 682
		internal const string net_auth_client_server = "net_auth_client_server";

		// Token: 0x040002AB RID: 683
		internal const string net_auth_noencryption = "net_auth_noencryption";

		// Token: 0x040002AC RID: 684
		internal const string net_auth_SSPI = "net_auth_SSPI";

		// Token: 0x040002AD RID: 685
		internal const string net_auth_failure = "net_auth_failure";

		// Token: 0x040002AE RID: 686
		internal const string net_auth_eof = "net_auth_eof";

		// Token: 0x040002AF RID: 687
		internal const string net_auth_alert = "net_auth_alert";

		// Token: 0x040002B0 RID: 688
		internal const string net_auth_ignored_reauth = "net_auth_ignored_reauth";

		// Token: 0x040002B1 RID: 689
		internal const string net_auth_empty_read = "net_auth_empty_read";

		// Token: 0x040002B2 RID: 690
		internal const string net_auth_message_not_encrypted = "net_auth_message_not_encrypted";

		// Token: 0x040002B3 RID: 691
		internal const string net_auth_must_specify_extended_protection_scheme = "net_auth_must_specify_extended_protection_scheme";

		// Token: 0x040002B4 RID: 692
		internal const string net_frame_size = "net_frame_size";

		// Token: 0x040002B5 RID: 693
		internal const string net_frame_read_io = "net_frame_read_io";

		// Token: 0x040002B6 RID: 694
		internal const string net_frame_read_size = "net_frame_read_size";

		// Token: 0x040002B7 RID: 695
		internal const string net_frame_max_size = "net_frame_max_size";

		// Token: 0x040002B8 RID: 696
		internal const string net_jscript_load = "net_jscript_load";

		// Token: 0x040002B9 RID: 697
		internal const string net_proxy_not_gmt = "net_proxy_not_gmt";

		// Token: 0x040002BA RID: 698
		internal const string net_proxy_invalid_dayofweek = "net_proxy_invalid_dayofweek";

		// Token: 0x040002BB RID: 699
		internal const string net_param_not_string = "net_param_not_string";

		// Token: 0x040002BC RID: 700
		internal const string net_value_cannot_be_negative = "net_value_cannot_be_negative";

		// Token: 0x040002BD RID: 701
		internal const string net_invalid_offset = "net_invalid_offset";

		// Token: 0x040002BE RID: 702
		internal const string net_offset_plus_count = "net_offset_plus_count";

		// Token: 0x040002BF RID: 703
		internal const string net_cannot_be_false = "net_cannot_be_false";

		// Token: 0x040002C0 RID: 704
		internal const string net_invalid_enum = "net_invalid_enum";

		// Token: 0x040002C1 RID: 705
		internal const string net_listener_already = "net_listener_already";

		// Token: 0x040002C2 RID: 706
		internal const string net_cache_shadowstream_not_writable = "net_cache_shadowstream_not_writable";

		// Token: 0x040002C3 RID: 707
		internal const string net_cache_validator_fail = "net_cache_validator_fail";

		// Token: 0x040002C4 RID: 708
		internal const string net_cache_access_denied = "net_cache_access_denied";

		// Token: 0x040002C5 RID: 709
		internal const string net_cache_validator_result = "net_cache_validator_result";

		// Token: 0x040002C6 RID: 710
		internal const string net_cache_retrieve_failure = "net_cache_retrieve_failure";

		// Token: 0x040002C7 RID: 711
		internal const string net_cache_not_supported_body = "net_cache_not_supported_body";

		// Token: 0x040002C8 RID: 712
		internal const string net_cache_not_supported_command = "net_cache_not_supported_command";

		// Token: 0x040002C9 RID: 713
		internal const string net_cache_not_accept_response = "net_cache_not_accept_response";

		// Token: 0x040002CA RID: 714
		internal const string net_cache_method_failed = "net_cache_method_failed";

		// Token: 0x040002CB RID: 715
		internal const string net_cache_key_failed = "net_cache_key_failed";

		// Token: 0x040002CC RID: 716
		internal const string net_cache_no_stream = "net_cache_no_stream";

		// Token: 0x040002CD RID: 717
		internal const string net_cache_unsupported_partial_stream = "net_cache_unsupported_partial_stream";

		// Token: 0x040002CE RID: 718
		internal const string net_cache_not_configured = "net_cache_not_configured";

		// Token: 0x040002CF RID: 719
		internal const string net_cache_non_seekable_stream_not_supported = "net_cache_non_seekable_stream_not_supported";

		// Token: 0x040002D0 RID: 720
		internal const string net_invalid_cast = "net_invalid_cast";

		// Token: 0x040002D1 RID: 721
		internal const string net_collection_readonly = "net_collection_readonly";

		// Token: 0x040002D2 RID: 722
		internal const string net_not_ipermission = "net_not_ipermission";

		// Token: 0x040002D3 RID: 723
		internal const string net_no_classname = "net_no_classname";

		// Token: 0x040002D4 RID: 724
		internal const string net_no_typename = "net_no_typename";

		// Token: 0x040002D5 RID: 725
		internal const string net_array_too_small = "net_array_too_small";

		// Token: 0x040002D6 RID: 726
		internal const string net_servicePointAddressNotSupportedInHostMode = "net_servicePointAddressNotSupportedInHostMode";

		// Token: 0x040002D7 RID: 727
		internal const string net_log_listener_delegate_exception = "net_log_listener_delegate_exception";

		// Token: 0x040002D8 RID: 728
		internal const string net_log_listener_unsupported_authentication_scheme = "net_log_listener_unsupported_authentication_scheme";

		// Token: 0x040002D9 RID: 729
		internal const string net_log_listener_unmatched_authentication_scheme = "net_log_listener_unmatched_authentication_scheme";

		// Token: 0x040002DA RID: 730
		internal const string net_log_listener_create_valid_identity_failed = "net_log_listener_create_valid_identity_failed";

		// Token: 0x040002DB RID: 731
		internal const string net_log_listener_no_cbt_disabled = "net_log_listener_no_cbt_disabled";

		// Token: 0x040002DC RID: 732
		internal const string net_log_listener_no_cbt_http = "net_log_listener_no_cbt_http";

		// Token: 0x040002DD RID: 733
		internal const string net_log_listener_no_cbt_platform = "net_log_listener_no_cbt_platform";

		// Token: 0x040002DE RID: 734
		internal const string net_log_listener_no_cbt_trustedproxy = "net_log_listener_no_cbt_trustedproxy";

		// Token: 0x040002DF RID: 735
		internal const string net_log_listener_cbt = "net_log_listener_cbt";

		// Token: 0x040002E0 RID: 736
		internal const string net_log_listener_no_spn_kerberos = "net_log_listener_no_spn_kerberos";

		// Token: 0x040002E1 RID: 737
		internal const string net_log_listener_no_spn_disabled = "net_log_listener_no_spn_disabled";

		// Token: 0x040002E2 RID: 738
		internal const string net_log_listener_no_spn_cbt = "net_log_listener_no_spn_cbt";

		// Token: 0x040002E3 RID: 739
		internal const string net_log_listener_no_spn_platform = "net_log_listener_no_spn_platform";

		// Token: 0x040002E4 RID: 740
		internal const string net_log_listener_no_spn_whensupported = "net_log_listener_no_spn_whensupported";

		// Token: 0x040002E5 RID: 741
		internal const string net_log_listener_no_spn_loopback = "net_log_listener_no_spn_loopback";

		// Token: 0x040002E6 RID: 742
		internal const string net_log_listener_spn = "net_log_listener_spn";

		// Token: 0x040002E7 RID: 743
		internal const string net_log_listener_spn_passed = "net_log_listener_spn_passed";

		// Token: 0x040002E8 RID: 744
		internal const string net_log_listener_spn_failed = "net_log_listener_spn_failed";

		// Token: 0x040002E9 RID: 745
		internal const string net_log_listener_spn_failed_always = "net_log_listener_spn_failed_always";

		// Token: 0x040002EA RID: 746
		internal const string net_log_listener_spn_failed_empty = "net_log_listener_spn_failed_empty";

		// Token: 0x040002EB RID: 747
		internal const string net_log_listener_spn_failed_dump = "net_log_listener_spn_failed_dump";

		// Token: 0x040002EC RID: 748
		internal const string net_log_sspi_enumerating_security_packages = "net_log_sspi_enumerating_security_packages";

		// Token: 0x040002ED RID: 749
		internal const string net_log_sspi_security_package_not_found = "net_log_sspi_security_package_not_found";

		// Token: 0x040002EE RID: 750
		internal const string net_log_sspi_security_context_input_buffer = "net_log_sspi_security_context_input_buffer";

		// Token: 0x040002EF RID: 751
		internal const string net_log_sspi_security_context_input_buffers = "net_log_sspi_security_context_input_buffers";

		// Token: 0x040002F0 RID: 752
		internal const string net_log_remote_certificate = "net_log_remote_certificate";

		// Token: 0x040002F1 RID: 753
		internal const string net_log_locating_private_key_for_certificate = "net_log_locating_private_key_for_certificate";

		// Token: 0x040002F2 RID: 754
		internal const string net_log_cert_is_of_type_2 = "net_log_cert_is_of_type_2";

		// Token: 0x040002F3 RID: 755
		internal const string net_log_found_cert_in_store = "net_log_found_cert_in_store";

		// Token: 0x040002F4 RID: 756
		internal const string net_log_did_not_find_cert_in_store = "net_log_did_not_find_cert_in_store";

		// Token: 0x040002F5 RID: 757
		internal const string net_log_open_store_failed = "net_log_open_store_failed";

		// Token: 0x040002F6 RID: 758
		internal const string net_log_got_certificate_from_delegate = "net_log_got_certificate_from_delegate";

		// Token: 0x040002F7 RID: 759
		internal const string net_log_no_delegate_and_have_no_client_cert = "net_log_no_delegate_and_have_no_client_cert";

		// Token: 0x040002F8 RID: 760
		internal const string net_log_no_delegate_but_have_client_cert = "net_log_no_delegate_but_have_client_cert";

		// Token: 0x040002F9 RID: 761
		internal const string net_log_attempting_restart_using_cert = "net_log_attempting_restart_using_cert";

		// Token: 0x040002FA RID: 762
		internal const string net_log_no_issuers_try_all_certs = "net_log_no_issuers_try_all_certs";

		// Token: 0x040002FB RID: 763
		internal const string net_log_server_issuers_look_for_matching_certs = "net_log_server_issuers_look_for_matching_certs";

		// Token: 0x040002FC RID: 764
		internal const string net_log_selected_cert = "net_log_selected_cert";

		// Token: 0x040002FD RID: 765
		internal const string net_log_n_certs_after_filtering = "net_log_n_certs_after_filtering";

		// Token: 0x040002FE RID: 766
		internal const string net_log_finding_matching_certs = "net_log_finding_matching_certs";

		// Token: 0x040002FF RID: 767
		internal const string net_log_using_cached_credential = "net_log_using_cached_credential";

		// Token: 0x04000300 RID: 768
		internal const string net_log_remote_cert_user_declared_valid = "net_log_remote_cert_user_declared_valid";

		// Token: 0x04000301 RID: 769
		internal const string net_log_remote_cert_user_declared_invalid = "net_log_remote_cert_user_declared_invalid";

		// Token: 0x04000302 RID: 770
		internal const string net_log_remote_cert_has_no_errors = "net_log_remote_cert_has_no_errors";

		// Token: 0x04000303 RID: 771
		internal const string net_log_remote_cert_has_errors = "net_log_remote_cert_has_errors";

		// Token: 0x04000304 RID: 772
		internal const string net_log_remote_cert_not_available = "net_log_remote_cert_not_available";

		// Token: 0x04000305 RID: 773
		internal const string net_log_remote_cert_name_mismatch = "net_log_remote_cert_name_mismatch";

		// Token: 0x04000306 RID: 774
		internal const string net_log_proxy_autodetect_script_location_parse_error = "net_log_proxy_autodetect_script_location_parse_error";

		// Token: 0x04000307 RID: 775
		internal const string net_log_proxy_autodetect_failed = "net_log_proxy_autodetect_failed";

		// Token: 0x04000308 RID: 776
		internal const string net_log_proxy_script_execution_error = "net_log_proxy_script_execution_error";

		// Token: 0x04000309 RID: 777
		internal const string net_log_proxy_script_download_compile_error = "net_log_proxy_script_download_compile_error";

		// Token: 0x0400030A RID: 778
		internal const string net_log_proxy_system_setting_update = "net_log_proxy_system_setting_update";

		// Token: 0x0400030B RID: 779
		internal const string net_log_proxy_update_due_to_ip_config_change = "net_log_proxy_update_due_to_ip_config_change";

		// Token: 0x0400030C RID: 780
		internal const string net_log_proxy_called_with_null_parameter = "net_log_proxy_called_with_null_parameter";

		// Token: 0x0400030D RID: 781
		internal const string net_log_proxy_called_with_invalid_parameter = "net_log_proxy_called_with_invalid_parameter";

		// Token: 0x0400030E RID: 782
		internal const string net_log_proxy_ras_notsupported_exception = "net_log_proxy_ras_notsupported_exception";

		// Token: 0x0400030F RID: 783
		internal const string net_log_cache_validation_failed_resubmit = "net_log_cache_validation_failed_resubmit";

		// Token: 0x04000310 RID: 784
		internal const string net_log_cache_refused_server_response = "net_log_cache_refused_server_response";

		// Token: 0x04000311 RID: 785
		internal const string net_log_cache_ftp_proxy_doesnt_support_partial = "net_log_cache_ftp_proxy_doesnt_support_partial";

		// Token: 0x04000312 RID: 786
		internal const string net_log_cache_ftp_method = "net_log_cache_ftp_method";

		// Token: 0x04000313 RID: 787
		internal const string net_log_cache_ftp_supports_bin_only = "net_log_cache_ftp_supports_bin_only";

		// Token: 0x04000314 RID: 788
		internal const string net_log_cache_replacing_entry_with_HTTP_200 = "net_log_cache_replacing_entry_with_HTTP_200";

		// Token: 0x04000315 RID: 789
		internal const string net_log_cache_now_time = "net_log_cache_now_time";

		// Token: 0x04000316 RID: 790
		internal const string net_log_cache_max_age_absolute = "net_log_cache_max_age_absolute";

		// Token: 0x04000317 RID: 791
		internal const string net_log_cache_age1 = "net_log_cache_age1";

		// Token: 0x04000318 RID: 792
		internal const string net_log_cache_age1_date_header = "net_log_cache_age1_date_header";

		// Token: 0x04000319 RID: 793
		internal const string net_log_cache_age1_last_synchronized = "net_log_cache_age1_last_synchronized";

		// Token: 0x0400031A RID: 794
		internal const string net_log_cache_age1_last_synchronized_age_header = "net_log_cache_age1_last_synchronized_age_header";

		// Token: 0x0400031B RID: 795
		internal const string net_log_cache_age2 = "net_log_cache_age2";

		// Token: 0x0400031C RID: 796
		internal const string net_log_cache_max_age_cache_s_max_age = "net_log_cache_max_age_cache_s_max_age";

		// Token: 0x0400031D RID: 797
		internal const string net_log_cache_max_age_expires_date = "net_log_cache_max_age_expires_date";

		// Token: 0x0400031E RID: 798
		internal const string net_log_cache_max_age_cache_max_age = "net_log_cache_max_age_cache_max_age";

		// Token: 0x0400031F RID: 799
		internal const string net_log_cache_no_max_age_use_10_percent = "net_log_cache_no_max_age_use_10_percent";

		// Token: 0x04000320 RID: 800
		internal const string net_log_cache_no_max_age_use_default = "net_log_cache_no_max_age_use_default";

		// Token: 0x04000321 RID: 801
		internal const string net_log_cache_validator_invalid_for_policy = "net_log_cache_validator_invalid_for_policy";

		// Token: 0x04000322 RID: 802
		internal const string net_log_cache_response_last_modified = "net_log_cache_response_last_modified";

		// Token: 0x04000323 RID: 803
		internal const string net_log_cache_cache_last_modified = "net_log_cache_cache_last_modified";

		// Token: 0x04000324 RID: 804
		internal const string net_log_cache_partial_and_non_zero_content_offset = "net_log_cache_partial_and_non_zero_content_offset";

		// Token: 0x04000325 RID: 805
		internal const string net_log_cache_response_valid_based_on_policy = "net_log_cache_response_valid_based_on_policy";

		// Token: 0x04000326 RID: 806
		internal const string net_log_cache_null_response_failure = "net_log_cache_null_response_failure";

		// Token: 0x04000327 RID: 807
		internal const string net_log_cache_ftp_response_status = "net_log_cache_ftp_response_status";

		// Token: 0x04000328 RID: 808
		internal const string net_log_cache_resp_valid_based_on_retry = "net_log_cache_resp_valid_based_on_retry";

		// Token: 0x04000329 RID: 809
		internal const string net_log_cache_no_update_based_on_method = "net_log_cache_no_update_based_on_method";

		// Token: 0x0400032A RID: 810
		internal const string net_log_cache_removed_existing_invalid_entry = "net_log_cache_removed_existing_invalid_entry";

		// Token: 0x0400032B RID: 811
		internal const string net_log_cache_not_updated_based_on_policy = "net_log_cache_not_updated_based_on_policy";

		// Token: 0x0400032C RID: 812
		internal const string net_log_cache_not_updated_because_no_response = "net_log_cache_not_updated_because_no_response";

		// Token: 0x0400032D RID: 813
		internal const string net_log_cache_removed_existing_based_on_method = "net_log_cache_removed_existing_based_on_method";

		// Token: 0x0400032E RID: 814
		internal const string net_log_cache_existing_not_removed_because_unexpected_response_status = "net_log_cache_existing_not_removed_because_unexpected_response_status";

		// Token: 0x0400032F RID: 815
		internal const string net_log_cache_removed_existing_based_on_policy = "net_log_cache_removed_existing_based_on_policy";

		// Token: 0x04000330 RID: 816
		internal const string net_log_cache_not_updated_based_on_ftp_response_status = "net_log_cache_not_updated_based_on_ftp_response_status";

		// Token: 0x04000331 RID: 817
		internal const string net_log_cache_update_not_supported_for_ftp_restart = "net_log_cache_update_not_supported_for_ftp_restart";

		// Token: 0x04000332 RID: 818
		internal const string net_log_cache_removed_entry_because_ftp_restart_response_changed = "net_log_cache_removed_entry_because_ftp_restart_response_changed";

		// Token: 0x04000333 RID: 819
		internal const string net_log_cache_last_synchronized = "net_log_cache_last_synchronized";

		// Token: 0x04000334 RID: 820
		internal const string net_log_cache_suppress_update_because_synched_last_minute = "net_log_cache_suppress_update_because_synched_last_minute";

		// Token: 0x04000335 RID: 821
		internal const string net_log_cache_updating_last_synchronized = "net_log_cache_updating_last_synchronized";

		// Token: 0x04000336 RID: 822
		internal const string net_log_cache_cannot_remove = "net_log_cache_cannot_remove";

		// Token: 0x04000337 RID: 823
		internal const string net_log_cache_key_status = "net_log_cache_key_status";

		// Token: 0x04000338 RID: 824
		internal const string net_log_cache_key_remove_failed_status = "net_log_cache_key_remove_failed_status";

		// Token: 0x04000339 RID: 825
		internal const string net_log_cache_usecount_file = "net_log_cache_usecount_file";

		// Token: 0x0400033A RID: 826
		internal const string net_log_cache_stream = "net_log_cache_stream";

		// Token: 0x0400033B RID: 827
		internal const string net_log_cache_filename = "net_log_cache_filename";

		// Token: 0x0400033C RID: 828
		internal const string net_log_cache_lookup_failed = "net_log_cache_lookup_failed";

		// Token: 0x0400033D RID: 829
		internal const string net_log_cache_exception = "net_log_cache_exception";

		// Token: 0x0400033E RID: 830
		internal const string net_log_cache_expected_length = "net_log_cache_expected_length";

		// Token: 0x0400033F RID: 831
		internal const string net_log_cache_last_modified = "net_log_cache_last_modified";

		// Token: 0x04000340 RID: 832
		internal const string net_log_cache_expires = "net_log_cache_expires";

		// Token: 0x04000341 RID: 833
		internal const string net_log_cache_max_stale = "net_log_cache_max_stale";

		// Token: 0x04000342 RID: 834
		internal const string net_log_cache_dumping_metadata = "net_log_cache_dumping_metadata";

		// Token: 0x04000343 RID: 835
		internal const string net_log_cache_create_failed = "net_log_cache_create_failed";

		// Token: 0x04000344 RID: 836
		internal const string net_log_cache_set_expires = "net_log_cache_set_expires";

		// Token: 0x04000345 RID: 837
		internal const string net_log_cache_set_last_modified = "net_log_cache_set_last_modified";

		// Token: 0x04000346 RID: 838
		internal const string net_log_cache_set_last_synchronized = "net_log_cache_set_last_synchronized";

		// Token: 0x04000347 RID: 839
		internal const string net_log_cache_enable_max_stale = "net_log_cache_enable_max_stale";

		// Token: 0x04000348 RID: 840
		internal const string net_log_cache_disable_max_stale = "net_log_cache_disable_max_stale";

		// Token: 0x04000349 RID: 841
		internal const string net_log_cache_set_new_metadata = "net_log_cache_set_new_metadata";

		// Token: 0x0400034A RID: 842
		internal const string net_log_cache_dumping = "net_log_cache_dumping";

		// Token: 0x0400034B RID: 843
		internal const string net_log_cache_key = "net_log_cache_key";

		// Token: 0x0400034C RID: 844
		internal const string net_log_cache_no_commit = "net_log_cache_no_commit";

		// Token: 0x0400034D RID: 845
		internal const string net_log_cache_error_deleting_filename = "net_log_cache_error_deleting_filename";

		// Token: 0x0400034E RID: 846
		internal const string net_log_cache_update_failed = "net_log_cache_update_failed";

		// Token: 0x0400034F RID: 847
		internal const string net_log_cache_delete_failed = "net_log_cache_delete_failed";

		// Token: 0x04000350 RID: 848
		internal const string net_log_cache_commit_failed = "net_log_cache_commit_failed";

		// Token: 0x04000351 RID: 849
		internal const string net_log_cache_committed_as_partial = "net_log_cache_committed_as_partial";

		// Token: 0x04000352 RID: 850
		internal const string net_log_cache_max_stale_and_update_status = "net_log_cache_max_stale_and_update_status";

		// Token: 0x04000353 RID: 851
		internal const string net_log_cache_failing_request_with_exception = "net_log_cache_failing_request_with_exception";

		// Token: 0x04000354 RID: 852
		internal const string net_log_cache_request_method = "net_log_cache_request_method";

		// Token: 0x04000355 RID: 853
		internal const string net_log_cache_http_status_parse_failure = "net_log_cache_http_status_parse_failure";

		// Token: 0x04000356 RID: 854
		internal const string net_log_cache_http_status_line = "net_log_cache_http_status_line";

		// Token: 0x04000357 RID: 855
		internal const string net_log_cache_cache_control = "net_log_cache_cache_control";

		// Token: 0x04000358 RID: 856
		internal const string net_log_cache_invalid_http_version = "net_log_cache_invalid_http_version";

		// Token: 0x04000359 RID: 857
		internal const string net_log_cache_no_http_response_header = "net_log_cache_no_http_response_header";

		// Token: 0x0400035A RID: 858
		internal const string net_log_cache_http_header_parse_error = "net_log_cache_http_header_parse_error";

		// Token: 0x0400035B RID: 859
		internal const string net_log_cache_metadata_name_value_parse_error = "net_log_cache_metadata_name_value_parse_error";

		// Token: 0x0400035C RID: 860
		internal const string net_log_cache_content_range_error = "net_log_cache_content_range_error";

		// Token: 0x0400035D RID: 861
		internal const string net_log_cache_cache_control_error = "net_log_cache_cache_control_error";

		// Token: 0x0400035E RID: 862
		internal const string net_log_cache_unexpected_status = "net_log_cache_unexpected_status";

		// Token: 0x0400035F RID: 863
		internal const string net_log_cache_object_and_exception = "net_log_cache_object_and_exception";

		// Token: 0x04000360 RID: 864
		internal const string net_log_cache_revalidation_not_needed = "net_log_cache_revalidation_not_needed";

		// Token: 0x04000361 RID: 865
		internal const string net_log_cache_not_updated_based_on_cache_protocol_status = "net_log_cache_not_updated_based_on_cache_protocol_status";

		// Token: 0x04000362 RID: 866
		internal const string net_log_cache_closing_cache_stream = "net_log_cache_closing_cache_stream";

		// Token: 0x04000363 RID: 867
		internal const string net_log_cache_exception_ignored = "net_log_cache_exception_ignored";

		// Token: 0x04000364 RID: 868
		internal const string net_log_cache_no_cache_entry = "net_log_cache_no_cache_entry";

		// Token: 0x04000365 RID: 869
		internal const string net_log_cache_null_cached_stream = "net_log_cache_null_cached_stream";

		// Token: 0x04000366 RID: 870
		internal const string net_log_cache_requested_combined_but_null_cached_stream = "net_log_cache_requested_combined_but_null_cached_stream";

		// Token: 0x04000367 RID: 871
		internal const string net_log_cache_returned_range_cache = "net_log_cache_returned_range_cache";

		// Token: 0x04000368 RID: 872
		internal const string net_log_cache_entry_not_found_freshness_undefined = "net_log_cache_entry_not_found_freshness_undefined";

		// Token: 0x04000369 RID: 873
		internal const string net_log_cache_dumping_cache_context = "net_log_cache_dumping_cache_context";

		// Token: 0x0400036A RID: 874
		internal const string net_log_cache_result = "net_log_cache_result";

		// Token: 0x0400036B RID: 875
		internal const string net_log_cache_uri_with_query_has_no_expiration = "net_log_cache_uri_with_query_has_no_expiration";

		// Token: 0x0400036C RID: 876
		internal const string net_log_cache_uri_with_query_and_cached_resp_from_http_10 = "net_log_cache_uri_with_query_and_cached_resp_from_http_10";

		// Token: 0x0400036D RID: 877
		internal const string net_log_cache_valid_as_fresh_or_because_policy = "net_log_cache_valid_as_fresh_or_because_policy";

		// Token: 0x0400036E RID: 878
		internal const string net_log_cache_accept_based_on_retry_count = "net_log_cache_accept_based_on_retry_count";

		// Token: 0x0400036F RID: 879
		internal const string net_log_cache_date_header_older_than_cache_entry = "net_log_cache_date_header_older_than_cache_entry";

		// Token: 0x04000370 RID: 880
		internal const string net_log_cache_server_didnt_satisfy_range = "net_log_cache_server_didnt_satisfy_range";

		// Token: 0x04000371 RID: 881
		internal const string net_log_cache_304_received_on_unconditional_request = "net_log_cache_304_received_on_unconditional_request";

		// Token: 0x04000372 RID: 882
		internal const string net_log_cache_304_received_on_unconditional_request_expected_200_206 = "net_log_cache_304_received_on_unconditional_request_expected_200_206";

		// Token: 0x04000373 RID: 883
		internal const string net_log_cache_last_modified_header_older_than_cache_entry = "net_log_cache_last_modified_header_older_than_cache_entry";

		// Token: 0x04000374 RID: 884
		internal const string net_log_cache_freshness_outside_policy_limits = "net_log_cache_freshness_outside_policy_limits";

		// Token: 0x04000375 RID: 885
		internal const string net_log_cache_need_to_remove_invalid_cache_entry_304 = "net_log_cache_need_to_remove_invalid_cache_entry_304";

		// Token: 0x04000376 RID: 886
		internal const string net_log_cache_resp_status = "net_log_cache_resp_status";

		// Token: 0x04000377 RID: 887
		internal const string net_log_cache_resp_304_or_request_head = "net_log_cache_resp_304_or_request_head";

		// Token: 0x04000378 RID: 888
		internal const string net_log_cache_dont_update_cached_headers = "net_log_cache_dont_update_cached_headers";

		// Token: 0x04000379 RID: 889
		internal const string net_log_cache_update_cached_headers = "net_log_cache_update_cached_headers";

		// Token: 0x0400037A RID: 890
		internal const string net_log_cache_partial_resp_not_combined_with_existing_entry = "net_log_cache_partial_resp_not_combined_with_existing_entry";

		// Token: 0x0400037B RID: 891
		internal const string net_log_cache_request_contains_conditional_header = "net_log_cache_request_contains_conditional_header";

		// Token: 0x0400037C RID: 892
		internal const string net_log_cache_not_a_get_head_post = "net_log_cache_not_a_get_head_post";

		// Token: 0x0400037D RID: 893
		internal const string net_log_cache_cannot_update_cache_if_304 = "net_log_cache_cannot_update_cache_if_304";

		// Token: 0x0400037E RID: 894
		internal const string net_log_cache_cannot_update_cache_with_head_resp = "net_log_cache_cannot_update_cache_with_head_resp";

		// Token: 0x0400037F RID: 895
		internal const string net_log_cache_http_resp_is_null = "net_log_cache_http_resp_is_null";

		// Token: 0x04000380 RID: 896
		internal const string net_log_cache_resp_cache_control_is_no_store = "net_log_cache_resp_cache_control_is_no_store";

		// Token: 0x04000381 RID: 897
		internal const string net_log_cache_resp_cache_control_is_public = "net_log_cache_resp_cache_control_is_public";

		// Token: 0x04000382 RID: 898
		internal const string net_log_cache_resp_cache_control_is_private = "net_log_cache_resp_cache_control_is_private";

		// Token: 0x04000383 RID: 899
		internal const string net_log_cache_resp_cache_control_is_private_plus_headers = "net_log_cache_resp_cache_control_is_private_plus_headers";

		// Token: 0x04000384 RID: 900
		internal const string net_log_cache_resp_older_than_cache = "net_log_cache_resp_older_than_cache";

		// Token: 0x04000385 RID: 901
		internal const string net_log_cache_revalidation_required = "net_log_cache_revalidation_required";

		// Token: 0x04000386 RID: 902
		internal const string net_log_cache_needs_revalidation = "net_log_cache_needs_revalidation";

		// Token: 0x04000387 RID: 903
		internal const string net_log_cache_resp_allows_caching = "net_log_cache_resp_allows_caching";

		// Token: 0x04000388 RID: 904
		internal const string net_log_cache_auth_header_and_no_s_max_age = "net_log_cache_auth_header_and_no_s_max_age";

		// Token: 0x04000389 RID: 905
		internal const string net_log_cache_post_resp_without_cache_control_or_expires = "net_log_cache_post_resp_without_cache_control_or_expires";

		// Token: 0x0400038A RID: 906
		internal const string net_log_cache_valid_based_on_status_code = "net_log_cache_valid_based_on_status_code";

		// Token: 0x0400038B RID: 907
		internal const string net_log_cache_resp_no_cache_control = "net_log_cache_resp_no_cache_control";

		// Token: 0x0400038C RID: 908
		internal const string net_log_cache_age = "net_log_cache_age";

		// Token: 0x0400038D RID: 909
		internal const string net_log_cache_policy_min_fresh = "net_log_cache_policy_min_fresh";

		// Token: 0x0400038E RID: 910
		internal const string net_log_cache_policy_max_age = "net_log_cache_policy_max_age";

		// Token: 0x0400038F RID: 911
		internal const string net_log_cache_policy_cache_sync_date = "net_log_cache_policy_cache_sync_date";

		// Token: 0x04000390 RID: 912
		internal const string net_log_cache_policy_max_stale = "net_log_cache_policy_max_stale";

		// Token: 0x04000391 RID: 913
		internal const string net_log_cache_control_no_cache = "net_log_cache_control_no_cache";

		// Token: 0x04000392 RID: 914
		internal const string net_log_cache_control_no_cache_removing_some_headers = "net_log_cache_control_no_cache_removing_some_headers";

		// Token: 0x04000393 RID: 915
		internal const string net_log_cache_control_must_revalidate = "net_log_cache_control_must_revalidate";

		// Token: 0x04000394 RID: 916
		internal const string net_log_cache_cached_auth_header = "net_log_cache_cached_auth_header";

		// Token: 0x04000395 RID: 917
		internal const string net_log_cache_cached_auth_header_no_control_directive = "net_log_cache_cached_auth_header_no_control_directive";

		// Token: 0x04000396 RID: 918
		internal const string net_log_cache_after_validation = "net_log_cache_after_validation";

		// Token: 0x04000397 RID: 919
		internal const string net_log_cache_resp_status_304 = "net_log_cache_resp_status_304";

		// Token: 0x04000398 RID: 920
		internal const string net_log_cache_head_resp_has_different_content_length = "net_log_cache_head_resp_has_different_content_length";

		// Token: 0x04000399 RID: 921
		internal const string net_log_cache_head_resp_has_different_content_md5 = "net_log_cache_head_resp_has_different_content_md5";

		// Token: 0x0400039A RID: 922
		internal const string net_log_cache_head_resp_has_different_etag = "net_log_cache_head_resp_has_different_etag";

		// Token: 0x0400039B RID: 923
		internal const string net_log_cache_304_head_resp_has_different_last_modified = "net_log_cache_304_head_resp_has_different_last_modified";

		// Token: 0x0400039C RID: 924
		internal const string net_log_cache_existing_entry_has_to_be_discarded = "net_log_cache_existing_entry_has_to_be_discarded";

		// Token: 0x0400039D RID: 925
		internal const string net_log_cache_existing_entry_should_be_discarded = "net_log_cache_existing_entry_should_be_discarded";

		// Token: 0x0400039E RID: 926
		internal const string net_log_cache_206_resp_non_matching_entry = "net_log_cache_206_resp_non_matching_entry";

		// Token: 0x0400039F RID: 927
		internal const string net_log_cache_206_resp_starting_position_not_adjusted = "net_log_cache_206_resp_starting_position_not_adjusted";

		// Token: 0x040003A0 RID: 928
		internal const string net_log_cache_combined_resp_requested = "net_log_cache_combined_resp_requested";

		// Token: 0x040003A1 RID: 929
		internal const string net_log_cache_updating_headers_on_304 = "net_log_cache_updating_headers_on_304";

		// Token: 0x040003A2 RID: 930
		internal const string net_log_cache_suppressing_headers_update_on_304 = "net_log_cache_suppressing_headers_update_on_304";

		// Token: 0x040003A3 RID: 931
		internal const string net_log_cache_status_code_not_304_206 = "net_log_cache_status_code_not_304_206";

		// Token: 0x040003A4 RID: 932
		internal const string net_log_cache_sxx_resp_cache_only = "net_log_cache_sxx_resp_cache_only";

		// Token: 0x040003A5 RID: 933
		internal const string net_log_cache_sxx_resp_can_be_replaced = "net_log_cache_sxx_resp_can_be_replaced";

		// Token: 0x040003A6 RID: 934
		internal const string net_log_cache_vary_header_empty = "net_log_cache_vary_header_empty";

		// Token: 0x040003A7 RID: 935
		internal const string net_log_cache_vary_header_contains_asterisks = "net_log_cache_vary_header_contains_asterisks";

		// Token: 0x040003A8 RID: 936
		internal const string net_log_cache_no_headers_in_metadata = "net_log_cache_no_headers_in_metadata";

		// Token: 0x040003A9 RID: 937
		internal const string net_log_cache_vary_header_mismatched_count = "net_log_cache_vary_header_mismatched_count";

		// Token: 0x040003AA RID: 938
		internal const string net_log_cache_vary_header_mismatched_field = "net_log_cache_vary_header_mismatched_field";

		// Token: 0x040003AB RID: 939
		internal const string net_log_cache_vary_header_match = "net_log_cache_vary_header_match";

		// Token: 0x040003AC RID: 940
		internal const string net_log_cache_range = "net_log_cache_range";

		// Token: 0x040003AD RID: 941
		internal const string net_log_cache_range_invalid_format = "net_log_cache_range_invalid_format";

		// Token: 0x040003AE RID: 942
		internal const string net_log_cache_range_not_in_cache = "net_log_cache_range_not_in_cache";

		// Token: 0x040003AF RID: 943
		internal const string net_log_cache_range_in_cache = "net_log_cache_range_in_cache";

		// Token: 0x040003B0 RID: 944
		internal const string net_log_cache_partial_resp = "net_log_cache_partial_resp";

		// Token: 0x040003B1 RID: 945
		internal const string net_log_cache_range_request_range = "net_log_cache_range_request_range";

		// Token: 0x040003B2 RID: 946
		internal const string net_log_cache_could_be_partial = "net_log_cache_could_be_partial";

		// Token: 0x040003B3 RID: 947
		internal const string net_log_cache_condition_if_none_match = "net_log_cache_condition_if_none_match";

		// Token: 0x040003B4 RID: 948
		internal const string net_log_cache_condition_if_modified_since = "net_log_cache_condition_if_modified_since";

		// Token: 0x040003B5 RID: 949
		internal const string net_log_cache_cannot_construct_conditional_request = "net_log_cache_cannot_construct_conditional_request";

		// Token: 0x040003B6 RID: 950
		internal const string net_log_cache_cannot_construct_conditional_range_request = "net_log_cache_cannot_construct_conditional_range_request";

		// Token: 0x040003B7 RID: 951
		internal const string net_log_cache_entry_size_too_big = "net_log_cache_entry_size_too_big";

		// Token: 0x040003B8 RID: 952
		internal const string net_log_cache_condition_if_range = "net_log_cache_condition_if_range";

		// Token: 0x040003B9 RID: 953
		internal const string net_log_cache_conditional_range_not_implemented_on_http_10 = "net_log_cache_conditional_range_not_implemented_on_http_10";

		// Token: 0x040003BA RID: 954
		internal const string net_log_cache_saving_request_headers = "net_log_cache_saving_request_headers";

		// Token: 0x040003BB RID: 955
		internal const string net_log_cache_only_byte_range_implemented = "net_log_cache_only_byte_range_implemented";

		// Token: 0x040003BC RID: 956
		internal const string net_log_cache_multiple_complex_range_not_implemented = "net_log_cache_multiple_complex_range_not_implemented";

		// Token: 0x040003BD RID: 957
		internal const string net_log_unknown = "net_log_unknown";

		// Token: 0x040003BE RID: 958
		internal const string net_log_operation_returned_something = "net_log_operation_returned_something";

		// Token: 0x040003BF RID: 959
		internal const string net_log_operation_failed_with_error = "net_log_operation_failed_with_error";

		// Token: 0x040003C0 RID: 960
		internal const string net_log_buffered_n_bytes = "net_log_buffered_n_bytes";

		// Token: 0x040003C1 RID: 961
		internal const string net_log_method_equal = "net_log_method_equal";

		// Token: 0x040003C2 RID: 962
		internal const string net_log_releasing_connection = "net_log_releasing_connection";

		// Token: 0x040003C3 RID: 963
		internal const string net_log_unexpected_exception = "net_log_unexpected_exception";

		// Token: 0x040003C4 RID: 964
		internal const string net_log_server_response_error_code = "net_log_server_response_error_code";

		// Token: 0x040003C5 RID: 965
		internal const string net_log_resubmitting_request = "net_log_resubmitting_request";

		// Token: 0x040003C6 RID: 966
		internal const string net_log_retrieving_localhost_exception = "net_log_retrieving_localhost_exception";

		// Token: 0x040003C7 RID: 967
		internal const string net_log_resolved_servicepoint_may_not_be_remote_server = "net_log_resolved_servicepoint_may_not_be_remote_server";

		// Token: 0x040003C8 RID: 968
		internal const string net_log_received_status_line = "net_log_received_status_line";

		// Token: 0x040003C9 RID: 969
		internal const string net_log_sending_headers = "net_log_sending_headers";

		// Token: 0x040003CA RID: 970
		internal const string net_log_received_headers = "net_log_received_headers";

		// Token: 0x040003CB RID: 971
		internal const string net_log_shell_expression_pattern_format_warning = "net_log_shell_expression_pattern_format_warning";

		// Token: 0x040003CC RID: 972
		internal const string net_log_exception_in_callback = "net_log_exception_in_callback";

		// Token: 0x040003CD RID: 973
		internal const string net_log_sending_command = "net_log_sending_command";

		// Token: 0x040003CE RID: 974
		internal const string net_log_received_response = "net_log_received_response";

		// Token: 0x040003CF RID: 975
		internal const string Mail7BitStreamInvalidCharacter = "Mail7BitStreamInvalidCharacter";

		// Token: 0x040003D0 RID: 976
		internal const string MailAddressInvalidFormat = "MailAddressInvalidFormat";

		// Token: 0x040003D1 RID: 977
		internal const string MailAddressUnsupportedFormat = "MailAddressUnsupportedFormat";

		// Token: 0x040003D2 RID: 978
		internal const string MailSubjectInvalidFormat = "MailSubjectInvalidFormat";

		// Token: 0x040003D3 RID: 979
		internal const string MailBase64InvalidCharacter = "MailBase64InvalidCharacter";

		// Token: 0x040003D4 RID: 980
		internal const string MailCollectionIsReadOnly = "MailCollectionIsReadOnly";

		// Token: 0x040003D5 RID: 981
		internal const string MailDateInvalidFormat = "MailDateInvalidFormat";

		// Token: 0x040003D6 RID: 982
		internal const string MailHeaderFieldAlreadyExists = "MailHeaderFieldAlreadyExists";

		// Token: 0x040003D7 RID: 983
		internal const string MailHeaderFieldInvalidCharacter = "MailHeaderFieldInvalidCharacter";

		// Token: 0x040003D8 RID: 984
		internal const string MailHeaderFieldMalformedHeader = "MailHeaderFieldMalformedHeader";

		// Token: 0x040003D9 RID: 985
		internal const string MailHeaderFieldMismatchedName = "MailHeaderFieldMismatchedName";

		// Token: 0x040003DA RID: 986
		internal const string MailHeaderIndexOutOfBounds = "MailHeaderIndexOutOfBounds";

		// Token: 0x040003DB RID: 987
		internal const string MailHeaderItemAccessorOnlySingleton = "MailHeaderItemAccessorOnlySingleton";

		// Token: 0x040003DC RID: 988
		internal const string MailHeaderListHasChanged = "MailHeaderListHasChanged";

		// Token: 0x040003DD RID: 989
		internal const string MailHeaderResetCalledBeforeEOF = "MailHeaderResetCalledBeforeEOF";

		// Token: 0x040003DE RID: 990
		internal const string MailHeaderTargetArrayTooSmall = "MailHeaderTargetArrayTooSmall";

		// Token: 0x040003DF RID: 991
		internal const string MailHeaderInvalidCID = "MailHeaderInvalidCID";

		// Token: 0x040003E0 RID: 992
		internal const string MailHostNotFound = "MailHostNotFound";

		// Token: 0x040003E1 RID: 993
		internal const string MailReaderGetContentStreamAlreadyCalled = "MailReaderGetContentStreamAlreadyCalled";

		// Token: 0x040003E2 RID: 994
		internal const string MailReaderTruncated = "MailReaderTruncated";

		// Token: 0x040003E3 RID: 995
		internal const string MailWriterIsInContent = "MailWriterIsInContent";

		// Token: 0x040003E4 RID: 996
		internal const string MailWriterLineLengthTooSmall = "MailWriterLineLengthTooSmall";

		// Token: 0x040003E5 RID: 997
		internal const string MailServerDoesNotSupportStartTls = "MailServerDoesNotSupportStartTls";

		// Token: 0x040003E6 RID: 998
		internal const string MailServerResponse = "MailServerResponse";

		// Token: 0x040003E7 RID: 999
		internal const string SSPIAuthenticationOrSPNNull = "SSPIAuthenticationOrSPNNull";

		// Token: 0x040003E8 RID: 1000
		internal const string SSPIPInvokeError = "SSPIPInvokeError";

		// Token: 0x040003E9 RID: 1001
		internal const string SSPIInvalidHandleType = "SSPIInvalidHandleType";

		// Token: 0x040003EA RID: 1002
		internal const string SmtpAlreadyConnected = "SmtpAlreadyConnected";

		// Token: 0x040003EB RID: 1003
		internal const string SmtpAuthenticationFailed = "SmtpAuthenticationFailed";

		// Token: 0x040003EC RID: 1004
		internal const string SmtpAuthenticationFailedNoCreds = "SmtpAuthenticationFailedNoCreds";

		// Token: 0x040003ED RID: 1005
		internal const string SmtpDataStreamOpen = "SmtpDataStreamOpen";

		// Token: 0x040003EE RID: 1006
		internal const string SmtpDefaultMimePreamble = "SmtpDefaultMimePreamble";

		// Token: 0x040003EF RID: 1007
		internal const string SmtpDefaultSubject = "SmtpDefaultSubject";

		// Token: 0x040003F0 RID: 1008
		internal const string SmtpInvalidResponse = "SmtpInvalidResponse";

		// Token: 0x040003F1 RID: 1009
		internal const string SmtpNotConnected = "SmtpNotConnected";

		// Token: 0x040003F2 RID: 1010
		internal const string SmtpSystemStatus = "SmtpSystemStatus";

		// Token: 0x040003F3 RID: 1011
		internal const string SmtpHelpMessage = "SmtpHelpMessage";

		// Token: 0x040003F4 RID: 1012
		internal const string SmtpServiceReady = "SmtpServiceReady";

		// Token: 0x040003F5 RID: 1013
		internal const string SmtpServiceClosingTransmissionChannel = "SmtpServiceClosingTransmissionChannel";

		// Token: 0x040003F6 RID: 1014
		internal const string SmtpOK = "SmtpOK";

		// Token: 0x040003F7 RID: 1015
		internal const string SmtpUserNotLocalWillForward = "SmtpUserNotLocalWillForward";

		// Token: 0x040003F8 RID: 1016
		internal const string SmtpStartMailInput = "SmtpStartMailInput";

		// Token: 0x040003F9 RID: 1017
		internal const string SmtpServiceNotAvailable = "SmtpServiceNotAvailable";

		// Token: 0x040003FA RID: 1018
		internal const string SmtpMailboxBusy = "SmtpMailboxBusy";

		// Token: 0x040003FB RID: 1019
		internal const string SmtpLocalErrorInProcessing = "SmtpLocalErrorInProcessing";

		// Token: 0x040003FC RID: 1020
		internal const string SmtpInsufficientStorage = "SmtpInsufficientStorage";

		// Token: 0x040003FD RID: 1021
		internal const string SmtpPermissionDenied = "SmtpPermissionDenied";

		// Token: 0x040003FE RID: 1022
		internal const string SmtpCommandUnrecognized = "SmtpCommandUnrecognized";

		// Token: 0x040003FF RID: 1023
		internal const string SmtpSyntaxError = "SmtpSyntaxError";

		// Token: 0x04000400 RID: 1024
		internal const string SmtpCommandNotImplemented = "SmtpCommandNotImplemented";

		// Token: 0x04000401 RID: 1025
		internal const string SmtpBadCommandSequence = "SmtpBadCommandSequence";

		// Token: 0x04000402 RID: 1026
		internal const string SmtpCommandParameterNotImplemented = "SmtpCommandParameterNotImplemented";

		// Token: 0x04000403 RID: 1027
		internal const string SmtpMailboxUnavailable = "SmtpMailboxUnavailable";

		// Token: 0x04000404 RID: 1028
		internal const string SmtpUserNotLocalTryAlternatePath = "SmtpUserNotLocalTryAlternatePath";

		// Token: 0x04000405 RID: 1029
		internal const string SmtpExceededStorageAllocation = "SmtpExceededStorageAllocation";

		// Token: 0x04000406 RID: 1030
		internal const string SmtpMailboxNameNotAllowed = "SmtpMailboxNameNotAllowed";

		// Token: 0x04000407 RID: 1031
		internal const string SmtpTransactionFailed = "SmtpTransactionFailed";

		// Token: 0x04000408 RID: 1032
		internal const string SmtpSendMailFailure = "SmtpSendMailFailure";

		// Token: 0x04000409 RID: 1033
		internal const string SmtpRecipientFailed = "SmtpRecipientFailed";

		// Token: 0x0400040A RID: 1034
		internal const string SmtpRecipientRequired = "SmtpRecipientRequired";

		// Token: 0x0400040B RID: 1035
		internal const string SmtpFromRequired = "SmtpFromRequired";

		// Token: 0x0400040C RID: 1036
		internal const string SmtpAllRecipientsFailed = "SmtpAllRecipientsFailed";

		// Token: 0x0400040D RID: 1037
		internal const string SmtpClientNotPermitted = "SmtpClientNotPermitted";

		// Token: 0x0400040E RID: 1038
		internal const string SmtpMustIssueStartTlsFirst = "SmtpMustIssueStartTlsFirst";

		// Token: 0x0400040F RID: 1039
		internal const string SmtpNeedAbsolutePickupDirectory = "SmtpNeedAbsolutePickupDirectory";

		// Token: 0x04000410 RID: 1040
		internal const string SmtpGetIisPickupDirectoryFailed = "SmtpGetIisPickupDirectoryFailed";

		// Token: 0x04000411 RID: 1041
		internal const string SmtpPickupDirectoryDoesnotSupportSsl = "SmtpPickupDirectoryDoesnotSupportSsl";

		// Token: 0x04000412 RID: 1042
		internal const string SmtpOperationInProgress = "SmtpOperationInProgress";

		// Token: 0x04000413 RID: 1043
		internal const string SmtpAuthResponseInvalid = "SmtpAuthResponseInvalid";

		// Token: 0x04000414 RID: 1044
		internal const string SmtpEhloResponseInvalid = "SmtpEhloResponseInvalid";

		// Token: 0x04000415 RID: 1045
		internal const string MimeTransferEncodingNotSupported = "MimeTransferEncodingNotSupported";

		// Token: 0x04000416 RID: 1046
		internal const string SeekNotSupported = "SeekNotSupported";

		// Token: 0x04000417 RID: 1047
		internal const string WriteNotSupported = "WriteNotSupported";

		// Token: 0x04000418 RID: 1048
		internal const string InvalidHexDigit = "InvalidHexDigit";

		// Token: 0x04000419 RID: 1049
		internal const string InvalidSSPIContext = "InvalidSSPIContext";

		// Token: 0x0400041A RID: 1050
		internal const string InvalidSSPIContextKey = "InvalidSSPIContextKey";

		// Token: 0x0400041B RID: 1051
		internal const string InvalidSSPINegotiationElement = "InvalidSSPINegotiationElement";

		// Token: 0x0400041C RID: 1052
		internal const string InvalidHeaderName = "InvalidHeaderName";

		// Token: 0x0400041D RID: 1053
		internal const string InvalidHeaderValue = "InvalidHeaderValue";

		// Token: 0x0400041E RID: 1054
		internal const string CannotGetEffectiveTimeOfSSPIContext = "CannotGetEffectiveTimeOfSSPIContext";

		// Token: 0x0400041F RID: 1055
		internal const string CannotGetExpiryTimeOfSSPIContext = "CannotGetExpiryTimeOfSSPIContext";

		// Token: 0x04000420 RID: 1056
		internal const string ReadNotSupported = "ReadNotSupported";

		// Token: 0x04000421 RID: 1057
		internal const string InvalidAsyncResult = "InvalidAsyncResult";

		// Token: 0x04000422 RID: 1058
		internal const string UnspecifiedHost = "UnspecifiedHost";

		// Token: 0x04000423 RID: 1059
		internal const string InvalidPort = "InvalidPort";

		// Token: 0x04000424 RID: 1060
		internal const string SmtpInvalidOperationDuringSend = "SmtpInvalidOperationDuringSend";

		// Token: 0x04000425 RID: 1061
		internal const string MimePartCantResetStream = "MimePartCantResetStream";

		// Token: 0x04000426 RID: 1062
		internal const string MediaTypeInvalid = "MediaTypeInvalid";

		// Token: 0x04000427 RID: 1063
		internal const string ContentTypeInvalid = "ContentTypeInvalid";

		// Token: 0x04000428 RID: 1064
		internal const string ContentDispositionInvalid = "ContentDispositionInvalid";

		// Token: 0x04000429 RID: 1065
		internal const string AttributeNotSupported = "AttributeNotSupported";

		// Token: 0x0400042A RID: 1066
		internal const string Cannot_remove_with_null = "Cannot_remove_with_null";

		// Token: 0x0400042B RID: 1067
		internal const string Config_base_elements_only = "Config_base_elements_only";

		// Token: 0x0400042C RID: 1068
		internal const string Config_base_no_child_nodes = "Config_base_no_child_nodes";

		// Token: 0x0400042D RID: 1069
		internal const string Config_base_required_attribute_empty = "Config_base_required_attribute_empty";

		// Token: 0x0400042E RID: 1070
		internal const string Config_base_required_attribute_missing = "Config_base_required_attribute_missing";

		// Token: 0x0400042F RID: 1071
		internal const string Config_base_time_overflow = "Config_base_time_overflow";

		// Token: 0x04000430 RID: 1072
		internal const string Config_base_type_must_be_configurationvalidation = "Config_base_type_must_be_configurationvalidation";

		// Token: 0x04000431 RID: 1073
		internal const string Config_base_type_must_be_typeconverter = "Config_base_type_must_be_typeconverter";

		// Token: 0x04000432 RID: 1074
		internal const string Config_base_unknown_format = "Config_base_unknown_format";

		// Token: 0x04000433 RID: 1075
		internal const string Config_base_unrecognized_attribute = "Config_base_unrecognized_attribute";

		// Token: 0x04000434 RID: 1076
		internal const string Config_base_unrecognized_element = "Config_base_unrecognized_element";

		// Token: 0x04000435 RID: 1077
		internal const string Config_invalid_boolean_attribute = "Config_invalid_boolean_attribute";

		// Token: 0x04000436 RID: 1078
		internal const string Config_invalid_integer_attribute = "Config_invalid_integer_attribute";

		// Token: 0x04000437 RID: 1079
		internal const string Config_invalid_positive_integer_attribute = "Config_invalid_positive_integer_attribute";

		// Token: 0x04000438 RID: 1080
		internal const string Config_invalid_type_attribute = "Config_invalid_type_attribute";

		// Token: 0x04000439 RID: 1081
		internal const string Config_missing_required_attribute = "Config_missing_required_attribute";

		// Token: 0x0400043A RID: 1082
		internal const string Config_name_value_file_section_file_invalid_root = "Config_name_value_file_section_file_invalid_root";

		// Token: 0x0400043B RID: 1083
		internal const string Config_provider_must_implement_type = "Config_provider_must_implement_type";

		// Token: 0x0400043C RID: 1084
		internal const string Config_provider_name_null_or_empty = "Config_provider_name_null_or_empty";

		// Token: 0x0400043D RID: 1085
		internal const string Config_provider_not_found = "Config_provider_not_found";

		// Token: 0x0400043E RID: 1086
		internal const string Config_property_name_cannot_be_empty = "Config_property_name_cannot_be_empty";

		// Token: 0x0400043F RID: 1087
		internal const string Config_section_cannot_clear_locked_section = "Config_section_cannot_clear_locked_section";

		// Token: 0x04000440 RID: 1088
		internal const string Config_section_record_not_found = "Config_section_record_not_found";

		// Token: 0x04000441 RID: 1089
		internal const string Config_source_cannot_contain_file = "Config_source_cannot_contain_file";

		// Token: 0x04000442 RID: 1090
		internal const string Config_system_already_set = "Config_system_already_set";

		// Token: 0x04000443 RID: 1091
		internal const string Config_unable_to_read_security_policy = "Config_unable_to_read_security_policy";

		// Token: 0x04000444 RID: 1092
		internal const string Config_write_xml_returned_null = "Config_write_xml_returned_null";

		// Token: 0x04000445 RID: 1093
		internal const string Cannot_clear_sections_within_group = "Cannot_clear_sections_within_group";

		// Token: 0x04000446 RID: 1094
		internal const string Cannot_exit_up_top_directory = "Cannot_exit_up_top_directory";

		// Token: 0x04000447 RID: 1095
		internal const string Could_not_create_listener = "Could_not_create_listener";

		// Token: 0x04000448 RID: 1096
		internal const string TextWriterTL_DefaultConstructor_NotSupported = "TextWriterTL_DefaultConstructor_NotSupported";

		// Token: 0x04000449 RID: 1097
		internal const string Could_not_create_type_instance = "Could_not_create_type_instance";

		// Token: 0x0400044A RID: 1098
		internal const string Could_not_find_type = "Could_not_find_type";

		// Token: 0x0400044B RID: 1099
		internal const string Could_not_get_constructor = "Could_not_get_constructor";

		// Token: 0x0400044C RID: 1100
		internal const string EmptyTypeName_NotAllowed = "EmptyTypeName_NotAllowed";

		// Token: 0x0400044D RID: 1101
		internal const string Incorrect_base_type = "Incorrect_base_type";

		// Token: 0x0400044E RID: 1102
		internal const string Only_specify_one = "Only_specify_one";

		// Token: 0x0400044F RID: 1103
		internal const string Provider_Already_Initialized = "Provider_Already_Initialized";

		// Token: 0x04000450 RID: 1104
		internal const string Reference_listener_cant_have_properties = "Reference_listener_cant_have_properties";

		// Token: 0x04000451 RID: 1105
		internal const string Reference_to_nonexistent_listener = "Reference_to_nonexistent_listener";

		// Token: 0x04000452 RID: 1106
		internal const string SettingsPropertyNotFound = "SettingsPropertyNotFound";

		// Token: 0x04000453 RID: 1107
		internal const string SettingsPropertyReadOnly = "SettingsPropertyReadOnly";

		// Token: 0x04000454 RID: 1108
		internal const string SettingsPropertyWrongType = "SettingsPropertyWrongType";

		// Token: 0x04000455 RID: 1109
		internal const string Type_isnt_tracelistener = "Type_isnt_tracelistener";

		// Token: 0x04000456 RID: 1110
		internal const string Unable_to_convert_type_from_string = "Unable_to_convert_type_from_string";

		// Token: 0x04000457 RID: 1111
		internal const string Unable_to_convert_type_to_string = "Unable_to_convert_type_to_string";

		// Token: 0x04000458 RID: 1112
		internal const string Value_must_be_numeric = "Value_must_be_numeric";

		// Token: 0x04000459 RID: 1113
		internal const string Could_not_create_from_default_value = "Could_not_create_from_default_value";

		// Token: 0x0400045A RID: 1114
		internal const string Could_not_create_from_default_value_2 = "Could_not_create_from_default_value_2";

		// Token: 0x0400045B RID: 1115
		internal const string InvalidDirName = "InvalidDirName";

		// Token: 0x0400045C RID: 1116
		internal const string FSW_IOError = "FSW_IOError";

		// Token: 0x0400045D RID: 1117
		internal const string PatternInvalidChar = "PatternInvalidChar";

		// Token: 0x0400045E RID: 1118
		internal const string BufferSizeTooLarge = "BufferSizeTooLarge";

		// Token: 0x0400045F RID: 1119
		internal const string FSW_ChangedFilter = "FSW_ChangedFilter";

		// Token: 0x04000460 RID: 1120
		internal const string FSW_Enabled = "FSW_Enabled";

		// Token: 0x04000461 RID: 1121
		internal const string FSW_Filter = "FSW_Filter";

		// Token: 0x04000462 RID: 1122
		internal const string FSW_IncludeSubdirectories = "FSW_IncludeSubdirectories";

		// Token: 0x04000463 RID: 1123
		internal const string FSW_Path = "FSW_Path";

		// Token: 0x04000464 RID: 1124
		internal const string FSW_SynchronizingObject = "FSW_SynchronizingObject";

		// Token: 0x04000465 RID: 1125
		internal const string FSW_Changed = "FSW_Changed";

		// Token: 0x04000466 RID: 1126
		internal const string FSW_Created = "FSW_Created";

		// Token: 0x04000467 RID: 1127
		internal const string FSW_Deleted = "FSW_Deleted";

		// Token: 0x04000468 RID: 1128
		internal const string FSW_Renamed = "FSW_Renamed";

		// Token: 0x04000469 RID: 1129
		internal const string FSW_BufferOverflow = "FSW_BufferOverflow";

		// Token: 0x0400046A RID: 1130
		internal const string FileSystemWatcherDesc = "FileSystemWatcherDesc";

		// Token: 0x0400046B RID: 1131
		internal const string NotSet = "NotSet";

		// Token: 0x0400046C RID: 1132
		internal const string TimerAutoReset = "TimerAutoReset";

		// Token: 0x0400046D RID: 1133
		internal const string TimerEnabled = "TimerEnabled";

		// Token: 0x0400046E RID: 1134
		internal const string TimerInterval = "TimerInterval";

		// Token: 0x0400046F RID: 1135
		internal const string TimerIntervalElapsed = "TimerIntervalElapsed";

		// Token: 0x04000470 RID: 1136
		internal const string TimerSynchronizingObject = "TimerSynchronizingObject";

		// Token: 0x04000471 RID: 1137
		internal const string MismatchedCounterTypes = "MismatchedCounterTypes";

		// Token: 0x04000472 RID: 1138
		internal const string NoPropertyForAttribute = "NoPropertyForAttribute";

		// Token: 0x04000473 RID: 1139
		internal const string InvalidAttributeType = "InvalidAttributeType";

		// Token: 0x04000474 RID: 1140
		internal const string Generic_ArgCantBeEmptyString = "Generic_ArgCantBeEmptyString";

		// Token: 0x04000475 RID: 1141
		internal const string BadLogName = "BadLogName";

		// Token: 0x04000476 RID: 1142
		internal const string InvalidProperty = "InvalidProperty";

		// Token: 0x04000477 RID: 1143
		internal const string NotifyCreateFailed = "NotifyCreateFailed";

		// Token: 0x04000478 RID: 1144
		internal const string CantMonitorEventLog = "CantMonitorEventLog";

		// Token: 0x04000479 RID: 1145
		internal const string InitTwice = "InitTwice";

		// Token: 0x0400047A RID: 1146
		internal const string InvalidParameter = "InvalidParameter";

		// Token: 0x0400047B RID: 1147
		internal const string MissingParameter = "MissingParameter";

		// Token: 0x0400047C RID: 1148
		internal const string ParameterTooLong = "ParameterTooLong";

		// Token: 0x0400047D RID: 1149
		internal const string LocalSourceAlreadyExists = "LocalSourceAlreadyExists";

		// Token: 0x0400047E RID: 1150
		internal const string SourceAlreadyExists = "SourceAlreadyExists";

		// Token: 0x0400047F RID: 1151
		internal const string LocalLogAlreadyExistsAsSource = "LocalLogAlreadyExistsAsSource";

		// Token: 0x04000480 RID: 1152
		internal const string LogAlreadyExistsAsSource = "LogAlreadyExistsAsSource";

		// Token: 0x04000481 RID: 1153
		internal const string DuplicateLogName = "DuplicateLogName";

		// Token: 0x04000482 RID: 1154
		internal const string RegKeyMissing = "RegKeyMissing";

		// Token: 0x04000483 RID: 1155
		internal const string LocalRegKeyMissing = "LocalRegKeyMissing";

		// Token: 0x04000484 RID: 1156
		internal const string RegKeyMissingShort = "RegKeyMissingShort";

		// Token: 0x04000485 RID: 1157
		internal const string InvalidParameterFormat = "InvalidParameterFormat";

		// Token: 0x04000486 RID: 1158
		internal const string NoLogName = "NoLogName";

		// Token: 0x04000487 RID: 1159
		internal const string RegKeyNoAccess = "RegKeyNoAccess";

		// Token: 0x04000488 RID: 1160
		internal const string MissingLog = "MissingLog";

		// Token: 0x04000489 RID: 1161
		internal const string SourceNotRegistered = "SourceNotRegistered";

		// Token: 0x0400048A RID: 1162
		internal const string LocalSourceNotRegistered = "LocalSourceNotRegistered";

		// Token: 0x0400048B RID: 1163
		internal const string CantRetrieveEntries = "CantRetrieveEntries";

		// Token: 0x0400048C RID: 1164
		internal const string IndexOutOfBounds = "IndexOutOfBounds";

		// Token: 0x0400048D RID: 1165
		internal const string CantReadLogEntryAt = "CantReadLogEntryAt";

		// Token: 0x0400048E RID: 1166
		internal const string MissingLogProperty = "MissingLogProperty";

		// Token: 0x0400048F RID: 1167
		internal const string CantOpenLog = "CantOpenLog";

		// Token: 0x04000490 RID: 1168
		internal const string NeedSourceToOpen = "NeedSourceToOpen";

		// Token: 0x04000491 RID: 1169
		internal const string NeedSourceToWrite = "NeedSourceToWrite";

		// Token: 0x04000492 RID: 1170
		internal const string CantOpenLogAccess = "CantOpenLogAccess";

		// Token: 0x04000493 RID: 1171
		internal const string LogEntryTooLong = "LogEntryTooLong";

		// Token: 0x04000494 RID: 1172
		internal const string TooManyReplacementStrings = "TooManyReplacementStrings";

		// Token: 0x04000495 RID: 1173
		internal const string LogSourceMismatch = "LogSourceMismatch";

		// Token: 0x04000496 RID: 1174
		internal const string NoAccountInfo = "NoAccountInfo";

		// Token: 0x04000497 RID: 1175
		internal const string NoCurrentEntry = "NoCurrentEntry";

		// Token: 0x04000498 RID: 1176
		internal const string MessageNotFormatted = "MessageNotFormatted";

		// Token: 0x04000499 RID: 1177
		internal const string EventID = "EventID";

		// Token: 0x0400049A RID: 1178
		internal const string LogDoesNotExists = "LogDoesNotExists";

		// Token: 0x0400049B RID: 1179
		internal const string InvalidCustomerLogName = "InvalidCustomerLogName";

		// Token: 0x0400049C RID: 1180
		internal const string CannotDeleteEqualSource = "CannotDeleteEqualSource";

		// Token: 0x0400049D RID: 1181
		internal const string RentionDaysOutOfRange = "RentionDaysOutOfRange";

		// Token: 0x0400049E RID: 1182
		internal const string MaximumKilobytesOutOfRange = "MaximumKilobytesOutOfRange";

		// Token: 0x0400049F RID: 1183
		internal const string SomeLogsInaccessible = "SomeLogsInaccessible";

		// Token: 0x040004A0 RID: 1184
		internal const string BadConfigSwitchValue = "BadConfigSwitchValue";

		// Token: 0x040004A1 RID: 1185
		internal const string ConfigSectionsUnique = "ConfigSectionsUnique";

		// Token: 0x040004A2 RID: 1186
		internal const string ConfigSectionsUniquePerSection = "ConfigSectionsUniquePerSection";

		// Token: 0x040004A3 RID: 1187
		internal const string SourceListenerDoesntExist = "SourceListenerDoesntExist";

		// Token: 0x040004A4 RID: 1188
		internal const string SourceSwitchDoesntExist = "SourceSwitchDoesntExist";

		// Token: 0x040004A5 RID: 1189
		internal const string ReadOnlyCounter = "ReadOnlyCounter";

		// Token: 0x040004A6 RID: 1190
		internal const string ReadOnlyRemoveInstance = "ReadOnlyRemoveInstance";

		// Token: 0x040004A7 RID: 1191
		internal const string NotCustomCounter = "NotCustomCounter";

		// Token: 0x040004A8 RID: 1192
		internal const string CategoryNameMissing = "CategoryNameMissing";

		// Token: 0x040004A9 RID: 1193
		internal const string CounterNameMissing = "CounterNameMissing";

		// Token: 0x040004AA RID: 1194
		internal const string InstanceNameProhibited = "InstanceNameProhibited";

		// Token: 0x040004AB RID: 1195
		internal const string InstanceNameRequired = "InstanceNameRequired";

		// Token: 0x040004AC RID: 1196
		internal const string MissingInstance = "MissingInstance";

		// Token: 0x040004AD RID: 1197
		internal const string PerformanceCategoryExists = "PerformanceCategoryExists";

		// Token: 0x040004AE RID: 1198
		internal const string InvalidCounterName = "InvalidCounterName";

		// Token: 0x040004AF RID: 1199
		internal const string DuplicateCounterName = "DuplicateCounterName";

		// Token: 0x040004B0 RID: 1200
		internal const string CantDeleteCategory = "CantDeleteCategory";

		// Token: 0x040004B1 RID: 1201
		internal const string MissingCategory = "MissingCategory";

		// Token: 0x040004B2 RID: 1202
		internal const string MissingCategoryDetail = "MissingCategoryDetail";

		// Token: 0x040004B3 RID: 1203
		internal const string CantReadCategory = "CantReadCategory";

		// Token: 0x040004B4 RID: 1204
		internal const string MissingCounter = "MissingCounter";

		// Token: 0x040004B5 RID: 1205
		internal const string CategoryNameNotSet = "CategoryNameNotSet";

		// Token: 0x040004B6 RID: 1206
		internal const string CounterExists = "CounterExists";

		// Token: 0x040004B7 RID: 1207
		internal const string CantReadCategoryIndex = "CantReadCategoryIndex";

		// Token: 0x040004B8 RID: 1208
		internal const string CantReadCounter = "CantReadCounter";

		// Token: 0x040004B9 RID: 1209
		internal const string CantReadInstance = "CantReadInstance";

		// Token: 0x040004BA RID: 1210
		internal const string RemoteWriting = "RemoteWriting";

		// Token: 0x040004BB RID: 1211
		internal const string CounterLayout = "CounterLayout";

		// Token: 0x040004BC RID: 1212
		internal const string PossibleDeadlock = "PossibleDeadlock";

		// Token: 0x040004BD RID: 1213
		internal const string SharedMemoryGhosted = "SharedMemoryGhosted";

		// Token: 0x040004BE RID: 1214
		internal const string HelpNotAvailable = "HelpNotAvailable";

		// Token: 0x040004BF RID: 1215
		internal const string PerfInvalidHelp = "PerfInvalidHelp";

		// Token: 0x040004C0 RID: 1216
		internal const string PerfInvalidCounterName = "PerfInvalidCounterName";

		// Token: 0x040004C1 RID: 1217
		internal const string PerfInvalidCategoryName = "PerfInvalidCategoryName";

		// Token: 0x040004C2 RID: 1218
		internal const string MustAddCounterCreationData = "MustAddCounterCreationData";

		// Token: 0x040004C3 RID: 1219
		internal const string RemoteCounterAdmin = "RemoteCounterAdmin";

		// Token: 0x040004C4 RID: 1220
		internal const string NoInstanceInformation = "NoInstanceInformation";

		// Token: 0x040004C5 RID: 1221
		internal const string PerfCounterPdhError = "PerfCounterPdhError";

		// Token: 0x040004C6 RID: 1222
		internal const string MultiInstanceOnly = "MultiInstanceOnly";

		// Token: 0x040004C7 RID: 1223
		internal const string SingleInstanceOnly = "SingleInstanceOnly";

		// Token: 0x040004C8 RID: 1224
		internal const string InstanceNameTooLong = "InstanceNameTooLong";

		// Token: 0x040004C9 RID: 1225
		internal const string CategoryNameTooLong = "CategoryNameTooLong";

		// Token: 0x040004CA RID: 1226
		internal const string InstanceLifetimeProcessonReadOnly = "InstanceLifetimeProcessonReadOnly";

		// Token: 0x040004CB RID: 1227
		internal const string InstanceLifetimeProcessforSingleInstance = "InstanceLifetimeProcessforSingleInstance";

		// Token: 0x040004CC RID: 1228
		internal const string InstanceAlreadyExists = "InstanceAlreadyExists";

		// Token: 0x040004CD RID: 1229
		internal const string CantSetLifetimeAfterInitialized = "CantSetLifetimeAfterInitialized";

		// Token: 0x040004CE RID: 1230
		internal const string ProcessLifetimeNotValidInGlobal = "ProcessLifetimeNotValidInGlobal";

		// Token: 0x040004CF RID: 1231
		internal const string CantConvertProcessToGlobal = "CantConvertProcessToGlobal";

		// Token: 0x040004D0 RID: 1232
		internal const string CantConvertGlobalToProcess = "CantConvertGlobalToProcess";

		// Token: 0x040004D1 RID: 1233
		internal const string PriorityClassNotSupported = "PriorityClassNotSupported";

		// Token: 0x040004D2 RID: 1234
		internal const string WinNTRequired = "WinNTRequired";

		// Token: 0x040004D3 RID: 1235
		internal const string Win2kRequired = "Win2kRequired";

		// Token: 0x040004D4 RID: 1236
		internal const string NoAssociatedProcess = "NoAssociatedProcess";

		// Token: 0x040004D5 RID: 1237
		internal const string ProcessIdRequired = "ProcessIdRequired";

		// Token: 0x040004D6 RID: 1238
		internal const string NotSupportedRemote = "NotSupportedRemote";

		// Token: 0x040004D7 RID: 1239
		internal const string NoProcessInfo = "NoProcessInfo";

		// Token: 0x040004D8 RID: 1240
		internal const string WaitTillExit = "WaitTillExit";

		// Token: 0x040004D9 RID: 1241
		internal const string NoProcessHandle = "NoProcessHandle";

		// Token: 0x040004DA RID: 1242
		internal const string MissingProccess = "MissingProccess";

		// Token: 0x040004DB RID: 1243
		internal const string BadMinWorkset = "BadMinWorkset";

		// Token: 0x040004DC RID: 1244
		internal const string BadMaxWorkset = "BadMaxWorkset";

		// Token: 0x040004DD RID: 1245
		internal const string WinNTRequiredForRemote = "WinNTRequiredForRemote";

		// Token: 0x040004DE RID: 1246
		internal const string ProcessHasExited = "ProcessHasExited";

		// Token: 0x040004DF RID: 1247
		internal const string ProcessHasExitedNoId = "ProcessHasExitedNoId";

		// Token: 0x040004E0 RID: 1248
		internal const string ThreadExited = "ThreadExited";

		// Token: 0x040004E1 RID: 1249
		internal const string Win2000Required = "Win2000Required";

		// Token: 0x040004E2 RID: 1250
		internal const string WinXPRequired = "WinXPRequired";

		// Token: 0x040004E3 RID: 1251
		internal const string Win2k3Required = "Win2k3Required";

		// Token: 0x040004E4 RID: 1252
		internal const string ProcessNotFound = "ProcessNotFound";

		// Token: 0x040004E5 RID: 1253
		internal const string CantGetProcessId = "CantGetProcessId";

		// Token: 0x040004E6 RID: 1254
		internal const string ProcessDisabled = "ProcessDisabled";

		// Token: 0x040004E7 RID: 1255
		internal const string WaitReasonUnavailable = "WaitReasonUnavailable";

		// Token: 0x040004E8 RID: 1256
		internal const string NotSupportedRemoteThread = "NotSupportedRemoteThread";

		// Token: 0x040004E9 RID: 1257
		internal const string UseShellExecuteRequiresSTA = "UseShellExecuteRequiresSTA";

		// Token: 0x040004EA RID: 1258
		internal const string CantRedirectStreams = "CantRedirectStreams";

		// Token: 0x040004EB RID: 1259
		internal const string CantUseEnvVars = "CantUseEnvVars";

		// Token: 0x040004EC RID: 1260
		internal const string CantStartAsUser = "CantStartAsUser";

		// Token: 0x040004ED RID: 1261
		internal const string CouldntConnectToRemoteMachine = "CouldntConnectToRemoteMachine";

		// Token: 0x040004EE RID: 1262
		internal const string CouldntGetProcessInfos = "CouldntGetProcessInfos";

		// Token: 0x040004EF RID: 1263
		internal const string InputIdleUnkownError = "InputIdleUnkownError";

		// Token: 0x040004F0 RID: 1264
		internal const string FileNameMissing = "FileNameMissing";

		// Token: 0x040004F1 RID: 1265
		internal const string EnvironmentBlock = "EnvironmentBlock";

		// Token: 0x040004F2 RID: 1266
		internal const string EnumProcessModuleFailed = "EnumProcessModuleFailed";

		// Token: 0x040004F3 RID: 1267
		internal const string PendingAsyncOperation = "PendingAsyncOperation";

		// Token: 0x040004F4 RID: 1268
		internal const string NoAsyncOperation = "NoAsyncOperation";

		// Token: 0x040004F5 RID: 1269
		internal const string InvalidApplication = "InvalidApplication";

		// Token: 0x040004F6 RID: 1270
		internal const string StandardOutputEncodingNotAllowed = "StandardOutputEncodingNotAllowed";

		// Token: 0x040004F7 RID: 1271
		internal const string StandardErrorEncodingNotAllowed = "StandardErrorEncodingNotAllowed";

		// Token: 0x040004F8 RID: 1272
		internal const string CountersOOM = "CountersOOM";

		// Token: 0x040004F9 RID: 1273
		internal const string MappingCorrupted = "MappingCorrupted";

		// Token: 0x040004FA RID: 1274
		internal const string SetSecurityDescriptorFailed = "SetSecurityDescriptorFailed";

		// Token: 0x040004FB RID: 1275
		internal const string CantCreateFileMapping = "CantCreateFileMapping";

		// Token: 0x040004FC RID: 1276
		internal const string CantMapFileView = "CantMapFileView";

		// Token: 0x040004FD RID: 1277
		internal const string CantGetMappingSize = "CantGetMappingSize";

		// Token: 0x040004FE RID: 1278
		internal const string CantGetStandardOut = "CantGetStandardOut";

		// Token: 0x040004FF RID: 1279
		internal const string CantGetStandardIn = "CantGetStandardIn";

		// Token: 0x04000500 RID: 1280
		internal const string CantGetStandardError = "CantGetStandardError";

		// Token: 0x04000501 RID: 1281
		internal const string CantMixSyncAsyncOperation = "CantMixSyncAsyncOperation";

		// Token: 0x04000502 RID: 1282
		internal const string NoFileMappingSize = "NoFileMappingSize";

		// Token: 0x04000503 RID: 1283
		internal const string EnvironmentBlockTooLong = "EnvironmentBlockTooLong";

		// Token: 0x04000504 RID: 1284
		internal const string Arg_InvalidSerialPort = "Arg_InvalidSerialPort";

		// Token: 0x04000505 RID: 1285
		internal const string Arg_InvalidSerialPortExtended = "Arg_InvalidSerialPortExtended";

		// Token: 0x04000506 RID: 1286
		internal const string Arg_SecurityException = "Arg_SecurityException";

		// Token: 0x04000507 RID: 1287
		internal const string Argument_InvalidOffLen = "Argument_InvalidOffLen";

		// Token: 0x04000508 RID: 1288
		internal const string ArgumentNull_Array = "ArgumentNull_Array";

		// Token: 0x04000509 RID: 1289
		internal const string ArgumentNull_Buffer = "ArgumentNull_Buffer";

		// Token: 0x0400050A RID: 1290
		internal const string ArgumentOutOfRange_Bounds_Lower_Upper = "ArgumentOutOfRange_Bounds_Lower_Upper";

		// Token: 0x0400050B RID: 1291
		internal const string ArgumentOutOfRange_Enum = "ArgumentOutOfRange_Enum";

		// Token: 0x0400050C RID: 1292
		internal const string ArgumentOutOfRange_NeedNonNegNumRequired = "ArgumentOutOfRange_NeedNonNegNumRequired";

		// Token: 0x0400050D RID: 1293
		internal const string ArgumentOutOfRange_Timeout = "ArgumentOutOfRange_Timeout";

		// Token: 0x0400050E RID: 1294
		internal const string ArgumentOutOfRange_WriteTimeout = "ArgumentOutOfRange_WriteTimeout";

		// Token: 0x0400050F RID: 1295
		internal const string ArgumentOutOfRange_NeedPosNum = "ArgumentOutOfRange_NeedPosNum";

		// Token: 0x04000510 RID: 1296
		internal const string ArgumentOutOfRange_OffsetOut = "ArgumentOutOfRange_OffsetOut";

		// Token: 0x04000511 RID: 1297
		internal const string IndexOutOfRange_IORaceCondition = "IndexOutOfRange_IORaceCondition";

		// Token: 0x04000512 RID: 1298
		internal const string IO_BindHandleFailed = "IO_BindHandleFailed";

		// Token: 0x04000513 RID: 1299
		internal const string IO_OperationAborted = "IO_OperationAborted";

		// Token: 0x04000514 RID: 1300
		internal const string NotSupported_UnseekableStream = "NotSupported_UnseekableStream";

		// Token: 0x04000515 RID: 1301
		internal const string UnauthorizedAccess_IODenied_Path = "UnauthorizedAccess_IODenied_Path";

		// Token: 0x04000516 RID: 1302
		internal const string IO_EOF_ReadBeyondEOF = "IO_EOF_ReadBeyondEOF";

		// Token: 0x04000517 RID: 1303
		internal const string IO_UnknownError = "IO_UnknownError";

		// Token: 0x04000518 RID: 1304
		internal const string ObjectDisposed_StreamClosed = "ObjectDisposed_StreamClosed";

		// Token: 0x04000519 RID: 1305
		internal const string Arg_WrongAsyncResult = "Arg_WrongAsyncResult";

		// Token: 0x0400051A RID: 1306
		internal const string InvalidOperation_EndReadCalledMultiple = "InvalidOperation_EndReadCalledMultiple";

		// Token: 0x0400051B RID: 1307
		internal const string InvalidOperation_EndWriteCalledMultiple = "InvalidOperation_EndWriteCalledMultiple";

		// Token: 0x0400051C RID: 1308
		internal const string IO_PortNotFound = "IO_PortNotFound";

		// Token: 0x0400051D RID: 1309
		internal const string IO_PortNotFoundFileName = "IO_PortNotFoundFileName";

		// Token: 0x0400051E RID: 1310
		internal const string UnauthorizedAccess_IODenied_NoPathName = "UnauthorizedAccess_IODenied_NoPathName";

		// Token: 0x0400051F RID: 1311
		internal const string IO_PathTooLong = "IO_PathTooLong";

		// Token: 0x04000520 RID: 1312
		internal const string IO_SharingViolation_NoFileName = "IO_SharingViolation_NoFileName";

		// Token: 0x04000521 RID: 1313
		internal const string IO_SharingViolation_File = "IO_SharingViolation_File";

		// Token: 0x04000522 RID: 1314
		internal const string NotSupported_UnwritableStream = "NotSupported_UnwritableStream";

		// Token: 0x04000523 RID: 1315
		internal const string ObjectDisposed_WriterClosed = "ObjectDisposed_WriterClosed";

		// Token: 0x04000524 RID: 1316
		internal const string BaseStream_Invalid_Not_Open = "BaseStream_Invalid_Not_Open";

		// Token: 0x04000525 RID: 1317
		internal const string PortNameEmpty_String = "PortNameEmpty_String";

		// Token: 0x04000526 RID: 1318
		internal const string Port_not_open = "Port_not_open";

		// Token: 0x04000527 RID: 1319
		internal const string Port_already_open = "Port_already_open";

		// Token: 0x04000528 RID: 1320
		internal const string Cant_be_set_when_open = "Cant_be_set_when_open";

		// Token: 0x04000529 RID: 1321
		internal const string Max_Baud = "Max_Baud";

		// Token: 0x0400052A RID: 1322
		internal const string In_Break_State = "In_Break_State";

		// Token: 0x0400052B RID: 1323
		internal const string Write_timed_out = "Write_timed_out";

		// Token: 0x0400052C RID: 1324
		internal const string CantSetRtsWithHandshaking = "CantSetRtsWithHandshaking";

		// Token: 0x0400052D RID: 1325
		internal const string NotSupportedOS = "NotSupportedOS";

		// Token: 0x0400052E RID: 1326
		internal const string NotSupportedEncoding = "NotSupportedEncoding";

		// Token: 0x0400052F RID: 1327
		internal const string BaudRate = "BaudRate";

		// Token: 0x04000530 RID: 1328
		internal const string DataBits = "DataBits";

		// Token: 0x04000531 RID: 1329
		internal const string DiscardNull = "DiscardNull";

		// Token: 0x04000532 RID: 1330
		internal const string DtrEnable = "DtrEnable";

		// Token: 0x04000533 RID: 1331
		internal const string Encoding = "Encoding";

		// Token: 0x04000534 RID: 1332
		internal const string Handshake = "Handshake";

		// Token: 0x04000535 RID: 1333
		internal const string NewLine = "NewLine";

		// Token: 0x04000536 RID: 1334
		internal const string Parity = "Parity";

		// Token: 0x04000537 RID: 1335
		internal const string ParityReplace = "ParityReplace";

		// Token: 0x04000538 RID: 1336
		internal const string PortName = "PortName";

		// Token: 0x04000539 RID: 1337
		internal const string ReadBufferSize = "ReadBufferSize";

		// Token: 0x0400053A RID: 1338
		internal const string ReadTimeout = "ReadTimeout";

		// Token: 0x0400053B RID: 1339
		internal const string ReceivedBytesThreshold = "ReceivedBytesThreshold";

		// Token: 0x0400053C RID: 1340
		internal const string RtsEnable = "RtsEnable";

		// Token: 0x0400053D RID: 1341
		internal const string SerialPortDesc = "SerialPortDesc";

		// Token: 0x0400053E RID: 1342
		internal const string StopBits = "StopBits";

		// Token: 0x0400053F RID: 1343
		internal const string WriteBufferSize = "WriteBufferSize";

		// Token: 0x04000540 RID: 1344
		internal const string WriteTimeout = "WriteTimeout";

		// Token: 0x04000541 RID: 1345
		internal const string SerialErrorReceived = "SerialErrorReceived";

		// Token: 0x04000542 RID: 1346
		internal const string SerialPinChanged = "SerialPinChanged";

		// Token: 0x04000543 RID: 1347
		internal const string SerialDataReceived = "SerialDataReceived";

		// Token: 0x04000544 RID: 1348
		internal const string CounterType = "CounterType";

		// Token: 0x04000545 RID: 1349
		internal const string CounterName = "CounterName";

		// Token: 0x04000546 RID: 1350
		internal const string CounterHelp = "CounterHelp";

		// Token: 0x04000547 RID: 1351
		internal const string EventLogDesc = "EventLogDesc";

		// Token: 0x04000548 RID: 1352
		internal const string ErrorDataReceived = "ErrorDataReceived";

		// Token: 0x04000549 RID: 1353
		internal const string LogEntries = "LogEntries";

		// Token: 0x0400054A RID: 1354
		internal const string LogLog = "LogLog";

		// Token: 0x0400054B RID: 1355
		internal const string LogMachineName = "LogMachineName";

		// Token: 0x0400054C RID: 1356
		internal const string LogMonitoring = "LogMonitoring";

		// Token: 0x0400054D RID: 1357
		internal const string LogSynchronizingObject = "LogSynchronizingObject";

		// Token: 0x0400054E RID: 1358
		internal const string LogSource = "LogSource";

		// Token: 0x0400054F RID: 1359
		internal const string LogEntryWritten = "LogEntryWritten";

		// Token: 0x04000550 RID: 1360
		internal const string LogEntryMachineName = "LogEntryMachineName";

		// Token: 0x04000551 RID: 1361
		internal const string LogEntryData = "LogEntryData";

		// Token: 0x04000552 RID: 1362
		internal const string LogEntryIndex = "LogEntryIndex";

		// Token: 0x04000553 RID: 1363
		internal const string LogEntryCategory = "LogEntryCategory";

		// Token: 0x04000554 RID: 1364
		internal const string LogEntryCategoryNumber = "LogEntryCategoryNumber";

		// Token: 0x04000555 RID: 1365
		internal const string LogEntryEventID = "LogEntryEventID";

		// Token: 0x04000556 RID: 1366
		internal const string LogEntryEntryType = "LogEntryEntryType";

		// Token: 0x04000557 RID: 1367
		internal const string LogEntryMessage = "LogEntryMessage";

		// Token: 0x04000558 RID: 1368
		internal const string LogEntrySource = "LogEntrySource";

		// Token: 0x04000559 RID: 1369
		internal const string LogEntryReplacementStrings = "LogEntryReplacementStrings";

		// Token: 0x0400055A RID: 1370
		internal const string LogEntryResourceId = "LogEntryResourceId";

		// Token: 0x0400055B RID: 1371
		internal const string LogEntryTimeGenerated = "LogEntryTimeGenerated";

		// Token: 0x0400055C RID: 1372
		internal const string LogEntryTimeWritten = "LogEntryTimeWritten";

		// Token: 0x0400055D RID: 1373
		internal const string LogEntryUserName = "LogEntryUserName";

		// Token: 0x0400055E RID: 1374
		internal const string OutputDataReceived = "OutputDataReceived";

		// Token: 0x0400055F RID: 1375
		internal const string PC_CounterHelp = "PC_CounterHelp";

		// Token: 0x04000560 RID: 1376
		internal const string PC_CounterType = "PC_CounterType";

		// Token: 0x04000561 RID: 1377
		internal const string PC_ReadOnly = "PC_ReadOnly";

		// Token: 0x04000562 RID: 1378
		internal const string PC_RawValue = "PC_RawValue";

		// Token: 0x04000563 RID: 1379
		internal const string ProcessAssociated = "ProcessAssociated";

		// Token: 0x04000564 RID: 1380
		internal const string ProcessDesc = "ProcessDesc";

		// Token: 0x04000565 RID: 1381
		internal const string ProcessExitCode = "ProcessExitCode";

		// Token: 0x04000566 RID: 1382
		internal const string ProcessTerminated = "ProcessTerminated";

		// Token: 0x04000567 RID: 1383
		internal const string ProcessExitTime = "ProcessExitTime";

		// Token: 0x04000568 RID: 1384
		internal const string ProcessHandle = "ProcessHandle";

		// Token: 0x04000569 RID: 1385
		internal const string ProcessHandleCount = "ProcessHandleCount";

		// Token: 0x0400056A RID: 1386
		internal const string ProcessId = "ProcessId";

		// Token: 0x0400056B RID: 1387
		internal const string ProcessMachineName = "ProcessMachineName";

		// Token: 0x0400056C RID: 1388
		internal const string ProcessMainModule = "ProcessMainModule";

		// Token: 0x0400056D RID: 1389
		internal const string ProcessModules = "ProcessModules";

		// Token: 0x0400056E RID: 1390
		internal const string ProcessSynchronizingObject = "ProcessSynchronizingObject";

		// Token: 0x0400056F RID: 1391
		internal const string ProcessSessionId = "ProcessSessionId";

		// Token: 0x04000570 RID: 1392
		internal const string ProcessThreads = "ProcessThreads";

		// Token: 0x04000571 RID: 1393
		internal const string ProcessEnableRaisingEvents = "ProcessEnableRaisingEvents";

		// Token: 0x04000572 RID: 1394
		internal const string ProcessExited = "ProcessExited";

		// Token: 0x04000573 RID: 1395
		internal const string ProcessFileName = "ProcessFileName";

		// Token: 0x04000574 RID: 1396
		internal const string ProcessWorkingDirectory = "ProcessWorkingDirectory";

		// Token: 0x04000575 RID: 1397
		internal const string ProcessBasePriority = "ProcessBasePriority";

		// Token: 0x04000576 RID: 1398
		internal const string ProcessMainWindowHandle = "ProcessMainWindowHandle";

		// Token: 0x04000577 RID: 1399
		internal const string ProcessMainWindowTitle = "ProcessMainWindowTitle";

		// Token: 0x04000578 RID: 1400
		internal const string ProcessMaxWorkingSet = "ProcessMaxWorkingSet";

		// Token: 0x04000579 RID: 1401
		internal const string ProcessMinWorkingSet = "ProcessMinWorkingSet";

		// Token: 0x0400057A RID: 1402
		internal const string ProcessNonpagedSystemMemorySize = "ProcessNonpagedSystemMemorySize";

		// Token: 0x0400057B RID: 1403
		internal const string ProcessPagedMemorySize = "ProcessPagedMemorySize";

		// Token: 0x0400057C RID: 1404
		internal const string ProcessPagedSystemMemorySize = "ProcessPagedSystemMemorySize";

		// Token: 0x0400057D RID: 1405
		internal const string ProcessPeakPagedMemorySize = "ProcessPeakPagedMemorySize";

		// Token: 0x0400057E RID: 1406
		internal const string ProcessPeakWorkingSet = "ProcessPeakWorkingSet";

		// Token: 0x0400057F RID: 1407
		internal const string ProcessPeakVirtualMemorySize = "ProcessPeakVirtualMemorySize";

		// Token: 0x04000580 RID: 1408
		internal const string ProcessPriorityBoostEnabled = "ProcessPriorityBoostEnabled";

		// Token: 0x04000581 RID: 1409
		internal const string ProcessPriorityClass = "ProcessPriorityClass";

		// Token: 0x04000582 RID: 1410
		internal const string ProcessPrivateMemorySize = "ProcessPrivateMemorySize";

		// Token: 0x04000583 RID: 1411
		internal const string ProcessPrivilegedProcessorTime = "ProcessPrivilegedProcessorTime";

		// Token: 0x04000584 RID: 1412
		internal const string ProcessProcessName = "ProcessProcessName";

		// Token: 0x04000585 RID: 1413
		internal const string ProcessProcessorAffinity = "ProcessProcessorAffinity";

		// Token: 0x04000586 RID: 1414
		internal const string ProcessResponding = "ProcessResponding";

		// Token: 0x04000587 RID: 1415
		internal const string ProcessStandardError = "ProcessStandardError";

		// Token: 0x04000588 RID: 1416
		internal const string ProcessStandardInput = "ProcessStandardInput";

		// Token: 0x04000589 RID: 1417
		internal const string ProcessStandardOutput = "ProcessStandardOutput";

		// Token: 0x0400058A RID: 1418
		internal const string ProcessStartInfo = "ProcessStartInfo";

		// Token: 0x0400058B RID: 1419
		internal const string ProcessStartTime = "ProcessStartTime";

		// Token: 0x0400058C RID: 1420
		internal const string ProcessTotalProcessorTime = "ProcessTotalProcessorTime";

		// Token: 0x0400058D RID: 1421
		internal const string ProcessUserProcessorTime = "ProcessUserProcessorTime";

		// Token: 0x0400058E RID: 1422
		internal const string ProcessVirtualMemorySize = "ProcessVirtualMemorySize";

		// Token: 0x0400058F RID: 1423
		internal const string ProcessWorkingSet = "ProcessWorkingSet";

		// Token: 0x04000590 RID: 1424
		internal const string ProcModModuleName = "ProcModModuleName";

		// Token: 0x04000591 RID: 1425
		internal const string ProcModFileName = "ProcModFileName";

		// Token: 0x04000592 RID: 1426
		internal const string ProcModBaseAddress = "ProcModBaseAddress";

		// Token: 0x04000593 RID: 1427
		internal const string ProcModModuleMemorySize = "ProcModModuleMemorySize";

		// Token: 0x04000594 RID: 1428
		internal const string ProcModEntryPointAddress = "ProcModEntryPointAddress";

		// Token: 0x04000595 RID: 1429
		internal const string ProcessVerb = "ProcessVerb";

		// Token: 0x04000596 RID: 1430
		internal const string ProcessArguments = "ProcessArguments";

		// Token: 0x04000597 RID: 1431
		internal const string ProcessErrorDialog = "ProcessErrorDialog";

		// Token: 0x04000598 RID: 1432
		internal const string ProcessWindowStyle = "ProcessWindowStyle";

		// Token: 0x04000599 RID: 1433
		internal const string ProcessCreateNoWindow = "ProcessCreateNoWindow";

		// Token: 0x0400059A RID: 1434
		internal const string ProcessEnvironmentVariables = "ProcessEnvironmentVariables";

		// Token: 0x0400059B RID: 1435
		internal const string ProcessRedirectStandardInput = "ProcessRedirectStandardInput";

		// Token: 0x0400059C RID: 1436
		internal const string ProcessRedirectStandardOutput = "ProcessRedirectStandardOutput";

		// Token: 0x0400059D RID: 1437
		internal const string ProcessRedirectStandardError = "ProcessRedirectStandardError";

		// Token: 0x0400059E RID: 1438
		internal const string ProcessUseShellExecute = "ProcessUseShellExecute";

		// Token: 0x0400059F RID: 1439
		internal const string ThreadBasePriority = "ThreadBasePriority";

		// Token: 0x040005A0 RID: 1440
		internal const string ThreadCurrentPriority = "ThreadCurrentPriority";

		// Token: 0x040005A1 RID: 1441
		internal const string ThreadId = "ThreadId";

		// Token: 0x040005A2 RID: 1442
		internal const string ThreadPriorityBoostEnabled = "ThreadPriorityBoostEnabled";

		// Token: 0x040005A3 RID: 1443
		internal const string ThreadPriorityLevel = "ThreadPriorityLevel";

		// Token: 0x040005A4 RID: 1444
		internal const string ThreadPrivilegedProcessorTime = "ThreadPrivilegedProcessorTime";

		// Token: 0x040005A5 RID: 1445
		internal const string ThreadStartAddress = "ThreadStartAddress";

		// Token: 0x040005A6 RID: 1446
		internal const string ThreadStartTime = "ThreadStartTime";

		// Token: 0x040005A7 RID: 1447
		internal const string ThreadThreadState = "ThreadThreadState";

		// Token: 0x040005A8 RID: 1448
		internal const string ThreadTotalProcessorTime = "ThreadTotalProcessorTime";

		// Token: 0x040005A9 RID: 1449
		internal const string ThreadUserProcessorTime = "ThreadUserProcessorTime";

		// Token: 0x040005AA RID: 1450
		internal const string ThreadWaitReason = "ThreadWaitReason";

		// Token: 0x040005AB RID: 1451
		internal const string VerbEditorDefault = "VerbEditorDefault";

		// Token: 0x040005AC RID: 1452
		internal const string AppSettingsReaderNoKey = "AppSettingsReaderNoKey";

		// Token: 0x040005AD RID: 1453
		internal const string AppSettingsReaderNoParser = "AppSettingsReaderNoParser";

		// Token: 0x040005AE RID: 1454
		internal const string AppSettingsReaderCantParse = "AppSettingsReaderCantParse";

		// Token: 0x040005AF RID: 1455
		internal const string AppSettingsReaderEmptyString = "AppSettingsReaderEmptyString";

		// Token: 0x040005B0 RID: 1456
		internal const string InvalidPermissionState = "InvalidPermissionState";

		// Token: 0x040005B1 RID: 1457
		internal const string PermissionNumberOfElements = "PermissionNumberOfElements";

		// Token: 0x040005B2 RID: 1458
		internal const string PermissionItemExists = "PermissionItemExists";

		// Token: 0x040005B3 RID: 1459
		internal const string PermissionItemDoesntExist = "PermissionItemDoesntExist";

		// Token: 0x040005B4 RID: 1460
		internal const string PermissionBadParameterEnum = "PermissionBadParameterEnum";

		// Token: 0x040005B5 RID: 1461
		internal const string PermissionInvalidLength = "PermissionInvalidLength";

		// Token: 0x040005B6 RID: 1462
		internal const string PermissionTypeMismatch = "PermissionTypeMismatch";

		// Token: 0x040005B7 RID: 1463
		internal const string Argument_NotAPermissionElement = "Argument_NotAPermissionElement";

		// Token: 0x040005B8 RID: 1464
		internal const string Argument_InvalidXMLBadVersion = "Argument_InvalidXMLBadVersion";

		// Token: 0x040005B9 RID: 1465
		internal const string InvalidPermissionLevel = "InvalidPermissionLevel";

		// Token: 0x040005BA RID: 1466
		internal const string TargetNotWebBrowserPermissionLevel = "TargetNotWebBrowserPermissionLevel";

		// Token: 0x040005BB RID: 1467
		internal const string WebBrowserBadXml = "WebBrowserBadXml";

		// Token: 0x040005BC RID: 1468
		internal const string KeyedCollNeedNonNegativeNum = "KeyedCollNeedNonNegativeNum";

		// Token: 0x040005BD RID: 1469
		internal const string KeyedCollDuplicateKey = "KeyedCollDuplicateKey";

		// Token: 0x040005BE RID: 1470
		internal const string KeyedCollReferenceKeyNotFound = "KeyedCollReferenceKeyNotFound";

		// Token: 0x040005BF RID: 1471
		internal const string KeyedCollKeyNotFound = "KeyedCollKeyNotFound";

		// Token: 0x040005C0 RID: 1472
		internal const string KeyedCollInvalidKey = "KeyedCollInvalidKey";

		// Token: 0x040005C1 RID: 1473
		internal const string KeyedCollCapacityOverflow = "KeyedCollCapacityOverflow";

		// Token: 0x040005C2 RID: 1474
		internal const string InvalidOperation_EnumEnded = "InvalidOperation_EnumEnded";

		// Token: 0x040005C3 RID: 1475
		internal const string OrderedDictionary_ReadOnly = "OrderedDictionary_ReadOnly";

		// Token: 0x040005C4 RID: 1476
		internal const string OrderedDictionary_SerializationMismatch = "OrderedDictionary_SerializationMismatch";

		// Token: 0x040005C5 RID: 1477
		internal const string Async_ExceptionOccurred = "Async_ExceptionOccurred";

		// Token: 0x040005C6 RID: 1478
		internal const string Async_QueueingFailed = "Async_QueueingFailed";

		// Token: 0x040005C7 RID: 1479
		internal const string Async_OperationCancelled = "Async_OperationCancelled";

		// Token: 0x040005C8 RID: 1480
		internal const string Async_OperationAlreadyCompleted = "Async_OperationAlreadyCompleted";

		// Token: 0x040005C9 RID: 1481
		internal const string Async_NullDelegate = "Async_NullDelegate";

		// Token: 0x040005CA RID: 1482
		internal const string BackgroundWorker_WorkerAlreadyRunning = "BackgroundWorker_WorkerAlreadyRunning";

		// Token: 0x040005CB RID: 1483
		internal const string BackgroundWorker_WorkerDoesntReportProgress = "BackgroundWorker_WorkerDoesntReportProgress";

		// Token: 0x040005CC RID: 1484
		internal const string BackgroundWorker_WorkerDoesntSupportCancellation = "BackgroundWorker_WorkerDoesntSupportCancellation";

		// Token: 0x040005CD RID: 1485
		internal const string Async_ProgressChangedEventArgs_ProgressPercentage = "Async_ProgressChangedEventArgs_ProgressPercentage";

		// Token: 0x040005CE RID: 1486
		internal const string Async_ProgressChangedEventArgs_UserState = "Async_ProgressChangedEventArgs_UserState";

		// Token: 0x040005CF RID: 1487
		internal const string Async_AsyncEventArgs_Cancelled = "Async_AsyncEventArgs_Cancelled";

		// Token: 0x040005D0 RID: 1488
		internal const string Async_AsyncEventArgs_Error = "Async_AsyncEventArgs_Error";

		// Token: 0x040005D1 RID: 1489
		internal const string Async_AsyncEventArgs_UserState = "Async_AsyncEventArgs_UserState";

		// Token: 0x040005D2 RID: 1490
		internal const string BackgroundWorker_CancellationPending = "BackgroundWorker_CancellationPending";

		// Token: 0x040005D3 RID: 1491
		internal const string BackgroundWorker_DoWork = "BackgroundWorker_DoWork";

		// Token: 0x040005D4 RID: 1492
		internal const string BackgroundWorker_IsBusy = "BackgroundWorker_IsBusy";

		// Token: 0x040005D5 RID: 1493
		internal const string BackgroundWorker_ProgressChanged = "BackgroundWorker_ProgressChanged";

		// Token: 0x040005D6 RID: 1494
		internal const string BackgroundWorker_RunWorkerCompleted = "BackgroundWorker_RunWorkerCompleted";

		// Token: 0x040005D7 RID: 1495
		internal const string BackgroundWorker_WorkerReportsProgress = "BackgroundWorker_WorkerReportsProgress";

		// Token: 0x040005D8 RID: 1496
		internal const string BackgroundWorker_WorkerSupportsCancellation = "BackgroundWorker_WorkerSupportsCancellation";

		// Token: 0x040005D9 RID: 1497
		internal const string BackgroundWorker_DoWorkEventArgs_Argument = "BackgroundWorker_DoWorkEventArgs_Argument";

		// Token: 0x040005DA RID: 1498
		internal const string BackgroundWorker_DoWorkEventArgs_Result = "BackgroundWorker_DoWorkEventArgs_Result";

		// Token: 0x040005DB RID: 1499
		internal const string BackgroundWorker_Desc = "BackgroundWorker_Desc";

		// Token: 0x040005DC RID: 1500
		internal const string InstanceCreationEditorDefaultText = "InstanceCreationEditorDefaultText";

		// Token: 0x040005DD RID: 1501
		internal const string PropertyTabAttributeBadPropertyTabScope = "PropertyTabAttributeBadPropertyTabScope";

		// Token: 0x040005DE RID: 1502
		internal const string PropertyTabAttributeTypeLoadException = "PropertyTabAttributeTypeLoadException";

		// Token: 0x040005DF RID: 1503
		internal const string PropertyTabAttributeArrayLengthMismatch = "PropertyTabAttributeArrayLengthMismatch";

		// Token: 0x040005E0 RID: 1504
		internal const string PropertyTabAttributeParamsBothNull = "PropertyTabAttributeParamsBothNull";

		// Token: 0x040005E1 RID: 1505
		internal const string InstanceDescriptorCannotBeStatic = "InstanceDescriptorCannotBeStatic";

		// Token: 0x040005E2 RID: 1506
		internal const string InstanceDescriptorMustBeStatic = "InstanceDescriptorMustBeStatic";

		// Token: 0x040005E3 RID: 1507
		internal const string InstanceDescriptorMustBeReadable = "InstanceDescriptorMustBeReadable";

		// Token: 0x040005E4 RID: 1508
		internal const string InstanceDescriptorLengthMismatch = "InstanceDescriptorLengthMismatch";

		// Token: 0x040005E5 RID: 1509
		internal const string ToolboxItemAttributeFailedGetType = "ToolboxItemAttributeFailedGetType";

		// Token: 0x040005E6 RID: 1510
		internal const string PropertyDescriptorCollectionBadValue = "PropertyDescriptorCollectionBadValue";

		// Token: 0x040005E7 RID: 1511
		internal const string PropertyDescriptorCollectionBadKey = "PropertyDescriptorCollectionBadKey";

		// Token: 0x040005E8 RID: 1512
		internal const string AspNetHostingPermissionBadXml = "AspNetHostingPermissionBadXml";

		// Token: 0x040005E9 RID: 1513
		internal const string CorruptedGZipHeader = "CorruptedGZipHeader";

		// Token: 0x040005EA RID: 1514
		internal const string UnknownCompressionMode = "UnknownCompressionMode";

		// Token: 0x040005EB RID: 1515
		internal const string UnknownState = "UnknownState";

		// Token: 0x040005EC RID: 1516
		internal const string InvalidHuffmanData = "InvalidHuffmanData";

		// Token: 0x040005ED RID: 1517
		internal const string InvalidCRC = "InvalidCRC";

		// Token: 0x040005EE RID: 1518
		internal const string InvalidStreamSize = "InvalidStreamSize";

		// Token: 0x040005EF RID: 1519
		internal const string UnknownBlockType = "UnknownBlockType";

		// Token: 0x040005F0 RID: 1520
		internal const string InvalidBlockLength = "InvalidBlockLength";

		// Token: 0x040005F1 RID: 1521
		internal const string GenericInvalidData = "GenericInvalidData";

		// Token: 0x040005F2 RID: 1522
		internal const string CannotReadFromDeflateStream = "CannotReadFromDeflateStream";

		// Token: 0x040005F3 RID: 1523
		internal const string CannotWriteToDeflateStream = "CannotWriteToDeflateStream";

		// Token: 0x040005F4 RID: 1524
		internal const string NotReadableStream = "NotReadableStream";

		// Token: 0x040005F5 RID: 1525
		internal const string NotWriteableStream = "NotWriteableStream";

		// Token: 0x040005F6 RID: 1526
		internal const string InvalidArgumentOffsetCount = "InvalidArgumentOffsetCount";

		// Token: 0x040005F7 RID: 1527
		internal const string InvalidBeginCall = "InvalidBeginCall";

		// Token: 0x040005F8 RID: 1528
		internal const string InvalidEndCall = "InvalidEndCall";

		// Token: 0x040005F9 RID: 1529
		internal const string StreamSizeOverflow = "StreamSizeOverflow";

		// Token: 0x040005FA RID: 1530
		internal const string InvalidOperation_HCCountOverflow = "InvalidOperation_HCCountOverflow";

		// Token: 0x040005FB RID: 1531
		internal const string Argument_InvalidThreshold = "Argument_InvalidThreshold";

		// Token: 0x040005FC RID: 1532
		internal const string Argument_SemaphoreInitialMaximum = "Argument_SemaphoreInitialMaximum";

		// Token: 0x040005FD RID: 1533
		internal const string Argument_WaitHandleNameTooLong = "Argument_WaitHandleNameTooLong";

		// Token: 0x040005FE RID: 1534
		internal const string Threading_SemaphoreFullException = "Threading_SemaphoreFullException";

		// Token: 0x040005FF RID: 1535
		internal const string WaitHandleCannotBeOpenedException_InvalidHandle = "WaitHandleCannotBeOpenedException_InvalidHandle";

		// Token: 0x04000600 RID: 1536
		internal const string ArgumentNotAPermissionElement = "ArgumentNotAPermissionElement";

		// Token: 0x04000601 RID: 1537
		internal const string ArgumentWrongType = "ArgumentWrongType";

		// Token: 0x04000602 RID: 1538
		internal const string BadXmlVersion = "BadXmlVersion";

		// Token: 0x04000603 RID: 1539
		internal const string BinarySerializationNotSupported = "BinarySerializationNotSupported";

		// Token: 0x04000604 RID: 1540
		internal const string BothScopeAttributes = "BothScopeAttributes";

		// Token: 0x04000605 RID: 1541
		internal const string NoScopeAttributes = "NoScopeAttributes";

		// Token: 0x04000606 RID: 1542
		internal const string PositionOutOfRange = "PositionOutOfRange";

		// Token: 0x04000607 RID: 1543
		internal const string ProviderInstantiationFailed = "ProviderInstantiationFailed";

		// Token: 0x04000608 RID: 1544
		internal const string ProviderTypeLoadFailed = "ProviderTypeLoadFailed";

		// Token: 0x04000609 RID: 1545
		internal const string SaveAppScopedNotSupported = "SaveAppScopedNotSupported";

		// Token: 0x0400060A RID: 1546
		internal const string SettingsResetFailed = "SettingsResetFailed";

		// Token: 0x0400060B RID: 1547
		internal const string SettingsSaveFailed = "SettingsSaveFailed";

		// Token: 0x0400060C RID: 1548
		internal const string SettingsSaveFailedNoSection = "SettingsSaveFailedNoSection";

		// Token: 0x0400060D RID: 1549
		internal const string StringDeserializationFailed = "StringDeserializationFailed";

		// Token: 0x0400060E RID: 1550
		internal const string StringSerializationFailed = "StringSerializationFailed";

		// Token: 0x0400060F RID: 1551
		internal const string UnknownSerializationFormat = "UnknownSerializationFormat";

		// Token: 0x04000610 RID: 1552
		internal const string UnknownSeekOrigin = "UnknownSeekOrigin";

		// Token: 0x04000611 RID: 1553
		internal const string UnknownUserLevel = "UnknownUserLevel";

		// Token: 0x04000612 RID: 1554
		internal const string UserSettingsNotSupported = "UserSettingsNotSupported";

		// Token: 0x04000613 RID: 1555
		internal const string XmlDeserializationFailed = "XmlDeserializationFailed";

		// Token: 0x04000614 RID: 1556
		internal const string XmlSerializationFailed = "XmlSerializationFailed";

		// Token: 0x04000615 RID: 1557
		internal const string MemberRelationshipService_RelationshipNotSupported = "MemberRelationshipService_RelationshipNotSupported";

		// Token: 0x04000616 RID: 1558
		internal const string MaskedTextProviderPasswordAndPromptCharError = "MaskedTextProviderPasswordAndPromptCharError";

		// Token: 0x04000617 RID: 1559
		internal const string MaskedTextProviderInvalidCharError = "MaskedTextProviderInvalidCharError";

		// Token: 0x04000618 RID: 1560
		internal const string MaskedTextProviderMaskNullOrEmpty = "MaskedTextProviderMaskNullOrEmpty";

		// Token: 0x04000619 RID: 1561
		internal const string MaskedTextProviderMaskInvalidChar = "MaskedTextProviderMaskInvalidChar";

		// Token: 0x0400061A RID: 1562
		internal const string StandardOleMarshalObjectGetMarshalerFailed = "StandardOleMarshalObjectGetMarshalerFailed";

		// Token: 0x0400061B RID: 1563
		internal const string SoundAPIBadSoundLocation = "SoundAPIBadSoundLocation";

		// Token: 0x0400061C RID: 1564
		internal const string SoundAPIFileDoesNotExist = "SoundAPIFileDoesNotExist";

		// Token: 0x0400061D RID: 1565
		internal const string SoundAPIFormatNotSupported = "SoundAPIFormatNotSupported";

		// Token: 0x0400061E RID: 1566
		internal const string SoundAPIInvalidWaveFile = "SoundAPIInvalidWaveFile";

		// Token: 0x0400061F RID: 1567
		internal const string SoundAPIInvalidWaveHeader = "SoundAPIInvalidWaveHeader";

		// Token: 0x04000620 RID: 1568
		internal const string SoundAPILoadTimedOut = "SoundAPILoadTimedOut";

		// Token: 0x04000621 RID: 1569
		internal const string SoundAPILoadTimeout = "SoundAPILoadTimeout";

		// Token: 0x04000622 RID: 1570
		internal const string SoundAPIReadError = "SoundAPIReadError";

		// Token: 0x04000623 RID: 1571
		private static SR loader;

		// Token: 0x04000624 RID: 1572
		private ResourceManager resources;

		// Token: 0x04000625 RID: 1573
		private static object s_InternalSyncObject;
	}
}
