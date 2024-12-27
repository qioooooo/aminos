using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000061 RID: 97
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpCacheVaryByContentEncodings
	{
		// Token: 0x060003A8 RID: 936 RVA: 0x00011136 File Offset: 0x00010136
		internal HttpCacheVaryByContentEncodings()
		{
			this.Reset();
		}

		// Token: 0x060003A9 RID: 937 RVA: 0x00011144 File Offset: 0x00010144
		internal void Reset()
		{
			this._isModified = false;
			this._contentEncodings = null;
		}

		// Token: 0x060003AA RID: 938 RVA: 0x00011154 File Offset: 0x00010154
		internal void ResetFromContentEncodings(string[] contentEncodings)
		{
			this.Reset();
			if (contentEncodings != null)
			{
				this._isModified = true;
				this._contentEncodings = new string[contentEncodings.Length];
				for (int i = 0; i < contentEncodings.Length; i++)
				{
					this._contentEncodings[i] = contentEncodings[i];
				}
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00011198 File Offset: 0x00010198
		internal bool IsCacheableEncoding(string coding)
		{
			if (this._contentEncodings == null)
			{
				return true;
			}
			if (coding == null)
			{
				return true;
			}
			for (int i = 0; i < this._contentEncodings.Length; i++)
			{
				if (this._contentEncodings[i] == coding)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x000111DA File Offset: 0x000101DA
		internal bool IsModified()
		{
			return this._isModified;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x000111E2 File Offset: 0x000101E2
		internal string[] GetContentEncodings()
		{
			return this._contentEncodings;
		}

		// Token: 0x1700016B RID: 363
		public bool this[string contentEncoding]
		{
			get
			{
				if (string.IsNullOrEmpty(contentEncoding))
				{
					throw new ArgumentNullException(SR.GetString("Parameter_NullOrEmpty", new object[] { "contentEncoding" }));
				}
				if (this._contentEncodings == null)
				{
					return false;
				}
				for (int i = 0; i < this._contentEncodings.Length; i++)
				{
					if (this._contentEncodings[i] == contentEncoding)
					{
						return true;
					}
				}
				return false;
			}
			set
			{
				if (string.IsNullOrEmpty(contentEncoding))
				{
					throw new ArgumentNullException(SR.GetString("Parameter_NullOrEmpty", new object[] { "contentEncoding" }));
				}
				if (!value)
				{
					return;
				}
				this._isModified = true;
				if (this._contentEncodings != null)
				{
					string[] array = new string[this._contentEncodings.Length + 1];
					for (int i = 0; i < this._contentEncodings.Length; i++)
					{
						array[i] = this._contentEncodings[i];
					}
					array[array.Length - 1] = contentEncoding;
					this._contentEncodings = array;
					return;
				}
				this._contentEncodings = new string[1];
				this._contentEncodings[0] = contentEncoding;
			}
		}

		// Token: 0x04000FCB RID: 4043
		private string[] _contentEncodings;

		// Token: 0x04000FCC RID: 4044
		private bool _isModified;
	}
}
