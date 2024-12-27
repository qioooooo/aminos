using System;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200019C RID: 412
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class StandardCommands
	{
		// Token: 0x04000B07 RID: 2823
		private const int cmdidDesignerVerbFirst = 8192;

		// Token: 0x04000B08 RID: 2824
		private const int cmdidDesignerVerbLast = 8448;

		// Token: 0x04000B09 RID: 2825
		private const int cmdidArrangeIcons = 12298;

		// Token: 0x04000B0A RID: 2826
		private const int cmdidLineupIcons = 12299;

		// Token: 0x04000B0B RID: 2827
		private const int cmdidShowLargeIcons = 12300;

		// Token: 0x04000B0C RID: 2828
		private static readonly Guid standardCommandSet = StandardCommands.ShellGuids.VSStandardCommandSet97;

		// Token: 0x04000B0D RID: 2829
		private static readonly Guid ndpCommandSet = new Guid("{74D21313-2AEE-11d1-8BFB-00A0C90F26F7}");

		// Token: 0x04000B0E RID: 2830
		public static readonly CommandID AlignBottom = new CommandID(StandardCommands.standardCommandSet, 1);

		// Token: 0x04000B0F RID: 2831
		public static readonly CommandID AlignHorizontalCenters = new CommandID(StandardCommands.standardCommandSet, 2);

		// Token: 0x04000B10 RID: 2832
		public static readonly CommandID AlignLeft = new CommandID(StandardCommands.standardCommandSet, 3);

		// Token: 0x04000B11 RID: 2833
		public static readonly CommandID AlignRight = new CommandID(StandardCommands.standardCommandSet, 4);

		// Token: 0x04000B12 RID: 2834
		public static readonly CommandID AlignToGrid = new CommandID(StandardCommands.standardCommandSet, 5);

		// Token: 0x04000B13 RID: 2835
		public static readonly CommandID AlignTop = new CommandID(StandardCommands.standardCommandSet, 6);

		// Token: 0x04000B14 RID: 2836
		public static readonly CommandID AlignVerticalCenters = new CommandID(StandardCommands.standardCommandSet, 7);

		// Token: 0x04000B15 RID: 2837
		public static readonly CommandID ArrangeBottom = new CommandID(StandardCommands.standardCommandSet, 8);

		// Token: 0x04000B16 RID: 2838
		public static readonly CommandID ArrangeRight = new CommandID(StandardCommands.standardCommandSet, 9);

		// Token: 0x04000B17 RID: 2839
		public static readonly CommandID BringForward = new CommandID(StandardCommands.standardCommandSet, 10);

		// Token: 0x04000B18 RID: 2840
		public static readonly CommandID BringToFront = new CommandID(StandardCommands.standardCommandSet, 11);

		// Token: 0x04000B19 RID: 2841
		public static readonly CommandID CenterHorizontally = new CommandID(StandardCommands.standardCommandSet, 12);

		// Token: 0x04000B1A RID: 2842
		public static readonly CommandID CenterVertically = new CommandID(StandardCommands.standardCommandSet, 13);

		// Token: 0x04000B1B RID: 2843
		public static readonly CommandID ViewCode = new CommandID(StandardCommands.standardCommandSet, 333);

		// Token: 0x04000B1C RID: 2844
		public static readonly CommandID DocumentOutline = new CommandID(StandardCommands.standardCommandSet, 239);

		// Token: 0x04000B1D RID: 2845
		public static readonly CommandID Copy = new CommandID(StandardCommands.standardCommandSet, 15);

		// Token: 0x04000B1E RID: 2846
		public static readonly CommandID Cut = new CommandID(StandardCommands.standardCommandSet, 16);

		// Token: 0x04000B1F RID: 2847
		public static readonly CommandID Delete = new CommandID(StandardCommands.standardCommandSet, 17);

		// Token: 0x04000B20 RID: 2848
		public static readonly CommandID Group = new CommandID(StandardCommands.standardCommandSet, 20);

		// Token: 0x04000B21 RID: 2849
		public static readonly CommandID HorizSpaceConcatenate = new CommandID(StandardCommands.standardCommandSet, 21);

		// Token: 0x04000B22 RID: 2850
		public static readonly CommandID HorizSpaceDecrease = new CommandID(StandardCommands.standardCommandSet, 22);

		// Token: 0x04000B23 RID: 2851
		public static readonly CommandID HorizSpaceIncrease = new CommandID(StandardCommands.standardCommandSet, 23);

		// Token: 0x04000B24 RID: 2852
		public static readonly CommandID HorizSpaceMakeEqual = new CommandID(StandardCommands.standardCommandSet, 24);

		// Token: 0x04000B25 RID: 2853
		public static readonly CommandID Paste = new CommandID(StandardCommands.standardCommandSet, 26);

		// Token: 0x04000B26 RID: 2854
		public static readonly CommandID Properties = new CommandID(StandardCommands.standardCommandSet, 28);

		// Token: 0x04000B27 RID: 2855
		public static readonly CommandID Redo = new CommandID(StandardCommands.standardCommandSet, 29);

		// Token: 0x04000B28 RID: 2856
		public static readonly CommandID MultiLevelRedo = new CommandID(StandardCommands.standardCommandSet, 30);

		// Token: 0x04000B29 RID: 2857
		public static readonly CommandID SelectAll = new CommandID(StandardCommands.standardCommandSet, 31);

		// Token: 0x04000B2A RID: 2858
		public static readonly CommandID SendBackward = new CommandID(StandardCommands.standardCommandSet, 32);

		// Token: 0x04000B2B RID: 2859
		public static readonly CommandID SendToBack = new CommandID(StandardCommands.standardCommandSet, 33);

		// Token: 0x04000B2C RID: 2860
		public static readonly CommandID SizeToControl = new CommandID(StandardCommands.standardCommandSet, 35);

		// Token: 0x04000B2D RID: 2861
		public static readonly CommandID SizeToControlHeight = new CommandID(StandardCommands.standardCommandSet, 36);

		// Token: 0x04000B2E RID: 2862
		public static readonly CommandID SizeToControlWidth = new CommandID(StandardCommands.standardCommandSet, 37);

		// Token: 0x04000B2F RID: 2863
		public static readonly CommandID SizeToFit = new CommandID(StandardCommands.standardCommandSet, 38);

		// Token: 0x04000B30 RID: 2864
		public static readonly CommandID SizeToGrid = new CommandID(StandardCommands.standardCommandSet, 39);

		// Token: 0x04000B31 RID: 2865
		public static readonly CommandID SnapToGrid = new CommandID(StandardCommands.standardCommandSet, 40);

		// Token: 0x04000B32 RID: 2866
		public static readonly CommandID TabOrder = new CommandID(StandardCommands.standardCommandSet, 41);

		// Token: 0x04000B33 RID: 2867
		public static readonly CommandID Undo = new CommandID(StandardCommands.standardCommandSet, 43);

		// Token: 0x04000B34 RID: 2868
		public static readonly CommandID MultiLevelUndo = new CommandID(StandardCommands.standardCommandSet, 44);

		// Token: 0x04000B35 RID: 2869
		public static readonly CommandID Ungroup = new CommandID(StandardCommands.standardCommandSet, 45);

		// Token: 0x04000B36 RID: 2870
		public static readonly CommandID VertSpaceConcatenate = new CommandID(StandardCommands.standardCommandSet, 46);

		// Token: 0x04000B37 RID: 2871
		public static readonly CommandID VertSpaceDecrease = new CommandID(StandardCommands.standardCommandSet, 47);

		// Token: 0x04000B38 RID: 2872
		public static readonly CommandID VertSpaceIncrease = new CommandID(StandardCommands.standardCommandSet, 48);

		// Token: 0x04000B39 RID: 2873
		public static readonly CommandID VertSpaceMakeEqual = new CommandID(StandardCommands.standardCommandSet, 49);

		// Token: 0x04000B3A RID: 2874
		public static readonly CommandID ShowGrid = new CommandID(StandardCommands.standardCommandSet, 103);

		// Token: 0x04000B3B RID: 2875
		public static readonly CommandID ViewGrid = new CommandID(StandardCommands.standardCommandSet, 125);

		// Token: 0x04000B3C RID: 2876
		public static readonly CommandID Replace = new CommandID(StandardCommands.standardCommandSet, 230);

		// Token: 0x04000B3D RID: 2877
		public static readonly CommandID PropertiesWindow = new CommandID(StandardCommands.standardCommandSet, 235);

		// Token: 0x04000B3E RID: 2878
		public static readonly CommandID LockControls = new CommandID(StandardCommands.standardCommandSet, 369);

		// Token: 0x04000B3F RID: 2879
		public static readonly CommandID F1Help = new CommandID(StandardCommands.standardCommandSet, 377);

		// Token: 0x04000B40 RID: 2880
		public static readonly CommandID ArrangeIcons = new CommandID(StandardCommands.ndpCommandSet, 12298);

		// Token: 0x04000B41 RID: 2881
		public static readonly CommandID LineupIcons = new CommandID(StandardCommands.ndpCommandSet, 12299);

		// Token: 0x04000B42 RID: 2882
		public static readonly CommandID ShowLargeIcons = new CommandID(StandardCommands.ndpCommandSet, 12300);

		// Token: 0x04000B43 RID: 2883
		public static readonly CommandID VerbFirst = new CommandID(StandardCommands.ndpCommandSet, 8192);

		// Token: 0x04000B44 RID: 2884
		public static readonly CommandID VerbLast = new CommandID(StandardCommands.ndpCommandSet, 8448);

		// Token: 0x0200019D RID: 413
		private static class VSStandardCommands
		{
			// Token: 0x04000B45 RID: 2885
			internal const int cmdidAlignBottom = 1;

			// Token: 0x04000B46 RID: 2886
			internal const int cmdidAlignHorizontalCenters = 2;

			// Token: 0x04000B47 RID: 2887
			internal const int cmdidAlignLeft = 3;

			// Token: 0x04000B48 RID: 2888
			internal const int cmdidAlignRight = 4;

			// Token: 0x04000B49 RID: 2889
			internal const int cmdidAlignToGrid = 5;

			// Token: 0x04000B4A RID: 2890
			internal const int cmdidAlignTop = 6;

			// Token: 0x04000B4B RID: 2891
			internal const int cmdidAlignVerticalCenters = 7;

			// Token: 0x04000B4C RID: 2892
			internal const int cmdidArrangeBottom = 8;

			// Token: 0x04000B4D RID: 2893
			internal const int cmdidArrangeRight = 9;

			// Token: 0x04000B4E RID: 2894
			internal const int cmdidBringForward = 10;

			// Token: 0x04000B4F RID: 2895
			internal const int cmdidBringToFront = 11;

			// Token: 0x04000B50 RID: 2896
			internal const int cmdidCenterHorizontally = 12;

			// Token: 0x04000B51 RID: 2897
			internal const int cmdidCenterVertically = 13;

			// Token: 0x04000B52 RID: 2898
			internal const int cmdidCode = 14;

			// Token: 0x04000B53 RID: 2899
			internal const int cmdidCopy = 15;

			// Token: 0x04000B54 RID: 2900
			internal const int cmdidCut = 16;

			// Token: 0x04000B55 RID: 2901
			internal const int cmdidDelete = 17;

			// Token: 0x04000B56 RID: 2902
			internal const int cmdidFontName = 18;

			// Token: 0x04000B57 RID: 2903
			internal const int cmdidFontSize = 19;

			// Token: 0x04000B58 RID: 2904
			internal const int cmdidGroup = 20;

			// Token: 0x04000B59 RID: 2905
			internal const int cmdidHorizSpaceConcatenate = 21;

			// Token: 0x04000B5A RID: 2906
			internal const int cmdidHorizSpaceDecrease = 22;

			// Token: 0x04000B5B RID: 2907
			internal const int cmdidHorizSpaceIncrease = 23;

			// Token: 0x04000B5C RID: 2908
			internal const int cmdidHorizSpaceMakeEqual = 24;

			// Token: 0x04000B5D RID: 2909
			internal const int cmdidLockControls = 369;

			// Token: 0x04000B5E RID: 2910
			internal const int cmdidInsertObject = 25;

			// Token: 0x04000B5F RID: 2911
			internal const int cmdidPaste = 26;

			// Token: 0x04000B60 RID: 2912
			internal const int cmdidPrint = 27;

			// Token: 0x04000B61 RID: 2913
			internal const int cmdidProperties = 28;

			// Token: 0x04000B62 RID: 2914
			internal const int cmdidRedo = 29;

			// Token: 0x04000B63 RID: 2915
			internal const int cmdidMultiLevelRedo = 30;

			// Token: 0x04000B64 RID: 2916
			internal const int cmdidSelectAll = 31;

			// Token: 0x04000B65 RID: 2917
			internal const int cmdidSendBackward = 32;

			// Token: 0x04000B66 RID: 2918
			internal const int cmdidSendToBack = 33;

			// Token: 0x04000B67 RID: 2919
			internal const int cmdidShowTable = 34;

			// Token: 0x04000B68 RID: 2920
			internal const int cmdidSizeToControl = 35;

			// Token: 0x04000B69 RID: 2921
			internal const int cmdidSizeToControlHeight = 36;

			// Token: 0x04000B6A RID: 2922
			internal const int cmdidSizeToControlWidth = 37;

			// Token: 0x04000B6B RID: 2923
			internal const int cmdidSizeToFit = 38;

			// Token: 0x04000B6C RID: 2924
			internal const int cmdidSizeToGrid = 39;

			// Token: 0x04000B6D RID: 2925
			internal const int cmdidSnapToGrid = 40;

			// Token: 0x04000B6E RID: 2926
			internal const int cmdidTabOrder = 41;

			// Token: 0x04000B6F RID: 2927
			internal const int cmdidToolbox = 42;

			// Token: 0x04000B70 RID: 2928
			internal const int cmdidUndo = 43;

			// Token: 0x04000B71 RID: 2929
			internal const int cmdidMultiLevelUndo = 44;

			// Token: 0x04000B72 RID: 2930
			internal const int cmdidUngroup = 45;

			// Token: 0x04000B73 RID: 2931
			internal const int cmdidVertSpaceConcatenate = 46;

			// Token: 0x04000B74 RID: 2932
			internal const int cmdidVertSpaceDecrease = 47;

			// Token: 0x04000B75 RID: 2933
			internal const int cmdidVertSpaceIncrease = 48;

			// Token: 0x04000B76 RID: 2934
			internal const int cmdidVertSpaceMakeEqual = 49;

			// Token: 0x04000B77 RID: 2935
			internal const int cmdidZoomPercent = 50;

			// Token: 0x04000B78 RID: 2936
			internal const int cmdidBackColor = 51;

			// Token: 0x04000B79 RID: 2937
			internal const int cmdidBold = 52;

			// Token: 0x04000B7A RID: 2938
			internal const int cmdidBorderColor = 53;

			// Token: 0x04000B7B RID: 2939
			internal const int cmdidBorderDashDot = 54;

			// Token: 0x04000B7C RID: 2940
			internal const int cmdidBorderDashDotDot = 55;

			// Token: 0x04000B7D RID: 2941
			internal const int cmdidBorderDashes = 56;

			// Token: 0x04000B7E RID: 2942
			internal const int cmdidBorderDots = 57;

			// Token: 0x04000B7F RID: 2943
			internal const int cmdidBorderShortDashes = 58;

			// Token: 0x04000B80 RID: 2944
			internal const int cmdidBorderSolid = 59;

			// Token: 0x04000B81 RID: 2945
			internal const int cmdidBorderSparseDots = 60;

			// Token: 0x04000B82 RID: 2946
			internal const int cmdidBorderWidth1 = 61;

			// Token: 0x04000B83 RID: 2947
			internal const int cmdidBorderWidth2 = 62;

			// Token: 0x04000B84 RID: 2948
			internal const int cmdidBorderWidth3 = 63;

			// Token: 0x04000B85 RID: 2949
			internal const int cmdidBorderWidth4 = 64;

			// Token: 0x04000B86 RID: 2950
			internal const int cmdidBorderWidth5 = 65;

			// Token: 0x04000B87 RID: 2951
			internal const int cmdidBorderWidth6 = 66;

			// Token: 0x04000B88 RID: 2952
			internal const int cmdidBorderWidthHairline = 67;

			// Token: 0x04000B89 RID: 2953
			internal const int cmdidFlat = 68;

			// Token: 0x04000B8A RID: 2954
			internal const int cmdidForeColor = 69;

			// Token: 0x04000B8B RID: 2955
			internal const int cmdidItalic = 70;

			// Token: 0x04000B8C RID: 2956
			internal const int cmdidJustifyCenter = 71;

			// Token: 0x04000B8D RID: 2957
			internal const int cmdidJustifyGeneral = 72;

			// Token: 0x04000B8E RID: 2958
			internal const int cmdidJustifyLeft = 73;

			// Token: 0x04000B8F RID: 2959
			internal const int cmdidJustifyRight = 74;

			// Token: 0x04000B90 RID: 2960
			internal const int cmdidRaised = 75;

			// Token: 0x04000B91 RID: 2961
			internal const int cmdidSunken = 76;

			// Token: 0x04000B92 RID: 2962
			internal const int cmdidUnderline = 77;

			// Token: 0x04000B93 RID: 2963
			internal const int cmdidChiseled = 78;

			// Token: 0x04000B94 RID: 2964
			internal const int cmdidEtched = 79;

			// Token: 0x04000B95 RID: 2965
			internal const int cmdidShadowed = 80;

			// Token: 0x04000B96 RID: 2966
			internal const int cmdidCompDebug1 = 81;

			// Token: 0x04000B97 RID: 2967
			internal const int cmdidCompDebug2 = 82;

			// Token: 0x04000B98 RID: 2968
			internal const int cmdidCompDebug3 = 83;

			// Token: 0x04000B99 RID: 2969
			internal const int cmdidCompDebug4 = 84;

			// Token: 0x04000B9A RID: 2970
			internal const int cmdidCompDebug5 = 85;

			// Token: 0x04000B9B RID: 2971
			internal const int cmdidCompDebug6 = 86;

			// Token: 0x04000B9C RID: 2972
			internal const int cmdidCompDebug7 = 87;

			// Token: 0x04000B9D RID: 2973
			internal const int cmdidCompDebug8 = 88;

			// Token: 0x04000B9E RID: 2974
			internal const int cmdidCompDebug9 = 89;

			// Token: 0x04000B9F RID: 2975
			internal const int cmdidCompDebug10 = 90;

			// Token: 0x04000BA0 RID: 2976
			internal const int cmdidCompDebug11 = 91;

			// Token: 0x04000BA1 RID: 2977
			internal const int cmdidCompDebug12 = 92;

			// Token: 0x04000BA2 RID: 2978
			internal const int cmdidCompDebug13 = 93;

			// Token: 0x04000BA3 RID: 2979
			internal const int cmdidCompDebug14 = 94;

			// Token: 0x04000BA4 RID: 2980
			internal const int cmdidCompDebug15 = 95;

			// Token: 0x04000BA5 RID: 2981
			internal const int cmdidExistingSchemaEdit = 96;

			// Token: 0x04000BA6 RID: 2982
			internal const int cmdidFind = 97;

			// Token: 0x04000BA7 RID: 2983
			internal const int cmdidGetZoom = 98;

			// Token: 0x04000BA8 RID: 2984
			internal const int cmdidQueryOpenDesign = 99;

			// Token: 0x04000BA9 RID: 2985
			internal const int cmdidQueryOpenNew = 100;

			// Token: 0x04000BAA RID: 2986
			internal const int cmdidSingleTableDesign = 101;

			// Token: 0x04000BAB RID: 2987
			internal const int cmdidSingleTableNew = 102;

			// Token: 0x04000BAC RID: 2988
			internal const int cmdidShowGrid = 103;

			// Token: 0x04000BAD RID: 2989
			internal const int cmdidNewTable = 104;

			// Token: 0x04000BAE RID: 2990
			internal const int cmdidCollapsedView = 105;

			// Token: 0x04000BAF RID: 2991
			internal const int cmdidFieldView = 106;

			// Token: 0x04000BB0 RID: 2992
			internal const int cmdidVerifySQL = 107;

			// Token: 0x04000BB1 RID: 2993
			internal const int cmdidHideTable = 108;

			// Token: 0x04000BB2 RID: 2994
			internal const int cmdidPrimaryKey = 109;

			// Token: 0x04000BB3 RID: 2995
			internal const int cmdidSave = 110;

			// Token: 0x04000BB4 RID: 2996
			internal const int cmdidSaveAs = 111;

			// Token: 0x04000BB5 RID: 2997
			internal const int cmdidSortAscending = 112;

			// Token: 0x04000BB6 RID: 2998
			internal const int cmdidSortDescending = 113;

			// Token: 0x04000BB7 RID: 2999
			internal const int cmdidAppendQuery = 114;

			// Token: 0x04000BB8 RID: 3000
			internal const int cmdidCrosstabQuery = 115;

			// Token: 0x04000BB9 RID: 3001
			internal const int cmdidDeleteQuery = 116;

			// Token: 0x04000BBA RID: 3002
			internal const int cmdidMakeTableQuery = 117;

			// Token: 0x04000BBB RID: 3003
			internal const int cmdidSelectQuery = 118;

			// Token: 0x04000BBC RID: 3004
			internal const int cmdidUpdateQuery = 119;

			// Token: 0x04000BBD RID: 3005
			internal const int cmdidParameters = 120;

			// Token: 0x04000BBE RID: 3006
			internal const int cmdidTotals = 121;

			// Token: 0x04000BBF RID: 3007
			internal const int cmdidViewCollapsed = 122;

			// Token: 0x04000BC0 RID: 3008
			internal const int cmdidViewFieldList = 123;

			// Token: 0x04000BC1 RID: 3009
			internal const int cmdidViewKeys = 124;

			// Token: 0x04000BC2 RID: 3010
			internal const int cmdidViewGrid = 125;

			// Token: 0x04000BC3 RID: 3011
			internal const int cmdidInnerJoin = 126;

			// Token: 0x04000BC4 RID: 3012
			internal const int cmdidRightOuterJoin = 127;

			// Token: 0x04000BC5 RID: 3013
			internal const int cmdidLeftOuterJoin = 128;

			// Token: 0x04000BC6 RID: 3014
			internal const int cmdidFullOuterJoin = 129;

			// Token: 0x04000BC7 RID: 3015
			internal const int cmdidUnionJoin = 130;

			// Token: 0x04000BC8 RID: 3016
			internal const int cmdidShowSQLPane = 131;

			// Token: 0x04000BC9 RID: 3017
			internal const int cmdidShowGraphicalPane = 132;

			// Token: 0x04000BCA RID: 3018
			internal const int cmdidShowDataPane = 133;

			// Token: 0x04000BCB RID: 3019
			internal const int cmdidShowQBEPane = 134;

			// Token: 0x04000BCC RID: 3020
			internal const int cmdidSelectAllFields = 135;

			// Token: 0x04000BCD RID: 3021
			internal const int cmdidOLEObjectMenuButton = 136;

			// Token: 0x04000BCE RID: 3022
			internal const int cmdidObjectVerbList0 = 137;

			// Token: 0x04000BCF RID: 3023
			internal const int cmdidObjectVerbList1 = 138;

			// Token: 0x04000BD0 RID: 3024
			internal const int cmdidObjectVerbList2 = 139;

			// Token: 0x04000BD1 RID: 3025
			internal const int cmdidObjectVerbList3 = 140;

			// Token: 0x04000BD2 RID: 3026
			internal const int cmdidObjectVerbList4 = 141;

			// Token: 0x04000BD3 RID: 3027
			internal const int cmdidObjectVerbList5 = 142;

			// Token: 0x04000BD4 RID: 3028
			internal const int cmdidObjectVerbList6 = 143;

			// Token: 0x04000BD5 RID: 3029
			internal const int cmdidObjectVerbList7 = 144;

			// Token: 0x04000BD6 RID: 3030
			internal const int cmdidObjectVerbList8 = 145;

			// Token: 0x04000BD7 RID: 3031
			internal const int cmdidObjectVerbList9 = 146;

			// Token: 0x04000BD8 RID: 3032
			internal const int cmdidConvertObject = 147;

			// Token: 0x04000BD9 RID: 3033
			internal const int cmdidCustomControl = 148;

			// Token: 0x04000BDA RID: 3034
			internal const int cmdidCustomizeItem = 149;

			// Token: 0x04000BDB RID: 3035
			internal const int cmdidRename = 150;

			// Token: 0x04000BDC RID: 3036
			internal const int cmdidImport = 151;

			// Token: 0x04000BDD RID: 3037
			internal const int cmdidNewPage = 152;

			// Token: 0x04000BDE RID: 3038
			internal const int cmdidMove = 153;

			// Token: 0x04000BDF RID: 3039
			internal const int cmdidCancel = 154;

			// Token: 0x04000BE0 RID: 3040
			internal const int cmdidFont = 155;

			// Token: 0x04000BE1 RID: 3041
			internal const int cmdidExpandLinks = 156;

			// Token: 0x04000BE2 RID: 3042
			internal const int cmdidExpandImages = 157;

			// Token: 0x04000BE3 RID: 3043
			internal const int cmdidExpandPages = 158;

			// Token: 0x04000BE4 RID: 3044
			internal const int cmdidRefocusDiagram = 159;

			// Token: 0x04000BE5 RID: 3045
			internal const int cmdidTransitiveClosure = 160;

			// Token: 0x04000BE6 RID: 3046
			internal const int cmdidCenterDiagram = 161;

			// Token: 0x04000BE7 RID: 3047
			internal const int cmdidZoomIn = 162;

			// Token: 0x04000BE8 RID: 3048
			internal const int cmdidZoomOut = 163;

			// Token: 0x04000BE9 RID: 3049
			internal const int cmdidRemoveFilter = 164;

			// Token: 0x04000BEA RID: 3050
			internal const int cmdidHidePane = 165;

			// Token: 0x04000BEB RID: 3051
			internal const int cmdidDeleteTable = 166;

			// Token: 0x04000BEC RID: 3052
			internal const int cmdidDeleteRelationship = 167;

			// Token: 0x04000BED RID: 3053
			internal const int cmdidRemove = 168;

			// Token: 0x04000BEE RID: 3054
			internal const int cmdidJoinLeftAll = 169;

			// Token: 0x04000BEF RID: 3055
			internal const int cmdidJoinRightAll = 170;

			// Token: 0x04000BF0 RID: 3056
			internal const int cmdidAddToOutput = 171;

			// Token: 0x04000BF1 RID: 3057
			internal const int cmdidOtherQuery = 172;

			// Token: 0x04000BF2 RID: 3058
			internal const int cmdidGenerateChangeScript = 173;

			// Token: 0x04000BF3 RID: 3059
			internal const int cmdidSaveSelection = 174;

			// Token: 0x04000BF4 RID: 3060
			internal const int cmdidAutojoinCurrent = 175;

			// Token: 0x04000BF5 RID: 3061
			internal const int cmdidAutojoinAlways = 176;

			// Token: 0x04000BF6 RID: 3062
			internal const int cmdidEditPage = 177;

			// Token: 0x04000BF7 RID: 3063
			internal const int cmdidViewLinks = 178;

			// Token: 0x04000BF8 RID: 3064
			internal const int cmdidStop = 179;

			// Token: 0x04000BF9 RID: 3065
			internal const int cmdidPause = 180;

			// Token: 0x04000BFA RID: 3066
			internal const int cmdidResume = 181;

			// Token: 0x04000BFB RID: 3067
			internal const int cmdidFilterDiagram = 182;

			// Token: 0x04000BFC RID: 3068
			internal const int cmdidShowAllObjects = 183;

			// Token: 0x04000BFD RID: 3069
			internal const int cmdidShowApplications = 184;

			// Token: 0x04000BFE RID: 3070
			internal const int cmdidShowOtherObjects = 185;

			// Token: 0x04000BFF RID: 3071
			internal const int cmdidShowPrimRelationships = 186;

			// Token: 0x04000C00 RID: 3072
			internal const int cmdidExpand = 187;

			// Token: 0x04000C01 RID: 3073
			internal const int cmdidCollapse = 188;

			// Token: 0x04000C02 RID: 3074
			internal const int cmdidRefresh = 189;

			// Token: 0x04000C03 RID: 3075
			internal const int cmdidLayout = 190;

			// Token: 0x04000C04 RID: 3076
			internal const int cmdidShowResources = 191;

			// Token: 0x04000C05 RID: 3077
			internal const int cmdidInsertHTMLWizard = 192;

			// Token: 0x04000C06 RID: 3078
			internal const int cmdidShowDownloads = 193;

			// Token: 0x04000C07 RID: 3079
			internal const int cmdidShowExternals = 194;

			// Token: 0x04000C08 RID: 3080
			internal const int cmdidShowInBoundLinks = 195;

			// Token: 0x04000C09 RID: 3081
			internal const int cmdidShowOutBoundLinks = 196;

			// Token: 0x04000C0A RID: 3082
			internal const int cmdidShowInAndOutBoundLinks = 197;

			// Token: 0x04000C0B RID: 3083
			internal const int cmdidPreview = 198;

			// Token: 0x04000C0C RID: 3084
			internal const int cmdidOpen = 261;

			// Token: 0x04000C0D RID: 3085
			internal const int cmdidOpenWith = 199;

			// Token: 0x04000C0E RID: 3086
			internal const int cmdidShowPages = 200;

			// Token: 0x04000C0F RID: 3087
			internal const int cmdidRunQuery = 201;

			// Token: 0x04000C10 RID: 3088
			internal const int cmdidClearQuery = 202;

			// Token: 0x04000C11 RID: 3089
			internal const int cmdidRecordFirst = 203;

			// Token: 0x04000C12 RID: 3090
			internal const int cmdidRecordLast = 204;

			// Token: 0x04000C13 RID: 3091
			internal const int cmdidRecordNext = 205;

			// Token: 0x04000C14 RID: 3092
			internal const int cmdidRecordPrevious = 206;

			// Token: 0x04000C15 RID: 3093
			internal const int cmdidRecordGoto = 207;

			// Token: 0x04000C16 RID: 3094
			internal const int cmdidRecordNew = 208;

			// Token: 0x04000C17 RID: 3095
			internal const int cmdidInsertNewMenu = 209;

			// Token: 0x04000C18 RID: 3096
			internal const int cmdidInsertSeparator = 210;

			// Token: 0x04000C19 RID: 3097
			internal const int cmdidEditMenuNames = 211;

			// Token: 0x04000C1A RID: 3098
			internal const int cmdidDebugExplorer = 212;

			// Token: 0x04000C1B RID: 3099
			internal const int cmdidDebugProcesses = 213;

			// Token: 0x04000C1C RID: 3100
			internal const int cmdidViewThreadsWindow = 214;

			// Token: 0x04000C1D RID: 3101
			internal const int cmdidWindowUIList = 215;

			// Token: 0x04000C1E RID: 3102
			internal const int cmdidNewProject = 216;

			// Token: 0x04000C1F RID: 3103
			internal const int cmdidOpenProject = 217;

			// Token: 0x04000C20 RID: 3104
			internal const int cmdidOpenSolution = 218;

			// Token: 0x04000C21 RID: 3105
			internal const int cmdidCloseSolution = 219;

			// Token: 0x04000C22 RID: 3106
			internal const int cmdidFileNew = 221;

			// Token: 0x04000C23 RID: 3107
			internal const int cmdidFileOpen = 222;

			// Token: 0x04000C24 RID: 3108
			internal const int cmdidFileClose = 223;

			// Token: 0x04000C25 RID: 3109
			internal const int cmdidSaveSolution = 224;

			// Token: 0x04000C26 RID: 3110
			internal const int cmdidSaveSolutionAs = 225;

			// Token: 0x04000C27 RID: 3111
			internal const int cmdidSaveProjectItemAs = 226;

			// Token: 0x04000C28 RID: 3112
			internal const int cmdidPageSetup = 227;

			// Token: 0x04000C29 RID: 3113
			internal const int cmdidPrintPreview = 228;

			// Token: 0x04000C2A RID: 3114
			internal const int cmdidExit = 229;

			// Token: 0x04000C2B RID: 3115
			internal const int cmdidReplace = 230;

			// Token: 0x04000C2C RID: 3116
			internal const int cmdidGoto = 231;

			// Token: 0x04000C2D RID: 3117
			internal const int cmdidPropertyPages = 232;

			// Token: 0x04000C2E RID: 3118
			internal const int cmdidFullScreen = 233;

			// Token: 0x04000C2F RID: 3119
			internal const int cmdidProjectExplorer = 234;

			// Token: 0x04000C30 RID: 3120
			internal const int cmdidPropertiesWindow = 235;

			// Token: 0x04000C31 RID: 3121
			internal const int cmdidTaskListWindow = 236;

			// Token: 0x04000C32 RID: 3122
			internal const int cmdidOutputWindow = 237;

			// Token: 0x04000C33 RID: 3123
			internal const int cmdidObjectBrowser = 238;

			// Token: 0x04000C34 RID: 3124
			internal const int cmdidDocOutlineWindow = 239;

			// Token: 0x04000C35 RID: 3125
			internal const int cmdidImmediateWindow = 240;

			// Token: 0x04000C36 RID: 3126
			internal const int cmdidWatchWindow = 241;

			// Token: 0x04000C37 RID: 3127
			internal const int cmdidLocalsWindow = 242;

			// Token: 0x04000C38 RID: 3128
			internal const int cmdidCallStack = 243;

			// Token: 0x04000C39 RID: 3129
			internal const int cmdidAutosWindow = 747;

			// Token: 0x04000C3A RID: 3130
			internal const int cmdidThisWindow = 748;

			// Token: 0x04000C3B RID: 3131
			internal const int cmdidAddNewItem = 220;

			// Token: 0x04000C3C RID: 3132
			internal const int cmdidAddExistingItem = 244;

			// Token: 0x04000C3D RID: 3133
			internal const int cmdidNewFolder = 245;

			// Token: 0x04000C3E RID: 3134
			internal const int cmdidSetStartupProject = 246;

			// Token: 0x04000C3F RID: 3135
			internal const int cmdidProjectSettings = 247;

			// Token: 0x04000C40 RID: 3136
			internal const int cmdidProjectReferences = 367;

			// Token: 0x04000C41 RID: 3137
			internal const int cmdidStepInto = 248;

			// Token: 0x04000C42 RID: 3138
			internal const int cmdidStepOver = 249;

			// Token: 0x04000C43 RID: 3139
			internal const int cmdidStepOut = 250;

			// Token: 0x04000C44 RID: 3140
			internal const int cmdidRunToCursor = 251;

			// Token: 0x04000C45 RID: 3141
			internal const int cmdidAddWatch = 252;

			// Token: 0x04000C46 RID: 3142
			internal const int cmdidEditWatch = 253;

			// Token: 0x04000C47 RID: 3143
			internal const int cmdidQuickWatch = 254;

			// Token: 0x04000C48 RID: 3144
			internal const int cmdidToggleBreakpoint = 255;

			// Token: 0x04000C49 RID: 3145
			internal const int cmdidClearBreakpoints = 256;

			// Token: 0x04000C4A RID: 3146
			internal const int cmdidShowBreakpoints = 257;

			// Token: 0x04000C4B RID: 3147
			internal const int cmdidSetNextStatement = 258;

			// Token: 0x04000C4C RID: 3148
			internal const int cmdidShowNextStatement = 259;

			// Token: 0x04000C4D RID: 3149
			internal const int cmdidEditBreakpoint = 260;

			// Token: 0x04000C4E RID: 3150
			internal const int cmdidDetachDebugger = 262;

			// Token: 0x04000C4F RID: 3151
			internal const int cmdidCustomizeKeyboard = 263;

			// Token: 0x04000C50 RID: 3152
			internal const int cmdidToolsOptions = 264;

			// Token: 0x04000C51 RID: 3153
			internal const int cmdidNewWindow = 265;

			// Token: 0x04000C52 RID: 3154
			internal const int cmdidSplit = 266;

			// Token: 0x04000C53 RID: 3155
			internal const int cmdidCascade = 267;

			// Token: 0x04000C54 RID: 3156
			internal const int cmdidTileHorz = 268;

			// Token: 0x04000C55 RID: 3157
			internal const int cmdidTileVert = 269;

			// Token: 0x04000C56 RID: 3158
			internal const int cmdidTechSupport = 270;

			// Token: 0x04000C57 RID: 3159
			internal const int cmdidAbout = 271;

			// Token: 0x04000C58 RID: 3160
			internal const int cmdidDebugOptions = 272;

			// Token: 0x04000C59 RID: 3161
			internal const int cmdidDeleteWatch = 274;

			// Token: 0x04000C5A RID: 3162
			internal const int cmdidCollapseWatch = 275;

			// Token: 0x04000C5B RID: 3163
			internal const int cmdidPbrsToggleStatus = 282;

			// Token: 0x04000C5C RID: 3164
			internal const int cmdidPropbrsHide = 283;

			// Token: 0x04000C5D RID: 3165
			internal const int cmdidDockingView = 284;

			// Token: 0x04000C5E RID: 3166
			internal const int cmdidHideActivePane = 285;

			// Token: 0x04000C5F RID: 3167
			internal const int cmdidPaneNextTab = 286;

			// Token: 0x04000C60 RID: 3168
			internal const int cmdidPanePrevTab = 287;

			// Token: 0x04000C61 RID: 3169
			internal const int cmdidPaneCloseToolWindow = 288;

			// Token: 0x04000C62 RID: 3170
			internal const int cmdidPaneActivateDocWindow = 289;

			// Token: 0x04000C63 RID: 3171
			internal const int cmdidDockingViewFloater = 291;

			// Token: 0x04000C64 RID: 3172
			internal const int cmdidAutoHideWindow = 292;

			// Token: 0x04000C65 RID: 3173
			internal const int cmdidMoveToDropdownBar = 293;

			// Token: 0x04000C66 RID: 3174
			internal const int cmdidFindCmd = 294;

			// Token: 0x04000C67 RID: 3175
			internal const int cmdidStart = 295;

			// Token: 0x04000C68 RID: 3176
			internal const int cmdidRestart = 296;

			// Token: 0x04000C69 RID: 3177
			internal const int cmdidAddinManager = 297;

			// Token: 0x04000C6A RID: 3178
			internal const int cmdidMultiLevelUndoList = 298;

			// Token: 0x04000C6B RID: 3179
			internal const int cmdidMultiLevelRedoList = 299;

			// Token: 0x04000C6C RID: 3180
			internal const int cmdidToolboxAddTab = 300;

			// Token: 0x04000C6D RID: 3181
			internal const int cmdidToolboxDeleteTab = 301;

			// Token: 0x04000C6E RID: 3182
			internal const int cmdidToolboxRenameTab = 302;

			// Token: 0x04000C6F RID: 3183
			internal const int cmdidToolboxTabMoveUp = 303;

			// Token: 0x04000C70 RID: 3184
			internal const int cmdidToolboxTabMoveDown = 304;

			// Token: 0x04000C71 RID: 3185
			internal const int cmdidToolboxRenameItem = 305;

			// Token: 0x04000C72 RID: 3186
			internal const int cmdidToolboxListView = 306;

			// Token: 0x04000C73 RID: 3187
			internal const int cmdidWindowUIGetList = 308;

			// Token: 0x04000C74 RID: 3188
			internal const int cmdidInsertValuesQuery = 309;

			// Token: 0x04000C75 RID: 3189
			internal const int cmdidShowProperties = 310;

			// Token: 0x04000C76 RID: 3190
			internal const int cmdidThreadSuspend = 311;

			// Token: 0x04000C77 RID: 3191
			internal const int cmdidThreadResume = 312;

			// Token: 0x04000C78 RID: 3192
			internal const int cmdidThreadSetFocus = 313;

			// Token: 0x04000C79 RID: 3193
			internal const int cmdidDisplayRadix = 314;

			// Token: 0x04000C7A RID: 3194
			internal const int cmdidOpenProjectItem = 315;

			// Token: 0x04000C7B RID: 3195
			internal const int cmdidPaneNextPane = 316;

			// Token: 0x04000C7C RID: 3196
			internal const int cmdidPanePrevPane = 317;

			// Token: 0x04000C7D RID: 3197
			internal const int cmdidClearPane = 318;

			// Token: 0x04000C7E RID: 3198
			internal const int cmdidGotoErrorTag = 319;

			// Token: 0x04000C7F RID: 3199
			internal const int cmdidTaskListSortByCategory = 320;

			// Token: 0x04000C80 RID: 3200
			internal const int cmdidTaskListSortByFileLine = 321;

			// Token: 0x04000C81 RID: 3201
			internal const int cmdidTaskListSortByPriority = 322;

			// Token: 0x04000C82 RID: 3202
			internal const int cmdidTaskListSortByDefaultSort = 323;

			// Token: 0x04000C83 RID: 3203
			internal const int cmdidTaskListFilterByNothing = 325;

			// Token: 0x04000C84 RID: 3204
			internal const int cmdidTaskListFilterByCategoryCodeSense = 326;

			// Token: 0x04000C85 RID: 3205
			internal const int cmdidTaskListFilterByCategoryCompiler = 327;

			// Token: 0x04000C86 RID: 3206
			internal const int cmdidTaskListFilterByCategoryComment = 328;

			// Token: 0x04000C87 RID: 3207
			internal const int cmdidToolboxAddItem = 329;

			// Token: 0x04000C88 RID: 3208
			internal const int cmdidToolboxReset = 330;

			// Token: 0x04000C89 RID: 3209
			internal const int cmdidSaveProjectItem = 331;

			// Token: 0x04000C8A RID: 3210
			internal const int cmdidViewForm = 332;

			// Token: 0x04000C8B RID: 3211
			internal const int cmdidViewCode = 333;

			// Token: 0x04000C8C RID: 3212
			internal const int cmdidPreviewInBrowser = 334;

			// Token: 0x04000C8D RID: 3213
			internal const int cmdidBrowseWith = 336;

			// Token: 0x04000C8E RID: 3214
			internal const int cmdidSearchSetCombo = 307;

			// Token: 0x04000C8F RID: 3215
			internal const int cmdidSearchCombo = 337;

			// Token: 0x04000C90 RID: 3216
			internal const int cmdidEditLabel = 338;

			// Token: 0x04000C91 RID: 3217
			internal const int cmdidExceptions = 339;

			// Token: 0x04000C92 RID: 3218
			internal const int cmdidToggleSelMode = 341;

			// Token: 0x04000C93 RID: 3219
			internal const int cmdidToggleInsMode = 342;

			// Token: 0x04000C94 RID: 3220
			internal const int cmdidLoadUnloadedProject = 343;

			// Token: 0x04000C95 RID: 3221
			internal const int cmdidUnloadLoadedProject = 344;

			// Token: 0x04000C96 RID: 3222
			internal const int cmdidElasticColumn = 345;

			// Token: 0x04000C97 RID: 3223
			internal const int cmdidHideColumn = 346;

			// Token: 0x04000C98 RID: 3224
			internal const int cmdidTaskListPreviousView = 347;

			// Token: 0x04000C99 RID: 3225
			internal const int cmdidZoomDialog = 348;

			// Token: 0x04000C9A RID: 3226
			internal const int cmdidFindNew = 349;

			// Token: 0x04000C9B RID: 3227
			internal const int cmdidFindMatchCase = 350;

			// Token: 0x04000C9C RID: 3228
			internal const int cmdidFindWholeWord = 351;

			// Token: 0x04000C9D RID: 3229
			internal const int cmdidFindSimplePattern = 276;

			// Token: 0x04000C9E RID: 3230
			internal const int cmdidFindRegularExpression = 352;

			// Token: 0x04000C9F RID: 3231
			internal const int cmdidFindBackwards = 353;

			// Token: 0x04000CA0 RID: 3232
			internal const int cmdidFindInSelection = 354;

			// Token: 0x04000CA1 RID: 3233
			internal const int cmdidFindStop = 355;

			// Token: 0x04000CA2 RID: 3234
			internal const int cmdidFindHelp = 356;

			// Token: 0x04000CA3 RID: 3235
			internal const int cmdidFindInFiles = 277;

			// Token: 0x04000CA4 RID: 3236
			internal const int cmdidReplaceInFiles = 278;

			// Token: 0x04000CA5 RID: 3237
			internal const int cmdidNextLocation = 279;

			// Token: 0x04000CA6 RID: 3238
			internal const int cmdidPreviousLocation = 280;

			// Token: 0x04000CA7 RID: 3239
			internal const int cmdidTaskListNextError = 357;

			// Token: 0x04000CA8 RID: 3240
			internal const int cmdidTaskListPrevError = 358;

			// Token: 0x04000CA9 RID: 3241
			internal const int cmdidTaskListFilterByCategoryUser = 359;

			// Token: 0x04000CAA RID: 3242
			internal const int cmdidTaskListFilterByCategoryShortcut = 360;

			// Token: 0x04000CAB RID: 3243
			internal const int cmdidTaskListFilterByCategoryHTML = 361;

			// Token: 0x04000CAC RID: 3244
			internal const int cmdidTaskListFilterByCurrentFile = 362;

			// Token: 0x04000CAD RID: 3245
			internal const int cmdidTaskListFilterByChecked = 363;

			// Token: 0x04000CAE RID: 3246
			internal const int cmdidTaskListFilterByUnchecked = 364;

			// Token: 0x04000CAF RID: 3247
			internal const int cmdidTaskListSortByDescription = 365;

			// Token: 0x04000CB0 RID: 3248
			internal const int cmdidTaskListSortByChecked = 366;

			// Token: 0x04000CB1 RID: 3249
			internal const int cmdidStartNoDebug = 368;

			// Token: 0x04000CB2 RID: 3250
			internal const int cmdidFindNext = 370;

			// Token: 0x04000CB3 RID: 3251
			internal const int cmdidFindPrev = 371;

			// Token: 0x04000CB4 RID: 3252
			internal const int cmdidFindSelectedNext = 372;

			// Token: 0x04000CB5 RID: 3253
			internal const int cmdidFindSelectedPrev = 373;

			// Token: 0x04000CB6 RID: 3254
			internal const int cmdidSearchGetList = 374;

			// Token: 0x04000CB7 RID: 3255
			internal const int cmdidInsertBreakpoint = 375;

			// Token: 0x04000CB8 RID: 3256
			internal const int cmdidEnableBreakpoint = 376;

			// Token: 0x04000CB9 RID: 3257
			internal const int cmdidF1Help = 377;

			// Token: 0x04000CBA RID: 3258
			internal const int cmdidPropSheetOrProperties = 397;

			// Token: 0x04000CBB RID: 3259
			internal const int cmdidTshellStep = 398;

			// Token: 0x04000CBC RID: 3260
			internal const int cmdidTshellRun = 399;

			// Token: 0x04000CBD RID: 3261
			internal const int cmdidMarkerCmd0 = 400;

			// Token: 0x04000CBE RID: 3262
			internal const int cmdidMarkerCmd1 = 401;

			// Token: 0x04000CBF RID: 3263
			internal const int cmdidMarkerCmd2 = 402;

			// Token: 0x04000CC0 RID: 3264
			internal const int cmdidMarkerCmd3 = 403;

			// Token: 0x04000CC1 RID: 3265
			internal const int cmdidMarkerCmd4 = 404;

			// Token: 0x04000CC2 RID: 3266
			internal const int cmdidMarkerCmd5 = 405;

			// Token: 0x04000CC3 RID: 3267
			internal const int cmdidMarkerCmd6 = 406;

			// Token: 0x04000CC4 RID: 3268
			internal const int cmdidMarkerCmd7 = 407;

			// Token: 0x04000CC5 RID: 3269
			internal const int cmdidMarkerCmd8 = 408;

			// Token: 0x04000CC6 RID: 3270
			internal const int cmdidMarkerCmd9 = 409;

			// Token: 0x04000CC7 RID: 3271
			internal const int cmdidMarkerLast = 409;

			// Token: 0x04000CC8 RID: 3272
			internal const int cmdidMarkerEnd = 410;

			// Token: 0x04000CC9 RID: 3273
			internal const int cmdidReloadProject = 412;

			// Token: 0x04000CCA RID: 3274
			internal const int cmdidUnloadProject = 413;

			// Token: 0x04000CCB RID: 3275
			internal const int cmdidDetachAttachOutline = 420;

			// Token: 0x04000CCC RID: 3276
			internal const int cmdidShowHideOutline = 421;

			// Token: 0x04000CCD RID: 3277
			internal const int cmdidSyncOutline = 422;

			// Token: 0x04000CCE RID: 3278
			internal const int cmdidRunToCallstCursor = 423;

			// Token: 0x04000CCF RID: 3279
			internal const int cmdidNoCmdsAvailable = 424;

			// Token: 0x04000CD0 RID: 3280
			internal const int cmdidContextWindow = 427;

			// Token: 0x04000CD1 RID: 3281
			internal const int cmdidAlias = 428;

			// Token: 0x04000CD2 RID: 3282
			internal const int cmdidGotoCommandLine = 429;

			// Token: 0x04000CD3 RID: 3283
			internal const int cmdidEvaluateExpression = 430;

			// Token: 0x04000CD4 RID: 3284
			internal const int cmdidImmediateMode = 431;

			// Token: 0x04000CD5 RID: 3285
			internal const int cmdidEvaluateStatement = 432;

			// Token: 0x04000CD6 RID: 3286
			internal const int cmdidFindResultWindow1 = 433;

			// Token: 0x04000CD7 RID: 3287
			internal const int cmdidFindResultWindow2 = 434;

			// Token: 0x04000CD8 RID: 3288
			internal const int cmdidWindow1 = 570;

			// Token: 0x04000CD9 RID: 3289
			internal const int cmdidWindow2 = 571;

			// Token: 0x04000CDA RID: 3290
			internal const int cmdidWindow3 = 572;

			// Token: 0x04000CDB RID: 3291
			internal const int cmdidWindow4 = 573;

			// Token: 0x04000CDC RID: 3292
			internal const int cmdidWindow5 = 574;

			// Token: 0x04000CDD RID: 3293
			internal const int cmdidWindow6 = 575;

			// Token: 0x04000CDE RID: 3294
			internal const int cmdidWindow7 = 576;

			// Token: 0x04000CDF RID: 3295
			internal const int cmdidWindow8 = 577;

			// Token: 0x04000CE0 RID: 3296
			internal const int cmdidWindow9 = 578;

			// Token: 0x04000CE1 RID: 3297
			internal const int cmdidWindow10 = 579;

			// Token: 0x04000CE2 RID: 3298
			internal const int cmdidWindow11 = 580;

			// Token: 0x04000CE3 RID: 3299
			internal const int cmdidWindow12 = 581;

			// Token: 0x04000CE4 RID: 3300
			internal const int cmdidWindow13 = 582;

			// Token: 0x04000CE5 RID: 3301
			internal const int cmdidWindow14 = 583;

			// Token: 0x04000CE6 RID: 3302
			internal const int cmdidWindow15 = 584;

			// Token: 0x04000CE7 RID: 3303
			internal const int cmdidWindow16 = 585;

			// Token: 0x04000CE8 RID: 3304
			internal const int cmdidWindow17 = 586;

			// Token: 0x04000CE9 RID: 3305
			internal const int cmdidWindow18 = 587;

			// Token: 0x04000CEA RID: 3306
			internal const int cmdidWindow19 = 588;

			// Token: 0x04000CEB RID: 3307
			internal const int cmdidWindow20 = 589;

			// Token: 0x04000CEC RID: 3308
			internal const int cmdidWindow21 = 590;

			// Token: 0x04000CED RID: 3309
			internal const int cmdidWindow22 = 591;

			// Token: 0x04000CEE RID: 3310
			internal const int cmdidWindow23 = 592;

			// Token: 0x04000CEF RID: 3311
			internal const int cmdidWindow24 = 593;

			// Token: 0x04000CF0 RID: 3312
			internal const int cmdidWindow25 = 594;

			// Token: 0x04000CF1 RID: 3313
			internal const int cmdidMoreWindows = 595;

			// Token: 0x04000CF2 RID: 3314
			internal const int cmdidTaskListTaskHelp = 598;

			// Token: 0x04000CF3 RID: 3315
			internal const int cmdidClassView = 599;

			// Token: 0x04000CF4 RID: 3316
			internal const int cmdidMRUProj1 = 600;

			// Token: 0x04000CF5 RID: 3317
			internal const int cmdidMRUProj2 = 601;

			// Token: 0x04000CF6 RID: 3318
			internal const int cmdidMRUProj3 = 602;

			// Token: 0x04000CF7 RID: 3319
			internal const int cmdidMRUProj4 = 603;

			// Token: 0x04000CF8 RID: 3320
			internal const int cmdidMRUProj5 = 604;

			// Token: 0x04000CF9 RID: 3321
			internal const int cmdidMRUProj6 = 605;

			// Token: 0x04000CFA RID: 3322
			internal const int cmdidMRUProj7 = 606;

			// Token: 0x04000CFB RID: 3323
			internal const int cmdidMRUProj8 = 607;

			// Token: 0x04000CFC RID: 3324
			internal const int cmdidMRUProj9 = 608;

			// Token: 0x04000CFD RID: 3325
			internal const int cmdidMRUProj10 = 609;

			// Token: 0x04000CFE RID: 3326
			internal const int cmdidMRUProj11 = 610;

			// Token: 0x04000CFF RID: 3327
			internal const int cmdidMRUProj12 = 611;

			// Token: 0x04000D00 RID: 3328
			internal const int cmdidMRUProj13 = 612;

			// Token: 0x04000D01 RID: 3329
			internal const int cmdidMRUProj14 = 613;

			// Token: 0x04000D02 RID: 3330
			internal const int cmdidMRUProj15 = 614;

			// Token: 0x04000D03 RID: 3331
			internal const int cmdidMRUProj16 = 615;

			// Token: 0x04000D04 RID: 3332
			internal const int cmdidMRUProj17 = 616;

			// Token: 0x04000D05 RID: 3333
			internal const int cmdidMRUProj18 = 617;

			// Token: 0x04000D06 RID: 3334
			internal const int cmdidMRUProj19 = 618;

			// Token: 0x04000D07 RID: 3335
			internal const int cmdidMRUProj20 = 619;

			// Token: 0x04000D08 RID: 3336
			internal const int cmdidMRUProj21 = 620;

			// Token: 0x04000D09 RID: 3337
			internal const int cmdidMRUProj22 = 621;

			// Token: 0x04000D0A RID: 3338
			internal const int cmdidMRUProj23 = 622;

			// Token: 0x04000D0B RID: 3339
			internal const int cmdidMRUProj24 = 623;

			// Token: 0x04000D0C RID: 3340
			internal const int cmdidMRUProj25 = 624;

			// Token: 0x04000D0D RID: 3341
			internal const int cmdidSplitNext = 625;

			// Token: 0x04000D0E RID: 3342
			internal const int cmdidSplitPrev = 626;

			// Token: 0x04000D0F RID: 3343
			internal const int cmdidCloseAllDocuments = 627;

			// Token: 0x04000D10 RID: 3344
			internal const int cmdidNextDocument = 628;

			// Token: 0x04000D11 RID: 3345
			internal const int cmdidPrevDocument = 629;

			// Token: 0x04000D12 RID: 3346
			internal const int cmdidTool1 = 630;

			// Token: 0x04000D13 RID: 3347
			internal const int cmdidTool2 = 631;

			// Token: 0x04000D14 RID: 3348
			internal const int cmdidTool3 = 632;

			// Token: 0x04000D15 RID: 3349
			internal const int cmdidTool4 = 633;

			// Token: 0x04000D16 RID: 3350
			internal const int cmdidTool5 = 634;

			// Token: 0x04000D17 RID: 3351
			internal const int cmdidTool6 = 635;

			// Token: 0x04000D18 RID: 3352
			internal const int cmdidTool7 = 636;

			// Token: 0x04000D19 RID: 3353
			internal const int cmdidTool8 = 637;

			// Token: 0x04000D1A RID: 3354
			internal const int cmdidTool9 = 638;

			// Token: 0x04000D1B RID: 3355
			internal const int cmdidTool10 = 639;

			// Token: 0x04000D1C RID: 3356
			internal const int cmdidTool11 = 640;

			// Token: 0x04000D1D RID: 3357
			internal const int cmdidTool12 = 641;

			// Token: 0x04000D1E RID: 3358
			internal const int cmdidTool13 = 642;

			// Token: 0x04000D1F RID: 3359
			internal const int cmdidTool14 = 643;

			// Token: 0x04000D20 RID: 3360
			internal const int cmdidTool15 = 644;

			// Token: 0x04000D21 RID: 3361
			internal const int cmdidTool16 = 645;

			// Token: 0x04000D22 RID: 3362
			internal const int cmdidTool17 = 646;

			// Token: 0x04000D23 RID: 3363
			internal const int cmdidTool18 = 647;

			// Token: 0x04000D24 RID: 3364
			internal const int cmdidTool19 = 648;

			// Token: 0x04000D25 RID: 3365
			internal const int cmdidTool20 = 649;

			// Token: 0x04000D26 RID: 3366
			internal const int cmdidTool21 = 650;

			// Token: 0x04000D27 RID: 3367
			internal const int cmdidTool22 = 651;

			// Token: 0x04000D28 RID: 3368
			internal const int cmdidTool23 = 652;

			// Token: 0x04000D29 RID: 3369
			internal const int cmdidTool24 = 653;

			// Token: 0x04000D2A RID: 3370
			internal const int cmdidExternalCommands = 654;

			// Token: 0x04000D2B RID: 3371
			internal const int cmdidPasteNextTBXCBItem = 655;

			// Token: 0x04000D2C RID: 3372
			internal const int cmdidToolboxShowAllTabs = 656;

			// Token: 0x04000D2D RID: 3373
			internal const int cmdidProjectDependencies = 657;

			// Token: 0x04000D2E RID: 3374
			internal const int cmdidCloseDocument = 658;

			// Token: 0x04000D2F RID: 3375
			internal const int cmdidToolboxSortItems = 659;

			// Token: 0x04000D30 RID: 3376
			internal const int cmdidViewBarView1 = 660;

			// Token: 0x04000D31 RID: 3377
			internal const int cmdidViewBarView2 = 661;

			// Token: 0x04000D32 RID: 3378
			internal const int cmdidViewBarView3 = 662;

			// Token: 0x04000D33 RID: 3379
			internal const int cmdidViewBarView4 = 663;

			// Token: 0x04000D34 RID: 3380
			internal const int cmdidViewBarView5 = 664;

			// Token: 0x04000D35 RID: 3381
			internal const int cmdidViewBarView6 = 665;

			// Token: 0x04000D36 RID: 3382
			internal const int cmdidViewBarView7 = 666;

			// Token: 0x04000D37 RID: 3383
			internal const int cmdidViewBarView8 = 667;

			// Token: 0x04000D38 RID: 3384
			internal const int cmdidViewBarView9 = 668;

			// Token: 0x04000D39 RID: 3385
			internal const int cmdidViewBarView10 = 669;

			// Token: 0x04000D3A RID: 3386
			internal const int cmdidViewBarView11 = 670;

			// Token: 0x04000D3B RID: 3387
			internal const int cmdidViewBarView12 = 671;

			// Token: 0x04000D3C RID: 3388
			internal const int cmdidViewBarView13 = 672;

			// Token: 0x04000D3D RID: 3389
			internal const int cmdidViewBarView14 = 673;

			// Token: 0x04000D3E RID: 3390
			internal const int cmdidViewBarView15 = 674;

			// Token: 0x04000D3F RID: 3391
			internal const int cmdidViewBarView16 = 675;

			// Token: 0x04000D40 RID: 3392
			internal const int cmdidViewBarView17 = 676;

			// Token: 0x04000D41 RID: 3393
			internal const int cmdidViewBarView18 = 677;

			// Token: 0x04000D42 RID: 3394
			internal const int cmdidViewBarView19 = 678;

			// Token: 0x04000D43 RID: 3395
			internal const int cmdidViewBarView20 = 679;

			// Token: 0x04000D44 RID: 3396
			internal const int cmdidViewBarView21 = 680;

			// Token: 0x04000D45 RID: 3397
			internal const int cmdidViewBarView22 = 681;

			// Token: 0x04000D46 RID: 3398
			internal const int cmdidViewBarView23 = 682;

			// Token: 0x04000D47 RID: 3399
			internal const int cmdidViewBarView24 = 683;

			// Token: 0x04000D48 RID: 3400
			internal const int cmdidSolutionCfg = 684;

			// Token: 0x04000D49 RID: 3401
			internal const int cmdidSolutionCfgGetList = 685;

			// Token: 0x04000D4A RID: 3402
			internal const int cmdidManageIndexes = 675;

			// Token: 0x04000D4B RID: 3403
			internal const int cmdidManageRelationships = 676;

			// Token: 0x04000D4C RID: 3404
			internal const int cmdidManageConstraints = 677;

			// Token: 0x04000D4D RID: 3405
			internal const int cmdidTaskListCustomView1 = 678;

			// Token: 0x04000D4E RID: 3406
			internal const int cmdidTaskListCustomView2 = 679;

			// Token: 0x04000D4F RID: 3407
			internal const int cmdidTaskListCustomView3 = 680;

			// Token: 0x04000D50 RID: 3408
			internal const int cmdidTaskListCustomView4 = 681;

			// Token: 0x04000D51 RID: 3409
			internal const int cmdidTaskListCustomView5 = 682;

			// Token: 0x04000D52 RID: 3410
			internal const int cmdidTaskListCustomView6 = 683;

			// Token: 0x04000D53 RID: 3411
			internal const int cmdidTaskListCustomView7 = 684;

			// Token: 0x04000D54 RID: 3412
			internal const int cmdidTaskListCustomView8 = 685;

			// Token: 0x04000D55 RID: 3413
			internal const int cmdidTaskListCustomView9 = 686;

			// Token: 0x04000D56 RID: 3414
			internal const int cmdidTaskListCustomView10 = 687;

			// Token: 0x04000D57 RID: 3415
			internal const int cmdidTaskListCustomView11 = 688;

			// Token: 0x04000D58 RID: 3416
			internal const int cmdidTaskListCustomView12 = 689;

			// Token: 0x04000D59 RID: 3417
			internal const int cmdidTaskListCustomView13 = 690;

			// Token: 0x04000D5A RID: 3418
			internal const int cmdidTaskListCustomView14 = 691;

			// Token: 0x04000D5B RID: 3419
			internal const int cmdidTaskListCustomView15 = 692;

			// Token: 0x04000D5C RID: 3420
			internal const int cmdidTaskListCustomView16 = 693;

			// Token: 0x04000D5D RID: 3421
			internal const int cmdidTaskListCustomView17 = 694;

			// Token: 0x04000D5E RID: 3422
			internal const int cmdidTaskListCustomView18 = 695;

			// Token: 0x04000D5F RID: 3423
			internal const int cmdidTaskListCustomView19 = 696;

			// Token: 0x04000D60 RID: 3424
			internal const int cmdidTaskListCustomView20 = 697;

			// Token: 0x04000D61 RID: 3425
			internal const int cmdidTaskListCustomView21 = 698;

			// Token: 0x04000D62 RID: 3426
			internal const int cmdidTaskListCustomView22 = 699;

			// Token: 0x04000D63 RID: 3427
			internal const int cmdidTaskListCustomView23 = 700;

			// Token: 0x04000D64 RID: 3428
			internal const int cmdidTaskListCustomView24 = 701;

			// Token: 0x04000D65 RID: 3429
			internal const int cmdidTaskListCustomView25 = 702;

			// Token: 0x04000D66 RID: 3430
			internal const int cmdidTaskListCustomView26 = 703;

			// Token: 0x04000D67 RID: 3431
			internal const int cmdidTaskListCustomView27 = 704;

			// Token: 0x04000D68 RID: 3432
			internal const int cmdidTaskListCustomView28 = 705;

			// Token: 0x04000D69 RID: 3433
			internal const int cmdidTaskListCustomView29 = 706;

			// Token: 0x04000D6A RID: 3434
			internal const int cmdidTaskListCustomView30 = 707;

			// Token: 0x04000D6B RID: 3435
			internal const int cmdidTaskListCustomView31 = 708;

			// Token: 0x04000D6C RID: 3436
			internal const int cmdidTaskListCustomView32 = 709;

			// Token: 0x04000D6D RID: 3437
			internal const int cmdidTaskListCustomView33 = 710;

			// Token: 0x04000D6E RID: 3438
			internal const int cmdidTaskListCustomView34 = 711;

			// Token: 0x04000D6F RID: 3439
			internal const int cmdidTaskListCustomView35 = 712;

			// Token: 0x04000D70 RID: 3440
			internal const int cmdidTaskListCustomView36 = 713;

			// Token: 0x04000D71 RID: 3441
			internal const int cmdidTaskListCustomView37 = 714;

			// Token: 0x04000D72 RID: 3442
			internal const int cmdidTaskListCustomView38 = 715;

			// Token: 0x04000D73 RID: 3443
			internal const int cmdidTaskListCustomView39 = 716;

			// Token: 0x04000D74 RID: 3444
			internal const int cmdidTaskListCustomView40 = 717;

			// Token: 0x04000D75 RID: 3445
			internal const int cmdidTaskListCustomView41 = 718;

			// Token: 0x04000D76 RID: 3446
			internal const int cmdidTaskListCustomView42 = 719;

			// Token: 0x04000D77 RID: 3447
			internal const int cmdidTaskListCustomView43 = 720;

			// Token: 0x04000D78 RID: 3448
			internal const int cmdidTaskListCustomView44 = 721;

			// Token: 0x04000D79 RID: 3449
			internal const int cmdidTaskListCustomView45 = 722;

			// Token: 0x04000D7A RID: 3450
			internal const int cmdidTaskListCustomView46 = 723;

			// Token: 0x04000D7B RID: 3451
			internal const int cmdidTaskListCustomView47 = 724;

			// Token: 0x04000D7C RID: 3452
			internal const int cmdidTaskListCustomView48 = 725;

			// Token: 0x04000D7D RID: 3453
			internal const int cmdidTaskListCustomView49 = 726;

			// Token: 0x04000D7E RID: 3454
			internal const int cmdidTaskListCustomView50 = 727;

			// Token: 0x04000D7F RID: 3455
			internal const int cmdidObjectSearch = 728;

			// Token: 0x04000D80 RID: 3456
			internal const int cmdidCommandWindow = 729;

			// Token: 0x04000D81 RID: 3457
			internal const int cmdidCommandWindowMarkMode = 730;

			// Token: 0x04000D82 RID: 3458
			internal const int cmdidLogCommandWindow = 731;

			// Token: 0x04000D83 RID: 3459
			internal const int cmdidShell = 732;

			// Token: 0x04000D84 RID: 3460
			internal const int cmdidSingleChar = 733;

			// Token: 0x04000D85 RID: 3461
			internal const int cmdidZeroOrMore = 734;

			// Token: 0x04000D86 RID: 3462
			internal const int cmdidOneOrMore = 735;

			// Token: 0x04000D87 RID: 3463
			internal const int cmdidBeginLine = 736;

			// Token: 0x04000D88 RID: 3464
			internal const int cmdidEndLine = 737;

			// Token: 0x04000D89 RID: 3465
			internal const int cmdidBeginWord = 738;

			// Token: 0x04000D8A RID: 3466
			internal const int cmdidEndWord = 739;

			// Token: 0x04000D8B RID: 3467
			internal const int cmdidCharInSet = 740;

			// Token: 0x04000D8C RID: 3468
			internal const int cmdidCharNotInSet = 741;

			// Token: 0x04000D8D RID: 3469
			internal const int cmdidOr = 742;

			// Token: 0x04000D8E RID: 3470
			internal const int cmdidEscape = 743;

			// Token: 0x04000D8F RID: 3471
			internal const int cmdidTagExp = 744;

			// Token: 0x04000D90 RID: 3472
			internal const int cmdidPatternMatchHelp = 745;

			// Token: 0x04000D91 RID: 3473
			internal const int cmdidRegExList = 746;

			// Token: 0x04000D92 RID: 3474
			internal const int cmdidDebugReserved1 = 747;

			// Token: 0x04000D93 RID: 3475
			internal const int cmdidDebugReserved2 = 748;

			// Token: 0x04000D94 RID: 3476
			internal const int cmdidDebugReserved3 = 749;

			// Token: 0x04000D95 RID: 3477
			internal const int cmdidWildZeroOrMore = 754;

			// Token: 0x04000D96 RID: 3478
			internal const int cmdidWildSingleChar = 755;

			// Token: 0x04000D97 RID: 3479
			internal const int cmdidWildSingleDigit = 756;

			// Token: 0x04000D98 RID: 3480
			internal const int cmdidWildCharInSet = 757;

			// Token: 0x04000D99 RID: 3481
			internal const int cmdidWildCharNotInSet = 758;

			// Token: 0x04000D9A RID: 3482
			internal const int cmdidFindWhatText = 759;

			// Token: 0x04000D9B RID: 3483
			internal const int cmdidTaggedExp1 = 760;

			// Token: 0x04000D9C RID: 3484
			internal const int cmdidTaggedExp2 = 761;

			// Token: 0x04000D9D RID: 3485
			internal const int cmdidTaggedExp3 = 762;

			// Token: 0x04000D9E RID: 3486
			internal const int cmdidTaggedExp4 = 763;

			// Token: 0x04000D9F RID: 3487
			internal const int cmdidTaggedExp5 = 764;

			// Token: 0x04000DA0 RID: 3488
			internal const int cmdidTaggedExp6 = 765;

			// Token: 0x04000DA1 RID: 3489
			internal const int cmdidTaggedExp7 = 766;

			// Token: 0x04000DA2 RID: 3490
			internal const int cmdidTaggedExp8 = 767;

			// Token: 0x04000DA3 RID: 3491
			internal const int cmdidTaggedExp9 = 768;

			// Token: 0x04000DA4 RID: 3492
			internal const int cmdidEditorWidgetClick = 769;

			// Token: 0x04000DA5 RID: 3493
			internal const int cmdidCmdWinUpdateAC = 770;

			// Token: 0x04000DA6 RID: 3494
			internal const int cmdidSlnCfgMgr = 771;

			// Token: 0x04000DA7 RID: 3495
			internal const int cmdidAddNewProject = 772;

			// Token: 0x04000DA8 RID: 3496
			internal const int cmdidAddExistingProject = 773;

			// Token: 0x04000DA9 RID: 3497
			internal const int cmdidAddNewSolutionItem = 774;

			// Token: 0x04000DAA RID: 3498
			internal const int cmdidAddExistingSolutionItem = 775;

			// Token: 0x04000DAB RID: 3499
			internal const int cmdidAutoHideContext1 = 776;

			// Token: 0x04000DAC RID: 3500
			internal const int cmdidAutoHideContext2 = 777;

			// Token: 0x04000DAD RID: 3501
			internal const int cmdidAutoHideContext3 = 778;

			// Token: 0x04000DAE RID: 3502
			internal const int cmdidAutoHideContext4 = 779;

			// Token: 0x04000DAF RID: 3503
			internal const int cmdidAutoHideContext5 = 780;

			// Token: 0x04000DB0 RID: 3504
			internal const int cmdidAutoHideContext6 = 781;

			// Token: 0x04000DB1 RID: 3505
			internal const int cmdidAutoHideContext7 = 782;

			// Token: 0x04000DB2 RID: 3506
			internal const int cmdidAutoHideContext8 = 783;

			// Token: 0x04000DB3 RID: 3507
			internal const int cmdidAutoHideContext9 = 784;

			// Token: 0x04000DB4 RID: 3508
			internal const int cmdidAutoHideContext10 = 785;

			// Token: 0x04000DB5 RID: 3509
			internal const int cmdidAutoHideContext11 = 786;

			// Token: 0x04000DB6 RID: 3510
			internal const int cmdidAutoHideContext12 = 787;

			// Token: 0x04000DB7 RID: 3511
			internal const int cmdidAutoHideContext13 = 788;

			// Token: 0x04000DB8 RID: 3512
			internal const int cmdidAutoHideContext14 = 789;

			// Token: 0x04000DB9 RID: 3513
			internal const int cmdidAutoHideContext15 = 790;

			// Token: 0x04000DBA RID: 3514
			internal const int cmdidAutoHideContext16 = 791;

			// Token: 0x04000DBB RID: 3515
			internal const int cmdidAutoHideContext17 = 792;

			// Token: 0x04000DBC RID: 3516
			internal const int cmdidAutoHideContext18 = 793;

			// Token: 0x04000DBD RID: 3517
			internal const int cmdidAutoHideContext19 = 794;

			// Token: 0x04000DBE RID: 3518
			internal const int cmdidAutoHideContext20 = 795;

			// Token: 0x04000DBF RID: 3519
			internal const int cmdidAutoHideContext21 = 796;

			// Token: 0x04000DC0 RID: 3520
			internal const int cmdidAutoHideContext22 = 797;

			// Token: 0x04000DC1 RID: 3521
			internal const int cmdidAutoHideContext23 = 798;

			// Token: 0x04000DC2 RID: 3522
			internal const int cmdidAutoHideContext24 = 799;

			// Token: 0x04000DC3 RID: 3523
			internal const int cmdidAutoHideContext25 = 800;

			// Token: 0x04000DC4 RID: 3524
			internal const int cmdidAutoHideContext26 = 801;

			// Token: 0x04000DC5 RID: 3525
			internal const int cmdidAutoHideContext27 = 802;

			// Token: 0x04000DC6 RID: 3526
			internal const int cmdidAutoHideContext28 = 803;

			// Token: 0x04000DC7 RID: 3527
			internal const int cmdidAutoHideContext29 = 804;

			// Token: 0x04000DC8 RID: 3528
			internal const int cmdidAutoHideContext30 = 805;

			// Token: 0x04000DC9 RID: 3529
			internal const int cmdidAutoHideContext31 = 806;

			// Token: 0x04000DCA RID: 3530
			internal const int cmdidAutoHideContext32 = 807;

			// Token: 0x04000DCB RID: 3531
			internal const int cmdidAutoHideContext33 = 808;

			// Token: 0x04000DCC RID: 3532
			internal const int cmdidShellNavBackward = 809;

			// Token: 0x04000DCD RID: 3533
			internal const int cmdidShellNavForward = 810;

			// Token: 0x04000DCE RID: 3534
			internal const int cmdidShellNavigate1 = 811;

			// Token: 0x04000DCF RID: 3535
			internal const int cmdidShellNavigate2 = 812;

			// Token: 0x04000DD0 RID: 3536
			internal const int cmdidShellNavigate3 = 813;

			// Token: 0x04000DD1 RID: 3537
			internal const int cmdidShellNavigate4 = 814;

			// Token: 0x04000DD2 RID: 3538
			internal const int cmdidShellNavigate5 = 815;

			// Token: 0x04000DD3 RID: 3539
			internal const int cmdidShellNavigate6 = 816;

			// Token: 0x04000DD4 RID: 3540
			internal const int cmdidShellNavigate7 = 817;

			// Token: 0x04000DD5 RID: 3541
			internal const int cmdidShellNavigate8 = 818;

			// Token: 0x04000DD6 RID: 3542
			internal const int cmdidShellNavigate9 = 819;

			// Token: 0x04000DD7 RID: 3543
			internal const int cmdidShellNavigate10 = 820;

			// Token: 0x04000DD8 RID: 3544
			internal const int cmdidShellNavigate11 = 821;

			// Token: 0x04000DD9 RID: 3545
			internal const int cmdidShellNavigate12 = 822;

			// Token: 0x04000DDA RID: 3546
			internal const int cmdidShellNavigate13 = 823;

			// Token: 0x04000DDB RID: 3547
			internal const int cmdidShellNavigate14 = 824;

			// Token: 0x04000DDC RID: 3548
			internal const int cmdidShellNavigate15 = 825;

			// Token: 0x04000DDD RID: 3549
			internal const int cmdidShellNavigate16 = 826;

			// Token: 0x04000DDE RID: 3550
			internal const int cmdidShellNavigate17 = 827;

			// Token: 0x04000DDF RID: 3551
			internal const int cmdidShellNavigate18 = 828;

			// Token: 0x04000DE0 RID: 3552
			internal const int cmdidShellNavigate19 = 829;

			// Token: 0x04000DE1 RID: 3553
			internal const int cmdidShellNavigate20 = 830;

			// Token: 0x04000DE2 RID: 3554
			internal const int cmdidShellNavigate21 = 831;

			// Token: 0x04000DE3 RID: 3555
			internal const int cmdidShellNavigate22 = 832;

			// Token: 0x04000DE4 RID: 3556
			internal const int cmdidShellNavigate23 = 833;

			// Token: 0x04000DE5 RID: 3557
			internal const int cmdidShellNavigate24 = 834;

			// Token: 0x04000DE6 RID: 3558
			internal const int cmdidShellNavigate25 = 835;

			// Token: 0x04000DE7 RID: 3559
			internal const int cmdidShellNavigate26 = 836;

			// Token: 0x04000DE8 RID: 3560
			internal const int cmdidShellNavigate27 = 837;

			// Token: 0x04000DE9 RID: 3561
			internal const int cmdidShellNavigate28 = 838;

			// Token: 0x04000DEA RID: 3562
			internal const int cmdidShellNavigate29 = 839;

			// Token: 0x04000DEB RID: 3563
			internal const int cmdidShellNavigate30 = 840;

			// Token: 0x04000DEC RID: 3564
			internal const int cmdidShellNavigate31 = 841;

			// Token: 0x04000DED RID: 3565
			internal const int cmdidShellNavigate32 = 842;

			// Token: 0x04000DEE RID: 3566
			internal const int cmdidShellNavigate33 = 843;

			// Token: 0x04000DEF RID: 3567
			internal const int cmdidShellWindowNavigate1 = 844;

			// Token: 0x04000DF0 RID: 3568
			internal const int cmdidShellWindowNavigate2 = 845;

			// Token: 0x04000DF1 RID: 3569
			internal const int cmdidShellWindowNavigate3 = 846;

			// Token: 0x04000DF2 RID: 3570
			internal const int cmdidShellWindowNavigate4 = 847;

			// Token: 0x04000DF3 RID: 3571
			internal const int cmdidShellWindowNavigate5 = 848;

			// Token: 0x04000DF4 RID: 3572
			internal const int cmdidShellWindowNavigate6 = 849;

			// Token: 0x04000DF5 RID: 3573
			internal const int cmdidShellWindowNavigate7 = 850;

			// Token: 0x04000DF6 RID: 3574
			internal const int cmdidShellWindowNavigate8 = 851;

			// Token: 0x04000DF7 RID: 3575
			internal const int cmdidShellWindowNavigate9 = 852;

			// Token: 0x04000DF8 RID: 3576
			internal const int cmdidShellWindowNavigate10 = 853;

			// Token: 0x04000DF9 RID: 3577
			internal const int cmdidShellWindowNavigate11 = 854;

			// Token: 0x04000DFA RID: 3578
			internal const int cmdidShellWindowNavigate12 = 855;

			// Token: 0x04000DFB RID: 3579
			internal const int cmdidShellWindowNavigate13 = 856;

			// Token: 0x04000DFC RID: 3580
			internal const int cmdidShellWindowNavigate14 = 857;

			// Token: 0x04000DFD RID: 3581
			internal const int cmdidShellWindowNavigate15 = 858;

			// Token: 0x04000DFE RID: 3582
			internal const int cmdidShellWindowNavigate16 = 859;

			// Token: 0x04000DFF RID: 3583
			internal const int cmdidShellWindowNavigate17 = 860;

			// Token: 0x04000E00 RID: 3584
			internal const int cmdidShellWindowNavigate18 = 861;

			// Token: 0x04000E01 RID: 3585
			internal const int cmdidShellWindowNavigate19 = 862;

			// Token: 0x04000E02 RID: 3586
			internal const int cmdidShellWindowNavigate20 = 863;

			// Token: 0x04000E03 RID: 3587
			internal const int cmdidShellWindowNavigate21 = 864;

			// Token: 0x04000E04 RID: 3588
			internal const int cmdidShellWindowNavigate22 = 865;

			// Token: 0x04000E05 RID: 3589
			internal const int cmdidShellWindowNavigate23 = 866;

			// Token: 0x04000E06 RID: 3590
			internal const int cmdidShellWindowNavigate24 = 867;

			// Token: 0x04000E07 RID: 3591
			internal const int cmdidShellWindowNavigate25 = 868;

			// Token: 0x04000E08 RID: 3592
			internal const int cmdidShellWindowNavigate26 = 869;

			// Token: 0x04000E09 RID: 3593
			internal const int cmdidShellWindowNavigate27 = 870;

			// Token: 0x04000E0A RID: 3594
			internal const int cmdidShellWindowNavigate28 = 871;

			// Token: 0x04000E0B RID: 3595
			internal const int cmdidShellWindowNavigate29 = 872;

			// Token: 0x04000E0C RID: 3596
			internal const int cmdidShellWindowNavigate30 = 873;

			// Token: 0x04000E0D RID: 3597
			internal const int cmdidShellWindowNavigate31 = 874;

			// Token: 0x04000E0E RID: 3598
			internal const int cmdidShellWindowNavigate32 = 875;

			// Token: 0x04000E0F RID: 3599
			internal const int cmdidShellWindowNavigate33 = 876;

			// Token: 0x04000E10 RID: 3600
			internal const int cmdidOBSDoFind = 877;

			// Token: 0x04000E11 RID: 3601
			internal const int cmdidOBSMatchCase = 878;

			// Token: 0x04000E12 RID: 3602
			internal const int cmdidOBSMatchSubString = 879;

			// Token: 0x04000E13 RID: 3603
			internal const int cmdidOBSMatchWholeWord = 880;

			// Token: 0x04000E14 RID: 3604
			internal const int cmdidOBSMatchPrefix = 881;

			// Token: 0x04000E15 RID: 3605
			internal const int cmdidBuildSln = 882;

			// Token: 0x04000E16 RID: 3606
			internal const int cmdidRebuildSln = 883;

			// Token: 0x04000E17 RID: 3607
			internal const int cmdidDeploySln = 884;

			// Token: 0x04000E18 RID: 3608
			internal const int cmdidCleanSln = 885;

			// Token: 0x04000E19 RID: 3609
			internal const int cmdidBuildSel = 886;

			// Token: 0x04000E1A RID: 3610
			internal const int cmdidRebuildSel = 887;

			// Token: 0x04000E1B RID: 3611
			internal const int cmdidDeploySel = 888;

			// Token: 0x04000E1C RID: 3612
			internal const int cmdidCleanSel = 889;

			// Token: 0x04000E1D RID: 3613
			internal const int cmdidCancelBuild = 890;

			// Token: 0x04000E1E RID: 3614
			internal const int cmdidBatchBuildDlg = 891;

			// Token: 0x04000E1F RID: 3615
			internal const int cmdidBuildCtx = 892;

			// Token: 0x04000E20 RID: 3616
			internal const int cmdidRebuildCtx = 893;

			// Token: 0x04000E21 RID: 3617
			internal const int cmdidDeployCtx = 894;

			// Token: 0x04000E22 RID: 3618
			internal const int cmdidCleanCtx = 895;

			// Token: 0x04000E23 RID: 3619
			internal const int cmdidMRUFile1 = 900;

			// Token: 0x04000E24 RID: 3620
			internal const int cmdidMRUFile2 = 901;

			// Token: 0x04000E25 RID: 3621
			internal const int cmdidMRUFile3 = 902;

			// Token: 0x04000E26 RID: 3622
			internal const int cmdidMRUFile4 = 903;

			// Token: 0x04000E27 RID: 3623
			internal const int cmdidMRUFile5 = 904;

			// Token: 0x04000E28 RID: 3624
			internal const int cmdidMRUFile6 = 905;

			// Token: 0x04000E29 RID: 3625
			internal const int cmdidMRUFile7 = 906;

			// Token: 0x04000E2A RID: 3626
			internal const int cmdidMRUFile8 = 907;

			// Token: 0x04000E2B RID: 3627
			internal const int cmdidMRUFile9 = 908;

			// Token: 0x04000E2C RID: 3628
			internal const int cmdidMRUFile10 = 909;

			// Token: 0x04000E2D RID: 3629
			internal const int cmdidMRUFile11 = 910;

			// Token: 0x04000E2E RID: 3630
			internal const int cmdidMRUFile12 = 911;

			// Token: 0x04000E2F RID: 3631
			internal const int cmdidMRUFile13 = 912;

			// Token: 0x04000E30 RID: 3632
			internal const int cmdidMRUFile14 = 913;

			// Token: 0x04000E31 RID: 3633
			internal const int cmdidMRUFile15 = 914;

			// Token: 0x04000E32 RID: 3634
			internal const int cmdidMRUFile16 = 915;

			// Token: 0x04000E33 RID: 3635
			internal const int cmdidMRUFile17 = 916;

			// Token: 0x04000E34 RID: 3636
			internal const int cmdidMRUFile18 = 917;

			// Token: 0x04000E35 RID: 3637
			internal const int cmdidMRUFile19 = 918;

			// Token: 0x04000E36 RID: 3638
			internal const int cmdidMRUFile20 = 919;

			// Token: 0x04000E37 RID: 3639
			internal const int cmdidMRUFile21 = 920;

			// Token: 0x04000E38 RID: 3640
			internal const int cmdidMRUFile22 = 921;

			// Token: 0x04000E39 RID: 3641
			internal const int cmdidMRUFile23 = 922;

			// Token: 0x04000E3A RID: 3642
			internal const int cmdidMRUFile24 = 923;

			// Token: 0x04000E3B RID: 3643
			internal const int cmdidMRUFile25 = 924;

			// Token: 0x04000E3C RID: 3644
			internal const int cmdidGotoDefn = 925;

			// Token: 0x04000E3D RID: 3645
			internal const int cmdidGotoDecl = 926;

			// Token: 0x04000E3E RID: 3646
			internal const int cmdidBrowseDefn = 927;

			// Token: 0x04000E3F RID: 3647
			internal const int cmdidShowMembers = 928;

			// Token: 0x04000E40 RID: 3648
			internal const int cmdidShowBases = 929;

			// Token: 0x04000E41 RID: 3649
			internal const int cmdidShowDerived = 930;

			// Token: 0x04000E42 RID: 3650
			internal const int cmdidShowDefns = 931;

			// Token: 0x04000E43 RID: 3651
			internal const int cmdidShowRefs = 932;

			// Token: 0x04000E44 RID: 3652
			internal const int cmdidShowCallers = 933;

			// Token: 0x04000E45 RID: 3653
			internal const int cmdidShowCallees = 934;

			// Token: 0x04000E46 RID: 3654
			internal const int cmdidDefineSubset = 935;

			// Token: 0x04000E47 RID: 3655
			internal const int cmdidSetSubset = 936;

			// Token: 0x04000E48 RID: 3656
			internal const int cmdidCVGroupingNone = 950;

			// Token: 0x04000E49 RID: 3657
			internal const int cmdidCVGroupingSortOnly = 951;

			// Token: 0x04000E4A RID: 3658
			internal const int cmdidCVGroupingGrouped = 952;

			// Token: 0x04000E4B RID: 3659
			internal const int cmdidCVShowPackages = 953;

			// Token: 0x04000E4C RID: 3660
			internal const int cmdidQryManageIndexes = 954;

			// Token: 0x04000E4D RID: 3661
			internal const int cmdidBrowseComponent = 955;

			// Token: 0x04000E4E RID: 3662
			internal const int cmdidPrintDefault = 956;

			// Token: 0x04000E4F RID: 3663
			internal const int cmdidBrowseDoc = 957;

			// Token: 0x04000E50 RID: 3664
			internal const int cmdidStandardMax = 1000;

			// Token: 0x04000E51 RID: 3665
			internal const int cmdidFormsFirst = 24576;

			// Token: 0x04000E52 RID: 3666
			internal const int cmdidFormsLast = 28671;

			// Token: 0x04000E53 RID: 3667
			internal const int cmdidVBEFirst = 32768;

			// Token: 0x04000E54 RID: 3668
			internal const int msotcidBookmarkWellMenu = 32769;

			// Token: 0x04000E55 RID: 3669
			internal const int cmdidZoom200 = 32770;

			// Token: 0x04000E56 RID: 3670
			internal const int cmdidZoom150 = 32771;

			// Token: 0x04000E57 RID: 3671
			internal const int cmdidZoom100 = 32772;

			// Token: 0x04000E58 RID: 3672
			internal const int cmdidZoom75 = 32773;

			// Token: 0x04000E59 RID: 3673
			internal const int cmdidZoom50 = 32774;

			// Token: 0x04000E5A RID: 3674
			internal const int cmdidZoom25 = 32775;

			// Token: 0x04000E5B RID: 3675
			internal const int cmdidZoom10 = 32784;

			// Token: 0x04000E5C RID: 3676
			internal const int msotcidZoomWellMenu = 32785;

			// Token: 0x04000E5D RID: 3677
			internal const int msotcidDebugPopWellMenu = 32786;

			// Token: 0x04000E5E RID: 3678
			internal const int msotcidAlignWellMenu = 32787;

			// Token: 0x04000E5F RID: 3679
			internal const int msotcidArrangeWellMenu = 32788;

			// Token: 0x04000E60 RID: 3680
			internal const int msotcidCenterWellMenu = 32789;

			// Token: 0x04000E61 RID: 3681
			internal const int msotcidSizeWellMenu = 32790;

			// Token: 0x04000E62 RID: 3682
			internal const int msotcidHorizontalSpaceWellMenu = 32791;

			// Token: 0x04000E63 RID: 3683
			internal const int msotcidVerticalSpaceWellMenu = 32800;

			// Token: 0x04000E64 RID: 3684
			internal const int msotcidDebugWellMenu = 32801;

			// Token: 0x04000E65 RID: 3685
			internal const int msotcidDebugMenuVB = 32802;

			// Token: 0x04000E66 RID: 3686
			internal const int msotcidStatementBuilderWellMenu = 32803;

			// Token: 0x04000E67 RID: 3687
			internal const int msotcidProjWinInsertMenu = 32804;

			// Token: 0x04000E68 RID: 3688
			internal const int msotcidToggleMenu = 32805;

			// Token: 0x04000E69 RID: 3689
			internal const int msotcidNewObjInsertWellMenu = 32806;

			// Token: 0x04000E6A RID: 3690
			internal const int msotcidSizeToWellMenu = 32807;

			// Token: 0x04000E6B RID: 3691
			internal const int msotcidCommandBars = 32808;

			// Token: 0x04000E6C RID: 3692
			internal const int msotcidVBOrderMenu = 32809;

			// Token: 0x04000E6D RID: 3693
			internal const int msotcidMSOnTheWeb = 32810;

			// Token: 0x04000E6E RID: 3694
			internal const int msotcidVBDesignerMenu = 32816;

			// Token: 0x04000E6F RID: 3695
			internal const int msotcidNewProjectWellMenu = 32817;

			// Token: 0x04000E70 RID: 3696
			internal const int msotcidProjectWellMenu = 32818;

			// Token: 0x04000E71 RID: 3697
			internal const int msotcidVBCode1ContextMenu = 32819;

			// Token: 0x04000E72 RID: 3698
			internal const int msotcidVBCode2ContextMenu = 32820;

			// Token: 0x04000E73 RID: 3699
			internal const int msotcidVBWatchContextMenu = 32821;

			// Token: 0x04000E74 RID: 3700
			internal const int msotcidVBImmediateContextMenu = 32822;

			// Token: 0x04000E75 RID: 3701
			internal const int msotcidVBLocalsContextMenu = 32823;

			// Token: 0x04000E76 RID: 3702
			internal const int msotcidVBFormContextMenu = 32824;

			// Token: 0x04000E77 RID: 3703
			internal const int msotcidVBControlContextMenu = 32825;

			// Token: 0x04000E78 RID: 3704
			internal const int msotcidVBProjWinContextMenu = 32826;

			// Token: 0x04000E79 RID: 3705
			internal const int msotcidVBProjWinContextBreakMenu = 32827;

			// Token: 0x04000E7A RID: 3706
			internal const int msotcidVBPreviewWinContextMenu = 32828;

			// Token: 0x04000E7B RID: 3707
			internal const int msotcidVBOBContextMenu = 32829;

			// Token: 0x04000E7C RID: 3708
			internal const int msotcidVBForms3ContextMenu = 32830;

			// Token: 0x04000E7D RID: 3709
			internal const int msotcidVBForms3ControlCMenu = 32831;

			// Token: 0x04000E7E RID: 3710
			internal const int msotcidVBForms3ControlCMenuGroup = 32832;

			// Token: 0x04000E7F RID: 3711
			internal const int msotcidVBForms3ControlPalette = 32833;

			// Token: 0x04000E80 RID: 3712
			internal const int msotcidVBForms3ToolboxCMenu = 32834;

			// Token: 0x04000E81 RID: 3713
			internal const int msotcidVBForms3MPCCMenu = 32835;

			// Token: 0x04000E82 RID: 3714
			internal const int msotcidVBForms3DragDropCMenu = 32836;

			// Token: 0x04000E83 RID: 3715
			internal const int msotcidVBToolBoxContextMenu = 32837;

			// Token: 0x04000E84 RID: 3716
			internal const int msotcidVBToolBoxGroupContextMenu = 32838;

			// Token: 0x04000E85 RID: 3717
			internal const int msotcidVBPropBrsHostContextMenu = 32839;

			// Token: 0x04000E86 RID: 3718
			internal const int msotcidVBPropBrsContextMenu = 32840;

			// Token: 0x04000E87 RID: 3719
			internal const int msotcidVBPalContextMenu = 32841;

			// Token: 0x04000E88 RID: 3720
			internal const int msotcidVBProjWinProjectContextMenu = 32842;

			// Token: 0x04000E89 RID: 3721
			internal const int msotcidVBProjWinFormContextMenu = 32843;

			// Token: 0x04000E8A RID: 3722
			internal const int msotcidVBProjWinModClassContextMenu = 32844;

			// Token: 0x04000E8B RID: 3723
			internal const int msotcidVBProjWinRelDocContextMenu = 32845;

			// Token: 0x04000E8C RID: 3724
			internal const int msotcidVBDockedWindowContextMenu = 32846;

			// Token: 0x04000E8D RID: 3725
			internal const int msotcidVBShortCutForms = 32847;

			// Token: 0x04000E8E RID: 3726
			internal const int msotcidVBShortCutCodeWindows = 32848;

			// Token: 0x04000E8F RID: 3727
			internal const int msotcidVBShortCutMisc = 32849;

			// Token: 0x04000E90 RID: 3728
			internal const int msotcidVBBuiltInMenus = 32850;

			// Token: 0x04000E91 RID: 3729
			internal const int msotcidPreviewWinFormPos = 32851;

			// Token: 0x04000E92 RID: 3730
			internal const int msotcidVBAddinFirst = 33280;
		}

		// Token: 0x0200019E RID: 414
		private static class ShellGuids
		{
			// Token: 0x04000E93 RID: 3731
			internal static readonly Guid VSStandardCommandSet97 = new Guid("{5efc7975-14bc-11cf-9b2b-00aa00573819}");

			// Token: 0x04000E94 RID: 3732
			internal static readonly Guid guidDsdCmdId = new Guid("{1F0FD094-8e53-11d2-8f9c-0060089fc486}");

			// Token: 0x04000E95 RID: 3733
			internal static readonly Guid SID_SOleComponentUIManager = new Guid("{5efc7974-14bc-11cf-9b2b-00aa00573819}");

			// Token: 0x04000E96 RID: 3734
			internal static readonly Guid GUID_VSTASKCATEGORY_DATADESIGNER = new Guid("{6B32EAED-13BB-11d3-A64F-00C04F683820}");

			// Token: 0x04000E97 RID: 3735
			internal static readonly Guid GUID_PropertyBrowserToolWindow = new Guid(-285584864, -7528, 4560, new byte[] { 143, 120, 0, 160, 201, 17, 0, 87 });
		}
	}
}
