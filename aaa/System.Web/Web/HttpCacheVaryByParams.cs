using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000056 RID: 86
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpCacheVaryByParams
	{
		// Token: 0x0600032B RID: 811 RVA: 0x0000F216 File Offset: 0x0000E216
		internal HttpCacheVaryByParams()
		{
			this.Reset();
		}

		// Token: 0x0600032C RID: 812 RVA: 0x0000F224 File Offset: 0x0000E224
		internal void Reset()
		{
			this._isModified = false;
			this._paramsStar = false;
			this._parameters = null;
			this._ignoreParams = -1;
		}

		// Token: 0x0600032D RID: 813 RVA: 0x0000F244 File Offset: 0x0000E244
		internal void ResetFromParams(string[] parameters)
		{
			this.Reset();
			if (parameters != null)
			{
				this._isModified = true;
				if (parameters[0].Length == 0)
				{
					this.IgnoreParams = true;
					return;
				}
				if (parameters[0].Equals("*"))
				{
					this._paramsStar = true;
					return;
				}
				this._parameters = new HttpDictionary();
				int i = 0;
				int num = parameters.Length;
				while (i < num)
				{
					this._parameters.SetValue(parameters[i], parameters[i]);
					i++;
				}
			}
		}

		// Token: 0x0600032E RID: 814 RVA: 0x0000F2B6 File Offset: 0x0000E2B6
		internal bool IsModified()
		{
			return this._isModified;
		}

		// Token: 0x0600032F RID: 815 RVA: 0x0000F2BE File Offset: 0x0000E2BE
		internal bool AcceptsParams()
		{
			return this._ignoreParams == 1 || this._paramsStar || this._parameters != null;
		}

		// Token: 0x06000330 RID: 816 RVA: 0x0000F2E0 File Offset: 0x0000E2E0
		internal string[] GetParams()
		{
			string[] array = null;
			if (this._ignoreParams == 1)
			{
				array = new string[] { string.Empty };
			}
			else if (this._paramsStar)
			{
				array = new string[] { "*" };
			}
			else if (this._parameters != null)
			{
				int size = this._parameters.Size;
				int num = 0;
				for (int i = 0; i < size; i++)
				{
					object obj = this._parameters.GetValue(i);
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
						object obj = this._parameters.GetValue(i);
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

		// Token: 0x17000134 RID: 308
		public bool this[string header]
		{
			get
			{
				if (header == null)
				{
					throw new ArgumentNullException("header");
				}
				if (header.Length == 0)
				{
					return this._ignoreParams == 1;
				}
				return this._paramsStar || (this._parameters != null && this._parameters.GetValue(header) != null);
			}
			set
			{
				if (header == null)
				{
					throw new ArgumentNullException("header");
				}
				if (header.Length == 0)
				{
					this.IgnoreParams = value;
					return;
				}
				if (value)
				{
					this._isModified = true;
					this._ignoreParams = 0;
					if (header.Equals("*"))
					{
						this._paramsStar = true;
						this._parameters = null;
						return;
					}
					if (!this._paramsStar)
					{
						if (this._parameters == null)
						{
							this._parameters = new HttpDictionary();
						}
						this._parameters.SetValue(header, header);
					}
				}
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000333 RID: 819 RVA: 0x0000F478 File Offset: 0x0000E478
		// (set) Token: 0x06000334 RID: 820 RVA: 0x0000F483 File Offset: 0x0000E483
		public bool IgnoreParams
		{
			get
			{
				return this._ignoreParams == 1;
			}
			set
			{
				if (this._paramsStar || this._parameters != null)
				{
					return;
				}
				if (this._ignoreParams == -1 || this._ignoreParams == 1)
				{
					this._ignoreParams = (value ? 1 : 0);
					this._isModified = true;
				}
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x06000335 RID: 821 RVA: 0x0000F4BC File Offset: 0x0000E4BC
		internal bool IsVaryByStar
		{
			get
			{
				return this._paramsStar;
			}
		}

		// Token: 0x04000F53 RID: 3923
		private HttpDictionary _parameters;

		// Token: 0x04000F54 RID: 3924
		private int _ignoreParams;

		// Token: 0x04000F55 RID: 3925
		private bool _isModified;

		// Token: 0x04000F56 RID: 3926
		private bool _paramsStar;
	}
}
