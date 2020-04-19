using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Text;

using Gallery.Entities.Elections;
using Gallery.Entities.ImageGallery;
using Gallery.Entities.Taxonomy;
using Gallery.Migration;

using Polly;

namespace migrate
{
    class Program
    {
        static int Main(string[] args)
        {

            Policy.
                Handle<SqlException>()
                .WaitAndRetry(5, t => TimeSpan.FromSeconds(t ^ 2));

            var parts = args.Length > 0 ? args[0] : "*";

            if(parts == "*" || parts == "categories")
            {
                Console.WriteLine("Migrating categories");
                var cateResult = MigrateCategories();
                if (!cateResult) return ExitOn("Category migration failed.");
            }

            if (parts == "*" || parts == "subjects")
            {
                Console.WriteLine("Migrating subjects.");
                var subjectResult = MigrateSubjects();
                if (!subjectResult) return ExitOn("Subject migration failed.");
            }

            if (parts == "*" || parts == "elections")
            {
                Console.WriteLine("Migrating elections.");
                var electionResult = MigrateElections();
                if (!electionResult) return ExitOn("Election migration failed.");

                var historyResult = MigrateHistoricRanks();
                if (!historyResult) return ExitOn("Historic ranks migration failed.");
            }

            if(parts == "rcate")
            {
                Console.WriteLine("Restoring categories.");
                var restoreResult = ReverseMigrateSubjectCategories();
                if (!restoreResult) return ExitOn("Category restoration failed.");
            }

            return ExitOn("Migration complete.");
        }

        public static int ExitOn(string step)
        {
            Console.WriteLine(step);
            Console.ReadLine();
            return 0;
        }

        private static string GetDbConnectionString()
        {
            var env = Environment.GetEnvironmentVariable("GALLERY_CONSTR");
            return String.IsNullOrEmpty(env) ? 
                ConfigurationManager.ConnectionStrings["galleryDb"].ConnectionString : env;
        }

        public static bool MigrateSubjects()
        {
            var connStr = GetDbConnectionString();
            var dbGallery = new SqlTrackedImageGallery(connStr);
            int dbCount = dbGallery.Subjects.Count;
            Console.WriteLine(String.Format("{0} subjects in the database.", dbCount.ToString("#,##0")));

            var galleryPath = ConfigurationManager.AppSettings["gallerySource"].ToString();
            var fsoGallery = new FileSystemImageGallery(galleryPath);
            int fsCount = fsoGallery.Subjects.Count;
            Console.WriteLine(String.Format("{0} subjects in the file system.", fsCount.ToString("#,##0")));

            var res = SubjectMigrationHelper.MigrateToDB(fsoGallery, dbGallery);
            Console.WriteLine(String.Format("{0} subject(s) successfully migrated.", res.Saved));
            Console.WriteLine(String.Format("{0} subject(s) failed to migrate.", res.Failures));

            return true;
        }

        public static bool MigrateElections()
        {
            string rootPath = ConfigurationManager.AppSettings["electionSource"];
            var connStr = GetDbConnectionString();

            var sqlElectionResultSetFactory = new SqlBackedElectionResultSetFacotry(connStr);
            var dbGallery = new SqlTrackedImageGallery(connStr);
            var fsElectionResultSetFactory = new FileSystemElectionResultSetFactory(rootPath, dbGallery);

            var sqlSet = sqlElectionResultSetFactory.GetResultSet();
            var sqlCount = sqlSet.Count.ToString("#,##0");
            Console.WriteLine($"{sqlCount} results in DB.");

            var fsSet = fsElectionResultSetFactory.GetResultSet();
            var fsCount = fsSet.Count.ToString("#,##0");
            Console.WriteLine($"{fsCount} results in file system.");
            var fsParseErrorCount = fsSet.ParseErrors.Count.ToString("#,##0");
            Console.WriteLine($"{fsParseErrorCount} parse errors found in file system.");
            fsSet.ParseErrors.ForEach(Console.WriteLine);

            var electionWriter = new SqlBackedElectionWriter(dbGallery, connStr);
            var helper = new ElectionMigrationHelper(electionWriter);
            var deltas = helper.GetDeltas(fsSet, sqlSet);
            var deltaCount = deltas.Count.ToString("#,##0");
            Console.WriteLine($"{deltaCount} deltas found.");

            return helper.ApplyDeltas(deltas);
        }

        public static bool MigrateHistoricRanks()
        {
            var connStr = GetDbConnectionString();
            var helper = new HistoricRanksMigrationHelper(connStr);
            var result = helper.UpdateHistoricRanks();
            if(result.Success)
            {
                Console.WriteLine($"Completed history migration with {result.EntryCount:0,00#} historic rank entries.");
            }
            else
            {
                Console.WriteLine($"Failed history migration. Error: {result.ExceptionReported}.");
            }
            return result.Success;
        }

        public static bool MigrateCategories()
        {
            var connStr = GetDbConnectionString();
            var helper = new CategoryMigrationHelper(connStr);
            var rootDir = ConfigurationManager.AppSettings["galleryRoot"];

            var outputPath = Path.Combine(rootDir, "cates.txt");
            var writer = new CategoryMigrationWriter(outputPath);
            var importCount = helper.Migrate((l) => writer.WriteExportLine(l));
            Console.WriteLine($"{importCount.ToString("#,##0")} subject categories migrated.");
            writer.Dispose();
            return true;
        }

        private class SubjectCategories
        {
            public string SubjectName { get; set; }
            public List<String> Categories { get; private set; }

            public SubjectCategories()
            {
                Categories = new List<String>();
            }

            public static SubjectCategories FromLine(string line)
            {
                var lineAsChars = line.ToCharArray();
                const char DQUOTE = '"';
                const char SPACE = ' ';
                var token = new StringBuilder();
                var tokenNum = 0;
                var inQuotedString = false;
                var ret = new SubjectCategories();

                for(int i = 0; i < lineAsChars.Length; i++)
                {
                    var c = lineAsChars[i];

                    if (DQUOTE == c)
                    {
                        inQuotedString = !inQuotedString;
                    }
                    else if (SPACE == c)
                    {
                        if(inQuotedString)
                        {
                            token.Append(SPACE);
                        }
                        else
                        {
                            var val = token.ToString();
                            token = new StringBuilder();

                            if(0 == tokenNum)
                            {
                                ret.SubjectName = val;
                            }
                            else
                            {
                                ret.Categories.Add(val);
                            }
                            tokenNum++;
                        }
                    }
                    else
                    {
                        token.Append(c);
                    }
                }
                if(tokenNum > 0) ret.Categories.Add(token.ToString());

                return ret;
            }
            
        }

        public static bool ReverseMigrateSubjectCategories()
        {
            var connStr = GetDbConnectionString();
            var rootDir = ConfigurationManager.AppSettings["galleryRoot"];
            var inputPath = Path.Combine(rootDir, "cates.txt");

            var lines = File.ReadAllLines(inputPath);
            var subjectCategories = new List<SubjectCategories>();
            for(int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if(line.Length > 0)
                {
                    var sc = SubjectCategories.FromLine(line);
                    subjectCategories.Add(sc);
                }
            }

            var repo = new SubjectCategoryRepository(connStr);
            subjectCategories.ForEach(sc =>
            {
                sc.Categories.ForEach(c => {
                    Console.WriteLine($"{sc.SubjectName}: {c}");
                    repo.SetSubjectCategory(sc.SubjectName, c);
                });
            });

            return true;
        }
    }
}
