using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GitStat.Core.Contracts;
using GitStat.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GitStat.Persistence
{
    public class CommitRepository : ICommitRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public CommitRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddRange(Commit[] commits)
        {
            _dbContext.Commits.AddRange(commits);
        }
        public  Commit[] Commits4Weeks()
        {
            return null;
        }
         public Commit[] CommitWithId4()
        {
            return _dbContext.Commits
                .Where(i => i.DeveloperId == 4)
                .ToArray();
        }


    }
}