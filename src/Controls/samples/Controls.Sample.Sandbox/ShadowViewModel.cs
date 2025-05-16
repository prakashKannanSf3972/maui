#nullable disable
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maui.Controls.Sample
{
    public class ShadowViewModel : INotifyPropertyChanged
    {
        private Color _color = Colors.Black;
        private double _offsetX = 0;
        private double _offsetY = 0;
        private double _radius = 10;
        private float _opacity = 0.5f;
        private Point _offset = new Point(0, 0);
        private FlowDirection _flowDirection = FlowDirection.LeftToRight;
        private bool _isEnabled = true;
        private bool _isVisible = true;

        public ShadowViewModel()
        {
            ResetCommand = new Command(Reset);
        }

        public ICommand ResetCommand { get; }

        public Color Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }

        public double OffsetX
        {
            get => _offsetX;
            set
            {
                if (_offsetX != value)
                {
                    _offsetX = value;
                    Offset = new Point(_offsetX, _offsetY);
                    OnPropertyChanged();
                }
            }
        }

        public double OffsetY
        {
            get => _offsetY;
            set
            {
                if (_offsetY != value)
                {
                    _offsetY = value;
                    Offset = new Point(_offsetX, _offsetY);
                    OnPropertyChanged();
                }
            }
        }

        public Point Offset
        {
            get => _offset;
            set
            {
                if (_offset != value)
                {
                    _offset = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Radius
        {
            get => _radius;
            set
            {
                if (_radius != value)
                {
                    _radius = value;
                    OnPropertyChanged();
                }
            }
        }

        public float Opacity
        {
            get => _opacity;
            set
            {
                if (_opacity != value)
                {
                    _opacity = value;
                    OnPropertyChanged();
                }
            }
        }

        public FlowDirection FlowDirection
        {
            get => _flowDirection;
            set
            {
                if (_flowDirection != value)
                {
                    _flowDirection = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEnabled
        {
            get => _isEnabled;
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                if (_isVisible != value)
                {
                    _isVisible = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Reset()
        {
            Color = Colors.Black;
            OffsetX = 0;
            OffsetY = 0;
            Radius = 10;
            Opacity = 0.5f;
            FlowDirection = FlowDirection.LeftToRight;
            IsEnabled = true;
            IsVisible = true;
        }

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
