using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.ComponentModel.Design
{
	// Token: 0x0200016D RID: 365
	[ComVisible(true)]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	[HostProtection(SecurityAction.LinkDemand, SharedState = true)]
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	public class MenuCommand
	{
		// Token: 0x06000BCE RID: 3022 RVA: 0x00028A0F File Offset: 0x00027A0F
		public MenuCommand(EventHandler handler, CommandID command)
		{
			this.execHandler = handler;
			this.commandID = command;
			this.status = 3;
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000BCF RID: 3023 RVA: 0x00028A2C File Offset: 0x00027A2C
		// (set) Token: 0x06000BD0 RID: 3024 RVA: 0x00028A3C File Offset: 0x00027A3C
		public virtual bool Checked
		{
			get
			{
				return (this.status & 4) != 0;
			}
			set
			{
				this.SetStatus(4, value);
			}
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000BD1 RID: 3025 RVA: 0x00028A46 File Offset: 0x00027A46
		// (set) Token: 0x06000BD2 RID: 3026 RVA: 0x00028A56 File Offset: 0x00027A56
		public virtual bool Enabled
		{
			get
			{
				return (this.status & 2) != 0;
			}
			set
			{
				this.SetStatus(2, value);
			}
		}

		// Token: 0x06000BD3 RID: 3027 RVA: 0x00028A60 File Offset: 0x00027A60
		private void SetStatus(int mask, bool value)
		{
			int num = this.status;
			if (value)
			{
				num |= mask;
			}
			else
			{
				num &= ~mask;
			}
			if (num != this.status)
			{
				this.status = num;
				this.OnCommandChanged(EventArgs.Empty);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000BD4 RID: 3028 RVA: 0x00028A9D File Offset: 0x00027A9D
		public virtual IDictionary Properties
		{
			get
			{
				if (this.properties == null)
				{
					this.properties = new HybridDictionary();
				}
				return this.properties;
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000BD5 RID: 3029 RVA: 0x00028AB8 File Offset: 0x00027AB8
		// (set) Token: 0x06000BD6 RID: 3030 RVA: 0x00028AC8 File Offset: 0x00027AC8
		public virtual bool Supported
		{
			get
			{
				return (this.status & 1) != 0;
			}
			set
			{
				this.SetStatus(1, value);
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x00028AD2 File Offset: 0x00027AD2
		// (set) Token: 0x06000BD8 RID: 3032 RVA: 0x00028AE0 File Offset: 0x00027AE0
		public virtual bool Visible
		{
			get
			{
				return (this.status & 16) == 0;
			}
			set
			{
				this.SetStatus(16, !value);
			}
		}

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x06000BD9 RID: 3033 RVA: 0x00028AEE File Offset: 0x00027AEE
		// (remove) Token: 0x06000BDA RID: 3034 RVA: 0x00028B07 File Offset: 0x00027B07
		public event EventHandler CommandChanged
		{
			add
			{
				this.statusHandler = (EventHandler)Delegate.Combine(this.statusHandler, value);
			}
			remove
			{
				this.statusHandler = (EventHandler)Delegate.Remove(this.statusHandler, value);
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x00028B20 File Offset: 0x00027B20
		public virtual CommandID CommandID
		{
			get
			{
				return this.commandID;
			}
		}

		// Token: 0x06000BDC RID: 3036 RVA: 0x00028B28 File Offset: 0x00027B28
		public virtual void Invoke()
		{
			if (this.execHandler != null)
			{
				try
				{
					this.execHandler(this, EventArgs.Empty);
				}
				catch (CheckoutException ex)
				{
					if (ex != CheckoutException.Canceled)
					{
						throw;
					}
				}
			}
		}

		// Token: 0x06000BDD RID: 3037 RVA: 0x00028B70 File Offset: 0x00027B70
		public virtual void Invoke(object arg)
		{
			this.Invoke();
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00028B78 File Offset: 0x00027B78
		public virtual int OleStatus
		{
			get
			{
				return this.status;
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00028B80 File Offset: 0x00027B80
		protected virtual void OnCommandChanged(EventArgs e)
		{
			if (this.statusHandler != null)
			{
				this.statusHandler(this, e);
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00028B98 File Offset: 0x00027B98
		public override string ToString()
		{
			string text = this.commandID.ToString() + " : ";
			if ((this.status & 1) != 0)
			{
				text += "Supported";
			}
			if ((this.status & 2) != 0)
			{
				text += "|Enabled";
			}
			if ((this.status & 16) == 0)
			{
				text += "|Visible";
			}
			if ((this.status & 4) != 0)
			{
				text += "|Checked";
			}
			return text;
		}

		// Token: 0x04000ABE RID: 2750
		private const int ENABLED = 2;

		// Token: 0x04000ABF RID: 2751
		private const int INVISIBLE = 16;

		// Token: 0x04000AC0 RID: 2752
		private const int CHECKED = 4;

		// Token: 0x04000AC1 RID: 2753
		private const int SUPPORTED = 1;

		// Token: 0x04000AC2 RID: 2754
		private EventHandler execHandler;

		// Token: 0x04000AC3 RID: 2755
		private EventHandler statusHandler;

		// Token: 0x04000AC4 RID: 2756
		private CommandID commandID;

		// Token: 0x04000AC5 RID: 2757
		private int status;

		// Token: 0x04000AC6 RID: 2758
		private IDictionary properties;
	}
}
