using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FastBezier
{

    /// <summary>
    /// Represents 3D vector structure.
    /// </summary>
    struct V3D
    {

        /// <summary>
        /// X coordinate.
        /// </summary>
        public double X;

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public double Y;

        /// <summary>
        /// Z coordinate;
        /// </summary>
        public double Z;

        /// <summary>
        /// Test if both vectors are equal.
        /// </summary>
        /// <param name="obj">The object the equality is tested with.</param>
        /// <returns></returns>
        public override bool Equals(object obj) => obj is V3D v && v.X == X && v.Y == Y && v.Z == Z;

        /// <summary>
        /// Returns the hash code calculated for this instance.
        /// </summary>
        /// <returns>Integer most likely to be unique for different vectors.</returns>
        public override int GetHashCode() => 17 * (17 * X.GetHashCode() + Y.GetHashCode()) + Z.GetHashCode();

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        public double Length => Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Gets the value indicating whether this vector is a zero vector (a vector whose all coordinates equal zero).
        /// </summary>
        public bool Zero => X == 0 && Y == 0 && Z == 0;

        /// <summary>
        /// Creates a new vector.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        /// <param name="z">Z coordinate.</param>
        public V3D(double x, double y, double z) { X = x; Y = y; Z = z; }

        public V3D(Vector3 point) { X = point.x; Y = point.y; Z = point.z; }

        /// <summary>
        /// Binary equality test.
        /// </summary>
        /// <param name="a">Left side.</param>
        /// <param name="b">Right side.</param>
        /// <returns>True if both vectors are not null and equal.</returns>
        public static bool operator ==(V3D a, V3D b) => (object)a != null && (object)b != null && a.X == b.X && a.Y == b.Y && a.Z == b.Z;

        /// <summary>
        /// Binary inequality test.
        /// </summary>
        /// <param name="a">Left side.</param>
        /// <param name="b">Right side.</param>
        /// <returns>True if one of the vectors is null or the vectors are not equals.</returns>
        public static bool operator !=(V3D a, V3D b) => (object)a == null || (object)b == null || a.X != b.X || a.Y != b.Y || a.Z != b.Z;

        /// <summary>
        /// Vector addition.
        /// </summary>
        /// <param name="a">Left side.</param>
        /// <param name="b">Right side.</param>
        /// <returns>The sum of vectors.</returns>
        public static V3D operator +(V3D a, V3D b) => new V3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        /// <summary>
        /// Vector subtraction.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>The difference of vectors.</returns>
        public static V3D operator -(V3D a, V3D b) => new V3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        /// <summary>
        /// Vector negation.
        /// </summary>
        /// <param name="a">Unary argument.</param>
        /// <returns>The negative vector.</returns>
        public static V3D operator -(V3D a) => new V3D(-a.X, -a.Y, -a.Z);

        /// <summary>
        /// Unary plus. Clones the vector.
        /// </summary>
        /// <param name="a">Unary argument.</param>
        /// <returns>This vector clone.</returns>
        public static V3D operator +(V3D a) => new V3D(+a.X, +a.Y, +a.Z);

        /// <summary>
        /// Scalar product.
        /// </summary>
        /// <param name="a">Left side vector.</param>
        /// <param name="k">Right side scalar.</param>
        /// <returns>The scalar product.</returns>
        public static V3D operator *(V3D a, double k) => new V3D(k * a.X, k * a.Y, k * a.Z);

        /// <summary>
        /// Scalar product.
        /// </summary>
        /// <param name="k">Left side scalar.</param>
        /// <param name="a">Right side vector.</param>
        /// <returns>The scalar product.</returns>
        public static V3D operator *(double k, V3D a) => new V3D(k * a.X, k * a.Y, k * a.Z);

        /// <summary>
        /// Scalar quotient.
        /// </summary>
        /// <param name="a">Left side vector.</param>
        /// <param name="k">Right side scalar.</param>
        /// <returns>The scalar quotient.</returns>
        public static V3D operator /(V3D a, double k) => new V3D(a.X / k, a.Y / k, a.Z / k);

        /// <summary>
        /// Scalar quotient.
        /// </summary>
        /// <param name="k">Left side scalar.</param>
        /// <param name="a">Right side vector.</param>
        /// <returns>The scalar quotient.</returns>
        public static V3D operator /(double k, V3D a) => new V3D(k / a.X, k / a.Y, k / a.Z);

        /// <summary>
        /// Returns dot product.
        /// </summary>
        /// <param name="a">A vector.</param>
        /// <returns>Dot product.</returns>
        public double Dot(V3D a) => X * a.X + Y * a.Y + Z * a.Z;

        /// <summary>
        /// Returns cross product.
        /// </summary>
        /// <param name="a">A vector.</param>
        /// <returns>Cross product.</returns>
        public V3D Cross(V3D a) => new V3D(Y * a.Z - Z * a.Y, Z * a.X - X * a.Z, X * a.Y - Y * a.X);

        /// <summary>
        /// Returns the vector between a and b that divides vector (a - b) in t ratio. For zero it's a, for 1 it's b.
        /// </summary>
        /// <param name="a">Vector a.</param>
        /// <param name="b">Vector b.</param>
        /// <param name="t">A number between 0 and 1.</param>
        /// <returns>the vector between a and b that divides vector (a - b) in t ratio. For zero it's a, for 1 it's b.</returns>
        public static V3D Interpolate(V3D a, V3D b, double t) => new V3D(a.X * (1.0 - t) + b.X * t, a.Y * (1.0 - t) + b.Y * t, a.Z * (1.0 - t) + b.Z * t);

    }

    /// <summary>
    /// Quadratic Bézier curve calculation class.
    /// </summary>
    class Bezier2
    {

        protected const double InterpolationPrecision = 0.001;

        /// <summary>
        /// Start point.
        /// </summary>
        public V3D A;
        /// <summary>
        /// Control point.
        /// </summary>
        public V3D B;
        /// <summary>
        /// End point.
        /// </summary>
        public V3D C;

        /// <summary>
        /// Creates a quadratic Bézier curve.
        /// </summary>
        /// <param name="a">Start point.</param>
        /// <param name="b">Control point.</param>
        /// <param name="c">End point.</param>
        public Bezier2(V3D a, V3D b, V3D c) { A = a; B = b; C = c; }

        /// <summary>
        /// Interpolated point at t : 0..1 position
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public V3D P(double t) => (1.0 - t) * (1.0 - t) * A + 2.0 * t * (1.0 - t) * B + t * t * C;

        /// <summary>
        /// Gets the calculated length.
        /// </summary>
        /// <remarks>
        /// Integral calculation by Dave Eberly, slightly modified for the edge case with colinear control point.
        /// See: http://www.gamedev.net/topic/551455-length-of-a-generalized-quadratic-bezier-curve-in-3d/
        /// </remarks>
        public double Length
        {
            get
            {
                if (A == C)
                {
                    if (A == B) return 0.0;
                    return (A - B).Length;
                }
                if (B == A || B == C) return (A - C).Length;
                V3D A0 = B - A;
                V3D A1 = A - 2.0 * B + C;
                if (!A1.Zero)
                {
                    double c = 4.0 * A1.Dot(A1);
                    double b = 8.0 * A0.Dot(A1);
                    double a = 4.0 * A0.Dot(A0);
                    double q = 4.0 * a * c - b * b;
                    double twoCpB = 2.0 * c + b;
                    double sumCBA = c + b + a;
                    var l0 = (0.25 / c) * (twoCpB * Math.Sqrt(sumCBA) - b * Math.Sqrt(a));
                    if (q == 0.0) return l0;
                    var l1 = (q / (8.0 * Math.Pow(c, 1.5))) * (Math.Log(2.0 * Math.Sqrt(c * sumCBA) + twoCpB) - Math.Log(2.0 * Math.Sqrt(c * a) + b));
                    return l0 + l1;
                }
                else return 2.0 * A0.Length;
            }
        }

        /// <summary>
        /// Gets the old slow and inefficient line interpolated length.
        /// </summary>
        public double InterpolatedLength
        {
            get
            {
                if (A == C)
                {
                    if (A == B) return 0;
                    return (A - B).Length;
                }
                if (B == A || B == C) return (A - C).Length;
                double dt = InterpolationPrecision / (C - A).Length, length = 0.0;
                for (double t = dt; t < 1.0; t += dt) length += (P(t - dt) - P(t)).Length;
                return length;
            }
        }

    }
}