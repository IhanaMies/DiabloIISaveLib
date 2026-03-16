using DiabloIISaveLib.IO;

namespace DiabloIISaveLib.Versions.v99;

public sealed class CorpseList_v99 : IDisposable
{
    public CorpseList_v99(ushort? header, ushort count)
    {
        this.header = header;
        this.count = count;
        corpses = new List<Corpse_v99>(count);
    }

    public ushort? header { get; set; }
    public ushort count { get; set; }
    public List<Corpse_v99> corpses { get; }

    public void Write(IBitWriter writer, int version)
    {
        writer.WriteUInt16(header ?? 0x4D4A);
        writer.WriteUInt16(count);
        for (int i = 0; i < count; i++)
        {
            corpses[i].Write(writer, version);
        }
    }

    public static CorpseList_v99 Read(IBitReader reader, int version)
    {
        var corpseList = new CorpseList_v99(
            header: reader.ReadUInt16(),
            count: reader.ReadUInt16()
        );
        for (int i = 0; i < corpseList.count; i++)
        {
            corpseList.corpses.Add(Corpse_v99.Read(reader, version));
        }
        return corpseList;
    }

    public void Dispose()
    {
        corpses.Clear();
    }
}

public sealed class Corpse_v99
{
    private Corpse_v99(IBitReader reader, int version)
    {
        Unk0x0 = reader.ReadUInt32();
        X = reader.ReadUInt32();
        Y = reader.ReadUInt32();
        items = ItemList_v99.Read(reader, version);
    }

    public uint? Unk0x0 { get; set; }
    public uint X { get; set; }
    public uint Y { get; set; }
    public ItemList_v99 items { get; }

    public void Write(IBitWriter writer, int version)
    {
        writer.WriteUInt32(Unk0x0 ?? 0x0);
        writer.WriteUInt32(X);
        writer.WriteUInt32(Y);
        items.Write(writer, version);
    }

    public static Corpse_v99 Read(IBitReader reader, int version)
    {
        var corpse = new Corpse_v99(reader, version);
        return corpse;
    }
}

