using System;
using System.Drawing.Design;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	public sealed class BinaryEditor : UITypeEditor
	{
		internal object GetService(Type serviceType)
		{
			if (this.context == null)
			{
				return null;
			}
			IDesignerHost designerHost = this.context.GetService(typeof(IDesignerHost)) as IDesignerHost;
			if (designerHost == null)
			{
				return this.context.GetService(serviceType);
			}
			return designerHost.GetService(serviceType);
		}

		internal byte[] ConvertToBytes(object value)
		{
			if (value is Stream)
			{
				Stream stream = (Stream)value;
				stream.Position = 0L;
				int num = (int)(stream.Length - stream.Position);
				byte[] array = new byte[num];
				stream.Read(array, 0, num);
				return array;
			}
			if (value is byte[])
			{
				return (byte[])value;
			}
			if (value is string)
			{
				int num2 = ((string)value).Length * 2;
				byte[] array2 = new byte[num2];
				Encoding.Unicode.GetBytes(((string)value).ToCharArray(), 0, num2 / 2, array2, 0);
				return array2;
			}
			return null;
		}

		internal void ConvertToValue(byte[] bytes, ref object value)
		{
			if (value is Stream)
			{
				Stream stream = (Stream)value;
				stream.Position = 0L;
				stream.Write(bytes, 0, bytes.Length);
				return;
			}
			if (value is byte[])
			{
				value = bytes;
				return;
			}
			if (value is string)
			{
				value = BitConverter.ToString(bytes);
			}
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			if (provider != null)
			{
				this.context = context;
				IWindowsFormsEditorService windowsFormsEditorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
				if (windowsFormsEditorService != null)
				{
					if (this.binaryUI == null)
					{
						this.binaryUI = new BinaryUI(this);
					}
					this.binaryUI.Value = value;
					if (windowsFormsEditorService.ShowDialog(this.binaryUI) == DialogResult.OK)
					{
						value = this.binaryUI.Value;
					}
					this.binaryUI.Value = null;
				}
			}
			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		internal void ShowHelp()
		{
			IHelpService helpService = this.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword(BinaryEditor.HELP_KEYWORD);
			}
		}

		private static readonly string HELP_KEYWORD = "System.ComponentModel.Design.BinaryEditor";

		private ITypeDescriptorContext context;

		private BinaryUI binaryUI;
	}
}
