using System;
using System.IO;
using System.Text.Json.Serialization;

namespace Files
{
    public class FilePath : IEquatable<FilePath>, IEquatable<string>
    {
        public bool Equals(FilePath other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return IsEmpty == other.IsEmpty && Value == other.Value;
        }

        public bool Equals(string other)
        {
            var otherFile = new FilePath(other);
            if (other is null) return false;
            return IsEmpty == otherFile.IsEmpty && Value == otherFile.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((FilePath)obj);
        }

        public override int GetHashCode() => HashCode.Combine(IsEmpty, Value);

        public static bool operator ==(FilePath left, FilePath right) => Equals(left, right);
        public static bool operator !=(FilePath left, FilePath right) => !Equals(left, right);

        [JsonIgnore] public bool IsEmpty { get; private set; }
        public string Value { get; }

        [JsonConstructor]
        public FilePath(string path)
        {
            Value = path;
            IsEmpty = string.IsNullOrWhiteSpace(path) || !path.Contains(Path.AltDirectorySeparatorChar);
        }

        public static FilePath Empty => new("") { IsEmpty = true };

        public static implicit operator string(FilePath path) => path.Value;

        public static implicit operator FilePath(string path)
        {
            path = path
                .Replace('/', Path.AltDirectorySeparatorChar)
                .Replace('\\', Path.AltDirectorySeparatorChar);
            return !string.IsNullOrWhiteSpace(path) && path.Contains(Path.AltDirectorySeparatorChar)
                ? new FilePath(path)
                : Empty;
        }

        public static implicit operator ReadOnlySpan<char>(FilePath path) => path.Value;
        public static implicit operator FilePath(ReadOnlySpan<char> path) => path.ToString();

        public override string ToString() => Value;
    }
}