using System;

namespace System.Management
{
	// Token: 0x0200001B RID: 27
	public enum ManagementStatus
	{
		// Token: 0x0400009B RID: 155
		NoError,
		// Token: 0x0400009C RID: 156
		False,
		// Token: 0x0400009D RID: 157
		ResetToDefault = 262146,
		// Token: 0x0400009E RID: 158
		Different,
		// Token: 0x0400009F RID: 159
		Timedout,
		// Token: 0x040000A0 RID: 160
		NoMoreData,
		// Token: 0x040000A1 RID: 161
		OperationCanceled,
		// Token: 0x040000A2 RID: 162
		Pending,
		// Token: 0x040000A3 RID: 163
		DuplicateObjects,
		// Token: 0x040000A4 RID: 164
		PartialResults = 262160,
		// Token: 0x040000A5 RID: 165
		Failed = -2147217407,
		// Token: 0x040000A6 RID: 166
		NotFound,
		// Token: 0x040000A7 RID: 167
		AccessDenied,
		// Token: 0x040000A8 RID: 168
		ProviderFailure,
		// Token: 0x040000A9 RID: 169
		TypeMismatch,
		// Token: 0x040000AA RID: 170
		OutOfMemory,
		// Token: 0x040000AB RID: 171
		InvalidContext,
		// Token: 0x040000AC RID: 172
		InvalidParameter,
		// Token: 0x040000AD RID: 173
		NotAvailable,
		// Token: 0x040000AE RID: 174
		CriticalError,
		// Token: 0x040000AF RID: 175
		InvalidStream,
		// Token: 0x040000B0 RID: 176
		NotSupported,
		// Token: 0x040000B1 RID: 177
		InvalidSuperclass,
		// Token: 0x040000B2 RID: 178
		InvalidNamespace,
		// Token: 0x040000B3 RID: 179
		InvalidObject,
		// Token: 0x040000B4 RID: 180
		InvalidClass,
		// Token: 0x040000B5 RID: 181
		ProviderNotFound,
		// Token: 0x040000B6 RID: 182
		InvalidProviderRegistration,
		// Token: 0x040000B7 RID: 183
		ProviderLoadFailure,
		// Token: 0x040000B8 RID: 184
		InitializationFailure,
		// Token: 0x040000B9 RID: 185
		TransportFailure,
		// Token: 0x040000BA RID: 186
		InvalidOperation,
		// Token: 0x040000BB RID: 187
		InvalidQuery,
		// Token: 0x040000BC RID: 188
		InvalidQueryType,
		// Token: 0x040000BD RID: 189
		AlreadyExists,
		// Token: 0x040000BE RID: 190
		OverrideNotAllowed,
		// Token: 0x040000BF RID: 191
		PropagatedQualifier,
		// Token: 0x040000C0 RID: 192
		PropagatedProperty,
		// Token: 0x040000C1 RID: 193
		Unexpected,
		// Token: 0x040000C2 RID: 194
		IllegalOperation,
		// Token: 0x040000C3 RID: 195
		CannotBeKey,
		// Token: 0x040000C4 RID: 196
		IncompleteClass,
		// Token: 0x040000C5 RID: 197
		InvalidSyntax,
		// Token: 0x040000C6 RID: 198
		NondecoratedObject,
		// Token: 0x040000C7 RID: 199
		ReadOnly,
		// Token: 0x040000C8 RID: 200
		ProviderNotCapable,
		// Token: 0x040000C9 RID: 201
		ClassHasChildren,
		// Token: 0x040000CA RID: 202
		ClassHasInstances,
		// Token: 0x040000CB RID: 203
		QueryNotImplemented,
		// Token: 0x040000CC RID: 204
		IllegalNull,
		// Token: 0x040000CD RID: 205
		InvalidQualifierType,
		// Token: 0x040000CE RID: 206
		InvalidPropertyType,
		// Token: 0x040000CF RID: 207
		ValueOutOfRange,
		// Token: 0x040000D0 RID: 208
		CannotBeSingleton,
		// Token: 0x040000D1 RID: 209
		InvalidCimType,
		// Token: 0x040000D2 RID: 210
		InvalidMethod,
		// Token: 0x040000D3 RID: 211
		InvalidMethodParameters,
		// Token: 0x040000D4 RID: 212
		SystemProperty,
		// Token: 0x040000D5 RID: 213
		InvalidProperty,
		// Token: 0x040000D6 RID: 214
		CallCanceled,
		// Token: 0x040000D7 RID: 215
		ShuttingDown,
		// Token: 0x040000D8 RID: 216
		PropagatedMethod,
		// Token: 0x040000D9 RID: 217
		UnsupportedParameter,
		// Token: 0x040000DA RID: 218
		MissingParameterID,
		// Token: 0x040000DB RID: 219
		InvalidParameterID,
		// Token: 0x040000DC RID: 220
		NonconsecutiveParameterIDs,
		// Token: 0x040000DD RID: 221
		ParameterIDOnRetval,
		// Token: 0x040000DE RID: 222
		InvalidObjectPath,
		// Token: 0x040000DF RID: 223
		OutOfDiskSpace,
		// Token: 0x040000E0 RID: 224
		BufferTooSmall,
		// Token: 0x040000E1 RID: 225
		UnsupportedPutExtension,
		// Token: 0x040000E2 RID: 226
		UnknownObjectType,
		// Token: 0x040000E3 RID: 227
		UnknownPacketType,
		// Token: 0x040000E4 RID: 228
		MarshalVersionMismatch,
		// Token: 0x040000E5 RID: 229
		MarshalInvalidSignature,
		// Token: 0x040000E6 RID: 230
		InvalidQualifier,
		// Token: 0x040000E7 RID: 231
		InvalidDuplicateParameter,
		// Token: 0x040000E8 RID: 232
		TooMuchData,
		// Token: 0x040000E9 RID: 233
		ServerTooBusy,
		// Token: 0x040000EA RID: 234
		InvalidFlavor,
		// Token: 0x040000EB RID: 235
		CircularReference,
		// Token: 0x040000EC RID: 236
		UnsupportedClassUpdate,
		// Token: 0x040000ED RID: 237
		CannotChangeKeyInheritance,
		// Token: 0x040000EE RID: 238
		CannotChangeIndexInheritance = -2147217328,
		// Token: 0x040000EF RID: 239
		TooManyProperties,
		// Token: 0x040000F0 RID: 240
		UpdateTypeMismatch,
		// Token: 0x040000F1 RID: 241
		UpdateOverrideNotAllowed,
		// Token: 0x040000F2 RID: 242
		UpdatePropagatedMethod,
		// Token: 0x040000F3 RID: 243
		MethodNotImplemented,
		// Token: 0x040000F4 RID: 244
		MethodDisabled,
		// Token: 0x040000F5 RID: 245
		RefresherBusy,
		// Token: 0x040000F6 RID: 246
		UnparsableQuery,
		// Token: 0x040000F7 RID: 247
		NotEventClass,
		// Token: 0x040000F8 RID: 248
		MissingGroupWithin,
		// Token: 0x040000F9 RID: 249
		MissingAggregationList,
		// Token: 0x040000FA RID: 250
		PropertyNotAnObject,
		// Token: 0x040000FB RID: 251
		AggregatingByObject,
		// Token: 0x040000FC RID: 252
		UninterpretableProviderQuery = -2147217313,
		// Token: 0x040000FD RID: 253
		BackupRestoreWinmgmtRunning,
		// Token: 0x040000FE RID: 254
		QueueOverflow,
		// Token: 0x040000FF RID: 255
		PrivilegeNotHeld,
		// Token: 0x04000100 RID: 256
		InvalidOperator,
		// Token: 0x04000101 RID: 257
		LocalCredentials,
		// Token: 0x04000102 RID: 258
		CannotBeAbstract,
		// Token: 0x04000103 RID: 259
		AmendedObject,
		// Token: 0x04000104 RID: 260
		ClientTooSlow,
		// Token: 0x04000105 RID: 261
		RegistrationTooBroad = -2147213311,
		// Token: 0x04000106 RID: 262
		RegistrationTooPrecise
	}
}
