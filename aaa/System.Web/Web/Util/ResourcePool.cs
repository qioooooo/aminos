using System;
using System.Collections;
using System.Threading;

namespace System.Web.Util
{
	// Token: 0x0200077D RID: 1917
	internal class ResourcePool : IDisposable
	{
		// Token: 0x06005CA8 RID: 23720 RVA: 0x001734CE File Offset: 0x001724CE
		internal ResourcePool(TimeSpan interval, int max)
		{
			this._interval = interval;
			this._resources = new ArrayList(4);
			this._max = max;
			this._callback = new TimerCallback(this.TimerProc);
		}

		// Token: 0x06005CA9 RID: 23721 RVA: 0x00173502 File Offset: 0x00172502
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x00173514 File Offset: 0x00172514
		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				lock (this)
				{
					if (!this._disposed)
					{
						if (this._resources != null)
						{
							foreach (object obj in this._resources)
							{
								IDisposable disposable = (IDisposable)obj;
								disposable.Dispose();
							}
							this._resources.Clear();
						}
						if (this._timer != null)
						{
							this._timer.Dispose();
							this._timer = null;
						}
						this._disposed = true;
					}
				}
			}
		}

		// Token: 0x06005CAB RID: 23723 RVA: 0x001735CC File Offset: 0x001725CC
		internal object RetrieveResource()
		{
			object obj = null;
			if (this._resources.Count != 0)
			{
				lock (this)
				{
					if (!this._disposed)
					{
						if (this._resources.Count == 0)
						{
							obj = null;
						}
						else
						{
							obj = this._resources[this._resources.Count - 1];
							this._resources.RemoveAt(this._resources.Count - 1);
							if (this._resources.Count < this._iDisposable)
							{
								this._iDisposable = this._resources.Count;
							}
						}
					}
				}
			}
			return obj;
		}

		// Token: 0x06005CAC RID: 23724 RVA: 0x00173678 File Offset: 0x00172678
		internal void StoreResource(IDisposable o)
		{
			lock (this)
			{
				if (!this._disposed && this._resources.Count < this._max)
				{
					this._resources.Add(o);
					o = null;
					if (this._timer == null)
					{
						this._timer = new Timer(this._callback, null, this._interval, this._interval);
					}
				}
			}
			if (o != null)
			{
				o.Dispose();
			}
		}

		// Token: 0x06005CAD RID: 23725 RVA: 0x00173700 File Offset: 0x00172700
		private void TimerProc(object userData)
		{
			IDisposable[] array = null;
			lock (this)
			{
				if (!this._disposed)
				{
					if (this._resources.Count == 0)
					{
						if (this._timer != null)
						{
							this._timer.Dispose();
							this._timer = null;
						}
						return;
					}
					array = new IDisposable[this._iDisposable];
					this._resources.CopyTo(0, array, 0, this._iDisposable);
					this._resources.RemoveRange(0, this._iDisposable);
					this._iDisposable = this._resources.Count;
				}
			}
			if (array != null)
			{
				for (int i = 0; i < array.Length; i++)
				{
					try
					{
						array[i].Dispose();
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x04003180 RID: 12672
		private ArrayList _resources;

		// Token: 0x04003181 RID: 12673
		private int _iDisposable;

		// Token: 0x04003182 RID: 12674
		private int _max;

		// Token: 0x04003183 RID: 12675
		private Timer _timer;

		// Token: 0x04003184 RID: 12676
		private TimerCallback _callback;

		// Token: 0x04003185 RID: 12677
		private TimeSpan _interval;

		// Token: 0x04003186 RID: 12678
		private bool _disposed;
	}
}
