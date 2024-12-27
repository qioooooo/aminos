using System;

namespace System.Xml.Xsl.IlGen
{
	// Token: 0x02000039 RID: 57
	internal enum XmlILOptimization
	{
		// Token: 0x040002D2 RID: 722
		None,
		// Token: 0x040002D3 RID: 723
		FoldConstant,
		// Token: 0x040002D4 RID: 724
		TailCall,
		// Token: 0x040002D5 RID: 725
		AnnotateAncestor,
		// Token: 0x040002D6 RID: 726
		AnnotateAncestorSelf,
		// Token: 0x040002D7 RID: 727
		AnnotateAttribute,
		// Token: 0x040002D8 RID: 728
		AnnotateAttrNmspLoop,
		// Token: 0x040002D9 RID: 729
		AnnotateBarrier,
		// Token: 0x040002DA RID: 730
		AnnotateConstruction,
		// Token: 0x040002DB RID: 731
		AnnotateContent,
		// Token: 0x040002DC RID: 732
		AnnotateContentLoop,
		// Token: 0x040002DD RID: 733
		AnnotateDescendant,
		// Token: 0x040002DE RID: 734
		AnnotateDescendantLoop,
		// Token: 0x040002DF RID: 735
		AnnotateDescendantSelf,
		// Token: 0x040002E0 RID: 736
		AnnotateDifference,
		// Token: 0x040002E1 RID: 737
		AnnotateDod,
		// Token: 0x040002E2 RID: 738
		AnnotateDodMerge,
		// Token: 0x040002E3 RID: 739
		AnnotateDodReverse,
		// Token: 0x040002E4 RID: 740
		AnnotateFilter,
		// Token: 0x040002E5 RID: 741
		AnnotateFilterAttributeKind,
		// Token: 0x040002E6 RID: 742
		AnnotateFilterContentKind,
		// Token: 0x040002E7 RID: 743
		AnnotateFilterElements,
		// Token: 0x040002E8 RID: 744
		AnnotateFollowingSibling,
		// Token: 0x040002E9 RID: 745
		AnnotateIndex1,
		// Token: 0x040002EA RID: 746
		AnnotateIndex2,
		// Token: 0x040002EB RID: 747
		AnnotateIntersect,
		// Token: 0x040002EC RID: 748
		AnnotateInvoke,
		// Token: 0x040002ED RID: 749
		AnnotateJoinAndDod,
		// Token: 0x040002EE RID: 750
		AnnotateLet,
		// Token: 0x040002EF RID: 751
		AnnotateMaxLengthEq,
		// Token: 0x040002F0 RID: 752
		AnnotateMaxLengthGe,
		// Token: 0x040002F1 RID: 753
		AnnotateMaxLengthGt,
		// Token: 0x040002F2 RID: 754
		AnnotateMaxLengthLe,
		// Token: 0x040002F3 RID: 755
		AnnotateMaxLengthLt,
		// Token: 0x040002F4 RID: 756
		AnnotateMaxLengthNe,
		// Token: 0x040002F5 RID: 757
		AnnotateMaxPositionEq,
		// Token: 0x040002F6 RID: 758
		AnnotateMaxPositionLe,
		// Token: 0x040002F7 RID: 759
		AnnotateMaxPositionLt,
		// Token: 0x040002F8 RID: 760
		AnnotateNamespace,
		// Token: 0x040002F9 RID: 761
		AnnotateNodeRange,
		// Token: 0x040002FA RID: 762
		AnnotateParent,
		// Token: 0x040002FB RID: 763
		AnnotatePositionalIterator,
		// Token: 0x040002FC RID: 764
		AnnotatePreceding,
		// Token: 0x040002FD RID: 765
		AnnotatePrecedingSibling,
		// Token: 0x040002FE RID: 766
		AnnotateRoot,
		// Token: 0x040002FF RID: 767
		AnnotateRootLoop,
		// Token: 0x04000300 RID: 768
		AnnotateSingleTextRtf,
		// Token: 0x04000301 RID: 769
		AnnotateSingletonLoop,
		// Token: 0x04000302 RID: 770
		AnnotateTrackCallers,
		// Token: 0x04000303 RID: 771
		AnnotateUnion,
		// Token: 0x04000304 RID: 772
		AnnotateUnionContent,
		// Token: 0x04000305 RID: 773
		AnnotateXPathFollowing,
		// Token: 0x04000306 RID: 774
		AnnotateXPathPreceding,
		// Token: 0x04000307 RID: 775
		CommuteDodFilter,
		// Token: 0x04000308 RID: 776
		CommuteFilterLoop,
		// Token: 0x04000309 RID: 777
		EliminateAdd,
		// Token: 0x0400030A RID: 778
		EliminateAfter,
		// Token: 0x0400030B RID: 779
		EliminateAnd,
		// Token: 0x0400030C RID: 780
		EliminateAverage,
		// Token: 0x0400030D RID: 781
		EliminateBefore,
		// Token: 0x0400030E RID: 782
		EliminateConditional,
		// Token: 0x0400030F RID: 783
		EliminateDifference,
		// Token: 0x04000310 RID: 784
		EliminateDivide,
		// Token: 0x04000311 RID: 785
		EliminateDod,
		// Token: 0x04000312 RID: 786
		EliminateEq,
		// Token: 0x04000313 RID: 787
		EliminateFilter,
		// Token: 0x04000314 RID: 788
		EliminateGe,
		// Token: 0x04000315 RID: 789
		EliminateGt,
		// Token: 0x04000316 RID: 790
		EliminateIntersection,
		// Token: 0x04000317 RID: 791
		EliminateIs,
		// Token: 0x04000318 RID: 792
		EliminateIsEmpty,
		// Token: 0x04000319 RID: 793
		EliminateIsType,
		// Token: 0x0400031A RID: 794
		EliminateIterator,
		// Token: 0x0400031B RID: 795
		EliminateIteratorUsedAtMostOnce,
		// Token: 0x0400031C RID: 796
		EliminateLe,
		// Token: 0x0400031D RID: 797
		EliminateLength,
		// Token: 0x0400031E RID: 798
		EliminateLoop,
		// Token: 0x0400031F RID: 799
		EliminateLt,
		// Token: 0x04000320 RID: 800
		EliminateMaximum,
		// Token: 0x04000321 RID: 801
		EliminateMinimum,
		// Token: 0x04000322 RID: 802
		EliminateModulo,
		// Token: 0x04000323 RID: 803
		EliminateMultiply,
		// Token: 0x04000324 RID: 804
		EliminateNamespaceDecl,
		// Token: 0x04000325 RID: 805
		EliminateNe,
		// Token: 0x04000326 RID: 806
		EliminateNegate,
		// Token: 0x04000327 RID: 807
		EliminateNop,
		// Token: 0x04000328 RID: 808
		EliminateNot,
		// Token: 0x04000329 RID: 809
		EliminateOr,
		// Token: 0x0400032A RID: 810
		EliminatePositionOf,
		// Token: 0x0400032B RID: 811
		EliminateReturnDod,
		// Token: 0x0400032C RID: 812
		EliminateSequence,
		// Token: 0x0400032D RID: 813
		EliminateSort,
		// Token: 0x0400032E RID: 814
		EliminateStrConcat,
		// Token: 0x0400032F RID: 815
		EliminateStrConcatSingle,
		// Token: 0x04000330 RID: 816
		EliminateStrLength,
		// Token: 0x04000331 RID: 817
		EliminateSubtract,
		// Token: 0x04000332 RID: 818
		EliminateSum,
		// Token: 0x04000333 RID: 819
		EliminateTypeAssert,
		// Token: 0x04000334 RID: 820
		EliminateTypeAssertOptional,
		// Token: 0x04000335 RID: 821
		EliminateUnion,
		// Token: 0x04000336 RID: 822
		EliminateUnusedFunctions,
		// Token: 0x04000337 RID: 823
		EliminateXsltConvert,
		// Token: 0x04000338 RID: 824
		FoldConditionalNot,
		// Token: 0x04000339 RID: 825
		FoldNamedDescendants,
		// Token: 0x0400033A RID: 826
		FoldNone,
		// Token: 0x0400033B RID: 827
		FoldXsltConvertLiteral,
		// Token: 0x0400033C RID: 828
		IntroduceDod,
		// Token: 0x0400033D RID: 829
		IntroducePrecedingDod,
		// Token: 0x0400033E RID: 830
		NormalizeAddEq,
		// Token: 0x0400033F RID: 831
		NormalizeAddLiteral,
		// Token: 0x04000340 RID: 832
		NormalizeAttribute,
		// Token: 0x04000341 RID: 833
		NormalizeConditionalText,
		// Token: 0x04000342 RID: 834
		NormalizeDifference,
		// Token: 0x04000343 RID: 835
		NormalizeEqLiteral,
		// Token: 0x04000344 RID: 836
		NormalizeGeLiteral,
		// Token: 0x04000345 RID: 837
		NormalizeGtLiteral,
		// Token: 0x04000346 RID: 838
		NormalizeIdEq,
		// Token: 0x04000347 RID: 839
		NormalizeIdNe,
		// Token: 0x04000348 RID: 840
		NormalizeIntersect,
		// Token: 0x04000349 RID: 841
		NormalizeInvokeEmpty,
		// Token: 0x0400034A RID: 842
		NormalizeLeLiteral,
		// Token: 0x0400034B RID: 843
		NormalizeLengthGt,
		// Token: 0x0400034C RID: 844
		NormalizeLengthNe,
		// Token: 0x0400034D RID: 845
		NormalizeLoopConditional,
		// Token: 0x0400034E RID: 846
		NormalizeLoopInvariant,
		// Token: 0x0400034F RID: 847
		NormalizeLoopLoop,
		// Token: 0x04000350 RID: 848
		NormalizeLoopText,
		// Token: 0x04000351 RID: 849
		NormalizeLtLiteral,
		// Token: 0x04000352 RID: 850
		NormalizeMuenchian,
		// Token: 0x04000353 RID: 851
		NormalizeMultiplyLiteral,
		// Token: 0x04000354 RID: 852
		NormalizeNeLiteral,
		// Token: 0x04000355 RID: 853
		NormalizeNestedSequences,
		// Token: 0x04000356 RID: 854
		NormalizeSingletonLet,
		// Token: 0x04000357 RID: 855
		NormalizeSortXsltConvert,
		// Token: 0x04000358 RID: 856
		NormalizeUnion,
		// Token: 0x04000359 RID: 857
		NormalizeXsltConvertEq,
		// Token: 0x0400035A RID: 858
		NormalizeXsltConvertGe,
		// Token: 0x0400035B RID: 859
		NormalizeXsltConvertGt,
		// Token: 0x0400035C RID: 860
		NormalizeXsltConvertLe,
		// Token: 0x0400035D RID: 861
		NormalizeXsltConvertLt,
		// Token: 0x0400035E RID: 862
		NormalizeXsltConvertNe,
		// Token: 0x0400035F RID: 863
		Last_
	}
}
