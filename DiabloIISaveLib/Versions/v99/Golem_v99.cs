using DiabloIISaveLib.IO;

namespace DiabloIISaveLib.Versions.v99;

public class Golem_v99
{
    private Golem_v99(IBitReader reader, int version)
    {
        header = reader.ReadUInt16();
        exists = reader.ReadByte() == 1;
        if (exists)
        {
            item = Item_v99.Read(reader, version);
        }
    }

    public ushort? header { get; set; }
    public bool exists { get; set; }
    public Item_v99? item { get; set; }

    public void Write(IBitWriter writer, int version)
    {
        writer.WriteUInt16(header ?? 0x666B);
        writer.WriteByte((byte)(exists ? 1 : 0));
        if (exists)
        {
            item?.Write(writer, version);
        }
    }

    public static Golem_v99 Read(IBitReader reader, int version)
    {
        var golem = new Golem_v99(reader, version);
        return golem;
    }
}
