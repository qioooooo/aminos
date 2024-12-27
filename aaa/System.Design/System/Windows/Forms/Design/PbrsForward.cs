using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000278 RID: 632
	internal class PbrsForward : IWindowTarget
	{
		// Token: 0x060017AF RID: 6063 RVA: 0x0007B409 File Offset: 0x0007A409
		public PbrsForward(Control target, IServiceProvider sp)
		{
			this.target = target;
			this.oldTarget = target.WindowTarget;
			this.sp = sp;
			target.WindowTarget = this;
		}

		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060017B0 RID: 6064 RVA: 0x0007B432 File Offset: 0x0007A432
		private IMenuCommandService MenuCommandService
		{
			get
			{
				if (this.menuCommandSvc == null && this.sp != null)
				{
					this.menuCommandSvc = (IMenuCommandService)this.sp.GetService(typeof(IMenuCommandService));
				}
				return this.menuCommandSvc;
			}
		}

		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060017B1 RID: 6065 RVA: 0x0007B46A File Offset: 0x0007A46A
		private ISupportInSituService InSituSupportService
		{
			get
			{
				return (ISupportInSituService)this.sp.GetService(typeof(ISupportInSituService));
			}
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x0007B486 File Offset: 0x0007A486
		public void Dispose()
		{
			this.target.WindowTarget = this.oldTarget;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x0007B499 File Offset: 0x0007A499
		void IWindowTarget.OnHandleChange(IntPtr newHandle)
		{
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0007B49C File Offset: 0x0007A49C
		void IWindowTarget.OnMessage(ref Message m)
		{
			this.ignoreMessages = false;
			if (((m.Msg >= 256 && m.Msg <= 264) || (m.Msg >= 269 && m.Msg <= 271)) && this.InSituSupportService != null)
			{
				this.ignoreMessages = this.InSituSupportService.IgnoreMessages;
			}
			int msg = m.Msg;
			if (msg <= 258)
			{
				if (msg != 8)
				{
					switch (msg)
					{
					case 256:
						this.lastKeyDown = m;
						goto IL_0354;
					case 257:
						break;
					case 258:
						goto IL_025D;
					default:
						goto IL_0354;
					}
				}
				else
				{
					if (this.postCharMessage)
					{
						UnsafeNativeMethods.PostMessage(this.target.Handle, 6552, IntPtr.Zero, IntPtr.Zero);
						this.postCharMessage = false;
						goto IL_0354;
					}
					goto IL_0354;
				}
			}
			else
			{
				switch (msg)
				{
				case 269:
				case 271:
					goto IL_025D;
				case 270:
					break;
				default:
				{
					if (msg != 6552)
					{
						goto IL_0354;
					}
					if (this.bufferedChars == null)
					{
						return;
					}
					IntPtr intPtr = IntPtr.Zero;
					if (!this.ignoreMessages)
					{
						intPtr = NativeMethods.GetFocus();
					}
					else if (this.InSituSupportService != null)
					{
						intPtr = this.InSituSupportService.GetEditWindow();
					}
					else
					{
						intPtr = NativeMethods.GetFocus();
					}
					if (intPtr != m.HWnd)
					{
						foreach (object obj in this.bufferedChars)
						{
							PbrsForward.BufferedKey bufferedKey = (PbrsForward.BufferedKey)obj;
							if (bufferedKey.KeyChar.Msg == 258)
							{
								if (bufferedKey.KeyDown.Msg != 0)
								{
									NativeMethods.SendMessage(intPtr, 256, bufferedKey.KeyDown.WParam, bufferedKey.KeyDown.LParam);
								}
								NativeMethods.SendMessage(intPtr, 258, bufferedKey.KeyChar.WParam, bufferedKey.KeyChar.LParam);
								if (bufferedKey.KeyUp.Msg != 0)
								{
									NativeMethods.SendMessage(intPtr, 257, bufferedKey.KeyUp.WParam, bufferedKey.KeyUp.LParam);
								}
							}
							else
							{
								NativeMethods.SendMessage(intPtr, bufferedKey.KeyChar.Msg, bufferedKey.KeyChar.WParam, bufferedKey.KeyChar.LParam);
							}
						}
					}
					this.bufferedChars.Clear();
					return;
				}
				}
			}
			this.lastKeyDown.Msg = 0;
			goto IL_0354;
			IL_025D:
			ISelectionService selectionService = (ISelectionService)this.sp.GetService(typeof(ISelectionService));
			if ((Control.ModifierKeys & (Keys.Control | Keys.Alt)) == Keys.None)
			{
				if (this.bufferedChars == null)
				{
					this.bufferedChars = new ArrayList();
				}
				this.bufferedChars.Add(new PbrsForward.BufferedKey(this.lastKeyDown, m, this.lastKeyDown));
				if (!this.ignoreMessages && this.MenuCommandService != null)
				{
					this.postCharMessage = true;
					this.MenuCommandService.GlobalInvoke(StandardCommands.PropertiesWindow);
				}
				else if (this.ignoreMessages && m.Msg != 271 && this.InSituSupportService != null)
				{
					this.postCharMessage = true;
					this.InSituSupportService.HandleKeyChar();
				}
				if (this.postCharMessage)
				{
					return;
				}
			}
			IL_0354:
			if (this.oldTarget != null)
			{
				this.oldTarget.OnMessage(ref m);
			}
		}

		// Token: 0x0400139F RID: 5023
		private const int WM_PRIVATE_POSTCHAR = 6552;

		// Token: 0x040013A0 RID: 5024
		private Control target;

		// Token: 0x040013A1 RID: 5025
		private IWindowTarget oldTarget;

		// Token: 0x040013A2 RID: 5026
		private Message lastKeyDown;

		// Token: 0x040013A3 RID: 5027
		private ArrayList bufferedChars;

		// Token: 0x040013A4 RID: 5028
		private bool postCharMessage;

		// Token: 0x040013A5 RID: 5029
		private IMenuCommandService menuCommandSvc;

		// Token: 0x040013A6 RID: 5030
		private IServiceProvider sp;

		// Token: 0x040013A7 RID: 5031
		private bool ignoreMessages;

		// Token: 0x02000279 RID: 633
		private struct BufferedKey
		{
			// Token: 0x060017B5 RID: 6069 RVA: 0x0007B830 File Offset: 0x0007A830
			public BufferedKey(Message keyDown, Message keyChar, Message keyUp)
			{
				this.KeyChar = keyChar;
				this.KeyDown = keyDown;
				this.KeyUp = keyUp;
			}

			// Token: 0x040013A8 RID: 5032
			public readonly Message KeyDown;

			// Token: 0x040013A9 RID: 5033
			public readonly Message KeyUp;

			// Token: 0x040013AA RID: 5034
			public readonly Message KeyChar;
		}
	}
}
