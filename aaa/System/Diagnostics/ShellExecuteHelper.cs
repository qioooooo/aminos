using System;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.Win32;

namespace System.Diagnostics
{
	// Token: 0x0200077D RID: 1917
	internal class ShellExecuteHelper
	{
		// Token: 0x06003B30 RID: 15152 RVA: 0x000FBDD3 File Offset: 0x000FADD3
		public ShellExecuteHelper(NativeMethods.ShellExecuteInfo executeInfo)
		{
			this._executeInfo = executeInfo;
		}

		// Token: 0x06003B31 RID: 15153 RVA: 0x000FBDE4 File Offset: 0x000FADE4
		public void ShellExecuteFunction()
		{
			if (!(this._succeeded = NativeMethods.ShellExecuteEx(this._executeInfo)))
			{
				this._errorCode = Marshal.GetLastWin32Error();
			}
		}

		// Token: 0x06003B32 RID: 15154 RVA: 0x000FBE14 File Offset: 0x000FAE14
		public bool ShellExecuteOnSTAThread()
		{
			if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
			{
				ThreadStart threadStart = new ThreadStart(this.ShellExecuteFunction);
				Thread thread = new Thread(threadStart);
				thread.SetApartmentState(ApartmentState.STA);
				thread.Start();
				thread.Join();
			}
			else
			{
				this.ShellExecuteFunction();
			}
			return this._succeeded;
		}

		// Token: 0x17000DDE RID: 3550
		// (get) Token: 0x06003B33 RID: 15155 RVA: 0x000FBE62 File Offset: 0x000FAE62
		public int ErrorCode
		{
			get
			{
				return this._errorCode;
			}
		}

		// Token: 0x040033D1 RID: 13265
		private NativeMethods.ShellExecuteInfo _executeInfo;

		// Token: 0x040033D2 RID: 13266
		private int _errorCode;

		// Token: 0x040033D3 RID: 13267
		private bool _succeeded;
	}
}
