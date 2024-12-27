using System;
using System.Drawing;
using System.Globalization;

namespace System.Windows.Forms.Internal
{
	// Token: 0x0200002F RID: 47
	internal sealed class WindowsSolidBrush : WindowsBrush
	{
		// Token: 0x0600014E RID: 334 RVA: 0x000055CC File Offset: 0x000045CC
		protected override void CreateBrush()
		{
			IntPtr intPtr = IntSafeNativeMethods.CreateSolidBrush(ColorTranslator.ToWin32(base.Color));
			intPtr == IntPtr.Zero;
			base.NativeHandle = intPtr;
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000055FD File Offset: 0x000045FD
		public WindowsSolidBrush(DeviceContext dc)
			: base(dc)
		{
		}

		// Token: 0x06000150 RID: 336 RVA: 0x00005606 File Offset: 0x00004606
		public WindowsSolidBrush(DeviceContext dc, Color color)
			: base(dc, color)
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00005610 File Offset: 0x00004610
		public override object Clone()
		{
			return new WindowsSolidBrush(base.DC, base.Color);
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00005624 File Offset: 0x00004624
		public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "{0}: Color={1}", new object[]
			{
				base.GetType().Name,
				base.Color
			});
		}
	}
}
