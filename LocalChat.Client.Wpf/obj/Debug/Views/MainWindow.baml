   M S B A M L   `   `   `  ����O  KLocalChat.Client.Wpf, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:LocalChat.Client.WpfLocalChat.Client.Wpf  5      %    LocalChat.Client.Wpf.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Z VDevExpress.Mvvm.UI, Version=18.2.0.0, Culture=neutral, PublicKeyToken=382ad28f6ae31f30W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      W SDevExpress.Mvvm, Version=18.2.0.0, Culture=neutral, PublicKeyToken=382ad28f6ae31f30Adxmvvm2http://schemas.devexpress.com/winfx/2008/xaml/mvvm   5      0local"clr-namespace:LocalChat.Client.Wpf   5      V RMaterialDesignThemes.Wpf, Version=2.5.0.1205, Culture=neutral, PublicKeyToken=nullFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5	        � Title$  
Local Chat��5
      $	��500��5      $	��600��5         MaterialDesignPaper#���   5        MaterialDesignFont#���  5      $��13(�5      $��Regular"�5        MaterialDesignBody#���  5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5       � DataContext 5      (   "LocalChat.Client.Wpf.MainViewModel  5   
   2  ,DevExpress.Mvvm.UI.Interactivity.Interaction   	Behaviors 5      '  !DevExpress.Mvvm.UI.EventToCommand  5   
   �  �DevExpress.Mvvm.UI.Interactivity.EventTriggerBase`1[[System.Windows.DependencyObject, WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]]   	EventName$ Closing��6I   +  %DevExpress.Mvvm.UI.EventToCommandBase   Command 6    �� *WindowClosingCommand+6I   .��5      )  #MaterialDesignThemes.Wpf.DialogHost     
Identifier$ 
RootDialog��6     5   
     5      $ Loaded��6L    6$   �� *WindowLoadedCommand+6L   .��5   
   � -   5         �Name$ MainGrid��5      	 � RowDefinitions	 5       �� 5!      $��* �6    �� 5"      $
��0.4* �6    .+�5$      �� $��0��5%      $��	12,0,12,0q�5&      ��5'      �� *
Messages+��5(      �� 5)      .R�5*      �� 
 ��Name$
 Border��6!    �� Padding$ 8q�61   ._�5+      � .+�5,   "   �� $��1��6-   $��8,0,0,0q�6=   .+�5-   &   �� $��0,0,0,3q�5.   )   $��11(�5/   )   $
��Bold"�50   )   ��51   )   �� *
Username+#  MaterialDesignThemes.Wpf.Card  52   &   $
��Auto��53   )   $��8q�54   )     PrimaryHueLightBrush#���  55   )    " PrimaryHueLightForegroundBrush#���  56   )   .��57   *   ��  �� Orientation$ 
Horizontal=�65   .+�58   .   �� $��14(�68   ��6F   �� *Text+�� 59   .   $��5,0,0,0q�5:   1   $��Bottom=�5;   1   $��9(�5<   1   ��5=   1   �� *CreatedDate+ �� StringFormat$ hh:mm��5>   +   5?   '   5@   #   5A      5B       �� Triggers 5C      �� 5D       �� Value$
 True��6I    �� Binding 6*   �� *
IsSelected+.a�5E   "   ��  �� 
TargetName$ Border��6)     MaterialDesignSelection �� Value# �  6S    �� Property � ��5F      5H      ��5J      �� 5K       �� 
TargetType" ��6   .�5L      ��  � {� .  66   5M       5O      1	   +LocalChat.Client.Wpf.ListViewScrollBehavior	  5P      5R      �� 5S      $��1��5T      $	��100��5U      $��30,30,30,30q�5V      $��Bottom=�5W      )
  #MaterialDesignThemes.Wpf.HintAssist 
  Hint$ Enter your message��5X       �� AcceptsReturn .  * &MaterialDesignOutlinedTextFieldTextBox#��[ 5Z       �� TextWrapping$
 Wrap=�5[      " �� VerticalScrollBarVisibility$
 Auto=�5\      .��64   ��5]      �� 5^       �� Mode$ TwoWay=�5_       �� Path$ CurrentMessage�5`       �� UpdateSourceTrigger$ PropertyChanged=�5a      .��5b        J� InputBindings 5c      �� 5d      
 �� Key$ Enter��6!    �� Command 6-   �� *SendMessage+.��5e   )   5f      5g      5h      5i      