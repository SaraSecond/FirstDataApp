using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFDataApp.Models
{
    public class SortViewModel
    {
        public SortState NameSort { get; set; }
        public SortState ManufacturerSort { get; private set; }
        public SortState PriceSort { get; private set; }
        public SortState StoreSotr { get; private set; }
        public SortState CountSort { get; set; }
        public SortState Current { get; set; }


        public SortViewModel()
        {

        }

        public SortViewModel(SortState sortOrder)
        {
            NameSort = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ManufacturerSort = sortOrder == SortState.ManufacturerAsc ? SortState.ManufacturerDesc : SortState.ManufacturerAsc;
            PriceSort = sortOrder == SortState.PriceAsc ? SortState.PriceDesc : SortState.PriceAsc;
            StoreSotr = sortOrder == SortState.StoreAsc ? SortState.StoreDesc : SortState.StoreAsc;
            CountSort = sortOrder == SortState.CountAsc ? SortState.CountDesc : SortState.CountAsc;

            Current = sortOrder;
        }

        
}
}
