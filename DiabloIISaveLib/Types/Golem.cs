using DiabloIISaveLib.IO;

namespace DiabloIISaveLib.Types;

public class Golem
{
    private Golem(IBitReader reader, int version)
    {
        header = reader.ReadUInt16();
        exists = reader.ReadByte() == 1;
        if (exists)
        {
            item = Item.Read(reader, version);
        }
    }

    public ushort? header { get; set; }
    public bool exists { get; set; }
    public Item? item { get; set; }

    public void Write(IBitWriter writer, int version)
    {
        writer.WriteUInt16(header ?? 0x666B);
        writer.WriteByte((byte)(exists ? 1 : 0));
        if (exists)
        {
            item?.Write(writer, version);
        }
    }

    public static Golem Read(IBitReader reader, int version)
    {
        var golem = new Golem(reader, version);
        return golem;
    }
}
