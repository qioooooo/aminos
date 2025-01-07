using System;
using System.Text;

namespace System.Xml
{
	internal class BufferBuilder
	{
		public int Length
		{
			get
			{
				return this.length;
			}
			set
			{
				if (value < 0 || value > this.length)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value == 0)
				{
					this.Clear();
					return;
				}
				this.SetLength(value);
			}
		}

		public void Append(char value)
		{
			if (this.length + 1 <= 65536)
			{
				if (this.stringBuilder == null)
				{
					this.stringBuilder = new StringBuilder();
				}
				this.stringBuilder.Append(value);
			}
			else
			{
				if (this.lastBuffer == null)
				{
					this.CreateBuffers();
				}
				if (this.lastBufferIndex == this.lastBuffer.Length)
				{
					this.AddBuffer();
				}
				this.lastBuffer[this.lastBufferIndex++] = value;
			}
			this.length++;
		}

		public void Append(char[] value)
		{
			this.Append(value, 0, value.Length);
		}

		public unsafe void Append(char[] value, int start, int count)
		{
			if (value == null)
			{
				if (start == 0 && count == 0)
				{
					return;
				}
				throw new ArgumentNullException("value");
			}
			else
			{
				if (count == 0)
				{
					return;
				}
				if (start < 0)
				{
					throw new ArgumentOutOfRangeException("start");
				}
				if (count < 0 || start + count > value.Length)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (this.length + count <= 65536)
				{
					if (this.stringBuilder == null)
					{
						this.stringBuilder = new StringBuilder((count < 16) ? 16 : count);
					}
					this.stringBuilder.Append(value, start, count);
					this.length += count;
					return;
				}
				fixed (char* ptr = &value[start])
				{
					this.AppendHelper(ptr, count);
				}
				return;
			}
		}

		public void Append(string value)
		{
			this.Append(value, 0, value.Length);
		}

		public unsafe void Append(string value, int start, int count)
		{
			if (value == null)
			{
				if (start == 0 && count == 0)
				{
					return;
				}
				throw new ArgumentNullException("value");
			}
			else
			{
				if (count == 0)
				{
					return;
				}
				if (start < 0)
				{
					throw new ArgumentOutOfRangeException("start");
				}
				if (count < 0 || start + count > value.Length)
				{
					throw new ArgumentOutOfRangeException("count");
				}
				if (this.length + count <= 65536)
				{
					if (this.stringBuilder == null)
					{
						this.stringBuilder = new StringBuilder(value, start, count, 0);
					}
					else
					{
						this.stringBuilder.Append(value, start, count);
					}
					this.length += count;
					return;
				}
				fixed (char* ptr = value)
				{
					this.AppendHelper(ptr + start, count);
				}
				return;
			}
		}

		public void Clear()
		{
			if (this.length <= 65536)
			{
				if (this.stringBuilder != null)
				{
					this.stringBuilder.Length = 0;
				}
			}
			else
			{
				if (this.lastBuffer != null)
				{
					this.ClearBuffers();
				}
				this.stringBuilder = null;
			}
			this.length = 0;
		}

		internal void ClearBuffers()
		{
			if (this.buffers != null)
			{
				for (int i = 0; i < this.buffersCount; i++)
				{
					this.Recycle(this.buffers[i]);
				}
				this.lastBuffer = null;
			}
			this.lastBufferIndex = 0;
			this.buffersCount = 0;
		}

		public override string ToString()
		{
			string text;
			if (this.length <= 65536 || (this.buffersCount == 1 && this.lastBufferIndex == 0))
			{
				text = ((this.stringBuilder != null) ? this.stringBuilder.ToString() : string.Empty);
			}
			else
			{
				if (this.stringBuilder == null)
				{
					this.stringBuilder = new StringBuilder(this.length);
				}
				else
				{
					this.stringBuilder.Capacity = this.length;
				}
				int num = this.length - this.stringBuilder.Length;
				for (int i = 0; i < this.buffersCount - 1; i++)
				{
					char[] buffer = this.buffers[i].buffer;
					this.stringBuilder.Append(buffer, 0, buffer.Length);
					num -= buffer.Length;
				}
				this.stringBuilder.Append(this.buffers[this.buffersCount - 1].buffer, 0, num);
				this.ClearBuffers();
				text = this.stringBuilder.ToString();
			}
			return text;
		}

		public string ToString(int startIndex, int len)
		{
			if (startIndex < 0 || startIndex >= this.length)
			{
				throw new ArgumentOutOfRangeException("startIndex");
			}
			if (len < 0 || startIndex + len > this.length)
			{
				throw new ArgumentOutOfRangeException("len");
			}
			if (this.length > 65536 && (this.buffersCount != 1 || this.lastBufferIndex != 0))
			{
				StringBuilder stringBuilder = new StringBuilder(len);
				if (this.stringBuilder != null)
				{
					if (startIndex < this.stringBuilder.Length)
					{
						if (len < this.stringBuilder.Length)
						{
							return this.stringBuilder.ToString(startIndex, len);
						}
						stringBuilder.Append(this.stringBuilder.ToString(startIndex, this.stringBuilder.Length));
						startIndex = 0;
					}
					else
					{
						startIndex -= this.stringBuilder.Length;
					}
				}
				int num = 0;
				while (num < this.buffersCount && startIndex >= this.buffers[num].buffer.Length)
				{
					startIndex -= this.buffers[num].buffer.Length;
					num++;
				}
				if (num < this.buffersCount)
				{
					int num2 = len;
					while (num < this.buffersCount && num2 > 0)
					{
						char[] buffer = this.buffers[num].buffer;
						int num3 = ((buffer.Length < num2) ? buffer.Length : num2);
						stringBuilder.Append(buffer, startIndex, num3);
						startIndex = 0;
						num2 -= num3;
						num++;
					}
				}
				return stringBuilder.ToString();
			}
			if (this.stringBuilder == null)
			{
				return string.Empty;
			}
			return this.stringBuilder.ToString(startIndex, len);
		}

		private void CreateBuffers()
		{
			if (this.buffers == null)
			{
				this.lastBuffer = new char[65536];
				this.buffers = new BufferBuilder.Buffer[4];
				this.buffers[0].buffer = this.lastBuffer;
				this.buffersCount = 1;
				return;
			}
			this.AddBuffer();
		}

		private unsafe void AppendHelper(char* pSource, int count)
		{
			if (this.lastBuffer == null)
			{
				this.CreateBuffers();
			}
			while (count > 0)
			{
				if (this.lastBufferIndex >= this.lastBuffer.Length)
				{
					this.AddBuffer();
				}
				int num = count;
				int num2 = this.lastBuffer.Length - this.lastBufferIndex;
				if (num2 < num)
				{
					num = num2;
				}
				fixed (char* ptr = &this.lastBuffer[this.lastBufferIndex])
				{
					BufferBuilder.wstrcpy(ptr, pSource, num);
				}
				pSource += num;
				this.length += num;
				this.lastBufferIndex += num;
				count -= num;
			}
		}

		private void AddBuffer()
		{
			if (this.buffersCount + 1 == this.buffers.Length)
			{
				BufferBuilder.Buffer[] array = new BufferBuilder.Buffer[this.buffers.Length * 2];
				Array.Copy(this.buffers, 0, array, 0, this.buffers.Length);
				this.buffers = array;
			}
			char[] array2;
			if (this.buffers[this.buffersCount].recycledBuffer != null)
			{
				array2 = (char[])this.buffers[this.buffersCount].recycledBuffer.Target;
				if (array2 != null)
				{
					this.buffers[this.buffersCount].recycledBuffer.Target = null;
					goto IL_00A4;
				}
			}
			array2 = new char[65536];
			IL_00A4:
			this.lastBuffer = array2;
			this.buffers[this.buffersCount++].buffer = array2;
			this.lastBufferIndex = 0;
		}

		private void Recycle(BufferBuilder.Buffer buf)
		{
			if (buf.recycledBuffer == null)
			{
				buf.recycledBuffer = new WeakReference(buf.buffer);
			}
			else
			{
				buf.recycledBuffer.Target = buf.buffer;
			}
			buf.buffer = null;
		}

		private void SetLength(int newLength)
		{
			if (newLength == this.length)
			{
				return;
			}
			if (this.length <= 65536)
			{
				this.stringBuilder.Length = newLength;
			}
			else
			{
				int num = newLength;
				int i = 0;
				while (i < this.buffersCount && num >= this.buffers[i].buffer.Length)
				{
					num -= this.buffers[i].buffer.Length;
					i++;
				}
				if (i < this.buffersCount)
				{
					this.lastBuffer = this.buffers[i].buffer;
					this.lastBufferIndex = num;
					i++;
					int num2 = i;
					while (i < this.buffersCount)
					{
						this.Recycle(this.buffers[i]);
						i++;
					}
					this.buffersCount = num2;
				}
			}
			this.length = newLength;
		}

		internal unsafe static void wstrcpy(char* dmem, char* smem, int charCount)
		{
			if (charCount > 0)
			{
				if (((dmem ^ smem) & 3) == 0)
				{
					while ((dmem & 3) != 0 && charCount > 0)
					{
						*dmem = *smem;
						dmem++;
						smem++;
						charCount--;
					}
					if (charCount >= 8)
					{
						charCount -= 8;
						do
						{
							*(int*)dmem = (int)(*(uint*)smem);
							*(int*)(dmem + 2) = (int)(*(uint*)(smem + 2));
							*(int*)(dmem + 4) = (int)(*(uint*)(smem + 4));
							*(int*)(dmem + 6) = (int)(*(uint*)(smem + 6));
							dmem += 8;
							smem += 8;
							charCount -= 8;
						}
						while (charCount >= 0);
					}
					if ((charCount & 4) != 0)
					{
						*(int*)dmem = (int)(*(uint*)smem);
						*(int*)(dmem + 2) = (int)(*(uint*)(smem + 2));
						dmem += 4;
						smem += 4;
					}
					if ((charCount & 2) != 0)
					{
						*(int*)dmem = (int)(*(uint*)smem);
						dmem += 2;
						smem += 2;
					}
				}
				else
				{
					if (charCount >= 8)
					{
						charCount -= 8;
						do
						{
							*dmem = *smem;
							dmem[1] = smem[1];
							dmem[2] = smem[2];
							dmem[3] = smem[3];
							dmem[4] = smem[4];
							dmem[5] = smem[5];
							dmem[6] = smem[6];
							dmem[7] = smem[7];
							dmem += 8;
							smem += 8;
							charCount -= 8;
						}
						while (charCount >= 0);
					}
					if ((charCount & 4) != 0)
					{
						*dmem = *smem;
						dmem[1] = smem[1];
						dmem[2] = smem[2];
						dmem[3] = smem[3];
						dmem += 4;
						smem += 4;
					}
					if ((charCount & 2) != 0)
					{
						*dmem = *smem;
						dmem[1] = smem[1];
						dmem += 2;
						smem += 2;
					}
				}
				if ((charCount & 1) != 0)
				{
					*dmem = *smem;
				}
			}
		}

		private const int BufferSize = 65536;

		private const int InitialBufferArrayLength = 4;

		private const int MaxStringBuilderLength = 65536;

		private const int DefaultSBCapacity = 16;

		private StringBuilder stringBuilder;

		private BufferBuilder.Buffer[] buffers;

		private int buffersCount;

		private char[] lastBuffer;

		private int lastBufferIndex;

		private int length;

		private struct Buffer
		{
			internal char[] buffer;

			internal WeakReference recycledBuffer;
		}
	}
}
