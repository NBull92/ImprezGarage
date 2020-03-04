using Prism.Events;
using System;

namespace ImprezGarage.Modules.PetrolExpenditure
{
    public class PetrolEvents
    {
        public class FilteredDatesChanged : PubSubEvent<Tuple<DateTime, DateTime>> { }
    }
}
