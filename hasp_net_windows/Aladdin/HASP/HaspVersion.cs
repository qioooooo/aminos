using System;

namespace Aladdin.HASP
{
	[Serializable]
	public struct HaspVersion
	{
		public int MajorVersion
		{
			get
			{
				return this.majorVersion;
			}
		}

		public int MinorVersion
		{
			get
			{
				return this.minorVersion;
			}
		}

		public int ServerBuild
		{
			get
			{
				return this.serverBuild;
			}
		}

		public int BuildNumber
		{
			get
			{
				return this.buildNumber;
			}
		}

		public HaspVersion(int majorVersion, int minorVersion, int serverBuild, int buildNumber)
		{
			this.majorVersion = majorVersion;
			this.minorVersion = minorVersion;
			this.serverBuild = serverBuild;
			this.buildNumber = buildNumber;
		}

		public override bool Equals(object obj)
		{
			bool flag;
			if (obj == null)
			{
				flag = false;
			}
			else if (obj.GetType() != typeof(HaspVersion))
			{
				flag = false;
			}
			else
			{
				HaspVersion haspVersion = (HaspVersion)obj;
				flag = haspVersion.majorVersion == this.majorVersion && haspVersion.minorVersion == this.minorVersion && haspVersion.serverBuild == this.serverBuild && haspVersion.buildNumber == this.buildNumber;
			}
			return flag;
		}

		public override int GetHashCode()
		{
			return this.majorVersion ^ this.minorVersion ^ this.serverBuild ^ this.buildNumber;
		}

		public static bool operator ==(HaspVersion left, HaspVersion right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(HaspVersion left, HaspVersion right)
		{
			return !left.Equals(right);
		}

		private int majorVersion;

		private int minorVersion;

		private int serverBuild;

		private int buildNumber;
	}
}
