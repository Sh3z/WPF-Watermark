WPF-Watermark
=============

Provides attached behaviour for adorning content in WPF

Loosely based on http://stackoverflow.com/questions/18843737/attached-property-with-watermark-textbox

Usage example
---
```
<UserControl
  x:Class="UserControl"
  xmlns:Watermark="clr-namespace:Watermark;assembly=Watermark">
  <UserControl.Resources>
    <BooleanToVisibilityConverter x:Key="BooleanToVisibilityConverter" />
  </UserControl.Resources>
  <DockPanel Watermark:Watermark.WatermarkVisibility="{Binding Path=IsWatermarkVisible, Converter={StaticResource BooleanToVisibilityConverter}}">
    <Watermark:Watermark.Watermark>
      <TextBlock Text="This is a TextBlock watermark" HorizontalAlignment="Center" VerticalAlignment="Center" />
    </Watermark:Watermark.Watermark>
  </DockPanel>
</UserControl>
```
