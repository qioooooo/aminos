using System;
using System.ComponentModel;
using System.Globalization;

namespace System.Drawing.Printing
{
	// Token: 0x02000110 RID: 272
	[Serializable]
	public class PaperSize
	{
		// Token: 0x06000E6B RID: 3691 RVA: 0x0002AE7D File Offset: 0x00029E7D
		public PaperSize()
		{
			this.kind = PaperKind.Custom;
			this.name = string.Empty;
			this.createdByDefaultConstructor = true;
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0002AE9E File Offset: 0x00029E9E
		internal PaperSize(PaperKind kind, string name, int width, int height)
		{
			this.kind = kind;
			this.name = name;
			this.width = width;
			this.height = height;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0002AEC3 File Offset: 0x00029EC3
		public PaperSize(string name, int width, int height)
		{
			this.kind = PaperKind.Custom;
			this.name = name;
			this.width = width;
			this.height = height;
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06000E6E RID: 3694 RVA: 0x0002AEE7 File Offset: 0x00029EE7
		// (set) Token: 0x06000E6F RID: 3695 RVA: 0x0002AEEF File Offset: 0x00029EEF
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				if (this.kind != PaperKind.Custom && !this.createdByDefaultConstructor)
				{
					throw new ArgumentException(SR.GetString("PSizeNotCustom"));
				}
				this.height = value;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x06000E70 RID: 3696 RVA: 0x0002AF18 File Offset: 0x00029F18
		public PaperKind Kind
		{
			get
			{
				if (this.kind <= PaperKind.PrcEnvelopeNumber10Rotated && this.kind != (PaperKind)48 && this.kind != (PaperKind)49)
				{
					return this.kind;
				}
				return PaperKind.Custom;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x06000E71 RID: 3697 RVA: 0x0002AF40 File Offset: 0x00029F40
		// (set) Token: 0x06000E72 RID: 3698 RVA: 0x0002AF48 File Offset: 0x00029F48
		public string PaperName
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.kind != PaperKind.Custom && !this.createdByDefaultConstructor)
				{
					throw new ArgumentException(SR.GetString("PSizeNotCustom"));
				}
				this.name = value;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06000E73 RID: 3699 RVA: 0x0002AF71 File Offset: 0x00029F71
		// (set) Token: 0x06000E74 RID: 3700 RVA: 0x0002AF79 File Offset: 0x00029F79
		public int RawKind
		{
			get
			{
				return (int)this.kind;
			}
			set
			{
				this.kind = (PaperKind)value;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06000E75 RID: 3701 RVA: 0x0002AF82 File Offset: 0x00029F82
		// (set) Token: 0x06000E76 RID: 3702 RVA: 0x0002AF8A File Offset: 0x00029F8A
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				if (this.kind != PaperKind.Custom && !this.createdByDefaultConstructor)
				{
					throw new ArgumentException(SR.GetString("PSizeNotCustom"));
				}
				this.width = value;
			}
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x0002AFB4 File Offset: 0x00029FB4
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[PaperSize ",
				this.PaperName,
				" Kind=",
				TypeDescriptor.GetConverter(typeof(PaperKind)).ConvertToString((int)this.Kind),
				" Height=",
				this.Height.ToString(CultureInfo.InvariantCulture),
				" Width=",
				this.Width.ToString(CultureInfo.InvariantCulture),
				"]"
			});
		}

		// Token: 0x04000C12 RID: 3090
		private PaperKind kind;

		// Token: 0x04000C13 RID: 3091
		private string name;

		// Token: 0x04000C14 RID: 3092
		private int width;

		// Token: 0x04000C15 RID: 3093
		private int height;

		// Token: 0x04000C16 RID: 3094
		private bool createdByDefaultConstructor;
	}
}
