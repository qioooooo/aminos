using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	// Token: 0x020003E2 RID: 994
	[DefaultProperty("FileName")]
	[DefaultEvent("FileOk")]
	public abstract class FileDialog : CommonDialog
	{
		// Token: 0x06003B7D RID: 15229 RVA: 0x000D7EFD File Offset: 0x000D6EFD
		internal FileDialog()
		{
			this.Reset();
		}

		// Token: 0x17000B51 RID: 2897
		// (get) Token: 0x06003B7E RID: 15230 RVA: 0x000D7F1D File Offset: 0x000D6F1D
		// (set) Token: 0x06003B7F RID: 15231 RVA: 0x000D7F2A File Offset: 0x000D6F2A
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		[SRDescription("FDaddExtensionDescr")]
		public bool AddExtension
		{
			get
			{
				return this.GetOption(int.MinValue);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(int.MinValue, value);
			}
		}

		// Token: 0x17000B52 RID: 2898
		// (get) Token: 0x06003B80 RID: 15232 RVA: 0x000D7F42 File Offset: 0x000D6F42
		// (set) Token: 0x06003B81 RID: 15233 RVA: 0x000D7F4F File Offset: 0x000D6F4F
		[SRDescription("FDcheckFileExistsDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public virtual bool CheckFileExists
		{
			get
			{
				return this.GetOption(4096);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(4096, value);
			}
		}

		// Token: 0x17000B53 RID: 2899
		// (get) Token: 0x06003B82 RID: 15234 RVA: 0x000D7F67 File Offset: 0x000D6F67
		// (set) Token: 0x06003B83 RID: 15235 RVA: 0x000D7F74 File Offset: 0x000D6F74
		[SRDescription("FDcheckPathExistsDescr")]
		[DefaultValue(true)]
		[SRCategory("CatBehavior")]
		public bool CheckPathExists
		{
			get
			{
				return this.GetOption(2048);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(2048, value);
			}
		}

		// Token: 0x17000B54 RID: 2900
		// (get) Token: 0x06003B84 RID: 15236 RVA: 0x000D7F8C File Offset: 0x000D6F8C
		// (set) Token: 0x06003B85 RID: 15237 RVA: 0x000D7FA2 File Offset: 0x000D6FA2
		[SRCategory("CatBehavior")]
		[DefaultValue("")]
		[SRDescription("FDdefaultExtDescr")]
		public string DefaultExt
		{
			get
			{
				if (this.defaultExt != null)
				{
					return this.defaultExt;
				}
				return "";
			}
			set
			{
				if (value != null)
				{
					if (value.StartsWith("."))
					{
						value = value.Substring(1);
					}
					else if (value.Length == 0)
					{
						value = null;
					}
				}
				this.defaultExt = value;
			}
		}

		// Token: 0x17000B55 RID: 2901
		// (get) Token: 0x06003B86 RID: 15238 RVA: 0x000D7FD1 File Offset: 0x000D6FD1
		// (set) Token: 0x06003B87 RID: 15239 RVA: 0x000D7FE1 File Offset: 0x000D6FE1
		[SRCategory("CatBehavior")]
		[SRDescription("FDdereferenceLinksDescr")]
		[DefaultValue(true)]
		public bool DereferenceLinks
		{
			get
			{
				return !this.GetOption(1048576);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(1048576, !value);
			}
		}

		// Token: 0x17000B56 RID: 2902
		// (get) Token: 0x06003B88 RID: 15240 RVA: 0x000D7FFC File Offset: 0x000D6FFC
		internal string DialogCaption
		{
			get
			{
				int windowTextLength = SafeNativeMethods.GetWindowTextLength(new HandleRef(this, this.dialogHWnd));
				StringBuilder stringBuilder = new StringBuilder(windowTextLength + 1);
				UnsafeNativeMethods.GetWindowText(new HandleRef(this, this.dialogHWnd), stringBuilder, stringBuilder.Capacity);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x17000B57 RID: 2903
		// (get) Token: 0x06003B89 RID: 15241 RVA: 0x000D8044 File Offset: 0x000D7044
		// (set) Token: 0x06003B8A RID: 15242 RVA: 0x000D8094 File Offset: 0x000D7094
		[DefaultValue("")]
		[SRDescription("FDfileNameDescr")]
		[SRCategory("CatData")]
		public string FileName
		{
			get
			{
				if (this.fileNames == null)
				{
					return "";
				}
				if (this.fileNames[0].Length > 0)
				{
					if (this.securityCheckFileNames)
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.AllAccess, this.fileNames[0]);
					}
					return this.fileNames[0];
				}
				return "";
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				if (value == null)
				{
					this.fileNames = null;
				}
				else
				{
					this.fileNames = new string[] { value };
				}
				this.securityCheckFileNames = false;
			}
		}

		// Token: 0x17000B58 RID: 2904
		// (get) Token: 0x06003B8B RID: 15243 RVA: 0x000D80D0 File Offset: 0x000D70D0
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[SRDescription("FDFileNamesDescr")]
		public string[] FileNames
		{
			get
			{
				string[] fileNamesInternal = this.FileNamesInternal;
				if (this.securityCheckFileNames)
				{
					foreach (string text in fileNamesInternal)
					{
						IntSecurity.DemandFileIO(FileIOPermissionAccess.AllAccess, text);
					}
				}
				return fileNamesInternal;
			}
		}

		// Token: 0x17000B59 RID: 2905
		// (get) Token: 0x06003B8C RID: 15244 RVA: 0x000D8109 File Offset: 0x000D7109
		internal string[] FileNamesInternal
		{
			get
			{
				if (this.fileNames == null)
				{
					return new string[0];
				}
				return (string[])this.fileNames.Clone();
			}
		}

		// Token: 0x17000B5A RID: 2906
		// (get) Token: 0x06003B8D RID: 15245 RVA: 0x000D812A File Offset: 0x000D712A
		// (set) Token: 0x06003B8E RID: 15246 RVA: 0x000D8140 File Offset: 0x000D7140
		[SRDescription("FDfilterDescr")]
		[DefaultValue("")]
		[Localizable(true)]
		[SRCategory("CatBehavior")]
		public string Filter
		{
			get
			{
				if (this.filter != null)
				{
					return this.filter;
				}
				return "";
			}
			set
			{
				if (value != this.filter)
				{
					if (value != null && value.Length > 0)
					{
						string[] array = value.Split(new char[] { '|' });
						if (array == null || array.Length % 2 != 0)
						{
							throw new ArgumentException(SR.GetString("FileDialogInvalidFilter"));
						}
					}
					else
					{
						value = null;
					}
					this.filter = value;
				}
			}
		}

		// Token: 0x17000B5B RID: 2907
		// (get) Token: 0x06003B8F RID: 15247 RVA: 0x000D81A0 File Offset: 0x000D71A0
		private string[] FilterExtensions
		{
			get
			{
				string text = this.filter;
				ArrayList arrayList = new ArrayList();
				if (this.defaultExt != null)
				{
					arrayList.Add(this.defaultExt);
				}
				if (text != null)
				{
					string[] array = text.Split(new char[] { '|' });
					if (this.filterIndex * 2 - 1 >= array.Length)
					{
						throw new InvalidOperationException(SR.GetString("FileDialogInvalidFilterIndex"));
					}
					if (this.filterIndex > 0)
					{
						string[] array2 = array[this.filterIndex * 2 - 1].Split(new char[] { ';' });
						foreach (string text2 in array2)
						{
							int num = (this.supportMultiDottedExtensions ? text2.IndexOf('.') : text2.LastIndexOf('.'));
							if (num >= 0)
							{
								arrayList.Add(text2.Substring(num + 1, text2.Length - (num + 1)));
							}
						}
					}
				}
				string[] array4 = new string[arrayList.Count];
				arrayList.CopyTo(array4, 0);
				return array4;
			}
		}

		// Token: 0x17000B5C RID: 2908
		// (get) Token: 0x06003B90 RID: 15248 RVA: 0x000D82AF File Offset: 0x000D72AF
		// (set) Token: 0x06003B91 RID: 15249 RVA: 0x000D82B7 File Offset: 0x000D72B7
		[DefaultValue(1)]
		[SRDescription("FDfilterIndexDescr")]
		[SRCategory("CatBehavior")]
		public int FilterIndex
		{
			get
			{
				return this.filterIndex;
			}
			set
			{
				this.filterIndex = value;
			}
		}

		// Token: 0x17000B5D RID: 2909
		// (get) Token: 0x06003B92 RID: 15250 RVA: 0x000D82C0 File Offset: 0x000D72C0
		// (set) Token: 0x06003B93 RID: 15251 RVA: 0x000D82D6 File Offset: 0x000D72D6
		[DefaultValue("")]
		[SRDescription("FDinitialDirDescr")]
		[SRCategory("CatData")]
		public string InitialDirectory
		{
			get
			{
				if (this.initialDir != null)
				{
					return this.initialDir;
				}
				return "";
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.initialDir = value;
			}
		}

		// Token: 0x17000B5E RID: 2910
		// (get) Token: 0x06003B94 RID: 15252 RVA: 0x000D82E9 File Offset: 0x000D72E9
		protected virtual IntPtr Instance
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return UnsafeNativeMethods.GetModuleHandle(null);
			}
		}

		// Token: 0x17000B5F RID: 2911
		// (get) Token: 0x06003B95 RID: 15253 RVA: 0x000D82F1 File Offset: 0x000D72F1
		protected int Options
		{
			get
			{
				return this.options & 1051421;
			}
		}

		// Token: 0x17000B60 RID: 2912
		// (get) Token: 0x06003B96 RID: 15254 RVA: 0x000D82FF File Offset: 0x000D72FF
		// (set) Token: 0x06003B97 RID: 15255 RVA: 0x000D8308 File Offset: 0x000D7308
		[SRCategory("CatBehavior")]
		[SRDescription("FDrestoreDirectoryDescr")]
		[DefaultValue(false)]
		public bool RestoreDirectory
		{
			get
			{
				return this.GetOption(8);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(8, value);
			}
		}

		// Token: 0x17000B61 RID: 2913
		// (get) Token: 0x06003B98 RID: 15256 RVA: 0x000D831C File Offset: 0x000D731C
		// (set) Token: 0x06003B99 RID: 15257 RVA: 0x000D8326 File Offset: 0x000D7326
		[DefaultValue(false)]
		[SRDescription("FDshowHelpDescr")]
		[SRCategory("CatBehavior")]
		public bool ShowHelp
		{
			get
			{
				return this.GetOption(16);
			}
			set
			{
				this.SetOption(16, value);
			}
		}

		// Token: 0x17000B62 RID: 2914
		// (get) Token: 0x06003B9A RID: 15258 RVA: 0x000D8331 File Offset: 0x000D7331
		// (set) Token: 0x06003B9B RID: 15259 RVA: 0x000D8339 File Offset: 0x000D7339
		[SRDescription("FDsupportMultiDottedExtensionsDescr")]
		[DefaultValue(false)]
		[SRCategory("CatBehavior")]
		public bool SupportMultiDottedExtensions
		{
			get
			{
				return this.supportMultiDottedExtensions;
			}
			set
			{
				this.supportMultiDottedExtensions = value;
			}
		}

		// Token: 0x17000B63 RID: 2915
		// (get) Token: 0x06003B9C RID: 15260 RVA: 0x000D8342 File Offset: 0x000D7342
		// (set) Token: 0x06003B9D RID: 15261 RVA: 0x000D8358 File Offset: 0x000D7358
		[SRCategory("CatAppearance")]
		[DefaultValue("")]
		[Localizable(true)]
		[SRDescription("FDtitleDescr")]
		public string Title
		{
			get
			{
				if (this.title != null)
				{
					return this.title;
				}
				return "";
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.title = value;
			}
		}

		// Token: 0x17000B64 RID: 2916
		// (get) Token: 0x06003B9E RID: 15262 RVA: 0x000D836B File Offset: 0x000D736B
		// (set) Token: 0x06003B9F RID: 15263 RVA: 0x000D837B File Offset: 0x000D737B
		[SRDescription("FDvalidateNamesDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(true)]
		public bool ValidateNames
		{
			get
			{
				return !this.GetOption(256);
			}
			set
			{
				IntSecurity.FileDialogCustomization.Demand();
				this.SetOption(256, !value);
			}
		}

		// Token: 0x140001F0 RID: 496
		// (add) Token: 0x06003BA0 RID: 15264 RVA: 0x000D8396 File Offset: 0x000D7396
		// (remove) Token: 0x06003BA1 RID: 15265 RVA: 0x000D83A9 File Offset: 0x000D73A9
		[SRDescription("FDfileOkDescr")]
		public event CancelEventHandler FileOk
		{
			add
			{
				base.Events.AddHandler(FileDialog.EventFileOk, value);
			}
			remove
			{
				base.Events.RemoveHandler(FileDialog.EventFileOk, value);
			}
		}

		// Token: 0x06003BA2 RID: 15266 RVA: 0x000D83BC File Offset: 0x000D73BC
		private bool DoFileOk(IntPtr lpOFN)
		{
			NativeMethods.OPENFILENAME_I openfilename_I = (NativeMethods.OPENFILENAME_I)UnsafeNativeMethods.PtrToStructure(lpOFN, typeof(NativeMethods.OPENFILENAME_I));
			int num = this.options;
			int num2 = this.filterIndex;
			string[] array = this.fileNames;
			bool flag = this.securityCheckFileNames;
			bool flag2 = false;
			try
			{
				this.options = (this.options & -2) | (openfilename_I.Flags & 1);
				this.filterIndex = openfilename_I.nFilterIndex;
				this.charBuffer.PutCoTaskMem(openfilename_I.lpstrFile);
				this.securityCheckFileNames = true;
				Thread.MemoryBarrier();
				if ((this.options & 512) == 0)
				{
					this.fileNames = new string[] { this.charBuffer.GetString() };
				}
				else
				{
					this.fileNames = this.GetMultiselectFiles(this.charBuffer);
				}
				if (this.ProcessFileNames())
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					if (NativeWindow.WndProcShouldBeDebuggable)
					{
						this.OnFileOk(cancelEventArgs);
						flag2 = !cancelEventArgs.Cancel;
					}
					else
					{
						try
						{
							this.OnFileOk(cancelEventArgs);
							flag2 = !cancelEventArgs.Cancel;
						}
						catch (Exception ex)
						{
							Application.OnThreadException(ex);
						}
					}
				}
			}
			finally
			{
				if (!flag2)
				{
					this.securityCheckFileNames = flag;
					Thread.MemoryBarrier();
					this.fileNames = array;
					this.options = num;
					this.filterIndex = num2;
				}
			}
			return flag2;
		}

		// Token: 0x06003BA3 RID: 15267 RVA: 0x000D8518 File Offset: 0x000D7518
		internal static bool FileExists(string fileName)
		{
			bool flag = false;
			try
			{
				new FileIOPermission(FileIOPermissionAccess.Read, IntSecurity.UnsafeGetFullPath(fileName)).Assert();
				try
				{
					flag = File.Exists(fileName);
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
			catch (PathTooLongException)
			{
			}
			return flag;
		}

		// Token: 0x06003BA4 RID: 15268 RVA: 0x000D856C File Offset: 0x000D756C
		private string[] GetMultiselectFiles(UnsafeNativeMethods.CharBuffer charBuffer)
		{
			string text = charBuffer.GetString();
			string text2 = charBuffer.GetString();
			if (text2.Length == 0)
			{
				return new string[] { text };
			}
			if (text[text.Length - 1] != '\\')
			{
				text += "\\";
			}
			ArrayList arrayList = new ArrayList();
			do
			{
				if (text2[0] != '\\' && (text2.Length <= 3 || text2[1] != ':' || text2[2] != '\\'))
				{
					text2 = text + text2;
				}
				arrayList.Add(text2);
				text2 = charBuffer.GetString();
			}
			while (text2.Length > 0);
			string[] array = new string[arrayList.Count];
			arrayList.CopyTo(array, 0);
			return array;
		}

		// Token: 0x06003BA5 RID: 15269 RVA: 0x000D8622 File Offset: 0x000D7622
		internal bool GetOption(int option)
		{
			return (this.options & option) != 0;
		}

		// Token: 0x06003BA6 RID: 15270 RVA: 0x000D8634 File Offset: 0x000D7634
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		protected override IntPtr HookProc(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			if (msg == 78)
			{
				this.dialogHWnd = UnsafeNativeMethods.GetParent(new HandleRef(null, hWnd));
				try
				{
					UnsafeNativeMethods.OFNOTIFY ofnotify = (UnsafeNativeMethods.OFNOTIFY)UnsafeNativeMethods.PtrToStructure(lparam, typeof(UnsafeNativeMethods.OFNOTIFY));
					switch (ofnotify.hdr_code)
					{
					case -606:
						if (this.ignoreSecondFileOkNotification)
						{
							if (this.okNotificationCount != 0)
							{
								this.ignoreSecondFileOkNotification = false;
								UnsafeNativeMethods.SetWindowLong(new HandleRef(null, hWnd), 0, new HandleRef(null, NativeMethods.InvalidIntPtr));
								return NativeMethods.InvalidIntPtr;
							}
							this.okNotificationCount = 1;
						}
						if (!this.DoFileOk(ofnotify.lpOFN))
						{
							UnsafeNativeMethods.SetWindowLong(new HandleRef(null, hWnd), 0, new HandleRef(null, NativeMethods.InvalidIntPtr));
							return NativeMethods.InvalidIntPtr;
						}
						break;
					case -604:
						this.ignoreSecondFileOkNotification = true;
						this.okNotificationCount = 0;
						break;
					case -602:
					{
						NativeMethods.OPENFILENAME_I openfilename_I = (NativeMethods.OPENFILENAME_I)UnsafeNativeMethods.PtrToStructure(ofnotify.lpOFN, typeof(NativeMethods.OPENFILENAME_I));
						int num = (int)UnsafeNativeMethods.SendMessage(new HandleRef(this, this.dialogHWnd), 1124, IntPtr.Zero, IntPtr.Zero);
						if (num > openfilename_I.nMaxFile)
						{
							try
							{
								int num2 = num + 2048;
								UnsafeNativeMethods.CharBuffer charBuffer = UnsafeNativeMethods.CharBuffer.CreateBuffer(num2);
								IntPtr intPtr = charBuffer.AllocCoTaskMem();
								Marshal.FreeCoTaskMem(openfilename_I.lpstrFile);
								openfilename_I.lpstrFile = intPtr;
								openfilename_I.nMaxFile = num2;
								this.charBuffer = charBuffer;
								Marshal.StructureToPtr(openfilename_I, ofnotify.lpOFN, true);
								Marshal.StructureToPtr(ofnotify, lparam, true);
							}
							catch
							{
							}
						}
						this.ignoreSecondFileOkNotification = false;
						break;
					}
					case -601:
						CommonDialog.MoveToScreenCenter(this.dialogHWnd);
						break;
					}
				}
				catch
				{
					if (this.dialogHWnd != IntPtr.Zero)
					{
						UnsafeNativeMethods.EndDialog(new HandleRef(this, this.dialogHWnd), IntPtr.Zero);
					}
					throw;
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x06003BA7 RID: 15271 RVA: 0x000D884C File Offset: 0x000D784C
		private static string MakeFilterString(string s, bool dereferenceLinks)
		{
			if (s == null || s.Length == 0)
			{
				if (dereferenceLinks && Environment.OSVersion.Version.Major >= 5)
				{
					s = " |*.*";
				}
				else if (s == null)
				{
					return null;
				}
			}
			int length = s.Length;
			char[] array = new char[length + 2];
			s.CopyTo(0, array, 0, length);
			for (int i = 0; i < length; i++)
			{
				if (array[i] == '|')
				{
					array[i] = '\0';
				}
			}
			array[length + 1] = '\0';
			return new string(array);
		}

		// Token: 0x06003BA8 RID: 15272 RVA: 0x000D88C4 File Offset: 0x000D78C4
		protected void OnFileOk(CancelEventArgs e)
		{
			CancelEventHandler cancelEventHandler = (CancelEventHandler)base.Events[FileDialog.EventFileOk];
			if (cancelEventHandler != null)
			{
				cancelEventHandler(this, e);
			}
		}

		// Token: 0x06003BA9 RID: 15273 RVA: 0x000D88F4 File Offset: 0x000D78F4
		private bool ProcessFileNames()
		{
			if ((this.options & 256) == 0)
			{
				string[] filterExtensions = this.FilterExtensions;
				for (int i = 0; i < this.fileNames.Length; i++)
				{
					string text = this.fileNames[i];
					if ((this.options & -2147483648) != 0 && !Path.HasExtension(text))
					{
						bool flag = (this.options & 4096) != 0;
						for (int j = 0; j < filterExtensions.Length; j++)
						{
							string extension = Path.GetExtension(text);
							string text2 = text.Substring(0, text.Length - extension.Length);
							if (filterExtensions[j].IndexOfAny(new char[] { '*', '?' }) == -1)
							{
								text2 = text2 + "." + filterExtensions[j];
							}
							if (!flag || FileDialog.FileExists(text2))
							{
								text = text2;
								break;
							}
						}
						this.fileNames[i] = text;
					}
					if (!this.PromptUserIfAppropriate(text))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06003BAA RID: 15274 RVA: 0x000D89F8 File Offset: 0x000D79F8
		internal bool MessageBoxWithFocusRestore(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
		{
			IntPtr focus = UnsafeNativeMethods.GetFocus();
			bool flag;
			try
			{
				flag = RTLAwareMessageBox.Show(null, message, caption, buttons, icon, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0) == DialogResult.Yes;
			}
			finally
			{
				UnsafeNativeMethods.SetFocus(new HandleRef(null, focus));
			}
			return flag;
		}

		// Token: 0x06003BAB RID: 15275 RVA: 0x000D8A40 File Offset: 0x000D7A40
		private void PromptFileNotFound(string fileName)
		{
			this.MessageBoxWithFocusRestore(SR.GetString("FileDialogFileNotFound", new object[] { fileName }), this.DialogCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		// Token: 0x06003BAC RID: 15276 RVA: 0x000D8A73 File Offset: 0x000D7A73
		internal virtual bool PromptUserIfAppropriate(string fileName)
		{
			if ((this.options & 4096) != 0 && !FileDialog.FileExists(fileName))
			{
				this.PromptFileNotFound(fileName);
				return false;
			}
			return true;
		}

		// Token: 0x06003BAD RID: 15277 RVA: 0x000D8A98 File Offset: 0x000D7A98
		public override void Reset()
		{
			this.options = -2147481596;
			this.title = null;
			this.initialDir = null;
			this.defaultExt = null;
			this.fileNames = null;
			this.filter = null;
			this.filterIndex = 1;
			this.supportMultiDottedExtensions = false;
			this._customPlaces.Clear();
		}

		// Token: 0x06003BAE RID: 15278 RVA: 0x000D8AEC File Offset: 0x000D7AEC
		protected override bool RunDialog(IntPtr hWndOwner)
		{
			if (Control.CheckForIllegalCrossThreadCalls && Application.OleRequired() != ApartmentState.STA)
			{
				throw new ThreadStateException(SR.GetString("DebuggingExceptionOnly", new object[] { SR.GetString("ThreadMustBeSTA") }));
			}
			this.EnsureFileDialogPermission();
			if (this.UseVistaDialogInternal)
			{
				return this.RunDialogVista(hWndOwner);
			}
			return this.RunDialogOld(hWndOwner);
		}

		// Token: 0x06003BAF RID: 15279
		internal abstract void EnsureFileDialogPermission();

		// Token: 0x06003BB0 RID: 15280 RVA: 0x000D8B4C File Offset: 0x000D7B4C
		private bool RunDialogOld(IntPtr hWndOwner)
		{
			NativeMethods.WndProc wndProc = new NativeMethods.WndProc(this.HookProc);
			NativeMethods.OPENFILENAME_I openfilename_I = new NativeMethods.OPENFILENAME_I();
			bool flag;
			try
			{
				this.charBuffer = UnsafeNativeMethods.CharBuffer.CreateBuffer(8192);
				if (this.fileNames != null)
				{
					this.charBuffer.PutString(this.fileNames[0]);
				}
				openfilename_I.lStructSize = Marshal.SizeOf(typeof(NativeMethods.OPENFILENAME_I));
				if (Environment.OSVersion.Platform != PlatformID.Win32NT || Environment.OSVersion.Version.Major < 5)
				{
					openfilename_I.lStructSize = 76;
				}
				openfilename_I.hwndOwner = hWndOwner;
				openfilename_I.hInstance = this.Instance;
				openfilename_I.lpstrFilter = FileDialog.MakeFilterString(this.filter, this.DereferenceLinks);
				openfilename_I.nFilterIndex = this.filterIndex;
				openfilename_I.lpstrFile = this.charBuffer.AllocCoTaskMem();
				openfilename_I.nMaxFile = 8192;
				openfilename_I.lpstrInitialDir = this.initialDir;
				openfilename_I.lpstrTitle = this.title;
				openfilename_I.Flags = this.Options | 8912928;
				openfilename_I.lpfnHook = wndProc;
				openfilename_I.FlagsEx = 16777216;
				if (this.defaultExt != null && this.AddExtension)
				{
					openfilename_I.lpstrDefExt = this.defaultExt;
				}
				flag = this.RunFileDialog(openfilename_I);
			}
			finally
			{
				this.charBuffer = null;
				if (openfilename_I.lpstrFile != IntPtr.Zero)
				{
					Marshal.FreeCoTaskMem(openfilename_I.lpstrFile);
				}
			}
			return flag;
		}

		// Token: 0x06003BB1 RID: 15281
		internal abstract bool RunFileDialog(NativeMethods.OPENFILENAME_I ofn);

		// Token: 0x06003BB2 RID: 15282 RVA: 0x000D8CCC File Offset: 0x000D7CCC
		internal void SetOption(int option, bool value)
		{
			if (value)
			{
				this.options |= option;
				return;
			}
			this.options &= ~option;
		}

		// Token: 0x06003BB3 RID: 15283 RVA: 0x000D8CF0 File Offset: 0x000D7CF0
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder(base.ToString() + ": Title: " + this.Title + ", FileName: ");
			try
			{
				stringBuilder.Append(this.FileName);
			}
			catch (Exception ex)
			{
				stringBuilder.Append("<");
				stringBuilder.Append(ex.GetType().FullName);
				stringBuilder.Append(">");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x17000B65 RID: 2917
		// (get) Token: 0x06003BB4 RID: 15284 RVA: 0x000D8D70 File Offset: 0x000D7D70
		internal virtual bool SettingsSupportVistaDialog
		{
			get
			{
				return !this.ShowHelp && (Application.VisualStyleState & VisualStyleState.ClientAreaEnabled) == VisualStyleState.ClientAreaEnabled;
			}
		}

		// Token: 0x17000B66 RID: 2918
		// (get) Token: 0x06003BB5 RID: 15285 RVA: 0x000D8D88 File Offset: 0x000D7D88
		internal bool UseVistaDialogInternal
		{
			get
			{
				if (UnsafeNativeMethods.IsVista && this._autoUpgradeEnabled && this.SettingsSupportVistaDialog)
				{
					new EnvironmentPermission(PermissionState.Unrestricted).Assert();
					try
					{
						return SystemInformation.BootMode == BootMode.Normal;
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
					return false;
				}
				return false;
			}
		}

		// Token: 0x06003BB6 RID: 15286
		internal abstract FileDialogNative.IFileDialog CreateVistaDialog();

		// Token: 0x06003BB7 RID: 15287 RVA: 0x000D8DDC File Offset: 0x000D7DDC
		[SecurityPermission(SecurityAction.Assert, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private bool RunDialogVista(IntPtr hWndOwner)
		{
			FileDialogNative.IFileDialog fileDialog = this.CreateVistaDialog();
			this.OnBeforeVistaDialog(fileDialog);
			FileDialog.VistaDialogEvents vistaDialogEvents = new FileDialog.VistaDialogEvents(this);
			uint num;
			fileDialog.Advise(vistaDialogEvents, out num);
			bool flag;
			try
			{
				int num2 = fileDialog.Show(hWndOwner);
				flag = 0 == num2;
			}
			finally
			{
				fileDialog.Unadvise(num);
				GC.KeepAlive(vistaDialogEvents);
			}
			return flag;
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x000D8E38 File Offset: 0x000D7E38
		internal virtual void OnBeforeVistaDialog(FileDialogNative.IFileDialog dialog)
		{
			dialog.SetDefaultExtension(this.DefaultExt);
			dialog.SetFileName(this.FileName);
			if (!string.IsNullOrEmpty(this.InitialDirectory))
			{
				try
				{
					FileDialogNative.IShellItem shellItemForPath = FileDialog.GetShellItemForPath(this.InitialDirectory);
					dialog.SetDefaultFolder(shellItemForPath);
					dialog.SetFolder(shellItemForPath);
				}
				catch (FileNotFoundException)
				{
				}
			}
			dialog.SetTitle(this.Title);
			dialog.SetOptions(this.GetOptions());
			this.SetFileTypes(dialog);
			this._customPlaces.Apply(dialog);
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x000D8EC4 File Offset: 0x000D7EC4
		private FileDialogNative.FOS GetOptions()
		{
			FileDialogNative.FOS fos = (FileDialogNative.FOS)(this.options & 1063690);
			fos |= FileDialogNative.FOS.FOS_DEFAULTNOMINIMODE;
			return fos | FileDialogNative.FOS.FOS_FORCEFILESYSTEM;
		}

		// Token: 0x06003BBA RID: 15290
		internal abstract string[] ProcessVistaFiles(FileDialogNative.IFileDialog dialog);

		// Token: 0x06003BBB RID: 15291 RVA: 0x000D8EEC File Offset: 0x000D7EEC
		private bool HandleVistaFileOk(FileDialogNative.IFileDialog dialog)
		{
			int num = this.options;
			int num2 = this.filterIndex;
			string[] array = this.fileNames;
			bool flag = this.securityCheckFileNames;
			bool flag2 = false;
			try
			{
				this.securityCheckFileNames = true;
				Thread.MemoryBarrier();
				uint num3;
				dialog.GetFileTypeIndex(out num3);
				this.filterIndex = (int)num3;
				this.fileNames = this.ProcessVistaFiles(dialog);
				if (this.ProcessFileNames())
				{
					CancelEventArgs cancelEventArgs = new CancelEventArgs();
					if (NativeWindow.WndProcShouldBeDebuggable)
					{
						this.OnFileOk(cancelEventArgs);
						flag2 = !cancelEventArgs.Cancel;
					}
					else
					{
						try
						{
							this.OnFileOk(cancelEventArgs);
							flag2 = !cancelEventArgs.Cancel;
						}
						catch (Exception ex)
						{
							Application.OnThreadException(ex);
						}
					}
				}
			}
			finally
			{
				if (!flag2)
				{
					this.securityCheckFileNames = flag;
					Thread.MemoryBarrier();
					this.fileNames = array;
					this.options = num;
					this.filterIndex = num2;
				}
				else if ((this.options & 4) != 0)
				{
					this.options &= -2;
				}
			}
			return flag2;
		}

		// Token: 0x06003BBC RID: 15292 RVA: 0x000D8FF0 File Offset: 0x000D7FF0
		private void SetFileTypes(FileDialogNative.IFileDialog dialog)
		{
			FileDialogNative.COMDLG_FILTERSPEC[] filterItems = this.FilterItems;
			dialog.SetFileTypes((uint)filterItems.Length, filterItems);
			if (filterItems.Length > 0)
			{
				dialog.SetFileTypeIndex((uint)this.filterIndex);
			}
		}

		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x06003BBD RID: 15293 RVA: 0x000D9020 File Offset: 0x000D8020
		private FileDialogNative.COMDLG_FILTERSPEC[] FilterItems
		{
			get
			{
				return FileDialog.GetFilterItems(this.filter);
			}
		}

		// Token: 0x06003BBE RID: 15294 RVA: 0x000D9030 File Offset: 0x000D8030
		private static FileDialogNative.COMDLG_FILTERSPEC[] GetFilterItems(string filter)
		{
			List<FileDialogNative.COMDLG_FILTERSPEC> list = new List<FileDialogNative.COMDLG_FILTERSPEC>();
			if (!string.IsNullOrEmpty(filter))
			{
				string[] array = filter.Split(new char[] { '|' });
				if (array.Length % 2 == 0)
				{
					for (int i = 1; i < array.Length; i += 2)
					{
						FileDialogNative.COMDLG_FILTERSPEC comdlg_FILTERSPEC;
						comdlg_FILTERSPEC.pszSpec = array[i];
						comdlg_FILTERSPEC.pszName = array[i - 1];
						list.Add(comdlg_FILTERSPEC);
					}
				}
			}
			return list.ToArray();
		}

		// Token: 0x06003BBF RID: 15295 RVA: 0x000D909C File Offset: 0x000D809C
		internal static FileDialogNative.IShellItem GetShellItemForPath(string path)
		{
			FileDialogNative.IShellItem shellItem = null;
			IntPtr zero = IntPtr.Zero;
			uint num = 0U;
			if (0 <= UnsafeNativeMethods.Shell32.SHILCreateFromPath(path, out zero, ref num) && 0 <= UnsafeNativeMethods.Shell32.SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, zero, out shellItem))
			{
				return shellItem;
			}
			throw new FileNotFoundException();
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x000D90DC File Offset: 0x000D80DC
		internal static string GetFilePathFromShellItem(FileDialogNative.IShellItem item)
		{
			string text;
			item.GetDisplayName((FileDialogNative.SIGDN)2147647488U, out text);
			return text;
		}

		// Token: 0x17000B68 RID: 2920
		// (get) Token: 0x06003BC1 RID: 15297 RVA: 0x000D90F7 File Offset: 0x000D80F7
		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public FileDialogCustomPlacesCollection CustomPlaces
		{
			get
			{
				return this._customPlaces;
			}
		}

		// Token: 0x17000B69 RID: 2921
		// (get) Token: 0x06003BC2 RID: 15298 RVA: 0x000D90FF File Offset: 0x000D80FF
		// (set) Token: 0x06003BC3 RID: 15299 RVA: 0x000D9107 File Offset: 0x000D8107
		[DefaultValue(true)]
		public bool AutoUpgradeEnabled
		{
			get
			{
				return this._autoUpgradeEnabled;
			}
			set
			{
				this._autoUpgradeEnabled = value;
			}
		}

		// Token: 0x04001DBA RID: 7610
		private const int FILEBUFSIZE = 8192;

		// Token: 0x04001DBB RID: 7611
		internal const int OPTION_ADDEXTENSION = -2147483648;

		// Token: 0x04001DBC RID: 7612
		protected static readonly object EventFileOk = new object();

		// Token: 0x04001DBD RID: 7613
		internal int options;

		// Token: 0x04001DBE RID: 7614
		private string title;

		// Token: 0x04001DBF RID: 7615
		private string initialDir;

		// Token: 0x04001DC0 RID: 7616
		private string defaultExt;

		// Token: 0x04001DC1 RID: 7617
		private string[] fileNames;

		// Token: 0x04001DC2 RID: 7618
		private bool securityCheckFileNames;

		// Token: 0x04001DC3 RID: 7619
		private string filter;

		// Token: 0x04001DC4 RID: 7620
		private int filterIndex;

		// Token: 0x04001DC5 RID: 7621
		private bool supportMultiDottedExtensions;

		// Token: 0x04001DC6 RID: 7622
		private bool ignoreSecondFileOkNotification;

		// Token: 0x04001DC7 RID: 7623
		private int okNotificationCount;

		// Token: 0x04001DC8 RID: 7624
		private UnsafeNativeMethods.CharBuffer charBuffer;

		// Token: 0x04001DC9 RID: 7625
		private IntPtr dialogHWnd;

		// Token: 0x04001DCA RID: 7626
		private bool _autoUpgradeEnabled = true;

		// Token: 0x04001DCB RID: 7627
		private FileDialogCustomPlacesCollection _customPlaces = new FileDialogCustomPlacesCollection();

		// Token: 0x020003F8 RID: 1016
		private class VistaDialogEvents : FileDialogNative.IFileDialogEvents
		{
			// Token: 0x06003C2C RID: 15404 RVA: 0x000D912C File Offset: 0x000D812C
			public VistaDialogEvents(FileDialog dialog)
			{
				this._dialog = dialog;
			}

			// Token: 0x06003C2D RID: 15405 RVA: 0x000D913B File Offset: 0x000D813B
			public int OnFileOk(FileDialogNative.IFileDialog pfd)
			{
				if (!this._dialog.HandleVistaFileOk(pfd))
				{
					return 1;
				}
				return 0;
			}

			// Token: 0x06003C2E RID: 15406 RVA: 0x000D914E File Offset: 0x000D814E
			public int OnFolderChanging(FileDialogNative.IFileDialog pfd, FileDialogNative.IShellItem psiFolder)
			{
				return 0;
			}

			// Token: 0x06003C2F RID: 15407 RVA: 0x000D9151 File Offset: 0x000D8151
			public void OnFolderChange(FileDialogNative.IFileDialog pfd)
			{
			}

			// Token: 0x06003C30 RID: 15408 RVA: 0x000D9153 File Offset: 0x000D8153
			public void OnSelectionChange(FileDialogNative.IFileDialog pfd)
			{
			}

			// Token: 0x06003C31 RID: 15409 RVA: 0x000D9155 File Offset: 0x000D8155
			public void OnShareViolation(FileDialogNative.IFileDialog pfd, FileDialogNative.IShellItem psi, out FileDialogNative.FDE_SHAREVIOLATION_RESPONSE pResponse)
			{
				pResponse = FileDialogNative.FDE_SHAREVIOLATION_RESPONSE.FDESVR_DEFAULT;
			}

			// Token: 0x06003C32 RID: 15410 RVA: 0x000D915A File Offset: 0x000D815A
			public void OnTypeChange(FileDialogNative.IFileDialog pfd)
			{
			}

			// Token: 0x06003C33 RID: 15411 RVA: 0x000D915C File Offset: 0x000D815C
			public void OnOverwrite(FileDialogNative.IFileDialog pfd, FileDialogNative.IShellItem psi, out FileDialogNative.FDE_OVERWRITE_RESPONSE pResponse)
			{
				pResponse = FileDialogNative.FDE_OVERWRITE_RESPONSE.FDEOR_DEFAULT;
			}

			// Token: 0x04001E04 RID: 7684
			private FileDialog _dialog;
		}
	}
}
