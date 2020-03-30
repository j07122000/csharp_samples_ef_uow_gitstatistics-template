using System;
using System.IO;
using ConsoleTables;
using System.Linq;
using System.Text;
using GitStat.Core.Contracts;
using GitStat.Core.Entities;
using GitStat.Persistence;

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
                Console.WriteLine("Commits der letzten 4 Wochen");
                Console.WriteLine("----------------------------");
                Console.WriteLine();
                var weeks = unitOfWork.CommitRepository
                    .Commits4Weeks();
                Print(
                    $"Team Tabelle (sortiert nach Rang):",
                    ConsoleTable
                        .From(weeks)
                        .Configure(o => o.NumberAlignment = Alignment.Right)
                        .ToStringAlternative());

                Console.WriteLine("Commits der letzten 4 Wochen");
                Console.WriteLine("----------------------------");
                Console.WriteLine();
                var id = unitOfWork.CommitRepository
                    .CommitWithId4();
                //WriteMeasurements(id);
            }

            Console.Write("Beenden mit Eingabetaste ...");
            Console.ReadLine();
        }
        private static void Print(string c, string res)
        {
            Console.WriteLine();

            if (!string.IsNullOrEmpty(c))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(new String('=', c.Length));
                Console.WriteLine(c);
                Console.WriteLine(new String('=', c.Length));
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(res);
            Console.ResetColor();
            Console.WriteLine();
        }

    }

}
