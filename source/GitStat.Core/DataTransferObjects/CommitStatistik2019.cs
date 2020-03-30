using System;
using System.Collections.Generic;
using System.Text;

namespace GitStat.Core.DataTransferObjects
{
    class CommitStatistik2019
    {
        public string Name { get; set; }
        public DateTime Date { get; set; }

        public int FilesChanges { get; set; }
        public int Insertions { get; set; }
        public int Deletions { get; set; }
    }
}
