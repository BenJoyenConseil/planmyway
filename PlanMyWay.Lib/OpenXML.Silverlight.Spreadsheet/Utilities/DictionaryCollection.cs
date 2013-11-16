using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using OpenXML.Silverlight.Spreadsheet.Elements;
using System.Collections.Generic;
using System.Linq;
using FiftyNine.Ag.OpenXML.Common.Packaging;

namespace OpenXML.Silverlight.Spreadsheet.Utilities
{
    public class DictionaryCollection<T> where T : IndexedSpreadsheetElement
    {
        PackagePart _parent;
        Dictionary<int, T> _items = new Dictionary<int,T>();

        public DictionaryCollection(PackagePart parent)
        {
            _parent = parent;
        }

        public T this[int index]
        {
            get
            {
                if (!_items.ContainsKey(index))
                {
                    T item = _parent.CreateElement<T>();
                    item.SetUp(index);
                    _items.Add(index, item);
                    OnItemCreated(item);
                }
                return _items[index];
            }
            set
            {
                if (value == null)
                {
                    if (_items.ContainsKey(index))
                    {
                        _items.Remove(index);
                    }
                    return;
                }
                if (!_items.ContainsKey(index))
                {
                    _items.Add(index, value);
                }
                else
                {
                    _items[index] = value;
                }
            }
        }
        public IEnumerable<int> Indexes
        {
            get { return _items.Keys.OrderBy(k => k); }
        }

        protected virtual void OnItemCreated(T item)
        {
            if (ItemCreated != null)
                ItemCreated(this, new PayloadEventArgs<T>(item));
        }
        public event EventHandler<PayloadEventArgs<T>> ItemCreated;
    }

    public class PayloadEventArgs<T> : EventArgs
    {
        public PayloadEventArgs(T payload)
        {
            Payload = payload;
        }

        public T Payload
        {
            get;
            private set;
        }
    }
}
