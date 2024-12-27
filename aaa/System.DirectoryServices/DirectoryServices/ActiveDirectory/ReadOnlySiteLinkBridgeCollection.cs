using System;
using System.Collections;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000D5 RID: 213
	public class ReadOnlySiteLinkBridgeCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06000699 RID: 1689 RVA: 0x00022FED File Offset: 0x00021FED
		internal ReadOnlySiteLinkBridgeCollection()
		{
		}

		// Token: 0x1700018B RID: 395
		public ActiveDirectorySiteLinkBridge this[int index]
		{
			get
			{
				return (ActiveDirectorySiteLinkBridge)base.InnerList[index];
			}
		}

		// Token: 0x0600069B RID: 1691 RVA: 0x00023008 File Offset: 0x00022008
		public bool Contains(ActiveDirectorySiteLinkBridge bridge)
		{
			if (bridge == null)
			{
				throw new ArgumentNullException("bridge");
			}
			string text = (string)PropertyManager.GetPropertyValue(bridge.context, bridge.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySiteLinkBridge activeDirectorySiteLinkBridge = (ActiveDirectorySiteLinkBridge)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLinkBridge.context, activeDirectorySiteLinkBridge.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600069C RID: 1692 RVA: 0x00023090 File Offset: 0x00022090
		public int IndexOf(ActiveDirectorySiteLinkBridge bridge)
		{
			if (bridge == null)
			{
				throw new ArgumentNullException("bridge");
			}
			string text = (string)PropertyManager.GetPropertyValue(bridge.context, bridge.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySiteLinkBridge activeDirectorySiteLinkBridge = (ActiveDirectorySiteLinkBridge)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLinkBridge.context, activeDirectorySiteLinkBridge.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600069D RID: 1693 RVA: 0x00023117 File Offset: 0x00022117
		public void CopyTo(ActiveDirectorySiteLinkBridge[] bridges, int index)
		{
			base.InnerList.CopyTo(bridges, index);
		}

		// Token: 0x0600069E RID: 1694 RVA: 0x00023126 File Offset: 0x00022126
		internal int Add(ActiveDirectorySiteLinkBridge bridge)
		{
			return base.InnerList.Add(bridge);
		}

		// Token: 0x0600069F RID: 1695 RVA: 0x00023134 File Offset: 0x00022134
		internal void Clear()
		{
			base.InnerList.Clear();
		}
	}
}
