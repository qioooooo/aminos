using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Drawing.Printing
{
	// Token: 0x02000117 RID: 279
	[Serializable]
	public class PrinterResolution
	{
		// Token: 0x06000EAA RID: 3754 RVA: 0x0002B79A File Offset: 0x0002A79A
		public PrinterResolution()
		{
			this.kind = PrinterResolutionKind.Custom;
		}

		// Token: 0x06000EAB RID: 3755 RVA: 0x0002B7A9 File Offset: 0x0002A7A9
		internal PrinterResolution(PrinterResolutionKind kind, int x, int y)
		{
			this.kind = kind;
			this.x = x;
			this.y = y;
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x06000EAC RID: 3756 RVA: 0x0002B7C6 File Offset: 0x0002A7C6
		// (set) Token: 0x06000EAD RID: 3757 RVA: 0x0002B7CE File Offset: 0x0002A7CE
		public PrinterResolutionKind Kind
		{
			get
			{
				return this.kind;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, -4, 0))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PrinterResolutionKind));
				}
				this.kind = value;
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06000EAE RID: 3758 RVA: 0x0002B7FE File Offset: 0x0002A7FE
		// (set) Token: 0x06000EAF RID: 3759 RVA: 0x0002B806 File Offset: 0x0002A806
		public int X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x = value;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06000EB0 RID: 3760 RVA: 0x0002B80F File Offset: 0x0002A80F
		// (set) Token: 0x06000EB1 RID: 3761 RVA: 0x0002B817 File Offset: 0x0002A817
		public int Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y = value;
			}
		}

		// Token: 0x06000EB2 RID: 3762 RVA: 0x0002B820 File Offset: 0x0002A820
		public override string ToString()
		{
			if (this.kind != PrinterResolutionKind.Custom)
			{
				return "[PrinterResolution " + TypeDescriptor.GetConverter(typeof(PrinterResolutionKind)).ConvertToString((int)this.Kind) + "]";
			}
			return string.Concat(new string[]
			{
				"[PrinterResolution X=",
				this.X.ToString(CultureInfo.InvariantCulture),
				" Y=",
				this.Y.ToString(CultureInfo.InvariantCulture),
				"]"
			});
		}

		// Token: 0x04000C3C RID: 3132
		private int x;

		// Token: 0x04000C3D RID: 3133
		private int y;

		// Token: 0x04000C3E RID: 3134
		private PrinterResolutionKind kind;
	}
}
