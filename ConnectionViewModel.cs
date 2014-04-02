﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WilCommon;
using System.Reflection;
using System.Diagnostics;

namespace RgbToSpectrum
{
    /// <summary>
    /// Wraps Filters list
    /// </summary>
    public class ConnectionViewModel : ViewModelBase
    {
        public ConnectionViewModel()
        {
            filters = AppDomain.CurrentDomain.GetAssemblies()
                       .SelectMany(assembly => assembly.GetTypes())
                       .Where(type => type.IsSubclassOf(typeof(Filter)) && !type.IsAbstract && (type.GetConstructor(Type.EmptyTypes) != null))
                       .Select(t => (Filter)Activator.CreateInstance(t)).ToList();

            SelectedFilter = filters[0];
        }

        private readonly IList<Filter> filters;
        public IEnumerable<Filter> Filters { get { return filters; } }

        private Filter selectedFilter;
        public Filter SelectedFilter
        {
            get { return selectedFilter; }
            set { SetValue(ref selectedFilter, value); }
        }
    }
    


    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        protected bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);

            return true;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}