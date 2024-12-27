using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005B6 RID: 1462
	[Editor("System.Web.UI.Design.WebControls.HotSpotCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HotSpotCollection : StateManagedCollection
	{
		// Token: 0x1700119F RID: 4511
		public HotSpot this[int index]
		{
			get
			{
				return (HotSpot)((IList)this)[index];
			}
		}

		// Token: 0x06004786 RID: 18310 RVA: 0x0012466C File Offset: 0x0012366C
		public int Add(HotSpot spot)
		{
			return ((IList)this).Add(spot);
		}

		// Token: 0x06004787 RID: 18311 RVA: 0x00124678 File Offset: 0x00123678
		protected override object CreateKnownType(int index)
		{
			switch (index)
			{
			case 0:
				return new CircleHotSpot();
			case 1:
				return new RectangleHotSpot();
			case 2:
				return new PolygonHotSpot();
			default:
				throw new ArgumentOutOfRangeException(SR.GetString("HotSpotCollection_InvalidTypeIndex"));
			}
		}

		// Token: 0x06004788 RID: 18312 RVA: 0x001246BC File Offset: 0x001236BC
		protected override Type[] GetKnownTypes()
		{
			return HotSpotCollection.knownTypes;
		}

		// Token: 0x06004789 RID: 18313 RVA: 0x001246C3 File Offset: 0x001236C3
		public void Insert(int index, HotSpot spot)
		{
			((IList)this).Insert(index, spot);
		}

		// Token: 0x0600478A RID: 18314 RVA: 0x001246CD File Offset: 0x001236CD
		protected override void OnValidate(object o)
		{
			base.OnValidate(o);
			if (!(o is HotSpot))
			{
				throw new ArgumentException(SR.GetString("HotSpotCollection_InvalidType"));
			}
		}

		// Token: 0x0600478B RID: 18315 RVA: 0x001246EE File Offset: 0x001236EE
		public void Remove(HotSpot spot)
		{
			((IList)this).Remove(spot);
		}

		// Token: 0x0600478C RID: 18316 RVA: 0x001246F7 File Offset: 0x001236F7
		public void RemoveAt(int index)
		{
			((IList)this).RemoveAt(index);
		}

		// Token: 0x0600478D RID: 18317 RVA: 0x00124700 File Offset: 0x00123700
		protected override void SetDirtyObject(object o)
		{
			((HotSpot)o).SetDirty();
		}

		// Token: 0x04002AA3 RID: 10915
		private static readonly Type[] knownTypes = new Type[]
		{
			typeof(CircleHotSpot),
			typeof(RectangleHotSpot),
			typeof(PolygonHotSpot)
		};
	}
}
