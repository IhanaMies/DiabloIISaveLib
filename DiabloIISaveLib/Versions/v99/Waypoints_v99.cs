using DiabloIISaveLib.IO;

namespace DiabloIISaveLib.Versions.v99;

public sealed class WaypointsSection_v99
{
    private readonly WaypointsDifficulty_v99[] _difficulties = new WaypointsDifficulty_v99[3];

    //0x0279 [waypoint data = 0x57, 0x53 "WS"]
    public ushort? Header { get; set; }
    //0x027b [waypoint header version = 0x1, 0x0, 0x0, 0x0]
    public uint? Version { get; set; }
    //0x027f [waypoint header length = 0x50, 0x0]
    public ushort? Length { get; set; }
    public WaypointsDifficulty_v99 Normal => _difficulties[0];
    public WaypointsDifficulty_v99 Nightmare => _difficulties[1];
    public WaypointsDifficulty_v99 Hell => _difficulties[2];
    public WaypointsDifficulty_v99[] Difficulties => _difficulties;

    public void Write(IBitWriter writer)
    {
        writer.WriteUInt16(Header ?? 0x5357);
        writer.WriteUInt32(Version ?? 0x1);
        writer.WriteUInt16(Length ?? 0x50);

        for (int i = 0; i < _difficulties.Length; i++)
        {
            _difficulties[i].Write(writer);
        }
    }

    public static WaypointsSection_v99 Read(IBitReader reader)
    {
        var waypointsSection = new WaypointsSection_v99
        {
            Header = reader.ReadUInt16(),
            Version = reader.ReadUInt32(),
            Length = reader.ReadUInt16()
        };

        for (int i = 0; i < waypointsSection._difficulties.Length; i++)
        {
            waypointsSection._difficulties[i] = WaypointsDifficulty_v99.Read(reader);
        }

        return waypointsSection;
    }
}

public sealed class WaypointsDifficulty_v99
{
    private readonly Waypoints_v99[] _acts = new Waypoints_v99[5];
    private static readonly int[] _numBits = [9, 9, 9, 3, 9];

    private WaypointsDifficulty_v99(IBitReader reader)
    {
        Header = reader.ReadUInt16();

        for (int i = 0; i < _acts.Length; i++)
        {
            _acts[i] = Waypoints_v99.Read(reader, _numBits[i]);
        }

        reader.ReadInt32(9);
        reader.Align();
        reader.AdvanceBits(16 * 8);
    }

    //[0x02, 0x01]
    public ushort? Header { get; set; }
    public Waypoints_v99[] Acts => _acts;

    public void Write(IBitWriter writer)
    {
        writer.WriteUInt16(Header ?? 0x102);

        for (int i = 0; i < _acts.Length; i++)
        {
            _acts[i].Write(writer, _numBits[i]);
        }

        writer.WriteInt32(0, 9);
        writer.Align();
        Span<byte> padding = stackalloc byte[16];
        padding.Clear();
        writer.WriteBytes(padding);
    }

    public static WaypointsDifficulty_v99 Read(IBitReader reader)
    {
        var waypointsDifficulty = new WaypointsDifficulty_v99(reader);
        return waypointsDifficulty;
    }
}

[Flags]
public enum WaypointFlags : ushort
{
    None = 0x0,
    Town = 0x1,
    Waypoint1 = 0x2,
    Waypoint2 = 0x4,
    Waypoint3 = 0x8,
    Waypoint4 = 0x10,
    Waypoint5 = 0x20,
    Waypoint6 = 0x40,
    Waypoint7 = 0x80,
    Waypoint8 = 0x100,
    All = 0x1FF
}

[Flags]
public enum ActIWaypoints : ushort
{
    None = 0x0,
    RogueEncampment = 0x1,
    ColdPlains = 0x2,
    StonyField = 0x4,
    DarkWoods = 0x8,
    BlackMarsh = 0x10,
    OuterCloister = 0x20,
    JailLvl1 = 0x40,
    InnerCloister = 0x80,
    CatacombsLvl2 = 0x100,
    All = 0x1FF
}

[Flags]
public enum ActIIWaypoints : ushort
{
    None = 0x0,
    LutGholein = 0x1,
    SewersLvl2 = 0x2,
    DryHills = 0x4,
    HallsOfTheDeadLvl2 = 0x8,
    FarOasis = 0x10,
    LostCity = 0x20,
    PalaceCellarLvl1 = 0x40,
    ArcaneSanctuary = 0x80,
    CanyonOfTheMagi = 0x100,
    All = 0x1FF
}

[Flags]
public enum ActIIIWaypoints : ushort
{
    None = 0x0,
    KurastDocks = 0x1,
    SpiderForest = 0x2,
    GreatMarsh = 0x4,
    FlayerJungle = 0x8,
    LowerKurast = 0x10,
    KurastBazaar = 0x20,
    UpperKurast = 0x40,
    Travincal = 0x80,
    DuranceOfHateLvl2 = 0x100,
    All = 0x1FF
}

[Flags]
public enum ActIVWaypoints : ushort
{
    None = 0x0,
    ThePandemoniumFortress = 0x1,
    CityOfTheDamned = 0x2,
    RiverOfFlame = 0x4,
    All = 0x7
}

[Flags]
public enum ActVWaypoints : ushort
{
    None = 0x0,
    Harrogath = 0x1,
    FrigidHighlands = 0x2,
    ArreatPlateau = 0x4,
    CrystallinePassage = 0x8,
    HallsOfPain = 0x10,
    GlacialTrail = 0x20,
    FrozenTundra = 0x40,
    TheAncientsWay = 0x80,
    WorldstoneKeepLvl2 = 0x100,
    All = 0x1FF
}

public struct Waypoints_v99
{
    private WaypointFlags _flags;

    private Waypoints_v99(WaypointFlags flags) => _flags = flags;
    private Waypoints_v99(ushort flags) => _flags = (WaypointFlags)flags;

    public ushort Value => (ushort)_flags;

    // Implicit conversions FROM act enums
    public static implicit operator Waypoints_v99(ActIWaypoints wp) => new((WaypointFlags)wp);
    public static implicit operator Waypoints_v99(ActIIWaypoints wp) => new((WaypointFlags)wp);
    public static implicit operator Waypoints_v99(ActIIIWaypoints wp) => new((WaypointFlags)wp);
    public static implicit operator Waypoints_v99(ActIVWaypoints wp) => new((WaypointFlags)wp);
    public static implicit operator Waypoints_v99(ActVWaypoints wp) => new((WaypointFlags)wp);
    public static implicit operator Waypoints_v99(WaypointFlags wp) => new(wp);

    // Implicit conversions TO
    public static implicit operator WaypointFlags(Waypoints_v99 wp) => wp._flags;

    // Bitwise operators with Waypoints
    public static Waypoints_v99 operator &(Waypoints_v99 left, Waypoints_v99 right) =>
        new(left._flags & right._flags);

    public static Waypoints_v99 operator |(Waypoints_v99 left, Waypoints_v99 right) =>
        new(left._flags | right._flags);

    public static Waypoints_v99 operator ~(Waypoints_v99 wp) =>
        new(~wp._flags);

    // Bitwise operators with each act enum
    public static Waypoints_v99 operator &(Waypoints_v99 left, ActIWaypoints right) =>
        new(left._flags & (WaypointFlags)right);

    public static Waypoints_v99 operator |(Waypoints_v99 left, ActIWaypoints right) =>
        new(left._flags | (WaypointFlags)right);

    public static Waypoints_v99 operator &(Waypoints_v99 left, ActIIWaypoints right) =>
        new(left._flags & (WaypointFlags)right);

    public static Waypoints_v99 operator |(Waypoints_v99 left, ActIIWaypoints right) =>
        new(left._flags | (WaypointFlags)right);

    public static Waypoints_v99 operator &(Waypoints_v99 left, ActIIIWaypoints right) =>
        new(left._flags & (WaypointFlags)right);

    public static Waypoints_v99 operator |(Waypoints_v99 left, ActIIIWaypoints right) =>
        new(left._flags | (WaypointFlags)right);

    public static Waypoints_v99 operator &(Waypoints_v99 left, ActIVWaypoints right) =>
        new(left._flags & (WaypointFlags)right);

    public static Waypoints_v99 operator |(Waypoints_v99 left, ActIVWaypoints right) =>
        new(left._flags | (WaypointFlags)right);

    public static Waypoints_v99 operator &(Waypoints_v99 left, ActVWaypoints right) =>
        new(left._flags & (WaypointFlags)right);

    public static Waypoints_v99 operator |(Waypoints_v99 left, ActVWaypoints right) =>
        new(left._flags | (WaypointFlags)right);

    public void Write(IBitWriter writer, int numBits)
    {
        writer.WriteUInt16((ushort)_flags, numBits);
    }

    public static Waypoints_v99 Read(IBitReader reader, int numBits)
    {
        ushort bits = reader.ReadUInt16(numBits);
        return new(bits);
    }
}
