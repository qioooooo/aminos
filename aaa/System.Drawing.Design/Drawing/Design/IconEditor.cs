using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Drawing.Design
{
	// Token: 0x02000019 RID: 25
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class IconEditor : UITypeEditor
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x0000586C File Offset: 0x0000486C
		protected static string CreateExtensionsString(string[] extensions, string sep)
		{
			if (extensions == null || extensions.Length == 0)
			{
				return null;
			}
			string text = null;
			for (int i = 0; i < extensions.Length - 1; i++)
			{
				text = text + "*." + extensions[i] + sep;
			}
			return text + "*." + extensions[extensions.Length - 1];
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x000058BC File Offset: 0x000048BC
		protected static string CreateFilterEntry(IconEditor e)
		{
			string fileDialogDescription = e.GetFileDialogDescription();
			string text = IconEditor.CreateExtensionsString(e.GetExtensions(), ",");
			string text2 = IconEditor.CreateExtensionsString(e.GetExtensions(), ";");
			return string.Concat(new string[] { fileDialogDescription, "(", text, ")|", text2 });
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x0000591C File Offset: 0x0000491C
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.fileDialog == null)
					{
						this.fileDialog = new OpenFileDialog();
						string text = IconEditor.CreateFilterEntry(this);
						for (int i = 0; i < IconEditor.imageExtenders.Length; i++)
						{
						}
						this.fileDialog.Filter = text;
					}
					IntPtr focus = UnsafeNativeMethods.GetFocus();
					try
					{
						if (this.fileDialog.ShowDialog() == DialogResult.OK)
						{
							FileStream fileStream = new FileStream(this.fileDialog.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
							value = this.LoadFromStream(fileStream);
						}
					}
					finally
					{
						if (focus != IntPtr.Zero)
						{
							UnsafeNativeMethods.SetFocus(new HandleRef(null, focus));
						}
					}
				}
			}
			return value;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x000059E4 File Offset: 0x000049E4
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x000059E7 File Offset: 0x000049E7
		protected virtual string GetFileDialogDescription()
		{
			return SR.GetString("iconFileDescription");
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x000059F4 File Offset: 0x000049F4
		protected virtual string[] GetExtensions()
		{
			return new string[] { "ico" };
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00005A11 File Offset: 0x00004A11
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00005A14 File Offset: 0x00004A14
		protected virtual Icon LoadFromStream(Stream stream)
		{
			return new Icon(stream);
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00005A1C File Offset: 0x00004A1C
		public override void PaintValue(PaintValueEventArgs e)
		{
			Icon icon = e.Value as Icon;
			if (icon != null)
			{
				Size size = icon.Size;
				Rectangle bounds = e.Bounds;
				if (icon.Width < bounds.Width)
				{
					bounds.X = (bounds.Width - icon.Width) / 2;
					bounds.Width = icon.Width;
				}
				if (icon.Height < bounds.Height)
				{
					bounds.X = (bounds.Height - icon.Height) / 2;
					bounds.Height = icon.Height;
				}
				e.Graphics.DrawIcon(icon, bounds);
			}
		}

		// Token: 0x04000085 RID: 133
		internal static Type[] imageExtenders = new Type[0];

		// Token: 0x04000086 RID: 134
		internal FileDialog fileDialog;
	}
}
