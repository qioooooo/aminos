using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows.Forms;

namespace Microsoft.VisualBasic.MyServices.Internal
{
	internal partial class ProgressDialog : Form
	{
		public event ProgressDialog.UserHitCancelEventHandler UserHitCancel;

		public ProgressDialog()
		{
			base.Resize += this.ProgressDialog_Resize;
			base.Shown += this.ProgressDialog_Activated;
			base.FormClosing += this.ProgressDialog_FormClosing;
			this.m_Canceled = false;
			this.m_FormClosableSemaphore = new ManualResetEvent(false);
			this.InitializeComponent();
		}

		public void Increment(int incrementAmount)
		{
			this.ProgressBarWork.Increment(incrementAmount);
		}

		public void CloseDialog()
		{
			this.m_CloseDialogInvoked = true;
			this.Close();
		}

		public void ShowProgressDialog()
		{
			try
			{
				if (!this.m_Closing)
				{
					this.ShowDialog();
				}
			}
			finally
			{
				this.FormClosableSemaphore.Set();
			}
		}

		public string LabelText
		{
			get
			{
				return this.LabelInfo.Text;
			}
			set
			{
				this.LabelInfo.Text = value;
			}
		}

		public ManualResetEvent FormClosableSemaphore
		{
			get
			{
				return this.m_FormClosableSemaphore;
			}
		}

		public void IndicateClosing()
		{
			this.m_Closing = true;
		}

		public bool UserCanceledTheDialog
		{
			get
			{
				return this.m_Canceled;
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.Style |= 262144;
				return createParams;
			}
		}

		private void ButtonCloseDialog_Click(object sender, EventArgs e)
		{
			this.ButtonCloseDialog.Enabled = false;
			this.m_Canceled = true;
			ProgressDialog.UserHitCancelEventHandler userHitCancelEvent = this.UserHitCancelEvent;
			if (userHitCancelEvent != null)
			{
				userHitCancelEvent();
			}
		}

		private void ProgressDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (((e.CloseReason == CloseReason.UserClosing) & !this.m_CloseDialogInvoked) && ((this.ProgressBarWork.Value < 100) & !this.m_Canceled))
			{
				e.Cancel = true;
				this.m_Canceled = true;
				ProgressDialog.UserHitCancelEventHandler userHitCancelEvent = this.UserHitCancelEvent;
				if (userHitCancelEvent != null)
				{
					userHitCancelEvent();
				}
			}
		}

		private void ProgressDialog_Resize(object sender, EventArgs e)
		{
			Control labelInfo = this.LabelInfo;
			Size size = new Size(checked(this.ClientSize.Width - 20), 0);
			labelInfo.MaximumSize = size;
		}

		private void ProgressDialog_Activated(object sender, EventArgs e)
		{
			this.m_FormClosableSemaphore.Set();
		}

		internal virtual Label LabelInfo
		{
			get
			{
				return this._LabelInfo;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._LabelInfo = value;
			}
		}

		internal virtual ProgressBar ProgressBarWork
		{
			get
			{
				return this._ProgressBarWork;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				this._ProgressBarWork = value;
			}
		}

		internal virtual Button ButtonCloseDialog
		{
			get
			{
				return this._ButtonCloseDialog;
			}
			[MethodImpl(MethodImplOptions.Synchronized)]
			set
			{
				if (this._ButtonCloseDialog != null)
				{
					this._ButtonCloseDialog.Click -= this.ButtonCloseDialog_Click;
				}
				this._ButtonCloseDialog = value;
				if (this._ButtonCloseDialog != null)
				{
					this._ButtonCloseDialog.Click += this.ButtonCloseDialog_Click;
				}
			}
		}

		private bool m_Closing;

		private bool m_Canceled;

		private ManualResetEvent m_FormClosableSemaphore;

		private bool m_CloseDialogInvoked;

		private const int WS_THICKFRAME = 262144;

		private const int BORDER_SIZE = 20;

		[AccessedThroughProperty("LabelInfo")]
		private Label _LabelInfo;

		[AccessedThroughProperty("ProgressBarWork")]
		private ProgressBar _ProgressBarWork;

		[AccessedThroughProperty("ButtonCloseDialog")]
		private Button _ButtonCloseDialog;

		public delegate void UserHitCancelEventHandler();
	}
}
