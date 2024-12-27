using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Threading;

namespace System.Drawing
{
	// Token: 0x02000049 RID: 73
	public sealed class ImageAnimator
	{
		// Token: 0x06000474 RID: 1140 RVA: 0x00011EC8 File Offset: 0x00010EC8
		private ImageAnimator()
		{
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00011ED0 File Offset: 0x00010ED0
		public static void UpdateFrames(Image image)
		{
			if (!ImageAnimator.anyFrameDirty || image == null || ImageAnimator.imageInfoList == null)
			{
				return;
			}
			if (ImageAnimator.threadWriterLockWaitCount > 0)
			{
				return;
			}
			ImageAnimator.rwImgListLock.AcquireReaderLock(-1);
			try
			{
				bool flag = false;
				bool flag2 = false;
				foreach (ImageAnimator.ImageInfo imageInfo in ImageAnimator.imageInfoList)
				{
					if (imageInfo.Image == image)
					{
						if (imageInfo.FrameDirty)
						{
							lock (imageInfo.Image)
							{
								imageInfo.UpdateFrame();
							}
						}
						flag2 = true;
					}
					if (imageInfo.FrameDirty)
					{
						flag = true;
					}
					if (flag && flag2)
					{
						break;
					}
				}
				ImageAnimator.anyFrameDirty = flag;
			}
			finally
			{
				ImageAnimator.rwImgListLock.ReleaseReaderLock();
			}
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x00011FB4 File Offset: 0x00010FB4
		public static void UpdateFrames()
		{
			if (!ImageAnimator.anyFrameDirty || ImageAnimator.imageInfoList == null)
			{
				return;
			}
			if (ImageAnimator.threadWriterLockWaitCount > 0)
			{
				return;
			}
			ImageAnimator.rwImgListLock.AcquireReaderLock(-1);
			try
			{
				foreach (ImageAnimator.ImageInfo imageInfo in ImageAnimator.imageInfoList)
				{
					lock (imageInfo.Image)
					{
						imageInfo.UpdateFrame();
					}
				}
				ImageAnimator.anyFrameDirty = false;
			}
			finally
			{
				ImageAnimator.rwImgListLock.ReleaseReaderLock();
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x0001206C File Offset: 0x0001106C
		public static void Animate(Image image, EventHandler onFrameChangedHandler)
		{
			if (image == null)
			{
				return;
			}
			ImageAnimator.ImageInfo imageInfo = null;
			lock (image)
			{
				imageInfo = new ImageAnimator.ImageInfo(image);
			}
			ImageAnimator.StopAnimate(image, onFrameChangedHandler);
			bool isReaderLockHeld = ImageAnimator.rwImgListLock.IsReaderLockHeld;
			LockCookie lockCookie = default(LockCookie);
			ImageAnimator.threadWriterLockWaitCount++;
			try
			{
				if (isReaderLockHeld)
				{
					lockCookie = ImageAnimator.rwImgListLock.UpgradeToWriterLock(-1);
				}
				else
				{
					ImageAnimator.rwImgListLock.AcquireWriterLock(-1);
				}
			}
			finally
			{
				ImageAnimator.threadWriterLockWaitCount--;
			}
			try
			{
				if (imageInfo.Animated)
				{
					if (ImageAnimator.imageInfoList == null)
					{
						ImageAnimator.imageInfoList = new List<ImageAnimator.ImageInfo>();
					}
					imageInfo.FrameChangedHandler = onFrameChangedHandler;
					ImageAnimator.imageInfoList.Add(imageInfo);
					if (ImageAnimator.animationThread == null)
					{
						ImageAnimator.animationThread = new Thread(new ThreadStart(ImageAnimator.AnimateImages50ms));
						ImageAnimator.animationThread.Name = typeof(ImageAnimator).Name;
						ImageAnimator.animationThread.IsBackground = true;
						ImageAnimator.animationThread.Start();
					}
				}
			}
			finally
			{
				if (isReaderLockHeld)
				{
					ImageAnimator.rwImgListLock.DowngradeFromWriterLock(ref lockCookie);
				}
				else
				{
					ImageAnimator.rwImgListLock.ReleaseWriterLock();
				}
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x000121A4 File Offset: 0x000111A4
		public static bool CanAnimate(Image image)
		{
			if (image == null)
			{
				return false;
			}
			lock (image)
			{
				Guid[] frameDimensionsList = image.FrameDimensionsList;
				foreach (Guid guid in frameDimensionsList)
				{
					FrameDimension frameDimension = new FrameDimension(guid);
					if (frameDimension.Equals(FrameDimension.Time))
					{
						return image.GetFrameCount(FrameDimension.Time) > 1;
					}
				}
			}
			return false;
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00012230 File Offset: 0x00011230
		public static void StopAnimate(Image image, EventHandler onFrameChangedHandler)
		{
			if (image == null || ImageAnimator.imageInfoList == null)
			{
				return;
			}
			bool isReaderLockHeld = ImageAnimator.rwImgListLock.IsReaderLockHeld;
			LockCookie lockCookie = default(LockCookie);
			ImageAnimator.threadWriterLockWaitCount++;
			try
			{
				if (isReaderLockHeld)
				{
					lockCookie = ImageAnimator.rwImgListLock.UpgradeToWriterLock(-1);
				}
				else
				{
					ImageAnimator.rwImgListLock.AcquireWriterLock(-1);
				}
			}
			finally
			{
				ImageAnimator.threadWriterLockWaitCount--;
			}
			try
			{
				int i = 0;
				while (i < ImageAnimator.imageInfoList.Count)
				{
					ImageAnimator.ImageInfo imageInfo = ImageAnimator.imageInfoList[i];
					if (image == imageInfo.Image)
					{
						if (onFrameChangedHandler == imageInfo.FrameChangedHandler || (onFrameChangedHandler != null && onFrameChangedHandler.Equals(imageInfo.FrameChangedHandler)))
						{
							ImageAnimator.imageInfoList.Remove(imageInfo);
							break;
						}
						break;
					}
					else
					{
						i++;
					}
				}
			}
			finally
			{
				if (isReaderLockHeld)
				{
					ImageAnimator.rwImgListLock.DowngradeFromWriterLock(ref lockCookie);
				}
				else
				{
					ImageAnimator.rwImgListLock.ReleaseWriterLock();
				}
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00012324 File Offset: 0x00011324
		private static void AnimateImages50ms()
		{
			for (;;)
			{
				ImageAnimator.rwImgListLock.AcquireReaderLock(-1);
				try
				{
					for (int i = 0; i < ImageAnimator.imageInfoList.Count; i++)
					{
						ImageAnimator.ImageInfo imageInfo = ImageAnimator.imageInfoList[i];
						imageInfo.FrameTimer += 5;
						if (imageInfo.FrameTimer >= imageInfo.FrameDelay(imageInfo.Frame))
						{
							imageInfo.FrameTimer = 0;
							if (imageInfo.Frame + 1 < imageInfo.FrameCount)
							{
								imageInfo.Frame++;
							}
							else
							{
								imageInfo.Frame = 0;
							}
							if (imageInfo.FrameDirty)
							{
								ImageAnimator.anyFrameDirty = true;
							}
						}
					}
				}
				finally
				{
					ImageAnimator.rwImgListLock.ReleaseReaderLock();
				}
				Thread.Sleep(50);
			}
		}

		// Token: 0x040002B9 RID: 697
		private static List<ImageAnimator.ImageInfo> imageInfoList;

		// Token: 0x040002BA RID: 698
		private static bool anyFrameDirty;

		// Token: 0x040002BB RID: 699
		private static Thread animationThread;

		// Token: 0x040002BC RID: 700
		private static ReaderWriterLock rwImgListLock = new ReaderWriterLock();

		// Token: 0x040002BD RID: 701
		[ThreadStatic]
		private static int threadWriterLockWaitCount;

		// Token: 0x0200004A RID: 74
		private class ImageInfo
		{
			// Token: 0x0600047C RID: 1148 RVA: 0x000123F0 File Offset: 0x000113F0
			public ImageInfo(Image image)
			{
				this.image = image;
				this.animated = ImageAnimator.CanAnimate(image);
				if (this.animated)
				{
					this.frameCount = image.GetFrameCount(FrameDimension.Time);
					PropertyItem propertyItem = image.GetPropertyItem(20736);
					if (propertyItem != null)
					{
						byte[] value = propertyItem.Value;
						this.frameDelay = new int[this.FrameCount];
						for (int i = 0; i < this.FrameCount; i++)
						{
							this.frameDelay[i] = (int)value[i * 4] + 256 * (int)value[i * 4 + 1] + 65536 * (int)value[i * 4 + 2] + 16777216 * (int)value[i * 4 + 3];
						}
					}
				}
				else
				{
					this.frameCount = 1;
				}
				if (this.frameDelay == null)
				{
					this.frameDelay = new int[this.FrameCount];
				}
			}

			// Token: 0x17000174 RID: 372
			// (get) Token: 0x0600047D RID: 1149 RVA: 0x000124C3 File Offset: 0x000114C3
			public bool Animated
			{
				get
				{
					return this.animated;
				}
			}

			// Token: 0x17000175 RID: 373
			// (get) Token: 0x0600047E RID: 1150 RVA: 0x000124CB File Offset: 0x000114CB
			// (set) Token: 0x0600047F RID: 1151 RVA: 0x000124D4 File Offset: 0x000114D4
			public int Frame
			{
				get
				{
					return this.frame;
				}
				set
				{
					if (this.frame != value)
					{
						if (value < 0 || value >= this.FrameCount)
						{
							throw new ArgumentException(SR.GetString("InvalidFrame"), "value");
						}
						if (this.Animated)
						{
							this.frame = value;
							this.frameDirty = true;
							this.OnFrameChanged(EventArgs.Empty);
						}
					}
				}
			}

			// Token: 0x17000176 RID: 374
			// (get) Token: 0x06000480 RID: 1152 RVA: 0x0001252D File Offset: 0x0001152D
			public bool FrameDirty
			{
				get
				{
					return this.frameDirty;
				}
			}

			// Token: 0x17000177 RID: 375
			// (get) Token: 0x06000481 RID: 1153 RVA: 0x00012535 File Offset: 0x00011535
			// (set) Token: 0x06000482 RID: 1154 RVA: 0x0001253D File Offset: 0x0001153D
			public EventHandler FrameChangedHandler
			{
				get
				{
					return this.onFrameChangedHandler;
				}
				set
				{
					this.onFrameChangedHandler = value;
				}
			}

			// Token: 0x17000178 RID: 376
			// (get) Token: 0x06000483 RID: 1155 RVA: 0x00012546 File Offset: 0x00011546
			public int FrameCount
			{
				get
				{
					return this.frameCount;
				}
			}

			// Token: 0x06000484 RID: 1156 RVA: 0x0001254E File Offset: 0x0001154E
			public int FrameDelay(int frame)
			{
				return this.frameDelay[frame];
			}

			// Token: 0x17000179 RID: 377
			// (get) Token: 0x06000485 RID: 1157 RVA: 0x00012558 File Offset: 0x00011558
			// (set) Token: 0x06000486 RID: 1158 RVA: 0x00012560 File Offset: 0x00011560
			internal int FrameTimer
			{
				get
				{
					return this.frameTimer;
				}
				set
				{
					this.frameTimer = value;
				}
			}

			// Token: 0x1700017A RID: 378
			// (get) Token: 0x06000487 RID: 1159 RVA: 0x00012569 File Offset: 0x00011569
			internal Image Image
			{
				get
				{
					return this.image;
				}
			}

			// Token: 0x06000488 RID: 1160 RVA: 0x00012571 File Offset: 0x00011571
			internal void UpdateFrame()
			{
				if (this.frameDirty)
				{
					this.image.SelectActiveFrame(FrameDimension.Time, this.Frame);
					this.frameDirty = false;
				}
			}

			// Token: 0x06000489 RID: 1161 RVA: 0x00012599 File Offset: 0x00011599
			protected void OnFrameChanged(EventArgs e)
			{
				if (this.onFrameChangedHandler != null)
				{
					this.onFrameChangedHandler(this.image, e);
				}
			}

			// Token: 0x040002BE RID: 702
			private const int PropertyTagFrameDelay = 20736;

			// Token: 0x040002BF RID: 703
			private Image image;

			// Token: 0x040002C0 RID: 704
			private int frame;

			// Token: 0x040002C1 RID: 705
			private int frameCount;

			// Token: 0x040002C2 RID: 706
			private bool frameDirty;

			// Token: 0x040002C3 RID: 707
			private bool animated;

			// Token: 0x040002C4 RID: 708
			private EventHandler onFrameChangedHandler;

			// Token: 0x040002C5 RID: 709
			private int[] frameDelay;

			// Token: 0x040002C6 RID: 710
			private int frameTimer;
		}
	}
}
