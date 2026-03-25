using DiabloIISaveLib.IO;
using Serilog;

namespace DiabloIISaveLib.Data;

public class ClassSkills_v99
{
    private const int SKILL_COUNT = 30;

    private static readonly uint[] SKILL_OFFSETS = { 6, 36, 66, 96, 126, 221, 251 };
    public ushort? Header { get; set; }
    public List<ClassSkill_v99> Skills { get; } = new List<ClassSkill_v99>(SKILL_COUNT);

    public ClassSkill_v99 this[int i] => Skills[i];

    public void Write(IBitWriter writer)
    {
        writer.WriteUInt16(Header ?? 0x6669);
        for (int i = 0; i < SKILL_COUNT; i++)
        {
            Skills[i].Write(writer);
        }
    }

    public static ClassSkills_v99 Read(IBitReader reader, int playerClass)
    {
        var classSkills = new ClassSkills_v99
        {
            Header = reader.ReadUInt16()
        };
        uint offset = SKILL_OFFSETS[playerClass];
        for (uint i = 0; i < SKILL_COUNT; i++)
        {
            var skill = ClassSkill_v99.Read(offset + i, reader.ReadByte());
            classSkills.Skills.Add(skill);
        }
        return classSkills;
    }
}

public class ClassSkill_v99
{
    public uint Id { get; set; }
    public byte Points { get; set; }

    public void Write(IBitWriter writer) => writer.WriteByte(Points);

    public static ClassSkill_v99 Read(uint id, byte points)
    {
        var classSkill = new ClassSkill_v99
        {
            Id = id,
            Points = points
        };
        return classSkill;
    }
}

//header skill
public class Skill_v99
{
    public uint Id { get; set; }

    public void Write(IBitWriter writer) => writer.WriteUInt32(Id);

    public static Skill_v99 Read(IBitReader reader)
    {
        var skill = new Skill_v99
        {
            Id = reader.ReadUInt32()
        };
		Log.Verbose($"Read skill.Id ({skill.Id}). 32 bits. Position: {reader.Position}");
		return skill;
    }
}
