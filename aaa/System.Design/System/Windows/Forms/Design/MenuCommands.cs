using System;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000275 RID: 629
	public sealed class MenuCommands : StandardCommands
	{
		// Token: 0x04001350 RID: 4944
		private const int mnuidSelection = 1280;

		// Token: 0x04001351 RID: 4945
		private const int mnuidContainer = 1281;

		// Token: 0x04001352 RID: 4946
		private const int mnuidTraySelection = 1283;

		// Token: 0x04001353 RID: 4947
		private const int mnuidComponentTray = 1286;

		// Token: 0x04001354 RID: 4948
		private const int cmdidDesignerProperties = 4097;

		// Token: 0x04001355 RID: 4949
		private const int cmdidReverseCancel = 16385;

		// Token: 0x04001356 RID: 4950
		private const int cmdidSetStatusText = 16387;

		// Token: 0x04001357 RID: 4951
		private const int cmdidSetStatusRectangle = 16388;

		// Token: 0x04001358 RID: 4952
		private const int cmdidSpace = 16405;

		// Token: 0x04001359 RID: 4953
		private const int ECMD_CANCEL = 103;

		// Token: 0x0400135A RID: 4954
		private const int ECMD_RETURN = 3;

		// Token: 0x0400135B RID: 4955
		private const int ECMD_UP = 11;

		// Token: 0x0400135C RID: 4956
		private const int ECMD_DOWN = 13;

		// Token: 0x0400135D RID: 4957
		private const int ECMD_LEFT = 7;

		// Token: 0x0400135E RID: 4958
		private const int ECMD_RIGHT = 9;

		// Token: 0x0400135F RID: 4959
		private const int ECMD_RIGHT_EXT = 10;

		// Token: 0x04001360 RID: 4960
		private const int ECMD_UP_EXT = 12;

		// Token: 0x04001361 RID: 4961
		private const int ECMD_LEFT_EXT = 8;

		// Token: 0x04001362 RID: 4962
		private const int ECMD_DOWN_EXT = 14;

		// Token: 0x04001363 RID: 4963
		private const int ECMD_TAB = 4;

		// Token: 0x04001364 RID: 4964
		private const int ECMD_BACKTAB = 5;

		// Token: 0x04001365 RID: 4965
		private const int ECMD_INVOKESMARTTAG = 147;

		// Token: 0x04001366 RID: 4966
		private const int ECMD_CTLMOVELEFT = 1224;

		// Token: 0x04001367 RID: 4967
		private const int ECMD_CTLMOVEDOWN = 1225;

		// Token: 0x04001368 RID: 4968
		private const int ECMD_CTLMOVERIGHT = 1226;

		// Token: 0x04001369 RID: 4969
		private const int ECMD_CTLMOVEUP = 1227;

		// Token: 0x0400136A RID: 4970
		private const int ECMD_CTLSIZEDOWN = 1228;

		// Token: 0x0400136B RID: 4971
		private const int ECMD_CTLSIZEUP = 1229;

		// Token: 0x0400136C RID: 4972
		private const int ECMD_CTLSIZELEFT = 1230;

		// Token: 0x0400136D RID: 4973
		private const int ECMD_CTLSIZERIGHT = 1231;

		// Token: 0x0400136E RID: 4974
		private const int cmdidEditLabel = 338;

		// Token: 0x0400136F RID: 4975
		private const int ECMD_HOME = 15;

		// Token: 0x04001370 RID: 4976
		private const int ECMD_HOME_EXT = 16;

		// Token: 0x04001371 RID: 4977
		private const int ECMD_END = 17;

		// Token: 0x04001372 RID: 4978
		private const int ECMD_END_EXT = 18;

		// Token: 0x04001373 RID: 4979
		private static readonly Guid VSStandardCommandSet97 = new Guid("{5efc7975-14bc-11cf-9b2b-00aa00573819}");

		// Token: 0x04001374 RID: 4980
		private static readonly Guid wfMenuGroup = new Guid("{74D21312-2AEE-11d1-8BFB-00A0C90F26F7}");

		// Token: 0x04001375 RID: 4981
		private static readonly Guid wfCommandSet = new Guid("{74D21313-2AEE-11d1-8BFB-00A0C90F26F7}");

		// Token: 0x04001376 RID: 4982
		private static readonly Guid guidVSStd2K = new Guid("{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}");

		// Token: 0x04001377 RID: 4983
		public static readonly CommandID SelectionMenu = new CommandID(MenuCommands.wfMenuGroup, 1280);

		// Token: 0x04001378 RID: 4984
		public static readonly CommandID ContainerMenu = new CommandID(MenuCommands.wfMenuGroup, 1281);

		// Token: 0x04001379 RID: 4985
		public static readonly CommandID TraySelectionMenu = new CommandID(MenuCommands.wfMenuGroup, 1283);

		// Token: 0x0400137A RID: 4986
		public static readonly CommandID ComponentTrayMenu = new CommandID(MenuCommands.wfMenuGroup, 1286);

		// Token: 0x0400137B RID: 4987
		public static readonly CommandID DesignerProperties = new CommandID(MenuCommands.wfCommandSet, 4097);

		// Token: 0x0400137C RID: 4988
		public static readonly CommandID KeyCancel = new CommandID(MenuCommands.guidVSStd2K, 103);

		// Token: 0x0400137D RID: 4989
		public static readonly CommandID KeyReverseCancel = new CommandID(MenuCommands.wfCommandSet, 16385);

		// Token: 0x0400137E RID: 4990
		public static readonly CommandID KeyInvokeSmartTag = new CommandID(MenuCommands.guidVSStd2K, 147);

		// Token: 0x0400137F RID: 4991
		public static readonly CommandID KeyDefaultAction = new CommandID(MenuCommands.guidVSStd2K, 3);

		// Token: 0x04001380 RID: 4992
		public static readonly CommandID KeyMoveUp = new CommandID(MenuCommands.guidVSStd2K, 11);

		// Token: 0x04001381 RID: 4993
		public static readonly CommandID KeyMoveDown = new CommandID(MenuCommands.guidVSStd2K, 13);

		// Token: 0x04001382 RID: 4994
		public static readonly CommandID KeyMoveLeft = new CommandID(MenuCommands.guidVSStd2K, 7);

		// Token: 0x04001383 RID: 4995
		public static readonly CommandID KeyMoveRight = new CommandID(MenuCommands.guidVSStd2K, 9);

		// Token: 0x04001384 RID: 4996
		public static readonly CommandID KeyNudgeUp = new CommandID(MenuCommands.guidVSStd2K, 1227);

		// Token: 0x04001385 RID: 4997
		public static readonly CommandID KeyNudgeDown = new CommandID(MenuCommands.guidVSStd2K, 1225);

		// Token: 0x04001386 RID: 4998
		public static readonly CommandID KeyNudgeLeft = new CommandID(MenuCommands.guidVSStd2K, 1224);

		// Token: 0x04001387 RID: 4999
		public static readonly CommandID KeyNudgeRight = new CommandID(MenuCommands.guidVSStd2K, 1226);

		// Token: 0x04001388 RID: 5000
		public static readonly CommandID KeySizeWidthIncrease = new CommandID(MenuCommands.guidVSStd2K, 10);

		// Token: 0x04001389 RID: 5001
		public static readonly CommandID KeySizeHeightIncrease = new CommandID(MenuCommands.guidVSStd2K, 12);

		// Token: 0x0400138A RID: 5002
		public static readonly CommandID KeySizeWidthDecrease = new CommandID(MenuCommands.guidVSStd2K, 8);

		// Token: 0x0400138B RID: 5003
		public static readonly CommandID KeySizeHeightDecrease = new CommandID(MenuCommands.guidVSStd2K, 14);

		// Token: 0x0400138C RID: 5004
		public static readonly CommandID KeyNudgeWidthIncrease = new CommandID(MenuCommands.guidVSStd2K, 1231);

		// Token: 0x0400138D RID: 5005
		public static readonly CommandID KeyNudgeHeightIncrease = new CommandID(MenuCommands.guidVSStd2K, 1228);

		// Token: 0x0400138E RID: 5006
		public static readonly CommandID KeyNudgeWidthDecrease = new CommandID(MenuCommands.guidVSStd2K, 1230);

		// Token: 0x0400138F RID: 5007
		public static readonly CommandID KeyNudgeHeightDecrease = new CommandID(MenuCommands.guidVSStd2K, 1229);

		// Token: 0x04001390 RID: 5008
		public static readonly CommandID KeySelectNext = new CommandID(MenuCommands.guidVSStd2K, 4);

		// Token: 0x04001391 RID: 5009
		public static readonly CommandID KeySelectPrevious = new CommandID(MenuCommands.guidVSStd2K, 5);

		// Token: 0x04001392 RID: 5010
		public static readonly CommandID KeyTabOrderSelect = new CommandID(MenuCommands.wfCommandSet, 16405);

		// Token: 0x04001393 RID: 5011
		public static readonly CommandID EditLabel = new CommandID(MenuCommands.VSStandardCommandSet97, 338);

		// Token: 0x04001394 RID: 5012
		public static readonly CommandID KeyHome = new CommandID(MenuCommands.guidVSStd2K, 15);

		// Token: 0x04001395 RID: 5013
		public static readonly CommandID KeyEnd = new CommandID(MenuCommands.guidVSStd2K, 17);

		// Token: 0x04001396 RID: 5014
		public static readonly CommandID KeyShiftHome = new CommandID(MenuCommands.guidVSStd2K, 16);

		// Token: 0x04001397 RID: 5015
		public static readonly CommandID KeyShiftEnd = new CommandID(MenuCommands.guidVSStd2K, 18);

		// Token: 0x04001398 RID: 5016
		public static readonly CommandID SetStatusText = new CommandID(MenuCommands.wfCommandSet, 16387);

		// Token: 0x04001399 RID: 5017
		public static readonly CommandID SetStatusRectangle = new CommandID(MenuCommands.wfCommandSet, 16388);
	}
}
