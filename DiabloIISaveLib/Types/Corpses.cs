using DiabloIISaveLib.IO;

namespace DiabloIISaveLib.Types;

public sealed class CorpseList : IDisposable
{
    public CorpseList(ushort? header, ushort count)
    {
        this.header = header;
        this.count = count;
        corpses = new List<Corpse>(count);
    }

    public ushort? header { get; set; }
    public ushort count { get; set; }
    public List<Corpse> corpses { get; }

    public void Write(IBitWriter writer, int version)
    {
        writer.WriteUInt16(header ?? 0x4D4A);
        writer.WriteUInt16(count);
        for (int i = 0; i < count; i++)
        {
            corpses[i].Write(writer, version);
        }
    }

    public static CorpseList Read(IBitReader reader, int version)
    {
        var corpseList = new CorpseList(
            header: reader.ReadUInt16(),
            count: reader.ReadUInt16()
        );
        for (int i = 0; i < corpseList.count; i++)
        {
            corpseList.corpses.Add(Corpse.Read(reader, version));
        }
        return corpseList;
    }

    public void Dispose()
    {
        corpses.Clear();
    }
}

public sealed class Corpse
{
    private Corpse(IBitReader reader, int version)
    {
        Unk0x0 = reader.ReadUInt32();
        X = reader.ReadUInt32();
        Y = reader.ReadUInt32();
        items = ItemList.Read(reader, version);
    }

    public uint? Unk0x0 { get; set; }
    public uint X { get; set; }
    public uint Y { get; set; }
    public ItemList items { get; }

    public void Write(IBitWriter writer, int version)
    {
        writer.WriteUInt32(Unk0x0 ?? 0x0);
        writer.WriteUInt32(X);
        writer.WriteUInt32(Y);
        items.Write(writer, version);
    }

    public static Corpse Read(IBitReader reader, int version)
    {
        var corpse = new Corpse(reader, version);
        return corpse;
    }
}

