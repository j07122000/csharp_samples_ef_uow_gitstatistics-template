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
        private static string newString = null;

        public static bool Header(string[] array)
        {
            if (array.Length > 4)
            {
                foreach (var item in array)
                {

                    newString = newString + item + ';';
                }
            }
            return true;
        }
        public static bool Body(string[] array)
        {
            if (Header(array) == true)
            {
                foreach (var item in array)
                {

                    newString = newString + item + ';';
                }
            }
            return true;
        }


        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static Commit[] ReadFromCsv()
        {
            List<Developer> dev = new List<Developer>();
            List<Commit> com = new List<Commit>();
            string[] txt = File.ReadAllLines(MyFile.GetFullNameInApplicationTree(Filename));

            foreach (var a in txt)
            {

                if (Header(txt) == true && Body(txt) == false)
                {
                    Commit commit;
                   // Developer deve;

                    string[] commits = newString.Split(';');
                    com.Add(commit = new Commit
                    {
                        Developer = new Developer
                        {
                            Name = commits[1]
                        },
                        
                        Date = DateTime.Parse(commits[2]),
                        FilesChanges = Convert.ToInt32(commits[4]),
                        Insertions  = Convert.ToInt32(commits[5]),

                    });
                
                   
                }
               
            }
            return com.ToArray();
        }
    }
}
