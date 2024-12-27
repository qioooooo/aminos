using System;
using System.ComponentModel;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004F7 RID: 1271
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class CircleHotSpot : HotSpot
	{
		// Token: 0x17000E9B RID: 3739
		// (get) Token: 0x06003E0B RID: 15883 RVA: 0x00103497 File Offset: 0x00102497
		protected internal override string MarkupName
		{
			get
			{
				return "circle";
			}
		}

		// Token: 0x17000E9C RID: 3740
		// (get) Token: 0x06003E0C RID: 15884 RVA: 0x001034A0 File Offset: 0x001024A0
		// (set) Token: 0x06003E0D RID: 15885 RVA: 0x001034C9 File Offset: 0x001024C9
		[WebCategory("Appearance")]
		[WebSysDescription("CircleHotSpot_Radius")]
		[DefaultValue(0)]
		public int Radius
		{
			get
			{
				object obj = base.ViewState["Radius"];
				if (obj != null)
				{
					return (int)obj;
				}
				return 0;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				base.ViewState["Radius"] = value;
			}
		}

		// Token: 0x17000E9D RID: 3741
		// (get) Token: 0x06003E0E RID: 15886 RVA: 0x001034F0 File Offset: 0x001024F0
		// (set) Token: 0x06003E0F RID: 15887 RVA: 0x00103519 File Offset: 0x00102519
		[DefaultValue(0)]
		[WebCategory("Appearance")]
		[WebSysDescription("CircleHotSpot_X")]
		public int X
		{
			get
			{
				object obj = base.ViewState["X"];
				if (obj == null)
				{
					return 0;
				}
				return (int)obj;
			}
			set
			{
				base.ViewState["X"] = value;
			}
		}

		// Token: 0x17000E9E RID: 3742
		// (get) Token: 0x06003E10 RID: 15888 RVA: 0x00103534 File Offset: 0x00102534
		// (set) Token: 0x06003E11 RID: 15889 RVA: 0x0010355D File Offset: 0x0010255D
		[WebSysDescription("CircleHotSpot_Y")]
		[WebCategory("Appearance")]
		[DefaultValue(0)]
		public int Y
		{
			get
			{
				object obj = base.ViewState["Y"];
				if (obj == null)
				{
					return 0;
				}
				return (int)obj;
			}
			set
			{
				base.ViewState["Y"] = value;
			}
		}

		// Token: 0x06003E12 RID: 15890 RVA: 0x00103578 File Offset: 0x00102578
		public override string GetCoordinates()
		{
			return string.Concat(new object[] { this.X, ",", this.Y, ",", this.Radius });
		}
	}
}
