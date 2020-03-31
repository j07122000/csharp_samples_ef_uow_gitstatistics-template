using GitStat.Core.Entities;
using System;
using System.Threading.Tasks;

namespace GitStat.Core.Contracts
{
    public interface IDeveloperRepository
    {
        (string DeveloperName, int Commit, int FChanges, int Insertion, int Deletion)[] CommitAndDev();
    }
}
