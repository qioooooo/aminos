using System;

namespace System.Drawing
{
	// Token: 0x02000066 RID: 102
	public sealed class SystemIcons
	{
		// Token: 0x06000687 RID: 1671 RVA: 0x0001A6CA File Offset: 0x000196CA
		private SystemIcons()
		{
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000688 RID: 1672 RVA: 0x0001A6D2 File Offset: 0x000196D2
		public static Icon Application
		{
			get
			{
				if (SystemIcons._application == null)
				{
					SystemIcons._application = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32512));
				}
				return SystemIcons._application;
			}
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000689 RID: 1673 RVA: 0x0001A6F9 File Offset: 0x000196F9
		public static Icon Asterisk
		{
			get
			{
				if (SystemIcons._asterisk == null)
				{
					SystemIcons._asterisk = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32516));
				}
				return SystemIcons._asterisk;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x0600068A RID: 1674 RVA: 0x0001A720 File Offset: 0x00019720
		public static Icon Error
		{
			get
			{
				if (SystemIcons._error == null)
				{
					SystemIcons._error = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32513));
				}
				return SystemIcons._error;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0001A747 File Offset: 0x00019747
		public static Icon Exclamation
		{
			get
			{
				if (SystemIcons._exclamation == null)
				{
					SystemIcons._exclamation = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32515));
				}
				return SystemIcons._exclamation;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x0600068C RID: 1676 RVA: 0x0001A76E File Offset: 0x0001976E
		public static Icon Hand
		{
			get
			{
				if (SystemIcons._hand == null)
				{
					SystemIcons._hand = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32513));
				}
				return SystemIcons._hand;
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0001A795 File Offset: 0x00019795
		public static Icon Information
		{
			get
			{
				if (SystemIcons._information == null)
				{
					SystemIcons._information = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32516));
				}
				return SystemIcons._information;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x0600068E RID: 1678 RVA: 0x0001A7BC File Offset: 0x000197BC
		public static Icon Question
		{
			get
			{
				if (SystemIcons._question == null)
				{
					SystemIcons._question = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32514));
				}
				return SystemIcons._question;
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x0600068F RID: 1679 RVA: 0x0001A7E3 File Offset: 0x000197E3
		public static Icon Warning
		{
			get
			{
				if (SystemIcons._warning == null)
				{
					SystemIcons._warning = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32515));
				}
				return SystemIcons._warning;
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x0001A80A File Offset: 0x0001980A
		public static Icon WinLogo
		{
			get
			{
				if (SystemIcons._winlogo == null)
				{
					SystemIcons._winlogo = new Icon(SafeNativeMethods.LoadIcon(NativeMethods.NullHandleRef, 32517));
				}
				return SystemIcons._winlogo;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000691 RID: 1681 RVA: 0x0001A831 File Offset: 0x00019831
		public static Icon Shield
		{
			get
			{
				if (SystemIcons._shield == null)
				{
					SystemIcons._shield = new Icon(typeof(SystemIcons), "ShieldIcon.ico");
				}
				return SystemIcons._shield;
			}
		}

		// Token: 0x04000481 RID: 1153
		private static Icon _application;

		// Token: 0x04000482 RID: 1154
		private static Icon _asterisk;

		// Token: 0x04000483 RID: 1155
		private static Icon _error;

		// Token: 0x04000484 RID: 1156
		private static Icon _exclamation;

		// Token: 0x04000485 RID: 1157
		private static Icon _hand;

		// Token: 0x04000486 RID: 1158
		private static Icon _information;

		// Token: 0x04000487 RID: 1159
		private static Icon _question;

		// Token: 0x04000488 RID: 1160
		private static Icon _warning;

		// Token: 0x04000489 RID: 1161
		private static Icon _winlogo;

		// Token: 0x0400048A RID: 1162
		private static Icon _shield;
	}
}
