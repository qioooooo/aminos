using System;
using System.Drawing.Design;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	// Token: 0x020000F5 RID: 245
	public sealed class BinaryEditor : UITypeEditor
	{
		// Token: 0x06000A12 RID: 2578 RVA: 0x00026B54 File Offset: 0x00025B54
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

		// Token: 0x06000A13 RID: 2579 RVA: 0x00026BA0 File Offset: 0x00025BA0
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

		// Token: 0x06000A14 RID: 2580 RVA: 0x00026C34 File Offset: 0x00025C34
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

		// Token: 0x06000A15 RID: 2581 RVA: 0x00026C84 File Offset: 0x00025C84
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

		// Token: 0x06000A16 RID: 2582 RVA: 0x00026CFD File Offset: 0x00025CFD
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x00026D00 File Offset: 0x00025D00
		internal void ShowHelp()
		{
			IHelpService helpService = this.GetService(typeof(IHelpService)) as IHelpService;
			if (helpService != null)
			{
				helpService.ShowHelpFromKeyword(BinaryEditor.HELP_KEYWORD);
			}
		}

		// Token: 0x04000D5E RID: 3422
		private static readonly string HELP_KEYWORD = "System.ComponentModel.Design.BinaryEditor";

		// Token: 0x04000D5F RID: 3423
		private ITypeDescriptorContext context;

		// Token: 0x04000D60 RID: 3424
		private BinaryUI binaryUI;
	}
}
