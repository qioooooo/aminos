using System;
using System.Collections;
using System.Collections.Specialized;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006B9 RID: 1721
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ConsumerConnectionPointCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06005468 RID: 21608 RVA: 0x0015801F File Offset: 0x0015701F
		public ConsumerConnectionPointCollection()
		{
		}

		// Token: 0x06005469 RID: 21609 RVA: 0x00158028 File Offset: 0x00157028
		public ConsumerConnectionPointCollection(ICollection connectionPoints)
		{
			if (connectionPoints == null)
			{
				throw new ArgumentNullException("connectionPoints");
			}
			this._ids = new HybridDictionary(connectionPoints.Count, true);
			foreach (object obj in connectionPoints)
			{
				if (obj == null)
				{
					throw new ArgumentException(SR.GetString("Collection_CantAddNull"), "connectionPoints");
				}
				ConsumerConnectionPoint consumerConnectionPoint = obj as ConsumerConnectionPoint;
				if (consumerConnectionPoint == null)
				{
					throw new ArgumentException(SR.GetString("Collection_InvalidType", new object[] { "ConsumerConnectionPoint" }), "connectionPoints");
				}
				string id = consumerConnectionPoint.ID;
				if (this._ids.Contains(id))
				{
					throw new ArgumentException(SR.GetString("WebPart_Collection_DuplicateID", new object[] { "ConsumerConnectionPoint", id }), "connectionPoints");
				}
				base.InnerList.Add(consumerConnectionPoint);
				this._ids.Add(id, consumerConnectionPoint);
			}
		}

		// Token: 0x17001590 RID: 5520
		// (get) Token: 0x0600546A RID: 21610 RVA: 0x00158148 File Offset: 0x00157148
		public ConsumerConnectionPoint Default
		{
			get
			{
				return this[ConnectionPoint.DefaultID];
			}
		}

		// Token: 0x17001591 RID: 5521
		public ConsumerConnectionPoint this[int index]
		{
			get
			{
				return (ConsumerConnectionPoint)base.InnerList[index];
			}
		}

		// Token: 0x17001592 RID: 5522
		public ConsumerConnectionPoint this[string id]
		{
			get
			{
				if (this._ids == null)
				{
					return null;
				}
				return (ConsumerConnectionPoint)this._ids[id];
			}
		}

		// Token: 0x0600546D RID: 21613 RVA: 0x00158185 File Offset: 0x00157185
		public bool Contains(ConsumerConnectionPoint connectionPoint)
		{
			return base.InnerList.Contains(connectionPoint);
		}

		// Token: 0x0600546E RID: 21614 RVA: 0x00158193 File Offset: 0x00157193
		public int IndexOf(ConsumerConnectionPoint connectionPoint)
		{
			return base.InnerList.IndexOf(connectionPoint);
		}

		// Token: 0x0600546F RID: 21615 RVA: 0x001581A1 File Offset: 0x001571A1
		public void CopyTo(ConsumerConnectionPoint[] array, int index)
		{
			base.InnerList.CopyTo(array, index);
		}

		// Token: 0x04002EE1 RID: 12001
		private HybridDictionary _ids;
	}
}
