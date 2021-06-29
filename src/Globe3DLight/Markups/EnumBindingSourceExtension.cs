using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System.Resources;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia;
using System.IO;

namespace Globe3DLight.Markups
{
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _enumType;
        public Type EnumType
        {
            get { return this._enumType; }
            set
            {
                if (value != this._enumType)
                {
                    if (null != value)
                    {
                        Type enumType = Nullable.GetUnderlyingType(value) ?? value;

                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    this._enumType = value;
                }
            }
        }

        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type enumType)
        {
            this.EnumType = enumType;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == this._enumType)
                throw new InvalidOperationException("The EnumType must be specified.");

            Type actualEnumType = Nullable.GetUnderlyingType(this._enumType) ?? this._enumType;
            Array enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == this._enumType)
                return enumValues;

            Array tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }



    public class CustomBinding : IBinding
    {
        public InstancedBinding Initiate(IAvaloniaObject target, AvaloniaProperty targetProperty, object anchor = null, bool enableDataValidation = false)
        {
            throw new NotImplementedException();
        }
    }


    public class ColorBindingSourceExtension : MarkupExtension
    {
        public Color Color { get; set; }

        public ColorBindingSourceExtension() { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {           
            return Colors.AliceBlue;
        }
    }

    //[MarkupExtensionReturnType(typeof(object))]
    //public class DelayBindingExtension : MarkupExtension
    //{
    //    public DelayBindingExtension()
    //    {
    //        Delay = TimeSpan.FromSeconds(0.5);
    //    }

    //    public DelayBindingExtension(PropertyPath path)
    //        : this()
    //    {
    //        Path = path;
    //    }

    //    public IValueConverter Converter { get; set; }
    //    public object ConverterParamter { get; set; }
    //    public string ElementName { get; set; }
    //    public RelativeSource RelativeSource { get; set; }
    //    public object Source { get; set; }
    //    public bool ValidatesOnDataErrors { get; set; }
    //    public bool ValidatesOnExceptions { get; set; }
    //    public TimeSpan Delay { get; set; }
    //    [ConstructorArgument("path")]
    //    public PropertyPath Path { get; set; }
    //    [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
    //    public CultureInfo ConverterCulture { get; set; }

    //    public override object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        var valueProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
    //        if (valueProvider != null)
    //        {
    //            var bindingTarget = valueProvider.TargetObject as DependencyObject;
    //            var bindingProperty = valueProvider.TargetProperty as DependencyProperty;
    //            if (bindingProperty == null || bindingTarget == null)
    //            {
    //                throw new NotSupportedException(string.Format(
    //                    "The property '{0}' on target '{1}' is not valid for a DelayBinding. The DelayBinding target must be a DependencyObject, "
    //                    + "and the target property must be a DependencyProperty.",
    //                    valueProvider.TargetProperty,
    //                    valueProvider.TargetObject));
    //            }

    //            var binding = new Binding();
    //            binding.Path = Path;
    //            binding.Converter = Converter;
    //            binding.ConverterCulture = ConverterCulture;
    //            binding.ConverterParameter = ConverterParamter;
    //            if (ElementName != null) binding.ElementName = ElementName;
    //            if (RelativeSource != null) binding.RelativeSource = RelativeSource;
    //            if (Source != null) binding.Source = Source;
    //            binding.ValidatesOnDataErrors = ValidatesOnDataErrors;
    //            binding.ValidatesOnExceptions = ValidatesOnExceptions;

    //            return DelayBinding.SetBinding(bindingTarget, bindingProperty, Delay, binding);
    //        }
    //        return null;
    //    }
    //}

    //public class DelayBinding
    //{
    //    private readonly BindingExpressionBase _bindingExpression;

    //    protected DelayBinding(BindingExpressionBase bindingExpression, DependencyObject bindingTarget, DependencyProperty bindingTargetProperty, TimeSpan delay)
    //    {
    //        _bindingExpression = bindingExpression;

    //    }
    //}
}
