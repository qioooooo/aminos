using System;
using Aladdin.HASP.Internal;

namespace Aladdin.HASP
{
	public sealed class HaspLegacy : Hasp
	{
		public HaspLegacy(Hasp other)
			: base(other)
		{
		}

		public HaspLegacy(HaspLegacy other)
			: base(other)
		{
		}

		public new HaspStatus Decrypt(byte[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Decrypt(char[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Decrypt(double[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Decrypt(short[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Decrypt(int[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Decrypt(long[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Decrypt(float[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_decrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Decrypt(ref string data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_decrypt(this.key.handle, ref data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Encrypt(byte[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Encrypt(char[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Encrypt(double[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Encrypt(short[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Encrypt(int[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Encrypt(long[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Encrypt(float[] data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_encrypt(this.key.handle, data);
				}
			}
			return haspStatus;
		}

		public new HaspStatus Encrypt(ref string data)
		{
			HaspStatus haspStatus;
			if (data == null)
			{
				haspStatus = HaspStatus.InvalidParameter;
			}
			else
			{
				HaspStatus haspStatus2 = base.Hasp_Prologue(this.key);
				if (haspStatus2 != HaspStatus.StatusOk)
				{
					haspStatus = haspStatus2;
				}
				else
				{
					haspStatus = ApiDisp.legacy_encrypt(this.key.handle, ref data);
				}
			}
			return haspStatus;
		}

		public HaspStatus SetIdleTime(short idleTime)
		{
			HaspStatus haspStatus = base.Hasp_Prologue(this.key);
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				haspStatus2 = ApiDisp.legacy_set_idletime(this.key.handle, idleTime);
			}
			return haspStatus2;
		}

		public HaspStatus SetRtc(DateTime rtc)
		{
			HaspStatus haspStatus = base.Hasp_Prologue(this.key);
			HaspStatus haspStatus2;
			if (haspStatus != HaspStatus.StatusOk)
			{
				haspStatus2 = haspStatus;
			}
			else
			{
				DateTime dateTime = new DateTime(1970, 1, 1);
				haspStatus2 = ApiDisp.legacy_set_rtc(this.key.handle, (rtc.Ticks - dateTime.Ticks) / 10L / 1000L / 1000L);
			}
			return haspStatus2;
		}

		public new bool IsValid()
		{
			return base.IsValid() && base.HasLegacy && (this.key.IsLoggedIn || !this.key.loggedOut);
		}

		public new object Clone()
		{
			return new HaspLegacy(this);
		}

		public new HaspLegacy Assign(Hasp other)
		{
			base.Assign(other);
			return this;
		}
	}
}
