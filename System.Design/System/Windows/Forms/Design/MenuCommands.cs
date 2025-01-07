using System;
using System.ComponentModel.Design;

namespace System.Windows.Forms.Design
{
	public sealed class MenuCommands : StandardCommands
	{
		private const int mnuidSelection = 1280;

		private const int mnuidContainer = 1281;

		private const int mnuidTraySelection = 1283;

		private const int mnuidComponentTray = 1286;

		private const int cmdidDesignerProperties = 4097;

		private const int cmdidReverseCancel = 16385;

		private const int cmdidSetStatusText = 16387;

		private const int cmdidSetStatusRectangle = 16388;

		private const int cmdidSpace = 16405;

		private const int ECMD_CANCEL = 103;

		private const int ECMD_RETURN = 3;

		private const int ECMD_UP = 11;

		private const int ECMD_DOWN = 13;

		private const int ECMD_LEFT = 7;

		private const int ECMD_RIGHT = 9;

		private const int ECMD_RIGHT_EXT = 10;

		private const int ECMD_UP_EXT = 12;

		private const int ECMD_LEFT_EXT = 8;

		private const int ECMD_DOWN_EXT = 14;

		private const int ECMD_TAB = 4;

		private const int ECMD_BACKTAB = 5;

		private const int ECMD_INVOKESMARTTAG = 147;

		private const int ECMD_CTLMOVELEFT = 1224;

		private const int ECMD_CTLMOVEDOWN = 1225;

		private const int ECMD_CTLMOVERIGHT = 1226;

		private const int ECMD_CTLMOVEUP = 1227;

		private const int ECMD_CTLSIZEDOWN = 1228;

		private const int ECMD_CTLSIZEUP = 1229;

		private const int ECMD_CTLSIZELEFT = 1230;

		private const int ECMD_CTLSIZERIGHT = 1231;

		private const int cmdidEditLabel = 338;

		private const int ECMD_HOME = 15;

		private const int ECMD_HOME_EXT = 16;

		private const int ECMD_END = 17;

		private const int ECMD_END_EXT = 18;

		private static readonly Guid VSStandardCommandSet97 = new Guid("{5efc7975-14bc-11cf-9b2b-00aa00573819}");

		private static readonly Guid wfMenuGroup = new Guid("{74D21312-2AEE-11d1-8BFB-00A0C90F26F7}");

		private static readonly Guid wfCommandSet = new Guid("{74D21313-2AEE-11d1-8BFB-00A0C90F26F7}");

		private static readonly Guid guidVSStd2K = new Guid("{1496A755-94DE-11D0-8C3F-00C04FC2AAE2}");

		public static readonly CommandID SelectionMenu = new CommandID(MenuCommands.wfMenuGroup, 1280);

		public static readonly CommandID ContainerMenu = new CommandID(MenuCommands.wfMenuGroup, 1281);

		public static readonly CommandID TraySelectionMenu = new CommandID(MenuCommands.wfMenuGroup, 1283);

		public static readonly CommandID ComponentTrayMenu = new CommandID(MenuCommands.wfMenuGroup, 1286);

		public static readonly CommandID DesignerProperties = new CommandID(MenuCommands.wfCommandSet, 4097);

		public static readonly CommandID KeyCancel = new CommandID(MenuCommands.guidVSStd2K, 103);

		public static readonly CommandID KeyReverseCancel = new CommandID(MenuCommands.wfCommandSet, 16385);

		public static readonly CommandID KeyInvokeSmartTag = new CommandID(MenuCommands.guidVSStd2K, 147);

		public static readonly CommandID KeyDefaultAction = new CommandID(MenuCommands.guidVSStd2K, 3);

		public static readonly CommandID KeyMoveUp = new CommandID(MenuCommands.guidVSStd2K, 11);

		public static readonly CommandID KeyMoveDown = new CommandID(MenuCommands.guidVSStd2K, 13);

		public static readonly CommandID KeyMoveLeft = new CommandID(MenuCommands.guidVSStd2K, 7);

		public static readonly CommandID KeyMoveRight = new CommandID(MenuCommands.guidVSStd2K, 9);

		public static readonly CommandID KeyNudgeUp = new CommandID(MenuCommands.guidVSStd2K, 1227);

		public static readonly CommandID KeyNudgeDown = new CommandID(MenuCommands.guidVSStd2K, 1225);

		public static readonly CommandID KeyNudgeLeft = new CommandID(MenuCommands.guidVSStd2K, 1224);

		public static readonly CommandID KeyNudgeRight = new CommandID(MenuCommands.guidVSStd2K, 1226);

		public static readonly CommandID KeySizeWidthIncrease = new CommandID(MenuCommands.guidVSStd2K, 10);

		public static readonly CommandID KeySizeHeightIncrease = new CommandID(MenuCommands.guidVSStd2K, 12);

		public static readonly CommandID KeySizeWidthDecrease = new CommandID(MenuCommands.guidVSStd2K, 8);

		public static readonly CommandID KeySizeHeightDecrease = new CommandID(MenuCommands.guidVSStd2K, 14);

		public static readonly CommandID KeyNudgeWidthIncrease = new CommandID(MenuCommands.guidVSStd2K, 1231);

		public static readonly CommandID KeyNudgeHeightIncrease = new CommandID(MenuCommands.guidVSStd2K, 1228);

		public static readonly CommandID KeyNudgeWidthDecrease = new CommandID(MenuCommands.guidVSStd2K, 1230);

		public static readonly CommandID KeyNudgeHeightDecrease = new CommandID(MenuCommands.guidVSStd2K, 1229);

		public static readonly CommandID KeySelectNext = new CommandID(MenuCommands.guidVSStd2K, 4);

		public static readonly CommandID KeySelectPrevious = new CommandID(MenuCommands.guidVSStd2K, 5);

		public static readonly CommandID KeyTabOrderSelect = new CommandID(MenuCommands.wfCommandSet, 16405);

		public static readonly CommandID EditLabel = new CommandID(MenuCommands.VSStandardCommandSet97, 338);

		public static readonly CommandID KeyHome = new CommandID(MenuCommands.guidVSStd2K, 15);

		public static readonly CommandID KeyEnd = new CommandID(MenuCommands.guidVSStd2K, 17);

		public static readonly CommandID KeyShiftHome = new CommandID(MenuCommands.guidVSStd2K, 16);

		public static readonly CommandID KeyShiftEnd = new CommandID(MenuCommands.guidVSStd2K, 18);

		public static readonly CommandID SetStatusText = new CommandID(MenuCommands.wfCommandSet, 16387);

		public static readonly CommandID SetStatusRectangle = new CommandID(MenuCommands.wfCommandSet, 16388);
	}
}
