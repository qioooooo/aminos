using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Design;

namespace System.Windows.Forms.Design
{
	internal class PbrsForward : IWindowTarget
	{
		public PbrsForward(Control target, IServiceProvider sp)
		{
			this.target = target;
			this.oldTarget = target.WindowTarget;
			this.sp = sp;
			target.WindowTarget = this;
		}

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

		private ISupportInSituService InSituSupportService
		{
			get
			{
				return (ISupportInSituService)this.sp.GetService(typeof(ISupportInSituService));
			}
		}

		public void Dispose()
		{
			this.target.WindowTarget = this.oldTarget;
		}

		void IWindowTarget.OnHandleChange(IntPtr newHandle)
		{
		}

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

		private const int WM_PRIVATE_POSTCHAR = 6552;

		private Control target;

		private IWindowTarget oldTarget;

		private Message lastKeyDown;

		private ArrayList bufferedChars;

		private bool postCharMessage;

		private IMenuCommandService menuCommandSvc;

		private IServiceProvider sp;

		private bool ignoreMessages;

		private struct BufferedKey
		{
			public BufferedKey(Message keyDown, Message keyChar, Message keyUp)
			{
				this.KeyChar = keyChar;
				this.KeyDown = keyDown;
				this.KeyUp = keyUp;
			}

			public readonly Message KeyDown;

			public readonly Message KeyUp;

			public readonly Message KeyChar;
		}
	}
}
