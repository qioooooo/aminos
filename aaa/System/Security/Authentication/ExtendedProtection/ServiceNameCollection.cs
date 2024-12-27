using System;
using System.Collections;

namespace System.Security.Authentication.ExtendedProtection
{
	// Token: 0x0200034D RID: 845
	public class ServiceNameCollection : ReadOnlyCollectionBase
	{
		// Token: 0x06001A81 RID: 6785 RVA: 0x0005C8DC File Offset: 0x0005B8DC
		public ServiceNameCollection(ICollection items)
		{
			if (items == null)
			{
				throw new ArgumentNullException("items");
			}
			base.InnerList.AddRange(items);
		}

		// Token: 0x06001A82 RID: 6786 RVA: 0x0005C900 File Offset: 0x0005B900
		public ServiceNameCollection Merge(string serviceName)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(base.InnerList);
			this.AddIfNew(arrayList, serviceName);
			return new ServiceNameCollection(arrayList);
		}

		// Token: 0x06001A83 RID: 6787 RVA: 0x0005C930 File Offset: 0x0005B930
		public ServiceNameCollection Merge(IEnumerable serviceNames)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.AddRange(base.InnerList);
			foreach (object obj in serviceNames)
			{
				this.AddIfNew(arrayList, obj as string);
			}
			return new ServiceNameCollection(arrayList);
		}

		// Token: 0x06001A84 RID: 6788 RVA: 0x0005C9A4 File Offset: 0x0005B9A4
		private void AddIfNew(ArrayList newServiceNames, string serviceName)
		{
			if (string.IsNullOrEmpty(serviceName))
			{
				throw new ArgumentException(SR.GetString("security_ServiceNameCollection_EmptyServiceName"));
			}
			if (!this.Contains(serviceName, newServiceNames))
			{
				newServiceNames.Add(serviceName);
			}
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x0005C9D0 File Offset: 0x0005B9D0
		private bool Contains(string searchServiceName, ICollection serviceNames)
		{
			bool flag = false;
			foreach (object obj in serviceNames)
			{
				string text = (string)obj;
				if (string.Compare(text, searchServiceName, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					flag = true;
					break;
				}
			}
			return flag;
		}
	}
}
