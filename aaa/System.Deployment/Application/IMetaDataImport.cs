using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Application
{
	// Token: 0x0200007E RID: 126
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7DAC8207-D3AE-4c75-9B67-92801A497D44")]
	[ComImport]
	internal interface IMetaDataImport
	{
		// Token: 0x060003B1 RID: 945
		[PreserveSig]
		void CloseEnum();

		// Token: 0x060003B2 RID: 946
		void CountEnum(IntPtr iRef, ref uint ulCount);

		// Token: 0x060003B3 RID: 947
		void ResetEnum();

		// Token: 0x060003B4 RID: 948
		void EnumTypeDefs();

		// Token: 0x060003B5 RID: 949
		void EnumInterfaceImpls();

		// Token: 0x060003B6 RID: 950
		void EnumTypeRefs();

		// Token: 0x060003B7 RID: 951
		void FindTypeDefByName();

		// Token: 0x060003B8 RID: 952
		void GetScopeProps();

		// Token: 0x060003B9 RID: 953
		void GetModuleFromScope();

		// Token: 0x060003BA RID: 954
		void GetTypeDefProps();

		// Token: 0x060003BB RID: 955
		void GetInterfaceImplProps();

		// Token: 0x060003BC RID: 956
		void GetTypeRefProps();

		// Token: 0x060003BD RID: 957
		void ResolveTypeRef();

		// Token: 0x060003BE RID: 958
		void EnumMembers();

		// Token: 0x060003BF RID: 959
		void EnumMembersWithName();

		// Token: 0x060003C0 RID: 960
		void EnumMethods();

		// Token: 0x060003C1 RID: 961
		void EnumMethodsWithName();

		// Token: 0x060003C2 RID: 962
		void EnumFields();

		// Token: 0x060003C3 RID: 963
		void EnumFieldsWithName();

		// Token: 0x060003C4 RID: 964
		void EnumParams();

		// Token: 0x060003C5 RID: 965
		void EnumMemberRefs();

		// Token: 0x060003C6 RID: 966
		void EnumMethodImpls();

		// Token: 0x060003C7 RID: 967
		void EnumPermissionSets();

		// Token: 0x060003C8 RID: 968
		void FindMember();

		// Token: 0x060003C9 RID: 969
		void FindMethod();

		// Token: 0x060003CA RID: 970
		void FindField();

		// Token: 0x060003CB RID: 971
		void FindMemberRef();

		// Token: 0x060003CC RID: 972
		void GetMethodProps();

		// Token: 0x060003CD RID: 973
		void GetMemberRefProps();

		// Token: 0x060003CE RID: 974
		void EnumProperties();

		// Token: 0x060003CF RID: 975
		void EnumEvents();

		// Token: 0x060003D0 RID: 976
		void GetEventProps();

		// Token: 0x060003D1 RID: 977
		void EnumMethodSemantics();

		// Token: 0x060003D2 RID: 978
		void GetMethodSemantics();

		// Token: 0x060003D3 RID: 979
		void GetClassLayout();

		// Token: 0x060003D4 RID: 980
		void GetFieldMarshal();

		// Token: 0x060003D5 RID: 981
		void GetRVA();

		// Token: 0x060003D6 RID: 982
		void GetPermissionSetProps();

		// Token: 0x060003D7 RID: 983
		void GetSigFromToken();

		// Token: 0x060003D8 RID: 984
		void GetModuleRefProps();

		// Token: 0x060003D9 RID: 985
		void EnumModuleRefs();

		// Token: 0x060003DA RID: 986
		void GetTypeSpecFromToken();

		// Token: 0x060003DB RID: 987
		void GetNameFromToken();

		// Token: 0x060003DC RID: 988
		void EnumUnresolvedMethods();

		// Token: 0x060003DD RID: 989
		void GetUserString();

		// Token: 0x060003DE RID: 990
		void GetPinvokeMap();

		// Token: 0x060003DF RID: 991
		void EnumSignatures();

		// Token: 0x060003E0 RID: 992
		void EnumTypeSpecs();

		// Token: 0x060003E1 RID: 993
		void EnumUserStrings();

		// Token: 0x060003E2 RID: 994
		void GetParamForMethodIndex();

		// Token: 0x060003E3 RID: 995
		void EnumCustomAttributes();

		// Token: 0x060003E4 RID: 996
		void GetCustomAttributeProps();

		// Token: 0x060003E5 RID: 997
		void FindTypeRef();

		// Token: 0x060003E6 RID: 998
		void GetMemberProps();

		// Token: 0x060003E7 RID: 999
		void GetFieldProps();

		// Token: 0x060003E8 RID: 1000
		void GetPropertyProps();

		// Token: 0x060003E9 RID: 1001
		void GetParamProps();

		// Token: 0x060003EA RID: 1002
		void GetCustomAttributeByName();

		// Token: 0x060003EB RID: 1003
		void IsValidToken();

		// Token: 0x060003EC RID: 1004
		void GetNestedClassProps();

		// Token: 0x060003ED RID: 1005
		void GetNativeCallConvFromSig();

		// Token: 0x060003EE RID: 1006
		void IsGlobal();
	}
}
