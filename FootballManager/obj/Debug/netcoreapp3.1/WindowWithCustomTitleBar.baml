   M S B A M L   `   `   `  ����J  FFootballManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:FootballManagerFootballManager  5	      H DSamplesCommon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullD2clr-namespace:SamplesCommon;assembly=SamplesCommonSamplesCommon 5
      .    (FootballManager.WindowWithCustomTitleBar   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      D @ModernWpf, Version=0.9.4.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.4.0, Culture=neutral, PublicKeyToken=nullL	 HModernWpf.MahApps, Version=0.9.4.0, Culture=neutral, PublicKeyToken=null.ui!http://schemas.modernwpf.com/2019   	 5      -localclr-namespace:FootballManager   	 5	      ?sc2clr-namespace:SamplesCommon;assembly=SamplesCommon   	 5
      -   5          �Name$  Window��5       � Title$ Custom Title Bar��5      $	��200��5      $	��200��5       � WindowStartupLocation$ CenterOwner=�5       3  /SystemControlPageBackgroundChromeMediumLowBrush#���   5      0  *ModernWpf.Controls.Primitives.WindowHelper   UseModernWindowStyle$
 True��5      !  ModernWpf.Controls.TitleBar   ExtendViewIntoTitleBar$
 True��5         IsBackButtonVisible$
 True��5        AppTitleBarStyle   Style# �  5        AppTitleBarButtonStyle   ButtonStyle# �  5        AppTitleBarBackButtonStyle   BackButtonStyle# �  5        ModernWpf.ThemeManager	   RequestedTheme$
	 Dark=�5      
 � 	Resources
 5      /  )SamplesCommon.PixelsToGridLengthConverter  PixelsToGridLength �� 
TargetType3  -ModernWpf.Controls.Primitives.TitleBarControl �� Property �� Value2  ,ModernWpf.Controls.Primitives.TitleBarButton �� ElementName  TitleBarBackButtonStyle �� BasedOn��%s  &	       &	      &	 l     &	 �     7    5   
   6)   �� 5   
   "  6)   .�5      ��  �@ RequestedTheme$
 Dark=�6?   5      �� 5   
   "  6/   .�5       ��  �@ IsActive 6)   �� *
IsActive+$ Window��5!      �� 5"   
   "  63   8   .�5#      ��  �@ IsActive 6)   �� *
IsActive+$ Window��5$      5%      .��5'      �  � RowDefinitions 5(   
   �� 5)      $
��Auto �6   �� 5*      .+�5-   
   � -   5         �Name$ AppTitleBar��5.       & "SystemControlBackgroundAccentBrush#���  5/      $
	 Dark=�51      ��50      �� $ Window�� �� Path$ (ui:TitleBar.Height)���53      �� 54      " �6   .�55      ��  � �� ( $SystemControlForegroundBaseHighBrush# �  6?    �� Triggers 56      �� 57       �� Binding 6&   �� *
IsActive+$ Window�� �� Value	 False6W   .a�58      ��  � �� + 'SystemControlDisabledBaseMediumLowBrush# �  6G   59      5;       � ColumnDefinitions 5>      �� 5?      ��6#   �� $ Window��$* $(ui:TitleBar.SystemOverlayLeftInset)� �� 	Converter# [ �� 5@      $
��Auto �6#   �� 5A      �� 5B      $
��Auto �6#   �� 5C      ��6#   �� $ Window��$+ %(ui:TitleBar.SystemOverlayRightInset)�# [ .+�5F        -   5          Name$ ForwardButton��5G      $��1��5H      #��[ 5I      $	����5J      ��5K      Z� �� 5N      $��0��5O      $��5��5P      $��13(�5Q      $��Center=�5R      $��Center=�5S      ��5T      �� $ Window��$ Title��� 5V      $��3��5W       �� Orientation$ 
Horizontal=�5X      .+�5Y        #��[ 5Z       !	 ContentControlThemeFontFamily#��� 	 5[      $��13(�5\      $	��NaN��5]      $
��16,0q�5^      $��	User Name��5_      �� 5`      $��1��5b      $
��24,8q�5c      ��5a      �� $ AppTitleBar��$ (TextElement.Foreground)�5c      5d      5e      5g      5h      