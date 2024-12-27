using System;
using System.Collections.Specialized;

namespace System.Net
{
	// Token: 0x020006DE RID: 1758
	internal class TrackingStringDictionary : StringDictionary
	{
		// Token: 0x06003637 RID: 13879 RVA: 0x000E78C5 File Offset: 0x000E68C5
		internal TrackingStringDictionary()
			: this(false)
		{
		}

		// Token: 0x06003638 RID: 13880 RVA: 0x000E78CE File Offset: 0x000E68CE
		internal TrackingStringDictionary(bool isReadOnly)
		{
			this.isReadOnly = isReadOnly;
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06003639 RID: 13881 RVA: 0x000E78DD File Offset: 0x000E68DD
		// (set) Token: 0x0600363A RID: 13882 RVA: 0x000E78E5 File Offset: 0x000E68E5
		internal bool IsChanged
		{
			get
			{
				return this.isChanged;
			}
			set
			{
				this.isChanged = value;
			}
		}

		// Token: 0x0600363B RID: 13883 RVA: 0x000E78EE File Offset: 0x000E68EE
		public override void Add(string key, string value)
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
			}
			base.Add(key, value);
			this.isChanged = true;
		}

		// Token: 0x0600363C RID: 13884 RVA: 0x000E7917 File Offset: 0x000E6917
		public override void Clear()
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
			}
			base.Clear();
			this.isChanged = true;
		}

		// Token: 0x0600363D RID: 13885 RVA: 0x000E793E File Offset: 0x000E693E
		public override void Remove(string key)
		{
			if (this.isReadOnly)
			{
				throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
			}
			base.Remove(key);
			this.isChanged = true;
		}

		// Token: 0x17000C8A RID: 3210
		public override string this[string key]
		{
			get
			{
				return base[key];
			}
			set
			{
				if (this.isReadOnly)
				{
					throw new InvalidOperationException(SR.GetString("MailCollectionIsReadOnly"));
				}
				base[key] = value;
				this.isChanged = true;
			}
		}

		// Token: 0x04003169 RID: 12649
		private bool isChanged;

		// Token: 0x0400316A RID: 12650
		private bool isReadOnly;
	}
}
