   M S B A M L   `   `   `  ����  IDE,    &MipSim.IDE.OpcodeGenerationErrorWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   ` \ICSharpCode.AvalonEdit, Version=4.3.2.9632, Culture=neutral, PublicKeyToken=9cc39be672370310?
avalonEdit.http://icsharpcode.net/sharpdevelop/avalonedit  5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$/  )MIPS Simulator - Opcode Generation Errors��5   	   
�������$	��600��6R   $	��400��6^    � 
ResizeMode$ CanMinimize=�6k   .��5      ]� .+�5   
   &   System.Windows.Controls.DataGrid  -   5        $��	ErrorGrid��5      $��10, 10 , 10, 10q�6$      AutoGenerateColumns .     CanUserReorderColumns .     CanUserResizeColumns .     CanUserResizeRows .     CanUserSortColumns .     ColumnHeaderHeight$ 30Q�6�      ClipboardCopyMode$ IncludeHeader=�6�   	   HeadersVisibility$	 Column=�6  
   RowBackground
 �	#FFFBA8A8   
IsReadOnly .    VerticalGridLinesBrush 5	      �� 5
       �� Color# � ��6"      HorizontalGridLinesBrush 5      �� 5      # � ��6      Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Header$
 Line��6s      MinWidth$ 50Q�6�      Width5  /System.Windows.Controls.DataGridLengthConverter$ 50 6�      
IsReadOnly . 1  +System.Windows.Controls.DataGridBoundColumn   Binding 6-   ��  �� Path$ CodeLine�   ClipboardContentBinding 6P   Z� 6�     5      $ Error��6r   $ * 6�    6-   �� $ Message� 6O   Z�    ElementStyle 5      �� 5   "   .�5   &   ��  �� Property � �� �� Value$
 Wrap=�6O   5   #   5        5      $
 Text��6u   $ * 6�    6-   �� $ 
CodeString� 6R   Z� 6�   5      5      5      