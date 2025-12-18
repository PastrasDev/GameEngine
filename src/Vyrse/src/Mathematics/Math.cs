using System;
using System.Numerics;
using System.Runtime.CompilerServices;

public static partial class Math
{
    public static class Policies
    {
        public enum AngleUndefined
        {
            ReturnNaN,
            ReturnZero,
            ReturnNinety
        }
    }

    public static class Constants<T> where T : IFloatingPointIeee754<T>, IRootFunctions<T>
    {
        private static readonly bool IsFloat = typeof(T) == typeof(float);
        public static readonly T KEpsilon = IsFloat ? T.CreateChecked(1e-6f) : T.CreateChecked(1e-12d);
        public static readonly T KSqrEpsilon = KEpsilon * KEpsilon;
        public static readonly T KAlmostZero = IsFloat ? T.CreateChecked(1e-5f) : T.CreateChecked(1e-15d);
        public static readonly T KSqrAlmostZero = KAlmostZero * KAlmostZero;
        public static readonly T KRelativeEpsilon = IsFloat ? T.CreateChecked(1e-5f) : T.CreateChecked(1e-12d);
        public static readonly T KSqrRelativeEpsilon = KRelativeEpsilon * KRelativeEpsilon;
        public static readonly T PI = IsFloat ? T.CreateChecked(MathF.PI) : T.CreateChecked(System.Math.PI);
        public static readonly T Tau = IsFloat ? T.CreateChecked(MathF.Tau) : T.CreateChecked(System.Math.Tau);
        public static readonly T PI_Half = PI * T.CreateChecked(0.5);
        public static readonly T PI_Quarter = PI * T.CreateChecked(0.25);
        public static readonly T PI_SQR = PI * PI;
        public static readonly T PI_SQRT = T.Sqrt(PI);
        public static readonly T NegInfinity = T.NegativeInfinity;
        public static readonly T PosInfinity = T.PositiveInfinity;
        public static readonly T NaN = T.NaN;
        public static readonly T Deg2Rad = PI / T.CreateChecked(180);
        public static readonly T Rad2Deg = T.CreateChecked(180) / PI;
    }
    
    public static partial bool IsNaN<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial bool IsInfinite<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial bool IsFinite<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial bool IsZero<T>(T value) where T : INumber<T>;
    public static partial bool IsZero<T>(T value, T eps) where T : IFloatingPointIeee754<T>;
    public static partial bool Approx<T>(T a, T b, T absEps, T relEps) where T : IFloatingPointIeee754<T>;
    public static partial bool Approx<T>(T a, T b, T eps) where T : IFloatingPointIeee754<T>;
    public static partial bool Approx<T>(T a, T b) where T : IFloatingPointIeee754<T>;

    public static partial T Add<T>(T a, T b) where T : INumber<T>;
    public static partial T Sub<T>(T a, T b) where T : INumber<T>;
    public static partial T Mul<T>(T a, T b) where T : INumber<T>;
    public static partial T Div<T>(T a, T b) where T : INumber<T>;

    public static partial T Lerp<T>(T a, T b, T t) where T : INumber<T>;
    public static partial T Unlerp<T>(T a, T b, T v) where T : INumber<T>;
    public static partial T Smoothstep<T>(T a, T b, T v) where T : INumber<T>;
    public static partial T SmoothstepUnclamped<T>(T a, T b, T v) where T : INumber<T>;
    public static partial T Remap<T>(T inMin, T inMax, T outMin, T outMax, T value) where T : INumber<T>;

    public static partial T Sign<T>(T value) where T : INumber<T>;
    public static partial T CopySign<T>(T x, T y) where T : INumber<T>;
    public static partial T Abs<T>(T value) where T : INumber<T>;
    public static partial T Floor<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial T Ceil<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial T Round<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial T Negate<T>(T value) where T : INumber<T>;
    public static partial T Exponent<T>(T value) where T : IFloatingPointIeee754<T>;

    public static partial T Min<T>(T a, T b) where T : INumber<T>;
    public static partial T Max<T>(T a, T b) where T : INumber<T>;
    public static partial T Clamp<T>(T value, T min, T max) where T : INumber<T>;
    public static partial T Saturate<T>(T value) where T : INumber<T>;
    public static partial T OneMinus<T>(T value) where T : INumber<T>;

    public static partial T Sin<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial T Cos<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial T Tan<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial T Asin<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial T Acos<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial T Atan<T>(T value) where T : IFloatingPointIeee754<T>;
    public static partial T Atan2<T>(T y, T x) where T : IFloatingPointIeee754<T>;
    public static partial T Degrees<T>(T radians) where T : IFloatingPointIeee754<T>;
    public static partial T Radians<T>(T degrees) where T : IFloatingPointIeee754<T>;
    public static partial T Sqrt<T>(T value) where T : IRootFunctions<T>;
    public static partial T RSqrt<T>(T value) where T : IRootFunctions<T>;
    public static partial T Sqr<T>(T value) where T : IRootFunctions<T>;

    public static partial T Mod<T>(T a, T b) where T : INumber<T>;
    public static partial T Select<T>(T falseValue, T trueValue, bool test) where T : INumber<T>;
}

public static partial class Math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial bool IsNaN<T>(T value) where T : IFloatingPointIeee754<T> => T.IsNaN(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial bool IsInfinite<T>(T value) where T : IFloatingPointIeee754<T> => T.IsInfinity(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial bool IsFinite<T>(T value) where T : IFloatingPointIeee754<T> => T.IsFinite(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial bool IsZero<T>(T value) where T : INumber<T> => T.IsZero(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial bool IsZero<T>(T value, T eps) where T : IFloatingPointIeee754<T> => Abs(value) <= eps;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial bool Approx<T>(T a, T b, T absEps, T relEps) where T : IFloatingPointIeee754<T> => (T.Abs(a - b) <= T.Max(absEps, relEps * T.Max(T.Abs(a), T.Abs(b)))) & !T.IsNaN(a) & !T.IsNaN(b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial bool Approx<T>(T a, T b, T eps) where T : IFloatingPointIeee754<T> => Approx(a, b, eps, eps);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial bool Approx<T>(T a, T b) where T : IFloatingPointIeee754<T> => Approx(a, b, Constants<T>.KEpsilon, Constants<T>.KRelativeEpsilon);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Add<T>(T a, T b) where T : INumber<T> => a + b;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Sub<T>(T a, T b) where T : INumber<T> => a - b;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Mul<T>(T a, T b) where T : INumber<T> => a * b;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Div<T>(T a, T b) where T : INumber<T> => a / b;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Lerp<T>(T a, T b, T t) where T : INumber<T> => a + (b - a) * t;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Unlerp<T>(T a, T b, T v) where T : INumber<T> => (v - a) / (b - a);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Smoothstep<T>(T a, T b, T v) where T : INumber<T> { var t = Clamp(Unlerp(a, b, v), T.Zero, T.One); return t * t * (T.CreateChecked(3) - T.CreateChecked(2) * t); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T SmoothstepUnclamped<T>(T a, T b, T v) where T : INumber<T> { var t = Unlerp(a, b, v); return t * t * (T.CreateChecked(3) - T.CreateChecked(2) * t); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Remap<T>(T inMin, T inMax, T outMin, T outMax, T value) where T : INumber<T> => Lerp(outMin, outMax, Clamp(Unlerp(inMin, inMax, value), T.Zero, T.One));

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Sign<T>(T value) where T : INumber<T> => T.CreateChecked(T.Sign(value));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T CopySign<T>(T x, T y) where T : INumber<T> => T.CreateChecked(T.CopySign(x, y));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Abs<T>(T value) where T : INumber<T> => T.Abs(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Floor<T>(T value) where T : IFloatingPointIeee754<T> => T.Floor(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Ceil<T>(T value) where T : IFloatingPointIeee754<T> => T.Ceiling(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Round<T>(T value) where T : IFloatingPointIeee754<T> => T.Round(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Negate<T>(T value) where T : INumber<T> => -value;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Exponent<T>(T value) where T : IFloatingPointIeee754<T> => T.CreateChecked(T.Exp(value));

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Min<T>(T a, T b) where T : INumber<T> => T.Min(a, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Max<T>(T a, T b) where T : INumber<T> => T.Max(a, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Clamp<T>(T value, T min, T max) where T : INumber<T> => T.Clamp(value, min, max);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Saturate<T>(T value) where T : INumber<T> => Clamp(value, T.Zero, T.One);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T OneMinus<T>(T value) where T : INumber<T> => T.One - value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Sin<T>(T value) where T : IFloatingPointIeee754<T> => T.Sin(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Cos<T>(T value) where T : IFloatingPointIeee754<T> => T.Cos(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Tan<T>(T value) where T : IFloatingPointIeee754<T> => T.Tan(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Asin<T>(T value) where T : IFloatingPointIeee754<T> => T.Asin(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Acos<T>(T value) where T : IFloatingPointIeee754<T> => T.Acos(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Atan<T>(T value) where T : IFloatingPointIeee754<T> => T.Atan(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Atan2<T>(T y, T x) where T : IFloatingPointIeee754<T> => T.Atan2(y, x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Degrees<T>(T radians) where T : IFloatingPointIeee754<T> => radians * Constants<T>.Rad2Deg;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Radians<T>(T degrees) where T : IFloatingPointIeee754<T> => degrees * Constants<T>.Deg2Rad;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Sqrt<T>(T value) where T : IRootFunctions<T> => T.Sqrt(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T RSqrt<T>(T value) where T : IRootFunctions<T> => T.One / Sqrt(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Sqr<T>(T value) where T : IRootFunctions<T> => value * value;

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Mod<T>(T a, T b) where T : INumber<T> { var r = a % b; return r < T.Zero ? r + (b < T.Zero ? -Abs(b) : Abs(b)) : r; }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static partial T Select<T>(T falseValue, T trueValue, bool test) where T : INumber<T> => test ? trueValue : falseValue;
}