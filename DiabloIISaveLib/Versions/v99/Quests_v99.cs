using DiabloIISaveLib.IO;
using System.Diagnostics;

namespace DiabloIISaveLib.Versions.v99;

public sealed class QuestsSection_v99 : IDisposable
{
    private readonly QuestsDifficulty_v99[] _difficulties = new QuestsDifficulty_v99[3];

    //0x014f [quests header identifier = 0x57, 0x6f, 0x6f, 0x21 "Woo!"]
    public uint? Header { get; set; }
    //0x0153 [version = 0x6, 0x0, 0x0, 0x0]
    public uint? Version { get; set; }
    //0x0153 [quests header length = 0x2a, 0x1]
    public ushort? Length { get; set; }

    public QuestsDifficulty_v99 Normal => _difficulties[0];
    public QuestsDifficulty_v99 Nightmare => _difficulties[1];
    public QuestsDifficulty_v99 Hell => _difficulties[2];

    public void Write(IBitWriter writer)
    {
        writer.WriteUInt32(Header ?? 0x216F6F57);
        writer.WriteUInt32(Version ?? 0x6);
        writer.WriteUInt16(Length ?? 0x12A);

        for (int i = 0; i < _difficulties.Length; i++)
        {
            _difficulties[i].Write(writer);
        }
    }

    public static QuestsSection_v99 Read(IBitReader reader)
    {
        var questSection = new QuestsSection_v99
        {
            Header = reader.ReadUInt32(),
            Version = reader.ReadUInt32(),
            Length = reader.ReadUInt16()
        };

        for (int i = 0; i < questSection._difficulties.Length; i++)
        {
            questSection._difficulties[i] = QuestsDifficulty_v99.Read(reader);
        }

        return questSection;
    }

    public void Dispose()
    {
        for (int i = 0; i < _difficulties.Length; i++)
        {
            Interlocked.Exchange(ref _difficulties[i]!, null)?.Dispose();
        }
    }
}

public sealed class QuestsDifficulty_v99 : IDisposable
{
    private QuestsDifficulty_v99(IBitReader reader)
    {
        ActI = ActIQuests_v99.Read(reader);
        ActII = ActIIQuests_v99.Read(reader);
        ActIII = ActIIIQuests_v99.Read(reader);
        ActIV = ActIVQuests_v99.Read(reader);
        ActV = ActVQuests_v99.Read(reader);
    }

    public ActIQuests_v99 ActI { get; set; }
    public ActIIQuests_v99 ActII { get; set; }
    public ActIIIQuests_v99 ActIII { get; set; }
    public ActIVQuests_v99 ActIV { get; set; }
    public ActVQuests_v99 ActV { get; set; }

    public void Write(IBitWriter writer)
    {
        ActI.Write(writer);
        ActII.Write(writer);
        ActIII.Write(writer);
        ActIV.Write(writer);
        ActV.Write(writer);
    }

    public static QuestsDifficulty_v99 Read(IBitReader reader)
    {
        var qd = new QuestsDifficulty_v99(reader);
        return qd;
    }

    public void Dispose()
    {
        ActI.Dispose();
        ActII.Dispose();
        ActIII.Dispose();
        ActIV.Dispose();
        ActV.Dispose();
    }
}

[Flags]
public enum QuestFlags : ushort
{
    None = 0,
    RewardGranted = 0x1,
    RewardPending = 0x2,
    Started = 0x4,
    LeftTown = 0x8,
    EnterArea = 0x10,
    Custom1 = 0x20,
    Custom2 = 0x40,
    Custom3 = 0x80,
    Custom4 = 0x100,
    Custom5 = 0x200,
    Custom6 = 0x400,
    Custom7 = 0x800,
    QuestLog = 0x1000,
    PrimaryGoalAchieved = 0x2000,
    CompletedNow = 0x4000,
    CompletedBefore = 0x8000,
}

public sealed class Quest_v99 : IDisposable
{
    private ushort _flags;

    public QuestFlags Flags { get => (QuestFlags)_flags; set => _flags = (ushort)value; }

    private Quest_v99(ushort flags) => _flags = flags;

    public void Write(IBitWriter writer)
    {
        writer.WriteUInt16(_flags);
    }

    public static Quest_v99 Read(IBitReader reader)
    {
        ushort bits = reader.ReadUInt16(16);
        return new Quest_v99(bits);
    }

    public void Dispose() { }
}

public sealed class ActIQuests_v99 : IDisposable
{
    private readonly Quest_v99[] _quests = new Quest_v99[7];

    public Quest_v99 Introduction => _quests[0];
    public Quest_v99 DenOfEvil => _quests[1];
    public Quest_v99 SistersBurialGrounds => _quests[2];
    public Quest_v99 ToolsOfTheTrade => _quests[3];
    public Quest_v99 TheSearchForCain => _quests[4];
    public Quest_v99 TheForgottenTower => _quests[5];
    public Quest_v99 SistersToTheSlaughter => _quests[6];

    public void Write(IBitWriter writer)
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            _quests[i].Write(writer);
        }
    }

    public static ActIQuests_v99 Read(IBitReader reader)
    {
        var quests = new ActIQuests_v99();
        for (int i = 0; i < quests._quests.Length; i++)
        {
            quests._quests[i] = Quest_v99.Read(reader);
        }
        return quests;
    }

    public void Dispose()
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            Interlocked.Exchange(ref _quests[i]!, null)?.Dispose();
        }
    }
}

public sealed class ActIIQuests_v99 : IDisposable
{
    private readonly Quest_v99[] _quests = new Quest_v99[8];

    public Quest_v99 Arrival => _quests[0];
    public Quest_v99 Introduction => _quests[1];
    public Quest_v99 RadamentsLair => _quests[2];
    public Quest_v99 TheHoradricStaff => _quests[3];
    public Quest_v99 TaintedSun => _quests[4];
    public Quest_v99 ArcaneSanctuary => _quests[5];
    public Quest_v99 TheSummoner => _quests[6];
    public Quest_v99 TheSevenTombs => _quests[7];

    public void Write(IBitWriter writer)
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            _quests[i].Write(writer);
        }
    }

    public static ActIIQuests_v99 Read(IBitReader reader)
    {
        var quests = new ActIIQuests_v99();
        for (int i = 0; i < quests._quests.Length; i++)
        {
            quests._quests[i] = Quest_v99.Read(reader);
        }
        return quests;
    }

    public void Dispose()
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            Interlocked.Exchange(ref _quests[i]!, null)?.Dispose();
        }
    }
}

public sealed class ActIIIQuests_v99 : IDisposable
{
    private readonly Quest_v99[] _quests = new Quest_v99[8];

    public Quest_v99 Arrival => _quests[0];
    public Quest_v99 Introduction => _quests[1];
    public Quest_v99 LamEsensTome => _quests[2];
    public Quest_v99 KhalimsWill => _quests[3];
    public Quest_v99 BladeOfTheOldReligion => _quests[4];
    public Quest_v99 TheGoldenBird => _quests[5];
    public Quest_v99 TheBlackenedTemple => _quests[6];
    public Quest_v99 TheGuardian => _quests[7];

    public void Write(IBitWriter writer)
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            _quests[i].Write(writer);
        }
    }

    public static ActIIIQuests_v99 Read(IBitReader reader)
    {
        var quests = new ActIIIQuests_v99();
        for (int i = 0; i < quests._quests.Length; i++)
        {
            quests._quests[i] = Quest_v99.Read(reader);
        }
        return quests;
    }

    public void Dispose()
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            Interlocked.Exchange(ref _quests[i]!, null)?.Dispose();
        }
    }
}

public sealed class ActIVQuests_v99 : IDisposable
{
    private readonly Quest_v99[] _quests = new Quest_v99[8];

    public Quest_v99 Arrival => _quests[0];
    public Quest_v99 Introduction => _quests[1];
    public Quest_v99 TheFallenAngel => _quests[2];
    public Quest_v99 TerrorsEnd => _quests[3];
    public Quest_v99 Hellforge => _quests[4];

    //3 shorts at the end of ActIV completion. presumably for extra quests never used.
    public Quest_v99 Extra1 => _quests[5];
    public Quest_v99 Extra2 => _quests[6];
    public Quest_v99 Extra3 => _quests[7];

    public void Write(IBitWriter writer)
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            _quests[i].Write(writer);
        }
    }

    public static ActIVQuests_v99 Read(IBitReader reader)
    {
        var quests = new ActIVQuests_v99();
        for (int i = 0; i < quests._quests.Length; i++)
        {
            quests._quests[i] = Quest_v99.Read(reader);
        }
        return quests;
    }

    public void Dispose()
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            Interlocked.Exchange(ref _quests[i]!, null)?.Dispose();
        }
    }
}

public sealed class ActVQuests_v99 : IDisposable
{
    private readonly Quest_v99[] _quests = new Quest_v99[17];

    public Quest_v99 Arrival => _quests[0];
    public Quest_v99 Introduction => _quests[1];
    //2 shorts after ActV introduction. presumably for extra quests never used.
    public Quest_v99 Extra1 => _quests[2];
    public Quest_v99 Extra2 => _quests[3];
    public Quest_v99 SiegeOnHarrogath => _quests[4];
    public Quest_v99 RescueOnMountArreat => _quests[5];
    public Quest_v99 PrisonOfIce => _quests[6];
    public Quest_v99 BetrayalOfHarrogath => _quests[7];
    public Quest_v99 RiteOfPassage => _quests[8];
    public Quest_v99 EveOfDestruction => _quests[9];
    public Quest_v99 Completion => _quests[10];
    //6 shorts after ActV completion. presumably for extra quests never used.
    public Quest_v99 Extra3 => _quests[11];
    public Quest_v99 Extra4 => _quests[12];
    public Quest_v99 Extra5 => _quests[13];
    public Quest_v99 Extra6 => _quests[14];
    public Quest_v99 Extra7 => _quests[15];
    public Quest_v99 Extra8 => _quests[16];

    public void Write(IBitWriter writer)
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            _quests[i].Write(writer);
        }
    }

    public static ActVQuests_v99 Read(IBitReader reader)
    {
        var quests = new ActVQuests_v99();
        for (int i = 0; i < quests._quests.Length; i++)
        {
            quests._quests[i] = Quest_v99.Read(reader);
        }
        return quests;
    }

    public void Dispose()
    {
        for (int i = 0; i < _quests.Length; i++)
        {
            Interlocked.Exchange(ref _quests[i]!, null)?.Dispose();
        }
    }
}
