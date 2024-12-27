using System;
using System.Data.ProviderBase;
using System.Runtime.CompilerServices;

namespace System.Data.OleDb
{
	// Token: 0x02000262 RID: 610
	internal sealed class ChapterHandle : WrappedIUnknown
	{
		// Token: 0x060020DA RID: 8410 RVA: 0x00264DB8 File Offset: 0x002641B8
		internal static ChapterHandle CreateChapterHandle(object chapteredRowset, RowBinding binding, int valueOffset)
		{
			if (chapteredRowset == null || IntPtr.Zero == binding.ReadIntPtr(valueOffset))
			{
				return ChapterHandle.DB_NULL_HCHAPTER;
			}
			return new ChapterHandle(chapteredRowset, binding, valueOffset);
		}

		// Token: 0x060020DB RID: 8411 RVA: 0x00264DEC File Offset: 0x002641EC
		internal static ChapterHandle CreateChapterHandle(IntPtr chapter)
		{
			if (IntPtr.Zero == chapter)
			{
				return ChapterHandle.DB_NULL_HCHAPTER;
			}
			return new ChapterHandle(chapter);
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x00264E14 File Offset: 0x00264214
		private ChapterHandle(IntPtr chapter)
			: base(null)
		{
			this._chapterHandle = chapter;
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x00264E30 File Offset: 0x00264230
		private ChapterHandle(object chapteredRowset, RowBinding binding, int valueOffset)
			: base(chapteredRowset)
		{
			RuntimeHelpers.PrepareConstrainedRegions();
			try
			{
			}
			finally
			{
				this._chapterHandle = binding.InterlockedExchangePointer(valueOffset);
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x00264E78 File Offset: 0x00264278
		internal IntPtr HChapter
		{
			get
			{
				return this._chapterHandle;
			}
		}

		// Token: 0x060020DF RID: 8415 RVA: 0x00264E8C File Offset: 0x0026428C
		protected override bool ReleaseHandle()
		{
			IntPtr chapterHandle = this._chapterHandle;
			this._chapterHandle = IntPtr.Zero;
			if (IntPtr.Zero != this.handle && IntPtr.Zero != chapterHandle)
			{
				Bid.Trace("<oledb.IChapteredRowset.ReleaseChapter|API|OLEDB> Chapter=%Id\n", chapterHandle);
				OleDbHResult oleDbHResult = NativeOledbWrapper.IChapteredRowsetReleaseChapter(this.handle, chapterHandle);
				Bid.Trace("<oledb.IChapteredRowset.ReleaseChapter|API|OLEDB|RET> %08X{HRESULT}\n", oleDbHResult);
			}
			return base.ReleaseHandle();
		}

		// Token: 0x04001540 RID: 5440
		internal static readonly ChapterHandle DB_NULL_HCHAPTER = new ChapterHandle(IntPtr.Zero);

		// Token: 0x04001541 RID: 5441
		private IntPtr _chapterHandle;
	}
}
