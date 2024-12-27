using System;
using System.Collections.ObjectModel;
using System.Net.Mime;
using System.Text;

namespace System.Net.Mail
{
	// Token: 0x0200069A RID: 1690
	public class MailAddressCollection : Collection<MailAddress>
	{
		// Token: 0x06003426 RID: 13350 RVA: 0x000DBEC8 File Offset: 0x000DAEC8
		public void Add(string addresses)
		{
			if (addresses == null)
			{
				throw new ArgumentNullException("addresses");
			}
			if (addresses == string.Empty)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "addresses" }), "addresses");
			}
			this.ParseValue(addresses);
		}

		// Token: 0x06003427 RID: 13351 RVA: 0x000DBF1C File Offset: 0x000DAF1C
		protected override void SetItem(int index, MailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.SetItem(index, item);
		}

		// Token: 0x06003428 RID: 13352 RVA: 0x000DBF34 File Offset: 0x000DAF34
		protected override void InsertItem(int index, MailAddress item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			base.InsertItem(index, item);
		}

		// Token: 0x06003429 RID: 13353 RVA: 0x000DBF4C File Offset: 0x000DAF4C
		internal void ParseValue(string addresses)
		{
			for (int i = 0; i < addresses.Length; i++)
			{
				MailAddress mailAddress = MailBnfHelper.ReadMailAddress(addresses, ref i);
				if (mailAddress == null)
				{
					return;
				}
				base.Add(mailAddress);
				if (!MailBnfHelper.SkipCFWS(addresses, ref i))
				{
					break;
				}
				if (addresses[i] != ',')
				{
					return;
				}
			}
		}

		// Token: 0x0600342A RID: 13354 RVA: 0x000DBF94 File Offset: 0x000DAF94
		internal string ToEncodedString()
		{
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MailAddress mailAddress in this)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(mailAddress.ToEncodedString());
				flag = false;
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600342B RID: 13355 RVA: 0x000DC004 File Offset: 0x000DB004
		public override string ToString()
		{
			bool flag = true;
			StringBuilder stringBuilder = new StringBuilder();
			foreach (MailAddress mailAddress in this)
			{
				if (!flag)
				{
					stringBuilder.Append(", ");
				}
				stringBuilder.Append(mailAddress.ToString());
				flag = false;
			}
			return stringBuilder.ToString();
		}
	}
}
