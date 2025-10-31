using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Vyrse.Mathematics;

[PublicAPI]
public static partial class Math
{
    public static class Policies
    {
        public enum AngleUndefined { ReturnNaN, ReturnZero, ReturnNinety }
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
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isNaN<T>(T value) where T : IFloatingPointIeee754<T> => T.IsNaN(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isInfinite<T>(T value) where T : IFloatingPointIeee754<T> => T.IsInfinity(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isFinite<T>(T value) where T : IFloatingPointIeee754<T> => T.IsFinite(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isZero<T>(T value) where T : INumber<T> => T.IsZero(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isZero<T>(T value, T eps) where T : IFloatingPointIeee754<T> => abs(value) <= eps;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool approx<T>(T a, T b, T absEps, T relEps) where T : IFloatingPointIeee754<T> => (T.Abs(a - b) <= T.Max(absEps, relEps * T.Max(T.Abs(a), T.Abs(b)))) & !T.IsNaN(a) & !T.IsNaN(b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool approx<T>(T a, T b, T eps) where T : IFloatingPointIeee754<T> => approx(a, b, eps, eps);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool approx<T>(T a, T b) where T : IFloatingPointIeee754<T> => approx(a, b, Constants<T>.KEpsilon, Constants<T>.KRelativeEpsilon);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T add<T>(T a, T b) where T : INumber<T> => a + b;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T sub<T>(T a, T b) where T : INumber<T> => a - b;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T mul<T>(T a, T b) where T : INumber<T> => a * b;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T div<T>(T a, T b) where T : INumber<T> => a / b;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T lerp<T>(T a, T b, T t) where T : INumber<T> => a + (b - a) * t;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T unlerp<T>(T a, T b, T v) where T : INumber<T> => (v - a) / (b - a);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T smoothstep<T>(T a, T b, T v) where T : INumber<T> { var t = clamp(unlerp(a, b, v), T.Zero, T.One); return t * t * (T.CreateChecked(3) - T.CreateChecked(2) * t); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T smoothstepUnclamped<T>(T a, T b, T v) where T : INumber<T> { var t = unlerp(a, b, v); return t * t * (T.CreateChecked(3) - T.CreateChecked(2) * t); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T remap<T>(T inMin, T inMax, T outMin, T outMax, T value) where T : INumber<T> => lerp(outMin, outMax, clamp(unlerp(inMin, inMax, value), T.Zero, T.One));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T sign<T>(T value) where T : INumber<T> => T.CreateChecked(T.Sign(value));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T copySign<T>(T x, T y) where T : INumber<T> => T.CreateChecked(T.CopySign(x, y));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T abs<T>(T value) where T : INumber<T> => T.Abs(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T floor<T>(T value) where T : IFloatingPointIeee754<T> => T.Floor(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T ceil<T>(T value) where T : IFloatingPointIeee754<T> => T.Ceiling(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T round<T>(T value) where T : IFloatingPointIeee754<T> => T.Round(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T negate<T>(T value) where T : INumber<T> => -value;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T exponent<T>(T value) where T : IFloatingPointIeee754<T> => T.CreateChecked(T.Exp(value));
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T min<T>(T a, T b) where T : INumber<T> => T.Min(a, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T max<T>(T a, T b) where T : INumber<T> => T.Max(a, b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T clamp<T>(T value, T min, T max) where T : INumber<T> => T.Clamp(value, min, max);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T saturate<T>(T value) where T : INumber<T> => clamp(value, T.Zero, T.One);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T oneMinus<T>(T value) where T : INumber<T> => T.One - value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T sin<T>(T value) where T : IFloatingPointIeee754<T> => T.Sin(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T cos<T>(T value) where T : IFloatingPointIeee754<T> => T.Cos(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T tan<T>(T value) where T : IFloatingPointIeee754<T> => T.Tan(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T asin<T>(T value) where T : IFloatingPointIeee754<T> => T.Asin(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T acos<T>(T value) where T : IFloatingPointIeee754<T> => T.Acos(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T atan<T>(T value) where T : IFloatingPointIeee754<T> => T.Atan(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T atan2<T>(T y, T x) where T : IFloatingPointIeee754<T> => T.Atan2(y, x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T degrees<T>(T radians) where T : IFloatingPointIeee754<T> => radians * Constants<T>.Rad2Deg;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T radians<T>(T degrees) where T : IFloatingPointIeee754<T> => degrees * Constants<T>.Deg2Rad;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T sqrt<T>(T value) where T : IRootFunctions<T> => T.Sqrt(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T rsqrt<T>(T value) where T : IRootFunctions<T> => T.One / sqrt(value);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T sqr<T>(T value) where T : IRootFunctions<T> => value * value;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T mod<T>(T a, T b) where T : INumber<T> { var r = a % b; return r < T.Zero ? r + (b < T.Zero ? -abs(b) : abs(b)) : r; }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T select<T>(T falseValue, T trueValue, bool test) where T : INumber<T> => test ? trueValue : falseValue;
}