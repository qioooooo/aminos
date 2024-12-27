using System;
using System.Globalization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x02000438 RID: 1080
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public sealed class HtmlHistory : IDisposable
	{
		// Token: 0x060040D4 RID: 16596 RVA: 0x000E90EE File Offset: 0x000E80EE
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		internal HtmlHistory(UnsafeNativeMethods.IOmHistory history)
		{
			this.htmlHistory = history;
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x060040D5 RID: 16597 RVA: 0x000E90FD File Offset: 0x000E80FD
		private UnsafeNativeMethods.IOmHistory NativeOmHistory
		{
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.htmlHistory;
			}
		}

		// Token: 0x060040D6 RID: 16598 RVA: 0x000E911E File Offset: 0x000E811E
		public void Dispose()
		{
			this.htmlHistory = null;
			this.disposed = true;
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x060040D7 RID: 16599 RVA: 0x000E9134 File Offset: 0x000E8134
		public int Length
		{
			get
			{
				return (int)this.NativeOmHistory.GetLength();
			}
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x000E9144 File Offset: 0x000E8144
		public void Back(int numberBack)
		{
			if (numberBack < 0)
			{
				throw new ArgumentOutOfRangeException("numberBack", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"numberBack",
					numberBack.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (numberBack > 0)
			{
				object obj = -numberBack;
				this.NativeOmHistory.Go(ref obj);
			}
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x000E91B4 File Offset: 0x000E81B4
		public void Forward(int numberForward)
		{
			if (numberForward < 0)
			{
				throw new ArgumentOutOfRangeException("numberForward", SR.GetString("InvalidLowBoundArgumentEx", new object[]
				{
					"numberForward",
					numberForward.ToString(CultureInfo.CurrentCulture),
					0.ToString(CultureInfo.CurrentCulture)
				}));
			}
			if (numberForward > 0)
			{
				object obj = numberForward;
				this.NativeOmHistory.Go(ref obj);
			}
		}

		// Token: 0x060040DA RID: 16602 RVA: 0x000E9222 File Offset: 0x000E8222
		public void Go(Uri url)
		{
			this.Go(url.ToString());
		}

		// Token: 0x060040DB RID: 16603 RVA: 0x000E9230 File Offset: 0x000E8230
		public void Go(string urlString)
		{
			object obj = urlString;
			this.NativeOmHistory.Go(ref obj);
		}

		// Token: 0x060040DC RID: 16604 RVA: 0x000E924C File Offset: 0x000E824C
		public void Go(int relativePosition)
		{
			object obj = relativePosition;
			this.NativeOmHistory.Go(ref obj);
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x060040DD RID: 16605 RVA: 0x000E926D File Offset: 0x000E826D
		public object DomHistory
		{
			get
			{
				return this.NativeOmHistory;
			}
		}

		// Token: 0x04001F64 RID: 8036
		private UnsafeNativeMethods.IOmHistory htmlHistory;

		// Token: 0x04001F65 RID: 8037
		private bool disposed;
	}
}
