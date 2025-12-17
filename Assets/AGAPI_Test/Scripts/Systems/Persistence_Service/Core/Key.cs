#nullable enable
using System;
using System.Diagnostics;

namespace AGAPI.Systems
{
    [DebuggerDisplay("{Value ?? \"<invalid>\",nq}", Type = "Key")]
    public readonly struct Key : IEquatable<Key>, IEquatable<string?>
    {
        public string? Value { get; }

        public bool IsValid => !string.IsNullOrEmpty(Value);

        public Key(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Key cannot be null/empty/whitespace.", nameof(value));
            Value = value; // keep exact casing; treat keys as case-sensitive schema
        }

        // Value semantics: ordinal, case-sensitive
        public bool Equals(Key other) =>
            StringComparer.Ordinal.Equals(Value, other.Value);

        public bool Equals(string? other) =>
            StringComparer.Ordinal.Equals(Value, other);

        public override bool Equals(object? obj) =>
            obj is Key k && Equals(k);

        public override int GetHashCode() =>
            IsValid ? StringComparer.Ordinal.GetHashCode(Value!) : 0;

        public override string ToString() => Value ?? string.Empty;

    }

}
