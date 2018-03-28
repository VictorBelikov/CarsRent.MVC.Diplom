namespace CarsRentMVC.Models
{
    public class MenuItem
    {
        public string Name { get; set; } // Текст надписи
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Active { get; set; } // Текущий пункт
    }
}