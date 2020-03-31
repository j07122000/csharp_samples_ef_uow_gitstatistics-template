using System;
using System.IO;
using ConsoleTables;
using System.Linq;
using System.Text;
using GitStat.Core.Contracts;
using GitStat.Core.Entities;
using GitStat.Persistence;
using System.Collections.Generic;

namespace GitStat.ImportConsole
{
    class Program
    {
        static async System.Threading.Tasks.Task Main()
        {
            Console.WriteLine("Import der Commits in die Datenbank");
           using (IUnitOfWork unitOfWorkImport = new UnitOfWork())
            {
                Console.WriteLine("Datenbank löschen");
                unitOfWorkImport.DeleteDatabase();
                Console.WriteLine("Datenbank migrieren");
                unitOfWorkImport.MigrateDatabase();
                Console.WriteLine("Commits werden von commits.txt eingelesen");
                var commits = ImportController.ReadFromCsv();
                if (commits.Length == 0)
                {
                    Console.WriteLine("!!! Es wurden keine Commits eingelesen");
                    return;
                }
                Console.WriteLine(
                    $"  Es wurden {commits.Count()} Commits eingelesen, werden in Datenbank gespeichert ...");
                unitOfWorkImport.CommitRepository.AddRange(commits);
                int countDevelopers = commits.GroupBy(c => c.Developer).Count();
                int savedRows = unitOfWorkImport.SaveChanges();
                Console.WriteLine(
                    $"{countDevelopers} Developers und {savedRows - countDevelopers} Commits wurden in Datenbank gespeichert!");
                Console.WriteLine();
                var csvCommits = commits.Select(c =>
                    $"{c.Developer.Name};{c.Date};{c.Message};{c.HashCode};{c.FilesChanges};{c.Insertions};{c.Deletions}");
                File.WriteAllLines("commits.csv", csvCommits, Encoding.UTF8);
            }
            Console.WriteLine("Datenbankabfragen");
            Console.WriteLine("=================");
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
             
                Console.WriteLine();
                Console.WriteLine("Commits der letzten 4 Wochen");
                Console.WriteLine("----------------------------");
                var weeks = unitOfWork.CommitRepository
                    .Commits4Weeks();
                 WriteCommit(weeks);
                Console.WriteLine();

                Console.WriteLine("Commit mit Id 4");
                Console.WriteLine("----------------------------");
                Console.WriteLine();
                var id = unitOfWork.CommitRepository
                    .CommitWithId4();
                Console.WriteLine(id.ToString());
                Console.WriteLine();

                Console.WriteLine("Statistik der Commits der Developer");
                Console.WriteLine("----------------------------");
                Console.WriteLine();
                var statistik =  unitOfWork.DeveloperRepository
                   .CommitAndDev();
                WriteStatistik(statistik);
                Console.WriteLine();

            }

            Console.Write("Beenden mit Eingabetaste ...");
            Console.ReadLine();
        }
     
        private static void WriteCommit(Commit[] commits)
        {
            Console.WriteLine("Developer            Date        FileChanges       Insertions       Deletions");
            for (int i = 0; i < commits.Length; i++)
            {
                Console.WriteLine($"{commits[i].Developer.Name}       {commits[i].Date.ToShortDateString()}          {commits[i].FilesChanges}       {commits[i].Insertions}         {commits[i].Deletions}");
            }
        }
        private static void WriteStatistik((string DeveloperName, int Commit, int FChanges, int Insertion, int Deletion)[] values)
        {
            Console.WriteLine("Developer            Commits        FileChanges       Insertions       Deletions");
            for (int i = 0; i < values.Length; i++)
            {
                var result = values[i];
                Console.WriteLine($"{result.DeveloperName}       {result.Commit}          {result.FChanges}       {result.Insertion}         {result.Deletion}");
            }
        }
    }

}
