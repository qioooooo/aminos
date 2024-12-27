using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Interop;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices
{
	// Token: 0x02000022 RID: 34
	[DSDescription("DirectorySearcherDesc")]
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class DirectorySearcher : global::System.ComponentModel.Component
	{
		// Token: 0x060000C3 RID: 195 RVA: 0x000046AD File Offset: 0x000036AD
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectorySearcher()
			: this(null, "(objectClass=*)", null, SearchScope.Subtree)
		{
			this.scopeSpecified = false;
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x000046C4 File Offset: 0x000036C4
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectorySearcher(DirectoryEntry searchRoot)
			: this(searchRoot, "(objectClass=*)", null, SearchScope.Subtree)
		{
			this.scopeSpecified = false;
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x000046DB File Offset: 0x000036DB
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectorySearcher(DirectoryEntry searchRoot, string filter)
			: this(searchRoot, filter, null, SearchScope.Subtree)
		{
			this.scopeSpecified = false;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x000046EE File Offset: 0x000036EE
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectorySearcher(DirectoryEntry searchRoot, string filter, string[] propertiesToLoad)
			: this(searchRoot, filter, propertiesToLoad, SearchScope.Subtree)
		{
			this.scopeSpecified = false;
		}

		// Token: 0x060000C7 RID: 199 RVA: 0x00004701 File Offset: 0x00003701
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectorySearcher(string filter)
			: this(null, filter, null, SearchScope.Subtree)
		{
			this.scopeSpecified = false;
		}

		// Token: 0x060000C8 RID: 200 RVA: 0x00004714 File Offset: 0x00003714
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectorySearcher(string filter, string[] propertiesToLoad)
			: this(null, filter, propertiesToLoad, SearchScope.Subtree)
		{
			this.scopeSpecified = false;
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00004727 File Offset: 0x00003727
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectorySearcher(string filter, string[] propertiesToLoad, SearchScope scope)
			: this(null, filter, propertiesToLoad, scope)
		{
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00004734 File Offset: 0x00003734
		[DirectoryServicesPermission(SecurityAction.Demand, Unrestricted = true)]
		public DirectorySearcher(DirectoryEntry searchRoot, string filter, string[] propertiesToLoad, SearchScope scope)
		{
			this.searchRoot = searchRoot;
			this.filter = filter;
			if (propertiesToLoad != null)
			{
				this.PropertiesToLoad.AddRange(propertiesToLoad);
			}
			this.SearchScope = scope;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x000047CB File Offset: 0x000037CB
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed && disposing)
			{
				if (this.rootEntryAllocated)
				{
					this.searchRoot.Dispose();
				}
				this.rootEntryAllocated = false;
				this.disposed = true;
			}
			base.Dispose(disposing);
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x060000CC RID: 204 RVA: 0x00004800 File Offset: 0x00003800
		// (set) Token: 0x060000CD RID: 205 RVA: 0x00004808 File Offset: 0x00003808
		[DSDescription("DSCacheResults")]
		[DefaultValue(true)]
		public bool CacheResults
		{
			get
			{
				return this.cacheResults;
			}
			set
			{
				if (this.directoryVirtualListViewSpecified && value)
				{
					throw new ArgumentException(Res.GetString("DSBadCacheResultsVLV"));
				}
				this.cacheResults = value;
				this.cacheResultsSpecified = true;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000CE RID: 206 RVA: 0x00004833 File Offset: 0x00003833
		// (set) Token: 0x060000CF RID: 207 RVA: 0x0000483B File Offset: 0x0000383B
		[DSDescription("DSClientTimeout")]
		public TimeSpan ClientTimeout
		{
			get
			{
				return this.clientTimeout;
			}
			set
			{
				if (value.TotalSeconds > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("TimespanExceedMax"), "value");
				}
				this.clientTimeout = value;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000D0 RID: 208 RVA: 0x0000486B File Offset: 0x0000386B
		// (set) Token: 0x060000D1 RID: 209 RVA: 0x00004873 File Offset: 0x00003873
		[DSDescription("DSPropertyNamesOnly")]
		[DefaultValue(false)]
		public bool PropertyNamesOnly
		{
			get
			{
				return this.propertyNamesOnly;
			}
			set
			{
				this.propertyNamesOnly = value;
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000D2 RID: 210 RVA: 0x0000487C File Offset: 0x0000387C
		// (set) Token: 0x060000D3 RID: 211 RVA: 0x00004884 File Offset: 0x00003884
		[DefaultValue("(objectClass=*)")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		[SettingsBindable(true)]
		[DSDescription("DSFilter")]
		public string Filter
		{
			get
			{
				return this.filter;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					value = "(objectClass=*)";
				}
				this.filter = value;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000D4 RID: 212 RVA: 0x0000489F File Offset: 0x0000389F
		// (set) Token: 0x060000D5 RID: 213 RVA: 0x000048A7 File Offset: 0x000038A7
		[DefaultValue(0)]
		[DSDescription("DSPageSize")]
		public int PageSize
		{
			get
			{
				return this.pageSize;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("DSBadPageSize"));
				}
				if (this.directorySynchronizationSpecified && value != 0)
				{
					throw new ArgumentException(Res.GetString("DSBadPageSizeDirsync"));
				}
				this.pageSize = value;
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000D6 RID: 214 RVA: 0x000048DF File Offset: 0x000038DF
		[DSDescription("DSPropertiesToLoad")]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", "System.Drawing.Design.UITypeEditor, System.Drawing, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public StringCollection PropertiesToLoad
		{
			get
			{
				if (this.propertiesToLoad == null)
				{
					this.propertiesToLoad = new StringCollection();
				}
				return this.propertiesToLoad;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000D7 RID: 215 RVA: 0x000048FA File Offset: 0x000038FA
		// (set) Token: 0x060000D8 RID: 216 RVA: 0x00004902 File Offset: 0x00003902
		[DSDescription("DSReferralChasing")]
		[DefaultValue(ReferralChasingOption.External)]
		public ReferralChasingOption ReferralChasing
		{
			get
			{
				return this.referralChasing;
			}
			set
			{
				if (value != ReferralChasingOption.None && value != ReferralChasingOption.Subordinate && value != ReferralChasingOption.External && value != ReferralChasingOption.All)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ReferralChasingOption));
				}
				this.referralChasing = value;
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000D9 RID: 217 RVA: 0x00004933 File Offset: 0x00003933
		// (set) Token: 0x060000DA RID: 218 RVA: 0x0000493C File Offset: 0x0000393C
		[SettingsBindable(true)]
		[DefaultValue(SearchScope.Subtree)]
		[DSDescription("DSSearchScope")]
		public SearchScope SearchScope
		{
			get
			{
				return this.scope;
			}
			set
			{
				if (value < SearchScope.Base || value > SearchScope.Subtree)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SearchScope));
				}
				if (this.attributeScopeQuerySpecified && value != SearchScope.Base)
				{
					throw new ArgumentException(Res.GetString("DSBadASQSearchScope"));
				}
				this.scope = value;
				this.scopeSpecified = true;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000DB RID: 219 RVA: 0x00004990 File Offset: 0x00003990
		// (set) Token: 0x060000DC RID: 220 RVA: 0x00004998 File Offset: 0x00003998
		[DSDescription("DSServerPageTimeLimit")]
		public TimeSpan ServerPageTimeLimit
		{
			get
			{
				return this.serverPageTimeLimit;
			}
			set
			{
				if (value.TotalSeconds > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("TimespanExceedMax"), "value");
				}
				this.serverPageTimeLimit = value;
			}
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060000DD RID: 221 RVA: 0x000049C8 File Offset: 0x000039C8
		// (set) Token: 0x060000DE RID: 222 RVA: 0x000049D0 File Offset: 0x000039D0
		[DSDescription("DSServerTimeLimit")]
		public TimeSpan ServerTimeLimit
		{
			get
			{
				return this.serverTimeLimit;
			}
			set
			{
				if (value.TotalSeconds > 2147483647.0)
				{
					throw new ArgumentException(Res.GetString("TimespanExceedMax"), "value");
				}
				this.serverTimeLimit = value;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060000DF RID: 223 RVA: 0x00004A00 File Offset: 0x00003A00
		// (set) Token: 0x060000E0 RID: 224 RVA: 0x00004A08 File Offset: 0x00003A08
		[DSDescription("DSSizeLimit")]
		[DefaultValue(0)]
		public int SizeLimit
		{
			get
			{
				return this.sizeLimit;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentException(Res.GetString("DSBadSizeLimit"));
				}
				this.sizeLimit = value;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060000E1 RID: 225 RVA: 0x00004A28 File Offset: 0x00003A28
		// (set) Token: 0x060000E2 RID: 226 RVA: 0x00004AAE File Offset: 0x00003AAE
		[DSDescription("DSSearchRoot")]
		[DefaultValue(null)]
		public DirectoryEntry SearchRoot
		{
			get
			{
				if (this.searchRoot == null && !base.DesignMode)
				{
					DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://RootDSE", true, null, null, AuthenticationTypes.Secure);
					string text = (string)directoryEntry.Properties["defaultNamingContext"][0];
					directoryEntry.Dispose();
					this.searchRoot = new DirectoryEntry("LDAP://" + text, true, null, null, AuthenticationTypes.Secure);
					this.rootEntryAllocated = true;
					this.assertDefaultNamingContext = "LDAP://" + text;
				}
				return this.searchRoot;
			}
			set
			{
				if (this.rootEntryAllocated)
				{
					this.searchRoot.Dispose();
				}
				this.rootEntryAllocated = false;
				this.assertDefaultNamingContext = null;
				this.searchRoot = value;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060000E3 RID: 227 RVA: 0x00004AD8 File Offset: 0x00003AD8
		// (set) Token: 0x060000E4 RID: 228 RVA: 0x00004AE0 File Offset: 0x00003AE0
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
		[TypeConverter(typeof(ExpandableObjectConverter))]
		[DSDescription("DSSort")]
		public SortOption Sort
		{
			get
			{
				return this.sort;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.sort = value;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060000E5 RID: 229 RVA: 0x00004AF7 File Offset: 0x00003AF7
		// (set) Token: 0x060000E6 RID: 230 RVA: 0x00004AFF File Offset: 0x00003AFF
		[DSDescription("DSAsynchronous")]
		[ComVisible(false)]
		[DefaultValue(false)]
		public bool Asynchronous
		{
			get
			{
				return this.asynchronous;
			}
			set
			{
				this.asynchronous = value;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060000E7 RID: 231 RVA: 0x00004B08 File Offset: 0x00003B08
		// (set) Token: 0x060000E8 RID: 232 RVA: 0x00004B10 File Offset: 0x00003B10
		[ComVisible(false)]
		[DefaultValue(false)]
		[DSDescription("DSTombstone")]
		public bool Tombstone
		{
			get
			{
				return this.tombstone;
			}
			set
			{
				this.tombstone = value;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060000E9 RID: 233 RVA: 0x00004B19 File Offset: 0x00003B19
		// (set) Token: 0x060000EA RID: 234 RVA: 0x00004B24 File Offset: 0x00003B24
		[DefaultValue("")]
		[ComVisible(false)]
		[DSDescription("DSAttributeQuery")]
		[TypeConverter("System.Diagnostics.Design.StringValueConverter, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
		public string AttributeScopeQuery
		{
			get
			{
				return this.attributeScopeQuery;
			}
			set
			{
				if (value == null)
				{
					value = "";
				}
				if (value.Length != 0)
				{
					if (this.scopeSpecified && this.SearchScope != SearchScope.Base)
					{
						throw new ArgumentException(Res.GetString("DSBadASQSearchScope"));
					}
					this.scope = SearchScope.Base;
					this.attributeScopeQuerySpecified = true;
				}
				else
				{
					this.attributeScopeQuerySpecified = false;
				}
				this.attributeScopeQuery = value;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060000EB RID: 235 RVA: 0x00004B81 File Offset: 0x00003B81
		// (set) Token: 0x060000EC RID: 236 RVA: 0x00004B89 File Offset: 0x00003B89
		[ComVisible(false)]
		[DSDescription("DSDerefAlias")]
		[DefaultValue(DereferenceAlias.Never)]
		public DereferenceAlias DerefAlias
		{
			get
			{
				return this.derefAlias;
			}
			set
			{
				if (value < DereferenceAlias.Never || value > DereferenceAlias.Always)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DereferenceAlias));
				}
				this.derefAlias = value;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060000ED RID: 237 RVA: 0x00004BB0 File Offset: 0x00003BB0
		// (set) Token: 0x060000EE RID: 238 RVA: 0x00004BB8 File Offset: 0x00003BB8
		[DSDescription("DSSecurityMasks")]
		[ComVisible(false)]
		[DefaultValue(SecurityMasks.None)]
		public SecurityMasks SecurityMasks
		{
			get
			{
				return this.securityMask;
			}
			set
			{
				if (value > (SecurityMasks.Owner | SecurityMasks.Group | SecurityMasks.Dacl | SecurityMasks.Sacl))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(SecurityMasks));
				}
				this.securityMask = value;
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060000EF RID: 239 RVA: 0x00004BDC File Offset: 0x00003BDC
		// (set) Token: 0x060000F0 RID: 240 RVA: 0x00004BE4 File Offset: 0x00003BE4
		[DefaultValue(ExtendedDN.None)]
		[DSDescription("DSExtendedDn")]
		[ComVisible(false)]
		public ExtendedDN ExtendedDN
		{
			get
			{
				return this.extendedDN;
			}
			set
			{
				if (value < ExtendedDN.None || value > ExtendedDN.Standard)
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(ExtendedDN));
				}
				this.extendedDN = value;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060000F1 RID: 241 RVA: 0x00004C0B File Offset: 0x00003C0B
		// (set) Token: 0x060000F2 RID: 242 RVA: 0x00004C39 File Offset: 0x00003C39
		[DefaultValue(null)]
		[ComVisible(false)]
		[DSDescription("DSDirectorySynchronization")]
		[Browsable(false)]
		public DirectorySynchronization DirectorySynchronization
		{
			get
			{
				if (this.directorySynchronizationSpecified && this.searchResult != null)
				{
					this.sync.ResetDirectorySynchronizationCookie(this.searchResult.DirsyncCookie);
				}
				return this.sync;
			}
			set
			{
				if (value != null)
				{
					if (this.PageSize != 0)
					{
						throw new ArgumentException(Res.GetString("DSBadPageSizeDirsync"));
					}
					this.directorySynchronizationSpecified = true;
				}
				else
				{
					this.directorySynchronizationSpecified = false;
				}
				this.sync = value;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060000F3 RID: 243 RVA: 0x00004C70 File Offset: 0x00003C70
		// (set) Token: 0x060000F4 RID: 244 RVA: 0x00004D24 File Offset: 0x00003D24
		[DSDescription("DSVirtualListView")]
		[Browsable(false)]
		[ComVisible(false)]
		[DefaultValue(null)]
		public DirectoryVirtualListView VirtualListView
		{
			get
			{
				if (this.directoryVirtualListViewSpecified && this.searchResult != null)
				{
					DirectoryVirtualListView vlvresponse = this.searchResult.VLVResponse;
					this.vlv.Offset = vlvresponse.Offset;
					this.vlv.ApproximateTotal = vlvresponse.ApproximateTotal;
					this.vlv.DirectoryVirtualListViewContext = vlvresponse.DirectoryVirtualListViewContext;
					if (this.vlv.ApproximateTotal != 0)
					{
						this.vlv.TargetPercentage = (int)((double)this.vlv.Offset / (double)this.vlv.ApproximateTotal * 100.0);
					}
					else
					{
						this.vlv.TargetPercentage = 0;
					}
				}
				return this.vlv;
			}
			set
			{
				if (value != null)
				{
					if (this.cacheResultsSpecified && this.CacheResults)
					{
						throw new ArgumentException(Res.GetString("DSBadCacheResultsVLV"));
					}
					this.directoryVirtualListViewSpecified = true;
					this.cacheResults = false;
				}
				else
				{
					this.directoryVirtualListViewSpecified = false;
				}
				this.vlv = value;
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00004D74 File Offset: 0x00003D74
		public SearchResult FindOne()
		{
			SearchResult searchResult = null;
			SearchResultCollection searchResultCollection = this.FindAll(false);
			try
			{
				using (IEnumerator enumerator = searchResultCollection.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						SearchResult searchResult2 = (SearchResult)enumerator.Current;
						if (this.directorySynchronizationSpecified)
						{
							DirectorySynchronization directorySynchronization = this.DirectorySynchronization;
						}
						if (this.directoryVirtualListViewSpecified)
						{
							DirectoryVirtualListView virtualListView = this.VirtualListView;
						}
						searchResult = searchResult2;
					}
				}
			}
			finally
			{
				this.searchResult = null;
				searchResultCollection.Dispose();
			}
			return searchResult;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00004E10 File Offset: 0x00003E10
		public SearchResultCollection FindAll()
		{
			return this.FindAll(true);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x00004E1C File Offset: 0x00003E1C
		private SearchResultCollection FindAll(bool findMoreThanOne)
		{
			this.searchResult = null;
			DirectoryEntry directoryEntry;
			if (this.assertDefaultNamingContext == null)
			{
				directoryEntry = this.SearchRoot.CloneBrowsable();
			}
			else
			{
				directoryEntry = this.SearchRoot.CloneBrowsable();
			}
			global::System.DirectoryServices.Interop.UnsafeNativeMethods.IAds adsObject = directoryEntry.AdsObject;
			if (!(adsObject is global::System.DirectoryServices.Interop.UnsafeNativeMethods.IDirectorySearch))
			{
				throw new NotSupportedException(Res.GetString("DSSearchUnsupported", new object[] { this.SearchRoot.Path }));
			}
			if (this.directoryVirtualListViewSpecified)
			{
				this.SearchRoot.Bind(true);
			}
			global::System.DirectoryServices.Interop.UnsafeNativeMethods.IDirectorySearch directorySearch = (global::System.DirectoryServices.Interop.UnsafeNativeMethods.IDirectorySearch)adsObject;
			this.SetSearchPreferences(directorySearch, findMoreThanOne);
			string[] array = null;
			if (this.PropertiesToLoad.Count > 0)
			{
				if (!this.PropertiesToLoad.Contains("ADsPath"))
				{
					this.PropertiesToLoad.Add("ADsPath");
				}
				array = new string[this.PropertiesToLoad.Count];
				this.PropertiesToLoad.CopyTo(array, 0);
			}
			IntPtr intPtr;
			if (array != null)
			{
				directorySearch.ExecuteSearch(this.Filter, array, array.Length, out intPtr);
			}
			else
			{
				directorySearch.ExecuteSearch(this.Filter, null, -1, out intPtr);
				array = new string[0];
			}
			SearchResultCollection searchResultCollection = new SearchResultCollection(directoryEntry, intPtr, array, this);
			this.searchResult = searchResultCollection;
			return searchResultCollection;
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x00004F44 File Offset: 0x00003F44
		private unsafe void SetSearchPreferences(global::System.DirectoryServices.Interop.UnsafeNativeMethods.IDirectorySearch adsSearch, bool findMoreThanOne)
		{
			ArrayList arrayList = new ArrayList();
			AdsSearchPreferenceInfo adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
			{
				dwSearchPref = 5,
				vValue = new AdsValueHelper((int)this.SearchScope).GetStruct()
			};
			arrayList.Add(adsSearchPreferenceInfo);
			if (this.sizeLimit != 0 || !findMoreThanOne)
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 2,
					vValue = new AdsValueHelper(findMoreThanOne ? this.SizeLimit : 1).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			if (this.ServerTimeLimit >= new TimeSpan(0L))
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 3,
					vValue = new AdsValueHelper((int)this.ServerTimeLimit.TotalSeconds).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
			{
				dwSearchPref = 4,
				vValue = new AdsValueHelper(this.PropertyNamesOnly).GetStruct()
			};
			arrayList.Add(adsSearchPreferenceInfo);
			if (this.ClientTimeout >= new TimeSpan(0L))
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 6,
					vValue = new AdsValueHelper((int)this.ClientTimeout.TotalSeconds).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			if (this.PageSize != 0)
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 7,
					vValue = new AdsValueHelper(this.PageSize).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			if (this.ServerPageTimeLimit >= new TimeSpan(0L))
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 8,
					vValue = new AdsValueHelper((int)this.ServerPageTimeLimit.TotalSeconds).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
			{
				dwSearchPref = 9,
				vValue = new AdsValueHelper((int)this.ReferralChasing).GetStruct()
			};
			arrayList.Add(adsSearchPreferenceInfo);
			if (this.Asynchronous)
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 0,
					vValue = new AdsValueHelper(this.Asynchronous).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			if (this.Tombstone)
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 13,
					vValue = new AdsValueHelper(this.Tombstone).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			if (this.attributeScopeQuerySpecified)
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 15,
					vValue = new AdsValueHelper(this.AttributeScopeQuery, AdsType.ADSTYPE_CASE_IGNORE_STRING).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			if (this.DerefAlias != DereferenceAlias.Never)
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 1,
					vValue = new AdsValueHelper((int)this.DerefAlias).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			if (this.SecurityMasks != SecurityMasks.None)
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 16,
					vValue = new AdsValueHelper((int)this.SecurityMasks).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			if (this.ExtendedDN != ExtendedDN.None)
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 18,
					vValue = new AdsValueHelper((int)this.ExtendedDN).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
			}
			if (this.directorySynchronizationSpecified)
			{
				adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
				{
					dwSearchPref = 12,
					vValue = new AdsValueHelper(this.DirectorySynchronization.GetDirectorySynchronizationCookie(), AdsType.ADSTYPE_PROV_SPECIFIC).GetStruct()
				};
				arrayList.Add(adsSearchPreferenceInfo);
				if (this.DirectorySynchronization.Option != DirectorySynchronizationOptions.None)
				{
					adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
					{
						dwSearchPref = 17,
						vValue = new AdsValueHelper((int)this.DirectorySynchronization.Option).GetStruct()
					};
					arrayList.Add(adsSearchPreferenceInfo);
				}
			}
			IntPtr intPtr = (IntPtr)0;
			IntPtr intPtr2 = (IntPtr)0;
			IntPtr intPtr3 = (IntPtr)0;
			try
			{
				if (this.Sort.PropertyName != null && this.Sort.PropertyName.Length > 0)
				{
					adsSearchPreferenceInfo = default(AdsSearchPreferenceInfo);
					adsSearchPreferenceInfo.dwSearchPref = 10;
					AdsSortKey adsSortKey = default(AdsSortKey);
					adsSortKey.pszAttrType = Marshal.StringToCoTaskMemUni(this.Sort.PropertyName);
					intPtr = adsSortKey.pszAttrType;
					adsSortKey.pszReserved = (IntPtr)0;
					adsSortKey.fReverseOrder = ((this.Sort.Direction == SortDirection.Descending) ? (-1) : 0);
					byte[] array = new byte[Marshal.SizeOf(adsSortKey)];
					Marshal.Copy((IntPtr)((void*)(&adsSortKey)), array, 0, array.Length);
					adsSearchPreferenceInfo.vValue = new AdsValueHelper(array, AdsType.ADSTYPE_PROV_SPECIFIC).GetStruct();
					arrayList.Add(adsSearchPreferenceInfo);
				}
				if (this.directoryVirtualListViewSpecified)
				{
					adsSearchPreferenceInfo = default(AdsSearchPreferenceInfo);
					adsSearchPreferenceInfo.dwSearchPref = 14;
					AdsVLV adsVLV = new AdsVLV();
					adsVLV.beforeCount = this.vlv.BeforeCount;
					adsVLV.afterCount = this.vlv.AfterCount;
					adsVLV.offset = this.vlv.Offset;
					if (this.vlv.Target.Length != 0)
					{
						adsVLV.target = Marshal.StringToCoTaskMemUni(this.vlv.Target);
					}
					else
					{
						adsVLV.target = IntPtr.Zero;
					}
					intPtr2 = adsVLV.target;
					if (this.vlv.DirectoryVirtualListViewContext == null)
					{
						adsVLV.contextIDlength = 0;
						adsVLV.contextID = (IntPtr)0;
					}
					else
					{
						adsVLV.contextIDlength = this.vlv.DirectoryVirtualListViewContext.context.Length;
						adsVLV.contextID = Marshal.AllocCoTaskMem(adsVLV.contextIDlength);
						intPtr3 = adsVLV.contextID;
						Marshal.Copy(this.vlv.DirectoryVirtualListViewContext.context, 0, adsVLV.contextID, adsVLV.contextIDlength);
					}
					IntPtr intPtr4 = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(AdsVLV)));
					byte[] array2 = new byte[Marshal.SizeOf(adsVLV)];
					try
					{
						Marshal.StructureToPtr(adsVLV, intPtr4, false);
						Marshal.Copy(intPtr4, array2, 0, array2.Length);
					}
					finally
					{
						Marshal.FreeHGlobal(intPtr4);
					}
					adsSearchPreferenceInfo.vValue = new AdsValueHelper(array2, AdsType.ADSTYPE_PROV_SPECIFIC).GetStruct();
					arrayList.Add(adsSearchPreferenceInfo);
				}
				if (this.cacheResultsSpecified)
				{
					adsSearchPreferenceInfo = new AdsSearchPreferenceInfo
					{
						dwSearchPref = 11,
						vValue = new AdsValueHelper(this.CacheResults).GetStruct()
					};
					arrayList.Add(adsSearchPreferenceInfo);
				}
				AdsSearchPreferenceInfo[] array3 = new AdsSearchPreferenceInfo[arrayList.Count];
				for (int i = 0; i < arrayList.Count; i++)
				{
					array3[i] = (AdsSearchPreferenceInfo)arrayList[i];
				}
				DirectorySearcher.DoSetSearchPrefs(adsSearch, array3);
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Marshal.FreeCoTaskMem(intPtr);
				}
				if (intPtr2 != (IntPtr)0)
				{
					Marshal.FreeCoTaskMem(intPtr2);
				}
				if (intPtr3 != (IntPtr)0)
				{
					Marshal.FreeCoTaskMem(intPtr3);
				}
			}
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000570C File Offset: 0x0000470C
		private static void DoSetSearchPrefs(global::System.DirectoryServices.Interop.UnsafeNativeMethods.IDirectorySearch adsSearch, AdsSearchPreferenceInfo[] prefs)
		{
			int num = Marshal.SizeOf(typeof(AdsSearchPreferenceInfo));
			IntPtr intPtr = Marshal.AllocHGlobal((IntPtr)(num * prefs.Length));
			try
			{
				IntPtr intPtr2 = intPtr;
				for (int i = 0; i < prefs.Length; i++)
				{
					Marshal.StructureToPtr(prefs[i], intPtr2, false);
					intPtr2 = Utils.AddToIntPtr(intPtr2, num);
				}
				adsSearch.SetSearchPreference(intPtr, prefs.Length);
				intPtr2 = intPtr;
				for (int j = 0; j < prefs.Length; j++)
				{
					int num2 = Marshal.ReadInt32(intPtr2, 32);
					if (num2 != 0)
					{
						int dwSearchPref = prefs[j].dwSearchPref;
						string text = "";
						switch (dwSearchPref)
						{
						case 0:
							text = "Asynchronous";
							break;
						case 1:
							text = "DerefAlias";
							break;
						case 2:
							text = "SizeLimit";
							break;
						case 3:
							text = "ServerTimeLimit";
							break;
						case 4:
							text = "PropertyNamesOnly";
							break;
						case 5:
							text = "SearchScope";
							break;
						case 6:
							text = "ClientTimeout";
							break;
						case 7:
							text = "PageSize";
							break;
						case 8:
							text = "ServerPageTimeLimit";
							break;
						case 9:
							text = "ReferralChasing";
							break;
						case 10:
							text = "Sort";
							break;
						case 11:
							text = "CacheResults";
							break;
						case 12:
							text = "DirectorySynchronization";
							break;
						case 13:
							text = "Tombstone";
							break;
						case 14:
							text = "VirtualListView";
							break;
						case 15:
							text = "AttributeScopeQuery";
							break;
						case 16:
							text = "SecurityMasks";
							break;
						case 17:
							text = "DirectorySynchronizationFlag";
							break;
						case 18:
							text = "ExtendedDn";
							break;
						}
						throw new InvalidOperationException(Res.GetString("DSSearchPreferencesNotAccepted", new object[] { text }));
					}
					intPtr2 = Utils.AddToIntPtr(intPtr2, num);
				}
			}
			finally
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x04000181 RID: 385
		private const string defaultFilter = "(objectClass=*)";

		// Token: 0x04000182 RID: 386
		private DirectoryEntry searchRoot;

		// Token: 0x04000183 RID: 387
		private string filter = "(objectClass=*)";

		// Token: 0x04000184 RID: 388
		private StringCollection propertiesToLoad;

		// Token: 0x04000185 RID: 389
		private bool disposed;

		// Token: 0x04000186 RID: 390
		private static readonly TimeSpan minusOneSecond = new TimeSpan(0, 0, -1);

		// Token: 0x04000187 RID: 391
		private SearchScope scope = SearchScope.Subtree;

		// Token: 0x04000188 RID: 392
		private bool scopeSpecified;

		// Token: 0x04000189 RID: 393
		private int sizeLimit;

		// Token: 0x0400018A RID: 394
		private TimeSpan serverTimeLimit = DirectorySearcher.minusOneSecond;

		// Token: 0x0400018B RID: 395
		private bool propertyNamesOnly;

		// Token: 0x0400018C RID: 396
		private TimeSpan clientTimeout = DirectorySearcher.minusOneSecond;

		// Token: 0x0400018D RID: 397
		private int pageSize;

		// Token: 0x0400018E RID: 398
		private TimeSpan serverPageTimeLimit = DirectorySearcher.minusOneSecond;

		// Token: 0x0400018F RID: 399
		private ReferralChasingOption referralChasing = ReferralChasingOption.External;

		// Token: 0x04000190 RID: 400
		private SortOption sort = new SortOption();

		// Token: 0x04000191 RID: 401
		private bool cacheResults = true;

		// Token: 0x04000192 RID: 402
		private bool cacheResultsSpecified;

		// Token: 0x04000193 RID: 403
		private bool rootEntryAllocated;

		// Token: 0x04000194 RID: 404
		private string assertDefaultNamingContext;

		// Token: 0x04000195 RID: 405
		private bool asynchronous;

		// Token: 0x04000196 RID: 406
		private bool tombstone;

		// Token: 0x04000197 RID: 407
		private string attributeScopeQuery = "";

		// Token: 0x04000198 RID: 408
		private bool attributeScopeQuerySpecified;

		// Token: 0x04000199 RID: 409
		private DereferenceAlias derefAlias;

		// Token: 0x0400019A RID: 410
		private SecurityMasks securityMask;

		// Token: 0x0400019B RID: 411
		private ExtendedDN extendedDN = ExtendedDN.None;

		// Token: 0x0400019C RID: 412
		private DirectorySynchronization sync;

		// Token: 0x0400019D RID: 413
		internal bool directorySynchronizationSpecified;

		// Token: 0x0400019E RID: 414
		private DirectoryVirtualListView vlv;

		// Token: 0x0400019F RID: 415
		internal bool directoryVirtualListViewSpecified;

		// Token: 0x040001A0 RID: 416
		internal SearchResultCollection searchResult;
	}
}
