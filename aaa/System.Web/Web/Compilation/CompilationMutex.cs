using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Web.Util;

namespace System.Web.Compilation
{
	// Token: 0x02000165 RID: 357
	internal sealed class CompilationMutex : IDisposable
	{
		// Token: 0x06001012 RID: 4114 RVA: 0x00047934 File Offset: 0x00046934
		internal CompilationMutex(string name, string comment)
		{
			string text = (string)Misc.GetAspNetRegValue("CompilationMutexName", null, null);
			if (text != null)
			{
				string name2 = this._name;
				this._name = string.Concat(new string[] { name2, "Global\\", name, "-", text });
			}
			else
			{
				this._name = this._name + "Local\\" + name;
			}
			this._comment = comment;
			this._mutexHandle = new HandleRef(this, UnsafeNativeMethods.InstrumentedMutexCreate(this._name));
			if (this._mutexHandle.Handle == IntPtr.Zero)
			{
				throw new InvalidOperationException(SR.GetString("CompilationMutex_Create"));
			}
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x000479F0 File Offset: 0x000469F0
		~CompilationMutex()
		{
			this.Close();
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00047A1C File Offset: 0x00046A1C
		void IDisposable.Dispose()
		{
			this.Close();
			GC.SuppressFinalize(this);
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00047A2A File Offset: 0x00046A2A
		internal void Close()
		{
			if (this._mutexHandle.Handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.InstrumentedMutexDelete(this._mutexHandle);
				this._mutexHandle = new HandleRef(this, IntPtr.Zero);
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00047A60 File Offset: 0x00046A60
		internal void WaitOne()
		{
			if (this._mutexHandle.Handle == IntPtr.Zero)
			{
				throw new InvalidOperationException(SR.GetString("CompilationMutex_Null"));
			}
			for (;;)
			{
				int lockStatus = this._lockStatus;
				if (lockStatus == -1 || this._draining)
				{
					break;
				}
				if (Interlocked.CompareExchange(ref this._lockStatus, lockStatus + 1, lockStatus) == lockStatus)
				{
					goto Block_3;
				}
			}
			throw new InvalidOperationException(SR.GetString("CompilationMutex_Drained"));
			Block_3:
			if (UnsafeNativeMethods.InstrumentedMutexGetLock(this._mutexHandle, -1) == -1)
			{
				Interlocked.Decrement(ref this._lockStatus);
				throw new InvalidOperationException(SR.GetString("CompilationMutex_Failed"));
			}
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00047AF4 File Offset: 0x00046AF4
		internal void ReleaseMutex()
		{
			if (this._mutexHandle.Handle == IntPtr.Zero)
			{
				throw new InvalidOperationException(SR.GetString("CompilationMutex_Null"));
			}
			if (UnsafeNativeMethods.InstrumentedMutexReleaseLock(this._mutexHandle) != 0)
			{
				Interlocked.Decrement(ref this._lockStatus);
			}
		}

		// Token: 0x17000407 RID: 1031
		// (get) Token: 0x06001018 RID: 4120 RVA: 0x00047B41 File Offset: 0x00046B41
		private string MutexDebugName
		{
			get
			{
				return this._name;
			}
		}

		// Token: 0x0400163A RID: 5690
		private string _name;

		// Token: 0x0400163B RID: 5691
		private string _comment;

		// Token: 0x0400163C RID: 5692
		private HandleRef _mutexHandle;

		// Token: 0x0400163D RID: 5693
		private int _lockStatus;

		// Token: 0x0400163E RID: 5694
		private bool _draining;
	}
}
