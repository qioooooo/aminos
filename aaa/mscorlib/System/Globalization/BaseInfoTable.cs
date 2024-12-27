using System;

namespace System.Globalization
{
	// Token: 0x0200036F RID: 879
	internal abstract class BaseInfoTable
	{
		// Token: 0x060022C3 RID: 8899 RVA: 0x00058797 File Offset: 0x00057797
		internal BaseInfoTable(string fileName, bool fromAssembly)
		{
			this.fileName = fileName;
			this.fromAssembly = fromAssembly;
			this.InitializeBaseInfoTablePointers(fileName, fromAssembly);
		}

		// Token: 0x060022C4 RID: 8900 RVA: 0x000587BC File Offset: 0x000577BC
		internal unsafe void InitializeBaseInfoTablePointers(string fileName, bool fromAssembly)
		{
			if (fromAssembly)
			{
				this.m_pDataFileStart = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(BaseInfoTable).Assembly, fileName);
			}
			else
			{
				this.memoryMapFile = new AgileSafeNativeMemoryHandle(fileName);
				if (this.memoryMapFile.FileSize == 0L)
				{
					this.m_valid = false;
					return;
				}
				this.m_pDataFileStart = this.memoryMapFile.GetBytePtr();
			}
			EndianessHeader* pDataFileStart = (EndianessHeader*)this.m_pDataFileStart;
			this.m_pCultureHeader = (CultureTableHeader*)(this.m_pDataFileStart + pDataFileStart->leOffset);
			this.SetDataItemPointers();
		}

		// Token: 0x1700062A RID: 1578
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x0005883D File Offset: 0x0005783D
		internal bool IsValid
		{
			get
			{
				return this.m_valid;
			}
		}

		// Token: 0x060022C6 RID: 8902 RVA: 0x00058848 File Offset: 0x00057848
		public override bool Equals(object value)
		{
			BaseInfoTable baseInfoTable = value as BaseInfoTable;
			return baseInfoTable != null && this.fromAssembly == baseInfoTable.fromAssembly && CultureInfo.InvariantCulture.CompareInfo.Compare(this.fileName, baseInfoTable.fileName, CompareOptions.IgnoreCase) == 0;
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x00058890 File Offset: 0x00057890
		public override int GetHashCode()
		{
			return this.fileName.GetHashCode();
		}

		// Token: 0x060022C8 RID: 8904
		internal abstract void SetDataItemPointers();

		// Token: 0x060022C9 RID: 8905 RVA: 0x000588A0 File Offset: 0x000578A0
		internal unsafe string GetStringPoolString(uint offset)
		{
			char* ptr = (char*)(this.m_pDataPool + offset);
			if (ptr[1] == '\0')
			{
				return string.Empty;
			}
			return new string(ptr + 1, 0, (int)(*ptr));
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000588D4 File Offset: 0x000578D4
		internal unsafe string[] GetStringArray(uint iOffset)
		{
			if (iOffset == 0U)
			{
				return new string[0];
			}
			ushort* ptr = this.m_pDataPool + iOffset;
			int num = (int)(*ptr);
			string[] array = new string[num];
			uint* ptr2 = (uint*)(ptr + 1);
			for (int i = 0; i < num; i++)
			{
				array[i] = this.GetStringPoolString(ptr2[i]);
			}
			return array;
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x0005892C File Offset: 0x0005792C
		internal unsafe int[][] GetWordArrayArray(uint iOffset)
		{
			if (iOffset == 0U)
			{
				return new int[0][];
			}
			short* ptr = (short*)(this.m_pDataPool + iOffset);
			int num = (int)(*ptr);
			int[][] array = new int[num][];
			uint* ptr2 = (uint*)(ptr + 1);
			for (int i = 0; i < num; i++)
			{
				ptr = (short*)(this.m_pDataPool + ptr2[i]);
				int num2 = (int)(*ptr);
				ptr++;
				array[i] = new int[num2];
				for (int j = 0; j < num2; j++)
				{
					array[i][j] = (int)ptr[j];
				}
			}
			return array;
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000589B8 File Offset: 0x000579B8
		internal unsafe int CompareStringToStringPoolStringBinary(string name, int offset)
		{
			int num = 0;
			char* ptr = (char*)(this.m_pDataPool + offset);
			if (ptr[1] == '\0')
			{
				if (name.Length == 0)
				{
					return 0;
				}
				return 1;
			}
			else
			{
				int num2 = 0;
				while (num2 < (int)(*ptr) && num2 < name.Length)
				{
					num = (int)(name[num2] - ((ptr[num2 + 1] <= 'Z' && ptr[num2 + 1] >= 'A') ? (ptr[num2 + 1] + 'a' - 'A') : ptr[num2 + 1]));
					if (num != 0)
					{
						break;
					}
					num2++;
				}
				if (num != 0)
				{
					return num;
				}
				return name.Length - (int)(*ptr);
			}
		}

		// Token: 0x04000E71 RID: 3697
		internal unsafe byte* m_pDataFileStart;

		// Token: 0x04000E72 RID: 3698
		protected AgileSafeNativeMemoryHandle memoryMapFile;

		// Token: 0x04000E73 RID: 3699
		protected unsafe CultureTableHeader* m_pCultureHeader;

		// Token: 0x04000E74 RID: 3700
		internal unsafe byte* m_pItemData;

		// Token: 0x04000E75 RID: 3701
		internal uint m_numItem;

		// Token: 0x04000E76 RID: 3702
		internal uint m_itemSize;

		// Token: 0x04000E77 RID: 3703
		internal unsafe ushort* m_pDataPool;

		// Token: 0x04000E78 RID: 3704
		internal bool fromAssembly;

		// Token: 0x04000E79 RID: 3705
		internal string fileName;

		// Token: 0x04000E7A RID: 3706
		protected bool m_valid = true;
	}
}
