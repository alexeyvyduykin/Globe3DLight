using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Templates;
using System.Globalization;
using System;
using System.IO;
using System.Linq;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Threading;
using Avalonia.Utilities;
using Avalonia.Controls.Metadata;


namespace Globe3DLight.Controls
{

    public class NumberField1ValueChangedEventArgs : RoutedEventArgs
    {
        public NumberField1ValueChangedEventArgs(RoutedEvent routedEvent, double oldValue, double newValue) : base(routedEvent)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public double OldValue { get; }
        public double NewValue { get; }
    }

    public class NumberField : TemplatedControl
    {
        public static readonly DirectProperty<NumberField, bool> ClipValueToMinMaxProperty =
            AvaloniaProperty.RegisterDirect<NumberField, bool>(nameof(ClipValueToMinMax),
                updown => updown.ClipValueToMinMax, (updown, b) => updown.ClipValueToMinMax = b);

        public static readonly DirectProperty<NumberField, CultureInfo> CultureInfoProperty =
            AvaloniaProperty.RegisterDirect<NumberField, CultureInfo>(nameof(CultureInfo), o => o.CultureInfo,
                (o, v) => o.CultureInfo = v, CultureInfo.CurrentCulture);

        public static readonly StyledProperty<string> FormatStringProperty =
            AvaloniaProperty.Register<NumberField, string>(nameof(FormatString), string.Empty);

        public static readonly StyledProperty<double> IncrementProperty =
            AvaloniaProperty.Register<NumberField, double>(nameof(Increment), 1.0d, coerce: OnCoerceIncrement);

        public static readonly StyledProperty<bool> IsReadOnlyProperty =
            AvaloniaProperty.Register<NumberField, bool>(nameof(IsReadOnly));

        public static readonly StyledProperty<double> MaximumProperty =
            AvaloniaProperty.Register<NumberField, double>(nameof(Maximum), double.MaxValue, coerce: OnCoerceMaximum);

        public static readonly StyledProperty<double> MinimumProperty =
            AvaloniaProperty.Register<NumberField, double>(nameof(Minimum), double.MinValue, coerce: OnCoerceMinimum);

        public static readonly DirectProperty<NumberField, NumberStyles> ParsingNumberStyleProperty =
            AvaloniaProperty.RegisterDirect<NumberField, NumberStyles>(nameof(ParsingNumberStyle),
                updown => updown.ParsingNumberStyle, (updown, style) => updown.ParsingNumberStyle = style);

        public static readonly DirectProperty<NumberField, string> TextProperty =
            AvaloniaProperty.RegisterDirect<NumberField, string>(nameof(Text), o => o.Text, (o, v) => o.Text = v,
                defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

        public static readonly DirectProperty<NumberField, double> ValueProperty =
            AvaloniaProperty.RegisterDirect<NumberField, double>(nameof(Value), updown => updown.Value,
                (updown, v) => updown.Value = v, defaultBindingMode: BindingMode.TwoWay, enableDataValidation: true);

        public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
            ContentControl.HorizontalContentAlignmentProperty.AddOwner<NumberField>();

        public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
            ContentControl.VerticalContentAlignmentProperty.AddOwner<NumberField>();

        private IDisposable _textBoxTextChangedSubscription;

        private double _value;
        private string _text;
        private bool _internalValueSet;
        private bool _clipValueToMinMax;
        private bool _isSyncingTextAndValueProperties;
        private bool _isTextChangedFromUI;
        private CultureInfo _cultureInfo;
        private NumberStyles _parsingNumberStyle = NumberStyles.Any;

        //-------------------------------------------------
        //private Spinner Spinner { get; set; }

        private Button DecreaseButton { get; set; }
        private Button IncreaseButton { get; set; }

        //-------------------------------------------------
        private TextBox TextBox { get; set; }

        public bool ClipValueToMinMax
        {
            get { return _clipValueToMinMax; }
            set { SetAndRaise(ClipValueToMinMaxProperty, ref _clipValueToMinMax, value); }
        }

        public CultureInfo CultureInfo
        {
            get { return _cultureInfo; }
            set { SetAndRaise(CultureInfoProperty, ref _cultureInfo, value); }
        }

        public string FormatString
        {
            get { return GetValue(FormatStringProperty); }
            set { SetValue(FormatStringProperty, value); }
        }

        public double Increment
        {
            get { return GetValue(IncrementProperty); }
            set { SetValue(IncrementProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public double Maximum
        {
            get { return GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public double Minimum
        {
            get { return GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public NumberStyles ParsingNumberStyle
        {
            get { return _parsingNumberStyle; }
            set { SetAndRaise(ParsingNumberStyleProperty, ref _parsingNumberStyle, value); }
        }

        public string Text
        {
            get { return _text; }
            set { SetAndRaise(TextProperty, ref _text, value); }
        }

        public double Value
        {
            get { return _value; }
            set
            {
                value = OnCoerceValue(value);
                SetAndRaise(ValueProperty, ref _value, value);
            }
        }

        public HorizontalAlignment HorizontalContentAlignment
        {
            get => GetValue(HorizontalContentAlignmentProperty);
            set => SetValue(HorizontalContentAlignmentProperty, value);
        }

        public VerticalAlignment VerticalContentAlignment
        {
            get => GetValue(VerticalContentAlignmentProperty);
            set => SetValue(VerticalContentAlignmentProperty, value);
        }

        public NumberField()
        {
            Initialized += (sender, e) =>
            {
                if (!_internalValueSet && IsInitialized)
                {
                    SyncTextAndValueProperties(false, null, true);
                }

                SetValidSpinDirection();
            };
        }

        static NumberField()
        {
            CultureInfoProperty.Changed.Subscribe(OnCultureInfoChanged);
            FormatStringProperty.Changed.Subscribe(FormatStringChanged);
            IncrementProperty.Changed.Subscribe(IncrementChanged);
            IsReadOnlyProperty.Changed.Subscribe(OnIsReadOnlyChanged);
            MaximumProperty.Changed.Subscribe(OnMaximumChanged);
            MinimumProperty.Changed.Subscribe(OnMinimumChanged);
            TextProperty.Changed.Subscribe(OnTextChanged);
            ValueProperty.Changed.Subscribe(OnValueChanged);
        }
     
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            CommitInput();
            base.OnLostFocus(e);
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            if (TextBox != null)
            {
                TextBox.PointerPressed -= TextBoxOnPointerPressed;
                _textBoxTextChangedSubscription?.Dispose();
            }
            TextBox = e.NameScope.Find<TextBox>("PART_TextBox");
            if (TextBox != null)
            {
                TextBox.Text = Text;
                TextBox.PointerPressed += TextBoxOnPointerPressed;
                _textBoxTextChangedSubscription = TextBox.GetObservable(TextBox.TextProperty).Subscribe(txt => TextBoxOnTextChanged());
            }

            //-------------------------------------------------------------------

            if (DecreaseButton != null)
            {
                DecreaseButton.Click -= OnDecreaseButtonClick;
            }
            DecreaseButton = e.NameScope.Find<Button>("PART_DecreaseButton");
            if (DecreaseButton != null)
            {
                DecreaseButton.Click += OnDecreaseButtonClick;
            }

            if (IncreaseButton != null)
            {
                IncreaseButton.Click -= IncreaseButtonClick;
            }
            IncreaseButton = e.NameScope.Find<Button>("PART_IncreaseButton");
            if (IncreaseButton != null)
            {
                IncreaseButton.Click += IncreaseButtonClick;
            }

            //SetButtonUsage();

            //if (Spinner != null)
            //{
            //    Spinner.Spin -= OnSpinnerSpin;
            //}

            //Spinner = e.NameScope.Find<Spinner>("PART_Spinner");

            //if (Spinner != null)
            //{
            //    Spinner.Spin += OnSpinnerSpin;
            //}
            //--------------------------------------------------------------------

            SetValidSpinDirection();
        }

        //private void SetButtonUsage()
        //{
        //    if (IncreaseButton != null)
        //    {
        //        IncreaseButton.IsEnabled = (ValidSpinDirection & ValidSpinDirections.Increase) == ValidSpinDirections.Increase;
        //    }

        //    if (DecreaseButton != null)
        //    {
        //        DecreaseButton.IsEnabled = (ValidSpinDirection & ValidSpinDirections.Decrease) == ValidSpinDirections.Decrease;
        //    }
        //}

        private void OnDecreaseButtonClick(object sender, RoutedEventArgs e)
        {
            OnSpin(new SpinEventArgs(SpinDirection.Decrease));
        }

        private void IncreaseButtonClick(object sender, RoutedEventArgs e)
        {          
            OnSpin(new SpinEventArgs(SpinDirection.Increase));
        }

        //private void OnButtonClick(object sender, RoutedEventArgs e)
        //{
        //    var direction = sender == IncreaseButton ? SpinDirection.Increase : SpinDirection.Decrease;
        //    OnSpin(new SpinEventArgs(/*SpinEvent,*/ direction));
        //}

        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    var commitSuccess = CommitInput();
                    e.Handled = !commitSuccess;
                    break;
            }
        }

        protected override void UpdateDataValidation<T>(AvaloniaProperty<T> property, BindingValue<T> value)
        {
            if (property == TextProperty || property == ValueProperty)
            {
                DataValidationErrors.SetError(this, value.Error);
            }
        }

        protected virtual void OnCultureInfoChanged(CultureInfo oldValue, CultureInfo newValue)
        {
            if (IsInitialized)
            {
                SyncTextAndValueProperties(false, null);
            }
        }

        protected virtual void OnFormatStringChanged(string oldValue, string newValue)
        {
            if (IsInitialized)
            {
                SyncTextAndValueProperties(false, null);
            }
        }

        protected virtual void OnIncrementChanged(double oldValue, double newValue)
        {
            if (IsInitialized)
            {
                SetValidSpinDirection();
            }
        }

        protected virtual void OnIsReadOnlyChanged(bool oldValue, bool newValue)
        {
            SetValidSpinDirection();
        }

        protected virtual void OnMaximumChanged(double oldValue, double newValue)
        {
            if (IsInitialized)
            {
                SetValidSpinDirection();
            }
            if (ClipValueToMinMax)
            {
                Value = MathUtilities.Clamp(Value, Minimum, Maximum);
            }
        }

        protected virtual void OnMinimumChanged(double oldValue, double newValue)
        {
            if (IsInitialized)
            {
                SetValidSpinDirection();
            }
            if (ClipValueToMinMax)
            {
                Value = MathUtilities.Clamp(Value, Minimum, Maximum);
            }
        }

        protected virtual void OnTextChanged(string oldValue, string newValue)
        {
            if (IsInitialized)
            {
                SyncTextAndValueProperties(true, Text);
            }
        }

        protected virtual void OnValueChanged(double oldValue, double newValue)
        {
            if (!_internalValueSet && IsInitialized)
            {
                SyncTextAndValueProperties(false, null, true);
            }

            SetValidSpinDirection();

            RaiseValueChangedEvent(oldValue, newValue);
        }

        protected virtual double OnCoerceIncrement(double baseValue)
        {
            return baseValue;
        }

        protected virtual double OnCoerceMaximum(double baseValue)
        {
            return Math.Max(baseValue, Minimum);
        }

        protected virtual double OnCoerceMinimum(double baseValue)
        {
            return Math.Min(baseValue, Maximum);
        }

        protected virtual double OnCoerceValue(double baseValue)
        {
            return baseValue;
        }

        protected virtual void OnSpin(SpinEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            var handler = Spinned;
            handler?.Invoke(this, e);

            if (e.Direction == SpinDirection.Increase)
            {
                OnIncrement();
            }
            else
            {
                OnDecrement();
            }
        }

        protected virtual void RaiseValueChangedEvent(double oldValue, double newValue)
        {
            var e = new NumberField1ValueChangedEventArgs(ValueChangedEvent, oldValue, newValue);
            RaiseEvent(e);
        }

        private double ConvertTextToValue(string text)
        {
            double result = 0;

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            // Since the conversion from Value to text using a FormatString may not be parsable,
            // we verify that the already existing text is not the exact same value.
            var currentValueText = ConvertValueToText();
            if (Equals(currentValueText, text))
            {
                return Value;
            }

            result = ConvertTextToValueCore(currentValueText, text);

            if (ClipValueToMinMax)
            {
                return MathUtilities.Clamp(result, Minimum, Maximum);
            }

            ValidateMinMax(result);

            return result;
        }

        private string ConvertValueToText()
        {
            //Manage FormatString of type "{}{0:N2} °" (in xaml) or "{0:N2} °" in code-behind.
            if (FormatString.Contains("{0"))
            {
                return string.Format(CultureInfo, FormatString, Value);
            }

            return Value.ToString(FormatString, CultureInfo);
        }

        private void OnIncrement()
        {
            var result = Value + Increment;
            Value = MathUtilities.Clamp(result, Minimum, Maximum);
        }

        private void OnDecrement()
        {
            var result = Value - Increment;
            Value = MathUtilities.Clamp(result, Minimum, Maximum);
        }

        private void SetValidSpinDirection()
        {
            var validDirections = ValidSpinDirections.None;

            // Zero increment always prevents spin.
            if (Increment != 0 && IsReadOnly == false)
            {
                if (Value < Maximum)
                {
                    validDirections = validDirections | ValidSpinDirections.Increase;
                }

                if (Value > Minimum)
                {
                    validDirections = validDirections | ValidSpinDirections.Decrease;
                }
            }

            //if (Spinner != null)
            //{
            //    Spinner.ValidSpinDirection = validDirections;
            //}
        }

        private static void OnCultureInfoChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is NumberField upDown)
            {
                var oldValue = (CultureInfo)e.OldValue;
                var newValue = (CultureInfo)e.NewValue;
                upDown.OnCultureInfoChanged(oldValue, newValue);
            }
        }

        private static void IncrementChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is NumberField upDown)
            {
                var oldValue = (double)e.OldValue;
                var newValue = (double)e.NewValue;
                upDown.OnIncrementChanged(oldValue, newValue);
            }
        }

        private static void FormatStringChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is NumberField upDown)
            {
                var oldValue = (string)e.OldValue;
                var newValue = (string)e.NewValue;
                upDown.OnFormatStringChanged(oldValue, newValue);
            }
        }

        private static void OnIsReadOnlyChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is NumberField upDown)
            {
                var oldValue = (bool)e.OldValue;
                var newValue = (bool)e.NewValue;
                upDown.OnIsReadOnlyChanged(oldValue, newValue);
            }
        }

        private static void OnMaximumChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is NumberField upDown)
            {
                var oldValue = (double)e.OldValue;
                var newValue = (double)e.NewValue;
                upDown.OnMaximumChanged(oldValue, newValue);
            }
        }

        private static void OnMinimumChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is NumberField upDown)
            {
                var oldValue = (double)e.OldValue;
                var newValue = (double)e.NewValue;
                upDown.OnMinimumChanged(oldValue, newValue);
            }
        }

        private static void OnTextChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is NumberField upDown)
            {
                var oldValue = (string)e.OldValue;
                var newValue = (string)e.NewValue;
                upDown.OnTextChanged(oldValue, newValue);
            }
        }

        private static void OnValueChanged(AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Sender is NumberField upDown)
            {
                var oldValue = (double)e.OldValue;
                var newValue = (double)e.NewValue;
                upDown.OnValueChanged(oldValue, newValue);
            }
        }

        private void SetValueInternal(double value)
        {
            _internalValueSet = true;
            try
            {
                Value = value;
            }
            finally
            {
                _internalValueSet = false;
            }
        }

        private static double OnCoerceMaximum(IAvaloniaObject instance, double value)
        {
            if (instance is NumberField upDown)
            {
                return upDown.OnCoerceMaximum(value);
            }

            return value;
        }

        private static double OnCoerceMinimum(IAvaloniaObject instance, double value)
        {
            if (instance is NumberField upDown)
            {
                return upDown.OnCoerceMinimum(value);
            }

            return value;
        }

        private static double OnCoerceIncrement(IAvaloniaObject instance, double value)
        {
            if (instance is NumberField upDown)
            {
                return upDown.OnCoerceIncrement(value);
            }

            return value;
        }

        private void TextBoxOnTextChanged()
        {
            try
            {
                _isTextChangedFromUI = true;
                if (TextBox != null)
                {
                    Text = TextBox.Text;
                }
            }
            finally
            {
                _isTextChangedFromUI = false;
            }
        }

        private void OnSpinnerSpin(object sender, SpinEventArgs e)
        {
            if (IsReadOnly == false)
            {
                var spin = !e.UsingMouseWheel;
                spin |= ((TextBox != null) && TextBox.IsFocused);

                if (spin)
                {
                    e.Handled = true;
                    OnSpin(e);
                }
            }
        }

        public event EventHandler<SpinEventArgs> Spinned;

        private void TextBoxOnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            //if (e.Pointer.Captured != Spinner)
            {

                //Dispatcher.UIThread.InvokeAsync(() => { e.Pointer.Capture(Spinner); }, DispatcherPriority.Input);
            }
        }

        public static readonly RoutedEvent<NumberField1ValueChangedEventArgs> ValueChangedEvent =
            RoutedEvent.Register<NumberField, NumberField1ValueChangedEventArgs>(nameof(ValueChanged), RoutingStrategies.Bubble);

        public event EventHandler<NumberField1ValueChangedEventArgs> ValueChanged
        {
            add { AddHandler(ValueChangedEvent, value); }
            remove { RemoveHandler(ValueChangedEvent, value); }
        }

        private bool CommitInput()
        {
            return SyncTextAndValueProperties(true, Text);
        }

        private bool SyncTextAndValueProperties(bool updateValueFromText, string text)
        {
            return SyncTextAndValueProperties(updateValueFromText, text, false);
        }

        private bool SyncTextAndValueProperties(bool updateValueFromText, string text, bool forceTextUpdate)
        {
            if (_isSyncingTextAndValueProperties)
                return true;

            _isSyncingTextAndValueProperties = true;
            var parsedTextIsValid = true;
            try
            {
                if (updateValueFromText)
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        try
                        {
                            var newValue = ConvertTextToValue(text);
                            if (!Equals(newValue, Value))
                            {
                                SetValueInternal(newValue);
                            }
                        }
                        catch
                        {
                            parsedTextIsValid = false;
                        }
                    }
                }

                // Do not touch the ongoing text input from user.
                if (!_isTextChangedFromUI)
                {
                    var keepEmpty = !forceTextUpdate && string.IsNullOrEmpty(Text);
                    if (!keepEmpty)
                    {
                        var newText = ConvertValueToText();
                        if (!Equals(Text, newText))
                        {
                            Text = newText;
                        }
                    }

                    // Sync Text and textBox
                    if (TextBox != null)
                    {
                        TextBox.Text = Text;
                    }
                }

                if (_isTextChangedFromUI && parsedTextIsValid == false)
                {
                    // Text input was made from the user and the text
                    // represents an invalid value. Disable the spinner in this case.
                    //if (Spinner != null)
                    //{
                    //    Spinner.ValidSpinDirection = ValidSpinDirections.None;
                    //}
                }
                else
                {
                    SetValidSpinDirection();
                }
            }
            finally
            {
                _isSyncingTextAndValueProperties = false;
            }
            return parsedTextIsValid;
        }

        private double ConvertTextToValueCore(string currentValueText, string text)
        {
            double result;

            if (IsPercent(FormatString))
            {
                result = decimal.ToDouble(ParsePercent(text, CultureInfo));
            }
            else
            {
                // Problem while converting new text
                if (!double.TryParse(text, ParsingNumberStyle, CultureInfo, out var outputValue))
                {
                    var shouldThrow = true;

                    // Check if CurrentValueText is also failing => it also contains special characters. ex : 90°
                    if (!double.TryParse(currentValueText, ParsingNumberStyle, CultureInfo, out var _))
                    {
                        // extract non-digit characters
                        var currentValueTextSpecialCharacters = currentValueText.Where(c => !char.IsDigit(c));
                        var textSpecialCharacters = text.Where(c => !char.IsDigit(c));
                        // same non-digit characters on currentValueText and new text => remove them on new Text to parse it again.
                        if (currentValueTextSpecialCharacters.Except(textSpecialCharacters).ToList().Count == 0)
                        {
                            foreach (var character in textSpecialCharacters)
                            {
                                text = text.Replace(character.ToString(), string.Empty);
                            }
                            // if without the special characters, parsing is good, do not throw
                            if (double.TryParse(text, ParsingNumberStyle, CultureInfo, out outputValue))
                            {
                                shouldThrow = false;
                            }
                        }
                    }

                    if (shouldThrow)
                    {
                        throw new InvalidDataException("Input string was not in a correct format.");
                    }
                }
                result = outputValue;
            }
            return result;
        }

        private void ValidateMinMax(double value)
        {
            if (value < Minimum)
            {
                throw new ArgumentOutOfRangeException(nameof(value), string.Format("Value must be greater than Minimum value of {0}", Minimum));
            }
            else if (value > Maximum)
            {
                throw new ArgumentOutOfRangeException(nameof(value), string.Format("Value must be less than Maximum value of {0}", Maximum));
            }
        }

        private static decimal ParsePercent(string text, IFormatProvider cultureInfo)
        {
            var info = NumberFormatInfo.GetInstance(cultureInfo);
            text = text.Replace(info.PercentSymbol, null);
            var result = decimal.Parse(text, NumberStyles.Any, info);
            result = result / 100;
            return result;
        }


        private bool IsPercent(string stringToTest)
        {
            var PIndex = stringToTest.IndexOf("P", StringComparison.Ordinal);
            if (PIndex >= 0)
            {
                //stringToTest contains a "P" between 2 "'", it's considered as text, not percent
                var isText = stringToTest.Substring(0, PIndex).Contains("'")
                             && stringToTest.Substring(PIndex, FormatString.Length - PIndex).Contains("'");

                return !isText;
            }
            return false;
        }
    }


    public class ButtonSpinner11 : Spinner
    {

        //protected override void OnPointerReleased(PointerReleasedEventArgs e)
        //{
        //    base.OnPointerReleased(e);
        //    Point mousePosition;
        //    if (IncreaseButton != null && IncreaseButton.IsEnabled == false)
        //    {
        //        mousePosition = e.GetPosition(IncreaseButton);
        //        if (mousePosition.X > 0 && mousePosition.X < IncreaseButton.Width &&
        //            mousePosition.Y > 0 && mousePosition.Y < IncreaseButton.Height)
        //        {
        //            e.Handled = true;
        //        }
        //    }

        //    if (DecreaseButton != null && DecreaseButton.IsEnabled == false)
        //    {
        //        mousePosition = e.GetPosition(DecreaseButton);
        //        if (mousePosition.X > 0 && mousePosition.X < DecreaseButton.Width &&
        //            mousePosition.Y > 0 && mousePosition.Y < DecreaseButton.Height)
        //        {
        //            e.Handled = true;
        //        }
        //    }
        //}
    }
}
