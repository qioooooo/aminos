using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200009D RID: 157
	[ComVisible(false)]
	[SuppressUnmanagedCodeSecurity]
	internal class Wldap32
	{
		// Token: 0x06000327 RID: 807
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_bind_sW")]
		public static extern int ldap_bind_s([In] IntPtr ldapHandle, string dn, SEC_WINNT_AUTH_IDENTITY_EX credentials, BindMethod method);

		// Token: 0x06000328 RID: 808
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_initW", SetLastError = true)]
		public static extern IntPtr ldap_init(string hostName, int portNumber);

		// Token: 0x06000329 RID: 809
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int ldap_connect([In] IntPtr ldapHandle, LDAP_TIMEVAL timeout);

		// Token: 0x0600032A RID: 810
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, ExactSpelling = true)]
		public static extern int ldap_unbind([In] IntPtr ldapHandle);

		// Token: 0x0600032B RID: 811
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_get_optionW")]
		public static extern int ldap_get_option_int([In] IntPtr ldapHandle, [In] LdapOption option, ref int outValue);

		// Token: 0x0600032C RID: 812
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_set_optionW")]
		public static extern int ldap_set_option_int([In] IntPtr ldapHandle, [In] LdapOption option, ref int inValue);

		// Token: 0x0600032D RID: 813
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_get_optionW")]
		public static extern int ldap_get_option_ptr([In] IntPtr ldapHandle, [In] LdapOption option, ref IntPtr outValue);

		// Token: 0x0600032E RID: 814
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_set_optionW")]
		public static extern int ldap_set_option_ptr([In] IntPtr ldapHandle, [In] LdapOption option, ref IntPtr inValue);

		// Token: 0x0600032F RID: 815
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_get_optionW")]
		public static extern int ldap_get_option_sechandle([In] IntPtr ldapHandle, [In] LdapOption option, ref SecurityHandle outValue);

		// Token: 0x06000330 RID: 816
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_get_optionW")]
		public static extern int ldap_get_option_secInfo([In] IntPtr ldapHandle, [In] LdapOption option, [In] [Out] SecurityPackageContextConnectionInformation outValue);

		// Token: 0x06000331 RID: 817
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_set_optionW")]
		public static extern int ldap_set_option_referral([In] IntPtr ldapHandle, [In] LdapOption option, ref LdapReferralCallback outValue);

		// Token: 0x06000332 RID: 818
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_set_optionW")]
		public static extern int ldap_set_option_clientcert([In] IntPtr ldapHandle, [In] LdapOption option, QUERYCLIENTCERT outValue);

		// Token: 0x06000333 RID: 819
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_set_optionW")]
		public static extern int ldap_set_option_servercert([In] IntPtr ldapHandle, [In] LdapOption option, VERIFYSERVERCERT outValue);

		// Token: 0x06000334 RID: 820
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl)]
		public static extern int LdapGetLastError();

		// Token: 0x06000335 RID: 821
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "cldap_openW", SetLastError = true)]
		public static extern IntPtr cldap_open(string hostName, int portNumber);

		// Token: 0x06000336 RID: 822
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_simple_bind_sW")]
		public static extern int ldap_simple_bind_s([In] IntPtr ldapHandle, string distinguishedName, string password);

		// Token: 0x06000337 RID: 823
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_delete_extW")]
		public static extern int ldap_delete_ext([In] IntPtr ldapHandle, string dn, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

		// Token: 0x06000338 RID: 824
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, SetLastError = true)]
		public static extern int ldap_result([In] IntPtr ldapHandle, int messageId, int all, LDAP_TIMEVAL timeout, ref IntPtr Mesage);

		// Token: 0x06000339 RID: 825
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_parse_resultW")]
		public static extern int ldap_parse_result([In] IntPtr ldapHandle, [In] IntPtr result, ref int serverError, ref IntPtr dn, ref IntPtr message, ref IntPtr referral, ref IntPtr control, byte freeIt);

		// Token: 0x0600033A RID: 826
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_parse_resultW")]
		public static extern int ldap_parse_result_referral([In] IntPtr ldapHandle, [In] IntPtr result, IntPtr serverError, IntPtr dn, IntPtr message, ref IntPtr referral, IntPtr control, byte freeIt);

		// Token: 0x0600033B RID: 827
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_memfreeW")]
		public static extern void ldap_memfree([In] IntPtr value);

		// Token: 0x0600033C RID: 828
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_value_freeW")]
		public static extern int ldap_value_free([In] IntPtr value);

		// Token: 0x0600033D RID: 829
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_controls_freeW")]
		public static extern int ldap_controls_free([In] IntPtr value);

		// Token: 0x0600033E RID: 830
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int ldap_abandon([In] IntPtr ldapHandle, [In] int messagId);

		// Token: 0x0600033F RID: 831
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_start_tls_sW")]
		public static extern int ldap_start_tls(IntPtr ldapHandle, ref int ServerReturnValue, ref IntPtr Message, IntPtr ServerControls, IntPtr ClientControls);

		// Token: 0x06000340 RID: 832
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_stop_tls_s")]
		public static extern byte ldap_stop_tls(IntPtr ldapHandle);

		// Token: 0x06000341 RID: 833
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_rename_extW")]
		public static extern int ldap_rename([In] IntPtr ldapHandle, string dn, string newRdn, string newParentDn, int deleteOldRdn, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

		// Token: 0x06000342 RID: 834
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_compare_extW")]
		public static extern int ldap_compare([In] IntPtr ldapHandle, string dn, string attributeName, string strValue, berval binaryValue, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

		// Token: 0x06000343 RID: 835
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_add_extW")]
		public static extern int ldap_add([In] IntPtr ldapHandle, string dn, IntPtr attrs, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

		// Token: 0x06000344 RID: 836
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_modify_extW")]
		public static extern int ldap_modify([In] IntPtr ldapHandle, string dn, IntPtr attrs, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

		// Token: 0x06000345 RID: 837
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_extended_operationW")]
		public static extern int ldap_extended_operation([In] IntPtr ldapHandle, string oid, berval data, IntPtr servercontrol, IntPtr clientcontrol, ref int messageNumber);

		// Token: 0x06000346 RID: 838
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_parse_extended_resultW")]
		public static extern int ldap_parse_extended_result([In] IntPtr ldapHandle, [In] IntPtr result, ref IntPtr oid, ref IntPtr data, byte freeIt);

		// Token: 0x06000347 RID: 839
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int ldap_msgfree([In] IntPtr result);

		// Token: 0x06000348 RID: 840
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_search_extW")]
		public static extern int ldap_search([In] IntPtr ldapHandle, string dn, int scope, string filter, IntPtr attributes, bool attributeOnly, IntPtr servercontrol, IntPtr clientcontrol, int timelimit, int sizelimit, ref int messageNumber);

		// Token: 0x06000349 RID: 841
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern IntPtr ldap_first_entry([In] IntPtr ldapHandle, [In] IntPtr result);

		// Token: 0x0600034A RID: 842
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern IntPtr ldap_next_entry([In] IntPtr ldapHandle, [In] IntPtr result);

		// Token: 0x0600034B RID: 843
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern IntPtr ldap_first_reference([In] IntPtr ldapHandle, [In] IntPtr result);

		// Token: 0x0600034C RID: 844
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern IntPtr ldap_next_reference([In] IntPtr ldapHandle, [In] IntPtr result);

		// Token: 0x0600034D RID: 845
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_get_dnW")]
		public static extern IntPtr ldap_get_dn([In] IntPtr ldapHandle, [In] IntPtr result);

		// Token: 0x0600034E RID: 846
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_first_attributeW")]
		public static extern IntPtr ldap_first_attribute([In] IntPtr ldapHandle, [In] IntPtr result, ref IntPtr address);

		// Token: 0x0600034F RID: 847
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_next_attributeW")]
		public static extern IntPtr ldap_next_attribute([In] IntPtr ldapHandle, [In] IntPtr result, [In] [Out] IntPtr address);

		// Token: 0x06000350 RID: 848
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern IntPtr ber_free([In] IntPtr berelement, int option);

		// Token: 0x06000351 RID: 849
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_get_values_lenW")]
		public static extern IntPtr ldap_get_values_len([In] IntPtr ldapHandle, [In] IntPtr result, string name);

		// Token: 0x06000352 RID: 850
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern IntPtr ldap_value_free_len([In] IntPtr berelement);

		// Token: 0x06000353 RID: 851
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_parse_referenceW")]
		public static extern int ldap_parse_reference([In] IntPtr ldapHandle, [In] IntPtr result, ref IntPtr referrals);

		// Token: 0x06000354 RID: 852
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ber_alloc_t")]
		public static extern IntPtr ber_alloc(int option);

		// Token: 0x06000355 RID: 853
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ber_printf")]
		public static extern int ber_printf_emptyarg(BerSafeHandle berElement, string format);

		// Token: 0x06000356 RID: 854
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ber_printf")]
		public static extern int ber_printf_int(BerSafeHandle berElement, string format, int value);

		// Token: 0x06000357 RID: 855
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ber_printf")]
		public static extern int ber_printf_bytearray(BerSafeHandle berElement, string format, HGlobalMemHandle value, int length);

		// Token: 0x06000358 RID: 856
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ber_printf")]
		public static extern int ber_printf_berarray(BerSafeHandle berElement, string format, IntPtr value);

		// Token: 0x06000359 RID: 857
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int ber_flatten(BerSafeHandle berElement, ref IntPtr value);

		// Token: 0x0600035A RID: 858
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern IntPtr ber_init(berval value);

		// Token: 0x0600035B RID: 859
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int ber_scanf(BerSafeHandle berElement, string format);

		// Token: 0x0600035C RID: 860
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ber_scanf")]
		public static extern int ber_scanf_int(BerSafeHandle berElement, string format, ref int value);

		// Token: 0x0600035D RID: 861
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ber_scanf")]
		public static extern int ber_scanf_ptr(BerSafeHandle berElement, string format, ref IntPtr value);

		// Token: 0x0600035E RID: 862
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ber_scanf")]
		public static extern int ber_scanf_bitstring(BerSafeHandle berElement, string format, ref IntPtr value, ref int length);

		// Token: 0x0600035F RID: 863
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int ber_bvfree(IntPtr value);

		// Token: 0x06000360 RID: 864
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int ber_bvecfree(IntPtr value);

		// Token: 0x06000361 RID: 865
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_create_sort_controlW")]
		public static extern int ldap_create_sort_control(ConnectionHandle handle, IntPtr keys, byte critical, ref IntPtr control);

		// Token: 0x06000362 RID: 866
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode, EntryPoint = "ldap_control_freeW")]
		public static extern int ldap_control_free(IntPtr control);

		// Token: 0x06000363 RID: 867
		[DllImport("Crypt32.dll", CharSet = CharSet.Unicode)]
		public static extern int CertFreeCRLContext(IntPtr certContext);

		// Token: 0x06000364 RID: 868
		[DllImport("wldap32.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
		public static extern int ldap_result2error([In] IntPtr ldapHandle, [In] IntPtr result, int freeIt);

		// Token: 0x04000305 RID: 773
		public const int SEC_WINNT_AUTH_IDENTITY_UNICODE = 2;

		// Token: 0x04000306 RID: 774
		public const int SEC_WINNT_AUTH_IDENTITY_VERSION = 512;

		// Token: 0x04000307 RID: 775
		public const string MICROSOFT_KERBEROS_NAME_W = "Kerberos";
	}
}
