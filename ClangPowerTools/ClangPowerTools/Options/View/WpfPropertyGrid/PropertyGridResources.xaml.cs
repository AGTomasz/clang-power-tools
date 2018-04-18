﻿/*
 * Copyright 2002 - 2011 Caphyon LTD. All rights reserved.
 *
 * mailto: eng@caphyon.com
 * http://www.caphyon.com
 *
 */
using System.Windows;
using System.Windows.Input;

namespace Caphyon.AdvInstVSIntegration.ProjectEditor.View.WpfPropertyGrid
{
  /// <summary>
  /// Interaction logic for PropertyGridResources.xaml
  /// </summary>
  public partial class PropertyGridResources
  {
    public PropertyGridResources()
    {
      InitializeComponent();
    }

    private void TextBoxPreviewMouseUp(object sender, MouseButtonEventArgs e)
    {
      MouseUpDownEventForwarding(sender, e, UIElement.MouseUpEvent);
    }

    private void MouseUpDownEventForwarding(object sender, MouseButtonEventArgs e, RoutedEvent evFwd)
    {
      FrameworkElement tb = sender as FrameworkElement;
      if (null != tb && tb.Parent is UIElement)
      {
        MouseButtonEventArgs newEvArgs = new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, e.ChangedButton);
        newEvArgs.RoutedEvent = evFwd;
        ((UIElement)tb.Parent).RaiseEvent(newEvArgs);
      }
    }

    private void TextBoxPreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      MouseUpDownEventForwarding(sender, e, UIElement.MouseDownEvent);
    }
  }
}
