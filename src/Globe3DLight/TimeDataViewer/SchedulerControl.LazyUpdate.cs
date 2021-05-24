using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Threading;
using Avalonia.LogicalTree;
using System.ComponentModel;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Styling;
using Avalonia.VisualTree;
using TimeDataViewer.ViewModels;
using TimeDataViewer;
using TimeDataViewer.Spatial;
using System.Xml;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Controls.Metadata;
using Avalonia.Input.GestureRecognizers;
using Avalonia.Input.TextInput;
using Avalonia.Interactivity;
using Avalonia.Controls.Primitives;
using Avalonia.Media.Imaging;
using TimeDataViewer.Models;
using TimeDataViewer.Core;
using Avalonia.Controls.Generators;
using System.Threading.Tasks;
using TimeDataViewer.Views;
using Avalonia.Collections;

namespace TimeDataViewer
{
    public partial class SchedulerControl
    {
        private DispatcherTimer _timer;
        private bool _init = false;
        private bool _dirty = false;
        private bool _complete = true;       
        private int _iBegSave = 0;
        private int _jBegSave = 0;
        private readonly int _maxPacks = 10;

        private void SeriesInvalidateDataEvent(object? sender, EventArgs e)
        {
            SeriesInvalidateData();
        }

        private void SeriesInvalidateData()
        {
            if (Series.All(s => s.DirtyItems) == true)
            {
                if (_complete == true)
                {
                    foreach (var item in Series)
                    {
                        item.DirtyItems = false;
                    }

                    _init = false;
                    _dirty = false;
                    _complete = false;
                    _iBegSave = 0;
                    _jBegSave = 0;

                    _seriesViewModels = Series.Select(s => s.SeriesViewModel).ToList();
                    _epoch = Epoch;

                    UpdateViewport();

                    _markers.Clear();

                    StartLazyUpdate();
                }
                else
                {
                    _dirty = true;
                }
            }
        }

        private void StartLazyUpdate()
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(10), DispatcherPriority.Normal, new EventHandler(OnStartLazyUpdate));
            }

            if (_complete == false)
            {
                _timer.Start();
            }
        }

        private void StopLazyUpdate()
        {
            _complete = true;

            if(_dirty == true)
            {
                SeriesInvalidateData();
            }
        }

        private void OnStartLazyUpdate(object? sender, EventArgs args)
        {
            _timer.Stop();
            LazyUpdateMarkers();
        }

        private void LazyUpdateMarkers()
        {
            int packs = 0;
         
            if (_init == false)
            {                                
                for (int i = 0; i < _seriesViewModels.Count; i++)
                {
                    _markers.Add(_seriesViewModels[i]);
                    packs++;
                }

                _init = true;
            }

            for (int i = _iBegSave; i < _seriesViewModels.Count; i++)
            {
                for (int j = _jBegSave; j < _seriesViewModels[i].Intervals.Count; j++)
                {
                    if (++packs > _maxPacks)
                    {
                        _iBegSave = i;
                        _jBegSave = j;

                        StartLazyUpdate();

                        return;
                    }

                    _markers.Add(_seriesViewModels[i].Intervals[j]);
                }

                _jBegSave = 0;
            }

            InvalidateVisual();
            
            StopLazyUpdate();
        }
    }
}
