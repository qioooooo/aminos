using System;
using System.Collections;
using System.Deployment.Internal.Isolation;
using System.Runtime.InteropServices;

namespace System.Deployment.Application
{
	// Token: 0x02000012 RID: 18
	internal class ReferenceIdentity : ICloneable
	{
		// Token: 0x0600009F RID: 159 RVA: 0x00005442 File Offset: 0x00004442
		public ReferenceIdentity()
		{
			this._idComPtr = IsolationInterop.IdentityAuthority.CreateReference();
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x0000545A File Offset: 0x0000445A
		public ReferenceIdentity(string text)
		{
			this._idComPtr = IsolationInterop.IdentityAuthority.TextToReference(0U, text);
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00005474 File Offset: 0x00004474
		public ReferenceIdentity(IReferenceIdentity idComPtr)
		{
			this._idComPtr = idComPtr;
		}

		// Token: 0x1700002B RID: 43
		public string this[string name]
		{
			get
			{
				return this._idComPtr.GetAttribute(null, name);
			}
			set
			{
				this._idComPtr.SetAttribute(null, name, value);
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x060000A4 RID: 164 RVA: 0x000054A2 File Offset: 0x000044A2
		public string Name
		{
			get
			{
				return this["name"];
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x060000A5 RID: 165 RVA: 0x000054AF File Offset: 0x000044AF
		public string Culture
		{
			get
			{
				return this["culture"];
			}
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x060000A6 RID: 166 RVA: 0x000054BC File Offset: 0x000044BC
		public Version Version
		{
			get
			{
				string text = this["version"];
				if (text == null)
				{
					return null;
				}
				return new Version(text);
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x060000A7 RID: 167 RVA: 0x000054E0 File Offset: 0x000044E0
		public string PublicKeyToken
		{
			get
			{
				return this["publicKeyToken"];
			}
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060000A8 RID: 168 RVA: 0x000054ED File Offset: 0x000044ED
		// (set) Token: 0x060000A9 RID: 169 RVA: 0x000054FA File Offset: 0x000044FA
		public string ProcessorArchitecture
		{
			get
			{
				return this["processorArchitecture"];
			}
			set
			{
				this["processorArchitecture"] = value;
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060000AA RID: 170 RVA: 0x00005508 File Offset: 0x00004508
		public ulong Hash
		{
			get
			{
				return IsolationInterop.IdentityAuthority.HashReference(0U, this._idComPtr);
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060000AB RID: 171 RVA: 0x0000551C File Offset: 0x0000451C
		public IDENTITY_ATTRIBUTE[] Attributes
		{
			get
			{
				IEnumIDENTITY_ATTRIBUTE enumIDENTITY_ATTRIBUTE = null;
				IDENTITY_ATTRIBUTE[] array2;
				try
				{
					ArrayList arrayList = new ArrayList();
					enumIDENTITY_ATTRIBUTE = this._idComPtr.EnumAttributes();
					IDENTITY_ATTRIBUTE[] array = new IDENTITY_ATTRIBUTE[1];
					while (enumIDENTITY_ATTRIBUTE.Next(1U, array) == 1U)
					{
						arrayList.Add(array[0]);
					}
					array2 = (IDENTITY_ATTRIBUTE[])arrayList.ToArray(typeof(IDENTITY_ATTRIBUTE));
				}
				finally
				{
					if (enumIDENTITY_ATTRIBUTE != null)
					{
						Marshal.ReleaseComObject(enumIDENTITY_ATTRIBUTE);
					}
				}
				return array2;
			}
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060000AC RID: 172 RVA: 0x000055A0 File Offset: 0x000045A0
		public IReferenceIdentity ComPointer
		{
			get
			{
				return this._idComPtr;
			}
		}

		// Token: 0x060000AD RID: 173 RVA: 0x000055A8 File Offset: 0x000045A8
		public override bool Equals(object obj)
		{
			return obj is ReferenceIdentity && IsolationInterop.IdentityAuthority.AreReferencesEqual(0U, this.ComPointer, ((ReferenceIdentity)obj).ComPointer);
		}

		// Token: 0x060000AE RID: 174 RVA: 0x000055D0 File Offset: 0x000045D0
		public override int GetHashCode()
		{
			return (int)this.Hash;
		}

		// Token: 0x060000AF RID: 175 RVA: 0x000055D9 File Offset: 0x000045D9
		public override string ToString()
		{
			return IsolationInterop.IdentityAuthority.ReferenceToText(0U, this._idComPtr);
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x000055EC File Offset: 0x000045EC
		public object Clone()
		{
			return new ReferenceIdentity(this._idComPtr.Clone(IntPtr.Zero, null));
		}

		// Token: 0x04000051 RID: 81
		private IReferenceIdentity _idComPtr;
	}
}
