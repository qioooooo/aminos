using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Collections
{
	// Token: 0x02000249 RID: 585
	[ComVisible(true)]
	[Serializable]
	public sealed class Comparer : IComparer, ISerializable
	{
		// Token: 0x0600176B RID: 5995 RVA: 0x0003CAC5 File Offset: 0x0003BAC5
		private Comparer()
		{
			this.m_compareInfo = null;
		}

		// Token: 0x0600176C RID: 5996 RVA: 0x0003CAD4 File Offset: 0x0003BAD4
		public Comparer(CultureInfo culture)
		{
			if (culture == null)
			{
				throw new ArgumentNullException("culture");
			}
			this.m_compareInfo = culture.CompareInfo;
		}

		// Token: 0x0600176D RID: 5997 RVA: 0x0003CAF8 File Offset: 0x0003BAF8
		private Comparer(SerializationInfo info, StreamingContext context)
		{
			this.m_compareInfo = null;
			SerializationInfoEnumerator enumerator = info.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string name;
				if ((name = enumerator.Name) != null && name == "CompareInfo")
				{
					this.m_compareInfo = (CompareInfo)info.GetValue("CompareInfo", typeof(CompareInfo));
				}
			}
		}

		// Token: 0x0600176E RID: 5998 RVA: 0x0003CB5C File Offset: 0x0003BB5C
		public int Compare(object a, object b)
		{
			if (a == b)
			{
				return 0;
			}
			if (a == null)
			{
				return -1;
			}
			if (b == null)
			{
				return 1;
			}
			if (this.m_compareInfo != null)
			{
				string text = a as string;
				string text2 = b as string;
				if (text != null && text2 != null)
				{
					return this.m_compareInfo.Compare(text, text2);
				}
			}
			IComparable comparable = a as IComparable;
			if (comparable != null)
			{
				return comparable.CompareTo(b);
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_ImplementIComparable"));
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x0003CBC4 File Offset: 0x0003BBC4
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException("info");
			}
			if (this.m_compareInfo != null)
			{
				info.AddValue("CompareInfo", this.m_compareInfo);
			}
		}

		// Token: 0x04000939 RID: 2361
		private const string CompareInfoName = "CompareInfo";

		// Token: 0x0400093A RID: 2362
		private CompareInfo m_compareInfo;

		// Token: 0x0400093B RID: 2363
		public static readonly Comparer Default = new Comparer(CultureInfo.CurrentCulture);

		// Token: 0x0400093C RID: 2364
		public static readonly Comparer DefaultInvariant = new Comparer(CultureInfo.InvariantCulture);
	}
}
