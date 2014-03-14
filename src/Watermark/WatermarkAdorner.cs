using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace Watermark
{
    /// <summary>
    /// Represents an <see cref="Adorner"/> used to present content using
    /// the <see cref="Watermark"/> attached behaviour.
    /// </summary>
    public class WatermarkAdorner : Adorner
    {
        /// <summary>
        /// Dependency Property initializer.
        /// </summary>
        static WatermarkAdorner()
        {
            FrameworkPropertyMetadata m = new FrameworkPropertyMetadata();
            m.AffectsArrange = true;
            m.AffectsMeasure = true;
            m.AffectsRender = true;
            m.DefaultValue = null;
            WatermarkContentProperty = DependencyProperty.Register(
                "WatermarkContent", typeof( object ), typeof( WatermarkAdorner ) );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkAdorner"/>
        /// class.
        /// </summary>
        /// <param name="adornedElement">The element to bind the adorner
        /// to.</param>
        /// <exception cref="ArgumentNullException">adornedElement is
        /// null.</exception>
        public WatermarkAdorner( UIElement adornedElement )
            : base( adornedElement )
        {
            IsHitTestVisible = false;
            _presenter = new ContentPresenter();
            Binding b = new Binding( "WatermarkContent" );
            b.Source = this;
            b.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            BindingOperations.SetBinding( _presenter, ContentPresenter.ContentProperty, b );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WatermarkAdorner"/>
        /// class with the provided watermark content.
        /// </summary>
        /// <param name="adornedElement">The element to bind the adorner
        /// to.</param>
        /// <param name="watermark">The content to present within the
        /// adorner.</param>
        /// <exception cref="ArgumentNullException">adornedElement is
        /// null.</exception>
        public WatermarkAdorner( UIElement adornedElement, object watermark )
            : this( adornedElement )
        {
            WatermarkContent = watermark;
        }


        /// <summary>
        /// Identifies the WatermarkContent Dependency Property.
        /// </summary>
        public static readonly DependencyProperty WatermarkContentProperty;


        /// <summary>
        /// Gets or sets the displayed content within this
        /// <see cref="WatermarkAdorner"/>. This is a Dependency Property.
        /// </summary>
        public object WatermarkContent
        {
            get
            {
                return GetValue( WatermarkContentProperty );
            }
            set
            {
                SetValue( WatermarkContentProperty, value );
            }
        }

        /// <summary>
        /// Gets the number of children for the <see cref="ContainerVisual"/>.
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return 1;
            }
        }


        /// <summary>
        /// Returns a specified child <see cref="Visual"/> for the parent
        /// <see cref="ContainerVisual"/>.
        /// </summary>
        /// <param name="index">A 32-bit signed integer that represents the
        /// index value of the child <see cref="Visual"/>. The value of index must be between 0 and <see cref="VisualChildrenCount"/> - 1.</param>
        /// <returns>The child <see cref="Visual"/>.</returns>
        protected override Visual GetVisualChild( int index )
        {
            return _presenter;
        }

        /// <summary>
        /// Implements any custom measuring behavior for the adorner.
        /// </summary>
        /// <param name="constraint">A size to constrain the adorner to.</param>
        /// <returns>A <see cref="Size"/> object representing the amount of layout
        /// space needed by the adorner.</returns>
        protected override Size MeasureOverride( Size constraint )
        {
            Size adornedElementSize = AdornedElement.RenderSize;
            _presenter.Measure( adornedElementSize );
            return adornedElementSize;
        }

        /// <summary>
        /// When overridden in a derived class, positions child elements and determines
        /// a size for a <see cref="FrameworkElement"/> derived class. 
        /// </summary>
        /// <param name="finalSize">The final area within the parent that this element
        /// should use to arrange itself and its children.</param>
        /// <returns>The actual size used.</returns>
        protected override Size ArrangeOverride( Size finalSize )
        {
            _presenter.Arrange( new Rect( finalSize ) );
            return finalSize;
        }

        


        /// <summary>
        /// Retains the <see cref="ContentPresenter"/> used to render the
        /// watermark's content.
        /// </summary>
        [DebuggerBrowsable( DebuggerBrowsableState.Never )]
        private ContentPresenter _presenter;
    }
}
