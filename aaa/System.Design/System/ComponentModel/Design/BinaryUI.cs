using System;
using System.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	// Token: 0x020000F4 RID: 244
	internal partial class BinaryUI : Form
	{
		// Token: 0x06000A06 RID: 2566 RVA: 0x000261C8 File Offset: 0x000251C8
		public BinaryUI(BinaryEditor editor)
		{
			this.editor = editor;
			this.InitializeComponent();
		}

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x06000A07 RID: 2567 RVA: 0x000261DD File Offset: 0x000251DD
		// (set) Token: 0x06000A08 RID: 2568 RVA: 0x000261E8 File Offset: 0x000251E8
		public object Value
		{
			get
			{
				return this.value;
			}
			set
			{
				this.value = value;
				byte[] array = null;
				if (value != null)
				{
					array = this.editor.ConvertToBytes(value);
				}
				if (array != null)
				{
					this.byteViewer.SetBytes(array);
					this.byteViewer.Enabled = true;
					return;
				}
				this.byteViewer.SetBytes(new byte[0]);
				this.byteViewer.Enabled = false;
			}
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x00026247 File Offset: 0x00025247
		private void RadioAuto_checkedChanged(object source, EventArgs e)
		{
			if (this.radioAuto.Checked)
			{
				this.byteViewer.SetDisplayMode(DisplayMode.Auto);
			}
		}

		// Token: 0x06000A0A RID: 2570 RVA: 0x00026262 File Offset: 0x00025262
		private void RadioHex_checkedChanged(object source, EventArgs e)
		{
			if (this.radioHex.Checked)
			{
				this.byteViewer.SetDisplayMode(DisplayMode.Hexdump);
			}
		}

		// Token: 0x06000A0B RID: 2571 RVA: 0x0002627D File Offset: 0x0002527D
		private void RadioAnsi_checkedChanged(object source, EventArgs e)
		{
			if (this.radioAnsi.Checked)
			{
				this.byteViewer.SetDisplayMode(DisplayMode.Ansi);
			}
		}

		// Token: 0x06000A0C RID: 2572 RVA: 0x00026298 File Offset: 0x00025298
		private void RadioUnicode_checkedChanged(object source, EventArgs e)
		{
			if (this.radioUnicode.Checked)
			{
				this.byteViewer.SetDisplayMode(DisplayMode.Unicode);
			}
		}

		// Token: 0x06000A0D RID: 2573 RVA: 0x000262B4 File Offset: 0x000252B4
		private void ButtonOK_click(object source, EventArgs e)
		{
			object obj = this.value;
			this.editor.ConvertToValue(this.byteViewer.GetBytes(), ref obj);
			this.value = obj;
		}

		// Token: 0x06000A0E RID: 2574 RVA: 0x000262E8 File Offset: 0x000252E8
		private void ButtonSave_click(object source, EventArgs e)
		{
			try
			{
				SaveFileDialog saveFileDialog = new SaveFileDialog();
				saveFileDialog.FileName = SR.GetString("BinaryEditorFileName");
				saveFileDialog.Title = SR.GetString("BinaryEditorSaveFile");
				saveFileDialog.Filter = SR.GetString("BinaryEditorAllFiles") + " (*.*)|*.*";
				DialogResult dialogResult = saveFileDialog.ShowDialog();
				if (dialogResult == DialogResult.OK)
				{
					this.byteViewer.SaveToFile(saveFileDialog.FileName);
				}
			}
			catch (IOException ex)
			{
				RTLAwareMessageBox.Show(null, SR.GetString("BinaryEditorFileError", new object[] { ex.Message }), SR.GetString("BinaryEditorTitle"), MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0);
			}
		}

		// Token: 0x06000A0F RID: 2575 RVA: 0x00026398 File Offset: 0x00025398
		private void Form_HelpRequested(object sender, HelpEventArgs e)
		{
			this.editor.ShowHelp();
		}

		// Token: 0x06000A10 RID: 2576 RVA: 0x000263A5 File Offset: 0x000253A5
		private void Form_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.editor.ShowHelp();
		}

		// Token: 0x04000D51 RID: 3409
		private BinaryEditor editor;

		// Token: 0x04000D52 RID: 3410
		private object value;
	}
}
