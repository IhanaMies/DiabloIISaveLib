using DiabloIISaveLib.IO;
using System.Diagnostics.CodeAnalysis;

namespace DiabloIISaveLib.Data;

public class Appearances_v99
{
    private readonly Appearance[] _parts = new Appearance[16];

    public Appearance head { get => _parts[0]; set => _parts[0] = value; }
    public Appearance torso { get => _parts[1]; set => _parts[1] = value; }
    public Appearance legs { get => _parts[2]; set => _parts[2] = value; }
    public Appearance right_arm { get => _parts[3]; set => _parts[3] = value; }
    public Appearance left_arm { get => _parts[4]; set => _parts[4] = value; }
    public Appearance right_hand { get => _parts[5]; set => _parts[5] = value; }
    public Appearance left_hand { get => _parts[6]; set => _parts[6] = value; }
    public Appearance shield { get => _parts[7]; set => _parts[7] = value; }
    public Appearance special1 { get => _parts[8]; set => _parts[8] = value; }
    public Appearance special2 { get => _parts[9]; set => _parts[9] = value; }
    public Appearance special3 { get => _parts[10]; set => _parts[10] = value; }
    public Appearance special4 { get => _parts[11]; set => _parts[11] = value; }
    public Appearance special5 { get => _parts[12]; set => _parts[12] = value; }
    public Appearance special6 { get => _parts[13]; set => _parts[13] = value; }
    public Appearance special7 { get => _parts[14]; set => _parts[14] = value; }
    public Appearance special8 { get => _parts[15]; set => _parts[15] = value; }

    public void Write(IBitWriter writer)
    {
        for (int i = 0; i < _parts.Length; i++)
            _parts[i].Write(writer);
    }

    public static Appearances_v99 Read(IBitReader reader)
    {
        var appearances = new Appearances_v99();
        var parts = appearances._parts;

        for (int i = 0; i < parts.Length; i++)
            parts[i] = new Appearance(reader);

        return appearances;
    }
}

public readonly struct Appearance : IEquatable<Appearance>
{
    public Appearance(byte graphic, byte tint)
    {
        Graphic = graphic;
        Tint = tint;
    }

    public Appearance(IBitReader reader)
    {
        Graphic = reader.ReadByte();
        Tint = reader.ReadByte();
    }

    public readonly byte Graphic { get; }
    public readonly byte Tint { get; }

    public void Write(IBitWriter writer)
    {
        writer.WriteByte(Graphic);
        writer.WriteByte(Tint);
    }

    public bool Equals([AllowNull] Appearance other)
    {
        return Graphic == other.Graphic
            && Tint == other.Tint;
    }

    public override bool Equals(object? obj) => obj is Appearance app && Equals(app);

    public override int GetHashCode() => HashCode.Combine(Graphic, Tint);

    public static bool operator ==(Appearance left, Appearance right) => left.Equals(right);

    public static bool operator !=(Appearance left, Appearance right) => !left.Equals(right);
}
