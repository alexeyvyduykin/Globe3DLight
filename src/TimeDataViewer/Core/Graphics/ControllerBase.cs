using System.Collections.Generic;
using System.Linq;

namespace TimeDataViewer.Core
{
    public abstract class ControllerBase : IController
    {
        // A synchronization object that is used when the actual model in the current view is <c>null</c>.      
        private readonly object _syncRoot = new();

        protected ControllerBase()
        {
            InputCommandBindings = new List<InputCommandBinding>();
            MouseDownManipulators = new List<ManipulatorBase<OxyMouseEventArgs>>();
            MouseHoverManipulators = new List<ManipulatorBase<OxyMouseEventArgs>>();
        }

        /// <summary>
        /// Gets the input bindings.
        /// </summary>
        /// <remarks>This collection is used to specify the customized input gestures (both key, mouse and touch).</remarks>
        public List<InputCommandBinding> InputCommandBindings { get; private set; }

        /// <summary>
        /// Gets the manipulators that are created by mouse down events. These manipulators are removed when the mouse button is released.
        /// </summary>
        protected IList<ManipulatorBase<OxyMouseEventArgs>> MouseDownManipulators { get; private set; }

        /// <summary>
        /// Gets the manipulators that are created by mouse enter events. These manipulators are removed when the mouse leaves the control.
        /// </summary>
        protected IList<ManipulatorBase<OxyMouseEventArgs>> MouseHoverManipulators { get; private set; }

        /// <summary>
        /// Gets or sets the current mouse event element
        /// </summary>
        protected UIElement CurrentMouseEventElement { get; set; }

        // Handles mouse enter events.
        public virtual bool HandleMouseEnter(IView view, OxyMouseEventArgs args)
        {
            lock (GetSyncRoot(view))
            {
                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleMouseEnter(this, args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                var command = GetCommand(new OxyMouseEnterGesture());
                return HandleCommand(command, view, args);
            }
        }

        // Handles mouse leave events.
        public virtual bool HandleMouseLeave(IView view, OxyMouseEventArgs args)
        {
            lock (GetSyncRoot(view))
            {
                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleMouseLeave(this, args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                foreach (var m in MouseHoverManipulators.ToArray())
                {
                    m.Completed(args);
                    MouseHoverManipulators.Remove(m);
                }

                return true;
            }
        }

        // Returns the elements that are hit at the specified position.
        public virtual IEnumerable<HitTestResult> HitTest(IView view, HitTestArguments args)
        {
            yield break;
        }

        // Handles mouse down events.
        public virtual bool HandleMouseDown(IView view, OxyMouseDownEventArgs args)
        {
            lock (GetSyncRoot(view))
            {
                var hitargs = new HitTestArguments(args.Position, 10);
                foreach (var result in HitTest(view, hitargs))
                {
                    args.HitTestResult = result;
                    result.Element.OnMouseDown(args);
                    if (args.Handled)
                    {
                        CurrentMouseEventElement = result.Element;
                        return true;
                    }
                }

                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleMouseDown(this, args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                var command = GetCommand(new OxyMouseDownGesture(args.ChangedButton));
                return HandleCommand(command, view, args);
            }
        }

        // Handles mouse move events.
        public virtual bool HandleMouseMove(IView view, OxyMouseEventArgs args)
        {
            lock (GetSyncRoot(view))
            {
                if (CurrentMouseEventElement != null)
                {
                    CurrentMouseEventElement.OnMouseMove(args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleMouseMove(this, args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                foreach (var m in MouseDownManipulators)
                {
                    m.Delta(args);
                }

                foreach (var m in MouseHoverManipulators)
                {
                    m.Delta(args);
                }

                return true;
            }
        }

        // Handles mouse up events.
        public virtual bool HandleMouseUp(IView view, OxyMouseEventArgs args)
        {
            lock (GetSyncRoot(view))
            {
                if (CurrentMouseEventElement != null)
                {
                    CurrentMouseEventElement.OnMouseUp(args);
                    CurrentMouseEventElement = null;
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                if (view.ActualModel != null)
                {
                    view.ActualModel.HandleMouseUp(this, args);
                    if (args.Handled)
                    {
                        return true;
                    }
                }

                foreach (var m in MouseDownManipulators.ToArray())
                {
                    m.Completed(args);
                    MouseDownManipulators.Remove(m);
                }

                return true;
            }
        }

        // Handles mouse wheel events.
        public virtual bool HandleMouseWheel(IView view, OxyMouseWheelEventArgs args)
        {
            lock (GetSyncRoot(view))
            {
                var command = GetCommand(new OxyMouseWheelGesture());
                return HandleCommand(command, view, args);
            }
        }

        // Adds the specified mouse manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse down event arguments.
        public virtual void AddMouseManipulator(
            IView view,
            ManipulatorBase<OxyMouseEventArgs> manipulator,
            OxyMouseDownEventArgs args)
        {
            MouseDownManipulators.Add(manipulator);
            manipulator.Started(args);
        }

        // Adds the specified mouse hover manipulator and invokes the <see cref="MouseManipulator.Started" /> method with the specified mouse event arguments.
        public virtual void AddHoverManipulator(
            IView view,
            ManipulatorBase<OxyMouseEventArgs> manipulator,
            OxyMouseEventArgs args)
        {
            MouseHoverManipulators.Add(manipulator);
            manipulator.Started(args);
        }

        // Binds the specified command to the specified mouse gesture. Removes old bindings to the gesture.
        public virtual void Bind(OxyMouseDownGesture gesture, IViewCommand<OxyMouseDownEventArgs> command)
        {
            BindCore(gesture, command);
        }

        // Binds the specified command to the specified mouse enter gesture. Removes old bindings to the gesture.
        public virtual void Bind(OxyMouseEnterGesture gesture, IViewCommand<OxyMouseEventArgs> command)
        {
            BindCore(gesture, command);
        }

        // Binds the specified command to the specified mouse wheel gesture. Removes old bindings to the gesture.
        public virtual void Bind(OxyMouseWheelGesture gesture, IViewCommand<OxyMouseWheelEventArgs> command)
        {
            BindCore(gesture, command);
        }

        // Unbinds the specified gesture.
        public virtual void Unbind(OxyInputGesture gesture)
        {
            // ReSharper disable once RedundantNameQualifier
            foreach (var icb in InputCommandBindings.Where(icb => icb.Gesture.Equals(gesture)).ToArray())
            {
                InputCommandBindings.Remove(icb);
            }
        }

        // Unbinds the specified command from all gestures.
        public virtual void Unbind(IViewCommand command)
        {
            // ReSharper disable once RedundantNameQualifier
            foreach (var icb in InputCommandBindings.Where(icb => object.ReferenceEquals(icb.Command, command)).ToArray())
            {
                InputCommandBindings.Remove(icb);
            }
        }

        /// <summary>
        /// Unbinds all commands.
        /// </summary>
        public virtual void UnbindAll()
        {
            InputCommandBindings.Clear();
        }

        // Binds the specified command to the specified gesture. Removes old bindings to the gesture.
        protected void BindCore(OxyInputGesture gesture, IViewCommand command)
        {
            var current = InputCommandBindings.FirstOrDefault(icb => icb.Gesture.Equals(gesture));
            if (current != null)
            {
                InputCommandBindings.Remove(current);
            }

            if (command != null)
            {
                InputCommandBindings.Add(new InputCommandBinding(gesture, command));
            }
        }

        // Gets the command for the specified <see cref="OxyInputGesture" />.
        protected virtual IViewCommand? GetCommand(OxyInputGesture gesture)
        {
            var binding = InputCommandBindings.FirstOrDefault(b => b.Gesture.Equals(gesture));
            if (binding == null)
            {
                return null;
            }

            return binding.Command;
        }

        // Handles a command triggered by an input gesture.
        protected virtual bool HandleCommand(IViewCommand command, IView view, OxyInputEventArgs args)
        {
            if (command == null)
            {
                return false;
            }

            command.Execute(view, this, args);

            args.Handled = true;
            return true;
        }

        // Gets the synchronization object for the specified view.
        protected object GetSyncRoot(IView view)
        {
            return view.ActualModel != null ? view.ActualModel.SyncRoot : _syncRoot;
        }
    }
}
