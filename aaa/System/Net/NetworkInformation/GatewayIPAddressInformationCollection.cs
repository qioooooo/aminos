using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Net.NetworkInformation
{
	// Token: 0x020005EA RID: 1514
	public class GatewayIPAddressInformationCollection : ICollection<GatewayIPAddressInformation>, IEnumerable<GatewayIPAddressInformation>, IEnumerable
	{
		// Token: 0x06002FB5 RID: 12213 RVA: 0x000CF0BF File Offset: 0x000CE0BF
		protected internal GatewayIPAddressInformationCollection()
		{
		}

		// Token: 0x06002FB6 RID: 12214 RVA: 0x000CF0D2 File Offset: 0x000CE0D2
		public virtual void CopyTo(GatewayIPAddressInformation[] array, int offset)
		{
			this.addresses.CopyTo(array, offset);
		}

		// Token: 0x17000A60 RID: 2656
		// (get) Token: 0x06002FB7 RID: 12215 RVA: 0x000CF0E1 File Offset: 0x000CE0E1
		public virtual int Count
		{
			get
			{
				return this.addresses.Count;
			}
		}

		// Token: 0x17000A61 RID: 2657
		// (get) Token: 0x06002FB8 RID: 12216 RVA: 0x000CF0EE File Offset: 0x000CE0EE
		public virtual bool IsReadOnly
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000A62 RID: 2658
		public virtual GatewayIPAddressInformation this[int index]
		{
			get
			{
				return this.addresses[index];
			}
		}

		// Token: 0x06002FBA RID: 12218 RVA: 0x000CF0FF File Offset: 0x000CE0FF
		public virtual void Add(GatewayIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002FBB RID: 12219 RVA: 0x000CF110 File Offset: 0x000CE110
		internal void InternalAdd(GatewayIPAddressInformation address)
		{
			this.addresses.Add(address);
		}

		// Token: 0x06002FBC RID: 12220 RVA: 0x000CF11E File Offset: 0x000CE11E
		public virtual bool Contains(GatewayIPAddressInformation address)
		{
			return this.addresses.Contains(address);
		}

		// Token: 0x06002FBD RID: 12221 RVA: 0x000CF12C File Offset: 0x000CE12C
		public virtual IEnumerator<GatewayIPAddressInformation> GetEnumerator()
		{
			return this.addresses.GetEnumerator();
		}

		// Token: 0x06002FBE RID: 12222 RVA: 0x000CF139 File Offset: 0x000CE139
		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}

		// Token: 0x06002FBF RID: 12223 RVA: 0x000CF13C File Offset: 0x000CE13C
		public virtual bool Remove(GatewayIPAddressInformation address)
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x06002FC0 RID: 12224 RVA: 0x000CF14D File Offset: 0x000CE14D
		public virtual void Clear()
		{
			throw new NotSupportedException(SR.GetString("net_collection_readonly"));
		}

		// Token: 0x04002CBA RID: 11450
		private Collection<GatewayIPAddressInformation> addresses = new Collection<GatewayIPAddressInformation>();
	}
}
