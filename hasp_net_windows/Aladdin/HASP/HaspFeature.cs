using System;

namespace Aladdin.HASP
{
	[Serializable]
	public struct HaspFeature : IComparable<HaspFeature>
	{
		public HaspFeature(int feature)
		{
			this.feature = feature;
		}

		public HaspFeature(FeatureType feature)
		{
			this.feature = (int)feature;
		}

		public static HaspFeature Default
		{
			get
			{
				return new HaspFeature(FeatureType.Default);
			}
		}

		public static HaspFeature ProgNumDefault
		{
			get
			{
				return new HaspFeature(FeatureType.ProgNumDefault);
			}
		}

		public int Feature
		{
			get
			{
				return this.feature;
			}
		}

		public int FeatureId
		{
			get
			{
				return this.IsProgNum ? (this.feature & -65281) : this.feature;
			}
		}

		public bool IsDefault
		{
			get
			{
				return this.FeatureId == (this.IsProgNum ? (-65536) : 0);
			}
		}

		public bool IsProgNum
		{
			get
			{
				return (this.feature & -65536) == -65536;
			}
		}

		public FeatureOptions Options
		{
			get
			{
				return (FeatureOptions)(this.IsProgNum ? (this.feature & 65280) : 0);
			}
		}

		public int CompareTo(HaspFeature other)
		{
			return this.feature.CompareTo(other.feature);
		}

		public static bool operator ==(HaspFeature left, HaspFeature right)
		{
			return left.CompareTo(right) == 0;
		}

		public static bool operator !=(HaspFeature left, HaspFeature right)
		{
			return !(left == right);
		}

		public override bool Equals(object obj)
		{
			return this == (HaspFeature)obj;
		}

		public override int GetHashCode()
		{
			return this.feature;
		}

		public static HaspFeature FromFeature(int feature)
		{
			return new HaspFeature(feature & 1048575);
		}

		public static HaspFeature FromProgNum(int number)
		{
			number &= 255;
			return new HaspFeature(number | -65536);
		}

		public bool HasOption(FeatureOptions option)
		{
			bool flag;
			if (this.IsProgNum)
			{
				flag = option == FeatureOptions.Default;
			}
			else
			{
				int num = this.feature & 65280;
				if (option == FeatureOptions.Default)
				{
					flag = num == 0;
				}
				else
				{
					flag = option == FeatureOptions.Default;
				}
			}
			return flag;
		}

		public bool SetOptions(FeatureOptions add, FeatureOptions remove)
		{
			bool flag;
			if (!this.IsProgNum)
			{
				flag = false;
			}
			else
			{
				if ((add & FeatureOptions.NotRemote) == FeatureOptions.NotRemote)
				{
					add &= ~FeatureOptions.NotLocal;
					remove |= FeatureOptions.NotLocal;
				}
				if ((add & FeatureOptions.NotLocal) == FeatureOptions.NotLocal)
				{
					add &= ~FeatureOptions.NotRemote;
					remove |= FeatureOptions.NotRemote;
				}
				this.feature |= (int)(add & (FeatureOptions)65280);
				this.feature &= (int)(~(int)(remove & (FeatureOptions)65280));
				flag = true;
			}
			return flag;
		}

		public override string ToString()
		{
			return this.feature.ToString();
		}

		private int feature;
	}
}
