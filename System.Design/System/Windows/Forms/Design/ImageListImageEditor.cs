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
	public class ImageListImageEditor : ImageEditor
	{
		protected override Type[] GetImageExtenders()
		{
			return ImageListImageEditor.imageExtenders;
		}

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

		protected override string GetFileDialogDescription()
		{
			return SR.GetString("imageFileDescription");
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		private ImageListImage LoadImageFromStream(Stream stream, bool imageIsIcon)
		{
			byte[] array = new byte[stream.Length];
			stream.Read(array, 0, (int)stream.Length);
			MemoryStream memoryStream = new MemoryStream(array);
			return ImageListImage.ImageListImageFromStream(memoryStream, imageIsIcon);
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			if (e.Value is ImageListImage)
			{
				e = new PaintValueEventArgs(e.Context, ((ImageListImage)e.Value).Image, e.Graphics, e.Bounds);
			}
			base.PaintValue(e);
		}

		internal static Type[] imageExtenders = new Type[] { typeof(BitmapEditor) };

		private OpenFileDialog fileDialog;
	}
}
