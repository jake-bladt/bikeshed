using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using Gallery.Entities.ImageGallery;

namespace Gallery.Entities.Elections
{
    public class FileSystemElectionResultSetFactory : IElectionResultSetFactory
    {
        protected readonly string _ElectionRoot;
        protected readonly IImageGallery _Gallery;

        protected Dictionary<string, ElectionType> ElectionTypesByName = new Dictionary<string, ElectionType>
          {
              { "travel", ElectionType.Travel },
              { "rookie", ElectionType.Rookie },
              { "star",   ElectionType.Star   },
              { "walkin", ElectionType.WalkIn },
              { "wonder", ElectionType.Wonder },
              { "prospect", ElectionType.Prospect },
              { "rider", ElectionType.Rider }
          };

        public FileSystemElectionResultSetFactory(string root, IImageGallery gallery)
        {
            _ElectionRoot = root;
            _Gallery = gallery;
        }

        public ElectionResultSet GetResultSet()
        {
            var ret = new ElectionResultSet();
            if (Directory.Exists(_ElectionRoot))
            {
                var di = new DirectoryInfo(_ElectionRoot);
                var subdirs = di.GetDirectories();

                // Iterate through primary election directories
                var yearDirs = subdirs.ToList().Where(d => IsAnnualFolder(d));
                yearDirs.ToList().ForEach(d => GetYearResults(d, ret));

                //  Iterate through run-offs
                var runoffPath = Path.Combine(_ElectionRoot, "runoff");
                if(Directory.Exists(runoffPath))
                {
                    var allRunoffs = new DirectoryInfo(runoffPath).GetDirectories();
                    allRunoffs.ToList().ForEach(ro =>
                    {
                        var electionDate = ro.CreationTime;
                        var electionName = RunoffElectionName(ro.Name);
                        GetElectionResults(ro, electionName, electionDate, ret);
                    });
                }
                else
                {
                    ret.ParseErrors.Add($"Could not find runoff directory at {runoffPath}");
                }

                // Iterate through special elections
                var specialPath = Path.Combine(_ElectionRoot, "special");
                if (Directory.Exists(specialPath))
                {
                    var allRunoffs = new DirectoryInfo(specialPath).GetDirectories();
                    allRunoffs.ToList().ForEach(special =>
                    {
                        var electionDate = special.CreationTime;
                        var electionName = SpecialElectionName(special.Name);
                        GetElectionResults(special, electionName, electionDate, ret);
                    });
                }
                else
                {
                    ret.ParseErrors.Add($"Could not find runoff directory at {runoffPath}");
                }

            }
            else
            {
                ret.ParseErrors.Add($"Directory {_ElectionRoot} does not exist.");
            }
            return ret;
        }

        protected bool GetYearResults(DirectoryInfo yearDir, ElectionResultSet resultSet)
        {
            var monthDirs = yearDir.GetDirectories().ToList().Where(d => IsMonthlyFolder(d));
            return monthDirs.ToList()
                .Aggregate<DirectoryInfo, bool>(true, (res, item) => res && GetMonthResults(item, resultSet));
        }

        protected bool GetMonthResults(DirectoryInfo monthDir, ElectionResultSet resultSet)
        {
            return ElectionTypesByName.ToList().Aggregate<KeyValuePair<string, ElectionType>, bool>(true, (res, kvp) =>
            {
                var bankPath = Path.Combine(monthDir.FullName, kvp.Key);
                if(Directory.Exists(bankPath))
                {
                    var di = new DirectoryInfo(bankPath);
                    var electionDate = DateFromDirectoryPattern(monthDir.Name);
                    var electionName = BankElectionName(di.Name, electionDate, kvp.Key);
                    return res && GetElectionResults(di, electionName, electionDate, resultSet);
                }
                else
                {
                    return true; // Historic months don't have all election types
                }
            });
        }

        protected SingleElectionResult ResultFromFile(string electionName, DateTime electionDate, FileInfo fi, int subjectCount)
        {
            string stripped = fi.Name.Replace(".jpg", String.Empty);
            var parts = stripped.Split('-');
            if (parts.Length < 2) return new SingleElectionResult { OrdinalRank = -1 };
            int rank = -1;
            var subjectName = new StringBuilder(parts[1]);
            if(parts.Length > 2)
            {
                for(int i = 2; i < parts.Length; i++)
                {
                    subjectName.Append('-');
                    subjectName.Append(parts[i]);
                }
            }

            return new SingleElectionResult
            {
                ElectionName = electionName,
                SubjectName = subjectName.ToString(),
                EventDate = electionDate,
                OrdinalRank = int.TryParse(parts[0], out rank) ? rank : -1,
                PointValue = subjectCount - rank + 1
            };
        }

        protected bool GetElectionResults(DirectoryInfo di, string electionName, DateTime electionDate, ElectionResultSet resultSet)
        {
            var images = di.GetFiles("*.jpg");
            var parsedResults = images.ToList().Select(fi => ResultFromFile(electionName, electionDate, fi, images.Length));

            if(parsedResults.Any(r => r.OrdinalRank == -1))
            {
                resultSet.ParseErrors.Add($"Election at {di.FullName} could not be parsed. One or more images had no rank.");
            }
            else
            {
                var sortingDictionary = new Dictionary<int, SingleElectionResult>();
                var isValid = true;
                parsedResults.ToList().ForEach(r =>
                {
                    // Look for alternate ranks.
                    if(sortingDictionary.ContainsKey(r.OrdinalRank))
                    {
                        resultSet.ParseErrors.Add($"Election at {di.FullName} could not be parsed. Rank {r.OrdinalRank} duplicated.");
                        isValid = false;
                    }
                    else
                    {
                        // Look for misnamed subjects.
                        if(_Gallery.Subjects.ContainsKey(r.SubjectName))
                        {
                            sortingDictionary[r.OrdinalRank] = r;
                        }
                        else
                        {
                            resultSet.ParseErrors.Add(
                                $"Election at {di.FullName} could not be parsed. Unrecognized subject {r.SubjectName} at {r.OrdinalRank}.");
                            isValid = false;
                        }
                    }
                });

                // Look for missing ranks.
                for(int i = 1; i <= images.Length; i++)
                {
                    if(!sortingDictionary.ContainsKey(i))
                    {
                        resultSet.ParseErrors.Add($"Election at {di.FullName} could not be parsed. No subject at rank {i}.");
                        isValid = false;
                    }
                }

                // If there are no errors, add this election's results to the result set.
                if(isValid)
                {
                    sortingDictionary.ToList().ForEach(kvp => resultSet.Add(kvp.Value));
                }
            }
            return true;
        }

        protected string SpecialElectionName(string directoryName)
        {
            var dnameArr = directoryName.ToCharArray();
            var sb = new StringBuilder();
            sb.Append(dnameArr[0]);
            for (int i = 1; i < dnameArr.Length; i++)
            {
                var c = dnameArr[i].ToString();
                if (c == c.ToUpper() && c != c.ToLower()) sb.Append(" ");
                sb.Append(c);
            }
            return sb.ToString();
        }

        protected string RunoffElectionName(string dirName)
        {
            if (!dirName.Contains("-")) return dirName;
            var dirNameParts = dirName.Split('-');
            var ordinal = Int32.Parse(dirNameParts[dirNameParts.Length - 1]);
            var sb = new StringBuilder(dirNameParts[0]);
            if (dirNameParts.Length > 2)
            {
                for (int i = 1; i < dirNameParts.Length - 2; i++)
                {
                    sb.Append("-");
                    sb.Append(dirNameParts[i]);
                }
            }

            return sb.ToString() + " #" + ordinal;
        }

        protected string BankElectionName(String monthPattern, DateTime electionDate, String electionTypeName)
        {
            return $"{electionDate.ToString("MMMM yyyy")} {electionTypeName}";
        }

        protected DateTime DateFromDirectoryPattern(string pattern)
        {
            if(pattern.Length == 4)
            {
                return new DateTime(int.Parse(pattern), 1, 1);
            }

            if(pattern.Length == 6)
            {
                var year = int.Parse(pattern.Substring(0, 4));
                var month = int.Parse(pattern.Substring(4, 2));
                return new DateTime(year, month, 1);
            }

            return DateTime.MinValue;
        }

        protected bool IsAnnualFolder(DirectoryInfo di)
        {
            string pattern = @"^\d{4}$";
            return Regex.IsMatch(di.Name, pattern);
        }

        protected bool IsMonthlyFolder(DirectoryInfo di)
        {
            string pattern = @"^\d{6}$";
            return Regex.IsMatch(di.Name, pattern);
        }

    }
}
