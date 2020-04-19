using System.Collections.Generic;
using System.Linq;

using Gallery.Entities.ImageGallery;
using Gallery.Entities.Subjects;

namespace Gallery.Migration
{
    public class SubjectMigrationHelper
    {
        public class Result
        {
            public int Saved { get; set; }
            public int Failures { get; set; }

            public Result()
            {
                Saved = 0; Failures = 0;
            }
        }

        public static List<ISubject> GetDeltas(IImageGallery source, IImageGallery target)
        {
            var ret = new List<ISubject>();
            source.Subjects.ToList().ForEach(kvp =>
            {
                var key = kvp.Key;
                if (target.Subjects.ContainsKey(key))
                {
                    var sourceSubject = kvp.Value;
                    var targetSubject = target.Subjects[key];
                    if (sourceSubject.Name != targetSubject.Name || sourceSubject.ImageCount != targetSubject.ImageCount)
                    {
                        ret.Add(sourceSubject);
                    }
                }
                else
                {
                    ret.Add(kvp.Value);
                }
            });
            return ret;
        }

        public static Result MigrateToDB(IImageGallery source, SqlTrackedImageGallery target)
        {
            var ret = new Result();
            var deltas = GetDeltas(source, target);

            deltas.ForEach(d =>
            {
                if (target.Add(d))
                {
                    ret.Saved++;
                }
                else
                {
                    ret.Failures++;
                };
            });

            return ret;
        }
    }
}
