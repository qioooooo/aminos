using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000CB RID: 203
	[SuppressUnmanagedCodeSecurity]
	[ComVisible(false)]
	internal sealed class NativeComInterfaces
	{
		// Token: 0x040004FA RID: 1274
		internal const int ADS_SETTYPE_DN = 4;

		// Token: 0x040004FB RID: 1275
		internal const int ADS_FORMAT_X500_DN = 7;

		// Token: 0x040004FC RID: 1276
		internal const int ADS_ESCAPEDMODE_ON = 2;

		// Token: 0x040004FD RID: 1277
		internal const int ADS_ESCAPEDMODE_OFF_EX = 4;

		// Token: 0x040004FE RID: 1278
		internal const int ADS_FORMAT_LEAF = 11;

		// Token: 0x020000CC RID: 204
		[Guid("080d0d78-f421-11d0-a36e-00c04fb950dc")]
		[ComImport]
		internal class Pathname
		{
			// Token: 0x06000628 RID: 1576
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern Pathname();
		}

		// Token: 0x020000CD RID: 205
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("D592AED4-F420-11D0-A36E-00C04FB950DC")]
		[ComImport]
		internal interface IAdsPathname
		{
			// Token: 0x06000629 RID: 1577
			[SuppressUnmanagedCodeSecurity]
			int Set([MarshalAs(UnmanagedType.BStr)] [In] string bstrADsPath, [MarshalAs(UnmanagedType.U4)] [In] int lnSetType);

			// Token: 0x0600062A RID: 1578
			int SetDisplayType([MarshalAs(UnmanagedType.U4)] [In] int lnDisplayType);

			// Token: 0x0600062B RID: 1579
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.BStr)]
			string Retrieve([MarshalAs(UnmanagedType.U4)] [In] int lnFormatType);

			// Token: 0x0600062C RID: 1580
			[return: MarshalAs(UnmanagedType.U4)]
			int GetNumElements();

			// Token: 0x0600062D RID: 1581
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetElement([MarshalAs(UnmanagedType.U4)] [In] int lnElementIndex);

			// Token: 0x0600062E RID: 1582
			void AddLeafElement([MarshalAs(UnmanagedType.BStr)] [In] string bstrLeafElement);

			// Token: 0x0600062F RID: 1583
			void RemoveLeafElement();

			// Token: 0x06000630 RID: 1584
			[return: MarshalAs(UnmanagedType.Interface)]
			object CopyPath();

			// Token: 0x06000631 RID: 1585
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.BStr)]
			string GetEscapedElement([MarshalAs(UnmanagedType.U4)] [In] int lnReserved, [MarshalAs(UnmanagedType.BStr)] [In] string bstrInStr);

			// Token: 0x17000166 RID: 358
			// (get) Token: 0x06000632 RID: 1586
			// (set) Token: 0x06000633 RID: 1587
			int EscapedMode
			{
				get; [SuppressUnmanagedCodeSecurity]
				set;
			}
		}

		// Token: 0x020000CE RID: 206
		[Guid("C8F93DD3-4AE0-11CF-9E73-00AA004A5691")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		internal interface IAdsProperty
		{
			// Token: 0x17000167 RID: 359
			// (get) Token: 0x06000634 RID: 1588
			string Name
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000168 RID: 360
			// (get) Token: 0x06000635 RID: 1589
			string Class
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000169 RID: 361
			// (get) Token: 0x06000636 RID: 1590
			string GUID
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x1700016A RID: 362
			// (get) Token: 0x06000637 RID: 1591
			string ADsPath
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x1700016B RID: 363
			// (get) Token: 0x06000638 RID: 1592
			string Parent
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x1700016C RID: 364
			// (get) Token: 0x06000639 RID: 1593
			string Schema
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x0600063A RID: 1594
			void GetInfo();

			// Token: 0x0600063B RID: 1595
			void SetInfo();

			// Token: 0x0600063C RID: 1596
			[return: MarshalAs(UnmanagedType.Struct)]
			object Get([MarshalAs(UnmanagedType.BStr)] [In] string bstrName);

			// Token: 0x0600063D RID: 1597
			void Put([MarshalAs(UnmanagedType.BStr)] [In] string bstrName, [MarshalAs(UnmanagedType.Struct)] [In] object vProp);

			// Token: 0x0600063E RID: 1598
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetEx([MarshalAs(UnmanagedType.BStr)] [In] string bstrName);

			// Token: 0x0600063F RID: 1599
			void PutEx([MarshalAs(UnmanagedType.U4)] [In] int lnControlCode, [MarshalAs(UnmanagedType.BStr)] [In] string bstrName, [MarshalAs(UnmanagedType.Struct)] [In] object vProp);

			// Token: 0x06000640 RID: 1600
			void GetInfoEx([MarshalAs(UnmanagedType.Struct)] [In] object vProperties, [MarshalAs(UnmanagedType.U4)] [In] int lnReserved);

			// Token: 0x1700016D RID: 365
			// (get) Token: 0x06000641 RID: 1601
			// (set) Token: 0x06000642 RID: 1602
			string OID
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x1700016E RID: 366
			// (get) Token: 0x06000643 RID: 1603
			// (set) Token: 0x06000644 RID: 1604
			string Syntax
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x1700016F RID: 367
			// (get) Token: 0x06000645 RID: 1605
			// (set) Token: 0x06000646 RID: 1606
			int MaxRange
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.U4)]
				get;
				[param: MarshalAs(UnmanagedType.U4)]
				set;
			}

			// Token: 0x17000170 RID: 368
			// (get) Token: 0x06000647 RID: 1607
			// (set) Token: 0x06000648 RID: 1608
			int MinRange
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.U4)]
				get;
				[param: MarshalAs(UnmanagedType.U4)]
				set;
			}

			// Token: 0x17000171 RID: 369
			// (get) Token: 0x06000649 RID: 1609
			// (set) Token: 0x0600064A RID: 1610
			bool MultiValued
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.VariantBool)]
				get;
				[param: MarshalAs(UnmanagedType.VariantBool)]
				set;
			}

			// Token: 0x0600064B RID: 1611
			object Qualifiers();
		}

		// Token: 0x020000CF RID: 207
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("C8F93DD0-4AE0-11CF-9E73-00AA004A5691")]
		[ComImport]
		internal interface IAdsClass
		{
			// Token: 0x17000172 RID: 370
			// (get) Token: 0x0600064C RID: 1612
			string Name
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000173 RID: 371
			// (get) Token: 0x0600064D RID: 1613
			string Class
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000174 RID: 372
			// (get) Token: 0x0600064E RID: 1614
			string GUID
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000175 RID: 373
			// (get) Token: 0x0600064F RID: 1615
			string ADsPath
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x06000650 RID: 1616
			string Parent
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x06000651 RID: 1617
			string Schema
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x06000652 RID: 1618
			void GetInfo();

			// Token: 0x06000653 RID: 1619
			void SetInfo();

			// Token: 0x06000654 RID: 1620
			[return: MarshalAs(UnmanagedType.Struct)]
			object Get([MarshalAs(UnmanagedType.BStr)] [In] string bstrName);

			// Token: 0x06000655 RID: 1621
			void Put([MarshalAs(UnmanagedType.BStr)] [In] string bstrName, [MarshalAs(UnmanagedType.Struct)] [In] object vProp);

			// Token: 0x06000656 RID: 1622
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetEx([MarshalAs(UnmanagedType.BStr)] [In] string bstrName);

			// Token: 0x06000657 RID: 1623
			void PutEx([MarshalAs(UnmanagedType.U4)] [In] int lnControlCode, [MarshalAs(UnmanagedType.BStr)] [In] string bstrName, [MarshalAs(UnmanagedType.Struct)] [In] object vProp);

			// Token: 0x06000658 RID: 1624
			void GetInfoEx([MarshalAs(UnmanagedType.Struct)] [In] object vProperties, [MarshalAs(UnmanagedType.U4)] [In] int lnReserved);

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000659 RID: 1625
			string PrimaryInterface
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x0600065A RID: 1626
			// (set) Token: 0x0600065B RID: 1627
			string CLSID
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x0600065C RID: 1628
			// (set) Token: 0x0600065D RID: 1629
			string OID
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x1700017B RID: 379
			// (get) Token: 0x0600065E RID: 1630
			// (set) Token: 0x0600065F RID: 1631
			bool Abstract
			{
				[return: MarshalAs(UnmanagedType.VariantBool)]
				get;
				[param: MarshalAs(UnmanagedType.VariantBool)]
				set;
			}

			// Token: 0x1700017C RID: 380
			// (get) Token: 0x06000660 RID: 1632
			// (set) Token: 0x06000661 RID: 1633
			bool Auxiliary
			{
				[return: MarshalAs(UnmanagedType.VariantBool)]
				get;
				[param: MarshalAs(UnmanagedType.VariantBool)]
				set;
			}

			// Token: 0x1700017D RID: 381
			// (get) Token: 0x06000662 RID: 1634
			// (set) Token: 0x06000663 RID: 1635
			object MandatoryProperties
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x1700017E RID: 382
			// (get) Token: 0x06000664 RID: 1636
			// (set) Token: 0x06000665 RID: 1637
			object OptionalProperties
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x1700017F RID: 383
			// (get) Token: 0x06000666 RID: 1638
			// (set) Token: 0x06000667 RID: 1639
			object NamingProperties
			{
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x17000180 RID: 384
			// (get) Token: 0x06000668 RID: 1640
			// (set) Token: 0x06000669 RID: 1641
			object DerivedFrom
			{
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x17000181 RID: 385
			// (get) Token: 0x0600066A RID: 1642
			// (set) Token: 0x0600066B RID: 1643
			object AuxDerivedFrom
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x17000182 RID: 386
			// (get) Token: 0x0600066C RID: 1644
			// (set) Token: 0x0600066D RID: 1645
			object PossibleSuperiors
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x17000183 RID: 387
			// (get) Token: 0x0600066E RID: 1646
			// (set) Token: 0x0600066F RID: 1647
			object Containment
			{
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x17000184 RID: 388
			// (get) Token: 0x06000670 RID: 1648
			// (set) Token: 0x06000671 RID: 1649
			bool Container
			{
				[return: MarshalAs(UnmanagedType.VariantBool)]
				get;
				[param: MarshalAs(UnmanagedType.VariantBool)]
				set;
			}

			// Token: 0x17000185 RID: 389
			// (get) Token: 0x06000672 RID: 1650
			// (set) Token: 0x06000673 RID: 1651
			string HelpFileName
			{
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x17000186 RID: 390
			// (get) Token: 0x06000674 RID: 1652
			// (set) Token: 0x06000675 RID: 1653
			int HelpFileContext
			{
				[return: MarshalAs(UnmanagedType.U4)]
				get;
				[param: MarshalAs(UnmanagedType.U4)]
				set;
			}

			// Token: 0x06000676 RID: 1654
			[return: MarshalAs(UnmanagedType.Interface)]
			object Qualifiers();
		}
	}
}
