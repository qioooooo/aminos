using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.Management
{
	// Token: 0x020002FB RID: 763
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class WebBaseEventCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06002606 RID: 9734 RVA: 0x000A2C70 File Offset: 0x000A1C70
		public WebBaseEventCollection(ICollection events)
		{
			if (events == null)
			{
				throw new ArgumentNullException("events");
			}
			foreach (object obj in events)
			{
				WebBaseEvent webBaseEvent = (WebBaseEvent)obj;
				base.InnerList.Add(webBaseEvent);
			}
		}

		// Token: 0x06002607 RID: 9735 RVA: 0x000A2CE0 File Offset: 0x000A1CE0
		internal WebBaseEventCollection(WebBaseEvent eventRaised)
		{
			if (eventRaised == null)
			{
				throw new ArgumentNullException("eventRaised");
			}
			base.InnerList.Add(eventRaised);
		}

		// Token: 0x170007ED RID: 2029
		public WebBaseEvent this[int index]
		{
			get
			{
				return (WebBaseEvent)base.InnerList[index];
			}
		}

		// Token: 0x06002609 RID: 9737 RVA: 0x000A2D16 File Offset: 0x000A1D16
		public int IndexOf(WebBaseEvent value)
		{
			return base.InnerList.IndexOf(value);
		}

		// Token: 0x0600260A RID: 9738 RVA: 0x000A2D24 File Offset: 0x000A1D24
		public bool Contains(WebBaseEvent value)
		{
			return base.InnerList.Contains(value);
		}
	}
}
