   M S B A M L   `   `   `  ����H  DSamplesCommon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:SamplesCommonSamplesCommon  5      %    SamplesCommon.AccentColorPicker   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      D @ModernWpf, Version=0.9.4.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.4.0, Culture=neutral, PublicKeyToken=null,ui!http://schemas.modernwpf.com/2019   5      )localclr-namespace:SamplesCommon   5      .��5      �� $��Choose your accent color��6   .��5   
   �� .+�5      !  ModernWpf.Controls.GridView  $��0q�5      $
��Left=�5      $	��Top=�5        �� SelectionMode$  Single=�5       �� SelectedValuePath$ Color��5      $��Disabled=�5        ModernWpf.ThemeManager   HasThemeResources$
 True��5       �� SelectedValue 5      �� *AccentColor+%   SamplesCommon.ThemeManagerProxy   Current �� Source# Z ��5      �� 5      .R�5      A�  A� Rows$ 6��5       A� Columns$ 8��5      5      ��5          SamplesCommon.AccentColors  5       ��5"      �� 5#       �� 
TargetType%  ModernWpf.Controls.GridViewItem"  6   .�5$      �� 	 �� Property	 � ��
 �� Value$
 0,0,2,3q�63   �� 5%      	 � ��$
 Stretch=�6G   �� 5&      	 � ��$
 Stretch=�6E   �� 5'      	 � ��
 5(      �� 5)   "    �� 
TargetType"  62   .R�5*   &   ��  ��Name$ ContentBorder��5+   )   #��z��5,   )   #��z��5-   )   #��z��5.   )   1  +ModernWpf.Controls.Primitives.ControlHelper   CornerRadius �� CornerRadius# z 5/   )    J� SnapsToDevicePixels$
 True��50   )   ._�51   *   � .+�52   .   ��  ��Name$ ContentPresenter��53   1   #��z��54   1   #��z��55   1   #��z��56   1   # z 57   1   �� 58   .    ��Name$ BorderRectangle��59   1    J� IsHitTestVisible$ False��5:   1    ,  (SystemControlHighlightListAccentLowBrush#���   5;   1   $��2��5<   1    J� Opacity$ 0Q�5=   1   �� 5>   .   $ MultiSelectSquare��5?   1    , (SystemControlBackgroundChromeMediumBrush#���  5@   1   $��20��5A   1   $��20��5B   1   $��0,2,2,0q�5C   1   $	��Top=�5D   1   $��Right=�5E   1   $y�	Collapsed=�5F   1   ._�5G   2   !  ModernWpf.Controls.FontIcon    Name$ MultiSelectCheck��5H   5     SymbolThemeFontFamily   
FontFamily# �  5I   5      Glyph$	 ��5J   5      FontSize$ 16(�5K   5    . *SystemControlForegroundBaseMediumHighBrush$  ModernWpf.Controls.IconElement   
Foreground# �  5L   5   $ 0Q�5M   5   5N   /   5O   +   5P   '    �� Triggers 5Q   &   P� 5R   *    P� Property �@J�IsMouseOver P� Value$
 True��6I   . �5S   .   ��  �� 
TargetName$ BorderRectangle��65   	 �@J�Opacity$
 1Q�6e   �� 5T   .   $ BorderRectangle��65    & "SystemControlHighlightListLowBrush#
 �  6d   	 � ���� 5U   .   $ ContentPresenter��65    ( $SystemControlForegroundBaseHighBrush#
 �  6u   	 � ��5V   +   P� 5W   *    �@��
IsSelected$
 True��6H   . �5X   .   �� $ MultiSelectCheck��65   	 �@J�Opacity$
 1Q�6f   �� 5Y   .   $ BorderRectangle��65   	 �@J�Opacity$
 1Q�6e   �� 5Z   .   $ BorderRectangle��65    % !SystemControlHighlightAccentBrush#
 �  6d   	 � ���� 5[   .   $ ContentPresenter��65   #
 �  6u   	 � ���� 5\   .   $ MultiSelectSquare��65   	 � y�$
 Visible=�6j   �� 5]   .   $ MultiSelectSquare��65   #
 �  6j   	 � ��5^   +   ]� 5_   *    ]� 
Conditions 5`   .   �� 5a   2    �� Property �@J�IsMouseOver �� Value$
 True��6S   �� 5b   2    �@��
IsSelected$
 True��6R   ./�5d   .   �� $ MultiSelectCheck��65   	 �@J�Opacity$
 1Q�6f   �� 5e   .   $ BorderRectangle��65   	 �@J�Opacity$
 1Q�6e   �� 5f   .   $ BorderRectangle��65    / +SystemControlHighlightListAccentMediumBrush#
 �  6d   	 � ���� 5g   .   $ ContentPresenter��65   #
 �  6u   	 � ���� 5h   .   $ MultiSelectSquare��65   	 � y�$
 Visible=�6j   �� 5i   .   $ MultiSelectSquare��65   #
 �  6j   	 � ��5j   +   P� 5k   *    � {�$ False��6G   . �5l   .   �� $ ContentBorder��65    $  ListViewItemDisabledThemeOpacity#
 �  6c   	 �@J�Opacity5m   +   5o   #   5q      5r      ��5t      �� 5u      .R�5v      ��   ^� InitialShowDelay$
  1000��5x      ! ^� BetweenShowDelay$	! 500��5y      ��5w      �� *Brush+" �� Mode$" OneTime=�# � ToolTip# 5z      _� 5{   "   ��6*   �� *Name+$" OneTime=�5}      5~      $ � 	Resources$ 5�      $	  ModernWpf.ResourceDictionaryEx% 	  ThemeDictionariesO Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089�
  �System.Collections.Generic.Dictionary`2[[System.Object, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089],[System.Windows.ResourceDictionary, PresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]] 	 Key  ModernWpf.ThemeDictionary
&   Key 
 SystemBaseHighColor' �� Color  SystemAltHighColor	  5�      % 5�      
 �� 5�      	Light	 $& Light��6?   �� 5�   "   &"SystemControlHighlightListLowBrush	 #' [
 6]   �� 5�   "   %!SystemControlHighlightAccentBrush	 #' [
 6\   �� 5�   "   /+SystemControlHighlightListAccentMediumBrush	 #' [
 6f   �� 5�   "   .*SystemControlForegroundBaseMediumHighBrush	 #' [ 6e   5�      �� 5�      Dark	 $
& Dark��6>   �� 5�   "   &"SystemControlHighlightListLowBrush	 #' [
 6]   �� 5�   "   %!SystemControlHighlightAccentBrush	 #' [
 6\   �� 5�   "   /+SystemControlHighlightListAccentMediumBrush	 #' [
 6f   �� 5�   "   .*SystemControlForegroundBaseMediumHighBrush	 #' [ 6e   5�      5�      %    5�      5�      �� 5�      -   5        $��Reset accent color��5�      $��0,12,0,0q�5�      5�      5�      5�      5�      