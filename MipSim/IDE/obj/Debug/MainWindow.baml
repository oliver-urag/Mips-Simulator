   M S B A M L   `   `   `  ����  IDE    IDE.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   ` \ICSharpCode.AvalonEdit, Version=4.3.2.9632, Culture=neutral, PublicKeyToken=9cc39be672370310?
avalonEdit.http://icsharpcode.net/sharpdevelop/avalonedit  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  MIPS Simulator��5   	   
�������$	��700��67   $	��800��6C    � 
ResizeMode$ CanMinimize=�6P   .��5      ]� .+�5   
   +  %System.Windows.Controls.Ribbon.Ribbon  $	��Top=�6    �� SelectedIndex$ 0��6&      ApplicationMenu 5	      :  4System.Windows.Controls.Ribbon.RibbonApplicationMenu  5
      5  /System.Windows.Controls.Ribbon.RibbonMenuButton   KeyTip$ F��6(   .@�5      >  8System.Windows.Controls.Ribbon.RibbonApplicationMenuItem  $��Preferences��60     5      $
��Exit��60   5      .@�5      .  (System.Windows.Controls.Ribbon.RibbonTab  $��Toolbar��6      KeyTip$ H��6)   
�������.@�5      0  *System.Windows.Controls.Ribbon.RibbonGroup  -   5          Name$ 	FileGroup��5      $
��File��61   .@�5      1  +System.Windows.Controls.Ribbon.RibbonButton  -   5           Label$	 New��5   #      KeyTip$ X��6/   	   LargeImageSource$	 Images\new-file.png��6:   
   ControlSizeDefinition
 5      @  :System.Windows.Controls.Ribbon.RibbonControlSizeDefinition  5         	ImageSize$ Large=�6:      IsLabelVisible . 6L   5        5      -   5        $
 Open��5   #   $ X��60   $	 Images\open-file.png��6;   
 5        5      $ Large=�6:    . 6L   5        5       -   5        $
 Save��5    #   $ X��60   $	 Images\save.png��6;   
 5!        5"      $ Large=�6:    . 6L   5$      5%        5'      -   5        $ DevGroup��5'      $��	Developer��60   .@�5)        -   5        $ 	CheckCode��5)   #   $ X��65   
 5*        5+      $ Large=�6:    . 6L   5-      5.        50      -   5        $ 	HelpGroup��50      $
��Help��61   .@�52        -   5        $
 Help��52   #   $ X��60   $	 Images\help.png��6;   
 53        54      $ Large=�6:    . 6L   56        58      -	   5        $ About��58   #   $ X��61   $	 Images\about.png��6<   
 59        5:      $ Large=�6:    . 6L   5<      5=      5>      5?      ]� 5@   
    J� IsManipulationEnabled . .+�5A      � $��5q�6    � RowDefinitions 5B      �� 5C      $
��28px �6$   �� 5D      $��* �6$   �� 5E      $
��28px �6$   �� 5F      $��* �6$    � ColumnDefinitions 5H      �� 5I      $��* �6'   �� 5J      $��5 �6'   �� 5K      $	��300 �6'   .+�5M      �� $��0��6   $��0��6%   $
��Code��65   $
��Bold"�6D   $��Center=�6V   �� 5N      $��0��6   $��2��6%   $��	Registers��65   $��Center=�6I   $
��Bold"�6d   �� 5O      $��2��6   $��2��6%   $��Memory��65   $��Center=�6F   $
��Bold"�6a   �� 5Q      -
   5        $��taCode��5Q   !   $��1��6/   $��0��6<   $��3��6L   $��1q�6]   
���   �
���   �6�   &	   System.Windows.Controls.DataGrid	  5R      -   5        $��RegGrid��5R      $��1��6*   $��2��67    	  AutoGenerateColumns .   	  CanUserReorderColumns .   	  CanUserResizeColumns .   	  CanUserResizeRows .   	  CanUserSortColumns .   	  ColumnHeaderHeight$ 30Q�5S   m    	  ClipboardCopyMode$ IncludeHeader=�6�    	  HeadersVisibility$ Column=�6�    	  VerticalGridLinesBrush 5T      �� 5U       �� Color# � ��6*    	  HorizontalGridLinesBrush 5W      �� 5X      # � ��6*    	  Columns 5Z      0
  *System.Windows.Controls.DataGridTextColumn
  5[      ,  &System.Windows.Controls.DataGridColumn   Header$ Register��6w      MinWidth$	 120Q�6�      Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 6�      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn    Binding  6-   �� ! �� Path$! RegisterName�"   ClipboardContentBinding" 6T   Z� 6�   
  5\      $ Value (HEX)��6s   $ * 6�     6-   �� $! HexValue�" 6P   Z� 6�   5^      	  5_      -   5        $��MemGrid��5_      $��3��6*   $��2��67    .   .   .   .   .  $ 30Q�5`   m   $ IncludeHeader=�6�   $ Column=�6�    5a      �� 5b      # � ��6*    5d      �� 5e      # � ��6*    5g      
  5h      $ Address��6r   $	 120Q�6�   $	 120 6�    .   6-   �� $! Address�" 6O   Z� 6�   
  5i      $ Value (HEX)��6s   $ * 6�     6-   �� $! HexValue�" 6P   Z� 6�   5k      5l      5m      5n      5o      