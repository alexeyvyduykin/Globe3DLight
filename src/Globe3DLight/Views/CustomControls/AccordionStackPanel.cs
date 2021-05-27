using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.VisualTree;

namespace Globe3DLight.Views.CustomControls
{
    public class AccordionStackPanel : StackPanel
    {
        static AccordionStackPanel()
        {
            AffectsParentMeasure<AccordionStackPanel>(IsExpandedProperty);
        }

        protected override Size MeasureOverride(Size constraint)
        {
            var children = Children;

            double accumulatedHeight = 0;
            Control? expandedElement = null;

            for (int i = 0, count = children.Count; i < count; ++i)
            {
                Control child = (Control)children[i];

                var childConstraint = new Size(constraint.Width, constraint.Height);

                child.Measure(childConstraint);

                bool isExpanded = GetIsExpanded(child);
                if (isExpanded == true)
                {
                    expandedElement = child;
                }
                else
                {
                    accumulatedHeight += child.DesiredSize.Height;
                }
            }

            if (expandedElement is not null)
            {
                double space = constraint.Height - accumulatedHeight;
                space = Math.Max(0, space);

                expandedElement.Measure(new Size(constraint.Width, space));

                accumulatedHeight += expandedElement.DesiredSize.Height;
            }

            return new Size(constraint.Width, accumulatedHeight);
        }


        public static readonly AttachedProperty<bool> IsExpandedProperty =
            AvaloniaProperty.RegisterAttached<AccordionStackPanel, Control, bool>("IsExpanded", default, false, coerce: Coerce);

        public static bool GetIsExpanded(Control control)
        {
            return control.GetValue(IsExpandedProperty);
        }

        public static void SetIsExpanded(Control control, bool value)
        {
            control.SetValue(IsExpandedProperty, value);
        }

        private static bool Coerce(IAvaloniaObject obj, bool value)
        {
            Control? target = obj as Control;
            if (target is not null)
            {
                bool newValue = value;
                if (newValue == true)
                {
                    var panel = GetPanel(target);
                    if (panel is not null)
                    {
                        foreach (Control child in panel.Children)
                        {
                            if (child != target)
                            {
                                SetIsExpanded(child, false);
                            }
                        }
                    }
                }
            }

            return value;
        }

        private static AccordionStackPanel? GetPanel(Control child)
        {
            var parent = child.Parent;

            if (parent is ContentPresenter presenter)
            {
                if (presenter.GetVisualParent() is AccordionStackPanel panel)
                {
                    return panel;
                }
            }
            else if (child.GetVisualParent() is AccordionStackPanel psp)
            {
                return psp;
            }

            return null;
        }
    }
}
