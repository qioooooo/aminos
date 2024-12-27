using System;
using System.ComponentModel;

namespace System.Drawing.Printing
{
	// Token: 0x02000111 RID: 273
	[Serializable]
	public class PaperSource
	{
		// Token: 0x06000E78 RID: 3704 RVA: 0x0002B04E File Offset: 0x0002A04E
		public PaperSource()
		{
			this.kind = PaperSourceKind.Custom;
			this.name = string.Empty;
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0002B06C File Offset: 0x0002A06C
		internal PaperSource(PaperSourceKind kind, string name)
		{
			this.kind = kind;
			this.name = name;
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06000E7A RID: 3706 RVA: 0x0002B082 File Offset: 0x0002A082
		public PaperSourceKind Kind
		{
			get
			{
				if (this.kind >= (PaperSourceKind)256)
				{
					return PaperSourceKind.Custom;
				}
				return this.kind;
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06000E7B RID: 3707 RVA: 0x0002B09D File Offset: 0x0002A09D
		// (set) Token: 0x06000E7C RID: 3708 RVA: 0x0002B0A5 File Offset: 0x0002A0A5
		public int RawKind
		{
			get
			{
				return (int)this.kind;
			}
			set
			{
				this.kind = (PaperSourceKind)value;
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06000E7D RID: 3709 RVA: 0x0002B0AE File Offset: 0x0002A0AE
		// (set) Token: 0x06000E7E RID: 3710 RVA: 0x0002B0B6 File Offset: 0x0002A0B6
		public string SourceName
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x06000E7F RID: 3711 RVA: 0x0002B0C0 File Offset: 0x0002A0C0
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"[PaperSource ",
				this.SourceName,
				" Kind=",
				TypeDescriptor.GetConverter(typeof(PaperSourceKind)).ConvertToString(this.Kind),
				"]"
			});
		}

		// Token: 0x04000C17 RID: 3095
		private string name;

		// Token: 0x04000C18 RID: 3096
		private PaperSourceKind kind;
	}
}
