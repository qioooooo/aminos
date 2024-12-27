using System;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Text
{
	// Token: 0x02000401 RID: 1025
	internal class Normalization
	{
		// Token: 0x06002A61 RID: 10849 RVA: 0x00087A68 File Offset: 0x00086A68
		internal unsafe Normalization(NormalizationForm form, string strDataFile)
		{
			this.normalizationForm = form;
			if (!Normalization.nativeLoadNormalizationDLL())
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNormalizationForm"));
			}
			byte* globalizationResourceBytePtr = GlobalizationAssembly.GetGlobalizationResourceBytePtr(typeof(Normalization).Assembly, strDataFile);
			if (globalizationResourceBytePtr == null)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNormalizationForm"));
			}
			byte* ptr = Normalization.nativeNormalizationInitNormalization(form, globalizationResourceBytePtr);
			if (ptr == null)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
		}

		// Token: 0x06002A62 RID: 10850 RVA: 0x00087AE4 File Offset: 0x00086AE4
		internal static Normalization GetNormalization(NormalizationForm form)
		{
			if (form <= (NormalizationForm)13)
			{
				switch (form)
				{
				case NormalizationForm.FormC:
					return Normalization.GetFormC();
				case NormalizationForm.FormD:
					return Normalization.GetFormD();
				case (NormalizationForm)3:
				case (NormalizationForm)4:
					break;
				case NormalizationForm.FormKC:
					return Normalization.GetFormKC();
				case NormalizationForm.FormKD:
					return Normalization.GetFormKD();
				default:
					if (form == (NormalizationForm)13)
					{
						return Normalization.GetFormIDNA();
					}
					break;
				}
			}
			else
			{
				switch (form)
				{
				case (NormalizationForm)257:
					return Normalization.GetFormCDisallowUnassigned();
				case (NormalizationForm)258:
					return Normalization.GetFormDDisallowUnassigned();
				case (NormalizationForm)259:
				case (NormalizationForm)260:
					break;
				case (NormalizationForm)261:
					return Normalization.GetFormKCDisallowUnassigned();
				case (NormalizationForm)262:
					return Normalization.GetFormKDDisallowUnassigned();
				default:
					if (form == (NormalizationForm)269)
					{
						return Normalization.GetFormIDNADisallowUnassigned();
					}
					break;
				}
			}
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidNormalizationForm"));
		}

		// Token: 0x06002A63 RID: 10851 RVA: 0x00087B98 File Offset: 0x00086B98
		internal static Normalization GetFormC()
		{
			if (Normalization.NFC != null)
			{
				return Normalization.NFC;
			}
			Normalization.NFC = new Normalization(NormalizationForm.FormC, "normnfc.nlp");
			return Normalization.NFC;
		}

		// Token: 0x06002A64 RID: 10852 RVA: 0x00087BBC File Offset: 0x00086BBC
		internal static Normalization GetFormD()
		{
			if (Normalization.NFD != null)
			{
				return Normalization.NFD;
			}
			Normalization.NFD = new Normalization(NormalizationForm.FormD, "normnfd.nlp");
			return Normalization.NFD;
		}

		// Token: 0x06002A65 RID: 10853 RVA: 0x00087BE0 File Offset: 0x00086BE0
		internal static Normalization GetFormKC()
		{
			if (Normalization.NFKC != null)
			{
				return Normalization.NFKC;
			}
			Normalization.NFKC = new Normalization(NormalizationForm.FormKC, "normnfkc.nlp");
			return Normalization.NFKC;
		}

		// Token: 0x06002A66 RID: 10854 RVA: 0x00087C04 File Offset: 0x00086C04
		internal static Normalization GetFormKD()
		{
			if (Normalization.NFKD != null)
			{
				return Normalization.NFKD;
			}
			Normalization.NFKD = new Normalization(NormalizationForm.FormKD, "normnfkd.nlp");
			return Normalization.NFKD;
		}

		// Token: 0x06002A67 RID: 10855 RVA: 0x00087C28 File Offset: 0x00086C28
		internal static Normalization GetFormIDNA()
		{
			if (Normalization.IDNA != null)
			{
				return Normalization.IDNA;
			}
			Normalization.IDNA = new Normalization((NormalizationForm)13, "normidna.nlp");
			return Normalization.IDNA;
		}

		// Token: 0x06002A68 RID: 10856 RVA: 0x00087C4D File Offset: 0x00086C4D
		internal static Normalization GetFormCDisallowUnassigned()
		{
			if (Normalization.NFCDisallowUnassigned != null)
			{
				return Normalization.NFCDisallowUnassigned;
			}
			Normalization.NFCDisallowUnassigned = new Normalization((NormalizationForm)257, "normnfc.nlp");
			return Normalization.NFCDisallowUnassigned;
		}

		// Token: 0x06002A69 RID: 10857 RVA: 0x00087C75 File Offset: 0x00086C75
		internal static Normalization GetFormDDisallowUnassigned()
		{
			if (Normalization.NFDDisallowUnassigned != null)
			{
				return Normalization.NFDDisallowUnassigned;
			}
			Normalization.NFDDisallowUnassigned = new Normalization((NormalizationForm)258, "normnfd.nlp");
			return Normalization.NFDDisallowUnassigned;
		}

		// Token: 0x06002A6A RID: 10858 RVA: 0x00087C9D File Offset: 0x00086C9D
		internal static Normalization GetFormKCDisallowUnassigned()
		{
			if (Normalization.NFKCDisallowUnassigned != null)
			{
				return Normalization.NFKCDisallowUnassigned;
			}
			Normalization.NFKCDisallowUnassigned = new Normalization((NormalizationForm)261, "normnfkc.nlp");
			return Normalization.NFKCDisallowUnassigned;
		}

		// Token: 0x06002A6B RID: 10859 RVA: 0x00087CC5 File Offset: 0x00086CC5
		internal static Normalization GetFormKDDisallowUnassigned()
		{
			if (Normalization.NFKDDisallowUnassigned != null)
			{
				return Normalization.NFKDDisallowUnassigned;
			}
			Normalization.NFKDDisallowUnassigned = new Normalization((NormalizationForm)262, "normnfkd.nlp");
			return Normalization.NFKDDisallowUnassigned;
		}

		// Token: 0x06002A6C RID: 10860 RVA: 0x00087CED File Offset: 0x00086CED
		internal static Normalization GetFormIDNADisallowUnassigned()
		{
			if (Normalization.IDNADisallowUnassigned != null)
			{
				return Normalization.IDNADisallowUnassigned;
			}
			Normalization.IDNADisallowUnassigned = new Normalization((NormalizationForm)269, "normidna.nlp");
			return Normalization.IDNADisallowUnassigned;
		}

		// Token: 0x06002A6D RID: 10861 RVA: 0x00087D15 File Offset: 0x00086D15
		internal static bool IsNormalized(string strInput, NormalizationForm normForm)
		{
			return Normalization.GetNormalization(normForm).IsNormalized(strInput);
		}

		// Token: 0x06002A6E RID: 10862 RVA: 0x00087D24 File Offset: 0x00086D24
		private bool IsNormalized(string strInput)
		{
			if (strInput == null)
			{
				throw new ArgumentNullException(Environment.GetResourceString("ArgumentNull_String"), "strInput");
			}
			int num = 0;
			int num2 = Normalization.nativeNormalizationIsNormalizedString(this.normalizationForm, ref num, strInput, strInput.Length);
			int num3 = num;
			if (num3 == 0)
			{
				return (num2 & 1) == 1;
			}
			if (num3 == 8)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
			if (num3 == 1113)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequenceNoIndex"), "strInput");
			}
			throw new InvalidOperationException(Environment.GetResourceString("UnknownError_Num", new object[] { num }));
		}

		// Token: 0x06002A6F RID: 10863 RVA: 0x00087DBE File Offset: 0x00086DBE
		internal static string Normalize(string strInput, NormalizationForm normForm)
		{
			return Normalization.GetNormalization(normForm).Normalize(strInput);
		}

		// Token: 0x06002A70 RID: 10864 RVA: 0x00087DCC File Offset: 0x00086DCC
		internal string Normalize(string strInput)
		{
			if (strInput == null)
			{
				throw new ArgumentNullException("strInput", Environment.GetResourceString("ArgumentNull_String"));
			}
			int num = this.GuessLength(strInput);
			if (num == 0)
			{
				return string.Empty;
			}
			char[] array = null;
			int num2 = 122;
			while (num2 == 122)
			{
				array = new char[num];
				num = Normalization.nativeNormalizationNormalizeString(this.normalizationForm, ref num2, strInput, strInput.Length, array, array.Length);
				if (num2 != 0)
				{
					int num3 = num2;
					if (num3 <= 87)
					{
						if (num3 == 8)
						{
							throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
						}
						if (num3 != 87)
						{
						}
					}
					else
					{
						if (num3 == 122)
						{
							continue;
						}
						if (num3 == 1113)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidCharSequence", new object[] { num }), "strInput");
						}
					}
					throw new InvalidOperationException(Environment.GetResourceString("UnknownError_Num", new object[] { num2 }));
				}
			}
			return new string(array, 0, num);
		}

		// Token: 0x06002A71 RID: 10865 RVA: 0x00087EC8 File Offset: 0x00086EC8
		internal int GuessLength(string strInput)
		{
			if (strInput == null)
			{
				throw new ArgumentNullException("strInput", Environment.GetResourceString("ArgumentNull_String"));
			}
			int num = 0;
			int num2 = Normalization.nativeNormalizationNormalizeString(this.normalizationForm, ref num, strInput, strInput.Length, null, 0);
			if (num == 0)
			{
				return num2;
			}
			if (num == 8)
			{
				throw new OutOfMemoryException(Environment.GetResourceString("Arg_OutOfMemoryException"));
			}
			throw new InvalidOperationException(Environment.GetResourceString("UnknownError_Num", new object[] { num }));
		}

		// Token: 0x06002A72 RID: 10866
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern bool nativeLoadNormalizationDLL();

		// Token: 0x06002A73 RID: 10867
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeNormalizationNormalizeString(NormalizationForm NormForm, ref int iError, string lpSrcString, int cwSrcLength, char[] lpDstString, int cwDstLength);

		// Token: 0x06002A74 RID: 10868
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern int nativeNormalizationIsNormalizedString(NormalizationForm NormForm, ref int iError, string lpString, int cwLength);

		// Token: 0x06002A75 RID: 10869
		[MethodImpl(MethodImplOptions.InternalCall)]
		private unsafe static extern byte* nativeNormalizationInitNormalization(NormalizationForm NormForm, byte* pTableData);

		// Token: 0x0400149E RID: 5278
		private const int ERROR_SUCCESS = 0;

		// Token: 0x0400149F RID: 5279
		private const int ERROR_NOT_ENOUGH_MEMORY = 8;

		// Token: 0x040014A0 RID: 5280
		private const int ERROR_INVALID_PARAMETER = 87;

		// Token: 0x040014A1 RID: 5281
		private const int ERROR_INSUFFICIENT_BUFFER = 122;

		// Token: 0x040014A2 RID: 5282
		private const int ERROR_NO_UNICODE_TRANSLATION = 1113;

		// Token: 0x040014A3 RID: 5283
		private static Normalization NFC;

		// Token: 0x040014A4 RID: 5284
		private static Normalization NFD;

		// Token: 0x040014A5 RID: 5285
		private static Normalization NFKC;

		// Token: 0x040014A6 RID: 5286
		private static Normalization NFKD;

		// Token: 0x040014A7 RID: 5287
		private static Normalization IDNA;

		// Token: 0x040014A8 RID: 5288
		private static Normalization NFCDisallowUnassigned;

		// Token: 0x040014A9 RID: 5289
		private static Normalization NFDDisallowUnassigned;

		// Token: 0x040014AA RID: 5290
		private static Normalization NFKCDisallowUnassigned;

		// Token: 0x040014AB RID: 5291
		private static Normalization NFKDDisallowUnassigned;

		// Token: 0x040014AC RID: 5292
		private static Normalization IDNADisallowUnassigned;

		// Token: 0x040014AD RID: 5293
		private NormalizationForm normalizationForm;
	}
}
