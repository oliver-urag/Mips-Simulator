   M S B A M L   `   `   `  ����  IDE    IDE.SimulationWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   ` \ICSharpCode.AvalonEdit, Version=4.3.2.9632, Culture=neutral, PublicKeyToken=9cc39be672370310?
avalonEdit.http://icsharpcode.net/sharpdevelop/avalonedit  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  MIPS Simulator��5   	   
�������$
��1000��67   $	��800��6D    � 
ResizeMode$ CanMinimize=�6Q   .��5      � $��5q�6    � RowDefinitions 5	      �� 5
      $
��28px �6$   �� 5      $��* �6$   �� 5      $
��28px �6$   �� 5      $��* �6$    � ColumnDefinitions 5      �� 5      $	��300 �6'   �� 5      $��5 �6'   �� 5      $	��300 �6'   �� 5      $��5 �6'   �� 5      $��* �6   .+�5   
   �� $��0��6   $��0��6   $
��Code��6-   $
��Bold"�6<   $��Center=�6N   �� 5   
   $��0��6   $��4��6   $��Pipeline Map��6-   $
��Bold"�6D   $��Center=�6V   �� 5   
   $��2��6   $��0��6   $��	Registers��6-   $��Center=�6A   $
��Bold"�6\   �� 5   
   $��2��6   $��2��6   $��Memory��6-   $��Center=�6>   $
��Bold"�6Y   �� 5   
   $��2��6   $��4��6   $��Clock Cycle Log��6-   $��Center=�6G   $
��Bold"�6b   &   System.Windows.Controls.DataGrid  5   
   -   5        $��InstGrid��5      $��1��6#   $��0��60   $��3��6@      AutoGenerateColumns .     CanUserReorderColumns .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .  	   ColumnHeaderHeight$	 30Q�5   m   
   ClipboardCopyMode$
 IncludeHeader=�6�      HeadersVisibility$ Column=�6�      VerticalGridLinesBrush 5      �� 5       �� Color# � ��6*      HorizontalGridLinesBrush 5       �� 5!      # � ��6*      Columns 5#      0  *System.Windows.Controls.DataGridTextColumn  5$      ,  &System.Windows.Controls.DataGridColumn   Header$ Address��6m      Width5  /System.Windows.Controls.DataGridLengthConverter$ 60 6~      MinWidth$ 60Q�6�   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6%   ��  �� Path$ 
AddressHex�   ClipboardContentBinding 6J   Z� 6�     5%      $ Instruction��6q   $	 200 6�   $	 200Q�6�    6%   �� $ AllInstruction� 6N   Z� 6�     5&      $ Opcode (HEX)��6l   $	 100 6�    6%   �� $ 	OpcodeHex� 6I   Z� 6�     5'      $ Opcode (BIN)��6i   $ * 6    6%   �� $ Opcode� 6F   Z� 6   5)        5*   
   -   5        $��RegGrid��5*      $��3��6"   $��0��6/    .   .   .   .   .  $	 30Q�5+   m   $
 IncludeHeader=�6�   $ Column=�6�    5,      �� 5-      # � ��6"    5/      �� 50      # � ��6"    52        53      $ Address��6j   $	 120Q�6{   $	 120 6�      
IsReadOnly .  6%   �� $ Address� 6G   Z� 6�     54      $ Value (HEX)��6k   $ * 6�    6%   �� $ HexValue� 6H   Z� 6�   56        57   
   -   5        $��MemGrid��57      $��3��6"   $��2��6/    .   .   .   .   .  $	 30Q�58   m   $
 IncludeHeader=�6�   $ Column=�6�    59      �� 5:      # � ��6*    5<      �� 5=      # � ��6*    5?        5@      $ Address��6r   $	 120Q�6�   $	 120 6�    .  6-   �� $ Address� 6O   Z� 6�     5A      $ Value (HEX)��6s   $ * 6�    6-   �� $ HexValue� 6P   Z� 6�   5C      5D      5E      