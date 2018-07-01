﻿using SQLite;

namespace Creating_XML.core.db.tables
{
    public class OfferImageTable : Table
    {
        [AutoIncrement, PrimaryKey]
        public int Id { get; set; }

        [Indexed(Name = "Image", Order = 1, Unique = true), NotNull]
        public int OfferId { get; set; }

        [Indexed(Name = "Image", Order = 2, Unique = true), NotNull]
        public string Url { get; set; }
    }
}