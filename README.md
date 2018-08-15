# jalali-converter
Convert Jalali datetime to Gregorian datetime and vice versa
## Getting Started
```
    DateTime dateTimeOne = new DateTime(year: 1992, month: 01, day:01);
    DateTime dateTimeTwo = new DateTime(year: 2000, month: 05, day: 05);

    PersianDate persianDateOne = (PersianDate)dateTimeOne;
    string persianDateTimeString = persianDateOne.ToString(DateStringType.Digit);

    PersianDate persianDateTwo = (PersianDate)dateTimeOne;
    string persianDateTimeTwoString = persianDateOne.ToString(DateStringType.Digit);

    PersianDate persianDateSubtraction = ((PersianDate)dateTimeOne - (PersianDate)dateTimeTwo);
    string persianDateSubtractionString = persianDateSubtraction.ToString(DateStringType.SubDateString);
```
