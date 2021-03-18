using System;
using System.Collections.Generic;
using System.Text;
using Avalonia.Data.Converters;
using System.Globalization;
using Globe3DLight.Time;

namespace Globe3DLight.AvaloniaUI.Converters
{

    //public class TimePresenterToStringConverter : IValueConverter
    //{
    //    /// <summary>
    //    /// Converts a value.
    //    /// </summary>
    //    /// <param name="value">The value to convert.</param>
    //    /// <param name="targetType">The type of the target.</param>
    //    /// <param name="parameter">A user-defined parameter.</param>
    //    /// <param name="culture">The culture to use.</param>
    //    /// <returns>The converted value.</returns>
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value != null && value is ITimePresenter timePresenter)
    //        {
    //            var dt0 = timePresenter.Begin;// timeInterval.BeginTime;
    //            var dt = dt0.AddSeconds(timePresenter.CurrentTime /*timeInterval.CurrentTime*/);
    //            return dt.ToLongDateString() + " " + dt.ToLongTimeString();

    //            //    var dt0 = timePresenter.BeginTime;// DateTime.FromOADate(JulianDateBegin - 2415018.5);

    //            //    var dt = dt0.AddSeconds(timePresenter.CurrentTime);

    //            //   return dt.ToLongDateString() + " " + dt.ToLongTimeString();
    //        }

    //        return string.Empty;
    //    }

    //    /// <summary>
    //    /// Converts a value.
    //    /// </summary>
    //    /// <param name="value">The value to convert.</param>
    //    /// <param name="targetType">The type of the target.</param>
    //    /// <param name="parameter">A user-defined parameter.</param>
    //    /// <param name="culture">The culture to use.</param>
    //    /// <returns>The converted value.</returns>
    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


    //public class TimePresenterToSliderConverter : IValueConverter
    //{
    //    /// <summary>
    //    /// Converts a value.
    //    /// </summary>
    //    /// <param name="value">The value to convert.</param>
    //    /// <param name="targetType">The type of the target.</param>
    //    /// <param name="parameter">A user-defined parameter.</param>
    //    /// <param name="culture">The culture to use.</param>
    //    /// <returns>The converted value.</returns>
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        if (value != null && value is TimeInterval timeInterval/*ITimePresenter timePresenter*/)
    //        {
    //            var parameterString = parameter as string;

    //            if (!string.IsNullOrEmpty(parameterString))
    //            {
    //                string[] parameters = parameterString.Split(new char[] { '|' });

    //                if (int.TryParse(parameters[0], out _) && int.TryParse(parameters[1], out int max))
    //                {
    //                    var _currentTimeNEW = timeInterval.CurrentTime * max / timeInterval.TimeSpan.TotalSeconds;
    //                    return (int)_currentTimeNEW;
    //                }
    //            }

    //            //var _currentTimeNEW = value * Span.TotalSeconds / SliderTimeMaximum;

    //            //StringTime = ToCurrentTime(_currentTimeNEW);

    //            //_advancedTimer.SetElapsedTime(_currentTimeNEW);

    //            //if (_advancedTimer.IsRunning() == false)
    //            //{
    //            //    OnUpdate?.Invoke(_currentTimeNEW);
    //            //}

    //            //this.Update(ref _currentTime, _currentTimeNEW);







    //            //  var dt0 = timePresenter.BeginTime;// DateTime.FromOADate(JulianDateBegin - 2415018.5);

    //            //  var dt = dt0.AddSeconds(timePresenter.CurrentTime);

    //            return 500;// dt.ToLongDateString() + " " + dt.ToLongTimeString();
    //        }

    //        return default;
    //    }

    //    /// <summary>
    //    /// Converts a value.
    //    /// </summary>
    //    /// <param name="value">The value to convert.</param>
    //    /// <param name="targetType">The type of the target.</param>
    //    /// <param name="parameter">A user-defined parameter.</param>
    //    /// <param name="culture">The culture to use.</param>
    //    /// <returns>The converted value.</returns>
    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        throw new NotImplementedException();

    //        //if (value != null && value is ITimePresenter timePresenter)
    //        //{
    //        //var _currentTimeNEW = value * Span.TotalSeconds / SliderTimeMaximum;

    //        //StringTime = ToCurrentTime(_currentTimeNEW);

    //        //_advancedTimer.SetElapsedTime(_currentTimeNEW);

    //        //if (_advancedTimer.IsRunning() == false)
    //        //{
    //        //    OnUpdate?.Invoke(_currentTimeNEW);
    //        //}

    //        //this.Update(ref _currentTime, _currentTimeNEW);

    //        //return 22;
    //        //}
    //        //return 0;
    //    }
    //}
}
