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
            List<Developer> dev = new List<Developer>();
            List<Commit> com = new List<Commit>();
            string[] txt = File.ReadAllLines(MyFile.GetFullNameInApplicationTree(Filename));

            return null;
            //    string[] dev = File.ReadAllLines(MyFile.GetFullNameInApplicationTree(Filename));
            /*  string[][] com = MyFile.ReadStringMatrixFromCsv(Filename, true);
            var developer = com
             .GroupBy(line => line[0])
             .Select(grp => new Developer()
             {
                 Name = grp.Key.ToString()
             })
             .ToArray();
            Commit[] commit = com
                .Select(line => new Commit()
                {
                    Developer = dev.Where(d => d.Name.Equals(line[0])).Single(),
                    Date = DateTime.Parse(line[1]),
                    FilesChanges = Convert.ToInt32(line[2])
                   /* Insertions = Int16.Parse(line[5]),
                    Deletions = Int16.Parse(line[6])

                })
                .ToArray();
            return commit;*/
        }

    }
}
