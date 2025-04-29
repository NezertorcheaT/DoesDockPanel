using System;
using System.IO;
using System.Text.Json.Serialization;

public readonly struct FilePath : IEquatable<FilePath>, IEquatable<string>
{
    public bool Equals(FilePath other) => IsEmpty == other.IsEmpty && Value == other.Value;
    public override bool Equals(object obj) => obj.GetType() == GetType() && Equals((FilePath)obj);

    public bool Equals(string other)
    {
        var otherFile = new FilePath(other);
        return IsEmpty == otherFile.IsEmpty && Value == otherFile.Value;
    }

    public override int GetHashCode() => HashCode.Combine(IsEmpty, Value);

    public static bool operator ==(FilePath left, string right) => Equals(left.Value, right);
    public static bool operator !=(FilePath left, string right) => !Equals(left.Value, right);
    public static bool operator ==(FilePath left, FilePath right) => Equals(left, right);
    public static bool operator !=(FilePath left, FilePath right) => !Equals(left, right);

    [JsonIgnore] public bool IsEmpty { get; }
    public string Value { get; }

    [JsonConstructor]
    public FilePath(string path) : this(
        Validate(path),
        string.IsNullOrWhiteSpace(path) || !path.Contains(Path.AltDirectorySeparatorChar)
    )
    {
    }

    private FilePath(string path, bool isEmpty)
    {
        Value = path;
        IsEmpty = isEmpty;
    }

    private static string Validate(string path) => string.IsNullOrWhiteSpace(path)
        ? string.Empty
        : path.Replace('/', Path.AltDirectorySeparatorChar).Replace('\\', Path.AltDirectorySeparatorChar);

    public static FilePath Empty => new(string.Empty, true);

    public static implicit operator string(FilePath path) => path.IsEmpty ? string.Empty : path.Value;

    public static implicit operator FilePath(string path)
    {
        if (path is null) return Empty;
        path = Validate(path);
        return !string.IsNullOrWhiteSpace(path) && path.Contains(Path.AltDirectorySeparatorChar)
            ? new FilePath(path)
            : Empty;
    }

    public static implicit operator ReadOnlySpan<char>(FilePath path) => path.Value;
    public static implicit operator FilePath(ReadOnlySpan<char> path) => path.ToString();

    public override string ToString() => Value;
}