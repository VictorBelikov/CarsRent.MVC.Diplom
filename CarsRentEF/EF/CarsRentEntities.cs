namespace CarsRentEF.EF
{
    using System.Data.Entity;
    using Models;

    public class CarsRentEntities : DbContext
    {
        public CarsRentEntities() : base("name=CarsRentConnection") { }

        public virtual DbSet<Rent> Аренды { get; set; }
        public virtual DbSet<Car> Автомобили { get; set; }
        public virtual DbSet<Client> Клиенты { get; set; }
        public virtual DbSet<Discount> Скидки { get; set; }
        public virtual DbSet<Penalty> Штрафы { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
    }
}