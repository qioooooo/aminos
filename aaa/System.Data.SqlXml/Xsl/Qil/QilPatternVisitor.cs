using System;
using System.Collections;

namespace System.Xml.Xsl.Qil
{
	// Token: 0x0200003B RID: 59
	internal abstract class QilPatternVisitor : QilReplaceVisitor
	{
		// Token: 0x06000356 RID: 854 RVA: 0x0001657E File Offset: 0x0001557E
		public QilPatternVisitor(QilPatternVisitor.QilPatterns patterns, QilFactory f)
			: base(f)
		{
			this.Patterns = patterns;
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000357 RID: 855 RVA: 0x00016599 File Offset: 0x00015599
		// (set) Token: 0x06000358 RID: 856 RVA: 0x000165A1 File Offset: 0x000155A1
		public QilPatternVisitor.QilPatterns Patterns
		{
			get
			{
				return this.patterns;
			}
			set
			{
				this.patterns = value;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000359 RID: 857 RVA: 0x000165AA File Offset: 0x000155AA
		// (set) Token: 0x0600035A RID: 858 RVA: 0x000165B2 File Offset: 0x000155B2
		public int Threshold
		{
			get
			{
				return this.threshold;
			}
			set
			{
				this.threshold = value;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600035B RID: 859 RVA: 0x000165BB File Offset: 0x000155BB
		public int ReplacementCount
		{
			get
			{
				return this.replacementCnt;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600035C RID: 860 RVA: 0x000165C3 File Offset: 0x000155C3
		public int LastReplacement
		{
			get
			{
				return this.lastReplacement;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600035D RID: 861 RVA: 0x000165CB File Offset: 0x000155CB
		public bool Matching
		{
			get
			{
				return this.ReplacementCount < this.Threshold;
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000165DB File Offset: 0x000155DB
		protected virtual bool AllowReplace(int pattern, QilNode original)
		{
			if (this.Matching)
			{
				this.replacementCnt++;
				this.lastReplacement = pattern;
				return true;
			}
			return false;
		}

		// Token: 0x0600035F RID: 863 RVA: 0x000165FD File Offset: 0x000155FD
		protected virtual QilNode Replace(int pattern, QilNode original, QilNode replacement)
		{
			replacement.SourceLine = original.SourceLine;
			return replacement;
		}

		// Token: 0x06000360 RID: 864 RVA: 0x0001660C File Offset: 0x0001560C
		protected virtual QilNode NoReplace(QilNode node)
		{
			return node;
		}

		// Token: 0x06000361 RID: 865 RVA: 0x0001660F File Offset: 0x0001560F
		protected override QilNode Visit(QilNode node)
		{
			if (node == null)
			{
				return this.VisitNull();
			}
			node = this.VisitChildren(node);
			return base.Visit(node);
		}

		// Token: 0x06000362 RID: 866 RVA: 0x0001662B File Offset: 0x0001562B
		protected override QilNode VisitQilExpression(QilExpression n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000363 RID: 867 RVA: 0x00016634 File Offset: 0x00015634
		protected override QilNode VisitFunctionList(QilList n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000364 RID: 868 RVA: 0x0001663D File Offset: 0x0001563D
		protected override QilNode VisitGlobalVariableList(QilList n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000365 RID: 869 RVA: 0x00016646 File Offset: 0x00015646
		protected override QilNode VisitGlobalParameterList(QilList n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x0001664F File Offset: 0x0001564F
		protected override QilNode VisitActualParameterList(QilList n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x00016658 File Offset: 0x00015658
		protected override QilNode VisitFormalParameterList(QilList n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000368 RID: 872 RVA: 0x00016661 File Offset: 0x00015661
		protected override QilNode VisitSortKeyList(QilList n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000369 RID: 873 RVA: 0x0001666A File Offset: 0x0001566A
		protected override QilNode VisitBranchList(QilList n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600036A RID: 874 RVA: 0x00016673 File Offset: 0x00015673
		protected override QilNode VisitOptimizeBarrier(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600036B RID: 875 RVA: 0x0001667C File Offset: 0x0001567C
		protected override QilNode VisitUnknown(QilNode n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600036C RID: 876 RVA: 0x00016685 File Offset: 0x00015685
		protected override QilNode VisitDataSource(QilDataSource n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600036D RID: 877 RVA: 0x0001668E File Offset: 0x0001568E
		protected override QilNode VisitNop(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600036E RID: 878 RVA: 0x00016697 File Offset: 0x00015697
		protected override QilNode VisitError(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600036F RID: 879 RVA: 0x000166A0 File Offset: 0x000156A0
		protected override QilNode VisitWarning(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000370 RID: 880 RVA: 0x000166A9 File Offset: 0x000156A9
		protected override QilNode VisitFor(QilIterator n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000371 RID: 881 RVA: 0x000166B2 File Offset: 0x000156B2
		protected override QilNode VisitForReference(QilIterator n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000372 RID: 882 RVA: 0x000166BB File Offset: 0x000156BB
		protected override QilNode VisitLet(QilIterator n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000166C4 File Offset: 0x000156C4
		protected override QilNode VisitLetReference(QilIterator n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000166CD File Offset: 0x000156CD
		protected override QilNode VisitParameter(QilParameter n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x000166D6 File Offset: 0x000156D6
		protected override QilNode VisitParameterReference(QilParameter n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000376 RID: 886 RVA: 0x000166DF File Offset: 0x000156DF
		protected override QilNode VisitPositionOf(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000166E8 File Offset: 0x000156E8
		protected override QilNode VisitTrue(QilNode n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000378 RID: 888 RVA: 0x000166F1 File Offset: 0x000156F1
		protected override QilNode VisitFalse(QilNode n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000379 RID: 889 RVA: 0x000166FA File Offset: 0x000156FA
		protected override QilNode VisitLiteralString(QilLiteral n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600037A RID: 890 RVA: 0x00016703 File Offset: 0x00015703
		protected override QilNode VisitLiteralInt32(QilLiteral n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600037B RID: 891 RVA: 0x0001670C File Offset: 0x0001570C
		protected override QilNode VisitLiteralInt64(QilLiteral n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600037C RID: 892 RVA: 0x00016715 File Offset: 0x00015715
		protected override QilNode VisitLiteralDouble(QilLiteral n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600037D RID: 893 RVA: 0x0001671E File Offset: 0x0001571E
		protected override QilNode VisitLiteralDecimal(QilLiteral n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600037E RID: 894 RVA: 0x00016727 File Offset: 0x00015727
		protected override QilNode VisitLiteralQName(QilName n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00016730 File Offset: 0x00015730
		protected override QilNode VisitLiteralType(QilLiteral n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00016739 File Offset: 0x00015739
		protected override QilNode VisitLiteralObject(QilLiteral n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000381 RID: 897 RVA: 0x00016742 File Offset: 0x00015742
		protected override QilNode VisitAnd(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000382 RID: 898 RVA: 0x0001674B File Offset: 0x0001574B
		protected override QilNode VisitOr(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00016754 File Offset: 0x00015754
		protected override QilNode VisitNot(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001675D File Offset: 0x0001575D
		protected override QilNode VisitConditional(QilTernary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00016766 File Offset: 0x00015766
		protected override QilNode VisitChoice(QilChoice n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000386 RID: 902 RVA: 0x0001676F File Offset: 0x0001576F
		protected override QilNode VisitLength(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00016778 File Offset: 0x00015778
		protected override QilNode VisitSequence(QilList n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00016781 File Offset: 0x00015781
		protected override QilNode VisitUnion(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000389 RID: 905 RVA: 0x0001678A File Offset: 0x0001578A
		protected override QilNode VisitIntersection(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00016793 File Offset: 0x00015793
		protected override QilNode VisitDifference(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600038B RID: 907 RVA: 0x0001679C File Offset: 0x0001579C
		protected override QilNode VisitAverage(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600038C RID: 908 RVA: 0x000167A5 File Offset: 0x000157A5
		protected override QilNode VisitSum(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600038D RID: 909 RVA: 0x000167AE File Offset: 0x000157AE
		protected override QilNode VisitMinimum(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600038E RID: 910 RVA: 0x000167B7 File Offset: 0x000157B7
		protected override QilNode VisitMaximum(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600038F RID: 911 RVA: 0x000167C0 File Offset: 0x000157C0
		protected override QilNode VisitNegate(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000390 RID: 912 RVA: 0x000167C9 File Offset: 0x000157C9
		protected override QilNode VisitAdd(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000391 RID: 913 RVA: 0x000167D2 File Offset: 0x000157D2
		protected override QilNode VisitSubtract(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000392 RID: 914 RVA: 0x000167DB File Offset: 0x000157DB
		protected override QilNode VisitMultiply(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000393 RID: 915 RVA: 0x000167E4 File Offset: 0x000157E4
		protected override QilNode VisitDivide(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000394 RID: 916 RVA: 0x000167ED File Offset: 0x000157ED
		protected override QilNode VisitModulo(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000395 RID: 917 RVA: 0x000167F6 File Offset: 0x000157F6
		protected override QilNode VisitStrLength(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000396 RID: 918 RVA: 0x000167FF File Offset: 0x000157FF
		protected override QilNode VisitStrConcat(QilStrConcat n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00016808 File Offset: 0x00015808
		protected override QilNode VisitStrParseQName(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00016811 File Offset: 0x00015811
		protected override QilNode VisitNe(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x06000399 RID: 921 RVA: 0x0001681A File Offset: 0x0001581A
		protected override QilNode VisitEq(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00016823 File Offset: 0x00015823
		protected override QilNode VisitGt(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x0001682C File Offset: 0x0001582C
		protected override QilNode VisitGe(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00016835 File Offset: 0x00015835
		protected override QilNode VisitLt(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600039D RID: 925 RVA: 0x0001683E File Offset: 0x0001583E
		protected override QilNode VisitLe(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600039E RID: 926 RVA: 0x00016847 File Offset: 0x00015847
		protected override QilNode VisitIs(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x0600039F RID: 927 RVA: 0x00016850 File Offset: 0x00015850
		protected override QilNode VisitAfter(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00016859 File Offset: 0x00015859
		protected override QilNode VisitBefore(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x00016862 File Offset: 0x00015862
		protected override QilNode VisitLoop(QilLoop n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001686B File Offset: 0x0001586B
		protected override QilNode VisitFilter(QilLoop n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x00016874 File Offset: 0x00015874
		protected override QilNode VisitSort(QilLoop n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x0001687D File Offset: 0x0001587D
		protected override QilNode VisitSortKey(QilSortKey n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A5 RID: 933 RVA: 0x00016886 File Offset: 0x00015886
		protected override QilNode VisitDocOrderDistinct(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A6 RID: 934 RVA: 0x0001688F File Offset: 0x0001588F
		protected override QilNode VisitFunction(QilFunction n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A7 RID: 935 RVA: 0x00016898 File Offset: 0x00015898
		protected override QilNode VisitFunctionReference(QilFunction n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A8 RID: 936 RVA: 0x000168A1 File Offset: 0x000158A1
		protected override QilNode VisitInvoke(QilInvoke n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x000168AA File Offset: 0x000158AA
		protected override QilNode VisitContent(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003AA RID: 938 RVA: 0x000168B3 File Offset: 0x000158B3
		protected override QilNode VisitAttribute(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003AB RID: 939 RVA: 0x000168BC File Offset: 0x000158BC
		protected override QilNode VisitParent(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000168C5 File Offset: 0x000158C5
		protected override QilNode VisitRoot(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000168CE File Offset: 0x000158CE
		protected override QilNode VisitXmlContext(QilNode n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003AE RID: 942 RVA: 0x000168D7 File Offset: 0x000158D7
		protected override QilNode VisitDescendant(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003AF RID: 943 RVA: 0x000168E0 File Offset: 0x000158E0
		protected override QilNode VisitDescendantOrSelf(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x000168E9 File Offset: 0x000158E9
		protected override QilNode VisitAncestor(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B1 RID: 945 RVA: 0x000168F2 File Offset: 0x000158F2
		protected override QilNode VisitAncestorOrSelf(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x000168FB File Offset: 0x000158FB
		protected override QilNode VisitPreceding(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00016904 File Offset: 0x00015904
		protected override QilNode VisitFollowingSibling(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x0001690D File Offset: 0x0001590D
		protected override QilNode VisitPrecedingSibling(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00016916 File Offset: 0x00015916
		protected override QilNode VisitNodeRange(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x0001691F File Offset: 0x0001591F
		protected override QilNode VisitDeref(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00016928 File Offset: 0x00015928
		protected override QilNode VisitElementCtor(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00016931 File Offset: 0x00015931
		protected override QilNode VisitAttributeCtor(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x0001693A File Offset: 0x0001593A
		protected override QilNode VisitCommentCtor(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00016943 File Offset: 0x00015943
		protected override QilNode VisitPICtor(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003BB RID: 955 RVA: 0x0001694C File Offset: 0x0001594C
		protected override QilNode VisitTextCtor(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00016955 File Offset: 0x00015955
		protected override QilNode VisitRawTextCtor(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003BD RID: 957 RVA: 0x0001695E File Offset: 0x0001595E
		protected override QilNode VisitDocumentCtor(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003BE RID: 958 RVA: 0x00016967 File Offset: 0x00015967
		protected override QilNode VisitNamespaceDecl(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x00016970 File Offset: 0x00015970
		protected override QilNode VisitRtfCtor(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x00016979 File Offset: 0x00015979
		protected override QilNode VisitNameOf(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C1 RID: 961 RVA: 0x00016982 File Offset: 0x00015982
		protected override QilNode VisitLocalNameOf(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C2 RID: 962 RVA: 0x0001698B File Offset: 0x0001598B
		protected override QilNode VisitNamespaceUriOf(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00016994 File Offset: 0x00015994
		protected override QilNode VisitPrefixOf(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x0001699D File Offset: 0x0001599D
		protected override QilNode VisitTypeAssert(QilTargetType n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x000169A6 File Offset: 0x000159A6
		protected override QilNode VisitIsType(QilTargetType n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x000169AF File Offset: 0x000159AF
		protected override QilNode VisitIsEmpty(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C7 RID: 967 RVA: 0x000169B8 File Offset: 0x000159B8
		protected override QilNode VisitXPathNodeValue(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000169C1 File Offset: 0x000159C1
		protected override QilNode VisitXPathFollowing(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x000169CA File Offset: 0x000159CA
		protected override QilNode VisitXPathPreceding(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003CA RID: 970 RVA: 0x000169D3 File Offset: 0x000159D3
		protected override QilNode VisitXPathNamespace(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000169DC File Offset: 0x000159DC
		protected override QilNode VisitXsltGenerateId(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000169E5 File Offset: 0x000159E5
		protected override QilNode VisitXsltInvokeLateBound(QilInvokeLateBound n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000169EE File Offset: 0x000159EE
		protected override QilNode VisitXsltInvokeEarlyBound(QilInvokeEarlyBound n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003CE RID: 974 RVA: 0x000169F7 File Offset: 0x000159F7
		protected override QilNode VisitXsltCopy(QilBinary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003CF RID: 975 RVA: 0x00016A00 File Offset: 0x00015A00
		protected override QilNode VisitXsltCopyOf(QilUnary n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x00016A09 File Offset: 0x00015A09
		protected override QilNode VisitXsltConvert(QilTargetType n)
		{
			return this.NoReplace(n);
		}

		// Token: 0x04000361 RID: 865
		private QilPatternVisitor.QilPatterns patterns;

		// Token: 0x04000362 RID: 866
		private int replacementCnt;

		// Token: 0x04000363 RID: 867
		private int lastReplacement;

		// Token: 0x04000364 RID: 868
		private int threshold = int.MaxValue;

		// Token: 0x0200003C RID: 60
		internal sealed class QilPatterns
		{
			// Token: 0x060003D1 RID: 977 RVA: 0x00016A12 File Offset: 0x00015A12
			private QilPatterns(QilPatternVisitor.QilPatterns toCopy)
			{
				this.bits = new BitArray(toCopy.bits);
			}

			// Token: 0x060003D2 RID: 978 RVA: 0x00016A2B File Offset: 0x00015A2B
			public QilPatterns(int szBits, bool allSet)
			{
				this.bits = new BitArray(szBits, allSet);
			}

			// Token: 0x060003D3 RID: 979 RVA: 0x00016A40 File Offset: 0x00015A40
			public QilPatternVisitor.QilPatterns Clone()
			{
				return new QilPatternVisitor.QilPatterns(this);
			}

			// Token: 0x060003D4 RID: 980 RVA: 0x00016A48 File Offset: 0x00015A48
			public void ClearAll()
			{
				this.bits.SetAll(false);
			}

			// Token: 0x060003D5 RID: 981 RVA: 0x00016A56 File Offset: 0x00015A56
			public void Add(int i)
			{
				this.bits.Set(i, true);
			}

			// Token: 0x060003D6 RID: 982 RVA: 0x00016A65 File Offset: 0x00015A65
			public bool IsSet(int i)
			{
				return this.bits[i];
			}

			// Token: 0x04000365 RID: 869
			private BitArray bits;
		}
	}
}
