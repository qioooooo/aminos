using System;
using System.Security.Permissions;

namespace System.Media
{
	// Token: 0x0200021F RID: 543
	[HostProtection(SecurityAction.LinkDemand, UI = true)]
	public sealed class SystemSounds
	{
		// Token: 0x0600125B RID: 4699 RVA: 0x0003E222 File Offset: 0x0003D222
		private SystemSounds()
		{
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x0600125C RID: 4700 RVA: 0x0003E22A File Offset: 0x0003D22A
		public static SystemSound Asterisk
		{
			get
			{
				if (SystemSounds.asterisk == null)
				{
					SystemSounds.asterisk = new SystemSound(64);
				}
				return SystemSounds.asterisk;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600125D RID: 4701 RVA: 0x0003E244 File Offset: 0x0003D244
		public static SystemSound Beep
		{
			get
			{
				if (SystemSounds.beep == null)
				{
					SystemSounds.beep = new SystemSound(0);
				}
				return SystemSounds.beep;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x0600125E RID: 4702 RVA: 0x0003E25D File Offset: 0x0003D25D
		public static SystemSound Exclamation
		{
			get
			{
				if (SystemSounds.exclamation == null)
				{
					SystemSounds.exclamation = new SystemSound(48);
				}
				return SystemSounds.exclamation;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x0600125F RID: 4703 RVA: 0x0003E277 File Offset: 0x0003D277
		public static SystemSound Hand
		{
			get
			{
				if (SystemSounds.hand == null)
				{
					SystemSounds.hand = new SystemSound(16);
				}
				return SystemSounds.hand;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001260 RID: 4704 RVA: 0x0003E291 File Offset: 0x0003D291
		public static SystemSound Question
		{
			get
			{
				if (SystemSounds.question == null)
				{
					SystemSounds.question = new SystemSound(32);
				}
				return SystemSounds.question;
			}
		}

		// Token: 0x040010B2 RID: 4274
		private static SystemSound asterisk;

		// Token: 0x040010B3 RID: 4275
		private static SystemSound beep;

		// Token: 0x040010B4 RID: 4276
		private static SystemSound exclamation;

		// Token: 0x040010B5 RID: 4277
		private static SystemSound hand;

		// Token: 0x040010B6 RID: 4278
		private static SystemSound question;

		// Token: 0x02000220 RID: 544
		private class NativeMethods
		{
			// Token: 0x06001261 RID: 4705 RVA: 0x0003E2AB File Offset: 0x0003D2AB
			private NativeMethods()
			{
			}

			// Token: 0x040010B7 RID: 4279
			internal const int MB_ICONHAND = 16;

			// Token: 0x040010B8 RID: 4280
			internal const int MB_ICONQUESTION = 32;

			// Token: 0x040010B9 RID: 4281
			internal const int MB_ICONEXCLAMATION = 48;

			// Token: 0x040010BA RID: 4282
			internal const int MB_ICONASTERISK = 64;
		}
	}
}
