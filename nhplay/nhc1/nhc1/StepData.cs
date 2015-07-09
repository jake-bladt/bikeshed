using System;
using FluentNHibernate.Mapping;

namespace nhc1
{
    public class StepData
    {
        public virtual int Id { get; set; }
        public virtual DateTime Date { get; set; }
        public virtual int Steps { get; set; }
        public virtual User User { get; set; }
    }

    public class StepDataMap : ClassMap<StepData>
    {
        public StepDataMap()
        {
            Id(x => x.Id);
            Map(x => x.Date);
            Map(x => x.Steps);
            References(x => x.User);
        }
    }
}
