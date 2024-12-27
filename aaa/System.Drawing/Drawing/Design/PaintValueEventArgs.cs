using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Drawing.Design
{
	// Token: 0x020000F8 RID: 248
	[PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
	[PermissionSet(SecurityAction.LinkDemand, Name = "FullTrust")]
	public class PaintValueEventArgs : EventArgs
	{
		// Token: 0x06000DAA RID: 3498 RVA: 0x00028129 File Offset: 0x00027129
		public PaintValueEventArgs(ITypeDescriptorContext context, object value, Graphics graphics, Rectangle bounds)
		{
			this.context = context;
			this.valueToPaint = value;
			this.graphics = graphics;
			if (graphics == null)
			{
				throw new ArgumentNullException("graphics");
			}
			this.bounds = bounds;
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000DAB RID: 3499 RVA: 0x0002815C File Offset: 0x0002715C
		public Rectangle Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00028164 File Offset: 0x00027164
		public ITypeDescriptorContext Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06000DAD RID: 3501 RVA: 0x0002816C File Offset: 0x0002716C
		public Graphics Graphics
		{
			get
			{
				return this.graphics;
			}
		}

		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x00028174 File Offset: 0x00027174
		public object Value
		{
			get
			{
				return this.valueToPaint;
			}
		}

		// Token: 0x04000B66 RID: 2918
		private readonly ITypeDescriptorContext context;

		// Token: 0x04000B67 RID: 2919
		private readonly object valueToPaint;

		// Token: 0x04000B68 RID: 2920
		private readonly Graphics graphics;

		// Token: 0x04000B69 RID: 2921
		private readonly Rectangle bounds;
	}
}
