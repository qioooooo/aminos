using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Drawing.Internal;
using System.Runtime.InteropServices;

namespace System.Drawing
{
	// Token: 0x02000057 RID: 87
	public sealed class Pen : MarshalByRefObject, ISystemColorTracker, ICloneable, IDisposable
	{
		// Token: 0x060004A9 RID: 1193 RVA: 0x0001396F File Offset: 0x0001296F
		private Pen(IntPtr nativePen)
		{
			this.SetNativePen(nativePen);
		}

		// Token: 0x060004AA RID: 1194 RVA: 0x0001397E File Offset: 0x0001297E
		internal Pen(Color color, bool immutable)
			: this(color)
		{
			this.immutable = immutable;
		}

		// Token: 0x060004AB RID: 1195 RVA: 0x0001398E File Offset: 0x0001298E
		public Pen(Color color)
			: this(color, 1f)
		{
		}

		// Token: 0x060004AC RID: 1196 RVA: 0x0001399C File Offset: 0x0001299C
		public Pen(Color color, float width)
		{
			this.color = color;
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipCreatePen1(color.ToArgb(), width, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativePen(zero);
			if (this.color.IsSystemColor)
			{
				SystemColorTracker.Add(this);
			}
		}

		// Token: 0x060004AD RID: 1197 RVA: 0x000139F1 File Offset: 0x000129F1
		public Pen(Brush brush)
			: this(brush, 1f)
		{
		}

		// Token: 0x060004AE RID: 1198 RVA: 0x00013A00 File Offset: 0x00012A00
		public Pen(Brush brush, float width)
		{
			IntPtr zero = IntPtr.Zero;
			if (brush == null)
			{
				throw new ArgumentNullException("brush");
			}
			int num = SafeNativeMethods.Gdip.GdipCreatePen2(new HandleRef(brush, brush.NativeBrush), width, 0, out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.SetNativePen(zero);
		}

		// Token: 0x060004AF RID: 1199 RVA: 0x00013A4E File Offset: 0x00012A4E
		internal void SetNativePen(IntPtr nativePen)
		{
			if (nativePen == IntPtr.Zero)
			{
				throw new ArgumentNullException("nativePen");
			}
			this.nativePen = nativePen;
		}

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x060004B0 RID: 1200 RVA: 0x00013A6F File Offset: 0x00012A6F
		[EditorBrowsable(EditorBrowsableState.Never)]
		[Browsable(false)]
		internal IntPtr NativePen
		{
			get
			{
				return this.nativePen;
			}
		}

		// Token: 0x060004B1 RID: 1201 RVA: 0x00013A78 File Offset: 0x00012A78
		public object Clone()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipClonePen(new HandleRef(this, this.NativePen), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return new Pen(zero);
		}

		// Token: 0x060004B2 RID: 1202 RVA: 0x00013AAF File Offset: 0x00012AAF
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060004B3 RID: 1203 RVA: 0x00013AC0 File Offset: 0x00012AC0
		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				this.immutable = false;
			}
			else if (this.immutable)
			{
				throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Brush" }));
			}
			if (this.nativePen != IntPtr.Zero)
			{
				try
				{
					SafeNativeMethods.Gdip.GdipDeletePen(new HandleRef(this, this.NativePen));
				}
				catch (Exception ex)
				{
					if (ClientUtils.IsSecurityOrCriticalException(ex))
					{
						throw;
					}
				}
				finally
				{
					this.nativePen = IntPtr.Zero;
				}
			}
		}

		// Token: 0x060004B4 RID: 1204 RVA: 0x00013B60 File Offset: 0x00012B60
		~Pen()
		{
			this.Dispose(false);
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00013B90 File Offset: 0x00012B90
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x00013BC8 File Offset: 0x00012BC8
		public float Width
		{
			get
			{
				float[] array = new float[1];
				float[] array2 = array;
				int num = SafeNativeMethods.Gdip.GdipGetPenWidth(new HandleRef(this, this.NativePen), array2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array2[0];
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenWidth(new HandleRef(this, this.NativePen), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00013C1C File Offset: 0x00012C1C
		public void SetLineCap(LineCap startCap, LineCap endCap, DashCap dashCap)
		{
			if (this.immutable)
			{
				throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
			}
			int num = SafeNativeMethods.Gdip.GdipSetPenLineCap197819(new HandleRef(this, this.NativePen), (int)startCap, (int)endCap, (int)dashCap);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x060004B8 RID: 1208 RVA: 0x00013C70 File Offset: 0x00012C70
		// (set) Token: 0x060004B9 RID: 1209 RVA: 0x00013CA0 File Offset: 0x00012CA0
		public LineCap StartCap
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPenStartCap(new HandleRef(this, this.NativePen), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (LineCap)num;
			}
			set
			{
				if (value <= LineCap.ArrowAnchor)
				{
					switch (value)
					{
					case LineCap.Flat:
					case LineCap.Square:
					case LineCap.Round:
					case LineCap.Triangle:
						goto IL_0062;
					default:
						switch (value)
						{
						case LineCap.NoAnchor:
						case LineCap.SquareAnchor:
						case LineCap.RoundAnchor:
						case LineCap.DiamondAnchor:
						case LineCap.ArrowAnchor:
							goto IL_0062;
						}
						break;
					}
				}
				else if (value == LineCap.AnchorMask || value == LineCap.Custom)
				{
					goto IL_0062;
				}
				throw new InvalidEnumArgumentException("value", (int)value, typeof(LineCap));
				IL_0062:
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenStartCap(new HandleRef(this, this.NativePen), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x060004BA RID: 1210 RVA: 0x00013D54 File Offset: 0x00012D54
		// (set) Token: 0x060004BB RID: 1211 RVA: 0x00013D84 File Offset: 0x00012D84
		public LineCap EndCap
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPenEndCap(new HandleRef(this, this.NativePen), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (LineCap)num;
			}
			set
			{
				if (value <= LineCap.ArrowAnchor)
				{
					switch (value)
					{
					case LineCap.Flat:
					case LineCap.Square:
					case LineCap.Round:
					case LineCap.Triangle:
						goto IL_0062;
					default:
						switch (value)
						{
						case LineCap.NoAnchor:
						case LineCap.SquareAnchor:
						case LineCap.RoundAnchor:
						case LineCap.DiamondAnchor:
						case LineCap.ArrowAnchor:
							goto IL_0062;
						}
						break;
					}
				}
				else if (value == LineCap.AnchorMask || value == LineCap.Custom)
				{
					goto IL_0062;
				}
				throw new InvalidEnumArgumentException("value", (int)value, typeof(LineCap));
				IL_0062:
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenEndCap(new HandleRef(this, this.NativePen), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x00013E38 File Offset: 0x00012E38
		// (set) Token: 0x060004BD RID: 1213 RVA: 0x00013E68 File Offset: 0x00012E68
		public DashCap DashCap
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPenDashCap197819(new HandleRef(this, this.NativePen), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (DashCap)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid_NotSequential(value, (int)value, new int[] { 0, 2, 3 }))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DashCap));
				}
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenDashCap197819(new HandleRef(this, this.NativePen), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x060004BE RID: 1214 RVA: 0x00013EF0 File Offset: 0x00012EF0
		// (set) Token: 0x060004BF RID: 1215 RVA: 0x00013F20 File Offset: 0x00012F20
		public LineJoin LineJoin
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPenLineJoin(new HandleRef(this, this.NativePen), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (LineJoin)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 3))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(LineJoin));
				}
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenLineJoin(new HandleRef(this, this.NativePen), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x060004C0 RID: 1216 RVA: 0x00013F98 File Offset: 0x00012F98
		// (set) Token: 0x060004C1 RID: 1217 RVA: 0x00013FD0 File Offset: 0x00012FD0
		public CustomLineCap CustomStartCap
		{
			get
			{
				IntPtr zero = IntPtr.Zero;
				int num = SafeNativeMethods.Gdip.GdipGetPenCustomStartCap(new HandleRef(this, this.NativePen), out zero);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return CustomLineCap.CreateCustomLineCapObject(zero);
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenCustomStartCap(new HandleRef(this, this.NativePen), new HandleRef(value, (value == null) ? IntPtr.Zero : value.nativeCap));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x060004C2 RID: 1218 RVA: 0x0001403C File Offset: 0x0001303C
		// (set) Token: 0x060004C3 RID: 1219 RVA: 0x00014074 File Offset: 0x00013074
		public CustomLineCap CustomEndCap
		{
			get
			{
				IntPtr zero = IntPtr.Zero;
				int num = SafeNativeMethods.Gdip.GdipGetPenCustomEndCap(new HandleRef(this, this.NativePen), out zero);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return CustomLineCap.CreateCustomLineCapObject(zero);
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenCustomEndCap(new HandleRef(this, this.NativePen), new HandleRef(value, (value == null) ? IntPtr.Zero : value.nativeCap));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x060004C4 RID: 1220 RVA: 0x000140E0 File Offset: 0x000130E0
		// (set) Token: 0x060004C5 RID: 1221 RVA: 0x00014118 File Offset: 0x00013118
		public float MiterLimit
		{
			get
			{
				float[] array = new float[1];
				float[] array2 = array;
				int num = SafeNativeMethods.Gdip.GdipGetPenMiterLimit(new HandleRef(this, this.NativePen), array2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array2[0];
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenMiterLimit(new HandleRef(this, this.NativePen), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x060004C6 RID: 1222 RVA: 0x0001416C File Offset: 0x0001316C
		// (set) Token: 0x060004C7 RID: 1223 RVA: 0x0001419C File Offset: 0x0001319C
		public PenAlignment Alignment
		{
			get
			{
				PenAlignment penAlignment = PenAlignment.Center;
				int num = SafeNativeMethods.Gdip.GdipGetPenMode(new HandleRef(this, this.NativePen), out penAlignment);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return penAlignment;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 4))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(PenAlignment));
				}
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenMode(new HandleRef(this, this.NativePen), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x060004C8 RID: 1224 RVA: 0x00014214 File Offset: 0x00013214
		// (set) Token: 0x060004C9 RID: 1225 RVA: 0x00014250 File Offset: 0x00013250
		public Matrix Transform
		{
			get
			{
				Matrix matrix = new Matrix();
				int num = SafeNativeMethods.Gdip.GdipGetPenTransform(new HandleRef(this, this.NativePen), new HandleRef(matrix, matrix.nativeMatrix));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return matrix;
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenTransform(new HandleRef(this, this.NativePen), new HandleRef(value, value.nativeMatrix));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x060004CA RID: 1226 RVA: 0x000142BC File Offset: 0x000132BC
		public void ResetTransform()
		{
			int num = SafeNativeMethods.Gdip.GdipResetPenTransform(new HandleRef(this, this.NativePen));
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060004CB RID: 1227 RVA: 0x000142E5 File Offset: 0x000132E5
		public void MultiplyTransform(Matrix matrix)
		{
			this.MultiplyTransform(matrix, MatrixOrder.Prepend);
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x000142F0 File Offset: 0x000132F0
		public void MultiplyTransform(Matrix matrix, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipMultiplyPenTransform(new HandleRef(this, this.NativePen), new HandleRef(matrix, matrix.nativeMatrix), order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x00014326 File Offset: 0x00013326
		public void TranslateTransform(float dx, float dy)
		{
			this.TranslateTransform(dx, dy, MatrixOrder.Prepend);
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x00014334 File Offset: 0x00013334
		public void TranslateTransform(float dx, float dy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipTranslatePenTransform(new HandleRef(this, this.NativePen), dx, dy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x00014360 File Offset: 0x00013360
		public void ScaleTransform(float sx, float sy)
		{
			this.ScaleTransform(sx, sy, MatrixOrder.Prepend);
		}

		// Token: 0x060004D0 RID: 1232 RVA: 0x0001436C File Offset: 0x0001336C
		public void ScaleTransform(float sx, float sy, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipScalePenTransform(new HandleRef(this, this.NativePen), sx, sy, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060004D1 RID: 1233 RVA: 0x00014398 File Offset: 0x00013398
		public void RotateTransform(float angle)
		{
			this.RotateTransform(angle, MatrixOrder.Prepend);
		}

		// Token: 0x060004D2 RID: 1234 RVA: 0x000143A4 File Offset: 0x000133A4
		public void RotateTransform(float angle, MatrixOrder order)
		{
			int num = SafeNativeMethods.Gdip.GdipRotatePenTransform(new HandleRef(this, this.NativePen), angle, order);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
		}

		// Token: 0x060004D3 RID: 1235 RVA: 0x000143D0 File Offset: 0x000133D0
		private void InternalSetColor(Color value)
		{
			int num = SafeNativeMethods.Gdip.GdipSetPenColor(new HandleRef(this, this.NativePen), this.color.ToArgb());
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			this.color = value;
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001440C File Offset: 0x0001340C
		public PenType PenType
		{
			get
			{
				int num = -1;
				int num2 = SafeNativeMethods.Gdip.GdipGetPenFillType(new HandleRef(this, this.NativePen), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (PenType)num;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x060004D5 RID: 1237 RVA: 0x0001443C File Offset: 0x0001343C
		// (set) Token: 0x060004D6 RID: 1238 RVA: 0x00014490 File Offset: 0x00013490
		public Color Color
		{
			get
			{
				if (this.color == Color.Empty)
				{
					int num = 0;
					int num2 = SafeNativeMethods.Gdip.GdipGetPenColor(new HandleRef(this, this.NativePen), out num);
					if (num2 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num2);
					}
					this.color = Color.FromArgb(num);
				}
				return this.color;
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				if (value != this.color)
				{
					Color color = this.color;
					this.color = value;
					this.InternalSetColor(value);
					if (value.IsSystemColor && !color.IsSystemColor)
					{
						SystemColorTracker.Add(this);
					}
				}
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x060004D7 RID: 1239 RVA: 0x00014500 File Offset: 0x00013500
		// (set) Token: 0x060004D8 RID: 1240 RVA: 0x00014578 File Offset: 0x00013578
		public Brush Brush
		{
			get
			{
				Brush brush = null;
				switch (this.PenType)
				{
				case PenType.SolidColor:
					brush = new SolidBrush(this.GetNativeBrush());
					break;
				case PenType.HatchFill:
					brush = new HatchBrush(this.GetNativeBrush());
					break;
				case PenType.TextureFill:
					brush = new TextureBrush(this.GetNativeBrush());
					break;
				case PenType.PathGradient:
					brush = new PathGradientBrush(this.GetNativeBrush());
					break;
				case PenType.LinearGradient:
					brush = new LinearGradientBrush(this.GetNativeBrush());
					break;
				}
				return brush;
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenBrushFill(new HandleRef(this, this.NativePen), new HandleRef(value, value.NativeBrush));
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x000145E4 File Offset: 0x000135E4
		private IntPtr GetNativeBrush()
		{
			IntPtr zero = IntPtr.Zero;
			int num = SafeNativeMethods.Gdip.GdipGetPenBrushFill(new HandleRef(this, this.NativePen), out zero);
			if (num != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num);
			}
			return zero;
		}

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x060004DA RID: 1242 RVA: 0x00014618 File Offset: 0x00013618
		// (set) Token: 0x060004DB RID: 1243 RVA: 0x00014648 File Offset: 0x00013648
		public DashStyle DashStyle
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPenDashStyle(new HandleRef(this, this.NativePen), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return (DashStyle)num;
			}
			set
			{
				if (!ClientUtils.IsEnumValid(value, (int)value, 0, 5))
				{
					throw new InvalidEnumArgumentException("value", (int)value, typeof(DashStyle));
				}
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenDashStyle(new HandleRef(this, this.NativePen), (int)value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				if (value == DashStyle.Custom)
				{
					this.EnsureValidDashPattern();
				}
			}
		}

		// Token: 0x060004DC RID: 1244 RVA: 0x000146CC File Offset: 0x000136CC
		private void EnsureValidDashPattern()
		{
			int num = 0;
			int num2 = SafeNativeMethods.Gdip.GdipGetPenDashCount(new HandleRef(this, this.NativePen), out num);
			if (num2 != 0)
			{
				throw SafeNativeMethods.Gdip.StatusException(num2);
			}
			if (num == 0)
			{
				this.DashPattern = new float[] { 1f };
			}
		}

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x060004DD RID: 1245 RVA: 0x00014714 File Offset: 0x00013714
		// (set) Token: 0x060004DE RID: 1246 RVA: 0x0001474C File Offset: 0x0001374C
		public float DashOffset
		{
			get
			{
				float[] array = new float[1];
				float[] array2 = array;
				int num = SafeNativeMethods.Gdip.GdipGetPenDashOffset(new HandleRef(this, this.NativePen), array2);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
				return array2[0];
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenDashOffset(new HandleRef(this, this.NativePen), value);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x060004DF RID: 1247 RVA: 0x000147A0 File Offset: 0x000137A0
		// (set) Token: 0x060004E0 RID: 1248 RVA: 0x00014824 File Offset: 0x00013824
		public float[] DashPattern
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPenDashCount(new HandleRef(this, this.NativePen), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				int num3 = num;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(4 * num3));
				num2 = SafeNativeMethods.Gdip.GdipGetPenDashArray(new HandleRef(this, this.NativePen), intPtr, num3);
				float[] array;
				try
				{
					if (num2 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num2);
					}
					array = new float[num3];
					Marshal.Copy(intPtr, array, 0, num3);
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
				return array;
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				if (value == null || value.Length == 0)
				{
					throw new ArgumentException(SR.GetString("InvalidDashPattern"));
				}
				int num = value.Length;
				IntPtr intPtr = Marshal.AllocHGlobal(checked(4 * num));
				try
				{
					Marshal.Copy(value, 0, intPtr, num);
					int num2 = SafeNativeMethods.Gdip.GdipSetPenDashArray(new HandleRef(this, this.NativePen), new HandleRef(intPtr, intPtr), num);
					if (num2 != 0)
					{
						throw SafeNativeMethods.Gdip.StatusException(num2);
					}
				}
				finally
				{
					Marshal.FreeHGlobal(intPtr);
				}
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x060004E1 RID: 1249 RVA: 0x000148CC File Offset: 0x000138CC
		// (set) Token: 0x060004E2 RID: 1250 RVA: 0x00014920 File Offset: 0x00013920
		public float[] CompoundArray
		{
			get
			{
				int num = 0;
				int num2 = SafeNativeMethods.Gdip.GdipGetPenCompoundCount(new HandleRef(this, this.NativePen), out num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				float[] array = new float[num];
				num2 = SafeNativeMethods.Gdip.GdipGetPenCompoundArray(new HandleRef(this, this.NativePen), array, num);
				if (num2 != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num2);
				}
				return array;
			}
			set
			{
				if (this.immutable)
				{
					throw new ArgumentException(SR.GetString("CantChangeImmutableObjects", new object[] { "Pen" }));
				}
				int num = SafeNativeMethods.Gdip.GdipSetPenCompoundArray(new HandleRef(this, this.NativePen), value, value.Length);
				if (num != 0)
				{
					throw SafeNativeMethods.Gdip.StatusException(num);
				}
			}
		}

		// Token: 0x060004E3 RID: 1251 RVA: 0x00014975 File Offset: 0x00013975
		void ISystemColorTracker.OnSystemColorChanged()
		{
			if (this.NativePen != IntPtr.Zero)
			{
				this.InternalSetColor(this.color);
			}
		}

		// Token: 0x040003CC RID: 972
		private IntPtr nativePen;

		// Token: 0x040003CD RID: 973
		private Color color;

		// Token: 0x040003CE RID: 974
		private bool immutable;
	}
}
