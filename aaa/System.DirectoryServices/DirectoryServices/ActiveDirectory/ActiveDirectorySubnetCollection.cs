using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000080 RID: 128
	public class ActiveDirectorySubnetCollection : CollectionBase
	{
		// Token: 0x060003AF RID: 943 RVA: 0x000139DC File Offset: 0x000129DC
		internal ActiveDirectorySubnetCollection(DirectoryContext context, string siteDN)
		{
			this.context = context;
			this.siteDN = siteDN;
			Hashtable hashtable = new Hashtable();
			this.changeList = Hashtable.Synchronized(hashtable);
		}

		// Token: 0x170000EA RID: 234
		public ActiveDirectorySubnet this[int index]
		{
			get
			{
				return (ActiveDirectorySubnet)base.InnerList[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!value.existing)
				{
					throw new InvalidOperationException(Res.GetString("SubnetNotCommitted", new object[] { value.Name }));
				}
				if (!this.Contains(value))
				{
					base.List[index] = value;
					return;
				}
				throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { value }), "value");
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00013AB0 File Offset: 0x00012AB0
		public int Add(ActiveDirectorySubnet subnet)
		{
			if (subnet == null)
			{
				throw new ArgumentNullException("subnet");
			}
			if (!subnet.existing)
			{
				throw new InvalidOperationException(Res.GetString("SubnetNotCommitted", new object[] { subnet.Name }));
			}
			if (!this.Contains(subnet))
			{
				return base.List.Add(subnet);
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { subnet }), "subnet");
		}

		// Token: 0x060003B3 RID: 947 RVA: 0x00013B2C File Offset: 0x00012B2C
		public void AddRange(ActiveDirectorySubnet[] subnets)
		{
			if (subnets == null)
			{
				throw new ArgumentNullException("subnets");
			}
			for (int i = 0; i < subnets.Length; i++)
			{
				if (subnets[i] == null)
				{
					throw new ArgumentException("subnets");
				}
			}
			for (int j = 0; j < subnets.Length; j++)
			{
				this.Add(subnets[j]);
			}
		}

		// Token: 0x060003B4 RID: 948 RVA: 0x00013B84 File Offset: 0x00012B84
		public void AddRange(ActiveDirectorySubnetCollection subnets)
		{
			if (subnets == null)
			{
				throw new ArgumentNullException("subnets");
			}
			int count = subnets.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(subnets[i]);
			}
		}

		// Token: 0x060003B5 RID: 949 RVA: 0x00013BC0 File Offset: 0x00012BC0
		public bool Contains(ActiveDirectorySubnet subnet)
		{
			if (subnet == null)
			{
				throw new ArgumentNullException("subnet");
			}
			if (!subnet.existing)
			{
				throw new InvalidOperationException(Res.GetString("SubnetNotCommitted", new object[] { subnet.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(subnet.context, subnet.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySubnet activeDirectorySubnet = (ActiveDirectorySubnet)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySubnet.context, activeDirectorySubnet.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00013C73 File Offset: 0x00012C73
		public void CopyTo(ActiveDirectorySubnet[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060003B7 RID: 951 RVA: 0x00013C84 File Offset: 0x00012C84
		public int IndexOf(ActiveDirectorySubnet subnet)
		{
			if (subnet == null)
			{
				throw new ArgumentNullException("subnet");
			}
			if (!subnet.existing)
			{
				throw new InvalidOperationException(Res.GetString("SubnetNotCommitted", new object[] { subnet.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(subnet.context, subnet.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySubnet activeDirectorySubnet = (ActiveDirectorySubnet)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySubnet.context, activeDirectorySubnet.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060003B8 RID: 952 RVA: 0x00013D38 File Offset: 0x00012D38
		public void Insert(int index, ActiveDirectorySubnet subnet)
		{
			if (subnet == null)
			{
				throw new ArgumentNullException("subnet");
			}
			if (!subnet.existing)
			{
				throw new InvalidOperationException(Res.GetString("SubnetNotCommitted", new object[] { subnet.Name }));
			}
			if (!this.Contains(subnet))
			{
				base.List.Insert(index, subnet);
				return;
			}
			throw new ArgumentException(Res.GetString("AlreadyExistingInCollection", new object[] { subnet }), "subnet");
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00013DB4 File Offset: 0x00012DB4
		public void Remove(ActiveDirectorySubnet subnet)
		{
			if (subnet == null)
			{
				throw new ArgumentNullException("subnet");
			}
			if (!subnet.existing)
			{
				throw new InvalidOperationException(Res.GetString("SubnetNotCommitted", new object[] { subnet.Name }));
			}
			string text = (string)PropertyManager.GetPropertyValue(subnet.context, subnet.cachedEntry, PropertyManager.DistinguishedName);
			for (int i = 0; i < base.InnerList.Count; i++)
			{
				ActiveDirectorySubnet activeDirectorySubnet = (ActiveDirectorySubnet)base.InnerList[i];
				string text2 = (string)PropertyManager.GetPropertyValue(activeDirectorySubnet.context, activeDirectorySubnet.cachedEntry, PropertyManager.DistinguishedName);
				if (Utils.Compare(text2, text) == 0)
				{
					base.List.Remove(activeDirectorySubnet);
					return;
				}
			}
			throw new ArgumentException(Res.GetString("NotFoundInCollection", new object[] { subnet }), "subnet");
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00013E94 File Offset: 0x00012E94
		protected override void OnClear()
		{
			if (this.initialized)
			{
				this.copyList.Clear();
				foreach (object obj in base.List)
				{
					this.copyList.Add(obj);
				}
			}
		}

		// Token: 0x060003BB RID: 955 RVA: 0x00013F04 File Offset: 0x00012F04
		protected override void OnClearComplete()
		{
			if (this.initialized)
			{
				for (int i = 0; i < this.copyList.Count; i++)
				{
					this.OnRemoveComplete(i, this.copyList[i]);
				}
			}
		}

		// Token: 0x060003BC RID: 956 RVA: 0x00013F44 File Offset: 0x00012F44
		protected override void OnInsertComplete(int index, object value)
		{
			if (this.initialized)
			{
				ActiveDirectorySubnet activeDirectorySubnet = (ActiveDirectorySubnet)value;
				string text = (string)PropertyManager.GetPropertyValue(activeDirectorySubnet.context, activeDirectorySubnet.cachedEntry, PropertyManager.DistinguishedName);
				try
				{
					if (this.changeList.Contains(text))
					{
						((DirectoryEntry)this.changeList[text]).Properties["siteObject"].Value = this.siteDN;
					}
					else
					{
						DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.MakePath(text));
						directoryEntry.Properties["siteObject"].Value = this.siteDN;
						this.changeList.Add(text, directoryEntry);
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
		}

		// Token: 0x060003BD RID: 957 RVA: 0x00014018 File Offset: 0x00013018
		protected override void OnRemoveComplete(int index, object value)
		{
			ActiveDirectorySubnet activeDirectorySubnet = (ActiveDirectorySubnet)value;
			string text = (string)PropertyManager.GetPropertyValue(activeDirectorySubnet.context, activeDirectorySubnet.cachedEntry, PropertyManager.DistinguishedName);
			try
			{
				if (this.changeList.Contains(text))
				{
					((DirectoryEntry)this.changeList[text]).Properties["siteObject"].Clear();
				}
				else
				{
					DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.MakePath(text));
					directoryEntry.Properties["siteObject"].Clear();
					this.changeList.Add(text, directoryEntry);
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
		}

		// Token: 0x060003BE RID: 958 RVA: 0x000140D4 File Offset: 0x000130D4
		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			this.OnRemoveComplete(index, oldValue);
			this.OnInsertComplete(index, newValue);
		}

		// Token: 0x060003BF RID: 959 RVA: 0x000140E8 File Offset: 0x000130E8
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is ActiveDirectorySubnet))
			{
				throw new ArgumentException("value");
			}
			if (!((ActiveDirectorySubnet)value).existing)
			{
				throw new InvalidOperationException(Res.GetString("SubnetNotCommitted", new object[] { ((ActiveDirectorySubnet)value).Name }));
			}
		}

		// Token: 0x060003C0 RID: 960 RVA: 0x0001414C File Offset: 0x0001314C
		private string MakePath(string subnetDN)
		{
			string rdnFromDN = Utils.GetRdnFromDN(subnetDN);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < rdnFromDN.Length; i++)
			{
				if (rdnFromDN[i] == '/')
				{
					stringBuilder.Append('\\');
				}
				stringBuilder.Append(rdnFromDN[i]);
			}
			return stringBuilder.ToString() + "," + subnetDN.Substring(rdnFromDN.Length + 1);
		}

		// Token: 0x04000374 RID: 884
		internal Hashtable changeList;

		// Token: 0x04000375 RID: 885
		internal bool initialized;

		// Token: 0x04000376 RID: 886
		private string siteDN;

		// Token: 0x04000377 RID: 887
		private DirectoryContext context;

		// Token: 0x04000378 RID: 888
		private ArrayList copyList = new ArrayList();
	}
}
