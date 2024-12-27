using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200007E RID: 126
	public class ActiveDirectorySiteLinkCollection : CollectionBase
	{
		// Token: 0x0600038F RID: 911 RVA: 0x00012ABA File Offset: 0x00011ABA
		internal ActiveDirectorySiteLinkCollection()
		{
		}

		// Token: 0x170000E6 RID: 230
		public ActiveDirectorySiteLink this[int index]
		{
			get
			{
				return (ActiveDirectorySiteLink)base.InnerList[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!value.existing)
				{
					throw new InvalidOperationException(Res.GetString("SiteLinkNotCommitted", new object[] { value.Name }));
				}
				if (!this.Contains(value))
				{
					base.List[index] = value;
					return;
				}
				throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { value }), "value");
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00012B58 File Offset: 0x00011B58
		public int Add(ActiveDirectorySiteLink link)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}
			if (!link.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteLinkNotCommitted", new object[] { link.Name }));
			}
			if (!this.Contains(link))
			{
				return base.List.Add(link);
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { link }), "link");
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00012BD4 File Offset: 0x00011BD4
		public void AddRange(ActiveDirectorySiteLink[] links)
		{
			if (links == null)
			{
				throw new ArgumentNullException("links");
			}
			for (int i = 0; i < links.Length; i++)
			{
				this.Add(links[i]);
			}
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00012C08 File Offset: 0x00011C08
		public void AddRange(ActiveDirectorySiteLinkCollection links)
		{
			if (links == null)
			{
				throw new ArgumentNullException("links");
			}
			int count = links.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(links[i]);
			}
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00012C44 File Offset: 0x00011C44
		public bool Contains(ActiveDirectorySiteLink link)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}
			if (!link.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteLinkNotCommitted", new object[] { link.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(link.context, link.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySiteLink activeDirectorySiteLink = (ActiveDirectorySiteLink)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLink.context, activeDirectorySiteLink.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00012CF7 File Offset: 0x00011CF7
		public void CopyTo(ActiveDirectorySiteLink[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00012D08 File Offset: 0x00011D08
		public int IndexOf(ActiveDirectorySiteLink link)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}
			if (!link.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteLinkNotCommitted", new object[] { link.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(link.context, link.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySiteLink activeDirectorySiteLink = (ActiveDirectorySiteLink)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLink.context, activeDirectorySiteLink.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00012DBC File Offset: 0x00011DBC
		public void Insert(int index, ActiveDirectorySiteLink link)
		{
			if (link == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!link.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteLinkNotCommitted", new object[] { link.Name }));
			}
			if (!this.Contains(link))
			{
				base.List.Insert(index, link);
				return;
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { link }), "link");
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00012E38 File Offset: 0x00011E38
		public void Remove(ActiveDirectorySiteLink link)
		{
			if (link == null)
			{
				throw new ArgumentNullException("link");
			}
			if (!link.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteLinkNotCommitted", new object[] { link.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(link.context, link.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySiteLink activeDirectorySiteLink = (ActiveDirectorySiteLink)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLink.context, activeDirectorySiteLink.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					base.List.Remove(activeDirectorySiteLink);
					return;
				}
			}
			throw new ArgumentException(Res.GetString("NotFoundInCollection", new object[] { link }), "link");
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00012F18 File Offset: 0x00011F18
		protected override void OnClearComplete()
		{
			if (this.initialized)
			{
				try
				{
					if (this.de.Properties.Contains("siteLinkList"))
					{
						this.de.Properties["siteLinkList"].Clear();
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00012F80 File Offset: 0x00011F80
		protected override void OnInsertComplete(int index, object value)
		{
			if (this.initialized)
			{
				ActiveDirectorySiteLink activeDirectorySiteLink = (ActiveDirectorySiteLink)value;
				string text = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLink.context, activeDirectorySiteLink.cachedEntry, PropertyManager.DistinguishedName);
				try
				{
					this.de.Properties["siteLinkList"].Add(text);
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00012FF4 File Offset: 0x00011FF4
		protected override void OnRemoveComplete(int index, object value)
		{
			ActiveDirectorySiteLink activeDirectorySiteLink = (ActiveDirectorySiteLink)value;
			string text = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLink.context, activeDirectorySiteLink.cachedEntry, PropertyManager.DistinguishedName);
			try
			{
				this.de.Properties["siteLinkList"].Remove(text);
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00013060 File Offset: 0x00012060
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			ActiveDirectorySiteLink activeDirectorySiteLink = (ActiveDirectorySiteLink)newValue;
			string text = (string)PropertyManager.GetPropertyValue(activeDirectorySiteLink.context, activeDirectorySiteLink.cachedEntry, PropertyManager.DistinguishedName);
			try
			{
				this.de.Properties["siteLinkList"][index] = text;
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x0600039E RID: 926 RVA: 0x000130CC File Offset: 0x000120CC
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is ActiveDirectorySiteLink))
			{
				throw new ArgumentException("value");
			}
			if (!((ActiveDirectorySiteLink)value).existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteLinkNotCommitted", new object[] { ((ActiveDirectorySiteLink)value).Name }));
			}
		}

		// Token: 0x0400036B RID: 875
		internal DirectoryEntry de;

		// Token: 0x0400036C RID: 876
		internal bool initialized;

		// Token: 0x0400036D RID: 877
		internal DirectoryContext context;
	}
}
