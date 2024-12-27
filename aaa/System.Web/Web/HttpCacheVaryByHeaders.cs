using System;
using System.Security.Permissions;
using System.Text;

namespace System.Web
{
	// Token: 0x02000060 RID: 96
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpCacheVaryByHeaders
	{
		// Token: 0x06000396 RID: 918 RVA: 0x00010E2C File Offset: 0x0000FE2C
		internal HttpCacheVaryByHeaders()
		{
			this.Reset();
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00010E3A File Offset: 0x0000FE3A
		internal void Reset()
		{
			this._isModified = false;
			this._varyStar = false;
			this._headers = null;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00010E54 File Offset: 0x0000FE54
		internal void ResetFromHeaders(string[] headers)
		{
			if (headers == null)
			{
				this._isModified = false;
				this._varyStar = false;
				this._headers = null;
				return;
			}
			this._isModified = true;
			if (headers[0].Equals("*"))
			{
				this._varyStar = true;
				this._headers = null;
				return;
			}
			this._varyStar = false;
			this._headers = new HttpDictionary();
			int i = 0;
			int num = headers.Length;
			while (i < num)
			{
				this._headers.SetValue(headers[i], headers[i]);
				i++;
			}
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00010ED2 File Offset: 0x0000FED2
		internal bool IsModified()
		{
			return this._isModified;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00010EDC File Offset: 0x0000FEDC
		internal string ToHeaderString()
		{
			if (this._varyStar)
			{
				return "*";
			}
			if (this._headers != null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				int i = 0;
				int size = this._headers.Size;
				while (i < size)
				{
					object value = this._headers.GetValue(i);
					if (value != null)
					{
						HttpCachePolicy.AppendValueToHeader(stringBuilder, (string)value);
					}
					i++;
				}
				if (stringBuilder.Length > 0)
				{
					return stringBuilder.ToString();
				}
			}
			return null;
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00010F4C File Offset: 0x0000FF4C
		internal string[] GetHeaders()
		{
			string[] array = null;
			if (this._varyStar)
			{
				return new string[] { "*" };
			}
			if (this._headers != null)
			{
				int size = this._headers.Size;
				int num = 0;
				for (int i = 0; i < size; i++)
				{
					object obj = this._headers.GetValue(i);
					if (obj != null)
					{
						num++;
					}
				}
				if (num > 0)
				{
					array = new string[num];
					int num2 = 0;
					for (int i = 0; i < size; i++)
					{
						object obj = this._headers.GetValue(i);
						if (obj != null)
						{
							array[num2] = (string)obj;
							num2++;
						}
					}
				}
			}
			return array;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00010FEC File Offset: 0x0000FFEC
		public void VaryByUnspecifiedParameters()
		{
			this._isModified = true;
			this._varyStar = true;
			this._headers = null;
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00011003 File Offset: 0x00010003
		internal bool GetVaryByUnspecifiedParameters()
		{
			return this._varyStar;
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x0600039E RID: 926 RVA: 0x0001100B File Offset: 0x0001000B
		// (set) Token: 0x0600039F RID: 927 RVA: 0x00011018 File Offset: 0x00010018
		public bool AcceptTypes
		{
			get
			{
				return this["Accept"];
			}
			set
			{
				this._isModified = true;
				this["Accept"] = value;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x060003A0 RID: 928 RVA: 0x0001102D File Offset: 0x0001002D
		// (set) Token: 0x060003A1 RID: 929 RVA: 0x0001103A File Offset: 0x0001003A
		public bool UserLanguage
		{
			get
			{
				return this["Accept-Language"];
			}
			set
			{
				this._isModified = true;
				this["Accept-Language"] = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x060003A2 RID: 930 RVA: 0x0001104F File Offset: 0x0001004F
		// (set) Token: 0x060003A3 RID: 931 RVA: 0x0001105C File Offset: 0x0001005C
		public bool UserAgent
		{
			get
			{
				return this["User-Agent"];
			}
			set
			{
				this._isModified = true;
				this["User-Agent"] = value;
			}
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x060003A4 RID: 932 RVA: 0x00011071 File Offset: 0x00010071
		// (set) Token: 0x060003A5 RID: 933 RVA: 0x0001107E File Offset: 0x0001007E
		public bool UserCharSet
		{
			get
			{
				return this["Accept-Charset"];
			}
			set
			{
				this._isModified = true;
				this["Accept-Charset"] = value;
			}
		}

		// Token: 0x1700016A RID: 362
		public bool this[string header]
		{
			get
			{
				if (header == null)
				{
					throw new ArgumentNullException("header");
				}
				if (header.Equals("*"))
				{
					return this._varyStar;
				}
				return this._headers != null && this._headers.GetValue(header) != null;
			}
			set
			{
				if (header == null)
				{
					throw new ArgumentNullException("header");
				}
				if (!value)
				{
					return;
				}
				this._isModified = true;
				if (header.Equals("*"))
				{
					this.VaryByUnspecifiedParameters();
					return;
				}
				if (!this._varyStar)
				{
					if (this._headers == null)
					{
						this._headers = new HttpDictionary();
					}
					this._headers.SetValue(header, header);
				}
			}
		}

		// Token: 0x04000FC8 RID: 4040
		private bool _isModified;

		// Token: 0x04000FC9 RID: 4041
		private bool _varyStar;

		// Token: 0x04000FCA RID: 4042
		private HttpDictionary _headers;
	}
}
