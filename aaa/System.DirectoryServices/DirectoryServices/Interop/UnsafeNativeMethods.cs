using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace System.DirectoryServices.Interop
{
	// Token: 0x02000060 RID: 96
	[SuppressUnmanagedCodeSecurity]
	[ComVisible(false)]
	internal class UnsafeNativeMethods
	{
		// Token: 0x06000219 RID: 537
		[DllImport("activeds.dll", CharSet = CharSet.Unicode, EntryPoint = "ADsOpenObject", ExactSpelling = true)]
		private static extern int IntADsOpenObject(string path, string userName, string password, int flags, [In] [Out] ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object ppObject);

		// Token: 0x0600021A RID: 538 RVA: 0x00008838 File Offset: 0x00007838
		public static int ADsOpenObject(string path, string userName, string password, int flags, [In] [Out] ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object ppObject)
		{
			int num;
			try
			{
				num = UnsafeNativeMethods.IntADsOpenObject(path, userName, password, flags, ref iid, out ppObject);
			}
			catch (EntryPointNotFoundException)
			{
				throw new InvalidOperationException(Res.GetString("DSAdsiNotInstalled"));
			}
			return num;
		}

		// Token: 0x0400029A RID: 666
		internal const int S_ADS_NOMORE_ROWS = 20498;

		// Token: 0x0400029B RID: 667
		internal const int INVALID_FILTER = -2147016642;

		// Token: 0x0400029C RID: 668
		internal const int SIZE_LIMIT_EXCEEDED = -2147016669;

		// Token: 0x02000061 RID: 97
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("FD8256D0-FD15-11CE-ABC4-02608C9E7553")]
		[ComImport]
		public interface IAds
		{
			// Token: 0x17000087 RID: 135
			// (get) Token: 0x0600021C RID: 540
			string Name
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000088 RID: 136
			// (get) Token: 0x0600021D RID: 541
			string Class
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x17000089 RID: 137
			// (get) Token: 0x0600021E RID: 542
			string GUID
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x1700008A RID: 138
			// (get) Token: 0x0600021F RID: 543
			string ADsPath
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x1700008B RID: 139
			// (get) Token: 0x06000220 RID: 544
			string Parent
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x1700008C RID: 140
			// (get) Token: 0x06000221 RID: 545
			string Schema
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
			}

			// Token: 0x06000222 RID: 546
			[SuppressUnmanagedCodeSecurity]
			void GetInfo();

			// Token: 0x06000223 RID: 547
			[SuppressUnmanagedCodeSecurity]
			void SetInfo();

			// Token: 0x06000224 RID: 548
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Struct)]
			object Get([MarshalAs(UnmanagedType.BStr)] [In] string bstrName);

			// Token: 0x06000225 RID: 549
			[SuppressUnmanagedCodeSecurity]
			void Put([MarshalAs(UnmanagedType.BStr)] [In] string bstrName, [MarshalAs(UnmanagedType.Struct)] [In] object vProp);

			// Token: 0x06000226 RID: 550
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetEx([MarshalAs(UnmanagedType.BStr)] [In] string bstrName, [MarshalAs(UnmanagedType.Struct)] out object value);

			// Token: 0x06000227 RID: 551
			[SuppressUnmanagedCodeSecurity]
			void PutEx([MarshalAs(UnmanagedType.U4)] [In] int lnControlCode, [MarshalAs(UnmanagedType.BStr)] [In] string bstrName, [MarshalAs(UnmanagedType.Struct)] [In] object vProp);

			// Token: 0x06000228 RID: 552
			[SuppressUnmanagedCodeSecurity]
			void GetInfoEx([MarshalAs(UnmanagedType.Struct)] [In] object vProperties, [MarshalAs(UnmanagedType.U4)] [In] int lnReserved);
		}

		// Token: 0x02000062 RID: 98
		[Guid("001677D0-FD16-11CE-ABC4-02608C9E7553")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IAdsContainer
		{
			// Token: 0x1700008D RID: 141
			// (get) Token: 0x06000229 RID: 553
			int Count
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.U4)]
				get;
			}

			// Token: 0x1700008E RID: 142
			// (get) Token: 0x0600022A RID: 554
			object _NewEnum
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Interface)]
				get;
			}

			// Token: 0x1700008F RID: 143
			// (get) Token: 0x0600022B RID: 555
			// (set) Token: 0x0600022C RID: 556
			object Filter
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[SuppressUnmanagedCodeSecurity]
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x17000090 RID: 144
			// (get) Token: 0x0600022D RID: 557
			// (set) Token: 0x0600022E RID: 558
			object Hints
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[SuppressUnmanagedCodeSecurity]
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x0600022F RID: 559
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Interface)]
			object GetObject([MarshalAs(UnmanagedType.BStr)] [In] string className, [MarshalAs(UnmanagedType.BStr)] [In] string relativeName);

			// Token: 0x06000230 RID: 560
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Interface)]
			object Create([MarshalAs(UnmanagedType.BStr)] [In] string className, [MarshalAs(UnmanagedType.BStr)] [In] string relativeName);

			// Token: 0x06000231 RID: 561
			[SuppressUnmanagedCodeSecurity]
			void Delete([MarshalAs(UnmanagedType.BStr)] [In] string className, [MarshalAs(UnmanagedType.BStr)] [In] string relativeName);

			// Token: 0x06000232 RID: 562
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Interface)]
			object CopyHere([MarshalAs(UnmanagedType.BStr)] [In] string sourceName, [MarshalAs(UnmanagedType.BStr)] [In] string newName);

			// Token: 0x06000233 RID: 563
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Interface)]
			object MoveHere([MarshalAs(UnmanagedType.BStr)] [In] string sourceName, [MarshalAs(UnmanagedType.BStr)] [In] string newName);
		}

		// Token: 0x02000063 RID: 99
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("B2BD0902-8878-11D1-8C21-00C04FD8D503")]
		[ComImport]
		public interface IAdsDeleteOps
		{
			// Token: 0x06000234 RID: 564
			[SuppressUnmanagedCodeSecurity]
			void DeleteObject(int flags);
		}

		// Token: 0x02000064 RID: 100
		[Guid("7b9e38b0-a97c-11d0-8534-00c04fd8d503")]
		[ComImport]
		public class PropertyValue
		{
			// Token: 0x06000235 RID: 565
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern PropertyValue();
		}

		// Token: 0x02000065 RID: 101
		[Guid("79FA9AD0-A97C-11D0-8534-00C04FD8D503")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IAdsPropertyValue
		{
			// Token: 0x06000236 RID: 566
			[SuppressUnmanagedCodeSecurity]
			void Clear();

			// Token: 0x17000091 RID: 145
			// (get) Token: 0x06000237 RID: 567
			// (set) Token: 0x06000238 RID: 568
			int ADsType
			{
				[SuppressUnmanagedCodeSecurity]
				get;
				[SuppressUnmanagedCodeSecurity]
				set;
			}

			// Token: 0x17000092 RID: 146
			// (get) Token: 0x06000239 RID: 569
			// (set) Token: 0x0600023A RID: 570
			string DNString
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x17000093 RID: 147
			// (get) Token: 0x0600023B RID: 571
			// (set) Token: 0x0600023C RID: 572
			string CaseExactString
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x17000094 RID: 148
			// (get) Token: 0x0600023D RID: 573
			// (set) Token: 0x0600023E RID: 574
			string CaseIgnoreString
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x17000095 RID: 149
			// (get) Token: 0x0600023F RID: 575
			// (set) Token: 0x06000240 RID: 576
			string PrintableString
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x17000096 RID: 150
			// (get) Token: 0x06000241 RID: 577
			// (set) Token: 0x06000242 RID: 578
			string NumericString
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x17000097 RID: 151
			// (get) Token: 0x06000243 RID: 579
			// (set) Token: 0x06000244 RID: 580
			bool Boolean { get; set; }

			// Token: 0x17000098 RID: 152
			// (get) Token: 0x06000245 RID: 581
			// (set) Token: 0x06000246 RID: 582
			int Integer { get; set; }

			// Token: 0x17000099 RID: 153
			// (get) Token: 0x06000247 RID: 583
			// (set) Token: 0x06000248 RID: 584
			object OctetString
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[SuppressUnmanagedCodeSecurity]
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x1700009A RID: 154
			// (get) Token: 0x06000249 RID: 585
			// (set) Token: 0x0600024A RID: 586
			object SecurityDescriptor
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x1700009B RID: 155
			// (get) Token: 0x0600024B RID: 587
			// (set) Token: 0x0600024C RID: 588
			object LargeInteger
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}

			// Token: 0x1700009C RID: 156
			// (get) Token: 0x0600024D RID: 589
			// (set) Token: 0x0600024E RID: 590
			object UTCTime
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}
		}

		// Token: 0x02000066 RID: 102
		[Guid("72d3edc2-a4c4-11d0-8533-00c04fd8d503")]
		[ComImport]
		public class PropertyEntry
		{
			// Token: 0x0600024F RID: 591
			[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
			public extern PropertyEntry();
		}

		// Token: 0x02000067 RID: 103
		[Guid("05792C8E-941F-11D0-8529-00C04FD8D503")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IAdsPropertyEntry
		{
			// Token: 0x06000250 RID: 592
			[SuppressUnmanagedCodeSecurity]
			void Clear();

			// Token: 0x1700009D RID: 157
			// (get) Token: 0x06000251 RID: 593
			// (set) Token: 0x06000252 RID: 594
			string Name
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.BStr)]
				get;
				[SuppressUnmanagedCodeSecurity]
				[param: MarshalAs(UnmanagedType.BStr)]
				set;
			}

			// Token: 0x1700009E RID: 158
			// (get) Token: 0x06000253 RID: 595
			// (set) Token: 0x06000254 RID: 596
			int ADsType
			{
				[SuppressUnmanagedCodeSecurity]
				get;
				[SuppressUnmanagedCodeSecurity]
				set;
			}

			// Token: 0x1700009F RID: 159
			// (get) Token: 0x06000255 RID: 597
			// (set) Token: 0x06000256 RID: 598
			int ControlCode
			{
				[SuppressUnmanagedCodeSecurity]
				get;
				[SuppressUnmanagedCodeSecurity]
				set;
			}

			// Token: 0x170000A0 RID: 160
			// (get) Token: 0x06000257 RID: 599
			// (set) Token: 0x06000258 RID: 600
			object Values
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.Struct)]
				get;
				[SuppressUnmanagedCodeSecurity]
				[param: MarshalAs(UnmanagedType.Struct)]
				set;
			}
		}

		// Token: 0x02000068 RID: 104
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("C6F602B6-8F69-11D0-8528-00C04FD8D503")]
		[ComImport]
		public interface IAdsPropertyList
		{
			// Token: 0x170000A1 RID: 161
			// (get) Token: 0x06000259 RID: 601
			int PropertyCount
			{
				[SuppressUnmanagedCodeSecurity]
				[return: MarshalAs(UnmanagedType.U4)]
				get;
			}

			// Token: 0x0600025A RID: 602
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.I4)]
			int Next([MarshalAs(UnmanagedType.Struct)] out object nextProp);

			// Token: 0x0600025B RID: 603
			void Skip([In] int cElements);

			// Token: 0x0600025C RID: 604
			[SuppressUnmanagedCodeSecurity]
			void Reset();

			// Token: 0x0600025D RID: 605
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Struct)]
			object Item([MarshalAs(UnmanagedType.Struct)] [In] object varIndex);

			// Token: 0x0600025E RID: 606
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetPropertyItem([MarshalAs(UnmanagedType.BStr)] [In] string bstrName, int ADsType);

			// Token: 0x0600025F RID: 607
			[SuppressUnmanagedCodeSecurity]
			void PutPropertyItem([MarshalAs(UnmanagedType.Struct)] [In] object varData);

			// Token: 0x06000260 RID: 608
			void ResetPropertyItem([MarshalAs(UnmanagedType.Struct)] [In] object varEntry);

			// Token: 0x06000261 RID: 609
			void PurgePropertyList();
		}

		// Token: 0x02000069 RID: 105
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("109BA8EC-92F0-11D0-A790-00C04FD8D5A8")]
		[ComImport]
		public interface IDirectorySearch
		{
			// Token: 0x06000262 RID: 610
			[SuppressUnmanagedCodeSecurity]
			void SetSearchPreference([In] IntPtr pSearchPrefs, int dwNumPrefs);

			// Token: 0x06000263 RID: 611
			[SuppressUnmanagedCodeSecurity]
			void ExecuteSearch([MarshalAs(UnmanagedType.LPWStr)] [In] string pszSearchFilter, [MarshalAs(UnmanagedType.LPArray)] [In] string[] pAttributeNames, [In] int dwNumberAttributes, out IntPtr hSearchResult);

			// Token: 0x06000264 RID: 612
			[SuppressUnmanagedCodeSecurity]
			void AbandonSearch([In] IntPtr hSearchResult);

			// Token: 0x06000265 RID: 613
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.U4)]
			int GetFirstRow([In] IntPtr hSearchResult);

			// Token: 0x06000266 RID: 614
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.U4)]
			int GetNextRow([In] IntPtr hSearchResult);

			// Token: 0x06000267 RID: 615
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.U4)]
			int GetPreviousRow([In] IntPtr hSearchResult);

			// Token: 0x06000268 RID: 616
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			[return: MarshalAs(UnmanagedType.U4)]
			int GetNextColumnName([In] IntPtr hSearchResult, [Out] IntPtr ppszColumnName);

			// Token: 0x06000269 RID: 617
			[SuppressUnmanagedCodeSecurity]
			void GetColumn([In] IntPtr hSearchResult, [In] IntPtr szColumnName, [In] IntPtr pSearchColumn);

			// Token: 0x0600026A RID: 618
			[SuppressUnmanagedCodeSecurity]
			void FreeColumn([In] IntPtr pSearchColumn);

			// Token: 0x0600026B RID: 619
			[SuppressUnmanagedCodeSecurity]
			void CloseSearchHandle([In] IntPtr hSearchResult);
		}

		// Token: 0x0200006A RID: 106
		[Guid("46F14FDA-232B-11D1-A808-00C04FD8D5A8")]
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[ComImport]
		public interface IAdsObjectOptions
		{
			// Token: 0x0600026C RID: 620
			[SuppressUnmanagedCodeSecurity]
			[return: MarshalAs(UnmanagedType.Struct)]
			object GetOption(int flag);

			// Token: 0x0600026D RID: 621
			[SuppressUnmanagedCodeSecurity]
			void SetOption(int flag, [MarshalAs(UnmanagedType.Struct)] [In] object varValue);
		}

		// Token: 0x0200006B RID: 107
		[InterfaceType(ComInterfaceType.InterfaceIsDual)]
		[Guid("46f14fda-232b-11d1-a808-00c04fd8d5a8")]
		[ComImport]
		public interface IAdsObjectOptions2
		{
			// Token: 0x0600026E RID: 622
			[SuppressUnmanagedCodeSecurity]
			[PreserveSig]
			int GetOption(int flag, [MarshalAs(UnmanagedType.Struct)] out object value);

			// Token: 0x0600026F RID: 623
			[SuppressUnmanagedCodeSecurity]
			void SetOption(int option, Variant value);
		}
	}
}
