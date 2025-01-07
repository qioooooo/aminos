using System;

namespace System.Xml.Schema
{
	internal class ChameleonKey
	{
		public ChameleonKey(string ns, Uri location)
		{
			this.targetNS = ns;
			this.chameleonLocation = location;
		}

		public override int GetHashCode()
		{
			if (this.hashCode == 0)
			{
				this.hashCode = this.targetNS.GetHashCode() + this.chameleonLocation.GetHashCode();
			}
			return this.hashCode;
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			ChameleonKey chameleonKey = obj as ChameleonKey;
			return chameleonKey != null && this.targetNS.Equals(chameleonKey.targetNS) && this.chameleonLocation.Equals(chameleonKey.chameleonLocation);
		}

		internal string targetNS;

		internal Uri chameleonLocation;

		private int hashCode;
	}
}
