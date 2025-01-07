﻿using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;

namespace System.Windows.Forms.Design
{
	internal class ImageListDesigner : ComponentDesigner
	{
		private ColorDepth ColorDepth
		{
			get
			{
				return this.ImageList.ColorDepth;
			}
			set
			{
				this.ImageList.Images.Clear();
				this.ImageList.ColorDepth = value;
				this.Images.PopulateHandle();
			}
		}

		private bool ShouldSerializeColorDepth()
		{
			return this.Images.Count == 0;
		}

		private ImageListDesigner.OriginalImageCollection Images
		{
			get
			{
				if (this.originalImageCollection == null)
				{
					this.originalImageCollection = new ImageListDesigner.OriginalImageCollection(this);
				}
				return this.originalImageCollection;
			}
		}

		internal ImageList ImageList
		{
			get
			{
				return (ImageList)base.Component;
			}
		}

		private Size ImageSize
		{
			get
			{
				return this.ImageList.ImageSize;
			}
			set
			{
				this.ImageList.Images.Clear();
				this.ImageList.ImageSize = value;
				this.Images.PopulateHandle();
			}
		}

		private bool ShouldSerializeImageSize()
		{
			return this.Images.Count == 0;
		}

		private Color TransparentColor
		{
			get
			{
				return this.ImageList.TransparentColor;
			}
			set
			{
				this.ImageList.Images.Clear();
				this.ImageList.TransparentColor = value;
				this.Images.PopulateHandle();
			}
		}

		private bool ShouldSerializeTransparentColor()
		{
			return !this.TransparentColor.Equals(Color.LightGray);
		}

		private ImageListStreamer ImageStream
		{
			get
			{
				return this.ImageList.ImageStream;
			}
			set
			{
				this.ImageList.ImageStream = value;
				this.Images.ReloadFromImageList();
			}
		}

		protected override void PreFilterProperties(IDictionary properties)
		{
			base.PreFilterProperties(properties);
			string[] array = new string[] { "ColorDepth", "ImageSize", "ImageStream", "TransparentColor" };
			Attribute[] array2 = new Attribute[0];
			for (int i = 0; i < array.Length; i++)
			{
				PropertyDescriptor propertyDescriptor = (PropertyDescriptor)properties[array[i]];
				if (propertyDescriptor != null)
				{
					properties[array[i]] = TypeDescriptor.CreateProperty(typeof(ImageListDesigner), propertyDescriptor, array2);
				}
			}
			PropertyDescriptor propertyDescriptor2 = (PropertyDescriptor)properties["Images"];
			if (propertyDescriptor2 != null)
			{
				Attribute[] array3 = new Attribute[propertyDescriptor2.Attributes.Count];
				propertyDescriptor2.Attributes.CopyTo(array3, 0);
				properties["Images"] = TypeDescriptor.CreateProperty(typeof(ImageListDesigner), "Images", typeof(ImageListDesigner.OriginalImageCollection), array3);
			}
		}

		public override DesignerActionListCollection ActionLists
		{
			get
			{
				if (this._actionLists == null)
				{
					this._actionLists = new DesignerActionListCollection();
					this._actionLists.Add(new ImageListActionList(this));
				}
				return this._actionLists;
			}
		}

		private ImageListDesigner.OriginalImageCollection originalImageCollection;

		private DesignerActionListCollection _actionLists;

		[Editor("System.Windows.Forms.Design.ImageCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
		internal class OriginalImageCollection : IList, ICollection, IEnumerable
		{
			internal OriginalImageCollection(ImageListDesigner owner)
			{
				this.owner = owner;
				this.ReloadFromImageList();
			}

			private void AssertInvariant()
			{
			}

			public int Count
			{
				get
				{
					this.AssertInvariant();
					return this.list.Count;
				}
			}

			public bool IsReadOnly
			{
				get
				{
					return false;
				}
			}

			bool IList.IsFixedSize
			{
				get
				{
					return false;
				}
			}

			[Browsable(false)]
			[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
			public ImageListImage this[int index]
			{
				get
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException(SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					return (ImageListImage)this.list[index];
				}
				set
				{
					if (index < 0 || index >= this.Count)
					{
						throw new ArgumentOutOfRangeException(SR.GetString("InvalidArgument", new object[]
						{
							"index",
							index.ToString(CultureInfo.CurrentCulture)
						}));
					}
					if (value == null)
					{
						throw new ArgumentException(SR.GetString("InvalidArgument", new object[] { "value", "null" }));
					}
					this.AssertInvariant();
					this.list[index] = value;
					this.RecreateHandle();
				}
			}

			object IList.this[int index]
			{
				get
				{
					return this[index];
				}
				set
				{
					if (value is ImageListImage)
					{
						this[index] = (ImageListImage)value;
						return;
					}
					throw new ArgumentException(SR.GetString("ImageListDesignerBadImageListImage", new object[] { "value" }));
				}
			}

			public void SetKeyName(int index, string name)
			{
				this[index].Name = name;
				this.owner.ImageList.Images.SetKeyName(index, name);
			}

			public int Add(ImageListImage value)
			{
				int num = this.list.Add(value);
				if (value.Name != null)
				{
					this.owner.ImageList.Images.Add(value.Name, value.Image);
				}
				else
				{
					this.owner.ImageList.Images.Add(value.Image);
				}
				return num;
			}

			public void AddRange(ImageListImage[] values)
			{
				if (values == null)
				{
					throw new ArgumentNullException("values");
				}
				foreach (ImageListImage imageListImage in values)
				{
					if (imageListImage != null)
					{
						this.Add(imageListImage);
					}
				}
			}

			int IList.Add(object value)
			{
				if (value is ImageListImage)
				{
					return this.Add((ImageListImage)value);
				}
				throw new ArgumentException(SR.GetString("ImageListDesignerBadImageListImage", new object[] { "value" }));
			}

			internal void ReloadFromImageList()
			{
				this.list.Clear();
				StringCollection keys = this.owner.ImageList.Images.Keys;
				for (int i = 0; i < this.owner.ImageList.Images.Count; i++)
				{
					this.list.Add(new ImageListImage((Bitmap)this.owner.ImageList.Images[i], keys[i]));
				}
			}

			public void Clear()
			{
				this.AssertInvariant();
				this.list.Clear();
				this.owner.ImageList.Images.Clear();
			}

			public bool Contains(ImageListImage value)
			{
				return this.list.Contains(value.Image);
			}

			bool IList.Contains(object value)
			{
				return value is ImageListImage && this.Contains((ImageListImage)value);
			}

			public IEnumerator GetEnumerator()
			{
				return this.list.GetEnumerator();
			}

			public int IndexOf(Image value)
			{
				return this.list.IndexOf(value);
			}

			int IList.IndexOf(object value)
			{
				if (value is Image)
				{
					return this.IndexOf((Image)value);
				}
				return -1;
			}

			void IList.Insert(int index, object value)
			{
				throw new NotSupportedException();
			}

			internal void PopulateHandle()
			{
				for (int i = 0; i < this.list.Count; i++)
				{
					ImageListImage imageListImage = (ImageListImage)this.list[i];
					this.owner.ImageList.Images.Add(imageListImage.Name, imageListImage.Image);
				}
			}

			private void RecreateHandle()
			{
				this.owner.ImageList.Images.Clear();
				this.PopulateHandle();
			}

			public void Remove(Image value)
			{
				this.AssertInvariant();
				this.list.Remove(value);
				this.RecreateHandle();
			}

			void IList.Remove(object value)
			{
				if (value is Image)
				{
					this.Remove((Image)value);
				}
			}

			public void RemoveAt(int index)
			{
				if (index < 0 || index >= this.Count)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("InvalidArgument", new object[]
					{
						"index",
						index.ToString(CultureInfo.CurrentCulture)
					}));
				}
				this.AssertInvariant();
				this.list.RemoveAt(index);
				this.RecreateHandle();
			}

			int ICollection.Count
			{
				get
				{
					return this.Count;
				}
			}

			bool ICollection.IsSynchronized
			{
				get
				{
					return false;
				}
			}

			object ICollection.SyncRoot
			{
				get
				{
					return null;
				}
			}

			void ICollection.CopyTo(Array array, int index)
			{
				this.list.CopyTo(array, index);
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.GetEnumerator();
			}

			private ImageListDesigner owner;

			private IList list = new ArrayList();
		}
	}
}
