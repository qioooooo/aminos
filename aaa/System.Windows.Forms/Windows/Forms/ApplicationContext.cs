using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000213 RID: 531
	public class ApplicationContext : IDisposable
	{
		// Token: 0x06001891 RID: 6289 RVA: 0x0002BB26 File Offset: 0x0002AB26
		public ApplicationContext()
			: this(null)
		{
		}

		// Token: 0x06001892 RID: 6290 RVA: 0x0002BB2F File Offset: 0x0002AB2F
		public ApplicationContext(Form mainForm)
		{
			this.MainForm = mainForm;
		}

		// Token: 0x06001893 RID: 6291 RVA: 0x0002BB40 File Offset: 0x0002AB40
		~ApplicationContext()
		{
			this.Dispose(false);
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001894 RID: 6292 RVA: 0x0002BB70 File Offset: 0x0002AB70
		// (set) Token: 0x06001895 RID: 6293 RVA: 0x0002BB78 File Offset: 0x0002AB78
		public Form MainForm
		{
			get
			{
				return this.mainForm;
			}
			set
			{
				EventHandler eventHandler = new EventHandler(this.OnMainFormDestroy);
				if (this.mainForm != null)
				{
					this.mainForm.HandleDestroyed -= eventHandler;
				}
				this.mainForm = value;
				if (this.mainForm != null)
				{
					this.mainForm.HandleDestroyed += eventHandler;
				}
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001896 RID: 6294 RVA: 0x0002BBC1 File Offset: 0x0002ABC1
		// (set) Token: 0x06001897 RID: 6295 RVA: 0x0002BBC9 File Offset: 0x0002ABC9
		[DefaultValue(null)]
		[TypeConverter(typeof(StringConverter))]
		[Localizable(false)]
		[Bindable(true)]
		[SRDescription("ControlTagDescr")]
		[SRCategory("CatData")]
		public object Tag
		{
			get
			{
				return this.userData;
			}
			set
			{
				this.userData = value;
			}
		}

		// Token: 0x14000050 RID: 80
		// (add) Token: 0x06001898 RID: 6296 RVA: 0x0002BBD2 File Offset: 0x0002ABD2
		// (remove) Token: 0x06001899 RID: 6297 RVA: 0x0002BBEB File Offset: 0x0002ABEB
		public event EventHandler ThreadExit;

		// Token: 0x0600189A RID: 6298 RVA: 0x0002BC04 File Offset: 0x0002AC04
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x0600189B RID: 6299 RVA: 0x0002BC13 File Offset: 0x0002AC13
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.mainForm != null)
			{
				if (!this.mainForm.IsDisposed)
				{
					this.mainForm.Dispose();
				}
				this.mainForm = null;
			}
		}

		// Token: 0x0600189C RID: 6300 RVA: 0x0002BC3F File Offset: 0x0002AC3F
		public void ExitThread()
		{
			this.ExitThreadCore();
		}

		// Token: 0x0600189D RID: 6301 RVA: 0x0002BC47 File Offset: 0x0002AC47
		protected virtual void ExitThreadCore()
		{
			if (this.ThreadExit != null)
			{
				this.ThreadExit(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600189E RID: 6302 RVA: 0x0002BC62 File Offset: 0x0002AC62
		protected virtual void OnMainFormClosed(object sender, EventArgs e)
		{
			this.ExitThreadCore();
		}

		// Token: 0x0600189F RID: 6303 RVA: 0x0002BC6C File Offset: 0x0002AC6C
		private void OnMainFormDestroy(object sender, EventArgs e)
		{
			Form form = (Form)sender;
			if (!form.RecreatingHandle)
			{
				form.HandleDestroyed -= this.OnMainFormDestroy;
				this.OnMainFormClosed(sender, e);
			}
		}

		// Token: 0x040011FF RID: 4607
		private Form mainForm;

		// Token: 0x04001200 RID: 4608
		private object userData;
	}
}
