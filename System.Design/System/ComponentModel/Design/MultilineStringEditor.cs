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
	public sealed class MultilineStringEditor : UITypeEditor
	{
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

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.DropDown;
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return false;
		}

		private MultilineStringEditor.MultilineStringEditorUI _editorUI;

		private class MultilineStringEditorUI : RichTextBox
		{
			internal MultilineStringEditorUI()
			{
				this.InitializeComponent();
				this._watermarkFormat = new StringFormat();
				this._watermarkFormat.Alignment = StringAlignment.Center;
				this._watermarkFormat.LineAlignment = StringAlignment.Center;
				this._fallbackFonts = new Hashtable(2);
			}

			private void InitializeComponent()
			{
				base.RichTextShortcutsEnabled = false;
				base.WordWrap = false;
				base.BorderStyle = BorderStyle.None;
				this.Multiline = true;
				base.ScrollBars = RichTextBoxScrollBars.Both;
				base.DetectUrls = false;
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing && this._watermarkBrush != null)
				{
					this._watermarkBrush.Dispose();
					this._watermarkBrush = null;
				}
				base.Dispose(disposing);
			}

			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			protected override object CreateRichEditOleCallback()
			{
				return new MultilineStringEditor.OleCallback(this);
			}

			protected override bool IsInputKey(Keys keyData)
			{
				return ((keyData & Keys.KeyCode) == Keys.Return && this.Multiline && (keyData & Keys.Alt) == Keys.None) || base.IsInputKey(keyData);
			}

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

			internal object Value
			{
				get
				{
					return this.Text;
				}
			}

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

			internal bool EndEdit()
			{
				this._editing = false;
				this._editorService = null;
				this._ctrlEnterPressed = false;
				this.Text = null;
				return !this._escapePressed;
			}

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

			protected override void OnContentsResized(ContentsResizedEventArgs e)
			{
				this._contentsResizedRaised = true;
				this.ResizeToContent();
				base.OnContentsResized(e);
			}

			protected override void OnTextChanged(EventArgs e)
			{
				if (!this._contentsResizedRaised)
				{
					this.ResizeToContent();
				}
				this._contentsResizedRaised = false;
				base.OnTextChanged(e);
			}

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

			private bool ShouldShowWatermark
			{
				get
				{
					return this.Text.Length == 0 && this.WatermarkSize.Width < base.ClientSize.Width;
				}
			}

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

			private const int _caretPadding = 3;

			private const int _workAreaPadding = 16;

			private IWindowsFormsEditorService _editorService;

			private bool _editing;

			private bool _escapePressed;

			private bool _ctrlEnterPressed;

			private SolidBrush _watermarkBrush;

			private Hashtable _fallbackFonts;

			private readonly StringFormat _watermarkFormat;

			private bool _contentsResizedRaised;

			private Size _minimumSize = Size.Empty;

			private Size _watermarkSize = Size.Empty;
		}

		private class OleCallback : UnsafeNativeMethods.IRichTextBoxOleCallback
		{
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

			internal OleCallback(RichTextBox owner)
			{
				this.owner = owner;
			}

			public int GetNewStorage(out UnsafeNativeMethods.IStorage storage)
			{
				UnsafeNativeMethods.ILockBytes lockBytes = UnsafeNativeMethods.CreateILockBytesOnHGlobal(NativeMethods.NullHandleRef, true);
				storage = UnsafeNativeMethods.StgCreateDocfileOnILockBytes(lockBytes, 4114, 0);
				return 0;
			}

			public int GetInPlaceContext(IntPtr lplpFrame, IntPtr lplpDoc, IntPtr lpFrameInfo)
			{
				return -2147467263;
			}

			public int ShowContainerUI(int fShow)
			{
				return 0;
			}

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

			public int DeleteObject(IntPtr lpoleobj)
			{
				return 0;
			}

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

			public int ContextSensitiveHelp(int fEnterMode)
			{
				return -2147467263;
			}

			public int GetClipboardData(NativeMethods.CHARRANGE lpchrg, int reco, IntPtr lplpdataobj)
			{
				return -2147467263;
			}

			public int GetDragDropEffect(bool fDrag, int grfKeyState, ref int pdwEffect)
			{
				pdwEffect = 0;
				return 0;
			}

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

			private RichTextBox owner;

			private bool unrestricted;

			private static TraceSwitch richTextDbg;
		}
	}
}
