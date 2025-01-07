using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Aladdin.HASP.Internal;

namespace Aladdin.HASP
{
	public sealed class HaspFile : ICloneable, IDisposable, IComparable<HaspFile>
	{
		internal HaspFile()
		{
			this.init();
		}

		public HaspFile(HaspFileId fileId, Hasp other)
		{
			this.parent = other;
			this.init((int)fileId);
		}

		public HaspFile(int fileId, Hasp other)
		{
			this.parent = other;
			this.init(fileId);
		}

		public HaspFile(HaspFile other)
		{
			if (!(other == null))
			{
				this.parent = other.parent;
				this.init(other.getFileIdInt());
			}
		}

		~HaspFile()
		{
			this.Dispose(false);
		}

		private int handle
		{
			get
			{
				return this.parent.key.handle;
			}
		}

		public static implicit operator Hasp(HaspFile other)
		{
			Hasp hasp;
			if (other == null)
			{
				hasp = null;
			}
			else
			{
				hasp = other.parent;
			}
			return hasp;
		}

		public static bool CanWriteString(string value)
		{
			return value != null && (int)HaspFile.maxStringLength() >= value.Length;
		}

		private HaspFileId getFileId()
		{
			HaspFileId haspFileId;
			if (Enum.IsDefined(typeof(HaspFileId), (HaspFileId)this.pIntId))
			{
				haspFileId = (HaspFileId)this.pIntId;
			}
			else
			{
				haspFileId = HaspFileId.Custom;
			}
			return haspFileId;
		}

		private int getFileIdInt()
		{
			return this.pIntId;
		}

		public static int FilePosFromString(string value)
		{
			int num;
			if (value == null)
			{
				num = 0;
			}
			else
			{
				num = (HaspFile.CanWriteString(value) ? (value.Length + 2) : 0);
			}
			return num;
		}

		public HaspStatus FileSize(ref int size)
		{
			HaspStatus haspStatus = this.parent.Hasp_Prologue(this.parent.key);
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				size = 0;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.get_size(this.handle, this.pIntId, ref size);
			}
			return haspStatus2;
		}

		private void init()
		{
			this.init(65524);
		}

		private void init(int fileId)
		{
			this.ulFilePos = 0;
			if (this.parent == null)
			{
				this.pIntId = 0;
			}
			else
			{
				this.pIntId = fileId;
				if (this.parent.Feature.IsProgNum)
				{
					switch (fileId)
					{
					case 65524:
					case 65525:
						this.pIntId = 0;
						break;
					}
				}
				else
				{
					switch (fileId)
					{
					case 65520:
					case 65522:
						this.pIntId = 0;
						break;
					}
				}
			}
		}

		private static byte maxStringLength()
		{
			return byte.MaxValue;
		}

		public HaspFile Assign(HaspFile other)
		{
			HaspFile haspFile;
			if (other == null)
			{
				this.parent = null;
				this.init();
				haspFile = this;
			}
			else
			{
				this.parent = other.parent;
				this.init(other.getFileIdInt());
				haspFile = this;
			}
			return haspFile;
		}

		public HaspStatus Read(ref bool value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = false;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		public HaspStatus Read(ref byte value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = 0;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		public HaspStatus Read(byte[] buffer, int index, int count)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else if (buffer == null)
			{
				haspStatus2 = HaspStatus.InvalidParameter;
			}
			else if (0 == count)
			{
				haspStatus2 = HaspStatus.StatusOk;
			}
			else if (index + count > buffer.Length)
			{
				haspStatus2 = HaspStatus.InvalidParameter;
			}
			else if (index == 0 && count == buffer.Length)
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, buffer);
			}
			else
			{
				byte[] array = new byte[count];
				haspStatus = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, array);
				if (haspStatus != HaspStatus.StatusOk)
				{
					haspStatus2 = haspStatus;
				}
				else
				{
					array.CopyTo(buffer, index);
					haspStatus2 = haspStatus;
				}
			}
			return haspStatus2;
		}

		public HaspStatus Read(ref char value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = '\0';
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		public HaspStatus Read(ref double value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = 0.0;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		public HaspStatus Read(ref short value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = 0;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		public HaspStatus Read(ref int value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = 0;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		public HaspStatus Read(ref long value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = 0L;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		public HaspStatus Read(ref float value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = 0f;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		public HaspStatus Read(ref string value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = "";
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		[CLSCompliant(false)]
		public HaspStatus Read(ref ushort value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = 0;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		[CLSCompliant(false)]
		public HaspStatus Read(ref uint value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = 0U;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		[CLSCompliant(false)]
		public HaspStatus Read(ref ulong value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				value = 0UL;
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.read(this.handle, this.pIntId, this.ulFilePos, ref value);
			}
			return haspStatus2;
		}

		public int FilePos
		{
			get
			{
				return this.ulFilePos;
			}
			set
			{
				this.ulFilePos = value;
			}
		}

		public override string ToString()
		{
			HaspFileId haspFileId;
			if (Enum.IsDefined(typeof(HaspFileId), (HaspFileId)this.pIntId))
			{
				haspFileId = (HaspFileId)this.pIntId;
			}
			else
			{
				haspFileId = HaspFileId.Custom;
			}
			return haspFileId.ToString();
		}

		public HaspStatus Write(bool value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		public HaspStatus Write(byte value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		public HaspStatus Write(byte[] buffer, int index, int count)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else if (buffer == null)
			{
				haspStatus2 = HaspStatus.InvalidParameter;
			}
			else if (index + count > buffer.Length)
			{
				haspStatus2 = HaspStatus.InvalidParameter;
			}
			else if (index == 0)
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, buffer);
			}
			else
			{
				byte[] array = new byte[count];
				Array.Copy(buffer, index, array, 0, count);
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, array);
			}
			return haspStatus2;
		}

		public HaspStatus Write(char value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		public HaspStatus Write(double value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		public HaspStatus Write(short value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		public HaspStatus Write(int value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		public HaspStatus Write(long value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		public HaspStatus Write(float value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		public HaspStatus Write(string value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		[CLSCompliant(false)]
		public HaspStatus Write(ushort value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		[CLSCompliant(false)]
		public HaspStatus Write(uint value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		[CLSCompliant(false)]
		public HaspStatus Write(ulong value)
		{
			HaspStatus haspStatus = this.Hasp_Prologue();
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.write(this.handle, this.pIntId, this.ulFilePos, value);
			}
			return haspStatus2;
		}

		public HaspFileId FileId
		{
			get
			{
				HaspFileId haspFileId;
				if (Enum.IsDefined(typeof(HaspFileId), (HaspFileId)this.pIntId))
				{
					haspFileId = (HaspFileId)this.pIntId;
				}
				else
				{
					haspFileId = HaspFileId.Custom;
				}
				return haspFileId;
			}
		}

		[PermissionSet(SecurityAction.LinkDemand)]
		public static int TypeSize(Type type)
		{
			return Marshal.SizeOf(type);
		}

		public object Clone()
		{
			return new HaspFile(this);
		}

		private HaspStatus Hasp_Prologue()
		{
			return this.parent.Hasp_Prologue(this.parent.key);
		}

		public bool IsValid()
		{
			HaspFileId haspFileId;
			if (Enum.IsDefined(typeof(HaspFileId), (HaspFileId)this.pIntId))
			{
				haspFileId = (HaspFileId)this.pIntId;
			}
			else
			{
				haspFileId = HaspFileId.Custom;
			}
			return !this.isDisposed && this.parent.IsValid() && haspFileId != HaspFileId.None && (this.parent.key.IsLoggedIn || !this.parent.key.loggedOut);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.isDisposed)
			{
				this.parent = null;
			}
			this.isDisposed = true;
		}

		public bool IsLoggedIn()
		{
			return !this.isDisposed && this.IsValid() && this.parent.IsLoggedIn();
		}

		public int CompareTo(HaspFile other)
		{
			int num;
			if (other == null)
			{
				num = 1;
			}
			else if (this.parent == null)
			{
				num = -1;
			}
			else
			{
				try
				{
					if (other.parent == null)
					{
						return 1;
					}
				}
				catch (NullReferenceException)
				{
					return 1;
				}
				int num2 = this.parent.CompareTo(other.parent);
				if (num2 != 0)
				{
					num = num2;
				}
				else
				{
					num2 = this.pIntId.CompareTo(other.pIntId);
					if (num2 != 0)
					{
						num = num2;
					}
					else
					{
						num = this.ulFilePos.CompareTo(other.ulFilePos);
					}
				}
			}
			return num;
		}

		public int CompareTo(Hasp other)
		{
			int num;
			if (other == null)
			{
				num = 1;
			}
			else
			{
				num = this.parent.CompareTo(other);
			}
			return num;
		}

		public override bool Equals(object obj)
		{
			return this == obj;
		}

		public static bool operator ==(HaspFile left, object right)
		{
			bool flag;
			if (left == null && right == null)
			{
				flag = true;
			}
			else if (left == null || right == null)
			{
				flag = false;
			}
			else
			{
				HaspFile haspFile = right as HaspFile;
				if (haspFile != null)
				{
					flag = left.CompareTo(haspFile) == 0;
				}
				else
				{
					Hasp hasp = right as Hasp;
					flag = hasp != null && left.parent.CompareTo(hasp) == 0;
				}
			}
			return flag;
		}

		public static bool operator !=(HaspFile left, object right)
		{
			return !(left == right);
		}

		public override int GetHashCode()
		{
			return this.pIntId;
		}

		private Hasp parent;

		private int pIntId;

		private int ulFilePos;

		private bool isDisposed = false;
	}
}
