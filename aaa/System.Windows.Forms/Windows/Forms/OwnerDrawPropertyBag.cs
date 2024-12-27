using System;
using System.Drawing;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace System.Windows.Forms
{
	// Token: 0x020005AC RID: 1452
	[Serializable]
	public class OwnerDrawPropertyBag : MarshalByRefObject, ISerializable
	{
		// Token: 0x06004B31 RID: 19249 RVA: 0x00110834 File Offset: 0x0010F834
		protected OwnerDrawPropertyBag(SerializationInfo info, StreamingContext context)
		{
			foreach (SerializationEntry serializationEntry in info)
			{
				if (serializationEntry.Name == "Font")
				{
					this.font = (Font)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "ForeColor")
				{
					this.foreColor = (Color)serializationEntry.Value;
				}
				else if (serializationEntry.Name == "BackColor")
				{
					this.backColor = (Color)serializationEntry.Value;
				}
			}
		}

		// Token: 0x06004B32 RID: 19250 RVA: 0x001108EB File Offset: 0x0010F8EB
		internal OwnerDrawPropertyBag()
		{
		}

		// Token: 0x17000ED9 RID: 3801
		// (get) Token: 0x06004B33 RID: 19251 RVA: 0x00110909 File Offset: 0x0010F909
		// (set) Token: 0x06004B34 RID: 19252 RVA: 0x00110911 File Offset: 0x0010F911
		public Font Font
		{
			get
			{
				return this.font;
			}
			set
			{
				this.font = value;
			}
		}

		// Token: 0x17000EDA RID: 3802
		// (get) Token: 0x06004B35 RID: 19253 RVA: 0x0011091A File Offset: 0x0010F91A
		// (set) Token: 0x06004B36 RID: 19254 RVA: 0x00110922 File Offset: 0x0010F922
		public Color ForeColor
		{
			get
			{
				return this.foreColor;
			}
			set
			{
				this.foreColor = value;
			}
		}

		// Token: 0x17000EDB RID: 3803
		// (get) Token: 0x06004B37 RID: 19255 RVA: 0x0011092B File Offset: 0x0010F92B
		// (set) Token: 0x06004B38 RID: 19256 RVA: 0x00110933 File Offset: 0x0010F933
		public Color BackColor
		{
			get
			{
				return this.backColor;
			}
			set
			{
				this.backColor = value;
			}
		}

		// Token: 0x17000EDC RID: 3804
		// (get) Token: 0x06004B39 RID: 19257 RVA: 0x0011093C File Offset: 0x0010F93C
		internal IntPtr FontHandle
		{
			get
			{
				if (this.fontWrapper == null)
				{
					this.fontWrapper = new Control.FontHandleWrapper(this.Font);
				}
				return this.fontWrapper.Handle;
			}
		}

		// Token: 0x06004B3A RID: 19258 RVA: 0x00110962 File Offset: 0x0010F962
		public virtual bool IsEmpty()
		{
			return this.Font == null && this.foreColor.IsEmpty && this.backColor.IsEmpty;
		}

		// Token: 0x06004B3B RID: 19259 RVA: 0x00110988 File Offset: 0x0010F988
		public static OwnerDrawPropertyBag Copy(OwnerDrawPropertyBag value)
		{
			OwnerDrawPropertyBag ownerDrawPropertyBag2;
			lock (OwnerDrawPropertyBag.internalSyncObject)
			{
				OwnerDrawPropertyBag ownerDrawPropertyBag = new OwnerDrawPropertyBag();
				if (value == null)
				{
					ownerDrawPropertyBag2 = ownerDrawPropertyBag;
				}
				else
				{
					ownerDrawPropertyBag.backColor = value.backColor;
					ownerDrawPropertyBag.foreColor = value.foreColor;
					ownerDrawPropertyBag.Font = value.font;
					ownerDrawPropertyBag2 = ownerDrawPropertyBag;
				}
			}
			return ownerDrawPropertyBag2;
		}

		// Token: 0x06004B3C RID: 19260 RVA: 0x001109F0 File Offset: 0x0010F9F0
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		void ISerializable.GetObjectData(SerializationInfo si, StreamingContext context)
		{
			si.AddValue("BackColor", this.BackColor);
			si.AddValue("ForeColor", this.ForeColor);
			si.AddValue("Font", this.Font);
		}

		// Token: 0x040030F4 RID: 12532
		private Font font;

		// Token: 0x040030F5 RID: 12533
		private Color foreColor = Color.Empty;

		// Token: 0x040030F6 RID: 12534
		private Color backColor = Color.Empty;

		// Token: 0x040030F7 RID: 12535
		private Control.FontHandleWrapper fontWrapper;

		// Token: 0x040030F8 RID: 12536
		private static object internalSyncObject = new object();
	}
}
