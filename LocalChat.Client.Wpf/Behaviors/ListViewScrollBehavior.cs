using DevExpress.Mvvm.UI.Interactivity;
using System.Collections.Specialized;
using System.Windows.Controls;

namespace LocalChat.Client.Wpf
{
    public class ListViewScrollBehavior : Behavior<ListView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged += ListViewCollectionChanged;
        }
        private void ListViewCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                AssociatedObject.ScrollIntoView(e.NewItems[0]);
            }
        }
        protected override void OnDetaching()
        {
            ((INotifyCollectionChanged)AssociatedObject.Items).CollectionChanged -= ListViewCollectionChanged;
            base.OnDetaching();
        }
    }
}
