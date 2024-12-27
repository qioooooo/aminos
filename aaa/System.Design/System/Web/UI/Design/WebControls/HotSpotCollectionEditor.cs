using System;
using System.ComponentModel.Design;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000459 RID: 1113
	public class HotSpotCollectionEditor : CollectionEditor
	{
		// Token: 0x060028A8 RID: 10408 RVA: 0x000DF4BA File Offset: 0x000DE4BA
		public HotSpotCollectionEditor(Type type)
			: base(type)
		{
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000DF4C3 File Offset: 0x000DE4C3
		protected override bool CanSelectMultipleInstances()
		{
			return false;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000DF4C8 File Offset: 0x000DE4C8
		protected override Type[] CreateNewItemTypes()
		{
			return new Type[]
			{
				typeof(CircleHotSpot),
				typeof(RectangleHotSpot),
				typeof(PolygonHotSpot)
			};
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x060028AB RID: 10411 RVA: 0x000DF504 File Offset: 0x000DE504
		protected override string HelpTopic
		{
			get
			{
				return "net.Asp.HotSpot.CollectionEditor";
			}
		}
	}
}
