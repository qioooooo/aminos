using System;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace System.Management
{
	// Token: 0x0200001D RID: 29
	public class ManagementNamedValueCollection : NameObjectCollectionBase
	{
		// Token: 0x14000004 RID: 4
		// (add) Token: 0x060000ED RID: 237 RVA: 0x00006F90 File Offset: 0x00005F90
		// (remove) Token: 0x060000EE RID: 238 RVA: 0x00006FA9 File Offset: 0x00005FA9
		internal event IdentifierChangedEventHandler IdentifierChanged;

		// Token: 0x060000EF RID: 239 RVA: 0x00006FC2 File Offset: 0x00005FC2
		private void FireIdentifierChanged()
		{
			if (this.IdentifierChanged != null)
			{
				this.IdentifierChanged(this, null);
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00006FD9 File Offset: 0x00005FD9
		public ManagementNamedValueCollection()
		{
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00006FE1 File Offset: 0x00005FE1
		protected ManagementNamedValueCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00006FEC File Offset: 0x00005FEC
		internal IWbemContext GetContext()
		{
			IWbemContext wbemContext = null;
			if (0 < this.Count)
			{
				try
				{
					wbemContext = (IWbemContext)new WbemContext();
					foreach (object obj in this)
					{
						string text = (string)obj;
						object obj2 = base.BaseGet(text);
						int num = wbemContext.SetValue_(text, 0, ref obj2);
						if (((long)num & (long)((ulong)(-2147483648))) != 0L)
						{
							break;
						}
					}
				}
				catch
				{
				}
			}
			return wbemContext;
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0000708C File Offset: 0x0000608C
		public void Add(string name, object value)
		{
			try
			{
				base.BaseRemove(name);
			}
			catch
			{
			}
			base.BaseAdd(name, value);
			this.FireIdentifierChanged();
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x000070C4 File Offset: 0x000060C4
		public void Remove(string name)
		{
			base.BaseRemove(name);
			this.FireIdentifierChanged();
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x000070D3 File Offset: 0x000060D3
		public void RemoveAll()
		{
			base.BaseClear();
			this.FireIdentifierChanged();
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000070E4 File Offset: 0x000060E4
		public ManagementNamedValueCollection Clone()
		{
			ManagementNamedValueCollection managementNamedValueCollection = new ManagementNamedValueCollection();
			foreach (object obj in this)
			{
				string text = (string)obj;
				object obj2 = base.BaseGet(text);
				if (obj2 != null)
				{
					Type type = obj2.GetType();
					if (type.IsByRef)
					{
						try
						{
							object obj3 = ((ICloneable)obj2).Clone();
							managementNamedValueCollection.Add(text, obj3);
							continue;
						}
						catch
						{
							throw new NotSupportedException();
						}
					}
					managementNamedValueCollection.Add(text, obj2);
				}
				else
				{
					managementNamedValueCollection.Add(text, null);
				}
			}
			return managementNamedValueCollection;
		}

		// Token: 0x17000028 RID: 40
		public object this[string name]
		{
			get
			{
				return base.BaseGet(name);
			}
		}
	}
}
