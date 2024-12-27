using System;

namespace System.Reflection
{
	// Token: 0x02000309 RID: 777
	[Serializable]
	internal enum MetadataColumnType
	{
		// Token: 0x04000BEF RID: 3055
		Module,
		// Token: 0x04000BF0 RID: 3056
		TypeRef,
		// Token: 0x04000BF1 RID: 3057
		TypeDef,
		// Token: 0x04000BF2 RID: 3058
		FieldPtr,
		// Token: 0x04000BF3 RID: 3059
		Field,
		// Token: 0x04000BF4 RID: 3060
		MethodPtr,
		// Token: 0x04000BF5 RID: 3061
		Method,
		// Token: 0x04000BF6 RID: 3062
		ParamPtr,
		// Token: 0x04000BF7 RID: 3063
		Param,
		// Token: 0x04000BF8 RID: 3064
		InterfaceImpl,
		// Token: 0x04000BF9 RID: 3065
		MemberRef,
		// Token: 0x04000BFA RID: 3066
		Constant,
		// Token: 0x04000BFB RID: 3067
		CustomAttribute,
		// Token: 0x04000BFC RID: 3068
		FieldMarshal,
		// Token: 0x04000BFD RID: 3069
		DeclSecurity,
		// Token: 0x04000BFE RID: 3070
		ClassLayout,
		// Token: 0x04000BFF RID: 3071
		FieldLayout,
		// Token: 0x04000C00 RID: 3072
		StandAloneSig,
		// Token: 0x04000C01 RID: 3073
		EventMap,
		// Token: 0x04000C02 RID: 3074
		EventPtr,
		// Token: 0x04000C03 RID: 3075
		Event,
		// Token: 0x04000C04 RID: 3076
		PropertyMap,
		// Token: 0x04000C05 RID: 3077
		PropertyPtr,
		// Token: 0x04000C06 RID: 3078
		Property,
		// Token: 0x04000C07 RID: 3079
		MethodSemantics,
		// Token: 0x04000C08 RID: 3080
		MethodImpl,
		// Token: 0x04000C09 RID: 3081
		ModuleRef,
		// Token: 0x04000C0A RID: 3082
		TypeSpec,
		// Token: 0x04000C0B RID: 3083
		ImplMap,
		// Token: 0x04000C0C RID: 3084
		FieldRVA,
		// Token: 0x04000C0D RID: 3085
		ENCLog,
		// Token: 0x04000C0E RID: 3086
		ENCMap,
		// Token: 0x04000C0F RID: 3087
		Assembly,
		// Token: 0x04000C10 RID: 3088
		AssemblyProcessor,
		// Token: 0x04000C11 RID: 3089
		AssemblyOS,
		// Token: 0x04000C12 RID: 3090
		AssemblyRef,
		// Token: 0x04000C13 RID: 3091
		AssemblyRefProcessor,
		// Token: 0x04000C14 RID: 3092
		AssemblyRefOS,
		// Token: 0x04000C15 RID: 3093
		File,
		// Token: 0x04000C16 RID: 3094
		ExportedType,
		// Token: 0x04000C17 RID: 3095
		ManifestResource,
		// Token: 0x04000C18 RID: 3096
		NestedClass,
		// Token: 0x04000C19 RID: 3097
		GenericParam,
		// Token: 0x04000C1A RID: 3098
		MethodSpec,
		// Token: 0x04000C1B RID: 3099
		GenericParamConstraint,
		// Token: 0x04000C1C RID: 3100
		TableIdMax = 63,
		// Token: 0x04000C1D RID: 3101
		CodedToken,
		// Token: 0x04000C1E RID: 3102
		TypeDefOrRef,
		// Token: 0x04000C1F RID: 3103
		HasConstant,
		// Token: 0x04000C20 RID: 3104
		HasCustomAttribute,
		// Token: 0x04000C21 RID: 3105
		HasFieldMarshal,
		// Token: 0x04000C22 RID: 3106
		HasDeclSecurity,
		// Token: 0x04000C23 RID: 3107
		MemberRefParent,
		// Token: 0x04000C24 RID: 3108
		HasSemantic,
		// Token: 0x04000C25 RID: 3109
		MethodDefOrRef,
		// Token: 0x04000C26 RID: 3110
		MemberForwarded,
		// Token: 0x04000C27 RID: 3111
		Implementation,
		// Token: 0x04000C28 RID: 3112
		CustomAttributeType,
		// Token: 0x04000C29 RID: 3113
		ResolutionScope,
		// Token: 0x04000C2A RID: 3114
		TypeOrMethodDef,
		// Token: 0x04000C2B RID: 3115
		CodedTokenMax = 95,
		// Token: 0x04000C2C RID: 3116
		Short,
		// Token: 0x04000C2D RID: 3117
		UShort,
		// Token: 0x04000C2E RID: 3118
		Long,
		// Token: 0x04000C2F RID: 3119
		ULong,
		// Token: 0x04000C30 RID: 3120
		Byte,
		// Token: 0x04000C31 RID: 3121
		StringHeap,
		// Token: 0x04000C32 RID: 3122
		GuidHeap,
		// Token: 0x04000C33 RID: 3123
		BlobHeap
	}
}
