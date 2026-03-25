using DiabloIISaveLib.IO;

namespace DiabloIISaveLib.Data;

public record Stat(int Layer, long Value);

//variable size. depends on # of attributes
public class Attributes_v99
{
    public ushort? header { get; set; }
    public Dictionary<ushort, Stat> stats { get; } = new Dictionary<ushort, Stat>();

    public static Attributes_v99 Read(IBitReader reader)
    {
        var attributes = new Attributes_v99
        {
            header = reader.ReadUInt16()
        };
        ushort id = reader.ReadUInt16(9);
        while (id != 0x1ff)
        {
            var property = Constants.item_stat_costs.Values.FirstOrDefault(x => x.id == id);
            if (property == null)
            {
                throw new Exception($"No ItemStatCost record found for id: {id} at bit {reader.Position - 9}");
            }
            int layer = 0;

            int csvParamBits = property.csv_param ?? 0;

            if (csvParamBits > 0)
            {
                layer = reader.ReadInt32Signed(csvParamBits);
            }

            int csvBits = property.csv_bits ?? 0;
            bool csvSigned = property.csv_signed ?? false;

            long value;

            if (csvBits < 32 && csvSigned)
                value = reader.ReadInt32Signed(csvBits);
            else
                value = reader.ReadInt32(csvBits);

            //int valShift = property["ValShift"].ToInt32();
            //value >>= valShift;

            attributes.stats.Add(id, new Stat(layer, value));

            id = reader.ReadUInt16(9);
        }
        reader.Align();
        return attributes;
    }

    public void Write(IBitWriter writer)
    {
        writer.WriteUInt16(header ?? 0x6667);
        foreach (var entry in stats)
        {
            var property = Constants.item_stat_costs.Values.FirstOrDefault(x => x.id == entry.Key);
            if (property == null)
            {
                throw new Exception($"No ItemStatCost record found for id: {entry.Key}");
            }
            writer.WriteUInt16(property.id!.Value, 9);

            int csvParamBits = property.csv_param ?? 0;

            if (csvParamBits > 0)
            {
                writer.WriteInt32(entry.Value.Layer, csvParamBits);
            }

            int csvBits = property.csv_bits ?? 0;

            long value = entry.Value.Value;

            writer.WriteUInt32((uint)value, csvBits);
        }
        writer.WriteUInt16(0x1ff, 9);
        writer.Align();
    }
}
