   M S B A M L   `   `   `  ����>  :IDE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:MipSim.IDE
MipSim.IDE  5   	       MipSim.IDE.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   ` \ICSharpCode.AvalonEdit, Version=4.3.2.9632, Culture=neutral, PublicKeyToken=9cc39be672370310?
avalonEdit.http://icsharpcode.net/sharpdevelop/avalonedit  5   	   )
validationclr-namespace:MipSim.IDE  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  MIPS Simulator - Editor��5   	   
�������$
��1000��6@   $	��600��6M    � 
ResizeMode$ CanMinimize=�6Z   .��5      ]� .+�5	   
   o� -   5        $��MenuMainWindow��5	      $	��Top=�6%   $��28��6:   $��SemiBold"�6F   ��5
      �� 5       �� EndPoint$	 0,1&�6&    �� 
StartPoint$	 0,0&�65   .��5      �  � Color$ 	#FFEAE8E8��6#   � 5      $ 	#FF00A3EF��6#    � Offset$ 0.944Q�65   � 5      $ 	#FF057087��6#   $ 1Q�65   5      .@�5      m� $��28��6   $��7,3,7,3q�6#   ��5      �� 5      $
��File��6    $��Right=�6,   $��Center=�6H   .@�5      m� -   5        $	��New��5      6(   m� 5      -   5        $
��Open��5      6)   m� 5      -   5        $
��Save��5      6)   5      m� 5      -   5        $��28��5      $��7,3,7,3q�6#   ��5      �� 5      $��Execute��6    $��Right=�6/   $��Center=�6K   5      m� 5      $��28��6   $��7,3,7,3q�6#   ��5      �� 5       $
��Help��6    $��Right=�6,   $��Center=�6H   .@�5"      m� -   5        $
��Help��5"      6)   m� 5#      -   5        $��About��5#      6*   5$      5%      ]� 5&   
    J� IsManipulationEnabled . .+�5'      � $��5q�6    � RowDefinitions 5(      �� 5)      $
��35px �6$   �� 5*      $��* �6$    � ColumnDefinitions 5,      �� 5-      $��* �6'   �� 5.      $��5 �6'   �� 5/      $	��120 �6'   �� 50      $	��180 �6'   �� 51      $��5 �6'   �� 52      $	��120 �6'   �� 53      $	��180 �6'   .+�56      �� $��0��6   $��0��6%   $
��Code��65   $��Center=�6I   $
��Bold"�6d   �� 57      $��0��6   $��2��6%   $��2��65   $��	Registers��6I   $��Center=�6]   $
��Bold"�6x   �� 58      $��0��6   $��5��6%   $��2��65   $��Memory��6I   $��Center=�6]   $
��Bold"�6x   �� 5:      -   5        $��taCode��5:   !   $��1��6/   $��0��6<   $��3��6L   $��1q�6]   
���   �
���   �6�   &   System.Windows.Controls.DataGrid  5<      -	   5        $��RegGrid��5<      $��1��6*   $��2��67   $��2��6G   	   AutoGenerateColumns	 .  
   CanUserReorderColumns
 .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .     ColumnHeaderHeight$ 30Q�6�      ClipboardCopyMode$ IncludeHeader=�6�      HeadersVisibility$ Column=�6!     VerticalGridLinesBrush 5=      �� 5>       �� Color# � ��6*      HorizontalGridLinesBrush 5@      �� 5A      # � ��6*      Columns 5C      0  *System.Windows.Controls.DataGridTextColumn  5D      ,  &System.Windows.Controls.DataGridColumn   Header$ Register��6m      MinWidth$	 120Q�6      Width5  /System.Windows.Controls.DataGridLengthConverter$	 120 6�      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6-   ��  �� Path$ Id�   ClipboardContentBinding 6J   Z� 6�     5E      $ Value (HEX)��6P   $ * 6e    6-   Z�  5F      �� 5G   "   $ HexValue�6*    �� Mode$ TwoWay=�6:    �� NotifyOnValidationError .  �� ValidatesOnExceptions .  �� FallbackValue$ 0��6�     �� ValidationRules  5H   &      MipSim.IDE.HexValidation  5I   *   5K   #   !   EditingElementStyle! 5M      �� 5N   "   " �� 
TargetType"" ��6(   .�5O   &   �� # �� Property# �@��	MaxLength$ �� Value$$ 16��6B   5P   #   5R      5T      �� 5V      -
   5        $��
tbxGotoMem��5V      $��0��6,   $��6��69   $��0px 5px 0px 5pxq�6I   % �� 	MaxLength$% 4��6b   6p     5X      -   5        $��MemGrid��5X      $��1��6*   $��5��67   $��2��6G   	 .  
 .   .   .   .  $ 30Q�5Y   m   $ Column=�5Z       5[      �� 5\      # � ��6*    5^      �� 5_      # � ��6*    5a        5b      $ Address��5c   -   $	 120Q�6>   $	 120 6M    .  5b   -   �� $ Address� 6O   Z� 5c   Y     5d      $ Value (HEX)��6P   $ * 6e    6-   Z�  5e      �� 5f   "   $ HexValue�6*   $ TwoWay=�6:    .  . $ 0��6�     5g   &     5h   *   5j   #   ! 5l      �� 5m   "   "" ��6(   .�5n   &   �� # �@��	MaxLength$$ 2��6B   5o   #   5q      5s      5t      5u      5v      5w      