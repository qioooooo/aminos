using System;
using System.Collections;
using System.ComponentModel;

namespace System.Configuration.Install
{
	// Token: 0x02000012 RID: 18
	public class InstallerCollection : CollectionBase
	{
		// Token: 0x0600006B RID: 107 RVA: 0x0000408E File Offset: 0x0000308E
		internal InstallerCollection(Installer owner)
		{
			this.owner = owner;
		}

		// Token: 0x17000017 RID: 23
		public Installer this[int index]
		{
			get
			{
				return (Installer)base.List[index];
			}
			set
			{
				base.List[index] = value;
			}
		}

		// Token: 0x0600006E RID: 110 RVA: 0x000040BF File Offset: 0x000030BF
		public int Add(Installer value)
		{
			return base.List.Add(value);
		}

		// Token: 0x0600006F RID: 111 RVA: 0x000040D0 File Offset: 0x000030D0
		public void AddRange(InstallerCollection value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			int count = value.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000070 RID: 112 RVA: 0x0000410C File Offset: 0x0000310C
		public void AddRange(Installer[] value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			for (int i = 0; i < value.Length; i++)
			{
				this.Add(value[i]);
			}
		}

		// Token: 0x06000071 RID: 113 RVA: 0x0000413F File Offset: 0x0000313F
		public bool Contains(Installer value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x06000072 RID: 114 RVA: 0x0000414D File Offset: 0x0000314D
		public void CopyTo(Installer[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x06000073 RID: 115 RVA: 0x0000415C File Offset: 0x0000315C
		public int IndexOf(Installer value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x06000074 RID: 116 RVA: 0x0000416A File Offset: 0x0000316A
		public void Insert(int index, Installer value)
		{
			base.List.Insert(index, value);
		}

		// Token: 0x06000075 RID: 117 RVA: 0x00004179 File Offset: 0x00003179
		public void Remove(Installer value)
		{
			base.List.Remove(value);
		}

		// Token: 0x06000076 RID: 118 RVA: 0x00004187 File Offset: 0x00003187
		protected override void OnInsert(int index, object value)
		{
			if (value == this.owner)
			{
				throw new ArgumentException(Res.GetString("CantAddSelf"));
			}
			bool traceVerbose = CompModSwitches.InstallerDesign.TraceVerbose;
			((Installer)value).parent = this.owner;
		}

		// Token: 0x06000077 RID: 119 RVA: 0x000041BE File Offset: 0x000031BE
		protected override void OnRemove(int index, object value)
		{
			bool traceVerbose = CompModSwitches.InstallerDesign.TraceVerbose;
			((Installer)value).parent = null;
		}

		// Token: 0x06000078 RID: 120 RVA: 0x000041D8 File Offset: 0x000031D8
		protected override void OnSet(int index, object oldValue, object newValue)
		{
			if (newValue == this.owner)
			{
				throw new ArgumentException(Res.GetString("CantAddSelf"));
			}
			bool traceVerbose = CompModSwitches.InstallerDesign.TraceVerbose;
			((Installer)oldValue).parent = null;
			((Installer)newValue).parent = this.owner;
		}

		// Token: 0x040000F2 RID: 242
		private Installer owner;
	}
}
