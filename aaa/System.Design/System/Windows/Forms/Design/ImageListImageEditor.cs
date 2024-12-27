using System;
using System.Collections;
using System.ComponentModel;
using System.Design;
using System.Drawing.Design;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000251 RID: 593
	public class ImageListImageEditor : ImageEditor
	{
		// Token: 0x060016A3 RID: 5795 RVA: 0x00075569 File Offset: 0x00074569
		protected override Type[] GetImageExtenders()
		{
			return ImageListImageEditor.imageExtenders;
		}

		// Token: 0x060016A4 RID: 5796 RVA: 0x00075570 File Offset: 0x00074570
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			ArrayList arrayList = new ArrayList();
			if (provider != null)
			{
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.fileDialog == null)
					{
						this.fileDialog = new OpenFileDialog();
						this.fileDialog.Multiselect = true;
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
							foreach (string text2 in this.fileDialog.FileNames)
							{
								FileStream fileStream = new FileStream(text2, FileMode.Open, FileAccess.Read, FileShare.Read);
								ImageListImage imageListImage = this.LoadImageFromStream(fileStream, text2.EndsWith(".ico"));
								imageListImage.Name = Path.GetFileName(text2);
								arrayList.Add(imageListImage);
							}
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
				return arrayList;
			}
			return value;
		}

		// Token: 0x060016A5 RID: 5797 RVA: 0x000756F0 File Offset: 0x000746F0
		protected override string GetFileDialogDescription()
		{
			return SR.GetString("imageFileDescription");
		}

		// Token: 0x060016A6 RID: 5798 RVA: 0x000756FC File Offset: 0x000746FC
		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		// Token: 0x060016A7 RID: 5799 RVA: 0x00075700 File Offset: 0x00074700
		private ImageListImage LoadImageFromStream(Stream stream, bool imageIsIcon)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, (int)stream.Length);
			MemoryStream memoryStream = new MemoryStream(array);
			return ImageListImage.ImageListImageFromStream(memoryStream, imageIsIcon);
		}

		// Token: 0x060016A8 RID: 5800 RVA: 0x00075738 File Offset: 0x00074738
		public override void PaintValue(PaintValueEventArgs e)
		{
			if (e.Value is ImageListImage)
			{
				e = new PaintValueEventArgs(e.Context, ((ImageListImage)e.Value).Image, e.Graphics, e.Bounds);
			}
			base.PaintValue(e);
		}

		// Token: 0x040012F6 RID: 4854
		internal static Type[] imageExtenders = new Type[] { typeof(BitmapEditor) };

		// Token: 0x040012F7 RID: 4855
		private OpenFileDialog fileDialog;
	}
}
