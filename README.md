# CarsRent.MVC.Diplom
На основе функциональных требований к программному средству была разработана архитектура системы. После анализа современных архитектурных подходов, а также принципов проектирования приложений, была выбрана двухслойная архитектура, состоящая из уровня работы с базой данных и уровня бизнес-логики и представления. В средних и больших проектах используется трехуровневая архитектура с разделением бизнес-логики и представления на отдельные уровни. В нашем случае, в этом не было необходимости, т.к. такой подход лишь усложнил бы процесс разработки лишней перегонкой данных по уровням. Второй уровень нашего приложения выполнен с применением паттерна MVC, который позволяет разделять ответственности работы с кодом и делает разработку структурированной и удобной. На основании выбранного решения были разработаны и детально рассмотрены схема ресурсов программного средства. Для хранения данных выбрана СУБД MSSQL Server 2014. База данных разработана с подходом Code First. Серверная сторона программного средства разработана на объектно-ориентированном языке высокого уровня C#, на клиентской стороне был использован язык программирования JavaScript, язык гипертекстовой разметки HTML5, таблицы каскадных стилей CSS3.
Для обеспечения корректной и надежной работы функций приложения был разработан ряд тестовых сценариев. Все они были успешно пройдены при тестировании приложения. При проектировании системы был внедрен механизм внедрения зависимостей, который позволил избежать агрегации классов и связать сущности через использование интерфейсов. Это решение позволит при необходимости с легкостью перейти на автоматизированное тестирование с помощью Moq фреймворка.
