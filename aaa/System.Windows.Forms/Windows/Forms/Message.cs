using System;
using System.Security;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020004BE RID: 1214
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public struct Message
	{
		// Token: 0x17000E7F RID: 3711
		// (get) Token: 0x0600488D RID: 18573 RVA: 0x00107B34 File Offset: 0x00106B34
		// (set) Token: 0x0600488E RID: 18574 RVA: 0x00107B3C File Offset: 0x00106B3C
		public IntPtr HWnd
		{
			get
			{
				return this.hWnd;
			}
			set
			{
				this.hWnd = value;
			}
		}

		// Token: 0x17000E80 RID: 3712
		// (get) Token: 0x0600488F RID: 18575 RVA: 0x00107B45 File Offset: 0x00106B45
		// (set) Token: 0x06004890 RID: 18576 RVA: 0x00107B4D File Offset: 0x00106B4D
		public int Msg
		{
			get
			{
				return this.msg;
			}
			set
			{
				this.msg = value;
			}
		}

		// Token: 0x17000E81 RID: 3713
		// (get) Token: 0x06004891 RID: 18577 RVA: 0x00107B56 File Offset: 0x00106B56
		// (set) Token: 0x06004892 RID: 18578 RVA: 0x00107B5E File Offset: 0x00106B5E
		public IntPtr WParam
		{
			get
			{
				return this.wparam;
			}
			set
			{
				this.wparam = value;
			}
		}

		// Token: 0x17000E82 RID: 3714
		// (get) Token: 0x06004893 RID: 18579 RVA: 0x00107B67 File Offset: 0x00106B67
		// (set) Token: 0x06004894 RID: 18580 RVA: 0x00107B6F File Offset: 0x00106B6F
		public IntPtr LParam
		{
			get
			{
				return this.lparam;
			}
			set
			{
				this.lparam = value;
			}
		}

		// Token: 0x17000E83 RID: 3715
		// (get) Token: 0x06004895 RID: 18581 RVA: 0x00107B78 File Offset: 0x00106B78
		// (set) Token: 0x06004896 RID: 18582 RVA: 0x00107B80 File Offset: 0x00106B80
		public IntPtr Result
		{
			get
			{
				return this.result;
			}
			set
			{
				this.result = value;
			}
		}

		// Token: 0x06004897 RID: 18583 RVA: 0x00107B89 File Offset: 0x00106B89
		public object GetLParam(Type cls)
		{
			return UnsafeNativeMethods.PtrToStructure(this.lparam, cls);
		}

		// Token: 0x06004898 RID: 18584 RVA: 0x00107B98 File Offset: 0x00106B98
		public static Message Create(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam)
		{
			return new Message
			{
				hWnd = hWnd,
				msg = msg,
				wparam = wparam,
				lparam = lparam,
				result = IntPtr.Zero
			};
		}

		// Token: 0x06004899 RID: 18585 RVA: 0x00107BDC File Offset: 0x00106BDC
		public override bool Equals(object o)
		{
			if (!(o is Message))
			{
				return false;
			}
			Message message = (Message)o;
			return this.hWnd == message.hWnd && this.msg == message.msg && this.wparam == message.wparam && this.lparam == message.lparam && this.result == message.result;
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x00107C59 File Offset: 0x00106C59
		public static bool operator !=(Message a, Message b)
		{
			return !a.Equals(b);
		}

		// Token: 0x0600489B RID: 18587 RVA: 0x00107C71 File Offset: 0x00106C71
		public static bool operator ==(Message a, Message b)
		{
			return a.Equals(b);
		}

		// Token: 0x0600489C RID: 18588 RVA: 0x00107C86 File Offset: 0x00106C86
		public override int GetHashCode()
		{
			return ((int)this.hWnd << 4) | this.msg;
		}

		// Token: 0x0600489D RID: 18589 RVA: 0x00107C9C File Offset: 0x00106C9C
		public override string ToString()
		{
			bool flag = false;
			try
			{
				IntSecurity.UnmanagedCode.Demand();
				flag = true;
			}
			catch (SecurityException)
			{
			}
			if (flag)
			{
				return MessageDecoder.ToString(this);
			}
			return base.ToString();
		}

		// Token: 0x0400224B RID: 8779
		private IntPtr hWnd;

		// Token: 0x0400224C RID: 8780
		private int msg;

		// Token: 0x0400224D RID: 8781
		private IntPtr wparam;

		// Token: 0x0400224E RID: 8782
		private IntPtr lparam;

		// Token: 0x0400224F RID: 8783
		private IntPtr result;
	}
}
