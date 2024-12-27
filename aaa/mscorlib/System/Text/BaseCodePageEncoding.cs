using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace System.Text
{
	// Token: 0x020003DD RID: 989
	[Serializable]
	internal abstract class BaseCodePageEncoding : EncodingNLS, ISerializable
	{
		// Token: 0x06002979 RID: 10617 RVA: 0x0008220F File Offset: 0x0008120F
		internal BaseCodePageEncoding(int codepage)
			: this(codepage, codepage)
		{
		}

		// Token: 0x0600297A RID: 10618 RVA: 0x00082219 File Offset: 0x00081219
		internal BaseCodePageEncoding(int codepage, int dataCodePage)
		{
			this.bFlagDataTable = true;
			this.pCodePage = null;
			base..ctor((codepage == 0) ? Win32Native.GetACP() : codepage);
			this.dataTableCodePage = dataCodePage;
			this.LoadCodePageTables();
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x00082248 File Offset: 0x00081248
		internal BaseCodePageEncoding(SerializationInfo info, StreamingContext context)
		{
			this.bFlagDataTable = true;
			this.pCodePage = null;
			base..ctor(0);
			throw new ArgumentNullException("this");
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x0008226C File Offset: 0x0008126C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.SerializeEncoding(info, context);
			info.AddValue(this.m_bUseMlangTypeForSerialization ? "m_maxByteSize" : "maxCharSize", this.IsSingleByte ? 1 : 2);
			info.SetType(this.m_bUseMlangTypeForSerialization ? typeof(MLangCodePageEncoding) : typeof(CodePageEncoding));
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000822CC File Offset: 0x000812CC
		private unsafe void LoadCodePageTables()
		{
			BaseCodePageEncoding.CodePageHeader* ptr = BaseCodePageEncoding.FindCodePage(this.dataTableCodePage);
			if (ptr == null)
			{
				throw new NotSupportedException(Environment.GetResourceString("NotSupported_NoCodepageData", new object[] { this.CodePage }));
			}
			this.pCodePage = ptr;
			this.LoadManagedCodePage();
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x00082320 File Offset: 0x00081320
		private unsafe static BaseCodePageEncoding.CodePageHeader* FindCodePage(int codePage)
		{
			for (int i = 0; i < (int)BaseCodePageEncoding.m_pCodePageFileHeader->CodePageCount; i++)
			{
				BaseCodePageEncoding.CodePageIndex* ptr = &BaseCodePageEncoding.m_pCodePageFileHeader->CodePages + i;
				if ((int)ptr->CodePage == codePage)
				{
					return (BaseCodePageEncoding.CodePageHeader*)(BaseCodePageEncoding.m_pCodePageFileHeader + ptr->Offset / sizeof(BaseCodePageEncoding.CodePageDataFileHeader));
				}
			}
			return null;
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x00082374 File Offset: 0x00081374
		internal unsafe static int GetCodePageByteSize(int codePage)
		{
			BaseCodePageEncoding.CodePageHeader* ptr = BaseCodePageEncoding.FindCodePage(codePage);
			if (ptr == null)
			{
				return 0;
			}
			return (int)ptr->ByteCount;
		}

		// Token: 0x06002980 RID: 10624
		protected abstract void LoadManagedCodePage();

		// Token: 0x06002981 RID: 10625 RVA: 0x00082398 File Offset: 0x00081398
		protected unsafe byte* GetSharedMemory(int iSize)
		{
			string memorySectionName = this.GetMemorySectionName();
			IntPtr intPtr;
			byte* ptr = EncodingTable.nativeCreateOpenFileMapping(memorySectionName, iSize, out intPtr);
			if (ptr == null)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
			if (intPtr != IntPtr.Zero)
			{
				this.safeMemorySectionHandle = new SafeViewOfFileHandle((IntPtr)((void*)ptr), true);
				this.safeFileMappingHandle = new SafeFileMappingHandle(intPtr, true);
			}
			return ptr;
		}

		// Token: 0x06002982 RID: 10626 RVA: 0x000823F8 File Offset: 0x000813F8
		protected unsafe virtual string GetMemorySectionName()
		{
			int num = (this.bFlagDataTable ? this.dataTableCodePage : this.CodePage);
			return string.Format(CultureInfo.InvariantCulture, "NLS_CodePage_{0}_{1}_{2}_{3}_{4}", new object[]
			{
				num,
				this.pCodePage->VersionMajor,
				this.pCodePage->VersionMinor,
				this.pCodePage->VersionRevision,
				this.pCodePage->VersionBuild
			});
		}

		// Token: 0x06002983 RID: 10627
		protected abstract void ReadBestFitTable();

		// Token: 0x06002984 RID: 10628 RVA: 0x0008248A File Offset: 0x0008148A
		internal override char[] GetBestFitUnicodeToBytesData()
		{
			if (this.arrayUnicodeBestFit == null)
			{
				this.ReadBestFitTable();
			}
			return this.arrayUnicodeBestFit;
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000824A0 File Offset: 0x000814A0
		internal override char[] GetBestFitBytesToUnicodeData()
		{
			if (this.arrayUnicodeBestFit == null)
			{
				this.ReadBestFitTable();
			}
			return this.arrayBytesBestFit;
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000824B6 File Offset: 0x000814B6
		internal void CheckMemorySection()
		{
			if (this.safeMemorySectionHandle != null && this.safeMemorySectionHandle.DangerousGetHandle() == IntPtr.Zero)
			{
				this.LoadManagedCodePage();
			}
		}

		// Token: 0x04001401 RID: 5121
		internal const string CODE_PAGE_DATA_FILE_NAME = "codepages.nlp";

		// Token: 0x04001402 RID: 5122
		[NonSerialized]
		protected int dataTableCodePage;

		// Token: 0x04001403 RID: 5123
		[NonSerialized]
		protected bool bFlagDataTable;

		// Token: 0x04001404 RID: 5124
		[NonSerialized]
		protected int iExtraBytes;

		// Token: 0x04001405 RID: 5125
		[NonSerialized]
		protected char[] arrayUnicodeBestFit;

		// Token: 0x04001406 RID: 5126
		[NonSerialized]
		protected char[] arrayBytesBestFit;

		// Token: 0x04001407 RID: 5127
		[NonSerialized]
		protected bool m_bUseMlangTypeForSerialization;

		// Token: 0x04001408 RID: 5128
		private unsafe static BaseCodePageEncoding.CodePageDataFileHeader* m_pCodePageFileHeader = (BaseCodePageEncoding.CodePageDataFileHeader*)GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(CharUnicodeInfo).Assembly, "codepages.nlp");

		// Token: 0x04001409 RID: 5129
		[NonSerialized]
		protected unsafe BaseCodePageEncoding.CodePageHeader* pCodePage;

		// Token: 0x0400140A RID: 5130
		[NonSerialized]
		protected SafeViewOfFileHandle safeMemorySectionHandle;

		// Token: 0x0400140B RID: 5131
		[NonSerialized]
		protected SafeFileMappingHandle safeFileMappingHandle;

		// Token: 0x020003DE RID: 990
		[StructLayout(LayoutKind.Explicit)]
		internal struct CodePageDataFileHeader
		{
			// Token: 0x0400140C RID: 5132
			[FieldOffset(0)]
			internal char TableName;

			// Token: 0x0400140D RID: 5133
			[FieldOffset(32)]
			internal ushort Version;

			// Token: 0x0400140E RID: 5134
			[FieldOffset(40)]
			internal short CodePageCount;

			// Token: 0x0400140F RID: 5135
			[FieldOffset(42)]
			internal short unused1;

			// Token: 0x04001410 RID: 5136
			[FieldOffset(44)]
			internal BaseCodePageEncoding.CodePageIndex CodePages;
		}

		// Token: 0x020003DF RID: 991
		[StructLayout(LayoutKind.Explicit, Pack = 2)]
		internal struct CodePageIndex
		{
			// Token: 0x04001411 RID: 5137
			[FieldOffset(0)]
			internal char CodePageName;

			// Token: 0x04001412 RID: 5138
			[FieldOffset(32)]
			internal short CodePage;

			// Token: 0x04001413 RID: 5139
			[FieldOffset(34)]
			internal short ByteCount;

			// Token: 0x04001414 RID: 5140
			[FieldOffset(36)]
			internal int Offset;
		}

		// Token: 0x020003E0 RID: 992
		[StructLayout(LayoutKind.Explicit)]
		internal struct CodePageHeader
		{
			// Token: 0x04001415 RID: 5141
			[FieldOffset(0)]
			internal char CodePageName;

			// Token: 0x04001416 RID: 5142
			[FieldOffset(32)]
			internal ushort VersionMajor;

			// Token: 0x04001417 RID: 5143
			[FieldOffset(34)]
			internal ushort VersionMinor;

			// Token: 0x04001418 RID: 5144
			[FieldOffset(36)]
			internal ushort VersionRevision;

			// Token: 0x04001419 RID: 5145
			[FieldOffset(38)]
			internal ushort VersionBuild;

			// Token: 0x0400141A RID: 5146
			[FieldOffset(40)]
			internal short CodePage;

			// Token: 0x0400141B RID: 5147
			[FieldOffset(42)]
			internal short ByteCount;

			// Token: 0x0400141C RID: 5148
			[FieldOffset(44)]
			internal char UnicodeReplace;

			// Token: 0x0400141D RID: 5149
			[FieldOffset(46)]
			internal ushort ByteReplace;

			// Token: 0x0400141E RID: 5150
			[FieldOffset(48)]
			internal short FirstDataWord;
		}
	}
}
