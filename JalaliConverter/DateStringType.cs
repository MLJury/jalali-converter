using System;

namespace JalaliConverter
{
    [Flags]
    public enum DateStringType : byte
    {
        Unknown = 0,
        SubDateString = 1,
        String = 2,
        Digit = 3
    }
}
