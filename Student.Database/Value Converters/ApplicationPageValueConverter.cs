using System;
using System.Diagnostics;
using System.Globalization;

namespace Student.Database
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public class ApplicationPageValueConverter : BaseValueConverter<ApplicationPageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Find the appropriate page
            switch ((ApplicationPage)value)
            {
                case ApplicationPage.Dashboard:
                    return new DashboardPage();

                case ApplicationPage.Insert:
                    return new InsertPage();

                case ApplicationPage.Load:
                    return new LoadPage();

                case ApplicationPage.Edit:
                    return new EditPage();

                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
