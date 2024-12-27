using System;
using System.Collections;
using System.Design;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Permissions;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Microsoft.Win32;

namespace System.ComponentModel.Design
{
	// Token: 0x02000137 RID: 311
	public sealed class MultilineStringEditor : UITypeEditor
	{
		// Token: 0x06000C18 RID: 3096 RVA: 0x0002F6EC File Offset: 0x0002E6EC
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this._editorUI == null)
					{
						this._editorUI = new MultilineStringEditor.MultilineStringEditorUI();
					}
					this._editorUI.BeginEdit(windowsFormsEditorService, value);
					windowsFormsEditorService.DropDownControl(this._editorUI);
					object value2 = this._editorUI.Value;
					if (this._editorUI.EndEdit())
					{
						value = value2;
					}
				}
			}
			return value;
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0002F75E File Offset: 0x0002E75E
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0002F761 File Offset: 0x0002E761
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		// Token: 0x04000E82 RID: 3714
		private MultilineStringEditor.MultilineStringEditorUI _editorUI;

		// Token: 0x02000138 RID: 312
		private class MultilineStringEditorUI : RichTextBox
		{
			// Token: 0x06000C1C RID: 3100 RVA: 0x0002F76C File Offset: 0x0002E76C
			internal MultilineStringEditorUI()
			{
				this.InitializeComponent();
				this._watermarkFormat = new StringFormat();
				this._watermarkFormat.Alignment = StringAlignment.Center;
				this._watermarkFormat.LineAlignment = StringAlignment.Center;
				this._fallbackFonts = new Hashtable(2);
			}

			// Token: 0x06000C1D RID: 3101 RVA: 0x0002F7CA File Offset: 0x0002E7CA
			private void InitializeComponent()
			{
				base.RichTextShortcutsEnabled = false;
				base.WordWrap = false;
				base.BorderStyle = BorderStyle.None;
				this.Multiline = true;
				base.ScrollBars = RichTextBoxScrollBars.Both;
				base.DetectUrls = false;
			}

			// Token: 0x06000C1E RID: 3102 RVA: 0x0002F7F6 File Offset: 0x0002E7F6
			protected override void Dispose(bool disposing)
			{
				if (disposing && this._watermarkBrush != null)
				{
					this._watermarkBrush.Dispose();
					this._watermarkBrush = null;
				}
				base.Dispose(disposing);
			}

			// Token: 0x06000C1F RID: 3103 RVA: 0x0002F81C File Offset: 0x0002E81C
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override object CreateRichEditOleCallback()
			{
				return new MultilineStringEditor.OleCallback(this);
			}

			// Token: 0x06000C20 RID: 3104 RVA: 0x0002F824 File Offset: 0x0002E824
			protected override bool IsInputKey(Keys keyData)
			{
				return ((keyData & Keys.KeyCode) == Keys.Return && this.Multiline && (keyData & Keys.Alt) == Keys.None) || base.IsInputKey(keyData);
			}

			// Token: 0x06000C21 RID: 3105 RVA: 0x0002F84C File Offset: 0x0002E84C
			protected override bool ProcessDialogKey(Keys keyData)
			{
				if ((keyData & (Keys.Shift | Keys.Alt)) == Keys.None)
				{
					Keys keys = keyData & Keys.KeyCode;
					if (keys == Keys.Escape && (keyData & Keys.Control) == Keys.None)
					{
						this._escapePressed = true;
					}
				}
				return base.ProcessDialogKey(keyData);
			}

			// Token: 0x06000C22 RID: 3106 RVA: 0x0002F888 File Offset: 0x0002E888
			protected override void OnKeyDown(KeyEventArgs e)
			{
				if (this.ShouldShowWatermark)
				{
					base.Invalidate();
				}
				if (e.Control && e.KeyCode == Keys.Return && e.Modifiers == Keys.Control)
				{
					this._editorService.CloseDropDown();
					this._ctrlEnterPressed = true;
				}
			}

			// Token: 0x170001B9 RID: 441
			// (get) Token: 0x06000C23 RID: 3107 RVA: 0x0002F8D4 File Offset: 0x0002E8D4
			internal object Value
			{
				get
				{
					return this.Text;
				}
			}

			// Token: 0x06000C24 RID: 3108 RVA: 0x0002F8DC File Offset: 0x0002E8DC
			internal void BeginEdit(IWindowsFormsEditorService editorService, object value)
			{
				this._editing = true;
				this._editorService = editorService;
				this._minimumSize = Size.Empty;
				this._watermarkSize = Size.Empty;
				this._escapePressed = false;
				this._ctrlEnterPressed = false;
				this.Text = (string)value;
			}

			// Token: 0x06000C25 RID: 3109 RVA: 0x0002F91C File Offset: 0x0002E91C
			internal bool EndEdit()
			{
				this._editing = false;
				this._editorService = null;
				this._ctrlEnterPressed = false;
				this.Text = null;
				return !this._escapePressed;
			}

			// Token: 0x06000C26 RID: 3110 RVA: 0x0002F944 File Offset: 0x0002E944
			private void ResizeToContent()
			{
				if (!base.Visible)
				{
					return;
				}
				Size contentSize = this.ContentSize;
				contentSize.Width += SystemInformation.VerticalScrollBarWidth;
				contentSize.Width = Math.Max(contentSize.Width, this.MinimumSize.Width);
				Rectangle workingArea = Screen.GetWorkingArea(this);
				int num = base.PointToScreen(base.Location).X - workingArea.Left;
				int num2 = Math.Min(contentSize.Width - base.ClientSize.Width, num);
				base.ClientSize = new Size(base.ClientSize.Width + num2, this.MinimumSize.Height);
			}

			// Token: 0x170001BA RID: 442
			// (get) Token: 0x06000C27 RID: 3111 RVA: 0x0002FA08 File Offset: 0x0002EA08
			private Size ContentSize
			{
				get
				{
					NativeMethods.RECT rect = default(NativeMethods.RECT);
					HandleRef handleRef = new HandleRef(null, UnsafeNativeMethods.GetDC(NativeMethods.NullHandleRef));
					HandleRef handleRef2 = new HandleRef(null, this.Font.ToHfont());
					HandleRef handleRef3 = new HandleRef(null, SafeNativeMethods.SelectObject(handleRef, handleRef2));
					try
					{
						SafeNativeMethods.DrawText(handleRef, this.Text, this.Text.Length, ref rect, 1024);
					}
					finally
					{
						NativeMethods.ExternalDeleteObject(handleRef2);
						SafeNativeMethods.SelectObject(handleRef, handleRef3);
						UnsafeNativeMethods.ReleaseDC(NativeMethods.NullHandleRef, handleRef);
					}
					return new Size(rect.right - rect.left + 3, rect.bottom - rect.top);
				}
			}

			// Token: 0x06000C28 RID: 3112 RVA: 0x0002FAC4 File Offset: 0x0002EAC4
			protected override void OnContentsResized(ContentsResizedEventArgs e)
			{
				this._contentsResizedRaised = true;
				this.ResizeToContent();
				base.OnContentsResized(e);
			}

			// Token: 0x06000C29 RID: 3113 RVA: 0x0002FADA File Offset: 0x0002EADA
			protected override void OnTextChanged(EventArgs e)
			{
				if (!this._contentsResizedRaised)
				{
					this.ResizeToContent();
				}
				this._contentsResizedRaised = false;
				base.OnTextChanged(e);
			}

			// Token: 0x06000C2A RID: 3114 RVA: 0x0002FAF8 File Offset: 0x0002EAF8
			protected override void OnVisibleChanged(EventArgs e)
			{
				if (base.Visible)
				{
					this.ProcessSurrogateFonts(0, this.Text.Length);
					base.Select(this.Text.Length, 0);
				}
				this.ResizeToContent();
				base.OnVisibleChanged(e);
			}

			// Token: 0x170001BB RID: 443
			// (get) Token: 0x06000C2B RID: 3115 RVA: 0x0002FB34 File Offset: 0x0002EB34
			public override Size MinimumSize
			{
				get
				{
					if (this._minimumSize == Size.Empty)
					{
						Rectangle workingArea = Screen.GetWorkingArea(this);
						this._minimumSize = new Size((int)Math.Min(Math.Ceiling((double)this.WatermarkSize.Width * 1.75), (double)(workingArea.Width / 4)), Math.Min(this.Font.Height * 10, workingArea.Height / 4));
					}
					return this._minimumSize;
				}
			}

			// Token: 0x170001BC RID: 444
			// (get) Token: 0x06000C2C RID: 3116 RVA: 0x0002FBB5 File Offset: 0x0002EBB5
			// (set) Token: 0x06000C2D RID: 3117 RVA: 0x0002FBBD File Offset: 0x0002EBBD
			public override Font Font
			{
				get
				{
					return base.Font;
				}
				set
				{
				}
			}

			// Token: 0x06000C2E RID: 3118 RVA: 0x0002FBC0 File Offset: 0x0002EBC0
			public void ProcessSurrogateFonts(int start, int length)
			{
				string text = this.Text;
				if (text == null)
				{
					return;
				}
				int[] array = StringInfo.ParseCombiningCharacters(text);
				if (array.Length != text.Length)
				{
					for (int i = 0; i < array.Length; i++)
					{
						if (array[i] >= start && array[i] < start + length)
						{
							char c = text[array[i]];
							char c2 = '\0';
							if (array[i] + 1 < text.Length)
							{
								c2 = text[array[i] + 1];
							}
							if (c >= '\ud800' && c <= '\udbff' && c2 >= '\udc00' && c2 <= '\udfff')
							{
								int num = (int)(c / '@' - '\u0360' + '\u0001');
								Font font = this._fallbackFonts[num] as Font;
								if (font == null)
								{
									using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion\\LanguagePack\\SurrogateFallback"))
									{
										if (registryKey != null)
										{
											string text2 = (string)registryKey.GetValue("Plane" + num);
											if (!string.IsNullOrEmpty(text2))
											{
												font = new Font(text2, base.Font.Size, base.Font.Style);
											}
											this._fallbackFonts[num] = font;
										}
									}
								}
								if (font != null)
								{
									int num2 = ((i == array.Length - 1) ? (text.Length - array[i]) : (array[i + 1] - array[i]));
									base.Select(array[i], num2);
									base.SelectionFont = font;
								}
							}
						}
					}
				}
			}

			// Token: 0x170001BD RID: 445
			// (get) Token: 0x06000C2F RID: 3119 RVA: 0x0002FD5C File Offset: 0x0002ED5C
			// (set) Token: 0x06000C30 RID: 3120 RVA: 0x0002FDD5 File Offset: 0x0002EDD5
			public override string Text
			{
				get
				{
					if (!base.IsHandleCreated)
					{
						return "";
					}
					int windowTextLength = SafeNativeMethods.GetWindowTextLength(new HandleRef(this, base.Handle));
					StringBuilder stringBuilder = new StringBuilder(windowTextLength + 1);
					UnsafeNativeMethods.GetWindowText(new HandleRef(this, base.Handle), stringBuilder, stringBuilder.Capacity);
					if (!this._ctrlEnterPressed)
					{
						return stringBuilder.ToString();
					}
					string text = stringBuilder.ToString();
					int num = text.LastIndexOf("\r\n");
					return text.Remove(num, 2);
				}
				set
				{
					base.Text = value;
				}
			}

			// Token: 0x170001BE RID: 446
			// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0002FDE0 File Offset: 0x0002EDE0
			private Size WatermarkSize
			{
				get
				{
					if (this._watermarkSize == Size.Empty)
					{
						SizeF sizeF;
						using (Graphics graphics = base.CreateGraphics())
						{
							sizeF = graphics.MeasureString(SR.GetString("MultilineStringEditorWatermark"), this.Font);
						}
						this._watermarkSize = new Size((int)Math.Ceiling((double)sizeF.Width), (int)Math.Ceiling((double)sizeF.Height));
					}
					return this._watermarkSize;
				}
			}

			// Token: 0x170001BF RID: 447
			// (get) Token: 0x06000C32 RID: 3122 RVA: 0x0002FE68 File Offset: 0x0002EE68
			private bool ShouldShowWatermark
			{
				get
				{
					return this.Text.Length == 0 && this.WatermarkSize.Width < base.ClientSize.Width;
				}
			}

			// Token: 0x170001C0 RID: 448
			// (get) Token: 0x06000C33 RID: 3123 RVA: 0x0002FEA4 File Offset: 0x0002EEA4
			private Brush WatermarkBrush
			{
				get
				{
					if (this._watermarkBrush == null)
					{
						Color window = SystemColors.Window;
						Color windowText = SystemColors.WindowText;
						Color color = Color.FromArgb((int)((short)((double)windowText.R * 0.3 + (double)window.R * 0.7)), (int)((short)((double)windowText.G * 0.3 + (double)window.G * 0.7)), (int)((short)((double)windowText.B * 0.3 + (double)window.B * 0.7)));
						this._watermarkBrush = new SolidBrush(color);
					}
					return this._watermarkBrush;
				}
			}

			// Token: 0x06000C34 RID: 3124 RVA: 0x0002FF54 File Offset: 0x0002EF54
			protected override void WndProc(ref Message m)
			{
				base.WndProc(ref m);
				int msg = m.Msg;
				if (msg != 15)
				{
					return;
				}
				if (this.ShouldShowWatermark)
				{
					using (Graphics graphics = base.CreateGraphics())
					{
						graphics.DrawString(SR.GetString("MultilineStringEditorWatermark"), this.Font, this.WatermarkBrush, new RectangleF(0f, 0f, (float)base.ClientSize.Width, (float)base.ClientSize.Height), this._watermarkFormat);
					}
				}
			}

			// Token: 0x04000E83 RID: 3715
			private const int _caretPadding = 3;

			// Token: 0x04000E84 RID: 3716
			private const int _workAreaPadding = 16;

			// Token: 0x04000E85 RID: 3717
			private IWindowsFormsEditorService _editorService;

			// Token: 0x04000E86 RID: 3718
			private bool _editing;

			// Token: 0x04000E87 RID: 3719
			private bool _escapePressed;

			// Token: 0x04000E88 RID: 3720
			private bool _ctrlEnterPressed;

			// Token: 0x04000E89 RID: 3721
			private SolidBrush _watermarkBrush;

			// Token: 0x04000E8A RID: 3722
			private Hashtable _fallbackFonts;

			// Token: 0x04000E8B RID: 3723
			private readonly StringFormat _watermarkFormat;

			// Token: 0x04000E8C RID: 3724
			private bool _contentsResizedRaised;

			// Token: 0x04000E8D RID: 3725
			private Size _minimumSize = Size.Empty;

			// Token: 0x04000E8E RID: 3726
			private Size _watermarkSize = Size.Empty;
		}

		// Token: 0x02000139 RID: 313
		private class OleCallback : UnsafeNativeMethods.IRichTextBoxOleCallback
		{
			// Token: 0x170001C1 RID: 449
			// (get) Token: 0x06000C35 RID: 3125 RVA: 0x0002FFF0 File Offset: 0x0002EFF0
			private static TraceSwitch RichTextDbg
			{
				get
				{
					if (MultilineStringEditor.OleCallback.richTextDbg == null)
					{
						MultilineStringEditor.OleCallback.richTextDbg = new TraceSwitch("RichTextDbg", "Debug info about RichTextBox");
					}
					return MultilineStringEditor.OleCallback.richTextDbg;
				}
			}

			// Token: 0x06000C36 RID: 3126 RVA: 0x00030012 File Offset: 0x0002F012
			internal OleCallback(RichTextBox owner)
			{
				this.owner = owner;
			}

			// Token: 0x06000C37 RID: 3127 RVA: 0x00030024 File Offset: 0x0002F024
			public int GetNewStorage(out UnsafeNativeMethods.IStorage storage)
			{
				UnsafeNativeMethods.ILockBytes lockBytes = UnsafeNativeMethods.CreateILockBytesOnHGlobal(NativeMethods.NullHandleRef, true);
				storage = UnsafeNativeMethods.StgCreateDocfileOnILockBytes(lockBytes, 4114, 0);
				return 0;
			}

			// Token: 0x06000C38 RID: 3128 RVA: 0x0003004C File Offset: 0x0002F04C
			public int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo)
			{
				return -2147467263;
			}

			// Token: 0x06000C39 RID: 3129 RVA: 0x00030053 File Offset: 0x0002F053
			public int ShowContainerUI(int fShow)
			{
				return 0;
			}

			// Token: 0x06000C3A RID: 3130 RVA: 0x00030058 File Offset: 0x0002F058
			public int QueryInsertObject(ref Guid lpclsid, IntPtr lpstg, int cp)
			{
				if (this.unrestricted)
				{
					return 0;
				}
				Guid guid = default(Guid);
				int num = UnsafeNativeMethods.ReadClassStg(new HandleRef(null, lpstg), ref guid);
				if (!NativeMethods.Succeeded(num))
				{
					return 1;
				}
				if (guid == Guid.Empty)
				{
					guid = lpclsid;
				}
				string text;
				if ((text = guid.ToString().ToUpper(CultureInfo.InvariantCulture)) != null && (text == "00000315-0000-0000-C000-000000000046" || text == "00000316-0000-0000-C000-000000000046" || text == "00000319-0000-0000-C000-000000000046" || text == "0003000A-0000-0000-C000-000000000046"))
				{
					return 0;
				}
				return 1;
			}

			// Token: 0x06000C3B RID: 3131 RVA: 0x000300F6 File Offset: 0x0002F0F6
			public int DeleteObject(IntPtr lpoleobj)
			{
				return 0;
			}

			// Token: 0x06000C3C RID: 3132 RVA: 0x000300FC File Offset: 0x0002F0FC
			public int QueryAcceptData(global::System.Runtime.InteropServices.ComTypes.IDataObject lpdataobj, IntPtr lpcfFormat, int reco, int fReally, IntPtr hMetaPict)
			{
				if (reco != 0)
				{
					return -2147467263;
				}
				DataObject dataObject = new DataObject(lpdataobj);
				if (dataObject != null && (dataObject.GetDataPresent(DataFormats.Text) || dataObject.GetDataPresent(DataFormats.UnicodeText)))
				{
					return 0;
				}
				return -2147467259;
			}

			// Token: 0x06000C3D RID: 3133 RVA: 0x0003013D File Offset: 0x0002F13D
			public int ContextSensitiveHelp(int fEnterMode)
			{
				return -2147467263;
			}

			// Token: 0x06000C3E RID: 3134 RVA: 0x00030144 File Offset: 0x0002F144
			public int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj)
			{
				return -2147467263;
			}

			// Token: 0x06000C3F RID: 3135 RVA: 0x0003014B File Offset: 0x0002F14B
			public int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect)
			{
				pdwEffect = 0;
				return 0;
			}

			// Token: 0x06000C40 RID: 3136 RVA: 0x00030154 File Offset: 0x0002F154
			public int GetContextMenu(short seltype, IntPtr lpoleobj, NativeMethods.CHARRANGE lpchrg, out IntPtr hmenu)
			{
				ContextMenu contextMenu = new TextBox
				{
					Visible = true
				}.ContextMenu;
				if (contextMenu == null || !this.owner.ShortcutsEnabled)
				{
					hmenu = IntPtr.Zero;
				}
				else
				{
					hmenu = contextMenu.Handle;
				}
				return 0;
			}

			// Token: 0x04000E8F RID: 3727
			private RichTextBox owner;

			// Token: 0x04000E90 RID: 3728
			private bool unrestricted;

			// Token: 0x04000E91 RID: 3729
			private static TraceSwitch richTextDbg;
		}
	}
}
