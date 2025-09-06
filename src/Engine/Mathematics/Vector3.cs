using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Engine.Mathematics;

[StructLayout(LayoutKind.Sequential)]
public struct Vector3<T> : IEquatable<Vector3<T>>, ISpanFormattable where T : INumber<T>
{
    public T x;
    public T y;
    public T z;

    #region Constructors
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] 
    public Vector3(T x, T y, T z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] 
    public Vector3(T value) : this(value, value, value) { }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] 
    public Vector3(ReadOnlySpan<T> values)
    {
        if (values.Length < 3) throw new ArgumentOutOfRangeException(nameof(values), "Not enough values to fill the vector.");
        x = values[0];
        y = values[1];
        z = values[2];
    }
    
    #endregion
    
    #region Constants

    public static readonly Vector3<T> Zero = default; 
    public static readonly Vector3<T> One = new(T.One, T.One, T.One); 
    public static readonly Vector3<T> Up = new(T.Zero, T.One, T.Zero); 
    public static readonly Vector3<T> Down = new(T.Zero, -T.One, T.Zero); 
    public static readonly Vector3<T> Right = new(T.One, T.Zero, T.Zero); 
    public static readonly Vector3<T> Left = new(-T.One, T.Zero, T.Zero); 
    public static readonly Vector3<T> Forward = new(T.Zero, T.Zero, T.One); 
    public static readonly Vector3<T> Back = new(T.Zero, T.Zero, -T.One);
    
    #endregion
    
    #region Indexer
    
    public T this[int i]
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
#if DEBUG || ENGINE_MATH_BOUNDS_CHECKS
            if ((uint)i > 2u) throw new ArgumentOutOfRangeException(nameof(i));
#endif
            return i == 0 ? x : (i == 1 ? y : z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        set
        {
#if DEBUG || ENGINE_MATH_BOUNDS_CHECKS
            if ((uint)i > 2u) throw new ArgumentOutOfRangeException(nameof(i));
#endif
            if (i == 0) x = value; else if (i == 1) y = value; else z = value;
        }
    }
    
    #endregion

    #region Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> operator +(Vector3<T> a, Vector3<T> b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> operator -(Vector3<T> a, Vector3<T> b) => new(a.x - b.x, a.y - b.y, a.z - b.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> operator -(Vector3<T> v) => new(-v.x, -v.y, -v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> operator *(Vector3<T> v, T s) => new(v.x * s, v.y * s, v.z * s);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> operator *(T s, Vector3<T> v) => new(v.x * s, v.y * s, v.z * s);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> operator /(Vector3<T> v, T s) => new(v.x / s, v.y / s, v.z / s);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator ==(Vector3<T> a, Vector3<T> b) => a.Equals(b);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool operator !=(Vector3<T> a, Vector3<T> b) => !a.Equals(b);

    #endregion

    #region Equality & Hashing

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public bool Equals(Vector3<T> other) { var cmp = EqualityComparer<T>.Default; return cmp.Equals(x, other.x) && cmp.Equals(y, other.y) && cmp.Equals(z, other.z); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public override bool Equals(object? obj) => obj is Vector3<T> v && Equals(v);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public override int GetHashCode() => HashCode.Combine(x, y, z);

    #endregion
    
    #region Deconstruction
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Deconstruct(out T x) => x = this.x; 
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Deconstruct (out T x, out T y) => (x, y) = (this.x, this.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public void Deconstruct(out T x, out T y, out T z) => (x, y, z) = (this.x, this.y, this.z);
    
    #endregion
    
    
    #region Formatting
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public override string ToString() => ToString(null, null);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public string ToString(string? format, IFormatProvider? provider)
    {
        string sx = x is IFormattable fx ? fx.ToString(format, provider) : x?.ToString() ?? "null";
        string sy = y is IFormattable fy ? fy.ToString(format, provider) : y?.ToString() ?? "null";
        string sz = z is IFormattable fz ? fz.ToString(format, provider) : z?.ToString() ?? "null";
        return $"({sx}, {sy}, {sz})";
    }

    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        int i = 0;
        if (destination.Length < 1) { charsWritten = 0; return false; }
        destination[i++] = '(';
        if (!TryFormatComponent(x, destination, ref i, format, provider)) { charsWritten = 0; return false; }
        if (i + 2 > destination.Length) { charsWritten = 0; return false; }
        destination[i++] = ','; destination[i++] = ' ';
        if (!TryFormatComponent(y, destination, ref i, format, provider)) { charsWritten = 0; return false; }
        if (i + 2 > destination.Length) { charsWritten = 0; return false; }
        destination[i++] = ','; destination[i++] = ' ';
        if (!TryFormatComponent(z, destination, ref i, format, provider)) { charsWritten = 0; return false; }
        if (i + 1 > destination.Length) { charsWritten = 0; return false; }
        destination[i++] = ')';
        charsWritten = i;
        return true;
    }
    
    private static bool TryFormatComponent(in T value, Span<char> dest, ref int index, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        if (value is ISpanFormattable sf)
        {
            if (!sf.TryFormat(dest[index..], out int w, format, provider))
                return false;
            index += w;
            return true;
        }
        
        string s = value is IFormattable f ? f.ToString(format.ToString(), provider) : value?.ToString() ?? "null";
        if (index + s.Length > dest.Length) return false;
        s.AsSpan().CopyTo(dest[index..]);
        index += s.Length;
        return true;
    }
    
    #endregion
}

public static partial class Math
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool approx<T>(in Vector3<T> a, in Vector3<T> b, T absEps, T relEps) where T : IFloatingPointIeee754<T> => ((a.x == b.x) | ((T.Abs(a.x - b.x) <= T.Max(absEps, relEps * T.Max(T.Abs(a.x), T.Abs(b.x)))) & !T.IsNaN(a.x) & !T.IsNaN(b.x))) & ((a.y == b.y) | ((T.Abs(a.y - b.y) <= T.Max(absEps, relEps * T.Max(T.Abs(a.y), T.Abs(b.y)))) & !T.IsNaN(a.y) & !T.IsNaN(b.y))) & ((a.z == b.z) | ((T.Abs(a.z - b.z) <= T.Max(absEps, relEps * T.Max(T.Abs(a.z), T.Abs(b.z)))) & !T.IsNaN(a.z) & !T.IsNaN(b.z)));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool approx<T>(in Vector3<T> a, in Vector3<T> b, T eps) where T : IFloatingPointIeee754<T> => approx(a, b, eps, eps);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool approx<T>(in Vector3<T> a, in Vector3<T> b) where T : IFloatingPointIeee754<T> => approx(a, b, Constants<T>.KEpsilon, Constants<T>.KRelativeEpsilon);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool approx<T>(in Vector3<T> a, T b, T absEps, T relEps) where T : IFloatingPointIeee754<T> => approx(a, new Vector3<T>(b), absEps, relEps);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool approx<T>(in Vector3<T> a, T b, T eps) where T : IFloatingPointIeee754<T> => approx(a, new Vector3<T>(b), eps, eps);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool approx<T>(in Vector3<T> a, T b) where T : IFloatingPointIeee754<T> => approx(a, new Vector3<T>(b), Constants<T>.KEpsilon, Constants<T>.KRelativeEpsilon);
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isAnyZero<T>(in Vector3<T> v) where T : INumber<T> => T.IsZero(v.x) || T.IsZero(v.y) || T.IsZero(v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isZero<T>(in Vector3<T> v) where T : INumber<T> => T.IsZero(v.x) && T.IsZero(v.y) && T.IsZero(v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isXZero<T>(in Vector3<T> v) where T : INumber<T> => T.IsZero(v.x);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isYZero<T>(in Vector3<T> v) where T : INumber<T> => T.IsZero(v.y);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isZZero<T>(in Vector3<T> v) where T : INumber<T> => T.IsZero(v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isNormalized<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T>, IRootFunctions<T> => approx(lengthSq(v), T.One);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isInfinite<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T> => T.IsInfinity(v.x) | T.IsInfinity(v.y) | T.IsInfinity(v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isFinite<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T> => T.IsFinite(v.x) & T.IsFinite(v.y) & T.IsFinite(v.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static bool isNaN<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T> => T.IsNaN(v.x) | T.IsNaN(v.y) | T.IsNaN(v.z);

    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T angleDeg<T>(in Vector3<T> from, in Vector3<T> to, Policies.AngleUndefined policy = Policies.AngleUndefined.ReturnNaN) where T : IFloatingPointIeee754<T>, IRootFunctions<T> => degrees(angleRad(from, to, policy));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T angleRad<T>(in Vector3<T> from, in Vector3<T> to, Policies.AngleUndefined policy = Policies.AngleUndefined.ReturnNaN) where T : IFloatingPointIeee754<T>, IRootFunctions<T> { var la2  = lengthSq(from); var lb2  = lengthSq(to); var eps2 = Constants<T>.KSqrAlmostZero; var nz   = (la2 > eps2) & (lb2 > eps2); var invLa= rsqrt(max(la2, eps2)); var invLb= rsqrt(max(lb2, eps2)); var cosR = clamp(dot(from, to) * invLa * invLb, -T.One, T.One); var cosθ = nz ? cosR : policy == Policies.AngleUndefined.ReturnZero ? T.One : policy == Policies.AngleUndefined.ReturnNinety ? T.Zero : T.NaN; return acos(cosθ); }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> abs<T>(in Vector3<T> v) where T : INumber<T> => new(T.Abs(v.x), T.Abs(v.y), T.Abs(v.z));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> sin<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T> => new(T.Sin(v.x), T.Sin(v.y), T.Sin(v.z));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> clamp<T>(in Vector3<T> v, in Vector3<T> min, in Vector3<T> max) where T : INumber<T> => new(T.Clamp(v.x, min.x, max.x), T.Clamp(v.y, min.y, max.y), T.Clamp(v.z, min.z, max.z));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> min<T>(in Vector3<T> a, in Vector3<T> b) where T : INumber<T> => new(T.Min(a.x, b.x), T.Min(a.y, b.y), T.Min(a.z, b.z));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> max<T>(in Vector3<T> a, in Vector3<T> b) where T : INumber<T> => new(T.Max(a.x, b.x), T.Max(a.y, b.y), T.Max(a.z, b.z));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> lerp<T>(in Vector3<T> a, in Vector3<T> b, T t) where T : INumber<T> => a + (b - a) * t;
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> cos<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T> => new(T.Cos(v.x), T.Cos(v.y), T.Cos(v.z));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> tan<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T> => new(T.Tan(v.x), T.Tan(v.y), T.Tan(v.z));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T dot<T>(in Vector3<T> a, in Vector3<T> b) where T : INumber<T> => (a.x * b.x) + (a.y * b.y) + (a.z * b.z);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T lengthSq<T>(in Vector3<T> v) where T : INumber<T> => dot(v, v);
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static T length<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T>, IRootFunctions<T> => T.Sqrt(dot(v, v));
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> normalize<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T>, IRootFunctions<T> { T lsq = lengthSq(v); T eps = Constants<T>.KEpsilon; T invLen = T.One / T.Sqrt(T.Max(lsq, eps * eps)); return new Vector3<T>(v.x * invLen, v.y * invLen, v.z * invLen); }
    [MethodImpl(MethodImplOptions.AggressiveInlining)] public static Vector3<T> unsafeNormalize<T>(in Vector3<T> v) where T : IFloatingPointIeee754<T>, IRootFunctions<T> => v / length(v);
}
