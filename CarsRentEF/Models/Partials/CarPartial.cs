using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarsRentEF.Models.MetaData;

namespace CarsRentEF.Models
{
    [MetadataType(typeof(CarMetaData))]
    public partial class Car
    {
        public override string ToString() {
            return $"{АвтоID} {Марка} {this.Модель} {this.ТипКузова} {this.ГодВыпуска} " +
                   $"{this.Цвет ?? "Undefined color"}. Стоимость: {this.Стоимость:f0}$";
        }

        // Вычисляемое поле. Не сохраняется в БД, не будет заполняться при создании объекта из БД
        [NotMapped]
        public string BrandColor => $"{Модель} + ({Цвет})";
    }
}
