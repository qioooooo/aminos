using System;

namespace System.Reflection
{
	// Token: 0x02000306 RID: 774
	[Serializable]
	internal enum MetadataTokenType
	{
		// Token: 0x04000B97 RID: 2967
		Module,
		// Token: 0x04000B98 RID: 2968
		TypeRef = 16777216,
		// Token: 0x04000B99 RID: 2969
		TypeDef = 33554432,
		// Token: 0x04000B9A RID: 2970
		FieldDef = 67108864,
		// Token: 0x04000B9B RID: 2971
		MethodDef = 100663296,
		// Token: 0x04000B9C RID: 2972
		ParamDef = 134217728,
		// Token: 0x04000B9D RID: 2973
		InterfaceImpl = 150994944,
		// Token: 0x04000B9E RID: 2974
		MemberRef = 167772160,
		// Token: 0x04000B9F RID: 2975
		CustomAttribute = 201326592,
		// Token: 0x04000BA0 RID: 2976
		Permission = 234881024,
		// Token: 0x04000BA1 RID: 2977
		Signature = 285212672,
		// Token: 0x04000BA2 RID: 2978
		Event = 335544320,
		// Token: 0x04000BA3 RID: 2979
		Property = 385875968,
		// Token: 0x04000BA4 RID: 2980
		ModuleRef = 436207616,
		// Token: 0x04000BA5 RID: 2981
		TypeSpec = 452984832,
		// Token: 0x04000BA6 RID: 2982
		Assembly = 536870912,
		// Token: 0x04000BA7 RID: 2983
		AssemblyRef = 587202560,
		// Token: 0x04000BA8 RID: 2984
		File = 637534208,
		// Token: 0x04000BA9 RID: 2985
		ExportedType = 654311424,
		// Token: 0x04000BAA RID: 2986
		ManifestResource = 671088640,
		// Token: 0x04000BAB RID: 2987
		GenericPar = 704643072,
		// Token: 0x04000BAC RID: 2988
		MethodSpec = 721420288,
		// Token: 0x04000BAD RID: 2989
		String = 1879048192,
		// Token: 0x04000BAE RID: 2990
		Name = 1895825408,
		// Token: 0x04000BAF RID: 2991
		BaseType = 1912602624,
		// Token: 0x04000BB0 RID: 2992
		Invalid = 2147483647
	}
}
