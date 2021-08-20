#nullable enable
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TimeDataViewer.Core
{
    /// <remarks>The category axis is using the index of the label collection items as coordinates.
    /// If you have 5 categories in the Labels collection, the categories will be placed at coordinates 0 to 4.
    /// The range of the axis will be from -0.5 to 4.5 (excluding padding).</remarks>
    public class CategoryAxis : LinearAxis
    {
        private readonly List<string> _labels = new();
        private readonly List<string> _itemsSourceLabels = new();

        // The current offset of the bars (not used for stacked bar series).
        // <remarks>These offsets are modified during rendering.</remarks>
        private double[] _currentBarOffset;

        /// <summary>
        /// The current max value per StackIndex and Label.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] _currentMaxValue;

        /// <summary>
        /// The current min value per StackIndex and Label.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] _currentMinValue;

        /// <summary>
        /// The base value per StackIndex and Label for positive values of stacked bar series.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] _currentPositiveBaseValues;

        /// <summary>
        /// The base value per StackIndex and Label for negative values of stacked bar series.
        /// </summary>
        /// <remarks>These values are modified during rendering.</remarks>
        private double[,] _currentNegativeBaseValues;

        /// <summary>
        /// The maximum stack index.
        /// </summary>
        private int _maxStackIndex;

        /// <summary>
        /// The maximal width of all labels.
        /// </summary>
        private double _maxWidth;

        public CategoryAxis()
        {
            Position = AxisPosition.Bottom;
            MajorStep = 1;
            GapWidth = 1;
        }

        /// <summary>
        /// Gets or sets the gap width.
        /// </summary>
        /// <remarks>The default value is 1.0 (100%). The gap width is given as a fraction of the total width/height of the items in a category.</remarks>
        public double GapWidth { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ticks are centered. If this is <c>false</c>, ticks will be drawn between each category. If this is <c>true</c>, ticks will be drawn in the middle of each category.
        /// </summary>
        public bool IsTickCentered { get; set; }

        /// <summary>
        /// Gets or sets the items source (used to update the Labels collection).
        /// </summary>
        /// <value>The items source.</value>
        public IEnumerable ItemsSource { get; set; }

        /// <summary>
        /// Gets or sets the data field for the labels.
        /// </summary>
        public string LabelField { get; set; }

        public List<string> Labels => _labels;

        public List<string> ActualLabels
        {
            get
            {
                return ItemsSource != null ? _itemsSourceLabels : _labels;
            }
        }

        /// <summary>
        /// Gets or sets the original offset of the bars (not used for stacked bar series).
        /// </summary>
        private double[] BarOffset { get; set; }

        /// <summary>
        /// Gets or sets the stack index mapping. The mapping indicates to which rank a specific stack index belongs.
        /// </summary>
        private Dictionary<string, int> StackIndexMapping { get; set; }

        /// <summary>
        /// Gets or sets the offset of the bars per StackIndex and Label (only used for stacked bar series).
        /// </summary>
        private double[,] StackedBarOffset { get; set; }

        /// <summary>
        /// Gets or sets sum of the widths of the single bars per label. This is used to find the bar width of BarSeries
        /// </summary>
        private double[] TotalWidthPerCategory { get; set; }

        /// <summary>
        /// Gets the maximum width of all category labels.
        /// </summary>
        /// <returns>The maximum width.</returns>
        public double GetMaxWidth()
        {
            return _maxWidth;
        }

        /// <summary>
        /// Gets the category value.
        /// </summary>
        /// <param name="categoryIndex">Index of the category.</param>
        /// <param name="stackIndex">Index of the stack.</param>
        /// <param name="actualBarWidth">Actual width of the bar.</param>
        /// <returns>The get category value.</returns>
        public double GetCategoryValue(int categoryIndex, int stackIndex, double actualBarWidth)
        {
            var offsetBegin = StackedBarOffset[stackIndex, categoryIndex];
            var offsetEnd = StackedBarOffset[stackIndex + 1, categoryIndex];
            return categoryIndex - 0.5 + ((offsetEnd + offsetBegin - actualBarWidth) * 0.5);
        }

        /// <summary>
        /// Gets the category value.
        /// </summary>
        /// <param name="categoryIndex">Index of the category.</param>
        /// <returns>The get category value.</returns>
        public double GetCategoryValue(int categoryIndex)
        {
            return categoryIndex - 0.5 + BarOffset[categoryIndex];
        }

        /// <summary>
        /// Gets the coordinates used to draw ticks and tick labels (numbers or category names).
        /// </summary>
        /// <param name="majorLabelValues">The major label values.</param>
        /// <param name="majorTickValues">The major tick values.</param>
        /// <param name="minorTickValues">The minor tick values.</param>
        public override void GetTickValues(
            out IList<double> majorLabelValues, out IList<double> majorTickValues, out IList<double> minorTickValues)
        {
            base.GetTickValues(out majorLabelValues, out majorTickValues, out minorTickValues);
            minorTickValues.Clear();

            if (!IsTickCentered)
            {
                // Subtract 0.5 from the label values to get the tick values.
                // Add one extra tick at the end.
                var mv = new List<double>(majorLabelValues.Count);
                mv.AddRange(majorLabelValues.Select(v => v - 0.5));
                if (mv.Count > 0)
                {
                    mv.Add(mv[mv.Count - 1] + 1);
                }

                majorTickValues = mv;
            }
        }

        /// <summary>
        /// Gets the value from an axis coordinate, converts from double to the correct data type if necessary. e.g. DateTimeAxis returns the DateTime and CategoryAxis returns category strings.
        /// </summary>
        /// <param name="x">The coordinate.</param>
        /// <returns>The value.</returns>
        public override object GetValue(double x)
        {
            return FormatValue(x);
        }

        /// <summary>
        /// Gets the current bar offset for the specified category index.
        /// </summary>
        /// <param name="categoryIndex">The category index.</param>
        /// <returns>The offset.</returns>
        public double GetCurrentBarOffset(int categoryIndex)
        {
            return _currentBarOffset[categoryIndex];
        }

        /// <summary>
        /// Increases the current bar offset for the specified category index.
        /// </summary>
        /// <param name="categoryIndex">The category index.</param>
        /// <param name="delta">The offset increase.</param>
        public void IncreaseCurrentBarOffset(int categoryIndex, double delta)
        {
            _currentBarOffset[categoryIndex] += delta;
        }

        /// <summary>
        /// Gets the current base value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <param name="negativeValue">if set to <c>true</c> get the base value for negative values.</param>
        /// <returns>The current base value.</returns>
        public double GetCurrentBaseValue(int stackIndex, int categoryIndex, bool negativeValue)
        {
            return negativeValue ? _currentNegativeBaseValues[stackIndex, categoryIndex] : _currentPositiveBaseValues[stackIndex, categoryIndex];
        }

        /// <summary>
        /// Sets the current base value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">Index of the stack.</param>
        /// <param name="categoryIndex">Index of the category.</param>
        /// <param name="negativeValue">if set to <c>true</c> set the base value for negative values.</param>
        /// <param name="newValue">The new value.</param>
        public void SetCurrentBaseValue(int stackIndex, int categoryIndex, bool negativeValue, double newValue)
        {
            if (negativeValue)
            {
                _currentNegativeBaseValues[stackIndex, categoryIndex] = newValue;
            }
            else
            {
                _currentPositiveBaseValues[stackIndex, categoryIndex] = newValue;
            }
        }

        /// <summary>
        /// Gets the current maximum value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <returns>The current value.</returns>
        public double GetCurrentMaxValue(int stackIndex, int categoryIndex)
        {
            return _currentMaxValue[stackIndex, categoryIndex];
        }

        /// <summary>
        /// Sets the current maximum value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <param name="newValue">The new value.</param>
        public void SetCurrentMaxValue(int stackIndex, int categoryIndex, double newValue)
        {
            _currentMaxValue[stackIndex, categoryIndex] = newValue;
        }

        /// <summary>
        /// Gets the current minimum value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <returns>The current value.</returns>
        public double GetCurrentMinValue(int stackIndex, int categoryIndex)
        {
            return _currentMinValue[stackIndex, categoryIndex];
        }

        /// <summary>
        /// Sets the current minimum value for the specified stack and category index.
        /// </summary>
        /// <param name="stackIndex">The stack index.</param>
        /// <param name="categoryIndex">The category index.</param>
        /// <param name="newValue">The new value.</param>
        public void SetCurrentMinValue(int stackIndex, int categoryIndex, double newValue)
        {
            _currentMinValue[stackIndex, categoryIndex] = newValue;
        }

        /// <summary>
        /// Gets the stack index for the specified stack group.
        /// </summary>
        /// <param name="stackGroup">The stack group.</param>
        /// <returns>The stack index.</returns>
        public int GetStackIndex(string stackGroup)
        {
            return StackIndexMapping[stackGroup];
        }

        /// <summary>
        /// Updates the actual maximum and minimum values. If the user has zoomed/panned the axis, the internal ViewMaximum/ViewMinimum values will be used. If Maximum or Minimum have been set, these values will be used. Otherwise the maximum and minimum values of the series will be used, including the 'padding'.
        /// </summary>
        internal override void UpdateActualMaxMin()
        {
            // Update the DataMinimum/DataMaximum from the number of categories
            Include(-0.5);

            var actualLabels = ActualLabels;

            if (actualLabels.Count > 0)
            {
                Include((actualLabels.Count - 1) + 0.5);
            }
            else
            {
                Include(0.5);
            }

            base.UpdateActualMaxMin();

            MinorStep = 1;
        }

        /// <summary>
        /// Updates the axis with information from the plot series.
        /// </summary>
        /// <param name="series">The series collection.</param>
        /// <remarks>This is used by the category axis that need to know the number of series using the axis.</remarks>
        internal override void UpdateFromSeries(Series[] series)
        {
            base.UpdateFromSeries(series);

            UpdateLabels(series);

            var actualLabels = ActualLabels;
            if (actualLabels.Count == 0)
            {
                TotalWidthPerCategory = null;
                _maxWidth = double.NaN;
                BarOffset = null;
                StackedBarOffset = null;
                StackIndexMapping = null;

                return;
            }

            TotalWidthPerCategory = new double[actualLabels.Count];

            var usedSeries = series.Where(s => s.IsUsing(this)).ToList();

            // Add width of stacked series
            var categorizedSeries = usedSeries.OfType<CategorizedSeries>().ToList();
            var stackedSeries = categorizedSeries.OfType<IStackableSeries>().Where(s => s.IsStacked).ToList();
            var stackIndices = stackedSeries.Select(s => s.StackGroup).Distinct().ToList();
            var stackRankBarWidth = new Dictionary<int, double>();
            for (var j = 0; j < stackIndices.Count; j++)
            {
                var maxBarWidth =
                    stackedSeries.Where(s => s.StackGroup == stackIndices[j]).Select(
                        s => ((CategorizedSeries)s).GetBarWidth()).Concat(new[] { 0.0 }).Max();
                for (var i = 0; i < actualLabels.Count; i++)
                {
                    int k = 0;
                    if (
                        stackedSeries.SelectMany(s => ((CategorizedSeries)s).GetItems()).Any(
                            item => item.GetCategoryIndex(k++) == i))
                    {
                        TotalWidthPerCategory[i] += maxBarWidth;
                    }
                }

                stackRankBarWidth[j] = maxBarWidth;
            }

            // Add width of unstacked series
            var unstackedBarSeries = categorizedSeries.Where(s => !(s is IStackableSeries) || !((IStackableSeries)s).IsStacked).ToList();
            foreach (var s in unstackedBarSeries)
            {
                for (var i = 0; i < actualLabels.Count; i++)
                {
                    int j = 0;
                    var numberOfItems = s.GetItems().Count(item => item.GetCategoryIndex(j++) == i);
                    TotalWidthPerCategory[i] += s.GetBarWidth() * numberOfItems;
                }
            }

            _maxWidth = TotalWidthPerCategory.Max();

            // Calculate BarOffset and StackedBarOffset
            BarOffset = new double[actualLabels.Count];
            StackedBarOffset = new double[stackIndices.Count + 1, actualLabels.Count];

            var factor = 0.5 / (1 + GapWidth) / _maxWidth;
            for (var i = 0; i < actualLabels.Count; i++)
            {
                BarOffset[i] = 0.5 - (TotalWidthPerCategory[i] * factor);
            }

            for (var j = 0; j <= stackIndices.Count; j++)
            {
                for (var i = 0; i < actualLabels.Count; i++)
                {
                    int k = 0;
                    if (
                        stackedSeries.SelectMany(s => ((CategorizedSeries)s).GetItems()).All(
                            item => item.GetCategoryIndex(k++) != i))
                    {
                        continue;
                    }

                    StackedBarOffset[j, i] = BarOffset[i];
                    if (j < stackIndices.Count)
                    {
                        BarOffset[i] += stackRankBarWidth[j] / (1 + GapWidth) / _maxWidth;
                    }
                }
            }

            stackIndices.Sort();
            StackIndexMapping = new Dictionary<string, int>();
            for (var i = 0; i < stackIndices.Count; i++)
            {
                StackIndexMapping.Add(stackIndices[i], i);
            }

            _maxStackIndex = stackIndices.Count;
        }

        /// <summary>
        /// Resets the current values.
        /// </summary>
        /// <remarks>The current values may be modified during update of max/min and rendering.</remarks>
        protected internal override void ResetCurrentValues()
        {
            base.ResetCurrentValues();
            _currentBarOffset = BarOffset != null ? BarOffset.ToArray() : null;
            var actualLabels = ActualLabels;
            if (_maxStackIndex > 0)
            {
                _currentPositiveBaseValues = new double[_maxStackIndex, actualLabels.Count];
                _currentPositiveBaseValues.Fill2D(double.NaN);
                _currentNegativeBaseValues = new double[_maxStackIndex, actualLabels.Count];
                _currentNegativeBaseValues.Fill2D(double.NaN);

                _currentMaxValue = new double[_maxStackIndex, actualLabels.Count];
                _currentMaxValue.Fill2D(double.NaN);
                _currentMinValue = new double[_maxStackIndex, actualLabels.Count];
                _currentMinValue.Fill2D(double.NaN);
            }
            else
            {
                _currentPositiveBaseValues = null;
                _currentNegativeBaseValues = null;
                _currentMaxValue = null;
                _currentMinValue = null;
            }
        }

        /// <summary>
        /// Formats the value to be used on the axis.
        /// </summary>
        /// <param name="x">The value to format.</param>
        /// <returns>The formatted value.</returns>
        protected override string FormatValueOverride(double x)
        {
            var index = (int)x;
            var actualLabels = ActualLabels;
            if (index >= 0 && index < actualLabels.Count)
            {
                return actualLabels[index];
            }

            return null;
        }

        /// <summary>
        /// Creates Labels list if no labels were set
        /// </summary>
        /// <param name="series">The list of series which are rendered</param>
        private void UpdateLabels(IEnumerable<Series> series)
        {
            if (ItemsSource != null)
            {
                _itemsSourceLabels.Clear();
                _itemsSourceLabels.AddRange(ItemsSource.Format(LabelField, StringFormat, CultureInfo.CurrentCulture));
                return;
            }

            if (Labels.Count == 0)
            {
                // auto-create labels
                // TODO: should not modify Labels collection
                foreach (var s in series)
                {
                    if (!s.IsUsing(this))
                    {
                        continue;
                    }

                    if (s is CategorizedSeries bsb)
                    {
                        int max = bsb.GetItems().Count;
                        while (Labels.Count < max)
                        {
                            Labels.Add((Labels.Count + 1).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }
            }
        }
    }
}
