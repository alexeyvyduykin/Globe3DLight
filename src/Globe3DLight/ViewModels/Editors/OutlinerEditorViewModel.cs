using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System.Reflection;

namespace Globe3DLight.ViewModels.Editors
{

    public interface ISceneModel
    {
        public bool IsVisible { get; set; }
    }

    public class ItemModel
    {
        public string Name { get; set; }

        public ObservableCollection<ItemModel> Children { get; set; } = new ObservableCollection<ItemModel>();
    }


    public class ItemSceneModel : ItemModel, ISceneModel
    {
        public bool IsVisible { get; set; }
    }

    public enum DisplayMode
    {
        Visual, 
        Logical
    }

    public class OutlinerEditorViewModel : ViewModelBase
    {
        private ObservableCollection<ItemModel> _visualItems;
        private ObservableCollection<ItemModel> _logicalItems;
        private DisplayMode _selectedMode;
        private ObservableCollection<ItemModel> _items;

        public OutlinerEditorViewModel()
        {
            var visualRoot = CreateVisualItems();
            var logicalRoot = CreateLogicalItems();
      
            _visualItems = new ObservableCollection<ItemModel>(visualRoot);

            _logicalItems = new ObservableCollection<ItemModel>(logicalRoot);
            
            PropertyChanged += OutlinerEditorViewModel_PropertyChanged;

            _selectedMode = DisplayMode.Visual;
            _items = _visualItems;
        }

        private void OutlinerEditorViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(SelectedMode))
            {
                switch (SelectedMode)
                {
                    case DisplayMode.Visual:
                        Items = _visualItems;
                        break;
                    case DisplayMode.Logical:
                        Items = _logicalItems;
                        break;
                    default:
                        break;
                }
            }
        }

        private List<ItemModel> CreateVisualItems()
        {
            List<ItemModel> list = new List<ItemModel>();

            List<ItemModel> root = new List<ItemModel>();

            List<ItemModel> sublist1 = new List<ItemModel>();
            List<ItemModel> sublist2 = new List<ItemModel>();
            List<ItemModel> sublist3 = new List<ItemModel>();

            sublist1.Add(new ItemSceneModel() { Name = "SubItem1", IsVisible = false });
            sublist1.Add(new ItemSceneModel() { Name = "SubItem2", IsVisible = true });

            sublist2.Add(new ItemSceneModel() { Name = "SubItem1", IsVisible = false });
            sublist2.Add(new ItemSceneModel() { Name = "SubItem2", IsVisible = true });
            sublist2.Add(new ItemSceneModel() { Name = "SubItem3", IsVisible = true });

            sublist3.Add(new ItemSceneModel() { Name = "SubItem1", IsVisible = false });

            list.Add(new ItemModel() { Name = "Item1" });
            list.Add(new ItemModel() { Name = "Item2", Children = new ObservableCollection<ItemModel>(sublist1) });
            list.Add(new ItemModel() { Name = "Item3" });
            list.Add(new ItemModel() { Name = "Item4", Children = new ObservableCollection<ItemModel>(sublist2) });
            list.Add(new ItemModel() { Name = "Item5", Children = new ObservableCollection<ItemModel>(sublist3) });

            root.Add(new ItemModel() { Name = "VisualRoot", Children = new ObservableCollection<ItemModel>(list) });

            return root;
        }

        private List<ItemModel> CreateLogicalItems()
        {
            List<ItemModel> list = new List<ItemModel>();

            List<ItemModel> root = new List<ItemModel>();

            List<ItemModel> sublist1 = new List<ItemModel>();
            List<ItemModel> sublist2 = new List<ItemModel>();

            sublist1.Add(new ItemSceneModel() { Name = "SubItem1", IsVisible = false });
            sublist1.Add(new ItemSceneModel() { Name = "SubItem2", IsVisible = true });

            sublist2.Add(new ItemSceneModel() { Name = "SubItem3", IsVisible = true });

            list.Add(new ItemModel() { Name = "Item1" });
            list.Add(new ItemModel() { Name = "Item2", Children = new ObservableCollection<ItemModel>(sublist1) });
            list.Add(new ItemModel() { Name = "Item3" });
            list.Add(new ItemModel() { Name = "Item4", Children = new ObservableCollection<ItemModel>(sublist2) });
      
            root.Add(new ItemModel() { Name = "LogicalRoot", Children = new ObservableCollection<ItemModel>(list) });

            return root;
        }

        public DisplayMode SelectedMode 
        {
            get => _selectedMode; 
            set => this.RaiseAndSetIfChanged(ref _selectedMode, value); 
        }

        public ObservableCollection<ItemModel> Items 
        {
            get => _items;
            set => this.RaiseAndSetIfChanged(ref _items, value); 
        }
    }
}
