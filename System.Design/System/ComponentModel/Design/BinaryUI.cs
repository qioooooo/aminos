using System;
using System.Design;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace System.ComponentModel.Design
{
	internal partial class BinaryUI : Form
	{
		public BinaryUI(BinaryEditor editor)
		{
			this.editor = editor;
			this.InitializeComponent();
		}

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

		private void RadioAuto_checkedChanged(object source, EventArgs e)
		{
			if (this.radioAuto.Checked)
			{
				this.byteViewer.SetDisplayMode(DisplayMode.Auto);
			}
		}

		private void RadioHex_checkedChanged(object source, EventArgs e)
		{
			if (this.radioHex.Checked)
			{
				this.byteViewer.SetDisplayMode(DisplayMode.Hexdump);
			}
		}

		private void RadioAnsi_checkedChanged(object source, EventArgs e)
		{
			if (this.radioAnsi.Checked)
			{
				this.byteViewer.SetDisplayMode(DisplayMode.Ansi);
			}
		}

		private void RadioUnicode_checkedChanged(object source, EventArgs e)
		{
			if (this.radioUnicode.Checked)
			{
				this.byteViewer.SetDisplayMode(DisplayMode.Unicode);
			}
		}

		private void ButtonOK_click(object source, EventArgs e)
		{
			object obj = this.value;
			this.editor.ConvertToValue(this.byteViewer.GetBytes(), ref obj);
			this.value = obj;
		}

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

		private void Form_HelpRequested(object sender, HelpEventArgs e)
		{
			this.editor.ShowHelp();
		}

		private void Form_HelpButtonClicked(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			this.editor.ShowHelp();
		}

		private BinaryEditor editor;

		private object value;
	}
}
