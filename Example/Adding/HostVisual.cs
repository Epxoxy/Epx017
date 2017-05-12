using System;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Example
{
    [ContentProperty("Child")]
    public class VisualWrapper : FrameworkElement
    {
        public Visual Child
        {
            get { return _child; }
            set
            {
                if(_child != null)
                {
                    RemoveVisualChild(_child);
                }
                _child = value;
                if(_child != null)
                {
                    AddVisualChild(_child);
                }
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            if(_child != null && index == 0)
            {
                return _child;
            }
            else
            {
                throw new ArgumentOutOfRangeException("index");
            }
        }
        protected override int VisualChildrenCount
        {
            get
            {
                 return _child != null? 1 :0 ;
            }
        }
        private Visual _child;
    }

    public class VisualTargetPresentationSource : PresentationSource
    {
        public VisualTargetPresentationSource(HostVisual hostVisual)
        {
            _visualTarget = new VisualTarget(hostVisual);
        }

        public override bool IsDisposed
        {
            get
            {
                return false;
            }
        }

        public override Visual RootVisual
        {
            get
            {
                return _visualTarget.RootVisual;
            }

            set
            {
                Visual oldRoot = _visualTarget.RootVisual;

                _visualTarget.RootVisual = value;
                RootChanged(oldRoot, value);

                UIElement rootElement = value as UIElement;
                if(rootElement != null)
                {
                    rootElement.Measure(new Size(Double.PositiveInfinity, Double.PositiveInfinity));
                    rootElement.Arrange(new Rect(rootElement.DesiredSize));
                }
            }
        }

        protected override CompositionTarget GetCompositionTargetCore()
        {
            return _visualTarget;
        }

        private VisualTarget _visualTarget;
    }

}
