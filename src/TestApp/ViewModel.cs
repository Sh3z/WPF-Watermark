using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watermark.TestApp
{
    /// <summary>
    /// Demonstrative view-model.
    /// </summary>
    public class ViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModel"/>
        /// class.
        /// </summary>
        public ViewModel()
        {
            _isWatermarkVisible = false;
        }


        /// <summary>
        /// Occurs when the value of a property is changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Gets or sets whether the watermark is visible.
        /// </summary>
        public bool IsWatermarkVisible
        {
            get
            {
                return _isWatermarkVisible;
            }
            set
            {
                _isWatermarkVisible = value;
                if( PropertyChanged != null )
                {
                    PropertyChanged( this, new PropertyChangedEventArgs( "IsWatermarkVisible" ) );
                }
            }
        }
        [DebuggerBrowsable( DebuggerBrowsableState.Never )]
        private bool _isWatermarkVisible;
    }
}
