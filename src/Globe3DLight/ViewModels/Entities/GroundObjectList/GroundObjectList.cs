using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.ObjectModel;

namespace Globe3DLight.ViewModels.Entities
{
    public class GroundObjectList : ViewModelBase
    {
        private string _searchString;      
        private IList<GroundObject> _groundObjects;
        private GroundObject? _selectedGroundObject;
        private EntityList _gos;

        public GroundObjectList(EntityList gos)
        {
            _gos = gos;

            _groundObjects = CreateFrom(_gos);
            _selectedGroundObject = _groundObjects.FirstOrDefault();
            _searchString = string.Empty;

            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(GroundObjectList.SearchString))
                {
                    GroundObjects = CreateFrom(_gos);
                    SelectedGroundObject = GroundObjects.FirstOrDefault();
                }
            };
        }

        private IList<GroundObject> CreateFrom(EntityList source)
        {
            Func<GroundObject, bool> namePredicate =
                (s => (string.IsNullOrEmpty(SearchString) == false) ? s.Name.Contains(SearchString) : true);

            var list = source.Values.Cast<GroundObject>();

            return list.Where(namePredicate).ToList();
        }

        public string SearchString
        {
            get => _searchString;
            set => RaiseAndSetIfChanged(ref _searchString, value);
        }

        public IList<GroundObject> GroundObjects
        {
            get => _groundObjects;
            set => RaiseAndSetIfChanged(ref _groundObjects, value);
        }

        public GroundObject? SelectedGroundObject
        {
            get => _selectedGroundObject;
            set => RaiseAndSetIfChanged(ref _selectedGroundObject, value);
        }
    }
}
