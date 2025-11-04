using AdventOfCodeBase;

namespace AdventOfCode_2023
{
    internal class SeedFertilizer
    {
        internal static long GetInput()
        {
            const long MapGroupCount = 7;

            Queue<string> fileQueue = InputGatherer.GetInputs("5 - Seed Fertilizer");
            bool isSecond = InputGatherer.GetUserInput("Seed Fertilizer");

            string seedsLine = fileQueue.Dequeue().Substring("seeds:".Length);
            long[] seedPairs = seedsLine.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(long.Parse)
                .ToArray();

            List<RangeMapGroup> mapGroups = new();

            fileQueue.Dequeue();

            for (long i = 0; i < MapGroupCount; i++)
            {
                fileQueue.Dequeue();

                List<RangeMap> maps = new();
                string? line = fileQueue.Dequeue();
                while (!string.IsNullOrEmpty(line) && char.IsDigit(line[0]))
                {
                    long[] parts = line.Split(' ').Select(long.Parse).ToArray();
                    maps.Add(new RangeMap(parts[0], parts[1], parts[2]));
                    line = fileQueue.Dequeue();
                }
                mapGroups.Add(new RangeMapGroup(maps.ToArray()));
            }

            List<SeedRange> seeds = new ();
            for (int seedPairId = 0; seedPairId < seedPairs.Length; seedPairId++)
            {
                long startingSeed = seedPairs[seedPairId];
                long length = 1;
                if (isSecond)
                    length = seedPairs[seedPairId + 1];
                seeds.Add(new(startingSeed, length));
            }

            List<SeedRange> seedRanges = seeds;

            foreach (RangeMapGroup group in mapGroups)
            {
                List<SeedRange> newSeedRanges = new List<SeedRange>();

                foreach (SeedRange seedRange in seedRanges)
                {
                    SeedRange[] mappedRanges = group.Map(seedRange);
                    newSeedRanges.AddRange(mappedRanges);
                }

                seedRanges = newSeedRanges;
            }

            return seedRanges.Select(s => s.start).Min();
        }
    }
}

class RangeMapGroup
{
    private readonly RangeMap[] _maps;

    public RangeMapGroup(RangeMap[] maps)
    {
        _maps = maps.OrderBy(s => s.sourceStart).ToArray();
    }

    public SeedRange[] Map(SeedRange range)
    {
        List<SeedRange> results = new();

        SeedRange remainingRange = range;

        foreach (RangeMap map in _maps)
        {
            if (remainingRange.start < map.sourceStart)
            {
                long cutOffLength = Math.Min(
                    remainingRange.length,
                    map.sourceStart - remainingRange.start);

                SeedRange cutOff = new SeedRange(remainingRange.start, cutOffLength);
                results.Add(cutOff);

                remainingRange = new SeedRange(
                    remainingRange.start + cutOffLength,
                    remainingRange.length - cutOffLength);
            }

            if (remainingRange.length <= 0)
                break;

            if (remainingRange.start >= map.sourceStart &&
                remainingRange.start < (map.sourceStart + map.rangeLength))
            {
                long intersectionLength = Math.Min(
                    remainingRange.length,
                    (map.sourceStart + map.rangeLength) - remainingRange.start);
                SeedRange intersection = new SeedRange(remainingRange.start, intersectionLength);
                SeedRange transformedRange = map.Transform(intersection);
                results.Add(transformedRange);

                remainingRange = new SeedRange(
                    remainingRange.start + intersectionLength,
                    remainingRange.length - intersectionLength);
            }

            if (remainingRange.length <= 0)
                break;
        }

        if (remainingRange.length > 0)
            results.Add(remainingRange);

        return results.ToArray();
    }
}

class RangeMap
{
    public long destinationStart;
    public long sourceStart; 
    public long rangeLength;
    
    public RangeMap(long destinationStart, long sourceStart, long rangeLength)
    {
        this.destinationStart = destinationStart;
        this.sourceStart = sourceStart;
        this.rangeLength = rangeLength;
    }

    public bool IsInSourceRange(long value) =>
        value >= sourceStart &&
        value < (sourceStart + rangeLength);

    public long MapSource(long value) =>
        destinationStart + (value - sourceStart);

    internal SeedRange Transform(SeedRange intersection) =>
        new(MapSource(intersection.start), intersection.length);
}

class SeedRange
{
    public readonly long start = -1;
    public readonly long length = -1;
    public SeedRange(long start, long length)
    {

    }
    public long End => start + length - 1;
}