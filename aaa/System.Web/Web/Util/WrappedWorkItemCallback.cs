using System;
using System.Runtime.InteropServices;

namespace System.Web.Util
{
	// Token: 0x020007A0 RID: 1952
	internal class WrappedWorkItemCallback
	{
		// Token: 0x06005D7F RID: 23935 RVA: 0x00176683 File Offset: 0x00175683
		internal WrappedWorkItemCallback(WorkItemCallback callback)
		{
			this._originalCallback = callback;
			this._wrapperCallback = new WorkItemCallback(this.OnCallback);
		}

		// Token: 0x06005D80 RID: 23936 RVA: 0x001766A4 File Offset: 0x001756A4
		internal void Post()
		{
			this._rootedThis = GCHandle.Alloc(this);
			if (UnsafeNativeMethods.PostThreadPoolWorkItem(this._wrapperCallback) != 1)
			{
				this._rootedThis.Free();
				throw new HttpException(SR.GetString("Cannot_post_workitem"));
			}
		}

		// Token: 0x06005D81 RID: 23937 RVA: 0x001766DB File Offset: 0x001756DB
		private void OnCallback()
		{
			this._rootedThis.Free();
			this._originalCallback();
		}

		// Token: 0x040031DE RID: 12766
		private GCHandle _rootedThis;

		// Token: 0x040031DF RID: 12767
		private WorkItemCallback _originalCallback;

		// Token: 0x040031E0 RID: 12768
		private WorkItemCallback _wrapperCallback;
	}
}
