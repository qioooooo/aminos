using System;

namespace System.Windows.Forms
{
	// Token: 0x020002BE RID: 702
	public sealed class Cursors
	{
		// Token: 0x060026D9 RID: 9945 RVA: 0x0005FA01 File Offset: 0x0005EA01
		private Cursors()
		{
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x0005FA09 File Offset: 0x0005EA09
		internal static Cursor KnownCursorFromHCursor(IntPtr handle)
		{
			if (handle == IntPtr.Zero)
			{
				return null;
			}
			return new Cursor(handle);
		}

		// Token: 0x17000621 RID: 1569
		// (get) Token: 0x060026DB RID: 9947 RVA: 0x0005FA20 File Offset: 0x0005EA20
		public static Cursor AppStarting
		{
			get
			{
				if (Cursors.appStarting == null)
				{
					Cursors.appStarting = new Cursor(32650, 0);
				}
				return Cursors.appStarting;
			}
		}

		// Token: 0x17000622 RID: 1570
		// (get) Token: 0x060026DC RID: 9948 RVA: 0x0005FA44 File Offset: 0x0005EA44
		public static Cursor Arrow
		{
			get
			{
				if (Cursors.arrow == null)
				{
					Cursors.arrow = new Cursor(32512, 0);
				}
				return Cursors.arrow;
			}
		}

		// Token: 0x17000623 RID: 1571
		// (get) Token: 0x060026DD RID: 9949 RVA: 0x0005FA68 File Offset: 0x0005EA68
		public static Cursor Cross
		{
			get
			{
				if (Cursors.cross == null)
				{
					Cursors.cross = new Cursor(32515, 0);
				}
				return Cursors.cross;
			}
		}

		// Token: 0x17000624 RID: 1572
		// (get) Token: 0x060026DE RID: 9950 RVA: 0x0005FA8C File Offset: 0x0005EA8C
		public static Cursor Default
		{
			get
			{
				if (Cursors.defaultCursor == null)
				{
					Cursors.defaultCursor = new Cursor(32512, 0);
				}
				return Cursors.defaultCursor;
			}
		}

		// Token: 0x17000625 RID: 1573
		// (get) Token: 0x060026DF RID: 9951 RVA: 0x0005FAB0 File Offset: 0x0005EAB0
		public static Cursor IBeam
		{
			get
			{
				if (Cursors.iBeam == null)
				{
					Cursors.iBeam = new Cursor(32513, 0);
				}
				return Cursors.iBeam;
			}
		}

		// Token: 0x17000626 RID: 1574
		// (get) Token: 0x060026E0 RID: 9952 RVA: 0x0005FAD4 File Offset: 0x0005EAD4
		public static Cursor No
		{
			get
			{
				if (Cursors.no == null)
				{
					Cursors.no = new Cursor(32648, 0);
				}
				return Cursors.no;
			}
		}

		// Token: 0x17000627 RID: 1575
		// (get) Token: 0x060026E1 RID: 9953 RVA: 0x0005FAF8 File Offset: 0x0005EAF8
		public static Cursor SizeAll
		{
			get
			{
				if (Cursors.sizeAll == null)
				{
					Cursors.sizeAll = new Cursor(32646, 0);
				}
				return Cursors.sizeAll;
			}
		}

		// Token: 0x17000628 RID: 1576
		// (get) Token: 0x060026E2 RID: 9954 RVA: 0x0005FB1C File Offset: 0x0005EB1C
		public static Cursor SizeNESW
		{
			get
			{
				if (Cursors.sizeNESW == null)
				{
					Cursors.sizeNESW = new Cursor(32643, 0);
				}
				return Cursors.sizeNESW;
			}
		}

		// Token: 0x17000629 RID: 1577
		// (get) Token: 0x060026E3 RID: 9955 RVA: 0x0005FB40 File Offset: 0x0005EB40
		public static Cursor SizeNS
		{
			get
			{
				if (Cursors.sizeNS == null)
				{
					Cursors.sizeNS = new Cursor(32645, 0);
				}
				return Cursors.sizeNS;
			}
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060026E4 RID: 9956 RVA: 0x0005FB64 File Offset: 0x0005EB64
		public static Cursor SizeNWSE
		{
			get
			{
				if (Cursors.sizeNWSE == null)
				{
					Cursors.sizeNWSE = new Cursor(32642, 0);
				}
				return Cursors.sizeNWSE;
			}
		}

		// Token: 0x1700062B RID: 1579
		// (get) Token: 0x060026E5 RID: 9957 RVA: 0x0005FB88 File Offset: 0x0005EB88
		public static Cursor SizeWE
		{
			get
			{
				if (Cursors.sizeWE == null)
				{
					Cursors.sizeWE = new Cursor(32644, 0);
				}
				return Cursors.sizeWE;
			}
		}

		// Token: 0x1700062C RID: 1580
		// (get) Token: 0x060026E6 RID: 9958 RVA: 0x0005FBAC File Offset: 0x0005EBAC
		public static Cursor UpArrow
		{
			get
			{
				if (Cursors.upArrow == null)
				{
					Cursors.upArrow = new Cursor(32516, 0);
				}
				return Cursors.upArrow;
			}
		}

		// Token: 0x1700062D RID: 1581
		// (get) Token: 0x060026E7 RID: 9959 RVA: 0x0005FBD0 File Offset: 0x0005EBD0
		public static Cursor WaitCursor
		{
			get
			{
				if (Cursors.wait == null)
				{
					Cursors.wait = new Cursor(32514, 0);
				}
				return Cursors.wait;
			}
		}

		// Token: 0x1700062E RID: 1582
		// (get) Token: 0x060026E8 RID: 9960 RVA: 0x0005FBF4 File Offset: 0x0005EBF4
		public static Cursor Help
		{
			get
			{
				if (Cursors.help == null)
				{
					Cursors.help = new Cursor(32651, 0);
				}
				return Cursors.help;
			}
		}

		// Token: 0x1700062F RID: 1583
		// (get) Token: 0x060026E9 RID: 9961 RVA: 0x0005FC18 File Offset: 0x0005EC18
		public static Cursor HSplit
		{
			get
			{
				if (Cursors.hSplit == null)
				{
					Cursors.hSplit = new Cursor("hsplit.cur", 0);
				}
				return Cursors.hSplit;
			}
		}

		// Token: 0x17000630 RID: 1584
		// (get) Token: 0x060026EA RID: 9962 RVA: 0x0005FC3C File Offset: 0x0005EC3C
		public static Cursor VSplit
		{
			get
			{
				if (Cursors.vSplit == null)
				{
					Cursors.vSplit = new Cursor("vsplit.cur", 0);
				}
				return Cursors.vSplit;
			}
		}

		// Token: 0x17000631 RID: 1585
		// (get) Token: 0x060026EB RID: 9963 RVA: 0x0005FC60 File Offset: 0x0005EC60
		public static Cursor NoMove2D
		{
			get
			{
				if (Cursors.noMove2D == null)
				{
					Cursors.noMove2D = new Cursor("nomove2d.cur", 0);
				}
				return Cursors.noMove2D;
			}
		}

		// Token: 0x17000632 RID: 1586
		// (get) Token: 0x060026EC RID: 9964 RVA: 0x0005FC84 File Offset: 0x0005EC84
		public static Cursor NoMoveHoriz
		{
			get
			{
				if (Cursors.noMoveHoriz == null)
				{
					Cursors.noMoveHoriz = new Cursor("nomoveh.cur", 0);
				}
				return Cursors.noMoveHoriz;
			}
		}

		// Token: 0x17000633 RID: 1587
		// (get) Token: 0x060026ED RID: 9965 RVA: 0x0005FCA8 File Offset: 0x0005ECA8
		public static Cursor NoMoveVert
		{
			get
			{
				if (Cursors.noMoveVert == null)
				{
					Cursors.noMoveVert = new Cursor("nomovev.cur", 0);
				}
				return Cursors.noMoveVert;
			}
		}

		// Token: 0x17000634 RID: 1588
		// (get) Token: 0x060026EE RID: 9966 RVA: 0x0005FCCC File Offset: 0x0005ECCC
		public static Cursor PanEast
		{
			get
			{
				if (Cursors.panEast == null)
				{
					Cursors.panEast = new Cursor("east.cur", 0);
				}
				return Cursors.panEast;
			}
		}

		// Token: 0x17000635 RID: 1589
		// (get) Token: 0x060026EF RID: 9967 RVA: 0x0005FCF0 File Offset: 0x0005ECF0
		public static Cursor PanNE
		{
			get
			{
				if (Cursors.panNE == null)
				{
					Cursors.panNE = new Cursor("ne.cur", 0);
				}
				return Cursors.panNE;
			}
		}

		// Token: 0x17000636 RID: 1590
		// (get) Token: 0x060026F0 RID: 9968 RVA: 0x0005FD14 File Offset: 0x0005ED14
		public static Cursor PanNorth
		{
			get
			{
				if (Cursors.panNorth == null)
				{
					Cursors.panNorth = new Cursor("north.cur", 0);
				}
				return Cursors.panNorth;
			}
		}

		// Token: 0x17000637 RID: 1591
		// (get) Token: 0x060026F1 RID: 9969 RVA: 0x0005FD38 File Offset: 0x0005ED38
		public static Cursor PanNW
		{
			get
			{
				if (Cursors.panNW == null)
				{
					Cursors.panNW = new Cursor("nw.cur", 0);
				}
				return Cursors.panNW;
			}
		}

		// Token: 0x17000638 RID: 1592
		// (get) Token: 0x060026F2 RID: 9970 RVA: 0x0005FD5C File Offset: 0x0005ED5C
		public static Cursor PanSE
		{
			get
			{
				if (Cursors.panSE == null)
				{
					Cursors.panSE = new Cursor("se.cur", 0);
				}
				return Cursors.panSE;
			}
		}

		// Token: 0x17000639 RID: 1593
		// (get) Token: 0x060026F3 RID: 9971 RVA: 0x0005FD80 File Offset: 0x0005ED80
		public static Cursor PanSouth
		{
			get
			{
				if (Cursors.panSouth == null)
				{
					Cursors.panSouth = new Cursor("south.cur", 0);
				}
				return Cursors.panSouth;
			}
		}

		// Token: 0x1700063A RID: 1594
		// (get) Token: 0x060026F4 RID: 9972 RVA: 0x0005FDA4 File Offset: 0x0005EDA4
		public static Cursor PanSW
		{
			get
			{
				if (Cursors.panSW == null)
				{
					Cursors.panSW = new Cursor("sw.cur", 0);
				}
				return Cursors.panSW;
			}
		}

		// Token: 0x1700063B RID: 1595
		// (get) Token: 0x060026F5 RID: 9973 RVA: 0x0005FDC8 File Offset: 0x0005EDC8
		public static Cursor PanWest
		{
			get
			{
				if (Cursors.panWest == null)
				{
					Cursors.panWest = new Cursor("west.cur", 0);
				}
				return Cursors.panWest;
			}
		}

		// Token: 0x1700063C RID: 1596
		// (get) Token: 0x060026F6 RID: 9974 RVA: 0x0005FDEC File Offset: 0x0005EDEC
		public static Cursor Hand
		{
			get
			{
				if (Cursors.hand == null)
				{
					Cursors.hand = new Cursor("hand.cur", 0);
				}
				return Cursors.hand;
			}
		}

		// Token: 0x0400165F RID: 5727
		private static Cursor appStarting;

		// Token: 0x04001660 RID: 5728
		private static Cursor arrow;

		// Token: 0x04001661 RID: 5729
		private static Cursor cross;

		// Token: 0x04001662 RID: 5730
		private static Cursor defaultCursor;

		// Token: 0x04001663 RID: 5731
		private static Cursor iBeam;

		// Token: 0x04001664 RID: 5732
		private static Cursor no;

		// Token: 0x04001665 RID: 5733
		private static Cursor sizeAll;

		// Token: 0x04001666 RID: 5734
		private static Cursor sizeNESW;

		// Token: 0x04001667 RID: 5735
		private static Cursor sizeNS;

		// Token: 0x04001668 RID: 5736
		private static Cursor sizeNWSE;

		// Token: 0x04001669 RID: 5737
		private static Cursor sizeWE;

		// Token: 0x0400166A RID: 5738
		private static Cursor upArrow;

		// Token: 0x0400166B RID: 5739
		private static Cursor wait;

		// Token: 0x0400166C RID: 5740
		private static Cursor help;

		// Token: 0x0400166D RID: 5741
		private static Cursor hSplit;

		// Token: 0x0400166E RID: 5742
		private static Cursor vSplit;

		// Token: 0x0400166F RID: 5743
		private static Cursor noMove2D;

		// Token: 0x04001670 RID: 5744
		private static Cursor noMoveHoriz;

		// Token: 0x04001671 RID: 5745
		private static Cursor noMoveVert;

		// Token: 0x04001672 RID: 5746
		private static Cursor panEast;

		// Token: 0x04001673 RID: 5747
		private static Cursor panNE;

		// Token: 0x04001674 RID: 5748
		private static Cursor panNorth;

		// Token: 0x04001675 RID: 5749
		private static Cursor panNW;

		// Token: 0x04001676 RID: 5750
		private static Cursor panSE;

		// Token: 0x04001677 RID: 5751
		private static Cursor panSouth;

		// Token: 0x04001678 RID: 5752
		private static Cursor panSW;

		// Token: 0x04001679 RID: 5753
		private static Cursor panWest;

		// Token: 0x0400167A RID: 5754
		private static Cursor hand;
	}
}
