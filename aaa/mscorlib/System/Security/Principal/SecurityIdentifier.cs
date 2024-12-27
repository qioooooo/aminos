using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x02000930 RID: 2352
	[ComVisible(false)]
	public sealed class SecurityIdentifier : IdentityReference, IComparable<SecurityIdentifier>
	{
		// Token: 0x06005542 RID: 21826 RVA: 0x00136110 File Offset: 0x00135110
		private void CreateFromParts(IdentifierAuthority identifierAuthority, int[] subAuthorities)
		{
			if (subAuthorities == null)
			{
				throw new ArgumentNullException("subAuthorities");
			}
			if (subAuthorities.Length > (int)SecurityIdentifier.MaxSubAuthorities)
			{
				throw new ArgumentOutOfRangeException("subAuthorities.Length", subAuthorities.Length, Environment.GetResourceString("IdentityReference_InvalidNumberOfSubauthorities", new object[] { SecurityIdentifier.MaxSubAuthorities }));
			}
			if (identifierAuthority < IdentifierAuthority.NullAuthority || identifierAuthority > (IdentifierAuthority)SecurityIdentifier.MaxIdentifierAuthority)
			{
				throw new ArgumentOutOfRangeException("identifierAuthority", identifierAuthority, Environment.GetResourceString("IdentityReference_IdentifierAuthorityTooLarge"));
			}
			this._IdentifierAuthority = identifierAuthority;
			this._SubAuthorities = new int[subAuthorities.Length];
			subAuthorities.CopyTo(this._SubAuthorities, 0);
			this._BinaryForm = new byte[8 + 4 * this.SubAuthorityCount];
			this._BinaryForm[0] = SecurityIdentifier.Revision;
			this._BinaryForm[1] = (byte)this.SubAuthorityCount;
			byte b;
			for (b = 0; b < 6; b += 1)
			{
				this._BinaryForm[(int)(2 + b)] = (byte)((this._IdentifierAuthority >> (int)(((5 - b) * 8) & 63)) & (IdentifierAuthority)255L);
			}
			b = 0;
			while ((int)b < this.SubAuthorityCount)
			{
				for (byte b2 = 0; b2 < 4; b2 += 1)
				{
					this._BinaryForm[(int)(8 + 4 * b + b2)] = (byte)((ulong)((long)this._SubAuthorities[(int)b]) >> (int)(b2 * 8));
				}
				b += 1;
			}
		}

		// Token: 0x06005543 RID: 21827 RVA: 0x00136250 File Offset: 0x00135250
		private void CreateFromBinaryForm(byte[] binaryForm, int offset)
		{
			if (binaryForm == null)
			{
				throw new ArgumentNullException("binaryForm");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset", offset, Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
			}
			if (binaryForm.Length - offset < SecurityIdentifier.MinBinaryLength)
			{
				throw new ArgumentOutOfRangeException("binaryForm", Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"));
			}
			if (binaryForm[offset] != SecurityIdentifier.Revision)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_InvalidSidRevision"), "binaryForm");
			}
			if (binaryForm[offset + 1] > SecurityIdentifier.MaxSubAuthorities)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_InvalidNumberOfSubauthorities", new object[] { SecurityIdentifier.MaxSubAuthorities }), "binaryForm");
			}
			int num = (int)(8 + 4 * binaryForm[offset + 1]);
			if (binaryForm.Length - offset < num)
			{
				throw new ArgumentException(Environment.GetResourceString("ArgumentOutOfRange_ArrayTooSmall"), "binaryForm");
			}
			IdentifierAuthority identifierAuthority = (IdentifierAuthority)(((ulong)binaryForm[offset + 2] << 40) + ((ulong)binaryForm[offset + 3] << 32) + ((ulong)binaryForm[offset + 4] << 24) + ((ulong)binaryForm[offset + 5] << 16) + ((ulong)binaryForm[offset + 6] << 8) + (ulong)binaryForm[offset + 7]);
			int[] array = new int[(int)binaryForm[offset + 1]];
			for (byte b = 0; b < binaryForm[offset + 1]; b += 1)
			{
				array[(int)b] = (int)binaryForm[offset + 8 + (int)(4 * b)] + ((int)binaryForm[offset + 8 + (int)(4 * b) + 1] << 8) + ((int)binaryForm[offset + 8 + (int)(4 * b) + 2] << 16) + ((int)binaryForm[offset + 8 + (int)(4 * b) + 3] << 24);
			}
			this.CreateFromParts(identifierAuthority, array);
		}

		// Token: 0x06005544 RID: 21828 RVA: 0x001363C0 File Offset: 0x001353C0
		public SecurityIdentifier(string sddlForm)
		{
			if (sddlForm == null)
			{
				throw new ArgumentNullException("sddlForm");
			}
			byte[] array;
			int num = Win32.CreateSidFromString(sddlForm, out array);
			if (num == 1337)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"), "sddlForm");
			}
			if (num == 8)
			{
				throw new OutOfMemoryException();
			}
			if (num != 0)
			{
				throw new SystemException(Win32Native.GetMessage(num));
			}
			this.CreateFromBinaryForm(array, 0);
		}

		// Token: 0x06005545 RID: 21829 RVA: 0x00136428 File Offset: 0x00135428
		public SecurityIdentifier(byte[] binaryForm, int offset)
		{
			this.CreateFromBinaryForm(binaryForm, offset);
		}

		// Token: 0x06005546 RID: 21830 RVA: 0x00136438 File Offset: 0x00135438
		[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
		public SecurityIdentifier(IntPtr binaryForm)
			: this(binaryForm, true)
		{
		}

		// Token: 0x06005547 RID: 21831 RVA: 0x00136442 File Offset: 0x00135442
		internal SecurityIdentifier(IntPtr binaryForm, bool noDemand)
			: this(Win32.ConvertIntPtrSidToByteArraySid(binaryForm), 0)
		{
		}

		// Token: 0x06005548 RID: 21832 RVA: 0x00136454 File Offset: 0x00135454
		public SecurityIdentifier(WellKnownSidType sidType, SecurityIdentifier domainSid)
		{
			if (!Win32.WellKnownSidApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_RequiresW2kSP3"));
			}
			if (sidType == WellKnownSidType.LogonIdsSid)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_CannotCreateLogonIdsSid"), "sidType");
			}
			if (sidType < WellKnownSidType.NullSid || sidType > WellKnownSidType.WinBuiltinTerminalServerLicenseServersSid)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidValue"), "sidType");
			}
			if (sidType >= WellKnownSidType.AccountAdministratorSid && sidType <= WellKnownSidType.AccountRasAndIasServersSid)
			{
				if (domainSid == null)
				{
					throw new ArgumentNullException("domainSid", Environment.GetResourceString("IdentityReference_DomainSidRequired", new object[] { sidType }));
				}
				SecurityIdentifier securityIdentifier;
				int windowsAccountDomainSid = Win32.GetWindowsAccountDomainSid(domainSid, out securityIdentifier);
				if (windowsAccountDomainSid == 122)
				{
					throw new OutOfMemoryException();
				}
				if (windowsAccountDomainSid == 1257)
				{
					throw new ArgumentException(Environment.GetResourceString("IdentityReference_NotAWindowsDomain"), "domainSid");
				}
				if (windowsAccountDomainSid != 0)
				{
					throw new SystemException(Win32Native.GetMessage(windowsAccountDomainSid));
				}
				if (securityIdentifier != domainSid)
				{
					throw new ArgumentException(Environment.GetResourceString("IdentityReference_NotAWindowsDomain"), "domainSid");
				}
			}
			byte[] array;
			int num = Win32.CreateWellKnownSid(sidType, domainSid, out array);
			if (num == 87)
			{
				throw new ArgumentException(Win32Native.GetMessage(num), "sidType/domainSid");
			}
			if (num != 0)
			{
				throw new SystemException(Win32Native.GetMessage(num));
			}
			this.CreateFromBinaryForm(array, 0);
		}

		// Token: 0x06005549 RID: 21833 RVA: 0x00136590 File Offset: 0x00135590
		internal SecurityIdentifier(SecurityIdentifier domainSid, uint rid)
		{
			int[] array = new int[domainSid.SubAuthorityCount + 1];
			int i;
			for (i = 0; i < domainSid.SubAuthorityCount; i++)
			{
				array[i] = domainSid.GetSubAuthority(i);
			}
			array[i] = (int)rid;
			this.CreateFromParts(domainSid.IdentifierAuthority, array);
		}

		// Token: 0x0600554A RID: 21834 RVA: 0x001365DD File Offset: 0x001355DD
		internal SecurityIdentifier(IdentifierAuthority identifierAuthority, int[] subAuthorities)
		{
			this.CreateFromParts(identifierAuthority, subAuthorities);
		}

		// Token: 0x17000ED8 RID: 3800
		// (get) Token: 0x0600554B RID: 21835 RVA: 0x001365ED File Offset: 0x001355ED
		internal static byte Revision
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x0600554C RID: 21836 RVA: 0x001365F0 File Offset: 0x001355F0
		internal byte[] BinaryForm
		{
			get
			{
				return this._BinaryForm;
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x0600554D RID: 21837 RVA: 0x001365F8 File Offset: 0x001355F8
		internal IdentifierAuthority IdentifierAuthority
		{
			get
			{
				return this._IdentifierAuthority;
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x0600554E RID: 21838 RVA: 0x00136600 File Offset: 0x00135600
		internal int SubAuthorityCount
		{
			get
			{
				return this._SubAuthorities.Length;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x0600554F RID: 21839 RVA: 0x0013660A File Offset: 0x0013560A
		public int BinaryLength
		{
			get
			{
				return this._BinaryForm.Length;
			}
		}

		// Token: 0x17000EDD RID: 3805
		// (get) Token: 0x06005550 RID: 21840 RVA: 0x00136614 File Offset: 0x00135614
		public SecurityIdentifier AccountDomainSid
		{
			get
			{
				if (!this._AccountDomainSidInitialized)
				{
					this._AccountDomainSid = this.GetAccountDomainSid();
					this._AccountDomainSidInitialized = true;
				}
				return this._AccountDomainSid;
			}
		}

		// Token: 0x06005551 RID: 21841 RVA: 0x00136638 File Offset: 0x00135638
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			SecurityIdentifier securityIdentifier = o as SecurityIdentifier;
			return !(securityIdentifier == null) && this == securityIdentifier;
		}

		// Token: 0x06005552 RID: 21842 RVA: 0x00136663 File Offset: 0x00135663
		public bool Equals(SecurityIdentifier sid)
		{
			return !(sid == null) && this == sid;
		}

		// Token: 0x06005553 RID: 21843 RVA: 0x00136678 File Offset: 0x00135678
		public override int GetHashCode()
		{
			int num = ((long)this.IdentifierAuthority).GetHashCode();
			for (int i = 0; i < this.SubAuthorityCount; i++)
			{
				num ^= this.GetSubAuthority(i);
			}
			return num;
		}

		// Token: 0x06005554 RID: 21844 RVA: 0x001366B0 File Offset: 0x001356B0
		public override string ToString()
		{
			if (this._SddlForm == null)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("S-1-{0}", (long)this._IdentifierAuthority);
				for (int i = 0; i < this.SubAuthorityCount; i++)
				{
					stringBuilder.AppendFormat("-{0}", (uint)this._SubAuthorities[i]);
				}
				this._SddlForm = stringBuilder.ToString();
			}
			return this._SddlForm;
		}

		// Token: 0x17000EDE RID: 3806
		// (get) Token: 0x06005555 RID: 21845 RVA: 0x0013671E File Offset: 0x0013571E
		public override string Value
		{
			get
			{
				return this.ToString().ToUpper(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x06005556 RID: 21846 RVA: 0x00136730 File Offset: 0x00135730
		internal static bool IsValidTargetTypeStatic(Type targetType)
		{
			return targetType == typeof(NTAccount) || targetType == typeof(SecurityIdentifier);
		}

		// Token: 0x06005557 RID: 21847 RVA: 0x00136751 File Offset: 0x00135751
		public override bool IsValidTargetType(Type targetType)
		{
			return SecurityIdentifier.IsValidTargetTypeStatic(targetType);
		}

		// Token: 0x06005558 RID: 21848 RVA: 0x0013675C File Offset: 0x0013575C
		internal SecurityIdentifier GetAccountDomainSid()
		{
			SecurityIdentifier securityIdentifier;
			int windowsAccountDomainSid = Win32.GetWindowsAccountDomainSid(this, out securityIdentifier);
			if (windowsAccountDomainSid == 122)
			{
				throw new OutOfMemoryException();
			}
			if (windowsAccountDomainSid == 1257)
			{
				securityIdentifier = null;
			}
			else if (windowsAccountDomainSid != 0)
			{
				throw new SystemException(Win32Native.GetMessage(windowsAccountDomainSid));
			}
			return securityIdentifier;
		}

		// Token: 0x06005559 RID: 21849 RVA: 0x00136799 File Offset: 0x00135799
		public bool IsAccountSid()
		{
			if (!this._AccountDomainSidInitialized)
			{
				this._AccountDomainSid = this.GetAccountDomainSid();
				this._AccountDomainSidInitialized = true;
			}
			return !(this._AccountDomainSid == null);
		}

		// Token: 0x0600555A RID: 21850 RVA: 0x001367C8 File Offset: 0x001357C8
		public override IdentityReference Translate(Type targetType)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (targetType == typeof(SecurityIdentifier))
			{
				return this;
			}
			if (targetType == typeof(NTAccount))
			{
				IdentityReferenceCollection identityReferenceCollection = SecurityIdentifier.Translate(new IdentityReferenceCollection(1) { this }, targetType, true);
				return identityReferenceCollection[0];
			}
			throw new ArgumentException(Environment.GetResourceString("IdentityReference_MustBeIdentityReference"), "targetType");
		}

		// Token: 0x0600555B RID: 21851 RVA: 0x00136834 File Offset: 0x00135834
		public static bool operator ==(SecurityIdentifier left, SecurityIdentifier right)
		{
			return (left == null && right == null) || (left != null && right != null && left.CompareTo(right) == 0);
		}

		// Token: 0x0600555C RID: 21852 RVA: 0x0013685F File Offset: 0x0013585F
		public static bool operator !=(SecurityIdentifier left, SecurityIdentifier right)
		{
			return !(left == right);
		}

		// Token: 0x0600555D RID: 21853 RVA: 0x0013686C File Offset: 0x0013586C
		public int CompareTo(SecurityIdentifier sid)
		{
			if (sid == null)
			{
				throw new ArgumentNullException("sid");
			}
			if (this.IdentifierAuthority < sid.IdentifierAuthority)
			{
				return -1;
			}
			if (this.IdentifierAuthority > sid.IdentifierAuthority)
			{
				return 1;
			}
			if (this.SubAuthorityCount < sid.SubAuthorityCount)
			{
				return -1;
			}
			if (this.SubAuthorityCount > sid.SubAuthorityCount)
			{
				return 1;
			}
			for (int i = 0; i < this.SubAuthorityCount; i++)
			{
				int num = this.GetSubAuthority(i) - sid.GetSubAuthority(i);
				if (num != 0)
				{
					return num;
				}
			}
			return 0;
		}

		// Token: 0x0600555E RID: 21854 RVA: 0x001368F4 File Offset: 0x001358F4
		internal int GetSubAuthority(int index)
		{
			return this._SubAuthorities[index];
		}

		// Token: 0x0600555F RID: 21855 RVA: 0x001368FE File Offset: 0x001358FE
		public bool IsWellKnown(WellKnownSidType type)
		{
			return Win32.IsWellKnownSid(this, type);
		}

		// Token: 0x06005560 RID: 21856 RVA: 0x00136907 File Offset: 0x00135907
		public void GetBinaryForm(byte[] binaryForm, int offset)
		{
			this._BinaryForm.CopyTo(binaryForm, offset);
		}

		// Token: 0x06005561 RID: 21857 RVA: 0x00136916 File Offset: 0x00135916
		public bool IsEqualDomainSid(SecurityIdentifier sid)
		{
			return Win32.IsEqualDomainSid(this, sid);
		}

		// Token: 0x06005562 RID: 21858 RVA: 0x00136920 File Offset: 0x00135920
		private static IdentityReferenceCollection TranslateToNTAccounts(IdentityReferenceCollection sourceSids, out bool someFailed)
		{
			if (!Win32.LsaApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			IntPtr[] array = new IntPtr[sourceSids.Count];
			GCHandle[] array2 = new GCHandle[sourceSids.Count];
			SafeLsaPolicyHandle safeLsaPolicyHandle = SafeLsaPolicyHandle.InvalidHandle;
			SafeLsaMemoryHandle invalidHandle = SafeLsaMemoryHandle.InvalidHandle;
			SafeLsaMemoryHandle invalidHandle2 = SafeLsaMemoryHandle.InvalidHandle;
			int i = 0;
			if (sourceSids == null)
			{
				throw new ArgumentNullException("sourceSids");
			}
			if (sourceSids.Count == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EmptyCollection"), "sourceSids");
			}
			IdentityReferenceCollection identityReferenceCollection2;
			try
			{
				foreach (IdentityReference identityReference in sourceSids)
				{
					SecurityIdentifier securityIdentifier = identityReference as SecurityIdentifier;
					if (securityIdentifier == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ImproperType"), "sourceSids");
					}
					array2[i] = GCHandle.Alloc(securityIdentifier.BinaryForm, GCHandleType.Pinned);
					array[i] = array2[i].AddrOfPinnedObject();
					i++;
				}
				safeLsaPolicyHandle = Win32.LsaOpenPolicy(null, PolicyRights.POLICY_LOOKUP_NAMES);
				someFailed = false;
				uint num = Win32Native.LsaLookupSids(safeLsaPolicyHandle, sourceSids.Count, array, ref invalidHandle, ref invalidHandle2);
				if (num == 3221225495U || num == 3221225626U)
				{
					throw new OutOfMemoryException();
				}
				if (num == 3221225506U)
				{
					throw new UnauthorizedAccessException();
				}
				if (num == 3221225587U || num == 263U)
				{
					someFailed = true;
				}
				else if (num != 0U)
				{
					int num2 = Win32Native.LsaNtStatusToWinError((int)num);
					throw new SystemException(Win32Native.GetMessage(num2));
				}
				IdentityReferenceCollection identityReferenceCollection = new IdentityReferenceCollection(sourceSids.Count);
				if (num == 0U || num == 263U)
				{
					Win32Native.LSA_REFERENCED_DOMAIN_LIST lsa_REFERENCED_DOMAIN_LIST = (Win32Native.LSA_REFERENCED_DOMAIN_LIST)Marshal.PtrToStructure(invalidHandle.DangerousGetHandle(), typeof(Win32Native.LSA_REFERENCED_DOMAIN_LIST));
					string[] array3 = new string[lsa_REFERENCED_DOMAIN_LIST.Entries];
					for (i = 0; i < lsa_REFERENCED_DOMAIN_LIST.Entries; i++)
					{
						Win32Native.LSA_TRUST_INFORMATION lsa_TRUST_INFORMATION = (Win32Native.LSA_TRUST_INFORMATION)Marshal.PtrToStructure(new IntPtr((long)lsa_REFERENCED_DOMAIN_LIST.Domains + (long)(i * Marshal.SizeOf(typeof(Win32Native.LSA_TRUST_INFORMATION)))), typeof(Win32Native.LSA_TRUST_INFORMATION));
						array3[i] = Marshal.PtrToStringUni(lsa_TRUST_INFORMATION.Name.Buffer, (int)(lsa_TRUST_INFORMATION.Name.Length / 2));
					}
					i = 0;
					while (i < sourceSids.Count)
					{
						Win32Native.LSA_TRANSLATED_NAME lsa_TRANSLATED_NAME = (Win32Native.LSA_TRANSLATED_NAME)Marshal.PtrToStructure(new IntPtr((long)invalidHandle2.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(Win32Native.LSA_TRANSLATED_NAME)))), typeof(Win32Native.LSA_TRANSLATED_NAME));
						switch (lsa_TRANSLATED_NAME.Use)
						{
						case 1:
						case 2:
						case 4:
						case 5:
						case 9:
						{
							string text = Marshal.PtrToStringUni(lsa_TRANSLATED_NAME.Name.Buffer, (int)(lsa_TRANSLATED_NAME.Name.Length / 2));
							string text2 = array3[lsa_TRANSLATED_NAME.DomainIndex];
							identityReferenceCollection.Add(new NTAccount(text2, text));
							break;
						}
						case 3:
						case 6:
						case 7:
						case 8:
							goto IL_02F0;
						default:
							goto IL_02F0;
						}
						IL_0302:
						i++;
						continue;
						IL_02F0:
						someFailed = true;
						identityReferenceCollection.Add(sourceSids[i]);
						goto IL_0302;
					}
				}
				else
				{
					for (i = 0; i < sourceSids.Count; i++)
					{
						identityReferenceCollection.Add(sourceSids[i]);
					}
				}
				identityReferenceCollection2 = identityReferenceCollection;
			}
			finally
			{
				for (i = 0; i < sourceSids.Count; i++)
				{
					if (array2[i].IsAllocated)
					{
						array2[i].Free();
					}
				}
				safeLsaPolicyHandle.Dispose();
				invalidHandle.Dispose();
				invalidHandle2.Dispose();
			}
			return identityReferenceCollection2;
		}

		// Token: 0x06005563 RID: 21859 RVA: 0x00136CEC File Offset: 0x00135CEC
		internal static IdentityReferenceCollection Translate(IdentityReferenceCollection sourceSids, Type targetType, bool forceSuccess)
		{
			bool flag = false;
			IdentityReferenceCollection identityReferenceCollection = SecurityIdentifier.Translate(sourceSids, targetType, out flag);
			if (forceSuccess && flag)
			{
				IdentityReferenceCollection identityReferenceCollection2 = new IdentityReferenceCollection();
				foreach (IdentityReference identityReference in identityReferenceCollection)
				{
					if (identityReference.GetType() != targetType)
					{
						identityReferenceCollection2.Add(identityReference);
					}
				}
				throw new IdentityNotMappedException(Environment.GetResourceString("IdentityReference_IdentityNotMapped"), identityReferenceCollection2);
			}
			return identityReferenceCollection;
		}

		// Token: 0x06005564 RID: 21860 RVA: 0x00136D6C File Offset: 0x00135D6C
		internal static IdentityReferenceCollection Translate(IdentityReferenceCollection sourceSids, Type targetType, out bool someFailed)
		{
			if (sourceSids == null)
			{
				throw new ArgumentNullException("sourceSids");
			}
			if (targetType == typeof(NTAccount))
			{
				return SecurityIdentifier.TranslateToNTAccounts(sourceSids, out someFailed);
			}
			throw new ArgumentException(Environment.GetResourceString("IdentityReference_MustBeIdentityReference"), "targetType");
		}

		// Token: 0x04002C6B RID: 11371
		internal static readonly long MaxIdentifierAuthority = 281474976710655L;

		// Token: 0x04002C6C RID: 11372
		internal static readonly byte MaxSubAuthorities = 15;

		// Token: 0x04002C6D RID: 11373
		public static readonly int MinBinaryLength = 8;

		// Token: 0x04002C6E RID: 11374
		public static readonly int MaxBinaryLength = (int)(8 + SecurityIdentifier.MaxSubAuthorities * 4);

		// Token: 0x04002C6F RID: 11375
		private IdentifierAuthority _IdentifierAuthority;

		// Token: 0x04002C70 RID: 11376
		private int[] _SubAuthorities;

		// Token: 0x04002C71 RID: 11377
		private byte[] _BinaryForm;

		// Token: 0x04002C72 RID: 11378
		private SecurityIdentifier _AccountDomainSid;

		// Token: 0x04002C73 RID: 11379
		private bool _AccountDomainSidInitialized;

		// Token: 0x04002C74 RID: 11380
		private string _SddlForm;
	}
}
