using System;
using System.Runtime.InteropServices;

namespace System.Drawing.Imaging
{
	// Token: 0x02000076 RID: 118
	[StructLayout(LayoutKind.Sequential)]
	public sealed class ColorMatrix
	{
		// Token: 0x06000746 RID: 1862 RVA: 0x0001BB5B File Offset: 0x0001AB5B
		public ColorMatrix()
		{
			this.matrix00 = 1f;
			this.matrix11 = 1f;
			this.matrix22 = 1f;
			this.matrix33 = 1f;
			this.matrix44 = 1f;
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000747 RID: 1863 RVA: 0x0001BB9A File Offset: 0x0001AB9A
		// (set) Token: 0x06000748 RID: 1864 RVA: 0x0001BBA2 File Offset: 0x0001ABA2
		public float Matrix00
		{
			get
			{
				return this.matrix00;
			}
			set
			{
				this.matrix00 = value;
			}
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0001BBAB File Offset: 0x0001ABAB
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x0001BBB3 File Offset: 0x0001ABB3
		public float Matrix01
		{
			get
			{
				return this.matrix01;
			}
			set
			{
				this.matrix01 = value;
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0001BBBC File Offset: 0x0001ABBC
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0001BBC4 File Offset: 0x0001ABC4
		public float Matrix02
		{
			get
			{
				return this.matrix02;
			}
			set
			{
				this.matrix02 = value;
			}
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0001BBCD File Offset: 0x0001ABCD
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x0001BBD5 File Offset: 0x0001ABD5
		public float Matrix03
		{
			get
			{
				return this.matrix03;
			}
			set
			{
				this.matrix03 = value;
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0001BBDE File Offset: 0x0001ABDE
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x0001BBE6 File Offset: 0x0001ABE6
		public float Matrix04
		{
			get
			{
				return this.matrix04;
			}
			set
			{
				this.matrix04 = value;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x0001BBEF File Offset: 0x0001ABEF
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x0001BBF7 File Offset: 0x0001ABF7
		public float Matrix10
		{
			get
			{
				return this.matrix10;
			}
			set
			{
				this.matrix10 = value;
			}
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0001BC00 File Offset: 0x0001AC00
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x0001BC08 File Offset: 0x0001AC08
		public float Matrix11
		{
			get
			{
				return this.matrix11;
			}
			set
			{
				this.matrix11 = value;
			}
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0001BC11 File Offset: 0x0001AC11
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x0001BC19 File Offset: 0x0001AC19
		public float Matrix12
		{
			get
			{
				return this.matrix12;
			}
			set
			{
				this.matrix12 = value;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000757 RID: 1879 RVA: 0x0001BC22 File Offset: 0x0001AC22
		// (set) Token: 0x06000758 RID: 1880 RVA: 0x0001BC2A File Offset: 0x0001AC2A
		public float Matrix13
		{
			get
			{
				return this.matrix13;
			}
			set
			{
				this.matrix13 = value;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000759 RID: 1881 RVA: 0x0001BC33 File Offset: 0x0001AC33
		// (set) Token: 0x0600075A RID: 1882 RVA: 0x0001BC3B File Offset: 0x0001AC3B
		public float Matrix14
		{
			get
			{
				return this.matrix14;
			}
			set
			{
				this.matrix14 = value;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x0600075B RID: 1883 RVA: 0x0001BC44 File Offset: 0x0001AC44
		// (set) Token: 0x0600075C RID: 1884 RVA: 0x0001BC4C File Offset: 0x0001AC4C
		public float Matrix20
		{
			get
			{
				return this.matrix20;
			}
			set
			{
				this.matrix20 = value;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x0600075D RID: 1885 RVA: 0x0001BC55 File Offset: 0x0001AC55
		// (set) Token: 0x0600075E RID: 1886 RVA: 0x0001BC5D File Offset: 0x0001AC5D
		public float Matrix21
		{
			get
			{
				return this.matrix21;
			}
			set
			{
				this.matrix21 = value;
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600075F RID: 1887 RVA: 0x0001BC66 File Offset: 0x0001AC66
		// (set) Token: 0x06000760 RID: 1888 RVA: 0x0001BC6E File Offset: 0x0001AC6E
		public float Matrix22
		{
			get
			{
				return this.matrix22;
			}
			set
			{
				this.matrix22 = value;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000761 RID: 1889 RVA: 0x0001BC77 File Offset: 0x0001AC77
		// (set) Token: 0x06000762 RID: 1890 RVA: 0x0001BC7F File Offset: 0x0001AC7F
		public float Matrix23
		{
			get
			{
				return this.matrix23;
			}
			set
			{
				this.matrix23 = value;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000763 RID: 1891 RVA: 0x0001BC88 File Offset: 0x0001AC88
		// (set) Token: 0x06000764 RID: 1892 RVA: 0x0001BC90 File Offset: 0x0001AC90
		public float Matrix24
		{
			get
			{
				return this.matrix24;
			}
			set
			{
				this.matrix24 = value;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000765 RID: 1893 RVA: 0x0001BC99 File Offset: 0x0001AC99
		// (set) Token: 0x06000766 RID: 1894 RVA: 0x0001BCA1 File Offset: 0x0001ACA1
		public float Matrix30
		{
			get
			{
				return this.matrix30;
			}
			set
			{
				this.matrix30 = value;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000767 RID: 1895 RVA: 0x0001BCAA File Offset: 0x0001ACAA
		// (set) Token: 0x06000768 RID: 1896 RVA: 0x0001BCB2 File Offset: 0x0001ACB2
		public float Matrix31
		{
			get
			{
				return this.matrix31;
			}
			set
			{
				this.matrix31 = value;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000769 RID: 1897 RVA: 0x0001BCBB File Offset: 0x0001ACBB
		// (set) Token: 0x0600076A RID: 1898 RVA: 0x0001BCC3 File Offset: 0x0001ACC3
		public float Matrix32
		{
			get
			{
				return this.matrix32;
			}
			set
			{
				this.matrix32 = value;
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x0600076B RID: 1899 RVA: 0x0001BCCC File Offset: 0x0001ACCC
		// (set) Token: 0x0600076C RID: 1900 RVA: 0x0001BCD4 File Offset: 0x0001ACD4
		public float Matrix33
		{
			get
			{
				return this.matrix33;
			}
			set
			{
				this.matrix33 = value;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x0600076D RID: 1901 RVA: 0x0001BCDD File Offset: 0x0001ACDD
		// (set) Token: 0x0600076E RID: 1902 RVA: 0x0001BCE5 File Offset: 0x0001ACE5
		public float Matrix34
		{
			get
			{
				return this.matrix34;
			}
			set
			{
				this.matrix34 = value;
			}
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x0600076F RID: 1903 RVA: 0x0001BCEE File Offset: 0x0001ACEE
		// (set) Token: 0x06000770 RID: 1904 RVA: 0x0001BCF6 File Offset: 0x0001ACF6
		public float Matrix40
		{
			get
			{
				return this.matrix40;
			}
			set
			{
				this.matrix40 = value;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000771 RID: 1905 RVA: 0x0001BCFF File Offset: 0x0001ACFF
		// (set) Token: 0x06000772 RID: 1906 RVA: 0x0001BD07 File Offset: 0x0001AD07
		public float Matrix41
		{
			get
			{
				return this.matrix41;
			}
			set
			{
				this.matrix41 = value;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000773 RID: 1907 RVA: 0x0001BD10 File Offset: 0x0001AD10
		// (set) Token: 0x06000774 RID: 1908 RVA: 0x0001BD18 File Offset: 0x0001AD18
		public float Matrix42
		{
			get
			{
				return this.matrix42;
			}
			set
			{
				this.matrix42 = value;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000775 RID: 1909 RVA: 0x0001BD21 File Offset: 0x0001AD21
		// (set) Token: 0x06000776 RID: 1910 RVA: 0x0001BD29 File Offset: 0x0001AD29
		public float Matrix43
		{
			get
			{
				return this.matrix43;
			}
			set
			{
				this.matrix43 = value;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000777 RID: 1911 RVA: 0x0001BD32 File Offset: 0x0001AD32
		// (set) Token: 0x06000778 RID: 1912 RVA: 0x0001BD3A File Offset: 0x0001AD3A
		public float Matrix44
		{
			get
			{
				return this.matrix44;
			}
			set
			{
				this.matrix44 = value;
			}
		}

		// Token: 0x06000779 RID: 1913 RVA: 0x0001BD43 File Offset: 0x0001AD43
		[CLSCompliant(false)]
		public ColorMatrix(float[][] newColorMatrix)
		{
			this.SetMatrix(newColorMatrix);
		}

		// Token: 0x0600077A RID: 1914 RVA: 0x0001BD54 File Offset: 0x0001AD54
		internal void SetMatrix(float[][] newColorMatrix)
		{
			this.matrix00 = newColorMatrix[0][0];
			this.matrix01 = newColorMatrix[0][1];
			this.matrix02 = newColorMatrix[0][2];
			this.matrix03 = newColorMatrix[0][3];
			this.matrix04 = newColorMatrix[0][4];
			this.matrix10 = newColorMatrix[1][0];
			this.matrix11 = newColorMatrix[1][1];
			this.matrix12 = newColorMatrix[1][2];
			this.matrix13 = newColorMatrix[1][3];
			this.matrix14 = newColorMatrix[1][4];
			this.matrix20 = newColorMatrix[2][0];
			this.matrix21 = newColorMatrix[2][1];
			this.matrix22 = newColorMatrix[2][2];
			this.matrix23 = newColorMatrix[2][3];
			this.matrix24 = newColorMatrix[2][4];
			this.matrix30 = newColorMatrix[3][0];
			this.matrix31 = newColorMatrix[3][1];
			this.matrix32 = newColorMatrix[3][2];
			this.matrix33 = newColorMatrix[3][3];
			this.matrix34 = newColorMatrix[3][4];
			this.matrix40 = newColorMatrix[4][0];
			this.matrix41 = newColorMatrix[4][1];
			this.matrix42 = newColorMatrix[4][2];
			this.matrix43 = newColorMatrix[4][3];
			this.matrix44 = newColorMatrix[4][4];
		}

		// Token: 0x0600077B RID: 1915 RVA: 0x0001BE74 File Offset: 0x0001AE74
		internal float[][] GetMatrix()
		{
			float[][] array = new float[5][];
			for (int i = 0; i < 5; i++)
			{
				array[i] = new float[5];
			}
			array[0][0] = this.matrix00;
			array[0][1] = this.matrix01;
			array[0][2] = this.matrix02;
			array[0][3] = this.matrix03;
			array[0][4] = this.matrix04;
			array[1][0] = this.matrix10;
			array[1][1] = this.matrix11;
			array[1][2] = this.matrix12;
			array[1][3] = this.matrix13;
			array[1][4] = this.matrix14;
			array[2][0] = this.matrix20;
			array[2][1] = this.matrix21;
			array[2][2] = this.matrix22;
			array[2][3] = this.matrix23;
			array[2][4] = this.matrix24;
			array[3][0] = this.matrix30;
			array[3][1] = this.matrix31;
			array[3][2] = this.matrix32;
			array[3][3] = this.matrix33;
			array[3][4] = this.matrix34;
			array[4][0] = this.matrix40;
			array[4][1] = this.matrix41;
			array[4][2] = this.matrix42;
			array[4][3] = this.matrix43;
			array[4][4] = this.matrix44;
			return array;
		}

		// Token: 0x170002D0 RID: 720
		public float this[int row, int column]
		{
			get
			{
				return this.GetMatrix()[row][column];
			}
			set
			{
				float[][] matrix = this.GetMatrix();
				matrix[row][column] = value;
				this.SetMatrix(matrix);
			}
		}

		// Token: 0x040004B9 RID: 1209
		private float matrix00;

		// Token: 0x040004BA RID: 1210
		private float matrix01;

		// Token: 0x040004BB RID: 1211
		private float matrix02;

		// Token: 0x040004BC RID: 1212
		private float matrix03;

		// Token: 0x040004BD RID: 1213
		private float matrix04;

		// Token: 0x040004BE RID: 1214
		private float matrix10;

		// Token: 0x040004BF RID: 1215
		private float matrix11;

		// Token: 0x040004C0 RID: 1216
		private float matrix12;

		// Token: 0x040004C1 RID: 1217
		private float matrix13;

		// Token: 0x040004C2 RID: 1218
		private float matrix14;

		// Token: 0x040004C3 RID: 1219
		private float matrix20;

		// Token: 0x040004C4 RID: 1220
		private float matrix21;

		// Token: 0x040004C5 RID: 1221
		private float matrix22;

		// Token: 0x040004C6 RID: 1222
		private float matrix23;

		// Token: 0x040004C7 RID: 1223
		private float matrix24;

		// Token: 0x040004C8 RID: 1224
		private float matrix30;

		// Token: 0x040004C9 RID: 1225
		private float matrix31;

		// Token: 0x040004CA RID: 1226
		private float matrix32;

		// Token: 0x040004CB RID: 1227
		private float matrix33;

		// Token: 0x040004CC RID: 1228
		private float matrix34;

		// Token: 0x040004CD RID: 1229
		private float matrix40;

		// Token: 0x040004CE RID: 1230
		private float matrix41;

		// Token: 0x040004CF RID: 1231
		private float matrix42;

		// Token: 0x040004D0 RID: 1232
		private float matrix43;

		// Token: 0x040004D1 RID: 1233
		private float matrix44;
	}
}
