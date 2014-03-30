using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Utilities
{
    public class NameMapper
    {
        public static string DirectoryNameToDisplayName(string dirName)
        {
            var retBuilder = new StringBuilder();

            string working = dirName.Replace(".", " ");
            var workingArr = working.ToCharArray();
            bool capNext = true;

            for (int i = 0; i < workingArr.Length; i++)
            {
                retBuilder.Append(capNext ? workingArr[i].ToString().ToUpper() : workingArr[i].ToString());
                capNext = (workingArr[i] == ' ' || workingArr[i] == '-');
            }

            var ret = retBuilder.ToString();
            string endsWith = ret.Substring(ret.Length - 1);
            if (endsWith == endsWith.ToUpper()) ret += ".";

            return ret;
        }

        public static Tuple<int, String> ElectionFileNameToElectionEntry(string line)
        {
            int pos = 0;
            string name = String.Empty;
            
            // Set up most common exception for reuse.
            string invalidFileName = String.Format("{0} is not a valid election file name.", line);
            var invalidNameException = new ArgumentException(invalidFileName);

            if (!line.Contains("-")) throw invalidNameException;
            var strPos = line.Split('-')[0];
            if (!Int32.TryParse(strPos, out pos)) throw invalidNameException;
            name = line.Replace(strPos + "-", String.Empty).Replace(".jpg", String.Empty);

            return new Tuple<int, string>(pos, name);
        }

    }
}
