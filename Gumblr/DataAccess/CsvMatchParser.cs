using Gumblr.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gumblr.DataAccess
{
    public interface IMatchParser
    {
        IEnumerable<Match> ParseMatches();
    }

    public class CsvMatchParser : IMatchParser
    {
        IEnumerable<string> mLines;

        public CsvMatchParser(IEnumerable<string> aLines)
        {
            mLines = aLines;
        }

        public IEnumerable<Match> ParseMatches()
        {
            var matchByIndex = new Dictionary<int, Match>();
            foreach (var line in mLines)
            {
                var split = line.Split(',');

                var index = int.Parse(split[0]);

                MatchDependency dependency = null;

                string host = null;
                string visitor = null;
                string group = null;
                MatchStage stage = MatchStage.Group;
                if (split.Length > 6)
                {
                    Enum.TryParse<MatchStage>(split[2], true, out stage);
                }

                if (stage == MatchStage.Group)
                {
                    group = split[1];
                    host = split[6];
                    visitor = split[7];
                }
                else if (stage == MatchStage.RoundOfSixteen)
                {
                    if (index == 49)
                    {
                        Console.WriteLine(index);
                    }


                    var hostDependentIndices = split[6].Split(';').Select(x => int.Parse(x));
                    var visitorDependentIndices = split[7].Split(';').Select(x => int.Parse(x));

                    if (hostDependentIndices.Count() != 6) throw new Exception();
                    if (visitorDependentIndices.Count() != 6) throw new Exception();

                    dependency = new MatchDependency()
                    {
                        Type = MatchDependencyType.TwoGroups,
                        HostDeterminingMatchIds = hostDependentIndices.Select(x => matchByIndex[x].MatchId),
                        VisitorDeterminingMatchIds = visitorDependentIndices.Select(x => matchByIndex[x].MatchId),
                    };
                }
                else
                {
                    dependency = new MatchDependency()
                    {
                        Type = MatchDependencyType.TwoMatches,
                        HostDeterminingMatchIds = new List<string> { matchByIndex[int.Parse(split[6])].MatchId },
                        VisitorDeterminingMatchIds = new List<string> { matchByIndex[int.Parse(split[7])].MatchId },
                    };
                }

                var startTime = DateTime.Parse(split[3]);
                var venue = split[4];

                var match = new Match
                {
                    MatchId = Guid.NewGuid().ToString(),
                    Stage = stage,
                    Group = group,
                    StartTime = startTime,
                    Venue = venue,
                    Host = host,
                    Visitor = visitor,
                    Dependency = dependency,
                };

                matchByIndex[index] = match;
            }

            return matchByIndex.Values.ToList();
        }

    }
}