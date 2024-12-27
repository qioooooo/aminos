using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Security.Util;
using System.Threading;

namespace System.Security.Policy
{
	// Token: 0x020004AB RID: 1195
	[ComVisible(true)]
	[Serializable]
	public sealed class HashMembershipCondition : ISerializable, IDeserializationCallback, IReportMatchMembershipCondition, IMembershipCondition, ISecurityEncodable, ISecurityPolicyEncodable
	{
		// Token: 0x17000896 RID: 2198
		// (get) Token: 0x0600305C RID: 12380 RVA: 0x000A698C File Offset: 0x000A598C
		private object InternalSyncObject
		{
			get
			{
				if (this.s_InternalSyncObject == null)
				{
					object obj = new object();
					Interlocked.CompareExchange(ref this.s_InternalSyncObject, obj, null);
				}
				return this.s_InternalSyncObject;
			}
		}

		// Token: 0x0600305D RID: 12381 RVA: 0x000A69BB File Offset: 0x000A59BB
		internal HashMembershipCondition()
		{
		}

		// Token: 0x0600305E RID: 12382 RVA: 0x000A69C4 File Offset: 0x000A59C4
		private HashMembershipCondition(SerializationInfo info, StreamingContext context)
		{
			this.m_value = (byte[])info.GetValue("HashValue", typeof(byte[]));
			string text = (string)info.GetValue("HashAlgorithm", typeof(string));
			if (text != null)
			{
				this.m_hashAlg = HashAlgorithm.Create(text);
				return;
			}
			this.m_hashAlg = new SHA1Managed();
		}

		// Token: 0x0600305F RID: 12383 RVA: 0x000A6A30 File Offset: 0x000A5A30
		public HashMembershipCondition(HashAlgorithm hashAlg, byte[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (hashAlg == null)
			{
				throw new ArgumentNullException("hashAlg");
			}
			this.m_value = new byte[value.Length];
			Array.Copy(value, this.m_value, value.Length);
			this.m_hashAlg = hashAlg;
		}

		// Token: 0x06003060 RID: 12384 RVA: 0x000A6A83 File Offset: 0x000A5A83
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("HashValue", this.HashValue);
			info.AddValue("HashAlgorithm", this.HashAlgorithm.ToString());
		}

		// Token: 0x06003061 RID: 12385 RVA: 0x000A6AAC File Offset: 0x000A5AAC
		void IDeserializationCallback.OnDeserialization(object sender)
		{
		}

		// Token: 0x17000897 RID: 2199
		// (get) Token: 0x06003063 RID: 12387 RVA: 0x000A6AC5 File Offset: 0x000A5AC5
		// (set) Token: 0x06003062 RID: 12386 RVA: 0x000A6AAE File Offset: 0x000A5AAE
		public HashAlgorithm HashAlgorithm
		{
			get
			{
				if (this.m_hashAlg == null && this.m_element != null)
				{
					this.ParseHashAlgorithm();
				}
				return this.m_hashAlg;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("HashAlgorithm");
				}
				this.m_hashAlg = value;
			}
		}

		// Token: 0x17000898 RID: 2200
		// (get) Token: 0x06003065 RID: 12389 RVA: 0x000A6B10 File Offset: 0x000A5B10
		// (set) Token: 0x06003064 RID: 12388 RVA: 0x000A6AE3 File Offset: 0x000A5AE3
		public byte[] HashValue
		{
			get
			{
				if (this.m_value == null && this.m_element != null)
				{
					this.ParseHashValue();
				}
				if (this.m_value == null)
				{
					return null;
				}
				byte[] array = new byte[this.m_value.Length];
				Array.Copy(this.m_value, array, this.m_value.Length);
				return array;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.m_value = new byte[value.Length];
				Array.Copy(value, this.m_value, value.Length);
			}
		}

		// Token: 0x06003066 RID: 12390 RVA: 0x000A6B60 File Offset: 0x000A5B60
		public bool Check(Evidence evidence)
		{
			object obj = null;
			return ((IReportMatchMembershipCondition)this).Check(evidence, out obj);
		}

		// Token: 0x06003067 RID: 12391 RVA: 0x000A6B78 File Offset: 0x000A5B78
		bool IReportMatchMembershipCondition.Check(Evidence evidence, out object usedEvidence)
		{
			usedEvidence = null;
			if (evidence == null)
			{
				return false;
			}
			IEnumerator hostEnumerator = evidence.GetHostEnumerator();
			while (hostEnumerator.MoveNext())
			{
				object obj = hostEnumerator.Current;
				Hash hash = obj as Hash;
				if (hash != null)
				{
					if (this.m_value == null && this.m_element != null)
					{
						this.ParseHashValue();
					}
					if (this.m_hashAlg == null && this.m_element != null)
					{
						this.ParseHashAlgorithm();
					}
					byte[] array = null;
					lock (this.InternalSyncObject)
					{
						array = hash.GenerateHash(this.m_hashAlg);
					}
					if (array != null && HashMembershipCondition.CompareArrays(array, this.m_value))
					{
						usedEvidence = hash;
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003068 RID: 12392 RVA: 0x000A6C28 File Offset: 0x000A5C28
		public IMembershipCondition Copy()
		{
			if (this.m_value == null && this.m_element != null)
			{
				this.ParseHashValue();
			}
			if (this.m_hashAlg == null && this.m_element != null)
			{
				this.ParseHashAlgorithm();
			}
			return new HashMembershipCondition(this.m_hashAlg, this.m_value);
		}

		// Token: 0x06003069 RID: 12393 RVA: 0x000A6C67 File Offset: 0x000A5C67
		public SecurityElement ToXml()
		{
			return this.ToXml(null);
		}

		// Token: 0x0600306A RID: 12394 RVA: 0x000A6C70 File Offset: 0x000A5C70
		public void FromXml(SecurityElement e)
		{
			this.FromXml(e, null);
		}

		// Token: 0x0600306B RID: 12395 RVA: 0x000A6C7C File Offset: 0x000A5C7C
		public SecurityElement ToXml(PolicyLevel level)
		{
			if (this.m_value == null && this.m_element != null)
			{
				this.ParseHashValue();
			}
			if (this.m_hashAlg == null && this.m_element != null)
			{
				this.ParseHashAlgorithm();
			}
			SecurityElement securityElement = new SecurityElement("IMembershipCondition");
			XMLUtil.AddClassAttribute(securityElement, base.GetType(), "System.Security.Policy.HashMembershipCondition");
			securityElement.AddAttribute("version", "1");
			if (this.m_value != null)
			{
				securityElement.AddAttribute("HashValue", Hex.EncodeHexString(this.HashValue));
			}
			if (this.m_hashAlg != null)
			{
				securityElement.AddAttribute("HashAlgorithm", this.HashAlgorithm.GetType().FullName);
			}
			return securityElement;
		}

		// Token: 0x0600306C RID: 12396 RVA: 0x000A6D24 File Offset: 0x000A5D24
		public void FromXml(SecurityElement e, PolicyLevel level)
		{
			if (e == null)
			{
				throw new ArgumentNullException("e");
			}
			if (!e.Tag.Equals("IMembershipCondition"))
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_MembershipConditionElement"));
			}
			lock (this.InternalSyncObject)
			{
				this.m_element = e;
				this.m_value = null;
				this.m_hashAlg = null;
			}
		}

		// Token: 0x0600306D RID: 12397 RVA: 0x000A6D9C File Offset: 0x000A5D9C
		public override bool Equals(object o)
		{
			HashMembershipCondition hashMembershipCondition = o as HashMembershipCondition;
			if (hashMembershipCondition != null)
			{
				if (this.m_hashAlg == null && this.m_element != null)
				{
					this.ParseHashAlgorithm();
				}
				if (hashMembershipCondition.m_hashAlg == null && hashMembershipCondition.m_element != null)
				{
					hashMembershipCondition.ParseHashAlgorithm();
				}
				if (this.m_hashAlg != null && hashMembershipCondition.m_hashAlg != null && this.m_hashAlg.GetType() == hashMembershipCondition.m_hashAlg.GetType())
				{
					if (this.m_value == null && this.m_element != null)
					{
						this.ParseHashValue();
					}
					if (hashMembershipCondition.m_value == null && hashMembershipCondition.m_element != null)
					{
						hashMembershipCondition.ParseHashValue();
					}
					if (this.m_value.Length != hashMembershipCondition.m_value.Length)
					{
						return false;
					}
					for (int i = 0; i < this.m_value.Length; i++)
					{
						if (this.m_value[i] != hashMembershipCondition.m_value[i])
						{
							return false;
						}
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600306E RID: 12398 RVA: 0x000A6E7C File Offset: 0x000A5E7C
		public override int GetHashCode()
		{
			if (this.m_hashAlg == null && this.m_element != null)
			{
				this.ParseHashAlgorithm();
			}
			int num = ((this.m_hashAlg != null) ? this.m_hashAlg.GetType().GetHashCode() : 0);
			if (this.m_value == null && this.m_element != null)
			{
				this.ParseHashValue();
			}
			return num ^ HashMembershipCondition.GetByteArrayHashCode(this.m_value);
		}

		// Token: 0x0600306F RID: 12399 RVA: 0x000A6EE0 File Offset: 0x000A5EE0
		public override string ToString()
		{
			if (this.m_hashAlg == null)
			{
				this.ParseHashAlgorithm();
			}
			return string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Hash_ToString"), new object[]
			{
				this.m_hashAlg.GetType().AssemblyQualifiedName,
				Hex.EncodeHexString(this.HashValue)
			});
		}

		// Token: 0x06003070 RID: 12400 RVA: 0x000A6F38 File Offset: 0x000A5F38
		private void ParseHashValue()
		{
			lock (this.InternalSyncObject)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("HashValue");
					if (text == null)
					{
						throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Environment.GetResourceString("Argument_InvalidXMLElement"), new object[]
						{
							"HashValue",
							base.GetType().FullName
						}));
					}
					this.m_value = Hex.DecodeHexString(text);
					if (this.m_value != null && this.m_hashAlg != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x06003071 RID: 12401 RVA: 0x000A6FE8 File Offset: 0x000A5FE8
		private void ParseHashAlgorithm()
		{
			lock (this.InternalSyncObject)
			{
				if (this.m_element != null)
				{
					string text = this.m_element.Attribute("HashAlgorithm");
					if (text != null)
					{
						this.m_hashAlg = HashAlgorithm.Create(text);
					}
					else
					{
						this.m_hashAlg = new SHA1Managed();
					}
					if (this.m_value != null && this.m_hashAlg != null)
					{
						this.m_element = null;
					}
				}
			}
		}

		// Token: 0x06003072 RID: 12402 RVA: 0x000A706C File Offset: 0x000A606C
		private static bool CompareArrays(byte[] first, byte[] second)
		{
			if (first.Length != second.Length)
			{
				return false;
			}
			int num = first.Length;
			for (int i = 0; i < num; i++)
			{
				if (first[i] != second[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003073 RID: 12403 RVA: 0x000A70A0 File Offset: 0x000A60A0
		private static int GetByteArrayHashCode(byte[] baData)
		{
			if (baData == null)
			{
				return 0;
			}
			int num = 0;
			for (int i = 0; i < baData.Length; i++)
			{
				num = (num << 8) ^ (int)baData[i] ^ (num >> 24);
			}
			return num;
		}

		// Token: 0x04001823 RID: 6179
		private const string s_tagHashValue = "HashValue";

		// Token: 0x04001824 RID: 6180
		private const string s_tagHashAlgorithm = "HashAlgorithm";

		// Token: 0x04001825 RID: 6181
		private byte[] m_value;

		// Token: 0x04001826 RID: 6182
		private HashAlgorithm m_hashAlg;

		// Token: 0x04001827 RID: 6183
		private SecurityElement m_element;

		// Token: 0x04001828 RID: 6184
		private object s_InternalSyncObject;
	}
}
