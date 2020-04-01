using GitStat.Core.Contracts;
using GitStat.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace GitStat.Persistence
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DeveloperRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public  (string DeveloperName, int Commit, int FChanges, int Insertion, int Deletion)[] CommitAndDev()
        {
            return  _dbContext.Developers
                .Select(s => new
                {
                    DeveloperName = s.Name,
                   Commits = s.Commits.Count,
                   FChanges = s.Commits.Sum(c => c.FilesChanges),
                   Insertion = s.Commits.Sum(c => c.Insertions),
                   Deletion = s.Commits.Sum(c => c.Deletions)

                })
                .OrderByDescending(d => d.Commits)
                .AsEnumerable()                 //wird zu enumerable konvertiert
                .Select(c => (c.DeveloperName, c.Commits, c.FChanges, c.Insertion, c.Deletion))
                .ToArray();
          
            

        }


    }
}