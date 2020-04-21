using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace TsTimeline
{
    public static class ObservableCollectionEx 
    {
        public static ObservableCollection<TDest> ToFromSyncedObservableCollection<TDest, TSource>(this ObservableCollection<TSource> from , Func<TSource,TDest> converter)
        {
            var collection = new ObservableCollection<TDest>(from.Select(converter));
            var pairs = new List<Tuple<TSource, TDest>>();

            for (int i = 0; i < from.Count; ++i)
            {
                pairs.Add(new Tuple<TSource, TDest>(from[i],collection[i]));                
            }
            
            from.CollectionChanged += (s, e) =>
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var startIndex = 0;
                
                    var pair = pairs.FirstOrDefault();
                    if (pair != null)
                    {
                        startIndex = collection.IndexOf(pair.Item2);
                    }

                    var targetindex = startIndex + e.NewStartingIndex;
                    
                    var source = (TSource) e.NewItems[0];
                    var dest = converter(source);
                    pairs.Add(new Tuple<TSource, TDest>(source,dest));
                    collection.Insert(targetindex,dest);
                }

                if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    var source = (TSource) e.OldItems[0];
                    var item = pairs.First(x => x.Item1.GetHashCode() == source.GetHashCode());
                    pairs.Remove(item);
                    collection.Remove(item.Item2);
                };
            };
            return collection;
        }
    }
}