using System;

namespace System.Reflection
{
	// Token: 0x0200030A RID: 778
	[Serializable]
	internal enum MetadataColumn
	{
		// Token: 0x04000C35 RID: 3125
		ModuleGeneration,
		// Token: 0x04000C36 RID: 3126
		ModuleName,
		// Token: 0x04000C37 RID: 3127
		ModuleMvid,
		// Token: 0x04000C38 RID: 3128
		ModuleEncId,
		// Token: 0x04000C39 RID: 3129
		ModuleEncBaseId,
		// Token: 0x04000C3A RID: 3130
		TypeRefResolutionScope = 0,
		// Token: 0x04000C3B RID: 3131
		TypeRefName,
		// Token: 0x04000C3C RID: 3132
		TypeRefNamespace,
		// Token: 0x04000C3D RID: 3133
		TypeDefFlags = 0,
		// Token: 0x04000C3E RID: 3134
		TypeDefName,
		// Token: 0x04000C3F RID: 3135
		TypeDefNamespace,
		// Token: 0x04000C40 RID: 3136
		TypeDefExtends,
		// Token: 0x04000C41 RID: 3137
		TypeDefFieldList,
		// Token: 0x04000C42 RID: 3138
		TypeDefMethodList,
		// Token: 0x04000C43 RID: 3139
		FieldPtrField = 0,
		// Token: 0x04000C44 RID: 3140
		FieldFlags = 0,
		// Token: 0x04000C45 RID: 3141
		FieldName,
		// Token: 0x04000C46 RID: 3142
		FieldSignature,
		// Token: 0x04000C47 RID: 3143
		MethodPtrMethod = 0,
		// Token: 0x04000C48 RID: 3144
		MethodRVA = 0,
		// Token: 0x04000C49 RID: 3145
		MethodImplFlags,
		// Token: 0x04000C4A RID: 3146
		MethodFlags,
		// Token: 0x04000C4B RID: 3147
		MethodName,
		// Token: 0x04000C4C RID: 3148
		MethodSignature,
		// Token: 0x04000C4D RID: 3149
		MethodParamList,
		// Token: 0x04000C4E RID: 3150
		ParamPtrParam = 0,
		// Token: 0x04000C4F RID: 3151
		ParamFlags = 0,
		// Token: 0x04000C50 RID: 3152
		ParamSequence,
		// Token: 0x04000C51 RID: 3153
		ParamName,
		// Token: 0x04000C52 RID: 3154
		InterfaceImplClass = 0,
		// Token: 0x04000C53 RID: 3155
		InterfaceImplInterface,
		// Token: 0x04000C54 RID: 3156
		MemberRefClass = 0,
		// Token: 0x04000C55 RID: 3157
		MemberRefName,
		// Token: 0x04000C56 RID: 3158
		MemberRefSignature,
		// Token: 0x04000C57 RID: 3159
		ConstantType = 0,
		// Token: 0x04000C58 RID: 3160
		ConstantParent,
		// Token: 0x04000C59 RID: 3161
		ConstantValue,
		// Token: 0x04000C5A RID: 3162
		CustomAttributeParent = 0,
		// Token: 0x04000C5B RID: 3163
		CustomAttributeType,
		// Token: 0x04000C5C RID: 3164
		CustomAttributeArgument,
		// Token: 0x04000C5D RID: 3165
		FieldMarshalParent = 0,
		// Token: 0x04000C5E RID: 3166
		FieldMarshalNativeType,
		// Token: 0x04000C5F RID: 3167
		DeclSecurityAction = 0,
		// Token: 0x04000C60 RID: 3168
		DeclSecurityParent,
		// Token: 0x04000C61 RID: 3169
		DeclSecurityPermissionSet,
		// Token: 0x04000C62 RID: 3170
		ClassLayoutPackingSize = 0,
		// Token: 0x04000C63 RID: 3171
		ClassLayoutClassSize,
		// Token: 0x04000C64 RID: 3172
		ClassLayoutParent,
		// Token: 0x04000C65 RID: 3173
		FieldLayoutOffSet = 0,
		// Token: 0x04000C66 RID: 3174
		FieldLayoutField,
		// Token: 0x04000C67 RID: 3175
		StandAloneSigSignature = 0,
		// Token: 0x04000C68 RID: 3176
		EventMapParent = 0,
		// Token: 0x04000C69 RID: 3177
		EventMapEventList,
		// Token: 0x04000C6A RID: 3178
		EventPtrEvent = 0,
		// Token: 0x04000C6B RID: 3179
		EventEventFlags = 0,
		// Token: 0x04000C6C RID: 3180
		EventName,
		// Token: 0x04000C6D RID: 3181
		EventEventType,
		// Token: 0x04000C6E RID: 3182
		PropertyMapParent = 0,
		// Token: 0x04000C6F RID: 3183
		PropertyMapPropertyList,
		// Token: 0x04000C70 RID: 3184
		PropertyPtrProperty = 0,
		// Token: 0x04000C71 RID: 3185
		PropertyPropFlags = 0,
		// Token: 0x04000C72 RID: 3186
		PropertyName,
		// Token: 0x04000C73 RID: 3187
		PropertyType,
		// Token: 0x04000C74 RID: 3188
		MethodSemanticsSemantic = 0,
		// Token: 0x04000C75 RID: 3189
		MethodSemanticsMethod,
		// Token: 0x04000C76 RID: 3190
		MethodSemanticsAssociation,
		// Token: 0x04000C77 RID: 3191
		MethodImplClass = 0,
		// Token: 0x04000C78 RID: 3192
		MethodImplMethodBody,
		// Token: 0x04000C79 RID: 3193
		MethodImplMethodDeclaration,
		// Token: 0x04000C7A RID: 3194
		ModuleRefName = 0,
		// Token: 0x04000C7B RID: 3195
		TypeSpecSignature = 0,
		// Token: 0x04000C7C RID: 3196
		ImplMapMappingFlags = 0,
		// Token: 0x04000C7D RID: 3197
		ImplMapMemberForwarded,
		// Token: 0x04000C7E RID: 3198
		ImplMapImportName,
		// Token: 0x04000C7F RID: 3199
		ImplMapImportScope,
		// Token: 0x04000C80 RID: 3200
		FieldRVARVA = 0,
		// Token: 0x04000C81 RID: 3201
		FieldRVAField,
		// Token: 0x04000C82 RID: 3202
		ENCLogToken = 0,
		// Token: 0x04000C83 RID: 3203
		ENCLogFuncCode,
		// Token: 0x04000C84 RID: 3204
		ENCMapToken = 0,
		// Token: 0x04000C85 RID: 3205
		AssemblyHashAlgId = 0,
		// Token: 0x04000C86 RID: 3206
		AssemblyMajorVersion,
		// Token: 0x04000C87 RID: 3207
		AssemblyMinorVersion,
		// Token: 0x04000C88 RID: 3208
		AssemblyBuildNumber,
		// Token: 0x04000C89 RID: 3209
		AssemblyRevisionNumber,
		// Token: 0x04000C8A RID: 3210
		AssemblyFlags,
		// Token: 0x04000C8B RID: 3211
		AssemblyPublicKey,
		// Token: 0x04000C8C RID: 3212
		AssemblyName,
		// Token: 0x04000C8D RID: 3213
		AssemblyLocale,
		// Token: 0x04000C8E RID: 3214
		AssemblyProcessorProcessor = 0,
		// Token: 0x04000C8F RID: 3215
		AssemblyOSOSPlatformId = 0,
		// Token: 0x04000C90 RID: 3216
		AssemblyOSOSMajorVersion,
		// Token: 0x04000C91 RID: 3217
		AssemblyOSOSMinorVersion,
		// Token: 0x04000C92 RID: 3218
		AssemblyRefMajorVersion = 0,
		// Token: 0x04000C93 RID: 3219
		AssemblyRefMinorVersion,
		// Token: 0x04000C94 RID: 3220
		AssemblyRefBuildNumber,
		// Token: 0x04000C95 RID: 3221
		AssemblyRefRevisionNumber,
		// Token: 0x04000C96 RID: 3222
		AssemblyRefFlags,
		// Token: 0x04000C97 RID: 3223
		AssemblyRefPublicKeyOrToken,
		// Token: 0x04000C98 RID: 3224
		AssemblyRefName,
		// Token: 0x04000C99 RID: 3225
		AssemblyRefLocale,
		// Token: 0x04000C9A RID: 3226
		AssemblyRefHashValue,
		// Token: 0x04000C9B RID: 3227
		AssemblyRefProcessorProcessor = 0,
		// Token: 0x04000C9C RID: 3228
		AssemblyRefProcessorAssemblyRef,
		// Token: 0x04000C9D RID: 3229
		AssemblyRefOSOSPlatformId = 0,
		// Token: 0x04000C9E RID: 3230
		AssemblyRefOSOSMajorVersion,
		// Token: 0x04000C9F RID: 3231
		AssemblyRefOSOSMinorVersion,
		// Token: 0x04000CA0 RID: 3232
		AssemblyRefOSAssemblyRef,
		// Token: 0x04000CA1 RID: 3233
		FileFlags = 0,
		// Token: 0x04000CA2 RID: 3234
		FileName,
		// Token: 0x04000CA3 RID: 3235
		FileHashValue,
		// Token: 0x04000CA4 RID: 3236
		ExportedTypeFlags = 0,
		// Token: 0x04000CA5 RID: 3237
		ExportedTypeTypeDefId,
		// Token: 0x04000CA6 RID: 3238
		ExportedTypeTypeName,
		// Token: 0x04000CA7 RID: 3239
		ExportedTypeTypeNamespace,
		// Token: 0x04000CA8 RID: 3240
		ExportedTypeImplementation,
		// Token: 0x04000CA9 RID: 3241
		ManifestResourceOffset = 0,
		// Token: 0x04000CAA RID: 3242
		ManifestResourceFlags,
		// Token: 0x04000CAB RID: 3243
		ManifestResourceName,
		// Token: 0x04000CAC RID: 3244
		ManifestResourceImplementation,
		// Token: 0x04000CAD RID: 3245
		NestedClassNestedClass = 0,
		// Token: 0x04000CAE RID: 3246
		NestedClassEnclosingClass,
		// Token: 0x04000CAF RID: 3247
		GenericParamNumber = 0,
		// Token: 0x04000CB0 RID: 3248
		GenericParamFlags,
		// Token: 0x04000CB1 RID: 3249
		GenericParamOwner,
		// Token: 0x04000CB2 RID: 3250
		GenericParamName,
		// Token: 0x04000CB3 RID: 3251
		GenericParamKind,
		// Token: 0x04000CB4 RID: 3252
		GenericParamDeprecatedConstraint,
		// Token: 0x04000CB5 RID: 3253
		MethodSpecMethod = 0,
		// Token: 0x04000CB6 RID: 3254
		MethodSpecInstantiation,
		// Token: 0x04000CB7 RID: 3255
		GenericParamConstraintOwner = 0,
		// Token: 0x04000CB8 RID: 3256
		GenericParamConstraintConstraint
	}
}
