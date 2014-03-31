using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gallery.Entities.Utilities
{
    public class DuplicateElectionEntryException : System.Exception
    {
        public DuplicateElectionEntryException() : base() {}
        public DuplicateElectionEntryException(string msg) : base(msg) { }
    }

    public class UnknownSubjectException : System.Exception
    {
        public UnknownSubjectException() : base() { }
        public UnknownSubjectException(string msg) : base(msg) { }
    }

    public class DuplicateSubjectException : System.Exception
    {
        public DuplicateSubjectException() : base() { }
        public DuplicateSubjectException(string msg) : base(msg) { }
    }

    public class EmptyElectionSlotException : System.Exception
    {
        public EmptyElectionSlotException() : base() { }
        public EmptyElectionSlotException(string msg) : base(msg) { }
    }

    public class ElectionMigrationException : System.Exception
    {
        public ElectionMigrationException() : base() { }
        public ElectionMigrationException(string msg) : base(msg) { }
    }


}
