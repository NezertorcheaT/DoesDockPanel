using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using ArgumentException = System.ArgumentException;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

namespace CustomHelper
{
    public static partial class Helper
    {
        public static void
            RegisterBuildCallback<T, T2, T3, T4>(this IContainerBuilder builder,
                Action<IObjectResolver, T, T2, T3, T4> action) =>
            builder.RegisterBuildCallback(r =>
            {
                if (!r.TryResolve<T>(out var t)) return;
                if (!r.TryResolve<T2>(out var t2)) return;
                if (!r.TryResolve<T3>(out var t3)) return;
                if (!r.TryResolve<T4>(out var t4)) return;
                action(r, t, t2, t3, t4);
            });

        public static void
            RegisterBuildCallback<T, T2, T3>(this IContainerBuilder builder,
                Action<IObjectResolver, T, T2, T3> action) =>
            builder.RegisterBuildCallback(r =>
            {
                if (!r.TryResolve<T>(out var t)) return;
                if (!r.TryResolve<T2>(out var t2)) return;
                if (!r.TryResolve<T3>(out var t3)) return;
                action(r, t, t2, t3);
            });

        public static void
            RegisterBuildCallback<T, T2>(this IContainerBuilder builder, Action<IObjectResolver, T, T2> action) =>
            builder.RegisterBuildCallback(r =>
            {
                if (!r.TryResolve<T>(out var t)) return;
                if (!r.TryResolve<T2>(out var t2)) return;
                action(r, t, t2);
            });

        public static void
            RegisterBuildCallback<T>(this IContainerBuilder builder, Action<IObjectResolver, T> action) =>
            builder.RegisterBuildCallback(r =>
            {
                if (!r.TryResolve<T>(out var t)) return;
                action(r, t);
            });


        public static bool Contains(this Range range, float i) => i <= range.End.Value && i >= range.Start.Value;
        public static bool Contains(this RangeInt range, float i) => i <= range.end && i >= range.start;

        public static void ClearKids(this GameObject gameObject) => gameObject.transform.ClearKids();

        public static void ClearKids(this Transform transform)
        {
            for (var i = 0; i < transform.childCount; i++)
                Object.Destroy(transform.GetChild(i).gameObject);
        }

        /// <summary>
        /// <para>Selects a random element of an enumerable, using Unity's random</para>
        /// Forces enumeration
        /// </summary>
        public static T TakeRandomOrDefault<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is null) return default;
            if (enumerable is IList<T> list)
                return list.Count switch
                {
                    0 => default,
                    1 => list[0],
                    _ => list[Random.Range(0, list.Count)]
                };

            var array = enumerable.ToArray();
            return array.Length switch
            {
                0 => default,
                1 => array[0],
                _ => array[Random.Range(0, array.Length)]
            };
        }

        /// <summary>
        /// <para>Selects a random element of an enumerable, using System random</para>
        /// Forces enumeration
        /// </summary>
        public static T TakeRandomOrDefault<T>(this IEnumerable<T> enumerable, int seed)
        {
            var random = new System.Random(seed);
            return enumerable.TakeRandomOrDefault(random);
        }

        /// <summary>
        /// <para>Selects a random element of an enumerable, using System random</para>
        /// Forces enumeration
        /// </summary>
        public static T TakeRandomOrDefault<T>(this IEnumerable<T> enumerable, System.Random random)
        {
            if (enumerable is null) return default;
            if (enumerable is IList<T> list)
                return list.Count switch
                {
                    0 => default,
                    1 => list[0],
                    _ => list[random.Next(0, list.Count)]
                };

            var array = enumerable.ToArray();
            return array.Length switch
            {
                0 => default,
                1 => array[0],
                _ => array[random.Next(0, array.Length)]
            };
        }


        /// <summary>
        /// <para>Selects a random element of an enumerable, using Unity's random</para>
        /// Forces enumeration
        /// </summary>
        public static T TakeRandom<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable is IList<T> list)
                return list.Count switch
                {
                    0 => throw new ArgumentException("list is empty"),
                    1 => list[0],
                    _ => list[Random.Range(0, list.Count)]
                };

            var array = enumerable.ToArray();
            return array.Length switch
            {
                0 => throw new ArgumentException("enumerable is null"),
                1 => array[0],
                _ => array[Random.Range(0, array.Length)]
            };
        }

        /// <summary>
        /// <para>Selects a random element of an enumerable, using System random</para>
        /// Forces enumeration
        /// </summary>
        public static T TakeRandom<T>(this IEnumerable<T> enumerable, int seed)
        {
            var random = new System.Random(seed);
            return enumerable.TakeRandom(random);
        }

        /// <summary>
        /// <para>Selects a random element of an enumerable, using System random</para>
        /// Forces enumeration
        /// </summary>
        public static T TakeRandom<T>(this IEnumerable<T> enumerable, System.Random random)
        {
            if (enumerable is IList<T> list)
                return list.Count switch
                {
                    0 => throw new ArgumentException("list is null"),
                    1 => list[0],
                    _ => list[random.Next(0, list.Count)]
                };

            var array = enumerable.ToArray();
            return array.Length switch
            {
                0 => throw new ArgumentException("enumerable is null"),
                1 => array[0],
                _ => array[random.Next(0, array.Length)]
            };
        }

        /// <summary>
        /// Takes only elements of certain type
        /// </summary>
        /// <param name="enumerable"></param>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        [NotNull]
        [LinqTunnel]
        public static IEnumerable<TResult> AsType<TResult>(this IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                if (item is TResult result)
                    yield return result;
            }
        }

        public static string ByComma<T>(this IEnumerable<T> enumerable) =>
            enumerable.Select(i => i.ToString()).ByComma();

        public static string ByComma(this IEnumerable<GameObject> enumerable) =>
            enumerable.Select(i => i.name).ByComma();

        public static string ByComma(this IEnumerable<string> enumerable)
        {
            var array = enumerable as string[] ?? enumerable.ToArray();
            return array.Length == 0
                ? string.Empty
                : array.Aggregate((a, b) => $"{a}, {b}");
        }

        public static bool IsPrefab(this GameObject gameObject) => gameObject.scene.name is null;
        public static bool IsOnPrefab(this Component component) => component.gameObject.scene.name is null;

        public static int Area(this Vector2Int value) => value.x * value.y;
        public static float Area(this Vector2 value) => value.x * value.y;

        public static Vector2 Swap(this Vector2 a) => new(a.y, a.x);

        public static Vector2Int ToInt(this Vector2 a) => new((int)a.x, (int)a.y);
        public static Vector3Int ToInt(this Vector3 a) => new((int)a.x, (int)a.y, (int)a.z);
        public static Vector3Int ToVector3Int(this Vector2Int a, int def = 0) => new(a.x, a.y, def);
        public static Vector3Int ToVector3Int(this Vector2 a, int def = 0) => new((int)a.x, (int)a.y, def);
        public static Vector2Int ToVector2Int(this Vector2 a) => new((int)a.x, (int)a.y);
        public static Vector3Int ToVector3Int(this Vector3 a) => new((int)a.x, (int)a.y, (int)a.z);
        public static Vector2Int ToVector2Int(this Vector3 a) => new((int)a.x, (int)a.y);
        public static Vector2 ToVector2(this Vector3 a) => a;
        public static Vector2Int ToVector2Int(this Vector3Int a) => new(a.x, a.y);

        public static Vector3 Flatten(this Vector3 a) => new(a.x, a.y, 0);
        public static Vector3Int Flatten(this Vector3Int a) => new(a.x, a.y, 0);

        public static bool Contains2D(this BoundsInt a, Vector3Int point) => a.Contains2D(point.ToVector3());

        public static bool Contains2D(this BoundsInt a, Vector3 point) =>
            point.x >= a.xMin &&
            point.x <= a.xMax &&
            point.y >= a.yMin &&
            point.y <= a.yMax;

        public static bool Contains2D(this Bounds a, Vector3Int point) => a.Contains2D(point.ToVector3());

        public static bool Contains2D(this Bounds a, Vector3 point) =>
            point.x >= a.min.x &&
            point.x <= a.max.x &&
            point.y >= a.min.y &&
            point.y <= a.max.y;

        public static Vector2 Abs(this Vector2 a) => new(Mathf.Abs(a.x), Mathf.Abs(a.y));

        public static Vector2Int Inverse(this Vector2Int a) => new(a.y, a.x);
        public static Vector2 Inverse(this Vector2 a) => new(a.y, a.x);

        public const float Intersects2DContract = 0.5f;

        public static bool IntersectsMany2D(this BoundsInt b, IEnumerable<BoundsInt> enumerable,
            bool cutCorners = false) =>
            enumerable.Any(a => b.Intersects2D(a, cutCorners));

        //гениально
        public static bool Intersects2D(this BoundsInt a, BoundsInt b, bool corners = false, float contract = 0.5f)
        {
            if (a.Contains2D(b.center.ToVector3Int()))
                return true;
            if (b.Contains2D(a.center.ToVector3Int()))
                return true;

            return corners
                ? a.Contains2D(b.min + contract.ToVector3()) ||
                  a.Contains2D(b.max - contract.ToVector3()) ||
                  a.Contains2D(new Vector3(b.xMax - contract, b.yMin + contract)) ||
                  a.Contains2D(new Vector3(b.xMin + contract, b.yMax - contract)) ||
                  b.Contains2D(a.min + contract.ToVector3()) ||
                  b.Contains2D(new Vector3(a.xMin + contract, a.yMax - contract)) ||
                  b.Contains2D(new Vector3(a.xMin + contract, a.yMax - contract)) ||
                  b.Contains2D(new Vector3(a.xMin + contract, a.yMax - contract))
                : a.Contains2D(b.min) ||
                  a.Contains2D(new Vector3Int(b.xMin, b.yMax)) ||
                  a.Contains2D(new Vector3Int(b.xMin, b.yMax)) ||
                  a.Contains2D(new Vector3Int(b.xMin, b.yMax)) ||
                  b.Contains2D(a.min) ||
                  b.Contains2D(a.max) ||
                  b.Contains2D(new Vector3Int(a.xMax, a.yMin)) ||
                  b.Contains2D(new Vector3Int(a.xMin, a.yMax));
        }

        public static bool IntersectsMany2D(this IEnumerable<BoundsInt> enumerable, BoundsInt b,
            bool cutCorners = false) =>
            enumerable.Any(a => b.Intersects2D(a, cutCorners));

        public static bool IntersectsMany2D(this Bounds b, IEnumerable<Bounds> enumerable, bool cutCorners = false) =>
            enumerable.Any(a => b.Intersects2D(a, cutCorners));

        public static bool IntersectsMany2D(this IEnumerable<Bounds> enumerable, Bounds b, bool cutCorners = false) =>
            enumerable.Any(a => b.Intersects2D(a, cutCorners));

        //гениально 2
        public static bool Intersects2D(this Bounds a, Bounds b, bool corners = false)
        {
            if (corners)
                a = new Bounds(a.center, a.size - Intersects2DContract.ToVector3());

            return
                a.Contains2D(b.center.ToVector3Int()) ||
                b.Contains2D(a.center.ToVector3Int()) ||
                a.Contains2D(b.min) ||
                a.Contains2D(b.max) ||
                a.Contains2D(new Vector3(b.max.x, b.min.y)) ||
                a.Contains2D(new Vector3(b.min.x, b.max.y)) ||
                b.Contains2D(a.min) ||
                b.Contains2D(a.max) ||
                b.Contains2D(new Vector3(a.max.x, a.min.y)) ||
                b.Contains2D(new Vector3(a.min.x, a.max.y));
        }

        public static Quaternion DirectionToGlobalRotation2D(this Vector3 direction) =>
            Quaternion.FromToRotation(Vector3.right, direction.normalized);

        public static Vector2Int ToVector2Int(this int a) => new(a, a);
        public static Vector2Int ToVector2Int(this float a) => new Vector2(a, a).ToVector2Int();
        public static Vector2 ToVector2(this int a) => new(a, a);
        public static Vector2 ToVector2(this float a) => new(a, a);

        public static Vector3Int ToVector3Int(this int a) => new(a, a, a);
        public static Vector3Int ToVector3Int(this float a) => new Vector3(a, a, a).ToVector3Int();
        public static Vector3 ToVector3(this int a) => new(a, a, a);
        public static Vector3 ToVector3(this float a) => new(a, a, a);

        public static Vector3 ToVector3(this Vector2 vec) => new(vec.x, vec.y, 0);
        public static Vector3 ToVector3(this Vector2Int vec) => new(vec.x, vec.y, 0);
        public static Vector3 ToVector3(this Vector3Int vec) => new(vec.x, vec.y, vec.z);
        public static Vector2Int ToVector2(this Vector2Int vec) => new(vec.x, vec.y);

        /// <summary>
        /// кароч куб
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static float Qb(this float n) => n * n * n;

        /// <summary>
        /// кароч квадрат
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public static float Sq(this float n) => n * n;

        public static float Pow(this float n, float power) => Mathf.Pow(n, power);

        public static Vector3 Multiply(this Vector3 a, Vector3 b) => new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        public static Vector2 Multiply(this Vector2 a, float bx, float by) => new Vector2(a.x * bx, a.y * by);

        public static Vector3 Multiply(this Vector3 a, float bx, float by, float bz) =>
            new Vector3(a.x * bx, a.y * by, a.z * bz);

        public static Vector2 Divide(this Vector2 a, Vector2 b) => new Vector2(a.x / b.x, a.y / b.y);
        public static Vector3 Divide(this Vector3 a, Vector3 b) => new Vector3(a.x / b.x, a.y / b.y, a.z / b.z);


        public static void DrawBox(Vector2 point, Vector2 size)
        {
#if UNITY_EDITOR
            Debug.DrawLine(point + new Vector2(size.x / 2f, size.y / 2f),
                point + new Vector2(-size.x / 2f, size.y / 2f));
            Debug.DrawLine(point + new Vector2(-size.x / 2f, size.y / 2f),
                point + new Vector2(-size.x / 2f, -size.y / 2f));
            Debug.DrawLine(point + new Vector2(-size.x / 2f, -size.y / 2f),
                point + new Vector2(size.x / 2f, -size.y / 2f));
            Debug.DrawLine(point + new Vector2(size.x / 2f, -size.y / 2f),
                point + new Vector2(size.x / 2f, size.y / 2f));
#endif
        }

        /// <summary>
        /// штука, аналог которой используется в <c>UnityEngine.Random.Range(float, float)</c><br />
        /// разница в том, что здесь источник рандомного значения - вызывающий<br />
        /// по сути Lerp, только для одного числа
        /// </summary>
        /// <param name="value">значение, в пределах [0, 1]</param>
        /// <param name="min">минимум разброса</param>
        /// <param name="max">максимум разброса</param>
        public static float RandomSpread(float value, float min, float max) =>
            min == max ? min : min + value * (max - min);

        /// <summary>
        /// почему-то я не нашёл в стандартной библиотеке переопределение, аналогичное Clamp
        /// </summary>
        public static float Repeat(float value, float min, float max)
        {
            value -= min;
            max -= min;
            value -= (int)(value / max) * max;
            return value + min;
        }

        public static void OpenWithDefaultProgram(FilePath file)
        {
            if (file.IsEmpty) return;
            using var filerOpener = new Process();

            filerOpener.StartInfo.FileName = "explorer";
            filerOpener.StartInfo.Arguments = file.Value
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar);
            filerOpener.Start();
        }

        public static void OpenWithDefaultProgram(string file) =>
            OpenWithDefaultProgram(new FilePath(file));
    }
}