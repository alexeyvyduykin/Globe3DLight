﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Globe3DLight
{
    /// <summary>
    /// Observable object.
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged, ICopyable
    {
        private bool _isDirty;
        private ObservableObject _owner = null;
        private string _name = "";

        /// <inheritdoc/>
        public virtual ObservableObject Owner
        {
            get => _owner;
            set => Update(ref _owner, value);
        }

        /// <inheritdoc/>
        public virtual string Name
        {
            get => _name;
            set => Update(ref _name, value);
        }

        /// <inheritdoc/>
        public virtual bool IsDirty()
        {
            return _isDirty;
        }

        /// <inheritdoc/>
        public virtual void Invalidate()
        {
            _isDirty = false;
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <inheritdoc/>
        public abstract object Copy(IDictionary<object, object> shared);

        /// <inheritdoc/>
        public void Notify([CallerMemberName] string propertyName = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <inheritdoc/>
        public bool Update<T>(ref T field, T value, [CallerMemberName] string propertyName = default)
        {
            if (!Equals(field, value))
            {
                field = value;
                _isDirty = true;
                Notify(propertyName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Check whether the <see cref="Owner"/> property has changed from its default value.
        /// </summary>
        /// <returns>Returns true if the property has changed; otherwise, returns false.</returns>
        public virtual bool ShouldSerializeOwner() => _owner != null;

        /// <summary>
        /// Check whether the <see cref="Name"/> property has changed from its default value.
        /// </summary>
        /// <returns>Returns true if the property has changed; otherwise, returns false.</returns>
        public virtual bool ShouldSerializeName() => !string.IsNullOrWhiteSpace(_name);

        /// <summary>
        /// The <see cref="_isDirty"/> property is not serialized.
        /// </summary>
        /// <returns>Returns always false.</returns>
        public virtual bool ShouldSerializeIsDirty() => false;
    }
}
