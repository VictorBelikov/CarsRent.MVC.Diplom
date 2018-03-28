namespace CarsRentEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Create : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ДопОборудование",
                c => new
                    {
                        ОборудованиеID = c.Int(nullable: false, identity: true),
                        Наименование = c.String(nullable: false),
                        ИнвНомер = c.Int(nullable: false),
                        Стоимость = c.Double(nullable: false),
                        СтоимостьПрокатаВСут = c.Double(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ОборудованиеID);
            
            CreateTable(
                "dbo.Отчеты",
                c => new
                    {
                        ОтчетID = c.Int(nullable: false, identity: true),
                        Наименование = c.String(nullable: false, maxLength: 50),
                        ДатаФормирования = c.DateTime(nullable: false, storeType: "date"),
                        НачалоПериода = c.DateTime(nullable: false, storeType: "date"),
                        КонецПериода = c.DateTime(nullable: false, storeType: "date"),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ОтчетID);
            
            CreateTable(
                "dbo.Автомобили",
                c => new
                    {
                        АвтоID = c.Int(nullable: false, identity: true),
                        Госномер = c.String(nullable: false, maxLength: 10),
                        VIN = c.String(nullable: false, maxLength: 20),
                        Марка = c.String(nullable: false, maxLength: 20),
                        Модель = c.String(nullable: false, maxLength: 20),
                        ГодВыпуска = c.Int(nullable: false),
                        ТипКузова = c.String(nullable: false, maxLength: 20),
                        ТипДвигателя = c.String(nullable: false, maxLength: 20),
                        ОбъемДвигателя = c.Double(nullable: false),
                        Мощность = c.Int(nullable: false),
                        Цвет = c.String(maxLength: 20),
                        Стоимость = c.Double(nullable: false),
                        СтоимостПрокатаВСут = c.Double(nullable: false),
                        Статус = c.String(nullable: false, maxLength: 10),
                        Image = c.Binary(),
                        MimeType = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.АвтоID)
                .Index(t => t.Госномер, unique: true, name: "IDX_Автомобили_Госномер")
                .Index(t => t.VIN, unique: true, name: "IDX_Автомобили_VIN");
            
            CreateTable(
                "dbo.Аренда",
                c => new
                    {
                        АрендаID = c.Int(nullable: false, identity: true),
                        КлиентID = c.Int(nullable: false),
                        АвтоID = c.Int(nullable: false),
                        ДатаВыдачи = c.DateTime(nullable: false, storeType: "date"),
                        ДатаВозврата = c.DateTime(nullable: false, storeType: "date"),
                        ИтоговаяСумма = c.Double(nullable: false),
                        Состояние = c.String(nullable: false, maxLength: 20),
                        Скидки = c.String(),
                        ДопОборудование = c.String(),
                        Штрафы = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.АрендаID)
                .ForeignKey("dbo.Автомобили", t => t.АвтоID, cascadeDelete: true)
                .ForeignKey("dbo.Клиенты", t => t.КлиентID, cascadeDelete: true)
                .Index(t => t.КлиентID)
                .Index(t => t.АвтоID);
            
            CreateTable(
                "dbo.Клиенты",
                c => new
                    {
                        КлиентID = c.Int(nullable: false, identity: true),
                        ФИО = c.String(nullable: false, maxLength: 50),
                        Адрес = c.String(nullable: false, maxLength: 50),
                        Телефон = c.String(nullable: false, maxLength: 15),
                        НомерПаспорта = c.String(nullable: false, maxLength: 20),
                        НомерВодУд = c.String(nullable: false, maxLength: 20),
                        ЧерныйСписок = c.String(nullable: false, maxLength: 3),
                        Image = c.Binary(),
                        MimeType = c.String(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.КлиентID)
                .Index(t => t.НомерПаспорта, unique: true, name: "IDX_Клиенты_НомерПаспорта")
                .Index(t => t.НомерВодУд, unique: true, name: "IDX_Клиенты_НомерВодУд");
            
            CreateTable(
                "dbo.Скидки",
                c => new
                    {
                        СкидкаID = c.Int(nullable: false, identity: true),
                        НаименованиеСкидки = c.String(nullable: false, maxLength: 30),
                        Процент = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.СкидкаID);
            
            CreateTable(
                "dbo.Штрафы",
                c => new
                    {
                        ШтрафID = c.Int(nullable: false, identity: true),
                        НаименованиеШтрафа = c.String(nullable: false, maxLength: 30),
                        СуммаШтрафа = c.Double(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.ШтрафID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Аренда", "КлиентID", "dbo.Клиенты");
            DropForeignKey("dbo.Аренда", "АвтоID", "dbo.Автомобили");
            DropIndex("dbo.Клиенты", "IDX_Клиенты_НомерВодУд");
            DropIndex("dbo.Клиенты", "IDX_Клиенты_НомерПаспорта");
            DropIndex("dbo.Аренда", new[] { "АвтоID" });
            DropIndex("dbo.Аренда", new[] { "КлиентID" });
            DropIndex("dbo.Автомобили", "IDX_Автомобили_VIN");
            DropIndex("dbo.Автомобили", "IDX_Автомобили_Госномер");
            DropTable("dbo.Штрафы");
            DropTable("dbo.Скидки");
            DropTable("dbo.Клиенты");
            DropTable("dbo.Аренда");
            DropTable("dbo.Автомобили");
            DropTable("dbo.Отчеты");
            DropTable("dbo.ДопОборудование");
        }
    }
}
