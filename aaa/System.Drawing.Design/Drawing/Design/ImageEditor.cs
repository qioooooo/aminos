using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.Drawing.Design
{
	// Token: 0x02000006 RID: 6
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ImageEditor : UITypeEditor
	{
		// Token: 0x0600000D RID: 13 RVA: 0x0000229D File Offset: 0x0000129D
		protected virtual Type[] GetImageExtenders()
		{
			return ImageEditor.imageExtenders;
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022A4 File Offset: 0x000012A4
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

		// Token: 0x0600000F RID: 15 RVA: 0x000022F4 File Offset: 0x000012F4
		protected static string CreateFilterEntry(ImageEditor e)
		{
			string fileDialogDescription = e.GetFileDialogDescription();
			string text = ImageEditor.CreateExtensionsString(e.GetExtensions(), ",");
			string text2 = ImageEditor.CreateExtensionsString(e.GetExtensions(), ";");
			return string.Concat(new string[] { fileDialogDescription, "(", text, ")|", text2 });
		}

		// Token: 0x06000010 RID: 16 RVA: 0x00002354 File Offset: 0x00001354
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
						string text = ImageEditor.CreateFilterEntry(this);
						for (int i = 0; i < this.GetImageExtenders().Length; i++)
						{
							ImageEditor imageEditor = (ImageEditor)Activator.CreateInstance(this.GetImageExtenders()[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
							Type type = base.GetType();
							Type type2 = imageEditor.GetType();
							if (!type.Equals(type2) && imageEditor != null && type.IsInstanceOfType(imageEditor))
							{
								text = text + "|" + ImageEditor.CreateFilterEntry(imageEditor);
							}
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

		// Token: 0x06000011 RID: 17 RVA: 0x00002478 File Offset: 0x00001478
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x0000247B File Offset: 0x0000147B
		protected virtual string GetFileDialogDescription()
		{
			return SR.GetString("imageFileDescription");
		}

		// Token: 0x06000013 RID: 19 RVA: 0x00002488 File Offset: 0x00001488
		protected virtual string[] GetExtensions()
		{
			ArrayList arrayList = new ArrayList();
			for (int i = 0; i < this.GetImageExtenders().Length; i++)
			{
				ImageEditor imageEditor = (ImageEditor)Activator.CreateInstance(this.GetImageExtenders()[i], BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.CreateInstance, null, null, null);
				if (!imageEditor.GetType().Equals(typeof(ImageEditor)))
				{
					arrayList.AddRange(new ArrayList(imageEditor.GetExtensions()));
				}
			}
			return (string[])arrayList.ToArray(typeof(string));
		}

		// Token: 0x06000014 RID: 20 RVA: 0x00002506 File Offset: 0x00001506
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x06000015 RID: 21 RVA: 0x0000250C File Offset: 0x0000150C
		protected virtual Image LoadFromStream(Stream stream)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, (int)stream.Length);
			MemoryStream memoryStream = new MemoryStream(array);
			return Image.FromStream(memoryStream);
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002544 File Offset: 0x00001544
		public override void PaintValue(PaintValueEventArgs e)
		{
			Image image = e.Value as Image;
			if (image != null)
			{
				Rectangle bounds = e.Bounds;
				bounds.Width--;
				bounds.Height--;
				e.Graphics.DrawRectangle(SystemPens.WindowFrame, bounds);
				e.Graphics.DrawImage(image, e.Bounds);
			}
		}

		// Token: 0x0400003F RID: 63
		internal static Type[] imageExtenders = new Type[]
		{
			typeof(BitmapEditor),
			typeof(MetafileEditor)
		};

		// Token: 0x04000040 RID: 64
		internal FileDialog fileDialog;
	}
}
