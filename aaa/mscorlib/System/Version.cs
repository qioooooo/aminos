using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace System
{
	// Token: 0x02000125 RID: 293
	[ComVisible(true)]
	[Serializable]
	public sealed class Version : ICloneable, IComparable, IComparable<Version>, IEquatable<Version>
	{
		// Token: 0x0600111C RID: 4380 RVA: 0x00030754 File Offset: 0x0002F754
		public Version(int major, int minor, int build, int revision)
		{
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (build < 0)
			{
				throw new ArgumentOutOfRangeException("build", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (revision < 0)
			{
				throw new ArgumentOutOfRangeException("revision", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			this._Major = major;
			this._Minor = minor;
			this._Build = build;
			this._Revision = revision;
		}

		// Token: 0x0600111D RID: 4381 RVA: 0x000307F8 File Offset: 0x0002F7F8
		public Version(int major, int minor, int build)
		{
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (build < 0)
			{
				throw new ArgumentOutOfRangeException("build", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			this._Major = major;
			this._Minor = minor;
			this._Build = build;
		}

		// Token: 0x0600111E RID: 4382 RVA: 0x0003087C File Offset: 0x0002F87C
		public Version(int major, int minor)
		{
			if (major < 0)
			{
				throw new ArgumentOutOfRangeException("major", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			if (minor < 0)
			{
				throw new ArgumentOutOfRangeException("minor", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			this._Major = major;
			this._Minor = minor;
		}

		// Token: 0x0600111F RID: 4383 RVA: 0x000308E0 File Offset: 0x0002F8E0
		public Version(string version)
		{
			if (version == null)
			{
				throw new ArgumentNullException("version");
			}
			string[] array = version.Split(new char[] { '.' });
			int num = array.Length;
			if (num < 2 || num > 4)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_VersionString"));
			}
			this._Major = int.Parse(array[0], CultureInfo.InvariantCulture);
			if (this._Major < 0)
			{
				throw new ArgumentOutOfRangeException("version", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			this._Minor = int.Parse(array[1], CultureInfo.InvariantCulture);
			if (this._Minor < 0)
			{
				throw new ArgumentOutOfRangeException("version", Environment.GetResourceString("ArgumentOutOfRange_Version"));
			}
			num -= 2;
			if (num > 0)
			{
				this._Build = int.Parse(array[2], CultureInfo.InvariantCulture);
				if (this._Build < 0)
				{
					throw new ArgumentOutOfRangeException("build", Environment.GetResourceString("ArgumentOutOfRange_Version"));
				}
				num--;
				if (num > 0)
				{
					this._Revision = int.Parse(array[3], CultureInfo.InvariantCulture);
					if (this._Revision < 0)
					{
						throw new ArgumentOutOfRangeException("revision", Environment.GetResourceString("ArgumentOutOfRange_Version"));
					}
				}
			}
		}

		// Token: 0x06001120 RID: 4384 RVA: 0x00030A13 File Offset: 0x0002FA13
		public Version()
		{
			this._Major = 0;
			this._Minor = 0;
		}

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06001121 RID: 4385 RVA: 0x00030A37 File Offset: 0x0002FA37
		public int Major
		{
			get
			{
				return this._Major;
			}
		}

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06001122 RID: 4386 RVA: 0x00030A3F File Offset: 0x0002FA3F
		public int Minor
		{
			get
			{
				return this._Minor;
			}
		}

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06001123 RID: 4387 RVA: 0x00030A47 File Offset: 0x0002FA47
		public int Build
		{
			get
			{
				return this._Build;
			}
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06001124 RID: 4388 RVA: 0x00030A4F File Offset: 0x0002FA4F
		public int Revision
		{
			get
			{
				return this._Revision;
			}
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06001125 RID: 4389 RVA: 0x00030A57 File Offset: 0x0002FA57
		public short MajorRevision
		{
			get
			{
				return (short)(this._Revision >> 16);
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x06001126 RID: 4390 RVA: 0x00030A63 File Offset: 0x0002FA63
		public short MinorRevision
		{
			get
			{
				return (short)(this._Revision & 65535);
			}
		}

		// Token: 0x06001127 RID: 4391 RVA: 0x00030A74 File Offset: 0x0002FA74
		public object Clone()
		{
			return new Version
			{
				_Major = this._Major,
				_Minor = this._Minor,
				_Build = this._Build,
				_Revision = this._Revision
			};
		}

		// Token: 0x06001128 RID: 4392 RVA: 0x00030AB8 File Offset: 0x0002FAB8
		public int CompareTo(object version)
		{
			if (version == null)
			{
				return 1;
			}
			Version version2 = version as Version;
			if (version2 == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_MustBeVersion"));
			}
			if (this._Major != version2._Major)
			{
				if (this._Major > version2._Major)
				{
					return 1;
				}
				return -1;
			}
			else if (this._Minor != version2._Minor)
			{
				if (this._Minor > version2._Minor)
				{
					return 1;
				}
				return -1;
			}
			else if (this._Build != version2._Build)
			{
				if (this._Build > version2._Build)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (this._Revision == version2._Revision)
				{
					return 0;
				}
				if (this._Revision > version2._Revision)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x06001129 RID: 4393 RVA: 0x00030B6C File Offset: 0x0002FB6C
		public int CompareTo(Version value)
		{
			if (value == null)
			{
				return 1;
			}
			if (this._Major != value._Major)
			{
				if (this._Major > value._Major)
				{
					return 1;
				}
				return -1;
			}
			else if (this._Minor != value._Minor)
			{
				if (this._Minor > value._Minor)
				{
					return 1;
				}
				return -1;
			}
			else if (this._Build != value._Build)
			{
				if (this._Build > value._Build)
				{
					return 1;
				}
				return -1;
			}
			else
			{
				if (this._Revision == value._Revision)
				{
					return 0;
				}
				if (this._Revision > value._Revision)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x0600112A RID: 4394 RVA: 0x00030C08 File Offset: 0x0002FC08
		public override bool Equals(object obj)
		{
			Version version = obj as Version;
			return !(version == null) && this._Major == version._Major && this._Minor == version._Minor && this._Build == version._Build && this._Revision == version._Revision;
		}

		// Token: 0x0600112B RID: 4395 RVA: 0x00030C64 File Offset: 0x0002FC64
		public bool Equals(Version obj)
		{
			return !(obj == null) && this._Major == obj._Major && this._Minor == obj._Minor && this._Build == obj._Build && this._Revision == obj._Revision;
		}

		// Token: 0x0600112C RID: 4396 RVA: 0x00030CB8 File Offset: 0x0002FCB8
		public override int GetHashCode()
		{
			int num = 0;
			num |= (this._Major & 15) << 28;
			num |= (this._Minor & 255) << 20;
			num |= (this._Build & 255) << 12;
			return num | (this._Revision & 4095);
		}

		// Token: 0x0600112D RID: 4397 RVA: 0x00030D0A File Offset: 0x0002FD0A
		public override string ToString()
		{
			if (this._Build == -1)
			{
				return this.ToString(2);
			}
			if (this._Revision == -1)
			{
				return this.ToString(3);
			}
			return this.ToString(4);
		}

		// Token: 0x0600112E RID: 4398 RVA: 0x00030D38 File Offset: 0x0002FD38
		public string ToString(int fieldCount)
		{
			switch (fieldCount)
			{
			case 0:
				return string.Empty;
			case 1:
				return string.Concat(this._Major);
			case 2:
				return this._Major + "." + this._Minor;
			default:
				if (this._Build == -1)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[] { "0", "2" }), "fieldCount");
				}
				if (fieldCount == 3)
				{
					return string.Concat(new object[] { this._Major, ".", this._Minor, ".", this._Build });
				}
				if (this._Revision == -1)
				{
					throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[] { "0", "3" }), "fieldCount");
				}
				if (fieldCount == 4)
				{
					return string.Concat(new object[] { this.Major, ".", this._Minor, ".", this._Build, ".", this._Revision });
				}
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("ArgumentOutOfRange_Bounds_Lower_Upper"), new object[] { "0", "4" }), "fieldCount");
			}
		}

		// Token: 0x0600112F RID: 4399 RVA: 0x00030F04 File Offset: 0x0002FF04
		public static bool operator ==(Version v1, Version v2)
		{
			if (object.ReferenceEquals(v1, null))
			{
				return object.ReferenceEquals(v2, null);
			}
			return v1.Equals(v2);
		}

		// Token: 0x06001130 RID: 4400 RVA: 0x00030F1E File Offset: 0x0002FF1E
		public static bool operator !=(Version v1, Version v2)
		{
			return !(v1 == v2);
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x00030F2A File Offset: 0x0002FF2A
		public static bool operator <(Version v1, Version v2)
		{
			if (v1 == null)
			{
				throw new ArgumentNullException("v1");
			}
			return v1.CompareTo(v2) < 0;
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00030F44 File Offset: 0x0002FF44
		public static bool operator <=(Version v1, Version v2)
		{
			if (v1 == null)
			{
				throw new ArgumentNullException("v1");
			}
			return v1.CompareTo(v2) <= 0;
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x00030F61 File Offset: 0x0002FF61
		public static bool operator >(Version v1, Version v2)
		{
			return v2 < v1;
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x00030F6A File Offset: 0x0002FF6A
		public static bool operator >=(Version v1, Version v2)
		{
			return v2 <= v1;
		}

		// Token: 0x040005BB RID: 1467
		private int _Major;

		// Token: 0x040005BC RID: 1468
		private int _Minor;

		// Token: 0x040005BD RID: 1469
		private int _Build = -1;

		// Token: 0x040005BE RID: 1470
		private int _Revision = -1;
	}
}
