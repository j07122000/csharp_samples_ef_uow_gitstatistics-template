using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using GitStat.Core.Entities;
using Utils;

namespace GitStat.ImportConsole
{
    public class ImportController
    {
        const string Filename = "commits.txt";

        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static Commit[] ReadFromCsv()
        {
            string[][] matrix =  MyFile.ReadStringMatrixFromCsv(Filename, true);
            var developer = matrix
             .GroupBy(line => line[0])
             .Select(grp => new Developer
             {
                 Name = grp.Key
             })
             .ToArray();
            var commit = matrix
                .Select(line => new Commit
                {
                    Developer = developer.Single(d => d.Name == line[0]),
                    Date = DateTime.Parse(line[1]),
                    FilesChanges = Int16.Parse(line[4]),
                    Insertions = Int16.Parse(line[5]),
                    Deletions = Int16.Parse(line[6])

                })
                .ToArray();
            return commit;
        }

    }
}
