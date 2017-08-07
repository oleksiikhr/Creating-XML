﻿using SQLite;

namespace XML.classes.db.currency
{
    class CurrencyTable
    {
        [PrimaryKey, Unique]
        public string CurrencyId { get; set; }
        
        [NotNull]
        public string Rate { get; set; }
    }
}
