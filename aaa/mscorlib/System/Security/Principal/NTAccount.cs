using System;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Security.Principal
{
	// Token: 0x0200092C RID: 2348
	[ComVisible(false)]
	public sealed class NTAccount : IdentityReference
	{
		// Token: 0x06005535 RID: 21813 RVA: 0x001359B4 File Offset: 0x001349B4
		public NTAccount(string domainName, string accountName)
		{
			if (accountName == null)
			{
				throw new ArgumentNullException("accountName");
			}
			if (accountName.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StringZeroLength"), "accountName");
			}
			if (accountName.Length > 256)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_AccountNameTooLong"), "accountName");
			}
			if (domainName != null && domainName.Length > 255)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_DomainNameTooLong"), "domainName");
			}
			if (domainName == null || domainName.Length == 0)
			{
				this._Name = accountName;
				return;
			}
			this._Name = domainName + "\\" + accountName;
		}

		// Token: 0x06005536 RID: 21814 RVA: 0x00135A60 File Offset: 0x00134A60
		public NTAccount(string name)
		{
			if (name == null)
			{
				throw new ArgumentNullException("name");
			}
			if (name.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_StringZeroLength"), "name");
			}
			if (name.Length > 512)
			{
				throw new ArgumentException(Environment.GetResourceString("IdentityReference_AccountNameTooLong"), "name");
			}
			this._Name = name;
		}

		// Token: 0x17000ED7 RID: 3799
		// (get) Token: 0x06005537 RID: 21815 RVA: 0x00135AC7 File Offset: 0x00134AC7
		public override string Value
		{
			get
			{
				return this.ToString();
			}
		}

		// Token: 0x06005538 RID: 21816 RVA: 0x00135ACF File Offset: 0x00134ACF
		public override bool IsValidTargetType(Type targetType)
		{
			return targetType == typeof(SecurityIdentifier) || targetType == typeof(NTAccount);
		}

		// Token: 0x06005539 RID: 21817 RVA: 0x00135AF0 File Offset: 0x00134AF0
		public override IdentityReference Translate(Type targetType)
		{
			if (targetType == null)
			{
				throw new ArgumentNullException("targetType");
			}
			if (targetType == typeof(NTAccount))
			{
				return this;
			}
			if (targetType == typeof(SecurityIdentifier))
			{
				IdentityReferenceCollection identityReferenceCollection = NTAccount.Translate(new IdentityReferenceCollection(1) { this }, targetType, true);
				return identityReferenceCollection[0];
			}
			throw new ArgumentException(Environment.GetResourceString("IdentityReference_MustBeIdentityReference"), "targetType");
		}

		// Token: 0x0600553A RID: 21818 RVA: 0x00135B5C File Offset: 0x00134B5C
		public override bool Equals(object o)
		{
			if (o == null)
			{
				return false;
			}
			NTAccount ntaccount = o as NTAccount;
			return !(ntaccount == null) && this == ntaccount;
		}

		// Token: 0x0600553B RID: 21819 RVA: 0x00135B87 File Offset: 0x00134B87
		public override int GetHashCode()
		{
			return StringComparer.InvariantCultureIgnoreCase.GetHashCode(this._Name);
		}

		// Token: 0x0600553C RID: 21820 RVA: 0x00135B99 File Offset: 0x00134B99
		public override string ToString()
		{
			return this._Name;
		}

		// Token: 0x0600553D RID: 21821 RVA: 0x00135BA4 File Offset: 0x00134BA4
		internal static IdentityReferenceCollection Translate(IdentityReferenceCollection sourceAccounts, Type targetType, bool forceSuccess)
		{
			bool flag = false;
			IdentityReferenceCollection identityReferenceCollection = NTAccount.Translate(sourceAccounts, targetType, out flag);
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

		// Token: 0x0600553E RID: 21822 RVA: 0x00135C24 File Offset: 0x00134C24
		internal static IdentityReferenceCollection Translate(IdentityReferenceCollection sourceAccounts, Type targetType, out bool someFailed)
		{
			if (sourceAccounts == null)
			{
				throw new ArgumentNullException("sourceAccounts");
			}
			if (targetType == typeof(SecurityIdentifier))
			{
				return NTAccount.TranslateToSids(sourceAccounts, out someFailed);
			}
			throw new ArgumentException(Environment.GetResourceString("IdentityReference_MustBeIdentityReference"), "targetType");
		}

		// Token: 0x0600553F RID: 21823 RVA: 0x00135C60 File Offset: 0x00134C60
		public static bool operator ==(NTAccount left, NTAccount right)
		{
			return (left == null && right == null) || (left != null && right != null && left.ToString().Equals(right.ToString(), StringComparison.OrdinalIgnoreCase));
		}

		// Token: 0x06005540 RID: 21824 RVA: 0x00135C93 File Offset: 0x00134C93
		public static bool operator !=(NTAccount left, NTAccount right)
		{
			return !(left == right);
		}

		// Token: 0x06005541 RID: 21825 RVA: 0x00135CA0 File Offset: 0x00134CA0
		private static IdentityReferenceCollection TranslateToSids(IdentityReferenceCollection sourceAccounts, out bool someFailed)
		{
			if (!Win32.LsaApisSupported)
			{
				throw new PlatformNotSupportedException(Environment.GetResourceString("PlatformNotSupported_Win9x"));
			}
			SafeLsaPolicyHandle safeLsaPolicyHandle = SafeLsaPolicyHandle.InvalidHandle;
			SafeLsaMemoryHandle invalidHandle = SafeLsaMemoryHandle.InvalidHandle;
			SafeLsaMemoryHandle invalidHandle2 = SafeLsaMemoryHandle.InvalidHandle;
			int i = 0;
			if (sourceAccounts == null)
			{
				throw new ArgumentNullException("sourceAccounts");
			}
			if (sourceAccounts.Count == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Arg_EmptyCollection"), "sourceAccounts");
			}
			IdentityReferenceCollection identityReferenceCollection2;
			try
			{
				Win32Native.UNICODE_STRING[] array = new Win32Native.UNICODE_STRING[sourceAccounts.Count];
				foreach (IdentityReference identityReference in sourceAccounts)
				{
					NTAccount ntaccount = identityReference as NTAccount;
					if (ntaccount == null)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_ImproperType"), "sourceAccounts");
					}
					array[i].Buffer = ntaccount.ToString();
					if (array[i].Buffer.Length * 2 + 2 > 65535)
					{
						throw new SystemException();
					}
					array[i].Length = (ushort)(array[i].Buffer.Length * 2);
					array[i].MaximumLength = array[i].Length + 2;
					i++;
				}
				safeLsaPolicyHandle = Win32.LsaOpenPolicy(null, PolicyRights.POLICY_LOOKUP_NAMES);
				someFailed = false;
				uint num;
				if (Win32.LsaLookupNames2Supported)
				{
					num = Win32Native.LsaLookupNames2(safeLsaPolicyHandle, 0, sourceAccounts.Count, array, ref invalidHandle, ref invalidHandle2);
				}
				else
				{
					num = Win32Native.LsaLookupNames(safeLsaPolicyHandle, sourceAccounts.Count, array, ref invalidHandle, ref invalidHandle2);
				}
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
				IdentityReferenceCollection identityReferenceCollection = new IdentityReferenceCollection(sourceAccounts.Count);
				if (num == 0U || num == 263U)
				{
					if (Win32.LsaLookupNames2Supported)
					{
						i = 0;
						while (i < sourceAccounts.Count)
						{
							Win32Native.LSA_TRANSLATED_SID2 lsa_TRANSLATED_SID = (Win32Native.LSA_TRANSLATED_SID2)Marshal.PtrToStructure(new IntPtr((long)invalidHandle2.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(Win32Native.LSA_TRANSLATED_SID2)))), typeof(Win32Native.LSA_TRANSLATED_SID2));
							switch (lsa_TRANSLATED_SID.Use)
							{
							case 1:
							case 2:
							case 4:
							case 5:
							case 9:
								identityReferenceCollection.Add(new SecurityIdentifier(lsa_TRANSLATED_SID.Sid, true));
								break;
							case 3:
							case 6:
							case 7:
							case 8:
								goto IL_0287;
							default:
								goto IL_0287;
							}
							IL_0298:
							i++;
							continue;
							IL_0287:
							someFailed = true;
							identityReferenceCollection.Add(sourceAccounts[i]);
							goto IL_0298;
						}
					}
					else
					{
						Win32Native.LSA_REFERENCED_DOMAIN_LIST lsa_REFERENCED_DOMAIN_LIST = (Win32Native.LSA_REFERENCED_DOMAIN_LIST)Marshal.PtrToStructure(invalidHandle.DangerousGetHandle(), typeof(Win32Native.LSA_REFERENCED_DOMAIN_LIST));
						SecurityIdentifier[] array2 = new SecurityIdentifier[lsa_REFERENCED_DOMAIN_LIST.Entries];
						for (i = 0; i < lsa_REFERENCED_DOMAIN_LIST.Entries; i++)
						{
							array2[i] = new SecurityIdentifier(((Win32Native.LSA_TRUST_INFORMATION)Marshal.PtrToStructure(new IntPtr((long)lsa_REFERENCED_DOMAIN_LIST.Domains + (long)(i * Marshal.SizeOf(typeof(Win32Native.LSA_TRUST_INFORMATION)))), typeof(Win32Native.LSA_TRUST_INFORMATION))).Sid, true);
						}
						i = 0;
						while (i < sourceAccounts.Count)
						{
							Win32Native.LSA_TRANSLATED_SID lsa_TRANSLATED_SID2 = (Win32Native.LSA_TRANSLATED_SID)Marshal.PtrToStructure(new IntPtr((long)invalidHandle2.DangerousGetHandle() + (long)(i * Marshal.SizeOf(typeof(Win32Native.LSA_TRANSLATED_SID)))), typeof(Win32Native.LSA_TRANSLATED_SID));
							switch (lsa_TRANSLATED_SID2.Use)
							{
							case 1:
							case 2:
							case 4:
							case 5:
							case 9:
								identityReferenceCollection.Add(new SecurityIdentifier(array2[lsa_TRANSLATED_SID2.DomainIndex], lsa_TRANSLATED_SID2.Rid));
								break;
							case 3:
							case 6:
							case 7:
							case 8:
								goto IL_03CF;
							default:
								goto IL_03CF;
							}
							IL_03E0:
							i++;
							continue;
							IL_03CF:
							someFailed = true;
							identityReferenceCollection.Add(sourceAccounts[i]);
							goto IL_03E0;
						}
					}
				}
				else
				{
					for (i = 0; i < sourceAccounts.Count; i++)
					{
						identityReferenceCollection.Add(sourceAccounts[i]);
					}
				}
				identityReferenceCollection2 = identityReferenceCollection;
			}
			finally
			{
				safeLsaPolicyHandle.Dispose();
				invalidHandle.Dispose();
				invalidHandle2.Dispose();
			}
			return identityReferenceCollection2;
		}

		// Token: 0x04002C14 RID: 11284
		internal const int MaximumAccountNameLength = 256;

		// Token: 0x04002C15 RID: 11285
		internal const int MaximumDomainNameLength = 255;

		// Token: 0x04002C16 RID: 11286
		private readonly string _Name;
	}
}
