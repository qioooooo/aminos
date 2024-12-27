using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Security.Util;
using System.Text;

namespace System.Security.Permissions
{
	// Token: 0x02000635 RID: 1589
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
	[ComVisible(true)]
	[Serializable]
	public sealed class PermissionSetAttribute : CodeAccessSecurityAttribute
	{
		// Token: 0x060039B7 RID: 14775 RVA: 0x000C2806 File Offset: 0x000C1806
		public PermissionSetAttribute(SecurityAction action)
			: base(action)
		{
			this.m_unicode = false;
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x060039B8 RID: 14776 RVA: 0x000C2816 File Offset: 0x000C1816
		// (set) Token: 0x060039B9 RID: 14777 RVA: 0x000C281E File Offset: 0x000C181E
		public string File
		{
			get
			{
				return this.m_file;
			}
			set
			{
				this.m_file = value;
			}
		}

		// Token: 0x170009D4 RID: 2516
		// (get) Token: 0x060039BA RID: 14778 RVA: 0x000C2827 File Offset: 0x000C1827
		// (set) Token: 0x060039BB RID: 14779 RVA: 0x000C282F File Offset: 0x000C182F
		public bool UnicodeEncoded
		{
			get
			{
				return this.m_unicode;
			}
			set
			{
				this.m_unicode = value;
			}
		}

		// Token: 0x170009D5 RID: 2517
		// (get) Token: 0x060039BC RID: 14780 RVA: 0x000C2838 File Offset: 0x000C1838
		// (set) Token: 0x060039BD RID: 14781 RVA: 0x000C2840 File Offset: 0x000C1840
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				this.m_name = value;
			}
		}

		// Token: 0x170009D6 RID: 2518
		// (get) Token: 0x060039BE RID: 14782 RVA: 0x000C2849 File Offset: 0x000C1849
		// (set) Token: 0x060039BF RID: 14783 RVA: 0x000C2851 File Offset: 0x000C1851
		public string XML
		{
			get
			{
				return this.m_xml;
			}
			set
			{
				this.m_xml = value;
			}
		}

		// Token: 0x170009D7 RID: 2519
		// (get) Token: 0x060039C0 RID: 14784 RVA: 0x000C285A File Offset: 0x000C185A
		// (set) Token: 0x060039C1 RID: 14785 RVA: 0x000C2862 File Offset: 0x000C1862
		public string Hex
		{
			get
			{
				return this.m_hex;
			}
			set
			{
				this.m_hex = value;
			}
		}

		// Token: 0x060039C2 RID: 14786 RVA: 0x000C286B File Offset: 0x000C186B
		public override IPermission CreatePermission()
		{
			return null;
		}

		// Token: 0x060039C3 RID: 14787 RVA: 0x000C2870 File Offset: 0x000C1870
		private PermissionSet BruteForceParseStream(Stream stream)
		{
			Encoding[] array = new Encoding[]
			{
				Encoding.UTF8,
				Encoding.ASCII,
				Encoding.Unicode
			};
			StreamReader streamReader = null;
			Exception ex = null;
			int num = 0;
			while (streamReader == null && num < array.Length)
			{
				try
				{
					stream.Position = 0L;
					streamReader = new StreamReader(stream, array[num]);
					return this.ParsePermissionSet(new Parser(streamReader));
				}
				catch (Exception ex2)
				{
					if (ex == null)
					{
						ex = ex2;
					}
				}
				num++;
			}
			throw ex;
		}

		// Token: 0x060039C4 RID: 14788 RVA: 0x000C28F8 File Offset: 0x000C18F8
		private PermissionSet ParsePermissionSet(Parser parser)
		{
			SecurityElement topElement = parser.GetTopElement();
			PermissionSet permissionSet = new PermissionSet(PermissionState.None);
			permissionSet.FromXml(topElement);
			return permissionSet;
		}

		// Token: 0x060039C5 RID: 14789 RVA: 0x000C291C File Offset: 0x000C191C
		public PermissionSet CreatePermissionSet()
		{
			if (this.m_unrestricted)
			{
				return new PermissionSet(PermissionState.Unrestricted);
			}
			if (this.m_name != null)
			{
				return PolicyLevel.GetBuiltInSet(this.m_name);
			}
			if (this.m_xml != null)
			{
				return this.ParsePermissionSet(new Parser(this.m_xml.ToCharArray()));
			}
			if (this.m_hex != null)
			{
				return this.BruteForceParseStream(new MemoryStream(global::System.Security.Util.Hex.DecodeHexString(this.m_hex)));
			}
			if (this.m_file != null)
			{
				return this.BruteForceParseStream(new FileStream(this.m_file, FileMode.Open, FileAccess.Read));
			}
			return new PermissionSet(PermissionState.None);
		}

		// Token: 0x04001DD0 RID: 7632
		private string m_file;

		// Token: 0x04001DD1 RID: 7633
		private string m_name;

		// Token: 0x04001DD2 RID: 7634
		private bool m_unicode;

		// Token: 0x04001DD3 RID: 7635
		private string m_xml;

		// Token: 0x04001DD4 RID: 7636
		private string m_hex;
	}
}
