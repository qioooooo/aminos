using System;

namespace System.CodeDom.Compiler
{
	// Token: 0x020001F6 RID: 502
	[Flags]
	[Serializable]
	public enum GeneratorSupport
	{
		// Token: 0x04000F86 RID: 3974
		ArraysOfArrays = 1,
		// Token: 0x04000F87 RID: 3975
		EntryPointMethod = 2,
		// Token: 0x04000F88 RID: 3976
		GotoStatements = 4,
		// Token: 0x04000F89 RID: 3977
		MultidimensionalArrays = 8,
		// Token: 0x04000F8A RID: 3978
		StaticConstructors = 16,
		// Token: 0x04000F8B RID: 3979
		TryCatchStatements = 32,
		// Token: 0x04000F8C RID: 3980
		ReturnTypeAttributes = 64,
		// Token: 0x04000F8D RID: 3981
		DeclareValueTypes = 128,
		// Token: 0x04000F8E RID: 3982
		DeclareEnums = 256,
		// Token: 0x04000F8F RID: 3983
		DeclareDelegates = 512,
		// Token: 0x04000F90 RID: 3984
		DeclareInterfaces = 1024,
		// Token: 0x04000F91 RID: 3985
		DeclareEvents = 2048,
		// Token: 0x04000F92 RID: 3986
		AssemblyAttributes = 4096,
		// Token: 0x04000F93 RID: 3987
		ParameterAttributes = 8192,
		// Token: 0x04000F94 RID: 3988
		ReferenceParameters = 16384,
		// Token: 0x04000F95 RID: 3989
		ChainedConstructorArguments = 32768,
		// Token: 0x04000F96 RID: 3990
		NestedTypes = 65536,
		// Token: 0x04000F97 RID: 3991
		MultipleInterfaceMembers = 131072,
		// Token: 0x04000F98 RID: 3992
		PublicStaticMembers = 262144,
		// Token: 0x04000F99 RID: 3993
		ComplexExpressions = 524288,
		// Token: 0x04000F9A RID: 3994
		Win32Resources = 1048576,
		// Token: 0x04000F9B RID: 3995
		Resources = 2097152,
		// Token: 0x04000F9C RID: 3996
		PartialTypes = 4194304,
		// Token: 0x04000F9D RID: 3997
		GenericTypeReference = 8388608,
		// Token: 0x04000F9E RID: 3998
		GenericTypeDeclaration = 16777216,
		// Token: 0x04000F9F RID: 3999
		DeclareIndexerProperties = 33554432
	}
}
