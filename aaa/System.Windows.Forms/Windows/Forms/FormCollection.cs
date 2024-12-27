using System;
using System.Collections;

namespace System.Windows.Forms
{
	// Token: 0x02000412 RID: 1042
	public class FormCollection : ReadOnlyCollectionBase
	{
		// Token: 0x17000BEB RID: 3051
		public virtual Form this[string name]
		{
			get
			{
				if (name != null)
				{
					lock (FormCollection.CollectionSyncRoot)
					{
						foreach (object obj in base.InnerList)
						{
							Form form = (Form)obj;
							if (string.Equals(form.Name, name, StringComparison.OrdinalIgnoreCase))
							{
								return form;
							}
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17000BEC RID: 3052
		public virtual Form this[int index]
		{
			get
			{
				Form form = null;
				lock (FormCollection.CollectionSyncRoot)
				{
					form = (Form)base.InnerList[index];
				}
				return form;
			}
		}

		// Token: 0x06003E51 RID: 15953 RVA: 0x000E3200 File Offset: 0x000E2200
		internal void Add(Form form)
		{
			lock (FormCollection.CollectionSyncRoot)
			{
				base.InnerList.Add(form);
			}
		}

		// Token: 0x06003E52 RID: 15954 RVA: 0x000E3240 File Offset: 0x000E2240
		internal bool Contains(Form form)
		{
			bool flag = false;
			lock (FormCollection.CollectionSyncRoot)
			{
				flag = base.InnerList.Contains(form);
			}
			return flag;
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x000E3284 File Offset: 0x000E2284
		internal void Remove(Form form)
		{
			lock (FormCollection.CollectionSyncRoot)
			{
				base.InnerList.Remove(form);
			}
		}

		// Token: 0x04001EB2 RID: 7858
		internal static object CollectionSyncRoot = new object();
	}
}
