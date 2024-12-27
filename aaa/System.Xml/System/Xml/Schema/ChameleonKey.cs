using System;

namespace System.Xml.Schema
{
	// Token: 0x02000186 RID: 390
	internal class ChameleonKey
	{
		// Token: 0x060014B6 RID: 5302 RVA: 0x0005872B File Offset: 0x0005772B
		public ChameleonKey(string ns, Uri location)
		{
			this.targetNS = ns;
			this.chameleonLocation = location;
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x00058741 File Offset: 0x00057741
		public override int GetHashCode()
		{
			if (this.hashCode == 0)
			{
				this.hashCode = this.targetNS.GetHashCode() + this.chameleonLocation.GetHashCode();
			}
			return this.hashCode;
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x00058770 File Offset: 0x00057770
		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			ChameleonKey chameleonKey = obj as ChameleonKey;
			return chameleonKey != null && this.targetNS.Equals(chameleonKey.targetNS) && this.chameleonLocation.Equals(chameleonKey.chameleonLocation);
		}

		// Token: 0x04000C83 RID: 3203
		internal string targetNS;

		// Token: 0x04000C84 RID: 3204
		internal Uri chameleonLocation;

		// Token: 0x04000C85 RID: 3205
		private int hashCode;
	}
}
