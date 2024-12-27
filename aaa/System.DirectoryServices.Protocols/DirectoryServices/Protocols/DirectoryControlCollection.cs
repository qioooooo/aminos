using System;
using System.Collections;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200002E RID: 46
	public class DirectoryControlCollection : CollectionBase
	{
		// Token: 0x060000D9 RID: 217 RVA: 0x00005144 File Offset: 0x00004144
		public DirectoryControlCollection()
		{
			Utility.CheckOSVersion();
		}

		// Token: 0x17000037 RID: 55
		public DirectoryControl this[int index]
		{
			get
			{
				return (DirectoryControl)base.List[index];
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				base.List[index] = value;
			}
		}

		// Token: 0x060000DC RID: 220 RVA: 0x00005181 File Offset: 0x00004181
		public int Add(DirectoryControl control)
		{
			if (control == null)
			{
				throw new ArgumentNullException("control");
			}
			return base.List.Add(control);
		}

		// Token: 0x060000DD RID: 221 RVA: 0x000051A0 File Offset: 0x000041A0
		public void AddRange(DirectoryControl[] controls)
		{
			if (controls == null)
			{
				throw new ArgumentNullException("controls");
			}
			for (int i = 0; i < controls.Length; i++)
			{
				if (controls[i] == null)
				{
					throw new ArgumentException(Res.GetString("ContainNullControl"), "controls");
				}
			}
			base.InnerList.AddRange(controls);
		}

		// Token: 0x060000DE RID: 222 RVA: 0x000051F4 File Offset: 0x000041F4
		public void AddRange(DirectoryControlCollection controlCollection)
		{
			if (controlCollection == null)
			{
				throw new ArgumentNullException("controlCollection");
			}
			int count = controlCollection.Count;
			for (int i = 0; i < count; i++)
			{
				this.Add(controlCollection[i]);
			}
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00005230 File Offset: 0x00004230
		public bool Contains(DirectoryControl value)
		{
			return base.List.Contains(value);
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x0000523E File Offset: 0x0000423E
		public void CopyTo(DirectoryControl[] array, int index)
		{
			base.List.CopyTo(array, index);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0000524D File Offset: 0x0000424D
		public int IndexOf(DirectoryControl value)
		{
			return base.List.IndexOf(value);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0000525B File Offset: 0x0000425B
		public void Insert(int index, DirectoryControl value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			base.List.Insert(index, value);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00005278 File Offset: 0x00004278
		public void Remove(DirectoryControl value)
		{
			base.List.Remove(value);
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00005288 File Offset: 0x00004288
		protected override void OnValidate(object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (!(value is DirectoryControl))
			{
				throw new ArgumentException(Res.GetString("InvalidValueType", new object[] { "DirectoryControl" }), "value");
			}
		}
	}
}
