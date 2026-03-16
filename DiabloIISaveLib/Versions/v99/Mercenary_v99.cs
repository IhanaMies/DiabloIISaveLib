using DiabloIISaveLib.IO;

namespace DiabloIISaveLib.Versions.v99;

public sealed class Mercenary_v99
{
    public uint Flags { get; set; }
    public uint Id { get; set; }
    public ushort NameId { get; set; }
    public ushort TypeId { get; set; }
    public uint Experience { get; set; }

    public void Write(IBitWriter writer)
    {
        writer.WriteUInt32(Flags);
        writer.WriteUInt32(Id);
        writer.WriteUInt16(NameId);
        writer.WriteUInt16(TypeId);
        writer.WriteUInt32(Experience);
    }

    public static Mercenary_v99 Read(IBitReader reader)
    {
        var mercenary = new Mercenary_v99
        {
            Flags = reader.ReadUInt32(),
            Id = reader.ReadUInt32(),
            NameId = reader.ReadUInt16(),
            TypeId = reader.ReadUInt16(),
            Experience = reader.ReadUInt32()
        };
        return mercenary;
    }
}

public sealed class MercenaryItemList : IDisposable
{
    public ushort? header { get; set; }
    public ItemList_v99? item_list { get; private set; }

    public void Write(IBitWriter writer, Mercenary_v99 mercenary, int version)
    {
        writer.WriteUInt16(header ?? 0x666A);
        if (mercenary.Id != 0)
        {
            item_list?.Write(writer, version);
        }
    }

    public static MercenaryItemList Read(IBitReader reader, Mercenary_v99 mercenary, int version)
    {
        var mercenaryItemList = new MercenaryItemList
        {
            header = reader.ReadUInt16()
        };
        if (mercenary.Id != 0)
        {
            mercenaryItemList.item_list = ItemList_v99.Read(reader, version);
        }
        return mercenaryItemList;
    }

    public void Dispose()
    {
        item_list?.items.Clear();
    }
}
