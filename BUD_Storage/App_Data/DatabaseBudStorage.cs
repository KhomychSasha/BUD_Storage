namespace BUD_Storage.App_Data
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class DatabaseBudStorage : DbContext
    {
        public DatabaseBudStorage()
            : base("name=DatabaseBudStorage")
        {
        }

        public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    public class MyEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}