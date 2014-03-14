using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Watermark
{
    /// <summary>
    /// Provides attached behaviour to present content within an
    /// <see cref="AdornerLayer"/> atop existing content.
    /// </summary>
    public static class Watermark
    {
        /// <summary>
        /// Dependency Property initializer.
        /// </summary>
        static Watermark()
        {
            _adornedElements = new List<FrameworkElement>();

            FrameworkPropertyMetadata m = new FrameworkPropertyMetadata();
            m.AffectsRender = true;
            m.PropertyChangedCallback = _watermarkChanged;
            WatermarkProperty = DependencyProperty.RegisterAttached(
                "Watermark", typeof( object ), typeof( Watermark ), m );

            m = new FrameworkPropertyMetadata();
            m.AffectsRender = true;
            m.DefaultValue = Visibility.Hidden;
            m.PropertyChangedCallback = _visibilityChanged;
            WatermarkVisibilityProperty = DependencyProperty.RegisterAttached(
                "WatermarkVisibility", typeof( Visibility ), typeof( Watermark ), m );
        }


        /// <summary>
        /// Represents the Watermark attached property.
        /// </summary>
        public static readonly DependencyProperty WatermarkProperty;

        /// <summary>
        /// Represents the WatermarkVisibility attached property.
        /// </summary>
        public static readonly DependencyProperty WatermarkVisibilityProperty;


        /// <summary>
        /// Gets the content within the watermark for a given element.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to
        /// resolve the content of the watermark for.</param>
        /// <returns>The content of the watermark for the provided
        /// element.</returns>
        /// <exception cref="ArgumentNullException">element is null</exception>
        public static object GetWatermark( FrameworkElement element )
        {
            if( element == null )
            {
                throw new ArgumentNullException( "element" );
            }
            else
            {
                return element.GetValue( WatermarkProperty );
            }
        }
        
        /// <summary>
        /// Sets the content to be displayed within the watermark for
        /// a given element
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to
        /// set the content of the watermark for</param>
        /// <param name="watermark">The content to be displayed within the
        /// watermark.</param>
        /// <exception cref="ArgumentNullException">element is null</exception>
        public static void SetWatermark( FrameworkElement element, object watermark )
        {
            if( element == null )
            {
                throw new ArgumentNullException( "element" );
            }
            else
            {
                element.SetValue( WatermarkProperty, watermark );
            }
        }

        /// <summary>
        /// Gets the visibility of the watermark for a given element
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/>
        /// to resolve the visibility of the watermark for</param>
        /// <returns>The <see cref="Visibility"/> of the watermark for
        /// the provided element</returns>
        /// <exception cref="ArgumentNullException">element is null</exception>
        public static Visibility GetWatermarkVisibility( FrameworkElement element )
        {
            if( element == null )
            {
                throw new ArgumentNullException( "element" );
            }
            else
            {
                return (Visibility)element.GetValue( WatermarkVisibilityProperty );
            }
        }

        /// <summary>
        /// Sets the visibility of the watermark for the given element.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to set
        /// the visibility of the watermark against.</param>
        /// <param name="visibility">The <see cref="Visibility"/> to set the
        /// watermark.</param>
        /// <exception cref="ArgumentNullException">element is null</exception>
        public static void SetWatermarkVisibility( FrameworkElement element, Visibility visibility )
        {
            if( element == null )
            {
                throw new ArgumentNullException( "element" );
            }
            else
            {
                element.SetValue( WatermarkVisibilityProperty, visibility );
            }
        }

        /// <summary>
        /// Resolves the <see cref="WatermarkAdorner"/> for the provided
        /// element.
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to find
        /// the corresponding adorner for</param>
        /// <returns>The <see cref="WatermarkAdorner"/> for the given element,
        /// or null if no adorner exists.</returns>
        public static WatermarkAdorner AdornerForElement( FrameworkElement element )
        {
            if( _adornedElements.Contains( element ) )
            {
                return _getWatermarkAdorner( element );
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Occurs when the Watermark property of an object is modified.
        /// </summary>
        /// <param name="d">The object the property was modified against</param>
        /// <param name="e">Event information</param>
        private static void _watermarkChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            FrameworkElement element = (FrameworkElement)d;
            if( _adornedElements.Contains( element ) == false )
            {
                _adornedElements.Add( element );
                element.Loaded += _elementLoaded;
                element.Unloaded += _elementUnloaded;
            }

            _createOrUpdateWatermark( element, e.NewValue );
            _updateWatermarkVisibility( element );
        }

        /// <summary>
        /// Occurs when the Visibility property of an object is modified.
        /// </summary>
        /// <param name="d">The object the property was modified against</param>
        /// <param name="e">Event information</param>
        private static void _visibilityChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
        {
            FrameworkElement element = (FrameworkElement)d;
            _updateWatermarkVisibility( element );
        }

        /// <summary>
        /// Creates or updates the watermark for the given element
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to
        /// coerce the value of the adorner for</param>
        /// <param name="watermark">The content to present within the
        /// adorner</param>
        private static void _createOrUpdateWatermark( FrameworkElement element, object watermark )
        {
            WatermarkAdorner adorner = Watermark.AdornerForElement( element );
            if( adorner == null )
            {
                AdornerLayer layer = AdornerLayer.GetAdornerLayer( element );
                if( layer != null )
                {
                    WatermarkAdorner a = new WatermarkAdorner( element, watermark );
                    a.Visibility = Visibility.Hidden;
                    layer.Add( a );
                }
            }
            else
            {
                adorner.WatermarkContent = watermark;
            }
        }

        /// <summary>
        /// Ensures the visibility state of the watermark is up-to-date
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to update
        /// the visibility state for</param>
        private static void _updateWatermarkVisibility( FrameworkElement element )
        {
            if( GetWatermarkVisibility( element ) == Visibility.Visible )
            {
                _showWatermark( element );
            }
            else
            {
                _hideWatermark( element );
            }
        }

        /// <summary>
        /// Displays the watermark for the provided element
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to
        /// display the watermark for</param>
        private static void _showWatermark( FrameworkElement element )
        {
            WatermarkAdorner a = _getWatermarkAdorner( element );
            if( a != null )
            {
                a.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// Hides the watermark for the given element
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to hide
        /// the adorner for</param>
        private static void _hideWatermark( FrameworkElement element )
        {
            WatermarkAdorner a = _getWatermarkAdorner( element );
            if( a != null )
            {
                a.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Removes the watermark adorner from the given element
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to
        /// remove the adorner for</param>
        private static void _removeWatermark( FrameworkElement element )
        {
            WatermarkAdorner a = _getWatermarkAdorner( element );
            if( a != null )
            {
                a.Visibility = Visibility.Hidden;
                AdornerLayer layer = AdornerLayer.GetAdornerLayer( element );
                layer.Remove( a );
            }
        }

        /// <summary>
        /// Resolves the <see cref="WatermarkAdorner"/> for the given element
        /// </summary>
        /// <param name="element">The <see cref="FrameworkElement"/> to resolve
        /// the adorner for</param>
        /// <returns>The <see cref="WatermarkAdorner"/> for the given element,
        /// or null if no adorner is found</returns>
        private static WatermarkAdorner _getWatermarkAdorner( FrameworkElement element )
        {
            AdornerLayer layer = AdornerLayer.GetAdornerLayer( element );
            if( layer == null )
            {
                return null;
            }
            else
            {
                return _getWatermarkAdorner( layer, element );
            }
        }

        /// <summary>
        /// Resolves the <see cref="WatermarkAdorner"/> for the provided
        /// element info
        /// </summary>
        /// <param name="layer">The <see cref="AdornerLayer"/> the adorner resides
        /// within</param>
        /// <param name="element">The <see cref="FrameworkElement"/> being adorned
        /// by the provided layer</param>
        /// <returns>The <see cref="WatermarkAdorner"/> for the given element, or
        /// null if no adorner exists for the element</returns>
        private static WatermarkAdorner _getWatermarkAdorner( AdornerLayer layer, FrameworkElement element )
        {
            var adorners = layer.GetAdorners( element ) ?? new Adorner[0];
            return adorners.OfType<WatermarkAdorner>().FirstOrDefault();
        }

        /// <summary>
        /// Occurs when an adorned <see cref="FrameworkElement"/> has been loaded.
        /// </summary>
        /// <param name="sender">The <see cref="FrameworkElement"/>.</param>
        /// <param name="e">Event information.</param>
        private static void _elementLoaded( object sender, RoutedEventArgs e )
        {
            FrameworkElement element = (FrameworkElement)sender;
            _createOrUpdateWatermark( element, GetWatermark( element ) );
            _updateWatermarkVisibility( element );
        }

        /// <summary>
        /// Occurs when an adorned <see cref="FrameworkElement"/> has been unloaded.
        /// </summary>
        /// <param name="sender">The <see cref="FrameworkElement"/>.</param>
        /// <param name="e">Event information.</param>
        private static void _elementUnloaded( object sender, RoutedEventArgs e )
        {
            FrameworkElement element = (FrameworkElement)sender;
            element.Loaded -= _elementLoaded;
            element.Unloaded -= _elementUnloaded;
            _adornedElements.Remove( element );
        }


        /// <summary>
        /// Maintains the set of adorned elements.
        /// </summary>
        [DebuggerBrowsable( DebuggerBrowsableState.Never )]
        private static ICollection<FrameworkElement> _adornedElements;
    }
}
