   M S B A M L   `   `   `  ���� H  DSamplesCommon, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:SamplesCommonSamplesCommon  %    SamplesCommon.AccentColorPicker   W SPresentationCore, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     D @ModernWpf, Version=0.9.4.0, Culture=neutral, PublicKeyToken=nullM IModernWpf.Controls, Version=0.9.4.0, Culture=neutral, PublicKeyToken=nullL HModernWpf.MahApps, Version=0.9.4.0, Culture=neutral, PublicKeyToken=null.ui!http://schemas.modernwpf.com/2019    +localclr-namespace:SamplesCommon    .���� $��Choose your accent color��.���� .+�!  ModernWpf.Controls.GridView  $��0q�$
��Left=�$	��Top=�  �� SelectionMode$  Single=� �� SelectedValuePath$ Color��$��Disabled=�  ModernWpf.ThemeManager   HasThemeResources$
 True�� �� SelectedValue �� *AccentColor+%   SamplesCommon.ThemeManagerProxy   Current �� Source# Z ���� .R�A�  A� Rows$ 6�� A� Columns$ 8����    SamplesCommon.AccentColors  ����  �� 
TargetType%  ModernWpf.Controls.GridViewItem"  .��� 	 �� Property	 � ��
 �� Value$
 0,0,2,3q��� 	 � ��$
 Stretch=��� 	 � ��$
 Stretch=��� 	 � ��
 ��  �� 
TargetType"  .R���  ��Name$ ContentBorder��#��z��#��z��#��z��1  +ModernWpf.Controls.Primitives.ControlHelper   CornerRadius �� CornerRadius# z  J� SnapsToDevicePixels$
 True��._�� .+���  ��Name$ ContentPresenter��#��z��#��z��#��z��# z ��  ��Name$ BorderRectangle�� J� IsHitTestVisible$ False�� ,  (SystemControlHighlightListAccentLowBrush#���   $��2�� J� Opacity$ 0Q��� $ MultiSelectSquare�� , (SystemControlBackgroundChromeMediumBrush#���  $��20��$��20��$��0,2,2,0q�$	��Top=�$��Right=�$y�	Collapsed=�._�!  ModernWpf.Controls.FontIcon    Name$ MultiSelectCheck��  SymbolThemeFontFamily   
FontFamily# �     Glyph$	 ��   FontSize$ 16(� . *SystemControlForegroundBaseMediumHighBrush$  ModernWpf.Controls.IconElement   
Foreground# �  $ 0Q� �� Triggers P�  P� Property �@J�IsMouseOver P� Value$
 True��. ���  �� 
TargetName$ BorderRectangle��	 �@J�Opacity$
 1Q��� $ BorderRectangle�� & "SystemControlHighlightListLowBrush#
 �  	 � ���� $ ContentPresenter�� ( $SystemControlForegroundBaseHighBrush#
 �  	 � ��P�  �@��
IsSelected$
 True��. ��� $ MultiSelectCheck��	 �@J�Opacity$
 1Q��� $ BorderRectangle��	 �@J�Opacity$
 1Q��� $ BorderRectangle�� % !SystemControlHighlightAccentBrush#
 �  	 � ���� $ ContentPresenter��#
 �  	 � ���� $ MultiSelectSquare��	 � y�$
 Visible=��� $ MultiSelectSquare��#
 �  	 � ��]�  ]� 
Conditions ��  �� Property �@J�IsMouseOver �� Value$
 True����  �@��
IsSelected$
 True��./��� $ MultiSelectCheck��	 �@J�Opacity$
 1Q��� $ BorderRectangle��	 �@J�Opacity$
 1Q��� $ BorderRectangle�� / +SystemControlHighlightListAccentMediumBrush#
 �  	 � ���� $ ContentPresenter��#
 �  	 � ���� $ MultiSelectSquare��	 � y�$
 Visible=��� $ MultiSelectSquare��#
 �  	 � ��P�  � {�$ False��. ��� $ ContentBorder�� $  ListViewItemDisabledThemeOpacity#
 �  	 �@J�Opacity���� .R���   ^� InitialShowDelay$
  1000��! ^� BetweenShowDelay$	! 500������ *Brush+" �� Mode$" OneTime=�# � ToolTip# _� ���� *Name+$" OneTime=�$ � 	Resources$ $	  ModernWpf.ResourceDictionaryEx% 	  ThemeDictionariesY	 USystem.Collections, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a�
 	 �System.Collections.Generic.Dictionary`2[[System.Object, System.Runtime, Version=5.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a],[System.Windows.ResourceDictionary, PresentationFramework, Version=5.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35]] 	 Key  ModernWpf.ThemeDictionary
&   Key 
 SystemBaseHighColor' �� Color  SystemAltHighColor	  % 
 �� 	Light	 $& Light���� &"SystemControlHighlightListLowBrush	 #' [
 �� %!SystemControlHighlightAccentBrush	 #' [
 �� /+SystemControlHighlightListAccentMediumBrush	 #' [
 �� .*SystemControlForegroundBaseMediumHighBrush	 #' [ �� Dark	 $
& Dark���� &"SystemControlHighlightListLowBrush	 #' [
 �� %!SystemControlHighlightAccentBrush	 #' [
 �� /+SystemControlHighlightListAccentMediumBrush	 #' [
 �� .*SystemControlForegroundBaseMediumHighBrush	 #' [ %    �� -   $��Reset accent color��$��0,12,0,0q�