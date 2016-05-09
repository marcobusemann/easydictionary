using System;
using Windows.UI.Xaml.Controls;

namespace SplitViewMenu
{
    public sealed class SimpleNavMenuItem : INavigationMenuItem
    {
        public string Label { get; set; }
        public string FontFamily { get; set; }
        public string Symbol { get; set; }
        public object Arguments { get; set; }
        public Type DestinationPage { get; set; }
    }
}