using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x0200027B RID: 635
	[SRDescription("DescriptionColorDialog")]
	[DefaultProperty("Color")]
	public class ColorDialog : CommonDialog
	{
		// Token: 0x06002229 RID: 8745 RVA: 0x0004A8BC File Offset: 0x000498BC
		public ColorDialog()
		{
			this.customColors = new int[16];
			this.Reset();
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600222A RID: 8746 RVA: 0x0004A8D7 File Offset: 0x000498D7
		// (set) Token: 0x0600222B RID: 8747 RVA: 0x0004A8E3 File Offset: 0x000498E3
		[SRCategory("CatBehavior")]
		[SRDescription("CDallowFullOpenDescr")]
		[DefaultValue(true)]
		public virtual bool AllowFullOpen
		{
			get
			{
				return !this.GetOption(4);
			}
			set
			{
				this.SetOption(4, !value);
			}
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600222C RID: 8748 RVA: 0x0004A8F0 File Offset: 0x000498F0
		// (set) Token: 0x0600222D RID: 8749 RVA: 0x0004A8FD File Offset: 0x000498FD
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		[SRDescription("CDanyColorDescr")]
		public virtual bool AnyColor
		{
			get
			{
				return this.GetOption(256);
			}
			set
			{
				this.SetOption(256, value);
			}
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x0600222E RID: 8750 RVA: 0x0004A90B File Offset: 0x0004990B
		// (set) Token: 0x0600222F RID: 8751 RVA: 0x0004A913 File Offset: 0x00049913
		[SRDescription("CDcolorDescr")]
		[SRCategory("CatData")]
		public Color Color
		{
			get
			{
				return this.color;
			}
			set
			{
				if (!value.IsEmpty)
				{
					this.color = value;
					return;
				}
				this.color = Color.Black;
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06002230 RID: 8752 RVA: 0x0004A931 File Offset: 0x00049931
		// (set) Token: 0x06002231 RID: 8753 RVA: 0x0004A944 File Offset: 0x00049944
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		[Browsable(false)]
		[SRDescription("CDcustomColorsDescr")]
		public int[] CustomColors
		{
			get
			{
				return (int[])this.customColors.Clone();
			}
			set
			{
				int num = ((value == null) ? 0 : Math.Min(value.Length, 16));
				if (num > 0)
				{
					Array.Copy(value, 0, this.customColors, 0, num);
				}
				for (int i = num; i < 16; i++)
				{
					this.customColors[i] = 16777215;
				}
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06002232 RID: 8754 RVA: 0x0004A98F File Offset: 0x0004998F
		// (set) Token: 0x06002233 RID: 8755 RVA: 0x0004A998 File Offset: 0x00049998
		[SRDescription("CDfullOpenDescr")]
		[SRCategory("CatAppearance")]
		[DefaultValue(false)]
		public virtual bool FullOpen
		{
			get
			{
				return this.GetOption(2);
			}
			set
			{
				this.SetOption(2, value);
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06002234 RID: 8756 RVA: 0x0004A9A2 File Offset: 0x000499A2
		protected virtual IntPtr Instance
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			[SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
			get
			{
				return UnsafeNativeMethods.GetModuleHandle(null);
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06002235 RID: 8757 RVA: 0x0004A9AA File Offset: 0x000499AA
		protected virtual int Options
		{
			get
			{
				return this.options;
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06002236 RID: 8758 RVA: 0x0004A9B2 File Offset: 0x000499B2
		// (set) Token: 0x06002237 RID: 8759 RVA: 0x0004A9BB File Offset: 0x000499BB
		[SRCategory("CatBehavior")]
		[SRDescription("CDshowHelpDescr")]
		[DefaultValue(false)]
		public virtual bool ShowHelp
		{
			get
			{
				return this.GetOption(8);
			}
			set
			{
				this.SetOption(8, value);
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06002238 RID: 8760 RVA: 0x0004A9C5 File Offset: 0x000499C5
		// (set) Token: 0x06002239 RID: 8761 RVA: 0x0004A9D2 File Offset: 0x000499D2
		[SRDescription("CDsolidColorOnlyDescr")]
		[SRCategory("CatBehavior")]
		[DefaultValue(false)]
		public virtual bool SolidColorOnly
		{
			get
			{
				return this.GetOption(128);
			}
			set
			{
				this.SetOption(128, value);
			}
		}

		// Token: 0x0600223A RID: 8762 RVA: 0x0004A9E0 File Offset: 0x000499E0
		private bool GetOption(int option)
		{
			return (this.options & option) != 0;
		}

		// Token: 0x0600223B RID: 8763 RVA: 0x0004A9F0 File Offset: 0x000499F0
		public override void Reset()
		{
			this.options = 0;
			this.color = Color.Black;
			this.CustomColors = null;
		}

		// Token: 0x0600223C RID: 8764 RVA: 0x0004AA0B File Offset: 0x00049A0B
		private void ResetColor()
		{
			this.Color = Color.Black;
		}

		// Token: 0x0600223D RID: 8765 RVA: 0x0004AA18 File Offset: 0x00049A18
		protected override bool RunDialog(IntPtr hwndOwner)
		{
			NativeMethods.WndProc wndProc = new NativeMethods.WndProc(this.HookProc);
			NativeMethods.CHOOSECOLOR choosecolor = new NativeMethods.CHOOSECOLOR();
			IntPtr intPtr = Marshal.AllocCoTaskMem(64);
			bool flag;
			try
			{
				Marshal.Copy(this.customColors, 0, intPtr, 16);
				choosecolor.hwndOwner = hwndOwner;
				choosecolor.hInstance = this.Instance;
				choosecolor.rgbResult = ColorTranslator.ToWin32(this.color);
				choosecolor.lpCustColors = intPtr;
				int num = this.Options | 17;
				if (!this.AllowFullOpen)
				{
					num &= -3;
				}
				choosecolor.Flags = num;
				choosecolor.lpfnHook = wndProc;
				if (!SafeNativeMethods.ChooseColor(choosecolor))
				{
					flag = false;
				}
				else
				{
					if (choosecolor.rgbResult != ColorTranslator.ToWin32(this.color))
					{
						this.color = ColorTranslator.FromOle(choosecolor.rgbResult);
					}
					Marshal.Copy(intPtr, this.customColors, 0, 16);
					flag = true;
				}
			}
			finally
			{
				Marshal.FreeCoTaskMem(intPtr);
			}
			return flag;
		}

		// Token: 0x0600223E RID: 8766 RVA: 0x0004AB00 File Offset: 0x00049B00
		private void SetOption(int option, bool value)
		{
			if (value)
			{
				this.options |= option;
				return;
			}
			this.options &= ~option;
		}

		// Token: 0x0600223F RID: 8767 RVA: 0x0004AB24 File Offset: 0x00049B24
		private bool ShouldSerializeColor()
		{
			return !this.Color.Equals(Color.Black);
		}

		// Token: 0x06002240 RID: 8768 RVA: 0x0004AB54 File Offset: 0x00049B54
		public override string ToString()
		{
			string text = base.ToString();
			return text + ",  Color: " + this.Color.ToString();
		}

		// Token: 0x04001500 RID: 5376
		private int options;

		// Token: 0x04001501 RID: 5377
		private int[] customColors;

		// Token: 0x04001502 RID: 5378
		private Color color;
	}
}
