namespace BUD_Storage.App_Data
{
    using BUD_Storage.ExcelTables;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Windows;

    class CustomContextInitializer : DropCreateDatabaseIfModelChanges<DatabaseBudStorage>
    {
        protected override void Seed(DatabaseBudStorage db)
        {
            Dictionary<int, string> units_in_db = new Dictionary<int, string>();

            // UnitsOfMeasurement
            string filePathForUnitsOfMeasurement = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\StartDB\Units_of_measurement.xlsx";
            Excel tableUnitsOfMeasurement = new Excel(filePathForUnitsOfMeasurement, 1);

            for (int i = 2; i <= Int32.Parse(tableUnitsOfMeasurement.ReadCell(1, 1)); ++i)
            {
                Unit_Of_Measurement unit = new Unit_Of_Measurement()
                {
                    Code = Int32.Parse(tableUnitsOfMeasurement.ReadCell(i, 1)),
                    Name = tableUnitsOfMeasurement.ReadCell(i, 2),
                    Marking = tableUnitsOfMeasurement.ReadCell(i, 3)
                };

                units_in_db.Add(i - 1, unit.Marking);
                 
                db.Entities_Unit_Of_Measurements.Add(unit);
            }
            tableUnitsOfMeasurement.CloseFile();

            //Products
            string filePathForProducts = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\StartDB\Products.xlsx";
            Excel tableProducts = new Excel(filePathForProducts, 1);

            for (int i = 2; i <= Int32.Parse(tableProducts.ReadCell(1, 1)); ++i)
            {
                Product prod = new Product()
                {
                    Code = Int32.Parse(tableProducts.ReadCell(i, 1)),
                    Name = tableProducts.ReadCell(i, 2),
                    King = tableProducts.ReadCell(i, 3),
                };

                foreach (var unt in units_in_db)
                {
                    if (tableProducts.ReadCell(i, 4) == unt.Value)
                    {
                        prod.IdUnit_Of_Measurement = unt.Key;
                    }
                }
                
                db.Entities_Products.Add(prod);
            }
            tableProducts.CloseFile();

            //Contractors
            string filePathForContractors = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\StartDB\Contractors.xlsx";
            Excel tableContractors = new Excel(filePathForContractors, 1);

            for (int i = 2; i <= Int32.Parse(tableContractors.ReadCell(1, 1)); ++i)
            {
                Contractor contract = new Contractor()
                {
                    Short_Name = tableContractors.ReadCell(i, 1),
                    Full_Name = tableContractors.ReadCell(i, 2),
                    EDRPOU = tableContractors.ReadCell(i, 3),
                };

                db.Entities_Contractors.Add(contract);
            }
            tableContractors.CloseFile();

            //Warehouses
            string filePathForWarehouses = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + @"\StartDB\Warehouses.xlsx";
            Excel tableWarehouses = new Excel(filePathForWarehouses, 1);

            for (int i = 2; i <= Int32.Parse(tableWarehouses.ReadCell(1, 1)); ++i)
            {
                Warehouse warehouse = new Warehouse()
                {
                    Name = tableWarehouses.ReadCell(i, 1),
                };

                db.Entities_Warehouses.Add(warehouse);
            }
            tableWarehouses.CloseFile();

            db.SaveChanges();
        }
    }

    public class DatabaseBudStorage : DbContext
    {
        static DatabaseBudStorage()
        {
            Database.SetInitializer<DatabaseBudStorage>(new CustomContextInitializer());
        }

        public DatabaseBudStorage()
            : base("name=DatabaseBudStorage")
        {
        }

        public virtual DbSet<Unit_Of_Measurement> Entities_Unit_Of_Measurements { get; set; }
        public virtual DbSet<Product> Entities_Products { get; set; }
        public virtual DbSet<Warehouse> Entities_Warehouses { get; set; }
        public virtual DbSet<Contractor> Entities_Contractors { get; set; }
        public virtual DbSet<Product_In_The_Warehouse> Entities_Product_In_The_Warehouses { get; set; }
        public virtual DbSet<Invoice> Entities_Invoices { get; set; }
        public virtual DbSet<Moving> Entities_Movings { get; set; }
    }

    public class Unit_Of_Measurement
    {
        public int Id { get; set; }
        public int Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Marking { get; set; }

        public virtual ICollection<Product> Product { get; set; }
    }

    public class Product
    {
        public int Id { get; set; }
        public int Code { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string King { get; set; }

        public int IdUnit_Of_Measurement { get; set; }
        public Unit_Of_Measurement Unit_Of_Measurements { get; set; }
    }

    public class Warehouse
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Comment { get; set; }

        public virtual ICollection<Product_In_The_Warehouse> Product_In_The_Warehouses { get; set; }
    }

    public class Contractor
    {
        public int Id { get; set; }
        [Required]
        public string Short_Name { get; set; }
        [Required]
        public string Full_Name { get; set; }
        [Required]
        public string EDRPOU { get; set; }

        public virtual ICollection<Product_In_The_Warehouse> Product_In_The_Warehouses { get; set; }
    }

    public class Product_In_The_Warehouse
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public int IdWarehouse { get; set; }
        public int IdContractor { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int VAT { get; set; }
        public decimal Price_VAT { get; set; }

        public Product Products { get; set; }
        public Warehouse Warehouses { get; set; }
        public Contractor Contractors { get; set; }

        public virtual ICollection<Invoice> Invoices { get; set; }
        public virtual ICollection<Moving> Movings { get; set; }
    }

    public class Invoice
    {
        public int Id { get; set; }
        public int NumberInvoice { get; set; }
        public DateTime DateInvoice { get; set; }

        public int IdProduct_In_The_Warehouse { get; set; }
        public Product_In_The_Warehouse Product_In_The_Warehouses { get; set; }
    }

    public class Moving
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public DateTime DateMoving { get; set; }
        public int First_Warehouse { get; set; }
        public int Second_Warehouse { get; set; }


        public int IdProduct_In_The_Warehouse { get; set; }
        public Product_In_The_Warehouse Product_In_The_Warehouses { get; set; }
        public Warehouse Warehouses { get; set; }
    }
}