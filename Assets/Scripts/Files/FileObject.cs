using System;
using UnityEngine;

namespace Files
{
    public abstract class FileObject : IDisposable, IEquatable<FileObject>
    {
        public bool Equals(FileObject other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return File == other.File;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is FileObject other && Equals(other);
        }

        public override int GetHashCode()
        {
            return File.GetHashCode();
        }

        public static bool operator ==(FileObject left, FileObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FileObject left, FileObject right)
        {
            return !Equals(left, right);
        }

        public Texture2D Image;
        public string File;
        public override string ToString() => $"{base.ToString()} {{ File: {File}, Image: {Image} }}";
        public abstract void Dispose();
    }
}