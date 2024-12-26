using System;
using System.Runtime.InteropServices;

namespace Microsoft.JScript
{
	// Token: 0x020000A6 RID: 166
	[ComVisible(true)]
	[Guid("268CA962-2FEF-3152-BA46-E18658B7FA4F")]
	public enum JSError
	{
		// Token: 0x04000333 RID: 819
		NoError,
		// Token: 0x04000334 RID: 820
		InvalidCall = 5,
		// Token: 0x04000335 RID: 821
		OutOfMemory = 7,
		// Token: 0x04000336 RID: 822
		TypeMismatch = 13,
		// Token: 0x04000337 RID: 823
		OutOfStack = 28,
		// Token: 0x04000338 RID: 824
		InternalError = 51,
		// Token: 0x04000339 RID: 825
		FileNotFound = 53,
		// Token: 0x0400033A RID: 826
		NeedObject = 424,
		// Token: 0x0400033B RID: 827
		CantCreateObject = 429,
		// Token: 0x0400033C RID: 828
		OLENoPropOrMethod = 438,
		// Token: 0x0400033D RID: 829
		ActionNotSupported = 445,
		// Token: 0x0400033E RID: 830
		NotCollection = 451,
		// Token: 0x0400033F RID: 831
		SyntaxError = 1002,
		// Token: 0x04000340 RID: 832
		NoColon,
		// Token: 0x04000341 RID: 833
		NoSemicolon,
		// Token: 0x04000342 RID: 834
		NoLeftParen,
		// Token: 0x04000343 RID: 835
		NoRightParen,
		// Token: 0x04000344 RID: 836
		NoRightBracket,
		// Token: 0x04000345 RID: 837
		NoLeftCurly,
		// Token: 0x04000346 RID: 838
		NoRightCurly,
		// Token: 0x04000347 RID: 839
		NoIdentifier,
		// Token: 0x04000348 RID: 840
		NoEqual,
		// Token: 0x04000349 RID: 841
		IllegalChar = 1014,
		// Token: 0x0400034A RID: 842
		UnterminatedString,
		// Token: 0x0400034B RID: 843
		NoCommentEnd,
		// Token: 0x0400034C RID: 844
		BadReturn = 1018,
		// Token: 0x0400034D RID: 845
		BadBreak,
		// Token: 0x0400034E RID: 846
		BadContinue,
		// Token: 0x0400034F RID: 847
		BadHexDigit = 1023,
		// Token: 0x04000350 RID: 848
		NoWhile,
		// Token: 0x04000351 RID: 849
		BadLabel,
		// Token: 0x04000352 RID: 850
		NoLabel,
		// Token: 0x04000353 RID: 851
		DupDefault,
		// Token: 0x04000354 RID: 852
		NoMemberIdentifier,
		// Token: 0x04000355 RID: 853
		NoCcEnd,
		// Token: 0x04000356 RID: 854
		CcOff,
		// Token: 0x04000357 RID: 855
		NotConst,
		// Token: 0x04000358 RID: 856
		NoAt,
		// Token: 0x04000359 RID: 857
		NoCatch,
		// Token: 0x0400035A RID: 858
		InvalidElse,
		// Token: 0x0400035B RID: 859
		NoComma = 1100,
		// Token: 0x0400035C RID: 860
		DupVisibility,
		// Token: 0x0400035D RID: 861
		IllegalVisibility,
		// Token: 0x0400035E RID: 862
		BadSwitch,
		// Token: 0x0400035F RID: 863
		CcInvalidEnd,
		// Token: 0x04000360 RID: 864
		CcInvalidElse,
		// Token: 0x04000361 RID: 865
		CcInvalidElif,
		// Token: 0x04000362 RID: 866
		ErrEOF,
		// Token: 0x04000363 RID: 867
		IncompatibleVisibility,
		// Token: 0x04000364 RID: 868
		ClassNotAllowed,
		// Token: 0x04000365 RID: 869
		NeedCompileTimeConstant,
		// Token: 0x04000366 RID: 870
		DuplicateName,
		// Token: 0x04000367 RID: 871
		NeedType,
		// Token: 0x04000368 RID: 872
		NotInsideClass,
		// Token: 0x04000369 RID: 873
		InvalidPositionDirective,
		// Token: 0x0400036A RID: 874
		MustBeEOL,
		// Token: 0x0400036B RID: 875
		WrongDirective = 1118,
		// Token: 0x0400036C RID: 876
		CannotNestPositionDirective,
		// Token: 0x0400036D RID: 877
		CircularDefinition,
		// Token: 0x0400036E RID: 878
		Deprecated,
		// Token: 0x0400036F RID: 879
		IllegalUseOfThis,
		// Token: 0x04000370 RID: 880
		NotAccessible,
		// Token: 0x04000371 RID: 881
		CannotUseNameOfClass,
		// Token: 0x04000372 RID: 882
		MustImplementMethod = 1128,
		// Token: 0x04000373 RID: 883
		NeedInterface,
		// Token: 0x04000374 RID: 884
		UnreachableCatch = 1133,
		// Token: 0x04000375 RID: 885
		TypeCannotBeExtended,
		// Token: 0x04000376 RID: 886
		UndeclaredVariable,
		// Token: 0x04000377 RID: 887
		VariableLeftUninitialized,
		// Token: 0x04000378 RID: 888
		KeywordUsedAsIdentifier,
		// Token: 0x04000379 RID: 889
		NotAllowedInSuperConstructorCall = 1140,
		// Token: 0x0400037A RID: 890
		NotMeantToBeCalledDirectly,
		// Token: 0x0400037B RID: 891
		GetAndSetAreInconsistent,
		// Token: 0x0400037C RID: 892
		InvalidCustomAttribute,
		// Token: 0x0400037D RID: 893
		InvalidCustomAttributeArgument,
		// Token: 0x0400037E RID: 894
		InvalidCustomAttributeClassOrCtor = 1146,
		// Token: 0x0400037F RID: 895
		TooManyParameters = 1148,
		// Token: 0x04000380 RID: 896
		AmbiguousBindingBecauseOfWith,
		// Token: 0x04000381 RID: 897
		AmbiguousBindingBecauseOfEval,
		// Token: 0x04000382 RID: 898
		NoSuchMember,
		// Token: 0x04000383 RID: 899
		ItemNotAllowedOnExpandoClass,
		// Token: 0x04000384 RID: 900
		MethodNotAllowedOnExpandoClass,
		// Token: 0x04000385 RID: 901
		MethodClashOnExpandoSuperClass = 1155,
		// Token: 0x04000386 RID: 902
		BaseClassIsExpandoAlready,
		// Token: 0x04000387 RID: 903
		AbstractCannotBePrivate,
		// Token: 0x04000388 RID: 904
		NotIndexable,
		// Token: 0x04000389 RID: 905
		StaticMissingInStaticInit,
		// Token: 0x0400038A RID: 906
		MissingConstructForAttributes,
		// Token: 0x0400038B RID: 907
		OnlyClassesAllowed,
		// Token: 0x0400038C RID: 908
		ExpandoClassShouldNotImpleEnumerable,
		// Token: 0x0400038D RID: 909
		NonCLSCompliantMember,
		// Token: 0x0400038E RID: 910
		NotDeletable,
		// Token: 0x0400038F RID: 911
		PackageExpected,
		// Token: 0x04000390 RID: 912
		UselessExpression = 1169,
		// Token: 0x04000391 RID: 913
		HidesParentMember,
		// Token: 0x04000392 RID: 914
		CannotChangeVisibility,
		// Token: 0x04000393 RID: 915
		HidesAbstractInBase,
		// Token: 0x04000394 RID: 916
		NewNotSpecifiedInMethodDeclaration,
		// Token: 0x04000395 RID: 917
		MethodInBaseIsNotVirtual,
		// Token: 0x04000396 RID: 918
		NoMethodInBaseToNew,
		// Token: 0x04000397 RID: 919
		DifferentReturnTypeFromBase,
		// Token: 0x04000398 RID: 920
		ClashWithProperty,
		// Token: 0x04000399 RID: 921
		OverrideAndHideUsedTogether,
		// Token: 0x0400039A RID: 922
		InvalidLanguageOption,
		// Token: 0x0400039B RID: 923
		NoMethodInBaseToOverride,
		// Token: 0x0400039C RID: 924
		NotValidForConstructor,
		// Token: 0x0400039D RID: 925
		CannotReturnValueFromVoidFunction,
		// Token: 0x0400039E RID: 926
		AmbiguousMatch,
		// Token: 0x0400039F RID: 927
		AmbiguousConstructorCall,
		// Token: 0x040003A0 RID: 928
		SuperClassConstructorNotAccessible,
		// Token: 0x040003A1 RID: 929
		OctalLiteralsAreDeprecated,
		// Token: 0x040003A2 RID: 930
		VariableMightBeUnitialized,
		// Token: 0x040003A3 RID: 931
		NotOKToCallSuper,
		// Token: 0x040003A4 RID: 932
		IllegalUseOfSuper,
		// Token: 0x040003A5 RID: 933
		BadWayToLeaveFinally,
		// Token: 0x040003A6 RID: 934
		NoCommaOrTypeDefinitionError,
		// Token: 0x040003A7 RID: 935
		AbstractWithBody,
		// Token: 0x040003A8 RID: 936
		NoRightParenOrComma,
		// Token: 0x040003A9 RID: 937
		NoRightBracketOrComma,
		// Token: 0x040003AA RID: 938
		ExpressionExpected,
		// Token: 0x040003AB RID: 939
		UnexpectedSemicolon,
		// Token: 0x040003AC RID: 940
		TooManyTokensSkipped,
		// Token: 0x040003AD RID: 941
		BadVariableDeclaration,
		// Token: 0x040003AE RID: 942
		BadFunctionDeclaration,
		// Token: 0x040003AF RID: 943
		BadPropertyDeclaration,
		// Token: 0x040003B0 RID: 944
		DoesNotHaveAnAddress = 1203,
		// Token: 0x040003B1 RID: 945
		TooFewParameters,
		// Token: 0x040003B2 RID: 946
		UselessAssignment,
		// Token: 0x040003B3 RID: 947
		SuspectAssignment,
		// Token: 0x040003B4 RID: 948
		SuspectSemicolon,
		// Token: 0x040003B5 RID: 949
		ImpossibleConversion,
		// Token: 0x040003B6 RID: 950
		FinalPrecludesAbstract,
		// Token: 0x040003B7 RID: 951
		NeedInstance,
		// Token: 0x040003B8 RID: 952
		CannotBeAbstract = 1212,
		// Token: 0x040003B9 RID: 953
		InvalidBaseTypeForEnum,
		// Token: 0x040003BA RID: 954
		CannotInstantiateAbstractClass,
		// Token: 0x040003BB RID: 955
		ArrayMayBeCopied,
		// Token: 0x040003BC RID: 956
		AbstractCannotBeStatic,
		// Token: 0x040003BD RID: 957
		StaticIsAlreadyFinal,
		// Token: 0x040003BE RID: 958
		StaticMethodsCannotOverride,
		// Token: 0x040003BF RID: 959
		StaticMethodsCannotHide,
		// Token: 0x040003C0 RID: 960
		ExpandoPrecludesOverride,
		// Token: 0x040003C1 RID: 961
		IllegalParamArrayAttribute,
		// Token: 0x040003C2 RID: 962
		ExpandoPrecludesAbstract,
		// Token: 0x040003C3 RID: 963
		ShouldBeAbstract,
		// Token: 0x040003C4 RID: 964
		BadModifierInInterface,
		// Token: 0x040003C5 RID: 965
		VarIllegalInInterface = 1226,
		// Token: 0x040003C6 RID: 966
		InterfaceIllegalInInterface,
		// Token: 0x040003C7 RID: 967
		NoVarInEnum,
		// Token: 0x040003C8 RID: 968
		InvalidImport,
		// Token: 0x040003C9 RID: 969
		EnumNotAllowed,
		// Token: 0x040003CA RID: 970
		InvalidCustomAttributeTarget,
		// Token: 0x040003CB RID: 971
		PackageInWrongContext,
		// Token: 0x040003CC RID: 972
		ConstructorMayNotHaveReturnType,
		// Token: 0x040003CD RID: 973
		OnlyClassesAndPackagesAllowed,
		// Token: 0x040003CE RID: 974
		InvalidDebugDirective,
		// Token: 0x040003CF RID: 975
		CustomAttributeUsedMoreThanOnce,
		// Token: 0x040003D0 RID: 976
		NestedInstanceTypeCannotBeExtendedByStatic,
		// Token: 0x040003D1 RID: 977
		PropertyLevelAttributesMustBeOnGetter,
		// Token: 0x040003D2 RID: 978
		BadThrow,
		// Token: 0x040003D3 RID: 979
		ParamListNotLast,
		// Token: 0x040003D4 RID: 980
		NoSuchType,
		// Token: 0x040003D5 RID: 981
		BadOctalLiteral,
		// Token: 0x040003D6 RID: 982
		InstanceNotAccessibleFromStatic,
		// Token: 0x040003D7 RID: 983
		StaticRequiresTypeName,
		// Token: 0x040003D8 RID: 984
		NonStaticWithTypeName,
		// Token: 0x040003D9 RID: 985
		NoSuchStaticMember,
		// Token: 0x040003DA RID: 986
		SuspectLoopCondition,
		// Token: 0x040003DB RID: 987
		ExpectedAssembly,
		// Token: 0x040003DC RID: 988
		AssemblyAttributesMustBeGlobal,
		// Token: 0x040003DD RID: 989
		ExpandoPrecludesStatic,
		// Token: 0x040003DE RID: 990
		DuplicateMethod,
		// Token: 0x040003DF RID: 991
		NotAnExpandoFunction,
		// Token: 0x040003E0 RID: 992
		NotValidVersionString,
		// Token: 0x040003E1 RID: 993
		ExecutablesCannotBeLocalized,
		// Token: 0x040003E2 RID: 994
		StringConcatIsSlow,
		// Token: 0x040003E3 RID: 995
		CcInvalidInDebugger,
		// Token: 0x040003E4 RID: 996
		ExpandoMustBePublic,
		// Token: 0x040003E5 RID: 997
		DelegatesShouldNotBeExplicitlyConstructed,
		// Token: 0x040003E6 RID: 998
		ImplicitlyReferencedAssemblyNotFound,
		// Token: 0x040003E7 RID: 999
		PossibleBadConversion,
		// Token: 0x040003E8 RID: 1000
		PossibleBadConversionFromString,
		// Token: 0x040003E9 RID: 1001
		InvalidResource,
		// Token: 0x040003EA RID: 1002
		WrongUseOfAddressOf,
		// Token: 0x040003EB RID: 1003
		NonCLSCompliantType,
		// Token: 0x040003EC RID: 1004
		MemberTypeCLSCompliantMismatch,
		// Token: 0x040003ED RID: 1005
		TypeAssemblyCLSCompliantMismatch,
		// Token: 0x040003EE RID: 1006
		IncompatibleAssemblyReference,
		// Token: 0x040003EF RID: 1007
		InvalidAssemblyKeyFile,
		// Token: 0x040003F0 RID: 1008
		TypeNameTooLong,
		// Token: 0x040003F1 RID: 1009
		MemberInitializerCannotContainFuncExpr,
		// Token: 0x040003F2 RID: 1010
		CantAssignThis = 5000,
		// Token: 0x040003F3 RID: 1011
		NumberExpected,
		// Token: 0x040003F4 RID: 1012
		FunctionExpected,
		// Token: 0x040003F5 RID: 1013
		CannotAssignToFunctionResult,
		// Token: 0x040003F6 RID: 1014
		StringExpected = 5005,
		// Token: 0x040003F7 RID: 1015
		DateExpected,
		// Token: 0x040003F8 RID: 1016
		ObjectExpected,
		// Token: 0x040003F9 RID: 1017
		IllegalAssignment,
		// Token: 0x040003FA RID: 1018
		UndefinedIdentifier,
		// Token: 0x040003FB RID: 1019
		BooleanExpected,
		// Token: 0x040003FC RID: 1020
		VBArrayExpected = 5013,
		// Token: 0x040003FD RID: 1021
		EnumeratorExpected = 5015,
		// Token: 0x040003FE RID: 1022
		RegExpExpected,
		// Token: 0x040003FF RID: 1023
		RegExpSyntax,
		// Token: 0x04000400 RID: 1024
		UncaughtException = 5022,
		// Token: 0x04000401 RID: 1025
		InvalidPrototype,
		// Token: 0x04000402 RID: 1026
		URIEncodeError,
		// Token: 0x04000403 RID: 1027
		URIDecodeError,
		// Token: 0x04000404 RID: 1028
		FractionOutOfRange,
		// Token: 0x04000405 RID: 1029
		PrecisionOutOfRange,
		// Token: 0x04000406 RID: 1030
		ArrayLengthConstructIncorrect = 5029,
		// Token: 0x04000407 RID: 1031
		ArrayLengthAssignIncorrect,
		// Token: 0x04000408 RID: 1032
		NeedArrayObject,
		// Token: 0x04000409 RID: 1033
		NoConstructor,
		// Token: 0x0400040A RID: 1034
		IllegalEval,
		// Token: 0x0400040B RID: 1035
		NotYetImplemented,
		// Token: 0x0400040C RID: 1036
		MustProvideNameForNamedParameter,
		// Token: 0x0400040D RID: 1037
		DuplicateNamedParameter,
		// Token: 0x0400040E RID: 1038
		MissingNameParameter,
		// Token: 0x0400040F RID: 1039
		MoreNamedParametersThanArguments,
		// Token: 0x04000410 RID: 1040
		NonSupportedInDebugger,
		// Token: 0x04000411 RID: 1041
		AssignmentToReadOnly,
		// Token: 0x04000412 RID: 1042
		WriteOnlyProperty,
		// Token: 0x04000413 RID: 1043
		IncorrectNumberOfIndices,
		// Token: 0x04000414 RID: 1044
		RefParamsNonSupportedInDebugger,
		// Token: 0x04000415 RID: 1045
		CannotCallSecurityMethodLateBound,
		// Token: 0x04000416 RID: 1046
		CannotUseStaticSecurityAttribute,
		// Token: 0x04000417 RID: 1047
		NonClsException,
		// Token: 0x04000418 RID: 1048
		FuncEvalAborted = 6000,
		// Token: 0x04000419 RID: 1049
		FuncEvalTimedout,
		// Token: 0x0400041A RID: 1050
		FuncEvalThreadSuspended,
		// Token: 0x0400041B RID: 1051
		FuncEvalThreadSleepWaitJoin,
		// Token: 0x0400041C RID: 1052
		FuncEvalBadThreadState,
		// Token: 0x0400041D RID: 1053
		FuncEvalBadThreadNotStarted,
		// Token: 0x0400041E RID: 1054
		NoFuncEvalAllowed,
		// Token: 0x0400041F RID: 1055
		FuncEvalBadLocation,
		// Token: 0x04000420 RID: 1056
		FuncEvalWebMethod,
		// Token: 0x04000421 RID: 1057
		StaticVarNotAvailable,
		// Token: 0x04000422 RID: 1058
		TypeObjectNotAvailable,
		// Token: 0x04000423 RID: 1059
		ExceptionFromHResult,
		// Token: 0x04000424 RID: 1060
		SideEffectsDisallowed
	}
}
