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
        private static List<Developer> _devops = new List<Developer>();
        //habe mir den code von einem Mitschüler genommen da ich nicht ganz 
        //wusste wie ich die Textdatei einlesen soll und den ImportController 
        //aber brauchte um weiter zu arbeiten
        //der ImportController funktioniert allerdings nicht ganz bei mir  
        /*  private static string newString = null;

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
          }*/



        /// <summary>
        /// Liefert die Messwerte mit den dazugehörigen Sensoren
        /// </summary>
        public static Commit[] ReadFromCsv()
        {
            List<Commit> commits = new List<Commit>();
            string[] lines = System.IO.File.ReadAllLines(MyFile.GetFullNameInApplicationTree(Filename));
            List<CommitBlock> blocks = GetBlocks(lines);
            return GetCommits(blocks);
            /* List<Developer> dev = new List<Developer>();
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
             return com.ToArray();*/
        }
        private static Commit[] GetCommits(List<CommitBlock> blocks)
        {
            List<Commit> commits = new List<Commit>();

            foreach (CommitBlock block in blocks)
            {
                ReadDevops(block);

                string[] head = block.Head.Split(',');
                string[] body = block.LastLine.Split(' ');

                Developer devop = _devops.Where(d => d.Name.Equals(head[1])).Single();
                string hashCode = head[0];
                DateTime date = DateTime.Parse(head[2]);
                string message = head[3];
                int filesChanged = Convert.ToInt32(body[1]);
                int insertions = GetInsertions(body);
                int deletions = GetDeletions(body);

                Commit newCommit = new Commit()
                {
                    Developer = devop,
                    HashCode = hashCode,
                    Date = date,
                    Message = message,
                    FilesChanges = filesChanged,
                    Insertions = insertions,
                    Deletions = deletions,
                };

                devop.Commits.Add(newCommit);
                commits.Add(newCommit);
            }

            return commits.ToArray();
        

          }
        private static int GetDeletions(string[] body)
        {
            if (body.Length == 6)
            {
                if (body[5].Equals("insertions(+)"))
                {
                    return 0;
                }
                else
                {
                    return Convert.ToInt32(body[4]);
                }
            }
            else
            {
                return Convert.ToInt32(body[6]);
            }
        }
        private static int GetInsertions(string[] body)
        {
            if (body.Length == 6)
            {
                if (body[5].Equals("insertions(+)"))
                {
                    return Convert.ToInt32(body[4]);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return Convert.ToInt32(body[6]);
            }
        }
        private static void ReadDevops(CommitBlock block)
        {
            string[] headData = block.Head.Split(',');

            if (_devops.Where(d => d.Name.Equals(headData[1])).SingleOrDefault() == null)
            {
                _devops.Add(new Developer()
                {
                    Name = headData[1]
                });
            }
        }
        private static List<CommitBlock> GetBlocks(string[] lines)
        {
            List<CommitBlock> blocks = new List<CommitBlock>();
            int idx = 0;

            while (idx < lines.Length)
            {
                CommitBlock block = new CommitBlock();

                while (idx < lines.Length && !String.IsNullOrEmpty(lines[idx]))
                {
                    if (idx + 1 == lines.Length)
                    {
                        block.LastLine = lines[idx];
                    }
                    else if (block.Head != null && String.IsNullOrEmpty(lines[idx + 1]))
                    {
                        block.LastLine = lines[idx];
                    }
                    else if (block.Head == null)
                    {
                        block.Head = lines[idx];
                    }

                    idx++;
                }

                blocks.Add(block);
                idx++;
            }

            return blocks;
        }
    }

}
