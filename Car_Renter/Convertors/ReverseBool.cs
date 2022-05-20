using System;

using System.Globalization;

using System.Windows.Data;

namespace Car_Renter.Convertors
{
    class ReverseBool : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool Data = bool.Parse( value.ToString());

            if (Data) Data = false;
            else if (Data==false) Data = true;


            return Data;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    
}
}
