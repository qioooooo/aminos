using System;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x02000057 RID: 87
	internal enum QilNodeType
	{
		// Token: 0x0400039D RID: 925
		QilExpression,
		// Token: 0x0400039E RID: 926
		FunctionList,
		// Token: 0x0400039F RID: 927
		GlobalVariableList,
		// Token: 0x040003A0 RID: 928
		GlobalParameterList,
		// Token: 0x040003A1 RID: 929
		ActualParameterList,
		// Token: 0x040003A2 RID: 930
		FormalParameterList,
		// Token: 0x040003A3 RID: 931
		SortKeyList,
		// Token: 0x040003A4 RID: 932
		BranchList,
		// Token: 0x040003A5 RID: 933
		OptimizeBarrier,
		// Token: 0x040003A6 RID: 934
		Unknown,
		// Token: 0x040003A7 RID: 935
		DataSource,
		// Token: 0x040003A8 RID: 936
		Nop,
		// Token: 0x040003A9 RID: 937
		Error,
		// Token: 0x040003AA RID: 938
		Warning,
		// Token: 0x040003AB RID: 939
		For,
		// Token: 0x040003AC RID: 940
		Let,
		// Token: 0x040003AD RID: 941
		Parameter,
		// Token: 0x040003AE RID: 942
		PositionOf,
		// Token: 0x040003AF RID: 943
		True,
		// Token: 0x040003B0 RID: 944
		False,
		// Token: 0x040003B1 RID: 945
		LiteralString,
		// Token: 0x040003B2 RID: 946
		LiteralInt32,
		// Token: 0x040003B3 RID: 947
		LiteralInt64,
		// Token: 0x040003B4 RID: 948
		LiteralDouble,
		// Token: 0x040003B5 RID: 949
		LiteralDecimal,
		// Token: 0x040003B6 RID: 950
		LiteralQName,
		// Token: 0x040003B7 RID: 951
		LiteralType,
		// Token: 0x040003B8 RID: 952
		LiteralObject,
		// Token: 0x040003B9 RID: 953
		And,
		// Token: 0x040003BA RID: 954
		Or,
		// Token: 0x040003BB RID: 955
		Not,
		// Token: 0x040003BC RID: 956
		Conditional,
		// Token: 0x040003BD RID: 957
		Choice,
		// Token: 0x040003BE RID: 958
		Length,
		// Token: 0x040003BF RID: 959
		Sequence,
		// Token: 0x040003C0 RID: 960
		Union,
		// Token: 0x040003C1 RID: 961
		Intersection,
		// Token: 0x040003C2 RID: 962
		Difference,
		// Token: 0x040003C3 RID: 963
		Average,
		// Token: 0x040003C4 RID: 964
		Sum,
		// Token: 0x040003C5 RID: 965
		Minimum,
		// Token: 0x040003C6 RID: 966
		Maximum,
		// Token: 0x040003C7 RID: 967
		Negate,
		// Token: 0x040003C8 RID: 968
		Add,
		// Token: 0x040003C9 RID: 969
		Subtract,
		// Token: 0x040003CA RID: 970
		Multiply,
		// Token: 0x040003CB RID: 971
		Divide,
		// Token: 0x040003CC RID: 972
		Modulo,
		// Token: 0x040003CD RID: 973
		StrLength,
		// Token: 0x040003CE RID: 974
		StrConcat,
		// Token: 0x040003CF RID: 975
		StrParseQName,
		// Token: 0x040003D0 RID: 976
		Ne,
		// Token: 0x040003D1 RID: 977
		Eq,
		// Token: 0x040003D2 RID: 978
		Gt,
		// Token: 0x040003D3 RID: 979
		Ge,
		// Token: 0x040003D4 RID: 980
		Lt,
		// Token: 0x040003D5 RID: 981
		Le,
		// Token: 0x040003D6 RID: 982
		Is,
		// Token: 0x040003D7 RID: 983
		After,
		// Token: 0x040003D8 RID: 984
		Before,
		// Token: 0x040003D9 RID: 985
		Loop,
		// Token: 0x040003DA RID: 986
		Filter,
		// Token: 0x040003DB RID: 987
		Sort,
		// Token: 0x040003DC RID: 988
		SortKey,
		// Token: 0x040003DD RID: 989
		DocOrderDistinct,
		// Token: 0x040003DE RID: 990
		Function,
		// Token: 0x040003DF RID: 991
		Invoke,
		// Token: 0x040003E0 RID: 992
		Content,
		// Token: 0x040003E1 RID: 993
		Attribute,
		// Token: 0x040003E2 RID: 994
		Parent,
		// Token: 0x040003E3 RID: 995
		Root,
		// Token: 0x040003E4 RID: 996
		XmlContext,
		// Token: 0x040003E5 RID: 997
		Descendant,
		// Token: 0x040003E6 RID: 998
		DescendantOrSelf,
		// Token: 0x040003E7 RID: 999
		Ancestor,
		// Token: 0x040003E8 RID: 1000
		AncestorOrSelf,
		// Token: 0x040003E9 RID: 1001
		Preceding,
		// Token: 0x040003EA RID: 1002
		FollowingSibling,
		// Token: 0x040003EB RID: 1003
		PrecedingSibling,
		// Token: 0x040003EC RID: 1004
		NodeRange,
		// Token: 0x040003ED RID: 1005
		Deref,
		// Token: 0x040003EE RID: 1006
		ElementCtor,
		// Token: 0x040003EF RID: 1007
		AttributeCtor,
		// Token: 0x040003F0 RID: 1008
		CommentCtor,
		// Token: 0x040003F1 RID: 1009
		PICtor,
		// Token: 0x040003F2 RID: 1010
		TextCtor,
		// Token: 0x040003F3 RID: 1011
		RawTextCtor,
		// Token: 0x040003F4 RID: 1012
		DocumentCtor,
		// Token: 0x040003F5 RID: 1013
		NamespaceDecl,
		// Token: 0x040003F6 RID: 1014
		RtfCtor,
		// Token: 0x040003F7 RID: 1015
		NameOf,
		// Token: 0x040003F8 RID: 1016
		LocalNameOf,
		// Token: 0x040003F9 RID: 1017
		NamespaceUriOf,
		// Token: 0x040003FA RID: 1018
		PrefixOf,
		// Token: 0x040003FB RID: 1019
		TypeAssert,
		// Token: 0x040003FC RID: 1020
		IsType,
		// Token: 0x040003FD RID: 1021
		IsEmpty,
		// Token: 0x040003FE RID: 1022
		XPathNodeValue,
		// Token: 0x040003FF RID: 1023
		XPathFollowing,
		// Token: 0x04000400 RID: 1024
		XPathPreceding,
		// Token: 0x04000401 RID: 1025
		XPathNamespace,
		// Token: 0x04000402 RID: 1026
		XsltGenerateId,
		// Token: 0x04000403 RID: 1027
		XsltInvokeLateBound,
		// Token: 0x04000404 RID: 1028
		XsltInvokeEarlyBound,
		// Token: 0x04000405 RID: 1029
		XsltCopy,
		// Token: 0x04000406 RID: 1030
		XsltCopyOf,
		// Token: 0x04000407 RID: 1031
		XsltConvert
	}
}
