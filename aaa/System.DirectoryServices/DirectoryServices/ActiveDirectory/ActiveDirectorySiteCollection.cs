using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200007B RID: 123
	public class ActiveDirectorySiteCollection : CollectionBase
	{
		// Token: 0x06000351 RID: 849 RVA: 0x00010DCC File Offset: 0x0000FDCC
		internal ActiveDirectorySiteCollection()
		{
		}

		// Token: 0x06000352 RID: 850 RVA: 0x00010DD4 File Offset: 0x0000FDD4
		internal ActiveDirectorySiteCollection(ArrayList sites)
		{
			for (int i = 0; i < sites.Count; i++)
			{
				this.Add((ActiveDirectorySite)sites[i]);
			}
		}

		// Token: 0x170000D9 RID: 217
		public ActiveDirectorySite this[int index]
		{
			get
			{
				return (ActiveDirectorySite)base.InnerList[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!value.existing)
				{
					throw new InvalidOperationException(Res.GetString("SiteNotCommitted", new object[] { value.Name }));
				}
				if (!this.Contains(value))
				{
					base.List[index] = value;
					return;
				}
				throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { value }), "value");
			}
		}

		// Token: 0x06000355 RID: 853 RVA: 0x00010EA0 File Offset: 0x0000FEA0
		public int Add(ActiveDirectorySite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			if (!site.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteNotCommitted", new object[] { site.Name }));
			}
			if (!this.Contains(site))
			{
				return base.List.Add(site);
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { site }), "site");
		}

		// Token: 0x06000356 RID: 854 RVA: 0x00010F1C File Offset: 0x0000FF1C
		public void AddRange(ActiveDirectorySite[] sites)
		{
			if (sites == null)
			{
				throw new ArgumentNullException("sites");
			}
			for (int i = 0; i < sites.Length; i++)
			{
				this.Add(sites[i]);
			}
		}

		// Token: 0x06000357 RID: 855 RVA: 0x00010F50 File Offset: 0x0000FF50
		public void AddRange(ActiveDirectorySiteCollection sites)
		{
			if (sites == null)
			{
				throw new ArgumentNullException("sites");
			}
			int count = sites.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(sites[i]);
			}
		}

		// Token: 0x06000358 RID: 856 RVA: 0x00010F8C File Offset: 0x0000FF8C
		public bool Contains(ActiveDirectorySite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			if (!site.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteNotCommitted", new object[] { site.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(site.context, site.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySite activeDirectorySite = (ActiveDirectorySite)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySite.context, activeDirectorySite.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000359 RID: 857 RVA: 0x0001103F File Offset: 0x0001003F
		public void CopyTo(ActiveDirectorySite[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x0600035A RID: 858 RVA: 0x00011050 File Offset: 0x00010050
		public int IndexOf(ActiveDirectorySite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			if (!site.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteNotCommitted", new object[] { site.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(site.context, site.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySite activeDirectorySite = (ActiveDirectorySite)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySite.context, activeDirectorySite.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x0600035B RID: 859 RVA: 0x00011104 File Offset: 0x00010104
		public void Insert(int index, ActiveDirectorySite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			if (!site.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteNotCommitted", new object[] { site.Name }));
			}
			if (!this.Contains(site))
			{
				base.List.Insert(index, site);
				return;
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { site }), "site");
		}

		// Token: 0x0600035C RID: 860 RVA: 0x00011180 File Offset: 0x00010180
		public void Remove(ActiveDirectorySite site)
		{
			if (site == null)
			{
				throw new ArgumentNullException("site");
			}
			if (!site.existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteNotCommitted", new object[] { site.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(site.context, site.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySite activeDirectorySite = (ActiveDirectorySite)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySite.context, activeDirectorySite.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					base.List.Remove(activeDirectorySite);
					return;
				}
			}
			throw new ArgumentException(Res.GetString("NotFoundInCollection", new object[] { site }), "site");
		}

		// Token: 0x0600035D RID: 861 RVA: 0x00011260 File Offset: 0x00010260
		protected override void OnClearComplete()
		{
			if (this.initialized)
			{
				try
				{
					if (this.de.Properties.Contains("siteList"))
					{
						this.de.Properties["siteList"].Clear();
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x0600035E RID: 862 RVA: 0x000112C8 File Offset: 0x000102C8
		protected override void OnInsertComplete(int index, object value)
		{
			if (this.initialized)
			{
				ActiveDirectorySite activeDirectorySite = (ActiveDirectorySite)value;
				string text = (string)PropertyManager.GetPropertyValue(activeDirectorySite.context, activeDirectorySite.cachedEntry, PropertyManager.DistinguishedName);
				try
				{
					this.de.Properties["siteList"].Add(text);
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x0600035F RID: 863 RVA: 0x0001133C File Offset: 0x0001033C
		protected override void OnRemoveComplete(int index, object value)
		{
			ActiveDirectorySite activeDirectorySite = (ActiveDirectorySite)value;
			string text = (string)PropertyManager.GetPropertyValue(activeDirectorySite.context, activeDirectorySite.cachedEntry, PropertyManager.DistinguishedName);
			try
			{
				this.de.Properties["siteList"].Remove(text);
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x06000360 RID: 864 RVA: 0x000113A8 File Offset: 0x000103A8
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			ActiveDirectorySite activeDirectorySite = (ActiveDirectorySite)newValue;
			string text = (string)PropertyManager.GetPropertyValue(activeDirectorySite.context, activeDirectorySite.cachedEntry, PropertyManager.DistinguishedName);
			try
			{
				this.de.Properties["siteList"][index] = text;
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x06000361 RID: 865 RVA: 0x00011414 File Offset: 0x00010414
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is ActiveDirectorySite))
			{
				throw new ArgumentException("value");
			}
			if (!((ActiveDirectorySite)value).existing)
			{
				throw new InvalidOperationException(Res.GetString("SiteNotCommitted", new object[] { ((ActiveDirectorySite)value).Name }));
			}
		}

		// Token: 0x04000354 RID: 852
		internal DirectoryEntry de;

		// Token: 0x04000355 RID: 853
		internal bool initialized;

		// Token: 0x04000356 RID: 854
		internal DirectoryContext context;
	}
}
